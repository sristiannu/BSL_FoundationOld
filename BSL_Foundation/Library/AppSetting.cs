
using System;
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal sealed class AppSetting
  {
    private string _fullFileNamePath;
    private string _webAppName;
    private Language _language;
    private string _apiName;
    private string _apiNameDirectory;
    private string _server;
    private string _database;
    private string _username;
    private string _password;

    private AppSetting()
    {
    }

    internal AppSetting(string fullFileNamePath, string webAppName, Language language, string apiName, string apiNameDirectory, string server, string database, string username, string password)
    {
      this._fullFileNamePath = fullFileNamePath;
      this._webAppName = webAppName;
      this._language = language;
      this._apiName = apiName;
      this._apiNameDirectory = apiNameDirectory;
      this._server = server;
      this._database = database;
      this._username = username;
      this._password = password;
      if (language == Language.CSharp)
        this.GenerateCS();
      else
        this.GenerateVB();
    }

    private void GenerateCS()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("namespace " + this._webAppName);
        stringBuilder.AppendLine("{");
        stringBuilder.AppendLine("    public class AppSettings");
        stringBuilder.AppendLine("    {");
        stringBuilder.AppendLine("        public string WebApiBaseAddress { get; set; }");
        stringBuilder.AppendLine("        public int GridNumberOfRows { get; set; }");
        stringBuilder.AppendLine("        public int GridNumberOfPagesToShow { get; set; }");
        stringBuilder.AppendLine("    }");
        stringBuilder.AppendLine("}");
        streamWriter.Write(stringBuilder.ToString());
      }
      this.GenerateTempAppSettingFile();
    }

    private void GenerateTempAppSettingFile()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._apiNameDirectory + "\\AppSetting.cs"))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("namespace " + this._apiName);
        stringBuilder.AppendLine("{");
        stringBuilder.AppendLine("    public class AppSetting");
        stringBuilder.AppendLine("    {");
        stringBuilder.AppendLine("        public static string GetConnectionString()");
        stringBuilder.AppendLine("        {");
        stringBuilder.AppendLine("            return \"Data Source=" + this._server + ";Initial Catalog=" + this._database + ";User ID=" + this._username + ";Password=" + this._password + "\";");
        stringBuilder.AppendLine("        }");
        stringBuilder.AppendLine("    }");
        stringBuilder.AppendLine("}");
        streamWriter.Write(stringBuilder.ToString());
      }
    }

    private void GenerateVB()
    {
      throw new NotImplementedException();
    }
  }
}
