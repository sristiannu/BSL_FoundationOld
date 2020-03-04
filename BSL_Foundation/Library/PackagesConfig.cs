
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class PackagesConfig
  {
    private string _fullFileNamePath;
    private bool _isUseWebApi;

    internal PackagesConfig()
    {
    }

    internal PackagesConfig(string fullFileNamePath, bool isUseWebApi, ApplicationType appType = ApplicationType.ASPNETMVC5)
    {
      this._fullFileNamePath = fullFileNamePath;
      this._isUseWebApi = isUseWebApi;
      if (appType == ApplicationType.ASPNETMVC5)
        this.GenerateMvc5();
      else if (appType == ApplicationType.WINFORMS)
        this.GenerateWinForms();
      else
        this.Generate();
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        stringBuilder.AppendLine("<packages>");
        stringBuilder.AppendLine("  <package id=\"jQuery\" version=\"1.5.1\" />");
        stringBuilder.AppendLine("  <package id=\"jQuery.vsdoc\" version=\"1.5.1\" />");
        stringBuilder.AppendLine("  <package id=\"jQuery.Validation\" version=\"1.8.0\" />");
        stringBuilder.AppendLine("  <package id=\"jQuery.UI.Combined\" version=\"1.8.11\" />");
        stringBuilder.AppendLine("  <package id=\"EntityFramework\" version=\"4.1.10331.0\" />");
        stringBuilder.AppendLine("  <package id=\"Modernizr\" version=\"1.7\" />");
        stringBuilder.AppendLine("  <package id=\"DataAnnotationsExtensions\" version=\"0.6.0.0\" />");
        stringBuilder.AppendLine("  <package id=\"WebActivator\" version=\"1.1.0.0\" />");
        stringBuilder.AppendLine("  <package id=\"DataAnnotationsExtensions.MVC3\" version=\"0.6.0.0\" />");
        stringBuilder.AppendLine("</packages>");
        streamWriter.Write(stringBuilder.ToString());
      }
    }

    private void GenerateWinForms()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        stringBuilder.AppendLine("<packages>");
        stringBuilder.AppendLine("</packages>");
        streamWriter.Write(stringBuilder.ToString());
      }
    }

    private void GenerateMvc5()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        stringBuilder.AppendLine("<packages>");
        stringBuilder.AppendLine("  <package id=\"Antlr\" version=\"3.4.1.9004\" targetFramework=\"net451\" />");
        stringBuilder.AppendLine("  <package id=\"bootstrap\" version=\"3.0.0\" targetFramework=\"net451\" />");
        stringBuilder.AppendLine("  <package id=\"EntityFramework\" version=\"6.0.0\" targetFramework=\"net451\" />");
        stringBuilder.AppendLine("  <package id=\"jQuery\" version=\"1.10.2\" targetFramework=\"net451\" />");
        stringBuilder.AppendLine("  <package id=\"jQuery.Validation\" version=\"1.11.1\" targetFramework=\"net451\" />");
        stringBuilder.AppendLine("  <package id=\"Microsoft.AspNet.Identity.Core\" version=\"1.0.0\" targetFramework=\"net451\" />");
        stringBuilder.AppendLine("  <package id=\"Microsoft.AspNet.Identity.EntityFramework\" version=\"1.0.0\" targetFramework=\"net451\" />");
        stringBuilder.AppendLine("  <package id=\"Microsoft.AspNet.Identity.Owin\" version=\"1.0.0\" targetFramework=\"net451\" />");
        stringBuilder.AppendLine("  <package id=\"Microsoft.AspNet.Mvc\" version=\"5.0.0\" targetFramework=\"net451\" />");
        stringBuilder.AppendLine("  <package id=\"Microsoft.AspNet.Razor\" version=\"3.0.0\" targetFramework=\"net451\" />");
        stringBuilder.AppendLine("  <package id=\"Microsoft.AspNet.Web.Optimization\" version=\"1.1.1\" targetFramework=\"net451\" />");
        if (this._isUseWebApi)
        {
          stringBuilder.AppendLine("  <package id=\"Microsoft.AspNet.WebApi\" version=\"5.0.0\" targetFramework=\"net451\" />");
          stringBuilder.AppendLine("  <package id=\"Microsoft.AspNet.WebApi.Client\" version=\"5.0.0\" targetFramework=\"net451\" />");
          stringBuilder.AppendLine("  <package id=\"Microsoft.AspNet.WebApi.Core\" version=\"5.0.0\" targetFramework=\"net451\" />");
          stringBuilder.AppendLine("  <package id=\"Microsoft.AspNet.WebApi.WebHost\" version=\"5.0.0\" targetFramework=\"net451\" />");
        }
        stringBuilder.AppendLine("  <package id=\"Microsoft.AspNet.WebPages\" version=\"3.0.0\" targetFramework=\"net451\" />");
        stringBuilder.AppendLine("  <package id=\"Microsoft.jQuery.Unobtrusive.Validation\" version=\"3.0.0\" targetFramework=\"net451\" />");
        stringBuilder.AppendLine("  <package id=\"Microsoft.Owin\" version=\"2.0.0\" targetFramework=\"net451\" />");
        stringBuilder.AppendLine("  <package id=\"Microsoft.Owin.Host.SystemWeb\" version=\"2.0.0\" targetFramework=\"net451\" />");
        stringBuilder.AppendLine("  <package id=\"Microsoft.Owin.Security\" version=\"2.0.0\" targetFramework=\"net451\" />");
        stringBuilder.AppendLine("  <package id=\"Microsoft.Owin.Security.Cookies\" version=\"2.0.0\" targetFramework=\"net451\" />");
        stringBuilder.AppendLine("  <package id=\"Microsoft.Owin.Security.Facebook\" version=\"2.0.0\" targetFramework=\"net451\" />");
        stringBuilder.AppendLine("  <package id=\"Microsoft.Owin.Security.Google\" version=\"2.0.0\" targetFramework=\"net451\" />");
        stringBuilder.AppendLine("  <package id=\"Microsoft.Owin.Security.MicrosoftAccount\" version=\"2.0.0\" targetFramework=\"net451\" />");
        stringBuilder.AppendLine("  <package id=\"Microsoft.Owin.Security.OAuth\" version=\"2.0.0\" targetFramework=\"net451\" />");
        stringBuilder.AppendLine("  <package id=\"Microsoft.Owin.Security.Twitter\" version=\"2.0.0\" targetFramework=\"net451\" />");
        stringBuilder.AppendLine("  <package id=\"Microsoft.Web.Infrastructure\" version=\"1.0.0.0\" targetFramework=\"net451\" />");
        stringBuilder.AppendLine("  <package id=\"Modernizr\" version=\"2.6.2\" targetFramework=\"net451\" />");
        stringBuilder.AppendLine("  <package id=\"Newtonsoft.Json\" version=\"5.0.6\" targetFramework=\"net451\" />");
        stringBuilder.AppendLine("  <package id=\"Owin\" version=\"1.0\" targetFramework=\"net451\" />");
        stringBuilder.AppendLine("  <package id=\"Respond\" version=\"1.2.0\" targetFramework=\"net451\" />");
        stringBuilder.AppendLine("  <package id=\"WebGrease\" version=\"1.5.2\" targetFramework=\"net451\" />");
        stringBuilder.AppendLine("</packages>");
        streamWriter.Write(stringBuilder.ToString());
      }
    }

    private void GenerateMvc4()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        stringBuilder.AppendLine("<packages>");
        stringBuilder.AppendLine("  <package id=\"DotNetOpenAuth.AspNet\" version=\"4.1.4.12333\" targetFramework=\"net45\" />");
        stringBuilder.AppendLine("  <package id=\"DotNetOpenAuth.Core\" version=\"4.1.4.12333\" targetFramework=\"net45\" />");
        stringBuilder.AppendLine("  <package id=\"DotNetOpenAuth.OAuth.Consumer\" version=\"4.1.4.12333\" targetFramework=\"net45\" />");
        stringBuilder.AppendLine("  <package id=\"DotNetOpenAuth.OAuth.Core\" version=\"4.1.4.12333\" targetFramework=\"net45\" />");
        stringBuilder.AppendLine("  <package id=\"DotNetOpenAuth.OpenId.Core\" version=\"4.1.4.12333\" targetFramework=\"net45\" />");
        stringBuilder.AppendLine("  <package id=\"DotNetOpenAuth.OpenId.RelyingParty\" version=\"4.1.4.12333\" targetFramework=\"net45\" />");
        stringBuilder.AppendLine("  <package id=\"EntityFramework\" version=\"5.0.0\" targetFramework=\"net45\" />");
        stringBuilder.AppendLine("  <package id=\"jQuery\" version=\"1.8.2\" targetFramework=\"net45\" />");
        stringBuilder.AppendLine("  <package id=\"jQuery.UI.Combined\" version=\"1.8.24\" targetFramework=\"net45\" />");
        stringBuilder.AppendLine("  <package id=\"jQuery.Validation\" version=\"1.10.0\" targetFramework=\"net45\" />");
        stringBuilder.AppendLine("  <package id=\"knockoutjs\" version=\"2.2.0\" targetFramework=\"net45\" />");
        stringBuilder.AppendLine("  <package id=\"Microsoft.AspNet.Mvc\" version=\"4.0.20710.0\" targetFramework=\"net45\" />");
        stringBuilder.AppendLine("  <package id=\"Microsoft.AspNet.Mvc.FixedDisplayModes\" version=\"1.0.0\" targetFramework=\"net45\" />");
        stringBuilder.AppendLine("  <package id=\"Microsoft.AspNet.Razor\" version=\"2.0.20715.0\" targetFramework=\"net45\" />");
        stringBuilder.AppendLine("  <package id=\"Microsoft.AspNet.Web.Optimization\" version=\"1.0.0\" targetFramework=\"net45\" />");
        stringBuilder.AppendLine("  <package id=\"Microsoft.AspNet.WebApi\" version=\"4.0.20710.0\" targetFramework=\"net45\" />");
        stringBuilder.AppendLine("  <package id=\"Microsoft.AspNet.WebApi.Client\" version=\"4.0.20710.0\" targetFramework=\"net45\" />");
        stringBuilder.AppendLine("  <package id=\"Microsoft.AspNet.WebApi.Core\" version=\"4.0.20710.0\" targetFramework=\"net45\" />");
        stringBuilder.AppendLine("  <package id=\"Microsoft.AspNet.WebApi.OData\" version=\"4.0.0\" targetFramework=\"net45\" />");
        stringBuilder.AppendLine("  <package id=\"Microsoft.AspNet.WebApi.WebHost\" version=\"4.0.20710.0\" targetFramework=\"net45\" />");
        stringBuilder.AppendLine("  <package id=\"Microsoft.AspNet.WebPages\" version=\"2.0.20710.0\" targetFramework=\"net45\" />");
        stringBuilder.AppendLine("  <package id=\"Microsoft.AspNet.WebPages.Data\" version=\"2.0.20710.0\" targetFramework=\"net45\" />");
        stringBuilder.AppendLine("  <package id=\"Microsoft.AspNet.WebPages.OAuth\" version=\"2.0.20710.0\" targetFramework=\"net45\" />");
        stringBuilder.AppendLine("  <package id=\"Microsoft.AspNet.WebPages.WebData\" version=\"2.0.20710.0\" targetFramework=\"net45\" />");
        stringBuilder.AppendLine("  <package id=\"Microsoft.Data.Edm\" version=\"5.2.0\" targetFramework=\"net45\" />");
        stringBuilder.AppendLine("  <package id=\"Microsoft.Data.OData\" version=\"5.2.0\" targetFramework=\"net45\" />");
        stringBuilder.AppendLine("  <package id=\"Microsoft.jQuery.Unobtrusive.Ajax\" version=\"2.0.30116.0\" targetFramework=\"net45\" />");
        stringBuilder.AppendLine("  <package id=\"Microsoft.jQuery.Unobtrusive.Validation\" version=\"2.0.30116.0\" targetFramework=\"net45\" />");
        stringBuilder.AppendLine("  <package id=\"Microsoft.Net.Http\" version=\"2.0.20710.0\" targetFramework=\"net45\" />");
        stringBuilder.AppendLine("  <package id=\"Microsoft.Web.Infrastructure\" version=\"1.0.0.0\" targetFramework=\"net45\" />");
        stringBuilder.AppendLine("  <package id=\"Modernizr\" version=\"2.6.2\" targetFramework=\"net45\" />");
        stringBuilder.AppendLine("  <package id=\"Newtonsoft.Json\" version=\"4.5.11\" targetFramework=\"net45\" />");
        stringBuilder.AppendLine("  <package id=\"System.Spatial\" version=\"5.2.0\" targetFramework=\"net45\" />");
        stringBuilder.AppendLine("  <package id=\"WebGrease\" version=\"1.3.0\" targetFramework=\"net45\" />");
        stringBuilder.AppendLine("</packages>");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
