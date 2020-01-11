using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using NHibernate.Bytecode.CodeDom;
using Traffective.Contracts;

namespace Traffective.Data
{
    /// <summary>
    /// Tracking entity to store data about view and click tracking
    /// </summary>
    [ActiveRecord]
    public class Tracking : ActiveRecordBase<Tracking>, ITracking
    {
        [PrimaryKey(Generator = PrimaryKeyType.Guid)]
        public Guid Id { get; set; }
        [Property]
        public DateTime CreatedTimeStamp { get; set; }
        [Property]
        public DateTime UpdatedTimeStamp { get; set; }

        #region Implementation of ITracking
        [Property]
        public int AdvertiserId { get; set; }
        [Property]
        public int PublisherId { get; set; }
        [Property]
        public long ViewTime { get; set; }
        [Property]
        public string Referer { get; set; }
        [Property]
        public string FingerPrintId { get; set; }
        [Property]
        public string MarketingChannel { get; set; }
        [Property]
        public string TouchPoint { get; set; }
        [Property]
        public int AdvertisementId { get; set; }
        [Property]
        public string IP { get; set; }

        #endregion
    }
}
