
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class AppConfigFile
  {
    private string _fullFileNamePath;
    private string _server;
    private string _database;
    private string _username;
    private string _password;

    private AppConfigFile()
    {
    }

    internal AppConfigFile(string fullFileNamePath, string server, string database, string username, string password)
    {
      this._fullFileNamePath = fullFileNamePath;
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
        stringBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        stringBuilder.AppendLine("<configuration>");
        stringBuilder.AppendLine("  <startup>");
        stringBuilder.AppendLine("    <supportedRuntime version=\"v4.0\" sku=\".NETFramework,Version=v4.5.1\" />");
        stringBuilder.AppendLine("  </startup>");
        stringBuilder.AppendLine("  <connectionStrings>");
        stringBuilder.AppendLine("    <add name=\"ConnectionString\"");
        stringBuilder.AppendLine("      connectionString=\"Data Source=" + this._server + ";Initial Catalog=" + this._database + ";User ID=" + this._username + ";Password=" + this._password + "\"");
        stringBuilder.AppendLine("      providerName=\"System.Data.SqlClient\" />");
        stringBuilder.AppendLine("  </connectionStrings>");
        stringBuilder.AppendLine("</configuration>");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
