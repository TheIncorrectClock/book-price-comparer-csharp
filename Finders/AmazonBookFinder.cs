using System.Net;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using BookSearchAPI.Models;

namespace BookSearchAPI.Finders
{
    public class AmazonBookFinder : BookFinder
    {
        private static string URL = "https://www.amazon.com/gp/product/";

        private static string PAGE_REGEX = @"<div class=.{0,2}a-row.{0,2}>\s*<span class=.{0,2}a-size-large mediaTab_title.{0,2}>Paperback</span>\s*</div>\s*<div class=.{0,2}a-row.{0,2}>\s*<span class=.{0,2}a-size-base mediaTab_subtitle.{0,2}>\s*((\$\d+\.\d+\s*-\s*\$\d+\.\d+)|(\$\d+\.\d+))\s*</span>\s*</div>";

        override protected bool isISBNInProperFormat(string isbn)
        {
            return isbn.Length == 10;
        }

        override protected string GetBookPage(string isbn)
        {
            string searchUrl = getSearchURL(isbn);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(searchUrl);
            request.Method = "GET";

            Task<WebResponse> task = request.GetResponseAsync();
            HttpWebResponse response = (HttpWebResponse)task.Result;
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }

        private string getSearchURL(string isbn)
        {
            return URL + isbn;
        }

        override protected BookInfo ExtractBookInfoFromBookPage(string responseBody, string isbn)
        {
            BookInfo book = new BookInfo(null, null, null);
            Regex regex = new Regex(PAGE_REGEX);
            Match match = regex.Match(responseBody);
            if(match.Success) {
                book = new BookInfo(isbn, match.Groups[1].Value, getSearchURL(isbn));
            }

            return book;
        }
    }
}