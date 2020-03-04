
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class CrudOperationFile
  {
    private string _fullFileNamePath;
    private string _apiName;

    private CrudOperationFile()
    {
    }

    internal CrudOperationFile(string fullFileNamePath, Language language, string apiName)
    {
      this._fullFileNamePath = fullFileNamePath;
      this._apiName = apiName;
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
        stringBuilder.AppendLine("    public enum CrudOperation");
        stringBuilder.AppendLine("    {");
        stringBuilder.AppendLine("        Add,");
        stringBuilder.AppendLine("        Update,");
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
        stringBuilder.AppendLine("    internal Enum CrudOperation");
        stringBuilder.AppendLine("        Add");
        stringBuilder.AppendLine("        Update");
        stringBuilder.AppendLine("    End Enum");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("End Namespace");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
