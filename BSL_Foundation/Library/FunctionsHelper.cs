using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
    internal class FunctionsHelper
    {
        private Table _table;
        private Tables _selectedTables;
        private const string _fileExtension = ".cs";
        private string _directory;
        private string _webAppRootDirectory;
        private string _webAppName;
        private string _apiName;
        #pragma warning disable CS0169 // The field 'FunctionsHelper._viewName' is never used
        private string _viewName;
        #pragma warning restore CS0169 // The field 'FunctionsHelper._viewName' is never used
        private StringBuilder _commaDelimitedColNames;
        #pragma warning disable CS0169 // The field 'FunctionsHelper._currentColumn' is never used
        private Column _currentColumn;
        #pragma warning restore CS0169 // The field 'FunctionsHelper._currentColumn' is never used
        private bool _isUseWebApi;
        private bool _isUseAuditLogging;
        private ControllerBaseType _controllerBaseType;
        private string _businessObjectName;
        private GeneratedSqlType _generatedSqlType;
        private bool _isEmailNotification;

        internal FunctionsHelper(Table table, Tables selectedTables, string webAppName, string apiName, string webAppRootDirectory, bool isUseWebApi, bool isUseAuditLogging, GeneratedSqlType generatedSqlType, bool isEmailNotification)
        {
            this._table = table;
            this._selectedTables = selectedTables;
            this._webAppRootDirectory = webAppRootDirectory + "Pages\\";
            this._webAppName = webAppName;
            this._apiName = apiName;
            this._isUseWebApi = isUseWebApi;
            this._isUseAuditLogging = isUseAuditLogging;
            this._commaDelimitedColNames = new StringBuilder();
            this._directory = webAppRootDirectory + "Helper\\";
            this._businessObjectName = Functions.GetFullyQualifiedTableName(this._table, this._selectedTables, Language.CSharp, this._table.NameFullyQualifiedBusinessObject, apiName);
            this._generatedSqlType = generatedSqlType;
            this._isEmailNotification = isEmailNotification;
            this._controllerBaseType = !isUseWebApi ? ControllerBaseType.ControllerBase : ControllerBaseType.ApiControllerBase;
            this.Generate();
        }

        private void Generate()
        {
            using (StreamWriter streamWriter = new StreamWriter(this._directory + this._table.Name + "Functions.cs"))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("using " + this._apiName + ".Domain;");
                sb.AppendLine("using " + this._apiName + ".BusinessObject;");
                if (this._isUseAuditLogging && !this._isUseWebApi)
                    sb.AppendLine("using Application_Components.AuditLog;");
                if (this._isEmailNotification && !this._isUseWebApi)
                    sb.AppendLine("using Application_Components.EmailNotification;");
                if (this._isUseWebApi)
                {
                    sb.AppendLine("using Newtonsoft.Json;");
                    sb.AppendLine("using System;");
                    sb.AppendLine("using System.Net.Http;");
                    sb.AppendLine("using System.Text;");
                }
                else
                    sb.AppendLine("using System;");
                sb.AppendLine("");
                sb.AppendLine("namespace " + this._webAppName);
                sb.AppendLine("{");
                sb.AppendLine("     public class " + this._table.Name + "Functions");
                sb.AppendLine("     {");
                sb.AppendLine("         private " + this._table.Name + "Functions()");
                sb.AppendLine("         {");
                sb.AppendLine("         }");
                sb.AppendLine("");
                this.WriteAddEdit(sb);
                sb.AppendLine("     }");
                sb.AppendLine("}");
                streamWriter.Write(sb.ToString());
            }
        }

        private void WriteAddEdit(StringBuilder sb)
        {
            sb.AppendLine("");
            sb.AppendLine("         /// <summary>");
            sb.AppendLine("         /// Used when adding or updating a record.");
            sb.AppendLine("         /// </summary>");
            if (this._isUseAuditLogging && !this._isUseWebApi)
                sb.AppendLine("         internal static void AddOrEdit(" + this._table.Name + " model, CrudOperation operation, IAuditLog _IAuditLog, bool isForListInline = false)");
            else
                sb.AppendLine("         internal static void AddOrEdit(" + this._table.Name + " model, CrudOperation operation, bool isForListInline = false)");
            sb.AppendLine("         {");
            if (this._isUseWebApi && this._controllerBaseType == ControllerBaseType.ApiControllerBase)
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
                    sb.AppendLine("                      _IAuditLog.CreateAuditTrail(_IAuditLog.GetAuditActionTypeValue(\"Create\"), Convert.ToInt32(id), \"" + this._table.Name + "\", null, " + this._table.VariableObjName + ", 0, 100);");
                sb.AppendLine("                 }");
                sb.AppendLine("                 else");
                sb.AppendLine("                    " + this._table.VariableObjName + ".Update();");
                if (this._isUseAuditLogging && this._isUseWebApi)
                    sb.AppendLine("                      _IAuditLog.CreateAuditTrail(_IAuditLog.GetAuditActionTypeValue(\"Update\"), Convert.ToInt32(" + this._table.VariableObjName + "." + Functions.GetCommaDelimitedTableNameDotPrimaryKeys(this._table).Split('.')[1] + "), \"" + this._table.Name + "\", " + this._table.VariableObjName + "Old, " + this._table.VariableObjName + ", 0, 100);");
                if (this._controllerBaseType == ControllerBaseType.ApiControllerBase)
                {
                    sb.AppendLine("");
                    sb.AppendLine("                 return Ok();");
                }
                sb.AppendLine("             }");
                sb.AppendLine("             catch (Exception ex)");
                sb.AppendLine("             {");
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
                sb.AppendLine("                " + this._table.VariableObjName + " = new " + this._businessObjectName + "();");
                sb.AppendLine("             else");
                sb.AppendLine("             {");
                if (this._table.PrimaryKeyCount > 1)
                    sb.AppendLine("                 " + this._table.VariableObjName + " = " + this._businessObjectName + ".SelectByPrimaryKey(" + Functions.GetCommaDelimitedModelDotPrimaryKeys(this._table, this._generatedSqlType) + ");");
                else if (this._generatedSqlType == GeneratedSqlType.EFCore && this._table.FirstPrimaryKeySQLDataType == SQLType.uniqueidentifier)
                    sb.AppendLine("                 " + this._table.VariableObjName + " = " + this._businessObjectName + ".SelectByPrimaryKey(model." + this._table.FirstPrimaryKeyName + ".ToString());");
                else
                    sb.AppendLine("                 " + this._table.VariableObjName + " = " + this._businessObjectName + ".SelectByPrimaryKey(model." + this._table.FirstPrimaryKeyName + ");");
                sb.AppendLine("                 " + this._table.VariableObjName + "Old = " + this._table.VariableObjName + ".ShallowCopy();");
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
                sb.AppendLine("                id = " + this._table.VariableObjName + ".Insert();");
                if (this._isUseAuditLogging && !this._isUseWebApi)
                    sb.AppendLine("                _IAuditLog.CreateAuditTrail(_IAuditLog.GetAuditActionTypeValue(\"Create\"), Convert.ToInt32(id), \"" + this._table.Name + "\", null, " + this._table.VariableObjName + ", 0, 100);");
                sb.AppendLine("             }");
                sb.AppendLine("             else");
                sb.AppendLine("             {");
                sb.AppendLine("                " + this._table.VariableObjName + ".Update();");
                if (this._isUseAuditLogging && !this._isUseWebApi)
                    sb.AppendLine("                _IAuditLog.CreateAuditTrail(_IAuditLog.GetAuditActionTypeValue(\"Update\"), Convert.ToInt32(" + this._table.VariableObjName + "." + Functions.GetCommaDelimitedTableNameDotPrimaryKeys(this._table).Split('.')[1] + "), \"" + this._table.Name + "\", " + this._table.VariableObjName + "Old, " + this._table.VariableObjName + ", 0, 100);");
                sb.AppendLine("             }");
            }
            sb.AppendLine("         }");
        }
    }
}
