using System;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Questica.ETO.Framework;
using System.Configuration;

namespace SQL_WebApp
{
    public partial class Time_Edit : System.Web.UI.Page
    {
        // Creates SQL connection \\
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["mainConn"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["sid"] == null)
            {
                Response.Redirect("login.aspx");
            }

            // Opens connection to SQL database \\
            conn.Open();

            // Populate grid view with current pay-period entries \\
            SqlCommand data  = new SqlCommand("csp0334getCurrentTimeCardEntries", conn);
            data.CommandType = System.Data.CommandType.StoredProcedure;

            // Creates parameter \\
            SqlParameter user = new SqlParameter("@EmployeeID", Session["empID"].ToString());
            data.Parameters.Add(user);

            // Execute stored procedure \\
            SqlDataReader rdr = data.ExecuteReader();
            if (rdr.HasRows)
            {
                TimeView.DataSource = rdr;
                TimeView.DataBind();
                rdr.Close();

                // Disables editing of entries that have already been approved \\
                foreach (GridViewRow row in TimeView.Rows)
                {
                    SqlCommand checkApproved = new SqlCommand("SELECT Approved FROM ctbl0334TimeCardEntryDetails WHERE EntryID = @EntryID", conn);
                    SqlParameter entryID     = new SqlParameter("@EntryID", get_key(row, conn));
                    checkApproved.Parameters.Add(entryID);

                    SqlDataReader approved_rdr = checkApproved.ExecuteReader();
                    approved_rdr.Read();

                    bool approved = approved_rdr.GetBoolean(0);
                    approved_rdr.Close();

                    if (approved)
                    {
                        row.Cells[0].Text = null;
                    }
                }
            }
            else
            {
                rdr.Close();
                Response.Redirect("Default.aspx");
            }

            // Displays total hours if entries present \\
            getTotalHours(conn);

            // Closes connection to database \\
            conn.Close();
        }

        protected void getTotalHours(SqlConnection conn)
        {
            // Runs command to get total hours \\
            SqlCommand hours = new SqlCommand("csp0334getTotalHours", conn);
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
                    double totalHours = hourrdr.GetDouble(0);
                    lblTotalHours.Text = "Total Hours: " + totalHours.ToString();
                    lblTotalHours.Visible = true;
                }
            }
            hourrdr.Close();
        }

        protected void TimeView_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Makes edit controls visible \\
            pnlEdit.Visible = true;

            // Opens connection to SQL database \\
            conn.Open();

            // Calls stored procedure to get available entry dates \\
            SqlCommand date  = new SqlCommand("csp0334getDatesTimecard", conn);
            date.CommandType = System.Data.CommandType.StoredProcedure;

            // Populates date drop-down with available dates \\
            SqlDataReader daterdr  = date.ExecuteReader();
            drpDate.DataSource     = daterdr;
            drpDate.DataTextField  = "Date";
            drpDate.DataValueField = "Date";
            drpDate.DataBind();
            daterdr.Close();

            // Creates SQL command to get ProjectID \\
            SqlCommand project  = new SqlCommand("csp0334getProjectID", conn);
            project.CommandType = System.Data.CommandType.StoredProcedure;

            // Populates project drop-down with ProjectID's \\
            SqlDataReader projectrdr  = project.ExecuteReader();
            drpProject.DataSource     = projectrdr;
            drpProject.DataTextField  = "Project";
            drpProject.DataValueField = "ID";
            drpProject.DataBind();
            projectrdr.Close();

            // Creates SQL command to get stations \\
            SqlCommand station  = new SqlCommand("csp0334getStations", conn);
            station.CommandType = System.Data.CommandType.StoredProcedure;

            // Adds project paramter to stored procedure \\
            SqlParameter id = new SqlParameter("@Project", TimeView.SelectedRow.Cells[2].Text);
            station.Parameters.Add(id);

            // Populates drop-down with stations \\
            SqlDataReader stationrdr  = station.ExecuteReader();
            drpStation.DataSource     = stationrdr;
            drpStation.DataTextField  = "Station";
            drpStation.DataValueField = "ID";
            drpStation.DataBind();
            stationrdr.Close();

            // Creates SQL command to get labor codes \\
            SqlCommand labor  = new SqlCommand("csp0334getLaborCodes", conn);
            labor.CommandType = System.Data.CommandType.StoredProcedure;

            // Populates drop-down with labor codes \\
            SqlDataReader laborrdr  = labor.ExecuteReader();
            drpLabor.DataSource     = laborrdr;
            drpLabor.DataTextField  = "LaborCode";
            drpLabor.DataValueField = "HourType";
            drpLabor.DataBind();
            laborrdr.Close();

            // Checks road checkbox if previously selected \\
            if (((CheckBox)TimeView.SelectedRow.Cells[6].Controls[0]).Checked)
            {
                chkRoad.Checked = true;
            }
            else
            {
                chkRoad.Checked = false;
            }

            // Sets initially selected values \\
            drpDate.SelectedValue    = TimeView.SelectedRow.Cells[1].Text;
            drpProject.SelectedValue = TimeView.SelectedRow.Cells[2].Text;
            drpStation.SelectedValue = TimeView.SelectedRow.Cells[3].Text;
            drpLabor.SelectedValue   = TimeView.SelectedRow.Cells[4].Text;
            txtHours.Text            = TimeView.SelectedRow.Cells[5].Text;

            // Closes connection to database \\
            conn.Close();
        }

        protected int get_key(GridViewRow row, SqlConnection conn)
        {
            // Calls stored procedure to get entry ID key \\
            SqlCommand cmd  = new SqlCommand("csp0334getTimeCardEntryKey", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            // Creates parameters \\
            SqlParameter user    = new SqlParameter("@EmployeeID", Session["empID"].ToString());
            SqlParameter date    = new SqlParameter("@EntryDate", row.Cells[1].Text);
            SqlParameter project = new SqlParameter("@ProjectID", row.Cells[2].Text);
            SqlParameter labor   = new SqlParameter("@LaborCode", row.Cells[4].Text);
            SqlParameter station = new SqlParameter("@Station", row.Cells[3].Text);
            SqlParameter hours   = new SqlParameter("@Hours", row.Cells[5].Text);
            SqlParameter road    = new SqlParameter("@Road", null);
            if (((CheckBox)row.Cells[6].Controls[0]).Checked)
            {
                road.Value = 1;
            }
            else
            {
                road.Value = 0;
            }

            // Adds parameters to stored procedure \\
            cmd.Parameters.Add(date);
            cmd.Parameters.Add(user);
            cmd.Parameters.Add(project);
            cmd.Parameters.Add(station);
            cmd.Parameters.Add(labor);
            cmd.Parameters.Add(hours);
            cmd.Parameters.Add(road);

            // Executes stored procedure \\
            SqlDataReader rdr = cmd.ExecuteReader();
            int key           = -5;
            if (rdr.HasRows)
            {
                rdr.Read();
                key = rdr.GetInt32(rdr.GetOrdinal("EntryID"));
            }
            rdr.Close();

            return key;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            // Opens connection to SQL database \\
            conn.Open();

            // Calls function to get EntryID key \\
            int key = get_key(TimeView.SelectedRow, conn);
            if (key == -1)
            {
                lblError.Text    = "Error deleting entry. Please try again.";
                lblError.Visible = true;
                return;
            }

            // Calls stored procedure to delete timecard entry \\
            SqlCommand cmd  = new SqlCommand("csp0334deleteTimeCardEntry", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            // Creates entry ID parameter \\
            SqlParameter id = new SqlParameter("@EntryID", key);
            cmd.Parameters.Add(id);

            // Executes stored procedure \\
            cmd.ExecuteReader();

            // Closes connection to database \\
            conn.Close();

            // Reloads edit page \\
            Response.Redirect("Time-Edit.aspx");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // Opens connection to SQL database \\
            conn.Open();

            // Calls function to get EntryID key \\
            int key = get_key(TimeView.SelectedRow, conn);
            if (key == -1)
            {
                lblError.Text    = "Error submitting changes. Please try again.";
                lblError.Visible = true;
                return;
            }

            // Calls stored procedure to delete timecard entry \\
            SqlCommand cmd  = new SqlCommand("csp0334updateTimeCardEntry", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            // Creates parameters \\
            SqlParameter id        = new SqlParameter("@EntryID", key);
            SqlParameter date      = new SqlParameter("EntryDate", Convert.ToDateTime(drpDate.SelectedValue.ToString()));
            SqlParameter project   = new SqlParameter("@ProjectID", drpProject.SelectedValue.ToString());
            SqlParameter station   = new SqlParameter("@Station", drpStation.SelectedValue.ToString());
            SqlParameter laborCode = new SqlParameter("@LaborCode", drpLabor.SelectedValue.ToString());
            SqlParameter hours     = new SqlParameter("@Hours", txtHours.Text);
            SqlParameter road      = new SqlParameter();
            road.ParameterName     = "@Road";
            if (chkRoad.Checked)
            {
                road.Value = 1;
            }
            else
            {
                road.Value = 0;
            }

            // Adds parameters \\
            cmd.Parameters.Add(id);
            cmd.Parameters.Add(date);
            cmd.Parameters.Add(project);
            cmd.Parameters.Add(station);
            cmd.Parameters.Add(laborCode);
            cmd.Parameters.Add(hours);
            cmd.Parameters.Add(road);

            // Executes stored procedure \\
            cmd.ExecuteReader();

            // Closes connection to database \\
            conn.Close();

            // Reloads edit page \\
            Response.Redirect("Time-Edit.aspx");
        }

        protected void drpProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Opens connection to database \\
            conn.Open();

            // Calls stored procedure to get stations \\
            SqlCommand station  = new SqlCommand("csp0334getStations", conn);
            station.CommandType = System.Data.CommandType.StoredProcedure;

            // Creates project parameter \\
            SqlParameter id = new SqlParameter("@Project", drpProject.SelectedValue);
            station.Parameters.Add(id);

            // Populates drop-down with stations \\
            SqlDataReader stationrdr  = station.ExecuteReader();
            drpStation.DataSource     = stationrdr;
            drpStation.DataTextField  = "Station";
            drpStation.DataValueField = "ID";
            drpStation.DataBind();
            stationrdr.Close();

            // Closes connection \\
            conn.Close();

            // Adds initial selection to station ddl \\
            drpStation.Items.Insert(0, new ListItem(null, null));
        }
    }
}
