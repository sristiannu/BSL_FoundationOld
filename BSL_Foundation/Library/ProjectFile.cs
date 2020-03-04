
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
    internal sealed class ProjectFile
    {
        private string _fullFileNamePath;
        private ProjectFileType _projectFileType;
        private string _webAppName;
        private string _apiName;
        private string _webApiName;
        private bool _isUseWebApi;
        private GeneratedSqlType _generatedSqlType;
        private bool _isUseLogging;
        private bool _isUseSecurity;
        private bool _isUseCaching;
        private bool _isUseAuditLogging;
        private bool _isEmailNotification;

        private ProjectFile()
        {
        }

        internal ProjectFile(string fullFileNamePath, ProjectFileType projectFileType, string webAppName, string apiName, string webApiName, bool isUseWebApi, GeneratedSqlType generatedSqlType, bool isUseLogging, bool isUseSecurity, bool isUseCaching, bool isUseAuditLogging, bool isEmailNotification)
        {
            this._fullFileNamePath = fullFileNamePath;
            this._projectFileType = projectFileType;
            this._webAppName = webAppName;
            this._apiName = apiName;
            this._webApiName = webApiName;
            this._isUseWebApi = isUseWebApi;
            this._generatedSqlType = generatedSqlType;
            this._isUseLogging = isUseLogging;
            this._isUseSecurity = isUseSecurity;
            this._isUseCaching = isUseCaching;
            this._isUseAuditLogging = isUseAuditLogging;
            this._isEmailNotification = isEmailNotification;
            this.Generate();
        }

        private void Generate()
        {
            using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
            {
                StringBuilder stringBuilder = new StringBuilder();
                if (this._projectFileType == ProjectFileType.WebApp || this._projectFileType == ProjectFileType.WebAPI)
                    stringBuilder.AppendLine("<Project Sdk=\"Microsoft.NET.Sdk.Web\">");
                else
                    stringBuilder.AppendLine("<Project Sdk=\"Microsoft.NET.Sdk\">");
                stringBuilder.AppendLine("");
                stringBuilder.AppendLine("  <PropertyGroup>");
                stringBuilder.AppendLine("    <TargetFramework>netcoreapp2.1</TargetFramework>");
                stringBuilder.AppendLine("  </PropertyGroup>");
                stringBuilder.AppendLine("");
                if (this._projectFileType == ProjectFileType.WebApp || this._projectFileType == ProjectFileType.WebAPI)
                {
                    stringBuilder.AppendLine("  <ItemGroup>");
                    stringBuilder.AppendLine("    <PackageReference Include=\"Microsoft.AspNetCore.All\" Version=\"2.1.8\" />");
                    stringBuilder.AppendLine("    <PackageReference Include=\"Newtonsoft.Json\" Version=\"12.0.1\" />");
                    stringBuilder.AppendLine("  </ItemGroup>");
                    stringBuilder.AppendLine("");
                    stringBuilder.AppendLine("  <ItemGroup>");
                    if (this._projectFileType == ProjectFileType.WebApp)
                    {
                        stringBuilder.AppendLine("    <PackageReference Include=\"BuildBundlerMinifier\" Version=\"2.8.391\" />");
                    }
                    if (_isUseLogging)
                    {
                        stringBuilder.AppendLine("    <PackageReference Include=\"NLog.Web.AspNetCore\" Version=\"4.8.0\" />");
                        stringBuilder.AppendLine("    <PackageReference Include=\"NLog.WindowsEventLog\" Version=\"4.5.11\" />");
                    }
                    if (_isUseCaching)
                    {
                        stringBuilder.AppendLine("    <PackageReference Include=\"Microsoft.Extensions.Caching.Redis\" Version=\"2.2.0\" />");
                    }
                    if (_isUseAuditLogging)
                    {
                        stringBuilder.AppendLine("    <PackageReference Include=\"CompareNETObjects\" Version=\"4.57.0\" />");
                    }
                    if (_isEmailNotification)
                    {
                        //todo
                    }
                    stringBuilder.AppendLine("  </ItemGroup>");
                    stringBuilder.AppendLine("");
                    stringBuilder.AppendLine("  <ItemGroup>");
                    stringBuilder.AppendLine("    <DotNetCliToolReference Include=\"Microsoft.VisualStudio.Web.CodeGeneration.Tools\" Version=\"2.0.4\" />");
                    stringBuilder.AppendLine("  </ItemGroup>");
                    stringBuilder.AppendLine("");
                    stringBuilder.AppendLine("  <ItemGroup>");
                    stringBuilder.AppendLine("    <ProjectReference Include=\"..\\" + this._apiName + "\\" + this._apiName + ".csproj\" />");
                    if (this._projectFileType == ProjectFileType.WebApp && this._isUseWebApi)
                        stringBuilder.AppendLine("    <ProjectReference Include=\"..\\" + this._webApiName + "\\" + this._webApiName + ".csproj\" />");
                    stringBuilder.AppendLine("  </ItemGroup>");

                    if (_isUseLogging || _isUseSecurity)
                    {
                        stringBuilder.AppendLine("");
                        stringBuilder.AppendLine("  <ItemGroup>");
                        if (_isUseLogging)
                        {
                            stringBuilder.AppendLine("  <Reference Include=\"Application_Components.Logging\">");
                            stringBuilder.AppendLine("    <HintPath>External Dlls\\Application_Components.Logging.dll</HintPath>");
                            stringBuilder.AppendLine("  </Reference>");
                        }
                        if (_isUseSecurity)
                        {
                            stringBuilder.AppendLine("  <Reference Include=\"Application_Components.Security\">");
                            stringBuilder.AppendLine("    <HintPath>External Dlls\\Application_Components.Security.dll</HintPath>");
                            stringBuilder.AppendLine("  </Reference>");
                        }
                        if (_isUseCaching)
                        {
                            stringBuilder.AppendLine("  <Reference Include=\"Application_Components.Caching\">");
                            stringBuilder.AppendLine("    <HintPath>External Dlls\\Application_Components.Caching.dll</HintPath>");
                            stringBuilder.AppendLine("  </Reference>");
                        }
                        if (_isUseAuditLogging)
                        {
                            stringBuilder.AppendLine("  <Reference Include=\"Application_Components.AuditLog\">");
                            stringBuilder.AppendLine("    <HintPath>External Dlls\\Application_Components.AuditLog.dll</HintPath>");
                            stringBuilder.AppendLine("  </Reference>");
                        }
                        if (_isEmailNotification)
                        {
                            stringBuilder.AppendLine("  <Reference Include=\"Application_Components.EmailNotification\">");
                            stringBuilder.AppendLine("    <HintPath>External Dlls\\Application_Components.EmailNotification.dll</HintPath>");
                            stringBuilder.AppendLine("  </Reference>");
                        }
                        stringBuilder.AppendLine("  </ItemGroup>");

                        if (_isUseLogging)
                        {
                            stringBuilder.AppendLine("");
                            stringBuilder.AppendLine("  <ItemGroup>");
                            stringBuilder.AppendLine("  <Content Update=\"nlog.config\">");
                            stringBuilder.AppendLine("    <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>");
                            stringBuilder.AppendLine("  </Content>");
                            stringBuilder.AppendLine("  </ItemGroup>");
                        }
                    }
                }
                else if (this._projectFileType == ProjectFileType.BusinessAndDataAPI && this._generatedSqlType == GeneratedSqlType.EFCore)
                {
                    stringBuilder.AppendLine("  <ItemGroup>");
                    stringBuilder.AppendLine("    <PackageReference Include=\"Microsoft.EntityFrameworkCore.SqlServer\" Version=\"2.1.0\" />");
                    stringBuilder.AppendLine("    <PackageReference Include=\"Microsoft.EntityFrameworkCore.Tools\" Version=\"2.1.0\" />");
                    stringBuilder.AppendLine("  </ItemGroup>");
                }
                else if (this._projectFileType == ProjectFileType.BusinessAndDataAPI && (this._generatedSqlType == GeneratedSqlType.StoredProcedures || this._generatedSqlType == GeneratedSqlType.AdHocSQL))
                {
                    stringBuilder.AppendLine("  <ItemGroup>");
                    stringBuilder.AppendLine("    <PackageReference Include=\"System.Data.SqlClient\" Version=\"4.5.0\" />");
                    stringBuilder.AppendLine("  </ItemGroup>");
                }

                stringBuilder.AppendLine("");
                stringBuilder.AppendLine("</Project>");
                streamWriter.Write(stringBuilder.ToString());
            }
        }
    }
}
