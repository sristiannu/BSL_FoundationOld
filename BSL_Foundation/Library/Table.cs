
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace KPIT_K_Foundation
{
  internal class Table
  {
    private string _path;
    private Language _language;
    private string _nameSpace;
    private string _connectionString;
    private bool _isUseJQueryValidation;

    internal Table()
    {
    }

    internal Table(DataRow dr, string connectionString, Language language, string path, bool isUseJQueryValidation, string nameSpace)
    {
      this._path = path;
      this._language = language;
      this._nameSpace = nameSpace;
      this._connectionString = connectionString;
      this._isUseJQueryValidation = isUseJQueryValidation;
      this.SetMembers(dr);
    }

    internal string Name { get; private set; }

    internal string NameOriginal { get; set; }

    internal string NameLowerCase { get; private set; }

    internal string NameWithUnderScore { get; set; }

    internal string NameFullyQualifiedBusinessObject { get; set; }

    internal string SingularName { get; set; }

    internal string SingularNameWithSpace { get; set; }

    internal string SingularNameWithUnderScore { get; set; }

    internal string PluralName { get; set; }

    internal string PluralNameNoSpace { get; set; }

    internal string PluralNameWithUnderscore { get; set; }

    internal string Owner { get; private set; }

    internal string OwnerOriginal { get; private set; }

    internal string OwnerWithUnderscore { get; set; }

    internal string Type { get; private set; }

    internal int PrimaryKeyCount { get; private set; }

    internal int ForeignKeyCount { get; private set; }

    internal bool IsContainsPrimaryAndForeignKeyColumnsOnly { get; set; }

    internal string LinqFromVariable { get; set; }

    internal string VariableObjName { get; set; }

    internal string VariableEntityName { get; set; }

    internal string VariableObjCollectionName { get; set; }

    internal Columns Columns { get; private set; }

    internal Columns PrimaryKeyColumns { get; set; }

    internal string DataKeyNames { get; set; }

    internal int PrimaryKeyAutoFilledCount { get; set; }

    internal Columns OrderByColumns { get; set; }

    internal string FirstPrimaryKeyName { get; set; }

    internal string FirstPrimaryKeyNameOriginal { get; set; }

    internal string FirstPrimaryKeyStoredProcParameter { get; set; }

    internal string FirstPrimaryKeySystemType { get; set; }

    internal string FirstPrimaryKeySystemTypeNative { get; set; }

    internal SQLType FirstPrimaryKeySQLDataType { get; set; }

    internal Column DataTextFieldColumn { get; set; }

    internal string DataTextField { get; set; }

    internal string DataTextFieldOriginalName { get; set; }

    internal string DataTextFieldSystemType { get; set; }

    internal bool IsDataTextFieldNullable { get; set; }

    internal bool IsDataTextFieldBinaryOrSpatialDataType { get; private set; }

    internal bool IsContainsMoneyOrDecimalField { get; set; }

    internal bool IsContainsNumericFields { get; set; }

    internal bool IsContainsDateFields { get; set; }

    internal bool IsContainsBitFields { get; set; }

    public bool IsContainsForeignKeysWithTableSelected { get; set; }

    internal bool IsContainsCodeGenFormatFields { get; set; }

    internal bool IsGenerateMaskPartialView { get; set; }

    internal string PrimaryKeyName { get; set; }

    internal bool IsContainsBinaryOrSpatialDataTypeColumnsOnly { get; set; }

    internal bool IsContainsBinaryOrSpatialDataTypes { get; set; }

    public IEnumerable<Column> ForeignKeyColumns { get; set; }

    public IEnumerable<Column> ForeignKeyColumnsTableIsSelectedAndOnlyOnePK { get; set; }

    public IEnumerable<Column> NoneBinaryOrSpatialDataTypeColumns { get; set; }

    public IEnumerable<Column> ColumnsWithDropDownListData { get; set; }

    public bool IsContainsHiddenColumns { get; set; }

    private void SetMembers(DataRow dr)
    {
      string str1 = dr["TABLE_NAME"].ToString().Trim();
      string lower = str1.Substring(0, 1).ToLower();
      str1.Substring(0, 1).ToUpper();
      str1.Substring(1, str1.Length - 1);
      string str2 = dr["TABLE_OWNER"].ToString();
      string str3 = !(str2.ToLower() != "dbo") ? string.Empty : str2.Substring(0, 1).ToUpper() + str2.Substring(1, str2.Length - 1) + "_";
      this.NameOriginal = str1.Replace("]", "]]");
      this.Name = Functions.ConvertToPascal(str1);
      this.Name = Functions.ReplaceNoneAlphaNumericWithUnderscore(this.Name);
      this.NameLowerCase = this.Name.ToLower(CultureInfo.CurrentCulture);
      this.NameFullyQualifiedBusinessObject = Functions.IsAReservedKeyword(this.Name, this._language) || SystemWebUIPage.IsAProperty(this.Name) ? (this._language != Language.CSharp ? "BusinessObject." + this.Name : this._nameSpace + ".BusinessObject." + this.Name) : this.Name;
      this.VariableObjName = "obj" + this.Name;
      this.VariableObjCollectionName = "obj" + this.Name + "Col";
      this.VariableEntityName = "ent" + this.Name;
      string singularNameWithUnderScore;
      this.SingularName = Functions.GetSingularForm(this.Name, out singularNameWithUnderScore);
      this.SingularNameWithUnderScore = singularNameWithUnderScore;
      this.SingularNameWithSpace = Functions.GetNameWithSpaces(this.SingularName);
      this.PluralName = Functions.GetPluralForm(this.Name);
      this.PluralNameNoSpace = this.PluralName.Replace(" ", "");
      this.PluralNameWithUnderscore = this.PluralName.Replace(" ", "_");
      this.OwnerOriginal = dr["TABLE_OWNER"].ToString().Replace("]", "]]");
      this.Owner = Functions.ConvertToPascal(str3);
      this.Owner = Functions.ReplaceNoneAlphaNumericWithUnderscore(this.Owner);
      this.OwnerWithUnderscore = str3;
      this.Type = dr["TABLE_TYPE"].ToString().ToLower(CultureInfo.CurrentCulture);
      this.LinqFromVariable = !"[e], ".Contains("[" + lower + "],") ? lower : lower + "1";
      this.Columns = new Columns(dr["TABLE_NAME"].ToString().Trim(), dr["TABLE_OWNER"].ToString(), this._language, this._connectionString, this._path, this._isUseJQueryValidation, this._nameSpace);
      this.SetMembersUsingForeach(this.Columns);
      this.PrimaryKeyCount = this.PrimaryKeyColumns.Count;
      this.GetDataTextField(this.Columns);
    }

    private void GetDataTextField(Columns columns)
    {
      bool flag = false;
      foreach (Column column in (List<Column>) columns)
      {
        if (!column.IsPrimaryKey && !column.IsForeignKey && (!column.IsNullable && column.SQLDataType != SQLType.xml) && (column.SystemType != "object" && column.SystemType != "System.Data.Linq.Binary" && (column.SQLDataType != SQLType.uniqueidentifier && column.SQLDataType != SQLType.bit)) && (column.SQLDataType != SQLType.ntext && column.SQLDataType != SQLType.text && (column.Precision < 301 && column.SystemType.ToLower() == "string")))
        {
          this.DataTextFieldColumn = column;
          this.DataTextField = column.Name;
          this.DataTextFieldOriginalName = column.NameOriginal.Replace("]", "]]");
          this.DataTextFieldSystemType = column.SystemType;
          this.IsDataTextFieldNullable = column.IsNullable;
          this.IsDataTextFieldBinaryOrSpatialDataType = column.IsBinaryOrSpatialDataType;
          flag = true;
          break;
        }
      }
      if (!flag)
      {
        foreach (Column column in (List<Column>) columns)
        {
          if (!column.IsPrimaryKey && !column.IsForeignKey && (column.SQLDataType != SQLType.xml && column.SystemType != "object") && (column.SystemType != "System.Data.Linq.Binary" && column.SQLDataType != SQLType.uniqueidentifier && (column.SQLDataType != SQLType.bit && column.SQLDataType != SQLType.ntext)) && (column.SQLDataType != SQLType.text && column.Precision < 301 && column.SystemType.ToLower() == "string"))
          {
            this.DataTextFieldColumn = column;
            this.DataTextField = column.Name;
            this.DataTextFieldOriginalName = column.NameOriginal.Replace("]", "]]");
            this.DataTextFieldSystemType = column.SystemType;
            this.IsDataTextFieldNullable = column.IsNullable;
            this.IsDataTextFieldBinaryOrSpatialDataType = column.IsBinaryOrSpatialDataType;
            flag = true;
            break;
          }
        }
      }
      if (!flag)
      {
        foreach (Column column in (List<Column>) columns)
        {
          if (!column.IsPrimaryKey && !column.IsForeignKey && (column.SQLDataType != SQLType.xml && column.SystemType != "object") && (column.SystemType != "System.Data.Linq.Binary" && column.SQLDataType != SQLType.uniqueidentifier && (column.SQLDataType != SQLType.bit && column.SQLDataType != SQLType.ntext)) && (column.SQLDataType != SQLType.text && column.Precision < 301))
          {
            this.DataTextFieldColumn = column;
            this.DataTextField = column.Name;
            this.DataTextFieldOriginalName = column.NameOriginal.Replace("]", "]]");
            this.DataTextFieldSystemType = column.SystemType;
            this.IsDataTextFieldNullable = column.IsNullable;
            this.IsDataTextFieldBinaryOrSpatialDataType = column.IsBinaryOrSpatialDataType;
            flag = true;
            break;
          }
        }
      }
      if (flag)
        return;
      foreach (Column column in (List<Column>) columns)
      {
        if (!column.IsPrimaryKey && !column.IsForeignKey && (column.SQLDataType != SQLType.xml && column.SystemType != "object") && (column.SystemType != "System.Data.Linq.Binary" && column.SQLDataType != SQLType.uniqueidentifier))
        {
          this.DataTextFieldColumn = column;
          this.DataTextField = column.Name;
          this.DataTextFieldOriginalName = column.NameOriginal.Replace("]", "]]");
          this.DataTextFieldSystemType = column.SystemType;
          this.IsDataTextFieldNullable = column.IsNullable;
          this.IsDataTextFieldBinaryOrSpatialDataType = column.IsBinaryOrSpatialDataType;
          break;
        }
      }
    }

    private void SetMembersUsingForeach(Columns columns)
    {
      this.PrimaryKeyColumns = new Columns();
      this.OrderByColumns = new Columns();
      bool flag = false;
      int num1 = 0;
      string str = string.Empty;
      int num2 = 0;
      this.IsContainsBinaryOrSpatialDataTypeColumnsOnly = true;
      this.IsContainsBinaryOrSpatialDataTypes = false;
      foreach (Column column in (List<Column>) columns)
      {
        if (column.SQLDataType == SQLType.uniqueidentifier)
        {
          flag = true;
          break;
        }
      }
      foreach (Column column in (List<Column>) columns)
      {
        if (column.IsPrimaryKey)
        {
          this.PrimaryKeyColumns.Add(column);
          str = str + column.Name + ",";
          if (num1 == 0)
          {
            this.FirstPrimaryKeyName = column.Name;
            this.FirstPrimaryKeyNameOriginal = column.NameOriginal.Replace("]", "]]");
            this.FirstPrimaryKeySystemType = column.SystemType;
            this.FirstPrimaryKeySystemTypeNative = column.SystemTypeNative;
            this.FirstPrimaryKeyStoredProcParameter = column.StoredProcParameter;
            this.FirstPrimaryKeySQLDataType = column.SQLDataType;
            ++num1;
          }
          if (column.IsPrimaryKeyUnique)
            ++this.PrimaryKeyAutoFilledCount;
        }
        if (column.IsForeignKey)
          ++this.ForeignKeyCount;
        if (column.SQLDataType != SQLType.text && column.SQLDataType != SQLType.ntext && (column.SQLDataType != SQLType.image && column.SQLDataType != SQLType.xml) && (column.SQLDataType != SQLType.sql_variant && column.SQLDataType != SQLType.datetimeoffset) || column.SQLDataType == SQLType.datetime && !flag)
          this.OrderByColumns.Add(column);
        if (!column.IsPrimaryKey && !column.IsForeignKey)
          ++num2;
        if (column.SQLDataType == SQLType.decimalnumber || column.SQLDataType == SQLType.money || column.SQLDataType == SQLType.smallmoney)
          this.IsContainsMoneyOrDecimalField = true;
        if (column.SQLDataType == SQLType.decimalnumber || column.SQLDataType == SQLType.money || (column.SQLDataType == SQLType.smallmoney || column.SQLDataType == SQLType.numeric) || (column.SQLDataType == SQLType.integer || column.SQLDataType == SQLType.smallint || (column.SQLDataType == SQLType.bigint || column.SQLDataType == SQLType.floatnumber)) || column.SQLDataType == SQLType.real)
          this.IsContainsNumericFields = true;
        if (column.SQLDataType == SQLType.datetime || column.SQLDataType == SQLType.smalldatetime || (column.SQLDataType == SQLType.date || column.SQLDataType == SQLType.datetime2))
          this.IsContainsDateFields = true;
        if (column.SQLDataType == SQLType.bit)
          this.IsContainsBitFields = true;
        if (column.IsDescriptionContainCodeGenFormat)
        {
          this.IsContainsCodeGenFormatFields = true;
          if (column.MaskInputType == "password" || column.MaskInputType == "url" || column.MaskInputType == "email")
            this.IsGenerateMaskPartialView = true;
        }
        if (!column.IsBinaryOrSpatialDataType)
          this.IsContainsBinaryOrSpatialDataTypeColumnsOnly = false;
        else
          this.IsContainsBinaryOrSpatialDataTypes = true;
      }
      this.DataKeyNames = string.IsNullOrEmpty(str) ? (string) null : str.Remove(str.Length - 1, 1);
      if (num2 != 0)
        return;
      this.IsContainsPrimaryAndForeignKeyColumnsOnly = true;
    }
  }
}
