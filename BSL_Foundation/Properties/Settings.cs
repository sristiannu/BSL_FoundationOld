// Decompiled with JetBrains decompiler
// Type: KPIT_K_Foundation.Properties.Settings
// Assembly: KPIT_K_Foundation, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3E24485D-8C50-491B-A39C-F68D812B0B56
// Assembly location: E:\Misc\Express\Junnark AspCoreGen 2.0 Razor\Foundation.exe

using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace KPIT_K_Foundation.Properties
{
    [CompilerGenerated]
    [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.3.0.0")]
    internal sealed class Settings : ApplicationSettingsBase
    {
        private static Settings defaultInstance = (Settings)SettingsBase.Synchronized((SettingsBase)new Settings());

        public static Settings Default
        {
            get
            {
                return Settings.defaultInstance;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("localhost")]
        public string Server
        {
            get
            {
                return (string)this[nameof(Server)];
            }
            set
            {
                this[nameof(Server)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        public string DatabaseName
        {
            get
            {
                return (string)this[nameof(DatabaseName)];
            }
            set
            {
                this[nameof(DatabaseName)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        public string Username
        {
            get
            {
                return (string)this[nameof(Username)];
            }
            set
            {
                this[nameof(Username)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        public string Namespace
        {
            get
            {
                return (string)this[nameof(Namespace)];
            }
            set
            {
                this[nameof(Namespace)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        public string Directory
        {
            get
            {
                return (string)this[nameof(Directory)];
            }
            set
            {
                this[nameof(Directory)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("0")]
        public int Language
        {
            get
            {
                return (int)this[nameof(Language)];
            }
            set
            {
                this[nameof(Language)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        public string Password
        {
            get
            {
                return (string)this[nameof(Password)];
            }
            set
            {
                this[nameof(Password)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowPassword
        {
            get
            {
                return (bool)this[nameof(ShowPassword)];
            }
            set
            {
                this[nameof(ShowPassword)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool GenerateCodeForAllTables
        {
            get
            {
                return (bool)this[nameof(GenerateCodeForAllTables)];
            }
            set
            {
                this[nameof(GenerateCodeForAllTables)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        public string SpPrefix
        {
            get
            {
                return (string)this[nameof(SpPrefix)];
            }
            set
            {
                this[nameof(SpPrefix)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        public string SpSuffix
        {
            get
            {
                return (string)this[nameof(SpSuffix)];
            }
            set
            {
                this[nameof(SpSuffix)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("1")]
        public int StoredProcPrefixSuffixIndex
        {
            get
            {
                return (int)this[nameof(StoredProcPrefixSuffixIndex)];
            }
            set
            {
                this[nameof(StoredProcPrefixSuffixIndex)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("\\Enum\\")]
        public string EnumDirectory
        {
            get
            {
                return (string)this[nameof(EnumDirectory)];
            }
            set
            {
                this[nameof(EnumDirectory)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("\\Test\\")]
        public string TestDirectory
        {
            get
            {
                return (string)this[nameof(TestDirectory)];
            }
            set
            {
                this[nameof(TestDirectory)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool IsLoadTablesEnabled
        {
            get
            {
                return (bool)this[nameof(IsLoadTablesEnabled)];
            }
            set
            {
                this[nameof(IsLoadTablesEnabled)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool IsClearSelectedTablesEnabled
        {
            get
            {
                return (bool)this[nameof(IsClearSelectedTablesEnabled)];
            }
            set
            {
                this[nameof(IsClearSelectedTablesEnabled)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool IsLoadViewsEnabled
        {
            get
            {
                return (bool)this[nameof(IsLoadViewsEnabled)];
            }
            set
            {
                this[nameof(IsLoadViewsEnabled)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool IsClearSelectedViewsEnabled
        {
            get
            {
                return (bool)this[nameof(IsClearSelectedViewsEnabled)];
            }
            set
            {
                this[nameof(IsClearSelectedViewsEnabled)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("1")]
        public int DatabaseObjectsToGenerateFromIndex
        {
            get
            {
                return (int)this[nameof(DatabaseObjectsToGenerateFromIndex)];
            }
            set
            {
                this[nameof(DatabaseObjectsToGenerateFromIndex)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("13")]
        public int JQueryUITheme
        {
            get
            {
                return (int)this[nameof(JQueryUITheme)];
            }
            set
            {
                this[nameof(JQueryUITheme)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("2")]
        public int SelectedTabIndex
        {
            get
            {
                return (int)this[nameof(SelectedTabIndex)];
            }
            set
            {
                this[nameof(SelectedTabIndex)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsGenerateCodeButtonEnabled
        {
            get
            {
                return (bool)this[nameof(IsGenerateCodeButtonEnabled)];
            }
            set
            {
                this[nameof(IsGenerateCodeButtonEnabled)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("Generate Code for All Tables")]
        public string GenerateCodeButtonText
        {
            get
            {
                return (string)this[nameof(GenerateCodeButtonText)];
            }
            set
            {
                this[nameof(GenerateCodeButtonText)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool IsCancelButtonEnabled
        {
            get
            {
                return (bool)this[nameof(IsCancelButtonEnabled)];
            }
            set
            {
                this[nameof(IsCancelButtonEnabled)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsOverwriteLayoutPage
        {
            get
            {
                return (bool)this[nameof(IsOverwriteLayoutPage)];
            }
            set
            {
                this[nameof(IsOverwriteLayoutPage)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsOverwriteFunctionsFile
        {
            get
            {
                return (bool)this[nameof(IsOverwriteFunctionsFile)];
            }
            set
            {
                this[nameof(IsOverwriteFunctionsFile)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsOverwriteSiteCss
        {
            get
            {
                return (bool)this[nameof(IsOverwriteSiteCss)];
            }
            set
            {
                this[nameof(IsOverwriteSiteCss)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsCheckedViewListCrudRedirect
        {
            get
            {
                return (bool)this[nameof(IsCheckedViewListCrudRedirect)];
            }
            set
            {
                this[nameof(IsCheckedViewListCrudRedirect)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsCheckedViewListCrud
        {
            get
            {
                return (bool)this[nameof(IsCheckedViewListCrud)];
            }
            set
            {
                this[nameof(IsCheckedViewListCrud)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsCheckedViewListReadOnly
        {
            get
            {
                return (bool)this[nameof(IsCheckedViewListReadOnly)];
            }
            set
            {
                this[nameof(IsCheckedViewListReadOnly)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsCheckedViewUpdateRecord
        {
            get
            {
                return (bool)this[nameof(IsCheckedViewUpdateRecord)];
            }
            set
            {
                this[nameof(IsCheckedViewUpdateRecord)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsCheckedViewListTotals
        {
            get
            {
                return (bool)this[nameof(IsCheckedViewListTotals)];
            }
            set
            {
                this[nameof(IsCheckedViewListTotals)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsCheckedViewListGroupedBy
        {
            get
            {
                return (bool)this[nameof(IsCheckedViewListGroupedBy)];
            }
            set
            {
                this[nameof(IsCheckedViewListGroupedBy)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsCheckedViewListTotalsGroupedBy
        {
            get
            {
                return (bool)this[nameof(IsCheckedViewListTotalsGroupedBy)];
            }
            set
            {
                this[nameof(IsCheckedViewListTotalsGroupedBy)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsCheckedViewAddRecord
        {
            get
            {
                return (bool)this[nameof(IsCheckedViewAddRecord)];
            }
            set
            {
                this[nameof(IsCheckedViewAddRecord)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsCheckedViewRecordDetails
        {
            get
            {
                return (bool)this[nameof(IsCheckedViewRecordDetails)];
            }
            set
            {
                this[nameof(IsCheckedViewRecordDetails)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("ListCrudRedirect")]
        public string NameForCheckedViewListCrudRedirect
        {
            get
            {
                return (string)this[nameof(NameForCheckedViewListCrudRedirect)];
            }
            set
            {
                this[nameof(NameForCheckedViewListCrudRedirect)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("ListCrud")]
        public string NameForCheckedViewListCrud
        {
            get
            {
                return (string)this[nameof(NameForCheckedViewListCrud)];
            }
            set
            {
                this[nameof(NameForCheckedViewListCrud)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("ListReadOnly")]
        public string NameForCheckedViewListReadOnly
        {
            get
            {
                return (string)this[nameof(NameForCheckedViewListReadOnly)];
            }
            set
            {
                this[nameof(NameForCheckedViewListReadOnly)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("Update")]
        public string NameForCheckedViewUpdateRecord
        {
            get
            {
                return (string)this[nameof(NameForCheckedViewUpdateRecord)];
            }
            set
            {
                this[nameof(NameForCheckedViewUpdateRecord)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("ListTotals")]
        public string NameForCheckedViewListTotals
        {
            get
            {
                return (string)this[nameof(NameForCheckedViewListTotals)];
            }
            set
            {
                this[nameof(NameForCheckedViewListTotals)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("ListGroupedBy")]
        public string NameForCheckedViewListGroupedBy
        {
            get
            {
                return (string)this[nameof(NameForCheckedViewListGroupedBy)];
            }
            set
            {
                this[nameof(NameForCheckedViewListGroupedBy)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("ListTotalsGroupedBy")]
        public string NameForCheckedViewListTotalsGroupedBy
        {
            get
            {
                return (string)this[nameof(NameForCheckedViewListTotalsGroupedBy)];
            }
            set
            {
                this[nameof(NameForCheckedViewListTotalsGroupedBy)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("Add")]
        public string NameForCheckedViewAddRecord
        {
            get
            {
                return (string)this[nameof(NameForCheckedViewAddRecord)];
            }
            set
            {
                this[nameof(NameForCheckedViewAddRecord)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("Details")]
        public string NameForCheckedViewRecordDetails
        {
            get
            {
                return (string)this[nameof(NameForCheckedViewRecordDetails)];
            }
            set
            {
                this[nameof(NameForCheckedViewRecordDetails)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("AssignWorkflowSteps")]
        public string NameForCheckedWorkflowSteps
        {
            get
            {
                return (string)this[nameof(NameForCheckedWorkflowSteps)];
            }
            set
            {
                this[nameof(NameForCheckedWorkflowSteps)] = (object)value;
            }
        }
        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("Unbound")]
        public string NameForCheckedViewUnbound
        {
            get
            {
                return (string)this[nameof(NameForCheckedViewUnbound)];
            }
            set
            {
                this[nameof(NameForCheckedViewUnbound)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        public string WebsiteName
        {
            get
            {
                return (string)this[nameof(WebsiteName)];
            }
            set
            {
                this[nameof(WebsiteName)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsAutomaticallyOpenTab
        {
            get
            {
                return (bool)this[nameof(IsAutomaticallyOpenTab)];
            }
            set
            {
                this[nameof(IsAutomaticallyOpenTab)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsCheckedViewListSearch
        {
            get
            {
                return (bool)this[nameof(IsCheckedViewListSearch)];
            }
            set
            {
                this[nameof(IsCheckedViewListSearch)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsCheckedViewUnbound
        {
            get
            {
                return (bool)this[nameof(IsCheckedViewUnbound)];
            }
            set
            {
                this[nameof(IsCheckedViewUnbound)] = (object)value;
            }
        }
        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsCheckedViewWorkflowSteps
        {
            get
            {
                return (bool)this[nameof(IsCheckedViewWorkflowSteps)];
            }
            set
            {
                this[nameof(IsCheckedViewWorkflowSteps)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("ListSearch")]
        public string NameForCheckedViewListSearch
        {
            get
            {
                return (string)this[nameof(NameForCheckedViewListSearch)];
            }
            set
            {
                this[nameof(NameForCheckedViewListSearch)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsCloseButtonEnabled
        {
            get
            {
                return (bool)this[nameof(IsCloseButtonEnabled)];
            }
            set
            {
                this[nameof(IsCloseButtonEnabled)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("C:\\Program Files (x86)\\KPIT\\AspCoreGen 2.0 Razor Professional Plus\\AppFiles\\")]
        public string ApplicationDirectory
        {
            get
            {
                return (string)this[nameof(ApplicationDirectory)];
            }
            set
            {
                this[nameof(ApplicationDirectory)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsOverwriteBundleConfigClass
        {
            get
            {
                return (bool)this[nameof(IsOverwriteBundleConfigClass)];
            }
            set
            {
                this[nameof(IsOverwriteBundleConfigClass)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsOverwriteGlobalAsax
        {
            get
            {
                return (bool)this[nameof(IsOverwriteGlobalAsax)];
            }
            set
            {
                this[nameof(IsOverwriteGlobalAsax)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsOverwritePackagesConfig
        {
            get
            {
                return (bool)this[nameof(IsOverwritePackagesConfig)];
            }
            set
            {
                this[nameof(IsOverwritePackagesConfig)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsOverwriteRouteConfigClassFile
        {
            get
            {
                return (bool)this[nameof(IsOverwriteRouteConfigClassFile)];
            }
            set
            {
                this[nameof(IsOverwriteRouteConfigClassFile)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool IsUseWebApi
        {
            get
            {
                return (bool)this[nameof(IsUseWebApi)];
            }
            set
            {
                this[nameof(IsUseWebApi)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("http://localhost:27229/")]
        public string WebApiBaseAddress
        {
            get
            {
                return (string)this[nameof(WebApiBaseAddress)];
            }
            set
            {
                this[nameof(WebApiBaseAddress)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsCheckedViewListInline
        {
            get
            {
                return (bool)this[nameof(IsCheckedViewListInline)];
            }
            set
            {
                this[nameof(IsCheckedViewListInline)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("ListInline")]
        public string NameForCheckedViewListInline
        {
            get
            {
                return (string)this[nameof(NameForCheckedViewListInline)];
            }
            set
            {
                this[nameof(NameForCheckedViewListInline)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsCheckedViewListScrollLoad
        {
            get
            {
                return (bool)this[nameof(IsCheckedViewListScrollLoad)];
            }
            set
            {
                this[nameof(IsCheckedViewListScrollLoad)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("ListScrollLoad")]
        public string NameForCheckedViewListScrollLoad
        {
            get
            {
                return (string)this[nameof(NameForCheckedViewListScrollLoad)];
            }
            set
            {
                this[nameof(NameForCheckedViewListScrollLoad)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsOverwriteViewStartPage
        {
            get
            {
                return (bool)this[nameof(IsOverwriteViewStartPage)];
            }
            set
            {
                this[nameof(IsOverwriteViewStartPage)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsOverwriteStartUpAuthClassFile
        {
            get
            {
                return (bool)this[nameof(IsOverwriteStartUpAuthClassFile)];
            }
            set
            {
                this[nameof(IsOverwriteStartUpAuthClassFile)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsOverwriteIdentityModelsClassFile
        {
            get
            {
                return (bool)this[nameof(IsOverwriteIdentityModelsClassFile)];
            }
            set
            {
                this[nameof(IsOverwriteIdentityModelsClassFile)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsOverwriteFilterConfigClass
        {
            get
            {
                return (bool)this[nameof(IsOverwriteFilterConfigClass)];
            }
            set
            {
                this[nameof(IsOverwriteFilterConfigClass)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsOverwriteWebApiConfigClass
        {
            get
            {
                return (bool)this[nameof(IsOverwriteWebApiConfigClass)];
            }
            set
            {
                this[nameof(IsOverwriteWebApiConfigClass)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsOverwriteReferencesJs
        {
            get
            {
                return (bool)this[nameof(IsOverwriteReferencesJs)];
            }
            set
            {
                this[nameof(IsOverwriteReferencesJs)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsOverwriteErrorPage
        {
            get
            {
                return (bool)this[nameof(IsOverwriteErrorPage)];
            }
            set
            {
                this[nameof(IsOverwriteErrorPage)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsOverwriteStartUpClassFile
        {
            get
            {
                return (bool)this[nameof(IsOverwriteStartUpClassFile)];
            }
            set
            {
                this[nameof(IsOverwriteStartUpClassFile)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsCheckedViewListForeach
        {
            get
            {
                return (bool)this[nameof(IsCheckedViewListForeach)];
            }
            set
            {
                this[nameof(IsCheckedViewListForeach)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("ListForeach")]
        public string NameForCheckedViewListForeach
        {
            get
            {
                return (string)this[nameof(NameForCheckedViewListForeach)];
            }
            set
            {
                this[nameof(NameForCheckedViewListForeach)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsCheckedViewListMasterDetailGrid
        {
            get
            {
                return (bool)this[nameof(IsCheckedViewListMasterDetailGrid)];
            }
            set
            {
                this[nameof(IsCheckedViewListMasterDetailGrid)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsCheckedViewListMasterDetailSubGrid
        {
            get
            {
                return (bool)this[nameof(IsCheckedViewListMasterDetailSubGrid)];
            }
            set
            {
                this[nameof(IsCheckedViewListMasterDetailSubGrid)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("ListMasterDetailGridBy")]
        public string NameForCheckedViewListMasterDetailGrid
        {
            get
            {
                return (string)this[nameof(NameForCheckedViewListMasterDetailGrid)];
            }
            set
            {
                this[nameof(NameForCheckedViewListMasterDetailGrid)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("ListMasterDetailSubGridBy")]
        public string NameForCheckedViewListMasterDetailSubGrid
        {
            get
            {
                return (string)this[nameof(NameForCheckedViewListMasterDetailSubGrid)];
            }
            set
            {
                this[nameof(NameForCheckedViewListMasterDetailSubGrid)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsGenerateCodeExamples
        {
            get
            {
                return (bool)this[nameof(IsGenerateCodeExamples)];
            }
            set
            {
                this[nameof(IsGenerateCodeExamples)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("27229")]
        public string DevServerPort
        {
            get
            {
                return (string)this[nameof(DevServerPort)];
            }
            set
            {
                this[nameof(DevServerPort)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsOverwriteAppSettingsWebApi
        {
            get
            {
                return (bool)this[nameof(IsOverwriteAppSettingsWebApi)];
            }
            set
            {
                this[nameof(IsOverwriteAppSettingsWebApi)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsOverwriteProgramClassWebApi
        {
            get
            {
                return (bool)this[nameof(IsOverwriteProgramClassWebApi)];
            }
            set
            {
                this[nameof(IsOverwriteProgramClassWebApi)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsOverwriteLaunchSettingsJsonWebApi
        {
            get
            {
                return (bool)this[nameof(IsOverwriteLaunchSettingsJsonWebApi)];
            }
            set
            {
                this[nameof(IsOverwriteLaunchSettingsJsonWebApi)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsOverwriteStartUpClassWebApi
        {
            get
            {
                return (bool)this[nameof(IsOverwriteStartUpClassWebApi)];
            }
            set
            {
                this[nameof(IsOverwriteStartUpClassWebApi)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("1")]
        public int GenerateSqlIndex
        {
            get
            {
                return (int)this[nameof(GenerateSqlIndex)];
            }
            set
            {
                this[nameof(GenerateSqlIndex)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsOverwriteBowerJson
        {
            get
            {
                return (bool)this[nameof(IsOverwriteBowerJson)];
            }
            set
            {
                this[nameof(IsOverwriteBowerJson)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsOverwriteLaunchSettingsJson
        {
            get
            {
                return (bool)this[nameof(IsOverwriteLaunchSettingsJson)];
            }
            set
            {
                this[nameof(IsOverwriteLaunchSettingsJson)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsOverwriteValidationScriptsPartialView
        {
            get
            {
                return (bool)this[nameof(IsOverwriteValidationScriptsPartialView)];
            }
            set
            {
                this[nameof(IsOverwriteValidationScriptsPartialView)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsOverwriteViewImportsView
        {
            get
            {
                return (bool)this[nameof(IsOverwriteViewImportsView)];
            }
            set
            {
                this[nameof(IsOverwriteViewImportsView)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsOverwriteAppSettingsJson
        {
            get
            {
                return (bool)this[nameof(IsOverwriteAppSettingsJson)];
            }
            set
            {
                this[nameof(IsOverwriteAppSettingsJson)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsOverwriteBundleConfigJson
        {
            get
            {
                return (bool)this[nameof(IsOverwriteBundleConfigJson)];
            }
            set
            {
                this[nameof(IsOverwriteBundleConfigJson)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsOverwriteProgramClassFile
        {
            get
            {
                return (bool)this[nameof(IsOverwriteProgramClassFile)];
            }
            set
            {
                this[nameof(IsOverwriteProgramClassFile)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsOverwriteAssemblyInfo
        {
            get
            {
                return (bool)this[nameof(IsOverwriteAssemblyInfo)];
            }
            set
            {
                this[nameof(IsOverwriteAssemblyInfo)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool IsOverwritProjectJsonBusLayer
        {
            get
            {
                return (bool)this[nameof(IsOverwritProjectJsonBusLayer)];
            }
            set
            {
                this[nameof(IsOverwritProjectJsonBusLayer)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool IsUseLogging
        {
            get
            {
                return (bool)this[nameof(IsUseLogging)];
            }
            set
            {
                this[nameof(IsUseLogging)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool IsUseFileLogging
        {
            get
            {
                return (bool)this[nameof(IsUseFileLogging)];
            }
            set
            {
                this[nameof(IsUseFileLogging)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool IsUseDatabaseLogging
        {
            get
            {
                return (bool)this[nameof(IsUseDatabaseLogging)];
            }
            set
            {
                this[nameof(IsUseDatabaseLogging)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool IsUseEventLogging
        {
            get
            {
                return (bool)this[nameof(IsUseEventLogging)];
            }
            set
            {
                this[nameof(IsUseEventLogging)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool IsUseWebApplication
        {
            get
            {
                return (bool)this[nameof(IsUseWebApplication)];
            }
            set
            {
                this[nameof(IsUseWebApplication)] = (object)value;
            }
        }
                                      
        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool IsTxtWebAppNameEnabled
        {
            get
            {
                return (bool)this[nameof(IsTxtWebAppNameEnabled)];
            }
            set
            {
                this[nameof(IsTxtWebAppNameEnabled)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool IsTxtDirectoryEnabled
        {
            get
            {
                return (bool)this[nameof(IsTxtDirectoryEnabled)];
            }
            set
            {
                this[nameof(IsTxtDirectoryEnabled)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool IsBtnBrowseCodeDirectoryEnabled
        {
            get
            {
                return (bool)this[nameof(IsBtnBrowseCodeDirectoryEnabled)];
            }
            set
            {
                this[nameof(IsBtnBrowseCodeDirectoryEnabled)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool IsCbxGenerateCodeExamplesEnabled
        {
            get
            {
                return (bool)this[nameof(IsCbxGenerateCodeExamplesEnabled)];
            }
            set
            {
                this[nameof(IsCbxGenerateCodeExamplesEnabled)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool IsTxtDevServerPortEnabled
        {
            get
            {
                return (bool)this[nameof(IsTxtDevServerPortEnabled)];
            }
            set
            {
                this[nameof(IsTxtDevServerPortEnabled)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool IsTxtWebAPINameEnabled
        {
            get
            {
                return (bool)this[nameof(IsTxtWebAPINameEnabled)];
            }
            set
            {
                this[nameof(IsTxtWebAPINameEnabled)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool IsTxtWebAPINameDirectoryEnabled
        {
            get
            {
                return (bool)this[nameof(IsTxtWebAPINameDirectoryEnabled)];
            }
            set
            {
                this[nameof(IsTxtWebAPINameDirectoryEnabled)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool IsBtnBrowseWebAPIDirectoryEnabled
        {
            get
            {
                return (bool)this[nameof(IsBtnBrowseWebAPIDirectoryEnabled)];
            }
            set
            {
                this[nameof(IsBtnBrowseWebAPIDirectoryEnabled)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        public string WebAPIName
        {
            get
            {
                return (string)this[nameof(WebAPIName)];
            }
            set
            {
                this[nameof(WebAPIName)] = (object)value;
            }
        }
        
        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        public string WebAPINameDirectory
        {
            get
            {
                return (string)this[nameof(WebAPINameDirectory)];
            }
            set
            {
                this[nameof(WebAPINameDirectory)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        public string APIName
        {
            get
            {
                return (string)this[nameof(APIName)];
            }
            set
            {
                this[nameof(APIName)] = (object)value;
            }
        }
        
        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        public string APINameDirectory
        {
            get
            {
                return (string)this[nameof(APINameDirectory)];
            }
            set
            {
                this[nameof(APINameDirectory)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool IsUseSecurity
        {
            get
            {
                return (bool)this[nameof(IsUseSecurity)];
            }
            set
            {
                this[nameof(IsUseSecurity)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool IsUseCaching
        {
            get
            {
                return (bool)this[nameof(IsUseCaching)];
            }
            set
            {
                this[nameof(IsUseCaching)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool IsUseAuditLogging
        {
            get
            {
                return (bool)this[nameof(IsUseAuditLogging)];
            }
            set
            {
                this[nameof(IsUseAuditLogging)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool IsEmailNotification
        {
            get
            {
                return (bool)this[nameof(IsEmailNotification)];
            }
            set
            {
                this[nameof(IsEmailNotification)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        public string UserNameForSmtp
        {
            get
            {
                return (string)this[nameof(UserNameForSmtp)];
            }
            set
            {
                this[nameof(UserNameForSmtp)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        public string NumberofWorflows
        {
            get
            {
                return (string)this[nameof(NumberofWorflows)];
            }
            set
            {
                this[nameof(NumberofWorflows)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        public string WorkflowName
        {
            get
            {
                return (string)this[nameof(WorkflowName)];
            }
            set
            {
                this[nameof(WorkflowName)] = (object)value;
            }
        }
        
        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        public string NumberOfSteps
        {
            get
            {
                return (string)this[nameof(NumberOfSteps)];
            }
            set
            {
                this[nameof(NumberOfSteps)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        public string EscalationTime
        {
            get
            {
                return (string)this[nameof(EscalationTime)];
            }
            set
            {
                this[nameof(EscalationTime)] = (object)value;
            }
        }
    }
}
