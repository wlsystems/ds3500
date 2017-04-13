using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using static System.Net.HttpStatusCode;
using Newtonsoft.Json.Linq;
/// <summary>
/// The Bogglenamespace contains the boggle.svc
/// </summary>
namespace Boggle
{
    /// <summary>
    /// Represents a connection with a remote client.  Takes care of receiving and sending
    /// information to that client according to the protocol.
    /// </summary>
    public class ClientConnection
    {
        // Incoming/outgoing is UTF8-encoded.  This is a multi-byte encoding.  The first 128 Unicode characters
        // (which corresponds to the old ASCII character set and contains the common keyboard characters) are
        // encoded into a single byte.  The rest of the Unicode characters can take from 2 to 4 bytes to encode.
        private static System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

        // Buffer size for reading incoming bytes
        private const int BUFFER_SIZE = 1024;

        // The socket through which we communicate with the remote client
        private Socket socket;

        // Text that has been received from the client but not yet dealt with
        private StringBuilder incoming;

        // Text that needs to be sent to the client but which we have not yet started sending
        private StringBuilder outgoing;

        // For decoding incoming UTF8-encoded byte streams.
        private Decoder decoder = encoding.GetDecoder();

        // Buffers that will contain incoming bytes and characters
        private byte[] incomingBytes = new byte[BUFFER_SIZE];
        private char[] incomingChars = new char[BUFFER_SIZE];

        // Records whether an asynchronous send attempt is ongoing
        private bool sendIsOngoing = false;

        // For synchronizing sends
        private readonly object sendSync = new object();

        // Bytes that we are actively trying to send, along with the
        // index of the leftmost byte whose send has not yet been completed
        private byte[] pendingBytes = new byte[0];
        private int pendingIndex = 0;
        //save the command string because the state is reset.
        String[] cmd;
        // Name of chatter or null if unknown
        private string name = null;
        private BoggleService server;
        private int programCounter;
        /// <summary>
        /// Creates a ClientConnection from the socket, then begins communicating with it.
        /// </summary>
        public ClientConnection(Socket s, BoggleService server)
        {
            cmd = new String[3]; 
            programCounter = 0;
            // Record the socket and server and initialize incoming/outgoing
            this.server = server;
            socket = s;
            incoming = new StringBuilder();
            outgoing = new StringBuilder();

            try
            {
                // Ask the socket to call MessageReceive as soon as up to 1024 bytes arrive.
                socket.BeginReceive(incomingBytes, 0, incomingBytes.Length,
                                    SocketFlags.None, MessageReceived, null);
            }
            catch (ObjectDisposedException)
            {
            }
        }

        /// <summary>
        /// Called when some data has been received.
        /// </summary>
        private void MessageReceived(IAsyncResult result)
        {
            // Figure out how many bytes have come in
            int bytesRead = socket.EndReceive(result);
            HttpStatusCode status;
            // If no bytes were received, it means the client closed its side of the socket.
            // Report that to the console and close our socket.
            if (bytesRead == 0)
            {
                Console.WriteLine("Socket closed");
                server.RemoveClient(this);
                socket.Close();
            }

            // Otherwise, decode and display the incoming bytes.  Then request more bytes.
            else
            {

                // Convert the bytes into characters and appending to incoming
                int charsRead = decoder.GetChars(incomingBytes, 0, bytesRead, incomingChars, 0, false);
                incoming.Append(incomingChars, 0, charsRead);
                Console.WriteLine(incoming);

                // Echo any complete lines, after capitalizing them
                int lastNewline = -1;
                int start = 0;
                String line = "";
                String[] input = new string[5];
   

                for (int i = 0; i < incoming.Length; i++)
                {
                    if (incoming[i] == '\n')
                    {
                        line = incoming.ToString(start, i + 1 - start);
                        if (name == null)
                        {
                            name = line.Substring(0, line.Length - 2);
                            server.SendToAllClients("Welcome " + name + "\r\n");
                            cmd = name.Split('/');
                        }
                        else
                        {
                            server.SendToAllClients(name + "> " + line.ToUpper());
                        }
                        lastNewline = i;
                        start = i + 1;
                    }
                }
                if (programCounter == 1)
                {
                    if (cmd[0].Equals("POST "))
                    {
                        if (cmd[2].Equals("users HTTP"))
                        {
                            line = incoming.ToString();
                            input = line.Split('"');
                            NewPlayer np = new NewPlayer();
                            np.Nickname = input[3];
                            server.Register(np, out status);
                        }                       
                    }
                }
                programCounter += 1;
                incoming.Remove(0, lastNewline + 1);    
                try
                {
                    // Ask for some more data
                    socket.BeginReceive(incomingBytes, 0, incomingBytes.Length,
                        SocketFlags.None, MessageReceived, null);
                }
                catch (ObjectDisposedException)
                {
                }
            }
        }

        /// <summary>
        /// Sends a string to the client
        /// </summary>
        public void SendMessage(string lines)
        {
            // Get exclusive access to send mechanism
            lock (sendSync)
            {
                // Append the message to the outgoing lines
                outgoing.Append(lines);

                // If there's not a send ongoing, start one.
                if (!sendIsOngoing)
                {
                    sendIsOngoing = true;
                    SendBytes();
                }
            }
        }

        /// <summary>
        /// Attempts to send the entire outgoing string.
        /// This method should not be called unless sendSync has been acquired.
        /// </summary>
        private void SendBytes()
        {
            // If we're in the middle of the process of sending out a block of bytes,
            // keep doing that.
            if (pendingIndex < pendingBytes.Length)
            {
                try
                {
                    socket.BeginSend(pendingBytes, pendingIndex, pendingBytes.Length - pendingIndex,
                                     SocketFlags.None, MessageSent, null);
                }
                catch (ObjectDisposedException)
                {
                }
            }

            // If we're not currently dealing with a block of bytes, make a new block of bytes
            // out of outgoing and start sending that.
            else if (outgoing.Length > 0)
            {
                pendingBytes = encoding.GetBytes(outgoing.ToString());
                pendingIndex = 0;
                outgoing.Clear();
                try
                {
                    socket.BeginSend(pendingBytes, 0, pendingBytes.Length,
                                     SocketFlags.None, MessageSent, null);
                }
                catch (ObjectDisposedException)
                {
                }
            }

            // If there's nothing to send, shut down for the time being.
            else
            {
                sendIsOngoing = false;
            }
        }

        /// <summary>
        /// Called when a message has been successfully sent
        /// </summary>
        private void MessageSent(IAsyncResult result)
        {
            // Find out how many bytes were actually sent
            int bytesSent = socket.EndSend(result);

            // Get exclusive access to send mechanism
            lock (sendSync)
            {
                // The socket has been closed
                if (bytesSent == 0)
                {
                    socket.Close();
                    server.RemoveClient(this);
                    Console.WriteLine("Socket closed");
                }

                // Update the pendingIndex and keep trying
                else
                {
                    pendingIndex += bytesSent;
                    SendBytes();
                }
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class BoggleService 
    {
        /// <summary>
        /// Keeps track of the currently pending game
        /// </summary>
        private static string BoggleDB;
        private readonly static Pending pending = new Pending();
        private readonly static Dict dic = new Dict();
        // Listens for incoming connection requests
        private TcpListener server;

        // All the clients that have connected but haven't closed
        private List<ClientConnection> clients = new List<ClientConnection>();

        // Read/write lock to coordinate access to the clients list
        private readonly ReaderWriterLockSlim sync = new ReaderWriterLockSlim();
        public BoggleService(int port)
        {
            // A TcpListener listens for incoming connection requests
            server = new TcpListener(IPAddress.Any, port);

            // Start the TcpListener
            server.Start();

            // Ask the server to call ConnectionRequested at some point in the future when 
            // a connection request arrives.  It could be a very long time until this happens.
            // The waiting and the calling will happen on another thread.  BeginAcceptSocket 
            // returns immediately, and the constructor returns to Main.
            server.BeginAcceptSocket(ConnectionRequested, null);

            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            AppDomain.CurrentDomain.SetData("DataDirectory", baseDirectory);
            //BoggleDB = ConfigurationManager.ConnectionStrings["BoggleDB"].ConnectionString;
            BoggleDB = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + baseDirectory + @"BoggleDB.mdf; Integrated Security = True; User Instance=False;";
        }

        /// <summary>
        /// This is the callback method that is passed to BeginAcceptSocket.  It is called
        /// when a connection request has arrived at the server.
        /// </summary>
        private void ConnectionRequested(IAsyncResult result)
        {
            // We obtain the socket corresonding to the connection request.  Notice that we
            // are passing back the IAsyncResult object.
            Socket s = server.EndAcceptSocket(result);

            // We ask the server to listen for another connection request.  As before, this
            // will happen on another thread.
            server.BeginAcceptSocket(ConnectionRequested, null);

            // We create a new ClientConnection, which will take care of communicating with
            // the remote client.  We add the new client to the list of clients, taking 
            // care to use a write lock.
            try
            {
                sync.EnterWriteLock();
                clients.Add(new ClientConnection(s, this));
            }
            finally
            {
                sync.ExitWriteLock();
            }
        }

        /// <summary>
        /// Sends the message to all clients
        /// </summary>
        public void SendToAllClients(string msg)
        {
            // Here we use a read lock to access the clients list, which allows concurrent
            // message sending.
            try
            {
                sync.EnterReadLock();
                foreach (ClientConnection c in clients)
                {
                    c.SendMessage(msg);
                }
            }
            finally
            {
                sync.ExitReadLock();
            }
        }

        /// <summary>
        /// Remove c from the client list.
        /// </summary>
        public void RemoveClient(ClientConnection c)
        {
            try
            {
                sync.EnterWriteLock();
                clients.Remove(c);
            }
            finally
            {
                sync.ExitWriteLock();
            }
        }
  
    /// <summary>
    /// The most recent call to SetStatus determines the response code used when.
    /// an http response is sent..
    /// </summary>
    /// <param name="status"></param>
    private static void SetStatus(HttpStatusCode status)
        {
            //WebOperationContext.Current.OutgoingResponse.StatusCode = status;
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
        public Person Register(NewPlayer user, out HttpStatusCode status)
        {
            Dictionary<string, dynamic> placeholders = new Dictionary<string, dynamic>();
            
            if (user.Nickname == null || user.Nickname.Trim().Length == 0 || user.Nickname.Trim().Length > 50)
            {
                status = Forbidden;
                return null;
            }
            dynamic userID = Guid.NewGuid().ToString();
            placeholders.Add("@UserID", userID);
            placeholders.Add("@Nickname", user.Nickname);
            string sql = "insert into Users (UserID, Nickname) values(@UserID, @Nickname)";
            Person p = new Person();
            p.UserToken = userID;
            status = Created;
            Helper(sql, placeholders, 1);
            return p;
        }

        public List<Dictionary<string, dynamic>> Helper(string strCommand, Dictionary<string, dynamic> coms, int type)
        {
            List<Dictionary<string, dynamic>> obj = new List<Dictionary<string, dynamic>>();
            using (SqlConnection conn = new SqlConnection(BoggleDB))
            {
                //open connection
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    using (SqlCommand command =
                        new SqlCommand(strCommand,
                                        conn,
                                        trans))
                    {
                        foreach (KeyValuePair<string, dynamic> entry in coms)
                            command.Parameters.AddWithValue(entry.Key, entry.Value);
                        trans.Commit();
                        if (type == 1)
                            command.ExecuteNonQuery();
                        if (type == 2)
                        {
                            Dictionary<string, dynamic> d = new Dictionary<string, dynamic>();
                            d.Add("GameID", command.ExecuteScalar().ToString());
                            obj.Add(d);
                            return obj;
                        }
                        if (type == 3)
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                // Check to see user is in table, set forbidden if not
                                if (!reader.HasRows)
                                {
                                    reader.Close();
                                    return null;
                                }
                                else
                                {
                                    int j = 0;
                                    while (reader.Read())
                                    {
                                        Dictionary<string, dynamic> d = new Dictionary<string, dynamic>();
                                        for (int i = 0; i < reader.FieldCount; i++)
                                            if (reader.GetValue(i) != null)
                                            {
                                                d.Add(reader.GetName(i), reader.GetValue(i).ToString());
                                                obj.Add(d);
                                            }
                                        j = j + 1;
                                    }
                                    return obj;
                                }
                            }
                        }
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Returns a Stream version of index.html.
        /// </summary>
        /// <returns></returns>
        public Stream API()
        {
            SetStatus(OK);
            //WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
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
            string cmd = "";
            Dictionary<string, dynamic> placeholders = new Dictionary<string, dynamic>();
            NewGame ng = new NewGame();
            if (obj.UserToken == null | obj.TimeLimit < 5 | obj.TimeLimit > 120)   //token is null or time is invalid
            {
                SetStatus(Forbidden);
                return null;
            }
            else if (obj.UserToken == pending.UserToken)    //user is already in pending game, so returns status conflict
            {
                SetStatus(Conflict);
                return null;
            }
            // Here, the SqlCommand is a select query.  We are interested in whether item.UserID exists in
            // the Users table.
            cmd = "select UserID from Users where UserID = @UserID";
            placeholders.Add("@UserID", obj.UserToken);
            if (Helper(cmd, placeholders, 3) == null)
            {
                SetStatus(Forbidden);
                return null;
            }
            if (pending.UserToken == null)  //very first request, initializes pending
            {
                pending.UserToken = "";
                //dic.strings = new HashSet<string>(File.ReadAllLines(HttpRuntime.AppDomainAppPath + "/dictionary.txt"));
            }
            if (pending.UserToken == "")
            {
                // Here we are executing an insert command, but notice the "output inserted.ItemID" portion.  
                // We are asking the DB to send back the auto-generated GameID.
                cmd = "insert into Games (Player1) output inserted.GameID values(@Player1)";
                List<Dictionary<string, dynamic>> obj2 = new List<Dictionary<string, dynamic>>();
                placeholders.Clear();
                placeholders.Add("@Player1", obj.UserToken);
                // We execute the command with the ExecuteScalar method, which will return to
                // us the requested auto-generated ItemID.
                obj2 = Helper(cmd, placeholders, 2);
                pending.TimeLimit = obj.TimeLimit;
                SetStatus(Accepted);
                ng.GameID = obj2[0]["GameID"];
                pending.GameID = Int32.Parse(obj2[0]["GameID"]);
                pending.UserToken = obj.UserToken;
            }
            else
            {
                string board = new BoggleBoard().ToString();
                cmd = "update Games set Player2= @Player2, TimeLimit=@TimeLimit, StartTime=@StartTime, Board=@Board, Player1Score=@Player1Score, Player2Score=@Player2Score where GameID=@GameID";
                placeholders.Clear();
                int time = (pending.TimeLimit + obj.TimeLimit) / 2;
                int startTime = (int)DateTime.Now.TimeOfDay.TotalSeconds;
                int score = 0;
                placeholders.Add("@Player2", obj.UserToken);
                placeholders.Add("@GameID", pending.GameID);
                placeholders.Add("@TimeLimit", time);
                placeholders.Add("@StartTime", startTime);
                placeholders.Add("@Board", board);
                placeholders.Add("@Player1Score", score);
                placeholders.Add("@Player2Score", score);
                Helper(cmd, placeholders, 1);
                SetStatus(Created);
                ng.GameID = pending.GameID.ToString();
                pending.UserToken = "";
                pending.GameID = 0;
            }
            return ng;
        }
        /// <summary>
        ///  Takes in a user token.  If userToken is invalid or user is not in the pending game returns a status of Forbidden. If user
        ///  in the pending game, they are removed and returns a status response of OK. 
        /// </summary>
        public void CancelJoin(Person cancelobj)
        {
            if ((cancelobj.UserToken == null) | (pending.UserToken != cancelobj.UserToken))
            {
                SetStatus(Forbidden);      //the userToken was null, the user is not registered or they are not in the pending game
                return;
            }
            string sql = "delete from Games where GameID = @GameID";
            Dictionary<string, dynamic> placeholders = new Dictionary<string, dynamic>();
            placeholders.Add("@GameID", pending.GameID);
            Helper(sql, placeholders, 1);
            pending.UserToken = "";
            pending.TimeLimit = 0;
            SetStatus(OK);
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
            string jsonClient = null;

            if (pending.GameID.ToString() == GameID)              //the game status requested is for the pending game
            {
                PendingGame pg = new PendingGame();
                pg.GameState = "pending";
                SetStatus(OK);
               // jsonClient = JsonConvert.SerializeObject(pg);
                //WebOperationContext.Current.OutgoingResponse.ContentType =
              //      "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes(jsonClient));
            }

            string sql = "select * from Games where GameID = @GameID";
            Dictionary<string, dynamic> d = new Dictionary<string, dynamic>();
            d.Add("@GameID", GameID);
            List<Dictionary<string, dynamic>> obj2 = new List<Dictionary<string, dynamic>>();
            obj2 = Helper(sql, d, 3);

            int timeLeft = 0;
            if (obj2 == null)    //the obj is empty so GameID is not in the the table
            {
                SetStatus(Forbidden);
                return null;
            }

            List<Dictionary<string, dynamic>> nickname1 = new List<Dictionary<string, dynamic>>();
            List<Dictionary<string, dynamic>> nickname2 = new List<Dictionary<string, dynamic>>();
            sql = "select * from Users where UserID = @UserId";
            d.Clear();
            string UserID = obj2[0]["Player1"];
            d.Add("@UserID", UserID);
            nickname1 = Helper(sql, d, 3);
            d.Clear();
            UserID = obj2[0]["Player2"];
            string user1 = obj2[0]["Player1"];
            d.Add("@UserID", UserID);
            nickname2 = Helper(sql, d, 3);

            timeLeft = SetTime(Int32.Parse(obj2[0]["TimeLimit"]), Int32.Parse(obj2[0]["StartTime"]));

            if (Brief == "yes")                            //either active or completed game, with brief as a parameter
            {
                ActiveGameBrief agb = new ActiveGameBrief();
                Player p1 = new Player();
                Player p2 = new Player();

                p1.Score = int.Parse(obj2[0]["Player1Score"]);     //adds player1 and player2
                p2.Score = int.Parse(obj2[0]["Player2Score"]);
                agb.Player1 = p1;
                agb.Player2 = p2;
                if (timeLeft > 0)                       //checks time to decide if game is active or completed
                {
                    agb.GameState = "active";
                    agb.TimeLeft = SetTime(Int32.Parse(obj2[0]["TimeLimit"]), int.Parse(obj2[0]["StartTime"]));
                }
                else
                {
                    agb.GameState = "completed";
                    agb.TimeLeft = 0;
                }
           //     jsonClient = JsonConvert.SerializeObject(agb);
            }
            else if (timeLeft <= 0)
            {
                GameCompleted gc = new GameCompleted();     //game state is completed and not brief, returns the full game item
                gc.GameState = "completed";
                gc.TimeLimit = int.Parse(obj2[0]["TimeLimit"]);
                gc.Board = obj2[0]["Board"];
                gc.TimeLeft = 0;
                PlayerCompleted p1 = new PlayerCompleted();
                PlayerCompleted p2 = new PlayerCompleted();
                p1.Score = int.Parse(obj2[0]["Player1Score"]);
                p2.Score = int.Parse(obj2[0]["Player2Score"]);
                p1.Nickname = nickname1[0]["Nickname"];
                p2.Nickname = nickname2[0]["Nickname"];
                ///looking up player words
                sql = "select Word, Score from Words where GameID = @GameID AND Player = @UserID order by Id";
                d.Clear();
                d.Add("@GameID", GameID); //first lookup player 2 using previously stored GameID
                d.Add("@UserID", UserID);
                obj2 = Helper(sql, d, 3);
                p2.WordsPlayed = GetWordList(sql, UserID, GameID);
                UserID = user1;
                d.Clear();
                d.Add("@GameID", GameID); //first lookup player 2 using previously stored GameID
                d.Add("@UserID", UserID);
                obj2 = Helper(sql, d, 3);
                p1.WordsPlayed = GetWordList(sql, user1, GameID);
                gc.Player1 = p1;
                gc.Player2 = p2;
           //     jsonClient = JsonConvert.SerializeObject(gc);
            }

            else      //game state is active and not brief
            {
                ActiveGame ag = new ActiveGame();
                ag.TimeLimit = int.Parse(obj2[0]["TimeLimit"]);
                ag.GameState = "active";
                ag.Board = obj2[0]["Board"];
                Player p1 = new Player();
                Player p2 = new Player();
                p1.Nickname = nickname1[0]["Nickname"];
                p2.Nickname = nickname2[0]["Nickname"];
                p1.Score = int.Parse(obj2[0]["Player1Score"]);
                p2.Score = int.Parse(obj2[0]["Player2Score"]);
                ag.Player1 = p1;
                ag.Player2 = p2;
                ag.TimeLeft = SetTime(Int32.Parse(obj2[0]["TimeLimit"]), int.Parse(obj2[0]["StartTime"]));
              //  jsonClient = JsonConvert.SerializeObject(ag);
            }
            //serializes which ever game was pulled and returns a stream
            SetStatus(OK);
            //WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            return new MemoryStream(Encoding.UTF8.GetBytes(jsonClient));
        }
        public List<WordsPlayed> GetWordList(string sql, string userID, string gid)
        {
            using (SqlConnection conn = new SqlConnection(BoggleDB))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    // Notice that we have to work a bit to construct the proper query, since it depends on what
                    // options the user specified.
                    String query = sql;

                    using (SqlCommand command = new SqlCommand(query, conn, trans))
                    {

                        command.Parameters.AddWithValue("@UserID", userID);
                        command.Parameters.AddWithValue("@GameID", gid);


                        // We are going to be creating some ToDoItem objects and returning them in
                        // this list.
                        List<WordsPlayed> result = new List<WordsPlayed>();

                        // As with all queries, we use the ExecuteReader method:
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Notice how we extract the values from each row by column name.
                                result.Add(new WordsPlayed
                                {
                                    Word = (string)reader["Word"],
                                    Score = (int)reader["Score"]
                                });
                            }
                        }
                        trans.Commit();
                        return result;
                    }
                }
            }
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
            gid = gid.Trim(' ');
            if (gid == null || gid == "")  //this is checking for null or empty gameIDs
            {
                SetStatus(Forbidden);
                return null;
            }
            WordScore ws = new WordScore();
            WordsPlayed wpObj = new WordsPlayed();
            String word = w.Word.Trim(' ').ToUpper();
            wpObj.Word = word;
            int player = 3;
            Dictionary<string, dynamic> dd = new Dictionary<string, dynamic>();
            List<Dictionary<string, dynamic>> game = new List<Dictionary<string, dynamic>>();
            List<Dictionary<string, dynamic>> words = new List<Dictionary<string, dynamic>>();
            string sql = "select * from Games where GameID = @GameID";
            dd.Add("@GameID", gid);
            game = Helper(sql, dd, 3);
            sql = "select * from Users where UserID = @UserID";
            int playerScore = 0;
            if (pending.GameID.ToString() == gid)
            {
                SetStatus(Conflict);
                return null;
            }
            if (game != null)
            {

                dd.Clear();
                dd.Add("@UserID", w.UserToken);
                if (Helper(sql, dd, 3) == null)
                {
                    SetStatus(Forbidden);
                    return null;
                }
                int i;
                if (game[0]["Player1"].Equals(w.UserToken))
                {
                    player = 1;
                    if (Int32.TryParse(game[0]["Player1Score"], out i))
                        playerScore = i;
                }
                else if (game[0]["Player2"].Equals(w.UserToken))
                {
                    player = 2;
                    if (Int32.TryParse(game[0]["Player2Score"], out i))
                        playerScore = i;
                }
            }
            else
            {
                SetStatus(Forbidden);
                return null;
            }
            dd.Add("@GameID", gid);
            sql = "select Word, Score from Words where GameID = @GameID AND Player = @UserID";
            words = Helper(sql, dd, 3);
            int timeLeft = SetTime(Int32.Parse(game[0]["TimeLimit"]), Int32.Parse(game[0]["StartTime"]));

            if (word == null | word == "" | player == 3)
            {
                SetStatus(Forbidden);
                return ws;
            }
            else if (timeLeft <= 0)
            {
                SetStatus(Conflict);
                return ws;
            }
            if (word.Length <= 2)
            {
                ws.Score = 0;
                wpObj.Score = 0;
                return AddScore(gid, ws, player, wpObj, playerScore, w.UserToken);
            }

            BoggleBoard bb = new BoggleBoard(game[0]["Board"]);
            if (CheckSetDup(wpObj, sql, w.UserToken, gid) == 0)
            {
                ws.Score = 0;
                wpObj.Score = 0;
                return AddScore(gid, ws, player, wpObj, playerScore, w.UserToken);
            }

            if (dic.strings.Contains(word) && bb.CanBeFormed(word))
            {
                if (word.Length == 3 | word.Length == 4)
                    ws.Score = 1;
                else if (word.Length == 5)
                    ws.Score = 2;
                else if (word.Length == 6)
                    ws.Score = 3;
                else if (word.Length == 7)
                    ws.Score = 5;
                else if (word.Length > 7)
                    ws.Score = 11;
                wpObj.Score = ws.Score;
            }
            else
            {
                ws.Score = -1;
                wpObj.Score = -1;
            }
            return AddScore(gid, ws, player, wpObj, playerScore, w.UserToken);

        }

        public int CheckSetDup(WordsPlayed wpObj, String sql, string userID, string gid)
        {
            string wordNew = wpObj.Word;
            List<WordsPlayed> iwp;
            iwp = GetWordList(sql, userID, gid);
            foreach (var word in iwp)
            {
                if (word.Word.Equals(wordNew))
                {
                    return 0;
                }

            }
            return -100;

        }
        /// <summary>
        /// Add the score of the word to the list of WordsPlayed. 
        /// Update the total score of the player for this game.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="gid"></param>
        /// <param name="ws"></param>
        /// <param name="player"></param>
        /// <param name="wpObj"></param>
        /// <returns></returns>
        public WordScore AddScore(string gid, WordScore ws, int player, WordsPlayed wpObj, int playerScore, string UserID)
        {
            SetStatus(OK);
            Dictionary<string, dynamic> d = new Dictionary<string, dynamic>();
            string sql = "insert into Words (Word, GameID, Player, Score) values(@Word, @GameID, @Player, @Score)";
            d.Add("@GameID", gid);
            d.Add("@Score", wpObj.Score);
            d.Add("@Player", UserID);
            d.Add("@Word", wpObj.Word);
            Helper(sql, d, 1);
            d.Clear();
            int newScore = ws.Score + playerScore;
            if (player == 1)
            {
                sql = "update Games set Player1Score=@Player1Score where GameID=@GameID";
                d.Add("@GameID", gid);
                d.Add("@Player1Score", newScore);
                Helper(sql, d, 1);
            }
            else if (player == 2)
            {
                sql = "update Games set Player2Score=@Player2Score where GameID=@GameID";
                d.Add("@GameID", gid);
                d.Add("@Player2Score", newScore);
                Helper(sql, d, 1);
            }
            return ws;

        }

    }
}