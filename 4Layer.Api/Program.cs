using _4Layer.Api.Common.Helper;
using _4Layer.Api.Extensions;
using _4Layer.Infrastructure;
using _4Layer.Application;

var builder = WebApplication.CreateBuilder(args);

//Add Swagger
builder.Services.AddSwaggerSetup();
// Add services Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);
//Add services JsonCaseSetup
builder.Services.AddJsonCaseSetup();
// Add Json error catalog
builder.Services.AddSingleton<IErrorCatalog, ErrorCatalog>();
// Add application services
builder.Services.AddApplication();
//Add authentication and authorization
builder.Services.AddJwtAuthentication(builder.Configuration);
// Add controllers
builder.Services.AddControllers();
//Add services Validation
builder.Services.AddValidationSetup();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   app.SetUpUseSwagger();
}
app.UsePortSetup();
app.UseHttpsRedirection();
// Add custom middlewares
app.UseCustomMiddlewares();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();


