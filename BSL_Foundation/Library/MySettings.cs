
using KPIT_K_Foundation.Properties;
using System.Drawing;

namespace KPIT_K_Foundation
{
  internal sealed class MySettings
  {
    private static ApplicationVersion _appVersion = ApplicationVersion.ProfessionalPlus;
    private const bool _isTesting = false;
    private const bool _isCreateStoredProcInDbase = true;

    private MySettings()
    {
    }

    internal static bool IsTesting
    {
      get
      {
        return false;
      }
    }

    internal static bool IsCreateStoredProcInDbase
    {
      get
      {
        return true;
      }
    }

    internal static ApplicationVersion AppVersion
    {
      get
      {
        return MySettings._appVersion;
      }
    }

    internal static string AppVersionNumber
    {
      get
      {
        return "2.0.0";
      }
    }

    internal static string ProductEditionID
    {
      get
      {
        return MySettings._appVersion == ApplicationVersion.ProfessionalPlus ? "ACG2RAZORPP" : "ACG2RAZORExp";
      }
    }

    internal static ApplicationType AppType
    {
      get
      {
        return ApplicationType.ASPCORE20RAZOR;
      }
    }

    internal static string AppTitle
    {
      get
      {
        return MySettings._appVersion == ApplicationVersion.ProfessionalPlus ? "BSL Foundation V2.0" : "AspCoreGen 2.0 Razor Express";
      }
    }

    internal static int SerialAndActivationLength
    {
      get
      {
        return 50;
      }
    }

    internal static string AppActivationFirstSevenCharacters
    {
      get
      {
        return "ACG2RAZ";
      }
    }

    internal static string AppActivationFiveLastCharacters
    {
      get
      {
        return "AJGA1";
      }
    }

    internal static string SerialNumberFileName
    {
      get
      {
        return "serialnumberACG2Razpp.jsv";
      }
    }

    internal static Icon AppIcon
    {
      get
      {
        return Resources.Logo;
      }
    }
  }
}
