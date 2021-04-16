using System;
using System.Collections.Generic;
using System.Text;

namespace Pra.Bibliotheek.Core.Services
{
    class Helper
    {
        public static string GetConnectionString()
        {
            return @"Data Source=(local)\SQLEXPRESS;Initial Catalog=Books; Integrated security=true;";
        }
        public static string HandleQuotes(string value)
        {
            return value.Trim().Replace("'", "''");
        }
    }
}
