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

		[FunctionName("GetClips")]
		public async Task<IActionResult> Run(
				[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
				ILogger log)
		{

			var userId = long.Parse("0" + req.Query?["twitchuserid"].ToString());

			var results = await _Repo.GetClips(userId);

			results = (await GetClipViews(results)).OrderByDescending(r => r.Views);

			return new OkObjectResult(results);

		}

		private async Task<IEnumerable<Clip>> GetClipViews(IEnumerable<Clip> results)
		{

			var ids = results.Select(r => $"&id={r.TwitchId}").ToArray();
			var part = string.Join(' ', ids).Replace(" ", "").Substring(1);

			var resultString = await _Client.GetStringAsync($"https://api.twitch.tv/helix/clips?{part}");
			var twitchResults = JsonSerializer.Deserialize<TwitchClipData>(resultString).data.OrderBy(d => d.id);

			return results.OrderBy(r => r.TwitchId).Zip(twitchResults, (clip, record) => new Clip
			{
				ChannelName = clip.ChannelName,
				CommentCount = clip.CommentCount,
				IsLikedByMe = clip.IsLikedByMe,
				Likes = clip.Likes,
				Title =clip.Title,
				TwitchId = clip.TwitchId,
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
