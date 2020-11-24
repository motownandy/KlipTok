using System;
using System.Net.Http;

namespace ClipTok.Twitch
{
  public class ClipClient
  {
    private readonly HttpClient _Client;

    public ClipClient(IHttpClientFactory clientFactory)
    {

        _Client = clientFactory.CreateClient("Twitch.Clip");

    }

  }

}
