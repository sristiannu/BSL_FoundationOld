
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class FieldTypeFile
  {
    private string _fullFileNamePath;
    private string _apiName;

    private FieldTypeFile()
    {
    }

    internal FieldTypeFile(string fullFileNamePath, Language language, string apiName)
    {
      this._fullFileNamePath = fullFileNamePath;
      this._apiName = apiName.Trim();
      if (language == Language.CSharp)
        this.GenerateCS();
      else
        this.GenerateVB();
    }

    private void GenerateCS()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("namespace " + this._apiName + ".Domain");
        stringBuilder.AppendLine("{");
        stringBuilder.AppendLine("    public enum FieldType");
        stringBuilder.AppendLine("    {");
        stringBuilder.AppendLine("        Default,");
        stringBuilder.AppendLine("        String,");
        stringBuilder.AppendLine("        Date,");
        stringBuilder.AppendLine("        Boolean,");
        stringBuilder.AppendLine("        Numeric,");
        stringBuilder.AppendLine("        Decimal,");
        stringBuilder.AppendLine("    }");
        stringBuilder.AppendLine("}");
        streamWriter.Write(stringBuilder.ToString());
      }
    }

    private void GenerateVB()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Imports System.Web");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("Namespace Domain");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("    internal Enum FieldType");
        stringBuilder.AppendLine("        [Default]");
        stringBuilder.AppendLine("        [String]");
        stringBuilder.AppendLine("        [Date]");
        stringBuilder.AppendLine("        [Boolean]");
        stringBuilder.AppendLine("        [Numeric]");
        stringBuilder.AppendLine("        [Decimal]");
        stringBuilder.AppendLine("    End Enum");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("End Namespace");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
