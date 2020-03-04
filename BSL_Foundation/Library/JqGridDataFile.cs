
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class JqGridDataFile
  {
    private string _fullFileNamePath;
    private string _websiteName;
    private ApplicationType _appType;

    private JqGridDataFile()
    {
    }

    internal JqGridDataFile(string fullFileNamePath, Language language, string websiteName, ApplicationType appType = ApplicationType.ASPNET)
    {
      this._fullFileNamePath = fullFileNamePath;
      this._websiteName = websiteName.Trim();
      this._appType = appType;
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
        stringBuilder.AppendLine("using System;");
        stringBuilder.AppendLine("using System.Collections.Generic;");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("namespace " + this._websiteName + ".Domain");
        stringBuilder.AppendLine("{");
        stringBuilder.AppendLine("    internal class JqGridData");
        stringBuilder.AppendLine("    {");
        stringBuilder.AppendLine("        internal int total { get; set; }");
        stringBuilder.AppendLine("        internal int page { get; set; }");
        stringBuilder.AppendLine("        internal int records { get; set; }");
        stringBuilder.AppendLine("        internal List<JqGridRow> rows { get; set; }");
        stringBuilder.AppendLine("    }");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("    internal class JqGridRow");
        stringBuilder.AppendLine("    {");
        stringBuilder.AppendLine("        internal object id { get; set; }");
        stringBuilder.AppendLine("        internal List<string> cell { get; set; }");
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
        stringBuilder.AppendLine("Imports System.Collections.Generic");
        stringBuilder.AppendLine("");
        if (this._appType == ApplicationType.ASPNETMVC)
          stringBuilder.AppendLine("Namespace Domain");
        else
          stringBuilder.AppendLine("Namespace " + this._websiteName + ".Domain");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("    internal Class JqGridData");
        stringBuilder.AppendLine("        Private _total As Integer");
        stringBuilder.AppendLine("        Private _page As Integer");
        stringBuilder.AppendLine("        Private _records As Integer");
        stringBuilder.AppendLine("        Private _rows As List(Of JqGridRow)");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        internal Property total() As Integer");
        stringBuilder.AppendLine("            Get");
        stringBuilder.AppendLine("                Return _total");
        stringBuilder.AppendLine("            End Get");
        stringBuilder.AppendLine("            Set");
        stringBuilder.AppendLine("                _total = Value");
        stringBuilder.AppendLine("            End Set");
        stringBuilder.AppendLine("        End Property");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        internal Property page() As Integer");
        stringBuilder.AppendLine("            Get");
        stringBuilder.AppendLine("                Return _page");
        stringBuilder.AppendLine("            End Get");
        stringBuilder.AppendLine("            Set");
        stringBuilder.AppendLine("                _page = Value");
        stringBuilder.AppendLine("            End Set");
        stringBuilder.AppendLine("        End Property");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        internal Property records() As Integer");
        stringBuilder.AppendLine("            Get");
        stringBuilder.AppendLine("                Return _records");
        stringBuilder.AppendLine("            End Get");
        stringBuilder.AppendLine("            Set");
        stringBuilder.AppendLine("                _records = Value");
        stringBuilder.AppendLine("            End Set");
        stringBuilder.AppendLine("        End Property");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        internal Property rows() As List(Of JqGridRow)");
        stringBuilder.AppendLine("            Get");
        stringBuilder.AppendLine("                Return _rows");
        stringBuilder.AppendLine("            End Get");
        stringBuilder.AppendLine("            Set");
        stringBuilder.AppendLine("                _rows = Value");
        stringBuilder.AppendLine("            End Set");
        stringBuilder.AppendLine("        End Property");
        stringBuilder.AppendLine("    End Class");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("    internal Class JqGridRow");
        stringBuilder.AppendLine("        Private _id As Object");
        stringBuilder.AppendLine("        Private _cell As List(Of String)");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        internal Property id() As Object");
        stringBuilder.AppendLine("            Get");
        stringBuilder.AppendLine("                Return _id");
        stringBuilder.AppendLine("            End Get");
        stringBuilder.AppendLine("            Set");
        stringBuilder.AppendLine("                _id = Value");
        stringBuilder.AppendLine("            End Set");
        stringBuilder.AppendLine("        End Property");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        internal Property cell() As List(Of String)");
        stringBuilder.AppendLine("            Get");
        stringBuilder.AppendLine("                Return _cell");
        stringBuilder.AppendLine("            End Get");
        stringBuilder.AppendLine("            Set");
        stringBuilder.AppendLine("                _cell = Value");
        stringBuilder.AppendLine("            End Set");
        stringBuilder.AppendLine("        End Property");
        stringBuilder.AppendLine("    End Class");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("End Namespace");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
