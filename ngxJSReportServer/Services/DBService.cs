using Dapper;
using Microsoft.Data.SqlClient;
using ngxJSReportServer.Model;
using System.Data;

namespace ngxJSReportServer.Services
{
    public class DBService
    {

        public static List<TableModel> GetTables()
        {
            List<TableModel> tables = new List<TableModel>();
            using (SqlConnection conn = new SqlConnection("Server=10.86.1.103;Initial Catalog=TFH_SVIL;persist security info=True;User id=sa; Password=KPI2019!;trustServerCertificate=true"))
            {
                try
                {
                    conn.Open();

                    DataTable ts = conn.GetSchema("Tables");
                    TableModel curr = null;
                    String[] tableFilter = new String[4];
                    foreach (DataRow r in ts.Rows)
                    {
                        curr = new TableModel()
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
                            curr.Fields.Add(new FieldModel()
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


        public static object ExecuteQuery(string query)
        {
            using (SqlConnection conn = new SqlConnection("Server=10.86.1.103;Initial Catalog=TFH_SVIL;persist security info=True;User id=sa; Password=KPI2019!;trustServerCertificate=true"))
            {
                try
                {
                    conn.Open();
                    return conn.Query(query).AsList();
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
        }
    }
}
