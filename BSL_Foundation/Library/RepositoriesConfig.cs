
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class RepositoriesConfig
  {
    private string _pathAndFileName;
    private string _websiteName;

    private RepositoriesConfig()
    {
    }

    internal RepositoriesConfig(string pathAndFileName, string websiteName)
    {
      this._pathAndFileName = pathAndFileName;
      this._websiteName = websiteName.Trim();
      this.Generate();
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._pathAndFileName))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        stringBuilder.AppendLine("<repositories>");
        stringBuilder.AppendLine("  <repository path=\"..\\" + this._websiteName + "\\packages.config\" />");
        stringBuilder.AppendLine("</repositories>");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
