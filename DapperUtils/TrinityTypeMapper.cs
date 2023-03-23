using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TrinitySharp.DapperUtils
{
    public class TrinityTypeMapper<ModelType> : Dapper.SqlMapper.ITypeMap
    {
        Dictionary<string, string> SqlColumnMappings { get; set; }
        public TrinityTypeMapper()
        {
            SqlColumnMappings = new Dictionary<string, string>();
            var attributes = new List<Attributes.SqlColumn>();
            var properties = typeof(ModelType).GetProperties();
            if(properties != null && properties.Length > 0)
            {
                foreach( var property in properties) 
                {
                    var attr = property.GetCustomAttribute<Attributes.SqlColumn>();
                    if (attr != null) { attributes.Add(attr); }
                }

                if (attributes != null && attributes.Count > 0)
                {
                    foreach (var attr in attributes)
                    {
                        SqlColumnMappings.Add(attr.FieldName, attr.PropertyName);
                    }
                }
            }
        }
            

        public ConstructorInfo FindConstructor(string[] names, Type[] types)
        {
            return typeof(ModelType).GetConstructor(Array.Empty<Type>());
        }

        public ConstructorInfo FindExplicitConstructor()
        {
            return null;
        }

        public SqlMapper.IMemberMap GetConstructorParameter(ConstructorInfo constructor, string columnName)
        {
            throw new NotSupportedException();
        }


        public SqlMapper.IMemberMap GetMember(string columnName)
        {
            if (SqlColumnMappings.ContainsKey(columnName))
            {
                var prop = typeof(ModelType).GetProperty(SqlColumnMappings[columnName]);
                return prop != null ? new TrinityMemberMap(columnName, prop) : null;
            }

            return null;

        }
    }

    
}
