
using System.IO;

namespace KPIT_K_Foundation
{
  internal sealed class ProjectFragmentJsonLock
  {
    private string _fullFileNamePath;
    private string _apiName;

    internal ProjectFragmentJsonLock(string fullFileNamePath, string apiName)
    {
      this._apiName = apiName;
      this._fullFileNamePath = fullFileNamePath;
      this.ReplaceText();
    }

    private void ReplaceText()
    {
      File.WriteAllText(this._fullFileNamePath, File.ReadAllText(this._fullFileNamePath).Replace("[AspCoreGen1-API-Name]", this._apiName));
    }
  }
}
