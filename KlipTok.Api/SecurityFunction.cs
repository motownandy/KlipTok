using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using KlipTok.Twitch.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace KlipTok.Api
{

	public class SecurityFunction
	{

		static string TwitchClientId = System.Environment.GetEnvironmentVariable("twitchclientid");
		static string TwitchSecret = System.Environment.GetEnvironmentVariable("twitchsecret");
		private readonly HttpClient _Client;

		public SecurityFunction(IHttpClientFactory clientFactory)
		{
			_Client = clientFactory.CreateClient();
		}

		[FunctionName("auth")]
		public async Task<IActionResult> Authenticate([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest request, ILogger log)
		{

			string code, uri;

			try
			{
				code = request.Query["twitchcode"].ToString();
				uri = request.Query["redirect_uri"].ToString();
			}
			catch (Exception ex)
			{
				log.LogError(ex, "Unable to fetch querystring parms");
				return new BadRequestObjectResult($"Fetching Querystring: {ex.Message}");
			}

			var targetUrl = $"https://id.twitch.tv/oauth2/token" +
					$"?client_id={TwitchClientId}" +
					$"&client_secret={TwitchSecret}" +
					$"&code={code}" +
					"&grant_type=authorization_code" +
					$"&redirect_uri={uri}";

			HttpResponseMessage results = null;

			try
			{
				results = await _Client.PostAsync(targetUrl, new StringContent(""));
			}
			catch (Exception ex)
			{
				log.LogError(ex, "Unable to get with HttpClient");
				return new BadRequestObjectResult($"Getting with HttpClient: {ex.Message}");
			}

			try
			{
				results.EnsureSuccessStatusCode();
			}
			catch (Exception ex)
			{
				log.LogError(ex, "Twitch reported bad status code");
				return new BadRequestObjectResult(new
				{
					Code = code,
					Uri = uri,
					Client = TwitchClientId,
					SecretLength = TwitchSecret.Length
				});
			}

			string outResults = "";

			try
			{
				outResults = await results.Content.ReadAsStringAsync();
			}
			catch (Exception ex)
			{
				log.LogError(ex, "Unable to read string content");
				return new BadRequestObjectResult($"Unable to read string content: {ex.Message}");
			}

			return new OkObjectResult(outResults);

		}

	}

}