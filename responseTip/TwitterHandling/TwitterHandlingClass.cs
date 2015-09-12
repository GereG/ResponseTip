using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using System.Drawing;
using System.Diagnostics;

namespace TwitterHandling
{
    public sealed class TwitterHandlingClass
    {
        public class SearchResults
        {
            public UserRecord[] searchResultsData;

            public SearchResults()
            {
                searchResultsData = new UserRecord[20];
                for(int i=0;i<20;i++)
                {
                    searchResultsData[i] = new UserRecord();
                }
            }

            public SearchResults(SearchResults original)
            {
                Copy(original);
/*                searchResultsData = new UserRecord[20];
                for (int i = 0; i < 20; i++)
                {
                    searchResultsData[i] = new UserRecord();
                }*/
            }

            public void Copy(SearchResults original)
            {
                for (int i = 0; i < 20; i++)
                {
                    searchResultsData[i] = original.searchResultsData[i];
                }
            }
        }
        public class UserRecord
        {
            public Image userProfileImage;
            public string userProfileName;
            public string userName;

            public UserRecord()
            {

            }

        }
            
        public static void twitterAuthentication(string ConsumerKey, string ConsumerSecret, string AccessToken, string AccessTokenSecret)
        {
            Auth.SetUserCredentials(ConsumerKey, ConsumerSecret, AccessToken, AccessTokenSecret);
        }

        public static void PublishTweet(string tweet_string)
        {
            Tweet.PublishTweet(tweet_string);
        }
        public static SearchResults SearchUsersM(string username)
        {
            SearchResults searchResults=new SearchResults();
            var users = Search.SearchUsers(username);
            //            IEnumerator<Tweetinvi.Core.Interfaces.IUser> enumerator=user.GetEnumerator();
            for (int i=0; i<20; i++)
            {
                var uzivatel = users.ElementAt(i);
                var stream = User.GetProfileImageStream(uzivatel);
                var image = Bitmap.FromStream(stream);
                searchResults.searchResultsData[i].userProfileImage = image;
                searchResults.searchResultsData[i].userProfileName=users.ElementAt(i).ScreenName;
                searchResults.searchResultsData[i].userName = users.ElementAt(i).Name;
                Debug.WriteLine("Username: " + username + "  ScreenName: " + searchResults.searchResultsData[i].userProfileName + " Name: " + searchResults.searchResultsData[i].userName);
            }

            return searchResults;
 //           System.d
            
            //           return Search.SearchUsers(username);
        }

//        public static void 
    }

}
