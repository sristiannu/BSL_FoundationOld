
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class RegisterClientValidationExtensionsFile
  {
    private string _fullFileNamePath;
    private string _websiteName;

    private RegisterClientValidationExtensionsFile()
    {
    }

    internal RegisterClientValidationExtensionsFile(string fullFileNamePath, Language language, string websiteName)
    {
      this._fullFileNamePath = fullFileNamePath;
      this._websiteName = websiteName;
      this.Generate();
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("using DataAnnotationsExtensions.ClientValidation;");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("[assembly: WebActivator.PreApplicationStartMethod(typeof(" + this._websiteName + ".App_Start.RegisterClientValidationExtensions), \"Start\")]");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("namespace " + this._websiteName + ".App_Start {");
        stringBuilder.AppendLine("    internal static class RegisterClientValidationExtensions {");
        stringBuilder.AppendLine("        internal static void Start() {");
        stringBuilder.AppendLine("            DataAnnotationsModelValidatorProviderExtensions.RegisterValidationExtensions();");
        stringBuilder.AppendLine("        }");
        stringBuilder.AppendLine("    }");
        stringBuilder.AppendLine("}");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
