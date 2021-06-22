using System.Collections.Generic;
using Pra.Bibliotheek.Core.Entities;

namespace Pra.Bibliotheek.Core.Interfaces
{
    public interface IBookService
    {
        List<Author> GetAuthors();
        bool AddAuthor(Author author);
        bool UpdateAuthor(Author author);
        bool DeleteAuthor(Author author);
        bool IsAuthorInUse(Author author);
        bool DoesAuthorIDExist(string authorID);
        Author FindAuthorByName(string name);
        Author FindAuthorByID(string authorID);

        List<Publisher> GetPublishers();
        bool AddPublisher(Publisher publisher);
        bool UpdatePublisher(Publisher publisher);
        bool DeletePublisher(Publisher publisher);
        bool IsPublisherInUse(Publisher publisher);
        bool DoesPublisherIDExist(string publisherID);
        Publisher FindPublisherByName(string name);
        Publisher FindPublisherByID(string publisherID);

        List<Book> GetBooks(Author author, Publisher publisher);
        bool AddBook(Book book);
        bool UpdateBook(Book book);
        bool DeleteBook(Book book);
    }
}
