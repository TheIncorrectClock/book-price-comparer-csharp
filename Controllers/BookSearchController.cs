using BookSearchAPI.Finders;
using BookSearchAPI.Models;
using Microsoft.AspNetCore.Mvc;
using BookSearchAPI.Providers;

namespace BookSearchAPI.Controllers
{
    [Route("BookSearch")]
    public class BookSearchController : Controller
    {
        private BookSearchWebAppConfiguration config;

        public BookSearchController(BookSearchWebAppConfiguration config)
        {
            this.config = config;
        }

        [HttpGet("{title}")]
        public string SearchForBook(string title)
        {
            System.Console.WriteLine();

            ISBNFinder finder = new ISBNFinder();
            string[] isbns = finder.FindBookInRemoteDatabase(title);

            IBookFinder amazonFinder;
            if(bool.Parse(config.GetValue("use_aws")))
            {
                amazonFinder = new AmazonAWSBookFinder(AmazonAWSRequestDataProvider.Provide("D:\\workspace\\csharp\\book-price-comparer-csharp\\Resources\\aws_data.cfg"));
            }
            else
            {
                amazonFinder = new AmazonBookFinder();
            }
            BookInfo amazonBook = amazonFinder.ExtractBookInfo(isbns);

            IBookFinder apressFinder = new ApressBookFinder();
            BookInfo apressBook = apressFinder.ExtractBookInfo(isbns);

            return "apress: " + apressBook.ToString() + " | amazon: " + amazonBook.ToString();
        }

    }

}
