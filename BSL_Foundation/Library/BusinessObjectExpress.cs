
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class BusinessObjectExpress
  {
    private Table _table;
    private Tables _selectedTables;
    private string _fileExtension;
    private string _nameSpace;
    private string _directory;
    private Language _language;

    internal BusinessObjectExpress()
    {
    }

    internal BusinessObjectExpress(Table table, Tables selectedTables, string nameSpace, string path, string businessObjectDirectory, Language language, string fileExtension)
    {
      this._table = table;
      this._selectedTables = selectedTables;
      this._fileExtension = fileExtension;
      this._nameSpace = nameSpace;
      this._directory = path + businessObjectDirectory;
      this._language = language;
      this.Generate();
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._directory + this._table.Name + this._fileExtension))
      {
        StringBuilder sb = new StringBuilder();
        sb.Append("using System; \r\n");
        sb.Append("using System.Data; \r\n");
        sb.Append("using System.Collections.Generic; \r\n");
        sb.Append(" \r\n");
        sb.Append("namespace " + this._nameSpace + ".BusinessObject \r\n");
        sb.Append("{ \r\n");
        sb.Append("     public class " + this._table.Name + "\r\n");
        sb.Append("     { \r\n");
        this.WriteProperties(sb);
        this.WriteConstructor(sb);
        this.WriteMethods(sb);
        sb.Append("     } \r\n");
        sb.Append("} \r\n");
        streamWriter.Write(sb.ToString());
      }
    }

    private void WriteProperties(StringBuilder sb)
    {
      this.WritePropertiesFor4or35(sb);
    }

    private void WritePropertiesFor4or35(StringBuilder sb)
    {
      StringBuilder stringBuilder = new StringBuilder();
      int num = 0;
      foreach (Column column1 in (List<Column>) this._table.Columns)
      {
        Column column = column1;
        sb.Append("         /// <summary> \r\n");
        sb.Append("         /// Gets or Sets " + column.Name + " \r\n");
        sb.Append("         /// </summary> \r\n");
        sb.Append("         public " + column.SystemType + column.NullableChar + " " + column.Name + " { get; set; } \r\n");
        sb.Append(" \r\n");
        if (column.IsForeignKey && this._selectedTables.Find((Predicate<Table>) (t => t.Owner + "." + t.Name == column.ForeignKeyTableOwner + "." + column.ForeignKeyTableName)) != null)
        {
          string foreignKeyTableName = column.ForeignKeyTableName;
          if (stringBuilder.ToString().Contains(column.ForeignKeyTableName + ","))
          {
            ++num;
            foreignKeyTableName += num.ToString();
          }
          string keywordVariableName = Functions.GetNoneKeywordVariableName(foreignKeyTableName, this._language);
          sb.Append("         /// <summary> \r\n");
          sb.Append("         /// Gets or Sets " + column.Name + ".  Related to column " + column.Name + " \r\n");
          sb.Append("         /// </summary> \r\n");
          sb.Append("         public " + column.ForeignKeyTableName + " " + keywordVariableName + " { get; set; } \r\n");
          sb.Append(" \r\n");
          stringBuilder.Append(column.ForeignKeyTableName + ",");
        }
      }
      sb.Append(" \r\n");
    }

    private void WritePropertiesFor2(StringBuilder sb)
    {
      StringBuilder stringBuilder = new StringBuilder();
      int num = 0;
      sb.Append("         // private members \r\n");
      foreach (Column column1 in (List<Column>) this._table.Columns)
      {
        Column column = column1;
        sb.Append("         private " + column.SystemType + column.NullableChar + " _" + column.NameCamelStyle + "; \r\n");
        if (column.IsForeignKey && this._selectedTables.Find((Predicate<Table>) (t => t.Owner + "." + t.Name == column.ForeignKeyTableOwner + "." + column.ForeignKeyTableName)) != null)
        {
          string foreignKeyTableName = column.ForeignKeyTableName;
          if (stringBuilder.ToString().Contains(column.ForeignKeyTableName + ","))
          {
            ++num;
            foreignKeyTableName += num.ToString();
          }
          string keywordVariableName = Functions.GetNoneKeywordVariableName(foreignKeyTableName, this._language);
          sb.Append("         private " + column.ForeignKeyTableName + " _" + Functions.ConvertToCamel(keywordVariableName) + "; \r\n");
          stringBuilder.Append(column.ForeignKeyTableName + ",");
        }
      }
      stringBuilder.Clear();
      sb.Append("\r\n");
      foreach (Column column in (List<Column>) this._table.Columns)
      {
        sb.Append("         /// <summary> \r\n");
        sb.Append("         /// Gets or Sets " + column.Name + " \r\n");
        sb.Append("         /// </summary> \r\n");
        sb.Append("         public " + column.SystemType + column.NullableChar + " " + column.Name + " \r\n");
        sb.Append("         { \r\n");
        sb.Append("             get { return _" + column.NameCamelStyle + "; } \r\n");
        sb.Append("             set { _" + column.NameCamelStyle + " = value; } \r\n");
        sb.Append("         } \r\n");
        sb.Append("\r\n");
      }
    }

    private void WriteConstructor(StringBuilder sb)
    {
      sb.Append("         /// <summary> \r\n");
      sb.Append("         /// Constructor \r\n");
      sb.Append("         /// </summary> \r\n");
      sb.Append("         public " + this._table.Name + "() \r\n");
      sb.Append("         { \r\n");
      sb.Append("         } \r\n");
    }

    private void WriteMethods(StringBuilder sb)
    {
      if (!this._table.IsContainsPrimaryAndForeignKeyColumnsOnly)
        this.WriteSelectByPrimaryKeyMethod(sb);
      this.WriteSelectAllMethod(sb);
      this.WriteCollectionByMethods(sb);
      this.WriteInsertMethod(sb);
      if (!this._table.IsContainsPrimaryAndForeignKeyColumnsOnly)
        this.WriteUpdateMethod(sb);
      this.WriteDeleteMethod(sb);
    }

    private void WriteSelectByPrimaryKeyMethod(StringBuilder sb)
    {
      sb.Append(" \r\n");
      sb.Append("         /// <summary>\r\n");
      sb.Append("         /// Selects a record by primary key(s) \r\n");
      sb.Append("         /// </summary>\r\n");
      sb.Append("         public " + this._table.Name + " SelectByPrimaryKey(" + Functions.GetCommaDelimitedPrimaryKeysAndType(this._table, this._language) + ") \r\n");
      sb.Append("         { \r\n");
      sb.Append("             throw new NotImplementedException(); \r\n");
      sb.Append("         } \r\n");
    }

    private void WriteSelectAllMethod(StringBuilder sb)
    {
      sb.Append(" \r\n");
      sb.Append("         /// <summary> \r\n");
      sb.Append("         /// Selects all records as a collection (List) of " + this._table.Name + " \r\n");
      sb.Append("         /// </summary> \r\n");
      sb.Append("         public List<" + this._table.Name + "> SelectAll() \r\n");
      sb.Append("         { \r\n");
      sb.Append("             throw new NotImplementedException(); \r\n");
      sb.Append("         } \r\n");
    }

    private void WriteCollectionByMethods(StringBuilder sb)
    {
      StringBuilder stringBuilder = new StringBuilder();
      int num = 0;
      foreach (Column column in (List<Column>) this._table.Columns)
      {
        if (column.IsForeignKey)
        {
          string foreignKeyTableName = column.ForeignKeyTableName;
          if (stringBuilder.ToString().Contains(column.ForeignKeyTableName + ","))
          {
            ++num;
            foreignKeyTableName += num.ToString();
          }
          sb.Append(" \r\n");
          sb.Append("         /// <summary>\r\n");
          sb.Append("         /// Selects all " + this._table.Name + " by " + column.ForeignKeyTableName + ", related to column " + column.Name + " \r\n");
          sb.Append("         /// </summary> \r\n");
          sb.Append("         public List<" + this._table.Name + "> Select" + this._table.Name + "CollectionBy" + foreignKeyTableName + "(" + column.ForeignKeySystemType + " " + column.ForeignKeyColumnNameCamelStyle + ") \r\n");
          sb.Append("         { \r\n");
          sb.Append("             throw new NotImplementedException(); \r\n");
          sb.Append("         } \r\n");
          stringBuilder.Append(column.ForeignKeyTableName + ",");
        }
      }
    }

    private void WriteInsertMethod(StringBuilder sb)
    {
      string methodReturnType = Functions.GetInsertMethodReturnType(this._table, Language.CSharp);
      if (methodReturnType == "void")
      {
        string empty = string.Empty;
      }
      sb.Append(" \r\n");
      sb.Append("         /// <summary>\r\n");
      sb.Append("         /// Inserts a record \r\n");
      sb.Append("         /// </summary> \r\n");
      sb.Append("         public " + methodReturnType + " Insert() \r\n");
      sb.Append("         { \r\n");
      sb.Append("             throw new NotImplementedException(); \r\n");
      sb.Append("         } \r\n");
    }

    private void WriteUpdateMethod(StringBuilder sb)
    {
      sb.Append(" \r\n");
      sb.Append("         /// <summary>\r\n");
      sb.Append("         /// Updates a record \r\n");
      sb.Append("         /// </summary> \r\n");
      sb.Append("         public void Update() \r\n");
      sb.Append("         { \r\n");
      sb.Append("             throw new NotImplementedException(); \r\n");
      sb.Append("         } \r\n");
    }

    private void WriteDeleteMethod(StringBuilder sb)
    {
      sb.Append(" \r\n");
      sb.Append("         /// <summary>\r\n");
      sb.Append("         /// Deletes a record based on primary key(s) \r\n");
      sb.Append("         /// </summary>\r\n");
      sb.Append("         public void Delete(" + Functions.GetCommaDelimitedPrimaryKeysAndType(this._table, this._language) + ") \r\n");
      sb.Append("         { \r\n");
      sb.Append("             throw new NotImplementedException(); \r\n");
      sb.Append("         } \r\n");
    }
  }
}
