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
    public partial class Register : Form
    {
        OleDbConnection conn;
        OleDbCommand cmd;
        OleDbDataAdapter adapter;

        public Register()
        {
            InitializeComponent();
        }

        String admin = "";
        int check = 0;

        public Register(String name)
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

        public Register(String name, int check)
        {
            InitializeComponent();
            lblUser.Text = name;
            btnReg.Text = "Go to Check-in";
            GetImage();
            this.admin = name;
            this.check = check;
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

        void GetCustomers()
        {
            conn = new OleDbConnection("Provider=Microsoft.ACE.OleDb.16.0; Data Source=hotel.mdb");
            adapter = new OleDbDataAdapter("SELECT * FROM Customers", conn);
            conn.Open();
            conn.Close();
        }

        void Clear()
        {
            txtName.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
            dtpDOB.Text = "01-01-2000";
            txtCity.Text = "";
            txtCountry.Text = "";
            txtNIC.Text = "";
            cmbGender.SelectedIndex = 0;
        }

        private void Register_Load(object sender, EventArgs e)
        {
            cmbGender.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbGender.SelectedIndex = 0;
            dtpDOB.Text = "01-01-2000";
            GetCustomers();
        }

        private void btnReg_Click(object sender, EventArgs e)
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

                if(check == 0)
                {
                    MessageBox.Show("Customer Registration Successful!.");
                    Clear();
                    GetCustomers();
                } 
                else 
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
                    this.Hide();
                    ci.Show();
                }
                
            }
        }

        private void txtPhone_Leave(object sender, EventArgs e)
        {
            String num = txtPhone.Text;
            int x = 0;
            //int x = Int32.Parse(txtPhone.Text);
            //MessageBox.Show("numbers : " + x);

            if (!int.TryParse(num, out x))
            {
                MessageBox.Show("The phone number mustn't contain any letters");
                txtPhone.Clear();
                txtPhone.Focus();
            }
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            //e.Handled = !char.IsDigit(e.KeyChar);
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
