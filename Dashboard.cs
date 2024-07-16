using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
namespace Concert_ticket_booking_system_final
{
    public partial class Dashboard : Form
    {
        private Form activeForm;
        public Dashboard()
        {
            InitializeComponent();
        }
        private void openChildForms(Form childForm)
        {
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.Dock = DockStyle.Fill;
            this.desktopPanel.Controls.Add(childForm);
            this.desktopPanel.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }
        private void desktopPanel_Paint(object sender, PaintEventArgs e)
        {
        }
        private void concertsBtn_Click(object sender, EventArgs e)
        {
            openChildForms(new childForms.crud());
        }
        private void seatBookingBtn_Click(object sender, EventArgs e)
        {
            openChildForms(new childForms.seatBooking());
        }
        private void settingsBtn_Click(object sender, EventArgs e)
        {
            openChildForms(new childForms.settings());
        }
        private void Dashboard_Load(object sender, EventArgs e)
        {
            openChildForms(new childForms.welcome());
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            openChildForms(new childForms.welcome());
        }
        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
        }
    }
}
