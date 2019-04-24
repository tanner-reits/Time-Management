using System;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Questica.ETO.Framework;
using System.Configuration;

namespace SQL_WebApp
{
    public partial class Management : System.Web.UI.Page
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
                // Opens connection to SQL database \\
                conn.Open();

                // Calls stored procedure to get pay-periods \\
                SqlCommand cmd  = new SqlCommand("csp0334getManagerEmployees", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                // Creates parameters \\
                SqlParameter firstName = new SqlParameter("@FirstName", Session["firstName"].ToString());
                SqlParameter lastName  = new SqlParameter("@LastName", Session["lastName"].ToString());

                // Adds parameters \\
                cmd.Parameters.Add(firstName);
                cmd.Parameters.Add(lastName);

                // Populates dropdown with dates \\
                drpEmployee.DataSource     = cmd.ExecuteReader();
                drpEmployee.DataTextField  = "Name";
                drpEmployee.DataValueField = "Name";
                drpEmployee.DataBind();

                // Adds initial selection \\
                drpEmployee.Items.Insert(0, new ListItem(null, null));

                // Closes connection to database \\
                conn.Close();

                if (drpEmployee.Items.Count == 1)
                {
                    pnlSelect.Visible   = false;
                    drpEmployee.Visible = false;
                    lblEmployee.Visible = false;
                    lblNotice.Text      = "You do not have any available employees to review.";
                    lblNotice.Visible   = true;
                }
            }
        }

        protected void btnApproveSelected_Click(object sender, EventArgs e)
        {
            foreach(GridViewRow row in EmpReview.Rows)
            {
                CheckBox cb = (CheckBox)row.FindControl("SelectCheckBox");
                if (cb.Checked)
                {
                    // Opens connection \\
                    conn.Open();

                    // Creates command to approve entry \\
                    SqlCommand cmd = new SqlCommand("csp0334approveSelectedEntry", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    // Adds parameter \\
                    SqlParameter entryID = new SqlParameter("@EntryID", get_key(row, conn));
                    cmd.Parameters.Add(entryID);

                    // Executes command \\
                    cmd.ExecuteReader();

                    // Closes connection \\
                    conn.Close();

                    // Refreshes gridview \\
                    drpEmployee_SelectedIndexChanged(drpEmployee, null);
                }
            }
        }

        protected void drpEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpEmployee.SelectedIndex != 0)
            {
                // Splits employee name into first and last name \\
                string full  = drpEmployee.SelectedValue.ToString();
                var names    = full.Split();
                string first = names[0];
                string last  = names[1];

                // Opens connection to database \\
                conn.Open();

                // Calls stored procedure to populate gridview \\
                SqlCommand cmd  = new SqlCommand("csp0334getSelectedEmployeeTimeCardEntries", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                // Creates paramters \\
                SqlParameter firstName = new SqlParameter("@FirstName", first);
                SqlParameter lastName  = new SqlParameter("@LastName", last);

                // Adds paramters \\
                cmd.Parameters.Add(firstName);
                cmd.Parameters.Add(lastName);

                // Fills gridView with entries \\
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    pnlSelect.Visible          = true;
                    btnApprove.Visible         = true;
                    btnApproveSelected.Visible = true;
                    pnlEdit.Visible            = false;
                    lblNotice.Visible          = false;
                    EmpReview.Visible          = true;
                    EmpReview.DataSource       = rdr;
                    EmpReview.DataBind();
                    EmpReview.SelectedIndex    = -1;
                }
                else
                {
                    lblNotice.Text             = "No entries available for the selected employee.";
                    lblNotice.Visible          = true;
                    EmpReview.Visible          = false;
                    pnlSelect.Visible          = true;
                    btnApprove.Visible         = false;
                    btnApproveSelected.Visible = false;
                    pnlEdit.Visible            = false;
                    lblTotalHours.Visible      = false;
                }
                rdr.Close();

                // Disables checkbox if already approved \\
                if (EmpReview.Visible)
                {
                    foreach (GridViewRow row in EmpReview.Rows)
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
                            CheckBox cb = (CheckBox)row.FindControl("SelectCheckBox");
                            cb.Checked = true;
                            cb.Enabled = false;
                        }
                    }
                }

                // Gets total hours for selected employee \\
                getEmpTotalHours(conn, first, last);

                // Closes connection to database \\
                conn.Close();
            }
            else
            {
                EmpReview.Visible     = false;
                lblNotice.Text        = "Please select a valid employee from the list";
                lblNotice.Visible     = true;
                pnlSelect.Visible     = false;
                lblTotalHours.Visible = false;
            }
        }

        protected void getEmpTotalHours(SqlConnection conn, string firstName, string lastName)
        {
            // Runs command to get total hours \\
            SqlCommand hours  = new SqlCommand("csp0334getSelectedEmpTotalHours", conn);
            hours.CommandType = System.Data.CommandType.StoredProcedure;

            // Adds paramters \\
            SqlParameter first = new SqlParameter("@FirstName", firstName);
            SqlParameter last  = new SqlParameter("@LastName", lastName);
            hours.Parameters.Add(first);
            hours.Parameters.Add(last);

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

        protected void empReview_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Shows appropriate controls \\
            pnlSelect.Visible    = false;
            pnlEdit.Visible      = true;
            btnSubmit.Visible    = true;
            btnDelete.Visible    = true;
            btnSubmitNew.Visible = false;

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
            SqlParameter id = new SqlParameter("@Project", EmpReview.SelectedRow.Cells[3].Text);
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
            if (((CheckBox)EmpReview.SelectedRow.Cells[7].Controls[0]).Checked)
            {
                chkRoad.Checked = true;
            }
            else
            {
                chkRoad.Checked = false;
            }

            // Sets initially selected values \\
            drpDate.SelectedValue    = EmpReview.SelectedRow.Cells[2].Text;
            drpProject.SelectedValue = EmpReview.SelectedRow.Cells[3].Text;
            drpStation.SelectedValue = EmpReview.SelectedRow.Cells[4].Text;
            drpLabor.SelectedValue   = EmpReview.SelectedRow.Cells[5].Text;
            txtHours.Text            = EmpReview.SelectedRow.Cells[6].Text;

            // Closes connection to database \\
            conn.Close();
        }

        protected int get_key(GridViewRow row, SqlConnection conn)
        {
            // Calls method to get employeeID \\
            SqlDataReader userrdr = selectOnName("EmployeeID", conn);

            // Adds value to parameter \\
            int id = -5;
            if (userrdr.HasRows)
            {
                id = userrdr.GetInt32(userrdr.GetOrdinal("EmployeeID"));
            }
            userrdr.Close();

            // Calls stored procedure to get entry ID key \\
            SqlCommand cmd  = new SqlCommand("csp0334getTimeCardEntryKey", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            // Creates parameters \\
            SqlParameter user    = new SqlParameter("@EmployeeID", id);
            SqlParameter date    = new SqlParameter("@EntryDate", row.Cells[2].Text);
            SqlParameter project = new SqlParameter("@ProjectID", row.Cells[3].Text);
            SqlParameter labor   = new SqlParameter("@LaborCode", row.Cells[5].Text);
            SqlParameter station = new SqlParameter("@Station", row.Cells[4].Text);
            SqlParameter hours   = new SqlParameter("@Hours", row.Cells[6].Text);
            SqlParameter road    = new SqlParameter("@Road", null);
            if (((CheckBox)row.Cells[7].Controls[0]).Checked)
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
            int key = get_key(EmpReview.SelectedRow, conn);
            if (key == -5)
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

            // Hides controls and refreshes gridview \\
            pnlEdit.Visible = false;
            drpEmployee_SelectedIndexChanged(drpEmployee, null);
            EmpReview.SelectedIndex = -1;    
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // Opens connection to SQL database \\
            conn.Open();

            // Calls function to get EntryID key \\
            int key = get_key(EmpReview.SelectedRow, conn);
            if (key == -5)
            {
                lblError.Text    = "Error submitting changes. Please try again.";
                lblError.Visible = true;
                return;
            }

            // Calls stored procedure to delete timecard entry \\
            SqlCommand cmd  = new SqlCommand("csp0334updateTimeCardEntry", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            // Creates parameters \\
            SqlParameter id = new SqlParameter("@EntryID", key);
            cmd             = addParameters(cmd);
            cmd.Parameters.Add(id);

            // Executes stored procedure \\
            cmd.ExecuteReader();

            // Closes connection to database \\
            conn.Close();

            // Hides controls and refreshes gridview \\
            pnlEdit.Visible = false;
            drpEmployee_SelectedIndexChanged(drpEmployee, null);
            EmpReview.SelectedIndex = -1;
        }

        protected SqlCommand addParameters(SqlCommand cmd)
        {
            // Creates parameters \\
            SqlParameter date      = new SqlParameter("@EntryDate", Convert.ToDateTime(drpDate.SelectedValue.ToString()));
            SqlParameter project   = new SqlParameter("@ProjectID", drpProject.SelectedValue.ToString());
            SqlParameter station   = new SqlParameter("@Station", drpStation.SelectedValue.ToString());
            SqlParameter laborCode = new SqlParameter("@LaborCode", drpLabor.SelectedValue.ToString());
            SqlParameter hours     = new SqlParameter("@Hours", txtHours.Text);
            SqlParameter manager   = new SqlParameter("@Manager", 1);
            SqlParameter road      = new SqlParameter("@Road", null);
            if (chkRoad.Checked == true)
            {
                road.Value = 1;
            }
            else
            {
                road.Value = 0;
            }

            // Adds parameters \\
            cmd.Parameters.Add(date);
            cmd.Parameters.Add(project);
            cmd.Parameters.Add(station);
            cmd.Parameters.Add(laborCode);
            cmd.Parameters.Add(hours);
            cmd.Parameters.Add(road);
            cmd.Parameters.Add(manager);

            return cmd;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlEdit.Visible         = false;
            pnlSelect.Visible       = true;
            EmpReview.SelectedIndex = -1;
        }

        protected void drpProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Opens connection to database \\
            conn.Open();

            // Creates SQL command to get stations \\
            SqlCommand station  = new SqlCommand("csp0334getStations", conn);
            station.CommandType = System.Data.CommandType.StoredProcedure;

            // Adds project paramter to stored procedure \\
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

        protected void btnApproveAll_Click(object sender, EventArgs e)
        {
            // Opens connection to database \\
            conn.Open();

            // Sets all employee entries for current period to approved status \\
            SqlCommand cmd  = new SqlCommand("csp0334approveAllEntries", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            // Calls method to get employeeID \\
            SqlDataReader rdr = selectOnName("EmployeeID", conn);
            int empID         = rdr.GetInt32(rdr.GetOrdinal("EmployeeID"));
            rdr.Close();

            SqlParameter id = new SqlParameter("@EmployeeID", empID);
            cmd.Parameters.Add(id);
            cmd.ExecuteReader();

            // Closes connection to database \\
            conn.Close();
            drpEmployee_SelectedIndexChanged(drpEmployee, null);
        }

        protected void btnNewEntry_Click(object sender, EventArgs e)
        {
            // Shows entry fields \\
            pnlEdit.Visible      = true;
            pnlSelect.Visible    = false;
            btnDelete.Visible    = false;
            btnSubmit.Visible    = false;
            btnCancel.Visible    = true;
            btnSubmitNew.Visible = true;

            // Clears all ddl \\
            drpDate.Items.Clear();
            drpProject.Items.Clear();
            drpStation.Items.Clear();
            drpLabor.Items.Clear();
            txtHours.Text   = null;
            chkRoad.Checked = false;

            // Calls stored procedure to get available entry dates \\
            conn.Open();
            SqlCommand cmd  = new SqlCommand("csp0334getDatesTimecard", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            // Populates date drop-down with available dates \\
            drpDate.DataSource     = cmd.ExecuteReader();
            drpDate.DataTextField  = "Date";
            drpDate.DataValueField = "Date";
            drpDate.DataBind();

            // Sets initial selection to null \\
            drpDate.Items.Insert(0, new ListItem(null, null));

            conn.Close();
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

        protected void btnSubmitNew_Click(object sender, EventArgs e)
        {
            // Opens connection \\
            conn.Open();

            // Calls method to get UserID \\
            SqlDataReader rdr = selectOnName("UserID", conn);
            string username   = rdr.GetString(0);
            rdr.Close();

            // Calls procedure to write entry to table \\
            SqlCommand cmd  = new SqlCommand("csp0334InsertTimeCardDetails", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            // Creates parameters \\
            SqlParameter user = new SqlParameter("@Username", username);
            cmd               = addParameters(cmd);
            cmd.Parameters.Add(user);

            // Executes stored procedure \\
            cmd.ExecuteReader();

            // Closes connection \\
            conn.Close();

            // Refreshes gridview and hides controls \\
            drpEmployee_SelectedIndexChanged(drpEmployee, null);
            EmpReview.SelectedIndex = -1;
            pnlEdit.Visible         = false;
            pnlSelect.Visible       = true;
        }

        protected SqlDataReader selectOnName(string parameter, SqlConnection conn)
        {
            // Splits name \\
            string full  = drpEmployee.SelectedValue.ToString();
            var names    = full.Split();
            string first = names[0];
            string last  = names[1];

            // Creates command and paramters to get username for selected employee \\
            SqlCommand cmd         = new SqlCommand("SELECT " + parameter + " FROM tblEmployee WHERE EmpFirstName = @FirstName AND EmpLastName = @LastName", conn);
            SqlParameter firstName = new SqlParameter("@FirstName", first);
            SqlParameter lastName  = new SqlParameter("@LastName", last);
            cmd.Parameters.Add(firstName);
            cmd.Parameters.Add(lastName);

            // Executes command to get username \\
            SqlDataReader rdr = cmd.ExecuteReader();
            rdr.Read();

            return rdr;
        }
    }
}
