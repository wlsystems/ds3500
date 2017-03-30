using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.Net.HttpStatusCode;
using System.Diagnostics;
using Newtonsoft.Json;
using BoggleList;
using System.Dynamic;

namespace Boggle
{
    /// <summary>
    /// Provides a way to start and stop the IIS web server from within the test
    /// cases.  If something prevents the test cases from stopping the web server,
    /// subsequent tests may not work properly until the stray process is killed
    /// manually.
    /// </summary>
    public static class IISAgent
    {
        // Reference to the running process
        private static Process process = null;

        /// <summary>
        /// Starts IIS
        /// </summary>
        public static void Start(string arguments)
        {
            if (process == null)
            {
                ProcessStartInfo info = new ProcessStartInfo(Properties.Resources.IIS_EXECUTABLE, arguments);
                info.WindowStyle = ProcessWindowStyle.Minimized;
                info.UseShellExecute = false;
                process = Process.Start(info);
            }
        }

        /// <summary>
        ///  Stops IIS
        /// </summary>
        public static void Stop()
        {
            if (process != null)
            {
                process.Kill();
            }
        }
    }
    [TestClass]
    public class BoggleTests
    {
        /// <summary>
        /// This is automatically run prior to all the tests to start the server
        /// </summary>
        [ClassInitialize()]
        public static void StartIIS(TestContext testContext)
        {
            IISAgent.Start(@"/site:""BoggleService"" /apppool:""Clr4IntegratedAppPool"" /config:""..\..\..\.vs\config\applicationhost.config""");
        }

        /// <summary>
        /// This is automatically run when all tests have completed to stop the server
        /// </summary>
        [ClassCleanup()]
        public static void StopIIS()
        {
            IISAgent.Stop();
        }

        private RestTestClient client = new RestTestClient("http://localhost:60000/BoggleService.svc/");

        /// <summary>
        /// Note that DoGetAsync (and the other similar methods) returns a Response object, which contains
        /// the response Stats and the deserialized JSON response (if any).  See RestTestClient.cs
        /// for details.
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            Response r = client.DoGetAsync("word?index={0}", "-5").Result;
            Assert.AreEqual(Forbidden, r.Status);

            r = client.DoGetAsync("word?index={0}", "5").Result;
            Assert.AreEqual(OK, r.Status);

            string word = (string) r.Data;
            Assert.AreEqual("AAL", word);
        }
        /// <summary>
        ///  Does a basic register method to check for created and for
        ///  forbidden if nickname is null or an empty string.  
        /// </summary>
        [TestMethod]
        public void TestRegister()
        {
            dynamic user = new ExpandoObject();
            user.Nickname = "Bugs Bunny";
            Response r = client.DoPostAsync("users", user).Result;
            Assert.AreEqual(Created, r.Status);
            
            user.Nickname = null;
            r = client.DoPostAsync("users", user).Result;
            Assert.AreEqual(Forbidden, r.Status);

            user.Nickname = "         ";
            r = client.DoPostAsync("users", user).Result;
            Assert.AreEqual(Forbidden, r.Status);

            user.Nickname = "     Elmo         ";
            r = client.DoPostAsync("users", user).Result;
            Assert.AreEqual(Created, r.Status);
        }

        [TestMethod]
        public void TestJoinGame()
        {
            dynamic user = new ExpandoObject();
            user.Nickname = "Bugs Bunny";
            Response r = client.DoPostAsync("users", user).Result;
            Assert.AreEqual(Created, r.Status);
            user = r.Data;
            string userToken = user.UserToken;
            Assert.IsNotNull(userToken);

            user = new ExpandoObject();
            user.Nickname = "Bob";
            r = client.DoPostAsync("users", user).Result;
            Assert.AreEqual(Created, r.Status);
            user = r.Data;
            string user2Token = user.UserToken;
            Assert.IsNotNull(user2Token);

            dynamic game = new ExpandoObject();
            game.TimeLimit = 50;
            game.UserToken = userToken;
            r = client.DoPostAsync("games", game).Result;
            Assert.AreEqual(Accepted, r.Status);

            game = new ExpandoObject();
            game.TimeLimit = 50;
            game.UserToken = user2Token;
            r = client.DoPostAsync("games", game).Result;
            Assert.AreEqual(Created, r.Status);

        }

        [TestMethod]
        public void TestJoinGameForPlayerwithWrongTime()
        {
            dynamic user = new ExpandoObject();
            user.Nickname = "Bugs Bunny";
            Response r = client.DoPostAsync("users", user).Result;
            Assert.AreEqual(Created, r.Status);
            user = r.Data;
            string userToken = user.userToken;

            dynamic game = new ExpandoObject();
            game.TimeLimit = 500;
            game.UserToken = userToken;
            r = client.DoPostAsync("games", game).Result;
            Assert.AreEqual(Forbidden, r.Status);

            game.TimeLimit = 2;
            r = client.DoPostAsync("games", game).Result;
            Assert.AreEqual(Forbidden, r.Status);

        }
    }
}
