using Audit.Core;
using Audit.Core.Providers;
using Audit.SqlServer.Providers;
using Audit.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System.Configuration;
using System.Reflection;
using System.Text.Json;
using WebApplication1.Data;
using WebApplication1.UOW;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            Audit.Core.Configuration.Setup()
                .UseSqlServer(config => config
                .ConnectionString(builder.Configuration.GetConnectionString("DefaultConnection"))
                .TableName("AuditLogs")
                .IdColumnName("Id")
                .CustomColumn("EventType", ev => ev.EventType)
                .CustomColumn("UserId", ev => ev.Environment.UserName)
                .CustomColumn("RequestPath", ev => ev.GetWebApiAuditAction()?.RequestUrl)
                .CustomColumn("RequestMethod", ev => ev.GetWebApiAuditAction()?.HttpMethod)
                .CustomColumn("RequestBody", ev => ev.GetWebApiAuditAction()?.ActionParameters != null ? JsonSerializer.Serialize(ev.GetWebApiAuditAction().ActionParameters) : null)
                .CustomColumn("ResponseBody", ev => ev.GetWebApiAuditAction()?.ResponseBody)
                .CustomColumn("EventDate", ev => DateTime.Now));


            ConfigureLogging(builder.Configuration,builder.Environment);
            builder.Host.UseSerilog();
            // Add services to the container. 
            builder.Services.AddDbContext<BikeStoresDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<AuditApiAttribute>(); 
            });
            builder.Services.AddScoped<AuditApiAttribute>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();

            app.UseMiddleware<LoggingMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        private static void ConfigureLogging(IConfiguration configuration, IHostEnvironment environment)
        {
            var env = environment.EnvironmentName;

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.MSSqlServer(configuration.GetConnectionString("DefaultConnection"), tableName: "Logs", autoCreateSqlTable: true)
                .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, env))
                .Enrich.WithProperty("Environment", env)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        private static ElasticsearchSinkOptions ConfigureElasticSink(IConfiguration configuration, string environment)
        {
            var elasticUri = configuration["ElasticConfiguration:Uri"];
            return new ElasticsearchSinkOptions(new Uri(elasticUri))
            {
                AutoRegisterTemplate = true,
                IndexFormat = "SLog"
            };
        }
    }
}


//$"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}" (old index format)