
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class AddEditPartialPage
  {
    private const Language _language = Language.CSharp;
    private Table _table;
    private Tables _selectedTables;
    private const string _fileExtension = ".cs";
    private string _directory;
    private string _webAppName;
    private string _apiName;
    private MVCGridViewType _viewType;
    private string _viewName;
    private ApplicationVersion _appVersion;
    private AppendAddEditRecordContentType _appendAddEditRecordContentType;

    internal AddEditPartialPage(MVCGridViewType viewType, Table table, Tables selectedTables, string webAppName, string apiName, string webAppRootDirectory, AppendAddEditRecordContentType appendAddEditRecordContentType, string viewName = "", ApplicationVersion appVersion = ApplicationVersion.ProfessionalPlus)
    {
      this._viewType = viewType;
      this._table = table;
      this._selectedTables = selectedTables;
      this._webAppName = webAppName;
      this._apiName = apiName;
      this._viewName = viewName.Trim();
      this._appVersion = appVersion;
      this._appendAddEditRecordContentType = appendAddEditRecordContentType;
      this._directory = this._appendAddEditRecordContentType != AppendAddEditRecordContentType.Unbound ? webAppRootDirectory + MyConstants.DirectoryRazorPageShared : webAppRootDirectory + "Pages\\" + this._table.Name + "\\";
      this.Generate();
    }

    private void Generate()
    {
      string path = this._directory + "_AddEdit" + this._table.Name + "Partial.cshtml";
      if (this._appendAddEditRecordContentType == AppendAddEditRecordContentType.Unbound)
        path = this._directory + this._table.Name + "_" + this._viewName + ".cshtml";
      using (StreamWriter streamWriter = new StreamWriter(path))
      {
        StringBuilder sb = new StringBuilder();
        if (this._appendAddEditRecordContentType == AppendAddEditRecordContentType.Unbound)
          sb.AppendLine("@page");
        if (this._appendAddEditRecordContentType == AppendAddEditRecordContentType.Unbound && this._appVersion != ApplicationVersion.Express || this._appendAddEditRecordContentType == AppendAddEditRecordContentType.AddEditPartialView)
          sb.AppendLine("@using " + this._apiName + ".Domain;");
        if (this._appendAddEditRecordContentType == AppendAddEditRecordContentType.AddEditPartialView)
          sb.AppendLine("@model " + this._webAppName + ".PartialModels.AddEdit" + this._table.Name + "PartialModel");
        else if (this._appendAddEditRecordContentType == AppendAddEditRecordContentType.Unbound)
          sb.AppendLine("@model " + this._webAppName + ".Pages." + this._table.Name + "_UnboundModel");
        sb.AppendLine("");
        if (this._appendAddEditRecordContentType == AppendAddEditRecordContentType.Unbound)
        {
          Functions.GetAdditionalJavaScriptForValidationScriptsPartial(sb, this._table);
          sb.AppendLine("<h2>" + this._viewName + " Record</h2>");
          sb.AppendLine("<div>");
        }
        Functions.AppendAddEditRecordMVC(sb, this._table, this._selectedTables, AppendAddEditRecordType.AddEdit, this._appendAddEditRecordContentType, this._appVersion, this._viewType);
        if (this._appendAddEditRecordContentType == AppendAddEditRecordContentType.Unbound)
          sb.AppendLine("</div>");
        streamWriter.Write(sb.ToString());
      }
    }
  }
}
