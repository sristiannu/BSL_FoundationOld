
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class ViewStartFile
  {
    private string _fullFileNamePath;

    private ViewStartFile()
    {
    }

    internal ViewStartFile(string fullFileNamePath, Language language)
    {
      this._fullFileNamePath = fullFileNamePath;
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
        stringBuilder.AppendLine("@{");
        stringBuilder.AppendLine("    Layout = \"_Layout\";");
        stringBuilder.AppendLine("}");
        streamWriter.Write(stringBuilder.ToString());
      }
    }

    private void GenerateVB()
    {
    }
  }
}
