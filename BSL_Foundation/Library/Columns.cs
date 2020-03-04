
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class Columns : List<Column>
  {
    private Dbase _dbase;
    private string _path;
    private bool _isUseJQueryValidation;
    private string _nameSpace;

    internal Columns()
    {
    }

    internal Columns(string tableName, string tableOwner, Language language, string connectionString, string path, bool isUseJQueryValidation, string nameSpace)
    {
      this._dbase = new Dbase(connectionString, path);
      this._path = path;
      this._isUseJQueryValidation = isUseJQueryValidation;
      this._nameSpace = nameSpace;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("Exec sp_columns @table_owner = '" + tableOwner + "',  @table_name = '" + tableName + "' ");
      stringBuilder.Append("Exec sp_pkeys @table_owner = '" + tableOwner + "',  @table_name = '" + tableName + "' ");
      stringBuilder.Append("Exec sp_fkeys @fktable_owner = '" + tableOwner + "',  @fktable_name = '" + tableName + "' ");
      stringBuilder.Append("Select Top 1 * From [" + tableOwner + "].[" + tableName + "] ");
      stringBuilder.Append("select isc.column_name, isc.data_type, isc.datetime_precision, isc.numeric_precision, isc.numeric_precision_radix, isc.column_default, isc.numeric_scale, isc.character_maximum_length, ordinal_position, ");
      stringBuilder.Append("(select top 1 name from sys.computed_columns where name = isc.column_name and object_id in (select id from sysobjects where name = '" + tableName + "')) as computed_formula, ");
      stringBuilder.Append("(select top 1 value From sys.extended_properties where name = 'MS_Description' and class_desc = 'OBJECT_OR_COLUMN' and minor_id = ordinal_position and major_id in (select id from sysobjects where name = '" + tableName + "')) as column_description, ");
      stringBuilder.Append("isc.domain_name as user_defined_type ");
      stringBuilder.Append("From information_schema.columns isc ");
      stringBuilder.Append("where table_schema = '" + tableOwner + "' AND table_name = '" + tableName + "' ");
      stringBuilder.Append("select [name], column_id, [precision], scale, is_nullable, is_identity, is_computed ");
      stringBuilder.Append("from sys.columns where [object_id] = object_id('" + tableOwner + "." + tableName + "') ");
      DataSet dataSet1 = this._dbase.GetDataSet(stringBuilder.ToString(), true);
      if (dataSet1 == null)
        return;
      int index1 = 0;
      DataTable table1 = dataSet1.Tables[0];
      DataTable table2 = dataSet1.Tables[1];
      DataTable table3 = dataSet1.Tables[2];
      DataTable table4 = dataSet1.Tables[3];
      DataTable table5 = dataSet1.Tables[4];
      DataTable table6 = dataSet1.Tables[5];
      foreach (DataRow row1 in (InternalDataCollectionBase) table1.Rows)
      {
        DataRow row2 = table5.Rows[index1];
        DataRow row3 = table6.Rows[index1];
        Column column = new Column(table1, table2, table3, table4, row1, row2, row3, connectionString, language, this._path, this._isUseJQueryValidation, this._nameSpace);
        if (!string.IsNullOrEmpty(column.Name) && column.SQLDataType != SQLType.binary && (column.SQLDataType != SQLType.image && column.SQLDataType != SQLType.varbinary) && (column.SQLDataType != SQLType.varbinarymax && column.SQLDataType != SQLType.timestamp && (column.SQLDataType != SQLType.geography && column.SQLDataType != SQLType.geometry)) && (column.SQLDataType != SQLType.hierarchyid && column.SQLDataType != SQLType.sql_variant))
        {
          DataSet dataSet2 = this._dbase.GetDataSet("select ReferencingTableName = quotename(object_name(pt.parent_object_id)), ReferencingColumnName = quotename(pc.name) from sys.foreign_key_columns as pt inner join sys.columns as pc on pt.parent_object_id = pc.[object_id] AND pt.parent_column_id = pc.column_id inner join sys.columns as rc on pt.referenced_column_id = rc.column_id AND pt.referenced_object_id = rc.[object_id] where quotename(object_schema_name(pt.referenced_object_id)) = '[" + tableOwner + "]' and quotename(object_name(pt.referenced_object_id)) = '[" + tableName + "]' ", true);
          if (dataSet2 != null && dataSet2.Tables.Count > 0)
          {
            DataTable table7 = dataSet2.Tables[0];
            if (table7 != null && table7.Rows.Count > 0)
            {
              column.ReferencingTableName = new List<string>();
              column.ReferencingColumnName = new List<string>();
              for (int index2 = 0; index2 < table7.Rows.Count; ++index2)
              {
                column.ReferencingTableName.Add(Functions.ReplaceNoneAlphaNumericWithUnderscore(Functions.ConvertToPascal(table7.Rows[index2]["ReferencingTableName"].ToString().Trim().Replace("[", "").Replace("]", ""))));
                column.ReferencingColumnName.Add(Functions.ReplaceNoneAlphaNumericWithUnderscore(Functions.ConvertToPascal(table7.Rows[index2]["ReferencingColumnName"].ToString().Trim().Replace("[", "").Replace("]", ""))));
              }
            }
          }
          DataSet dataSet3 = this._dbase.GetDataSet("select ind.name IndexName from sys.indexes ind inner join sys.index_columns ic ON  ind.object_id = ic.object_id and ind.index_id = ic.index_id inner join sys.columns col ON ic.object_id = col.object_id and ic.column_id = col.column_id inner join sys.tables t ON ind.object_id = t.object_id where ind.is_primary_key = 0 AND t.is_ms_shipped = 0 AND t.name = '" + column.TableNameOriginal + "' AND col.name = '" + column.NameOriginal + "' ", true);
          if (dataSet1 != null && dataSet3.Tables.Count > 0)
          {
            DataTable table7 = dataSet3.Tables[0];
            if (table7.Rows.Count > 0)
            {
              List<string> stringList = new List<string>();
              foreach (DataRow row4 in (InternalDataCollectionBase) table7.Rows)
                stringList.Add(row4["IndexName"].ToString());
              column.IndexName = stringList;
            }
          }
          this.Add(column);
        }
        ++index1;
      }
      dataSet1.Dispose();
      table1.Dispose();
      table2.Dispose();
      table3.Dispose();
      table4.Dispose();
      table5.Dispose();
      table6.Dispose();
    }
  }
}
