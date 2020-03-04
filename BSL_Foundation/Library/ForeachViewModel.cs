
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class ForeachViewModel
  {
    private Table _table;
    private const string _fileExtension = ".cs";
    private string _foreachViewModelDirectory;
    private string _apiName;

    internal ForeachViewModel(Table table, string apiName, string apiRootDirectory)
    {
      this._table = table;
      this._foreachViewModelDirectory = apiRootDirectory + MyConstants.DirectoryPageModel + "Foreach\\";
      this._apiName = apiName;
      this.Generate();
    }

    private void Generate()
    {
      string path = this._foreachViewModelDirectory + this._table.Name + "Foreach" + MyConstants.WordPropertyModel + ".cs";
      if (File.Exists(path))
        return;
      using (StreamWriter streamWriter = new StreamWriter(path))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("using " + this._apiName + "." + MyConstants.WordPropertyModelsDotBase + ";");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("namespace " + this._apiName + "." + MyConstants.WordPropertyModels);
        stringBuilder.AppendLine("{");
        stringBuilder.AppendLine("     /// <summary>");
        stringBuilder.AppendLine("     /// This file will not be overwritten.  You can put");
        stringBuilder.AppendLine("     /// additional " + this._table.Name + "ForeachViewModel code in this class.");
        stringBuilder.AppendLine("     /// </summary>");
        stringBuilder.AppendLine("     public class " + this._table.Name + "Foreach" + MyConstants.WordPropertyModel + " : " + this._table.Name + "Foreach" + MyConstants.WordPropetyModelBase);
        stringBuilder.AppendLine("     { ");
        stringBuilder.AppendLine("     } ");
        stringBuilder.AppendLine("}");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
