using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpDevKit.Utilities
{
    public static class Miscellany
    {
        public static byte[] ConvertDataTableToCSVBytes(DataTable dt, Encoding encoding)
        {
            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in dt.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                sb.AppendLine(string.Join(",", fields));
            }

            var ms = new MemoryStream();
            var bytes = encoding.GetBytes(sb.ToString());
            return bytes;
        }

        public static void InvokeCmd(string[] commands, out string output,out string error)
        {
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo("cmd.exe")
            {
                RedirectStandardInput = true,
                UseShellExecute = false,
                RedirectStandardOutput=true,
                RedirectStandardError=true,
                CreateNoWindow=true,
            };
            p.Start();
            foreach (var i in commands)
                p.StandardInput.WriteLine(i);
            p.StandardInput.WriteLine("exit");
            output = p.StandardOutput.ReadToEnd();
            error = p.StandardError.ReadToEnd();
            p.Close();
        }
    }
}
