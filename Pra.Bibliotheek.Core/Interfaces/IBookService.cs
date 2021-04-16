using System;
using System.Collections.Generic;
using System.Text;
using Pra.Bibliotheek.Core.Entities;

namespace Pra.Bibliotheek.Core.Interfaces
{
    public interface IBookService
    {
        public bool SaveData();

        public List<Author> GetAuthors();
        public bool AddAuthor(Author author);
        public bool UpdateAuthor(Author author);
        public bool DeleteAuthor(Author author);
        public bool IsAuthorInUse(Author author);
        public bool DoesAuthorIDExists(string authorID);
        public Author FindAuthorByName(string name);
        public Author FindAuthorByID(string authorID);

        public List<Publisher> GetPublishers();
        public bool AddPublisher(Publisher publisher);
        public bool UpdatePublisher(Publisher publisher);
        public bool DeletePublisher(Publisher publisher);
        public bool IsPublisherInUse(Publisher publisher);
        public bool DoesPulblisherIDExists(string publisherID);
        public Publisher FindPublisherByName(string name);
        public Publisher FindPublisherByID(string publisherID);

        public List<Book> GetBooks(Author author = null, Publisher publisher = null);
        public bool AddBook(Book book);
        public bool UpdateBook(Book book);
        public bool DeleteBook(Book book);
    }
}

