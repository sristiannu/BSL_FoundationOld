
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal sealed class DataLayerBaseClassic
  {
    private Table _table;
    private Tables _selectedTables;
    private DataTable _referencedTables;
    private const string _fileExtension = ".cs";
    private string _nameSpace;
    private string _directory;
    private const Language _language = Language.CSharp;
    private bool _isUseStoredProcedure;
    private int _spPrefixSuffixIndex;
    private string _storedProcPrefix;
    private string _storedProcSuffix;
    private DatabaseObjectToGenerateFrom _generateFrom;
    private ApplicationVersion _appVersion;
    private string _commandTypeCode;

    private DataLayerBaseClassic()
    {
    }

    internal DataLayerBaseClassic(Table table, Tables selectedTables, DataTable referencedTables, string nameSpace, string path, bool isUseStoredProcedure, int spPrefixSuffixIndex, string storedProcPrefix, string storedProcSuffix, DatabaseObjectToGenerateFrom generateFrom, ApplicationVersion appVersion)
    {
      this._table = table;
      this._selectedTables = selectedTables;
      this._referencedTables = referencedTables;
      this._nameSpace = nameSpace;
      this._directory = path + MyConstants.DirectoryDataLayerBase;
      this._isUseStoredProcedure = isUseStoredProcedure;
      this._spPrefixSuffixIndex = spPrefixSuffixIndex;
      this._storedProcPrefix = storedProcPrefix;
      this._storedProcSuffix = storedProcSuffix;
      this._generateFrom = generateFrom;
      this._appVersion = appVersion;
      this._commandTypeCode = !this._isUseStoredProcedure ? "command.CommandType = CommandType.Text;" : "command.CommandType = CommandType.StoredProcedure;";
      this.Generate();
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._directory + this._table.Name + MyConstants.WordDataLayerBase + ".cs"))
      {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("using System;");
        sb.AppendLine("using System.Data;");
        sb.AppendLine("using System.Data.SqlClient;");
        sb.AppendLine("using " + this._nameSpace + ".BusinessObject;");
        sb.AppendLine("using System.Collections.Generic;");
        sb.AppendLine("");
        sb.AppendLine("namespace " + this._nameSpace + ".DataLayer.Base");
        sb.AppendLine("{");
        sb.AppendLine("     /// <summary>");
        sb.AppendLine("     /// Base class for " + this._table.Name + "DataLayer.  Do not make changes to this class,");
        sb.AppendLine("     /// instead, put additional code in the " + this._table.Name + "DataLayer class");
        sb.AppendLine("     /// </summary>");
        sb.AppendLine("     internal class " + this._table.Name + MyConstants.WordDataLayerBase ?? "");
        sb.AppendLine("     {");
        this.WriteConstructor(sb);
        if (this._appVersion == ApplicationVersion.ProfessionalPlus)
          this.WriteMethods(sb);
        else
          this.WriteMethodsExpress(sb);
        sb.AppendLine("     }");
        sb.AppendLine("}");
        streamWriter.Write(sb.ToString());
      }
    }

    private void WriteConstructor(StringBuilder sb)
    {
      sb.AppendLine("         // constructor");
      sb.AppendLine("         internal " + this._table.Name + MyConstants.WordDataLayerBase + "()");
      sb.AppendLine("         {");
      sb.AppendLine("         }");
    }

    private void WriteMethods(StringBuilder sb)
    {
      if (this._generateFrom == DatabaseObjectToGenerateFrom.Tables)
      {
        if (!this._table.IsContainsPrimaryAndForeignKeyColumnsOnly)
          this.WriteSelectByPrimaryKey45Method(sb);
        this.WriteGetRecordCountMethod(sb);
        this.WriteGetRecordByCountMethods(sb);
        this.WriteGetRecordCountSharedMethods(sb);
        this.WriteGetRecordCountDynamicWhere(sb);
        this.WriteSelectSkipAndTakeMethod(sb);
        this.WriteSelectSkipAndTakeByMethods(sb);
        this.WriteSelectSkipAndTakeDynamicWhereMethod(sb);
        this.WriteSelectAllMethod(sb);
        this.WriteSelectAllDynamicWhereMethod(sb);
        this.WriteCollectionByMethods(sb);
        if (!this._table.IsContainsPrimaryAndForeignKeyColumnsOnly && this._table.PrimaryKeyCount == 1)
          this.WriteSelectDropDownListData(sb);
        this.WriteSelectSharedMethod(sb);
        this.WriteInsertMethod(sb);
        if (!this._table.IsContainsPrimaryAndForeignKeyColumnsOnly)
          this.WriteUpdateMethod(sb);
        this.WriteInsertUpdateMethod(sb);
        this.WriteDeleteMethod(sb);
        this.WriteAddSearchCommandParamsSharedMethod(sb);
      }
      else
      {
        this.WriteSelectSharedMethod(sb);
        this.WriteGetRecordCountMethod(sb);
        this.WriteGetRecordCountSharedMethods(sb);
        this.WriteSelectSkipAndTakeMethod(sb);
      }
      this.WriteCreateFromDataRowSharedMethod(sb);
    }

    private void WriteSelectAllMethod(StringBuilder sb)
    {
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Selects all " + this._table.Name);
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         internal static List<" + this._table.Name + "> SelectAll()");
      sb.AppendLine("         {");
      if (this._isUseStoredProcedure)
      {
        string storedProcName = Functions.GetStoredProcName(this._table, StoredProcName.SelectAll, this._spPrefixSuffixIndex, this._storedProcPrefix, this._storedProcSuffix);
        sb.AppendLine("             return SelectShared(\"" + storedProcName + "\", String.Empty, null);");
      }
      else
      {
        sb.AppendLine("             string dynamicSqlScript = " + this._table.Name + "Sql.SelectAll();");
        sb.AppendLine("             return SelectShared(dynamicSqlScript, String.Empty, null);");
      }
      sb.AppendLine("         }");
    }

    private void WriteGetRecordCountMethod(StringBuilder sb)
    {
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Gets the total number of records in the " + this._table.Name + " table");
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         internal static int GetRecordCount()");
      sb.AppendLine("         {");
      if (this._isUseStoredProcedure)
      {
        string storedProcName = Functions.GetStoredProcName(this._table, StoredProcName.GetRecordCount, this._spPrefixSuffixIndex, this._storedProcPrefix, this._storedProcSuffix);
        sb.AppendLine("             return GetRecordCountShared(\"" + storedProcName + "\", null, null, true, null);");
      }
      else
      {
        sb.AppendLine("             string sql = " + this._table.Name + "Sql.GetRecordCount();");
        sb.AppendLine("             return GetRecordCountShared(null, null, null, false, sql);");
      }
      sb.AppendLine("         }");
    }

    private void WriteGetRecordByCountMethods(StringBuilder sb)
    {
      foreach (Column foreignKeyColumn in this._table.ForeignKeyColumns)
      {
        sb.AppendLine("");
        sb.AppendLine("         /// <summary>");
        sb.AppendLine("         /// Gets the total number of records in the " + this._table.Name + " table by " + foreignKeyColumn.Name);
        sb.AppendLine("         /// </summary>");
        sb.AppendLine("         internal static int GetRecordCountBy" + foreignKeyColumn.Name + "(" + foreignKeyColumn.SystemType + " " + foreignKeyColumn.NameCamelStyle + ")");
        sb.AppendLine("         {");
        if (this._isUseStoredProcedure)
        {
          string storedProcNameBy = Functions.GetStoredProcNameBy(this._table, foreignKeyColumn, this._spPrefixSuffixIndex, this._storedProcPrefix, this._storedProcSuffix, StoredProcName.GetRecordCountBy);
          sb.AppendLine("             return GetRecordCountShared(\"" + storedProcNameBy + "\", \"" + foreignKeyColumn.NameCamelStyle + "\", " + foreignKeyColumn.NameCamelStyle + ", true, null);");
        }
        else
        {
          sb.AppendLine("             string sql = " + this._table.Name + "Sql.GetRecordCountBy" + foreignKeyColumn.Name + "(" + foreignKeyColumn.NameCamelStyle + ");");
          sb.AppendLine("             return GetRecordCountShared(null, \"" + foreignKeyColumn.NameCamelStyle + "\", " + foreignKeyColumn.NameCamelStyle + ", false, sql);");
        }
        sb.AppendLine("         }");
      }
    }

    private void WriteSelectSkipAndTakeMethod(StringBuilder sb)
    {
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Selects " + this._table.Name + " records sorted by the sortByExpression and returns records from the startRowIndex with rows (# of rows)");
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         internal static List<" + this._table.Name + "> SelectSkipAndTake(string sortByExpression, int startRowIndex, int rows)");
      sb.AppendLine("         {");
      if (this._isUseStoredProcedure)
      {
        string storedProcName = Functions.GetStoredProcName(this._table, StoredProcName.SelectSkipAndTake, this._spPrefixSuffixIndex, this._storedProcPrefix, this._storedProcSuffix);
        sb.AppendLine("             return SelectShared(\"" + storedProcName + "\", null, null, true, null, sortByExpression, startRowIndex, rows);");
      }
      else
      {
        sb.AppendLine("             string dynamicSqlScript = " + this._table.Name + "Sql.SelectSkipAndTake();");
        sb.AppendLine("             return SelectShared(dynamicSqlScript, null, null, sortByExpression, startRowIndex, rows);");
      }
      sb.AppendLine("         }");
    }

    private void WriteSelectSkipAndTakeByMethods(StringBuilder sb)
    {
      foreach (Column foreignKeyColumn in this._table.ForeignKeyColumns)
      {
        sb.AppendLine("");
        sb.AppendLine("         /// <summary>");
        sb.AppendLine("         /// Selects records by " + foreignKeyColumn.Name + " as a collection (List) of " + this._table.Name + " sorted by the sortByExpression and returns the rows (# of records) starting from the startRowIndex");
        sb.AppendLine("         /// </summary>");
        sb.AppendLine("         internal static List<" + this._table.Name + "> SelectSkipAndTakeBy" + foreignKeyColumn.Name + "(string sortByExpression, int startRowIndex, int rows, " + foreignKeyColumn.SystemType + " " + foreignKeyColumn.NameCamelStyle + ")");
        sb.AppendLine("         {");
        if (this._isUseStoredProcedure)
        {
          string storedProcNameBy = Functions.GetStoredProcNameBy(this._table, foreignKeyColumn, this._spPrefixSuffixIndex, this._storedProcPrefix, this._storedProcSuffix, StoredProcName.SelectSkipAndTakeBy);
          sb.AppendLine("             return SelectShared(\"" + storedProcNameBy + "\", \"" + foreignKeyColumn.NameCamelStyle + "\", " + foreignKeyColumn.NameCamelStyle + ", true, null, sortByExpression, startRowIndex, rows);");
        }
        else
        {
          sb.AppendLine("             string dynamicSqlScript = " + this._table.Name + "Sql.SelectSkipAndTakeBy" + foreignKeyColumn.Name + "(" + foreignKeyColumn.NameCamelStyle + ");");
          sb.AppendLine("             return SelectShared(dynamicSqlScript, \"" + foreignKeyColumn.NameCamelStyle + "\", " + foreignKeyColumn.NameCamelStyle + ", sortByExpression, startRowIndex, rows);");
        }
        sb.AppendLine("         }");
      }
    }

    private void WriteCollectionByMethods(StringBuilder sb)
    {
      foreach (Column foreignKeyColumn in this._table.ForeignKeyColumns)
      {
        string storedProcNameBy = Functions.GetStoredProcNameBy(this._table, foreignKeyColumn, this._spPrefixSuffixIndex, this._storedProcPrefix, this._storedProcSuffix, StoredProcName.SelectAllBy);
        sb.AppendLine("");
        sb.AppendLine("         /// <summary>");
        sb.AppendLine("         /// Selects all " + this._table.Name + " by " + foreignKeyColumn.ForeignKeyTableName + ", related to column " + foreignKeyColumn.Name ?? "");
        sb.AppendLine("         /// </summary>");
        sb.AppendLine("         internal static List<" + this._table.Name + "> Select" + this._table.Name + "CollectionBy" + foreignKeyColumn.Name + "(" + foreignKeyColumn.ForeignKeySystemType + " " + foreignKeyColumn.ForeignKeyColumnNameCamelStyle + ")");
        sb.AppendLine("         {");
        if (this._isUseStoredProcedure)
        {
          sb.AppendLine("             return SelectShared(\"" + storedProcNameBy + "\", \"" + foreignKeyColumn.NameCamelStyle + "\", " + foreignKeyColumn.ForeignKeyColumnNameCamelStyle + ");");
        }
        else
        {
          sb.AppendLine("             string dynamicSqlScript = " + this._table.Name + "Sql.SelectAllBy" + foreignKeyColumn.Name + "(" + foreignKeyColumn.ForeignKeyColumnNameCamelStyle + ");");
          sb.AppendLine("             return SelectShared(dynamicSqlScript, \"" + foreignKeyColumn.ForeignKeyColumnNameCamelStyle + "\", " + foreignKeyColumn.ForeignKeyColumnNameCamelStyle + ");");
        }
        sb.AppendLine("         }");
      }
    }

    private void WriteInsertMethod(StringBuilder sb)
    {
      string methodReturnType = Functions.GetInsertMethodReturnType(this._table, Language.CSharp);
      string str = "return ";
      if (methodReturnType == "void")
        str = string.Empty;
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Inserts a record");
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         internal static " + methodReturnType + " Insert(" + this._table.Name + " " + this._table.VariableObjName + ")");
      sb.AppendLine("         {");
      if (this._isUseStoredProcedure)
      {
        string storedProcName = Functions.GetStoredProcName(this._table, StoredProcName.Insert, this._spPrefixSuffixIndex, this._storedProcPrefix, this._storedProcSuffix);
        sb.AppendLine("             string storedProcName = \"" + storedProcName + "\";");
        sb.AppendLine("             " + str + "InsertUpdate(" + this._table.VariableObjName + ", false, storedProcName);");
      }
      else
        sb.AppendLine("             " + str + "InsertUpdate(" + this._table.VariableObjName + ", false);");
      sb.AppendLine("         }");
    }

    private void WriteUpdateMethod(StringBuilder sb)
    {
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Updates a record");
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         internal static void Update(" + this._table.Name + " " + this._table.VariableObjName + ")");
      sb.AppendLine("         {");
      if (this._isUseStoredProcedure)
      {
        string storedProcName = Functions.GetStoredProcName(this._table, StoredProcName.Update, this._spPrefixSuffixIndex, this._storedProcPrefix, this._storedProcSuffix);
        sb.AppendLine("             string storedProcName = \"" + storedProcName + "\";");
        sb.AppendLine("             InsertUpdate(" + this._table.VariableObjName + ", true, storedProcName);");
      }
      else
        sb.AppendLine("             InsertUpdate(" + this._table.VariableObjName + ", true);");
      sb.AppendLine("         }");
    }

    private void WriteAddSearchCommandParamsSharedMethod(StringBuilder sb)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (Column spatialDataTypeColumn in this._table.NoneBinaryOrSpatialDataTypeColumns)
      {
        if (Functions.GetFieldType(spatialDataTypeColumn) == "String")
          stringBuilder.AppendLine("              if(!String.IsNullOrEmpty(" + spatialDataTypeColumn.NameCamelStyle + "))");
        else
          stringBuilder.AppendLine("              if(" + spatialDataTypeColumn.NameCamelStyle + " != null)");
        stringBuilder.AppendLine("                  command.Parameters.AddWithValue(\"@" + spatialDataTypeColumn.NameCamelStyle + "\", " + spatialDataTypeColumn.NameCamelStyle + ");");
        stringBuilder.AppendLine("              else");
        stringBuilder.AppendLine("                  command.Parameters.AddWithValue(\"@" + spatialDataTypeColumn.NameCamelStyle + "\", System.DBNull.Value);");
        stringBuilder.AppendLine("");
      }
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Adds search parameters to the Command object");
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         private static void AddSearchCommandParamsShared(SqlCommand command, " + Functions.GetCommaDelimitedParamsForSearchMethod(this._table, Language.CSharp) + ")");
      sb.AppendLine("         {");
      sb.Append(stringBuilder.ToString());
      sb.AppendLine("         }");
    }

    private void WriteCreateFromDataRowSharedMethod(StringBuilder sb)
    {
      int num = 0;
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Creates a " + this._table.Name + " object from the passed data row");
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         private static " + this._table.Name + " Create" + this._table.Name + "FromDataRowShared(DataRow dr)");
      sb.AppendLine("         {");
      sb.AppendLine("             " + this._table.Name + " " + this._table.VariableObjName + " = new " + this._table.Name + "();");
      sb.AppendLine("");
      foreach (Column column in (List<Column>) this._table.Columns)
      {
        if (column.IsNullable && column.SQLDataType != SQLType.bit)
        {
          sb.AppendLine("");
          if (column.IsForeignKey)
          {
            string empty = string.Empty;
            string.IsNullOrEmpty(column.NullableChar);
            if (column.SQLDataType == SQLType.sysname)
              sb.AppendLine("             if (dr[" + (object) num + "] != System.DBNull.Value)");
            else
              sb.AppendLine("             if (dr[\"" + column.Name + "\"] != System.DBNull.Value)");
            if (column.SQLDataType == SQLType.sysname)
              sb.AppendLine("                 " + this._table.VariableObjName + "." + column.Name + " = (" + column.SystemType + ")dr[" + (object) num + "];");
            else if (column.SystemType == "System.Xml.Linq.XElement")
              sb.AppendLine("                 " + this._table.VariableObjName + "." + column.Name + " = System.Xml.Linq.XElement.Parse(dr[\"" + column.Name + "\"].ToString());");
            else if (column.SystemType.ToLower() == "string")
              sb.AppendLine("                 " + this._table.VariableObjName + "." + column.Name + " = dr[\"" + column.Name + "\"].ToString();");
            else
              sb.AppendLine("                 " + this._table.VariableObjName + "." + column.Name + " = (" + column.SystemType + ")dr[\"" + column.Name + "\"];");
            sb.AppendLine("             else");
            sb.AppendLine("                 " + this._table.VariableObjName + "." + column.Name + " = null;");
            sb.AppendLine("");
          }
          else
          {
            if (column.SQLDataType == SQLType.sysname)
              sb.AppendLine("             if (dr[" + (object) num + "] != System.DBNull.Value)");
            else
              sb.AppendLine("             if (dr[\"" + column.Name + "\"] != System.DBNull.Value)");
            if (column.SQLDataType == SQLType.sysname)
              sb.AppendLine("                 " + this._table.VariableObjName + "." + column.Name + " = (" + column.SystemType + ")dr[" + (object) num + "];");
            else if (column.SystemType == "System.Xml.Linq.XElement")
              sb.AppendLine("                 " + this._table.VariableObjName + "." + column.Name + " = System.Xml.Linq.XElement.Parse(dr[\"" + column.Name + "\"].ToString());");
            else if (column.SystemType.ToLower() == "string")
              sb.AppendLine("                 " + this._table.VariableObjName + "." + column.Name + " = dr[\"" + column.Name + "\"].ToString();");
            else
              sb.AppendLine("                 " + this._table.VariableObjName + "." + column.Name + " = (" + column.SystemType + ")dr[\"" + column.Name + "\"];");
            sb.AppendLine("             else");
            sb.AppendLine("                 " + this._table.VariableObjName + "." + column.Name + " = null;");
          }
        }
        else if (column.IsNullable && column.SQLDataType == SQLType.bit)
        {
          sb.AppendLine("             if (dr[\"" + column.Name + "\"] != System.DBNull.Value)");
          sb.AppendLine("                 " + this._table.VariableObjName + "." + column.Name + " = (" + column.SystemType + ")dr[\"" + column.Name + "\"];");
          sb.AppendLine("             else");
          sb.AppendLine("                 " + this._table.VariableObjName + "." + column.Name + " = false;");
        }
        else if (column.SQLDataType == SQLType.sysname)
          sb.AppendLine("             " + this._table.VariableObjName + "." + column.Name + " = (" + column.SystemType + ")dr[" + (object) num + "];");
        else if (column.SystemType == "System.Xml.Linq.XElement")
          sb.AppendLine("             " + this._table.VariableObjName + "." + column.Name + " = System.Xml.Linq.XElement.Parse(dr[\"" + column.Name + "\"].ToString());");
        else if (column.SystemType.ToLower() == "string")
          sb.AppendLine("             " + this._table.VariableObjName + "." + column.Name + " = dr[\"" + column.Name + "\"].ToString();");
        else
          sb.AppendLine("             " + this._table.VariableObjName + "." + column.Name + " = (" + column.SystemType + ")dr[\"" + column.Name + "\"];");
        ++num;
      }
      Functions.WriteRelatedTableStringBuilder(this._table, sb, RelatedTablePart.DataLayerBaseWriteSelectSharedMethod, this._selectedTables, this._referencedTables, Language.CSharp);
      sb.AppendLine("");
      sb.AppendLine("             return " + this._table.VariableObjName + ";");
      sb.AppendLine("         }");
    }

    private void WriteMethodsExpress(StringBuilder sb)
    {
      if (!this._table.IsContainsPrimaryAndForeignKeyColumnsOnly)
        this.WriteSelectByPrimaryKeyMethodExpress(sb);
      this.WriteGetRecordCountMethodExpress(sb);
      this.WriteGetRecordByCountMethodsExpress(sb);
      this.WriteGetRecordCountDynamicWhereExpress(sb);
      this.WriteSelectSkipAndTakeMethodExpress(sb);
      this.WriteSelectSkipAndTakeByMethodsExpress(sb);
      this.WriteSelectSkipAndTakeDynamicWhereMethodExpress(sb);
      this.WriteSelectAllMethodExpress(sb);
      this.WriteCollectionByMethodsExpress(sb);
      if (!this._table.IsContainsPrimaryAndForeignKeyColumnsOnly && this._table.PrimaryKeyCount == 1)
        this.WriteSelectDropDownListDataExpress(sb);
      this.WriteSelectSharedMethodExpress(sb);
      this.WriteInsertMethodExpress(sb);
      if (!this._table.IsContainsPrimaryAndForeignKeyColumnsOnly)
        this.WriteUpdateMethodExpress(sb);
      this.WriteInsertUpdateMethodExpress(sb);
      this.WriteDeleteMethodExpress(sb);
    }

    private void WriteSelectByPrimaryKeyMethodExpress(StringBuilder sb)
    {
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Selects a record by primary key(s)");
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         internal static " + this._table.Name + " SelectByPrimaryKey(" + Functions.GetCommaDelimitedPrimaryKeysAndType(this._table, Language.CSharp) + ")");
      sb.AppendLine("         {");
      sb.AppendLine("            // add your code here");
      sb.AppendLine("            throw new NotImplementedException();");
      sb.AppendLine("         }");
    }

    private void WriteSelectAllMethodExpress(StringBuilder sb)
    {
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Selects all " + this._table.Name ?? "");
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         internal static List<" + this._table.Name + "> SelectAll()");
      sb.AppendLine("         {");
      sb.AppendLine("            // add your code here");
      sb.AppendLine("            throw new NotImplementedException();");
      sb.AppendLine("         }");
    }

    private void WriteCollectionByMethodsExpress(StringBuilder sb)
    {
      foreach (Column foreignKeyColumn in this._table.ForeignKeyColumns)
      {
        Functions.GetStoredProcNameBy(this._table, foreignKeyColumn, this._spPrefixSuffixIndex, this._storedProcPrefix, this._storedProcSuffix, StoredProcName.SelectAllBy);
        sb.AppendLine("");
        sb.AppendLine("         /// <summary>");
        sb.AppendLine("         /// Selects all " + this._table.Name + " by " + foreignKeyColumn.ForeignKeyTableName + ", related to column " + foreignKeyColumn.Name ?? "");
        sb.AppendLine("         /// </summary>");
        sb.AppendLine("         internal static List<" + this._table.Name + "> Select" + this._table.Name + "CollectionBy" + foreignKeyColumn.Name + "(" + foreignKeyColumn.ForeignKeySystemType + " " + foreignKeyColumn.ForeignKeyColumnNameCamelStyle + ")");
        sb.AppendLine("         {");
        sb.AppendLine("            // add your code here");
        sb.AppendLine("            throw new NotImplementedException();");
        sb.AppendLine("         }");
      }
    }

    private void WriteSelectDropDownListDataExpress(StringBuilder sb)
    {
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Selects " + this._table.FirstPrimaryKeyName + " and " + this._table.DataTextField + " columns for use with a DropDownList web control");
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         internal static List<" + this._table.Name + "> Select" + this._table.Name + MyConstants.WordDropDownListData + "()");
      sb.AppendLine("         {");
      sb.AppendLine("            // add your code here");
      sb.AppendLine("            throw new NotImplementedException();");
      sb.AppendLine("         }");
    }

    private void WriteSelectSharedMethodExpress(StringBuilder sb)
    {
      sb.AppendLine("");
      sb.AppendLine("         internal static List<" + this._table.Name + "> SelectShared(string storedProcName, string param, object paramValue)");
      sb.AppendLine("         {");
      sb.AppendLine("            // add your code here");
      sb.AppendLine("            throw new NotImplementedException();");
      sb.AppendLine("         }");
    }

    private void WriteInsertMethodExpress(StringBuilder sb)
    {
      string methodReturnType = Functions.GetInsertMethodReturnType(this._table, Language.CSharp);
      if (methodReturnType == "void")
      {
        string empty = string.Empty;
      }
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Inserts a record");
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         internal static " + methodReturnType + " Insert(" + this._table.Name + " " + this._table.VariableObjName + ")");
      sb.AppendLine("         {");
      sb.AppendLine("            // add your code here");
      sb.AppendLine("            throw new NotImplementedException();");
      sb.AppendLine("         }");
    }

    private void WriteUpdateMethodExpress(StringBuilder sb)
    {
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Updates a record");
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         internal static void Update(" + this._table.Name + " " + this._table.VariableObjName + ")");
      sb.AppendLine("         {");
      sb.AppendLine("            // add your code here");
      sb.AppendLine("            throw new NotImplementedException();");
      sb.AppendLine("         }");
    }

    private void WriteInsertUpdateMethodExpress(StringBuilder sb)
    {
      string methodReturnType = Functions.GetInsertMethodReturnType(this._table, Language.CSharp);
      sb.AppendLine("");
      sb.AppendLine("         private static " + methodReturnType + " InsertUpdate(" + this._table.Name + " " + this._table.VariableObjName + ", bool isUpdate)");
      sb.AppendLine("         {");
      sb.AppendLine("            // add your code here");
      sb.AppendLine("            throw new NotImplementedException();");
      sb.AppendLine("         }");
    }

    private void WriteDeleteMethodExpress(StringBuilder sb)
    {
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Deletes a record based on primary key(s)");
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         internal static void Delete(" + Functions.GetCommaDelimitedPrimaryKeysAndType(this._table, Language.CSharp) + ")");
      sb.AppendLine("         {");
      sb.AppendLine("            // add your code here");
      sb.AppendLine("            throw new NotImplementedException();");
      sb.AppendLine("         }");
    }

    private void WriteGetRecordCountMethodExpress(StringBuilder sb)
    {
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Gets the total number of records in the " + this._table.Name + " table");
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         internal static int GetRecordCount()");
      sb.AppendLine("         {");
      sb.AppendLine("            // add your code here");
      sb.AppendLine("            throw new NotImplementedException();");
      sb.AppendLine("         }");
    }

    private void WriteGetRecordByCountMethodsExpress(StringBuilder sb)
    {
      foreach (Column foreignKeyColumn in this._table.ForeignKeyColumns)
      {
        sb.AppendLine("");
        sb.AppendLine("         /// <summary>");
        sb.AppendLine("         /// Gets the total number of records in the " + this._table.Name + " table by " + foreignKeyColumn.Name);
        sb.AppendLine("         /// </summary>");
        sb.AppendLine("         internal static int GetRecordCountBy" + foreignKeyColumn.Name + "(" + foreignKeyColumn.SystemType + " " + foreignKeyColumn.NameCamelStyle + ")");
        sb.AppendLine("         {");
        sb.AppendLine("            // add your code here");
        sb.AppendLine("            throw new NotImplementedException();");
        sb.AppendLine("         }");
      }
    }

    private void WriteGetRecordCountDynamicWhereExpress(StringBuilder sb)
    {
      Functions.GetStoredProcName(this._table, StoredProcName.GetRecordCountWhereDynamic, this._spPrefixSuffixIndex, this._storedProcPrefix, this._storedProcSuffix);
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Gets the total number of records in the " + this._table.Name + " table based on search parameters");
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         internal static int GetRecordCountDynamicWhere(" + Functions.GetCommaDelimitedParamsForSearchMethod(this._table, Language.CSharp) + ")");
      sb.AppendLine("         {");
      sb.AppendLine("            // add your code here");
      sb.AppendLine("            throw new NotImplementedException();");
      sb.AppendLine("         }");
    }

    private void WriteSelectSkipAndTakeMethodExpress(StringBuilder sb)
    {
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Selects " + this._table.Name + " records sorted by the sortByExpression and returns records from the startRowIndex wint rows (# of records)");
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         internal static List<" + this._table.Name + "> SelectSkipAndTake(string sortByExpression, int startRowIndex, int rows)");
      sb.AppendLine("         {");
      sb.AppendLine("            // add your code here");
      sb.AppendLine("            throw new NotImplementedException();");
      sb.AppendLine("         }");
    }

    private void WriteSelectSkipAndTakeByMethodsExpress(StringBuilder sb)
    {
      foreach (Column foreignKeyColumn in this._table.ForeignKeyColumns)
      {
        sb.AppendLine("");
        sb.AppendLine("         /// <summary>");
        sb.AppendLine("         /// Selects records by " + foreignKeyColumn.Name + " as a collection (List) of " + this._table.Name + " sorted by the sortByExpression and returns the rows (# of records) starting from the startRowIndex");
        sb.AppendLine("         /// </summary>");
        sb.AppendLine("         internal static List<" + this._table.Name + "> SelectSkipAndTakeBy" + foreignKeyColumn.Name + "(string sortByExpression, int startRowIndex, int rows, " + foreignKeyColumn.SystemType + " " + foreignKeyColumn.NameCamelStyle + ")");
        sb.AppendLine("         {");
        sb.AppendLine("            // add your code here");
        sb.AppendLine("            throw new NotImplementedException();");
        sb.AppendLine("         }");
      }
    }

    private void WriteSelectSkipAndTakeDynamicWhereMethodExpress(StringBuilder sb)
    {
      Functions.GetStoredProcName(this._table, StoredProcName.SelectSkipAndTakeWhereDynamic, this._spPrefixSuffixIndex, this._storedProcPrefix, this._storedProcSuffix);
      sb.AppendLine("");
      sb.AppendLine("         internal static List<" + this._table.Name + "> SelectSkipAndTakeDynamicWhere(" + Functions.GetCommaDelimitedParamsForSearchMethod(this._table, Language.CSharp) + ", string sortByExpression, int startRowIndex, int rows)");
      sb.AppendLine("         {");
      sb.AppendLine("            // add your code here");
      sb.AppendLine("            throw new NotImplementedException();");
      sb.AppendLine("         }");
    }

    private void WriteSelectByPrimaryKey45Method(StringBuilder sb)
    {
      StringBuilder sbDocComments = new StringBuilder();
      StringBuilder sbCodeBeforeUsing = new StringBuilder();
      StringBuilder sbParameters = new StringBuilder();
      StringBuilder sbDataAdapterRows = new StringBuilder();
      string method = "internal static " + this._table.Name + " SelectByPrimaryKey(" + Functions.GetCommaDelimitedPrimaryKeysAndType(this._table, Language.CSharp) + ")";
      string variableObjName = this._table.VariableObjName;
      sbDocComments.AppendLine("         /// <summary>");
      sbDocComments.AppendLine("         /// Selects a record by primary key(s)");
      sbDocComments.AppendLine("         /// </summary>");
      string str = !this._isUseStoredProcedure ? "string dynamicSqlScript = " + this._table.Name + "Sql.SelectByPrimaryKey();" : "string storedProcName = \"" + Functions.GetStoredProcName(this._table, StoredProcName.SelectByPrimaryKey, this._spPrefixSuffixIndex, this._storedProcPrefix, this._storedProcSuffix) + "\";";
      sbCodeBeforeUsing.AppendLine("              " + this._table.Name + " " + this._table.VariableObjName + " = null;");
      sbCodeBeforeUsing.AppendLine("              " + str);
      sbCodeBeforeUsing.AppendLine("");
      sbParameters.AppendLine("");
      sbParameters.AppendLine("                      // parameters");
      foreach (Column primaryKeyColumn in (List<Column>) this._table.PrimaryKeyColumns)
        sbParameters.AppendLine("                      command.Parameters.AddWithValue(\"@" + primaryKeyColumn.NameCamelStyle + "\", " + primaryKeyColumn.NameCamelStyle + ");");
      sbDataAdapterRows.AppendLine("                                  " + this._table.VariableObjName + " = Create" + this._table.Name + "FromDataRowShared(dt.Rows[0]);");
      this.WriteMethodWithUsingSqlConnection(sb, sbDocComments, sbCodeBeforeUsing, sbParameters, sbDataAdapterRows, method, variableObjName, true, "");
    }

    private void WriteGetRecordCountSharedMethods(StringBuilder sb)
    {
      StringBuilder sbDocComments = new StringBuilder();
      StringBuilder sbCodeBeforeUsing = new StringBuilder();
      StringBuilder sbParameters = new StringBuilder();
      StringBuilder sbDataAdapterRows = new StringBuilder();
      string method = "internal static int GetRecordCountShared(string storedProcName = null, string param = null, object paramValue = null, bool isUseStoredProc = true, string dynamicSqlScript = null)";
      string returnObject = "recordCount";
      sbCodeBeforeUsing.AppendLine("              int recordCount = 0;");
      sbCodeBeforeUsing.AppendLine("");
      if (this._table.ForeignKeyCount > 0)
      {
        sbParameters.AppendLine("");
        sbParameters.AppendLine("                      // parameters");
        sbParameters.AppendLine("                      switch (param)");
        sbParameters.AppendLine("                      {");
        foreach (Column foreignKeyColumn in this._table.ForeignKeyColumns)
        {
          sbParameters.AppendLine("                          case \"" + foreignKeyColumn.NameCamelStyle + "\":");
          sbParameters.AppendLine("                              command.Parameters.AddWithValue(\"@" + foreignKeyColumn.NameCamelStyle + "\", paramValue);");
          sbParameters.AppendLine("                              break;");
        }
        sbParameters.AppendLine("                          default:");
        sbParameters.AppendLine("                              break;");
        sbParameters.AppendLine("                      }");
      }
      sbDataAdapterRows.AppendLine("                                  recordCount = (int)dt.Rows[0][\"RecordCount\"];");
      this.WriteMethodWithUsingSqlConnection(sb, sbDocComments, sbCodeBeforeUsing, sbParameters, sbDataAdapterRows, method, returnObject, true, "");
    }

    private void WriteGetRecordCountDynamicWhere(StringBuilder sb)
    {
      StringBuilder sbDocComments = new StringBuilder();
      StringBuilder sbCodeBeforeUsing = new StringBuilder();
      StringBuilder sbParameters = new StringBuilder();
      StringBuilder sbDataAdapterRows = new StringBuilder();
      string method = "internal static int GetRecordCountDynamicWhere(" + Functions.GetCommaDelimitedParamsForSearchMethod(this._table, Language.CSharp) + ")";
      string returnObject = "recordCount";
      sbDocComments.AppendLine("         /// <summary>");
      sbDocComments.AppendLine("         /// Gets the total number of records in the " + this._table.Name + " table based on search parameters");
      sbDocComments.AppendLine("         /// </summary>");
      string str = !this._isUseStoredProcedure ? "string dynamicSqlScript = " + this._table.Name + "Sql.GetRecordCountDynamicWhere();" : "string storedProcName = \"" + Functions.GetStoredProcName(this._table, StoredProcName.GetRecordCountWhereDynamic, this._spPrefixSuffixIndex, this._storedProcPrefix, this._storedProcSuffix) + "\";";
      sbCodeBeforeUsing.AppendLine("              int recordCount = 0;");
      sbCodeBeforeUsing.AppendLine("              " + str);
      sbCodeBeforeUsing.AppendLine("");
      sbParameters.AppendLine("");
      sbParameters.AppendLine("                      // search parameters");
      sbParameters.AppendLine("                      AddSearchCommandParamsShared(command, " + Functions.GetCommaDelimitedParamsForPassingToSearchMethod(this._table) + ");");
      sbDataAdapterRows.AppendLine("                                  recordCount = (int)dt.Rows[0][\"RecordCount\"];");
      this.WriteMethodWithUsingSqlConnection(sb, sbDocComments, sbCodeBeforeUsing, sbParameters, sbDataAdapterRows, method, returnObject, true, "");
    }

    private void WriteSelectSkipAndTakeDynamicWhereMethod(StringBuilder sb)
    {
      StringBuilder sbDocComments = new StringBuilder();
      StringBuilder sbCodeBeforeUsing = new StringBuilder();
      StringBuilder sbParameters = new StringBuilder();
      StringBuilder sbDataAdapterRows = new StringBuilder();
      string method = "internal static List<" + this._table.Name + "> SelectSkipAndTakeDynamicWhere(" + Functions.GetCommaDelimitedParamsForSearchMethod(this._table, Language.CSharp) + ", string sortByExpression, int startRowIndex, int rows)";
      string objCollectionName = this._table.VariableObjCollectionName;
      sbDocComments.AppendLine("         /// <summary>");
      sbDocComments.AppendLine("         /// Selects " + this._table.Name + " records sorted by the sortByExpression and returns records from the startRowIndex with rows (# of records) based on search parameters");
      sbDocComments.AppendLine("         /// </summary>");
      string str = !this._isUseStoredProcedure ? "string dynamicSqlScript = " + this._table.Name + "Sql.SelectSkipAndTakeDynamicWhere();" : "string storedProcName = \"" + Functions.GetStoredProcName(this._table, StoredProcName.SelectSkipAndTakeWhereDynamic, this._spPrefixSuffixIndex, this._storedProcPrefix, this._storedProcSuffix) + "\";";
      sbCodeBeforeUsing.AppendLine("              List<" + this._table.Name + "> " + this._table.VariableObjCollectionName + " = null;");
      sbCodeBeforeUsing.AppendLine("              " + str);
      sbCodeBeforeUsing.AppendLine("");
      sbParameters.AppendLine("");
      sbParameters.AppendLine("                      // select, skip, take, sort parameters");
      sbParameters.AppendLine("                      command.Parameters.AddWithValue(\"@start\", startRowIndex);");
      sbParameters.AppendLine("                      command.Parameters.AddWithValue(\"@numberOfRows\", rows);");
      sbParameters.AppendLine("                      command.Parameters.AddWithValue(\"@sortByExpression\", sortByExpression);");
      sbParameters.AppendLine("");
      sbParameters.AppendLine("                      // search parameters");
      sbParameters.AppendLine("                      AddSearchCommandParamsShared(command, " + Functions.GetCommaDelimitedParamsForPassingToSearchMethod(this._table) + ");");
      sbDataAdapterRows.AppendLine("                                  " + this._table.VariableObjCollectionName + " = new List<" + this._table.Name + ">();");
      sbDataAdapterRows.AppendLine("");
      sbDataAdapterRows.AppendLine("                                  foreach (DataRow dr in dt.Rows)");
      sbDataAdapterRows.AppendLine("                                  {");
      sbDataAdapterRows.AppendLine("                                      " + this._table.Name + " " + this._table.VariableObjName + " = Create" + this._table.Name + "FromDataRowShared(dr);");
      sbDataAdapterRows.AppendLine("                                      " + this._table.VariableObjCollectionName + ".Add(" + this._table.VariableObjName + ");");
      sbDataAdapterRows.AppendLine("                                  }");
      this.WriteMethodWithUsingSqlConnection(sb, sbDocComments, sbCodeBeforeUsing, sbParameters, sbDataAdapterRows, method, objCollectionName, true, "");
    }

    private void WriteSelectAllDynamicWhereMethod(StringBuilder sb)
    {
      StringBuilder sbDocComments = new StringBuilder();
      StringBuilder sbCodeBeforeUsing = new StringBuilder();
      StringBuilder sbParameters = new StringBuilder();
      StringBuilder sbDataAdapterRows = new StringBuilder();
      string method = "internal static List<" + this._table.Name + "> SelectAllDynamicWhere(" + Functions.GetCommaDelimitedParamsForSearchMethod(this._table, Language.CSharp) + ")";
      string objCollectionName = this._table.VariableObjCollectionName;
      string storedProcName = Functions.GetStoredProcName(this._table, StoredProcName.SelectAllWhereDynamic, this._spPrefixSuffixIndex, this._storedProcPrefix, this._storedProcSuffix);
      sbDocComments.AppendLine("         /// <summary>");
      sbDocComments.AppendLine("         /// Selects records based on the passed filters as a collection (List) of " + this._table.Name + ".");
      sbDocComments.AppendLine("         /// </summary>");
      sbCodeBeforeUsing.AppendLine("              List<" + this._table.Name + "> " + this._table.VariableObjCollectionName + " = null;");
      if (this._isUseStoredProcedure)
        sbCodeBeforeUsing.AppendLine("              string storedProcName = \"" + storedProcName + "\";");
      else
        sbCodeBeforeUsing.AppendLine("              string dynamicSqlScript = " + this._table.Name + "Sql.SelectAllDynamicWhere();");
      sbCodeBeforeUsing.AppendLine("");
      sbParameters.AppendLine("");
      sbParameters.AppendLine("                      // search parameters");
      sbParameters.AppendLine("                      AddSearchCommandParamsShared(command, " + Functions.GetCommaDelimitedParamsForPassingToSearchMethod(this._table) + ");");
      sbDataAdapterRows.AppendLine("                                  " + this._table.VariableObjCollectionName + " = new List<" + this._table.Name + ">();");
      sbDataAdapterRows.AppendLine("");
      sbDataAdapterRows.AppendLine("                                  foreach (DataRow dr in dt.Rows)");
      sbDataAdapterRows.AppendLine("                                  {");
      sbDataAdapterRows.AppendLine("                                      " + this._table.Name + " " + this._table.VariableObjName + " = Create" + this._table.Name + "FromDataRowShared(dr);");
      sbDataAdapterRows.AppendLine("                                      " + this._table.VariableObjCollectionName + ".Add(" + this._table.VariableObjName + ");");
      sbDataAdapterRows.AppendLine("                                  }");
      this.WriteMethodWithUsingSqlConnection(sb, sbDocComments, sbCodeBeforeUsing, sbParameters, sbDataAdapterRows, method, objCollectionName, true, "");
    }

    private void WriteSelectDropDownListData(StringBuilder sb)
    {
      StringBuilder sbDocComments = new StringBuilder();
      StringBuilder sbCodeBeforeUsing = new StringBuilder();
      StringBuilder sbParameters = new StringBuilder();
      StringBuilder sbDataAdapterRows = new StringBuilder();
      string method = "internal static List<" + this._table.Name + "> Select" + this._table.Name + MyConstants.WordDropDownListData + "()";
      string objCollectionName = this._table.VariableObjCollectionName;
      sbDocComments.AppendLine("         /// <summary>");
      sbDocComments.AppendLine("         /// Selects " + this._table.FirstPrimaryKeyName + " and " + this._table.DataTextField + " columns for use with a DropDownList web control");
      sbDocComments.AppendLine("         /// </summary>");
      string str;
      if (this._isUseStoredProcedure)
        str = "string storedProcName = \"" + Functions.GetStoredProcName(this._table, StoredProcName.SelectDropDownListData, this._spPrefixSuffixIndex, this._storedProcPrefix, this._storedProcSuffix) + "\";";
      else
        str = "string dynamicSqlScript = " + this._table.Name + "Sql.Select" + this._table.Name + MyConstants.WordDropDownListData + "();";
      sbCodeBeforeUsing.AppendLine("              List<" + this._table.Name + "> " + this._table.VariableObjCollectionName + " = null;");
      sbCodeBeforeUsing.AppendLine("              " + str);
      sbCodeBeforeUsing.AppendLine("");
      sbDataAdapterRows.AppendLine("                                  " + this._table.VariableObjCollectionName + " = new List<" + this._table.Name + ">();");
      sbDataAdapterRows.AppendLine("");
      sbDataAdapterRows.AppendLine("                                  foreach (DataRow dr in dt.Rows)");
      sbDataAdapterRows.AppendLine("                                  {");
      sbDataAdapterRows.AppendLine("                                      " + this._table.Name + " " + this._table.VariableObjName + " = new " + this._table.Name + "();");
      if (this._table.FirstPrimaryKeySQLDataType == SQLType.uniqueidentifier)
        sbDataAdapterRows.AppendLine("                                      " + this._table.VariableObjName + "." + this._table.FirstPrimaryKeyName + " = dr[\"" + this._table.FirstPrimaryKeyName + "\"].ToString();");
      else
        sbDataAdapterRows.AppendLine("                                      " + this._table.VariableObjName + "." + this._table.FirstPrimaryKeyName + " = (" + this._table.FirstPrimaryKeySystemType + ")dr[\"" + this._table.FirstPrimaryKeyName + "\"];");
      if (this._table.IsDataTextFieldNullable)
      {
        sbDataAdapterRows.AppendLine("");
        sbDataAdapterRows.AppendLine("                                      if (dr[\"" + this._table.DataTextField + "\"] != System.DBNull.Value)");
        sbDataAdapterRows.AppendLine("                                          " + this._table.VariableObjName + "." + this._table.DataTextField + " = (" + this._table.DataTextFieldSystemType + ")(dr[\"" + this._table.DataTextField + "\"]);");
        sbDataAdapterRows.AppendLine("                                      else");
        sbDataAdapterRows.AppendLine("                                          " + this._table.VariableObjName + "." + this._table.DataTextField + " = null;");
      }
      else
        sbDataAdapterRows.AppendLine("                                      " + this._table.VariableObjName + "." + this._table.DataTextField + " = (" + this._table.DataTextFieldSystemType + ")(dr[\"" + this._table.DataTextField + "\"]);");
      sbDataAdapterRows.AppendLine("");
      sbDataAdapterRows.AppendLine("                                      " + this._table.VariableObjCollectionName + ".Add(" + this._table.VariableObjName + ");");
      sbDataAdapterRows.AppendLine("                                  }");
      this.WriteMethodWithUsingSqlConnection(sb, sbDocComments, sbCodeBeforeUsing, sbParameters, sbDataAdapterRows, method, objCollectionName, true, "");
    }

    private void WriteSelectSharedMethod(StringBuilder sb)
    {
      StringBuilder sbDocComments = new StringBuilder();
      StringBuilder sbCodeBeforeUsing = new StringBuilder();
      StringBuilder sbParameters = new StringBuilder();
      StringBuilder sbDataAdapterRows = new StringBuilder();
      string objCollectionName = this._table.VariableObjCollectionName;
      string method = !this._isUseStoredProcedure ? "internal static List<" + this._table.Name + "> SelectShared(string dynamicSqlScript, string param, object paramValue, string sortByExpression = null, int? startRowIndex = null, int? rows = null)" : "internal static List<" + this._table.Name + "> SelectShared(string storedProcName, string param, object paramValue, bool isUseStoredProc = true, string dynamicSqlScript = null, string sortByExpression = null, int? startRowIndex = null, int? rows = null)";
      sbCodeBeforeUsing.AppendLine("              List<" + this._table.Name + "> " + this._table.VariableObjCollectionName + " = null;");
      sbCodeBeforeUsing.AppendLine("");
      sbParameters.AppendLine("");
      sbParameters.AppendLine("                      // select, skip, take, sort parameters");
      sbParameters.AppendLine("                      if (!String.IsNullOrEmpty(sortByExpression) && startRowIndex != null && rows != null)");
      sbParameters.AppendLine("                      {");
      sbParameters.AppendLine("                          command.Parameters.AddWithValue(\"@start\", startRowIndex.Value);");
      sbParameters.AppendLine("                          command.Parameters.AddWithValue(\"@numberOfRows\", rows.Value);");
      sbParameters.AppendLine("                          command.Parameters.AddWithValue(\"@sortByExpression\", sortByExpression);");
      sbParameters.AppendLine("                      }");
      if (this._table.ForeignKeyCount > 0)
      {
        sbParameters.AppendLine("");
        sbParameters.AppendLine("                      // parameters");
        sbParameters.AppendLine("                      switch (param)");
        sbParameters.AppendLine("                      {");
        foreach (Column foreignKeyColumn in this._table.ForeignKeyColumns)
        {
          sbParameters.AppendLine("                          case \"" + foreignKeyColumn.NameCamelStyle + "\":");
          sbParameters.AppendLine("                              command.Parameters.AddWithValue(\"@" + foreignKeyColumn.NameCamelStyle + "\", paramValue);");
          sbParameters.AppendLine("                              break;");
        }
        sbParameters.AppendLine("                          default:");
        sbParameters.AppendLine("                              break;");
        sbParameters.AppendLine("                      }");
      }
      sbDataAdapterRows.AppendLine("                                  " + this._table.VariableObjCollectionName + " = new List<" + this._table.Name + ">();");
      sbDataAdapterRows.AppendLine("");
      sbDataAdapterRows.AppendLine("                                  foreach (DataRow dr in dt.Rows)");
      sbDataAdapterRows.AppendLine("                                  {");
      sbDataAdapterRows.AppendLine("                                      " + this._table.Name + " " + this._table.VariableObjName + " = Create" + this._table.Name + "FromDataRowShared(dr);");
      sbDataAdapterRows.AppendLine("                                      " + this._table.VariableObjCollectionName + ".Add(" + this._table.VariableObjName + ");");
      sbDataAdapterRows.AppendLine("                                  }");
      this.WriteMethodWithUsingSqlConnection(sb, sbDocComments, sbCodeBeforeUsing, sbParameters, sbDataAdapterRows, method, objCollectionName, true, "");
    }

    private void WriteInsertUpdateMethod(StringBuilder sb)
    {
      StringBuilder sbDocComments = new StringBuilder();
      StringBuilder sbCodeBeforeUsing = new StringBuilder();
      StringBuilder sbParameters = new StringBuilder();
      StringBuilder sbDataAdapterRows = new StringBuilder();
      string str = "newlyCreated" + this._table.FirstPrimaryKeyName;
      string returnObject = str;
      string methodReturnType = Functions.GetInsertMethodReturnType(this._table, Language.CSharp);
      string method;
      if (this._isUseStoredProcedure)
        method = "private static " + methodReturnType + " InsertUpdate(" + this._table.Name + " " + this._table.VariableObjName + ", bool isUpdate, string storedProcName)";
      else
        method = "private static " + methodReturnType + " InsertUpdate(" + this._table.Name + " " + this._table.VariableObjName + ", bool isUpdate)";
      if (methodReturnType != "void")
      {
        sbCodeBeforeUsing.AppendLine("              " + this._table.FirstPrimaryKeySystemType + " " + str + " = " + this._table.VariableObjName + "." + this._table.FirstPrimaryKeyName + ";");
        sbCodeBeforeUsing.AppendLine("");
      }
      if (!this._isUseStoredProcedure)
      {
        sbCodeBeforeUsing.AppendLine("              string dynamicSqlScript = String.Empty;");
        sbCodeBeforeUsing.AppendLine("");
        sbCodeBeforeUsing.AppendLine("              if (isUpdate)");
        sbCodeBeforeUsing.AppendLine("                  dynamicSqlScript = " + this._table.Name + "Sql.Update();");
        sbCodeBeforeUsing.AppendLine("              else");
        sbCodeBeforeUsing.AppendLine("                  dynamicSqlScript = " + this._table.Name + "Sql.Insert();");
      }
      int num = 0;
      string empty = string.Empty;
      foreach (Column column in (List<Column>) this._table.Columns)
      {
        if (column.IsNullable && column.SQLDataType != SQLType.bit && (!column.IsIdentity && !column.IsComputed) && (!column.IsUniqueIdWithNewId && !column.IsDateWithGetDate))
        {
          string keywordVariableName = Functions.GetNoneKeywordVariableName(column.NameCamelStyle, Language.CSharp);
          sbCodeBeforeUsing.AppendLine("              object " + keywordVariableName + " = " + this._table.VariableObjName + "." + column.Name + ";");
          ++num;
        }
      }
      if (num > 0)
        sbCodeBeforeUsing.AppendLine("");
      foreach (Column column in (List<Column>) this._table.Columns)
      {
        if (column.IsNullable && column.SQLDataType != SQLType.bit && (!column.IsIdentity && !column.IsComputed) && (!column.IsUniqueIdWithNewId && !column.IsDateWithGetDate))
        {
          if (column.IsStringField)
            sbCodeBeforeUsing.AppendLine("              if (String.IsNullOrEmpty(" + this._table.VariableObjName + "." + column.Name + "))");
          else
            sbCodeBeforeUsing.AppendLine("              if (" + this._table.VariableObjName + "." + column.Name + " is null)");
          string keywordVariableName = Functions.GetNoneKeywordVariableName(column.NameCamelStyle, Language.CSharp);
          sbCodeBeforeUsing.AppendLine("                  " + keywordVariableName + " = System.DBNull.Value;");
          sbCodeBeforeUsing.AppendLine("");
        }
      }
      sbParameters.AppendLine("");
      sbParameters.AppendLine("                      // parameters");
      if (this._table.PrimaryKeyAutoFilledCount == this._table.PrimaryKeyCount)
      {
        sbParameters.AppendLine("                      if (isUpdate)");
        sbParameters.AppendLine("                      {");
        sbParameters.AppendLine("                          // for update only");
        foreach (Column primaryKeyColumn in (List<Column>) this._table.PrimaryKeyColumns)
          sbParameters.AppendLine("                          command.Parameters.AddWithValue(\"@" + primaryKeyColumn.NameCamelStyle + "\", " + this._table.VariableObjName + "." + primaryKeyColumn.Name + ");");
        sbParameters.AppendLine("                      }");
        sbParameters.AppendLine("");
      }
      else if (this._table.PrimaryKeyAutoFilledCount == 0)
      {
        foreach (Column primaryKeyColumn in (List<Column>) this._table.PrimaryKeyColumns)
          sbParameters.AppendLine("                      command.Parameters.AddWithValue(\"@" + primaryKeyColumn.NameCamelStyle + "\", " + this._table.VariableObjName + "." + primaryKeyColumn.Name + ");");
      }
      else
      {
        sbParameters.AppendLine("                      if (isUpdate)");
        sbParameters.AppendLine("                      {");
        sbParameters.AppendLine("                          // for update only");
        foreach (Column primaryKeyColumn in (List<Column>) this._table.PrimaryKeyColumns)
        {
          if (primaryKeyColumn.IsPrimaryKeyUnique)
            sbParameters.AppendLine("                          command.Parameters.AddWithValue(\"@" + primaryKeyColumn.NameCamelStyle + "\", " + this._table.VariableObjName + "." + primaryKeyColumn.Name + ");");
        }
        sbParameters.AppendLine("                      }");
        sbParameters.AppendLine("");
        foreach (Column primaryKeyColumn in (List<Column>) this._table.PrimaryKeyColumns)
        {
          if (!primaryKeyColumn.IsPrimaryKeyUnique)
            sbParameters.AppendLine("                      command.Parameters.AddWithValue(\"@" + primaryKeyColumn.NameCamelStyle + "\", " + this._table.VariableObjName + "." + primaryKeyColumn.Name + ");");
        }
      }
      foreach (Column column in (List<Column>) this._table.Columns)
      {
        if (!column.IsPrimaryKey && !column.IsIdentity && (!column.IsComputed && !column.IsUniqueIdWithNewId) && !column.IsDateWithGetDate)
        {
          if (!column.IsNullable)
          {
            if (column.SystemType == "System.Xml.Linq.XElement")
              sbParameters.AppendLine("                      command.Parameters.AddWithValue(\"@" + column.NameCamelStyle + "\", " + this._table.VariableObjName + "." + column.Name + ".ToString());");
            else
              sbParameters.AppendLine("                      command.Parameters.AddWithValue(\"@" + column.NameCamelStyle + "\", " + this._table.VariableObjName + "." + column.Name + ");");
          }
          else
          {
            string keywordVariableName = Functions.GetNoneKeywordVariableName(column.NameCamelStyle, Language.CSharp);
            if (column.SystemType == "System.Xml.Linq.XElement")
              sbParameters.AppendLine("                      command.Parameters.AddWithValue(\"@" + column.NameCamelStyle + "\", " + keywordVariableName + ".ToString());");
            else if (column.SQLDataType == SQLType.bit)
              sbParameters.AppendLine("                      command.Parameters.AddWithValue(\"@" + column.NameCamelStyle + "\", " + this._table.VariableObjName + "." + column.Name + ");");
            else
              sbParameters.AppendLine("                      command.Parameters.AddWithValue(\"@" + column.NameCamelStyle + "\", " + keywordVariableName + ");");
          }
        }
      }
      if (methodReturnType != "void")
      {
        sbDataAdapterRows.AppendLine("                      if (isUpdate)");
        sbDataAdapterRows.AppendLine("                          command.ExecuteNonQuery();");
        sbDataAdapterRows.AppendLine("                      else");
        if (this._table.FirstPrimaryKeyStoredProcParameter == "uniqueidentifier")
          sbDataAdapterRows.AppendLine("                          " + str + " = command.ExecuteScalar().ToString();");
        else
          sbDataAdapterRows.AppendLine("                          " + str + " = (" + this._table.FirstPrimaryKeySystemType + ")command.ExecuteScalar();");
      }
      else
        sbDataAdapterRows.AppendLine("                      command.ExecuteNonQuery();");
      this.WriteMethodWithUsingSqlConnection(sb, sbDocComments, sbCodeBeforeUsing, sbParameters, sbDataAdapterRows, method, returnObject, false, methodReturnType);
    }

    private void WriteDeleteMethod(StringBuilder sb)
    {
      StringBuilder sbDocComments = new StringBuilder();
      StringBuilder sbCodeBeforeUsing = new StringBuilder();
      StringBuilder sbParameters = new StringBuilder();
      StringBuilder sbDataAdapterRows = new StringBuilder();
      string method = "internal static void Delete(" + Functions.GetCommaDelimitedPrimaryKeysAndType(this._table, Language.CSharp) + ")";
      string empty = string.Empty;
      string returnType = "void";
      sbDocComments.AppendLine("         /// <summary>");
      sbDocComments.AppendLine("         /// Deletes a record based on primary key(s)");
      sbDocComments.AppendLine("         /// </summary>");
      string str = !this._isUseStoredProcedure ? "string dynamicSqlScript = " + this._table.Name + "Sql.Delete();" : "string storedProcName = \"" + Functions.GetStoredProcName(this._table, StoredProcName.Delete, this._spPrefixSuffixIndex, this._storedProcPrefix, this._storedProcSuffix) + "\";";
      sbCodeBeforeUsing.AppendLine("              " + str);
      sbCodeBeforeUsing.AppendLine("");
      sbParameters.AppendLine("");
      sbParameters.AppendLine("                      // parameters");
      foreach (Column primaryKeyColumn in (List<Column>) this._table.PrimaryKeyColumns)
        sbParameters.AppendLine("                      command.Parameters.AddWithValue(\"@" + primaryKeyColumn.NameCamelStyle + "\", " + primaryKeyColumn.NameCamelStyle + ");");
      sbDataAdapterRows.AppendLine("                      // execute");
      sbDataAdapterRows.AppendLine("                      command.ExecuteNonQuery();");
      this.WriteMethodWithUsingSqlConnection(sb, sbDocComments, sbCodeBeforeUsing, sbParameters, sbDataAdapterRows, method, empty, false, returnType);
    }

    private void WriteMethodWithUsingSqlConnection(StringBuilder sb, StringBuilder sbDocComments, StringBuilder sbCodeBeforeUsing, StringBuilder sbParameters, StringBuilder sbDataAdapterRows, string method, string returnObject, bool isUseDataAdapter = true, string returnType = "")
    {
      string str = "storedProcName";
      if (!this._isUseStoredProcedure)
        str = "dynamicSqlScript";
      sb.AppendLine("");
      sb.Append(sbDocComments.ToString());
      sb.AppendLine("         " + method);
      sb.AppendLine("         {");
      sb.Append(sbCodeBeforeUsing.ToString());
      sb.AppendLine("              using (SqlConnection connection = new SqlConnection(AppSettings.GetConnectionString()))");
      sb.AppendLine("              {");
      sb.AppendLine("                  connection.Open();");
      sb.AppendLine("");
      sb.AppendLine("                  using (SqlCommand command = new SqlCommand(" + str + ", connection))");
      sb.AppendLine("                  {");
      sb.AppendLine("                      " + this._commandTypeCode);
      if (method.Contains("GetRecordCountShared(") || method.Contains("SelectShared("))
      {
        sb.AppendLine("");
        sb.AppendLine("                      if (paramValue is null)");
        sb.AppendLine("                          paramValue = DBNull.Value;");
      }
      sb.Append(sbParameters.ToString());
      sb.AppendLine("");
      if (isUseDataAdapter)
      {
        sb.AppendLine("                      using (SqlDataAdapter da = new SqlDataAdapter(command))");
        sb.AppendLine("                      {");
        sb.AppendLine("                          DataTable dt = new DataTable();");
        sb.AppendLine("                          da.Fill(dt);");
        sb.AppendLine("");
        sb.AppendLine("                          if (dt != null)");
        sb.AppendLine("                          {");
        sb.AppendLine("                              if (dt.Rows.Count > 0)");
        sb.AppendLine("                              {");
        sb.Append(sbDataAdapterRows.ToString());
        sb.AppendLine("                              }");
        sb.AppendLine("                          }");
        sb.AppendLine("                      }");
      }
      else
        sb.Append(sbDataAdapterRows.ToString());
      sb.AppendLine("                  }");
      sb.AppendLine("              }");
      if (returnType != "void")
      {
        sb.AppendLine("");
        sb.AppendLine("              return " + returnObject + ";");
      }
      sb.AppendLine("         }");
    }
  }
}
