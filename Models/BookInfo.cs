namespace BookSearchAPI.Models
{
    public class BookInfo
    {
        private string isbn {
            get;
        }

        private string price {
            get;
        }

        private string url {
            get;
        }

        public BookInfo(string isbn, string price, string url) {
            this.isbn = isbn;
            this.price = price;
            this.url = url;
        }

        override public string ToString() {
            return "[ isbn: " + isbn + " | price: " + price + " | url: " + url + " ]";
        }
    }
}