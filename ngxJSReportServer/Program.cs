
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using ngxJSReportServer.DataAccess;

const string corspolicies = "CORSPolicies";

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
    List<string> origins = new List<string>() { "http://localhost:4200"};

    /*
    var cfgOrigin = configuration.GetValue<string>("origins");
    if (cfgOrigin != null)
    {
        origins.AddRange(cfgOrigin.Split(";"));
    }
    */
    

    options.AddPolicy(name: corspolicies,
                      builder =>
                      {
                          builder.WithOrigins(origins.ToArray())
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                      });
});


builder.Services.AddControllers().AddNewtonsoftJson(opts =>
{
    opts.SerializerSettings.ContractResolver = new DefaultContractResolver();
    opts.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

string connString = configuration.GetConnectionString("AppConnection");

builder.Services.AddDbContextFactory<AppDBContext>(options => {
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    options.UseSqlServer(connString);
});

builder.Services.AddScoped<AppDBContext>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
    dbContext.Database.EnsureCreated();
    // use context
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(corspolicies);
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
    

app.Run();
