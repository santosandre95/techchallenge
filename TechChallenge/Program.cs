using Microsoft.EntityFrameworkCore;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Interface;
using Application.Applications.Interfaces;
using Application.Applications;
using FluentValidation.AspNetCore;
using System.Reflection;
using Core.Entities;
using Core.Validations;
using FluentValidation;
using Prometheus;

using Microsoft.Data.SqlClient;

using TechChallengeApi.RabbitMqEvents;


var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

// Configura Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});


builder.Services.AddSingleton<RabbitMqEventBus>();


var connectionString = configuration.GetConnectionString("SqlConnection");
TestDatabaseConnection(connectionString);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString);
}, ServiceLifetime.Scoped);

builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IContactApplication, ContactApplication>();
builder.Services.AddScoped<IValidator<Contact>, ContactValidator>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMq"));
builder.Services.AddSingleton<RabbitMqEventBus>();



var app = builder.Build();

ApplyDatabaseMigrations(app);

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseCors("MyPolicy");
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));
}


//ApplyMigrationsIfNeeded(app);

var counter = Metrics.CreateCounter("TechChallengeApi", "Counts request to the metrics api endpoint",
    new CounterConfiguration
    {
        LabelNames = new[] { "method", "endpoint" }
    });

app.Use((context, next) =>
{
    counter.WithLabels(context.Request.Method, context.Request.Path).Inc();
    return next();
});

app.UseMetricServer();
app.UseHttpMetrics();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();

void TestDatabaseConnection(string connectionString)
{
    try
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            Console.WriteLine("Connection to the database was successful.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed to connect to the database: {ex.Message}");
    }
}

void ApplyDatabaseMigrations(WebApplication app)
{
    var migrationFlagFile = "/app/hasRunMigrations";

    if (!File.Exists(migrationFlagFile))
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
            File.Create(migrationFlagFile).Dispose();
        }
    }
}
