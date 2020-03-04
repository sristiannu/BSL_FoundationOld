
namespace KPIT_K_Foundation
{
  public class KeywordsCS : IKeywords
  {
    public bool IsThisVariableAKeyword(string variable)
    {
      string[] strArray = new string[80]
      {
        "abstract    ",
        "as          ",
        "base        ",
        "bool        ",
        "break       ",
        "byte        ",
        "case        ",
        "catch       ",
        "char        ",
        "checked     ",
        "class       ",
        "const       ",
        "continue    ",
        "decimal     ",
        "default     ",
        "delegate    ",
        "do\t         ",
        "double\t     ",
        "else\t     ",
        "enum        ",
        "event\t     ",
        "explicit    ",
        "extern      ",
        "false       ",
        "finally     ",
        "fixed       ",
        "float       ",
        "for         ",
        "foreach\t ",
        "from        ",
        "goto\t     ",
        "if\t         ",
        "implicit    ",
        "in\t         ",
        "int\t     ",
        "interface   ",
        "internal    ",
        "is\t         ",
        "Location    ",
        "lock        ",
        "long        ",
        "namespace\t ",
        "new         ",
        "null        ",
        "object      ",
        "operator    ",
        "out         ",
        "override    ",
        "params      ",
        "private     ",
        "protected   ",
        "internal      ",
        "readonly    ",
        "ref         ",
        "Region      ",
        "return      ",
        "sbyte       ",
        "sealed      ",
        "short       ",
        "sizeof      ",
        "stackalloc  ",
        "static\t     ",
        "string      ",
        "struct      ",
        "switch      ",
        "this        ",
        "throw       ",
        "true        ",
        "try         ",
        "typeof      ",
        "uint        ",
        "ulong       ",
        "unchecked   ",
        "unsafe      ",
        "ushort      ",
        "using       ",
        "virtual     ",
        "void        ",
        "volatile    ",
        "while       "
      };
      if (string.IsNullOrEmpty(variable))
        return false;
      foreach (string str in strArray)
      {
        if (str.Trim() == variable.Trim())
          return true;
      }
      return false;
    }
  }
}
