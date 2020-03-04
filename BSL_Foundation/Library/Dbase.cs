
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
    internal class Dbase
    {
        private string _connectionString;
        private string _path;

        internal bool IsSqlVersion2012OrHigher { get; set; }

        internal Dbase()
        {
        }

        internal Dbase(string connectionString)
        {
            this._connectionString = connectionString;
        }

        internal Dbase(string connectionString, string path)
        {
            this._connectionString = connectionString;
            this._path = path;
        }

        internal bool CanConnectToDatabase()
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(this._connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand command = sqlConnection.CreateCommand();
                    command.CommandText = "SELECT SERVERPROPERTY('productversion')";
                    this.IsSqlVersion2012OrHigher = Convert.ToInt32((command.ExecuteScalar() as string).Substring(0, 2).Replace(".", "")) > 10;
                    command.Dispose();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        internal DataSet GetDbaseDataSet(SqlCommand command, SqlConnection connection, string path)
        {
            DataSet dataSet = null;
            try
            {
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command))
                {
                    dataSet = (DataSet)Activator.CreateInstance(typeof(DataSet));
                    sqlDataAdapter.Fill(dataSet);
                    return dataSet;
                }
            }
            catch (Exception ex)
            {
                Functions.WriteToErrorLog(ex.Message, path);
            }
            return dataSet;
        }

        internal DataSet GetDataSet(string sqlTextCommand, bool isWriteErrorToLog = true)
        {
            DataSet dataSet = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Open();
                    using (SqlCommand selectCommand = new SqlCommand(sqlTextCommand, connection))
                    {
                        selectCommand.CommandType = CommandType.Text;
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                        {
                            dataSet = (DataSet)Activator.CreateInstance(typeof(DataSet));
                            sqlDataAdapter.Fill(dataSet);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (isWriteErrorToLog)
                    Functions.WriteToErrorLog(ex.Message, this._path);
                else
                    throw;
            }
            return dataSet;
        }

        internal DataSet GetDataSet(string storedProcedureName, string[] parameterName, string[] parameterValue)
        {
            DataSet dataSet = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Open();
                    using (SqlCommand selectCommand = new SqlCommand(storedProcedureName, connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        for (int index = 0; index < parameterName.Length; ++index)
                            selectCommand.Parameters.AddWithValue(parameterName[index], parameterValue[index]);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                        {
                            dataSet = (DataSet)Activator.CreateInstance(typeof(DataSet));
                            sqlDataAdapter.Fill(dataSet);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Functions.WriteToErrorLog(ex.Message, this._path);
            }
            return dataSet;
        }

        internal static SqlCommand Command(string sql)
        {
            SqlCommand sqlCommand = new SqlCommand(sql);
            sqlCommand.CommandType = CommandType.Text;
            return sqlCommand;
        }

        internal SqlConnection Connect()
        {
            SqlConnection sqlConnection = new SqlConnection(this._connectionString);
            sqlConnection.Open();
            return sqlConnection;
        }

        internal DataSet GetAllColumnsByTable(string tableName, string tableOwner)
        {
            SqlCommand command = Dbase.Command("sp_columns @table_owner = [" + tableOwner + "], @table_name = [" + tableName + "]");
            DataSet dataSet = this.CreateDataSet(command);
            command.Dispose();
            return dataSet;
        }

        internal DataSet GetAllForeignKeysByTable(string tableName, string tableOwner)
        {
            SqlCommand command = Dbase.Command("sp_fkeys @fktable_owner = [" + tableOwner + "], @fktable_name = [" + tableName + "]");
            DataSet dataSet = this.CreateDataSet(command);
            command.Dispose();
            return dataSet;
        }

        internal DataSet CreateDataSet(SqlCommand command)
        {
            DataSet dataSet = null;
            try
            {
                SqlConnection sqlConnection = this.Connect();
                command.Connection = sqlConnection;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
                dataSet = new DataSet();
                dataSet.Locale = CultureInfo.CurrentCulture;
                sqlDataAdapter.Fill(dataSet);
                sqlDataAdapter.Dispose();
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                Functions.WriteToErrorLog(ex.Message, this._path);
            }
            return dataSet;
        }

        internal DataTable CreateDataTable(string sql, bool isWriteErrorToLog = true)
        {
            DataTable dataTable = null;
            SqlConnection selectConnection = null;
            SqlDataAdapter sqlDataAdapter = null;
            try
            {
                selectConnection = this.Connect();
                sqlDataAdapter = new SqlDataAdapter(sql, selectConnection);
                dataTable = new DataTable();
                dataTable.Locale = CultureInfo.CurrentCulture;
                sqlDataAdapter.Fill(dataTable);
                sqlDataAdapter.Dispose();
                selectConnection.Close();
            }
            catch (Exception ex)
            {
                if (isWriteErrorToLog)
                    Functions.WriteToErrorLog(ex.Message, this._path);
                else
                    throw;
            }
            finally
            {
                if (selectConnection != null)
                {
                    selectConnection.Close();
                    selectConnection.Dispose();
                }
                sqlDataAdapter?.Dispose();
            }
            return dataTable;
        }

        internal DataTable GetReferencedTables(string databaseName)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("USE " + databaseName + " ");
            stringBuilder.Append("SELECT f.name AS foreign_key, ");
            stringBuilder.Append("OBJECT_SCHEMA_NAME(f.parent_object_id) AS table_owner, ");
            stringBuilder.Append("OBJECT_NAME(f.parent_object_id) AS table_name, ");
            stringBuilder.Append("COL_NAME(fc.parent_object_id, fc.parent_column_id) AS column_name, ");
            stringBuilder.Append("OBJECT_SCHEMA_NAME(f.referenced_object_id) AS referenced_table_owner, ");
            stringBuilder.Append("OBJECT_NAME (f.referenced_object_id) AS referenced_table_name, ");
            stringBuilder.Append("COL_NAME(fc.referenced_object_id, fc.referenced_column_id) AS referenced_column_name ");
            stringBuilder.Append("FROM sys.foreign_keys AS f ");
            stringBuilder.Append("INNER JOIN sys.foreign_key_columns AS fc ");
            stringBuilder.Append("ON f.OBJECT_ID = fc.constraint_object_id ");
            return this.CreateDataTable(stringBuilder.ToString(), false);
        }

        internal string GetPrimaryKey(string tableName, string tableOwner)
        {
            SqlCommand command = Dbase.Command("sp_pkeys @table_owner = [" + tableOwner + "], @table_name = [" + tableName + "]");
            DataSet dataSet = this.CreateDataSet(command);
            string str = "";
            int num = 0;
            foreach (DataRow row in (InternalDataCollectionBase)dataSet.Tables[0].Rows)
            {
                ++num;
                str += row["column_name"].ToString();
                if (num < dataSet.Tables[0].Rows.Count)
                    str += ",";
            }
            command.Dispose();
            dataSet.Dispose();
            return str;
        }

        internal string GetForeignKey(string tableName, string tableOwner)
        {
            SqlCommand command = Dbase.Command("sp_fkeys @fktable_owner = [" + tableOwner + "], @fktable_name = [" + tableName + "]");
            DataSet dataSet = this.CreateDataSet(command);
            string str = "";
            int num = 0;
            foreach (DataRow row in (InternalDataCollectionBase)dataSet.Tables[0].Rows)
            {
                ++num;
                str += row["fkcolumn_name"].ToString();
                if (num < dataSet.Tables[0].Rows.Count)
                    str += ",";
            }
            command.Dispose();
            dataSet.Dispose();
            return str;
        }

        internal int GetPrimaryKeyCount(string tableName, string tableOwner)
        {
            SqlCommand command = Dbase.Command("sp_pkeys @table_owner = [" + tableOwner + "], @table_name = [" + tableName + "]");
            DataSet dataSet = this.CreateDataSet(command);
            int count = dataSet.Tables[0].Rows.Count;
            command.Dispose();
            dataSet.Dispose();
            return count;
        }

        internal string GetForeignKeyTableName(DataSet dsForeignKeys, string columnName)
        {
            string str = "";
            if (dsForeignKeys != null && dsForeignKeys.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in (InternalDataCollectionBase)dsForeignKeys.Tables[0].Rows)
                {
                    if (row["PKCOLUMN_NAME"].ToString() == columnName)
                    {
                        str = row["PKTABLE_NAME"].ToString();
                        break;
                    }
                }
            }
            return str;
        }

        internal string GetForeignKeyTableOwner(DataSet dsForeignKeys, string columnName)
        {
            string str = "";
            if (dsForeignKeys != null && dsForeignKeys.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in (InternalDataCollectionBase)dsForeignKeys.Tables[0].Rows)
                {
                    if (row["PKCOLUMN_NAME"].ToString() == columnName)
                    {
                        str = row["PKTABLE_OWNER"].ToString();
                        break;
                    }
                }
            }
            return str;
        }

        internal bool IsForeignKey(string tableName, string columnName)
        {
            bool flag = false;
            SqlCommand command = Dbase.Command("SELECT KCU1.COLUMN_NAME AS FK_COLUMN_NAME " + "FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS RC " + "JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU1 " + "ON KCU1.CONSTRAINT_CATALOG = RC.CONSTRAINT_CATALOG  " + "AND KCU1.CONSTRAINT_SCHEMA = RC.CONSTRAINT_SCHEMA " + "AND KCU1.CONSTRAINT_NAME = RC.CONSTRAINT_NAME " + "JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU2 " + "ON KCU2.CONSTRAINT_CATALOG = RC.UNIQUE_CONSTRAINT_CATALOG " + "AND KCU2.CONSTRAINT_SCHEMA = RC.UNIQUE_CONSTRAINT_SCHEMA " + "AND KCU2.CONSTRAINT_NAME = RC.UNIQUE_CONSTRAINT_NAME " + "AND KCU2.ORDINAL_POSITION = KCU1.ORDINAL_POSITION " + "Where KCU1.TABLE_NAME = '" + tableName + "' " + "And KCU1.COLUMN_NAME = '" + columnName + "' ");
            DataSet dataSet = this.CreateDataSet(command);
            if (dataSet.Tables[0].Rows.Count > 0)
                flag = true;
            command.Dispose();
            dataSet.Dispose();
            return flag;
        }

        internal bool IsTableHaveXmlDataType(string tableName, string tableOwner)
        {
            bool flag = false;
            SqlCommand command = Dbase.Command("Select * From [" + tableOwner + "].[" + tableName + "]");
            DataSet dataSet = this.CreateDataSet(command);
            foreach (DataColumn column in dataSet.Tables[0].Columns)
            {
                if (column.DataType.ToString().ToLower(CultureInfo.CurrentCulture) != "xml")
                {
                    flag = true;
                    break;
                }
            }
            command.Dispose();
            dataSet.Dispose();
            return flag;
        }

        internal bool IsLinkTable(string tableName, string tableOwner)
        {
            bool flag1 = true;
            DataSet allColumnsByTable = this.GetAllColumnsByTable(tableName, tableOwner);
            string lower1 = this.GetForeignKey(tableName, tableOwner).ToLower(CultureInfo.CurrentCulture);
            string lower2 = this.GetPrimaryKey(tableName, tableOwner).ToLower(CultureInfo.CurrentCulture);
            foreach (DataRow row in (InternalDataCollectionBase)allColumnsByTable.Tables[0].Rows)
            {
                string[] strArray1 = lower2.Split(",".ToCharArray(), StringSplitOptions.None);
                string[] strArray2 = lower1.Split(",".ToCharArray(), StringSplitOptions.None);
                bool flag2 = false;
                bool flag3 = false;
                foreach (string str in strArray1)
                {
                    if (str.Trim().ToLower(CultureInfo.CurrentCulture) == row["column_name"].ToString().Trim().ToLower(CultureInfo.CurrentCulture))
                    {
                        flag2 = true;
                        break;
                    }
                }
                foreach (string str in strArray2)
                {
                    if (str.Trim().ToLower(CultureInfo.CurrentCulture) == row["column_name"].ToString().Trim().ToLower(CultureInfo.CurrentCulture))
                    {
                        flag3 = true;
                        break;
                    }
                }
                if (!flag2 && !flag3)
                {
                    flag1 = false;
                    break;
                }
            }
            return flag1;
        }

        internal void FindForeignKey(string tableName, string tableOwner, string columnName, out string foreignKeyTableName, out string foreignKeyTableOwner, out bool isForeignKey, out string foreignKeyColumnName)
        {
            isForeignKey = false;
            foreignKeyTableOwner = string.Empty;
            foreignKeyTableName = string.Empty;
            foreignKeyColumnName = string.Empty;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT KCU2.TABLE_NAME AS UNIQUE_TABLE_NAME,  KCU2.TABLE_SCHEMA AS UNIQUE_TABLE_SCHEMA, KCU2.COLUMN_NAME AS UNIQUE_COLUMN_NAME ");
            stringBuilder.Append("FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS RC ");
            stringBuilder.Append("JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU1 ");
            stringBuilder.Append("ON KCU1.CONSTRAINT_CATALOG = RC.CONSTRAINT_CATALOG  ");
            stringBuilder.Append("AND KCU1.CONSTRAINT_SCHEMA = RC.CONSTRAINT_SCHEMA ");
            stringBuilder.Append("AND KCU1.CONSTRAINT_NAME = RC.CONSTRAINT_NAME ");
            stringBuilder.Append("JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU2 ");
            stringBuilder.Append("ON KCU2.CONSTRAINT_CATALOG = RC.UNIQUE_CONSTRAINT_CATALOG ");
            stringBuilder.Append("AND KCU2.CONSTRAINT_SCHEMA = RC.UNIQUE_CONSTRAINT_SCHEMA ");
            stringBuilder.Append("AND KCU2.CONSTRAINT_NAME = RC.UNIQUE_CONSTRAINT_NAME ");
            stringBuilder.Append("AND KCU2.ORDINAL_POSITION = KCU1.ORDINAL_POSITION ");
            stringBuilder.Append("Where KCU1.TABLE_NAME = '" + tableName.Replace("'", "''") + "' ");
            stringBuilder.Append("And KCU1.TABLE_SCHEMA = '" + tableOwner.Replace("'", "''") + "' ");
            stringBuilder.Append("And KCU1.COLUMN_NAME = '" + columnName.Replace("'", "''") + "' ");
            SqlCommand command = Dbase.Command(stringBuilder.ToString());
            DataSet dataSet = this.CreateDataSet(command);
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                isForeignKey = true;
                foreignKeyTableOwner = dataSet.Tables[0].Rows[0]["UNIQUE_TABLE_SCHEMA"].ToString().Trim();
                foreignKeyTableName = dataSet.Tables[0].Rows[0]["UNIQUE_TABLE_NAME"].ToString().Trim();
                foreignKeyColumnName = dataSet.Tables[0].Rows[0]["UNIQUE_COLUMN_NAME"].ToString().Trim();
            }
            command.Dispose();
            dataSet.Dispose();
        }

        internal void CreateStoredProcedure(string sql, string storedProcedureName, string sprocErrorFilePath)
        {
            SqlCommand sqlCommand1 = Dbase.Command(sql);
            SqlCommand sqlCommand2 = Dbase.Command("drop procedure " + storedProcedureName);
            SqlConnection sqlConnection = this.Connect();
            sqlCommand2.Connection = sqlConnection;
            sqlCommand1.Connection = sqlConnection;
            try
            {
                sqlCommand2.ExecuteNonQuery();
            }
            catch
            {
            }
            try
            {
                sqlCommand1.ExecuteNonQuery();
            }
            catch
            {
                try
                {
                    if (!File.Exists(sprocErrorFilePath))
                    {
                        using (new StreamWriter(sprocErrorFilePath))
#pragma warning disable CS0642 // Possible mistaken empty statement
                            ;
#pragma warning restore CS0642 // Possible mistaken empty statement
                    }
                    StreamWriter streamWriter = File.AppendText(sprocErrorFilePath);
                    streamWriter.Write(sql);
                    streamWriter.WriteLine("");
                    streamWriter.WriteLine("");
                    streamWriter.Close();
                    streamWriter.Dispose();
                }
                catch
                {
                }
            }
            sqlCommand1.Dispose();
            sqlConnection.Dispose();
        }

        internal void CreateTable(string sql, string tablenName)
        {
            SqlCommand sqlCommand1 = Dbase.Command(sql);
            SqlCommand sqlCommand2 = Dbase.Command("IF (EXISTS (SELECT * " +
                                                               "FROM INFORMATION_SCHEMA.TABLES " +
                                                               "WHERE TABLE_SCHEMA = 'dbo' " +
                                                               "AND  TABLE_NAME = '" + tablenName + "'))" +
                                                   "BEGIN " +
                                                   "    SELECT 'Exist'" +
                                                   "End " +
                                                   "ELSE " +
                                                   "BEGIN " +
                                                   "    SELECT 'Not Exist'" +
                                                   "END");

            SqlConnection sqlConnection = this.Connect();
            sqlCommand2.Connection = sqlConnection;
            sqlCommand1.Connection = sqlConnection;
            try
            {
                SqlDataReader dr = sqlCommand2.ExecuteReader();
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        if (dr[0].ToString() == "Exist")
                            return;
                    }
                }
                dr.Close();
                sqlCommand1.ExecuteNonQuery();
            }
            catch (Exception) { }
            finally
            {
                sqlCommand2.Dispose();
                sqlCommand1.Dispose();
                sqlConnection.Dispose();
            }
        }

    }
}
