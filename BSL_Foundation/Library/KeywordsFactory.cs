
namespace KPIT_K_Foundation
{
  internal class KeywordsFactory
  {
    internal static IKeywords GetKeywords(Language language)
    {
      if (language == Language.CSharp)
        return (IKeywords) new KeywordsCS();
      return (IKeywords) new KeywordsVB();
    }
  }
}
