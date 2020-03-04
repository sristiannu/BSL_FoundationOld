
using System;
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class AssemblyInfoFile
  {
    private string _assemblyGuid = Guid.NewGuid().ToString().ToLower();
    private string _apiNameDirectory;
    private string _apiName;

    private AssemblyInfoFile()
    {
    }

    internal AssemblyInfoFile(string apiName, string apiNameDirectory, Language language)
    {
      this._apiNameDirectory = apiNameDirectory;
      this._apiName = apiName.Trim();
      if (language == Language.CSharp)
        this.GenerateCS();
      else
        this.GenerateVB();
    }

    private void GenerateCS()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._apiNameDirectory))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("using System.Reflection;");
        stringBuilder.AppendLine("using System.Runtime.CompilerServices;");
        stringBuilder.AppendLine("using System.Runtime.InteropServices;");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("// General Information about an assembly is controlled through the following");
        stringBuilder.AppendLine("// set of attributes. Change these attribute values to modify the information");
        stringBuilder.AppendLine("// associated with an assembly.");
        stringBuilder.AppendLine("[assembly: AssemblyTitle(\"" + this._apiName + "\")]");
        stringBuilder.AppendLine("[assembly: AssemblyDescription(\"\")]");
        stringBuilder.AppendLine("[assembly: AssemblyConfiguration(\"\")]");
        stringBuilder.AppendLine("[assembly: AssemblyCompany(\"BSL\")]");
        stringBuilder.AppendLine("[assembly: AssemblyProduct(\"" + this._apiName + "\")]");
        stringBuilder.AppendLine("[assembly: AssemblyCopyright(\"Copyright © BSL " + DateTime.Now.Year.ToString() + "\")]");
        stringBuilder.AppendLine("[assembly: AssemblyTrademark(\"\")]");
        stringBuilder.AppendLine("[assembly: AssemblyCulture(\"\")]");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("// Setting ComVisible to false makes the types in this assembly not visible ");
        stringBuilder.AppendLine("// to COM components.  If you need to access a type in this assembly from ");
        stringBuilder.AppendLine("// COM, set the ComVisible attribute to true on that type.");
        stringBuilder.AppendLine("[assembly: ComVisible(false)]");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("// The following GUID is for the ID of the typelib if this project is exposed to COM");
        stringBuilder.AppendLine("[assembly: Guid(\"" + this._assemblyGuid + "\")]");
        stringBuilder.AppendLine("// Version information for an assembly consists of the following four values:");
        stringBuilder.AppendLine("//");
        stringBuilder.AppendLine("//      Major Version");
        stringBuilder.AppendLine("//      Minor Version");
        stringBuilder.AppendLine("//      Build Number");
        stringBuilder.AppendLine("//      Revision");
        stringBuilder.AppendLine("//");
        stringBuilder.AppendLine("// You can specify all the values or you can default the Build and Revision Numbers");
        stringBuilder.AppendLine("// by using the '*' as shown below:");
        stringBuilder.AppendLine("// [assembly: AssemblyVersion(\"1.0.*\")]");
        stringBuilder.AppendLine("[assembly: AssemblyVersion(\"1.0.0.0\")]");
        stringBuilder.AppendLine("[assembly: AssemblyFileVersion(\"1.0.0.0\")]");
        streamWriter.Write(stringBuilder.ToString());
      }
    }

    private void GenerateVB()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._apiNameDirectory))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Imports System");
        stringBuilder.AppendLine("Imports System.Reflection");
        stringBuilder.AppendLine("Imports System.Runtime.InteropServices");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("' General Information about an assembly is controlled through the following ");
        stringBuilder.AppendLine("' set of attributes. Change these attribute values to modify the information");
        stringBuilder.AppendLine("' associated with an assembly.");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("' Review the values of the assembly attributes");
        stringBuilder.AppendLine("<Assembly: AssemblyTitle(\"" + this._apiName + "\")>");
        stringBuilder.AppendLine("<Assembly: AssemblyDescription(\"\")>");
        stringBuilder.AppendLine("<Assembly: AssemblyCompany(\"BSL\")>");
        stringBuilder.AppendLine("<Assembly: AssemblyProduct(\"" + this._apiName + "\")>");
        stringBuilder.AppendLine("<Assembly: AssemblyCopyright(\"Copyright © BSL " + DateTime.Now.Year.ToString() + "\")>");
        stringBuilder.AppendLine("<Assembly: AssemblyTrademark(\"\")>");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("<Assembly: ComVisible(False)>");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("' The following GUID is for the ID of the typelib if this project is exposed to COM");
        stringBuilder.AppendLine("<Assembly: Guid(\"" + this._assemblyGuid + "\")>");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("' Version information for an assembly consists of the following four values:");
        stringBuilder.AppendLine("'");
        stringBuilder.AppendLine("'      Major Version");
        stringBuilder.AppendLine("'      Minor Version ");
        stringBuilder.AppendLine("'      Build Number");
        stringBuilder.AppendLine("'      Revision");
        stringBuilder.AppendLine("'");
        stringBuilder.AppendLine("' You can specify all the values or you can default the Build and Revision Numbers ");
        stringBuilder.AppendLine("' by using the '*' as shown below:");
        stringBuilder.AppendLine("' <Assembly: AssemblyVersion(\"1.0.*\")> ");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("<Assembly: AssemblyVersion(\"1.0.0.0\")> ");
        stringBuilder.AppendLine("<Assembly: AssemblyFileVersion(\"1.0.0.0\")> ");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
