using System.IO;
using System.Text;

namespace KPIT_K_Foundation.Library
{
    internal class NlogConfig
    {
        private string _fullFileNamePath;
        private bool _isUseFileLogging;
        private bool _isUseDatabaseLogging;
        private bool _isUseEventLogging;
        private string _fileLogPath;
        private string _connectionString;
        private string _appName;

        internal NlogConfig()
        {
        }

        internal NlogConfig(string fullFileNamePath, bool isUseFileLogging, bool isUseDatabaseLogging, bool isUseEventLogging, string fileLogPath = "", string connectionString = "", string appName = "")
        {
            this._fullFileNamePath = fullFileNamePath;
            this._isUseFileLogging = isUseFileLogging;
            this._isUseDatabaseLogging = isUseDatabaseLogging;
            this._isUseEventLogging = isUseEventLogging;
            this._fileLogPath = fileLogPath;
            this._connectionString = connectionString;
            this._appName = appName;
            this.Generate();
            this.BuildLogTable();
        }

        private void Generate()
        {
            using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                stringBuilder.AppendLine("<nlog xmlns=\"http://www.nlog-project.org/schemas/NLog.xsd\"");
                stringBuilder.AppendLine("      xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"");
                stringBuilder.AppendLine("      autoReload=\"true\"");
                stringBuilder.AppendLine("      internalLogLevel=\"Warn\"");
                stringBuilder.AppendLine("      internalLogFile=\"c:\\temp\\internal-nlog.txt\">");
                stringBuilder.AppendLine("");
                stringBuilder.AppendLine("  <extensions>");
                stringBuilder.AppendLine("    <add assembly=\"NLog.WindowsEventLog\" />");
                stringBuilder.AppendLine("  </extensions>");
                stringBuilder.AppendLine("");
                stringBuilder.AppendLine("  <!-- define various log targets -->");
                stringBuilder.AppendLine("");
                stringBuilder.AppendLine("  <targets>");
                if (_isUseFileLogging)
                {
                    stringBuilder.AppendLine("    <!-- write logs to file -->");
                    stringBuilder.AppendLine("    <target xsi:type=\"File\" name=\"file\" fileName=\"" + _fileLogPath + "\"");
                    stringBuilder.AppendLine("        layout=\"${longdate}|${event-properties:item=EventId.Id}|${logger}|${uppercase:${level}}|${message} ${exception}\" />");
                }
                if (_isUseEventLogging)
                {
                    stringBuilder.AppendLine("");
                    stringBuilder.AppendLine("    <!-- write logs to event -->");
                    stringBuilder.AppendLine("    <target xsi:type=\"EventLog\" name=\"eventlog\" source=\"" + this._appName + "\" log=\"Application\"");
                    stringBuilder.AppendLine("        layout=\"${message}${newline}${exception:format=ToString}\" />");
                }
                if (_isUseDatabaseLogging)
                {
                    stringBuilder.AppendLine("");
                    stringBuilder.AppendLine("    <!-- write logs to database -->");
                    stringBuilder.AppendLine("    <target name=\"database\"");
                    stringBuilder.AppendLine("            xsi:type=\"Database\"");
                    stringBuilder.AppendLine("            connectionString=\"" + _connectionString + "\">");
                    stringBuilder.AppendLine("      <commandText>");
                    stringBuilder.AppendLine("        insert into log (");
                    stringBuilder.AppendLine("        Application, Logged, Level, Message,");
                    stringBuilder.AppendLine("        Logger, CallSite, Exception");
                    stringBuilder.AppendLine("        ) values (");
                    stringBuilder.AppendLine("        @Application, @Logged, @Level, @Message,");
                    stringBuilder.AppendLine("        @Logger, @Callsite, @Exception");
                    stringBuilder.AppendLine("        );");
                    stringBuilder.AppendLine("      </commandText>");
                    stringBuilder.AppendLine("");
                    stringBuilder.AppendLine("      <parameter name=\"@application\" layout=\"AspNetCoreNlog\" />");
                    stringBuilder.AppendLine("      <parameter name=\"@logged\" layout=\"${date}\" />");
                    stringBuilder.AppendLine("      <parameter name=\"@level\" layout=\"${level}\" />");
                    stringBuilder.AppendLine("      <parameter name=\"@message\" layout=\"${message}\" />");
                    stringBuilder.AppendLine("      <parameter name=\"@logger\" layout=\"${logger}\" />");
                    stringBuilder.AppendLine("      <parameter name=\"@callSite\" layout=\"${callsite:filename=true}\" />");
                    stringBuilder.AppendLine("      <parameter name=\"@exception\" layout=\"${exception:tostring}\" />");
                    stringBuilder.AppendLine("    </target>");
                }
                stringBuilder.AppendLine("  </targets>");
                stringBuilder.AppendLine("");
                stringBuilder.AppendLine("  <rules>");
                if (_isUseFileLogging)
                {
                    stringBuilder.AppendLine("    <logger name=\"filelogger\" minlevel=\"Trace\" writeTo=\"file\" />");
                }
                if (_isUseDatabaseLogging)
                {
                    stringBuilder.AppendLine("    <logger name=\"databaselogger\" minlevel=\"Trace\" writeTo=\"database\" />");
                }
                if (_isUseEventLogging)
                {
                    stringBuilder.AppendLine("    <logger name=\"eventlogger\" minlevel=\"Trace\" writeTo=\"eventlog\" />");
                }
                stringBuilder.AppendLine("  </rules>");
                stringBuilder.AppendLine("</nlog>");

                streamWriter.Write(stringBuilder.ToString());
            }
        }

        private void BuildLogTable()
        {
            Dbase _dbase = new Dbase(_connectionString);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("CREATE TABLE [dbo].[Log](");
            stringBuilder.AppendLine("[Application] [varchar](50) NULL,");
            stringBuilder.AppendLine("[Logged] [datetime] NULL,");
            stringBuilder.AppendLine("[Level] [varchar](50) NULL,");
            stringBuilder.AppendLine("[Message] [text] NULL,");
            stringBuilder.AppendLine("[Logger] [varchar](250) NULL,");
            stringBuilder.AppendLine("[Callsite] [varchar](512) NULL,");
            stringBuilder.AppendLine("[Exception] [text] NULL");
            stringBuilder.AppendLine(")");
            _dbase.CreateTable(stringBuilder.ToString(), "Log");
        }
    }
}
