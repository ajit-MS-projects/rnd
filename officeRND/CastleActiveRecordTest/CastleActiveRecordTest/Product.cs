using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.ActiveRecord;

namespace CastleActiveRecordTest
{
	[ActiveRecord]
	public class Product : ActiveRecordBase<Product>
	{
		[PrimaryKey]
		public int Id { get; set; }

		[Property]
		public string ArticleNumber { get; set; }

		[Property]
		public string Title { get; set; }

		[Property]
		public float Price { get; set; }

	}
}