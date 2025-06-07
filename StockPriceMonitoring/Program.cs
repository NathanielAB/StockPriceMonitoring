using Newtonsoft.Json;
using StockPriceMonitoring.Alerts.Features.CreateAlert;
using StockPriceMonitoring.Alerts.Internals;
using StockPriceMonitoring.Alerts.Internals.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddNewtonsoftJson(options => options.SerializerSettings.Converters = [
    .. options.SerializerSettings.Converters,
    new CreateAlertRequestConverter(),
    new AlertEntityConverter()
]);

builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddSingleton(_ => {
    var path = Path.Combine(AppContext.BaseDirectory, "Data", "UserAlertsData.json");
    return new AlertRepository(path);
});

builder.Services.AddSingleton<IAlertChecker, AlertChecker>();
builder.Services.AddSingleton<INotificationManager, NotificationManager>();
builder.Services.AddSingleton<IStockPriceFetcher, StockPriceFetcher>();
builder.Services.Decorate<IStockPriceFetcher, StockPriceFetcherCached>();
builder.Services.AddHostedService<StockPricesRetrievalService>();

builder.Services.AddMemoryCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
