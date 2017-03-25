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
        /// Sends back index.html as the response body.
        /// </summary>
        [WebGet(UriTemplate = "/api")]
        Stream API();
    }
}
