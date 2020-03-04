
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class BusinessObjectBase
  {
    private Table _table;
    private Tables _selectedTables;
    private DataTable _referencedTables;
    private const string _fileExtension = ".cs";
    private string _apiName;
    private string _apiNameDirectory;
    private const Language _language = Language.CSharp;
    private DatabaseObjectToGenerateFrom _generateFrom;
    private bool _isUseStoredProcedure;
    private ApplicationVersion _appVersion;
    private BusinessObjectBaseType _businessObjectBaseType;
    private string _businessObjectName;
    private GeneratedSqlType _generatedSqlType;

    internal BusinessObjectBase()
    {
    }

    internal BusinessObjectBase(Table table, Tables selectedTables, DataTable referencedTables, string apiName, string apiNameDirectory, DatabaseObjectToGenerateFrom generateFrom, bool isUseStoredProcedure, ApplicationVersion appVersion, GeneratedSqlType generatedSqlType, BusinessObjectBaseType businessObjectBaseType = BusinessObjectBaseType.BusinessObjectBase)
    {
      this._table = table;
      this._selectedTables = selectedTables;
      this._referencedTables = referencedTables;
      this._apiName = apiName;
      this._apiNameDirectory = apiNameDirectory + MyConstants.DirectoryBusinessObjectBase;
      this._generateFrom = generateFrom;
      this._appVersion = appVersion;
      this._isUseStoredProcedure = isUseStoredProcedure;
      this._businessObjectBaseType = businessObjectBaseType;
      this._generatedSqlType = generatedSqlType;
      if (this._businessObjectBaseType == BusinessObjectBaseType.BusinessObjectBase)
        this.Generate();
      else
        this._businessObjectName = Functions.GetFullyQualifiedTableName(this._table, this._selectedTables, Language.CSharp, this._table.NameFullyQualifiedBusinessObject, apiName);
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._apiNameDirectory + this._table.Name + MyConstants.WordBase + ".cs"))
      {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("using System;");
        sb.AppendLine("using " + this._apiName + ".DataLayer;");
        sb.AppendLine("using System.Collections.Generic;");
        sb.AppendLine("using System.Threading.Tasks;");
        sb.AppendLine("");
        sb.AppendLine("namespace " + this._apiName + ".BusinessObject.Base");
        sb.AppendLine("{");
        sb.AppendLine("     /// <summary>");
        sb.AppendLine("     /// Base class for " + this._table.Name + ".  Do not make changes to this class,");
        sb.AppendLine("     /// instead, put additional code in the " + this._table.Name + " class");
        sb.AppendLine("     /// </summary>");
        sb.AppendLine("     public class " + this._table.Name + MyConstants.WordBase + " : " + this._apiName + "." + MyConstants.WordModels + "." + this._table.Name + MyConstants.WordModel);
        sb.AppendLine("     {");
        this.WriteProperties(sb);
        this.WriteConstructor(sb);
        this.WriteMethods(sb);
        this.WriteComparison(sb);
        sb.AppendLine("     }");
        sb.AppendLine("}");
        streamWriter.Write(sb.ToString());
      }
    }

    private void WriteProperties(StringBuilder sb)
    {
      if (this._generatedSqlType == GeneratedSqlType.StoredProcedures || this._generatedSqlType == GeneratedSqlType.AdHocSQL)
      {
        StringBuilder stringBuilder = new StringBuilder();
        int num = 0;
        foreach (Column currentColumn in this._table.ForeignKeyColumnsTableIsSelectedAndOnlyOnePK)
        {
          string foreignKeyTableName = currentColumn.ForeignKeyTableName;
          if (stringBuilder.ToString().Contains(currentColumn.ForeignKeyTableName + ","))
          {
            ++num;
            foreignKeyTableName += num.ToString();
          }
          sb.AppendLine("         /// <summary>");
          sb.AppendLine("         /// Gets or sets the Related " + currentColumn.ForeignKeyTableName + ".  Related to column " + currentColumn.Name ?? "");
          sb.AppendLine("         /// </summary>");
          if (this._table.Name == currentColumn.ForeignKeyTable.Name)
            sb.AppendLine("         public Lazy<" + currentColumn.ForeignKeyTableName + "> " + currentColumn.ForeignKeyTable.Name + currentColumn.Name);
          else
            sb.AppendLine("         public Lazy<" + currentColumn.ForeignKeyTableName + "> " + foreignKeyTableName);
          sb.AppendLine("         {");
          sb.AppendLine("             get");
          sb.AppendLine("             {");
          if (currentColumn.IsNullable)
          {
            if (currentColumn.SystemType.ToLower() == "string")
            {
              sb.AppendLine("                 if(!String.IsNullOrEmpty(" + currentColumn.Name + "))");
              sb.AppendLine("                     return new Lazy<" + currentColumn.ForeignKeyTableName + ">(() => " + MyConstants.WordBusinessObject + "." + currentColumn.ForeignKeyTableName + ".SelectByPrimaryKey(" + currentColumn.Name + "));");
              sb.AppendLine("                 else");
              sb.AppendLine("                     return null;");
            }
            else
            {
              sb.AppendLine("                 bool hasValue = " + currentColumn.SystemTypeNative + ".TryParse(" + currentColumn.Name + ".ToString(), out " + currentColumn.SystemType + " value);");
              sb.AppendLine("");
              sb.AppendLine("                 if (hasValue)");
              sb.AppendLine("                     return new Lazy<" + currentColumn.ForeignKeyTableName + ">(() => " + MyConstants.WordBusinessObject + "." + currentColumn.ForeignKeyTableName + ".SelectByPrimaryKey(value));");
              sb.AppendLine("                 else");
              sb.AppendLine("                     return null;");
            }
          }
          else
          {
            string variableWithConvert = Functions.GetForeignKeyVariableWithConvert(currentColumn.ForeignKeyTable, currentColumn);
            sb.AppendLine("                 return new Lazy<" + currentColumn.ForeignKeyTableName + ">(() => " + MyConstants.WordBusinessObject + "." + currentColumn.ForeignKeyTableName + ".SelectByPrimaryKey(" + variableWithConvert + "));");
          }
          sb.AppendLine("             }");
          sb.AppendLine("             set{ }");
          sb.AppendLine("         } ");
          sb.AppendLine("");
          stringBuilder.Append(currentColumn.ForeignKeyTableName + ",");
        }
      }
      Functions.WriteRelatedTableStringBuilder(this._table, sb, RelatedTablePart.BusinessObjectBaseWriteProperties, this._selectedTables, this._referencedTables, Language.CSharp);
      sb.AppendLine("");
    }

    private void WriteConstructor(StringBuilder sb)
    {
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Constructor");
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         public " + this._table.Name + MyConstants.WordBase + "()");
      sb.AppendLine("         {");
      sb.AppendLine("         }");
    }

    private void WriteMethods(StringBuilder sb)
    {
      if (this._generateFrom == DatabaseObjectToGenerateFrom.Tables)
      {
        if (!this._table.IsContainsPrimaryAndForeignKeyColumnsOnly)
          this.WriteSelectByPrimaryKeyMethod(sb);
        this.WriteGetRecordCountMethod(sb);
        this.WriteGetRecordByCountMethods(sb);
        this.WriteGetRecordCountDynamicWhere(sb);
        this.WriteSelectSkipAndTakeMethod(sb);
        this.WriteSelectSkipAndTakeByMethods(sb);
        this.WriteSelectSkipAndTakeDynamicWhereMethod(sb);
        this.WriteSelectAllMethod(sb);
        this.WriteSelectAllWithSortExpressionMethod(sb);
        this.WriteSelectAllDynamicWhereMethod(sb);
        this.WriteSelectAllDynamicWhereWithSortExpressionMethod(sb);
        this.WriteCollectionByMethods(sb);
        if (!this._table.IsContainsPrimaryAndForeignKeyColumnsOnly && this._table.PrimaryKeyCount == 1)
          this.WriteSelectDropDownListData(sb);
        this.WriteSortByExpressionMethod(sb);
        this.WriteInsertMethod(sb);
        if (!this._table.IsContainsPrimaryAndForeignKeyColumnsOnly)
          this.WriteUpdateMethod(sb);
        this.WriteDeleteMethod(sb);
      }
      else
      {
        this.WriteGetRecordCountMethod(sb);
        this.WriteSelectSkipAndTakeMethod(sb);
      }
      this.WriteGetSortExpressionMethod(sb);
    }

    internal string WriteMethodsForControllerBase()
    {
      StringBuilder sb = new StringBuilder();
      if (this._generateFrom == DatabaseObjectToGenerateFrom.Tables)
      {
        if (!this._table.IsContainsPrimaryAndForeignKeyColumnsOnly)
          this.WriteSelectByPrimaryKeyMethod(sb);
        this.WriteGetRecordCountMethod(sb);
        this.WriteGetRecordByCountMethods(sb);
        this.WriteGetRecordCountDynamicWhere(sb);
        this.WriteSelectSkipAndTakeMethod(sb);
        this.WriteSelectSkipAndTakeByMethods(sb);
        this.WriteSelectSkipAndTakeDynamicWhereMethod(sb);
        this.WriteSelectAllMethod(sb);
        this.WriteSelectAllWithSortExpressionMethod(sb);
        if (this._appVersion != ApplicationVersion.Express)
          this.WriteSelectAllDynamicWhereMethod(sb);
        this.WriteCollectionByMethods(sb);
        if (!this._table.IsContainsPrimaryAndForeignKeyColumnsOnly && this._table.PrimaryKeyCount == 1)
          this.WriteSelectDropDownListData(sb);
        this.WriteGetJsonCollectionMethod(sb);
      }
      else if (this._businessObjectBaseType == BusinessObjectBaseType.BusinessObjectBase)
      {
        this.WriteSelectAllMethod(sb);
        this.WriteSelectAllWithSortExpressionMethod(sb);
        this.WriteSortByExpressionMethod(sb);
      }
      if (this._generateFrom == DatabaseObjectToGenerateFrom.Tables)
        this.WriteGetSortExpressionMethod(sb);
      return sb.ToString();
    }

    private void WriteGetRecordCountMethod(StringBuilder sb)
    {
      StringBuilder stringBuilder = new StringBuilder();
      string str = "static ";
      if (this._businessObjectBaseType == BusinessObjectBaseType.MethodsForApiControllerBase)
      {
        str = string.Empty;
        stringBuilder.AppendLine("         /// <returns>Total number of records in the " + this._table.Name + " table</returns>");
        stringBuilder.AppendLine("         [HttpGet]");
      }
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Gets the total number of records in the " + this._table.Name + " table");
      sb.AppendLine("         /// </summary>");
      sb.Append(stringBuilder.ToString());
      sb.AppendLine("         public " + str + "int GetRecordCount()");
      sb.AppendLine("         {");
      if (this._businessObjectBaseType == BusinessObjectBaseType.MethodsForApiControllerBase)
        sb.AppendLine("             return " + this._businessObjectName + ".GetRecordCount();");
      else
        sb.AppendLine("             return " + this._table.Name + MyConstants.WordDataLayer + ".GetRecordCount();");
      sb.AppendLine("         }");
    }

    private void WriteGetRecordByCountMethods(StringBuilder sb)
    {
      string str1 = "static ";
      if (this._businessObjectBaseType == BusinessObjectBaseType.MethodsForApiControllerBase)
        str1 = string.Empty;
      foreach (Column foreignKeyColumn in this._table.ForeignKeyColumns)
      {
        StringBuilder stringBuilder = new StringBuilder();
        string str2 = foreignKeyColumn.NameCamelStyle;
        if (this._businessObjectBaseType == BusinessObjectBaseType.MethodsForApiControllerBase)
        {
          str2 = "id";
          stringBuilder.AppendLine("         /// <param name=\"id\">" + foreignKeyColumn.NameCamelStyle + "</param>");
          stringBuilder.AppendLine("         /// <returns>Total number of records in the " + this._table.Name + " table by " + foreignKeyColumn.NameCamelStyle + "</returns>");
          stringBuilder.AppendLine("         [HttpGet]");
        }
        sb.AppendLine("");
        sb.AppendLine("         /// <summary>");
        sb.AppendLine("         /// Gets the total number of records in the " + this._table.Name + " table by " + foreignKeyColumn.Name);
        sb.AppendLine("         /// </summary>");
        if (this._businessObjectBaseType == BusinessObjectBaseType.MethodsForApiControllerBase)
          sb.Append(stringBuilder.ToString());
        sb.AppendLine("         public " + str1 + "int GetRecordCountBy" + foreignKeyColumn.Name + "(" + foreignKeyColumn.SystemType + " " + str2 + ")");
        sb.AppendLine("         {");
        if (this._businessObjectBaseType == BusinessObjectBaseType.MethodsForApiControllerBase)
          sb.AppendLine("             return " + this._businessObjectName + ".GetRecordCountBy" + foreignKeyColumn.Name + "(" + str2 + ");");
        else
          sb.AppendLine("             return " + this._table.Name + MyConstants.WordDataLayer + ".GetRecordCountBy" + foreignKeyColumn.Name + "(" + str2 + ");");
        sb.AppendLine("         }");
      }
    }

    private void WriteGetRecordCountDynamicWhere(StringBuilder sb)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      string str1 = "static ";
      string str2 = "             return " + this._table.Name + "DataLayer.GetRecordCountDynamicWhere(" + Functions.GetCommaDelimitedParamsForPassingToSearchMethod(this._table) + ");";
      if (this._businessObjectBaseType == BusinessObjectBaseType.MethodsForApiControllerBase)
      {
        str1 = string.Empty;
        StringBuilder stringBuilder2 = new StringBuilder();
        foreach (Column spatialDataTypeColumn in this._table.NoneBinaryOrSpatialDataTypeColumns)
          stringBuilder2.AppendLine("         /// <param name=\"" + spatialDataTypeColumn.NameCamelStyle + "\">" + spatialDataTypeColumn.Name + "</param>");
        stringBuilder1.Append(stringBuilder2.ToString());
        stringBuilder1.AppendLine("         /// <returns>Total number of records in the " + this._table.Name + " table based on the search parameters</returns>");
        stringBuilder1.AppendLine("         [HttpGet]");
        str2 = "             return " + this._businessObjectName + ".GetRecordCountDynamicWhere(" + Functions.GetCommaDelimitedParamsForPassingToSearchMethod(this._table) + ");";
      }
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Gets the total number of records in the " + this._table.Name + " table based on search parameters");
      sb.AppendLine("         /// </summary>");
      sb.Append(stringBuilder1.ToString());
      sb.AppendLine("         public " + str1 + "int GetRecordCountDynamicWhere(" + Functions.GetCommaDelimitedParamsForSearchMethod(this._table, Language.CSharp) + ")");
      sb.AppendLine("         {");
      sb.AppendLine(str2);
      sb.AppendLine("         }");
    }

    private void WriteSelectSkipAndTakeMethod(StringBuilder sb)
    {
      string str1 = ", isIncludeRelatedProperties";
      if (!this._table.IsContainsForeignKeysWithTableSelected || this._generatedSqlType == GeneratedSqlType.StoredProcedures || this._generatedSqlType == GeneratedSqlType.AdHocSQL)
        str1 = string.Empty;
      string str2 = "static List<" + this._table.Name + ">";
      string str3 = "             return " + this._table.Name + MyConstants.WordDataLayer + ".SelectSkipAndTake(sortByExpression, startRowIndex, rows" + str1 + ");";
      if (this._businessObjectBaseType == BusinessObjectBaseType.MethodsForApiControllerBase)
      {
        str2 = "List<" + this._businessObjectName + ">";
        str3 = "             return " + this._businessObjectName + ".SelectSkipAndTake(rows, startRowIndex, sortByExpression" + str1 + ");";
      }
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Selects records as a collection (List) of " + this._table.Name + " sorted by the sortByExpression and returns the rows (# of records) starting from the startRowIndex");
      sb.AppendLine("         /// </summary>");
      if (this._table.IsContainsForeignKeysWithTableSelected)
        sb.AppendLine("         public " + str2 + " SelectSkipAndTake(int rows, int startRowIndex, string sortByExpression, bool isIncludeRelatedProperties = true)");
      else
        sb.AppendLine("         public " + str2 + " SelectSkipAndTake(int rows, int startRowIndex, string sortByExpression)");
      sb.AppendLine("         {");
      sb.AppendLine("             sortByExpression = GetSortExpression(sortByExpression);");
      sb.AppendLine(str3);
      sb.AppendLine("         }");
    }

    private void WriteSelectSkipAndTakeByMethods(StringBuilder sb)
    {
      foreach (Column foreignKeyColumn in this._table.ForeignKeyColumns)
      {
        if (this._businessObjectBaseType == BusinessObjectBaseType.MethodsForApiControllerBase)
          this.WriteSelectSkipAndTakeByMethodsWebApi(sb, foreignKeyColumn);
        else
          this.WriteSelectSkipAndTakeByMethods(sb, foreignKeyColumn);
      }
    }

    private void WriteSelectSkipAndTakeByMethods(StringBuilder sb, Column column)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      string str1 = "static List<" + this._table.Name + ">";
      string str2 = "             return " + this._table.Name + MyConstants.WordDataLayer + ".SelectSkipAndTakeBy" + column.Name + "(sortByExpression, startRowIndex, rows, " + column.NameCamelStyle + ");";
      if (this._businessObjectBaseType == BusinessObjectBaseType.MethodsForApiControllerBase)
      {
        str1 = "object";
        StringBuilder stringBuilder2 = new StringBuilder();
        stringBuilder1.AppendLine("         /// <param name=\"rows\">Number of rows to retrieve</param>");
        stringBuilder1.AppendLine("         /// <param name=\"startRowIndex\">Zero-based.  Row index where to start taking rows from</param>");
        stringBuilder1.AppendLine("         /// <param name=\"sortByExpression\">Field to sort and sort direction.  E.g. \"FieldName asc\" or \"FieldName desc\"</param>");
        stringBuilder1.AppendLine("         /// <param name=\"" + column.NameCamelStyle + "\">" + column.Name + "</param>");
        stringBuilder1.AppendLine("         /// <returns>Serialized " + this._table.Name + " collection in json format</returns>");
        stringBuilder1.AppendLine("         [HttpGet]");
        str2 = "             return GetJsonCollection(" + this._table.VariableObjCollectionName + ");";
      }
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Selects records by " + column.Name + " as a collection (List) of " + this._table.Name + " sorted by the sortByExpression starting from the startRowIndex");
      sb.AppendLine("         /// </summary>");
      sb.Append(stringBuilder1.ToString());
      sb.AppendLine("         public " + str1 + " SelectSkipAndTakeBy" + column.Name + "(int rows, int startRowIndex, string sortByExpression, " + column.SystemType + " " + column.NameCamelStyle + ")");
      sb.AppendLine("         {");
      if (this._businessObjectBaseType == BusinessObjectBaseType.MethodsForApiControllerBase)
      {
        sb.AppendLine("             sortByExpression = GetSortExpression(sortByExpression);");
        sb.AppendLine("             List<" + this._table.Name + "> " + this._table.VariableObjCollectionName + " = " + this._table.Name + ".SelectSkipAndTakeBy" + column.Name + "(rows, startRowIndex, sortByExpression, " + column.NameCamelStyle + ");");
      }
      else
        sb.AppendLine("             sortByExpression = GetSortExpression(sortByExpression);");
      sb.AppendLine(str2);
      sb.AppendLine("         }");
    }

    private void WriteSelectSkipAndTakeByMethodsWebApi(StringBuilder sb, Column column)
    {
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Selects records by " + column.Name + " as a collection (List) of " + this._table.Name + " sorted by the sortByExpression starting from the startRowIndex");
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         /// <param name=\"id\">" + column.NameWithSpaces + "</param>");
      sb.AppendLine("         /// <param name=\"sidx\">Column to sort</param>");
      sb.AppendLine("         /// <param name=\"sord\">Sort direction</param>");
      sb.AppendLine("         /// <param name=\"page\">Page of the grid to show</param>");
      sb.AppendLine("         /// <param name=\"rows\">Number of rows to retrieve</param>");
      sb.AppendLine("         /// <returns>Serialized " + this._table.Name + " collection in json format</returns>");
      sb.AppendLine("         [HttpGet]");
      sb.AppendLine("         public object SelectSkipAndTakeBy" + column.Name + "(" + column.SystemType + " id, string sidx, string sord, int _page, int rows)");
      sb.AppendLine("         {");
      sb.AppendLine("             string sortByExpression = GetSortExpression(sidx + \" \" + sord);");
      sb.AppendLine("             int startRowIndex = _page - 1;");
      sb.AppendLine("             List<" + this._table.Name + "> " + this._table.VariableObjCollectionName + " = " + this._table.Name + ".SelectSkipAndTakeBy" + column.Name + "(rows, startRowIndex, sortByExpression, id);");
      sb.AppendLine("             int totalRecords = " + this._table.Name + ".GetRecordCountBy" + column.Name + "(id);");
      sb.AppendLine("             return GetJsonCollection(" + this._table.VariableObjCollectionName + ", totalRecords, _page, rows);");
      sb.AppendLine("         }");
    }

    private void WriteSelectSkipAndTakeDynamicWhereMethod(StringBuilder sb)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      string str1 = "static List<" + this._table.Name + ">";
      string str2 = string.Empty;
      string passingToSearchMethod = Functions.GetCommaDelimitedParamsForPassingToSearchMethod(this._table);
      if (this._businessObjectBaseType == BusinessObjectBaseType.MethodsForApiControllerBase)
      {
        str1 = "object";
        str2 = ", int _page";
        StringBuilder stringBuilder2 = new StringBuilder();
        foreach (Column spatialDataTypeColumn in this._table.NoneBinaryOrSpatialDataTypeColumns)
          stringBuilder2.AppendLine("         /// <param name=\"" + spatialDataTypeColumn.NameCamelStyle + "\">" + spatialDataTypeColumn.Name + "</param>");
        stringBuilder1.Append(stringBuilder2.ToString());
        stringBuilder1.AppendLine("         /// <param name=\"rows\">Number of rows to retrieve</param>");
        stringBuilder1.AppendLine("         /// <param name=\"startRowIndex\">Zero-based.  Row index where to start taking rows from</param>");
        stringBuilder1.AppendLine("         /// <param name=\"sortByExpression\">Field to sort and sort direction.  E.g. \"FieldName asc\" or \"FieldName desc\"</param>");
        if (this._businessObjectBaseType == BusinessObjectBaseType.MethodsForApiControllerBase)
          stringBuilder1.AppendLine("         /// <param name=\"page\">Page of the grid to show</param>");
        stringBuilder1.AppendLine("         /// <returns>Serialized " + this._table.Name + " collection in json format</returns>");
        stringBuilder1.AppendLine("         [HttpGet]");
      }
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Selects records as a collection (List) of " + this._table.Name + " sorted by the sortByExpression starting from the startRowIndex, based on the search parameters");
      sb.AppendLine("         /// </summary>");
      sb.Append(stringBuilder1.ToString());
      sb.AppendLine("         public " + str1 + " SelectSkipAndTakeDynamicWhere(" + Functions.GetCommaDelimitedParamsForSearchMethod(this._table, Language.CSharp) + ", int rows, int startRowIndex, string sortByExpression" + str2 + ")");
      sb.AppendLine("         {");
      sb.AppendLine("             sortByExpression = GetSortExpression(sortByExpression);");
      if (this._businessObjectBaseType == BusinessObjectBaseType.MethodsForApiControllerBase)
      {
        sb.AppendLine("             List<" + this._businessObjectName + "> " + this._table.VariableObjCollectionName + " = " + this._businessObjectName + ".SelectSkipAndTakeDynamicWhere(" + passingToSearchMethod + ", rows, startRowIndex, sortByExpression);");
        sb.AppendLine("             int totalRecords = " + this._businessObjectName + ".GetRecordCountDynamicWhere(" + passingToSearchMethod + ");");
        sb.AppendLine("             return GetJsonCollection(" + this._table.VariableObjCollectionName + ", totalRecords, _page, rows);");
      }
      else
        sb.AppendLine("             return " + this._table.Name + MyConstants.WordDataLayer + ".SelectSkipAndTakeDynamicWhere(" + passingToSearchMethod + ", sortByExpression, startRowIndex, rows);");
      sb.AppendLine("         }");
    }

    private void WriteSelectByPrimaryKeyMethod(StringBuilder sb)
    {
      string str1 = "object";
      string str2 = "static " + this._table.Name;
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Selects a record by primary key(s)");
      sb.AppendLine("         /// </summary>");
      if (this._businessObjectBaseType == BusinessObjectBaseType.BusinessObjectBase)
      {
        sb.AppendLine("         public " + str2 + " SelectByPrimaryKey(" + Functions.GetCommaDelimitedPrimaryKeysAndType(this._table, Language.CSharp) + ")");
        sb.AppendLine("         {");
        sb.AppendLine("             return " + this._table.Name + "DataLayer.SelectByPrimaryKey(" + Functions.GetCommaDelimitedPrimaryKeys(this._table, Language.CSharp, true, false, false) + ");");
        sb.AppendLine("         }");
      }
      else
      {
        StringBuilder stringBuilder1 = new StringBuilder();
        StringBuilder stringBuilder2 = new StringBuilder();
        string str3 = "id";
        string str4 = this._table.FirstPrimaryKeySystemType + " id";
        if (this._table.PrimaryKeyCount > 1)
        {
          str3 = Functions.GetCommaDelimitedPrimaryKeys(this._table, Language.CSharp, true, false, false);
          str4 = Functions.GetCommaDelimitedPrimaryKeysAndType(this._table, Language.CSharp);
          foreach (Column primaryKeyColumn in (List<Column>) this._table.PrimaryKeyColumns)
            stringBuilder2.AppendLine("         /// <param name=\"" + primaryKeyColumn.NameCamelStyle + "\">" + primaryKeyColumn.Name + "</param>");
        }
        else
          stringBuilder2.AppendLine("         /// <param name=\"id\">" + this._table.FirstPrimaryKeyName + "</param>");
        foreach (Column column in (List<Column>) this._table.Columns)
        {
          string str5 = string.Empty;
          if (column.SystemTypeNative.ToLower() == "datetime")
          {
            if (column.IsNullable)
              str5 = ".HasValue ? " + this._table.VariableObjName + "." + column.Name + ".Value.ToString(\"d\") : null";
            else
              str5 = ".ToString(\"d\")";
          }
          stringBuilder1.AppendLine("                 " + column.Name + " = " + this._table.VariableObjName + "." + column.Name + str5 + ",");
        }
        sb.Append(stringBuilder2.ToString());
        sb.AppendLine("         /// <returns>One serialized " + this._table.Name + " record in json format</returns>");
        sb.AppendLine("         [HttpGet]");
        sb.AppendLine("         public " + str1 + " SelectByPrimaryKey(" + str4 + ")");
        sb.AppendLine("         {");
        sb.AppendLine("             " + this._businessObjectName + " " + this._table.VariableObjName + " = " + this._businessObjectName + ".SelectByPrimaryKey(" + str3 + ");");
        sb.AppendLine("");
        sb.AppendLine("             var jsonData = new");
        sb.AppendLine("             {");
        sb.Append(Functions.RemoveLastComma(stringBuilder1.ToString()));
        sb.AppendLine("");
        sb.AppendLine("             };");
        sb.AppendLine("");
        sb.AppendLine("             return jsonData;");
        sb.AppendLine("         }");
      }
    }

    private void WriteSelectAllMethod(StringBuilder sb)
    {
      string str = "static List<" + this._table.Name + ">";
      if (this._businessObjectBaseType == BusinessObjectBaseType.MethodsForApiControllerBase)
        str = "object";
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Selects all records as a collection (List) of " + this._table.Name);
      sb.AppendLine("         /// </summary>");
      if (this._businessObjectBaseType == BusinessObjectBaseType.MethodsForApiControllerBase)
      {
        sb.AppendLine("         /// <returns>Serialized " + this._businessObjectName + " collection in json format</returns>");
        sb.AppendLine("         [HttpGet]");
      }
      sb.AppendLine("         public " + str + " SelectAll()");
      sb.AppendLine("         {");
      if (this._businessObjectBaseType == BusinessObjectBaseType.MethodsForApiControllerBase)
      {
        sb.AppendLine("             List<" + this._businessObjectName + "> " + this._table.VariableObjCollectionName + " = " + this._businessObjectName + ".SelectAll();");
        sb.AppendLine("             return GetJsonCollection(" + this._table.VariableObjCollectionName + ", " + this._table.VariableObjCollectionName + ".Count, 1, " + this._table.VariableObjCollectionName + ".Count);");
      }
      else
        sb.AppendLine("             return " + this._table.Name + "DataLayer.SelectAll();");
      sb.AppendLine("         }");
    }

    private void WriteSelectAllWithSortExpressionMethod(StringBuilder sb)
    {
      string str = "static List<" + this._table.Name + ">";
      if (this._businessObjectBaseType == BusinessObjectBaseType.MethodsForApiControllerBase)
        str = "object";
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Selects all records as a collection (List) of " + this._table.Name + " sorted by the sort expression");
      sb.AppendLine("         /// </summary>");
      if (this._businessObjectBaseType == BusinessObjectBaseType.MethodsForApiControllerBase)
      {
        sb.AppendLine("         /// <param name=\"sortByExpression\">Field to sort and sort direction.  E.g. \"FieldName asc\" or \"FieldName desc\"</param>");
        sb.AppendLine("         /// <returns>Serialized " + this._businessObjectName + " collection in json format</returns>");
        sb.AppendLine("         [HttpGet]");
      }
      sb.AppendLine("         public " + str + " SelectAll(string sortByExpression)");
      sb.AppendLine("         {");
      if (this._businessObjectBaseType == BusinessObjectBaseType.MethodsForApiControllerBase)
      {
        sb.AppendLine("             sortByExpression = GetSortExpression(sortByExpression);");
        sb.AppendLine("             List<" + this._businessObjectName + "> " + this._table.VariableObjCollectionName + " = " + this._businessObjectName + ".SelectAll(sortByExpression);");
        sb.AppendLine("             return GetJsonCollection(" + this._table.VariableObjCollectionName + ", " + this._table.VariableObjCollectionName + ".Count, 1, " + this._table.VariableObjCollectionName + ".Count);");
      }
      else
      {
        sb.AppendLine("             List<" + this._table.Name + "> " + this._table.VariableObjCollectionName + " = " + this._table.Name + MyConstants.WordDataLayer + ".SelectAll();");
        sb.AppendLine("             return SortByExpression(" + this._table.VariableObjCollectionName + ", sortByExpression);");
      }
      sb.AppendLine("         }");
    }

    private void WriteSelectAllDynamicWhereMethod(StringBuilder sb)
    {
      sb.AppendLine("");
      StringBuilder stringBuilder1 = new StringBuilder();
      string str = "static List<" + this._table.Name + ">";
      string passingToSearchMethod = Functions.GetCommaDelimitedParamsForPassingToSearchMethod(this._table);
      if (this._businessObjectBaseType == BusinessObjectBaseType.MethodsForApiControllerBase)
      {
        str = "object";
        StringBuilder stringBuilder2 = new StringBuilder();
        foreach (Column spatialDataTypeColumn in this._table.NoneBinaryOrSpatialDataTypeColumns)
          stringBuilder2.AppendLine("         /// <param name=\"" + spatialDataTypeColumn.NameCamelStyle + "\">" + spatialDataTypeColumn.Name + "</param>");
        stringBuilder1.Append(stringBuilder2.ToString());
        stringBuilder1.AppendLine("         /// <returns>Serialized " + this._table.Name + " collection in json format</returns>");
        stringBuilder1.AppendLine("         [HttpGet]");
      }
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Selects records based on the passed filters as a collection (List) of " + this._table.Name + ".");
      sb.AppendLine("         /// </summary>");
      sb.Append(stringBuilder1.ToString());
      sb.AppendLine("         public " + str + " SelectAllDynamicWhere(" + Functions.GetCommaDelimitedParamsForSearchMethod(this._table, Language.CSharp) + ")");
      sb.AppendLine("         {");
      if (this._businessObjectBaseType == BusinessObjectBaseType.MethodsForApiControllerBase)
      {
        sb.AppendLine("             List<" + this._businessObjectName + "> " + this._table.VariableObjCollectionName + " = " + this._businessObjectName + ".SelectAllDynamicWhere(" + passingToSearchMethod + ");");
        sb.AppendLine("             return GetJsonCollection(" + this._table.VariableObjCollectionName + ", " + this._table.VariableObjCollectionName + ".Count, 1, " + this._table.VariableObjCollectionName + ".Count);");
      }
      else
        sb.AppendLine("             return " + this._table.Name + "DataLayer.SelectAllDynamicWhere(" + passingToSearchMethod + ");");
      sb.AppendLine("         }");
    }

    private void WriteSelectAllDynamicWhereWithSortExpressionMethod(StringBuilder sb)
    {
      sb.AppendLine("");
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = new StringBuilder();
      foreach (Column column in (List<Column>) this._table.Columns)
      {
        if (!column.IsBinaryOrSpatialDataType)
        {
          string str = "?";
          if (column.SystemType == "string")
            str = "";
          stringBuilder1.Append(column.SystemType + str + " " + column.NameCamelStyle + ", ");
          stringBuilder2.Append(column.NameCamelStyle + ", ");
        }
      }
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Selects records based on the passed filters as a collection (List) of " + this._table.Name + " sorted by the sort expression.");
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         public static List<" + this._table.Name + "> SelectAllDynamicWhere(" + stringBuilder1.ToString() + "string sortByExpression)");
      sb.AppendLine("         {");
      sb.AppendLine("             List<" + this._table.Name + "> " + this._table.VariableObjCollectionName + " = " + this._table.Name + "DataLayer.SelectAllDynamicWhere(" + Functions.RemoveLastComma(stringBuilder2.ToString()) + ");");
      sb.AppendLine("             return SortByExpression(" + this._table.VariableObjCollectionName + ", sortByExpression);");
      sb.AppendLine("         }");
    }

    private void WriteCollectionByMethods(StringBuilder sb)
    {
      foreach (Column foreignKeyColumn in this._table.ForeignKeyColumns)
      {
        StringBuilder stringBuilder1 = new StringBuilder();
        StringBuilder stringBuilder2 = new StringBuilder();
        string str1 = "static List<" + this._table.Name + ">";
        string str2 = foreignKeyColumn.ForeignKeyColumnNameCamelStyle;
        if (this._businessObjectBaseType == BusinessObjectBaseType.MethodsForApiControllerBase)
        {
          str1 = "object";
          str2 = "id";
          stringBuilder1.AppendLine("         /// <param name=\"id\">" + foreignKeyColumn.ForeignKeyColumnNameCamelStyle + "</param>");
          stringBuilder1.AppendLine("         /// <param name=\"sidx\">Column to sort</param>");
          stringBuilder1.AppendLine("         /// <param name=\"sord\">Sort direction</param>");
          stringBuilder1.AppendLine("         /// <param name=\"page\">Page of the grid to show</param>");
          stringBuilder1.AppendLine("         /// <param name=\"rows\">Number of rows to retrieve</param>");
          stringBuilder1.AppendLine("         /// <returns>Total number of records in the " + this._table.Name + " table by " + foreignKeyColumn.NameCamelStyle + "</returns>");
          stringBuilder1.AppendLine("         [HttpGet]");
          stringBuilder2.AppendLine("         /// <param name=\"id\">" + foreignKeyColumn.ForeignKeyColumnNameCamelStyle + "</param>");
          stringBuilder2.AppendLine("         /// <param name=\"sortByExpression\">Field to sort and sort direction.  E.g. \"FieldName asc\" or \"FieldName desc\"</param>");
          stringBuilder2.AppendLine("         /// <returns>Total number of records in the " + this._table.Name + " table by " + foreignKeyColumn.NameCamelStyle + "</returns>");
          stringBuilder2.AppendLine("         [HttpGet]");
        }
        sb.AppendLine("");
        sb.AppendLine("         /// <summary>");
        sb.AppendLine("         /// Selects all " + this._table.Name + " by " + foreignKeyColumn.ForeignKeyTableName + ", related to column " + foreignKeyColumn.Name ?? "");
        sb.AppendLine("         /// </summary>");
        sb.Append(stringBuilder1.ToString());
        if (this._businessObjectBaseType == BusinessObjectBaseType.MethodsForApiControllerBase)
          sb.AppendLine("         public " + str1 + " Select" + this._table.Name + "CollectionBy" + foreignKeyColumn.Name + "(" + foreignKeyColumn.ForeignKeySystemType + " " + str2 + ", string sidx, string sord, int _page, int rows)");
        else
          sb.AppendLine("         public " + str1 + " Select" + this._table.Name + "CollectionBy" + foreignKeyColumn.Name + "(" + foreignKeyColumn.ForeignKeySystemType + " " + str2 + ")");
        sb.AppendLine("         {");
        if (this._businessObjectBaseType == BusinessObjectBaseType.MethodsForApiControllerBase)
        {
          sb.AppendLine("             string sortByExpression = GetSortExpression(sidx + \" \" + sord);");
          sb.AppendLine("             int startRowIndex = _page;");
          sb.AppendLine("             List<" + this._table.Name + "> " + this._table.VariableObjCollectionName + " = " + this._table.NameFullyQualifiedBusinessObject + ".SelectSkipAndTakeBy" + foreignKeyColumn.Name + "(rows, startRowIndex, sortByExpression, id);");
          sb.AppendLine("             int totalRecords = " + this._table.NameFullyQualifiedBusinessObject + ".GetRecordCountBy" + foreignKeyColumn.Name + "(id);");
          sb.AppendLine("             return GetJsonCollection(" + this._table.VariableObjCollectionName + ", totalRecords, _page, rows);");
        }
        else
          sb.AppendLine("             return " + this._table.Name + "DataLayer.Select" + this._table.Name + "CollectionBy" + foreignKeyColumn.Name + "(" + foreignKeyColumn.ForeignKeyColumnNameCamelStyle + ");");
        sb.AppendLine("         }");
        if (this._businessObjectBaseType != BusinessObjectBaseType.MethodsForApiControllerBase)
        {
          sb.AppendLine("");
          sb.AppendLine("         /// <summary>");
          sb.AppendLine("         /// Selects all " + this._table.Name + " by " + foreignKeyColumn.ForeignKeyTableName + ", related to column " + foreignKeyColumn.Name + ", sorted by the sort expression");
          sb.AppendLine("         /// </summary>");
          sb.Append(stringBuilder2.ToString());
          sb.AppendLine("         public " + str1 + " Select" + this._table.Name + "CollectionBy" + foreignKeyColumn.Name + "(" + foreignKeyColumn.ForeignKeySystemType + " " + str2 + ", string sortByExpression)");
          sb.AppendLine("         {");
          sb.AppendLine("             List<" + this._table.Name + "> " + this._table.VariableObjCollectionName + " = " + this._table.Name + "DataLayer.Select" + this._table.Name + "CollectionBy" + foreignKeyColumn.Name + "(" + foreignKeyColumn.ForeignKeyColumnNameCamelStyle + ");");
          sb.AppendLine("             return SortByExpression(" + this._table.VariableObjCollectionName + ", sortByExpression);");
          sb.AppendLine("         }");
        }
      }
    }

    private void WriteSelectDropDownListData(StringBuilder sb)
    {
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Selects " + this._table.FirstPrimaryKeyName + " and " + this._table.DataTextField + " columns for use with a DropDownList web control, ComboBox, CheckedBoxList, ListView, ListBox, etc");
      sb.AppendLine("         /// </summary>");
      if (this._businessObjectBaseType == BusinessObjectBaseType.BusinessObjectBase)
      {
        sb.AppendLine("         public static List<" + this._table.Name + "> Select" + this._table.Name + MyConstants.WordDropDownListData + "()");
        sb.AppendLine("         {");
        sb.AppendLine("             return " + this._table.Name + "DataLayer.Select" + this._table.Name + MyConstants.WordDropDownListData + "();");
        sb.AppendLine("         }");
      }
      else
      {
        sb.AppendLine("         /// <returns>Serialized " + this._table.Name + " collection in json format</returns>");
        sb.AppendLine("         [HttpGet]");
        sb.AppendLine("         public object Select" + this._table.Name + MyConstants.WordDropDownListData + "()");
        sb.AppendLine("         {");
        sb.AppendLine("             List<" + this._businessObjectName + "> " + this._table.VariableObjCollectionName + " = " + this._businessObjectName + ".Select" + this._table.Name + MyConstants.WordDropDownListData + "();");
        sb.AppendLine("");
        sb.AppendLine("             if(" + this._table.VariableObjCollectionName + " != null)");
        sb.AppendLine("             {");
        sb.AppendLine("                 var jsonData = (from " + this._table.VariableObjName + " in " + this._table.VariableObjCollectionName);
        sb.AppendLine("                     select new");
        sb.AppendLine("                     {");
        sb.AppendLine("                         " + this._table.FirstPrimaryKeyName + " = " + this._table.VariableObjName + "." + this._table.FirstPrimaryKeyName + ",");
        sb.AppendLine("                         " + this._table.DataTextField + " = " + this._table.VariableObjName + "." + this._table.DataTextField);
        sb.AppendLine("                     }).ToArray();");
        sb.AppendLine("");
        sb.AppendLine("                 return jsonData;");
        sb.AppendLine("             }");
        sb.AppendLine("");
        sb.AppendLine("             return null;");
        sb.AppendLine("         }");
      }
    }

    private void WriteSortByExpressionMethod(StringBuilder sb)
    {
      IEnumerable<Column> columns = this._table.Columns.Where<Column>((Func<Column, bool>) (c => c.SystemType != "System.Xml.Linq.XElement"));
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Sorts the List<" + this._table.Name + " >by sort expression");
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         public static List<" + this._table.Name + "> SortByExpression(List<" + this._table.Name + "> " + this._table.VariableObjCollectionName + ", string sortExpression)");
      sb.AppendLine("         {");
      sb.AppendLine("             bool isSortDescending = sortExpression.ToLower().Contains(\" desc\");");
      sb.AppendLine("");
      sb.AppendLine("             if (isSortDescending)");
      sb.AppendLine("             {");
      sb.AppendLine("                 sortExpression = sortExpression.Replace(\" DESC\", \"\");");
      sb.AppendLine("                 sortExpression = sortExpression.Replace(\" desc\", \"\");");
      sb.AppendLine("             }");
      sb.AppendLine("             else");
      sb.AppendLine("             {");
      sb.AppendLine("                 sortExpression = sortExpression.Replace(\" ASC\", \"\");");
      sb.AppendLine("                 sortExpression = sortExpression.Replace(\" asc\", \"\");");
      sb.AppendLine("             }");
      sb.AppendLine("");
      sb.AppendLine("             switch (sortExpression)");
      sb.AppendLine("             {");
      foreach (Column column in columns)
      {
        sb.AppendLine("                 case \"" + column.Name + "\":");
        sb.AppendLine("                     " + this._table.VariableObjCollectionName + ".Sort(" + this._apiName + "." + MyConstants.WordBusinessObject + "." + this._table.Name + ".By" + column.Name + ");");
        sb.AppendLine("                     break;");
      }
      sb.AppendLine("                 default:");
      sb.AppendLine("                     break;");
      sb.AppendLine("             }");
      sb.AppendLine("");
      sb.AppendLine("             if (isSortDescending)");
      sb.AppendLine("                 " + this._table.VariableObjCollectionName + ".Reverse();");
      sb.AppendLine("");
      sb.AppendLine("             return " + this._table.VariableObjCollectionName + ";");
      sb.AppendLine("         }");
    }

    private void WriteInsertMethod(StringBuilder sb)
    {
      string methodReturnType = Functions.GetInsertMethodReturnType(this._table, Language.CSharp);
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Inserts a record");
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         public " + methodReturnType + " Insert()");
      sb.AppendLine("         {");
      sb.AppendLine("             " + this._table.Name + " " + this._table.VariableObjName + " = (" + this._table.Name + ")this;");
      if (methodReturnType == "void")
        sb.AppendLine("             " + this._table.Name + "DataLayer.Insert(obj" + this._table.Name + ");");
      else
        sb.AppendLine("             return " + this._table.Name + "DataLayer.Insert(obj" + this._table.Name + ");");
      sb.AppendLine("         }");
    }

    private void WriteUpdateMethod(StringBuilder sb)
    {
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Updates a record");
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         public void Update()");
      sb.AppendLine("         {");
      sb.AppendLine("             " + this._table.Name + " " + this._table.VariableObjName + " = (" + this._table.Name + ")this;");
      sb.AppendLine("             " + this._table.Name + "DataLayer.Update(" + this._table.VariableObjName + ");");
      sb.AppendLine("         }");
    }

    private void WriteDeleteMethod(StringBuilder sb)
    {
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Deletes a record based on primary key(s)");
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         public static void Delete(" + Functions.GetCommaDelimitedPrimaryKeysAndType(this._table, Language.CSharp) + ")");
      sb.AppendLine("         {");
      sb.AppendLine("             " + this._table.Name + "DataLayer.Delete(" + Functions.GetCommaDelimitedPrimaryKeys(this._table, Language.CSharp, true, false, false) + ");");
      sb.AppendLine("         }");
    }

    private void WriteComparison(StringBuilder sb)
    {
      foreach (Column column in (List<Column>) this._table.Columns)
      {
        if (column.SystemType != "System.Xml.Linq.XElement")
        {
          sb.AppendLine("");
          sb.AppendLine("         /// <summary>");
          sb.AppendLine("         /// Compares " + column.Name + " used for sorting");
          sb.AppendLine("         /// </summary>");
          sb.AppendLine("         public static Comparison<" + this._table.Name + "> By" + column.Name + " = delegate(" + this._table.Name + " x, " + this._table.Name + " y)");
          sb.AppendLine("         {");
          if (this._generatedSqlType == GeneratedSqlType.EFCore && column.SQLDataType == SQLType.uniqueidentifier)
          {
            if (column.SystemType.ToLower() == "string")
            {
              if (column.IsNullable)
              {
                sb.AppendLine("             string value1 = (!x." + column.Name + ".HasValue || x." + column.Name + " == Guid.Empty ? String.Empty : x." + column.Name + ".ToString()) ?? String.Empty;");
                sb.AppendLine("             string value2 = (!y." + column.Name + ".HasValue || y." + column.Name + " == Guid.Empty ? String.Empty : y." + column.Name + ".ToString()) ?? String.Empty;");
                sb.AppendLine("             return value1.CompareTo(value2);");
              }
              else
              {
                sb.AppendLine("             string value1 = x." + column.Name + ".ToString() ?? String.Empty;");
                sb.AppendLine("             string value2 = y." + column.Name + ".ToString() ?? String.Empty;");
                sb.AppendLine("             return value1.CompareTo(value2);");
              }
            }
            else if (column.IsNullable)
            {
              if (column.SystemType.ToLower() == "bool")
                sb.AppendLine("             return x." + column.Name + ".CompareTo(y." + column.Name + ");");
              else
                sb.AppendLine("             return Nullable.Compare(x." + column.Name + ", y." + column.Name + ");");
            }
            else
              sb.AppendLine("             return x." + column.Name + ".CompareTo(y." + column.Name + ");");
          }
          else if (column.SystemType.ToLower() == "string")
          {
            sb.AppendLine("             string value1 = x." + column.Name + " ?? String.Empty;");
            sb.AppendLine("             string value2 = y." + column.Name + " ?? String.Empty;");
            sb.AppendLine("             return value1.CompareTo(value2);");
          }
          else if (column.IsNullable)
          {
            if (column.SystemType.ToLower() == "bool")
              sb.AppendLine("             return x." + column.Name + ".CompareTo(y." + column.Name + ");");
            else
              sb.AppendLine("             return Nullable.Compare(x." + column.Name + ", y." + column.Name + ");");
          }
          else
            sb.AppendLine("             return x." + column.Name + ".CompareTo(y." + column.Name + ");");
          sb.AppendLine("         };");
        }
      }
      sb.AppendLine("");
    }

    private void WriteGetSortExpressionMethod(StringBuilder sb)
    {
      string str1 = "static ";
      string str2 = this._table.FirstPrimaryKeyName;
      if (string.IsNullOrEmpty(str2))
        str2 = this._table.Columns[0].Name;
      if (this._businessObjectBaseType == BusinessObjectBaseType.MethodsForApiControllerBase)
        str1 = string.Empty;
      sb.AppendLine("");
      sb.AppendLine("         private " + str1 + "string GetSortExpression(string sortByExpression)");
      sb.AppendLine("         {");
      sb.AppendLine("             if (String.IsNullOrEmpty(sortByExpression) || sortByExpression == \" asc\")");
      sb.AppendLine("                 sortByExpression = \"" + str2 + "\";");
      sb.AppendLine("             else if (sortByExpression.Contains(\" asc\"))");
      sb.AppendLine("                 sortByExpression = sortByExpression.Replace(\" asc\", \"\");");
      sb.AppendLine("");
      sb.AppendLine("             return sortByExpression;");
      sb.AppendLine("         }");
    }

    private void WriteGetJsonCollectionMethod(StringBuilder sb)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = new StringBuilder();
      stringBuilder1.AppendLine("             var jsonData = new");
      stringBuilder1.AppendLine("             {");
      stringBuilder1.AppendLine("                 total = totalPages,");
      stringBuilder1.AppendLine("                 _page,");
      stringBuilder1.AppendLine("                 records = totalRecords,");
      stringBuilder1.AppendLine("                 rows = (");
      stringBuilder1.AppendLine("                     from " + this._table.VariableObjName + " in " + this._table.VariableObjCollectionName);
      stringBuilder1.AppendLine("                     select new");
      stringBuilder1.AppendLine("                     {");
      if (this._table.PrimaryKeyCount > 1)
      {
        int num = 1;
        string str1 = string.Empty;
        string str2 = string.Empty;
        foreach (Column primaryKeyColumn in (List<Column>) this._table.PrimaryKeyColumns)
        {
          if (primaryKeyColumn.SystemType.ToLower() != "string")
            str2 = ".ToString()";
          str1 = str1 + this._table.VariableObjName + "." + primaryKeyColumn.Name + str2;
          if (num < this._table.PrimaryKeyCount)
            str1 += " + ";
          ++num;
        }
        if (this._generateFrom == DatabaseObjectToGenerateFrom.Tables)
          stringBuilder1.AppendLine("                         id = " + str1 + ",");
      }
      else if (this._generateFrom == DatabaseObjectToGenerateFrom.Tables)
        stringBuilder1.AppendLine("                         id = " + this._table.VariableObjName + "." + this._table.FirstPrimaryKeyName + ",");
      stringBuilder1.AppendLine("                         cell = new string[] { ");
      foreach (Column column in (List<Column>) this._table.Columns)
      {
        if (this._generatedSqlType == GeneratedSqlType.EFCore && column.SQLDataType == SQLType.uniqueidentifier)
        {
          if (column.IsNullable && column.IsForeignKey && column.IsStringField)
          {
            if (column.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK)
              stringBuilder2.AppendLine("                             !" + this._table.VariableObjName + "." + column.Name + ".HasValue || " + this._table.VariableObjName + "." + column.Name + " == Guid.Empty ? String.Empty : " + this._table.VariableObjName + "." + column.Name + ".Value.ToString(),");
            else
              stringBuilder2.AppendLine("                             " + this._table.VariableObjName + "." + column.Name + ".ToString(),");
          }
          else
            stringBuilder2.AppendLine("                             " + this._table.VariableObjName + "." + column.Name + ".ToString(),");
        }
        else
        {
          string str = string.Empty;
          if (column.IsNullable && column.IsForeignKey && column.IsStringField)
          {
            if (column.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK)
              stringBuilder2.AppendLine("                             !String.IsNullOrEmpty(" + this._table.VariableObjName + "." + column.Name + ") ? " + this._table.VariableObjName + "." + column.Name + " : \"\",");
            else
              stringBuilder2.AppendLine("                             " + this._table.VariableObjName + "." + column.Name + str + ",");
          }
          else
          {
            if (column.SystemTypeNative.ToLower() == "datetime")
            {
              if (column.IsNullable)
                str = ".HasValue ? " + this._table.VariableObjName + "." + column.Name + ".Value.ToString(\"d\") : \"\"";
              else
                str = ".ToString(\"d\")";
            }
            else if (column.IsNullable && column.SQLDataType != SQLType.bit && column.SystemTypeNative.ToLower() != "string")
            {
              if (column.SQLDataType == SQLType.xml)
                str = " != null ? " + this._table.VariableObjName + "." + column.Name + ".ToString(System.Xml.Linq.SaveOptions.DisableFormatting) : \"\"";
              else
                str = ".HasValue ? " + this._table.VariableObjName + "." + column.Name + ".Value.ToString() : \"\"";
            }
            else if (column.SystemTypeNative.ToLower() != "string")
              str = ".ToString()";
            stringBuilder2.AppendLine("                             " + this._table.VariableObjName + "." + column.Name + str + ",");
          }
        }
      }
      stringBuilder1.Append(Functions.RemoveLastComma(stringBuilder2.ToString()));
      stringBuilder1.AppendLine("");
      stringBuilder1.AppendLine("                         }");
      stringBuilder1.AppendLine("                     }).ToArray()");
      stringBuilder1.AppendLine("             };");
      sb.AppendLine("");
      sb.AppendLine("         private object GetJsonCollection(List<" + this._businessObjectName + "> " + this._table.VariableObjCollectionName + ", int totalRecords, int _page, int rows)");
      sb.AppendLine("         {");
      sb.AppendLine("             if (" + this._table.VariableObjCollectionName + " is null)");
      sb.AppendLine("                 return null;");
      sb.AppendLine("");
      sb.AppendLine("             int totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);");
      sb.AppendLine("");
      sb.Append(stringBuilder1.ToString());
      sb.AppendLine("");
      sb.AppendLine("             return jsonData;");
      sb.AppendLine("         }");
    }
  }
}
