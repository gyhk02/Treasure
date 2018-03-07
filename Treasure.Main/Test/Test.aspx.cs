using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Treasure.Model.General;

namespace Treasure.Main.Test
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack == false)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(GeneralVO.id);
                dt.Columns.Add(GeneralVO.no);
                dt.Columns.Add(GeneralVO.name);

                for (int idx = 1; idx < 10; idx++)
                {
                    DataRow row = dt.NewRow();
                    row[GeneralVO.id] = idx;
                    row[GeneralVO.no] = idx + "NO";
                    row[GeneralVO.name] = idx + "NAME";
                    dt.Rows.Add(row);
                }

                grv1.DataSource = dt;
                grv1.DataBind();
            }
        }
        
    }
}