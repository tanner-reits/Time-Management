using System;
using System.Data.SqlClient;
using Questica.ETO.Framework;
using System.Configuration;

namespace SQL_WebApp
{
    /* CODE EXECUTED ON PAGE LOAD */
    public partial class WebForm1 : System.Web.UI.Page
    {
        /* CODE EXECUTED WHEN SUBMIT BUTTON IS CLICKED */
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            // Opens connection to SQL database \\
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["mainConn"].ConnectionString);

            try
            {
                // Calls login stored procedure method
                SqlDataReader rdr = empLogin(txtUsername.Text.ToLower(), txtPassword.Text, conn);

                // Performs appropriate assignments and redirects \\
                if (rdr.HasRows && chkActive(rdr))
                {
                    Session["firstName"] = rdr.GetString(rdr.GetOrdinal("EmpFirstName"));
                    Session["lastName"]  = rdr.GetString(rdr.GetOrdinal("EmpLastName"));
                    Session["username"]  = rdr.GetString(rdr.GetOrdinal("UserID"));
                    Session["empID"]     = rdr.GetSqlInt32(rdr.GetOrdinal("EmployeeID"));
                    Session["sid"]       = Session.SessionID;

                    rdr.Close();
                    Response.Redirect("Default.aspx");
                }
                else
                {
                    rdr.Close();
                    txtUsername.Text = null;
                    txtPassword.Text = null;
                    lblError.Visible = true;
                }
            }
            finally
            {
                conn.Close();
            }
        }

        private bool chkActive(SqlDataReader rdr)
        {
            rdr.Read();
            if(rdr.GetBoolean(rdr.GetOrdinal("EmpActive")) == true)
            {
                return true;
            }

            return false;
        }

        private SqlDataReader empLogin(String username, String password, SqlConnection conn)
        {
            // Opens connection \\
            conn.Open();

            // Calls method to get guid \\
            Guid uid = getUid(username, conn);

            // Calls stored procedure \\
            SqlCommand cmd  = new SqlCommand("uspEmployeeLogin", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            // Creates parameters \\
            SqlParameter guid = new SqlParameter("@uidLoginIdentifier", uid);
            SqlParameter pass = new SqlParameter("@vbcPassword", Questica.ETO.Framework.Helpers.GeneralHelper.EncryptString(password));
            SqlParameter user = new SqlParameter("@nvcUserID", username);

            // Adds parameters to command \\
            cmd.Parameters.Add(guid);
            cmd.Parameters.Add(user);
            cmd.Parameters.Add(pass);

            // Creates data reader to execute command \\
            SqlDataReader rdr = cmd.ExecuteReader();

            return rdr;
        }

        private Guid getUid(String username, SqlConnection conn)
        {
            // Creates SQL command \\
            SqlCommand cmd = new SqlCommand("SELECT LoginIdentifier FROM tblEmployee WHERE UserID = @Username", conn);

            // Creates username parameter \\
            SqlParameter user = new SqlParameter("@Username", username);
            cmd.Parameters.Add(user);

            // Creates data reader to execute command \\
            SqlDataReader rdr = cmd.ExecuteReader();

            // Sets GUID variable \\
            Guid uid;
            if (rdr.HasRows)
            {
                rdr.Read();
                uid = rdr.GetGuid(rdr.GetOrdinal("LoginIdentifier"));
            }
            else
            {
                uid = Guid.Empty;
            }

            // Closes data reader \\
            rdr.Close();

            return uid;
        }
    }
}