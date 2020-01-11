using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ReadMetaTags
{
	public partial class WebForm2 : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			LimitedQueue<string> qq = new LimitedQueue<string>(3);
			Queue<string> q = new Queue<string>(3);
			qq.Enqueue("A");
			qq.Enqueue("B");
			qq.Enqueue("C");
			qq.Enqueue("D");
			List<string> l = qq.ToList();

			foreach (var v in l)
			{
				Response.Write(v+"<br/>");
			}
			foreach (var v in qq)
			{
				Response.Write(v + "<br/>");
			}
		}
	}
}