using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Win32;
namespace Concert_ticket_booking_system_final
{
    public partial class Login : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=.;Initial Catalog=Click Concert;Integrated Security=True");
        public Login()
        {
            InitializeComponent();
        }
        private void label2_Click(object sender, EventArgs e)
        {
            //OPENS REGISTRATION FORM IF USER DOES NOT HAVE AN ACCOUNT Registration register = new Registration(); 
            register.Show();
            this.Hide();
        }
        private void loginBtn_Click(object sender, EventArgs e)
        {
            if (usernameTextBox.Text == "" || passwordTextbox.Text == "")
            {
                errorLabel.Visible = true;
                errorLabel.Text = "Please enter all the details";
            }
            else
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                        //CHECKING FOR USER IN DB 
                        string findUserLogin = "SELECT * FROM user_details WHERE user_name = '" + usernameTextBox.Text + "' AND user_password = '" + passwordTextbox.Text + "';"; SqlCommand cmd = new SqlCommand(findUserLogin, conn);
                        SqlDataReader dr = cmd.ExecuteReader();
                        //IF USER EXISTS OPEN USER DASHBOARD 
                        if (dr.Read())
                        {
                            dr.Close();
                            this.Hide();
                            Dashboard dashboard = new Dashboard();
                            dashboard.Show();
                            this.Hide();
                        }
                        //IF USERNAME NOT FOUND THROW ERROR 
                        else
                        {
                            dr.Close();
                            errorLabel.Visible = true;
                            errorLabel.Text = "Incorrect username or password.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}
