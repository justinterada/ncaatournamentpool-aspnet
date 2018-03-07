<%@ Page Language="C#" ValidateRequest="False" %>
<%@ import Namespace="NcaaTourneyPool" %>
<%@ import Namespace="System.IO" %>
<%@ import Namespace="System.Net" %>
<%@ import Namespace="System.Collections.Specialized" %>
<%@ import Namespace="System.Data" %>
<%@ import Namespace="System.Data.OleDb" %>
<script runat="server">
bool isEditing;

    void SubmitBtn_Click(Object s, EventArgs e)
    {

    }
    
    public void BindUserGrid()
    {
        DataTable userTable = CommonMethods.loadUsers(Server);
        userList.DataSource = userTable;
		userList.DataBind();
    }
    
    public void BindRoundGrid()
    {
        DataTable roundTable = CommonMethods.loadRounds(Server);
        roundList.DataSource = roundTable;
		roundList.DataBind();
    }

    void Page_Load(Object s, EventArgs e)
    {
		if(!IsPostBack)
		{
			BindUserGrid();
			BindRoundGrid();
		}
    }

	protected void deleteUser(Object sender, DataGridCommandEventArgs e)
    {
		testlabel.Text = "Delete hit at " + Convert.ToString(e.Item.ItemIndex);
    }
    

	protected void deleteRound(Object sender, DataGridCommandEventArgs e)
    {
		testlabel.Text = "Delete hit at " + Convert.ToString(e.Item.ItemIndex);
    }
    
    protected void updateUser(Object sender, DataGridCommandEventArgs e)
    {

		if(!AddingNewUser)
		{
		}
		else
		{
			
			AddingNewUser = false;
		}
		userList.EditItemIndex = -1;
		BindUserGrid();
    }
    
    protected void updateRound(Object sender, DataGridCommandEventArgs e)
    {

		if(!AddingNewRound)
		{
		}
		else
		{
			
			AddingNewRound = false;
		}
		roundList.EditItemIndex = -1;
		BindRoundGrid();
    }

    protected void editUser(Object sender, DataGridCommandEventArgs e)
    {
		if (!isEditing)
		{
			userList.EditItemIndex = (int)e.Item.ItemIndex;
			BindUserGrid();
		}
    }
    
    protected void cancelEditUser(Object sender, DataGridCommandEventArgs e)
    {
		userList.EditItemIndex = -1;
		BindUserGrid();
    }

	protected void UserDataGrid_ItemCreated(Object sender, DataGridItemEventArgs e) 
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem) 
		{
			WebControl button;
			button = (WebControl) e.Item.Cells[5].Controls[1];
		
			button.Attributes.Add("onclick", "return confirm (\"Are you sure you want to delete? \");");
		}
	}
	
	protected void editRound(Object sender, DataGridCommandEventArgs e)
    {
		if (!isEditing)
		{
			roundList.EditItemIndex = (int)e.Item.ItemIndex;
			BindRoundGrid();
		}
    }
    
    protected void cancelEditRound(Object sender, DataGridCommandEventArgs e)
    {
		roundList.EditItemIndex = -1;
		BindRoundGrid();
    }

	protected void RoundDataGrid_ItemCreated(Object sender, DataGridItemEventArgs e) 
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem) 
		{
			WebControl button;
			button = (WebControl) e.Item.Cells[4].Controls[1];
		
			button.Attributes.Add("onclick", "return confirm (\"Are you sure you want to delete? \");");
		}
	}
	
    void AddNewUser_Click(Object sender, EventArgs e) {
    
        // add a new row to the end of the data, and set editing mode 'on'
    
        CheckIsEditing("");
    
        if (!isEditing) {
    
            // set the flag so we know to do an insert at Update time
            AddingNewUser = true;
    
            // add new row to the end of the dataset after binding
    
			DataTable userTable = CommonMethods.loadUsers(Server);
    
            int recordCount = userTable.Rows.Count;
            if (recordCount > 1)
                recordCount--;
                
            // add a new blank row to the end of the data
            object[] rowValues = { "", "", 0, Convert.ToInt32(userTable.Rows[recordCount-1][3])+1, "" };
            userTable.Rows.Add(rowValues);
    
            // figure out the EditItemIndex, last record on last page

            userList.EditItemIndex = recordCount;
    
            // databind
   			userList.DataSource = userTable;
			userList.DataBind();
        }
    }
    
    // ---------------------------------------------------------------
    //
    // Helpers Methods:
    //
    
    // property to keep track of whether we are adding a new record,
    // and save it in viewstate between postbacks
    
    protected bool AddingNewUser {
    
        get {
            object o = ViewState["AddingNewUser"];
            return (o == null) ? false : (bool)o;
        }
        set {
            ViewState["AddingNewUser"] = value;
        }
    }
    
    protected bool AddingNewRound {
    
        get {
            object o = ViewState["AddingNewRound"];
            return (o == null) ? false : (bool)o;
        }
        set {
            ViewState["AddingNewRound"] = value;
        }
    }
    
    void CheckIsEditing(string commandName) {
    
        if (userList.EditItemIndex != -1 || roundList.EditItemIndex != -1) {
    
            // we are currently editing a row
            if (commandName != "Cancel" && commandName != "Update") {
    
                // user's edit changes (if any) will not be committed
                testlabel.Text = "Your changes have not been saved yet.  Please press update to save your changes, or cancel to discard your changes, before selecting another item.";
                isEditing = true;
            }
        }
    }
    

</script>
<html>
<html>
<head>
    <title></title>
</head>
<body>
    <div id="mainbodynoleftbar">
        <h1>Setup Draft
        </h1>
        <form method="post" runat="server">
        <asp:Label id = "testlabel" runat = "server" />
<table><tr><td>
<asp:DataGrid id = "userList" runat = "server" AutoGenerateColumns = "False" OnEditCommand = "editUser" OnCancelCommand = "cancelEditUser" OnUpdateCommand = "updateUser" OnItemCreated= "UserDataGrid_ItemCreated">
<Columns>
	<asp:BoundColumn HeaderText = "Initial Order" DataField = "InitialOrder" ReadOnly = "true" />
	<asp:BoundColumn HeaderText = "First Name" DataField = "FirstName" />
	<asp:BoundColumn HeaderText = "Last Name" DataField = "LastName" />
	<asp:BoundColumn HeaderText = "Color" DataField = "Color" />
	<asp:EditCommandColumn HeaderText = "Edit User" ButtonType = "LinkButton" EditText = "Edit" CancelText = "Cancel" UpdateText = "Update" />
    <asp:TemplateColumn
           HeaderText="Delete User"
           Visible="True">
		<ItemTemplate>
			<asp:LinkButton id="deleteUserButton" Text="Delete User" CommandName="delete" runat="server"></asp:LinkButton>
		</ItemTemplate>
		<EditItemTemplate>
		</EditItemTemplate>
	</asp:TemplateColumn>
</Columns>
</asp:DataGrid>
</td>
<td>
<asp:DataGrid id = "roundList" runat = "server" AutoGenerateColumns = "False" OnEditCommand = "editRound" OnCancelCommand = "cancelEditRound" OnUpdateCommand = "updateRound" OnItemCreated= "RoundDataGrid_ItemCreated">
<Columns>
	<asp:BoundColumn HeaderText = "Round Number" DataField = "RoundNo" ReadOnly = "true" />
	<asp:BoundColumn HeaderText = "Points To Spend" DataField = "PointsToSpend" />
	<asp:BoundColumn HeaderText = "Maximum Hold Over Points" DataField = "MaxHoldOverPoints" />
	<asp:EditCommandColumn HeaderText = "Edit Round" ButtonType = "LinkButton" EditText = "Edit" CancelText = "Cancel" UpdateText = "Update" />
    <asp:TemplateColumn
           HeaderText="Delete Round"
           Visible="True">
		<ItemTemplate>
			<asp:LinkButton id="deleteRoundButton" Text="Delete Round" CommandName="delete" runat="server"></asp:LinkButton>
		</ItemTemplate>
		<EditItemTemplate>
		</EditItemTemplate>
	</asp:TemplateColumn>
</Columns>
</asp:DataGrid>
</td>
</tr>
</table>
        </form>
    </div>
</body>
</html>
