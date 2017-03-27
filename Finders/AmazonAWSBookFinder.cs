using System;
using System.Text;
using BookSearchAPI.Models;
using System.Security.Cryptography;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace BookSearchAPI.Finders
{
    public class AmazonAWSBookFinder : BookFinder
    {
        private static string AWS_DATATIME_FORMAT = "yyyy-MM-ddTHH:mm:ssZ";

        private static int HTTP_LENGTH = 7;

        private AmazonAWSRequestData data;

        public AmazonAWSBookFinder(AmazonAWSRequestData data) {
            this.data = data;
        }

        private static string URL = "http://webservices.amazon.com/onca/xml";

        private static string PRODUCT_URL = "https://www.amazon.com/gp/product/";

        private static string PAGE_REGEX = @"<LowestNewPrice>\s*<Amount>\d*</Amount>\s*<CurrencyCode>[A-Z]*</CurrencyCode>\s*<FormattedPrice>(.\d+\.\d+)</FormattedPrice>";

        override protected bool isISBNInProperFormat(string isbn)
        {
            return isbn.Length == 10;
        }

        override protected string GetBookPage(string isbn)
        {
            string searchUrl = GetSearchURL(isbn);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(searchUrl);
            request.Method = "GET";

            Task<WebResponse> task = request.GetResponseAsync();
            HttpWebResponse response = (HttpWebResponse)task.Result;
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }

        private string GetSearchURL(string isbn)
        {
            string url = (data.URL == null) ? URL : data.URL;

            StringBuilder urlBuilder = new StringBuilder();
            urlBuilder.Append("?AWSAccessKeyId=");
            urlBuilder.Append(data.AWSAccessKeyId);
            urlBuilder.Append("&AssociateTag=");
            urlBuilder.Append(data.AssociateTag);
            urlBuilder.Append("&IdType=ISBN");
            urlBuilder.Append("&ItemId=");
            urlBuilder.Append(isbn);
            urlBuilder.Append("&Operation=ItemLookup");
            urlBuilder.Append("&ResponseGroup=Offers");
            urlBuilder.Append("&Service=AWSECommerceService");
            urlBuilder.Append("&Timestamp=");
            urlBuilder.Append(GenerateTimeStamp());

            string signature = SignRequest(urlBuilder.ToString(), url, data.SecretKey);

            urlBuilder.Append("&Signature=");
            urlBuilder.Append(signature);

            return url + urlBuilder.ToString();
        }

        override protected BookInfo ExtractBookInfoFromBookPage(string responseBody, string isbn)
        {
            BookInfo book = new BookInfo(null, null, null);
            Regex regex = new Regex(PAGE_REGEX);
            Match match = regex.Match(responseBody);
            if(match.Success) {
                book = new BookInfo(isbn, match.Groups[1].Value, PRODUCT_URL + isbn);
            }
            return book;
        }

        private string SignRequest(string request, string url, string key)
        {
            string httpLessUrl = url.Replace("http://", "");
            string urlPrefix = httpLessUrl.Substring(0, httpLessUrl.IndexOf("/"));
            string urlSuffix = httpLessUrl.Substring(httpLessUrl.IndexOf("/"));

            StringBuilder signRequestBuilder = new StringBuilder();
            signRequestBuilder.Append("GET");
            signRequestBuilder.Append("\n");
            signRequestBuilder.Append(urlPrefix);
            signRequestBuilder.Append("\n");
            signRequestBuilder.Append(urlSuffix);
            signRequestBuilder.Append("\n");
            signRequestBuilder.Append(request.Substring(1));

            HMACSHA256 hmac = new HMACSHA256(Encoding.ASCII.GetBytes(key));
            string signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.ASCII.GetBytes(signRequestBuilder.ToString())));
            
            return signature.Replace("+", "%2B").Replace("=", "%3D");
        }

        private string GenerateTimeStamp()
        {
            return System.DateTime.UtcNow.ToString(AWS_DATATIME_FORMAT).Replace(":", "%3A");
        }
    }
}