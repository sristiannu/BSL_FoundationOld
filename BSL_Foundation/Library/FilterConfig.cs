
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class FilterConfig
  {
    private string _fullFileNamePath;
    private string _websiteName;
    private Language _language;

    internal FilterConfig()
    {
    }

    internal FilterConfig(string fullFileNamePath, string websiteName, Language language)
    {
      this._fullFileNamePath = fullFileNamePath;
      this._websiteName = websiteName;
      this._language = language;
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
        stringBuilder.AppendLine("using System.Web;");
        stringBuilder.AppendLine("using System.Web.Mvc;");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("namespace " + this._websiteName);
        stringBuilder.AppendLine("{");
        stringBuilder.AppendLine("    internal class FilterConfig");
        stringBuilder.AppendLine("    {");
        stringBuilder.AppendLine("        internal static void RegisterGlobalFilters(GlobalFilterCollection filters)");
        stringBuilder.AppendLine("        {");
        stringBuilder.AppendLine("            filters.Add(new HandleErrorAttribute());");
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
        stringBuilder.AppendLine("Imports System.Web");
        stringBuilder.AppendLine("Imports System.Web.Mvc");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("internal Module FilterConfig");
        stringBuilder.AppendLine("    internal Sub RegisterGlobalFilters(ByVal filters As GlobalFilterCollection)");
        stringBuilder.AppendLine("        filters.Add(New HandleErrorAttribute())");
        stringBuilder.AppendLine("    End Sub");
        stringBuilder.AppendLine("End Module");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
