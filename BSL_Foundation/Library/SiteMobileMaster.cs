
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class SiteMobileMaster
  {
    private string _fullFileNamePath;
    private Language _language;
    private string _fileExtension;

    internal SiteMobileMaster()
    {
    }

    internal SiteMobileMaster(string fullFileNamePath, Language language, string fileExtension)
    {
      this._fullFileNamePath = fullFileNamePath;
      this._language = language;
      this._fileExtension = fileExtension;
      this.GenerateWebForm();
      if (this._language == Language.CSharp)
        this.GenerateCodeCS();
      else
        this.GenerateCodeVB();
    }

    private void GenerateWebForm()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        string str = "C#";
        StringBuilder stringBuilder = new StringBuilder();
        if (this._language == Language.VB)
          str = "VB";
        stringBuilder.AppendLine("<%@ Master Language=\"" + str + "\" AutoEventWireup=\"true\" CodeFile=\"Site.Mobile.master" + this._fileExtension + "\" Inherits=\"ASP.Site_Mobile\" %>");
        stringBuilder.AppendLine("<%@ Register Src=\"~/ViewSwitcher.ascx\" TagPrefix=\"friendlyUrls\" TagName=\"ViewSwitcher\" %>");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("<!DOCTYPE html>");
        stringBuilder.AppendLine("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
        stringBuilder.AppendLine("<head runat=\"server\">");
        stringBuilder.AppendLine("    <meta name=\"viewport\" content=\"width=device-width\" />");
        stringBuilder.AppendLine("    <title></title>");
        stringBuilder.AppendLine("    <asp:ContentPlaceHolder runat=\"server\" ID=\"HeadContent\" />");
        stringBuilder.AppendLine("</head>");
        stringBuilder.AppendLine("<body>");
        stringBuilder.AppendLine("    <form id=\"form1\" runat=\"server\">");
        stringBuilder.AppendLine("    <div>");
        stringBuilder.AppendLine("        <h1>Mobile Master Page</h1>");
        stringBuilder.AppendLine("        <asp:ContentPlaceHolder runat=\"server\" ID=\"FeaturedContent\" />");
        stringBuilder.AppendLine("        <section class=\"content-wrapper main-content clear-fix\">");
        stringBuilder.AppendLine("            <asp:ContentPlaceHolder runat=\"server\" ID=\"MainContent\" />");
        stringBuilder.AppendLine("        </section>");
        stringBuilder.AppendLine("        <friendlyUrls:ViewSwitcher runat=\"server\" />");
        stringBuilder.AppendLine("    </div>");
        stringBuilder.AppendLine("    </form>");
        stringBuilder.AppendLine("</body>");
        stringBuilder.AppendLine("</html>");
        streamWriter.Write(stringBuilder.ToString());
      }
    }

    private void GenerateCodeCS()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath + this._fileExtension))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("using System;");
        stringBuilder.AppendLine("using System.Collections.Generic;");
        stringBuilder.AppendLine("using System.Linq;");
        stringBuilder.AppendLine("using System.Web;");
        stringBuilder.AppendLine("using System.Web.UI;");
        stringBuilder.AppendLine("using System.Web.UI.WebControls;");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("namespace ASP");
        stringBuilder.AppendLine("{");
        stringBuilder.AppendLine("    internal partial class Site_Mobile : System.Web.UI.MasterPage");
        stringBuilder.AppendLine("    {");
        stringBuilder.AppendLine("        protected void Page_Load(object sender, EventArgs e)");
        stringBuilder.AppendLine("        {");
        stringBuilder.AppendLine("        }");
        stringBuilder.AppendLine("    }");
        stringBuilder.AppendLine("}");
        streamWriter.Write(stringBuilder.ToString());
      }
    }

    private void GenerateCodeVB()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath + this._fileExtension))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Imports System");
        stringBuilder.AppendLine("Imports System.Collections.Generic");
        stringBuilder.AppendLine("Imports System.Linq");
        stringBuilder.AppendLine("Imports System.Web");
        stringBuilder.AppendLine("Imports System.Web.UI");
        stringBuilder.AppendLine("Imports System.Web.UI.WebControls");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("Namespace ASP");
        stringBuilder.AppendLine("    internal Partial Class Site_Mobile");
        stringBuilder.AppendLine("               Inherits System.Web.UI.MasterPage");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load");
        stringBuilder.AppendLine("        End Sub");
        stringBuilder.AppendLine("    End Class");
        stringBuilder.AppendLine("End Namespace");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
