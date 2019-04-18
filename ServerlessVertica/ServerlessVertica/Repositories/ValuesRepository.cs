using System.Collections.Generic;
using System.Data.Odbc;

namespace ServerlessVertica.Repositories
{
    public class ValuesRepository : IValuesRepository
    {
        private readonly string connectionString;

        public ValuesRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IEnumerable<string> GetValues()
        {
            using (var con = new OdbcConnection(connectionString))
            {
                con.Open();
                using (var com = new OdbcCommand(@"SELECT Value FROM Values ", con))
                {
                    var reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        yield return reader["Value"].ToString();
                    }
                }
            }
        }
    }
}
