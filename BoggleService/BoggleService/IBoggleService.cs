using BoggleList;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Boggle
{
    [ServiceContract]
    public interface IBoggleService
    {

        /// <summary>
        /// Registers a new user.
        /// If  user.NickName is null or is empty after trimming, responds with status code Forbidden.
        /// Otherwise, creates a user, returns the user's token, and responds with status code Created. 
        /// </summary>
        [WebInvoke(Method = "POST", UriTemplate = "/users")]
        Person Register(UserInfo user);

        /// <summary>
        /// If  user.Nickname is not valid responds with status code Forbidden.  If time limit isn't valid reponse with status code Forbidden. If
        /// user is already in a game, response with status code Conflict.  
        /// Otherwise, places user in a game.  
        /// </summary>
        [WebInvoke(Method = "POST", UriTemplate = "/games")]
        string JoinGame(string userToken, int TimeLimit);

        /// <summary>
        ///  Takes in a user token.  If userToken is invalid or user is not in the pending game returns a status of Forbidden. If user
        ///  in the pending game, they are removed and returns a status response of OK.  
        /// </summary>
        [WebInvoke(Method = "PUT", UriTemplate = "/games")]
        void CancelJoin(string userToken);
        
        /// <summary>
        ///  Takes in a word, checks to see if the word is valid and exists on the board. If Word is null or empty when trimmed, or if
        ///  GameID or UserToken is missing or invalid, or if UserToken is not a player in the game identified by GameID, responds with 
        ///  response code 403 (Forbidden).  If game is not active response, with code Conflict. 
        /// </summary>
        [WebInvoke(Method = "PUT", UriTemplate = "/games/{string gameID}")]
        string PlayWord(string gameID, string userToken, string word);

        /// <summary>
        /// Sends back index.html as the response body.
        /// </summary>
        [WebGet(UriTemplate = "/api")]
        Stream API();
    }
}
