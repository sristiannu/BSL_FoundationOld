
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class BundleConfig
  {
    private string _fullFileNamePath;

    internal BundleConfig()
    {
    }

    internal BundleConfig(string fullFileNamePath)
    {
      this._fullFileNamePath = fullFileNamePath;
      this.GenerateCode();
    }

    private void GenerateCode()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("// Configure bundling and minification for the project.");
        stringBuilder.AppendLine("// More info at https://go.microsoft.com/fwlink/?LinkId=808241");
        stringBuilder.AppendLine("  [");
        stringBuilder.AppendLine("  {");
        stringBuilder.AppendLine("    \"outputFileName\": \"wwwroot/css/site.min.css\",");
        stringBuilder.AppendLine("    // An array of relative input file paths. Globbing patterns supported");
        stringBuilder.AppendLine("    \"inputFiles\": [");
        stringBuilder.AppendLine("      \"wwwroot/css/site.css\"");
        stringBuilder.AppendLine("    ]");
        stringBuilder.AppendLine("  },");
        stringBuilder.AppendLine("  {");
        stringBuilder.AppendLine("    \"outputFileName\": \"wwwroot/js/site.min.js\",");
        stringBuilder.AppendLine("    \"inputFiles\": [");
        stringBuilder.AppendLine("      \"wwwroot/js/site.js\"");
        stringBuilder.AppendLine("    ],");
        stringBuilder.AppendLine("    // Optionally specify minification options");
        stringBuilder.AppendLine("    \"minify\": {");
        stringBuilder.AppendLine("      \"enabled\": true,");
        stringBuilder.AppendLine("      \"renameLocals\": true");
        stringBuilder.AppendLine("    },");
        stringBuilder.AppendLine("    // Optionally generate .map file");
        stringBuilder.AppendLine("    \"sourceMap\": false");
        stringBuilder.AppendLine("  }");
        stringBuilder.AppendLine("]");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
