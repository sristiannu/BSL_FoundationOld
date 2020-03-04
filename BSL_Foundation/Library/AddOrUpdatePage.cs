
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class AddOrUpdatePage
  {
    private const Language _language = Language.CSharp;
    private const string _fileExtension = ".cs";
    private Table _table;
    private string _directory;
    private string _viewName;
    private string _webAppName;

    internal AddOrUpdatePage(Table table, string webAppName, string viewName, string webAppRootDirectory)
    {
      this._table = table;
      this._webAppName = webAppName;
      this._directory = webAppRootDirectory + MyConstants.DirectoryRazorPage + this._table.Name + "\\";
      this._viewName = viewName.Trim();
      this.Generate();
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._directory + this._table.Name + "_" + this._viewName + ".cshtml"))
      {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("@page");
        sb.AppendLine("@model " + this._webAppName + ".Pages." + this._table.Name + "_" + this._viewName + "Model");
        Functions.GetAdditionalJavaScriptForValidationScriptsPartial(sb, this._table);
        sb.AppendLine("<h2>" + this._viewName + " Record</h2>");
        sb.AppendLine("@Html.ValidationSummary(true)");
        sb.AppendLine("<div>");
        sb.AppendLine("    @Html.Partial(\"../Shared/_AddEdit" + this._table.Name + "Partial.cshtml\", Model.PartialModel)");
        sb.AppendLine("</div>");
        streamWriter.Write(sb.ToString());
      }
    }
  }
}
