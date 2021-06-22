using System;
using System.Data;
using System.Data.SqlClient;

namespace Pra.Bibliotheek.Core.Services
{
    class DBService
    {
        public static DataTable ExecuteSelect(string sqlInstruction)
        {
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlInstruction, Helper.GetConnectionString());
            try
            {
                sqlDataAdapter.Fill(dataSet);
            }
            catch
            {
                return null;
            }
            return dataSet.Tables[0];
        }
        public static bool ExecuteCommand(string sqlInstruction)
        {
            SqlConnection sqlConnection = new SqlConnection(Helper.GetConnectionString());
            SqlCommand sqlCommand = new SqlCommand(sqlInstruction, sqlConnection);
            try
            {
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                sqlConnection.Close();
            }
        }


        public static string ExecuteScalar(string sqlScalarInstruction)
        {
            SqlConnection sqlConnection = new SqlConnection(Helper.GetConnectionString());
            SqlCommand sqlCommand = new SqlCommand(sqlScalarInstruction, sqlConnection);
            try
            {
                sqlConnection.Open();
                return sqlCommand.ExecuteScalar().ToString();
            }
            catch
            {
                return null;
            }
            finally
            {
                sqlConnection.Close();
            }
        }
    }
}
