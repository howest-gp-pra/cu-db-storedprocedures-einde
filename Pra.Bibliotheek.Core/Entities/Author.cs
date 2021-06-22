using System;
using System.Collections.Generic;
using System.Text;

namespace Pra.Bibliotheek.Core.Entities
{
    public class Author
    {
        public string ID { get; }
        public string Name { get; set; }

        public Author(string name)
        {
            ID = Guid.NewGuid().ToString();
            Name = name;
        }

        public Author(string id, string name)
        {
            ID = id;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
