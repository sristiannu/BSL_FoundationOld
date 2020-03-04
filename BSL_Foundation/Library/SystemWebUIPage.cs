
namespace KPIT_K_Foundation
{
  internal class SystemWebUIPage
  {
    internal static bool IsAProperty(string identifier)
    {
      string[] strArray = new string[79]
      {
        "Application",
        "AppRelativeTemplateSourceDirectory",
        "AppRelativeVirtualPath",
        "AsyncTimeout",
        "AutoPostBackControl",
        "BindingContainer",
        "Buffer",
        "Cache",
        "ClientID",
        "ClientIDMode",
        "ClientQueryString",
        "ClientScript",
        "ClientTarget",
        "CodePage",
        "ContentType",
        "Controls",
        "Culture",
        "DataItemContainer",
        "DataKeysContainer",
        "EnableEventValidation",
        "EnableTheming",
        "EnableViewState",
        "EnableViewStateMac",
        "ErrorPage",
        "Form",
        "Header",
        "ID",
        "IdSeparator",
        "IsAsync",
        "IsCallback",
        "IsCrossPagePostBack",
        "IsPostBack",
        "IsPostBackEventControlRegistered",
        "IsReusable",
        "IsValid",
        "Items",
        "LCID",
        "MaintainScrollPositionOnPostBack",
        "Master",
        "MasterPageFile",
        "MaxPageStateFieldLength",
        "MetaDescription",
        "MetaKeywords",
        "ModelBindingExecutionContext",
        "ModelState",
        "NamingContainer",
        "Page",
        "PageAdapter",
        "Parent",
        "PreviousPage",
        "RenderingCompatibility",
        "Request",
        "Response",
        "ResponseEncoding",
        "RouteData",
        "Server",
        "Session",
        "Site",
        "SkinID",
        "SkipFormActionValidation",
        "SmartNavigation",
        "StyleSheetTheme",
        "TemplateControl",
        "TemplateSourceDirectory",
        "Theme",
        "Title",
        "Trace",
        "TraceEnabled",
        "TraceModeValue",
        "UICulture",
        "UniqueID",
        "UnobtrusiveValidationMode",
        "User",
        "ValidateRequestMode",
        "Validators",
        "ViewStateEncryptionMode",
        "ViewStateMode",
        "ViewStateUserKey",
        "Visible"
      };
      foreach (string str in strArray)
      {
        if (str == identifier.Trim())
          return true;
      }
      return false;
    }
  }
}
