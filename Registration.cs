using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Ticket_Booking_App;
namespace Concert_ticket_booking_system_final
{
    public partial class Registration : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=.;Initial Catalog=Click Concert;Integrated Security=True");
        public Registration()
        {
            InitializeComponent();
        }
        private void registerBtn_Click(object sender, EventArgs e)
        {
            if (usernameTextBox.Text == "" || passwordTextbox.Text == "" ||
            confirmpasswordTextbox.Text == "")
            {
                errorLabel.Text = "Please enter all the details";
                errorLabel.Visible = true;
            }
            else if (passwordTextbox.Text != confirmpasswordTextbox.Text)
            {
                passwordPanel.BackColor = Color.Tomato;
                confirmPasswordPanel.BackColor = Color.Tomato;
                errorLabel.Text = "Password does not match. Please try again";
                errorLabel.Visible = true;
            }
            else if (passwordTextbox.Text.Length <= 3)
            {
                errorLabel.Visible = true;
                errorLabel.Text = "Password too short.";
            }
            else
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                        //CHECK IF USERNAME IS AVAILABLE 
                        string selectQuery = "SELECT * FROM user_details WHERE user_name ='" + usernameTextBox.Text + "';";
                        SqlCommand cmd = new SqlCommand(selectQuery, conn);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            dr.Close();
                            errorLabel.Visible = true;
                            errorLabel.Text = "Whoops! This username is already taken!";
                            usernamePanel.BackColor = Color.Tomato;
                        }
                        //IF USERNAME IS AVAILABLE REGISTER USER AND REDIRECT TO LOGIN PAGE 
                        else
                        {
                            dr.Close();
                            string insertQuery = "INSERT INTO user_details VALUES(@user_name, @user_password)";
                            cmd = new SqlCommand(insertQuery, conn);
                            cmd.Parameters.AddWithValue("@user_name", usernameTextBox.Text); cmd.Parameters.AddWithValue("@user_password",
                            confirmpasswordTextbox.Text);
                            cmd.ExecuteNonQuery();
                            Login login = new Login();
                            login.Show();
                            login.accountCreationLabel.Visible = true;
                            login.accountCreationLabel.Text = "Your account was successfully created! Please login.";
                            this.Hide();
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
        //IF USER ALREADY HAS AN ACCOUNT LOGIN 
        private void label2_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }
    }
}
