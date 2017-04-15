//Dustin Shiozaki u0054455
//Tracy King u0040235

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq; 
using System.Web;

namespace Boggle

{

    /// <summary>
    /// Load dictionary... 
    /// </summary>
    [Serializable()]
    public class Dict
    {
        public HashSet<string> strings { get; set; }
    }
    /// <summary>
    /// Used when the player sends a join game request.
    /// </summary>
    [Serializable()]
    public class NewGameRequest : Person
    {
        public int TimeLimit { get; set; }
    }
    /// <summary>
    /// The objec the user sends to play a word that contains the word to be played and the UserToken
    /// </summary>
    [Serializable()]
    public class PlayerWord : Person
    {
        public string Word { get; set; }
    }
    /// <summary>
    /// Has the time limit and the game ID.
    /// </summary>
    [Serializable()]
    public class Pending : NewGameRequest
    {
        public int GameID { get; set; }
    }

    /// <summary>
    /// Returns the Game Id to the user.
    /// </summary>
    [Serializable()]
    public class NewGame
    {
        public string GameID { get; set; }
    }
    /// <summary>
    /// Returns the user token to the user.
    /// </summary>
    [Serializable()]
    public class Person
    {
        public string UserToken { get; set; }
    }
    /// <summary>
    /// The user sends this object to register.
    /// </summary>
    [Serializable()]
    public class NewPlayer
    {
        public string Nickname { get; set; }
    }
    /// <summary>
    /// Returns the score to the user in this object.
    /// </summary>
    [Serializable()]
    public class WordScore
    {
        public int Score { get; set; }
    }
    /// <summary>
    /// Player contains the score, and nickname. 
    /// </summary>
    [Serializable()]
    public class Player : NewPlayer
    {
        public int Score { get; set; }
    }

    /// <summary>
    /// Contains a list of the words played and the corresponding score.
    /// </summary>
    [Serializable()]
    public class PlayerCompleted : Player
    {
        public List<WordsPlayed> WordsPlayed { get; set; }
    }


    /// <summary>
    /// Response (if game is pending)
    /// </summary>
    [Serializable()]
    public class PendingGame
    {
        public string GameState { get; set; }
    }
    /// <summary>
    /// Response (if game is active or completed and "Brief=yes" was a parameter)
    /// </summary>
    [Serializable()]
    public class ActiveGameBrief : PendingGame
    {
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public int TimeLeft { get; set; }
    }

    /// <summary>
    /// Response (if game is active and "Brief=yes" was not a parameter)
    /// </summary>
    [Serializable()]
    public class ActiveGame : ActiveGameBrief
    {
        public int TimeLimit { get; set; }
        public string Board { get; set; }
    }

    /// <summary>
    /// Response (if game is active and "Brief=yes" was not a parameter)
    /// </summary>
    [Serializable()]
    public class GameCompleted : PendingGame
    {
        public int TimeLimit { get; set; }
        public string Board { get; set; }
        public int TimeLeft { get; set; }
        public PlayerCompleted Player1 { get; set; }
        public PlayerCompleted Player2 { get; set; }
    }

    /// <summary>
    /// GameItem has one extra field that the user won't see.
    /// </summary>
    [Serializable()]
    public class GameItem : GameCompleted
    {
        public int StartTime { get; set; }
    }
    /// <summary>
    /// Contains the word and corresponding score.
    /// </summary>
    [Serializable()]
    public class WordsPlayed
    {
        public int Score { get; set; }
        public string Word { get; set; }
    }
}