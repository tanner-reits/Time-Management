using System;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Questica.ETO.Framework;
using System.Configuration;

namespace SQL_WebApp
{
    public partial class time_entry : System.Web.UI.Page
    {
        // Creates SQL connection \\
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["mainConn"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["sid"] == null)
            {
                Response.Redirect("login.aspx");
            }

            if (!IsPostBack)
            {
                try
                {
                    // Opens connection to SQL database \\
                    conn.Open();

                    // Calls stored procedure to get current time card entries \\
                    SqlCommand data  = new SqlCommand("csp0334getCurrentTimeCardEntries", conn);
                    data.CommandType = System.Data.CommandType.StoredProcedure;

                    // Creates parameter for the user's employee ID \\
                    SqlParameter user = new SqlParameter("@EmployeeID", Session["empID"].ToString());
                    data.Parameters.Add(user);

                    // Executes Stored Procedure \\
                    SqlDataReader rdr = data.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        lblWelcome.Text     = "Welcome " + Session["firstName"].ToString() + ". Below is the time you entered for the current pay period(s).";
                        TimeView.DataSource = rdr;
                        TimeView.DataBind();
                    }
                    else
                    {
                        lblWelcome.Text = Session["firstName"].ToString() + ", you currently do not have any time entries for the current pay period. Please submit a new entry below.";
                        lblInfo.Visible = false;
                        btnEdit.Visible = false;
                    }
                    rdr.Close();

                    // Calls method to get total hours if entries present
                    getTotalHours(conn);

                    // Calls stored procedure to get available entry dates \\
                    SqlCommand cmd  = new SqlCommand("csp0334getDatesTimecard", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    // Populates date drop-down with available dates \\
                    drpDate.DataSource     = cmd.ExecuteReader();
                    drpDate.DataTextField  = "Date";
                    drpDate.DataValueField = "Date";
                    drpDate.DataBind();

                    // Sets initial selection to null \\
                    drpDate.Items.Insert(0, new ListItem(null, null));
                }
                finally
                {
                    // Closes connection to database \\
                    conn.Close();
                }
            }
        }

        protected void getTotalHours(SqlConnection conn)
        {
            // Runs command to get total hours \\
            SqlCommand hours  = new SqlCommand("csp0334getTotalHours", conn);
            hours.CommandType = System.Data.CommandType.StoredProcedure;

            // Adds paramter \\
            SqlParameter user = new SqlParameter("@EmployeeID", Session["empID"].ToString());
            hours.Parameters.Add(user);

            // Executes command \\
            SqlDataReader hourrdr = hours.ExecuteReader();

            // Fills label if data present \\
            if (hourrdr.HasRows)
            {
                hourrdr.Read();
                if (hourrdr.GetDouble(0) != 0)
                {
                    double totalHours     = hourrdr.GetDouble(0);
                    lblTotalHours.Text    = "Total Hours: " + totalHours.ToString();
                    lblTotalHours.Visible = true;
                }
            }
            hourrdr.Close();
        }

        protected void drpDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Opens connection to SQL database \\
            conn.Open();

            try
            {
                if (drpProject.SelectedItem == null)
                {
                    // Call stored procedure to get ProjectID \\
                    SqlCommand cmd  = new SqlCommand("csp0334getProjectID", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    // Populates project drop-down with ProjectID's \\
                    drpProject.DataSource     = cmd.ExecuteReader();
                    drpProject.DataTextField  = "Project";
                    drpProject.DataValueField = "ID";
                    drpProject.DataBind();

                    // Sets intial selection to blank \\
                    drpProject.Items.Insert(0, new ListItem(null, null));
                }
            }
            finally
            {
                // Closes the connection to database \\
                conn.Close();
            }
        }

        protected void drpProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Opens connection to SQL database \\
            conn.Open();

            try
            {
                // Calls stored procedure to get stations \\
                SqlCommand cmd  = new SqlCommand("csp0334getStations", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                // Creates parameter for ProjectID \\
                SqlParameter project = new SqlParameter("@Project", drpProject.Text);
                cmd.Parameters.Add(project);

                // Populates drop-down with stations \\
                drpStation.DataSource     = cmd.ExecuteReader();
                drpStation.DataTextField  = "Station";
                drpStation.DataValueField = "ID";
                drpStation.DataBind();

                // Sets intial selection to blank \\
                drpStation.Items.Insert(0, new ListItem(null, null));
            }
            finally
            {
                // Closes the connection to database \\
                conn.Close();
            }
        }

        protected void drpStation_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Opens connection to SQL database \\
            conn.Open();

            try
            {
                if (drpLabor.SelectedItem == null)
                {
                    // Calls stored procedure to get labor codes \\
                    SqlCommand cmd  = new SqlCommand("csp0334getLaborCodes", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    // Populates drop-down with labor codes \\
                    drpLabor.DataSource     = cmd.ExecuteReader();
                    drpLabor.DataTextField  = "LaborCode";
                    drpLabor.DataValueField = "HourType";
                    drpLabor.DataBind();

                    // Sets intial selection to blank \\
                    drpLabor.Items.Insert(0, new ListItem(null, null));
                }
            }
            finally
            {
                // Closes the connection to database \\
                conn.Close();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                // Opens connection to SQL database \\
                conn.Open();

                // Calls stored procedure to write to temp-table \\
                SqlCommand cmd  = new SqlCommand("csp0334InsertTimeCardDetails", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                // Creates parameters \\
                SqlParameter username  = new SqlParameter("@Username", Session["username"]);
                SqlParameter entryDate = new SqlParameter("@EntryDate", Convert.ToDateTime(drpDate.SelectedValue.ToString()));
                SqlParameter projectID = new SqlParameter("@ProjectID", drpProject.SelectedValue.ToString());
                SqlParameter station   = new SqlParameter("@Station", drpStation.SelectedValue.ToString());
                SqlParameter laborCode = new SqlParameter("@LaborCode", drpLabor.SelectedValue);
                SqlParameter hours     = new SqlParameter("@Hours", txtHours.Text);
                SqlParameter manager   = new SqlParameter("@Manager", false);
                SqlParameter road      = new SqlParameter("@Road", null);
                if (chkRoad.Checked == true)
                {
                    road.Value = 1;
                }
                else
                {
                    road.Value = 0;
                }

                // Adds paramters to stored procedure \\
                cmd.Parameters.Add(entryDate);
                cmd.Parameters.Add(username);
                cmd.Parameters.Add(projectID);
                cmd.Parameters.Add(station);
                cmd.Parameters.Add(laborCode);
                cmd.Parameters.Add(road);
                cmd.Parameters.Add(manager);
                cmd.Parameters.Add(hours);

                // Executes stored procedure \\
                cmd.ExecuteReader();
            }
            finally
            {
                // Closes connection to database \\
                conn.Close();

                // Reloads entry page \\
                Response.Redirect("Default.aspx");
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("Time-Edit.aspx");
        }
    }
}