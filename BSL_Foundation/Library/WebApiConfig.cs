
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class WebApiConfig
  {
    private string _fullFileNamePath;
    private string _websiteName;
    private Language _language;
    private Tables _selectedTables;

    internal WebApiConfig()
    {
    }

    internal WebApiConfig(Tables selectedTables, string fullFileNamePath, string websiteName, Language language)
    {
      this._fullFileNamePath = fullFileNamePath;
      this._websiteName = websiteName;
      this._language = language;
      this._selectedTables = selectedTables;
      if (this._language == Language.CSharp)
        this.GenerateCodeCS();
      else
        this.GenerateCodeVB();
    }

    private void GenerateCodeCS()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("using System;");
        stringBuilder.AppendLine("using System.Collections.Generic;");
        stringBuilder.AppendLine("using System.Linq;");
        stringBuilder.AppendLine("using System.Web.Http;");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("namespace " + this._websiteName);
        stringBuilder.AppendLine("{");
        stringBuilder.AppendLine("    internal static class WebApiConfig");
        stringBuilder.AppendLine("    {");
        stringBuilder.AppendLine("        internal static void Register(HttpConfiguration config)");
        stringBuilder.AppendLine("        {");
        stringBuilder.AppendLine("            // Web API configuration and services");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            // Web API routes");
        stringBuilder.AppendLine("            config.MapHttpAttributeRoutes();");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            config.Routes.MapHttpRoute(");
        stringBuilder.AppendLine("               name: \"SearchApi\",");
        stringBuilder.AppendLine("               routeTemplate: \"api/{controller}/{action}/{_search}/{nd}/{rows}/{page}/{sidx}/{sord}/{filters}\",");
        stringBuilder.AppendLine("               defaults: new { filters = RouteParameter.Optional }");
        stringBuilder.AppendLine("            );");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            config.Routes.MapHttpRoute(");
        stringBuilder.AppendLine("               name: \"SelectSkipAndTakeApi\",");
        stringBuilder.AppendLine("               routeTemplate: \"api/{controller}/{action}/{sidx}/{sord}/{page}/{rows}\"");
        stringBuilder.AppendLine("            );");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            config.Routes.MapHttpRoute(");
        stringBuilder.AppendLine("                name: \"ActionApi\",");
        stringBuilder.AppendLine("                routeTemplate: \"api/{controller}/{action}/{id}\",");
        stringBuilder.AppendLine("                defaults: new { id = RouteParameter.Optional }");
        stringBuilder.AppendLine("            );");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            config.Routes.MapHttpRoute(");
        stringBuilder.AppendLine("                name: \"DefaultApi\",");
        stringBuilder.AppendLine("                routeTemplate: \"api/{controller}/{id}\",");
        stringBuilder.AppendLine("                defaults: new { id = RouteParameter.Optional }");
        stringBuilder.AppendLine("            );");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.");
        stringBuilder.AppendLine("            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.");
        stringBuilder.AppendLine("            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.");
        stringBuilder.AppendLine("            //config.EnableQuerySupport();");
        stringBuilder.AppendLine("        }");
        stringBuilder.AppendLine("    }");
        stringBuilder.AppendLine("}");
        streamWriter.Write(stringBuilder.ToString());
      }
    }

    private void GenerateCodeVB()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Imports System");
        stringBuilder.AppendLine("Imports System.Collections.Generic");
        stringBuilder.AppendLine("Imports System.Linq");
        stringBuilder.AppendLine("Imports System.Web.Http");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("internal Module WebApiConfig");
        stringBuilder.AppendLine("    internal Sub Register(ByVal config As HttpConfiguration)");
        stringBuilder.AppendLine("        ' Web API configuration and services");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        ' Web API routes");
        stringBuilder.AppendLine("        config.MapHttpAttributeRoutes()");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        config.Routes.MapHttpRoute( _");
        stringBuilder.AppendLine("           name:=\"SearchApi\", _");
        stringBuilder.AppendLine("           routeTemplate:=\"api/{controller}/{action}/{_search}/{nd}/{rows}/{page}/{sidx}/{sord}/{filters}\", _");
        stringBuilder.AppendLine("           defaults:=New With { .filters = RouteParameter.Optional } _");
        stringBuilder.AppendLine("        )");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        config.Routes.MapHttpRoute( _");
        stringBuilder.AppendLine("           name:=\"SelectSkipAndTakeApi\", _");
        stringBuilder.AppendLine("           routeTemplate:=\"api/{controller}/{action}/{sidx}/{sord}/{page}/{rows}\" _");
        stringBuilder.AppendLine("        )");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        config.Routes.MapHttpRoute( _");
        stringBuilder.AppendLine("            name:=\"ActionApi\", _");
        stringBuilder.AppendLine("            routeTemplate:=\"api/{controller}/{action}/{id}\", _");
        stringBuilder.AppendLine("            defaults:=New With { .id = RouteParameter.Optional } _");
        stringBuilder.AppendLine("        )");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        config.Routes.MapHttpRoute( _");
        stringBuilder.AppendLine("            name:=\"DefaultApi\", _");
        stringBuilder.AppendLine("            routeTemplate:=\"api/{controller}/{id}\", _");
        stringBuilder.AppendLine("            defaults:=New With { .id = RouteParameter.Optional } _");
        stringBuilder.AppendLine("        )");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        'Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable(Of T) return type.");
        stringBuilder.AppendLine("        'To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.");
        stringBuilder.AppendLine("        'For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.");
        stringBuilder.AppendLine("        'config.EnableQuerySupport()");
        stringBuilder.AppendLine("    End Sub");
        stringBuilder.AppendLine("End Module");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
