using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
    internal class ModelBase
    {
        private const Language _language = Language.CSharp;
        private Table _table;
        private const string _fileExtension = ".cs";
        private string _apiRootDirectory;
        private string _apiName;
        private GeneratedSqlType _generatedSqlType;

        internal ModelBase(Table table, string apiName, string apiRootDirectory, GeneratedSqlType generatedSqlType)
        {
            this._table = table;
            this._apiName = apiName;
            this._apiRootDirectory = apiRootDirectory + MyConstants.DirectoryModelBase;
            this._generatedSqlType = generatedSqlType;
            this.Generate();
        }

        private void Generate()
        {
            using (StreamWriter streamWriter = new StreamWriter(this._apiRootDirectory + this._table.Name + MyConstants.WordModelBase + ".cs"))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("using System;");
                sb.AppendLine("using System.ComponentModel.DataAnnotations;");
                sb.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");
                sb.AppendLine("");
                sb.AppendLine("namespace " + this._apiName + "." + MyConstants.WordModelsDotBase);
                sb.AppendLine("{");
                sb.AppendLine("     /// <summary>");
                sb.AppendLine("     /// Base class for " + this._table.Name + MyConstants.WordModel + ".  Do not make changes to this class,");
                sb.AppendLine("     /// instead, put additional code in the " + this._table.Name + MyConstants.WordModel + " class ");
                sb.AppendLine("     /// </summary>");
                sb.AppendLine("     public class " + this._table.Name + MyConstants.WordModelBase);
                sb.AppendLine("     {");
                this.WriteProperties(sb);
                sb.AppendLine("     }");
                sb.AppendLine("}");
                streamWriter.Write(sb.ToString());
            }
        }

        private void WriteProperties(StringBuilder sb)
        {
            foreach (Column column in this._table.Columns)
            {
                string str = column.SystemType + column.NullableChar;
                if (column.SystemType == "bool")
                    str = column.SystemType;
                sb.AppendLine("         /// <summary> ");
                sb.AppendLine("         /// Gets or Sets " + column.Name + " ");
                sb.AppendLine("         /// </summary> ");
                if (!column.IsComputed)
                {
                    if (column.IsIdentity)
                        sb.AppendLine("         [DatabaseGenerated(DatabaseGeneratedOption.Identity)]");
                    if (column.IsPrimaryKey && !column.IsPrimaryKeyUnique)
                        sb.AppendLine("         [Required(ErrorMessage = \"{0} is required!\")]");
                    else if ((!column.IsPrimaryKey || !column.IsPrimaryKeyUnique) && (column.IsNullable || !column.IsPrimaryKeyUnique) && (!column.IsDateWithGetDate && !column.IsIdentity && (!column.IsUniqueIdWithNewId && !column.IsNullable)))
                        sb.AppendLine("         [Required(ErrorMessage = \"{0} is required!\")]");
                    if (column.SystemTypeNative == "DateTime")
                    {
                        sb.AppendLine("         [RegularExpression(@\"^(((((0[13578])|([13578])|(1[02]))[\\-\\/\\s]?((0[1-9])|([1-9])|([1-2][0-9])|(3[01])))|((([469])|(11))[\\-\\/\\s]?((0[1-9])|([1-9])|([1-2][0-9])|(30)))|((02|2)[\\-\\/\\s]?((0[1-9])|([1-9])|([1-2][0-9]))))[\\-\\/\\s]?\\d{4})(\\s(((0[1-9])|([1-9])|(1[0-2]))\\:([0-5][0-9])((\\s)|(\\:([0-5][0-9])\\s))([AM|PM|am|pm]{2,2})))?$\", ErrorMessage = \"{0} must be a valid date!\")]");
                        sb.AppendLine("         [DisplayFormat(DataFormatString = \"{0:M/d/yyyy}\", ApplyFormatInEditMode = true)]");
                    }
                    else if ((column.SQLDataType == SQLType.integer || column.SQLDataType == SQLType.bigint || column.SQLDataType == SQLType.smallint) && (!column.IsPrimaryKeyUnique && !column.IsForeignKey))
                    {
                        if (column.SQLDataType == SQLType.integer)
                            sb.AppendLine("         [Range(typeof(Int32), \"" + int.MinValue + "\", \"" + int.MaxValue + "\", ErrorMessage = \"{0} must be an integer!\")]");
                        else if (column.SQLDataType == SQLType.bigint)
                            sb.AppendLine("         [Range(typeof(Int64), \"" + long.MinValue + "\", \"" + long.MaxValue + "\", ErrorMessage = \"{0} must be an integer!\")]");
                        else
                            sb.AppendLine("         [Range(typeof(Int16), \"" + short.MinValue + "\", \"" + short.MaxValue + "\", ErrorMessage = \"{0} must be an integer!\")]");
                    }
                    else if (column.SQLDataType == SQLType.money || column.SQLDataType == SQLType.smallmoney)
                        sb.AppendLine("         [RegularExpression(@\"[-+]?[0-9]*\\.?[0-9]?[0-9]\", ErrorMessage = \"{0} must be a valid decimal!\")]");
                    else if (column.SQLDataType == SQLType.floatnumber || column.SQLDataType == SQLType.real || column.SQLDataType == SQLType.decimalnumber)
                        sb.AppendLine("         [RegularExpression(@\"[-+]?[0-9]*\\.?[0-9]?[0-9]\", ErrorMessage = \"{0} must be a valid decimal!\")]");
                    if (column.SQLDataType == SQLType.character || column.SQLDataType == SQLType.nchar || (column.SQLDataType == SQLType.nvarchar || column.SQLDataType == SQLType.varchar))
                        sb.AppendLine("         [StringLength(" + column.Length + ", ErrorMessage = \"{0} must be a maximum of {1} characters long!\")]");
                    if (column.SQLDataType == SQLType.uniqueidentifier)
                        sb.AppendLine("         [RegularExpression(@\"^(\\{){0,1}[0-9a-fA-F]{8}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{12}(\\}){0,1}$\", ErrorMessage = \"{0} must be a valid guid!\")]");
                }
                sb.AppendLine("         [Display(Name = \"" + column.NameWithSpaces + "\")]");
                if (this._generatedSqlType == GeneratedSqlType.EFCore && column.SQLDataType == SQLType.uniqueidentifier)
                {
                    if (column.IsNullable)
                        sb.AppendLine("         public Guid? " + column.Name + " { get; set; } ");
                    else
                        sb.AppendLine("         public Guid " + column.Name + " { get; set; } ");
                }
                else
                    sb.AppendLine("         public " + str + " " + column.Name + " { get; set; } ");
                sb.AppendLine("");
                if (column.IsNullable && !column.IsPrimaryKeyUnique && !column.IsForeignKey && (column.SQLDataType == SQLType.integer || column.SQLDataType == SQLType.bigint || (column.SQLDataType == SQLType.smallint || column.SQLDataType == SQLType.tinyint)))
                {
                    sb.AppendLine("         [NotMapped]");
                    sb.AppendLine("         [RegularExpression(\"([0-9]+)\", ErrorMessage = \"{0} must be a number!\")]");
                    if (column.SQLDataType == SQLType.integer)
                        sb.AppendLine("         [Range(typeof(Int32), \"" + int.MinValue + "\", \"" + int.MaxValue + "\", ErrorMessage = \"{0} must be between {1} and {2}!\")]");
                    else if (column.SQLDataType == SQLType.bigint)
                        sb.AppendLine("         [Range(typeof(Int64), \"" + long.MinValue + "\", \"" + long.MaxValue + "\", ErrorMessage = \"{0} must be between {1} and {2}!\")]");
                    else if (column.SQLDataType == SQLType.smallint)
                        sb.AppendLine("         [Range(typeof(Int16), \"" + short.MinValue + "\", \"" + short.MaxValue + "\", ErrorMessage = \"{0} must be between {1} and {2}\")]");
                    else
                        sb.AppendLine("         [Range(typeof(byte), \"" + (byte)0 + "\", \"" + byte.MaxValue + "\", ErrorMessage = \"{0} must be between {1} and {2}!\")]");
                    sb.AppendLine("         [Display(Name = \"" + column.NameWithSpaces + "\")]");
                    sb.AppendLine("         public string " + column.Name + "Hidden { get; set; } ");
                    sb.AppendLine("");
                }
            }
        }
    }
}
