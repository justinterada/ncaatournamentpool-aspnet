<%@ Page Language="c#" CodeBehind="bracketview.aspx.cs" Inherits="NcaaTourneyPool.bracketview" AutoEventWireup="false" %>
<%@ import Namespace="NcaaTourneyPool" %>
<%@ import Namespace="System.IO" %>
<%@ import Namespace="System.Net" %>
<%@ import Namespace="System.Collections.Specialized" %>
<%@ import Namespace="System.Data" %>
<%@ import Namespace="System.Data.OleDb" %>
<html>
<head>
    <title>Bracket View - Hit F5 to Refresh</title>
    <LINK REL=StyleSheet HREF="style.css" TYPE="text/css">
        <script type = "text/javascript">
    <!--
    <% 
    if (Request.QueryString["Refresh"] != "Off")
    {
    %>
		setTimeout('document.location.reload();', 30000);
    <%
    }
    %>

    -->
    </script>
</head>
<body>
<table>
	<tr>
		<td>
<table class="firstRoundTable">
	<th><%= bracketOneName %></th>
	<%= CreateTableRowsFor64TeamBracket(bracketOneTeams) %>
</table>
		</td>
        <td>
<table class="firstRoundTable">
	<th><%= bracketFourName %></th>
	<%= CreateTableRowsFor64TeamBracket(bracketFourTeams) %>
</table>
		</td>
		<td>
<table class="firstRoundTable">
<th><%= bracketThreeName %></th>
	<%= CreateTableRowsFor64TeamBracket(bracketThreeTeams) %>
</table>
		</td>
        <td>
<table class="firstRoundTable">
	<th><%= bracketTwoName %></th>
	<%= CreateTableRowsFor64TeamBracket(bracketTwoTeams) %>
</table>
		</td>
	</tr>
<tr>
	<td colspan="4" align="center">
<table class="legendTable"><tr>
<% foreach (Player thisplayer in thePlayers)
{
	Response.Write("<td bgcolor = \"" + thisplayer.color + "\">" + thisplayer.firstName + " " + thisplayer.lastName + "</td>");
}  %>
</tr></table>
	</td>
</tr>
</table>
</body>
</html>