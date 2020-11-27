using KlipTok.Api.Models;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(KlipTok.Api.Startup))]

namespace KlipTok.Api
{


	public class Startup : FunctionsStartup
	{
		public override void Configure(IFunctionsHostBuilder builder)
		{
			builder.Services.AddHttpClient();
			builder.Services.AddScoped<IAzureTableRepository, AllTheClipsRepository>();
			builder.Services.AddScoped<ClipCommentsRepository>();
			builder.Services.AddScoped<ClipLikesRepository>();
			builder.Services.AddScoped<TwitchClipRepository>();
		}

	}

}