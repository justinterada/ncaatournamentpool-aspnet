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
		CommonMethods.resetDraft(Server);
		Response.Redirect("default.aspx");
    }
    
    void Page_Load()
    {
		
    	WebControl button;
		button = (WebControl)clearDraft;
		
		button.Attributes.Add("onclick", "return confirm (\"Are you sure you want to reset the draft? \");");
    }
    
  
</script>
<html>
<html>
<head>
    <title>Clear Draft</title>
    <style type="text/css"> @import url(style.css);</style>
</head>
<body>
    <div id="mainbodynoleftbar">
        <h1>Clear Draft
        </h1>
        <form action = "cleardraft.aspx" runat="server">

			<asp:LinkButton id="clearDraft" Text="Reset Draft" OnClick="ClearDraft_Click" runat="server"></asp:LinkButton>

        </form>
    </div>
</body>
</html>
