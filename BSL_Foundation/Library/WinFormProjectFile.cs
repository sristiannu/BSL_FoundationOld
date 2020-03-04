
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class WinFormProjectFile
  {
    private Tables _selectedTables;
    private string _fileExtension;
    private string _fullFileNamePath;
    private Language _language;
    private string _windowsAppName;
    private string _projectGuid;
    private bool _isUseStoredProcedure;
    private IsCheckedView _isCheckedView;
    private ViewNames _viewNames;
    private DatabaseObjectToGenerateFrom _generateFrom;
    private ApplicationVersion _appVersion;

    private WinFormProjectFile()
    {
    }

    internal WinFormProjectFile(Tables selectedTables, string fullFileNamePath, Language language, string windowsAppName, string projectGuid, DatabaseObjectToGenerateFrom generateFrom, ApplicationVersion appVersion, bool isUseStoredProcedure, IsCheckedView isCheckedView, ViewNames viewNames, string fileExtension)
    {
      this._selectedTables = selectedTables;
      this._fullFileNamePath = fullFileNamePath;
      this._language = language;
      this._windowsAppName = windowsAppName.Trim();
      this._projectGuid = projectGuid;
      this._generateFrom = generateFrom;
      this._appVersion = appVersion;
      this._isUseStoredProcedure = isUseStoredProcedure;
      this._isCheckedView = isCheckedView;
      this._viewNames = viewNames;
      this._fileExtension = fileExtension;
      this.Generate();
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder sb = new StringBuilder();
        StringBuilder stringBuilder1 = new StringBuilder();
        StringBuilder stringBuilder2 = new StringBuilder();
        StringBuilder stringBuilder3 = new StringBuilder();
        StringBuilder stringBuilder4 = new StringBuilder();
        StringBuilder stringBuilder5 = new StringBuilder();
        StringBuilder stringBuilder6 = new StringBuilder();
        StringBuilder stringBuilder7 = new StringBuilder();
        StringBuilder stringBuilder8 = new StringBuilder();
        StringBuilder stringBuilder9 = new StringBuilder();
        StringBuilder stringBuilder10 = new StringBuilder();
        StringBuilder stringBuilder11 = new StringBuilder();
        IOrderedEnumerable<Table> orderedEnumerable = this._selectedTables.OrderBy<Table, string>((st => st.Name));
        sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        sb.AppendLine("<Project ToolsVersion=\"12.0\" DefaultTargets=\"Build\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\">");
        sb.AppendLine("  <Import Project=\"$(MSBuildExtensionsPath)\\$(MSBuildToolsVersion)\\Microsoft.Common.props\" Condition=\"Exists('$(MSBuildExtensionsPath)\\$(MSBuildToolsVersion)\\Microsoft.Common.props')\" />");
        sb.AppendLine("  <PropertyGroup>");
        sb.AppendLine("    <Configuration Condition=\" '$(Configuration)' == '' \">Debug</Configuration>");
        sb.AppendLine("    <Platform Condition=\" '$(Platform)' == '' \">AnyCPU</Platform>");
        sb.AppendLine("    <ProjectGuid>" + this._projectGuid + "}</ProjectGuid>");
        sb.AppendLine("    <OutputType>WinExe</OutputType>");
        if (this._language == Language.CSharp)
          sb.AppendLine("    <AppDesignerFolder>Properties</AppDesignerFolder>");
        else
          sb.AppendLine("    <StartupObject>" + this._windowsAppName + ".My.MyApplication</StartupObject>");
        sb.AppendLine("    <RootNamespace>" + this._windowsAppName + "</RootNamespace>");
        sb.AppendLine("    <AssemblyName>" + this._windowsAppName + "</AssemblyName>");
        sb.AppendLine("    <TargetFrameworkVersion>v4.5.1</TargetFrameworkVersion>");
        sb.AppendLine("    <FileAlignment>512</FileAlignment>");
        if (this._language == Language.VB)
          sb.AppendLine("    <MyType>WindowsForms</MyType>");
        sb.AppendLine("    <AutoGenerateBindingRedirects>true</AutoGenerateBindingRedirects>");
        sb.AppendLine("    <SccProjectName>SAK</SccProjectName>");
        sb.AppendLine("    <SccLocalPath>SAK</SccLocalPath>");
        sb.AppendLine("    <SccAuxPath>SAK</SccAuxPath>");
        sb.AppendLine("    <SccProvider>SAK</SccProvider>");
        sb.AppendLine("  </PropertyGroup>");
        sb.AppendLine("  <PropertyGroup Condition=\" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' \">");
        sb.AppendLine("    <PlatformTarget>AnyCPU</PlatformTarget>");
        sb.AppendLine("    <DebugSymbols>true</DebugSymbols>");
        sb.AppendLine("    <DebugType>full</DebugType>");
        if (this._language == Language.CSharp)
        {
          sb.AppendLine("    <Optimize>false</Optimize>");
        }
        else
        {
          sb.AppendLine("    <DefineDebug>true</DefineDebug>");
          sb.AppendLine("    <DefineTrace>true</DefineTrace>");
        }
        sb.AppendLine("    <OutputPath>bin\\Debug\\</OutputPath>");
        if (this._language == Language.CSharp)
        {
          sb.AppendLine("    <DefineConstants>DEBUG;TRACE</DefineConstants>");
          sb.AppendLine("    <ErrorReport>prompt</ErrorReport>");
          sb.AppendLine("    <WarningLevel>4</WarningLevel>");
        }
        else
        {
          sb.AppendLine("    <DocumentationFile>WindowsApplicationVB1.xml</DocumentationFile>");
          sb.AppendLine("    <NoWarn>42016,41999,42017,42018,42019,42032,42036,42020,42021,42022</NoWarn>");
        }
        sb.AppendLine("  </PropertyGroup>");
        sb.AppendLine("  <PropertyGroup Condition=\" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' \">");
        sb.AppendLine("    <PlatformTarget>AnyCPU</PlatformTarget>");
        sb.AppendLine("    <DebugType>pdbonly</DebugType>");
        if (this._language == Language.VB)
        {
          sb.AppendLine("    <DefineDebug>false</DefineDebug>");
          sb.AppendLine("    <DefineTrace>true</DefineTrace>");
        }
        sb.AppendLine("    <Optimize>true</Optimize>");
        sb.AppendLine("    <OutputPath>bin\\Release\\</OutputPath>");
        if (this._language == Language.CSharp)
        {
          sb.AppendLine("    <DefineConstants>TRACE</DefineConstants>");
          sb.AppendLine("    <ErrorReport>prompt</ErrorReport>");
          sb.AppendLine("    <WarningLevel>4</WarningLevel>");
        }
        else
        {
          sb.AppendLine("    <DocumentationFile>WindowsApplicationVB1.xml</DocumentationFile>");
          sb.AppendLine("    <NoWarn>42016,41999,42017,42018,42019,42032,42036,42020,42021,42022</NoWarn>");
        }
        sb.AppendLine("  </PropertyGroup>");
        if (this._language == Language.VB)
        {
          sb.AppendLine("  <PropertyGroup>");
          sb.AppendLine("    <OptionExplicit>On</OptionExplicit>");
          sb.AppendLine("  </PropertyGroup>");
          sb.AppendLine("  <PropertyGroup>");
          sb.AppendLine("    <OptionCompare>Binary</OptionCompare>");
          sb.AppendLine("  </PropertyGroup>");
          sb.AppendLine("  <PropertyGroup>");
          sb.AppendLine("    <OptionStrict>Off</OptionStrict>");
          sb.AppendLine("  </PropertyGroup>");
          sb.AppendLine("  <PropertyGroup>");
          sb.AppendLine("    <OptionInfer>On</OptionInfer>");
          sb.AppendLine("  </PropertyGroup>");
        }
        sb.AppendLine("  <ItemGroup>");
        sb.AppendLine("    <Reference Include=\"System\" />");
        sb.AppendLine("    <Reference Include=\"System.configuration\" />");
        sb.AppendLine("    <Reference Include=\"System.Core\" />");
        sb.AppendLine("    <Reference Include=\"System.Runtime.Serialization\" />");
        sb.AppendLine("    <Reference Include=\"System.Security\" />");
        sb.AppendLine("    <Reference Include=\"System.Xml.Linq\" />");
        sb.AppendLine("    <Reference Include=\"System.Data.DataSetExtensions\" />");
        if (this._language == Language.CSharp)
          sb.AppendLine("    <Reference Include=\"Microsoft.CSharp\" />");
        sb.AppendLine("    <Reference Include=\"System.Data\" />");
        sb.AppendLine("    <Reference Include=\"System.Deployment\" />");
        sb.AppendLine("    <Reference Include=\"System.Drawing\" />");
        sb.AppendLine("    <Reference Include=\"System.Windows.Forms\" />");
        sb.AppendLine("    <Reference Include=\"System.Xml\" />");
        sb.AppendLine("  </ItemGroup>");
        if (this._language == Language.VB)
        {
          sb.AppendLine("  <ItemGroup>");
          sb.AppendLine("    <Import Include=\"Microsoft.VisualBasic\" />");
          sb.AppendLine("    <Import Include=\"System\" />");
          sb.AppendLine("    <Import Include=\"System.Collections\" />");
          sb.AppendLine("    <Import Include=\"System.Collections.Generic\" />");
          sb.AppendLine("    <Import Include=\"System.Data\" />");
          sb.AppendLine("    <Import Include=\"System.Drawing\" />");
          sb.AppendLine("    <Import Include=\"System.Diagnostics\" />");
          sb.AppendLine("    <Import Include=\"System.Windows.Forms\" />");
          sb.AppendLine("    <Import Include=\"System.Linq\" />");
          sb.AppendLine("    <Import Include=\"System.Xml.Linq\" />");
          sb.AppendLine("    <Import Include=\"System.Threading.Tasks\" />");
          sb.AppendLine("  </ItemGroup>");
        }
        sb.AppendLine("  <ItemGroup>");
        this.AppendCompileInclude(sb, "", "DefaultWinForm", "");
        foreach (Table table in  orderedEnumerable)
        {
          if (this._generateFrom == DatabaseObjectToGenerateFrom.Tables && !table.IsContainsPrimaryAndForeignKeyColumnsOnly && this._appVersion != ApplicationVersion.Express)
          {
            if (this._isCheckedView.AddRecord)
              this.AppendCompileInclude(sb, table.Name, this._viewNames.AddRecord, "Add\\");
            if (this._isCheckedView.RecordDetails)
              this.AppendCompileInclude(sb, table.Name, this._viewNames.RecordDetails, "Details\\");
            if (this._isCheckedView.ListCrudRedirect)
              this.AppendCompileInclude(sb, table.Name, this._viewNames.ListCrudRedirect, "");
            foreach (Column column in  table.Columns)
            {
              if (column.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK)
              {
                if (this._isCheckedView.ListMasterDetailGrid)
                  this.AppendCompileInclude(sb, "", this._viewNames.ListMasterDetailGrid + table.Name + "By" + column.Name, "");
                if (this._isCheckedView.ListByRelatedField)
                  this.AppendCompileInclude(sb, "", this._viewNames.ListByRelatedField + table.Name + "By" + column.Name, "");
              }
            }
            if (this._isCheckedView.ListInline)
              this.AppendCompileInclude(sb, table.Name, this._viewNames.ListInline, "");
            if (this._isCheckedView.ListReadOnly)
              this.AppendCompileInclude(sb, table.Name, this._viewNames.ListReadOnly, "");
            if (this._isCheckedView.ListSearch)
              this.AppendCompileInclude(sb, table.Name, this._viewNames.ListSearch, "");
            if (this._isCheckedView.UpdateRecord)
              this.AppendCompileInclude(sb, table.Name, this._viewNames.UpdateRecord, "Edit\\");
            if (this._isCheckedView.Unbound)
              this.AppendCompileInclude(sb, table.Name, this._viewNames.Unbound, "Unbound\\");
          }
          if (this._generateFrom == DatabaseObjectToGenerateFrom.Views && !table.IsContainsPrimaryAndForeignKeyColumnsOnly && (this._appVersion != ApplicationVersion.Express && this._isCheckedView.ListReadOnly))
            this.AppendCompileInclude(sb, table.Name, this._viewNames.ListReadOnly, "");
          if (this._appVersion == ApplicationVersion.Express)
            this.AppendCompileInclude(sb, table.Name, this._viewNames.Unbound, "Unbound\\");
        }
        sb.AppendLine("    <Compile Include=\"Code\\Helper\\ConstantHelper" + this._fileExtension + "\" />");
        sb.AppendLine("    <Compile Include=\"Code\\Helper\\ToolStripHelper" + this._fileExtension + "\" />");
        sb.AppendLine("    <Compile Include=\"Code\\Helper\\WindowHelper" + this._fileExtension + "\" />");
        sb.AppendLine("    <Compile Include=\"Domain\\CrudOperation" + this._fileExtension + "\" />");
        foreach (Table table in orderedEnumerable)
        {
          stringBuilder1.AppendLine("    <Compile Include=\"Code\\BusinessObjectBase\\" + table.Name + MyConstants.WordBase + this._fileExtension + "\" />");
          stringBuilder2.AppendLine("    <Compile Include=\"Code\\BusinessObjectCollection\\" + table.Name + MyConstants.WordList + this._fileExtension + "\" />");
          stringBuilder3.AppendLine("    <Compile Include=\"Code\\BusinessObject\\" + table.Name + this._fileExtension + "\" />");
          stringBuilder4.AppendLine("    <Compile Include=\"Code\\DataLayerBase\\" + table.Name + MyConstants.WordDataLayerBase + this._fileExtension + "\" />");
          stringBuilder5.AppendLine("    <Compile Include=\"Code\\DataLayer\\" + table.Name + MyConstants.WordDataLayer + this._fileExtension + "\" />");
          if (!this._isUseStoredProcedure && this._appVersion != ApplicationVersion.Express)
            stringBuilder6.AppendLine("    <Compile Include=\"Code\\SQL\\" + table.Name + MyConstants.WordSQL + this._fileExtension + "\" />");
          stringBuilder7.AppendLine("    <Compile Include=\"Code\\Example\\" + table.Name + MyConstants.WordExample + this._fileExtension + "\" />");
        }
        sb.Append(stringBuilder1.ToString());
        sb.Append(stringBuilder2.ToString());
        sb.Append(stringBuilder3.ToString());
        sb.Append(stringBuilder4.ToString());
        sb.Append(stringBuilder5.ToString());
        sb.Append(stringBuilder6.ToString());
        sb.Append(stringBuilder7.ToString());
        if (this._language == Language.CSharp)
        {
          sb.AppendLine("    <Compile Include=\"Program" + this._fileExtension + "\" />");
          sb.AppendLine("    <Compile Include=\"Properties\\AssemblyInfo" + this._fileExtension + "\" />");
        }
        else
        {
          sb.AppendLine("    <Compile Include=\"My Project\\AssemblyInfo" + this._fileExtension + "\" />");
          sb.AppendLine("    <Compile Include=\"My Project\\Application.Designer.vb\">");
          sb.AppendLine("      <AutoGen>True</AutoGen>");
          sb.AppendLine("      <DependentUpon>Application.myapp</DependentUpon>");
          sb.AppendLine("    </Compile>");
        }
        if (this._isCheckedView.AddRecord || this._isCheckedView.UpdateRecord)
        {
          foreach (Table table in orderedEnumerable)
          {
            if (!table.IsContainsPrimaryAndForeignKeyColumnsOnly)
            {
              sb.AppendLine("    <Compile Include=\"AddEditUserControls\\AddEdit" + table.Name + this._fileExtension + "\">");
              sb.AppendLine("      <SubType>UserControl</SubType>");
              sb.AppendLine("    </Compile>");
              sb.AppendLine("    <Compile Include=\"AddEditUserControls\\AddEdit" + table.Name + ".Designer" + this._fileExtension + "\">");
              sb.AppendLine("      <DependentUpon>AddEdit" + table.Name + this._fileExtension + "</DependentUpon>");
              sb.AppendLine("    </Compile>");
            }
          }
        }
        if (this._isCheckedView.Unbound)
        {
          foreach (Table table in  orderedEnumerable)
          {
            if (!table.IsContainsPrimaryAndForeignKeyColumnsOnly)
            {
              sb.AppendLine("    <Compile Include=\"UnboundAddEditUserControls\\UnboundAddEdit" + table.Name + this._fileExtension + "\">");
              sb.AppendLine("      <SubType>UserControl</SubType>");
              sb.AppendLine("    </Compile>");
              sb.AppendLine("    <Compile Include=\"UnboundAddEditUserControls\\UnboundAddEdit" + table.Name + ".Designer" + this._fileExtension + "\">");
              sb.AppendLine("      <DependentUpon>UnboundAddEdit" + table.Name + this._fileExtension + "</DependentUpon>");
              sb.AppendLine("    </Compile>");
            }
          }
        }
        this.AppendEmbeddedResourceInclude(sb, "", "DefaultWinForm", "");
        foreach (Table table in  orderedEnumerable)
        {
          if (this._generateFrom == DatabaseObjectToGenerateFrom.Tables && !table.IsContainsPrimaryAndForeignKeyColumnsOnly && this._appVersion != ApplicationVersion.Express)
          {
            if (this._isCheckedView.AddRecord)
              this.AppendEmbeddedResourceInclude(sb, table.Name, this._viewNames.AddRecord, "Add\\");
            if (this._isCheckedView.RecordDetails)
              this.AppendEmbeddedResourceInclude(sb, table.Name, this._viewNames.RecordDetails, "Details\\");
            if (this._isCheckedView.ListCrud)
              this.AppendEmbeddedResourceInclude(sb, table.Name, this._viewNames.ListCrud, "");
            if (this._isCheckedView.ListCrudRedirect)
              this.AppendEmbeddedResourceInclude(sb, table.Name, this._viewNames.ListCrudRedirect, "");
            foreach (Column column in table.Columns)
            {
              if (column.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK)
              {
                if (this._isCheckedView.ListMasterDetailGrid)
                  this.AppendEmbeddedResourceInclude(sb, "", this._viewNames.ListMasterDetailGrid + table.Name + "By" + column.Name, "");
                if (this._isCheckedView.ListByRelatedField)
                  this.AppendEmbeddedResourceInclude(sb, "", this._viewNames.ListByRelatedField + table.Name + "By" + column.Name, "");
              }
            }
            if (this._isCheckedView.ListInline)
              this.AppendEmbeddedResourceInclude(sb, table.Name, this._viewNames.ListInline, "");
            if (this._isCheckedView.ListReadOnly)
              this.AppendEmbeddedResourceInclude(sb, table.Name, this._viewNames.ListReadOnly, "");
            if (this._isCheckedView.ListSearch)
              this.AppendEmbeddedResourceInclude(sb, table.Name, this._viewNames.ListSearch, "");
            if (this._isCheckedView.Unbound)
              this.AppendEmbeddedResourceInclude(sb, table.Name, this._viewNames.Unbound, "Unbound\\");
            if (this._isCheckedView.UpdateRecord)
              this.AppendEmbeddedResourceInclude(sb, table.Name, this._viewNames.UpdateRecord, "Edit\\");
          }
          if (this._generateFrom == DatabaseObjectToGenerateFrom.Views && this._appVersion != ApplicationVersion.Express && this._isCheckedView.ListReadOnly)
            this.AppendEmbeddedResourceInclude(sb, table.Name, this._viewNames.ListReadOnly, "");
          if (this._appVersion == ApplicationVersion.Express)
            this.AppendEmbeddedResourceInclude(sb, table.Name, this._viewNames.Unbound, "Unbound\\");
        }
        if (this._language == Language.CSharp)
        {
          sb.AppendLine("    <EmbeddedResource Include=\"Properties\\Resources.resx\">");
          sb.AppendLine("      <Generator>ResXFileCodeGenerator</Generator>");
          sb.AppendLine("      <LastGenOutput>Resources.Designer" + this._fileExtension + "</LastGenOutput>");
          sb.AppendLine("      <SubType>Designer</SubType>");
          sb.AppendLine("    </EmbeddedResource>");
          sb.AppendLine("    <Compile Include=\"Properties\\Resources.Designer" + this._fileExtension + "\">");
          sb.AppendLine("      <AutoGen>True</AutoGen>");
          sb.AppendLine("      <DependentUpon>Resources.resx</DependentUpon>");
          sb.AppendLine("    </Compile>");
        }
        else
        {
          sb.AppendLine("    <Compile Include=\"My Project\\Resources.Designer.vb\">");
          sb.AppendLine("      <AutoGen>True</AutoGen>");
          sb.AppendLine("      <DesignTime>True</DesignTime>");
          sb.AppendLine("      <DependentUpon>Resources.resx</DependentUpon>");
          sb.AppendLine("    </Compile>");
        }
        if (this._language == Language.CSharp)
        {
          sb.AppendLine("    <None Include=\"packages.config\" />");
          sb.AppendLine("    <None Include=\"Properties\\Settings.settings\">");
          sb.AppendLine("      <Generator>SettingsSingleFileGenerator</Generator>");
          sb.AppendLine("      <LastGenOutput>Settings.Designer" + this._fileExtension + "</LastGenOutput>");
          sb.AppendLine("    </None>");
          sb.AppendLine("    <Compile Include=\"Properties\\Settings.Designer" + this._fileExtension + "\">");
        }
        else
          sb.AppendLine("    <Compile Include=\"My Project\\Settings.Designer.vb\">");
        sb.AppendLine("      <AutoGen>True</AutoGen>");
        sb.AppendLine("      <DependentUpon>Settings.settings</DependentUpon>");
        sb.AppendLine("      <DesignTimeSharedInput>True</DesignTimeSharedInput>");
        sb.AppendLine("    </Compile>");
        sb.AppendLine("  </ItemGroup>");
        sb.AppendLine("  <ItemGroup>");
        if (this._language == Language.VB)
        {
          sb.AppendLine("    <EmbeddedResource Include=\"My Project\\Resources.resx\">");
          sb.AppendLine("      <Generator>VbMyResourcesResXFileCodeGenerator</Generator>");
          sb.AppendLine("      <LastGenOutput>Resources.Designer.vb</LastGenOutput>");
          sb.AppendLine("      <CustomToolNamespace>My.Resources</CustomToolNamespace>");
          sb.AppendLine("      <SubType>Designer</SubType>");
          sb.AppendLine("    </EmbeddedResource>");
          sb.AppendLine("  </ItemGroup>");
          sb.AppendLine("  <ItemGroup>");
          sb.AppendLine("    <None Include=\"My Project\\Application.myapp\">");
          sb.AppendLine("      <Generator>MyApplicationCodeGenerator</Generator>");
          sb.AppendLine("      <LastGenOutput>Application.Designer.vb</LastGenOutput>");
          sb.AppendLine("    </None>");
          sb.AppendLine("    <None Include=\"My Project\\Settings.settings\">");
          sb.AppendLine("      <Generator>SettingsSingleFileGenerator</Generator>");
          sb.AppendLine("      <CustomToolNamespace>My</CustomToolNamespace>");
          sb.AppendLine("      <LastGenOutput>Settings.Designer.vb</LastGenOutput>");
          sb.AppendLine("    </None>");
        }
        sb.AppendLine("    <None Include=\"App.config\">");
        sb.AppendLine("      <SubType>Designer</SubType>");
        sb.AppendLine("    </None>");
        sb.AppendLine("  </ItemGroup>");
        if (this._language == Language.CSharp)
          sb.AppendLine("  <Import Project=\"$(MSBuildToolsPath)\\Microsoft.CSharp.targets\" />");
        else
          sb.AppendLine("  <Import Project=\"$(MSBuildToolsPath)\\Microsoft.VisualBasic.targets\" />");
        sb.AppendLine("  <!-- To modify your build process, add your task inside one of the targets below and uncomment it. ");
        sb.AppendLine("       Other similar extension points exist, see Microsoft.Common.targets.");
        sb.AppendLine("  <Target Name=\"BeforeBuild\">");
        sb.AppendLine("  </Target>");
        sb.AppendLine("  <Target Name=\"AfterBuild\">");
        sb.AppendLine("  </Target>");
        sb.AppendLine("  -->");
        sb.AppendLine("</Project>");
        streamWriter.Write(sb.ToString());
      }
    }

    private void AppendCompileInclude(StringBuilder sb, string tableName, string viewName, string folderName = "")
    {
      sb.AppendLine("    <Compile Include=\"" + folderName + viewName + tableName + this._fileExtension + "\">");
      sb.AppendLine("      <SubType>Form</SubType>");
      sb.AppendLine("    </Compile>");
      sb.AppendLine("    <Compile Include=\"" + folderName + viewName + tableName + ".Designer" + this._fileExtension + "\">");
      sb.AppendLine("      <DependentUpon>" + viewName + tableName + this._fileExtension + "</DependentUpon>");
      if (this._language == Language.VB)
        sb.AppendLine("      <SubType>Form</SubType>");
      sb.AppendLine("    </Compile>");
    }

    private void AppendEmbeddedResourceInclude(StringBuilder sb, string tableName, string viewName, string folderName = "")
    {
      sb.AppendLine("    <EmbeddedResource Include=\"" + folderName + viewName + tableName + ".resx\">");
      sb.AppendLine("      <DependentUpon>" + viewName + tableName + this._fileExtension + "</DependentUpon>");
      sb.AppendLine("    </EmbeddedResource>");
    }

    private void BuildCSPart(StringBuilder sb)
    {
      sb.AppendLine("        <MvcBuildViews>false</MvcBuildViews>");
      sb.AppendLine("        <UseIISExpress>true</UseIISExpress>");
      sb.AppendLine("        <IISExpressSSLPort />");
      sb.AppendLine("        <IISExpressAnonymousAuthentication />");
      sb.AppendLine("        <IISExpressWindowsAuthentication />");
      sb.AppendLine("        <IISExpressUseClassicPipelineMode />");
      sb.AppendLine("        <TargetFrameworkProfile />");
      sb.AppendLine("    </PropertyGroup>");
      sb.AppendLine("    <PropertyGroup Condition=\" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' \">");
      sb.AppendLine("        <DebugSymbols>true</DebugSymbols>");
      sb.AppendLine("        <DebugType>full</DebugType>");
      sb.AppendLine("        <Optimize>false</Optimize>");
      sb.AppendLine("        <OutputPath>bin\\</OutputPath>");
      sb.AppendLine("        <DefineConstants>DEBUG;TRACE</DefineConstants>");
      sb.AppendLine("        <ErrorReport>prompt</ErrorReport>");
      sb.AppendLine("        <WarningLevel>4</WarningLevel>");
      sb.AppendLine("    </PropertyGroup>");
      sb.AppendLine("    <PropertyGroup Condition=\" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' \">");
      sb.AppendLine("        <DebugType>pdbonly</DebugType>");
      sb.AppendLine("        <Optimize>true</Optimize>");
      sb.AppendLine("        <OutputPath>bin\\</OutputPath>");
      sb.AppendLine("        <DefineConstants>TRACE</DefineConstants>");
      sb.AppendLine("        <ErrorReport>prompt</ErrorReport>");
      sb.AppendLine("        <WarningLevel>4</WarningLevel>");
      sb.AppendLine("    </PropertyGroup>");
      sb.AppendLine("    <ItemGroup>");
      sb.AppendLine("      <Reference Include=\"Microsoft.CSharp\" />");
      sb.AppendLine("      <Reference Include=\"System\" />");
      sb.AppendLine("      <Reference Include=\"System.Data\" />");
      sb.AppendLine("      <Reference Include=\"System.Data.DataSetExtensions\" />");
      sb.AppendLine("      <Reference Include=\"System.Drawing\" />");
      sb.AppendLine("      <Reference Include=\"System.Web.DynamicData\" />");
      sb.AppendLine("      <Reference Include=\"System.Web.Entity\" />");
      sb.AppendLine("      <Reference Include=\"System.Web.ApplicationServices\" />");
      sb.AppendLine("      <Reference Include=\"System.ComponentModel.DataAnnotations\" />");
      sb.AppendLine("      <Reference Include=\"System.Web.Extensions\" />");
      sb.AppendLine("      <Reference Include=\"System.Web\" />");
      sb.AppendLine("      <Reference Include=\"System.Web.Abstractions\" />");
      sb.AppendLine("      <Reference Include=\"System.Web.Routing\" />");
      sb.AppendLine("      <Reference Include=\"System.Xml\" />");
      sb.AppendLine("      <Reference Include=\"System.Configuration\" />");
      sb.AppendLine("      <Reference Include=\"System.Web.Services\" />");
      sb.AppendLine("      <Reference Include=\"System.EnterpriseServices\" />");
      sb.AppendLine("      <Reference Include=\"Microsoft.Web.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL\">");
      sb.AppendLine("        <Private>True</Private>");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.Web.Infrastructure.1.0.0.0\\lib\\net40\\Microsoft.Web.Infrastructure.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"System.Net.Http\">");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"System.Net.Http.WebRequest\">");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"System.Web.Helpers, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL\">");
      sb.AppendLine("        <Private>True</Private>");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.AspNet.WebPages.3.0.0\\lib\\net45\\System.Web.Helpers.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"System.Web.Mvc, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL\">");
      sb.AppendLine("        <Private>True</Private>");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.AspNet.Mvc.5.0.0\\lib\\net45\\System.Web.Mvc.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"System.Web.Optimization\">");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.AspNet.Web.Optimization.1.1.1\\lib\\net40\\System.Web.Optimization.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"System.Web.Razor, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL\">");
      sb.AppendLine("        <Private>True</Private>");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.AspNet.Razor.3.0.0\\lib\\net45\\System.Web.Razor.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"System.Web.WebPages, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL\">");
      sb.AppendLine("        <Private>True</Private>");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.AspNet.WebPages.3.0.0\\lib\\net45\\System.Web.WebPages.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"System.Web.WebPages.Deployment, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL\">");
      sb.AppendLine("        <Private>True</Private>");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.AspNet.WebPages.3.0.0\\lib\\net45\\System.Web.WebPages.Deployment.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"System.Web.WebPages.Razor, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL\">");
      sb.AppendLine("        <Private>True</Private>");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.AspNet.WebPages.3.0.0\\lib\\net45\\System.Web.WebPages.Razor.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"Newtonsoft.Json\">");
      sb.AppendLine("        <Private>True</Private>");
      sb.AppendLine("        <HintPath>..\\packages\\Newtonsoft.Json.5.0.6\\lib\\net45\\Newtonsoft.Json.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"System.Xml.Linq\" />");
      sb.AppendLine("      <Reference Include=\"WebGrease\">");
      sb.AppendLine("        <Private>True</Private>");
      sb.AppendLine("        <HintPath>..\\packages\\WebGrease.1.5.2\\lib\\WebGrease.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"Antlr3.Runtime\">");
      sb.AppendLine("        <Private>True</Private>");
      sb.AppendLine("        <HintPath>..\\packages\\Antlr.3.4.1.9004\\lib\\Antlr3.Runtime.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"Newtonsoft.Json\">");
      sb.AppendLine("        <HintPath>..\\packages\\Newtonsoft.Json.5.0.6\\lib\\net45\\Newtonsoft.Json.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"System.Net.Http.Formatting\">");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.AspNet.WebApi.Client.5.0.0\\lib\\net45\\System.Net.Http.Formatting.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"System.Web.Http\">");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.AspNet.WebApi.Core.5.0.0\\lib\\net45\\System.Web.Http.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"System.Web.Http.WebHost\">");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.AspNet.WebApi.WebHost.5.0.0\\lib\\net45\\System.Web.Http.WebHost.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"EntityFramework\">");
      sb.AppendLine("        <HintPath>..\\packages\\EntityFramework.6.0.0\\lib\\net45\\EntityFramework.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"EntityFramework.SqlServer\">");
      sb.AppendLine("        <HintPath>..\\packages\\EntityFramework.6.0.0\\lib\\net45\\EntityFramework.SqlServer.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"Microsoft.AspNet.Identity.Core\">");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.AspNet.Identity.Core.1.0.0\\lib\\net45\\Microsoft.AspNet.Identity.Core.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"Microsoft.AspNet.Identity.Owin\">");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.AspNet.Identity.Owin.1.0.0\\lib\\net45\\Microsoft.AspNet.Identity.Owin.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"Microsoft.AspNet.Identity.EntityFramework\">");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.AspNet.Identity.EntityFramework.1.0.0\\lib\\net45\\Microsoft.AspNet.Identity.EntityFramework.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"Owin\">");
      sb.AppendLine("        <HintPath>..\\packages\\Owin.1.0\\lib\\net40\\Owin.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"Microsoft.Owin\">");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.Owin.2.0.0\\lib\\net45\\Microsoft.Owin.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"Microsoft.Owin.Host.SystemWeb\">");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.Owin.Host.SystemWeb.2.0.0\\lib\\net45\\Microsoft.Owin.Host.SystemWeb.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"Microsoft.Owin.Security\">");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.Owin.Security.2.0.0\\lib\\net45\\Microsoft.Owin.Security.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"Microsoft.Owin.Security.Facebook\">");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.Owin.Security.Facebook.2.0.0\\lib\\net45\\Microsoft.Owin.Security.Facebook.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"Microsoft.Owin.Security.Cookies\">");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.Owin.Security.Cookies.2.0.0\\lib\\net45\\Microsoft.Owin.Security.Cookies.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"Microsoft.Owin.Security.OAuth\">");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.Owin.Security.OAuth.2.0.0\\lib\\net45\\Microsoft.Owin.Security.OAuth.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"Microsoft.Owin.Security.Google\">");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.Owin.Security.Google.2.0.0\\lib\\net45\\Microsoft.Owin.Security.Google.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"Microsoft.Owin.Security.Twitter\">");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.Owin.Security.Twitter.2.0.0\\lib\\net45\\Microsoft.Owin.Security.Twitter.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"Microsoft.Owin.Security.MicrosoftAccount\">");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.Owin.Security.MicrosoftAccount.2.0.0\\lib\\net45\\Microsoft.Owin.Security.MicrosoftAccount.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("    </ItemGroup>");
    }

    private void BuildVBPart(StringBuilder sb)
    {
      sb.AppendLine("    <OptionExplicit>On</OptionExplicit>");
      sb.AppendLine("      <OptionCompare>Binary</OptionCompare>");
      sb.AppendLine("      <OptionStrict>Off</OptionStrict>");
      sb.AppendLine("      <OptionInfer>On</OptionInfer>");
      sb.AppendLine("      <MvcBuildViews>false</MvcBuildViews>");
      sb.AppendLine("      <UseIISExpress>true</UseIISExpress>");
      sb.AppendLine("      <IISExpressSSLPort />");
      sb.AppendLine("      <IISExpressAnonymousAuthentication />");
      sb.AppendLine("      <IISExpressWindowsAuthentication />");
      sb.AppendLine("      <IISExpressUseClassicPipelineMode />");
      sb.AppendLine("    </PropertyGroup>");
      sb.AppendLine("    <PropertyGroup Condition=\" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' \">");
      sb.AppendLine("      <DebugSymbols>true</DebugSymbols>");
      sb.AppendLine("      <DebugType>full</DebugType>");
      sb.AppendLine("      <DefineDebug>true</DefineDebug>");
      sb.AppendLine("      <DefineTrace>true</DefineTrace>");
      sb.AppendLine("      <OutputPath>bin\\</OutputPath>");
      sb.AppendLine("      <DocumentationFile>MVC5WebApp_VB.xml</DocumentationFile>");
      sb.AppendLine("      <NoWarn>42016,41999,42017,42018,42019,42032,42036,42020,42021,42022</NoWarn>");
      sb.AppendLine("    </PropertyGroup>");
      sb.AppendLine("    <PropertyGroup Condition=\" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' \">");
      sb.AppendLine("      <DebugType>pdbonly</DebugType>");
      sb.AppendLine("      <DefineDebug>false</DefineDebug>");
      sb.AppendLine("      <DefineTrace>true</DefineTrace>");
      sb.AppendLine("      <Optimize>true</Optimize>");
      sb.AppendLine("      <OutputPath>bin\\</OutputPath>");
      sb.AppendLine("      <DocumentationFile>MVC5WebApp_VB.xml</DocumentationFile>");
      sb.AppendLine("      <NoWarn>42016,41999,42017,42018,42019,42032,42036,42020,42021,42022</NoWarn>");
      sb.AppendLine("    </PropertyGroup>");
      sb.AppendLine("    <ItemGroup>");
      sb.AppendLine("      <Reference Include=\"System\" />");
      sb.AppendLine("      <Reference Include=\"System.Data\" />");
      sb.AppendLine("      <Reference Include=\"System.Drawing\" />");
      sb.AppendLine("      <Reference Include=\"System.Web.DynamicData\" />");
      sb.AppendLine("      <Reference Include=\"System.Web.Entity\" />");
      sb.AppendLine("      <Reference Include=\"System.Web.ApplicationServices\" />");
      sb.AppendLine("      <Reference Include=\"System.ComponentModel.DataAnnotations\" />");
      sb.AppendLine("      <Reference Include=\"System.Core\" />");
      sb.AppendLine("      <Reference Include=\"System.Data.DataSetExtensions\" />");
      sb.AppendLine("      <Reference Include=\"System.Xml.Linq\" />");
      sb.AppendLine("      <Reference Include=\"System.Web\" />");
      sb.AppendLine("      <Reference Include=\"System.Web.Extensions\" />");
      sb.AppendLine("      <Reference Include=\"System.Web.Abstractions\" />");
      sb.AppendLine("      <Reference Include=\"System.Web.Routing\" />");
      sb.AppendLine("      <Reference Include=\"System.Xml\" />");
      sb.AppendLine("      <Reference Include=\"System.Configuration\" />");
      sb.AppendLine("      <Reference Include=\"System.Web.Services\" />");
      sb.AppendLine("      <Reference Include=\"System.EnterpriseServices\" />");
      sb.AppendLine("      <Reference Include=\"Microsoft.Web.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL\">");
      sb.AppendLine("        <Private>True</Private>");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.Web.Infrastructure.1.0.0.0\\lib\\net40\\Microsoft.Web.Infrastructure.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"System.Net.Http\">");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"System.Net.Http.WebRequest\">");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"System.Web.Helpers, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL\">");
      sb.AppendLine("        <Private>True</Private>");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.AspNet.WebPages.3.0.0\\lib\\net45\\System.Web.Helpers.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"System.Web.Mvc, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL\">");
      sb.AppendLine("        <Private>True</Private>");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.AspNet.Mvc.5.0.0\\lib\\net45\\System.Web.Mvc.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"System.Web.Optimization\">");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.AspNet.Web.Optimization.1.1.1\\lib\\net40\\System.Web.Optimization.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"System.Web.Razor, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL\">");
      sb.AppendLine("        <Private>True</Private>");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.AspNet.Razor.3.0.0\\lib\\net45\\System.Web.Razor.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"System.Web.WebPages, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL\">");
      sb.AppendLine("        <Private>True</Private>");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.AspNet.WebPages.3.0.0\\lib\\net45\\System.Web.WebPages.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"System.Web.WebPages.Deployment, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL\">");
      sb.AppendLine("        <Private>True</Private>");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.AspNet.WebPages.3.0.0\\lib\\net45\\System.Web.WebPages.Deployment.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"System.Web.WebPages.Razor, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL\">");
      sb.AppendLine("        <Private>True</Private>");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.AspNet.WebPages.3.0.0\\lib\\net45\\System.Web.WebPages.Razor.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"Newtonsoft.Json\">");
      sb.AppendLine("        <Private>True</Private>");
      sb.AppendLine("        <HintPath>..\\packages\\Newtonsoft.Json.5.0.6\\lib\\net45\\Newtonsoft.Json.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"System.Net.Http.Formatting\">");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.AspNet.WebApi.Client.5.0.0\\lib\\net45\\System.Net.Http.Formatting.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"System.Web.Http\">");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.AspNet.WebApi.Core.5.0.0\\lib\\net45\\System.Web.Http.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"System.Web.Http.WebHost\">");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.AspNet.WebApi.WebHost.5.0.0\\lib\\net45\\System.Web.Http.WebHost.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"WebGrease\">");
      sb.AppendLine("        <Private>True</Private>");
      sb.AppendLine("        <HintPath>..\\packages\\WebGrease.1.5.2\\lib\\WebGrease.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"Antlr3.Runtime\">");
      sb.AppendLine("        <Private>True</Private>");
      sb.AppendLine("        <HintPath>..\\packages\\Antlr.3.4.1.9004\\lib\\Antlr3.Runtime.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("    </ItemGroup>");
      sb.AppendLine("    <ItemGroup>");
      sb.AppendLine("      <Reference Include=\"EntityFramework\">");
      sb.AppendLine("        <HintPath>..\\packages\\EntityFramework.6.0.0\\lib\\net45\\EntityFramework.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"EntityFramework.SqlServer\">");
      sb.AppendLine("        <HintPath>..\\packages\\EntityFramework.6.0.0\\lib\\net45\\EntityFramework.SqlServer.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"Microsoft.AspNet.Identity.Core\">");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.AspNet.Identity.Core.1.0.0\\lib\\net45\\Microsoft.AspNet.Identity.Core.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"Microsoft.AspNet.Identity.Owin\">");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.AspNet.Identity.Owin.1.0.0\\lib\\net45\\Microsoft.AspNet.Identity.Owin.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"Microsoft.AspNet.Identity.EntityFramework\">");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.AspNet.Identity.EntityFramework.1.0.0\\lib\\net45\\Microsoft.AspNet.Identity.EntityFramework.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"Owin\">");
      sb.AppendLine("        <HintPath>..\\packages\\Owin.1.0\\lib\\net40\\Owin.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"Microsoft.Owin\">");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.Owin.2.0.0\\lib\\net45\\Microsoft.Owin.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"Microsoft.Owin.Host.SystemWeb\">");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.Owin.Host.SystemWeb.2.0.0\\lib\\net45\\Microsoft.Owin.Host.SystemWeb.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"Microsoft.Owin.Security\">");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.Owin.Security.2.0.0\\lib\\net45\\Microsoft.Owin.Security.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"Microsoft.Owin.Security.Facebook\">");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.Owin.Security.Facebook.2.0.0\\lib\\net45\\Microsoft.Owin.Security.Facebook.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"Microsoft.Owin.Security.OAuth\">");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.Owin.Security.OAuth.2.0.0\\lib\\net45\\Microsoft.Owin.Security.OAuth.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"Microsoft.Owin.Security.Cookies\">");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.Owin.Security.Cookies.2.0.0\\lib\\net45\\Microsoft.Owin.Security.Cookies.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"Microsoft.Owin.Security.Google\">");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.Owin.Security.Google.2.0.0\\lib\\net45\\Microsoft.Owin.Security.Google.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"Microsoft.Owin.Security.Twitter\">");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.Owin.Security.Twitter.2.0.0\\lib\\net45\\Microsoft.Owin.Security.Twitter.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("      <Reference Include=\"Microsoft.Owin.Security.MicrosoftAccount\">");
      sb.AppendLine("        <HintPath>..\\packages\\Microsoft.Owin.Security.MicrosoftAccount.2.0.0\\lib\\net45\\Microsoft.Owin.Security.MicrosoftAccount.dll</HintPath>");
      sb.AppendLine("      </Reference>");
      sb.AppendLine("    </ItemGroup>");
      sb.AppendLine("    <ItemGroup>");
      sb.AppendLine("      <Import Include=\"Microsoft.VisualBasic\" />");
      sb.AppendLine("      <Import Include=\"System\" />");
      sb.AppendLine("      <Import Include=\"System.Collections\" />");
      sb.AppendLine("      <Import Include=\"System.Collections.Generic\" />");
      sb.AppendLine("      <Import Include=\"System.Data\" />");
      sb.AppendLine("      <Import Include=\"System.Linq\" />");
      sb.AppendLine("      <Import Include=\"System.Xml.Linq\" />");
      sb.AppendLine("      <Import Include=\"System.Diagnostics\" />");
      sb.AppendLine("      <Import Include=\"System.Collections.Specialized\" />");
      sb.AppendLine("      <Import Include=\"System.Configuration\" />");
      sb.AppendLine("      <Import Include=\"System.Text\" />");
      sb.AppendLine("      <Import Include=\"System.Text.RegularExpressions\" />");
      sb.AppendLine("      <Import Include=\"System.Web\" />");
      sb.AppendLine("      <Import Include=\"System.Web.Caching\" />");
      sb.AppendLine("      <Import Include=\"System.Web.Mvc\" />");
      sb.AppendLine("      <Import Include=\"System.Web.Mvc.Ajax\" />");
      sb.AppendLine("      <Import Include=\"System.Web.Mvc.Html\" />");
      sb.AppendLine("      <Import Include=\"System.Web.Routing\" />");
      sb.AppendLine("      <Import Include=\"System.Web.SessionState\" />");
      sb.AppendLine("      <Import Include=\"System.Web.Security\" />");
      sb.AppendLine("      <Import Include=\"System.Web.Profile\" />");
      sb.AppendLine("      <Import Include=\"System.Web.UI\" />");
      sb.AppendLine("      <Import Include=\"System.Web.UI.WebControls\" />");
      sb.AppendLine("      <Import Include=\"System.Web.UI.WebControls.WebParts\" />");
      sb.AppendLine("      <Import Include=\"System.Web.UI.HtmlControls\" />");
      sb.AppendLine("    </ItemGroup>");
    }
  }
}
