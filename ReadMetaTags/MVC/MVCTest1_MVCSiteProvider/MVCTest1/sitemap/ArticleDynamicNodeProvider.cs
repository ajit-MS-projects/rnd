using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcSiteMapProvider.Extensibility;

namespace MVCTest1.sitemap
{
    public class ArticleDynamicNodeProvider : DynamicNodeProviderBase
    {
        List<int>  articles = new List<int>(){1,2,3,4,5,66};
        public override IEnumerable<DynamicNode> GetDynamicNodeCollection()
        {
            foreach (var article in articles)
            {
                DynamicNode node = new DynamicNode();
                //node.Action = "Article";// here or from mvc.sitemap file
                //node.Controller = "Home";


                node.Title = "article " + article;
                node.RouteValues.Add("id", article);
                node.RouteValues.Add("ids", article+10);
                yield return node;
            }

        }
    }
}