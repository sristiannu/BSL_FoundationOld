
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class EfDbContext
  {
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
    private string _databaseNamePascalAlphaNumericOnly;
    private string _server;
    private string _database;
    private string _username;
    private string _password;

    internal EfDbContext()
    {
    }

    internal EfDbContext(Tables selectedTables, DataTable referencedTables, string apiName, string apiNameDirectory, bool isUseStoredProcedure, int spPrefixSuffixIndex, string storedProcPrefix, string storedProcSuffix, DatabaseObjectToGenerateFrom generateFrom, string server, string database, string username, string password)
    {
      this._selectedTables = selectedTables;
      this._referencedTables = referencedTables;
      this._apiName = apiName;
      this._apiNameDirectory = apiNameDirectory + "\\EF\\";
      this._isUseStoredProcedure = isUseStoredProcedure;
      this._spPrefixSuffixIndex = spPrefixSuffixIndex;
      this._storedProcPrefix = storedProcPrefix;
      this._storedProcSuffix = storedProcSuffix;
      this._generateFrom = generateFrom;
      this._databaseNamePascalAlphaNumericOnly = Functions.ReplaceNoneAlphaNumericWithUnderscore(Functions.ConvertToPascal(database));
      this._server = server;
      this._database = database;
      this._username = username;
      this._password = password;
      this.Generate();
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._apiNameDirectory + this._databaseNamePascalAlphaNumericOnly + "Context.cs"))
      {
        StringBuilder stringBuilder1 = new StringBuilder();
        StringBuilder stringBuilder2 = new StringBuilder();
        StringBuilder stringBuilder3 = new StringBuilder();
        StringBuilder stringBuilder4 = new StringBuilder();
        StringBuilder stringBuilder5 = new StringBuilder();
        StringBuilder stringBuilder6 = new StringBuilder();
        foreach (Table selectedTable in (List<Table>) this._selectedTables)
        {
          int num1 = 1;
          stringBuilder2.AppendLine("            modelBuilder.Entity<" + selectedTable.Name + ">(entity =>");
          stringBuilder2.AppendLine("            {");
          if (selectedTable.PrimaryKeyCount > 0)
          {
            if (selectedTable.PrimaryKeyCount > 1)
              stringBuilder2.AppendLine("                entity.HasKey(e => new { " + Functions.GetCommaDelimitedEDotPrimaryKeys(selectedTable) + " })");
            else
              stringBuilder2.AppendLine("                entity.HasKey(e => " + Functions.GetCommaDelimitedEDotPrimaryKeys(selectedTable) + ")");
            stringBuilder2.AppendLine("                    .HasName(\"" + selectedTable.PrimaryKeyName + "\");");
            stringBuilder2.AppendLine("");
          }
          if (selectedTable.Owner.ToLower() != "dbo")
          {
            stringBuilder2.AppendLine("                entity.ToTable(\"" + selectedTable.NameOriginal.Replace("\"", "\\\"") + "\", \"" + selectedTable.OwnerOriginal.Replace("\"", "\\\"") + "\");");
            stringBuilder2.AppendLine("");
          }
          int num2 = 0;
          foreach (Column column1 in (List<Column>) selectedTable.Columns)
          {
            int num3 = 1;
            if (column1.IndexName != null)
            {
              foreach (string str1 in column1.IndexName)
              {
                if (column1.IndexName.Count > 1 && str1 != column1.Name || column1.IndexName.Count == 1)
                {
                  string str2 = string.Empty;
                  int num4 = 0;
                  foreach (Column column2 in (List<Column>) selectedTable.Columns)
                  {
                    if (column2.IndexName != null)
                    {
                      foreach (string str3 in column2.IndexName)
                      {
                        if ((column2.IndexName.Count > 1 && str3 != column2.Name || column2.IndexName.Count == 1) && str1 == str3)
                        {
                          str2 = str2 + "e." + column2.Name + ", ";
                          ++num4;
                        }
                      }
                    }
                  }
                  if (!string.IsNullOrEmpty(str2))
                  {
                    if (num2 == 0)
                    {
                      stringBuilder3.AppendLine("                entity.HasIndex(e => new { " + Functions.RemoveLastComma(str2.Trim()) + " })");
                      if (!string.IsNullOrEmpty(column1.DefaultValue) && column1.DefaultValue.Contains("Guid().ToString()"))
                      {
                        stringBuilder3.AppendLine("                    .HasName(\"" + str1 + "\")");
                        stringBuilder3.AppendLine("                    .IsUnique();");
                      }
                      else
                        stringBuilder3.AppendLine("                    .HasName(\"" + str1 + "\");");
                      stringBuilder3.AppendLine("");
                      ++num2;
                    }
                    else if (num4 == 1 && num3 == 1)
                    {
                      stringBuilder3.AppendLine("                entity.HasIndex(e => e." + column1.Name + ")");
                      if (!string.IsNullOrEmpty(column1.DefaultValue) && column1.DefaultValue.Contains("Guid().ToString()"))
                      {
                        stringBuilder3.AppendLine("                    .HasName(\"" + str1 + "\")");
                        stringBuilder3.AppendLine("                    .IsUnique();");
                      }
                      else
                        stringBuilder3.AppendLine("                    .HasName(\"" + str1 + "\");");
                      stringBuilder3.AppendLine("");
                      ++num3;
                    }
                  }
                  else
                  {
                    stringBuilder3.AppendLine("                entity.HasIndex(e => e." + column1.Name + ")");
                    if (!string.IsNullOrEmpty(column1.DefaultValue) && column1.DefaultValue.Contains("Guid().ToString()"))
                    {
                      stringBuilder3.AppendLine("                    .HasName(\"" + str1 + "\")");
                      stringBuilder3.AppendLine("                    .IsUnique();");
                    }
                    else
                      stringBuilder3.AppendLine("                    .HasName(\"" + str1 + "\");");
                    stringBuilder3.AppendLine("");
                  }
                }
              }
            }
            stringBuilder4.Append("                entity.Property(e => e." + column1.Name + ")");
            if (column1.Name != column1.NameOriginal)
              stringBuilder4.Append("\r\n                    .HasColumnName(\"" + column1.NameOriginal + "\")");
            if (column1.SQLDataType == SQLType.character || column1.SQLDataType == SQLType.nvarchar || column1.SQLDataType == SQLType.varchar)
              stringBuilder4.Append("\r\n                    .HasMaxLength(" + (object) column1.Length + ")");
            if (!column1.IsNullable && !column1.IsPrimaryKey && string.IsNullOrEmpty(column1.DefaultValueSQL))
              stringBuilder4.Append("\r\n                    .IsRequired()");
            if (string.IsNullOrEmpty(column1.UserDefinedType))
            {
              if (column1.SQLDataType == SQLType.ntext || column1.SQLDataType == SQLType.image || (column1.SQLDataType == SQLType.image || column1.SQLDataType == SQLType.nchar) || (column1.SQLDataType == SQLType.money || column1.SQLDataType == SQLType.datetime || (column1.SQLDataType == SQLType.varchar || column1.SQLDataType == SQLType.binary)) || (column1.SQLDataType == SQLType.character || column1.SQLDataType == SQLType.numeric || (column1.SQLDataType == SQLType.date || column1.SQLDataType == SQLType.smalldatetime) || (column1.SQLDataType == SQLType.smallmoney || column1.SQLDataType == SQLType.text || (column1.SQLDataType == SQLType.varcharmax || column1.SQLDataType == SQLType.text))) || (column1.SQLDataType == SQLType.timestamp || column1.SQLDataType == SQLType.xml))
                stringBuilder4.Append("\r\n                    .HasColumnType(\"" + column1.StoredProcParameter + "\")");
            }
            else
              stringBuilder4.Append("\r\n                    .HasColumnType(\"" + column1.UserDefinedType + "\")");
            if (!string.IsNullOrEmpty(column1.DefaultValue) || !string.IsNullOrEmpty(column1.DefaultValueSQL))
            {
              if (!string.IsNullOrEmpty(column1.DefaultValue) && column1.DefaultValue.Contains("DateTime.Now"))
                stringBuilder4.Append("\r\n                    .HasDefaultValueSql(\"getdate()\")");
              else if (!string.IsNullOrEmpty(column1.DefaultValue) && column1.DefaultValue.Contains("Guid().ToString()"))
                stringBuilder4.Append("\r\n                    .HasDefaultValueSql(\"newid()\")");
              else if (!string.IsNullOrEmpty(column1.DefaultValueSQL))
                stringBuilder4.Append("\r\n                    .HasDefaultValueSql(\"" + column1.DefaultValueSQL + "\")");
            }
            if (column1.IsComputed && !string.IsNullOrEmpty(column1.ComputedFormula))
              stringBuilder4.Append("\r\n                    .HasComputedColumnSql(\"" + column1.ComputedFormula + "\")");
            if (column1.IsComputed || column1.SQLDataType == SQLType.timestamp)
              stringBuilder4.Append("\r\n                    .ValueGeneratedOnAddOrUpdate()");
            if ((column1.SQLDataType == SQLType.integer || column1.SQLDataType == SQLType.bigint) && (!column1.IsIdentity && column1.IsPrimaryKey))
              stringBuilder4.Append("\r\n                    .ValueGeneratedNever()");
            stringBuilder4.Append(";");
            stringBuilder4.AppendLine("");
            stringBuilder4.AppendLine("");
            if (column1.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK)
            {
              if (column1.Name != column1.ForeignKeyColumnName)
                stringBuilder5.AppendLine("                entity.HasOne(d => d." + column1.Name + "Navigation)");
              else if (selectedTable.Name == column1.SingularForeignKeyTableName)
              {
                stringBuilder5.AppendLine("                entity.HasOne(d => d." + column1.SingularForeignKeyTableName + (object) num1 + ")");
                ++num1;
              }
              else
                stringBuilder5.AppendLine("                entity.HasOne(d => d." + column1.SingularForeignKeyTableName + ")");
              if (selectedTable.Name == column1.ForeignKeyTableName)
                stringBuilder5.AppendLine("                    .WithMany(p => p.Inverse" + column1.Name + "Navigation)");
              else if (column1.IsForeignKeyWithTheSameReferencingTable)
                stringBuilder5.AppendLine("                    .WithMany(p => p." + selectedTable.Name + column1.Name + "Navigation)");
              else
                stringBuilder5.AppendLine("                    .WithMany(p => p." + selectedTable.Name + ")");
              stringBuilder5.AppendLine("                    .HasForeignKey(d => d." + column1.Name + ")");
              if (!column1.IsNullable)
                stringBuilder5.AppendLine("                    .OnDelete(DeleteBehavior.Restrict)");
              stringBuilder5.AppendLine("                    .HasConstraintName(\"" + column1.ForeignKeyConstraintName + "\");");
              stringBuilder5.AppendLine("");
            }
          }
          stringBuilder2.Append(stringBuilder3.ToString());
          stringBuilder2.Append(stringBuilder4.ToString());
          stringBuilder2.Append(stringBuilder5.ToString());
          stringBuilder2.AppendLine("            });");
          stringBuilder2.AppendLine("");
          stringBuilder3.Clear();
          stringBuilder4.Clear();
          stringBuilder5.Clear();
          stringBuilder6.AppendLine("        public virtual DbSet<" + selectedTable.Name + "> " + selectedTable.Name + " { get; set; }");
        }
        stringBuilder1.AppendLine("using System;");
        stringBuilder1.AppendLine("using Microsoft.EntityFrameworkCore;");
        stringBuilder1.AppendLine("using Microsoft.EntityFrameworkCore.Metadata;");
        stringBuilder1.AppendLine("using " + this._apiName + ".BusinessObject; ");
        stringBuilder1.AppendLine("");
        stringBuilder1.AppendLine("namespace " + this._apiName);
        stringBuilder1.AppendLine("{");
        stringBuilder1.AppendLine("    public partial class " + this._databaseNamePascalAlphaNumericOnly + "Context : DbContext");
        stringBuilder1.AppendLine("    {");
        stringBuilder1.AppendLine("        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)");
        stringBuilder1.AppendLine("        {");
        stringBuilder1.AppendLine("            #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.");
        stringBuilder1.AppendLine("            optionsBuilder.UseSqlServer(@\"Server=" + this._server + ";Database=" + this._database + ";User ID=" + this._username + ";Password=" + this._password + ";\");");
        stringBuilder1.AppendLine("        }");
        stringBuilder1.AppendLine("");
        stringBuilder1.AppendLine("        protected override void OnModelCreating(ModelBuilder modelBuilder)");
        stringBuilder1.AppendLine("        {");
        stringBuilder1.Append(stringBuilder2.ToString());
        stringBuilder1.AppendLine("        }");
        stringBuilder1.AppendLine("");
        stringBuilder1.Append(stringBuilder6.ToString());
        stringBuilder1.AppendLine("    }");
        stringBuilder1.AppendLine("}");
        streamWriter.Write(stringBuilder1.ToString());
      }
    }
  }
}
