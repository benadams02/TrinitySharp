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
        protected List<PropertyInfo> editableProperties { get; set; }
        private SqlConnection Connection { get; set; } 
        public GenericRepository(SqlConnection sqlConnection)
        {
            thisType = typeof(ModelType);
            Connection = sqlConnection;
            sqlTableAttr = thisType.GetCustomAttributes(typeof(Attributes.SqlTable), true).FirstOrDefault() as Attributes.SqlTable;
            editableProperties = GetSqlProperties(thisType).Values.ToList();
        }

        public virtual bool Delete(int ID)
        {
            throw new NotImplementedException();
        }

        public virtual IQueryable<ModelType> GetAll()
        {
            

            if (sqlTableAttr != null)
            {
                Type thisType = typeof(ModelType);
                Dictionary<Attributes.SqlColumn, PropertyInfo> SqlColumns = GetSqlProperties(thisType);

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
            Dictionary<Attributes.SqlColumn, PropertyInfo> SqlColumns = GetSqlProperties(thisType);

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
            throw new NotImplementedException();
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
