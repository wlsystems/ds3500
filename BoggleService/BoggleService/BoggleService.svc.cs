using BoggleList;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
        public Person Register(NewPlayer user)
        {
            Dictionary<string, dynamic> placeholders = new Dictionary<string, dynamic>();
            if (user.Nickname == null || user.Nickname.Trim().Length == 0 || user.Nickname.Trim().Length > 50)
            {
                SetStatus(Forbidden);
                return null;
            }
            dynamic userID = Guid.NewGuid().ToString();
            placeholders.Add("@UserID", userID);
            placeholders.Add("@Nickname", user.Nickname);
            string sql = "insert into Users (UserID, Nickname) values(@UserID, @Nickname)";
            Person p = new Person();
            p.UserToken = userID;
            SetStatus(Created);
            Helper(sql, placeholders, 1);
            return p;
        }

        public Dictionary<string, dynamic>[] Helper(string strCommand, Dictionary<string, dynamic> coms, int type)
        {
            Dictionary<string, dynamic>[] obj = new Dictionary<string, dynamic>[100];
            obj[0] = new Dictionary<string, dynamic>(); //sets it up to work with type == 2.
            using (SqlConnection conn = new SqlConnection(BoggleDB))
            {
                //open connection
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    using (SqlCommand command =
                        new SqlCommand(strCommand,
                                        conn,
                                        trans))
                    {
                        foreach (KeyValuePair<string, dynamic> entry in coms)
                            command.Parameters.AddWithValue(entry.Key, entry.Value);
                        trans.Commit();
                        if (type == 1)
                            command.ExecuteNonQuery();
                        if (type == 2)
                        {
                            obj[0].Add("GameID", command.ExecuteScalar().ToString());
                            return obj;
                        }
                        if (type == 3)
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                // Check to see user is in table, set forbidden if not
                                if (!reader.HasRows)
                                {
                                    reader.Close();
                                    return null;
                                }
                                else
                                {
                                    int j = 0;
                                    obj[j] = new Dictionary<string, dynamic>();
                                    while (reader.Read())
                                    {
                                        for (int i = 0; i < reader.FieldCount; i++)
                                            if (reader.GetValue(i) != null)
                                                obj[j].Add(reader.GetName(i), reader.GetValue(i).ToString());
                                        j = j + 1;
                                    }
                                    return obj;
                                }
                            }
                        }
                    }
                }
                return null;
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
            string cmd = "";
            Dictionary<string, dynamic> placeholders = new Dictionary<string, dynamic>();
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
            // Here, the SqlCommand is a select query.  We are interested in whether item.UserID exists in
            // the Users table.
            cmd = "select UserID from Users where UserID = @UserID";
            placeholders.Add("@UserID", obj.UserToken);
            if (Helper(cmd, placeholders, 3) == null)
            {
                SetStatus(Forbidden);
                return null;
            }
            if (pending.UserToken == null)  //very first request, initializes pending
            {
                pending.UserToken = "";
            }
            if (pending.UserToken == "")
            {
                // Here we are executing an insert command, but notice the "output inserted.ItemID" portion.  
                // We are asking the DB to send back the auto-generated GameID.
                cmd = "insert into Games (Player1) output inserted.GameID values(@Player1)";
                Dictionary<string, dynamic>[] obj2 = new Dictionary<string, dynamic>[1];
                placeholders.Clear();
                placeholders.Add("@Player1", obj.UserToken);
                // We execute the command with the ExecuteScalar method, which will return to
                // us the requested auto-generated ItemID.
                obj2 = Helper(cmd, placeholders, 2);
                pending.TimeLimit = obj.TimeLimit;
                SetStatus(Accepted);
                ng.GameID = obj2[0]["GameID"];
                pending.GameID = Int32.Parse(obj2[0]["GameID"]);
                pending.UserToken = obj.UserToken;
            }
            else
            {
                string board = new BoggleBoard().ToString();
                cmd = "update Games set Player2= @Player2, TimeLimit=@TimeLimit, StartTime=@startTime, Board=@Board, Player1Score=@Player1Score, Player2Score=@Player2Score where GameID=@GameID";
                placeholders.Clear();
                int time = (pending.TimeLimit + obj.TimeLimit) / 2;
                int startTime = (int)DateTime.Now.TimeOfDay.TotalSeconds;
                int score = 0;
                placeholders.Add("@Player2", obj.UserToken);
                placeholders.Add("@GameID", pending.GameID);
                placeholders.Add("@TimeLimit", time);
                placeholders.Add("@StartTime", startTime);
                placeholders.Add("@Board", board);
                placeholders.Add("@Player1Score", score);
                placeholders.Add("@Player2Score", score);
                Helper(cmd, placeholders, 1);
                SetStatus(Created);
                ng.GameID = pending.GameID.ToString();
                pending.UserToken = "";
                pending.GameID = 0;
            }
            return ng;
        }
        /// <summary>
        ///  Takes in a user token.  If userToken is invalid or user is not in the pending game returns a status of Forbidden. If user
        ///  in the pending game, they are removed and returns a status response of OK. 
        /// </summary>
        public void CancelJoin(Person cancelobj)
        {
            if ((cancelobj.UserToken == null) | (pending.UserToken != cancelobj.UserToken))
            {
                SetStatus(Forbidden);      //the userToken was null, the user is not registered or they are not in the pending game
                return;
            }
            string sql = "delete from Games where GameID = @GameID";
            Dictionary<string, dynamic> placeholders = new Dictionary<string, dynamic>();
            placeholders.Add("@GameID", pending.GameID);
            Helper(sql, placeholders, 1);
            pending.UserToken = "";
            pending.TimeLimit = 0;
            SetStatus(OK);
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
            GameID = GameID.Trim(' ');
            if (GameID == null || GameID == "")  //this is checking for null or empty gameIDs
            {
                SetStatus(Forbidden);
                return null;
            }
            string jsonClient = null;

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

            string sql = "select * from Games where CAST (GameID as nvarchar(50)) = @GameID";
            Dictionary<string, dynamic> d = new Dictionary<string, dynamic>();
            d.Add("@GameID", GameID);
            Dictionary<string, dynamic>[] obj2 = new Dictionary<string, dynamic>[100];
            obj2 = Helper(sql, d, 3);
            int timeLeft = 0;
            if (obj2 == null)    //the obj is empty so GameID is not in the the table
            {
                SetStatus(Forbidden);
                return null;
            }
            else
            {
                timeLeft = SetTime(Int32.Parse(obj2[0]["TimeLimit"]), Int32.Parse(obj2[0]["StartTime"]));
            }

            if (Brief == "yes")                            //either active or completed game, with brief as a parameter
            {
                ActiveGameBrief agb = new ActiveGameBrief();
                Player p1 = new Player();
                Player p2 = new Player();
                agb.TimeLeft = timeLeft;
                if (timeLeft > 0)
                {
                    agb.GameState = "active";
                }
                else
                    agb.GameState = "completed";
                p1.Score = obj2[0]["Player1Score"];
                p2.Score = obj2[0]["Player2Score"];
                agb.Player1 = p1;
                agb.Player2 = p2;
                SetStatus(OK);
                jsonClient = JsonConvert.SerializeObject(agb);
            }

            //         else if (t <= 0)
            //         {
            //             GameCompleted gc = new GameCompleted();     //game state is completed and not brief, returns the full game item
            //             gc.GameState = "completed";
            //             gc.TimeLeft = 0;
            //         using (SqlConnection conn = new SqlConnection(BoggleDB))
            //         {
            //             conn.Open();
            //             using (SqlTransaction trans = conn.BeginTransaction())
            //             {

            //                 using (SqlCommand command = new SqlCommand("select * from Games where GameID = @GameID", conn, trans))
            //                 {
            //                     command.Parameters.AddWithValue("@GameID", GameID);
            //                     using (SqlDataReader reader = command.ExecuteReader())
            //                     {
            //                         gc.Board = reader["Board"].ToString();
            //                         gc.TimeLimit= int.Parse(reader["TimeLimit"].ToString());

            //                     }

            //                 }
            //             }
            //         }
            //             gc.Player1 = games[GameID].Player1;
            //             gc.Player2 = games[GameID].Player2;
            //             SetStatus(OK);
            //             jsonClient = JsonConvert.SerializeObject(gc);
            //         }


            else      //game state is active and not brief
            {
                ActiveGame ag = new ActiveGame();
                //ag.TimeLeft = SetTime(GameID);
                Player p1 = new Player();
                Player p2 = new Player();
                string player1;
                string player2;

                sql = "select * from Games where CAST (GameID as nvarchar(50)) = @GameID";
                d = new Dictionary<string, dynamic>();
                d.Add("@GameID", GameID); obj2 = new Dictionary<string, dynamic>[100];
                obj2 = Helper(sql, d, 3);
                timeLeft = 0;
                if (obj2 == null)    //the obj is empty so GameID is not in the the table
                {
                    SetStatus(Forbidden);
                    return null;
                }
                else
                {
                    timeLeft = SetTime(Int32.Parse(obj2[0]["TimeLimit"]), Int32.Parse(obj2[0]["StartTime"]));
                    sql = "select Word, Player,Score from Words where CAST (GameID as nvarchar(50)) = @GameID";
                    d.Clear();

                    if (timeLeft >= 0)
                    {
                        string gs = "active";
                        jsonClient = JsonConvert.SerializeObject(ag);
                    }
                }




                using (SqlConnection conn = new SqlConnection(BoggleDB))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {

                        using (SqlCommand command = new SqlCommand("select * from Users where UserID = @UserID", conn, trans))
                        {
                            //command.Parameters.AddWithValue("@UserID", player1);
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                p1.Score = int.Parse(reader["Score"].ToString());
                                p1.Nickname = reader["Nickname"].ToString();
                                reader.Close();
                                trans.Commit();
                            }

                        }
                    }
                }
                using (SqlConnection conn = new SqlConnection(BoggleDB))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {

                        using (SqlCommand command = new SqlCommand("select * from Users where UserID = @UserID", conn, trans))
                        {
                            // command.Parameters.AddWithValue("@UserID", player2);
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                p2.Score = int.Parse(reader["Score"].ToString());
                                p2.Nickname = reader["Nickname"].ToString();
                                reader.Close();
                                trans.Commit();
                            }

                        }
                    }
                }
                //ag.Player1 = p1;
                //ag.Player2 = p2;
                jsonClient = JsonConvert.SerializeObject(ag);
            }
            //serializes which ever game was pulled and returns a stream
            SetStatus(OK);
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            return new MemoryStream(Encoding.UTF8.GetBytes(jsonClient));
        }


        /// <summary>
        /// Calculate and set the time left.
        /// </summary>
        /// <param name="timeLimit"></param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        private int SetTime(int timeLimit, int startTime)
        {
            if (timeLimit - ((int)DateTime.Now.TimeOfDay.TotalSeconds - startTime) > timeLimit)
                return 0;
            else
                return timeLimit - ((int)DateTime.Now.TimeOfDay.TotalSeconds - startTime);
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