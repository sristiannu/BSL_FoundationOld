
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class DynamicSql
  {
    private Table _table;
    private Tables _selectedTables;
    private const string _fileExtension = ".cs";
    private string _apiName;
    private string _apiNameDirectory;
    private DatabaseObjectToGenerateFrom _generateFrom;
    private bool _isSqlVersion2012OrHigher;

    internal DynamicSql()
    {
    }

    internal DynamicSql(Table table, Tables selectedTables, string apiName, string apiNameDirectory, DatabaseObjectToGenerateFrom generateFrom, bool isSqlVersion2012OrHigher = false)
    {
      this._table = table;
      this._selectedTables = selectedTables;
      this._apiName = apiName;
      this._apiNameDirectory = apiNameDirectory + MyConstants.DirectoryDynamicSQL;
      this._generateFrom = generateFrom;
      this._isSqlVersion2012OrHigher = isSqlVersion2012OrHigher;
      this.Generate();
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._apiNameDirectory + this._table.Name + "Sql.cs"))
      {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("using System; ");
        sb.AppendLine("using System.Text; ");
        sb.AppendLine("");
        sb.AppendLine("namespace " + this._apiName + ".DataLayer.Base");
        sb.AppendLine("{ ");
        sb.AppendLine("    internal sealed class " + this._table.Name + "Sql ");
        sb.AppendLine("    { ");
        sb.AppendLine("        private " + this._table.Name + "Sql() ");
        sb.AppendLine("        { ");
        sb.AppendLine("        } ");
        this.WriteMethods(sb);
        sb.AppendLine("    } ");
        sb.AppendLine("} ");
        streamWriter.Write(sb.ToString());
      }
    }

    private void WriteMethods(StringBuilder sb)
    {
      if (this._generateFrom == DatabaseObjectToGenerateFrom.Tables)
      {
        if (!this._table.IsContainsPrimaryAndForeignKeyColumnsOnly)
          this.BuildSelectByPrimaryKey(sb);
        this.BuildGetRecordCount(sb);
        this.BuildGetRecordByCount(sb);
        this.BuildGetRecordCountDynamicWhere(sb);
        this.BuildSelectSkipAndTake(sb);
        this.BuildSelectSkipAndTakeBy(sb);
        this.BuildSelectSkipAndTakeDynamicWhereMethod(sb);
        if (this._table.IsContainsMoneyOrDecimalField)
          this.BuildSelectTotals(sb);
        this.BuildSelectAll(sb);
        this.BuildSelectAllBy(sb);
        this.BuildSelectAllDynamicWhere(sb);
        if (!this._table.IsContainsPrimaryAndForeignKeyColumnsOnly)
          this.BuildSelectDropDownListData(sb);
        this.BuildInsert(sb);
        this.BuildUpdate(sb);
        this.BuildDelete(sb);
        this.BuildSelectStatement(sb);
      }
      else
      {
        this.BuildGetRecordCount(sb);
        this.BuildSelectSkipAndTake(sb);
        this.BuildSelectStatement(sb);
      }
    }

    private void BuildSelectByPrimaryKey(StringBuilder sb)
    {
      string whereStatement = this.GetWhereStatement();
      sb.AppendLine("");
      sb.AppendLine("        internal static string SelectByPrimaryKey() ");
      sb.AppendLine("        { ");
      sb.AppendLine("            string selectStatement = GetSelectStatement(); ");
      sb.AppendLine("            StringBuilder sb = new StringBuilder(); ");
      sb.AppendLine("");
      sb.AppendLine("            sb.Append(selectStatement);");
      sb.Append(whereStatement);
      sb.AppendLine("            return sb.ToString(); ");
      sb.AppendLine("        } ");
    }

    private void BuildGetRecordCount(StringBuilder sb)
    {
      sb.AppendLine("");
      sb.AppendLine("        internal static string GetRecordCount() ");
      sb.AppendLine("        { ");
      sb.AppendLine("            StringBuilder sb = new StringBuilder(); ");
      sb.AppendLine("");
      sb.AppendLine("            sb.Append(\"SELECT COUNT(*) AS RecordCount FROM [" + this._table.OwnerOriginal + "].[" + this._table.NameOriginal + "]\");");
      sb.AppendLine("");
      sb.AppendLine("            return sb.ToString(); ");
      sb.AppendLine("        } ");
    }

    private void BuildGetRecordByCount(StringBuilder sb)
    {
      foreach (Column foreignKeyColumn in this._table.ForeignKeyColumns)
      {
        sb.AppendLine("");
        sb.AppendLine("        /// <summary>");
        sb.AppendLine("        /// Related to column " + foreignKeyColumn.Name);
        sb.AppendLine("        /// </summary>");
        sb.AppendLine("        internal static string GetRecordCountBy" + foreignKeyColumn.Name + "(" + foreignKeyColumn.SystemType + " " + foreignKeyColumn.NameCamelStyle + ")");
        sb.AppendLine("        { ");
        sb.AppendLine("            StringBuilder sb = new StringBuilder(); ");
        sb.AppendLine("");
        sb.AppendLine("            sb.Append(\"SELECT COUNT(*) AS RecordCount FROM [" + this._table.OwnerOriginal + "].[" + this._table.NameOriginal + "] \");");
        sb.AppendLine("            sb.Append(\"WHERE [" + foreignKeyColumn.NameOriginal + "] = @" + foreignKeyColumn.NameCamelStyle + " \");");
        sb.AppendLine("");
        sb.AppendLine("            return sb.ToString(); ");
        sb.AppendLine("        } ");
      }
    }

    private void BuildGetRecordCountDynamicWhere(StringBuilder sb)
    {
      sb.AppendLine("");
      sb.AppendLine("        internal static string GetRecordCountDynamicWhere() ");
      sb.AppendLine("        { ");
      sb.AppendLine("            StringBuilder sb = new StringBuilder(); ");
      sb.AppendLine("");
      sb.AppendLine("            sb.Append(\"SELECT COUNT(*) AS RecordCount FROM [" + this._table.OwnerOriginal + "].[" + this._table.NameOriginal + "]\");");
      sb.Append(this.GetWhereParameterStatementForDynamicWhere());
      sb.AppendLine("");
      sb.AppendLine("            return sb.ToString(); ");
      sb.AppendLine("        } ");
    }

    private void BuildSelectSkipAndTake(StringBuilder sb)
    {
      string str = !this._isSqlVersion2012OrHigher ? this.GetSelectSkipAndTakeStatement((Column) null) : this.GetSelectSkipAndTakeStatementForSQL2012OrHigher((Column) null);
      sb.AppendLine("");
      sb.AppendLine("        internal static string SelectSkipAndTake() ");
      sb.AppendLine("        { ");
      sb.AppendLine("            string selectStatement = GetSelectStatement(); ");
      sb.AppendLine("            StringBuilder sb = new StringBuilder(); ");
      sb.AppendLine("");
      if (this._isSqlVersion2012OrHigher)
      {
        sb.AppendLine("            sb.Append(\"DECLARE @numberOfRowsToSkip int = @start - 1; \");");
        sb.AppendLine("");
        sb.Append(str.ToString());
        sb.AppendLine("");
        sb.AppendLine("            sb.Append(\"OFFSET @numberOfRowsToSkip ROWS \");");
        sb.AppendLine("            sb.Append(\"FETCH NEXT @numberOfRows ROWS ONLY \");");
      }
      else
      {
        sb.AppendLine("            sb.Append(\"DECLARE @end int \");");
        sb.AppendLine("            sb.Append(\"SET @end = @start + @numberOfRows - 1; \");");
        sb.AppendLine("");
        sb.AppendLine("            sb.Append(\"WITH temporaryTableOnly AS \");");
        sb.AppendLine("            sb.Append(\"( \");");
        sb.Append(str.ToString());
        sb.AppendLine("            sb.Append(\") \");");
        sb.AppendLine("            sb.Append(\"SELECT * FROM temporaryTableOnly \");");
        sb.AppendLine("            sb.Append(\"WHERE RowNum BETWEEN @start AND @end \");");
      }
      sb.AppendLine("");
      sb.AppendLine("            return sb.ToString(); ");
      sb.AppendLine("        } ");
    }

    private void BuildSelectSkipAndTakeBy(StringBuilder sb)
    {
      foreach (Column foreignKeyColumn in this._table.ForeignKeyColumns)
      {
        string str = !this._isSqlVersion2012OrHigher ? this.GetSelectSkipAndTakeStatement(foreignKeyColumn) : this.GetSelectSkipAndTakeStatementForSQL2012OrHigher(foreignKeyColumn);
        sb.AppendLine("");
        sb.AppendLine("        internal static string SelectSkipAndTakeBy" + foreignKeyColumn.Name + "(" + foreignKeyColumn.SystemType + " " + foreignKeyColumn.NameCamelStyle + ") ");
        sb.AppendLine("        { ");
        sb.AppendLine("            string selectStatement = GetSelectStatement(); ");
        sb.AppendLine("            StringBuilder sb = new StringBuilder(); ");
        sb.AppendLine("");
        if (this._isSqlVersion2012OrHigher)
        {
          sb.AppendLine("            sb.Append(\"DECLARE @numberOfRowsToSkip int = @start - 1; \");");
          sb.AppendLine("");
          sb.Append(str.ToString());
          sb.AppendLine("");
          sb.AppendLine("            sb.Append(\"OFFSET @numberOfRowsToSkip ROWS \");");
          sb.AppendLine("            sb.Append(\"FETCH NEXT @numberOfRows ROWS ONLY \");");
        }
        else
        {
          sb.AppendLine("            sb.Append(\"DECLARE @end int \");");
          sb.AppendLine("            sb.Append(\"SET @end = @start + @numberOfRows - 1; \");");
          sb.AppendLine("");
          sb.AppendLine("            sb.Append(\"WITH temporaryTableOnly AS \");");
          sb.AppendLine("            sb.Append(\"( \");");
          sb.Append(str.ToString());
          sb.AppendLine("            sb.Append(\"WHERE [" + foreignKeyColumn.NameOriginal + "] = @" + foreignKeyColumn.NameCamelStyle + " \");");
          sb.AppendLine("            sb.Append(\") \");");
          sb.AppendLine("            sb.Append(\"SELECT * FROM temporaryTableOnly \");");
          sb.AppendLine("            sb.Append(\"WHERE RowNum BETWEEN @start AND @end \");");
        }
        sb.AppendLine("");
        sb.AppendLine("            return sb.ToString(); ");
        sb.AppendLine("        } ");
      }
    }

    private void BuildSelectSkipAndTakeDynamicWhereMethod(StringBuilder sb)
    {
      sb.AppendLine("");
      sb.AppendLine("        internal static string SelectSkipAndTakeDynamicWhere() ");
      sb.AppendLine("        { ");
      sb.AppendLine("            StringBuilder sb = new StringBuilder(); ");
      sb.AppendLine("");
      if (this._isSqlVersion2012OrHigher)
      {
        sb.AppendLine("            sb.Append(\"DECLARE @numberOfRowsToSkip int = @start - 1; \");");
        sb.AppendLine("");
        sb.Append(this.GetSelectStatement());
        sb.Append(this.GetWhereParameterStatementForDynamicWhere());
        sb.AppendLine("");
        sb.Append(this.GetOrderByCaseWhenStatement());
        sb.AppendLine("");
        sb.AppendLine("");
        sb.AppendLine("            sb.Append(\"OFFSET @numberOfRowsToSkip ROWS \");");
        sb.AppendLine("            sb.Append(\"FETCH NEXT @numberOfRows ROWS ONLY \");");
      }
      else
      {
        sb.AppendLine("            sb.Append(\"DECLARE @end int \");");
        sb.AppendLine("            sb.Append(\"SET @end = @start + @numberOfRows - 1; \");");
        sb.AppendLine("");
        sb.AppendLine("            sb.Append(\"WITH temporaryTableOnly AS \");");
        sb.AppendLine("            sb.Append(\"( \");");
        sb.Append(this.GetSelectNoFromWithRowNumberStatement());
        sb.AppendLine("            sb.Append(\"( \");");
        sb.Append(this.GetOrderByCaseWhenStatement());
        sb.AppendLine("");
        sb.AppendLine("            sb.Append(\") AS 'RowNum' \");");
        sb.AppendLine("            sb.Append(\"FROM [" + this._table.OwnerOriginal + "].[" + this._table.NameOriginal + "] \");");
        sb.Append(this.GetWhereParameterStatementForDynamicWhere());
        sb.AppendLine("            sb.Append(\") \");");
        sb.AppendLine("            sb.Append(\"SELECT * FROM temporaryTableOnly \");");
        sb.AppendLine("            sb.Append(\"WHERE RowNum BETWEEN @start AND @end \");");
      }
      sb.AppendLine("");
      sb.AppendLine("            return sb.ToString(); ");
      sb.AppendLine("        } ");
    }

    private void BuildSelectTotals(StringBuilder sb)
    {
      string statementForTotals = this.GetSelectStatementForTotals();
      sb.AppendLine("");
      sb.AppendLine("        internal static string SelectTotals() ");
      sb.AppendLine("        { ");
      sb.AppendLine("            string selectStatement = GetSelectStatement(); ");
      sb.AppendLine("            StringBuilder sb = new StringBuilder(); ");
      sb.AppendLine("");
      sb.Append(statementForTotals.ToString());
      sb.AppendLine("            return sb.ToString(); ");
      sb.AppendLine("        } ");
    }

    private void BuildSelectAll(StringBuilder sb)
    {
      sb.AppendLine("");
      sb.AppendLine("        internal static string SelectAll() ");
      sb.AppendLine("        { ");
      sb.AppendLine("            string selectStatement = GetSelectStatement(); ");
      sb.AppendLine("            StringBuilder sb = new StringBuilder(); ");
      sb.AppendLine("");
      sb.AppendLine("            sb.Append(selectStatement);");
      sb.AppendLine("");
      sb.AppendLine("            return sb.ToString(); ");
      sb.AppendLine("        } ");
    }

    private void BuildSelectAllBy(StringBuilder sb)
    {
      foreach (Column foreignKeyColumn in this._table.ForeignKeyColumns)
      {
        sb.AppendLine("");
        sb.AppendLine("        /// <summary>");
        sb.AppendLine("        /// Related to column " + foreignKeyColumn.Name);
        sb.AppendLine("        /// </summary>");
        sb.AppendLine("        internal static string SelectAllBy" + foreignKeyColumn.Name + "(" + foreignKeyColumn.SystemType + " " + foreignKeyColumn.NameCamelStyle + ")");
        sb.AppendLine("        { ");
        sb.AppendLine("            string selectStatement = GetSelectStatement(); ");
        sb.AppendLine("            StringBuilder sb = new StringBuilder(); ");
        sb.AppendLine("");
        sb.AppendLine("            sb.Append(selectStatement);");
        if (foreignKeyColumn.IsSqlDataMustBeEnclosedInApostrophe)
          sb.AppendLine("            sb.Append(\"WHERE [" + foreignKeyColumn.NameOriginal + "] = '\" + " + foreignKeyColumn.NameCamelStyle + " + \"'\");");
        else
          sb.AppendLine("            sb.Append(\"WHERE [" + foreignKeyColumn.NameOriginal + "] = \" + " + foreignKeyColumn.NameCamelStyle + ");");
        sb.AppendLine("");
        sb.AppendLine("            return sb.ToString(); ");
        sb.AppendLine("        } ");
      }
    }

    private void BuildSelectAllDynamicWhere(StringBuilder sb)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (Column spatialDataTypeColumn in this._table.NoneBinaryOrSpatialDataTypeColumns)
      {
        if (Functions.GetFieldType(spatialDataTypeColumn) == "String")
          stringBuilder.AppendLine("            sb.Append(\"([" + spatialDataTypeColumn.NameOriginal + "] LIKE '%' + @" + spatialDataTypeColumn.NameCamelStyle + " + '%' OR @" + spatialDataTypeColumn.NameCamelStyle + " IS NULL) AND \");");
        else
          stringBuilder.AppendLine("            sb.Append(\"([" + spatialDataTypeColumn.NameOriginal + "] = @" + spatialDataTypeColumn.NameCamelStyle + " OR @" + spatialDataTypeColumn.NameCamelStyle + " IS NULL) AND \");");
      }
      sb.AppendLine("");
      sb.AppendLine("        internal static string SelectAllDynamicWhere()");
      sb.AppendLine("        {");
      sb.AppendLine("            string selectStatement = GetSelectStatement();");
      sb.AppendLine("            StringBuilder sb = new StringBuilder();");
      sb.AppendLine("");
      sb.AppendLine("            sb.Append(selectStatement);");
      sb.AppendLine("            sb.Append(\" WHERE \");");
      sb.Append(Functions.RemoveLastAnd(stringBuilder.ToString()));
      sb.AppendLine("");
      sb.AppendLine("            return sb.ToString();");
      sb.AppendLine("        } ");
    }

    private void BuildSelectDropDownListData(StringBuilder sb)
    {
      sb.AppendLine("");
      sb.AppendLine("        /// <summary>");
      sb.AppendLine("        /// Selects " + this._table.FirstPrimaryKeyName + " and " + this._table.DataTextField + " columns for use with a DropDownList web control");
      sb.AppendLine("        /// </summary>");
      sb.AppendLine("        internal static string Select" + this._table.Name + MyConstants.WordDropDownListData + "()");
      sb.AppendLine("        { ");
      sb.AppendLine("            string selectStatement = \"SELECT [" + this._table.FirstPrimaryKeyName + "], [" + this._table.DataTextField + "] FROM [" + this._table.OwnerOriginal + "].[" + this._table.NameOriginal + "] ORDER BY [" + this._table.DataTextField + "] ASC \"; ");
      sb.AppendLine("            StringBuilder sb = new StringBuilder(); ");
      sb.AppendLine("");
      sb.AppendLine("            sb.Append(selectStatement);");
      sb.AppendLine("");
      sb.AppendLine("            return sb.ToString(); ");
      sb.AppendLine("        } ");
    }

    private void BuildInsert(StringBuilder sb)
    {
      Functions.GetCommaDelimitedInsertOrUdateMethodParamsWithSystemType(this._table, "c#");
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = new StringBuilder();
      foreach (Column column in (List<Column>) this._table.Columns)
      {
        if (!column.IsPrimaryKey && !column.IsComputed && (!column.IsIdentity && !column.IsUniqueIdWithNewId) && !column.IsDateWithGetDate || column.IsPrimaryKey && !column.IsPrimaryKeyUnique)
        {
          stringBuilder1.AppendLine("            sb.Append(\"[" + column.NameOriginal + "], \");");
          stringBuilder2.AppendLine("            sb.Append(\"@" + column.NameCamelStyle + ",\");");
        }
      }
      if (stringBuilder1.ToString().Contains(","))
        stringBuilder1.Replace(",", "", stringBuilder1.ToString().LastIndexOf(","), 1);
      if (stringBuilder2.ToString().Contains(","))
        stringBuilder2.Replace(",", "", stringBuilder2.ToString().LastIndexOf(","), 1);
      sb.AppendLine("");
      sb.AppendLine("        internal static string Insert()");
      sb.AppendLine("        { ");
      sb.AppendLine("            StringBuilder sb = new StringBuilder(); ");
      sb.AppendLine("");
      sb.AppendLine("            sb.Append(\"INSERT INTO [" + this._table.OwnerOriginal + "].[" + this._table.NameOriginal + "] \");");
      sb.AppendLine("            sb.Append(\"(\");");
      sb.Append(stringBuilder1.ToString());
      sb.AppendLine("            sb.Append(\") \");");
      if (this._table.PrimaryKeyCount == 1)
        sb.AppendLine("            sb.Append(\"OUTPUT inserted.[" + this._table.FirstPrimaryKeyNameOriginal + "] \");");
      sb.AppendLine("            sb.Append(\"VALUES \");");
      sb.AppendLine("            sb.Append(\"(\");");
      sb.Append(stringBuilder2.ToString());
      sb.AppendLine("            sb.Append(\")\");");
      sb.AppendLine("");
      sb.AppendLine("            return sb.ToString(); ");
      sb.AppendLine("        } ");
    }

    private void BuildUpdate(StringBuilder sb)
    {
      Functions.GetCommaDelimitedInsertOrUdateMethodParamsWithSystemType(this._table, "c#");
      string whereStatement = this.GetWhereStatement();
      StringBuilder stringBuilder = new StringBuilder();
      foreach (Column column in (List<Column>) this._table.Columns)
      {
        if (!column.IsPrimaryKey && !column.IsComputed && (!column.IsIdentity && !column.IsUniqueIdWithNewId) && !column.IsDateWithGetDate || column.IsPrimaryKey && !column.IsPrimaryKeyUnique)
          stringBuilder.AppendLine("            sb.Append(\"[" + column.NameOriginal + "] = @" + column.NameCamelStyle + ",\");");
      }
      if (stringBuilder.ToString().Contains(","))
        stringBuilder.Replace(",", "", stringBuilder.ToString().LastIndexOf(","), 1);
      sb.AppendLine("");
      sb.AppendLine("        internal static string Update()");
      sb.AppendLine("        { ");
      sb.AppendLine("            StringBuilder sb = new StringBuilder(); ");
      sb.AppendLine("");
      sb.AppendLine("            sb.Append(\"UPDATE [" + this._table.OwnerOriginal + "].[" + this._table.NameOriginal + "] \");");
      sb.AppendLine("            sb.Append(\"SET \");");
      sb.Append(stringBuilder.ToString());
      sb.Append(whereStatement);
      sb.AppendLine("            return sb.ToString(); ");
      sb.AppendLine("        } ");
    }

    private void BuildDelete(StringBuilder sb)
    {
      string whereStatement = this.GetWhereStatement();
      sb.AppendLine("");
      sb.AppendLine("        internal static string Delete() ");
      sb.AppendLine("        { ");
      sb.AppendLine("            StringBuilder sb = new StringBuilder(); ");
      sb.AppendLine("");
      sb.AppendLine("            sb.Append(\"DELETE FROM [" + this._table.OwnerOriginal + "].[" + this._table.NameOriginal + "] \");");
      sb.Append(whereStatement);
      sb.AppendLine("            return sb.ToString(); ");
      sb.AppendLine("        } ");
    }

    private void BuildSelectStatement(StringBuilder sb)
    {
      string selectStatement = this.GetSelectStatement();
      sb.AppendLine("");
      sb.AppendLine("        private static string GetSelectStatement() ");
      sb.AppendLine("        { ");
      sb.AppendLine("            StringBuilder sb = new StringBuilder(); ");
      sb.AppendLine("");
      sb.AppendLine(selectStatement);
      sb.AppendLine("");
      sb.AppendLine("            return sb.ToString(); ");
      sb.AppendLine("        } ");
    }

    private string GetSelectStatement()
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = new StringBuilder();
      stringBuilder1.AppendLine("            sb.Append(\"SELECT \");");
      foreach (Column column in (List<Column>) this._table.Columns)
      {
        string str = string.Empty;
        if (column.NameOriginal != column.Name)
          str = " AS [" + column.Name + "]";
        stringBuilder2.AppendLine("            sb.Append(\"[" + column.NameOriginal + "]" + str + ", \");");
      }
      stringBuilder2.Replace(",", "", stringBuilder2.ToString().LastIndexOf(","), 1);
      stringBuilder1.Append(stringBuilder2.ToString());
      stringBuilder1.Append("            sb.Append(\"FROM [" + this._table.OwnerOriginal + "].[" + this._table.NameOriginal + "] \");");
      return stringBuilder1.ToString();
    }

    private string GetSelectStatementForTotals()
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = new StringBuilder();
      stringBuilder1.AppendLine("            sb.Append(\"SELECT \");");
      foreach (Column column in (List<Column>) this._table.Columns)
      {
        if (column.IsMoneyOrDecimalField)
          stringBuilder2.AppendLine("            sb.Append(\"SUM([" + column.NameOriginal + "]) AS [" + column.Name + MyConstants.WordTotal + "], \");");
      }
      stringBuilder1.Append(Functions.RemoveTheVeryLastComma(stringBuilder2.ToString()));
      stringBuilder1.AppendLine("");
      stringBuilder1.AppendLine("            sb.Append(\"FROM [" + this._table.OwnerOriginal + "].[" + this._table.NameOriginal + "]\");");
      return stringBuilder1.ToString();
    }

    private string GetSelectSkipAndTakeStatement(Column fkColumn = null)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = new StringBuilder();
      StringBuilder stringBuilder3 = new StringBuilder();
      stringBuilder1.AppendLine("            sb.Append(\"SELECT \");");
      foreach (Column column in (List<Column>) this._table.Columns)
      {
        string str = string.Empty;
        if (column.NameOriginal != column.Name)
          str = " AS [" + column.Name + "]";
        stringBuilder2.AppendLine("            sb.Append(\"[" + column.NameOriginal + "]" + str + ", \");");
      }
      stringBuilder2.AppendLine("            sb.Append(\"ROW_NUMBER() OVER \");");
      stringBuilder1.Append(stringBuilder2.ToString());
      stringBuilder1.AppendLine("            sb.Append(\"(\");");
      stringBuilder1.AppendLine("            sb.Append(\"ORDER BY \");");
      foreach (Column spatialDataTypeColumn in this._table.NoneBinaryOrSpatialDataTypeColumns)
      {
        stringBuilder3.AppendLine("\t        sb.Append(\"CASE WHEN @sortByExpression = '" + spatialDataTypeColumn.Name + "' THEN [" + spatialDataTypeColumn.NameOriginal + "] END, \");");
        stringBuilder3.AppendLine("\t        sb.Append(\"CASE WHEN @sortByExpression = '" + spatialDataTypeColumn.Name + " desc' THEN [" + spatialDataTypeColumn.NameOriginal + "] END DESC, \");");
        stringBuilder3.AppendLine("");
      }
      stringBuilder1.Append(Functions.RemoveTheVeryLastComma(stringBuilder3.ToString()));
      stringBuilder1.AppendLine("");
      stringBuilder1.AppendLine("            sb.Append(\") AS 'RowNum' \");");
      stringBuilder1.AppendLine("            sb.Append(\"FROM [" + this._table.OwnerOriginal + "].[" + this._table.NameOriginal + "] \");");
      return stringBuilder1.ToString();
    }

    private string GetSelectSkipAndTakeStatementForSQL2012OrHigher(Column fkColumn = null)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = new StringBuilder();
      StringBuilder stringBuilder3 = new StringBuilder();
      stringBuilder1.AppendLine("            sb.Append(\"SELECT \");");
      foreach (Column column in (List<Column>) this._table.Columns)
      {
        string str = string.Empty;
        if (column.NameOriginal != column.Name)
          str = " AS [" + column.Name + "]";
        stringBuilder2.AppendLine("            sb.Append(\"[" + column.NameOriginal + "]" + str + ", \");");
      }
      stringBuilder1.Append(Functions.RemoveTheVeryLastComma(stringBuilder2.ToString()));
      stringBuilder1.AppendLine("");
      stringBuilder1.AppendLine("            sb.Append(\"FROM [" + this._table.OwnerOriginal + "].[" + this._table.NameOriginal + "] \");");
      if (fkColumn != null)
        stringBuilder1.AppendLine("            sb.Append(\"WHERE [" + fkColumn.NameOriginal + "] = @" + fkColumn.NameCamelStyle + " \");");
      stringBuilder1.AppendLine("            sb.Append(\"ORDER BY \");");
      foreach (Column spatialDataTypeColumn in this._table.NoneBinaryOrSpatialDataTypeColumns)
      {
        stringBuilder3.AppendLine("\t        sb.Append(\"CASE WHEN @sortByExpression = '" + spatialDataTypeColumn.Name + "' THEN [" + spatialDataTypeColumn.NameOriginal + "] END, \");");
        stringBuilder3.AppendLine("\t        sb.Append(\"CASE WHEN @sortByExpression = '" + spatialDataTypeColumn.Name + " desc' THEN [" + spatialDataTypeColumn.NameOriginal + "] END DESC, \");");
        stringBuilder3.AppendLine("");
      }
      stringBuilder1.Append(Functions.RemoveTheVeryLastComma(stringBuilder3.ToString()));
      stringBuilder1.AppendLine("");
      return stringBuilder1.ToString();
    }

    private string GetWhereStatement()
    {
      int num = 0;
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = new StringBuilder();
      foreach (Column primaryKeyColumn in (List<Column>) this._table.PrimaryKeyColumns)
      {
        if (num > 0)
        {
          stringBuilder2.AppendLine("");
          stringBuilder2.AppendLine("            sb.Append(\" AND \");");
        }
        stringBuilder2.Append("            sb.Append(\"[" + primaryKeyColumn.NameOriginal + "] = @" + primaryKeyColumn.NameCamelStyle + " \");");
        ++num;
      }
      stringBuilder1.AppendLine("            sb.Append(\" WHERE \"); ");
      stringBuilder1.AppendLine(stringBuilder2.ToString());
      stringBuilder1.AppendLine("");
      return stringBuilder1.ToString();
    }

    private string GetWhereParameterStatementForDynamicWhere()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("");
      stringBuilder.AppendLine("            sb.Append(\" WHERE \");");
      foreach (Column spatialDataTypeColumn in this._table.NoneBinaryOrSpatialDataTypeColumns)
      {
        if (Functions.GetFieldType(spatialDataTypeColumn) == "String")
          stringBuilder.AppendLine("            sb.Append(\"([" + spatialDataTypeColumn.NameOriginal + "] LIKE '%' + @" + spatialDataTypeColumn.NameCamelStyle + " + '%' OR @" + spatialDataTypeColumn.NameCamelStyle + " IS NULL) AND \");");
        else
          stringBuilder.AppendLine("            sb.Append(\"([" + spatialDataTypeColumn.NameOriginal + "] = @" + spatialDataTypeColumn.NameCamelStyle + " OR @" + spatialDataTypeColumn.NameCamelStyle + " IS NULL) AND \");");
      }
      return Functions.RemoveLastAnd(stringBuilder.ToString());
    }

    private string GetOrderByCaseWhenStatement()
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = new StringBuilder();
      stringBuilder1.AppendLine("            sb.Append(\"ORDER BY \");");
      foreach (Column spatialDataTypeColumn in this._table.NoneBinaryOrSpatialDataTypeColumns)
      {
        stringBuilder2.AppendLine("\t        sb.Append(\"CASE WHEN @sortByExpression = '" + spatialDataTypeColumn.Name + "' THEN [" + spatialDataTypeColumn.NameOriginal + "] END, \");");
        stringBuilder2.AppendLine("\t        sb.Append(\"CASE WHEN @sortByExpression = '" + spatialDataTypeColumn.Name + " desc' THEN [" + spatialDataTypeColumn.NameOriginal + "] END DESC, \");");
        stringBuilder2.AppendLine("");
      }
      stringBuilder1.Append(Functions.RemoveTheVeryLastComma(stringBuilder2.ToString()));
      return stringBuilder1.ToString();
    }

    private string GetSelectNoFromWithRowNumberStatement()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("            sb.Append(\"SELECT \");");
      foreach (Column column in (List<Column>) this._table.Columns)
      {
        string str = string.Empty;
        if (column.NameOriginal != column.Name)
          str = " AS [" + column.Name + "]";
        stringBuilder.AppendLine("            sb.Append(\"[" + column.NameOriginal + "]" + str + ", \");");
      }
      stringBuilder.AppendLine("            sb.Append(\"ROW_NUMBER() OVER \");");
      return stringBuilder.ToString();
    }
  }
}
