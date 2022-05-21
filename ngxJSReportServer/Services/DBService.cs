using Dapper;
using Microsoft.Data.SqlClient;
using ngxJSReportServer.Model;
using System.Data;
using ngxJSReportServer.Common;
namespace ngxJSReportServer.Services
{
    public class DBService
    {

        public static DBModel GetTables(SQLAuthModel Auth)
        {
            List<TableModel> tables = new List<TableModel>();
            var ret = new DBModel();
            
            using (SqlConnection conn = new SqlConnection( Encription.Decrypt(Auth.ConnectionString)))
            {
                try
                {
                    conn.Open();
                    ret.SessionId = SessionManager.createSession(Auth);
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
                                FieldId = curr.Name + "." + f[3].ToString(),
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
            ret.Tables = tables;
            return ret;
        }


        public static object ExecuteQuery(string query, SQLAuthModel Auth)
        {
            using (SqlConnection conn = new SqlConnection(Encription.Decrypt(Auth.ConnectionString)))
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
