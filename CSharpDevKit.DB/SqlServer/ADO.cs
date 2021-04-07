using System;
using System.Data;
using System.Data.SqlClient;
namespace CSharpDevKit.DB.SqlServer
{
    public class ADO
    {
        string connStr;
        public ADO(string connStr)
        {
            this.connStr = connStr;
        }
        public DataTable Query(string queryString, params SqlParameter[]  parameters)
        {
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (var cmd = new SqlCommand(queryString, conn))
                {
                    cmd.Parameters.AddRange(parameters);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public int Exec(string queryString, params SqlParameter[] parameters)
        {
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (var cmd = new SqlCommand(queryString, conn))
                {
                    cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
