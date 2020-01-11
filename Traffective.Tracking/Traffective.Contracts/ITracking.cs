using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Traffective.Contracts
{
    /// <summary>
    /// Tracking entity to store data about view and click tracking
    /// </summary>
    public interface ITracking : IContentBase
    {
        int AdvertiserId { get; set; }
        int PublisherId { get; set; }
        
        long ViewTime { get; set; }
        string Referer { get; set; }
        string FingerPrintId { get; set; }
        string MarketingChannel { get; set; }
        string TouchPoint { get; set; }
        int AdvertisementId { get; set; }
        string IP { get; set; }
    }
}
