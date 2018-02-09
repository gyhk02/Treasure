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
                dt.Columns.Add(GeneralVO.Id);
                dt.Columns.Add(GeneralVO.No);
                dt.Columns.Add(GeneralVO.Name);

                for (int idx = 1; idx < 10; idx++)
                {
                    DataRow row = dt.NewRow();
                    row[GeneralVO.Id] = idx;
                    row[GeneralVO.No] = idx + "NO";
                    row[GeneralVO.Name] = idx + "NAME";
                    dt.Rows.Add(row);
                }

                grv1.DataSource = dt;
                grv1.DataBind();
            }
        }
        
    }
}