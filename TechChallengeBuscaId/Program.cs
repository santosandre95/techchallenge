using Application.Applications.Interfaces;
using Application.Applications;
using Core.Entities;
using Core.Validations;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Interface;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection;
using Microsoft.Identity.Client;
using Prometheus;


var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

var connectionString = configuration.GetConnectionString("SqlConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString);
}, ServiceLifetime.Scoped);

builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IContactApplication, ContactApplication>();
builder.Services.AddScoped<IValidator<Contact>, ContactValidator>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));
}



var counter = Prometheus.Metrics.CreateCounter("TechChallengeBuscaId", "Counts request to the metrics api endpoint",
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
