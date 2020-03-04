
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal sealed class DbaseFile
  {
    private string _nameSpace;
    private string _fullFileNamePath;
    private string _server;
    private string _database;
    private string _username;
    private string _password;

    internal DbaseFile()
    {
    }

    internal DbaseFile(string fullFileNamePath, string nameSpace, string server, string database, string username, string password)
    {
      this._fullFileNamePath = fullFileNamePath;
      this._nameSpace = nameSpace;
      this._server = server;
      this._database = database;
      this._username = username;
      this._password = password;
      this.Generate();
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("using System;");
        stringBuilder.AppendLine("using System.Data;");
        stringBuilder.AppendLine("using System.Data.SqlClient;");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("namespace " + this._nameSpace + "." + MyConstants.WordDataLayer);
        stringBuilder.AppendLine("{");
        stringBuilder.AppendLine("     internal sealed class " + MyConstants.WordDbase);
        stringBuilder.AppendLine("     {");
        stringBuilder.AppendLine("         private " + MyConstants.WordDbase + "()");
        stringBuilder.AppendLine("         {");
        stringBuilder.AppendLine("         }");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("         internal static SqlConnection GetConnection()");
        stringBuilder.AppendLine("         {");
        stringBuilder.AppendLine("             string connectionString = @\"Data Source=" + this._server + ";Initial Catalog=" + this._database + ";User ID=" + this._username + ";Password=" + this._password + "\";");
        stringBuilder.AppendLine("             SqlConnection connection = new SqlConnection(connectionString);");
        stringBuilder.AppendLine("             connection.Open();");
        stringBuilder.AppendLine("             return connection;");
        stringBuilder.AppendLine("         }");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("         internal static SqlCommand GetCommand(string storedProcedureName, SqlConnection connection)");
        stringBuilder.AppendLine("         {");
        stringBuilder.AppendLine("             SqlCommand command = new SqlCommand(storedProcedureName, connection);");
        stringBuilder.AppendLine("             command.CommandType = CommandType.StoredProcedure;");
        stringBuilder.AppendLine("             return command;");
        stringBuilder.AppendLine("         }");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("         internal static SqlCommand GetCommand(SqlConnection connection, string sql)");
        stringBuilder.AppendLine("         {");
        stringBuilder.AppendLine("             SqlCommand command = new SqlCommand(sql, connection);");
        stringBuilder.AppendLine("             command.CommandType = CommandType.Text;");
        stringBuilder.AppendLine("             return command;");
        stringBuilder.AppendLine("         }");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("         internal static DataSet GetDbaseDataSet(SqlCommand command)");
        stringBuilder.AppendLine("         {");
        stringBuilder.AppendLine("             SqlDataAdapter adapter = new SqlDataAdapter(command);");
        stringBuilder.AppendLine("             DataSet dataset = ((DataSet)Activator.CreateInstance(typeof(DataSet)));");
        stringBuilder.AppendLine("             adapter.Fill(dataset);");
        stringBuilder.AppendLine("             adapter.Dispose();");
        stringBuilder.AppendLine("             return dataset;");
        stringBuilder.AppendLine("         }");
        stringBuilder.AppendLine("     }");
        stringBuilder.AppendLine("}");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
