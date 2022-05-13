using jsreport.Client;
using jsreport.Types;
using ngxJSReportServer.Model;

namespace ngxJSReportServer.Services
{
    public static class Reporting
    {
        const string basetemplate = @"
<html>
<head>
    <meta content='text/html; charset=utf-8' http-equiv='Content-Type'>
    <link rel='stylesheet' href='https://cdnjs.cloudflare.com/ajax/libs/metro/4.1.5/css/metro.min.css'>
    <style>
        {{asset 'tollhost.css' 'utf8'}}
    </style>
</head>
<body>
  
    {{#*inline 'group'}}
    <table>
        {{#each key_values}}
        <tr>
            <td>{{label}}</td>
            <td>{{value}}</td>
        </tr>
        {{/each}}
    </table>
    {{#if this.rows.key_values}} {{>group}} {{else}}
    <tr>
      <td colspan='2'>  
        {{>items}}
      </td>
    </tr>
    {{/if}} {{/inline}} {{#*inline 'items'}}
    <table class='table striped'>
       <thead>
        <tr >
            {{#each this.fields}}
            <th>{{this.label}}</th>
            {{/each}}
        </tr>
       </thead>
       <tbody>
        {{#each rows}}
        <tr>
            {{#each ../fields}}
            <td style='width={{getwidth this}}; overflow: hidden'>{{getValue ../this this}}</td>
            {{/each}}
        </tr>
        {{/each}}
       </tbody>
    </table>

    {{/inline}} 
    {{#each groups}}
     {{{pdfCreatePagesGroup key}}}
     {{>group}} 
    
     {{/each}}
    
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

                var rr = new RenderRequest
                {
                    Template = new Template()
                    {
                        Content = basetemplate,
                        Scripts = scripts,
                        Engine = Engine.Handlebars,
                        Recipe = Recipe.ChromePdf,
                        Chrome = new Chrome()
                        {
                            //MarginTop = "0cm",
                            //MarginBottom = "0cm",
                            //MediaType = MediaType.Print,
                            //MarginLeft = "1cm",
                            //MarginRight = "1cm"
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
                                    Recipe = Recipe.ChromePdf,
                                    Chrome = new Chrome()
                                    {
                                        //MarginTop = "0cm",
                                        //MarginBottom = "0cm",
                                        //MediaType = MediaType.Print,
                                        //MarginLeft = "1cm",
                                        //MarginRight = "1cm"
                                    }
                                }
                            }
                        },

                    }
                };
                
                rr.Template.Chrome.MarginTop = "3cm";
                rr.Template.Chrome.MarginBottom = "1cm";
                rr.Template.Chrome.MediaType = MediaType.Print;
                rr.Template.Chrome.MarginLeft = "1cm";
                rr.Template.Chrome.MarginRight = "1cm";
                rr.Template.Chrome.Format = "A4";
                rr.Template.Chrome.Height = "16cm";
                Report r = await rs.RenderAsync(rr);
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
            }}";
            return x;
        }

    }


}
