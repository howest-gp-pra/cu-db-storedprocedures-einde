using System;
using System.Collections.Generic;
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

        public static bool ExecuteSP(string spName, List<SqlParameter> arguments)
        {
            SqlConnection sqlConnection = new SqlConnection(Helper.GetConnectionString());
            SqlCommand sqlCommand = new SqlCommand(spName, sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter sqlParameter in arguments)
            {
                sqlCommand.Parameters.Add(sqlParameter);
            }

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
        public static DataTable ExecuteSPWithDataTable(string spName, List<SqlParameter> arguments)
        {
            SqlConnection sqlConnection = new SqlConnection(Helper.GetConnectionString());
            SqlCommand sqlCommand = new SqlCommand(spName, sqlConnection);
            SqlDataReader sqlDataReader;
            sqlCommand.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter sqlParameter in arguments)
            {
                sqlCommand.Parameters.Add(sqlParameter);
            }

            try
            {
                sqlConnection.Open();
                sqlDataReader = sqlCommand.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(sqlDataReader);
                return dataTable;
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
