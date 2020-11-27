using System;
using System.Linq;
using System.Threading.Tasks;
using KlipTok.Api.Models;
using KlipTok.Twitch.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;


namespace KlipTok.Api
{
	public class ClipFunction
	{
		private readonly IAzureTableRepository _Repo;

		public ClipFunction(IAzureTableRepository repo)
		{
			_Repo = repo;
		}

		[FunctionName("LoadClips")]
		public async Task<IActionResult> LoadClips([HttpTrigger] HttpRequest req, ILogger logger)
		{

			var testClips = new[] {
				new Clip() {
					TwitchId = "SeductiveEvilDinosaurTheRinger",
					ChannelName = "PhareEwings",
					TwitchUserId = 102402222,
					Views = 11300,
					Title = "OMG It lights Up!",
					Likes = 10
				},
				new Clip() {
					TwitchId = "WiseAntsyVampireFloof",
					ChannelName = "csharpfritz",
					TwitchUserId = 96909659,
					Views = 100,
					Title = "Intro to KlipTok",
					Likes = 666
				},
				new Clip() {
					TwitchId = "PoisedVastLettuceHeyGirl",
					ChannelName = "PhareEwings",
					Views = 11300,
					TwitchUserId = 102402222,
					Title = "If You're Happy and You Know It...",
					Likes = 10
				},
				new Clip() {
					TwitchId = "HeadstrongPhilanthropicAmazonYouWHY",
					ChannelName = "Quiltoni",
					TwitchUserId = 145099717,
					Title = "Taught by Granite!"
				},
				new Clip() {
					TwitchId = "TangibleStrongNostrilKreygasm",
					ChannelName = "PhareEwings",
					TwitchUserId = 102402222,
					Views = 11300,
					Title = "Staff broke her <3",
					Likes = 10
				}
			};

			foreach (var clip in testClips)
			{
				await _Repo.AddClip(clip);
			}

			return new OkResult();

		}


		[FunctionName("GetClips")]
		public async Task<IActionResult> Run(
				[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
				ILogger log)
		{

			var userId = long.Parse("0" + req.Query?["twitchuserid"].ToString());

			var results = await _Repo.GetClips(userId);
			return new OkObjectResult(results);

			return new OkObjectResult(new[] {
				new Clip() {
					TwitchId="SeductiveEvilDinosaurTheRinger",
					ChannelName="PhareEwings",
					Views=11300,
					Title="OMG It lights Up!",
					Likes=10
				},
				new Clip() {
					TwitchId="WiseAntsyVampireFloof",
					ChannelName="csharpfritz",
					Views=100,
					Title="Intro to KlipTok",
					Likes=666
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
