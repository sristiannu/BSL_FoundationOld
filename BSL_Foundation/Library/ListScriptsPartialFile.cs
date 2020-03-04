
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal sealed class ListScriptsPartialFile
  {
    private string _fullFileNamePath;

    private ListScriptsPartialFile()
    {
    }

    internal ListScriptsPartialFile(string fullFileNamePath)
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
        stringBuilder.AppendLine("    <script src=\"~/js/jquery-ui-1.11.4.min.js\" asp-append-version=\"true\"></script>");
        stringBuilder.AppendLine("</environment>");
        stringBuilder.AppendLine("<environment names=\"Staging,Production\">");
        stringBuilder.AppendLine("    <script src=\"https://code.jquery.com/ui/1.11.4/jquery-ui.min.js\"");
        stringBuilder.AppendLine("            integrity=\"sha256-xNjb53/rY+WmG+4L6tTl9m6PpqknWZvRt0rO1SRnJzw=\" crossorigin=\"anonymous\"");
        stringBuilder.AppendLine("            asp-fallback-src=\"~/js/jquery-ui-1.11.4.min.js\">");
        stringBuilder.AppendLine("    </script>");
        stringBuilder.AppendLine("</environment>");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("<script src=\"~/js/jqgrid-i18n/grid.locale-en.min.js\" asp-append-version=\"true\"></script>");
        stringBuilder.AppendLine("<script src=\"~/js/jquery-jqgrid-4.13.2.min.js\" asp-append-version=\"true\"></script>");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
