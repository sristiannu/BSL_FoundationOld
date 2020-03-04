
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class CodeExample
  {
    private string _classAccessablity = "public";
    private string _methodAccessablity = "private";
    private Table _table;
    private Tables _selectedTables;
    private DataTable _referencedTables;
    private const string _fileExtension = ".cs";
    private const Language _language = Language.CSharp;
    private string _webAppName;
    private string _apiName;
    private string _webAppRootDirectory;
    private string _path;
    private bool _isTesting;
    private DatabaseObjectToGenerateFrom _generateFrom;
    private Dbase _dbase;
    private bool _isUseWebApi;
    private GeneratedSqlType _generatedSqlType;
    private ApplicationVersion _appVersion;

    internal CodeExample()
    {
    }

    internal CodeExample(Table table, Tables selectedTables, DataTable referencedTables, string webAppName, string apiName, string webAppRootDirectory, bool isTesting, string connectionString, DatabaseObjectToGenerateFrom generateFrom, GeneratedSqlType generatedSqlType, ApplicationVersion appVersion, bool isUseWebApi = false)
    {
      this._table = table;
      this._selectedTables = selectedTables;
      this._referencedTables = referencedTables;
      this._webAppName = webAppName;
      this._apiName = apiName;
      this._webAppRootDirectory = webAppRootDirectory + MyConstants.DirectoryExamples;
      this._path = webAppRootDirectory;
      this._isTesting = isTesting;
      this._dbase = new Dbase(connectionString, this._path);
      this._generateFrom = generateFrom;
      this._isUseWebApi = isUseWebApi;
      this._generatedSqlType = generatedSqlType;
      this._appVersion = appVersion;
      this.Generate();
    }

    private void Generate()
    {
      if (this._isTesting)
      {
        this._classAccessablity = "public";
        this._methodAccessablity = "public";
      }
      using (StreamWriter streamWriter = new StreamWriter(this._webAppRootDirectory + this._table.Name + MyConstants.WordExample + ".cs"))
      {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("using System;");
        sb.AppendLine("using System.Collections.Generic;");
        sb.AppendLine("using " + this._webAppName + ";");
        sb.AppendLine("using " + this._apiName + ".BusinessObject;");
        sb.AppendLine("using " + this._apiName + ";");
        sb.AppendLine("// using System.Windows.Forms;    // Note: remove comment when using with windows forms");
        if (this._isUseWebApi)
        {
          sb.AppendLine("");
          sb.AppendLine("// for use with web api");
          sb.AppendLine("using System.Net.Http;");
          sb.AppendLine("using Newtonsoft.Json;");
          sb.AppendLine("using System.Text;");
        }
        sb.AppendLine("");
        sb.AppendLine("/// <summary>");
        sb.AppendLine("/// These are data-centric code examples for the " + this._table.Name + " table.");
        sb.AppendLine("/// You can cut and paste the respective codes into your application");
        sb.AppendLine("/// by changing the sample values assigned from these examples.");
        sb.AppendLine("/// NOTE: This class contains private methods because they're");
        sb.AppendLine("/// not meant to be called by an outside client.  Each method contains");
        sb.AppendLine("/// code for the respective example being shown");
        sb.AppendLine("/// </summary>");
        sb.AppendLine(this._classAccessablity + " sealed class " + this._table.Name + MyConstants.WordExample);
        sb.AppendLine("{");
        sb.AppendLine("    private " + this._table.Name + "Example()");
        sb.AppendLine("    {");
        sb.AppendLine("    }");
        if (this._generateFrom == DatabaseObjectToGenerateFrom.Tables)
        {
          this.WriteSelectAllExample(sb);
          this.WriteSelectAllWithSortExample(sb);
          if (!this._table.IsContainsPrimaryAndForeignKeyColumnsOnly)
            this.WriteSelectByPrimaryKeyExample(sb);
          this.WriteCollectionByExample(sb);
          this.WriteCollectionBySortExample(sb);
          if (!this._table.IsContainsPrimaryAndForeignKeyColumnsOnly && this._table.PrimaryKeyCount == 1)
            this.WriteSelectDropDownListData(sb);
          this.WriteInsertExample(sb);
          if (!this._table.IsContainsPrimaryAndForeignKeyColumnsOnly)
            this.WriteUpdateExample(sb);
          this.WriteDeleteExample(sb);
          this.WriteGetRecordCountExample(sb);
          this.WriteGetRecordByCountExample(sb);
          this.WriteGetRecordCountDynamicWhere(sb);
          this.WriteSelectSkipAndTakeExample(sb);
          this.WriteSelectSkipAndTakeByExample(sb);
          this.WriteSelectSkipAndTakeDynamicWhereExample(sb);
          this.WriteSelectAllDynamicWhereExample(sb);
          this.WriteSelectAllDynamicWhereSortExample(sb);
        }
        else
          this.WriteSelectSkipAndTakeExample(sb);
        sb.AppendLine("}");
        streamWriter.Write(sb.ToString());
      }
    }

    private void WriteSelectByPrimaryKeyExample(StringBuilder sb)
    {
      string primaryKeyParameters = Functions.GetCommaDelimitedPrimaryKeyParameters(this._table, Language.CSharp, false, this._generatedSqlType, true);
      string[] withSampleValues = this.GetPkVariablesWithSampleValues(CrudOperation.Select, this._generatedSqlType);
      sb.AppendLine("");
      sb.AppendLine("    /// <summary>");
      sb.AppendLine("    /// Shows how to Select a record by Primary Key.  It also shows how to retrieve Lazily-loaded related Objects.  Related Objects are assigned for each Foreign Key.");
      sb.AppendLine("    /// </summary>");
      sb.AppendLine("    " + this._methodAccessablity + " void SelectByPrimaryKey()");
      sb.AppendLine("    {");
      foreach (string str in withSampleValues)
        sb.AppendLine("        " + str);
      sb.AppendLine("");
      sb.AppendLine("        // select a record by primary key(s)");
      if (this._table.FirstPrimaryKeySystemType == "Guid")
        sb.AppendLine("        // e.g. Guid.Parse(\"" + Guid.NewGuid().ToString() + "\")");
      sb.AppendLine("");
      if (this._isUseWebApi)
      {
        string delimitedPrimaryKeys = Functions.GetSlashDelimitedPrimaryKeys(this._table, Language.CSharp, true);
        sb.AppendLine("        // uncomment this code if you would rather access the middle tier instead of the web api");
        sb.AppendLine("        //" + this._table.NameFullyQualifiedBusinessObject + " " + this._table.VariableObjName + " = " + this._table.NameFullyQualifiedBusinessObject + ".SelectByPrimaryKey(" + primaryKeyParameters + ");");
        sb.AppendLine("");
        sb.AppendLine("        // ** code using the web api instead of the middle tier **");
        sb.AppendLine("        " + this._table.NameFullyQualifiedBusinessObject + " " + this._table.VariableObjName + " = null;");
        sb.AppendLine("");
        sb.AppendLine("        using (var client = new HttpClient())");
        sb.AppendLine("        {");
        sb.AppendLine("            client.BaseAddress = new Uri(Functions.GetWebApiBaseAddress());");
        sb.AppendLine("            HttpResponseMessage response = client.GetAsync(\"" + this._table.Name + "Api/SelectByPrimaryKey/\" + " + delimitedPrimaryKeys + ").Result;");
        sb.AppendLine("");
        sb.AppendLine("            if (response.IsSuccessStatusCode)");
        sb.AppendLine("            {");
        sb.AppendLine("                var responseBody = response.Content.ReadAsStringAsync().Result;");
        sb.AppendLine("                " + this._table.VariableObjName + " = JsonConvert.DeserializeObject<" + this._table.NameFullyQualifiedBusinessObject + ">(responseBody);");
        sb.AppendLine("            }");
        sb.AppendLine("        }");
        sb.AppendLine("        // ** code using the web api instead of the middle tier **");
      }
      else
        sb.AppendLine("        " + this._table.NameFullyQualifiedBusinessObject + " " + this._table.VariableObjName + " = " + this._table.NameFullyQualifiedBusinessObject + ".SelectByPrimaryKey(" + primaryKeyParameters + ");");
      sb.AppendLine("");
      sb.AppendLine("        if (" + this._table.VariableObjName + " != null)");
      sb.AppendLine("        {");
      sb.AppendLine("            // if record is found, a record is returned");
      foreach (Column column in (List<Column>) this._table.Columns)
      {
        string keywordVariableName = Functions.GetNoneKeywordVariableName(column.NameCamelStyle, Language.CSharp);
        if (this._generatedSqlType == GeneratedSqlType.EFCore && column.SQLDataType == SQLType.uniqueidentifier)
        {
          if (column.IsNullable)
            sb.AppendLine("            Guid? " + keywordVariableName + " = " + this._table.VariableObjName + "." + column.Name + ";");
          else
            sb.AppendLine("            Guid " + keywordVariableName + " = " + this._table.VariableObjName + "." + column.Name + ";");
        }
        else
          sb.AppendLine("            " + column.SystemType + column.NullableChar + " " + keywordVariableName + " = " + this._table.VariableObjName + "." + column.Name + ";");
      }
      this.WriteRelatedTable(sb);
      sb.AppendLine("        }");
      sb.AppendLine("    }");
    }

    private void WriteSelectAllExample(StringBuilder sb)
    {
      sb.AppendLine("");
      sb.AppendLine("    /// <summary>");
      sb.AppendLine("    /// Shows how to Select all records.  It also shows how to sort, bind, and loop through records.");
      sb.AppendLine("    /// </summary>");
      sb.AppendLine("    " + this._methodAccessablity + " void SelectAll()");
      sb.AppendLine("    {");
      sb.AppendLine("        // select all records");
      if (this._isUseWebApi)
      {
        sb.AppendLine("        // uncomment this code if you would rather access the middle tier instead of the web api");
        sb.AppendLine("        // List<" + this._table.Name + "> " + this._table.VariableObjCollectionName + " = " + this._table.NameFullyQualifiedBusinessObject + ".SelectAll();");
        sb.AppendLine("");
        sb.AppendLine("        // ** code using the web api instead of the middle tier **");
        sb.AppendLine("        List<" + this._table.Name + "> " + this._table.VariableObjCollectionName + " = null;");
        sb.AppendLine("");
        sb.AppendLine("        using (var client = new HttpClient())");
        sb.AppendLine("        {");
        sb.AppendLine("            client.BaseAddress = new Uri(Functions.GetWebApiBaseAddress());");
        sb.AppendLine("            HttpResponseMessage response = client.GetAsync(\"" + this._table.Name + "Api/SelectAll\").Result;");
        sb.AppendLine("");
        sb.AppendLine("            if (response.IsSuccessStatusCode)");
        sb.AppendLine("            {");
        sb.AppendLine("                var responseBody = response.Content.ReadAsStringAsync().Result;");
        sb.AppendLine("                " + this._table.VariableObjCollectionName + " = JsonConvert.DeserializeObject<List<" + this._table.Name + ">>(responseBody);");
        sb.AppendLine("            }");
        sb.AppendLine("        }");
        sb.AppendLine("        // ** code using the web api instead of the middle tier **");
      }
      else
        sb.AppendLine("        List<" + this._table.Name + "> " + this._table.VariableObjCollectionName + " = " + this._table.NameFullyQualifiedBusinessObject + ".SelectAll();");
      this.WriteSelectAllSharedExample(sb);
    }

    private void WriteSelectAllWithSortExample(StringBuilder sb)
    {
      sb.AppendLine("");
      sb.AppendLine("    /// <summary>");
      sb.AppendLine("    /// Shows how to Select all records sorted by column name in either ascending or descending order.");
      sb.AppendLine("    /// </summary>");
      sb.AppendLine("    " + this._methodAccessablity + " void SelectAllWithSortExpression()");
      sb.AppendLine("    {");
      sb.AppendLine("        // select all records sorted by " + this._table.FirstPrimaryKeyName + " in ascending order");
      sb.AppendLine("        string sortBy = \"" + this._table.FirstPrimaryKeyName + "\"; // ascending order");
      sb.AppendLine("        //string sortBy = \"" + this._table.FirstPrimaryKeyName + " desc\"; // descending order");
      sb.AppendLine("");
      if (this._isUseWebApi)
      {
        sb.AppendLine("        // uncomment this code if you would rather access the middle tier instead of the web api");
        sb.AppendLine("        // List<" + this._table.Name + "> " + this._table.VariableObjCollectionName + " = " + this._table.NameFullyQualifiedBusinessObject + ".SelectAll(sortBy);");
        sb.AppendLine("");
        sb.AppendLine("        // ** code using the web api instead of the middle tier **");
        sb.AppendLine("        List<" + this._table.Name + "> " + this._table.VariableObjCollectionName + " = null;");
        sb.AppendLine("");
        sb.AppendLine("        using (var client = new HttpClient())");
        sb.AppendLine("        {");
        sb.AppendLine("            client.BaseAddress = new Uri(Functions.GetWebApiBaseAddress());");
        sb.AppendLine("            HttpResponseMessage response = client.GetAsync(\"" + this._table.Name + "Api/SelectAll/\" + sortBy + \"\").Result;");
        sb.AppendLine("");
        sb.AppendLine("            if (response.IsSuccessStatusCode)");
        sb.AppendLine("            {");
        sb.AppendLine("                var responseBody = response.Content.ReadAsStringAsync().Result;");
        sb.AppendLine("                " + this._table.VariableObjCollectionName + " = JsonConvert.DeserializeObject<List<" + this._table.Name + ">>(responseBody);");
        sb.AppendLine("            }");
        sb.AppendLine("        }");
        sb.AppendLine("        // ** code using the web api instead of the middle tier **");
      }
      else
        sb.AppendLine("        List<" + this._table.Name + "> " + this._table.VariableObjCollectionName + " = " + this._table.NameFullyQualifiedBusinessObject + ".SelectAll(sortBy);");
      sb.AppendLine("    }");
    }

    private void WriteCollectionByExample(StringBuilder sb)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (Column foreignKeyColumn in this._table.ForeignKeyColumns)
      {
        string sampleData = this.GetSampleData(foreignKeyColumn, CrudOperation.Select, this._generatedSqlType, false);
        string str = foreignKeyColumn.NameCamelStyle + "Sample";
        if (this._generatedSqlType == GeneratedSqlType.EFCore && foreignKeyColumn.SQLDataType == SQLType.uniqueidentifier)
          str = !foreignKeyColumn.IsNullable ? str + ".ToString()" : str + ".Value.ToString()";
        sb.AppendLine("");
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// Shows how to Select all records by " + foreignKeyColumn.ForeignKeyTableName + ", related to column " + foreignKeyColumn.Name);
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    " + this._methodAccessablity + " void Select" + this._table.Name + "CollectionBy" + foreignKeyColumn.Name + "()");
        sb.AppendLine("    {");
        if (this._generatedSqlType == GeneratedSqlType.EFCore && foreignKeyColumn.SQLDataType == SQLType.uniqueidentifier)
        {
          if (foreignKeyColumn.IsNullable)
            sb.AppendLine("        Guid? " + foreignKeyColumn.NameCamelStyle + "Sample = " + sampleData + ";");
          else
            sb.AppendLine("        Guid " + foreignKeyColumn.NameCamelStyle + "Sample = " + sampleData + ";");
        }
        else
          sb.AppendLine("        " + foreignKeyColumn.SystemType + " " + foreignKeyColumn.NameCamelStyle + "Sample = " + sampleData + ";");
        sb.AppendLine("");
        if (this._isUseWebApi)
        {
          sb.AppendLine("        // uncomment this code if you would rather access the middle tier instead of the web api");
          sb.AppendLine("        // List<" + this._table.Name + "> " + this._table.VariableObjCollectionName + " = " + this._table.Name + ".Select" + this._table.Name + "CollectionBy" + foreignKeyColumn.Name + "(" + str + ");");
          sb.AppendLine("");
          sb.AppendLine("        // ** code using the web api instead of the middle tier **");
          sb.AppendLine("        List<" + this._table.Name + "> " + this._table.VariableObjCollectionName + " = null;");
          sb.AppendLine("");
          sb.AppendLine("        using (var client = new HttpClient())");
          sb.AppendLine("        {");
          sb.AppendLine("            client.BaseAddress = new Uri(Functions.GetWebApiBaseAddress());");
          sb.AppendLine("            HttpResponseMessage response = client.GetAsync(\"" + this._table.Name + "Api/Select" + this._table.Name + "CollectionBy" + foreignKeyColumn.Name + "/\" + " + str + ").Result;");
          sb.AppendLine("");
          sb.AppendLine("            if (response.IsSuccessStatusCode)");
          sb.AppendLine("            {");
          sb.AppendLine("                var responseBody = response.Content.ReadAsStringAsync().Result;");
          sb.AppendLine("                " + this._table.VariableObjCollectionName + " = JsonConvert.DeserializeObject<List<" + this._table.Name + ">>(responseBody);");
          sb.AppendLine("            }");
          sb.AppendLine("        }");
          sb.AppendLine("        // ** code using the web api instead of the middle tier **");
        }
        else
          sb.AppendLine("        List<" + this._table.Name + "> " + this._table.VariableObjCollectionName + " = " + this._table.Name + ".Select" + this._table.Name + "CollectionBy" + foreignKeyColumn.Name + "(" + str + ");");
        this.WriteSelectAllSharedExample(sb);
      }
    }

    private void WriteCollectionBySortExample(StringBuilder sb)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (Column foreignKeyColumn in this._table.ForeignKeyColumns)
      {
        string sampleData = this.GetSampleData(foreignKeyColumn, CrudOperation.Select, this._generatedSqlType, false);
        string str = foreignKeyColumn.NameCamelStyle + "Sample";
        if (this._generatedSqlType == GeneratedSqlType.EFCore && foreignKeyColumn.SQLDataType == SQLType.uniqueidentifier)
          str = !foreignKeyColumn.IsNullable ? str + ".ToString()" : str + ".Value.ToString()";
        sb.AppendLine("");
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// Shows how to Select all records by " + foreignKeyColumn.ForeignKeyTableName + ", related to column " + foreignKeyColumn.Name + " sorted by column name in either ascending or descending order.");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    " + this._methodAccessablity + " void Select" + this._table.Name + "CollectionBy" + foreignKeyColumn.Name + "WithSortExpression()");
        sb.AppendLine("    {");
        if (this._generatedSqlType == GeneratedSqlType.EFCore && foreignKeyColumn.SQLDataType == SQLType.uniqueidentifier)
        {
          if (foreignKeyColumn.IsNullable)
            sb.AppendLine("        Guid? " + foreignKeyColumn.NameCamelStyle + "Sample = " + sampleData + ";");
          else
            sb.AppendLine("        Guid " + foreignKeyColumn.NameCamelStyle + "Sample = " + sampleData + ";");
        }
        else
          sb.AppendLine("        " + foreignKeyColumn.SystemType + " " + foreignKeyColumn.NameCamelStyle + "Sample = " + sampleData + ";");
        sb.AppendLine("        string sortBy = \"" + this._table.FirstPrimaryKeyName + "\"; // ascending order");
        sb.AppendLine("        //string sortBy = \"" + this._table.FirstPrimaryKeyName + " desc\"; // descending order");
        sb.AppendLine("");
        if (this._isUseWebApi)
        {
          sb.AppendLine("        // uncomment this code if you would rather access the middle tier instead of the web api");
          sb.AppendLine("        // List<" + this._table.Name + "> " + this._table.VariableObjCollectionName + " = " + this._table.Name + ".Select" + this._table.Name + "CollectionBy" + foreignKeyColumn.Name + "(" + str + ", sortBy);");
          sb.AppendLine("");
          sb.AppendLine("        // ** code using the web api instead of the middle tier **");
          sb.AppendLine("        List<" + this._table.Name + "> " + this._table.VariableObjCollectionName + " = null;");
          sb.AppendLine("");
          sb.AppendLine("        using (var client = new HttpClient())");
          sb.AppendLine("        {");
          sb.AppendLine("            client.BaseAddress = new Uri(Functions.GetWebApiBaseAddress());");
          sb.AppendLine("            HttpResponseMessage response = client.GetAsync(\"" + this._table.Name + "Api/Select" + this._table.Name + "CollectionBy" + foreignKeyColumn.Name + "/\" + " + str + " + \"/\" + sortBy).Result;");
          sb.AppendLine("");
          sb.AppendLine("            if (response.IsSuccessStatusCode)");
          sb.AppendLine("            {");
          sb.AppendLine("                var responseBody = response.Content.ReadAsStringAsync().Result;");
          sb.AppendLine("                " + this._table.VariableObjCollectionName + " = JsonConvert.DeserializeObject<List<" + this._table.Name + ">>(responseBody);");
          sb.AppendLine("            }");
          sb.AppendLine("        }");
          sb.AppendLine("        // ** code using the web api instead of the middle tier **");
        }
        else
          sb.AppendLine("        List<" + this._table.Name + "> " + this._table.VariableObjCollectionName + " = " + this._table.Name + ".Select" + this._table.Name + "CollectionBy" + foreignKeyColumn.Name + "(" + str + ", sortBy);");
        sb.AppendLine("    }");
      }
    }

    private void WriteSelectAllSharedExample(StringBuilder sb)
    {
      string str = string.Empty;
      foreach (Column column in (List<Column>) this._table.Columns)
      {
        if (!column.IsPrimaryKey && column.SQLDataType != SQLType.xml)
        {
          str = column.Name;
          break;
        }
      }
      if (!string.IsNullOrEmpty(str))
      {
        sb.AppendLine("");
        sb.AppendLine("        // Example 1:  you can optionally sort the collection in ascending order by your chosen field ");
        sb.AppendLine("        " + this._table.VariableObjCollectionName + ".Sort(" + this._table.Name + ".By" + str + ");");
        sb.AppendLine("");
        sb.AppendLine("        // Example 2:  to sort in descending order, add this line to the Sort code in Example 1 ");
        sb.AppendLine("        " + this._table.VariableObjCollectionName + ".Reverse();");
      }
      sb.AppendLine("");
      sb.AppendLine("        // Example 3:  directly bind to a GridView - for ASP.NET Web Forms");
      sb.AppendLine("        // GridView grid = new GridView();");
      sb.AppendLine("        // grid.DataSource = " + this._table.VariableObjCollectionName + ";");
      sb.AppendLine("        // grid.DataBind();");
      sb.AppendLine("");
      sb.AppendLine("        // Example 4:  loop through all the " + this._table.Name + "(s)");
      sb.AppendLine("        foreach (" + this._table.NameFullyQualifiedBusinessObject + " " + this._table.VariableObjName + " in " + this._table.VariableObjCollectionName + ")");
      sb.AppendLine("        {");
      foreach (Column column in (List<Column>) this._table.Columns)
      {
        string keywordVariableName = Functions.GetNoneKeywordVariableName(column.NameCamelStyle, Language.CSharp);
        if (this._generatedSqlType == GeneratedSqlType.EFCore && column.SQLDataType == SQLType.uniqueidentifier)
        {
          if (column.IsNullable)
            sb.AppendLine("            Guid? " + keywordVariableName + " = " + this._table.VariableObjName + "." + column.Name + ";");
          else
            sb.AppendLine("            Guid " + keywordVariableName + " = " + this._table.VariableObjName + "." + column.Name + ";");
        }
        else
          sb.AppendLine("            " + column.SystemType + column.NullableChar + " " + keywordVariableName + " = " + this._table.VariableObjName + "." + column.Name + ";");
      }
      this.WriteRelatedTable(sb);
      sb.AppendLine("        }");
      sb.AppendLine("    }");
    }

    private void WriteSelectDropDownListData(StringBuilder sb)
    {
      string str1 = ".ToString()";
      string str2 = ".ToString()";
      if (this._table.FirstPrimaryKeySystemType.ToLower() == "string")
        str1 = string.Empty;
      if (this._table.DataTextFieldSystemType != null && this._table.DataTextFieldSystemType.ToLower() == "string")
        str2 = string.Empty;
      sb.AppendLine("");
      sb.AppendLine("    /// <summary>");
      sb.AppendLine("    /// The example below shows how to Select the " + this._table.FirstPrimaryKeyName + " and " + this._table.DataTextField + " columns for use with a with a Drop Down List, Combo Box, Checked Box List, List View, List Box, etc");
      sb.AppendLine("    /// </summary>");
      sb.AppendLine("    " + this._methodAccessablity + " void Select" + this._table.Name + MyConstants.WordDropDownListData + "()");
      sb.AppendLine("    {");
      if (this._isUseWebApi)
      {
        sb.AppendLine("        // uncomment this code if you would rather access the middle tier instead of the web api");
        sb.AppendLine("        // List<" + this._table.Name + "> " + this._table.VariableObjCollectionName + " = " + this._table.NameFullyQualifiedBusinessObject + ".Select" + this._table.Name + "DropDownListData();");
        sb.AppendLine("");
        sb.AppendLine("        // ** code using the web api instead of the middle tier **");
        sb.AppendLine("        List<" + this._table.Name + "> " + this._table.VariableObjCollectionName + " = null;");
        sb.AppendLine("");
        sb.AppendLine("        using (var client = new HttpClient())");
        sb.AppendLine("        {");
        sb.AppendLine("            client.BaseAddress = new Uri(Functions.GetWebApiBaseAddress());");
        sb.AppendLine("            HttpResponseMessage response = client.GetAsync(\"" + this._table.Name + "Api/Select" + this._table.Name + "DropDownListData\").Result;");
        sb.AppendLine("");
        sb.AppendLine("            if (response.IsSuccessStatusCode)");
        sb.AppendLine("            {");
        sb.AppendLine("                var responseBody = response.Content.ReadAsStringAsync().Result;");
        sb.AppendLine("                " + this._table.VariableObjCollectionName + " = JsonConvert.DeserializeObject<List<" + this._table.Name + ">>(responseBody);");
        sb.AppendLine("            }");
        sb.AppendLine("        }");
        sb.AppendLine("        // ** code using the web api instead of the middle tier **");
      }
      else
      {
        sb.AppendLine("        List<" + this._table.Name + "> " + this._table.VariableObjCollectionName + " = " + this._table.Name + ".Select" + this._table.Name + MyConstants.WordDropDownListData + "();");
        sb.AppendLine("");
        sb.AppendLine("        // Example 1:  directly bind to a drop down list - for ASP.NET Web Forms");
        sb.AppendLine("        // DropDownList ddl1 = new DropDownList();");
        sb.AppendLine("        // ddl1.DataValueField = \"" + this._table.FirstPrimaryKeyName + "\";");
        sb.AppendLine("        // ddl1.DataTextField = \"" + this._table.DataTextField + "\";");
        sb.AppendLine("        // ddl1.DataSource = " + this._table.VariableObjCollectionName + ";");
        sb.AppendLine("        // ddl1.DataBind();");
        sb.AppendLine("");
        sb.AppendLine("        // Example 2:  add each item through a loop - for ASP.NET Web Forms");
        sb.AppendLine("        // DropDownList ddl2 = new DropDownList();");
        sb.AppendLine("");
        sb.AppendLine("        // foreach (" + this._table.Name + " " + this._table.VariableObjName + " in " + this._table.VariableObjCollectionName + ")");
        sb.AppendLine("        // {");
        sb.AppendLine("        //     ddl2.Items.Add(new ListItem(" + this._table.VariableObjName + "." + this._table.DataTextField + str2 + ", " + this._table.VariableObjName + "." + this._table.FirstPrimaryKeyName + str1 + "));");
        sb.AppendLine("        // }");
        sb.AppendLine("");
        sb.AppendLine("        // Example 3:  bind to a combo box.  for Windows Forms (WinForms)");
        sb.AppendLine("        // ComboBox cbx1 = new ComboBox();");
        sb.AppendLine("");
        sb.AppendLine("        // foreach (" + this._table.Name + " " + this._table.VariableObjName + " in " + this._table.VariableObjCollectionName + ")");
        sb.AppendLine("        // {");
        sb.AppendLine("        //     cbx1.Items.Add(new ListItem(" + this._table.VariableObjName + "." + this._table.DataTextField + str2 + ", " + this._table.VariableObjName + "." + this._table.FirstPrimaryKeyName + str1 + "));");
        sb.AppendLine("        // }");
      }
      sb.AppendLine("    }");
    }

    private void WriteInsertExample(StringBuilder sb)
    {
      sb.AppendLine("");
      sb.AppendLine("    /// <summary>");
      sb.AppendLine("    /// Shows how to Insert or Create a New Record");
      sb.AppendLine("    /// </summary>");
      sb.AppendLine("    " + this._methodAccessablity + " void Insert()");
      sb.AppendLine("    {");
      sb.AppendLine("        // first instantiate a new " + this._table.Name ?? "");
      sb.AppendLine("        " + this._table.Name + " " + this._table.VariableObjName + " = new " + this._table.Name + "();");
      sb.AppendLine("");
      sb.AppendLine("        // assign values you want inserted");
      foreach (Column column in (List<Column>) this._table.Columns)
      {
        if (!column.IsComputed && (!column.IsPrimaryKey || !column.IsPrimaryKeyUnique))
        {
          string sampleData = this.GetSampleData(column, CrudOperation.Insert, this._generatedSqlType, false);
          sb.AppendLine("        " + this._table.VariableObjName + "." + column.Name + " = " + sampleData + ";");
        }
      }
      if (this._table.PrimaryKeyCount == 1)
      {
        sb.AppendLine("");
        sb.AppendLine("        // finally, insert a new record");
        sb.AppendLine("        // the insert method returns the newly created primary key");
        sb.AppendLine("        " + this._table.FirstPrimaryKeySystemType + " newlyCreatedPrimaryKey = " + this._table.VariableObjName + ".Insert();");
      }
      else
      {
        sb.AppendLine("");
        sb.AppendLine("        // finally, insert a new record");
        sb.AppendLine("        " + this._table.VariableObjName + ".Insert();");
      }
      if (this._isUseWebApi)
      {
        sb.AppendLine("");
        sb.AppendLine("        //// if you want to access the middle tier though the web api do this instead, make sure to comment code above");
        sb.AppendLine("        //string serializedModel = JsonConvert.SerializeObject(" + this._table.VariableObjName + ");");
        sb.AppendLine("        //");
        sb.AppendLine("        //using (var client = new HttpClient())");
        sb.AppendLine("        //{");
        sb.AppendLine("        //    client.BaseAddress = new Uri(Functions.GetWebApiBaseAddress());");
        sb.AppendLine("        //    HttpResponseMessage response = client.PostAsync(\"" + this._table.Name + "Api/Insert\", new StringContent(serializedModel, Encoding.UTF8, \"application/json\")).Result;");
        sb.AppendLine("        //");
        sb.AppendLine("        //    if (!response.IsSuccessStatusCode)");
        sb.AppendLine("        //        throw new Exception(\"Error Status Code: \" + response.StatusCode.ToString() + \" Error Reason: \" + response.ReasonPhrase + \" Error Message: \" + response.RequestMessage.ToString());");
        sb.AppendLine("        //}");
      }
      sb.AppendLine("    }");
    }

    private void WriteUpdateExample(StringBuilder sb)
    {
      string empty = string.Empty;
      sb.AppendLine("");
      sb.AppendLine("    /// <summary>");
      sb.AppendLine("    /// Shows how to Update an existing record by Primary Key");
      sb.AppendLine("    /// </summary>");
      sb.AppendLine("    " + this._methodAccessablity + " void Update()");
      sb.AppendLine("    {");
      sb.AppendLine("        // first instantiate a new " + this._table.Name ?? "");
      sb.AppendLine("        " + this._table.Name + " " + this._table.VariableObjName + " = new " + this._table.Name + "();");
      sb.AppendLine("");
      sb.AppendLine("        // assign the existing primary key(s)");
      sb.AppendLine("        // of the record you want updated");
      foreach (Column primaryKeyColumn in (List<Column>) this._table.PrimaryKeyColumns)
      {
        if (primaryKeyColumn.SystemType == "Guid")
          sb.AppendLine("        // e.g. Guid.Parse(\"" + Guid.NewGuid().ToString() + "\");");
        string sampleData = this.GetSampleData(primaryKeyColumn, CrudOperation.Update, this._generatedSqlType, false);
        sb.AppendLine("        " + this._table.VariableObjName + "." + primaryKeyColumn.Name + " = " + sampleData + ";");
      }
      sb.AppendLine("");
      sb.AppendLine("        // assign values you want updated");
      foreach (Column column in (List<Column>) this._table.Columns)
      {
        if (!column.IsPrimaryKey && !column.IsComputed)
        {
          string sampleData = this.GetSampleData(column, CrudOperation.None, this._generatedSqlType, false);
          sb.AppendLine("        " + this._table.VariableObjName + "." + column.Name + " = " + sampleData + ";");
        }
      }
      sb.AppendLine("");
      sb.AppendLine("        // finally, update an existing record");
      sb.AppendLine("        " + this._table.VariableObjName + ".Update();");
      if (this._isUseWebApi)
      {
        sb.AppendLine("");
        sb.AppendLine("        //// if you want to access the middle tier though the web api do this instead, make sure to comment code above");
        sb.AppendLine("        //string serializedModel = JsonConvert.SerializeObject(" + this._table.VariableObjName + ");");
        sb.AppendLine("        //");
        sb.AppendLine("        //using (var client = new HttpClient())");
        sb.AppendLine("        //{");
        sb.AppendLine("        //    client.BaseAddress = new Uri(Functions.GetWebApiBaseAddress());");
        sb.AppendLine("        //    HttpResponseMessage response = client.PostAsync(\"" + this._table.Name + "Api/Update\", new StringContent(serializedModel, Encoding.UTF8, \"application/json\")).Result;");
        sb.AppendLine("        //");
        sb.AppendLine("        //    if (!response.IsSuccessStatusCode)");
        sb.AppendLine("        //        throw new Exception(\"Error Status Code: \" + response.StatusCode.ToString() + \" Error Reason: \" + response.ReasonPhrase + \" Error Message: \" + response.RequestMessage.ToString());");
        sb.AppendLine("        //}");
      }
      sb.AppendLine("    }");
    }

    private void WriteDeleteExample(StringBuilder sb)
    {
      string delimitedSampleValues = this.GetDelimitedSampleValues(CrudOperation.Delete, this._generatedSqlType, true, false);
      sb.AppendLine("");
      sb.AppendLine("    /// <summary>");
      sb.AppendLine("    /// Shows how to Delete an existing record by Primary Key");
      sb.AppendLine("    /// </summary>");
      sb.AppendLine("    " + this._methodAccessablity + " void Delete()");
      sb.AppendLine("    {");
      sb.AppendLine("        // delete a record by primary key");
      if (this._table.FirstPrimaryKeySystemType == "Guid")
        sb.AppendLine("        // e.g. Guid.Parse(\"" + Guid.NewGuid().ToString() + "\");");
      sb.AppendLine("        " + this._table.Name + ".Delete(" + delimitedSampleValues + ");");
      if (this._isUseWebApi)
      {
        string[] sampleValueArray = delimitedSampleValues.Split(",".ToCharArray());
        string delimitedPrimaryKeys = sampleValueArray[0];
        string str = string.Empty;
        if (this._table.PrimaryKeyCount > 1)
        {
          delimitedPrimaryKeys = Functions.GetQueryStringDelimitedPrimaryKeys(this._table, Language.CSharp, sampleValueArray);
          str = "?";
        }
        sb.AppendLine("");
        sb.AppendLine("        //// if you want to access the middle tier though the web api do this instead, make sure to comment code above");
        sb.AppendLine("        //using (var client = new HttpClient())");
        sb.AppendLine("        //{");
        sb.AppendLine("        //    client.BaseAddress = new Uri(Functions.GetWebApiBaseAddress());");
        sb.AppendLine("        //    HttpResponseMessage response = client.DeleteAsync(\"" + this._table.Name + "Api/Delete/" + str + "\" + " + delimitedPrimaryKeys + ").Result;");
        sb.AppendLine("");
        sb.AppendLine("        //    if (!response.IsSuccessStatusCode)");
        sb.AppendLine("        //        throw new Exception(\"Error Status Code: \" + response.StatusCode.ToString() + \" Error Reason: \" + response.ReasonPhrase + \" Error Message: \" + response.RequestMessage.ToString());");
        sb.AppendLine("        //}");
      }
      sb.AppendLine("    }");
    }

    private void WriteGetRecordCountExample(StringBuilder sb)
    {
      sb.AppendLine("");
      sb.AppendLine("    /// <summary>");
      sb.AppendLine("    /// Shows how to get the total number of records");
      sb.AppendLine("    /// </summary>");
      sb.AppendLine("    " + this._methodAccessablity + " void GetRecordCount()");
      sb.AppendLine("    {");
      sb.AppendLine("        // get the total number of records in the " + this._table.Name + " table");
      sb.AppendLine("        int totalRecordCount = " + this._table.Name + ".GetRecordCount();");
      if (this._isUseWebApi)
      {
        sb.AppendLine("");
        sb.AppendLine("        //// if you want to access the middle tier though the web api do this instead, make sure to comment code above");
        sb.AppendLine("        //int totalRecordCount = 0;");
        sb.AppendLine("        //");
        sb.AppendLine("        //using (var client = new HttpClient())");
        sb.AppendLine("        //{");
        sb.AppendLine("        //    client.BaseAddress = new Uri(Functions.GetWebApiBaseAddress());");
        sb.AppendLine("        //    HttpResponseMessage response = client.GetAsync(\"" + this._table.Name + "Api/GetRecordCount/\").Result;");
        sb.AppendLine("        //");
        sb.AppendLine("        //    if (!response.IsSuccessStatusCode)");
        sb.AppendLine("        //    {");
        sb.AppendLine("        //        throw new Exception(\"Error Status Code: \" + response.StatusCode.ToString() + \" Error Reason: \" + response.ReasonPhrase + \" Error Message: \" + response.RequestMessage.ToString());");
        sb.AppendLine("        //    }");
        sb.AppendLine("        //    else");
        sb.AppendLine("        //    {");
        sb.AppendLine("        //        var responseBody = response.Content.ReadAsStringAsync().Result;");
        sb.AppendLine("        //        totalRecordCount = JsonConvert.DeserializeObject<int>(responseBody);");
        sb.AppendLine("        //    }");
        sb.AppendLine("        //}");
      }
      sb.AppendLine("    }");
    }

    private void WriteGetRecordByCountExample(StringBuilder sb)
    {
      foreach (Column foreignKeyColumn in this._table.ForeignKeyColumns)
      {
        string sampleData = this.GetSampleData(foreignKeyColumn, CrudOperation.Select, this._generatedSqlType, false);
        sb.AppendLine("");
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// Shows how to get the total number of records by " + foreignKeyColumn.Name);
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    " + this._methodAccessablity + " void GetRecordCountBy" + foreignKeyColumn.Name + "()");
        sb.AppendLine("    {");
        sb.AppendLine("        // get the total number of records in the " + this._table.Name + " table by " + foreignKeyColumn.Name);
        sb.AppendLine("        // " + sampleData + " here is just a sample " + foreignKeyColumn.Name + " change the value as you see fit");
        if (this._generatedSqlType == GeneratedSqlType.EFCore && foreignKeyColumn.SQLDataType == SQLType.uniqueidentifier)
          sb.AppendLine("        int totalRecordCount = " + this._table.Name + ".GetRecordCountBy" + foreignKeyColumn.Name + "(" + sampleData + ".ToString());");
        else
          sb.AppendLine("        int totalRecordCount = " + this._table.Name + ".GetRecordCountBy" + foreignKeyColumn.Name + "(" + sampleData + ");");
        if (this._isUseWebApi)
        {
          sb.AppendLine("");
          sb.AppendLine("        //// if you want to access the middle tier though the web api do this instead, make sure to comment code above");
          sb.AppendLine("        //int totalRecordCount = 0;");
          sb.AppendLine("        //");
          sb.AppendLine("        //using (var client = new HttpClient())");
          sb.AppendLine("        //{");
          sb.AppendLine("        //    client.BaseAddress = new Uri(Functions.GetWebApiBaseAddress());");
          sb.AppendLine("        //    HttpResponseMessage response = client.GetAsync(\"" + this._table.Name + "Api/GetRecordCountBy/\" + \"" + sampleData.Replace("\"", "") + "\").Result;");
          sb.AppendLine("        //");
          sb.AppendLine("        //    if (!response.IsSuccessStatusCode)");
          sb.AppendLine("        //    {");
          sb.AppendLine("        //        throw new Exception(\"Error Status Code: \" + response.StatusCode.ToString() + \" Error Reason: \" + response.ReasonPhrase + \" Error Message: \" + response.RequestMessage.ToString());");
          sb.AppendLine("        //    }");
          sb.AppendLine("        //    else");
          sb.AppendLine("        //    {");
          sb.AppendLine("        //        var responseBody = response.Content.ReadAsStringAsync().Result;");
          sb.AppendLine("        //        totalRecordCount = JsonConvert.DeserializeObject<int>(responseBody);");
          sb.AppendLine("        //    }");
          sb.AppendLine("        //}");
        }
        sb.AppendLine("    }");
      }
    }

    private void WriteGetRecordCountDynamicWhere(StringBuilder sb)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (Column primaryKeyColumn in (List<Column>) this._table.PrimaryKeyColumns)
      {
        if (!primaryKeyColumn.IsBinaryOrSpatialDataType)
          stringBuilder.AppendLine("        " + primaryKeyColumn.VariableFieldAssignmentClearValues);
      }
      foreach (Column spatialDataTypeColumn in this._table.NoneBinaryOrSpatialDataTypeColumns)
      {
        if (!spatialDataTypeColumn.IsPrimaryKey)
          stringBuilder.AppendLine("        " + spatialDataTypeColumn.VariableFieldAssignmentClearValues);
      }
      sb.AppendLine("");
      sb.AppendLine("    /// <summary>");
      sb.AppendLine("    /// Shows how to get the total number of records based on Search Parameters.");
      sb.AppendLine("    /// </summary>");
      sb.AppendLine("    " + this._methodAccessablity + " void GetRecordCountDynamicWhere()");
      sb.AppendLine("    {");
      sb.AppendLine("        // search parameters, everything is nullable, only items being searched for should be filled");
      sb.AppendLine("        // note: fields with String type uses a LIKE search, everything else uses an exact match");
      sb.AppendLine("        // also, every field you're searching for uses the AND operator");
      sb.AppendLine("        // e.g. int? productID = 1; string productName = \"ch\";");
      sb.AppendLine("        // will translate to: SELECT....WHERE productID = 1 AND productName LIKE '%ch%'");
      sb.AppendLine(stringBuilder.ToString());
      sb.AppendLine("        int totalRecordCount = " + this._table.Name + ".GetRecordCountDynamicWhere(" + Functions.GetCommaDelimitedParamsForPassingToSearchMethod(this._table) + ");");
      sb.AppendLine("    }");
    }

    private void WriteSelectSkipAndTakeExample(StringBuilder sb)
    {
      sb.AppendLine("");
      sb.AppendLine("    /// <summary>");
      sb.AppendLine("    /// Shows how to get a specific number of sorted records, starting from an index.  The total number of records are also retrieved when using the SelectSkipAndTake() method.");
      sb.AppendLine("    /// For example, if there are 200 records take only 10 records (numberOfRecordsToRetrieve), starting from the first index (startRetrievalFromRecordIndex = 0)");
      sb.AppendLine("    /// The example below uses some variables, here are their definitions:");
      sb.AppendLine("    /// totalRecordCount - total number of records if you were to retrieve everything");
      sb.AppendLine("    /// startRetrievalFromRecordIndex - the index to start taking records from. Zero (0) E.g. If you want to skip the first 20 records, then assign 19 here.");
      sb.AppendLine("    /// numberOfRecordsToRetrieve - take n records starting from the startRetrievalFromRecordIndex");
      sb.AppendLine("    /// sortBy - to sort in Ascending order by Field Name, just assign just the Field Name, do not pass 'asc'");
      sb.AppendLine("    /// sortBy - to sort in Descending order by Field Name, use the Field Name, a space and the word 'desc'");
      sb.AppendLine("    /// </summary>");
      sb.AppendLine("    " + this._methodAccessablity + " void SelectSkipAndTake()");
      sb.AppendLine("    {");
      sb.AppendLine("        int startRetrievalFromRecordIndex = 0;");
      sb.AppendLine("        int numberOfRecordsToRetrieve = 10;");
      sb.AppendLine("        string sortBy = \"" + this._table.FirstPrimaryKeyName + "\";");
      sb.AppendLine("        //string sortBy = \"" + this._table.FirstPrimaryKeyName + " desc\";");
      sb.AppendLine("");
      sb.AppendLine("        // 1. select a specific number of sorted records starting from the index you specify");
      sb.AppendLine("        List<" + this._table.Name + "> " + this._table.VariableObjCollectionName + " = " + this._table.Name + ".SelectSkipAndTake(numberOfRecordsToRetrieve, startRetrievalFromRecordIndex, sortBy);");
      sb.AppendLine("");
      sb.AppendLine("        // to use " + this._table.VariableObjCollectionName + " please see the SelectAll() method examples");
      sb.AppendLine("        // No need for Examples 1 and 2 because the Collection here is already sorted");
      sb.AppendLine("        // Example 2:  directly bind to a GridView - for ASP.NET Web Forms");
      sb.AppendLine("        // Example 3:  loop through all the " + this._table.Name + "(s).  The example above will only loop for 10 items.");
      sb.AppendLine("    }");
    }

    private void WriteSelectSkipAndTakeByExample(StringBuilder sb)
    {
      foreach (Column column in (List<Column>) this._table.Columns)
      {
        if (column.IsForeignKey)
        {
          string sampleData = this.GetSampleData(column, CrudOperation.Select, this._generatedSqlType, false);
          sb.AppendLine("");
          sb.AppendLine("    /// <summary>");
          sb.AppendLine("    /// Shows how to get a specific number of sorted records, starting from an index by the related Field Name.  The total number of records are also retrieved when using the SelectSkipAndTake() method.");
          sb.AppendLine("    /// For example, if there are 200 records, take only 10 records (numberOfRecordsToRetrieve), starting from the first index (startRetrievalFromRecordIndex = 0)");
          sb.AppendLine("    /// The example below uses some variables, here are their definitions:");
          sb.AppendLine("    /// totalRecordCount - total number of records if you were to retrieve everything");
          sb.AppendLine("    /// startRetrievalFromRecordIndex - the index to start taking records from. Zero (0) E.g. If you want to skip the first 20 records, then assign 19 here.");
          sb.AppendLine("    /// numberOfRecordsToRetrieve - take n records starting from the startRetrievalFromRecordIndex");
          sb.AppendLine("    /// sortBy - to sort in Ascending order by Field Name, just assign just the Field Name, do not pass 'asc'");
          sb.AppendLine("    /// sortBy - to sort in Descending order by Field Name, use the Field Name, a space and the word 'desc'");
          sb.AppendLine("    /// </summary>");
          sb.AppendLine("    " + this._methodAccessablity + " void SelectSkipAndTakeBy" + column.Name + "()");
          sb.AppendLine("    {");
          sb.AppendLine("        int startRetrievalFromRecordIndex = 0;");
          sb.AppendLine("        int numberOfRecordsToRetrieve = 10;");
          sb.AppendLine("        string sortBy = \"" + this._table.FirstPrimaryKeyName + "\";");
          sb.AppendLine("        //string sortBy = \"" + this._table.FirstPrimaryKeyName + " desc\";");
          sb.AppendLine("");
          sb.AppendLine("        // 1. select a specific number of sorted records with a " + column.Name + " = " + sampleData);
          sb.AppendLine("        // starting from the index you specify");
          if (this._generatedSqlType == GeneratedSqlType.EFCore && column.SQLDataType == SQLType.uniqueidentifier)
            sb.AppendLine("        List<" + this._table.Name + "> " + this._table.VariableObjCollectionName + " = " + this._table.Name + ".SelectSkipAndTakeBy" + column.Name + "(numberOfRecordsToRetrieve, startRetrievalFromRecordIndex, sortBy, " + sampleData + ".ToString());");
          else
            sb.AppendLine("        List<" + this._table.Name + "> " + this._table.VariableObjCollectionName + " = " + this._table.Name + ".SelectSkipAndTakeBy" + column.Name + "(numberOfRecordsToRetrieve, startRetrievalFromRecordIndex, sortBy, " + sampleData + ");");
          sb.AppendLine("");
          sb.AppendLine("        // to use " + this._table.VariableObjCollectionName + " please see the SelectAll() method examples");
          sb.AppendLine("        // No need for Examples 1 and 2 because the Collection here is already sorted");
          sb.AppendLine("        // Example 3:  directly bind to a GridView - for ASP.NET Web Forms");
          sb.AppendLine("        // Example 4:  loop through all the " + this._table.Name + "(s).  The example above will only loop for 10 items.");
          sb.AppendLine("    }");
        }
      }
    }

    private void WriteSelectSkipAndTakeDynamicWhereExample(StringBuilder sb)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (Column primaryKeyColumn in (List<Column>) this._table.PrimaryKeyColumns)
      {
        if (!primaryKeyColumn.IsBinaryOrSpatialDataType)
          stringBuilder.AppendLine("        " + primaryKeyColumn.VariableFieldAssignmentClearValues);
      }
      foreach (Column spatialDataTypeColumn in this._table.NoneBinaryOrSpatialDataTypeColumns)
      {
        if (!spatialDataTypeColumn.IsPrimaryKey)
          stringBuilder.AppendLine("        " + spatialDataTypeColumn.VariableFieldAssignmentClearValues);
      }
      sb.AppendLine("");
      sb.AppendLine("    /// <summary>");
      sb.AppendLine("    /// Shows how to get a specific number of sorted records, starting from an index, based on Search Parameters.  The number of records are also retrieved.");
      sb.AppendLine("    /// </summary>");
      sb.AppendLine("    " + this._methodAccessablity + " void SelectSkipAndTakeDynamicWhere()");
      sb.AppendLine("    {");
      sb.AppendLine("        int startRetrievalFromRecordIndex = 0;");
      sb.AppendLine("        int numberOfRecordsToRetrieve = 10;");
      sb.AppendLine("        string sortBy = \"" + this._table.FirstPrimaryKeyName + "\";");
      sb.AppendLine("        //string sortBy = \"" + this._table.FirstPrimaryKeyName + " desc\";");
      sb.AppendLine("");
      sb.AppendLine("        // search parameters, everything is nullable, only items being searched for should be filled");
      sb.AppendLine("        // note: fields with String type uses a LIKE search, everything else uses an exact match");
      sb.AppendLine("        // also, every field you're searching for uses the AND operator");
      sb.AppendLine("        // e.g. int? productID = 1; string productName = \"ch\";");
      sb.AppendLine("        // will translate to: SELECT....WHERE productID = 1 AND productName LIKE '%ch%'");
      sb.AppendLine(stringBuilder.ToString());
      sb.AppendLine("        // 1. select a specific number of sorted records starting from the index you specify based on Search Parameters");
      sb.AppendLine("        List<" + this._table.Name + "> " + this._table.VariableObjCollectionName + " = " + this._table.Name + ".SelectSkipAndTakeDynamicWhere(" + Functions.GetCommaDelimitedParamsForPassingToSearchMethod(this._table) + ", numberOfRecordsToRetrieve, startRetrievalFromRecordIndex, sortBy);");
      sb.AppendLine("");
      sb.AppendLine("        // to use " + this._table.VariableObjCollectionName + " please see the SelectAll() method examples");
      sb.AppendLine("        // No need for Examples 1 and 2 because the Collection here is already sorted");
      sb.AppendLine("        // Example 3:  directly bind to a GridView - for ASP.NET Web Forms");
      sb.AppendLine("        // Example 4:  loop through all the " + this._table.Name + "(s).  The example above will only loop for 10 items.");
      sb.AppendLine("    }");
    }

    private void WriteSelectAllDynamicWhereExample(StringBuilder sb)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (Column primaryKeyColumn in (List<Column>) this._table.PrimaryKeyColumns)
      {
        if (!primaryKeyColumn.IsBinaryOrSpatialDataType)
          stringBuilder.AppendLine("        " + primaryKeyColumn.VariableFieldAssignmentClearValues);
      }
      foreach (Column spatialDataTypeColumn in this._table.NoneBinaryOrSpatialDataTypeColumns)
      {
        if (!spatialDataTypeColumn.IsPrimaryKey)
          stringBuilder.AppendLine("        " + spatialDataTypeColumn.VariableFieldAssignmentClearValues);
      }
      sb.AppendLine("");
      sb.AppendLine("    /// <summary>");
      sb.AppendLine("    /// Shows how to get all records based on Search Parameters.");
      sb.AppendLine("    /// </summary>");
      sb.AppendLine("    " + this._methodAccessablity + " void SelectAllDynamicWhere()");
      sb.AppendLine("    {");
      sb.AppendLine("        // search parameters, everything is nullable, only items being searched for should be filled");
      sb.AppendLine("        // note: fields with String type uses a LIKE search, everything else uses an exact match");
      sb.AppendLine("        // also, every field you're searching for uses the AND operator");
      sb.AppendLine("        // e.g. int? productID = 1; string productName = \"ch\";");
      sb.AppendLine("        // will translate to: SELECT....WHERE productID = 1 AND productName LIKE '%ch%'");
      sb.AppendLine(stringBuilder.ToString());
      sb.AppendLine("        List<" + this._table.Name + "> " + this._table.VariableObjCollectionName + " = " + this._table.Name + ".SelectAllDynamicWhere(" + Functions.GetCommaDelimitedParamsForPassingToSearchMethod(this._table) + ");");
      sb.AppendLine("    }");
    }

    private void WriteSelectAllDynamicWhereSortExample(StringBuilder sb)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (Column primaryKeyColumn in (List<Column>) this._table.PrimaryKeyColumns)
      {
        if (!primaryKeyColumn.IsBinaryOrSpatialDataType)
          stringBuilder.AppendLine("        " + primaryKeyColumn.VariableFieldAssignmentClearValues);
      }
      foreach (Column spatialDataTypeColumn in this._table.NoneBinaryOrSpatialDataTypeColumns)
      {
        if (!spatialDataTypeColumn.IsPrimaryKey)
          stringBuilder.AppendLine("        " + spatialDataTypeColumn.VariableFieldAssignmentClearValues);
      }
      sb.AppendLine("");
      sb.AppendLine("    /// <summary>");
      sb.AppendLine("    /// Shows how to get all records based on Search Parameters sorted by column name in either ascending or descending order.");
      sb.AppendLine("    /// </summary>");
      sb.AppendLine("    " + this._methodAccessablity + " void SelectAllDynamicWhereWithSortExpression()");
      sb.AppendLine("    {");
      sb.AppendLine("        // search parameters, everything is nullable, only items being searched for should be filled");
      sb.AppendLine("        // note: fields with String type uses a LIKE search, everything else uses an exact match");
      sb.AppendLine("        // also, every field you're searching for uses the AND operator");
      sb.AppendLine("        // e.g. int? productID = 1; string productName = \"ch\";");
      sb.AppendLine("        // will translate to: SELECT....WHERE productID = 1 AND productName LIKE '%ch%'");
      sb.AppendLine(stringBuilder.ToString());
      sb.AppendLine("        string sortBy = \"" + this._table.FirstPrimaryKeyName + "\"; // ascending order");
      sb.AppendLine("        //string sortBy = \"" + this._table.FirstPrimaryKeyName + " desc\"; // descending order");
      sb.AppendLine("");
      sb.AppendLine("        List<" + this._table.Name + "> " + this._table.VariableObjCollectionName + " = " + this._table.Name + ".SelectAllDynamicWhere(" + Functions.GetCommaDelimitedParamsForPassingToSearchMethod(this._table) + ", sortBy);");
      sb.AppendLine("    }");
    }

    private string GetDelimitedSampleValues(CrudOperation operation, GeneratedSqlType generatedSqlType, bool isUseToStringWhenGuid, bool isSlashDelimited = false)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (Column primaryKeyColumn in (List<Column>) this._table.PrimaryKeyColumns)
      {
        string sampleData = this.GetSampleData(primaryKeyColumn, operation, generatedSqlType, isUseToStringWhenGuid);
        if (isSlashDelimited)
          stringBuilder.Append(sampleData + "/");
        else
          stringBuilder.Append(sampleData + ", ");
      }
      if (stringBuilder.Length <= 0)
        return (string) null;
      if (isSlashDelimited)
        stringBuilder.Replace("/", "", stringBuilder.ToString().LastIndexOf("/"), 1);
      else
        stringBuilder.Replace(",", "", stringBuilder.ToString().LastIndexOf(","), 1);
      return stringBuilder.ToString().Trim();
    }

    private string[] GetPkVariablesWithSampleValues(CrudOperation operation, GeneratedSqlType generatedSqlType)
    {
      int index = 0;
      string[] strArray = new string[this._table.PrimaryKeyCount];
      foreach (Column primaryKeyColumn in (List<Column>) this._table.PrimaryKeyColumns)
      {
        string sampleData = this.GetSampleData(primaryKeyColumn, operation, generatedSqlType, false);
        if (this._generatedSqlType == GeneratedSqlType.EFCore && primaryKeyColumn.SQLDataType == SQLType.uniqueidentifier)
          strArray[index] = "Guid " + primaryKeyColumn.NameCamelStyle + "Sample = Guid.NewGuid();";
        else
          strArray[index] = primaryKeyColumn.SystemType + " " + primaryKeyColumn.NameCamelStyle + "Sample = " + sampleData + ";";
        ++index;
      }
      return strArray;
    }

    private string GetSampleData(Column column, CrudOperation operation, GeneratedSqlType generatedSqlType, bool isUseToStringWhenGuid)
    {
      if (this._generatedSqlType == GeneratedSqlType.EFCore && column.SQLDataType == SQLType.uniqueidentifier)
        return isUseToStringWhenGuid ? "Guid.NewGuid().ToString()" : "Guid.NewGuid()";
      if (string.IsNullOrEmpty(column.SampleData))
        return this.GetConstantValueByType(column, operation);
      if (this._generatedSqlType == GeneratedSqlType.EFCore && column.SQLDataType == SQLType.uniqueidentifier)
        return isUseToStringWhenGuid ? "Guid.NewGuid().ToString()" : "Guid.NewGuid()";
      if (column.SystemType.ToLower() == "string" && column.SQLDataType == SQLType.xml)
        return "\"<root><name>Your Name</name></root>\"";
      if (column.SystemType.ToLower() == "string")
        return "\"" + column.SampleData.Trim().Replace(Environment.NewLine, " ").Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"";
      if (column.SystemTypeNative == "DateTime")
        return "DateTime.Now";
      if (column.SystemType == "TimeSpan")
        return "DateTime.Parse(\"23:59:59\").TimeOfDay";
      if (column.SystemType == "DateTimeOffset")
        return "DateTimeOffset.Parse(\"1666-09-02 1:00:00+0\")";
      if (column.SystemType == "int" || column.SystemType == "Int64" || (column.SystemType == "Int16" || column.SystemType == "byte"))
      {
        if (!column.IsPrimaryKey || this._table.IsContainsPrimaryAndForeignKeyColumnsOnly || operation != CrudOperation.Insert && operation != CrudOperation.Delete)
          return column.SampleData;
        DataSet dataSet = this._dbase.GetDataSet(("Select Top 1 [" + column.NameOriginal + "] From [" + column.TableOwnerOriginal + "].[" + column.TableNameOriginal + "] Order By [" + column.NameOriginal + "] Desc ").ToString(), true);
        if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
          return Convert.ToString(Convert.ToInt64(dataSet.Tables[0].Rows[0][column.NameOriginal]) + 1L);
        return column.SampleData;
      }
      if (column.SystemType == "double")
        return column.SampleData;
      if (column.SystemType == "Single")
        return column.SampleData + "f";
      if (column.SystemType == "bool")
        return column.SampleData == "1" ? "true" : "false";
      if (column.SystemType == "decimal")
        return "Convert.ToDecimal(\"" + column.SampleData + "\")";
      if (column.SystemType == "System.Xml.Linq.XElement")
        return "System.Xml.Linq.XElement.Parse(\"<root><name>Your Name</name></root>\")";
      if (!(column.SystemType == "Guid"))
        return "null";
      if (operation == CrudOperation.Select || operation == CrudOperation.Update || operation == CrudOperation.Delete)
        return "Guid.Parse(\"" + column.SampleData + "\")";
      return "Guid.NewGuid()";
    }

    private string GetConstantValueByType(Column column, CrudOperation operation)
    {
      if (column.SystemType.ToLower() == "string" && column.SQLDataType == SQLType.xml)
        return "\"<root><name>Your Name</name></root>\"";
      if (column.SystemType.ToLower() == "string")
        return "\"abc\"";
      if (column.SystemTypeNative == "DateTime")
        return "DateTime.Now";
      if (column.SystemType == "TimeSpan")
        return "DateTime.Parse(\"23:59:59\").TimeOfDay";
      if (column.SystemType == "DateTimeOffset")
        return "DateTimeOffset.Parse(\"1666-09-02 1:00:00+0\")";
      if (column.SystemType == "int" || column.SystemType == "Int64" || (column.SystemType == "Int16" || column.SystemType == "byte"))
        return "12";
      if (column.SystemType == "double")
        return "9.4";
      if (column.SystemType == "Single")
        return "2F";
      if (column.SystemType == "bool")
        return "true";
      if (column.SystemType == "decimal")
        return "52.4m";
      if (column.SystemType == "System.Xml.Linq.XElement")
        return "System.Xml.Linq.XElement.Parse(\"<root><name>Your Name</name></root>\")";
      if (!(column.SystemType == "Guid"))
        return "null";
      return operation == CrudOperation.Select || operation == CrudOperation.Update || operation == CrudOperation.Delete ? "Guid.Parse(\"\")" : "Guid.NewGuid()";
    }

    private void WriteRelatedTable(StringBuilder sb)
    {
      int num = 1;
      foreach (Column column in this._table.ForeignKeyColumnsTableIsSelectedAndOnlyOnePK)
      {
        string str1 = column.ForeignKeyTable.VariableObjName + "RelatedTo" + column.Name;
        string name = column.ForeignKeyTable.Name;
        if (this._table.Name == column.ForeignKeyTable.Name)
        {
          str1 = this._table.VariableObjName + "RelatedTo" + column.Name;
          string str2 = column.ForeignKeyTable.Name + column.Name;
        }
        sb.AppendLine("");
        sb.AppendLine("            // get the " + column.ForeignKeyTableName + " related to " + column.Name + ".");
        if (column.IsNullable)
        {
          if (this._generatedSqlType == GeneratedSqlType.EFCore && column.SQLDataType == SQLType.uniqueidentifier)
            sb.AppendLine("            if (!" + this._table.VariableObjName + "." + column.Name + ".HasValue)");
          else if (column.SystemType.ToLower() == "string")
            sb.AppendLine("            if (!String.IsNullOrEmpty(" + this._table.VariableObjName + "." + column.Name + "))");
          else
            sb.AppendLine("            if (" + this._table.VariableObjName + "." + column.Name + " != null)");
          sb.AppendLine("            {");
          if (column.Name != column.ForeignKeyColumnName)
          {
            if (this._generatedSqlType == GeneratedSqlType.EFCore && this._appVersion == ApplicationVersion.ProfessionalPlus)
              sb.AppendLine("                " + column.ForeignKeyTableName + " " + str1 + " = " + this._table.VariableObjName + "." + column.Name + "Navigation; ");
            else if (this._generatedSqlType == GeneratedSqlType.EFCore && column.SQLDataType == SQLType.uniqueidentifier)
              sb.AppendLine("                " + column.ForeignKeyTableName + " " + str1 + " = " + column.ForeignKeyTableName + ".SelectByPrimaryKey(" + column.NameCamelStyle + ".Value.ToString()); ");
            else if (column.IsStringField)
              sb.AppendLine("                " + column.ForeignKeyTableName + " " + str1 + " = " + column.ForeignKeyTableName + ".SelectByPrimaryKey(" + column.NameCamelStyle + "); ");
            else
              sb.AppendLine("                " + column.ForeignKeyTableName + " " + str1 + " = " + column.ForeignKeyTableName + ".SelectByPrimaryKey(" + column.NameCamelStyle + ".Value); ");
          }
          else if (this._table.Name == column.SingularForeignKeyTableName)
          {
            if (this._generatedSqlType == GeneratedSqlType.EFCore && column.SQLDataType == SQLType.uniqueidentifier)
              sb.AppendLine("                " + column.ForeignKeyTableName + " " + str1 + " = " + column.ForeignKeyTableName + ".SelectByPrimaryKey(" + column.NameCamelStyle + ".Value.ToString()); ");
            else if (column.IsStringField)
              sb.AppendLine("                " + column.ForeignKeyTableName + " " + str1 + " = " + column.ForeignKeyTableName + ".SelectByPrimaryKey(" + column.NameCamelStyle + "); ");
            else
              sb.AppendLine("                " + column.ForeignKeyTableName + " " + str1 + " = " + column.ForeignKeyTableName + ".SelectByPrimaryKey(" + column.NameCamelStyle + ".Value); ");
            ++num;
          }
          else if (this._generatedSqlType == GeneratedSqlType.EFCore && column.SQLDataType == SQLType.uniqueidentifier)
            sb.AppendLine("                " + column.ForeignKeyTableName + " " + str1 + " = " + column.ForeignKeyTableName + ".SelectByPrimaryKey(" + column.NameCamelStyle + ".Value.ToString()); ");
          else if (column.IsStringField)
            sb.AppendLine("                " + column.ForeignKeyTableName + " " + str1 + " = " + column.ForeignKeyTableName + ".SelectByPrimaryKey(" + column.NameCamelStyle + "); ");
          else
            sb.AppendLine("                " + column.ForeignKeyTableName + " " + str1 + " = " + column.ForeignKeyTableName + ".SelectByPrimaryKey(" + column.NameCamelStyle + ".Value); ");
          sb.AppendLine("            }");
        }
        else if (column.Name != column.ForeignKeyColumnName)
        {
          if (this._generatedSqlType == GeneratedSqlType.EFCore && this._appVersion == ApplicationVersion.ProfessionalPlus)
            sb.AppendLine("            " + column.ForeignKeyTableName + " " + str1 + " = " + this._table.VariableObjName + "." + column.Name + "Navigation; ");
          else if (this._generatedSqlType == GeneratedSqlType.EFCore && column.SQLDataType == SQLType.uniqueidentifier)
            sb.AppendLine("            " + column.ForeignKeyTableName + " " + str1 + " = " + column.ForeignKeyTableName + ".SelectByPrimaryKey(" + column.NameCamelStyle + ".ToString()); ");
          else
            sb.AppendLine("            " + column.ForeignKeyTableName + " " + str1 + " = " + column.ForeignKeyTableName + ".SelectByPrimaryKey(" + column.NameCamelStyle + "); ");
        }
        else if (this._table.Name == column.SingularForeignKeyTableName)
        {
          if (this._generatedSqlType == GeneratedSqlType.EFCore && column.SQLDataType == SQLType.uniqueidentifier)
            sb.AppendLine("            " + column.ForeignKeyTableName + " " + str1 + " = " + column.ForeignKeyTableName + ".SelectByPrimaryKey(" + column.NameCamelStyle + ".ToString()); ");
          else
            sb.AppendLine("            " + column.ForeignKeyTableName + " " + str1 + " = " + column.ForeignKeyTableName + ".SelectByPrimaryKey(" + column.NameCamelStyle + "); ");
          ++num;
        }
        else if (this._generatedSqlType == GeneratedSqlType.EFCore && column.SQLDataType == SQLType.uniqueidentifier)
          sb.AppendLine("            " + column.ForeignKeyTableName + " " + str1 + " = " + column.ForeignKeyTableName + ".SelectByPrimaryKey(" + column.NameCamelStyle + ".ToString()); ");
        else
          sb.AppendLine("            " + column.ForeignKeyTableName + " " + str1 + " = " + column.ForeignKeyTableName + ".SelectByPrimaryKey(" + column.NameCamelStyle + "); ");
      }
    }
  }
}
