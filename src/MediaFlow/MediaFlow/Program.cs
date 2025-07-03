using BmwNewsBot.Scraper;
using MediaFlow.Clients;
using MediaFlow.Clients.Interfaces;
using MediaFlow.Services;
using MediaFlow.Storage.EF;
using MediaFlow.Storage.Interfaces;
using MediaFlow.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace MediaFlow
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureServices((hostContext, services) =>
				{
					var config = hostContext.Configuration;

					services.AddHttpClient();
					// Register a named HttpClient with proxy
					//services.AddHttpClient("WithProxy")
					//			.ConfigurePrimaryHttpMessageHandler(() =>
							
					//				var proxy = new WebProxy("if_any")
					//				{
					//					UseDefaultCredentials = true
					//				};

					//				return new HttpClientHandler
					//				{
					//					Proxy = proxy,
					//					UseProxy = true,
					//					UseDefaultCredentials = true
					//				};
					//			});

					// EF
					services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=items.db"));

					services.AddScoped(typeof(IRepository<>), typeof(Repository<>));


					// Register Scrapers
					services.AddSingleton<DidYouKnowsScraper>();

					// Register Storage
					//services.AddSingleton<IProcessedItemStore, ProcessedItemStore>();

					// Register AI Chat Client
					services.AddSingleton<IAiChatClient, ChatGptClient>();

					// Register AutoPosterService as the background service
					services.AddHostedService<MediaFlowService>();
				})
				.ConfigureLogging(logging =>
				{
					logging.ClearProviders();
					logging.AddConsole();
				});
	}
}
