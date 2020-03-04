
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class ViewSwitcherUserControl
  {
    private string _fullFileNamePath;
    private Language _language;
    private string _fileExtension;

    internal ViewSwitcherUserControl()
    {
    }

    internal ViewSwitcherUserControl(string fullFileNamePath, Language language, string fileExtension)
    {
      this._fullFileNamePath = fullFileNamePath;
      this._language = language;
      this._fileExtension = fileExtension;
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
        stringBuilder.AppendLine("<%@ Control Language=\"C#\" AutoEventWireup=\"true\" CodeFile=\"ViewSwitcher.ascx.cs\" Inherits=\"ASP.ViewSwitcher\" %>");
        stringBuilder.AppendLine("<div id=\"viewSwitcher\">");
        stringBuilder.AppendLine("    <%: CurrentView %> view | <a href=\"<%: SwitchUrl %>\">Switch to <%: AlternateView %></a>");
        stringBuilder.AppendLine("</div>");
        streamWriter.Write(stringBuilder.ToString());
      }
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath + this._fileExtension))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("using System;");
        stringBuilder.AppendLine("using System.Collections.Generic;");
        stringBuilder.AppendLine("using System.Linq;");
        stringBuilder.AppendLine("using System.Web;");
        stringBuilder.AppendLine("using System.Web.Routing;");
        stringBuilder.AppendLine("using System.Web.UI;");
        stringBuilder.AppendLine("using System.Web.UI.WebControls;");
        stringBuilder.AppendLine("using Microsoft.AspNet.FriendlyUrls.Resolvers;");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("namespace ASP");
        stringBuilder.AppendLine("{");
        stringBuilder.AppendLine("    internal partial class ViewSwitcher : System.Web.UI.UserControl");
        stringBuilder.AppendLine("    {");
        stringBuilder.AppendLine("        protected string CurrentView { get; private set; }");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        protected string AlternateView { get; private set; }");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        protected string SwitchUrl { get; private set; }");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        protected void Page_Load(object sender, EventArgs e)");
        stringBuilder.AppendLine("        {");
        stringBuilder.AppendLine("            // Determine current view");
        stringBuilder.AppendLine("            var isMobile = WebFormsFriendlyUrlResolver.IsMobileView(new HttpContextWrapper(Context));");
        stringBuilder.AppendLine("            CurrentView = isMobile ? \"Mobile\" : \"Desktop\";");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            // Determine alternate view");
        stringBuilder.AppendLine("            AlternateView = isMobile ? \"Desktop\" : \"Mobile\";");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            // Create switch URL from the route, e.g. ~/__FriendlyUrls_SwitchView/Mobile?ReturnUrl=/Page");
        stringBuilder.AppendLine("            var switchViewRouteName = \"AspNet.FriendlyUrls.SwitchView\";");
        stringBuilder.AppendLine("            var switchViewRoute = RouteTable.Routes[switchViewRouteName];");
        stringBuilder.AppendLine("            if (switchViewRoute is null)");
        stringBuilder.AppendLine("            {");
        stringBuilder.AppendLine("                // Friendly URLs is not enabled or the name of the swith view route is out of sync");
        stringBuilder.AppendLine("                this.Visible = false;");
        stringBuilder.AppendLine("                return;");
        stringBuilder.AppendLine("            }");
        stringBuilder.AppendLine("            var url = GetRouteUrl(switchViewRouteName, new { view = AlternateView });");
        stringBuilder.AppendLine("            url += \"?ReturnUrl=\" + HttpUtility.UrlEncode(Request.RawUrl);");
        stringBuilder.AppendLine("            SwitchUrl = url;");
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
        stringBuilder.AppendLine("<%@ Control Language=\"VB\" AutoEventWireup=\"true\" CodeFile=\"ViewSwitcher.ascx.vb\" Inherits=\"ASP.ViewSwitcher\" %>");
        stringBuilder.AppendLine("<div id=\"viewSwitcher\">");
        stringBuilder.AppendLine("    <%: CurrentView %> view | <a href=\"<%: SwitchUrl %>\">Switch to <%: AlternateView %></a>");
        stringBuilder.AppendLine("</div>");
        streamWriter.Write(stringBuilder.ToString());
      }
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath + this._fileExtension))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Imports System");
        stringBuilder.AppendLine("Imports System.Collections.Generic");
        stringBuilder.AppendLine("Imports System.Linq");
        stringBuilder.AppendLine("Imports System.Web");
        stringBuilder.AppendLine("Imports System.Web.Routing");
        stringBuilder.AppendLine("Imports System.Web.UI");
        stringBuilder.AppendLine("Imports System.Web.UI.WebControls");
        stringBuilder.AppendLine("Imports Microsoft.AspNet.FriendlyUrls.Resolvers");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("Namespace ASP");
        stringBuilder.AppendLine("    internal Partial Class ViewSwitcher");
        stringBuilder.AppendLine("               Inherits System.Web.UI.UserControl");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        Private _currentView As String");
        stringBuilder.AppendLine("        Private _alternateView As String");
        stringBuilder.AppendLine("        Private _switchUrl As String");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        internal Property CurrentView() As String");
        stringBuilder.AppendLine("            Get");
        stringBuilder.AppendLine("                Return _currentView");
        stringBuilder.AppendLine("            End Get");
        stringBuilder.AppendLine("            Set(ByVal value As String)");
        stringBuilder.AppendLine("                _currentView = value");
        stringBuilder.AppendLine("            End Set");
        stringBuilder.AppendLine("        End Property");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        internal Property AlternateView() As String");
        stringBuilder.AppendLine("            Get");
        stringBuilder.AppendLine("                Return _alternateView");
        stringBuilder.AppendLine("            End Get");
        stringBuilder.AppendLine("            Set(ByVal value As String)");
        stringBuilder.AppendLine("                _alternateView = value");
        stringBuilder.AppendLine("            End Set");
        stringBuilder.AppendLine("        End Property");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        internal Property SwitchUrl() As String");
        stringBuilder.AppendLine("            Get");
        stringBuilder.AppendLine("                Return _switchUrl");
        stringBuilder.AppendLine("            End Get");
        stringBuilder.AppendLine("            Set(ByVal value As String)");
        stringBuilder.AppendLine("                _switchUrl = value");
        stringBuilder.AppendLine("            End Set");
        stringBuilder.AppendLine("        End Property");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load");
        stringBuilder.AppendLine("            ' Determine current view");
        stringBuilder.AppendLine("            Dim isMobile = WebFormsFriendlyUrlResolver.IsMobileView(New HttpContextWrapper(Context))");
        stringBuilder.AppendLine("            CurrentView = If(isMobile, \"Mobile\", \"Desktop\")");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            ' Determine alternate view");
        stringBuilder.AppendLine("            AlternateView = If(isMobile, \"Desktop\", \"Mobile\")");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            ' Create switch URL from the route, e.g. ~/__FriendlyUrls_SwitchView/Mobile?ReturnUrl=/Page");
        stringBuilder.AppendLine("            Dim switchViewRouteName = \"AspNet.FriendlyUrls.SwitchView\"");
        stringBuilder.AppendLine("            Dim switchViewRoute = RouteTable.Routes(switchViewRouteName)");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            If switchViewRoute Is Nothing Then");
        stringBuilder.AppendLine("                ' Friendly URLs is not enabled or the name of the swith view route is out of sync");
        stringBuilder.AppendLine("                Me.Visible = False");
        stringBuilder.AppendLine("                Return");
        stringBuilder.AppendLine("            End If");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            Dim url = GetRouteUrl(switchViewRouteName, New With { _");
        stringBuilder.AppendLine("                      .view = AlternateView _");
        stringBuilder.AppendLine("            })");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            url += \"?ReturnUrl=\" + HttpUtility.UrlEncode(Request.RawUrl)");
        stringBuilder.AppendLine("            SwitchUrl = url");
        stringBuilder.AppendLine("        End Sub");
        stringBuilder.AppendLine("    End Class");
        stringBuilder.AppendLine("End Namespace");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
