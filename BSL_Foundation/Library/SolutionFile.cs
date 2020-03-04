
using System;
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
    internal class SolutionFile
    {
        private const string projectGuid = "FAE04EC0-301F-11D3-BF4B-00C04F79EFBC";
        private string _fullFileNamePath;
        private bool _isUseWebApp;
        private string _webAppName;
        private string _webAppProjectGuid;
        private string _apiName;
        private string _apiProjectGuid;
        private bool _isUseWebApi;
        private string _webApiName;
        private string _webApiProjectGuid;
        private string _solutionGuid;
        private Language _language;
        private string _projectExtension;

        private SolutionFile()
        {
        }

        internal SolutionFile(string fullFileNamePath, bool isUseWebApp, string webAppName, string webAppProjectGuid, string apiName, string apiProjectGuid, bool isUseWebApi, string webApiName, string webApiProjectGuid, Language language)
        {
            this._fullFileNamePath = fullFileNamePath;
            this._isUseWebApp = isUseWebApp;
            this._webAppName = webAppName;
            this._webAppProjectGuid = webAppProjectGuid;
            this._apiName = apiName;
            this._apiProjectGuid = apiProjectGuid;
            this._isUseWebApi = isUseWebApi;
            this._webApiName = webApiName;
            this._webApiProjectGuid = webApiProjectGuid;
            this._solutionGuid = Guid.NewGuid().ToString().ToUpper();
            this._language = language;
            this._projectExtension = this._language != Language.CSharp ? ".vbproj" : ".csproj";
            this.Generate();
        }

        private void Generate()
        {
            using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("");
                stringBuilder.AppendLine("Microsoft Visual Studio Solution File, Format Version 12.00");
                stringBuilder.AppendLine("# Visual Studio 15");
                stringBuilder.AppendLine("VisualStudioVersion = 15.0.27703.2018");
                stringBuilder.AppendLine("MinimumVisualStudioVersion = 10.0.40219.1");
                if (this._isUseWebApp)
                {
                    stringBuilder.AppendLine("Project(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\") = \"" + this._webAppName + "\", \"" + this._webAppName + "\\" + this._webAppName + this._projectExtension + "\", \"{" + this._webAppProjectGuid + "}\"");
                    stringBuilder.AppendLine("EndProject");
                    stringBuilder.AppendLine("Project(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\") = \"" + this._apiName + "\", \"" + this._apiName + "\\" + this._apiName + this._projectExtension + "\", \"{" + this._apiProjectGuid + "}\"");
                    stringBuilder.AppendLine("EndProject");
                }
                if (this._isUseWebApi)
                {
                    stringBuilder.AppendLine("Project(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\") = \"" + this._webApiName + "\", \"" + this._webApiName + "\\" + this._webApiName + this._projectExtension + "\", \"{" + this._webApiProjectGuid + "}\"");
                    stringBuilder.AppendLine("EndProject");
                }
                if (!this._isUseWebApp && this._isUseWebApi)
                {
                    stringBuilder.AppendLine("Project(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\") = \"" + this._apiName + "\", \"" + this._apiName + "\\" + this._apiName + this._projectExtension + "\", \"{" + this._apiProjectGuid + "}\"");
                    stringBuilder.AppendLine("EndProject");
                }
                stringBuilder.AppendLine("Global");
                stringBuilder.AppendLine("\tGlobalSection(SolutionConfigurationPlatforms) = preSolution");
                stringBuilder.AppendLine("\t\tDebug|Any CPU = Debug|Any CPU");
                stringBuilder.AppendLine("\t\tRelease|Any CPU = Release|Any CPU");
                stringBuilder.AppendLine("\tEndGlobalSection");
                stringBuilder.AppendLine("\tGlobalSection(ProjectConfigurationPlatforms) = postSolution");
                if (this._isUseWebApp)
                {
                    stringBuilder.AppendLine("\t\t{" + this._webAppProjectGuid + "}.Debug|Any CPU.ActiveCfg = Debug|Any CPU");
                    stringBuilder.AppendLine("\t\t{" + this._webAppProjectGuid + "}.Debug|Any CPU.Build.0 = Debug|Any CPU");
                    stringBuilder.AppendLine("\t\t{" + this._webAppProjectGuid + "}.Release|Any CPU.ActiveCfg = Release|Any CPU");
                    stringBuilder.AppendLine("\t\t{" + this._webAppProjectGuid + "}.Release|Any CPU.Build.0 = Release|Any CPU");
                    stringBuilder.AppendLine("\t\t{" + this._apiProjectGuid + "}.Debug|Any CPU.ActiveCfg = Debug|Any CPU");
                    stringBuilder.AppendLine("\t\t{" + this._apiProjectGuid + "}.Debug|Any CPU.Build.0 = Debug|Any CPU");
                    stringBuilder.AppendLine("\t\t{" + this._apiProjectGuid + "}.Release|Any CPU.ActiveCfg = Release|Any CPU");
                    stringBuilder.AppendLine("\t\t{" + this._apiProjectGuid + "}.Release|Any CPU.Build.0 = Release|Any CPU");
                }
                if (this._isUseWebApi)
                {
                    stringBuilder.AppendLine("\t\t{" + this._webApiProjectGuid + "}.Debug|Any CPU.ActiveCfg = Debug|Any CPU");
                    stringBuilder.AppendLine("\t\t{" + this._webApiProjectGuid + "}.Debug|Any CPU.Build.0 = Debug|Any CPU");
                    stringBuilder.AppendLine("\t\t{" + this._webApiProjectGuid + "}.Release|Any CPU.ActiveCfg = Release|Any CPU");
                    stringBuilder.AppendLine("\t\t{" + this._webApiProjectGuid + "}.Release|Any CPU.Build.0 = Release|Any CPU");
                }
                if (!this._isUseWebApp && this._isUseWebApi)
                {
                    stringBuilder.AppendLine("\t\t{" + this._apiProjectGuid + "}.Debug|Any CPU.ActiveCfg = Debug|Any CPU");
                    stringBuilder.AppendLine("\t\t{" + this._apiProjectGuid + "}.Debug|Any CPU.Build.0 = Debug|Any CPU");
                    stringBuilder.AppendLine("\t\t{" + this._apiProjectGuid + "}.Release|Any CPU.ActiveCfg = Release|Any CPU");
                    stringBuilder.AppendLine("\t\t{" + this._apiProjectGuid + "}.Release|Any CPU.Build.0 = Release|Any CPU");
                }
                stringBuilder.AppendLine("\tEndGlobalSection");
                stringBuilder.AppendLine("\tGlobalSection(SolutionProperties) = preSolution");
                stringBuilder.AppendLine("\t\tHideSolutionNode = FALSE");
                stringBuilder.AppendLine("\tEndGlobalSection");
                stringBuilder.AppendLine("\tGlobalSection(ExtensibilityGlobals) = postSolution");
                stringBuilder.AppendLine("\t    SolutionGuid = {" + MyConstants.SolutionGuid + "}");
                stringBuilder.AppendLine("\tEndGlobalSection");
                stringBuilder.AppendLine("EndGlobal");
                streamWriter.Write(stringBuilder.ToString());
            }
        }
    }
}
