using Microsoft.Data.SqlClient;
using ngxJSReportServer.Model;
using System.Data;

namespace ngxJSReportServer.Services
{
    public class DBService
    {

        public static List<DBTable> GetTables()
        {
            List<DBTable> tables = new List<DBTable>();
            using (SqlConnection conn = new SqlConnection("Server=(local);Initial Catalog=TFH_SVIL;Persist Security Info=False;Encrypt=true;Integrated Security=SSPI;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;"))
            {
                try
                {
                    conn.Open();

                    DataTable ts = conn.GetSchema("Tables");
                    DBTable curr = null;
                    String[] tableFilter = new String[4];
                    foreach (DataRow r in ts.Rows)
                    {
                        curr = new DBTable()
                        {
                            Name = r[2].ToString(),
                            OriginalName = r[2].ToString(),
                            TableType = "Table",
                            Fields = new()
                        };
                        tables.Add(curr);
                        tableFilter[2] = r[2].ToString();
                        DataTable fs = conn.GetSchema("Columns", tableFilter);
                        foreach (DataRow f in fs.Rows)
                        {
                            curr.Fields.Add(new DBField()
                            {
                                Name = f[3].ToString(),
                                OriginalName = f[3].ToString(),
                                FieldType = f[7].ToString(),
                                GroupType = "",
                                Parent = curr.Name,
                                Id = curr.Name + "." + f[3].ToString(),
                                Selected = false,
                                Size = f[8].ToString(),
                            });
                        }
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    conn.Close();
                }
            }
            return tables;
        }
    }
}
