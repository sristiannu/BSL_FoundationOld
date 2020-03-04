
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class AddEditPartialPageModel
  {
    private Table _table;
    private Tables _selectedTables;
    private const string _fileExtension = ".cs";
    private string _directory;
    private string _webAppRootDirectory;
    private string _webAppName;
    private string _apiName;
    private MVCGridViewType _viewType;
#pragma warning disable CS0169 // The field 'AddEditPartialPageModel._viewName' is never used
    private string _viewName;
#pragma warning restore CS0169 // The field 'AddEditPartialPageModel._viewName' is never used
    private StringBuilder _commaDelimitedColNames;
#pragma warning disable CS0169 // The field 'AddEditPartialPageModel._currentColumn' is never used
    private Column _currentColumn;
#pragma warning restore CS0169 // The field 'AddEditPartialPageModel._currentColumn' is never used
    private AppendAddEditRecordContentType _appendAddEditRecordContentType;
    private bool _isUseWebApi;

    internal AddEditPartialPageModel(MVCGridViewType viewType, Table table, Tables selectedTables, string webAppName, string apiName, string webAppRootDirectory, AppendAddEditRecordContentType appendAddEditRecordContentType, bool isUseWebApi)
    {
      this._viewType = viewType;
      this._table = table;
      this._selectedTables = selectedTables;
      this._webAppRootDirectory = webAppRootDirectory + "Pages\\";
      this._webAppName = webAppName;
      this._apiName = apiName;
      this._isUseWebApi = isUseWebApi;
      this._commaDelimitedColNames = new StringBuilder();
      this._appendAddEditRecordContentType = appendAddEditRecordContentType;
      this._directory = this._appendAddEditRecordContentType != AppendAddEditRecordContentType.Unbound ? webAppRootDirectory + "PartialModels\\" : webAppRootDirectory + "Pages\\" + this._table.Name + "Pages\\";
      this.Generate();
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._directory + "AddEdit" + this._table.Name + "PartialModel.cs"))
      {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("using Microsoft.AspNetCore.Mvc;");
        sb.AppendLine("using System.Collections.Generic;");
        sb.AppendLine("using " + this._apiName + ".Domain;");
        sb.AppendLine("using " + this._apiName + ".BusinessObject;");
        sb.AppendLine("");
        sb.AppendLine("namespace " + this._webAppName + ".PartialModels");
        sb.AppendLine("{");
        sb.AppendLine("     public class AddEdit" + this._table.Name + "PartialModel");
        sb.AppendLine("     {");
        Functions.WriteBusinessObjectBindProperty(sb, this._table, this._apiName);
        Functions.WriteDropDownListDataProperties(sb, this._table);
        sb.AppendLine("         public CrudOperation Operation;");
        sb.AppendLine("         public string ReturnUrl;");
        sb.AppendLine("");
        sb.AppendLine("         public AddEdit" + this._table.Name + "PartialModel()");
        sb.AppendLine("         {");
        sb.AppendLine("         }");
        sb.AppendLine("");
        sb.AppendLine("     }");
        sb.AppendLine("}");
        streamWriter.Write(sb.ToString());
      }
    }
  }
}
