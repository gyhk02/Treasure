using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Treasure.BLL.General
{
    public class BasicWebBLL : System.Web.UI.Page
    {
        public BasicWebBLL()
        {
            Session["UserId"] = "";
        }
    }
}
