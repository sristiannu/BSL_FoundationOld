
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal sealed class AppSettingsJson
  {
    private string _fullFileNamePath;
    private string _webAppName;

    private AppSettingsJson()
    {
    }

    internal AppSettingsJson(string fullFileNamePath, string webAppName)
    {
      this._fullFileNamePath = fullFileNamePath;
      this._webAppName = webAppName;
      this.Generate();
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("{");
        stringBuilder.AppendLine("  \"AppSettings\": {");
        stringBuilder.AppendLine("    \"WebApiBaseAddress\": \"\",");
        stringBuilder.AppendLine("    \"GridNumberOfPagesToShow\": 10,");
        stringBuilder.AppendLine("    \"GridNumberOfRows\": 15");
        stringBuilder.AppendLine("  },");
        stringBuilder.AppendLine("  \"Data\": {");
        stringBuilder.AppendLine("    \"DefaultConnection\": {");
        stringBuilder.AppendLine("      \"ConnectionString\": \"Server=(localdb)\\\\mssqllocaldb;Database=" + this._webAppName + "d-10b515d5-af17-48bc-96f4-6d6adee6abab;Trusted_Connection=True;MultipleActiveResultSets=true\"");
        stringBuilder.AppendLine("    }");
        stringBuilder.AppendLine("  },");
        stringBuilder.AppendLine("  \"Logging\": {");
        stringBuilder.AppendLine("    \"IncludeScopes\": false,");
        stringBuilder.AppendLine("    \"LogLevel\": {");
        stringBuilder.AppendLine("      \"Default\": \"Verbose\",");
        stringBuilder.AppendLine("      \"System\": \"Information\",");
        stringBuilder.AppendLine("      \"Microsoft\": \"Information\"");
        stringBuilder.AppendLine("    }");
        stringBuilder.AppendLine("  }");
        stringBuilder.AppendLine("}");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
