using System;
using System.Linq;
using KlipTok.Twitch.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;


namespace KlipTok.Api
{
	public static class WeatherForecastFunction
	{
		[FunctionName("GetClips")]
		public static IActionResult Run(
				[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
				ILogger log)
		{

			return new OkObjectResult(new[] {
				new Clip() {
					TwitchId="SeductiveEvilDinosaurTheRinger",
					ChannelName="PhareEwings",
					Views=11300,
					Title="OMG It lights Up!",
					Likes=10
				},
				new Clip() {
					TwitchId="PoisedVastLettuceHeyGirl",
					ChannelName="PhareEwings",
					Views=11300,
					Title="If You're Happy and You Know It...",
					Likes=10
				},
				new Clip() {
					TwitchId="TangibleStrongNostrilKreygasm",
					ChannelName="PhareEwings",
					Views=11300,
					Title="Staff broke her <3",
					Likes=10
				},
			});

		}
	}
}
