﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FrontEndSite
{
    public partial class RecruitmentInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int id = Utilities.GetRequestParameter("Id");
            if (id <= 0)
            {
                Response.Redirect("~/Default.aspx", false);
            }

            ucNewsDetail.StartShowNewsDetail(id);
        }
    }
}