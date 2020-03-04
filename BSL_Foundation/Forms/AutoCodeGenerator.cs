using KPIT_K_Foundation.Library;
using KPIT_K_Foundation.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace KPIT_K_Foundation
{
    public class AutoCodeGenerator : Form
    {
        private DataRow getWorkflowTables;
        private int _spPrefixSuffixIndex = 1;
        private const string _sqlGetAllTables = "EXEC sp_tables @table_type = \"'TABLE'\"";
        private const string _sqlGetAllViews = "EXEC sp_tables @table_type = \"'VIEW'\"";
        private DataSet _dsAllTables;
        private string _connectionString;
        private string _progressText;
        private Tables _tables;
        private int _progressCtr;
        private string _rootDirectory;
        private string _webAppRootDirectory;
        private string _webAPIRootDirectory;
        private Language _language;
        private string _fileExtension;
        private Tables _selectedTables;
        private DataTable _referencedTables;
        private bool _isUseStoredProc;
        private string _spPrefix;
        private string _spSuffix;
        private string _selectedJQueryUITheme;
        private bool _isCancelled;
        private bool _isCreateDirectoryError;
        private bool _isUseLogging;
        private bool _isUseFileLogging;
        private bool _isUseDatabaseLogging;
        private bool _isUseEventLogging;
        private bool _isUseSecurity;
        private bool _isUseCaching;
        private bool _isUseAuditLogging;
        private bool _isUseWebApplication;
        private bool _isUseWebAPI;
        private string _serialNumber;
        private string _activationCode;
        private const int _thingsToProcessAfterFileGenerationCtr = 37;
        private bool _isOverwriteLaunchSettingsJson;
        private bool _isOverwriteSiteCss;
        private bool _isOverwriteFunctionsFile;
        private bool _isOverwriteLayoutView;
        private bool _isOverwriteValidationScriptsPartialView;
        private bool _isOverwriteViewImportsView;
        private bool _isOverwriteViewStartPage;
        private bool _isOverwriteAppSettingsJson;
        private bool _isOverwriteBundleConfigJson;
        private bool _isOverwriteProgramClass;
        private bool _isOverwriteStartUpClassFile;
#pragma warning disable CS0414 // The field 'Default._isOverwriteProjectJson' is assigned but its value is never used
        private bool _isOverwriteProjectJson;
#pragma warning restore CS0414 // The field 'Default._isOverwriteProjectJson' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'Default._isOverwriteAppConfig' is assigned but its value is never used
        private bool _isOverwriteAppConfig;
#pragma warning restore CS0414 // The field 'Default._isOverwriteAppConfig' is assigned but its value is never used
        private bool _isOverwriteAssemblyInfo;
        private bool _isOverwriteAppSettingsClass;
        private bool _isOverwriteLaunchSettingsJsonWebApi;
        private bool _isOverwriteAppSettingsJsonWebApi;
        private bool _isOverwriteProgramClassWebApi;
        private bool _isOverwriteStartUpClassFileWebApi;
#pragma warning disable CS0414 // The field 'Default._isOverwriteProjectJsonWebApi' is assigned but its value is never used
        private bool _isOverwriteProjectJsonWebApi;
#pragma warning restore CS0414 // The field 'Default._isOverwriteProjectJsonWebApi' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'Default._isOverwriteAppConfigWebApi' is assigned but its value is never used
        private bool _isOverwriteAppConfigWebApi;
#pragma warning restore CS0414 // The field 'Default._isOverwriteAppConfigWebApi' is assigned but its value is never used
        private bool _isCheckedViewListCrudRedirect;
        private bool _isCheckedViewAddRecord;
        private bool _isCheckedViewUpdateRecord;
        private bool _isCheckedViewRecordDetails;
        private bool _isCheckedViewListReadOnly;
        private bool _isCheckedViewListCrud;
        private bool _isCheckedViewListGroupedBy;
        private bool _isCheckedViewListTotals;
        private bool _isCheckedViewListTotalsGroupedBy;
        private bool _isCheckedViewListSearch;
        private bool _isCheckedViewListScrollLoad;
        private bool _isCheckedViewListInline;
        private bool _isCheckedViewListForeach;
        private bool _isCheckedViewListMasterDetailGrid;
        private bool _isCheckedViewListMasterDetailSubGrid;
        private bool _isCheckedViewUnbound;
        private bool _isCheckedViewWorkflowSteps;
        private const string _alphaNumericDashUnderscoreRegex = "^[a-zA-Z0-9-_]+$";
        private DatabaseObjectToGenerateFrom _generateFrom;
        private string _appFilesDirectory;
        private List<string> _viewNameList;
        private bool _isViewNameUnique;
        private bool _isNoTableOrViewsFound;
        private const bool _isJQueryValidationChecked = true;
        private bool _isSqlVersion2012OrHigher;
        private bool _isEmailNotification;
        private string _softwareDownloadDirectoryAndName;
        private string _webApiBaseAddress;
        private string _semicolon;
        private GeneratedSqlType _generatedSqlType;
        private IContainer components;
        private ToolTip toolTip1;
        private Button BtnCancel;
        private Label LblProgress;
        private BackgroundWorker BackgroundWorker1;
        private Button BtnClose;
        private TabControl TabControl1;
        private TabPage TabSelectedViews;
        private PictureBox pictureBox47;
        private Button BtnClearSelectedViews;
        private Button BtnLoadViews;
        private CheckedListBox ClbxViews;
        private TabPage TabDatabase;
        private GroupBox GbxGenerateSql;
        private GroupBox groupBox9;
        private RadioButton RbtnUseLinqToEntities;
        private PictureBox pictureBox2;
        private GroupBox GbxGeneratedSqlScript;
        private PictureBox pictureBox56;
        private PictureBox pictureBox57;
        private RadioButton RbtnSqlScriptAutomatic;
        private RadioButton RbtnSqlScript2008;
        private GroupBox GbxStoredProcedure;
        private PictureBox pictureBox5;
        private PictureBox pictureBox4;
        private PictureBox pictureBox3;
        private RadioButton RbtnNoPrefixOrSuffix;
        private RadioButton RbtnSpSuffix;
        private RadioButton RbtnSpPrefix;
        private TextBox TxtSpSuffix;
        private TextBox TxtSpPrefix;
        private GroupBox GboxStoredProcOrLinq;
        private PictureBox pictureBox68;
        private RadioButton RbtnUseAdHocSql;
        private PictureBox pictureBox1;
        private RadioButton RbtnUseStoredProc;
        private GroupBox groupBox1;
        private PictureBox pictureBox42;
        private PictureBox PicbUserName;
        private PictureBox PicbPassword;
        private PictureBox PicbShowPassword;
        private PictureBox PicbDatabaseName;
        private CheckBox CbxRememberPassword;
        private Label label1;
        private TextBox TxtServer;
        private TextBox TxtDatabase;
        private TextBox TxtUserName;
        private TextBox TxtPassword;
        private Label label4;
        private Label label2;
        private Label label3;
        private TabPage TabGeneratedCode;
        private GroupBox groupBox3;
        private PictureBox pictureBox9;
        private PictureBox pictureBox6;
        private PictureBox pictureBox7;
        private PictureBox pictureBox8;
        private RadioButton RbtnGenerateFromSelectedViews;
        private RadioButton RbtnGenerateFromSelectedTables;
        private RadioButton RbtnGenerateFromAllViews;
        private RadioButton RbtnGenerateFromAllTables;
        private GroupBox groupBox2;
        private PictureBox pictureBox55;
        private PictureBox pictureBox15;
        private TextBox TxtAPINameDirectory;
        private Label label8;
        private PictureBox pictureBox13;
        private Label LblLanguage;
        private ComboBox CbxLanguage;
        private Label label5;
        private TextBox TxtAPIName;
        private TabPage TabUISettings;
        private GroupBox GbxViewNames;
        private TextBox TxtListMasterDetailSubGrid;
        private TextBox TxtListMasterDetailGrid;
        private TextBox TxtListForeach;
        private TextBox TxtListScrollLoad;
        private TextBox TxtListInline;
        private TextBox TxtListTotals;
        private TextBox TxtListTotalsGroupedBy;
        private TextBox TxtUnbound;
        private TextBox TxtListSearch;
        private TextBox TxtListCrudRedirect;
        private TextBox TxtAdd;
        private TextBox TxtUpdate;
        private TextBox TxtDetails;
        private TextBox TxtListReadOnly;
        private TextBox TxtListCrud;
        private TextBox TxtListGroupedBy;
        private GroupBox GbxThemes;
        private PictureBox pictureBox19;
        private ComboBox CbxJQueryUITheme;
        private Label label10;
        private GroupBox GbxWebFormsToGenerate;
        private CheckBox CbxListMasterDetailSubGrid;
        private CheckBox CbxListMasterDetailGrid;
        private PictureBox pictureBox54;
        private PictureBox pictureBox49;
        private CheckBox CbxListForeach;
        private PictureBox pictureBox34;
        private CheckBox CbxListScrollLoad;
        private PictureBox pictureBox51;
        private CheckBox CbxListInline;
        private PictureBox pictureBox50;
        private PictureBox pictureBox39;
        private PictureBox pictureBox21;
        private PictureBox pictureBox11;
        private PictureBox pictureBox10;
        private PictureBox pictureBox17;
        private PictureBox pictureBox24;
        private PictureBox pictureBox27;
        private CheckBox CbxListSearch;
        private CheckBox CbxListTotalsGroupedBy;
        private PictureBox pictureBox20;
        private CheckBox CbxUnbound;
        private PictureBox pictureBox29;
        private CheckBox CbxRecordDetails;
        private CheckBox CbxUpdateRecord;
        private PictureBox pictureBox16;
        private CheckBox CbxListTotals;
        private CheckBox CbxListCrud;
        private PictureBox pictureBox26;
        private PictureBox pictureBox22;
        private CheckBox CbxListGroupedBy;
        private CheckBox CbxAddNewRecord;
        private CheckBox CbxListReadOnly;
        private CheckBox CbxListCrudRedirect;
        private TabPage TabAppSettings;
        private PictureBox pictureBox66;
        private GroupBox GbxOverwriteWebApiFiles;
        private PictureBox pictureBox64;
        private CheckBox CbxOverwriteStartUpClassWebApi;
        private CheckBox CbxOverwriteProgramClassWebApi;
        private PictureBox pictureBox63;
        private CheckBox CbxOverwriteAppSettingsJsonWebApi;
        private PictureBox pictureBox62;
        private CheckBox CbxOverwriteLaunchSettingsJsonWebApi;
        private PictureBox pictureBox33;
        private GroupBox GbxOverwriteBusinessDataLayerFiles;
        private CheckBox CbxOverwriteAssemblyInfo;
        private PictureBox pictureBox40;
        private CheckBox CbxOverwriteAppSettingsClass;
        private PictureBox pictureBox36;
        private CheckBox CbxAutomaticallyOpenTab;
        private GroupBox GbxOverwriteFiles;
        private CheckBox CbxOverwriteStartUpClass;
        private CheckBox CbxOverwriteFunctionsFile;
        private CheckBox CbxOverwriteSiteCss;
        private PictureBox pictureBox53;
        private PictureBox pictureBox35;
        private CheckBox CbxOverwriteViewStartPage;
        private PictureBox pictureBox23;
        private PictureBox pictureBox18;
        private CheckBox CbxOverwriteProgramClass;
        private CheckBox CbxOverwriteLayoutPage;
        private PictureBox pictureBox52;
        private PictureBox pictureBox12;
        private CheckBox CbxOverwriteAppSettingsJson;
        private PictureBox pictureBox31;
        private PictureBox pictureBox45;
        private PictureBox pictureBox43;
        private PictureBox pictureBox32;
        private CheckBox CbxOverwriteLaunchSettingsJson;
        private PictureBox pictureBox44;
        private PictureBox pictureBox28;
        private CheckBox CbxOverwriteViewImportsView;
        private CheckBox CbxOverwriteBowerJson;
        private CheckBox CbxOverwriteBundleConfigJson;
        private CheckBox CbxOverwriteValidationScriptsPartialView;
        private Button BtnRestoreAllSettings;
        private TextBox TxtAppFilesDirectory;
        private Button BtnBrowseAppDirectory;
        private Label label7;
        private PictureBox pictureBox38;
        private TabPage TabCompSettings;
        private GroupBox GbxApplication;
        private CheckBox CbxUseWebApi;
        private PictureBox pictureBox48;
        private CheckBox CbxUseWebApplication;
        private PictureBox pictureBox67;
        private GroupBox GbxWebApi;
        private PictureBox pictureBox58;
        private PictureBox pictureBox59;
        private TextBox TxtWebAPINameDirectory;
        private Label label9;
        private Label label11;
        private TextBox TxtWebAPIName;
        private GroupBox groupBox4;
        private PictureBox pictureBox61;
        private CheckBox CbxGenerateCodeExamples;
        private Label label13;
        private TextBox TxtDevServerPort;
        private PictureBox pictureBox60;
        private PictureBox pictureBox41;
        private PictureBox pictureBox14;
        private TextBox TxtDirectory;
        private Button BtnBrowseCodeDirectory;
        private Label label6;
        private Label label12;
        private TextBox TxtWebAppName;
        private Button BtnBrowseWebAPIDirectory;
        private ProgressBar progressBar;
        private TabControl tabControl2;
        private TabPage tabComponents;
        private TabPage tabWorkflow;
        private GroupBox GbxConfiguration;
        private GroupBox GbxSMTPSettings;
        private TextBox textBox2;
        private PictureBox PicBoxShowPassword;
        private Button btnTestCionnection;
        private CheckBox CbxShowPassword;
        private PictureBox pictureBox77;
        private PictureBox pictureBox76;
        private PictureBox pictureBox75;
        private PictureBox pictureBox74;
        private TextBox TxtPasswordForSmtp;
        private TextBox TxtUserNameForSmtp;
        private TextBox textBox1;
        private Label label17;
        private Label label16;
        private Label label15;
        private Label label14;
        private CheckBox CbxEmailNotification;
        private PictureBox pictureBox72;
        private GroupBox GbxAuditLogging;
        private CheckBox CbxUseAuditLogging;
        private PictureBox pictureBox71;
        private GroupBox GbxSecurity;
        private CheckBox CbxUseSecurity;
        private PictureBox pictureBox69;
        private GroupBox GbxCaching;
        private CheckBox CbxUseCaching;
        private PictureBox pictureBox70;
        private GroupBox GbxLogging;
        private GroupBox GbxLoggingType;
        private CheckBox CbxUseEventLogging;
        private CheckBox CbxUseDatabaseLogging;
        private CheckBox CbxUseFileLogging;
        private PictureBox pictureBox65;
        private PictureBox pictureBox25;
        private PictureBox pictureBox30;
        private CheckBox CbxUseLogging;
        private PictureBox pictureBox37;
        private RadioButton rdoConfigurableWorkflows;
        private RadioButton rdoDefaultWorkflows;
        private TextBox txtNumberofWorflows;
        private Label lblNumberofWorkflows;
        private Button btnAdd;
        private Label lblNoOfSteps;
        private TextBox txtWorkflowName;
        private Label lblWorkflowName;
        private TextBox txtNoOfSteps;
        private Label lbEscalationTime;
        private TextBox txtEscalationTime;
        private Label lblFromStep;
        private ComboBox cmbFromStep;
        private Label lblToStep;
        private ComboBox cmbToStep;
        private GroupBox grpExceptionFlow;
        private Button btnCancelWorkflow;
        private Button btnFinalize;
        private PictureBox pictureBox81;
        private PictureBox pictureBox80;
        private PictureBox pictureBox79;
        private PictureBox pictureBox78;
        private PictureBox pictureBox82;
        private PictureBox pictureBox83;
        private CheckBox CbxWorkflow;
        private PictureBox pictureBox73;
        private GroupBox gbxWorkflowSettings;
        private TabPage TabSelectedTables;
        private PictureBox pictureBox46;
        private Button BtnClearSelectedTables;
        private Button BtnLoadTables;
        private CheckedListBox ClbxTables;
        private TextBox TxtWorkflowAssignSteps;
        private CheckBox CbxWorkflowAssignSteps;
        private PictureBox pictureBox84;
        private Button BtnGenerateCode;
        List<int> lstOfSteps = new List<int>();

        public AutoCodeGenerator()
        {
            this.InitializeComponent();
            this.rdoDefaultWorkflows.Checked = false;
            this.rdoConfigurableWorkflows.Checked = false;
            this.gbxWorkflowSettings.Enabled = false;
            this.btnAdd.Enabled = false;
        }

        private void AutoCodeGenerator_Load(object sender, EventArgs e)
        {
            this.LoadThisForm();
        }

        private void LoadThisForm()
        {
            if (MySettings.AppVersion == ApplicationVersion.Express)
                this.SetExpressSettings();
            else if (this.TxtAppFilesDirectory.Text.Contains("Express\\AppFiles") || Settings.Default.ApplicationDirectory.Contains("Express\\AppFiles"))
            {
                Settings.Default.ApplicationDirectory = "C:\\Program Files (x86)\\KPIT\\AspCoreGen 2.0 Razor Professional Plus\\AppFiles\\";
                this.TxtAppFilesDirectory.Text = "C:\\Program Files (x86)\\KPIT\\AspCoreGen 2.0 Razor Professional Plus\\AppFiles\\";
            }
            this.NoVbDotNetOnFirstSale();
            this.Icon = MySettings.AppIcon;
            this.Text = MySettings.AppTitle;
            this.GetSavedApplicationSettings();
            if (this.CbxLanguage.SelectedIndex == -1)
                this.CbxLanguage.SelectedIndex = 0;
            if (this.CbxJQueryUITheme.SelectedIndex == -1)
                this.CbxJQueryUITheme.SelectedIndex = 13;
            //if (!this.CbxGenerateCodeExamples.Enabled)
            //    this.CbxGenerateCodeExamples.Enabled = true;
            this.AcceptButton = BtnGenerateCode;
        }

        private void NoVbDotNetOnFirstSale()
        {
            this.CbxLanguage.SelectedIndex = 0;
            this.CbxLanguage.Enabled = false;
        }

        private void SetExpressSettings()
        {
            this.RbtnUseLinqToEntities.Checked = true;
            Settings.Default.GenerateSqlIndex = 1;
            this.CbxOverwriteValidationScriptsPartialView.Checked = false;
            this.GbxGenerateSql.Text = "Generate SQL (Pro+ Only)";
            this.GbxGenerateSql.Enabled = false;
            this.RbtnGenerateFromAllViews.Enabled = false;
            this.RbtnGenerateFromSelectedViews.Enabled = false;
            this.GbxThemes.Text = "Themes (Pro+ Only)";
            this.GbxThemes.Enabled = false;
            this.GbxWebFormsToGenerate.Text = "        Views to Generate (Pro+ Only)";
            this.GbxWebFormsToGenerate.Enabled = false;
            this.GbxOverwriteFiles.Text = "Overwrite Web Application Files (Pro+ Only)";
            this.GbxOverwriteFiles.Enabled = false;
            this.GbxOverwriteBusinessDataLayerFiles.Text = "Overwrite Business/Data Layer Files (Pro+ Only)";
            this.GbxOverwriteBusinessDataLayerFiles.Enabled = false;
            this.GbxOverwriteWebApiFiles.Text = "Overwrite Web API Files (Pro+ Only)";
            this.GbxOverwriteWebApiFiles.Enabled = false;
            this.CbxUseWebApi.Checked = false;
            Settings.Default.IsUseWebApi = false;
            this.GbxWebApi.Text = "Web API (Pro+ Only)";
            this.GbxWebApi.Enabled = false;
            this.CbxListCrudRedirect.Checked = false;
            this.CbxAddNewRecord.Checked = false;
            this.CbxUpdateRecord.Checked = false;
            this.CbxRecordDetails.Checked = false;
            this.CbxListReadOnly.Checked = false;
            this.CbxListCrud.Checked = false;
            this.CbxListGroupedBy.Checked = false;
            this.CbxListTotals.Checked = false;
            this.CbxListTotalsGroupedBy.Checked = false;
            this.CbxListSearch.Checked = false;
            this.CbxListScrollLoad.Checked = false;
            this.CbxListInline.Checked = false;
            this.CbxListForeach.Checked = false;
            this.CbxListMasterDetailGrid.Checked = false;
            this.CbxListMasterDetailSubGrid.Checked = false;
            this.CbxUnbound.Checked = true;
            this.CbxWorkflowAssignSteps.Checked = true;
            this.TxtListCrudRedirect.Text = string.Empty;
            this.TxtAdd.Text = string.Empty;
            this.TxtUpdate.Text = string.Empty;
            this.TxtDetails.Text = string.Empty;
            this.TxtListReadOnly.Text = string.Empty;
            this.TxtListCrud.Text = string.Empty;
            this.TxtListGroupedBy.Text = string.Empty;
            this.TxtListTotals.Text = string.Empty;
            this.TxtListTotalsGroupedBy.Text = string.Empty;
            this.TxtListSearch.Text = string.Empty;
            this.TxtListScrollLoad.Text = string.Empty;
            this.TxtListInline.Text = string.Empty;
            this.TxtListForeach.Text = string.Empty;
            this.TxtListMasterDetailGrid.Text = string.Empty;
            this.TxtListMasterDetailSubGrid.Text = string.Empty;
            this.TxtUnbound.Text = Settings.Default.NameForCheckedViewUnbound;
            this.TxtWorkflowAssignSteps.Text = Settings.Default.NameForCheckedWorkflowSteps;
            if (!this.TxtAppFilesDirectory.Text.Contains("Professional Plus") && !Settings.Default.ApplicationDirectory.Contains("Professional Plus"))
                return;
            Settings.Default.ApplicationDirectory = "C:\\Program Files (x86)\\KPIT\\AspCoreGen 2.0 Razor Express\\AppFiles\\";
            this.TxtAppFilesDirectory.Text = "C:\\Program Files (x86)\\KPIT\\AspCoreGen 2.0 Razor Express\\AppFiles\\";
        }

        private void AutoCodeGenerator_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RememberSettings();
        }

        private void AutoCodeGenerator_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void BtnAbout_Click(object sender, EventArgs e)
        {
            string str1 = string.Empty;
            string str2 = "and a Class Library Project";
            if (MySettings.AppVersion == ApplicationVersion.ProfessionalPlus)
            {
                str1 = "  The Pro Plus Edition also generates Linq-to-Entities (Entity Framework Core), or Stored Procedures, or Ad-Hoc SQL scripts.";
                str2 = "a Class Library Project, and an optional ASP.NET Core 2.1 Web API Project";
            }
            int num = (int)MessageBox.Show("Product: " + MySettings.AppTitle + "\r\n\r\nDeveloped by: BSL \r\n\r\nVersion: " + MySettings.AppVersionNumber + ", " + MySettings.AppVersion.ToString() + " \r\n\r\nContact Email: mspracticesupport@kpit.com\r\n\r\n" + MySettings.AppTitle + " automatically generates an ASP.NET Core 2.1 Razor Web Project, " + str2 + " based on the tables in your MS SQL Server database.\r\n\r\n" + MySettings.AppTitle + " generates a 3-tier, layered web application code structure in C# compatible with the .NET Core 2.1.  It generates front end code, middle tier and data tier classes." + str1, MySettings.AppTitle);
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnGenerateCode_Click(object sender, EventArgs e)
        {
            this.AssignVariablesBeforeChecking4BlankFields();
            this._appFilesDirectory = this.TxtAppFilesDirectory.Text;
            if (!this.IsRequiredFieldsBlank())
            {
                if (!this.CanConnectToDatabase())
                    return;
                this._selectedJQueryUITheme = this.CbxJQueryUITheme.SelectedItem.ToString().ToLower();
                this._isNoTableOrViewsFound = false;
                if (MySettings.AppVersion == ApplicationVersion.Express)
                {
                    this._isOverwriteLaunchSettingsJson = true;
                    this._isOverwriteSiteCss = true;
                    this._isOverwriteFunctionsFile = true;
                    this._isOverwriteLayoutView = true;
                    this._isOverwriteValidationScriptsPartialView = true;
                    this._isOverwriteViewImportsView = true;
                    this._isOverwriteViewStartPage = true;
                    this._isOverwriteAppSettingsJson = true;
                    this._isOverwriteBundleConfigJson = true;
                    this._isOverwriteProgramClass = true;
                    this._isOverwriteStartUpClassFile = true;
                    this._isOverwriteProjectJson = true;
                    this._isOverwriteAppConfig = true;
                    this._isOverwriteAssemblyInfo = true;
                    this._isOverwriteAppSettingsClass = true;
                    this._isOverwriteLaunchSettingsJsonWebApi = true;
                    this._isOverwriteAppSettingsJsonWebApi = true;
                    this._isOverwriteProgramClassWebApi = true;
                    this._isOverwriteStartUpClassFileWebApi = true;
                    this._isOverwriteProjectJsonWebApi = true;
                    this._isOverwriteAppConfigWebApi = true;
                }
                else
                    this.AssignAppSettingsPrivateVariablesToCheckedBoxControlValues();
                if(CbxWorkflow.Checked==true)
                  this.SetWorkflow();
                this._generateFrom = this.RbtnGenerateFromAllTables.Checked || this.RbtnGenerateFromSelectedTables.Checked ? DatabaseObjectToGenerateFrom.Tables : DatabaseObjectToGenerateFrom.Views;
                this.SetPath();
                this.RememberSettings();
                try
                {
                    Functions.DeleteErrorLog(this._rootDirectory);
                    this.TabControl1.Enabled = false;
                    this.BtnGenerateCode.Enabled = false;
                    this.BtnCancel.Enabled = true;
                    this.ControlBox = false;
                    this.BtnClose.Enabled = false;
                    this.RunGenerateCodeWorker();
                }
                catch (Exception ex)
                {
                    int num = (int)MessageBox.Show(ex.Message, MySettings.AppTitle);
                }
            }
            else
                this.ShowRequiredFieldError();
        }

        private void SetWorkflow()
        {
            DateTime dateTime = DateTime.Now;
            string sqlFormattedDate = dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            this._connectionString = this.GetConnectionString();
            WorkflowHelperClass workflowHelper = new WorkflowHelperClass(
                "WorkflowMaster",
                this.txtWorkflowName.Text.ToString(),
                Convert.ToInt32(this.txtNumberofWorflows.Text),
                Convert.ToInt32(this.txtNoOfSteps.Text),
                Convert.ToInt32(this.txtEscalationTime.Text),
                this.TxtUserName.Text,
                this.TxtUserName.Text,
                DateTime.Now,
                DateTime.Now,
                _connectionString,
                true, Convert.ToInt32(cmbFromStep.Text), Convert.ToInt32(cmbToStep.Text)
                );
        }
        private void SetPath()
        {
            if (this.CbxUseWebApplication.Checked)
            {
                this._rootDirectory = this.TxtDirectory.Text.Trim();
                if (this._rootDirectory.Substring(this._rootDirectory.Length - 1, 1) == "\\")
                    this._rootDirectory = this._rootDirectory.Substring(0, this._rootDirectory.Length - 1);
                this._rootDirectory = this._rootDirectory + "\\" + this.TxtWebAppName.Text;
                this._webAppRootDirectory = this._rootDirectory + "\\" + this.TxtWebAppName.Text.Trim() + "\\";
                this._webAPIRootDirectory = this.TxtWebAPINameDirectory.Text.Trim();
            }
            else if (!this.CbxUseWebApplication.Checked && this.CbxUseWebApi.Checked)
            {
                this._rootDirectory = this.TxtWebAPINameDirectory.Text.Trim();
                if (this._rootDirectory.Substring(this._rootDirectory.Length - 1, 1) == "\\")
                    this._rootDirectory = this._rootDirectory.Substring(0, this._rootDirectory.Length - 1);
                this._rootDirectory = this._rootDirectory + "\\" + this.TxtWebAPIName.Text;
                this._webAppRootDirectory = this.TxtDirectory.Text.Trim() + "\\" + this.TxtWebAppName.Text.Trim() + "\\";
                this._webAPIRootDirectory = this._rootDirectory + "\\" + this.TxtWebAPIName.Text.Trim();
            }
        }

        private void AssignVariablesBeforeChecking4BlankFields()
        {
            this.AssignAppSettingsPrivateVariablesToCheckedBoxControlValues();
            this._isCheckedViewListCrudRedirect = this.CbxListCrudRedirect.Checked;
            this._isCheckedViewAddRecord = this.CbxAddNewRecord.Checked;
            this._isCheckedViewUpdateRecord = this.CbxUpdateRecord.Checked;
            this._isCheckedViewRecordDetails = this.CbxRecordDetails.Checked;
            this._isCheckedViewListReadOnly = this.CbxListReadOnly.Checked;
            this._isCheckedViewListCrud = this.CbxListCrud.Checked;
            this._isCheckedViewListGroupedBy = this.CbxListGroupedBy.Checked;
            this._isCheckedViewListTotals = this.CbxListTotals.Checked;
            this._isCheckedViewListTotalsGroupedBy = this.CbxListTotalsGroupedBy.Checked;
            this._isCheckedViewListSearch = this.CbxListSearch.Checked;
            this._isCheckedViewListScrollLoad = this.CbxListScrollLoad.Checked;
            this._isCheckedViewListInline = this.CbxListInline.Checked;
            this._isCheckedViewListForeach = this.CbxListForeach.Checked;
            this._isCheckedViewListMasterDetailGrid = this.CbxListMasterDetailGrid.Checked;
            this._isCheckedViewListMasterDetailSubGrid = this.CbxListMasterDetailSubGrid.Checked;
            this._isCheckedViewUnbound = this.CbxUnbound.Checked;
            this._isCheckedViewWorkflowSteps = this.CbxWorkflowAssignSteps.Checked;
            this._isUseLogging = this.CbxUseLogging.Checked;
            this._isUseFileLogging = this.CbxUseFileLogging.Checked;
            this._isUseDatabaseLogging = this.CbxUseDatabaseLogging.Checked;
            this._isUseEventLogging = this.CbxUseEventLogging.Checked;
            this._isUseSecurity = this.CbxUseSecurity.Checked;
            this._isUseCaching = this.CbxUseCaching.Checked;
            this._isUseAuditLogging = this.CbxUseAuditLogging.Checked;
            this._isUseWebApplication = this.CbxUseWebApplication.Checked;
            this._isUseWebAPI = this.CbxUseWebApi.Checked;
            this._isEmailNotification = this.CbxEmailNotification.Checked;

            this._viewNameList = new List<string>();
            this._isViewNameUnique = true;
            if (this._isCheckedViewListCrudRedirect && !string.IsNullOrEmpty(this.TxtListCrudRedirect.Text.Trim()))
                this._viewNameList.Add(this.TxtListCrudRedirect.Text.Trim());
            if (this._isCheckedViewAddRecord && !string.IsNullOrEmpty(this.TxtAdd.Text.Trim()))
                this._viewNameList.Add(this.TxtAdd.Text.Trim());
            if (this._isCheckedViewUpdateRecord && !string.IsNullOrEmpty(this.TxtUpdate.Text.Trim()))
                this._viewNameList.Add(this.TxtUpdate.Text.Trim());
            if (this._isCheckedViewRecordDetails && !string.IsNullOrEmpty(this.TxtDetails.Text.Trim()))
                this._viewNameList.Add(this.TxtDetails.Text.Trim());
            if (this._isCheckedViewListReadOnly && !string.IsNullOrEmpty(this.TxtListReadOnly.Text.Trim()))
                this._viewNameList.Add(this.TxtListReadOnly.Text.Trim());
            if (this._isCheckedViewListCrud && !string.IsNullOrEmpty(this.TxtListCrud.Text.Trim()))
                this._viewNameList.Add(this.TxtListCrud.Text.Trim());
            if (this._isCheckedViewListGroupedBy && !string.IsNullOrEmpty(this.TxtListGroupedBy.Text.Trim()))
                this._viewNameList.Add(this.TxtListGroupedBy.Text.Trim());
            if (this._isCheckedViewListTotals && !string.IsNullOrEmpty(this.TxtListTotals.Text.Trim()))
                this._viewNameList.Add(this.TxtListTotals.Text.Trim());
            if (this._isCheckedViewListTotalsGroupedBy && !string.IsNullOrEmpty(this.TxtListTotalsGroupedBy.Text.Trim()))
                this._viewNameList.Add(this.TxtListTotalsGroupedBy.Text.Trim());
            if (this._isCheckedViewListSearch && !string.IsNullOrEmpty(this.TxtListSearch.Text.Trim()))
                this._viewNameList.Add(this.TxtListSearch.Text.Trim());
            if (this._isCheckedViewListScrollLoad && !string.IsNullOrEmpty(this.TxtListScrollLoad.Text.Trim()))
                this._viewNameList.Add(this.TxtListScrollLoad.Text.Trim());
            if (this._isCheckedViewListInline && !string.IsNullOrEmpty(this.TxtListInline.Text.Trim()))
                this._viewNameList.Add(this.TxtListInline.Text.Trim());
            if (this._isCheckedViewListForeach && !string.IsNullOrEmpty(this.TxtListForeach.Text.Trim()))
                this._viewNameList.Add(this.TxtListForeach.Text.Trim());
            if (this._isCheckedViewListMasterDetailGrid && !string.IsNullOrEmpty(this.TxtListMasterDetailGrid.Text.Trim()))
                this._viewNameList.Add(this.TxtListMasterDetailGrid.Text.Trim());
            if (this._isCheckedViewListMasterDetailSubGrid && !string.IsNullOrEmpty(this.TxtListMasterDetailSubGrid.Text.Trim()))
                this._viewNameList.Add(this.TxtListMasterDetailSubGrid.Text.Trim());
            if (this._isCheckedViewWorkflowSteps && !string.IsNullOrEmpty(this.TxtWorkflowAssignSteps.Text.Trim()))
                this._viewNameList.Add(this.TxtWorkflowAssignSteps.Text.Trim());
            if (!this._isCheckedViewUnbound || string.IsNullOrEmpty(this.TxtUnbound.Text.Trim()))
                return;
            this._viewNameList.Add(this.TxtUnbound.Text.Trim());
            
        }

        private void AssignAppSettingsPrivateVariablesToCheckedBoxControlValues()
        {
            this._isOverwriteLaunchSettingsJson = this.CbxOverwriteLaunchSettingsJson.Checked;
            this._isOverwriteSiteCss = this.CbxOverwriteSiteCss.Checked;
            this._isOverwriteFunctionsFile = this.CbxOverwriteFunctionsFile.Checked;
            this._isOverwriteLayoutView = this.CbxOverwriteLayoutPage.Checked;
            this._isOverwriteValidationScriptsPartialView = this.CbxOverwriteValidationScriptsPartialView.Checked;
            this._isOverwriteViewImportsView = this.CbxOverwriteViewImportsView.Checked;
            this._isOverwriteViewStartPage = this.CbxOverwriteViewStartPage.Checked;
            this._isOverwriteAppSettingsJson = this.CbxOverwriteAppSettingsJson.Checked;
            this._isOverwriteBundleConfigJson = this.CbxOverwriteBundleConfigJson.Checked;
            this._isOverwriteProgramClass = this.CbxOverwriteProgramClass.Checked;
            this._isOverwriteStartUpClassFile = this.CbxOverwriteStartUpClass.Checked;
            this._isOverwriteAssemblyInfo = this.CbxOverwriteAssemblyInfo.Checked;
            this._isOverwriteAppSettingsClass = this.CbxOverwriteAppSettingsClass.Checked;
            this._isOverwriteLaunchSettingsJsonWebApi = this.CbxOverwriteLaunchSettingsJsonWebApi.Checked;
            this._isOverwriteAppSettingsJsonWebApi = this.CbxOverwriteAppSettingsJsonWebApi.Checked;
            this._isOverwriteProgramClassWebApi = this.CbxOverwriteProgramClassWebApi.Checked;
            this._isOverwriteStartUpClassFileWebApi = this.CbxOverwriteStartUpClassWebApi.Checked;
        }

        private void RunGenerateCodeWorker()
        {
            Dbase dbase = new Dbase(this._connectionString);
            this._dsAllTables = this._generateFrom != DatabaseObjectToGenerateFrom.Tables ? dbase.GetDataSet("EXEC sp_tables @table_type = \"'VIEW'\"", true) : dbase.GetDataSet("EXEC sp_tables @table_type = \"'TABLE'\"", true);
            if (this.RbtnGenerateFromAllTables.Checked || this.RbtnGenerateFromAllViews.Checked)
            {
                if (this._dsAllTables != null)
                {
                    if (this._dsAllTables.Tables[0].Rows.Count > 0)
                    {
                        int num = 0;
                        foreach (DataRow row in (InternalDataCollectionBase)this._dsAllTables.Tables[0].Rows)
                        {
                            string lower1 = row["TABLE_NAME"].ToString().Trim().ToLower();
                            string lower2 = row["TABLE_OWNER"].ToString().Trim().ToLower();
                            if (lower1 != "sysdiagrams" && lower1 != "dtproperties" && (lower2 != "sys" && lower2 != "information_schema"))
                                ++num;
                        }
                        this.progressBar.Maximum = num * 2 + 1;
                        this.progressBar.Maximum += 40;
                        if (num == 1)
                            ++this.progressBar.Maximum;
                        this.BackgroundWorker1.RunWorkerAsync();
                    }
                    else
                        this.ShowNoTableOrViewFoundError(null);
                }
                else
                    this.ShowNoTableOrViewFoundError(null);
            }
            else
            {
                if (!this.RbtnGenerateFromSelectedTables.Checked && !this.RbtnGenerateFromSelectedViews.Checked)
                    return;
                CheckedListBox checkedListBox = this.ClbxTables;
                // Added list of new tables if not selected
                checkedListBox = AddWorkflowTables(checkedListBox);
                if (this.RbtnGenerateFromSelectedViews.Checked)
                    checkedListBox = this.ClbxViews;
                if (this._dsAllTables != null)
                {
                    if (this._dsAllTables.Tables[0].Rows.Count > 0)
                    {
                        if (checkedListBox.Items.Count <= 0)
                            return;
                        this.progressBar.Maximum = checkedListBox.CheckedItems.Count * 2 + 1;
                        this.progressBar.Maximum += 40;
                        if (checkedListBox.CheckedItems.Count == 1)
                            ++this.progressBar.Maximum;
                        this.BackgroundWorker1.RunWorkerAsync();
                    }
                    else
                        this.ShowNoTableOrViewFoundError(null);
                }
                else
                    this.ShowNoTableOrViewFoundError(null);
            }
        }

        private CheckedListBox AddWorkflowTables(CheckedListBox checkedListBox)
        {

            checkedListBox.Items.Add("dbo.rolemaster", true);
            checkedListBox.Items.Add("dbo.usermaster", true);
            checkedListBox.Items.Add("dbo.userroles", true);
            checkedListBox.Items.Add("dbo.workflowmaster", true);
            checkedListBox.Items.Add("dbo.workflowstepsmaster", true);
            return checkedListBox;
            
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.BackgroundWorker1.CancelAsync();
        }

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.TabControl1.SelectedIndex == 2)
                this.TxtServer.Focus();
            else if (this.TabControl1.SelectedIndex == 3)
                this.TxtWebAppName.Focus();
            else if (this.TabControl1.SelectedIndex == 4)
            {
                this.TxtListCrudRedirect.Focus();
            }
            else
            {
                if (this.TabControl1.SelectedIndex != 5)
                    return;
                this.TxtAppFilesDirectory.Focus();
            }
        }

        private void CbxRememberPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (this.CbxRememberPassword.Checked)
                this.TxtPassword.PasswordChar = char.MinValue;
            else
                this.TxtPassword.PasswordChar = '*';
        }

        private void RbtnUseLinqToEntities_CheckedChanged(object sender, EventArgs e)
        {
            if (this.RbtnUseLinqToEntities.Checked)
            {
                this.RbtnUseStoredProc.Checked = false;
                this.RbtnUseAdHocSql.Checked = false;
                this.GbxStoredProcedure.Enabled = false;
                this.GbxGeneratedSqlScript.Enabled = false;
                this.RbtnGenerateFromAllViews.Enabled = false;
                this.RbtnGenerateFromSelectedViews.Enabled = false;
                if (this.RbtnGenerateFromAllViews.Checked || this.RbtnGenerateFromSelectedViews.Checked)
                    this.RbtnGenerateFromAllTables.Checked = true;
                this._generatedSqlType = GeneratedSqlType.EFCore;
            }
            else
            {
                this.RbtnGenerateFromAllViews.Enabled = true;
                this.RbtnGenerateFromSelectedViews.Enabled = true;
            }
        }

        private void RbtnUseStoredProc_CheckedChanged(object sender, EventArgs e)
        {
            if (this.RbtnUseStoredProc.Checked)
            {
                this.RbtnUseLinqToEntities.Checked = false;
                this.RbtnUseAdHocSql.Checked = false;
                this.GbxStoredProcedure.Enabled = true;
                this.GbxGeneratedSqlScript.Enabled = true;
                this._generatedSqlType = GeneratedSqlType.StoredProcedures;
            }
            if (this.RbtnSpPrefix.Checked)
                this.TxtSpPrefix.Focus();
            if (!this.RbtnSpSuffix.Checked)
                return;
            this.TxtSpSuffix.Focus();
        }

        private void RbtnUseAdHocSql_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.RbtnUseAdHocSql.Checked)
                return;
            this.RbtnUseLinqToEntities.Checked = false;
            this.RbtnUseStoredProc.Checked = false;
            this.GbxStoredProcedure.Enabled = false;
            this.GbxGeneratedSqlScript.Enabled = true;
            this._generatedSqlType = GeneratedSqlType.AdHocSQL;
        }

        private void RbtnNoPrefixOrSuffix_CheckedChanged(object sender, EventArgs e)
        {
            this._spPrefixSuffixIndex = 1;
            this.TxtSpPrefix.Enabled = false;
            this.TxtSpSuffix.Enabled = false;
        }

        private void RbtnSpPrefix_CheckedChanged(object sender, EventArgs e)
        {
            this._spPrefixSuffixIndex = 2;
            this.TxtSpPrefix.Enabled = true;
            this.TxtSpSuffix.Enabled = false;
            this.TxtSpPrefix.Focus();
        }

        private void RbtnSpSuffix_CheckedChanged(object sender, EventArgs e)
        {
            this._spPrefixSuffixIndex = 3;
            this.TxtSpSuffix.Enabled = true;
            this.TxtSpPrefix.Enabled = false;
            this.TxtSpSuffix.Focus();
        }

        private void RbtnGenerateFromAllTables_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.RbtnGenerateFromAllTables.Checked)
                return;
            this.BtnGenerateCode.Enabled = true;
            this.AcceptButton = BtnGenerateCode;
            this.BtnGenerateCode.Text = "Generate Code for All Tables";
            this.DisableSelectedTablesAndSelectedViews();
            this.EnableUISettingsForTables();
            if (this.GbxWebFormsToGenerate.Enabled || MySettings.AppVersion == ApplicationVersion.Express)
                return;
            this.GbxWebFormsToGenerate.Enabled = true;
        }

        private void RbtnGenerateFromAllViews_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.RbtnGenerateFromAllViews.Checked)
                return;
            this.BtnGenerateCode.Enabled = true;
            this.AcceptButton = BtnGenerateCode;
            this.BtnGenerateCode.Text = "Generate Code for All Views";
            this.DisableSelectedTablesAndSelectedViews();
            this.EnableUISettingsForViews();
            this.DisableUISettingsForViews();
            if (!this.GbxWebFormsToGenerate.Enabled)
                return;
            this.GbxWebFormsToGenerate.Enabled = false;
        }

        private void DisableSelectedTablesAndSelectedViews()
        {
            this.ClbxTables.Enabled = false;
            this.BtnLoadTables.Enabled = false;
            this.BtnClearSelectedTables.Enabled = false;
            this.ClbxViews.Enabled = false;
            this.BtnLoadViews.Enabled = false;
            this.BtnClearSelectedViews.Enabled = false;
        }

        private void EnableUISettingsForTables()
        {
            if (!this.GbxThemes.Enabled)
                this.GbxThemes.Enabled = true;
            if (this.CbxListCrudRedirect.Checked)
                this.TxtListCrudRedirect.Enabled = true;
            if (this.CbxAddNewRecord.Checked)
                this.TxtAdd.Enabled = true;
            if (this.CbxUpdateRecord.Checked)
                this.TxtUpdate.Enabled = true;
            if (this.CbxRecordDetails.Checked)
                this.TxtDetails.Enabled = true;
            if (this.CbxListReadOnly.Checked)
                this.TxtListReadOnly.Enabled = true;
            if (this.CbxListCrud.Checked)
                this.TxtListCrud.Enabled = true;
            if (this.CbxListGroupedBy.Checked)
                this.TxtListGroupedBy.Enabled = true;
            if (this.CbxListTotals.Checked)
                this.TxtListTotals.Enabled = true;
            if (this.CbxListTotalsGroupedBy.Checked)
                this.TxtListTotalsGroupedBy.Enabled = true;
            if (this.CbxListSearch.Checked)
                this.TxtListSearch.Enabled = true;
            if (this.CbxListScrollLoad.Checked)
                this.TxtListScrollLoad.Enabled = true;
            if (this.CbxListInline.Checked)
                this.TxtListInline.Enabled = true;
            if (this.CbxListForeach.Checked)
                this.TxtListForeach.Enabled = true;
            if (this.CbxUnbound.Checked)
                this.TxtUnbound.Enabled = true;
            if (CbxWorkflowAssignSteps.Checked)
                this.TxtWorkflowAssignSteps.Enabled = true;
            if (this.CbxListMasterDetailGrid.Checked)
                this.TxtListMasterDetailGrid.Enabled = true;
            if (!this.CbxListMasterDetailSubGrid.Checked)
                return;
            this.TxtListMasterDetailSubGrid.Enabled = true;
        }

        private void DisableUISettingsForViews()
        {
            if (this.GbxThemes.Enabled)
                this.GbxThemes.Enabled = false;
            this.TxtListCrudRedirect.Enabled = false;
            this.TxtAdd.Enabled = false;
            this.TxtUpdate.Enabled = false;
            this.TxtDetails.Enabled = false;
            this.TxtListCrud.Enabled = false;
            this.TxtListGroupedBy.Enabled = false;
            this.TxtListTotals.Enabled = false;
            this.TxtListTotalsGroupedBy.Enabled = false;
            this.TxtListSearch.Enabled = false;
            this.TxtListScrollLoad.Enabled = false;
            this.TxtListInline.Enabled = false;
            this.TxtListForeach.Enabled = false;
            this.TxtUnbound.Enabled = false;
            this.TxtWorkflowAssignSteps.Enabled = false;
            this.TxtListMasterDetailGrid.Enabled = false;
            this.TxtListMasterDetailSubGrid.Enabled = false;
        }

        private void EnableUISettingsForViews()
        {
            this.CbxListReadOnly.Checked = true;
        }

        private void RbtnGenerateFromSelectedTables_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.RbtnGenerateFromSelectedTables.Checked)
                return;
            this.BtnGenerateCode.Text = "Generate Code for Selected Tables Only";
            this.ClbxTables.Enabled = true;
            this.BtnLoadTables.Enabled = true;
            if (this.ClbxTables.Items.Count > 0)
            {
                if (this.IsAnyItemChecked(this.ClbxTables))
                {
                    this.BtnClearSelectedTables.Enabled = true;
                }
                else
                {
                    this.BtnClearSelectedTables.Enabled = false;
                    this.BtnGenerateCode.Enabled = false;
                }
            }
            else
            {
                this.BtnClearSelectedTables.Enabled = false;
                this.BtnGenerateCode.Enabled = false;
            }
            this.ClbxViews.Enabled = false;
            this.BtnLoadViews.Enabled = false;
            this.BtnClearSelectedViews.Enabled = false;
            this.EnableUISettingsForTables();
            if (!this.GbxWebFormsToGenerate.Enabled && MySettings.AppVersion != ApplicationVersion.Express)
                this.GbxWebFormsToGenerate.Enabled = true;
            if (!this.CbxAutomaticallyOpenTab.Checked)
                return;
            this.TabControl1.SelectedIndex = 0;
        }

        private void RbtnGenerateFromSelectedViews_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.RbtnGenerateFromSelectedViews.Checked)
                return;
            this.BtnGenerateCode.Text = "Generate Code for Selected Views Only";
            this.ClbxViews.Enabled = true;
            this.BtnLoadViews.Enabled = true;
            if (this.ClbxViews.Items.Count > 0)
            {
                if (this.IsAnyItemChecked(this.ClbxViews))
                {
                    this.BtnClearSelectedViews.Enabled = true;
                }
                else
                {
                    this.BtnClearSelectedViews.Enabled = false;
                    this.BtnGenerateCode.Enabled = false;
                }
            }
            else
            {
                this.BtnClearSelectedViews.Enabled = false;
                this.BtnGenerateCode.Enabled = false;
            }
            this.ClbxTables.Enabled = false;
            this.BtnLoadTables.Enabled = false;
            this.BtnClearSelectedTables.Enabled = false;
            this.EnableUISettingsForViews();
            this.DisableUISettingsForViews();
            if (this.GbxWebFormsToGenerate.Enabled)
                this.GbxWebFormsToGenerate.Enabled = false;
            if (!this.CbxAutomaticallyOpenTab.Checked)
                return;
            this.TabControl1.SelectedIndex = 1;
        }

        private bool IsAnyItemChecked(CheckedListBox clbx)
        {
            IEnumerator enumerator = clbx.CheckedIndices.GetEnumerator();
            try
            {
                if (enumerator.MoveNext())
                {
                    int current = (int)enumerator.Current;
                    return true;
                }
            }
            finally
            {
                (enumerator as IDisposable)?.Dispose();
            }
            return false;
        }

        private void BtnBrowseCodeDirectory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowNewFolderButton = false;
            folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
                return;
            this.TxtDirectory.Text = folderBrowserDialog.SelectedPath;
            string str = this.TxtDirectory.Text.Trim();
            if (!(str.Substring(str.Length - 1, 1) != "\\"))
                return;
            this.TxtDirectory.Text += "\\";
        }

        private void CbxLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SetLanguageAndFileExtension();
        }

        private void SetLanguageAndFileExtension()
        {
            if (this.CbxLanguage.SelectedIndex == 0)
            {
                this._language = Language.CSharp;
                this._fileExtension = ".cs";
                this.TxtAPIName.Enabled = true;
            }
            else
            {
                this._language = Language.VB;
                this._fileExtension = ".vb";
                this.TxtAPIName.Enabled = false;
            }
        }

        private void TxtWebAppName_TextChanged(object sender, EventArgs e)
        {
            if (this.CbxUseWebApplication.Checked)
            {
                this.TxtAPIName.Text = this.TxtWebAppName.Text.Trim() + "API";
                this.TxtWebAPIName.Text = this.TxtWebAppName.Text.Trim() + "WebAPI";
                this.UpdateAPIDirectories();
            }
        }

        private void TxtDirectory_TextChanged(object sender, EventArgs e)
        {
            if (this.CbxUseWebApplication.Checked)
                this.UpdateAPIDirectories();
        }

        private void UpdateAPIDirectories()
        {
            this.TxtAPINameDirectory.Text = this.TxtDirectory.Text.Trim() + this.TxtWebAppName.Text.Trim() + "\\" + this.TxtAPIName.Text.Trim();
            this.TxtWebAPINameDirectory.Text = this.TxtDirectory.Text.Trim() + this.TxtWebAppName.Text.Trim() + "\\" + this.TxtWebAPIName.Text.Trim();
        }

        private void UpdateBusinessAPIDirectories()
        {
            if (string.IsNullOrEmpty(this.TxtWebAPINameDirectory.Text))
            {
                this.TxtAPINameDirectory.Text = string.Empty;
            }
            else
            {
                if (this.TxtWebAPINameDirectory.Text.Substring(this.TxtWebAPINameDirectory.Text.Length - 1, 1) == "\\")
                    this.TxtAPINameDirectory.Text = this.TxtWebAPINameDirectory.Text + this.TxtWebAPIName.Text.Trim() + "\\" + this.TxtAPIName.Text.Trim();
                else
                    this.TxtAPINameDirectory.Text = this.TxtWebAPINameDirectory.Text + "\\" + this.TxtWebAPIName.Text.Trim() + "\\" + this.TxtAPIName.Text.Trim();
            }
        }

        private void CbxListCrudRedirect_CheckedChanged(object sender, EventArgs e)
        {
            if (this.CbxListCrudRedirect.Checked)
            {
                this.CbxAddNewRecord.Checked = true;
                this.CbxUpdateRecord.Checked = true;
                this.CbxRecordDetails.Checked = true;
                this.TxtListCrudRedirect.Enabled = true;
                this.TxtAdd.Enabled = true;
                this.TxtUpdate.Enabled = true;
                this.TxtDetails.Enabled = true;
            }
            else
            {
                this.CbxAddNewRecord.Checked = false;
                this.CbxUpdateRecord.Checked = false;
                this.CbxRecordDetails.Checked = false;
                this.TxtListCrudRedirect.Enabled = false;
                this.TxtAdd.Enabled = false;
                this.TxtUpdate.Enabled = false;
                this.TxtDetails.Enabled = false;
            }
            this._isCheckedViewListCrudRedirect = this.CbxListCrudRedirect.Checked;
            this._isCheckedViewAddRecord = this.CbxAddNewRecord.Checked;
            this._isCheckedViewUpdateRecord = this.CbxUpdateRecord.Checked;
            this._isCheckedViewRecordDetails = this.CbxRecordDetails.Checked;
        }

        private void CbxAddNewRecord_CheckedChanged(object sender, EventArgs e)
        {
            if (this.CbxAddNewRecord.Checked)
                this.CbxListCrudRedirect.Checked = true;
            else
                this.CbxListCrudRedirect.Checked = false;
        }

        private void CbxUpdateRecord_CheckedChanged(object sender, EventArgs e)
        {
            if (this.CbxUpdateRecord.Checked)
                this.CbxListCrudRedirect.Checked = true;
            else
                this.CbxListCrudRedirect.Checked = false;
        }

        private void CbxRecordDetails_CheckedChanged(object sender, EventArgs e)
        {
            if (this.CbxRecordDetails.Checked)
                this.CbxListCrudRedirect.Checked = true;
            else
                this.CbxListCrudRedirect.Checked = false;
        }

        private void CbxListReadOnly_CheckedChanged(object sender, EventArgs e)
        {
            this._isCheckedViewListReadOnly = this.CbxListReadOnly.Checked;
            if (this.CbxListReadOnly.Checked)
                this.TxtListReadOnly.Enabled = true;
            else
                this.TxtListReadOnly.Enabled = false;
        }

        private void CbxListCrud_CheckedChanged(object sender, EventArgs e)
        {
            this._isCheckedViewListCrud = this.CbxListCrud.Checked;
            if (this.CbxListCrud.Checked)
                this.TxtListCrud.Enabled = true;
            else
                this.TxtListCrud.Enabled = false;
        }

        private void CbxListGroupedBy_CheckedChanged(object sender, EventArgs e)
        {
            this._isCheckedViewListGroupedBy = this.CbxListGroupedBy.Checked;
            if (this.CbxListGroupedBy.Checked)
                this.TxtListGroupedBy.Enabled = true;
            else
                this.TxtListGroupedBy.Enabled = false;
        }

        private void CbxListTotals_CheckedChanged(object sender, EventArgs e)
        {
            this._isCheckedViewListTotals = this.CbxListTotals.Checked;
            if (this.CbxListTotals.Checked)
                this.TxtListTotals.Enabled = true;
            else
                this.TxtListTotals.Enabled = false;
        }

        private void CbxListTotalsGroupedBy_CheckedChanged(object sender, EventArgs e)
        {
            this._isCheckedViewListTotalsGroupedBy = this.CbxListTotalsGroupedBy.Checked;
            if (this.CbxListTotalsGroupedBy.Checked)
                this.TxtListTotalsGroupedBy.Enabled = true;
            else
                this.TxtListTotalsGroupedBy.Enabled = false;
        }

        private void CbxListSearch_CheckedChanged(object sender, EventArgs e)
        {
            this._isCheckedViewListSearch = this.CbxListSearch.Checked;
            if (this.CbxListSearch.Checked)
                this.TxtListSearch.Enabled = true;
            else
                this.TxtListSearch.Enabled = false;
        }

        private void CbxListScrollLoad_CheckedChanged(object sender, EventArgs e)
        {
            this._isCheckedViewListScrollLoad = this.CbxListScrollLoad.Checked;
            if (this.CbxListScrollLoad.Checked)
                this.TxtListScrollLoad.Enabled = true;
            else
                this.TxtListScrollLoad.Enabled = false;
        }

        private void CbxListInline_CheckedChanged(object sender, EventArgs e)
        {
            this._isCheckedViewListInline = this.CbxListInline.Checked;
            if (this.CbxListInline.Checked)
                this.TxtListInline.Enabled = true;
            else
                this.TxtListInline.Enabled = false;
        }

        private void CbxListForeach_CheckedChanged(object sender, EventArgs e)
        {
            this._isCheckedViewListForeach = this.CbxListForeach.Checked;
            if (this.CbxListForeach.Checked)
                this.TxtListForeach.Enabled = true;
            else
                this.TxtListForeach.Enabled = false;
        }

        private void CbxListMasterDetailGrid_CheckedChanged(object sender, EventArgs e)
        {
            this._isCheckedViewListMasterDetailGrid = this.CbxListMasterDetailGrid.Checked;
            if (this.CbxListMasterDetailGrid.Checked)
                this.TxtListMasterDetailGrid.Enabled = true;
            else
                this.TxtListMasterDetailGrid.Enabled = false;
        }

        private void CbxListMasterDetailSubGrid_CheckedChanged(object sender, EventArgs e)
        {
            this._isCheckedViewListMasterDetailSubGrid = this.CbxListMasterDetailSubGrid.Checked;
            if (this.CbxListMasterDetailSubGrid.Checked)
                this.TxtListMasterDetailSubGrid.Enabled = true;
            else
                this.TxtListMasterDetailSubGrid.Enabled = false;
        }

        private void CbxUnbound_CheckedChanged(object sender, EventArgs e)
        {
            this._isCheckedViewUnbound = this.CbxUnbound.Checked;
            if (this.CbxUnbound.Checked)
                this.TxtUnbound.Enabled = true;
            else
                this.TxtUnbound.Enabled = false;
        }

        private void BtnBrowseAppDirectory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowNewFolderButton = false;
            folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
                return;
            this.TxtAppFilesDirectory.Text = folderBrowserDialog.SelectedPath;
            string str = this.TxtAppFilesDirectory.Text.Trim();
            if (!(str.Substring(str.Length - 1, 1) != "\\"))
                return;
            this.TxtAppFilesDirectory.Text += "\\";
        }

        private void BtnBrowseWebAPIDirectory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowNewFolderButton = false;
            folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
                return;
            this.TxtWebAPINameDirectory.Text = folderBrowserDialog.SelectedPath;
            string str = this.TxtWebAPINameDirectory.Text.Trim();
            if (!(str.Substring(str.Length - 1, 1) != "\\"))
                return;
            this.TxtWebAPINameDirectory.Text += "\\";
        }

        private void SoftwareDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.LblProgress.Text = e.ProgressPercentage.ToString() + "% completed.";
            this.progressBar.Value = e.ProgressPercentage;
        }

        private void SoftwareDownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            int num1 = (int)MessageBox.Show("Software Update download is completed.  Please go to " + this._softwareDownloadDirectoryAndName + " to run the update.", MySettings.AppTitle);
            this.LblProgress.Text = string.Empty;
            this.progressBar.Value = 0;
            try
            {
                Process.Start(this._softwareDownloadDirectoryAndName);
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                int num2 = (int)MessageBox.Show("An error occured while opening the software update located in " + this._softwareDownloadDirectoryAndName + ".  Please try to download the update again.  If you get this error again, please try to download at a later date, or you may also contact us at mspracticesupport@bsl.com for help.", MySettings.AppTitle);
            }
            this._softwareDownloadDirectoryAndName = string.Empty;
        }

        private void BtnRestoreAllSettings_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to restore all settings to their default values?", MySettings.AppTitle, MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            this.RestoreAppSettingsToDefault();
        }

        public void RestoreAppSettingsToDefault()
        {
            Settings.Default.Reset();
            this.GetSavedApplicationSettings();
            this.TxtPassword.Text = string.Empty;
            Settings.Default.Password = string.Empty;
            this.NoVbDotNetOnFirstSale();
        }

        private void BtnLoadTables_Click(object sender, EventArgs e)
        {
            this.LoadSelectedTablesOrViews();
        }

        private void BtnClearSelectedTables_Click(object sender, EventArgs e)
        {
            bool flag = false;
            if (this.BtnClearSelectedTables.Text == "Select All")
                flag = true;
            for (int index = 0; index < this.ClbxTables.Items.Count; ++index)
                this.ClbxTables.SetItemChecked(index, flag);
            if (flag)
            {
                this.BtnClearSelectedTables.Text = "Clear Selection";
                this.BtnGenerateCode.Enabled = true;
                this.AcceptButton = BtnGenerateCode;
            }
            else
            {
                this.BtnClearSelectedTables.Text = "Select All";
                this.BtnGenerateCode.Enabled = false;
            }
        }

        private void ClbxTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.TurnOnOrOffGenerateCodeForSelectedTables();
        }

        private void TurnOnOrOffGenerateCodeForSelectedTables()
        {
            if (this.ClbxTables.CheckedItems.Count == 0)
            {
                this.BtnGenerateCode.Enabled = false;
                this.BtnClearSelectedTables.Text = "Select All";
                this.AcceptButton = BtnLoadTables;
            }
            else
            {
                this.BtnGenerateCode.Enabled = true;
                this.AcceptButton = BtnGenerateCode;
                this.BtnClearSelectedTables.Text = "Clear Selection";
            }
        }

        private void BtnLoadViews_Click(object sender, EventArgs e)
        {
            this.LoadSelectedTablesOrViews();
        }

        private void BtnClearSelectedViews_Click(object sender, EventArgs e)
        {
            bool flag = false;
            var row = getWorkflowTables;
            if (this.BtnClearSelectedViews.Text == "Select All")
                flag = true;
            for (int index = 0; index < this.ClbxViews.Items.Count; ++index)
                this.ClbxViews.SetItemChecked(index, flag);
            if (flag)
            {
                this.BtnClearSelectedViews.Text = "Clear Selection";
                this.BtnGenerateCode.Enabled = true;
                this.AcceptButton = BtnGenerateCode;
            }
            else
            {
                this.BtnClearSelectedViews.Text = "Select All";
                this.BtnGenerateCode.Enabled = false;
            }
        }

        private void ClbxViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.TurnOnOrOffGenerateCodeForSelectedViews();
        }

        private void TurnOnOrOffGenerateCodeForSelectedViews()
        {
            if (this.ClbxViews.CheckedItems.Count == 0)
            {
                this.BtnGenerateCode.Enabled = false;
                this.BtnClearSelectedViews.Text = "Select All";
                this.AcceptButton = BtnLoadViews;
            }
            else
            {
                this.BtnGenerateCode.Enabled = true;
                this.AcceptButton = BtnGenerateCode;
                this.BtnClearSelectedViews.Text = "Clear Selection";
            }
        }

        private bool IsRequiredFieldsBlank()
        {
            if (string.IsNullOrEmpty(this.TxtServer.Text) || string.IsNullOrEmpty(this.TxtDatabase.Text) || (string.IsNullOrEmpty(this.TxtUserName.Text) || string.IsNullOrEmpty(this.TxtPassword.Text) || string.IsNullOrEmpty(this.TxtAppFilesDirectory.Text)))
                return true;

            if (this.RbtnGenerateFromAllTables.Checked || this.RbtnGenerateFromSelectedTables.Checked)
            {
                if (!this._isCheckedViewListCrudRedirect && !this._isCheckedViewAddRecord && !this._isCheckedViewWorkflowSteps && (!this._isCheckedViewUpdateRecord && 
                    !this._isCheckedViewRecordDetails) && (!this._isCheckedViewListReadOnly && !this._isCheckedViewListCrud && 
                    (!this._isCheckedViewListGroupedBy && !this._isCheckedViewListTotals)) && 
                    (!this._isCheckedViewListTotalsGroupedBy && !this._isCheckedViewListSearch && 
                    (!this._isCheckedViewListScrollLoad && !this._isCheckedViewListInline) && (!this._isCheckedViewListForeach &&
                    !this._isCheckedViewListMasterDetailGrid && (!this._isCheckedViewListMasterDetailSubGrid && 
                    !this._isCheckedViewUnbound) )) || (this._isCheckedViewListCrudRedirect && 
                    string.IsNullOrEmpty(this.TxtListCrudRedirect.Text.Trim()) || this._isCheckedViewAddRecord &&
                    string.IsNullOrEmpty(this.TxtAdd.Text.Trim()) || (this._isCheckedViewUpdateRecord && 
                    string.IsNullOrEmpty(this.TxtUpdate.Text.Trim()) || this._isCheckedViewRecordDetails && 
                    string.IsNullOrEmpty(this.TxtDetails.Text.Trim())) || (this._isCheckedViewListReadOnly && 
                    string.IsNullOrEmpty(this.TxtListReadOnly.Text.Trim()) || this._isCheckedViewListCrud && 
                    string.IsNullOrEmpty(this.TxtListCrud.Text.Trim()) || (this._isCheckedViewListGroupedBy && 
                    string.IsNullOrEmpty(this.TxtListGroupedBy.Text.Trim()) || this._isCheckedViewListTotals && 
                    string.IsNullOrEmpty(this.TxtListTotals.Text.Trim())))) || (this._isCheckedViewListTotalsGroupedBy && 
                    string.IsNullOrEmpty(this.TxtListTotalsGroupedBy.Text.Trim()) || this._isCheckedViewListSearch && 
                    string.IsNullOrEmpty(this.TxtListSearch.Text.Trim()) || (this._isCheckedViewListScrollLoad && 
                    string.IsNullOrEmpty(this.TxtListScrollLoad.Text.Trim()) || this._isCheckedViewListInline && 
                    string.IsNullOrEmpty(this.TxtListInline.Text.Trim())) || (this._isCheckedViewListForeach && 
                    string.IsNullOrEmpty(this.TxtListForeach.Text.Trim()) || this._isCheckedViewListMasterDetailGrid &&
                    string.IsNullOrEmpty(this.TxtListMasterDetailGrid.Text.Trim()) || 
                    (this._isCheckedViewListMasterDetailSubGrid && string.IsNullOrEmpty(this.TxtListMasterDetailSubGrid.Text.Trim()) || this._isCheckedViewUnbound && string.IsNullOrEmpty(this.TxtUnbound.Text.Trim())))))
                    return true;
                for (int index1 = 0; index1 < this._viewNameList.Count; ++index1)
                {
                    for (int index2 = 0; index2 < this._viewNameList.Count; ++index2)
                    {
                        if (this._viewNameList[index1].ToLower() == this._viewNameList[index2].ToLower() && index1 != index2)
                        {
                            this._isViewNameUnique = false;
                            return true;
                        }
                    }
                }
                if (this._isCheckedViewListCrudRedirect && !Regex.IsMatch(this.TxtListCrudRedirect.Text.Trim(), "^[a-zA-Z0-9-_]+$") || this._isCheckedViewAddRecord && !Regex.IsMatch(this.TxtAdd.Text.Trim(), "^[a-zA-Z0-9-_]+$") || (this._isCheckedViewUpdateRecord && !Regex.IsMatch(this.TxtUpdate.Text.Trim(), "^[a-zA-Z0-9-_]+$") || this._isCheckedViewRecordDetails && !Regex.IsMatch(this.TxtDetails.Text.Trim(), "^[a-zA-Z0-9-_]+$")) || (this._isCheckedViewListReadOnly && !Regex.IsMatch(this.TxtListReadOnly.Text.Trim(), "^[a-zA-Z0-9-_]+$") || this._isCheckedViewListCrud && !Regex.IsMatch(this.TxtListCrud.Text.Trim(), "^[a-zA-Z0-9-_]+$") || (this._isCheckedViewListGroupedBy && !Regex.IsMatch(this.TxtListGroupedBy.Text.Trim(), "^[a-zA-Z0-9-_]+$") || this._isCheckedViewListTotals && !Regex.IsMatch(this.TxtListTotals.Text.Trim(), "^[a-zA-Z0-9-_]+$"))) || (this._isCheckedViewListTotalsGroupedBy && !Regex.IsMatch(this.TxtListTotalsGroupedBy.Text.Trim(), "^[a-zA-Z0-9-_]+$") || this._isCheckedViewListSearch && !Regex.IsMatch(this.TxtListSearch.Text.Trim(), "^[a-zA-Z0-9-_]+$") || (this._isCheckedViewListScrollLoad && !Regex.IsMatch(this.TxtListScrollLoad.Text.Trim(), "^[a-zA-Z0-9-_]+$") || this._isCheckedViewListInline && !Regex.IsMatch(this.TxtListInline.Text.Trim(), "^[a-zA-Z0-9-_]+$")) || (this._isCheckedViewListForeach && !Regex.IsMatch(this.TxtListForeach.Text.Trim(), "^[a-zA-Z0-9-_]+$") || this._isCheckedViewListMasterDetailGrid && !Regex.IsMatch(this.TxtListMasterDetailGrid.Text.Trim(), "^[a-zA-Z0-9-_]+$") || (this._isCheckedViewListMasterDetailSubGrid && !Regex.IsMatch(this.TxtListMasterDetailSubGrid.Text.Trim(), "^[a-zA-Z0-9-_]+$") || this._isCheckedViewUnbound && !Regex.IsMatch(this.TxtUnbound.Text.Trim(), "^[a-zA-Z0-9-_]+$")))))
                    return true;
            }
            else if (string.IsNullOrEmpty(this.TxtListReadOnly.Text.Trim()) || !Regex.IsMatch(this.TxtListReadOnly.Text.Trim(), "^[a-zA-Z0-9-_]+$"))
                return true;

            if (!this._isUseWebApplication && !this._isUseWebAPI)
            {
                return true;
            }
            else if (!this._isUseWebApplication && this._isUseWebAPI)
            {
                if (string.IsNullOrEmpty(this.TxtWebAPINameDirectory.Text) || string.IsNullOrEmpty(this.TxtWebAPIName.Text) || string.IsNullOrEmpty(this.TxtAPIName.Text))
                    return true;
            }
            else if (this._isUseWebApplication)
            {
                if (string.IsNullOrEmpty(this.TxtDirectory.Text) || string.IsNullOrEmpty(this.TxtWebAppName.Text) || string.IsNullOrEmpty(this.TxtWebAPIName.Text) || string.IsNullOrEmpty(this.TxtAPIName.Text) || (this._isUseWebAPI && string.IsNullOrEmpty(this.TxtDevServerPort.Text.Trim())))
                    return true;
                if (!string.IsNullOrEmpty(this.TxtWebAppName.Text) && !Regex.IsMatch(this.TxtWebAppName.Text.Trim(), "^[a-zA-Z0-9-_]+$"))
                    return true;
            }

            if (this._isUseLogging)
            {
                if (!this._isUseFileLogging && !this._isUseDatabaseLogging && !this._isUseEventLogging)
                    return true;
            }

            if (this.CbxEmailNotification.Checked)
            {
                if (string.IsNullOrEmpty(this.TxtUserNameForSmtp.Text.Trim()) || string.IsNullOrEmpty(this.TxtPasswordForSmtp.Text.Trim()))
                    return true;
            }

            return !Directory.Exists(this.TxtAppFilesDirectory.Text) && MySettings.AppVersion != ApplicationVersion.Express || MySettings.AppVersion == ApplicationVersion.Express && !Directory.Exists(this._appFilesDirectory) || this.RbtnUseStoredProc.Checked && (this.RbtnSpPrefix.Checked && (string.IsNullOrEmpty(this.TxtSpPrefix.Text) || !Regex.IsMatch(this.TxtSpPrefix.Text.Trim(), "^[a-zA-Z0-9-_]+$")) || this.RbtnSpSuffix.Checked && (string.IsNullOrEmpty(this.TxtSpSuffix.Text) || !Regex.IsMatch(this.TxtSpSuffix.Text.Trim(), "^[a-zA-Z0-9-_]+$")));
        }

        private void ShowRequiredFieldError()
        {
            string text = "At least one required field is blank, or names are not unique, or names are not alphanumeric.  Please go to the Database Settings, Code Settings, UI Settings, or App Settings tab to correct the following errors." + Environment.NewLine;
            if (string.IsNullOrEmpty(this.TxtServer.Text.Trim()))
                text = text + Environment.NewLine + "- Server is required. (Database Settings)";
            if (string.IsNullOrEmpty(this.TxtDatabase.Text.Trim()))
                text = text + Environment.NewLine + "- Database Name is required. (Database Settings)";
            if (string.IsNullOrEmpty(this.TxtUserName.Text.Trim()))
                text = text + Environment.NewLine + "- Username is required. (Database Settings)";
            if (string.IsNullOrEmpty(this.TxtPassword.Text.Trim()))
                text = text + Environment.NewLine + "- Password is required. (Database Settings)";
            if (string.IsNullOrEmpty(this.TxtAppFilesDirectory.Text.Trim()))
                text = text + Environment.NewLine + "- App Files Directory is required. (App Settings)";
            if (this.CbxEmailNotification.Checked && string.IsNullOrEmpty(this.TxtUserNameForSmtp.Text.Trim()))
                text = text + Environment.NewLine + "- Username is required. (Component Settings)";
            if (this.CbxEmailNotification.Checked && string.IsNullOrEmpty(this.TxtPasswordForSmtp.Text.Trim()))
                text = text + Environment.NewLine + "- Password is required. (Component Settings)";
            else if (!Directory.Exists(this._appFilesDirectory))
                text = text + Environment.NewLine + "- App Files Directory does not exist. (App Settings)";

            if (this.RbtnUseStoredProc.Checked)
            {
                if (this.RbtnSpPrefix.Checked && string.IsNullOrEmpty(this.TxtSpPrefix.Text.Trim()))
                    text = text + Environment.NewLine + "- Stored Procedure Prefix is required. (Database Settings)";
                else if (this.RbtnSpSuffix.Checked && string.IsNullOrEmpty(this.TxtSpSuffix.Text.Trim()))
                    text = text + Environment.NewLine + "- Stored Procedure Suffix is required. (Database Settings)";
                if (this.RbtnSpPrefix.Checked && !string.IsNullOrEmpty(this.TxtSpPrefix.Text.Trim()) && !Regex.IsMatch(this.TxtSpPrefix.Text.Trim(), "^[a-zA-Z0-9-_]+$"))
                    text = text + Environment.NewLine + "- Invalid Stored Procedure Prefix.  Alphanumeric, dashes, underscores only. (Database Settings)";
                else if (this.RbtnSpSuffix.Checked && !string.IsNullOrEmpty(this.TxtSpSuffix.Text.Trim()) && !Regex.IsMatch(this.TxtSpSuffix.Text.Trim(), "^[a-zA-Z0-9-_]+$"))
                    text = text + Environment.NewLine + "- Invalid Stored Procedure Suffix.  Alphanumeric, dashes, underscores only. (Database Settings)";
            }
            if (this.RbtnGenerateFromAllTables.Checked || this.RbtnGenerateFromSelectedTables.Checked)
            {
                if (!this._isCheckedViewListCrudRedirect && !this._isCheckedViewAddRecord && (!this._isCheckedViewUpdateRecord && !this._isCheckedViewRecordDetails) && (!this._isCheckedViewListReadOnly && !this._isCheckedViewListCrud && (!this._isCheckedViewListGroupedBy && !this._isCheckedViewListTotals)) && (!this._isCheckedViewListTotalsGroupedBy && !this._isCheckedViewListSearch && (!this._isCheckedViewListScrollLoad && !this._isCheckedViewListInline) && (!this._isCheckedViewListForeach && !this._isCheckedViewListMasterDetailGrid && (!this._isCheckedViewListMasterDetailSubGrid && !this._isCheckedViewUnbound)&& !this._isCheckedViewWorkflowSteps)))
                {
                    text = text + Environment.NewLine + "- At least One (1) view to generate must be checked. (UI Settings)";
                }
                else
                {
                    if (this._isCheckedViewListCrudRedirect && string.IsNullOrEmpty(this.TxtListCrudRedirect.Text.Trim()))
                        text = text + Environment.NewLine + "- Name for List with Add, Edit Redirect, & Delete is required. (UI Settings)";
                    if (this._isCheckedViewAddRecord && string.IsNullOrEmpty(this.TxtAdd.Text.Trim()))
                        text = text + Environment.NewLine + "- Name for Add New Record is required. (UI Settings)";
                    if (this._isCheckedViewUpdateRecord && string.IsNullOrEmpty(this.TxtUpdate.Text.Trim()))
                        text = text + Environment.NewLine + "- Name for Update Record is required. (UI Settings)";
                    if (this._isCheckedViewRecordDetails && string.IsNullOrEmpty(this.TxtDetails.Text.Trim()))
                        text = text + Environment.NewLine + "- Name for Record Details (Read-Only) is required. (UI Settings)";
                    if (this._isCheckedViewListReadOnly && string.IsNullOrEmpty(this.TxtListReadOnly.Text.Trim()))
                        text = text + Environment.NewLine + "- Name for List (Read-Only) is required. (UI Settings)";
                    if (this._isCheckedViewListCrud && string.IsNullOrEmpty(this.TxtListCrud.Text.Trim()))
                        text = text + Environment.NewLine + "- Name for List with Add, Edit, & Delete (Same Page) is required. (UI Settings)";
                    if (this._isCheckedViewListGroupedBy && string.IsNullOrEmpty(this.TxtListGroupedBy.Text.Trim()))
                        text = text + Environment.NewLine + "- Name for List with Grouping is required. (UI Settings)";
                    if (this._isCheckedViewListTotals && string.IsNullOrEmpty(this.TxtListTotals.Text.Trim()))
                        text = text + Environment.NewLine + "- Name for List with Totals is required. (UI Settings)";
                    if (this._isCheckedViewListTotalsGroupedBy && string.IsNullOrEmpty(this.TxtListTotalsGroupedBy.Text.Trim()))
                        text = text + Environment.NewLine + "- Name for List with Total and Grouping is required. (UI Settings)";
                    if (this._isCheckedViewListSearch && string.IsNullOrEmpty(this.TxtListSearch.Text.Trim()))
                        text = text + Environment.NewLine + "- Name for List with Search is required. (UI Settings)";
                    if (this._isCheckedViewListScrollLoad && string.IsNullOrEmpty(this.TxtListScrollLoad.Text.Trim()))
                        text = text + Environment.NewLine + "- Name for List Scroll-Load Data is required. (UI Settings)";
                    if (this._isCheckedViewListInline && string.IsNullOrEmpty(this.TxtListInline.Text.Trim()))
                        text = text + Environment.NewLine + "- Name for List with Inline Add & Edit is required. (UI Settings)";
                    if (this._isCheckedViewListForeach && string.IsNullOrEmpty(this.TxtListForeach.Text.Trim()))
                        text = text + Environment.NewLine + "- Name for List with Manual Foreach is required. (UI Settings)";
                    if (this._isCheckedViewListMasterDetailGrid && string.IsNullOrEmpty(this.TxtListMasterDetailGrid.Text.Trim()))
                        text = text + Environment.NewLine + "- Name for List with Master Detail (Grid) is required. (UI Settings)";
                    if (this._isCheckedViewListMasterDetailSubGrid && string.IsNullOrEmpty(this.TxtListMasterDetailSubGrid.Text.Trim()))
                        text = text + Environment.NewLine + "- Name for List with Master Detail (Sub Grid) is required. (UI Settings)";
                    if (this._isCheckedViewUnbound && string.IsNullOrEmpty(this.TxtUnbound.Text.Trim()))
                        text = text + Environment.NewLine + "- Name for Unbound View is required. (UI Settings)";
                    if (!this._isViewNameUnique)
                        text = text + Environment.NewLine + "- View names must be unique. (UI Settings)";
                    if (this._isCheckedViewListCrudRedirect && !string.IsNullOrEmpty(this.TxtListCrudRedirect.Text.Trim()) && !Regex.IsMatch(this.TxtListCrudRedirect.Text.Trim(), "^[a-zA-Z0-9-_]+$"))
                        text = text + Environment.NewLine + "- Invalid name for List with Add, Edit Redirect, & Delete.  Alphanumeric, dashes, underscores only. (UI Settings)";
                    if (this._isCheckedViewAddRecord && !string.IsNullOrEmpty(this.TxtAdd.Text.Trim()) && !Regex.IsMatch(this.TxtAdd.Text.Trim(), "^[a-zA-Z0-9-_]+$"))
                        text = text + Environment.NewLine + "- Invalid name for Add New Record.  Alphanumeric, dashes, underscores only. (UI Settings)";
                    if (this._isCheckedViewUpdateRecord && !string.IsNullOrEmpty(this.TxtUpdate.Text.Trim()) && !Regex.IsMatch(this.TxtUpdate.Text.Trim(), "^[a-zA-Z0-9-_]+$"))
                        text = text + Environment.NewLine + "- Invalid name for Update Record.  Alphanumeric, dashes, underscores only. (UI Settings)";
                    if (this._isCheckedViewRecordDetails && !string.IsNullOrEmpty(this.TxtDetails.Text.Trim()) && !Regex.IsMatch(this.TxtDetails.Text.Trim(), "^[a-zA-Z0-9-_]+$"))
                        text = text + Environment.NewLine + "- Invalid name for Record Details (Read-Only)  Alphanumeric, dashes, underscores only. (UI Settings)";
                    if (this._isCheckedViewListReadOnly && !string.IsNullOrEmpty(this.TxtListReadOnly.Text.Trim()) && !Regex.IsMatch(this.TxtListReadOnly.Text.Trim(), "^[a-zA-Z0-9-_]+$"))
                        text = text + Environment.NewLine + "- Invalid name for List (Read-Only).  Alphanumeric, dashes, underscores only. (UI Settings)";
                    if (this._isCheckedViewListCrud && !string.IsNullOrEmpty(this.TxtListCrud.Text.Trim()) && !Regex.IsMatch(this.TxtListCrud.Text.Trim(), "^[a-zA-Z0-9-_]+$"))
                        text = text + Environment.NewLine + "- Invalid name for List with Add, Edit, & Delete (Same Page).  Alphanumeric, dashes, underscores only. (UI Settings)";
                    if (this._isCheckedViewListGroupedBy && !string.IsNullOrEmpty(this.TxtListGroupedBy.Text.Trim()) && !Regex.IsMatch(this.TxtListGroupedBy.Text.Trim(), "^[a-zA-Z0-9-_]+$"))
                        text = text + Environment.NewLine + "- Invalid name for List with Grouping.  Alphanumeric, dashes, underscores only. (UI Settings)";
                    if (this._isCheckedViewListTotals && !string.IsNullOrEmpty(this.TxtListTotals.Text.Trim()) && !Regex.IsMatch(this.TxtListTotals.Text.Trim(), "^[a-zA-Z0-9-_]+$"))
                        text = text + Environment.NewLine + "- Invalid name for List with Totals.  Alphanumeric, dashes, underscores only. (UI Settings)";
                    if (this._isCheckedViewListTotalsGroupedBy && !string.IsNullOrEmpty(this.TxtListTotalsGroupedBy.Text.Trim()) && !Regex.IsMatch(this.TxtListTotalsGroupedBy.Text.Trim(), "^[a-zA-Z0-9-_]+$"))
                        text = text + Environment.NewLine + "- Invalid name for List with Total and Grouping.  Alphanumeric, dashes, underscores only. (UI Settings)";
                    if (this._isCheckedViewListSearch && !string.IsNullOrEmpty(this.TxtListSearch.Text.Trim()) && !Regex.IsMatch(this.TxtListSearch.Text.Trim(), "^[a-zA-Z0-9-_]+$"))
                        text = text + Environment.NewLine + "- Invalid name for List with Search.  Alphanumeric, dashes, underscores only. (UI Settings)";
                    if (this._isCheckedViewListScrollLoad && !string.IsNullOrEmpty(this.TxtListScrollLoad.Text.Trim()) && !Regex.IsMatch(this.TxtListScrollLoad.Text.Trim(), "^[a-zA-Z0-9-_]+$"))
                        text = text + Environment.NewLine + "- Invalid name for List Scroll-Load Data.  Alphanumeric, dashes, underscores only. (UI Settings)";
                    if (this._isCheckedViewListInline && !string.IsNullOrEmpty(this.TxtListInline.Text.Trim()) && !Regex.IsMatch(this.TxtListInline.Text.Trim(), "^[a-zA-Z0-9-_]+$"))
                        text = text + Environment.NewLine + "- Invalid name for List with Inline Add & Edit.  Alphanumeric, dashes, underscores only. (UI Settings)";
                    if (this._isCheckedViewListForeach && !string.IsNullOrEmpty(this.TxtListForeach.Text.Trim()) && !Regex.IsMatch(this.TxtListForeach.Text.Trim(), "^[a-zA-Z0-9-_]+$"))
                        text = text + Environment.NewLine + "- Invalid name for List with Manual Foreach.  Alphanumeric, dashes, underscores only. (UI Settings)";
                    if (this._isCheckedViewListMasterDetailGrid && !string.IsNullOrEmpty(this.TxtListMasterDetailGrid.Text.Trim()) && !Regex.IsMatch(this.TxtListMasterDetailGrid.Text.Trim(), "^[a-zA-Z0-9-_]+$"))
                        text = text + Environment.NewLine + "- Invalid name for List with Master Detail (Grid).  Alphanumeric, dashes, underscores only. (UI Settings)";
                    if (this._isCheckedViewListMasterDetailSubGrid && !string.IsNullOrEmpty(this.TxtListMasterDetailSubGrid.Text.Trim()) && !Regex.IsMatch(this.TxtListMasterDetailSubGrid.Text.Trim(), "^[a-zA-Z0-9-_]+$"))
                        text = text + Environment.NewLine + "- Invalid name for List with Master Detail (Sub Grid).  Alphanumeric, dashes, underscores only. (UI Settings)";
                    if (this._isCheckedViewUnbound && !string.IsNullOrEmpty(this.TxtUnbound.Text.Trim()) && !Regex.IsMatch(this.TxtUnbound.Text.Trim(), "^[a-zA-Z0-9-_]+$"))
                        text = text + Environment.NewLine + "- Invalid name for Unbound View.  Alphanumeric, dashes, underscores only. (UI Settings)";
                    if (this._isCheckedViewWorkflowSteps && !string.IsNullOrEmpty(this.TxtWorkflowAssignSteps.Text.Trim()) && !Regex.IsMatch(this.TxtWorkflowAssignSteps.Text.Trim(), "^[a-zA-Z0-9-_]+$"))
                        text = text + Environment.NewLine + "- Invalid name for AssignWorkflowSteps View.  Alphanumeric, dashes, underscores only. (UI Settings)";
                }
            }
            else if (string.IsNullOrEmpty(this.TxtListReadOnly.Text.Trim()))
                text = text + Environment.NewLine + "- Name for List (Read-Only) is required. (UI Settings)";

            if (this.CbxUseLogging.Checked && !this.CbxUseFileLogging.Checked && !this.CbxUseDatabaseLogging.Checked && !this.CbxUseEventLogging.Checked)
                text = text + Environment.NewLine + "- Select atleast one logging type. (Log Settings)";

            if (!this._isUseWebApplication && !this._isUseWebAPI)
            {
                text = text + Environment.NewLine + "- At least One (1) application to generate must be checked. (Code Settings)";
            }
            else if (!this._isUseWebApplication && this._isUseWebAPI)
            {
                if (string.IsNullOrEmpty(this.TxtWebAPIName.Text.Trim()))
                    text = text + Environment.NewLine + "- Web API Name is required. (Code Settings)";
                if (string.IsNullOrEmpty(this.TxtWebAPINameDirectory.Text.Trim()))
                    text = text + Environment.NewLine + "- Web API Directory is required. (Code Settings)";
                if (string.IsNullOrEmpty(this.TxtAPIName.Text.Trim()))
                    text = text + Environment.NewLine + "- API Name is required. (Code Settings)";
            }
            else if (this._isUseWebApplication)
            {
                if (string.IsNullOrEmpty(this.TxtWebAppName.Text.Trim()))
                    text = text + Environment.NewLine + "- Web Application Name is required. (Code Settings)";
                if (string.IsNullOrEmpty(this.TxtDirectory.Text.Trim()))
                    text = text + Environment.NewLine + "- Web Application Directory is required. (Code Settings)";
                if (!string.IsNullOrEmpty(this.TxtWebAppName.Text.Trim()) && !Regex.IsMatch(this.TxtWebAppName.Text.Trim(), "^[a-zA-Z0-9-_]+$"))
                    text = text + Environment.NewLine + "- Invalid Web Application Name.  Alphanumeric, dashes, underscores only. (Code Settings)";
                if (this.CbxUseWebApi.Checked && string.IsNullOrEmpty(this.TxtDevServerPort.Text))
                    text = text + Environment.NewLine + "- Dev Server Port is required. (Code Settings)";
                if (string.IsNullOrEmpty(this.TxtWebAPIName.Text.Trim()))
                    text = text + Environment.NewLine + "- Web API Name is required. (Code Settings)";
                if (string.IsNullOrEmpty(this.TxtAPIName.Text.Trim()))
                    text = text + Environment.NewLine + "- API Name is required. (Code Settings)";
            }

            if (MessageBox.Show(text, MySettings.AppTitle) != DialogResult.OK)
                return;

            this.SetTextBoxFocus();
        }

        private void SetTextBoxFocus()
        {
            if (string.IsNullOrEmpty(this.TxtServer.Text))
                this.TxtServer.Focus();
            else if (string.IsNullOrEmpty(this.TxtDatabase.Text))
                this.TxtDatabase.Focus();
            else if (string.IsNullOrEmpty(this.TxtUserName.Text))
                this.TxtUserName.Focus();
            else if (string.IsNullOrEmpty(this.TxtPassword.Text))
            {
                this.TxtPassword.Focus();
            }
            else if (string.IsNullOrEmpty(this.TxtUserNameForSmtp.Text))
                this.TxtUserNameForSmtp.Focus();
            else if (string.IsNullOrEmpty(this.TxtPasswordForSmtp.Text))
                this.TxtPasswordForSmtp.Focus();
            else
            {
                if (!string.IsNullOrEmpty(this.TxtDirectory.Text))
                    return;
                this.TxtDirectory.Focus();
            }
        }

        private void LoadSelectedTablesOrViews()
        {
            this.AssignVariablesBeforeChecking4BlankFields();
            this._appFilesDirectory = this.TxtAppFilesDirectory.Text;
            if (!this.IsRequiredFieldsBlank())
            {
                if (!this.CanConnectToDatabase())
                    return;
                string str = "table";
                Button button = this.BtnLoadTables;
                CheckedListBox checkedListBox = this.ClbxTables;
                Dbase dbase = new Dbase(this._connectionString);
                if (this.RbtnGenerateFromSelectedTables.Checked)
                    this._dsAllTables = dbase.GetDataSet("EXEC sp_tables @table_type = \"'TABLE'\"", false);
                if (this.RbtnGenerateFromSelectedViews.Checked)
                {
                    str = "view";
                    button = this.BtnLoadViews;
                    checkedListBox = this.ClbxViews;
                    this._dsAllTables = dbase.GetDataSet("EXEC sp_tables @table_type = \"'VIEW'\"", false);
                }
                int num1 = 0;
                foreach (DataRow row in (InternalDataCollectionBase)this._dsAllTables.Tables[0].Rows)
                {
                    string lower1 = row["TABLE_NAME"].ToString().Trim().ToLower();
                    string lower2 = row["TABLE_OWNER"].ToString().Trim().ToLower();
                    if (lower1 != "sysdiagrams" && lower1 != "dtproperties" && (lower2 != "sys" && lower2 != "information_schema"))
                        ++num1;
                }
                if (num1 > 0)
                {
                    checkedListBox.Items.Clear();
                    foreach (DataRow row in (InternalDataCollectionBase)this._dsAllTables.Tables[0].Rows)
                    {
                        string lower1 = row["TABLE_NAME"].ToString().Trim().ToLower();
                        string lower2 = row["TABLE_OWNER"].ToString().Trim().ToLower();
                        if (lower1 != "sysdiagrams" && lower1 != "dtproperties" && (lower2 != "sys" && lower2 != "information_schema"))
                        {
                            StringBuilder stringBuilder = new StringBuilder();
                            stringBuilder.Append("select column_name from information_schema.columns ");
                            stringBuilder.Append("where table_schema = '" + lower2 + "' AND table_name = '" + lower1 + "' ");
                            stringBuilder.Append("Exec sp_pkeys @table_owner = '" + lower2 + "',  @table_name = '" + lower1 + "' ");
                            stringBuilder.Append("Exec sp_fkeys @fktable_owner = '" + lower2 + "',  @fktable_name = '" + lower1 + "' ");
                            DataSet dataSet = dbase.GetDataSet(stringBuilder.ToString(), false);
                            if (dataSet != null)
                            {
                                DataTable table1 = dataSet.Tables[0];
                                DataTable table2 = dataSet.Tables[1];
                                DataTable table3 = dataSet.Tables[2];
                                if (str == "table" && table2.Rows.Count > 0)
                                {
                                    if (table1.Rows.Count > table2.Rows.Count + table3.Rows.Count)
                                    {
                                        checkedListBox.Items.Add(lower2 + "." + lower1);
                                    }
                                    else
                                    {
                                        EnumerableRowCollection<string> first = table1.AsEnumerable().Select<DataRow, string>(c => c.Field<string>("column_name"));
                                        EnumerableRowCollection<string> enumerableRowCollection1 = table2.AsEnumerable().Select<DataRow, string>(pk => pk.Field<string>("column_name"));
                                        EnumerableRowCollection<string> enumerableRowCollection2 = table3.AsEnumerable().Select<DataRow, string>(fk => fk.Field<string>("fkcolumn_name"));
                                        EnumerableRowCollection<string> enumerableRowCollection3 = enumerableRowCollection1;
                                        if (first.Except<string>(enumerableRowCollection3).Except<string>(enumerableRowCollection2).Count<string>() > 0)
                                            checkedListBox.Items.Add(lower2 + "." + lower1);
                                    }
                                }
                                else
                                    checkedListBox.Items.Add(lower2 + "." + lower1);
                            }
                        }
                    }
                    if (str == "table")
                        this.BtnClearSelectedTables.Enabled = true;
                    else
                        this.BtnClearSelectedViews.Enabled = true;
                }
                else
                {
                    if (str == "table")
                        this.BtnClearSelectedTables.Enabled = false;
                    else
                        this.BtnClearSelectedViews.Enabled = false;
                    button.Enabled = true;
                    int num2 = (int)MessageBox.Show("No " + str + "s found!", MySettings.AppTitle);
                }
            }
            else
                this.ShowRequiredFieldError();
        }

        private bool CanConnectToDatabase()
        {
            this._connectionString = this.GetConnectionString();
            Dbase dbase = new Dbase(this._connectionString);
            int num1 = dbase.CanConnectToDatabase() ? 1 : 0;
            if (num1 == 0)
            {
                int num2 = (int)MessageBox.Show("Please check your Server, Database, User Name, and/or Password under the Database Settings table.  Make sure you type them correctly.", "Database Connection Error");
                return num1 != 0;
            }
            if (this.RbtnSqlScriptAutomatic.Checked)
            {
                this._isSqlVersion2012OrHigher = dbase.IsSqlVersion2012OrHigher;
                return num1 != 0;
            }
            this._isSqlVersion2012OrHigher = false;
            return num1 != 0;
        }

        private string GetConnectionString()
        {
            //return "Data Source=" + this.TxtServer.Text + ";Initial Catalog=" + this.TxtDatabase.Text + ";Integrated Security=True;";
            return "Data Source=" + this.TxtServer.Text + ";Initial Catalog=" + this.TxtDatabase.Text + ";User ID=" + this.TxtUserName.Text + ";Password=" + this.TxtPassword.Text;
        }

        private void RememberSettings()
        {
            Settings.Default.SelectedTabIndex = this.TabControl1.SelectedIndex;
            Settings.Default.IsGenerateCodeButtonEnabled = this.BtnGenerateCode.Enabled;
            Settings.Default.GenerateCodeButtonText = this.BtnGenerateCode.Text;
            Settings.Default.IsCancelButtonEnabled = this.BtnCancel.Enabled;
            Settings.Default.IsLoadTablesEnabled = this.BtnLoadTables.Enabled;
            Settings.Default.IsClearSelectedTablesEnabled = this.BtnClearSelectedTables.Enabled;
            Settings.Default.IsLoadViewsEnabled = this.BtnLoadViews.Enabled;
            Settings.Default.IsClearSelectedViewsEnabled = this.BtnClearSelectedViews.Enabled;
            Settings.Default.Server = this.TxtServer.Text;
            Settings.Default.DatabaseName = this.TxtDatabase.Text;
            Settings.Default.Username = this.TxtUserName.Text;
            Settings.Default.ShowPassword = this.CbxRememberPassword.Checked;
            if (this.CbxRememberPassword.Checked)
                Settings.Default.Password = this.TxtPassword.Text;
            Settings.Default.GenerateSqlIndex = !this.RbtnUseLinqToEntities.Checked ? (!this.RbtnUseStoredProc.Checked ? 3 : 2) : 1;
            Settings.Default.StoredProcPrefixSuffixIndex = !this.RbtnNoPrefixOrSuffix.Checked ? (!this.RbtnSpPrefix.Checked ? 3 : 2) : 1;
            Settings.Default.SpPrefix = this.TxtSpPrefix.Text;
            Settings.Default.SpSuffix = this.TxtSpSuffix.Text;
            Settings.Default.IsGenerateCodeExamples = this.CbxGenerateCodeExamples.Checked;
            Settings.Default.DatabaseObjectsToGenerateFromIndex = !this.RbtnGenerateFromAllTables.Checked ? (!this.RbtnGenerateFromAllViews.Checked ? (!this.RbtnGenerateFromSelectedTables.Checked ? 4 : 3) : 2) : 1;
            Settings.Default.Directory = this.TxtDirectory.Text;
            Settings.Default.Language = this.CbxLanguage.SelectedIndex;
            Settings.Default.WebsiteName = this.TxtWebAppName.Text;
            Settings.Default.IsUseWebApi = this.CbxUseWebApi.Checked;
            Settings.Default.DevServerPort = this.TxtDevServerPort.Text;
            Settings.Default.IsCheckedViewListCrudRedirect = this.CbxListCrudRedirect.Checked;
            Settings.Default.IsCheckedViewAddRecord = this.CbxAddNewRecord.Checked;
            Settings.Default.IsCheckedViewUpdateRecord = this.CbxUpdateRecord.Checked;
            Settings.Default.IsCheckedViewRecordDetails = this.CbxRecordDetails.Checked;
            Settings.Default.IsCheckedViewListReadOnly = this.CbxListReadOnly.Checked;
            Settings.Default.IsCheckedViewListCrud = this.CbxListCrud.Checked;
            Settings.Default.IsCheckedViewListGroupedBy = this.CbxListGroupedBy.Checked;
            Settings.Default.IsCheckedViewListTotals = this.CbxListTotals.Checked;
            Settings.Default.IsCheckedViewListTotalsGroupedBy = this.CbxListTotalsGroupedBy.Checked;
            Settings.Default.IsCheckedViewListSearch = this.CbxListSearch.Checked;
            Settings.Default.IsCheckedViewListScrollLoad = this.CbxListScrollLoad.Checked;
            Settings.Default.IsCheckedViewListInline = this.CbxListInline.Checked;
            Settings.Default.IsCheckedViewListForeach = this.CbxListForeach.Checked;
            Settings.Default.IsCheckedViewListMasterDetailGrid = this.CbxListMasterDetailGrid.Checked;
            Settings.Default.IsCheckedViewListMasterDetailSubGrid = this.CbxListMasterDetailSubGrid.Checked;
            Settings.Default.IsCheckedViewUnbound = this.CbxUnbound.Checked;
            Settings.Default.NameForCheckedViewListCrudRedirect = this.TxtListCrudRedirect.Text.Trim();
            Settings.Default.NameForCheckedViewAddRecord = this.TxtAdd.Text.Trim();
            Settings.Default.NameForCheckedViewUpdateRecord = this.TxtUpdate.Text.Trim();
            Settings.Default.NameForCheckedViewRecordDetails = this.TxtDetails.Text.Trim();
            Settings.Default.NameForCheckedViewListReadOnly = this.TxtListReadOnly.Text.Trim();
            Settings.Default.NameForCheckedViewListCrud = this.TxtListCrud.Text.Trim();
            Settings.Default.NameForCheckedViewListGroupedBy = this.TxtListGroupedBy.Text.Trim();
            Settings.Default.NameForCheckedViewListTotals = this.TxtListTotals.Text.Trim();
            Settings.Default.NameForCheckedViewListTotalsGroupedBy = this.TxtListTotalsGroupedBy.Text.Trim();
            Settings.Default.NameForCheckedViewListSearch = this.TxtListSearch.Text.Trim();
            Settings.Default.NameForCheckedViewListScrollLoad = this.TxtListScrollLoad.Text.Trim();
            Settings.Default.NameForCheckedViewListInline = this.TxtListInline.Text.Trim();
            Settings.Default.NameForCheckedViewListForeach = this.TxtListForeach.Text.Trim();
            Settings.Default.NameForCheckedViewListMasterDetailGrid = this.TxtListMasterDetailGrid.Text.Trim();
            Settings.Default.NameForCheckedViewListMasterDetailSubGrid = this.TxtListMasterDetailSubGrid.Text.Trim();
            Settings.Default.NameForCheckedViewUnbound = this.TxtUnbound.Text.Trim();
            Settings.Default.NameForCheckedWorkflowSteps = this.TxtWorkflowAssignSteps.Text.Trim();
            Settings.Default.JQueryUITheme = this.CbxJQueryUITheme.SelectedIndex;
            Settings.Default.IsOverwriteLaunchSettingsJson = this.CbxOverwriteLaunchSettingsJson.Checked;
            Settings.Default.IsOverwriteSiteCss = this.CbxOverwriteSiteCss.Checked;
            Settings.Default.IsOverwriteFunctionsFile = this.CbxOverwriteFunctionsFile.Checked;
            Settings.Default.IsOverwriteLayoutPage = this.CbxOverwriteLayoutPage.Checked;
            Settings.Default.IsOverwriteValidationScriptsPartialView = this.CbxOverwriteValidationScriptsPartialView.Checked;
            Settings.Default.IsOverwriteViewImportsView = this.CbxOverwriteViewImportsView.Checked;
            Settings.Default.IsOverwriteViewStartPage = this.CbxOverwriteViewStartPage.Checked;
            Settings.Default.IsOverwriteAppSettingsJson = this.CbxOverwriteAppSettingsJson.Checked;
            Settings.Default.IsOverwriteBundleConfigJson = this.CbxOverwriteBundleConfigJson.Checked;
            Settings.Default.IsOverwriteProgramClassFile = this.CbxOverwriteProgramClass.Checked;
            Settings.Default.IsOverwriteStartUpClassFile = this.CbxOverwriteStartUpClass.Checked;
            Settings.Default.IsOverwriteAssemblyInfo = this.CbxOverwriteAssemblyInfo.Checked;
            Settings.Default.IsOverwritProjectJsonBusLayer = this.CbxOverwriteAppSettingsClass.Checked;
            Settings.Default.IsOverwriteLaunchSettingsJsonWebApi = this.CbxOverwriteLaunchSettingsJsonWebApi.Checked;
            Settings.Default.IsOverwriteAppSettingsWebApi = this.CbxOverwriteAppSettingsJsonWebApi.Checked;
            Settings.Default.IsOverwriteProgramClassWebApi = this.CbxOverwriteProgramClassWebApi.Checked;
            Settings.Default.IsOverwriteStartUpClassWebApi = this.CbxOverwriteStartUpClassWebApi.Checked;
            Settings.Default.ApplicationDirectory = this.TxtAppFilesDirectory.Text;
            Settings.Default.IsAutomaticallyOpenTab = this.CbxAutomaticallyOpenTab.Checked;
            Settings.Default.IsUseLogging = this.CbxUseLogging.Checked;
            Settings.Default.IsUseFileLogging = this.CbxUseFileLogging.Checked;
            Settings.Default.IsUseDatabaseLogging = this.CbxUseDatabaseLogging.Checked;
            Settings.Default.IsUseEventLogging = this.CbxUseEventLogging.Checked;
            Settings.Default.IsUseWebApplication = this.CbxUseWebApplication.Checked;
            Settings.Default.IsUseSecurity = this.CbxUseSecurity.Checked;
            Settings.Default.IsUseCaching = this.CbxUseCaching.Checked;
            Settings.Default.IsUseAuditLogging = this.CbxUseAuditLogging.Checked;

            Settings.Default.IsEmailNotification = this.CbxEmailNotification.Checked;
            Settings.Default.UserNameForSmtp = this.TxtUserNameForSmtp.Text.Trim();
            Settings.Default.NumberofWorflows = this.txtNumberofWorflows.Text.Trim();
            Settings.Default.NumberOfSteps = this.txtNoOfSteps.Text.Trim();
            Settings.Default.EscalationTime = this.txtEscalationTime.Text.Trim();

            Settings.Default.IsTxtWebAppNameEnabled = this.TxtWebAppName.Enabled;
            Settings.Default.IsTxtDirectoryEnabled = this.TxtDirectory.Enabled;
            Settings.Default.IsBtnBrowseCodeDirectoryEnabled = this.BtnBrowseCodeDirectory.Enabled;
            Settings.Default.IsCbxGenerateCodeExamplesEnabled = this.CbxGenerateCodeExamples.Enabled;
            Settings.Default.IsTxtDevServerPortEnabled = this.TxtDevServerPort.Enabled;
            Settings.Default.IsTxtWebAPINameEnabled = this.TxtWebAPIName.Enabled;
            Settings.Default.IsTxtWebAPINameDirectoryEnabled = this.TxtWebAPINameDirectory.Enabled;
            Settings.Default.IsBtnBrowseWebAPIDirectoryEnabled = this.BtnBrowseWebAPIDirectory.Enabled;

            Settings.Default.WebAPIName = this.TxtWebAPIName.Text.Trim();
            Settings.Default.WebAPINameDirectory = this.TxtWebAPINameDirectory.Text.Trim();
            Settings.Default.APIName = this.TxtAPIName.Text.Trim();
            Settings.Default.APINameDirectory = this.TxtAPINameDirectory.Text.Trim();
            Settings.Default.Save();
        }

        private void GetSavedApplicationSettings()
        {
            this.TabControl1.SelectedIndex = Settings.Default.SelectedTabIndex;
            this.BtnGenerateCode.Enabled = Settings.Default.IsGenerateCodeButtonEnabled;
            this.BtnGenerateCode.Text = Settings.Default.GenerateCodeButtonText;
            this.BtnCancel.Enabled = Settings.Default.IsCancelButtonEnabled;
            this.BtnLoadTables.Enabled = Settings.Default.IsLoadTablesEnabled;
            this.BtnClearSelectedTables.Enabled = Settings.Default.IsClearSelectedTablesEnabled;
            this.BtnLoadViews.Enabled = Settings.Default.IsLoadViewsEnabled;
            this.BtnClearSelectedViews.Enabled = Settings.Default.IsClearSelectedViewsEnabled;
            this.TxtServer.Text = Settings.Default.Server;
            this.TxtDatabase.Text = Settings.Default.DatabaseName;
            this.TxtUserName.Text = Settings.Default.Username;
            this.TxtUserNameForSmtp.Text = Settings.Default.UserNameForSmtp;
            this.txtNumberofWorflows.Text = Settings.Default.NumberofWorflows;
            this.txtWorkflowName.Text = Settings.Default.WorkflowName;
            this.txtNoOfSteps.Text = Settings.Default.NumberOfSteps;
            this.txtEscalationTime.Text = Settings.Default.EscalationTime;

            this.CbxRememberPassword.Checked = Settings.Default.ShowPassword;
            if (this.CbxRememberPassword.Checked)
                this.TxtPassword.Text = Settings.Default.Password;
            if (Settings.Default.GenerateSqlIndex == 1)
            {
                this.RbtnUseLinqToEntities.Checked = true;
                this.RbtnUseStoredProc.Checked = false;
                this.RbtnUseAdHocSql.Checked = false;
                this._generatedSqlType = GeneratedSqlType.EFCore;
            }
            else if (Settings.Default.GenerateSqlIndex == 2)
            {
                this.RbtnUseStoredProc.Checked = true;
                this.RbtnUseLinqToEntities.Checked = false;
                this.RbtnUseAdHocSql.Checked = false;
                this._generatedSqlType = GeneratedSqlType.StoredProcedures;
            }
            else
            {
                this.RbtnUseAdHocSql.Checked = true;
                this.RbtnUseLinqToEntities.Checked = false;
                this.RbtnUseStoredProc.Checked = false;
                this._generatedSqlType = GeneratedSqlType.AdHocSQL;
            }
            if (Settings.Default.StoredProcPrefixSuffixIndex == 1)
                this.RbtnNoPrefixOrSuffix.Checked = true;
            else if (Settings.Default.StoredProcPrefixSuffixIndex == 2)
                this.RbtnSpPrefix.Checked = true;
            else
                this.RbtnSpSuffix.Checked = true;
            this.TxtSpPrefix.Text = Settings.Default.SpPrefix;
            this.TxtSpSuffix.Text = Settings.Default.SpSuffix;
            this.CbxGenerateCodeExamples.Checked = Settings.Default.IsGenerateCodeExamples;
            if (Settings.Default.DatabaseObjectsToGenerateFromIndex == 1)
                this.RbtnGenerateFromAllTables.Checked = true;
            else if (Settings.Default.DatabaseObjectsToGenerateFromIndex == 2)
                this.RbtnGenerateFromAllViews.Checked = true;
            else if (Settings.Default.DatabaseObjectsToGenerateFromIndex == 3)
                this.RbtnGenerateFromSelectedTables.Checked = true;
            else
                this.RbtnGenerateFromSelectedViews.Checked = true;
            this.TxtDirectory.Text = Settings.Default.Directory;
            this.CbxLanguage.SelectedIndex = Settings.Default.Language;
            this.CbxUseWebApplication.Checked = Settings.Default.IsUseWebApplication;
            this.CbxUseWebApi.Checked = Settings.Default.IsUseWebApi;
            this.TxtWebAppName.Text = Settings.Default.WebsiteName;
            this.TxtDevServerPort.Text = Settings.Default.DevServerPort;
            this.TxtWebAPIName.Text = Settings.Default.WebAPIName;
            this.TxtWebAPINameDirectory.Text = Settings.Default.WebAPINameDirectory;
            this.TxtAPIName.Text = Settings.Default.APIName;
            this.TxtAPINameDirectory.Text = Settings.Default.APINameDirectory;
            if (this.CbxUseWebApplication.Checked)
            {
                if (string.IsNullOrEmpty(this.TxtDirectory.Text))
                {
                    this.TxtAPINameDirectory.Text = string.Empty;
                    this.TxtWebAPINameDirectory.Text = string.Empty;
                }
                else
                {
                    this.TxtAPINameDirectory.Text = this.TxtDirectory.Text.Trim() + this.TxtWebAppName.Text.Trim() + "\\" + this.TxtAPIName.Text.Trim();
                    this.TxtWebAPINameDirectory.Text = this.TxtDirectory.Text.Trim() + this.TxtWebAppName.Text.Trim() + "\\" + this.TxtWebAPIName.Text.Trim();
                }
            }
            else if (!this.CbxUseWebApplication.Checked && this.CbxUseWebApi.Checked)
            {
                if (string.IsNullOrEmpty(this.TxtWebAPINameDirectory.Text))
                {
                    this.TxtAPINameDirectory.Text = string.Empty;
                }
                else
                {
                    if (this.TxtWebAPINameDirectory.Text.Substring(this.TxtWebAPINameDirectory.Text.Length - 1, 1) == "\\")
                        this.TxtAPINameDirectory.Text = this.TxtWebAPINameDirectory.Text + this.TxtWebAPIName.Text.Trim() + "\\" + this.TxtAPIName.Text.Trim();
                    else
                        this.TxtAPINameDirectory.Text = this.TxtWebAPINameDirectory.Text + "\\" + this.TxtWebAPIName.Text.Trim() + "\\" + this.TxtAPIName.Text.Trim();
                }
            }

            if (MySettings.AppVersion != ApplicationVersion.Express)
            {
                this.CbxListCrudRedirect.Checked = Settings.Default.IsCheckedViewListCrudRedirect;
                this.CbxAddNewRecord.Checked = Settings.Default.IsCheckedViewAddRecord;
                this.CbxUpdateRecord.Checked = Settings.Default.IsCheckedViewUpdateRecord;
                this.CbxRecordDetails.Checked = Settings.Default.IsCheckedViewRecordDetails;
                this.CbxListReadOnly.Checked = Settings.Default.IsCheckedViewListReadOnly;
                this.CbxListCrud.Checked = Settings.Default.IsCheckedViewListCrud;
                this.CbxListGroupedBy.Checked = Settings.Default.IsCheckedViewListGroupedBy;
                this.CbxListTotals.Checked = Settings.Default.IsCheckedViewListTotals;
                this.CbxListTotalsGroupedBy.Checked = Settings.Default.IsCheckedViewListTotalsGroupedBy;
                this.CbxListSearch.Checked = Settings.Default.IsCheckedViewListSearch;
                this.CbxListScrollLoad.Checked = Settings.Default.IsCheckedViewListScrollLoad;
                this.CbxListInline.Checked = Settings.Default.IsCheckedViewListInline;
                this.CbxListForeach.Checked = Settings.Default.IsCheckedViewListForeach;
                this.CbxListMasterDetailGrid.Checked = Settings.Default.IsCheckedViewListMasterDetailGrid;
                this.CbxListMasterDetailSubGrid.Checked = Settings.Default.IsCheckedViewListMasterDetailSubGrid;
                this.CbxUnbound.Checked = Settings.Default.IsCheckedViewUnbound;
                this.CbxWorkflowAssignSteps.Checked = Settings.Default.IsCheckedViewWorkflowSteps;
                this.TxtListCrudRedirect.Text = Settings.Default.NameForCheckedViewListCrudRedirect;
                this.TxtAdd.Text = Settings.Default.NameForCheckedViewAddRecord;
                this.TxtUpdate.Text = Settings.Default.NameForCheckedViewUpdateRecord;
                this.TxtDetails.Text = Settings.Default.NameForCheckedViewRecordDetails;
                this.TxtListReadOnly.Text = Settings.Default.NameForCheckedViewListReadOnly;
                this.TxtListCrud.Text = Settings.Default.NameForCheckedViewListCrud;
                this.TxtListGroupedBy.Text = Settings.Default.NameForCheckedViewListGroupedBy;
                this.TxtListTotals.Text = Settings.Default.NameForCheckedViewListTotals;
                this.TxtListTotalsGroupedBy.Text = Settings.Default.NameForCheckedViewListTotalsGroupedBy;
                this.TxtListSearch.Text = Settings.Default.NameForCheckedViewListSearch;
                this.TxtListScrollLoad.Text = Settings.Default.NameForCheckedViewListScrollLoad;
                this.TxtListInline.Text = Settings.Default.NameForCheckedViewListInline;
                this.TxtListForeach.Text = Settings.Default.NameForCheckedViewListForeach;
                this.TxtListMasterDetailGrid.Text = Settings.Default.NameForCheckedViewListMasterDetailGrid;
                this.TxtListMasterDetailSubGrid.Text = Settings.Default.NameForCheckedViewListMasterDetailSubGrid;
                this.TxtUnbound.Text = Settings.Default.NameForCheckedViewUnbound;
                this.TxtWorkflowAssignSteps.Text = Settings.Default.NameForCheckedWorkflowSteps;
            }

            this.CbxJQueryUITheme.SelectedIndex = Settings.Default.JQueryUITheme;
            this.CbxOverwriteLaunchSettingsJson.Checked = Settings.Default.IsOverwriteLaunchSettingsJson;
            this.CbxOverwriteSiteCss.Checked = Settings.Default.IsOverwriteSiteCss;
            this.CbxOverwriteFunctionsFile.Checked = Settings.Default.IsOverwriteFunctionsFile;
            this.CbxOverwriteLayoutPage.Checked = Settings.Default.IsOverwriteLayoutPage;
            this.CbxOverwriteValidationScriptsPartialView.Checked = Settings.Default.IsOverwriteValidationScriptsPartialView;
            this.CbxOverwriteViewImportsView.Checked = Settings.Default.IsOverwriteViewImportsView;
            this.CbxOverwriteViewStartPage.Checked = Settings.Default.IsOverwriteViewStartPage;
            this.CbxOverwriteAppSettingsJson.Checked = Settings.Default.IsOverwriteAppSettingsJson;
            this.CbxOverwriteBundleConfigJson.Checked = Settings.Default.IsOverwriteBundleConfigJson;
            this.CbxOverwriteProgramClass.Checked = Settings.Default.IsOverwriteProgramClassFile;
            this.CbxOverwriteStartUpClass.Checked = Settings.Default.IsOverwriteStartUpClassFile;
            this.CbxOverwriteAssemblyInfo.Checked = Settings.Default.IsOverwriteAssemblyInfo;
            this.CbxOverwriteAppSettingsClass.Checked = Settings.Default.IsOverwritProjectJsonBusLayer;
            this.CbxOverwriteLaunchSettingsJsonWebApi.Checked = Settings.Default.IsOverwriteLaunchSettingsJsonWebApi;
            this.CbxOverwriteAppSettingsJsonWebApi.Checked = Settings.Default.IsOverwriteAppSettingsWebApi;
            this.CbxOverwriteProgramClassWebApi.Checked = Settings.Default.IsOverwriteProgramClassWebApi;
            this.CbxOverwriteStartUpClassWebApi.Checked = Settings.Default.IsOverwriteStartUpClassWebApi;
            this.TxtAppFilesDirectory.Text = Settings.Default.ApplicationDirectory;
            this.CbxAutomaticallyOpenTab.Checked = Settings.Default.IsAutomaticallyOpenTab;
            this.CbxUseLogging.Checked = Settings.Default.IsUseLogging;
            this.CbxUseFileLogging.Checked = Settings.Default.IsUseFileLogging;
            this.CbxUseDatabaseLogging.Checked = Settings.Default.IsUseDatabaseLogging;
            this.CbxUseEventLogging.Checked = Settings.Default.IsUseEventLogging;
            this.CbxUseSecurity.Checked = Settings.Default.IsUseSecurity;
            this.CbxUseCaching.Checked = Settings.Default.IsUseCaching;
            this.CbxUseAuditLogging.Checked = Settings.Default.IsUseAuditLogging;

            this.CbxEmailNotification.Checked = Settings.Default.IsEmailNotification;

            this.TxtWebAppName.Enabled = Settings.Default.IsTxtWebAppNameEnabled;
            this.TxtDirectory.Enabled = Settings.Default.IsTxtDirectoryEnabled;
            this.BtnBrowseCodeDirectory.Enabled = Settings.Default.IsBtnBrowseCodeDirectoryEnabled;
            this.CbxGenerateCodeExamples.Enabled = Settings.Default.IsCbxGenerateCodeExamplesEnabled;
            this.TxtDevServerPort.Enabled = Settings.Default.IsTxtDevServerPortEnabled;
            this.TxtWebAPIName.Enabled = Settings.Default.IsTxtWebAPINameEnabled;
            this.TxtWebAPINameDirectory.Enabled = Settings.Default.IsTxtWebAPINameDirectoryEnabled;
            this.BtnBrowseWebAPIDirectory.Enabled = Settings.Default.IsBtnBrowseWebAPIDirectoryEnabled;
        }

        private void InitializeProgressBarValues()
        {
            this.progressBar.Minimum = 0;
            this.progressBar.Maximum = 1;
        }

        private void GetSerialNumberAndActivationCode()
        {
            string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\" + MySettings.SerialNumberFileName;
            if (System.IO.File.Exists(path))
            {
                try
                {
                    using (StreamReader streamReader = new StreamReader(path))
                    {
                        int num = 1;
                        string str;
                        while ((str = streamReader.ReadLine()) != null)
                        {
                            switch (num)
                            {
                                case 1:
                                    this._serialNumber = str.Replace("serial number: ", "");
                                    break;
                                case 2:
                                    this._activationCode = str.Replace("activation code: ", "");
                                    return;
                            }
                            ++num;
                        }
                    }
                }
                catch
                {
                    throw new Exception("An error occured while trying to parse file to get serial number and/or activation code.");
                }
            }
            else
            {
                int num1 = (int)MessageBox.Show("An error occured.  The file " + path + " cannot be found", "Missing File");
            }
        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            this._semicolon = this._language != Language.CSharp ? string.Empty : ";";
            this.CreateRootDirectory(worker, e);
            if (this.RbtnGenerateFromAllTables.Checked || this.RbtnGenerateFromAllViews.Checked)
                this.GetTablesOrViews(worker, e);
            else if (this.RbtnGenerateFromSelectedTables.Checked || this.RbtnGenerateFromSelectedViews.Checked)
                this.GetSelectedTablesOrViews(worker, e);
            this.Generate(worker, e);
        }

        private void CreateRootDirectory(BackgroundWorker worker, DoWorkEventArgs e)
        {
            if (worker.CancellationPending)
            {
                e.Cancel = true;
                this._isCancelled = true;
            }
            else
            {
                try
                {
                    this.ReportProgress("Creating root directory: " + this._rootDirectory);
                    Directory.CreateDirectory(this._rootDirectory);
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                    this._isCreateDirectoryError = true;
                    throw;
                }
            }
        }

        private void BackgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar.Value = e.ProgressPercentage;
            this.LblProgress.Text = (100 * this._progressCtr / this.progressBar.Maximum).ToString() + "% completed.  " + this._progressText;
            if (this.LblProgress.Text.Length <= 100)
                return;
            this.LblProgress.Text = this.LblProgress.Text.Substring(0, 100) + "...";
        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!this._isNoTableOrViewsFound)
            {
                this.progressBar.Value = this.progressBar.Maximum;
                string path = this._rootDirectory + "\\ErrorLog.txt";
                string text = !this._isCancelled ? (this._isCreateDirectoryError || e.Error != null ? "An error occured. " + e.Error.Message : (!this.RbtnGenerateFromAllTables.Checked ? (!this.RbtnGenerateFromAllViews.Checked ? (!this.RbtnGenerateFromSelectedTables.Checked ? "Razor Pages, Properties, Page Models, Business Layer, and Data Layer code for selected views have been generated in " + this._rootDirectory : "Razor Pages, Properties, Page Models, Business Layer, and Data Layer code for selected tables have been generated in " + this._rootDirectory) : "Razor Pages, Properties, Page Models, Business Layer, and Data Layer code for all views have been generated in " + this._rootDirectory) : "Razor Pages, Properties, Page Models, Business Layer, and Data Layer code for all tables have been generated in " + this._rootDirectory)) : "Code generation was cancelled.";
                if (System.IO.File.Exists(path) && !this._isCancelled)
                    text = text + "  Some errors were encountered during code generation.  Please look at the " + path + " file";
                this.LblProgress.Text = "100% Completed.  Code Generation is completed.";
                if (MessageBox.Show(text, MySettings.AppTitle) == DialogResult.OK)
                {
                    this.LblProgress.Text = string.Empty;
                    this.progressBar.Value = 0;
                    this.BtnGenerateCode.Enabled = true;
                    this.AcceptButton = BtnGenerateCode;
                }
            }
            this._isCancelled = false;
            this._isCreateDirectoryError = false;
            this._selectedTables = null;
            this._referencedTables = null;
            this._progressCtr = 0;
            this._isNoTableOrViewsFound = false;
            this.TabControl1.Enabled = true;
            this.BtnCancel.Enabled = false;
            this.ControlBox = true;
            this.BtnClose.Enabled = true;
        }

        private void GetTablesOrViews(BackgroundWorker worker, DoWorkEventArgs e)
        {
            if (this._dsAllTables != null)
            {
                this._tables = new Tables();
                if (this._dsAllTables.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in (InternalDataCollectionBase)this._dsAllTables.Tables[0].Rows)
                    {
                        if (worker.CancellationPending)
                        {
                            e.Cancel = true;
                            this._isCancelled = true;
                        }
                        else
                            this.Do1stPass(row);
                    }
                    this.Do2ndPass();
                }
                else
                    this.ShowNoTableOrViewFoundError(e);
            }
            else
                this.ShowNoTableOrViewFoundError(e);
        }

        private void ShowNoTableOrViewFoundError(DoWorkEventArgs e)
        {
            this._isNoTableOrViewsFound = true;
            if (this._generateFrom == DatabaseObjectToGenerateFrom.Tables)
            {
                int num1 = (int)MessageBox.Show("No tables were found!  Please make sure you have tables in the " + this.TxtDatabase.Text + " database.  Also make sure that your table(s) have Primary Keys.", MySettings.AppTitle);
            }
            else
            {
                int num2 = (int)MessageBox.Show("No views were found!  Please make sure you have tables in the " + this.TxtDatabase.Text + " database.", MySettings.AppTitle);
            }
            if (e == null)
            {
                this.BtnGenerateCode.Enabled = true;
                this.AcceptButton = BtnGenerateCode;
                this.BtnCancel.Enabled = false;
                this.LblProgress.Text = string.Empty;
                this.progressBar.Value = 0;
                this._isCancelled = false;
                this._isCreateDirectoryError = false;
                this._selectedTables = null;
                this._referencedTables = null;
                this._progressCtr = 0;
                this.TabControl1.Enabled = true;
            }
            else
                e.Cancel = true;
        }

        private void GetSelectedTablesOrViews(BackgroundWorker worker, DoWorkEventArgs e)
        {
            if (this._dsAllTables != null)
            {
                this._tables = new Tables();
                CheckedListBox checkedListBox = this.ClbxTables;
                if (this._generateFrom == DatabaseObjectToGenerateFrom.Views)
                    checkedListBox = this.ClbxViews;
                //checkedListBox = AddWorkflowTables(checkedListBox);
                foreach (object checkedItem in checkedListBox.CheckedItems)
                {
                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        this._isCancelled = true;
                    }
                    else
                        this.Do1stPass(((IEnumerable<DataRow>)this._dsAllTables.Tables[0].Select("TABLE_OWNER + '.' + TABLE_NAME = '" + checkedItem.ToString() + "'")).Single<DataRow>());
                }
                this.Do2ndPass();
            }
            else
                this.ShowNoTableOrViewFoundError(e);
        }

        private bool Do1stPass(DataRow dr)
        {
            string lower1 = dr["TABLE_NAME"].ToString().Trim().ToLower();
            string lower2 = dr["TABLE_OWNER"].ToString().Trim().ToLower();
            if (lower1 != "sysdiagrams" && lower1 != "dtproperties" && (lower2 != "sys" && lower2 != "information_schema"))
            {
                Table table = new Table(dr, this._connectionString, this._language, this.TxtDirectory.Text.Trim(), true, this.TxtAPIName.Text.Trim());
                if (this._generateFrom == DatabaseObjectToGenerateFrom.Tables && (table.PrimaryKeyCount == 0 || string.IsNullOrEmpty(table.Name)) || this._generateFrom == DatabaseObjectToGenerateFrom.Views && string.IsNullOrEmpty(table.Name))
                    return true;
                if (table.Columns.Count > 1 && !table.IsContainsPrimaryAndForeignKeyColumnsOnly && !table.IsContainsBinaryOrSpatialDataTypeColumnsOnly)
                {
                    this.ReportProgress("Loading " + table.OwnerOriginal + "." + table.NameOriginal);
                    this._tables.Add(table);
                }
            }
            return false;
        }

        private void Do2ndPass()
        {
            if (this._tables.Count <= 0)
                return;
            foreach (Table table in _tables)
            {
                bool flag1 = false;
                bool flag2 = false;
                List<string> source = new List<string>();
                foreach (Column column in table.Columns)
                {
                    if (column.IsForeignKey)
                    {
                        Table foreignTable = this.GetForeignTable(table, column, this._tables);
                        if (foreignTable != null && foreignTable.PrimaryKeyCount == 1)
                        {
                            column.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK = true;
                            column.ForeignKeyTable = foreignTable;
                            column.DropDownListDataPropertyName = column.ForeignKeyTableName + MyConstants.WordDropDownListData;
                            source.Add(column.ForeignKeyTableName);
                            table.IsContainsForeignKeysWithTableSelected = true;
                        }
                        else
                        {
                            column.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK = false;
                            column.ForeignKeyTable = null;
                            column.WebControlFieldAssignment = Functions.GetWebControlFieldAssignment(table, column, this._language);
                        }
                        if (column.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK)
                        {
                            column.WinControlType = WinControlType.ComboBox;
                            column.WinControlTypeString = "ComboBox";
                            column.WebControlID = "Cbx" + column.Name;
                            column.WebControllValue = column.WebControlID + ".SelectedValue";
                        }
                        else
                        {
                            string str = column.NameCamelStyle;
                            if (this._language == Language.VB && column.Name.ToLower() == table.Name.ToLower())
                                str = column.NameCamelStyle + "1";
                            column.WinControlType = WinControlType.TextBox;
                            column.WinControlTypeString = "TextBox";
                            column.WebControlID = "Txt" + column.Name;
                            column.WebControllValue = column.WebControlID + ".Text";
                            column.WebControlFieldAssignmentReverse = column.WebControllValue + " = obj" + table.Name + "." + column.Name + ".ToString()" + this._semicolon;
                            column.VariableFieldAssignment = str + " = Convert.To" + column.SystemTypeNative + "(" + column.WebControllValue + ".ToString())" + this._semicolon;
                        }
                    }
                    column.SearchControlID = column.SQLDataType == SQLType.bit || column.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK ? "Ddl" + column.Name : column.WebControlID;
                    if (column.Name == table.Name && table.Name == table.NameFullyQualifiedBusinessObject)
                        flag2 = true;
                    if (column.IsPrimaryKey && string.IsNullOrEmpty(table.PrimaryKeyName))
                        table.PrimaryKeyName = column.PrimaryKeyName;
                    if (column.IsNullable && !column.IsPrimaryKeyUnique && !column.IsForeignKey && (column.SQLDataType == SQLType.integer || column.SQLDataType == SQLType.bigint || (column.SQLDataType == SQLType.smallint || column.SQLDataType == SQLType.tinyint)))
                        flag1 = true;
                }
                if (source != null)
                {
                    foreach (Column column1 in table.Columns.Where<Column>(c => c.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK))
                    {
                        Column column = column1;
                        if (source.Where<string>(c => c == column.ForeignKeyTableName).Count<string>() > 1)
                            column.IsForeignKeyWithTheSameReferencingTable = true;
                    }
                }
                if (flag2)
                    table.NameFullyQualifiedBusinessObject = this._language != Language.CSharp ? "BusinessObject." + table.Name : this.TxtAPIName.Text.Trim() + ".BusinessObject." + table.Name;
                table.IsContainsHiddenColumns = flag1;
                table.ForeignKeyColumns = table.Columns.Where<Column>(c => c.IsForeignKey);
                table.ForeignKeyColumnsTableIsSelectedAndOnlyOnePK = table.Columns.Where<Column>(c => c.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK);
                table.NoneBinaryOrSpatialDataTypeColumns = table.Columns.Where<Column>(c => !c.IsBinaryOrSpatialDataType);
                table.ColumnsWithDropDownListData = table.Columns.Where<Column>(c =>
               {
                   if (c.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK)
                       return !string.IsNullOrEmpty(c.DropDownListDataPropertyName);
                   return false;
               });
            }
        }

        private void Generate(BackgroundWorker worker, DoWorkEventArgs e)
        {
            this._selectedTables = this._tables;
            this._isUseStoredProc = this.RbtnUseStoredProc.Checked;
            this._spPrefix = this.TxtSpPrefix.Text.Trim();
            this._spSuffix = this.TxtSpSuffix.Text.Trim();
            this._referencedTables = new Dbase(this._connectionString).GetReferencedTables(this.TxtDatabase.Text.Trim());
            string strWebAPILaunchPort = Convert.ToString(Convert.ToInt32(this.TxtDevServerPort.Text.Trim() != "" ? this.TxtDevServerPort.Text : "0") + 2);

            if (this._selectedTables.Count > 0)
            {
                this.CreateDirectories();
                this._webApiBaseAddress = "http://localhost:" + strWebAPILaunchPort + "/";

                foreach (Table selectedTable in _selectedTables)
                {
                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        this._isCancelled = true;
                    }
                    else
                    {
                        this.ReportProgress("Generating code for " + selectedTable.OwnerOriginal + "." + selectedTable.NameOriginal);
                        this.GenerateFiles(selectedTable);
                    }
                }

                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    this._isCancelled = true;
                }
                else
                {
                    if (this._selectedTables.Count <= 0)
                        return;
                    this.ReportProgress("Starting AspCoreGen code generator");
                    this.ReportProgress("Copying src Web App files");
                    this.ReportProgress("Copying Web App wwwroot files");
                    if (this.CbxUseWebApplication.Checked)
                        Functions.CopyFolder(this.TxtAppFilesDirectory.Text.Trim() + "wwwrootWebApp\\", this._rootDirectory + "\\" + this.TxtWebAppName.Text.Trim() + "\\wwwroot");
                    this.ReportProgress("Testing code 4");
                    this.ReportProgress("Testing code 5");
                    this.ReportProgress("Testing code 6");
                    string upper1 = Guid.NewGuid().ToString().ToUpper();
                    string upper2 = Guid.NewGuid().ToString().ToUpper();
                    string upper3 = Guid.NewGuid().ToString().ToUpper();
                    string fullFileNamePath1 = this._webAppRootDirectory + this.TxtWebAppName.Text.Trim() + this._fileExtension + "proj";
                    string fullFileNamePath2 = this.TxtAPINameDirectory.Text + "\\" + this.TxtAPIName.Text.Trim() + this._fileExtension + "proj";
                    string fullFileNamePath3 = this._webAPIRootDirectory + "\\" + this.TxtWebAPIName.Text.Trim() + this._fileExtension + "proj";
                    string fullFileNamePath4 = this._rootDirectory + "\\" + this.TxtWebAppName.Text.Trim() + MyConstants.WordDotSln;
                    string str1 = this._rootDirectory + "\\global.json";
                    string destFileName1 = this._webAppRootDirectory + "\\appsettings.json";
                    string destFileName2 = this._webAppRootDirectory + "\\appsettings.Development.json";
                    string str2 = this._webAppRootDirectory + "\\Properties\\launchSettings.json";
                    string str3 = this._webAppRootDirectory + "\\wwwroot\\css\\site.css";
                    string str4 = this._webAppRootDirectory + "\\Pages\\_Layout" + this._fileExtension + "html";
                    string str5 = this._webAppRootDirectory + "\\Pages\\_ValidationScriptsPartial" + this._fileExtension + "html";
                    string str6 = this._webAppRootDirectory + "\\Pages\\_ViewStart" + this._fileExtension + "html";
                    string str7 = this._webAppRootDirectory + "\\Pages\\_ViewImports" + this._fileExtension + "html";
                    string str8 = this._webAppRootDirectory + MyConstants.DirectoryHelper + MyConstants.WordFunctions + this._fileExtension;
                    string str9 = this._webAppRootDirectory + "\\" + MyConstants.WordBundleConfigDotJson;
                    string path1 = this._webAppRootDirectory + "\\Program" + this._fileExtension;
                    string str10 = this._webAppRootDirectory + "\\StartUp" + this._fileExtension;
                    string str11 = this.TxtAPINameDirectory.Text.Trim() + "\\packages.config";
                    string str12 = this.TxtAPINameDirectory.Text.Trim() + "\\Properties\\" + MyConstants.WordAssemblyInfo + this._fileExtension;
                    string fullFileNamePath5 = this.TxtAPINameDirectory.Text.Trim() + "\\Domain\\" + MyConstants.WordCrudOperation + this._fileExtension;
                    string destFileName3 = this._webAPIRootDirectory + "\\appsettings.json";
                    string destFileName4 = this._webAPIRootDirectory + "\\appsettings.Development.json";
                    string str13 = this._webAPIRootDirectory + "\\Properties\\launchSettings.json";
                    string path2 = this._webAPIRootDirectory + "\\Program" + this._fileExtension;
                    string str14 = this._webAPIRootDirectory + "\\StartUp" + this._fileExtension;
                    string fullFileNamePath6 = this.TxtAPINameDirectory.Text.Trim() + "\\AppSettings" + this._fileExtension;
                    string srcLogDllPath = Application.StartupPath + "\\Application_Components.Logging.dll";
                    string srcSecurityDllPath = Application.StartupPath + "\\Application_Components.Security.dll";
                    string srcCachingDllPath = Application.StartupPath + "\\Application_Components.Caching.dll";
                    string srcAuditLogDllPath = Application.StartupPath + "\\Application_Components.AuditLog.dll";
                    string srcEmailNotificationDllPath = Application.StartupPath + "\\Application_Components.EmailNotification.dll";
                    string dstWebAppLogDllPath = this._webAppRootDirectory + "\\External Dlls\\Application_Components.Logging.dll";
                    string dstWebAPILogDllPath = this._webAPIRootDirectory + "\\External Dlls\\Application_Components.Logging.dll";
                    string dstWebAppSecurityDllPath = this._webAppRootDirectory + "\\External Dlls\\Application_Components.Security.dll";
                    string dstWebAPISecurityDllPath = this._webAPIRootDirectory + "\\External Dlls\\Application_Components.Security.dll";
                    string dstWebAppCachingDllPath = this._webAppRootDirectory + "\\External Dlls\\Application_Components.Caching.dll";
                    string dstWebAPICachingDllPath = this._webAPIRootDirectory + "\\External Dlls\\Application_Components.Caching.dll";
                    string dstWebAppAuditLogDllPath = this._webAppRootDirectory + "\\External Dlls\\Application_Components.AuditLog.dll";
                    string dstWebAPIAuditLogDllPath = this._webAPIRootDirectory + "\\External Dlls\\Application_Components.AuditLog.dll";
                    string dstWebAppEmailNotificationDllPath = this._webAppRootDirectory + "\\External Dlls\\Application_Components.EmailNotification.dll";
                    string dstWebAPIEmailNotificationDllPath = this._webAPIRootDirectory + "\\External Dlls\\Application_Components.EmailNotification.dll";
                    string dstWebAppNlogPath = this._webAppRootDirectory + "\\" + MyConstants.WordNlogDotConfig;
                    string dstWebAPINlogPath = this._webAPIRootDirectory + "\\" + MyConstants.WordNlogDotConfig;
                    string WebAppLogFilePath = this._webAppRootDirectory + "\\bin\\debug\\netcoreapp2.1\\Log\\logs.log";
                    string WebAPILogFilePath = this._webAPIRootDirectory + "\\bin\\debug\\netcoreapp2.1\\Log\\logs.log";

                    if (this.CbxUseWebApplication.Checked)
                    {
                        this.ReportProgress("Generating " + this.TxtWebAppName.Text.Trim() + this._fileExtension + "proj Web App Project File");
                        ProjectFile projectFile1 = new ProjectFile(fullFileNamePath1, ProjectFileType.WebApp, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), this.TxtWebAPIName.Text.Trim(), this.CbxUseWebApi.Checked, this._generatedSqlType, this.CbxUseLogging.Checked, this.CbxUseSecurity.Checked, this.CbxUseCaching.Checked, this.CbxUseAuditLogging.Checked, this.CbxEmailNotification.Checked);

                        if (this.CbxUseLogging.Checked)
                        {
                            System.IO.File.Copy(srcLogDllPath, dstWebAppLogDllPath, true);
                            this.ReportProgress("Generating " + MyConstants.WordNlogDotConfig + " Web App Log File");
                            NlogConfig nlogWebApp = new NlogConfig(dstWebAppNlogPath, this._isUseFileLogging, this._isUseDatabaseLogging, this._isUseEventLogging, WebAppLogFilePath, this._connectionString, this.TxtWebAppName.Text);
                        }
                        if (this.CbxUseSecurity.Checked)
                        {
                            System.IO.File.Copy(srcSecurityDllPath, dstWebAppSecurityDllPath, true);
                        }
                        if (this.CbxUseCaching.Checked)
                        {
                            System.IO.File.Copy(srcCachingDllPath, dstWebAppCachingDllPath, true);
                        }
                        if (this.CbxUseAuditLogging.Checked)
                        {
                            System.IO.File.Copy(srcAuditLogDllPath, dstWebAppAuditLogDllPath, true);
                        }
                        if (this.CbxEmailNotification.Checked)
                        {
                            System.IO.File.Copy(srcEmailNotificationDllPath, dstWebAppEmailNotificationDllPath, true);
                        }
                    }

                    this.ReportProgress("Generating " + this.TxtAPINameDirectory.Text.Trim() + this._fileExtension + "proj Business/Data Layer API Project File");
                    ProjectFile projectFile2 = new ProjectFile(fullFileNamePath2, ProjectFileType.BusinessAndDataAPI, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), this.TxtWebAPIName.Text.Trim(), this.CbxUseWebApi.Checked, this._generatedSqlType, this.CbxUseLogging.Checked, this.CbxUseSecurity.Checked, this.CbxUseCaching.Checked, this.CbxUseAuditLogging.Checked, this.CbxEmailNotification.Checked);

                    if (this.CbxUseWebApi.Checked)
                    {
                        this.ReportProgress("Generating " + this._webAPIRootDirectory + ".xproj Web API Project File");
                        ProjectFile projectFile3 = new ProjectFile(fullFileNamePath3, ProjectFileType.WebAPI, this.TxtWebAPIName.Text.Trim(), this.TxtAPIName.Text.Trim(), this.TxtWebAPIName.Text.Trim(), this.CbxUseWebApi.Checked, this._generatedSqlType, this.CbxUseLogging.Checked, this.CbxUseSecurity.Checked, this.CbxUseCaching.Checked, this.CbxUseAuditLogging.Checked, this.CbxEmailNotification.Checked);

                        if (this.CbxUseLogging.Checked)
                        {
                            System.IO.File.Copy(srcLogDllPath, dstWebAPILogDllPath, true);
                            this.ReportProgress("Generating " + MyConstants.WordNlogDotConfig + " Web App Log File");
                            NlogConfig nlogWebApp = new NlogConfig(dstWebAPINlogPath, this._isUseFileLogging, this._isUseDatabaseLogging, this._isUseEventLogging, WebAPILogFilePath, this._connectionString, this.TxtWebAPIName.Text);
                        }
                        if (this.CbxUseSecurity.Checked)
                        {
                            System.IO.File.Copy(srcSecurityDllPath, dstWebAPISecurityDllPath, true);
                        }
                        if (this.CbxUseCaching.Checked)
                        {
                            System.IO.File.Copy(srcCachingDllPath, dstWebAPICachingDllPath, true);
                        }
                        if (this.CbxUseAuditLogging.Checked)
                        {
                            System.IO.File.Copy(srcAuditLogDllPath, dstWebAPIAuditLogDllPath, true);
                        }
                        if (this.CbxEmailNotification.Checked)
                        {
                            System.IO.File.Copy(srcEmailNotificationDllPath, dstWebAPIEmailNotificationDllPath, true);
                        }
                    }
                    else
                        this.ReportProgress("Skipping Web API Project File");

                    if (this.CbxUseWebApplication.Checked)
                    {
                        this.ReportProgress("Generating Web Application Solution File");
                        string webAppName = this.TxtWebAppName.Text.Trim();
                        string webAppProjectGuid = upper1;
                        string apiName = this.TxtAPIName.Text.Trim();
                        string apiProjectGuid = upper2;
                        string webApiName = this.TxtWebAPIName.Text.Trim();
                        string webApiProjectGuid = upper3;
                        int language = (int)this._language;

                        SolutionFile solutionFile = new SolutionFile(fullFileNamePath4, this.CbxUseWebApplication.Checked, webAppName, webAppProjectGuid, apiName, apiProjectGuid, this.CbxUseWebApi.Checked, webApiName, webApiProjectGuid, (Language)language);

                        this.ReportProgress("Generating launchSettings.json file");

                        try
                        {
                            System.IO.File.Copy(this.TxtAppFilesDirectory.Text.Trim() + "srcWebAppFiles\\appsettings.json", destFileName1, true);
                            System.IO.File.Copy(this.TxtAppFilesDirectory.Text.Trim() + "srcWebAppFiles\\appsettings.Development.json", destFileName2, true);
                        }
                        catch
                        {
                        }

                        if (!System.IO.File.Exists(str2) || System.IO.File.Exists(str2) && this._isOverwriteLaunchSettingsJson)
                        {
                            this.ReportProgress("Generating launchSettings.json file");
                            try
                            {
                                System.IO.File.Copy(this.TxtAppFilesDirectory.Text.Trim() + "srcWebAppFiles\\Properties\\launchSettings.json", str2, true);
                                Functions.ReplaceLaunchPortAndCreateFile(str2, this.TxtDevServerPort.Text.Trim(), this.TxtWebAppName.Text.Trim(), this.TxtWebAPIName.Text.Trim());
                            }
                            catch { }
                        }
                        else
                            this.ReportProgress("Skipping launchSettings.json file");

                        if (!System.IO.File.Exists(str3) || System.IO.File.Exists(str3) && this._isOverwriteSiteCss)
                        {
                            this.ReportProgress("Generating site.css file");
                            try
                            {
                                System.IO.File.Copy(this.TxtAppFilesDirectory.Text.Trim() + "srcWebAppFiles\\site.css", str3, true);
                            }
                            catch { }
                        }
                        else
                            this.ReportProgress("Skipping site.css file");

                        if (!System.IO.File.Exists(str4) || System.IO.File.Exists(str4) && this._isOverwriteLayoutView)
                        {
                            this.ReportProgress("Generating Layout View");
                            LayoutFile layoutFile = new LayoutFile(str4, this.TxtWebAppName.Text.Trim(), this._selectedJQueryUITheme);
                        }
                        else
                            this.ReportProgress("Skipping _Layout View");

                        if (!System.IO.File.Exists(str5) || System.IO.File.Exists(str5) && this._isOverwriteValidationScriptsPartialView)
                        {
                            this.ReportProgress("Generating _ValidationScriptsPartial View");
                            ValidationScriptsPartialView scriptsPartialView = new ValidationScriptsPartialView(str5);
                        }
                        else
                            this.ReportProgress("Skipping _ValidationScriptsPartial View");

                        if (!System.IO.File.Exists(str6) || System.IO.File.Exists(str6) && this._isOverwriteViewStartPage)
                        {
                            this.ReportProgress("Generating _ViewStart View");
                            ViewStartFile viewStartFile = new ViewStartFile(str6, this._language);
                        }
                        else
                            this.ReportProgress("Skipping _ViewStart View");

                        if (!System.IO.File.Exists(str7) || System.IO.File.Exists(str7) && this._isOverwriteViewImportsView)
                        {
                            this.ReportProgress("Generating _ViewImports View");
                            ViewImportsFile viewImportsFile = new ViewImportsFile(str7, this.TxtWebAppName.Text.Trim(), this.TxtWebAPIName.Text.Trim(), true, this._language);
                        }
                        else
                            this.ReportProgress("Skipping _ViewImports View");

                        if ((!System.IO.File.Exists(str8) || System.IO.File.Exists(str8) && this._isOverwriteFunctionsFile) && MySettings.AppVersion != ApplicationVersion.Express)
                        {
                            this.ReportProgress("Generating Functions" + this._fileExtension + " file");
                            FunctionsFile functionsFile = new FunctionsFile(str8, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), this._language, this._webApiBaseAddress);
                        }
                        else
                            this.ReportProgress("Skipping Functions" + this._fileExtension + " file");

                        if (!System.IO.File.Exists(str9) || System.IO.File.Exists(str9) && this._isOverwriteBundleConfigJson)
                        {
                            this.ReportProgress("Generating " + MyConstants.WordBundleConfigDotJson + " file");
                            BundleConfig bundleConfig = new BundleConfig(str9);
                        }
                        else
                            this.ReportProgress("Skipping BundleConfig" + this._fileExtension + " file");

                        if (!System.IO.File.Exists(path1) || System.IO.File.Exists(path1) && this._isOverwriteProgramClass)
                        {
                            this.ReportProgress("Generating Program" + this._fileExtension);
                            ProgramFile programFile = new ProgramFile(this._webAppRootDirectory, this.TxtWebAppName.Text.Trim());
                        }
                        else
                            this.ReportProgress("Skipping Program" + this._fileExtension);

                        if (!System.IO.File.Exists(str10) || System.IO.File.Exists(str10) && this._isOverwriteStartUpClassFile)
                        {
                            this.ReportProgress("Generating Web App StartUp" + this._fileExtension);
                            StartUp startUp = new StartUp(str10, this.TxtWebAppName.Text.Trim(), this.TxtWebAPIName.Text, StartUpType.WebApp, this._selectedTables, this.CbxUseLogging.Checked, this.CbxUseCaching.Checked, this.CbxUseAuditLogging.Checked, false, this.CbxEmailNotification.Checked);
                        }
                        else
                            this.ReportProgress("Skipping Web App StartUp" + this._fileExtension);

                        this.ReportProgress("Testing code 22");
                        this.ReportProgress("Skipping packages.config file");
                        this.ReportProgress("Skipping AssemblyInfo" + this._fileExtension);
                    }
                    else if (!this.CbxUseWebApplication.Checked && this.CbxUseWebApi.Checked)
                    {
                        this.ReportProgress("Generating Web API Solution File");
                        string apiName = this.TxtAPIName.Text.Trim();
                        string apiProjectGuid = upper2;
                        string webApiName = this.TxtWebAPIName.Text.Trim();
                        string webApiProjectGuid = upper3;
                        int language = (int)this._language;

                        SolutionFile solutionFile = new SolutionFile(fullFileNamePath4, this.CbxUseWebApplication.Checked, string.Empty, string.Empty, apiName, apiProjectGuid, this.CbxUseWebApi.Checked, webApiName, webApiProjectGuid, (Language)language);
                    }

                    this.ReportProgress("Generating CrudOperation" + this._fileExtension);
                    CrudOperationFile crudOperationFile = new CrudOperationFile(fullFileNamePath5, this._language, this.TxtAPIName.Text.Trim());
                    this.ReportProgress("Skipping FieldType" + this._fileExtension);

                    if (this.CbxUseWebApi.Checked)
                    {
                        this.ReportProgress("Generating appsettings.json file for Web API");

                        try
                        {
                            System.IO.File.Copy(this.TxtAppFilesDirectory.Text.Trim() + "srcWebApiFiles\\appsettings.json", destFileName3, true);
                            System.IO.File.Copy(this.TxtAppFilesDirectory.Text.Trim() + "srcWebApiFiles\\appsettings.Development.json", destFileName4, true);
                        }
                        catch { }

                        if (!System.IO.File.Exists(str13) || System.IO.File.Exists(str13) && this._isOverwriteLaunchSettingsJsonWebApi)
                        {
                            this.ReportProgress("Generating Web API launchSettings.json file");
                            try
                            {
                                System.IO.File.Copy(this.TxtAppFilesDirectory.Text.Trim() + "srcWebApiFiles\\Properties\\launchSettings.json", str13, true);
                                Functions.ReplaceLaunchPortAndCreateFile(str13, strWebAPILaunchPort, this.TxtWebAppName.Text.Trim(), this.TxtWebAPIName.Text.Trim());
                            }
                            catch { }

                        }
                        else
                            this.ReportProgress("Skipping Web API launchSettings.json file");

                        if (!System.IO.File.Exists(path2) || System.IO.File.Exists(path2) && this._isOverwriteProgramClassWebApi)
                        {
                            this.ReportProgress("Generating Web API Program" + this._fileExtension);
                            ProgramFile programFile = new ProgramFile(this._webAPIRootDirectory, this.TxtWebAPIName.Text.Trim());
                        }
                        else
                            this.ReportProgress("Skipping Web API Program" + this._fileExtension);
                    }
                    else
                    {
                        this.ReportProgress("Skipping Web API launchSettings.json file");
                        this.ReportProgress("Skipping Web API appsettings.json");
                        this.ReportProgress("Skipping Web API Program" + this._fileExtension);
                    }

                    if (this.CbxUseWebApi.Checked)
                    {
                        if (!System.IO.File.Exists(str14) || System.IO.File.Exists(str14) && this._isOverwriteStartUpClassFileWebApi)
                        {
                            this.ReportProgress("Generating Web API StartUp" + this._fileExtension);
                            StartUp startUp = new StartUp(str14, this.TxtWebAppName.Text.Trim(), this.TxtWebAPIName.Text.Trim(), StartUpType.WebAPI, this._selectedTables, this.CbxUseLogging.Checked, this.CbxUseCaching.Checked, this.CbxUseAuditLogging.Checked, true, this.CbxEmailNotification.Checked);
                        }
                        else
                            this.ReportProgress("Skipping Web API StartUp" + this._fileExtension);
                    }
                    else
                        this.ReportProgress("Skipping Web API StartUp" + this._fileExtension);

                    if (this.CbxUseWebApplication.Checked)
                    {
                        this.ReportProgress("Generating Home Index Razor Page");
                        HomeIndexPage homeIndexPage = new HomeIndexPage(this._selectedTables, this._webAppRootDirectory, this._language, this.TxtWebAppName.Text.Trim(), this._generateFrom, MySettings.AppVersion, this._isUseStoredProc, this.GetIsCheckedWebFormAssignMents(), this.GetViewNames(), this.CbxUseWebApi.Checked, this._generatedSqlType, this._spPrefixSuffixIndex, this._spPrefix, this._spSuffix, this.TxtDatabase.Text.Trim());

                        this.ReportProgress("Generating Home Index Razor Page Model");
                        HomeIndexPageModel homeIndexPageModel = new HomeIndexPageModel(this._webAppRootDirectory, this._language, this.TxtWebAppName.Text.Trim());
                    }

                    if (this._generatedSqlType == GeneratedSqlType.EFCore && MySettings.AppVersion == ApplicationVersion.ProfessionalPlus)
                    {
                        this.ReportProgress("Generating Entity Framework context file");
                        EfDbContext efDbContext = new EfDbContext(this._selectedTables, this._referencedTables, this.TxtAPIName.Text.Trim(), this.TxtAPINameDirectory.Text.Trim(), this._isUseStoredProc, this._spPrefixSuffixIndex, this._spPrefix, this._spSuffix, this._generateFrom, this.TxtServer.Text.Trim(), this.TxtDatabase.Text.Trim(), this.TxtUserName.Text, this.TxtPassword.Text.Trim());
                    }
                    else
                        this.ReportProgress("Skipping Entity Framework context file");

                    if (this._generatedSqlType == GeneratedSqlType.StoredProcedures || this._generatedSqlType == GeneratedSqlType.AdHocSQL)
                    {
                        this.ReportProgress("Generating AppSetting" + this._fileExtension + " file");
                        AppSettingsClass appSettingsClass = new AppSettingsClass(fullFileNamePath6, this.TxtAPIName.Text.Trim(), this.TxtServer.Text.Trim(), this.TxtDatabase.Text.Trim(), this.TxtUserName.Text, this.TxtPassword.Text.Trim());
                        this.ReportProgress("Generating app.config file");
                        this.ReportProgress("Generating packages folder files");
                    }
                    else
                    {
                        this.ReportProgress("Skipping AppSetting" + this._fileExtension + " file");
                        this.ReportProgress("Skipping app.config file");
                        this.ReportProgress("Skipping packages folder files");
                    }
                }
            }
            else
                this.ShowNoTableOrViewFoundError(e);
        }

        private void ReportProgress(string progressText)
        {
            ++this._progressCtr;
            this.BackgroundWorker1.ReportProgress(this._progressCtr);
            this._progressText = progressText;
        }

        private void CreateDirectories()
        {
            try
            {
                if (this.CbxUseWebApplication.Checked)
                {
                    Directory.CreateDirectory(this._rootDirectory + "\\" + this.TxtWebAppName.Text.Trim());
                    Directory.CreateDirectory(this._rootDirectory + "\\" + this.TxtWebAppName.Text.Trim() + "\\Helper");
                    Directory.CreateDirectory(this._rootDirectory + "\\" + this.TxtWebAppName.Text.Trim() + "\\Properties");
                    Directory.CreateDirectory(this._rootDirectory + "\\" + this.TxtWebAppName.Text.Trim() + "\\Pages");
                    Directory.CreateDirectory(this._rootDirectory + "\\" + this.TxtWebAppName.Text.Trim() + "\\Pages\\Shared");
                    Directory.CreateDirectory(this._rootDirectory + "\\" + this.TxtWebAppName.Text.Trim() + "\\PartialModels");
                    Directory.CreateDirectory(this._rootDirectory + "\\" + this.TxtWebAppName.Text.Trim() + "\\wwwroot");
                    Directory.CreateDirectory(this._rootDirectory + "\\" + this.TxtWebAppName.Text.Trim() + "\\wwwroot\\css");
                    Directory.CreateDirectory(this._rootDirectory + "\\" + this.TxtWebAppName.Text.Trim() + "\\wwwroot\\images");
                    Directory.CreateDirectory(this._rootDirectory + "\\" + this.TxtWebAppName.Text.Trim() + "\\wwwroot\\js");
                    Directory.CreateDirectory(this._rootDirectory + "\\" + this.TxtWebAppName.Text.Trim() + "\\wwwroot\\lib");
                    Directory.CreateDirectory(this._rootDirectory + "\\" + this.TxtWebAppName.Text.Trim() + "\\External Dlls");

                    if (this.CbxGenerateCodeExamples.Checked)
                        Directory.CreateDirectory(this._rootDirectory + "\\" + this.TxtWebAppName.Text.Trim() + "\\" + MyConstants.DirectoryExamples);

                    foreach (Table selectedTable in _selectedTables)
                        Directory.CreateDirectory(this._rootDirectory + "\\" + this.TxtWebAppName.Text.Trim() + "\\Pages\\" + selectedTable.Name);

                    if (this._isCheckedViewListForeach)
                    {
                        Directory.CreateDirectory(this._rootDirectory + "\\" + this.TxtWebAppName.Text.Trim() + "\\PageModels\\Foreach");
                        Directory.CreateDirectory(this._rootDirectory + "\\" + this.TxtWebAppName.Text.Trim() + "\\PageModels\\Foreach\\Base");
                    }
                }

                Directory.CreateDirectory(this.TxtAPINameDirectory.Text.Trim());
                Directory.CreateDirectory(this.TxtAPINameDirectory.Text.Trim() + "\\" + MyConstants.DirectoryBusinessObject);
                Directory.CreateDirectory(this.TxtAPINameDirectory.Text.Trim() + "\\" + MyConstants.DirectoryBusinessObject + "\\Base");
                Directory.CreateDirectory(this.TxtAPINameDirectory.Text.Trim() + "\\" + MyConstants.DirectoryDataLayer);
                Directory.CreateDirectory(this.TxtAPINameDirectory.Text.Trim() + "\\" + MyConstants.DirectoryDataLayer + "\\Base");
                Directory.CreateDirectory(this.TxtAPINameDirectory.Text.Trim() + "\\Models");
                Directory.CreateDirectory(this.TxtAPINameDirectory.Text.Trim() + "\\Models\\Base");
                Directory.CreateDirectory(this.TxtAPINameDirectory.Text.Trim() + "\\Domain");

                if (this._generatedSqlType == GeneratedSqlType.EFCore)
                    Directory.CreateDirectory(this.TxtAPINameDirectory.Text.Trim() + "\\EF");

                if (this.RbtnUseAdHocSql.Checked)
                    Directory.CreateDirectory(this.TxtAPINameDirectory.Text.Trim() + "\\SQL");

                if (!this.CbxUseWebApi.Checked)
                    return;

                Directory.CreateDirectory(this._webAPIRootDirectory);
                Directory.CreateDirectory(this._webAPIRootDirectory + "\\Controllers");
                Directory.CreateDirectory(this._webAPIRootDirectory + "\\Controllers\\Base");
                Directory.CreateDirectory(this._webAPIRootDirectory + "\\Properties");
                Directory.CreateDirectory(this._webAPIRootDirectory + "\\External Dlls");
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                this._isCreateDirectoryError = true;
                throw;
            }
        }

        private void GenerateFiles(Table table)
        {
            IsCheckedView webFormAssignMents = this.GetIsCheckedWebFormAssignMents();
            ViewNames viewNames = this.GetViewNames();
            DataLayer dataLayer = new DataLayer(table, this.TxtAPIName.Text.Trim(), this.TxtAPINameDirectory.Text.Trim());
            BusinessObjectBase businessObjectBase = new BusinessObjectBase(table, this._selectedTables, this._referencedTables, this.TxtAPIName.Text.Trim(), this.TxtAPINameDirectory.Text.Trim(), this._generateFrom, this._isUseStoredProc, MySettings.AppVersion, this._generatedSqlType, BusinessObjectBaseType.BusinessObjectBase);
            BusinessObject businessObject = new BusinessObject(table, this.TxtAPIName.Text.Trim(), this.TxtAPINameDirectory.Text.Trim());

            if (this.CbxUseWebApplication.Checked && this.CbxGenerateCodeExamples.Checked)
            {
                CodeExample codeExample = new CodeExample(table, this._selectedTables, this._referencedTables, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), this._webAppRootDirectory, false, this._connectionString, this._generateFrom, this._generatedSqlType, MySettings.AppVersion, this.CbxUseWebApi.Checked);
            }

            if (MySettings.AppVersion == ApplicationVersion.ProfessionalPlus)
            {
                if (this._generatedSqlType == GeneratedSqlType.EFCore)
                {
                    DataLayerBase dataLayerBase = new DataLayerBase(table, this._selectedTables, this._referencedTables, this.TxtAPIName.Text.Trim(), this.TxtAPINameDirectory.Text.Trim(), this._isUseStoredProc, this._spPrefixSuffixIndex, this._spPrefix, this._spSuffix, this._generateFrom, MySettings.AppVersion, this.TxtDatabase.Text.Trim(), this._generatedSqlType);
                    EfBusinessObject efBusinessObject = new EfBusinessObject(table, this._selectedTables, this._referencedTables, this.TxtAPIName.Text.Trim(), this.TxtAPINameDirectory.Text.Trim(), this._isUseStoredProc, this._spPrefixSuffixIndex, this._spPrefix, this._spSuffix, this._generateFrom);
                }
                else
                {
                    DataLayerBaseClassic layerBaseClassic = new DataLayerBaseClassic(table, this._selectedTables, this._referencedTables, this.TxtAPIName.Text.Trim(), this.TxtAPINameDirectory.Text.Trim(), this._isUseStoredProc, this._spPrefixSuffixIndex, this._spPrefix, this._spSuffix, this._generateFrom, MySettings.AppVersion);
                    if (this.RbtnUseStoredProc.Checked)
                    {
                        StoredProcedure storedProcedure = new StoredProcedure(table, this.TxtAPINameDirectory.Text.Trim(), this._connectionString, true, this._spPrefixSuffixIndex, this._spPrefix, this._spSuffix, this._generateFrom, this.TxtAPINameDirectory.Text.Trim() + "\\sprocErrors.txt", this._isSqlVersion2012OrHigher);
                    }
                    else
                    {
                        DynamicSql dynamicSql = new DynamicSql(table, this._selectedTables, this.TxtAPIName.Text.Trim(), this.TxtAPINameDirectory.Text.Trim(), this._generateFrom, this._isSqlVersion2012OrHigher);
                    }
                }
            }
            else
            {
                DataLayerBase dataLayerBase1 = new DataLayerBase(table, this._selectedTables, this._referencedTables, this.TxtAPIName.Text.Trim(), this.TxtAPINameDirectory.Text.Trim(), this._isUseStoredProc, this._spPrefixSuffixIndex, this._spPrefix, this._spSuffix, this._generateFrom, MySettings.AppVersion, this.TxtDatabase.Text.Trim(), this._generatedSqlType);
            }

            ModelBase modelBase = new ModelBase(table, this.TxtAPIName.Text.Trim(), this.TxtAPINameDirectory.Text.Trim(), this._generatedSqlType);
            Model model = new Model(table, this.TxtAPIName.Text.Trim(), this.TxtAPINameDirectory.Text.Trim());

            if (!table.IsContainsPrimaryAndForeignKeyColumnsOnly && this.CbxUseWebApi.Checked)
            {
                ControllerlBase controllerlBase = new ControllerlBase(table, this._selectedTables, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), this.TxtWebAPIName.Text.Trim(), this._webAppRootDirectory, this.TxtAPINameDirectory.Text.Trim(), this._webAPIRootDirectory, this._isUseStoredProc, this._isSqlVersion2012OrHigher, webFormAssignMents, viewNames, this._generateFrom, MySettings.AppVersion, this._generatedSqlType, this.CbxUseLogging.Checked, this.CbxUseCaching.Checked, this.CbxUseAuditLogging.Checked, this.CbxEmailNotification.Checked, ControllerBaseType.ApiControllerBase, this.CbxUseWebApi.Checked, null);
                ApiController apiController = new ApiController(table, this.TxtWebAPIName.Text.Trim(), this._webAPIRootDirectory, this.CbxUseLogging.Checked, this.CbxUseCaching.Checked, this.CbxUseAuditLogging.Checked, this.CbxEmailNotification.Checked);
            }

            if (this.CbxUseWebApplication.Checked && this._generateFrom == DatabaseObjectToGenerateFrom.Tables && !table.IsContainsPrimaryAndForeignKeyColumnsOnly && MySettings.AppVersion != ApplicationVersion.Express)
            {
                if (webFormAssignMents.RecordDetails)
                {
                    DetailsPage detailsPage = new DetailsPage(table, this._selectedTables, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), this._webAppRootDirectory, this.TxtDetails.Text.Trim(), MySettings.AppVersion);
                    RazorPageModel razorPageModel = new RazorPageModel(MVCGridViewType.RecordDetails, this.TxtDetails.Text.Trim(), table, this._selectedTables, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), this.TxtWebAPIName.Text.Trim(), this._webAppRootDirectory, this.TxtAPINameDirectory.Text.Trim(), this._webAPIRootDirectory, this._isUseStoredProc, this._isSqlVersion2012OrHigher, webFormAssignMents, viewNames, this._generateFrom, MySettings.AppVersion, this._generatedSqlType, this.CbxUseLogging.Checked, this.CbxUseCaching.Checked, this.CbxUseAuditLogging.Checked, this.CbxEmailNotification.Checked, ControllerBaseType.ControllerBase, this.CbxUseWebApi.Checked);
                }
                if (webFormAssignMents.AddRecord || webFormAssignMents.UpdateRecord)
                {
                    AddEditPartialPage addEditPartialPage = new AddEditPartialPage(MVCGridViewType.AddEdit, table, this._selectedTables, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), this._webAppRootDirectory, AppendAddEditRecordContentType.AddEditPartialView, "", MySettings.AppVersion);
                    AddEditPartialPageModel partialPageModel = new AddEditPartialPageModel(MVCGridViewType.AddEdit, table, this._selectedTables, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), this._webAppRootDirectory, AppendAddEditRecordContentType.AddEditPartialView, this.CbxUseWebApi.Checked);
                    FunctionsHelper functionsHelper = new FunctionsHelper(table, this._selectedTables, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), this._webAppRootDirectory, this.CbxUseWebApi.Checked, this.CbxUseAuditLogging.Checked, this._generatedSqlType, this.CbxEmailNotification.Checked);
                }
                if (webFormAssignMents.Unbound)
                {
                    AddEditPartialPage addEditPartialPage = new AddEditPartialPage(MVCGridViewType.Unbound, table, this._selectedTables, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), this._webAppRootDirectory, AppendAddEditRecordContentType.Unbound, this.TxtUnbound.Text.Trim(), MySettings.AppVersion);
                    RazorPageModel razorPageModel = new RazorPageModel(MVCGridViewType.Unbound, this.TxtUnbound.Text.Trim(), table, this._selectedTables, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), this.TxtWebAPIName.Text.Trim(), this._webAppRootDirectory, this.TxtAPINameDirectory.Text.Trim(), this._webAPIRootDirectory, this._isUseStoredProc, this._isSqlVersion2012OrHigher, webFormAssignMents, viewNames, this._generateFrom, MySettings.AppVersion, this._generatedSqlType, this.CbxUseLogging.Checked, this.CbxUseCaching.Checked, this.CbxUseAuditLogging.Checked, this.CbxEmailNotification.Checked, ControllerBaseType.ControllerBase, false, null);
                }
                if (webFormAssignMents.AddRecord)
                {
                    AddOrUpdatePage addOrUpdatePage = new AddOrUpdatePage(table, this.TxtWebAppName.Text.Trim(), this.TxtAdd.Text.Trim(), this._webAppRootDirectory);
                    RazorPageModel razorPageModel = new RazorPageModel(MVCGridViewType.Add, this.TxtAdd.Text.Trim(), table, this._selectedTables, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), this.TxtWebAPIName.Text.Trim(), this._webAppRootDirectory, this.TxtAPINameDirectory.Text.Trim(), this._webAPIRootDirectory, this._isUseStoredProc, this._isSqlVersion2012OrHigher, webFormAssignMents, viewNames, this._generateFrom, MySettings.AppVersion, this._generatedSqlType, this.CbxUseLogging.Checked, this.CbxUseCaching.Checked, this.CbxUseAuditLogging.Checked, this.CbxEmailNotification.Checked, ControllerBaseType.ControllerBase, this.CbxUseWebApi.Checked, null);
                }
                if (webFormAssignMents.UpdateRecord)
                {
                    AddOrUpdatePage addOrUpdatePage = new AddOrUpdatePage(table, this.TxtWebAppName.Text.Trim(), this.TxtUpdate.Text.Trim(), this._webAppRootDirectory);
                    RazorPageModel razorPageModel = new RazorPageModel(MVCGridViewType.Update, this.TxtUpdate.Text.Trim(), table, this._selectedTables, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), this.TxtWebAPIName.Text.Trim(), this._webAppRootDirectory, this.TxtAPINameDirectory.Text.Trim(), this._webAPIRootDirectory, this._isUseStoredProc, this._isSqlVersion2012OrHigher, webFormAssignMents, viewNames, this._generateFrom, MySettings.AppVersion, this._generatedSqlType, this.CbxUseLogging.Checked, this.CbxUseCaching.Checked, this.CbxUseAuditLogging.Checked, this.CbxEmailNotification.Checked, ControllerBaseType.ControllerBase, this.CbxUseWebApi.Checked, null);
                }
                if (webFormAssignMents.ListReadOnly)
                {
                    RazorPage razorPage = new RazorPage(MVCGridViewType.ReadOnly, table, this._selectedTables, this._webAppRootDirectory, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), viewNames, this.TxtListReadOnly.Text.Trim(), this._selectedJQueryUITheme, this._generatedSqlType, MySettings.AppVersion, this.CbxUseWebApi.Checked, this.CbxUseAuditLogging.Checked, this._webApiBaseAddress);
                    RazorPageModel razorPageModel = new RazorPageModel(MVCGridViewType.ReadOnly, this.TxtListReadOnly.Text.Trim(), table, this._selectedTables, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), this.TxtWebAPIName.Text.Trim(), this._webAppRootDirectory, this.TxtAPINameDirectory.Text.Trim(), this._webAPIRootDirectory, this._isUseStoredProc, this._isSqlVersion2012OrHigher, webFormAssignMents, viewNames, this._generateFrom, MySettings.AppVersion, this._generatedSqlType, this.CbxUseLogging.Checked, this.CbxUseCaching.Checked, this.CbxUseAuditLogging.Checked, this.CbxEmailNotification.Checked, ControllerBaseType.ControllerBase, this.CbxUseWebApi.Checked, null);
                }
                if (webFormAssignMents.ListCrudRedirect)
                {
                    RazorPage razorPage = new RazorPage(MVCGridViewType.Redirect, table, this._selectedTables, this._webAppRootDirectory, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), viewNames, this.TxtListCrudRedirect.Text.Trim(), this._selectedJQueryUITheme, this._generatedSqlType, MySettings.AppVersion, this.CbxUseWebApi.Checked, this.CbxUseAuditLogging.Checked, this._webApiBaseAddress);
                    RazorPageModel razorPageModel = new RazorPageModel(MVCGridViewType.Redirect, this.TxtListCrudRedirect.Text.Trim(), table, this._selectedTables, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), this.TxtWebAPIName.Text.Trim(), this._webAppRootDirectory, this.TxtAPINameDirectory.Text.Trim(), this._webAPIRootDirectory, this._isUseStoredProc, this._isSqlVersion2012OrHigher, webFormAssignMents, viewNames, this._generateFrom, MySettings.AppVersion, this._generatedSqlType, this.CbxUseLogging.Checked, this.CbxUseCaching.Checked, this.CbxUseAuditLogging.Checked, this.CbxEmailNotification.Checked, ControllerBaseType.ControllerBase, this.CbxUseWebApi.Checked, null);
                }
                if (webFormAssignMents.ListCrud)
                {
                    RazorPage razorPage = new RazorPage(MVCGridViewType.CRUD, table, this._selectedTables, this._webAppRootDirectory, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), viewNames, this.TxtListCrud.Text.Trim(), this._selectedJQueryUITheme, this._generatedSqlType, MySettings.AppVersion, this.CbxUseWebApi.Checked, this.CbxUseAuditLogging.Checked, this._webApiBaseAddress);
                    RazorPageModel razorPageModel = new RazorPageModel(MVCGridViewType.CRUD, this.TxtListCrud.Text.Trim(), table, this._selectedTables, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), this.TxtWebAPIName.Text.Trim(), this._webAppRootDirectory, this.TxtAPINameDirectory.Text.Trim(), this._webAPIRootDirectory, this._isUseStoredProc, this._isSqlVersion2012OrHigher, webFormAssignMents, viewNames, this._generateFrom, MySettings.AppVersion, this._generatedSqlType, this.CbxUseLogging.Checked, this.CbxUseCaching.Checked, this.CbxUseAuditLogging.Checked, this.CbxEmailNotification.Checked, ControllerBaseType.ControllerBase, this.CbxUseWebApi.Checked, null);
                    FunctionsHelper functionsHelper = new FunctionsHelper(table, this._selectedTables, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), this._webAppRootDirectory, this.CbxUseWebApi.Checked, this.CbxUseAuditLogging.Checked, this._generatedSqlType, this.CbxEmailNotification.Checked);
                }
                if (webFormAssignMents.ListSearch)
                {
                    RazorPage razorPage = new RazorPage(MVCGridViewType.Search, table, this._selectedTables, this._webAppRootDirectory, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), viewNames, this.TxtListSearch.Text.Trim(), this._selectedJQueryUITheme, this._generatedSqlType, MySettings.AppVersion, this.CbxUseWebApi.Checked, this.CbxUseAuditLogging.Checked, this._webApiBaseAddress);
                    RazorPageModel razorPageModel = new RazorPageModel(MVCGridViewType.Search, this.TxtListSearch.Text.Trim(), table, this._selectedTables, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), this.TxtWebAPIName.Text.Trim(), this._webAppRootDirectory, this.TxtAPINameDirectory.Text.Trim(), this._webAPIRootDirectory, this._isUseStoredProc, this._isSqlVersion2012OrHigher, webFormAssignMents, viewNames, this._generateFrom, MySettings.AppVersion, this._generatedSqlType, this.CbxUseLogging.Checked, this.CbxUseCaching.Checked, this.CbxUseAuditLogging.Checked, this.CbxEmailNotification.Checked, ControllerBaseType.ControllerBase, this.CbxUseWebApi.Checked, null);
                }
                if (webFormAssignMents.ListScrollLoad)
                {
                    RazorPage razorPage = new RazorPage(MVCGridViewType.ScrollLoad, table, this._selectedTables, this._webAppRootDirectory, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), viewNames, this.TxtListScrollLoad.Text.Trim(), this._selectedJQueryUITheme, this._generatedSqlType, MySettings.AppVersion, this.CbxUseWebApi.Checked, this.CbxUseAuditLogging.Checked, this._webApiBaseAddress);
                    RazorPageModel razorPageModel = new RazorPageModel(MVCGridViewType.ScrollLoad, this.TxtListScrollLoad.Text.Trim(), table, this._selectedTables, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), this.TxtWebAPIName.Text.Trim(), this._webAppRootDirectory, this.TxtAPINameDirectory.Text.Trim(), this._webAPIRootDirectory, this._isUseStoredProc, this._isSqlVersion2012OrHigher, webFormAssignMents, viewNames, this._generateFrom, MySettings.AppVersion, this._generatedSqlType, this.CbxUseLogging.Checked, this.CbxUseCaching.Checked, this.CbxUseAuditLogging.Checked, this.CbxEmailNotification.Checked, ControllerBaseType.ControllerBase, this.CbxUseWebApi.Checked, null);
                }
                if (webFormAssignMents.ListInline)
                {
                    RazorPage razorPage = new RazorPage(MVCGridViewType.Inline, table, this._selectedTables, this._webAppRootDirectory, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), viewNames, this.TxtListInline.Text.Trim(), this._selectedJQueryUITheme, this._generatedSqlType, MySettings.AppVersion, this.CbxUseWebApi.Checked, this.CbxUseAuditLogging.Checked, this._webApiBaseAddress);
                    RazorPageModel razorPageModel = new RazorPageModel(MVCGridViewType.Inline, this.TxtListInline.Text.Trim(), table, this._selectedTables, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), this.TxtWebAPIName.Text.Trim(), this._webAppRootDirectory, this.TxtAPINameDirectory.Text.Trim(), this._webAPIRootDirectory, this._isUseStoredProc, this._isSqlVersion2012OrHigher, webFormAssignMents, viewNames, this._generateFrom, MySettings.AppVersion, this._generatedSqlType, this.CbxUseLogging.Checked, this.CbxUseCaching.Checked, this.CbxUseAuditLogging.Checked, this.CbxEmailNotification.Checked, ControllerBaseType.ControllerBase, this.CbxUseWebApi.Checked, null);
                    FunctionsHelper functionsHelper = new FunctionsHelper(table, this._selectedTables, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), this._webAppRootDirectory, this.CbxUseWebApi.Checked, this.CbxUseAuditLogging.Checked, this._generatedSqlType, this.CbxEmailNotification.Checked);
                }
                if (webFormAssignMents.ListForeach)
                {
                    RazorPage razorPage = new RazorPage(MVCGridViewType.Foreach, table, this._selectedTables, this._webAppRootDirectory, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), viewNames, this.TxtListForeach.Text.Trim(), this._selectedJQueryUITheme, this._generatedSqlType, MySettings.AppVersion, this.CbxUseWebApi.Checked, this.CbxUseAuditLogging.Checked, this._webApiBaseAddress);
                    RazorPageModel razorPageModel = new RazorPageModel(MVCGridViewType.Foreach, this.TxtListForeach.Text.Trim(), table, this._selectedTables, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), this.TxtWebAPIName.Text.Trim(), this._webAppRootDirectory, this.TxtAPINameDirectory.Text.Trim(), this._webAPIRootDirectory, this._isUseStoredProc, this._isSqlVersion2012OrHigher, webFormAssignMents, viewNames, this._generateFrom, MySettings.AppVersion, this._generatedSqlType, this.CbxUseLogging.Checked, this.CbxUseCaching.Checked, this.CbxUseAuditLogging.Checked, this.CbxEmailNotification.Checked, ControllerBaseType.ControllerBase, this.CbxUseWebApi.Checked, null);
                }
                if (webFormAssignMents.ListMasterDetailGrid)
                {
                    RazorPage razorPage = new RazorPage(MVCGridViewType.MasterDetailGrid, table, this._selectedTables, this._webAppRootDirectory, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), viewNames, this.TxtListMasterDetailGrid.Text.Trim(), this._selectedJQueryUITheme, this._generatedSqlType, MySettings.AppVersion, this.CbxUseWebApi.Checked, this.CbxUseAuditLogging.Checked, this._webApiBaseAddress);
                    RazorPageModel razorPageModel = new RazorPageModel(MVCGridViewType.MasterDetailGrid, this.TxtListMasterDetailGrid.Text.Trim(), table, this._selectedTables, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), this.TxtWebAPIName.Text.Trim(), this._webAppRootDirectory, this.TxtAPINameDirectory.Text.Trim(), this._webAPIRootDirectory, this._isUseStoredProc, this._isSqlVersion2012OrHigher, webFormAssignMents, viewNames, this._generateFrom, MySettings.AppVersion, this._generatedSqlType, this.CbxUseLogging.Checked, this.CbxUseCaching.Checked, this.CbxUseAuditLogging.Checked, this.CbxEmailNotification.Checked, ControllerBaseType.ControllerBase, this.CbxUseWebApi.Checked, null);
                }
                if (webFormAssignMents.ListMasterDetailSubGrid)
                {
                    RazorPage razorPage = new RazorPage(MVCGridViewType.MasterDetailSubGrid, table, this._selectedTables, this._webAppRootDirectory, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), viewNames, this.TxtListMasterDetailSubGrid.Text.Trim(), this._selectedJQueryUITheme, this._generatedSqlType, MySettings.AppVersion, this.CbxUseWebApi.Checked, this.CbxUseAuditLogging.Checked, this._webApiBaseAddress);
                    RazorPageModel razorPageModel = new RazorPageModel(MVCGridViewType.MasterDetailSubGrid, this.TxtListMasterDetailSubGrid.Text.Trim(), table, this._selectedTables, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), this.TxtWebAPIName.Text.Trim(), this._webAppRootDirectory, this.TxtAPINameDirectory.Text.Trim(), this._webAPIRootDirectory, this._isUseStoredProc, this._isSqlVersion2012OrHigher, webFormAssignMents, viewNames, this._generateFrom, MySettings.AppVersion, this._generatedSqlType, this.CbxUseLogging.Checked, this.CbxUseCaching.Checked, this.CbxUseAuditLogging.Checked, this.CbxEmailNotification.Checked, ControllerBaseType.ControllerBase, this.CbxUseWebApi.Checked, null);
                }
                if (webFormAssignMents.ListTotals && table.IsContainsMoneyOrDecimalField)
                {
                    RazorPage razorPage = new RazorPage(MVCGridViewType.Totals, table, this._selectedTables, this._webAppRootDirectory, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), viewNames, this.TxtListTotals.Text.Trim(), this._selectedJQueryUITheme, this._generatedSqlType, MySettings.AppVersion, this.CbxUseWebApi.Checked, this.CbxUseAuditLogging.Checked, this._webApiBaseAddress);
                    RazorPageModel razorPageModel = new RazorPageModel(MVCGridViewType.Totals, this.TxtListTotals.Text.Trim(), table, this._selectedTables, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), this.TxtWebAPIName.Text.Trim(), this._webAppRootDirectory, this.TxtAPINameDirectory.Text.Trim(), this._webAPIRootDirectory, this._isUseStoredProc, this._isSqlVersion2012OrHigher, webFormAssignMents, viewNames, this._generateFrom, MySettings.AppVersion, this._generatedSqlType, this.CbxUseLogging.Checked, this.CbxUseCaching.Checked, this.CbxUseAuditLogging.Checked, this.CbxEmailNotification.Checked, ControllerBaseType.ControllerBase, this.CbxUseWebApi.Checked, null);
                }
                if (webFormAssignMents.ListGroupedBy && table.ForeignKeyCount > 0)
                {
                    RazorPage razorPage = new RazorPage(MVCGridViewType.GroupedBy, table, this._selectedTables, this._webAppRootDirectory, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), viewNames, this.TxtListGroupedBy.Text.Trim(), this._selectedJQueryUITheme, this._generatedSqlType, MySettings.AppVersion, this.CbxUseWebApi.Checked, this.CbxUseAuditLogging.Checked, this._webApiBaseAddress);
                    RazorPageModel razorPageModel = new RazorPageModel(MVCGridViewType.GroupedBy, this.TxtListGroupedBy.Text.Trim(), table, this._selectedTables, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), this.TxtWebAPIName.Text.Trim(), this._webAppRootDirectory, this.TxtAPINameDirectory.Text.Trim(), this._webAPIRootDirectory, this._isUseStoredProc, this._isSqlVersion2012OrHigher, webFormAssignMents, viewNames, this._generateFrom, MySettings.AppVersion, this._generatedSqlType, this.CbxUseLogging.Checked, this.CbxUseCaching.Checked, this.CbxUseAuditLogging.Checked, this.CbxEmailNotification.Checked, ControllerBaseType.ControllerBase, this.CbxUseWebApi.Checked);
                }
                if (webFormAssignMents.ListTotalsGroupedBy && table.ForeignKeyCount > 0 && table.IsContainsMoneyOrDecimalField)
                {
                    RazorPage razorPage = new RazorPage(MVCGridViewType.GroupedByWithTotals, table, this._selectedTables, this._webAppRootDirectory, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), viewNames, this.TxtListTotalsGroupedBy.Text.Trim(), this._selectedJQueryUITheme, this._generatedSqlType, MySettings.AppVersion, this.CbxUseWebApi.Checked, this.CbxUseAuditLogging.Checked, this._webApiBaseAddress);
                    RazorPageModel razorPageModel = new RazorPageModel(MVCGridViewType.GroupedByWithTotals, this.TxtListTotalsGroupedBy.Text.Trim(), table, this._selectedTables, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), this.TxtWebAPIName.Text.Trim(), this._webAppRootDirectory, this.TxtAPINameDirectory.Text.Trim(), this._webAPIRootDirectory, this._isUseStoredProc, this._isSqlVersion2012OrHigher, webFormAssignMents, viewNames, this._generateFrom, MySettings.AppVersion, this._generatedSqlType, this.CbxUseLogging.Checked, this.CbxUseCaching.Checked, this.CbxUseAuditLogging.Checked, this.CbxEmailNotification.Checked, ControllerBaseType.ControllerBase, this.CbxUseWebApi.Checked, null);
                }
                if (webFormAssignMents.WorkflowAssignSteps && table.Name == "WorkflowStepsMaster")
                {
                    this._connectionString = GetConnectionString();

                    AssignWorklowStepsPage assignWorklowSteps = new AssignWorklowStepsPage(MVCGridViewType.AssignWorkflowSteps, table, this._selectedTables, this._webAppRootDirectory, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), viewNames, this.TxtWorkflowAssignSteps.Text.Trim(), this._selectedJQueryUITheme, this._generatedSqlType, AppendAddEditRecordContentType.AssignWorkflowSteps);
                    AssignWorkflowStepsModel razorPageModel = new AssignWorkflowStepsModel(MVCGridViewType.AssignWorkflowSteps, table, this._selectedTables, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), this.TxtWebAPIName.Text.Trim(), this._webAppRootDirectory, this.TxtAPINameDirectory.Text.Trim(), this._webAPIRootDirectory, this._isUseStoredProc, this._isSqlVersion2012OrHigher, webFormAssignMents, this._generateFrom, MySettings.AppVersion, this._generatedSqlType, this.CbxUseLogging.Checked, this.CbxUseCaching.Checked, this.CbxUseAuditLogging.Checked, this.CbxEmailNotification.Checked,
                        this._connectionString, ControllerBaseType.ControllerBase, this.CbxUseWebApi.Checked);
                }
            }

            if (!this.CbxUseWebApplication.Checked)
                return;

            if (this._generateFrom == DatabaseObjectToGenerateFrom.Views && !table.IsContainsPrimaryAndForeignKeyColumnsOnly)
            {
                RazorPage razorPage = new RazorPage(MVCGridViewType.ReadOnly, table, this._selectedTables, this._webAppRootDirectory, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), viewNames, this.TxtListReadOnly.Text.Trim(), this._selectedJQueryUITheme, this._generatedSqlType, MySettings.AppVersion, this.CbxUseWebApi.Checked, this.CbxUseAuditLogging.Checked, this._webApiBaseAddress);
                RazorPageModel razorPageModel = new RazorPageModel(MVCGridViewType.ReadOnly, this.TxtListReadOnly.Text.Trim(), table, this._selectedTables, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), this.TxtWebAPIName.Text.Trim(), this._webAppRootDirectory, this.TxtAPINameDirectory.Text.Trim(), this._webAPIRootDirectory, this._isUseStoredProc, this._isSqlVersion2012OrHigher, webFormAssignMents, viewNames, this._generateFrom, MySettings.AppVersion, this._generatedSqlType, this.CbxUseLogging.Checked, this.CbxUseCaching.Checked, this.CbxUseAuditLogging.Checked, this.CbxEmailNotification.Checked, ControllerBaseType.ControllerBase, this.CbxUseWebApi.Checked, null);
            }

            if (MySettings.AppVersion != ApplicationVersion.Express || table.IsContainsPrimaryAndForeignKeyColumnsOnly)
                return;

            AddEditPartialPage addEditPartialPage1 = new AddEditPartialPage(MVCGridViewType.Unbound, table, this._selectedTables, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), this._webAppRootDirectory, AppendAddEditRecordContentType.Unbound, this.TxtUnbound.Text.Trim(), MySettings.AppVersion);
            RazorPageModel razorPageModel1 = new RazorPageModel(MVCGridViewType.Unbound, this.TxtUnbound.Text.Trim(), table, this._selectedTables, this.TxtWebAppName.Text.Trim(), this.TxtAPIName.Text.Trim(), this.TxtWebAPIName.Text.Trim(), this._webAppRootDirectory, this.TxtAPINameDirectory.Text.Trim(), this._webAPIRootDirectory, this._isUseStoredProc, this._isSqlVersion2012OrHigher, webFormAssignMents, viewNames, this._generateFrom, MySettings.AppVersion, this._generatedSqlType, this.CbxUseLogging.Checked, this.CbxUseCaching.Checked, this.CbxUseAuditLogging.Checked, this.CbxEmailNotification.Checked, ControllerBaseType.ControllerBase, false, null);
        }

        private IsCheckedView GetIsCheckedWebFormAssignMents()
        {
            IsCheckedView isCheckedView = new IsCheckedView();
            if (this.RbtnGenerateFromAllViews.Checked || this.RbtnGenerateFromSelectedViews.Checked)
            {
                isCheckedView.ListReadOnly = this._isCheckedViewListReadOnly;
            }
            else
            {
                isCheckedView.ListCrudRedirect = this._isCheckedViewListCrudRedirect;
                isCheckedView.AddRecord = this._isCheckedViewAddRecord;
                isCheckedView.UpdateRecord = this._isCheckedViewUpdateRecord;
                isCheckedView.RecordDetails = this._isCheckedViewRecordDetails;
                isCheckedView.ListReadOnly = this._isCheckedViewListReadOnly;
                isCheckedView.ListCrud = this._isCheckedViewListCrud;
                isCheckedView.ListGroupedBy = this._isCheckedViewListGroupedBy;
                isCheckedView.ListTotals = this._isCheckedViewListTotals;
                isCheckedView.ListTotalsGroupedBy = this._isCheckedViewListTotalsGroupedBy;
                isCheckedView.ListSearch = this._isCheckedViewListSearch;
                isCheckedView.ListScrollLoad = this._isCheckedViewListScrollLoad;
                isCheckedView.ListInline = this._isCheckedViewListInline;
                isCheckedView.ListForeach = this._isCheckedViewListForeach;
                isCheckedView.ListMasterDetailGrid = this._isCheckedViewListMasterDetailGrid;
                isCheckedView.ListMasterDetailSubGrid = this._isCheckedViewListMasterDetailSubGrid;
                isCheckedView.Unbound = this._isCheckedViewUnbound;
                isCheckedView.WorkflowAssignSteps = this._isCheckedViewWorkflowSteps;
            }
            return isCheckedView;
        }

        private ViewNames GetViewNames()
        {
            return new ViewNames()
            {
                ListCrudRedirect = this.TxtListCrudRedirect.Text.Trim(),
                AddRecord = this.TxtAdd.Text.Trim(),
                UpdateRecord = this.TxtUpdate.Text.Trim(),
                RecordDetails = this.TxtDetails.Text.Trim(),
                ListReadOnly = this.TxtListReadOnly.Text.Trim(),
                ListCrud = this.TxtListCrud.Text.Trim(),
                ListGroupedBy = this.TxtListGroupedBy.Text.Trim(),
                ListTotals = this.TxtListTotals.Text.Trim(),
                ListTotalsGroupedBy = this.TxtListTotalsGroupedBy.Text.Trim(),
                ListSearch = this.TxtListSearch.Text.Trim(),
                ListScrollLoad = this.TxtListScrollLoad.Text.Trim(),
                ListInline = this.TxtListInline.Text.Trim(),
                ListForeach = this.TxtListForeach.Text.Trim(),
                ListMasterDetailGrid = this.TxtListMasterDetailGrid.Text.Trim(),
                ListMasterDetailSubGrid = this.TxtListMasterDetailSubGrid.Text.Trim(),
                Unbound = this.TxtUnbound.Text.Trim(),
                WorkflowAssignSteps=this.TxtWorkflowAssignSteps.Text.Trim()

            };
        }

        private Table GetForeignTable(Table table, Column column, Tables selectedTables)
        {
            return selectedTables.Find(t => t.OwnerOriginal + "." + t.NameOriginal == column.ForeignKeyTableOwnerOriginal + "." + column.ForeignKeyTableNameOriginal);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void CbxUseLogging_CheckedChanged(object sender, EventArgs e)
        {
            this._isUseLogging = this.CbxUseLogging.Checked;

            if (this.CbxUseLogging.Checked)
                this.GbxLoggingType.Enabled = true;
            else
                this.GbxLoggingType.Enabled = false;
        }

        private void TxtAPIName_TextChanged(object sender, EventArgs e)
        {
            if (this.CbxUseWebApplication.Checked)
            {
                if (TxtAPIName.Focused)
                    this.TxtAPINameDirectory.Text = this.TxtDirectory.Text.Trim() + this.TxtWebAppName.Text.Trim() + "\\" + this.TxtAPIName.Text.Trim();
            }
            else if (!this.CbxUseWebApplication.Checked && this.CbxUseWebApi.Checked)
            {
                if (TxtAPIName.Focused)
                    UpdateBusinessAPIDirectories();
            }
        }

        private void TxtWebAPIName_TextChanged(object sender, EventArgs e)
        {
            if (this.CbxUseWebApplication.Checked)
            {
                if (TxtWebAPIName.Focused)
                    this.TxtWebAPINameDirectory.Text = this.TxtDirectory.Text.Trim() + this.TxtWebAppName.Text.Trim() + "\\" + this.TxtWebAPIName.Text.Trim();
            }
            else if (!this.CbxUseWebApplication.Checked && this.CbxUseWebApi.Checked)
            {
                this.TxtAPIName.Text = this.TxtWebAPIName.Text.Trim() + "API";
                this.UpdateBusinessAPIDirectories();
            }
        }

        private void CbxUseWebApplication_CheckedChanged(object sender, EventArgs e)
        {
            if (this.CbxUseWebApplication.Checked)
            {
                TxtWebAppName.Enabled = true;
                TxtDirectory.Enabled = true;
                BtnBrowseCodeDirectory.Enabled = true;
                CbxGenerateCodeExamples.Enabled = true;
                TxtDevServerPort.Enabled = true;

                TxtWebAPINameDirectory.Enabled = false;
                BtnBrowseWebAPIDirectory.Enabled = false;

                this.TxtAPIName.Text = this.TxtWebAppName.Text.Trim() + "API";
                this.TxtWebAPIName.Text = this.TxtWebAppName.Text.Trim() + "WebAPI";
                this.UpdateAPIDirectories();
            }
            else if (!this.CbxUseWebApplication.Checked && this.CbxUseWebApi.Checked)
            {
                TxtWebAppName.Enabled = false;
                TxtDirectory.Enabled = false;
                BtnBrowseCodeDirectory.Enabled = false;
                CbxGenerateCodeExamples.Enabled = false;
                TxtDevServerPort.Enabled = false;

                TxtWebAPINameDirectory.Enabled = true;
                BtnBrowseWebAPIDirectory.Enabled = true;

                this.TxtAPIName.Text = this.TxtWebAPIName.Text.Trim() + "API";
                UpdateBusinessAPIDirectories();
            }
        }

        private void CbxUseWebApi_CheckedChanged(object sender, EventArgs e)
        {
            if (this.CbxUseWebApplication.Checked && this.CbxUseWebApi.Checked)
            {
                TxtWebAPIName.Enabled = true;
                TxtWebAPINameDirectory.Enabled = false;
                BtnBrowseWebAPIDirectory.Enabled = false;

                this.TxtAPIName.Text = this.TxtWebAppName.Text.Trim() + "API";
                this.TxtWebAPIName.Text = this.TxtWebAppName.Text.Trim() + "WebAPI";
                this.UpdateAPIDirectories();
            }
            else if (!this.CbxUseWebApplication.Checked && this.CbxUseWebApi.Checked)
            {
                TxtWebAppName.Enabled = false;
                TxtDirectory.Enabled = false;
                BtnBrowseCodeDirectory.Enabled = false;
                CbxGenerateCodeExamples.Enabled = false;
                TxtDevServerPort.Enabled = false;

                TxtWebAPIName.Enabled = true;
                TxtWebAPINameDirectory.Enabled = true;
                BtnBrowseWebAPIDirectory.Enabled = true;

                this.TxtAPIName.Text = this.TxtWebAPIName.Text.Trim() + "API";
                UpdateBusinessAPIDirectories();
            }
            else
            {
                TxtWebAPIName.Enabled = false;
                TxtWebAPINameDirectory.Enabled = false;
                BtnBrowseWebAPIDirectory.Enabled = false;
            }
        }

        private void TxtWebAPINameDirectory_TextChanged(object sender, EventArgs e)
        {
            if (!this.CbxUseWebApplication.Checked && this.CbxUseWebApi.Checked)
                this.UpdateBusinessAPIDirectories();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoCodeGenerator));
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.RbtnGenerateFromAllTables = new System.Windows.Forms.RadioButton();
            this.TabUISettings = new System.Windows.Forms.TabPage();
            this.GbxViewNames = new System.Windows.Forms.GroupBox();
            this.TxtWorkflowAssignSteps = new System.Windows.Forms.TextBox();
            this.TxtListMasterDetailSubGrid = new System.Windows.Forms.TextBox();
            this.TxtListMasterDetailGrid = new System.Windows.Forms.TextBox();
            this.TxtListForeach = new System.Windows.Forms.TextBox();
            this.TxtListScrollLoad = new System.Windows.Forms.TextBox();
            this.TxtListInline = new System.Windows.Forms.TextBox();
            this.TxtListTotals = new System.Windows.Forms.TextBox();
            this.TxtListTotalsGroupedBy = new System.Windows.Forms.TextBox();
            this.TxtUnbound = new System.Windows.Forms.TextBox();
            this.TxtListSearch = new System.Windows.Forms.TextBox();
            this.TxtListCrudRedirect = new System.Windows.Forms.TextBox();
            this.TxtAdd = new System.Windows.Forms.TextBox();
            this.TxtUpdate = new System.Windows.Forms.TextBox();
            this.TxtDetails = new System.Windows.Forms.TextBox();
            this.TxtListReadOnly = new System.Windows.Forms.TextBox();
            this.TxtListCrud = new System.Windows.Forms.TextBox();
            this.TxtListGroupedBy = new System.Windows.Forms.TextBox();
            this.GbxThemes = new System.Windows.Forms.GroupBox();
            this.pictureBox19 = new System.Windows.Forms.PictureBox();
            this.CbxJQueryUITheme = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.GbxWebFormsToGenerate = new System.Windows.Forms.GroupBox();
            this.CbxWorkflowAssignSteps = new System.Windows.Forms.CheckBox();
            this.pictureBox84 = new System.Windows.Forms.PictureBox();
            this.CbxListMasterDetailSubGrid = new System.Windows.Forms.CheckBox();
            this.CbxListMasterDetailGrid = new System.Windows.Forms.CheckBox();
            this.pictureBox54 = new System.Windows.Forms.PictureBox();
            this.pictureBox49 = new System.Windows.Forms.PictureBox();
            this.CbxListForeach = new System.Windows.Forms.CheckBox();
            this.pictureBox34 = new System.Windows.Forms.PictureBox();
            this.CbxListScrollLoad = new System.Windows.Forms.CheckBox();
            this.pictureBox51 = new System.Windows.Forms.PictureBox();
            this.CbxListInline = new System.Windows.Forms.CheckBox();
            this.pictureBox50 = new System.Windows.Forms.PictureBox();
            this.pictureBox39 = new System.Windows.Forms.PictureBox();
            this.pictureBox21 = new System.Windows.Forms.PictureBox();
            this.pictureBox11 = new System.Windows.Forms.PictureBox();
            this.pictureBox10 = new System.Windows.Forms.PictureBox();
            this.pictureBox17 = new System.Windows.Forms.PictureBox();
            this.pictureBox24 = new System.Windows.Forms.PictureBox();
            this.pictureBox27 = new System.Windows.Forms.PictureBox();
            this.CbxListSearch = new System.Windows.Forms.CheckBox();
            this.CbxListTotalsGroupedBy = new System.Windows.Forms.CheckBox();
            this.pictureBox20 = new System.Windows.Forms.PictureBox();
            this.CbxUnbound = new System.Windows.Forms.CheckBox();
            this.pictureBox29 = new System.Windows.Forms.PictureBox();
            this.CbxRecordDetails = new System.Windows.Forms.CheckBox();
            this.CbxUpdateRecord = new System.Windows.Forms.CheckBox();
            this.pictureBox16 = new System.Windows.Forms.PictureBox();
            this.CbxListTotals = new System.Windows.Forms.CheckBox();
            this.CbxListCrud = new System.Windows.Forms.CheckBox();
            this.pictureBox26 = new System.Windows.Forms.PictureBox();
            this.pictureBox22 = new System.Windows.Forms.PictureBox();
            this.CbxListGroupedBy = new System.Windows.Forms.CheckBox();
            this.CbxAddNewRecord = new System.Windows.Forms.CheckBox();
            this.CbxListReadOnly = new System.Windows.Forms.CheckBox();
            this.CbxListCrudRedirect = new System.Windows.Forms.CheckBox();
            this.TabAppSettings = new System.Windows.Forms.TabPage();
            this.pictureBox66 = new System.Windows.Forms.PictureBox();
            this.GbxOverwriteWebApiFiles = new System.Windows.Forms.GroupBox();
            this.pictureBox64 = new System.Windows.Forms.PictureBox();
            this.CbxOverwriteStartUpClassWebApi = new System.Windows.Forms.CheckBox();
            this.CbxOverwriteProgramClassWebApi = new System.Windows.Forms.CheckBox();
            this.pictureBox63 = new System.Windows.Forms.PictureBox();
            this.CbxOverwriteAppSettingsJsonWebApi = new System.Windows.Forms.CheckBox();
            this.pictureBox62 = new System.Windows.Forms.PictureBox();
            this.CbxOverwriteLaunchSettingsJsonWebApi = new System.Windows.Forms.CheckBox();
            this.pictureBox33 = new System.Windows.Forms.PictureBox();
            this.GbxOverwriteBusinessDataLayerFiles = new System.Windows.Forms.GroupBox();
            this.CbxOverwriteAssemblyInfo = new System.Windows.Forms.CheckBox();
            this.pictureBox40 = new System.Windows.Forms.PictureBox();
            this.CbxOverwriteAppSettingsClass = new System.Windows.Forms.CheckBox();
            this.pictureBox36 = new System.Windows.Forms.PictureBox();
            this.CbxAutomaticallyOpenTab = new System.Windows.Forms.CheckBox();
            this.GbxOverwriteFiles = new System.Windows.Forms.GroupBox();
            this.CbxOverwriteStartUpClass = new System.Windows.Forms.CheckBox();
            this.CbxOverwriteFunctionsFile = new System.Windows.Forms.CheckBox();
            this.CbxOverwriteSiteCss = new System.Windows.Forms.CheckBox();
            this.pictureBox53 = new System.Windows.Forms.PictureBox();
            this.pictureBox35 = new System.Windows.Forms.PictureBox();
            this.CbxOverwriteViewStartPage = new System.Windows.Forms.CheckBox();
            this.pictureBox23 = new System.Windows.Forms.PictureBox();
            this.pictureBox18 = new System.Windows.Forms.PictureBox();
            this.CbxOverwriteProgramClass = new System.Windows.Forms.CheckBox();
            this.CbxOverwriteLayoutPage = new System.Windows.Forms.CheckBox();
            this.pictureBox52 = new System.Windows.Forms.PictureBox();
            this.pictureBox12 = new System.Windows.Forms.PictureBox();
            this.CbxOverwriteAppSettingsJson = new System.Windows.Forms.CheckBox();
            this.pictureBox31 = new System.Windows.Forms.PictureBox();
            this.pictureBox45 = new System.Windows.Forms.PictureBox();
            this.pictureBox43 = new System.Windows.Forms.PictureBox();
            this.pictureBox32 = new System.Windows.Forms.PictureBox();
            this.CbxOverwriteLaunchSettingsJson = new System.Windows.Forms.CheckBox();
            this.pictureBox44 = new System.Windows.Forms.PictureBox();
            this.pictureBox28 = new System.Windows.Forms.PictureBox();
            this.CbxOverwriteViewImportsView = new System.Windows.Forms.CheckBox();
            this.CbxOverwriteBowerJson = new System.Windows.Forms.CheckBox();
            this.CbxOverwriteBundleConfigJson = new System.Windows.Forms.CheckBox();
            this.CbxOverwriteValidationScriptsPartialView = new System.Windows.Forms.CheckBox();
            this.BtnRestoreAllSettings = new System.Windows.Forms.Button();
            this.TxtAppFilesDirectory = new System.Windows.Forms.TextBox();
            this.BtnBrowseAppDirectory = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.pictureBox38 = new System.Windows.Forms.PictureBox();
            this.pictureBox8 = new System.Windows.Forms.PictureBox();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.pictureBox42 = new System.Windows.Forms.PictureBox();
            this.PicbUserName = new System.Windows.Forms.PictureBox();
            this.PicbPassword = new System.Windows.Forms.PictureBox();
            this.PicbShowPassword = new System.Windows.Forms.PictureBox();
            this.PicbDatabaseName = new System.Windows.Forms.PictureBox();
            this.CbxRememberPassword = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtServer = new System.Windows.Forms.TextBox();
            this.TxtDatabase = new System.Windows.Forms.TextBox();
            this.TxtUserName = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.pictureBox9 = new System.Windows.Forms.PictureBox();
            this.RbtnGenerateFromSelectedViews = new System.Windows.Forms.RadioButton();
            this.RbtnGenerateFromSelectedTables = new System.Windows.Forms.RadioButton();
            this.RbtnGenerateFromAllViews = new System.Windows.Forms.RadioButton();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pictureBox55 = new System.Windows.Forms.PictureBox();
            this.pictureBox15 = new System.Windows.Forms.PictureBox();
            this.pictureBox13 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox57 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox68 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox56 = new System.Windows.Forms.PictureBox();
            this.pictureBox47 = new System.Windows.Forms.PictureBox();
            this.pictureBox61 = new System.Windows.Forms.PictureBox();
            this.pictureBox60 = new System.Windows.Forms.PictureBox();
            this.pictureBox41 = new System.Windows.Forms.PictureBox();
            this.pictureBox14 = new System.Windows.Forms.PictureBox();
            this.pictureBox58 = new System.Windows.Forms.PictureBox();
            this.pictureBox59 = new System.Windows.Forms.PictureBox();
            this.pictureBox67 = new System.Windows.Forms.PictureBox();
            this.pictureBox48 = new System.Windows.Forms.PictureBox();
            this.pictureBox37 = new System.Windows.Forms.PictureBox();
            this.pictureBox30 = new System.Windows.Forms.PictureBox();
            this.pictureBox25 = new System.Windows.Forms.PictureBox();
            this.pictureBox65 = new System.Windows.Forms.PictureBox();
            this.pictureBox70 = new System.Windows.Forms.PictureBox();
            this.pictureBox69 = new System.Windows.Forms.PictureBox();
            this.pictureBox71 = new System.Windows.Forms.PictureBox();
            this.PicBoxShowPassword = new System.Windows.Forms.PictureBox();
            this.pictureBox78 = new System.Windows.Forms.PictureBox();
            this.pictureBox79 = new System.Windows.Forms.PictureBox();
            this.pictureBox80 = new System.Windows.Forms.PictureBox();
            this.pictureBox81 = new System.Windows.Forms.PictureBox();
            this.pictureBox82 = new System.Windows.Forms.PictureBox();
            this.pictureBox83 = new System.Windows.Forms.PictureBox();
            this.pictureBox46 = new System.Windows.Forms.PictureBox();
            this.LblProgress = new System.Windows.Forms.Label();
            this.BackgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.BtnClose = new System.Windows.Forms.Button();
            this.TxtPassword = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.TxtAPINameDirectory = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.LblLanguage = new System.Windows.Forms.Label();
            this.CbxLanguage = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TxtAPIName = new System.Windows.Forms.TextBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.RbtnUseLinqToEntities = new System.Windows.Forms.RadioButton();
            this.RbtnSqlScriptAutomatic = new System.Windows.Forms.RadioButton();
            this.RbtnSqlScript2008 = new System.Windows.Forms.RadioButton();
            this.RbtnNoPrefixOrSuffix = new System.Windows.Forms.RadioButton();
            this.RbtnSpSuffix = new System.Windows.Forms.RadioButton();
            this.RbtnSpPrefix = new System.Windows.Forms.RadioButton();
            this.TxtSpSuffix = new System.Windows.Forms.TextBox();
            this.TxtSpPrefix = new System.Windows.Forms.TextBox();
            this.RbtnUseAdHocSql = new System.Windows.Forms.RadioButton();
            this.RbtnUseStoredProc = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.BtnGenerateCode = new System.Windows.Forms.Button();
            this.TabGeneratedCode = new System.Windows.Forms.TabPage();
            this.GbxApplication = new System.Windows.Forms.GroupBox();
            this.CbxUseWebApi = new System.Windows.Forms.CheckBox();
            this.CbxUseWebApplication = new System.Windows.Forms.CheckBox();
            this.GbxWebApi = new System.Windows.Forms.GroupBox();
            this.BtnBrowseWebAPIDirectory = new System.Windows.Forms.Button();
            this.TxtWebAPINameDirectory = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.TxtWebAPIName = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.CbxGenerateCodeExamples = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.TxtDevServerPort = new System.Windows.Forms.TextBox();
            this.TxtDirectory = new System.Windows.Forms.TextBox();
            this.BtnBrowseCodeDirectory = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.TxtWebAppName = new System.Windows.Forms.TextBox();
            this.GbxGeneratedSqlScript = new System.Windows.Forms.GroupBox();
            this.TabControl1 = new System.Windows.Forms.TabControl();
            this.TabSelectedTables = new System.Windows.Forms.TabPage();
            this.BtnClearSelectedTables = new System.Windows.Forms.Button();
            this.BtnLoadTables = new System.Windows.Forms.Button();
            this.ClbxTables = new System.Windows.Forms.CheckedListBox();
            this.TabSelectedViews = new System.Windows.Forms.TabPage();
            this.BtnClearSelectedViews = new System.Windows.Forms.Button();
            this.BtnLoadViews = new System.Windows.Forms.Button();
            this.ClbxViews = new System.Windows.Forms.CheckedListBox();
            this.TabDatabase = new System.Windows.Forms.TabPage();
            this.GbxGenerateSql = new System.Windows.Forms.GroupBox();
            this.GbxStoredProcedure = new System.Windows.Forms.GroupBox();
            this.GboxStoredProcOrLinq = new System.Windows.Forms.GroupBox();
            this.TabCompSettings = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabComponents = new System.Windows.Forms.TabPage();
            this.GbxConfiguration = new System.Windows.Forms.GroupBox();
            this.GbxSMTPSettings = new System.Windows.Forms.GroupBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.btnTestCionnection = new System.Windows.Forms.Button();
            this.CbxShowPassword = new System.Windows.Forms.CheckBox();
            this.pictureBox77 = new System.Windows.Forms.PictureBox();
            this.pictureBox76 = new System.Windows.Forms.PictureBox();
            this.pictureBox75 = new System.Windows.Forms.PictureBox();
            this.pictureBox74 = new System.Windows.Forms.PictureBox();
            this.TxtPasswordForSmtp = new System.Windows.Forms.TextBox();
            this.TxtUserNameForSmtp = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.CbxEmailNotification = new System.Windows.Forms.CheckBox();
            this.pictureBox72 = new System.Windows.Forms.PictureBox();
            this.GbxAuditLogging = new System.Windows.Forms.GroupBox();
            this.CbxUseAuditLogging = new System.Windows.Forms.CheckBox();
            this.GbxLogging = new System.Windows.Forms.GroupBox();
            this.GbxLoggingType = new System.Windows.Forms.GroupBox();
            this.CbxUseEventLogging = new System.Windows.Forms.CheckBox();
            this.CbxUseDatabaseLogging = new System.Windows.Forms.CheckBox();
            this.CbxUseFileLogging = new System.Windows.Forms.CheckBox();
            this.CbxUseLogging = new System.Windows.Forms.CheckBox();
            this.GbxCaching = new System.Windows.Forms.GroupBox();
            this.CbxUseCaching = new System.Windows.Forms.CheckBox();
            this.GbxSecurity = new System.Windows.Forms.GroupBox();
            this.CbxUseSecurity = new System.Windows.Forms.CheckBox();
            this.tabWorkflow = new System.Windows.Forms.TabPage();
            this.gbxWorkflowSettings = new System.Windows.Forms.GroupBox();
            this.rdoDefaultWorkflows = new System.Windows.Forms.RadioButton();
            this.rdoConfigurableWorkflows = new System.Windows.Forms.RadioButton();
            this.btnCancelWorkflow = new System.Windows.Forms.Button();
            this.btnFinalize = new System.Windows.Forms.Button();
            this.lblNumberofWorkflows = new System.Windows.Forms.Label();
            this.grpExceptionFlow = new System.Windows.Forms.GroupBox();
            this.lblFromStep = new System.Windows.Forms.Label();
            this.cmbToStep = new System.Windows.Forms.ComboBox();
            this.lblToStep = new System.Windows.Forms.Label();
            this.cmbFromStep = new System.Windows.Forms.ComboBox();
            this.txtEscalationTime = new System.Windows.Forms.TextBox();
            this.txtNumberofWorflows = new System.Windows.Forms.TextBox();
            this.lbEscalationTime = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lblWorkflowName = new System.Windows.Forms.Label();
            this.txtNoOfSteps = new System.Windows.Forms.TextBox();
            this.txtWorkflowName = new System.Windows.Forms.TextBox();
            this.lblNoOfSteps = new System.Windows.Forms.Label();
            this.CbxWorkflow = new System.Windows.Forms.CheckBox();
            this.pictureBox73 = new System.Windows.Forms.PictureBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            this.TabUISettings.SuspendLayout();
            this.GbxViewNames.SuspendLayout();
            this.GbxThemes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox19)).BeginInit();
            this.GbxWebFormsToGenerate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox84)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox54)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox49)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox34)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox51)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox50)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox39)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox21)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox17)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox24)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox27)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox20)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox29)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox16)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox26)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox22)).BeginInit();
            this.TabAppSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox66)).BeginInit();
            this.GbxOverwriteWebApiFiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox64)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox63)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox62)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox33)).BeginInit();
            this.GbxOverwriteBusinessDataLayerFiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox40)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox36)).BeginInit();
            this.GbxOverwriteFiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox53)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox35)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox23)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox18)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox52)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox31)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox45)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox43)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox32)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox44)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox28)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox38)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox42)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicbUserName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicbPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicbShowPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicbDatabaseName)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox55)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox15)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox57)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox68)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox56)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox47)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox61)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox60)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox41)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox58)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox59)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox67)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox48)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox37)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox30)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox25)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox65)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox70)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox69)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox71)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBoxShowPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox78)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox79)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox80)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox81)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox82)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox83)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox46)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.TabGeneratedCode.SuspendLayout();
            this.GbxApplication.SuspendLayout();
            this.GbxWebApi.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.GbxGeneratedSqlScript.SuspendLayout();
            this.TabControl1.SuspendLayout();
            this.TabSelectedTables.SuspendLayout();
            this.TabSelectedViews.SuspendLayout();
            this.TabDatabase.SuspendLayout();
            this.GbxGenerateSql.SuspendLayout();
            this.GbxStoredProcedure.SuspendLayout();
            this.GboxStoredProcOrLinq.SuspendLayout();
            this.TabCompSettings.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabComponents.SuspendLayout();
            this.GbxConfiguration.SuspendLayout();
            this.GbxSMTPSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox77)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox76)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox75)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox74)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox72)).BeginInit();
            this.GbxAuditLogging.SuspendLayout();
            this.GbxLogging.SuspendLayout();
            this.GbxLoggingType.SuspendLayout();
            this.GbxCaching.SuspendLayout();
            this.GbxSecurity.SuspendLayout();
            this.tabWorkflow.SuspendLayout();
            this.gbxWorkflowSettings.SuspendLayout();
            this.grpExceptionFlow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox73)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox7
            // 
            this.pictureBox7.ErrorImage = null;
            this.pictureBox7.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox7.InitialImage = null;
            this.pictureBox7.Location = new System.Drawing.Point(13, 90);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(16, 16);
            this.pictureBox7.TabIndex = 28;
            this.pictureBox7.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox7, "Genetates models, views, controllers and code for selected views you choose from " +
        "the Selected Views tab.    Note: Generates List (Read Only) views under UI Setti" +
        "ngs.");
            // 
            // RbtnGenerateFromAllTables
            // 
            this.RbtnGenerateFromAllTables.AutoSize = true;
            this.RbtnGenerateFromAllTables.Checked = true;
            this.RbtnGenerateFromAllTables.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.RbtnGenerateFromAllTables.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.RbtnGenerateFromAllTables.Location = new System.Drawing.Point(37, 21);
            this.RbtnGenerateFromAllTables.Name = "RbtnGenerateFromAllTables";
            this.RbtnGenerateFromAllTables.Size = new System.Drawing.Size(71, 17);
            this.RbtnGenerateFromAllTables.TabIndex = 0;
            this.RbtnGenerateFromAllTables.TabStop = true;
            this.RbtnGenerateFromAllTables.Text = "All Tables";
            this.RbtnGenerateFromAllTables.UseVisualStyleBackColor = true;
            this.RbtnGenerateFromAllTables.CheckedChanged += new System.EventHandler(this.RbtnGenerateFromAllTables_CheckedChanged);
            // 
            // TabUISettings
            // 
            this.TabUISettings.Controls.Add(this.GbxViewNames);
            this.TabUISettings.Controls.Add(this.GbxThemes);
            this.TabUISettings.Controls.Add(this.GbxWebFormsToGenerate);
            this.TabUISettings.Location = new System.Drawing.Point(4, 22);
            this.TabUISettings.Name = "TabUISettings";
            this.TabUISettings.Size = new System.Drawing.Size(600, 547);
            this.TabUISettings.TabIndex = 4;
            this.TabUISettings.Text = "UI Settings";
            this.TabUISettings.UseVisualStyleBackColor = true;
            // 
            // GbxViewNames
            // 
            this.GbxViewNames.Controls.Add(this.TxtWorkflowAssignSteps);
            this.GbxViewNames.Controls.Add(this.TxtListMasterDetailSubGrid);
            this.GbxViewNames.Controls.Add(this.TxtListMasterDetailGrid);
            this.GbxViewNames.Controls.Add(this.TxtListForeach);
            this.GbxViewNames.Controls.Add(this.TxtListScrollLoad);
            this.GbxViewNames.Controls.Add(this.TxtListInline);
            this.GbxViewNames.Controls.Add(this.TxtListTotals);
            this.GbxViewNames.Controls.Add(this.TxtListTotalsGroupedBy);
            this.GbxViewNames.Controls.Add(this.TxtUnbound);
            this.GbxViewNames.Controls.Add(this.TxtListSearch);
            this.GbxViewNames.Controls.Add(this.TxtListCrudRedirect);
            this.GbxViewNames.Controls.Add(this.TxtAdd);
            this.GbxViewNames.Controls.Add(this.TxtUpdate);
            this.GbxViewNames.Controls.Add(this.TxtDetails);
            this.GbxViewNames.Controls.Add(this.TxtListReadOnly);
            this.GbxViewNames.Controls.Add(this.TxtListCrud);
            this.GbxViewNames.Controls.Add(this.TxtListGroupedBy);
            this.GbxViewNames.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.GbxViewNames.Location = new System.Drawing.Point(300, 91);
            this.GbxViewNames.Name = "GbxViewNames";
            this.GbxViewNames.Size = new System.Drawing.Size(194, 433);
            this.GbxViewNames.TabIndex = 29;
            this.GbxViewNames.TabStop = false;
            this.GbxViewNames.Text = "View Names";
            // 
            // TxtWorkflowAssignSteps
            // 
            this.TxtWorkflowAssignSteps.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.TxtWorkflowAssignSteps.Location = new System.Drawing.Point(9, 404);
            this.TxtWorkflowAssignSteps.Name = "TxtWorkflowAssignSteps";
            this.TxtWorkflowAssignSteps.Size = new System.Drawing.Size(172, 20);
            this.TxtWorkflowAssignSteps.TabIndex = 82;
            this.TxtWorkflowAssignSteps.Text = "AssignWorkflowSteps";
            // 
            // TxtListMasterDetailSubGrid
            // 
            this.TxtListMasterDetailSubGrid.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.TxtListMasterDetailSubGrid.Location = new System.Drawing.Point(9, 355);
            this.TxtListMasterDetailSubGrid.Name = "TxtListMasterDetailSubGrid";
            this.TxtListMasterDetailSubGrid.Size = new System.Drawing.Size(172, 20);
            this.TxtListMasterDetailSubGrid.TabIndex = 81;
            this.TxtListMasterDetailSubGrid.Text = "ListMasterDetailSubGridBy";
            // 
            // TxtListMasterDetailGrid
            // 
            this.TxtListMasterDetailGrid.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.TxtListMasterDetailGrid.Location = new System.Drawing.Point(9, 331);
            this.TxtListMasterDetailGrid.Name = "TxtListMasterDetailGrid";
            this.TxtListMasterDetailGrid.Size = new System.Drawing.Size(172, 20);
            this.TxtListMasterDetailGrid.TabIndex = 80;
            this.TxtListMasterDetailGrid.Text = "ListMasterDetailGridBy";
            // 
            // TxtListForeach
            // 
            this.TxtListForeach.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.TxtListForeach.Location = new System.Drawing.Point(9, 308);
            this.TxtListForeach.Name = "TxtListForeach";
            this.TxtListForeach.Size = new System.Drawing.Size(172, 20);
            this.TxtListForeach.TabIndex = 79;
            this.TxtListForeach.Text = "ListForeach";
            // 
            // TxtListScrollLoad
            // 
            this.TxtListScrollLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.TxtListScrollLoad.Location = new System.Drawing.Point(9, 261);
            this.TxtListScrollLoad.Name = "TxtListScrollLoad";
            this.TxtListScrollLoad.Size = new System.Drawing.Size(172, 20);
            this.TxtListScrollLoad.TabIndex = 78;
            this.TxtListScrollLoad.Text = "ListScrollLoad";
            // 
            // TxtListInline
            // 
            this.TxtListInline.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.TxtListInline.Location = new System.Drawing.Point(9, 285);
            this.TxtListInline.Name = "TxtListInline";
            this.TxtListInline.Size = new System.Drawing.Size(172, 20);
            this.TxtListInline.TabIndex = 77;
            this.TxtListInline.Text = "ListInline";
            // 
            // TxtListTotals
            // 
            this.TxtListTotals.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.TxtListTotals.Location = new System.Drawing.Point(9, 190);
            this.TxtListTotals.Name = "TxtListTotals";
            this.TxtListTotals.Size = new System.Drawing.Size(172, 20);
            this.TxtListTotals.TabIndex = 76;
            this.TxtListTotals.Text = "ListTotals";
            // 
            // TxtListTotalsGroupedBy
            // 
            this.TxtListTotalsGroupedBy.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.TxtListTotalsGroupedBy.Location = new System.Drawing.Point(9, 214);
            this.TxtListTotalsGroupedBy.Name = "TxtListTotalsGroupedBy";
            this.TxtListTotalsGroupedBy.Size = new System.Drawing.Size(172, 20);
            this.TxtListTotalsGroupedBy.TabIndex = 73;
            this.TxtListTotalsGroupedBy.Text = "ListTotalsGroupedBy";
            // 
            // TxtUnbound
            // 
            this.TxtUnbound.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.TxtUnbound.Location = new System.Drawing.Point(9, 378);
            this.TxtUnbound.Name = "TxtUnbound";
            this.TxtUnbound.Size = new System.Drawing.Size(172, 20);
            this.TxtUnbound.TabIndex = 72;
            this.TxtUnbound.Text = "Unbound";
            // 
            // TxtListSearch
            // 
            this.TxtListSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.TxtListSearch.Location = new System.Drawing.Point(9, 237);
            this.TxtListSearch.Name = "TxtListSearch";
            this.TxtListSearch.Size = new System.Drawing.Size(172, 20);
            this.TxtListSearch.TabIndex = 75;
            this.TxtListSearch.Text = "ListSearch";
            // 
            // TxtListCrudRedirect
            // 
            this.TxtListCrudRedirect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.TxtListCrudRedirect.Location = new System.Drawing.Point(9, 30);
            this.TxtListCrudRedirect.Name = "TxtListCrudRedirect";
            this.TxtListCrudRedirect.Size = new System.Drawing.Size(172, 20);
            this.TxtListCrudRedirect.TabIndex = 63;
            this.TxtListCrudRedirect.Text = "ListCrudRedirect";
            // 
            // TxtAdd
            // 
            this.TxtAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.TxtAdd.Location = new System.Drawing.Point(9, 54);
            this.TxtAdd.Name = "TxtAdd";
            this.TxtAdd.Size = new System.Drawing.Size(172, 20);
            this.TxtAdd.TabIndex = 64;
            this.TxtAdd.Text = "Add";
            // 
            // TxtUpdate
            // 
            this.TxtUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.TxtUpdate.Location = new System.Drawing.Point(9, 76);
            this.TxtUpdate.Name = "TxtUpdate";
            this.TxtUpdate.Size = new System.Drawing.Size(172, 20);
            this.TxtUpdate.TabIndex = 71;
            this.TxtUpdate.Text = "Update";
            // 
            // TxtDetails
            // 
            this.TxtDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.TxtDetails.Location = new System.Drawing.Point(9, 100);
            this.TxtDetails.Name = "TxtDetails";
            this.TxtDetails.Size = new System.Drawing.Size(172, 20);
            this.TxtDetails.TabIndex = 65;
            this.TxtDetails.Text = "Details";
            // 
            // TxtListReadOnly
            // 
            this.TxtListReadOnly.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.TxtListReadOnly.Location = new System.Drawing.Point(9, 122);
            this.TxtListReadOnly.Name = "TxtListReadOnly";
            this.TxtListReadOnly.Size = new System.Drawing.Size(172, 20);
            this.TxtListReadOnly.TabIndex = 66;
            this.TxtListReadOnly.Text = "ListReadOnly";
            // 
            // TxtListCrud
            // 
            this.TxtListCrud.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.TxtListCrud.Location = new System.Drawing.Point(9, 145);
            this.TxtListCrud.Name = "TxtListCrud";
            this.TxtListCrud.Size = new System.Drawing.Size(172, 20);
            this.TxtListCrud.TabIndex = 67;
            this.TxtListCrud.Text = "ListCrud";
            // 
            // TxtListGroupedBy
            // 
            this.TxtListGroupedBy.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.TxtListGroupedBy.Location = new System.Drawing.Point(9, 168);
            this.TxtListGroupedBy.Name = "TxtListGroupedBy";
            this.TxtListGroupedBy.Size = new System.Drawing.Size(172, 20);
            this.TxtListGroupedBy.TabIndex = 68;
            this.TxtListGroupedBy.Text = "ListGroupedBy";
            // 
            // GbxThemes
            // 
            this.GbxThemes.Controls.Add(this.pictureBox19);
            this.GbxThemes.Controls.Add(this.CbxJQueryUITheme);
            this.GbxThemes.Controls.Add(this.label10);
            this.GbxThemes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.GbxThemes.Location = new System.Drawing.Point(16, 14);
            this.GbxThemes.Name = "GbxThemes";
            this.GbxThemes.Size = new System.Drawing.Size(226, 59);
            this.GbxThemes.TabIndex = 28;
            this.GbxThemes.TabStop = false;
            this.GbxThemes.Text = "Themes";
            // 
            // pictureBox19
            // 
            this.pictureBox19.ErrorImage = null;
            this.pictureBox19.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox19.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox19.InitialImage = null;
            this.pictureBox19.Location = new System.Drawing.Point(13, 22);
            this.pictureBox19.Name = "pictureBox19";
            this.pictureBox19.Size = new System.Drawing.Size(16, 16);
            this.pictureBox19.TabIndex = 38;
            this.pictureBox19.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox19, "Theme used in the generated JQuery UI controls");
            // 
            // CbxJQueryUITheme
            // 
            this.CbxJQueryUITheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CbxJQueryUITheme.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxJQueryUITheme.FormattingEnabled = true;
            this.CbxJQueryUITheme.Items.AddRange(new object[] {
            "Black-Tie",
            "Blitzer",
            "Cupertino",
            "Dark-Hive",
            "Dot-Luv",
            "Eggplant",
            "Excite-Bike",
            "Hot-Sneaks",
            "Humanity",
            "Le-Frog",
            "Mint-Choc",
            "Overcast",
            "Pepper-Grinder",
            "Redmond",
            "Smoothness",
            "South-Street",
            "Start",
            "Sunny",
            "Swanky-Purse",
            "Trontastic",
            "UI-Darkness",
            "UI-Lightness",
            "Vader"});
            this.CbxJQueryUITheme.Location = new System.Drawing.Point(116, 21);
            this.CbxJQueryUITheme.Name = "CbxJQueryUITheme";
            this.CbxJQueryUITheme.Size = new System.Drawing.Size(98, 21);
            this.CbxJQueryUITheme.TabIndex = 29;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label10.Location = new System.Drawing.Point(38, 24);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(57, 13);
            this.label10.TabIndex = 30;
            this.label10.Text = "JQuery UI:";
            // 
            // GbxWebFormsToGenerate
            // 
            this.GbxWebFormsToGenerate.Controls.Add(this.CbxWorkflowAssignSteps);
            this.GbxWebFormsToGenerate.Controls.Add(this.pictureBox84);
            this.GbxWebFormsToGenerate.Controls.Add(this.CbxListMasterDetailSubGrid);
            this.GbxWebFormsToGenerate.Controls.Add(this.CbxListMasterDetailGrid);
            this.GbxWebFormsToGenerate.Controls.Add(this.pictureBox54);
            this.GbxWebFormsToGenerate.Controls.Add(this.pictureBox49);
            this.GbxWebFormsToGenerate.Controls.Add(this.CbxListForeach);
            this.GbxWebFormsToGenerate.Controls.Add(this.pictureBox34);
            this.GbxWebFormsToGenerate.Controls.Add(this.CbxListScrollLoad);
            this.GbxWebFormsToGenerate.Controls.Add(this.pictureBox51);
            this.GbxWebFormsToGenerate.Controls.Add(this.CbxListInline);
            this.GbxWebFormsToGenerate.Controls.Add(this.pictureBox50);
            this.GbxWebFormsToGenerate.Controls.Add(this.pictureBox39);
            this.GbxWebFormsToGenerate.Controls.Add(this.pictureBox21);
            this.GbxWebFormsToGenerate.Controls.Add(this.pictureBox11);
            this.GbxWebFormsToGenerate.Controls.Add(this.pictureBox10);
            this.GbxWebFormsToGenerate.Controls.Add(this.pictureBox17);
            this.GbxWebFormsToGenerate.Controls.Add(this.pictureBox24);
            this.GbxWebFormsToGenerate.Controls.Add(this.pictureBox27);
            this.GbxWebFormsToGenerate.Controls.Add(this.CbxListSearch);
            this.GbxWebFormsToGenerate.Controls.Add(this.CbxListTotalsGroupedBy);
            this.GbxWebFormsToGenerate.Controls.Add(this.pictureBox20);
            this.GbxWebFormsToGenerate.Controls.Add(this.CbxUnbound);
            this.GbxWebFormsToGenerate.Controls.Add(this.pictureBox29);
            this.GbxWebFormsToGenerate.Controls.Add(this.CbxRecordDetails);
            this.GbxWebFormsToGenerate.Controls.Add(this.CbxUpdateRecord);
            this.GbxWebFormsToGenerate.Controls.Add(this.pictureBox16);
            this.GbxWebFormsToGenerate.Controls.Add(this.CbxListTotals);
            this.GbxWebFormsToGenerate.Controls.Add(this.CbxListCrud);
            this.GbxWebFormsToGenerate.Controls.Add(this.pictureBox26);
            this.GbxWebFormsToGenerate.Controls.Add(this.pictureBox22);
            this.GbxWebFormsToGenerate.Controls.Add(this.CbxListGroupedBy);
            this.GbxWebFormsToGenerate.Controls.Add(this.CbxAddNewRecord);
            this.GbxWebFormsToGenerate.Controls.Add(this.CbxListReadOnly);
            this.GbxWebFormsToGenerate.Controls.Add(this.CbxListCrudRedirect);
            this.GbxWebFormsToGenerate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.GbxWebFormsToGenerate.Location = new System.Drawing.Point(16, 91);
            this.GbxWebFormsToGenerate.Name = "GbxWebFormsToGenerate";
            this.GbxWebFormsToGenerate.Size = new System.Drawing.Size(278, 433);
            this.GbxWebFormsToGenerate.TabIndex = 27;
            this.GbxWebFormsToGenerate.TabStop = false;
            this.GbxWebFormsToGenerate.Text = "        Views to Generate";
            // 
            // CbxWorkflowAssignSteps
            // 
            this.CbxWorkflowAssignSteps.AutoSize = true;
            this.CbxWorkflowAssignSteps.Checked = true;
            this.CbxWorkflowAssignSteps.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxWorkflowAssignSteps.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxWorkflowAssignSteps.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.CbxWorkflowAssignSteps.Location = new System.Drawing.Point(40, 401);
            this.CbxWorkflowAssignSteps.Name = "CbxWorkflowAssignSteps";
            this.CbxWorkflowAssignSteps.Size = new System.Drawing.Size(155, 17);
            this.CbxWorkflowAssignSteps.TabIndex = 93;
            this.CbxWorkflowAssignSteps.Text = "AssignWorkflowSteps View";
            this.CbxWorkflowAssignSteps.UseVisualStyleBackColor = true;
            this.CbxWorkflowAssignSteps.CheckedChanged += new System.EventHandler(this.CbxWorkflowAssignSteps_CheckedChanged);
            // 
            // pictureBox84
            // 
            this.pictureBox84.ErrorImage = null;
            this.pictureBox84.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox84.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox84.InitialImage = null;
            this.pictureBox84.Location = new System.Drawing.Point(12, 399);
            this.pictureBox84.Name = "pictureBox84";
            this.pictureBox84.Size = new System.Drawing.Size(16, 16);
            this.pictureBox84.TabIndex = 92;
            this.pictureBox84.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox84, "Plain MVC views that are not bound to the database.  Can be used for adding or up" +
        "dating records, just enter your own logic.");
            // 
            // CbxListMasterDetailSubGrid
            // 
            this.CbxListMasterDetailSubGrid.AutoSize = true;
            this.CbxListMasterDetailSubGrid.Checked = true;
            this.CbxListMasterDetailSubGrid.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxListMasterDetailSubGrid.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxListMasterDetailSubGrid.Location = new System.Drawing.Point(40, 355);
            this.CbxListMasterDetailSubGrid.Name = "CbxListMasterDetailSubGrid";
            this.CbxListMasterDetailSubGrid.Size = new System.Drawing.Size(179, 17);
            this.CbxListMasterDetailSubGrid.TabIndex = 91;
            this.CbxListMasterDetailSubGrid.Text = "List with Master Detail (Sub Grid)";
            this.CbxListMasterDetailSubGrid.UseVisualStyleBackColor = true;
            this.CbxListMasterDetailSubGrid.CheckedChanged += new System.EventHandler(this.CbxListMasterDetailSubGrid_CheckedChanged);
            // 
            // CbxListMasterDetailGrid
            // 
            this.CbxListMasterDetailGrid.AutoSize = true;
            this.CbxListMasterDetailGrid.Checked = true;
            this.CbxListMasterDetailGrid.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxListMasterDetailGrid.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxListMasterDetailGrid.Location = new System.Drawing.Point(40, 332);
            this.CbxListMasterDetailGrid.Name = "CbxListMasterDetailGrid";
            this.CbxListMasterDetailGrid.Size = new System.Drawing.Size(157, 17);
            this.CbxListMasterDetailGrid.TabIndex = 90;
            this.CbxListMasterDetailGrid.Text = "List with Master Detail (Grid)";
            this.CbxListMasterDetailGrid.UseVisualStyleBackColor = true;
            this.CbxListMasterDetailGrid.CheckedChanged += new System.EventHandler(this.CbxListMasterDetailGrid_CheckedChanged);
            // 
            // pictureBox54
            // 
            this.pictureBox54.ErrorImage = null;
            this.pictureBox54.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox54.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox54.InitialImage = null;
            this.pictureBox54.Location = new System.Drawing.Point(12, 355);
            this.pictureBox54.Name = "pictureBox54";
            this.pictureBox54.Size = new System.Drawing.Size(16, 16);
            this.pictureBox54.TabIndex = 89;
            this.pictureBox54.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox54, "Two views, one as a Sub Grid (detail ) of the Main Grid (master) with Master-Deta" +
        "il relationship.  No CRUD.");
            // 
            // pictureBox49
            // 
            this.pictureBox49.ErrorImage = null;
            this.pictureBox49.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox49.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox49.InitialImage = null;
            this.pictureBox49.Location = new System.Drawing.Point(12, 332);
            this.pictureBox49.Name = "pictureBox49";
            this.pictureBox49.Size = new System.Drawing.Size(16, 16);
            this.pictureBox49.TabIndex = 88;
            this.pictureBox49.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox49, "Two views on top of each that has a list of items with Master-Detail relationship" +
        ".  No CRUD.");
            // 
            // CbxListForeach
            // 
            this.CbxListForeach.AutoSize = true;
            this.CbxListForeach.Checked = true;
            this.CbxListForeach.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxListForeach.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxListForeach.Location = new System.Drawing.Point(40, 308);
            this.CbxListForeach.Name = "CbxListForeach";
            this.CbxListForeach.Size = new System.Drawing.Size(144, 17);
            this.CbxListForeach.TabIndex = 87;
            this.CbxListForeach.Text = "List with Manual Foreach";
            this.CbxListForeach.UseVisualStyleBackColor = true;
            this.CbxListForeach.CheckedChanged += new System.EventHandler(this.CbxListForeach_CheckedChanged);
            // 
            // pictureBox34
            // 
            this.pictureBox34.ErrorImage = null;
            this.pictureBox34.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox34.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox34.InitialImage = null;
            this.pictureBox34.Location = new System.Drawing.Point(12, 308);
            this.pictureBox34.Name = "pictureBox34";
            this.pictureBox34.Size = new System.Drawing.Size(16, 16);
            this.pictureBox34.TabIndex = 86;
            this.pictureBox34.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox34, "Views that has a list of items.  Rather than using the JQGrid plugin, code manual" +
        "ly fills the list using a Foreach Loop.  No CRUD functionality.");
            // 
            // CbxListScrollLoad
            // 
            this.CbxListScrollLoad.AutoSize = true;
            this.CbxListScrollLoad.Checked = true;
            this.CbxListScrollLoad.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxListScrollLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxListScrollLoad.Location = new System.Drawing.Point(40, 260);
            this.CbxListScrollLoad.Name = "CbxListScrollLoad";
            this.CbxListScrollLoad.Size = new System.Drawing.Size(124, 17);
            this.CbxListScrollLoad.TabIndex = 85;
            this.CbxListScrollLoad.Text = "List Scroll-Load Data";
            this.CbxListScrollLoad.UseVisualStyleBackColor = true;
            this.CbxListScrollLoad.CheckedChanged += new System.EventHandler(this.CbxListScrollLoad_CheckedChanged);
            // 
            // pictureBox51
            // 
            this.pictureBox51.ErrorImage = null;
            this.pictureBox51.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox51.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox51.InitialImage = null;
            this.pictureBox51.Location = new System.Drawing.Point(12, 260);
            this.pictureBox51.Name = "pictureBox51";
            this.pictureBox51.Size = new System.Drawing.Size(16, 16);
            this.pictureBox51.TabIndex = 84;
            this.pictureBox51.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox51, "Views that dynamically loads list items as you scroll on the list.  No CRUD funct" +
        "ionality. ");
            // 
            // CbxListInline
            // 
            this.CbxListInline.AutoSize = true;
            this.CbxListInline.Checked = true;
            this.CbxListInline.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxListInline.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxListInline.Location = new System.Drawing.Point(40, 285);
            this.CbxListInline.Name = "CbxListInline";
            this.CbxListInline.Size = new System.Drawing.Size(144, 17);
            this.CbxListInline.TabIndex = 83;
            this.CbxListInline.Text = "List with Inline Add && Edit";
            this.CbxListInline.UseVisualStyleBackColor = true;
            this.CbxListInline.CheckedChanged += new System.EventHandler(this.CbxListInline_CheckedChanged);
            // 
            // pictureBox50
            // 
            this.pictureBox50.ErrorImage = null;
            this.pictureBox50.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox50.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox50.InitialImage = null;
            this.pictureBox50.Location = new System.Drawing.Point(12, 285);
            this.pictureBox50.Name = "pictureBox50";
            this.pictureBox50.Size = new System.Drawing.Size(16, 16);
            this.pictureBox50.TabIndex = 82;
            this.pictureBox50.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox50, "Views that has a list of items with CRUD functionality.  Adding and updating reco" +
        "rds are done Inline.");
            // 
            // pictureBox39
            // 
            this.pictureBox39.ErrorImage = null;
            this.pictureBox39.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox39.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox39.InitialImage = null;
            this.pictureBox39.Location = new System.Drawing.Point(12, 377);
            this.pictureBox39.Name = "pictureBox39";
            this.pictureBox39.Size = new System.Drawing.Size(16, 16);
            this.pictureBox39.TabIndex = 81;
            this.pictureBox39.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox39, "Plain MVC views that are not bound to the database.  Can be used for adding or up" +
        "dating records, just enter your own logic.");
            // 
            // pictureBox21
            // 
            this.pictureBox21.ErrorImage = null;
            this.pictureBox21.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox21.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox21.InitialImage = null;
            this.pictureBox21.Location = new System.Drawing.Point(12, 236);
            this.pictureBox21.Name = "pictureBox21";
            this.pictureBox21.Size = new System.Drawing.Size(16, 16);
            this.pictureBox21.TabIndex = 80;
            this.pictureBox21.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox21, "Views that has a list of items with search functionality on the column headers.");
            // 
            // pictureBox11
            // 
            this.pictureBox11.ErrorImage = null;
            this.pictureBox11.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox11.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox11.InitialImage = null;
            this.pictureBox11.Location = new System.Drawing.Point(12, 145);
            this.pictureBox11.Name = "pictureBox11";
            this.pictureBox11.Size = new System.Drawing.Size(16, 16);
            this.pictureBox11.TabIndex = 79;
            this.pictureBox11.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox11, "Views that has a list of items with CRUD functionality.  Adding and updating reco" +
        "rds are done on the same page.");
            // 
            // pictureBox10
            // 
            this.pictureBox10.ErrorImage = null;
            this.pictureBox10.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox10.InitialImage = null;
            this.pictureBox10.Location = new System.Drawing.Point(12, 123);
            this.pictureBox10.Name = "pictureBox10";
            this.pictureBox10.Size = new System.Drawing.Size(16, 16);
            this.pictureBox10.TabIndex = 78;
            this.pictureBox10.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox10, "Views that has a list of items, No CRUD functionality.   Note: The only MVC Objec" +
        "ts generated when All Views or Selected Views Only is selected under Code Settin" +
        "gs.");
            // 
            // pictureBox17
            // 
            this.pictureBox17.ErrorImage = null;
            this.pictureBox17.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox17.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox17.InitialImage = null;
            this.pictureBox17.Location = new System.Drawing.Point(12, 79);
            this.pictureBox17.Name = "pictureBox17";
            this.pictureBox17.Size = new System.Drawing.Size(16, 16);
            this.pictureBox17.TabIndex = 77;
            this.pictureBox17.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox17, "Views that update an existing record.");
            // 
            // pictureBox24
            // 
            this.pictureBox24.ErrorImage = null;
            this.pictureBox24.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox24.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox24.InitialImage = null;
            this.pictureBox24.Location = new System.Drawing.Point(12, 32);
            this.pictureBox24.Name = "pictureBox24";
            this.pictureBox24.Size = new System.Drawing.Size(16, 16);
            this.pictureBox24.TabIndex = 76;
            this.pictureBox24.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox24, "Views that has a list of items with CRUD functionality.  Adding and updating reco" +
        "rds redirects to another view.");
            // 
            // pictureBox27
            // 
            this.pictureBox27.ErrorImage = null;
            this.pictureBox27.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox27.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox27.InitialImage = null;
            this.pictureBox27.Location = new System.Drawing.Point(12, 0);
            this.pictureBox27.Name = "pictureBox27";
            this.pictureBox27.Size = new System.Drawing.Size(16, 16);
            this.pictureBox27.TabIndex = 75;
            this.pictureBox27.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox27, "Generated for All Tables or Selected Tables only (selected under Code Settings)");
            // 
            // CbxListSearch
            // 
            this.CbxListSearch.AutoSize = true;
            this.CbxListSearch.Checked = true;
            this.CbxListSearch.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxListSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxListSearch.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.CbxListSearch.Location = new System.Drawing.Point(40, 236);
            this.CbxListSearch.Name = "CbxListSearch";
            this.CbxListSearch.Size = new System.Drawing.Size(101, 17);
            this.CbxListSearch.TabIndex = 73;
            this.CbxListSearch.Text = "List with Search";
            this.CbxListSearch.UseVisualStyleBackColor = true;
            this.CbxListSearch.CheckedChanged += new System.EventHandler(this.CbxListSearch_CheckedChanged);
            // 
            // CbxListTotalsGroupedBy
            // 
            this.CbxListTotalsGroupedBy.AutoSize = true;
            this.CbxListTotalsGroupedBy.Checked = true;
            this.CbxListTotalsGroupedBy.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxListTotalsGroupedBy.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxListTotalsGroupedBy.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.CbxListTotalsGroupedBy.Location = new System.Drawing.Point(40, 213);
            this.CbxListTotalsGroupedBy.Name = "CbxListTotalsGroupedBy";
            this.CbxListTotalsGroupedBy.Size = new System.Drawing.Size(163, 17);
            this.CbxListTotalsGroupedBy.TabIndex = 56;
            this.CbxListTotalsGroupedBy.Text = "List with Totals and Grouping";
            this.CbxListTotalsGroupedBy.UseVisualStyleBackColor = true;
            this.CbxListTotalsGroupedBy.CheckedChanged += new System.EventHandler(this.CbxListTotalsGroupedBy_CheckedChanged);
            // 
            // pictureBox20
            // 
            this.pictureBox20.ErrorImage = null;
            this.pictureBox20.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox20.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox20.InitialImage = null;
            this.pictureBox20.Location = new System.Drawing.Point(12, 213);
            this.pictureBox20.Name = "pictureBox20";
            this.pictureBox20.Size = new System.Drawing.Size(16, 16);
            this.pictureBox20.TabIndex = 55;
            this.pictureBox20.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox20, "Views that has a list of items and Totals Showing in the Footer.   Items are also" +
        " grouped together.");
            // 
            // CbxUnbound
            // 
            this.CbxUnbound.AutoSize = true;
            this.CbxUnbound.Checked = true;
            this.CbxUnbound.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxUnbound.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxUnbound.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.CbxUnbound.Location = new System.Drawing.Point(40, 378);
            this.CbxUnbound.Name = "CbxUnbound";
            this.CbxUnbound.Size = new System.Drawing.Size(96, 17);
            this.CbxUnbound.TabIndex = 53;
            this.CbxUnbound.Text = "Unbound View";
            this.CbxUnbound.UseVisualStyleBackColor = true;
            this.CbxUnbound.CheckedChanged += new System.EventHandler(this.CbxUnbound_CheckedChanged);
            // 
            // pictureBox29
            // 
            this.pictureBox29.ErrorImage = null;
            this.pictureBox29.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox29.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox29.InitialImage = null;
            this.pictureBox29.Location = new System.Drawing.Point(12, 102);
            this.pictureBox29.Name = "pictureBox29";
            this.pictureBox29.Size = new System.Drawing.Size(16, 16);
            this.pictureBox29.TabIndex = 52;
            this.pictureBox29.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox29, "Views that show the details of a specific record.  Details shown are read-only.");
            // 
            // CbxRecordDetails
            // 
            this.CbxRecordDetails.AutoSize = true;
            this.CbxRecordDetails.Checked = true;
            this.CbxRecordDetails.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxRecordDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxRecordDetails.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.CbxRecordDetails.Location = new System.Drawing.Point(40, 103);
            this.CbxRecordDetails.Name = "CbxRecordDetails";
            this.CbxRecordDetails.Size = new System.Drawing.Size(155, 17);
            this.CbxRecordDetails.TabIndex = 51;
            this.CbxRecordDetails.Text = "Record Details (Read-Only)";
            this.CbxRecordDetails.UseVisualStyleBackColor = true;
            this.CbxRecordDetails.CheckedChanged += new System.EventHandler(this.CbxRecordDetails_CheckedChanged);
            // 
            // CbxUpdateRecord
            // 
            this.CbxUpdateRecord.AutoSize = true;
            this.CbxUpdateRecord.Checked = true;
            this.CbxUpdateRecord.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxUpdateRecord.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxUpdateRecord.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.CbxUpdateRecord.Location = new System.Drawing.Point(40, 80);
            this.CbxUpdateRecord.Name = "CbxUpdateRecord";
            this.CbxUpdateRecord.Size = new System.Drawing.Size(99, 17);
            this.CbxUpdateRecord.TabIndex = 48;
            this.CbxUpdateRecord.Text = "Update Record";
            this.CbxUpdateRecord.UseVisualStyleBackColor = true;
            this.CbxUpdateRecord.CheckedChanged += new System.EventHandler(this.CbxUpdateRecord_CheckedChanged);
            // 
            // pictureBox16
            // 
            this.pictureBox16.ErrorImage = null;
            this.pictureBox16.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox16.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox16.InitialImage = null;
            this.pictureBox16.Location = new System.Drawing.Point(12, 190);
            this.pictureBox16.Name = "pictureBox16";
            this.pictureBox16.Size = new System.Drawing.Size(16, 16);
            this.pictureBox16.TabIndex = 47;
            this.pictureBox16.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox16, "Views that has a list of items and Totals Showing in the Footer.  Number of recor" +
        "ds is also visible.");
            // 
            // CbxListTotals
            // 
            this.CbxListTotals.AutoSize = true;
            this.CbxListTotals.Checked = true;
            this.CbxListTotals.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxListTotals.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxListTotals.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.CbxListTotals.Location = new System.Drawing.Point(40, 191);
            this.CbxListTotals.Name = "CbxListTotals";
            this.CbxListTotals.Size = new System.Drawing.Size(96, 17);
            this.CbxListTotals.TabIndex = 46;
            this.CbxListTotals.Text = "List with Totals";
            this.CbxListTotals.UseVisualStyleBackColor = true;
            this.CbxListTotals.CheckedChanged += new System.EventHandler(this.CbxListTotals_CheckedChanged);
            // 
            // CbxListCrud
            // 
            this.CbxListCrud.AutoSize = true;
            this.CbxListCrud.Checked = true;
            this.CbxListCrud.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxListCrud.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxListCrud.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.CbxListCrud.Location = new System.Drawing.Point(40, 147);
            this.CbxListCrud.Name = "CbxListCrud";
            this.CbxListCrud.Size = new System.Drawing.Size(220, 17);
            this.CbxListCrud.TabIndex = 42;
            this.CbxListCrud.Text = "List with Add, Edit, && Delete (Same Page)";
            this.CbxListCrud.UseVisualStyleBackColor = true;
            this.CbxListCrud.CheckedChanged += new System.EventHandler(this.CbxListCrud_CheckedChanged);
            // 
            // pictureBox26
            // 
            this.pictureBox26.ErrorImage = null;
            this.pictureBox26.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox26.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox26.InitialImage = null;
            this.pictureBox26.Location = new System.Drawing.Point(12, 167);
            this.pictureBox26.Name = "pictureBox26";
            this.pictureBox26.Size = new System.Drawing.Size(16, 16);
            this.pictureBox26.TabIndex = 40;
            this.pictureBox26.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox26, "Views that has a list of items grouped by a certain item.  No CRUD.");
            // 
            // pictureBox22
            // 
            this.pictureBox22.ErrorImage = null;
            this.pictureBox22.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox22.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox22.InitialImage = null;
            this.pictureBox22.Location = new System.Drawing.Point(12, 56);
            this.pictureBox22.Name = "pictureBox22";
            this.pictureBox22.Size = new System.Drawing.Size(16, 16);
            this.pictureBox22.TabIndex = 38;
            this.pictureBox22.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox22, "Views that add a new record.");
            // 
            // CbxListGroupedBy
            // 
            this.CbxListGroupedBy.AutoSize = true;
            this.CbxListGroupedBy.Checked = true;
            this.CbxListGroupedBy.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxListGroupedBy.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxListGroupedBy.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.CbxListGroupedBy.Location = new System.Drawing.Point(40, 168);
            this.CbxListGroupedBy.Name = "CbxListGroupedBy";
            this.CbxListGroupedBy.Size = new System.Drawing.Size(110, 17);
            this.CbxListGroupedBy.TabIndex = 3;
            this.CbxListGroupedBy.Text = "List with Grouping";
            this.CbxListGroupedBy.UseVisualStyleBackColor = true;
            this.CbxListGroupedBy.CheckedChanged += new System.EventHandler(this.CbxListGroupedBy_CheckedChanged);
            // 
            // CbxAddNewRecord
            // 
            this.CbxAddNewRecord.AutoSize = true;
            this.CbxAddNewRecord.Checked = true;
            this.CbxAddNewRecord.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxAddNewRecord.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxAddNewRecord.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.CbxAddNewRecord.Location = new System.Drawing.Point(40, 57);
            this.CbxAddNewRecord.Name = "CbxAddNewRecord";
            this.CbxAddNewRecord.Size = new System.Drawing.Size(108, 17);
            this.CbxAddNewRecord.TabIndex = 2;
            this.CbxAddNewRecord.Text = "Add New Record";
            this.CbxAddNewRecord.UseVisualStyleBackColor = true;
            this.CbxAddNewRecord.CheckedChanged += new System.EventHandler(this.CbxAddNewRecord_CheckedChanged);
            // 
            // CbxListReadOnly
            // 
            this.CbxListReadOnly.AutoSize = true;
            this.CbxListReadOnly.Checked = true;
            this.CbxListReadOnly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxListReadOnly.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxListReadOnly.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.CbxListReadOnly.Location = new System.Drawing.Point(40, 124);
            this.CbxListReadOnly.Name = "CbxListReadOnly";
            this.CbxListReadOnly.Size = new System.Drawing.Size(101, 17);
            this.CbxListReadOnly.TabIndex = 1;
            this.CbxListReadOnly.Text = "List (Read-Only)";
            this.CbxListReadOnly.UseVisualStyleBackColor = true;
            this.CbxListReadOnly.CheckedChanged += new System.EventHandler(this.CbxListReadOnly_CheckedChanged);
            // 
            // CbxListCrudRedirect
            // 
            this.CbxListCrudRedirect.AutoSize = true;
            this.CbxListCrudRedirect.Checked = true;
            this.CbxListCrudRedirect.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxListCrudRedirect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxListCrudRedirect.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.CbxListCrudRedirect.Location = new System.Drawing.Point(40, 34);
            this.CbxListCrudRedirect.Name = "CbxListCrudRedirect";
            this.CbxListCrudRedirect.Size = new System.Drawing.Size(202, 17);
            this.CbxListCrudRedirect.TabIndex = 0;
            this.CbxListCrudRedirect.Text = "List with Add, Edit Redirect, && Delete ";
            this.CbxListCrudRedirect.UseVisualStyleBackColor = true;
            this.CbxListCrudRedirect.CheckedChanged += new System.EventHandler(this.CbxListCrudRedirect_CheckedChanged);
            // 
            // TabAppSettings
            // 
            this.TabAppSettings.Controls.Add(this.pictureBox66);
            this.TabAppSettings.Controls.Add(this.GbxOverwriteWebApiFiles);
            this.TabAppSettings.Controls.Add(this.GbxOverwriteBusinessDataLayerFiles);
            this.TabAppSettings.Controls.Add(this.CbxAutomaticallyOpenTab);
            this.TabAppSettings.Controls.Add(this.GbxOverwriteFiles);
            this.TabAppSettings.Controls.Add(this.BtnRestoreAllSettings);
            this.TabAppSettings.Controls.Add(this.TxtAppFilesDirectory);
            this.TabAppSettings.Controls.Add(this.BtnBrowseAppDirectory);
            this.TabAppSettings.Controls.Add(this.label7);
            this.TabAppSettings.Controls.Add(this.pictureBox38);
            this.TabAppSettings.Location = new System.Drawing.Point(4, 22);
            this.TabAppSettings.Name = "TabAppSettings";
            this.TabAppSettings.Size = new System.Drawing.Size(600, 547);
            this.TabAppSettings.TabIndex = 5;
            this.TabAppSettings.Text = "App Settings";
            this.TabAppSettings.UseVisualStyleBackColor = true;
            // 
            // pictureBox66
            // 
            this.pictureBox66.ErrorImage = null;
            this.pictureBox66.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox66.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox66.InitialImage = null;
            this.pictureBox66.Location = new System.Drawing.Point(21, 390);
            this.pictureBox66.Name = "pictureBox66";
            this.pictureBox66.Size = new System.Drawing.Size(16, 16);
            this.pictureBox66.TabIndex = 68;
            this.pictureBox66.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox66, "Opens the Selected Tables/Selected Views tab when the Selected Tables/Selected Vi" +
        "ews is selected on the Code Settings tab.");
            // 
            // GbxOverwriteWebApiFiles
            // 
            this.GbxOverwriteWebApiFiles.Controls.Add(this.pictureBox64);
            this.GbxOverwriteWebApiFiles.Controls.Add(this.CbxOverwriteStartUpClassWebApi);
            this.GbxOverwriteWebApiFiles.Controls.Add(this.CbxOverwriteProgramClassWebApi);
            this.GbxOverwriteWebApiFiles.Controls.Add(this.pictureBox63);
            this.GbxOverwriteWebApiFiles.Controls.Add(this.CbxOverwriteAppSettingsJsonWebApi);
            this.GbxOverwriteWebApiFiles.Controls.Add(this.pictureBox62);
            this.GbxOverwriteWebApiFiles.Controls.Add(this.CbxOverwriteLaunchSettingsJsonWebApi);
            this.GbxOverwriteWebApiFiles.Controls.Add(this.pictureBox33);
            this.GbxOverwriteWebApiFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GbxOverwriteWebApiFiles.Location = new System.Drawing.Point(18, 294);
            this.GbxOverwriteWebApiFiles.Name = "GbxOverwriteWebApiFiles";
            this.GbxOverwriteWebApiFiles.Size = new System.Drawing.Size(519, 81);
            this.GbxOverwriteWebApiFiles.TabIndex = 67;
            this.GbxOverwriteWebApiFiles.TabStop = false;
            this.GbxOverwriteWebApiFiles.Text = "Overwrite Web API Files";
            // 
            // pictureBox64
            // 
            this.pictureBox64.ErrorImage = null;
            this.pictureBox64.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox64.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox64.InitialImage = null;
            this.pictureBox64.Location = new System.Drawing.Point(256, 48);
            this.pictureBox64.Name = "pictureBox64";
            this.pictureBox64.Size = new System.Drawing.Size(16, 16);
            this.pictureBox64.TabIndex = 109;
            this.pictureBox64.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox64, "Overwrites the StartUp Class file.");
            // 
            // CbxOverwriteStartUpClassWebApi
            // 
            this.CbxOverwriteStartUpClassWebApi.AutoSize = true;
            this.CbxOverwriteStartUpClassWebApi.Checked = true;
            this.CbxOverwriteStartUpClassWebApi.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxOverwriteStartUpClassWebApi.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxOverwriteStartUpClassWebApi.Location = new System.Drawing.Point(283, 50);
            this.CbxOverwriteStartUpClassWebApi.Name = "CbxOverwriteStartUpClassWebApi";
            this.CbxOverwriteStartUpClassWebApi.Size = new System.Drawing.Size(138, 17);
            this.CbxOverwriteStartUpClassWebApi.TabIndex = 108;
            this.CbxOverwriteStartUpClassWebApi.Text = "Overwrite StartUp Class";
            this.CbxOverwriteStartUpClassWebApi.UseVisualStyleBackColor = true;
            // 
            // CbxOverwriteProgramClassWebApi
            // 
            this.CbxOverwriteProgramClassWebApi.AutoSize = true;
            this.CbxOverwriteProgramClassWebApi.Checked = true;
            this.CbxOverwriteProgramClassWebApi.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxOverwriteProgramClassWebApi.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxOverwriteProgramClassWebApi.Location = new System.Drawing.Point(283, 27);
            this.CbxOverwriteProgramClassWebApi.Name = "CbxOverwriteProgramClassWebApi";
            this.CbxOverwriteProgramClassWebApi.Size = new System.Drawing.Size(141, 17);
            this.CbxOverwriteProgramClassWebApi.TabIndex = 107;
            this.CbxOverwriteProgramClassWebApi.Text = "Overwrite Program Class";
            this.CbxOverwriteProgramClassWebApi.UseVisualStyleBackColor = true;
            // 
            // pictureBox63
            // 
            this.pictureBox63.ErrorImage = null;
            this.pictureBox63.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox63.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox63.InitialImage = null;
            this.pictureBox63.Location = new System.Drawing.Point(256, 26);
            this.pictureBox63.Name = "pictureBox63";
            this.pictureBox63.Size = new System.Drawing.Size(16, 16);
            this.pictureBox63.TabIndex = 106;
            this.pictureBox63.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox63, "Overwrites the Program Class file.");
            // 
            // CbxOverwriteAppSettingsJsonWebApi
            // 
            this.CbxOverwriteAppSettingsJsonWebApi.AutoSize = true;
            this.CbxOverwriteAppSettingsJsonWebApi.Checked = true;
            this.CbxOverwriteAppSettingsJsonWebApi.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxOverwriteAppSettingsJsonWebApi.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxOverwriteAppSettingsJsonWebApi.Location = new System.Drawing.Point(40, 50);
            this.CbxOverwriteAppSettingsJsonWebApi.Name = "CbxOverwriteAppSettingsJsonWebApi";
            this.CbxOverwriteAppSettingsJsonWebApi.Size = new System.Drawing.Size(150, 17);
            this.CbxOverwriteAppSettingsJsonWebApi.TabIndex = 105;
            this.CbxOverwriteAppSettingsJsonWebApi.Text = "Overwrite appsettings.json";
            this.CbxOverwriteAppSettingsJsonWebApi.UseVisualStyleBackColor = true;
            // 
            // pictureBox62
            // 
            this.pictureBox62.ErrorImage = null;
            this.pictureBox62.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox62.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox62.InitialImage = null;
            this.pictureBox62.Location = new System.Drawing.Point(13, 49);
            this.pictureBox62.Name = "pictureBox62";
            this.pictureBox62.Size = new System.Drawing.Size(16, 16);
            this.pictureBox62.TabIndex = 104;
            this.pictureBox62.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox62, "Overwrites the appsetting.json file.");
            // 
            // CbxOverwriteLaunchSettingsJsonWebApi
            // 
            this.CbxOverwriteLaunchSettingsJsonWebApi.AutoSize = true;
            this.CbxOverwriteLaunchSettingsJsonWebApi.Checked = true;
            this.CbxOverwriteLaunchSettingsJsonWebApi.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxOverwriteLaunchSettingsJsonWebApi.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxOverwriteLaunchSettingsJsonWebApi.Location = new System.Drawing.Point(40, 28);
            this.CbxOverwriteLaunchSettingsJsonWebApi.Name = "CbxOverwriteLaunchSettingsJsonWebApi";
            this.CbxOverwriteLaunchSettingsJsonWebApi.Size = new System.Drawing.Size(166, 17);
            this.CbxOverwriteLaunchSettingsJsonWebApi.TabIndex = 91;
            this.CbxOverwriteLaunchSettingsJsonWebApi.Text = "Overwrite launchSettings.json";
            this.CbxOverwriteLaunchSettingsJsonWebApi.UseVisualStyleBackColor = true;
            // 
            // pictureBox33
            // 
            this.pictureBox33.ErrorImage = null;
            this.pictureBox33.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox33.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox33.InitialImage = null;
            this.pictureBox33.Location = new System.Drawing.Point(13, 27);
            this.pictureBox33.Name = "pictureBox33";
            this.pictureBox33.Size = new System.Drawing.Size(16, 16);
            this.pictureBox33.TabIndex = 90;
            this.pictureBox33.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox33, "Overwrites the launchSettings.json under properties.");
            // 
            // GbxOverwriteBusinessDataLayerFiles
            // 
            this.GbxOverwriteBusinessDataLayerFiles.Controls.Add(this.CbxOverwriteAssemblyInfo);
            this.GbxOverwriteBusinessDataLayerFiles.Controls.Add(this.pictureBox40);
            this.GbxOverwriteBusinessDataLayerFiles.Controls.Add(this.CbxOverwriteAppSettingsClass);
            this.GbxOverwriteBusinessDataLayerFiles.Controls.Add(this.pictureBox36);
            this.GbxOverwriteBusinessDataLayerFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GbxOverwriteBusinessDataLayerFiles.Location = new System.Drawing.Point(18, 218);
            this.GbxOverwriteBusinessDataLayerFiles.Name = "GbxOverwriteBusinessDataLayerFiles";
            this.GbxOverwriteBusinessDataLayerFiles.Size = new System.Drawing.Size(519, 60);
            this.GbxOverwriteBusinessDataLayerFiles.TabIndex = 65;
            this.GbxOverwriteBusinessDataLayerFiles.TabStop = false;
            this.GbxOverwriteBusinessDataLayerFiles.Text = "Overwrite Business/Data Layer Files";
            // 
            // CbxOverwriteAssemblyInfo
            // 
            this.CbxOverwriteAssemblyInfo.AutoSize = true;
            this.CbxOverwriteAssemblyInfo.Checked = true;
            this.CbxOverwriteAssemblyInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxOverwriteAssemblyInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxOverwriteAssemblyInfo.Location = new System.Drawing.Point(40, 27);
            this.CbxOverwriteAssemblyInfo.Name = "CbxOverwriteAssemblyInfo";
            this.CbxOverwriteAssemblyInfo.Size = new System.Drawing.Size(164, 17);
            this.CbxOverwriteAssemblyInfo.TabIndex = 99;
            this.CbxOverwriteAssemblyInfo.Text = "Overwrite AssemblyInfo Class";
            this.CbxOverwriteAssemblyInfo.UseVisualStyleBackColor = true;
            // 
            // pictureBox40
            // 
            this.pictureBox40.ErrorImage = null;
            this.pictureBox40.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox40.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox40.InitialImage = null;
            this.pictureBox40.Location = new System.Drawing.Point(13, 26);
            this.pictureBox40.Name = "pictureBox40";
            this.pictureBox40.Size = new System.Drawing.Size(16, 16);
            this.pictureBox40.TabIndex = 98;
            this.pictureBox40.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox40, "Overwrites the AssemblyInfo Class in the Properties folder.");
            // 
            // CbxOverwriteAppSettingsClass
            // 
            this.CbxOverwriteAppSettingsClass.AutoSize = true;
            this.CbxOverwriteAppSettingsClass.Checked = true;
            this.CbxOverwriteAppSettingsClass.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxOverwriteAppSettingsClass.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxOverwriteAppSettingsClass.Location = new System.Drawing.Point(283, 27);
            this.CbxOverwriteAppSettingsClass.Name = "CbxOverwriteAppSettingsClass";
            this.CbxOverwriteAppSettingsClass.Size = new System.Drawing.Size(159, 17);
            this.CbxOverwriteAppSettingsClass.TabIndex = 80;
            this.CbxOverwriteAppSettingsClass.Text = "Overwrite AppSettings Class";
            this.CbxOverwriteAppSettingsClass.UseVisualStyleBackColor = true;
            // 
            // pictureBox36
            // 
            this.pictureBox36.ErrorImage = null;
            this.pictureBox36.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox36.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox36.InitialImage = null;
            this.pictureBox36.Location = new System.Drawing.Point(256, 26);
            this.pictureBox36.Name = "pictureBox36";
            this.pictureBox36.Size = new System.Drawing.Size(16, 16);
            this.pictureBox36.TabIndex = 96;
            this.pictureBox36.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox36, "Overwrites the project.json and project.lock.json files.");
            // 
            // CbxAutomaticallyOpenTab
            // 
            this.CbxAutomaticallyOpenTab.AutoSize = true;
            this.CbxAutomaticallyOpenTab.Checked = true;
            this.CbxAutomaticallyOpenTab.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxAutomaticallyOpenTab.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.CbxAutomaticallyOpenTab.Location = new System.Drawing.Point(45, 392);
            this.CbxAutomaticallyOpenTab.Name = "CbxAutomaticallyOpenTab";
            this.CbxAutomaticallyOpenTab.Size = new System.Drawing.Size(303, 17);
            this.CbxAutomaticallyOpenTab.TabIndex = 62;
            this.CbxAutomaticallyOpenTab.Text = "Automatically Open Selected Tables or Selected Views tab";
            this.CbxAutomaticallyOpenTab.UseVisualStyleBackColor = true;
            // 
            // GbxOverwriteFiles
            // 
            this.GbxOverwriteFiles.Controls.Add(this.CbxOverwriteStartUpClass);
            this.GbxOverwriteFiles.Controls.Add(this.CbxOverwriteFunctionsFile);
            this.GbxOverwriteFiles.Controls.Add(this.CbxOverwriteSiteCss);
            this.GbxOverwriteFiles.Controls.Add(this.pictureBox53);
            this.GbxOverwriteFiles.Controls.Add(this.pictureBox35);
            this.GbxOverwriteFiles.Controls.Add(this.CbxOverwriteViewStartPage);
            this.GbxOverwriteFiles.Controls.Add(this.pictureBox23);
            this.GbxOverwriteFiles.Controls.Add(this.pictureBox18);
            this.GbxOverwriteFiles.Controls.Add(this.CbxOverwriteProgramClass);
            this.GbxOverwriteFiles.Controls.Add(this.CbxOverwriteLayoutPage);
            this.GbxOverwriteFiles.Controls.Add(this.pictureBox52);
            this.GbxOverwriteFiles.Controls.Add(this.pictureBox12);
            this.GbxOverwriteFiles.Controls.Add(this.CbxOverwriteAppSettingsJson);
            this.GbxOverwriteFiles.Controls.Add(this.pictureBox31);
            this.GbxOverwriteFiles.Controls.Add(this.pictureBox45);
            this.GbxOverwriteFiles.Controls.Add(this.pictureBox43);
            this.GbxOverwriteFiles.Controls.Add(this.pictureBox32);
            this.GbxOverwriteFiles.Controls.Add(this.CbxOverwriteLaunchSettingsJson);
            this.GbxOverwriteFiles.Controls.Add(this.pictureBox44);
            this.GbxOverwriteFiles.Controls.Add(this.pictureBox28);
            this.GbxOverwriteFiles.Controls.Add(this.CbxOverwriteViewImportsView);
            this.GbxOverwriteFiles.Controls.Add(this.CbxOverwriteBowerJson);
            this.GbxOverwriteFiles.Controls.Add(this.CbxOverwriteBundleConfigJson);
            this.GbxOverwriteFiles.Controls.Add(this.CbxOverwriteValidationScriptsPartialView);
            this.GbxOverwriteFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.GbxOverwriteFiles.Location = new System.Drawing.Point(18, 15);
            this.GbxOverwriteFiles.Name = "GbxOverwriteFiles";
            this.GbxOverwriteFiles.Size = new System.Drawing.Size(519, 185);
            this.GbxOverwriteFiles.TabIndex = 32;
            this.GbxOverwriteFiles.TabStop = false;
            this.GbxOverwriteFiles.Text = "Overwrite Web Application Files ";
            // 
            // CbxOverwriteStartUpClass
            // 
            this.CbxOverwriteStartUpClass.AutoSize = true;
            this.CbxOverwriteStartUpClass.Checked = true;
            this.CbxOverwriteStartUpClass.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxOverwriteStartUpClass.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxOverwriteStartUpClass.Location = new System.Drawing.Point(283, 145);
            this.CbxOverwriteStartUpClass.Name = "CbxOverwriteStartUpClass";
            this.CbxOverwriteStartUpClass.Size = new System.Drawing.Size(138, 17);
            this.CbxOverwriteStartUpClass.TabIndex = 118;
            this.CbxOverwriteStartUpClass.Text = "Overwrite StartUp Class";
            this.CbxOverwriteStartUpClass.UseVisualStyleBackColor = true;
            // 
            // CbxOverwriteFunctionsFile
            // 
            this.CbxOverwriteFunctionsFile.AutoSize = true;
            this.CbxOverwriteFunctionsFile.Checked = true;
            this.CbxOverwriteFunctionsFile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxOverwriteFunctionsFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxOverwriteFunctionsFile.Location = new System.Drawing.Point(40, 78);
            this.CbxOverwriteFunctionsFile.Name = "CbxOverwriteFunctionsFile";
            this.CbxOverwriteFunctionsFile.Size = new System.Drawing.Size(148, 17);
            this.CbxOverwriteFunctionsFile.TabIndex = 117;
            this.CbxOverwriteFunctionsFile.Text = "Overwrite Functions Class";
            this.CbxOverwriteFunctionsFile.UseVisualStyleBackColor = true;
            // 
            // CbxOverwriteSiteCss
            // 
            this.CbxOverwriteSiteCss.AutoSize = true;
            this.CbxOverwriteSiteCss.Checked = true;
            this.CbxOverwriteSiteCss.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxOverwriteSiteCss.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxOverwriteSiteCss.Location = new System.Drawing.Point(40, 54);
            this.CbxOverwriteSiteCss.Name = "CbxOverwriteSiteCss";
            this.CbxOverwriteSiteCss.Size = new System.Drawing.Size(109, 17);
            this.CbxOverwriteSiteCss.TabIndex = 116;
            this.CbxOverwriteSiteCss.Text = "Overwrite site.css";
            this.CbxOverwriteSiteCss.UseVisualStyleBackColor = true;
            // 
            // pictureBox53
            // 
            this.pictureBox53.ErrorImage = null;
            this.pictureBox53.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox53.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox53.InitialImage = null;
            this.pictureBox53.Location = new System.Drawing.Point(256, 145);
            this.pictureBox53.Name = "pictureBox53";
            this.pictureBox53.Size = new System.Drawing.Size(16, 16);
            this.pictureBox53.TabIndex = 107;
            this.pictureBox53.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox53, "Overwrites the StartUp Class file.");
            // 
            // pictureBox35
            // 
            this.pictureBox35.ErrorImage = null;
            this.pictureBox35.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox35.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox35.InitialImage = null;
            this.pictureBox35.Location = new System.Drawing.Point(13, 123);
            this.pictureBox35.Name = "pictureBox35";
            this.pictureBox35.Size = new System.Drawing.Size(16, 16);
            this.pictureBox35.TabIndex = 95;
            this.pictureBox35.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox35, "Overwrites the _ValidationScriptsPartial view in the Views/Shared folder.");
            // 
            // CbxOverwriteViewStartPage
            // 
            this.CbxOverwriteViewStartPage.AutoSize = true;
            this.CbxOverwriteViewStartPage.Checked = true;
            this.CbxOverwriteViewStartPage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxOverwriteViewStartPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxOverwriteViewStartPage.Location = new System.Drawing.Point(283, 31);
            this.CbxOverwriteViewStartPage.Name = "CbxOverwriteViewStartPage";
            this.CbxOverwriteViewStartPage.Size = new System.Drawing.Size(147, 17);
            this.CbxOverwriteViewStartPage.TabIndex = 102;
            this.CbxOverwriteViewStartPage.Text = "Overwrite ViewStart Page";
            this.CbxOverwriteViewStartPage.UseVisualStyleBackColor = true;
            // 
            // pictureBox23
            // 
            this.pictureBox23.ErrorImage = null;
            this.pictureBox23.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox23.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox23.InitialImage = null;
            this.pictureBox23.Location = new System.Drawing.Point(13, 146);
            this.pictureBox23.Name = "pictureBox23";
            this.pictureBox23.Size = new System.Drawing.Size(16, 16);
            this.pictureBox23.TabIndex = 94;
            this.pictureBox23.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox23, "Overwrites the _ViewImports view in the Views.");
            // 
            // pictureBox18
            // 
            this.pictureBox18.ErrorImage = null;
            this.pictureBox18.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox18.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox18.InitialImage = null;
            this.pictureBox18.Location = new System.Drawing.Point(256, 99);
            this.pictureBox18.Name = "pictureBox18";
            this.pictureBox18.Size = new System.Drawing.Size(16, 16);
            this.pictureBox18.TabIndex = 93;
            this.pictureBox18.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox18, "Overwrites the bundleconfig.json.");
            // 
            // CbxOverwriteProgramClass
            // 
            this.CbxOverwriteProgramClass.AutoSize = true;
            this.CbxOverwriteProgramClass.Checked = true;
            this.CbxOverwriteProgramClass.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxOverwriteProgramClass.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxOverwriteProgramClass.Location = new System.Drawing.Point(283, 122);
            this.CbxOverwriteProgramClass.Name = "CbxOverwriteProgramClass";
            this.CbxOverwriteProgramClass.Size = new System.Drawing.Size(141, 17);
            this.CbxOverwriteProgramClass.TabIndex = 105;
            this.CbxOverwriteProgramClass.Text = "Overwrite Program Class";
            this.CbxOverwriteProgramClass.UseVisualStyleBackColor = true;
            // 
            // CbxOverwriteLayoutPage
            // 
            this.CbxOverwriteLayoutPage.AutoSize = true;
            this.CbxOverwriteLayoutPage.Checked = true;
            this.CbxOverwriteLayoutPage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxOverwriteLayoutPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxOverwriteLayoutPage.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.CbxOverwriteLayoutPage.Location = new System.Drawing.Point(40, 101);
            this.CbxOverwriteLayoutPage.Name = "CbxOverwriteLayoutPage";
            this.CbxOverwriteLayoutPage.Size = new System.Drawing.Size(138, 17);
            this.CbxOverwriteLayoutPage.TabIndex = 25;
            this.CbxOverwriteLayoutPage.Text = "Overwrite _Layout View";
            this.CbxOverwriteLayoutPage.UseVisualStyleBackColor = true;
            // 
            // pictureBox52
            // 
            this.pictureBox52.ErrorImage = null;
            this.pictureBox52.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox52.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox52.InitialImage = null;
            this.pictureBox52.Location = new System.Drawing.Point(256, 121);
            this.pictureBox52.Name = "pictureBox52";
            this.pictureBox52.Size = new System.Drawing.Size(16, 16);
            this.pictureBox52.TabIndex = 104;
            this.pictureBox52.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox52, "Overwrites the Program Class file.");
            // 
            // pictureBox12
            // 
            this.pictureBox12.ErrorImage = null;
            this.pictureBox12.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox12.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox12.InitialImage = null;
            this.pictureBox12.Location = new System.Drawing.Point(256, 76);
            this.pictureBox12.Name = "pictureBox12";
            this.pictureBox12.Size = new System.Drawing.Size(16, 16);
            this.pictureBox12.TabIndex = 92;
            this.pictureBox12.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox12, "Overwrites the bower.json file.");
            // 
            // CbxOverwriteAppSettingsJson
            // 
            this.CbxOverwriteAppSettingsJson.AutoSize = true;
            this.CbxOverwriteAppSettingsJson.Checked = true;
            this.CbxOverwriteAppSettingsJson.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxOverwriteAppSettingsJson.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxOverwriteAppSettingsJson.Location = new System.Drawing.Point(283, 54);
            this.CbxOverwriteAppSettingsJson.Name = "CbxOverwriteAppSettingsJson";
            this.CbxOverwriteAppSettingsJson.Size = new System.Drawing.Size(150, 17);
            this.CbxOverwriteAppSettingsJson.TabIndex = 103;
            this.CbxOverwriteAppSettingsJson.Text = "Overwrite appsettings.json";
            this.CbxOverwriteAppSettingsJson.UseVisualStyleBackColor = true;
            // 
            // pictureBox31
            // 
            this.pictureBox31.ErrorImage = null;
            this.pictureBox31.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox31.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox31.InitialImage = null;
            this.pictureBox31.Location = new System.Drawing.Point(13, 53);
            this.pictureBox31.Name = "pictureBox31";
            this.pictureBox31.Size = new System.Drawing.Size(16, 16);
            this.pictureBox31.TabIndex = 89;
            this.pictureBox31.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox31, "Overwrites the site.css stylesheet in the wwwroot/css folder.");
            // 
            // pictureBox45
            // 
            this.pictureBox45.ErrorImage = null;
            this.pictureBox45.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox45.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox45.InitialImage = null;
            this.pictureBox45.Location = new System.Drawing.Point(256, 53);
            this.pictureBox45.Name = "pictureBox45";
            this.pictureBox45.Size = new System.Drawing.Size(16, 16);
            this.pictureBox45.TabIndex = 101;
            this.pictureBox45.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox45, "Overwrites the appsetting.json file.");
            // 
            // pictureBox43
            // 
            this.pictureBox43.ErrorImage = null;
            this.pictureBox43.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox43.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox43.InitialImage = null;
            this.pictureBox43.Location = new System.Drawing.Point(256, 30);
            this.pictureBox43.Name = "pictureBox43";
            this.pictureBox43.Size = new System.Drawing.Size(16, 16);
            this.pictureBox43.TabIndex = 100;
            this.pictureBox43.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox43, "Overwrites the generated _ViewStart file in the Views folder.");
            // 
            // pictureBox32
            // 
            this.pictureBox32.ErrorImage = null;
            this.pictureBox32.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox32.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox32.InitialImage = null;
            this.pictureBox32.Location = new System.Drawing.Point(13, 77);
            this.pictureBox32.Name = "pictureBox32";
            this.pictureBox32.Size = new System.Drawing.Size(16, 16);
            this.pictureBox32.TabIndex = 88;
            this.pictureBox32.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox32, "Overwrites the Functions helper class file in the Helper folder.");
            // 
            // CbxOverwriteLaunchSettingsJson
            // 
            this.CbxOverwriteLaunchSettingsJson.AutoSize = true;
            this.CbxOverwriteLaunchSettingsJson.Checked = true;
            this.CbxOverwriteLaunchSettingsJson.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxOverwriteLaunchSettingsJson.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxOverwriteLaunchSettingsJson.Location = new System.Drawing.Point(40, 31);
            this.CbxOverwriteLaunchSettingsJson.Name = "CbxOverwriteLaunchSettingsJson";
            this.CbxOverwriteLaunchSettingsJson.Size = new System.Drawing.Size(166, 17);
            this.CbxOverwriteLaunchSettingsJson.TabIndex = 85;
            this.CbxOverwriteLaunchSettingsJson.Text = "Overwrite launchSettings.json";
            this.CbxOverwriteLaunchSettingsJson.UseVisualStyleBackColor = true;
            // 
            // pictureBox44
            // 
            this.pictureBox44.ErrorImage = null;
            this.pictureBox44.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox44.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox44.InitialImage = null;
            this.pictureBox44.Location = new System.Drawing.Point(13, 30);
            this.pictureBox44.Name = "pictureBox44";
            this.pictureBox44.Size = new System.Drawing.Size(16, 16);
            this.pictureBox44.TabIndex = 84;
            this.pictureBox44.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox44, "Overwrites the launchSettings.json under properties.");
            // 
            // pictureBox28
            // 
            this.pictureBox28.ErrorImage = null;
            this.pictureBox28.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox28.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox28.InitialImage = null;
            this.pictureBox28.Location = new System.Drawing.Point(13, 100);
            this.pictureBox28.Name = "pictureBox28";
            this.pictureBox28.Size = new System.Drawing.Size(16, 16);
            this.pictureBox28.TabIndex = 87;
            this.pictureBox28.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox28, "Overwrites the _Layout view in the Views/Shared folder.");
            // 
            // CbxOverwriteViewImportsView
            // 
            this.CbxOverwriteViewImportsView.AutoSize = true;
            this.CbxOverwriteViewImportsView.Checked = true;
            this.CbxOverwriteViewImportsView.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxOverwriteViewImportsView.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxOverwriteViewImportsView.Location = new System.Drawing.Point(40, 147);
            this.CbxOverwriteViewImportsView.Name = "CbxOverwriteViewImportsView";
            this.CbxOverwriteViewImportsView.Size = new System.Drawing.Size(163, 17);
            this.CbxOverwriteViewImportsView.TabIndex = 78;
            this.CbxOverwriteViewImportsView.Text = "Overwrite _ViewImports View";
            this.CbxOverwriteViewImportsView.UseVisualStyleBackColor = true;
            // 
            // CbxOverwriteBowerJson
            // 
            this.CbxOverwriteBowerJson.AutoSize = true;
            this.CbxOverwriteBowerJson.Checked = true;
            this.CbxOverwriteBowerJson.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxOverwriteBowerJson.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxOverwriteBowerJson.Location = new System.Drawing.Point(283, 77);
            this.CbxOverwriteBowerJson.Name = "CbxOverwriteBowerJson";
            this.CbxOverwriteBowerJson.Size = new System.Drawing.Size(125, 17);
            this.CbxOverwriteBowerJson.TabIndex = 76;
            this.CbxOverwriteBowerJson.Text = "Overwrite bower.json";
            this.CbxOverwriteBowerJson.UseVisualStyleBackColor = true;
            // 
            // CbxOverwriteBundleConfigJson
            // 
            this.CbxOverwriteBundleConfigJson.AutoSize = true;
            this.CbxOverwriteBundleConfigJson.Checked = true;
            this.CbxOverwriteBundleConfigJson.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxOverwriteBundleConfigJson.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxOverwriteBundleConfigJson.Location = new System.Drawing.Point(283, 100);
            this.CbxOverwriteBundleConfigJson.Name = "CbxOverwriteBundleConfigJson";
            this.CbxOverwriteBundleConfigJson.Size = new System.Drawing.Size(157, 17);
            this.CbxOverwriteBundleConfigJson.TabIndex = 75;
            this.CbxOverwriteBundleConfigJson.Text = "Overwrite bundleconfig.json";
            this.CbxOverwriteBundleConfigJson.UseVisualStyleBackColor = true;
            // 
            // CbxOverwriteValidationScriptsPartialView
            // 
            this.CbxOverwriteValidationScriptsPartialView.AutoSize = true;
            this.CbxOverwriteValidationScriptsPartialView.Checked = true;
            this.CbxOverwriteValidationScriptsPartialView.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxOverwriteValidationScriptsPartialView.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxOverwriteValidationScriptsPartialView.Location = new System.Drawing.Point(40, 124);
            this.CbxOverwriteValidationScriptsPartialView.Name = "CbxOverwriteValidationScriptsPartialView";
            this.CbxOverwriteValidationScriptsPartialView.Size = new System.Drawing.Size(213, 17);
            this.CbxOverwriteValidationScriptsPartialView.TabIndex = 73;
            this.CbxOverwriteValidationScriptsPartialView.Text = "Overwrite _ValidationScriptsPartial View";
            this.CbxOverwriteValidationScriptsPartialView.UseVisualStyleBackColor = true;
            // 
            // BtnRestoreAllSettings
            // 
            this.BtnRestoreAllSettings.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnRestoreAllSettings.Location = new System.Drawing.Point(21, 485);
            this.BtnRestoreAllSettings.Name = "BtnRestoreAllSettings";
            this.BtnRestoreAllSettings.Size = new System.Drawing.Size(215, 25);
            this.BtnRestoreAllSettings.TabIndex = 24;
            this.BtnRestoreAllSettings.Text = "Restore All Settings to Default";
            this.BtnRestoreAllSettings.UseVisualStyleBackColor = true;
            this.BtnRestoreAllSettings.Click += new System.EventHandler(this.BtnRestoreAllSettings_Click);
            // 
            // TxtAppFilesDirectory
            // 
            this.TxtAppFilesDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.TxtAppFilesDirectory.Location = new System.Drawing.Point(21, 447);
            this.TxtAppFilesDirectory.Name = "TxtAppFilesDirectory";
            this.TxtAppFilesDirectory.Size = new System.Drawing.Size(384, 20);
            this.TxtAppFilesDirectory.TabIndex = 21;
            this.TxtAppFilesDirectory.Text = "C:\\Program Files (x86)\\KPIT\\AspCoreGen 2.0 Razor Professional Plus\\AppFiles\\";
            // 
            // BtnBrowseAppDirectory
            // 
            this.BtnBrowseAppDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.BtnBrowseAppDirectory.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnBrowseAppDirectory.Location = new System.Drawing.Point(411, 446);
            this.BtnBrowseAppDirectory.Name = "BtnBrowseAppDirectory";
            this.BtnBrowseAppDirectory.Size = new System.Drawing.Size(71, 20);
            this.BtnBrowseAppDirectory.TabIndex = 22;
            this.BtnBrowseAppDirectory.Text = "browse...";
            this.BtnBrowseAppDirectory.UseVisualStyleBackColor = true;
            this.BtnBrowseAppDirectory.Click += new System.EventHandler(this.BtnBrowseAppDirectory_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(42, 428);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(98, 13);
            this.label7.TabIndex = 23;
            this.label7.Text = "App Files Directory:";
            // 
            // pictureBox38
            // 
            this.pictureBox38.ErrorImage = null;
            this.pictureBox38.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox38.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox38.InitialImage = null;
            this.pictureBox38.Location = new System.Drawing.Point(21, 426);
            this.pictureBox38.Name = "pictureBox38";
            this.pictureBox38.Size = new System.Drawing.Size(16, 16);
            this.pictureBox38.TabIndex = 61;
            this.pictureBox38.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox38, "The directory where AspCoreGen 2.0 Razor AppFiles folder was installed.");
            // 
            // pictureBox8
            // 
            this.pictureBox8.ErrorImage = null;
            this.pictureBox8.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox8.InitialImage = null;
            this.pictureBox8.Location = new System.Drawing.Point(13, 44);
            this.pictureBox8.Name = "pictureBox8";
            this.pictureBox8.Size = new System.Drawing.Size(16, 16);
            this.pictureBox8.TabIndex = 27;
            this.pictureBox8.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox8, "Genetates models, views, controllers and code for all views in the database.  Not" +
        "e: Generates List (Read Only) views under UI Settings.");
            // 
            // pictureBox6
            // 
            this.pictureBox6.ErrorImage = null;
            this.pictureBox6.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox6.InitialImage = null;
            this.pictureBox6.Location = new System.Drawing.Point(13, 67);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(16, 16);
            this.pictureBox6.TabIndex = 29;
            this.pictureBox6.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox6, "Genetates models, views, controllers and code for selected tables you choose from" +
        " the Selected Tables tab.");
            // 
            // pictureBox42
            // 
            this.pictureBox42.ErrorImage = null;
            this.pictureBox42.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox42.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox42.InitialImage = null;
            this.pictureBox42.Location = new System.Drawing.Point(10, 21);
            this.pictureBox42.Name = "pictureBox42";
            this.pictureBox42.Size = new System.Drawing.Size(16, 16);
            this.pictureBox42.TabIndex = 39;
            this.pictureBox42.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox42, "The name of the MS SQL Server where your database is located.  E.g. \"localhost\".");
            // 
            // PicbUserName
            // 
            this.PicbUserName.ErrorImage = null;
            this.PicbUserName.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.PicbUserName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.PicbUserName.InitialImage = null;
            this.PicbUserName.Location = new System.Drawing.Point(10, 71);
            this.PicbUserName.Name = "PicbUserName";
            this.PicbUserName.Size = new System.Drawing.Size(16, 16);
            this.PicbUserName.TabIndex = 25;
            this.PicbUserName.TabStop = false;
            this.toolTip1.SetToolTip(this.PicbUserName, "The user name you use to get access to your database.  User names that have admin" +
        " rights to your database.  E.g. \"sa\"");
            // 
            // PicbPassword
            // 
            this.PicbPassword.ErrorImage = null;
            this.PicbPassword.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.PicbPassword.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.PicbPassword.InitialImage = null;
            this.PicbPassword.Location = new System.Drawing.Point(10, 98);
            this.PicbPassword.Name = "PicbPassword";
            this.PicbPassword.Size = new System.Drawing.Size(16, 16);
            this.PicbPassword.TabIndex = 24;
            this.PicbPassword.TabStop = false;
            this.toolTip1.SetToolTip(this.PicbPassword, "The database password paired with the user name above.  This field cannot be blan" +
        "k.  E.g. \"mypassword\".");
            // 
            // PicbShowPassword
            // 
            this.PicbShowPassword.ErrorImage = null;
            this.PicbShowPassword.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.PicbShowPassword.InitialImage = null;
            this.PicbShowPassword.Location = new System.Drawing.Point(340, 97);
            this.PicbShowPassword.Name = "PicbShowPassword";
            this.PicbShowPassword.Size = new System.Drawing.Size(18, 18);
            this.PicbShowPassword.TabIndex = 23;
            this.PicbShowPassword.TabStop = false;
            this.toolTip1.SetToolTip(this.PicbShowPassword, "Masks\' the password with an * when unchecked.  Shows the password when checked, a" +
        "nd remembers the last password you entered when you close the application.");
            // 
            // PicbDatabaseName
            // 
            this.PicbDatabaseName.ErrorImage = null;
            this.PicbDatabaseName.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.PicbDatabaseName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.PicbDatabaseName.InitialImage = null;
            this.PicbDatabaseName.Location = new System.Drawing.Point(10, 47);
            this.PicbDatabaseName.Name = "PicbDatabaseName";
            this.PicbDatabaseName.Size = new System.Drawing.Size(16, 16);
            this.PicbDatabaseName.TabIndex = 20;
            this.PicbDatabaseName.TabStop = false;
            this.toolTip1.SetToolTip(this.PicbDatabaseName, "The database you want to generate code from.  E.g. \"NorthWind\"");
            // 
            // CbxRememberPassword
            // 
            this.CbxRememberPassword.AutoSize = true;
            this.CbxRememberPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxRememberPassword.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.CbxRememberPassword.Location = new System.Drawing.Point(364, 99);
            this.CbxRememberPassword.Name = "CbxRememberPassword";
            this.CbxRememberPassword.Size = new System.Drawing.Size(102, 17);
            this.CbxRememberPassword.TabIndex = 5;
            this.CbxRememberPassword.Text = "Show Password";
            this.CbxRememberPassword.UseVisualStyleBackColor = true;
            this.CbxRememberPassword.CheckedChanged += new System.EventHandler(this.CbxRememberPassword_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(34, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Server:";
            // 
            // TxtServer
            // 
            this.TxtServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.TxtServer.Location = new System.Drawing.Point(127, 19);
            this.TxtServer.Name = "TxtServer";
            this.TxtServer.Size = new System.Drawing.Size(332, 20);
            this.TxtServer.TabIndex = 0;
            // 
            // TxtDatabase
            // 
            this.TxtDatabase.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.TxtDatabase.Location = new System.Drawing.Point(127, 45);
            this.TxtDatabase.Name = "TxtDatabase";
            this.TxtDatabase.Size = new System.Drawing.Size(332, 20);
            this.TxtDatabase.TabIndex = 2;
            // 
            // TxtUserName
            // 
            this.TxtUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.TxtUserName.Location = new System.Drawing.Point(127, 71);
            this.TxtUserName.Name = "TxtUserName";
            this.TxtUserName.Size = new System.Drawing.Size(332, 20);
            this.TxtUserName.TabIndex = 3;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.pictureBox9);
            this.groupBox3.Controls.Add(this.pictureBox6);
            this.groupBox3.Controls.Add(this.pictureBox7);
            this.groupBox3.Controls.Add(this.pictureBox8);
            this.groupBox3.Controls.Add(this.RbtnGenerateFromSelectedViews);
            this.groupBox3.Controls.Add(this.RbtnGenerateFromSelectedTables);
            this.groupBox3.Controls.Add(this.RbtnGenerateFromAllViews);
            this.groupBox3.Controls.Add(this.RbtnGenerateFromAllTables);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupBox3.Location = new System.Drawing.Point(13, 13);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(523, 119);
            this.groupBox3.TabIndex = 25;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Database Objects to Generate From";
            // 
            // pictureBox9
            // 
            this.pictureBox9.ErrorImage = null;
            this.pictureBox9.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox9.InitialImage = null;
            this.pictureBox9.Location = new System.Drawing.Point(13, 21);
            this.pictureBox9.Name = "pictureBox9";
            this.pictureBox9.Size = new System.Drawing.Size(16, 16);
            this.pictureBox9.TabIndex = 39;
            this.pictureBox9.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox9, "Genetates models, views, controllers and code for all tables in the database.\r\n");
            // 
            // RbtnGenerateFromSelectedViews
            // 
            this.RbtnGenerateFromSelectedViews.AutoSize = true;
            this.RbtnGenerateFromSelectedViews.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.RbtnGenerateFromSelectedViews.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.RbtnGenerateFromSelectedViews.Location = new System.Drawing.Point(37, 90);
            this.RbtnGenerateFromSelectedViews.Name = "RbtnGenerateFromSelectedViews";
            this.RbtnGenerateFromSelectedViews.Size = new System.Drawing.Size(122, 17);
            this.RbtnGenerateFromSelectedViews.TabIndex = 3;
            this.RbtnGenerateFromSelectedViews.Text = "Selected Views Only";
            this.RbtnGenerateFromSelectedViews.UseVisualStyleBackColor = true;
            this.RbtnGenerateFromSelectedViews.CheckedChanged += new System.EventHandler(this.RbtnGenerateFromSelectedViews_CheckedChanged);
            // 
            // RbtnGenerateFromSelectedTables
            // 
            this.RbtnGenerateFromSelectedTables.AutoSize = true;
            this.RbtnGenerateFromSelectedTables.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.RbtnGenerateFromSelectedTables.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.RbtnGenerateFromSelectedTables.Location = new System.Drawing.Point(37, 67);
            this.RbtnGenerateFromSelectedTables.Name = "RbtnGenerateFromSelectedTables";
            this.RbtnGenerateFromSelectedTables.Size = new System.Drawing.Size(126, 17);
            this.RbtnGenerateFromSelectedTables.TabIndex = 2;
            this.RbtnGenerateFromSelectedTables.Text = "Selected Tables Only";
            this.RbtnGenerateFromSelectedTables.UseVisualStyleBackColor = true;
            this.RbtnGenerateFromSelectedTables.CheckedChanged += new System.EventHandler(this.RbtnGenerateFromSelectedTables_CheckedChanged);
            // 
            // RbtnGenerateFromAllViews
            // 
            this.RbtnGenerateFromAllViews.AutoSize = true;
            this.RbtnGenerateFromAllViews.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.RbtnGenerateFromAllViews.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.RbtnGenerateFromAllViews.Location = new System.Drawing.Point(37, 44);
            this.RbtnGenerateFromAllViews.Name = "RbtnGenerateFromAllViews";
            this.RbtnGenerateFromAllViews.Size = new System.Drawing.Size(67, 17);
            this.RbtnGenerateFromAllViews.TabIndex = 1;
            this.RbtnGenerateFromAllViews.Text = "All Views";
            this.RbtnGenerateFromAllViews.UseVisualStyleBackColor = true;
            this.RbtnGenerateFromAllViews.CheckedChanged += new System.EventHandler(this.RbtnGenerateFromAllViews_CheckedChanged);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Enabled = false;
            this.BtnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnCancel.Location = new System.Drawing.Point(531, 612);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(73, 34);
            this.BtnCancel.TabIndex = 42;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 20000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 100;
            // 
            // pictureBox55
            // 
            this.pictureBox55.ErrorImage = null;
            this.pictureBox55.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox55.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox55.InitialImage = null;
            this.pictureBox55.Location = new System.Drawing.Point(13, 82);
            this.pictureBox55.Name = "pictureBox55";
            this.pictureBox55.Size = new System.Drawing.Size(16, 16);
            this.pictureBox55.TabIndex = 42;
            this.pictureBox55.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox55, "The directory where the Business Layer/Data Layer project is generated in");
            // 
            // pictureBox15
            // 
            this.pictureBox15.ErrorImage = null;
            this.pictureBox15.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox15.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox15.InitialImage = null;
            this.pictureBox15.Location = new System.Drawing.Point(13, 55);
            this.pictureBox15.Name = "pictureBox15";
            this.pictureBox15.Size = new System.Drawing.Size(16, 16);
            this.pictureBox15.TabIndex = 39;
            this.pictureBox15.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox15, "API Name.  Name of the Class Library Project where the Business Object and Data L" +
        "ayer code are generated.");
            // 
            // pictureBox13
            // 
            this.pictureBox13.ErrorImage = null;
            this.pictureBox13.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox13.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox13.InitialImage = null;
            this.pictureBox13.Location = new System.Drawing.Point(13, 27);
            this.pictureBox13.Name = "pictureBox13";
            this.pictureBox13.Size = new System.Drawing.Size(16, 16);
            this.pictureBox13.TabIndex = 35;
            this.pictureBox13.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox13, "Classes will be generated in either C# or VB.NET");
            // 
            // pictureBox2
            // 
            this.pictureBox2.ErrorImage = null;
            this.pictureBox2.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox2.InitialImage = null;
            this.pictureBox2.Location = new System.Drawing.Point(14, 19);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(16, 16);
            this.pictureBox2.TabIndex = 40;
            this.pictureBox2.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox2, "Generates Linq-to-Entities inside your code using Entity Framework Core.");
            // 
            // pictureBox57
            // 
            this.pictureBox57.ErrorImage = null;
            this.pictureBox57.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox57.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox57.InitialImage = null;
            this.pictureBox57.Location = new System.Drawing.Point(12, 27);
            this.pictureBox57.Name = "pictureBox57";
            this.pictureBox57.Size = new System.Drawing.Size(16, 16);
            this.pictureBox57.TabIndex = 39;
            this.pictureBox57.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox57, "Automatically detects your MS SQL Server version and then generates SQL Scripts b" +
        "ased on that version.");
            // 
            // pictureBox5
            // 
            this.pictureBox5.ErrorImage = null;
            this.pictureBox5.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox5.InitialImage = null;
            this.pictureBox5.Location = new System.Drawing.Point(12, 77);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(16, 16);
            this.pictureBox5.TabIndex = 41;
            this.pictureBox5.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox5, "Stored procedures generated will have a suffix that you provide.  E.g. \"Categorie" +
        "s_SelectAll_suffix\"");
            // 
            // pictureBox4
            // 
            this.pictureBox4.ErrorImage = null;
            this.pictureBox4.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox4.InitialImage = null;
            this.pictureBox4.Location = new System.Drawing.Point(12, 51);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(16, 16);
            this.pictureBox4.TabIndex = 40;
            this.pictureBox4.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox4, "Stored procedures generated will have a prefix that you provide.  E.g. \"prefix_Ca" +
        "tegories_SelectAll\"");
            // 
            // pictureBox3
            // 
            this.pictureBox3.ErrorImage = null;
            this.pictureBox3.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox3.InitialImage = null;
            this.pictureBox3.Location = new System.Drawing.Point(12, 27);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(16, 16);
            this.pictureBox3.TabIndex = 39;
            this.pictureBox3.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox3, "Stored procedures generated will have no prefix and no suffix.  E.g. \"Categories_" +
        "SelectAll\"");
            // 
            // pictureBox68
            // 
            this.pictureBox68.ErrorImage = null;
            this.pictureBox68.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox68.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox68.InitialImage = null;
            this.pictureBox68.Location = new System.Drawing.Point(230, 21);
            this.pictureBox68.Name = "pictureBox68";
            this.pictureBox68.Size = new System.Drawing.Size(16, 16);
            this.pictureBox68.TabIndex = 39;
            this.pictureBox68.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox68, "Generates Ad Hoc SQL Script inside your code using Classic .NET.");
            // 
            // pictureBox1
            // 
            this.pictureBox1.ErrorImage = null;
            this.pictureBox1.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(14, 22);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.TabIndex = 39;
            this.pictureBox1.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox1, "Generates Stored Procedures directly in the database using Classic .NET.");
            // 
            // pictureBox56
            // 
            this.pictureBox56.ErrorImage = null;
            this.pictureBox56.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox56.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox56.InitialImage = null;
            this.pictureBox56.Location = new System.Drawing.Point(12, 51);
            this.pictureBox56.Name = "pictureBox56";
            this.pictureBox56.Size = new System.Drawing.Size(16, 16);
            this.pictureBox56.TabIndex = 40;
            this.pictureBox56.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox56, "Generates SQL Scripts based on MS SQL 2008 and below.  Use this when your local M" +
        "S SQL used for development is a higher version than what you have in production." +
        "");
            // 
            // pictureBox47
            // 
            this.pictureBox47.ErrorImage = null;
            this.pictureBox47.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox47.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox47.InitialImage = null;
            this.pictureBox47.Location = new System.Drawing.Point(205, 482);
            this.pictureBox47.Name = "pictureBox47";
            this.pictureBox47.Size = new System.Drawing.Size(16, 16);
            this.pictureBox47.TabIndex = 25;
            this.pictureBox47.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox47, "Go to the Code Settings tab and choose Selected Views Only to enable the Load Vie" +
        "ws button");
            // 
            // pictureBox61
            // 
            this.pictureBox61.ErrorImage = null;
            this.pictureBox61.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox61.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox61.InitialImage = null;
            this.pictureBox61.Location = new System.Drawing.Point(13, 73);
            this.pictureBox61.Name = "pictureBox61";
            this.pictureBox61.Size = new System.Drawing.Size(16, 16);
            this.pictureBox61.TabIndex = 51;
            this.pictureBox61.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox61, "Port used when the Web app is ran.  E.g.  http://localhost:12345/.  12345 is the " +
        "port.  Only used during development.  ");
            // 
            // pictureBox60
            // 
            this.pictureBox60.ErrorImage = null;
            this.pictureBox60.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox60.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox60.InitialImage = null;
            this.pictureBox60.Location = new System.Drawing.Point(288, 72);
            this.pictureBox60.Name = "pictureBox60";
            this.pictureBox60.Size = new System.Drawing.Size(16, 16);
            this.pictureBox60.TabIndex = 42;
            this.pictureBox60.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox60, "Generates code examples");
            this.pictureBox60.Visible = false;
            // 
            // pictureBox41
            // 
            this.pictureBox41.ErrorImage = null;
            this.pictureBox41.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox41.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox41.InitialImage = null;
            this.pictureBox41.Location = new System.Drawing.Point(13, 22);
            this.pictureBox41.Name = "pictureBox41";
            this.pictureBox41.Size = new System.Drawing.Size(16, 16);
            this.pictureBox41.TabIndex = 39;
            this.pictureBox41.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox41, "The Name of the Web Application you want generated.  A folder will be generated a" +
        "s a root folder inside the Directory below.");
            // 
            // pictureBox14
            // 
            this.pictureBox14.ErrorImage = null;
            this.pictureBox14.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox14.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox14.InitialImage = null;
            this.pictureBox14.Location = new System.Drawing.Point(13, 47);
            this.pictureBox14.Name = "pictureBox14";
            this.pictureBox14.Size = new System.Drawing.Size(16, 16);
            this.pictureBox14.TabIndex = 38;
            this.pictureBox14.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox14, "The base directory where all ASP.Net 5Core objects (models, views, controllers, e" +
        "tc) and business objects are generated");
            // 
            // pictureBox58
            // 
            this.pictureBox58.ErrorImage = null;
            this.pictureBox58.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox58.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox58.InitialImage = null;
            this.pictureBox58.Location = new System.Drawing.Point(12, 50);
            this.pictureBox58.Name = "pictureBox58";
            this.pictureBox58.Size = new System.Drawing.Size(16, 16);
            this.pictureBox58.TabIndex = 48;
            this.pictureBox58.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox58, "The directory where the Web API project is generated in");
            // 
            // pictureBox59
            // 
            this.pictureBox59.ErrorImage = null;
            this.pictureBox59.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox59.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox59.InitialImage = null;
            this.pictureBox59.Location = new System.Drawing.Point(12, 23);
            this.pictureBox59.Name = "pictureBox59";
            this.pictureBox59.Size = new System.Drawing.Size(16, 16);
            this.pictureBox59.TabIndex = 45;
            this.pictureBox59.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox59, "Web API Name.  Name of the Web API project.");
            // 
            // pictureBox67
            // 
            this.pictureBox67.ErrorImage = null;
            this.pictureBox67.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox67.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox67.InitialImage = null;
            this.pictureBox67.Location = new System.Drawing.Point(13, 19);
            this.pictureBox67.Name = "pictureBox67";
            this.pictureBox67.Size = new System.Drawing.Size(16, 16);
            this.pictureBox67.TabIndex = 55;
            this.pictureBox67.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox67, "Generates Web Application which will encapsulate calls to the Business Objects.");
            // 
            // pictureBox48
            // 
            this.pictureBox48.ErrorImage = null;
            this.pictureBox48.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox48.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox48.InitialImage = null;
            this.pictureBox48.Location = new System.Drawing.Point(13, 157);
            this.pictureBox48.Name = "pictureBox48";
            this.pictureBox48.Size = new System.Drawing.Size(16, 16);
            this.pictureBox48.TabIndex = 57;
            this.pictureBox48.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox48, "Generates Web API which will encapsulate calls to the Business Objects.");
            // 
            // pictureBox37
            // 
            this.pictureBox37.ErrorImage = null;
            this.pictureBox37.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox37.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox37.InitialImage = null;
            this.pictureBox37.Location = new System.Drawing.Point(21, 30);
            this.pictureBox37.Name = "pictureBox37";
            this.pictureBox37.Size = new System.Drawing.Size(16, 16);
            this.pictureBox37.TabIndex = 55;
            this.pictureBox37.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox37, "Add Library which can be use to log the events of application");
            // 
            // pictureBox30
            // 
            this.pictureBox30.ErrorImage = null;
            this.pictureBox30.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox30.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox30.InitialImage = null;
            this.pictureBox30.Location = new System.Drawing.Point(14, 29);
            this.pictureBox30.Name = "pictureBox30";
            this.pictureBox30.Size = new System.Drawing.Size(16, 16);
            this.pictureBox30.TabIndex = 53;
            this.pictureBox30.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox30, "Application will perform logging in text file");
            // 
            // pictureBox25
            // 
            this.pictureBox25.ErrorImage = null;
            this.pictureBox25.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox25.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox25.InitialImage = null;
            this.pictureBox25.Location = new System.Drawing.Point(14, 56);
            this.pictureBox25.Name = "pictureBox25";
            this.pictureBox25.Size = new System.Drawing.Size(16, 16);
            this.pictureBox25.TabIndex = 54;
            this.pictureBox25.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox25, "Application will perform logging in Database");
            // 
            // pictureBox65
            // 
            this.pictureBox65.ErrorImage = null;
            this.pictureBox65.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox65.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox65.InitialImage = null;
            this.pictureBox65.Location = new System.Drawing.Point(14, 84);
            this.pictureBox65.Name = "pictureBox65";
            this.pictureBox65.Size = new System.Drawing.Size(16, 16);
            this.pictureBox65.TabIndex = 55;
            this.pictureBox65.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox65, "Application will perform logging in event");
            // 
            // pictureBox70
            // 
            this.pictureBox70.ErrorImage = null;
            this.pictureBox70.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox70.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox70.InitialImage = null;
            this.pictureBox70.Location = new System.Drawing.Point(21, 28);
            this.pictureBox70.Name = "pictureBox70";
            this.pictureBox70.Size = new System.Drawing.Size(16, 16);
            this.pictureBox70.TabIndex = 59;
            this.pictureBox70.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox70, "Add Library which can be use for caching of data in application");
            // 
            // pictureBox69
            // 
            this.pictureBox69.ErrorImage = null;
            this.pictureBox69.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox69.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox69.InitialImage = null;
            this.pictureBox69.Location = new System.Drawing.Point(21, 24);
            this.pictureBox69.Name = "pictureBox69";
            this.pictureBox69.Size = new System.Drawing.Size(16, 16);
            this.pictureBox69.TabIndex = 57;
            this.pictureBox69.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox69, "Add Library which can be use for encryption & decryption of data in application");
            // 
            // pictureBox71
            // 
            this.pictureBox71.ErrorImage = null;
            this.pictureBox71.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox71.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox71.InitialImage = null;
            this.pictureBox71.Location = new System.Drawing.Point(21, 28);
            this.pictureBox71.Name = "pictureBox71";
            this.pictureBox71.Size = new System.Drawing.Size(16, 16);
            this.pictureBox71.TabIndex = 59;
            this.pictureBox71.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox71, "Add Library which can be use to log the application audit details in audit tables" +
        ".");
            // 
            // PicBoxShowPassword
            // 
            this.PicBoxShowPassword.ErrorImage = null;
            this.PicBoxShowPassword.InitialImage = null;
            this.PicBoxShowPassword.Location = new System.Drawing.Point(340, 143);
            this.PicBoxShowPassword.Name = "PicBoxShowPassword";
            this.PicBoxShowPassword.Size = new System.Drawing.Size(18, 18);
            this.PicBoxShowPassword.TabIndex = 14;
            this.PicBoxShowPassword.TabStop = false;
            this.toolTip1.SetToolTip(this.PicBoxShowPassword, "Masks\' the password with an * when unchecked.  Shows the password when checked, a" +
        "nd remembers the last password you entered when you close the application.");
            // 
            // pictureBox78
            // 
            this.pictureBox78.ErrorImage = null;
            this.pictureBox78.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox78.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox78.InitialImage = null;
            this.pictureBox78.Location = new System.Drawing.Point(18, 77);
            this.pictureBox78.Name = "pictureBox78";
            this.pictureBox78.Size = new System.Drawing.Size(16, 16);
            this.pictureBox78.TabIndex = 56;
            this.pictureBox78.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox78, "Add Library which can be use to log the events of application");
            // 
            // pictureBox79
            // 
            this.pictureBox79.ErrorImage = null;
            this.pictureBox79.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox79.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox79.InitialImage = null;
            this.pictureBox79.Location = new System.Drawing.Point(18, 118);
            this.pictureBox79.Name = "pictureBox79";
            this.pictureBox79.Size = new System.Drawing.Size(16, 16);
            this.pictureBox79.TabIndex = 57;
            this.pictureBox79.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox79, "Add Library which can be use to log the events of application");
            // 
            // pictureBox80
            // 
            this.pictureBox80.ErrorImage = null;
            this.pictureBox80.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox80.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox80.InitialImage = null;
            this.pictureBox80.Location = new System.Drawing.Point(18, 163);
            this.pictureBox80.Name = "pictureBox80";
            this.pictureBox80.Size = new System.Drawing.Size(16, 16);
            this.pictureBox80.TabIndex = 58;
            this.pictureBox80.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox80, "Add Library which can be use to log the events of application");
            // 
            // pictureBox81
            // 
            this.pictureBox81.ErrorImage = null;
            this.pictureBox81.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox81.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox81.InitialImage = null;
            this.pictureBox81.Location = new System.Drawing.Point(18, 209);
            this.pictureBox81.Name = "pictureBox81";
            this.pictureBox81.Size = new System.Drawing.Size(16, 16);
            this.pictureBox81.TabIndex = 59;
            this.pictureBox81.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox81, "Add Library which can be use to log the events of application");
            // 
            // pictureBox82
            // 
            this.pictureBox82.ErrorImage = null;
            this.pictureBox82.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox82.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox82.InitialImage = null;
            this.pictureBox82.Location = new System.Drawing.Point(22, 29);
            this.pictureBox82.Name = "pictureBox82";
            this.pictureBox82.Size = new System.Drawing.Size(16, 16);
            this.pictureBox82.TabIndex = 60;
            this.pictureBox82.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox82, "Add Library which can be use to log the events of application");
            // 
            // pictureBox83
            // 
            this.pictureBox83.ErrorImage = null;
            this.pictureBox83.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox83.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox83.InitialImage = null;
            this.pictureBox83.Location = new System.Drawing.Point(245, 26);
            this.pictureBox83.Name = "pictureBox83";
            this.pictureBox83.Size = new System.Drawing.Size(16, 16);
            this.pictureBox83.TabIndex = 61;
            this.pictureBox83.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox83, "Add Library which can be use to log the events of application");
            // 
            // pictureBox46
            // 
            this.pictureBox46.ErrorImage = null;
            this.pictureBox46.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox46.Image")));
            this.pictureBox46.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox46.InitialImage = null;
            this.pictureBox46.Location = new System.Drawing.Point(208, 482);
            this.pictureBox46.Name = "pictureBox46";
            this.pictureBox46.Size = new System.Drawing.Size(16, 16);
            this.pictureBox46.TabIndex = 25;
            this.pictureBox46.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox46, "Go to the Code Settings tab and choose Selected Tables Only to enable the Load Ta" +
        "bles button");
            // 
            // LblProgress
            // 
            this.LblProgress.AutoSize = true;
            this.LblProgress.ForeColor = System.Drawing.Color.ForestGreen;
            this.LblProgress.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LblProgress.Location = new System.Drawing.Point(24, 633);
            this.LblProgress.Name = "LblProgress";
            this.LblProgress.Size = new System.Drawing.Size(0, 13);
            this.LblProgress.TabIndex = 38;
            // 
            // BackgroundWorker1
            // 
            this.BackgroundWorker1.WorkerReportsProgress = true;
            this.BackgroundWorker1.WorkerSupportsCancellation = true;
            this.BackgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker1_DoWork);
            this.BackgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BackgroundWorker1_ProgressChanged);
            this.BackgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker1_RunWorkerCompleted);
            // 
            // BtnClose
            // 
            this.BtnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.BtnClose.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnClose.Location = new System.Drawing.Point(27, 610);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(73, 34);
            this.BtnClose.TabIndex = 41;
            this.BtnClose.Text = "Close";
            this.BtnClose.UseVisualStyleBackColor = true;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // TxtPassword
            // 
            this.TxtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.TxtPassword.Location = new System.Drawing.Point(127, 97);
            this.TxtPassword.Name = "TxtPassword";
            this.TxtPassword.PasswordChar = '*';
            this.TxtPassword.Size = new System.Drawing.Size(196, 20);
            this.TxtPassword.TabIndex = 4;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pictureBox55);
            this.groupBox2.Controls.Add(this.pictureBox15);
            this.groupBox2.Controls.Add(this.TxtAPINameDirectory);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.pictureBox13);
            this.groupBox2.Controls.Add(this.LblLanguage);
            this.groupBox2.Controls.Add(this.CbxLanguage);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.TxtAPIName);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupBox2.Location = new System.Drawing.Point(13, 413);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(523, 117);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Business Layer and Data Layer API";
            // 
            // TxtAPINameDirectory
            // 
            this.TxtAPINameDirectory.Enabled = false;
            this.TxtAPINameDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.TxtAPINameDirectory.Location = new System.Drawing.Point(106, 81);
            this.TxtAPINameDirectory.Name = "TxtAPINameDirectory";
            this.TxtAPINameDirectory.Size = new System.Drawing.Size(351, 20);
            this.TxtAPINameDirectory.TabIndex = 40;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label8.Location = new System.Drawing.Point(33, 83);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 13);
            this.label8.TabIndex = 41;
            this.label8.Text = "Directory:";
            // 
            // LblLanguage
            // 
            this.LblLanguage.AutoSize = true;
            this.LblLanguage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.LblLanguage.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LblLanguage.Location = new System.Drawing.Point(34, 29);
            this.LblLanguage.Name = "LblLanguage";
            this.LblLanguage.Size = new System.Drawing.Size(58, 13);
            this.LblLanguage.TabIndex = 24;
            this.LblLanguage.Text = "Language:";
            // 
            // CbxLanguage
            // 
            this.CbxLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CbxLanguage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxLanguage.FormattingEnabled = true;
            this.CbxLanguage.Items.AddRange(new object[] {
            "C#",
            "VB.NET"});
            this.CbxLanguage.Location = new System.Drawing.Point(107, 26);
            this.CbxLanguage.Name = "CbxLanguage";
            this.CbxLanguage.Size = new System.Drawing.Size(115, 21);
            this.CbxLanguage.TabIndex = 23;
            this.CbxLanguage.SelectedIndexChanged += new System.EventHandler(this.CbxLanguage_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(33, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Name:";
            // 
            // TxtAPIName
            // 
            this.TxtAPIName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.TxtAPIName.Location = new System.Drawing.Point(106, 53);
            this.TxtAPIName.Name = "TxtAPIName";
            this.TxtAPIName.Size = new System.Drawing.Size(351, 20);
            this.TxtAPIName.TabIndex = 4;
            this.TxtAPIName.TextChanged += new System.EventHandler(this.TxtAPIName_TextChanged);
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.RbtnUseLinqToEntities);
            this.groupBox9.Controls.Add(this.pictureBox2);
            this.groupBox9.Location = new System.Drawing.Point(15, 21);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(446, 51);
            this.groupBox9.TabIndex = 41;
            this.groupBox9.TabStop = false;
            // 
            // RbtnUseLinqToEntities
            // 
            this.RbtnUseLinqToEntities.AutoSize = true;
            this.RbtnUseLinqToEntities.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.RbtnUseLinqToEntities.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.RbtnUseLinqToEntities.Location = new System.Drawing.Point(38, 20);
            this.RbtnUseLinqToEntities.Name = "RbtnUseLinqToEntities";
            this.RbtnUseLinqToEntities.Size = new System.Drawing.Size(231, 17);
            this.RbtnUseLinqToEntities.TabIndex = 7;
            this.RbtnUseLinqToEntities.Text = "Use Linq-to-Entities (Entity Framework Core)";
            this.RbtnUseLinqToEntities.UseVisualStyleBackColor = true;
            this.RbtnUseLinqToEntities.CheckedChanged += new System.EventHandler(this.RbtnUseLinqToEntities_CheckedChanged);
            // 
            // RbtnSqlScriptAutomatic
            // 
            this.RbtnSqlScriptAutomatic.AutoSize = true;
            this.RbtnSqlScriptAutomatic.Checked = true;
            this.RbtnSqlScriptAutomatic.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.RbtnSqlScriptAutomatic.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.RbtnSqlScriptAutomatic.Location = new System.Drawing.Point(40, 28);
            this.RbtnSqlScriptAutomatic.Name = "RbtnSqlScriptAutomatic";
            this.RbtnSqlScriptAutomatic.Size = new System.Drawing.Size(72, 17);
            this.RbtnSqlScriptAutomatic.TabIndex = 24;
            this.RbtnSqlScriptAutomatic.TabStop = true;
            this.RbtnSqlScriptAutomatic.Text = "Automatic";
            this.RbtnSqlScriptAutomatic.UseVisualStyleBackColor = true;
            // 
            // RbtnSqlScript2008
            // 
            this.RbtnSqlScript2008.AutoSize = true;
            this.RbtnSqlScript2008.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.RbtnSqlScript2008.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.RbtnSqlScript2008.Location = new System.Drawing.Point(40, 51);
            this.RbtnSqlScript2008.Name = "RbtnSqlScript2008";
            this.RbtnSqlScript2008.Size = new System.Drawing.Size(127, 17);
            this.RbtnSqlScript2008.TabIndex = 0;
            this.RbtnSqlScript2008.Text = "MS SQL 2008 & Below";
            this.RbtnSqlScript2008.UseVisualStyleBackColor = true;
            // 
            // RbtnNoPrefixOrSuffix
            // 
            this.RbtnNoPrefixOrSuffix.AutoSize = true;
            this.RbtnNoPrefixOrSuffix.Checked = true;
            this.RbtnNoPrefixOrSuffix.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.RbtnNoPrefixOrSuffix.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.RbtnNoPrefixOrSuffix.Location = new System.Drawing.Point(40, 28);
            this.RbtnNoPrefixOrSuffix.Name = "RbtnNoPrefixOrSuffix";
            this.RbtnNoPrefixOrSuffix.Size = new System.Drawing.Size(109, 17);
            this.RbtnNoPrefixOrSuffix.TabIndex = 24;
            this.RbtnNoPrefixOrSuffix.TabStop = true;
            this.RbtnNoPrefixOrSuffix.Text = "No Prefix or Suffix";
            this.RbtnNoPrefixOrSuffix.UseVisualStyleBackColor = true;
            this.RbtnNoPrefixOrSuffix.CheckedChanged += new System.EventHandler(this.RbtnNoPrefixOrSuffix_CheckedChanged);
            // 
            // RbtnSpSuffix
            // 
            this.RbtnSpSuffix.AutoSize = true;
            this.RbtnSpSuffix.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.RbtnSpSuffix.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.RbtnSpSuffix.Location = new System.Drawing.Point(40, 75);
            this.RbtnSpSuffix.Name = "RbtnSpSuffix";
            this.RbtnSpSuffix.Size = new System.Drawing.Size(51, 17);
            this.RbtnSpSuffix.TabIndex = 23;
            this.RbtnSpSuffix.Text = "Suffix";
            this.RbtnSpSuffix.UseVisualStyleBackColor = true;
            this.RbtnSpSuffix.CheckedChanged += new System.EventHandler(this.RbtnSpSuffix_CheckedChanged);
            // 
            // RbtnSpPrefix
            // 
            this.RbtnSpPrefix.AutoSize = true;
            this.RbtnSpPrefix.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.RbtnSpPrefix.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.RbtnSpPrefix.Location = new System.Drawing.Point(40, 51);
            this.RbtnSpPrefix.Name = "RbtnSpPrefix";
            this.RbtnSpPrefix.Size = new System.Drawing.Size(54, 17);
            this.RbtnSpPrefix.TabIndex = 0;
            this.RbtnSpPrefix.Text = "Prefix:";
            this.RbtnSpPrefix.UseVisualStyleBackColor = true;
            this.RbtnSpPrefix.CheckedChanged += new System.EventHandler(this.RbtnSpPrefix_CheckedChanged);
            // 
            // TxtSpSuffix
            // 
            this.TxtSpSuffix.Enabled = false;
            this.TxtSpSuffix.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.TxtSpSuffix.Location = new System.Drawing.Point(99, 76);
            this.TxtSpSuffix.Name = "TxtSpSuffix";
            this.TxtSpSuffix.Size = new System.Drawing.Size(92, 20);
            this.TxtSpSuffix.TabIndex = 20;
            // 
            // TxtSpPrefix
            // 
            this.TxtSpPrefix.Enabled = false;
            this.TxtSpPrefix.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.TxtSpPrefix.Location = new System.Drawing.Point(100, 50);
            this.TxtSpPrefix.Name = "TxtSpPrefix";
            this.TxtSpPrefix.Size = new System.Drawing.Size(92, 20);
            this.TxtSpPrefix.TabIndex = 22;
            // 
            // RbtnUseAdHocSql
            // 
            this.RbtnUseAdHocSql.AutoSize = true;
            this.RbtnUseAdHocSql.Checked = true;
            this.RbtnUseAdHocSql.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.RbtnUseAdHocSql.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.RbtnUseAdHocSql.Location = new System.Drawing.Point(254, 22);
            this.RbtnUseAdHocSql.Name = "RbtnUseAdHocSql";
            this.RbtnUseAdHocSql.Size = new System.Drawing.Size(153, 17);
            this.RbtnUseAdHocSql.TabIndex = 6;
            this.RbtnUseAdHocSql.TabStop = true;
            this.RbtnUseAdHocSql.Text = "Use Ad Hoc/Dynamic SQL";
            this.RbtnUseAdHocSql.UseVisualStyleBackColor = true;
            this.RbtnUseAdHocSql.CheckedChanged += new System.EventHandler(this.RbtnUseAdHocSql_CheckedChanged);
            // 
            // RbtnUseStoredProc
            // 
            this.RbtnUseStoredProc.AutoSize = true;
            this.RbtnUseStoredProc.Checked = true;
            this.RbtnUseStoredProc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.RbtnUseStoredProc.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.RbtnUseStoredProc.Location = new System.Drawing.Point(38, 23);
            this.RbtnUseStoredProc.Name = "RbtnUseStoredProc";
            this.RbtnUseStoredProc.Size = new System.Drawing.Size(135, 17);
            this.RbtnUseStoredProc.TabIndex = 6;
            this.RbtnUseStoredProc.TabStop = true;
            this.RbtnUseStoredProc.Text = "Use Stored Procedures";
            this.RbtnUseStoredProc.UseVisualStyleBackColor = true;
            this.RbtnUseStoredProc.CheckedChanged += new System.EventHandler(this.RbtnUseStoredProc_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pictureBox42);
            this.groupBox1.Controls.Add(this.PicbUserName);
            this.groupBox1.Controls.Add(this.PicbPassword);
            this.groupBox1.Controls.Add(this.PicbShowPassword);
            this.groupBox1.Controls.Add(this.PicbDatabaseName);
            this.groupBox1.Controls.Add(this.CbxRememberPassword);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.TxtServer);
            this.groupBox1.Controls.Add(this.TxtDatabase);
            this.groupBox1.Controls.Add(this.TxtUserName);
            this.groupBox1.Controls.Add(this.TxtPassword);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(11, 15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(530, 134);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Database Connection";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(34, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Password:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(34, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Database Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(34, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "User Name:";
            // 
            // BtnGenerateCode
            // 
            this.BtnGenerateCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.BtnGenerateCode.ForeColor = System.Drawing.Color.RoyalBlue;
            this.BtnGenerateCode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnGenerateCode.Location = new System.Drawing.Point(233, 612);
            this.BtnGenerateCode.Name = "BtnGenerateCode";
            this.BtnGenerateCode.Size = new System.Drawing.Size(278, 34);
            this.BtnGenerateCode.TabIndex = 39;
            this.BtnGenerateCode.Text = "Generate Code for All Tables";
            this.BtnGenerateCode.UseVisualStyleBackColor = true;
            this.BtnGenerateCode.Click += new System.EventHandler(this.BtnGenerateCode_Click);
            // 
            // TabGeneratedCode
            // 
            this.TabGeneratedCode.Controls.Add(this.GbxApplication);
            this.TabGeneratedCode.Controls.Add(this.groupBox3);
            this.TabGeneratedCode.Controls.Add(this.groupBox2);
            this.TabGeneratedCode.Location = new System.Drawing.Point(4, 22);
            this.TabGeneratedCode.Name = "TabGeneratedCode";
            this.TabGeneratedCode.Padding = new System.Windows.Forms.Padding(3);
            this.TabGeneratedCode.Size = new System.Drawing.Size(600, 547);
            this.TabGeneratedCode.TabIndex = 0;
            this.TabGeneratedCode.Text = "Code Settings";
            this.TabGeneratedCode.UseVisualStyleBackColor = true;
            // 
            // GbxApplication
            // 
            this.GbxApplication.Controls.Add(this.CbxUseWebApi);
            this.GbxApplication.Controls.Add(this.pictureBox48);
            this.GbxApplication.Controls.Add(this.CbxUseWebApplication);
            this.GbxApplication.Controls.Add(this.pictureBox67);
            this.GbxApplication.Controls.Add(this.GbxWebApi);
            this.GbxApplication.Controls.Add(this.groupBox4);
            this.GbxApplication.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.GbxApplication.Location = new System.Drawing.Point(13, 134);
            this.GbxApplication.Name = "GbxApplication";
            this.GbxApplication.Size = new System.Drawing.Size(523, 273);
            this.GbxApplication.TabIndex = 38;
            this.GbxApplication.TabStop = false;
            // 
            // CbxUseWebApi
            // 
            this.CbxUseWebApi.AutoSize = true;
            this.CbxUseWebApi.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxUseWebApi.Location = new System.Drawing.Point(38, 159);
            this.CbxUseWebApi.Name = "CbxUseWebApi";
            this.CbxUseWebApi.Size = new System.Drawing.Size(91, 17);
            this.CbxUseWebApi.TabIndex = 56;
            this.CbxUseWebApi.Text = "Use Web API";
            this.CbxUseWebApi.UseVisualStyleBackColor = true;
            this.CbxUseWebApi.CheckedChanged += new System.EventHandler(this.CbxUseWebApi_CheckedChanged);
            // 
            // CbxUseWebApplication
            // 
            this.CbxUseWebApplication.AutoSize = true;
            this.CbxUseWebApplication.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxUseWebApplication.Location = new System.Drawing.Point(38, 20);
            this.CbxUseWebApplication.Name = "CbxUseWebApplication";
            this.CbxUseWebApplication.Size = new System.Drawing.Size(126, 17);
            this.CbxUseWebApplication.TabIndex = 54;
            this.CbxUseWebApplication.Text = "Use Web Application";
            this.CbxUseWebApplication.UseVisualStyleBackColor = true;
            this.CbxUseWebApplication.CheckedChanged += new System.EventHandler(this.CbxUseWebApplication_CheckedChanged);
            // 
            // GbxWebApi
            // 
            this.GbxWebApi.Controls.Add(this.BtnBrowseWebAPIDirectory);
            this.GbxWebApi.Controls.Add(this.pictureBox58);
            this.GbxWebApi.Controls.Add(this.pictureBox59);
            this.GbxWebApi.Controls.Add(this.TxtWebAPINameDirectory);
            this.GbxWebApi.Controls.Add(this.label9);
            this.GbxWebApi.Controls.Add(this.label11);
            this.GbxWebApi.Controls.Add(this.TxtWebAPIName);
            this.GbxWebApi.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.GbxWebApi.Location = new System.Drawing.Point(13, 184);
            this.GbxWebApi.Name = "GbxWebApi";
            this.GbxWebApi.Size = new System.Drawing.Size(490, 80);
            this.GbxWebApi.TabIndex = 38;
            this.GbxWebApi.TabStop = false;
            this.GbxWebApi.Text = "Web API";
            // 
            // BtnBrowseWebAPIDirectory
            // 
            this.BtnBrowseWebAPIDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.BtnBrowseWebAPIDirectory.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnBrowseWebAPIDirectory.Location = new System.Drawing.Point(386, 49);
            this.BtnBrowseWebAPIDirectory.Name = "BtnBrowseWebAPIDirectory";
            this.BtnBrowseWebAPIDirectory.Size = new System.Drawing.Size(71, 20);
            this.BtnBrowseWebAPIDirectory.TabIndex = 49;
            this.BtnBrowseWebAPIDirectory.Text = "browse...";
            this.BtnBrowseWebAPIDirectory.UseVisualStyleBackColor = true;
            this.BtnBrowseWebAPIDirectory.Click += new System.EventHandler(this.BtnBrowseWebAPIDirectory_Click);
            // 
            // TxtWebAPINameDirectory
            // 
            this.TxtWebAPINameDirectory.Enabled = false;
            this.TxtWebAPINameDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.TxtWebAPINameDirectory.Location = new System.Drawing.Point(107, 49);
            this.TxtWebAPINameDirectory.Name = "TxtWebAPINameDirectory";
            this.TxtWebAPINameDirectory.Size = new System.Drawing.Size(273, 20);
            this.TxtWebAPINameDirectory.TabIndex = 46;
            this.TxtWebAPINameDirectory.TextChanged += new System.EventHandler(this.TxtWebAPINameDirectory_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label9.Location = new System.Drawing.Point(34, 51);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 13);
            this.label9.TabIndex = 47;
            this.label9.Text = "Directory:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label11.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label11.Location = new System.Drawing.Point(34, 24);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(38, 13);
            this.label11.TabIndex = 44;
            this.label11.Text = "Name:";
            // 
            // TxtWebAPIName
            // 
            this.TxtWebAPIName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.TxtWebAPIName.Location = new System.Drawing.Point(107, 21);
            this.TxtWebAPIName.Name = "TxtWebAPIName";
            this.TxtWebAPIName.Size = new System.Drawing.Size(351, 20);
            this.TxtWebAPIName.TabIndex = 43;
            this.TxtWebAPIName.TextChanged += new System.EventHandler(this.TxtWebAPIName_TextChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.pictureBox61);
            this.groupBox4.Controls.Add(this.CbxGenerateCodeExamples);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Controls.Add(this.TxtDevServerPort);
            this.groupBox4.Controls.Add(this.pictureBox60);
            this.groupBox4.Controls.Add(this.pictureBox41);
            this.groupBox4.Controls.Add(this.pictureBox14);
            this.groupBox4.Controls.Add(this.TxtDirectory);
            this.groupBox4.Controls.Add(this.BtnBrowseCodeDirectory);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.TxtWebAppName);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupBox4.Location = new System.Drawing.Point(13, 44);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(490, 100);
            this.groupBox4.TabIndex = 37;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Web Application";
            // 
            // CbxGenerateCodeExamples
            // 
            this.CbxGenerateCodeExamples.AutoSize = true;
            this.CbxGenerateCodeExamples.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxGenerateCodeExamples.Location = new System.Drawing.Point(312, 73);
            this.CbxGenerateCodeExamples.Name = "CbxGenerateCodeExamples";
            this.CbxGenerateCodeExamples.Size = new System.Drawing.Size(146, 17);
            this.CbxGenerateCodeExamples.TabIndex = 43;
            this.CbxGenerateCodeExamples.Text = "Generate Code Examples";
            this.CbxGenerateCodeExamples.UseVisualStyleBackColor = true;
            this.CbxGenerateCodeExamples.Visible = false;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label13.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label13.Location = new System.Drawing.Point(34, 74);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(86, 13);
            this.label13.TabIndex = 50;
            this.label13.Text = "Dev Server Port:";
            // 
            // TxtDevServerPort
            // 
            this.TxtDevServerPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.TxtDevServerPort.Location = new System.Drawing.Point(127, 71);
            this.TxtDevServerPort.Name = "TxtDevServerPort";
            this.TxtDevServerPort.Size = new System.Drawing.Size(136, 20);
            this.TxtDevServerPort.TabIndex = 49;
            // 
            // TxtDirectory
            // 
            this.TxtDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.TxtDirectory.Location = new System.Drawing.Point(127, 46);
            this.TxtDirectory.Name = "TxtDirectory";
            this.TxtDirectory.Size = new System.Drawing.Size(273, 20);
            this.TxtDirectory.TabIndex = 35;
            this.TxtDirectory.TextChanged += new System.EventHandler(this.TxtDirectory_TextChanged);
            // 
            // BtnBrowseCodeDirectory
            // 
            this.BtnBrowseCodeDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.BtnBrowseCodeDirectory.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnBrowseCodeDirectory.Location = new System.Drawing.Point(406, 46);
            this.BtnBrowseCodeDirectory.Name = "BtnBrowseCodeDirectory";
            this.BtnBrowseCodeDirectory.Size = new System.Drawing.Size(71, 20);
            this.BtnBrowseCodeDirectory.TabIndex = 36;
            this.BtnBrowseCodeDirectory.Text = "browse...";
            this.BtnBrowseCodeDirectory.UseVisualStyleBackColor = true;
            this.BtnBrowseCodeDirectory.Click += new System.EventHandler(this.BtnBrowseCodeDirectory_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(34, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 13);
            this.label6.TabIndex = 37;
            this.label6.Text = "Directory:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label12.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label12.Location = new System.Drawing.Point(34, 23);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(38, 13);
            this.label12.TabIndex = 19;
            this.label12.Text = "Name:";
            // 
            // TxtWebAppName
            // 
            this.TxtWebAppName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.TxtWebAppName.Location = new System.Drawing.Point(127, 20);
            this.TxtWebAppName.Name = "TxtWebAppName";
            this.TxtWebAppName.Size = new System.Drawing.Size(351, 20);
            this.TxtWebAppName.TabIndex = 4;
            this.TxtWebAppName.TextChanged += new System.EventHandler(this.TxtWebAppName_TextChanged);
            // 
            // GbxGeneratedSqlScript
            // 
            this.GbxGeneratedSqlScript.Controls.Add(this.pictureBox56);
            this.GbxGeneratedSqlScript.Controls.Add(this.pictureBox57);
            this.GbxGeneratedSqlScript.Controls.Add(this.RbtnSqlScriptAutomatic);
            this.GbxGeneratedSqlScript.Controls.Add(this.RbtnSqlScript2008);
            this.GbxGeneratedSqlScript.Location = new System.Drawing.Point(245, 126);
            this.GbxGeneratedSqlScript.Name = "GbxGeneratedSqlScript";
            this.GbxGeneratedSqlScript.Size = new System.Drawing.Size(201, 107);
            this.GbxGeneratedSqlScript.TabIndex = 42;
            this.GbxGeneratedSqlScript.TabStop = false;
            this.GbxGeneratedSqlScript.Text = "Generated SQL Script";
            // 
            // TabControl1
            // 
            this.TabControl1.Controls.Add(this.TabSelectedTables);
            this.TabControl1.Controls.Add(this.TabSelectedViews);
            this.TabControl1.Controls.Add(this.TabDatabase);
            this.TabControl1.Controls.Add(this.TabGeneratedCode);
            this.TabControl1.Controls.Add(this.TabUISettings);
            this.TabControl1.Controls.Add(this.TabAppSettings);
            this.TabControl1.Controls.Add(this.TabCompSettings);
            this.TabControl1.Location = new System.Drawing.Point(23, 16);
            this.TabControl1.Name = "TabControl1";
            this.TabControl1.SelectedIndex = 0;
            this.TabControl1.Size = new System.Drawing.Size(608, 573);
            this.TabControl1.TabIndex = 36;
            this.TabControl1.Click += new System.EventHandler(this.TabControl1_SelectIndexChanged);
            // 
            // TabSelectedTables
            // 
            this.TabSelectedTables.AutoScrollMargin = new System.Drawing.Size(0, 10);
            this.TabSelectedTables.Controls.Add(this.pictureBox46);
            this.TabSelectedTables.Controls.Add(this.BtnClearSelectedTables);
            this.TabSelectedTables.Controls.Add(this.BtnLoadTables);
            this.TabSelectedTables.Controls.Add(this.ClbxTables);
            this.TabSelectedTables.Location = new System.Drawing.Point(4, 22);
            this.TabSelectedTables.Name = "TabSelectedTables";
            this.TabSelectedTables.Padding = new System.Windows.Forms.Padding(3);
            this.TabSelectedTables.Size = new System.Drawing.Size(600, 547);
            this.TabSelectedTables.TabIndex = 1;
            this.TabSelectedTables.Text = "Selected Tables";
            this.TabSelectedTables.UseVisualStyleBackColor = true;
            // 
            // BtnClearSelectedTables
            // 
            this.BtnClearSelectedTables.Enabled = false;
            this.BtnClearSelectedTables.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnClearSelectedTables.Location = new System.Drawing.Point(111, 476);
            this.BtnClearSelectedTables.Name = "BtnClearSelectedTables";
            this.BtnClearSelectedTables.Size = new System.Drawing.Size(89, 28);
            this.BtnClearSelectedTables.TabIndex = 2;
            this.BtnClearSelectedTables.Text = "Select All";
            this.BtnClearSelectedTables.UseVisualStyleBackColor = true;
            this.BtnClearSelectedTables.Click += new System.EventHandler(this.BtnClearSelectedTables_Click);
            // 
            // BtnLoadTables
            // 
            this.BtnLoadTables.Enabled = false;
            this.BtnLoadTables.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnLoadTables.Location = new System.Drawing.Point(13, 476);
            this.BtnLoadTables.Name = "BtnLoadTables";
            this.BtnLoadTables.Size = new System.Drawing.Size(92, 28);
            this.BtnLoadTables.TabIndex = 1;
            this.BtnLoadTables.Text = "Load Tables";
            this.BtnLoadTables.UseVisualStyleBackColor = true;
            this.BtnLoadTables.Click += new System.EventHandler(this.BtnLoadTables_Click);
            // 
            // ClbxTables
            // 
            this.ClbxTables.CheckOnClick = true;
            this.ClbxTables.Enabled = false;
            this.ClbxTables.FormattingEnabled = true;
            this.ClbxTables.Location = new System.Drawing.Point(13, 16);
            this.ClbxTables.Name = "ClbxTables";
            this.ClbxTables.Size = new System.Drawing.Size(524, 454);
            this.ClbxTables.TabIndex = 0;
            this.ClbxTables.SelectedIndexChanged += new System.EventHandler(this.ClbxTables_SelectedIndexChanged);
            // 
            // TabSelectedViews
            // 
            this.TabSelectedViews.Controls.Add(this.pictureBox47);
            this.TabSelectedViews.Controls.Add(this.BtnClearSelectedViews);
            this.TabSelectedViews.Controls.Add(this.BtnLoadViews);
            this.TabSelectedViews.Controls.Add(this.ClbxViews);
            this.TabSelectedViews.Location = new System.Drawing.Point(4, 22);
            this.TabSelectedViews.Name = "TabSelectedViews";
            this.TabSelectedViews.Size = new System.Drawing.Size(600, 547);
            this.TabSelectedViews.TabIndex = 3;
            this.TabSelectedViews.Text = "Selected Views";
            this.TabSelectedViews.UseVisualStyleBackColor = true;
            // 
            // BtnClearSelectedViews
            // 
            this.BtnClearSelectedViews.Enabled = false;
            this.BtnClearSelectedViews.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnClearSelectedViews.Location = new System.Drawing.Point(111, 476);
            this.BtnClearSelectedViews.Name = "BtnClearSelectedViews";
            this.BtnClearSelectedViews.Size = new System.Drawing.Size(89, 28);
            this.BtnClearSelectedViews.TabIndex = 5;
            this.BtnClearSelectedViews.Text = "Select All";
            this.BtnClearSelectedViews.UseVisualStyleBackColor = true;
            this.BtnClearSelectedViews.Click += new System.EventHandler(this.BtnClearSelectedViews_Click);
            // 
            // BtnLoadViews
            // 
            this.BtnLoadViews.Enabled = false;
            this.BtnLoadViews.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnLoadViews.Location = new System.Drawing.Point(13, 476);
            this.BtnLoadViews.Name = "BtnLoadViews";
            this.BtnLoadViews.Size = new System.Drawing.Size(92, 28);
            this.BtnLoadViews.TabIndex = 4;
            this.BtnLoadViews.Text = "Load Views";
            this.BtnLoadViews.UseVisualStyleBackColor = true;
            this.BtnLoadViews.Click += new System.EventHandler(this.BtnLoadViews_Click);
            // 
            // ClbxViews
            // 
            this.ClbxViews.CheckOnClick = true;
            this.ClbxViews.Enabled = false;
            this.ClbxViews.FormattingEnabled = true;
            this.ClbxViews.Location = new System.Drawing.Point(13, 16);
            this.ClbxViews.Name = "ClbxViews";
            this.ClbxViews.Size = new System.Drawing.Size(526, 454);
            this.ClbxViews.TabIndex = 3;
            this.ClbxViews.SelectedIndexChanged += new System.EventHandler(this.ClbxViews_SelectedIndexChanged);
            // 
            // TabDatabase
            // 
            this.TabDatabase.Controls.Add(this.GbxGenerateSql);
            this.TabDatabase.Controls.Add(this.groupBox1);
            this.TabDatabase.Location = new System.Drawing.Point(4, 22);
            this.TabDatabase.Name = "TabDatabase";
            this.TabDatabase.Padding = new System.Windows.Forms.Padding(3);
            this.TabDatabase.Size = new System.Drawing.Size(600, 547);
            this.TabDatabase.TabIndex = 2;
            this.TabDatabase.Text = "Database Settings";
            this.TabDatabase.UseVisualStyleBackColor = true;
            // 
            // GbxGenerateSql
            // 
            this.GbxGenerateSql.Controls.Add(this.groupBox9);
            this.GbxGenerateSql.Controls.Add(this.GbxGeneratedSqlScript);
            this.GbxGenerateSql.Controls.Add(this.GbxStoredProcedure);
            this.GbxGenerateSql.Controls.Add(this.GboxStoredProcOrLinq);
            this.GbxGenerateSql.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.GbxGenerateSql.Location = new System.Drawing.Point(11, 166);
            this.GbxGenerateSql.Name = "GbxGenerateSql";
            this.GbxGenerateSql.Size = new System.Drawing.Size(530, 265);
            this.GbxGenerateSql.TabIndex = 25;
            this.GbxGenerateSql.TabStop = false;
            this.GbxGenerateSql.Text = "Generated SQL";
            // 
            // GbxStoredProcedure
            // 
            this.GbxStoredProcedure.Controls.Add(this.pictureBox5);
            this.GbxStoredProcedure.Controls.Add(this.pictureBox4);
            this.GbxStoredProcedure.Controls.Add(this.pictureBox3);
            this.GbxStoredProcedure.Controls.Add(this.RbtnNoPrefixOrSuffix);
            this.GbxStoredProcedure.Controls.Add(this.RbtnSpSuffix);
            this.GbxStoredProcedure.Controls.Add(this.RbtnSpPrefix);
            this.GbxStoredProcedure.Controls.Add(this.TxtSpSuffix);
            this.GbxStoredProcedure.Controls.Add(this.TxtSpPrefix);
            this.GbxStoredProcedure.Location = new System.Drawing.Point(30, 126);
            this.GbxStoredProcedure.Name = "GbxStoredProcedure";
            this.GbxStoredProcedure.Size = new System.Drawing.Size(201, 107);
            this.GbxStoredProcedure.TabIndex = 23;
            this.GbxStoredProcedure.TabStop = false;
            this.GbxStoredProcedure.Text = "Stored Procedure";
            // 
            // GboxStoredProcOrLinq
            // 
            this.GboxStoredProcOrLinq.Controls.Add(this.pictureBox68);
            this.GboxStoredProcOrLinq.Controls.Add(this.RbtnUseAdHocSql);
            this.GboxStoredProcOrLinq.Controls.Add(this.pictureBox1);
            this.GboxStoredProcOrLinq.Controls.Add(this.RbtnUseStoredProc);
            this.GboxStoredProcOrLinq.Location = new System.Drawing.Point(15, 65);
            this.GboxStoredProcOrLinq.Name = "GboxStoredProcOrLinq";
            this.GboxStoredProcOrLinq.Size = new System.Drawing.Size(446, 184);
            this.GboxStoredProcOrLinq.TabIndex = 2;
            this.GboxStoredProcOrLinq.TabStop = false;
            // 
            // TabCompSettings
            // 
            this.TabCompSettings.AutoScroll = true;
            this.TabCompSettings.AutoScrollMargin = new System.Drawing.Size(10, 20);
            this.TabCompSettings.Controls.Add(this.tabControl2);
            this.TabCompSettings.Location = new System.Drawing.Point(4, 22);
            this.TabCompSettings.Margin = new System.Windows.Forms.Padding(0);
            this.TabCompSettings.Name = "TabCompSettings";
            this.TabCompSettings.Size = new System.Drawing.Size(600, 547);
            this.TabCompSettings.TabIndex = 6;
            this.TabCompSettings.Text = "Components Settings";
            this.TabCompSettings.UseVisualStyleBackColor = true;
            this.TabCompSettings.Click += new System.EventHandler(this.TabCompSettings_Click);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabComponents);
            this.tabControl2.Controls.Add(this.tabWorkflow);
            this.tabControl2.Location = new System.Drawing.Point(14, 3);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(559, 800);
            this.tabControl2.TabIndex = 39;
            // 
            // tabComponents
            // 
            this.tabComponents.Controls.Add(this.GbxConfiguration);
            this.tabComponents.Controls.Add(this.GbxAuditLogging);
            this.tabComponents.Controls.Add(this.GbxLogging);
            this.tabComponents.Controls.Add(this.GbxCaching);
            this.tabComponents.Controls.Add(this.GbxSecurity);
            this.tabComponents.Location = new System.Drawing.Point(4, 22);
            this.tabComponents.Name = "tabComponents";
            this.tabComponents.Padding = new System.Windows.Forms.Padding(3);
            this.tabComponents.Size = new System.Drawing.Size(551, 774);
            this.tabComponents.TabIndex = 0;
            this.tabComponents.Text = "Components";
            this.tabComponents.UseVisualStyleBackColor = true;
            // 
            // GbxConfiguration
            // 
            this.GbxConfiguration.Controls.Add(this.GbxSMTPSettings);
            this.GbxConfiguration.Controls.Add(this.CbxEmailNotification);
            this.GbxConfiguration.Controls.Add(this.pictureBox72);
            this.GbxConfiguration.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.GbxConfiguration.Location = new System.Drawing.Point(18, 472);
            this.GbxConfiguration.Name = "GbxConfiguration";
            this.GbxConfiguration.Size = new System.Drawing.Size(526, 293);
            this.GbxConfiguration.TabIndex = 62;
            this.GbxConfiguration.TabStop = false;
            this.GbxConfiguration.Text = "Configuration";
            // 
            // GbxSMTPSettings
            // 
            this.GbxSMTPSettings.Controls.Add(this.textBox2);
            this.GbxSMTPSettings.Controls.Add(this.PicBoxShowPassword);
            this.GbxSMTPSettings.Controls.Add(this.btnTestCionnection);
            this.GbxSMTPSettings.Controls.Add(this.CbxShowPassword);
            this.GbxSMTPSettings.Controls.Add(this.pictureBox77);
            this.GbxSMTPSettings.Controls.Add(this.pictureBox76);
            this.GbxSMTPSettings.Controls.Add(this.pictureBox75);
            this.GbxSMTPSettings.Controls.Add(this.pictureBox74);
            this.GbxSMTPSettings.Controls.Add(this.TxtPasswordForSmtp);
            this.GbxSMTPSettings.Controls.Add(this.TxtUserNameForSmtp);
            this.GbxSMTPSettings.Controls.Add(this.textBox1);
            this.GbxSMTPSettings.Controls.Add(this.label17);
            this.GbxSMTPSettings.Controls.Add(this.label16);
            this.GbxSMTPSettings.Controls.Add(this.label15);
            this.GbxSMTPSettings.Controls.Add(this.label14);
            this.GbxSMTPSettings.Enabled = false;
            this.GbxSMTPSettings.Location = new System.Drawing.Point(21, 63);
            this.GbxSMTPSettings.Name = "GbxSMTPSettings";
            this.GbxSMTPSettings.Size = new System.Drawing.Size(486, 222);
            this.GbxSMTPSettings.TabIndex = 63;
            this.GbxSMTPSettings.TabStop = false;
            this.GbxSMTPSettings.Text = "SMTPSettings";
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.textBox2.Location = new System.Drawing.Point(107, 64);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(212, 20);
            this.textBox2.TabIndex = 5;
            this.textBox2.Text = "127.0.0.1";
            // 
            // btnTestCionnection
            // 
            this.btnTestCionnection.Enabled = false;
            this.btnTestCionnection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnTestCionnection.Location = new System.Drawing.Point(326, 180);
            this.btnTestCionnection.Name = "btnTestCionnection";
            this.btnTestCionnection.Size = new System.Drawing.Size(100, 25);
            this.btnTestCionnection.TabIndex = 13;
            this.btnTestCionnection.Text = "Test Connection";
            this.btnTestCionnection.UseVisualStyleBackColor = true;
            // 
            // CbxShowPassword
            // 
            this.CbxShowPassword.AutoSize = true;
            this.CbxShowPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxShowPassword.Location = new System.Drawing.Point(364, 144);
            this.CbxShowPassword.Name = "CbxShowPassword";
            this.CbxShowPassword.Size = new System.Drawing.Size(102, 17);
            this.CbxShowPassword.TabIndex = 12;
            this.CbxShowPassword.Text = "Show Password";
            this.CbxShowPassword.UseVisualStyleBackColor = true;
            this.CbxShowPassword.CheckedChanged += new System.EventHandler(this.CbxShowPassword_CheckedChanged);
            // 
            // pictureBox77
            // 
            this.pictureBox77.ErrorImage = null;
            this.pictureBox77.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox77.InitialImage = null;
            this.pictureBox77.Location = new System.Drawing.Point(14, 141);
            this.pictureBox77.Name = "pictureBox77";
            this.pictureBox77.Size = new System.Drawing.Size(16, 16);
            this.pictureBox77.TabIndex = 11;
            this.pictureBox77.TabStop = false;
            // 
            // pictureBox76
            // 
            this.pictureBox76.ErrorImage = null;
            this.pictureBox76.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox76.InitialImage = null;
            this.pictureBox76.Location = new System.Drawing.Point(14, 64);
            this.pictureBox76.Name = "pictureBox76";
            this.pictureBox76.Size = new System.Drawing.Size(16, 16);
            this.pictureBox76.TabIndex = 10;
            this.pictureBox76.TabStop = false;
            // 
            // pictureBox75
            // 
            this.pictureBox75.ErrorImage = null;
            this.pictureBox75.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox75.InitialImage = null;
            this.pictureBox75.Location = new System.Drawing.Point(14, 105);
            this.pictureBox75.Name = "pictureBox75";
            this.pictureBox75.Size = new System.Drawing.Size(16, 16);
            this.pictureBox75.TabIndex = 9;
            this.pictureBox75.TabStop = false;
            // 
            // pictureBox74
            // 
            this.pictureBox74.ErrorImage = null;
            this.pictureBox74.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox74.InitialImage = null;
            this.pictureBox74.Location = new System.Drawing.Point(14, 28);
            this.pictureBox74.Name = "pictureBox74";
            this.pictureBox74.Size = new System.Drawing.Size(16, 16);
            this.pictureBox74.TabIndex = 8;
            this.pictureBox74.TabStop = false;
            // 
            // TxtPasswordForSmtp
            // 
            this.TxtPasswordForSmtp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.TxtPasswordForSmtp.Location = new System.Drawing.Point(107, 141);
            this.TxtPasswordForSmtp.Name = "TxtPasswordForSmtp";
            this.TxtPasswordForSmtp.PasswordChar = '*';
            this.TxtPasswordForSmtp.Size = new System.Drawing.Size(212, 20);
            this.TxtPasswordForSmtp.TabIndex = 7;
            // 
            // TxtUserNameForSmtp
            // 
            this.TxtUserNameForSmtp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.TxtUserNameForSmtp.Location = new System.Drawing.Point(107, 105);
            this.TxtUserNameForSmtp.Name = "TxtUserNameForSmtp";
            this.TxtUserNameForSmtp.Size = new System.Drawing.Size(212, 20);
            this.TxtUserNameForSmtp.TabIndex = 6;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.textBox1.Location = new System.Drawing.Point(107, 28);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(212, 20);
            this.textBox1.TabIndex = 4;
            this.textBox1.Text = "25";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label17.Location = new System.Drawing.Point(35, 141);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(56, 13);
            this.label17.TabIndex = 3;
            this.label17.Text = "Password:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label16.Location = new System.Drawing.Point(36, 108);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(60, 13);
            this.label16.TabIndex = 2;
            this.label16.Text = "UserName:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label15.Location = new System.Drawing.Point(36, 67);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(56, 13);
            this.label15.TabIndex = 1;
            this.label15.Text = "SmtpHost:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label14.Location = new System.Drawing.Point(35, 28);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 13);
            this.label14.TabIndex = 0;
            this.label14.Text = "SmtpPort:";
            // 
            // CbxEmailNotification
            // 
            this.CbxEmailNotification.AutoSize = true;
            this.CbxEmailNotification.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxEmailNotification.Location = new System.Drawing.Point(45, 28);
            this.CbxEmailNotification.Name = "CbxEmailNotification";
            this.CbxEmailNotification.Size = new System.Drawing.Size(104, 17);
            this.CbxEmailNotification.TabIndex = 58;
            this.CbxEmailNotification.Text = "EmailNotification";
            this.CbxEmailNotification.UseVisualStyleBackColor = true;
            this.CbxEmailNotification.CheckedChanged += new System.EventHandler(this.CbxEmailNotification_CheckedChanged);
            // 
            // pictureBox72
            // 
            this.pictureBox72.ErrorImage = null;
            this.pictureBox72.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox72.InitialImage = null;
            this.pictureBox72.Location = new System.Drawing.Point(21, 28);
            this.pictureBox72.Name = "pictureBox72";
            this.pictureBox72.Size = new System.Drawing.Size(16, 16);
            this.pictureBox72.TabIndex = 0;
            this.pictureBox72.TabStop = false;
            // 
            // GbxAuditLogging
            // 
            this.GbxAuditLogging.Controls.Add(this.CbxUseAuditLogging);
            this.GbxAuditLogging.Controls.Add(this.pictureBox71);
            this.GbxAuditLogging.Location = new System.Drawing.Point(18, 394);
            this.GbxAuditLogging.Name = "GbxAuditLogging";
            this.GbxAuditLogging.Size = new System.Drawing.Size(526, 63);
            this.GbxAuditLogging.TabIndex = 61;
            this.GbxAuditLogging.TabStop = false;
            this.GbxAuditLogging.Text = "Audit Logging";
            // 
            // CbxUseAuditLogging
            // 
            this.CbxUseAuditLogging.AutoSize = true;
            this.CbxUseAuditLogging.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxUseAuditLogging.Location = new System.Drawing.Point(45, 28);
            this.CbxUseAuditLogging.Name = "CbxUseAuditLogging";
            this.CbxUseAuditLogging.Size = new System.Drawing.Size(113, 17);
            this.CbxUseAuditLogging.TabIndex = 58;
            this.CbxUseAuditLogging.Text = "Use Audit Logging";
            this.CbxUseAuditLogging.UseVisualStyleBackColor = true;
            // 
            // GbxLogging
            // 
            this.GbxLogging.Controls.Add(this.GbxLoggingType);
            this.GbxLogging.Controls.Add(this.CbxUseLogging);
            this.GbxLogging.Controls.Add(this.pictureBox37);
            this.GbxLogging.Location = new System.Drawing.Point(18, 8);
            this.GbxLogging.Name = "GbxLogging";
            this.GbxLogging.Size = new System.Drawing.Size(526, 211);
            this.GbxLogging.TabIndex = 58;
            this.GbxLogging.TabStop = false;
            this.GbxLogging.Text = "Logging";
            // 
            // GbxLoggingType
            // 
            this.GbxLoggingType.Controls.Add(this.CbxUseEventLogging);
            this.GbxLoggingType.Controls.Add(this.CbxUseDatabaseLogging);
            this.GbxLoggingType.Controls.Add(this.CbxUseFileLogging);
            this.GbxLoggingType.Controls.Add(this.pictureBox65);
            this.GbxLoggingType.Controls.Add(this.pictureBox25);
            this.GbxLoggingType.Controls.Add(this.pictureBox30);
            this.GbxLoggingType.Enabled = false;
            this.GbxLoggingType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.GbxLoggingType.Location = new System.Drawing.Point(21, 65);
            this.GbxLoggingType.Name = "GbxLoggingType";
            this.GbxLoggingType.Size = new System.Drawing.Size(472, 116);
            this.GbxLoggingType.TabIndex = 56;
            this.GbxLoggingType.TabStop = false;
            this.GbxLoggingType.Text = "Logging Type";
            // 
            // CbxUseEventLogging
            // 
            this.CbxUseEventLogging.AutoSize = true;
            this.CbxUseEventLogging.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxUseEventLogging.Location = new System.Drawing.Point(38, 84);
            this.CbxUseEventLogging.Name = "CbxUseEventLogging";
            this.CbxUseEventLogging.Size = new System.Drawing.Size(95, 17);
            this.CbxUseEventLogging.TabIndex = 58;
            this.CbxUseEventLogging.Text = "Event Logging";
            this.CbxUseEventLogging.UseVisualStyleBackColor = true;
            // 
            // CbxUseDatabaseLogging
            // 
            this.CbxUseDatabaseLogging.AutoSize = true;
            this.CbxUseDatabaseLogging.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxUseDatabaseLogging.Location = new System.Drawing.Point(38, 57);
            this.CbxUseDatabaseLogging.Name = "CbxUseDatabaseLogging";
            this.CbxUseDatabaseLogging.Size = new System.Drawing.Size(113, 17);
            this.CbxUseDatabaseLogging.TabIndex = 57;
            this.CbxUseDatabaseLogging.Text = "Database Logging";
            this.CbxUseDatabaseLogging.UseVisualStyleBackColor = true;
            // 
            // CbxUseFileLogging
            // 
            this.CbxUseFileLogging.AutoSize = true;
            this.CbxUseFileLogging.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxUseFileLogging.Location = new System.Drawing.Point(38, 30);
            this.CbxUseFileLogging.Name = "CbxUseFileLogging";
            this.CbxUseFileLogging.Size = new System.Drawing.Size(83, 17);
            this.CbxUseFileLogging.TabIndex = 56;
            this.CbxUseFileLogging.Text = "File Logging";
            this.CbxUseFileLogging.UseVisualStyleBackColor = true;
            // 
            // CbxUseLogging
            // 
            this.CbxUseLogging.AutoSize = true;
            this.CbxUseLogging.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxUseLogging.Location = new System.Drawing.Point(45, 31);
            this.CbxUseLogging.Name = "CbxUseLogging";
            this.CbxUseLogging.Size = new System.Drawing.Size(86, 17);
            this.CbxUseLogging.TabIndex = 54;
            this.CbxUseLogging.Text = "Use Logging";
            this.CbxUseLogging.UseVisualStyleBackColor = true;
            this.CbxUseLogging.CheckedChanged += new System.EventHandler(this.CbxUseLogging_CheckedChanged);
            // 
            // GbxCaching
            // 
            this.GbxCaching.Controls.Add(this.CbxUseCaching);
            this.GbxCaching.Controls.Add(this.pictureBox70);
            this.GbxCaching.Location = new System.Drawing.Point(18, 310);
            this.GbxCaching.Name = "GbxCaching";
            this.GbxCaching.Size = new System.Drawing.Size(526, 63);
            this.GbxCaching.TabIndex = 59;
            this.GbxCaching.TabStop = false;
            this.GbxCaching.Text = "Caching";
            // 
            // CbxUseCaching
            // 
            this.CbxUseCaching.AutoSize = true;
            this.CbxUseCaching.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxUseCaching.Location = new System.Drawing.Point(45, 28);
            this.CbxUseCaching.Name = "CbxUseCaching";
            this.CbxUseCaching.Size = new System.Drawing.Size(87, 17);
            this.CbxUseCaching.TabIndex = 58;
            this.CbxUseCaching.Text = "Use Caching";
            this.CbxUseCaching.UseVisualStyleBackColor = true;
            // 
            // GbxSecurity
            // 
            this.GbxSecurity.Controls.Add(this.CbxUseSecurity);
            this.GbxSecurity.Controls.Add(this.pictureBox69);
            this.GbxSecurity.Location = new System.Drawing.Point(18, 234);
            this.GbxSecurity.Name = "GbxSecurity";
            this.GbxSecurity.Size = new System.Drawing.Size(526, 63);
            this.GbxSecurity.TabIndex = 60;
            this.GbxSecurity.TabStop = false;
            this.GbxSecurity.Text = "Security";
            // 
            // CbxUseSecurity
            // 
            this.CbxUseSecurity.AutoSize = true;
            this.CbxUseSecurity.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxUseSecurity.Location = new System.Drawing.Point(45, 24);
            this.CbxUseSecurity.Name = "CbxUseSecurity";
            this.CbxUseSecurity.Size = new System.Drawing.Size(86, 17);
            this.CbxUseSecurity.TabIndex = 56;
            this.CbxUseSecurity.Text = "Use Security";
            this.CbxUseSecurity.UseVisualStyleBackColor = true;
            // 
            // tabWorkflow
            // 
            this.tabWorkflow.BackColor = System.Drawing.Color.White;
            this.tabWorkflow.Controls.Add(this.gbxWorkflowSettings);
            this.tabWorkflow.Controls.Add(this.CbxWorkflow);
            this.tabWorkflow.Controls.Add(this.pictureBox73);
            this.tabWorkflow.Location = new System.Drawing.Point(4, 22);
            this.tabWorkflow.Name = "tabWorkflow";
            this.tabWorkflow.Padding = new System.Windows.Forms.Padding(3);
            this.tabWorkflow.Size = new System.Drawing.Size(551, 774);
            this.tabWorkflow.TabIndex = 1;
            this.tabWorkflow.Text = "Workflow";
            // 
            // gbxWorkflowSettings
            // 
            this.gbxWorkflowSettings.Controls.Add(this.rdoDefaultWorkflows);
            this.gbxWorkflowSettings.Controls.Add(this.rdoConfigurableWorkflows);
            this.gbxWorkflowSettings.Controls.Add(this.pictureBox78);
            this.gbxWorkflowSettings.Controls.Add(this.btnCancelWorkflow);
            this.gbxWorkflowSettings.Controls.Add(this.pictureBox81);
            this.gbxWorkflowSettings.Controls.Add(this.btnFinalize);
            this.gbxWorkflowSettings.Controls.Add(this.lblNumberofWorkflows);
            this.gbxWorkflowSettings.Controls.Add(this.grpExceptionFlow);
            this.gbxWorkflowSettings.Controls.Add(this.pictureBox80);
            this.gbxWorkflowSettings.Controls.Add(this.txtEscalationTime);
            this.gbxWorkflowSettings.Controls.Add(this.txtNumberofWorflows);
            this.gbxWorkflowSettings.Controls.Add(this.lbEscalationTime);
            this.gbxWorkflowSettings.Controls.Add(this.pictureBox79);
            this.gbxWorkflowSettings.Controls.Add(this.btnAdd);
            this.gbxWorkflowSettings.Controls.Add(this.lblWorkflowName);
            this.gbxWorkflowSettings.Controls.Add(this.txtNoOfSteps);
            this.gbxWorkflowSettings.Controls.Add(this.txtWorkflowName);
            this.gbxWorkflowSettings.Controls.Add(this.lblNoOfSteps);
            this.gbxWorkflowSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxWorkflowSettings.Location = new System.Drawing.Point(31, 61);
            this.gbxWorkflowSettings.Name = "gbxWorkflowSettings";
            this.gbxWorkflowSettings.Size = new System.Drawing.Size(504, 461);
            this.gbxWorkflowSettings.TabIndex = 62;
            this.gbxWorkflowSettings.TabStop = false;
            this.gbxWorkflowSettings.Text = "Workflow Settings";
            // 
            // rdoDefaultWorkflows
            // 
            this.rdoDefaultWorkflows.AutoSize = true;
            this.rdoDefaultWorkflows.Checked = true;
            this.rdoDefaultWorkflows.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoDefaultWorkflows.Location = new System.Drawing.Point(14, 30);
            this.rdoDefaultWorkflows.Name = "rdoDefaultWorkflows";
            this.rdoDefaultWorkflows.Size = new System.Drawing.Size(112, 17);
            this.rdoDefaultWorkflows.TabIndex = 0;
            this.rdoDefaultWorkflows.TabStop = true;
            this.rdoDefaultWorkflows.Text = "Default Workflows";
            this.rdoDefaultWorkflows.UseVisualStyleBackColor = true;
            this.rdoDefaultWorkflows.CheckedChanged += new System.EventHandler(this.rdoDefaultWorkflows_CheckedChanged);
            // 
            // rdoConfigurableWorkflows
            // 
            this.rdoConfigurableWorkflows.AutoSize = true;
            this.rdoConfigurableWorkflows.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoConfigurableWorkflows.Location = new System.Drawing.Point(180, 30);
            this.rdoConfigurableWorkflows.Name = "rdoConfigurableWorkflows";
            this.rdoConfigurableWorkflows.Size = new System.Drawing.Size(137, 17);
            this.rdoConfigurableWorkflows.TabIndex = 1;
            this.rdoConfigurableWorkflows.Text = "Configurable Workflows";
            this.rdoConfigurableWorkflows.UseVisualStyleBackColor = true;
            this.rdoConfigurableWorkflows.CheckedChanged += new System.EventHandler(this.rdoConfigurableWorkflows_CheckedChanged);
            // 
            // btnCancelWorkflow
            // 
            this.btnCancelWorkflow.BackColor = System.Drawing.Color.LightGray;
            this.btnCancelWorkflow.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelWorkflow.Location = new System.Drawing.Point(395, 372);
            this.btnCancelWorkflow.Name = "btnCancelWorkflow";
            this.btnCancelWorkflow.Size = new System.Drawing.Size(75, 31);
            this.btnCancelWorkflow.TabIndex = 19;
            this.btnCancelWorkflow.Text = "Cancel";
            this.btnCancelWorkflow.UseVisualStyleBackColor = false;
            this.btnCancelWorkflow.Click += new System.EventHandler(this.btnCancelWorkflow_Click);
            // 
            // btnFinalize
            // 
            this.btnFinalize.BackColor = System.Drawing.Color.LightGray;
            this.btnFinalize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFinalize.Location = new System.Drawing.Point(293, 372);
            this.btnFinalize.Name = "btnFinalize";
            this.btnFinalize.Size = new System.Drawing.Size(75, 31);
            this.btnFinalize.TabIndex = 18;
            this.btnFinalize.Text = "Finalize";
            this.btnFinalize.UseVisualStyleBackColor = false;
            this.btnFinalize.Click += new System.EventHandler(this.btnFinalize_Click);
            // 
            // lblNumberofWorkflows
            // 
            this.lblNumberofWorkflows.BackColor = System.Drawing.Color.Transparent;
            this.lblNumberofWorkflows.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumberofWorkflows.Location = new System.Drawing.Point(52, 77);
            this.lblNumberofWorkflows.Name = "lblNumberofWorkflows";
            this.lblNumberofWorkflows.Size = new System.Drawing.Size(138, 32);
            this.lblNumberofWorkflows.TabIndex = 2;
            this.lblNumberofWorkflows.Text = "Number of Workflows:";
            this.lblNumberofWorkflows.Click += new System.EventHandler(this.lblNumberofWorkflows_Click);
            // 
            // grpExceptionFlow
            // 
            this.grpExceptionFlow.BackColor = System.Drawing.Color.White;
            this.grpExceptionFlow.Controls.Add(this.pictureBox83);
            this.grpExceptionFlow.Controls.Add(this.pictureBox82);
            this.grpExceptionFlow.Controls.Add(this.lblFromStep);
            this.grpExceptionFlow.Controls.Add(this.cmbToStep);
            this.grpExceptionFlow.Controls.Add(this.lblToStep);
            this.grpExceptionFlow.Controls.Add(this.cmbFromStep);
            this.grpExceptionFlow.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpExceptionFlow.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grpExceptionFlow.Location = new System.Drawing.Point(18, 257);
            this.grpExceptionFlow.Name = "grpExceptionFlow";
            this.grpExceptionFlow.Size = new System.Drawing.Size(470, 98);
            this.grpExceptionFlow.TabIndex = 17;
            this.grpExceptionFlow.TabStop = false;
            this.grpExceptionFlow.Text = "Exception Flow";
            // 
            // lblFromStep
            // 
            this.lblFromStep.AutoSize = true;
            this.lblFromStep.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromStep.Location = new System.Drawing.Point(44, 29);
            this.lblFromStep.Name = "lblFromStep";
            this.lblFromStep.Size = new System.Drawing.Size(55, 13);
            this.lblFromStep.TabIndex = 13;
            this.lblFromStep.Text = "From Step";
            // 
            // cmbToStep
            // 
            this.cmbToStep.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbToStep.FormattingEnabled = true;
            this.cmbToStep.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cmbToStep.Location = new System.Drawing.Point(275, 48);
            this.cmbToStep.Name = "cmbToStep";
            this.cmbToStep.Size = new System.Drawing.Size(159, 21);
            this.cmbToStep.TabIndex = 16;
            // 
            // lblToStep
            // 
            this.lblToStep.AutoSize = true;
            this.lblToStep.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToStep.Location = new System.Drawing.Point(267, 26);
            this.lblToStep.Name = "lblToStep";
            this.lblToStep.Size = new System.Drawing.Size(45, 13);
            this.lblToStep.TabIndex = 14;
            this.lblToStep.Text = "To Step";
            // 
            // cmbFromStep
            // 
            this.cmbFromStep.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbFromStep.FormattingEnabled = true;
            this.cmbFromStep.Location = new System.Drawing.Point(47, 48);
            this.cmbFromStep.Name = "cmbFromStep";
            this.cmbFromStep.Size = new System.Drawing.Size(159, 21);
            this.cmbFromStep.TabIndex = 12;
            this.cmbFromStep.SelectedIndexChanged += new System.EventHandler(this.cmbFromStep_SelectedIndexChanged);
            // 
            // txtEscalationTime
            // 
            this.txtEscalationTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEscalationTime.Location = new System.Drawing.Point(205, 212);
            this.txtEscalationTime.Name = "txtEscalationTime";
            this.txtEscalationTime.Size = new System.Drawing.Size(100, 20);
            this.txtEscalationTime.TabIndex = 10;
            // 
            // txtNumberofWorflows
            // 
            this.txtNumberofWorflows.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNumberofWorflows.Location = new System.Drawing.Point(205, 74);
            this.txtNumberofWorflows.Name = "txtNumberofWorflows";
            this.txtNumberofWorflows.Size = new System.Drawing.Size(100, 20);
            this.txtNumberofWorflows.TabIndex = 3;
            // 
            // lbEscalationTime
            // 
            this.lbEscalationTime.AutoSize = true;
            this.lbEscalationTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbEscalationTime.Location = new System.Drawing.Point(52, 212);
            this.lbEscalationTime.Name = "lbEscalationTime";
            this.lbEscalationTime.Size = new System.Drawing.Size(127, 13);
            this.lbEscalationTime.TabIndex = 9;
            this.lbEscalationTime.Text = "Escalation Time in (Mins):";
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.LightGray;
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Location = new System.Drawing.Point(330, 72);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(65, 23);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lblWorkflowName
            // 
            this.lblWorkflowName.AutoSize = true;
            this.lblWorkflowName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWorkflowName.Location = new System.Drawing.Point(52, 121);
            this.lblWorkflowName.Name = "lblWorkflowName";
            this.lblWorkflowName.Size = new System.Drawing.Size(89, 13);
            this.lblWorkflowName.TabIndex = 5;
            this.lblWorkflowName.Text = "Workflow Name: ";
            // 
            // txtNoOfSteps
            // 
            this.txtNoOfSteps.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNoOfSteps.Location = new System.Drawing.Point(205, 166);
            this.txtNoOfSteps.Name = "txtNoOfSteps";
            this.txtNoOfSteps.Size = new System.Drawing.Size(100, 20);
            this.txtNoOfSteps.TabIndex = 8;
            this.txtNoOfSteps.TextChanged += new System.EventHandler(this.txtNoOfSteps_TextChanged);
            // 
            // txtWorkflowName
            // 
            this.txtWorkflowName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWorkflowName.Location = new System.Drawing.Point(205, 118);
            this.txtWorkflowName.Name = "txtWorkflowName";
            this.txtWorkflowName.Size = new System.Drawing.Size(100, 20);
            this.txtWorkflowName.TabIndex = 6;
            // 
            // lblNoOfSteps
            // 
            this.lblNoOfSteps.AutoSize = true;
            this.lblNoOfSteps.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoOfSteps.Location = new System.Drawing.Point(52, 166);
            this.lblNoOfSteps.Name = "lblNoOfSteps";
            this.lblNoOfSteps.Size = new System.Drawing.Size(66, 13);
            this.lblNoOfSteps.TabIndex = 7;
            this.lblNoOfSteps.Text = "No of Steps:";
            // 
            // CbxWorkflow
            // 
            this.CbxWorkflow.AutoSize = true;
            this.CbxWorkflow.Enabled = false;
            this.CbxWorkflow.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CbxWorkflow.Location = new System.Drawing.Point(49, 24);
            this.CbxWorkflow.Name = "CbxWorkflow";
            this.CbxWorkflow.Size = new System.Drawing.Size(71, 17);
            this.CbxWorkflow.TabIndex = 61;
            this.CbxWorkflow.Text = "Workflow";
            this.CbxWorkflow.UseVisualStyleBackColor = true;
            this.CbxWorkflow.CheckedChanged += new System.EventHandler(this.CbxWorkflow_CheckedChanged);
            // 
            // pictureBox73
            // 
            this.pictureBox73.ErrorImage = null;
            this.pictureBox73.Image = global::KPIT_K_Foundation.Properties.Resources.tooltip;
            this.pictureBox73.InitialImage = null;
            this.pictureBox73.Location = new System.Drawing.Point(18, 25);
            this.pictureBox73.Name = "pictureBox73";
            this.pictureBox73.Size = new System.Drawing.Size(16, 16);
            this.pictureBox73.TabIndex = 60;
            this.pictureBox73.TabStop = false;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.progressBar.Location = new System.Drawing.Point(1, 937);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(598, 1032);
            this.progressBar.TabIndex = 37;
            // 
            // AutoCodeGenerator
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(656, 661);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.LblProgress);
            this.Controls.Add(this.BtnClose);
            this.Controls.Add(this.BtnGenerateCode);
            this.Controls.Add(this.TabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "AutoCodeGenerator";
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Auto Code Generator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AutoCodeGenerator_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AutoCodeGenerator_FormClosed);
            this.Load += new System.EventHandler(this.AutoCodeGenerator_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            this.TabUISettings.ResumeLayout(false);
            this.GbxViewNames.ResumeLayout(false);
            this.GbxViewNames.PerformLayout();
            this.GbxThemes.ResumeLayout(false);
            this.GbxThemes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox19)).EndInit();
            this.GbxWebFormsToGenerate.ResumeLayout(false);
            this.GbxWebFormsToGenerate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox84)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox54)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox49)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox34)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox51)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox50)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox39)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox21)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox17)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox24)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox27)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox20)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox29)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox16)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox26)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox22)).EndInit();
            this.TabAppSettings.ResumeLayout(false);
            this.TabAppSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox66)).EndInit();
            this.GbxOverwriteWebApiFiles.ResumeLayout(false);
            this.GbxOverwriteWebApiFiles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox64)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox63)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox62)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox33)).EndInit();
            this.GbxOverwriteBusinessDataLayerFiles.ResumeLayout(false);
            this.GbxOverwriteBusinessDataLayerFiles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox40)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox36)).EndInit();
            this.GbxOverwriteFiles.ResumeLayout(false);
            this.GbxOverwriteFiles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox53)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox35)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox23)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox18)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox52)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox31)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox45)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox43)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox32)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox44)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox28)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox38)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox42)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicbUserName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicbPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicbShowPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicbDatabaseName)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox55)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox15)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox57)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox68)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox56)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox47)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox61)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox60)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox41)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox58)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox59)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox67)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox48)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox37)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox30)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox25)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox65)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox70)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox69)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox71)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBoxShowPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox78)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox79)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox80)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox81)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox82)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox83)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox46)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.TabGeneratedCode.ResumeLayout(false);
            this.GbxApplication.ResumeLayout(false);
            this.GbxApplication.PerformLayout();
            this.GbxWebApi.ResumeLayout(false);
            this.GbxWebApi.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.GbxGeneratedSqlScript.ResumeLayout(false);
            this.GbxGeneratedSqlScript.PerformLayout();
            this.TabControl1.ResumeLayout(false);
            this.TabSelectedTables.ResumeLayout(false);
            this.TabSelectedViews.ResumeLayout(false);
            this.TabDatabase.ResumeLayout(false);
            this.GbxGenerateSql.ResumeLayout(false);
            this.GbxStoredProcedure.ResumeLayout(false);
            this.GbxStoredProcedure.PerformLayout();
            this.GboxStoredProcOrLinq.ResumeLayout(false);
            this.GboxStoredProcOrLinq.PerformLayout();
            this.TabCompSettings.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabComponents.ResumeLayout(false);
            this.GbxConfiguration.ResumeLayout(false);
            this.GbxConfiguration.PerformLayout();
            this.GbxSMTPSettings.ResumeLayout(false);
            this.GbxSMTPSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox77)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox76)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox75)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox74)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox72)).EndInit();
            this.GbxAuditLogging.ResumeLayout(false);
            this.GbxAuditLogging.PerformLayout();
            this.GbxLogging.ResumeLayout(false);
            this.GbxLogging.PerformLayout();
            this.GbxLoggingType.ResumeLayout(false);
            this.GbxLoggingType.PerformLayout();
            this.GbxCaching.ResumeLayout(false);
            this.GbxCaching.PerformLayout();
            this.GbxSecurity.ResumeLayout(false);
            this.GbxSecurity.PerformLayout();
            this.tabWorkflow.ResumeLayout(false);
            this.tabWorkflow.PerformLayout();
            this.gbxWorkflowSettings.ResumeLayout(false);
            this.gbxWorkflowSettings.PerformLayout();
            this.grpExceptionFlow.ResumeLayout(false);
            this.grpExceptionFlow.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox73)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void lblNumberofWorkflows_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TabCompSettings_Click(object sender, EventArgs e)
        {

        }

        private void CbxEmailNotification_CheckedChanged(object sender, EventArgs e)
        {
            if (this.CbxEmailNotification.Checked)
            {
                this.CbxWorkflow.Enabled = true;
                this.GbxSMTPSettings.Enabled = true;
            }
            else
            {
                this.CbxWorkflow.Checked = false;
                this.CbxWorkflow.Enabled = false;
                this.GbxSMTPSettings.Enabled = false;
            }
        }

        private void CbxShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (this.CbxShowPassword.Checked)
                this.TxtPasswordForSmtp.PasswordChar = char.MinValue;
            else
                this.TxtPasswordForSmtp.PasswordChar = '*';
        }

        private void rdoDefaultWorkflows_CheckedChanged(object sender, EventArgs e)
        {
            grpExceptionFlow.Enabled = false;
        }

        private void rdoConfigurableWorkflows_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoConfigurableWorkflows.Checked)
                grpExceptionFlow.Enabled = true;
        }
        private void btnFinalize_Click(object sender, EventArgs e)
        {
            if (rdoDefaultWorkflows.Checked || rdoConfigurableWorkflows.Checked)
                gbxWorkflowSettings.Enabled = false;
            else
                MessageBox.Show("Please select workflow type.");
        }

        private void btnCancelWorkflow_Click(object sender, EventArgs e)
        {
            this.txtWorkflowName.Text = "";
            this.txtNumberofWorflows.Text = "";
            this.txtNoOfSteps.Text = "";
            this.txtEscalationTime.Text = "";
        }

        private void CbxWorkflow_CheckedChanged(object sender, EventArgs e)
        {
            if (this.CbxWorkflow.Checked)
            {
                this.gbxWorkflowSettings.Enabled = true;
            }
            else
            {
                this.gbxWorkflowSettings.Enabled = false;
            }
        }

        private void TabControl1_SelectIndexChanged(object sender, EventArgs e)
        {
            if (TabControl1.SelectedTab == TabControl1.TabPages["TabSelectedTables"])
                this.Height = 700;
            else if (TabControl1.SelectedTab == TabControl1.TabPages["TabSelectedViews"])
                this.Height = 700;
            else if (TabControl1.SelectedTab == TabControl1.TabPages["TabDatabase"])
                this.Height = 700;
            else if (TabControl1.SelectedTab == TabControl1.TabPages["TabGeneratedCode"])
                this.Height = 700;
            else if (TabControl1.SelectedTab == TabControl1.TabPages["TabUISettings"])
                this.Height = 700;
            else if (TabControl1.SelectedTab == TabControl1.TabPages["TabAppSettings"])
                this.Height = 700;
            else if (TabControl1.SelectedTab == TabControl1.TabPages["TabCompSettings"])
            {
                this.Height = 700;
            }
        }

        private void CbxWorkflowAssignSteps_CheckedChanged(object sender, EventArgs e)
        {
            this._isCheckedViewWorkflowSteps = this.CbxWorkflowAssignSteps.Checked;
            if (this.CbxWorkflowAssignSteps.Checked)
                this.TxtWorkflowAssignSteps.Enabled = true;
            else
                this.TxtWorkflowAssignSteps.Enabled = false;
        }

        private void txtNoOfSteps_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNoOfSteps.Text))
            {
                lstOfSteps = new List<int>();
                var getNumberOfSteps = Convert.ToInt32(txtNoOfSteps.Text);
                for (int i = 1; i <= getNumberOfSteps; --getNumberOfSteps)
                {
                    lstOfSteps.Add(getNumberOfSteps);
                }

                lstOfSteps.Reverse();
                BindingSource bindingSource = new BindingSource();
                bindingSource.DataSource = lstOfSteps;
                this.cmbFromStep.DataSource = bindingSource;

                BindingSource bindingSource1 = new BindingSource();
                bindingSource1.DataSource = lstOfSteps;
                this.cmbToStep.DataSource = bindingSource1;
            }
            else
            {
                this.cmbFromStep.DataSource = null;
                this.cmbToStep.DataSource = null;
            }
        }

        private void cmbFromStep_SelectedIndexChanged(object sender, EventArgs e)
        {
            var fromStep = Convert.ToInt32(cmbFromStep.Text.Trim());
            var toSteps = lstOfSteps.Where(x => x < fromStep).ToList();

            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = toSteps;

            this.cmbToStep.DataSource = bindingSource;
        }
    }
}
