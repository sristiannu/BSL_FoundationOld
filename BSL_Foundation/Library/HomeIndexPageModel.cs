
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class HomeIndexPageModel
  {
    private string _fileExtension = ".cs";
    private string _directory;
    private Language _language;
    private string _webAppName;

    private HomeIndexPageModel()
    {
    }

    internal HomeIndexPageModel(string directory, Language language, string webAppName)
    {
      this._directory = directory + MyConstants.DirectoryRazorPage;
      this._language = language;
      this._webAppName = webAppName;
      this.Generate();
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._directory + MyConstants.WordIndex + this._fileExtension + "html" + this._fileExtension))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("using Microsoft.AspNetCore.Mvc.RazorPages;");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("namespace " + this._webAppName + ".Pages");
        stringBuilder.AppendLine("{");
        stringBuilder.AppendLine("    public class Index : PageModel");
        stringBuilder.AppendLine("    {");
        stringBuilder.AppendLine("    }");
        stringBuilder.AppendLine("}");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
