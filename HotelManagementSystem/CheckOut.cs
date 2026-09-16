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
    public partial class CheckOut : Form
    {
        OleDbConnection conn;
        OleDbCommand cmd;
        OleDbDataAdapter adapter;
        DataTable dt;

        public CheckOut()
        {
            InitializeComponent();
        }

        String admin = "";

        public CheckOut(String name)
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

        void GetRoom()
        {
            conn = new OleDbConnection("Provider=Microsoft.ACE.OleDb.16.0; Data Source=hotel.mdb");
            dt = new DataTable();
            adapter = new OleDbDataAdapter("SELECT * FROM Rooms WHERE RoomNo='" + txtRoomNo.Text + "'", conn);
            conn.Open();
            adapter.Fill(dt);
            cmbPrice.DataSource = dt;
            cmbPrice.DisplayMember = "Price";
            cmbDC.DataSource = dt;
            cmbDC.DisplayMember = "DailyCharge";
            conn.Close();
        }

        double NumberOfDays()
        {
            DateTime StartDate = dtpCheckIn.Value;
            DateTime EndDate = dtpCheckOut.Value;

            double days = (EndDate - StartDate).TotalDays;

            return days;
        }

        void Clear()
        {
            txtName.Text = "";
            txtCname.Text = "";
            txtRoomNo.Text = "";
            txtPrice.Text = "";
            dtpCheckIn.CustomFormat = " ";
            btnProceed.Enabled = false;
            dtpCheckOut.Value = DateTime.Now;
            dgvBookings.ClearSelection();
        }

        private void CheckOut_Load(object sender, EventArgs e)
        {
            GetBookings();
            Clear();
            dtpCheckIn.Enabled = false;
            dtpCheckOut.Enabled = false;
            btnProceed.Enabled = false;
            cmbPrice.Visible = false;
            cmbDC.Visible = false;
        }

        private void dgvBookings_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            String checkOut = dgvBookings.CurrentRow.Cells[7].Value.ToString();
            if (checkOut.Equals("Yes"))
            {
                txtName.Text = "";
                txtCname.Text = "";
                txtRoomNo.Text = "";
                txtPrice.Text = "";
                dtpCheckIn.CustomFormat = " ";
                dtpCheckOut.Value = DateTime.Now;
            }
            else
            {
                lblBookingID.Text = dgvBookings.CurrentRow.Cells[0].Value.ToString();
                txtCname.Text = dgvBookings.CurrentRow.Cells[2].Value.ToString();
                txtRoomNo.Text = dgvBookings.CurrentRow.Cells[3].Value.ToString();
                dtpCheckIn.CustomFormat = "dd-MM-yyyy";
                dtpCheckIn.Text = dgvBookings.CurrentRow.Cells[4].Value.ToString();
                txtPrice.Text = dgvBookings.CurrentRow.Cells[6].Value.ToString();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            GetBookings();
            Clear();
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            dtpCheckOut.CustomFormat = "MM-dd-yyyy";
            String query = "UPDATE Bookings SET CheckoutDate=@checkoutDT,Price=@price,CheckOut=@checkOut WHERE ID=@bookingID";
            cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("@checkoutDT", dtpCheckOut.Text);
            cmd.Parameters.AddWithValue("@price", txtPrice.Text);
            cmd.Parameters.AddWithValue("@checkOut", "Yes");
            cmd.Parameters.AddWithValue("@bookingID", lblBookingID.Text);

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

            MessageBox.Show("Customer Check-Out Successful!");
            GetBookings();
            Clear();
        }

        private void btnGetPrice_Click(object sender, EventArgs e)
        {
            if (txtCname.Text == "" || txtRoomNo.Text == "")
            {
                MessageBox.Show("Select the Check-In Record");
            }
            else
            {
                double days = NumberOfDays();
                int count = Convert.ToInt32(days);
                //MessageBox.Show(days + ", " + count);

                if ((count - days) < 0)
                {
                    count = count + 1;
                    //MessageBox.Show(days + ", " + count);
                }
                
                GetRoom();

                int price = Int32.Parse(cmbPrice.Text);
                int dailyCharge = Int32.Parse(cmbDC.Text);

                if (count == 1 || count == 2)
                {
                    txtPrice.Text = price.ToString();
                }
                else
                {
                    int newPrice = price + (dailyCharge * (count - 2));
                    //MessageBox.Show("" + newPrice);
                    txtPrice.Text = newPrice.ToString();
                }
                
                btnProceed.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataView dv = dt.DefaultView;
            dv.RowFilter = "CustomerName LIKE '%" + txtName.Text + "%'";
            dgvBookings.DataSource = dv;
            //txtName.Focus();
        }

        private void txtName_Enter(object sender, EventArgs e)
        {
            if (txtName.Text == "")
            {
                GetBookings();
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
