
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class HomeIndexPage
  {
    private string _fileExtension = ".cs";
    private Tables _selectedTables;
    private string _directory;
    private Language _language;
    private string _webAppName;
    private bool _isUseStoredProcedure;
    private IsCheckedView _isCheckedView;
    private ViewNames _viewNames;
    private bool _isUseWebApi;
    private DatabaseObjectToGenerateFrom _generateFrom;
    private ApplicationVersion _appVersion;
    private GeneratedSqlType _generatedSqlType;
    private int _spPrefixSuffixIndex;
    private string _storedProcPrefix;
    private string _storedProcSuffix;
    private string _databaseName;

    private HomeIndexPage()
    {
    }

    internal HomeIndexPage(Tables selectedTables, string directory, Language language, string weAppName, DatabaseObjectToGenerateFrom generateFrom, ApplicationVersion appVersion, bool isUseStoredProcedure, IsCheckedView isCheckedView, ViewNames viewNames, bool isUseWebApi, GeneratedSqlType generatedSqlType, int spPrefixSuffixIndex, string storedProcPrefix, string storedProcSuffix, string databaseName)
    {
      this._selectedTables = selectedTables;
      this._directory = directory + MyConstants.DirectoryRazorPage;
      this._language = language;
      this._webAppName = weAppName.Trim();
      this._generateFrom = generateFrom;
      this._appVersion = appVersion;
      this._isUseStoredProcedure = isUseStoredProcedure;
      this._isCheckedView = isCheckedView;
      this._viewNames = viewNames;
      this._isUseWebApi = isUseWebApi;
      this._generatedSqlType = generatedSqlType;
      this._spPrefixSuffixIndex = spPrefixSuffixIndex;
      this._storedProcPrefix = storedProcPrefix;
      this._storedProcSuffix = storedProcSuffix;
      this._databaseName = databaseName;
      if (language == Language.VB)
        this._fileExtension = ".vb";
      this.Generate();
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._directory + MyConstants.WordIndex + this._fileExtension + "html"))
      {
        StringBuilder stringBuilder = new StringBuilder();
        StringBuilder models = new StringBuilder();
        StringBuilder views1 = new StringBuilder();
        StringBuilder views2 = new StringBuilder();
        StringBuilder partialPages = new StringBuilder();
        StringBuilder partialPageModels = new StringBuilder();
        StringBuilder webApi = new StringBuilder();
        StringBuilder businessObjects = new StringBuilder();
        StringBuilder dataLayer = new StringBuilder();
        StringBuilder storedProcs = new StringBuilder();
        StringBuilder adHocSql = new StringBuilder();
        StringBuilder entityFramework = new StringBuilder();
        int num = 2;
        string str1 = string.Empty;
        string str2 = "and Models";
        string str3 = string.Empty;
        this.ListPages(views2);
        this.ListPageModels(views1);
        this.ListPartialPages(partialPages, partialPageModels);
        this.ListModels(models);
        this.ListBusinessObjects(businessObjects);
        this.ListDataLayer(dataLayer);
        this.ListStoredProcedures(storedProcs);
        this.ListAdHocSQL(adHocSql);
        this.ListEntityFramework(entityFramework);
        if (this._isUseWebApi)
        {
          num = 3;
          str2 = "Models, and Web API Controllers";
          this.ListWebApi(webApi);
        }
        if (this._language == Language.CSharp)
        {
          stringBuilder.AppendLine("@page");
          stringBuilder.AppendLine("@model " + this._webAppName + ".Pages.Index");
          stringBuilder.AppendLine("@{");
          stringBuilder.AppendLine("    ViewData[\"Title\"] = \"Generated ASP.NET Core Code\";");
          stringBuilder.AppendLine("}");
        }
        else
        {
          stringBuilder.AppendLine("@Code");
          stringBuilder.AppendLine("    ViewBag.Title = \"Generated ASP.NET Core Code\"");
          stringBuilder.AppendLine("End Code");
        }
        if (this._isUseStoredProcedure)
          str1 = "  In addition, stored procedures were also generated in your Microsoft SQL Server.";
        else
          str3 = this._generatedSqlType != GeneratedSqlType.EFCore ? "Ad Hoc/Dynamic SQL classes, " : "Entity Framework classes, ";
        if (MySettings.AppVersion == ApplicationVersion.Express)
        {
          str1 = string.Empty;
          str3 = string.Empty;
        }
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("<br/><br/>");
        stringBuilder.AppendLine("<b>Thank You for using " + MySettings.AppTitle + ". " + MySettings.AppTitle + " generated " + (object) num + " applications/projects for use with Visual Studio 2017 or later." + str1);
        stringBuilder.AppendLine("  Some of the main objects generated are listed below in their respective projects including: Pages, Page Models, Partial Pages, Partial Page Models, Business Objects, Data Layer, " + str3 + str2 + ".</b><br/><br/>");
        stringBuilder.AppendLine("<div style=\"background-color: black; color: white; padding: 14px; font-weight: bold;\">");
        stringBuilder.AppendLine("    1.  Web Application Project - ASP.NET Core Razor Web Application (.NET Core)");
        stringBuilder.AppendLine("</div>");
        stringBuilder.AppendLine("<br/>");
        stringBuilder.AppendLine("<div id=\"divViewsFolder\" style=\"color: White; background-color: #507CD1; font-weight: bold; font-size: 12px; padding: 8px;\">PAGES - Pages Folder (click to hide list)</div>");
        stringBuilder.AppendLine("<div id=\"divViewsFolderToToggle\">");
        stringBuilder.Append(views2.ToString());
        stringBuilder.Append(partialPages.ToString());
        stringBuilder.AppendLine("</div>");
        stringBuilder.AppendLine("<br/>");
        stringBuilder.AppendLine("<div id=\"divPageModelsFolder\" style=\"color: White; background-color: #507CD1; font-weight: bold; font-size: 12px; padding: 8px;\">PAGE MODELS - Pages Folder (click to show list)</div>");
        stringBuilder.AppendLine("<div id=\"divPageModelsFolderToToggle\">");
        stringBuilder.Append(views1.ToString());
        stringBuilder.AppendLine("</div>");
        stringBuilder.AppendLine("<br/>");
        stringBuilder.AppendLine("<div id=\"divPartialPagesFolder\" style=\"color: White; background-color: #507CD1; font-weight: bold; font-size: 12px; padding: 8px;\">PARTIAL PAGES - Pages\\Shared Folder (click to show list)</div>");
        stringBuilder.AppendLine("<div id=\"divPartialPagesFolderToToggle\">");
        stringBuilder.Append(partialPages.ToString());
        stringBuilder.AppendLine("</div>");
        stringBuilder.AppendLine("<br/>");
        stringBuilder.AppendLine("<div id=\"divPartialPageModelsFolder\" style=\"color: White; background-color: #507CD1; font-weight: bold; font-size: 12px; padding: 8px;\">PARTIAL PAGE MODELS - PartialModels Folder (click to show list)</div>");
        stringBuilder.AppendLine("<div id=\"divPartialPageModelsFolderToToggle\">");
        stringBuilder.Append(partialPageModels.ToString());
        stringBuilder.AppendLine("</div>");
        stringBuilder.AppendLine("<br/>");
        stringBuilder.AppendLine("<br/><br/>");
        stringBuilder.AppendLine("<div style=\"background-color: black; color: white; padding: 14px; font-weight: bold;\">");
        stringBuilder.AppendLine("    2.  Business Objects/Data Layer - Class Library Project (.NET Core)");
        stringBuilder.AppendLine("</div>");
        stringBuilder.AppendLine("<br/>");
        stringBuilder.AppendLine("<div id=\"divBusinessObjects\" style=\"color: White; background-color: #507CD1; font-weight: bold; font-size: 12px; padding: 8px;\">BUSINESS OBJECTS/MIDDLE TIER - BusinessObject Folder (click to show list)</div>");
        stringBuilder.AppendLine("<div id=\"divBusinessObjectsToToggle\">");
        stringBuilder.Append(businessObjects.ToString());
        stringBuilder.AppendLine("</div>");
        stringBuilder.AppendLine("<br/>");
        stringBuilder.AppendLine("<div id=\"divDataLayer\" style=\"color: White; background-color: #507CD1; font-weight: bold; font-size: 12px; padding: 8px;\">DATA LAYER/DAL/DATA TIER - DataLayer Folder (click to show list)</div>");
        stringBuilder.AppendLine("<div id=\"divDataLayerToToggle\">");
        stringBuilder.Append(dataLayer.ToString());
        stringBuilder.AppendLine("</div>");
        stringBuilder.AppendLine("<br/>");
        stringBuilder.AppendLine("<div id=\"divModelsFolder\" style=\"color: White; background-color: #507CD1; font-weight: bold; font-size: 12px; padding: 8px;\">MODELS - Models Folder (click to show list)</div>");
        stringBuilder.AppendLine("<div id=\"divModelsFolderToToggle\">");
        stringBuilder.Append(models.ToString());
        stringBuilder.AppendLine("</div>");
        stringBuilder.AppendLine("<br/>");
        if (this._generatedSqlType == GeneratedSqlType.AdHocSQL)
        {
          stringBuilder.AppendLine("<div id=\"divSqlFolder\" style=\"color: White; background-color: #507CD1; font-weight: bold; font-size: 12px; padding: 8px;\">AD HOC/DYNAMIC SQL (SQL Scripts in Code) - SQL Folder (click to show list)</div>");
          stringBuilder.AppendLine("<div id=\"divSqlFolderToToggle\">");
          stringBuilder.Append(adHocSql.ToString());
          stringBuilder.AppendLine("</div>");
          stringBuilder.AppendLine("<br/>");
        }
        else if (this._generatedSqlType == GeneratedSqlType.EFCore)
        {
          stringBuilder.AppendLine("<div id=\"divEfFolder\" style=\"color: White; background-color: #507CD1; font-weight: bold; font-size: 12px; padding: 8px;\">ENTITY FRAMEWORK - EF Folder (click to show list)</div>");
          stringBuilder.AppendLine("<div id=\"divEfFolderToToggle\">");
          stringBuilder.Append(entityFramework.ToString());
          stringBuilder.AppendLine("</div>");
          stringBuilder.AppendLine("<br/>");
        }
        stringBuilder.AppendLine("<br/><br/>");
        if (this._isUseWebApi)
        {
          stringBuilder.AppendLine("<div style=\"background-color: black; color: white; padding: 14px; font-weight: bold;\">");
          stringBuilder.AppendLine("    3.  Web API Project - ASP.NET Core Web API Application (.NET Core)");
          stringBuilder.AppendLine("</div>");
          stringBuilder.AppendLine("<br/>");
          stringBuilder.AppendLine("<div id=\"divPageModelsFolderWebApi\" style=\"color: White; background-color: #507CD1; font-weight: bold; font-size: 12px; padding: 8px;\">CONTROLLERS - Controllers Folder (click to show list)</div>");
          stringBuilder.AppendLine("<div id=\"divPageModelsFolderToToggleWebApi\">");
          stringBuilder.Append(webApi.ToString());
          stringBuilder.AppendLine("</div>");
          stringBuilder.AppendLine("<br/>");
          stringBuilder.AppendLine("<br/><br/>");
        }
        if (this._isUseStoredProcedure)
        {
          stringBuilder.AppendLine("<div style=\"background-color: black; color: white; padding: 14px; font-weight: bold;\">");
          stringBuilder.AppendLine("    " + (num + 1).ToString() + ".  Stored Procedures");
          stringBuilder.AppendLine("</div>");
          stringBuilder.AppendLine("<br/>");
          stringBuilder.AppendLine("<div id=\"divStoredProcedures\" style=\"color: White; background-color: #507CD1; font-weight: bold; font-size: 12px; padding: 8px;\">STORED PROCEDURES - In Microsoft SQL Server (click to show list)</div>");
          stringBuilder.AppendLine("<div id=\"divStoredProceduresToToggle\">");
          stringBuilder.Append(storedProcs.ToString());
          stringBuilder.AppendLine("</div>");
          stringBuilder.AppendLine("<br/>");
        }
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("<script src=\"~/js/jquery-1.12.2.min.js\"></script>");
        stringBuilder.AppendLine("<script>");
        stringBuilder.AppendLine("    $(document).ready(function () {");
        stringBuilder.AppendLine("        $(\"#divPageModelsFolderToToggle\").hide();");
        stringBuilder.AppendLine("        $(\"#divBusinessObjectsToToggle\").hide();");
        stringBuilder.AppendLine("        $(\"#divDataLayerToToggle\").hide();");
        stringBuilder.AppendLine("        $(\"#divPartialPagesFolderToToggle\").hide();");
        stringBuilder.AppendLine("        $(\"#divPartialPageModelsFolderToToggle\").hide();");
        stringBuilder.AppendLine("        $(\"#divModelsFolderToToggle\").hide();");
        if (this._isUseWebApi)
          stringBuilder.AppendLine("            $(\"#divPageModelsFolderToToggleWebApi\").hide();");
        if (this._generatedSqlType == GeneratedSqlType.EFCore)
          stringBuilder.AppendLine("            $(\"#divEfFolderToToggle\").hide();");
        else if (this._generatedSqlType == GeneratedSqlType.AdHocSQL)
          stringBuilder.AppendLine("            $(\"#divSqlFolderToToggle\").hide();");
        else if (this._isUseStoredProcedure)
          stringBuilder.AppendLine("            $(\"#divStoredProceduresToToggle\").hide();");
        stringBuilder.AppendLine("        $(\"#divViewsFolder\").click(function () {");
        stringBuilder.AppendLine("            $(\"#divViewsFolderToToggle\").toggle();");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            if ($(\"#divViewsFolder\").text() == 'PAGES - Pages Folder (click to hide list)')");
        stringBuilder.AppendLine("                $(\"#divViewsFolder\").text('PAGES - Pages Folder (click to show list)');");
        stringBuilder.AppendLine("            else");
        stringBuilder.AppendLine("                $(\"#divViewsFolder\").text('PAGES - Pages Folder (click to hide list)');");
        stringBuilder.AppendLine("        });");
        stringBuilder.AppendLine("        $(\"#divPageModelsFolder\").click(function () {");
        stringBuilder.AppendLine("            $(\"#divPageModelsFolderToToggle\").toggle();");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            if ($(\"#divPageModelsFolder\").text() == 'PAGE MODELS - Pages Folder (click to hide list)')");
        stringBuilder.AppendLine("                $(\"#divPageModelsFolder\").text('PAGE MODELS - Pages Folder (click to show list)');");
        stringBuilder.AppendLine("            else");
        stringBuilder.AppendLine("                $(\"#divPageModelsFolder\").text('PAGE MODELS - Pages Folder (click to hide list)');");
        stringBuilder.AppendLine("        });");
        stringBuilder.AppendLine("        $(\"#divPartialPagesFolder\").click(function () {");
        stringBuilder.AppendLine("            $(\"#divPartialPagesFolderToToggle\").toggle();");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            if ($(\"#divPartialPagesFolder\").text() == 'PARTIAL PAGES - Pages\\Shared Folder (click to hide list)')");
        stringBuilder.AppendLine("                $(\"#divPartialPagesFolder\").text('PARTIAL PAGES - Pages\\Shared Folder (click to show list)');");
        stringBuilder.AppendLine("            else");
        stringBuilder.AppendLine("                $(\"#divPartialPagesFolder\").text('PARTIAL PAGES - Pages\\Shared Folder (click to hide list)');");
        stringBuilder.AppendLine("        });");
        stringBuilder.AppendLine("        $(\"#divPartialPageModelsFolder\").click(function () {");
        stringBuilder.AppendLine("            $(\"#divPartialPageModelsFolderToToggle\").toggle();");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            if ($(\"#divPartialPageModelsFolder\").text() == 'PARTIAL PAGE MODELS - PartialModels Folder (click to hide list)')");
        stringBuilder.AppendLine("                $(\"#divPartialPageModelsFolder\").text('PARTIAL PAGE MODELS - PartialModels Folder (click to show list)');");
        stringBuilder.AppendLine("            else");
        stringBuilder.AppendLine("                $(\"#divPartialPageModelsFolder\").text('PARTIAL PAGE MODELS - PartialModels Folder (click to hide list)');");
        stringBuilder.AppendLine("        });");
        stringBuilder.AppendLine("        $(\"#divBusinessObjects\").click(function () {");
        stringBuilder.AppendLine("            $(\"#divBusinessObjectsToToggle\").toggle();");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            if ($(\"#divBusinessObjects\").text() == 'BUSINESS OBJECTS/MIDDLE TIER - BusinessObject Folder (click to hide list)')");
        stringBuilder.AppendLine("                $(\"#divBusinessObjects\").text('BUSINESS OBJECTS/MIDDLE TIER - BusinessObject Folder (click to show list)');");
        stringBuilder.AppendLine("            else");
        stringBuilder.AppendLine("                $(\"#divBusinessObjects\").text('BUSINESS OBJECTS/MIDDLE TIER - BusinessObject Folder (click to hide list)');");
        stringBuilder.AppendLine("        });");
        stringBuilder.AppendLine("        $(\"#divDataLayer\").click(function () {");
        stringBuilder.AppendLine("            $(\"#divDataLayerToToggle\").toggle();");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            if ($(\"#divDataLayer\").text() == 'DATA LAYER/DAL/DATA TIER - DataLayer Folder (click to hide list)')");
        stringBuilder.AppendLine("                $(\"#divDataLayer\").text('DATA LAYER/DAL/DATA TIER - DataLayer Folder (click to show list)');");
        stringBuilder.AppendLine("            else");
        stringBuilder.AppendLine("                $(\"#divDataLayer\").text('DATA LAYER/DAL/DATA TIER - DataLayer Folder (click to hide list)');");
        stringBuilder.AppendLine("        });");
        stringBuilder.AppendLine("        $(\"#divModelsFolder\").click(function () {");
        stringBuilder.AppendLine("            $(\"#divModelsFolderToToggle\").toggle();");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            if ($(\"#divModelsFolder\").text() == 'MODELS - Models Folder (click to hide list)')");
        stringBuilder.AppendLine("                $(\"#divModelsFolder\").text('MODELS - Models Folder (click to show list)');");
        stringBuilder.AppendLine("            else");
        stringBuilder.AppendLine("                $(\"#divModelsFolder\").text('MODELS - Models Folder (click to hide list)');");
        stringBuilder.AppendLine("        });");
        if (this._isUseWebApi)
        {
          stringBuilder.AppendLine("        $(\"#divPageModelsFolderWebApi\").click(function () {");
          stringBuilder.AppendLine("            $(\"#divPageModelsFolderToToggleWebApi\").toggle();");
          stringBuilder.AppendLine("");
          stringBuilder.AppendLine("            if ($(\"#divPageModelsFolderWebApi\").text() == 'CONTROLLERS - Controllers Folder (click to hide list)')");
          stringBuilder.AppendLine("                $(\"#divPageModelsFolderWebApi\").text('CONTROLLERS - Controllers Folder (click to show list)');");
          stringBuilder.AppendLine("            else");
          stringBuilder.AppendLine("                $(\"#divPageModelsFolderWebApi\").text('CONTROLLERS - Controllers Folder (click to hide list)');");
          stringBuilder.AppendLine("        });");
        }
        if (this._generatedSqlType == GeneratedSqlType.EFCore)
        {
          stringBuilder.AppendLine("        $(\"#divEfFolder\").click(function () {");
          stringBuilder.AppendLine("            $(\"#divEfFolderToToggle\").toggle();");
          stringBuilder.AppendLine("");
          stringBuilder.AppendLine("            if ($(\"#divEfFolder\").text() == 'ENTITY FRAMEWORK - EF Folder (click to hide list)')");
          stringBuilder.AppendLine("                $(\"#divEfFolder\").text('ENTITY FRAMEWORK - EF Folder (click to show list)');");
          stringBuilder.AppendLine("            else");
          stringBuilder.AppendLine("                $(\"#divEfFolder\").text('ENTITY FRAMEWORK - EF Folder (click to hide list)');");
          stringBuilder.AppendLine("        });");
        }
        else if (this._generatedSqlType == GeneratedSqlType.AdHocSQL)
        {
          stringBuilder.AppendLine("        $(\"#divSqlFolder\").click(function () {");
          stringBuilder.AppendLine("            $(\"#divSqlFolderToToggle\").toggle();");
          stringBuilder.AppendLine("");
          stringBuilder.AppendLine("            if ($(\"#divSqlFolder\").text() == 'AD HOC/DYNAMIC SQL (SQL Scripts in Code) - SQL Folder (click to hide list)')");
          stringBuilder.AppendLine("                $(\"#divSqlFolder\").text('AD HOC/DYNAMIC SQL (SQL Scripts in Code) - SQL Folder (click to show list)');");
          stringBuilder.AppendLine("            else");
          stringBuilder.AppendLine("                $(\"#divSqlFolder\").text('AD HOC/DYNAMIC SQL (SQL Scripts in Code) - SQL Folder (click to hide list)');");
          stringBuilder.AppendLine("        });");
        }
        else if (this._isUseStoredProcedure)
        {
          stringBuilder.AppendLine("        $(\"#divStoredProcedures\").click(function () {");
          stringBuilder.AppendLine("            $(\"#divStoredProceduresToToggle\").toggle();");
          stringBuilder.AppendLine("");
          stringBuilder.AppendLine("            if ($(\"#divStoredProcedures\").text() == 'STORED PROCEDURES - In Microsoft SQL Server (click to hide list)')");
          stringBuilder.AppendLine("                $(\"#divStoredProcedures\").text('STORED PROCEDURES - In Microsoft SQL Server (click to show list)');");
          stringBuilder.AppendLine("            else");
          stringBuilder.AppendLine("                $(\"#divStoredProcedures\").text('STORED PROCEDURES - In Microsoft SQL Server (click to hide list)');");
          stringBuilder.AppendLine("        });");
        }
        stringBuilder.AppendLine("    });");
        stringBuilder.AppendLine("</script>");
        streamWriter.Write(stringBuilder.ToString());
      }
    }

    private void ListPages(StringBuilder views)
    {
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
      StringBuilder stringBuilder12 = new StringBuilder();
      StringBuilder stringBuilder13 = new StringBuilder();
      StringBuilder stringBuilder14 = new StringBuilder();
      StringBuilder stringBuilder15 = new StringBuilder();
      StringBuilder stringBuilder16 = new StringBuilder();
      StringBuilder stringBuilder17 = new StringBuilder();
            if (this._isCheckedView.ListCrudRedirect)
        stringBuilder1.AppendLine("    <div style=\"font-weight: bolder; color: navy; padding: 24px 0px 12px 0px;\">LIST WITH ADD, EDIT REDIRECT, & DELETE PAGES</div>");
      if (this._isCheckedView.AddRecord)
        stringBuilder2.AppendLine("    <div style=\"font-weight: bolder; color: navy; padding: 24px 0px 12px 0px;\">ADD NEW RECORD PAGES</div>");
      if (this._isCheckedView.UpdateRecord)
        stringBuilder3.AppendLine("    <div style=\"font-weight: bolder; color: navy; padding: 24px 0px 12px 0px;\">UPDATE RECORD PAGES</div>");
      if (this._isCheckedView.RecordDetails)
        stringBuilder4.AppendLine("    <div style=\"font-weight: bolder; color: navy; padding: 24px 0px 12px 0px;\">RECORD DETAILS (READ-ONLY) PAGES</div>");
      if (this._isCheckedView.ListReadOnly)
        stringBuilder5.AppendLine("    <div style=\"font-weight: bolder; color: navy; padding: 24px 0px 12px 0px;\">LIST (READ-ONLY) PAGES</div>");
      if (this._isCheckedView.ListCrud)
        stringBuilder6.AppendLine("    <div style=\"font-weight: bolder; color: navy; padding: 24px 0px 12px 0px;\">LIST WITH ADD, EDIT, & DELETE (SAME PAGE) PAGES</div>");
      if (this._isCheckedView.ListGroupedBy)
        stringBuilder7.AppendLine("    <div style=\"font-weight: bolder; color: navy; padding: 24px 0px 12px 0px;\">LIST WITH GROUPING PAGES</div>");
      if (this._isCheckedView.ListTotals)
        stringBuilder8.AppendLine("    <div style=\"font-weight: bolder; color: navy; padding: 24px 0px 12px 0px;\">LIST WITH TOTALS PAGES</div>");
      if (this._isCheckedView.ListTotalsGroupedBy)
        stringBuilder9.AppendLine("    <div style=\"font-weight: bolder; color: navy; padding: 24px 0px 12px 0px;\">LIST WITH TOTALS AND GROUPING PAGES</div>");
      if (this._isCheckedView.ListSearch)
        stringBuilder10.AppendLine("    <div style=\"font-weight: bolder; color: navy; padding: 24px 0px 12px 0px;\">LIST WITH SEARCH PAGES</div>");
      if (this._isCheckedView.ListScrollLoad)
        stringBuilder11.AppendLine("    <div style=\"font-weight: bolder; color: navy; padding: 24px 0px 12px 0px;\">LIST SCROLL-LOAD DATA PAGES</div>");
      if (this._isCheckedView.ListInline)
        stringBuilder12.AppendLine("    <div style=\"font-weight: bolder; color: navy; padding: 24px 0px 12px 0px;\">LIST WITH INLINE ADD & EDIT PAGES</div>");
      if (this._isCheckedView.ListForeach)
        stringBuilder13.AppendLine("    <div style=\"font-weight: bolder; color: navy; padding: 24px 0px 12px 0px;\">LIST WITH MANUAL FOREACH PAGES</div>");
      if (this._isCheckedView.ListMasterDetailGrid)
        stringBuilder14.AppendLine("    <div style=\"font-weight: bolder; color: navy; padding: 24px 0px 12px 0px;\">LIST WITH MASTER DETAIL GRID PAGES</div>");
      if (this._isCheckedView.ListMasterDetailSubGrid)
        stringBuilder15.AppendLine("    <div style=\"font-weight: bolder; color: navy; padding: 24px 0px 12px 0px;\">LIST WITH MASTER DETAIL SUB GRID PAGES</div>");
      if (this._isCheckedView.Unbound)
         stringBuilder16.AppendLine("    <div style=\"font-weight: bolder; color: navy; padding: 24px 0px 12px 0px;\">UNBOUND PAGES</div>");
      if (this._isCheckedView.WorkflowAssignSteps)
         stringBuilder17.AppendLine("    <div style=\"font-weight: bolder; color: navy; padding: 24px 0px 12px 0px;\">ASSIGN USERS TO WORKFLOW STEPS DETAIL PAGE</div>");
  
      foreach (Table selectedTable in (List<Table>) this._selectedTables)
      {
        if (this._generateFrom == DatabaseObjectToGenerateFrom.Tables && !selectedTable.IsContainsPrimaryAndForeignKeyColumnsOnly && this._appVersion != ApplicationVersion.Express)
        {
          if (this._isCheckedView.ListCrudRedirect)
            stringBuilder1.AppendLine("    <a href=\"~/" + selectedTable.Name + "/" + selectedTable.Name + "_" + this._viewNames.ListCrudRedirect + "\">" + selectedTable.Name + "/" + selectedTable.Name + "_" + this._viewNames.ListCrudRedirect + "</a><br/>");
          if (this._isCheckedView.AddRecord)
            stringBuilder2.AppendLine("    <a href=\"~/" + selectedTable.Name + "/" + selectedTable.Name + "_" + this._viewNames.AddRecord + "?returnUrl=/Index\">" + selectedTable.Name + "/" + selectedTable.Name + "_" + this._viewNames.AddRecord + "</a><br/>");
          if (this._isCheckedView.UpdateRecord)
            stringBuilder3.AppendLine("    Pages\\" + selectedTable.Name + "\\" + selectedTable.Name + "_" + this._viewNames.UpdateRecord + "<br/>");
          if (this._isCheckedView.RecordDetails)
            stringBuilder4.AppendLine("    Pages\\" + selectedTable.Name + "\\" + selectedTable.Name + "_" + this._viewNames.RecordDetails + "<br/>");
          if (this._isCheckedView.ListReadOnly)
            stringBuilder5.AppendLine("    <a href=\"~/" + selectedTable.Name + "/" + selectedTable.Name + "_" + this._viewNames.ListReadOnly + "\">" + selectedTable.Name + "/" + selectedTable.Name + "_" + this._viewNames.ListReadOnly + "</a><br/>");
          if (this._isCheckedView.ListCrud)
            stringBuilder6.AppendLine("    <a href=\"~/" + selectedTable.Name + "/" + selectedTable.Name + "_" + this._viewNames.ListCrud + "\">" + selectedTable.Name + "/" + selectedTable.Name + "_" + this._viewNames.ListCrud + "</a><br/>");
          if (this._isCheckedView.ListTotals && selectedTable.IsContainsMoneyOrDecimalField)
            stringBuilder8.AppendLine("    <a href=\"~/" + selectedTable.Name + "/" + selectedTable.Name + "_" + this._viewNames.ListTotals + "\">" + selectedTable.Name + "/" + selectedTable.Name + "_" + this._viewNames.ListTotals + "</a><br/>");
          if (this._isCheckedView.ListSearch)
            stringBuilder10.AppendLine("    <a href=\"~/" + selectedTable.Name + "/" + selectedTable.Name + "_" + this._viewNames.ListSearch + "\">" + selectedTable.Name + "/" + selectedTable.Name + "_" + this._viewNames.ListSearch + "</a><br/>");
          if (this._isCheckedView.ListScrollLoad)
            stringBuilder11.AppendLine("    <a href=\"~/" + selectedTable.Name + "/" + selectedTable.Name + "_" + this._viewNames.ListScrollLoad + "\">" + selectedTable.Name + "/" + selectedTable.Name + "_" + this._viewNames.ListScrollLoad + "</a><br/>");
          if (this._isCheckedView.ListInline)
            stringBuilder12.AppendLine("    <a href=\"~/" + selectedTable.Name + "/" + selectedTable.Name + "_" + this._viewNames.ListInline + "\">" + selectedTable.Name + "/" + selectedTable.Name + "_" + this._viewNames.ListInline + "</a><br/>");
          if (this._isCheckedView.ListForeach)
            stringBuilder13.AppendLine("    <a href=\"~/" + selectedTable.Name + "/" + selectedTable.Name + "_" + this._viewNames.ListForeach + "\">" + selectedTable.Name + "/" + selectedTable.Name + "_" + this._viewNames.ListForeach + "</a><br/>");
          if (this._isCheckedView.Unbound)
            stringBuilder16.AppendLine("    <a href=\"~/" + selectedTable.Name + "/" + selectedTable.Name + "_" + this._viewNames.Unbound + "\">" + selectedTable.Name + "/" + selectedTable.Name + "_" + this._viewNames.Unbound + "</a><br/>");
          if (this._isCheckedView.WorkflowAssignSteps && selectedTable.Name=="WorkflowStepsMaster")
             stringBuilder17.AppendLine("    <a href=\"~/" + selectedTable.Name + "/" + selectedTable.Name + "_AssignUsers" + "\">" + selectedTable.Name + "/" + selectedTable.Name + "_" + this._viewNames.WorkflowAssignSteps + "</a><br/>");
          foreach (Column column in (List<Column>) selectedTable.Columns)
          {
            if (column.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK)
            {
              if (this._isCheckedView.ListGroupedBy)
                stringBuilder7.AppendLine("    <a href=\"~/" + selectedTable.Name + "/" + selectedTable.Name + "_" + this._viewNames.ListGroupedBy + column.Name + "\">" + selectedTable.Name + "/" + selectedTable.Name + "_" + this._viewNames.ListGroupedBy + column.Name + "</a><br/>");
              if (this._isCheckedView.ListTotalsGroupedBy && selectedTable.IsContainsMoneyOrDecimalField)
                stringBuilder9.AppendLine("    <a href=\"~/" + selectedTable.Name + "/" + selectedTable.Name + "_" + this._viewNames.ListTotalsGroupedBy + column.Name + "\">" + selectedTable.Name + "/" + selectedTable.Name + "_" + this._viewNames.ListTotalsGroupedBy + column.Name + "</a><br/>");
              if (this._isCheckedView.ListMasterDetailGrid)
                stringBuilder14.AppendLine("    <a href=\"~/" + selectedTable.Name + "/" + selectedTable.Name + "_" + this._viewNames.ListMasterDetailGrid + column.Name + "\">" + selectedTable.Name + "/" + selectedTable.Name + "_" + this._viewNames.ListMasterDetailGrid + column.Name + "</a><br/>");
              if (this._isCheckedView.ListMasterDetailSubGrid)
                stringBuilder15.AppendLine("    <a href=\"~/" + selectedTable.Name + "/" + selectedTable.Name + "_" + this._viewNames.ListMasterDetailSubGrid + column.Name + "\">" + selectedTable.Name + "/" + selectedTable.Name + "_" + this._viewNames.ListMasterDetailSubGrid + column.Name + "</a><br/>");
            }
          }
        }
        if (this._generateFrom == DatabaseObjectToGenerateFrom.Views && !selectedTable.IsContainsPrimaryAndForeignKeyColumnsOnly && this._appVersion != ApplicationVersion.Express)
          stringBuilder5.AppendLine("    <a href=\"~/" + selectedTable.Name + "/" + selectedTable.Name + "_" + this._viewNames.ListReadOnly + "\">" + selectedTable.Name + "/" + selectedTable.Name + "_" + this._viewNames.ListReadOnly + "</a><br/>");
        if (this._appVersion == ApplicationVersion.Express && !selectedTable.IsContainsPrimaryAndForeignKeyColumnsOnly)
          stringBuilder16.AppendLine("    <a href=\"~/" + selectedTable.Name + "/" + selectedTable.Name + "_" + this._viewNames.Unbound + "\">" + selectedTable.Name + "/" + selectedTable.Name + "_" + this._viewNames.Unbound + "</a><br/>");
      }
      views.Append((object) stringBuilder1);
      views.Append((object) stringBuilder2);
      views.Append((object) stringBuilder3);
      views.Append((object) stringBuilder4);
      views.Append((object) stringBuilder5);
      views.Append((object) stringBuilder6);
      views.Append((object) stringBuilder7);
      views.Append((object) stringBuilder8);
      views.Append((object) stringBuilder9);
      views.Append((object) stringBuilder10);
      views.Append((object) stringBuilder11);
      views.Append((object) stringBuilder12);
      views.Append((object) stringBuilder13);
      views.Append((object) stringBuilder14);
      views.Append((object) stringBuilder15);
      views.Append((object) stringBuilder16);
      views.Append((object)stringBuilder17);
        }

    private void ListPageModels(StringBuilder views)
    {
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
      StringBuilder stringBuilder12 = new StringBuilder();
      StringBuilder stringBuilder13 = new StringBuilder();
      StringBuilder stringBuilder14 = new StringBuilder();
      StringBuilder stringBuilder15 = new StringBuilder();
      StringBuilder stringBuilder16 = new StringBuilder();
      StringBuilder stringBuilder17 = new StringBuilder();
      if (this._isCheckedView.ListCrudRedirect)
        stringBuilder1.AppendLine("    <div style=\"font-weight: bolder; color: navy; padding: 24px 0px 12px 0px;\">LIST WITH ADD, EDIT REDIRECT, & DELETE PAGE MODELS</div>");
      if (this._isCheckedView.AddRecord)
        stringBuilder2.AppendLine("    <div style=\"font-weight: bolder; color: navy; padding: 24px 0px 12px 0px;\">ADD NEW RECORD PAGE MODELS</div>");
      if (this._isCheckedView.UpdateRecord)
        stringBuilder3.AppendLine("    <div style=\"font-weight: bolder; color: navy; padding: 24px 0px 12px 0px;\">UPDATE RECORD PAGE MODELS</div>");
      if (this._isCheckedView.RecordDetails)
        stringBuilder4.AppendLine("    <div style=\"font-weight: bolder; color: navy; padding: 24px 0px 12px 0px;\">RECORD DETAILS (READ-ONLY) PAGE MODELS</div>");
      if (this._isCheckedView.ListReadOnly)
        stringBuilder5.AppendLine("    <div style=\"font-weight: bolder; color: navy; padding: 24px 0px 12px 0px;\">LIST (READ-ONLY) PAGE MODELS</div>");
      if (this._isCheckedView.ListCrud)
        stringBuilder6.AppendLine("    <div style=\"font-weight: bolder; color: navy; padding: 24px 0px 12px 0px;\">LIST WITH ADD, EDIT, & DELETE (SAME PAGE) PAGE MODELS</div>");
      if (this._isCheckedView.ListGroupedBy)
        stringBuilder7.AppendLine("    <div style=\"font-weight: bolder; color: navy; padding: 24px 0px 12px 0px;\">LIST WITH GROUPING PAGE MODELS</div>");
      if (this._isCheckedView.ListTotals)
        stringBuilder8.AppendLine("    <div style=\"font-weight: bolder; color: navy; padding: 24px 0px 12px 0px;\">LIST WITH TOTALS PAGE MODELS</div>");
      if (this._isCheckedView.ListTotalsGroupedBy)
        stringBuilder9.AppendLine("    <div style=\"font-weight: bolder; color: navy; padding: 24px 0px 12px 0px;\">LIST WITH TOTALS AND GROUPING PAGE MODELS</div>");
      if (this._isCheckedView.ListSearch)
        stringBuilder10.AppendLine("    <div style=\"font-weight: bolder; color: navy; padding: 24px 0px 12px 0px;\">LIST WITH SEARCH PAGE MODELS</div>");
      if (this._isCheckedView.ListScrollLoad)
        stringBuilder11.AppendLine("    <div style=\"font-weight: bolder; color: navy; padding: 24px 0px 12px 0px;\">LIST SCROLL-LOAD DATA PAGE MODELS</div>");
      if (this._isCheckedView.ListInline)
        stringBuilder12.AppendLine("    <div style=\"font-weight: bolder; color: navy; padding: 24px 0px 12px 0px;\">LIST WITH INLINE ADD & EDIT PAGE MODELS</div>");
      if (this._isCheckedView.ListForeach)
        stringBuilder13.AppendLine("    <div style=\"font-weight: bolder; color: navy; padding: 24px 0px 12px 0px;\">LIST WITH MANUAL FOREACH PAGE MODELS</div>");
      if (this._isCheckedView.ListMasterDetailGrid)
        stringBuilder14.AppendLine("    <div style=\"font-weight: bolder; color: navy; padding: 24px 0px 12px 0px;\">LIST WITH MASTER DETAIL GRID PAGE MODELS</div>");
      if (this._isCheckedView.ListMasterDetailSubGrid)
        stringBuilder15.AppendLine("    <div style=\"font-weight: bolder; color: navy; padding: 24px 0px 12px 0px;\">LIST WITH MASTER DETAIL SUB GRID PAGE MODELS</div>");
      if (this._isCheckedView.Unbound)
        stringBuilder16.AppendLine("    <div style=\"font-weight: bolder; color: navy; padding: 24px 0px 12px 0px;\">UNBOUND PAGE MODELS</div>");
     if (this._isCheckedView.WorkflowAssignSteps )
        stringBuilder17.AppendLine("    <div style=\"font-weight: bolder; color: navy; padding: 24px 0px 12px 0px;\">ASSIGN USERS TO WORKFLOWSTEPS PAGE MODELS</div>");
            foreach (Table selectedTable in (List<Table>) this._selectedTables)
      {
        if (this._generateFrom == DatabaseObjectToGenerateFrom.Tables && !selectedTable.IsContainsPrimaryAndForeignKeyColumnsOnly && this._appVersion != ApplicationVersion.Express)
        {
          if (this._isCheckedView.ListCrudRedirect)
            stringBuilder1.AppendLine("    Pages\\" + selectedTable.Name + "\\" + selectedTable.Name + "_ " + this._viewNames.ListCrudRedirect + ".cshtml.cs<br/>");
          if (this._isCheckedView.AddRecord)
            stringBuilder2.AppendLine("    Pages\\" + selectedTable.Name + "\\" + selectedTable.Name + "_" + this._viewNames.AddRecord + ".cshtml.cs<br/>");
          if (this._isCheckedView.UpdateRecord)
            stringBuilder3.AppendLine("    Pages\\" + selectedTable.Name + "\\" + selectedTable.Name + "_ " + this._viewNames.UpdateRecord + "<br/>");
          if (this._isCheckedView.RecordDetails)
            stringBuilder4.AppendLine("    Pages\\" + selectedTable.Name + "\\" + selectedTable.Name + "_ " + this._viewNames.RecordDetails + "<br/>");
          if (this._isCheckedView.ListReadOnly)
            stringBuilder5.AppendLine("    Pages\\" + selectedTable.Name + "\\" + selectedTable.Name + "_" + this._viewNames.ListReadOnly + ".cshtml.cs<br/>");
          if (this._isCheckedView.ListCrud)
            stringBuilder6.AppendLine("    Pages\\" + selectedTable.Name + "\\" + selectedTable.Name + "_" + this._viewNames.ListCrud + ".cshtml.cs<br/>");
          if (this._isCheckedView.ListTotals && selectedTable.IsContainsMoneyOrDecimalField)
            stringBuilder8.AppendLine("    Pages\\" + selectedTable.Name + "\\" + selectedTable.Name + "_" + this._viewNames.ListTotals + ".cshtml.cs<br/>");
          if (this._isCheckedView.ListSearch)
            stringBuilder10.AppendLine("    Pages\\" + selectedTable.Name + "\\" + selectedTable.Name + "_" + this._viewNames.ListSearch + ".cshtml.cs<br/>");
          if (this._isCheckedView.ListScrollLoad)
            stringBuilder11.AppendLine("    Pages\\" + selectedTable.Name + "\\" + selectedTable.Name + "_" + this._viewNames.ListScrollLoad + ".cshtml.cs<br/>");
          if (this._isCheckedView.ListInline)
            stringBuilder12.AppendLine("    Pages\\" + selectedTable.Name + "\\" + selectedTable.Name + "_" + this._viewNames.ListInline + ".cshtml.cs<br/>");
          if (this._isCheckedView.ListForeach)
            stringBuilder13.AppendLine("    Pages\\" + selectedTable.Name + "\\" + selectedTable.Name + "_" + this._viewNames.ListForeach + ".cshtml.cs<br/>");
          if (this._isCheckedView.Unbound)
            stringBuilder16.AppendLine("    Pages\\" + selectedTable.Name + "\\" + selectedTable.Name + "_" + this._viewNames.Unbound + ".cshtml.cs<br/>");
          if (this._isCheckedView.WorkflowAssignSteps && selectedTable.Name == "WorkflowStepsMaster")
             stringBuilder17.AppendLine("    Pages\\" + selectedTable.Name + "\\" + selectedTable.Name + "_" + this._viewNames.WorkflowAssignSteps + ".cshtml.cs<br/>");
           foreach (Column column in (List<Column>) selectedTable.Columns)
          {
            if (column.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK)
            {
              if (this._isCheckedView.ListGroupedBy)
                stringBuilder7.AppendLine("    Pages\\" + selectedTable.Name + "\\" + selectedTable.Name + "_" + this._viewNames.ListGroupedBy + column.Name + ".cshtml.cs<br/>");
              if (this._isCheckedView.ListTotalsGroupedBy && selectedTable.IsContainsMoneyOrDecimalField)
                stringBuilder9.AppendLine("    Pages\\" + selectedTable.Name + "\\" + selectedTable.Name + "_" + this._viewNames.ListTotalsGroupedBy + column.Name + ".cshtml.cs<br/>");
              if (this._isCheckedView.ListMasterDetailGrid)
                stringBuilder14.AppendLine("    Pages\\" + selectedTable.Name + "\\" + selectedTable.Name + "_" + this._viewNames.ListMasterDetailGrid + column.Name + ".cshtml.cs<br/>");
              if (this._isCheckedView.ListMasterDetailSubGrid)
                stringBuilder15.AppendLine("    Pages\\" + selectedTable.Name + "\\" + selectedTable.Name + "_" + this._viewNames.ListMasterDetailSubGrid + column.Name + ".cshtml.cs<br/>");
          }
          }
        }
        if (this._generateFrom == DatabaseObjectToGenerateFrom.Views && !selectedTable.IsContainsPrimaryAndForeignKeyColumnsOnly && this._appVersion != ApplicationVersion.Express)
          stringBuilder5.AppendLine("    Pages\\" + selectedTable.Name + "\\" + selectedTable.Name + "_" + this._viewNames.ListReadOnly + ".cshtml.cs<br/>");
        if (this._appVersion == ApplicationVersion.Express && !selectedTable.IsContainsPrimaryAndForeignKeyColumnsOnly)
          stringBuilder16.AppendLine("    Pages\\" + selectedTable.Name + "\\" + selectedTable.Name + "_" + this._viewNames.Unbound + ".cshtml.cs<br/>");
      }
      views.Append((object) stringBuilder1);
      views.Append((object) stringBuilder2);
      views.Append((object) stringBuilder3);
      views.Append((object) stringBuilder4);
      views.Append((object) stringBuilder5);
      views.Append((object) stringBuilder6);
      views.Append((object) stringBuilder7);
      views.Append((object) stringBuilder8);
      views.Append((object) stringBuilder9);
      views.Append((object) stringBuilder10);
      views.Append((object) stringBuilder11);
      views.Append((object) stringBuilder12);
      views.Append((object) stringBuilder13);
      views.Append((object) stringBuilder14);
      views.Append((object) stringBuilder15);
      views.Append((object) stringBuilder16);
      views.Append((object)stringBuilder17);
    }

    private void ListPartialPages(StringBuilder partialPages, StringBuilder partialPageModels)
    {
      if (!this._isCheckedView.AddRecord && !this._isCheckedView.UpdateRecord)
        return;
      partialPages.AppendLine("    <br/>");
      partialPageModels.AppendLine("    <br/>");
      foreach (Table selectedTable in (List<Table>) this._selectedTables)
      {
        if (this._generateFrom == DatabaseObjectToGenerateFrom.Tables && !selectedTable.IsContainsPrimaryAndForeignKeyColumnsOnly && this._appVersion != ApplicationVersion.Express)
        {
          partialPages.AppendLine("    _AddEdit" + selectedTable.Name + "Partial" + this._fileExtension + "html<br/>");
          partialPageModels.AppendLine("    AddEdit" + selectedTable.Name + "PartialModel" + this._fileExtension + "<br/>");
        }
      }
    }

    private void ListModels(StringBuilder models)
    {
      models.AppendLine("    <br/>");
      foreach (Table selectedTable in (List<Table>) this._selectedTables)
        models.AppendLine("    " + selectedTable.Name + MyConstants.WordModel + this._fileExtension + "<br/>");
      models.AppendLine("    <br/><br/>");
      models.AppendLine("    <div style=\"color: White; background-color: #507CD1; font-weight: bold; font-size: 12px; padding: 8px;\">MODELS (Base Class) - Models\\Base Folder</div>");
      models.AppendLine("    <br/>");
      foreach (Table selectedTable in (List<Table>) this._selectedTables)
        models.AppendLine("    " + selectedTable.Name + MyConstants.WordModelBase + this._fileExtension + "<br/>");
      models.AppendLine("    <br/><br/>");
    }

    private void ListBusinessObjects(StringBuilder businessObjects)
    {
      businessObjects.AppendLine("    <br/>");
      foreach (Table selectedTable in (List<Table>) this._selectedTables)
        businessObjects.AppendLine("    " + selectedTable.Name + this._fileExtension + "<br/>");
      businessObjects.AppendLine("    <br/><br/>");
      businessObjects.AppendLine("    <div style=\"color: White; background-color: #507CD1; font-weight: bold; font-size: 12px; padding: 8px;\">BUSINESS OBJECTS (Base Class) - BusinessObject\\Base Folder</div>");
      businessObjects.AppendLine("    <br/>");
      foreach (Table selectedTable in (List<Table>) this._selectedTables)
        businessObjects.AppendLine("    " + selectedTable.Name + "Base" + this._fileExtension + "<br/>");
      businessObjects.AppendLine("    <br/><br/>");
    }

    private void ListDataLayer(StringBuilder dataLayer)
    {
      dataLayer.AppendLine("    <br/>");
      foreach (Table selectedTable in (List<Table>) this._selectedTables)
        dataLayer.AppendLine("    " + selectedTable.Name + "DataLayer" + this._fileExtension + "<br/>");
      dataLayer.AppendLine("    <br/><br/>");
      dataLayer.AppendLine("    <div style=\"color: White; background-color: #507CD1; font-weight: bold; font-size: 12px; padding: 8px;\">DATA LAYER (Base Class) - DataLayer\\Base Folder</div>");
      dataLayer.AppendLine("    <br/>");
      foreach (Table selectedTable in (List<Table>) this._selectedTables)
        dataLayer.AppendLine("    " + selectedTable.Name + "DataLayerBase" + this._fileExtension + "<br/>");
      dataLayer.AppendLine("    <br/><br/>");
    }

    private void ListWebApi(StringBuilder webApi)
    {
      webApi.AppendLine("    <br/>");
      foreach (Table selectedTable in (List<Table>) this._selectedTables)
      {
        if (!selectedTable.IsContainsPrimaryAndForeignKeyColumnsOnly)
          webApi.AppendLine("    " + selectedTable.Name + MyConstants.WordApi + MyConstants.WordController + this._fileExtension + "<br/>");
      }
      webApi.AppendLine("    <br/><br/>");
      webApi.AppendLine("    <div style=\"color: White; background-color: #507CD1; font-weight: bold; font-size: 12px; padding: 8px;\">CONTROLLERS (Base Class) - Controllers\\Base Folder</div>");
      webApi.AppendLine("    <br/>");
      foreach (Table selectedTable in (List<Table>) this._selectedTables)
      {
        if (!selectedTable.IsContainsPrimaryAndForeignKeyColumnsOnly)
          webApi.AppendLine("    " + selectedTable.Name + MyConstants.WordApi + MyConstants.WordControllerBase + this._fileExtension + "<br/>");
      }
      webApi.AppendLine("    <br/><br/>");
    }

    private void ListStoredProcedures(StringBuilder storedProcs)
    {
      storedProcs.AppendLine("<br/>");
      foreach (Table selectedTable in (List<Table>) this._selectedTables)
      {
        if (this._generateFrom == DatabaseObjectToGenerateFrom.Tables)
        {
          this.GenerateStoredProcLinks(storedProcs, selectedTable, StoredProcName.Delete);
          this.GenerateStoredProcLinks(storedProcs, selectedTable, StoredProcName.GetRecordCount);
          this.GenerateStoredProcLinks(storedProcs, selectedTable, StoredProcName.GetRecordCountBy);
          this.GenerateStoredProcLinks(storedProcs, selectedTable, StoredProcName.GetRecordCountWhereDynamic);
          this.GenerateStoredProcLinks(storedProcs, selectedTable, StoredProcName.Insert);
          this.GenerateStoredProcLinks(storedProcs, selectedTable, StoredProcName.SelectAll);
          this.GenerateStoredProcLinks(storedProcs, selectedTable, StoredProcName.SelectAllBy);
          this.GenerateStoredProcLinks(storedProcs, selectedTable, StoredProcName.SelectAllWhereDynamic);
          this.GenerateStoredProcLinks(storedProcs, selectedTable, StoredProcName.SelectByPrimaryKey);
          this.GenerateStoredProcLinks(storedProcs, selectedTable, StoredProcName.SelectDropDownListData);
          this.GenerateStoredProcLinks(storedProcs, selectedTable, StoredProcName.SelectSkipAndTake);
          this.GenerateStoredProcLinks(storedProcs, selectedTable, StoredProcName.SelectSkipAndTakeBy);
          if (selectedTable.IsContainsMoneyOrDecimalField)
            this.GenerateStoredProcLinks(storedProcs, selectedTable, StoredProcName.SelectTotals);
          this.GenerateStoredProcLinks(storedProcs, selectedTable, StoredProcName.SelectSkipAndTakeWhereDynamic);
          this.GenerateStoredProcLinks(storedProcs, selectedTable, StoredProcName.Update);
        }
        else
          this.GenerateStoredProcLinks(storedProcs, selectedTable, StoredProcName.SelectAll);
      }
    }

    private void GenerateStoredProcLinks(StringBuilder sb, Table table, StoredProcName storedProc)
    {
      if (storedProc == StoredProcName.SelectAllBy || storedProc == StoredProcName.SelectSkipAndTakeBy || storedProc == StoredProcName.GetRecordCountBy)
      {
        StringBuilder stringBuilder = new StringBuilder();
        int num = 0;
        foreach (Column column in (List<Column>) table.Columns)
        {
          if (column.IsForeignKey)
          {
            string foreignKeyTableName = column.ForeignKeyTableName;
            if (stringBuilder.ToString().Contains(column.ForeignKeyTableName + ","))
            {
              ++num;
              string str = foreignKeyTableName + num.ToString();
            }
            string storedProcNameBy = Functions.GetStoredProcNameBy(table, column, this._spPrefixSuffixIndex, this._storedProcPrefix, this._storedProcSuffix, storedProc);
            sb.AppendLine("            " + storedProcNameBy + "<br/>");
          }
        }
      }
      else
      {
        string storedProcName = Functions.GetStoredProcName(table, storedProc, this._spPrefixSuffixIndex, this._storedProcPrefix, this._storedProcSuffix);
        if (storedProc == StoredProcName.SelectByPrimaryKey || storedProc == StoredProcName.Update || storedProc == StoredProcName.SelectDropDownListData)
        {
          if (!table.IsContainsPrimaryAndForeignKeyColumnsOnly)
          {
            if (storedProc == StoredProcName.Update)
              sb.AppendLine("            " + storedProcName + "<br/>");
            else
              sb.AppendLine("            " + storedProcName + "<br/>");
          }
          else
          {
            if (storedProc != StoredProcName.Update)
              return;
            sb.Replace("<br/>", "<br/><br/><br/>", sb.ToString().LastIndexOf("<br/>"), 5);
          }
        }
        else
          sb.AppendLine("                    " + storedProcName + "<br/>");
      }
    }

    private void ListAdHocSQL(StringBuilder adHocSql)
    {
      if (this._generatedSqlType != GeneratedSqlType.AdHocSQL)
        return;
      adHocSql.AppendLine("    <br/>");
      foreach (Table selectedTable in (List<Table>) this._selectedTables)
        adHocSql.AppendLine("    " + selectedTable.Name + "Sql" + this._fileExtension + "<br/>");
    }

    private void ListEntityFramework(StringBuilder entityFramework)
    {
      if (this._generatedSqlType != GeneratedSqlType.EFCore)
        return;
      entityFramework.AppendLine("    <br/>");
      entityFramework.AppendLine("    " + this._databaseName + "Context" + this._fileExtension + "<br/><br/>");
      foreach (Table selectedTable in (List<Table>) this._selectedTables)
        entityFramework.AppendLine("    " + selectedTable.Name + this._fileExtension + "<br/>");
    }
  }
}
