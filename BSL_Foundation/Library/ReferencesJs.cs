
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class ReferencesJs
  {
    private string _fullFileNamePath;

    internal ReferencesJs()
    {
    }

    internal ReferencesJs(string fullFileNamePath)
    {
      this._fullFileNamePath = fullFileNamePath;
      this.GenerateCode();
    }

    private void GenerateCode()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("/// <autosync enabled=\"true\" />");
        stringBuilder.AppendLine("/// <reference path=\"modernizr-2.6..min2.js\" />");
        stringBuilder.AppendLine("/// <reference path=\"jquery-2.0.3.min.js\" />");
        stringBuilder.AppendLine("/// <reference path=\"bootstrap.min.js\" />");
        stringBuilder.AppendLine("/// <reference path=\"respond.min.js\" />");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
