using Application.Applications.Interfaces;
using Application.Applications;
using Core.Entities;
using Core.Validations;
using FluentValidation;
using Infrastructure.Repositories.Interface;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TechChallengeAdd.RabbitMqEvents;
using TechChallengeApi.RabbitMqEvents;
using Prometheus;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

var builder = WebApplication.CreateBuilder(args);


var configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


var connectionString = configuration.GetConnectionString("SqlConnection");

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

builder.Services.Configure<TechChallengeAdd.RabbitMqEvents.RabbitMqSettings>(configuration.GetSection("RabbitMq"));
builder.Services.AddSingleton<RabbitMqEventBus>();
builder.Services.AddSingleton<RabbitMqEventConsumer>();

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

ApplyMigrationsIfNeeded(app);



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


void ApplyMigrationsIfNeeded(IHost app)
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var dbExists = context.Database.GetService<IDatabaseCreator>() is RelationalDatabaseCreator databaseCreator && databaseCreator.Exists();

        if (!dbExists)
        {
            context.Database.Migrate();
        }
    }
}