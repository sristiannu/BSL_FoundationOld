
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class GlobalAsax
  {
    private string _path;
    private Language _language;
    private string _languageAbbreviation;
    private string _nameSpace;
    private ApplicationType _appType;
    private bool _isUseFriendlyUrls;

    private GlobalAsax()
    {
    }

    internal GlobalAsax(string path, Language language, string nameSpace, string languageAbbreviation, ApplicationType appType = ApplicationType.ASPNET45, bool isUseFriendlyUrls = false)
    {
      this._path = path;
      this._language = language;
      this._languageAbbreviation = languageAbbreviation;
      this._nameSpace = nameSpace;
      this._appType = appType;
      this._isUseFriendlyUrls = isUseFriendlyUrls;
      this.Generate();
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._path))
      {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("<%@ Application Language=\"" + this._languageAbbreviation + "\" %>");
        sb.AppendLine("<%@ Import Namespace=\"" + this._nameSpace + "\" %>");
        sb.AppendLine("<%@ Import Namespace=\"System.Web.Optimization\" %>");
        if (this._isUseFriendlyUrls)
          sb.AppendLine("<%@ Import Namespace=\"System.Web.Routing\" %>");
        sb.AppendLine("");
        sb.AppendLine("<script runat=\"server\">");
        sb.AppendLine("");
        if (this._language == Language.CSharp)
          this.BuildCSBody(sb);
        else
          this.BuildVBBody(sb);
        sb.AppendLine("");
        sb.AppendLine("</script>");
        streamWriter.Write(sb.ToString());
      }
    }

    private void BuildCSBody(StringBuilder sb)
    {
      sb.AppendLine("    void Application_Start(object sender, EventArgs e)");
      sb.AppendLine("    {");
      sb.AppendLine("        // Code that runs on application startup");
      sb.AppendLine("        BundleConfig.RegisterBundles(BundleTable.Bundles);");
      if (this._isUseFriendlyUrls)
      {
        sb.AppendLine("");
        sb.AppendLine("        // used for friendlyurls");
        sb.AppendLine("        RouteConfig.RegisterRoutes(RouteTable.Routes);");
      }
      sb.AppendLine("    }");
      sb.AppendLine("");
      sb.AppendLine("    void Application_End(object sender, EventArgs e)");
      sb.AppendLine("    {");
      sb.AppendLine("        //  Code that runs on application shutdown");
      sb.AppendLine("    }");
      sb.AppendLine("");
      sb.AppendLine("    void Application_Error(object sender, EventArgs e)");
      sb.AppendLine("    {");
      sb.AppendLine("        // Code that runs when an unhandled error occurs");
      sb.AppendLine("    }");
    }

    private void BuildVBBody(StringBuilder sb)
    {
      sb.AppendLine("    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)");
      sb.AppendLine("        ' Code that runs on application startup");
      sb.AppendLine("        BundleConfig.RegisterBundles(BundleTable.Bundles)");
      if (this._isUseFriendlyUrls)
      {
        sb.AppendLine("");
        sb.AppendLine("        'used for friendlyurls");
        sb.AppendLine("        RouteConfig.RegisterRoutes(RouteTable.Routes)");
      }
      sb.AppendLine("    End Sub");
      sb.AppendLine("    ");
      sb.AppendLine("    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)");
      sb.AppendLine("        ' Code that runs on application shutdown");
      sb.AppendLine("    End Sub");
      sb.AppendLine("    ");
      sb.AppendLine("    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)");
      sb.AppendLine("        ' Code that runs when an unhandled error occurs");
      sb.AppendLine("    End Sub");
    }
  }
}
