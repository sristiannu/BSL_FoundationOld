
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class Model
  {
    private Table _table;
    private const string _fileExtension = ".cs";
    private string _webAppRootDirectory;
    private string _webAppName;

    internal Model(Table table, string webAppName, string webAppRootDirectory)
    {
      this._table = table;
      this._webAppRootDirectory = webAppRootDirectory + MyConstants.DirectoryModel;
      this._webAppName = webAppName;
      this.Generate();
    }

    private void Generate()
    {
      string path = this._webAppRootDirectory + this._table.Name + MyConstants.WordModel + ".cs";
      if (File.Exists(path))
        return;
      using (StreamWriter streamWriter = new StreamWriter(path))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("using " + this._webAppName + "." + MyConstants.WordModelsDotBase + ";");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("namespace " + this._webAppName + "." + MyConstants.WordModels);
        stringBuilder.AppendLine("{ ");
        stringBuilder.AppendLine("     /// <summary>");
        stringBuilder.AppendLine("     /// This file will not be overwritten.  You can put");
        stringBuilder.AppendLine("     /// additional " + this._table.Name + MyConstants.WordModel + " code in this class.");
        stringBuilder.AppendLine("     /// </summary>");
        stringBuilder.AppendLine("     public class " + this._table.Name + MyConstants.WordModel + " : " + this._table.Name + MyConstants.WordModelBase);
        stringBuilder.AppendLine("     { ");
        stringBuilder.AppendLine("     } ");
        stringBuilder.AppendLine("} ");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
