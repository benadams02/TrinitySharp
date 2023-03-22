using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrinitySharp
{
    public static partial class SqlConnectionExtensions
    {
        /// <summary>
        /// Attempts to open the SQL connection and return true if successful, false if unsuccessful.
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool TryOpenConnection(this SqlConnection sqlConnection)
        {
            if (sqlConnection == null) { throw new ArgumentNullException(nameof(sqlConnection)); }

            return OpenConnection(sqlConnection, 0);
        }

        private static bool OpenConnection(SqlConnection sqlConnection, int retryCount = 0)
        {
            if (retryCount <= 3)
            {
                if (sqlConnection.State == System.Data.ConnectionState.Closed)
                {
                    try
                    {
                        sqlConnection.Open();
                        if (sqlConnection.State == System.Data.ConnectionState.Open)
                        {
                            return true;
                        }
                        else
                        {
                            return OpenConnection(sqlConnection, retryCount++);
                        }
                    }
                    catch (Exception)
                    {
                        sqlConnection.Close();
                        OpenConnection(sqlConnection, retryCount++);
                        throw;
                    }
                }
                else
                {
                    sqlConnection.Close();
                    return OpenConnection(sqlConnection, retryCount++);
                }
            }
            else
            {
                return false;
            }
        }
    }
}
