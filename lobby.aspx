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
			// CommonMethods.clearCart(Server);
			nowStatus = CommonMethods.loadCurrentStatus();
			theRounds = CommonMethods.loadRoundsForLobby();
			thePlayers = CommonMethods.loadPlayersForLobby();
		}
    }




</script>
<html>
    <head>
	    <script language="javascript" src="opengal.js" type="text/javascript"></script>
        <title>Lobby</title>
        <link rel="StyleSheet" href="css/default.css" type="text/css" />
        <link rel="StyleSheet" href="css/custom-theme/jquery-ui-1.8.10.custom.css" type="text/css" />
        <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, minimum-scale=1, user-scalable=no" />
        <script language="javascript" src="js/jquery-1.5.1.min.js" type="text/javascript"></script>
        <script language="javascript" src="js/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
        <script language="javascript" src="js/CommonMethods.js" type="text/javascript"></script>
        <script language="javascript" src="js/Lobby.js" type="text/javascript"></script>
    </head>
    <script type="text/javascript">
    <!--
        $(document).ready(onReady);
    -->
    </script>
    <body>
        <h1>Lobby</h1>
	    <p><a id="logOutButton" href="Default.aspx">Log out</a></p>
        
        <% 
        if(!nowStatus.finished)
        {
            Response.Write("<div id=\"roundWrapper\">");
			foreach (Round thisRound in theRounds)
			{
				Response.Write(string.Format("<div id=\"round{0}\" class=\"{1}\">", nowStatus.round, (thisRound.roundNumber == nowStatus.round) ? "round currentRound" : "round"));
				Response.Write("<h2>Round " + thisRound.roundNumber + "</h2>");
				Response.Write("<span class=\"roundDescription\">");
				Response.Write("Points to Spend: " + thisRound.pointsToSpend + "<br />");
				Response.Write("Maximum Hold Over: " + thisRound.maxHoldOverPoints);
				Response.Write("</span>");
                Response.Write("<ol class=\"roundOrder\">");
                int countRounds = 0;
				foreach (int m in thisRound.roundOrder)
					countRounds++;
				for(int n = 0; n < countRounds; n++)
				{
					if(n == nowStatus.currentOrderIndex && thisRound.roundNumber == nowStatus.round) 
						Response.Write("<li class = \"currentPlayer\">");
					else Response.Write("<li>");
					Response.Write(thePlayers[thisRound.roundOrder[n]-1].firstName + " " + thePlayers[thisRound.roundOrder[n]-1].lastName);
					Response.Write("</li>");
				}
				Response.Write("</ol>");
				Response.Write("</div>");
	        }

            Response.Write("</div>");

            Response.Write("<br style=\"clear: both;\" />");
            
	        if(userId == thePlayers[theRounds[nowStatus.round-1].roundOrder[nowStatus.currentOrderIndex]-1].userId)
			{
				Response.Write("<p><a id=\"chooseTeamsLink\" href = \"chooseteams2.aspx?userId=" + userId + "\">It's Your Turn!  Click Here to Choose Teams!</a></p>");
			}
	    }
	    else
	    {
			Response.Write("<p>This draft has been completed.</p>");
			Response.Write("<h3>Entries into Final Lottery</h3>");
			Response.Write("<table class = \"lotteryTable\">");
			Response.Write("<tr><th>Player</th><th>Entries</th></tr>");
			foreach (Player thisPlayer in thePlayers)
			{
				Response.Write("<tr><td>");
				Response.Write(thisPlayer.firstName + " " + thisPlayer.lastName + "</td><td>" + (thisPlayer.pointsHeldOver / 2).ToString() + "</td></tr>");
			}
			Response.Write("</table>");
	    }
	    
	    %>
	<p><a href = "javascript:openGallery('bracketview.aspx',900,530)">Open bracket in new window</a></p>
	<h3>Log</h3>
	<!-- #include virtual='log.inc' -->
    </body>
</html>
