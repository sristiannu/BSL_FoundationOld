
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class MvcWebConfig
  {
    private string _path;
    private Language _language;
    private bool _isUseWebApi;
    private string _webApiBaseAddress;
    private string _server;
    private string _database;
    private string _username;
    private string _password;

    private MvcWebConfig()
    {
    }

    internal MvcWebConfig(string path, Language language, string server, string database, string username, string password, ApplicationType appType = ApplicationType.ASPNETMVC, bool isUseWebApi = false, string webApiBaseAddress = "")
    {
      this._path = path;
      this._language = language;
      this._isUseWebApi = isUseWebApi;
      this._webApiBaseAddress = webApiBaseAddress;
      this._server = server;
      this._database = database;
      this._username = username;
      this._password = password;
      if (appType == ApplicationType.ASPNETMVC5)
      {
        this.GenerateMVC5();
        this.GenerateDebugMVC5();
        this.GenerateReleaseMVC5();
      }
      else
      {
        this.Generate();
        this.GenerateDebug();
        this.GenerateRelease();
      }
    }

    private void GenerateMVC5()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._path + MyConstants.WordWebDotConfig))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("<?xml version=\"1.0\"?>");
        stringBuilder.AppendLine("<!--");
        stringBuilder.AppendLine("  For more information on how to configure your ASP.NET application, please visit");
        stringBuilder.AppendLine("  http://go.microsoft.com/fwlink/?LinkId=301880");
        stringBuilder.AppendLine("  -->");
        stringBuilder.AppendLine("<configuration>");
        stringBuilder.AppendLine("  <configSections>");
        stringBuilder.AppendLine("    <!-- For more information on Entity Framework configuration, visit http://go.microsoft.com/fwlink/?LinkID=237468 -->");
        stringBuilder.AppendLine("    <section name=\"entityFramework\" type=\"System.Data.Entity.Internal.ConfigFile.EntityFrameworkSection, EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\" requirePermission=\"false\"/>");
        stringBuilder.AppendLine("  </configSections>");
        stringBuilder.AppendLine("  <connectionStrings>");
        stringBuilder.AppendLine("  </connectionStrings>");
        stringBuilder.AppendLine("  <appSettings>");
        stringBuilder.AppendLine("    <add key=\"webpages:Version\" value=\"3.0.0.0\"/>");
        stringBuilder.AppendLine("    <add key=\"webpages:Enabled\" value=\"false\"/>");
        stringBuilder.AppendLine("    <add key=\"ClientValidationEnabled\" value=\"true\"/>");
        stringBuilder.AppendLine("    <add key=\"UnobtrusiveJavaScriptEnabled\" value=\"true\"/>");
        stringBuilder.AppendLine("    <add key=\"ConnectionString\" value=\"Data Source=" + this._server + ";Initial Catalog=" + this._database + ";User ID=" + this._username + ";Password=" + this._password + "\"/>");
        if (this._isUseWebApi)
          stringBuilder.AppendLine("    <add key=\"WebApiBaseAddress\" value=\"" + this._webApiBaseAddress + "\" />");
        stringBuilder.AppendLine("    <add key=\"GridNumberOfRows\" value=\"10\" />");
        stringBuilder.AppendLine("    <add key=\"GridNumberOfPagesToShow\" value=\"7\" />");
        stringBuilder.AppendLine("  </appSettings>");
        stringBuilder.AppendLine("  <!--");
        stringBuilder.AppendLine("    For a description of web.config changes see http://go.microsoft.com/fwlink/?LinkId=235367.");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("    The following attributes can be set on the <httpRuntime> tag.");
        stringBuilder.AppendLine("      <system.Web>");
        stringBuilder.AppendLine("        <httpRuntime targetFramework=\"4.5.1\" />");
        stringBuilder.AppendLine("      </system.Web>");
        stringBuilder.AppendLine("  -->");
        stringBuilder.AppendLine("  <system.web>");
        stringBuilder.AppendLine("    <authentication mode=\"None\"/>");
        stringBuilder.AppendLine("    <compilation debug=\"true\" targetFramework=\"4.5.1\"/>");
        stringBuilder.AppendLine("    <httpRuntime targetFramework=\"4.5.1\"/>");
        stringBuilder.AppendLine("  </system.web>");
        stringBuilder.AppendLine("  <system.webServer>");
        stringBuilder.AppendLine("    <modules>");
        stringBuilder.AppendLine("      <remove name=\"FormsAuthenticationModule\"/>");
        stringBuilder.AppendLine("    </modules>");
        stringBuilder.AppendLine("    <handlers>");
        stringBuilder.AppendLine("      <remove name=\"ExtensionlessUrlHandler-Integrated-4.0\"/>");
        stringBuilder.AppendLine("      <remove name=\"OPTIONSVerbHandler\"/>");
        stringBuilder.AppendLine("      <remove name=\"TRACEVerbHandler\"/>");
        stringBuilder.AppendLine("      <add name=\"ExtensionlessUrlHandler-Integrated-4.0\" path=\"*.\" verb=\"*\" type=\"System.Web.Handlers.TransferRequestHandler\" preCondition=\"integratedMode,runtimeVersionv4.0\"/>");
        stringBuilder.AppendLine("    </handlers>");
        stringBuilder.AppendLine("  </system.webServer>");
        stringBuilder.AppendLine("  <runtime>");
        stringBuilder.AppendLine("    <assemblyBinding xmlns=\"urn:schemas-microsoft-com:asm.v1\">");
        stringBuilder.AppendLine("      <dependentAssembly>");
        stringBuilder.AppendLine("        <assemblyIdentity name=\"System.Web.Helpers\" publicKeyToken=\"31bf3856ad364e35\"/>");
        stringBuilder.AppendLine("        <bindingRedirect oldVersion=\"1.0.0.0-3.0.0.0\" newVersion=\"3.0.0.0\"/>");
        stringBuilder.AppendLine("      </dependentAssembly>");
        stringBuilder.AppendLine("      <dependentAssembly>");
        stringBuilder.AppendLine("        <assemblyIdentity name=\"System.Web.Mvc\" publicKeyToken=\"31bf3856ad364e35\"/>");
        stringBuilder.AppendLine("        <bindingRedirect oldVersion=\"1.0.0.0-5.0.0.0\" newVersion=\"5.0.0.0\"/>");
        stringBuilder.AppendLine("      </dependentAssembly>");
        stringBuilder.AppendLine("      <dependentAssembly>");
        stringBuilder.AppendLine("        <assemblyIdentity name=\"System.Web.Optimization\" publicKeyToken=\"31bf3856ad364e35\"/>");
        stringBuilder.AppendLine("        <bindingRedirect oldVersion=\"1.0.0.0-1.1.0.0\" newVersion=\"1.1.0.0\"/>");
        stringBuilder.AppendLine("      </dependentAssembly>");
        stringBuilder.AppendLine("      <dependentAssembly>");
        stringBuilder.AppendLine("        <assemblyIdentity name=\"System.Web.WebPages\" publicKeyToken=\"31bf3856ad364e35\"/>");
        stringBuilder.AppendLine("        <bindingRedirect oldVersion=\"1.0.0.0-3.0.0.0\" newVersion=\"3.0.0.0\"/>");
        stringBuilder.AppendLine("      </dependentAssembly>");
        stringBuilder.AppendLine("      <dependentAssembly>");
        stringBuilder.AppendLine("        <assemblyIdentity name=\"WebGrease\" publicKeyToken=\"31bf3856ad364e35\"/>");
        stringBuilder.AppendLine("        <bindingRedirect oldVersion=\"1.0.0.0-1.5.2.14234\" newVersion=\"1.5.2.14234\"/>");
        stringBuilder.AppendLine("      </dependentAssembly>");
        stringBuilder.AppendLine("    </assemblyBinding>");
        stringBuilder.AppendLine("  </runtime>");
        stringBuilder.AppendLine("  <entityFramework>");
        stringBuilder.AppendLine("    <defaultConnectionFactory type=\"System.Data.Entity.Infrastructure.SqlConnectionFactory, EntityFramework\"/>");
        stringBuilder.AppendLine("    <providers>");
        stringBuilder.AppendLine("      <provider invariantName=\"System.Data.SqlClient\" type=\"System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer\"/>");
        stringBuilder.AppendLine("    </providers>");
        stringBuilder.AppendLine("  </entityFramework>");
        stringBuilder.AppendLine("</configuration>");
        streamWriter.Write(stringBuilder.ToString());
      }
    }

    private void GenerateMVC4()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._path + MyConstants.WordWebDotConfig))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        stringBuilder.AppendLine("<!--");
        stringBuilder.AppendLine("  For more information on how to configure your ASP.NET application, please visit");
        stringBuilder.AppendLine("  http://go.microsoft.com/fwlink/?LinkId=169433");
        stringBuilder.AppendLine("  -->");
        stringBuilder.AppendLine("<configuration>");
        stringBuilder.AppendLine("  <configSections>");
        stringBuilder.AppendLine("    <!-- For more information on Entity Framework configuration, visit http://go.microsoft.com/fwlink/?LinkID=237468 -->");
        stringBuilder.AppendLine("    <section name=\"entityFramework\" type=\"System.Data.Entity.Internal.ConfigFile.EntityFrameworkSection, EntityFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\" requirePermission=\"false\" />");
        stringBuilder.AppendLine("  </configSections>");
        stringBuilder.AppendLine("  <connectionStrings />");
        stringBuilder.AppendLine("  <appSettings>");
        stringBuilder.AppendLine("    <add key=\"webpages:Version\" value=\"2.0.0.0\" />");
        stringBuilder.AppendLine("    <add key=\"webpages:Enabled\" value=\"false\" />");
        stringBuilder.AppendLine("    <add key=\"PreserveLoginUrl\" value=\"true\" />");
        stringBuilder.AppendLine("    <add key=\"ClientValidationEnabled\" value=\"true\" />");
        stringBuilder.AppendLine("    <add key=\"UnobtrusiveJavaScriptEnabled\" value=\"true\" />");
        if (this._isUseWebApi)
          stringBuilder.AppendLine("    <add key=\"WebApiBaseAddress\" value=\"" + this._webApiBaseAddress + "\" />");
        stringBuilder.AppendLine("  </appSettings>");
        stringBuilder.AppendLine("  <system.web>");
        stringBuilder.AppendLine("    <compilation debug=\"true\" targetFramework=\"4.5\" />");
        stringBuilder.AppendLine("    <httpRuntime targetFramework=\"4.5\" />");
        stringBuilder.AppendLine("    <pages>");
        stringBuilder.AppendLine("      <namespaces>");
        stringBuilder.AppendLine("        <add namespace=\"System.Web.Helpers\" />");
        stringBuilder.AppendLine("        <add namespace=\"System.Web.Mvc\" />");
        stringBuilder.AppendLine("        <add namespace=\"System.Web.Mvc.Ajax\" />");
        stringBuilder.AppendLine("        <add namespace=\"System.Web.Mvc.Html\" />");
        stringBuilder.AppendLine("        <add namespace=\"System.Web.Optimization\" />");
        stringBuilder.AppendLine("        <add namespace=\"System.Web.Routing\" />");
        stringBuilder.AppendLine("        <add namespace=\"System.Web.WebPages\" />");
        stringBuilder.AppendLine("      </namespaces>");
        stringBuilder.AppendLine("    </pages>");
        stringBuilder.AppendLine("  </system.web>");
        stringBuilder.AppendLine("  <system.webServer>");
        stringBuilder.AppendLine("    <validation validateIntegratedModeConfiguration=\"false\" />");
        stringBuilder.AppendLine("  <handlers><remove name=\"ExtensionlessUrlHandler-ISAPI-4.0_32bit\" /><remove name=\"ExtensionlessUrlHandler-ISAPI-4.0_64bit\" /><remove name=\"ExtensionlessUrlHandler-Integrated-4.0\" /><add name=\"ExtensionlessUrlHandler-ISAPI-4.0_32bit\" path=\"*.\" verb=\"GET,HEAD,POST,DEBUG,PUT,DELETE,PATCH,OPTIONS\" modules=\"IsapiModule\" scriptProcessor=\"%windir%\\Microsoft.NET\\Framework\\v4.0.30319\\aspnet_isapi.dll\" preCondition=\"classicMode,runtimeVersionv4.0,bitness32\" responseBufferLimit=\"0\" /><add name=\"ExtensionlessUrlHandler-ISAPI-4.0_64bit\" path=\"*.\" verb=\"GET,HEAD,POST,DEBUG,PUT,DELETE,PATCH,OPTIONS\" modules=\"IsapiModule\" scriptProcessor=\"%windir%\\Microsoft.NET\\Framework64\\v4.0.30319\\aspnet_isapi.dll\" preCondition=\"classicMode,runtimeVersionv4.0,bitness64\" responseBufferLimit=\"0\" /><add name=\"ExtensionlessUrlHandler-Integrated-4.0\" path=\"*.\" verb=\"GET,HEAD,POST,DEBUG,PUT,DELETE,PATCH,OPTIONS\" type=\"System.Web.Handlers.TransferRequestHandler\" preCondition=\"integratedMode,runtimeVersionv4.0\" /></handlers></system.webServer>");
        stringBuilder.AppendLine("  <runtime>");
        stringBuilder.AppendLine("    <assemblyBinding xmlns=\"urn:schemas-microsoft-com:asm.v1\">");
        stringBuilder.AppendLine("      <dependentAssembly>");
        stringBuilder.AppendLine("        <assemblyIdentity name=\"DotNetOpenAuth.Core\" publicKeyToken=\"2780ccd10d57b246\" />");
        stringBuilder.AppendLine("        <bindingRedirect oldVersion=\"0.0.0.0-4.1.0.0\" newVersion=\"4.1.0.0\" />");
        stringBuilder.AppendLine("      </dependentAssembly>");
        stringBuilder.AppendLine("      <dependentAssembly>");
        stringBuilder.AppendLine("        <assemblyIdentity name=\"DotNetOpenAuth.AspNet\" publicKeyToken=\"2780ccd10d57b246\" />");
        stringBuilder.AppendLine("        <bindingRedirect oldVersion=\"0.0.0.0-4.1.0.0\" newVersion=\"4.1.0.0\" />");
        stringBuilder.AppendLine("      </dependentAssembly>");
        stringBuilder.AppendLine("      <dependentAssembly>");
        stringBuilder.AppendLine("        <assemblyIdentity name=\"System.Web.Helpers\" publicKeyToken=\"31bf3856ad364e35\" />");
        stringBuilder.AppendLine("        <bindingRedirect oldVersion=\"1.0.0.0-2.0.0.0\" newVersion=\"2.0.0.0\" />");
        stringBuilder.AppendLine("      </dependentAssembly>");
        stringBuilder.AppendLine("      <dependentAssembly>");
        stringBuilder.AppendLine("        <assemblyIdentity name=\"System.Web.Mvc\" publicKeyToken=\"31bf3856ad364e35\" />");
        stringBuilder.AppendLine("        <bindingRedirect oldVersion=\"0.0.0.0-4.0.0.0\" newVersion=\"4.0.0.0\" />");
        stringBuilder.AppendLine("      </dependentAssembly>");
        stringBuilder.AppendLine("      <dependentAssembly>");
        stringBuilder.AppendLine("        <assemblyIdentity name=\"System.Web.WebPages\" publicKeyToken=\"31bf3856ad364e35\" />");
        stringBuilder.AppendLine("        <bindingRedirect oldVersion=\"1.0.0.0-2.0.0.0\" newVersion=\"2.0.0.0\" />");
        stringBuilder.AppendLine("      </dependentAssembly>");
        stringBuilder.AppendLine("      <dependentAssembly>");
        stringBuilder.AppendLine("        <assemblyIdentity name=\"WebGrease\" publicKeyToken=\"31bf3856ad364e35\" />");
        stringBuilder.AppendLine("        <bindingRedirect oldVersion=\"0.0.0.0-1.3.0.0\" newVersion=\"1.3.0.0\" />");
        stringBuilder.AppendLine("      </dependentAssembly>");
        stringBuilder.AppendLine("    </assemblyBinding>");
        stringBuilder.AppendLine("  </runtime>");
        stringBuilder.AppendLine("  <entityFramework>");
        stringBuilder.AppendLine("    <defaultConnectionFactory type=\"System.Data.Entity.Infrastructure.SqlConnectionFactory, EntityFramework\" />");
        stringBuilder.AppendLine("  </entityFramework>");
        stringBuilder.AppendLine("</configuration>");
        streamWriter.Write(stringBuilder.ToString());
      }
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._path + MyConstants.WordWebDotConfig))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("<?xml version=\"1.0\"?>");
        stringBuilder.AppendLine("<!--");
        stringBuilder.AppendLine("  For more information on how to configure your ASP.NET application, please visit");
        stringBuilder.AppendLine("  http://go.microsoft.com/fwlink/?LinkId=152368");
        stringBuilder.AppendLine("  -->");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("<configuration>");
        stringBuilder.AppendLine("    <appSettings>");
        stringBuilder.AppendLine("        <add key=\"webpages:Version\" value=\"1.0.0.0\"/>");
        stringBuilder.AppendLine("        <add key=\"ClientValidationEnabled\" value=\"true\"/>");
        stringBuilder.AppendLine("        <add key=\"UnobtrusiveJavaScriptEnabled\" value=\"true\"/>");
        stringBuilder.AppendLine("    </appSettings>");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("    <system.web>");
        stringBuilder.AppendLine("        <compilation debug=\"true\" targetFramework=\"4.0\">");
        stringBuilder.AppendLine("            <assemblies>");
        stringBuilder.AppendLine("                <add assembly=\"System.Web.Abstractions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\" />");
        stringBuilder.AppendLine("                <add assembly=\"System.Web.Helpers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\" />");
        stringBuilder.AppendLine("                <add assembly=\"System.Web.Routing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\" />");
        stringBuilder.AppendLine("                <add assembly=\"System.Web.Mvc, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\" />");
        stringBuilder.AppendLine("                <add assembly=\"System.Web.WebPages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\" />");
        stringBuilder.AppendLine("            </assemblies>");
        stringBuilder.AppendLine("        </compilation>");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        <pages>");
        stringBuilder.AppendLine("            <namespaces>");
        stringBuilder.AppendLine("                <add namespace=\"System.Web.Helpers\" />");
        stringBuilder.AppendLine("                <add namespace=\"System.Web.Mvc\" />");
        stringBuilder.AppendLine("                <add namespace=\"System.Web.Mvc.Ajax\" />");
        stringBuilder.AppendLine("                <add namespace=\"System.Web.Mvc.Html\" />");
        stringBuilder.AppendLine("                <add namespace=\"System.Web.Routing\" />");
        stringBuilder.AppendLine("                <add namespace=\"System.Web.WebPages\"/>");
        stringBuilder.AppendLine("            </namespaces>");
        stringBuilder.AppendLine("        </pages>");
        stringBuilder.AppendLine("    </system.web>");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("    <system.webServer>");
        stringBuilder.AppendLine("        <validation validateIntegratedModeConfiguration=\"false\"/>");
        stringBuilder.AppendLine("        <modules runAllManagedModulesForAllRequests=\"true\"/>");
        stringBuilder.AppendLine("    </system.webServer>");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("    <runtime>");
        stringBuilder.AppendLine("        <assemblyBinding xmlns=\"urn:schemas-microsoft-com:asm.v1\">");
        stringBuilder.AppendLine("            <dependentAssembly>");
        stringBuilder.AppendLine("                <assemblyIdentity name=\"System.Web.Mvc\" publicKeyToken=\"31bf3856ad364e35\" />");
        stringBuilder.AppendLine("                <bindingRedirect oldVersion=\"1.0.0.0-2.0.0.0\" newVersion=\"3.0.0.0\" />");
        stringBuilder.AppendLine("            </dependentAssembly>");
        stringBuilder.AppendLine("        </assemblyBinding>");
        stringBuilder.AppendLine("    </runtime>");
        stringBuilder.AppendLine("</configuration>");
        streamWriter.Write(stringBuilder.ToString());
      }
    }

    private void GenerateDebug()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._path + MyConstants.WordWebDotDebugDotConfig))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("<?xml version=\"1.0\"?>");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("<!-- For more information on using web.config transformation visit http://go.microsoft.com/fwlink/?LinkId=125889 -->");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("<configuration xmlns:xdt=\"http://schemas.microsoft.com/XML-Document-Transform\">");
        stringBuilder.AppendLine("  <!--");
        stringBuilder.AppendLine("    In the example below, the \"SetAttributes\" transform will change the value of ");
        stringBuilder.AppendLine("    \"connectionString\" to use \"ReleaseSQLServer\" only when the \"Match\" locator ");
        stringBuilder.AppendLine("    finds an atrribute \"name\" that has a value of \"MyDB\".");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("    <connectionStrings>");
        stringBuilder.AppendLine("      <add name=\"MyDB\"");
        stringBuilder.AppendLine("        connectionString=\"Data Source=ReleaseSQLServer;Initial Catalog=MyReleaseDB;Integrated Security=True\"");
        stringBuilder.AppendLine("        xdt:Transform=\"SetAttributes\" xdt:Locator=\"Match(name)\"/>");
        stringBuilder.AppendLine("    </connectionStrings>");
        stringBuilder.AppendLine("  -->");
        stringBuilder.AppendLine("  <system.web>");
        stringBuilder.AppendLine("    <!--");
        stringBuilder.AppendLine("      In the example below, the \"Replace\" transform will replace the entire ");
        stringBuilder.AppendLine("      <customErrors> section of your web.config file.");
        stringBuilder.AppendLine("      Note that because there is only one customErrors section under the ");
        stringBuilder.AppendLine("      <system.web> node, there is no need to use the \"xdt:Locator\" attribute.");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("      <customErrors defaultRedirect=\"GenericError.htm\"");
        stringBuilder.AppendLine("        mode=\"RemoteOnly\" xdt:Transform=\"Replace\">");
        stringBuilder.AppendLine("        <error statusCode=\"500\" redirect=\"InternalError.htm\"/>");
        stringBuilder.AppendLine("      </customErrors>");
        stringBuilder.AppendLine("    -->");
        stringBuilder.AppendLine("  </system.web>");
        stringBuilder.AppendLine("</configuration>");
        streamWriter.Write(stringBuilder.ToString());
      }
    }

    private void GenerateRelease()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._path + MyConstants.WordWebDotReleaseDotConfig))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("<?xml version=\"1.0\"?>");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("<!-- For more information on using web.config transformation visit http://go.microsoft.com/fwlink/?LinkId=125889 -->");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("<configuration xmlns:xdt=\"http://schemas.microsoft.com/XML-Document-Transform\">");
        stringBuilder.AppendLine("  <!--");
        stringBuilder.AppendLine("    In the example below, the \"SetAttributes\" transform will change the value of ");
        stringBuilder.AppendLine("    \"connectionString\" to use \"ReleaseSQLServer\" only when the \"Match\" locator");
        stringBuilder.AppendLine("    finds an atrribute \"name\" that has a value of \"MyDB\".");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("    <connectionStrings>");
        stringBuilder.AppendLine("      <add name=\"MyDB\"");
        stringBuilder.AppendLine("        connectionString=\"Data Source=ReleaseSQLServer;Initial Catalog=MyReleaseDB;Integrated Security=True\"");
        stringBuilder.AppendLine("        xdt:Transform=\"SetAttributes\" xdt:Locator=\"Match(name)\"/>");
        stringBuilder.AppendLine("    </connectionStrings>");
        stringBuilder.AppendLine("  -->");
        stringBuilder.AppendLine("  <system.web>");
        stringBuilder.AppendLine("    <compilation xdt:Transform=\"RemoveAttributes(debug)\" />");
        stringBuilder.AppendLine("    <!--");
        stringBuilder.AppendLine("      In the example below, the \"Replace\" transform will replace the entire ");
        stringBuilder.AppendLine("      <customErrors> section of your web.config file.");
        stringBuilder.AppendLine("      Note that because there is only one customErrors section under the ");
        stringBuilder.AppendLine("      <system.web> node, there is no need to use the \"xdt:Locator\" attribute.");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("      <customErrors defaultRedirect=\"GenericError.htm\"");
        stringBuilder.AppendLine("        mode=\"RemoteOnly\" xdt:Transform=\"Replace\">");
        stringBuilder.AppendLine("        <error statusCode=\"500\" redirect=\"InternalError.htm\"/>");
        stringBuilder.AppendLine("      </customErrors>");
        stringBuilder.AppendLine("    -->");
        stringBuilder.AppendLine("  </system.web>");
        stringBuilder.AppendLine("</configuration>");
        streamWriter.Write(stringBuilder.ToString());
      }
    }

    private void GenerateDebugMVC5()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._path + MyConstants.WordWebDotDebugDotConfig))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("<?xml version=\"1.0\"?>");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("<!-- For more information on using Web.config transformation visit http://go.microsoft.com/fwlink/?LinkId=301874 -->");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("<configuration xmlns:xdt=\"http://schemas.microsoft.com/XML-Document-Transform\">");
        stringBuilder.AppendLine("  <!--");
        stringBuilder.AppendLine("    In the example below, the \"SetAttributes\" transform will change the value of");
        stringBuilder.AppendLine("    \"connectionString\" to use \"ReleaseSQLServer\" only when the \"Match\" locator");
        stringBuilder.AppendLine("    finds an atrribute \"name\" that has a value of \"MyDB\".");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("    <connectionStrings>");
        stringBuilder.AppendLine("      <add name=\"MyDB\"");
        stringBuilder.AppendLine("        connectionString=\"Data Source=ReleaseSQLServer;Initial Catalog=MyReleaseDB;Integrated Security=True\"");
        stringBuilder.AppendLine("        xdt:Transform=\"SetAttributes\" xdt:Locator=\"Match(name)\"/>");
        stringBuilder.AppendLine("    </connectionStrings>");
        stringBuilder.AppendLine("  -->");
        stringBuilder.AppendLine("  <system.web>");
        stringBuilder.AppendLine("    <!--");
        stringBuilder.AppendLine("      In the example below, the \"Replace\" transform will replace the entire");
        stringBuilder.AppendLine("      <customErrors> section of your Web.config file.");
        stringBuilder.AppendLine("      Note that because there is only one customErrors section under the");
        stringBuilder.AppendLine("      <system.web> node, there is no need to use the \"xdt:Locator\" attribute.");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("      <customErrors defaultRedirect=\"GenericError.htm\"");
        stringBuilder.AppendLine("        mode=\"RemoteOnly\" xdt:Transform=\"Replace\">");
        stringBuilder.AppendLine("        <error statusCode=\"500\" redirect=\"InternalError.htm\"/>");
        stringBuilder.AppendLine("      </customErrors>");
        stringBuilder.AppendLine("    -->");
        stringBuilder.AppendLine("  </system.web>");
        stringBuilder.AppendLine("</configuration>");
        streamWriter.Write(stringBuilder.ToString());
      }
    }

    private void GenerateReleaseMVC5()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._path + MyConstants.WordWebDotReleaseDotConfig))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("<?xml version=\"1.0\"?>");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("<!-- For more information on using Web.config transformation visit http://go.microsoft.com/fwlink/?LinkId=301874 -->");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("<configuration xmlns:xdt=\"http://schemas.microsoft.com/XML-Document-Transform\">");
        stringBuilder.AppendLine("  <!--");
        stringBuilder.AppendLine("    In the example below, the \"SetAttributes\" transform will change the value of");
        stringBuilder.AppendLine("    \"connectionString\" to use \"ReleaseSQLServer\" only when the \"Match\" locator");
        stringBuilder.AppendLine("    finds an atrribute \"name\" that has a value of \"MyDB\".");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("    <connectionStrings>");
        stringBuilder.AppendLine("      <add name=\"MyDB\"");
        stringBuilder.AppendLine("        connectionString=\"Data Source=ReleaseSQLServer;Initial Catalog=MyReleaseDB;Integrated Security=True\"");
        stringBuilder.AppendLine("        xdt:Transform=\"SetAttributes\" xdt:Locator=\"Match(name)\"/>");
        stringBuilder.AppendLine("    </connectionStrings>");
        stringBuilder.AppendLine("  -->");
        stringBuilder.AppendLine("  <system.web>");
        stringBuilder.AppendLine("    <compilation xdt:Transform=\"RemoveAttributes(debug)\" />");
        stringBuilder.AppendLine("    <!--");
        stringBuilder.AppendLine("      In the example below, the \"Replace\" transform will replace the entire");
        stringBuilder.AppendLine("      <customErrors> section of your Web.config file.");
        stringBuilder.AppendLine("      Note that because there is only one customErrors section under the");
        stringBuilder.AppendLine("      <system.web> node, there is no need to use the \"xdt:Locator\" attribute.");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("      <customErrors defaultRedirect=\"GenericError.htm\"");
        stringBuilder.AppendLine("        mode=\"RemoteOnly\" xdt:Transform=\"Replace\">");
        stringBuilder.AppendLine("        <error statusCode=\"500\" redirect=\"InternalError.htm\"/>");
        stringBuilder.AppendLine("      </customErrors>");
        stringBuilder.AppendLine("    -->");
        stringBuilder.AppendLine("  </system.web>");
        stringBuilder.AppendLine("</configuration>");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
