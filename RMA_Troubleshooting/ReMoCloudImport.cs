using System;
using System.Data;
using System.Windows.Forms;

namespace RMA_Troubleshooting
{
    public partial class ReMoCloudImport : Form
    {
        public ReMoCloudImport()
        {
            InitializeComponent();
        }

        private void btn_Qry_Click(object sender, EventArgs e)
        {
            string ErrMSG = "";
            DataTable dt;
            dt = Myfunc.GetReMoCloudData("FJ1193A003598");

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                string sResult = "";
                DataRow SnRow = dt.Rows[i];
                sResult = Myfunc.callWebServiceBody(SnRow["PKEY"].ToString(), SnRow["AD_ORDERNO"].ToString(), SnRow["AD_COMPANYNAME"].ToString(), SnRow["AD_EMAIL"].ToString(), SnRow["AD_TECHNICALCONTACT"].ToString(),
                                                    SnRow["AD_PHONE"].ToString(), SnRow["AD_STREETADDRESS"].ToString(), SnRow["SERIAL"].ToString(), SnRow["EXPORT_WARRANTY_DATE"].ToString(), SnRow["OGB04"].ToString());


                string[] tResut_body = sResult.Split(':');
                if (tResut_body[0] != "OK")
                {
                    //throw new Exception("Calling detail webService has some error ,please contact with MIS");
                    ErrMSG += tResut_body[1];
                }
            }

            if (ErrMSG != string.Empty)
            {
                MessageBox.Show(ErrMSG);
            }
            else
            {
                MessageBox.Show("OK");
            }

        }
    }
}
