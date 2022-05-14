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
    <link rel='stylesheet' href='https://cdn.metroui.org.ua/v4/css/metro-all.min.css'>
    <style>
        {{asset '/Sinelec/Shared/tollhost.css' 'utf8'}}
    </style>
</head>
<body>
  
    {{#*inline 'group'}}
    <div class='group-header'>
            {{#each key_values}}
            <div class='group-label'>{{label}}:</div> <div class='group-label'>{{value}}</div>
            
            {{/each}}
    </div>
    {{#if this.rows.key_values}} {{>group}} {{else}}
    
    <div class='details-container'>
        {{>items}}
    </div>
    {{/if}} {{/inline}} {{#*inline 'items'}}
    <table class='table compact row-border'>
       <thead>
        <tr>
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

    <h1 class='title'>{{title.Title}}</h1>

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
                            MarginTop = "3cm",
                            MarginBottom = "2cm",
                            MediaType = MediaType.Print,
                            MarginLeft = "1cm",
                            MarginRight = "1cm"
                        },
                        PdfOperations = new List<PdfOperation>()
                        {

                             new PdfOperation()
                            {
                                Template = new Template
                                {
                                    Name = "TollHostHeader",
                                    Recipe = Recipe.ChromePdf,
                                    Engine = Engine.Handlebars,
                                    Chrome = new Chrome()
                                    {
                                        MarginTop = "1cm",
                                        MarginBottom = "1cm",
                                        MediaType = MediaType.Print,
                                        MarginLeft = "1cm",
                                        MarginRight = "1cm"
                                    },
                                },
                                Type = PdfOperationType.Merge,
                                MergeWholeDocument = true
                            }
                        },

                    }
                };
                
               
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
