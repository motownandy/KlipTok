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

		public long TwitchUserId { get; set; }

		public int Views { get; set; }

		public string Title { get; set; }

		public long Likes { get; set; }

		public bool IsLikedByMe { get; set; }

		public int CommentCount { get; set; }


	}
}
