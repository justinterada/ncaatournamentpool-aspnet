using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace NcaaTourneyPool.Controls
{
    public partial class Matchup : System.Web.UI.UserControl
    {
        public PlaceHolder Team1PlaceHolder
        {
            get { return team1; }
        }

        public PlaceHolder Team2PlaceHolder
        {
            get { return team2; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}