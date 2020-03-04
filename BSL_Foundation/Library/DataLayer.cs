
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class DataLayer
  {
    private Table _table;
    private const string _fileExtension = ".cs";
    private string _apiName;
    private string _apiNameDirectory;

    internal DataLayer()
    {
    }

    internal DataLayer(Table table, string apiName, string apiNameDirectory)
    {
      this._table = table;
      this._apiName = apiName;
      this._apiNameDirectory = apiNameDirectory + "\\" + MyConstants.DirectoryDataLayer;
      this.Generate();
    }

    private void Generate()
    {
      string path = this._apiNameDirectory + this._table.Name + MyConstants.WordDataLayer + ".cs";
      if (File.Exists(path))
        return;
      using (StreamWriter streamWriter = new StreamWriter(path))
      {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("using System;");
        sb.AppendLine("using " + this._apiName + ".DataLayer.Base;");
        sb.AppendLine("");
        sb.AppendLine("namespace " + this._apiName + ".DataLayer");
        sb.AppendLine("{");
        sb.AppendLine("     /// <summary>\r\n");
        sb.AppendLine("     /// This file will not be overwritten.  You can put");
        sb.AppendLine("     /// additional " + this._table.Name + " DataLayer code in this class");
        sb.AppendLine("     /// </summary>\r\n");
        sb.AppendLine("     internal class " + this._table.Name + MyConstants.WordDataLayer + " : " + this._table.Name + "DataLayerBase");
        sb.AppendLine("     {");
        this.WriteConstructor(sb);
        sb.AppendLine("     }");
        sb.AppendLine("}");
        streamWriter.Write(sb.ToString());
      }
    }

    private void WriteConstructor(StringBuilder sb)
    {
      sb.AppendLine("         // constructor");
      sb.AppendLine("         internal " + this._table.Name + MyConstants.WordDataLayer + "()");
      sb.AppendLine("         {");
      sb.AppendLine("         }");
    }
  }
}
