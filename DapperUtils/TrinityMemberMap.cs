using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TrinitySharp.DapperUtils
{
    public class TrinityMemberMap : Dapper.SqlMapper.IMemberMap
    {
        public TrinityMemberMap(string ColumnName, PropertyInfo Property) 
        {
            _columnName= ColumnName;
            _property= Property;
        }
        private string _columnName = string.Empty;
        private PropertyInfo _property;
        public string ColumnName => _columnName;

        public Type MemberType => Property?.PropertyType;

        public PropertyInfo Property => _property;

        public FieldInfo Field => throw new NotImplementedException();

        public ParameterInfo Parameter => throw new NotImplementedException();
    }
}
