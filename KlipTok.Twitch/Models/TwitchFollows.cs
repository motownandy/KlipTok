using System;
using System.Collections.Generic;
using System.Text;

namespace KlipTok.Twitch.Models
{


	public class FollowsOutput
	{
		public int total { get; set; }
		public FollowsRecord[] data { get; set; }
		public Pagination pagination { get; set; }
	}

	public class Pagination
	{
		public string cursor { get; set; }
	}

	public class FollowsRecord
	{
		public string from_id { get; set; }
		public string from_name { get; set; }
		public string to_id { get; set; }
		public string to_name { get; set; }
		public DateTime followed_at { get; set; }
	}

}
