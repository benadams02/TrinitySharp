using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using static TrinitySharp.Attributes;

namespace TrinitySharp
{
    public static partial class TypeExtensions
    {
        public static PropertyInfo GetPrimaryKeyProperty(this Type type)
        {
            if (type == null) { throw new ArgumentNullException(); }

            PropertyInfo[] props = type.GetProperties();

            for (int i = 0; i < props.Length; i++)
            {
                Attributes.SqlColumn attr = props[i].GetCustomAttribute<Attributes.SqlColumn>();
                if (attr != null && attr.PrimaryKey) { return props[i]; }
            }

            return null;
        }

        public static Attributes.SqlColumn GetPrimaryKeyAttribute(this Type type)
        {
            if (type == null) { throw new ArgumentNullException(); }

            PropertyInfo[] props = type.GetProperties();

            for (int i = 0; i < props.Length; i++)
            {
                Attributes.SqlColumn attr = props[i].GetCustomAttribute<Attributes.SqlColumn>();
                if (attr != null && attr.PrimaryKey) { return attr; }
            }

            return null;
        }

        /// <summary>
        /// Generates a SQL Insert Command string with parameters
        /// </summary>
        /// <param name="typeIn"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string GenerateSqlInsertString(this Type typeIn)
        {
            if (typeIn == null) { throw new ArgumentNullException(nameof(typeIn)); }

            Attributes.SqlTable sqlTable = typeIn.GetCustomAttribute<Attributes.SqlTable>();
            if (sqlTable != null)
            {
                List<Attributes.SqlColumn> sqlColumns = new List<Attributes.SqlColumn>();
                var props = typeIn.GetProperties();

                foreach (var prop in props)
                {
                    var attr = prop.GetCustomAttribute<Attributes.SqlColumn>();
                    if (attr != null && attr.PrimaryKey == false) { sqlColumns.Add(attr); }

                }

                if (sqlColumns.Count > 0)
                {
                    List<string> sqlColumnNames = sqlColumns.Select(x => x.FieldName).ToList();
                    List<string> sqlColumnParams = sqlColumnNames.ToList();
                    if (sqlColumnNames.Count > 0)
                    {
                        for (int i = 0; i < sqlColumnNames.Count; i++)
                        {
                            sqlColumnNames[i] = $"[{sqlColumnNames[i]}]";
                        }
                    }
                    if (sqlColumnParams.Count > 0)
                    {
                        for (int i = 0; i < sqlColumnParams.Count; i++)
                        {
                            sqlColumnParams[i] = $"@{sqlColumnParams[i]}";
                        }
                    }

                    string cmd = $"INSERT INTO [dbo].[{sqlTable.TableName}] ({string.Join(", ",sqlColumnNames)}) VALUES ({string.Join(", ", sqlColumnParams)})";

                    return cmd;
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GenerateSqlUpdateString(this Type typeIn)
        {
            if (typeIn == null) { throw new ArgumentNullException(nameof(typeIn)); }

            Attributes.SqlTable sqlTable = typeIn.GetCustomAttribute<Attributes.SqlTable>();
            if (sqlTable != null)
            {
                List<Attributes.SqlColumn> sqlColumns = new List<Attributes.SqlColumn>();
                var props = typeIn.GetProperties();

                foreach (var prop in props)
                {
                    var attr = prop.GetCustomAttribute<Attributes.SqlColumn>();
                    if (attr != null) { sqlColumns.Add(attr); }

                }

                if (sqlColumns.Count > 0)
                {
                    string sqlPrimaryKey = sqlColumns.Where(x => x.PrimaryKey == true).Select(y=>y.FieldName).FirstOrDefault();
                    List<string> sqlColumnNames = sqlColumns.Where(y=>y.PrimaryKey == false).Select(x => x.FieldName).ToList();

                    var updateStrings = "";
                    foreach (var str in sqlColumnNames)
                    {
                        updateStrings = updateStrings + $"[{str}] = @{str}, ";
                    }
                    if (updateStrings.EndsWith(", ")) { updateStrings = updateStrings.Substring(0, updateStrings.Length - 2); }
                    string cmd = $"UPDATE [dbo].[{sqlTable.TableName}] SET {updateStrings} WHERE  [{sqlPrimaryKey}] = @{sqlPrimaryKey}";

                    return cmd;
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GenerateSqlDeleteString(this Type typeIn)
        {
            if (typeIn == null) { throw new ArgumentNullException(nameof(typeIn)); }

            Attributes.SqlTable sqlTable = typeIn.GetCustomAttribute<Attributes.SqlTable>();
            if (sqlTable != null)
            {
                List<Attributes.SqlColumn> sqlColumns = new List<Attributes.SqlColumn>();
                var props = typeIn.GetProperties();

                foreach (var prop in props)
                {
                    var attr = prop.GetCustomAttribute<Attributes.SqlColumn>();
                    if (attr != null && attr.PrimaryKey) { sqlColumns.Add(attr); }

                }

                if (sqlColumns.Count > 0)
                {
                    string sqlPrimaryKey = sqlColumns.First().FieldName;

                    string cmd = $"DELETE FROM [dbo].[{sqlTable.TableName}] WHERE  [{sqlPrimaryKey}] = @{sqlPrimaryKey}";

                    return cmd;
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public static List<Attributes.SqlColumn> GetSqlColumns(this Type typeIn)
        {
            List<Attributes.SqlColumn> sqlColumns = new List<Attributes.SqlColumn>();
            var props = typeIn.GetProperties();

            foreach (var prop in props)
            {
                var attr = prop.GetCustomAttribute<Attributes.SqlColumn>();
                if (attr != null) { sqlColumns.Add(attr); }

            }

            return sqlColumns;
        }
    }
}
