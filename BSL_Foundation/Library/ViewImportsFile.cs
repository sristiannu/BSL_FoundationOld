
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class ViewImportsFile
  {
    private string _fullFileNamePath;
    private string _webAppName;
    private string _webApiName;
    private bool _isForWebApp;

    private ViewImportsFile()
    {
    }

    internal ViewImportsFile(string fullFileNamePath, string webAppName, string webApiName, bool isForWebApp, Language language)
    {
      this._fullFileNamePath = fullFileNamePath;
      this._webAppName = webAppName;
      this._webApiName = webApiName;
      this._isForWebApp = isForWebApp;
      if (language == Language.CSharp)
        this.GenerateCS();
      else
        this.GenerateVB();
    }

    private void GenerateCS()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        if (this._isForWebApp)
          stringBuilder.AppendLine("@using " + this._webAppName);
        else
          stringBuilder.AppendLine("@using " + this._webApiName);
        stringBuilder.AppendLine("@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers");
        streamWriter.Write(stringBuilder.ToString());
      }
    }

    private void GenerateVB()
    {
    }
  }
}
