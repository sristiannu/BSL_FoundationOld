
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
    internal class ApiController
    {
        private Table _table;
        private const string _fileExtension = ".cs";
        private string _webApiRootDirectory;
        private string _webApiName;
        private bool _isUseLogging;
        private bool _isUseCaching;
        private bool _isUseAuditLogging;
        private bool _isEmailNotification;

        internal ApiController(Table table, string webApiName, string webApiRootDirectory, bool isUseLogging, bool isUseCaching, bool isUseAuditLogging, bool isEmailNotification)
        {
            this._table = table;
            this._webApiRootDirectory = webApiRootDirectory + "\\Controllers\\";
            this._webApiName = webApiName;
            this._isUseLogging = isUseLogging;
            this._isUseCaching = isUseCaching;
            this._isUseAuditLogging = isUseAuditLogging;
            this._isEmailNotification = isEmailNotification;
            this.Generate();
        }

        private void Generate()
        {
            string path = this._webApiRootDirectory + this._table.Name + MyConstants.WordApi + MyConstants.WordController + ".cs";
            if (File.Exists(path))
                return;
            using (StreamWriter streamWriter = new StreamWriter(path))
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("using System;");
                stringBuilder.AppendLine("using " + this._webApiName + "." + MyConstants.WordApi + MyConstants.WordControllersDotBase + ";");

                if (this._isUseLogging)
                    stringBuilder.AppendLine("using Application_Components.Logging;");
                if (this._isUseCaching)
                    stringBuilder.AppendLine("using Application_Components.Caching;");
                if (this._isUseAuditLogging)
                    stringBuilder.AppendLine("using Application_Components.AuditLog;");
                if (this._isEmailNotification)
                    stringBuilder.AppendLine("using Application_Components.EmailNotification;");
                stringBuilder.AppendLine("");
                stringBuilder.AppendLine("namespace " + this._webApiName + "." + MyConstants.WordControllers);
                stringBuilder.AppendLine("{");
                stringBuilder.AppendLine("     /// <summary>");
                stringBuilder.AppendLine("     /// This file will not be overwritten.  You can put");
                stringBuilder.AppendLine("     /// additional " + this._table.Name + " " + MyConstants.WordApi + "Controller code in this class.");
                stringBuilder.AppendLine("     /// </summary>");
                stringBuilder.AppendLine("     public class " + this._table.Name + MyConstants.WordApi + MyConstants.WordController + " : " + this._table.Name + MyConstants.WordApi + MyConstants.WordControllerBase);
                stringBuilder.AppendLine("     {");
                stringBuilder.AppendLine("");

                if (this._isUseLogging || this._isUseCaching)
                {
                    if (this._isUseLogging)
                        stringBuilder.AppendLine("ILog _Ilog;");
                    if (this._isUseCaching)
                        stringBuilder.AppendLine("IRedisCacheManager _IRediscache;");
                    if (this._isUseAuditLogging)
                        stringBuilder.AppendLine("IAuditLog _IAuditLog;");
                    if (this._isEmailNotification)
                        stringBuilder.AppendLine("IEmail _IEmail;");
                    this.WriteConstructor(stringBuilder);
                }

                stringBuilder.AppendLine("     }");
                stringBuilder.AppendLine("}");
                streamWriter.Write(stringBuilder.ToString());
            }
        }

        private void WriteConstructor(StringBuilder sb)
        {
            string paramlist = string.Empty;
            string baseparamlist = string.Empty;

            if (this._isUseLogging)
            {
                paramlist = paramlist + (paramlist != "" ? "," : "") + "ILog Ilog";
                baseparamlist = baseparamlist + (baseparamlist != "" ? "," : "") + "Ilog";
            }

            if (this._isUseCaching)
            {
                paramlist = paramlist + (paramlist != "" ? "," : "") + "IRedisCacheManager IRediscache";
                baseparamlist = baseparamlist + (baseparamlist != "" ? "," : "") + "IRediscache";
            }

            if (this._isUseAuditLogging)
            {
                paramlist = paramlist + (paramlist != "" ? "," : "") + "IAuditLog IAuditLog";
                baseparamlist = baseparamlist + (baseparamlist != "" ? "," : "") + "IAuditLog";
            }

            if (this._isEmailNotification)
            {
                paramlist = paramlist + (paramlist != "" ? "," : "") + "IEmail IEmail";
                baseparamlist = baseparamlist + (baseparamlist != "" ? "," : "") + "IEmail";
            }

            sb.AppendLine("");
            sb.AppendLine("         /// <summary>");
            sb.AppendLine("         /// Default Constructor: /" + this._table.Name + MyConstants.WordApi + MyConstants.WordController);
            sb.AppendLine("         /// </summary>");
            sb.AppendLine("         public " + this._table.Name + MyConstants.WordApi + MyConstants.WordController + " (" + paramlist + ") : base(" + baseparamlist + ")");
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
    }
}
