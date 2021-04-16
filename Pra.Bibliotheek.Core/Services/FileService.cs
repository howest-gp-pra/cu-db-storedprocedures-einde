using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;

namespace Pra.Bibliotheek.Core.Services
{
    class FileService
    {
        private static string XMLFile = Directory.GetCurrentDirectory() + "/books.xml";

        public static DataSet ReadFile()
        {
            if (File.Exists(XMLFile))
            {
                try
                {
                    DataSet dataSet = new DataSet();
                    dataSet.ReadXml(XMLFile, XmlReadMode.ReadSchema);
                    return dataSet;
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public static bool WriteFile(DataSet dataSet)
        {
            if (File.Exists(XMLFile))
            {
                try
                {
                    File.Delete(XMLFile);
                }
                catch
                {
                    return false;
                }
            }
            try
            {
                dataSet.WriteXml(XMLFile, XmlWriteMode.WriteSchema);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
