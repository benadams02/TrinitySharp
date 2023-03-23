using Dapper;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace TrinitySharp.SQL.Repositories
{
    public class GenericRepository<ModelType> : IRepository<ModelType> where ModelType : class
    {
        private Type thisType;
        protected Attributes.SqlTable sqlTableAttr { get; set; }
        protected SqlConnection Connection { get; set; }
        protected Dictionary<Attributes.SqlColumn, PropertyInfo> SqlColumns { get; set; }
        public GenericRepository(SqlConnection sqlConnection)
        {
            thisType = typeof(ModelType);
            Connection = sqlConnection;
            sqlTableAttr = thisType.GetCustomAttributes(typeof(Attributes.SqlTable), true).FirstOrDefault() as Attributes.SqlTable;
            SqlColumns = GetSqlProperties(thisType);
        }

        public virtual bool Delete(int ID)
        {
            string sqlStatement = typeof(ModelType).GenerateSqlDeleteString();
            if (string.IsNullOrEmpty(sqlStatement)) return false;

            SqlCommand cmd = new SqlCommand(sqlStatement, Connection);
            var pkAttr = typeof(ModelType).GetPrimaryKeyAttribute();
            if (pkAttr != null)
            {
                cmd.Parameters.Add(new SqlParameter($"@{pkAttr.FieldName}", pkAttr.SqlDbType) { Value = ID});
            }

            return cmd.ExecuteNonQuery() > 0;
        }

        public virtual IQueryable<ModelType> GetAll()
        {
            if (sqlTableAttr != null)
            {
                IEnumerable<ModelType> objects = Connection.Query<ModelType>($"SELECT * FROM [{sqlTableAttr.TableName}]");
                
                return objects.AsQueryable();
            }
            else
            {
                return null;
            }
        }

        public virtual ModelType GetByID(int ID)
        {
            
            if (sqlTableAttr != null)
            {
                string sqlPrimaryKey = SqlColumns.Where(x => x.Key.PrimaryKey == true).Select(x => x.Key.FieldName).First();

                var results = Connection.Query<ModelType>($"SELECT * FROM [{sqlTableAttr.TableName}] WHERE [{sqlPrimaryKey}] = {ID}");

                if (results.Count() > 0)
                {
                    ModelType thisObj = results.First();
                    return thisObj;
                }
                

                return default(ModelType);
            }
            else
            {
                return default(ModelType);
            }
        }

        public virtual bool Save(ModelType ObjIn)
        {
            throw new NotImplementedException();
        }

        public virtual bool Update(ModelType ObjIn)
        {
            string sqlStatement = typeof(ModelType).GenerateSqlUpdateString();
            if (string.IsNullOrEmpty(sqlStatement)) return false;

            SqlCommand cmd = new SqlCommand(sqlStatement, Connection);
            var paramList = ObjIn.GenerateSqlParameterCollection().ToList();
            if (paramList != null && paramList.Count > 0)
            {
                foreach (var param in paramList)
                {
                    cmd.Parameters.Add(param);
                }
            }

            return cmd.ExecuteNonQuery() > 0;
        }

        public virtual ModelType Insert(ModelType ObjIn)
        {
            string sqlStatement = typeof(ModelType).GenerateSqlInsertString();
            if (string.IsNullOrEmpty(sqlStatement)) return default(ModelType);

            sqlStatement += "; SELECT SCOPE_IDENTITY()";
            SqlCommand cmd = new SqlCommand(sqlStatement, Connection);
            var paramList = ObjIn.GenerateSqlParameterCollection(true).ToList();
            if (paramList != null && paramList.Count >0) 
            {
                foreach (var param in paramList)
                {
                    cmd.Parameters.Add(param);
                }
            }
            else
            {
                return default(ModelType);
            }

            var result = Convert.ToInt32(cmd.ExecuteScalar().ToString());
            ObjIn.GetType().GetPrimaryKeyProperty().SetValue(ObjIn,(int)result);

            return ObjIn;
        }

        protected static Dictionary<Attributes.SqlColumn, PropertyInfo> GetSqlProperties(Type type)
        {
            Dictionary<Attributes.SqlColumn, PropertyInfo> SqlColumns = new Dictionary<Attributes.SqlColumn, PropertyInfo>();
            var props = type.GetProperties();

            foreach (var prop in props)
            {
                var attr = (prop.GetCustomAttribute(typeof(Attributes.SqlColumn), true) as Attributes.SqlColumn);
                if (attr != null) SqlColumns.Add(attr, prop);

            }
            return SqlColumns;
        }
    }
}
