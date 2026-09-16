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
    public partial class CheckIn : Form
    {
        OleDbConnection conn;
        OleDbCommand cmd;
        OleDbDataAdapter adapter;
        DataTable dt;

        public CheckIn()
        {
            InitializeComponent();
        }

        String admin = "";

        public CheckIn(String name)
        {
            InitializeComponent();
            lblUser.Text = name;
            GetImage();
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

        void GetCustomers()
        {
            conn = new OleDbConnection("Provider=Microsoft.ACE.OleDb.16.0; Data Source=hotel.mdb");
            dt = new DataTable();
            adapter = new OleDbDataAdapter("SELECT ID,Name,NIC FROM Customers", conn);
            conn.Open();
            adapter.Fill(dt);
            dgvCustomers.DataSource = dt;
            conn.Close();
        }

        void Clear()
        {
            dgvCustomers.ClearSelection();
            txtCID.Text = "";
            txtName.Text = "";
            txtNIC.Text = "";
            txtSearch.Text = "";
            cmbRoomType.SelectedIndex = 0;
            cmbBed.SelectedIndex = 0;
            cmbRoomNo.Text = "";
        }

        void GetRoomNo()
        {
            conn = new OleDbConnection("Provider=Microsoft.ACE.OleDb.16.0; Data Source=hotel.mdb");
            dt = new DataTable();
            adapter = new OleDbDataAdapter("SELECT * FROM Rooms WHERE status='Available' ", conn);
            conn.Open();
            adapter.Fill(dt);
            cmbRoomNo.DataSource = dt;
            cmbRoomNo.DisplayMember = "RoomNo";
            conn.Close();
        }

        void SelectRoomNo()
        {
            conn = new OleDbConnection("Provider=Microsoft.ACE.OleDb.16.0; Data Source=hotel.mdb");
            dt = new DataTable();
            adapter = new OleDbDataAdapter("SELECT * FROM Rooms WHERE Bed='" + cmbBed.Text + "' AND " + 
                "RoomType ='" + cmbRoomType.Text + "' AND Status='Available' AND Booked='No' ", conn);
            conn.Open();
            adapter.Fill(dt);
            cmbRoomNo.DataSource = dt;
            cmbRoomNo.DisplayMember = "RoomNo";
            conn.Close();
        }

        void GetPrice()
        {
            conn = new OleDbConnection("Provider=Microsoft.ACE.OleDb.16.0; Data Source=hotel.mdb");
            dt = new DataTable();
            adapter = new OleDbDataAdapter("SELECT * FROM Rooms WHERE RoomNo='" + cmbRoomNo.Text + "' ", conn);
            conn.Open();
            adapter.Fill(dt);
            cmbPrice.DataSource = dt;
            cmbPrice.DisplayMember = "Price";
            conn.Close();
        }

        private void CheckIn_Load(object sender, EventArgs e)
        {
            cmbRoomType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBed.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRoomNo.DropDownStyle = ComboBoxStyle.DropDownList;
            
            GetCustomers();
            //GetRoomNo();

            cmbPrice.Visible = false;
            Clear();
        }

        private void dgvCustomers_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtCID.Text = dgvCustomers.CurrentRow.Cells[0].Value.ToString();
            txtName.Text = dgvCustomers.CurrentRow.Cells[1].Value.ToString();
            txtNIC.Text = dgvCustomers.CurrentRow.Cells[2].Value.ToString();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
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
                rg = new Register(admin, 1);
            }
            this.Hide();
            rg.Show();
        }

        private void cmbBed_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectRoomNo();
            //MessageBox.Show(cmbRoomNo.Text);
            if (cmbRoomNo.Text == "")
            {
                txtPrice.Text = "";
            }
            else
            {
                GetPrice();
                txtPrice.Text = cmbPrice.Text;
            }
            
        }

        private void cmbRoomType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectRoomNo();
            if (cmbRoomNo.Text == "")
            {
                txtPrice.Text = "";
            }
            else
            {
                GetPrice();
                txtPrice.Text = cmbPrice.Text;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            GetCustomers();
            DataView dv = dt.DefaultView;
            dv.RowFilter = "Name LIKE '%" + txtSearch.Text + "%'";
            dgvCustomers.DataSource = dv;
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            if (txtCID.Text == "" || cmbRoomNo.Text == "" || dtpCheckIn.Text == "")
            {
                MessageBox.Show("All the fields are required to be filled");
            }
            else
            {
                dtpCheckIn.CustomFormat = "MM-dd-yyyy";
                String query = "INSERT INTO Bookings (CustomerID,CustomerName,RoomNo,CheckInDate,CheckOut) VALUES" +
                "(@cID,@name,@roomNo,@checkInDt,@checkOut) ";
                cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@cID", txtCID.Text);
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@roomNo", cmbRoomNo.Text);
                cmd.Parameters.AddWithValue("@checkInDt", dtpCheckIn.Text);
                cmd.Parameters.AddWithValue("@checkOut", "No");

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                query = "UPDATE Rooms SET Booked=@booked WHERE RoomNo=@roomNo";
                cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@booked", "Yes");
                cmd.Parameters.AddWithValue("@roomNo", cmbRoomNo.Text);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Customer Check-In Successful!");
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
