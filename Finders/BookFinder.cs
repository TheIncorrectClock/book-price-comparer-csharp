using BookSearchAPI.Models;

namespace BookSearchAPI.Finders
{
    public abstract class BookFinder : IBookFinder
    {
        public BookInfo ExtractBookInfo(string[] isbns) {

            string isbn = RetriveISBN(isbns);

            string responseBody = GetBookPage(isbn);

            return ExtractBookInfoFromBookPage(responseBody, isbn);
        }
        private string RetriveISBN(string[] isbns)
        {
            string isbn = null;
            foreach(var isbnNo in isbns)
            {   
                if(isISBNInProperFormat(isbnNo))
                {
                    isbn = isbnNo;
                    break;
                }
            }
            return isbn;
        }

        abstract protected bool isISBNInProperFormat(string isbn);

        abstract protected string GetBookPage(string isbn);
        
        abstract protected BookInfo ExtractBookInfoFromBookPage(string responseBody, string isbn);
    }
}