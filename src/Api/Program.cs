using AutoMapper;
using Infrastructure;
using Infrastructure.Mappings;
using Infrastructure.MediatR.Companies.Commands;
using Infrastructure.MediatR.Companies.Handlers;
using MediatR;

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
builder.Services.AddMContext(builder.Configuration);
builder.Services.AddMediatR(typeof(UpdateCompanyInformationHandler).Assembly);
builder.Services.AddTransient<IRequestHandler<UpdateCompanyInformation, bool>,
    UpdateCompanyInformationHandler>();
var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new CompanyProfile()); });

IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

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