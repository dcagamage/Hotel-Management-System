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
    public partial class Rooms : Form
    {
        OleDbConnection conn;
        OleDbCommand cmd;
        OleDbDataAdapter adapter;
        DataTable dt;

        public Rooms()
        {
            InitializeComponent();
        }

        String admin = "";

        public Rooms(String name)
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

        void GetRooms()
        {
            conn = new OleDbConnection("Provider=Microsoft.ACE.OleDb.16.0; Data Source=hotel.mdb");
            dt = new DataTable();
            adapter = new OleDbDataAdapter("SELECT * FROM Rooms", conn);
            conn.Open();
            adapter.Fill(dt);
            dgvRooms.DataSource = dt;
            conn.Close();
        }

        void Clear()
        {
            dgvRooms.ClearSelection();
            txtRoomNo.Text = "";
            txtPrice.Text = "";
            txtDailyCharge.Text = "";
            cmbRoomType.SelectedIndex = 0;
            cmbBed.SelectedIndex = 0;
            cmbStatus.SelectedIndex = 0;
        }

        void RoomPrice()
        {
            String bed = cmbBed.Text;
            String roomType = cmbRoomType.Text;

            if (bed.Equals("Single") && roomType.Equals("Non-AC"))
            {
                txtPrice.Text = "5000";
                txtDailyCharge.Text = "500";
            }
            else if (bed.Equals("Single") && roomType.Equals("AC"))
            {
                txtPrice.Text = "6000";
                txtDailyCharge.Text = "750";
            }
            else if (bed.Equals("Double") && roomType.Equals("Non-AC"))
            {
                txtPrice.Text = "7000";
                txtDailyCharge.Text = "1000";
            }
            else if (bed.Equals("Double") && roomType.Equals("AC"))
            {
                txtPrice.Text = "8000";
                txtDailyCharge.Text = "1250";
            }
            else if (bed.Equals("Triple") && roomType.Equals("Non-AC"))
            {
                txtPrice.Text = "9000";
                txtDailyCharge.Text = "1500";
            }
            else if (bed.Equals("Triple") && roomType.Equals("AC"))
            {
                txtPrice.Text = "10000";
                txtDailyCharge.Text = "1750";

            }

        }

        private void Rooms_Load(object sender, EventArgs e)
        {
            cmbRoomType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBed.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;

            GetRooms();

            Clear();
            
        }

        private void dgvRooms_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            String booked = dgvRooms.CurrentRow.Cells[6].Value.ToString(); ;
            if (booked.Equals("Yes"))
            {
                txtRoomNo.Text = "";
                cmbRoomType.SelectedIndex = 0;
                cmbBed.SelectedIndex = 0;
                txtPrice.Text = "";
                txtDailyCharge.Text = "";
                cmbStatus.SelectedIndex = 0;
            }
            else
            {
                txtRoomNo.Text = dgvRooms.CurrentRow.Cells[0].Value.ToString();
                cmbRoomType.Text = dgvRooms.CurrentRow.Cells[1].Value.ToString();
                cmbBed.Text = dgvRooms.CurrentRow.Cells[2].Value.ToString();
                txtPrice.Text = dgvRooms.CurrentRow.Cells[3].Value.ToString();
                txtDailyCharge.Text = dgvRooms.CurrentRow.Cells[4].Value.ToString();
                cmbStatus.Text = dgvRooms.CurrentRow.Cells[5].Value.ToString();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtRoomNo.Text == "" || txtPrice.Text == "" || txtDailyCharge.Text == "" || cmbRoomType.Text == "-Select-" || cmbBed.Text == "-Select-" || cmbStatus.Text == "-Select-")
            {
                MessageBox.Show("All the fields are required to be filled");
            }
            else
            {
                String query = "INSERT INTO Rooms (RoomNo,RoomType,Bed,Price,Status,Booked) VALUES" +
                    "(@roomNo,@roomType,@bed,@price,@dailyCharge,@status,@booked)";
                cmd = new OleDbCommand(query, conn);

                cmd.Parameters.AddWithValue("@roomNo", txtRoomNo.Text);
                cmd.Parameters.AddWithValue("@roomType", cmbRoomType.Text);
                cmd.Parameters.AddWithValue("@bed", cmbBed.Text);
                cmd.Parameters.AddWithValue("@price", txtPrice.Text);
                cmd.Parameters.AddWithValue("@dailyCharge", txtDailyCharge.Text);
                cmd.Parameters.AddWithValue("@status", cmbStatus.Text);
                cmd.Parameters.AddWithValue("@booked", "No");

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Room Added.");
                GetRooms();
                Clear();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtRoomNo.Text == "" || txtPrice.Text == "" || txtDailyCharge.Text == "" || cmbRoomType.Text == "-Select-" || cmbBed.Text == "-Select-" || cmbStatus.Text == "-Select-")
            {
                MessageBox.Show("Select the Room that needed to be Updated");
            }
            else
            {
                String query = "UPDATE Rooms SET " +
                    "RoomType=@roomType,Bed=@bed,Price=@price,DailyCharge=@dailyCharge,Status=@status WHERE RoomNo=@roomNo";
                cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@roomType", cmbRoomType.Text);
                cmd.Parameters.AddWithValue("@bed", cmbBed.Text);
                cmd.Parameters.AddWithValue("@price", txtPrice.Text);
                cmd.Parameters.AddWithValue("@dailyCharge", txtDailyCharge.Text);
                cmd.Parameters.AddWithValue("@status", cmbStatus.Text);
                cmd.Parameters.AddWithValue("@roomNo", Convert.ToInt32(txtRoomNo.Text));

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Room Updated.");
                GetRooms();
                Clear();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtRoomNo.Text == "" || txtPrice.Text == "" || txtDailyCharge.Text == "" || cmbRoomType.Text == "-Select-" || cmbBed.Text == "-Select-" || cmbStatus.Text == "-Select-")
            {
                MessageBox.Show("Select the Room that needed to be Removed");
            }
            else
            {
                String query = "DELETE FROM Rooms WHERE RoomNo=@roomNo";
                cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@roomNo", Convert.ToInt32(txtRoomNo.Text));

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Room Removed.");
                GetRooms();
                Clear();
            }
        }

        private void cmbRoomType_SelectedIndexChanged(object sender, EventArgs e)
        {
            RoomPrice();
        }

        private void cmbBed_SelectedIndexChanged(object sender, EventArgs e)
        {
            RoomPrice();
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
            this.Close();
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
            //Admins ad = new Admins();
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
