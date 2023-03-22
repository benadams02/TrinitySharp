using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TrinitySharp
{
    public static partial class SqlParameterExtension
    {
        /// <summary>
        /// Generates a SQL Insert Command string with parameters
        /// </summary>
        /// <param name="typeIn"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static IEnumerable<SqlParameter> GenerateSqlParamterCollection(this object obj)
        {
            if (obj == null) { throw new ArgumentNullException(nameof(obj)); }

            PropertyInfo[] properties = obj.GetType().GetProperties().Where(x => x.IsDefined(typeof(Attributes.SqlColumn))).ToArray();

            if (properties.Length > 0)
            {
                SqlParameter[] sqlParameters = new SqlParameter[properties.Length];

                for (int i = 0; i < properties.Length; i++)
                {
                    PropertyInfo prop = properties[i];
                    Attributes.SqlColumn attr = prop.GetCustomAttribute<Attributes.SqlColumn>();

                    if (attr != null && attr.PrimaryKey == false)
                    {
                        SqlParameter sqlParam = new SqlParameter();
                        sqlParam.ParameterName = $"@{attr.FieldName}";
                        sqlParam.SqlDbType = attr.SqlDbType;
                        sqlParam.Value = prop.GetValue(obj);
                        sqlParameters[i] = sqlParam;
                    }
                }
                sqlParameters = sqlParameters.Where(x => x != null).ToArray();

                return (IEnumerable<SqlParameter>)sqlParameters;
            }

            return null;

        }
    }
}
