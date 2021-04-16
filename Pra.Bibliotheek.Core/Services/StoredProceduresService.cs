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
            List<Book> books = new List<Book>();

            string spName = "GetBooks";
            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = null;
            if (author != null)
            {
                sqlParameters[0] = new SqlParameter();
                sqlParameters[0].ParameterName = "authorID";
                sqlParameters[0].Value = author.ID;
            }
            sqlParameters[1] = null;
            if (publisher != null)
            {
                sqlParameters[1] = new SqlParameter();
                sqlParameters[1].ParameterName = "publisherID";
                sqlParameters[1].Value = author.ID;
            }
            DataTable dataTable = DBService.ExecuteSPWithDataTable(spName, sqlParameters);
            foreach (DataRow dr in dataTable.Rows)
            {
                books.Add(new Book(dr["id"].ToString(), dr["title"].ToString(), dr["authorID"].ToString(), dr["publisherID"].ToString(), int.Parse(dr["year"].ToString())));
            }
            return books;
        }
        public bool AddBook(Book book)
        {
            if (!DoesAuthorIDExists(book.AuthorID))
                return false;
            if (!DoesPulblisherIDExists(book.PublisherID))
                return false;

            string spName = "AddBook";

            SqlParameter[] sqlParameters = new SqlParameter[5];
            sqlParameters[0] = new SqlParameter();
            sqlParameters[0].ParameterName = "id";
            sqlParameters[0].Value = book.ID;
            sqlParameters[1] = new SqlParameter();
            sqlParameters[1].ParameterName = "title";
            sqlParameters[1].Value = book.Title;
            sqlParameters[2] = new SqlParameter();
            sqlParameters[2].ParameterName = "authorID";
            sqlParameters[2].Value = book.AuthorID;
            sqlParameters[3] = new SqlParameter();
            sqlParameters[3].ParameterName = "publisherID";
            sqlParameters[3].Value = book.PublisherID;
            sqlParameters[4] = new SqlParameter();
            sqlParameters[4].ParameterName = "year";
            sqlParameters[4].Value = book.Year;

            return DBService.ExecuteSP(spName, sqlParameters);
        }
        public bool UpdateBook(Book book)
        {
            string spName = "UpdateBook";

            SqlParameter[] sqlParameters = new SqlParameter[5];
            sqlParameters[0] = new SqlParameter();
            sqlParameters[0].ParameterName = "id";
            sqlParameters[0].Value = book.ID;
            sqlParameters[1] = new SqlParameter();
            sqlParameters[1].ParameterName = "title";
            sqlParameters[1].Value = book.Title;
            sqlParameters[2] = new SqlParameter();
            sqlParameters[2].ParameterName = "authorID";
            sqlParameters[2].Value = book.AuthorID;
            sqlParameters[3] = new SqlParameter();
            sqlParameters[3].ParameterName = "publisherID";
            sqlParameters[3].Value = book.PublisherID;
            sqlParameters[4] = new SqlParameter();
            sqlParameters[4].ParameterName = "year";
            sqlParameters[4].Value = book.Year;

            return DBService.ExecuteSP(spName, sqlParameters);
        }
        public bool DeleteBook(Book book)
        {
            string spName = "DeleteBook";

            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter();
            sqlParameters[0].ParameterName = "id";
            sqlParameters[0].Value = book.ID;

            return DBService.ExecuteSP(spName, sqlParameters);
        }

        // CRUD AUTEUR
        public List<Author> GetAuthors()
        {
            List<Author> authors = new List<Author>();
            string spName = "GetAuthors";
            DataTable dataTable = DBService.ExecuteSPWithDataTable(spName, null);
            foreach (DataRow dr in dataTable.Rows)
            {
                authors.Add(new Author(dr["id"].ToString(), dr["name"].ToString()));
            }
            return authors;
        }
        public bool AddAuthor(Author author)
        {
            string spName = "AddAuthor";
            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter();
            sqlParameters[0].ParameterName = "id";
            sqlParameters[0].Value = author.ID;
            sqlParameters[1] = new SqlParameter();
            sqlParameters[1].ParameterName = "name";
            sqlParameters[1].Value = author.Name;
            return DBService.ExecuteSP(spName, sqlParameters);
        }
        public bool UpdateAuthor(Author author)
        {
            string spName = "UpdateAuthor";
            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter();
            sqlParameters[0].ParameterName = "id";
            sqlParameters[0].Value = author.ID;
            sqlParameters[1] = new SqlParameter();
            sqlParameters[1].ParameterName = "name";
            sqlParameters[1].Value = author.Name;
            return DBService.ExecuteSP(spName, sqlParameters);
        }
        public bool DeleteAuthor(Author author)
        {
            string spName = "DeleteAuthor";
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter();
            sqlParameters[0].ParameterName = "id";
            sqlParameters[0].Value = author.ID;
            return DBService.ExecuteSP(spName, sqlParameters);
        }
        public bool IsAuthorInUse(Author author)
        {
            string spName = "IsAuthorInUse";
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter();
            sqlParameters[0].ParameterName = "id";
            sqlParameters[0].Value = author.ID;
            DataTable dataTable = DBService.ExecuteSPWithDataTable(spName, sqlParameters);
            if (dataTable == null)
                return false;
            else if (int.Parse(dataTable.Rows[0][0].ToString()) == 0)
                return false;
            else
                return true;
        }
        public bool DoesAuthorIDExists(string authorID)
        {
            string spName = "DoesAuthorIDExists";
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter();
            sqlParameters[0].ParameterName = "id";
            sqlParameters[0].Value = authorID;
            DataTable dataTable = DBService.ExecuteSPWithDataTable(spName, sqlParameters);
            if (dataTable == null)
                return false;
            else if (int.Parse(dataTable.Rows[0][0].ToString()) == 0)
                return false;
            else
                return true;
        }
        public Author FindAuthorByName(string name)
        {
            string spName = "FindAuthorByName";
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter();
            sqlParameters[0].ParameterName = "name";
            sqlParameters[0].Value = name;
            DataTable dataTable = DBService.ExecuteSPWithDataTable(spName, sqlParameters);
            if (dataTable == null)
                return null;
            else if (dataTable.Rows.Count == 0)
                return null;
            else
                return new Author(dataTable.Rows[0]["id"].ToString(), dataTable.Rows[0]["name"].ToString());
        }
        public Author FindAuthorByID(string authorID)
        {
            string spName = "FindAuthorByName";
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter();
            sqlParameters[0].ParameterName = "id";
            sqlParameters[0].Value = authorID;
            DataTable dataTable = DBService.ExecuteSPWithDataTable(spName, sqlParameters);
            if (dataTable == null)
                return null;
            else if (dataTable.Rows.Count == 0)
                return null;
            else
                return new Author(dataTable.Rows[0]["id"].ToString(), dataTable.Rows[0]["name"].ToString());
        }

        // CRUD UITGEVER
        public List<Publisher> GetPublishers()
        {
            List<Publisher> publishers = new List<Publisher>();
            string spName = "GetPublishers";
            DataTable dataTable = DBService.ExecuteSPWithDataTable(spName, null);
            foreach (DataRow dr in dataTable.Rows)
            {
                publishers.Add(new Publisher(dr["id"].ToString(), dr["name"].ToString()));
            }
            return publishers;
        }
        public bool AddPublisher(Publisher publisher)
        {
            string spName = "AddPublisher";
            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter();
            sqlParameters[0].ParameterName = "id";
            sqlParameters[0].Value = publisher.ID;
            sqlParameters[1] = new SqlParameter();
            sqlParameters[1].ParameterName = "name";
            sqlParameters[1].Value = publisher.Name;
            return DBService.ExecuteSP(spName, sqlParameters);
        }
        public bool UpdatePublisher(Publisher publisher)
        {
            string spName = "UpdatePublisher";
            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter();
            sqlParameters[0].ParameterName = "id";
            sqlParameters[0].Value = publisher.ID;
            sqlParameters[1] = new SqlParameter();
            sqlParameters[1].ParameterName = "name";
            sqlParameters[1].Value = publisher.Name;
            return DBService.ExecuteSP(spName, sqlParameters);
        }
        public bool DeletePublisher(Publisher publisher)
        {
            string spName = "DeletePublisher";
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter();
            sqlParameters[0].ParameterName = "id";
            sqlParameters[0].Value = publisher.ID;
            return DBService.ExecuteSP(spName, sqlParameters);
        }
        public bool IsPublisherInUse(Publisher publisher)
        {
            string spName = "IsPublisherInUse";
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter();
            sqlParameters[0].ParameterName = "id";
            sqlParameters[0].Value = publisher.ID;
            DataTable dataTable = DBService.ExecuteSPWithDataTable(spName, sqlParameters);
            if (dataTable == null)
                return false;
            else if (int.Parse(dataTable.Rows[0][0].ToString()) == 0)
                return false;
            else
                return true;
        }
        public bool DoesPulblisherIDExists(string publisherID)
        {
            string spName = "DoesPulblisherIDExists";
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter();
            sqlParameters[0].ParameterName = "id";
            sqlParameters[0].Value = publisherID;
            DataTable dataTable = DBService.ExecuteSPWithDataTable(spName, sqlParameters);
            if (dataTable == null)
                return false;
            else if (int.Parse(dataTable.Rows[0][0].ToString()) == 0)
                return false;
            else
                return true;
        }
        public Publisher FindPublisherByName(string name)
        {
            string spName = "FindPublisherByName";
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter();
            sqlParameters[0].ParameterName = "name";
            sqlParameters[0].Value = name;
            DataTable dataTable = DBService.ExecuteSPWithDataTable(spName, sqlParameters);
            if (dataTable == null)
                return null;
            else if (dataTable.Rows.Count == 0)
                return null;
            else
                return new Publisher(dataTable.Rows[0]["id"].ToString(), dataTable.Rows[0]["name"].ToString());
        }
        public Publisher FindPublisherByID(string publisherID)
        {
            string spName = "FindPublisherByID";
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter();
            sqlParameters[0].ParameterName = "id";
            sqlParameters[0].Value = publisherID;
            DataTable dataTable = DBService.ExecuteSPWithDataTable(spName, sqlParameters);
            if (dataTable == null)
                return null;
            else if (dataTable.Rows.Count == 0)
                return null;
            else
                return new Publisher(dataTable.Rows[0]["id"].ToString(), dataTable.Rows[0]["name"].ToString());
        }

        public bool SaveData()
        {
            throw new NotImplementedException();
        }
    }
}

