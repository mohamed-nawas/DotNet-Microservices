using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Making use of http client factory
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// setup corresponding db server depending on host environment
if (builder.Environment.IsProduction())
{
    Console.WriteLine("Using SQL Server Database");
    builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConnectionSqlServer")));
}
else
{
    Console.WriteLine("Using InMemory Database");
    builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("InMemoryDb"));
}
builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();

var app = builder.Build();

// configuration from host environments
Console.WriteLine($"Command service endpoint: {app.Configuration["CommandService:BaseUrl"]}");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

DbSeeder.seedPopulation(app, app.Environment);

app.Run();
