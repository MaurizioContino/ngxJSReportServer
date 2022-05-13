using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ngxJSReportServer.Model;
using ngxJSReportServer.Services;

namespace ngxJSReportServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Queries : ControllerBase
    {
        
        [HttpPost]
        public object TableList(QueryModel q)
        {
            string query = QueryService.GetQuery(q);
            var auth = SessionManager.getAuth(q.SessionId);
            return DBService.ExecuteQuery(query, auth);

        }

        [HttpPost("query")]
        public string PrepareQuery(QueryModel q)
        {
            var auth = SessionManager.getAuth(q.SessionId);
            return QueryService.GetQuery(q);
        }


    }
}
