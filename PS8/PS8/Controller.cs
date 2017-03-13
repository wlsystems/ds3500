using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;

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

        /// <summary>
        /// A string that is exactly 16 characters long and contents the conent of the board.    
        /// </summary>
        private string gameboardcontent;

        /// <summary>
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
            view.SetServerURL += View_SetServerURL;
        }

        private void View_SetServerURL(string obj)
        {
            MessageBox.Show("a");
            url = obj;
        }

        /// <summary>
        /// Cancels the current operation (currently unimplemented)
        /// </summary>
        private void Cancel()
        {
        }

        /// <summary>
        /// Registers a user with the given name and email.
        /// </summary>
        private void Register(string name, string server)
        {
            try
            {
                view.EnableControls(false);
                using (HttpClient client = CreateClient())
                {
                    // Create the parameter
                    dynamic user = new ExpandoObject();
                    user.Name = name;
                    user.server = server;

                    // Compose and send the request.
                    StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = client.PostAsync("RegisterUser", content).Result;

                    // Deal with the response
                    if (response.IsSuccessStatusCode)
                    {
                        String result = response.Content.ReadAsStringAsync().Result;
                        user1Token = (string)JsonConvert.DeserializeObject(result);
                        view.UserRegistered = true;
                    }
                    else
                    {
                        Console.WriteLine("Error registering: " + response.StatusCode);
                        Console.WriteLine(response.ReasonPhrase);
                    }
                }
            }
            finally
            {
                view.EnableControls(true);
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

