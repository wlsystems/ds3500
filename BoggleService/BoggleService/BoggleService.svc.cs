using BoggleList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.ServiceModel.Web;
using System.Threading;
using static System.Net.HttpStatusCode;

namespace Boggle
{
    public class BoggleService : IBoggleService
    {
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

        /// <summary>
        /// Takes in a user ID,  adds it to the user dictionary and returns an object containing
        /// the game object. 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Person Register(UserInfo user)
        {
            lock (sync)
            {
                if (user.Nickname == "stall")
                {
                    Thread.Sleep(5000);
                }
                if (user.Nickname == null || user.Nickname.Trim().Length == 0)
                {
                    SetStatus(Forbidden);
                    return null;
                }
                else
                {
                    SetStatus(Created);
                    string userID = Guid.NewGuid().ToString();
                    users.Add(userID, user);
                    Person p = new Person();
                    p.UserToken = userID;
                    SetStatus(Created);
                    return p;
                }
            }
        }

        /// <summary>
        ///   Takes in a user token and a time limit request to join a new game.  Checks to see if time limit is between 5-120,  also checks to see if user exists has a
        ///   current registered user.   If player 1,  sets status as "Accepted" and creates a pending game.  If player 2,  sets status as "Created"
        ///   and the game becomes active and is started.  
        /// </summary>
        /// <param name="UserToken"></param>
        /// <param name="TimeLimit"></param>
        /// <returns></returns>
        public GameInfo JoinGame(string UserToken, int TimeLimit)
        {
            lock (sync)
            {
                if (TimeLimit > 120 || TimeLimit < 5)  //checks for invalid time and set status to forbidden
                {
                    SetStatus(Forbidden);
                    return null;
                }


                if (!users.ContainsKey(UserToken))  //checks to see if userToken is valid, sets status to forbidden
                {
                    SetStatus(Forbidden);
                    return null;
                }

                if (users[UserToken].InGame == true)  // user is already in an active or pending game
                {
                    SetStatus(Conflict);
                    return null;
                }

                GameInfo gID = new GameInfo();
                if (games == null)      // this is the very first game to start, the games dictionary is empty. 
                {
                    SetStatus(Accepted);
                    GameItem g = new GameItem();                //creates a new game item
                    g.Player1 = users[UserToken];           //sets player 1 
                    users[UserToken].InGame = true;
                    g.GameID = 101;
                    g.TimeLimit = TimeLimit;                //sets time limit
                    g.GameState = "pending";                //sets game status to pending since there is only one player
                    games.Add(g.GameID.ToString(), g);         //adds game ID and game item to dictionary
                    gID.GameID = g.GameID.ToString();                 //returns game id as a string;    
                    return gID;
                }

                int index = games.Keys.Count;      //most recenty created game
                string indexStr = index.ToString();
                if ((games != null) && (games[indexStr].GameState == "pending"))    //a pending game exist, this player will be player two
                {
                    SetStatus(Created);                     //sets game status to created 
                    users[UserToken].InGame = true;
                    games[indexStr].Player2 = users[UserToken];  //adds player two
                    int timeAvg = games[indexStr].TimeLimit + TimeLimit / 2;
                    games[indexStr].TimeLimit = timeAvg;           //takes an average of the two requested time limits and averages them 
                    games[indexStr].GameState = "active";          //sets status to active
                    //timerStart(index);   // TODO  will start the timer in game
                    gID.GameID = indexStr;
                    return gID;
                }
                else         //there is no current pending game 
                {
                    SetStatus(Accepted);                          //Status for player 1
                    GameItem g = new GameItem();                //creates a new game item
                    users[UserToken].InGame = true;
                    g.Player1 = users[UserToken];           //sets player 1 
                    g.GameID = index + 1;                 //sets gameID
                    g.TimeLimit = TimeLimit;                //sets time limit
                    g.GameState = "pending";                //sets game status to pending since there is only one player
                    games.Add(g.GameID.ToString(), g);         //adds game ID and game item to dictionary
                    gID.GameID = g.GameID.ToString();                 //returns game id as a string;    
                    return gID;                //returns game id as a string;
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
    }
}