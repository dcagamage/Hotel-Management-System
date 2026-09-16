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
    public partial class Admins : Form
    {
        OleDbConnection conn;
        OleDbCommand cmd;
        OleDbDataAdapter adapter;
        DataTable dt;

        public Admins()
        {
            InitializeComponent();
        }

        String admin = "";

        public Admins(String name)
        {
            InitializeComponent();
            lblUser.Text = name;
            this.admin = name;
            try
            {
                GetImage();
            }
            catch (Exception e)
            {
                //MessageBox.Show("" + e);
                MessageBox.Show("Location of the icon image has been changed", "Error displaying Icon");
            }
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
                pictureBox1.Image = Image.FromFile(img);
            }
            else
                MessageBox.Show("Error");

            conn.Close();
        }

        void GetAdmins()
        {
            conn = new OleDbConnection("Provider=Microsoft.ACE.OleDb.16.0; Data Source=hotel.mdb");
            dt = new DataTable();
//            adapter = new OleDbDataAdapter("SELECT ID,Username,Password FROM Admins", conn);
            adapter = new OleDbDataAdapter("SELECT * FROM Admins", conn);
            conn.Open();
            adapter.Fill(dt);
            dgvAdmins.DataSource = dt;
            conn.Close();
        }

        void Clear()
        {
            dgvAdmins.ClearSelection();
            lblID.Text = "ID";
            txtUsername.Text = "";
            txtPassword.Text = "";
            //pictureBox2.Image = null;
            try
            {
                label8.Text = "D:\\IDM Computing\\C# programming\\HotelManagementSystem\\User Icons\\user.png";
                pictureBox2.Image = Image.FromFile(label8.Text);
            }
            catch (Exception e)
            {
                //MessageBox.Show("" + e);
                MessageBox.Show("Location of the icon image has been changed [Admins.cs : line 88]");
            }

            btnBrowse.Text = "Browse";
            btnChange.Visible = false;
        }

        private void Admins_Load(object sender, EventArgs e)
        {
            GetAdmins();
            Clear();
            //label8.Visible = true;
        }

        private void dgvAdmins_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            lblID.Text = dgvAdmins.CurrentRow.Cells[0].Value.ToString();
            txtUsername.Text = dgvAdmins.CurrentRow.Cells[1].Value.ToString();
            txtPassword.Text = dgvAdmins.CurrentRow.Cells[2].Value.ToString();
            label8.Text = dgvAdmins.CurrentRow.Cells[3].Value.ToString();
            try
            {
                pictureBox2.Image = Image.FromFile(label8.Text);
            }
            catch (Exception e1)
            {
                //MessageBox.Show("" + e);
                MessageBox.Show("Location of the icon image has been changed");
            }
            //btnBrowse.Visible = false;
            btnBrowse.Text = "Change";
            btnChange.Visible = true;


        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "" || txtPassword.Text == "" )
            {
                MessageBox.Show("All the fields should be filled");
            }
            else
            {
                String query = "INSERT INTO Admins ([Username],[Password],[image]) VALUES" +
                "(@username,@password,@image)";
                cmd = new OleDbCommand(query, conn);

                cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                cmd.Parameters.AddWithValue("@password", txtPassword.Text);
                cmd.Parameters.AddWithValue("@image", label8.Text);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("New Admin Added.");
                GetAdmins();
                Clear();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lblID.Text == "ID" || txtUsername.Text == "" || txtPassword.Text == "")
            {
                MessageBox.Show("Select the record from the table");
            }
            else
            {
                String query = "DELETE FROM Admins WHERE ID=@id";
                cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(lblID.Text));

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Admin Removed.");
                GetAdmins();
                Clear();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            if (txtPassword.PasswordChar == '*')
            {
                txtPassword.PasswordChar = '\0';
                btnShow.BackColor = Color.Silver;
            }
            else if (txtPassword.PasswordChar == '\0')
            {
                txtPassword.PasswordChar = '*';
                btnShow.BackColor = Color.White;
            }
            txtPassword.Focus();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            DataView dv = dt.DefaultView;
            dv.RowFilter = "Username LIKE '%" + txtSearch.Text + "%'";
            dgvAdmins.DataSource = dv;
        }

        private void btnDashboard_Click_1(object sender, EventArgs e)
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

        private void btnRooms_Click_1(object sender, EventArgs e)
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

        private void btnCustomers_Click_1(object sender, EventArgs e)
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

        private void btnBookings_Click_1(object sender, EventArgs e)
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

        private void btnAdmins_Click_1(object sender, EventArgs e)
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
            this.Close();
            ad.Show();
        }

        private void btnLogout_Click_1(object sender, EventArgs e)
        {
            Login lg = new Login();
            this.Dispose();
            lg.Show();
        }

        private void btnExit_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimize_Click_1(object sender, EventArgs e)
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

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog img1 = new OpenFileDialog();
            img1.Filter = " choose image(*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif";

            if (img1.ShowDialog() == DialogResult.OK)
            {
                label8.Text = img1.FileName.ToString();
                pictureBox2.Image = Image.FromFile(img1.FileName);
            }

        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            String query = "UPDATE Admins SET [image]=@image WHERE ID=@id";
            cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("@image", label8.Text);
            cmd.Parameters.AddWithValue("@id", Convert.ToInt32(lblID.Text));

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            MessageBox.Show("Image Updated.");
            GetAdmins();
            Clear();

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
