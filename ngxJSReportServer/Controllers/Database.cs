using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ngxJSReportServer.Model;
using ngxJSReportServer.Services;

namespace ngxJSReportServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Database : ControllerBase
    {
        
        [HttpPost]
        public DBModel TableList(SQLAuthModel Auth)
        {
            return DBService.GetTables(Auth);
        }
        
        

    }
}
