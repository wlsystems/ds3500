using BoggleList;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
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
        /// <summary>
        /// The most recent call to SetStatus determines the response code used when
        /// an http response is sent.
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
                if (newUser.Nickname == "stall")
                {
                    Thread.Sleep(5000);
                }
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

        /// <summary>
        /// Demo.  You can delete this.
        /// </summary>
        public string WordAtIndex(int n)
        {
            if (n < 0)
            {
                SetStatus(Forbidden);
                return null;
            }

            string line;
            using (StreamReader file = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "dictionary.txt"))
            {
                while ((line = file.ReadLine()) != null)
                {
                    if (n == 0) break;
                    n--;
                }
            }

            if (n == 0)
            {
                SetStatus(OK);
                return line;
            }
            else
            {
                SetStatus(Forbidden);
                return null;
            }
        }

        public NewGame JoinGame(NewGameRequest obj)
        {
            NewGame ng = new NewGame();
            if (obj.UserToken == null | obj.TimeLimit < 5 | obj.TimeLimit > 120)
                SetStatus(Forbidden);
            else if (obj.UserToken == pending.UserToken)
                SetStatus(Conflict);
            if (pending.UserToken == null)
            {
                pending.GameID = 101;
                pending.UserToken = "";
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
                g.TimeLimit = pending.TimeLimit + obj.TimeLimit / 2;
                g.Player1 = users[pending.UserToken];
                g.Player2 = users[obj.UserToken];
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
        /// <summary>
        ///  Takes in a user token.  If userToken is invalid or user is not in the pending game returns a status of Forbidden. If user
        ///  in the pending game, they are removed and returns a status response of OK. 
        /// </summary>
        public void CancelJoin(Person cancelobj)
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
        /// <summary>
        /// Returns the status of the game. 
        /// </summary>
        /// <param name="gameobj"></param>
        /// <returns></returns>
        public Stream GameStatus( string GameID, string Brief)
        {
            if (!games.ContainsKey(GameID))
                if (pending.GameID.ToString() != GameID)           // game is not in dictionary and not pending
                {
                SetStatus(Forbidden);
                return null;
                }
            if (pending.GameID.ToString() == GameID)              //penidng status for player 1 while waiting
            {
                ActiveGame pg = new ActiveGame();
                pg.GameState = "pending";
                SetStatus(OK);
                string jsonClient = JsonConvert.SerializeObject(pg);
                WebOperationContext.Current.OutgoingResponse.ContentType =
                    "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes(jsonClient));
            }
            else if (Brief == "yes")                            //either active or completed game, with brief as a parameter
            {
                ActiveGameBrief agb = new ActiveGameBrief();
                agb.GameState = games[GameID].GameState;
                agb.TimeLeft = games[GameID].TimeLeft;
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
                ag.TimeLeft = games[GameID].TimeLeft;
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
            else 
            {
                GameCompleted gc = new GameCompleted();     //game state is completed and not brief, returns gameitem minus start time
                gc = games[GameID];
                SetStatus(OK);
                string jsonClient = JsonConvert.SerializeObject(gc);
                WebOperationContext.Current.OutgoingResponse.ContentType =
                    "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes(jsonClient));
            }
        }
    }
}