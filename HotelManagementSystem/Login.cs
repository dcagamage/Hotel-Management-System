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
    public partial class Login : Form
    {
        OleDbConnection conn;
        OleDbDataAdapter adapter;
        DataTable dt;

        public Login()
        {
            InitializeComponent();
        }

        private void checkLogin()
        {
            conn = new OleDbConnection("Provider=Microsoft.ACE.OleDb.16.0; Data Source=hotel.mdb");
            dt = new DataTable();
            adapter = new OleDbDataAdapter("SELECT * FROM Admins WHERE Username='" + txtUsername.Text + "' AND Password ='" + txtPassword.Text + "' ", conn);
            conn.Open();
            adapter.Fill(dt);
            if (dt.Rows.Count <= 0)
            {
                MessageBox.Show("Incorrect username or password");
            }
            else if (dt.Rows.Count > 0)
            {
//                Dashboard ds = new Dashboard();
                Dashboard ds = new Dashboard(txtUsername.Text);
                this.Hide();
                ds.Show();
                
//                MessageBox.Show("Login Successful");
            }
            conn.Close();
        }

        private void setVisible()
        {
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            button6.Visible = false;
            button7.Visible = false;
            button8.Visible = false;
            button9.Visible = false;
            textBox1.Visible = false;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "" || txtPassword.Text == "")
            {
                MessageBox.Show("Both username and password are required");
            }
            /*else if (txtUsername.Text == "Denuka" && txtPassword.Text == "qwerty123")
            {
                Dashboard ds = new Dashboard();
                this.Hide();
                ds.Show();
            }*/
            else
            {
                checkLogin();
                //MessageBox.Show("Incorrect username or password");
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            setVisible();
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dashboard ds = new Dashboard();
            this.Hide();
            ds.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Rooms rm = new Rooms();
            this.Hide();
            rm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Register reg = new Register();
            this.Hide();
            reg.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Customers ct = new Customers();
            this.Hide();
            ct.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Bookings bk = new Bookings();
            this.Hide();
            bk.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Admins ad = new Admins();
            this.Hide();
            ad.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            CheckIn ci = new CheckIn();
            this.Hide();
            ci.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            CheckOut co = new CheckOut();
            this.Hide();
            co.Show();
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            if(textBox1.Text == "")
            {
                Reports.reportViewer rpt = new Reports.reportViewer();
                rpt.Show();
            } else
            {
                int num = Int32.Parse(textBox1.Text);
                Reports.reportViewer rpt = new Reports.reportViewer(num);
                rpt.Show();
            }
            
        }
    }
}
