using System.Net;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

namespace BookSearchAPI.Finders
{
    public class ISBNFinder
    {
        public static string URL = "http://openlibrary.org/search.json?";
        public string[] FindBookInRemoteDatabase(string title)
        {
            string responseBody = GetBookData(title);

            return ExtractISBNNumbers(responseBody);
        }

        private string GetBookData(string title)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL + "title=" + title.Replace(" ", "+") + "&publisher=apress");
            request.Method = "GET";

            Task<WebResponse> task = request.GetResponseAsync();

            HttpWebResponse response = (HttpWebResponse)task.Result;
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        private string[] ExtractISBNNumbers(string responseBody)
        {
            List<string> allIsbns = new List<string>();

            int isbnPrevIndex = 0;
            int isbnIndex = responseBody.IndexOf("isbn");
            while(isbnIndex > isbnPrevIndex) {
                int begin = responseBody.IndexOf("[", isbnIndex);
                int end = responseBody.IndexOf("]", isbnIndex);

                string isbnStr = responseBody.Substring(begin + 1, end - begin - 1);
                string[] isbns = isbnStr.Split(new char[]{','});

                for (int i = 0; i < isbns.Length; i++)
                {
                    allIsbns.Add(isbns[i].Trim().Replace("\"", "").Replace("-", ""));
                }

                isbnPrevIndex = isbnIndex;
                isbnIndex = responseBody.IndexOf("isbn", isbnPrevIndex);
            }

            return allIsbns.ToArray();
        }
    }
}