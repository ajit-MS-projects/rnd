using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;

namespace CastleActiveRecordTest
{
	[ActiveRecord]
	public class Report : ActiveRecordLinqBase<Report>
	{
		[PrimaryKey]
		public int Id { get; set; }

		[Property]
		public int AdvertiserId { get; set; }

		[Property]
		public DateTime Date { get; set; }

		[Property]
		public int Hour { get; set; }

		[Property]
		public long AdImpressions { get; set; }

		[Property]
		public long Clicks { get; set; }

		[Property]
		public double ClickRate { get; set; }

		[Property]
		public long Leads { get; set; }

		[Property]
		public double LeadRate { get; set; }

		[Property]
		public long PostViewLeads { get; set; }

		[Property]
		public long Sales { get; set; }

		[Property]
		public double SalesRate { get; set; }

		[Property]
		public long PostViewSales { get; set; }

		[Property]
		public long SalesValue { get; set; }

		[Property]
		public long TotalAffiliateCommission { get; set; }


	}
}