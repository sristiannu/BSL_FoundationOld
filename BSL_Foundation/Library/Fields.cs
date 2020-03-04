
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class Fields
  {
    private Table _table;
    private const string _fileExtension = ".cs";
    private string _apiRootDirectory;
    private string _apiName;

    internal Fields(Table table, string apiName, string apiRootDirectory)
    {
      this._table = table;
      this._apiRootDirectory = apiRootDirectory + MyConstants.DirectoryFields;
      this._apiName = apiName;
      this.Generate();
    }

    private void Generate()
    {
      string path = this._apiRootDirectory + this._table.Name + MyConstants.WordFields + ".cs";
      if (File.Exists(path))
        return;
      using (StreamWriter streamWriter = new StreamWriter(path))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("using System;");
        stringBuilder.AppendLine("using " + this._apiName + "." + MyConstants.WordFieldsDotBase + ";");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("namespace " + this._apiName + "." + MyConstants.WordModels);
        stringBuilder.AppendLine("{");
        stringBuilder.AppendLine("     /// <summary>");
        stringBuilder.AppendLine("     /// This file will not be overwritten.  You can put");
        stringBuilder.AppendLine("     /// additional " + this._table.Name + " Fields code in this class.");
        stringBuilder.AppendLine("     /// </summary>");
        stringBuilder.AppendLine("     public class " + this._table.Name + MyConstants.WordFields + " : " + this._table.Name + MyConstants.WordFieldsBase);
        stringBuilder.AppendLine("     {");
        stringBuilder.AppendLine("     }");
        stringBuilder.AppendLine("}");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
