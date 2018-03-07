<%@ Page Language="c#" CodeBehind="chooseteams.aspx.cs" Inherits="NcaaTourneyPool.chooseteams" AutoEventWireup="true" %>
<%@ import Namespace="NcaaTourneyPool" %>
<%@ import Namespace="System.IO" %>
<%@ import Namespace="System.Net" %>
<%@ import Namespace="System.Collections.Specialized" %>
<%@ import Namespace="System.Data" %>
<%@ import Namespace="System.Data.OleDb" %>
<html>
<head>
	<script language="javascript" src="opengal.js" type="text/javascript"></script>
	<script type = "text/javascript">
	
		function onCheckTeam(checkBoxName, pointsAvailableName, teamValue)
		{
			var chkBx = document.getElementById(checkBoxName);
			var ptsAvail = document.getElementById(pointsAvailableName);
			
			if (chkBx.checked)
			{
				ptsAvail.value = parseInt(ptsAvail.value) - teamValue;
			}
			else
			{
				ptsAvail.value = parseInt(ptsAvail.value) + teamValue;
			}
		}
	
	</script>
    <title>Choose Your Teams: Round <%= nowStatus.round %></title>
    <link rel="StyleSheet" href="style.css" type="text/css" />
</head>
<body>
    <div id="mainbodynoleftbar">
        <h1>Choose Your Teams: Round <%= nowStatus.round %>
        </h1>
<form runat="server">
        <p><a runat = "server" href = "javascript:openGallery('bracketview.aspx',900,530)" ID="A1">Launch Bracket View (Hit F5 to refresh in bracket view)</a></p>

        <p>Points Available: <b><asp:TextBox id = "pointsAvailableLabel" runat = "server" CssClass = "DisabledBox" onFocus="blur();"  /></b></p>
        <p>Maximum Hold Over: <asp:TextBox id = "maxHoldOverLabel" runat = "server"   CssClass = "DisabledBox" onFocus="blur();"  /></p>
        <p><asp:CompareValidator ControlToValidate = "pointsAvailableLabel" id = "valPointsAvailable" runat = "server" Text = "Cannot hold over this many points." Display = "Dynamic" /><asp:CompareValidator ControlToValidate = "pointsAvailableLabel" ValueToCompare = "0" id = "valPointsLimit" Operator = "GreaterThanEqual" runat = "server" Text = "You cannot afford all these teams." Display = "Dynamic" Type = "Integer" /><asp:Label id = "errorMessageLabel" runat = "server" /></p>

        
        <p><asp:LinkButton id="LinkButton1" Text="Reset" OnClick="resetPage" runat="server" Visible = "False" ></asp:LinkButton></p>
        <p><asp:Button id="LinkButton2" Text="Submit" OnClick="submitPage" runat="server"></asp:Button></p>
<table><tr valign = "top"><td>
<div style = "overflow: auto; overflow-y: scroll; height: 350px; width: 450px; border: solid 2px #556d95;">
<asp:DataGrid CellPadding = "2" CellSpacing = "2" Gridlines = "None" CssClass="GridStyle1" id = "teamList" runat = "server" AutoGenerateColumns = "False">
<HeaderStyle CssClass="HeaderStyle" />
<ItemStyle CssClass="ItemStyle" />
<Columns>
	<asp:TemplateColumn HeaderText = "">
		<ItemStyle Width="16px" HorizontalAlign="Center" />
		<ItemTemplate>
			<asp:CheckBox id = "chkSelectTeam" runat = "server" />
		</ItemTemplate>
	</asp:TemplateColumn>
	<asp:BoundColumn HeaderText = "Bracket" DataField = "Bracket" ReadOnly = "true" />
	<asp:BoundColumn HeaderText = "Rank" DataField = "TeamRank" ReadOnly = "true"  />
	<asp:BoundColumn HeaderText = "School" DataField = "TeamName" ReadOnly = "true"  />
	<asp:BoundColumn HeaderText = "Wins" DataField = "TeamWins" ReadOnly = "true"  />
	<asp:BoundColumn HeaderText = "Losses" DataField = "TeamLosses" ReadOnly = "true"  />
	<asp:TemplateColumn
           HeaderText="Point Cost"
           Visible="True">
		<ItemTemplate>
			<b><asp:Label id = "pointCostAvailableTeams" runat = "server" Text = '<%# DataBinder.Eval(Container, "DataItem.Cost") %>' /></b>
		</ItemTemplate>
	</asp:TemplateColumn>
	<asp:TemplateColumn
           HeaderText="Point Cost"
           Visible="False">
		<ItemTemplate>
			<asp:Label id = "lblSchool" runat = "server" Text = '<%# DataBinder.Eval(Container, "DataItem.TeamName") %>' />
			<asp:Label id = "lblWins" runat = "server" Text = '<%# DataBinder.Eval(Container, "DataItem.TeamWins") %>' />
			<asp:Label id = "lblLosses" runat = "server" Text = '<%# DataBinder.Eval(Container, "DataItem.TeamLosses") %>' />
			<asp:Label id = "lblBracketId" runat = "server" Text = '<%# DataBinder.Eval(Container, "DataItem.BracketId") %>' />
			<asp:Label id = "lblBracketName" runat = "server" Text = '<%# DataBinder.Eval(Container, "DataItem.Bracket") %>' />
			<asp:Label id = "lblTeamRank" runat = "server" Text = '<%# DataBinder.Eval(Container, "DataItem.TeamRank") %>' />
			<asp:Label id = "lblSCurveRank" runat = "server" Text = '<%# DataBinder.Eval(Container, "DataItem.SCurveRank") %>' />
			<asp:Label id = "lblCost" runat = "server" Text = '<%# DataBinder.Eval(Container, "DataItem.Cost") %>' />
		</ItemTemplate>
	</asp:TemplateColumn>	
</Columns>
</asp:DataGrid>
</div>
</td></tr></table>
		</form>
    </div>
    </body>
</html>
