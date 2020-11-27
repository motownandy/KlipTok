using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
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
		private readonly HttpClient _Client;
		static string TwitchClientId = System.Environment.GetEnvironmentVariable("twitchclientid");


		public ClipFunction(IAzureTableRepository repo, IHttpClientFactory httpClientFactory)
		{
			_Repo = repo;
			_Client = httpClientFactory.CreateClient();
			_Client.DefaultRequestHeaders.Add("Client-Id", TwitchClientId);

			if (SecurityFunction.AppAccessToken == null) {
				SecurityFunction.GetAppAccessToken(httpClientFactory.CreateClient()).GetAwaiter().GetResult();
			}
			_Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {SecurityFunction.AppAccessToken.access_token}");

		}

		//[FunctionName("LoadClips")]
		//public async Task<IActionResult> LoadClips([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
		//		ILogger log)
		//{

		//	var clips = new Clip[] {
		//		new Clip {
		//			ChannelName = "ElliFace",
		//			Title = "ElliFace announces CSharpFritz made Twitch Partner!",
		//			TwitchClipSlug="DistinctNiceScallionPicoMause",
		//			TwitchUserId = 32402099
		//		},
		//		new Clip {
		//			ChannelName="Instafluff",
		//			TwitchClipSlug = "AggressivePoorTigerSoBayed",
		//			Title="Instafluff immitates CsharpFritz",
		//			TwitchUserId = 83118047
		//		},
		//		new Clip {
		//			ChannelName = "ChefBrent",
		//			TwitchClipSlug="SpoopyOriginalPenguinWoofer",
		//			Title="You just need one hat",
		//			TwitchUserId=50912385
		//		},
		//		new Clip {
		//			ChannelName = "csharpfritz",
		//			TwitchClipSlug = "ExpensivePoorHerbsRitzMitz",
		//			Title = "Mint in Box",
		//			TwitchUserId = 96909659
		//		},
		//		new Clip {
		//			ChannelName = "csharpfritz",
		//			TwitchClipSlug = "ProductiveHilariousOctopusPeteZarollTie",
		//			Title = "A test of the Fritz emergency broadcast system",
		//			TwitchUserId = 96909659
		//		}
		//	};

		//	foreach (var clip in clips)
		//	{
		//		await _Repo.AddClip(clip);
		//	}

		//	return new OkResult();

		//}				


		[FunctionName("GetClips")]
		public async Task<IActionResult> Run(
				[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
				ILogger log)
		{

			var userId = long.Parse("0" + req.Query?["twitchuserid"].ToString());
			var filterUserId = req.Query["filter"];

			var results = await _Repo.GetClips(userId, filterUserId);

			results = (await GetClipViews(results)).OrderByDescending(r => r.Likes / (double)r.Views);

			return new OkObjectResult(results);

		}

		private async Task<IEnumerable<Clip>> GetClipViews(IEnumerable<Clip> results)
		{

			if (!results.Any()) return results;

			var ids = results.Select(r => $"&id={r.TwitchClipSlug}").ToArray();
			var part = string.Join(' ', ids).Replace(" ", "").Substring(1);

			var resultString = await _Client.GetStringAsync($"https://api.twitch.tv/helix/clips?{part}");
			var twitchResults = JsonSerializer.Deserialize<TwitchClipData>(resultString).data.OrderBy(d => d.id);

			return results.OrderBy(r => r.TwitchClipSlug).Zip(twitchResults, (clip, record) => new Clip
			{
				ChannelName = clip.ChannelName,
				CommentCount = clip.CommentCount,
				IsLikedByMe = clip.IsLikedByMe,
				Likes = clip.Likes,
				ThumbnailUrl = record.thumbnail_url,
				Title =clip.Title,
				TwitchClipSlug = clip.TwitchClipSlug,
				TwitchUserId = clip.TwitchUserId,
				Views = record.view_count
			}).ToArray();

		}

		[FunctionName("LikeClip")]
		public async Task<IActionResult> LikeClip(
			[HttpTrigger(AuthorizationLevel.Anonymous, "post", Route =null)] HttpRequest req, ILogger log
		) {

			var clipSlug = req.Form["clipslug"].ToString();
			var userId = req.Form["userid"].ToString();
			var addLike = bool.Parse(req.Form["addlike"]);

			await _Repo.AddLike(clipSlug, userId, addLike);

			return new OkResult();

		}
	}
}
