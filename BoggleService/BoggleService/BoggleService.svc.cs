using BoggleList;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net;
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
        private readonly static Dictionary<String, UserInfo> users = new Dictionary<String, UserInfo>();
        private readonly static Dictionary<String, GameItem> games = new Dictionary<String, GameItem>();
        private static readonly object sync = new object();
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
                    SetStatus(Created);
                    string userID = Guid.NewGuid().ToString();
                    UserInfo user = new UserInfo();
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

            if (pending == null)
            {
                pending.GameID = 101;
                pending.UserToken = "";
            }

            if (pending.UserToken == "")
            {
                pending.TimeLimit = obj.TimeLimit;
                pending.UserToken = obj.UserToken;
                SetStatus(Accepted);
                ng.GameID = "" + pending.GameID;
            }
            else
            {

                SetStatus(Created);
                ng.GameID = "" + pending.GameID;
                GameItem g = new GameItem();
                g.TimeLimit = pending.TimeLimit + obj.TimeLimit / 2;
                g.Player1 = users[pending.UserToken];
                g.Player2 = users[obj.UserToken];
                g.GameID = short.Parse(ng.GameID);
                g.StartTime = DateTime.Now.TimeOfDay.TotalSeconds;
                g.GameState = "active";
                games.Add(ng.GameID, g);
                pending.UserToken = "";
                pending.TimeLimit = 0;
                pending.GameID = pending.GameID + 1;
                SetStatus(Created);
            }
            return ng;
        }
    }
}