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
        DataTable teamTable = CommonMethods.loadTeamsDb();
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
        updatedTeamInfo.sCurveRank = Convert.ToInt32(e.Item.Cells[1].Text);
        updatedTeamInfo.rank = Convert.ToInt32(e.Item.Cells[2].Text);
		updatedTeamInfo.teamName = ((TextBox)e.Item.Cells[3].Controls[0]).Text;
		updatedTeamInfo.wins = Convert.ToInt32(((TextBox)e.Item.Cells[4].Controls[0]).Text);
		updatedTeamInfo.losses = Convert.ToInt32(((TextBox)e.Item.Cells[5].Controls[0]).Text);
		updatedTeamInfo.eliminated = ((bool)((CheckBox)e.Item.Cells[6].Controls[1]).Checked);

		CommonMethods.updateTeamInfo(updatedTeamInfo);
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
    <title>Edit Teams</title>
        <style type="text/css"> @import url(style.css);</style>

</head>
<body>
    <div id="mainbodynoleftbar">
        <h1>Edit Teams
        </h1><asp:Label id = "testlabel" runat = "server" />
<form runat="server">
<asp:DataGrid CellPadding = "2" CellSpacing = "2" Gridlines = "None" CssClass="GridStyle1" id = "teamList" runat = "server" AutoGenerateColumns = "False" OnEditCommand = "editItem" OnCancelCommand = "cancelEdit" OnUpdateCommand = "updateClub" >
<HeaderStyle CssClass="HeaderStyle" />
<ItemStyle CssClass="ItemStyle" />
<EditItemStyle CssClass="EditItemStyle" />

<Columns>
	<asp:BoundColumn HeaderText = "Bracket" DataField = "Bracket" ReadOnly = "true" />
	<asp:BoundColumn HeaderText = "S Curve Rank" DataField = "SCurveRank" ReadOnly = "true"  />
	<asp:BoundColumn HeaderText = "Rank" DataField = "TeamRank" ReadOnly = "true"  />
	<asp:BoundColumn HeaderText = "School" DataField = "TeamName" />
	<asp:BoundColumn HeaderText = "Wins" DataField = "TeamWins" />
	<asp:BoundColumn HeaderText = "Losses" DataField = "TeamLosses" />
    <asp:TemplateColumn
           HeaderText="Eliminated"
           Visible="True">
		<ItemTemplate>
			<asp:Label id = "isEliminatedLabel" runat = "server" Text = '<%# ((bool)DataBinder.Eval(Container, "DataItem.Eliminated")).ToString() %>' />
		</ItemTemplate>
		<EditItemTemplate>
            <asp:CheckBox id="isEliminatedCheckBox" runat="server"  Checked = '<%# ((bool)DataBinder.Eval(Container, "DataItem.Eliminated")) %>' ></asp:CheckBox>
		</EditItemTemplate>
	</asp:TemplateColumn>
	<asp:EditCommandColumn HeaderText = "Edit Club" ButtonType = "LinkButton" EditText = "Edit" CancelText = "Cancel" UpdateText = "Update" />
</Columns>
</asp:DataGrid>
		</form>
    </div>
    </body>
</html>
