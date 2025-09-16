using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NLog.Web;
using Template.API.HealthChecks;
using Template.API.Services;
using Template.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddMassTransit(x =>
{
    x.AddRider(rider =>
    {
        rider.UsingKafka((context, k) =>
        {
            k.Host(builder.Configuration["EventBusSettings:HostAddress"]!);
        });
    });
});

builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new OpenApiInfo { Title = "Template.API", Version = "v1" });
});

builder.Services.AddHealthChecks()
    .AddCheck<KafkaHealthCheck>("kafka")
    .AddDbContextCheck<DatabaseContext>();

builder.Host.UseNLog();
builder.Services.AddDbContext<DatabaseContext>(options=>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Account.API v1"));
}

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.UseRouting();

app.UseAuthorization();
app.MapControllers();

app.MapHealthChecks("/hc", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
