using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace EcomAPI.DBCommon
{

    public sealed class SQLHelper
    {

        #region Utility Methods & Constructors
        public SQLHelper()
        {

        }

        public static string GetConnection()
        {
            IConfiguration configuration = new ConfigurationBuilder()
     .SetBasePath(Directory.GetCurrentDirectory())
     .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
     .AddEnvironmentVariables()
     .Build();

            string strCon = configuration.GetConnectionString("DefaultConnection");
            return strCon;
        }

        public static void PrepareCommand(SqlConnection SqlConnection, SqlCommand SqlCommand, string commandText, SqlParameter[] cmdParams)
        {
            SqlCommand.Connection = SqlConnection;
            SqlCommand.CommandText = commandText;

            SqlCommand.CommandType = CommandType.StoredProcedure;
            if (cmdParams != null)
            {
                foreach (SqlParameter p in cmdParams)
                {
                    if (p != null)
                    {
                        // Check for derived output value with no value assigned
                        if ((p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Input) && (p.Value == null))
                            p.Value = DBNull.Value;
                        SqlCommand.Parameters.Add(p);
                    }
                }
            }
        }
        #endregion

        #region ExecuteNonQuery
        public static int ExecuteNonQuery(string commandText)
        {
            return ExecuteNonQuery(commandText, (SqlParameter[])null);
        }

        public static int ExecuteNonQuery(string commandText, params SqlParameter[] cmdParams)
        {
            SqlCommand SqlCommand = new SqlCommand();
            SqlConnection SqlConnection = new SqlConnection(GetConnection());
            SqlConnection.Open();
            PrepareCommand(SqlConnection, SqlCommand, commandText, cmdParams);
            int retval = SqlCommand.ExecuteNonQuery();
            SqlConnection.Close();
            SqlConnection.Dispose();
            SqlCommand.Parameters.Clear();
            SqlCommand.Dispose();
            return retval;
        }
        #endregion

        #region ExecuteDataset
        public static DataSet ExecuteDataset(string commandText)
        {
            return ExecuteDataset(commandText, (SqlParameter[])null);
        }

        public static DataSet ExecuteDataset(string commandText, params SqlParameter[] cmdParams)
        {
            SqlCommand SqlCommand = new SqlCommand();
            SqlConnection SqlConnection = new SqlConnection(GetConnection());
            PrepareCommand(SqlConnection, SqlCommand, commandText, cmdParams);
            DataSet DataSet;
            using (SqlDataAdapter da = new SqlDataAdapter(SqlCommand))
            {
                DataSet = new DataSet();
                da.Fill(DataSet);
                SqlCommand.Parameters.Clear();
            }
            SqlConnection.Dispose();
            return DataSet;
        }
        #endregion


    }


}
