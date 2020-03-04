
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class GlobalAsaxMvcFile
  {
    private Tables _selectedTables;
    private string _fileExtension;
    private string _fullFileNamePath;
    private Language _language;
    private string _languageAbbreviation;
    private string _websiteName;
    private bool _isUseWebApi;

    private GlobalAsaxMvcFile()
    {
    }

    internal GlobalAsaxMvcFile(Tables selectedTables, string fullFileNamePath, Language language, string websiteName, string languageAbbreviation, string fileExtension, ApplicationType appType = ApplicationType.ASPNETMVC5, bool isUseWebApi = false)
    {
      this._selectedTables = selectedTables;
      this._fullFileNamePath = fullFileNamePath;
      this._language = language;
      this._languageAbbreviation = languageAbbreviation;
      this._websiteName = websiteName;
      this._fileExtension = fileExtension;
      this._isUseWebApi = isUseWebApi;
      this.Generate();
      if (this._language == Language.CSharp)
        this.GenerateCodeCSMVC5();
      else
        this.GenerateCodeVBMVC5();
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("<%@ Application Codebehind=\"" + MyConstants.WordGlobalDotAsax + this._fileExtension + "\" Inherits=\"" + this._websiteName + ".MvcApplication\" Language=\"" + this._languageAbbreviation + "\" %>");
        streamWriter.Write(stringBuilder.ToString());
      }
    }

    private void GenerateCodeCSMVC5()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath + this._fileExtension))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("using System;");
        stringBuilder.AppendLine("using System.Collections.Generic;");
        stringBuilder.AppendLine("using System.Linq;");
        stringBuilder.AppendLine("using System.Web;");
        stringBuilder.AppendLine("using System.Web.Http;");
        stringBuilder.AppendLine("using System.Web.Mvc;");
        stringBuilder.AppendLine("using System.Web.Optimization;");
        stringBuilder.AppendLine("using System.Web.Routing;");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("namespace " + this._websiteName);
        stringBuilder.AppendLine("{");
        stringBuilder.AppendLine("    internal class MvcApplication : System.Web.HttpApplication");
        stringBuilder.AppendLine("    {");
        stringBuilder.AppendLine("        protected void Application_Start()");
        stringBuilder.AppendLine("        {");
        stringBuilder.AppendLine("            AreaRegistration.RegisterAllAreas();");
        if (this._isUseWebApi)
          stringBuilder.AppendLine("            GlobalConfiguration.Configure(WebApiConfig.Register);");
        stringBuilder.AppendLine("            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);");
        stringBuilder.AppendLine("            RouteConfig.RegisterRoutes(RouteTable.Routes);");
        stringBuilder.AppendLine("            BundleConfig.RegisterBundles(BundleTable.Bundles);");
        stringBuilder.AppendLine("        }");
        stringBuilder.AppendLine("    }");
        stringBuilder.AppendLine("}");
        streamWriter.Write(stringBuilder.ToString());
      }
    }

    private void GenerateCodeVBMVC5()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath + this._fileExtension))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Imports System.Web.Http");
        stringBuilder.AppendLine("Imports System.Web.Optimization");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("internal Class MvcApplication");
        stringBuilder.AppendLine("    Inherits System.Web.HttpApplication");
        stringBuilder.AppendLine("    Protected Sub Application_Start()");
        stringBuilder.AppendLine("        AreaRegistration.RegisterAllAreas()");
        if (this._isUseWebApi)
          stringBuilder.AppendLine("        GlobalConfiguration.Configure(AddressOf WebApiConfig.Register)");
        stringBuilder.AppendLine("        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters)");
        stringBuilder.AppendLine("        RouteConfig.RegisterRoutes(RouteTable.Routes)");
        stringBuilder.AppendLine("        BundleConfig.RegisterBundles(BundleTable.Bundles)");
        stringBuilder.AppendLine("    End Sub");
        stringBuilder.AppendLine("End Class");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
