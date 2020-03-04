
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class MvcWebConfigInViews
  {
    private string _path;
    private string _webSiteName;

    private MvcWebConfigInViews()
    {
    }

    internal MvcWebConfigInViews(string path, string webSiteName)
    {
      this._path = path;
      this._webSiteName = webSiteName;
      this.Generate();
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._path + MyConstants.WordWebDotConfig))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("<?xml version=\"1.0\"?>");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("<configuration>");
        stringBuilder.AppendLine("  <configSections>");
        stringBuilder.AppendLine("    <sectionGroup name=\"system.web.webPages.razor\" type=\"System.Web.WebPages.Razor.Configuration.RazorWebSectionGroup, System.Web.WebPages.Razor, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\">");
        stringBuilder.AppendLine("      <section name=\"host\" type=\"System.Web.WebPages.Razor.Configuration.HostSection, System.Web.WebPages.Razor, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\" requirePermission=\"false\" />");
        stringBuilder.AppendLine("      <section name=\"pages\" type=\"System.Web.WebPages.Razor.Configuration.RazorPagesSection, System.Web.WebPages.Razor, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\" requirePermission=\"false\" />");
        stringBuilder.AppendLine("    </sectionGroup>");
        stringBuilder.AppendLine("  </configSections>");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("  <system.web.webPages.razor>");
        stringBuilder.AppendLine("    <host factoryType=\"System.Web.Mvc.MvcWebRazorHostFactory, System.Web.Mvc, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\" />");
        stringBuilder.AppendLine("    <pages pageBaseType=\"System.Web.Mvc.WebViewPage\">");
        stringBuilder.AppendLine("      <namespaces>");
        stringBuilder.AppendLine("        <add namespace=\"System.Web.Mvc\" />");
        stringBuilder.AppendLine("        <add namespace=\"System.Web.Mvc.Ajax\" />");
        stringBuilder.AppendLine("        <add namespace=\"System.Web.Mvc.Html\" />");
        stringBuilder.AppendLine("        <add namespace=\"System.Web.Optimization\"/>");
        stringBuilder.AppendLine("        <add namespace=\"System.Web.Routing\" />");
        stringBuilder.AppendLine("        <add namespace=\"" + this._webSiteName + "\" />");
        stringBuilder.AppendLine("      </namespaces>");
        stringBuilder.AppendLine("    </pages>");
        stringBuilder.AppendLine("  </system.web.webPages.razor>");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("  <appSettings>");
        stringBuilder.AppendLine("    <add key=\"webpages:Enabled\" value=\"false\" />");
        stringBuilder.AppendLine("  </appSettings>");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("  <system.webServer>");
        stringBuilder.AppendLine("    <handlers>");
        stringBuilder.AppendLine("      <remove name=\"BlockViewHandler\"/>");
        stringBuilder.AppendLine("      <add name=\"BlockViewHandler\" path=\"*\" verb=\"*\" preCondition=\"integratedMode\" type=\"System.Web.HttpNotFoundHandler\" />");
        stringBuilder.AppendLine("    </handlers>");
        stringBuilder.AppendLine("  </system.webServer>");
        stringBuilder.AppendLine("</configuration>");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
