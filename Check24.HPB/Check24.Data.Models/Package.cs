using System.Collections;
using System.Collections.Generic;
using Check24.Contracts.Models;

namespace Check24.Data.Models
{
    public class Asset : IAsset
    {
        public int Id { get; set; }
        public double Price { get; set; }

        public List<IBid> Bids { get; set; }
        public string Name { get; set; }
        public List<IAsset> Package { get; set; }
        public Asset()
        {
            Package = new List<IAsset>();
            Bids=new List<IBid>();
        }

       
    }

    //public class AssetNode : IEnumerable<AssetNode>
    //{
    //    private readonly Dictionary<int, AssetNode> childs = new Dictionary<int, AssetNode>();
    //    public int Id { get; set; }
    //    public double Price { get; set; }
    //    public AssetNode Parent { get; private set; }

    //    public AssetNode(int id)
    //    {
    //        this.Id = id;
    //    }

    //    public void Add(AssetNode item)
    //    {
    //        if (item.Parent != null)
    //        {
    //            item.Parent.childs.Remove(item.Id);
    //        }

    //        item.Parent = this;
    //        this.childs.Add(item.Id, item);
    //    }
    //    #region Implementation of IEnumerable

    //    /// <summary>
    //    /// Returns an enumerator that iterates through the collection.
    //    /// </summary>
    //    /// <returns>
    //    /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
    //    /// </returns>
    //    /// <filterpriority>1</filterpriority>
    //    public IEnumerator<AssetNode> GetEnumerator()
    //    {
    //        return this.childs.Values.GetEnumerator();
    //    }

    //    /// <summary>
    //    /// Returns an enumerator that iterates through a collection.
    //    /// </summary>
    //    /// <returns>
    //    /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
    //    /// </returns>
    //    /// <filterpriority>2</filterpriority>
    //    IEnumerator IEnumerable.GetEnumerator()
    //    {
    //        return GetEnumerator();
    //    }

    //    #endregion
    //}
}
