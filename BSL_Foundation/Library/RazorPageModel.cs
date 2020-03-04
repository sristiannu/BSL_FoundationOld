using System.Data;
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
    internal class RazorPageModel
    {
        private const Language _language = Language.CSharp;
        private Table _table;
        private Tables _selectedTables;
        private string _webAppName;
        private string _apiName;
        private string _webApiName;
        private string _webAppRootDirectory;
        private string _apiNameDirectory;
        private string _webApiNameDirectory;
        private const string _fileExtension = ".cs";
        private string _directory;
        private bool _isUseStoredProcedure;
        private IsCheckedView _isCheckedView;
        private ViewNames _viewNamesCollection;
        private DatabaseObjectToGenerateFrom _generateFrom;
        private ApplicationVersion _appVersion;
        private string _businessObjectName;
        private StringBuilder _colModelNames;
        private string _pageTitle;
        private Table _currentFktable;
        private Column _currentColumn;
        private string _currentFkTableName;
        private string _currentFkSingularTableName;
        private ControllerBaseType _controllerBaseType;
        private bool _isUseWebApi;
        private DataTable _referencedTables;
        private string _modelName;
        private bool _isSqlVersion2012OrHigher;
        private GeneratedSqlType _generatedSqlType;
        private MVCGridViewType _viewType;
        private string _viewName;
        private bool _isUseLogging;
        private bool _isUseCaching;
        private bool _isUseAuditLogging;
        private bool _isEmailNotification;

        internal RazorPageModel(MVCGridViewType listViewType, string viewName, Table table, Tables selectedTables, string webAppName, string apiName, string webApiName, string webAppRootDirectory, string apiNameDirectory, string webApiNameDirectory, bool isUseStoredProcedure, bool isSqlVersion2012OrHigher, IsCheckedView isCheckedView, ViewNames viewNamesCollection, DatabaseObjectToGenerateFrom generateFrom, ApplicationVersion appVersion, GeneratedSqlType generatedSqlType, bool isUseLogging, bool isUseCaching, bool isUseAuditLogging, bool isEmailNotification, ControllerBaseType controllerBaseType = ControllerBaseType.ControllerBase, bool isUseWebApi = false, DataTable referencedTables = null)
        {
            this._table = table;
            this._selectedTables = selectedTables;
            this._webAppName = webAppName;
            this._apiName = apiName;
            this._webApiName = webApiName;
            this._webAppRootDirectory = webAppRootDirectory + "Pages\\";
            this._apiNameDirectory = apiNameDirectory;
            this._webApiNameDirectory = webApiNameDirectory;
            this._isUseStoredProcedure = isUseStoredProcedure;
            this._isCheckedView = isCheckedView;
            this._viewNamesCollection = viewNamesCollection;
            this._generateFrom = generateFrom;
            this._appVersion = appVersion;
            this._referencedTables = referencedTables;
            this._colModelNames = new StringBuilder();
            this._controllerBaseType = controllerBaseType;
            this._isUseWebApi = isUseWebApi;
            this._generatedSqlType = generatedSqlType;
            this._viewType = listViewType;
            this._viewName = viewName.Trim();
            this._modelName = Functions.GetFullyQualifiedModelName(table, selectedTables, apiName);
            this._businessObjectName = Functions.GetFullyQualifiedTableName(this._table, this._selectedTables, Language.CSharp, this._table.NameFullyQualifiedBusinessObject, apiName);
            this._isSqlVersion2012OrHigher = isSqlVersion2012OrHigher;
            this._directory = webAppRootDirectory + MyConstants.DirectoryControllerBase;
            this._isUseLogging = isUseLogging;
            this._isUseCaching = isUseCaching;
            this._isUseAuditLogging = isUseAuditLogging;
            this._isEmailNotification = isEmailNotification;
            if (this._viewType == MVCGridViewType.GroupedBy || this._viewType == MVCGridViewType.GroupedByWithTotals || (this._viewType == MVCGridViewType.MasterDetailGrid || this._viewType == MVCGridViewType.MasterDetailSubGrid))
                this.GenerateGroupedByOrMasterDetail();
            else
                this.Generate(null, null);
        }

        private void Generate(Table currentMasterTable = null, Table foreignKeyDetailTable = null)
        {
            string viewName = Functions.GetViewName(this._viewType, this._viewName, this._currentColumn);
            using (StreamWriter streamWriter = new StreamWriter(this._webAppRootDirectory + this._table.Name + "\\" + this._table.Name + "_" + viewName + ".cshtml.cs"))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("using System;");
                sb.AppendLine("using System.Collections.Generic;");
                sb.AppendLine("using System.Linq;");
                sb.AppendLine("using Microsoft.AspNetCore.Mvc;");
                sb.AppendLine("using Microsoft.AspNetCore.Mvc.RazorPages;");
                sb.AppendLine("using " + this._apiName + ".Domain;");
                sb.AppendLine("using " + this._apiName + ".BusinessObject;");
                sb.AppendLine("using Newtonsoft.Json;");

                if (this._isUseLogging)
                    sb.AppendLine("using Application_Components.Logging;");
                if (this._isUseCaching)
                    sb.AppendLine("using Application_Components.Caching;");
                if (this._isUseAuditLogging)
                {
                    sb.AppendLine("using Application_Components.AuditLog;");
                    sb.AppendLine("using Microsoft.AspNetCore.Http;");
                }
                if (this._isEmailNotification)
                {
                    sb.AppendLine("using Application_Components.EmailNotification;");
                }
                if (this._viewType == MVCGridViewType.Add || this._viewType == MVCGridViewType.Update)
                    sb.AppendLine("using " + this._webAppName + ".PartialModels;");
                if (this._isUseWebApi)
                    sb.AppendLine("using System.Net.Http;");

                sb.AppendLine("");
                sb.AppendLine("namespace " + this._webAppName + ".Pages");
                sb.AppendLine("{");
                sb.AppendLine("     [AutoValidateAntiforgeryToken]");
                sb.AppendLine("     public class " + this._table.Name + "_" + viewName + "Model : PageModel");
                sb.AppendLine("     {");

                if (this._viewType == MVCGridViewType.MasterDetailGrid || this._viewType == MVCGridViewType.MasterDetailSubGrid)
                    this.WriteMethods(sb, currentMasterTable, foreignKeyDetailTable);
                else
                    this.WriteMethods(sb, null, null);

                sb.AppendLine("     }");
                sb.AppendLine("}");
                streamWriter.Write(sb.ToString());
            }
        }

        private void GenerateGroupedByOrMasterDetail()
        {
            StringBuilder stringBuilder = new StringBuilder();
            int num1 = 0;
            int num2 = 1;
            foreach (Column column in this._table.ForeignKeyColumnsTableIsSelectedAndOnlyOnePK)
            {
                string name = column.ForeignKeyTable.Name;
                string empty = string.Empty;
                if (stringBuilder.ToString().Contains(column.ForeignKeyTable.Name + ","))
                {
                    ++num1;
                    name += num1.ToString();
                }
                if (this._table.Name == column.ForeignKeyTable.Name)
                    name += column.Name;
                this._currentFkTableName = name;
                if (this._table.Name == column.SingularForeignKeyTableName)
                {
                    this._currentFkSingularTableName = column.SingularForeignKeyTableName + num2;
                    ++num2;
                }
                else
                    this._currentFkSingularTableName = column.SingularForeignKeyTableName;
                this._currentFktable = column.ForeignKeyTable;
                this._currentColumn = column;
                this._pageTitle = "List of " + Functions.GetNameWithSpaces(this._table.Name) + " By " + Functions.GetNameWithSpaces(this._currentFktable.Name);
                if (this._viewType == MVCGridViewType.MasterDetailGrid || this._viewType == MVCGridViewType.MasterDetailSubGrid)
                    this.Generate(this._table, column.ForeignKeyTable);
                else
                    this.Generate(null, null);
            }
        }

        private void GenerateApiControllerBase()
        {
            using (StreamWriter streamWriter = new StreamWriter(this._directory + this._table.Name + MyConstants.WordApi + MyConstants.WordControllerBase + ".cs"))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("using Microsoft.AspNetCore.Mvc;");
                sb.AppendLine("using System;");
                sb.AppendLine("using System.Linq;");
                sb.AppendLine("using " + this._apiName + "." + MyConstants.WordBusinessObject + ";");
                sb.AppendLine("using " + this._apiName + "." + MyConstants.WordModels + ";");
                if (this._appVersion == ApplicationVersion.ProfessionalPlus)
                    sb.AppendLine("using " + this._apiName + ".Domain;");
                sb.AppendLine("using System.Collections.Generic;");
                sb.AppendLine("using Newtonsoft.Json;");
                if (this._controllerBaseType == ControllerBaseType.ApiControllerBase)
                {
                    sb.AppendLine("using System.Net;");
                    sb.AppendLine("using System.Net.Http;");
                    sb.AppendLine("using System.Threading.Tasks;");
                }
                sb.AppendLine("");
                sb.AppendLine("namespace " + this._webApiName + "." + MyConstants.WordApi + MyConstants.WordControllersDotBase);
                sb.AppendLine("{");
                sb.AppendLine("     /// <summary>");
                sb.AppendLine("     /// Base class for " + this._table.Name + MyConstants.WordApi + MyConstants.WordController + ".  Do not make changes to this class,");
                sb.AppendLine("     /// instead, put additional code in the " + this._table.Name + MyConstants.WordApi + MyConstants.WordController + " class");
                sb.AppendLine("     /// </summary>");
                sb.AppendLine("     public class " + this._table.Name + MyConstants.WordApi + MyConstants.WordControllerBase + " : Controller");
                sb.AppendLine("     {");
                if (this._viewType == MVCGridViewType.Add)
                    this.WriteOnPostAdd(sb);
                if (this._viewType == MVCGridViewType.Update)
                    this.WriteOnPostUpdate(sb);
                if (this._viewType == MVCGridViewType.Redirect || this._viewType == MVCGridViewType.CRUD || (this._viewType == MVCGridViewType.Foreach || this._viewType == MVCGridViewType.Inline))
                    this.WriteOnGetRemove(sb);
                if (this._viewType == MVCGridViewType.Search)
                    this.WriteGetFilteredData(sb);
                this.WriteOnGetGridData(sb, "");
                if (this._generateFrom == DatabaseObjectToGenerateFrom.Tables)
                {
                    this.WriteOnGetGridData(sb, "search");
                    this.WriteOnGetGridData(sb, "totals");
                }
                string str = new BusinessObjectBase(this._table, this._selectedTables, this._referencedTables, this._apiName, this._apiNameDirectory, this._generateFrom, this._isUseStoredProcedure, this._appVersion, this._generatedSqlType, BusinessObjectBaseType.MethodsForApiControllerBase).WriteMethodsForControllerBase();
                sb.Append(str);
                sb.AppendLine("     }");
                sb.AppendLine("}");
                streamWriter.Write(sb.ToString());
            }
        }

        private void WriteMethods(StringBuilder sb, Table currentMasterTable = null, Table foreignKeyDetailTable = null)
        {
            if (this._appVersion == ApplicationVersion.ProfessionalPlus)
            {
                if (this._isUseLogging)
                    sb.AppendLine("         ILog _Ilog;");
                if (this._isUseCaching)
                    sb.AppendLine("         IRedisCacheManager _IRediscache;");
                if (this._isUseAuditLogging)
                    sb.AppendLine("         IAuditLog _IAuditLog;");
                if (this._isEmailNotification)
                    sb.AppendLine("IEmail _IEmail;");
                sb.AppendLine("");

                if (this._viewType == MVCGridViewType.Foreach)
                {
                    this.WriteForeachProperties(sb);
                    if (this._isUseLogging || this._isUseCaching || this._isUseAuditLogging || this._isEmailNotification)
                        this.WriteConstructor(sb);
                    this.WriteForeachOnGet(sb);
                }
                else if (this._viewType == MVCGridViewType.Search || this._viewType == MVCGridViewType.Inline)
                {
                    Functions.WriteDropDownListDataProperties(sb, this._table);
                    if (this._isUseLogging || this._isUseCaching || this._isUseAuditLogging || this._isEmailNotification)
                        this.WriteConstructor(sb);
                    this.WriteOnGet(sb);
                }
                else if (this._viewType == MVCGridViewType.CRUD)
                {
                    Functions.WriteBusinessObjectBindProperty(sb, this._table, this._apiName);
                    Functions.WriteDropDownListDataProperties(sb, this._table);
                    if (this._isUseLogging || this._isUseCaching || this._isUseAuditLogging || this._isEmailNotification)
                        this.WriteConstructor(sb);
                    this.WriteListCrudOnGet(sb);
                    this.WriteListCrudOnGetAdd(sb);
                    this.WriteListCrudOnGetUpdate(sb);
                }
                else if (this._viewType == MVCGridViewType.Add || this._viewType == MVCGridViewType.Update)
                {
                    Functions.WriteBusinessObjectBindProperty(sb, this._table, this._apiName);
                    sb.AppendLine("         [BindProperty]");
                    sb.AppendLine("         public string ReturnUrl { get; set; }");
                    sb.AppendLine("");
                    sb.AppendLine("         public AddEdit" + this._table.Name + "PartialModel PartialModel { get; set; }");
                    sb.AppendLine("");
                    if (this._isUseLogging || this._isUseCaching || this._isUseAuditLogging || this._isEmailNotification)
                        this.WriteConstructor(sb);
                    this.WriteOnGet(sb);
                }
                else if (this._viewType == MVCGridViewType.RecordDetails)
                {
                    Functions.WriteBusinessObjectBindProperty(sb, this._table, this._apiName);
                    Functions.WriteReturnUrlBindProperty(sb);
                    if (this._isUseLogging || this._isUseCaching || this._isUseAuditLogging || this._isEmailNotification)
                        this.WriteConstructor(sb);
                    this.WriteOnGet(sb);
                }
                else if (this._viewType == MVCGridViewType.Unbound)
                {
                    Functions.WriteBusinessObjectBindProperty(sb, this._table, this._apiName);
                    Functions.WriteReturnUrlBindProperty(sb);
                    Functions.WriteDropDownListDataProperties(sb, this._table);
                    sb.AppendLine("         public CrudOperation Operation;");
                    sb.AppendLine("");
                    if (this._isUseLogging || this._isUseCaching || this._isUseAuditLogging || this._isEmailNotification)
                        this.WriteConstructor(sb);
                    this.WriteOnGet(sb);
                    this.WriteOnPostSubmit(sb);
                }
                //else if (this._viewType == MVCGridViewType.AssignWorkflowSteps)
                //{
                //    Functions.WriteBusinessObjectBindProperty(sb, this._table, this._apiName);
                //    Functions.WriteReturnUrlBindProperty(sb);
                //    Functions.WriteDropDownListDataProperties(sb, this._table);
                //    sb.AppendLine("         public CrudOperation Operation;");
                //    sb.AppendLine("");
                //    if (this._isUseLogging || this._isUseCaching || this._isUseAuditLogging || this._isEmailNotification)
                //        this.WriteConstructor(sb);
                //    this.WriteOnGet(sb);
                //    this.WriteOnPostSubmit(sb);
                //}

                else if (this._viewType == MVCGridViewType.Redirect)
                {
                    if (this._isUseLogging || this._isUseCaching || this._isUseAuditLogging || this._isEmailNotification)
                        this.WriteConstructor(sb);
                }

                if (this._viewType == MVCGridViewType.Add)
                    this.WriteOnPostAdd(sb);
                if (this._viewType == MVCGridViewType.Update)
                    this.WriteOnPostUpdate(sb);
                if (this._viewType == MVCGridViewType.Redirect || this._viewType == MVCGridViewType.CRUD || (this._viewType == MVCGridViewType.Foreach || this._viewType == MVCGridViewType.Inline))
                    this.WriteOnGetRemove(sb);
                if (this._viewType == MVCGridViewType.CRUD)
                    this.WriteSetData(sb);
                if (this._viewType == MVCGridViewType.Inline)
                {
                    this.WriteListInlineOnGetAdd(sb);
                    this.WriteListInlineOnGetUpdate(sb);
                }
                if (this._viewType == MVCGridViewType.Foreach)
                    this.WriteForeachGridDataAndGetData(sb);
                if (this._isUseWebApi)
                {
                    if (this._generateFrom == DatabaseObjectToGenerateFrom.Tables)
                    {
                        if (this._viewType == MVCGridViewType.Update || this._viewType == MVCGridViewType.RecordDetails)
                            this.WriteGetByPrimaryKeyWebApiCall(sb);
                        if (this._viewType == MVCGridViewType.Add || this._viewType == MVCGridViewType.Update || (this._viewType == MVCGridViewType.CRUD || this._viewType == MVCGridViewType.Inline) || this._viewType == MVCGridViewType.Search)
                            this.WriteGetDropDownListWebApiHttpClientCallsForFks(sb);
                    }
                }
                else
                {
                    if (this._viewType == MVCGridViewType.CRUD || this._viewType == MVCGridViewType.Redirect || (this._viewType == MVCGridViewType.ReadOnly || this._viewType == MVCGridViewType.ScrollLoad) || this._viewType == MVCGridViewType.Inline)
                        this.WriteOnGetGridData(sb, "");
                    if (this._viewType == MVCGridViewType.MasterDetailGrid || this._viewType == MVCGridViewType.MasterDetailSubGrid)
                    {
                        this._table = foreignKeyDetailTable;
                        this._businessObjectName = Functions.GetFullyQualifiedTableName(this._table, this._selectedTables, Language.CSharp, this._table.NameFullyQualifiedBusinessObject, this._apiName);
                        this.WriteOnGetGridData(sb, "");
                        this._table = currentMasterTable;
                        this._businessObjectName = Functions.GetFullyQualifiedTableName(this._table, this._selectedTables, Language.CSharp, this._table.NameFullyQualifiedBusinessObject, this._apiName);
                        this.WriteOnGetGridData(sb, "masterdetail");
                    }
                    else if (this._viewType == MVCGridViewType.Search)
                        this.WriteOnGetGridData(sb, "search");
                    else if (this._viewType == MVCGridViewType.Totals && this._table.IsContainsMoneyOrDecimalField)
                        this.WriteOnGetGridData(sb, "totals");
                    else if (this._viewType == MVCGridViewType.GroupedBy && this._table.ForeignKeyCount > 0)
                        this.WriteOnGetGridData(sb, "groupedby");
                    else if (this._viewType == MVCGridViewType.GroupedByWithTotals && this._table.ForeignKeyCount > 0 && this._table.IsContainsMoneyOrDecimalField)
                        this.WriteOnGetGridData(sb, "totalsgroupedby");
                }

                if (this._isUseAuditLogging)
                {
                    if (this._viewType == MVCGridViewType.Redirect || this._viewType == MVCGridViewType.CRUD || this._viewType == MVCGridViewType.Inline)
                        this.WriteAuditGet(sb);
                }
            }

            if (this._appVersion != ApplicationVersion.Express)
                return;
            Functions.WriteBusinessObjectBindProperty(sb, this._table, this._apiName);
            Functions.WriteReturnUrlBindProperty(sb);
            Functions.WriteDropDownListDataProperties(sb, this._table);
            sb.AppendLine("         public CrudOperation Operation;");
            sb.AppendLine("");
            if (this._isUseLogging || this._isUseCaching || this._isUseAuditLogging || this._isEmailNotification)
                this.WriteConstructor(sb);
            this.WriteOnGet(sb);
            this.WriteOnPostSubmit(sb);
            if (this._isUseAuditLogging)
            {
                if (this._viewType == MVCGridViewType.Redirect || this._viewType == MVCGridViewType.CRUD || this._viewType == MVCGridViewType.Inline)
                    this.WriteAuditGet(sb);
            }
        }

        private void WriteForeachGridDataAndGetData(StringBuilder sb)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Column column in this._table.Columns)
                stringBuilder.AppendLine("                 {\"" + column.Name + "\", \"" + column.NameWithSpaces + "\"},");
            sb.AppendLine("");
            sb.AppendLine("         public void OnGetGridData(string sidx, string sord, int? _page)");
            sb.AppendLine("         {");
            sb.AppendLine("             GetData(sidx, sord, _page);");
            sb.AppendLine("         }");
            sb.AppendLine("");
            sb.AppendLine("         public void GetData(string sidx, string sord, int? _page)");
            sb.AppendLine("         {");
            sb.AppendLine("             int rows = Functions.GetGridNumberOfRows();");
            sb.AppendLine("             int numberOfPagesToShow = Functions.GetGridNumberOfPagesToShow();");
            sb.AppendLine("             int currentPage = _page is null ? 1 : Convert.ToInt32(_page);");
            if (this._generatedSqlType == GeneratedSqlType.EFCore)
                sb.AppendLine("             int startRowIndex = ((currentPage * rows) - rows);");
            else
                sb.AppendLine("             int startRowIndex = ((currentPage * rows) - rows) + 1;");
            if (this._isUseWebApi)
            {
                sb.AppendLine("             int totalRecords = 0;");
                sb.AppendLine("");
                sb.AppendLine("             using (var client = new HttpClient())");
                sb.AppendLine("             {");
                sb.AppendLine("                 client.BaseAddress = new Uri(Functions.GetWebApiBaseAddress());");
                sb.AppendLine("                 HttpResponseMessage response = client.GetAsync(\"" + this._table.Name + "Api/GetRecordCount/\").Result;");
                sb.AppendLine("");
                sb.AppendLine("                 if (!response.IsSuccessStatusCode)");
                sb.AppendLine("                 {");
                sb.AppendLine("                     throw new Exception(\"Error Status Code: \" + response.StatusCode.ToString() + \" Error Reason: \" + response.ReasonPhrase + \" Error Message: \" + response.RequestMessage.ToString());");
                sb.AppendLine("                 }");
                sb.AppendLine("                 else");
                sb.AppendLine("                 {");
                sb.AppendLine("                     var responseBody = response.Content.ReadAsStringAsync().Result;");
                sb.AppendLine("                     totalRecords = JsonConvert.DeserializeObject<int>(responseBody);");
                sb.AppendLine("                 }");
                sb.AppendLine("             }");
                sb.AppendLine("");
                sb.AppendLine("             int totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);");
                sb.AppendLine("             List<" + this._businessObjectName + "> " + this._table.VariableObjCollectionName + " = null;");
                sb.AppendLine("");
                sb.AppendLine("             using (var client = new HttpClient())");
                sb.AppendLine("             {");
                sb.AppendLine("                 client.BaseAddress = new Uri(Functions.GetWebApiBaseAddress());");
                sb.AppendLine("                 bool isforJqGrid = false;");
                sb.AppendLine("                 HttpResponseMessage response = client.GetAsync(\"" + this._table.Name + "Api/SelectSkipAndTake/?rows=\" + rows + \"&startRowIndex=\" + startRowIndex + \"&_page=\" + currentPage + \"&sidx=\" + sidx + \"&sord=\" + sord + \"&isforJqGrid=\" + isforJqGrid).Result;");
                sb.AppendLine("");
                sb.AppendLine("                 if (!response.IsSuccessStatusCode)");
                sb.AppendLine("                 {");
                sb.AppendLine("                     throw new Exception(\"Error Status Code: \" + response.StatusCode.ToString() + \" Error Reason: \" + response.ReasonPhrase + \" Error Message: \" + response.RequestMessage.ToString());");
                sb.AppendLine("                 }");
                sb.AppendLine("                 else");
                sb.AppendLine("                 {");
                sb.AppendLine("                     var responseBody = response.Content.ReadAsStringAsync().Result;");
                sb.AppendLine("                     " + this._table.VariableObjCollectionName + " = JsonConvert.DeserializeObject<List<" + this._businessObjectName + ">>(responseBody);");
                sb.AppendLine("                 }");
                sb.AppendLine("             }");
            }
            else
            {
                sb.AppendLine("             int totalRecords = " + this._businessObjectName + ".GetRecordCount();");
                sb.AppendLine("             int totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);");
                sb.AppendLine("             List<" + this._businessObjectName + "> " + this._table.VariableObjCollectionName + " = " + this._businessObjectName + ".SelectSkipAndTake(rows, startRowIndex, sidx + \" \" + sord);");
            }
            sb.AppendLine("");
            sb.AppendLine("             // fields and titles");
            sb.AppendLine("             string[,] fieldNames = new string[,] {");
            sb.Append(Functions.RemoveLastComma(stringBuilder.ToString()));
            sb.AppendLine("");
            sb.AppendLine("             };");
            sb.AppendLine("");
            if (this._table.IsContainsBinaryOrSpatialDataTypes)
            {
                sb.AppendLine("             List<string> unsortableFields = new List<string>();");
                foreach (Column column in this._table.Columns)
                {
                    if (column.IsBinaryOrSpatialDataType)
                        sb.AppendLine("             unsortableFields.Add(\"" + column.Name + "\");");
                }
                sb.AppendLine("");
            }
            sb.AppendLine("             // assign properties");
            sb.AppendLine("             " + this._table.Name + "Data = " + this._table.VariableObjCollectionName + ";");
            sb.AppendLine("             " + this._table.Name + "FieldNames = fieldNames;");
            sb.AppendLine("             TotalPages = totalPages;");
            sb.AppendLine("             CurrentPage = currentPage;");
            sb.AppendLine("             FieldToSort = String.IsNullOrEmpty(sidx) ? \"" + this._table.FirstPrimaryKeyName + "\" : sidx;");
            sb.AppendLine("             FieldSortOrder = String.IsNullOrEmpty(sord) ? \"asc\" : sord;");
            sb.AppendLine("             FieldToSortWithOrder = String.IsNullOrEmpty(sidx) ? \"" + this._table.FirstPrimaryKeyName + "\" : (sidx + \" \" + sord).Trim();");
            sb.AppendLine("             NumberOfPagesToShow = numberOfPagesToShow;");
            sb.AppendLine("             StartPage = Functions.GetPagerStartPage(currentPage, numberOfPagesToShow, totalPages);");
            sb.AppendLine("             EndPage = Functions.GetPagerEndPage(StartPage, currentPage, numberOfPagesToShow, totalPages);");
            if (this._table.IsContainsBinaryOrSpatialDataTypes)
                sb.AppendLine("             UnsortableFields = unsortableFields;");
            sb.AppendLine("         }");
        }

        private void WriteListInlineOnGetAdd(StringBuilder sb)
        {
            sb.AppendLine("");
            sb.AppendLine("         /// <summary>");
            sb.AppendLine("         /// Handler, adds a new record.");
            sb.AppendLine("         /// </summary>");
            sb.AppendLine("         public IActionResult OnGetAdd(string serializedData)");
            sb.AppendLine("         {");
            sb.AppendLine("             " + this._table.Name + " " + this._table.VariableObjName + " = JsonConvert.DeserializeObject<" + this._table.Name + ">(serializedData);");
            if (this._isUseAuditLogging && !this._isUseWebApi)
                sb.AppendLine("             " + this._table.Name + "Functions.AddOrEdit(" + this._table.VariableObjName + ", CrudOperation.Add, _IAuditLog, true);");
            else
                sb.AppendLine("             " + this._table.Name + "Functions.AddOrEdit(" + this._table.VariableObjName + ", CrudOperation.Add, true);");
            sb.AppendLine("             return new JsonResult(true);");
            sb.AppendLine("         }");
        }

        private void WriteListInlineOnGetUpdate(StringBuilder sb)
        {
            sb.AppendLine("");
            sb.AppendLine("         /// <summary>");
            sb.AppendLine("         /// Handler, updates a record.");
            sb.AppendLine("         /// </summary>");
            if (this._table.PrimaryKeyCount > 1)
                sb.AppendLine("         public IActionResult OnGetUpdate(" + Functions.GetCommaDelimitedPrimaryKeysAndType(this._table, Language.CSharp) + ", string serializedData)");
            else
                sb.AppendLine("         public IActionResult OnGetUpdate(" + this._table.FirstPrimaryKeySystemType + " id, string serializedData)");
            sb.AppendLine("         {");
            sb.AppendLine("             " + this._table.Name + " " + this._table.VariableObjName + " = JsonConvert.DeserializeObject<" + this._table.Name + ">(serializedData);");
            if (this._isUseAuditLogging && !this._isUseWebApi)
                sb.AppendLine("             " + this._table.Name + "Functions.AddOrEdit(" + this._table.VariableObjName + ", CrudOperation.Update, _IAuditLog, true);");
            else
                sb.AppendLine("             " + this._table.Name + "Functions.AddOrEdit(" + this._table.VariableObjName + ", CrudOperation.Update, true);");
            sb.AppendLine("             return new JsonResult(true);");
            sb.AppendLine("         }");
        }

        private void WriteUnboundGet(StringBuilder sb)
        {
            sb.AppendLine("");
            sb.AppendLine("         /// <summary>");
            sb.AppendLine("         /// GET: /" + this._table.Name + "/" + this._viewNamesCollection.Unbound);
            sb.AppendLine("         /// </summary>");
            sb.AppendLine("         public IActionResult " + this._viewNamesCollection.Unbound + "()");
            sb.AppendLine("         {");
            sb.AppendLine("             return View(GetUnboundViewModel());");
            sb.AppendLine("         }");
        }

        private void WriteDetailsGet(StringBuilder sb)
        {
            sb.AppendLine("");
            sb.AppendLine("         /// <summary>");
            sb.AppendLine("         /// GET: /" + this._table.Name + "/" + this._viewNamesCollection.RecordDetails + "/5");
            sb.AppendLine("         /// </summary>");
        }

        private void WriteUnboundPost(StringBuilder sb)
        {
            sb.AppendLine("");
            sb.AppendLine("         /// <summary>");
            sb.AppendLine("         /// POST: /" + this._table.Name + "/" + this._viewNamesCollection.Unbound);
            sb.AppendLine("         /// </summary>");
            sb.AppendLine("         [HttpPost]");
            sb.AppendLine("         public IActionResult " + this._viewNamesCollection.Unbound + "(" + this._table.Name + MyConstants.WordPropertyModel + " viewModel, string returnUrl)");
            sb.AppendLine("         {");
            sb.AppendLine("             if (ModelState.IsValid)");
            sb.AppendLine("             {");
            sb.AppendLine("                 // do something here before redirecting");
            sb.AppendLine("");
            if (this._appVersion == ApplicationVersion.ProfessionalPlus)
            {
                sb.AppendLine("                 if (Url.IsLocalUrl(returnUrl))");
                sb.AppendLine("                     return Redirect(returnUrl);");
                sb.AppendLine("                 else");
                sb.AppendLine("                     return RedirectToAction(\"Index\", \"Home\");");
            }
            else
                sb.AppendLine("                 return RedirectToAction(\"/Home\");");
            sb.AppendLine("             }");
            sb.AppendLine("");
            sb.AppendLine("             // if we got this far, something failed, redisplay form");
            sb.AppendLine("             return View(GetUnboundViewModel());");
            sb.AppendLine("         }");
        }

        private void WriteAddGet(StringBuilder sb)
        {
            sb.AppendLine("");
            sb.AppendLine("         /// <summary>");
            sb.AppendLine("         /// GET: /" + this._table.Name + "/" + this._viewNamesCollection.AddRecord);
            sb.AppendLine("         /// </summary>");
            sb.AppendLine("         public IActionResult " + this._viewNamesCollection.AddRecord + "()");
            sb.AppendLine("         {");
            sb.AppendLine("             return GetAddViewModel();");
            sb.AppendLine("         }");
        }

        private void WriteConstructor(StringBuilder sb)
        {
            string paramlist = string.Empty;

            if (this._isUseLogging)
                paramlist = paramlist + (paramlist != "" ? "," : "") + "ILog Ilog";

            if (this._isUseCaching)
                paramlist = paramlist + (paramlist != "" ? "," : "") + "IRedisCacheManager IRediscache";

            if (this._isUseAuditLogging)
                paramlist = paramlist + (paramlist != "" ? "," : "") + "IAuditLog IAuditLog";
            if (this._isEmailNotification)
                paramlist = paramlist + (paramlist != "" ? "," : "") + "IEmail IEmail";
            sb.AppendLine("");
            sb.AppendLine("         /// <summary>");
            sb.AppendLine("         /// Default Constructor: /" + this._table.Name + "_" + this._viewName + "Model");
            sb.AppendLine("         /// </summary>");
            sb.AppendLine("         public " + this._table.Name + "_" + this._viewName + "Model" + " (" + paramlist + ")");
            sb.AppendLine("         {");
            if (this._isUseLogging)
            {
                sb.AppendLine("             if (_Ilog == null)");
                sb.AppendLine("                 _Ilog = Ilog;");
            }
            if (this._isUseCaching)
            {
                sb.AppendLine("             if (_IRediscache == null)");
                sb.AppendLine("                 _IRediscache = IRediscache;");
            }
            if (this._isUseAuditLogging)
            {
                sb.AppendLine("             if (_IAuditLog == null)");
                sb.AppendLine("                 _IAuditLog = IAuditLog;");
            }
            if (this._isEmailNotification)
            {
                sb.AppendLine("if (_IEmail == null)");
                sb.AppendLine("_IEmail = IEmail;");
            }
            sb.AppendLine("         }");
            sb.AppendLine("");
        }

        private void WriteOnPostAdd(StringBuilder sb)
        {
            if (this._controllerBaseType == ControllerBaseType.ApiControllerBase)
            {
                sb.AppendLine("         /// <summary>");
                sb.AppendLine("         /// Inserts/Adds/Creates a new record in the database");
                sb.AppendLine("         /// </summary>");
                sb.AppendLine("         /// <param name=\"model\">Pass the " + this._table.Name + MyConstants.WordModel + " here.  Arrives as " + this._table.Name + "Fields which automatically strips the data annotations from the " + this._table.Name + MyConstants.WordModel + ".</param>");
                sb.AppendLine("         /// <returns>IActionResult</returns>");
                sb.AppendLine("         [HttpPost]");
                sb.AppendLine("         public IActionResult Insert([FromBody]" + this._modelName + " model, bool isForListInline = false)");
                sb.AppendLine("         {");
                sb.AppendLine("             return AddEdit" + this._table.Name + "(model, CrudOperation.Add, isForListInline);");
                sb.AppendLine("         }");
            }
            else
            {
                sb.AppendLine("");
                sb.AppendLine("         public IActionResult OnPostAdd()");
                sb.AppendLine("         {");
                sb.AppendLine("             if (ModelState.IsValid)");
                sb.AppendLine("             {");
                sb.AppendLine("                 try");
                sb.AppendLine("                 {");
                sb.AppendLine("                     // add new record");
                if (this._isUseAuditLogging && !this._isUseWebApi)
                    sb.AppendLine("                     " + this._table.Name + "Functions.AddOrEdit(" + this._table.Name + ", CrudOperation.Add, _IAuditLog);");
                else
                    sb.AppendLine("                     " + this._table.Name + "Functions.AddOrEdit(" + this._table.Name + ", CrudOperation.Add);");
                sb.AppendLine("                     return RedirectToPage(ReturnUrl);");
                sb.AppendLine("                 }");
                sb.AppendLine("                 catch(Exception ex)");
                sb.AppendLine("                 {");
                if (this._isUseLogging)
                    sb.AppendLine("                     _Ilog.GetInstance().Error(\"Error Occured\", ex);");
                sb.AppendLine("");
                sb.AppendLine("                     if (ex.InnerException != null)");
                sb.AppendLine("                         ModelState.AddModelError(\"\", ex.InnerException.Message);");
                sb.AppendLine("                     else");
                sb.AppendLine("                         ModelState.AddModelError(\"\", ex.Message);");
                sb.AppendLine("                 }");
                sb.AppendLine("             }");
                sb.AppendLine("");
                sb.AppendLine("             // if we got this far, something failed, redisplay form");
                sb.AppendLine("             return LoadPage(ReturnUrl);");
                sb.AppendLine("         }");
            }
        }

        private void WriteOnPostSubmit(StringBuilder sb)
        {
            sb.AppendLine("");
            sb.AppendLine("         public IActionResult OnPostSubmit()");
            sb.AppendLine("         {");
            sb.AppendLine("             if (ModelState.IsValid)");
            sb.AppendLine("             {");
            sb.AppendLine("                 try");
            sb.AppendLine("                 {");
            sb.AppendLine("                     // add new record (or update existing record) here");
            sb.AppendLine("");
            sb.AppendLine("                     // redirect to another page");
            sb.AppendLine("                     return RedirectToPage(\"\");");
            sb.AppendLine("                 }");
            sb.AppendLine("                 catch(Exception ex)");
            sb.AppendLine("                 {");
            if (this._isUseLogging)
                sb.AppendLine("                     _Ilog.GetInstance().Error(\"Error Occured\", ex);");
            sb.AppendLine("");
            sb.AppendLine("                     if (ex.InnerException != null)");
            sb.AppendLine("                         ModelState.AddModelError(\"\", ex.InnerException.Message);");
            sb.AppendLine("                     else");
            sb.AppendLine("                         ModelState.AddModelError(\"\", ex.Message);");
            sb.AppendLine("                 }");
            sb.AppendLine("             }");
            sb.AppendLine("");
            sb.AppendLine("             // if we got this far, something failed, redisplay form");
            sb.AppendLine("             return Page();");
            sb.AppendLine("         }");
        }

        private void WriteUpdateGet(StringBuilder sb)
        {
            sb.AppendLine("");
            sb.AppendLine("         /// <summary>");
            sb.AppendLine("         /// GET: /" + this._table.Name + "/" + this._viewNamesCollection.UpdateRecord + "/5");
            sb.AppendLine("         /// </summary>");
            if (this._table.PrimaryKeyCount > 1)
                sb.AppendLine("         public IActionResult " + this._viewNamesCollection.UpdateRecord + "(" + Functions.GetCommaDelimitedPrimaryKeysAndType(this._table, Language.CSharp) + ")");
            else
                sb.AppendLine("         public IActionResult " + this._viewNamesCollection.UpdateRecord + "(" + this._table.FirstPrimaryKeySystemType + " id)");
            sb.AppendLine("         {");
            if (this._table.PrimaryKeyCount > 1)
                sb.AppendLine("             return GetUpdateViewModel(" + Functions.GetCommaDelimitedPrimaryKeys(this._table, Language.CSharp, true, false, false) + ");");
            else
                sb.AppendLine("             return GetUpdateViewModel(id);");
            sb.AppendLine("         }");
        }

        private void WriteOnPostUpdate(StringBuilder sb)
        {
            sb.AppendLine("");
            if (this._controllerBaseType == ControllerBaseType.ApiControllerBase)
            {
                sb.AppendLine("         /// <summary>");
                sb.AppendLine("         /// Updates an existing record in the database by primary key.  Pass the primary key in the " + this._table.Name + MyConstants.WordModel);
                sb.AppendLine("         /// </summary>");
                sb.AppendLine("         /// <param name=\"model\">Pass the " + this._table.Name + MyConstants.WordModel + " here.  Arrives as " + this._table.Name + "Fields which automatically strips the data annotations from the " + this._table.Name + MyConstants.WordModel + ".</param>");
                sb.AppendLine("         /// <returns>IActionResult</returns>");
                sb.AppendLine("         [HttpPost]");
                sb.AppendLine("         public IActionResult Update([FromBody]" + this._modelName + " model, bool isForListInline = false)");
                sb.AppendLine("         {");
                sb.AppendLine("             return AddEdit" + this._table.Name + "(model, CrudOperation.Update, isForListInline);");
                sb.AppendLine("         }");
            }
            else
            {
                sb.AppendLine("         public IActionResult OnPostUpdate()");
                sb.AppendLine("         {");
                sb.AppendLine("             if (ModelState.IsValid)");
                sb.AppendLine("             {");
                sb.AppendLine("                 try");
                sb.AppendLine("                 {");
                sb.AppendLine("                     // update record");
                if (this._isUseAuditLogging && !this._isUseWebApi)
                    sb.AppendLine("                     " + this._table.Name + "Functions.AddOrEdit(" + this._table.Name + ", CrudOperation.Update, _IAuditLog);");
                else
                    sb.AppendLine("                     " + this._table.Name + "Functions.AddOrEdit(" + this._table.Name + ", CrudOperation.Update);");
                sb.AppendLine("                     return RedirectToPage(ReturnUrl);");
                sb.AppendLine("                 }");
                sb.AppendLine("                 catch(Exception ex)");
                sb.AppendLine("                 {");
                if (this._isUseLogging)
                    sb.AppendLine("                     _Ilog.GetInstance().Error(\"Error Occured\", ex);");
                sb.AppendLine("");
                sb.AppendLine("                     if (ex.InnerException != null)");
                sb.AppendLine("                         ModelState.AddModelError(\"\", ex.InnerException.Message);");
                sb.AppendLine("                     else");
                sb.AppendLine("                         ModelState.AddModelError(\"\", ex.Message);");
                sb.AppendLine("                 }");
                sb.AppendLine("             }");
                sb.AppendLine("");
                sb.AppendLine("             // if we got this far, something failed, redisplay form");
                if (this._table.PrimaryKeyCount > 1)
                {
                    sb.AppendLine("             return LoadPage(" + Functions.GetCommaDelimitedTableNameDotPrimaryKeys(this._table) + ", ReturnUrl);");
                }
                else
                {
                    string str = string.Empty;
                    if (this._table.FirstPrimaryKeySQLDataType == SQLType.uniqueidentifier)
                        str = ".ToString()";
                    sb.AppendLine("             return LoadPage(" + this._table.Name + "." + this._table.FirstPrimaryKeyName + str + ", ReturnUrl);");
                }
                sb.AppendLine("         }");
            }
        }

        private void WriteOnGetRemove(StringBuilder sb)
        {
            string str1 = "id";
            string str2 = this._table.FirstPrimaryKeySystemType + " id";
            StringBuilder stringBuilder = new StringBuilder();
            if (this._table.PrimaryKeyCount > 1)
            {
                str1 = Functions.GetCommaDelimitedPrimaryKeys(this._table, Language.CSharp, true, false, false);
                str2 = Functions.GetCommaDelimitedPrimaryKeysAndType(this._table, Language.CSharp);
                foreach (Column primaryKeyColumn in this._table.PrimaryKeyColumns)
                    stringBuilder.AppendLine("         /// <param name=\"" + primaryKeyColumn.NameCamelStyle + "\">" + primaryKeyColumn.Name + "</param>");
            }
            else
                stringBuilder.AppendLine("         /// <param name=\"id\">" + this._table.FirstPrimaryKeyName + "</param>");
            if (this._controllerBaseType == ControllerBaseType.ApiControllerBase)
            {
                sb.AppendLine("");
                sb.AppendLine("         /// <summary>");
                sb.AppendLine("         /// Deletes an existing record by primary key.");
                sb.AppendLine("         /// </summary>");
                sb.Append(stringBuilder.ToString());
                sb.AppendLine("         /// <returns>IActionResult</returns>");
                sb.AppendLine("         [HttpDelete]");
                sb.AppendLine("         public IActionResult Delete(" + str2 + ")");
                sb.AppendLine("         {");
                sb.AppendLine("             try");
                sb.AppendLine("             {");
                sb.AppendLine("                 " + this._businessObjectName + ".Delete(" + str1 + ");");
                sb.AppendLine("                 return Ok();");
                sb.AppendLine("             }");
                sb.AppendLine("             catch (Exception ex)");
                sb.AppendLine("             {");
                sb.AppendLine("                 return BadRequest(\"Error Message: \" + ex.Message + \" Stack Trace: \" + ex.StackTrace);");
                sb.AppendLine("             }");
                sb.AppendLine("         }");
            }
            else
            {
                sb.AppendLine("");
                sb.AppendLine("         /// <summary>");
                sb.AppendLine("         /// Handler, deletes a record.");
                sb.AppendLine("         /// </summary>");
                sb.AppendLine("         public IActionResult OnGetRemove(" + str2 + ")");
                sb.AppendLine("         {");
                if (this._isUseWebApi && this._controllerBaseType == ControllerBaseType.ControllerBase)
                {
                    string str3 = "id";
                    string str4 = string.Empty;
                    if (this._table.PrimaryKeyCount > 1)
                    {
                        str3 = Functions.GetQueryStringDelimitedPrimaryKeys(this._table, Language.CSharp, null);
                        str4 = "?";
                    }
                    sb.AppendLine("             using (var client = new HttpClient())");
                    sb.AppendLine("             {");
                    sb.AppendLine("                 client.BaseAddress = new Uri(Functions.GetWebApiBaseAddress());");
                    sb.AppendLine("                 HttpResponseMessage response = client.DeleteAsync(\"" + this._table.Name + "Api/Delete/" + str4 + "\" + " + str3 + ").Result;");
                    sb.AppendLine("");
                    sb.AppendLine("                 if (!response.IsSuccessStatusCode)");
                    sb.AppendLine("                     throw new Exception(\"Error Status Code: \" + response.StatusCode.ToString() + \" Error Reason: \" + response.ReasonPhrase + \" Error Message: \" + response.RequestMessage.ToString());");
                    sb.AppendLine("");
                    sb.AppendLine("                 return new JsonResult(true);");
                    sb.AppendLine("             }");
                }
                else
                {
                    sb.AppendLine("             " + this._table.Name + " " + this._table.Name + " = " + this._table.Name + ".SelectByPrimaryKey(id);");
                    sb.AppendLine("             " + this._businessObjectName + ".Delete(" + str1 + ");");
                    if (this._isUseAuditLogging && !this._isUseWebApi)
                        sb.AppendLine("              _IAuditLog.CreateAuditTrail(_IAuditLog.GetAuditActionTypeValue(\"Delete\"), Convert.ToInt32(id), \"" + this._table.Name + "\", " + this._table.Name + ", null, 0, 100);");
                    sb.AppendLine("             return new JsonResult(true);");
                }
                sb.AppendLine("         }");
            }
        }

        private void WriteListCrudPost(StringBuilder sb, string action)
        {
            sb.AppendLine("");
            sb.AppendLine("         /// <summary>");
            sb.AppendLine("         /// POST: /" + this._table.Name + "/" + action);
            sb.AppendLine("         /// </summary>");
            sb.AppendLine("         [HttpPost]");
            sb.AppendLine("         public IActionResult " + action + "(string inputSubmit, " + this._table.Name + "ViewModel viewModel)");
            sb.AppendLine("         {");
            sb.AppendLine("             if (ModelState.IsValid)");
            sb.AppendLine("             {");
            sb.AppendLine("                 CrudOperation operation = CrudOperation.Add;");
            sb.AppendLine("");
            sb.AppendLine("                 if (inputSubmit == \"Update\")");
            sb.AppendLine("                     operation = CrudOperation.Update;");
            sb.AppendLine("");
            sb.AppendLine("                 try");
            sb.AppendLine("                 {");
            sb.AppendLine("                     AddEdit" + this._table.Name + "(viewModel, operation);");
            sb.AppendLine("                 }");
            sb.AppendLine("                 catch(Exception ex)");
            sb.AppendLine("                 {");
            if (this._isUseLogging)
                sb.AppendLine("                     _Ilog.GetInstance().Error(\"Error Occured\", ex);");
            sb.AppendLine("");
            sb.AppendLine("                     ModelState.AddModelError(\"\", ex.Message);");
            sb.AppendLine("                 }");
            sb.AppendLine("             }");
            sb.AppendLine("");
            sb.AppendLine("             return View(GetViewModel(\"" + action + "\"));");
            sb.AppendLine("         }");
        }

        private void WriteGetUnboundViewModel(StringBuilder sb)
        {
            sb.AppendLine("");
            sb.AppendLine("         private " + this._table.Name + "ViewModel GetUnboundViewModel()");
            sb.AppendLine("         {");
            sb.AppendLine("             " + this._table.Name + MyConstants.WordPropertyModel + " viewModel = new " + this._table.Name + MyConstants.WordPropertyModel + "();");
            if (this._appVersion == ApplicationVersion.ProfessionalPlus)
            {
                sb.AppendLine("             viewModel." + this._table.Name + MyConstants.WordModel + " = null;");
                sb.AppendLine("             viewModel.ViewControllerName = \"" + this._table.Name + "\";");
                sb.AppendLine("             viewModel.ViewActionName = \"" + this._viewNamesCollection.Unbound + "\";");
                sb.AppendLine("             viewModel.ViewReturnUrl = \"/Home\";");
            }
            sb.AppendLine("");
            sb.AppendLine("             return viewModel;");
            sb.AppendLine("         }");
        }

        private void WriteOnGetGridData(StringBuilder sb, string action = "")
        {
            string str1 = "";
            string str2 = "";
            int num1 = 1;
            if (action == "totals")
                str1 = "WithTotals";
            else if (action == "tooltip")
                str1 = "WithTooltip";
            else if (action == "groupedby")
                str1 = "GroupedBy" + this._currentColumn.Name;
            else if (action == "totalsgroupedby")
                str1 = "TotalsGroupedBy" + this._currentColumn.Name;
            else if (action == "search" || action == "masterdetail")
            {
                str1 = "WithFilters";
                str2 = ", string filters";
            }
            if (this._controllerBaseType == ControllerBaseType.ApiControllerBase)
            {
                if (action == "search")
                {
                    sb.AppendLine("");
                    sb.AppendLine("         /// <summary>");
                    sb.AppendLine("         /// Use in a JQGrid plugin.  Selects records as a collection (List) of " + this._table.Name + " sorted by the sortByExpression.");
                    sb.AppendLine("         /// Also returns total pages, current page, and total records based on the search filters.");
                    sb.AppendLine("         /// </summary>");
                    sb.AppendLine("         /// <param name=\"_search\">true or false</param>");
                    sb.AppendLine("         /// <param name=\"nd\">nd</param>");
                    sb.AppendLine("         /// <param name=\"rows\">Number of rows to retrieve</param>");
                    sb.AppendLine("         /// <param name=\"page\">Current page</param>");
                    sb.AppendLine("         /// <param name=\"sidx\">Field to sort.  Can be an empty string.</param>");
                    sb.AppendLine("         /// <param name=\"sord\">asc or an empty string = ascending.  desc = descending</param>");
                    sb.AppendLine("         /// <param name=\"filters\">Optional.  Filters used in search</param>");
                    sb.AppendLine("         /// <returns>Serialized " + this._businessObjectName + " collection in json format for use in a JQGrid plugin</returns>");
                    sb.AppendLine("         [HttpGet]");
                    sb.AppendLine("         public object SelectSkipAndTakeWithFilters(string _search, string nd, int rows, int _page, string sidx, string sord, string filters = \"\")");
                    sb.AppendLine("         {");
                }
                else
                {
                    sb.AppendLine("");
                    sb.AppendLine("         /// <summary>");
                    sb.AppendLine("         /// Use in a JQGrid plugin.  Selects records as a collection (List) of " + this._table.Name + " sorted by the sortByExpression.");
                    sb.AppendLine("         /// Also returns total pages, current page, and total records.");
                    sb.AppendLine("         /// </summary>");
                    sb.AppendLine("         /// <param name=\"sidx\">Field to sort.  Can be an empty string.</param>");
                    sb.AppendLine("         /// <param name=\"sord\">asc or an empty string = ascending.  desc = descending</param>");
                    sb.AppendLine("         /// <param name=\"page\">Current page</param>");
                    sb.AppendLine("         /// <param name=\"rows\">Number of rows to retrieve</param>");
                    if (!string.IsNullOrEmpty(str2))
                        sb.AppendLine("         /// <param name=\"filters\">Filters used in search</param>");
                    if (string.IsNullOrEmpty(str1))
                        sb.AppendLine("         /// <param name=\"isforJqGrid\">Optional isforJqGrid.  Default is true, returns json formatted string, otherwise, returns serialized List of " + this._table.Name + "</param>");
                    sb.AppendLine("         /// <returns>Serialized " + this._businessObjectName + " collection in json format for use in a JQGrid plugin</returns>");
                    sb.AppendLine("         [HttpGet]");
                    if (string.IsNullOrEmpty(str1))
                        sb.AppendLine("         public object SelectSkipAndTake" + str1 + "(string sidx, string sord, int _page, int rows" + str2 + ", bool isforJqGrid = true)");
                    else
                        sb.AppendLine("         public object SelectSkipAndTake" + str1 + "(string sidx, string sord, int _page, int rows" + str2 + ")");
                    sb.AppendLine("         {");
                }
            }
            else
            {
                sb.AppendLine("");
                sb.AppendLine("         /// <summary>");
                sb.AppendLine("         /// Gets the list of data for use by the jqgrid plug-in");
                sb.AppendLine("         /// </summary>");
                if (string.IsNullOrEmpty(str1))
                    sb.AppendLine("         public IActionResult OnGetGridData" + str1 + "(string sidx, string sord, int _page, int rows" + str2 + ", bool isforJqGrid = true)");
                else
                    sb.AppendLine("         public IActionResult OnGetGridData" + str1 + "(string sidx, string sord, int _page, int rows" + str2 + ")");
                sb.AppendLine("         {");
            }
            if (action == "groupedby" || action == "totalsgroupedby")
            {
                sb.AppendLine("             // using a groupField in the jqgrid passes that field");
                sb.AppendLine("             // along with the field to sort, remove the groupField");
                sb.AppendLine("             string groupBy = \"" + this._currentFktable.DataTextField + " asc, \";");
                sb.AppendLine("             sidx = sidx.Replace(groupBy, \"\");");
                sb.AppendLine("");
            }
            if (action == "search" || action == "masterdetail")
            {
                StringBuilder stringBuilder1 = new StringBuilder();
                StringBuilder stringBuilder2 = new StringBuilder();
                StringBuilder stringBuilder3 = new StringBuilder();
                StringBuilder stringBuilder4 = new StringBuilder();
                foreach (Column spatialDataTypeColumn in this._table.NoneBinaryOrSpatialDataTypeColumns)
                {
                    string str3 = "?";
                    if (spatialDataTypeColumn.SystemType == "string")
                        str3 = "";
                    stringBuilder1.Append(spatialDataTypeColumn.NameCamelStyle + ", ");
                    if (spatialDataTypeColumn.SystemType.ToLower() == "string")
                    {
                        stringBuilder2.AppendLine("             " + spatialDataTypeColumn.SystemType + " " + spatialDataTypeColumn.NameCamelStyle + " = String.Empty;");
                        stringBuilder3.AppendLine("                     if (rule[\"field\"].Value.ToLower() == \"" + spatialDataTypeColumn.NameLowerCase + "\")");
                        stringBuilder3.AppendLine("                         " + spatialDataTypeColumn.NameCamelStyle + " = rule[\"data\"].Value;");
                        stringBuilder3.AppendLine("");
                    }
                    else
                    {
                        stringBuilder2.AppendLine("             " + spatialDataTypeColumn.SystemType + str3 + " " + spatialDataTypeColumn.NameCamelStyle + " = null;");
                        stringBuilder3.AppendLine("                     if (rule[\"field\"].Value.ToLower() == \"" + spatialDataTypeColumn.NameLowerCase + "\")");
                        if (spatialDataTypeColumn.SQLDataType == SQLType.time)
                            stringBuilder3.AppendLine("                         " + spatialDataTypeColumn.NameCamelStyle + " = TimeSpan.Parse(rule[\"data\"].Value);");
                        else if (spatialDataTypeColumn.SQLDataType == SQLType.datetimeoffset)
                            stringBuilder3.AppendLine("                         " + spatialDataTypeColumn.NameCamelStyle + " = new DateTimeOffset(Convert.ToDateTime(rule[\"data\"].Value));");
                        else
                            stringBuilder3.AppendLine("                         " + spatialDataTypeColumn.NameCamelStyle + " = Convert.To" + spatialDataTypeColumn.SystemTypeNative + "(rule[\"data\"].Value);");
                        stringBuilder3.AppendLine("");
                        if (spatialDataTypeColumn.IsNumericField)
                        {
                            stringBuilder4.AppendLine("                 if (" + spatialDataTypeColumn.NameCamelStyle + " == -1)");
                            stringBuilder4.AppendLine("                     " + spatialDataTypeColumn.NameCamelStyle + " = null;");
                            stringBuilder4.AppendLine("");
                        }
                    }
                }
                sb.Append(stringBuilder2);
                sb.AppendLine("");
                sb.AppendLine("             if (!String.IsNullOrEmpty(filters))");
                sb.AppendLine("             {");
                sb.AppendLine("                 // deserialize json and get values being searched");
                sb.AppendLine("                 var jsonResult = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(filters);");
                sb.AppendLine("");
                sb.AppendLine("                 foreach (var rule in jsonResult[\"rules\"])");
                sb.AppendLine("                 {");
                sb.Append(stringBuilder3);
                sb.AppendLine("                 }");
                if (stringBuilder4.Length > 0)
                {
                    sb.AppendLine("");
                    sb.AppendLine("                 // sometimes jqgrid assigns a -1 to numeric fields when no value is assigned");
                    sb.AppendLine("                 // instead of assigning a null, we'll correct this here");
                    sb.Append(stringBuilder4);
                }
                sb.AppendLine("             }");
                sb.AppendLine("");
                sb.AppendLine("             int totalRecords = " + this._businessObjectName + ".GetRecordCountDynamicWhere(" + Functions.RemoveLastComma(stringBuilder1.ToString()) + ");");
                if (this._generatedSqlType == GeneratedSqlType.EFCore)
                    sb.AppendLine("             int startRowIndex = ((_page * rows) - rows);");
                else
                    sb.AppendLine("             int startRowIndex = ((_page * rows) - rows) + 1;");
                sb.AppendLine("             List<" + this._businessObjectName + "> " + this._table.VariableObjCollectionName + " = " + this._businessObjectName + ".SelectSkipAndTakeDynamicWhere(" + stringBuilder1.ToString() + "rows, startRowIndex, sidx + \" \" + sord);");
            }
            else
            {
                sb.AppendLine("             int totalRecords = " + this._businessObjectName + ".GetRecordCount();");
                if (this._generatedSqlType == GeneratedSqlType.EFCore)
                    sb.AppendLine("             int startRowIndex = ((_page * rows) - rows);");
                else
                    sb.AppendLine("             int startRowIndex = ((_page * rows) - rows) + 1;");
                if (string.IsNullOrEmpty(str1))
                {
                    if (this._table.IsContainsForeignKeysWithTableSelected && this._controllerBaseType == ControllerBaseType.ApiControllerBase && (this._isUseWebApi && this._generatedSqlType == GeneratedSqlType.EFCore))
                    {
                        sb.AppendLine("             bool isIncludeRelatedProperties = true;");
                        sb.AppendLine("");
                        sb.AppendLine("             if(!isforJqGrid)");
                        sb.AppendLine("                 isIncludeRelatedProperties = false;");
                        sb.AppendLine("");
                        sb.AppendLine("             List<" + this._businessObjectName + "> " + this._table.VariableObjCollectionName + " = " + this._businessObjectName + ".SelectSkipAndTake(rows, startRowIndex, sidx + \" \" + sord, isIncludeRelatedProperties);");
                    }
                    else
                        sb.AppendLine("             List<" + this._businessObjectName + "> " + this._table.VariableObjCollectionName + " = " + this._businessObjectName + ".SelectSkipAndTake(rows, startRowIndex, sidx + \" \" + sord);");
                    if (this._controllerBaseType == ControllerBaseType.ApiControllerBase)
                    {
                        sb.AppendLine("");
                        sb.AppendLine("             if (!isforJqGrid)");
                        sb.AppendLine("             {");
                        sb.AppendLine("                 if (" + this._table.VariableObjCollectionName + " is null)");
                        sb.AppendLine("                     return \"\";");
                        sb.AppendLine("");
                        sb.AppendLine("                 return " + this._table.VariableObjCollectionName + ";");
                        sb.AppendLine("             }");
                        sb.AppendLine("");
                    }
                }
                else
                {
                    sb.AppendLine("");
                    sb.AppendLine("             List<" + this._businessObjectName + "> " + this._table.VariableObjCollectionName + " = " + this._businessObjectName + ".SelectSkipAndTake(rows, startRowIndex, sidx + \" \" + sord);");
                }
            }
            sb.AppendLine("             int totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);");
            sb.AppendLine("");
            sb.AppendLine("             if (" + this._table.VariableObjCollectionName + " is null)");
            sb.AppendLine("                 return new JsonResult(\"{ total = 0, page = 0, records = 0, rows = null }\");");
            if (action != "tooltip")
            {
                sb.AppendLine("");
                sb.AppendLine("             var jsonData = new");
                sb.AppendLine("             {");
                sb.AppendLine("                 total = totalPages,");
                sb.AppendLine("                 _page,");
                sb.AppendLine("                 records = totalRecords,");
                if (action == "totals")
                    this.WriteColumnTotals(sb);
                sb.AppendLine("                 rows = (");
                sb.AppendLine("                     from " + this._table.VariableObjName + " in " + this._table.VariableObjCollectionName);
                sb.AppendLine("                     select new");
                sb.AppendLine("                     {");
                if (this._table.PrimaryKeyCount > 1)
                {
                    int num2 = 1;
                    string str3 = string.Empty;
                    string str4 = string.Empty;
                    foreach (Column primaryKeyColumn in this._table.PrimaryKeyColumns)
                    {
                        if (primaryKeyColumn.SystemType.ToLower() != "string")
                            str4 = ".ToString()";
                        str3 = str3 + this._table.VariableObjName + "." + primaryKeyColumn.Name + str4;
                        if (num2 < this._table.PrimaryKeyCount)
                            str3 += " + ";
                        ++num2;
                    }
                    if (this._generateFrom == DatabaseObjectToGenerateFrom.Tables)
                        sb.AppendLine("                         id = " + str3 + ",");
                }
                else if (this._generateFrom == DatabaseObjectToGenerateFrom.Tables)
                    sb.AppendLine("                         id = " + this._table.VariableObjName + "." + this._table.FirstPrimaryKeyName + ",");
                sb.AppendLine("                         cell = new string[] { ");
                StringBuilder stringBuilder = new StringBuilder();
                foreach (Column column in this._table.Columns)
                {
                    if (this._generatedSqlType == GeneratedSqlType.EFCore && column.SQLDataType == SQLType.uniqueidentifier)
                    {
                        if (column.IsNullable && column.IsForeignKey && column.IsStringField)
                        {
                            if (column.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK)
                                stringBuilder.AppendLine("                             !" + this._table.VariableObjName + "." + column.Name + ".HasValue || " + this._table.VariableObjName + "." + column.Name + " == Guid.Empty ? String.Empty : " + this._table.VariableObjName + "." + column.Name + ".Value.ToString(),");
                            else
                                stringBuilder.AppendLine("                             " + this._table.VariableObjName + "." + column.Name + ".ToString(),");
                        }
                        else if (column.IsNullable)
                            stringBuilder.AppendLine("                             !" + this._table.VariableObjName + "." + column.Name + ".HasValue || " + this._table.VariableObjName + "." + column.Name + " == Guid.Empty ? String.Empty : " + this._table.VariableObjName + "." + column.Name + ".Value.ToString(),");
                        else
                            stringBuilder.AppendLine("                             " + this._table.VariableObjName + "." + column.Name + ".ToString(),");
                    }
                    else
                    {
                        string str3 = string.Empty;
                        if (column.IsNullable && column.IsForeignKey && column.IsStringField)
                        {
                            if (column.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK)
                                stringBuilder.AppendLine("                             !String.IsNullOrEmpty(" + this._table.VariableObjName + "." + column.Name + ") ? " + this._table.VariableObjName + "." + column.Name + " : \"\",");
                            else
                                stringBuilder.AppendLine("                             " + this._table.VariableObjName + "." + column.Name + str3 + ",");
                        }
                        else
                        {
                            if (column.SystemTypeNative.ToLower() == "datetime")
                            {
                                if (column.IsNullable)
                                    str3 = ".HasValue ? " + this._table.VariableObjName + "." + column.Name + ".Value.ToString(\"d\") : \"\"";
                                else
                                    str3 = ".ToString(\"d\")";
                            }
                            else if (column.IsNullable && column.SQLDataType != SQLType.bit && column.SystemTypeNative.ToLower() != "string")
                            {
                                if (column.SQLDataType == SQLType.xml)
                                    str3 = " != null ? " + this._table.VariableObjName + "." + column.Name + ".ToString(System.Xml.Linq.SaveOptions.DisableFormatting) : \"\"";
                                else
                                    str3 = ".HasValue ? " + this._table.VariableObjName + "." + column.Name + ".Value.ToString() : \"\"";
                            }
                            else if (column.SystemTypeNative.ToLower() != "string")
                                str3 = ".ToString()";
                            stringBuilder.AppendLine("                             " + this._table.VariableObjName + "." + column.Name + str3 + ",");
                        }
                    }
                }
                if (action == "groupedby" || action == "totalsgroupedby")
                {
                    string str3 = string.Empty;
                    if (this._generatedSqlType == GeneratedSqlType.EFCore)
                    {
                        if (this._currentFktable.DataTextFieldSystemType.ToLower() != "string")
                            str3 = ".ToString()";
                        int num2;
                        if (this._currentColumn.IsNullable)
                        {
                            if (this._currentColumn.Name != this._currentColumn.ForeignKeyColumnName)
                                stringBuilder.AppendLine("                             " + this._table.VariableObjName + "." + this._currentColumn.Name + " is null ? \"\" : " + this._table.VariableObjName + "." + this._currentColumn.Name + "Navigation." + this._currentFktable.DataTextField + str3);
                            else if (this._table.Name == this._currentColumn.SingularForeignKeyTableName)
                            {
                                stringBuilder.AppendLine("                             " + this._table.VariableObjName + "." + this._currentColumn.Name + " is null ? \"\" : " + this._table.VariableObjName + "." + this._currentFkSingularTableName + num1 + "." + this._currentFktable.DataTextField + str3);
                                num2 = num1 + 1;
                            }
                            else
                                stringBuilder.AppendLine("                             " + this._table.VariableObjName + "." + this._currentColumn.Name + " is null ? \"\" : " + this._table.VariableObjName + "." + this._currentFkSingularTableName + "." + this._currentFktable.DataTextField + str3);
                        }
                        else if (this._currentColumn.Name != this._currentColumn.ForeignKeyColumnName)
                            stringBuilder.AppendLine("                             " + this._table.VariableObjName + "." + this._currentColumn.Name + "Navigation." + this._currentFktable.DataTextField + str3);
                        else if (this._table.Name == this._currentColumn.SingularForeignKeyTableName)
                        {
                            stringBuilder.AppendLine("                             " + this._table.VariableObjName + "." + this._currentFkSingularTableName + num1 + "." + this._currentFktable.DataTextField + str3);
                            num2 = num1 + 1;
                        }
                        else
                            stringBuilder.AppendLine("                             " + this._table.VariableObjName + "." + this._currentFkSingularTableName + "." + this._currentFktable.DataTextField + str3);
                    }
                    else
                    {
                        if (this._currentFktable.DataTextFieldSystemType.ToLower() != "string")
                            str3 = ".ToString()";
                        if (this._currentColumn.IsNullable)
                            stringBuilder.AppendLine("                             " + this._table.VariableObjName + "." + this._currentColumn.Name + " is null ? \"\" : " + this._table.VariableObjName + "." + this._currentFkTableName + ".Value." + this._currentFktable.DataTextField + str3);
                        else
                            stringBuilder.AppendLine("                             " + this._table.VariableObjName + "." + this._currentFkTableName + ".Value." + this._currentFktable.DataTextField + str3);
                    }
                }
                sb.Append(Functions.RemoveLastComma(stringBuilder.ToString()));
                sb.AppendLine("");
                sb.AppendLine("                         }");
                sb.AppendLine("                     }).ToArray()");
                sb.AppendLine("             };");
            }
            else
                this.BuildDataForToolTip(sb);
            sb.AppendLine("");
            if (this._controllerBaseType == ControllerBaseType.ApiControllerBase)
                sb.AppendLine("             return jsonData;");
            else
                sb.AppendLine("             return new JsonResult(jsonData);");
            sb.AppendLine("         }");
        }

        private void BuildDataForToolTip(StringBuilder sb)
        {
            sb.AppendLine("");
            sb.AppendLine("             // build rows for use by the jqgrid");
            sb.AppendLine("             List<JqGridRow> jqGridRows = new List<JqGridRow>();");
            sb.AppendLine("");
            sb.AppendLine("             foreach (var " + this._table.VariableObjName + " in paged" + this._table.Name + "Col)");
            sb.AppendLine("             {");
            sb.AppendLine("                 List<string> cells = new List<string>();");
            sb.AppendLine("");
            foreach (Column column in this._table.Columns)
            {
                string str = string.Empty;
                if (column.SystemTypeNative.ToLower() != "string")
                    str = ".ToString()";
                this._colModelNames.Append(column.Name + ",");
                if (column.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK)
                {
                    sb.AppendLine("                 cells.Add(" + this._table.VariableObjName + "." + column.Name + str + ");  // hidden");
                    sb.AppendLine("                 cells.Add(" + this._table.VariableObjName + "." + column.Name + str + ");  // tooltip");
                }
                else
                    sb.AppendLine("                 cells.Add(" + this._table.VariableObjName + "." + column.Name + str + ");");
            }
            StringBuilder stringBuilder = new StringBuilder();
            int num = 0;
            foreach (Column column1 in this._table.Columns)
            {
                if (column1.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK)
                {
                    string name = column1.ForeignKeyTable.Name;
                    string str1 = string.Empty;
                    if (stringBuilder.ToString().Contains(column1.ForeignKeyTable.Name + ","))
                    {
                        ++num;
                        name += num.ToString();
                    }
                    if (this._table.Name == column1.ForeignKeyTable.Name)
                        name += column1.Name;
                    sb.AppendLine("");
                    if (column1.IsNullable)
                    {
                        str1 = "    ";
                        sb.AppendLine("                 if (" + this._table.VariableObjName + "." + column1.Name + " != null)");
                        sb.AppendLine("                 {");
                    }
                    sb.AppendLine(str1 + "                 // values that will show up on the " + column1.Name + " tooltip,");
                    sb.AppendLine(str1 + "                 // these values are hidden in the jqGrid");
                    foreach (Column column2 in column1.ForeignKeyTable.Columns)
                    {
                        if (!this._colModelNames.ToString().Contains(column2.Name + ","))
                        {
                            string str2 = string.Empty;
                            if (column2.SystemTypeNative.ToLower() != "string")
                                str2 = ".ToString()";
                            sb.AppendLine(str1 + "                 cells.Add(" + this._table.VariableObjName + "." + name + ".Value." + column2.Name + str2 + ");");
                        }
                    }
                    if (column1.IsNullable)
                    {
                        sb.AppendLine("                 }");
                        sb.AppendLine("                 else");
                        sb.AppendLine("                 {");
                        sb.AppendLine("                     // add empty cells for each of the foreign table columns");
                        foreach (Column column2 in column1.ForeignKeyTable.Columns)
                        {
                            if (!this._colModelNames.ToString().Contains(column2.Name + ","))
                                sb.AppendLine("                     cells.Add(\"\");");
                        }
                        sb.AppendLine("                 }");
                    }
                }
            }
            sb.AppendLine("");
            sb.AppendLine("                 JqGridRow row = new JqGridRow();");
            sb.AppendLine("                 row.id = " + this._table.VariableObjName + "." + this._table.FirstPrimaryKeyName + ";");
            sb.AppendLine("                 row.cell = cells;");
            sb.AppendLine("");
            sb.AppendLine("                 jqGridRows.Add(row);");
            sb.AppendLine("             }");
            sb.AppendLine("");
            sb.AppendLine("             JqGridData jsonData = new JqGridData();");
            sb.AppendLine("             jsonData.total = totalPages;");
            sb.AppendLine("             jsonData.page = _page;");
            sb.AppendLine("             jsonData.records = totalRecords;");
            sb.AppendLine("             jsonData.rows = jqGridRows;");
            sb.AppendLine("");
        }

        private void WriteColumnTotals(StringBuilder sb)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Column column in this._table.Columns)
            {
                if (column.SQLDataType == SQLType.decimalnumber || column.SQLDataType == SQLType.money || column.SQLDataType == SQLType.smallmoney)
                    stringBuilder.AppendLine("                     " + column.Name + " = " + this._table.VariableObjCollectionName + ".Select(" + this._table.LinqFromVariable + " => " + this._table.LinqFromVariable + "." + column.Name + ").Sum().ToString(),");
            }
        }

        private void WriteForeachProperties(StringBuilder sb)
        {
            if (this._selectedTables.Exists((t => t.Name + "Model" == this._table.Name)))
                sb.AppendLine("         public List<" + this._apiName + ".BusinessObject." + this._table.Name + "> " + this._table.Name + "Data { get; set; }");
            else
                sb.AppendLine("         public List<" + this._table.Name + "> " + this._table.Name + "Data { get; set; }");
            sb.AppendLine("         public string[,] " + this._table.Name + "FieldNames { get; set; }");
            sb.AppendLine("         public string FieldToSort { get; set; }");
            sb.AppendLine("         public string FieldToSortWithOrder { get; set; }");
            sb.AppendLine("         public string FieldSortOrder { get; set; }");
            sb.AppendLine("         public int StartPage { get; set; }");
            sb.AppendLine("         public int EndPage { get; set; }");
            sb.AppendLine("         public int CurrentPage { get; set; }");
            sb.AppendLine("         public int NumberOfPagesToShow { get; set; }");
            sb.AppendLine("         public int TotalPages { get; set; }");
            sb.AppendLine("         public List<string> UnsortableFields { get; set; }");
        }

        private void WriteOnGet(StringBuilder sb)
        {
            sb.AppendLine("         /// <summary>");
            sb.AppendLine("         /// Initial handler the razor page encounters.");
            sb.AppendLine("         /// </summary>");
            if (this._viewType == MVCGridViewType.Update || this._viewType == MVCGridViewType.RecordDetails)
            {
                string str = "void";
                if (this._viewType == MVCGridViewType.Update)
                    str = "PageResult";
                if (this._table.PrimaryKeyCount > 1)
                {
                    sb.AppendLine("         public void OnGet(" + Functions.GetCommaDelimitedPrimaryKeysAndType(this._table, Language.CSharp) + ", string returnUrl)");
                    sb.AppendLine("         {");
                    sb.AppendLine("             LoadPage(" + Functions.GetCommaDelimitedPrimaryKeys(this._table, Language.CSharp, true, false, false) + ", returnUrl);");
                    sb.AppendLine("         }");
                    sb.AppendLine("");
                    sb.AppendLine("         public " + str + " LoadPage(" + Functions.GetCommaDelimitedPrimaryKeysAndType(this._table, Language.CSharp) + ", string returnUrl)");
                    sb.AppendLine("         {");
                    sb.AppendLine("             // select a record by primary key(s)");
                    if (this._isUseWebApi)
                        sb.AppendLine("             " + this._apiName + "." + MyConstants.WordBusinessObject + "." + this._table.Name + " " + this._table.VariableObjName + " = Get" + this._table.Name + "ByPrimaryKey(" + Functions.GetCommaDelimitedPrimaryKeys(this._table, Language.CSharp, true, false, false) + ");");
                    else
                        sb.AppendLine("             " + this._apiName + "." + MyConstants.WordBusinessObject + "." + this._table.Name + " " + this._table.VariableObjName + " = " + this._table.Name + ".SelectByPrimaryKey(" + Functions.GetCommaDelimitedPrimaryKeys(this._table, Language.CSharp, true, false, false) + ");");
                    sb.AppendLine("");
                    Functions.WriteOnGetPropertyAssignMents(sb, this._table, this._selectedTables, this._viewType, this._apiName, this._isUseWebApi, this._viewNamesCollection, this._generatedSqlType, this._modelName, this._businessObjectName);
                    sb.AppendLine("         }");
                }
                else
                {
                    sb.AppendLine("         public void OnGet(" + this._table.FirstPrimaryKeySystemType + " id, string returnUrl)");
                    sb.AppendLine("         {");
                    sb.AppendLine("             LoadPage(id, returnUrl);");
                    sb.AppendLine("         }");
                    sb.AppendLine("");
                    sb.AppendLine("         public " + str + " LoadPage(" + this._table.FirstPrimaryKeySystemType + " id, string returnUrl)");
                    sb.AppendLine("         {");
                    sb.AppendLine("             // select a record by primary key(s)");
                    if (this._isUseWebApi)
                        sb.AppendLine("             " + this._apiName + "." + MyConstants.WordBusinessObject + "." + this._table.Name + " " + this._table.VariableObjName + " = Get" + this._table.Name + "ByPrimaryKey(id);");
                    else
                        sb.AppendLine("             " + this._apiName + "." + MyConstants.WordBusinessObject + "." + this._table.Name + " " + this._table.VariableObjName + " = " + this._table.Name + ".SelectByPrimaryKey(id);");
                    sb.AppendLine("");
                    Functions.WriteOnGetPropertyAssignMents(sb, this._table, this._selectedTables, this._viewType, this._apiName, this._isUseWebApi, this._viewNamesCollection, this._generatedSqlType, this._modelName, this._businessObjectName);
                    sb.AppendLine("         }");
                }
            }
            else if (this._viewType == MVCGridViewType.Add)
            {
                sb.AppendLine("         public void OnGet(string returnUrl)");
                sb.AppendLine("         {");
                sb.AppendLine("             LoadPage(returnUrl);");
                sb.AppendLine("         }");
                sb.AppendLine("");
                sb.AppendLine("         private PageResult LoadPage(string returnUrl)");
                sb.AppendLine("         {");
                Functions.WriteOnGetPropertyAssignMents(sb, this._table, this._selectedTables, this._viewType, this._apiName, this._isUseWebApi, this._viewNamesCollection, this._generatedSqlType, this._modelName, this._businessObjectName);
                sb.AppendLine("         }");
            }
            else
            {
                sb.AppendLine("         public void OnGet()");
                sb.AppendLine("         {");
                if (this._viewType == MVCGridViewType.Unbound)
                {
                    sb.AppendLine("             // assign model(s) used by this page");
                    sb.AppendLine("             ReturnUrl = \"/Index\";");
                }
                else
                    Functions.WriteOnGetPropertyAssignMents(sb, this._table, this._selectedTables, this._viewType, this._apiName, this._isUseWebApi, this._viewNamesCollection, this._generatedSqlType, this._modelName, this._businessObjectName);
                sb.AppendLine("         }");
            }
        }

        private void WriteListCrudOnGet(StringBuilder sb)
        {
            sb.AppendLine("         /// <summary>");
            sb.AppendLine("         /// Initial handler the razor page encounters.");
            sb.AppendLine("         /// </summary>");
            sb.AppendLine("         public void OnGet()");
            sb.AppendLine("         {");
            sb.AppendLine("             SetData();");
            sb.AppendLine("         }");
        }

        private void WriteListCrudOnGetAdd(StringBuilder sb)
        {
            sb.AppendLine("");
            sb.AppendLine("         /// <summary>");
            sb.AppendLine("         /// Handler, adds a new record.");
            sb.AppendLine("         /// </summary>");
            sb.AppendLine("         public IActionResult OnGetAdd(string serializedData)");
            sb.AppendLine("         {");
            sb.AppendLine("             " + this._table.Name + " " + this._table.VariableObjName + " = JsonConvert.DeserializeObject<" + this._table.Name + ">(serializedData);");
            if (this._isUseAuditLogging && !this._isUseWebApi)
                sb.AppendLine("             " + this._table.Name + "Functions.AddOrEdit(" + this._table.VariableObjName + ", CrudOperation.Add, _IAuditLog, true);");
            else
                sb.AppendLine("             " + this._table.Name + "Functions.AddOrEdit(" + this._table.VariableObjName + ", CrudOperation.Add, true);");
            sb.AppendLine("             return new JsonResult(true);");
            sb.AppendLine("         }");
        }

        private void WriteListCrudOnGetUpdate(StringBuilder sb)
        {
            sb.AppendLine("");
            sb.AppendLine("         /// <summary>");
            sb.AppendLine("         /// Handler, updates a record.");
            sb.AppendLine("         /// </summary>");
            sb.AppendLine("         public IActionResult OnGetUpdate(string serializedData)");
            sb.AppendLine("         {");
            sb.AppendLine("             " + this._table.Name + " " + this._table.VariableObjName + " = JsonConvert.DeserializeObject<" + this._table.Name + ">(serializedData);");
            if (this._isUseAuditLogging && !this._isUseWebApi)
                sb.AppendLine("             " + this._table.Name + "Functions.AddOrEdit(" + this._table.VariableObjName + ", CrudOperation.Update, _IAuditLog, true);");
            else
                sb.AppendLine("             " + this._table.Name + "Functions.AddOrEdit(" + this._table.VariableObjName + ", CrudOperation.Update, true);");
            sb.AppendLine("             return new JsonResult(true);");
            sb.AppendLine("         }");
        }

        private void WriteForeachOnGet(StringBuilder sb)
        {
            StringBuilder stringBuilder = new StringBuilder();
            sb.AppendLine("         /// <summary>");
            sb.AppendLine("         /// Initial handler the razor page encounters.");
            sb.AppendLine("         /// </summary>");
            sb.AppendLine("         public void OnGet()");
            sb.AppendLine("         {");
            sb.AppendLine("             string sidx = String.Empty;");
            sb.AppendLine("             string sord = String.Empty;");
            sb.AppendLine("             int currentPage = 1;");
            sb.AppendLine("");
            sb.AppendLine("             GetData(sidx, sord, currentPage);");
            sb.AppendLine("         }");
        }

        private void WriteGetFilteredData(StringBuilder sb)
        {
            StringBuilder stringBuilder1 = new StringBuilder();
            StringBuilder stringBuilder2 = new StringBuilder();
            StringBuilder stringBuilder3 = new StringBuilder();
            foreach (Column column in this._table.Columns)
            {
                if (!column.IsBinaryOrSpatialDataType)
                {
                    if (Functions.GetFieldType(column) == "String")
                        stringBuilder3.Append("!String.IsNullOrEmpty(" + column.NameCamelStyle + ") || ");
                    else
                        stringBuilder3.Append(column.NameCamelStyle + " != null || ");
                    stringBuilder1.Append(column.NameCamelStyle + ", ");
                    if (column.SystemType.ToLower() == "string")
                        stringBuilder2.Append(column.SystemType + " " + column.NameCamelStyle + ", ");
                    else
                        stringBuilder2.Append(column.SystemType + "? " + column.NameCamelStyle + ", ");
                }
            }
            sb.AppendLine("");
            sb.AppendLine("         private List<" + this._businessObjectName + "> GetFilteredData(" + stringBuilder2.ToString() + "string sidx, string sord, int rows, int startRowIndex, string sortExpression)");
            sb.AppendLine("         {");
            sb.AppendLine("             if (" + Functions.RemoveLastOR(stringBuilder3.ToString()) + ")");
            sb.AppendLine("                 return " + this._businessObjectName + ".SelectSkipAndTakeDynamicWhere(" + stringBuilder1.ToString() + "rows, startRowIndex, sortExpression);");
            sb.AppendLine("");
            sb.AppendLine("             return " + this._businessObjectName + ".SelectSkipAndTake(rows, startRowIndex, sortExpression);");
            sb.AppendLine("         }");
        }

        private string GetWebApiForeignKeyViewAssignments()
        {
            StringBuilder stringBuilder1 = new StringBuilder();
            StringBuilder stringBuilder2 = new StringBuilder();
            if (this._table.ForeignKeyCount > 0)
            {
                foreach (Column column in this._table.Columns)
                {
                    if (column.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK)
                    {
                        string str = column.ForeignKeyTableName + MyConstants.WordDropDownListData;
                        if (!stringBuilder1.ToString().Contains(str + ","))
                        {
                            stringBuilder2.AppendLine("             viewModel." + str + " = Get" + column.ForeignKeyTable.Name + "DropDownListData();");
                            stringBuilder1.Append(str + ",");
                        }
                    }
                }
            }
            return stringBuilder2.ToString();
        }

        private void WriteGetByPrimaryKeyWebApiCall(StringBuilder sb)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string str1 = Functions.ConvertToCamel(this._table.FirstPrimaryKeyName);
            string str2 = string.Empty;
            if (this._table.PrimaryKeyCount > 1)
            {
                str1 = Functions.GetQueryStringDelimitedPrimaryKeys(this._table, Language.CSharp, null);
                str2 = "?";
            }
            sb.AppendLine("");
            sb.AppendLine("         private " + this._businessObjectName + " Get" + this._table.Name + "ByPrimaryKey(" + Functions.GetCommaDelimitedPrimaryKeyParametersWithSystemType(this._table, Language.CSharp) + ")");
            sb.AppendLine("         {");
            sb.AppendLine("             " + this._businessObjectName + " " + this._table.VariableObjName + " = null;");
            sb.AppendLine("");
            sb.AppendLine("             using (var client = new HttpClient())");
            sb.AppendLine("             {");
            sb.AppendLine("                 client.BaseAddress = new Uri(Functions.GetWebApiBaseAddress());");
            sb.AppendLine("                 HttpResponseMessage response = client.GetAsync(\"" + this._table.Name + "Api/SelectByPrimaryKey/" + str2 + "\" + " + str1 + ").Result;");
            sb.AppendLine("");
            sb.AppendLine("                 if (!response.IsSuccessStatusCode)");
            sb.AppendLine("                 {");
            sb.AppendLine("                     throw new Exception(\"Error Status Code: \" + response.StatusCode.ToString() + \" Error Reason: \" + response.ReasonPhrase + \" Error Message: \" + response.RequestMessage.ToString());");
            sb.AppendLine("                 }");
            sb.AppendLine("                 else");
            sb.AppendLine("                 {");
            sb.AppendLine("                     var responseBody = response.Content.ReadAsStringAsync().Result;");
            sb.AppendLine("                     " + this._table.VariableObjName + " = JsonConvert.DeserializeObject<" + this._businessObjectName + ">(responseBody);");
            sb.AppendLine("                 }");
            sb.AppendLine("             }");
            sb.AppendLine("");
            sb.AppendLine("             return " + this._table.VariableObjName + ";");
            sb.AppendLine("         }");
        }

        private void WriteGetDropDownListWebApiHttpClientCallsForFks(StringBuilder sb)
        {
            if (this._table.ForeignKeyCount <= 0)
                return;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Column column in this._table.ForeignKeyColumnsTableIsSelectedAndOnlyOnePK)
            {
                string str = column.ForeignKeyTableName + MyConstants.WordDropDownListData;
                if (!stringBuilder.ToString().Contains(str + ", "))
                {
                    stringBuilder.Append(str + ", ");
                    string qualifiedTableName = Functions.GetFullyQualifiedTableName(column.ForeignKeyTable, this._selectedTables, Language.CSharp, column.ForeignKeyTable.NameFullyQualifiedBusinessObject, this._apiName);
                    sb.AppendLine("");
                    sb.AppendLine("         /// <summary>");
                    sb.AppendLine("         /// Gets a List of " + column.ForeignKeyTableName + " for use with a DropDownList or Select List, etc.");
                    sb.AppendLine("         /// </summary>");
                    sb.AppendLine("         private List<" + qualifiedTableName + "> Get" + column.ForeignKeyTable.Name + "DropDownListData()");
                    sb.AppendLine("         {");
                    sb.AppendLine("             List<" + qualifiedTableName + "> " + column.ForeignKeyTable.VariableObjCollectionName + " = null;");
                    sb.AppendLine("");
                    sb.AppendLine("             using (var client = new HttpClient())");
                    sb.AppendLine("             {");
                    sb.AppendLine("                 client.BaseAddress = new Uri(Functions.GetWebApiBaseAddress());");
                    sb.AppendLine("                 HttpResponseMessage response = client.GetAsync(\"" + column.ForeignKeyTable.Name + "Api/Select" + column.ForeignKeyTable.Name + "DropDownListData/\").Result;");
                    sb.AppendLine("");
                    sb.AppendLine("                 if (!response.IsSuccessStatusCode)");
                    sb.AppendLine("                 {");
                    sb.AppendLine("                     throw new Exception(\"Error Status Code: \" + response.StatusCode.ToString() + \" Error Reason: \" + response.ReasonPhrase + \" Error Message: \" + response.RequestMessage.ToString());");
                    sb.AppendLine("                 }");
                    sb.AppendLine("                 else");
                    sb.AppendLine("                 {");
                    sb.AppendLine("                     var responseBody = response.Content.ReadAsStringAsync().Result;");
                    sb.AppendLine("                     " + column.ForeignKeyTable.VariableObjCollectionName + " = JsonConvert.DeserializeObject<List<" + qualifiedTableName + ">>(responseBody);");
                    sb.AppendLine("                 }");
                    sb.AppendLine("             }");
                    sb.AppendLine("");
                    sb.AppendLine("             return " + column.ForeignKeyTable.VariableObjCollectionName + ";");
                    sb.AppendLine("         }");
                }
            }
        }

        private void WriteSetData(StringBuilder sb)
        {
            StringBuilder stringBuilder = new StringBuilder();
            sb.AppendLine("");
            sb.AppendLine("         /// <summary>");
            sb.AppendLine("         /// Sets data needed on page intialization.");
            sb.AppendLine("         /// </summary>");
            sb.AppendLine("         private void SetData()");
            sb.AppendLine("         {");
            sb.AppendLine("             " + this._table.Name + " = new " + this._apiName + "." + MyConstants.WordBusinessObject + "." + this._table.Name + "();");
            foreach (Column column in this._table.ColumnsWithDropDownListData)
            {
                if (!stringBuilder.ToString().Contains(column.DropDownListDataPropertyName + ","))
                {
                    if (this._isUseWebApi)
                    {
                        sb.AppendLine("             " + column.DropDownListDataPropertyName + " = Get" + column.ForeignKeyTable.Name + "DropDownListData();");
                    }
                    else
                    {
                        string qualifiedTableName = Functions.GetFullyQualifiedTableName(column.ForeignKeyTable, this._selectedTables, Language.CSharp, column.ForeignKeyTableNameFullyQualifiedBusinessObject, this._apiName);
                        sb.AppendLine("             " + column.DropDownListDataPropertyName + " = " + qualifiedTableName + ".Select" + column.ForeignKeyTableName + "DropDownListData();");
                    }
                    stringBuilder.Append(column.DropDownListDataPropertyName + ",");
                }
            }
            sb.AppendLine("         }");
        }

        private void WriteAuditGet(StringBuilder sb)
        {
            if (this._isUseWebApi)
            {
                sb.AppendLine("");
                sb.AppendLine("         /// <summary>");
                sb.AppendLine("         /// Handler, get audit of a record.");
                sb.AppendLine("         /// </summary>");
                sb.AppendLine("         public IActionResult OnGetAudit(int id)");
                sb.AppendLine("         {");
                sb.AppendLine("             using (var client = new HttpClient())");
                sb.AppendLine("             {");
                sb.AppendLine("                List<AuditChange> lst;");
                sb.AppendLine("                client.BaseAddress = new Uri(Functions.GetWebApiBaseAddress());");
                sb.AppendLine("                HttpResponseMessage response = client.GetAsync(\""+ this._table.Name + "Api/Audit/\" + id).Result;");
                sb.AppendLine("");
                sb.AppendLine("                if (!response.IsSuccessStatusCode)");
                sb.AppendLine("                    throw new Exception(\"Error Status Code: \" + response.StatusCode.ToString() + \" Error Reason: \" + response.ReasonPhrase + \" Error Message: \" + response.RequestMessage.ToString());");
                sb.AppendLine("");
                sb.AppendLine("                // Get the response");
                sb.AppendLine("                lst = response.Content.ReadAsAsync<List<AuditChange>>().Result;");
                sb.AppendLine("");
                sb.AppendLine("                return new JsonResult(lst);");
                sb.AppendLine("             }");
                sb.AppendLine("         }");
            }
            else
            {
                sb.AppendLine("");
                sb.AppendLine("         /// <summary>");
                sb.AppendLine("         /// Handler, get audit of a record.");
                sb.AppendLine("         /// </summary>");
                sb.AppendLine("         public IActionResult OnGetAudit(int id)");
                sb.AppendLine("         {");
                sb.AppendLine("             var AuditTrail = _IAuditLog.GetAuditReport(\"" + this._table.Name + "\", id);");
                sb.AppendLine("             return new JsonResult(AuditTrail);");
                sb.AppendLine("         }");
            }
        }
    }
}
