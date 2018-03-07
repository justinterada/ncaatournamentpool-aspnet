<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChooseTeams2.aspx.cs" Inherits="NcaaTourneyPool.ChooseTeams2" %>
<%@ import Namespace="NcaaTourneyPool" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Choose Your Teams: Round <%= CurrentStatus.round %></title>
    <link rel="StyleSheet" href="css/default.css" type="text/css" />
    <link rel="StyleSheet" href="css/custom-theme/jquery-ui-1.8.10.custom.css" type="text/css" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, minimum-scale=1, user-scalable=no" />
    <script language="javascript" src="js/CommonMethods.js" type="text/javascript"></script>
    <script language="javascript" src="js/ChooseTeams.js" type="text/javascript"></script>
    <script language="javascript" src="js/jquery-1.5.1.min.js" type="text/javascript"></script>
    <script language="javascript" src="js/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <style type="text/css">
    ul.matchup li.selected
    {
    	background-color: <%= Players[UserId].color %>;
    }
    </style>
</head>

<body>
<script type="text/javascript">
<!--
    $(document).ready(onReady);

    currentStatus.currentRound = <%= JsonHelper.Serialize<Round>(CurrentRound) %>;
    currentStatus.currentPlayer = <%= JsonHelper.Serialize<Player>(CurrentPlayer) %>;
    currentStatus.pointsAvailable = currentStatus.currentRound.pointsToSpend + currentStatus.currentPlayer.pointsHeldOver;

    var allTeams = <%= JsonHelper.Serialize<Team[]>(AllTeamsArray) %>;
-->
</script>
    <form id="form1" runat="server">
        <h1 id="topLevelHeader">Choose Your Teams: Round <%= CurrentStatus.round%></h1>
        <asp:HiddenField ID="hiddenSelectedTeams" runat="server" />
        <div id="fixedTop" style="top: 0px; width: 100%; background-color: White;">
        <div id="fixedTopStaticSection">
        <div id="infoPanel"><span id="spanPointsRemaining" style="font-weight: bold; font-size: 16pt; border: solid 2px Black; background-color: Black; color: White; display: inline-block; width: 30px; height: 24px; vertical-align: middle; text-align: center;"></span>
            <span style="vertical-align: middle;">Round <%= CurrentStatus.round %>, maximum hold over: <span id="span1" style="font-weight: bold"><%= CurrentRound.maxHoldOverPoints %></span>
            <button style="margin: 0px;" id="buttonSelectBeforeConfirm" onclick="return confirmSelect();">Select</button></span>
            <asp:Button ID="buttonSubmit" runat="server" style="display:none;" />
        </div>
        <div id="fourPanelRadioSection"><input type="radio" id="bracketOneRadio" name="fourPanelRadio" checked="checked"/><label for="bracketOneRadio" style="width:25%;"><%= BracketOneName %></label><input type="radio" id="bracketFourRadio" name="fourPanelRadio"/><label for="bracketFourRadio" style="width:25%;"><%= BracketFourName %></label><input type="radio" id="bracketTwoRadio" name="fourPanelRadio"/><label for="bracketTwoRadio" style="width:25%;"><%= BracketTwoName %></label><input type="radio" id="bracketThreeRadio" name="fourPanelRadio"/><label for="bracketThreeRadio" style="width:25%;"><%= BracketThreeName %></label></div>
        <div id="twoPanelRadioSection"><input type="radio" id="bracketOneAndFourRadio" name="twoPanelRadio" checked="checked" /><label for="bracketOneAndFourRadio" style="width:50%;"><%= BracketOneName %> and <%= BracketFourName %></label><input type="radio" id="bracketTwoAndThreeRadio" name="twoPanelRadio" /><label for="bracketTwoAndThreeRadio" style="width:50%;"><%= BracketTwoName %> and <%= BracketThreeName %></label></div>
        </div>
        <div id="divError" class="ui-state-error ui-corner-all" style="display: none; padding: .7em">
        </div>
        </div>
        <div id="fixedTopPlaceholder" style="display:none;">
        </div>  
        <div id="brackets1and4">
            <div id="bracket1">
            <h2 id="bracket1Header"><%= BracketOneName %></h2>
                <asp:PlaceHolder ID="placeholderBracket1" runat="server" />
            </div>
            <div id="bracket4">
                <h2 id="bracket4Header"><%= BracketFourName %></h2>
                <asp:PlaceHolder ID="placeholderBracket4" runat="server" />
            </div>
        </div>
        
        <div id="brackets2and3">
            <div id="bracket2">
                <h2 id="bracket2Header"><%= BracketTwoName %></h2>
                <asp:PlaceHolder ID="placeholderBracket2" runat="server" />
            </div>
            <div id="bracket3">
                <h2 id="bracket3Header"><%= BracketThreeName %></h2>
                <asp:PlaceHolder ID="placeholderBracket3" runat="server" />
            </div>
        </div>
        
        <div id="dialog-confirm" title="Are you sure?" style="display: none;">
	        <p><span class="ui-icon ui-icon-alert" style="float:left; margin:0 7px 20px 0;"></span>Are you sure you want to select these teams?</p>
        </div>
    </form>
</body>
</html>
