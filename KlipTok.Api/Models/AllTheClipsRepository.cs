using KlipTok.Twitch.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace KlipTok.Api.Models
{
	public class AllTheClipsRepository : IAzureTableRepository
	{
		private readonly TwitchClipRepository _ClipRepository;
		private readonly ClipCommentsRepository _CommentsRepository;
		private readonly ClipLikesRepository _LikesRepository;

		public AllTheClipsRepository(
			TwitchClipRepository clipRepository, 
			ClipCommentsRepository commentsRepository, 
			ClipLikesRepository likesRepository)
		{
			_ClipRepository = clipRepository;
			_CommentsRepository = commentsRepository;
			_LikesRepository = likesRepository;
		}

		public async Task AddClip(Clip clip)
		{

			await _ClipRepository.AddOrUpdate(TwitchClip.FromClip(clip));

		}

		public Task AddComment(long twitchId, string twitchDisplayName, string parentCommentId)
		{
			throw new NotImplementedException();
		}

		public async Task AddLike(string clipSlug, string userId, bool addLike)
		{
			
			if (addLike) {
				await _LikesRepository.AddOrUpdate(new ClipLikes
				{
					ClipSlug = clipSlug,
					TwitchId = userId
				});
			} else {
				var theLike = await _LikesRepository.Get(clipSlug, userId);
				await _LikesRepository.Remove(theLike);
			}


		}

		public async Task<IEnumerable<Clip>> GetClips(long twitchId, string filterUserId)
		{

			// TODO: Read from our algorithm

			Clip[] clips = new Clip[] { };
			if (string.IsNullOrEmpty(filterUserId))
			{
				clips = (await _ClipRepository.GetAll())
					.Select(t => t.ToClip()).ToArray();
			} else {
				clips = (await _ClipRepository.GetAllForPartition(filterUserId))
					.Select(t => t.ToClip()).ToArray();
			}

			var keys = clips.Select(c => c.TwitchClipSlug).ToArray();
			var commentCounts = await _CommentsRepository.GetCountOfComments(keys);
			var clipLikes = await _LikesRepository.GetCountOfLikes(keys, twitchId);

			foreach (var clip in clips)
			{

				clip.CommentCount = commentCounts[clip.TwitchClipSlug];
				clip.Likes = clipLikes[clip.TwitchClipSlug].count;
				clip.IsLikedByMe = clipLikes[clip.TwitchClipSlug].isPresent;

			}

			return clips;

		}
	}
}
