using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoggleList

{
    public class UserInfo
    {
        public string Name { get; set; }
        public int score { get; set; }
        public string UserToken { get; set; }
        public List<Tuple<string, int>> PlayedWords { get; set; }
    }

    public class Game
    {
        public int GameID { get; set; } 
        public string GameState { get; set; }
        public string GameBoard { get; set; }
        public int TimeLeft { get; set; }
        public int TimeLimit { get; set; }
        public UserInfo Player1 { get; set; }
        public UserInfo Player2 { get; set; } 
        public double StartTime { get; set; }
    }

}