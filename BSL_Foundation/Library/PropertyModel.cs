
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class PropertyModel
  {
    private Table _table;
    private const string _fileExtension = ".cs";
    private string _apiNameDirectory;
    private string _webAppName;
    private string _apiName;

    internal PropertyModel(Table table, string apiName, string webAppName, string apiNameDirectory)
    {
      this._table = table;
      this._apiName = apiName;
      this._apiNameDirectory = apiNameDirectory + "\\PropertyModels\\";
      this._webAppName = webAppName;
      this.Generate();
    }

    private void Generate()
    {
      string path = this._apiNameDirectory + this._table.Name + MyConstants.WordPropertyModel + ".cs";
      if (File.Exists(path))
        return;
      using (StreamWriter streamWriter = new StreamWriter(path))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("using " + this._apiName + "." + MyConstants.WordPropertyModelsDotBase + ";");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("namespace " + this._apiName + "." + MyConstants.WordPropertyModels);
        stringBuilder.AppendLine("{ ");
        stringBuilder.AppendLine("     /// <summary>");
        stringBuilder.AppendLine("     /// This file will not be overwritten.  You can put");
        stringBuilder.AppendLine("     /// additional " + this._table.Name + MyConstants.WordPropertyModel + " code in this class.");
        stringBuilder.AppendLine("     /// </summary>");
        stringBuilder.AppendLine("     public class " + this._table.Name + MyConstants.WordPropertyModel + " : " + this._table.Name + MyConstants.WordPropetyModelBase);
        stringBuilder.AppendLine("     { ");
        stringBuilder.AppendLine("     } ");
        stringBuilder.AppendLine("} ");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
