using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using NcaaTourneyPool.Controls;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.Serialization.Json;
using System.IO;

namespace NcaaTourneyPool
{
    public partial class ChooseTeams2 : System.Web.UI.Page
    {
        private CurrentStatus _currentStatus;
        private Dictionary<int, Player> _players;
        private Round[] _rounds;

        private int _userId;

        private Dictionary<int, List<Team>> _bracketOneTeams;
        private Dictionary<int, List<Team>> _bracketTwoTeams;
        private Dictionary<int, List<Team>> _bracketThreeTeams;
        private Dictionary<int, List<Team>> _bracketFourTeams;

        private string _bracketOneName;
        private string _bracketTwoName;
        private string _bracketThreeName;
        private string _bracketFourName;

        private Dictionary<int, Team> _allTeamsBySCurveRank;
        private Team[] _allTeamsArray;

        protected CurrentStatus CurrentStatus { get { return _currentStatus; } }
        protected Round CurrentRound { get { return _rounds[CurrentStatus.round - 1]; } }
        protected Player CurrentPlayer { get { return Players[UserId]; } }
        protected Dictionary<int, Player> Players { get { return _players; } }
        protected int UserId { get { return _userId; } }

        protected string BracketOneName { get { return _bracketOneName; } }
        protected string BracketTwoName { get { return _bracketTwoName; } }
        protected string BracketThreeName { get { return _bracketThreeName; } }
        protected string BracketFourName { get { return _bracketFourName; } }

        protected Team[] AllTeamsArray { get { return _allTeamsArray; } }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            buttonSubmit.Click += new EventHandler(buttonSubmit_Click);
        }

        void buttonSubmit_Click(object sender, EventArgs e)
        {
            Team[] selectedTeams = GetSelectedTeams();

            int pointTotal = 0;

            foreach (Team team in selectedTeams)
            {
                if (team.pickedByPlayer != 0)
                {
                    throw new InvalidOperationException(string.Format("{0} has already been chosen.", team.teamName));
                }

                pointTotal += team.cost;
            }

            int pointsAvailable = CurrentRound.pointsToSpend + CurrentPlayer.pointsHeldOver;

            if (pointTotal > pointsAvailable)
            {
                throw new InvalidOperationException("Cannot afford these teams");
            }
            else if ((pointsAvailable - pointTotal) > CurrentRound.maxHoldOverPoints)
            {
                throw new InvalidOperationException(string.Format("Cannot hold over more than {0} points.", CurrentRound.maxHoldOverPoints));
            }

            CurrentPlayer.pointsHeldOver = pointsAvailable - pointTotal;
            CommonMethods.advanceDraft(selectedTeams, CurrentPlayer, CurrentStatus);
            Response.Redirect(string.Format("lobby.aspx?userId={0}", UserId));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _currentStatus = CommonMethods.loadCurrentStatus();
            _rounds = CommonMethods.loadRoundsForLobby();
            _userId = Convert.ToInt32(Request.QueryString["userId"]);

            int currentPickOrder = _rounds[_currentStatus.round - 1].roundOrder[_currentStatus.currentOrderIndex];

            // Load up the players
            _players = new Dictionary<int, Player>();
            foreach (Player player in CommonMethods.loadPlayersForLobby())
            {
                _players.Add(player.userId, player);

                if (player.initialPickOrder == currentPickOrder)
                {
                    _currentStatus.currentUserId = player.userId;
                }
            }

            if (CurrentStatus.currentUserId != UserId)
            {
                Response.Redirect(string.Format("lobby.aspx?userid={0}", UserId));
            }

            List<Team> allTeams = new List<Team>();

            Team[] teams = CommonMethods.loadTeamsForBracketView(1);
            _bracketOneTeams = CommonMethods.SortTeamsByRank(teams);
            BuildBracket(_bracketOneTeams, placeholderBracket1);
            allTeams.AddRange(teams);

            teams = CommonMethods.loadTeamsForBracketView(2);
            _bracketTwoTeams = CommonMethods.SortTeamsByRank(teams);
            BuildBracket(_bracketTwoTeams, placeholderBracket2);
            allTeams.AddRange(teams);

            teams = CommonMethods.loadTeamsForBracketView(3);
            _bracketThreeTeams = CommonMethods.SortTeamsByRank(teams);
            BuildBracket(_bracketThreeTeams, placeholderBracket3);
            allTeams.AddRange(teams);

            teams = CommonMethods.loadTeamsForBracketView(4);
            _bracketFourTeams = CommonMethods.SortTeamsByRank(teams);
            BuildBracket(_bracketFourTeams, placeholderBracket4);
            allTeams.AddRange(teams);

            allTeams.Sort(delegate(Team l, Team r) { return l.sCurveRank.CompareTo(r.sCurveRank); });

            _allTeamsBySCurveRank = new Dictionary<int, Team>();
            foreach (Team team in allTeams)
            {
                _allTeamsBySCurveRank.Add(team.sCurveRank, team);
            }

            _bracketOneName = CommonMethods.loadBracketName(1);
            _bracketTwoName = CommonMethods.loadBracketName(2);
            _bracketThreeName = CommonMethods.loadBracketName(3);
            _bracketFourName = CommonMethods.loadBracketName(4);

            _allTeamsArray = allTeams.ToArray();
        }

        private Team[] GetSelectedTeams()
        {
            List<Team> teams = new List<Team>();

            foreach (string s in hiddenSelectedTeams.Value.Split(','))
            {
                int sCurveRank = int.Parse(s);

                teams.Add(_allTeamsBySCurveRank[sCurveRank]);
            }

            return teams.ToArray();
        }

        /// <summary>
        /// Builds a bracket from the specified teams into the specified placeholder.
        /// </summary>
        /// <param name="teams">The teams to populate the bracket with</param>
        /// <param name="placeHolder">The place to put the controls</param>
        private void BuildBracket(Dictionary<int, List<Team>> teams, PlaceHolder placeHolder)
        {
            placeHolder.Controls.Add(CreateMatchup(teams[1], teams[16]));

            placeHolder.Controls.Add(CreateMatchup(teams[8], teams[9]));

            placeHolder.Controls.Add(CreateMatchup(teams[5], teams[12]));

            placeHolder.Controls.Add(CreateMatchup(teams[4], teams[13]));

            placeHolder.Controls.Add(CreateMatchup(teams[6], teams[11]));

            placeHolder.Controls.Add(CreateMatchup(teams[3], teams[14]));

            placeHolder.Controls.Add(CreateMatchup(teams[7], teams[10]));

            placeHolder.Controls.Add(CreateMatchup(teams[2], teams[15]));
        }

        private Matchup CreateMatchup(List<Team> first, List<Team> second)
        {
            Matchup matchup = (Matchup)LoadControl("~/Controls/Matchup.ascx");

            // Build the first matchup
            if (first.Count > 1)
            {
                Debug.Assert(first.Count == 2, "What? More than two teams?!");

                HtmlGenericControl listItem = new HtmlGenericControl();
                listItem.TagName = "li";
                listItem.Attributes.Add("class", "first");
                listItem.Controls.Add(CreateMatchup(new List<Team>(new Team[] { first[0] }), new List<Team>(new Team[] { first[1] })));

                matchup.Team1PlaceHolder.Controls.Add(listItem);
            }
            else
            {
                Debug.Assert(first.Count == 1, "What? No team?!");

                TeamDisplay team1Display = (TeamDisplay)LoadControl("~/Controls/TeamDisplay.ascx");
                team1Display.Team = first[0];
                team1Display.IsFirst = true;

                if (first[0].pickedByPlayer != 0)
                {
                    team1Display.Color = ColorTranslator.FromHtml(_players[first[0].pickedByPlayer].color);
                }

                matchup.Team1PlaceHolder.Controls.Add(team1Display);
            }

            // Build the second matchup
            if (second.Count > 1)
            {
                Debug.Assert(second.Count == 2, "What? More than two teams?!");

                HtmlGenericControl listItem = new HtmlGenericControl();
                listItem.TagName = "li";
                listItem.Attributes.Add("class", "last");
                listItem.Controls.Add(CreateMatchup(new List<Team>(new Team[] { second[0] }), new List<Team>(new Team[] { second[1] })));

                matchup.Team2PlaceHolder.Controls.Add(listItem);
            }
            else
            {
                Debug.Assert(second.Count == 1, "What? No team?!");

                TeamDisplay team2Display = (TeamDisplay)LoadControl("~/Controls/TeamDisplay.ascx");
                team2Display.Team = second[0];
                team2Display.IsLast = true;

                if (second[0].pickedByPlayer != 0)
                {
                    team2Display.Color = ColorTranslator.FromHtml(_players[second[0].pickedByPlayer].color);
                }

                matchup.Team2PlaceHolder.Controls.Add(team2Display);
            }

            return matchup;
        }
    }
}
