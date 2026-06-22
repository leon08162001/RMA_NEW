using System;
using System.Data;
using System.Windows.Forms;

namespace RMA_Troubleshooting
{
    public partial class Generate_INVOICE_AR : Form
    {
        int curRow = -1;
        //private string RMADBLink = "Data Source=192.168.7.20/TOPPROD;Persist Security Info=True;User ID=RMA;Password=4321rma";
        public Generate_INVOICE_AR()
        {
            InitializeComponent();

        }

        private void btn_Search_Click(object sender, EventArgs e)
        {

            DataTable dt;
            dt = Myfunc.GetNo_Invoice(tb_RMANO.Text);

            dgv_RMA.DataSource = dt;

        }

        public void No_Invoice()
        {
            DataTable dt = new DataTable();
            //string strSQL = @"SELECT a.*
            //                  FROM (  SELECT RMA_NO, NVL (SUM (RMASQ_QUOTE), 0) - NVL (RMACQ_DISCOUNT, 0) price,rma_invno,RMA_ARNO
            //                            FROM RMADETAIL,
            //                                 RMASALE_QUOTED,
            //                                 RMA
            //                                 LEFT OUTER JOIN RMACHARGE_QUOTED
            //                                    ON RMA.RMA_NO = RMACHARGE_QUOTED.RMACQ_RMANO
            //                           WHERE     RMA_NO = RMAD_RMANO
            //                                 AND RMASQ_RMADID = RMAD_ID
            //                                 AND RMADETAIL.RMAD_STATUS LIKE '9%'
            //                                 AND RMA_STATUS = '90'
            //                                 AND RMA_NO = :RMA_NO
            //                                 AND (NVL(rma_invno,' ') = ' ' OR NVL(RMA_ARNO,' ') = ' ')
            //                        GROUP BY RMA_NO, RMACQ_DISCOUNT,rma_invno,RMA_ARNO
            //                        HAVING NVL (SUM (RMASQ_QUOTE), 0) - NVL (RMACQ_DISCOUNT, 0) > 0) a
            //                       --LEFT JOIN cipherlab.ofb_file ON RMA_no = ofb06
            //                 --WHERE  rma_invno is null
            //                    ORDER BY RMA_NO";

            //using (OracleConnection Conn = new OracleConnection(RMADBLink))
            //{
            //    Conn.Open();
            //    OracleCommand cmd = new OracleCommand(strSQL, Conn);
            //    cmd.Parameters.Add(new OracleParameter("RMA_NO", OracleDbType.NVarchar2)).Value = tb_RMANO.Text.Trim();
            //    OracleDataAdapter da = new OracleDataAdapter(cmd);
            //    da.Fill(dt);
            //}
            dgv_RMA.DataSource = dt;
        }

        private void dgv_RMA_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (curRow != dgv_RMA.CurrentRow.Index)
            {
                dgv_Shipping.Columns.Clear();
                DataTable dt;

                dt = Myfunc.GetShipping_NO(dgv_RMA.CurrentRow.Cells[dgv_RMA.Columns["RMA_NO"].Index].Value.ToString().Trim());

                //string strSQL = @"SELECT DISTINCT RMASH_SHIPPINGNO,
                //                                    RMA_NO,
                //                                    RMASH_PACKINGLIST,
                //                                    RMA_COMPNO,
                //                                    RMA_CUNO,
                //                                    COMP_CURRENCYCODE,
                //                                    RMA_ADNAME,
                //                                    RMA_CSTMP,
                //                                    RMA_LUADNAME,
                //                                    RMA_LUSTMP
                //                      FROM RMA_SHIPPING
                //                           INNER JOIN RMA_SHIPPINGDETAIL ON RMASH_ID = RMASHD_RMASHID
                //                           INNER JOIN RMA ON RMASHD_RMANO = RMA_NO
                //                           INNER JOIN COMPANY ON RMA_COMPNO = COMP_NO
                //                     WHERE RMA_NO = :RMA_NO";
                //using (OracleConnection Conn = new OracleConnection(RMADBLink))
                //{
                //    Conn.Open();
                //    OracleCommand cmd = new OracleCommand(strSQL, Conn);
                //    cmd.Parameters.Add(new OracleParameter("RMA_NO", OracleDbType.NVarchar2)).Value = dgv_RMA.CurrentRow.Cells[dgv_RMA.Columns["RMA_NO"].Index].Value.ToString().Trim();
                //    OracleDataAdapter da = new OracleDataAdapter(cmd);
                //    da.Fill(dt);
                //}
                dgv_Shipping.DataSource = dt;

                DataGridViewCheckBoxColumn cbCol = new DataGridViewCheckBoxColumn();
                cbCol.Width = 80;
                cbCol.HeaderText = "CheckBox";
                cbCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv_Shipping.Columns.Insert(0, cbCol);
                foreach (DataGridViewColumn column in dgv_Shipping.Columns)
                {
                    if (column.Index != 0)
                        column.ReadOnly = true;

                }
                curRow = dgv_RMA.CurrentRow.Index;
            }

        }

        private void btn_RunSP_Click(object sender, EventArgs e)
        {
            bool run = false;
            DataTable dt_dgv = DataGridView2DataTable(dgv_Shipping);
            foreach (DataRow dr in dt_dgv.Rows)
            {
                Boolean.TryParse(dr["CheckBox"].ToString(), out run);
                if (run)
                    break;
            }
            if (!run)
            {
                MessageBox.Show("尚未勾選Shipping No");
                return;
            }

            if (MessageBox.Show("確定產生INVOICE & AR?", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string resultStr = string.Empty;
                //using (OracleConnection Conn = new OracleConnection())
                //{
                //    Conn.Open();
                //    foreach (DataRow dr in dt_dgv.Rows)
                //    {
                //        bool chk;
                //        Boolean.TryParse(dr["CheckBox"].ToString(), out chk);
                //        if (chk)
                //        {
                //            string ShippingNo = dr["RMASH_SHIPPINGNO"].ToString().Trim();
                //            string CustomerNo = dr["RMA_CUNO"].ToString().Trim();
                //            string Currency = dr["COMP_CURRENCYCODE"].ToString().Trim();
                //            string UserNo = "";
                //            string CompNo = dr["RMA_COMPNO"].ToString().Trim();

                //            OracleCommand cmd = new OracleCommand("SP_SHP_INS_AR", Conn);
                //            cmd.Parameters.Add(new OracleParameter("vCUSTNO", OracleDbType.NVarchar2)).Value = CustomerNo;
                //            cmd.Parameters.Add(new OracleParameter("vSHPNO", OracleDbType.NVarchar2)).Value = ShippingNo;
                //            cmd.Parameters.Add(new OracleParameter("vCurr", OracleDbType.NVarchar2)).Value = Currency;
                //            cmd.Parameters.Add(new OracleParameter("vUserNo", OracleDbType.NVarchar2)).Value = UserNo;
                //            cmd.Parameters.Add(new OracleParameter("vCompno", OracleDbType.NVarchar2)).Value = CompNo;
                //            cmd.CommandType = CommandType.StoredProcedure;

                //            OracleParameter Param = cmd.Parameters.Add("vResult", OracleDbType.NVarchar2, 250);
                //            Param.Direction = ParameterDirection.Output;
                //            cmd.ExecuteNonQuery();
                //            MessageBox.Show(Param.Value.ToString());
                //        }
                //    }
                //}
                resultStr = Myfunc.RUN_SP_SHP_INS_AR(dt_dgv);
                MessageBox.Show(resultStr);

                DataTable dt;
                dt = Myfunc.Get_Invoice(tb_RMANO.Text);
                dgv_RMA.DataSource = dt;

                dgv_Shipping.Columns.Clear();
                curRow = -1;
            }
        }

        public static DataTable DataGridView2DataTable(DataGridView dgv, int minRow = 0)
        {
            DataTable dt = new DataTable();
            // Header columns  
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                //DataColumn dc = new DataColumn(column.Name.ToString());  
                DataColumn dc = new DataColumn(column.HeaderText.ToString());
                dt.Columns.Add(dc);
            }
            // Data cells  
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                DataGridViewRow row = dgv.Rows[i];
                DataRow dr = dt.NewRow();
                for (int j = 0; j < dgv.Columns.Count; j++)
                {
                    dr[j] = (row.Cells[j].Value == null) ? "" : row.Cells[j].Value.ToString();
                }
                dt.Rows.Add(dr);
            }
            // Related to the bug arround min size when using ExcelLibrary for export  
            for (int i = dgv.Rows.Count; i < minRow; i++)
            {
                DataRow dr = dt.NewRow();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    dr[j] = " ";
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

    }
}
