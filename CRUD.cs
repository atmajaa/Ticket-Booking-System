using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace Concert_ticket_booking_system_final.childForms
{
    public partial class crud : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=.;Initial Catalog=Click Concert;Integrated Security=True");
        SqlDataAdapter da;
        SqlDataReader dr;
        DataTable dt = new DataTable();
        SqlCommandBuilder scb;
        public crud()
        {
            InitializeComponent();
        }
        private void crud_Load(object sender, EventArgs e)
        {
            //DATAGRIDVIEW STYLING 
            concertsDataGridView.EnableHeadersVisualStyles = false;
            concertsDataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 0, 34);
            concertsDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; //DISPLAY CONTENTS OF CONCERT_DETAILS ON FORM LOAD string selectQuery = "SELECT * FROM concert_details"; 
            da = new SqlDataAdapter(selectQuery, conn);
            da.Fill(dt);
            concertsDataGridView.DataSource = dt;
            concertsDataGridView.Columns[0].HeaderText = "Sr no.";
            concertsDataGridView.Columns[1].HeaderText = "Artist";
            concertsDataGridView.Columns[2].HeaderText = "Venue";
            concertsDataGridView.Columns[3].HeaderText = "Date";
            concertsDataGridView.Columns[4].HeaderText = "Time";
            concertsDataGridView.Columns[5].HeaderText = "Price (in rupees)"; //AUTOSIZE COLUMNS 
            concertsDataGridView.AutoSizeColumnsMode =
            DataGridViewAutoSizeColumnsMode.Fill;
            //DISPLAY VENUES FROM DB IN COMBOBOX 
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                    string showVenuesQuery = "SELECT venue FROM concert_venues;"; SqlCommand cmd = new SqlCommand(showVenuesQuery, conn); dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        venueCombo.Items.Add(dr[0]);
                    }
                    dr.Close();
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
        private void createBtn_Click(object sender, EventArgs e)
        {
            //INSERT ENTRY IN CONCERT_DETAILS TABLE 
            string insertQuery = "INSERT INTO concert_details VALUES ('" +
            artistNameTextbox.Text + "', '" +
            venueCombo.GetItemText(this.venueCombo.SelectedItem).ToString() + "', '" + dateTimePicker1.Text + "', '" + dateTimePicker2.Text + "', '" + priceTextbox.Text + "');"; string artistTableName = artistNameTextbox.Text.Replace(" ", String.Empty); //CREATE A NEW TABLE FOR EACH ARTIST 
            string createArtistTable = "CREATE TABLE " + artistTableName + " (seat_no INT PRIMARY KEY IDENTITY(1,1), status VARCHAR(20));";
            //ADD SEATS AND STATUS 
            string tableEntries = "INSERT INTO " + artistTableName + " (status) 
        VALUES('unbooked'); "; 
        //FIND DUPLICATE ENTRIES 
string duplicateEntriesQuery = "SELECT * FROM concert_details WHERE concert_venue = '" + venueCombo.GetItemText(this.venueCombo.SelectedItem).ToString() + "' and concert_time = '" + dateTimePicker2.Text + "' AND concert_date = '" + dateTimePicker1.Text + "';";
            if (artistNameTextbox.Text == "" || venueCombo.SelectedIndex == -1 ||
            !priceTextbox.MaskFull)
            {
                MessageBox.Show("Please enter all the values");
            }
            else
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                        SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                        SqlCommand createTableCmd = new SqlCommand(createArtistTable, conn); SqlCommand tableEntriesCmd = new SqlCommand(tableEntries, conn); SqlCommand findDuplicateEntries = new
                        SqlCommand(duplicateEntriesQuery, conn);
                        insertCmd.CommandType = CommandType.Text;
                        createTableCmd.CommandType = CommandType.Text;
                        SqlDataReader dr = findDuplicateEntries.ExecuteReader();
                        if (dr.Read())
                        {
                            dr.Close();
                            MessageBox.Show("Whoops! A concert at " +
                            venueCombo.SelectedItem.ToString() + " on " + dateTimePicker1.Text + ", " + dateTimePicker2.Text + " already exists!");
                            artistNameTextbox.Text = "";
                            priceTextbox.Text = "";
                            venueCombo.SelectedIndex = -1;
                        }
                        else
                        {
                            dr.Close();
                            insertCmd.ExecuteNonQuery();
                            updatesLabel.Visible = true;
                            updatesLabel.Text = "Record added successfully!";
                            createTableCmd.ExecuteNonQuery();
                            //ADD 50 SEATS(UNBOOKED) 
                            for (int i = 0; i < 50; i++)
                            {
                                tableEntriesCmd.ExecuteNonQuery();
                            }
                            //DISPLAY ONLY RECENTLY ADDED CONCERT ENTRY 
                            string showRecentEntry = " SELECT * FROM concert_details WHERE concert_artist ='" + artistNameTextbox.Text + "'";
                            da = new SqlDataAdapter(showRecentEntry, conn);
                            da.Fill(dt);
                            concertsDataGridView.DataSource = dt;
                            //CLEAR THE TEXTBOXES 
                            artistNameTextbox.Text = "";
                            priceTextbox.Text = "";
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
        private void concertsDataGridView_CellContentClick(object sender,
        DataGridViewCellEventArgs e)
        {
            concertsDataGridView.Columns[1].ReadOnly = true;
            concertsDataGridView.Columns[0].ReadOnly = true;
            concertsDataGridView.Columns[2].ReadOnly = true;
        }
        private void updateBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                    scb = new SqlCommandBuilder(da);
                    da.Update(dt);
                    updatesLabel.Visible = true;
                    updatesLabel.Text = "Your updates were added successfully!";
                    artistNameTextbox.Text = "";
                    priceTextbox.Text = "";
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
        private void deleteBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                    //ITERATE THROUGH EACH ROW, CHECK IF A ROW IS SELECTED AND THEN ITERATE OVER THE CELLS AND GET THE ARTIST NAME (INDEX) 
                    foreach (DataGridViewRow row in concertsDataGridView.Rows)
                    {
                        if (row.Selected)
                        {
                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                int indexName = cell.ColumnIndex;
                                if (indexName == 1)
                                {
                                    string nameCellValue = cell.Value.ToString();
                                    string nameCellValueTable = nameCellValue.Replace(" ", String.Empty); //DROP THE TABLE WITH ASSOCIATED ARTIST NAME 
                                    string dropTableQuery = "DROP TABLE " + nameCellValueTable + ";";
                                    SqlCommand dropTableCmd = new SqlCommand(dropTableQuery,
                                    conn);
                                    dropTableCmd.CommandType = CommandType.Text;
                                    dropTableCmd.ExecuteNonQuery();
                                    //DELETE ARTIST ENTRY FROM DB 
                                    string deleteQuery = "DELETE FROM concert_details WHERE 
                                concert_artist = '" + nameCellValue + "'; "; 
                                SqlCommand deleteConcertEntryCmd = new SqlCommand(deleteQuery, conn);
                                    deleteConcertEntryCmd.CommandType = CommandType.Text;
                                    deleteConcertEntryCmd.ExecuteNonQuery();
                                    updatesLabel.Text = "Record deleted sucessfully!";
                                }
                            }
                        }
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
            foreach (DataGridViewRow item in this.concertsDataGridView.SelectedRows)
            {
                //REMOVE SELECTED ROW FROM DATAGRIDVIEW 
                concertsDataGridView.Rows.RemoveAt(item.Index);
            }
        }
    }
}
