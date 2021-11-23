var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment;
var sharedFolder = Path.Combine(env.ContentRootPath, "..", "Configurations");

builder.Configuration
    .AddJsonFile(Path.Combine(sharedFolder, "appsettings.json"), true)
    .AddJsonFile(Path.Combine(sharedFolder, $"appsettings.{env.EnvironmentName}.json"), true)
    .AddJsonFile("appsettings.json", true)
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();