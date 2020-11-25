using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlipTok.Twitch.Models
{
	public class Clip
	{

		public string TwitchId { get; set; }

		public string ChannelName { get; set; }

		public int Followers { get; set; }

		public string Title { get; set; }

		public int UpVotes { get; set; }

		public int DownVotes { get; set; }

	}
}
