using Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IWebHostEnvironment env = builder.Environment;
string sharedFolder = Path.Combine(env.ContentRootPath, "..", "Configurations");

builder.Configuration
    .AddJsonFile(Path.Combine(sharedFolder, "appsettings.json"), true)
    .AddJsonFile(Path.Combine(sharedFolder, $"appsettings.{env.EnvironmentName}.json"), true)
    .AddJsonFile("appsettings.json", true)
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMorder(builder.Configuration);
builder.Services.AddMemoryCache();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();