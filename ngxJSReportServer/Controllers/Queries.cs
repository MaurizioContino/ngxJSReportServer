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
            return DBService.ExecuteQuery(query);

        }

        [HttpPost("query")]
        public string PrepareQuery(QueryModel q)
        {
            return QueryService.GetQuery(q);
        }


    }
}
