﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd
{
    public class DataAccessHelpers
    {
        private static string _tag = "[DataAccessHelpers]";
        private static int _commandTimeout = 36000;

        private static string ConnectionString()
        {
            string tag = _tag + "[ConnectionString]";
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;

                return connectionString;
            }
            catch (Exception ex)
            {
                LogHelpers.LogHandler.Error(ex.ToString());
                return string.Empty;
            }
        }

        private static SqlCommand CreateSqlCommandQuery(SqlConnection conn, string query, Hashtable hashTable)
        {
            string tag = _tag + "[CreateSqlCommandQuery]";

            try
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandTimeout = _commandTimeout;

                if (hashTable == null || hashTable.Count == 0)
                {
                    return cmd;
                }

                foreach (DictionaryEntry entry in hashTable)
                {
                    if (entry.Key == null || string.IsNullOrEmpty(entry.Key.ToString()))
                    {
                        continue;
                    }

                    string key = "@" + entry.Key.ToString();
                    object value = entry.Value == null ? DBNull.Value : entry.Value;

                    SqlParameter para = new SqlParameter(key, value);
                    cmd.Parameters.Add(para);
                }

                return cmd;
            }
            catch (Exception ex)
            {
                LogHelpers.LogHandler.Error(ex.ToString());
                return null;
            }
        }

        private static SqlCommand CreateSqlCommandStored(SqlConnection conn, string stored, Hashtable hashTable)
        {
            string tag = _tag + "[CreateSqlCommandStored]";

            try
            {
                SqlCommand cmd = new SqlCommand(stored, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = _commandTimeout;

                if (hashTable == null || hashTable.Count == 0)
                {
                    return cmd;
                }

                foreach (DictionaryEntry entry in hashTable)
                {
                    if (entry.Key == null || string.IsNullOrEmpty(entry.Key.ToString()))
                    {
                        continue;
                    }

                    string key = "@" + entry.Key.ToString();
                    object value = entry.Value == null ? DBNull.Value : entry.Value;

                    SqlParameter para = new SqlParameter(key, value);
                    cmd.Parameters.Add(para);
                }

                return cmd;
            }
            catch (Exception ex)
            {
                LogHelpers.LogHandler.Error(ex.ToString());

                return null;
            }
        }

        public static ResultData<DataSet> ExecuteQuery(string query)
        {
            string tag = _tag + "[ExecuteQuery]";
            try
            {
                SqlConnection conn = new SqlConnection(ConnectionString());
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandTimeout = _commandTimeout;
                conn.Open();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                da.Dispose();

                cmd.Dispose();
                conn.Close();

                return new ResultData<DataSet>() { 
                    Code = 0,
                    Data = ds
                };
            }
            catch (Exception ex)
            {
                LogHelpers.LogHandler.Error(ex.ToString());

                return new ResultData<DataSet>()
                { 
                    Code = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                };
            }
        }

        public static ResultData<DataSet> ExecuteQuery(string query, object obj)
        {
            string tag = _tag + "[ExecuteQuery]";

            try
            {
                SqlConnection conn = new SqlConnection(ConnectionString());
                SqlCommand cmd = CreateSqlCommandQuery(conn, query, Utilities.GetParameters(obj));
                conn.Open();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                cmd.Dispose();
                conn.Close();

                return new ResultData<DataSet>()
                {
                    Code = 0,
                    Data = ds
                };
            }
            catch (Exception ex)
            {
                LogHelpers.LogHandler.Error(ex.ToString());

                return new ResultData<DataSet>()
                {
                    Code = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                };
            }
        }

        public static ResultData<DataSet> ExecuteQuery(string query, Hashtable hashTable)
        {
            string tag = _tag + "[ExecuteQuery]";

            try
            {
                SqlConnection conn = new SqlConnection(ConnectionString());
                SqlCommand cmd = CreateSqlCommandQuery(conn, query, hashTable);
                conn.Open();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                cmd.Dispose();
                conn.Close();

                return new ResultData<DataSet>() { 
                    Code = 0,
                    Data = ds
                };
            }
            catch (Exception ex)
            {
                LogHelpers.LogHandler.Error(ex.ToString());

                return new ResultData<DataSet>() { 
                    Code = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                };
            }
        }

        public static ResultData<int> ExecuteQueryReturnValue(string query, object obj)
        {
            string tag = _tag + "[ExecuteQuery]";

            try
            {
                SqlConnection conn = new SqlConnection(ConnectionString());
                SqlCommand cmd = CreateSqlCommandQuery(conn, query, Utilities.GetParameters(obj));
                conn.Open();

                int result = cmd.ExecuteNonQuery();

                cmd.Dispose();
                conn.Close();

                return new ResultData<int>()
                {
                    Code = result
                };
            }
            catch (Exception ex)
            {
                LogHelpers.LogHandler.Error(ex.ToString());

                return new ResultData<int>()
                {
                    Code = ex.HResult,
                    ErrorMessage = ex.Message
                };
            }
        }

        public static ResultData<object> ExecuteScalar(string query)
        {
            string tag = _tag + "[ExecuteScalar]";

            try
            {
                SqlConnection conn = new SqlConnection(ConnectionString());
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandTimeout = _commandTimeout;
                conn.Open();

                object result = cmd.ExecuteScalar();

                cmd.Dispose();
                conn.Close();

                return new ResultData<object>() { 
                    Code = 0,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                LogHelpers.LogHandler.Error(ex.ToString());

                return new ResultData<object>() { 
                    Code = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                };
            }
        }

        public static ResultData<object> ExecuteScalar(string query, Hashtable hashTable)
        {
            string tag = _tag + "[ExcuteScalar]";

            try
            {
                SqlConnection conn = new SqlConnection(ConnectionString());
                SqlCommand cmd = CreateSqlCommandQuery(conn, query, hashTable);

                conn.Open();

                object result = cmd.ExecuteScalar();

                cmd.Dispose();
                conn.Close();

                return new ResultData<object>() { 
                    Code = 0,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                LogHelpers.LogHandler.Error(ex.ToString());

                return new ResultData<object>() { 
                    Code = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                };
            }
        }

        public static ResultData<int> ExecuteStored(string stored, Hashtable hashTable)
        {
            string tag = _tag + "[ExecuteStored]";

            try
            {
                SqlConnection conn = new SqlConnection(ConnectionString());
                SqlCommand cmd = CreateSqlCommandStored(conn, stored, hashTable);

                SqlParameter dbReturn = cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                dbReturn.Direction = ParameterDirection.ReturnValue;

                conn.Open();

                int exec = cmd.ExecuteNonQuery();

                cmd.Dispose();
                conn.Close();
                conn.Dispose();

                return new ResultData<int>() { 
                    Code = (int)dbReturn.Value,
                    DbReturnValue = (int)dbReturn.Value
                };
            }
            catch (Exception ex)
            {
                LogHelpers.LogHandler.Error(ex.ToString());

                return new ResultData<int>() { 
                    Code = ex.HResult,
                    ErrorMessage = ex.Message
                };
            }
        }

        public static ResultData<int> ExecuteStored(string stored, object obj)
        {
            string tag = _tag + "[ExecuteStored]";

            try
            {
                SqlConnection conn = new SqlConnection(ConnectionString());
                SqlCommand cmd = CreateSqlCommandStored(conn, stored, Utilities.GetParameters(obj));

                SqlParameter dbReturn = cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                dbReturn.Direction = ParameterDirection.ReturnValue;

                conn.Open();

                int exec = cmd.ExecuteNonQuery();

                cmd.Dispose();
                conn.Close();
                conn.Dispose();

                return new ResultData<int>()
                {
                    Code = (int)dbReturn.Value,
                    DbReturnValue = (int)dbReturn.Value
                };
            }
            catch (Exception ex)
            {
                LogHelpers.LogHandler.Error(ex.ToString());

                return new ResultData<int>()
                {
                    Code = ex.HResult,
                    ErrorMessage = ex.Message
                };
            }
        }

        public static ResultData<DataSet> ExecuteStoredReturnDataSet(string stored, Hashtable hashTable)
        {
            string tag = _tag + "[ExecuteStoredReturnDataSet]";

            try
            {
                SqlConnection conn = new SqlConnection(ConnectionString());
                SqlCommand cmd = CreateSqlCommandStored(conn, stored, hashTable);

                SqlParameter dbReturn = cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                dbReturn.Direction = ParameterDirection.ReturnValue;

                conn.Open();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                cmd.Dispose();
                conn.Close();

                return new ResultData<DataSet>() { 
                    Code = 0,
                    DbReturnValue = (int)dbReturn.Value,
                    Data = ds
                };
            }
            catch (Exception ex)
            {
                LogHelpers.LogHandler.Error(ex.ToString());

                return new ResultData<DataSet>() { 
                    Code = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                };
            }
        }

        public static ResultData<DataSet> ExecuteStoredReturnDataSet(string stored, Object obj)
        {
            string tag = _tag + "[ExecuteStoredReturnDataSet]";

            try
            {
                SqlConnection conn = new SqlConnection(ConnectionString());
                SqlCommand cmd = CreateSqlCommandStored(conn, stored, Utilities.GetParameters(obj));

                SqlParameter dbReturn = cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                dbReturn.Direction = ParameterDirection.ReturnValue;

                conn.Open();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                cmd.Dispose();
                conn.Close();

                return new ResultData<DataSet>()
                {
                    Code = 0,
                    DbReturnValue = (int)dbReturn.Value,
                    Data = ds
                };
            }
            catch (Exception ex)
            {
                LogHelpers.LogHandler.Error(ex.ToString());

                return new ResultData<DataSet>()
                {
                    Code = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                };
            }
        }
    }
}
