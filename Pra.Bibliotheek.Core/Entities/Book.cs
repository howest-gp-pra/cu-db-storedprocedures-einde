using System;
using System.Collections.Generic;
using System.Text;

namespace Pra.Bibliotheek.Core.Entities
{
    public class Book
    {
        public string ID { get; }
        public string Title { get; set; }
        public string AuthorID { get; set; }
        public string PublisherID { get; set; }
        public int Year { get; set; }

        public Book(string title, string authorID, string publisherID, int year)
        {
            ID = Guid.NewGuid().ToString();
            Title = title;
            AuthorID = authorID;
            PublisherID = publisherID;
            Year = year;
        }

        public Book(string id, string title, string authorID, string publisherID, int year)
            : this(title, authorID, publisherID, year)
        {
            ID = id;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
