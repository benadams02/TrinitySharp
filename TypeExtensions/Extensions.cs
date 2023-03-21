using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace TrinitySharp
{
    public static class Extensions
    {
        
    }
    public static partial class DateTimeExtensions
    {
        public static string ToISO8601Representation(this DateTime dateTime)
        {
            if (dateTime == null) { throw new ArgumentNullException(nameof(dateTime)); }

            return dateTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
        }
    }

    public static partial class StringExtensions
    {
        public static bool IsBase64String(this string stringIn)
        {
            if (stringIn == null) { throw new ArgumentNullException(nameof(stringIn)); }

            stringIn = stringIn.Trim();
            return (stringIn.Length % 4 == 0) && Regex.IsMatch(stringIn, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }
    }

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

            return OpenConnection(0, sqlConnection);
        }

        private static bool OpenConnection(int retryCount = 0, SqlConnection sqlConnection)
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
                            return OpenConnection(retryCount++, sqlConnection);
                        }
                    }
                    catch (Exception)
                    {
                        sqlConnection.Close();
                        OpenConnection(retryCount++, sqlConnection);
                        throw;
                    }
                }
                else
                {
                    sqlConnection.Close();
                    return OpenConnection(retryCount++, sqlConnection);
                }
            }
            else
            {
                return false;
            }
        }
    }

}
