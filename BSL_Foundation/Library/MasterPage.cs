
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class MasterPage
  {
    private string _fileExtension;
    private string _nameSpace;
    private string _fullFileNamePath;
    private ApplicationType _appType;
    private Language _language;
    private string _languageAbbreviation;
    private string _masterPageName;
    private string _webSiteName;

    internal MasterPage()
    {
    }

    internal MasterPage(string fullFileNamePath, string fileExtension, string nameSpace, string path, Language language, string languageAbbreviation, ApplicationType appType = ApplicationType.ASPNET, string webSiteName = "")
    {
      this._fullFileNamePath = fullFileNamePath;
      this._fileExtension = fileExtension;
      this._nameSpace = nameSpace;
      this._language = language;
      this._languageAbbreviation = languageAbbreviation;
      this._appType = appType;
      this._masterPageName = nameof (MasterPage);
      this._webSiteName = webSiteName;
      this.GenerateWebForm();
      if (this._appType == ApplicationType.ASPNET45)
        this._masterPageName = "Site";
      if (this._language == Language.CSharp)
        this.GenerateCodeCS();
      else
        this.GenerateCodeVB();
    }

    private void GenerateWebForm()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        if (this._appType == ApplicationType.ASPNET45)
        {
          stringBuilder.AppendLine("<%@ Master Language=\"" + this._languageAbbreviation + "\" AutoEventWireup=\"true\" CodeFile=\"Site.master" + this._fileExtension + "\" Inherits=\"" + this._nameSpace + ".Site\" %>");
          stringBuilder.AppendLine("");
          stringBuilder.AppendLine("<!DOCTYPE html>");
          stringBuilder.AppendLine("<!--[if lt IE 7]>      <html class=\"no-js lt-ie9 lt-ie8 lt-ie7\"> <![endif]-->");
          stringBuilder.AppendLine("<!--[if IE 7]>         <html class=\"no-js lt-ie9 lt-ie8\"> <![endif]-->");
          stringBuilder.AppendLine("<!--[if IE 8]>         <html class=\"no-js lt-ie9\"> <![endif]-->");
          stringBuilder.AppendLine("<!--[if gt IE 8]><!--> <html class=\"no-js\"> <!--<![endif]-->");
          stringBuilder.AppendLine("<head id=\"Head1\" runat=\"server\">");
          stringBuilder.AppendLine("    <meta charset=\"utf-8\" />");
          stringBuilder.AppendLine("    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge,chrome=1\">");
          stringBuilder.AppendLine("    <title><%: Page.Title %> - My ASP.NET Application</title>");
          stringBuilder.AppendLine("    <meta name=\"viewport\" content=\"width=device-width\" />");
          stringBuilder.AppendLine("    <link href=\"~/favicon.ico\" rel=\"shortcut icon\" type=\"image/x-icon\" />");
          stringBuilder.AppendLine("    <webopt:BundleReference ID=\"BundleReference1\" runat=\"server\" Path=\"~/Styles\" /> ");
          stringBuilder.AppendLine("    <asp:PlaceHolder ID=\"PlaceHolder1\" runat=\"server\"> ");
          stringBuilder.AppendLine("        <%: Styles.Render(\"~/Styles/themes/base/css\") %>");
          stringBuilder.AppendLine("        <script src=\"//ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js\"></script>");
          stringBuilder.AppendLine("        <script src=\"//ajax.googleapis.com/ajax/libs/jqueryui/1.9.2/jquery-ui.min.js\"></script>");
          stringBuilder.AppendLine("        <script>");
          stringBuilder.AppendLine("            window.jQuery ||");
          stringBuilder.AppendLine("            document.write('<script src=\"<%: Scripts.Url(\"~/Scripts/jquery-1.8.3.js\") %>\"><\\/script>');");
          stringBuilder.AppendLine("            document.write('<script src=\"<%: Scripts.Url(\"~/Scripts/jquery-ui-1.9.2.js\") %>\"><\\/script>');");
          stringBuilder.AppendLine("        </script>");
          stringBuilder.AppendLine("        <%: Scripts.Render(\"~/bundles/modernizr\") %>");
          stringBuilder.AppendLine("    </asp:PlaceHolder>");
          stringBuilder.AppendLine("    <asp:ContentPlaceHolder runat=\"server\" ID=\"HeadContent\" />");
          stringBuilder.AppendLine("</head>");
          stringBuilder.AppendLine("<body>");
          stringBuilder.AppendLine("    <!--[if lt IE 7]>");
          stringBuilder.AppendLine("        <p class=\"chromeframe\">You are using an <strong>outdated</strong> browser. Please <a href=\"http://browsehappy.com/\">upgrade your browser</a> or <a href=\"http://www.google.com/chromeframe/?redirect=true\">activate Google Chrome Frame</a> to improve your experience.</p>");
          stringBuilder.AppendLine("    <![endif]-->");
          stringBuilder.AppendLine("");
          stringBuilder.AppendLine("    <!-- Add your site or application content here -->");
          stringBuilder.AppendLine("    <form id=\"MasterPageForm1\" runat=\"server\">");
          stringBuilder.AppendLine("        <header style=\"text-align: left; padding-bottom: 20px;\">");
          stringBuilder.AppendLine("            <a class=\"visuallyhidden\" href=\"#main\">Skip Navigation</a><!-- show to screen readers only -->");
          stringBuilder.AppendLine("            <h2>" + this._webSiteName + "</h2>");
          stringBuilder.AppendLine("        </header>");
          stringBuilder.AppendLine("");
          stringBuilder.AppendLine("        <asp:ScriptManager ID=\"ScriptManager1\" runat=\"server\">");
          stringBuilder.AppendLine("            <Scripts>");
          stringBuilder.AppendLine("                <%--Framework scripts--%>");
          stringBuilder.AppendLine("                <asp:ScriptReference Name=\"MsAjaxBundle\" />");
          stringBuilder.AppendLine("                <asp:ScriptReference Name=\"WebForms.js\" Assembly=\"System.Web\" Path=\"~/Scripts/WebForms/WebForms.js\" />");
          stringBuilder.AppendLine("                <asp:ScriptReference Name=\"WebUIValidation.js\" Assembly=\"System.Web\" Path=\"~/Scripts/WebForms/WebUIValidation.js\" />");
          stringBuilder.AppendLine("                <asp:ScriptReference Name=\"MenuStandards.js\" Assembly=\"System.Web\" Path=\"~/Scripts/WebForms/MenuStandards.js\" />");
          stringBuilder.AppendLine("                <asp:ScriptReference Name=\"GridView.js\" Assembly=\"System.Web\" Path=\"~/Scripts/WebForms/GridView.js\" />");
          stringBuilder.AppendLine("                <asp:ScriptReference Name=\"DetailsView.js\" Assembly=\"System.Web\" Path=\"~/Scripts/WebForms/DetailsView.js\" />");
          stringBuilder.AppendLine("                <asp:ScriptReference Name=\"TreeView.js\" Assembly=\"System.Web\" Path=\"~/Scripts/WebForms/TreeView.js\" />");
          stringBuilder.AppendLine("                <asp:ScriptReference Name=\"WebParts.js\" Assembly=\"System.Web\" Path=\"~/Scripts/WebForms/WebParts.js\" />");
          stringBuilder.AppendLine("                <asp:ScriptReference Name=\"Focus.js\" Assembly=\"System.Web\" Path=\"~/Scripts/WebForms/Focus.js\" />");
          stringBuilder.AppendLine("                <asp:ScriptReference Name=\"WebFormsBundle\" />");
          stringBuilder.AppendLine("                <%--Site scripts--%>");
          stringBuilder.AppendLine("            </Scripts>");
          stringBuilder.AppendLine("        </asp:ScriptManager>");
          stringBuilder.AppendLine("");
          stringBuilder.AppendLine("        <div class=\"maincontent\">");
          stringBuilder.AppendLine("            <asp:ContentPlaceHolder runat=\"server\" ID=\"MainContent\" />");
          stringBuilder.AppendLine("        </div>");
          stringBuilder.AppendLine("");
          stringBuilder.AppendLine("        <footer style=\"text-align: left; padding-top: 30px;\">");
          stringBuilder.AppendLine("            <asp:HyperLink ID=\"HlnkHome\" Text=\"Back to home page\" NavigateUrl=\"~/Default.htm\" runat=\"server\" />");
          stringBuilder.AppendLine("        </footer>");
          stringBuilder.AppendLine("");
          stringBuilder.AppendLine("        <%: Scripts.Render(\"~/Scripts/plugins.js\") %>");
          stringBuilder.AppendLine("        <%: Scripts.Render(\"~/Scripts/main.js\") %>");
          stringBuilder.AppendLine("");
          stringBuilder.AppendLine("        <!-- Google Analytics: change UA-XXXXX-X to be your site's ID. -->");
          stringBuilder.AppendLine("        <script>");
          stringBuilder.AppendLine("            var _gaq = [['_setAccount', 'UA-XXXXX-X'], ['_trackPageview']];");
          stringBuilder.AppendLine("            (function (d, t) {");
          stringBuilder.AppendLine("                var g = d.createElement(t), s = d.getElementsByTagName(t)[0];");
          stringBuilder.AppendLine("                g.src = ('https:' == location.protocol ? '//ssl' : '//www') + '.google-analytics.com/ga.js';");
          stringBuilder.AppendLine("                s.parentNode.insertBefore(g, s)");
          stringBuilder.AppendLine("            }(document, 'script'));");
          stringBuilder.AppendLine("        </script>");
          stringBuilder.AppendLine("    </form>");
          stringBuilder.AppendLine("</body>");
          stringBuilder.AppendLine("</html>");
        }
        else
        {
          stringBuilder.AppendLine("<%@ Master Language=\"" + this._languageAbbreviation + "\" AutoEventWireup=\"true\" CodeFile=\"" + MyConstants.WordMasterPage + MyConstants.WordDotMaster + this._fileExtension + "\" Inherits=\"" + this._nameSpace + ".MasterPage\" %>");
          stringBuilder.AppendLine("");
          stringBuilder.AppendLine("<!DOCTYPE html internal \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
          stringBuilder.AppendLine("");
          stringBuilder.AppendLine("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
          stringBuilder.AppendLine("<head runat=\"server\">");
          stringBuilder.AppendLine("    <title></title>");
          stringBuilder.AppendLine("    <link rel=\"stylesheet\" type=\"text/css\" href=\"Styles/global.css\" />");
          stringBuilder.AppendLine("    <asp:ContentPlaceHolder id=\"head\" runat=\"server\">");
          stringBuilder.AppendLine("    </asp:ContentPlaceHolder>");
          stringBuilder.AppendLine("</head>");
          stringBuilder.AppendLine("<body>");
          stringBuilder.AppendLine("    <form id=\"MasterPageForm1\" class=\"cmxform\" runat=\"server\">");
          stringBuilder.AppendLine("    <div>");
          stringBuilder.AppendLine("        <asp:ContentPlaceHolder id=\"ContentPlaceHolder1\" runat=\"server\">");
          stringBuilder.AppendLine("");
          stringBuilder.AppendLine("        </asp:ContentPlaceHolder>");
          stringBuilder.AppendLine("    </div>");
          stringBuilder.AppendLine("    </form>");
          stringBuilder.AppendLine("</body>");
          stringBuilder.AppendLine("</html>");
        }
        streamWriter.Write(stringBuilder.ToString());
      }
    }

    private void GenerateCodeCS()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath + this._fileExtension))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("using System;");
        if (this._appType == ApplicationType.ASPNET45)
        {
          stringBuilder.AppendLine("using System.Web;");
          stringBuilder.AppendLine("using System.Web.Security;");
          stringBuilder.AppendLine("using System.Web.UI;");
        }
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("namespace " + this._nameSpace);
        stringBuilder.AppendLine("{");
        stringBuilder.AppendLine("    internal partial class " + this._masterPageName + " : System.Web.UI.MasterPage");
        stringBuilder.AppendLine("    {");
        if (this._appType == ApplicationType.ASPNET45)
        {
          stringBuilder.AppendLine("        private const string AntiXsrfTokenKey = \"__AntiXsrfToken\";");
          stringBuilder.AppendLine("        private const string AntiXsrfUserNameKey = \"__AntiXsrfUserName\";");
          stringBuilder.AppendLine("        private string _antiXsrfTokenValue;");
          stringBuilder.AppendLine("");
          stringBuilder.AppendLine("        protected void Page_Init(object sender, EventArgs e)");
          stringBuilder.AppendLine("        {");
          stringBuilder.AppendLine("            // The code below helps to protect against XSRF attacks");
          stringBuilder.AppendLine("            var requestCookie = Request.Cookies[AntiXsrfTokenKey];");
          stringBuilder.AppendLine("            Guid requestCookieGuidValue;");
          stringBuilder.AppendLine("            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))");
          stringBuilder.AppendLine("            {");
          stringBuilder.AppendLine("                // Use the Anti-XSRF token from the cookie");
          stringBuilder.AppendLine("                _antiXsrfTokenValue = requestCookie.Value;");
          stringBuilder.AppendLine("                Page.ViewStateUserKey = _antiXsrfTokenValue;");
          stringBuilder.AppendLine("            }");
          stringBuilder.AppendLine("            else");
          stringBuilder.AppendLine("            {");
          stringBuilder.AppendLine("                // Generate a new Anti-XSRF token and save to the cookie");
          stringBuilder.AppendLine("                _antiXsrfTokenValue = Guid.NewGuid().ToString(\"N\");");
          stringBuilder.AppendLine("                Page.ViewStateUserKey = _antiXsrfTokenValue;");
          stringBuilder.AppendLine("");
          stringBuilder.AppendLine("                var responseCookie = new HttpCookie(AntiXsrfTokenKey)");
          stringBuilder.AppendLine("                {");
          stringBuilder.AppendLine("                    HttpOnly = true,");
          stringBuilder.AppendLine("                    Value = _antiXsrfTokenValue");
          stringBuilder.AppendLine("                };");
          stringBuilder.AppendLine("");
          stringBuilder.AppendLine("                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)");
          stringBuilder.AppendLine("                {");
          stringBuilder.AppendLine("                    responseCookie.Secure = true;");
          stringBuilder.AppendLine("                }");
          stringBuilder.AppendLine("");
          stringBuilder.AppendLine("                Response.Cookies.Set(responseCookie);");
          stringBuilder.AppendLine("            }");
          stringBuilder.AppendLine("");
          stringBuilder.AppendLine("            Page.PreLoad += master_Page_PreLoad;");
          stringBuilder.AppendLine("        }");
          stringBuilder.AppendLine("");
          stringBuilder.AppendLine("        void master_Page_PreLoad(object sender, EventArgs e)");
          stringBuilder.AppendLine("        {");
          stringBuilder.AppendLine("            if (!IsPostBack)");
          stringBuilder.AppendLine("            {");
          stringBuilder.AppendLine("                // Set Anti-XSRF token");
          stringBuilder.AppendLine("                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;");
          stringBuilder.AppendLine("                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;");
          stringBuilder.AppendLine("            }");
          stringBuilder.AppendLine("            else");
          stringBuilder.AppendLine("            {");
          stringBuilder.AppendLine("                // Validate the Anti-XSRF token");
          stringBuilder.AppendLine("                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))");
          stringBuilder.AppendLine("                {");
          stringBuilder.AppendLine("                    throw new InvalidOperationException(\"Validation of Anti-XSRF token failed.\");");
          stringBuilder.AppendLine("                }");
          stringBuilder.AppendLine("            }");
          stringBuilder.AppendLine("        }");
          stringBuilder.AppendLine("");
        }
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
        int appType1 = (int) this._appType;
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("Namespace " + this._nameSpace);
        stringBuilder.AppendLine("    internal Partial Class " + this._masterPageName);
        stringBuilder.AppendLine("        Inherits System.Web.UI.MasterPage");
        stringBuilder.AppendLine("");
        int appType2 = (int) this._appType;
        stringBuilder.AppendLine("        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load");
        stringBuilder.AppendLine("        End Sub");
        stringBuilder.AppendLine("    End Class");
        stringBuilder.AppendLine("End Namespace");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
