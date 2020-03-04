
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal sealed class LazyFile
  {
    private string _fileExtension;
    private string _nameSpace;
    private string _directory;

    private LazyFile()
    {
    }

    private LazyFile(string fileExtension, string nameSpace, string path, string helpertDirectory)
    {
      this._fileExtension = fileExtension;
      this._nameSpace = nameSpace;
      this._directory = path + helpertDirectory;
      this.Generate();
    }

    private void Generate()
    {
      string path = this._directory + "Lazy" + this._fileExtension;
      if (File.Exists(path))
        return;
      using (StreamWriter streamWriter = new StreamWriter(path))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("using System;");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("namespace " + this._nameSpace + ".BusinessObject");
        stringBuilder.AppendLine("{");
        stringBuilder.AppendLine("    internal class Lazy<T> where T : new()");
        stringBuilder.AppendLine("    {");
        stringBuilder.AppendLine("        private readonly Func<T> _initializer;");
        stringBuilder.AppendLine("        private bool _isValueCreated;");
        stringBuilder.AppendLine("        private T _value;");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        internal Lazy(Func<T> initializer)");
        stringBuilder.AppendLine("        {");
        stringBuilder.AppendLine("            if (initializer is null)");
        stringBuilder.AppendLine("                throw new ArgumentNullException(\"initializer is Nullable\");");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            this._initializer = initializer;");
        stringBuilder.AppendLine("        }");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        internal bool IsValueCreated");
        stringBuilder.AppendLine("        {");
        stringBuilder.AppendLine("            get { return _isValueCreated; }");
        stringBuilder.AppendLine("        }");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        internal T Value");
        stringBuilder.AppendLine("        {");
        stringBuilder.AppendLine("            get");
        stringBuilder.AppendLine("            {");
        stringBuilder.AppendLine("                if (!_isValueCreated)");
        stringBuilder.AppendLine("                {");
        stringBuilder.AppendLine("                    _value = _initializer();");
        stringBuilder.AppendLine("                    _isValueCreated = true;");
        stringBuilder.AppendLine("                }");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("                return _value;");
        stringBuilder.AppendLine("            }");
        stringBuilder.AppendLine("        }");
        stringBuilder.AppendLine("    }");
        stringBuilder.AppendLine("}");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
