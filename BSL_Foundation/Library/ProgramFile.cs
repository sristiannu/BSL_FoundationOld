
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class ProgramFile
  {
    private string _fullFileNamePath;
    private string _appName;

    private ProgramFile()
    {
    }

    internal ProgramFile(string fullFileNamePath, string appName)
    {
      this._appName = appName;
      this._fullFileNamePath = fullFileNamePath + "\\Program.cs";
      this.Generate();
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("using Microsoft.AspNetCore;");
        stringBuilder.AppendLine("using Microsoft.AspNetCore.Hosting;");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("namespace " + this._appName);
        stringBuilder.AppendLine("{");
        stringBuilder.AppendLine("    public class Program");
        stringBuilder.AppendLine("    {");
        stringBuilder.AppendLine("        public static void Main(string[] args)");
        stringBuilder.AppendLine("        {");
        stringBuilder.AppendLine("            BuildWebHost(args).Run();");
        stringBuilder.AppendLine("        }");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        public static IWebHost BuildWebHost(string[] args) =>");
        stringBuilder.AppendLine("            WebHost.CreateDefaultBuilder(args)");
        stringBuilder.AppendLine("                .UseStartup<Startup>()");
        stringBuilder.AppendLine("                .Build();");
        stringBuilder.AppendLine("    }");
        stringBuilder.AppendLine("}");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
