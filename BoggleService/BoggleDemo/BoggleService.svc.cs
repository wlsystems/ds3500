using BoggleList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using static System.Net.HttpStatusCode;
/// <summary>
/// The Bogglenamespace contains the boggle.svc
/// </summary>
namespace Boggle
{
    /// <summary>
    /// 
    /// </summary>
    public class BoggleService : IBoggleService
    {
        // The connection string to the DB
        private static string BoggleDB;
        private int GameID = 101;
        private readonly static Dict dic = new Dict();


        static BoggleService()
        {
            BoggleDB = ConfigurationManager.ConnectionStrings["BoggleDB"].ConnectionString;
        }
        /// <summary>
        /// The most recent call to SetStatus determines the response code used when.
        /// an http response is sent..
        /// </summary>
        /// <param name="status"></param>
        private static void SetStatus(HttpStatusCode status)
        {
            WebOperationContext.Current.OutgoingResponse.StatusCode = status;
        }


        /// <summary>
        /// Join a game. 
        ///If UserToken is invalid, TimeLimit< 5, or TimeLimit> 120, responds with status 403 (Forbidden).
        ///Otherwise, if UserToken is already a player in the pending game, responds with status 409 (Conflict). 
        ///Otherwise, if there is already one player in the pending game, adds UserToken as the second player.The pending game becomes active and a new pending game with no players is created.The active game's time limit is the integer average of the time limits requested by the two players. Returns the new active game's GameID(which should be the same as the old pending game's GameID). Responds with status 201 (Created). 
        ///Otherwise, adds UserToken as the first player of the pending game, and the TimeLimit as the pending game's requested time limit. Returns the pending game's GameID. Responds with status 202 (Accepted). 
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        public Person Register(NewPlayer newUser)
        {
            if (newUser.Nickname == null || newUser.Nickname.Trim(' ').Length == 0)
            {
                SetStatus(Forbidden);   //if user nickname was null or nickname is empty string
                return null;
            }
            using (SqlConnection conn = new SqlConnection(BoggleDB))
            {
                // Connections must be opened
                conn.Open();

                // Database commands should be executed within a transaction.  When commands 
                // are executed within a transaction, either all of the commands will succeed
                // or all will be canceled.  You don't have to worry about some of the commands
                // changing the DB and others failing.
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    // An SqlCommand executes a SQL statement on the database.  In this case it is an
                    // insert statement.  The first parameter is the statement, the second is the
                    // connection, and the third is the transaction.  
                    //
                    // Note that I use symbols like @UserID as placeholders for values that need to appear
                    // in the statement.  You will see below how the placeholders are replaced.  You may be
                    // tempted to simply paste the values into the string, but this is a BAD IDEA that violates
                    // a cardinal rule of DB Security 101.  By using the placeholder approach, you don't have
                    // to worry about escaping special characters and you don't have to worry about one form
                    // of the SQL insertion attack.
                    using (SqlCommand command =
                        new SqlCommand("insert into Users (UserID, Nickname) values(@UserID, @Nickname)",
                                        conn,
                                        trans))
                    {
                        // We generate the userID to use.
                        string userID = Guid.NewGuid().ToString();

                        // This is where the placeholders are replaced.
                        command.Parameters.AddWithValue("@UserID", userID);
                        command.Parameters.AddWithValue("@Nickname", newUser.Nickname);

                        // This executes the command within the transaction over the connection.  The number of rows
                        // that were modified is returned.  Perhaps I should check and make sure that 1 is returned
                        // as expected.
                        command.ExecuteNonQuery();

                        // Immediately before each return that appears within the scope of a transaction, it is
                        // important to commit the transaction.  Otherwise, the transaction will be aborted and
                        // rolled back as soon as control leaves the scope of the transaction. 
                        trans.Commit();
                        Person p = new Person();
                        p.UserToken = userID;
                        SetStatus(Created);
                        return p;
                    }
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
        /// Join a game. 
        ///If UserToken is invalid, TimeLimit< 5, or TimeLimit> 120, responds with status 403 (Forbidden).
        ///Otherwise, if UserToken is already a player in the pending game, responds with status 409 (Conflict). 
        ///Otherwise, if there is already one player in the pending game, adds UserToken as the second player.The pending game becomes active and a new pending game with no players is created.The active game's time limit is the integer average of the time limits requested by the two players. Returns the new active game's GameID(which should be the same as the old pending game's GameID). Responds with status 201 (Created). 
        ///Otherwise, adds UserToken as the first player of the pending game, and the TimeLimit as the pending game's requested time limit. Returns the pending game's GameID. Responds with status 202 (Accepted). 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public NewGame JoinGame(NewGameRequest obj)
        {
            NewGame ng = new NewGame();
            string player = obj.UserToken;
            if (obj.UserToken == null | obj.TimeLimit < 5 | obj.TimeLimit > 120)   //token is null or time is invalid
            {
                SetStatus(Forbidden);
                return null;

            }
            using (SqlConnection conn = new SqlConnection(BoggleDB))
            {
                // Connections must be opened
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    using (SqlCommand command =
                        new SqlCommand("insert into Games (GameID, Player1) values(@GameID, @Player1)",
                                        conn,
                                        trans))
                    {
                        // This is where the placeholders are replaced.
                        command.Parameters.AddWithValue("@GameID", GameID);
                        command.Parameters.AddWithValue("@Player1", player);

                        // This executes the command within the transaction over the connection.  The number of rows
                        // that were modified is returned.  Perhaps I should check and make sure that 1 is returned
                        // as expected.
                        command.ExecuteNonQuery();
                        SetStatus(Created);
                        trans.Commit();
                        return ng;
                    }
                }
            }

        }

        /// <summary>
        ///  Takes in a user token.  If userToken is invalid or user is not in the pending game returns a status of Forbidden. If user
        ///  in the pending game, they are removed and returns a status response of OK. 
        /// </summary>
        public void CancelJoin(Person cancelobj)
        {
            if (cancelobj.UserToken == null)
            {
                SetStatus(Forbidden);      //the userToken was null, the user is not registered or they are not in the pending game
                return;
            }

        }


        /// <summary>
        /// Get game status information. 
        ///If GameID is invalid, responds with status 403 (Forbidden). 
        ///Otherwise, returns information about the game named by GameID as illustrated below.Note that the information returned depends on whether "Brief=yes" was included as a parameter as well as on the state of the game. Responds with status code 200 (OK). Note: The Board and Words are not case sensitive.
        /// </summary>
        /// <param name="gameobj"></param>
        /// <returns></returns>
        public Stream GameStatus(string GameID, string Brief)
        {
            GameID = GameID.Trim(' ');
            if (GameID == null || GameID == "")  //this is checking for null or empty gameIDs
            {
                SetStatus(Forbidden);
                return null;
            }
            return null;
        }

        /// <summary>
        /// Calculate and set the time left.
        /// </summary>
        /// <param name="timeLimit"></param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        private int SetTime(int timeLimit, int startTime)
        {
            if (timeLimit - ((int)DateTime.Now.TimeOfDay.TotalSeconds - startTime) > timeLimit)
                return 0;
            else
                return timeLimit - ((int)DateTime.Now.TimeOfDay.TotalSeconds - startTime);

        }

        /// <summary>
        /// Play a word in a game. 
        ///If Word is null or empty when trimmed, or if GameID or UserToken is missing or invalid, or if UserToken is not a player in the game identified by GameID, responds with response code 403 (Forbidden). 
        ///Otherwise, if the game state is anything other than "active", responds with response code 409 (Conflict). 
        ///Otherwise, records the trimmed Word as being played by UserToken in the game identified by GameID.Returns the score for Word in the context of the game(e.g. if Word has been played before the score is zero). Responds with status 200 (OK). Note: The word is not case sensitive.
        /// </summary>
        /// <param name="w"></param>
        /// <param name="gid"></param>
        /// <returns></returns>
        public WordScore PlayWord(PlayerWord w, string gid)
        {

            WordScore ws = new WordScore();
            gid = gid.Trim(' ');
            if (gid == null || gid == "")  //this is checking for null or empty gameIDs
            {
                SetStatus(Forbidden);
                return null;
            }

            String word = w.Word.Trim(' ').ToUpper();
            if (word == null | word == "" | w.UserToken == null)
            {
                SetStatus(Forbidden);
            }

            return ws;
        }

    }
}