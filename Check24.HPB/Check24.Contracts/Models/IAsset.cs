using System.Collections.Generic;

namespace Check24.Contracts.Models
{
    public interface IAsset
    {
        int Id { get; set; }
        double Price { get; set; }
        List<IBid> Bids { get; set; }
        List<IAsset> Package { get; set; }
        string Name { get; set; }
        
    }
}