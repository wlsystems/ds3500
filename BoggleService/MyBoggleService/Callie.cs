using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyBoggleService
{
    public class Callie
    {
        /// <summary>
        /// Launches a SimpleChatServer on port 60000.  Keeps the main
        /// thread active so we can send output to the console.
        /// </summary>
        
        // Listens for incoming connection requests
        private TcpListener server;

        // All the clients that have connected but haven't closed
        private List<ClientConnection> clients = new List<ClientConnection>();

        // Read/write lock to coordinate access to the clients list
        private readonly ReaderWriterLockSlim sync = new ReaderWriterLockSlim();

        /// <summary>
        /// Creates a SimpleChatServer that listens for connection requests on port 4000.
        /// </summary>
        public Callie(int port)
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
    }
}
