
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class DetailsPage
  {
    private const Language _language = Language.CSharp;
    private Table _table;
    private Tables _selectedTables;
    private const string _fileExtension = ".cs";
    private string _directory;
    private string _webAppName;
    private string _apiName;
    private string _viewName;
    private ApplicationVersion _appVersion;

    internal DetailsPage(Table table, Tables selectedTables, string webAppName, string apiName, string webAppRootDirectory, string viewName, ApplicationVersion appVersion)
    {
      this._table = table;
      this._selectedTables = selectedTables;
      this._webAppName = webAppName;
      this._apiName = apiName;
      this._viewName = viewName.Trim();
      this._appVersion = appVersion;
      this._directory = webAppRootDirectory + MyConstants.DirectoryRazorPage + this._table.Name + "\\";
      this.Generate();
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._directory + this._table.Name + "_" + this._viewName + ".cshtml"))
      {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("@page");
        sb.AppendLine("@model " + this._webAppName + ".Pages." + this._table.Name + "_" + this._viewName + "Model");
        sb.AppendLine("");
        sb.AppendLine("<h2>Record Details</h2>");
        sb.AppendLine("<div>");
        Functions.AppendAddEditRecordMVC(sb, this._table, this._selectedTables, AppendAddEditRecordType.AddEdit, AppendAddEditRecordContentType.Details, this._appVersion, MVCGridViewType.RecordDetails);
        sb.AppendLine("</div>");
        streamWriter.Write(sb.ToString());
      }
    }
  }
}
