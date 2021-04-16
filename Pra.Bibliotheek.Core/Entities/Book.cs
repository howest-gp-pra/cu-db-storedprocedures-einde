using System;
using System.Collections.Generic;
using System.Text;

namespace Pra.Bibliotheek.Core.Entities
{
    public class Book
    {
        public string ID { get; private set; }
        public string Title { get; set; }
        public string AuthorID { get; set; }
        public string PublisherID { get; set; }
        public int Year { get; set; }
        public Book()
        {
            ID = Guid.NewGuid().ToString();
        }
        public Book(string title, string authorID, string publisherID, int year) : this()
        {
            Title = title;
            AuthorID = authorID;
            PublisherID = publisherID;
            Year = year;
        }
        public Book(string id, string title, string authorID, string publisherID, int year)
        {
            ID = id;
            Title = title;
            AuthorID = authorID;
            PublisherID = publisherID;
            Year = year;
        }
        public override string ToString()
        {
            return $"{Title}";
        }
    }
}
