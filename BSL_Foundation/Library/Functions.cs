
using Microsoft.CSharp;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace KPIT_K_Foundation
{
    internal class Functions
    {
        internal Functions()
        {
        }

        internal static string GetForeignKeysAndType(Table table, Language language)
        {
            int num = 0;
            StringBuilder stringBuilder = new StringBuilder();
            foreach(Column foreignKeyColumn in table.ForeignKeyColumns)
            {
                if (num > 0)
                    stringBuilder.Append(", ");
                if (language == Language.CSharp)
                {
                    stringBuilder.Append(foreignKeyColumn.SystemType + " " + foreignKeyColumn.NameCamelStyle);
                }
                else
                {
                    string nameCamelStyle = foreignKeyColumn.NameCamelStyle;
                    if (foreignKeyColumn.Name.ToLower() == table.Name.ToLower())
                        nameCamelStyle += "1";
                    stringBuilder.Append("ByVal " + nameCamelStyle + " As " + foreignKeyColumn.SystemType);
                }
                    ++num;
            }
            return stringBuilder.ToString();
        }
            internal static string GetCommaDelimitedPrimaryKeysAndType(Table table, Language language)
        {
            int num = 0;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Column primaryKeyColumn in (List<Column>)table.PrimaryKeyColumns)
            {
                if (num > 0)
                    stringBuilder.Append(", ");
                if (language == Language.CSharp)
                {
                    stringBuilder.Append(primaryKeyColumn.SystemType + " " + primaryKeyColumn.NameCamelStyle);
                }
                else
                {
                    string nameCamelStyle = primaryKeyColumn.NameCamelStyle;
                    if (primaryKeyColumn.Name.ToLower() == table.Name.ToLower())
                        nameCamelStyle += "1";
                    stringBuilder.Append("ByVal " + nameCamelStyle + " As " + primaryKeyColumn.SystemType);
                }
                ++num;
            }
            return stringBuilder.ToString();
        }

        internal static string GetCommaDelimitedPrimaryKeys(Table table, Language language, bool isUseCamelStylePrimaryKeys = true, bool isUseUnderScoreBeforePk = false, bool isAdd1OnTheEndOfVariable = false)
        {
            int num = 0;
            string str1 = string.Empty;
            StringBuilder stringBuilder = new StringBuilder();
            if (isUseUnderScoreBeforePk)
                str1 = "_";
            foreach (Column primaryKeyColumn in (List<Column>)table.PrimaryKeyColumns)
            {
                string str2 = primaryKeyColumn.NameCamelStyle;
                if (num > 0)
                    stringBuilder.Append(", ");
                if (!isUseCamelStylePrimaryKeys)
                    str2 = primaryKeyColumn.Name;
                if (language == Language.CSharp)
                    stringBuilder.Append(str1 + str2);
                else if (primaryKeyColumn.Name.ToLower() == table.Name.ToLower() | isAdd1OnTheEndOfVariable)
                    stringBuilder.Append(str1 + str2 + "1");
                else
                    stringBuilder.Append(str1 + str2);
                ++num;
            }
            return stringBuilder.ToString();
        }

        internal static string GetCommaDelimitedModelDotPrimaryKeys(Table table, GeneratedSqlType generatedSqlType)
        {
            int num = 0;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Column primaryKeyColumn in (List<Column>)table.PrimaryKeyColumns)
            {
                if (num > 0)
                    stringBuilder.Append(", ");
                if (generatedSqlType == GeneratedSqlType.EFCore && primaryKeyColumn.SQLDataType == SQLType.uniqueidentifier)
                    stringBuilder.Append("model." + primaryKeyColumn.Name + ".ToString()");
                else
                    stringBuilder.Append("model." + primaryKeyColumn.Name);
                ++num;
            }
            return stringBuilder.ToString();
        }

        internal static string GetCommaDelimitedTableNameDotPrimaryKeys(Table table)
        {
            int num = 0;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Column primaryKeyColumn in (List<Column>)table.PrimaryKeyColumns)
            {
                string str = string.Empty;
                if (num > 0)
                    stringBuilder.Append(", ");
                if (primaryKeyColumn.SQLDataType == SQLType.uniqueidentifier)
                    str = ".ToString()";
                stringBuilder.Append(table.Name + "." + primaryKeyColumn.Name + str);
                ++num;
            }
            return stringBuilder.ToString();
        }

        internal static string GetCommaDelimitedEDotPrimaryKeys(Table table)
        {
            int num = 0;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Column primaryKeyColumn in (List<Column>)table.PrimaryKeyColumns)
            {
                if (num > 0)
                    stringBuilder.Append(", ");
                stringBuilder.Append("e." + primaryKeyColumn.Name);
                ++num;
            }
            return stringBuilder.ToString();
        }

        internal static string GetCommaDelimitedPrimaryKeysUrlParameters(Table table, Language language)
        {
            int num = 0;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Column primaryKeyColumn in (List<Column>)table.PrimaryKeyColumns)
            {
                if (num > 0)
                    stringBuilder.Append(" + \"&");
                if (language == Language.CSharp)
                    stringBuilder.Append(primaryKeyColumn.NameCamelStyle + "=\" + id" + (object)num);
                ++num;
            }
            stringBuilder.Append(" + \"");
            return stringBuilder.ToString();
        }

        internal static string GetPrimaryKeysAtItemUrlParameters(Table table, Language language)
        {
            int num = 0;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Column primaryKeyColumn in (List<Column>)table.PrimaryKeyColumns)
            {
                if (num > 0)
                    stringBuilder.Append("&");
                if (language == Language.CSharp)
                    stringBuilder.Append(primaryKeyColumn.NameCamelStyle + "=@item." + primaryKeyColumn.Name);
                ++num;
            }
            return stringBuilder.ToString();
        }

        internal static string GetForeignKeyVariableWithConvert(Table fkTable, Column currentColumn)
        {
            if (fkTable.PrimaryKeyCount != 1)
                return string.Empty;
            if (!currentColumn.IsNullable || currentColumn.SystemTypeNative.ToLower() == "string")
                return currentColumn.Name;
            if (currentColumn.SystemTypeNative.ToLower() == "guid")
                return "new Guid(" + currentColumn.Name + ")";
            return "Convert.To" + currentColumn.SystemTypeNative + "(" + currentColumn.Name + ")";
        }

        internal static string RemoveLastComma(string stringWithCommaInTheEnd)
        {
            string str = stringWithCommaInTheEnd;
            if (!string.IsNullOrEmpty(stringWithCommaInTheEnd))
            {
                stringWithCommaInTheEnd = stringWithCommaInTheEnd.TrimEnd((char[])Array.Empty<char>());
                if (stringWithCommaInTheEnd.Substring(stringWithCommaInTheEnd.Length - 1, 1) == ",")
                    str = stringWithCommaInTheEnd.TrimEnd((char[])Array.Empty<char>()).Remove(stringWithCommaInTheEnd.Length - 1, 1);
            }
            return str;
        }

        internal static string RemoveTheVeryLastComma(string stringWithComma)
        {
            string str = stringWithComma;
            if (!string.IsNullOrEmpty(stringWithComma) && stringWithComma.Contains(","))
            {
                stringWithComma = stringWithComma.TrimEnd((char[])Array.Empty<char>());
                int startIndex = stringWithComma.LastIndexOf(",");
                str = stringWithComma.TrimEnd((char[])Array.Empty<char>()).Remove(startIndex, 1);
            }
            return str;
        }

        internal static string RemoveLastAnd(string stringWithAndInTheEnd)
        {
            string str = stringWithAndInTheEnd;
            if (!string.IsNullOrEmpty(stringWithAndInTheEnd) && stringWithAndInTheEnd.Contains("AND "))
            {
                int startIndex = stringWithAndInTheEnd.LastIndexOf("AND ");
                str = stringWithAndInTheEnd.Remove(startIndex, 4);
            }
            return str;
        }

        internal static string RemoveLastOR(string stringWithOrInTheEnd)
        {
            string str = stringWithOrInTheEnd;
            if (!string.IsNullOrEmpty(stringWithOrInTheEnd))
            {
                stringWithOrInTheEnd = stringWithOrInTheEnd.TrimEnd((char[])Array.Empty<char>());
                if (stringWithOrInTheEnd.Substring(stringWithOrInTheEnd.Length - 2, 2) == "||")
                    str = stringWithOrInTheEnd.TrimEnd((char[])Array.Empty<char>()).Remove(stringWithOrInTheEnd.Length - 2, 2);
            }
            return str.Trim();
        }

        internal static string GetNameWithSpaces(string columnName)
        {
            string str1 = "";
            if (columnName.IndexOf("_", 0, StringComparison.OrdinalIgnoreCase) > 0)
            {
                columnName = columnName.Replace("_", " ").ToLower(CultureInfo.CurrentCulture);
                char ch = Convert.ToChar(" ");
                string str2 = columnName;
                char[] chArray = new char[1] { ch };
                foreach (string str3 in str2.Split(chArray))
                {
                    if (!string.IsNullOrEmpty(str3))
                        str1 = str1 + str3.Substring(0, 1).ToUpper() + str3.Substring(1, str3.Length - 1) + " ";
                }
            }
            else if (columnName.IndexOf(" ") > 0)
            {
                str1 = columnName;
            }
            else
            {
                int num1 = 0;
                int num2 = 1;
                str1 = columnName.Substring(0, 1);
                foreach (char c in columnName)
                {
                    if (num1 > 0)
                    {
                        ++num2;
                        if (char.IsUpper(c) && num2 != 2)
                        {
                            str1 += " ";
                            num2 = 1;
                        }
                        str1 += c.ToString();
                    }
                    ++num1;
                }
            }
            return str1.Trim();
        }

        internal static bool IsLetterAVowel(string letter)
        {
            letter = letter.ToLower(CultureInfo.CurrentCulture);
            return !(letter != "a") || !(letter != "e") || (!(letter != "i") || !(letter != "o")) || !(letter != "u");
        }

        internal static string GetSingularForm(string tableName, out string singularNameWithUnderScore)
        {
            string str1 = tableName;
            singularNameWithUnderScore = tableName;
            if (!string.IsNullOrEmpty(tableName))
            {
                if (tableName.Length >= 4)
                {
                    string lower1 = tableName.Substring(tableName.Length - 1, 1).ToLower(CultureInfo.CurrentCulture);
                    string lower2 = tableName.Substring(tableName.Length - 2, 2).ToLower(CultureInfo.CurrentCulture);
                    string lower3 = tableName.Substring(tableName.Length - 3, 3).ToLower(CultureInfo.CurrentCulture);
                    string lower4 = tableName.Substring(tableName.Length - 4, 4).ToLower(CultureInfo.CurrentCulture);
                    string str2 = "ss";
                    if (!(lower2 == str2))
                    {
                        if (lower3 == "ies")
                        {
                            str1 = tableName.Substring(0, tableName.Length - 3) + "y";
                            singularNameWithUnderScore = tableName.Substring(0, tableName.Length - 3).Replace(" ", "_") + "y";
                        }
                        else if (lower4 == "ches")
                        {
                            str1 = tableName.Substring(0, tableName.Length - 2);
                            singularNameWithUnderScore = tableName.Substring(0, tableName.Length - 2).Replace(" ", "_");
                        }
                        else if (lower1 == "s")
                        {
                            str1 = tableName.Substring(0, tableName.Length - 1);
                            singularNameWithUnderScore = tableName.Substring(0, tableName.Length - 1).Replace(" ", "_");
                        }
                    }
                }
                else if (tableName.Length >= 3)
                {
                    string lower1 = tableName.Substring(tableName.Length - 1, 1).ToLower(CultureInfo.CurrentCulture);
                    string lower2 = tableName.Substring(tableName.Length - 2, 2).ToLower(CultureInfo.CurrentCulture);
                    string lower3 = tableName.Substring(tableName.Length - 3, 3).ToLower(CultureInfo.CurrentCulture);
                    string str2 = "ss";
                    if (!(lower2 == str2))
                    {
                        if (lower3 == "ies")
                        {
                            str1 = tableName.Substring(0, tableName.Length - 3) + "y";
                            singularNameWithUnderScore = tableName.Substring(0, tableName.Length - 3).Replace(" ", "_") + "y";
                        }
                        else if (lower1 == "s")
                        {
                            str1 = tableName.Substring(0, tableName.Length - 1);
                            singularNameWithUnderScore = tableName.Substring(0, tableName.Length - 1).Replace(" ", "_");
                        }
                    }
                }
                else if (tableName.Length >= 2)
                {
                    string lower = tableName.Substring(tableName.Length - 1, 1).ToLower(CultureInfo.CurrentCulture);
                    if (!(tableName.Substring(tableName.Length - 2, 2).ToLower(CultureInfo.CurrentCulture) == "ss") && lower == "s")
                    {
                        str1 = tableName.Substring(0, tableName.Length - 1);
                        singularNameWithUnderScore = tableName.Substring(0, tableName.Length - 1).Replace(" ", "_");
                    }
                }
            }
            return str1.Replace(" ", "");
        }

        internal static string GetPluralForm(string tableName)
        {
            string str1 = tableName;
            if (tableName.Length >= 3)
            {
                string lower1 = tableName.Substring(tableName.Length - 1, 1).ToLower(CultureInfo.CurrentCulture);
                string lower2 = tableName.Substring(tableName.Length - 2, 2).ToLower(CultureInfo.CurrentCulture);
                string lower3 = tableName.Substring(tableName.Length - 3, 3).ToLower(CultureInfo.CurrentCulture);
                string lower4 = tableName.Substring(tableName.Length - 2, 1).ToLower(CultureInfo.CurrentCulture);
                string str2 = "ies";
                if (!(lower3 == str2) && !(lower2 == "es"))
                {
                    if (lower2 == "ss")
                        str1 = tableName + "es";
                    else if (!(lower1 == "s"))
                        str1 = !(lower1 == "y") || !Functions.IsLetterAVowel(lower4) ? (lower2 == "ao" || lower2 == "eo" || (lower2 == "io" || lower2 == "oo") || lower2 == "uo" ? tableName + "s" : (!(lower1 == "y") ? (lower1 == "s" || lower1 == "x" || (lower2 == "ch" || lower2 == "sh") ? tableName + "es" : tableName + "s") : tableName.Substring(0, tableName.Length - 1) + "ies")) : tableName + "s";
                }
            }
            else if (tableName.Length >= 2)
            {
                string lower1 = tableName.Substring(tableName.Length - 1, 1).ToLower(CultureInfo.CurrentCulture);
                string lower2 = tableName.Substring(tableName.Length - 2, 2).ToLower(CultureInfo.CurrentCulture);
                string lower3 = tableName.Substring(tableName.Length - 2, 1).ToLower(CultureInfo.CurrentCulture);
                if (!(lower2 == "es"))
                {
                    if (lower2 == "ss")
                        str1 = tableName + "es";
                    else if (!(lower1 == "s"))
                        str1 = !(lower1 == "y") || !Functions.IsLetterAVowel(lower3) ? (lower2 == "ao" || lower2 == "eo" || (lower2 == "io" || lower2 == "oo") || lower2 == "uo" ? tableName + "s" : (!(lower1 == "y") ? (lower1 == "s" || lower1 == "x" || (lower2 == "ch" || lower2 == "sh") ? tableName + "es" : tableName + "s") : tableName.Substring(0, tableName.Length - 1) + "ies")) : tableName + "s";
                }
            }
            else if (tableName.Length >= 1)
            {
                string lower = tableName.Substring(tableName.Length - 1, 1).ToLower(CultureInfo.CurrentCulture);
                if (!(lower == "s"))
                    str1 = !(lower == "x") ? tableName + "s" : tableName + "es";
            }
            return str1;
        }

        internal static void WriteToErrorLog(string errorMessage, string path)
        {
            string path1 = path + "\\ErrorLog.txt";
            if (System.IO.File.Exists(path1))
                return;
            try
            {
                using (System.IO.File.Create(path1))
#pragma warning disable CS0642 // Possible mistaken empty statement
                    ;
#pragma warning restore CS0642 // Possible mistaken empty statement
                StreamWriter streamWriter = System.IO.File.AppendText(path1);
                streamWriter.WriteLine(errorMessage);
                streamWriter.WriteLine("");
                streamWriter.WriteLine("");
                streamWriter.Close();
            }
            catch
            {
            }
        }

        internal static void DeleteErrorLog(string path)
        {
            string path1 = path + "\\ErrorLog.txt";
            if (!System.IO.File.Exists(path1))
                return;
            try
            {
                System.IO.File.Delete(path1);
            }
            catch
            {
            }
        }

        internal static string ConvertToPascal(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            if (str.Contains(" ") && str.Contains("_"))
                return str.Trim();
            if (!str.Contains(" ") && !str.Contains("_"))
            {
                if (str.Length > 1)
                {
                    int num1 = 0;
                    int num2 = 0;
                    for (int startIndex = 0; startIndex < str.Length; ++startIndex)
                    {
                        if (char.IsLetter(Convert.ToChar(str.Substring(startIndex, 1))))
                            ++num1;
                        if (char.IsUpper(Convert.ToChar(str.Substring(startIndex, 1))))
                            ++num2;
                    }
                    if (num1 == num2)
                        str = str.ToLower();
                    str = str.Substring(0, 1).ToUpper() + str.Substring(1, str.Length - 1);
                }
                return str;
            }
            str = str.Trim().ToLower();
            string[] strArray;
            if (str.Contains(" "))
                strArray = str.Split(' ');
            else
                strArray = str.Split('_');
            for (int index = 0; index < strArray.Length; ++index)
            {
                if (strArray[index].Length > 0)
                {
                    string str1 = strArray[index];
                    char upper = char.ToUpper(str1[0]);
                    strArray[index] = upper.ToString() + str1.Substring(1);
                }
            }
            return string.Join(string.Empty, strArray);
        }

        internal static string ConvertToCamel(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            if (str.Contains(" ") && str.Contains("_"))
                return str.Trim();
            if (!str.Contains(" ") && !str.Contains("_"))
            {
                if (str.Length > 1)
                {
                    int num1 = 0;
                    int num2 = 0;
                    for (int startIndex = 0; startIndex < str.Length; ++startIndex)
                    {
                        if (char.IsLetter(Convert.ToChar(str.Substring(startIndex, 1))))
                            ++num1;
                        if (char.IsUpper(Convert.ToChar(str.Substring(startIndex, 1))))
                            ++num2;
                    }
                    str = num1 != num2 ? str.Substring(0, 1).ToLower() + str.Substring(1, str.Length - 1) : str.ToLower();
                }
                return str;
            }
            str = str.Trim().ToLower();
            string[] strArray;
            if (str.Contains(" "))
                strArray = str.Split(' ');
            else
                strArray = str.Split('_');
            for (int index = 1; index < strArray.Length; ++index)
            {
                if (strArray[index].Length > 0)
                {
                    string str1 = strArray[index];
                    char upper = char.ToUpper(str1[0]);
                    strArray[index] = upper.ToString() + str1.Substring(1);
                }
            }
            return string.Join(string.Empty, strArray);
        }

        internal static string ReplaceNoneAlphaNumericWithUnderscore(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            str = str.Trim();
            StringBuilder stringBuilder = new StringBuilder();
            for (int startIndex = 0; startIndex < str.Length; ++startIndex)
            {
                char c = Convert.ToChar(str.Substring(startIndex, 1));
                if (!char.IsLetter(c) && !char.IsNumber(c) && c.ToString() != "_")
                    stringBuilder.Append("_");
                else
                    stringBuilder.Append(c);
            }
            return stringBuilder.ToString();
        }

        internal static void WriteRelatedTableStringBuilder(Table table, StringBuilder sb, RelatedTablePart part, Tables selectedTables, DataTable referencedTables, Language language)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int num = 0;
            foreach (Column primaryKeyColumn in (List<Column>)table.PrimaryKeyColumns)
            {
                if (referencedTables != null)
                {
                    foreach (DataRow row in (InternalDataCollectionBase)referencedTables.Rows)
                    {
                        DataRow dr = row;
                        if (table.OwnerOriginal == dr[4].ToString() && table.NameOriginal == dr[5].ToString() && primaryKeyColumn.NameOriginal == dr[5].ToString())
                        {
                            Table table1 = selectedTables.Find((Predicate<Table>)(t => t.OwnerOriginal + "." + t.NameOriginal == dr["table_owner"].ToString() + "." + dr["table_name"].ToString()));
                            if (table1 != null)
                            {
                                string name1 = table1.Name;
                                string str1 = table1.Name + "Collection";
                                string variableObjName = table1.VariableObjName;
                                string objCollectionName = table1.VariableObjCollectionName;
                                string name2 = table.Name;
                                string empty = string.Empty;
                                if (stringBuilder.ToString().Contains(name1 + ","))
                                {
                                    ++num;
                                    name1 += num.ToString();
                                    str1 += num.ToString();
                                    variableObjName += num.ToString();
                                    string str2 = objCollectionName + num.ToString();
                                    string str3 = name2 + num.ToString();
                                }
                                switch (part)
                                {
                                    case RelatedTablePart.BusinessObjectBaseWritePrivateMembers:
                                        if (language == Language.VB)
                                        {
                                            sb.Append("         Private _" + Functions.ConvertToCamel(str1) + " As Lazy(Of " + table1.Name + "Collection) \r\n");
                                            break;
                                        }
                                        break;
                                    case RelatedTablePart.CodeExampleSelects:
                                        if (language == Language.CSharp)
                                        {
                                            sb.Append(" \r\n");
                                            sb.Append("            // you can access all the " + name1 + "(s) related to the " + primaryKeyColumn.Name + " \r\n");
                                            sb.Append("            // since this uses the lazy loader pattern, the \r\n");
                                            sb.Append("            // " + table.VariableObjName + "." + str1 + " property is null until you access the .Value extension method \r\n");
                                            sb.Append("            if (" + table.VariableObjName + "." + str1 + ".Value != null) \r\n");
                                            sb.Append("            { \r\n");
                                            sb.Append("                if (" + table.VariableObjName + "." + str1 + ".IsValueCreated) \r\n");
                                            sb.Append("                { \r\n");
                                            sb.Append("                    foreach (" + table1.Name + " " + variableObjName + " in " + table.VariableObjName + "." + str1 + ".Value) \r\n");
                                            sb.Append("                    { \r\n");
                                            foreach (Column column in (List<Column>)table1.Columns)
                                                sb.Append("                        " + column.SystemType + column.NullableChar + " " + column.NameCamelStyle + "2 = " + variableObjName + "." + column.Name + "; \r\n");
                                            sb.Append("                    } \r\n");
                                            sb.Append("                } \r\n");
                                            sb.Append("            } \r\n");
                                            sb.Append(" \r\n");
                                            break;
                                        }
                                        sb.Append(" \r\n");
                                        sb.Append("            ' you can access all the " + name1 + "(s) related to the " + primaryKeyColumn.Name + " \r\n");
                                        sb.Append("            ' since this uses the lazy loader pattern, the \r\n");
                                        sb.Append("            ' " + table.VariableObjName + "." + str1 + " property is null until you access the .Value extension method \r\n");
                                        sb.Append("            If " + table.VariableObjName + "." + str1 + ".Value IsNot Nothing Then \r\n");
                                        sb.Append("                If " + table.VariableObjName + "." + str1 + ".IsValueCreated Then \r\n");
                                        sb.Append("                    For Each " + variableObjName + " As " + table1.Name + " In " + table.VariableObjName + "." + str1 + ".Value \r\n");
                                        foreach (Column column in (List<Column>)table1.Columns)
                                            sb.Append("                        Dim " + column.NameCamelStyle + "2 As " + column.SystemType + " = " + variableObjName + "." + column.Name + " \r\n");
                                        sb.Append("                    Next \r\n");
                                        sb.Append("                End If \r\n");
                                        sb.Append("            End If \r\n");
                                        sb.Append(" \r\n");
                                        break;
                                }
                                stringBuilder.Append(table1.Name + ",");
                                break;
                            }
                        }
                    }
                }
            }
        }

        internal static string GetCommaDelimitedPrimaryKeyParametersWithSystemType(Table table, Language language)
        {
            int num = 0;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Column primaryKeyColumn in (List<Column>)table.PrimaryKeyColumns)
            {
                if (num > 0)
                    stringBuilder.Append(", ");
                if (language == Language.CSharp)
                    stringBuilder.Append(primaryKeyColumn.SystemType + " " + primaryKeyColumn.NameCamelStyle);
                else
                    stringBuilder.Append(primaryKeyColumn.NameCamelStyle + " As " + primaryKeyColumn.SystemType);
                ++num;
            }
            return stringBuilder.ToString();
        }

        internal static string GetCommaDelimitedPrimaryKeyParameters(Table table, Language language, bool isForJavaScript, GeneratedSqlType generatedSqlType, bool isForSampleData = false)
        {
            int num = 0;
            string str = string.Empty;
            StringBuilder stringBuilder = new StringBuilder();
            if (isForSampleData)
                str = "Sample";
            foreach (Column primaryKeyColumn in (List<Column>)table.PrimaryKeyColumns)
            {
                if (num > 0)
                    stringBuilder.Append(", ");
                if (generatedSqlType == GeneratedSqlType.EFCore && primaryKeyColumn.SQLDataType == SQLType.uniqueidentifier && !isForJavaScript)
                    stringBuilder.Append(primaryKeyColumn.NameCamelStyle + str + ".ToString()");
                else
                    stringBuilder.Append(primaryKeyColumn.NameCamelStyle + str);
                ++num;
            }
            return stringBuilder.ToString();
        }

        internal static string GetCommaDelimitedAllColumnParameters(Table table, Language language)
        {
            int num = 0;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Column column in (List<Column>)table.Columns)
            {
                if (num > 0)
                    stringBuilder.Append(", ");
                stringBuilder.Append(column.NameCamelStyle);
                ++num;
            }
            return stringBuilder.ToString();
        }

        internal static string GetCommaDelimitedInsertOrUdateMethodParamsWithSystemType(Table table, string language)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (!(language == "c#"))
                return (string)null;
            foreach (Column column in (List<Column>)table.Columns)
            {
                string keywordVariableName = Functions.GetNoneKeywordVariableName(column.NameCamelStyle, Language.CSharp);
                if (!column.IsPrimaryKey || !column.IsPrimaryKeyUnique || column.IsComputed)
                    stringBuilder.Append(column.SystemType + column.NullableChar + " " + keywordVariableName + ", ");
            }
            stringBuilder.Replace(",", "", stringBuilder.ToString().LastIndexOf(","), 1);
            return stringBuilder.ToString().Trim();
        }

        internal static string GetInsertMethodReturnType(Table table, Language language = Language.CSharp)
        {
            if (table.PrimaryKeyCount <= 1)
                return table.FirstPrimaryKeySystemType;
            if (language == Language.CSharp)
                return "void";
            return string.Empty;
        }

        internal static string GetNoneKeywordVariableName(string variable, Language language)
        {
            if (KeywordsFactory.GetKeywords(language).IsThisVariableAKeyword(variable))
                variable += "1";
            return variable;
        }

        internal static string GetStoredProcName(Table table, StoredProcName sprocName, int spPrefixSuffixIndex, string storedProcPrefix, string storedProcSuffix)
        {
            switch (spPrefixSuffixIndex)
            {
                case 1:
                    switch (sprocName)
                    {
                        case StoredProcName.SelectByPrimaryKey:
                            return "[" + table.OwnerOriginal + "].[" + table.Name + "_SelectByPrimaryKey]";
                        case StoredProcName.SelectAll:
                            return "[" + table.OwnerOriginal + "].[" + table.Name + "_SelectAll]";
                        case StoredProcName.SelectSkipAndTake:
                            return "[" + table.OwnerOriginal + "].[" + table.Name + "_SelectSkipAndTake]";
                        case StoredProcName.SelectSkipAndTakeWhereDynamic:
                            return "[" + table.OwnerOriginal + "].[" + table.Name + "_SelectSkipAndTakeWhereDynamic]";
                        case StoredProcName.SelectTotals:
                            return "[" + table.OwnerOriginal + "].[" + table.Name + "_SelectTotals]";
                        case StoredProcName.SelectAllWhereDynamic:
                            return "[" + table.OwnerOriginal + "].[" + table.Name + "_SelectAllWhereDynamic]";
                        case StoredProcName.GetRecordCount:
                            return "[" + table.OwnerOriginal + "].[" + table.Name + "_GetRecordCount]";
                        case StoredProcName.GetRecordCountWhereDynamic:
                            return "[" + table.OwnerOriginal + "].[" + table.Name + "_GetRecordCountWhereDynamic]";
                        case StoredProcName.Insert:
                            return "[" + table.OwnerOriginal + "].[" + table.Name + "_Insert]";
                        case StoredProcName.Update:
                            return "[" + table.OwnerOriginal + "].[" + table.Name + "_Update]";
                        case StoredProcName.Delete:
                            return "[" + table.OwnerOriginal + "].[" + table.Name + "_Delete]";
                        case StoredProcName.SelectDropDownListData:
                            return "[" + table.OwnerOriginal + "].[" + table.Name + "_SelectDropDownListData]";
                        default:
                            return string.Empty;
                    }
                case 2:
                    switch (sprocName)
                    {
                        case StoredProcName.SelectByPrimaryKey:
                            return "[" + table.OwnerOriginal + "].[" + storedProcPrefix + table.Name + "_SelectByPrimaryKey]";
                        case StoredProcName.SelectAll:
                            return "[" + table.OwnerOriginal + "].[" + storedProcPrefix + table.Name + "_SelectAll]";
                        case StoredProcName.SelectSkipAndTake:
                            return "[" + table.OwnerOriginal + "].[" + storedProcPrefix + table.Name + "_SelectSkipAndTake]";
                        case StoredProcName.SelectSkipAndTakeWhereDynamic:
                            return "[" + table.OwnerOriginal + "].[" + storedProcPrefix + table.Name + "_SelectSkipAndTakeWhereDynamic]";
                        case StoredProcName.SelectTotals:
                            return "[" + table.OwnerOriginal + "].[" + storedProcPrefix + table.Name + "_SelectTotals]";
                        case StoredProcName.SelectAllWhereDynamic:
                            return "[" + table.OwnerOriginal + "].[" + storedProcPrefix + table.Name + "_SelectAllWhereDynamic]";
                        case StoredProcName.GetRecordCount:
                            return "[" + table.OwnerOriginal + "].[" + storedProcPrefix + table.Name + "_GetRecordCount]";
                        case StoredProcName.GetRecordCountWhereDynamic:
                            return "[" + table.OwnerOriginal + "].[" + storedProcPrefix + table.Name + "_GetRecordCountWhereDynamic]";
                        case StoredProcName.Insert:
                            return "[" + table.OwnerOriginal + "].[" + storedProcPrefix + table.Name + "_Insert]";
                        case StoredProcName.Update:
                            return "[" + table.OwnerOriginal + "].[" + storedProcPrefix + table.Name + "_Update]";
                        case StoredProcName.Delete:
                            return "[" + table.OwnerOriginal + "].[" + storedProcPrefix + table.Name + "_Delete]";
                        case StoredProcName.SelectDropDownListData:
                            return "[" + table.OwnerOriginal + "].[" + storedProcPrefix + table.Name + "_SelectDropDownListData]";
                        default:
                            return string.Empty;
                    }
                default:
                    switch (sprocName)
                    {
                        case StoredProcName.SelectByPrimaryKey:
                            return "[" + table.OwnerOriginal + "].[" + table.Name + "_SelectByPrimaryKey" + storedProcSuffix + "]";
                        case StoredProcName.SelectAll:
                            return "[" + table.OwnerOriginal + "].[" + table.Name + "_SelectAll" + storedProcSuffix + "]";
                        case StoredProcName.SelectSkipAndTake:
                            return "[" + table.OwnerOriginal + "].[" + table.Name + "_SelectSkipAndTake" + storedProcSuffix + "]";
                        case StoredProcName.SelectSkipAndTakeWhereDynamic:
                            return "[" + table.OwnerOriginal + "].[" + table.Name + "_SelectSkipAndTakeWhereDynamic" + storedProcSuffix + "]";
                        case StoredProcName.SelectTotals:
                            return "[" + table.OwnerOriginal + "].[" + table.Name + "_SelectTotals" + storedProcSuffix + "]";
                        case StoredProcName.SelectAllWhereDynamic:
                            return "[" + table.OwnerOriginal + "].[" + table.Name + "_SelectAllWhereDynamic" + storedProcSuffix + "]";
                        case StoredProcName.GetRecordCount:
                            return "[" + table.OwnerOriginal + "].[" + table.Name + "_GetRecordCount" + storedProcSuffix + "]";
                        case StoredProcName.GetRecordCountWhereDynamic:
                            return "[" + table.OwnerOriginal + "].[" + table.Name + "_GetRecordCountWhereDynamic" + storedProcSuffix + "]";
                        case StoredProcName.Insert:
                            return "[" + table.OwnerOriginal + "].[" + table.Name + "_Insert" + storedProcSuffix + "]";
                        case StoredProcName.Update:
                            return "[" + table.OwnerOriginal + "].[" + table.Name + "_Update" + storedProcSuffix + "]";
                        case StoredProcName.Delete:
                            return "[" + table.OwnerOriginal + "].[" + table.Name + "_Delete" + storedProcSuffix + "]";
                        case StoredProcName.SelectDropDownListData:
                            return "[" + table.OwnerOriginal + "].[" + table.Name + "_SelectDropDownListData" + storedProcSuffix + "]";
                        default:
                            return string.Empty;
                    }
            }
        }

        internal static string GetStoredProcNameBy(Table table, Column column, int spPrefixSuffixIndex, string storedProcPrefix, string storedProcSuffix, StoredProcName sprocName)
        {
            string str = string.Empty;
            switch (sprocName)
            {
                case StoredProcName.SelectAllBy:
                    str = "_SelectAllBy";
                    break;
                case StoredProcName.SelectSkipAndTakeBy:
                    str = "_SelectSkipAndTakeBy";
                    break;
                case StoredProcName.GetRecordCountBy:
                    str = "_GetRecordCountBy";
                    break;
            }
            if (spPrefixSuffixIndex == 1)
                return "[" + table.OwnerOriginal + "].[" + table.Name + str + column.Name + "]";
            if (spPrefixSuffixIndex == 2)
                return "[" + table.OwnerOriginal + "].[" + storedProcPrefix + table.Name + str + column.Name + "]";
            return "[" + table.OwnerOriginal + "].[" + table.Name + str + column.Name + storedProcSuffix + "]";
        }

        internal static void CopyFolder(string sourceFolder, string destinationFolder)
        {
            try
            {
                if (!Directory.Exists(sourceFolder))
                    return;
                if (!Directory.Exists(destinationFolder))
                    Directory.CreateDirectory(destinationFolder);
                foreach (string file in Directory.GetFiles(sourceFolder))
                {
                    string fileName = Path.GetFileName(file);
                    System.IO.File.Copy(file, Path.Combine(destinationFolder, fileName), true);
                }
                foreach (string directory in Directory.GetDirectories(sourceFolder))
                    Functions.CopyFolderFromSourceToDestination(directory, destinationFolder);
            }
            catch
            {
            }
        }

        private static void CopyFolderFromSourceToDestination(string sourceFolder, string destinationFolder)
        {
            string fileName = Path.GetFileName(sourceFolder);
            string destinationFolder1 = Path.Combine(destinationFolder, fileName);
            Functions.CopyFolder(sourceFolder, destinationFolder1);
        }

        internal static Table GetTableFromSelectedTables(string originalTableOwner, string originalTableName, Tables selectedTables)
        {
            return selectedTables.Find((Predicate<Table>)(t => t.OwnerOriginal.ToLower() + "." + t.NameOriginal.ToLower() == originalTableOwner.ToLower() + "." + originalTableName.ToLower()));
        }

        internal static string GetWebControlFieldAssignment(Table table, Column column, Language language = Language.CSharp)
        {
            string str1 = ";";
            if (language == Language.VB)
                str1 = string.Empty;
            string str2;
            if (column.SystemTypeNative.ToLower() == "string")
                str2 = table.VariableObjName + "." + column.Name + " = Txt" + column.Name + ".Text" + str1;
            else if (column.SQLDataType == SQLType.bit)
                str2 = table.VariableObjName + "." + column.Name + " = Cbx" + column.Name + ".Checked" + str1;
            else if (column.SystemTypeNative.ToLower() == "datetimeoffset")
                str2 = table.VariableObjName + "." + column.Name + " = DateTimeOffset.Parse(Txt" + column.Name + ".Text)" + str1;
            else if (column.SystemTypeNative.ToLower() == "timespan")
                str2 = table.VariableObjName + "." + column.Name + " = TimeSpan.Parse(Txt" + column.Name + ".Text)" + str1;
            else if (column.SQLDataType == SQLType.uniqueidentifier)
                str2 = table.VariableObjName + "." + column.Name + " = new Guid(Txt" + column.Name + ".Text)" + str1;
            else if (column.SQLDataType == SQLType.timestamp)
                str2 = table.VariableObjName + "." + column.Name + " = Functions.ConvertStringToByteArray(Txt" + column.Name + ".Text)" + str1;
            else if (column.DataTypeNumber == "-10")
                str2 = table.VariableObjName + "." + column.Name + " = System.Xml.Linq.XElement.Parse(Txt" + column.Name + ".Text)" + str1;
            else
                str2 = table.VariableObjName + "." + column.Name + " = Convert.To" + column.SystemTypeNative + "(Txt" + column.Name + ".Text)" + str1;
            return str2;
        }

        internal static string GetWebFormFileNamePrefix(GridViewType gridViewType, OrganizeWebForm organizeWebForm)
        {
            switch (gridViewType)
            {
                case GridViewType.Redirect:
                    return organizeWebForm.PrefixForCheckedWebFormGridView;
                case GridViewType.AddEdit:
                    return organizeWebForm.PrefixForCheckedWebFormGridViewAddEdit;
                case GridViewType.ReadOnly:
                    return organizeWebForm.PrefixForCheckedWebFormGridViewReadOnly;
                case GridViewType.MoreInfoOnly:
                    return organizeWebForm.PrefixForCheckedWebFormGridViewMoreInfo;
                case GridViewType.Totals:
                    return organizeWebForm.PrefixForCheckedWebFormGridViewTotals;
                case GridViewType.FilterBy:
                    return organizeWebForm.PrefixForCheckedWebFormGridViewFilterBy;
                case GridViewType.Grouping:
                    return organizeWebForm.PrefixForCheckedWebFormGridViewGrouping;
                case GridViewType.Inline:
                    return organizeWebForm.PrefixForCheckedWebFormGridViewInline;
                case GridViewType.Search:
                    return organizeWebForm.PrefixForCheckedWebFormGridViewSearch;
                default:
                    return string.Empty;
            }
        }

        internal static void AppendAddEditRecordMVC(StringBuilder sb, Table table, Tables selectedTables, AppendAddEditRecordType appendAddEditRecordType, AppendAddEditRecordContentType appendAddEditRecordContentType, ApplicationVersion appVersion, MVCGridViewType viewType)
        {
            switch (appendAddEditRecordContentType)
            {
                case AppendAddEditRecordContentType.AddEditGridView:
                    sb.AppendLine("<form method=\"post\">");
                    goto case AppendAddEditRecordContentType.Details;
                case AppendAddEditRecordContentType.Details:
                    sb.AppendLine("    <div>");
                    sb.AppendLine("        <fieldset>");
                    sb.AppendLine("            <legend></legend>");
                    sb.AppendLine("            <table>");
                    if (appendAddEditRecordContentType == AppendAddEditRecordContentType.AddEditPartialView || appendAddEditRecordContentType == AppendAddEditRecordContentType.AddEditGridView)
                    {
                        if (appendAddEditRecordType == AppendAddEditRecordType.AddEdit)
                        {
                            sb.AppendLine("                @if (Model.Operation == CrudOperation.Update)");
                            sb.AppendLine("                {");
                            foreach (Column primaryKeyColumn in (List<Column>)table.PrimaryKeyColumns)
                            {
                                sb.AppendLine("                    <tr>");
                                sb.AppendLine("                        <td class=\"editor-label\"><label asp-for=\"" + table.Name + "." + primaryKeyColumn.Name + "\"></label>:</td>");
                                sb.AppendLine("                        <td></td>");
                                sb.AppendLine("                        <td class=\"editor-field\"><input asp-for=\"" + table.Name + "." + primaryKeyColumn.Name + "\" readonly=\"readonly\" /></td>");
                                sb.AppendLine("                        <td></td>");
                                sb.AppendLine("                    </tr>");
                            }
                            sb.AppendLine("                }");
                            if (table.PrimaryKeyAutoFilledCount == 0)
                            {
                                sb.AppendLine("                else");
                                sb.AppendLine("                {");
                                foreach (Column primaryKeyColumn in (List<Column>)table.PrimaryKeyColumns)
                                {
                                    sb.AppendLine("                    <tr>");
                                    sb.AppendLine("                        <td class=\"editor-label\"><label asp-for=\"" + table.Name + "." + primaryKeyColumn.Name + "\"></label>:</td>");
                                    sb.AppendLine("                        <td><span style=\"color: red;\">*</span></td>");
                                    if (primaryKeyColumn.IsForeignKey)
                                    {
                                        if (primaryKeyColumn.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK)
                                            sb.AppendLine("                        <td class=\"editor-field\"><select id=\"" + primaryKeyColumn.NameCamelStyle + "\" asp-for=\"" + table.Name + "." + primaryKeyColumn.Name + "\" asp-items=\"@(new SelectList(Model." + primaryKeyColumn.ForeignKeyTableName + MyConstants.WordDropDownListData + ", \"" + primaryKeyColumn.ForeignKeyColumnName + "\", \"" + primaryKeyColumn.ForeignKeyTable.DataTextField + "\"))\"><option value=\"\">Select One</option></select></td>");
                                        else if (viewType == MVCGridViewType.CRUD)
                                            sb.AppendLine("                        <td class=\"editor-field\"><input asp-for=\"" + table.Name + "." + primaryKeyColumn.Name + "\" onblur=\"isDataValid()\" /></td>");
                                        else
                                            sb.AppendLine("                        <td class=\"editor-field\"><input asp-for=\"" + table.Name + "." + primaryKeyColumn.Name + "\" /></td>");
                                    }
                                    else if (viewType == MVCGridViewType.CRUD)
                                    {
                                        if (primaryKeyColumn.SystemTypeNative == "DateTime")
                                            sb.AppendLine("                        <td class=\"editor-field\"><input asp-for=\"" + table.Name + "." + primaryKeyColumn.Name + "\" class=\"datetextbox\" onblur=\"isDataValid()\" /></td>");
                                        else
                                            sb.AppendLine("                        <td class=\"editor-field\"><input asp-for=\"" + table.Name + "." + primaryKeyColumn.Name + "\" onblur=\"isDataValid()\" /></td>");
                                    }
                                    else if (primaryKeyColumn.SystemTypeNative == "DateTime")
                                        sb.AppendLine("                        <td class=\"editor-field\"><input asp-for=\"" + table.Name + "." + primaryKeyColumn.Name + "\" class=\"datetextbox\" /></td>");
                                    else
                                        sb.AppendLine("                        <td class=\"editor-field\"><input asp-for=\"" + table.Name + "." + primaryKeyColumn.Name + "\" /></td>");
                                    if (primaryKeyColumn.IsPrimaryKeyUnique)
                                        sb.AppendLine("                        <td class=\"editor-field\"></td>");
                                    else if (viewType == MVCGridViewType.CRUD)
                                        sb.AppendLine("                        <td class=\"editor-field\"><span id=\"" + primaryKeyColumn.NameCamelStyle + "Validation\" style=\"color: red;\"></span></td>");
                                    else
                                        sb.AppendLine("                        <td class=\"editor-field\"><span asp-validation-for=\"" + table.Name + "." + primaryKeyColumn.Name + "\"></span></td>");
                                    sb.AppendLine("                ");
                                    sb.AppendLine("                    </tr>");
                                }
                                sb.AppendLine("                }");
                            }
                            sb.AppendLine("");
                        }
                        else
                        {
                            foreach (Column primaryKeyColumn in (List<Column>)table.PrimaryKeyColumns)
                            {
                                string str1 = "                <tr id=\"trPrimaryKey\">";
                                string str2 = "                    <td class=\"editor-field\"><input asp-for=\"" + table.Name + "." + primaryKeyColumn.Name + "\" id=\"" + primaryKeyColumn.NameCamelStyle + "\" readonly=\"readonly\" /></td>";
                                if (primaryKeyColumn.SystemTypeNative == "DateTime")
                                    str2 = "                    <td class=\"editor-field\"><input asp-for=\"" + table.Name + "." + primaryKeyColumn.Name + "\" id=\"" + primaryKeyColumn.NameCamelStyle + "\" readonly=\"readonly\" class=\"datetextbox\" /></td>";
                                if (primaryKeyColumn.IsForeignKey)
                                {
                                    if (primaryKeyColumn.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK)
                                    {
                                        str1 = "                <tr>";
                                        str2 = "                    <td class=\"editor-field\"><select id=\"" + primaryKeyColumn.NameCamelStyle + "\" asp-for=\"" + table.Name + "." + primaryKeyColumn.Name + "\" asp-items=\"@(new SelectList(Model." + primaryKeyColumn.ForeignKeyTableName + MyConstants.WordDropDownListData + ", \"" + primaryKeyColumn.ForeignKeyColumnName + "\", \"" + primaryKeyColumn.ForeignKeyTable.DataTextField + "\"))\"><option value=\"\">Select One</option></select></td>";
                                    }
                                }
                                else if (!primaryKeyColumn.IsPrimaryKeyUnique)
                                    str1 = "                <tr>";
                                sb.AppendLine(str1);
                                sb.AppendLine("                    <td class=\"editor-label\"><label asp-for=\"" + table.Name + "." + primaryKeyColumn.Name + "\"></label>:</td>");
                                sb.AppendLine("                    <td><span style=\"color: red;\">*</span></td>");
                                sb.AppendLine(str2);
                                if (primaryKeyColumn.IsPrimaryKeyUnique)
                                    sb.AppendLine("                    <td class=\"editor-field\"></td>");
                                else if (viewType == MVCGridViewType.CRUD)
                                    sb.AppendLine("                    <td class=\"editor-field\"><span id=\"" + primaryKeyColumn.NameCamelStyle + "Validation\" style=\"color: red;\"></span></td>");
                                else
                                    sb.AppendLine("                    <td class=\"editor-field\"><span asp-validation-for=\"" + table.Name + "." + primaryKeyColumn.Name + "\"></span></td>");
                                sb.AppendLine("                </tr>");
                            }
                        }
                    }
                    else if (appendAddEditRecordContentType == AppendAddEditRecordContentType.Details)
                    {
                        foreach (Column primaryKeyColumn in (List<Column>)table.PrimaryKeyColumns)
                        {
                            sb.AppendLine("                <tr>");
                            sb.AppendLine("                    <td class=\"editor-label-bold\"><label asp-for=\"" + table.Name + "." + primaryKeyColumn.Name + "\"></label>:</td>");
                            sb.AppendLine("                    <td class=\"editor-label\">@Model." + table.Name + "." + primaryKeyColumn.Name + "</td>");
                            sb.AppendLine("                </tr>");
                        }
                    }
                    else if (appendAddEditRecordContentType == AppendAddEditRecordContentType.Unbound && table.PrimaryKeyAutoFilledCount == 0)
                    {
                        foreach (Column primaryKeyColumn in (List<Column>)table.PrimaryKeyColumns)
                        {
                            sb.AppendLine("                <tr>");
                            sb.AppendLine("                    <td class=\"editor-label\"><label asp-for=\"" + table.Name + "." + primaryKeyColumn.Name + "\"></label>:</td>");
                            sb.AppendLine("                    <td><span style=\"color: red;\">*</span></td>");
                            if (primaryKeyColumn.IsForeignKey)
                                sb.AppendLine("                    <td class=\"editor-field\"><select asp-for=\"" + table.Name + "." + primaryKeyColumn.Name + "\"><option value=\"\">Select One</option></select></td>");
                            else
                                sb.AppendLine("                    <td class=\"editor-field\"><input asp-for=\"" + table.Name + "." + primaryKeyColumn.Name + "\" id=\"" + primaryKeyColumn.NameCamelStyle + "\" /></td>");
                            if (primaryKeyColumn.IsPrimaryKeyUnique)
                                sb.AppendLine("                    <td class=\"editor-field\"></td>");
                            else
                                sb.AppendLine("                    <td class=\"editor-field\"><span asp-validation-for=\"" + table.Name + "." + primaryKeyColumn.Name + "\"></span></td>");
                            sb.AppendLine("                </tr>");
                        }
                    }
                    //else if (appendAddEditRecordContentType == AppendAddEditRecordContentType.AssignWorkflowSteps)
                    //{
                    //    foreach (Column keyColumn in table.Columns)
                    //    {

                    //        if (keyColumn.Name == "WorkflowId")
                    //        {
                    //            sb.AppendLine("                <tr>");
                    //            sb.AppendLine("                   <td class=\"editor-field\"><select id=\"" + keyColumn.NameCamelStyle + "\" asp-for=\"" + table.Name + "." + keyColumn.Name + "\" asp-items=\"@(new SelectList(Model.WorkflowMasterDetails" + MyConstants.WordDropDownListData + ", \"" + keyColumn.ForeignKeyColumnName + "\", \"" + keyColumn.ForeignKeyTable.DataTextField + "\"))\"><option value=\"\">Select One</option></select></td>");
                    //            sb.AppendLine("                </tr>");
                    //        }
                    //        if(keyColumn.Name=="StepTitle")
                    //        {
                    //            sb.AppendLine("                <tr>");
                    //            sb.AppendLine("                   <td class=\"display-field\"> asp-for=\"@(Model."+keyColumn.Name+"</td>");
                    //            sb.AppendLine("                </tr>");
                    //        }

                    //        if (keyColumn.Name=="UserId")
                    //        {
                    //            sb.AppendLine("                <tr>");
                    //            sb.AppendLine("                   <td class=\"display-field\"> asp-for=\"@(Model." + keyColumn.Name + "</td>");
                    //            sb.AppendLine("                </tr>");
                    //        }
                    //    }

                  //  }
                    foreach (Column column in (List<Column>)table.Columns)
                    {
                        if (appendAddEditRecordContentType != AppendAddEditRecordContentType.AssignWorkflowSteps)
                        {
                            if (!column.IsPrimaryKey && !column.IsComputed)
                            {
                                string str1 = "";
                                if (appendAddEditRecordType == AppendAddEditRecordType.GridView)
                                    str1 = " id=\"" + column.NameCamelStyle + "\"";
                                sb.AppendLine("                <tr>");
                                if (appendAddEditRecordContentType == AppendAddEditRecordContentType.Details)
                                    sb.AppendLine("                    <td class=\"editor-label-bold\"><label asp-for=\"" + table.Name + "." + column.Name + "\"></label>:</td>");
                                else if (column.IsNullable && !column.IsPrimaryKeyUnique && !column.IsForeignKey && (column.SQLDataType == SQLType.integer || column.SQLDataType == SQLType.bigint || (column.SQLDataType == SQLType.smallint || column.SQLDataType == SQLType.tinyint)))
                                    sb.AppendLine("                    <td class=\"editor-label\"><label asp-for=\"" + table.Name + "." + column.Name + "Hidden\"></label>:</td>");
                                else
                                    sb.AppendLine("                    <td class=\"editor-label\"><label asp-for=\"" + table.Name + "." + column.Name + "\"></label>:</td>");
                                if (appendAddEditRecordContentType != AppendAddEditRecordContentType.Details)
                                {
                                    if (!column.IsNullable && !column.IsDateWithGetDate && (!column.IsIdentity && !column.IsUniqueIdWithNewId))
                                        sb.AppendLine("                    <td><span style=\"color: red;\">*</span></td>");
                                    else
                                        sb.AppendLine("                    <td></td>");
                                }
                                if (!column.IsForeignKey)
                                {
                                    if (appendAddEditRecordContentType == AppendAddEditRecordContentType.Details)
                                        sb.AppendLine("                    <td class=\"editor-label\">@Model." + table.Name + "." + column.Name + "</td>");
                                    else if (column.SystemTypeNative == "DateTime")
                                    {
                                        string str2 = "";
                                        if (appendAddEditRecordType == AppendAddEditRecordType.GridView)
                                            str2 = " id=\"" + column.NameCamelStyle + "\"";
                                        if (viewType == MVCGridViewType.CRUD)
                                            sb.AppendLine("                    <td class=\"editor-field\"><input asp-for=\"" + table.Name + "." + column.Name + "\" class=\"datetextbox\"" + str2 + " type=\"text\" onblur=\"isDataValid()\" /></td>");
                                        else
                                            sb.AppendLine("                    <td class=\"editor-field\"><input asp-for=\"" + table.Name + "." + column.Name + "\" class=\"datetextbox\"" + str2 + " type=\"text\" /></td>");
                                    }
                                    else if (column.SQLDataType == SQLType.bit)
                                        sb.AppendLine("                    <td class=\"editor-field\"><input asp-for=\"" + table.Name + "." + column.Name + "\" type=\"checkbox\"" + str1 + " /></td>");
                                    else if (column.IsDescriptionContainCodeGenFormat && !string.IsNullOrEmpty(column.MaskInputType))
                                    {
                                        if (viewType == MVCGridViewType.CRUD)
                                            sb.AppendLine("                    <td class=\"editor-field\"><input asp-for=\"" + table.Name + "." + column.Name + "\"" + str1 + " type=\"" + column.MaskInputType + "\" onblur=\"isDataValid()\" /></td>");
                                        else
                                            sb.AppendLine("                    <td class=\"editor-field\"><input asp-for=\"" + table.Name + "." + column.Name + "\"" + str1 + " type=\"" + column.MaskInputType + "\"\" /></td>");
                                    }
                                    else if (viewType == MVCGridViewType.CRUD)
                                    {
                                        if (column.IsNullable && !column.IsPrimaryKeyUnique && !column.IsForeignKey && (column.SQLDataType == SQLType.integer || column.SQLDataType == SQLType.bigint || (column.SQLDataType == SQLType.smallint || column.SQLDataType == SQLType.tinyint)))
                                            sb.AppendLine("                    <td class=\"editor-field\"><input asp-for=\"" + table.Name + "." + column.Name + "Hidden\"" + str1 + " onblur=\"isDataValid()\" /></td>");
                                        else
                                            sb.AppendLine("                    <td class=\"editor-field\"><input asp-for=\"" + table.Name + "." + column.Name + "\"" + str1 + " onblur=\"isDataValid()\" /></td>");
                                    }
                                    else if (column.IsNullable && !column.IsPrimaryKeyUnique && !column.IsForeignKey && (column.SQLDataType == SQLType.integer || column.SQLDataType == SQLType.bigint || (column.SQLDataType == SQLType.smallint || column.SQLDataType == SQLType.tinyint)))
                                        sb.AppendLine("                    <td class=\"editor-field\"><input asp-for=\"" + table.Name + "." + column.Name + "Hidden\"" + str1 + " /></td>");
                                    else
                                        sb.AppendLine("                    <td class=\"editor-field\"><input asp-for=\"" + table.Name + "." + column.Name + "\"" + str1 + " /></td>");
                                }
                                else if (appendAddEditRecordContentType == AppendAddEditRecordContentType.Details)
                                    sb.AppendLine("                    <td class=\"editor-field\"><label asp-for=\"" + table.Name + "." + column.Name + "\"" + str1 + "></label></td>");
                                else if (column.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK && appendAddEditRecordContentType == AppendAddEditRecordContentType.Unbound)
                                {
                                    if (column.IsNullable)
                                        sb.AppendLine("                    <td class=\"editor-field\"><select asp-for=\"" + table.Name + "." + column.Name + "\"><option value=\"\">Select One</option></select></td>");
                                    else
                                        sb.AppendLine("                    <td class=\"editor-field\"><select asp-for=\"" + table.Name + "." + column.Name + "\"><option value=\"\">Select One</option></select></td>");
                                }
                                else if (column.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK)
                                {
                                    if (appendAddEditRecordContentType == AppendAddEditRecordContentType.Unbound)
                                        sb.AppendLine("                    <td class=\"editor-field\"><select asp-for=\"" + table.Name + "." + column.Name + "\"><option value=\"\">Select One</option></select></td>");
                                    else
                                        sb.AppendLine("                    <td class=\"editor-field\"><select id=\"" + column.NameCamelStyle + "\" asp-for=\"" + table.Name + "." + column.Name + "\" asp-items=\"@(new SelectList(Model." + column.ForeignKeyTableName + MyConstants.WordDropDownListData + ", \"" + column.ForeignKeyColumnName + "\", \"" + column.ForeignKeyTable.DataTextField + "\"))\"><option value=\"\">Select One</option></select></td>");
                                }
                                else if (viewType == MVCGridViewType.CRUD)
                                    sb.AppendLine("                    <td class=\"editor-field\"><input asp-for=\"" + table.Name + "." + column.Name + "\"" + str1 + " onblur=\"isDataValid()\" /></td>");
                                else
                                    sb.AppendLine("                    <td class=\"editor-field\"><input asp-for=\"" + table.Name + "." + column.Name + "\"" + str1 + " /></td>");
                                if (appendAddEditRecordContentType != AppendAddEditRecordContentType.Details)
                                {
                                    if (column.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK && !column.IsNullable)
                                        sb.AppendLine("                    <td class=\"editor-field\"><span id=\"" + column.NameCamelStyle + "Validation\" style=\"color: red;\"></span></td>");
                                    else if (column.IsForeignKeyAndFKTableIsSelectedAndOnlyOnePK && column.IsNullable)
                                        sb.AppendLine("                    <td class=\"editor-field\"></td>");
                                    else if (column.SQLDataType == SQLType.bit)
                                        sb.AppendLine("                    <td class=\"editor-field\"></td>");
                                    else if (viewType == MVCGridViewType.CRUD)
                                        sb.AppendLine("                    <td class=\"editor-field\"><span id=\"" + column.NameCamelStyle + "Validation\" style=\"color: red;\"></span></td>");
                                    else if (column.IsNullable && !column.IsPrimaryKeyUnique && !column.IsForeignKey && (column.SQLDataType == SQLType.integer || column.SQLDataType == SQLType.bigint || (column.SQLDataType == SQLType.smallint || column.SQLDataType == SQLType.tinyint)))
                                        sb.AppendLine("                    <td class=\"editor-field\"><span asp-validation-for=\"" + table.Name + "." + column.Name + "Hidden\"></span></td>");
                                    else
                                        sb.AppendLine("                    <td class=\"editor-field\"><span asp-validation-for=\"" + table.Name + "." + column.Name + "\"></span></td>");
                                }
                                sb.AppendLine("                </tr>");
                            }
                        }
                    }
                    if (appendAddEditRecordType == AppendAddEditRecordType.AddEdit)
                    {
                        sb.AppendLine("                <tr>");
                        if (appendAddEditRecordContentType != AppendAddEditRecordContentType.Details)
                            sb.AppendLine("                    <td colspan=\"2\"></td>");
                        sb.AppendLine("                    <td colspan=\"2\">");
                        sb.AppendLine("                        <br />");
                        if (appendAddEditRecordContentType == AppendAddEditRecordContentType.Unbound)
                            sb.AppendLine("                        <input type=\"submit\" asp-page-handler=\"Submit\" value=\"Submit\" class=\"button-150\" />");
                        else if (appendAddEditRecordContentType == AppendAddEditRecordContentType.AddEditPartialView)
                        {
                            sb.AppendLine("                        @if (Model.Operation == CrudOperation.Add)");
                            sb.AppendLine("                        {");
                            sb.AppendLine("                            <input type=\"submit\" asp-page-handler=\"Add\" value=\"Add\" class=\"button-150\" />");
                            sb.AppendLine("                        }");
                            sb.AppendLine("                        else");
                            sb.AppendLine("                        {");
                            sb.AppendLine("                            <input type=\"submit\" asp-page-handler=\"Update\" value=\"Update\" class=\"button-150\" />");
                            sb.AppendLine("                        }");
                        }
                        sb.AppendLine("");
                        if (appendAddEditRecordContentType == AppendAddEditRecordContentType.Details)
                            sb.AppendLine("                    <a href=\"@Model.ReturnUrl\">Return</a>");
                        else
                            sb.AppendLine("                        <input type=\"button\" value=\"Cancel\" onclick=\"window.location='@Model.ReturnUrl'; return false;\" class=\"button-100\" />");
                        sb.AppendLine("                    </td>");
                        sb.AppendLine("                </tr>");
                    }
                   
                    else
                    {
                        sb.AppendLine("                <tr>");
                        sb.AppendLine("                    <td colspan=\"2\"></td>");
                        sb.AppendLine("                    <td colspan=\"2\">");
                        sb.AppendLine("                        <br />");
                        if (viewType == MVCGridViewType.CRUD)
                        {
                            sb.AppendLine("                        <input id=\"inputAdd\" name=\"inputAdd\" type=\"button\" value=\"Add\" class=\"button-150\" onclick=\"saveNewItem()\" />");
                            sb.AppendLine("                        <input id=\"inputUpdate\" name=\"inputUpdate\" type=\"button\" value=\"Update\" class=\"button-150\" onclick=\"updateCurrentItem()\" />");
                        }
                        else
                        {
                            sb.AppendLine("                        <input id=\"inputAdd\" name=\"inputAdd\" type=\"submit\" value=\"Add\" class=\"button-150\" asp-page-handler=\"Add\" />");
                            sb.AppendLine("                        <input id=\"inputUpdate\" name=\"inputUpdate\" type=\"submit\" value=\"Update\" class=\"button-150\" asp-page-handler=\"Update\" />");
                        }
                        sb.AppendLine("                        <input type=\"button\" value=\"Cancel\" onclick=\"closeDialog(); return false;\" class=\"button-100\" />");
                        sb.AppendLine("                    </td>");
                        sb.AppendLine("                </tr>");
                    }
                    sb.AppendLine("            </table>");
                    sb.AppendLine("        </fieldset>");
                    sb.AppendLine("    </div>");
                    if (appendAddEditRecordContentType == AppendAddEditRecordContentType.Details)
                        break;
                    sb.AppendLine("</form>");
                    break;
                default:
                    sb.AppendLine("<form method=\"post\">");
                    sb.AppendLine("<input type=\"hidden\" asp-for=\"ReturnUrl\" />");
                    goto case AppendAddEditRecordContentType.Details;
            }
        }

        internal static string GetFieldType(Column column)
        {
            if (column.SystemType.ToLower() == "string")
                return "String";
            if (column.SQLDataType == SQLType.bit)
                return "Boolean";
            if (column.SQLDataType == SQLType.decimalnumber || column.SQLDataType == SQLType.money || (column.SQLDataType == SQLType.smallmoney || column.SQLDataType == SQLType.floatnumber) || column.SQLDataType == SQLType.real)
                return "Decimal";
            if (column.SQLDataType == SQLType.numeric || column.SQLDataType == SQLType.integer || (column.SQLDataType == SQLType.smallint || column.SQLDataType == SQLType.bigint))
                return "Numeric";
            return column.SQLDataType == SQLType.datetime || column.SQLDataType == SQLType.smalldatetime || (column.SQLDataType == SQLType.date || column.SQLDataType == SQLType.datetime2) ? "Date" : "Default";
        }

        internal static string GetCommaDelimitedParamsForSearchMethod(Table table, Language language = Language.CSharp)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Column column in (List<Column>)table.Columns)
            {
                if (!column.IsBinaryOrSpatialDataType)
                {
                    string str = "?";
                    if (column.SystemType.ToLower() == "string")
                        str = "";
                    if (language == Language.CSharp)
                        stringBuilder.Append(column.SystemType + str + " " + column.NameCamelStyle + ", ");
                    else
                        stringBuilder.Append(column.NameCamelStyle + " As " + column.SystemType + str + ", ");
                }
            }
            return Functions.RemoveLastComma(stringBuilder.ToString());
        }

        internal static string GetCommaDelimitedParamsForPassingToSearchMethod(Table table)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Column column in (List<Column>)table.Columns)
            {
                if (!column.IsBinaryOrSpatialDataType)
                    stringBuilder.Append(column.NameCamelStyle + ", ");
            }
            return Functions.RemoveLastComma(stringBuilder.ToString());
        }

        internal static bool CanConnectToInternet()
        {
            WebRequest webRequest = (WebRequest)null;
            WebResponse webResponse = (WebResponse)null;
            bool flag = true;
            try
            {
                webRequest = WebRequest.Create("http://www.google.com");
                webResponse = webRequest.GetResponse();
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (WebException ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                flag = false;
                webRequest?.Abort();
            }
            finally
            {
                webResponse?.Close();
            }
            return flag;
        }

        internal static bool IsAReservedKeyword(string identifier, Language language)
        {
            bool flag = language != Language.CSharp ? !new VBCodeProvider().IsValidIdentifier(identifier) : !new CSharpCodeProvider().IsValidIdentifier(identifier);
            if (!flag)
                return KeywordsFactory.GetKeywords(language).IsThisVariableAKeyword(identifier);
            return flag;
        }

        internal static string GetSlashDelimitedPrimaryKeys(Table table, Language language, bool isForSampleData = false)
        {
            int num = 0;
            string str = string.Empty;
            StringBuilder stringBuilder = new StringBuilder();
            if (isForSampleData)
                str = "Sample";
            foreach (Column primaryKeyColumn in (List<Column>)table.PrimaryKeyColumns)
            {
                if (language == Language.CSharp)
                {
                    if (num > 0)
                        stringBuilder.Append(" + \"/\" + ");
                }
                else if (num > 0)
                    stringBuilder.Append(" & \"/\" & ");
                stringBuilder.Append(primaryKeyColumn.NameCamelStyle + str);
                ++num;
            }
            return stringBuilder.ToString();
        }

        internal static string GetSlashDelimitedPrimaryKeysInCurlies(Table table)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Column primaryKeyColumn in (List<Column>)table.PrimaryKeyColumns)
                stringBuilder.Append("/{" + primaryKeyColumn.NameCamelStyle + "}");
            return stringBuilder.ToString();
        }

        internal static string GetQueryStringDelimitedPrimaryKeys(Table table, Language language, string[] sampleValueArray = null)
        {
            int index = 0;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Column primaryKeyColumn in (List<Column>)table.PrimaryKeyColumns)
            {
                if (language == Language.CSharp)
                {
                    if (sampleValueArray == null)
                    {
                        if (index > 0)
                            stringBuilder.Append(" + \"&" + primaryKeyColumn.NameCamelStyle + "=\" + " + primaryKeyColumn.NameCamelStyle);
                        else
                            stringBuilder.Append("\"" + primaryKeyColumn.NameCamelStyle + "=\" + " + primaryKeyColumn.NameCamelStyle);
                    }
                    else
                    {
                        string str = "\"" + sampleValueArray[index].Trim().Replace("\"", "") + "\"";
                        if (index > 0)
                            stringBuilder.Append(" + \"&" + primaryKeyColumn.NameCamelStyle + "=\" + " + str);
                        else
                            stringBuilder.Append("\"" + primaryKeyColumn.NameCamelStyle + "=\" + " + str);
                    }
                }
                else if (sampleValueArray == null)
                {
                    if (index > 0)
                        stringBuilder.Append(" + \"&" + primaryKeyColumn.NameCamelStyle + "=\" & " + primaryKeyColumn.NameCamelStyle);
                    else
                        stringBuilder.Append("\"" + primaryKeyColumn.NameCamelStyle + "=\" & " + primaryKeyColumn.NameCamelStyle);
                }
                else
                {
                    string str = "\"" + sampleValueArray[index].Trim().Replace("\"", "") + "\"";
                    if (index > 0)
                        stringBuilder.Append(" + \"&" + primaryKeyColumn.NameCamelStyle + "=\" & " + str);
                    else
                        stringBuilder.Append("\"" + primaryKeyColumn.NameCamelStyle + "=\" & " + str);
                }
                ++index;
            }
            return stringBuilder.ToString();
        }

        internal static JQueryUITheme GetJQueryUITheme(string jqueryUITheme)
        {
            switch (jqueryUITheme.ToLower())
            {
                case "black-tie":
                    return JQueryUITheme.BlackTie;
                case "blitzer":
                    return JQueryUITheme.Blitzer;
                case "cupertino":
                    return JQueryUITheme.Cupertino;
                case "dark-hive":
                    return JQueryUITheme.DarkHive;
                case "dot-luv":
                    return JQueryUITheme.DotLuv;
                case "eggplant":
                    return JQueryUITheme.Eggplant;
                case "excite-bike":
                    return JQueryUITheme.ExciteBike;
                case "hot-sneaks":
                    return JQueryUITheme.HotSneaks;
                case "humanity":
                    return JQueryUITheme.Humanity;
                case "le-frog":
                    return JQueryUITheme.LeFrog;
                case "mint-choc":
                    return JQueryUITheme.MintChoc;
                case "overcast":
                    return JQueryUITheme.Overcast;
                case "pepper-grinder":
                    return JQueryUITheme.PepperGrinder;
                case "smoothness":
                    return JQueryUITheme.Smoothness;
                case "south-street":
                    return JQueryUITheme.SouthStreet;
                case "start":
                    return JQueryUITheme.Start;
                case "sunny":
                    return JQueryUITheme.Sunny;
                case "swanky-purse":
                    return JQueryUITheme.SwankyPurse;
                case "trontastic":
                    return JQueryUITheme.Trontastic;
                case "ui-darkness":
                    return JQueryUITheme.UIDarkness;
                case "ui-lightness":
                    return JQueryUITheme.UILightness;
                case "vader":
                    return JQueryUITheme.Vader;
                default:
                    return JQueryUITheme.Redmond;
            }
        }

        internal static string GetHeaderFooterTableBgColorByJQueryUITheme(JQueryUITheme jqueryUITheme)
        {
            switch (jqueryUITheme)
            {
                case JQueryUITheme.BlackTie:
                    return "#000000";
                case JQueryUITheme.Blitzer:
                    return "#EEEEEE";
                case JQueryUITheme.Cupertino:
                    return "#D9ECF9";
                case JQueryUITheme.DarkHive:
                    return "#2B2B2B";
                case JQueryUITheme.DotLuv:
                    return "#474747";
                case JQueryUITheme.Eggplant:
                    return "#ECECEC";
                case JQueryUITheme.ExciteBike:
                    return "#1484E6";
                case JQueryUITheme.HotSneaks:
                    return "#93C3CD";
                case JQueryUITheme.Humanity:
                    return "#EEE5D6";
                case JQueryUITheme.LeFrog:
                    return "#51A412";
                case JQueryUITheme.MintChoc:
                    return "#2D2721";
                case JQueryUITheme.Overcast:
                    return "#EEEEEE";
                case JQueryUITheme.PepperGrinder:
                    return "#F6F5F4";
                case JQueryUITheme.Smoothness:
                    return "#E6E6E6";
                case JQueryUITheme.SouthStreet:
                    return "#469E01";
                case JQueryUITheme.Start:
                    return "#0178AE";
                case JQueryUITheme.Sunny:
                    return "#FEDB66";
                case JQueryUITheme.SwankyPurse:
                    return "#554929";
                case JQueryUITheme.Trontastic:
                    return "#0E0E0E";
                case JQueryUITheme.UIDarkness:
                    return "#585858";
                case JQueryUITheme.UILightness:
                    return "#F9F9F9";
                case JQueryUITheme.Vader:
                    return "#AFAFAF";
                default:
                    return "#DFEFFC";
            }
        }

        internal static string GetHeaderFooterFontColorByJQueryUITheme(JQueryUITheme jqueryUITheme)
        {
            switch (jqueryUITheme)
            {
                case JQueryUITheme.BlackTie:
                    return "#FFFFFF";
                case JQueryUITheme.Blitzer:
                    return "#CF0C0C";
                case JQueryUITheme.Cupertino:
                    return "#72A7CF";
                case JQueryUITheme.DarkHive:
                    return "#FFFFFF";
                case JQueryUITheme.DotLuv:
                    return "#FFFFFF";
                case JQueryUITheme.Eggplant:
                    return "#7A5874";
                case JQueryUITheme.ExciteBike:
                    return "#FFFFFF";
                case JQueryUITheme.HotSneaks:
                    return "#35414F";
                case JQueryUITheme.Humanity:
                    return "#6F6E6B";
                case JQueryUITheme.LeFrog:
                    return "#FFFFFF";
                case JQueryUITheme.MintChoc:
                    return "#9BCC60";
                case JQueryUITheme.Overcast:
                    return "#6783BF";
                case JQueryUITheme.PepperGrinder:
                    return "#808080";
                case JQueryUITheme.Smoothness:
                    return "#555555";
                case JQueryUITheme.SouthStreet:
                    return "#FFFFFF";
                case JQueryUITheme.Start:
                    return "#FFFFFF";
                case JQueryUITheme.Sunny:
                    return "#4C3000";
                case JQueryUITheme.SwankyPurse:
                    return "#F8EEC9";
                case JQueryUITheme.Trontastic:
                    return "#B8EC79";
                case JQueryUITheme.UIDarkness:
                    return "#FFFFFF";
                case JQueryUITheme.UILightness:
                    return "#1C94CE";
                case JQueryUITheme.Vader:
                    return "#666666";
                default:
                    return "#2E6E9E";
            }
        }

        internal static string GetFullyQualifiedModelName(Table table, Tables selectedTables, string nameSpace)
        {
            if (selectedTables.Where<Table>((Func<Table, bool>)(t => t.Name == table.Name + MyConstants.WordModel)).Count<Table>() > 0)
                return nameSpace + ".Models." + table.Name + MyConstants.WordModel;
            return table.Name + MyConstants.WordModel;
        }

        internal static string GetFullyQualifiedTableName(Table table, Tables selectedTables, Language language, string fullyQualifiedBusinessObjectName, string nameSpace = "")
        {
            if (selectedTables.Where<Table>((Func<Table, bool>)(t => t.Name + MyConstants.WordModel == fullyQualifiedBusinessObjectName)).Count<Table>() <= 0)
                return fullyQualifiedBusinessObjectName;
            if (language != Language.CSharp)
                return MyConstants.WordBusinessObject + "." + table.Name;
            return nameSpace + "." + MyConstants.WordBusinessObject + "." + table.Name;
        }

        internal static string GetMinimumValueOfPrimaryKey(Column column)
        {
            if (column.SystemType == "int" || column.SystemType == "Int64" || (column.SystemType == "Int16" || column.SystemType == "byte"))
                return "-1";
            if (column.SystemType == "double" || column.SystemType == "Single" || column.SystemType == "decimal")
                return "0.0";
            if (column.SQLDataType == SQLType.uniqueidentifier)
                return "00000000-0000-0000-0000-000000000000";
            return string.Empty;
        }

        internal static void GetAdditionalJavaScriptForValidationScriptsPartial(StringBuilder sb, Table table)
        {
            sb.AppendLine("@section AdditionalJavaScript {");
            if (table.IsGenerateMaskPartialView)
                sb.AppendLine("    @await Html.PartialAsync(\"_Mask" + table.Name + "Partial\")");
            sb.AppendLine("    @await Html.PartialAsync(\"_ValidationScriptsPartial\")");
            sb.AppendLine("");
            sb.AppendLine("    <script type=\"text/javascript\">");
            sb.AppendLine("        $(function () {");
            sb.AppendLine("            $('.datetextbox').datepicker({dateFormat: 'm/d/yy'});");
            sb.AppendLine("        });");
            sb.AppendLine("    </script>");
            sb.AppendLine("}");
            sb.AppendLine("");
        }

        internal static string GetLinqContextDotPrimaryKeysEquals(Table table, Language language, GeneratedSqlType generatedSqlType)
        {
            int num = 0;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Column primaryKeyColumn in (List<Column>)table.PrimaryKeyColumns)
            {
                if (language == Language.CSharp)
                {
                    if (num > 0)
                        stringBuilder.Append(" &&");
                    if (generatedSqlType == GeneratedSqlType.EFCore && primaryKeyColumn.SQLDataType == SQLType.uniqueidentifier)
                        stringBuilder.Append(table.LinqFromVariable + "." + primaryKeyColumn.Name + ".ToString() == " + primaryKeyColumn.NameCamelStyle);
                    else
                        stringBuilder.Append(table.LinqFromVariable + "." + primaryKeyColumn.Name + " == " + primaryKeyColumn.NameCamelStyle);
                }
                else
                {
                    if (num > 0)
                        stringBuilder.Append(" And");
                    if (generatedSqlType == GeneratedSqlType.EFCore && primaryKeyColumn.SQLDataType == SQLType.uniqueidentifier)
                        stringBuilder.Append(table.LinqFromVariable + "." + primaryKeyColumn.Name + " = " + primaryKeyColumn.NameCamelStyle);
                    else
                        stringBuilder.Append(table.LinqFromVariable + "." + primaryKeyColumn.Name + ".ToString() = " + primaryKeyColumn.NameCamelStyle);
                }
                ++num;
            }
            return stringBuilder.ToString();
        }

        internal static string GetLinqContextDotPrimaryKeysEqualsObjTableName(Table table, Language language)
        {
            int num = 0;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Column primaryKeyColumn in (List<Column>)table.PrimaryKeyColumns)
            {
                if (language == Language.CSharp)
                {
                    if (num > 0)
                        stringBuilder.Append(" &&");
                    stringBuilder.Append(table.LinqFromVariable + "." + primaryKeyColumn.Name + " == " + table.VariableObjName + "." + primaryKeyColumn.Name);
                }
                else
                {
                    if (num > 0)
                        stringBuilder.Append(" And");
                    stringBuilder.Append(table.LinqFromVariable + "." + primaryKeyColumn.Name + " = " + table.VariableObjName + "." + primaryKeyColumn.Name);
                }
                ++num;
            }
            return stringBuilder.ToString();
        }

        internal static void ReplaceLaunchPortAndCreateFile(string fullFileNamePath, string launchPort, string webAppName, string webAPIName)
        {
            string contents = System.IO.File.ReadAllText(fullFileNamePath).Replace("[LaunchPort]", launchPort).Replace("[WebAppName]", webAppName).Replace("[WebApiName]", webAPIName);
            System.IO.File.WriteAllText(fullFileNamePath, contents);
        }

        internal static string GetViewName(MVCGridViewType _viewType, string viewName, Column currentColumn)
        {
            switch (_viewType)
            {
                case MVCGridViewType.GroupedBy:
                    return viewName + currentColumn.Name;
                case MVCGridViewType.GroupedByWithTotals:
                    return viewName + currentColumn.Name;
                case MVCGridViewType.MasterDetailGrid:
                    return viewName + currentColumn.Name;
                case MVCGridViewType.MasterDetailSubGrid:
                    return viewName + currentColumn.Name;
                default:
                    return viewName;
            }
        }

        internal static void WriteBusinessObjectBindProperty(StringBuilder sb, Table table, string apiName)
        {
            sb.AppendLine("         [BindProperty]");
            sb.AppendLine("         public " + apiName + "." + MyConstants.WordBusinessObject + "." + table.Name + " " + table.Name + " { get; set; }");
            sb.AppendLine("");
        }

        internal static void WriteReturnUrlBindProperty(StringBuilder sb)
        {
            sb.AppendLine("         [BindProperty]");
            sb.AppendLine("         public string ReturnUrl { get; set; }");
            sb.AppendLine("");
        }

        internal static void WriteDropDownListDataProperties(StringBuilder sb, Table table)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Column column in table.ColumnsWithDropDownListData)
            {
                if (!stringBuilder.ToString().Contains(column.DropDownListDataPropertyName + ","))
                {
                    sb.AppendLine("         public List<" + column.ForeignKeyTableName + "> " + column.DropDownListDataPropertyName + ";");
                    stringBuilder.Append(column.DropDownListDataPropertyName + ",");
                }
            }
            if (stringBuilder.Length <= 0)
                return;
            sb.AppendLine("");
        }

        internal static void WriteOnGetPropertyAssignMents(StringBuilder sb, Table table, Tables selectedTables, MVCGridViewType viewType, string apiName, bool isUseWebApi, ViewNames viewNamesCollection, GeneratedSqlType generatedSqlType, string modelName, string businessObjectName)
        {
            StringBuilder stringBuilder = new StringBuilder();
            Language language = Language.CSharp;
            string str = string.Empty;
            if (viewType == MVCGridViewType.Add || viewType == MVCGridViewType.Update )
                str = "model.";
            if (viewType == MVCGridViewType.AddEdit || viewType == MVCGridViewType.Unbound)
                sb.AppendLine("             " + str + table.Name + " = new " + apiName + "." + MyConstants.WordBusinessObject + "." + table.Name + "();");
            if (table.IsContainsHiddenColumns && (viewType == MVCGridViewType.Update || viewType == MVCGridViewType.RecordDetails || viewType == MVCGridViewType.AssignWorkflowSteps))
            {
                sb.AppendLine("             // assign values to the model");
                sb.AppendLine("             " + table.Name + " " + table.VariableObjName + "Temp = new " + table.Name + "();");
                foreach (Column column in table.Columns)
                {
                    sb.AppendLine("             " + table.VariableObjName + "Temp." + column.Name + " = " + table.VariableObjName + "." + column.Name + ";");
                    if (column.IsNullable && !column.IsPrimaryKeyUnique && !column.IsForeignKey && (column.SQLDataType == SQLType.integer || column.SQLDataType == SQLType.bigint || (column.SQLDataType == SQLType.smallint || column.SQLDataType == SQLType.tinyint)))
                    {
                        sb.AppendLine("");
                        sb.AppendLine("             if (" + table.VariableObjName + "." + column.Name + ".HasValue)");
                        sb.AppendLine("                 " + table.VariableObjName + "Temp." + column.Name + "Hidden = " + table.VariableObjName + "." + column.Name + ".Value.ToString();");
                        sb.AppendLine("             else");
                        sb.AppendLine("                 " + table.VariableObjName + "Temp." + column.Name + "Hidden = null;");
                        sb.AppendLine("");
                    }
                }
            }
            if (viewType == MVCGridViewType.Add || viewType == MVCGridViewType.Update )
            {
                str = "model.";
                sb.AppendLine("             // create the model used by the partial page");
                sb.AppendLine("             AddEdit" + table.Name + "PartialModel model = new AddEdit" + table.Name + "PartialModel();");
            }
            
            if (viewType != MVCGridViewType.RecordDetails)
            {
                foreach (Column column in table.ColumnsWithDropDownListData)
                {
                    if (!stringBuilder.ToString().Contains(column.DropDownListDataPropertyName + ","))
                    {
                        if (isUseWebApi)
                        {
                            sb.AppendLine("             " + str + column.DropDownListDataPropertyName + " = Get" + column.ForeignKeyTable.Name + "DropDownListData();");
                        }
                        else
                        {
                            string qualifiedTableName = Functions.GetFullyQualifiedTableName(column.ForeignKeyTable, selectedTables, language, column.ForeignKeyTableNameFullyQualifiedBusinessObject, apiName);
                            sb.AppendLine("             " + str + column.DropDownListDataPropertyName + " = " + qualifiedTableName + ".Select" + column.ForeignKeyTableName + "DropDownListData();");
                        }
                        stringBuilder.Append(column.DropDownListDataPropertyName + ",");
                    }
                }
            }
            if (viewType == MVCGridViewType.Add)
            {
                sb.AppendLine("             model.Operation = CrudOperation.Add;");
                sb.AppendLine("             model.ReturnUrl = returnUrl;");
            }
            else if (viewType == MVCGridViewType.Update || viewType == MVCGridViewType.AssignWorkflowSteps)
            {
                sb.AppendLine("             model.Operation = CrudOperation.Update;");
                sb.AppendLine("             model.ReturnUrl = returnUrl;");
                if (table.IsContainsHiddenColumns)
                    sb.AppendLine("             model." + table.Name + " = " + table.VariableObjName + "Temp;");
                else
                    sb.AppendLine("             model." + table.Name + " = " + table.VariableObjName + ";");
            }
            if (viewType == MVCGridViewType.Add || viewType == MVCGridViewType.Update )
            {
                sb.AppendLine("");
                sb.AppendLine("             // assign values to the model used by this page");
                sb.AppendLine("             PartialModel = model;");
                sb.AppendLine("");
                sb.AppendLine("             // assign the return url");
                sb.AppendLine("             ReturnUrl = returnUrl;");
                sb.AppendLine("");
                sb.AppendLine("             return Page();");
            }
            else
            {
                if (viewType != MVCGridViewType.RecordDetails)
                    return;
                if (table.IsContainsHiddenColumns)
                {
                    sb.AppendLine("");
                    sb.AppendLine("             // assign values to this page's bound property");
                    sb.AppendLine("             " + table.Name + " = " + table.VariableObjName + "Temp;");
                }
                else
                {
                    sb.AppendLine("             // assign values to this page's bound property");
                    sb.AppendLine("             " + table.Name + " = " + table.VariableObjName + ";");
                }
                sb.AppendLine("");
                sb.AppendLine("             // assign the return url");
                sb.AppendLine("             ReturnUrl = returnUrl;");
            }
        }

        internal static void WriteGetSessionObject(StringBuilder sb)
        {
            sb.AppendLine("");
            sb.AppendLine("         /// <summary>");
            sb.AppendLine("         /// Get Object from session");
            sb.AppendLine("         /// </summary>");
            sb.AppendLine("         public object GetObjectFromSession(string key, Type type)");
            sb.AppendLine("         {");
            sb.AppendLine("             string value = HttpContext.Session.GetString(key);");
            sb.AppendLine("             object obj = null;");
            sb.AppendLine("             ");
            sb.AppendLine("             if (!string.IsNullOrEmpty(value))");
            sb.AppendLine("             {");
            sb.AppendLine("                 obj = JsonConvert.DeserializeObject(value, type);");
            sb.AppendLine("             }");
            sb.AppendLine("             ");
            sb.AppendLine("             return obj;");
            sb.AppendLine("         }");
            sb.AppendLine("");
        }
    }
}
