<%@ Page Language="C#" %>
<%@ import Namespace="NcaaTourneyPool" %>
<%@ import Namespace="System.IO" %>
<%@ import Namespace="System.Net" %>
<%@ import Namespace="System.Collections.Specialized" %>
<%@ import Namespace="System.Data" %>
<%@ import Namespace="System.Data.OleDb" %>
<script runat="server">
bool isEditing;

    void ClearDraft_Click(Object s, EventArgs e)
    {
		CommonMethods.runEndOfDraftLottery();
		Response.Redirect("default.aspx");
    }
    
    void Page_Load()
    {
		
    	WebControl button;
		button = (WebControl)clearDraft;
		
		button.Attributes.Add("onclick", "return confirm (\"Are you sure you want to run the lottery? \");");
    }
    
  
</script>
<html>
<html>
<head>
    <title>End of Draft Lottery</title>
    <style type="text/css"> @import url(style.css);</style>
</head>
<body>
    <div id="mainbodynoleftbar">
        <h1>Run End of Draft Lottery
        </h1>
        <form action = "runlottery.aspx" runat="server">

			<asp:LinkButton id="clearDraft" Text="Run Lottery" OnClick="ClearDraft_Click" runat="server"></asp:LinkButton>

        </form>
    </div>
</body>
</html>
