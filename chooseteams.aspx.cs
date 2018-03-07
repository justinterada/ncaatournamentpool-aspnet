using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace NcaaTourneyPool
{
	/// <summary>
	/// Summary description for chooseteams.
	/// </summary>
	public class chooseteams : System.Web.UI.Page
	{
		protected bool isEditing = false;
		protected DataTable teamTable;
		protected CurrentStatus nowStatus;
		protected Round[] theRounds;
		protected Player[] thePlayers;
		protected int userId;
		protected int pointsAvailable;



		protected TextBox pointsAvailableLabel;
		protected TextBox maxHoldOverLabel;
		protected DataGrid teamList;
		protected DataGrid selectedTeamList;
		protected Label errorMessageLabel;
		protected CompareValidator valPointsAvailable;


		public void BindGrid()
		{
			teamTable = CommonMethods.loadUnselectedTeams(Server);

			teamList.DataSource = teamTable;
			teamList.DataBind();
		
			pointsAvailableLabel.Text = pointsAvailable.ToString();
		}

		protected void Page_Load(Object s, EventArgs e)
		{
			teamList.ItemDataBound +=new DataGridItemEventHandler(teamList_ItemDataBound);
			nowStatus = CommonMethods.loadCurrentStatus();
			theRounds = CommonMethods.loadRoundsForLobby();
			thePlayers = CommonMethods.loadPlayersForLobby();
			userId = Convert.ToInt32(Request.QueryString["userId"]);

			if(userId != thePlayers[theRounds[nowStatus.round-1].roundOrder[nowStatus.currentOrderIndex]-1].userId)
				Response.Redirect("lobby.aspx?userId=" + userId);
			else
			{
				if(!IsPostBack)
				{
					teamTable = CommonMethods.loadUnselectedTeams(Server);
					nowStatus = CommonMethods.loadCurrentStatus();
					pointsAvailable = theRounds[nowStatus.round-1].pointsToSpend + thePlayers[theRounds[nowStatus.round-1].roundOrder[nowStatus.currentOrderIndex]-1].pointsHeldOver;
					pointsAvailableLabel.Text = pointsAvailable.ToString();
					maxHoldOverLabel.Text = theRounds[nowStatus.round-1].maxHoldOverPoints.ToString();

					valPointsAvailable.Type = System.Web.UI.WebControls.ValidationDataType.Integer;
					valPointsAvailable.ValueToCompare = theRounds[nowStatus.round-1].maxHoldOverPoints.ToString();
					valPointsAvailable.Operator = System.Web.UI.WebControls.ValidationCompareOperator.LessThanEqual;
					BindGrid();
				}
			}

		}
    
		public void resetPage(Object s, System.EventArgs e)
		{
			teamTable = CommonMethods.loadUnselectedTeams(Server);
			nowStatus = CommonMethods.loadCurrentStatus();
			pointsAvailable = theRounds[nowStatus.round-1].pointsToSpend + thePlayers[theRounds[nowStatus.round-1].roundOrder[nowStatus.currentOrderIndex]-1].pointsHeldOver;
			maxHoldOverLabel.Text = theRounds[nowStatus.round-1].maxHoldOverPoints.ToString();
			errorMessageLabel.Text = "";
			BindGrid();
		}
    
		public void submitPage(Object s, System.EventArgs e)
		{
			if(Page.IsValid)
			{
				nowStatus = CommonMethods.loadCurrentStatus();
				theRounds = CommonMethods.loadRoundsForLobby();
				thePlayers = CommonMethods.loadPlayersForLobby();

				ArrayList tempSelectedTeams = new ArrayList();

				Team[] selectedTeams;

				foreach(DataGridItem thisItem in teamList.Items)
				{
					CheckBox chkSelectTeam = thisItem.FindControl("chkSelectTeam") as CheckBox;
                    Label lblSCurveRank = thisItem.FindControl("lblSCurveRank") as Label;
                    Label lblTeamRank = thisItem.FindControl("lblTeamRank") as Label;
					Label lblBracketId = thisItem.FindControl("lblBracketId") as Label;
					Label lblSchool = thisItem.FindControl("lblSchool") as Label;
					Label lblWins = thisItem.FindControl("lblWins") as Label;
					Label lblLosses = thisItem.FindControl("lblLosses") as Label;
                    Label lblCost = thisItem.FindControl("lblCost") as Label;
                    Label lblBracketName = thisItem.FindControl("lblBracketName") as Label;

					if(chkSelectTeam.Checked)
					{
						Team thisTeam = new Team();
                        thisTeam.sCurveRank = Convert.ToInt32(lblSCurveRank.Text);
						thisTeam.bracket = lblBracketName.Text;
						thisTeam.wins = int.Parse(lblWins.Text);
						thisTeam.losses = int.Parse(lblLosses.Text);
						thisTeam.teamName = lblSchool.Text;
						thisTeam.bracketId = int.Parse(lblBracketId.Text);
                        thisTeam.cost = Convert.ToInt32(lblCost.Text);

						thisTeam.rank = int.Parse(lblTeamRank.Text);

						tempSelectedTeams.Add(thisTeam);
					}
				}

				selectedTeams = (Team[])tempSelectedTeams.ToArray(typeof(Team));

				Player mePlayer = thePlayers[theRounds[nowStatus.round-1].roundOrder[nowStatus.currentOrderIndex]-1];
				userId = Convert.ToInt32(Request.QueryString["userId"]);
				pointsAvailable = Convert.ToInt32(pointsAvailableLabel.Text);
				if (pointsAvailable <= theRounds[nowStatus.round-1].maxHoldOverPoints)
				{	
					int pointCheck = pointsAvailable;
					foreach(Team a in selectedTeams)
					{
						pointCheck = pointCheck + (a.cost);
					}
			
					if(selectedTeams.Length > 0 && pointCheck == theRounds[nowStatus.round-1].pointsToSpend + thePlayers[theRounds[nowStatus.round-1].roundOrder[nowStatus.currentOrderIndex]-1].pointsHeldOver)
					{
						mePlayer.pointsHeldOver = pointsAvailable;
						CommonMethods.advanceDraft(selectedTeams, mePlayer, nowStatus);
						Response.Redirect("lobby.aspx?userId=" + userId);
					}
					else
					{
						teamTable = CommonMethods.loadUnselectedTeams(Server);
						nowStatus = CommonMethods.loadCurrentStatus();
						pointsAvailable = theRounds[nowStatus.round-1].pointsToSpend + thePlayers[theRounds[nowStatus.round-1].roundOrder[nowStatus.currentOrderIndex]-1].pointsHeldOver;
						maxHoldOverLabel.Text = theRounds[nowStatus.round-1].maxHoldOverPoints.ToString();
						errorMessageLabel.Text = "A timeout has occured.  Please make your selections again.";
						BindGrid();
					}
				}
				else
				{
					errorMessageLabel.Text = "You can only hold over " + theRounds[nowStatus.round-1].maxHoldOverPoints.ToString() + " points.";
				}
			}
		}

		private void teamList_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			if(e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || 
				e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				DataRowView thisrow = e.Item.DataItem as DataRowView ;
				
				int pointValue = int.Parse(thisrow["Cost"].ToString());
				CheckBox chkSelectTeam = e.Item.FindControl("chkSelectTeam") as CheckBox;
				chkSelectTeam.Attributes.Add("onClick", string.Concat("onCheckTeam('", chkSelectTeam.ClientID, "','", pointsAvailableLabel.ClientID, "',", pointValue,");"));
			}
		}
	}
}
