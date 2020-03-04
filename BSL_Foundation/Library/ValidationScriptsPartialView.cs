
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal sealed class ValidationScriptsPartialView
  {
    private string _fullFileNamePath;

    private ValidationScriptsPartialView()
    {
    }

    internal ValidationScriptsPartialView(string fullFileNamePath)
    {
      this._fullFileNamePath = fullFileNamePath;
      this.Generate();
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("<environment names=\"Development\">");
        stringBuilder.AppendLine("    <script src=\"~/lib/jquery-validation/dist/jquery.validate.min.js\"></script>");
        stringBuilder.AppendLine("    <script src=\"~/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js\"></script>");
        stringBuilder.AppendLine("</environment>");
        stringBuilder.AppendLine("<environment names=\"Staging,Production\">");
        stringBuilder.AppendLine("    <script src=\"https://ajax.aspnetcdn.com/ajax/jquery.validate/1.14.0/jquery.validate.min.js\"");
        stringBuilder.AppendLine("            asp-fallback-src=\"~/lib/jquery-validation/dist/jquery.validate.min.js\"");
        stringBuilder.AppendLine("            asp-fallback-test=\"window.jQuery && window.jQuery.validator\">");
        stringBuilder.AppendLine("    </script>");
        stringBuilder.AppendLine("    <script src=\"https://ajax.aspnetcdn.com/ajax/mvc/5.2.3/jquery.validate.unobtrusive.min.js\"");
        stringBuilder.AppendLine("            asp-fallback-src=\"~/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js\"");
        stringBuilder.AppendLine("            asp-fallback-test=\"window.jQuery && window.jQuery.validator && window.jQuery.validator.unobtrusive\">");
        stringBuilder.AppendLine("    </script>");
        stringBuilder.AppendLine("</environment>");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
