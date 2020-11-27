using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;

namespace KlipTok.Api.Models
{
	public class ClipComments : TableEntity {

		public string ClipSlug
		{
			get { return PartitionKey; }
			set { PartitionKey = value; }
		}

		public string CommentId {
			get { return RowKey; }
		}

		public override IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
		{

			RowKey = !string.IsNullOrEmpty(RowKey) ? DateTime.UtcNow.ToString("s") + "_" + CommenterTwitchId : RowKey;

			return base.WriteEntity(operationContext);
		}

		public long CommenterTwitchId { get; set; }

		public string CommenterTwitchDisplayName { get; set; }

		public string ParentCommentId { get; set; }	

	}

}
