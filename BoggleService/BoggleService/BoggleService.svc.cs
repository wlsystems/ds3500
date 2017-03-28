using BoggleList;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Resources;
using System.ServiceModel.Web;
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
        private readonly static Dict dic = new Dict();

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
        public PendingGame GameStatus(string gid, string brief)
        {
            PendingGame pg = new PendingGame();
            return pg;
        }

        public WordScore PlayWord(PlayerWord w, string gid)
        {
            WordScore ws = new WordScore();
            String word = w.Word.Trim();
            if (word == null | gid == null | w.UserToken == null | !users.ContainsKey(w.UserToken) | !games.ContainsKey(gid) | (!games[gid].Player1.Equals(gid) && !games[gid].Player2.Equals(gid)))
            {
                SetStatus(Forbidden);
                return ws;
            }
            else if (games[gid].GameState != "active")
            {
                SetStatus(Conflict);
                return ws;
            }
            if (games[gid].Player2.WordsPlayed.ContainsKey(word) | games[gid].Player2.WordsPlayed.ContainsKey(word))
            {
                ws.WScore = 0;
                return ws;
            }
            BoggleBoard bb = new BoggleBoard(games[gid].Board.ToString());
            if (!bb.CanBeFormed(word.ToUpper()))
            {
                ws.WScore = -1;
                return ws;
            }
            else if (Dict.wordset.Contains(word.ToUpper()))
            {
                switch (word.Length)
                {
                    case 3:
                        ws.WScore = 1;
                        break;
                    case 4:
                        ws.WScore = 1;
                        break;
                    case 5:
                        ws.WScore = 2;
                        break;
                    case 6:
                        ws.WScore = 3;
                        break;
                    case 7:
                        ws.WScore = 5;
                        break;
                    default:
                        ws.WScore = 11;
                        break;
                }
                return ws;
            }
            return ws;
        }
    }
}