﻿using BoggleList;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Resources;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Web;
using static System.Net.HttpStatusCode;
/// <summary>
/// The Bogglenamespace contains the boggle.svc
/// </summary>
namespace Boggle
{
    /// <summary>
    /// 
    /// </summary>
    public class BoggleService : IBoggleService
    {
        /// <summary>
        /// Keeps track of the currently pending game
        /// </summary>
        private static string BoggleDB;
        private readonly static Pending pending = new Pending();
        private readonly static Dictionary<String, PlayerCompleted> users = new Dictionary<String, PlayerCompleted>();
        private readonly static Dictionary<String, GameItem> games = new Dictionary<String, GameItem>();
        private readonly static object sync = new object();
        private readonly static Dict dic = new Dict();

        static BoggleService()
        {
            BoggleDB = ConfigurationManager.ConnectionStrings["BoggleDB"].ConnectionString;

        }
        /// <summary>
        /// The most recent call to SetStatus determines the response code used when.
        /// an http response is sent..
        /// </summary>
        /// <param name="status"></param>
        private static void SetStatus(HttpStatusCode status)
        {
            WebOperationContext.Current.OutgoingResponse.StatusCode = status;
        }
        /// <summary>
        /// Join a game. 
        ///If UserToken is invalid, TimeLimit< 5, or TimeLimit> 120, responds with status 403 (Forbidden).
        ///Otherwise, if UserToken is already a player in the pending game, responds with status 409 (Conflict). 
        ///Otherwise, if there is already one player in the pending game, adds UserToken as the second player.The pending game becomes active and a new pending game with no players is created.The active game's time limit is the integer average of the time limits requested by the two players. Returns the new active game's GameID(which should be the same as the old pending game's GameID). Responds with status 201 (Created). 
        ///Otherwise, adds UserToken as the first player of the pending game, and the TimeLimit as the pending game's requested time limit. Returns the pending game's GameID. Responds with status 202 (Accepted). 
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        public Person  Register(NewPlayer user)
        {
            if (user.Nickname == null || user.Nickname.Trim().Length == 0 || user.Nickname.Trim().Length > 50)
            {
                SetStatus(Forbidden);
                return null;
            }

            using (SqlConnection conn = new SqlConnection(BoggleDB))
            {
                //open connection
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    using (SqlCommand command =
                        new SqlCommand("insert into Users (UserID, Nickname) values(@UserID, @Nickname)",
                                        conn,
                                        trans))
                    {
                        // We generate the userID to use.
                        string userID = Guid.NewGuid().ToString();

                        // This is where the placeholders are replaced.
                        command.Parameters.AddWithValue("@UserID", userID);
                        command.Parameters.AddWithValue("@Nickname", user.Nickname.Trim());

                        command.ExecuteNonQuery();
                        SetStatus(Created);

                        trans.Commit();
                        Person p = new Person();
                        p.UserToken = userID;
                        return p;
                    }
                }
            }
        }

        /// <summary>
        /// Returns a Stream version of index.html.
        /// </summary>
        /// <returns></returns>
        public Stream API()
        {
            SetStatus(OK);
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
            return File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + "index.html");
        }

        /// <summary>
        /// Join a game. 
        ///If UserToken is invalid, TimeLimit< 5, or TimeLimit> 120, responds with status 403 (Forbidden).
        ///Otherwise, if UserToken is already a player in the pending game, responds with status 409 (Conflict). 
        ///Otherwise, if there is already one player in the pending game, adds UserToken as the second player.The pending game becomes active and a new pending game with no players is created.The active game's time limit is the integer average of the time limits requested by the two players. Returns the new active game's GameID(which should be the same as the old pending game's GameID). Responds with status 201 (Created). 
        ///Otherwise, adds UserToken as the first player of the pending game, and the TimeLimit as the pending game's requested time limit. Returns the pending game's GameID. Responds with status 202 (Accepted). 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public NewGame JoinGame(NewGameRequest obj)
        {
            NewGame ng = new NewGame();
            if (obj.UserToken == null | obj.TimeLimit < 5 | obj.TimeLimit > 120)   //token is null or time is invalid
            {
                SetStatus(Forbidden);
                return null;
            }
            else if (obj.UserToken == pending.UserToken)    //user is already in pending game, so returns status conflict
            {
                SetStatus(Conflict);
                return null;
            }
            using (SqlConnection conn = new SqlConnection(BoggleDB))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    // Here, the SqlCommand is a select query.  We are interested in whether item.UserID exists in
                    // the Users table.
                    using (SqlCommand command = new SqlCommand("select UserID from Users where UserID = @UserID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@UserID", obj.UserToken);

                        // This executes a query (i.e. a select statement).  The result is an
                        // SqlDataReader that you can use to iterate through the rows in the response.
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Check to see user is in table, set forbidden if not
                            if (!reader.HasRows)
                            {
                                SetStatus(Forbidden);
                                reader.Close();
                                trans.Commit();
                                return null;
                            }
                        }

                    }
                    if (pending.UserToken == null)  //very first request, initializes pending
                    {
                        pending.UserToken = "";
                        pending.GameID = 101;
                    }
                    if (pending.UserToken == "")
                    {
                        // Here we are executing an insert command, but notice the "output inserted.ItemID" portion.  
                        // We are asking the DB to send back the auto-generated ItemID.
                        using (SqlCommand command = new SqlCommand("insert into Games (Player1) output inserted.GameID values(@Player1)", conn, trans))
                        {
                            command.Parameters.AddWithValue("@Player1", obj.UserToken);

                            // We execute the command with the ExecuteScalar method, which will return to
                            // us the requested auto-generated ItemID.
                            string gameID = command.ExecuteScalar().ToString();
                            SetStatus(Accepted);
                            ng.GameID = gameID;
                            pending.UserToken = obj.UserToken;
                            trans.Commit();
                        }
                    }
                    else
                    {
                        using (SqlCommand command = new SqlCommand("update into Games set Player2= @Player2, TimeLimit=@TimeLimit, StartTime=@StartTime where GameID=@GameID", conn, trans))
                        {
                            int time = (pending.TimeLimit + obj.TimeLimit) / 2;
                            int startTime = (int) DateTime.Now.TimeOfDay.TotalSeconds;
                            command.Parameters.AddWithValue("@Player2", obj.UserToken);
                            command.Parameters.AddWithValue("@GameID", pending.GameID);
                            command.Parameters.AddWithValue("@TimeLimit", time);
                            command.Parameters.AddWithValue("@StartTime", startTime);
                            command.ExecuteNonQuery();
                            SetStatus(Created);
                            ng.GameID = pending.GameID.ToString();
                            pending.GameID = pending.GameID + 1;
                            pending.UserToken = "";
                            trans.Commit();
                        }
                    }
                }
            }
            return ng;
        }
        /// <summary>
        ///  Takes in a user token.  If userToken is invalid or user is not in the pending game returns a status of Forbidden. If user
        ///  in the pending game, they are removed and returns a status response of OK. 
        /// </summary>
        public void CancelJoin(Person cancelobj)
        {
            lock (sync)
            {
                if ((cancelobj.UserToken == null) | (pending.UserToken != cancelobj.UserToken))
                {
                    SetStatus(Forbidden);      //the userToken was null, the user is not registered or they are not in the pending game
                    return;
                }

                using (SqlConnection conn = new SqlConnection(BoggleDB))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        // Here we're doing a delete command.
                        using (SqlCommand command = new SqlCommand("delete Player1 from Games where GamesID = @GameID", conn, trans))
                        {
                            command.Parameters.AddWithValue("@GameID", pending.GameID);
                            command.ExecuteNonQuery();
                            SetStatus(OK);
                            trans.Commit();
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Get game status information. 
        ///If GameID is invalid, responds with status 403 (Forbidden). 
        ///Otherwise, returns information about the game named by GameID as illustrated below.Note that the information returned depends on whether "Brief=yes" was included as a parameter as well as on the state of the game. Responds with status code 200 (OK). Note: The Board and Words are not case sensitive.
        /// </summary>
        /// <param name="gameobj"></param>
        /// <returns></returns>
        public Stream GameStatus(string GameID, string Brief)
        {
            lock (sync)
            {
                GameID = GameID.Trim(' ');
                if (GameID == null || GameID == "")  //this is checking for null or empty gameIDs
                {
                    SetStatus(Forbidden);
                    return null;
                }
                int t = 0;
                string jsonClient = null;
                if (!games.ContainsKey(GameID))
                    if (pending.GameID.ToString() != GameID)           // game is not in dictionary and not pending
                    {
                        SetStatus(Forbidden);
                        return null;
                    }
                if (pending.GameID.ToString() == GameID)              //the game status requested is for the pending game
                {
                    PendingGame pg = new PendingGame();
                    pg.GameState = "pending";
                    SetStatus(OK);
                    jsonClient = JsonConvert.SerializeObject(pg);
                    WebOperationContext.Current.OutgoingResponse.ContentType =
                        "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(jsonClient));
                }


                if (games.ContainsKey(GameID))            //checks the time left, to see if game is completed;
                    t = SetTime(games[GameID].TimeLimit, games[GameID].StartTime);

                if (Brief == "yes")                            //either active or completed game, with brief as a parameter
                {
                    ActiveGameBrief agb = new ActiveGameBrief();
                    agb.GameState = games[GameID].GameState;
                    if (t <= 0)
                    {
                        agb.GameState = "completed";
                    }
                    agb.TimeLeft = games[GameID].TimeLeft;
                    Player p1 = new Player();
                    Player p2 = new Player();
                    p1.Score = games[GameID].Player1.Score;
                    p2.Score = games[GameID].Player2.Score;
                    agb.Player1 = p1;
                    agb.Player2 = p2;
                    SetStatus(OK);
                    jsonClient = JsonConvert.SerializeObject(agb);
                }
                else if (t <= 0)
                {
                    GameCompleted gc = new GameCompleted();     //game state is completed and not brief, returns the full game item
                    gc.GameState = "completed";
                    games[GameID].GameState = "completed";
                    gc.Board = games[GameID].Board;
                    gc.Player1 = games[GameID].Player1;
                    gc.Player2 = games[GameID].Player2;
                    gc.TimeLimit = games[GameID].TimeLimit;
                    gc.TimeLeft = 0;                            //game is over
                    SetStatus(OK);
                    jsonClient = JsonConvert.SerializeObject(gc);
                }


                else if (games[GameID].GameState == "active")           //game state is active and not brief
                {
                    ActiveGame ag = new ActiveGame();
                    ag.GameState = games[GameID].GameState;
                    ag.Board = games[GameID].Board;
                    ag.TimeLeft = SetTime(games[GameID].TimeLimit, games[GameID].StartTime);
                    ag.TimeLimit = games[GameID].TimeLimit;
                    Player p1 = new Player();
                    Player p2 = new Player();
                    p1.Nickname = games[GameID].Player1.Nickname;
                    p2.Nickname = games[GameID].Player2.Nickname;
                    p1.Score = games[GameID].Player1.Score;
                    p2.Score = games[GameID].Player2.Score;
                    ag.Player1 = p1;
                    ag.Player2 = p2;
                    SetStatus(OK);
                    jsonClient = JsonConvert.SerializeObject(ag);
                }
                //serializes which ever game was pulled and returns a stream
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes(jsonClient));
            }
        }

        /// <summary>
        /// Calculate and set the time left.
        /// </summary>
        /// <param name="timeLimit"></param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        private int SetTime(int timeLimit, int startTime)
        {
            lock (sync)
            {
                if (timeLimit - ((int)DateTime.Now.TimeOfDay.TotalSeconds - startTime) > timeLimit)
                    return 0;
                else
                    return timeLimit - ((int)DateTime.Now.TimeOfDay.TotalSeconds - startTime);
            }
        }
        /// <summary>
        /// Play a word in a game. 
        ///If Word is null or empty when trimmed, or if GameID or UserToken is missing or invalid, or if UserToken is not a player in the game identified by GameID, responds with response code 403 (Forbidden). 
        ///Otherwise, if the game state is anything other than "active", responds with response code 409 (Conflict). 
        ///Otherwise, records the trimmed Word as being played by UserToken in the game identified by GameID.Returns the score for Word in the context of the game(e.g. if Word has been played before the score is zero). Responds with status 200 (OK). Note: The word is not case sensitive.
        /// </summary>
        /// <param name="w"></param>
        /// <param name="gid"></param>
        /// <returns></returns>
        public WordScore PlayWord(PlayerWord w, string gid)
        {
            lock (sync)
            {
                gid = gid.Trim(' ');
                if (gid == null || gid == "")  //this is checking for null or empty gameIDs
                {
                    SetStatus(Forbidden);
                    return null;
                }

                WordScore ws = new WordScore();
                WordsPlayed wpObj = new WordsPlayed();
                String word = w.Word.Trim(' ').ToUpper();
                wpObj.Word = word;
                int player = 3;
                if (games.ContainsKey(gid))
                {
                    if (!users.ContainsKey(w.UserToken))
                    {
                        SetStatus(Forbidden);
                        return null;
                    }
                    if (games[gid].Player1.Nickname.Equals(users[w.UserToken].Nickname))
                        player = 1;
                    else if (games[gid].Player2.Nickname.Equals(users[w.UserToken].Nickname))
                        player = 2;

                }
                else
                {
                    if (pending.GameID.ToString() == gid)
                    {
                        SetStatus(Conflict);
                            return null;
                    }
                    SetStatus(Forbidden);
                    return null;
                }
                GameStatus(gid, "y");
                if (word == null | word == "" | gid == null | w.UserToken == null | !users.ContainsKey(w.UserToken) | !games.ContainsKey(gid) | player == 3)
                {
                    SetStatus(Forbidden);
                    return ws;
                }
                else if (games[gid].GameState != "active")
                {
                    SetStatus(Conflict);
                    return ws;
                }
                if (word.Length <= 2)
                {
                    ws.Score = 0;
                    wpObj.Score = 0;
                    return AddScore(word, gid, ws, player, wpObj);
                }


                BoggleBoard bb = new BoggleBoard(games[gid].Board.ToString());
                if (CheckSetDup(player, gid, word, ws, wpObj, bb) == 0)
                {
                    ws.Score = 0;
                    wpObj.Score = 0;
                    return AddScore(word, gid, ws, player, wpObj);
                }

                if (dic.strings.Contains(word) && bb.CanBeFormed(word))
                {

                    if (word.Length == 3 | word.Length == 4)
                        ws.Score = 1;
                    else if (word.Length == 5)
                        ws.Score = 2;
                    else if (word.Length == 6)
                        ws.Score = 3;
                    else if (word.Length == 7)
                        ws.Score = 5;
                    else if (word.Length > 7)
                        ws.Score = 11;
                    wpObj.Score = ws.Score;
                }
                else
                {
                    ws.Score = -1;
                    wpObj.Score = -1;
                }
                return AddScore(word, gid, ws, player, wpObj);
            }
        }

        public static int CheckSetDup(int player, string gid, string word, WordScore ws, WordsPlayed wpObj, BoggleBoard bb)
        {
            lock (sync)
            {
                IEnumerator<WordsPlayed> iwp;
                iwp = games[gid].Player1.WordsPlayed.GetEnumerator();
                if (player == 2)
                    iwp = games[gid].Player2.WordsPlayed.GetEnumerator();
                while (iwp.MoveNext())
                {
                    if (iwp.Current.Word.Equals(word))
                        return 0;
                }
                    return -100;
            }
        }
        /// <summary>
        /// Add the score of the word to the list of WordsPlayed. 
        /// Update the total score of the player for this game.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="gid"></param>
        /// <param name="ws"></param>
        /// <param name="player"></param>
        /// <param name="wpObj"></param>
        /// <returns></returns>
        public static WordScore AddScore(string word, string gid, WordScore ws, int player, WordsPlayed wpObj)
        {
            lock (sync)
            {
                SetStatus(OK);
                if (player == 1)
                {
                    games[gid].Player1.Score = ws.Score + games[gid].Player1.Score;
                    games[gid].Player1.WordsPlayed.Add(wpObj);
                }

                else if (player == 2)
                {
                    games[gid].Player2.Score = ws.Score + games[gid].Player2.Score;
                    games[gid].Player2.WordsPlayed.Add(wpObj);
                }
                return ws;
            }
        }

    }
}