
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class HtmlHelpers
  {
    private string _fullFileNamePath;
    private string _websiteName;

    private HtmlHelpers()
    {
    }

    internal HtmlHelpers(string fullFileNamePath, Language language, string websiteName)
    {
      this._fullFileNamePath = fullFileNamePath;
      this._websiteName = websiteName.Trim();
      if (language == Language.CSharp)
        this.GenerateCS();
      else
        this.GenerateVB();
    }

    private void GenerateCS()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("using System;");
        stringBuilder.AppendLine("using System.Linq;");
        stringBuilder.AppendLine("using System.Linq.Expressions;");
        stringBuilder.AppendLine("using System.Web.Mvc;");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("namespace PPMAdmin.Helpers");
        stringBuilder.AppendLine("{");
        stringBuilder.AppendLine("    internal static class HtmlHelpers");
        stringBuilder.AppendLine("    {");
        stringBuilder.AppendLine("        /// <summary>");
        stringBuilder.AppendLine("        /// Use when assigning a nullable value (bool?) to a checbox.  Use instead of the CheckBoxFor.");
        stringBuilder.AppendLine("        /// </summary>");
        stringBuilder.AppendLine("        /// <returns>Returns false when null, otherwise returns true or false</returns>");
        stringBuilder.AppendLine("        internal static MvcHtmlString CheckBoxForNullable<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, bool?>> expression)");
        stringBuilder.AppendLine("        {");
        stringBuilder.AppendLine("            // get the name of the property");
        stringBuilder.AppendLine("            string[] propertyNameParts = expression.Body.ToString().Split('.');");
        stringBuilder.AppendLine("            string propertyName = propertyNameParts.Last();");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            // get the value of the property");
        stringBuilder.AppendLine("            Func<TModel, bool?> compiled = expression.Compile();");
        stringBuilder.AppendLine("            bool? nullabeBoolValue = compiled(html.ViewData.Model);");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            // build the checkbox");
        stringBuilder.AppendLine("            TagBuilder checkbox = new TagBuilder(\"input\");");
        stringBuilder.AppendLine("            checkbox.MergeAttribute(\"id\", propertyName);");
        stringBuilder.AppendLine("            checkbox.MergeAttribute(\"name\", propertyName);");
        stringBuilder.AppendLine("            checkbox.MergeAttribute(\"type\", \"checkbox\");");
        stringBuilder.AppendLine("            checkbox.MergeAttribute(\"value\", \"true\");");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            if (nullabeBoolValue != null)");
        stringBuilder.AppendLine("            {");
        stringBuilder.AppendLine("                if(nullabeBoolValue.Value)");
        stringBuilder.AppendLine("                    checkbox.MergeAttribute(\"checked\", \"checked\");");
        stringBuilder.AppendLine("            }");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            TagBuilder hidden = new TagBuilder(\"input\");");
        stringBuilder.AppendLine("            hidden.MergeAttribute(\"name\", propertyName);");
        stringBuilder.AppendLine("            hidden.MergeAttribute(\"type\", \"hidden\");");
        stringBuilder.AppendLine("            hidden.MergeAttribute(\"value\", \"false\");");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("            return MvcHtmlString.Create(checkbox.ToString(TagRenderMode.SelfClosing) + hidden.ToString(TagRenderMode.SelfClosing));");
        stringBuilder.AppendLine("        }");
        stringBuilder.AppendLine("    }");
        stringBuilder.AppendLine("}");
        streamWriter.Write(stringBuilder.ToString());
      }
    }

    private void GenerateVB()
    {
    }
  }
}
