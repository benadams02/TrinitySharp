namespace TrinitySharp
{
    public class Attributes : Attribute
    {
        [AttributeUsage(AttributeTargets.Property)]
        public class SqlColumn : Attribute
        {
            private string fieldName;
            private System.Data.SqlDbType sqlDbType;
            private int maxFieldSize;
            private bool primaryKey;
            private string _propertyName;

            public SqlColumn(string FieldName, System.Data.SqlDbType SqlDbType)
            {
                this.FieldName = FieldName;
                this.SqlDbType = SqlDbType;
                this.MaxFieldSize = 999;
                this.PrimaryKey = false;
                this.PropertyName = FieldName;
            }
            public SqlColumn(string FieldName, System.Data.SqlDbType SqlDbType, int MaxFieldSize)
            {
                this.FieldName = FieldName;
                this.SqlDbType = SqlDbType;
                this.MaxFieldSize = MaxFieldSize;
                this.PrimaryKey = false;
                this.PropertyName = FieldName;
            }
            public SqlColumn(string FieldName, System.Data.SqlDbType SqlDbType, int MaxFieldSize, bool PrimaryKey)
            {
                this.FieldName = FieldName;
                this.SqlDbType = SqlDbType;
                this.MaxFieldSize = MaxFieldSize;
                this.PrimaryKey = PrimaryKey;
                this.PropertyName = FieldName;
            }
            public SqlColumn(string FieldName, System.Data.SqlDbType SqlDbType, int MaxFieldSize, bool PrimaryKey, string PropertyName)
            {
                this.FieldName = FieldName;
                this.SqlDbType = SqlDbType;
                this.MaxFieldSize = MaxFieldSize;
                this.PrimaryKey = PrimaryKey;
                this.PropertyName = PropertyName;
            }
            public int MaxFieldSize
            {
                get { return maxFieldSize; }
                set { maxFieldSize = value; }
            }

            public string FieldName
            {
                get { return fieldName; }
                set { fieldName = value; }
            }

            public System.Data.SqlDbType SqlDbType
            {
                get { return sqlDbType; }
                set { sqlDbType = value; }
            }

            public bool PrimaryKey
            {
                get { return primaryKey; }
                set { primaryKey = value; }
            }

            public string PropertyName
            {
                get { return _propertyName; }
                set { _propertyName = value; }
            }
        }

        [AttributeUsage(AttributeTargets.Class)]
        public class SqlTable : Attribute
        {
            private string tableName;
            private string getSP;
            private string insertSP;
            private string deleteSP;
            private string updateSP;
            public SqlTable(string TableName, string GetSP = "", string InsertSP = "", string UpdateSP = "", string DeleteSP = "")
            {
                this.TableName = TableName;
                this.GetSP = GetSP;
                this.InsertSP = InsertSP;
                this.UpdateSP = UpdateSP;
                this.DeleteSP = DeleteSP;
            }

            public string TableName
            {
                get { return tableName; }
                set { tableName = value; }
            }

            public string GetSP
            {
                get { return getSP; }
                set { getSP = value; }
            }
            public string InsertSP
            {
                get { return insertSP; }
                set { insertSP = value; }
            }
            public string UpdateSP
            {
                get { return updateSP; }
                set { updateSP = value; }
            }
            public string DeleteSP
            {
                get { return deleteSP; }
                set { deleteSP = value; }
            }
        }

        [AttributeUsage(AttributeTargets.Class)]
        public class ApiInfo : Attribute
        {
            private string _endpoint;
            private string _PKName;

            public ApiInfo(string endpoint, string primaryKeyName)
            {
                Endpoint = endpoint;
                PrimaryKeyName = primaryKeyName;
            }

            public string Endpoint
            {
                get { return _endpoint; }
                set { _endpoint = value; }
            }
            public string PrimaryKeyName
            {
                get { return _PKName; }
                set { _PKName = value; }
            }

        }

    }
}
