using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Filters;
using SeuTempo.Application.Interfaces;
using SeuTempo.Application.Services;

namespace SeuTempo.API.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static void AddLog(this WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .Filter.ByExcluding(Matching.FromSource("System"))
                .Filter.ByExcluding(Matching.FromSource("Microsoft"))
                .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore"))
                .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.StaticFiles"))
                .Filter.ByExcluding(x => x.MessageTemplate.Text.Contains("Busines error"))
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ApplicationName", "API Serilog - SeuTempo")
                .WriteTo.Async(cons => cons.Console(outputTemplate: "{Timestamp:G} [{Level:u3}] ({SourceContext}) {Message:lj}{NewLine}{Exception}}"))
                .WriteTo.Async(txt => txt.File(
                        path: Environment.GetEnvironmentVariable("CAMINHO_LOG"),
                        outputTemplate: "{Timestamp:G} [{Level}] ({SourceContext}) {Message:lj}{NewLine}{Exception}}",
                        rollingInterval: RollingInterval.Day))
                .CreateLogger();

            builder.Host.UseSerilog(Log.Logger);
        }

        public static void AddHttpClient(this WebApplicationBuilder builder)
        {
            var aplicacao = builder.Configuration["ApiConfigService:NomeRequest"];
            var baseUrl = Environment.GetEnvironmentVariable("BASE_URL");
            var timeOut = Double.Parse(builder.Configuration["ApiConfigService:TimeOut"]);

            builder.Services.AddHttpClient(aplicacao, x =>
            {
                x.BaseAddress = new Uri(baseUrl);
                x.DefaultRequestHeaders.Add("Accept", "application/json");
                x.Timeout = TimeSpan.FromSeconds(timeOut);
            });

            builder.Services.AddHttpClient();
        }

        public static void AddVariaveis(this WebApplicationBuilder builder)
        {
            var apiConfig = builder.Configuration.GetSection(nameof(ApiConfigService));
            builder.Services.Configure<ApiConfigService>(apiConfig);
            builder.Services.AddSingleton<IApiConfigService>(x =>
                x.GetRequiredService<IOptions<ApiConfigService>>().Value);
        }
    }
}
