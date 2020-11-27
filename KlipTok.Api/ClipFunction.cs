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

		[FunctionName("GetClips")]
		public async Task<IActionResult> Run(
				[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
				ILogger log)
		{

			var userId = long.Parse("0" + req.Query?["twitchuserid"].ToString());

			var results = await _Repo.GetClips(userId);
			return new OkObjectResult(results);

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
