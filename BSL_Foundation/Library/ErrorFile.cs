
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class ErrorFile
  {
    private string _fullFileNamePath;
    private Language _language;

    private ErrorFile()
    {
    }

    internal ErrorFile(string path, Language language, ApplicationType appType = ApplicationType.ASPNETMVC)
    {
      this._fullFileNamePath = path;
      this._language = language;
      if (appType == ApplicationType.ASPNETMVC5)
      {
        if (language == Language.CSharp)
          this.GenerateCodeCSMVC4();
        else
          this.GenerateCodeVBMVC4();
      }
      else if (language == Language.CSharp)
        this.GenerateCS();
      else
        this.GenerateVB();
    }

    private void GenerateCS()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("@model System.Web.Mvc.HandleErrorInfo");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("@{");
        stringBuilder.AppendLine("    ViewBag.Title = \"Error\";");
        stringBuilder.AppendLine("}");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("<h2>");
        stringBuilder.AppendLine("    Sorry, an error occurred while processing your request.");
        stringBuilder.AppendLine("</h2>");
        streamWriter.Write(stringBuilder.ToString());
      }
    }

    private void GenerateVB()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("@ModelType System.Web.Mvc.HandleErrorInfo");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("@Code");
        stringBuilder.AppendLine("    ViewData(\"Title\") = \"Error\"");
        stringBuilder.AppendLine("End Code");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("<h2>");
        stringBuilder.AppendLine("    Sorry, an error occurred while processing your request.");
        stringBuilder.AppendLine("</h2>");
        streamWriter.Write(stringBuilder.ToString());
      }
    }

    private void GenerateCodeCSMVC4()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("@model System.Web.Mvc.HandleErrorInfo");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("@{");
        stringBuilder.AppendLine("    ViewBag.Title = \"Error\";");
        stringBuilder.AppendLine("}");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("<hgroup class=\"title\">");
        stringBuilder.AppendLine("    <h1 class=\"error\">Error.</h1>");
        stringBuilder.AppendLine("    <h2 class=\"error\">An error occurred while processing your request.</h2>");
        stringBuilder.AppendLine("</hgroup>");
        streamWriter.Write(stringBuilder.ToString());
      }
    }

    private void GenerateCodeVBMVC4()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("@ModelType System.Web.Mvc.HandleErrorInfo");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("@Code");
        stringBuilder.AppendLine("    ViewData(\"Title\") = \"Error\"");
        stringBuilder.AppendLine("End Code");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("<hgroup class=\"title\">");
        stringBuilder.AppendLine("    <h1 class=\"error\">Error.</h1>");
        stringBuilder.AppendLine("    <h2 class=\"error\">An error occurred while processing your request.</h2>");
        stringBuilder.AppendLine("</hgroup>");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
