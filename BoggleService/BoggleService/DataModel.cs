using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoggleList

{
    public class NewGameRequest
    {
        public string UserToken { get; set; }
        public int TimeLimit { get; set; }
       
    }
    public class PlayerWord : Person
    {
        public string Word { get; set; }
    }
    public class Pending : NewGameRequest
    {
        public int GameID { get; set; }
    }
    public class NewGame
    {
        public string GameID { get; set; }
    }
    public class Person
    {
        public string UserToken { get; set; }
    }

    public class NewPlayer
    {
        public string Nickname { get; set; }
    }
    public class ThisWordScore
    {
        public int WordScore { get; set; }
    }
    public class Player : NewPlayer
    {
        public int Score { get; set; }
    }
    public class WordsPlayed
    {
        
    }
    public class PlayerCompleted: Player
    {
        public List<Tuple<string, int>> WordsPlayed { get; set; }
    }


    /// <summary>
    /// Response (if game is pending)
    /// </summary>
    public class PendingGame
    {
        public string GameState { get; set; }
    }
    /// <summary>
    /// Response (if game is active or completed and "Brief=yes" was a parameter)
    /// </summary>
    public class ActiveGameBrief : PendingGame
    {
        public Player Player1 {get; set; }
        public Player Player2 { get; set; }
        public int TimeLeft { get; set; }
    }

    /// <summary>
    /// Response (if game is active and "Brief=yes" was not a parameter)
    /// </summary>
    public class ActiveGame : ActiveGameBrief
    {
        public int TimeLimit{ get; set; }
        public string Board { get; set; }
    }

    /// <summary>
    /// Response (if game is active and "Brief=yes" was not a parameter)
    /// </summary>
    public class GameCompleted : PendingGame
    {
        public int TimeLimit { get; set; }
        public string Board { get; set; }
        public int TimeLeft { get; set; }
        public PlayerCompleted Player1 { get; set; }
        public PlayerCompleted Player2 { get; set; }
    }

    public class GameItem: GameCompleted
    {
        public int StartTime { get; set; }
    }

}