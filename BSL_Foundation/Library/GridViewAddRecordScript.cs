
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class GridViewAddRecordScript
  {
    private string _path;
    private bool _isOrganizeGeneratedWebForms;
    private bool _isUseFriendlyUrls;

    private GridViewAddRecordScript()
    {
    }

    internal GridViewAddRecordScript(string path, bool isOrganizeGeneratedWebForms, bool isUseFriendlyUrls)
    {
      this._path = path;
      this._isOrganizeGeneratedWebForms = isOrganizeGeneratedWebForms;
      this._isUseFriendlyUrls = isUseFriendlyUrls;
      this.Generate();
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._path))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("var validator;");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("function pageLoad(sender, args) {");
        stringBuilder.AppendLine("    if (args.get_isPartialLoad()) {");
        stringBuilder.AppendLine("        InitializeToolTip();");
        stringBuilder.AppendLine("    }");
        stringBuilder.AppendLine("}");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("function SetGridViewHoverOn(gridView) {");
        stringBuilder.AppendLine("    if (gridView != null) {");
        stringBuilder.AppendLine("        gridView.originalBgColor = gridView.style.backgroundColor;");
        stringBuilder.AppendLine("        gridView.style.backgroundColor = '#FFFFCC';");
        stringBuilder.AppendLine("    }");
        stringBuilder.AppendLine("}");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("function SetGridViewHoverOff(gridView) {");
        stringBuilder.AppendLine("    if (gridView != null) {");
        stringBuilder.AppendLine("        gridView.style.backgroundColor = gridView.originalBgColor;");
        stringBuilder.AppendLine("    }");
        stringBuilder.AppendLine("}");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("function deleteItem(uniqueID, itemID) {");
        stringBuilder.AppendLine("    var dialogTitle = 'Permanently Delete Item ' + itemID + '?';");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("    $(\"#deleteConfirmationDialog\").html('<p><span class=\"ui-icon ui-icon-alert\" style=\"float:left; margin:0 7px 20px 0;\"></span>Please click delete to confirm deletion.</p>');");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("    $(\"#deleteConfirmationDialog\").dialog({");
        stringBuilder.AppendLine("        title: dialogTitle,");
        stringBuilder.AppendLine("        modal: true,");
        stringBuilder.AppendLine("        buttons: {");
        stringBuilder.AppendLine("            \"Delete\": function () { __doPostBack(uniqueID, ''); $(this).dialog(\"close\"); },");
        stringBuilder.AppendLine("            \"Cancel\": function () { $(this).dialog(\"close\"); }");
        stringBuilder.AppendLine("        }");
        stringBuilder.AppendLine("    });");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("    $('#deleteConfirmationDialog').dialog('open');");
        stringBuilder.AppendLine("    return false;");
        stringBuilder.AppendLine("}");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("function ShowError(errorMessage, dialogTitle) { ");
        stringBuilder.AppendLine("    $(document).ready(function () {");
        stringBuilder.AppendLine("        $(\"#errorDialog\").text(errorMessage); ");
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
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("function InitializeValidation() {");
        stringBuilder.AppendLine("    validator = $(\"#MasterPageForm1\").bind(\"invalid-form.validate\", function () { }).validate({");
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
        stringBuilder.AppendLine("function InitializeDatePicker(id) {");
        stringBuilder.AppendLine("    $(id).datepicker({");
        stringBuilder.AppendLine("        changeMonth: false,");
        stringBuilder.AppendLine("        changeYear: false,");
        stringBuilder.AppendLine("        showOn: \"button\",");
        if (this._isUseFriendlyUrls)
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
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("function InitializeAddEditRecord() {");
        stringBuilder.AppendLine("    $(\"#showAddNewRecord1\").click(function () {");
        stringBuilder.AppendLine("        addItem();");
        stringBuilder.AppendLine("    });");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("    $(\"#showAddNewRecord2\").click(function () {");
        stringBuilder.AppendLine("        addItem();");
        stringBuilder.AppendLine("    });");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("    $(\"#inpCancel\").click(function () {");
        stringBuilder.AppendLine("        var options = {};");
        stringBuilder.AppendLine("        $(\"#divAddEditRecord\").hide('blind', options, 300);");
        stringBuilder.AppendLine("        return false;");
        stringBuilder.AppendLine("    });");
        stringBuilder.AppendLine("}");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("function InitializeToolTip() {");
        stringBuilder.AppendLine("    $(\".gridViewToolTip\").tooltip({");
        stringBuilder.AppendLine("        track: true,");
        stringBuilder.AppendLine("        delay: 0,");
        stringBuilder.AppendLine("        showURL: false,");
        stringBuilder.AppendLine("        fade: 100,");
        stringBuilder.AppendLine("        bodyHandler: function () {");
        stringBuilder.AppendLine("            return $($(this).next().html()); ");
        stringBuilder.AppendLine("        },");
        stringBuilder.AppendLine("        showURL: false");
        stringBuilder.AppendLine("    });");
        stringBuilder.AppendLine("}");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("function addItem() {");
        stringBuilder.AppendLine("    clearFields();");
        stringBuilder.AppendLine("    showHideItem(addEditTitle, null);");
        stringBuilder.AppendLine("    resetValidationErrors();");
        stringBuilder.AppendLine("    return false;");
        stringBuilder.AppendLine("}");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("function editItem(parameterName, itemID) {");
        stringBuilder.AppendLine("    var paramNameArray = parameterName.split(\"|\");");
        stringBuilder.AppendLine("    var itemIDArray = itemID.split(\"|\");");
        stringBuilder.AppendLine("    var commaDelimParams = '';");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("    for (i = 0; i < paramNameArray.length; i++) {");
        stringBuilder.AppendLine("        if (i == 0)");
        stringBuilder.AppendLine("            commaDelimParams = commaDelimParams + \"'\" + paramNameArray[i] + \"':'\" + itemIDArray[i] + \"'\";");
        stringBuilder.AppendLine("        else");
        stringBuilder.AppendLine("            commaDelimParams = commaDelimParams + \",'\" + paramNameArray[i] + \"':'\" + itemIDArray[i] + \"'\";");
        stringBuilder.AppendLine("    }");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("    callWebMethod(urlAndMethod, commaDelimParams);");
        stringBuilder.AppendLine("    showHideItem(addEditTitle, commaDelimParams);");
        stringBuilder.AppendLine("    resetValidationErrors();");
        stringBuilder.AppendLine("    return false;");
        stringBuilder.AppendLine("}");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("function callWebMethod(urlAndMethod, parameter) {");
        stringBuilder.AppendLine("    $.ajax({");
        stringBuilder.AppendLine("        type: \"POST\",");
        stringBuilder.AppendLine("        url: urlAndMethod,");
        stringBuilder.AppendLine("        data: \"{\" + parameter + \"}\",");
        stringBuilder.AppendLine("        contentType: \"application/json; charset=utf-8\",");
        stringBuilder.AppendLine("        dataType: \"json\",");
        stringBuilder.AppendLine("        success: function (msg) {");
        stringBuilder.AppendLine("            assignRetrievedItems(msg);");
        stringBuilder.AppendLine("        }");
        stringBuilder.AppendLine("    });");
        stringBuilder.AppendLine("}");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("function showHideItem(title, itemID) {");
        stringBuilder.AppendLine("    if (itemID == null) {");
        stringBuilder.AppendLine("        // add");
        stringBuilder.AppendLine("        if ($(\"#trPrimaryKey\") != null)");
        stringBuilder.AppendLine("            $(\"#trPrimaryKey\").hide();");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        $(\"#h3AddEditRecord\").text(\"Add New \" + title);");
        stringBuilder.AppendLine("        $(\"#spanAddRecord\").show();");
        stringBuilder.AppendLine("        $(\"#spanUpdateRecord\").hide();");
        stringBuilder.AppendLine("    }");
        stringBuilder.AppendLine("    else {");
        stringBuilder.AppendLine("        // update");
        stringBuilder.AppendLine("        if ($(\"#trPrimaryKey\") != null)");
        stringBuilder.AppendLine("            $(\"#trPrimaryKey\").show();");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("        $(\"#h3AddEditRecord\").text(\"Edit \" + title);");
        stringBuilder.AppendLine("        $(\"#spanAddRecord\").hide();");
        stringBuilder.AppendLine("        $(\"#spanUpdateRecord\").show();");
        stringBuilder.AppendLine("    }");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("    var options = {};");
        stringBuilder.AppendLine("    $(\"#divAddEditRecord\").show('blind', options, 300);");
        stringBuilder.AppendLine("}");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("function resetValidationErrors() {");
        stringBuilder.AppendLine("    if (validator != null) {");
        stringBuilder.AppendLine("        validator.resetForm();");
        stringBuilder.AppendLine("        validator.submit = {};");
        stringBuilder.AppendLine("    }");
        stringBuilder.AppendLine("}");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("function ConvertNullToString(value) {");
        stringBuilder.AppendLine("    if (value == null)");
        stringBuilder.AppendLine("        return '';");
        stringBuilder.AppendLine("    else");
        stringBuilder.AppendLine("        return value;");
        stringBuilder.AppendLine("}");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("function ConvertNullToFalse(value) {");
        stringBuilder.AppendLine("    if (value == null)");
        stringBuilder.AppendLine("        return false;");
        stringBuilder.AppendLine("    else");
        stringBuilder.AppendLine("        return value;");
        stringBuilder.AppendLine("}");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("function ConvertToDate(value) {");
        stringBuilder.AppendLine("    try {");
        stringBuilder.AppendLine("        var pattern = /Date\\(([^)]+)\\)/;");
        stringBuilder.AppendLine("        var results = pattern.exec(value);");
        stringBuilder.AppendLine("        var dt = new Date(parseFloat(results[1]));");
        stringBuilder.AppendLine("        return (dt.getMonth() + 1) + \"/\" + dt.getDate() + \"/\" + dt.getFullYear();");
        stringBuilder.AppendLine("    }");
        stringBuilder.AppendLine("    catch (e) {");
        stringBuilder.AppendLine("        return '';");
        stringBuilder.AppendLine("    }");
        stringBuilder.AppendLine("}");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("function hideValidators() {");
        stringBuilder.AppendLine("    if (window.Page_Validators) {");
        stringBuilder.AppendLine("        for (var i = 0; i < Page_Validators.length; i++) {");
        stringBuilder.AppendLine("            var pageValidator = Page_Validators[i];");
        stringBuilder.AppendLine("            pageValidator.isvalid = true;");
        stringBuilder.AppendLine("            ValidatorUpdateDisplay(pageValidator);");
        stringBuilder.AppendLine("        }");
        stringBuilder.AppendLine("    }");
        stringBuilder.AppendLine("}");
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
