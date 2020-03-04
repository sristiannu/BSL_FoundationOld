
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal sealed class AppSettingsClass
  {
    private string _fullFileNamePath;
    private string _apiName;
    private string _server;
    private string _database;
    private string _username;
    private string _password;

    private AppSettingsClass()
    {
    }

    internal AppSettingsClass(string fullFileNamePath, string apiName, string server, string database, string username, string password)
    {
      this._fullFileNamePath = fullFileNamePath;
      this._apiName = apiName;
      this._server = server.Replace("\"", "\\\"");
      this._database = database.Replace("\"", "\\\"");
      this._username = username.Replace("\"", "\\\"");
      this._password = password.Replace("\"", "\\\"");
      this.Generate();
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("namespace " + this._apiName);
        stringBuilder.AppendLine("{");
        stringBuilder.AppendLine("    internal sealed class AppSettings");
        stringBuilder.AppendLine("    {");
        stringBuilder.AppendLine("        private AppSettings()");
        stringBuilder.AppendLine("        {");
        stringBuilder.AppendLine("        }");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        internal static string GetConnectionString()");
        stringBuilder.AppendLine("        {");
        stringBuilder.AppendLine("            return \"Data Source=" + this._server + ";Initial Catalog=" + this._database + ";User ID=" + this._username + ";Password=" + this._password + "\";");
        stringBuilder.AppendLine("        }");
        stringBuilder.AppendLine("    }");
        stringBuilder.AppendLine("}");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
