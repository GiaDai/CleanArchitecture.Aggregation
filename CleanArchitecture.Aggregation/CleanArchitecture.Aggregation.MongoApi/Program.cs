using CleanArchitecture.Aggregation.MongoApi.Interfaces;
using CleanArchitecture.Aggregation.MongoApi.Services;
using CleanArchitecture.Aggregation.MongoApi.Settings;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);
var _service = builder.Services;
var _config = builder.Configuration;
var _env = builder.Environment;

_service.Configure<SchoolDatabaseSettings>(settings => _config.GetSection(nameof(SchoolDatabaseSettings)).Bind(settings));
_service.AddSingleton<IMongoClient, MongoClient>(provider =>
{
    var connectionString = _config.GetSection("SchoolDatabaseSettings:ConnectionString").Value;
    return new MongoClient(connectionString);
});

_service.AddSingleton<IStudentService, StudentService>();
_service.AddSingleton<ICourseService, CourseService>();
// Add services to the container.

_service.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
_service.AddEndpointsApiExplorer();
_service.AddSwaggerGen();

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
