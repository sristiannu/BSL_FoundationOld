
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class DataLayerBase
  {
    private Table _table;
    private Tables _selectedTables;
    private DataTable _referencedTables;
    private const string _fileExtension = ".cs";
    private string _apiName;
    private string _apiNameDirectory;
    private const Language _language = Language.CSharp;
    private bool _isUseStoredProcedure;
    private int _spPrefixSuffixIndex;
    private string _storedProcPrefix;
    private string _storedProcSuffix;
    private DatabaseObjectToGenerateFrom _generateFrom;
    private ApplicationVersion _appVersion;
    private string _commandTypeCode;
    private string _contextName;
    private string _contextInitialization;
    private GeneratedSqlType _generatedSqlType;

    internal DataLayerBase()
    {
    }

    internal DataLayerBase(Table table, Tables selectedTables, DataTable referencedTables, string apiName, string apiNameDirectory, bool isUseStoredProcedure, int spPrefixSuffixIndex, string storedProcPrefix, string storedProcSuffix, DatabaseObjectToGenerateFrom generateFrom, ApplicationVersion appVersion, string database, GeneratedSqlType generatedSqlType)
    {
      this._table = table;
      this._selectedTables = selectedTables;
      this._referencedTables = referencedTables;
      this._apiName = apiName;
      this._apiNameDirectory = apiNameDirectory + "\\" + MyConstants.DirectoryDataLayerBase;
      this._isUseStoredProcedure = isUseStoredProcedure;
      this._spPrefixSuffixIndex = spPrefixSuffixIndex;
      this._storedProcPrefix = storedProcPrefix;
      this._storedProcSuffix = storedProcSuffix;
      this._generateFrom = generateFrom;
      this._appVersion = appVersion;
      this._commandTypeCode = !this._isUseStoredProcedure ? "command.CommandType = CommandType.Text;" : "command.CommandType = CommandType.StoredProcedure;";
      this._contextName = Functions.ReplaceNoneAlphaNumericWithUnderscore(Functions.ConvertToPascal(database)) + "Context";
      this._contextInitialization = this._contextName + " context = new " + this._contextName + "();";
      this._generatedSqlType = generatedSqlType;
      this.Generate();
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._apiNameDirectory + this._table.Name + MyConstants.WordDataLayerBase + ".cs"))
      {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("using " + this._apiName + ".BusinessObject;");
        sb.AppendLine("using System;");
        sb.AppendLine("using System.Collections.Generic;");
        sb.AppendLine("using System.Linq;");
        sb.AppendLine("using Microsoft.EntityFrameworkCore;");
        sb.AppendLine("");
        sb.AppendLine("namespace " + this._apiName + ".DataLayer.Base");
        sb.AppendLine("{");
        sb.AppendLine("     /// <summary>");
        sb.AppendLine("     /// Base class for " + this._table.Name + "DataLayer.  Do not make changes to this class,");
        sb.AppendLine("     /// instead, put additional code in the " + this._table.Name + "DataLayer class");
        sb.AppendLine("     /// </summary>");
        sb.AppendLine("     internal class " + this._table.Name + MyConstants.WordDataLayerBase ?? "");
        sb.AppendLine("     {");
        this.WriteConstructor(sb);
        if (this._appVersion == ApplicationVersion.ProfessionalPlus)
          this.WriteProPlusMethods(sb);
        else
          this.WriteExpressMethods(sb);
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
      sb.AppendLine("");
    }

    private void WriteProPlusMethods(StringBuilder sb)
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
        this.WriteSelectAllDynamicWhereMethod(sb);
        this.WriteCollectionByMethods(sb);
        if (!this._table.IsContainsPrimaryAndForeignKeyColumnsOnly && this._table.PrimaryKeyCount == 1)
          this.WriteSelectDropDownListData(sb);
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
    }

    private void WriteExpressMethods(StringBuilder sb)
    {
      if (!this._table.IsContainsPrimaryAndForeignKeyColumnsOnly)
        this.WriteExpressSelectByPrimaryKeyMethod(sb);
      this.WriteExpressGetRecordCountMethod(sb);
      this.WriteExpressGetRecordByCountMethods(sb);
      this.WriteExpressGetRecordCountDynamicWhere(sb);
      this.WriteExpressSelectSkipAndTakeMethod(sb);
      this.WriteExpressSelectSkipAndTakeByMethods(sb);
      this.WriteExpressSelectSkipAndTakeDynamicWhereMethod(sb);
      this.WriteExpressSelectAllMethod(sb);
      this.WriteExpressSelectAllDynamicWhereMethod(sb);
      this.WriteExpressCollectionByMethods(sb);
      if (!this._table.IsContainsPrimaryAndForeignKeyColumnsOnly && this._table.PrimaryKeyCount == 1)
        this.WriteExpressSelectDropDownListData(sb);
      this.WriteExpressInsertMethod(sb);
      if (!this._table.IsContainsPrimaryAndForeignKeyColumnsOnly)
        this.WriteExpressUpdateMethod(sb);
      this.WriteExpressDeleteMethod(sb);
    }

    private void WriteSelectAllMethod(StringBuilder sb)
    {
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Selects all " + this._table.Name);
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         internal static List<" + this._table.Name + "> SelectAll()");
      sb.AppendLine("         {");
      sb.AppendLine("             " + this._contextInitialization);
      if (this._isUseStoredProcedure)
        sb.AppendLine("             return context." + this._table.Name + ".ToList();");
      else
        sb.AppendLine("             return context." + this._table.Name + ".ToList();");
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
      sb.AppendLine("             " + this._contextInitialization);
      if (this._isUseStoredProcedure)
        sb.AppendLine("             return context." + this._table.Name + ".Count();");
      else
        sb.AppendLine("             return context." + this._table.Name + ".Count();");
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
        sb.AppendLine("             " + this._contextInitialization);
        if (this._isUseStoredProcedure)
        {
          if (this._generatedSqlType == GeneratedSqlType.EFCore && foreignKeyColumn.SQLDataType == SQLType.uniqueidentifier)
            sb.AppendLine("             return context." + this._table.Name + ".Where(" + this._table.LinqFromVariable + " => " + this._table.LinqFromVariable + "." + foreignKeyColumn.Name + ".ToString() == " + foreignKeyColumn.NameCamelStyle + ").Count();");
          else
            sb.AppendLine("             return context." + this._table.Name + ".Where(" + this._table.LinqFromVariable + " => " + this._table.LinqFromVariable + "." + foreignKeyColumn.Name + " == " + foreignKeyColumn.NameCamelStyle + ").Count();");
        }
        else if (this._generatedSqlType == GeneratedSqlType.EFCore && foreignKeyColumn.SQLDataType == SQLType.uniqueidentifier)
          sb.AppendLine("             return context." + this._table.Name + ".Where(" + this._table.LinqFromVariable + " => " + this._table.LinqFromVariable + "." + foreignKeyColumn.Name + ".ToString() == " + foreignKeyColumn.NameCamelStyle + ").Count();");
        else
          sb.AppendLine("             return context." + this._table.Name + ".Where(" + this._table.LinqFromVariable + " => " + this._table.LinqFromVariable + "." + foreignKeyColumn.Name + " == " + foreignKeyColumn.NameCamelStyle + ").Count();");
        sb.AppendLine("         }");
      }
    }

    private void WriteSelectSkipAndTakeMethod(StringBuilder sb)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = new StringBuilder();
      StringBuilder stringBuilder3 = new StringBuilder();
      StringBuilder stringBuilder4 = new StringBuilder();
      StringBuilder stringBuilder5 = new StringBuilder();
      string str1 = " desc";
      string str2 = "OrderByDescending";
      string str3 = string.Empty;
      for (int index = 0; index < 4; ++index)
      {
        stringBuilder1.AppendLine("                     switch (sortByExpression)");
        stringBuilder1.AppendLine("                     {");
        foreach (Column column in (List<Column>) this._table.Columns)
        {
          if (index > 1)
            str3 = this.IncludeNavProperties();
          if (column.Name != this._table.FirstPrimaryKeyName)
          {
            stringBuilder1.AppendLine("                         case \"" + column.Name + str1 + "\":");
            stringBuilder1.AppendLine("                             return context." + this._table.Name + str3 + "." + str2 + "(" + this._table.LinqFromVariable + " => " + this._table.LinqFromVariable + "." + column.Name + ").Skip(startRowIndex).Take(rows).ToList();");
          }
        }
        stringBuilder1.AppendLine("                         default:");
        stringBuilder1.AppendLine("                             return context." + this._table.Name + str3 + "." + str2 + "(" + this._table.LinqFromVariable + " => " + this._table.LinqFromVariable + "." + this._table.FirstPrimaryKeyName + ").Skip(startRowIndex).Take(rows).ToList();");
        stringBuilder1.AppendLine("                     }");
        switch (index)
        {
          case 0:
            stringBuilder2.Append(stringBuilder1.ToString());
            str1 = string.Empty;
            str2 = "OrderBy";
            break;
          case 1:
            stringBuilder3.Append(stringBuilder1.ToString());
            str1 = " desc";
            str2 = "OrderByDescending";
            if (this._table.IsContainsForeignKeysWithTableSelected)
              break;
            goto label_16;
          case 2:
            stringBuilder4.Append(stringBuilder1.ToString());
            str1 = string.Empty;
            str2 = "OrderBy";
            break;
          default:
            stringBuilder5.Append(stringBuilder1.ToString());
            break;
        }
        str3 = string.Empty;
        stringBuilder1.Clear();
      }
label_16:
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Selects " + this._table.Name + " records sorted by the sortByExpression and returns records from the startRowIndex with rows (# of rows)");
      sb.AppendLine("         /// </summary>");
      if (this._table.IsContainsForeignKeysWithTableSelected)
      {
        sb.AppendLine("         internal static List<" + this._table.Name + "> SelectSkipAndTake(string sortByExpression, int startRowIndex, int rows, bool isIncludeRelatedProperties = true)");
        sb.AppendLine("         {");
        sb.AppendLine("             " + this._contextInitialization);
        sb.AppendLine("");
        sb.AppendLine("             if (!isIncludeRelatedProperties)");
        sb.AppendLine("             {");
        if (this._isUseStoredProcedure)
        {
          sb.AppendLine("                 if (sortByExpression.Contains(\" desc\"))");
          sb.AppendLine("                 {");
          sb.Append(stringBuilder2.ToString());
          sb.AppendLine("                 }");
          sb.AppendLine("                 else");
          sb.AppendLine("                 {");
          sb.Append(stringBuilder3.ToString());
          sb.AppendLine("                 }");
        }
        else
        {
          sb.AppendLine("                 if (sortByExpression.Contains(\" desc\"))");
          sb.AppendLine("                 {");
          sb.Append(stringBuilder2.ToString());
          sb.AppendLine("                 }");
          sb.AppendLine("                 else");
          sb.AppendLine("                 {");
          sb.Append(stringBuilder3.ToString());
          sb.AppendLine("                 }");
        }
        sb.AppendLine("             }");
        sb.AppendLine("             else");
        sb.AppendLine("             {");
        if (this._isUseStoredProcedure)
        {
          sb.AppendLine("                 if (sortByExpression.Contains(\" desc\"))");
          sb.AppendLine("                 {");
          sb.Append(stringBuilder4.ToString());
          sb.AppendLine("                 }");
          sb.AppendLine("                 else");
          sb.AppendLine("                 {");
          sb.Append(stringBuilder5.ToString());
          sb.AppendLine("                 }");
        }
        else
        {
          sb.AppendLine("                 if (sortByExpression.Contains(\" desc\"))");
          sb.AppendLine("                 {");
          sb.Append(stringBuilder4.ToString());
          sb.AppendLine("                 }");
          sb.AppendLine("                 else");
          sb.AppendLine("                 {");
          sb.Append(stringBuilder5.ToString());
          sb.AppendLine("                 }");
        }
        sb.AppendLine("             }");
      }
      else
      {
        sb.AppendLine("         internal static List<" + this._table.Name + "> SelectSkipAndTake(string sortByExpression, int startRowIndex, int rows)");
        sb.AppendLine("         {");
        sb.AppendLine("             " + this._contextInitialization);
        sb.AppendLine("");
        if (this._isUseStoredProcedure)
        {
          sb.AppendLine("             if (sortByExpression.Contains(\" desc\"))");
          sb.AppendLine("             {");
          sb.Append(stringBuilder2.ToString());
          sb.AppendLine("             }");
          sb.AppendLine("             else");
          sb.AppendLine("             {");
          sb.Append(stringBuilder3.ToString());
          sb.AppendLine("             }");
        }
        else
        {
          sb.AppendLine("             if (sortByExpression.Contains(\" desc\"))");
          sb.AppendLine("             {");
          sb.Append(stringBuilder2.ToString());
          sb.AppendLine("             }");
          sb.AppendLine("             else");
          sb.AppendLine("             {");
          sb.Append(stringBuilder3.ToString());
          sb.AppendLine("             }");
        }
      }
      sb.AppendLine("         }");
    }

    private void WriteSelectSkipAndTakeByMethods(StringBuilder sb)
    {
      foreach (Column foreignKeyColumn in this._table.ForeignKeyColumns)
      {
        StringBuilder stringBuilder1 = new StringBuilder();
        StringBuilder stringBuilder2 = new StringBuilder();
        StringBuilder stringBuilder3 = new StringBuilder();
        string str1 = " desc";
        string str2 = "OrderByDescending";
        for (int index = 0; index < 2; ++index)
        {
          stringBuilder1.AppendLine("                 switch (sortByExpression)");
          stringBuilder1.AppendLine("                 {");
          foreach (Column column in (List<Column>) this._table.Columns)
          {
            if (column.Name != this._table.FirstPrimaryKeyName)
            {
              stringBuilder1.AppendLine("                     case \"" + column.Name + str1 + "\":");
              if (this._generatedSqlType == GeneratedSqlType.EFCore && foreignKeyColumn.SQLDataType == SQLType.uniqueidentifier)
                stringBuilder1.AppendLine("                         return context." + this._table.Name + ".Where(" + this._table.LinqFromVariable + " => " + this._table.LinqFromVariable + "." + foreignKeyColumn.Name + ".ToString() == " + foreignKeyColumn.NameCamelStyle + ")." + str2 + "(" + this._table.LinqFromVariable + " => " + this._table.LinqFromVariable + "." + column.Name + ").Skip(startRowIndex).Take(rows).ToList();");
              else
                stringBuilder1.AppendLine("                         return context." + this._table.Name + ".Where(" + this._table.LinqFromVariable + " => " + this._table.LinqFromVariable + "." + foreignKeyColumn.Name + " == " + foreignKeyColumn.NameCamelStyle + ")." + str2 + "(" + this._table.LinqFromVariable + " => " + this._table.LinqFromVariable + "." + column.Name + ").Skip(startRowIndex).Take(rows).ToList();");
            }
          }
          stringBuilder1.AppendLine("                     default:");
          if (this._generatedSqlType == GeneratedSqlType.EFCore && foreignKeyColumn.SQLDataType == SQLType.uniqueidentifier)
            stringBuilder1.AppendLine("                         return context." + this._table.Name + ".Where(" + this._table.LinqFromVariable + " => " + this._table.LinqFromVariable + "." + foreignKeyColumn.Name + ".ToString() == " + foreignKeyColumn.NameCamelStyle + ")." + str2 + "(" + this._table.LinqFromVariable + " => " + this._table.LinqFromVariable + "." + this._table.FirstPrimaryKeyName + ").Skip(startRowIndex).Take(rows).ToList();");
          else
            stringBuilder1.AppendLine("                         return context." + this._table.Name + ".Where(" + this._table.LinqFromVariable + " => " + this._table.LinqFromVariable + "." + foreignKeyColumn.Name + " == " + foreignKeyColumn.NameCamelStyle + ")." + str2 + "(" + this._table.LinqFromVariable + " => " + this._table.LinqFromVariable + "." + this._table.FirstPrimaryKeyName + ").Skip(startRowIndex).Take(rows).ToList();");
          stringBuilder1.AppendLine("                 }");
          if (index == 0)
          {
            stringBuilder2.Append(stringBuilder1.ToString());
            str1 = string.Empty;
            str2 = "OrderBy";
          }
          else
            stringBuilder3.Append(stringBuilder1.ToString());
          stringBuilder1.Clear();
        }
        sb.AppendLine("");
        sb.AppendLine("         /// <summary>");
        sb.AppendLine("         /// Selects records by " + foreignKeyColumn.Name + " as a collection (List) of " + this._table.Name + " sorted by the sortByExpression and returns the rows (# of records) starting from the startRowIndex");
        sb.AppendLine("         /// </summary>");
        sb.AppendLine("         internal static List<" + this._table.Name + "> SelectSkipAndTakeBy" + foreignKeyColumn.Name + "(string sortByExpression, int startRowIndex, int rows, " + foreignKeyColumn.SystemType + " " + foreignKeyColumn.NameCamelStyle + ")");
        sb.AppendLine("         {");
        sb.AppendLine("             " + this._contextInitialization);
        sb.AppendLine("");
        if (this._isUseStoredProcedure)
        {
          sb.AppendLine("             if (sortByExpression.Contains(\" desc\"))");
          sb.AppendLine("             {");
          sb.Append(stringBuilder2.ToString());
          sb.AppendLine("             }");
          sb.AppendLine("             else");
          sb.AppendLine("             {");
          sb.Append(stringBuilder3.ToString());
          sb.AppendLine("             }");
        }
        else
        {
          sb.AppendLine("             if (sortByExpression.Contains(\" desc\"))");
          sb.AppendLine("             {");
          sb.Append(stringBuilder2.ToString());
          sb.AppendLine("             }");
          sb.AppendLine("             else");
          sb.AppendLine("             {");
          sb.Append(stringBuilder3.ToString());
          sb.AppendLine("             }");
        }
        sb.AppendLine("         }");
      }
    }

    private void WriteCollectionByMethods(StringBuilder sb)
    {
      foreach (Column foreignKeyColumn in this._table.ForeignKeyColumns)
      {
        string str = "static List<" + this._table.Name + ">";
        Functions.GetStoredProcNameBy(this._table, foreignKeyColumn, this._spPrefixSuffixIndex, this._storedProcPrefix, this._storedProcSuffix, StoredProcName.SelectAllBy);
        sb.AppendLine("");
        sb.AppendLine("         /// <summary>");
        sb.AppendLine("         /// Selects all " + this._table.Name + " by " + foreignKeyColumn.ForeignKeyTableName + ", related to column " + foreignKeyColumn.Name ?? "");
        sb.AppendLine("         /// </summary>");
        sb.AppendLine("         internal " + str + " Select" + this._table.Name + "CollectionBy" + foreignKeyColumn.Name + "(" + foreignKeyColumn.ForeignKeySystemType + " " + foreignKeyColumn.ForeignKeyColumnNameCamelStyle + ")");
        sb.AppendLine("         {");
        sb.AppendLine("             " + this._contextInitialization);
        if (this._isUseStoredProcedure)
        {
          if (this._generatedSqlType == GeneratedSqlType.EFCore && foreignKeyColumn.SQLDataType == SQLType.uniqueidentifier)
            sb.AppendLine("             return context." + this._table.Name + ".Where(" + this._table.LinqFromVariable + " => " + this._table.LinqFromVariable + "." + foreignKeyColumn.Name + ".ToString() == " + foreignKeyColumn.ForeignKeyColumnNameCamelStyle + ").ToList();");
          else
            sb.AppendLine("             return context." + this._table.Name + ".Where(" + this._table.LinqFromVariable + " => " + this._table.LinqFromVariable + "." + foreignKeyColumn.Name + " == " + foreignKeyColumn.ForeignKeyColumnNameCamelStyle + ").ToList();");
        }
        else if (this._generatedSqlType == GeneratedSqlType.EFCore && foreignKeyColumn.SQLDataType == SQLType.uniqueidentifier)
          sb.AppendLine("             return context." + this._table.Name + ".Where(" + this._table.LinqFromVariable + " => " + this._table.LinqFromVariable + "." + foreignKeyColumn.Name + ".ToString() == " + foreignKeyColumn.ForeignKeyColumnNameCamelStyle + ").ToList();");
        else
          sb.AppendLine("             return context." + this._table.Name + ".Where(" + this._table.LinqFromVariable + " => " + this._table.LinqFromVariable + "." + foreignKeyColumn.Name + " == " + foreignKeyColumn.ForeignKeyColumnNameCamelStyle + ").ToList();");
        sb.AppendLine("         }");
      }
    }

    private void WriteInsertMethod(StringBuilder sb)
    {
      string methodReturnType = Functions.GetInsertMethodReturnType(this._table, Language.CSharp);
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Inserts a record");
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         internal static " + methodReturnType + " Insert(" + this._table.Name + " " + this._table.VariableObjName + ")");
      sb.AppendLine("         {");
      sb.AppendLine("             " + this._contextInitialization);
      if (this._isUseStoredProcedure)
      {
        sb.AppendLine("             " + this._table.Name + " " + this._table.VariableEntityName + " = new " + this._table.Name + "();");
        sb.AppendLine("");
        foreach (Column column in (List<Column>) this._table.Columns)
        {
          if (!column.IsPrimaryKey || !column.IsIdentity && !column.IsComputed && (!column.IsUniqueIdWithNewId && !column.IsDateWithGetDate))
            sb.AppendLine("             " + this._table.VariableEntityName + "." + column.Name + " = " + this._table.VariableObjName + "." + column.Name + ";");
        }
        sb.AppendLine("");
        sb.AppendLine("             context." + this._table.Name + ".Add(" + this._table.VariableEntityName + ");");
        sb.AppendLine("             context.SaveChanges();");
        if (methodReturnType != "void")
        {
          sb.AppendLine("");
          if (this._generatedSqlType == GeneratedSqlType.EFCore && this._table.FirstPrimaryKeySQLDataType == SQLType.uniqueidentifier)
            sb.AppendLine("             return " + this._table.VariableEntityName + "." + this._table.FirstPrimaryKeyName + ".ToString();");
          else
            sb.AppendLine("             return " + this._table.VariableEntityName + "." + this._table.FirstPrimaryKeyName + ";");
        }
      }
      else
      {
        sb.AppendLine("             " + this._table.Name + " " + this._table.VariableEntityName + " = new " + this._table.Name + "();");
        sb.AppendLine("");
        foreach (Column column in (List<Column>) this._table.Columns)
        {
          if (!column.IsPrimaryKey || !column.IsIdentity && !column.IsComputed && (!column.IsUniqueIdWithNewId && !column.IsDateWithGetDate))
            sb.AppendLine("             " + this._table.VariableEntityName + "." + column.Name + " = " + this._table.VariableObjName + "." + column.Name + ";");
        }
        sb.AppendLine("");
        sb.AppendLine("             context." + this._table.Name + ".Add(" + this._table.VariableEntityName + ");");
        sb.AppendLine("             context.SaveChanges();");
        if (methodReturnType != "void")
        {
          sb.AppendLine("");
          if (this._generatedSqlType == GeneratedSqlType.EFCore && this._table.FirstPrimaryKeySQLDataType == SQLType.uniqueidentifier)
            sb.AppendLine("             return " + this._table.VariableEntityName + "." + this._table.FirstPrimaryKeyName + ".ToString();");
          else
            sb.AppendLine("             return " + this._table.VariableEntityName + "." + this._table.FirstPrimaryKeyName + ";");
        }
      }
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
      sb.AppendLine("             " + this._contextInitialization);
      if (this._isUseStoredProcedure)
      {
        sb.AppendLine("             " + this._table.Name + " " + this._table.VariableEntityName + " = context." + this._table.Name + ".Where(" + this._table.LinqFromVariable + " => " + Functions.GetLinqContextDotPrimaryKeysEqualsObjTableName(this._table, Language.CSharp) + ").FirstOrDefault();");
        sb.AppendLine("");
        sb.AppendLine("             if (" + this._table.VariableEntityName + " != null)");
        sb.AppendLine("             {");
        foreach (Column column in (List<Column>) this._table.Columns)
        {
          if (!column.IsPrimaryKey && !column.IsComputed)
            sb.AppendLine("                 " + this._table.VariableEntityName + "." + column.Name + " = " + this._table.VariableObjName + "." + column.Name + ";");
        }
        sb.AppendLine("");
        sb.AppendLine("                 context.SaveChanges();");
        sb.AppendLine("             }");
      }
      else
      {
        sb.AppendLine("             " + this._table.Name + " " + this._table.VariableEntityName + " = context." + this._table.Name + ".Where(" + this._table.LinqFromVariable + " => " + Functions.GetLinqContextDotPrimaryKeysEqualsObjTableName(this._table, Language.CSharp) + ").FirstOrDefault();");
        sb.AppendLine("");
        sb.AppendLine("             if (" + this._table.VariableEntityName + " != null)");
        sb.AppendLine("             {");
        foreach (Column column in (List<Column>) this._table.Columns)
        {
          if (!column.IsPrimaryKey && !column.IsComputed)
            sb.AppendLine("                 " + this._table.VariableEntityName + "." + column.Name + " = " + this._table.VariableObjName + "." + column.Name + ";");
        }
        sb.AppendLine("");
        sb.AppendLine("                 context.SaveChanges();");
        sb.AppendLine("             }");
      }
      sb.AppendLine("         }");
    }

    private void WriteSelectByPrimaryKeyMethod(StringBuilder sb)
    {
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Selects a record by primary key(s)");
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         internal static " + this._table.Name + " SelectByPrimaryKey(" + Functions.GetCommaDelimitedPrimaryKeysAndType(this._table, Language.CSharp) + ")");
      sb.AppendLine("         {");
      sb.AppendLine("             " + this._contextInitialization);
      if (this._isUseStoredProcedure)
        sb.AppendLine("             return context." + this._table.Name + ".Where(" + this._table.LinqFromVariable + " => " + Functions.GetLinqContextDotPrimaryKeysEquals(this._table, Language.CSharp, this._generatedSqlType) + ").FirstOrDefault();");
      else
        sb.AppendLine("             return context." + this._table.Name + ".Where(" + this._table.LinqFromVariable + " => " + Functions.GetLinqContextDotPrimaryKeysEquals(this._table, Language.CSharp, this._generatedSqlType) + ").FirstOrDefault();");
      sb.AppendLine("         }");
    }

    private void WriteGetRecordCountDynamicWhere(StringBuilder sb)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = new StringBuilder();
      StringBuilder stringBuilder3 = new StringBuilder();
      stringBuilder1.AppendLine("             return context." + this._table.Name);
      stringBuilder1.AppendLine("                 .Where(" + this._table.LinqFromVariable + " =>");
      foreach (Column spatialDataTypeColumn in this._table.NoneBinaryOrSpatialDataTypeColumns)
      {
        if (spatialDataTypeColumn.IsStringField)
        {
          stringBuilder1.AppendLine("                           (!String.IsNullOrEmpty(" + spatialDataTypeColumn.NameCamelStyle + ") ? " + this._table.LinqFromVariable + "." + spatialDataTypeColumn.Name + ".Contains(" + spatialDataTypeColumn.NameCamelStyle + ") : 1 == 1) &&");
        }
        else
        {
          stringBuilder1.AppendLine("                           (" + spatialDataTypeColumn.NameCamelStyle + " != null ? " + this._table.LinqFromVariable + "." + spatialDataTypeColumn.Name + " == " + spatialDataTypeColumn.NameCamelStyle + "Value : 1 == 1) &&");
          if (spatialDataTypeColumn.IsDateOrTimeField && spatialDataTypeColumn.IsNullable)
            stringBuilder2.AppendLine("             " + spatialDataTypeColumn.SystemType + "? " + spatialDataTypeColumn.NameCamelStyle + "Value = null;");
          else if (spatialDataTypeColumn.IsNumericField || spatialDataTypeColumn.IsMoneyOrDecimalField || spatialDataTypeColumn.IsDateOrTimeField)
            stringBuilder2.AppendLine("             " + spatialDataTypeColumn.SystemType + " " + spatialDataTypeColumn.NameCamelStyle + "Value = " + spatialDataTypeColumn.SystemType + ".MinValue;");
          else if (spatialDataTypeColumn.SQLDataType == SQLType.bit)
            stringBuilder2.AppendLine("             " + spatialDataTypeColumn.SystemType + " " + spatialDataTypeColumn.NameCamelStyle + "Value = false;");
          stringBuilder3.AppendLine("             if (" + spatialDataTypeColumn.NameCamelStyle + " != null)");
          stringBuilder3.AppendLine("                " + spatialDataTypeColumn.NameCamelStyle + "Value = " + spatialDataTypeColumn.NameCamelStyle + ".Value;");
          stringBuilder3.AppendLine("");
        }
      }
      stringBuilder1.Remove(stringBuilder1.Length - 4, 3);
      stringBuilder1.AppendLine("                       ).Count();");
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Gets the total number of records in the " + this._table.Name + " table based on search parameters");
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         internal static int GetRecordCountDynamicWhere(" + Functions.GetCommaDelimitedParamsForSearchMethod(this._table, Language.CSharp) + ")");
      sb.AppendLine("         {");
      sb.AppendLine("             " + this._contextInitialization);
      sb.AppendLine("");
      if (this._isUseStoredProcedure)
      {
        sb.Append(stringBuilder2.ToString());
        sb.AppendLine("");
        sb.Append(stringBuilder3.ToString());
        sb.Append(stringBuilder1.ToString());
      }
      else
      {
        sb.Append(stringBuilder2.ToString());
        sb.AppendLine("");
        sb.Append(stringBuilder3.ToString());
        sb.Append(stringBuilder1.ToString());
      }
      sb.AppendLine("         }");
    }

    private void WriteSelectSkipAndTakeDynamicWhereMethod(StringBuilder sb)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = new StringBuilder();
      StringBuilder stringBuilder3 = new StringBuilder();
      StringBuilder stringBuilder4 = new StringBuilder();
      StringBuilder stringBuilder5 = new StringBuilder();
      string str1 = " desc";
      string str2 = "OrderByDescending";
      for (int index = 0; index < 2; ++index)
      {
        stringBuilder1.AppendLine("                 switch (sortByExpression)");
        stringBuilder1.AppendLine("                 {");
        foreach (Column column1 in (List<Column>) this._table.Columns)
        {
          if (column1.Name != this._table.FirstPrimaryKeyName)
          {
            stringBuilder1.AppendLine("                     case \"" + column1.Name + str1 + "\":");
            stringBuilder1.AppendLine("                         return context." + this._table.Name);
            stringBuilder1.AppendLine("                             .Where(" + this._table.LinqFromVariable + " =>");
            foreach (Column column2 in (List<Column>) this._table.Columns)
            {
              if (!column2.IsBinaryOrSpatialDataType)
              {
                if (column2.IsStringField)
                  stringBuilder1.AppendLine("                                       (!String.IsNullOrEmpty(" + column2.NameCamelStyle + ") ? " + this._table.LinqFromVariable + "." + column2.Name + ".Contains(" + column2.NameCamelStyle + ") : 1 == 1) &&");
                else
                  stringBuilder1.AppendLine("                                       (" + column2.NameCamelStyle + " != null ? " + this._table.LinqFromVariable + "." + column2.Name + " == " + column2.NameCamelStyle + "Value : 1 == 1) &&");
              }
            }
            stringBuilder1.Remove(stringBuilder1.Length - 4, 3);
            stringBuilder1.AppendLine("                                   )." + str2 + "(" + this._table.LinqFromVariable + " => " + this._table.LinqFromVariable + "." + column1.Name + ").Skip(startRowIndex).Take(rows).ToList();");
            stringBuilder1.AppendLine("");
          }
        }
        stringBuilder1.AppendLine("                     default:");
        stringBuilder1.AppendLine("                         return context." + this._table.Name);
        stringBuilder1.AppendLine("                             .Where(" + this._table.LinqFromVariable + " =>");
        foreach (Column spatialDataTypeColumn in this._table.NoneBinaryOrSpatialDataTypeColumns)
        {
          if (spatialDataTypeColumn.IsStringField)
            stringBuilder1.AppendLine("                                       (!String.IsNullOrEmpty(" + spatialDataTypeColumn.NameCamelStyle + ") ? " + this._table.LinqFromVariable + "." + spatialDataTypeColumn.Name + ".Contains(" + spatialDataTypeColumn.NameCamelStyle + ") : 1 == 1) &&");
          else
            stringBuilder1.AppendLine("                                       (" + spatialDataTypeColumn.NameCamelStyle + " != null ? " + this._table.LinqFromVariable + "." + spatialDataTypeColumn.Name + " == " + spatialDataTypeColumn.NameCamelStyle + "Value : 1 == 1) &&");
        }
        stringBuilder1.Remove(stringBuilder1.Length - 4, 3);
        stringBuilder1.AppendLine("                                   )." + str2 + "(" + this._table.LinqFromVariable + " => " + this._table.LinqFromVariable + "." + this._table.FirstPrimaryKeyName + ").Skip(startRowIndex).Take(rows).ToList();");
        stringBuilder1.AppendLine("                 }");
        if (index == 0)
        {
          stringBuilder2.Append(stringBuilder1.ToString());
          str1 = string.Empty;
          str2 = "OrderBy";
        }
        else
          stringBuilder3.Append(stringBuilder1.ToString());
        stringBuilder1.Clear();
      }
      foreach (Column column in (List<Column>) this._table.Columns)
      {
        if (!column.IsStringField)
        {
          if (column.IsDateOrTimeField && column.IsNullable)
            stringBuilder4.AppendLine("             " + column.SystemType + "? " + column.NameCamelStyle + "Value = null;");
          else if (column.IsNumericField || column.IsMoneyOrDecimalField || column.IsDateOrTimeField)
            stringBuilder4.AppendLine("             " + column.SystemType + " " + column.NameCamelStyle + "Value = " + column.SystemType + ".MinValue;");
          else if (column.SQLDataType == SQLType.bit)
            stringBuilder4.AppendLine("             " + column.SystemType + " " + column.NameCamelStyle + "Value = false;");
          stringBuilder5.AppendLine("             if (" + column.NameCamelStyle + " != null)");
          stringBuilder5.AppendLine("                " + column.NameCamelStyle + "Value = " + column.NameCamelStyle + ".Value;");
          stringBuilder5.AppendLine("");
        }
      }
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Selects " + this._table.Name + " records sorted by the sortByExpression and returns records from the startRowIndex with rows (# of records) based on search parameters");
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         internal static List<" + this._table.Name + "> SelectSkipAndTakeDynamicWhere(" + Functions.GetCommaDelimitedParamsForSearchMethod(this._table, Language.CSharp) + ", string sortByExpression, int startRowIndex, int rows)");
      sb.AppendLine("         {");
      sb.AppendLine("             " + this._contextInitialization);
      sb.AppendLine("");
      sb.Append(stringBuilder4.ToString());
      sb.AppendLine("");
      sb.Append(stringBuilder5.ToString());
      if (this._isUseStoredProcedure)
      {
        sb.AppendLine("             if (sortByExpression.Contains(\" desc\"))");
        sb.AppendLine("             {");
        sb.Append(stringBuilder2.ToString());
        sb.AppendLine("             }");
        sb.AppendLine("             else");
        sb.AppendLine("             {");
        sb.Append(stringBuilder3.ToString());
        sb.AppendLine("             }");
      }
      else
      {
        sb.AppendLine("             if (sortByExpression.Contains(\" desc\"))");
        sb.AppendLine("             {");
        sb.Append(stringBuilder2.ToString());
        sb.AppendLine("             }");
        sb.AppendLine("             else");
        sb.AppendLine("             {");
        sb.Append(stringBuilder3.ToString());
        sb.AppendLine("             }");
      }
      sb.AppendLine("         }");
    }

    private void WriteSelectAllDynamicWhereMethod(StringBuilder sb)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = new StringBuilder();
      StringBuilder stringBuilder3 = new StringBuilder();
      stringBuilder1.AppendLine("             return context." + this._table.Name);
      stringBuilder1.AppendLine("                 .Where(" + this._table.LinqFromVariable + " =>");
      foreach (Column spatialDataTypeColumn in this._table.NoneBinaryOrSpatialDataTypeColumns)
      {
        if (spatialDataTypeColumn.IsStringField)
        {
          stringBuilder1.AppendLine("                           (!String.IsNullOrEmpty(" + spatialDataTypeColumn.NameCamelStyle + ") ? " + this._table.LinqFromVariable + "." + spatialDataTypeColumn.Name + ".Contains(" + spatialDataTypeColumn.NameCamelStyle + ") : 1 == 1) &&");
        }
        else
        {
          stringBuilder1.AppendLine("                           (" + spatialDataTypeColumn.NameCamelStyle + " != null ? " + this._table.LinqFromVariable + "." + spatialDataTypeColumn.Name + " == " + spatialDataTypeColumn.NameCamelStyle + "Value : 1 == 1) &&");
          if (spatialDataTypeColumn.IsNumericField || spatialDataTypeColumn.IsMoneyOrDecimalField || spatialDataTypeColumn.IsDateOrTimeField)
            stringBuilder2.AppendLine("             " + spatialDataTypeColumn.SystemType + " " + spatialDataTypeColumn.NameCamelStyle + "Value = " + spatialDataTypeColumn.SystemType + ".MinValue;");
          else if (spatialDataTypeColumn.SQLDataType == SQLType.bit)
            stringBuilder2.AppendLine("             " + spatialDataTypeColumn.SystemType + " " + spatialDataTypeColumn.NameCamelStyle + "Value = false;");
          stringBuilder3.AppendLine("             if (" + spatialDataTypeColumn.NameCamelStyle + " != null)");
          stringBuilder3.AppendLine("                " + spatialDataTypeColumn.NameCamelStyle + "Value = " + spatialDataTypeColumn.NameCamelStyle + ".Value;");
          stringBuilder3.AppendLine("");
        }
      }
      stringBuilder1.Remove(stringBuilder1.Length - 4, 3);
      stringBuilder1.AppendLine("                       ).ToList();");
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Selects records based on the passed filters as a collection (List) of " + this._table.Name + ".");
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         internal static List<" + this._table.Name + "> SelectAllDynamicWhere(" + Functions.GetCommaDelimitedParamsForSearchMethod(this._table, Language.CSharp) + ")");
      sb.AppendLine("         {");
      sb.AppendLine("             " + this._contextInitialization);
      sb.AppendLine("");
      if (this._isUseStoredProcedure)
      {
        sb.Append(stringBuilder2.ToString());
        sb.AppendLine("");
        sb.Append(stringBuilder3.ToString());
        sb.Append(stringBuilder1.ToString());
      }
      else
      {
        sb.Append(stringBuilder2.ToString());
        sb.AppendLine("");
        sb.Append(stringBuilder3.ToString());
        sb.Append(stringBuilder1.ToString());
      }
      sb.AppendLine("         }");
    }

    private void WriteSelectDropDownListData(StringBuilder sb)
    {
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Selects " + this._table.FirstPrimaryKeyName + " and " + this._table.DataTextField + " columns for use with a DropDownList web control");
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         internal static List<" + this._table.Name + "> Select" + this._table.Name + MyConstants.WordDropDownListData + "()");
      sb.AppendLine("         {");
      sb.AppendLine("             " + this._contextInitialization);
      if (this._isUseStoredProcedure)
      {
        sb.AppendLine("             return (from " + this._table.LinqFromVariable + " in context." + this._table.Name ?? "");
        sb.AppendLine("                     select new " + this._table.Name + " { " + this._table.FirstPrimaryKeyName + " = " + this._table.LinqFromVariable + "." + this._table.FirstPrimaryKeyName + ", " + this._table.DataTextField + " = " + this._table.LinqFromVariable + "." + this._table.DataTextField + " }).ToList();");
      }
      else
      {
        sb.AppendLine("             return (from " + this._table.LinqFromVariable + " in context." + this._table.Name ?? "");
        sb.AppendLine("                     select new " + this._table.Name + " { " + this._table.FirstPrimaryKeyName + " = " + this._table.LinqFromVariable + "." + this._table.FirstPrimaryKeyName + ", " + this._table.DataTextField + " = " + this._table.LinqFromVariable + "." + this._table.DataTextField + " }).ToList();");
      }
      sb.AppendLine("         }");
    }

    private void WriteDeleteMethod(StringBuilder sb)
    {
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Deletes a record based on primary key(s)");
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         internal static void Delete(" + Functions.GetCommaDelimitedPrimaryKeysAndType(this._table, Language.CSharp) + ")");
      sb.AppendLine("         {");
      sb.AppendLine("             " + this._contextInitialization);
      if (this._isUseStoredProcedure)
      {
        if (this._generatedSqlType == GeneratedSqlType.EFCore && this._table.FirstPrimaryKeySQLDataType == SQLType.uniqueidentifier)
          sb.AppendLine("             var " + this._table.VariableObjName + " = context." + this._table.Name + ".Where(" + this._table.LinqFromVariable + " => " + this._table.LinqFromVariable + "." + this._table.FirstPrimaryKeyName + ".ToString() == " + Functions.ConvertToCamel(this._table.FirstPrimaryKeyName) + ").FirstOrDefault();");
        else
          sb.AppendLine("             var " + this._table.VariableObjName + " = context." + this._table.Name + ".Where(" + this._table.LinqFromVariable + " => " + this._table.LinqFromVariable + "." + this._table.FirstPrimaryKeyName + " == " + Functions.ConvertToCamel(this._table.FirstPrimaryKeyName) + ").FirstOrDefault();");
        sb.AppendLine("");
        sb.AppendLine("             if (" + this._table.VariableObjName + " != null)");
        sb.AppendLine("             {");
        sb.AppendLine("                 context." + this._table.Name + ".Remove(" + this._table.VariableObjName + ");");
        sb.AppendLine("                 context.SaveChanges();");
        sb.AppendLine("             }");
      }
      else
      {
        if (this._generatedSqlType == GeneratedSqlType.EFCore && this._table.FirstPrimaryKeySQLDataType == SQLType.uniqueidentifier)
          sb.AppendLine("             var " + this._table.VariableObjName + " = context." + this._table.Name + ".Where(" + this._table.LinqFromVariable + " => " + this._table.LinqFromVariable + "." + this._table.FirstPrimaryKeyName + ".ToString() == " + Functions.ConvertToCamel(this._table.FirstPrimaryKeyName) + ").FirstOrDefault();");
        else
          sb.AppendLine("             var " + this._table.VariableObjName + " = context." + this._table.Name + ".Where(" + this._table.LinqFromVariable + " => " + this._table.LinqFromVariable + "." + this._table.FirstPrimaryKeyName + " == " + Functions.ConvertToCamel(this._table.FirstPrimaryKeyName) + ").FirstOrDefault();");
        sb.AppendLine("");
        sb.AppendLine("             if (" + this._table.VariableObjName + " != null)");
        sb.AppendLine("             {");
        sb.AppendLine("                 context." + this._table.Name + ".Remove(" + this._table.VariableObjName + ");");
        sb.AppendLine("                 context.SaveChanges();");
        sb.AppendLine("             }");
      }
      sb.AppendLine("         }");
    }

    private string IncludeNavProperties()
    {
      int num = 1;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (Column column in (List<Column>) this._table.Columns)
      {
        if (column.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK)
        {
          if (column.Name != column.ForeignKeyColumnName)
            stringBuilder.Append(".Include(" + this._table.LinqFromVariable + " => " + this._table.LinqFromVariable + "." + column.Name + "Navigation)");
          else if (this._table.Name == column.SingularForeignKeyTableName)
          {
            stringBuilder.Append(".Include(" + this._table.LinqFromVariable + " => " + this._table.LinqFromVariable + "." + column.SingularForeignKeyTableName + (object) num + ")");
            ++num;
          }
          else
            stringBuilder.Append(".Include(" + this._table.LinqFromVariable + " => " + this._table.LinqFromVariable + "." + column.SingularForeignKeyTableName + ")");
        }
      }
      return stringBuilder.ToString();
    }

    private void WriteExpressSelectByPrimaryKeyMethod(StringBuilder sb)
    {
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Selects a record by primary key(s)");
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         internal static " + this._table.Name + " SelectByPrimaryKey(" + Functions.GetCommaDelimitedPrimaryKeysAndType(this._table, Language.CSharp) + ")");
      sb.AppendLine("         {");
      sb.AppendLine("            // add your code here");
      sb.AppendLine("            throw new NotImplementedException();");
      sb.AppendLine("         }");
    }

    private void WriteExpressGetRecordCountMethod(StringBuilder sb)
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

    private void WriteExpressGetRecordByCountMethods(StringBuilder sb)
    {
      foreach (Column column in (List<Column>) this._table.Columns)
      {
        if (column.IsForeignKey)
        {
          sb.AppendLine("");
          sb.AppendLine("         /// <summary>");
          sb.AppendLine("         /// Gets the total number of records in the " + this._table.Name + " table by " + column.Name);
          sb.AppendLine("         /// </summary>");
          sb.AppendLine("         internal static int GetRecordCountBy" + column.Name + "(" + column.SystemType + " " + column.NameCamelStyle + ")");
          sb.AppendLine("         {");
          sb.AppendLine("            // add your code here");
          sb.AppendLine("            throw new NotImplementedException();");
          sb.AppendLine("         }");
        }
      }
    }

    private void WriteExpressGetRecordCountDynamicWhere(StringBuilder sb)
    {
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

    private void WriteExpressSelectSkipAndTakeMethod(StringBuilder sb)
    {
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Selects " + this._table.Name + " records sorted by the sortByExpression and returns records from the startRowIndex with rows (# of rows)");
      sb.AppendLine("         /// </summary>");
      if (this._table.IsContainsForeignKeysWithTableSelected)
      {
        sb.AppendLine("         internal static List<" + this._table.Name + "> SelectSkipAndTake(string sortByExpression, int startRowIndex, int rows, bool isIncludeRelatedProperties = true)");
        sb.AppendLine("         {");
        sb.AppendLine("            // add your code here");
        sb.AppendLine("            throw new NotImplementedException();");
        sb.AppendLine("         }");
      }
      else
      {
        sb.AppendLine("         internal static List<" + this._table.Name + "> SelectSkipAndTake(string sortByExpression, int startRowIndex, int rows)");
        sb.AppendLine("         {");
        sb.AppendLine("            // add your code here");
        sb.AppendLine("            throw new NotImplementedException();");
        sb.AppendLine("         }");
      }
    }

    private void WriteExpressSelectSkipAndTakeByMethods(StringBuilder sb)
    {
      foreach (Column column in (List<Column>) this._table.Columns)
      {
        if (column.IsForeignKey)
        {
          sb.AppendLine("");
          sb.AppendLine("         /// <summary>");
          sb.AppendLine("         /// Selects records by " + column.Name + " as a collection (List) of " + this._table.Name + " sorted by the sortByExpression and returns the rows (# of records) starting from the startRowIndex");
          sb.AppendLine("         /// </summary>");
          sb.AppendLine("         internal static List<" + this._table.Name + "> SelectSkipAndTakeBy" + column.Name + "(string sortByExpression, int startRowIndex, int rows, " + column.SystemType + " " + column.NameCamelStyle + ")");
          sb.AppendLine("         {");
          sb.AppendLine("            // add your code here");
          sb.AppendLine("            throw new NotImplementedException();");
          sb.AppendLine("         }");
        }
      }
    }

    private void WriteExpressSelectSkipAndTakeDynamicWhereMethod(StringBuilder sb)
    {
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Selects " + this._table.Name + " records sorted by the sortByExpression and returns records from the startRowIndex with rows (# of records) based on search parameters");
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         internal static List<" + this._table.Name + "> SelectSkipAndTakeDynamicWhere(" + Functions.GetCommaDelimitedParamsForSearchMethod(this._table, Language.CSharp) + ", string sortByExpression, int startRowIndex, int rows)");
      sb.AppendLine("         {");
      sb.AppendLine("            // add your code here");
      sb.AppendLine("            throw new NotImplementedException();");
      sb.AppendLine("         }");
    }

    private void WriteExpressSelectAllMethod(StringBuilder sb)
    {
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Selects all " + this._table.Name);
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         internal static List<" + this._table.Name + "> SelectAll()");
      sb.AppendLine("         {");
      sb.AppendLine("            // add your code here");
      sb.AppendLine("            throw new NotImplementedException();");
      sb.AppendLine("         }");
    }

    private void WriteExpressSelectAllDynamicWhereMethod(StringBuilder sb)
    {
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Selects records based on the passed filters as a collection (List) of " + this._table.Name + ".");
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         internal static List<" + this._table.Name + "> SelectAllDynamicWhere(" + Functions.GetCommaDelimitedParamsForSearchMethod(this._table, Language.CSharp) + ")");
      sb.AppendLine("         {");
      sb.AppendLine("            // add your code here");
      sb.AppendLine("            throw new NotImplementedException();");
      sb.AppendLine("         }");
    }

    private void WriteExpressCollectionByMethods(StringBuilder sb)
    {
      foreach (Column column in (List<Column>) this._table.Columns)
      {
        if (column.IsForeignKey)
        {
          string str = "static List<" + this._table.Name + ">";
          Functions.GetStoredProcNameBy(this._table, column, this._spPrefixSuffixIndex, this._storedProcPrefix, this._storedProcSuffix, StoredProcName.SelectAllBy);
          sb.AppendLine("");
          sb.AppendLine("         /// <summary>");
          sb.AppendLine("         /// Selects all " + this._table.Name + " by " + column.ForeignKeyTableName + ", related to column " + column.Name ?? "");
          sb.AppendLine("         /// </summary>");
          sb.AppendLine("         internal " + str + " Select" + this._table.Name + "CollectionBy" + column.Name + "(" + column.ForeignKeySystemType + " " + column.ForeignKeyColumnNameCamelStyle + ")");
          sb.AppendLine("         {");
          sb.AppendLine("            // add your code here");
          sb.AppendLine("            throw new NotImplementedException();");
          sb.AppendLine("         }");
        }
      }
    }

    private void WriteExpressSelectDropDownListData(StringBuilder sb)
    {
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Selects " + this._table.FirstPrimaryKeyName + " and " + this._table.DataTextField + " columns for use with a DropDownList web control");
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         internal static List<" + this._table.Name + "> Select" + this._table.Name + MyConstants.WordDropDownListData + "()");
      sb.AppendLine("         {");
      sb.AppendLine("            // add your code here");
      sb.AppendLine("            throw new NotImplementedException();");
      sb.AppendLine("         }");
    }

    private void WriteExpressInsertMethod(StringBuilder sb)
    {
      string methodReturnType = Functions.GetInsertMethodReturnType(this._table, Language.CSharp);
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Inserts a record");
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         internal static " + methodReturnType + " Insert(" + this._table.Name + " " + this._table.VariableObjName + ")");
      sb.AppendLine("         {");
      sb.AppendLine("            // add your code here");
      sb.AppendLine("            throw new NotImplementedException();");
      sb.AppendLine("         }");
    }

    private void WriteExpressUpdateMethod(StringBuilder sb)
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

    private void WriteExpressDeleteMethod(StringBuilder sb)
    {
      sb.AppendLine("");
      sb.AppendLine("         /// <summary>");
      sb.AppendLine("         /// Deletes a record based on primary key(s)");
      sb.AppendLine("         /// </summary>");
      sb.AppendLine("         internal static void Delete(" + Functions.GetCommaDelimitedPrimaryKeysAndType(this._table, Language.CSharp) + ")");
      sb.AppendLine("         {");
      sb.AppendLine("            // add your code here");
      sb.AppendLine("            throw new NotImplementedException();");
      sb.AppendLine("         }");
    }
  }
}
