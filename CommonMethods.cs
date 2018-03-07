using System;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.IO;

namespace NcaaTourneyPool
{
    public static class CommonMethods
    {
        private const string SETPATH = "ncaatourneydb.mdb";
        private const string LOGPATH = "log.inc";

        private const string CONNECTSTRING = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";
        public static OleDbConnection dbConnect;

        public static Round[] loadRoundsForLobby()
        {
            string DB = HttpContext.Current.Server.MapPath(SETPATH);
            dbConnect = new OleDbConnection(CONNECTSTRING + DB);

            OleDbDataAdapter dbadapter;
            DataSet clubdataset;

            OleDbCommand olecommand = new OleDbCommand("getAllRounds", dbConnect);
            olecommand.CommandType = CommandType.StoredProcedure;

            dbadapter = new OleDbDataAdapter(olecommand);

            clubdataset = new DataSet();
            dbadapter.Fill(clubdataset, "clubsdata");

            DataTable roundTable = clubdataset.Tables["clubsdata"];

            CurrentStatus nowStatus = loadCurrentStatus();

            int recordCount = roundTable.Rows.Count;

            Round[] theRounds = new Round[recordCount];

            for (int i = 0; i < recordCount; i++)
            {
                theRounds[i] = new Round();
                theRounds[i].roundNumber = Convert.ToInt32(roundTable.Rows[i]["RoundNo"].ToString());
                theRounds[i].pointsToSpend = Convert.ToInt32(roundTable.Rows[i]["PointsToSpend"].ToString());
                theRounds[i].maxHoldOverPoints = Convert.ToInt32(roundTable.Rows[i]["MaxHoldOverPoints"].ToString());
                theRounds[i].roundOrder = new int[nowStatus.totalPlayers];

                if ((theRounds[i].roundNumber % 4) == 1)
                {
                    for (int j = 1; j <= nowStatus.totalPlayers; j++)
                    {
                        theRounds[i].roundOrder[j - 1] = j;
                    }
                }

                if ((theRounds[i].roundNumber % 4) == 2)
                {
                    for (int j = 1; j <= nowStatus.totalPlayers; j++)
                    {
                        theRounds[i].roundOrder[j - 1] = nowStatus.totalPlayers + 1 - j;
                    }
                }

                if ((theRounds[i].roundNumber % 4) == 3)
                {
                    for (int j = 1; j <= nowStatus.totalPlayers; j++)
                    {
                        if (j <= (nowStatus.totalPlayers / 2) + (nowStatus.totalPlayers % 2))
                            theRounds[i].roundOrder[j - 1] = nowStatus.totalPlayers / 2 + j;
                        else
                            theRounds[i].roundOrder[j - 1] = nowStatus.totalPlayers / 2 - (nowStatus.totalPlayers - j);
                    }
                }

                if ((theRounds[i].roundNumber % 4) == 0)
                {
                    for (int j = 1; j <= nowStatus.totalPlayers; j++)
                    {
                        if (j <= (nowStatus.totalPlayers / 2))
                            theRounds[i].roundOrder[j - 1] = nowStatus.totalPlayers / 2 - j + 1;
                        else
                            theRounds[i].roundOrder[j - 1] = nowStatus.totalPlayers / 2 + (nowStatus.totalPlayers - j) + 1;
                    }
                }
            }
            return theRounds;

        }

        public static Player[] loadPlayersForLobby()
        {
            string DB = HttpContext.Current.Server.MapPath(SETPATH);
            dbConnect = new OleDbConnection(CONNECTSTRING + DB);

            OleDbDataAdapter dbadapter;
            DataSet clubdataset;

            OleDbCommand olecommand = new OleDbCommand("getAllUsers", dbConnect);
            olecommand.CommandType = CommandType.StoredProcedure;

            dbadapter = new OleDbDataAdapter(olecommand);

            clubdataset = new DataSet();
            dbadapter.Fill(clubdataset, "clubsdata");

            DataTable playerTable = clubdataset.Tables["clubsdata"];

            CurrentStatus nowStatus = loadCurrentStatus();

            int recordCount = playerTable.Rows.Count;

            Player[] thePlayers = new Player[recordCount];

            for (int i = 0; i < recordCount; i++)
            {
                thePlayers[i] = new Player();
                thePlayers[i].firstName = playerTable.Rows[i]["FirstName"].ToString();
                thePlayers[i].lastName = playerTable.Rows[i]["LastName"].ToString();
                thePlayers[i].pointsHeldOver = Convert.ToInt32(playerTable.Rows[i]["PointsHeldOver"].ToString());
                thePlayers[i].userId = Convert.ToInt32(playerTable.Rows[i]["UserId"].ToString());
                thePlayers[i].color = playerTable.Rows[i]["Color"].ToString();
                thePlayers[i].initialPickOrder = Convert.ToInt32(playerTable.Rows[i]["InitialOrder"].ToString());
            }

            return thePlayers;

        }

        public static void resetDraft(HttpServerUtility theserver)
        {
            string DB = theserver.MapPath(SETPATH);
            dbConnect = new OleDbConnection(CONNECTSTRING + DB);

            OleDbCommand olecommand = new OleDbCommand("resetDraftStatus", dbConnect);
            olecommand.CommandType = CommandType.StoredProcedure;

            olecommand.Connection.Open();

            olecommand.ExecuteNonQuery();

            dbConnect.Close();

            olecommand = new OleDbCommand("resetTeams", dbConnect);
            olecommand.CommandType = CommandType.StoredProcedure;

            olecommand.Connection.Open();

            olecommand.ExecuteNonQuery();

            dbConnect.Close();

            olecommand = new OleDbCommand("resetUserPoints", dbConnect);
            olecommand.CommandType = CommandType.StoredProcedure;

            olecommand.Connection.Open();

            olecommand.ExecuteNonQuery();

            dbConnect.Close();

            clearLog();


        }

        public static CurrentStatus loadCurrentStatus()
        {
            string DB = HttpContext.Current.Server.MapPath(SETPATH);
            dbConnect = new OleDbConnection(CONNECTSTRING + DB);

            OleDbDataAdapter dbadapter;
            DataSet clubdataset;

            OleDbCommand olecommand = new OleDbCommand("getCurrentStatus", dbConnect);
            olecommand.CommandType = CommandType.StoredProcedure;

            dbadapter = new OleDbDataAdapter(olecommand);

            clubdataset = new DataSet();
            dbadapter.Fill(clubdataset, "clubsdata");

            DataTable statusTable = clubdataset.Tables["clubsdata"];
            CurrentStatus nowStatus = new CurrentStatus();

            nowStatus.round = Convert.ToInt32(statusTable.Rows[0]["Round"].ToString());
            nowStatus.maxHoldOverPoints = Convert.ToInt32(statusTable.Rows[0]["MaxHoldOverPoints"].ToString());
            nowStatus.pointsToSpend = Convert.ToInt32(statusTable.Rows[0]["PointsToSpend"].ToString());
            nowStatus.totalPlayers = Convert.ToInt32(statusTable.Rows[0]["UserCount"].ToString());
            nowStatus.currentOrderIndex = Convert.ToInt32(statusTable.Rows[0]["OrderIndex"].ToString());
            nowStatus.finished = (bool)statusTable.Rows[0]["Finished"];
            nowStatus.totalRounds = Convert.ToInt32(statusTable.Rows[0]["RoundCount"]);

            return nowStatus;
        }

        public static DataTable loadTeamCart(HttpServerUtility theserver)
        {
            string DB = theserver.MapPath(SETPATH);
            dbConnect = new OleDbConnection(CONNECTSTRING + DB);

            OleDbDataAdapter dbadapter;
            DataSet clubdataset;

            OleDbCommand olecommand = new OleDbCommand("getTeamCart", dbConnect);
            olecommand.CommandType = CommandType.StoredProcedure;

            dbadapter = new OleDbDataAdapter(olecommand);

            clubdataset = new DataSet();
            dbadapter.Fill(clubdataset, "clubsdata");

            return clubdataset.Tables["clubsdata"];
        }

        public static Team[] loadTeamsForBracketView(int bracketId)
        {
            string DB = HttpContext.Current.Server.MapPath(SETPATH);
            dbConnect = new OleDbConnection(CONNECTSTRING + DB);

            OleDbDataAdapter dbadapter;
            DataSet clubdataset;

            OleDbCommand olecommand = new OleDbCommand("getBracketOfTeams", dbConnect);
            olecommand.CommandType = CommandType.StoredProcedure;

            OleDbParameter parameters = new OleDbParameter("@BracketId", OleDbType.Integer);
            parameters.Value = bracketId;
            olecommand.Parameters.Add(parameters);

            dbadapter = new OleDbDataAdapter(olecommand);

            clubdataset = new DataSet();
            dbadapter.Fill(clubdataset, "clubsdata");

            DataTable teamsTable = clubdataset.Tables["clubsdata"];

            int recordCount = teamsTable.Rows.Count;

            Team[] theTeams = new Team[recordCount];

            CurrentStatus nowStatus = new CurrentStatus();

            for (int i = 0; i < recordCount; i++)
            {
                theTeams[i] = new Team();
                theTeams[i].sCurveRank = Convert.ToInt32(teamsTable.Rows[i]["SCurveRank"]);
                theTeams[i].teamName = teamsTable.Rows[i]["TeamName"].ToString();
                theTeams[i].bracket = teamsTable.Rows[i]["Bracket"].ToString();
                theTeams[i].bracketId = Convert.ToInt32(teamsTable.Rows[i]["BracketId"].ToString());
                theTeams[i].eliminated = ((bool)teamsTable.Rows[i]["Eliminated"]);
                theTeams[i].rank = Convert.ToInt32(teamsTable.Rows[i]["TeamRank"].ToString());
                theTeams[i].wins = Convert.ToInt32(teamsTable.Rows[i]["TeamWins"].ToString());
                theTeams[i].losses = Convert.ToInt32(teamsTable.Rows[i]["TeamLosses"].ToString());
                theTeams[i].cost = Convert.ToInt32(teamsTable.Rows[i]["Cost"]);
                theTeams[i].pickedByPlayer = Convert.ToInt32(teamsTable.Rows[i]["SelectedBy"].ToString());
            }

            return theTeams;

        }


        public static Team[] loadTeamCartOfTeams(HttpServerUtility theserver)
        {

            string DB = theserver.MapPath(SETPATH);
            dbConnect = new OleDbConnection(CONNECTSTRING + DB);

            OleDbDataAdapter dbadapter;
            DataSet clubdataset;

            OleDbCommand olecommand = new OleDbCommand("getTeamCart", dbConnect);
            olecommand.CommandType = CommandType.StoredProcedure;

            dbadapter = new OleDbDataAdapter(olecommand);

            clubdataset = new DataSet();
            dbadapter.Fill(clubdataset, "clubsdata");

            DataTable teamsTable = clubdataset.Tables["clubsdata"];

            int recordCount = teamsTable.Rows.Count;

            Team[] theTeams = new Team[recordCount];

            CurrentStatus nowStatus = new CurrentStatus();

            for (int i = 0; i < recordCount; i++)
            {
                theTeams[i] = new Team();
                theTeams[i].sCurveRank = Convert.ToInt32(teamsTable.Rows[i]["SCurveRank"]);
                theTeams[i].teamName = teamsTable.Rows[i]["TeamName"].ToString();
                theTeams[i].bracket = teamsTable.Rows[i]["Bracket"].ToString();
                theTeams[i].rank = Convert.ToInt32(teamsTable.Rows[i]["TeamRank"].ToString());
                theTeams[i].wins = Convert.ToInt32(teamsTable.Rows[i]["TeamWins"].ToString());
                theTeams[i].losses = Convert.ToInt32(teamsTable.Rows[i]["TeamLosses"].ToString());
                theTeams[i].cost = Convert.ToInt32(teamsTable.Rows[i]["Cost"]);
            }

            return theTeams;
        }

        public static void pickATeam(Team theTeam, Player thePlayer)
        {
            string DB = HttpContext.Current.Server.MapPath(SETPATH);
            dbConnect = new OleDbConnection(CONNECTSTRING + DB);

            OleDbCommand olecommand = new OleDbCommand("selectTeam", dbConnect);
            olecommand.CommandType = CommandType.StoredProcedure;

            OleDbParameter parameters = new OleDbParameter("@UserId", OleDbType.Integer);
            parameters.Value = thePlayer.userId;
            olecommand.Parameters.Add(parameters);

            parameters = new OleDbParameter("@SCurveRank", OleDbType.Integer);
            parameters.Value = theTeam.sCurveRank;
            olecommand.Parameters.Add(parameters);

            olecommand.Connection.Open();

            olecommand.ExecuteNonQuery();

            dbConnect.Close();
        }

        public static void advanceDraft(Team[] selectedTeams, Player thePlayer, CurrentStatus nowStatus)
        {
            string DB = HttpContext.Current.Server.MapPath(SETPATH);
            dbConnect = new OleDbConnection(CONNECTSTRING + DB);

            string logstring = "<b>Round " + nowStatus.round.ToString() + " - " + thePlayer.firstName + " " + thePlayer.lastName + ":</b> ";

            bool first = true;

            foreach (Team thisTeam in selectedTeams)
            {
                pickATeam(thisTeam, thePlayer);
                if (!first) logstring = logstring + ", ";
                logstring = logstring + thisTeam.teamName + " (" + thisTeam.rank.ToString() + ", " + thisTeam.bracket + ", " + (thisTeam.cost).ToString() + " points)";
                first = false;
            }

            logstring = logstring + " <i>Left " + thePlayer.pointsHeldOver.ToString() + " points</i>";
            if (nowStatus.currentOrderIndex >= nowStatus.totalPlayers - 1)
            {
                nowStatus.currentOrderIndex = 0;
                nowStatus.round++;
                logstring = logstring + "<br />";
            }
            else
            {
                nowStatus.currentOrderIndex++;
            }

            OleDbCommand olecommand;
            OleDbParameter parameters;

            if (nowStatus.round <= nowStatus.totalRounds)
            {
                olecommand = new OleDbCommand("updateStatus", dbConnect);
                olecommand.CommandType = CommandType.StoredProcedure;

                parameters = new OleDbParameter("@Round", OleDbType.Integer);
                parameters.Value = nowStatus.round;
                olecommand.Parameters.Add(parameters);

                parameters = new OleDbParameter("@OrderIndex", OleDbType.Integer);
                parameters.Value = nowStatus.currentOrderIndex;
                olecommand.Parameters.Add(parameters);

            }
            else
            {
                olecommand = new OleDbCommand("finishDraft", dbConnect);
                olecommand.CommandType = CommandType.StoredProcedure;
            }

            olecommand.Connection.Open();

            olecommand.ExecuteNonQuery();

            dbConnect.Close();

            olecommand = new OleDbCommand("updateUserStatus", dbConnect);
            olecommand.CommandType = CommandType.StoredProcedure;

            parameters = new OleDbParameter("@PointsHeldOver", OleDbType.Integer);
            parameters.Value = thePlayer.pointsHeldOver;
            olecommand.Parameters.Add(parameters);

            parameters = new OleDbParameter("@UserId", OleDbType.Integer);
            parameters.Value = thePlayer.userId;
            olecommand.Parameters.Add(parameters);

            olecommand.Connection.Open();

            olecommand.ExecuteNonQuery();

            dbConnect.Close();
            writeToLog(logstring);

            if (nowStatus.round > nowStatus.totalRounds)
            {
                runEndOfDraftLottery();
            }

        }

        public static void runEndOfDraftLottery()
        {

            string logstring = "<b>End of Draft Lottery</b><br />";

            Player[] thePlayers = loadPlayersForLobby();
            Team[] theTeams = loadTeamsForLotteryAsTeams();

            int totalEntries = 0;
            int entriesToDeduct = 1;

            foreach (Player thisPlayer in thePlayers)
            {
                // Adjust for the 2011 doubling of points
                thisPlayer.pointsHeldOver = Convert.ToInt32(Math.Floor(thisPlayer.pointsHeldOver / 2.0));

                totalEntries = totalEntries + thisPlayer.pointsHeldOver;
            }

            if (totalEntries == 0)
            {
                foreach (Player thisPlayer in thePlayers)
                {
                    thisPlayer.pointsHeldOver = 1;
                    totalEntries = totalEntries + thisPlayer.pointsHeldOver;
                }
            }

            foreach (Player thisPlayer in thePlayers)
            {
                logstring = logstring + "<b>" + thisPlayer.firstName + " " + thisPlayer.lastName + "</b> has <b>" + thisPlayer.pointsHeldOver.ToString() + "</b> entries.<br />";
            }

            logstring = logstring + "<br />";

            while (totalEntries < theTeams.Length)
            {
                totalEntries = 0;
                foreach (Player thisPlayer in thePlayers)
                {
                    thisPlayer.pointsHeldOver = thisPlayer.pointsHeldOver * 2;
                    totalEntries = totalEntries + thisPlayer.pointsHeldOver;
                }
                entriesToDeduct = entriesToDeduct * 2;
            }

            int[] theLotteryArray;

            Random meRandom = new Random();

            int teamsDone = 0;



            foreach (Team thisTeam in theTeams)
            {

                while (totalEntries < (theTeams.Length - teamsDone))
                {
                    if (totalEntries == 0)
                    {
                        foreach (Player thisPlayer in thePlayers)
                        {
                            thisPlayer.pointsHeldOver = 1;
                            totalEntries = totalEntries + thisPlayer.pointsHeldOver;
                            entriesToDeduct = 1;
                        }
                    }
                    else
                    {
                        totalEntries = 0;
                        foreach (Player thisPlayer in thePlayers)
                        {
                            thisPlayer.pointsHeldOver = thisPlayer.pointsHeldOver * 2;
                            totalEntries = totalEntries + thisPlayer.pointsHeldOver;
                        }
                        entriesToDeduct = entriesToDeduct * 2;
                    }
                }

                theLotteryArray = new int[totalEntries];

                int startIndex = 0;
                int playerIndex = 0;

                foreach (Player thisPlayer in thePlayers)
                {
                    if (thisPlayer.pointsHeldOver != 0)
                    {
                        for (int i = startIndex; i < startIndex + thisPlayer.pointsHeldOver; i++)
                        {
                            theLotteryArray[i] = playerIndex;
                        }
                    }
                    startIndex = startIndex + thisPlayer.pointsHeldOver;
                    playerIndex++;
                }


                int givenTo = theLotteryArray[meRandom.Next() % totalEntries];

                pickATeam(thisTeam, thePlayers[givenTo]);

                logstring = logstring + thisTeam.teamName + " (" + thisTeam.rank.ToString() + ", " + thisTeam.bracket + ", " + (thisTeam.cost).ToString() + " points) ";
                logstring = logstring + "is awarded to <b>" + thePlayers[givenTo].firstName + " " + thePlayers[givenTo].lastName + "</b>.<br />";

                thePlayers[givenTo].pointsHeldOver = thePlayers[givenTo].pointsHeldOver - entriesToDeduct;

                totalEntries = 0;

                foreach (Player thisPlayer in thePlayers)
                {
                    totalEntries = totalEntries + thisPlayer.pointsHeldOver;
                }

                teamsDone++;
            }

            OleDbCommand olecommand;

            olecommand = new OleDbCommand("finishDraft", dbConnect);
            olecommand.CommandType = CommandType.StoredProcedure;

            olecommand.Connection.Open();

            olecommand.ExecuteNonQuery();

            dbConnect.Close();

            writeToLog(logstring);
        }

        public static Team[] loadTeamsForLotteryAsTeams()
        {
            string DB = HttpContext.Current.Server.MapPath(SETPATH);
            dbConnect = new OleDbConnection(CONNECTSTRING + DB);

            OleDbDataAdapter dbadapter;
            DataSet clubdataset;

            OleDbCommand olecommand = new OleDbCommand("getUnselectedTeamsForLottery", dbConnect);
            olecommand.CommandType = CommandType.StoredProcedure;

            dbadapter = new OleDbDataAdapter(olecommand);

            clubdataset = new DataSet();
            dbadapter.Fill(clubdataset, "clubsdata");

            DataTable teamsTable = clubdataset.Tables["clubsdata"];

            int recordCount = teamsTable.Rows.Count;

            Team[] theTeams = new Team[recordCount];

            CurrentStatus nowStatus = new CurrentStatus();

            for (int i = 0; i < recordCount; i++)
            {
                theTeams[i] = new Team();
                theTeams[i].sCurveRank = Convert.ToInt32(teamsTable.Rows[i]["SCurveRank"]);
                theTeams[i].teamName = teamsTable.Rows[i]["TeamName"].ToString();
                theTeams[i].bracket = teamsTable.Rows[i]["Bracket"].ToString();
                theTeams[i].bracketId = Convert.ToInt32(teamsTable.Rows[i]["BracketId"].ToString());
                theTeams[i].rank = Convert.ToInt32(teamsTable.Rows[i]["TeamRank"].ToString());
                theTeams[i].wins = Convert.ToInt32(teamsTable.Rows[i]["TeamWins"].ToString());
                theTeams[i].losses = Convert.ToInt32(teamsTable.Rows[i]["TeamLosses"].ToString());
                theTeams[i].cost = Convert.ToInt32(teamsTable.Rows[i]["Cost"]);
                theTeams[i].pickedByPlayer = 0;
            }

            return theTeams;
        }

        private static void writeToLog(string logstring)
        {
            string logfile = HttpContext.Current.Server.MapPath(LOGPATH);
            StreamWriter filetowrite = new StreamWriter(logfile, true);
            filetowrite.WriteLine(logstring + "<br />");
            filetowrite.Close();
        }

        private static void clearLog()
        {
            string logfile = HttpContext.Current.Server.MapPath(LOGPATH);
            StreamWriter filetowrite = new StreamWriter(logfile, false);
            filetowrite.Close();
        }

        public static string loadBracketName(int bracketId)
        {
            string DB = HttpContext.Current.Server.MapPath(SETPATH);
            dbConnect = new OleDbConnection(CONNECTSTRING + DB);

            OleDbCommand olecommand = new OleDbCommand("getBracketName", dbConnect);
            olecommand.CommandType = CommandType.StoredProcedure;

            OleDbParameter parameters = new OleDbParameter("@BracketId", OleDbType.Integer);
            parameters.Value = bracketId;
            olecommand.Parameters.Add(parameters);

            string name = null;

            try
            {
                dbConnect.Open();

                name = olecommand.ExecuteScalar().ToString();
            }
            finally
            {
                if (dbConnect.State != ConnectionState.Closed)
                {
                    dbConnect.Close();
                }
            }

            return name;
        }

        public static DataTable loadUsers(HttpServerUtility theserver)
        {
            string DB = theserver.MapPath(SETPATH);
            dbConnect = new OleDbConnection(CONNECTSTRING + DB);

            OleDbDataAdapter dbadapter;
            DataSet clubdataset;

            OleDbCommand olecommand = new OleDbCommand("getAllUsers", dbConnect);
            olecommand.CommandType = CommandType.StoredProcedure;

            dbadapter = new OleDbDataAdapter(olecommand);

            clubdataset = new DataSet();
            dbadapter.Fill(clubdataset, "clubsdata");

            return clubdataset.Tables["clubsdata"];
        }

        public static DataTable loadRounds(HttpServerUtility theserver)
        {
            string DB = theserver.MapPath(SETPATH);
            dbConnect = new OleDbConnection(CONNECTSTRING + DB);

            OleDbDataAdapter dbadapter;
            DataSet clubdataset;

            OleDbCommand olecommand = new OleDbCommand("getAllRounds", dbConnect);
            olecommand.CommandType = CommandType.StoredProcedure;

            dbadapter = new OleDbDataAdapter(olecommand);

            clubdataset = new DataSet();
            dbadapter.Fill(clubdataset, "clubsdata");

            return clubdataset.Tables["clubsdata"];
        }

        public static DataTable loadTeamsDb()
        {
            string DB = HttpContext.Current.Server.MapPath(SETPATH);
            dbConnect = new OleDbConnection(CONNECTSTRING + DB);

            OleDbDataAdapter dbadapter;
            DataSet clubdataset;

            OleDbCommand olecommand = new OleDbCommand("getTeamsForEditing", dbConnect);
            olecommand.CommandType = CommandType.StoredProcedure;

            dbadapter = new OleDbDataAdapter(olecommand);

            clubdataset = new DataSet();
            dbadapter.Fill(clubdataset, "clubsdata");

            return clubdataset.Tables["clubsdata"];

        }

        public static DataTable loadUnselectedTeams(HttpServerUtility theserver)
        {
            string DB = theserver.MapPath(SETPATH);
            dbConnect = new OleDbConnection(CONNECTSTRING + DB);

            OleDbDataAdapter dbadapter;
            DataSet clubdataset;

            OleDbCommand olecommand = new OleDbCommand("getUnselectedTeams", dbConnect);
            olecommand.CommandType = CommandType.StoredProcedure;

            dbadapter = new OleDbDataAdapter(olecommand);

            clubdataset = new DataSet();
            dbadapter.Fill(clubdataset, "clubsdata");

            return clubdataset.Tables["clubsdata"];

        }

        public static void updateTeamInfo(Team updatedTeam)
        {
            string OFFICERDB = HttpContext.Current.Server.MapPath(SETPATH);
            dbConnect = new OleDbConnection(CONNECTSTRING + OFFICERDB);

            OleDbCommand olecommand = new OleDbCommand("updateTeamInfo", dbConnect);
            olecommand.CommandType = CommandType.StoredProcedure;

            OleDbParameter parameters = new OleDbParameter("@TeamName", OleDbType.Char);
            parameters.Value = updatedTeam.teamName;
            olecommand.Parameters.Add(parameters);

            parameters = new OleDbParameter("@TeamWins", OleDbType.Integer);
            parameters.Value = updatedTeam.wins;
            olecommand.Parameters.Add(parameters);

            parameters = new OleDbParameter("@TeamLosses", OleDbType.Integer);
            parameters.Value = updatedTeam.losses;
            olecommand.Parameters.Add(parameters);

            parameters = new OleDbParameter("@Eliminated", OleDbType.Boolean);
            parameters.Value = updatedTeam.eliminated;
            olecommand.Parameters.Add(parameters);


            parameters = new OleDbParameter("@SCurveRank", OleDbType.Integer);
            parameters.Value = updatedTeam.sCurveRank;
            olecommand.Parameters.Add(parameters);

            olecommand.Connection.Open();

            olecommand.ExecuteNonQuery();

            dbConnect.Close();

            return;
        }

        public static Dictionary<int, List<Team>> SortTeamsByRank(Team[] retrievedTeams)
        {
            Dictionary<int, List<Team>> result = new Dictionary<int, List<Team>>();

            foreach (Team team in retrievedTeams)
            {
                if (!result.ContainsKey(team.rank))
                {
                    result[team.rank] = new List<Team>();
                }

                result[team.rank].Add(team);
            }

            return result;
        }

    }
}
