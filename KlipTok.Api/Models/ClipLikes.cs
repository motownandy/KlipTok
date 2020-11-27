using Microsoft.Azure.Cosmos.Table;

namespace KlipTok.Api.Models
{
	public class ClipLikes : TableEntity {

		public string ClipSlug { 
			get { return PartitionKey; }
			set { PartitionKey = value; }
		}

		public string TwitchId {
			get { return RowKey; }
			set { RowKey = value; }
		}

	}

}
