
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class AddRecordScript
  {
    private string _path;
    private bool _isOrganizeGeneratedWebForms;
    private bool _isUseFriendlyUrls;
    private bool _isUnboundWebForm;

    private AddRecordScript()
    {
    }

    internal AddRecordScript(string path, bool isOrganizeGeneratedWebForms, bool isUseFriendlyUrls, bool isUnboundWebForm = false)
    {
      this._path = path;
      this._isOrganizeGeneratedWebForms = isOrganizeGeneratedWebForms;
      this._isUseFriendlyUrls = isUseFriendlyUrls;
      this._isUnboundWebForm = isUnboundWebForm;
      this.Generate();
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._path))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("function InitializeValidation() {");
        stringBuilder.AppendLine("    var validator = $(\"#MasterPageForm1\").bind(\"invalid-form.validate\", function () { }).validate({");
        stringBuilder.AppendLine("        errorElement: \"em\",");
        stringBuilder.AppendLine("        errorPlacement: function (error, element) {");
        stringBuilder.AppendLine("            error.appendTo(element.parent(\"td\").next(\"td\"));");
        stringBuilder.AppendLine("        },");
        stringBuilder.AppendLine("        success: function (label) {");
        stringBuilder.AppendLine("            label.text(\"ok!\").addClass(\"success\");");
        stringBuilder.AppendLine("        }");
        stringBuilder.AppendLine("    });");
        stringBuilder.AppendLine("}");
        stringBuilder.AppendLine("");
        if (this._isUseFriendlyUrls && !this._isUnboundWebForm)
        {
          stringBuilder.AppendLine("function InitializeDatePicker(id, imageUrl) {");
          stringBuilder.AppendLine("    if (imageUrl != null)");
          stringBuilder.AppendLine("    {");
          stringBuilder.AppendLine("        $(id).datepicker({");
          stringBuilder.AppendLine("            changeMonth: false,");
          stringBuilder.AppendLine("            changeYear: false,");
          stringBuilder.AppendLine("            showOn: \"button\",");
          stringBuilder.AppendLine("            buttonImage: imageUrl,");
          stringBuilder.AppendLine("            buttonImageOnly: true,");
          stringBuilder.AppendLine("            onSelect: function (date) {");
          stringBuilder.AppendLine("                this.focus()");
          stringBuilder.AppendLine("            }");
          stringBuilder.AppendLine("        });");
          stringBuilder.AppendLine("    }");
          stringBuilder.AppendLine("    else");
          stringBuilder.AppendLine("    {");
          stringBuilder.AppendLine("        $(id).datepicker({");
          stringBuilder.AppendLine("            changeMonth: false,");
          stringBuilder.AppendLine("            changeYear: false,");
          stringBuilder.AppendLine("            showOn: \"button\",");
          if (this._isUseFriendlyUrls)
          {
            if (this._isUnboundWebForm)
            {
              if (this._isOrganizeGeneratedWebForms)
                stringBuilder.AppendLine("            buttonImage: \"../Images/Calendar.png\",");
              else
                stringBuilder.AppendLine("            buttonImage: \"Images/Calendar.png\",");
            }
            else if (this._isOrganizeGeneratedWebForms)
              stringBuilder.AppendLine("            buttonImage: \"../../Images/Calendar.png\",");
            else
              stringBuilder.AppendLine("            buttonImage: \"../Images/Calendar.png\",");
          }
          else if (this._isUnboundWebForm)
          {
            if (this._isOrganizeGeneratedWebForms)
              stringBuilder.AppendLine("            buttonImage: \"../Images/Calendar.png\",");
            else
              stringBuilder.AppendLine("            buttonImage: \"Images/Calendar.png\",");
          }
          else if (this._isOrganizeGeneratedWebForms)
            stringBuilder.AppendLine("            buttonImage: \"../Images/Calendar.png\",");
          else
            stringBuilder.AppendLine("            buttonImage: \"Images/Calendar.png\",");
          stringBuilder.AppendLine("            buttonImageOnly: true,");
          stringBuilder.AppendLine("            onSelect: function (date) {");
          stringBuilder.AppendLine("                this.focus()");
          stringBuilder.AppendLine("            }");
          stringBuilder.AppendLine("        });");
          stringBuilder.AppendLine("    }");
          stringBuilder.AppendLine("}");
        }
        else
        {
          stringBuilder.AppendLine("function InitializeDatePicker(id) {");
          stringBuilder.AppendLine("    $(id).datepicker({");
          stringBuilder.AppendLine("        changeMonth: false,");
          stringBuilder.AppendLine("        changeYear: false,");
          stringBuilder.AppendLine("        showOn: \"button\",");
          if (this._isUseFriendlyUrls)
          {
            if (this._isUnboundWebForm)
            {
              if (this._isOrganizeGeneratedWebForms)
                stringBuilder.AppendLine("        buttonImage: \"../Images/Calendar.png\",");
              else
                stringBuilder.AppendLine("        buttonImage: \"Images/Calendar.png\",");
            }
            else if (this._isOrganizeGeneratedWebForms)
              stringBuilder.AppendLine("        buttonImage: \"../../Images/Calendar.png\",");
            else
              stringBuilder.AppendLine("        buttonImage: \"../Images/Calendar.png\",");
          }
          else if (this._isUnboundWebForm)
          {
            if (this._isOrganizeGeneratedWebForms)
              stringBuilder.AppendLine("        buttonImage: \"../Images/Calendar.png\",");
            else
              stringBuilder.AppendLine("        buttonImage: \"Images/Calendar.png\",");
          }
          else if (this._isOrganizeGeneratedWebForms)
            stringBuilder.AppendLine("        buttonImage: \"../Images/Calendar.png\",");
          else
            stringBuilder.AppendLine("        buttonImage: \"Images/Calendar.png\",");
          stringBuilder.AppendLine("        buttonImageOnly: true,");
          stringBuilder.AppendLine("        onSelect: function (date) {");
          stringBuilder.AppendLine("            this.focus()");
          stringBuilder.AppendLine("        }");
          stringBuilder.AppendLine("    });");
          stringBuilder.AppendLine("}");
        }
        if (!this._isUnboundWebForm)
        {
          stringBuilder.AppendLine("");
          stringBuilder.AppendLine("function ShowError(errorMessage, dialogTitle) {");
          stringBuilder.AppendLine("    $(document).ready(function () {");
          stringBuilder.AppendLine("        $(\"#errorDialog\").text(errorMessage);");
          stringBuilder.AppendLine("        $(\"#errorDialog\").dialog({");
          stringBuilder.AppendLine("            title: dialogTitle,");
          stringBuilder.AppendLine("            modal: true,");
          stringBuilder.AppendLine("            buttons: {");
          stringBuilder.AppendLine("                Ok: function () {");
          stringBuilder.AppendLine("                    $(this).dialog(\"close\");");
          stringBuilder.AppendLine("                }");
          stringBuilder.AppendLine("            }");
          stringBuilder.AppendLine("        });");
          stringBuilder.AppendLine("    });");
          stringBuilder.AppendLine("}");
        }
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
