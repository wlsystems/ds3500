﻿using Newtonsoft.Json;
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
        /// The token of the other player, or "0" if waiting for another player to join. 
        /// </summary>
        private string user2Token;

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
        /// A string that is exactly 16 characters long and contents the conent of the board.    
        /// </summary>
        private string gameboardcontent;

        // <summary>
        /// The total game time.    
        /// </summary>
        private int gameTime;

        /// <summary>
        /// For canceling the current operation
        /// </summary>
        private static CancellationTokenSource tokenSource;



        /// <summary>/
        /// Creates a Controller for the provided view
        /// </summary>
        public Controller1(BoggleForm view)
        {
            this.view = view;
            gameToken = "0";
            user1Token = "0";
            user2Token = "0";
            gameActive= false;  //true is game is active, false if it has ended.
            gameboardcontent = null;
            wordList = new List<string>();
            showBothClientsFinalLists = false;
            view.CancelPressed += Cancel;
            view.RegisterPressed += Register;
            view.SubmitPressed += SubmitWord;
            view.DonePressed += Done;
            view.FilterChanged += FilterListVisible;
            view.SetServerURL += Register;
            view.JoinGame += View_JoinGame;
            //view.CancelPressed += Cancel;

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
        /// Get the current status/state of the board.
        /// </summary>
        private void GameStatus()
        {
            try
            {
                dynamic game = new ExpandoObject();
                game = Sync(game, "games/"+gameToken, 3); //1 is for type post
            }
            finally
            {

            }
        }

        /// <summary>
        /// Uses the token to add the user to a game.  
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private void View_JoinGame(int time)
        {
            try
            {
                dynamic game = new ExpandoObject();
                game.TimeLimit = time;
                game.UserToken = user1Token;
                game.GameID = "";
                game = Sync(game, "games",1); //1 is for type post
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
                    //MessageBox.Show(game.toString());
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
                user = Sync(user, "users",1); //1 is for type POST
                user1Token = user.UserToken;
            }
            finally
            {
                view.TimeEnabled(true);
                view.UserRegistered = true;
            }   
        }

        //a general helper method for post requests
        private static ExpandoObject Sync(ExpandoObject obj, string Name, int type)//type is 1 for POST, 2 for PUT, 3 for GET
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
                        response = client.PutAsync(Name, content, tokenSource.Token).Result;
                    else if (type == 3)
                        response = client.GetAsync(Name).Result;
                    // Deal with the response
                    MessageBox.Show(response.ToString());
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
                        MessageBox.Show("Error registering: " + response.StatusCode);
                        MessageBox.Show(response.ReasonPhrase);
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
            try
            {
                view.EnableControls(false);
                using (HttpClient client = CreateClient())
                {
                    // Create the parameter
                    dynamic WordsPlayed = new ExpandoObject();
                    WordsPlayed.UserID = user1Token;
                    WordsPlayed.word= wordPlayed;

                    // Compose and send the request.
                    StringContent content = new StringContent(JsonConvert.SerializeObject(WordsPlayed), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = client.PostAsync("AddWord", content).Result;

                    // Deal with the response
                    if (response.IsSuccessStatusCode)
                    {
                        String result = response.Content.ReadAsStringAsync().Result;
                        MessageBox.Show(result);
                        dynamic wordToken = JsonConvert.DeserializeObject(result);
                    }
                    else
                    {
                        Console.WriteLine("Error submitting: " + response.StatusCode);
                        Console.WriteLine(response.ReasonPhrase);
                    }
                }
                Refresh();
            }
            finally
            {
                view.EnableControls(true);
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
                Refresh();
            }
            finally
            {
                view.EnableControls(true);
            }
        }

        /// <summary>
        /// Refreshes the display because something has changed.
        /// </summary>
        private void Refresh()
        {
            using (HttpClient client = CreateClient())
            {
                // Compose and send the request
                String url;
                if (showBothClientsFinalLists)
                {
                    url = String.Format("GetAllItems?completed={0}");
                }
                else
                {
                    url = String.Format("GetAllItems?completed={0}&user={1}", user1Token);
                }
                HttpResponseMessage response = client.GetAsync(url).Result;

                // Deal with the response
                if (response.IsSuccessStatusCode)
                {
                    String result = response.Content.ReadAsStringAsync().Result;
                    dynamic items = JsonConvert.DeserializeObject(result);
                    view.Clear();
                    wordList.Clear();
                    foreach (dynamic word in wordList)
                    {
                        view.AddWord((string)word.Description, (int) word.score, word.UserID == user1Token);
                        wordList.Add((string)word.WordID);
                    }
                }
                else
                {
                    Console.WriteLine("Error getting items: " + response.StatusCode);
                    Console.WriteLine(response.ReasonPhrase);
                }
            }
        }

        /// <summary>
        /// Creates an HttpClient for communicating with the server.
        /// </summary>
        private static HttpClient CreateClient()
        {
            // Create a client whose base address is the GitHub server
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);   ///TODO NEED TO FIX

            // Tell the server that the client will accept this particular type of response data
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            // There is more client configuration to do, depending on the request.
            return client;
        }
    }
}

