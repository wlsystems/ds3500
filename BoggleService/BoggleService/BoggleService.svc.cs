//TracyKing u0040235

using BoggleList;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Resources;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Web;
using static System.Net.HttpStatusCode;

namespace Boggle
{
    public class BoggleService : IBoggleService
    {
        /// <summary>
        /// Keeps track of the currently pending game
        /// </summary>
        private readonly static Pending pending = new Pending();
        private readonly static Dictionary<String, PlayerCompleted> users = new Dictionary<String, PlayerCompleted>();
        private readonly static Dictionary<String, GameItem> games = new Dictionary<String, GameItem>();
        private readonly static object sync = new object();
        private readonly static Dict dic = new Dict();

        /// <summary>
        /// The most recent call to SetStatus determines the response code used when.
        /// an http response is sent..
        /// </summary>
        /// <param name="status"></param>
        private static void SetStatus(HttpStatusCode status)
        {
            WebOperationContext.Current.OutgoingResponse.StatusCode = status;  
        }
        public Person Register(NewPlayer newUser)
        {
            lock (sync)
            {

                if (newUser.Nickname == null || newUser.Nickname.Trim().Length == 0)
                {
                    SetStatus(Forbidden);
                    return null;
                }
                else
                {
                    string userID = Guid.NewGuid().ToString();
                    PlayerCompleted user = new PlayerCompleted();
                    user.Nickname = newUser.Nickname;
                    users.Add(userID, user);
                    Person p = new Person();
                    p.UserToken = userID;
                    SetStatus(Created);
                    return p;
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


        public NewGame JoinGame(NewGameRequest obj)
        {
            lock (sync)
            {
                NewGame ng = new NewGame();
                if (obj.UserToken == null | obj.TimeLimit < 5 | obj.TimeLimit > 120)
                {
                    SetStatus(Forbidden);
                    return null;
                }
                else if (obj.UserToken == pending.UserToken)
                {
                    SetStatus(Conflict);
                    return null;
                }
                if (pending.UserToken == null)
                {
                    pending.GameID = 101;
                    pending.UserToken = "";
                    dic.strings = new HashSet<string>(File.ReadAllLines(HttpRuntime.AppDomainAppPath + "/dictionary.txt"));
                }

                if (pending.UserToken == "")
                {
                    pending.TimeLimit = obj.TimeLimit;
                    pending.UserToken = obj.UserToken;
                    ng.GameID = "" + pending.GameID;
                    SetStatus(Accepted);
                    ng.GameID = "" + pending.GameID;
                    return ng;
                }
                else
                {
                    ng.GameID = "" + pending.GameID;
                    GameItem g = new GameItem();
                    g.TimeLimit = (pending.TimeLimit + obj.TimeLimit) / 2;
                    g.Player1 = users[pending.UserToken];
                    g.Player2 = users[obj.UserToken];
                    g.Player1.Score = 0;
                    g.Player2.Score = 0;
                    g.Player1.WordsPlayed = new List<WordsPlayed>();
                    g.Player2.WordsPlayed = new List<WordsPlayed>();
                    g.StartTime = (int)DateTime.Now.TimeOfDay.TotalSeconds;
                    g.GameState = "active";
                    g.Board = new BoggleBoard().ToString();
                    games.Add(ng.GameID, g);
                    pending.UserToken = "";
                    pending.TimeLimit = 0;
                    pending.GameID = pending.GameID + 1;
                    SetStatus(Created);
                    return ng;
                }
            }  
        }
        /// <summary>
        ///  Takes in a user token.  If userToken is invalid or user is not in the pending game returns a status of Forbidden. If user
        ///  in the pending game, they are removed and returns a status response of OK. 
        /// </summary>
        public void CancelJoin(Person cancelobj)
        {
            lock (sync)
            {
                if ((cancelobj.UserToken == null) || !(users.ContainsKey(cancelobj.UserToken)) | (pending.UserToken != cancelobj.UserToken))
                {
                    SetStatus(Forbidden);
                    return;
                }

                if (pending.UserToken == cancelobj.UserToken)
                {
                    pending.UserToken = "";
                    pending.TimeLimit = 0;
                    SetStatus(OK);
                    return;
                }
            } 
        }
        /// <summary>
        /// Returns the status of the game. 
        /// </summary>
        /// <param name="gameobj"></param>
        /// <returns></returns>
        public Stream GameStatus( string GameID, string Brief)
        {
            lock (sync)
            {
                int t = 0;
                if (!games.ContainsKey(GameID))
                    if (pending.GameID.ToString() != GameID)           // game is not in dictionary and not pending
                    {
                        SetStatus(Forbidden);
                        return null;
                    }
                if (pending.GameID.ToString() == GameID)              //pendidng status for player 1 while waiting
                {
                    PendingGame pg = new PendingGame();
                    pg.GameState = "pending";
                    SetStatus(OK);
                    string jsonClient = JsonConvert.SerializeObject(pg);
                    WebOperationContext.Current.OutgoingResponse.ContentType =
                        "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(jsonClient));
                }
                if (games.ContainsKey(GameID))
                    t = SetTime(games[GameID].TimeLimit, games[GameID].StartTime); //get the time left
                if (t <= 0)
                {
                    GameCompleted gc = new GameCompleted();     //game state is completed and not brief, returns gameitem minus start time
                    gc.GameState = "completed";
                    gc.Board = games[GameID].Board;
                    gc.Player1 = games[GameID].Player1;
                    gc.Player2 = games[GameID].Player2;
                    gc.TimeLimit = games[GameID].TimeLimit;
                    gc.TimeLeft = 0;
                    SetStatus(OK);
                    string jsonClient = JsonConvert.SerializeObject(gc);
                    WebOperationContext.Current.OutgoingResponse.ContentType =
                        "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(jsonClient));
                }

                else if (Brief == "yes")                            //either active or completed game, with brief as a parameter
                {
                    ActiveGameBrief agb = new ActiveGameBrief();
                    agb.GameState = games[GameID].GameState;
                    agb.TimeLeft = games[GameID].TimeLeft;
                    Player p1 = new Player();
                    Player p2 = new Player();
                    p1.Score = games[GameID].Player1.Score;
                    p2.Score = games[GameID].Player2.Score;
                    agb.Player1 = p1;
                    agb.Player2 = p2;
                    SetStatus(OK);
                    string jsonClient = JsonConvert.SerializeObject(agb);
                    WebOperationContext.Current.OutgoingResponse.ContentType =
                        "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(jsonClient));
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
                    string jsonClient = JsonConvert.SerializeObject(ag);
                    WebOperationContext.Current.OutgoingResponse.ContentType =
                        "application/json; charset=utf-8";
                    return new MemoryStream(Encoding.UTF8.GetBytes(jsonClient));
                }
                return null;
            }
        }

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

        public WordScore PlayWord(PlayerWord w, string gid)
        {
            lock (sync)
            {
                WordScore ws = new WordScore();
                WordsPlayed wpObj = new WordsPlayed();
                String word = w.Word.Trim(' ').ToUpper();
                wpObj.Word = word;
                int player = 3;
                if (games.ContainsKey(gid))
                {
                    if (games[gid].Player1.Nickname.Equals(users[w.UserToken].Nickname))
                        player = 1;
                    else if (games[gid].Player2.Nickname.Equals(users[w.UserToken].Nickname))
                        player = 2;
                }
                else
                {
                    SetStatus(Forbidden);
                    return null;
                }
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
                BoggleBoard bb = new BoggleBoard(games[gid].Board.ToString());
                if (!bb.CanBeFormed(word))
                {
                    ws.WScore = -1;
                    wpObj.Score = -1;
                    return AddScore(word, gid, ws, player, wpObj);
                }
                else if (dic.strings.Contains(word))
                {
                    if (word.Length == 3 | word.Length == 4)
                        ws.WScore = 1;
                    else if (word.Length == 5)
                        ws.WScore = 2;
                    else if (word.Length == 6)
                        ws.WScore = 3;
                    else if (word.Length == 7)
                        ws.WScore = 5;
                    else
                        ws.WScore = 11;
                    wpObj.Score = ws.WScore;
                    WordsPlayed y = new WordsPlayed();
                    y.Word = word;
                    y.Score = 0;
                    if (player == 1)
                        if (games[gid].Player1.WordsPlayed.Contains(wpObj) | games[gid].Player1.WordsPlayed.Contains(y) )
                            ws.WScore = 0;
                    if (player == 2)
                        if (games[gid].Player2.WordsPlayed.Contains(wpObj) | games[gid].Player2.WordsPlayed.Contains(y))
                            ws.WScore = 0;
                }
                else //havent tested this last else yet so comment out if it cause prob
                    ws.WScore = -1;
                return AddScore(word, gid, ws, player, wpObj);
            }
        }
        public static WordScore AddScore(string word, string gid, WordScore ws, int player, WordsPlayed wpObj)
        {
            lock (sync)
            {
                if (player == 1)
                {
                    games[gid].Player1.Score = ws.WScore + games[gid].Player1.Score;
                    games[gid].Player1.WordsPlayed.Add(wpObj);
                }

                else if (player == 2)
                {
                    games[gid].Player2.Score = ws.WScore + games[gid].Player2.Score;
                    games[gid].Player2.WordsPlayed.Add(wpObj);
                }
                return ws;
            }
        }
    }
}