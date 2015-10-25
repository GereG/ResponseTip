using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Drawing.Imaging;
using Tweetinvi.Core.Parameters;

namespace TwitterHandling
{
    public sealed class TwitterHandlingClass
    {
        public class SearchResults
        {
            public UserRecord[] searchResultsData;

            public SearchResults(int numOfUsers)
            {
                searchResultsData = new UserRecord[numOfUsers];
                for(int i=0;i<numOfUsers;i++)
                {
                    searchResultsData[i] = new UserRecord();
                }
            }

            public SearchResults(SearchResults original)
            {
                Copy(original);

            }

            public void Copy(SearchResults original)
            {
                for (int i = 0; i < searchResultsData.Count(); i++)
                {
                    searchResultsData[i] = original.searchResultsData[i];
                }
            }
        }
        public class UserRecord
        {
            public string userProfileImageString;
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

        public static long PublishTweet(string tweet_string,long? inReplyToTweetId)
        {
            Tweetinvi.Core.Interfaces.ITweet newTweet=null;
            if (inReplyToTweetId == 0)
            {
                newTweet = Tweet.PublishTweet(tweet_string);
            }else
            {
                newTweet = Tweet.PublishTweet(tweet_string, new PublishTweetOptionalParameters { InReplyToTweetId = inReplyToTweetId });
            }
            return newTweet.Id;
            //            newTweet.
            PublishTweetOptionalParameters parameters = new PublishTweetOptionalParameters();
            parameters.InReplyToTweetId = newTweet.Id;
                }
        public static SearchResults SearchUsersM(string username)
        {
            IEnumerable < Tweetinvi.Core.Interfaces.IUser> users = Search.SearchUsers(username,20,0);

            int maxUsers = users.Count();
            if (maxUsers > 20) maxUsers = 20;

            SearchResults searchResults = new SearchResults(maxUsers);

            for (int i=0; i<maxUsers; i++)
            {
                var uzivatel = users.ElementAt(i);
                var stream = User.GetProfileImageStream(uzivatel);

                var image = Bitmap.FromStream(stream);
                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, ImageFormat.Png);
                    string imgString = Convert.ToBase64String(ms.ToArray());
                    searchResults.searchResultsData[i].userProfileImageString = String.Format("data:image/png;base64,{0}", imgString);
                    // use the memory stream to base64 encode..
                }

                searchResults.searchResultsData[i].userProfileName=users.ElementAt(i).ScreenName;
                searchResults.searchResultsData[i].userName = users.ElementAt(i).Name;
                Debug.WriteLine("Username: " + username + "  ScreenName: " + searchResults.searchResultsData[i].userProfileName + " Name: " + searchResults.searchResultsData[i].userName);
            }

            return searchResults;
 //           System.d
            
            //           return Search.SearchUsers(username);
        }

        public static long PostATweetOnAWall(string username,string question)
        {
            Tweetinvi.Core.Interfaces.ITweet newTweet = null;
            if (username != null)
            {
                string tweetText = "@" + username + " " + question;
                newTweet=Tweet.PublishTweet(tweetText);
            }
            else throw new NullReferenceException();

            return (newTweet.Id);
        }

        private static byte[] turnImageToByteArray(System.Drawing.Image img)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            return ms.ToArray();
        }

        public static void CheckAnswerToQuestion()
        {
            
        }



        //        public static void 
    }

}
