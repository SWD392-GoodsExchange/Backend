using ExchangeGood.API.Middleware;
using ExchangeGood.Service.Extensions;
using ExchangeGood.API.Extensions;
using ExchangeGood.DAO.Extensions;
using ExchangeGood.Repository.Extensions;
using ExchangeGood.API.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.IdentityServices();
builder.Services.AddRepositoryLayer();
builder.Services.AddServiceLayer(builder.Configuration);
builder.Services.AddDAOLayer(builder.Configuration);


var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(builder => builder
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials() // to support a SignalR
    .WithOrigins("https://localhost:4200"));

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("hubs/notification");

await AppDbInitializer.Seed(app);
app.Run();
