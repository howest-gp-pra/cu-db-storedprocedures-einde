using System;
using System.Collections.Generic;
using System.Data;
using Pra.Bibliotheek.Core.Entities;
using Pra.Bibliotheek.Core.Interfaces;
using System.Data.SqlClient;

namespace Pra.Bibliotheek.Core.Services
{
    public class StoredProceduresService : IBookService
    {
        // CRUD BOEKEN
        public List<Book> GetBooks(Author author = null, Publisher publisher = null)
        {
            string spName = "GetBooks";
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            if (author != null)
            {
                sqlParameters.Add(new SqlParameter("authorID", author.ID));
            }
            if (publisher != null)
            {
                sqlParameters.Add(new SqlParameter("publisherID", publisher.ID));
            }
            DataTable dataTable = DBService.ExecuteSPWithDataTable(spName, sqlParameters);

            if (dataTable == null)
                return null;

            List<Book> books = new List<Book>();
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

            string spName = "AddBook";

            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("id", book.ID),
                new SqlParameter("title", book.Title),
                new SqlParameter("authorID", book.AuthorID),
                new SqlParameter("publisherID", book.PublisherID),
                new SqlParameter("year", book.Year)
            };

            return DBService.ExecuteSP(spName, sqlParameters);
        }
        public bool UpdateBook(Book book)
        {
            string spName = "UpdateBook";

            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("id", book.ID),
                new SqlParameter("title", book.Title),
                new SqlParameter("authorID", book.AuthorID),
                new SqlParameter("publisherID", book.PublisherID),
                new SqlParameter("year", book.Year)
            };

            return DBService.ExecuteSP(spName, sqlParameters);
        }
        public bool DeleteBook(Book book)
        {
            string spName = "DeleteBook";

            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("id", book.ID)
            };

            return DBService.ExecuteSP(spName, sqlParameters);
        }

        // CRUD AUTEUR
        public List<Author> GetAuthors()
        {
            string spName = "GetAuthors";
            DataTable dataTable = DBService.ExecuteSPWithDataTable(spName, null);

            if (dataTable == null)
                return null;

            List<Author> authors = new List<Author>();
            foreach (DataRow dr in dataTable.Rows)
            {
                authors.Add(new Author(dr["id"].ToString(), dr["name"].ToString()));
            }
            return authors;
        }
        public bool AddAuthor(Author author)
        {
            string spName = "AddAuthor";
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("id", author.ID),
                new SqlParameter("name", author.Name)
            };
            return DBService.ExecuteSP(spName, sqlParameters);
        }
        public bool UpdateAuthor(Author author)
        {
            string spName = "UpdateAuthor";
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("id", author.ID),
                new SqlParameter("name", author.Name)
            };
            return DBService.ExecuteSP(spName, sqlParameters);
        }
        public bool DeleteAuthor(Author author)
        {
            string spName = "DeleteAuthor";
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("id", author.ID)
            };
            return DBService.ExecuteSP(spName, sqlParameters);
        }
        public bool IsAuthorInUse(Author author)
        {
            string spName = "IsAuthorInUse";
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("id", author.ID)
            };
            DataTable dataTable = DBService.ExecuteSPWithDataTable(spName, sqlParameters);
            return dataTable != null && int.Parse(dataTable.Rows[0][0].ToString()) > 0;
        }
        public bool DoesAuthorIDExist(string authorID)
        {
            string spName = "DoesAuthorIDExist";
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("id", authorID)
            };
            DataTable dataTable = DBService.ExecuteSPWithDataTable(spName, sqlParameters);
            return dataTable != null && int.Parse(dataTable.Rows[0][0].ToString()) > 0;
        }
        public Author FindAuthorByName(string name)
        {
            string spName = "FindAuthorByName";
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("name", name)
            };
            DataTable dataTable = DBService.ExecuteSPWithDataTable(spName, sqlParameters);
            if (dataTable == null || dataTable.Rows.Count == 0)
                return null;
            else
                return new Author(dataTable.Rows[0]["id"].ToString(), dataTable.Rows[0]["name"].ToString());
        }
        public Author FindAuthorByID(string authorID)
        {
            string spName = "FindAuthorByName";
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("id", authorID)
            };
            DataTable dataTable = DBService.ExecuteSPWithDataTable(spName, sqlParameters);
            if (dataTable == null || dataTable.Rows.Count == 0)
                return null;
            else
                return new Author(dataTable.Rows[0]["id"].ToString(), dataTable.Rows[0]["name"].ToString());
        }

        // CRUD UITGEVER
        public List<Publisher> GetPublishers()
        {
            string spName = "GetPublishers";
            DataTable dataTable = DBService.ExecuteSPWithDataTable(spName, null);

            if (dataTable == null)
                return null;

            List<Publisher> publishers = new List<Publisher>();
            foreach (DataRow dr in dataTable.Rows)
            {
                publishers.Add(new Publisher(dr["id"].ToString(), dr["name"].ToString()));
            }
            return publishers;
        }
        public bool AddPublisher(Publisher publisher)
        {
            string spName = "AddPublisher";
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("id", publisher.ID),
                new SqlParameter("name", publisher.Name)
            };
            return DBService.ExecuteSP(spName, sqlParameters);
        }
        public bool UpdatePublisher(Publisher publisher)
        {
            string spName = "UpdatePublisher";
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("id", publisher.ID),
                new SqlParameter("name", publisher.Name)
            };
            return DBService.ExecuteSP(spName, sqlParameters);
        }
        public bool DeletePublisher(Publisher publisher)
        {
            string spName = "DeletePublisher";
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("id", publisher.ID)
            };
            return DBService.ExecuteSP(spName, sqlParameters);
        }
        public bool IsPublisherInUse(Publisher publisher)
        {
            string spName = "IsPublisherInUse";
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("id", publisher.ID)
            };
            DataTable dataTable = DBService.ExecuteSPWithDataTable(spName, sqlParameters);
            return dataTable != null && int.Parse(dataTable.Rows[0][0].ToString()) > 0;

        }
        public bool DoesPublisherIDExist(string publisherID)
        {
            string spName = "DoesPublisherIDExist";
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("id", publisherID)
            };
            DataTable dataTable = DBService.ExecuteSPWithDataTable(spName, sqlParameters);
            return dataTable != null && int.Parse(dataTable.Rows[0][0].ToString()) > 0;
        }
        public Publisher FindPublisherByName(string name)
        {
            string spName = "FindPublisherByName";
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("name", name)
            };
            DataTable dataTable = DBService.ExecuteSPWithDataTable(spName, sqlParameters);
            if (dataTable == null || dataTable.Rows.Count == 0)
                return null;
            else
                return new Publisher(dataTable.Rows[0]["id"].ToString(), dataTable.Rows[0]["name"].ToString());
        }
        public Publisher FindPublisherByID(string publisherID)
        {
            string spName = "FindPublisherByID";
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("id", publisherID)
            };
            DataTable dataTable = DBService.ExecuteSPWithDataTable(spName, sqlParameters);
            if (dataTable == null || dataTable.Rows.Count == 0)
                return null;
            else
                return new Publisher(dataTable.Rows[0]["id"].ToString(), dataTable.Rows[0]["name"].ToString());
        }

    }
}

