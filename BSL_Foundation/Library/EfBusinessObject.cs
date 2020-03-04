
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class EfBusinessObject
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

    internal EfBusinessObject()
    {
    }

    internal EfBusinessObject(Table table, Tables selectedTables, DataTable referencedTables, string apiName, string apiNameDirectory, bool isUseStoredProcedure, int spPrefixSuffixIndex, string storedProcPrefix, string storedProcSuffix, DatabaseObjectToGenerateFrom generateFrom)
    {
      this._table = table;
      this._selectedTables = selectedTables;
      this._referencedTables = referencedTables;
      this._apiName = apiName;
      this._apiNameDirectory = apiNameDirectory + "\\EF\\";
      this._isUseStoredProcedure = isUseStoredProcedure;
      this._spPrefixSuffixIndex = spPrefixSuffixIndex;
      this._storedProcPrefix = storedProcPrefix;
      this._storedProcSuffix = storedProcSuffix;
      this._generateFrom = generateFrom;
      this.Generate();
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._apiNameDirectory + this._table.Name + ".cs"))
      {
        StringBuilder stringBuilder1 = new StringBuilder();
        StringBuilder stringBuilder2 = new StringBuilder();
        StringBuilder stringBuilder3 = new StringBuilder();
        StringBuilder stringBuilder4 = new StringBuilder();
        int num = 1;
        List<string> stringList = new List<string>();
        foreach (Column column in (List<Column>) this._table.Columns)
        {
          if (column.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK)
          {
            if (column.Name != column.ForeignKeyColumnName)
              stringBuilder4.AppendLine("        public virtual " + column.ForeignKeyTableName + " " + column.Name + "Navigation { get; set; }");
            else if (this._table.Name == column.SingularForeignKeyTableName)
            {
              stringBuilder4.AppendLine("        public virtual " + column.ForeignKeyTableName + " " + column.SingularForeignKeyTableName + (object) num + " { get; set; }");
              ++num;
            }
            else
              stringBuilder4.AppendLine("        public virtual " + column.ForeignKeyTableName + " " + column.SingularForeignKeyTableName + " { get; set; }");
            if (this._table.Name == column.ForeignKeyTableName)
              stringBuilder4.AppendLine("        public virtual ICollection<" + this._table.Name + "> Inverse" + column.Name + "Navigation { get; set; }");
          }
          if (column.ReferencingTableName != null && column.IsPrimaryKey && column.ReferencingTableName.Count > 0)
          {
            for (int index = 0; index < column.ReferencingTableName.Count; ++index)
            {
              string referencingTableName = column.ReferencingTableName[index];
              string str = column.ReferencingColumnName[index];
              if (this._table.Name != referencingTableName && this._selectedTables.Where<Table>((Func<Table, bool>) (t => t.Name == referencingTableName)).Count<Table>() > 0 && !stringList.Contains(referencingTableName + str))
              {
                if (column.ReferencingTableName.Where<string>((Func<string, bool>) (r => r == referencingTableName)).Count<string>() > 1)
                {
                  stringBuilder2.AppendLine("            " + referencingTableName + str + "Navigation = new HashSet<" + referencingTableName + ">();");
                  stringBuilder3.AppendLine("        public virtual ICollection<" + referencingTableName + "> " + referencingTableName + str + "Navigation { get; set; }");
                }
                else
                {
                  stringBuilder2.AppendLine("            " + referencingTableName + " = new HashSet<" + referencingTableName + ">();");
                  stringBuilder3.AppendLine("        public virtual ICollection<" + referencingTableName + "> " + referencingTableName + " { get; set; }");
                }
                stringList.Add(referencingTableName + str);
              }
            }
          }
        }
        stringBuilder1.AppendLine("using System;");
        stringBuilder1.AppendLine("using System.Collections.Generic;");
        stringBuilder1.AppendLine("");
        stringBuilder1.AppendLine("namespace " + this._apiName + ".BusinessObject");
        stringBuilder1.AppendLine("{");
        stringBuilder1.AppendLine("    public partial class " + this._table.Name);
        stringBuilder1.AppendLine("    {");
        stringBuilder1.AppendLine("        public " + this._table.Name + "()");
        stringBuilder1.AppendLine("        {");
        stringBuilder1.Append(stringBuilder2.ToString());
        stringBuilder1.AppendLine("        }");
        stringBuilder1.AppendLine("");
        stringBuilder1.Append(stringBuilder3.ToString());
        stringBuilder1.Append(stringBuilder4.ToString());
        stringBuilder1.AppendLine("    }");
        stringBuilder1.AppendLine("}");
        streamWriter.Write(stringBuilder1.ToString());
      }
    }
  }
}
