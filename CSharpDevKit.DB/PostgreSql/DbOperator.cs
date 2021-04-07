using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Transactions;

namespace CSharpDevKit.DB.PostgreSql
{
    public class DbOperator
    {
        DbConnection[] dbConnections;
        TransactionScope scope;
        public event EventHandler<Exception> ExceptionOccurred;
        public DbOperator(params DbConnection[] dbConnections)
        {
            this.dbConnections = dbConnections;
        }
        public DataTable Query(string sql, params NpgsqlParameter[] parameters)
        {
            ProcessParameters(parameters);
            var tableList = new List<DataTable>();
            foreach (var connection in dbConnections)
            {
                using (var conn = new NpgsqlConnection(connection.ConnInfo))
                {
                    conn.Open();
                    // Retrieve all rows
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddRange(parameters.Select(s => s.Clone()).ToArray());
                        var adapter = new NpgsqlDataAdapter(cmd);
                        var dt = new DataTable();
                        adapter.Fill(dt);
                        tableList.Add(dt);
                        dt.Columns.Add("Server");
                        foreach (DataRow row in dt.Rows)
                        {
                            row["Server"] = connection.Name;
                        }
                    }
                }
            }
            return tableList.Aggregate((a, b) => { a.Merge(b); return a; });
        }
        public void Exec(string sql, params NpgsqlParameter[] parameters)
        {
            scope = scope ?? new TransactionScope();
            ProcessParameters(parameters);
            try
            {
                foreach (var connection in dbConnections)
                {
                    using (var conn = new NpgsqlConnection(connection.ConnInfo))
                    {
                        conn.Open();
                        using (var cmd = new NpgsqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddRange(parameters.Select(s => s.Clone()).ToArray());
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OnExceptionOccurred(ex);
            }
        }
        public void Complete()
        {
            scope?.Complete();
            scope?.Dispose();
        }

        private void ProcessParameters(NpgsqlParameter[] parameters)
        {
            foreach (var p in parameters)
            {
                if (p.Value == null)
                {
                    p.Value = DBNull.Value;
                }
            }
        }
        protected void OnExceptionOccurred(Exception ex)
        {
            Debug.Fail("exception occurred while executing sql statements", ex.Message);
            ExceptionOccurred?.Invoke(this, ex);
        }
    }
}
