using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPIT_K_Foundation.Library
{
    internal class AssignWorkflowStepsModel
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
        // private DataTable _referencedTables;
        private string _modelName;
        private bool _isSqlVersion2012OrHigher;
        private GeneratedSqlType _generatedSqlType;
        private MVCGridViewType _viewType;
        private bool _isUseLogging;
        private bool _isUseCaching;
        private bool _isUseAuditLogging;
        private bool _isEmailNotification;
        private string _connectionString;

        internal AssignWorkflowStepsModel(MVCGridViewType listViewType, Table table, Tables selectedTables,
            string webAppName, string apiName, string webApiName, string webAppRootDirectory, string apiNameDirectory, 
            string webApiNameDirectory, bool isUseStoredProcedure, bool isSqlVersion2012OrHigher, IsCheckedView isCheckedView,
            DatabaseObjectToGenerateFrom generateFrom, ApplicationVersion appVersion, 
            GeneratedSqlType generatedSqlType, bool isUseLogging, bool isUseCaching, bool isUseAuditLogging, bool isEmailNotification, string connectionString,
            ControllerBaseType controllerBaseType = ControllerBaseType.ControllerBase, bool isUseWebApi = false)
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
            this._generateFrom = generateFrom;
            this._appVersion = appVersion;
            //this._referencedTables = referencedTables;
            this._colModelNames = new StringBuilder();
            this._controllerBaseType = controllerBaseType;
            this._isUseWebApi = isUseWebApi;
            this._generatedSqlType = generatedSqlType;
            this._viewType = listViewType;
            this._modelName = Functions.GetFullyQualifiedModelName(table, selectedTables, apiName);
            this._businessObjectName = Functions.GetFullyQualifiedTableName(this._table, this._selectedTables, Language.CSharp, this._table.NameFullyQualifiedBusinessObject, apiName);
            this._isSqlVersion2012OrHigher = isSqlVersion2012OrHigher;
            this._directory = webAppRootDirectory + MyConstants.DirectoryControllerBase;
            this._isUseLogging = isUseLogging;
            this._isUseCaching = isUseCaching;
            this._isUseAuditLogging = isUseAuditLogging;
            this._isEmailNotification = isEmailNotification;
            this._connectionString = connectionString;
            this.Generate(null, null);
            
        }
        private void Generate(Table currentMasterTable = null, Table foreignKeyDetailTable = null)
        {
            
            using (StreamWriter streamWriter = new StreamWriter(this._webAppRootDirectory + this._table.Name + "\\" + this._table.Name + "_AssignUsers.cshtml.cs"))
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
                sb.AppendLine("using System.Data.SqlClient;");
                sb.AppendLine("using System.Data; ");

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
                if (this._isUseWebApi)
                    sb.AppendLine("using System.Net.Http;");

                sb.AppendLine("");
                sb.AppendLine("namespace " + this._webAppName + ".Pages");
                sb.AppendLine("{");
                sb.AppendLine("     public class Workflows");
                sb.AppendLine("     { ");
                sb.AppendLine("      public int WorkflowId { get; set; }");
                sb.AppendLine("      public string WorkflowName { get; set; }");
                sb.AppendLine("     }");
                sb.AppendLine("     public class Users");
                sb.AppendLine("     {");
                sb.AppendLine("     public int UserId { get; set; }");
                sb.AppendLine("     public string UserName { get; set; }");
                sb.AppendLine("     }");
                sb.AppendLine("     public class WorkflowSteps");
                sb.AppendLine("     {");
                sb.AppendLine("     public int StepId { get; set; }");
                sb.AppendLine("     public string StepsTitle { get; set; }");
                sb.AppendLine("     public int WorkflowId { get; set; }");
                sb.AppendLine("     }");
                sb.AppendLine("     [AutoValidateAntiforgeryToken]"); 
                sb.AppendLine("     public class " + this._table.Name + "_AssignUsersModel : PageModel");
                sb.AppendLine("     {");

                this.WriteMethods(sb, null, null);

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
                    sb.AppendLine("         IEmail _IEmail;");
               
                sb.AppendLine("");
                Functions.WriteBusinessObjectBindProperty(sb, this._table, this._apiName);
                sb.AppendLine("         [BindProperty]");
                sb.AppendLine("         public string ReturnUrl { get; set; }");
                sb.AppendLine("");
                sb.AppendLine("         public List<Workflows> WorkflowMasterDetails { get; set; }");
                sb.AppendLine("         public List<Users> UserMasterDetails { get; set; }");
                sb.AppendLine("         public List<WorkflowSteps> WorkflowStepsDetails { get; set; }");
                sb.AppendLine("         public int selectedWorkflowId { get; set; }");

                sb.AppendLine("");
                if (this._isUseLogging || this._isUseCaching || this._isUseAuditLogging || this._isEmailNotification)
                    this.WriteConstructor(sb);
                this.WriteOnGet(sb);

                if (this._viewType == MVCGridViewType.AssignWorkflowSteps)
                    this.WriteOnPostUpdate(sb);

                if (this._isUseWebApi)
                {
                    if (this._generateFrom == DatabaseObjectToGenerateFrom.Tables)
                    {
                        if (this._viewType == MVCGridViewType.AssignWorkflowSteps || this._viewType == MVCGridViewType.RecordDetails)
                            this.WriteGetByPrimaryKeyWebApiCall(sb);
                        if (this._viewType == MVCGridViewType.Add || this._viewType == MVCGridViewType.AssignWorkflowSteps || (this._viewType == MVCGridViewType.CRUD || this._viewType == MVCGridViewType.Inline) || this._viewType == MVCGridViewType.Search)
                            this.WriteGetDropDownListWebApiHttpClientCallsForFks(sb);
                    }
                }
            }
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
            sb.AppendLine("         /// Default Constructor: /" + this._table.Name + "_AssignUsersModel");
            sb.AppendLine("         /// </summary>");
            sb.AppendLine("         public " + this._table.Name +"_AssignUsersModel" + " (" + paramlist + ")");
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
                sb.AppendLine("            if (_IEmail == null)");
                sb.AppendLine("                _IEmail = IEmail;");
            }
            sb.AppendLine("                WorkflowMasterDetails = new List<Workflows>();");
            sb.AppendLine("                UserMasterDetails = new List<Users>();");
            sb.AppendLine("                WorkflowStepsDetails = new List<WorkflowSteps>();");
            sb.AppendLine("         }");
            sb.AppendLine("");
        }
        private void WriteOnGet(StringBuilder sb)
        {
            sb.AppendLine("         /// <summary>");
            sb.AppendLine("         /// Initial handler the razor page encounters.");
            sb.AppendLine("         /// </summary>");
            string str = "PageResult";
            sb.AppendLine("         public void OnGet(" + this._table.FirstPrimaryKeySystemType + " id, string returnUrl)");
            sb.AppendLine("         {");
            sb.AppendLine("             LoadPage(id, returnUrl);");
            sb.AppendLine("         }");
            sb.AppendLine("");
            sb.AppendLine("         public " + str + " LoadPage(" + this._table.FirstPrimaryKeySystemType  +" "+ Functions.GetCommaDelimitedPrimaryKeys(this._table, Language.CSharp, true, false, false) + ", string returnUrl)");
            sb.AppendLine("         {");
            if (this._isUseWebApi)
                sb.AppendLine("             " + this._apiName + "." + MyConstants.WordBusinessObject + "." + this._table.Name + " " + this._table.VariableObjName + " = Get" + this._table.Name + "ByPrimaryKey(" + Functions.GetCommaDelimitedPrimaryKeys(this._table, Language.CSharp, true, false, false) + ");");
            else
                sb.AppendLine("             " + this._apiName + "." + MyConstants.WordBusinessObject + "." + this._table.Name + " " + this._table.VariableObjName + " = " + this._table.Name + ".SelectByPrimaryKey(" + Functions.GetCommaDelimitedPrimaryKeys(this._table, Language.CSharp, true, false, false) + ");");
            string paramlist = string.Empty;

            if (this._isUseLogging)
                paramlist = paramlist + (paramlist != "" ? "," : "") + "_Ilog";

            if (this._isUseCaching)
                paramlist = paramlist + (paramlist != "" ? "," : "") + "_IRediscache";

            if (this._isUseAuditLogging)
                paramlist = paramlist + (paramlist != "" ? "," : "") + "_IAuditLog";
            if (this._isEmailNotification)
                paramlist = paramlist + (paramlist != "" ? "," : "") + "_IEmail";
           
            sb.AppendLine("             WorkflowStepsMaster_AssignUsersModel model = new WorkflowStepsMaster_AssignUsersModel("+paramlist+");");
            sb.AppendLine("             model.WorkflowMasterDetails = GetWorkFlowIds();");
            sb.AppendLine("             model.UserMasterDetails = GetUserDetails();");
            sb.AppendLine("             model.WorkflowStepsDetails = GetWorkflowSteps();");
            sb.AppendLine("             model.ReturnUrl = returnUrl;");
            sb.AppendLine("             ReturnUrl = returnUrl;");
            sb.AppendLine("");
            sb.AppendLine("             return Page();");
            sb.AppendLine("         }");
            sb.AppendLine("");
            sb.AppendLine("         public List<Workflows> GetWorkFlowIds()");
            sb.AppendLine("         {");
            sb.AppendLine("              DataTable dt=new DataTable();");
            sb.AppendLine("              List<string> Workflowids = new List<string>();");
            sb.AppendLine("              SqlConnection sqlConnection = new SqlConnection(" + '"' +this._connectionString+ '"'+");");
            sb.AppendLine("              sqlConnection.Open();");
            sb.AppendLine("              SqlCommand command = new SqlCommand(" + '"' + "Select DISTINCT WorkflowId, WorkflowName from [dbo].[WorkflowMaster]" + '"' + ", sqlConnection);");
            sb.AppendLine("              SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);");
            sb.AppendLine("              sqlDataAdapter.Fill(dt);");
            sb.AppendLine("              if(dt.Rows.Count>0)");
            sb.AppendLine("              {");
            sb.AppendLine("                 for(int i=0;i<dt.Rows.Count;i++)");
            sb.AppendLine("                 { ");
            sb.AppendLine("                  Workflows wm=new Workflows();");
            sb.AppendLine("                  wm.WorkflowId=Convert.ToInt32(dt.Rows[i][" + '"' + "WorkflowId" + '"' + "]);");
            sb.AppendLine("                  wm.WorkflowName=dt.Rows[i][" + '"' + "WorkflowName" + '"' + "].ToString();");
            sb.AppendLine("                 WorkflowMasterDetails.Add(wm);");
            sb.AppendLine("                 } ");
            sb.AppendLine("              }");
            sb.AppendLine("              sqlConnection.Close();");
            sb.AppendLine("              return WorkflowMasterDetails;");
            sb.AppendLine("         }");
            sb.AppendLine("         public List<Users> GetUserDetails()");
            sb.AppendLine("         {");
            sb.AppendLine("             DataTable dt = new DataTable();");
            sb.AppendLine("             List<string> Workflowids = new List<string>();");
            sb.AppendLine("             SqlConnection sqlConnection = new SqlConnection(" + '"' + this._connectionString + '"' + ");");
            sb.AppendLine("             sqlConnection.Open();");
            sb.AppendLine("             SqlCommand command = new SqlCommand(" + '"' + "Select DISTINCT UserId, UserName from[dbo].[UserMaster]" + '"' + ", sqlConnection);");
            sb.AppendLine("             SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);");
            sb.AppendLine("             sqlDataAdapter.Fill(dt);");
            sb.AppendLine("             if (dt.Rows.Count > 0)");
            sb.AppendLine("             {");
            sb.AppendLine("                 for (int i = 0; i < dt.Rows.Count; i++)");
            sb.AppendLine("                 {");
            sb.AppendLine("                     Users um = new Users();");
            sb.AppendLine("                     um.UserId = Convert.ToInt32(dt.Rows[i][" + '"' + "UserId" + '"' + "]); ");
            sb.AppendLine("                     um.UserName = dt.Rows[i][" + '"' + "UserName" + '"' + "].ToString();");
            sb.AppendLine("                     UserMasterDetails.Add(um);");
            sb.AppendLine("                 }");
            sb.AppendLine("             }");
            sb.AppendLine("             sqlConnection.Close();");
            sb.AppendLine("             return UserMasterDetails;");
            sb.AppendLine("       ");
            sb.AppendLine("         }");
            sb.AppendLine("             public List<WorkflowSteps> GetWorkflowSteps()");
            sb.AppendLine("             {");
            sb.AppendLine("                 DataTable dt = new DataTable();");
            sb.AppendLine("                 List<string> Workflowids = new List<string>();");
            sb.AppendLine("                 SqlConnection sqlConnection = new SqlConnection(" + '"' + this._connectionString + '"' + ");");
            sb.AppendLine("                 sqlConnection.Open();");
            sb.AppendLine("                 SqlCommand command = new SqlCommand(" + '"' + "Select StepId,StepTitle,WorkflowId from[dbo].[WorkflowStepsMaster]" + '"' + ", sqlConnection);");
            sb.AppendLine("                 SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);");
            sb.AppendLine("                 sqlDataAdapter.Fill(dt);");
            sb.AppendLine("                 if (dt.Rows.Count > 0)");
            sb.AppendLine("                 {");
            sb.AppendLine("                     for (int i = 0; i < dt.Rows.Count; i++)");
            sb.AppendLine("                     {");
            sb.AppendLine("                         WorkflowSteps ws = new WorkflowSteps();");
            sb.AppendLine("                         ws.StepId = Convert.ToInt32(dt.Rows[i][" + '"' + "StepId" + '"' + "]); ");
            sb.AppendLine("                         ws.StepsTitle = dt.Rows[i][" + '"' + "StepTitle" + '"' + "].ToString();");
            sb.AppendLine("                         ws.WorkflowId = Convert.ToInt32(dt.Rows[i][" + '"' + "WorkflowId" + '"' + "]); ");
            sb.AppendLine("                         WorkflowStepsDetails.Add(ws);");
            sb.AppendLine("                     }");
            sb.AppendLine("                 }");
            sb.AppendLine("                 sqlConnection.Close();");
            sb.AppendLine("                 return WorkflowStepsDetails;");
            sb.AppendLine("             }");
        }

    }





}
