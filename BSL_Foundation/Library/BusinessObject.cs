
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
    internal class BusinessObject
    {
        private Table _table;
        private const string _fileExtension = ".cs";
        private string _apiName;
        private string _apiNameDirectory;

        internal BusinessObject()
        {
        }

        internal BusinessObject(Table table, string apiName, string apiNameDirectory)
        {
            this._table = table;
            this._apiName = apiName;
            this._apiNameDirectory = apiNameDirectory + MyConstants.DirectoryBusinessObject;
            this.Generate();
        }

        private void Generate()
        {
            string path = this._apiNameDirectory + this._table.Name + ".cs";
            if (File.Exists(path))
                return;
            using (StreamWriter streamWriter = new StreamWriter(path))
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("using System;");
                stringBuilder.AppendLine("using " + this._apiName + ".BusinessObject.Base;");
                stringBuilder.AppendLine("");
                stringBuilder.AppendLine("namespace " + this._apiName + ".BusinessObject");
                stringBuilder.AppendLine("{");
                stringBuilder.AppendLine("     /// <summary>");
                stringBuilder.AppendLine("     /// This file will not be overwritten.  You can put");
                stringBuilder.AppendLine("     /// additional " + this._table.Name + " Business Layer code in this class.");
                stringBuilder.AppendLine("     /// </summary>");
                stringBuilder.AppendLine("     public partial class " + this._table.Name + " : " + this._table.Name + "Base");
                stringBuilder.AppendLine("     {");
                stringBuilder.AppendLine("        public " + this._table.Name + " ShallowCopy()");
                stringBuilder.AppendLine("        {");
                stringBuilder.AppendLine("            return (" + this._table.Name + ")this.MemberwiseClone();");
                stringBuilder.AppendLine("        }");
                stringBuilder.AppendLine("     }");
                stringBuilder.AppendLine("}");
                streamWriter.Write(stringBuilder.ToString());
            }
        }

        private void WriteConstructor(StringBuilder sb)
        {
            sb.Append("         // constructor");
            sb.Append("         public " + this._table.Name + "()");
            sb.Append("         {");
            sb.Append("         }");
        }
    }
}
