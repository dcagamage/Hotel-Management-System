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
    public partial class Customers : Form
    {
        OleDbConnection conn;
        OleDbCommand cmd;
        OleDbDataAdapter adapter;
        DataTable dt;

        public Customers()
        {
            InitializeComponent();
        }

        String admin = "";

        public Customers(String name)
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

        void GetCustomers()
        {
            conn = new OleDbConnection("Provider=Microsoft.ACE.OleDb.16.0; Data Source=hotel.mdb");
            dt = new DataTable();
            adapter = new OleDbDataAdapter("SELECT * FROM Customers", conn);
            conn.Open();
            adapter.Fill(dt);
            dgvCustomers.DataSource = dt;
            conn.Close();
        }

        void Clear()
        {
            dgvCustomers.ClearSelection();
            txtName.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
            dtpDOB.Text = "01-01-2000";
            txtCity.Text = "";
            txtCountry.Text = "";
            txtNIC.Text = "";
            cmbGender.SelectedIndex = 0;
            lblID.Text = "0";
        }

        private void Customers_Load(object sender, EventArgs e)
        {
            cmbGender.DropDownStyle = ComboBoxStyle.DropDownList;
            lblID.Visible = false;

            GetCustomers();

            Clear();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();       
        }

        private void dgvCustomers_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            lblID.Text = dgvCustomers.CurrentRow.Cells[0].Value.ToString();
            txtName.Text = dgvCustomers.CurrentRow.Cells[1].Value.ToString();
            txtPhone.Text = dgvCustomers.CurrentRow.Cells[2].Value.ToString();
            txtEmail.Text = dgvCustomers.CurrentRow.Cells[3].Value.ToString();
            dtpDOB.Text = dgvCustomers.CurrentRow.Cells[4].Value.ToString();
            txtCity.Text = dgvCustomers.CurrentRow.Cells[5].Value.ToString();
            txtCountry.Text = dgvCustomers.CurrentRow.Cells[6].Value.ToString();
            cmbGender.Text = dgvCustomers.CurrentRow.Cells[7].Value.ToString();
            txtNIC.Text = dgvCustomers.CurrentRow.Cells[8].Value.ToString();
            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtPhone.Text == "" || txtEmail.Text == "" || txtCity.Text == "" || txtCountry.Text == "" || txtNIC.Text == "" || cmbGender.Text == "-Select-")
            {
                MessageBox.Show("All the fields are required to be filled");
            }
            else
            {
                String query = "INSERT INTO Customers (Name,PhoneNo,Email,DateOfBirth,City,Country,Gender,NIC) VALUES" +
                    "(@name,@phone,@email,@dob,@city,@country,@gender,@nic)";
                cmd = new OleDbCommand(query, conn);

                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
                cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@dob", dtpDOB.Text);
                cmd.Parameters.AddWithValue("@city", txtCity.Text);
                cmd.Parameters.AddWithValue("@country", txtCountry.Text);
                cmd.Parameters.AddWithValue("@gender", cmbGender.Text);
                cmd.Parameters.AddWithValue("@nic", txtNIC.Text);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Customer Added.");
                GetCustomers();
                Clear();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtPhone.Text == "" || txtEmail.Text == "" || txtCity.Text == "" || txtCountry.Text == "" || txtNIC.Text == "" || cmbGender.Text == "-Select-")
            {
                MessageBox.Show("Select the Customer that needed to be Edited");
            }
            else
            {
                String query = "UPDATE Customers SET " +
                    "Name=@name,PhoneNo=@phone,Email=@email,DateOfBirth=@dob,City=@city,Country=@country,Gender=@gender,NIC=@nic " +
                    "WHERE ID=@id";
                cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
                cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@dob", dtpDOB.Text);
                cmd.Parameters.AddWithValue("@city", txtCity.Text);
                cmd.Parameters.AddWithValue("@country", txtCountry.Text);
                cmd.Parameters.AddWithValue("@gender", cmbGender.Text);
                cmd.Parameters.AddWithValue("@nic", txtNIC.Text);
                cmd.Parameters.AddWithValue("@id", lblID.Text);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Customer Edited.");
                GetCustomers();
                Clear();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtPhone.Text == "" || txtEmail.Text == "" || txtCity.Text == "" || txtCountry.Text == "" || txtNIC.Text == "" || cmbGender.Text == "-Select-")
            {
                MessageBox.Show("Select the Customer that needed to be Removed");
            }
            else
            {
                String query = "DELETE FROM Customers WHERE ID=@id";
                cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(lblID.Text));

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Customer Removed.");
                GetCustomers();
                Clear();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            DataView dv = dt.DefaultView;
            dv.RowFilter = "Name LIKE '%" + txtSearch.Text + "%'";
            dgvCustomers.DataSource = dv;
        }

        private void txtPhone_Leave(object sender, EventArgs e)
        {
            String num = txtPhone.Text;
            int x = 0;

            if (!int.TryParse(num, out x))
            {
                MessageBox.Show("The phone number mustn't contain any letters");
                txtPhone.Clear();
                txtPhone.Focus();
            }
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
            this.Close();
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
            this.Hide();
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
