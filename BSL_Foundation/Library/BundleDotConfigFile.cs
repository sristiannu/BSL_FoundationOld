
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class BundleDotConfigFile
  {
    private string _fullFileNamePath;
    private string _jqueryTheme;

    internal BundleDotConfigFile()
    {
    }

    internal BundleDotConfigFile(string fullFileNamePath, string jqueryTheme)
    {
      this._fullFileNamePath = fullFileNamePath;
      this._jqueryTheme = jqueryTheme;
      this.Generate();
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
        stringBuilder.AppendLine("<bundles version=\"1.0\">");
        stringBuilder.AppendLine("  <styleBundle path=\"~/Styles\">");
        stringBuilder.AppendLine("      <include path=\"~/Styles/normalize.css\" />");
        stringBuilder.AppendLine("      <include path=\"~/Styles/global.css\" />");
        stringBuilder.AppendLine("  </styleBundle>");
        stringBuilder.AppendLine("  <styleBundle path=\"~/Styles/themes/base/css\">");
        stringBuilder.AppendLine("    <include path=\"~/Styles/themes/base/jquery.ui.core.css\" />");
        stringBuilder.AppendLine("    <include path=\"~/Styles/themes/base/jquery.ui.resizable.css\" />");
        stringBuilder.AppendLine("    <include path=\"~/Styles/themes/base/jquery.ui.selectable.css\" />");
        stringBuilder.AppendLine("    <include path=\"~/Styles/themes/base/jquery.ui.accordion.css\" />");
        stringBuilder.AppendLine("    <include path=\"~/Styles/themes/base/jquery.ui.autocomplete.css\" />");
        stringBuilder.AppendLine("    <include path=\"~/Styles/themes/base/jquery.ui.button.css\" />");
        stringBuilder.AppendLine("    <include path=\"~/Styles/themes/base/jquery.ui.dialog.css\" />");
        stringBuilder.AppendLine("    <include path=\"~/Styles/themes/base/jquery.ui.slider.css\" />");
        stringBuilder.AppendLine("    <include path=\"~/Styles/themes/base/jquery.ui.tabs.css\" />");
        stringBuilder.AppendLine("    <include path=\"~/Styles/themes/base/jquery.ui.datepicker.css\" />");
        stringBuilder.AppendLine("    <include path=\"~/Styles/themes/base/jquery.ui.progressbar.css\" />");
        stringBuilder.AppendLine("    <include path=\"~/Styles/themes/" + this._jqueryTheme + "/jquery-ui-1.9.2.custom.css\" />");
        stringBuilder.AppendLine("  </styleBundle>");
        stringBuilder.AppendLine("</bundles>");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
