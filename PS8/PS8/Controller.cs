using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
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

        /// <summary>
        /// True if a game is in session, false once the game has been completed.  
        /// </summary>
        private bool gameActive;

        /// <summary>
        /// List of all the words submitted by the user, in the order they were
        /// submitted
        /// </summary>
        private IList<string> wordList;

        /// <summary>
        /// True if both list of words should be shown, once the game is over, when false
        /// will only show the local client's list.  
        /// </summary>
        private bool showBothClientsFinalLists;

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
            wordList = new List<string>();
            showBothClientsFinalLists = false;
            view.CancelPressed += Cancel;
            view.RegisterPressed += Register;
            view.SubmitPressed += SubmitWord;
            view.DonePressed += Done;
            view.FilterChanged += FilterListVisible;
            view.SetServerURL += Register;
            view.JoinGame += View_JoinGame;

        }
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
                    while (game.GameState == "pending")
                    {
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
                tokenSource.Cancel();
            else if (cancelMode == 2)
            {
                try
                {
                    dynamic game = new ExpandoObject();
                    game.UserToken = user1Token;
                    game = Sync(game, "games", 2); //2 is for type PUT
                    tokenSource2.Cancel();
                }
                finally
                {
                    view.JoinEnabled(true);
                    view.CancelJoinEnabled(false);
                }
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
                view.AddWord(WordPlayed.Word);
                wordList.Add(wordPlayed);
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
        /// Changes the state of the filter that control what is to be displayed.
        /// </summary>
        private void FilterListVisible(bool showBothClientsFinalLists)
        {
            try
            {
                view.EnableControls(false);
                this.showBothClientsFinalLists = showBothClientsFinalLists;
            }
            finally
            {
                view.EnableControls(true);
            }
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