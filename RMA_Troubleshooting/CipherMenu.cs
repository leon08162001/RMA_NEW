using System;
using System.Windows.Forms;

namespace RMA_Troubleshooting
{
    public partial class CipherMenu : Form
    {
        bool AutoRun = false;
        private Form frmNow;
        public CipherMenu(string[] args)
        {
            InitializeComponent();

            Myfunc.RMAConnected();

            if (args.Length > 0)
            {
                if (args[0].ToUpper().Trim() == "AUTO")
                    AutoRun = true;
            }

            if (AutoRun)
            {
                Insert_Repair_Price_Click(null, null);
            }
        }

        private void Return_Quote_Click(object sender, EventArgs e)
        {
            frmNow = new Return_Quote();
            frmNow.Show();
        }

        private void Insert_Repair_Price_Click(object sender, EventArgs e)
        {
            string[] args = { "" };
            //string[] args = { "" };
            frmNow = new Insert_Repair_Price(args);
            frmNow.Show();
        }

        private void Generate_INVOICE_AR_Click(object sender, EventArgs e)
        {
            frmNow = new Generate_INVOICE_AR();
            frmNow.Show();
        }

        private void Pack_InsertSn_Click(object sender, EventArgs e)
        {
            frmNow = new Pack_InsertSn();
            frmNow.Show();
        }

        private void Pack_ExcelInsertSn_Click(object sender, EventArgs e)
        {
            frmNow = new Pack_ExcelInsertSn();
            frmNow.Show();
        }

        private void Delete_Sales_Quoted_Click(object sender, EventArgs e)
        {
            frmNow = new Delete_Sales_Quoted();
            frmNow.Show();
        }

        private void Generate_Quoted_Click(object sender, EventArgs e)
        {
            frmNow = new Generate_Quoted();
            frmNow.Show();
        }

        private void reImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmNow = new ReMoCloudImport();
            frmNow.Show();

        }
    }
}
