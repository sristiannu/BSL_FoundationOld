
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class LayoutFile
  {
    private string _fullFileNamePath;
    private string _webAppName;
    private string _jqueryUITheme;

    private LayoutFile()
    {
    }

    internal LayoutFile(string fullFileNamePath, string webAppName, string jqueryUITheme)
    {
      this._fullFileNamePath = fullFileNamePath;
      this._webAppName = webAppName;
      this._jqueryUITheme = jqueryUITheme;
      this.Generate();
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("<!DOCTYPE html>");
        stringBuilder.AppendLine("<html>");
        stringBuilder.AppendLine("    <head>");
        stringBuilder.AppendLine("        <meta charset=\"utf-8\" />");
        stringBuilder.AppendLine("        <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />");
        stringBuilder.AppendLine("        <title>@ViewData[\"Title\"] - " + this._webAppName + "</title>");
        stringBuilder.AppendLine("        <environment names=\"Development\">");
        stringBuilder.AppendLine("            <link rel=\"stylesheet\" href=\"~/lib/bootstrap/dist/css/bootstrap.min.css\" />");
        stringBuilder.AppendLine("            <link rel=\"stylesheet\" href=\"~/css/site.css\" />");
        stringBuilder.AppendLine("        </environment>");
        stringBuilder.AppendLine("        <environment names=\"Staging,Production\">");
        stringBuilder.AppendLine("            <link rel=\"stylesheet\" href=\"https://ajax.aspnetcdn.com/ajax/bootstrap/3.3.5/css/bootstrap.min.css\"");
        stringBuilder.AppendLine("                  asp-fallback-href=\"~/lib/bootstrap/dist/css/bootstrap.min.css\"");
        stringBuilder.AppendLine("                  asp-fallback-test-class=\"sr-only\" asp-fallback-test-property=\"position\" ");
        stringBuilder.AppendLine("                  asp-fallback-test-value=\"absolute\" />");
        stringBuilder.AppendLine("            <link rel=\"stylesheet\" href=\"~/css/site.min.css\" asp-append-version=\"true\" />");
        stringBuilder.AppendLine("        </environment>");
        stringBuilder.AppendLine("        <link rel=\"stylesheet\" href=\"~/css/jquery-ui-1.11.4-themes/" + this._jqueryUITheme + "/jquery-ui.min.css\" />");
        stringBuilder.AppendLine("        <link rel=\"stylesheet\" href=\"~/css/jquery-ui-1.11.4-themes/" + this._jqueryUITheme + "/theme.css\" />");
        stringBuilder.AppendLine("        @RenderSection(\"AdditionalCss\", required: false)");
        stringBuilder.AppendLine("    </head>");
        stringBuilder.AppendLine("    <body>");
        stringBuilder.AppendLine("        <div class=\"navbar navbar-inverse navbar-fixed-top\">");
        stringBuilder.AppendLine("            <div class=\"container\">");
        stringBuilder.AppendLine("                <div class=\"navbar-header\">");
        stringBuilder.AppendLine("                    <button type=\"button\" class=\"navbar-toggle\" data-toggle=\"collapse\" data-target=\".navbar-collapse\">");
        stringBuilder.AppendLine("                        <span class=\"sr-only\">Toggle navigation</span>");
        stringBuilder.AppendLine("                        <span class=\"icon-bar\"></span>");
        stringBuilder.AppendLine("                        <span class=\"icon-bar\"></span>");
        stringBuilder.AppendLine("                        <span class=\"icon-bar\"></span>");
        stringBuilder.AppendLine("                    </button>");
        stringBuilder.AppendLine("                    <a href=\"/Index\" class=\"navbar-brand\">" + this._webAppName + "</a>");
        stringBuilder.AppendLine("                </div>");
        stringBuilder.AppendLine("                <div class=\"navbar-collapse collapse\">");
        stringBuilder.AppendLine("                </div>");
        stringBuilder.AppendLine("            </div>");
        stringBuilder.AppendLine("        </div>");
        stringBuilder.AppendLine("        <div class=\"container body-content\">");
        stringBuilder.AppendLine("            @RenderBody()");
        stringBuilder.AppendLine("            <hr />");
        stringBuilder.AppendLine("            <footer>");
        stringBuilder.AppendLine("                <p>&copy; @DateTime.Now.Year - " + this._webAppName + "</p>");
        stringBuilder.AppendLine("            </footer>");
        stringBuilder.AppendLine("        </div>");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        <environment names=\"Development\">");
        stringBuilder.AppendLine("            <script src=\"~/js/jquery-1.12.2.min.js\"></script>");
        stringBuilder.AppendLine("            <script src=\"~/lib/bootstrap/dist/js/bootstrap.min.js\"></script>");
        stringBuilder.AppendLine("            <script src=\"~/js/jquery-ui-1.11.4.min.js\" asp-append-version=\"true\"></script>");
        stringBuilder.AppendLine("        </environment>");
        stringBuilder.AppendLine("        <environment names=\"Staging,Production\">");
        stringBuilder.AppendLine("            <script src=\"https://code.jquery.com/jquery-1.12.2.js\"");
        stringBuilder.AppendLine("                    integrity=\"sha256-VUCyr0ZXB5VhBibo2DkTVhdspjmxUgxDGaLQx7qb7xY=\" crossorigin=\"anonymous\"");
        stringBuilder.AppendLine("                    asp-fallback-src=\"~/js/jquery-1.12.2.min.js\">");
        stringBuilder.AppendLine("            </script>");
        stringBuilder.AppendLine("            <script src=\"https://ajax.aspnetcdn.com/ajax/bootstrap/3.3.5/bootstrap.min.js\"");
        stringBuilder.AppendLine("                    asp-fallback-src=\"~/lib/bootstrap/dist/js/bootstrap.min.js\"");
        stringBuilder.AppendLine("                    asp-fallback-test=\"window.jQuery && window.jQuery.fn && window.jQuery.fn.modal\">");
        stringBuilder.AppendLine("            </script>");
        stringBuilder.AppendLine("            <script src=\"https://code.jquery.com/ui/1.11.4/jquery-ui.min.js\"");
        stringBuilder.AppendLine("                    integrity=\"sha256-xNjb53/rY+WmG+4L6tTl9m6PpqknWZvRt0rO1SRnJzw=\" crossorigin=\"anonymous\"");
        stringBuilder.AppendLine("                    asp-fallback-src=\"~/js/jquery-ui-1.11.4.min.js\">");
        stringBuilder.AppendLine("            </script>");
        stringBuilder.AppendLine("        </environment>");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        @RenderSection(\"AdditionalJavaScript\", required: false)");
        stringBuilder.AppendLine("    </body>");
        stringBuilder.AppendLine("</html>");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
