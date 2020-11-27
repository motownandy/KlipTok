using KlipTok.Twitch.Models;
using Microsoft.Azure.Cosmos.Table;
using System.Text;

namespace KlipTok.Api.Models
{

	public class TwitchClip : TableEntity
	{

		public string ChannelId {		
			get { return base.PartitionKey; }
			set { PartitionKey = value; }
		}

		public string ClipSlug { 
			get { return base.RowKey; }
			set { base.RowKey = value; }
		}

		public string Title { get; set; }

		public string ChannelName { get; set; }

		public Clip ToClip() {
			return new Clip
			{
				ChannelName = this.ChannelName,
				Title = this.Title,
				TwitchId = this.ClipSlug
			};
		}

		public static TwitchClip FromClip(Clip clip) {

			return new TwitchClip
			{
				ChannelId = clip.TwitchUserId.ToString(),
				ChannelName = clip.ChannelName,
				ClipSlug = clip.TwitchId,
				Title = clip.Title
			};

		}

	}

}
