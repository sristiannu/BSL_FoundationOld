
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class FunctionsFile
  {
    private string _fullFileNamePath;
    private string _webAppName;
    private string _webApiBaseAddress;
    private Language _language;
    private string _apiName;

    internal FunctionsFile()
    {
    }

    internal FunctionsFile(string fullFileNamePath, string webAppName, string apiName, Language language, string webApiBaseAddress)
    {
      this._fullFileNamePath = fullFileNamePath;
      this._webAppName = webAppName;
      this._webApiBaseAddress = webApiBaseAddress;
      this._apiName = apiName;
      this._language = language;
      if (this._language == Language.CSharp)
        this.GenerateCodeCS();
      else
        this.GenerateCodeVB();
    }

    private void GenerateCodeCS()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("using System.Text.RegularExpressions;");
        stringBuilder.AppendLine("using " + this._apiName + ".Domain;");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("namespace " + this._webAppName);
        stringBuilder.AppendLine("{");
        stringBuilder.AppendLine("    public sealed class Functions");
        stringBuilder.AppendLine("    {");
        stringBuilder.AppendLine("        private Functions()");
        stringBuilder.AppendLine("        {");
        stringBuilder.AppendLine("        }");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        private static string RemoveSpecialChars(string text)");
        stringBuilder.AppendLine("        {");
        stringBuilder.AppendLine("            Regex regex = new Regex(\"[^a-zA-Z0-9 -]\");");
        stringBuilder.AppendLine("            return regex.Replace(text, \"\");");
        stringBuilder.AppendLine("        }");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        public static int GetPagerStartPage(int currentPage, int numberOfPagesToShow, int totalPages)");
        stringBuilder.AppendLine("        {");
        stringBuilder.AppendLine("            int startPage = 1;");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            if (currentPage <= numberOfPagesToShow)");
        stringBuilder.AppendLine("                startPage = 1;");
        stringBuilder.AppendLine("            else if ((currentPage > numberOfPagesToShow) && (currentPage % numberOfPagesToShow == 0))");
        stringBuilder.AppendLine("                startPage = ((currentPage / numberOfPagesToShow) - 1) * numberOfPagesToShow + 1;");
        stringBuilder.AppendLine("            else if (currentPage > numberOfPagesToShow)");
        stringBuilder.AppendLine("                startPage = (currentPage / numberOfPagesToShow) * numberOfPagesToShow + 1;");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            return startPage;");
        stringBuilder.AppendLine("        }");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        public static int GetPagerEndPage(int startPage, int currentPage, int numberOfPagesToShow, int totalPages)");
        stringBuilder.AppendLine("        {");
        stringBuilder.AppendLine("            int endPage = startPage + (numberOfPagesToShow - 1);");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            if (endPage >= totalPages)");
        stringBuilder.AppendLine("                return totalPages;");
        stringBuilder.AppendLine("            else");
        stringBuilder.AppendLine("                return startPage + (numberOfPagesToShow - 1);");
        stringBuilder.AppendLine("        }");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        public static string GetWebApiBaseAddress()");
        stringBuilder.AppendLine("        {");
        stringBuilder.AppendLine("            return \"" + this._webApiBaseAddress + "\";");
        stringBuilder.AppendLine("        }");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        public static int GetGridNumberOfRows()");
        stringBuilder.AppendLine("        {");
        stringBuilder.AppendLine("            return 15;");
        stringBuilder.AppendLine("        }");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        public static int GetGridNumberOfPagesToShow()");
        stringBuilder.AppendLine("        {");
        stringBuilder.AppendLine("            return 10;");
        stringBuilder.AppendLine("        }");
        stringBuilder.AppendLine("    }");
        stringBuilder.AppendLine("}");
        streamWriter.Write(stringBuilder.ToString());
      }
    }

    private void GenerateCodeVB()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Imports System");
        stringBuilder.AppendLine("Imports System.Web.UI.WebControls");
        stringBuilder.AppendLine("Imports System.Configuration");
        stringBuilder.AppendLine("Imports System.Text.RegularExpressions");
        stringBuilder.AppendLine("Imports System.Web.UI");
        stringBuilder.AppendLine("Imports System.IO");
        stringBuilder.AppendLine("Imports " + this._apiName + ".Domain");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("Namespace " + this._webAppName);
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("    Public NotInheritable Class Functions");
        stringBuilder.AppendLine("        Private Sub New()");
        stringBuilder.AppendLine("        End Sub");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        Public Shared Function RemoveSpecialChars(text As String) As String");
        stringBuilder.AppendLine("            Dim regex As New Regex(\"[^a-zA-Z0-9 -]\")");
        stringBuilder.AppendLine("            Return regex.Replace(text, \"\")");
        stringBuilder.AppendLine("        End Function");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        Public Shared Function GetWhereValue(fieldName As String, data As String, type As FieldType) As String");
        stringBuilder.AppendLine("            Select Case type");
        stringBuilder.AppendLine("                Case FieldType.[String]");
        stringBuilder.AppendLine("                    Return \"[\" & fieldName & \"] LIKE '%\" & data & \"%'\"");
        stringBuilder.AppendLine("                Case FieldType.[Date]");
        stringBuilder.AppendLine("                    Return \"[\" & fieldName & \"] = '\" & data & \"'\"");
        stringBuilder.AppendLine("                Case FieldType.[Boolean]");
        stringBuilder.AppendLine("                    If data = \"false\" Then");
        stringBuilder.AppendLine("                        Return \"([\" & fieldName & \"] = 0 OR [\" & fieldName & \"] IS NULL)\"");
        stringBuilder.AppendLine("                    Else");
        stringBuilder.AppendLine("                        Return \"[\" & fieldName & \"] = 1\"");
        stringBuilder.AppendLine("                    End If");
        stringBuilder.AppendLine("                Case FieldType.Numeric");
        stringBuilder.AppendLine("                    If data = \"0\" Then");
        stringBuilder.AppendLine("                        Return \"([\" & fieldName & \"] = \" & data & \" OR [\" & fieldName & \"] IS NULL)\"");
        stringBuilder.AppendLine("                    Else");
        stringBuilder.AppendLine("                        Return \"[\" & fieldName & \"] = \" & data");
        stringBuilder.AppendLine("                    End If");
        stringBuilder.AppendLine("                Case FieldType.[Decimal]");
        stringBuilder.AppendLine("                    If data = \"0\" OrElse data = \"0.0\" OrElse data = \"0.00\" Then");
        stringBuilder.AppendLine("                        Return \"([\" & fieldName & \"] = \" & data & \" OR [\" & fieldName & \"] IS NULL)\"");
        stringBuilder.AppendLine("                    Else");
        stringBuilder.AppendLine("                        Return \"[\" & fieldName & \"] = \" & data");
        stringBuilder.AppendLine("                    End If");
        stringBuilder.AppendLine("                Case Else");
        stringBuilder.AppendLine("                    Return \"[\" & fieldName & \"] = '\" & data & \"'\"");
        stringBuilder.AppendLine("            End Select");
        stringBuilder.AppendLine("        End Function");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        Public Shared Function GetPagerStartPage(currentPage As Integer, numberOfPagesToShow As Integer, totalPages  As Integer) As Integer");
        stringBuilder.AppendLine("            Dim startPage As Integer = 1");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            If currentPage <= numberOfPagesToShow Then");
        stringBuilder.AppendLine("                startPage = 1");
        stringBuilder.AppendLine("            ElseIf (currentPage > numberOfPagesToShow) AndAlso (currentPage Mod numberOfPagesToShow = 0) Then");
        stringBuilder.AppendLine("                startPage = ((currentPage \\ numberOfPagesToShow) - 1) * numberOfPagesToShow + 1");
        stringBuilder.AppendLine("            Else if currentPage > numberOfPagesToShow Then");
        stringBuilder.AppendLine("                startPage = (currentPage \\ numberOfPagesToShow) * numberOfPagesToShow + 1");
        stringBuilder.AppendLine("            End If");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            Return startPage");
        stringBuilder.AppendLine("        End Function");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        Public Shared Function GetPagerEndPage(startPage As Integer, currentPage As Integer, numberOfPagesToShow As Integer, totalPages As Integer) As Integer");
        stringBuilder.AppendLine("            Dim endPage As Integer = startPage + (numberOfPagesToShow - 1)");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            If endPage >= totalPages Then");
        stringBuilder.AppendLine("                Return totalPages");
        stringBuilder.AppendLine("            Else");
        stringBuilder.AppendLine("                Return startPage + (numberOfPagesToShow - 1)");
        stringBuilder.AppendLine("            End If");
        stringBuilder.AppendLine("        End Function");
        stringBuilder.AppendLine("    End Class");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("End Namespace");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
