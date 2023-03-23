using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrinitySharp.DapperUtils
{
    public static class DapperHelper
    {
        public static void RegisterModelMapping<Type>()
        {
            Dapper.SqlMapper.SetTypeMap(typeof(Type), new TrinityTypeMapper<Type>());
        }
    }
}
