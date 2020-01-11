using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Heuristic
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            double x = Math.Log(7.05, 2);
            Response.Write(x.ToString() + "<br>");
            
            double y = Math.Log(30542.57, 2);
            Response.Write(y.ToString() + "<br>");

            double z = Math.Log(13.56, 2);
            Response.Write(z.ToString() + "<br>");

            double a = Math.Log(1145346.5, 2);
            Response.Write(a.ToString() + "<br>");

        }
    }
}
