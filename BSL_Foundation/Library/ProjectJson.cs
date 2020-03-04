
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal sealed class ProjectJson
  {
    private string _webAppName;
    private string _apiName;
    private string _webApiName;
    private string _fullFileNamePath;
    private bool _isUseWebApi;
    private ProjectFileType _projectFileType;
    private GeneratedSqlType _generatedSqlType;

    private ProjectJson()
    {
    }

    internal ProjectJson(string webAppName, string apiName, string webApiName, string fullFileNamePath, bool isUseWebApi, ProjectFileType projectFileType, GeneratedSqlType generatedSqlType)
    {
      this._webAppName = webAppName;
      this._apiName = apiName;
      this._webApiName = webApiName;
      this._fullFileNamePath = fullFileNamePath;
      this._isUseWebApi = isUseWebApi;
      this._projectFileType = projectFileType;
      this._generatedSqlType = generatedSqlType;
      this.Generate();
      if (projectFileType != ProjectFileType.WebAPI || !isUseWebApi)
        return;
      this.ReplaceTextForWebAPI();
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("{");
        if (this._generatedSqlType == GeneratedSqlType.EFCore)
        {
          stringBuilder.AppendLine("  \"dependencies\": {");
          stringBuilder.AppendLine("    \"Microsoft.NETCore.App\": {");
          stringBuilder.AppendLine("      \"version\": \"1.0.1\",");
          stringBuilder.AppendLine("      \"type\": \"platform\"");
          stringBuilder.AppendLine("    },");
        }
        else
        {
          stringBuilder.AppendLine("  \"dependencies\": {");
          if (this._isUseWebApi)
            stringBuilder.AppendLine("    \"" + this._webApiName + "\": \"1.0.0-*\",");
        }
        stringBuilder.AppendLine("    \"Microsoft.AspNetCore.Diagnostics\": \"1.0.0\",");
        stringBuilder.AppendLine("    \"Microsoft.AspNetCore.Mvc\": \"1.0.1\",");
        stringBuilder.AppendLine("    \"Microsoft.AspNetCore.Razor.Tools\": {");
        stringBuilder.AppendLine("      \"version\": \"1.0.0-preview2-final\",");
        stringBuilder.AppendLine("      \"type\": \"build\"");
        stringBuilder.AppendLine("    },");
        stringBuilder.AppendLine("    \"Microsoft.AspNetCore.Routing\": \"1.0.1\", ");
        stringBuilder.AppendLine("    \"Microsoft.AspNetCore.Server.IISIntegration\": \"1.0.0\",");
        stringBuilder.AppendLine("    \"Microsoft.AspNetCore.Server.Kestrel\": \"1.0.1\",");
        stringBuilder.AppendLine("    \"Microsoft.AspNetCore.StaticFiles\": \"1.0.0\",");
        stringBuilder.AppendLine("    \"Microsoft.Extensions.Configuration.EnvironmentVariables\": \"1.0.0\",");
        stringBuilder.AppendLine("    \"Microsoft.Extensions.Configuration.Json\": \"1.0.0\",");
        if (this._generatedSqlType == GeneratedSqlType.StoredProcedures || this._generatedSqlType == GeneratedSqlType.AdHocSQL)
          stringBuilder.AppendLine("    \"Microsoft.Extensions.Logging\": \"1.0.0\",");
        else
          stringBuilder.AppendLine("    \"Microsoft.Extensions.Logging\": \"1.1.0\",");
        stringBuilder.AppendLine("    \"Microsoft.Extensions.Logging.Console\": \"1.0.0\",");
        stringBuilder.AppendLine("    \"Microsoft.Extensions.Logging.Debug\": \"1.0.0\",");
        stringBuilder.AppendLine("    \"Microsoft.Extensions.Options.ConfigurationExtensions\": \"1.0.0\",");
        if (this._projectFileType == ProjectFileType.WebApp)
        {
          if (this._generatedSqlType == GeneratedSqlType.EFCore)
          {
            stringBuilder.AppendLine("    \"Microsoft.VisualStudio.Web.BrowserLink.Loader\": \"14.1.0\",");
            stringBuilder.AppendLine("    \"" + this._apiName + "\": \"1.0.0-*\",");
            if (this._isUseWebApi)
              stringBuilder.AppendLine("    \"" + this._webApiName + "\": \"1.0.0-*\",");
          }
          else if (this._isUseWebApi)
            stringBuilder.AppendLine("    \"Microsoft.VisualStudio.Web.BrowserLink.Loader\": \"14.0.0\"");
          else
            stringBuilder.AppendLine("    \"Microsoft.VisualStudio.Web.BrowserLink.Loader\": \"14.0.0\"");
        }
        else
        {
          stringBuilder.AppendLine("    \"Microsoft.VisualStudio.Web.BrowserLink.Loader\": \"14.0.0\",");
          stringBuilder.AppendLine("    \"" + this._apiName + "\": \"1.0.0-*\"");
        }
        stringBuilder.AppendLine("  },");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("  \"tools\": {");
        stringBuilder.AppendLine("    \"BundlerMinifier.Core\": \"2.0.238\",");
        stringBuilder.AppendLine("    \"Microsoft.AspNetCore.Razor.Tools\": \"1.0.0-preview2-final\",");
        stringBuilder.AppendLine("    \"Microsoft.AspNetCore.Server.IISIntegration.Tools\": \"1.0.0-preview2-final\"");
        stringBuilder.AppendLine("  },");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("  \"frameworks\": {");
        if (this._generatedSqlType == GeneratedSqlType.EFCore)
        {
          stringBuilder.AppendLine("    \"netcoreapp1.0\": {");
          stringBuilder.AppendLine("      \"imports\": [");
          stringBuilder.AppendLine("        \"dotnet5.6\",");
          stringBuilder.AppendLine("        \"portable-net45+win8\"");
          stringBuilder.AppendLine("      ]");
        }
        else
        {
          stringBuilder.AppendLine("    \"net461\": {");
          stringBuilder.AppendLine("      \"dependencies\": {");
          stringBuilder.AppendLine("        \"" + this._apiName + "\": {");
          stringBuilder.AppendLine("          \"target\": \"project\"");
          stringBuilder.AppendLine("        }");
          stringBuilder.AppendLine("      }");
        }
        stringBuilder.AppendLine("    }");
        stringBuilder.AppendLine("  },");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("  \"buildOptions\": {");
        stringBuilder.AppendLine("    \"emitEntryPoint\": true,");
        stringBuilder.AppendLine("    \"preserveCompilationContext\": true");
        stringBuilder.AppendLine("  },");
        if (this._generatedSqlType == GeneratedSqlType.EFCore)
        {
          stringBuilder.AppendLine("");
          stringBuilder.AppendLine("  \"runtimeOptions\": {");
          stringBuilder.AppendLine("    \"configProperties\": {");
          stringBuilder.AppendLine("      \"System.GC.Server\": true");
          stringBuilder.AppendLine("    }");
          stringBuilder.AppendLine("  },");
        }
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("  \"publishOptions\": {");
        stringBuilder.AppendLine("    \"include\": [");
        stringBuilder.AppendLine("      \"wwwroot\",");
        stringBuilder.AppendLine("      \"**/*.cshtml\",");
        stringBuilder.AppendLine("      \"appsettings.json\",");
        stringBuilder.AppendLine("      \"web.config\"");
        stringBuilder.AppendLine("    ]");
        stringBuilder.AppendLine("  },");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("  \"scripts\": {");
        stringBuilder.AppendLine("    \"prepublish\": [ \"bower install\", \"dotnet bundle\" ],");
        stringBuilder.AppendLine("    \"postpublish\": [ \"dotnet publish-iis --publish-folder %publish:OutputPath% --framework %publish:FullTargetFramework%\" ]");
        stringBuilder.AppendLine("  }");
        stringBuilder.AppendLine("}");
        streamWriter.Write(stringBuilder.ToString());
      }
    }

    private void ReplaceTextForWebAPI()
    {
      File.WriteAllText(this._fullFileNamePath, File.ReadAllText(this._fullFileNamePath).Replace("[AspCoreGen1-API-Name]", this._apiName));
    }
  }
}
