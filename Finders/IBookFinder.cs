using BookSearchAPI.Models;
public interface IBookFinder
{
    BookInfo ExtractBookInfo(string[] isbn);
}