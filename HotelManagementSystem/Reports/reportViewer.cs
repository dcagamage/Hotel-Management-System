using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelManagementSystem.Reports
{
    public partial class reportViewer : Form
    {
        public reportViewer()
        {
            InitializeComponent();
        }

        int num = 0;

        public reportViewer(int num)
        {
            InitializeComponent();
            this.num = num;
        }

        String des_path;

        private void reportViewer1_Load(object sender, EventArgs e)
        {
            des_path = "D:\\IDM Computing\\C# programming\\HotelManagementSystem\\HotelManagementSystem\\Reports";
            //des_path = Application.StartupPath;
            ReportDocument rpd;
            //MessageBox.Show(des_path);
            if (num == 1)
            {
                rpd = new ReportDocument();
                rpd.Load(des_path + "\\Rooms1.rpt");
                crystalReportViewer1.ReportSource = rpd;
            }
            else if (num == 2)
            {
                rpd = new ReportDocument();
                rpd.Load(des_path + "\\Customers1.rpt");
                crystalReportViewer1.ReportSource = rpd;
            }
            else if (num == 3)
            {
                rpd = new ReportDocument();
                rpd.Load(des_path + "\\Bookings1.rpt");
                crystalReportViewer1.ReportSource = rpd;
            }
            else if (num == 4)
            {
                rpd = new ReportDocument();
                rpd.Load(des_path + "\\Admins1.rpt");
                crystalReportViewer1.ReportSource = rpd;
            }
        }


        private bool isFormClosing = false;
        private void reportViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            isFormClosing = true;
        }

        private void reportViewer_Deactivate(object sender, EventArgs e)
        {
            if (!isFormClosing)
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to close the currently viewing report?", "Close Report", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    this.Close();
                }
            }

        }
    }
}
