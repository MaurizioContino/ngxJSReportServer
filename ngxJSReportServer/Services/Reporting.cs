using jsreport.Client;
using jsreport.Types;
using ngxJSReportServer.Model;

namespace ngxJSReportServer.Services
{
    public static class Reporting
    {
        const string basetemplate = @"
        <html>
            <body style='padding:20px'>
                <table>
  
                {{#each groups}}
                  {{#each key_values}}
                    <tr>
                        <td>{{label}}</td>
                        <td>{{value}}</td>
                    </tr>
                  {{/each}}
                {{/each}}
                </table>

                <table>
                            {{#each rows}}

                        <tr>
                            {{#each ../fields}}
                                <td>{{getValue ../this this}}</td>
                            {{/each}}
                        </tr>
                            {{/each}}

                </table>
            </body>
        </html>
";

        public static async Task<Stream> RenderJsReport(string body, QueryModel q, string ReportModel)
        {
            var rs = new ReportingService("http://localhost:5488");
            var auth = SessionManager.getAuth(q.SessionId);
            try
            {
                IList<Script> scripts = new List<Script>();
                
                scripts.Add(new Script
                {
                    Content = BootStrapScript(auth.UserName!, auth.Password!, auth.Server!, auth.Database!, QueryService.GetQuery(q), ReportModel),
                });


                Report r = await rs.RenderAsync(new RenderRequest
                {
                    Template = new Template()
                    {
                        Content = basetemplate,
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
            catch(Exception ex)
            {
                throw ;
            }
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
                req.data.groups = BootStrap(model, dbdata.recordset);
                req.data.fields = model.Details.Fields
                req.data.rows = req.data.groups[0].rows 
            }}";
            return x;
        }

    }


}
