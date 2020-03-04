
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class PropertyModelBase
  {
    private const Language _language = Language.CSharp;
    private Table _table;
    private Tables _selectedTables;
    private string _webAppName;
    private string _apiName;
    private const string _fileExtension = ".cs";
    private string _apiNameDirectory;
    private ApplicationVersion _appVersion;

    internal PropertyModelBase(Table table, Tables selectedTables, string apiName, string webAppName, string apiNameDirectory, ApplicationVersion appVersion)
    {
      this._table = table;
      this._selectedTables = selectedTables;
      this._apiName = apiName;
      this._webAppName = webAppName;
      this._apiNameDirectory = apiNameDirectory + "\\PropertyModels\\Base\\";
      this._appVersion = appVersion;
      this.Generate();
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._apiNameDirectory + this._table.Name + MyConstants.WordPropetyModelBase + ".cs"))
      {
        StringBuilder sb = new StringBuilder();
        if (this._appVersion != ApplicationVersion.Express)
          sb.AppendLine("using " + this._apiName + ".Domain;");
        sb.AppendLine("using " + this._apiName + ".BusinessObject;");
        sb.AppendLine("using System.Collections.Generic;");
        sb.AppendLine("");
        sb.AppendLine("namespace " + this._apiName + "." + MyConstants.WordPropertyModelsDotBase);
        sb.AppendLine("{ ");
        sb.AppendLine("     /// <summary>");
        sb.AppendLine("     /// Base class for " + this._table.Name + MyConstants.WordPropertyModel + ".  Do not make changes to this class,");
        sb.AppendLine("     /// instead, put additional code in the " + this._table.Name + MyConstants.WordPropertyModel + " class ");
        sb.AppendLine("     /// </summary>");
        sb.AppendLine("     public class " + this._table.Name + MyConstants.WordPropetyModelBase);
        sb.AppendLine("     { ");
        this.WriteProperties(sb);
        sb.AppendLine("     } ");
        sb.AppendLine("} ");
        streamWriter.Write(sb.ToString());
      }
    }

    private void WriteProperties(StringBuilder sb)
    {
      string str1 = this._table.Name + MyConstants.WordModel;
      sb.AppendLine("         public " + this._apiName + "." + MyConstants.WordModels + "." + str1 + " " + str1 + " { get; set; }");
      if (this._appVersion == ApplicationVersion.ProfessionalPlus)
      {
        sb.AppendLine("         public CrudOperation Operation { get; set; }");
        sb.AppendLine("         public string ReturnUrl { get; set; }");
        StringBuilder stringBuilder = new StringBuilder();
        foreach (Column column1 in (List<Column>) this._table.Columns)
        {
          Column column = column1;
          if (column.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK)
          {
            string str2 = column.ForeignKeyTableName + MyConstants.WordDropDownListData;
            if (!stringBuilder.ToString().Contains(str2 + ","))
            {
              if (this._selectedTables.Exists((Predicate<Table>) (t => t.Name + "Model" == column.ForeignKeyTableName)))
                sb.AppendLine("         public List<BusinessObject." + column.ForeignKeyTableName + "> " + str2 + " { get; set; }");
              else
                sb.AppendLine("         public List<" + column.ForeignKeyTableName + "> " + str2 + " { get; set; }");
              stringBuilder.Append(str2 + ",");
            }
          }
        }
      }
      else
      {
        sb.AppendLine("         public string ViewControllerName { get; set; }");
        sb.AppendLine("         public string ViewActionName { get; set; }");
        sb.AppendLine("         public string ViewReturnUrl { get; set; }");
      }
    }
  }
}
