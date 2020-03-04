
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace KPIT_K_Foundation
{
  internal class Column
  {
    private Language _language;
    private Dbase _dbase;
    private string _path;
    private string _connectionString;
    private bool _isUseJQueryValidation;
    private string _semicolon;
    private string _defaultValue;
    private string _nameSpace;

    internal string Name { get; set; }

    internal string NameOriginal { get; set; }

    internal string NameLowerCase { get; set; }

    internal string NameWithSpaces { get; set; }

    internal string NameCamelStyle { get; set; }

    internal string NameGridColumn { get; set; }

    internal SQLType SQLDataType { get; set; }

    internal string DataTypeNumber { get; set; }

    internal string StoredProcParameter { get; set; }

    internal int Precision { get; set; }

    internal int Length { get; set; }

    internal bool IsIdentity { get; set; }

    internal bool IsNullable { get; set; }

    internal bool IsComputed { get; set; }

    internal bool IsBinaryOrSpatialDataType { get; set; }

    internal bool IsMoneyOrDecimalField { get; set; }

    internal bool IsNumericField { get; set; }

    internal bool IsDateOrTimeField { get; set; }

    internal bool IsDateField { get; set; }

    internal bool IsStringField { get; set; }

    internal string GridViewItemStyleHorizontalAlign { get; set; }

    internal bool IsMultiLine { get; set; }

    internal int Scale { get; set; }

    internal string DefaultValue { get; set; }

    internal string DefaultValueSQL { get; set; }

    internal int OrdinalPosition { get; set; }

    internal bool IsPrimaryKey { get; set; }

    internal string RequiredRedAsteriskText { get; set; }

    internal string RequiredFieldValidatorText { get; set; }

    internal bool IsUniqueIdPrimaryKeyWithNewId { get; set; }

    internal string TableName { get; set; }

    internal string TableNameOriginal { get; set; }

    internal string TableOwner { get; set; }

    internal string TableOwnerOriginal { get; set; }

    internal bool IsPrimaryKeyUnique { get; set; }

    internal bool IsUniqueIdWithNewId { get; set; }

    internal bool IsDateWithGetDate { get; set; }

    internal bool IsForeignKey { get; set; }

    internal bool IsForeignKeyWithTheSameReferencingTable { get; set; }

    internal string ForeignKeyTableName { get; set; }

    internal string ForeignKeyTableNameOriginal { get; set; }

    internal string ForeignKeyTableOwner { get; set; }

    internal string ForeignKeyTableOwnerOriginal { get; set; }

    internal string ForeignKeyTableOwnerWithUnderscore { get; set; }

    internal string ForeignKeyColumnName { get; set; }

    internal string ForeignKeyColumnNameOriginal { get; set; }

    internal string ForeignKeyColumnNameCamelStyle { get; set; }

    internal string ForeignKeySystemType { get; set; }

    internal string ForeignKeySystemTypeNative { get; set; }

    internal string ForeignKeyColumnNameNoSpaces { get; set; }

    internal string ForeignKeyTableNameFullyQualifiedBusinessObject { get; set; }

    internal string PluralForeignKeyTableName { get; set; }

    internal string PluralForeignKeyTableNameNoSpace { get; set; }

    internal string PluralForeignKeyTableNameWithUnderscore { get; set; }

    internal string SingularForeignKeyTableName { get; set; }

    internal bool IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK { get; set; }

    internal Table ForeignKeyTable { get; set; }

    internal string DropDownListDataPropertyName { get; set; }

    internal string SystemType { get; set; }

    internal string SystemTypeNative { get; set; }

    internal string NullableChar { get; set; }

    internal bool IsSqlDataMustBeEnclosedInApostrophe { get; set; }

    internal string GridViewColumnField { get; set; }

    internal string GridViewColumnFieldNoSortNoHyperLink { get; set; }

    internal string WebControl { get; set; }

    internal string WebControlReadOnly { get; set; }

    internal string WebControlID { get; set; }

    internal string SearchControlID { get; set; }

    internal string WebControllValue { get; set; }

    internal string WebControlFieldAssignment { get; set; }

    internal string WebControlFieldAssignmentReverse { get; set; }

    internal string WebControlFieldAssignmentReverseClearValues { get; set; }

    internal string WebControlFieldAssignmentLabelReverse { get; set; }

    internal string WebControlAspValidator { get; set; }

    internal WinControlType WinControlType { get; set; }

    internal string WinControlTypeString { get; set; }

    internal string WinControlFieldAssignmentForDataGrid { get; set; }

    internal string WinControlFieldAssignmentForDataGridMaster { get; set; }

    internal string WinControlFieldAssignmentForDataGridDetails { get; set; }

    internal string WinControlFieldAssignmentForDataGridNoType { get; set; }

    internal string WinControlFieldAssignmentForDataGridNoTypeMaster { get; set; }

    internal string WinControlFieldAssignmentForDataGridNoTypeDetails { get; set; }

    internal string WinMaskedTextBoxControlMask { get; set; }

    internal string WinMaskedTextBoxControlValidatingType { get; set; }

    internal string WinMaskedTextBoxControlFieldAssignment { get; set; }

    internal string NullableVariableDeclaration { get; set; }

    internal string VariableFieldAssignment { get; set; }

    internal string VariableFieldAssignmentClearValues { get; set; }

    internal string SampleData { get; set; }

    internal bool IsDescriptionContainCodeGenFormat { get; set; }

    internal string Mask { get; set; }

    internal string MaskInputType { get; set; }

    internal List<string> ReferencingColumnName { get; set; }

    internal List<string> ReferencingTableName { get; set; }

    internal string PrimaryKeyName { get; set; }

    internal string ForeignKeyConstraintName { get; set; }

    internal List<string> IndexName { get; set; }

    internal string ComputedFormula { get; set; }

    public string UserDefinedType { get; set; }

    internal Column()
    {
    }

    internal Column(DataTable dtColumns, DataTable dtPrimaryKeys, DataTable dtForeignKeys, DataTable dtSampleData, DataRow dr, DataRow drMoreColumnInfo, DataRow dtEvenMoreColumnInfo, string connectionString, Language language, string path, bool isUseJQueryValidation, string nameSpace)
    {
      this._language = language;
      this._dbase = new Dbase(connectionString, path);
      this._path = path;
      this._connectionString = connectionString;
      this._isUseJQueryValidation = isUseJQueryValidation;
      this._semicolon = ";";
      this._nameSpace = nameSpace;
      if (this._language == Language.VB)
        this._semicolon = string.Empty;
      this.SetMembers(dtColumns, dtPrimaryKeys, dtForeignKeys, dtSampleData, dr, drMoreColumnInfo, dtEvenMoreColumnInfo, connectionString);
    }

    private void SetMembers(DataTable dtColumns, DataTable dtPrimaryKeys, DataTable dtForeignKeys, DataTable dtSampleData, DataRow dr, DataRow drMoreColumnInfo, DataRow drEvenMoreColumnInfo, string connectionString)
    {
      this.TableNameOriginal = dr["table_name"].ToString().Replace("]", "]]");
      this.TableOwnerOriginal = dr["table_owner"].ToString().Replace("]", "]]");
      this.TableName = Functions.ConvertToPascal(this.TableNameOriginal);
      this.TableName = Functions.ReplaceNoneAlphaNumericWithUnderscore(this.TableName);
      this.TableOwner = Functions.ConvertToPascal(this.TableOwnerOriginal);
      this.TableOwner = Functions.ReplaceNoneAlphaNumericWithUnderscore(this.TableOwner);
      string str1 = dr["column_name"].ToString().Trim();
      this.NameOriginal = str1.Replace("]", "]]");
      this.Name = Functions.ConvertToPascal(str1);
      this.Name = Functions.ReplaceNoneAlphaNumericWithUnderscore(this.Name);
      this.Name = Functions.GetNoneKeywordVariableName(this.Name, this._language);
      if (this._language == Language.VB && this.TableName.ToLower() == this.Name.ToLower())
        this.Name += "1";
      this.NameLowerCase = this.Name.ToLower(CultureInfo.CurrentCulture);
      this.NameWithSpaces = Functions.GetNameWithSpaces(this.Name);
      this.NameCamelStyle = Functions.ConvertToCamel(this.Name);
      this.NameCamelStyle = Functions.ReplaceNoneAlphaNumericWithUnderscore(this.NameCamelStyle);
      this.NameCamelStyle = Functions.GetNoneKeywordVariableName(this.NameCamelStyle, this._language);
      this.NameGridColumn = this.Name + "GridColumn";
      this.SQLDataType = this.GetSQLDataType(drMoreColumnInfo["data_type"].ToString().ToLower().Trim(), drMoreColumnInfo["character_maximum_length"].ToString().ToLower().Trim());
      if (drMoreColumnInfo["user_defined_type"] != null && !string.IsNullOrEmpty(drMoreColumnInfo["user_defined_type"].ToString()))
        this.UserDefinedType = drMoreColumnInfo["user_defined_type"].ToString();
      this.IsNullable = Convert.ToBoolean(dr["nullable"]);
      this.IsIdentity = Convert.ToBoolean(drEvenMoreColumnInfo["is_identity"]);
      this.IsComputed = Convert.ToBoolean(drEvenMoreColumnInfo["is_computed"]);
      this.Scale = Convert.ToInt32(drEvenMoreColumnInfo["scale"]);
      this.Precision = Convert.ToInt32(drEvenMoreColumnInfo["precision"]);
      if (this.IsComputed && drMoreColumnInfo["computed_formula"] != null)
        this.ComputedFormula = drMoreColumnInfo["computed_formula"].ToString().Replace("(", "").Replace(")", "");
      this.Length = drMoreColumnInfo["character_maximum_length"] == DBNull.Value ? 0 : Convert.ToInt32(drMoreColumnInfo["character_maximum_length"]);
      this._defaultValue = dr["column_def"].ToString().Trim().Replace("(", "").Replace(")", "").ToLower();
      if (this._defaultValue.StartsWith("'") && this._defaultValue.EndsWith("'"))
        this._defaultValue = "\"" + this._defaultValue.Substring(1, this._defaultValue.Length - 2) + "\"";
      this.DefaultValueSQL = this._defaultValue;
      this._dbase.GetPrimaryKey(this.TableNameOriginal, this.TableOwnerOriginal);
      this.IsPrimaryKey = this.IsThisAPrimaryKey(dtPrimaryKeys, this.NameOriginal);
      this.IsPrimaryKeyUnique = this.IsPrimaryKey && this.IsIdentity || this.IsPrimaryKey && this.SQLDataType == SQLType.uniqueidentifier && this._defaultValue.ToLower().Contains("newid");
      this.IsUniqueIdPrimaryKeyWithNewId = this.IsPrimaryKey && this.SQLDataType == SQLType.uniqueidentifier && this._defaultValue.ToLower().Contains("newid");
      this.IsUniqueIdWithNewId = this.SQLDataType == SQLType.uniqueidentifier && this._defaultValue.ToLower().Contains("newid");
      this.IsDateWithGetDate = (this.SQLDataType == SQLType.datetime || this.SQLDataType == SQLType.datetime2 || (this.SQLDataType == SQLType.date || this.SQLDataType == SQLType.smalldatetime)) && this._defaultValue.ToLower().Contains("getdate");
      if (this._defaultValue.ToLower().Contains("newid"))
        this.DefaultValue = this._language != Language.CSharp ? "New Guid().ToString()" : "new Guid().ToString()";
      else if (this._defaultValue.ToLower().Contains("getdate"))
        this.DefaultValue = "DateTime.Now";
      this.IsStringField = this.SQLDataType == SQLType.character || this.SQLDataType == SQLType.nchar || (this.SQLDataType == SQLType.ntext || this.SQLDataType == SQLType.nvarchar) || (this.SQLDataType == SQLType.nvarcharmax || this.SQLDataType == SQLType.text || (this.SQLDataType == SQLType.varchar || this.SQLDataType == SQLType.varcharmax)) || (this.SQLDataType == SQLType.uniqueidentifier || this.SQLDataType == SQLType.xml);
      if (!this.IsNullable)
      {
        this.RequiredRedAsteriskText = "<span style=\"color:red;\">*</span>";
        this.RequiredFieldValidatorText = "<asp:RequiredFieldValidator ID=\"Rfv" + this.Name + "\" ControlToValidate=\"Txt" + this.Name + "\" ErrorMessage=\"" + this.NameWithSpaces + " is required.\" Display=\"Dynamic\" runat=\"server\" />";
      }
      string foreignKeyTableName;
      string foreignKeyTableOwner;
      bool isForeignKey;
      string foreignKeyColumnName;
      this._dbase.FindForeignKey(this.TableNameOriginal, this.TableOwnerOriginal, this.NameOriginal, out foreignKeyTableName, out foreignKeyTableOwner, out isForeignKey, out foreignKeyColumnName);
      this.IsForeignKey = isForeignKey;
      if (isForeignKey)
      {
        string str2 = foreignKeyTableOwner;
        string str3 = !(str2.ToLower() != "dbo") ? string.Empty : str2.Substring(0, 1).ToUpper() + str2.Substring(1, str2.Length - 1) + "_";
        this.ForeignKeyTableNameOriginal = foreignKeyTableName.Replace("]", "]]");
        this.ForeignKeyTableName = Functions.ConvertToPascal(foreignKeyTableName);
        this.ForeignKeyTableName = Functions.ReplaceNoneAlphaNumericWithUnderscore(this.ForeignKeyTableName);
        this.ForeignKeyTableOwnerOriginal = foreignKeyTableOwner.Replace("]", "]]");
        this.ForeignKeyTableOwner = Functions.ConvertToPascal(foreignKeyTableOwner);
        this.ForeignKeyTableOwner = Functions.ReplaceNoneAlphaNumericWithUnderscore(this.ForeignKeyTableOwner);
        this.ForeignKeyTableOwnerWithUnderscore = str3;
        this.ForeignKeyColumnNameOriginal = foreignKeyColumnName.Replace("]", "]]");
        this.ForeignKeyColumnName = Functions.ConvertToPascal(foreignKeyColumnName);
        this.ForeignKeyColumnName = Functions.ReplaceNoneAlphaNumericWithUnderscore(this.ForeignKeyColumnName);
        this.ForeignKeyColumnNameNoSpaces = foreignKeyColumnName.Replace(" ", "");
        this.ForeignKeyColumnNameCamelStyle = Functions.ConvertToCamel(foreignKeyColumnName);
        this.ForeignKeyColumnNameCamelStyle = Functions.ReplaceNoneAlphaNumericWithUnderscore(this.ForeignKeyColumnNameCamelStyle);
        this.ForeignKeyTableNameFullyQualifiedBusinessObject = Functions.IsAReservedKeyword(this.ForeignKeyTableName, this._language) || SystemWebUIPage.IsAProperty(this.ForeignKeyTableName) ? (this._language != Language.CSharp ? "BusinessObject." + this.ForeignKeyTableName : this._nameSpace + ".BusinessObject." + this.ForeignKeyTableName) : this.ForeignKeyTableName;
        if (!string.IsNullOrEmpty(this.ForeignKeyColumnName))
        {
          this.PluralForeignKeyTableName = Functions.GetPluralForm(this.ForeignKeyTableName);
          this.PluralForeignKeyTableNameWithUnderscore = this.PluralForeignKeyTableName.Replace(" ", "_");
          this.PluralForeignKeyTableNameNoSpace = this.PluralForeignKeyTableName.Replace(" ", "");
        }
        string singularNameWithUnderScore;
        this.SingularForeignKeyTableName = Functions.GetSingularForm(this.ForeignKeyTableName, out singularNameWithUnderScore);
      }
      this.GetSystemTypeAndStoredProcParam(drMoreColumnInfo);
      string str4 = drMoreColumnInfo["column_description"] == null ? string.Empty : drMoreColumnInfo["column_description"].ToString().ToLower().Trim();
      if (!string.IsNullOrEmpty(str4) && str4.Contains("codegenformat(") && (this.SQLDataType == SQLType.varchar || this.SQLDataType == SQLType.varcharmax || (this.SQLDataType == SQLType.nvarchar || this.SQLDataType == SQLType.nvarcharmax)))
      {
        this.IsDescriptionContainCodeGenFormat = true;
        if (str4.Contains("codegenformat(password)"))
          this.MaskInputType = "password";
        else if (str4.Contains("codegenformat(url)"))
          this.MaskInputType = "url";
        else if (str4.Contains("codegenformat(email)"))
          this.MaskInputType = "email";
        else if (str4.Contains("codegenformat(usphonenumber)"))
          this.Mask = "(999) 999-9999";
        else if (str4.Contains("codegenformat(ssn)"))
          this.Mask = "999-99-9999";
        else if (str4.Contains("codegenformat(pin)"))
          this.Mask = "9999";
        else if (str4.Contains("codegenformat(zipcode5)"))
          this.Mask = "99999";
        else if (str4.Contains("codegenformat(zipcode9)"))
          this.Mask = "99999-9999";
        else if (str4.Contains("codegenformat(ccvisamastercard)"))
          this.Mask = "9999 9999 9999 9999";
        else if (str4.Contains("codegenformat(ccamex)"))
        {
          this.Mask = "9999 999999 99999";
        }
        else
        {
          string str2 = str4.Replace("codegenformat(", "");
          this.Mask = str2.Remove(str2.LastIndexOf(")"));
        }
      }
      this.NullableChar = string.Empty;
      if (this.IsNullable && !this.IsStringField && (this.SystemType != "System.Data.Linq.Binary" && this.SystemType != "object") && !this.IsPrimaryKey)
        this.NullableChar = "?";
      this.IsSqlDataMustBeEnclosedInApostrophe = this.IsStringField || this.SystemType == "Date" || (this.SystemTypeNative == "DateTime" || this.SystemType == "Guid") || (this.SystemType == "System.Xml.Linq.XElement" || this.SystemType == "DateTimeOffset" || this.SystemType == "TimeSpan");
      if (this.IsForeignKey)
      {
        this.ForeignKeySystemType = this.SystemType;
        this.ForeignKeySystemTypeNative = this.SystemTypeNative;
      }
      if (dtSampleData != null && dtSampleData.Rows.Count > 0)
        this.SampleData = dtSampleData.Rows[0][str1].ToString();
      this.IsBinaryOrSpatialDataType = this.SQLDataType == SQLType.binary || this.SQLDataType == SQLType.varbinary || (this.SQLDataType == SQLType.varbinarymax || this.SQLDataType == SQLType.timestamp) || (this.SQLDataType == SQLType.hierarchyid || this.SQLDataType == SQLType.sql_variant || (this.SQLDataType == SQLType.xml || this.SQLDataType == SQLType.geography)) || (this.SQLDataType == SQLType.geometry || this.SQLDataType == SQLType.text || (this.SQLDataType == SQLType.image || this.SQLDataType == SQLType.ntext) || this.SQLDataType == SQLType.uniqueidentifier);
      this.IsMoneyOrDecimalField = this.SQLDataType == SQLType.decimalnumber || this.SQLDataType == SQLType.money || this.SQLDataType == SQLType.smallmoney;
      this.IsNumericField = this.SQLDataType == SQLType.bigint || this.SQLDataType == SQLType.floatnumber || (this.SQLDataType == SQLType.integer || this.SQLDataType == SQLType.money) || (this.SQLDataType == SQLType.numeric || this.SQLDataType == SQLType.real || (this.SQLDataType == SQLType.smallint || this.SQLDataType == SQLType.smallmoney)) || this.SQLDataType == SQLType.tinyint;
      this.IsDateField = false;
      if (this.SQLDataType == SQLType.date || this.SQLDataType == SQLType.datetime || (this.SQLDataType == SQLType.datetime2 || this.SQLDataType == SQLType.datetimeoffset) || (this.SQLDataType == SQLType.smalldatetime || this.SQLDataType == SQLType.time))
      {
        if (this.SQLDataType != SQLType.time)
          this.IsDateField = true;
        this.IsDateOrTimeField = true;
      }
      else
        this.IsDateOrTimeField = false;
      this.GridViewItemStyleHorizontalAlign = this.IsNumericField || this.IsDateOrTimeField ? "ItemStyle-HorizontalAlign=\"Right\"" : (this.SQLDataType != SQLType.bit ? "ItemStyle-HorizontalAlign=\"Left\"" : "ItemStyle-HorizontalAlign=\"Center\"");
      this.SetWebControl();
      this.SetGridViewColumnField();
      this.SetGridViewColumnFieldNoSortExpNoHyperLink();
      this.SetWebControlFieldAssignment();
      this.SetWebControlFieldAssignmentReverse();
      this.SetNullableVariableDeclaration();
      this.GetForeignKeyConstraintName(dtForeignKeys, this.NameOriginal);
    }

    private SQLType GetSQLDataType(string dataType, string maxLength)
    {
      switch (dataType)
      {
        case "bigint":
          return SQLType.bigint;
        case "binary":
          return SQLType.binary;
        case "bit":
          return SQLType.bit;
        case "char":
          return SQLType.character;
        case "date":
          return SQLType.date;
        case "datetime":
          return SQLType.datetime;
        case "datetime2":
          return SQLType.datetime2;
        case "datetimeoffset":
          return SQLType.datetimeoffset;
        case "decimal":
          return SQLType.decimalnumber;
        case "float":
          return SQLType.floatnumber;
        case "geography":
          return SQLType.geography;
        case "geometry":
          return SQLType.geometry;
        case "hierarchyid":
          return SQLType.hierarchyid;
        case "image":
          return SQLType.image;
        case "int":
          return SQLType.integer;
        case "money":
          return SQLType.money;
        case "nchar":
          return SQLType.nchar;
        case "ntext":
          return SQLType.ntext;
        case "numeric":
          return SQLType.numeric;
        case "nvarchar":
          return maxLength == "-1" ? SQLType.nvarcharmax : SQLType.nvarchar;
        case "real":
          return SQLType.real;
        case "smalldatetime":
          return SQLType.smalldatetime;
        case "smallint":
          return SQLType.smallint;
        case "smallmoney":
          return SQLType.smallmoney;
        case "sql_variant":
          return SQLType.sql_variant;
        case "sysname":
          return SQLType.sysname;
        case "text":
          return SQLType.text;
        case "time":
          return SQLType.time;
        case "timestamp":
          return SQLType.timestamp;
        case "tinyint":
          return SQLType.tinyint;
        case "uniqueidentifier":
          return SQLType.uniqueidentifier;
        case "varbinary":
          return maxLength == "-1" ? SQLType.varbinarymax : SQLType.varbinary;
        case "varchar":
          return maxLength == "-1" ? SQLType.varcharmax : SQLType.varchar;
        case "xml":
          return SQLType.xml;
        default:
          return SQLType.other;
      }
    }

    private void GetSystemTypeAndStoredProcParam(DataRow drMoreColumnInfo)
    {
      if (this.SQLDataType == SQLType.integer)
      {
        this.SystemType = this._language != Language.CSharp ? "Integer" : "int";
        this.SystemTypeNative = "Int32";
        this.StoredProcParameter = "int";
      }
      else if (this.SQLDataType == SQLType.bigint)
      {
        this.SystemType = this._language != Language.CSharp ? "Long" : "Int64";
        this.SystemTypeNative = "Int64";
        this.StoredProcParameter = "bigint";
      }
      else if (this.SQLDataType == SQLType.smallint)
      {
        this.SystemType = this._language != Language.CSharp ? "Short" : "Int16";
        this.SystemTypeNative = "Int16";
        this.StoredProcParameter = "smallint";
      }
      else if (this.SQLDataType == SQLType.floatnumber)
      {
        this.SystemType = this._language != Language.CSharp ? "Double" : "double";
        this.SystemTypeNative = "Double";
        this.StoredProcParameter = "float";
      }
      else if (this.SQLDataType == SQLType.real)
      {
        this.SystemType = "Single";
        this.SystemTypeNative = "Single";
        this.StoredProcParameter = "real";
      }
      else if (this.SQLDataType == SQLType.tinyint)
      {
        this.SystemType = this._language != Language.CSharp ? "Byte" : "byte";
        this.SystemTypeNative = "Byte";
        this.StoredProcParameter = "tinyint";
      }
      else if (this.SQLDataType == SQLType.datetime || this.SQLDataType == SQLType.smalldatetime || (this.SQLDataType == SQLType.date || this.SQLDataType == SQLType.datetime2))
      {
        if (this._language == Language.CSharp)
        {
          this.SystemType = "DateTime";
          this.SystemTypeNative = "DateTime";
        }
        else
        {
          this.SystemType = "Date";
          this.SystemTypeNative = "DateTime";
        }
        if (this.SQLDataType == SQLType.datetime)
          this.StoredProcParameter = "datetime";
        else if (this.SQLDataType == SQLType.smalldatetime)
          this.StoredProcParameter = "smalldatetime";
        else if (this.SQLDataType == SQLType.date)
          this.StoredProcParameter = "date";
        else
          this.StoredProcParameter = "datetime2";
      }
      else if (this.SQLDataType == SQLType.time)
      {
        this.SystemType = "TimeSpan";
        this.SystemTypeNative = "TimeSpan";
        this.StoredProcParameter = "time";
      }
      else if (this.SQLDataType == SQLType.datetimeoffset)
      {
        this.SystemType = "DateTimeOffset";
        this.SystemTypeNative = "DateTimeOffset";
        if (this.Precision == 34 && this.Scale == 7)
          this.StoredProcParameter = "datetimeoffset";
        else if (this.Precision == 26 && this.Scale == 0)
          this.StoredProcParameter = "datetimeoffset(0)";
        else if (this.Precision == 28 && this.Scale == 1)
          this.StoredProcParameter = "datetimeoffset(1)";
        else if (this.Precision == 29 && this.Scale == 2)
          this.StoredProcParameter = "datetimeoffset(2)";
        else if (this.Precision == 30 && this.Scale == 3)
          this.StoredProcParameter = "datetimeoffset(3)";
        else if (this.Precision == 31 && this.Scale == 4)
          this.StoredProcParameter = "datetimeoffset(4)";
        else if (this.Precision == 32 && this.Scale == 5)
          this.StoredProcParameter = "datetimeoffset(5)";
        else if (this.Precision == 33 && this.Scale == 6)
          this.StoredProcParameter = "datetimeoffset(6)";
        else if (this.Precision == 34 && this.Scale == 7)
          this.StoredProcParameter = "datetimeoffset(7)";
        else
          this.StoredProcParameter = "datetimeoffset";
      }
      else if (this.SQLDataType == SQLType.bit)
      {
        this.SystemType = this._language != Language.CSharp ? "Boolean" : "bool";
        this.SystemTypeNative = "Boolean";
        this.StoredProcParameter = "bit";
      }
      else if (this.SQLDataType == SQLType.decimalnumber || this.SQLDataType == SQLType.money || (this.SQLDataType == SQLType.smallmoney || this.SQLDataType == SQLType.numeric))
      {
        this.SystemType = this._language != Language.CSharp ? "Decimal" : "decimal";
        this.SystemTypeNative = "Decimal";
        if (this.SQLDataType == SQLType.decimalnumber)
          this.StoredProcParameter = "decimal(" + this.Precision + ", " +  this.Scale + ")";
        else if (this.SQLDataType == SQLType.money)
          this.StoredProcParameter = "money";
        else if (this.SQLDataType == SQLType.smallmoney)
          this.StoredProcParameter = "smallmoney";
        else
          this.StoredProcParameter = "numeric(" +  this.Precision + ", " + this.Scale + ")";
      }
      else if (this.SQLDataType == SQLType.uniqueidentifier)
      {
        this.SystemType = "string";
        this.SystemTypeNative = "string";
        this.StoredProcParameter = "uniqueidentifier";
      }
      else if (this.SQLDataType == SQLType.xml)
      {
        this.SystemType = "string";
        this.SystemTypeNative = "string";
        this.StoredProcParameter = "xml";
      }
      else if (this.SQLDataType == SQLType.binary || this.SQLDataType == SQLType.image || (this.SQLDataType == SQLType.varbinary || this.SQLDataType == SQLType.timestamp))
      {
        this.SystemType = "System.Data.Linq.Binary";
        this.SystemTypeNative = "System.Data.Linq.Binary";
        if (this.SQLDataType == SQLType.binary)
          this.StoredProcParameter = "binary";
        else if (this.SQLDataType == SQLType.image)
          this.StoredProcParameter = "image";
        else if (this.SQLDataType == SQLType.varbinary)
          this.StoredProcParameter = "varbinary";
        else
          this.StoredProcParameter = "timestamp";
      }
      else if (this.SQLDataType == SQLType.sql_variant)
      {
        this.SystemType = "object";
        this.SystemTypeNative = "object";
        this.StoredProcParameter = "sql_variant";
      }
      else
      {
        this.SystemType = this._language != Language.CSharp ? "String" : "string";
        this.SystemTypeNative = "String";
        if (this.SQLDataType == SQLType.character)
          this.StoredProcParameter = "char(" +  this.Length + ")";
        else if (this.SQLDataType == SQLType.nchar)
          this.StoredProcParameter = "nchar(" +  this.Length + ")";
        else if (this.SQLDataType == SQLType.ntext)
          this.StoredProcParameter = "ntext";
        else if (this.SQLDataType == SQLType.nvarchar)
          this.StoredProcParameter = "nvarchar(" + this.Length + ")";
        else if (this.SQLDataType == SQLType.text)
          this.StoredProcParameter = "text";
        else if (this.SQLDataType == SQLType.varchar)
          this.StoredProcParameter = "varchar(" + this.Length + ")";
        else if (this.SQLDataType == SQLType.sysname)
          this.StoredProcParameter = "nvarchar(128)";
        else if (this.SQLDataType == SQLType.varcharmax)
          this.StoredProcParameter = "varchar(max)";
        else if (this.SQLDataType == SQLType.nvarcharmax)
          this.StoredProcParameter = "nvarchar(max)";
        else if (this.SQLDataType == SQLType.varbinarymax)
          this.StoredProcParameter = "varbinary(max)";
        else
          this.StoredProcParameter = drMoreColumnInfo["data_type"].ToString();
      }
    }

    private bool IsThisAPrimaryKey(DataTable dtPrimaryKeys, string columnName)
    {
      bool flag = false;
      foreach (DataRow row in (InternalDataCollectionBase) dtPrimaryKeys.Rows)
      {
        if (row["column_name"].ToString().ToLower() == columnName.ToLower())
        {
          flag = true;
          this.PrimaryKeyName = row["pk_name"].ToString();
          break;
        }
      }
      return flag;
    }

    private void GetForeignKeyConstraintName(DataTable dtForeignKeys, string columnName)
    {
      foreach (DataRow row in (InternalDataCollectionBase) dtForeignKeys.Rows)
      {
        if (row["fkcolumn_name"].ToString().ToLower() == columnName.ToLower())
        {
          this.ForeignKeyConstraintName = row["fk_name"].ToString();
          break;
        }
      }
    }

    private void SetWebControl()
    {
      if (this.IsForeignKey)
      {
        this.WebControlID = "Ddl" + this.Name;
        this.WebControllValue = this.WebControlID + ".SelectedValue";
        this.WebControl = "<asp:DropDownList ID=\"" + this.WebControlID + "\" DataSourceID=\"Ods" + this.ForeignKeyTableName + "DropDownListData\" DataValueField=\"" + this.ForeignKeyColumnName + "\" DataTextField=\"[DataTextField]\" AppendDataBoundItems=\"true\" [CssClass] runat=\"server\"><asp:ListItem Value=\"\">Select One</asp:ListItem></asp:DropDownList>";
        this.WebControlReadOnly = "<asp:TextBox ID=\"Txt" + this.Name + "\" ReadOnly=\"true\" runat=\"server\" />";
      }
      else if (this.IsPrimaryKey && !this.IsPrimaryKeyUnique)
      {
        this.WebControlID = "Txt" + this.Name;
        this.WebControllValue = this.WebControlID + ".Text";
        this.WebControl = "<asp:TextBox ID=\"" + this.WebControlID + "\" [MaxLength] [CssClass] runat=\"server\" />";
        this.WebControlReadOnly = "<asp:TextBox ID=\"" + this.WebControlID + "\" ReadOnly=\"true\" runat=\"server\" />";
      }
      else if (this.IsPrimaryKey)
      {
        this.WebControlID = "Txt" + this.Name;
        this.WebControllValue = this.WebControlID + ".Text";
        this.WebControl = "<asp:TextBox ID=\"" + this.WebControlID + "\" [MaxLength] runat=\"server\" />";
        this.WebControlReadOnly = "<asp:TextBox ID=\"" + this.WebControlID + "\" ReadOnly=\"true\" runat=\"server\" />";
      }
      else if (this.SQLDataType == SQLType.bit)
      {
        this.WebControlID = "Cbx" + this.Name;
        this.WebControllValue = this.WebControlID + ".Checked";
        this.WebControl = "<asp:CheckBox ID=\"" + this.WebControlID + "\" runat=\"server\" />";
        this.WebControlReadOnly = "<asp:CheckBox ID=\"" + this.WebControlID + "\" ReadOnly=\"true\" runat=\"server\" />";
      }
      else
      {
        this.WebControlID = "Txt" + this.Name;
        this.WebControllValue = this.WebControlID + ".Text";
        this.WebControl = "<asp:TextBox ID=\"" + this.WebControlID + "\" [MaxLength] [SkindId] [CssClass] [MultiLine] runat=\"server\" />";
        this.WebControlReadOnly = "<asp:TextBox ID=\"" + this.WebControlID + "\" ReadOnly=\"true\" runat=\"server\" />";
      }
      if (!this._isUseJQueryValidation)
      {
        this.WebControl = this.WebControl.Replace("[CssClass] ", "");
        if (this.SQLDataType != SQLType.bit)
        {
          if (this.IsPrimaryKey && !this.IsPrimaryKeyUnique)
            this.WebControlAspValidator = "<asp:RequiredFieldValidator ID=\"Rfv" + this.Name + "\" ControlToValidate=\"" + this.WebControlID + "\" ErrorMessage=\"" + this.NameWithSpaces + " is required!\" Display=\"Dynamic\" runat=\"server\" />";
          else if (this.IsPrimaryKey && this.IsPrimaryKeyUnique)
            this.WebControlAspValidator = string.Empty;
          else if (!this.IsNullable && this.IsPrimaryKeyUnique)
            this.WebControlAspValidator = string.Empty;
          else if (!this.IsNullable)
            this.WebControlAspValidator = "<asp:RequiredFieldValidator ID=\"Rfv" + this.Name + "\" ControlToValidate=\"" + this.WebControlID + "\" ErrorMessage=\"" + this.NameWithSpaces + " is required!\" Display=\"Dynamic\" runat=\"server\" />";
          else
            this.WebControlAspValidator = string.Empty;
          if (this.SystemTypeNative == "DateTime")
            this.WebControlAspValidator = this.WebControlAspValidator + "<asp:CompareValidator ID=\"Compv" + this.Name + "\" ControlToValidate=\"" + this.WebControlID + "\" ErrorMessage=\"Invalid date!\" Display=\"Dynamic\" Operator=\"DataTypeCheck\" Type=\"Date\" runat=\"server\" />";
          else if ((this.SQLDataType == SQLType.integer || this.SQLDataType == SQLType.bigint || this.SQLDataType == SQLType.smallint) && (!this.IsPrimaryKey && !this.IsForeignKey))
            this.WebControlAspValidator = this.WebControlAspValidator + "<asp:CompareValidator ID=\"Compv" + this.Name + "\" ControlToValidate=\"" + this.WebControlID + "\" ErrorMessage=\"Invalid integer!\" Display=\"Dynamic\" Operator=\"DataTypeCheck\" Type=\"Integer\" runat=\"server\" />";
          else if (this.SQLDataType == SQLType.money || this.SQLDataType == SQLType.smallmoney)
            this.WebControlAspValidator = this.WebControlAspValidator + "<asp:CompareValidator ID=\"Compv" + this.Name + "\" ControlToValidate=\"" + this.WebControlID + "\" ErrorMessage=\"Invalid currency!\" Display=\"Dynamic\" Operator=\"DataTypeCheck\" Type=\"Currency\" runat=\"server\" />";
          else if (this.SQLDataType == SQLType.floatnumber || this.SQLDataType == SQLType.real || this.SQLDataType == SQLType.decimalnumber)
            this.WebControlAspValidator = this.WebControlAspValidator + "<asp:CompareValidator ID=\"Compv" + this.Name + "\" ControlToValidate=\"" + this.WebControlID + "\" ErrorMessage=\"Invalid number!\" Display=\"Dynamic\" Operator=\"DataTypeCheck\" Type=\"Double\" runat=\"server\" />";
        }
      }
      if (!this.IsNullable && this.WebControl.Contains("[CssClass]"))
        this.WebControl = this.WebControl.Replace("[CssClass]", "CssClass=\"{required:true, messages:{required:'" + this.NameWithSpaces + " is required!'}}\"");
      if (this.SystemTypeNative.ToLower() == "datetime" && this.WebControl.Contains("{required:true, "))
      {
        this.WebControl = this.WebControl.Replace("{required:true, ", "{required:true, date:true, ");
        this.WebControl = this.WebControl.Replace("{is required!'}}", "is required!', date:'" + this.NameWithSpaces + " is an invalid date!'}}");
        this.WebControl = this.WebControl.Replace("[SkindId]", "SkinID=\"TextBoxDate\"");
      }
      else if (this.SystemTypeNative.ToLower() == "datetime")
      {
        this.WebControl = this.WebControl.Replace("[CssClass]", "CssClass=\"{date:true, messages:{date:'" + this.NameWithSpaces + " is an invalid date!'}}\"");
        this.WebControl = this.WebControl.Replace("[SkindId]", "SkinID=\"TextBoxDate\"");
      }
      if ((this.SystemType.ToLower().Contains("int") || this.SystemType.ToLower() == "decimal" || (this.SystemType.ToLower() == "single" || this.SystemType.ToLower() == "long") || (this.SystemType.ToLower() == "short" || this.SystemType.ToLower() == "double")) && this.WebControl.Contains("{required:true, "))
      {
        this.WebControl = this.WebControl.Replace("{required:true, ", "{required:true, number:true, ");
        this.WebControl = this.WebControl.Replace("is required!'}}", "is required!', number:'" + this.NameWithSpaces + " is an invalid number!'}}");
      }
      else if (this.SystemType.ToLower().Contains("int") || this.SystemType.ToLower() == "decimal" || (this.SystemType.ToLower() == "single" || this.SystemType.ToLower() == "long") || (this.SystemType.ToLower() == "short" || this.SystemType.ToLower() == "double"))
        this.WebControl = this.WebControl.Replace("[CssClass]", "CssClass=\"{number:true, messages:{number:'" + this.NameWithSpaces + " is an invalid number!'}}\"");
      if (this.SQLDataType == SQLType.character || this.SQLDataType == SQLType.nchar || (this.SQLDataType == SQLType.nvarchar || this.SQLDataType == SQLType.varchar))
        this.WebControl = this.WebControl.Replace("[MaxLength]", "MaxLength=\"" + this.Length + "\"");
      if ((this.SQLDataType == SQLType.character || this.SQLDataType == SQLType.nchar || (this.SQLDataType == SQLType.nvarchar || this.SQLDataType == SQLType.varchar)) && this.Length > 99 || (this.SQLDataType == SQLType.ntext || this.SQLDataType == SQLType.text || (this.SQLDataType == SQLType.xml || this.SQLDataType == SQLType.nvarcharmax) || this.SQLDataType == SQLType.varcharmax))
      {
        this.WebControl = this.WebControl.Replace("[MultiLine]", "TextMode=\"MultiLine\" Rows=\"7\"");
        this.IsMultiLine = true;
      }
      if (this.WebControl.Contains("[CssClass]"))
        this.WebControl = this.WebControl.Replace("[CssClass] ", "");
      if (this.WebControl.Contains("[SkindId]"))
        this.WebControl = this.WebControl.Replace("[SkindId] ", "");
      if (this.WebControl.Contains("[MaxLength]"))
        this.WebControl = this.WebControl.Replace("[MaxLength] ", "");
      if (!this.WebControl.Contains("[MultiLine]"))
        return;
      this.WebControl = this.WebControl.Replace("[MultiLine] ", "");
    }

    private void SetWinControl()
    {
      if (this.IsForeignKey)
      {
        this.WinControlType = WinControlType.ComboBox;
        this.WinControlTypeString = "ComboBox";
        this.WebControlID = "Cbx" + this.Name;
        this.WebControllValue = this.WebControlID + ".SelectedValue";
      }
      else if (this.IsMoneyOrDecimalField)
      {
        this.WinControlType = WinControlType.MaskedTextBoxWithDecimal;
        this.WinMaskedTextBoxControlMask = "999,999,999.00";
        this.WinControlTypeString = "MaskedTextBox";
        this.WebControlID = "Mtb" + this.Name;
        this.WebControllValue = this.WebControlID + ".Text";
      }
      else if (this.IsNumericField)
      {
        if (this.IsIdentity)
        {
          this.WinControlType = WinControlType.TextBox;
          this.WinControlTypeString = "TextBox";
          this.WebControlID = "Txt" + this.Name;
        }
        else
        {
          this.WinControlType = WinControlType.MaskedTextBoxNumeric;
          this.WinMaskedTextBoxControlMask = "999,999,999";
          this.WinControlTypeString = "MaskedTextBox";
          this.WebControlID = "Mtb" + this.Name;
        }
        this.WebControllValue = this.WebControlID + ".Text";
      }
      else if (this.IsDateOrTimeField)
      {
        this.WinControlType = WinControlType.DateTimePicker;
        this.WinControlTypeString = "DateTimePicker";
        this.WebControlID = "Dtp" + this.Name;
        this.WebControllValue = this.WebControlID + ".Text";
      }
      else if (this.SQLDataType == SQLType.bit)
      {
        this.WinControlType = WinControlType.CheckBox;
        this.WinControlTypeString = "CheckBox";
        this.WebControllValue = this.WebControlID + ".Checked";
      }
      else
      {
        this.WinControlType = WinControlType.TextBox;
        this.WinControlTypeString = "TextBox";
        this.WebControllValue = this.WebControlID + ".Text";
      }
      if (this.WinControlTypeString == "MaskedTextBox")
      {
        this.WinMaskedTextBoxControlFieldAssignment = "obj" + this.TableName + "." + this.Name + " = Convert.To" + this.SystemTypeNative + "(mtb" + this.Name + "String)" + this._semicolon;
        this.WinMaskedTextBoxControlValidatingType = this._language != Language.CSharp ? "GetType(" + this.SystemType + ")" : "typeof(" + this.SystemType + ")";
      }
      if (!(this.SystemTypeNative.ToLower() == "datetime") || !this.WebControl.Contains("{required:true, "))
      {
        int num1 = this.SystemTypeNative.ToLower() == "datetime" ? 1 : 0;
      }
      if ((this.SystemType.ToLower().Contains("int") || this.SystemType.ToLower() == "decimal" || (this.SystemType.ToLower() == "single" || this.SystemType.ToLower() == "long") || (this.SystemType.ToLower() == "short" || this.SystemType.ToLower() == "double")) && this.WebControl.Contains("{required:true, ") || (this.SystemType.ToLower().Contains("int") || this.SystemType.ToLower() == "decimal" || (this.SystemType.ToLower() == "single" || this.SystemType.ToLower() == "long") || this.SystemType.ToLower() == "short"))
        return;
      int num2 = this.SystemType.ToLower() == "double" ? 1 : 0;
    }

    private void SetWebControlFieldAssignment()
    {
      string str = this.NameCamelStyle;
      string empty = string.Empty;
      if (this._language == Language.VB && this.Name.ToLower() == this.TableName.ToLower())
        str = this.NameCamelStyle + "1";
      if (this.IsStringField || this.SQLDataType == SQLType.bit)
      {
        this.WebControlFieldAssignment = "obj" + this.TableName + "." + this.Name + " = " + this.WebControllValue + empty + this._semicolon;
        this.VariableFieldAssignment = str + " = " + this.WebControllValue + empty + this._semicolon;
      }
      else if (this.SQLDataType == SQLType.datetimeoffset)
      {
        this.WebControlFieldAssignment = "obj" + this.TableName + "." + this.Name + " = DateTimeOffset.Parse(" + this.WebControllValue + empty + ")" + this._semicolon;
        this.VariableFieldAssignment = str + " = DateTimeOffset.Parse(" + this.WebControllValue + empty + ")" + this._semicolon;
      }
      else if (this.SQLDataType == SQLType.time)
      {
        this.WebControlFieldAssignment = "obj" + this.TableName + "." + this.Name + " = TimeSpan.Parse(" + this.WebControllValue + empty + ")" + this._semicolon;
        this.VariableFieldAssignment = str + " = TimeSpan.Parse(" + this.WebControllValue + empty + ")" + this._semicolon;
      }
      else if (this.SQLDataType == SQLType.uniqueidentifier)
      {
        this.WebControlFieldAssignment = "obj" + this.TableName + "." + this.Name + " = new Guid(" + this.WebControllValue + empty + ")" + this._semicolon;
        this.VariableFieldAssignment = str + " = new Guid(" + this.WebControllValue + empty + ")" + this._semicolon;
      }
      else if (this.SQLDataType == SQLType.timestamp)
      {
        this.WebControlFieldAssignment = "obj" + this.TableName + "." + this.Name + " = Functions.ConvertStringToByteArray(" + this.WebControllValue + empty + ")" + this._semicolon;
        this.VariableFieldAssignment = str + " = Functions.ConvertStringToByteArray(" + this.WebControllValue + empty + ")" + this._semicolon;
      }
      else if (this.SQLDataType == SQLType.xml)
      {
        this.WebControlFieldAssignment = "obj" + this.TableName + "." + this.Name + " = System.Xml.Linq.XElement.Parse(" + this.WebControllValue + empty + ")" + this._semicolon;
        this.VariableFieldAssignment = str + " = System.Xml.Linq.XElement.Parse(" + this.WebControllValue + empty + ")" + this._semicolon;
      }
      else
      {
        this.WebControlFieldAssignment = "obj" + this.TableName + "." + this.Name + " = Convert.To" + this.SystemTypeNative + "(" + this.WebControllValue + empty + ")" + this._semicolon;
        this.VariableFieldAssignment = str + " = Convert.To" + this.SystemTypeNative + "(" + this.WebControllValue + empty + ")" + this._semicolon;
      }
    }

    private void SetWebControlFieldAssignmentReverse()
    {
      if (this.IsStringField)
      {
        this.WebControlFieldAssignmentReverse = this.WebControllValue + " = obj" + this.TableName + "." + this.Name + this._semicolon;
        this.WebControlFieldAssignmentLabelReverse = "Lbl" + this.Name + ".Text = obj" + this.TableName + "." + this.Name + this._semicolon;
        this.WebControlFieldAssignmentReverseClearValues = this.WebControllValue + " = String.Empty" + this._semicolon;
        if (this._language == Language.CSharp)
          this.VariableFieldAssignmentClearValues = this.SystemType + " " + this.NameCamelStyle + " = null" + this._semicolon;
        else if (this.Name.ToLower() == this.TableName.ToLower())
          this.VariableFieldAssignmentClearValues = "Dim " + this.NameCamelStyle + "1 As " + this.SystemType + " = Nothing";
        else
          this.VariableFieldAssignmentClearValues = "Dim " + this.NameCamelStyle + " As " + this.SystemType + " = Nothing";
      }
      else if (this.SQLDataType == SQLType.bit && !this.IsNullable)
      {
        this.WebControlFieldAssignmentReverse = this.WebControllValue + " = obj" + this.TableName + "." + this.Name + this._semicolon;
        this.WebControlFieldAssignmentLabelReverse = "Lbl" + this.Name + ".Text = obj" + this.TableName + "." + this.Name + ".ToString()" + this._semicolon;
        this.WebControlFieldAssignmentReverseClearValues = this.WebControllValue + " = false" + this._semicolon;
        if (this._language == Language.CSharp)
          this.VariableFieldAssignmentClearValues = this.SystemType + "? " + this.NameCamelStyle + " = null" + this._semicolon;
        else if (this.Name.ToLower() == this.TableName.ToLower())
          this.VariableFieldAssignmentClearValues = "Dim " + this.NameCamelStyle + "1 As " + this.SystemType + "? = Nothing";
        else
          this.VariableFieldAssignmentClearValues = "Dim " + this.NameCamelStyle + " As " + this.SystemType + "? = Nothing";
      }
      else if (this.SQLDataType == SQLType.bit && this.IsNullable)
      {
        if (this._language == Language.CSharp)
        {
          this.WebControlFieldAssignmentReverse = this.WebControllValue + " = obj" + this.TableName + "." + this.Name + ".HasValue ? Convert.ToBoolean(obj" + this.TableName + "." + this.Name + ") : false;";
          this.WebControlFieldAssignmentReverseClearValues = this.WebControllValue + " = false;";
        }
        else
        {
          this.WebControlFieldAssignmentReverse = this.WebControllValue + " = If(obj" + this.TableName + "." + this.Name + ".HasValue, Convert.ToBoolean(obj" + this.TableName + "." + this.Name + "), False)";
          this.WebControlFieldAssignmentReverseClearValues = this.WebControllValue + " = False";
        }
        this.WebControlFieldAssignmentLabelReverse = "Lbl" + this.Name + ".Text = obj" + this.TableName + "." + this.Name + ".ToString()" + this._semicolon;
        if (this._language == Language.CSharp)
          this.VariableFieldAssignmentClearValues = this.SystemType + "? " + this.NameCamelStyle + " = null" + this._semicolon;
        else if (this.Name.ToLower() == this.TableName.ToLower())
          this.VariableFieldAssignmentClearValues = "Dim " + this.NameCamelStyle + "1 As " + this.SystemType + "? = Nothing";
        else
          this.VariableFieldAssignmentClearValues = "Dim " + this.NameCamelStyle + " As " + this.SystemType + "? = Nothing";
      }
      else
      {
        this.WebControlFieldAssignmentReverse = this.WebControllValue + " = obj" + this.TableName + "." + this.Name + ".ToString()" + this._semicolon;
        this.WebControlFieldAssignmentLabelReverse = "Lbl" + this.Name + ".Text = obj" + this.TableName + "." + this.Name + ".ToString()" + this._semicolon;
        this.WebControlFieldAssignmentReverseClearValues = this.WebControllValue + " = String.Empty" + this._semicolon;
        string str = "?";
        if (this.SQLDataType == SQLType.xml)
          str = "";
        if (this._language == Language.CSharp)
          this.VariableFieldAssignmentClearValues = this.SystemType + str + " " + this.NameCamelStyle + " = null" + this._semicolon;
        else if (this.Name.ToLower() == this.TableName.ToLower())
          this.VariableFieldAssignmentClearValues = "Dim " + this.NameCamelStyle + "1 As " + this.SystemType + str + " = Nothing";
        else
          this.VariableFieldAssignmentClearValues = "Dim " + this.NameCamelStyle + " As " + this.SystemType + str + " = Nothing";
      }
    }

    private void SetGridViewColumnField()
    {
      if (this.IsForeignKey)
        this.GridViewColumnField = "<asp:HyperLinkField DataTextField=\"" + this.Name + "\" HeaderText=\"" + this.NameWithSpaces + "\" DataNavigateUrlFields=\"" + this.Name + "\" DataNavigateUrlFormatString=\"" + this.SingularForeignKeyTableName + "View.aspx?" + this.ForeignKeyColumnName + "={0}\" SortExpression=\"" + this.Name + "\" " + this.GridViewItemStyleHorizontalAlign + " />";
      else if (this.SQLDataType == SQLType.varchar || this.SQLDataType == SQLType.nvarchar || this.SQLDataType == SQLType.text)
        this.GridViewColumnField = "<asp:BoundField DataField=\"" + this.Name + "\" HeaderText=\"" + this.NameWithSpaces + "\" ReadOnly=\"true\" SortExpression=\"" + this.Name + "\" />";
      else if (this.SQLDataType == SQLType.bit)
        this.GridViewColumnField = "<asp:CheckBoxField DataField=\"" + this.Name + "\" HeaderText=\"" + this.NameWithSpaces + "\" ReadOnly=\"true\" SortExpression=\"" + this.Name + "\" ItemStyle-HorizontalAlign=\"Center\" />";
      else if (this.SQLDataType == SQLType.smalldatetime || this.SQLDataType == SQLType.datetime)
        this.GridViewColumnField = "<asp:BoundField DataField=\"" + this.Name + "\" HeaderText=\"" + this.NameWithSpaces + "\" ReadOnly=\"true\" SortExpression=\"" + this.Name + "\" DataFormatString=\"{0:M/d/yyyy}\" HtmlEncode=\"false\" ItemStyle-HorizontalAlign=\"Right\" />";
      else if (this.SQLDataType == SQLType.bigint || this.SQLDataType == SQLType.smallint || this.SQLDataType == SQLType.integer)
        this.GridViewColumnField = "<asp:BoundField DataField=\"" + this.Name + "\" HeaderText=\"" + this.NameWithSpaces + "\" ReadOnly=\"true\" SortExpression=\"" + this.Name + "\" DataFormatString=\"{0:N0}\" HtmlEncode=\"false\" ItemStyle-HorizontalAlign=\"Right\" />";
      else if (this.SQLDataType == SQLType.money || this.SQLDataType == SQLType.smallmoney)
        this.GridViewColumnField = "<asp:BoundField DataField=\"" + this.Name + "\" HeaderText=\"" + this.NameWithSpaces + "\" ReadOnly=\"true\" SortExpression=\"" + this.Name + "\" DataFormatString=\"{0:c}\" HtmlEncode=\"false\" ItemStyle-HorizontalAlign=\"Right\" />";
      else if (this.SQLDataType == SQLType.floatnumber || this.SQLDataType == SQLType.decimalnumber)
        this.GridViewColumnField = "<asp:BoundField DataField=\"" + this.Name + "\" HeaderText=\"" + this.NameWithSpaces + "\" ReadOnly=\"true\" SortExpression=\"" + this.Name + "\" DataFormatString=\"{0:F2}\" HtmlEncode=\"false\" ItemStyle-HorizontalAlign=\"Right\" />";
      else
        this.GridViewColumnField = "<asp:BoundField DataField=\"" + this.Name + "\" HeaderText=\"" + this.NameWithSpaces + "\" ReadOnly=\"true\" SortExpression=\"" + this.Name + "\" />";
    }

    private void SetGridViewColumnFieldNoSortExpNoHyperLink()
    {
      if (this.SQLDataType == SQLType.varchar || this.SQLDataType == SQLType.nvarchar || this.SQLDataType == SQLType.text)
        this.GridViewColumnFieldNoSortNoHyperLink = "<asp:BoundField DataField=\"" + this.Name + "\" HeaderText=\"" + this.NameWithSpaces + "\" ReadOnly=\"true\" />";
      else if (this.SQLDataType == SQLType.bit)
        this.GridViewColumnFieldNoSortNoHyperLink = "<asp:CheckBoxField DataField=\"" + this.Name + "\" HeaderText=\"" + this.NameWithSpaces + "\" ReadOnly=\"true\" />";
      else if (this.SQLDataType == SQLType.smalldatetime || this.SQLDataType == SQLType.datetime)
        this.GridViewColumnFieldNoSortNoHyperLink = "<asp:BoundField DataField=\"" + this.Name + "\" HeaderText=\"" + this.NameWithSpaces + "\" ReadOnly=\"true\" DataFormatString=\"{0:M/d/yyyy}\" HtmlEncode=\"false\" ItemStyle-HorizontalAlign=\"Right\" />";
      else if (this.SQLDataType == SQLType.bigint || this.SQLDataType == SQLType.smallint || this.SQLDataType == SQLType.integer)
        this.GridViewColumnFieldNoSortNoHyperLink = "<asp:BoundField DataField=\"" + this.Name + "\" HeaderText=\"" + this.NameWithSpaces + "\" ReadOnly=\"true\" DataFormatString=\"{0:N0}\" HtmlEncode=\"false\" ItemStyle-HorizontalAlign=\"Right\" />";
      else if (this.SQLDataType == SQLType.money || this.SQLDataType == SQLType.smallmoney)
        this.GridViewColumnFieldNoSortNoHyperLink = "<asp:BoundField DataField=\"" + this.Name + "\" HeaderText=\"" + this.NameWithSpaces + "\" ReadOnly=\"true\" DataFormatString=\"{0:c}\" HtmlEncode=\"false\" ItemStyle-HorizontalAlign=\"Right\" />";
      else if (this.SQLDataType == SQLType.floatnumber || this.SQLDataType == SQLType.decimalnumber)
        this.GridViewColumnFieldNoSortNoHyperLink = "<asp:BoundField DataField=\"" + this.Name + "\" HeaderText=\"" + this.NameWithSpaces + "\" ReadOnly=\"true\" DataFormatString=\"{0:F2}\" HtmlEncode=\"false\" ItemStyle-HorizontalAlign=\"Right\" />";
      else
        this.GridViewColumnFieldNoSortNoHyperLink = "<asp:BoundField DataField=\"" + this.Name + "\" HeaderText=\"" + this.NameWithSpaces + "\" ReadOnly=\"true\" />";
    }

    private void SetNullableVariableDeclaration()
    {
      string str1 = "?";
      string str2 = " = null";
      string str3 = " = Nothing";
      if (this.IsPrimaryKey || !this.IsNullable || this.SQLDataType == SQLType.xml)
      {
        str2 = this.SQLDataType != SQLType.xml ? string.Empty : " = null";
        str1 = string.Empty;
        str3 = string.Empty;
      }
      if (this.IsStringField)
      {
        str1 = this.SQLDataType != SQLType.time ? string.Empty : "?";
        str2 = " = null";
        str3 = " = Nothing";
      }
      else if ((this.IsNumericField || this.SystemTypeNative == "Int16") && (!this.IsPrimaryKey && !this.IsNullable))
      {
        str2 = " = 0";
        str3 = " = 0";
      }
      else if (this.SQLDataType == SQLType.decimalnumber && !this.IsNullable)
      {
        str2 = " = 0m";
        str3 = " = 0D";
      }
      else if (this.SQLDataType == SQLType.time)
      {
        str2 = " = TimeSpan.MinValue";
        str3 = " = TimeSpan.MinValue";
      }
      else if (this.IsDateOrTimeField)
      {
        str2 = " = DateTime.MinValue";
        str3 = " = DateTime.MinValue";
      }
      if (this._language == Language.CSharp)
        this.NullableVariableDeclaration = this.SystemType + str1 + " " + this.NameCamelStyle + str2 + ";";
      else
        this.NullableVariableDeclaration = "Dim " + this.NameCamelStyle + " As " + this.SystemType + str1 + str3;
    }
  }
}
