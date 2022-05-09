using jsreport.Client;
using jsreport.Types;
using ngxJSReportServer.Model;

namespace ngxJSReportServer.Services
{
    public static class Reporting
    {
        public static async Task<Stream> RenderJsReport(string body, string header, QueryModel q)
        {
            var rs = new ReportingService("http://localhost:5488");
            
            IList<Script> scripts = new List<Script>();
            scripts.Add(new Script {
                Content = GenerateSqlScript("jsreport", "jsreport", "localhost", "TFH_SVIL", QueryService.GetQuery(q)),
            });

            Report r = await rs.RenderAsync(new RenderRequest
            {
                Template = new Template()
                {
                    Content = body,
                    Scripts = scripts,
                    Engine = Engine.Handlebars,
                    Recipe = Recipe.ChromePdf,
                    Chrome = new Chrome
                    {
                        MarginTop = "2cm"
                    },
                    PdfOperations = new List<PdfOperation>()
                    {
                        new PdfOperation()
                        {
                            Type = PdfOperationType.Merge,
                            Template = new Template
                            {
                                Content = header,
                                Engine = Engine.None,
                                Recipe = Recipe.ChromePdf
                            }
                        }
                    },
                    
                }
            });
            return r.Content;
        }
        static string GenerateSqlScript(string username, string password, string server, string database, string query)
        {
            return $@"
            const sql = require('mssql');
            const config = {{
                'user': '{username}',
                'password': '{password}',
                'server': '{server}',
                'database': '{database}',
                'stream': 'false',
                'options': {{
                    'trustedConnection': true,
                    'encrypt': false,
                    'trustServerCertificate': true,
                  }},
            }}

            async function beforeRender(req, res) {{
                await sql.connect(config)
                const sqlReq = new sql.Request();
                const recordset = await sqlReq.query(`{query}`)
                Object.assign(req.data, {{ data: recordset }});
            }}";

        }

    }


}
