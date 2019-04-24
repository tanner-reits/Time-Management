using System;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Questica.ETO.Framework;
using System.Configuration;

namespace SQL_WebApp
{
    public partial class Review : System.Web.UI.Page
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
                SqlCommand cmd  = new SqlCommand("csp0334getPayPeriods", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                // Populates dropdown with dates \\
                drpDateSelect.DataSource     = cmd.ExecuteReader();
                drpDateSelect.DataTextField  = "Period";
                drpDateSelect.DataValueField = "ID";
                drpDateSelect.DataBind();

                // Adds initial selection \\
                drpDateSelect.Items.Insert(0, new ListItem(null, null));

                // Closes connection to database \\
                conn.Close();
            }
        }

        protected void drpSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Opens connection to database \\
            conn.Open();

            // Calls stored procedure to populate gridview \\
            SqlCommand cmd  = new SqlCommand("csp0334getAllTimeCardEntries", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            // Creates parameters \\
            SqlParameter empID    = new SqlParameter("@EmployeeID", Session["empID"]);
            SqlParameter periodID = new SqlParameter("@TimePeriodID", drpDateSelect.SelectedValue);

            // Adds parameters \\
            cmd.Parameters.Add(periodID);
            cmd.Parameters.Add(empID);

            // Fills gridView with entries \\
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.HasRows)
            {
                lblNotice.Visible     = false;
                TimeReview.Visible    = true;
                TimeReview.DataSource = rdr;
                TimeReview.DataBind();
            }
            else
            {
                lblNotice.Visible  = true;
                TimeReview.Visible = false;
            }

            // Closes connection to database \\
            conn.Close();
        }
    }
}
