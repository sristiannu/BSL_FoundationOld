
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class AuthConfigFile
  {
    private string _fullFileNamePath;
    private Language _language;
    private string _fileExtension;
    private string _websiteName;

    private AuthConfigFile()
    {
    }

    internal AuthConfigFile(string fullFileNamePath, Language language, string fileExtension, string websiteName)
    {
      this._fullFileNamePath = fullFileNamePath;
      this._language = language;
      this._fileExtension = fileExtension;
      this._websiteName = websiteName;
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
        stringBuilder.AppendLine("using System;");
        stringBuilder.AppendLine("using System.Collections.Generic;");
        stringBuilder.AppendLine("using System.Linq;");
        stringBuilder.AppendLine("using System.Text;");
        stringBuilder.AppendLine("using Microsoft.Web.WebPages.OAuth;");
        stringBuilder.AppendLine("using " + this._websiteName + ".Models;");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("namespace " + this._websiteName);
        stringBuilder.AppendLine("{");
        stringBuilder.AppendLine("    internal static class AuthConfig");
        stringBuilder.AppendLine("    {");
        stringBuilder.AppendLine("        internal static void RegisterAuth()");
        stringBuilder.AppendLine("        {");
        stringBuilder.AppendLine("            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,");
        stringBuilder.AppendLine("            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            //OAuthWebSecurity.RegisterMicrosoftClient(");
        stringBuilder.AppendLine("            //    clientId: \"\",");
        stringBuilder.AppendLine("            //    clientSecret: \"\");");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            //OAuthWebSecurity.RegisterTwitterClient(");
        stringBuilder.AppendLine("            //    consumerKey: \"\",");
        stringBuilder.AppendLine("            //    consumerSecret: \"\");");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            //OAuthWebSecurity.RegisterFacebookClient(");
        stringBuilder.AppendLine("            //    appId: \"\",");
        stringBuilder.AppendLine("            //    appSecret: \"\");");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            //OAuthWebSecurity.RegisterGoogleClient();");
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
        stringBuilder.AppendLine("Imports Microsoft.Web.WebPages.OAuth");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("internal Class AuthConfig");
        stringBuilder.AppendLine("    internal Shared Sub RegisterAuth()");
        stringBuilder.AppendLine("        ' To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,");
        stringBuilder.AppendLine("        ' you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        ' OAuthWebSecurity.RegisterMicrosoftClient(");
        stringBuilder.AppendLine("        '     clientId:=\"\",");
        stringBuilder.AppendLine("        '     clientSecret:=\"\")");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        ' OAuthWebSecurity.RegisterTwitterClient(");
        stringBuilder.AppendLine("        '     consumerKey:=\"\",");
        stringBuilder.AppendLine("        '     consumerSecret:=\"\")");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        ' OAuthWebSecurity.RegisterFacebookClient(");
        stringBuilder.AppendLine("        '     appId:=\"\",");
        stringBuilder.AppendLine("        '     appSecret:=\"\")");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        ' OAuthWebSecurity.RegisterGoogleClient()");
        stringBuilder.AppendLine("    End Sub");
        stringBuilder.AppendLine("End Class");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
