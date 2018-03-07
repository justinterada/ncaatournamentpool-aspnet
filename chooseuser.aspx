<%@ Page Language="C#" %>
<%@ import Namespace="NcaaTourneyPool" %>
<%@ import Namespace="System.IO" %>
<%@ import Namespace="System.Net" %>
<%@ import Namespace="System.Collections.Specialized" %>
<%@ import Namespace="System.Data" %>
<%@ import Namespace="System.Data.OleDb" %>
<script runat="server">
bool isEditing = false;
   
    public void BindGrid()
    {
        DataTable teamTable = CommonMethods.loadTeamsDb(Server);
        teamList.DataSource = teamTable;
		teamList.DataBind();
    }

    void Page_Load(Object s, EventArgs e)
    {
		if(!IsPostBack)
		{
			BindGrid();
		}
    }

    protected void updateClub(Object sender, DataGridCommandEventArgs e)
    {
		Team updatedTeamInfo = new Team();
		updatedTeamInfo.bracket = e.Item.Cells[0].Text;
		updatedTeamInfo.rank = Convert.ToInt32(e.Item.Cells[1].Text);
		updatedTeamInfo.teamName = ((TextBox)e.Item.Cells[2].Controls[0]).Text;
		updatedTeamInfo.wins = Convert.ToInt32(((TextBox)e.Item.Cells[3].Controls[0]).Text);
		updatedTeamInfo.losses = Convert.ToInt32(((TextBox)e.Item.Cells[4].Controls[0]).Text);
		CommonMethods.updateTeamInfo(updatedTeamInfo, Server);
		teamList.EditItemIndex = -1;
		BindGrid();
    }

    protected void editItem(Object sender, DataGridCommandEventArgs e)
    {
		if (!isEditing)
		{
			teamList.EditItemIndex = (int)e.Item.ItemIndex;
			BindGrid();
		}
    }
    
    protected void cancelEdit(Object sender, DataGridCommandEventArgs e)
    {
		teamList.EditItemIndex = -1;
		BindGrid();
    }


</script>
<html>
<head>
    <title></title>
</head>
<body>
    <div id="mainbodynoleftbar">
        <h1>Team Management
        </h1><asp:Label id = "testlabel" runat = "server" />
<form runat="server">
<asp:DataGrid id = "teamList" runat = "server" AutoGenerateColumns = "False" OnEditCommand = "editItem" OnCancelCommand = "cancelEdit" OnUpdateCommand = "updateClub" >
<Columns>
	<asp:BoundColumn HeaderText = "Bracket" DataField = "Bracket" ReadOnly = "true" />
	<asp:BoundColumn HeaderText = "Rank" DataField = "TeamRank" ReadOnly = "true"  />
	<asp:BoundColumn HeaderText = "School" DataField = "TeamName" />
	<asp:BoundColumn HeaderText = "Wins" DataField = "TeamWins" />
	<asp:BoundColumn HeaderText = "Losses" DataField = "TeamLosses" />
	<asp:EditCommandColumn HeaderText = "Edit Club" ButtonType = "LinkButton" EditText = "Edit" CancelText = "Cancel" UpdateText = "Update" />
</Columns>
</asp:DataGrid>
		</form>
    </div>
    </body>
</html>
