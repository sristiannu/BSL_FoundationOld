using System.Data;
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class ControllerlBase
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
    private ViewNames _viewNames;
    private DatabaseObjectToGenerateFrom _generateFrom;
    private ApplicationVersion _appVersion;
    private string _businessObjectName;
    private StringBuilder _colModelNames;
    private Table _currentFkTable;
    private Column _currentColumn;
    private string _currentFkTableName;
    private string _currentFkSingularTableName;
    private ControllerBaseType _controllerBaseType;
    private bool _isUseWebApi;
    private DataTable _referencedTables;
    private string _modelName;
    private bool _isSqlVersion2012OrHigher;
    private GeneratedSqlType _generatedSqlType;
    private bool _isUseLogging;
    private bool _isUseCaching;
    private bool _isUseAuditLogging;
    private bool _isEmailNotification;

    internal ControllerlBase(Table table, Tables selectedTables, string webAppName, string apiName, string webApiName, string webAppRootDirectory, string apiNameDirectory, string webApiNameDirectory, bool isUseStoredProcedure, bool isSqlVersion2012OrHigher, IsCheckedView isCheckedView, ViewNames viewName, DatabaseObjectToGenerateFrom generateFrom, ApplicationVersion appVersion, GeneratedSqlType generatedSqlType, bool isUseLogging, bool isUseCaching, bool isUseAuditLogging, bool isEmailNotification, ControllerBaseType controllerBaseType = ControllerBaseType.ControllerBase, bool isUseWebApi = false, DataTable referencedTables = null)
    {
      this._table = table;
      this._selectedTables = selectedTables;
      this._webAppName = webAppName;
      this._apiName = apiName;
      this._webApiName = webApiName;
      this._webAppRootDirectory = webAppRootDirectory;
      this._apiNameDirectory = apiNameDirectory;
      this._webApiNameDirectory = webApiNameDirectory;
      this._isUseStoredProcedure = isUseStoredProcedure;
      this._isCheckedView = isCheckedView;
      this._viewNames = viewName;
      this._generateFrom = generateFrom;
      this._appVersion = appVersion;
      this._referencedTables = referencedTables;
      this._colModelNames = new StringBuilder();
      this._controllerBaseType = controllerBaseType;
      this._isUseWebApi = isUseWebApi;
      this._generatedSqlType = generatedSqlType;
      this._modelName = Functions.GetFullyQualifiedModelName(table, selectedTables, apiName);
      this._businessObjectName = Functions.GetFullyQualifiedTableName(this._table, this._selectedTables, Language.CSharp, this._table.NameFullyQualifiedBusinessObject, apiName);
      this._isSqlVersion2012OrHigher = isSqlVersion2012OrHigher;
      this._isUseLogging = isUseLogging;
      this._isUseCaching = isUseCaching;
      this._isUseAuditLogging = isUseAuditLogging;
      this._isEmailNotification = isEmailNotification;
      if (this._controllerBaseType == ControllerBaseType.ApiControllerBase)
      {
        this._directory = webApiNameDirectory + "\\Controllers\\Base\\";
        this.GenerateApiControllerBase();
      }
      else
      {
        this._directory = webAppRootDirectory + MyConstants.DirectoryControllerBase;
        this.Generate();
      }
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._directory + this._table.Name + MyConstants.WordControllerBase + ".cs"))
      {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("using Microsoft.AspNetCore.Mvc;");
        sb.AppendLine("using System;");
        sb.AppendLine("using System.Linq;");
        sb.AppendLine("using " + this._apiName + "." + MyConstants.WordBusinessObject + ";");
        sb.AppendLine("using " + this._apiName + "." + MyConstants.WordModels + ";");

        if (this._generateFrom == DatabaseObjectToGenerateFrom.Tables)
          sb.AppendLine("using " + this._apiName + ".ViewModels;");
        if (this._appVersion == ApplicationVersion.ProfessionalPlus)
          sb.AppendLine("using " + this._apiName + ".Domain;");
        if (this._isUseWebApi && this._controllerBaseType == ControllerBaseType.ControllerBase)
          sb.AppendLine("using System.Net.Http;");

        sb.AppendLine("using System.Collections.Generic;");
        sb.AppendLine("using Microsoft.Extensions.Options;");
        sb.AppendLine("using System.Threading.Tasks;");
        sb.AppendLine("using Newtonsoft.Json;");
        sb.AppendLine("using System.Text;");

        if (this._isUseLogging)
          sb.AppendLine("using Application_Components.Logging;");
        if (this._isUseCaching)
          sb.AppendLine("using Application_Components.Caching;");
        if (this._isUseAuditLogging)
          sb.AppendLine("using Application_Components.AuditLog;");
        if (this._isEmailNotification)
            sb.AppendLine("using Application_Components.EmailNotification;");
                sb.AppendLine("");
        sb.AppendLine("namespace " + this._webAppName + "." + MyConstants.WordControllersDotBase);
        sb.AppendLine("{");
        sb.AppendLine("     /// <summary>");
        sb.AppendLine("     /// Base class for " + this._table.Name + MyConstants.WordController + ".  Do not make changes to this class,");
        sb.AppendLine("     /// instead, put additional code in the " + this._table.Name + MyConstants.WordController + " class ");
        sb.AppendLine("     /// </summary>");
        sb.AppendLine("     public class " + this._table.Name + MyConstants.WordControllerBase + " : Controller");
        sb.AppendLine("     {");
        sb.AppendLine("");

        if (this._isUseLogging)
          sb.AppendLine("         ILog _Ilog;");
        if (this._isUseCaching)
          sb.AppendLine("         IRedisCacheManager _IRediscache;");
        if (this._isUseAuditLogging)
          sb.AppendLine("         IAuditLog _IAuditLog;");
        if (this._isEmailNotification)
          sb.AppendLine("IEmail _IEmail;");
        this.WriteMethods(sb);
        sb.AppendLine("     }");
        sb.AppendLine("}");
        streamWriter.Write(sb.ToString());
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

        if (this._isUseLogging)
          sb.AppendLine("using Application_Components.Logging;");
        if (this._isUseCaching)
          sb.AppendLine("using Application_Components.Caching;");
        if (this._isUseAuditLogging)
          sb.AppendLine("using Application_Components.AuditLog;");
        if (this._isEmailNotification)
            sb.AppendLine("using Application_Components.EmailNotification;");

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
        sb.AppendLine("");

        if (this._isUseLogging || this._isUseCaching)
        {
          if (this._isUseLogging)
            sb.AppendLine("         ILog _Ilog;");
          if (this._isUseCaching)
            sb.AppendLine("         IRedisCacheManager _IRediscache;");
          if (this._isUseAuditLogging)
            sb.AppendLine("         IAuditLog _IAuditLog;");
          if (this._isEmailNotification)
            sb.AppendLine("IEmail _IEmail;");
          this.WriteConstructor(sb);
        }

        if (this._isCheckedView.AddRecord)
          this.WriteAddPost(sb);
        if (this._isCheckedView.UpdateRecord)
          this.WriteUpdatePost(sb);
        if (this._isCheckedView.ListCrudRedirect || this._isCheckedView.ListCrud)
          this.WriteDeletePost(sb);
        if (this._isCheckedView.AddRecord || this._isCheckedView.UpdateRecord || this._isCheckedView.ListCrud)
          this.WriteAddEdit(sb);
        if (this._isCheckedView.ListSearch)
          this.WriteGetFilteredData(sb);

        if (this._isUseAuditLogging)
          WriteListGetAuditLog(sb);

        this.WriteGridData(sb, "");
        if (this._generateFrom == DatabaseObjectToGenerateFrom.Tables)
        {
          this.WriteGridData(sb, "search");
          this.WriteGridData(sb, "totals");
        }
        if (this._table.ForeignKeyCount > 0)
          this.WriteGridDataForGroupedBy(sb, "", true);
        if (this._table.ForeignKeyCount > 0 && this._table.IsContainsMoneyOrDecimalField)
          this.WriteGridDataForGroupedBy(sb, "totalsgroupedby", true);

        string str = new BusinessObjectBase(this._table, this._selectedTables, this._referencedTables, this._apiName, this._apiNameDirectory, this._generateFrom, this._isUseStoredProcedure, this._appVersion, this._generatedSqlType, BusinessObjectBaseType.MethodsForApiControllerBase).WriteMethodsForControllerBase();
        sb.Append(str);
        sb.AppendLine("     }");
        sb.AppendLine("}");
        streamWriter.Write(sb.ToString());
      }
    }

    private void WriteMethods(StringBuilder sb)
    {
      if (this._appVersion == ApplicationVersion.ProfessionalPlus)
      {
        if (this._isUseLogging || this._isUseCaching)
          this.WriteConstructor(sb);

        this.WriteListGet(sb, "Index", MVCGridViewType.None);
        if (this._isCheckedView.AddRecord)
        {
          this.WriteAddGet(sb);
          this.WriteAddPost(sb);
          this.WriteGetAddViewModel(sb);
        }
        if (this._isCheckedView.UpdateRecord)
        {
          this.WriteUpdateGet(sb);
          this.WriteUpdatePost(sb);
          this.WriteGetUpdateViewModel(sb);
        }
        if (this._isCheckedView.ListCrudRedirect || this._isCheckedView.ListCrud)
          this.WriteDeletePost(sb);
        if (this._isCheckedView.RecordDetails)
          this.WriteDetailsGet(sb);
        if (this._isCheckedView.ListCrudRedirect)
          this.WriteListGet(sb, this._viewNames.ListCrudRedirect, MVCGridViewType.Redirect);
        if (this._isCheckedView.ListReadOnly)
          this.WriteListGet(sb, this._viewNames.ListReadOnly, MVCGridViewType.ReadOnly);
        if (this._isCheckedView.ListCrud)
        {
          this.WriteListGet(sb, this._viewNames.ListCrud, MVCGridViewType.CRUD);
          this.WriteListCrudPost(sb, this._viewNames.ListCrud);
        }
        if (this._table.IsContainsMoneyOrDecimalField && this._isCheckedView.ListTotals)
          this.WriteListGet(sb, this._viewNames.ListTotals, MVCGridViewType.Totals);
        if (this._isCheckedView.ListSearch)
          this.WriteListGet(sb, this._viewNames.ListSearch, MVCGridViewType.Search);
        if (this._isCheckedView.ListScrollLoad)
          this.WriteListGet(sb, this._viewNames.ListScrollLoad, MVCGridViewType.ScrollLoad);
        if (this._isCheckedView.ListInline)
        {
          this.WriteListGet(sb, this._viewNames.ListInline, MVCGridViewType.Inline);
          this.WriteListInlineAddPost(sb);
          this.WriteListInlineUpdatePost(sb);
        }
        if (this._isCheckedView.ListForeach)
          this.WriteListForeach(sb);
        if (this._isCheckedView.ListMasterDetailGrid)
          this.WriteListMasteDetailOrSub(sb, MVCGridViewType.MasterDetailGrid);
        if (this._isCheckedView.ListMasterDetailSubGrid)
          this.WriteListMasteDetailOrSub(sb, MVCGridViewType.MasterDetailSubGrid);
        if (this._isCheckedView.Unbound)
        {
          this.WriteUnboundGet(sb);
          this.WriteUnboundPost(sb);
          this.WriteGetUnboundViewModel(sb);
        }
        if(this._isCheckedView.WorkflowAssignSteps)
                {
                    this.WriteWorkflowGet(sb);
                }
        if (this._isCheckedView.AddRecord || this._isCheckedView.UpdateRecord || this._isCheckedView.ListCrud)
          this.WriteAddEdit(sb);
        if (this._isCheckedView.ListCrud || this._isCheckedView.ListSearch)
          this.WriteGetViewModel(sb);
        if (!this._isUseWebApi && this._isCheckedView.ListSearch)
          this.WriteGetFilteredData(sb);
        if (!this._isUseWebApi)
        {
          if (this._isCheckedView.ListCrud || this._isCheckedView.ListCrudRedirect || this._isCheckedView.ListReadOnly)
            this.WriteGridData(sb, "");
          if (this._isCheckedView.ListSearch)
            this.WriteGridData(sb, "search");
          if (this._table.IsContainsMoneyOrDecimalField && this._isCheckedView.ListTotals)
            this.WriteGridData(sb, "totals");
          if (this._table.ForeignKeyCount > 0 && this._isCheckedView.ListGroupedBy)
            this.WriteGridDataForGroupedBy(sb, "", true);
          if (this._table.ForeignKeyCount > 0 && this._table.IsContainsMoneyOrDecimalField && this._isCheckedView.ListTotalsGroupedBy)
            this.WriteGridDataForGroupedBy(sb, "totalsgroupedby", true);
        }
        else
        {
          if (this._table.ForeignKeyCount > 0 && this._isCheckedView.ListGroupedBy)
            this.WriteGridDataForGroupedBy(sb, "", false);
          if (this._table.ForeignKeyCount > 0 && this._table.IsContainsMoneyOrDecimalField && this._isCheckedView.ListTotalsGroupedBy)
            this.WriteGridDataForGroupedBy(sb, "totalsgroupedby", false);
          if (this._generateFrom == DatabaseObjectToGenerateFrom.Tables)
          {
            this.WriteGetModelByPrimaryKeyWebApiCall(sb);
            this.WriteGetDropDownListWebApiHttpClientCallsForFks(sb);
          }
        }
      }
      if (this._appVersion != ApplicationVersion.Express)
        return;
      this.WriteListGet(sb, "Index", MVCGridViewType.None);
      this.WriteUnboundGet(sb);
      this.WriteUnboundPost(sb);
      this.WriteGetUnboundViewModel(sb);
    }

    private void WriteListGet(StringBuilder sb, string action, MVCGridViewType mvcGridViewType)
    {
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      if (action == "Index")
        sb.AppendLine("         /// GET: /" + this._table.Name + "/");
      else
        sb.AppendLine("         /// GET: /" + this._table.Name + "/" + action);
      sb.AppendLine("         /// </summary>");
      if (mvcGridViewType == MVCGridViewType.Search || mvcGridViewType == MVCGridViewType.CRUD || mvcGridViewType == MVCGridViewType.Inline)
      {
        sb.AppendLine("         public IActionResult " + action + "()");
        sb.AppendLine("         {");
        sb.AppendLine("             return View(GetViewModel(\"" + action + "\"));");
        sb.AppendLine("         }");
      }
      else
      {
        sb.AppendLine("         public IActionResult " + action + "()");
        sb.AppendLine("         {");
        sb.AppendLine("             return View();");
        sb.AppendLine("         }");
      }
    }

    private void WriteListForeach(StringBuilder sb)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (Column column in this._table.Columns)
        stringBuilder.AppendLine("                 {\"" + column.Name + "\", \"" + column.NameWithSpaces + "\"},");
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// GET: /" + this._table.Name + "/" + this._viewNames.ListForeach);
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         public IActionResult " + this._viewNames.ListForeach + "(string sidx, string sord, int? page)");
      sb.AppendLine("         {");
      sb.AppendLine("             int rows = Functions.GetGridNumberOfRows();");
      sb.AppendLine("             int numberOfPagesToShow = Functions.GetGridNumberOfPagesToShow();");
      sb.AppendLine("             int currentPage = page is null ? 1 : Convert.ToInt32(page);");
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
        sb.AppendLine("                 HttpResponseMessage response = client.GetAsync(\"" + this._table.Name + "Api/SelectSkipAndTake/?rows=\" + rows + \"&startRowIndex=\" + startRowIndex + \"&page=\" + currentPage + \"&sidx=\" + sidx + \"&sord=\" + sord + \"&isforJqGrid=\" + isforJqGrid).Result;");
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
      sb.AppendLine("             // view model");
      sb.AppendLine("             " + this._table.Name + "ForeachViewModel viewModel = new " + this._table.Name + "ForeachViewModel();");
      sb.AppendLine("             viewModel." + this._table.Name + "Data = " + this._table.VariableObjCollectionName + ";");
      sb.AppendLine("             viewModel." + this._table.Name + "FieldNames = fieldNames;");
      sb.AppendLine("             viewModel.TotalPages = totalPages;");
      sb.AppendLine("             viewModel.CurrentPage = currentPage;");
      sb.AppendLine("             viewModel.FieldToSort = String.IsNullOrEmpty(sidx) ? \"" + this._table.FirstPrimaryKeyName + "\" : sidx;");
      sb.AppendLine("             viewModel.FieldSortOrder = String.IsNullOrEmpty(sord) ? \"asc\" : sord;");
      sb.AppendLine("             viewModel.FieldToSortWithOrder = String.IsNullOrEmpty(sidx) ? \"" + this._table.FirstPrimaryKeyName + "\" : (sidx + \" \" + sord).Trim();");
      sb.AppendLine("             viewModel.NumberOfPagesToShow = numberOfPagesToShow;");
      sb.AppendLine("             viewModel.StartPage = Functions.GetPagerStartPage(currentPage, numberOfPagesToShow, totalPages);");
      sb.AppendLine("             viewModel.EndPage = Functions.GetPagerEndPage(viewModel.StartPage, currentPage, numberOfPagesToShow, totalPages);");
      if (this._table.IsContainsBinaryOrSpatialDataTypes)
        sb.AppendLine("             viewModel.UnsortableFields = unsortableFields;");
      sb.AppendLine("");
      sb.AppendLine("             return View(viewModel);");
      sb.AppendLine("         }");
    }

    private void WriteListInlineAddPost(StringBuilder sb)
    {
      if (this._controllerBaseType != ControllerBaseType.ControllerBase)
        return;
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// POST: /" + this._table.Name + "/" + this._viewNames.ListInline + "Add");
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         [HttpPost]");
      sb.AppendLine("         public IActionResult " + this._viewNames.ListInline + "Add([FromBody]" + this._modelName + " model)");
      sb.AppendLine("         {");
      sb.AppendLine("             " + this._table.Name + "ViewModel viewModel = new " + this._table.Name + "ViewModel();");
      sb.AppendLine("             viewModel." + this._table.Name + MyConstants.WordModel + " = model;");
      sb.AppendLine("");
      sb.AppendLine("             AddEdit" + this._table.Name + "(viewModel, CrudOperation.Add, true);");
      sb.AppendLine("             return Json(\"\");");
      sb.AppendLine("         }");
    }

    private void WriteListInlineUpdatePost(StringBuilder sb)
    {
      if (this._controllerBaseType != ControllerBaseType.ControllerBase)
        return;
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// POST: /" + this._table.Name + "/" + this._viewNames.ListInline + "Update");
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         [HttpPost]");
      sb.AppendLine("         public IActionResult " + this._viewNames.ListInline + "Update([FromBody]" + this._modelName + " model)");
      sb.AppendLine("         {");
      sb.AppendLine("             " + this._table.Name + "ViewModel viewModel = new " + this._table.Name + "ViewModel();");
      sb.AppendLine("             viewModel." + this._table.Name + MyConstants.WordModel + " = model;");
      sb.AppendLine("");
      sb.AppendLine("             AddEdit" + this._table.Name + "(viewModel, CrudOperation.Update, true);");
      sb.AppendLine("             return Json(\"\");");
      sb.AppendLine("         }");
    }

    private void WriteListMasteDetailOrSub(StringBuilder sb, MVCGridViewType mvcGridViewType)
    {
      foreach (Column column in this._table.Columns)
      {
        if (column.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK)
        {
          string action = string.Empty;
          switch (mvcGridViewType)
          {
            case MVCGridViewType.MasterDetailGrid:
              action = this._viewNames.ListMasterDetailGrid + column.Name;
              break;
            case MVCGridViewType.MasterDetailSubGrid:
              action = this._viewNames.ListMasterDetailSubGrid + column.Name;
              break;
          }
          if (this._controllerBaseType == ControllerBaseType.ControllerBase)
            this.WriteListGet(sb, action, mvcGridViewType);
        }
      }
    }

    private void WriteUnboundGet(StringBuilder sb)
    {
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// GET: /" + this._table.Name + "/" + this._viewNames.Unbound);
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         public IActionResult " + this._viewNames.Unbound + "()");
      sb.AppendLine("         {");
      sb.AppendLine("             return View(GetUnboundViewModel());");
      sb.AppendLine("         }");
    }

    private void WriteDetailsGet(StringBuilder sb)
    {
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// GET: /" + this._table.Name + "/" + this._viewNames.RecordDetails + "/5");
      sb.AppendLine("         /// </summary>");
      this.WriteUpdatePostAndDetailsBody(sb, false);
    }

    private void WriteUnboundPost(StringBuilder sb)
    {
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// POST: /" + this._table.Name + "/" + this._viewNames.Unbound);
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         [HttpPost]");
      sb.AppendLine("         public IActionResult " + this._viewNames.Unbound + "(" + this._table.Name + MyConstants.WordPropertyModel + " viewModel, string returnUrl)");
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
      sb.AppendLine("         /// GET: /" + this._table.Name + "/" + this._viewNames.AddRecord);
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         public IActionResult " + this._viewNames.AddRecord + "()");
      sb.AppendLine("         {");
      sb.AppendLine("             return GetAddViewModel();");
      sb.AppendLine("         }");
    }
        private void WriteWorkflowGet(StringBuilder sb)
        {
            sb.AppendLine("");
            sb.AppendLine("         /// <summary>");
            sb.AppendLine("         /// GET: /" + this._table.Name + "/" + this._viewNames.WorkflowAssignSteps);
            sb.AppendLine("         /// </summary>");
            sb.AppendLine("         public IActionResult " + this._viewNames.WorkflowAssignSteps + "()");
            sb.AppendLine("         {");
            sb.AppendLine("             return GetAssignViewModel();");
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
      sb.AppendLine("         /// Default Constructor: /" + this._table.Name + MyConstants.WordApi + MyConstants.WordControllerBase);
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         public " + this._table.Name + MyConstants.WordApi + MyConstants.WordControllerBase + " (" + paramlist + ")");
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

    private void WriteAddPost(StringBuilder sb)
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
        if (this._isUseAuditLogging)
          sb.AppendLine("             return AddEdit" + this._table.Name + "(model, CrudOperation.Add, _IAuditLog, isForListInline);");
        else
          sb.AppendLine("             return AddEdit" + this._table.Name + "(model, CrudOperation.Add, isForListInline);");
        sb.AppendLine("         }");
      }
      else
      {
        sb.AppendLine("");
        sb.AppendLine("         /// <summary>");
        sb.AppendLine("         /// POST: /" + this._table.Name + "/" + this._viewNames.AddRecord);
        sb.AppendLine("         /// </summary>");
        sb.AppendLine("         [HttpPost]");
        sb.AppendLine("         public IActionResult " + this._viewNames.AddRecord + "(" + this._table.Name + MyConstants.WordPropertyModel + " viewModel, string returnUrl)");
        sb.AppendLine("         {");
        sb.AppendLine("             if (ModelState.IsValid)");
        sb.AppendLine("             {");
        sb.AppendLine("                 try");
        sb.AppendLine("                 {");
        sb.AppendLine("                     // add new record");
        sb.AppendLine("                     AddEdit" + this._table.Name + "(viewModel, CrudOperation.Add);");
        sb.AppendLine("");
        sb.AppendLine("                     if (Url.IsLocalUrl(returnUrl))");
        sb.AppendLine("                         return Redirect(returnUrl);");
        sb.AppendLine("                     else");
        sb.AppendLine("                         return RedirectToAction(\"" + this._viewNames.ListCrudRedirect + "\", \"" + this._table.Name + "\");");
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
        sb.AppendLine("             return GetAddViewModel();");
        sb.AppendLine("         }");
      }
    }

    private void WriteGetAddViewModel(StringBuilder sb)
    {
      sb.AppendLine("");
      sb.AppendLine("         private IActionResult GetAddViewModel()");
      sb.AppendLine("         {");
      sb.AppendLine("             " + this._table.Name + MyConstants.WordPropertyModel + " viewModel = new " + this._table.Name + MyConstants.WordPropertyModel + "();");
      sb.AppendLine("             viewModel." + this._table.Name + MyConstants.WordModel + " = null;");
      sb.AppendLine("             viewModel.Operation = CrudOperation.Add;");
      sb.AppendLine("             viewModel.ViewControllerName = \"" + this._table.Name + "\";");
      sb.AppendLine("             viewModel.ViewActionName = \"" + this._viewNames.AddRecord + "\";");
      this.WriteViewModelAssignments(sb);
      sb.AppendLine("             viewModel.ViewReturnUrl = \"/" + this._table.Name + "/" + this._viewNames.ListCrudRedirect + "\";");
      sb.AppendLine("");
      sb.AppendLine("             return View(viewModel);");
      sb.AppendLine("         }");
    }
        private void WriteGetAssignViewModel(StringBuilder sb)
        {
            sb.AppendLine("");
            sb.AppendLine("         private IActionResult GetAssignViewModel()");
            sb.AppendLine("         {");
            sb.AppendLine("             " + this._table.Name + MyConstants.WordPropertyModel + " viewModel = new " + this._table.Name + MyConstants.WordPropertyModel + "();");
            sb.AppendLine("             viewModel." + this._table.Name + MyConstants.WordModel + " = null;");
            sb.AppendLine("             viewModel.Operation = CrudOperation.Add;");
            sb.AppendLine("             viewModel.ViewControllerName = \"" + this._table.Name + "\";");
            sb.AppendLine("             viewModel.ViewActionName = \"" + this._viewNames.WorkflowAssignSteps + "\";");
            this.WriteViewModelAssignments(sb);
            sb.AppendLine("             viewModel.ViewReturnUrl = \"/" + this._table.Name + "/" + this._viewNames.WorkflowAssignSteps + "\";");
            sb.AppendLine("");
            sb.AppendLine("             return View(viewModel);");
            sb.AppendLine("         }");
        }

        private void WriteUpdateGet(StringBuilder sb)
    {
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// GET: /" + this._table.Name + "/" + this._viewNames.UpdateRecord + "/5");
      sb.AppendLine("         /// </summary>");
      if (this._table.PrimaryKeyCount > 1)
        sb.AppendLine("         public IActionResult " + this._viewNames.UpdateRecord + "(" + Functions.GetCommaDelimitedPrimaryKeysAndType(this._table, Language.CSharp) + ")");
      else
        sb.AppendLine("         public IActionResult " + this._viewNames.UpdateRecord + "(" + this._table.FirstPrimaryKeySystemType + " id)");
      sb.AppendLine("         {");
      if (this._table.PrimaryKeyCount > 1)
        sb.AppendLine("             return GetUpdateViewModel(" + Functions.GetCommaDelimitedPrimaryKeys(this._table, Language.CSharp, true, false, false) + ");");
      else
        sb.AppendLine("             return GetUpdateViewModel(id);");
      sb.AppendLine("         }");
    }

    private void WriteUpdatePostAndDetailsBody(StringBuilder sb, bool isFromGetUpdateViewModel)
    {
      string str1 = this._viewNames.RecordDetails;
      if (isFromGetUpdateViewModel)
        str1 = "GetUpdateViewModel";
      if (this._table.PrimaryKeyCount > 1)
        sb.AppendLine("         public IActionResult " + str1 + "(" + Functions.GetCommaDelimitedPrimaryKeysAndType(this._table, Language.CSharp) + ")");
      else
        sb.AppendLine("         public IActionResult " + str1 + "(" + this._table.FirstPrimaryKeySystemType + " id)");
      sb.AppendLine("         {");
      if (this._isUseWebApi)
      {
        string str2 = "id";
        if (this._table.PrimaryKeyCount > 1)
          str2 = Functions.GetCommaDelimitedPrimaryKeyParameters(this._table, Language.CSharp, false, this._generatedSqlType, false);
        sb.AppendLine("             " + this._modelName + " model = GetModelByPrimaryKey(" + str2 + ");");
      }
      else
      {
        sb.AppendLine("             // select a record by primary key(s)");
        if (this._table.PrimaryKeyCount > 1)
          sb.AppendLine("             " + this._businessObjectName + " " + this._table.VariableObjName + " = " + this._businessObjectName + ".SelectByPrimaryKey(" + Functions.GetCommaDelimitedPrimaryKeys(this._table, Language.CSharp, true, false, false) + ");");
        else
          sb.AppendLine("             " + this._businessObjectName + " " + this._table.VariableObjName + " = " + this._businessObjectName + ".SelectByPrimaryKey(id);");
        sb.AppendLine("");
        sb.AppendLine("             // assign values to the model");
        sb.AppendLine("             " + this._modelName + " model = new " + this._modelName + "();");
        foreach (Column column in this._table.Columns)
        {
          sb.AppendLine("             model." + column.Name + " = " + this._table.VariableObjName + "." + column.Name + ";");
          if (column.IsNullable && !column.IsPrimaryKeyUnique && !column.IsForeignKey && (column.SQLDataType == SQLType.integer || column.SQLDataType == SQLType.bigint || (column.SQLDataType == SQLType.smallint || column.SQLDataType == SQLType.tinyint)))
          {
            sb.AppendLine("");
            sb.AppendLine("             if (" + this._table.VariableObjName + "." + column.Name + ".HasValue)");
            sb.AppendLine("                 model." + column.Name + "Hidden = " + this._table.VariableObjName + "." + column.Name + ".Value.ToString();");
            sb.AppendLine("             else");
            sb.AppendLine("                 model." + column.Name + "Hidden = null;");
            sb.AppendLine("");
          }
        }
      }
      sb.AppendLine("");
      sb.AppendLine("             // assign values to the view model");
      sb.AppendLine("             " + this._table.Name + MyConstants.WordPropertyModel + " viewModel = new " + this._table.Name + MyConstants.WordPropertyModel + "();");
      sb.AppendLine("             viewModel." + this._table.Name + MyConstants.WordModel + " = model;");
      if (isFromGetUpdateViewModel)
      {
        sb.AppendLine("             viewModel.Operation = CrudOperation.Update;");
        sb.AppendLine("             viewModel.ViewControllerName = \"" + this._table.Name + "\";");
        sb.AppendLine("             viewModel.ViewActionName = \"" + this._viewNames.UpdateRecord + "\";");
      }
      if (this._isUseWebApi)
        sb.Append(this.GetWebApiForeignKeyViewAssignments());
      else
        this.WriteViewModelAssignments(sb);
      sb.AppendLine("");
      sb.AppendLine("             viewModel.ViewReturnUrl = \"/" + this._table.Name + "/" + this._viewNames.ListCrudRedirect + "\";");
      sb.AppendLine("             viewModel.URLReferrer = Request.Headers[\"Referer\"].ToString();");
      sb.AppendLine("");
      sb.AppendLine("             return View(viewModel);");
      sb.AppendLine("         }");
    }

    private void WriteUpdatePost(StringBuilder sb)
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
        if (this._isUseAuditLogging)
          sb.AppendLine("             return AddEdit" + this._table.Name + "(model, CrudOperation.Update, _IAuditLog, isForListInline);");
        else
          sb.AppendLine("             return AddEdit" + this._table.Name + "(model, CrudOperation.Update, isForListInline);");
        sb.AppendLine("         }");
      }
      else
      {
        sb.AppendLine("         /// <summary>");
        sb.AppendLine("         /// POST: /" + this._table.Name + "/" + this._viewNames.UpdateRecord + "/5");
        sb.AppendLine("         /// </summary>");
        sb.AppendLine("         [HttpPost]");
        if (this._table.PrimaryKeyCount > 1)
          sb.AppendLine("         public IActionResult " + this._viewNames.UpdateRecord + "(" + Functions.GetCommaDelimitedPrimaryKeysAndType(this._table, Language.CSharp) + ", " + this._table.Name + MyConstants.WordPropertyModel + " viewModel, string returnUrl)");
        else
          sb.AppendLine("         public IActionResult " + this._viewNames.UpdateRecord + "(" + this._table.FirstPrimaryKeySystemType + " id, " + this._table.Name + MyConstants.WordPropertyModel + " viewModel, string returnUrl)");
        sb.AppendLine("         {");
        sb.AppendLine("             if (ModelState.IsValid)");
        sb.AppendLine("             {");
        sb.AppendLine("                 try");
        sb.AppendLine("                 {");
        sb.AppendLine("                     // update record");
        sb.AppendLine("                     AddEdit" + this._table.Name + "(viewModel, CrudOperation.Update);");
        sb.AppendLine("");
        sb.AppendLine("                     if (Url.IsLocalUrl(returnUrl))");
        sb.AppendLine("                         return Redirect(returnUrl);");
        sb.AppendLine("                     else");
        sb.AppendLine("                         return RedirectToAction(\"" + this._viewNames.ListCrudRedirect + "\", \"" + this._table.Name + "\");");
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
          sb.AppendLine("             return GetUpdateViewModel(" + Functions.GetCommaDelimitedPrimaryKeys(this._table, Language.CSharp, true, false, false) + ");");
        else
          sb.AppendLine("             return GetUpdateViewModel(id);");
        sb.AppendLine("         }");
      }
    }

    private void WriteGetUpdateViewModel(StringBuilder sb)
    {
      sb.AppendLine("");
      this.WriteUpdatePostAndDetailsBody(sb, true);
    }

    private void WriteAddEdit(StringBuilder sb)
    {
      sb.AppendLine("");
      if (this._controllerBaseType == ControllerBaseType.ApiControllerBase)
      {
        if (this._isUseAuditLogging)
          sb.AppendLine("         private IActionResult AddEdit" + this._table.Name + "(" + this._modelName + " model, CrudOperation operation, IAuditLog _IAuditLog, bool isForListInline = false)");
        else
          sb.AppendLine("         private IActionResult AddEdit" + this._table.Name + "(" + this._modelName + " model, CrudOperation operation, bool isForListInline = false)");
        sb.AppendLine("         {");
      }
      else
      {
        sb.AppendLine("         /// <summary>");
        sb.AppendLine("         /// GET: /" + this._table.Name + "/AddEdit" + this._table.Name);
        sb.AppendLine("         /// </summary>");
        sb.AppendLine("         private void AddEdit" + this._table.Name + "(" + this._table.Name + MyConstants.WordPropertyModel + " viewModel, CrudOperation operation, bool isForListInline = false)");
        sb.AppendLine("         {");
        sb.AppendLine("             " + this._modelName + " model = viewModel." + this._table.Name + MyConstants.WordModel + ";");
      }
      if (this._isUseWebApi && this._controllerBaseType == ControllerBaseType.ControllerBase)
      {
        sb.AppendLine("             string serializedModel = JsonConvert.SerializeObject(model);");
        sb.AppendLine("");
        sb.AppendLine("             using (var client = new HttpClient())");
        sb.AppendLine("             {");
        sb.AppendLine("                 client.BaseAddress = new Uri(Functions.GetWebApiBaseAddress());");
        sb.AppendLine("                 HttpResponseMessage response;");
        sb.AppendLine("");
        sb.AppendLine("                 if (operation == CrudOperation.Add)");
        sb.AppendLine("                     response = client.PostAsync(\"" + this._table.Name + "Api/Insert?isForListInline=\" + isForListInline, new StringContent(serializedModel, Encoding.UTF8, \"application/json\")).Result;");
        sb.AppendLine("                 else");
        sb.AppendLine("                     response = client.PostAsync(\"" + this._table.Name + "Api/Update?isForListInline=\" + isForListInline, new StringContent(serializedModel, Encoding.UTF8, \"application/json\")).Result;");
        sb.AppendLine("");
        sb.AppendLine("                 if (!response.IsSuccessStatusCode)");
        sb.AppendLine("                 {");
        sb.AppendLine("                     throw new Exception(\"Error Status Code: \" + response.StatusCode.ToString() + \" Error Reason: \" + response.ReasonPhrase + \" Error Message: \" + response.RequestMessage.ToString());");
        sb.AppendLine("                 }");
        sb.AppendLine("             }");
      }
      else if (this._isUseWebApi)
      {
        sb.AppendLine("             try");
        sb.AppendLine("             {");
        sb.AppendLine("                 " + this._businessObjectName + " " + this._table.VariableObjName + ";");
        sb.AppendLine("                 " + this._businessObjectName + " " + this._table.VariableObjName + "Old = new " + this._businessObjectName + "();");
        sb.AppendLine("                 decimal id = 0;");
        sb.AppendLine("");
        sb.AppendLine("                 if (operation == CrudOperation.Add)");
        sb.AppendLine("                    " + this._table.VariableObjName + " = new " + this._businessObjectName + "();");
        sb.AppendLine("                 else");
        sb.AppendLine("                 {");
        if (this._table.PrimaryKeyCount > 1)
          sb.AppendLine("                     " + this._table.VariableObjName + " = " + this._businessObjectName + ".SelectByPrimaryKey(" + Functions.GetCommaDelimitedModelDotPrimaryKeys(this._table, this._generatedSqlType) + ");");
        else if (this._generatedSqlType == GeneratedSqlType.EFCore && this._table.FirstPrimaryKeySQLDataType == SQLType.uniqueidentifier)
          sb.AppendLine("                     " + this._table.VariableObjName + " = " + this._businessObjectName + ".SelectByPrimaryKey(model." + this._table.FirstPrimaryKeyName + ".ToString());");
        else
          sb.AppendLine("                     " + this._table.VariableObjName + " = " + this._businessObjectName + ".SelectByPrimaryKey(model." + this._table.FirstPrimaryKeyName + ");");
        sb.AppendLine("                     " + this._table.VariableObjName + "Old = " + this._table.VariableObjName + ".ShallowCopy();");
        sb.AppendLine("                 }");
        sb.AppendLine("");
        StringBuilder stringBuilder1 = new StringBuilder();
        StringBuilder stringBuilder2 = new StringBuilder();
        bool flag = false;
        foreach (Column column in this._table.Columns)
        {
          if (!column.IsComputed && column.IsNullable && (!column.IsPrimaryKeyUnique && !column.IsForeignKey) && (column.SQLDataType == SQLType.integer || column.SQLDataType == SQLType.bigint || (column.SQLDataType == SQLType.smallint || column.SQLDataType == SQLType.tinyint)))
          {
            flag = true;
            break;
          }
        }
        foreach (Column column in this._table.Columns)
        {
          if (!column.IsComputed)
          {
            if (column.IsNullable && !column.IsPrimaryKeyUnique && !column.IsForeignKey && (column.SQLDataType == SQLType.integer || column.SQLDataType == SQLType.bigint || (column.SQLDataType == SQLType.smallint || column.SQLDataType == SQLType.tinyint)))
            {
              string str = "Byte";
              if (column.SQLDataType == SQLType.integer)
                str = "Int32";
              else if (column.SQLDataType == SQLType.bigint)
                str = "Int64";
              else if (column.SQLDataType == SQLType.smallint)
                str = "Int16";
              stringBuilder2.AppendLine("                     if(!String.IsNullOrEmpty(model." + column.Name + "Hidden))");
              stringBuilder2.AppendLine("                        " + this._table.VariableObjName + "." + column.Name + " = Convert.To" + str + "(model." + column.Name + "Hidden);");
              stringBuilder2.AppendLine("                     else");
              stringBuilder2.AppendLine("                        " + this._table.VariableObjName + "." + column.Name + " = null;");
              stringBuilder2.AppendLine("");
              stringBuilder1.AppendLine("                     " + this._table.VariableObjName + "." + column.Name + " = model." + column.Name + ";");
            }
            else
              sb.AppendLine("                 " + this._table.VariableObjName + "." + column.Name + " = model." + column.Name + ";");
          }
        }
        if (flag)
        {
          sb.AppendLine("");
          sb.AppendLine("                 if (isForListInline)");
          sb.AppendLine("                 {");
          sb.Append(stringBuilder1.ToString());
          sb.AppendLine("                 }");
          sb.AppendLine("                 else");
          sb.AppendLine("                 {");
          sb.Append(stringBuilder2.ToString());
          sb.AppendLine("                 }");
        }
        sb.AppendLine("");
        sb.AppendLine("                 if (operation == CrudOperation.Add)");
        sb.AppendLine("                 {");
        sb.AppendLine("                      id = " + this._table.VariableObjName + ".Insert();");
        if (this._isUseAuditLogging && this._isUseWebApi)
          sb.AppendLine("                     _IAuditLog.CreateAuditTrail(_IAuditLog.GetAuditActionTypeValue(\"Create\"), Convert.ToInt32(id), \"" + this._table.Name + "\", null, " + this._table.VariableObjName + ", 0, 100);");
        sb.AppendLine("                 }");
        sb.AppendLine("                 else");
        sb.AppendLine("                 {");
        sb.AppendLine("                      " + this._table.VariableObjName + ".Update();");
        if (this._isUseAuditLogging && this._isUseWebApi)
          sb.AppendLine("                     _IAuditLog.CreateAuditTrail(_IAuditLog.GetAuditActionTypeValue(\"Update\"), Convert.ToInt32(" + this._table.VariableObjName + "." + Functions.GetCommaDelimitedTableNameDotPrimaryKeys(this._table).Split('.')[1] + "), \"" + this._table.Name + "\", " + this._table.VariableObjName + "Old, " + this._table.VariableObjName + ", 0, 100);");
        sb.AppendLine("                 }");
        if (this._controllerBaseType == ControllerBaseType.ApiControllerBase)
        {
          sb.AppendLine("");
          sb.AppendLine("                 return Ok();");
        }
        sb.AppendLine("             }");
        sb.AppendLine("             catch (Exception ex)");
        sb.AppendLine("             {");
        if (this._isUseLogging)
          sb.AppendLine("                 _Ilog.GetInstance().Error(\"Error Occured\", ex);");
        sb.AppendLine("");
        sb.AppendLine("                 return BadRequest(\"Error Message: \" + ex.Message + \" Stack Trace: \" + ex.StackTrace);");
        sb.AppendLine("             }");
      }
      else
      {
        sb.AppendLine("             " + this._businessObjectName + " " + this._table.VariableObjName + ";");
        sb.AppendLine("             " + this._businessObjectName + " " + this._table.VariableObjName + "Old = new " + this._businessObjectName + "();");
        sb.AppendLine("             decimal id = 0;");
        sb.AppendLine("");
        sb.AppendLine("             if (operation == CrudOperation.Add)");
        sb.AppendLine("                id = " + this._table.VariableObjName + " = new " + this._businessObjectName + "();");
        sb.AppendLine("             else");
        sb.AppendLine("             {");
        if (this._table.PrimaryKeyCount > 1)
          sb.AppendLine("                 " + this._table.VariableObjName + " = " + this._businessObjectName + ".SelectByPrimaryKey(" + Functions.GetCommaDelimitedModelDotPrimaryKeys(this._table, this._generatedSqlType) + ");");
        else if (this._generatedSqlType == GeneratedSqlType.EFCore && this._table.FirstPrimaryKeySQLDataType == SQLType.uniqueidentifier)
          sb.AppendLine("                 " + this._table.VariableObjName + " = " + this._businessObjectName + ".SelectByPrimaryKey(model." + this._table.FirstPrimaryKeyName + ".ToString());");
        else
          sb.AppendLine("                 " + this._table.VariableObjName + " = " + this._businessObjectName + ".SelectByPrimaryKey(model." + this._table.FirstPrimaryKeyName + ");");
        sb.AppendLine("                 " + this._table.VariableObjName + "Old = " + this._table.VariableObjName + ".ShallowCopy(); ");
        sb.AppendLine("             }");
        sb.AppendLine("");
        StringBuilder stringBuilder1 = new StringBuilder();
        StringBuilder stringBuilder2 = new StringBuilder();
        bool flag = false;
        foreach (Column column in this._table.Columns)
        {
          if (!column.IsComputed && column.IsNullable && (!column.IsPrimaryKeyUnique && !column.IsForeignKey) && (column.SQLDataType == SQLType.integer || column.SQLDataType == SQLType.bigint || (column.SQLDataType == SQLType.smallint || column.SQLDataType == SQLType.tinyint)))
          {
            flag = true;
            break;
          }
        }
        foreach (Column column in this._table.Columns)
        {
          if (!column.IsComputed)
          {
            if (column.IsNullable && !column.IsPrimaryKeyUnique && !column.IsForeignKey && (column.SQLDataType == SQLType.integer || column.SQLDataType == SQLType.bigint || (column.SQLDataType == SQLType.smallint || column.SQLDataType == SQLType.tinyint)))
            {
              string str = "Byte";
              if (column.SQLDataType == SQLType.integer)
                str = "Int32";
              else if (column.SQLDataType == SQLType.bigint)
                str = "Int64";
              else if (column.SQLDataType == SQLType.smallint)
                str = "Int16";
              stringBuilder2.AppendLine("                 if(!String.IsNullOrEmpty(model." + column.Name + "Hidden))");
              stringBuilder2.AppendLine("                    " + this._table.VariableObjName + "." + column.Name + " = Convert.To" + str + "(model." + column.Name + "Hidden);");
              stringBuilder2.AppendLine("                 else");
              stringBuilder2.AppendLine("                    " + this._table.VariableObjName + "." + column.Name + " = null;");
              stringBuilder2.AppendLine("");
              stringBuilder1.AppendLine("                 " + this._table.VariableObjName + "." + column.Name + " = model." + column.Name + ";");
            }
            else
              sb.AppendLine("             " + this._table.VariableObjName + "." + column.Name + " = model." + column.Name + ";");
          }
        }
        if (flag)
        {
          sb.AppendLine("");
          sb.AppendLine("             if (isForListInline)");
          sb.AppendLine("             {");
          sb.Append(stringBuilder1.ToString());
          sb.AppendLine("             }");
          sb.AppendLine("             else");
          sb.AppendLine("             {");
          sb.Append(stringBuilder2.ToString());
          sb.AppendLine("             }");
        }
        sb.AppendLine("");
        sb.AppendLine("             if (operation == CrudOperation.Add)");
        sb.AppendLine("             {");
        sb.AppendLine("                  id = " + this._table.VariableObjName + ".Insert();");
        if (this._isUseAuditLogging && this._isUseWebApi)
          sb.AppendLine("               _IAuditLog.CreateAuditTrail(_IAuditLog.GetAuditActionTypeValue(\"Create\"), Convert.ToInt32(id), \"" + this._table.Name + "\", null, " + this._table.VariableObjName + ", 0, 100);");
        sb.AppendLine("             }");
        sb.AppendLine("             else");
        sb.AppendLine("             {");
        sb.AppendLine("                  " + this._table.VariableObjName + ".Update();");
        if (this._isUseAuditLogging && this._isUseWebApi)
          sb.AppendLine("               _IAuditLog.CreateAuditTrail(_IAuditLog.GetAuditActionTypeValue(\"Update\"), Convert.ToInt32(" + this._table.VariableObjName + "." + Functions.GetCommaDelimitedTableNameDotPrimaryKeys(this._table).Split('.')[1] + "), \"" + this._table.Name + "\", " + this._table.VariableObjName + "Old, " + this._table.VariableObjName + ", 0, 100);");
        sb.AppendLine("             }");
      }
      sb.AppendLine("         }");
    }

    private void WriteDeletePost(StringBuilder sb)
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
        sb.AppendLine("         /// Deletes an existing record by primary key");
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
        if (this._isUseLogging)
          sb.AppendLine("                 _Ilog.GetInstance().Error(\"Error Occured\", ex);");
        sb.AppendLine("");
        sb.AppendLine("                 return BadRequest(\"Error Message: \" + ex.Message + \" Stack Trace: \" + ex.StackTrace);");
        sb.AppendLine("             }");
        sb.AppendLine("         }");
      }
      else
      {
        sb.AppendLine("");
        sb.AppendLine("         /// <summary>");
        sb.AppendLine("         /// POST: /" + this._table.Name + "/Delete/5");
        sb.AppendLine("         /// </summary>");
        sb.AppendLine("         [HttpPost]");
        sb.AppendLine("         public IActionResult Delete(" + str2 + ", " + this._table.Name + MyConstants.WordPropertyModel + " viewModel, string returnUrl)");
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
          sb.AppendLine("                 return Json(true);");
          sb.AppendLine("             }");
        }
        else
        {
          sb.AppendLine("             " + this._businessObjectName + ".Delete(" + str1 + ");");
          sb.AppendLine("             return Json(true);");
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
        sb.AppendLine("             viewModel.ViewActionName = \"" + this._viewNames.Unbound + "\";");
        sb.AppendLine("             viewModel.ViewReturnUrl = \"/Home\";");
      }
      sb.AppendLine("");
      sb.AppendLine("             return viewModel;");
      sb.AppendLine("         }");
    }

    private void WriteGetViewModel(StringBuilder sb)
    {
      sb.AppendLine("");
      sb.AppendLine("         private " + this._table.Name + "ViewModel GetViewModel(string actionName)");
      sb.AppendLine("         {");
      sb.AppendLine("             " + this._table.Name + MyConstants.WordPropertyModel + " viewModel = new " + this._table.Name + MyConstants.WordPropertyModel + "();");
      sb.AppendLine("             viewModel." + this._table.Name + MyConstants.WordModel + " = null;");
      sb.AppendLine("             viewModel.ViewControllerName = \"" + this._table.Name + "\";");
      sb.AppendLine("             viewModel.ViewActionName = actionName;");
      if (this._isUseWebApi)
        sb.Append(this.GetWebApiForeignKeyViewAssignments());
      else
        this.WriteViewModelAssignments(sb);
      sb.AppendLine("");
      sb.AppendLine("             return viewModel;");
      sb.AppendLine("         }");
    }

    private void WriteGridData(StringBuilder sb, string action = "")
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
        sb.AppendLine("         /// GET: /" + this._table.Name + "/GridData" + str1);
        sb.AppendLine("         /// </summary>");
        if (string.IsNullOrEmpty(str1))
          sb.AppendLine("         public IActionResult GridData" + str1 + "(string sidx, string sord, int _page, int rows" + str2 + ", bool isforJqGrid = true)");
        else
          sb.AppendLine("         public IActionResult GridData" + str1 + "(string sidx, string sord, int _page, int rows" + str2 + ")");
        sb.AppendLine("         {");
      }
      if (action == "groupedby" || action == "totalsgroupedby")
      {
        sb.AppendLine("             // using a groupField in the jqgrid passes that field");
        sb.AppendLine("             // along with the field to sort, remove the groupField");
        sb.AppendLine("             string groupBy = \"" + this._currentFkTable.DataTextField + " asc, \";");
        sb.AppendLine("             sidx = sidx.Replace(groupBy, \"\");");
        sb.AppendLine("");
      }
      if (action == "search")
      {
        StringBuilder stringBuilder1 = new StringBuilder();
        StringBuilder stringBuilder2 = new StringBuilder();
        StringBuilder stringBuilder3 = new StringBuilder();
        StringBuilder stringBuilder4 = new StringBuilder();
        foreach (Column column in this._table.Columns)
        {
          if (!column.IsBinaryOrSpatialDataType)
          {
            string str3 = "?";
            if (column.SystemType == "string")
              str3 = "";
            stringBuilder1.Append(column.NameCamelStyle + ", ");
            if (column.SystemType.ToLower() == "string")
            {
              stringBuilder2.AppendLine("             " + column.SystemType + " " + column.NameCamelStyle + " = String.Empty;");
              stringBuilder3.AppendLine("                     if (rule[\"field\"].Value.ToLower() == \"" + column.NameLowerCase + "\")");
              stringBuilder3.AppendLine("                         " + column.NameCamelStyle + " = rule[\"data\"].Value;");
              stringBuilder3.AppendLine("");
            }
            else
            {
              stringBuilder2.AppendLine("             " + column.SystemType + str3 + " " + column.NameCamelStyle + " = null;");
              stringBuilder3.AppendLine("                     if (rule[\"field\"].Value.ToLower() == \"" + column.NameLowerCase + "\")");
              if (column.SQLDataType == SQLType.time)
                stringBuilder3.AppendLine("                         " + column.NameCamelStyle + " = TimeSpan.Parse(rule[\"data\"].Value);");
              else if (column.SQLDataType == SQLType.datetimeoffset)
                stringBuilder3.AppendLine("                         " + column.NameCamelStyle + " = new DateTimeOffset(Convert.ToDateTime(rule[\"data\"].Value));");
              else
                stringBuilder3.AppendLine("                         " + column.NameCamelStyle + " = Convert.To" + column.SystemTypeNative + "(rule[\"data\"].Value);");
              stringBuilder3.AppendLine("");
              if (column.IsNumericField)
              {
                stringBuilder4.AppendLine("                 if (" + column.NameCamelStyle + " == -1)");
                stringBuilder4.AppendLine("                     " + column.NameCamelStyle + " = null;");
                stringBuilder4.AppendLine("");
              }
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
      sb.AppendLine("                 return Json(\"{ total = 0, page = 0, records = 0, rows = null }\");");
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
            if (this._currentFkTable.DataTextFieldSystemType.ToLower() != "string")
              str3 = ".ToString()";
            int num2;
            if (this._currentColumn.IsNullable)
            {
              if (this._currentColumn.Name != this._currentColumn.ForeignKeyColumnName)
                stringBuilder.AppendLine("                             " + this._table.VariableObjName + "." + this._currentColumn.Name + " is null ? \"\" : " + this._table.VariableObjName + "." + this._currentColumn.Name + "Navigation." + this._currentFkTable.DataTextField + str3);
              else if (this._table.Name == this._currentColumn.SingularForeignKeyTableName)
              {
                stringBuilder.AppendLine("                             " + this._table.VariableObjName + "." + this._currentColumn.Name + " is null ? \"\" : " + this._table.VariableObjName + "." + this._currentFkSingularTableName + num1 + "." + this._currentFkTable.DataTextField + str3);
                num2 = num1 + 1;
              }
              else
                stringBuilder.AppendLine("                             " + this._table.VariableObjName + "." + this._currentColumn.Name + " is null ? \"\" : " + this._table.VariableObjName + "." + this._currentFkSingularTableName + "." + this._currentFkTable.DataTextField + str3);
            }
            else if (this._currentColumn.Name != this._currentColumn.ForeignKeyColumnName)
              stringBuilder.AppendLine("                             " + this._table.VariableObjName + "." + this._currentColumn.Name + "Navigation." + this._currentFkTable.DataTextField + str3);
            else if (this._table.Name == this._currentColumn.SingularForeignKeyTableName)
            {
              stringBuilder.AppendLine("                             " + this._table.VariableObjName + "." + this._currentFkSingularTableName + num1 + "." + this._currentFkTable.DataTextField + str3);
              num2 = num1 + 1;
            }
            else
              stringBuilder.AppendLine("                             " + this._table.VariableObjName + "." + this._currentFkSingularTableName + "." + this._currentFkTable.DataTextField + str3);
          }
          else
          {
            if (this._currentFkTable.DataTextFieldSystemType.ToLower() != "string")
              str3 = ".ToString()";
            if (this._currentColumn.IsNullable)
              stringBuilder.AppendLine("                             " + this._table.VariableObjName + "." + this._currentColumn.Name + " is null ? \"\" : " + this._table.VariableObjName + "." + this._currentFkTableName + ".Value." + this._currentFkTable.DataTextField + str3);
            else
              stringBuilder.AppendLine("                             " + this._table.VariableObjName + "." + this._currentFkTableName + "." + this._currentFkTable.DataTextField + str3);
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
        sb.AppendLine("             return Json(jsonData);");
      sb.AppendLine("         }");
    }

    private void WriteGridDataForGroupedBy(StringBuilder sb, string action = "", bool isWriteGridData = true)
    {
      StringBuilder stringBuilder = new StringBuilder();
      int num1 = 0;
      int num2 = 1;
      foreach (Column column in this._table.Columns)
      {
        if (column.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK)
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
          this._currentFkTable = column.ForeignKeyTable;
          this._currentColumn = column;
          if (this._table.Name == column.SingularForeignKeyTableName)
          {
            this._currentFkSingularTableName = column.SingularForeignKeyTableName + num2;
            ++num2;
          }
          else
            this._currentFkSingularTableName = column.SingularForeignKeyTableName;
          if (action == "totalsgroupedby")
          {
            if (this._controllerBaseType == ControllerBaseType.ControllerBase)
              this.WriteListGet(sb, this._viewNames.ListTotalsGroupedBy + this._currentColumn.Name, MVCGridViewType.GroupedByWithTotals);
            if (isWriteGridData)
              this.WriteGridData(sb, "totalsgroupedby");
          }
          else
          {
            if (this._controllerBaseType == ControllerBaseType.ControllerBase)
              this.WriteListGet(sb, this._viewNames.ListGroupedBy + this._currentColumn.Name, MVCGridViewType.GroupedBy);
            if (isWriteGridData)
              this.WriteGridData(sb, "groupedby");
          }
        }
      }
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
      sb.AppendLine("             jsonData.page = page;");
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

    private void WriteViewModelAssignments(StringBuilder sb)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (Column column in this._table.Columns)
      {
        if (column.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK)
        {
          string str = column.ForeignKeyTableName + MyConstants.WordDropDownListData;
          if (!stringBuilder.ToString().Contains(str + ","))
          {
            if (this._isUseWebApi)
            {
              sb.AppendLine("             viewModel." + str + " = Get" + column.ForeignKeyTable.Name + "DropDownListData();");
            }
            else
            {
              string qualifiedTableName = Functions.GetFullyQualifiedTableName(column.ForeignKeyTable, this._selectedTables, Language.CSharp, column.ForeignKeyTableNameFullyQualifiedBusinessObject, this._apiName);
              sb.AppendLine("             viewModel." + str + " = " + qualifiedTableName + ".Select" + column.ForeignKeyTableName + "DropDownListData();");
            }
            stringBuilder.Append(str + ",");
          }
        }
      }
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

    private void WriteGetModelByPrimaryKeyWebApiCall(StringBuilder sb)
    {
      StringBuilder stringBuilder = new StringBuilder();
      string str1 = Functions.ConvertToCamel(this._table.FirstPrimaryKeyName);
      string str2 = string.Empty;
      if (this._table.PrimaryKeyCount > 1)
      {
        str1 = Functions.GetQueryStringDelimitedPrimaryKeys(this._table, Language.CSharp, null);
        str2 = "?";
      }
      foreach (Column column in this._table.Columns)
      {
        if (!column.IsComputed && column.IsNullable && (!column.IsPrimaryKeyUnique && !column.IsForeignKey) && (column.SQLDataType == SQLType.integer || column.SQLDataType == SQLType.bigint || (column.SQLDataType == SQLType.smallint || column.SQLDataType == SQLType.tinyint)))
        {
          stringBuilder.AppendLine("");
          stringBuilder.AppendLine("                     if(" + this._table.VariableObjName + "." + column.Name + ".HasValue)");
          stringBuilder.AppendLine("                        model." + column.Name + "Hidden = " + this._table.VariableObjName + "." + column.Name + ".ToString();");
        }
        else
          stringBuilder.AppendLine("                     model." + column.Name + " = " + this._table.VariableObjName + "." + column.Name + ";");
      }
      sb.AppendLine("");
      sb.AppendLine("         private " + this._modelName + " GetModelByPrimaryKey(" + Functions.GetCommaDelimitedPrimaryKeyParametersWithSystemType(this._table, Language.CSharp) + ")");
      sb.AppendLine("         {");
      sb.AppendLine("             " + this._modelName + " model = null;");
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
      sb.AppendLine("                     " + this._businessObjectName + " " + this._table.VariableObjName + " = JsonConvert.DeserializeObject<" + this._businessObjectName + ">(responseBody);");
      sb.AppendLine("");
      sb.AppendLine("                     // assign values to the model");
      sb.AppendLine("                     model = new " + this._modelName + "();");
      sb.Append(stringBuilder.ToString());
      sb.AppendLine("                 }");
      sb.AppendLine("             }");
      sb.AppendLine("");
      sb.AppendLine("             return model;");
      sb.AppendLine("         }");
    }

    private void WriteGetDropDownListWebApiHttpClientCallsForFks(StringBuilder sb)
    {
      if (this._table.ForeignKeyCount <= 0)
        return;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (Column column in this._table.Columns)
      {
        if (column.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK)
        {
          string str = column.ForeignKeyTableName + MyConstants.WordDropDownListData;
          if (!stringBuilder.ToString().Contains(str + ", "))
          {
            stringBuilder.Append(str + ", ");
            string qualifiedTableName = Functions.GetFullyQualifiedTableName(column.ForeignKeyTable, this._selectedTables, Language.CSharp, column.ForeignKeyTable.NameFullyQualifiedBusinessObject, this._apiName);
            sb.AppendLine("");
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
    }

    private void WriteListGetAuditLog(StringBuilder sb)
    {
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Get the audit log by primary key");
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         /// <param name=\"id\">CustomerId</param>");
      sb.AppendLine("         /// <returns>IActionResult</returns>");
      sb.AppendLine("         public IActionResult Audit(int id)");
      sb.AppendLine("         {");
      sb.AppendLine("             try");
      sb.AppendLine("             {");
      sb.AppendLine("                 var AuditTrail = _IAuditLog.GetAuditReport(\"" + this._table.Name + "\", id);");
      sb.AppendLine("                 return new JsonResult(AuditTrail);");
      sb.AppendLine("             }");
      sb.AppendLine("             catch (Exception ex)");
      sb.AppendLine("             {");
      sb.AppendLine("                 _Ilog.GetInstance().Error(\"Error Occured\", ex);");
      sb.AppendLine("                 return BadRequest(\"Error Message: \" + ex.Message + \" Stack Trace: \" + ex.StackTrace);");
      sb.AppendLine("             }");
      sb.AppendLine("         }");
    }
  }
}
