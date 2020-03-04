
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
    internal class RazorPage
    {
        private const Language _language = Language.CSharp;
        private const string _fileExtension = ".cs";
        private MVCGridViewType _viewType;
        private Table _table;
        private Tables _selectedTables;
        private string _directory;
        private string _webAppName;
        private string _apiName;
        private ViewNames _viewNames;
        private string _viewName;
        private ApplicationVersion _appVersion;
        private string _pageTitle;
        private StringBuilder _commaDelimitedColNames;
        private StringBuilder _colModelNames;
        private Table _currentFktable;
        private Column _currentColumn;
        private bool _isUseWebApi;
        private bool _isUseAuditLogging;
        private string _webApiBaseAddress;
        private string _jqueryUITheme;
        private GeneratedSqlType _generatedSqlType;

        internal RazorPage(MVCGridViewType listViewType, Table table, Tables selectedTables, string webAppRootDirectory, string webAppName, string apiName, ViewNames viewNames, string viewName, string jqueryUITheme, GeneratedSqlType generatedSqlType, ApplicationVersion appVersion = ApplicationVersion.ProfessionalPlus, bool isUseWebApi = false, bool isUseAuditLogging = false, string webApiBaseAddress = "")
        {
            this._viewType = listViewType;
            this._table = table;
            this._selectedTables = selectedTables;
            this._directory = webAppRootDirectory + "Pages\\" + this._table.Name + "\\";
            this._webAppName = webAppName;
            this._apiName = apiName;
            this._viewNames = viewNames;
            this._viewName = viewName.Trim();
            this._appVersion = appVersion;
            this._pageTitle = "List of " + Functions.GetNameWithSpaces(this._table.Name);
            this._commaDelimitedColNames = new StringBuilder();
            this._colModelNames = new StringBuilder();
            this._isUseWebApi = isUseWebApi;
            this._webApiBaseAddress = webApiBaseAddress;
            this._jqueryUITheme = jqueryUITheme;
            this._generatedSqlType = generatedSqlType;
            this._isUseAuditLogging = isUseAuditLogging;
            foreach (Column column in this._table.Columns)
            {
                if (this._viewType == MVCGridViewType.ToolTip && column.IsForeignKey)
                {
                    if (column.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK)
                        this._commaDelimitedColNames.Append("'','" + column.NameWithSpaces + "',");
                    else
                        this._commaDelimitedColNames.Append("'" + column.NameWithSpaces + "',");
                }
                else
                    this._commaDelimitedColNames.Append("'" + column.NameWithSpaces + "',");
            }
            if (this._viewType == MVCGridViewType.GroupedBy || this._viewType == MVCGridViewType.GroupedByWithTotals || (this._viewType == MVCGridViewType.MasterDetailGrid || this._viewType == MVCGridViewType.MasterDetailSubGrid))
                this.GenerateGroupedBy();
            else if (this._viewType == MVCGridViewType.Foreach)
                this.GenerateForeach();
            else
                this.Generate(null, null);
        }

        private void Generate(Table currentMasterTable = null, Table foreignKeyDetailTable = null)
        {
            string actionName = this.GetActionName(null);
            string viewName = Functions.GetViewName(this._viewType, this._viewName, this._currentColumn);
            int num = 0;
            int masterDetailPassCtr = 0;
            using (StreamWriter streamWriter = new StreamWriter(this._directory + this._table.Name + "_" + viewName + ".cshtml"))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("@page");
                sb.AppendLine("@model " + this._webAppName + ".Pages." + this._table.Name + "_" + viewName + "Model");
                if (this._viewType == MVCGridViewType.Inline)
                    sb.AppendLine("@using System.Text.RegularExpressions;");
                sb.AppendLine("@{");
                sb.AppendLine("    ViewData[\"Title\"] = \"" + this._pageTitle + "\";");
                if (this._viewType == MVCGridViewType.Search)
                    this.WriteSearchPart(sb, MVCGridViewPart.Code);
                else if (this._viewType == MVCGridViewType.Inline)
                    this.WriteInlinePart(sb, MVCGridViewPart.Code);
                sb.AppendLine("}");
                sb.AppendLine("");
                sb.AppendLine("@section AdditionalCss {");
                sb.AppendLine("    <link rel=\"stylesheet\" href=\"~/css/ui.jqgrid.min.css\" />");
                sb.AppendLine("}");
                sb.AppendLine("");
                sb.AppendLine("@section AdditionalJavaScript {");
                sb.AppendLine("    <script src=\"~/js/jqgrid-i18n/grid.locale-en.min.js\" asp-append-version=\"true\"></script>");
                sb.AppendLine("    <script src=\"~/js/jquery-jqgrid-4.13.2.min.js\" asp-append-version=\"true\"></script>");
                if (this._viewType == MVCGridViewType.Redirect)
                    this.WriteRedirectPart(sb, MVCGridViewPart.JavaScript);
                else if (this._viewType == MVCGridViewType.CRUD)
                    this.WriteCrudPart(sb, MVCGridViewPart.JavaScript);
                else if (this._viewType == MVCGridViewType.Search)
                    this.WriteSearchPart(sb, MVCGridViewPart.JavaScript);
                else if (this._viewType == MVCGridViewType.Inline)
                    this.WriteInlinePart(sb, MVCGridViewPart.JavaScript);
                sb.AppendLine("");
                sb.AppendLine("    <script type=\"text/javascript\">");
                if (this._viewType == MVCGridViewType.Redirect)
                    this.WriteRedirectPart(sb, MVCGridViewPart.JavaScriptVariablesAndScript);
                else if (this._viewType == MVCGridViewType.CRUD)
                    this.WriteCrudPart(sb, MVCGridViewPart.JavaScriptVariablesAndScript);
                else if (this._viewType == MVCGridViewType.ReadOnly || this._viewType == MVCGridViewType.ScrollLoad)
                    this.WriteReadOnlyPart(sb, MVCGridViewPart.JavaScriptVariablesAndScript);
                else if (this._viewType == MVCGridViewType.Inline)
                    this.WriteInlinePart(sb, MVCGridViewPart.JavaScriptVariablesAndScript);
                while (true)
                {
                    if (this._viewType == MVCGridViewType.MasterDetailGrid || this._viewType == MVCGridViewType.MasterDetailSubGrid)
                    {
                        ++masterDetailPassCtr;
                        switch (masterDetailPassCtr)
                        {
                            case 1:
                                this._table = foreignKeyDetailTable;
                                break;
                            case 2:
                                this._table = currentMasterTable;
                                break;
                        }
                    }

                    if (this._viewType == MVCGridViewType.Redirect || this._viewType == MVCGridViewType.CRUD || this._viewType == MVCGridViewType.Inline)
                    {
                        if (_isUseAuditLogging)
                            this.WriteAuditLogPart(sb, viewName);
                        this.WriteFormatterPart(sb);
                    }

                    if (this._viewType == MVCGridViewType.MasterDetailSubGrid && masterDetailPassCtr == 2)
                    {
                        sb.AppendLine("        function SubGridDetail(subgrid_id, row_id) {");
                        sb.AppendLine("            var subgrid_table_id, pager_id;");
                        sb.AppendLine("            subgrid_table_id = subgrid_id + '_t';");
                        sb.AppendLine("            pager_id = 'p_' + subgrid_table_id;");
                        sb.AppendLine("");
                        sb.AppendLine("            $('#' + subgrid_id).html(\"<div style='padding: 20px;'><table id='\" + subgrid_table_id + \"'></table><div id='\" + pager_id + \"'></div></div>\");");
                        sb.AppendLine("");
                    }
                    else
                        sb.AppendLine("        $(function () {");
                    if (this._viewType == MVCGridViewType.CRUD)
                        this.WriteCrudPart(sb, MVCGridViewPart.JavaScriptInsideDollarFunctionHeader);
                    else if (this._viewType == MVCGridViewType.Search)
                        this.WriteSearchPart(sb, MVCGridViewPart.JavaScriptInsideDollarFunctionHeader);
                    else if (this._viewType == MVCGridViewType.Inline)
                        this.WriteInlinePart(sb, MVCGridViewPart.JavaScriptInsideDollarFunctionHeader);
                    if (this._viewType == MVCGridViewType.MasterDetailGrid && masterDetailPassCtr == 2)
                    {
                        sb.AppendLine("            $('#list-grid-detail').jqGrid({");
                        if (this._isUseWebApi)
                            sb.AppendLine("                url: \"@Functions.GetWebApiBaseAddress()" + currentMasterTable.Name + "Api/" + this.GetActionName(currentMasterTable) + "/?id=" + Functions.GetMinimumValueOfPrimaryKey(this._currentColumn) + "\",");
                        else
                            sb.AppendLine("                url: \"/" + currentMasterTable.Name + "/" + currentMasterTable.Name + "_" + viewName + "?handler=GridDataWithFilters&filters={\\\"groupOp\\\":\\\"AND\\\",\\\"rules\\\":[{\\\"field\\\":\\\"" + this._currentColumn.Name + "\\\",\\\"op\\\":\\\"eq\\\",\\\"data\\\":\\\"" + Functions.GetMinimumValueOfPrimaryKey(this._currentColumn) + "\\\"}]}\",");
                    }
                    else if (this._viewType == MVCGridViewType.MasterDetailSubGrid && masterDetailPassCtr == 2)
                        sb.AppendLine("            $('#' + subgrid_table_id).jqGrid({");
                    else
                        sb.AppendLine("            $('#list-grid').jqGrid({");
                    if (this._viewType == MVCGridViewType.MasterDetailGrid || this._viewType == MVCGridViewType.MasterDetailSubGrid)
                    {
                        if (masterDetailPassCtr == 1)
                        {
                            if (this._isUseWebApi)
                                sb.AppendLine("                url: '@Functions.GetWebApiBaseAddress()" + this._table.Name + "Api/" + actionName + "/',");
                            else
                                sb.AppendLine("                url: '/" + currentMasterTable.Name + "/" + currentMasterTable.Name + "_" + viewName + "?handler=" + actionName + "',");
                        }
                        else if (this._viewType == MVCGridViewType.MasterDetailSubGrid && masterDetailPassCtr == 2)
                        {
                            if (this._isUseWebApi)
                                sb.AppendLine("                url: \"@Functions.GetWebApiBaseAddress()" + currentMasterTable.Name + "Api/" + this.GetActionName(currentMasterTable) + "/?id=\" + row_id,");
                            else
                                sb.AppendLine("                url: \"/" + currentMasterTable.Name + "/" + currentMasterTable.Name + "_" + viewName + "?handler=GridDataWithFilters&filters={\\\"groupOp\\\":\\\"AND\\\",\\\"rules\\\":[{\\\"field\\\":\\\"" + this._currentColumn.Name + "\\\",\\\"op\\\":\\\"eq\\\",\\\"data\\\":\\\"\" + row_id + \"\\\"}]}\",");
                        }
                    }
                    else if (this._isUseWebApi)
                        sb.AppendLine("                url: '@Functions.GetWebApiBaseAddress()" + this._table.Name + "Api/" + actionName + "/',");
                    else
                        sb.AppendLine("                url: '/" + this._table.Name + "/" + this._table.Name + "_" + viewName + "?handler=" + actionName + "',");
                    sb.AppendLine("                datatype: 'json',");
                    sb.AppendLine("                mtype: 'GET',");
                    if (this._viewType == MVCGridViewType.ReadOnly || this._viewType == MVCGridViewType.ScrollLoad)
                        this.WriteReadOnlyPart(sb, MVCGridViewPart.ColNames);
                    else if (this._viewType == MVCGridViewType.Redirect)
                        this.WriteRedirectPart(sb, MVCGridViewPart.ColNames);
                    else if (this._viewType == MVCGridViewType.CRUD)
                        this.WriteCrudPart(sb, MVCGridViewPart.ColNames);
                    else if (this._viewType == MVCGridViewType.Totals)
                        this.WriteTotalsPart(sb, MVCGridViewPart.ColNames);
                    else if (this._viewType == MVCGridViewType.Search)
                        this.WriteSearchPart(sb, MVCGridViewPart.ColNames);
                    else if (this._viewType == MVCGridViewType.Inline)
                        this.WriteInlinePart(sb, MVCGridViewPart.ColNames);
                    else if (this._viewType == MVCGridViewType.ToolTip)
                        this.WriteToolTipPart(sb, MVCGridViewPart.ColNames);
                    else if (this._viewType == MVCGridViewType.GroupedBy || this._viewType == MVCGridViewType.GroupedByWithTotals)
                        this.WriteGroupedByPart(sb, MVCGridViewPart.ColNames);
                    else if (this._viewType == MVCGridViewType.MasterDetailGrid || this._viewType == MVCGridViewType.MasterDetailSubGrid)
                        this.WriteMasterDetailPart(sb, MVCGridViewPart.ColNames, masterDetailPassCtr, currentMasterTable, foreignKeyDetailTable, viewName);
                    foreach (Column column in this._table.Columns)
                    {
                        string str1 = "left";
                        string str2 = string.Empty;
                        string str3 = string.Empty;
                        string str4 = string.Empty;
                        string str5 = string.Empty;
                        string str6 = string.Empty;
                        string str7 = string.Empty;
                        ++num;
                        if (column.IsBinaryOrSpatialDataType)
                            str4 = ", sortable: false";
                        if (this._viewType == MVCGridViewType.Inline)
                        {
                            if (!column.IsPrimaryKeyUnique)
                                str7 = ", editable: true";
                        }
                        else if (this._viewType == MVCGridViewType.Search && column.IsBinaryOrSpatialDataType)
                            str5 = ", search: false ";
                        if (column.IsPrimaryKey || column.IsForeignKey)
                        {
                            if (column.SQLDataType == SQLType.integer || column.SQLDataType == SQLType.bigint || (column.SQLDataType == SQLType.smallint || column.SQLDataType == SQLType.floatnumber) || (column.SQLDataType == SQLType.real || column.SQLDataType == SQLType.decimalnumber || (column.SQLDataType == SQLType.money || column.SQLDataType == SQLType.smallmoney)))
                                str1 = "right";
                            if ((this._viewType == MVCGridViewType.ReadOnly || this._viewType == MVCGridViewType.ScrollLoad) && column.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK)
                                str2 = ", formatter: " + column.NameCamelStyle + "Link";
                        }
                        else if (column.SQLDataType == SQLType.bit)
                        {
                            str1 = "center";
                            str2 = this._viewType != MVCGridViewType.Search ? (this._viewType != MVCGridViewType.Inline ? ", formatter: 'checkbox'" : ", stype: 'select', edittype: 'checkbox', editoptions: { value: 'True:False' }") : ", formatter: 'select', stype: 'select', edittype: 'select', editoptions: { value: checkBoxSelectValues }";
                        }
                        else if (column.SystemTypeNative == "DateTime")
                            str3 = this._viewType != MVCGridViewType.Inline ? ", sorttype: \"date\"" : ", sorttype: \"date\", editoptions: { size: 20,  dataInit: function (el) { $(el).datepicker({ dateFormat: 'm/d/yy' }); }}";
                        else if (column.SQLDataType == SQLType.integer || column.SQLDataType == SQLType.bigint || column.SQLDataType == SQLType.smallint)
                        {
                            str1 = "right";
                            str2 = ", formatter: 'integer'";
                        }
                        else if (column.SQLDataType == SQLType.money || column.SQLDataType == SQLType.smallmoney)
                        {
                            str1 = "right";
                            str2 = ", formatter: 'currency', formatoptions: { decimalPlaces: 2, prefix: \"$\"}";
                            if (this._viewType == MVCGridViewType.GroupedByWithTotals)
                                str6 = ", summaryType: 'sum', summaryTpl: '<b>Total: {0}</b>'";
                        }
                        else if (column.SQLDataType == SQLType.floatnumber || column.SQLDataType == SQLType.real || column.SQLDataType == SQLType.decimalnumber)
                            str2 = ", formatter: 'currency', formatoptions: { decimalPlaces: 2 }";
                        else if (column.SystemType.ToLower() == "string" && this._viewType == MVCGridViewType.Search)
                            str5 = !column.IsBinaryOrSpatialDataType ? ", searchoptions: { sopt: ['cn']}" : ", search: false ";
                        if ((this._viewType == MVCGridViewType.Search || this._viewType == MVCGridViewType.Inline) && (column.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK && !string.IsNullOrEmpty(column.DropDownListDataPropertyName)))
                        {
                            str1 = "left";
                            str2 = ", formatter: 'select', stype: 'select', edittype: 'select', editoptions: { value: " + column.NameCamelStyle + "SelectValues }";
                        }
                        if ((this._viewType == MVCGridViewType.ReadOnly || this._viewType == MVCGridViewType.Totals || (this._viewType == MVCGridViewType.Search || this._viewType == MVCGridViewType.ScrollLoad)) && num == this._table.Columns.Count)
                            sb.AppendLine("                    { name: '" + column.Name + "', index: '" + column.Name + "', align: '" + str1 + "'" + str4 + str7 + str2 + str3 + str5 + " }");
                        else if (this._viewType == MVCGridViewType.ToolTip && column.IsForeignKey)
                        {
                            if (column.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK)
                            {
                                sb.AppendLine("                    { name: '" + column.Name + "', index: '" + column.Name + "', hidden: true },");
                                sb.AppendLine("                    { name: 'tooltipOperationFor" + column.Name + "', index: '" + column.Name + "', align: '" + str1 + "'" + str4 + str7 + str2 + str3 + str5 + " },");
                            }
                            else
                                sb.AppendLine("                    { name: '" + column.Name + "', index: '" + column.Name + "', align: '" + str1 + "'" + str4 + str7 + str2 + str3 + str5 + " },");
                        }
                        else
                            sb.AppendLine("                    { name: '" + column.Name + "', index: '" + column.Name + "', align: '" + str1 + "'" + str4 + str7 + str2 + str3 + str5 + str6 + " },");
                    }
                    if (this._viewType == MVCGridViewType.Redirect)
                        this.WriteRedirectPart(sb, MVCGridViewPart.AdditionalColumns);
                    else if (this._viewType == MVCGridViewType.CRUD)
                        this.WriteCrudPart(sb, MVCGridViewPart.AdditionalColumns);
                    else if (this._viewType == MVCGridViewType.ToolTip)
                        this.WriteToolTipPart(sb, MVCGridViewPart.AdditionalColumns);
                    else if (this._viewType == MVCGridViewType.GroupedBy || this._viewType == MVCGridViewType.GroupedByWithTotals)
                        this.WriteGroupedByPart(sb, MVCGridViewPart.AdditionalColumns);
                    else if (this._viewType == MVCGridViewType.Inline)
                        this.WriteInlinePart(sb, MVCGridViewPart.AdditionalColumns);
                    if (this._viewType == MVCGridViewType.ScrollLoad)
                    {
                        sb.AppendLine("                ],");
                        sb.AppendLine("                pager: $('#list-pager'),");
                        sb.AppendLine("                pageable: true,");
                        sb.AppendLine("                autoencode: true,");
                        sb.AppendLine("                jsonReader:");
                        sb.AppendLine("                {");
                        sb.AppendLine("                    page: \"d.page\"");
                        sb.AppendLine("                },");
                        sb.AppendLine("                scroll: 1,");
                        sb.AppendLine("                gridview: true,");
                        sb.AppendLine("                rownumbers: true,");
                        sb.AppendLine("                rownumWidth: 20,");
                        sb.AppendLine("                rowNum: 20,");
                        sb.AppendLine("                sortname: '" + this._table.FirstPrimaryKeyName + "',");
                        sb.AppendLine("                sortorder: \"asc\",");
                        sb.AppendLine("                viewrecords: true,");
                        sb.AppendLine("                caption: '" + this._pageTitle + "',");
                        sb.AppendLine("                height: '250',");
                    }
                    else
                    {
                        sb.AppendLine("                ],");
                        if (this._viewType == MVCGridViewType.MasterDetailGrid && masterDetailPassCtr == 2)
                            sb.AppendLine("                pager: $('#list-pager-detail'),");
                        else if (this._viewType == MVCGridViewType.MasterDetailSubGrid && masterDetailPassCtr == 2)
                            sb.AppendLine("                pager: pager_id,");
                        else
                            sb.AppendLine("                pager: $('#list-pager'),");
                        if ((this._viewType == MVCGridViewType.MasterDetailGrid || this._viewType == MVCGridViewType.MasterDetailSubGrid) && masterDetailPassCtr == 1)
                            sb.AppendLine("                rowNum: 5,");
                        else
                            sb.AppendLine("                rowNum: 10,");
                        sb.AppendLine("                pageable: true,");
                        sb.AppendLine("                autoencode: true,");
                        sb.AppendLine("                jsonReader:");
                        sb.AppendLine("                {");
                        sb.AppendLine("                    page: \"d.page\"");
                        sb.AppendLine("                },");
                        sb.AppendLine("                rowList: [5, 10, 20, 50],");
                        sb.AppendLine("                sortname: '" + this._table.FirstPrimaryKeyName + "',");
                        sb.AppendLine("                sortorder: \"asc\",");
                        sb.AppendLine("                viewrecords: true,");
                        if (this._viewType != MVCGridViewType.MasterDetailGrid && this._viewType != MVCGridViewType.MasterDetailSubGrid)
                            sb.AppendLine("                caption: '" + this._pageTitle + "',");
                        else if ((this._viewType == MVCGridViewType.MasterDetailGrid || this._viewType == MVCGridViewType.MasterDetailSubGrid) && masterDetailPassCtr == 1)
                            sb.AppendLine("                caption: '" + this._pageTitle + "',");
                        sb.AppendLine("                height: '100%',");
                    }
                    if (this._viewType == MVCGridViewType.ReadOnly || this._viewType == MVCGridViewType.ScrollLoad)
                        this.WriteReadOnlyPart(sb, MVCGridViewPart.AdditionalProperties);
                    else if (this._viewType == MVCGridViewType.Redirect)
                        this.WriteRedirectPart(sb, MVCGridViewPart.AdditionalProperties);
                    else if (this._viewType == MVCGridViewType.CRUD)
                        this.WriteCrudPart(sb, MVCGridViewPart.AdditionalProperties);
                    else if (this._viewType == MVCGridViewType.Totals)
                        this.WriteTotalsPart(sb, MVCGridViewPart.AdditionalProperties);
                    else if (this._viewType == MVCGridViewType.Search)
                        this.WriteSearchPart(sb, MVCGridViewPart.AdditionalProperties);
                    else if (this._viewType == MVCGridViewType.Inline)
                        this.WriteInlinePart(sb, MVCGridViewPart.AdditionalProperties);
                    else if (this._viewType == MVCGridViewType.ToolTip)
                        this.WriteToolTipPart(sb, MVCGridViewPart.AdditionalProperties);
                    else if (this._viewType == MVCGridViewType.GroupedBy || this._viewType == MVCGridViewType.GroupedByWithTotals)
                        this.WriteGroupedByPart(sb, MVCGridViewPart.AdditionalProperties);
                    else if (this._viewType == MVCGridViewType.MasterDetailGrid || this._viewType == MVCGridViewType.MasterDetailSubGrid)
                        this.WriteMasterDetailPart(sb, MVCGridViewPart.AdditionalProperties, masterDetailPassCtr, currentMasterTable, foreignKeyDetailTable, viewName);
                    sb.AppendLine("            });");
                    if (this._viewType == MVCGridViewType.MasterDetailSubGrid && masterDetailPassCtr == 2)
                        sb.AppendLine("        }");
                    else
                        sb.AppendLine("        });");
                    if ((this._viewType == MVCGridViewType.MasterDetailGrid || this._viewType == MVCGridViewType.MasterDetailSubGrid) && masterDetailPassCtr == 1)
                        sb.AppendLine("");
                    else
                        break;
                }
                sb.AppendLine("");
                sb.AppendLine("        // rename the page parameter to _page because asp.net core razor's page model");
                sb.AppendLine("        // does not recognize the page parameter when passed");
                sb.AppendLine("        $.extend(jQuery.jgrid.defaults, {");
                sb.AppendLine("            prmNames: {");
                sb.AppendLine("                page: \"_page\"");
                sb.AppendLine("            }");
                sb.AppendLine("        });");
                sb.AppendLine("    </script> ");
                sb.AppendLine("}");
                sb.AppendLine("");
                sb.AppendLine("<h2>@ViewData[\"Title\"]</h2>");
                sb.AppendLine("<br /><br />");
                if (this._viewType == MVCGridViewType.Redirect)
                    this.WriteRedirectPart(sb, MVCGridViewPart.BodyBeforeGrid);
                else if (this._viewType == MVCGridViewType.CRUD)
                    this.WriteCrudPart(sb, MVCGridViewPart.BodyBeforeGrid);
                else if (this._viewType == MVCGridViewType.Inline)
                    this.WriteInlinePart(sb, MVCGridViewPart.BodyBeforeGrid);
                sb.AppendLine("<table id=\"list-grid\"></table>");
                sb.AppendLine("<div id=\"list-pager\"></div>");
                if (this._viewType == MVCGridViewType.MasterDetailGrid)
                {
                    sb.AppendLine("<br /><br />");
                    sb.AppendLine("<table id=\"list-grid-detail\"></table>");
                    sb.AppendLine("<div id=\"list-pager-detail\"></div>");
                }
                if (this._viewType == MVCGridViewType.Redirect || this._viewType == MVCGridViewType.CRUD || this._viewType == MVCGridViewType.Inline)
                {
                    if (_isUseAuditLogging)
                    {
                        sb.AppendLine("");
                        sb.AppendLine("<!-- Modal HTML -->");
                        sb.AppendLine("<div id=\"myModal\" class=\"modal fade\">");
                        sb.AppendLine("    <div class=\"modal-dialog\">");
                        sb.AppendLine("        <div class=\"modal-content\">");
                        sb.AppendLine("            <div class=\"modal-header\">");
                        sb.AppendLine("                <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">&times;</button>");
                        sb.AppendLine("                <h4 class=\"modal-title\">Audit history</h4>");
                        sb.AppendLine("            </div>");
                        sb.AppendLine("            <div class=\"modal-body\">");
                        sb.AppendLine("                <div id=\"audit\"></div>");
                        sb.AppendLine("            </div>");
                        sb.AppendLine("            <div class=\"modal-footer\">");
                        sb.AppendLine("                <button type=\"button\" class=\"btn btn-primary\" data-dismiss=\"modal\">Close</button>");
                        sb.AppendLine("            </div>");
                        sb.AppendLine("        </div>");
                        sb.AppendLine("    </div>");
                        sb.AppendLine("</div>");
                    }
                }
                streamWriter.Write(sb.ToString());
            }
        }

        private void GenerateGroupedBy()
        {
            foreach (Column column in this._table.Columns)
            {
                if (column.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK)
                {
                    this._currentFktable = column.ForeignKeyTable;
                    this._currentColumn = column;
                    this._pageTitle = "List of " + Functions.GetNameWithSpaces(this._table.Name) + " By " + Functions.GetNameWithSpaces(this._currentFktable.Name);
                    if (this._viewType == MVCGridViewType.MasterDetailGrid || this._viewType == MVCGridViewType.MasterDetailSubGrid)
                        this.Generate(this._table, column.ForeignKeyTable);
                    else
                        this.Generate(null, null);
                }
            }
        }

        private void GenerateForeach()
        {
            using (StreamWriter streamWriter = new StreamWriter(this._directory + this._table.Name + "_" + Functions.GetViewName(this._viewType, this._viewName, this._currentColumn) + ".cshtml"))
            {
                StringBuilder stringBuilder1 = new StringBuilder();
                StringBuilder stringBuilder2 = new StringBuilder();
                string empty1 = string.Empty;
                string empty2 = string.Empty;
                int num1 = 2 + this._table.Columns.Count;
                int jqueryUiTheme = (int)Functions.GetJQueryUITheme(this._jqueryUITheme);
                string colorByJqueryUiTheme1 = Functions.GetHeaderFooterTableBgColorByJQueryUITheme((JQueryUITheme)jqueryUiTheme);
                string colorByJqueryUiTheme2 = Functions.GetHeaderFooterFontColorByJQueryUITheme((JQueryUITheme)jqueryUiTheme);
                foreach (Column column in this._table.Columns)
                {
                    string str = string.Empty;
                    if (column.SQLDataType == SQLType.integer || column.SQLDataType == SQLType.bigint || (column.SQLDataType == SQLType.smallint || column.SQLDataType == SQLType.floatnumber) || (column.SQLDataType == SQLType.real || column.SQLDataType == SQLType.decimalnumber || (column.SQLDataType == SQLType.money || column.SQLDataType == SQLType.smallmoney)))
                        str = " align=\"right\"";
                    else if (column.SQLDataType == SQLType.bit)
                        str = " align=\"center\"";
                    if (column.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK)
                        stringBuilder2.AppendLine("            <td" + str + "><a href=\"~/" + column.ForeignKeyTable.Name + "/" + column.ForeignKeyTable.Name + "_Details?id=@item." + column.Name + "&returnUrl=/" + this._table.Name + "/" + this._table.Name + "_" + this._viewName + "\">@item." + column.Name + "</a></td>");
                    else if (column.SQLDataType == SQLType.money || column.SQLDataType == SQLType.smallmoney)
                        stringBuilder2.AppendLine("            <td" + str + ">@Convert.ToDouble(item." + column.Name + ").ToString(\"C\")</td>");
                    else if (column.SQLDataType == SQLType.bit)
                        stringBuilder2.AppendLine("            <td" + str + "><span><input type=\"checkbox\" @(item." + column.Name + " ? \"checked=\\\"checked\\\"\" : \"\") disabled=\"disabled\" /></span></td>");
                    else if (column.SQLDataType == SQLType.date || column.SQLDataType == SQLType.datetime || column.SQLDataType == SQLType.datetime2)
                    {
                        if (column.IsNullable)
                            stringBuilder2.AppendLine("            <td" + str + ">@(item." + column.Name + ".HasValue ? item." + column.Name + ".Value.ToString(\"d\") : \"\")</td>");
                        else
                            stringBuilder2.AppendLine("            <td" + str + ">@item." + column.Name + ".ToString(\"d\")</td>");
                    }
                    else
                        stringBuilder2.AppendLine("            <td" + str + ">@item." + column.Name + "</td>");
                }
                string str1;
                string str2;
                if (this._table.PrimaryKeyCount > 1)
                {
                    int num2 = 0;
                    string str3 = string.Empty;
                    string str4 = string.Empty;
                    foreach (Column primaryKeyColumn in this._table.PrimaryKeyColumns)
                    {
                        str4 = str4 + "item." + primaryKeyColumn.Name;
                        str3 = str3 + "item." + primaryKeyColumn.Name;
                        if (num2 + 1 < this._table.PrimaryKeyCount)
                        {
                            str3 += " + \"/\" + ";
                            str4 += " + \"/\" + ";
                        }
                        ++num2;
                    }
                    str1 = "                <a href=\"/" + this._table.Name + "/" + this._table.Name + "_" + this._viewNames.UpdateRecord + "?" + Functions.GetPrimaryKeysAtItemUrlParameters(this._table, Language.CSharp) + "&returnUrl=/" + this._table.Name + "/" + this._table.Name + "_" + this._viewName + "\" title=\"Click to edit\"><img src=\"@Url.Content(\"~/images/Edit.gif\")\" alt=\"\" style=\"border:none;\" /></a>";
                    str2 = "                <input type=\"image\" id=\"imgDelete1\" title=\"Click to delete\" src=\"@Url.Content(\"~/images/Delete.png\")\" onclick=\"deleteItem('@(" + str4 + ")');\" style=\"border-style:none;\" />";
                }
                else
                {
                    str1 = "                <a href=\"/" + this._table.Name + "/" + this._table.Name + "_" + this._viewNames.UpdateRecord + "?id=@item." + this._table.FirstPrimaryKeyName + "&returnUrl=/" + this._table.Name + "/" + this._table.Name + "_" + this._viewName + "\" title=\"Click to edit\"><img src=\"@Url.Content(\"~/images/Edit.gif\")\" alt=\"\" style=\"border:none;\" /></a>";
                    str2 = "                <input type=\"image\" id=\"imgDelete1\" title=\"Click to delete\" src=\"@Url.Content(\"~/images/Delete.png\")\" onclick=\"deleteItem('@item." + this._table.FirstPrimaryKeyName + "');\" style=\"border-style:none;\" />";
                }
                stringBuilder1.AppendLine("@page");
                stringBuilder1.AppendLine("@model " + this._webAppName + ".Pages." + this._table.Name + "_" + this._viewName + "Model");
                stringBuilder1.AppendLine("@{");
                stringBuilder1.AppendLine("    ViewData[\"Title\"] = \"" + this._pageTitle + "\";");
                stringBuilder1.AppendLine("    string bgColor = \"#F7F6F3\";");
                stringBuilder1.AppendLine("}");
                stringBuilder1.AppendLine("");
                stringBuilder1.AppendLine("@section AdditionalJavaScript {");
                stringBuilder1.AppendLine("    <script src=\"~/js/jqgrid-listforeach.js\" asp-append-version=\"true\"></script>");
                stringBuilder1.AppendLine("");
                stringBuilder1.AppendLine("    <script type=\"text/javascript\">");
                stringBuilder1.AppendLine("        var urlAndMethod = '/" + this._table.Name + "/" + this._table.Name + "_" + this._viewName + "';");
                stringBuilder1.AppendLine("    </script>");
                stringBuilder1.AppendLine("}");
                stringBuilder1.AppendLine("");
                stringBuilder1.AppendLine("<h2>@ViewData[\"Title\"]</h2>");
                stringBuilder1.AppendLine("<br /><br />");
                stringBuilder1.AppendLine("<div id=\"errorConfirmationDialog\"></div>");
                stringBuilder1.AppendLine("<div id=\"errorDialog\"></div>");
                stringBuilder1.AppendLine("");
                stringBuilder1.AppendLine("<a href=\"/" + this._table.Name + "/" + this._table.Name + "_" + this._viewNames.AddRecord + "?returnUrl=/" + this._table.Name + "/" + this._table.Name + "_" + this._viewName + "\"><img src=\"@Url.Content(\"~/images/Add.gif\")\" alt=\"Add New " + Functions.GetNameWithSpaces(this._table.Name) + "\" style=\"border: none;\" /></a>&nbsp;<a href=\"/" + this._table.Name + "/" + this._table.Name + "_" + this._viewNames.AddRecord + "?returnUrl=/" + this._table.Name + "/" + this._table.Name + "_" + this._viewName + "\">Add New " + Functions.GetNameWithSpaces(this._table.Name) + "</a>");
                stringBuilder1.AppendLine("<br /><br />");
                stringBuilder1.AppendLine("");
                stringBuilder1.AppendLine("<table class=\"gridviewGridLines\" cellspacing =\"0\" cellpadding=\"8\" style=\"width:100%;border-collapse:collapse;\">");
                stringBuilder1.AppendLine("    <tr style=\"color:" + colorByJqueryUiTheme2 + ";background-color:" + colorByJqueryUiTheme1 + ";font-weight:bold;\">");
                stringBuilder1.AppendLine("        @for (int i = 0; i < Model." + this._table.Name + "FieldNames.GetLength(0); i++)");
                stringBuilder1.AppendLine("        {");
                stringBuilder1.AppendLine("            string fieldName = Model." + this._table.Name + "FieldNames[i, 0];");
                stringBuilder1.AppendLine("            string title = Model." + this._table.Name + "FieldNames[i, 1];");
                if (this._table.IsContainsBinaryOrSpatialDataTypes)
                {
                    stringBuilder1.AppendLine("            bool isUnsortableField = false;");
                    stringBuilder1.AppendLine("");
                    stringBuilder1.AppendLine("            if (Model.UnsortableFields != null && Model.UnsortableFields.Count > 0)");
                    stringBuilder1.AppendLine("            {");
                    stringBuilder1.AppendLine("                if(Model.UnsortableFields.Contains(fieldName))");
                    stringBuilder1.AppendLine("                {");
                    stringBuilder1.AppendLine("                    isUnsortableField = true;");
                    stringBuilder1.AppendLine("");
                    stringBuilder1.AppendLine("                    <td>@title</td>");
                    stringBuilder1.AppendLine("                }");
                    stringBuilder1.AppendLine("            }");
                    stringBuilder1.AppendLine("");
                    stringBuilder1.AppendLine("            if (!isUnsortableField)");
                    stringBuilder1.AppendLine("            {");
                    stringBuilder1.AppendLine("                if (Model.FieldToSortWithOrder.Contains(fieldName) && Model.FieldToSortWithOrder.Contains(\"asc\"))");
                    stringBuilder1.AppendLine("                {");
                    stringBuilder1.AppendLine("                    <td><a href=\"?sidx=@fieldName&sord=desc&handler=GridData\" style=\"color:" + colorByJqueryUiTheme2 + ";\">@title</a>@if (Model.FieldToSortWithOrder == fieldName + \" asc\") {<img src=\"@Url.Content(\"~/images/ArrowUp.png\")\" alt=\"\" />}</td>");
                    stringBuilder1.AppendLine("                }");
                    stringBuilder1.AppendLine("                else");
                    stringBuilder1.AppendLine("                {");
                    stringBuilder1.AppendLine("                    <td><a href=\"?sidx=@fieldName&sord=asc&handler=GridData\" style=\"color:" + colorByJqueryUiTheme2 + ";\">@title</a>@if (Model.FieldToSortWithOrder == fieldName + \" desc\") {<img src=\"@Url.Content(\"~/images/ArrowDown.png\")\" alt=\"\" />}</td>");
                    stringBuilder1.AppendLine("                }");
                    stringBuilder1.AppendLine("            }");
                }
                else
                {
                    stringBuilder1.AppendLine("");
                    stringBuilder1.AppendLine("            if (Model.FieldToSortWithOrder.Contains(fieldName) && Model.FieldToSortWithOrder.Contains(\"asc\"))");
                    stringBuilder1.AppendLine("            {");
                    stringBuilder1.AppendLine("                <td><a href=\"?sidx=@fieldName&sord=desc&handler=GridData\" style=\"color:" + colorByJqueryUiTheme2 + ";\">@title</a>@if (Model.FieldToSortWithOrder == fieldName + \" asc\") {<img src=\"@Url.Content(\"~/images/ArrowUp.png\")\" alt=\"\" />}</td>");
                    stringBuilder1.AppendLine("            }");
                    stringBuilder1.AppendLine("            else");
                    stringBuilder1.AppendLine("            {");
                    stringBuilder1.AppendLine("                <td><a href=\"?sidx=@fieldName&sord=asc&handler=GridData\" style=\"color:" + colorByJqueryUiTheme2 + ";\">@title</a>@if (Model.FieldToSortWithOrder == fieldName + \" desc\") {<img src=\"@Url.Content(\"~/images/ArrowDown.png\")\" alt=\"\" />}</td>");
                    stringBuilder1.AppendLine("            }");
                }
                stringBuilder1.AppendLine("        }");
                stringBuilder1.AppendLine("        <td>&nbsp;</td>");
                stringBuilder1.AppendLine("        <td>&nbsp;</td>");
                stringBuilder1.AppendLine("    </tr>");
                stringBuilder1.AppendLine("");
                stringBuilder1.AppendLine("    @foreach (var item in Model." + this._table.Name + "Data)");
                stringBuilder1.AppendLine("    {");
                stringBuilder1.AppendLine("        <tr style=\"color:#333333; background-color:@bgColor;\">");
                stringBuilder1.Append(stringBuilder2.ToString());
                stringBuilder1.AppendLine("            <td align=\"center\" style=\"width:30px;\">");
                stringBuilder1.AppendLine(str1);
                stringBuilder1.AppendLine("            </td>");
                stringBuilder1.AppendLine("            <td align=\"center\" style=\"width:30px;\">");
                stringBuilder1.AppendLine(str2);
                stringBuilder1.AppendLine("            </td>");
                stringBuilder1.AppendLine("        </tr>");
                stringBuilder1.AppendLine("");
                stringBuilder1.AppendLine("        bgColor = bgColor == \"#F7F6F3\" ? \"White\" : \"#F7F6F3\";");
                stringBuilder1.AppendLine("    }");
                stringBuilder1.AppendLine("");
                stringBuilder1.AppendLine("    <tr class=\"gridviewPagerStyle\" align=\"center\" style=\"background-color:" + colorByJqueryUiTheme1 + ";\">");
                stringBuilder1.AppendLine("        <td colspan=\"" + num1 + "\">");
                stringBuilder1.AppendLine("            <table>");
                stringBuilder1.AppendLine("                <tr>");
                stringBuilder1.AppendLine("                    @if (Model.CurrentPage > Model.NumberOfPagesToShow)");
                stringBuilder1.AppendLine("                    {");
                stringBuilder1.AppendLine("                        <td><a href=\"?sidx=@Model.FieldToSort&sord=@Model.FieldSortOrder&_page=1&handler=GridData\" style=\"color:#000000;\">< First</a></td>");
                stringBuilder1.AppendLine("                        <td><a href=\"?sidx=@Model.FieldToSort&sord=@Model.FieldSortOrder&_page=@(Model.StartPage - 1)&handler=GridData\" style=\"color:#000000;\">...</a></td>");
                stringBuilder1.AppendLine("                    }");
                stringBuilder1.AppendLine("");
                stringBuilder1.AppendLine("                    @for (int pageNumber = Model.StartPage; pageNumber <= Model.EndPage; pageNumber++)");
                stringBuilder1.AppendLine("                    {");
                stringBuilder1.AppendLine("                        if (pageNumber == Model.CurrentPage)");
                stringBuilder1.AppendLine("                        {");
                stringBuilder1.AppendLine("                            <td><span style=\"font-size:12px;\">@pageNumber</span></td>");
                stringBuilder1.AppendLine("                        }");
                stringBuilder1.AppendLine("                        else");
                stringBuilder1.AppendLine("                        {");
                stringBuilder1.AppendLine("                            <td><a href=\"?sidx=@Model.FieldToSort&sord=@Model.FieldSortOrder&_page=@pageNumber&handler=GridData\" style=\"color:#000000;\">@pageNumber</a></td>");
                stringBuilder1.AppendLine("                        }");
                stringBuilder1.AppendLine("                    }");
                stringBuilder1.AppendLine("");
                stringBuilder1.AppendLine("                    @if (Model.EndPage < Model.TotalPages)");
                stringBuilder1.AppendLine("                    {");
                stringBuilder1.AppendLine("                        <td><a href=\"?sidx=@Model.FieldToSort&sord=@Model.FieldSortOrder&_page=@(Model.EndPage + 1)&handler=GridData\" style=\"color:#000000;\">...</a></td>");
                stringBuilder1.AppendLine("                        <td><a href=\"?sidx=@Model.FieldToSort&sord=@Model.FieldSortOrder&_page=@Model.TotalPages&handler=GridData\" style=\"color:#000000;\">Last ></a></td>");
                stringBuilder1.AppendLine("                    }");
                stringBuilder1.AppendLine("                </tr>");
                stringBuilder1.AppendLine("            </table>");
                stringBuilder1.AppendLine("        </td>");
                stringBuilder1.AppendLine("    </tr>");
                stringBuilder1.AppendLine("</table>");
                streamWriter.Write(stringBuilder1.ToString());
            }
        }

        private void WriteRedirectPart(StringBuilder sb, MVCGridViewPart part)
        {
            switch (part)
            {
                case MVCGridViewPart.JavaScript:
                    sb.AppendLine("    <script src=\"~/js/jqgrid-listcrudredirect.js\" asp-append-version=\"true\"></script>");
                    break;
                case MVCGridViewPart.JavaScriptVariablesAndScript:
                    sb.AppendLine("        var urlAndMethod = '/" + this._table.Name + "/" + this._table.Name + "_" + this._viewName + "';");
                    sb.AppendLine("");
                    break;
                case MVCGridViewPart.ColNames:
                    if (_isUseAuditLogging)
                        sb.AppendLine("                colNames: [" + this._commaDelimitedColNames.ToString() + " '', '', ''],");
                    else
                        sb.AppendLine("                colNames: [" + this._commaDelimitedColNames.ToString() + " '', ''],");
                    sb.AppendLine("                colModel: [");
                    break;
                case MVCGridViewPart.AdditionalColumns:
                    sb.AppendLine("                    { name: 'editoperation', index: 'editoperation', align: 'center', width: 40, sortable: false , formatter: editFormatter },");
                    sb.AppendLine("                    { name: 'deleteoperation', index: 'deleteoperation', align: 'center', width: 40, sortable: false , formatter: deleteFormatter },");
                    if (this._isUseAuditLogging)
                        sb.AppendLine("                    { name: 'auditoperation', index: 'auditoperation', align: 'center', width: 40, sortable: false, formatter: auditFormatter }");
                    break;
                case MVCGridViewPart.AdditionalProperties:
                    sb.AppendLine("                width: '1200',");
                    sb.AppendLine("                gridComplete: function () {");
                    sb.AppendLine("                    var ids = jQuery(\"#list-grid\").jqGrid('getDataIDs');");
                    sb.AppendLine("                    for (var i = 0; i < ids.length; i++) {");
                    sb.AppendLine("                        var currentid = ids[i];");
                    if (this._table.PrimaryKeyCount > 1)
                    {
                        int num = 0;
                        string str = string.Empty;
                        foreach (Column primaryKeyColumn in this._table.PrimaryKeyColumns)
                        {
                            sb.AppendLine("                        var id" + num + " = jQuery(\"#list-grid\").jqGrid('getCell', ids[i], '" + primaryKeyColumn.Name + "');");
                            str = str + " + id" + num + " + ";
                            if (num + 1 < this._table.PrimaryKeyCount)
                                str += "\"/\"";
                            ++num;
                        }
                        sb.AppendLine("                        editLink = \"<a href='/" + this._table.Name + "/" + this._table.Name + "_" + this._viewNames.UpdateRecord + "?" + Functions.GetCommaDelimitedPrimaryKeysUrlParameters(this._table, Language.CSharp) + "&returnUrl=/" + this._table.Name + "/" + this._table.Name + "_" + this._viewName + "'><img src='/images/Edit.gif' style='border:none;' /></a>\";");
                        sb.AppendLine("                        deleteLink = \"<a href='#' onclick=\\\"deleteItem('\"" + str + "\"')\\\"><img src='/images/Delete.png' style='border:none;' /></a>\";");
                        if (this._isUseAuditLogging)
                            sb.AppendLine("                        auditLink =\"<a href='#' onClick=\\\"GetAuditHistory(" + str + ")\\\">Audit</a>\";");
                    }
                    else
                    {
                        sb.AppendLine("                        editLink = \"<a href='/" + this._table.Name + "/" + this._table.Name + "_" + this._viewNames.UpdateRecord + "?id=\" + currentid + \"&returnUrl=/" + this._table.Name + "/" + this._table.Name + "_" + this._viewName + "'><img src='/images/Edit.gif' style='border:none;' /></a>\";");
                        sb.AppendLine("                        deleteLink = \"<a href='#' onclick=\\\"deleteItem('\" + currentid + \"')\\\"><img src='/images/Delete.png' style='border:none;' /></a>\";");
                        if (this._isUseAuditLogging)
                            sb.AppendLine("                        auditLink =\"<a href='#' onClick=\\\"GetAuditHistory(\" + currentid + \")\\\">Audit</a>\";");
                    }
                    sb.AppendLine("                        jQuery(\"#list-grid\").jqGrid('setRowData', ids[i], { editoperation: editLink });");
                    sb.AppendLine("                        jQuery(\"#list-grid\").jqGrid('setRowData', ids[i], { deleteoperation: deleteLink });");
                    if (this._isUseAuditLogging)
                        sb.AppendLine("                        jQuery(\"#list-grid\").jqGrid('setRowData', ids[i], { auditoperation: auditLink });");
                    sb.AppendLine("                    }");
                    sb.AppendLine("                }");
                    break;
                case MVCGridViewPart.BodyBeforeGrid:
                    sb.AppendLine("<div id=\"errorConfirmationDialog\"></div>");
                    sb.AppendLine("<div id=\"errorDialog\"></div>");
                    sb.AppendLine("");
                    sb.AppendLine("<a href=\"/" + this._table.Name + "/" + this._table.Name + "_" + this._viewNames.AddRecord + "?returnUrl=/" + this._table.Name + "/" + this._table.Name + "_" + this._viewName + "\"><img src=\"@Url.Content(\"~/images/Add.gif\")\" alt=\"Add New " + this._table.Name + "\" style=\"border: none;\" /></a>&nbsp;<a href=\"/" + this._table.Name + "/" + this._table.Name + "_" + this._viewNames.AddRecord + "?returnUrl=/" + this._table.Name + "/" + this._table.Name + "_" + this._viewName + "\">Add New " + this._table.Name + "</a>");
                    sb.AppendLine("<br /><br />");
                    sb.AppendLine("");
                    break;
            }
        }

        private void WriteCrudPart(StringBuilder sb, MVCGridViewPart part)
        {
            switch (part)
            {
                case MVCGridViewPart.JavaScript:
                    if (this._table.IsGenerateMaskPartialView)
                        sb.AppendLine("    <script src=\"~/js/jquery.maskedinput.min.js\" asp-append-version=\"true\"></script>");
                    sb.AppendLine("    @await Html.PartialAsync(\"_ValidationScriptsPartial\")");
                    sb.AppendLine("    <script src=\"~/js/jqgrid-listcrud.js\" asp-append-version=\"true\"></script>");
                    break;
                case MVCGridViewPart.JavaScriptVariablesAndScript:
                    sb.AppendLine("        var addEditTitle = \"" + this._table.Name + "\";");
                    sb.AppendLine("        var urlAndMethod = \"/" + this._table.Name + "/" + this._table.Name + "_" + this._viewName + "\";");
                    sb.AppendLine("");
                    sb.AppendLine("        function addItem() {");
                    sb.AppendLine("            clearFields();");
                    sb.AppendLine("            resetValidationErrors();");
                    sb.AppendLine("            showHideItem(null);");
                    foreach (Column primaryKeyColumn in this._table.PrimaryKeyColumns)
                    {
                        if (!primaryKeyColumn.IsForeignKey && primaryKeyColumn.IsPrimaryKeyUnique)
                            sb.AppendLine("            $(\"#" + primaryKeyColumn.NameCamelStyle + "\").attr('disabled', true);");
                        else if (!primaryKeyColumn.IsForeignKey && !primaryKeyColumn.IsPrimaryKeyUnique)
                            sb.AppendLine("            $(\"#" + primaryKeyColumn.NameCamelStyle + "\").attr('readonly', false);");
                    }
                    sb.AppendLine("        }");
                    sb.AppendLine("");
                    sb.AppendLine("        function editItem(" + Functions.GetCommaDelimitedAllColumnParameters(this._table, Language.CSharp) + ") {");
                    sb.AppendLine("            clearFields();");
                    sb.AppendLine("            resetValidationErrors();");
                    sb.AppendLine("            showHideItem(" + Functions.GetCommaDelimitedPrimaryKeyParameters(this._table, Language.CSharp, true, this._generatedSqlType, false) + ");");
                    sb.AppendLine("");
                    foreach (Column primaryKeyColumn in this._table.PrimaryKeyColumns)
                    {
                        if (primaryKeyColumn.IsPrimaryKeyUnique)
                            sb.AppendLine("            $(\"#" + primaryKeyColumn.NameCamelStyle + "\").attr('disabled', false);");
                        else
                            sb.AppendLine("            $(\"#" + primaryKeyColumn.NameCamelStyle + "\").attr('readonly', true);");
                    }
                    foreach (Column column in this._table.Columns)
                    {
                        string str = "";
                        if (column.SystemType.ToLower() == "string")
                            str = ".replace(/~APOS/g, \"'\").replace(/~QUOT/g, '\"').replace(/~NEWL/g, '\\n')";
                        if (column.SQLDataType == SQLType.bit)
                        {
                            sb.AppendLine("");
                            sb.AppendLine("            if (" + column.NameCamelStyle + ".toLowerCase() == \"yes\")");
                            sb.AppendLine("                $(\"#" + column.NameCamelStyle + "\").prop('checked', true);");
                            sb.AppendLine("            else");
                            sb.AppendLine("                $(\"#" + column.NameCamelStyle + "\").prop('checked', false);");
                        }
                        else
                            sb.AppendLine("            $(\"#" + column.NameCamelStyle + "\").val(" + column.NameCamelStyle + str + ");");
                    }
                    sb.AppendLine(" ");
                    sb.AppendLine("            return false;");
                    sb.AppendLine("        }");
                    sb.AppendLine("");
                    sb.AppendLine("        function clearFields() {");
                    foreach (Column column in this._table.Columns)
                    {
                        if (column.SQLDataType == SQLType.bit)
                            sb.AppendLine("            $(\"#" + column.NameCamelStyle + "\").removeAttr('checked');");
                        else
                            sb.AppendLine("            $(\"#" + column.NameCamelStyle + "\").val('');");
                    }
                    sb.AppendLine("        }");
                    sb.AppendLine("");
                    int num1 = 0;
                    int num2 = this._table.Columns.Count - this._table.PrimaryKeyCount;
                    bool flag = false;
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
                    foreach (Column column in this._table.Columns)
                    {
                        if (column.IsPrimaryKey && !column.IsPrimaryKeyUnique && column.SQLDataType != SQLType.bit)
                            stringBuilder10.Append(column.Name + ", ");
                        else if ((!column.IsPrimaryKey || !column.IsPrimaryKeyUnique) && (column.IsNullable || !column.IsPrimaryKeyUnique) && (!column.IsNullable && column.SQLDataType != SQLType.bit))
                            stringBuilder10.Append(column.Name + ", ");
                        if (column.SystemTypeNative == "DateTime")
                            stringBuilder10.Append(column.Name + ", ");
                        else if ((column.SQLDataType == SQLType.integer || column.SQLDataType == SQLType.bigint || column.SQLDataType == SQLType.smallint) && (!column.IsPrimaryKey && !column.IsForeignKey))
                            stringBuilder10.Append(column.Name + ", ");
                        else if (column.SQLDataType == SQLType.money || column.SQLDataType == SQLType.smallmoney || (column.SQLDataType == SQLType.floatnumber || column.SQLDataType == SQLType.real) || column.SQLDataType == SQLType.decimalnumber)
                            stringBuilder10.Append(column.Name + ", ");
                        if (column.SQLDataType == SQLType.character || column.SQLDataType == SQLType.nchar || (column.SQLDataType == SQLType.nvarchar || column.SQLDataType == SQLType.nvarcharmax) || (column.SQLDataType == SQLType.varchar || column.SQLDataType == SQLType.varcharmax))
                            stringBuilder10.Append(column.Name + ", ");
                    }
                    foreach (Column primaryKeyColumn in this._table.PrimaryKeyColumns)
                    {
                        string str = "";
                        if (primaryKeyColumn.SystemType.ToLower() == "string")
                            str = ".replace(/~APOS/g, \"'\").replace(/~QUOT/g, '\"').replace(/~NEWL/g, '\\n').replace(/'/g, \"\\\\'\")";
                        if (!primaryKeyColumn.IsPrimaryKeyUnique)
                        {
                            stringBuilder2.AppendLine("            var " + primaryKeyColumn.NameCamelStyle + " = $('#" + primaryKeyColumn.NameCamelStyle + "').val();");
                            stringBuilder1.AppendLine("                \"'" + primaryKeyColumn.Name + "':'\" + " + primaryKeyColumn.NameCamelStyle + str + " + \"',\" +");
                            if (stringBuilder10.ToString().Contains(primaryKeyColumn.Name + ", "))
                            {
                                stringBuilder5.AppendLine("            var " + primaryKeyColumn.NameCamelStyle + " = $('#" + primaryKeyColumn.NameCamelStyle + "').val();");
                                stringBuilder7.AppendLine("            $('#" + primaryKeyColumn.NameCamelStyle + "Validation').text('');");
                            }
                        }
                        else
                        {
                            stringBuilder2.AppendLine("            var " + primaryKeyColumn.NameCamelStyle + " = $('#" + primaryKeyColumn.NameCamelStyle + "').val();");
                            stringBuilder1.AppendLine("                \"'" + primaryKeyColumn.Name + "':'\" + " + primaryKeyColumn.NameCamelStyle + str + " + \"',\" +");
                            if (stringBuilder10.ToString().Contains(primaryKeyColumn.Name + ", "))
                                stringBuilder5.AppendLine("            var " + primaryKeyColumn.NameCamelStyle + " = $('#" + primaryKeyColumn.NameCamelStyle + "').val();");
                        }
                        stringBuilder3.AppendLine("            if (" + primaryKeyColumn.NameCamelStyle + " == '')");
                        stringBuilder3.AppendLine("                " + primaryKeyColumn.NameCamelStyle + " = '" + Functions.GetMinimumValueOfPrimaryKey(primaryKeyColumn) + "';");
                    }
                    foreach (Column column in this._table.Columns)
                    {
                        if (!column.IsPrimaryKey)
                        {
                            ++num1;
                            string str = "";
                            if (column.SystemType.ToLower() == "string")
                                str = ".replace(/~APOS/g, \"'\").replace(/~QUOT/g, '\"').replace(/~NEWL/g, '\\n').replace(/'/g, \"\\\\'\")";
                            if (num1 == num2)
                            {
                                if (column.SQLDataType == SQLType.bit)
                                    stringBuilder4.AppendLine("                \"'" + column.Name + "':'\" + $('#" + column.NameCamelStyle + "').is(':checked') + \"'\";");
                                else
                                    stringBuilder4.AppendLine("                \"'" + column.Name + "':'\" + $('#" + column.NameCamelStyle + "').val()" + str + " + \"'\";");
                            }
                            else if (column.SQLDataType == SQLType.bit)
                                stringBuilder4.AppendLine("                \"'" + column.Name + "':'\" + $('#" + column.NameCamelStyle + "').is(':checked') + \"',\" +");
                            else
                                stringBuilder4.AppendLine("                \"'" + column.Name + "':'\" + $('#" + column.NameCamelStyle + "').val()" + str + " + \"',\" +");
                            if (stringBuilder10.ToString().Contains(column.Name + ", "))
                            {
                                stringBuilder6.AppendLine("            var " + column.NameCamelStyle + " = $('#" + column.NameCamelStyle + "').val();");
                                stringBuilder7.AppendLine("            $('#" + column.NameCamelStyle + "Validation').text('');");
                            }
                            else if (column.SQLDataType == SQLType.uniqueidentifier && column.IsNullable)
                            {
                                stringBuilder6.AppendLine("            var " + column.NameCamelStyle + " = $('#" + column.NameCamelStyle + "').val();");
                                stringBuilder7.AppendLine("            $('#" + column.NameCamelStyle + "Validation').text('');");
                                flag = true;
                            }
                        }
                        if (this._viewType == MVCGridViewType.CRUD)
                        {
                            if (column.IsPrimaryKey && !column.IsPrimaryKeyUnique && column.SQLDataType != SQLType.bit)
                                stringBuilder8.AppendLine("            if (" + column.NameCamelStyle + " == '') { isValid = false; $(\"#" + column.NameCamelStyle + "Validation\").text('" + column.NameWithSpaces + " is required.'); }");
                            else if ((!column.IsPrimaryKey || !column.IsPrimaryKeyUnique) && (column.IsNullable || !column.IsPrimaryKeyUnique) && (!column.IsNullable && column.SQLDataType != SQLType.bit))
                                stringBuilder8.AppendLine("            if (" + column.NameCamelStyle + " == '') { isValid = false; $(\"#" + column.NameCamelStyle + "Validation\").text('" + column.NameWithSpaces + " is required.'); }");
                            if (column.SystemTypeNative == "DateTime")
                                stringBuilder8.AppendLine("            if (" + column.NameCamelStyle + " != '' && !isDate(" + column.NameCamelStyle + ")) { isValid = false; $(\"#" + column.NameCamelStyle + "Validation\").text('" + column.NameWithSpaces + " must be a valid date.'); }");
                            else if ((column.SQLDataType == SQLType.integer || column.SQLDataType == SQLType.bigint || column.SQLDataType == SQLType.smallint) && (!column.IsPrimaryKeyUnique && !column.IsForeignKey))
                                stringBuilder8.AppendLine("            if (" + column.NameCamelStyle + " != '' && !isNumeric(" + column.NameCamelStyle + ")) { isValid = false; $(\"#" + column.NameCamelStyle + "Validation\").text('" + column.NameWithSpaces + " must be an integer.'); }");
                            else if (column.SQLDataType == SQLType.money || column.SQLDataType == SQLType.smallmoney || (column.SQLDataType == SQLType.floatnumber || column.SQLDataType == SQLType.real) || column.SQLDataType == SQLType.decimalnumber)
                                stringBuilder8.AppendLine("            if (" + column.NameCamelStyle + " != '' && !isDecimal(" + column.NameCamelStyle + ")) { isValid = false; $(\"#" + column.NameCamelStyle + "Validation\").text('" + column.NameWithSpaces + " must be a decimal.'); }");
                            if (column.SQLDataType == SQLType.integer && !column.IsPrimaryKeyUnique && !column.IsForeignKey)
                                stringBuilder8.AppendLine("            if (" + column.NameCamelStyle + " != '' && isNumeric(" + column.NameCamelStyle + ") && !isWithinInt32Range(" + column.NameCamelStyle + ")) { isValid = false; $(\"#" + column.NameCamelStyle + "Validation\").text('" + column.NameWithSpaces + " must be between " + int.MinValue + " and " + int.MaxValue + ".'); }");
                            else if (column.SQLDataType == SQLType.bigint && !column.IsPrimaryKeyUnique && !column.IsForeignKey)
                                stringBuilder8.AppendLine("            if (" + column.NameCamelStyle + " != '' && isNumeric(" + column.NameCamelStyle + ") && !isWithinInt64Range(" + column.NameCamelStyle + ")) { isValid = false; $(\"#" + column.NameCamelStyle + "Validation\").text('" + column.NameWithSpaces + " must be between " + long.MinValue + " and " + long.MaxValue + ".'); }");
                            else if (column.SQLDataType == SQLType.smallint && !column.IsPrimaryKeyUnique && !column.IsForeignKey)
                                stringBuilder8.AppendLine("            if (" + column.NameCamelStyle + " != '' && isNumeric(" + column.NameCamelStyle + ") && !isWithinInt16Range(" + column.NameCamelStyle + ")) { isValid = false; $(\"#" + column.NameCamelStyle + "Validation\").text('" + column.NameWithSpaces + " must be between " + short.MinValue + " and " + short.MaxValue + ".'); }");
                            else if (column.SQLDataType == SQLType.money)
                                stringBuilder8.AppendLine("            if (" + column.NameCamelStyle + " != '' && isDecimal(" + column.NameCamelStyle + ") && !isWithinMoneyRange(" + column.NameCamelStyle + ")) { isValid = false; $(\"#" + column.NameCamelStyle + "Validation\").text('" + column.NameWithSpaces + " must be a valid decimal.'); }");
                            else if (column.SQLDataType == SQLType.smallmoney)
                                stringBuilder8.AppendLine("            if (" + column.NameCamelStyle + " != '' && isDecimal(" + column.NameCamelStyle + ") && !isWithinSmallMoneyRange(" + column.NameCamelStyle + ")) { isValid = false; $(\"#" + column.NameCamelStyle + "Validation\").text('" + column.NameWithSpaces + " must be a valid decimal.'); }");
                            if (column.SQLDataType == SQLType.character || column.SQLDataType == SQLType.nchar || (column.SQLDataType == SQLType.nvarchar || column.SQLDataType == SQLType.nvarcharmax) || (column.SQLDataType == SQLType.varchar || column.SQLDataType == SQLType.varcharmax))
                                stringBuilder8.AppendLine("            if (" + column.NameCamelStyle + ".length > " + column.Length + ") { isValid = false; $(\"#" + column.NameCamelStyle + "Validation\").text('" + column.NameWithSpaces + " must be " + column.Length + " characters or less.'); }");
                            if (column.SQLDataType == SQLType.uniqueidentifier && !column.IsPrimaryKey)
                            {
                                stringBuilder8.AppendLine("            if (" + column.NameCamelStyle + " != '') {");
                                stringBuilder8.AppendLine("                if (!guidRegexPattern.test(" + column.NameCamelStyle + ")) { isValid = false; $(\"#" + column.NameCamelStyle + "Validation\").text('" + column.NameWithSpaces + " must be a valid guid.'); }");
                                stringBuilder8.AppendLine("            }");
                            }
                        }
                        else
                        {
                            if (column.IsPrimaryKey && !column.IsPrimaryKeyUnique && column.SQLDataType != SQLType.bit)
                                stringBuilder8.AppendLine("            if (" + column.NameCamelStyle + " == '') { isValid = false; $(\"#" + column.NameCamelStyle + "\").text('" + column.NameWithSpaces + " is required.'); }");
                            else if ((!column.IsPrimaryKey || !column.IsPrimaryKeyUnique) && (column.IsNullable || !column.IsPrimaryKeyUnique) && (!column.IsNullable && column.SQLDataType != SQLType.bit))
                                stringBuilder8.AppendLine("            if (" + column.NameCamelStyle + " == '') { isValid = false; $(\"#" + column.NameCamelStyle + "\").text('" + column.NameWithSpaces + " is required.'); }");
                            if (column.SystemTypeNative == "DateTime")
                                stringBuilder8.AppendLine("            if (" + column.NameCamelStyle + " != '' && !isDate(" + column.NameCamelStyle + ")) { isValid = false; $(\"#" + column.NameCamelStyle + "\").text('" + column.NameWithSpaces + " must be a valid date.'); }");
                            else if ((column.SQLDataType == SQLType.integer || column.SQLDataType == SQLType.bigint || column.SQLDataType == SQLType.smallint) && (!column.IsPrimaryKeyUnique && !column.IsForeignKey))
                                stringBuilder8.AppendLine("            if (" + column.NameCamelStyle + " != '' && !isNumeric(" + column.NameCamelStyle + ")) { isValid = false; $(\"#" + column.NameCamelStyle + "\").text('" + column.NameWithSpaces + " must be an integer.'); }");
                            else if (column.SQLDataType == SQLType.money || column.SQLDataType == SQLType.smallmoney || (column.SQLDataType == SQLType.floatnumber || column.SQLDataType == SQLType.real) || column.SQLDataType == SQLType.decimalnumber)
                                stringBuilder8.AppendLine("            if (" + column.NameCamelStyle + " != '' && !isDecimal(" + column.NameCamelStyle + ")) { isValid = false; $(\"#" + column.NameCamelStyle + "\").text('" + column.NameWithSpaces + " must be a decimal.'); }");
                            if (column.SQLDataType == SQLType.integer && !column.IsPrimaryKeyUnique && !column.IsForeignKey)
                                stringBuilder8.AppendLine("            if (" + column.NameCamelStyle + " != '' && isNumeric(" + column.NameCamelStyle + ") && !isWithinInt32Range(" + column.NameCamelStyle + ")) { isValid = false; $(\"#" + column.NameCamelStyle + "Validation\").text('" + column.NameWithSpaces + " must be between " + int.MinValue + " and " + int.MaxValue + ".'); }");
                            else if (column.SQLDataType == SQLType.bigint && !column.IsPrimaryKeyUnique && !column.IsForeignKey)
                                stringBuilder8.AppendLine("            if (" + column.NameCamelStyle + " != '' && isNumeric(" + column.NameCamelStyle + ") && !isWithinInt64Range(" + column.NameCamelStyle + ")) { isValid = false; $(\"#" + column.NameCamelStyle + "Validation\").text('" + column.NameWithSpaces + " must be between " + long.MinValue + " and " + long.MaxValue + ".'); }");
                            else if (column.SQLDataType == SQLType.smallint && !column.IsPrimaryKeyUnique && !column.IsForeignKey)
                                stringBuilder8.AppendLine("            if (" + column.NameCamelStyle + " != '' && isNumeric(" + column.NameCamelStyle + ") && !isWithinInt16Range(" + column.NameCamelStyle + ")) { isValid = false; $(\"#" + column.NameCamelStyle + "Validation\").text('" + column.NameWithSpaces + " must be between " + short.MinValue + " and " + short.MaxValue + ".'); }");
                            else if (column.SQLDataType == SQLType.money)
                                stringBuilder8.AppendLine("            if (" + column.NameCamelStyle + " != '' && isDecimal(" + column.NameCamelStyle + ") && !isWithinMoneyRange(" + column.NameCamelStyle + ")) { isValid = false; $(\"#" + column.NameCamelStyle + "Validation\").text('" + column.NameWithSpaces + " must be a valid decimal.'); }");
                            else if (column.SQLDataType == SQLType.smallmoney)
                                stringBuilder8.AppendLine("            if (" + column.NameCamelStyle + " != '' && isDecimal(" + column.NameCamelStyle + ") && !isWithinSmallMoneyRange(" + column.NameCamelStyle + ")) { isValid = false; $(\"#" + column.NameCamelStyle + "Validation\").text('" + column.NameWithSpaces + " must be a valid decimal.'); }");
                            if (column.SQLDataType == SQLType.character || column.SQLDataType == SQLType.nchar || (column.SQLDataType == SQLType.nvarchar || column.SQLDataType == SQLType.nvarcharmax) || (column.SQLDataType == SQLType.varchar || column.SQLDataType == SQLType.varcharmax))
                                stringBuilder8.AppendLine("            if (" + column.NameCamelStyle + ".length > " + column.Length + ") { isValid = false; $(\"#" + column.NameCamelStyle + "\").text('" + column.NameWithSpaces + " must be " + column.Length + " characters or less.'); }");
                            if (column.SQLDataType == SQLType.uniqueidentifier && !column.IsPrimaryKey)
                            {
                                stringBuilder8.AppendLine("            if (" + column.NameCamelStyle + " != '') {");
                                stringBuilder8.AppendLine("                if (!guidRegexPattern.test(" + column.NameCamelStyle + ")) { isValid = false; $(\"#" + column.NameCamelStyle + "\").text('" + column.NameWithSpaces + " must be a valid guid.'); }");
                                stringBuilder8.AppendLine("            }");
                            }
                        }
                    }
                    sb.AppendLine("        function getSerializedData() {");
                    sb.Append(stringBuilder2.ToString());
                    sb.AppendLine("");
                    sb.Append(stringBuilder3.ToString());
                    sb.AppendLine("");
                    sb.AppendLine("            var serializedData =");
                    sb.Append(stringBuilder1.ToString());
                    sb.Append(stringBuilder4.ToString());
                    sb.AppendLine("");
                    sb.AppendLine("            return serializedData;");
                    sb.AppendLine("        }");
                    sb.AppendLine("");
                    sb.AppendLine("        function isDataValid() {");
                    sb.AppendLine("            var isValid = true;");
                    sb.AppendLine("");
                    sb.AppendLine("            // get item values that needs to be validated");
                    sb.Append(stringBuilder5.ToString());
                    sb.Append(stringBuilder6);
                    sb.AppendLine("");
                    sb.AppendLine("            // clear all validation messages");
                    sb.Append(stringBuilder7.ToString());
                    sb.AppendLine("");
                    sb.AppendLine("            // validation");
                    if (flag)
                    {
                        sb.AppendLine("            var guidRegexPattern = /^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/i;");
                        sb.AppendLine("");
                    }
                    sb.Append(stringBuilder8.ToString());
                    sb.AppendLine("");
                    sb.AppendLine("            if (isValid)");
                    sb.AppendLine("                return true;");
                    sb.AppendLine("            else");
                    sb.AppendLine("                return false;");
                    sb.AppendLine("        }");
                    sb.AppendLine("");
                    break;
                case MVCGridViewPart.JavaScriptInsideDollarFunctionHeader:
                    if (this._table.IsGenerateMaskPartialView)
                    {
                        sb.AppendLine("            // mask specific fields");
                        foreach (Column column in this._table.Columns)
                        {
                            if (!string.IsNullOrEmpty(column.Mask))
                                sb.AppendLine("            $(\"#" + column.NameCamelStyle + "\").mask(\"" + column.Mask + "\");");
                        }
                        sb.AppendLine("");
                    }
                    sb.AppendLine("            InitializeAddUpdateRecordDialog();");
                    sb.AppendLine("            $('.datetextbox').datepicker({ dateFormat: \"m/d/yy\" });");
                    sb.AppendLine("");
                    break;
                case MVCGridViewPart.ColNames:
                    if (this._isUseAuditLogging)
                        sb.AppendLine("                colNames: [" + this._commaDelimitedColNames.ToString() + " '', '', ''],");
                    else
                        sb.AppendLine("                colNames: [" + this._commaDelimitedColNames.ToString() + " '', ''],");
                    sb.AppendLine("                colModel: [");
                    break;
                case MVCGridViewPart.AdditionalColumns:
                    sb.AppendLine("                    { name: 'editoperation', index: 'editoperation', align: 'center', width: 40, sortable: false, formatter: editFormatter },");
                    sb.AppendLine("                    { name: 'deleteoperation', index: 'deleteoperation', align: 'center', width: 40, sortable: false, formatter: deleteFormatter },");
                    if (this._isUseAuditLogging)
                        sb.AppendLine("                    { name: 'auditoperation', index: 'auditoperation', align: 'center', width: 40, sortable: false, formatter: auditFormatter }");
                    break;
                case MVCGridViewPart.AdditionalProperties:
                    string stringWithCommaInTheEnd = string.Empty;
                    sb.AppendLine("                width: '1200',");
                    sb.AppendLine("                gridComplete: function () {");
                    sb.AppendLine("                    var ids = jQuery(\"#list-grid\").jqGrid('getDataIDs');");
                    sb.AppendLine("                    for (var i = 0; i < ids.length; i++) {");
                    sb.AppendLine("                        var currentid = ids[i];");
                    foreach (Column column in this._table.Columns)
                    {
                        if (column.SystemType.ToLower() == "string")
                            sb.AppendLine("                        var " + column.NameCamelStyle + " = jQuery(\"#list-grid\").jqGrid('getCell', ids[i], '" + column.Name + "').replace(/'/g, '~APOS').replace(/\"/g, '~QUOT').replace(/\\n/g, '~NEWL');");
                        else
                            sb.AppendLine("                        var " + column.NameCamelStyle + " = jQuery(\"#list-grid\").jqGrid('getCell', ids[i], '" + column.Name + "');");
                        stringWithCommaInTheEnd = stringWithCommaInTheEnd + "'\" + " + column.NameCamelStyle + " + \"',";
                    }
                    string str1 = Functions.RemoveLastComma(stringWithCommaInTheEnd);
                    if (this._table.PrimaryKeyCount > 1)
                    {
                        int num3 = 0;
                        string str2 = string.Empty;
                        foreach (Column primaryKeyColumn in this._table.PrimaryKeyColumns)
                        {
                            sb.AppendLine("                        var id" + num3 + " = jQuery(\"#list-grid\").jqGrid('getCell', ids[i], '" + primaryKeyColumn.Name + "');");
                            str2 = str2 + " + id" + num3 + " + ";
                            if (num3 + 1 < this._table.PrimaryKeyCount)
                                str2 += "\"/\"";
                            ++num3;
                        }
                        sb.AppendLine("");
                        sb.AppendLine("                        editLink = \"<a href='#' onclick=\\\"editItem(" + str1 + ")\\\"><img src='/images/Edit.gif' style='border:none;' /></a>\";");
                        sb.AppendLine("                        deleteLink = \"<a href='#' onclick=\\\"deleteItem('\"" + str2 + "\"')\\\"><img src='/Images/Delete.png' style='border:none;' /></a>\";");
                        if (this._isUseAuditLogging)
                            sb.AppendLine("                        auditLink=\"<a href='#' onClick=\\\"GetAuditHistory(" + str1 + ")\\\">Audit</a>\";");
                    }
                    else
                    {
                        sb.AppendLine("");
                        sb.AppendLine("                        editLink = \"<a href='#' onclick=\\\"editItem(" + str1 + ")\\\"><img src='/images/Edit.gif' style='border:none;' /></a>\";");
                        sb.AppendLine("                        deleteLink = \"<a href='#' onclick=\\\"deleteItem('\" + currentid + \"')\\\"><img src='/images/Delete.png' style='border:none;' /></a>\";");
                        if (this._isUseAuditLogging)
                            sb.AppendLine("                        auditLink=\"<a href='#' onClick=\\\"GetAuditHistory(\" + currentid + \")\\\">Audit</a>\";");
                    }
                    sb.AppendLine("                        jQuery(\"#list-grid\").jqGrid('setRowData', ids[i], { editoperation: editLink });");
                    sb.AppendLine("                        jQuery(\"#list-grid\").jqGrid('setRowData', ids[i], { deleteoperation: deleteLink });");
                    if (this._isUseAuditLogging)
                        sb.AppendLine("                        jQuery(\"#list-grid\").jqGrid('setRowData', ids[i], { auditoperation: auditLink });");
                    sb.AppendLine("                    }");
                    sb.AppendLine("                }");
                    break;
                case MVCGridViewPart.BodyBeforeGrid:
                    sb.AppendLine("<div id=\"addUpdateRecordDialog\" title=\"Add New " + this._table.Name + "\">");
                    Functions.AppendAddEditRecordMVC(sb, this._table, this._selectedTables, AppendAddEditRecordType.GridView, AppendAddEditRecordContentType.AddEditGridView, this._appVersion, this._viewType);
                    sb.AppendLine("</div>");
                    sb.AppendLine("");
                    sb.AppendLine("<div id=\"errorConfirmationDialog\"></div>");
                    sb.AppendLine("<div id=\"errorDialog\"></div>");
                    sb.AppendLine("");
                    sb.AppendLine("<a href=\"#\" onclick=\"addItem()\"><img src=\"@Url.Content(\"~/images/Add.gif\")\" alt=\"Add New " + this._table.Name + "\" style=\"border: none;\" /></a>&nbsp;<a href=\"#\" onclick=\"addItem()\">Add New " + this._table.Name + "</a>");
                    sb.AppendLine("<br /><br />");
                    sb.AppendLine("");
                    break;
            }
        }

        private void WriteReadOnlyPart(StringBuilder sb, MVCGridViewPart part)
        {
            switch (part)
            {
                case MVCGridViewPart.JavaScriptVariablesAndScript:
                    using (List<Column>.Enumerator enumerator = this._table.Columns.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            Column current = enumerator.Current;
                            if (current.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK)
                            {
                                sb.AppendLine("        function " + current.NameCamelStyle + "Link(cellvalue, options, rowObject) {");
                                sb.AppendLine("            return \"<a href='/" + current.ForeignKeyTable.Name + "/" + current.ForeignKeyTable.Name + "_" + this._viewNames.RecordDetails + "?id=\" + cellvalue + \"&returnUrl=/" + this._table.Name + "/" + this._table.Name + "_" + this._viewName + "'>\" + cellvalue + \"</a>\";");
                                sb.AppendLine("        }");
                                sb.AppendLine("");
                            }
                        }
                        break;
                    }
                case MVCGridViewPart.ColNames:
                    sb.AppendLine("                colNames: [" + Functions.RemoveLastComma(this._commaDelimitedColNames.ToString()) + "],");
                    sb.AppendLine("                colModel: [");
                    break;
                case MVCGridViewPart.AdditionalProperties:
                    sb.AppendLine("                width: '1200'");
                    break;
            }
        }

        private void WriteSearchPart(StringBuilder sb, MVCGridViewPart part)
        {
            switch (part)
            {
                case MVCGridViewPart.Code:
                    StringBuilder stringBuilder1 = new StringBuilder();
                    if (this._table.ForeignKeyCount > 0)
                        sb.AppendLine("");
                    foreach (Column column in this._table.ColumnsWithDropDownListData)
                    {
                        if (!stringBuilder1.ToString().Contains(column.DropDownListDataPropertyName + ","))
                        {
                            sb.AppendLine("    string " + column.NameCamelStyle + "SelectValues = \":\";");
                            stringBuilder1.Append(column.DropDownListDataPropertyName + ",");
                        }
                    }
                    stringBuilder1.Clear();
                    using (IEnumerator<Column> enumerator = this._table.ColumnsWithDropDownListData.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            Column current = enumerator.Current;
                            if (!stringBuilder1.ToString().Contains(current.DropDownListDataPropertyName + ","))
                            {
                                sb.AppendLine("");
                                sb.AppendLine("    foreach (var item in Model." + current.ForeignKeyTable.Name + "DropDownListData.OrderBy(" + current.ForeignKeyTable.LinqFromVariable + " => " + current.ForeignKeyTable.LinqFromVariable + "." + current.ForeignKeyTable.FirstPrimaryKeyName + "))");
                                sb.AppendLine("    {");
                                sb.AppendLine("        " + current.NameCamelStyle + "SelectValues += \";\" + item." + current.ForeignKeyTable.FirstPrimaryKeyName + " + \":\" + item." + current.ForeignKeyTable.FirstPrimaryKeyName + ";");
                                sb.AppendLine("    }");
                                stringBuilder1.Append(current.DropDownListDataPropertyName + ",");
                            }
                        }
                        break;
                    }
                case MVCGridViewPart.JavaScript:
                    sb.AppendLine("    <script src=\"~/js/jqgrid-listsearch.js\" asp-append-version=\"true\"></script>");
                    sb.AppendLine("    <script src=\"~/js/jquery.searchFilter.min.js\" asp-append-version=\"true\"></script>");
                    break;
                case MVCGridViewPart.JavaScriptInsideDollarFunctionHeader:
                    StringBuilder stringBuilder2 = new StringBuilder();
                    if (this._viewType == MVCGridViewType.Search)
                        sb.AppendLine("            var checkBoxSelectValues = \":;True:<input type='checkbox' checked disabled /> True;False:<input type='checkbox' disabled /> False\";");
                    foreach (Column column in this._table.ColumnsWithDropDownListData)
                    {
                        if (!stringBuilder2.ToString().Contains(column.DropDownListDataPropertyName + ","))
                        {
                            sb.AppendLine("            var " + column.NameCamelStyle + "SelectValues = \"@" + column.NameCamelStyle + "SelectValues\";");
                            stringBuilder2.Append(column.DropDownListDataPropertyName + ",");
                        }
                    }
                    sb.AppendLine("");
                    break;
                case MVCGridViewPart.ColNames:
                    sb.AppendLine("                colNames: [" + Functions.RemoveLastComma(this._commaDelimitedColNames.ToString()) + "],");
                    sb.AppendLine("                colModel: [");
                    break;
                case MVCGridViewPart.AdditionalProperties:
                    string empty = string.Empty;
                    sb.AppendLine("                width: '1200',");
                    sb.AppendLine("                ignoreCase: true");
                    sb.AppendLine("            }).jqGrid('navGrid', '#list-pager', { edit: false, add: false, del: false, search: false, refresh: false });");
                    sb.AppendLine("");
                    sb.AppendLine("            $('#list-grid').jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false, beforeSearch: function () {");
                    if (this._table.IsContainsDateFields || this._table.IsContainsNumericFields)
                    {
                        sb.AppendLine("                // verify entered data before searching");
                        sb.AppendLine("                var postData = $('#list-grid').jqGrid('getGridParam', 'postData');");
                        sb.AppendLine("                var searchData = $.parseJSON(postData.filters);");
                        sb.AppendLine("                var isThereValidationErrors = false;");
                        sb.AppendLine("                var validationErrors = \"\";");
                        sb.AppendLine("");
                        sb.AppendLine("                for (var iRule = 0; iRule < searchData.rules.length; iRule++) {");
                        sb.AppendLine("                    var enteredValue = searchData.rules[iRule].data;");
                        sb.AppendLine("");
                        foreach (Column column in this._table.Columns)
                        {
                            if (!column.IsForeignKey && column.SQLDataType != SQLType.bit && column.SystemType.ToLower() != "string")
                            {
                                if (column.SQLDataType == SQLType.decimalnumber || column.SQLDataType == SQLType.money || (column.SQLDataType == SQLType.smallmoney || column.SQLDataType == SQLType.floatnumber) || column.SQLDataType == SQLType.real)
                                {
                                    sb.AppendLine("                    if (searchData.rules[iRule].field == \"" + column.Name + "\" && !isDecimal(enteredValue)) {");
                                    sb.AppendLine("                        validationErrors += \"  " + column.NameWithSpaces + " must be a valid decimal number.\";");
                                    sb.AppendLine("                        isThereValidationErrors = true;");
                                    sb.AppendLine("                    }");
                                }
                                else if (column.SQLDataType == SQLType.numeric || column.SQLDataType == SQLType.integer || (column.SQLDataType == SQLType.smallint || column.SQLDataType == SQLType.bigint))
                                {
                                    sb.AppendLine("                    if (searchData.rules[iRule].field == \"" + column.Name + "\" && !isNumeric(enteredValue)) {");
                                    sb.AppendLine("                        validationErrors += \"  " + column.NameWithSpaces + " must be a valid number.\";");
                                    sb.AppendLine("                        isThereValidationErrors = true;");
                                    sb.AppendLine("                    }");
                                }
                                else if (column.SQLDataType == SQLType.datetime || column.SQLDataType == SQLType.smalldatetime || (column.SQLDataType == SQLType.date || column.SQLDataType == SQLType.datetime2))
                                {
                                    sb.AppendLine("                    if (searchData.rules[iRule].field == \"" + column.Name + "\" && !isDate(enteredValue)) {");
                                    sb.AppendLine("                        validationErrors += \"  " + column.NameWithSpaces + " must be a valid date.\";");
                                    sb.AppendLine("                        isThereValidationErrors = true;");
                                    sb.AppendLine("                    }");
                                }
                            }
                        }
                        sb.AppendLine("                }");
                        sb.AppendLine("");
                        sb.AppendLine("                if(isThereValidationErrors)");
                        sb.AppendLine("                    alert($.trim(validationErrors));");
                        sb.AppendLine("");
                        sb.AppendLine("                return isThereValidationErrors;");
                    }
                    else
                    {
                        sb.AppendLine("                // no entered data to verify before searching");
                        sb.AppendLine("                return false;");
                    }
                    sb.AppendLine("            }");
                    break;
            }
        }

        private void WriteInlinePart(StringBuilder sb, MVCGridViewPart part)
        {
            switch (part)
            {
                case MVCGridViewPart.Code:
                    StringBuilder stringBuilder1 = new StringBuilder();
                    StringBuilder stringBuilder2 = new StringBuilder();
                    foreach (Column column in this._table.Columns)
                    {
                        if (column.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK)
                        {
                            stringBuilder1.AppendLine("    string " + column.NameCamelStyle + "SelectValues = \":\";");
                            stringBuilder2.AppendLine("");
                            stringBuilder2.AppendLine("    foreach (var item in Model." + column.ForeignKeyTable.Name + "DropDownListData.OrderBy(" + column.ForeignKeyTable.LinqFromVariable + " => " + column.ForeignKeyTable.LinqFromVariable + "." + column.ForeignKeyTable.FirstPrimaryKeyName + "))");
                            stringBuilder2.AppendLine("    {");
                            stringBuilder2.AppendLine("        " + column.NameCamelStyle + "SelectValues += \";\" + item." + column.ForeignKeyTable.FirstPrimaryKeyName + " + \":\" + item." + column.ForeignKeyTable.FirstPrimaryKeyName + " + \" - \" + Regex.Replace(item." + column.ForeignKeyTable.DataTextField + " is null ? \"\" : item." + column.ForeignKeyTable.DataTextField + ", \"[^a-zA-Z0-9 -]\", \"\");");
                            stringBuilder2.AppendLine("    }");
                        }
                    }
                    if (this._table.ForeignKeyCount > 0)
                        sb.AppendLine("");
                    sb.Append(stringBuilder1.ToString());
                    sb.Append(stringBuilder2.ToString());
                    break;
                case MVCGridViewPart.JavaScript:
                    sb.AppendLine("    <script src=\"~/js/jqgrid-listinline.js\" asp-append-version=\"true\"></script>");
                    break;
                case MVCGridViewPart.JavaScriptVariablesAndScript:
                    int num1 = 0;
                    int num2 = this._table.Columns.Count - this._table.PrimaryKeyCount;
                    bool flag = false;
                    StringBuilder stringBuilder3 = new StringBuilder();
                    StringBuilder stringBuilder4 = new StringBuilder();
                    StringBuilder stringBuilder5 = new StringBuilder();
                    StringBuilder stringBuilder6 = new StringBuilder();
                    StringBuilder stringBuilder7 = new StringBuilder();
                    StringBuilder stringBuilder8 = new StringBuilder();
                    StringBuilder stringBuilder9 = new StringBuilder();
                    StringBuilder stringBuilder10 = new StringBuilder();
                    foreach (Column column in this._table.Columns)
                    {
                        if (column.IsPrimaryKey && !column.IsPrimaryKeyUnique && column.SQLDataType != SQLType.bit)
                            stringBuilder10.Append(column.Name + ", ");
                        else if ((!column.IsPrimaryKey || !column.IsPrimaryKeyUnique) && (column.IsNullable || !column.IsPrimaryKeyUnique) && (!column.IsNullable && column.SQLDataType != SQLType.bit))
                            stringBuilder10.Append(column.Name + ", ");
                        if (column.SystemTypeNative == "DateTime")
                            stringBuilder10.Append(column.Name + ", ");
                        else if ((column.SQLDataType == SQLType.integer || column.SQLDataType == SQLType.bigint || column.SQLDataType == SQLType.smallint) && (!column.IsPrimaryKey && !column.IsForeignKey))
                            stringBuilder10.Append(column.Name + ", ");
                        else if (column.SQLDataType == SQLType.money || column.SQLDataType == SQLType.smallmoney || (column.SQLDataType == SQLType.floatnumber || column.SQLDataType == SQLType.real) || column.SQLDataType == SQLType.decimalnumber)
                            stringBuilder10.Append(column.Name + ", ");
                        if (column.SQLDataType == SQLType.character || column.SQLDataType == SQLType.nchar || (column.SQLDataType == SQLType.nvarchar || column.SQLDataType == SQLType.nvarcharmax) || (column.SQLDataType == SQLType.varchar || column.SQLDataType == SQLType.varcharmax))
                            stringBuilder10.Append(column.Name + ", ");
                    }
                    foreach (Column primaryKeyColumn in this._table.PrimaryKeyColumns)
                    {
                        string str = "";
                        if (primaryKeyColumn.SystemType.ToLower() == "string")
                            str = ".replace(/~APOS/g, \"'\").replace(/~QUOT/g, '\"').replace(/~NEWL/g, '\\n').replace(/'/g, \"\\\\'\")";
                        if (!primaryKeyColumn.IsPrimaryKeyUnique)
                        {
                            stringBuilder4.AppendLine("            var " + primaryKeyColumn.NameCamelStyle + " = $('#' + currentId + '_" + primaryKeyColumn.Name + "').val();");
                            stringBuilder3.AppendLine("                \"'" + primaryKeyColumn.Name + "':'\" + " + primaryKeyColumn.NameCamelStyle + str + " + \"',\" +");
                            if (stringBuilder10.ToString().Contains(primaryKeyColumn.Name + ", "))
                                stringBuilder7.AppendLine("            var " + primaryKeyColumn.NameCamelStyle + " = $('#' + currentId + '_" + primaryKeyColumn.Name + "').val();");
                        }
                        else
                        {
                            stringBuilder4.AppendLine("            var " + primaryKeyColumn.NameCamelStyle + " = rowData." + primaryKeyColumn.Name + ";");
                            stringBuilder3.AppendLine("                \"'" + primaryKeyColumn.Name + "':'\" + " + primaryKeyColumn.NameCamelStyle + str + " + \"',\" +");
                            if (stringBuilder10.ToString().Contains(primaryKeyColumn.Name + ", "))
                                stringBuilder7.AppendLine("            var " + primaryKeyColumn.NameCamelStyle + " = rowData." + primaryKeyColumn.Name + ";");
                        }
                        stringBuilder5.AppendLine("            if (" + primaryKeyColumn.NameCamelStyle + " == '')");
                        stringBuilder5.AppendLine("                " + primaryKeyColumn.NameCamelStyle + " = '" + Functions.GetMinimumValueOfPrimaryKey(primaryKeyColumn) + "';");
                    }
                    foreach (Column column in this._table.Columns)
                    {
                        if (!column.IsPrimaryKey)
                        {
                            ++num1;
                            string str = "";
                            if (column.SystemType.ToLower() == "string")
                                str = ".replace(/~APOS/g, \"'\").replace(/~QUOT/g, '\"').replace(/~NEWL/g, '\\n').replace(/'/g, \"\\\\'\")";
                            if (num1 == num2)
                            {
                                if (column.SQLDataType == SQLType.bit)
                                    stringBuilder6.AppendLine("                \"'" + column.Name + "':'\" + $('#' + currentId + '_" + column.Name + "').is(':checked') + \"'\";");
                                else
                                    stringBuilder6.AppendLine("                \"'" + column.Name + "':'\" + $('#' + currentId + '_" + column.Name + "').val()" + str + " + \"'\";");
                            }
                            else if (column.SQLDataType == SQLType.bit)
                                stringBuilder6.AppendLine("                \"'" + column.Name + "':'\" + $('#' + currentId + '_" + column.Name + "').is(':checked') + \"',\" +");
                            else
                                stringBuilder6.AppendLine("                \"'" + column.Name + "':'\" + $('#' + currentId + '_" + column.Name + "').val()" + str + " + \"',\" +");
                            if (stringBuilder10.ToString().Contains(column.Name + ", "))
                                stringBuilder8.AppendLine("            var " + column.NameCamelStyle + " = $('#' + currentId + '_" + column.Name + "').val();");
                            if (column.SQLDataType == SQLType.uniqueidentifier && column.IsNullable)
                            {
                                stringBuilder8.AppendLine("            var " + column.NameCamelStyle + " = $('#' + currentId + '_" + column.Name + "').val();");
                                flag = true;
                            }
                        }
                        if (column.IsPrimaryKey && !column.IsPrimaryKeyUnique && column.SQLDataType != SQLType.bit)
                            stringBuilder9.AppendLine("            if (" + column.NameCamelStyle + " == '') errorMessage += '- " + column.NameWithSpaces + " is required.<br/>';");
                        else if ((!column.IsPrimaryKey || !column.IsPrimaryKeyUnique) && (column.IsNullable || !column.IsPrimaryKeyUnique) && (!column.IsNullable && column.SQLDataType != SQLType.bit))
                            stringBuilder9.AppendLine("            if (" + column.NameCamelStyle + " == '') errorMessage += '- " + column.NameWithSpaces + " is required.<br/>';");
                        if (column.SystemTypeNative == "DateTime")
                            stringBuilder9.AppendLine("            if (" + column.NameCamelStyle + " != '' && !isDate(" + column.NameCamelStyle + ")) errorMessage += '- " + column.NameWithSpaces + " must be a valid date.<br/>';");
                        else if ((column.SQLDataType == SQLType.integer || column.SQLDataType == SQLType.bigint || column.SQLDataType == SQLType.smallint) && (!column.IsPrimaryKeyUnique && !column.IsForeignKey))
                            stringBuilder9.AppendLine("            if (" + column.NameCamelStyle + " != '' && !isNumeric(" + column.NameCamelStyle + ")) errorMessage += '- " + column.NameWithSpaces + " must be an integer.<br/>';");
                        else if (column.SQLDataType == SQLType.money || column.SQLDataType == SQLType.smallmoney || (column.SQLDataType == SQLType.floatnumber || column.SQLDataType == SQLType.real) || column.SQLDataType == SQLType.decimalnumber)
                            stringBuilder9.AppendLine("            if (" + column.NameCamelStyle + " != '' && !isDecimal(" + column.NameCamelStyle + ")) errorMessage += '- " + column.NameWithSpaces + " must be a decimal.<br/>';");
                        if (column.SQLDataType == SQLType.character || column.SQLDataType == SQLType.nchar || (column.SQLDataType == SQLType.nvarchar || column.SQLDataType == SQLType.nvarcharmax) || (column.SQLDataType == SQLType.varchar || column.SQLDataType == SQLType.varcharmax))
                            stringBuilder9.AppendLine("            if (" + column.NameCamelStyle + ".length > " + column.Length + ") errorMessage += '- " + column.NameWithSpaces + " must be " + column.Length + " characters or less.<br/>';");
                        if (column.SQLDataType == SQLType.uniqueidentifier && !column.IsPrimaryKey)
                        {
                            stringBuilder9.AppendLine("            if (" + column.NameCamelStyle + " != '') {");
                            stringBuilder9.AppendLine("                if (!guidRegexPattern.test(" + column.NameCamelStyle + ")) errorMessage += '- " + column.NameWithSpaces + " must be a valid guid.<br/>';");
                            stringBuilder9.AppendLine("            }");
                        }
                    }
                    sb.AppendLine("        var urlAndMethod = '/" + this._table.Name + "/" + this._table.Name + "_" + this._viewName + "';");
                    sb.AppendLine("        var errorMessage = '';");
                    sb.AppendLine("");
                    sb.AppendLine("        function getSerializedData(currentId) {");
                    sb.AppendLine("            var rowData = $('#list-grid').jqGrid('getRowData', currentId);");
                    sb.Append(stringBuilder4.ToString());
                    sb.AppendLine("");
                    sb.Append(stringBuilder5.ToString());
                    sb.AppendLine("");
                    sb.AppendLine("            var serializedData =");
                    sb.Append(stringBuilder3.ToString());
                    sb.Append(stringBuilder6.ToString());
                    sb.AppendLine("");
                    sb.AppendLine("            return serializedData;");
                    sb.AppendLine("        }");
                    sb.AppendLine("");
                    sb.AppendLine("        function isDataValid(currentId) {");
                    sb.AppendLine("            var rowData = $('#list-grid').jqGrid('getRowData', currentId);");
                    sb.AppendLine("        ");
                    sb.Append(stringBuilder7.ToString());
                    sb.Append(stringBuilder8);
                    sb.AppendLine("");
                    sb.AppendLine("            // validation");
                    if (flag)
                    {
                        sb.AppendLine("            var guidRegexPattern = /^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/i;");
                        sb.AppendLine("");
                    }
                    sb.Append(stringBuilder9.ToString());
                    sb.AppendLine("");
                    sb.AppendLine("            if (errorMessage == '')");
                    sb.AppendLine("                return true;");
                    sb.AppendLine("            else");
                    sb.AppendLine("                return false;");
                    sb.AppendLine("        }");
                    sb.AppendLine("");
                    break;
                case MVCGridViewPart.JavaScriptInsideDollarFunctionHeader:
                    StringBuilder stringBuilder11 = new StringBuilder();
                    foreach (Column column in this._table.ColumnsWithDropDownListData)
                    {
                        if (!stringBuilder11.ToString().Contains(column.DropDownListDataPropertyName + ","))
                        {
                            sb.AppendLine("            var " + column.NameCamelStyle + "SelectValues = \"@" + column.NameCamelStyle + "SelectValues\";");
                            stringBuilder11.Append(column.DropDownListDataPropertyName + ",");
                        }
                    }
                    sb.AppendLine("");
                    break;
                case MVCGridViewPart.ColNames:
                    if (_isUseAuditLogging)
                        sb.AppendLine("                colNames: [" + Functions.RemoveLastComma(this._commaDelimitedColNames.ToString()) + ", '', '', ''],");
                    else
                        sb.AppendLine("                colNames: [" + Functions.RemoveLastComma(this._commaDelimitedColNames.ToString()) + ", '', ''],");
                    sb.AppendLine("                colModel: [");
                    break;
                case MVCGridViewPart.AdditionalColumns:
                    sb.AppendLine("                    { name: 'editoperation', index: 'editoperation', align: 'center', width: 40, sortable: false, formatter: editFormatter },");
                    sb.AppendLine("                    { name: 'deleteoperation', index: 'deleteoperation', align: 'center', width: 40, sortable: false, formatter: deleteFormatter },");
                    if (_isUseAuditLogging)
                        sb.AppendLine("                    { name: 'auditoperation', index: 'auditoperation', align: 'center', width: 40, sortable: false, formatter: auditFormatter }");
                    break;
                case MVCGridViewPart.AdditionalProperties:
                    sb.AppendLine("                width: '1200',");
                    sb.AppendLine("                gridComplete: function () {");
                    sb.AppendLine("                    var ids = jQuery(\"#list-grid\").jqGrid('getDataIDs');");
                    sb.AppendLine("                    for (var i = 0; i < ids.length; i++) {");
                    sb.AppendLine("                        var currentid = ids[i];");
                    if (this._table.PrimaryKeyCount > 1)
                    {
                        int num3 = 0;
                        string empty = string.Empty;
                        string str1 = string.Empty;
                        foreach (Column primaryKeyColumn in this._table.PrimaryKeyColumns)
                        {
                            sb.AppendLine("                        var id" + num3 + " = jQuery(\"#list-grid\").jqGrid('getCell', ids[i], '" + primaryKeyColumn.Name + "');");
                            str1 = str1 + " + id" + num3 + " + ";
                            if (num3 < this._table.PrimaryKeyCount)
                            {
                                string str2;
                                empty += str2 = "/\" + id" + num3 + " + \"";
                            }
                            if (num3 + 1 < this._table.PrimaryKeyCount)
                                str1 += "\"/\"";
                            ++num3;
                        }
                        sb.AppendLine("                        editLink = \"<a id='editLink\" + currentid + \"' href='#' onclick=\\\"editRow('\" + currentid + \"');\\\"><img src='/images/Edit.gif' style='border:none;' /></a>\" +");
                        sb.AppendLine("                                \"<a id='saveLink\" + currentid + \"' style='display:none;' href='#' onclick=\\\"saveRow('\" + currentid + \"');\\\"><img src='/images/Checked.gif' style='border:none;' /></a>\";");
                        sb.AppendLine("");
                        sb.AppendLine("                        deleteLink = \"<a href='#' id='deleteLink\" + currentid + \"' onclick=\\\"deleteItem('\" + currentid + \"')\\\"><img src='/images/Delete.png' style='border:none;' /></a>\" +");
                        sb.AppendLine("                            \"<a id='cancelLink\" + currentid + \"' style='display:none;' href='#' onclick=\\\"cancelRow('\" + currentid + \"');\\\"><img src='/images/Unchecked.gif' style='border:none;' /></a>\";");
                        if (_isUseAuditLogging)
                            sb.AppendLine("                        auditLink=\"<a href='#' onClick=\\\"GetAuditHistory(\" + currentid + \")\\\">Audit</a>\";");
                    }
                    else
                    {
                        sb.AppendLine("                        editLink = \"<a id='editLink\" + currentid + \"' href='#' onclick=\\\"editRow('\" + currentid + \"', '" + this._viewNames.ListInline + "Update');\\\"><img src='/images/Edit.gif' style='border:none;' /></a>\" +");
                        sb.AppendLine("                                \"<a id='saveLink\" + currentid + \"' style='display:none;' href='#' onclick=\\\"saveRow('\" + currentid + \"');\\\"><img src='/images/Checked.gif' style='border:none;' /></a>\";");
                        sb.AppendLine("");
                        sb.AppendLine("                        deleteLink = \"<a href='#' id='deleteLink\" + currentid + \"' onclick=\\\"deleteItem('\" + currentid + \"')\\\"><img src='/images/Delete.png' style='border:none;' /></a>\" +");
                        sb.AppendLine("                            \"<a id='cancelLink\" + currentid + \"' style='display:none;' href='#' onclick=\\\"cancelRow('\" + currentid + \"');\\\"><img src='/images/Unchecked.gif' style='border:none;' /></a>\";");
                        if (_isUseAuditLogging)
                            sb.AppendLine("                        auditLink=\"<a href='#' onClick=\\\"GetAuditHistory(\" + currentid + \")\\\">Audit</a>\";");
                    }
                    sb.AppendLine("");
                    sb.AppendLine("                        jQuery(\"#list-grid\").jqGrid('setRowData', ids[i], { editoperation: editLink });");
                    sb.AppendLine("                        jQuery(\"#list-grid\").jqGrid('setRowData', ids[i], { deleteoperation: deleteLink });");
                    sb.AppendLine("                        jQuery(\"#list-grid\").jqGrid('setRowData', ids[i], { auditoperation: auditLink });");
                    sb.AppendLine("                    }");
                    sb.AppendLine("                }");
                    break;
                case MVCGridViewPart.BodyBeforeGrid:
                    sb.AppendLine("<div id=\"errorConfirmationDialog\"></div>");
                    sb.AppendLine("<div id=\"errorDialog\"></div>");
                    sb.AppendLine("");
                    sb.AppendLine("<a href=\"#\" id=\"addLink1\" onclick=\"addRow('" + this._viewNames.ListInline + "Add')\"><img src=\"@Url.Content(\"~/images/Add.gif\")\" alt=\"Add New " + this._table.Name + "\" style=\"border: none;\" /></a>&nbsp;<a href=\"#\" id=\"addLink2\" onclick=\"addRow('" + this._viewNames.ListInline + "Add')\">Add New " + this._table.Name + "</a>");
                    sb.AppendLine("<br /><br />");
                    sb.AppendLine("");
                    break;
            }
        }

        private void WriteTotalsPart(StringBuilder sb, MVCGridViewPart part)
        {
            switch (part)
            {
                case MVCGridViewPart.ColNames:
                    sb.AppendLine("                colNames: [" + Functions.RemoveLastComma(this._commaDelimitedColNames.ToString()) + "],");
                    sb.AppendLine("                colModel: [");
                    break;
                case MVCGridViewPart.AdditionalProperties:
                    StringBuilder stringBuilder1 = new StringBuilder();
                    StringBuilder stringBuilder2 = new StringBuilder();
                    foreach (Column column in this._table.Columns)
                    {
                        if (column.SQLDataType == SQLType.decimalnumber || column.SQLDataType == SQLType.money || column.SQLDataType == SQLType.smallmoney)
                        {
                            stringBuilder1.AppendLine("                    var " + column.NameCamelStyle + "Total = $('#list-grid').jqGrid('getCol', '" + column.Name + "', false, 'sum');");
                            stringBuilder2.Append("'" + column.Name + "': " + column.NameCamelStyle + "Total,");
                        }
                    }
                    sb.AppendLine("                width: '1200',");
                    sb.AppendLine("                footerrow : true,");
                    sb.AppendLine("                gridComplete: function() {");
                    if (stringBuilder1.Length > 0)
                    {
                        sb.Append(stringBuilder1);
                        sb.AppendLine("                    $('#list-grid').jqGrid('footerData', 'set', { " + Functions.RemoveLastComma(stringBuilder2.ToString()) + " });");
                    }
                    sb.AppendLine("                }");
                    break;
            }
        }

        private void WriteToolTipPart(StringBuilder sb, MVCGridViewPart part)
        {
            switch (part)
            {
                case MVCGridViewPart.ColNames:
                    int num = 0;
                    bool flag = true;
                    StringBuilder stringBuilder1 = new StringBuilder();
                    foreach (Column column in this._table.Columns)
                        this._colModelNames.Append(column.Name + ",");
                    foreach (Column column1 in this._table.Columns)
                    {
                        if (column1.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK)
                        {
                            foreach (Column column2 in column1.ForeignKeyTable.Columns)
                            {
                                if (!this._colModelNames.ToString().Contains(column2.Name + ","))
                                    ++num;
                            }
                        }
                    }
                    for (int index = 0; index < num; ++index)
                    {
                        if (flag)
                        {
                            stringBuilder1.Append("''");
                            flag = false;
                        }
                        else
                            stringBuilder1.Append(",''");
                    }
                    sb.AppendLine("                colNames: [" + this._commaDelimitedColNames.ToString() + stringBuilder1.ToString() + "],");
                    sb.AppendLine("                colModel: [");
                    break;
                case MVCGridViewPart.AdditionalColumns:
                    StringBuilder stringBuilder2 = new StringBuilder();
                    foreach (Column column1 in this._table.Columns)
                    {
                        if (column1.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK)
                        {
                            foreach (Column column2 in column1.ForeignKeyTable.Columns)
                            {
                                if (!this._colModelNames.ToString().Contains(column2.Name + ","))
                                    stringBuilder2.AppendLine("                    { name: '" + column2.Name + "', index: '" + column2.Name + "', hidden: true },");
                            }
                        }
                    }
                    sb.AppendLine(Functions.RemoveLastComma(stringBuilder2.ToString()));
                    break;
                case MVCGridViewPart.AdditionalProperties:
                    StringBuilder stringBuilder3 = new StringBuilder();
                    StringBuilder stringBuilder4 = new StringBuilder();
                    sb.AppendLine("                width: '1200',");
                    sb.AppendLine("                gridComplete: function () {");
                    sb.AppendLine("                    var ids = jQuery(\"#list-grid\").jqGrid('getDataIDs');");
                    sb.AppendLine("                    for (var i = 0; i < ids.length; i++) {");
                    sb.AppendLine("                        var currentid = ids[i];");
                    foreach (Column column1 in this._table.Columns)
                    {
                        if (column1.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK)
                        {
                            foreach (Column column2 in column1.ForeignKeyTable.Columns)
                                sb.AppendLine("                        var " + column2.NameCamelStyle + " = jQuery(\"#list-grid\").jqGrid('getCell', ids[i], '" + column2.Name + "');");
                            stringBuilder4.AppendLine("                        var tooltipLinkFor" + column1.Name + " = \"\";");
                            stringBuilder3.AppendLine("                        jQuery(\"#list-grid\").jqGrid('setRowData', ids[i], { tooltipOperationFor" + column1.Name + " : tooltipLinkFor" + column1.Name + " });");
                        }
                    }
                    sb.Append(stringBuilder4.ToString());
                    sb.AppendLine("");
                    foreach (Column column1 in this._table.Columns)
                    {
                        if (column1.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK)
                        {
                            sb.AppendLine("                        if (" + Functions.ConvertToCamel(column1.ForeignKeyTable.FirstPrimaryKeyName) + " != \"\") {");
                            sb.AppendLine("                            tooltipLinkFor" + column1.Name + " += \"<a href='#'>\" + " + Functions.ConvertToCamel(column1.ForeignKeyTable.FirstPrimaryKeyName) + " + \"</a>\";");
                            sb.AppendLine("                            tooltipLinkFor" + column1.Name + " += \"<div style='display: none;'>\";");
                            foreach (Column column2 in column1.ForeignKeyTable.Columns)
                                sb.AppendLine("                            tooltipLinkFor" + column1.Name + " += \"\\r\\n" + column2.NameWithSpaces + ": \" + " + column2.NameCamelStyle + ";");
                            sb.AppendLine("                            tooltipLinkFor" + column1.Name + " += \"</div>\";");
                            sb.AppendLine("                        }");
                            sb.AppendLine("");
                        }
                    }
                    sb.Append(stringBuilder3.ToString());
                    sb.AppendLine("                    }");
                    sb.AppendLine("                }");
                    break;
            }
        }

        private void WriteGroupedByPart(StringBuilder sb, MVCGridViewPart part)
        {
            switch (part)
            {
                case MVCGridViewPart.ColNames:
                    sb.AppendLine("                colNames: [" + this._commaDelimitedColNames.ToString() + " ''],");
                    sb.AppendLine("                colModel: [");
                    break;
                case MVCGridViewPart.AdditionalColumns:
                    sb.AppendLine("                    { name: '" + this._currentFktable.DataTextField + "', index: '" + this._currentFktable.DataTextField + "' }");
                    break;
                case MVCGridViewPart.AdditionalProperties:
                    sb.AppendLine("                width: '1200',");
                    sb.AppendLine("                grouping: true,");
                    sb.AppendLine("                groupingView: {");
                    sb.AppendLine("                    groupField: ['" + this._currentFktable.DataTextField + "'],");
                    sb.AppendLine("                    groupColumnShow: [false],");
                    sb.AppendLine("                    groupText: ['<b>{0} ({1} items)</b>'],");
                    if (this._viewType == MVCGridViewType.GroupedByWithTotals)
                    {
                        sb.AppendLine("                    groupDataSorted: true,");
                        sb.AppendLine("                    groupSummary : [true]");
                    }
                    else
                        sb.AppendLine("                    groupDataSorted: true");
                    sb.AppendLine("                }");
                    break;
            }
        }

        private void WriteMasterDetailPart(StringBuilder sb, MVCGridViewPart part, int masterDetailPassCtr, Table currentMasterTable, Table foreignKeyDetailTable, string viewName)
        {
            switch (part)
            {
                case MVCGridViewPart.ColNames:
                    if (masterDetailPassCtr == 1)
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        foreach (Column column in foreignKeyDetailTable.Columns)
                            stringBuilder.Append("'" + column.NameWithSpaces + "',");
                        sb.AppendLine("                colNames: [" + Functions.RemoveLastComma(stringBuilder.ToString()) + "],");
                    }
                    else
                        sb.AppendLine("                colNames: [" + Functions.RemoveLastComma(this._commaDelimitedColNames.ToString()) + "],");
                    sb.AppendLine("                colModel: [");
                    break;
                case MVCGridViewPart.AdditionalProperties:
                    if (this._viewType == MVCGridViewType.MasterDetailGrid && masterDetailPassCtr == 1)
                    {
                        sb.AppendLine("                width: '1200',");
                        sb.AppendLine("                multiselect: false,");
                        sb.AppendLine("                onSelectRow: function (ids) {");
                        sb.AppendLine("                    if (ids != null) {");
                        if (this._isUseWebApi)
                            sb.AppendLine("                        jQuery(\"#list-grid-detail\").jqGrid('setGridParam', { url: \"@Functions.GetWebApiBaseAddress()" + currentMasterTable.Name + "Api/" + this.GetActionName(currentMasterTable) + "/?id=\"  + ids, page: 1 });");
                        else
                            sb.AppendLine("                        jQuery(\"#list-grid-detail\").jqGrid('setGridParam', { url: \"/" + currentMasterTable.Name + "/" + currentMasterTable.Name + "_" + viewName + "?handler=GridDataWithFilters&filters={\\\"groupOp\\\":\\\"AND\\\",\\\"rules\\\":[{\\\"field\\\":\\\"" + this._currentColumn.Name + "\\\",\\\"op\\\":\\\"eq\\\",\\\"data\\\":\\\"\" + ids + \"\\\"}]}\", page: 1 });");
                        sb.AppendLine("                        jQuery(\"#list-grid-detail\").jqGrid('setCaption', \"List of " + Functions.GetNameWithSpaces(currentMasterTable.Name) + " By " + this._currentColumn.NameWithSpaces + ": \" + ids)");
                        sb.AppendLine("                        .trigger('reloadGrid');");
                        sb.AppendLine("                    }");
                        sb.AppendLine("                }");
                        break;
                    }
                    if (this._viewType == MVCGridViewType.MasterDetailGrid && masterDetailPassCtr == 2)
                    {
                        sb.AppendLine("                width: '1200'");
                        break;
                    }
                    if (this._viewType == MVCGridViewType.MasterDetailSubGrid && masterDetailPassCtr == 1)
                    {
                        sb.AppendLine("                width: '1200',");
                        sb.AppendLine("                multiselect: false,");
                        sb.AppendLine("                subGrid: true,");
                        sb.AppendLine("                subGridRowExpanded: SubGridDetail");
                        break;
                    }
                    if (this._viewType != MVCGridViewType.MasterDetailSubGrid || masterDetailPassCtr != 2)
                        break;
                    sb.AppendLine("                width: '1130'");
                    break;
            }
        }

        private string GetActionName(Table currentMasterTable = null)
        {
            if (this._isUseWebApi)
            {
                switch (this._viewType)
                {
                    case MVCGridViewType.Totals:
                        return "SelectSkipAndTakeWithTotals";
                    case MVCGridViewType.Search:
                        return "SelectSkipAndTakeWithFilters";
                    case MVCGridViewType.GroupedBy:
                        return "SelectSkipAndTakeGroupedBy" + this._currentColumn.Name;
                    case MVCGridViewType.GroupedByWithTotals:
                        return "SelectSkipAndTakeTotalsGroupedBy" + this._currentColumn.Name;
                    case MVCGridViewType.MasterDetailGrid:
                        if (currentMasterTable == null)
                            return "SelectSkipAndTake";
                        return "Select" + currentMasterTable.Name + "CollectionBy" + this._currentColumn.Name;
                    case MVCGridViewType.MasterDetailSubGrid:
                        if (currentMasterTable == null)
                            return "SelectSkipAndTake";
                        return "Select" + currentMasterTable.Name + "CollectionBy" + this._currentColumn.Name;
                    default:
                        return "SelectSkipAndTake";
                }
            }
            else
            {
                switch (this._viewType)
                {
                    case MVCGridViewType.Totals:
                        return "GridDataWithTotals";
                    case MVCGridViewType.Search:
                        return "GridDataWithFilters";
                    case MVCGridViewType.GroupedBy:
                        return "GridDataGroupedBy" + this._currentColumn.Name;
                    case MVCGridViewType.GroupedByWithTotals:
                        return "GridDataTotalsGroupedBy" + this._currentColumn.Name;
                    default:
                        return "GridData";
                }
            }
        }

        private void WriteFormatterPart(StringBuilder sb)
        {
            sb.AppendLine(" ");
            sb.AppendLine("        function editFormatter(cellvalue, options, rowObject) {");
            sb.AppendLine("            return cellvalue;");
            sb.AppendLine("        }");
            sb.AppendLine(" ");
            sb.AppendLine("        function deleteFormatter(cellvalue, options, rowObject) {");
            sb.AppendLine("            return cellvalue;");
            sb.AppendLine("        }");
            sb.AppendLine(" ");
            if (_isUseAuditLogging)
            {
                sb.AppendLine("        function auditFormatter(cellvalue, options, rowObject) {");
                sb.AppendLine("            return cellvalue;");
                sb.AppendLine("        }");
                sb.AppendLine(" ");
            }
        }

        private void WriteAuditLogPart(StringBuilder sb, string viewName)
        {
            sb.AppendLine("        function GetAuditHistory(recordID) {");
            sb.AppendLine("            $(\"#audit\").html(\"\");");
            sb.AppendLine("            var AuditDisplay = \"<table class='table table-condensed' cellpadding='5'>\";");
            sb.AppendLine("            $.getJSON(\"/" + this._table.Name + "/" + this._table.Name + "_" + viewName + "?handler=Audit&id=\" + recordID, function (AuditTrail) {");
            sb.AppendLine("                for (var i = 0; i < AuditTrail.length; i++) {");
            sb.AppendLine("                    AuditDisplay = AuditDisplay + \"<tr class='active'><td colspan='2'> Event date: \" + AuditTrail[i].dateTimeStamp + \"</td>\";");
            sb.AppendLine("                    AuditDisplay = AuditDisplay + \"<td> Action type:\" + AuditTrail[i].auditActionTypeName + \" </td></tr> \";");
            sb.AppendLine("                    AuditDisplay = AuditDisplay + \"<tr class='text-warning'><td>Field name</td><td>Before change</td><td>After change</td></tr>\";");
            sb.AppendLine("                    for (var j = 0; j < AuditTrail[i].changes.length; j++) {");
            sb.AppendLine("                        AuditDisplay = AuditDisplay + \" <tr> \";");
            sb.AppendLine("                        AuditDisplay = AuditDisplay + \" <td> \" + AuditTrail[i].changes[j].fieldName + \" </td> \";");
            sb.AppendLine("                        AuditDisplay = AuditDisplay + \" <td> \" + AuditTrail[i].changes[j].valueBefore + \" </td> \";");
            sb.AppendLine("                        AuditDisplay = AuditDisplay + \" <td> \" + AuditTrail[i].changes[j].valueAfter + \" </td> \";");
            sb.AppendLine("                        AuditDisplay = AuditDisplay + \" </tr> \";");
            sb.AppendLine("                    }");
            sb.AppendLine("                }");
            sb.AppendLine("                AuditDisplay = AuditDisplay + \" </table> \";");
            sb.AppendLine("                $(\"#audit\").html(AuditDisplay);");
            sb.AppendLine("                $(\"#myModal\").modal('show');");
            sb.AppendLine("            });");
            sb.AppendLine("        }");
        }
    }
}
