﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ngxJSReportServer.Model;
using ngxJSReportServer.Services;

namespace ngxJSReportServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Database : ControllerBase
    {
        
        [HttpGet]
        public List<TableModel> TableList()
        {
            return DBService.GetTables();
        }
        
        

    }
}
