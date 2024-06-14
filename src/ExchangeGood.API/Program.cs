using ExchangeGood.API.Middleware;
using ExchangeGood.Service.Extensions;
using ExchangeGood.API.Extensions;
using ExchangeGood.DAO.Extensions;
using ExchangeGood.Repository.Extensions;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.IdentityServices();
builder.Services.AddRepositoryLayer();
builder.Services.AddServiceLayer();
builder.Services.AddDAOLayer(builder.Configuration);


var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await AppDbInitializer.Seed(app);
app.Run();
