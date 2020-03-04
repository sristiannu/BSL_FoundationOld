
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class RouteConfig
  {
    private string _fullFileNamePath;
    private string _websiteName;
    private Tables _selectedTables;

    internal RouteConfig()
    {
    }

    internal RouteConfig(Tables selectedTables, string fullFileNamePath, Language language, string websiteName = "", ApplicationType appType = ApplicationType.ASPNET45)
    {
      this._fullFileNamePath = fullFileNamePath;
      this._websiteName = websiteName;
      this._selectedTables = selectedTables;
      if (appType == ApplicationType.ASPNETMVC5)
      {
        if (language == Language.CSharp)
          this.GenerateCodeCSMVC5();
        else
          this.GenerateCodeVBMVC5();
      }
      else if (language == Language.CSharp)
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
        stringBuilder.AppendLine("using System.Web;");
        stringBuilder.AppendLine("using System.Web.Routing;");
        stringBuilder.AppendLine("using Microsoft.AspNet.FriendlyUrls;");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("namespace ASP");
        stringBuilder.AppendLine("{");
        stringBuilder.AppendLine("    internal static class RouteConfig");
        stringBuilder.AppendLine("    {");
        stringBuilder.AppendLine("        internal static void RegisterRoutes(RouteCollection routes)");
        stringBuilder.AppendLine("        {");
        stringBuilder.AppendLine("            routes.EnableFriendlyUrls();");
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
        stringBuilder.AppendLine("Imports System.Web");
        stringBuilder.AppendLine("Imports System.Web.Routing");
        stringBuilder.AppendLine("Imports Microsoft.AspNet.FriendlyUrls");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("Namespace ASP");
        stringBuilder.AppendLine("    internal NotInheritable Class RouteConfig");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        Private Sub New()");
        stringBuilder.AppendLine("        End Sub");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        internal Shared Sub RegisterRoutes(routes As RouteCollection)");
        stringBuilder.AppendLine("            routes.EnableFriendlyUrls()");
        stringBuilder.AppendLine("        End Sub");
        stringBuilder.AppendLine("    End Class");
        stringBuilder.AppendLine("End Namespace");
        streamWriter.Write(stringBuilder.ToString());
      }
    }

    private void GenerateCodeCSMVC5()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("using System;");
        stringBuilder.AppendLine("using System.Collections.Generic;");
        stringBuilder.AppendLine("using System.Linq;");
        stringBuilder.AppendLine("using System.Web;");
        stringBuilder.AppendLine("using System.Web.Mvc;");
        stringBuilder.AppendLine("using System.Web.Routing;");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("namespace " + this._websiteName);
        stringBuilder.AppendLine("{");
        stringBuilder.AppendLine("    internal class RouteConfig");
        stringBuilder.AppendLine("    {");
        stringBuilder.AppendLine("        internal static void RegisterRoutes(RouteCollection routes)");
        stringBuilder.AppendLine("        {");
        stringBuilder.AppendLine("            routes.IgnoreRoute(\"{resource}.axd/{*pathInfo}\");");
        stringBuilder.AppendLine("");
        foreach (Table selectedTable in (List<Table>) this._selectedTables)
        {
          if (selectedTable.PrimaryKeyCount > 1 && !selectedTable.IsContainsPrimaryAndForeignKeyColumnsOnly)
          {
            string primaryKeysInCurlies = Functions.GetSlashDelimitedPrimaryKeysInCurlies(selectedTable);
            stringBuilder.AppendLine("            routes.MapRoute(");
            stringBuilder.AppendLine("                \"" + selectedTable.Name + "\",");
            stringBuilder.AppendLine("                \"" + selectedTable.Name + "/{action}" + primaryKeysInCurlies + "\",");
            stringBuilder.AppendLine("                new { controller = \"" + selectedTable.Name + "\", action = \"Index\" }");
            stringBuilder.AppendLine("            );");
            stringBuilder.AppendLine("");
          }
        }
        stringBuilder.AppendLine("            routes.MapRoute(");
        stringBuilder.AppendLine("                name: \"Default\",");
        stringBuilder.AppendLine("                url: \"{controller}/{action}/{id}\",");
        stringBuilder.AppendLine("                defaults: new { controller = \"Home\", action = \"Index\", id = UrlParameter.Optional }");
        stringBuilder.AppendLine("            );");
        stringBuilder.AppendLine("        }");
        stringBuilder.AppendLine("    }");
        stringBuilder.AppendLine("}");
        streamWriter.Write(stringBuilder.ToString());
      }
    }

    private void GenerateCodeVBMVC5()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Imports System");
        stringBuilder.AppendLine("Imports System.Collections.Generic");
        stringBuilder.AppendLine("Imports System.Linq");
        stringBuilder.AppendLine("Imports System.Web");
        stringBuilder.AppendLine("Imports System.Web.Mvc");
        stringBuilder.AppendLine("Imports System.Web.Routing");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("internal Module RouteConfig");
        stringBuilder.AppendLine("    internal Sub RegisterRoutes(ByVal routes As RouteCollection)");
        stringBuilder.AppendLine("        routes.IgnoreRoute(\"{resource}.axd/{*pathInfo}\")");
        stringBuilder.AppendLine("");
        foreach (Table selectedTable in (List<Table>) this._selectedTables)
        {
          if (selectedTable.PrimaryKeyCount > 1 && !selectedTable.IsContainsPrimaryAndForeignKeyColumnsOnly)
          {
            string primaryKeysInCurlies = Functions.GetSlashDelimitedPrimaryKeysInCurlies(selectedTable);
            stringBuilder.AppendLine("        routes.MapRoute(");
            stringBuilder.AppendLine("            \"" + selectedTable.Name + "\", _");
            stringBuilder.AppendLine("            \"" + selectedTable.Name + "/{action}" + primaryKeysInCurlies + "\", _");
            stringBuilder.AppendLine("            New With { .controller = \"" + selectedTable.Name + "\", .action = \"Index\" } _");
            stringBuilder.AppendLine("        )");
            stringBuilder.AppendLine("");
          }
        }
        stringBuilder.AppendLine("        routes.MapRoute( _");
        stringBuilder.AppendLine("            name:=\"Default\", _");
        stringBuilder.AppendLine("            url:=\"{controller}/{action}/{id}\", _");
        stringBuilder.AppendLine("            defaults:=New With {.controller = \"Home\", .action = \"Index\", .id = UrlParameter.Optional} _");
        stringBuilder.AppendLine("        )");
        stringBuilder.AppendLine("    End Sub");
        stringBuilder.AppendLine("End Module");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
