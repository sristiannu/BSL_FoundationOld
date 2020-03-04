
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class IdentityModels
  {
    private string _fullFileNamePath;
    private string _websiteName;

    internal IdentityModels()
    {
    }

    internal IdentityModels(string fullFileNamePath, string websiteName, Language language, ApplicationType appType = ApplicationType.ASPNET45)
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
        stringBuilder.AppendLine("using Microsoft.AspNet.Identity.EntityFramework;");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("namespace " + this._websiteName);
        stringBuilder.AppendLine("{");
        stringBuilder.AppendLine("    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.");
        stringBuilder.AppendLine("    internal class ApplicationUser : IdentityUser");
        stringBuilder.AppendLine("    {");
        stringBuilder.AppendLine("    }");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("    internal class ApplicationDbContext : IdentityDbContext<ApplicationUser>");
        stringBuilder.AppendLine("    {");
        stringBuilder.AppendLine("        internal ApplicationDbContext()");
        stringBuilder.AppendLine("            : base(\"DefaultConnection\")");
        stringBuilder.AppendLine("        {");
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
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
