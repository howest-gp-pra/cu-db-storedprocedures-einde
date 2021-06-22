using System;
using System.Collections.Generic;
using System.Text;

namespace Pra.Bibliotheek.Core.Entities
{
    public class Publisher
    {
        public string ID { get; }
        public string Name { get; set; }

        public Publisher(string name)
        {
            ID = Guid.NewGuid().ToString();
            Name = name;
        }

        public Publisher(string id, string name)
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
