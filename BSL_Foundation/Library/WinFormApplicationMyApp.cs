
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class WinFormApplicationMyApp
  {
    private string _fullFileNamePath;

    private WinFormApplicationMyApp()
    {
    }

    internal WinFormApplicationMyApp(string fullFileNamePath)
    {
      this._fullFileNamePath = fullFileNamePath;
      this.Generate();
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        stringBuilder.AppendLine("<MyApplicationData xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
        stringBuilder.AppendLine("  <MySubMain>true</MySubMain>");
        stringBuilder.AppendLine("  <MainForm>DefaultWinForm</MainForm>");
        stringBuilder.AppendLine("  <SingleInstance>false</SingleInstance>");
        stringBuilder.AppendLine("  <ShutdownMode>0</ShutdownMode>");
        stringBuilder.AppendLine("  <EnableVisualStyles>true</EnableVisualStyles>");
        stringBuilder.AppendLine("  <AuthenticationMode>0</AuthenticationMode>");
        stringBuilder.AppendLine("  <ApplicationType>0</ApplicationType>");
        stringBuilder.AppendLine("  <SaveMySettingsOnExit>true</SaveMySettingsOnExit>");
        stringBuilder.AppendLine("</MyApplicationData>");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
