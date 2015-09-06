using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;

namespace TwitterHandling
{
    public sealed class TwitterHandlingClass
    {

            public static void twitterAuthentication(string ConsumerKey, string ConsumerSecret, string AccessToken, string AccessTokenSecret)
            {
                Auth.SetUserCredentials(ConsumerKey,ConsumerSecret,AccessToken,AccessTokenSecret);
            }

            public static void PublishTweet(string tweet_string)
            {
                Tweet.PublishTweet(tweet_string);
            }
            public static void SearchUsersM(string username)
            {
                var users = Search.SearchUsers(username);
                //            IEnumerator<Tweetinvi.Core.Interfaces.IUser> enumerator=user.GetEnumerator();
                foreach (Tweetinvi.Core.Interfaces.IUser u in users)
                {

                }

                //           return Search.SearchUsers(username);

            }
        }
}
