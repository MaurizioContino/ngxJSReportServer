
using Newtonsoft.Json.Serialization;

const string corspolicies = "CORSPolicies";

var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

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
