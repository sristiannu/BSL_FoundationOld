
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class MaskPartialView
  {
    private const Language _language = Language.CSharp;
    private const string _fileExtension = ".cs";
    private Table _table;
    private string _directory;

    internal MaskPartialView(Table table, string webAppRootDirectory)
    {
      this._table = table;
      this._directory = webAppRootDirectory + MyConstants.DirectoryRazorPageShared + "\\";
      this.Generate();
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._directory + "_Mask" + this._table.Name + "Partial.cshtml"))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("<script src=\"~/js/jquery.maskedinput.min.js\" asp-append-version=\"true\"></script>");
        stringBuilder.AppendLine("<script>");
        stringBuilder.AppendLine("    $(function () {");
        foreach (Column column in (List<Column>) this._table.Columns)
        {
          if (!string.IsNullOrEmpty(column.Mask))
            stringBuilder.AppendLine("        $(\"#" + this._table.Name + "Model_" + column.Name + "\").mask(\"" + column.Mask + "\");");
        }
        stringBuilder.AppendLine("    });");
        stringBuilder.AppendLine("</script>");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
