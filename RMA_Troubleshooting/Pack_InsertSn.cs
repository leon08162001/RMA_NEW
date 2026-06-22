using Oracle.DataAccess.Client;
using System;
using System.Data;
using System.Windows.Forms;

namespace RMA_Troubleshooting
{
    public partial class Pack_InsertSn : Form
    {
        private string CipherlabDB = "Data Source=192.168.7.20/TOPPROD;Persist Security Info=True;User ID=cipherlab;Password=cip2us91ab";
        private string SerialDB = "Data Source=192.168.1.3/TOPPROD;Persist Security Info=True;User ID=extsys;Password=EXTSYS";
        public Pack_InsertSn()
        {
            InitializeComponent();
        }

        private void btn_Insert_Click(object sender, EventArgs e)
        {
            string ogb01 = "AXGA-180400091";//出通單號
            string PackNo = "ASXA-180400108";//包裝單號
            DataTable dt_Serial = new DataTable();
            //包裝箱號
            DataTable dt_Box = new DataTable();
            string strSQL = @"SELECT * FROM PMS_PACKBOX WHERE pack_no = '" + PackNo + "' AND CON_STATUS = 'N'";
            using (OracleConnection Conn = new OracleConnection(CipherlabDB))
            {
                Conn.Open();
                OracleCommand cmd = new OracleCommand(strSQL, Conn);
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                da.Fill(dt_Box);
            }
            dataGridView1.DataSource = dt_Box;

            string[] arr_BoxNo = tb_BoxNo.Text.Trim().Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None); //收集虛擬箱號

            if (dt_Box.Rows.Count != arr_BoxNo.Length)
            {
                MessageBox.Show("數量不對等");
                return;
            }
            strSQL = @"SELECT ITEM_SN
                          FROM spp_item a, spp_item_sn b
                         WHERE     a.item_id = b.item_id
                               AND a.pack_id = :pack_id
                               AND a.prd_id = 'BPOWER0000086'
                               AND a.ACTI = 'Y'
                               AND b.ACTI = 'Y'
                               AND b.PRD_ID = 'KT98052401SU3'
                        GROUP BY ITEM_SN";

            int Box_Cnt = 0;
            int Serial_Cnt = 0;
            using (OracleConnection Conn = new OracleConnection(SerialDB)) //收集之序號
            {
                Conn.Open();
                foreach (string BoxNo in arr_BoxNo)
                {
                    OracleCommand cmd = new OracleCommand(strSQL, Conn);
                    cmd.Parameters.Add(new OracleParameter("pack_id", OracleDbType.NVarchar2)).Value = BoxNo;
                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    da.Fill(dt_Serial);

                    Serial_Cnt += dt_Serial.Rows.Count;
                    dt_Serial.Clear();
                }
            }

            foreach (DataRow dr in dt_Box.Rows)
            {
                Box_Cnt += Convert.ToInt32(dr["BOX_QTY"].ToString());
            }

            if (Serial_Cnt != Box_Cnt)
            {
                MessageBox.Show("收集序號數量與包裝數量不符");
                return;
            }

            int iBox = 0;
            foreach (DataRow dr in dt_Box.Rows)
            {
                using (OracleConnection Conn = new OracleConnection(SerialDB))
                {
                    Conn.Open();
                    OracleCommand cmd = new OracleCommand(strSQL, Conn);
                    cmd.Parameters.Add(new OracleParameter("pack_id", OracleDbType.NVarchar2)).Value = arr_BoxNo[iBox];
                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    da.Fill(dt_Serial);
                }

                using (OracleConnection Conn = new OracleConnection(CipherlabDB))
                {
                    Conn.Open();
                    foreach (DataRow dr_Serial in dt_Serial.Rows)
                    {
                        OracleCommand cmd = new OracleCommand("sp_pxmr420_TxPackOgbSn", Conn);
                        cmd.Parameters.Add(new OracleParameter("vBoxNo", OracleDbType.NVarchar2)).Value = dr["BOX_NO"].ToString();
                        cmd.Parameters.Add(new OracleParameter("vOgb01", OracleDbType.NVarchar2)).Value = ogb01;
                        cmd.Parameters.Add(new OracleParameter("vOgb03", OracleDbType.NVarchar2)).Value = "1";
                        cmd.Parameters.Add(new OracleParameter("vSN", OracleDbType.NVarchar2)).Value = dr_Serial["ITEM_SN"].ToString();
                        cmd.Parameters.Add(new OracleParameter("vUser", OracleDbType.NVarchar2)).Value = "jay.gong";
                        cmd.CommandType = CommandType.StoredProcedure;

                        OracleParameter Param = cmd.Parameters.Add("vResult", OracleDbType.NVarchar2, 250);
                        Param.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();

                    }
                }

                dt_Serial.Clear();
                iBox++;
            }




        }

    }
}
