
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class WebConfig
  {
    private string _fullFileNamePath;
    private ApplicationType _appType;

    internal WebConfig()
    {
    }

    internal WebConfig(string fullFileNamePath, ApplicationType appType = ApplicationType.ASPNET)
    {
      this._fullFileNamePath = fullFileNamePath;
      this._appType = appType;
      this.Generate();
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        if (this._appType == ApplicationType.ASPNET45)
        {
          stringBuilder.AppendLine("<?xml version=\"1.0\"?>");
          stringBuilder.AppendLine("<configuration>");
          stringBuilder.AppendLine("  <appSettings>");
          stringBuilder.AppendLine("    <add key=\"ArrowUp\" value=\"Images/ArrowUp.png\" />");
          stringBuilder.AppendLine("    <add key=\"ArrowDown\" value=\"Images/ArrowDown.png\" />");
          stringBuilder.AppendLine("  </appSettings>");
          stringBuilder.AppendLine("  <system.web>");
          stringBuilder.AppendLine("    <compilation debug=\"true\" targetFramework=\"4.5\"/>");
          stringBuilder.AppendLine("    <httpRuntime targetFramework=\"4.5\"/>");
          stringBuilder.AppendLine("      <pages theme=\"Theme1\">");
          stringBuilder.AppendLine("      <namespaces>");
          stringBuilder.AppendLine("        <add namespace=\"System.Web.Optimization\"/>");
          stringBuilder.AppendLine("      </namespaces>");
          stringBuilder.AppendLine("      <controls>");
          stringBuilder.AppendLine("        <add assembly=\"Microsoft.AspNet.Web.Optimization.WebForms\" namespace=\"Microsoft.AspNet.Web.Optimization.WebForms\" tagPrefix=\"webopt\"/>");
          stringBuilder.AppendLine("      </controls>");
          stringBuilder.AppendLine("    </pages>");
          stringBuilder.AppendLine("  </system.web>");
          stringBuilder.AppendLine("</configuration>");
        }
        else
        {
          stringBuilder.AppendLine("<?xml version=\"1.0\"?>");
          stringBuilder.AppendLine("<configuration>");
          stringBuilder.AppendLine("    <appSettings>");
          stringBuilder.AppendLine("        <add key=\"ArrowUp\" value=\"Images/ArrowUp.png\" />");
          stringBuilder.AppendLine("        <add key=\"ArrowDown\" value=\"Images/ArrowDown.png\" />");
          stringBuilder.AppendLine("    </appSettings>");
          stringBuilder.AppendLine("    <system.web>");
          stringBuilder.AppendLine("        <compilation debug=\"true\" targetFramework=\"4.0\">");
          stringBuilder.AppendLine("            <assemblies>");
          stringBuilder.AppendLine("                <add assembly=\"System.Data.Entity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=B77A5C561934E089\" />");
          stringBuilder.AppendLine("                <add assembly=\"System.Data.Services, Version=4.0.0.0, Culture=neutral, PublicKeyToken=B77A5C561934E089\" />");
          stringBuilder.AppendLine("                <add assembly=\"System.Data.Services.Client, Version=4.0.0.0, Culture=neutral, PublicKeyToken=B77A5C561934E089\" />");
          stringBuilder.AppendLine("            </assemblies>");
          stringBuilder.AppendLine("        </compilation>");
          stringBuilder.AppendLine("        <pages theme=\"Theme1\" />");
          stringBuilder.AppendLine("    </system.web>");
          stringBuilder.AppendLine("</configuration>");
        }
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
