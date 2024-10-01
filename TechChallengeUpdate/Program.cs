
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Prometheus;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Interface;
using Application.Applications.Interfaces;
using Application.Applications;
using TechChallengeUpdate.RabbitMq;
using FluentValidation;
using Core.Entities;
using Core.Validations;


var builder = WebApplication.CreateBuilder(args);


var configuration = builder.Configuration;
// Add services to the container.

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
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});



builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMq"));
builder.Services.AddScoped<IContactApplication, ContactApplication>();
builder.Services.AddSingleton<IHostedService, RabbitMqConsumer>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.MapGet("/", () => "microsserviço  de Atualizar contato está online");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var counter = Metrics.CreateCounter("TechChallengeUpdate", "Counts request to the metrics api endpoint",
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

//ApplyMigrationsIfNeeded(app);

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
