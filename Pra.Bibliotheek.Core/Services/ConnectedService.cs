using System;
using System.Collections.Generic;
using System.Data;
using Pra.Bibliotheek.Core.Entities;
using Pra.Bibliotheek.Core.Interfaces;

namespace Pra.Bibliotheek.Core.Services
{
    public class ConnectedService : IBookService
    {

        // CRUD AUTEUR
        public List<Author> GetAuthors()
        {
            List<Author> authors = new List<Author>();
            string sql = "select id, name from author order by name";
            DataTable dataTable = DBService.ExecuteSelect(sql);
            foreach (DataRow dr in dataTable.Rows)
            {
                authors.Add(new Author(dr["id"].ToString(), dr["name"].ToString()));
            }
            return authors;
        }
        public bool AddAuthor(Author author)
        {
            string sql = $"insert into author (id, name) values ('{author.ID}', '{Helper.HandleQuotes(author.Name)}')";
            return DBService.ExecuteCommand(sql);
        }
        public bool UpdateAuthor(Author author)
        {
            string sql = $"update author set name = '{Helper.HandleQuotes(author.Name)}' where id = '{author.ID}'";
            return DBService.ExecuteCommand(sql);
        }
        public bool DeleteAuthor(Author author)
        {
            if (IsAuthorInUse(author))
                return false;
            string sql = $"delete from author where id = '{author.ID}'";
            return DBService.ExecuteCommand(sql);
        }
        public bool IsAuthorInUse(Author author)
        {
            string sql = $"select count(*) from book where authorID = '{author.ID}'";
            string count = DBService.ExecuteScalar(sql);
            return count != null && int.Parse(count) > 0;
        }
        public bool DoesAuthorIDExist(string authorID)
        {
            string sql = $"select count(*) from author where id = '{authorID}'";
            string count = DBService.ExecuteScalar(sql);
            return count != null && int.Parse(count) > 0;
        }
        public Author FindAuthorByName(string name)
        {
            string sql = $"select * from author where name = '{Helper.HandleQuotes(name)}'";
            DataTable dataTable = DBService.ExecuteSelect(sql);
            if (dataTable.Rows.Count > 0)
            {
                return new Author(dataTable.Rows[0]["id"].ToString(), dataTable.Rows[0]["name"].ToString());
            }
            return null;
        }
        public Author FindAuthorByID(string authorID)
        {
            string sql = $"select * from author where id = '{authorID}'";
            DataTable dataTable = DBService.ExecuteSelect(sql);
            if (dataTable.Rows.Count > 0)
            {
                return new Author(dataTable.Rows[0]["id"].ToString(), dataTable.Rows[0]["name"].ToString());
            }
            return null;
        }

        // CRUD UITGEVER
        public List<Publisher> GetPublishers()
        {
            List<Publisher> publishers = new List<Publisher>();
            string sql = $"select id, name from publisher order by name";
            DataTable dataTable = DBService.ExecuteSelect(sql);
            foreach (DataRow dr in dataTable.Rows)
            {
                publishers.Add(new Publisher(dr["id"].ToString(), dr["name"].ToString()));
            }
            return publishers;
        }
        public bool AddPublisher(Publisher publisher)
        {
            string sql = $"insert into publisher (id, name) values ('{publisher.ID}', '{Helper.HandleQuotes(publisher.Name)}')";
            return DBService.ExecuteCommand(sql);
        }
        public bool UpdatePublisher(Publisher publisher)
        {
            string sql = $"update publisher set name = '{Helper.HandleQuotes(publisher.Name)}' where id = '{publisher.ID}'";
            return DBService.ExecuteCommand(sql);
        }
        public bool DeletePublisher(Publisher publisher)
        {
            if (IsPublisherInUse(publisher))
                return false;
            string sql = $"delete from publisher where id = '{publisher.ID}'";
            return DBService.ExecuteCommand(sql);
        }
        public bool IsPublisherInUse(Publisher publisher)
        {
            string sql = $"select count(*) from book where publisherID = '{publisher.ID}'";
            string count = DBService.ExecuteScalar(sql);
            return count != null && int.Parse(count) > 0;
        }
        public bool DoesPublisherIDExist(string publisherID)
        {
            string sql = $"select count(*) from publisher where id = '{publisherID}'";
            string count = DBService.ExecuteScalar(sql);
            return count != null && int.Parse(count) > 0;
        }
        public Publisher FindPublisherByName(string name)
        {
            string sql = $"select * from publisher where name = '{Helper.HandleQuotes(name)}'";
            DataTable dataTable = DBService.ExecuteSelect(sql);
            if (dataTable.Rows.Count > 0)
            {
                return new Publisher(dataTable.Rows[0]["id"].ToString(), dataTable.Rows[0]["name"].ToString());
            }
            return null;
        }
        public Publisher FindPublisherByID(string publisherID)
        {
            string sql = $"select * from publisher where id = '{publisherID}'";
            DataTable dataTable = DBService.ExecuteSelect(sql);
            if (dataTable.Rows.Count > 0)
            {
                return new Publisher(dataTable.Rows[0]["id"].ToString(), dataTable.Rows[0]["name"].ToString());
            }
            return null;
        }

        // CRUD BOEKEN
        public List<Book> GetBooks(Author author = null, Publisher publisher = null)
        {
            List<Book> books = new List<Book>();

            List<string> filters = new List<string>();
            if (author != null)
                filters.Add($"authorID = '{author.ID}'");
            if (publisher != null)
                filters.Add($"publisherID = '{publisher.ID}'");

            string filter = string.Join(" and ", filters);

            string sql = "select * from book " + filter + " order by title";
            DataTable dataTable = DBService.ExecuteSelect(sql);
            foreach (DataRow dr in dataTable.Rows)
            {
                books.Add(new Book(dr["id"].ToString(), dr["title"].ToString(), dr["authorID"].ToString(), dr["publisherID"].ToString(), int.Parse(dr["year"].ToString())));
            }
            return books;
        }
        public bool AddBook(Book book)
        {
            if (!DoesAuthorIDExist(book.AuthorID))
                return false;
            if (!DoesPublisherIDExist(book.PublisherID))
                return false;

            string sql = $"insert into book (id, title, authorID, publisherID, year) values ('{book.ID}', '{Helper.HandleQuotes(book.Title)}' ,'{book.AuthorID}','{book.PublisherID}', {book.Year})";
            return DBService.ExecuteCommand(sql);
        }
        public bool UpdateBook(Book book)
        {
            string sql = $"update book set title = '{Helper.HandleQuotes(book.Title)}', authorID = '{book.AuthorID}', publisherID = '{book.PublisherID}', year = {book.Year} where id = '{book.ID}'";
            return DBService.ExecuteCommand(sql);
        }
        public bool DeleteBook(Book book)
        {
            string sql = $"delete from book where id = '{book.ID}'";
            return DBService.ExecuteCommand(sql);
        }

        public bool SaveData()
        {
            throw new NotImplementedException();
        }
    }
}
