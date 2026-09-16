using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace HotelManagementSystem
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        String admin = "";

        public Dashboard(String name)
        {
            InitializeComponent();
            lblUser.Text = name;
            this.admin = name;
            try
            {
                GetImage();
            }
            catch (Exception e) {
                //MessageBox.Show("" + e);
                MessageBox.Show("Location of the icon image has been changed; Try changing the icon through the 'Admin' interface","Error displaying Icon");
//                Database is located at "HotelManagementSystem\HotelManagementSystem\bin\x64\Debug\hotel.mdb"
            }
        }

/*        String getName()
        {
            return admin;
        }
 */

        void GetImage()
        {
            OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OleDb.16.0; Data Source=hotel.mdb");

            conn.Open();
            OleDbCommand cmd = new OleDbCommand("SELECT image FROM Admins WHERE Username=@name", conn);
            cmd.Parameters.AddWithValue("name", lblUser.Text);

            OleDbDataReader reader;
            reader = cmd.ExecuteReader();

            String img;
            if (reader.Read())
            {
                img = reader["image"].ToString();
                pictureBox1.Image = Image.FromFile(img);
            }
            else
                MessageBox.Show("Error");

            conn.Close();

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Login lg = new Login();
            this.Dispose();
            lg.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            Dashboard ds;
            if (admin == "")
            {
                ds = new Dashboard();
            }
            else
            {
                ds = new Dashboard(admin);
            }

            this.Close();
            ds.Show();
        }

        private void btnRooms_Click(object sender, EventArgs e)
        {
            Rooms rm;
            if (admin == "")
            {
                rm = new Rooms();
            }
            else
            {
                rm = new Rooms(admin);
            }
            //Rooms rm = new Rooms();
            this.Hide();
            rm.Show();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            Register rg;
            if (admin == "")
            {
                rg = new Register();
            }
            else
            {
                rg = new Register(admin);
            }
            //Register reg = new Register();
            this.Hide();
            rg.Show();
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            Customers ct;
            if (admin == "")
            {
                ct = new Customers();
            }
            else
            {
                ct = new Customers(admin);
            }
            //Customers ct = new Customers();
            this.Hide();
            ct.Show();
        }

        private void btnBookings_Click(object sender, EventArgs e)
        {
            Bookings bk;
            if (admin == "")
            {
                bk = new Bookings();
            }
            else
            {
                bk = new Bookings(admin);
            }
            //Bookings bk = new Bookings();
            this.Hide();
            bk.Show();
        }

        private void btnAdmins_Click(object sender, EventArgs e)
        {
            Admins ad;
            if (admin == "")
            {
                ad = new Admins();
            }
            else
            {
                ad = new Admins(admin);
            }
            //Admins ad = new Admins();
            this.Hide();
            ad.Show();
        }

        private void btnCheckin_Click(object sender, EventArgs e)
        {
            CheckIn ci;
            if (admin == "")
            {
                ci = new CheckIn();
            }
            else
            {
                ci = new CheckIn(admin);
            }
            //CheckIn ci = new CheckIn();
            this.Hide();
            ci.Show();
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            CheckOut co;
            if (admin == "")
            {
                co = new CheckOut();
            }
            else
            {
                co = new CheckOut(admin);
            }
            //CheckOut co = new CheckOut();
            this.Hide();
            co.Show();
        }

        private void btnExit_MouseLeave(object sender, EventArgs e)
        {
            btnExit.BackColor = Color.Navy;
        }

        private void btnExit_MouseEnter(object sender, EventArgs e)
        {
            btnExit.BackColor = Color.Red;
        }

        private void btnMinimize_MouseEnter(object sender, EventArgs e)
        {
            btnMinimize.BackColor = Color.DarkGray;
        }

        private void btnMinimize_MouseLeave(object sender, EventArgs e)
        {
            btnMinimize.BackColor = Color.Navy;
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {

        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            Reports.HomeReport hr;
            if (admin == "")
            {
                hr = new Reports.HomeReport();
                this.Hide(); 
                hr.Show();
            }
            else
            {
                hr = new Reports.HomeReport(admin);
                this.Hide(); 
                hr.Show();
            }     
        }

    }
}
