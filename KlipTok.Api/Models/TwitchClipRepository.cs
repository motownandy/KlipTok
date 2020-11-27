using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KlipTok.Api.Models
{
	public class TwitchClipRepository : BaseTableRepository<TwitchClip>
	{

		public TwitchClipRepository(IConfiguration configuration) : base(configuration) { }

		public async Task<IEnumerable<TwitchClip>> GetAll() {

			var table = GetCloudTable(TableName);

			var query = new TableQuery<TwitchClip>
			{
				FilterString = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.NotEqual, "0")
			};

			TableContinuationToken token = null;
			var outList = new List<TwitchClip>();
			while (true)
			{
				var results = await table.ExecuteQuerySegmentedAsync<TwitchClip>(query.Take(10), token);
				if (results.Results.Count == 0) break;

				outList.AddRange(results.Results);

				if (results.ContinuationToken != null)
				{
					token = results.ContinuationToken;
				}
				else
				{
					break;
				}

			}

			return outList;


		}

	}

	public class ClipCommentsRepository : BaseTableRepository<ClipComments>
	{

		public ClipCommentsRepository(IConfiguration configuration) : base(configuration) { }

		public async Task<Dictionary<string, int>> GetCountOfComments(IEnumerable<string> clipIds) {

			var outDictionary = new Dictionary<string, int>();
			foreach (var item in clipIds)
			{
				var count = (await base.GetAllForPartition(item)).Count();
				outDictionary.Add(item, count);
			}

			return outDictionary;

		}

	}

	public class ClipLikesRepository : BaseTableRepository<ClipLikes>
	{

		public ClipLikesRepository(IConfiguration configuration) : base(configuration) { }

		public async Task<Dictionary<string, long>> GetCountOfLikes(IEnumerable<string> clipIds)
		{

			var outDictionary = new Dictionary<string, long>();
			foreach (var item in clipIds)
			{
				var count = (await base.GetAllForPartition(item)).Count();
				outDictionary.Add(item, count);
			}

			return outDictionary;

		}
	}

}
