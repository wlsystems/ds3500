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
        /// Stores the local client name. 
        /// </summary>
        private string localClient;

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

        /// <summary>
        /// True if a game is in session, false once the game has been completed.  
        /// </summary>
        private bool gameActive;


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
            gameActive = false;  //true is game is active, false if it has ended.
            score = 0;
            view.CancelPressed += Cancel;
            view.SubmitPressed += SubmitWord;
            view.DonePressed += Done;
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
        /// Start the game
        /// </summary>
        private void GameStatusStart()
        {
            Task task = new Task(delegate { GameStatus(); });
            task.Start();
        }

        /// <summary>
        /// Get the current status/state of the board.
        /// </summary>
        private async void GameStatus()
        {
            tokenSource2 = new CancellationTokenSource();
            CancellationToken token2 = tokenSource2.Token;
                bool isActive = false;
                dynamic game = new ExpandoObject();
                try
                {
                    game = Sync(game, "games/" + gameToken, 3); //1 is for type post
                }
                finally
                {
                    bool A = false; //sets up the flashing effect
                    while (game.GameState == "pending")
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
                        game = Sync(game, "games/" + gameToken, 3); //1 is for type post
                        
                    }
                    view.CancelJoinEnabled(false);
                    if (game.GameState == "active")
                    {
                        view.SetStatusLabel(true, true);
                        tokenSource3.Cancel();
                        if (isActive == false)
                            view.EnableControls(true);
                        isActive = true;
                        view.SetLabel(game.Board);
                        view.Player1Update(game.Player1.Nickname);
                        view.Player2Update(game.Player2.Nickname);
                        Timer();
                    }
                }
        }

        private async void Timer()
        {
            tokenSource3 = new CancellationTokenSource();
            CancellationToken ct = tokenSource3.Token;

            dynamic game = new ExpandoObject();
            game = Sync(game, "games/" + gameToken, 3);
            while (game.GameState == "active")
            {
                view.UpdateTimer(game.TimeLeft);
                score = (int)game.Player1.Score;
                view.UpdateScore1(score);
                score = (int)game.Player2.Score;
                view.UpdateScore2(score);
                await Task.Delay(1000);
                if (ct.IsCancellationRequested)
                    break;
                game = Sync(game, "games/" + gameToken, 3);
            }
            if (game.GameState == "completed")
            {
                view.SetStatusLabel(false, false);
                IList<object> Player1List;
                Player1List = game.Player1.WordsPlayed;
                List<string> Player1String = new List<string>();
                List<int> Player1Score = new List<int>();
                dynamic WordsPlayed = new ExpandoObject();
                WordsPlayed.Word = "";
                WordsPlayed.Score = 0;
                foreach (object item in Player1List)
                {
                    WordsPlayed = (ExpandoObject)item;
                    Player1String.Add(WordsPlayed.Score.ToString()+ " " + WordsPlayed.Word.ToString());
                    //Player1Score.Add((int)WordsPlayed.Score);

                }
                view.ViewPlayer1Word(Player1String);


                IList<object> Player2List;
                Player2List = game.Player2.WordsPlayed;
                List<string> Player2String = new List<string>();
                List<int> Player2Score = new List<int>();
                dynamic Words2Played = new ExpandoObject();
                Words2Played.Word = "";
                WordsPlayed.Score = 0;
                foreach (object item in Player2List)
                {
                    Words2Played = (ExpandoObject)item;
                    Player2String.Add(Words2Played.Score.ToString()+" " + Words2Played.Word.ToString());
                    //Player2Score.Add((int)Words2Played.Score);

                }
                view.ViewPlayer2Word(Player2String);
            }
        }

        /// <summary>
        /// Uses the token to add the user to a game.  
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private void View_JoinGame(int time)
        {
            tokenSource3 = new CancellationTokenSource();
            try
            {
                dynamic game = new ExpandoObject();
                game.TimeLimit = time;
                game.UserToken = user1Token;
                game.GameID = "";
                game = Sync(game, "games", 1); //1 is for type post
                gameToken = game.GameID;
            }
            finally
            {
                view.UserRegistered = true;
                GameStatus();
            }
        }


        /// <summary>
        /// Cancels the current operation.
        /// </summary>
        private void Cancel(int cancelMode)
        {
            if (cancelMode == 1)
            {
                if (tokenSource != null)
                    tokenSource.Cancel();
            }
                
            else if (cancelMode == 2)
            {
                try
                {
                    dynamic game = new ExpandoObject();
                    game.UserToken = user1Token;
                    game = Sync(game, "games", 2); //2 is for type PUT
                    if (tokenSource2 != null)
                        tokenSource2.Cancel();
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
                if (tokenSource3 != null)
                    tokenSource3.Cancel();
            }
        }

        /// <summary>
        /// Registers a user with the given name and email..
        /// </summary>
        private void Register(string name, string server)
        {
            try
            {
                url = server;
                localClient = name;
                dynamic user = new ExpandoObject();
                user.Nickname = name;
                user.UserToken = "";
                user = Sync(user, "users", 1); //1 is for type POST
                user1Token = user.UserToken;
            }
            catch
            {
                MessageBox.Show("Invalid URL");
            }
            finally
            {
                view.TimeEnabled(true);
                view.UserRegistered = true;
            }
            
        }

        //a general helper method for post requests
        private ExpandoObject Sync(ExpandoObject obj, string Name, int type)//type is 1 for POST, 2 for PUT, 3 for GET
        {
            try
            {
                using (HttpClient client = CreateClient())
                {
                    Uri u = new Uri(url + "/BoggleService.svc/");
                    client.BaseAddress = u;
                    // Compose and send the request.
                    tokenSource = new CancellationTokenSource();
                    StringContent content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = null;
                    if (type == 1)
                        response = client.PostAsync(Name, content, tokenSource.Token).Result;
                    else if (type == 2)
                    {
                        response = client.PutAsync(Name, content, tokenSource.Token).Result;
                    }
                        
                    else if (type == 3)
                        response = client.GetAsync(Name).Result;
                    // Deal with the response
                    if (response.IsSuccessStatusCode)
                    {
                        string result = "";
                        var obj2 = new ExpandoObject();
                        result = response.Content.ReadAsStringAsync().Result;
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
        private void SubmitWord(string wordPlayed)
        {
            view.submitEnableControls(false);
            try
            {
                // Create the parameter
                dynamic WordPlayed = new ExpandoObject();
                dynamic game = new ExpandoObject();
                game = Sync(game, "games/" + gameToken, 3);
                WordPlayed.UserToken = user1Token;
                WordPlayed.Word = wordPlayed;

                // Compose and send the request.
                if (game.GameState == "completed")
                {
                    MessageBox.Show("Sorry that game is over!");
                    return;
                }
                WordPlayed.Score = 0;
                WordPlayed.Score = Sync(WordPlayed, "games/" + gameToken, 2);
            }

            finally
            {
                view.submitEnableControls(true);
            }
        }


        /// <summary>
        /// Ends the current game immediately. 
        /// </summary>
        private void Done()
        {
            ///TODO!!!!
        }


    

        /// <summary>
        /// Creates an HttpClient for communicating with the server.
        /// </summary>
        private static HttpClient CreateClient()
        {
            // Create a client whose base address is the GitHub server
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url); 

            // Tell the server that the client will accept this particular type of response data
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            // There is more client configuration to do, depending on the request.
            return client;
        }
    }
}