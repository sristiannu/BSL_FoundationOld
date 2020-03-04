
using System;
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class ForeachViewModelBase
  {
    private const Language _language = Language.CSharp;
    private Table _table;
    private Tables _selectedTables;
    private string _apiName;
    private const string _fileExtension = ".cs";
    private string _foreachViewModelBaseDirectory;

    internal ForeachViewModelBase(Table table, Tables selectedTables, string apiName, string apiRootDirectory)
    {
      this._table = table;
      this._selectedTables = selectedTables;
      this._apiName = apiName;
      this._foreachViewModelBaseDirectory = apiRootDirectory + MyConstants.DirectoryPageModel + "Foreach\\Base\\";
      this.Generate();
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._foreachViewModelBaseDirectory + this._table.Name + "Foreach" + MyConstants.WordPropetyModelBase + ".cs"))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("using " + this._apiName + ".Models;");
        stringBuilder.AppendLine("using " + this._apiName + ".BusinessObject;");
        stringBuilder.AppendLine("using System.Collections.Generic;");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("namespace " + this._apiName + "." + MyConstants.WordPropertyModelsDotBase);
        stringBuilder.AppendLine("{ ");
        stringBuilder.AppendLine("     /// <summary>");
        stringBuilder.AppendLine("     /// Base class for " + this._table.Name + "Foreach" + MyConstants.WordPropertyModel + ".  Do not make changes to this class,");
        stringBuilder.AppendLine("     /// instead, put additional code in the " + this._table.Name + "Foreach" + MyConstants.WordPropertyModel + " class");
        stringBuilder.AppendLine("     /// </summary>");
        stringBuilder.AppendLine("     public class " + this._table.Name + "Foreach" + MyConstants.WordPropetyModelBase);
        stringBuilder.AppendLine("     { ");
        if (this._selectedTables.Exists((Predicate<Table>) (t => t.Name + "Model" == this._table.Name)))
          stringBuilder.AppendLine("         public List<BusinessObject." + this._table.Name + "> " + this._table.Name + "Data { get; set; }");
        else
          stringBuilder.AppendLine("         public List<" + this._table.Name + "> " + this._table.Name + "Data { get; set; }");
        stringBuilder.AppendLine("         public string[,] " + this._table.Name + "FieldNames { get; set; }");
        stringBuilder.AppendLine("         public string FieldToSort { get; set; }");
        stringBuilder.AppendLine("         public string FieldToSortWithOrder { get; set; }");
        stringBuilder.AppendLine("         public string FieldSortOrder { get; set; }");
        stringBuilder.AppendLine("         public int StartPage { get; set; }");
        stringBuilder.AppendLine("         public int EndPage { get; set; }");
        stringBuilder.AppendLine("         public int CurrentPage { get; set; }");
        stringBuilder.AppendLine("         public int NumberOfPagesToShow { get; set; }");
        stringBuilder.AppendLine("         public int TotalPages { get; set; }");
        stringBuilder.AppendLine("         public List<string> UnsortableFields { get; set; }");
        stringBuilder.AppendLine("     } ");
        stringBuilder.AppendLine("} ");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
