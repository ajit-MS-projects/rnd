using System;
using Traffective.Contracts;

namespace Traffective.Tracking.Models
{
    /// <summary>
    /// Tracking entity to store data about view and click tracking
    /// </summary>
    public class Tracking : ITracking
    {
        public Guid Id { get; set; }
        public DateTime CreatedTimeStamp { get; set; }
        public DateTime UpdatedTimeStamp { get; set; }

        #region Implementation of ITracking

        public int AdvertiserId { get; set; }
        public int PublisherId { get; set; }
        public long ViewTime { get; set; }
        public string Referer { get; set; }
        public string FingerPrintId { get; set; }
        public string MarketingChannel { get; set; }
        public string TouchPoint { get; set; }
        public int AdvertisementId { get; set; }
        public string IP { get; set; }

        #endregion
    }
}
