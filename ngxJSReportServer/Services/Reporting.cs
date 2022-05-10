using jsreport.Client;
using jsreport.Types;
using ngxJSReportServer.Model;

namespace ngxJSReportServer.Services
{
    public static class Reporting
    {
        public static async Task<Stream> RenderJsReport(string body, QueryModel q, string ReportModel)
        {
            var rs = new ReportingService("http://localhost:5488");
            
            IList<Script> scripts = new List<Script>();
            scripts.Add(new Script {
                Content = BootStrapScript("jsreport", "jsreport", "localhost", "TFH_SVIL", QueryService.GetQuery(q), ReportModel),
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
                        MarginTop = "2cm",
                        MarginLeft = "2cm",
                        MarginRight = "2cm",
                        MarginBottom = "2cm"
                    },
                    PdfOperations = new List<PdfOperation>()
                    {
                       
                         new PdfOperation()
                        {
                            Type = PdfOperationType.Merge,
                            Template = new Template
                            {
                                Name="TollHostHeader",
                                Engine = Engine.Handlebars,
                                Recipe = Recipe.ChromePdf
                            }
                        }                        
                    },
                    
                }
            });
            return r.Content;
        }
        static string BootStrapScript(string username, string password, string server, string database, string query, string reportModel)
        {
            var x = $@"
            const sql = require('mssql');
            {{#asset TollHostFunc.js @encoding=utf8}}
            const config = {{
                'user': '{username}',
                'password': '{password}',
                'server': '{server}',
                'database': '{database}',
                'options': {{
                    'trustedConnection': true,
                    'encrypt': false,
                    'trustServerCertificate': true,
                  }},
            }}

            async function beforeRender(req, res) {{
                await sql.connect(config)
                const sqlReq = new sql.Request();
                const dbdata = await sqlReq.query(`{query}`)
                const model = JSON.parse('{reportModel}')
                req.data.groups = BootStrap(model, dbdata.recordset);
            }}";
            return x;
        }

    }


}
