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
using System.Collections.Generic;
using System.Text;

namespace NcaaTourneyPool
{
	/// <summary>
	/// Summary description for bracketview.
	/// </summary>
	public class bracketview : System.Web.UI.Page
	{
		protected bool isEditing = false;
		protected DataTable teamTable;
		protected DataTable selectedTeamTable;
		protected CurrentStatus nowStatus;
		protected Round[] theRounds;
		protected Player[] thePlayers;
		protected Dictionary<int, List<Team>> bracketOneTeams;
        protected Dictionary<int, List<Team>> bracketThreeTeams;
        protected Dictionary<int, List<Team>> bracketFourTeams;
        protected Dictionary<int, List<Team>> bracketTwoTeams;
		protected Hashtable playerColorHash;
		protected int userId;
		protected string bracketOneName; // 1st seed
		protected string bracketThreeName; // 3rd seed
		protected string bracketFourName; // 4th seed
		protected string bracketTwoName; // 2nd seed

		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);

			this.Load += new EventHandler(Page_Load);
		}

		protected void Page_Load(Object s, EventArgs e)
		{
			userId = Convert.ToInt32(Request.QueryString["userId"]);

			if(!IsPostBack)
			{
				playerColorHash = new Hashtable();

				nowStatus = CommonMethods.loadCurrentStatus();
				theRounds = CommonMethods.loadRoundsForLobby();
				thePlayers = CommonMethods.loadPlayersForLobby();

				foreach(Player player in thePlayers)
				{
					playerColorHash.Add(player.userId, player.color);
				}

                bracketOneName = CommonMethods.loadBracketName(1);
                bracketTwoName = CommonMethods.loadBracketName(2);
                bracketThreeName = CommonMethods.loadBracketName(3);
                bracketFourName = CommonMethods.loadBracketName(4);

                bracketOneTeams = CommonMethods.SortTeamsByRank(CommonMethods.loadTeamsForBracketView(1));
                bracketThreeTeams = CommonMethods.SortTeamsByRank(CommonMethods.loadTeamsForBracketView(3));
                bracketFourTeams = CommonMethods.SortTeamsByRank(CommonMethods.loadTeamsForBracketView(4));
                bracketTwoTeams = CommonMethods.SortTeamsByRank(CommonMethods.loadTeamsForBracketView(2));
			}
		}

		private Team[] ReorderArrayForBracketView(Team[] array)
		{
			if(array == null || (array.Length != 16 && array.Length != 24))
			{
				throw new ArgumentException(string.Format("array length wrong, was {0}", array != null ? array.Length : 0));
			}

			if (array.Length == 16)
			{
				return ReorderArrayFor64TeamBracketView(array);
			}
			else if (array.Length == 24)
			{
				return ReorderArrayFor96TeamBracketView(array);
			}
			else
			{
				return null;
			}
		}

		private Team[] ReorderArrayFor64TeamBracketView(Team[] array)
		{
			Team[] newArray = new Team[16];

			newArray[0] = array[0];
			newArray[1] = array[15];

			newArray[2] = array[7];
			newArray[3] = array[8];

			newArray[4] = array[4];
			newArray[5] = array[11];

			newArray[6] = array[3];
			newArray[7] = array[12];

			newArray[8] = array[5];
			newArray[9] = array[10];

			newArray[10] = array[2];
			newArray[11] = array[13];

			newArray[12] = array[6];
			newArray[13] = array[9];

			newArray[14] = array[1];
			newArray[15] = array[14];

			return newArray;
		}

		private Team[] ReorderArrayFor96TeamBracketView(Team[] array)
		{
			Team[] newArray = new Team[24];

			newArray[0] = array[0];
			newArray[1] = array[15];
			newArray[2] = array[16];

			newArray[3] = array[7];
			newArray[4] = array[8];
			newArray[5] = array[23];

			newArray[6] = array[4];
			newArray[7] = array[11];
			newArray[8] = array[20];

			newArray[9] = array[3];
			newArray[10] = array[12];
			newArray[11] = array[19];

			newArray[12] = array[5];
			newArray[13] = array[10];
			newArray[14] = array[21];

			newArray[15] = array[2];
			newArray[16] = array[13];
			newArray[17] = array[18];

			newArray[18] = array[6];
			newArray[19] = array[9];
			newArray[20] = array[22];

			newArray[21] = array[1];
			newArray[22] = array[14];
			newArray[23] = array[17];

			return newArray;
		}

        /// <summary>
        /// A horrendously hacky way of creating the output HTML for a team.
        /// </summary>
        /// <remarks>Just like everything else in this thing, it's hacky.</remarks>
        /// <param name="team">The team to create the table cell for</param>
        /// <returns>The html to put into the table</returns>
        public string CreateTableRowForTeam(Team team)
        {
            StringBuilder s = new StringBuilder("<tr><td class=\"firstRoundCell\" style=\"");

            if (team.pickedByPlayer != 0)
            {
                s.AppendFormat("background-color:{0};", playerColorHash[team.pickedByPlayer].ToString());
            }

            if (team.eliminated)
            {
                s.Append("-ms-filter:'progid:DXImageTransform.Microsoft.Alpha(Opacity=30)';filter: alpha(opacity=30);opacity: .3;"); 
            }
            
            s.Append("\">");
            
            s.AppendFormat("{0}. {1} ({2} - {3})", team.rank, team.teamName, team.wins, team.losses);

            s.Append("</td></tr>");

            return s.ToString();
        }

        /// <summary>
        /// A horrendously hacky method that creates rows for a specified rank and takes in the dictionary
        /// of teams to pull from.
        /// </summary>
        /// <param name="rank">The rank to pull</param>
        /// <param name="teams">The teams to pull from</param>
        /// <returns>The rows needed to render</returns>
        public string CreateTableRowsForRank(int rank, Dictionary<int, List<Team>> teams)
        {
            if (teams == null)
            {
                throw new ArgumentNullException("teams");
            }

            if (!teams.ContainsKey(rank))
            {
                throw new InvalidOperationException(string.Format("No rank {0} found in teams provided. Teams dictionary has {1} elements.", rank, teams.Count));
            }

            StringBuilder s = new StringBuilder();

            foreach (Team team in teams[rank])
            {
                s.Append(CreateTableRowForTeam(team));
            }

            return s.ToString();
        }

        /// <summary>
        /// A hacky method that returns the rendering output for one of the four brackets
        /// for a 64 (or 68) team bracket.
        /// </summary>
        /// <param name="teams">The teams to create the bracket for</param>
        /// <returns>The bracket rendering</returns>
        public string CreateTableRowsFor64TeamBracket(Dictionary<int, List<Team>> teams)
        {
            StringBuilder s = new StringBuilder();

            s.Append(CreateTableRowsForRank(1, teams));
            s.Append(CreateTableRowsForRank(16, teams));

            s.Append("<tr><td class=\"firstRoundSpacerCell\"></td></tr>");

            s.Append(CreateTableRowsForRank(8, teams));
            s.Append(CreateTableRowsForRank(9, teams));

            s.Append("<tr><td class=\"firstRoundSpacerCell\"></td></tr>");

            s.Append(CreateTableRowsForRank(5, teams));
            s.Append(CreateTableRowsForRank(12, teams));

            s.Append("<tr><td class=\"firstRoundSpacerCell\"></td></tr>");

            s.Append(CreateTableRowsForRank(4, teams));
            s.Append(CreateTableRowsForRank(13, teams));

            s.Append("<tr><td class=\"firstRoundSpacerCell\"></td></tr>");

            s.Append(CreateTableRowsForRank(6, teams));
            s.Append(CreateTableRowsForRank(11, teams));

            s.Append("<tr><td class=\"firstRoundSpacerCell\"></td></tr>");

            s.Append(CreateTableRowsForRank(3, teams));
            s.Append(CreateTableRowsForRank(14, teams));

            s.Append("<tr><td class=\"firstRoundSpacerCell\"></td></tr>");

            s.Append(CreateTableRowsForRank(7, teams));
            s.Append(CreateTableRowsForRank(10, teams));

            s.Append("<tr><td class=\"firstRoundSpacerCell\"></td></tr>");

            s.Append(CreateTableRowsForRank(2, teams));
            s.Append(CreateTableRowsForRank(15, teams));

            return s.ToString();
        }
	}
}
