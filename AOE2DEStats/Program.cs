using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using AOE2DEStats.Services;
using Blazored.LocalStorage;

namespace AOE2DEStats
{
    public class Program
    {
        public IConfiguration Configuration;

        public Program(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.RootComponents.Add<App>("app");
            ConfigureServices(builder);

            var host = builder.Build();
            await host.RunAsync();
        }

        private static void ConfigureServices(WebAssemblyHostBuilder builder)
        {
            builder.Services.AddScoped<IStringServices, StringServices>();
            builder.Services.AddScoped<ILeaderboardService, LeaderboardServices>();
            builder.Services.AddScoped<IPlayerServices, PlayerServices>();
            builder.Services.AddScoped<IMatchServices, MatchServices>();
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddBlazoredLocalStorage();
        }
    }
}
