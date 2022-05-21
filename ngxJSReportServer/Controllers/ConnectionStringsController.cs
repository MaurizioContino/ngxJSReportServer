using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ngxJSReportServer.ApiModels;
using ngxJSReportServer.Common;
using ngxJSReportServer.DataAccess;
using ngxJSReportServer.Model;
using ngxJSReportServer.Services;

namespace ngxJSReportServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ConnectionStrings : ControllerBase
    {

        AppDBContext db;

        public ConnectionStrings(AppDBContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public async Task<List<DbConnectionReqRes>> Get()
        {
            return await db.Connections.Select(v=> new DbConnectionReqRes() { Name=v.Name, Description = v.Description }).ToListAsync();
        }


        [HttpPost]
        public async Task<List<DbConnectionReqRes>> Post(DbConnectionReqRes item)
        {
            SQLAuthModel connection = null;
            if (item.Id.HasValue)
            {
                connection = await db.Connections.Where(v => v.SQLAuthModelId == item.Id.Value).FirstOrDefaultAsync();
                connection.Name = item.Name;
                connection.Description = item.Description;
                connection.ConnectionString = Encription.Encrypt(item.ConnectionString);
                db.Connections.Update(connection);
               
            }
            else
            {
                connection = new SQLAuthModel() { Name = item.Name, Description = item.Description, ConnectionString = Encription.Encrypt(item.ConnectionString) };
                await db.Connections.AddAsync(connection);
            }
            await db.SaveChangesAsync();
            return await Get();
        }


    }
}
