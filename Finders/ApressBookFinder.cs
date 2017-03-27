using System.Net;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using BookSearchAPI.Models;

namespace BookSearchAPI.Finders
{
    public class ApressBookFinder : BookFinder
    {
        private static string URL = "http://www.apress.com/gp/book/";

        private static string PAGE_REGEX = @"<span class=.{0,2}price-box.{0,2}\s*>\s*<span class=.{0,2}price.{0,2}>\s*(\d+\,\d+).*\s*</span>\s*</span>";

        override protected bool isISBNInProperFormat(string isbn)
        {
            return isbn.Length == 13;
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

        private string getSearchURL(string isbn) {
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