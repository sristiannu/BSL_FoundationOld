
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class StartUpAuth
  {
    private string _fullFileNamePath;
    private string _websiteName;

    internal StartUpAuth()
    {
    }

    internal StartUpAuth(string fullFileNamePath, string websiteName, Language language, ApplicationType appType = ApplicationType.ASPNET45)
    {
      this._fullFileNamePath = fullFileNamePath;
      this._websiteName = websiteName;
      if (language == Language.CSharp)
        this.GenerateCodeCS();
      else
        this.GenerateCodeVB();
    }

    private void GenerateCodeCS()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("using Microsoft.AspNet.Identity;");
        stringBuilder.AppendLine("using Microsoft.Owin;");
        stringBuilder.AppendLine("using Microsoft.Owin.Security.Cookies;");
        stringBuilder.AppendLine("using Owin;");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("namespace " + this._websiteName);
        stringBuilder.AppendLine("{");
        stringBuilder.AppendLine("    internal partial class Startup");
        stringBuilder.AppendLine("    {");
        stringBuilder.AppendLine("        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864");
        stringBuilder.AppendLine("        internal void ConfigureAuth(IAppBuilder app)");
        stringBuilder.AppendLine("        {");
        stringBuilder.AppendLine("            // Enable the application to use a cookie to store information for the signed in user");
        stringBuilder.AppendLine("            //app.UseCookieAuthentication(new CookieAuthenticationOptions");
        stringBuilder.AppendLine("            //{");
        stringBuilder.AppendLine("            //    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,");
        stringBuilder.AppendLine("            //    LoginPath = new PathString(\"/Account/Login\")");
        stringBuilder.AppendLine("            //});");
        stringBuilder.AppendLine("            // Use a cookie to temporarily store information about a user logging in with a third party login provider");
        stringBuilder.AppendLine("            //app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            // Uncomment the following lines to enable logging in with third party login providers");
        stringBuilder.AppendLine("            //app.UseMicrosoftAccountAuthentication(");
        stringBuilder.AppendLine("            //    clientId: \"\",");
        stringBuilder.AppendLine("            //    clientSecret: \"\");");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            //app.UseTwitterAuthentication(");
        stringBuilder.AppendLine("            //   consumerKey: \"\",");
        stringBuilder.AppendLine("            //   consumerSecret: \"\");");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            //app.UseFacebookAuthentication(");
        stringBuilder.AppendLine("            //   appId: \"\",");
        stringBuilder.AppendLine("            //   appSecret: \"\");");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            //app.UseGoogleAuthentication();");
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
        stringBuilder.AppendLine("Imports Microsoft.AspNet.Identity");
        stringBuilder.AppendLine("Imports Microsoft.Owin");
        stringBuilder.AppendLine("Imports Microsoft.Owin.Security.Cookies");
        stringBuilder.AppendLine("Imports Owin");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("internal Class Startup");
        stringBuilder.AppendLine("    internal Sub ConfigureAuth(app As IAppBuilder)");
        stringBuilder.AppendLine("        ' Enable the application to use a cookie to store information for the signed in user");
        stringBuilder.AppendLine("        'app.UseCookieAuthentication(New CookieAuthenticationOptions() With {");
        stringBuilder.AppendLine("        '.AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,");
        stringBuilder.AppendLine("        '.LoginPath = New PathString(\"/Account/Login\")})");
        stringBuilder.AppendLine("        ' Use a cookie to temporarily store information about a user logging in with a third party login provider");
        stringBuilder.AppendLine("        'app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie)");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        ' Uncomment the following lines to enable logging in with third party login providers");
        stringBuilder.AppendLine("        'app.UseMicrosoftAccountAuthentication(");
        stringBuilder.AppendLine("        '    clientId:=\"\",");
        stringBuilder.AppendLine("        '    clientSecret:=\"\")");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        'app.UseTwitterAuthentication(");
        stringBuilder.AppendLine("        '   consumerKey:=\"\",");
        stringBuilder.AppendLine("        '   consumerSecret:=\"\")");
        stringBuilder.AppendLine(" ");
        stringBuilder.AppendLine("        'app.UseFacebookAuthentication(");
        stringBuilder.AppendLine("        '   appId:=\"\",");
        stringBuilder.AppendLine("        '   appSecret:=\"\")");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        'app.UseGoogleAuthentication()");
        stringBuilder.AppendLine("    End Sub");
        stringBuilder.AppendLine("End Class");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
