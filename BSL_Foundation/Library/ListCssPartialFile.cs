
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal sealed class ListCssPartialFile
  {
    private string _fullFileNamePath;

    private ListCssPartialFile()
    {
    }

    internal ListCssPartialFile(string fullFileNamePath)
    {
      this._fullFileNamePath = fullFileNamePath;
      this.Generate();
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("<link rel=\"stylesheet\" href=\"~/css/jquery-ui-1.11.4-themes/redmond/jquery-ui.min.css\" />");
        stringBuilder.AppendLine("<link rel=\"stylesheet\" href=\"~/css/ui.jqgrid.min.css\" />");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
