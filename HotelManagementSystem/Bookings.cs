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
    public partial class Bookings : Form
    {
        OleDbConnection conn;
        OleDbCommand cmd;
        OleDbDataAdapter adapter;
        DataTable dt;

        public Bookings()
        {
            InitializeComponent();
        }

        String admin = "";

        public Bookings(String name)
        {
            InitializeComponent();
            lblUser.Text = name;
            try
            {
                GetImage();
            }
            catch (Exception e)
            {
                //MessageBox.Show("" + e);
                MessageBox.Show("Location of the icon image has been changed", "Error displaying Icon");
            }
            this.admin = name;
        }

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
                pictureBox2.Image = Image.FromFile(img);
            }
            else
                MessageBox.Show("Error");

            conn.Close();
        }

        void GetBookings()
        {
            conn = new OleDbConnection("Provider=Microsoft.ACE.OleDb.16.0; Data Source=hotel.mdb");
            dt = new DataTable();
            adapter = new OleDbDataAdapter("SELECT * FROM Bookings", conn);
            conn.Open();
            adapter.Fill(dt);
            dgvBookings.DataSource = dt;
            conn.Close();
        }

        void GetCustomer()
        {
            conn = new OleDbConnection("Provider=Microsoft.ACE.OleDb.16.0; Data Source=hotel.mdb");
            dt = new DataTable();
            int cID = Convert.ToInt32(txtCID.Text);
            adapter = new OleDbDataAdapter("SELECT ID,Name,PhoneNo,Gender,NIC FROM Customers WHERE ID=" + cID, conn);
            conn.Open();
            adapter.Fill(dt);
            dgvCustomers.DataSource = dt;
            conn.Close();
        }

        void GetRoom()
        {
            conn = new OleDbConnection("Provider=Microsoft.ACE.OleDb.16.0; Data Source=hotel.mdb");
            dt = new DataTable();
            adapter = new OleDbDataAdapter("SELECT RoomNo,RoomType,Bed,Price,DailyCharge FROM Rooms WHERE RoomNo='"+txtRoomNo.Text+"' ", conn);
            conn.Open();
            adapter.Fill(dt);
            dgvRooms.DataSource = dt;
            conn.Close();
        }

        void Clear()
        {
            txtBID.Text = "";
            txtCID.Text = "";
            txtRoomNo.Text = "";
            dgvBookings.ClearSelection();
            dgvCustomers.DataSource = null;
            dgvRooms.DataSource = null;
        }

        private void Bookings_Load(object sender, EventArgs e)
        {
            GetBookings();

            Clear();
        }

        private void dgvBookings_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtBID.Text = dgvBookings.CurrentRow.Cells[0].Value.ToString();
            txtCID.Text = dgvBookings.CurrentRow.Cells[1].Value.ToString();
            txtRoomNo.Text = dgvBookings.CurrentRow.Cells[3].Value.ToString();
            GetCustomer();
            GetRoom();

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtBID.Text == "" || txtCID.Text == "" || txtRoomNo.Text == "")
            {
                MessageBox.Show("Select the Record that needed to be Deleted");
            }
            else
            {
                String query = "DELETE FROM Bookings WHERE ID=@id";
                cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtBID.Text));

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                query = "UPDATE Rooms SET Booked=@booked WHERE RoomNo=@roomNo";
                cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@booked", "No");
                cmd.Parameters.AddWithValue("@roomNo", txtRoomNo.Text);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Booking Deleted.");
                GetBookings();
                Clear();
            }
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
            this.Hide();
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
            this.Hide();
            rm.Show();
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
            this.Close();
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
            this.Hide();
            ad.Show();
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

        private void btnExit_MouseEnter(object sender, EventArgs e)
        {
            btnExit.BackColor = Color.Red;
        }

        private void btnExit_MouseLeave(object sender, EventArgs e)
        {
            btnExit.BackColor = Color.Navy;
        }

        private void btnMinimize_MouseEnter(object sender, EventArgs e)
        {
            btnMinimize.BackColor = Color.DarkGray;
        }

        private void btnMinimize_MouseLeave(object sender, EventArgs e)
        {
            btnMinimize.BackColor = Color.Navy;
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
