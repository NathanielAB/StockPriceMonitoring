using Newtonsoft.Json;
using StockPriceMonitoring.Alerts;
using StockPriceMonitoring.Alerts.Features.CreateAlert;
using StockPriceMonitoring.Alerts.Internals;
using StockPriceMonitoring.Alerts.Internals.Models;

var builder = WebApplication.CreateBuilder(args);

var converters = new List<JsonConverter> {
    new CreateAlertRequestConverter(),
    new AlertEntityConverter()
};

builder.Services
    .AddControllers()
    .AddNewtonsoftJson(options => options.SerializerSettings.Converters = [
    .. options.SerializerSettings.Converters,
    .. converters
]);

JsonConvert.DefaultSettings = () => new JsonSerializerSettings {
    Converters = converters,
};

builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddSingleton<IAlertChecker, AlertChecker>();
builder.Services.AddSingleton<INotificationManager, NotificationManager>();
builder.Services.AddHostedService<StockPricesRetrievalService>();

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
