
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class StoredProcedure
  {
    private Table _table;
    private string _apiNameDirectory;
    private string _path;
    private bool _isCreateStoredProcInDbase;
    private Dbase _dbase;
    private string _connectionString;
    private int _spPrefixSuffixIndex;
    private string _storedProcPrefix;
    private string _storedProcSuffix;
    private DatabaseObjectToGenerateFrom _generateFrom;
    private string _storedProcedureErrorFilePath;
    private bool _isSqlVersion2012OrHigher;

    internal StoredProcedure()
    {
    }

    internal StoredProcedure(Table table, string apiNameDirectory, string connectionString, bool isCreateStoredProcInDbase, int spPrefixSuffixIndex, string storedProcPrefix, string storedProcSuffix, DatabaseObjectToGenerateFrom generateFrom, string storedProcedureErrorFilePath, bool isSqlVersion2012OrHigher = false)
    {
      this._table = table;
      this._apiNameDirectory = apiNameDirectory + MyConstants.DirectoryDynamicSQL;
      this._path = apiNameDirectory;
      this._connectionString = connectionString;
      this._dbase = new Dbase(connectionString, apiNameDirectory);
      this._isCreateStoredProcInDbase = isCreateStoredProcInDbase;
      this._spPrefixSuffixIndex = spPrefixSuffixIndex;
      this._storedProcPrefix = storedProcPrefix;
      this._storedProcSuffix = storedProcSuffix;
      this._generateFrom = generateFrom;
      this._storedProcedureErrorFilePath = storedProcedureErrorFilePath;
      this._isSqlVersion2012OrHigher = isSqlVersion2012OrHigher;
      this.Generate();
    }

    private void Generate()
    {
      if (this._generateFrom == DatabaseObjectToGenerateFrom.Tables)
      {
        if (this._isCreateStoredProcInDbase)
        {
          if (!this._table.IsContainsPrimaryAndForeignKeyColumnsOnly)
            this.BuildSelectByPrimaryKey(this._table);
          this.BuildGetRecordCount(this._table);
          this.BuildGetRecordByCount(this._table);
          this.BuildGetRecordCountDynamicWhere(this._table);
          this.BuildSelectSkipAndTake(this._table);
          this.BuildSelectSkipAndTakeBy(this._table);
          this.BuildSelectSkipAndTakeDynamicWhereMethod(this._table);
          if (this._table.IsContainsMoneyOrDecimalField)
            this.BuildSelectTotals(this._table);
          this.BuildSelectAll(this._table);
          this.BuildSelectAllWhereDynamic(this._table);
          this.BuildSelectAllBy(this._table);
          this.BuildInsert(this._table);
          if (!this._table.IsContainsPrimaryAndForeignKeyColumnsOnly)
            this.BuildUpdate(this._table);
          this.BuildDelete(this._table);
          if (this._table.IsContainsPrimaryAndForeignKeyColumnsOnly)
            return;
          this.BuildSelectDropDownListData(this._table);
        }
        else
        {
          using (StreamWriter streamWriter = new StreamWriter(this._apiNameDirectory + this._table.Name + ".sql"))
          {
            StringBuilder stringBuilder = new StringBuilder();
            if (!this._table.IsContainsPrimaryAndForeignKeyColumnsOnly)
              stringBuilder.Append(this.BuildSelectByPrimaryKey(this._table));
            stringBuilder.Append(this.BuildGetRecordCount(this._table));
            stringBuilder.Append(this.BuildGetRecordByCount(this._table));
            stringBuilder.Append(this.BuildSelectSkipAndTake(this._table));
            stringBuilder.Append(this.BuildSelectSkipAndTakeBy(this._table));
            stringBuilder.Append(this.BuildSelectAll(this._table));
            stringBuilder.Append(this.BuildSelectAllBy(this._table));
            stringBuilder.Append(this.BuildInsert(this._table));
            if (!this._table.IsContainsPrimaryAndForeignKeyColumnsOnly)
              stringBuilder.Append(this.BuildUpdate(this._table));
            stringBuilder.Append(this.BuildDelete(this._table));
            streamWriter.Write(stringBuilder.ToString());
          }
        }
      }
      else if (this._isCreateStoredProcInDbase)
      {
        this.BuildSelectAll(this._table);
        this.BuildGetRecordCount(this._table);
        this.BuildSelectSkipAndTake(this._table);
      }
      else
      {
        using (StreamWriter streamWriter = new StreamWriter(this._apiNameDirectory + this._table.Name + ".sql"))
        {
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.Append(this.BuildSelectAll(this._table));
          stringBuilder.Append(this.BuildGetRecordCount(this._table));
          stringBuilder.Append(this.BuildSelectSkipAndTake(this._table));
          streamWriter.Write(stringBuilder.ToString());
        }
      }
    }

    private string BuildSelectByPrimaryKey(Table table)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      string selectStatement = this.GetSelectStatement(table);
      int num = 0;
      StringBuilder stringBuilder2 = new StringBuilder();
      StringBuilder stringBuilder3 = new StringBuilder();
      string str = string.Empty;
      foreach (Column primaryKeyColumn in this._table.PrimaryKeyColumns)
      {
        if (num > 0)
        {
          stringBuilder3.AppendLine(" AND ");
          str = "  ";
        }
        stringBuilder2.AppendLine(str + " @" + primaryKeyColumn.NameCamelStyle + " " + primaryKeyColumn.StoredProcParameter + ",");
        stringBuilder3.Append(str + "[" + primaryKeyColumn.NameOriginal + "] = @" + primaryKeyColumn.NameCamelStyle);
        ++num;
      }
      string storedProcName = Functions.GetStoredProcName(table, StoredProcName.SelectByPrimaryKey, this._spPrefixSuffixIndex, this._storedProcPrefix, this._storedProcSuffix);
      stringBuilder1.AppendLine("CREATE PROCEDURE " + storedProcName);
      stringBuilder1.AppendLine("( ");
      stringBuilder1.AppendLine("  " + Functions.RemoveLastComma(stringBuilder2.ToString()));
      stringBuilder1.AppendLine(")");
      stringBuilder1.AppendLine("AS");
      stringBuilder1.AppendLine("BEGIN");
      stringBuilder1.AppendLine("  SET NOCOUNT ON;");
      stringBuilder1.AppendLine(" ");
      stringBuilder1.AppendLine("  " + selectStatement);
      stringBuilder1.AppendLine("  WHERE " + stringBuilder3.ToString());
      stringBuilder1.AppendLine("END");
      if (this._isCreateStoredProcInDbase)
      {
        this._dbase.CreateStoredProcedure(stringBuilder1.ToString(), storedProcName, this._storedProcedureErrorFilePath);
        return null;
      }
      stringBuilder1.AppendLine("GO");
      stringBuilder1.AppendLine("");
      return stringBuilder1.ToString();
    }

    private string BuildGetRecordCount(Table table)
    {
      StringBuilder stringBuilder = new StringBuilder();
      string storedProcName = Functions.GetStoredProcName(this._table, StoredProcName.GetRecordCount, this._spPrefixSuffixIndex, this._storedProcPrefix, this._storedProcSuffix);
      stringBuilder.AppendLine("CREATE PROCEDURE " + storedProcName);
      stringBuilder.AppendLine("AS");
      stringBuilder.AppendLine("BEGIN");
      stringBuilder.AppendLine("  SET NOCOUNT ON;");
      stringBuilder.AppendLine(" ");
      stringBuilder.AppendLine("  SELECT COUNT(*) AS RecordCount FROM [" + this._table.OwnerOriginal + "].[" + this._table.NameOriginal + "]");
      stringBuilder.AppendLine("END");
      if (this._isCreateStoredProcInDbase)
      {
        this._dbase.CreateStoredProcedure(stringBuilder.ToString(), storedProcName, this._storedProcedureErrorFilePath);
        return null;
      }
      stringBuilder.AppendLine("GO");
      stringBuilder.AppendLine("");
      return stringBuilder.ToString();
    }

    private string BuildGetRecordByCount(Table table)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      foreach (Column foreignKeyColumn in this._table.ForeignKeyColumns)
      {
        string storedProcNameBy = Functions.GetStoredProcNameBy(table, foreignKeyColumn, this._spPrefixSuffixIndex, this._storedProcPrefix, this._storedProcSuffix, StoredProcName.GetRecordCountBy);
        StringBuilder stringBuilder2 = new StringBuilder();
        stringBuilder2.AppendLine("CREATE PROCEDURE " + storedProcNameBy);
        stringBuilder2.AppendLine("(");
        stringBuilder2.AppendLine("   @" + foreignKeyColumn.NameCamelStyle + " " + foreignKeyColumn.StoredProcParameter);
        stringBuilder2.AppendLine(")");
        stringBuilder2.AppendLine("AS");
        stringBuilder2.AppendLine("BEGIN");
        stringBuilder2.AppendLine("  SET NOCOUNT ON;");
        stringBuilder2.AppendLine(" ");
        stringBuilder2.AppendLine("  SELECT COUNT(*) AS RecordCount FROM [" + this._table.OwnerOriginal + "].[" + this._table.NameOriginal + "]");
        stringBuilder2.AppendLine("  WHERE [" + foreignKeyColumn.NameOriginal + "] = @" + foreignKeyColumn.NameCamelStyle);
        stringBuilder2.AppendLine("END");
        if (this._isCreateStoredProcInDbase)
        {
          this._dbase.CreateStoredProcedure(stringBuilder2.ToString(), storedProcNameBy, this._storedProcedureErrorFilePath);
        }
        else
        {
          stringBuilder2.AppendLine("GO");
          stringBuilder2.AppendLine("");
          stringBuilder1.Append(stringBuilder2.ToString());
        }
      }
      if (this._isCreateStoredProcInDbase)
        return null;
      return stringBuilder1.ToString();
    }

    private string BuildGetRecordCountDynamicWhere(Table table)
    {
      StringBuilder stringBuilder = new StringBuilder();
      string storedProcName = Functions.GetStoredProcName(table, StoredProcName.GetRecordCountWhereDynamic, this._spPrefixSuffixIndex, this._storedProcPrefix, this._storedProcSuffix);
      stringBuilder.AppendLine("CREATE PROCEDURE " + storedProcName);
      stringBuilder.AppendLine("(");
      stringBuilder.AppendLine("   " + this.GetPassedParametersForDynamicWhere(table));
      stringBuilder.AppendLine(")");
      stringBuilder.AppendLine("AS");
      stringBuilder.AppendLine("BEGIN");
      stringBuilder.AppendLine("  SET NOCOUNT ON;");
      stringBuilder.AppendLine(" ");
      stringBuilder.AppendLine("  SELECT COUNT(*) AS RecordCount FROM [" + table.OwnerOriginal + "].[" + table.NameOriginal + "]");
      stringBuilder.AppendLine("  " + this.GetWhereParameterStatementForDynamicWhere(table));
      stringBuilder.AppendLine("END");
      if (this._isCreateStoredProcInDbase)
      {
        this._dbase.CreateStoredProcedure(stringBuilder.ToString(), storedProcName, this._storedProcedureErrorFilePath);
        return null;
      }
      stringBuilder.AppendLine("GO");
      stringBuilder.AppendLine("");
      return stringBuilder.ToString();
    }

    private string BuildSelectSkipAndTake(Table table)
    {
      StringBuilder stringBuilder = new StringBuilder();
      string storedProcName = Functions.GetStoredProcName(table, StoredProcName.SelectSkipAndTake, this._spPrefixSuffixIndex, this._storedProcPrefix, this._storedProcSuffix);
      string str = !this._isSqlVersion2012OrHigher ? this.GetSelectSkipAndTakeSelectStatement(table, null) : this.GetSelectSkipAndTakeSelectStatementForSQL2012OrHigher(table, (Column) null);
      stringBuilder.AppendLine("CREATE PROCEDURE " + storedProcName);
      stringBuilder.AppendLine("(");
      stringBuilder.AppendLine("    @start int,");
      stringBuilder.AppendLine("    @numberOfRows int,");
      stringBuilder.AppendLine("    @sortByExpression varchar(200)");
      stringBuilder.AppendLine(")");
      stringBuilder.AppendLine("AS");
      stringBuilder.AppendLine("BEGIN");
      stringBuilder.AppendLine("  SET NOCOUNT ON;");
      if (this._isSqlVersion2012OrHigher)
      {
        stringBuilder.AppendLine("  DECLARE @numberOfRowsToSkip int = @start - 1;");
        stringBuilder.AppendLine("");
        stringBuilder.Append(str.ToString());
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("  OFFSET @numberOfRowsToSkip ROWS");
        stringBuilder.AppendLine("  FETCH NEXT @numberOfRows ROWS ONLY");
      }
      else
      {
        stringBuilder.AppendLine("  DECLARE @end int");
        stringBuilder.AppendLine("  SET @end = @start + @numberOfRows - 1;");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("  WITH temporaryTableOnly AS");
        stringBuilder.AppendLine("  (");
        stringBuilder.Append(str.ToString());
        stringBuilder.AppendLine("  )");
        stringBuilder.AppendLine("  SELECT * FROM temporaryTableOnly");
        stringBuilder.AppendLine("  WHERE RowNum BETWEEN @start AND @end");
      }
      stringBuilder.AppendLine("END");
      if (this._isCreateStoredProcInDbase)
      {
        this._dbase.CreateStoredProcedure(stringBuilder.ToString(), storedProcName, this._storedProcedureErrorFilePath);
        return null;
      }
      stringBuilder.AppendLine("GO");
      stringBuilder.AppendLine("");
      return stringBuilder.ToString();
    }

    private string BuildSelectSkipAndTakeBy(Table table)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      foreach (Column foreignKeyColumn in this._table.ForeignKeyColumns)
      {
        StringBuilder stringBuilder2 = new StringBuilder();
        string storedProcNameBy = Functions.GetStoredProcNameBy(table, foreignKeyColumn, this._spPrefixSuffixIndex, this._storedProcPrefix, this._storedProcSuffix, StoredProcName.SelectSkipAndTakeBy);
        string str = !this._isSqlVersion2012OrHigher ? this.GetSelectSkipAndTakeSelectStatement(table, foreignKeyColumn) : this.GetSelectSkipAndTakeSelectStatementForSQL2012OrHigher(table, foreignKeyColumn);
        stringBuilder2.AppendLine("CREATE PROCEDURE " + storedProcNameBy);
        stringBuilder2.AppendLine("(");
        stringBuilder2.AppendLine("    @" + foreignKeyColumn.NameCamelStyle + " " + foreignKeyColumn.StoredProcParameter + ",");
        stringBuilder2.AppendLine("    @start int,");
        stringBuilder2.AppendLine("    @numberOfRows int,");
        stringBuilder2.AppendLine("    @sortByExpression varchar(200)");
        stringBuilder2.AppendLine(")");
        stringBuilder2.AppendLine("AS");
        stringBuilder2.AppendLine("BEGIN");
        stringBuilder2.AppendLine("  SET NOCOUNT ON;");
        if (this._isSqlVersion2012OrHigher)
        {
          stringBuilder2.AppendLine("  DECLARE @numberOfRowsToSkip int = @start - 1;");
          stringBuilder2.AppendLine("");
          stringBuilder2.Append(str.ToString());
          stringBuilder2.AppendLine("");
          stringBuilder2.AppendLine("  OFFSET @numberOfRowsToSkip ROWS");
          stringBuilder2.AppendLine("  FETCH NEXT @numberOfRows ROWS ONLY");
        }
        else
        {
          stringBuilder2.AppendLine("  DECLARE @end int");
          stringBuilder2.AppendLine("  SET @end = @start + @numberOfRows - 1;");
          stringBuilder2.AppendLine("");
          stringBuilder2.AppendLine("  WITH temporaryTableOnly AS");
          stringBuilder2.AppendLine("  (");
          stringBuilder2.Append(str.ToString());
          stringBuilder2.AppendLine("  )");
          stringBuilder2.AppendLine("  SELECT * FROM temporaryTableOnly");
          stringBuilder2.AppendLine("  WHERE RowNum BETWEEN @start AND @end");
        }
        stringBuilder2.AppendLine("END");
        if (this._isCreateStoredProcInDbase)
        {
          this._dbase.CreateStoredProcedure(stringBuilder2.ToString(), storedProcNameBy, this._storedProcedureErrorFilePath);
        }
        else
        {
          stringBuilder2.AppendLine("GO");
          stringBuilder2.AppendLine("");
          stringBuilder1.Append(stringBuilder2.ToString());
        }
      }
      if (this._isCreateStoredProcInDbase)
        return null;
      return stringBuilder1.ToString();
    }

    private string BuildSelectSkipAndTakeDynamicWhereMethod(Table table)
    {
      StringBuilder stringBuilder = new StringBuilder();
      string storedProcName = Functions.GetStoredProcName(table, StoredProcName.SelectSkipAndTakeWhereDynamic, this._spPrefixSuffixIndex, this._storedProcPrefix, this._storedProcSuffix);
      stringBuilder.AppendLine("CREATE PROCEDURE " + storedProcName);
      stringBuilder.AppendLine("(");
      stringBuilder.AppendLine("   @start int,");
      stringBuilder.AppendLine("   @numberOfRows int,");
      stringBuilder.AppendLine("   @sortByExpression varchar(200),");
      stringBuilder.AppendLine("   " + this.GetPassedParametersForDynamicWhere(table));
      stringBuilder.AppendLine(")");
      stringBuilder.AppendLine("AS");
      stringBuilder.AppendLine("BEGIN");
      stringBuilder.AppendLine("  SET NOCOUNT ON;");
      if (this._isSqlVersion2012OrHigher)
      {
        stringBuilder.AppendLine("  DECLARE @numberOfRowsToSkip int = @start - 1;");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("  " + this.GetSelectStatement(table));
        stringBuilder.Append(this.GetWhereParameterStatementForDynamicWhere(table));
        stringBuilder.AppendLine("");
        stringBuilder.Append(this.GetOrderByCaseWhenStatement(table, false));
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("  OFFSET @numberOfRowsToSkip ROWS ");
        stringBuilder.AppendLine("  FETCH NEXT @numberOfRows ROWS ONLY ");
      }
      else
      {
        stringBuilder.AppendLine("  DECLARE @end int");
        stringBuilder.AppendLine("  SET @end = @start + @numberOfRows - 1;");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("  WITH temporaryTableOnly AS");
        stringBuilder.AppendLine("  (");
        stringBuilder.Append(this.GetSelectNoFromWithRowNumberStatement(table));
        stringBuilder.AppendLine("\t    (");
        stringBuilder.Append(this.GetOrderByCaseWhenStatement(table, true));
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("\t  ) AS 'RowNum'");
        stringBuilder.AppendLine("     FROM [" + this._table.OwnerOriginal + "].[" + this._table.NameOriginal + "]");
        stringBuilder.Append(this.GetWhereParameterStatementForDynamicWhere(table));
        stringBuilder.AppendLine("  )");
        stringBuilder.AppendLine("  SELECT * FROM temporaryTableOnly");
        stringBuilder.AppendLine("  WHERE RowNum BETWEEN @start AND @end");
      }
      stringBuilder.AppendLine("END");
      if (this._isCreateStoredProcInDbase)
      {
        this._dbase.CreateStoredProcedure(stringBuilder.ToString(), storedProcName, this._storedProcedureErrorFilePath);
        return null;
      }
      stringBuilder.AppendLine("GO");
      stringBuilder.AppendLine("");
      return stringBuilder.ToString();
    }

    private string BuildSelectTotals(Table table)
    {
      StringBuilder stringBuilder = new StringBuilder();
      string statementForTotals = this.GetSelectStatementForTotals(table);
      string storedProcName = Functions.GetStoredProcName(table, StoredProcName.SelectTotals, this._spPrefixSuffixIndex, this._storedProcPrefix, this._storedProcSuffix);
      stringBuilder.AppendLine("CREATE PROCEDURE " + storedProcName);
      stringBuilder.AppendLine("AS");
      stringBuilder.AppendLine("BEGIN");
      stringBuilder.AppendLine("  SET NOCOUNT ON;");
      stringBuilder.AppendLine(" ");
      stringBuilder.AppendLine("  " + statementForTotals);
      stringBuilder.AppendLine("END");
      if (this._isCreateStoredProcInDbase)
      {
        this._dbase.CreateStoredProcedure(stringBuilder.ToString(), storedProcName, this._storedProcedureErrorFilePath);
        return null;
      }
      stringBuilder.AppendLine("GO");
      stringBuilder.AppendLine("");
      return stringBuilder.ToString();
    }

    private string BuildSelectAll(Table table)
    {
      StringBuilder stringBuilder = new StringBuilder();
      string selectStatement = this.GetSelectStatement(table);
      string storedProcName = Functions.GetStoredProcName(table, StoredProcName.SelectAll, this._spPrefixSuffixIndex, this._storedProcPrefix, this._storedProcSuffix);
      stringBuilder.AppendLine("CREATE PROCEDURE " + storedProcName);
      stringBuilder.AppendLine("AS");
      stringBuilder.AppendLine("BEGIN");
      stringBuilder.AppendLine("  SET NOCOUNT ON;");
      stringBuilder.AppendLine(" ");
      stringBuilder.AppendLine("  " + selectStatement);
      stringBuilder.AppendLine("END");
      if (this._isCreateStoredProcInDbase)
      {
        this._dbase.CreateStoredProcedure(stringBuilder.ToString(), storedProcName, this._storedProcedureErrorFilePath);
        return null;
      }
      stringBuilder.AppendLine("GO");
      stringBuilder.AppendLine("");
      return stringBuilder.ToString();
    }

    private string BuildSelectAllWhereDynamic(Table table)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = new StringBuilder();
      string selectStatement = this.GetSelectStatement(table);
      string storedProcName = Functions.GetStoredProcName(table, StoredProcName.SelectAllWhereDynamic, this._spPrefixSuffixIndex, this._storedProcPrefix, this._storedProcSuffix);
      foreach (Column spatialDataTypeColumn in this._table.NoneBinaryOrSpatialDataTypeColumns)
      {
        Functions.GetFieldType(spatialDataTypeColumn);
        stringBuilder2.AppendLine("   @" + spatialDataTypeColumn.NameCamelStyle + " " + spatialDataTypeColumn.StoredProcParameter + " = NULL,");
      }
      stringBuilder1.AppendLine("CREATE PROCEDURE " + storedProcName);
      stringBuilder1.AppendLine("(");
      stringBuilder1.AppendLine("   " + Functions.RemoveLastComma(stringBuilder2.ToString()));
      stringBuilder1.AppendLine(")");
      stringBuilder1.AppendLine("AS");
      stringBuilder1.AppendLine("BEGIN");
      stringBuilder1.AppendLine("  SET NOCOUNT ON;");
      stringBuilder1.AppendLine(" ");
      stringBuilder1.AppendLine("  " + selectStatement);
      stringBuilder1.AppendLine("  " + this.GetWhereParameterStatementForDynamicWhere(table));
      stringBuilder1.AppendLine("END");
      if (this._isCreateStoredProcInDbase)
      {
        this._dbase.CreateStoredProcedure(stringBuilder1.ToString(), storedProcName, this._storedProcedureErrorFilePath);
        return null;
      }
      stringBuilder1.AppendLine("GO");
      stringBuilder1.AppendLine("");
      return stringBuilder1.ToString();
    }

    private string BuildSelectAllBy(Table table)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      foreach (Column foreignKeyColumn in this._table.ForeignKeyColumns)
      {
        string storedProcNameBy = Functions.GetStoredProcNameBy(table, foreignKeyColumn, this._spPrefixSuffixIndex, this._storedProcPrefix, this._storedProcSuffix, StoredProcName.SelectAllBy);
        string selectStatement = this.GetSelectStatement(table);
        StringBuilder stringBuilder2 = new StringBuilder();
        stringBuilder2.AppendLine("CREATE PROCEDURE " + storedProcNameBy);
        stringBuilder2.AppendLine("(");
        stringBuilder2.AppendLine("   @" + foreignKeyColumn.NameCamelStyle + " " + foreignKeyColumn.StoredProcParameter);
        stringBuilder2.AppendLine(")");
        stringBuilder2.AppendLine("AS");
        stringBuilder2.AppendLine("BEGIN");
        stringBuilder2.AppendLine("  SET NOCOUNT ON;");
        stringBuilder2.AppendLine(" ");
        stringBuilder2.AppendLine("  " + selectStatement);
        stringBuilder2.AppendLine("  WHERE [" + foreignKeyColumn.NameOriginal + "] = @" + foreignKeyColumn.NameCamelStyle);
        stringBuilder2.AppendLine("END");
        if (this._isCreateStoredProcInDbase)
        {
          this._dbase.CreateStoredProcedure(stringBuilder2.ToString(), storedProcNameBy, this._storedProcedureErrorFilePath);
        }
        else
        {
          stringBuilder2.AppendLine("GO");
          stringBuilder2.AppendLine("");
          stringBuilder1.Append(stringBuilder2.ToString());
        }
      }
      if (this._isCreateStoredProcInDbase)
        return null;
      return stringBuilder1.ToString();
    }

    private string BuildInsert(Table table)
    {
      int num = 0;
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = new StringBuilder();
      StringBuilder stringBuilder3 = new StringBuilder();
      StringBuilder stringBuilder4 = new StringBuilder();
      string str = string.Empty;
      foreach (Column column in this._table.Columns)
      {
        if (num > 0)
          str = "  ";
        if (!column.IsPrimaryKey && !column.IsComputed && (!column.IsIdentity && !column.IsUniqueIdWithNewId) && !column.IsDateWithGetDate || column.IsPrimaryKey && !column.IsPrimaryKeyUnique)
        {
          stringBuilder2.AppendLine(str + " @" + column.NameCamelStyle + " " + column.StoredProcParameter + ",");
          stringBuilder3.AppendLine(str + "  [" + column.NameOriginal + "],");
          stringBuilder4.AppendLine(str + "  @" + column.NameCamelStyle + ",");
          ++num;
        }
      }
      string storedProcName = Functions.GetStoredProcName(table, StoredProcName.Insert, this._spPrefixSuffixIndex, this._storedProcPrefix, this._storedProcSuffix);
      stringBuilder1.AppendLine("CREATE PROCEDURE " + storedProcName);
      stringBuilder1.AppendLine("(");
      stringBuilder1.AppendLine("  " + Functions.RemoveLastComma(stringBuilder2.ToString()));
      stringBuilder1.AppendLine(")");
      stringBuilder1.AppendLine("AS");
      stringBuilder1.AppendLine("BEGIN");
      stringBuilder1.AppendLine("  SET NOCOUNT ON;");
      stringBuilder1.AppendLine("");
      stringBuilder1.AppendLine("  INSERT INTO [" + this._table.OwnerOriginal + "].[" + this._table.NameOriginal + "]");
      stringBuilder1.AppendLine("  (");
      stringBuilder1.AppendLine("  " + Functions.RemoveLastComma(stringBuilder3.ToString()));
      stringBuilder1.AppendLine("  )");
      if (this._table.PrimaryKeyCount == 1)
        stringBuilder1.AppendLine("  OUTPUT inserted.[" + this._table.FirstPrimaryKeyNameOriginal + "]");
      stringBuilder1.AppendLine("  VALUES");
      stringBuilder1.AppendLine("  (");
      stringBuilder1.AppendLine("  " + Functions.RemoveLastComma(stringBuilder4.ToString()));
      stringBuilder1.AppendLine("  )");
      stringBuilder1.AppendLine("END");
      if (this._isCreateStoredProcInDbase)
      {
        this._dbase.CreateStoredProcedure(stringBuilder1.ToString(), storedProcName, this._storedProcedureErrorFilePath);
        return null;
      }
      stringBuilder1.AppendLine("GO");
      stringBuilder1.AppendLine("");
      return stringBuilder1.ToString();
    }

    private string BuildUpdate(Table table)
    {
      int num1 = 0;
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = new StringBuilder();
      StringBuilder stringBuilder3 = new StringBuilder();
      StringBuilder stringBuilder4 = new StringBuilder();
      string str1 = string.Empty;
      foreach (Column primaryKeyColumn in this._table.PrimaryKeyColumns)
      {
        if (num1 > 0)
        {
          stringBuilder3.AppendLine(" AND ");
          str1 = "  ";
        }
        stringBuilder3.Append(str1 + "[" + primaryKeyColumn.NameOriginal + "] = @" + primaryKeyColumn.NameCamelStyle);
        ++num1;
      }
      int num2 = 0;
      string str2 = string.Empty;
      foreach (Column column in this._table.Columns)
      {
        if (num2 > 0)
          str2 = "  ";
        if (!column.IsPrimaryKey && !column.IsComputed && (!column.IsIdentity && !column.IsUniqueIdWithNewId) && !column.IsDateWithGetDate || column.IsPrimaryKey && !column.IsPrimaryKeyUnique)
        {
          stringBuilder4.AppendLine(str2 + "[" + column.NameOriginal + "] = @" + column.NameCamelStyle + ",");
          ++num2;
        }
        if (column.IsPrimaryKey || !column.IsComputed && !column.IsIdentity && (!column.IsUniqueIdWithNewId && !column.IsDateWithGetDate))
          stringBuilder2.AppendLine(str2 + " @" + column.NameCamelStyle + " " + column.StoredProcParameter + ",");
      }
      string storedProcName = Functions.GetStoredProcName(table, StoredProcName.Update, this._spPrefixSuffixIndex, this._storedProcPrefix, this._storedProcSuffix);
      stringBuilder1.AppendLine("CREATE PROCEDURE " + storedProcName);
      stringBuilder1.AppendLine("(");
      stringBuilder1.AppendLine("  " + Functions.RemoveLastComma(stringBuilder2.ToString()));
      stringBuilder1.AppendLine(")");
      stringBuilder1.AppendLine("AS");
      stringBuilder1.AppendLine("BEGIN");
      stringBuilder1.AppendLine("  SET NOCOUNT ON;");
      stringBuilder1.AppendLine("");
      stringBuilder1.AppendLine("  UPDATE [" + this._table.OwnerOriginal + "].[" + this._table.NameOriginal + "]");
      stringBuilder1.AppendLine("  SET");
      stringBuilder1.AppendLine("  " + Functions.RemoveLastComma(stringBuilder4.ToString()));
      stringBuilder1.AppendLine("  WHERE " + stringBuilder3.ToString());
      stringBuilder1.AppendLine("END");
      if (this._isCreateStoredProcInDbase)
      {
        this._dbase.CreateStoredProcedure(stringBuilder1.ToString(), storedProcName, this._storedProcedureErrorFilePath);
        return null;
      }
      stringBuilder1.AppendLine("GO");
      stringBuilder1.AppendLine("");
      return stringBuilder1.ToString();
    }

    private string BuildDelete(Table table)
    {
      int num = 0;
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = new StringBuilder();
      StringBuilder stringBuilder3 = new StringBuilder();
      string str = string.Empty;
      foreach (Column primaryKeyColumn in this._table.PrimaryKeyColumns)
      {
        if (num > 0)
        {
          stringBuilder3.AppendLine(" AND ");
          str = "  ";
        }
        stringBuilder2.AppendLine(str + " @" + primaryKeyColumn.NameCamelStyle + " " + primaryKeyColumn.StoredProcParameter + ",");
        stringBuilder3.Append(str + "[" + primaryKeyColumn.NameOriginal + "] = @" + primaryKeyColumn.NameCamelStyle);
        ++num;
      }
      string storedProcName = Functions.GetStoredProcName(table, StoredProcName.Delete, this._spPrefixSuffixIndex, this._storedProcPrefix, this._storedProcSuffix);
      stringBuilder1.AppendLine("CREATE PROCEDURE " + storedProcName);
      stringBuilder1.AppendLine("( ");
      stringBuilder1.AppendLine("  " + Functions.RemoveLastComma(stringBuilder2.ToString()));
      stringBuilder1.AppendLine(")");
      stringBuilder1.AppendLine("AS");
      stringBuilder1.AppendLine("BEGIN");
      stringBuilder1.AppendLine("  SET NOCOUNT ON;");
      stringBuilder1.AppendLine(" ");
      stringBuilder1.AppendLine("  DELETE FROM [" + this._table.OwnerOriginal + "].[" + this._table.NameOriginal + "]");
      stringBuilder1.AppendLine("  WHERE " + stringBuilder3.ToString());
      stringBuilder1.AppendLine("END");
      if (this._isCreateStoredProcInDbase)
      {
        this._dbase.CreateStoredProcedure(stringBuilder1.ToString(), storedProcName, this._storedProcedureErrorFilePath);
        return null;
      }
      stringBuilder1.AppendLine("GO");
      stringBuilder1.AppendLine("");
      return stringBuilder1.ToString();
    }

    private string BuildSelectDropDownListData(Table table)
    {
      StringBuilder stringBuilder = new StringBuilder();
      string storedProcName = Functions.GetStoredProcName(table, StoredProcName.SelectDropDownListData, this._spPrefixSuffixIndex, this._storedProcPrefix, this._storedProcSuffix);
      string str1 = string.Empty;
      string str2 = string.Empty;
      if (this._table.FirstPrimaryKeyNameOriginal != this._table.FirstPrimaryKeyName)
        str1 = " AS [" + this._table.FirstPrimaryKeyName + "]";
      if (this._table.DataTextFieldOriginalName != this._table.DataTextField)
        str2 = " AS [" + this._table.DataTextField + "]";
      stringBuilder.AppendLine("CREATE PROCEDURE " + storedProcName);
      stringBuilder.AppendLine("AS");
      stringBuilder.AppendLine("BEGIN");
      stringBuilder.AppendLine("  SET NOCOUNT ON;");
      stringBuilder.AppendLine(" ");
      stringBuilder.AppendLine("  SELECT [" + this._table.FirstPrimaryKeyNameOriginal + "]" + str1 + ", [" + this._table.DataTextFieldOriginalName + "]" + str2 + " ");
      stringBuilder.AppendLine("  FROM [" + this._table.OwnerOriginal + "].[" + this._table.NameOriginal + "] ");
      if (!this._table.IsDataTextFieldBinaryOrSpatialDataType)
        stringBuilder.AppendLine("  ORDER BY [" + this._table.DataTextFieldOriginalName + "] ASC ");
      stringBuilder.AppendLine("END");
      if (this._isCreateStoredProcInDbase)
      {
        this._dbase.CreateStoredProcedure(stringBuilder.ToString(), storedProcName, this._storedProcedureErrorFilePath);
        return null;
      }
      stringBuilder.AppendLine("GO");
      stringBuilder.AppendLine("");
      return stringBuilder.ToString();
    }

    private string GetSelectStatement(Table table)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = new StringBuilder();
      stringBuilder1.AppendLine("SELECT");
      foreach (Column column in this._table.Columns)
      {
        string str = string.Empty;
        if (column.NameOriginal != column.Name)
          str = " AS [" + column.Name + "]";
        stringBuilder2.AppendLine("  [" + column.NameOriginal + "]" + str + ",");
      }
      stringBuilder1.Append(Functions.RemoveLastComma(stringBuilder2.ToString()));
      stringBuilder1.AppendLine("");
      stringBuilder1.AppendLine("  FROM [" + this._table.OwnerOriginal + "].[" + this._table.NameOriginal + "]");
      return stringBuilder1.ToString();
    }

    private string GetSelectNoFromWithRowNumberStatement(Table table)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("\t  SELECT");
      foreach (Column column in this._table.Columns)
      {
        string str = string.Empty;
        if (column.NameOriginal != column.Name)
          str = " AS [" + column.Name + "]";
        stringBuilder.AppendLine("\t  [" + column.NameOriginal + "]" + str + ",");
      }
      stringBuilder.AppendLine("\t  ROW_NUMBER() OVER");
      return stringBuilder.ToString();
    }

    private string GetSelectStatementForTotals(Table table)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = new StringBuilder();
      stringBuilder1.AppendLine("SELECT");
      foreach (Column column in this._table.Columns.Where<Column>((c => c.IsMoneyOrDecimalField)))
        stringBuilder2.AppendLine("  SUM([" + column.NameOriginal + "]) AS [" + column.Name + MyConstants.WordTotal + "],");
      stringBuilder1.Append(Functions.RemoveLastComma(stringBuilder2.ToString()));
      stringBuilder1.AppendLine("");
      stringBuilder1.AppendLine("  FROM [" + this._table.OwnerOriginal + "].[" + this._table.NameOriginal + "]");
      return stringBuilder1.ToString();
    }

    private string GetSelectSkipAndTakeSelectStatement(Table table, Column fkColumn = null)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = new StringBuilder();
      StringBuilder stringBuilder3 = new StringBuilder();
      stringBuilder1.AppendLine("      SELECT");
      foreach (Column column in this._table.Columns)
      {
        string str = string.Empty;
        if (column.NameOriginal != column.Name)
          str = " AS [" + column.Name + "]";
        stringBuilder2.AppendLine("      [" + column.NameOriginal + "]" + str + ",");
      }
      stringBuilder1.Append(stringBuilder2.ToString());
      stringBuilder1.AppendLine("\t  ROW_NUMBER() OVER");
      stringBuilder1.AppendLine("\t  (");
      stringBuilder1.AppendLine("\t     ORDER BY");
      foreach (Column spatialDataTypeColumn in this._table.NoneBinaryOrSpatialDataTypeColumns)
      {
        stringBuilder3.AppendLine("\t     CASE WHEN @sortByExpression = '" + spatialDataTypeColumn.Name + "' THEN [" + spatialDataTypeColumn.NameOriginal + "] END,");
        stringBuilder3.AppendLine("\t     CASE WHEN @sortByExpression = '" + spatialDataTypeColumn.Name + " desc' THEN [" + spatialDataTypeColumn.NameOriginal + "] END DESC,");
        stringBuilder3.AppendLine("");
      }
      stringBuilder1.Append(Functions.RemoveLastComma(stringBuilder3.ToString()));
      stringBuilder1.AppendLine("");
      stringBuilder1.AppendLine("\t  ) AS 'RowNum'");
      stringBuilder1.AppendLine("\t  FROM [" + this._table.OwnerOriginal + "].[" + this._table.NameOriginal + "]");
      if (fkColumn != null)
        stringBuilder1.AppendLine("\t  WHERE [" + fkColumn.NameOriginal + "] = @" + fkColumn.NameCamelStyle);
      return stringBuilder1.ToString();
    }

    private string GetSelectSkipAndTakeSelectStatementForSQL2012OrHigher(Table table, Column fkColumn = null)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = new StringBuilder();
      StringBuilder stringBuilder3 = new StringBuilder();
      stringBuilder1.AppendLine("  SELECT");
      foreach (Column column in this._table.Columns)
      {
        string str = string.Empty;
        if (column.NameOriginal != column.Name)
          str = " AS [" + column.Name + "]";
        stringBuilder2.AppendLine("  [" + column.NameOriginal + "]" + str + ",");
      }
      stringBuilder1.Append(Functions.RemoveLastComma(stringBuilder2.ToString()));
      stringBuilder1.AppendLine("");
      stringBuilder1.AppendLine("  FROM [" + this._table.OwnerOriginal + "].[" + this._table.NameOriginal + "]");
      if (fkColumn != null)
        stringBuilder1.AppendLine("  WHERE [" + fkColumn.NameOriginal + "] = @" + fkColumn.NameCamelStyle);
      stringBuilder1.AppendLine("  ORDER BY");
      foreach (Column spatialDataTypeColumn in this._table.NoneBinaryOrSpatialDataTypeColumns)
      {
        stringBuilder3.AppendLine("  CASE WHEN @sortByExpression = '" + spatialDataTypeColumn.Name + "' THEN [" + spatialDataTypeColumn.NameOriginal + "] END,");
        stringBuilder3.AppendLine("  CASE WHEN @sortByExpression = '" + spatialDataTypeColumn.Name + " desc' THEN [" + spatialDataTypeColumn.NameOriginal + "] END DESC,");
        stringBuilder3.AppendLine("");
      }
      stringBuilder1.Append(Functions.RemoveLastComma(stringBuilder3.ToString()));
      stringBuilder1.AppendLine("");
      return stringBuilder1.ToString();
    }

    private string GetWhereParameterStatementForDynamicWhere(Table table)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("  WHERE ");
      foreach (Column spatialDataTypeColumn in this._table.NoneBinaryOrSpatialDataTypeColumns)
      {
        if (Functions.GetFieldType(spatialDataTypeColumn) == "String")
          stringBuilder.AppendLine("  ([" + spatialDataTypeColumn.NameOriginal + "] LIKE '%' + @" + spatialDataTypeColumn.NameCamelStyle + " + '%' OR @" + spatialDataTypeColumn.NameCamelStyle + " IS NULL) AND ");
        else
          stringBuilder.AppendLine("  ([" + spatialDataTypeColumn.NameOriginal + "] = @" + spatialDataTypeColumn.NameCamelStyle + " OR @" + spatialDataTypeColumn.NameCamelStyle + " IS NULL) AND ");
      }
      return Functions.RemoveLastAnd(stringBuilder.ToString());
    }

    private string GetPassedParametersForDynamicWhere(Table table)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (Column spatialDataTypeColumn in this._table.NoneBinaryOrSpatialDataTypeColumns)
        stringBuilder.AppendLine("   @" + spatialDataTypeColumn.NameCamelStyle + " " + spatialDataTypeColumn.StoredProcParameter + " = NULL,");
      return Functions.RemoveLastComma(stringBuilder.ToString());
    }

    private string GetOrderByCaseWhenStatement(Table table, bool isIndentmore = false)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = new StringBuilder();
      string str = string.Empty;
      if (isIndentmore)
        str = "          ";
      stringBuilder1.AppendLine(str + "  ORDER BY");
      foreach (Column spatialDataTypeColumn in this._table.NoneBinaryOrSpatialDataTypeColumns)
      {
        stringBuilder2.AppendLine(str + "  CASE WHEN @sortByExpression = '" + spatialDataTypeColumn.Name + "' THEN [" + spatialDataTypeColumn.NameOriginal + "] END,");
        stringBuilder2.AppendLine(str + "  CASE WHEN @sortByExpression = '" + spatialDataTypeColumn.Name + " desc' THEN [" + spatialDataTypeColumn.NameOriginal + "] END DESC,");
        stringBuilder2.AppendLine("");
      }
      stringBuilder1.Append(Functions.RemoveLastComma(stringBuilder2.ToString()));
      return stringBuilder1.ToString();
    }
  }
}
