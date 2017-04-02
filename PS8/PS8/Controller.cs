//Tracy King u0040235

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace PS8
{
    /// <summary>
    /// Controller for the Boggle Client
    /// </summary>
    class Controller1
    {

        /// <summary>
        /// Stores the url;
        /// </summary>
        private static string url;

        /// <summary>
        /// The view controlled by this Controller
        /// </summary>
        private BoggleForm view;

        /// <summary>
        /// The token of the local user who is registered, or "0" if not registered yet. 
        /// </summary>
        private string user1Token;


        /// <summary>
        /// The current game token, or "0" if no current game exist. 
        /// </summary>
        private string gameToken;


        // <summary>
        /// The current score provided by the game    
        /// </summary>
        private int score;

        /// <summary>
        /// For canceling the current operation
        /// </summary>
        private static CancellationTokenSource tokenSource;

        /// <summary>
        /// For canceling the current operation
        /// </summary>
        private static CancellationTokenSource tokenSource2;

        /// <summary>
        /// For canceling the join button operation.
        /// </summary>
        private static CancellationTokenSource tokenSource3;

        /// <summary>/
        /// Creates a Controller for the provided view
        /// </summary>
        public Controller1(BoggleForm view)
        {
            this.view = view;
            gameToken = "0";
            user1Token = "0";
            score = 0;
            view.CancelPressed += Cancel;
            view.SubmitPressed += SubmitWord;
            view.SetServerURL += Register;
            view.JoinGame += View_JoinGame;
        }

        /// <summary>
        /// Handles the cancel button and prevents multiple firings.
        /// </summary>
        public event EventHandler CancelPressed
        {
            add
            {
                CancelPressed -= value;
                CancelPressed += value;
            }
            remove
            {
                CancelPressed -= value;
            }
        }


        /// <summary>
        /// Handles the game while it is pending,  checks for canellation requests while it is pending, once the game status has
        /// changed to active, sets up the board and calls the timer.  
        /// </summary>
        private async void GameStatus()
        {
            tokenSource2 = new CancellationTokenSource();
            CancellationToken token2 = tokenSource2.Token;
            bool isActive = false;
            dynamic game = new ExpandoObject();
            try
            {
                Task<ExpandoObject> t = await Task<ExpandoObject>.Run(() => Sync(game, "games/" + gameToken, 3));
                game = await t;
            }
            finally
            {
                bool A = false; //sets up the flashing effect
                while (game.GameState == "pending")                 //game is pending, checks again in 1000 ms and checks for cancellation request
                {
                    A = !A;
                    if (A == true)
                        view.SetStatusLabel(true, false);
                    else
                        view.SetStatusLabel(false, true);
                    await Task.Delay(1000);
                    if (token2.IsCancellationRequested)
                    {
                        tokenSource3.Cancel();
                        break;
                    }
                    Task<ExpandoObject> t = await Task<ExpandoObject>.Run(() => Sync(game, "games/" + gameToken, 3));
                    game = await t;
                }
                view.CancelJoinEnabled(false);
                if (game.GameState == "active")
                {
                    view.SetStatusLabel(true, true);
                    tokenSource3.Cancel();
                    if (isActive == false)
                        view.EnableControls(true);
                    isActive = true;
                    view.SetLabel(game.Board);                      //this is the helper method that sets the game board. 
                    view.Player1Update(game.Player1.Nickname);      //sets the labels for player 1 and 2 
                    view.Player2Update(game.Player2.Nickname);
                    Timer();                                        //starts the timer and handles the remainder of the game. 
                }
            }
        }

        /// <summary>
        /// HAndles the game while it is active and finishs the game when the time expires after the status changes to "completed."
        /// </summary>
        private async void   Timer()
        {
            tokenSource3 = new CancellationTokenSource();
            CancellationToken ct = tokenSource3.Token;
            dynamic game = new ExpandoObject();
            Task<ExpandoObject> t = await Task<ExpandoObject>.Run(() => Sync(game, "games/" + gameToken, 3));
            game = await t;
            while (game.GameState == "active")
            {
                view.UpdateTimer(game.TimeLeft);            //updates score for both players every 500 ms
                score = (int)game.Player1.Score;
                view.UpdateScore1(score);
                score = (int)game.Player2.Score;
                view.UpdateScore2(score);
                await Task.Delay(500);
                if (ct.IsCancellationRequested)            //checks for cancellation request
                    break;
                t = await Task<ExpandoObject>.Run(() => Sync(game, "games/" + gameToken, 3));
                game = await t;//rechecks game status before looping again

            }
            if (game.GameState == "completed")          //game is now completed
            {
                view.SetJoinText("Play Again");
                view.SetStatusLabel(false, false);
                /// Pulls the list of objects for WordsPlayed for player 1, takes the words and scores and 
                /// sets them in a string list. 
                IList<object> Player1List;
                Player1List = game.Player1.WordsPlayed;   //Pulls list from server. 
                List<string> Player1String = new List<string>(); //List that the WordPlayed object will be placed into. 
                dynamic WordsPlayed = new ExpandoObject();
                WordsPlayed.Word = "";             //Initilizes the Word default
                WordsPlayed.Score = 0;             //Initilizes the score default
                if (Player1List != null)
                    foreach (object item in Player1List)            //Iterates though each Wordplayed object in the list. 
                    {
                        WordsPlayed = (ExpandoObject)item;        //Casts it as an Expando object. 
                        Player1String.Add(WordsPlayed.Score.ToString() + " " + WordsPlayed.Word.ToString());  //Adds word and score to string list
                    }
                view.ViewPlayer1Word(Player1String);                //Calls method that will set the text panel with the words. 

                //Repeats the above actions for player #2,  gets Wordplayeds and converts the words and scores
                //and puts them in a string list.  
                IList<object> Player2List;
                Player2List = game.Player2.WordsPlayed;    //List of WordPlayed object for Player 2 
                List<string> Player2String = new List<string>();
                dynamic Words2Played = new ExpandoObject();
                Words2Played.Word = "";                 //sets Word default
                WordsPlayed.Score = 0;                  //sets Score default
                if (Player2List != null)
                    foreach (object item in Player2List)    //iterates though each object in the WordPlayed object list.  
                    {
                        Words2Played = (ExpandoObject)item;
                        Player2String.Add(Words2Played.Score.ToString() + " " + Words2Played.Word.ToString());
                    }
                view.ViewPlayer2Word(Player2String);
                view.JoinEnabled(true);                 //Game is officially completed, enables player to join again.  
            }
        }

        /// <summary>
        ///  Takes the time and calls the helper method to post a new game.  
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private async void View_JoinGame(int time)
        {
            tokenSource3 = new CancellationTokenSource();
            try
            {
                dynamic game = new ExpandoObject();
                game.TimeLimit = time;
                game.UserToken = user1Token;
                game.GameID = "";
                Task<ExpandoObject> t = await Task<ExpandoObject>.Run(() => Sync(game, "games", 1));
                game = await t;
                gameToken = game.GameID;
            }
            finally
            {
                view.SetJoinText("Join Game");
                view.UserRegistered = true;
                GameStatus();
            }
        }


        /// <summary>
        /// Cancels the current operation.
        /// </summary>
        private async void Cancel(int cancelMode)
        {
            if (cancelMode == 1)
            {
                try
                {
                    if (tokenSource != null)
                        tokenSource.Cancel();
                }
                catch
                {
                    MessageBox.Show("Invalid URL");
                }
            }

            else if (cancelMode == 2)
            {
                try
                {
                    dynamic game = new ExpandoObject();
                    game.UserToken = user1Token;
                    Task<ExpandoObject> t = await Task<ExpandoObject>.Run(() => Sync(game, "games", 2));
                    game = await t;
                    if (tokenSource2 != null)
                        tokenSource2.Cancel();
                }
                catch
                {
                    MessageBox.Show("Invalid URL");
                }
                finally
                {
                    view.Clear();
                    view.JoinEnabled(true);
                    view.CancelJoinEnabled(false);
                }
            }
            else if (cancelMode == 3)
            {
                try
                {
                    if (tokenSource3 != null)
                        tokenSource3.Cancel();
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// Registers a user with the given name and requested server.  
        /// </summary>
        private async void Register(string name, string server)
        {
            await Task.Delay(500);
            try
            {
                url = server;
                dynamic user = new ExpandoObject();
                user.Nickname = name;
                user.UserToken = "";
                Task<ExpandoObject> t = await Task<ExpandoObject>.Run(() => Sync(user, "users", 1));
                user = await t;
                user1Token = user.UserToken;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());     //Unable to sucessfully register the user
            }
            finally
            {
                view.TimeEnabled(true);
                view.UserRegistered = true;
            }

        }

        /// <summary>
        /// This is our main helper method that interacts with the server,  it is called several times in our program. It takes in an ExpandoObject, a name location on the server
        /// and also takes an int for one of 3 type of actions, 1 for POST,  2 for PUT and 3 for GET.  
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="Name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private async Task<ExpandoObject> Sync(ExpandoObject obj, string Name, int type)
        {
            try
            {
                using (HttpClient client = CreateClient())
                {
                    Uri u = new Uri(url + "/BoggleService.svc/");
                    client.BaseAddress = u;
                    // Compose and send the request..
                    tokenSource = new CancellationTokenSource();
                    StringContent content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = null;
                    if (type == 1)
                    {
                        response = await Task.Run(() => client.PostAsync(Name, content, tokenSource.Token).Result);   //POST
                        MessageBox.Show(response.StatusCode.ToString());
                    }
                    else if (type == 2)
                    {
                        response = await Task.Run(() => client.PutAsync(Name, content, tokenSource.Token).Result);  //PUT
                    }
                    else if (type == 3)                                                         //GET
                        response = await Task.Run(() => client.GetAsync(Name).Result);
                    dynamic obj2 = null;

                    if (response.IsSuccessStatusCode)     // Deal with the response, checks for success status 
                    {
                        string result = "";
                        result = response.Content.ReadAsStringAsync().Result;
                        //MessageBox.Show(result);
                        if (result != "")
                            obj2 = JsonConvert.DeserializeObject<ExpandoObject>(result, new ExpandoObjectConverter());
                        return obj2;
                    }
                    else
                    {
                        //MessageBox.Show("Error registering: " + response.StatusCode);
                        //MessageBox.Show(response.ReasonPhrase);
                        return null;
                    }
                }
            }
            catch (TaskCanceledException)
            {
                return null;
            }
        }

        /// <summary>
        /// Submits a word during the game
        /// </summary>
        private async void SubmitWord(string wordPlayed)
        {
            view.submitEnableControls(false);                           //While a word is being submitted, temporarily disables controls
            try
            {
                dynamic WordPlayed = new ExpandoObject();                  //Creates new WordPlayed object
                dynamic game = new ExpandoObject();
                Task<ExpandoObject> t = await Task<ExpandoObject>.Run(() => Sync(game, "games/" + gameToken, 3));
                game = await t; //pulls game status to ensure it is still active before playing word
                WordPlayed.UserToken = user1Token;                        //sets user token
                WordPlayed.Word = wordPlayed;                             //sets word
                if (game.GameState == "completed")                      //If time has run out, pop ups an error message
                {
                    MessageBox.Show("Sorry that game is over!");
                    return;
                }
                WordPlayed.Score = 0;
                t = await Task<ExpandoObject>.Run(() => Sync(WordPlayed, "games/" + gameToken, 2));
                WordPlayed.Score = await t;      //put the Word and the score is returned 
            }

            finally
            {
                view.submitEnableControls(true);                    //Finished submitting word, reenables controls on board. 
            }
        }


        /// <summary>
        /// Creates an HttpClient for communicating with the server.
        /// </summary>
        private static HttpClient CreateClient()
        {
            // Create a client whose base address is the one provided by the user.  
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);

            // Tell the server that the client will accept this particular type of response data
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }
    }
}
