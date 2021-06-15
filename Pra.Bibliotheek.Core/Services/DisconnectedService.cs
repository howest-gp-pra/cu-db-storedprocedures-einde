﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Pra.Bibliotheek.Core.Entities;
using Pra.Bibliotheek.Core.Interfaces;

namespace Pra.Bibliotheek.Core.Services
{
    public class DisconnectedService : IBookService
    {
        private DataSet dSBib;
        private DataTable dtAuthors;
        private DataTable dtPublishers;
        private DataTable dtBooks;

        public DisconnectedService()
        {
            dSBib = FileService.ReadFile();
            if (dSBib == null)
            {
                PrepareObjects();
                CreateDataTables();
                DoSeeding();
            }
            else
            {
                ReadExistingData();
            }
        }

        public bool SaveData()
        {
            if (FileService.WriteFile(dSBib))
                return true;
            else
                return false;
        }
        private void ReadExistingData()
        {
            dtAuthors = dSBib.Tables["authors"];
            dtPublishers = dSBib.Tables["publishers"];
            dtBooks = dSBib.Tables["books"];
        }
        private void PrepareObjects()
        {
            dSBib = new DataSet();
            dtAuthors = new DataTable();
            dtPublishers = new DataTable();
            dtBooks = new DataTable();
            dSBib.Tables.Add(dtAuthors);
            dSBib.Tables.Add(dtPublishers);
            dSBib.Tables.Add(dtBooks);
        }
        private void CreateDataTables()
        {
            CreatedtAuthors();
            CreatedtPublishers();
            CreatedtBooks();

            dSBib.Relations.Add(dtAuthors.Columns["ID"], dtBooks.Columns["authorID"]);
            dSBib.Relations.Add(dtPublishers.Columns["ID"], dtBooks.Columns["publisherID"]);

        }
        private void CreatedtAuthors()
        {
            dtAuthors.TableName = "authors";
            DataColumn dc = new DataColumn();
            dc.ColumnName = "ID";
            dc.DataType = typeof(string);
            dc.MaxLength = 50;
            dc.Unique = true;
            dtAuthors.Columns.Add(dc);
            dtAuthors.PrimaryKey = new DataColumn[] { dc };

            dc = new DataColumn();
            dc.ColumnName = "Name";
            dc.DataType = typeof(string);
            dc.MaxLength = 100;
            dtAuthors.Columns.Add(dc);
        }
        private void CreatedtPublishers()
        {
            dtPublishers.TableName = "publishers";
            DataColumn dc = new DataColumn();
            dc.ColumnName = "ID";
            dc.DataType = typeof(string);
            dc.MaxLength = 50;
            dc.Unique = true;
            dtPublishers.Columns.Add(dc);
            dtPublishers.PrimaryKey = new DataColumn[] { dc };

            dc = new DataColumn();
            dc.ColumnName = "Name";
            dc.DataType = typeof(string);
            dc.MaxLength = 100;
            dtPublishers.Columns.Add(dc);
        }
        private void CreatedtBooks()
        {
            dtBooks.TableName = "Books";
            DataColumn dc = new DataColumn();
            dc.ColumnName = "ID";
            dc.DataType = typeof(string);
            dc.MaxLength = 50;
            dc.Unique = true;
            dtBooks.Columns.Add(dc);
            dtBooks.PrimaryKey = new DataColumn[] { dc };

            dc = new DataColumn();
            dc.ColumnName = "Title";
            dc.DataType = typeof(string);
            dc.MaxLength = 100;
            dtBooks.Columns.Add(dc);

            dc = new DataColumn();
            dc.ColumnName = "AuthorID";
            dc.DataType = typeof(string);
            dc.MaxLength = 50;
            dtBooks.Columns.Add(dc);

            dc = new DataColumn();
            dc.ColumnName = "PublisherID";
            dc.DataType = typeof(string);
            dc.MaxLength = 50;
            dtBooks.Columns.Add(dc);

            dc = new DataColumn();
            dc.ColumnName = "Year";
            dc.DataType = typeof(int);
            dtBooks.Columns.Add(dc);
        }
        private void DoSeeding()
        {
            DataRow dr;
            dr = dtAuthors.NewRow();
            dr[0] = Guid.NewGuid().ToString();
            dr[1] = "Boon Louis";
            dtAuthors.Rows.Add(dr);

            dr = dtAuthors.NewRow();
            dr[0] = Guid.NewGuid().ToString();
            dr[1] = "Tuchman Barbara";
            dtAuthors.Rows.Add(dr);

            dr = dtAuthors.NewRow();
            dr[0] = Guid.NewGuid().ToString();
            dr[1] = "Cook Robin";
            dtAuthors.Rows.Add(dr);

            dr = dtPublishers.NewRow();
            dr[0] = Guid.NewGuid().ToString();
            dr[1] = "AW Bruna";
            dtPublishers.Rows.Add(dr);

            dr = dtPublishers.NewRow();
            dr[0] = Guid.NewGuid().ToString();
            dr[1] = "Luttingh";
            dtPublishers.Rows.Add(dr);

            dr = dtBooks.NewRow();
            dr[0] = Guid.NewGuid().ToString();
            dr[1] = "De bende van Jan De Lichte";
            dr[2] = FindAuthorByName("Boon Louis").ID;
            dr[3] = FindPublisherByName("AW Bruna").ID;
            dr[4] = 1951;
            dtBooks.Rows.Add(dr);

            dr = dtBooks.NewRow();
            dr[0] = Guid.NewGuid().ToString();
            dr[1] = "De kannonen van augustus";
            dr[2] = FindAuthorByName("Tuchman Barbara").ID;
            dr[3] = FindPublisherByName("Luttingh").ID;
            dr[4] = 1962;
            dtBooks.Rows.Add(dr);
        }

        // CRUD AUTEUR
        public List<Author> GetAuthors()
        {
            List<Author> authors = new List<Author>();
            //foreach (DataRow dr in dtAuthors.Rows)
            //{
            //    authors.Add(new Author(dr["id"].ToString(), dr["name"].ToString()));
            //}

            authors = (from DataRow dr in dtAuthors.Rows
                       select new Author(dr["id"].ToString(), dr["name"].ToString())
                       ).ToList();

            authors = authors.OrderBy(a => a.Name).ToList();
            return authors;
        }
        public bool AddAuthor(Author author)
        {
            DataRow dr = dtAuthors.NewRow();
            dr["id"] = author.ID;
            dr["name"] = author.Name;
            try
            {
                dtAuthors.Rows.Add(dr);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateAuthor(Author author)
        {
            string search = $"ID = '{author.ID}' ";
            DataRow[] dataRows = dtAuthors.Select(search);
            if (dataRows.Count() == 1)
            {
                try
                {
                    dataRows[0]["name"] = author.Name;
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool DeleteAuthor(Author author)
        {
            if (IsAuthorInUse(author))
                return false;
            string search = $"ID = '{author.ID}' ";
            DataRow[] dataRows = dtAuthors.Select(search);
            if (dataRows.Count() == 1)
            {
                dataRows[0].Delete();
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsAuthorInUse(Author author)
        {
            string search = $"authorID = '{author.ID}' ";
            DataRow[] dataRows = dtBooks.Select(search);
            if (dataRows.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool DoesAuthorIDExist(string authorID)
        {
            string search = $"ID = '{authorID}' ";
            DataRow[] dataRows = dtAuthors.Select(search);
            if (dataRows.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Author FindAuthorByName(string name)
        {
            string search = $"name = '{name}' ";
            DataRow[] dataRows = dtAuthors.Select(search);
            if (dataRows.Count() > 0)
            {
                return new Author(dataRows[0]["id"].ToString(), dataRows[0]["name"].ToString());
            }
            return null;
        }
        public Author FindAuthorByID(string authorID)
        {
            string search = $"ID = '{authorID}' ";
            DataRow[] dataRows = dtAuthors.Select(search);
            if (dataRows.Count() == 1)
            {
                return new Author(dataRows[0]["id"].ToString(), dataRows[0]["name"].ToString());
            }
            return null;
        }

        // CRUD UITGEVER
        public List<Publisher> GetPublishers()
        {
            List<Publisher> publishers = new List<Publisher>();
            foreach (DataRow dr in dtPublishers.Rows)
            {
                publishers.Add(new Publisher(dr["id"].ToString(), dr["name"].ToString()));
            }
            publishers = publishers.OrderBy(a => a.Name).ToList();
            return publishers;
        }
        public bool AddPublisher(Publisher publisher)
        {
            DataRow dr = dtPublishers.NewRow();
            dr["id"] = publisher.ID;
            dr["name"] = publisher.Name;
            try
            {
                dtPublishers.Rows.Add(dr);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool UpdatePublisher(Publisher publisher)
        {
            string search = $"ID = '{publisher.ID}' ";
            DataRow[] dataRows = dtPublishers.Select(search);
            if (dataRows.Count() == 1)
            {
                dataRows[0]["name"] = publisher.Name;
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool DeletePublisher(Publisher publisher)
        {
            if (IsPublisherInUse(publisher))
                return false;
            string search = $"ID = '{publisher.ID}' ";
            DataRow[] dataRows = dtPublishers.Select(search);
            if (dataRows.Count() == 1)
            {
                dataRows[0].Delete();
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsPublisherInUse(Publisher publisher)
        {
            string search = $"publisherID = '{publisher.ID}' ";
            DataRow[] dataRows = dtBooks.Select(search);
            if (dataRows.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool DoesPulblisherIDExist(string publisherID)
        {
            string search = $"ID = '{publisherID}' ";
            DataRow[] dataRows = dtPublishers.Select(search);
            if (dataRows.Count() == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Publisher FindPublisherByName(string name)
        {
            string search = $"name = '{name}' ";
            DataRow[] dataRows = dtPublishers.Select(search);
            if (dataRows.Count() > 0)
            {
                return new Publisher(dataRows[0]["id"].ToString(), dataRows[0]["name"].ToString());
            }
            return null;
        }
        public Publisher FindPublisherByID(string publisherID)
        {
            string search = $"ID = '{publisherID}' ";
            DataRow[] dataRows = dtPublishers.Select(search);
            if (dataRows.Count() == 1)
            {
                return new Publisher(dataRows[0]["id"].ToString(), dataRows[0]["name"].ToString());
            }
            return null;
        }

        // CRUD BOEKEN
        public List<Book> GetBooks(Author author = null, Publisher publisher = null)
        {
            List<Book> books = new List<Book>();

            string filter = "";
            if (author != null)
                filter = $"authorID = '{author.ID}'";
            if (publisher != null)
            {
                if (filter != "")
                    filter += " and ";
                filter = $"publisherID = '{publisher.ID}'";
            }
            DataRow[] dataRows = dtBooks.Select(filter);
            foreach (DataRow dr in dataRows)
            {
                books.Add(new Book(dr["id"].ToString(), dr["title"].ToString(), dr["authorID"].ToString(), dr["publisherID"].ToString(), int.Parse(dr["year"].ToString())));
            }
            books = books.OrderBy(b => b.Title).ToList();
            return books;
        }
        public bool AddBook(Book book)
        {
            if (!DoesAuthorIDExist(book.AuthorID))
                return false;
            if (!DoesPulblisherIDExist(book.PublisherID))
                return false;

            DataRow dr = dtBooks.NewRow();
            dr["id"] = book.ID;
            dr["title"] = book.Title;
            dr["authorID"] = book.AuthorID;
            dr["publisherID"] = book.PublisherID;
            dr["year"] = book.Year;

            try
            {
                dtBooks.Rows.Add(dr);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateBook(Book book)
        {
            string search = $"ID = '{book.ID}' ";
            DataRow[] dataRows = dtBooks.Select(search);
            if (dataRows.Count() == 1)
            {
                dataRows[0]["title"] = book.Title;
                dataRows[0]["authorID"] = book.AuthorID;
                dataRows[0]["publisherID"] = book.PublisherID;
                dataRows[0]["year"] = book.Year;
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool DeleteBook(Book book)
        {
            string search = $"ID = '{book.ID}' ";
            DataRow[] dataRows = dtBooks.Select(search);
            if (dataRows.Count() == 1)
            {
                dataRows[0].Delete();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
