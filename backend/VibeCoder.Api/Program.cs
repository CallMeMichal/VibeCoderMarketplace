using VibeCoder.Application.Interfaces;
using VibeCoder.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Application layer DI
builder.Services.AddSingleton<ITitleGeneratorService, TitleGeneratorService>();
builder.Services.AddTransient<IProductCleaningService, ProductCleaningService>();
builder.Services.AddTransient<IExportService, ExportService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("Frontend");
app.MapControllers();

app.Run();
