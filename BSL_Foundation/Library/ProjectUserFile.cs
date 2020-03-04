
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class ProjectUserFile
  {
    private string _fullFileNamePath;
    private ProjectFileType _projectFileType;

    private ProjectUserFile()
    {
    }

    internal ProjectUserFile(string fullFileNamePath, ProjectFileType projectFileType)
    {
      this._fullFileNamePath = fullFileNamePath;
      this._projectFileType = projectFileType;
      this.Generate();
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        stringBuilder.AppendLine("<Project ToolsVersion=\"14.0\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\">");
        if (this._projectFileType == ProjectFileType.WebApp)
        {
          stringBuilder.AppendLine("  <PropertyGroup>");
          stringBuilder.AppendLine("    <ActiveDebugProfile>IIS Express</ActiveDebugProfile>");
          stringBuilder.AppendLine("  </PropertyGroup>");
        }
        else if (this._projectFileType == ProjectFileType.BusinessAndDataAPI)
        {
          stringBuilder.AppendLine("  <PropertyGroup>");
          stringBuilder.AppendLine("    <ActiveDebugProfile>Start</ActiveDebugProfile>");
          stringBuilder.AppendLine("  </PropertyGroup>");
        }
        else if (this._projectFileType == ProjectFileType.WebAPI)
        {
          stringBuilder.AppendLine("  <PropertyGroup>");
          stringBuilder.AppendLine("    <ActiveDebugProfile>IIS Express</ActiveDebugProfile>");
          stringBuilder.AppendLine("  </PropertyGroup>");
        }
        stringBuilder.AppendLine("</Project>");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
