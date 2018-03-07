using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Net;
using System.Collections.Specialized;
using System.Data.OleDb;
using System.Drawing;

namespace NcaaTourneyPool
{

	public class Team
	{
        public int sCurveRank;
		public string teamName;
		public int wins;
		public int losses;
		public bool eliminated;
		public int rank;
        public int cost;
		public string bracket;
		public int bracketId;
		public int pickedByPlayer;
	}

	public class Player
	{
		public string firstName;
		public string lastName;
		public int userId;
		public string color;
		public int pointsAvailable;
		public int pointsHeldOver;
		public int initialPickOrder;
	}

	public class CurrentStatus
	{
		public int round;
		public int currentUserId;
		public int currentOrderIndex;
		public int maxHoldOverPoints;
		public int pointsToSpend;
		public int totalPlayers;
		public int totalRounds;
		public bool finished;
	}

	public class Round
	{
		public int roundNumber;
		public int[] roundOrder;
		public int pointsToSpend;
		public int maxHoldOverPoints;
	}
}
