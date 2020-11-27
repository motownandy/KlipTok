using KlipTok.Twitch.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KlipTok.Api.Models
{
	public interface IAzureTableRepository {

		Task AddClip(Clip clip);

		Task<IEnumerable<Clip>> GetClips(long twitchId);

		Task AddLike();

		Task AddComment(long twitchId, string twitchDisplayName, string parentCommentId);

	}
}
