
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal sealed class ProjectJsonLock
  {
    private string _webAppName;
    private string _apiName;
    private string _webApiName;
    private string _fullFileNamePath;
    private bool _isUseWebApi;
    private GeneratedSqlType _generatedSqlType;
    private string _fileExtension;

    internal ProjectJsonLock(string webAppName, string apiName, string webApiName, string fullFileNamePath, bool isUseWebApi, ProjectFileType projectFileType, GeneratedSqlType generatedSqlType, string fileExtension)
    {
      this._webAppName = webAppName;
      this._apiName = apiName;
      this._webApiName = webApiName;
      this._fullFileNamePath = fullFileNamePath;
      this._isUseWebApi = isUseWebApi;
      this._generatedSqlType = generatedSqlType;
      this._fileExtension = fileExtension;
      switch (projectFileType)
      {
        case ProjectFileType.WebApp:
          this.ReplaceTextForWebApp();
          break;
        case ProjectFileType.WebAPI:
          if (!isUseWebApi)
            break;
          this.ReplaceTextForWebAPI();
          break;
      }
    }

    private void ReplaceTextForWebApp()
    {
      string str = File.ReadAllText(this._fullFileNamePath);
      if (this._isUseWebApi)
        str = this._generatedSqlType != GeneratedSqlType.EFCore ? str.Replace("[AspCoreGen1-WebAPI-Name]", this._webApiName) : str.Replace("[AspCoreGen1-WebAPI-Framework]", this.WebAppFramework()).Replace("[AspCoreGen1-WebAPI-Project]", this.WebAppProject()).Replace("[AspCoreGen1-WebAPI-ProjectFileDependencyGroups]", this.WebAppProjectFileDependencyGroups());
      string contents = str.Replace("[AspCoreGen1-API-Name]", this._apiName);
      if (this._generatedSqlType == GeneratedSqlType.StoredProcedures || this._generatedSqlType == GeneratedSqlType.AdHocSQL)
        contents = contents.Replace("[ClassicProjectExtension]", this._fileExtension + "proj");
      File.WriteAllText(this._fullFileNamePath, contents);
    }

    private string WebAppFramework()
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (this._isUseWebApi)
      {
        stringBuilder.AppendLine("      },");
        stringBuilder.AppendLine("      \"" + this._webApiName + "/1.0.0\": {");
        stringBuilder.AppendLine("              \"type\": \"project\",");
        stringBuilder.AppendLine("              \"framework\": \".NETCoreApp,Version=v1.0\",");
        stringBuilder.AppendLine("              \"dependencies\": {");
        stringBuilder.AppendLine("                \"Core11API\": \"1.0.0\",");
        stringBuilder.AppendLine("                \"Microsoft.AspNetCore.Mvc\": \"1.0.1\",");
        stringBuilder.AppendLine("                \"Microsoft.AspNetCore.Routing\": \"1.0.1\",");
        stringBuilder.AppendLine("                \"Microsoft.AspNetCore.Server.IISIntegration\": \"1.0.0\",");
        stringBuilder.AppendLine("                \"Microsoft.AspNetCore.Server.Kestrel\": \"1.0.1\",");
        stringBuilder.AppendLine("                \"Microsoft.Extensions.Configuration.EnvironmentVariables\": \"1.0.0\",");
        stringBuilder.AppendLine("                \"Microsoft.Extensions.Configuration.FileExtensions\": \"1.0.0\",");
        stringBuilder.AppendLine("                \"Microsoft.Extensions.Configuration.Json\": \"1.0.0\",");
        stringBuilder.AppendLine("                \"Microsoft.Extensions.Logging\": \"1.1.0\",");
        stringBuilder.AppendLine("                \"Microsoft.Extensions.Logging.Console\": \"1.0.0\",");
        stringBuilder.AppendLine("                \"Microsoft.Extensions.Logging.Debug\": \"1.0.0\",");
        stringBuilder.AppendLine("                \"Microsoft.Extensions.Options.ConfigurationExtensions\": \"1.0.0\",");
        stringBuilder.AppendLine("                \"Microsoft.NETCore.App\": \"1.0.1\"");
        stringBuilder.AppendLine("              },");
        stringBuilder.AppendLine("              \"compile\": {");
        stringBuilder.AppendLine("                \"netcoreapp1.0/" + this._webApiName + ".dll\": {}");
        stringBuilder.AppendLine("              },");
        stringBuilder.AppendLine("              \"runtime\": {");
        stringBuilder.AppendLine("                \"netcoreapp1.0/" + this._webApiName + ".dll\": {}");
        stringBuilder.AppendLine("              }");
        stringBuilder.AppendLine("            }");
        stringBuilder.AppendLine("      }");
      }
      else
        stringBuilder.AppendLine("      }");
      return stringBuilder.ToString();
    }

    private string WebAppClassicFramework()
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (this._isUseWebApi)
      {
        stringBuilder.AppendLine("      },");
        stringBuilder.AppendLine("      \"" + this._webApiName + "/1.0.0\": {");
        stringBuilder.AppendLine("              \"type\": \"project\",");
        stringBuilder.AppendLine("              \"framework\": \".NETCoreApp,Version=v1.0\",");
        stringBuilder.AppendLine("              \"dependencies\": {");
        stringBuilder.AppendLine("                \"Core11API\": \"1.0.0\",");
        stringBuilder.AppendLine("                \"Microsoft.AspNetCore.Mvc\": \"1.0.1\",");
        stringBuilder.AppendLine("                \"Microsoft.AspNetCore.Routing\": \"1.0.1\",");
        stringBuilder.AppendLine("                \"Microsoft.AspNetCore.Server.IISIntegration\": \"1.0.0\",");
        stringBuilder.AppendLine("                \"Microsoft.AspNetCore.Server.Kestrel\": \"1.0.1\",");
        stringBuilder.AppendLine("                \"Microsoft.Extensions.Configuration.EnvironmentVariables\": \"1.0.0\",");
        stringBuilder.AppendLine("                \"Microsoft.Extensions.Configuration.FileExtensions\": \"1.0.0\",");
        stringBuilder.AppendLine("                \"Microsoft.Extensions.Configuration.Json\": \"1.0.0\",");
        stringBuilder.AppendLine("                \"Microsoft.Extensions.Logging\": \"1.1.0\",");
        stringBuilder.AppendLine("                \"Microsoft.Extensions.Logging.Console\": \"1.0.0\",");
        stringBuilder.AppendLine("                \"Microsoft.Extensions.Logging.Debug\": \"1.0.0\",");
        stringBuilder.AppendLine("                \"Microsoft.Extensions.Options.ConfigurationExtensions\": \"1.0.0\",");
        stringBuilder.AppendLine("                \"Microsoft.NETCore.App\": \"1.0.1\"");
        stringBuilder.AppendLine("              },");
        stringBuilder.AppendLine("              \"compile\": {");
        stringBuilder.AppendLine("                \"netcoreapp1.0/" + this._webApiName + ".dll\": {}");
        stringBuilder.AppendLine("              },");
        stringBuilder.AppendLine("              \"runtime\": {");
        stringBuilder.AppendLine("                \"netcoreapp1.0/" + this._webApiName + ".dll\": {}");
        stringBuilder.AppendLine("              }");
        stringBuilder.AppendLine("            }");
        stringBuilder.AppendLine("      }");
      }
      else
        stringBuilder.AppendLine("      }");
      return stringBuilder.ToString();
    }

    private string WebAppProject()
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (this._isUseWebApi)
      {
        stringBuilder.AppendLine("    },");
        stringBuilder.AppendLine("    \"" + this._webApiName + "/1.0.0\": {");
        stringBuilder.AppendLine("      \"type\": \"project\",");
        stringBuilder.AppendLine("      \"path\": \"../" + this._webApiName + "/project.json\",");
        stringBuilder.AppendLine("      \"msbuildProject\": \"../" + this._webApiName + "/" + this._webApiName + ".xproj\"");
        stringBuilder.AppendLine("    }");
      }
      else
        stringBuilder.AppendLine("    }");
      return stringBuilder.ToString();
    }

    private string WebAppProjectFileDependencyGroups()
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (this._isUseWebApi)
      {
        stringBuilder.AppendLine("      \"" + this._webApiName + " >= 1.0.0-*\",");
        stringBuilder.AppendLine("      \"Microsoft.AspNetCore.Diagnostics >= 1.0.0\",");
      }
      else
        stringBuilder.AppendLine("      \"Microsoft.AspNetCore.Diagnostics >= 1.0.0\",");
      return stringBuilder.ToString();
    }

    private void ReplaceTextForWebAPI()
    {
      File.WriteAllText(this._fullFileNamePath, File.ReadAllText(this._fullFileNamePath).Replace("[AspCoreGen1-API-Name]", this._apiName));
    }
  }
}
