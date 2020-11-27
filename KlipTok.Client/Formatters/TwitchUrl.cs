using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KlipTok.Client.Formatters
{
	public class TwitchUrl
	{

		public static Uri GetLoginUrl(IConfiguration config, NavigationManager nav) {
			var clientId = config["twitch.clientid"];
			var thisUri = new Uri(nav.BaseUri);
			var port = thisUri.IsDefaultPort ? "" : (":" + thisUri.Port);
			var redirect = new Uri(new Uri(thisUri.Scheme + "://" + thisUri.Host + port), "/authorize");
			var scopes = "";
			var url = new Uri($"https://id.twitch.tv/oauth2/authorize?client_id={clientId}&redirect_uri={redirect}&response_type=code&scope={scopes}");

			return url;
		}

	}
}
