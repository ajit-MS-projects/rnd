// Type: MvcSiteMapProvider.Web.Html.SiteMapPathHelper
// Assembly: MvcSiteMapProvider, Version=3.2.3.0, Culture=neutral, PublicKeyToken=1923abe4657913cc
// Assembly location: D:\ajit\rnd\MVC\MVCTest1\packages\MvcSiteMapProvider.3.2.3.0\lib\net40\MvcSiteMapProvider.dll

using MvcSiteMapProvider;
using MvcSiteMapProvider.Web.Html.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace MvcSiteMapProvider.Web.Html
{
  /// <summary>
  /// MvcSiteMapHtmlHelper extension methods
  /// 
  /// </summary>
  public static class SiteMapPathHelper
  {
    /// <summary>
    /// Source metadata
    /// 
    /// </summary>
    private static Dictionary<string, object> SourceMetadata = new Dictionary<string, object>()
    {
      {
        "HtmlHelper",
        (object) typeof (SiteMapPathHelper).FullName
      }
    };

    static SiteMapPathHelper()
    {
    }

    /// <summary>
    /// Gets SiteMap path for the current request
    /// 
    /// </summary>
    /// <param name="helper">MvcSiteMapHtmlHelper instance</param>
    /// <returns>
    /// SiteMap path for the current request
    /// </returns>
    public static MvcHtmlString SiteMapPath(this MvcSiteMapHtmlHelper helper)
    {
      return SiteMapPathHelper.SiteMapPath(helper, (string) null);
    }

    /// <summary>
    /// Gets SiteMap path for the current request
    /// 
    /// </summary>
    /// <param name="helper">MvcSiteMapHtmlHelper instance</param><param name="templateName">Name of the template.</param>
    /// <returns>
    /// SiteMap path for the current request
    /// </returns>
    public static MvcHtmlString SiteMapPath(this MvcSiteMapHtmlHelper helper, string templateName)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SiteMapPathHelper.\u003C\u003Ec__DisplayClass0 cDisplayClass0 = new SiteMapPathHelper.\u003C\u003Ec__DisplayClass0();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass0.model = SiteMapPathHelper.BuildModel(helper, helper.Provider.CurrentNode);
      // ISSUE: reference to a compiler-generated field
      HtmlHelper<SiteMapPathHelperModel> htmlHelperForModel = helper.CreateHtmlHelperForModel<SiteMapPathHelperModel>(cDisplayClass0.model);
      ParameterExpression parameterExpression = Expression.Parameter(typeof (SiteMapPathHelperModel), "m");
      // ISSUE: field reference
      Expression<Func<SiteMapPathHelperModel, SiteMapPathHelperModel>> expression = Expression.Lambda<Func<SiteMapPathHelperModel, SiteMapPathHelperModel>>((Expression) Expression.Field((Expression) Expression.Constant((object) cDisplayClass0), FieldInfo.GetFieldFromHandle(__fieldref (SiteMapPathHelper.\u003C\u003Ec__DisplayClass0.model))), new ParameterExpression[1]
      {
        parameterExpression
      });
      string templateName1 = templateName;
      return DisplayExtensions.DisplayFor<SiteMapPathHelperModel, SiteMapPathHelperModel>(htmlHelperForModel, expression, templateName1);
    }

    /// <summary>
    /// Builds the model.
    /// 
    /// </summary>
    /// <param name="helper">The helper.</param><param name="startingNode">The starting node.</param>
    /// <returns>
    /// The model.
    /// </returns>
    private static SiteMapPathHelperModel BuildModel(MvcSiteMapHtmlHelper helper, SiteMapNode startingNode)
    {
      SiteMapPathHelperModel mapPathHelperModel = new SiteMapPathHelperModel();
      for (SiteMapNode node = startingNode; node != null; node = node.ParentNode)
      {
        MvcSiteMapNode mvcNode = node as MvcSiteMapNode;
        bool flag = true;
        if (mvcNode != null)
          flag = mvcNode.VisibilityProvider.IsVisible(node, HttpContext.Current, (IDictionary<string, object>) SiteMapPathHelper.SourceMetadata);
        if (flag && node.IsAccessibleToUser(HttpContext.Current))
        {
          SiteMapNodeModel siteMapNodeModel = SiteMapNodeModelMapper.MapToSiteMapNodeModel(node, mvcNode, (IDictionary<string, object>) SiteMapPathHelper.SourceMetadata);
          mapPathHelperModel.Nodes.Add(siteMapNodeModel);
        }
      }
      mapPathHelperModel.Nodes.Reverse();
      return mapPathHelperModel;
    }
  }
}
