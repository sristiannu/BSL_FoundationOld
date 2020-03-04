
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class FieldsBase
  {
    private const Language _language = Language.CSharp;
    private Table _table;
    private const string _fileExtension = ".cs";
    private string _apiRootDirectory;
    private string _apiName;

    internal FieldsBase(Table table, string apiName, string apiRootDirectory)
    {
      this._table = table;
      this._apiName = apiName;
      this._apiRootDirectory = apiRootDirectory + MyConstants.DirectoryFieldsBase;
      this.Generate();
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._apiRootDirectory + this._table.Name + MyConstants.WordFieldsBase + ".cs"))
      {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("using System;");
        sb.AppendLine("");
        sb.AppendLine("namespace " + this._apiName + "." + MyConstants.WordFieldsDotBase);
        sb.AppendLine("{");
        sb.AppendLine("     /// <summary>");
        sb.AppendLine("     /// Base class for " + this._table.Name + MyConstants.WordFields + ".  Do not make changes to this class,");
        sb.AppendLine("     /// instead, put additional code in the " + this._table.Name + MyConstants.WordFields + " class. ");
        sb.AppendLine("     /// Note: This class is identical to the " + this._table.Name + MyConstants.WordModelBase + " without/minus the data annotations.");
        sb.AppendLine("     /// </summary>");
        sb.AppendLine("     public class " + this._table.Name + MyConstants.WordFieldsBase);
        sb.AppendLine("     {");
        this.WriteProperties(sb);
        sb.AppendLine("     }");
        sb.AppendLine("}");
        streamWriter.Write(sb.ToString());
      }
    }

    private void WriteProperties(StringBuilder sb)
    {
      foreach (Column column in (List<Column>) this._table.Columns)
      {
        string str = column.SystemType + column.NullableChar;
        if (column.SystemType == "bool")
          str = column.SystemType;
        sb.AppendLine("         /// <summary> ");
        sb.AppendLine("         /// Gets or Sets " + column.Name + " ");
        sb.AppendLine("         /// </summary> ");
        sb.AppendLine("         public " + str + " " + column.Name + " { get; set; } ");
        sb.AppendLine("");
        if (column.IsMoneyOrDecimalField)
        {
          sb.AppendLine("         /// <summary> ");
          sb.AppendLine("         /// Gets or Sets " + column.Name + MyConstants.WordTotal);
          sb.AppendLine("         /// </summary> ");
          sb.AppendLine("         public " + column.SystemType + " " + column.Name + MyConstants.WordTotal + " { get; set; } ");
          sb.AppendLine("");
        }
      }
    }
  }
}
