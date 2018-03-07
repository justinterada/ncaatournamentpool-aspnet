<%@ Page Language="C#" %>
<%@ import Namespace="NcaaTourneyPool" %>
<%@ import Namespace="System.IO" %>
<%@ import Namespace="System.Net" %>
<%@ import Namespace="System.Collections.Specialized" %>
<%@ import Namespace="System.Data" %>
<%@ import Namespace="System.Data.OleDb" %>
<script runat="server">
bool isEditing = false;
DataTable teamTable;
DataTable selectedTeamTable;
CurrentStatus nowStatus;
Round[] theRounds;
Player[] thePlayers;
int userId;


    void Page_Load(Object s, EventArgs e)
    {
		userId = Convert.ToInt32(Request.QueryString["userId"]);

		if(!IsPostBack)
		{
			nowStatus = CommonMethods.loadCurrentStatus();
			theRounds = CommonMethods.loadRoundsForLobby();
			thePlayers = CommonMethods.loadPlayersForLobby();
		}
    }




</script>
<html>
<head>
    <title>Who are you?</title>
    <link rel="StyleSheet" href="css/default.css" type="text/css" />
    <link rel="StyleSheet" href="css/custom-theme/jquery-ui-1.8.10.custom.css" type="text/css" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, minimum-scale=1, user-scalable=no" />
    <script language="javascript" src="js/jquery-1.5.1.min.js" type="text/javascript"></script>
    <script language="javascript" src="js/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script language="javascript" src="js/CommonMethods.js" type="text/javascript"></script>
    <script type="text/javascript">
    <!--
        $(document).ready(function() {
            if (commonMethods.isMobileDevice()) {
                $("a").css("width", "100%");
            }
            else {
                $("a").css("width", "250px");
            }
            
            $("a").button();
        });
    -->
    </script>
</head>
<body>
    <h1>Who are you?</h1>
    
    <% 
		foreach (Player thisPlayer in thePlayers)
		{
			Response.Write("<a href = \"lobby.aspx?userid=" + thisPlayer.userId + "\" style=\"display: block; margin: 5px 5px 5px 0px;\">");
			Response.Write(thisPlayer.firstName + " " + thisPlayer.lastName + "</a>");
		}
    %>
    </body>
</html>
