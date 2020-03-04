using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPIT_K_Foundation.Library
{
    internal class AssignWorklowStepsPage
    {
        private Table _table;
        private Tables _selectedTables;
        private const string _fileExtension = ".cs";
        private string _directory;
        private string _webAppRootDirectory;
        private string _webAppName;
        private string _apiName;
        private string _viewName;
        private AppendAddEditRecordContentType _appendAddEditRecordContentType;
        private ApplicationVersion _appVersion;

        internal AssignWorklowStepsPage(MVCGridViewType listViewType, Table table, Tables selectedTables, string webAppRootDirectory, string webAppName, string apiName, ViewNames viewNames, string viewName, string jqueryUITheme, GeneratedSqlType generatedSqlType, AppendAddEditRecordContentType appendAddEditRecordContentType, ApplicationVersion appVersion = ApplicationVersion.ProfessionalPlus, bool isUseWebApi = false, bool isUseAuditLogging = false, string webApiBaseAddress = "")
        {

            this._table = table;
            this._selectedTables = selectedTables;
            this._webAppRootDirectory = webAppRootDirectory + "Pages\\";
            this._webAppName = webAppName;
            this._apiName = apiName;
            this._viewName = viewName.Trim();
            this._directory = webAppRootDirectory + MyConstants.DirectoryRazorPage + this._table.Name + "\\";
            this._appendAddEditRecordContentType = appendAddEditRecordContentType;
            this._appVersion = appVersion;
            this.Generate();
        }
        private void Generate()
        {
            string path =path = this._directory + this._table.Name + "_AssignUsers.cshtml";
            using (StreamWriter streamWriter = new StreamWriter(path))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("@page");
                sb.AppendLine("@model " + this._webAppName + ".Pages." + this._table.Name +"_AssignUsersModel");
                sb.AppendLine("");
                sb.AppendLine("@section AdditionalCss {");
                sb.AppendLine("    <link rel=\"stylesheet\" href=\"~/css/ui.jqgrid.min.css\" />");
                sb.AppendLine("    td,th{");
                sb.AppendLine("    padding: 8px;");
                sb.AppendLine("    text-align: left;");
                sb.AppendLine("     }");
                sb.AppendLine("}");
                sb.AppendLine(" <script src=\"~/js/jquery-1.12.2.min.js\"></script>");
                sb.AppendLine("<h2> Assign Users To" + this._viewName + "</h2>");
                sb.AppendLine("<div>");
                sb.AppendLine("<form method = \"post\"> ");
                sb.AppendLine(" <input type = \"hidden\" asp-for= \"ReturnUrl\" />");
                sb.AppendLine("     <div>");
                sb.AppendLine("         <fieldset>");
                sb.AppendLine("             <legend></legend>");
                sb.AppendLine("             <label asp-for= \"Workflow\"></label>:");
                sb.AppendLine("             <select name = \"selWorkflow\" id = \"selWorkflowId\" asp-for= \"WorkflowStepsMaster.WorKflowId\" asp-items = \"@(new SelectList(Model.WorkflowMasterDetails, \"WorkflowId\", \"WorkflowName\"))\" onchange = \"getWorkflowId();\" ><option value = \"\" > Select One </option></select>");
                sb.AppendLine("             <table>");
                sb.AppendLine("                         <tr>");
                sb.AppendLine("                             <th> <label> StepTitle </label></th>");
                sb.AppendLine("                             <th> <label> Available Users </label></th>");
                sb.AppendLine("                             <th> </th>");
                sb.AppendLine("                             <th> </th>");
                sb.AppendLine("                             <th> <label> Assigned Users </label></th>");
                sb.AppendLine("                         </tr>");
                sb.AppendLine("                         @if(Model.WorkflowStepsDetails.Count > 0)");
                sb.AppendLine("                         {");
                sb.AppendLine("                        var i = 0;");
                sb.AppendLine("                        @foreach(var item in Model.WorkflowStepsDetails)");
                sb.AppendLine("                             {");
                sb.AppendLine("                            i++;");
                sb.AppendLine("                        <tr>");
                sb.AppendLine("                        <td> @item.StepsTitle </td>");
                sb.AppendLine("                        <td><select id = @string.Format(\"avlUser{0}\", i) asp-items = \"@(new SelectList(Model.UserMasterDetails, \"UserId\", \"UserName\"))\" multiple = \"multiple\" ></select></td>");
                sb.AppendLine("                        <td>");
                sb.AppendLine("                            <button class=\"btnAvlUser\" id =@string.Format(\"btnRight{0}\", i) style=\"background-color: #ccc;color: #06010e;padding: 2px 8px;\" onclick =\"AvailableUser(@string.Format(\"avlUser{0}\", i),@string.Format(\"assgUser{0}\", i));\" > &gt</button>");
                sb.AppendLine("                            <button class=\"btnAssgUser\" id =@string.Format(\"btnLeft{0}\", i) style=\"background-color: #ccc;color: #06010e;padding: 2px 8px;\" onclick =\"AvailableUser(@string.Format(\"avlUser{0}\", i),@string.Format(\"assgUser{0}\", i));\" > &lt</button>");
                sb.AppendLine("                        </td>");
                sb.AppendLine("                        <td>");
                sb.AppendLine("                            <select id = @string.Format(\"assgUser{0}\", i) multiple=\"multiple\" ><option value = \"0\"> No Assigned user</option></select>");
                sb.AppendLine("                        </td>");
                sb.AppendLine("                    </tr>");
                sb.AppendLine("                }");
                sb.AppendLine("            }");
                sb.AppendLine("        </table>");
                sb.AppendLine("        <input type = \"submit\" asp-page-handler=\"Update\" value =\"Update\" class=\"button-150\" />");
                sb.AppendLine("        <input type = \"button\" value=\"Cancel\" onclick =\"window.location='@Model.ReturnUrl'; return false;\" class=\"button-100\" />");
                sb.AppendLine("        <input type = \"hidden\" id =\"hdnUserIds\" />");
                sb.AppendLine("        <input type = \"hidden\" id =\"hdnWorkflowId\" value=\"0\" name=\"hdnWorkflowId\" asp-for=\"WorkflowStepsMaster.WorKflowId\" />"); 
                sb.AppendLine("    </fieldset>");
                sb.AppendLine("</div>");
                sb.AppendLine("</form>");
                sb.AppendLine("</div>");
                streamWriter.Write(sb.ToString());
            }
        }
    }
}
