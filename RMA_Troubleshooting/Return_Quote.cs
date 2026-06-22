using System;
using System.Data;
using System.Windows.Forms;

namespace RMA_Troubleshooting
{
    public partial class Return_Quote : Form
    {
        int curRow = -1;
        //private string RMADBLink = "Data Source=192.168.7.20/TOPTEST;Persist Security Info=True;User ID=RMA;Password=4321rma";
        public Return_Quote()
        {
            InitializeComponent();
            lbl_msg.Text = "";
        }

        private void btn_Qry_Click(object sender, EventArgs e)
        {
            dgv_RMA.Columns.Clear();
            dgv_RMADetail.Columns.Clear();
            lbl_msg.Text = "";

            DataTable dt = new DataTable();

            dt = Myfunc.GetRMAData(tb_RMANO.Text.Trim());
            //string strSQL = @"SELECT RMA_NO,RMA_CUNO,RMA_COMPNO,RMA_STATUS,RMA_LUSTMP
            //                  FROM RMA
            //                 WHERE RMA_NO LIKE :RMA_NO AND RMA_STATUS >= 20 AND RMA_MARK = 0";

            //using (OracleConnection Conn = new OracleConnection(RMADBLink))
            //{
            //    Conn.Open();
            //    OracleCommand cmd = new OracleCommand(strSQL, Conn);
            //    cmd.Parameters.Add(new OracleParameter("RMA_NO", OracleDbType.NVarchar2)).Value = tb_RMANO.Text.Trim() + "%";
            //    OracleDataAdapter da = new OracleDataAdapter(cmd);
            //    da.Fill(dt);
            //}

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("無資料");
                return;
            }

            dgv_RMA.DataSource = dt;
            curRow = -1;
            dgv_RMA_CellClick(sender, null);
        }

        private void btn_return20_Click(object sender, EventArgs e)
        {
            if (dgv_RMADetail.Rows.Count == 0)
            {
                MessageBox.Show("無資料");
                return;
            }

            string sMsg = string.Empty;
            DataTable dt_dgv = DataGridView2DataTable(dgv_RMADetail);


            if (MessageBox.Show("確定將此單號回退至收貨階段(20)?", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                lbl_msg.Text = Myfunc.UpdateReturn20(dt_dgv);
                MessageBox.Show(lbl_msg.Text, "Message", MessageBoxButtons.OK);
            }

            //if (MessageBox.Show("確定將此單號回退至收貨階段?", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            //{
            //    string strSQL_RMA = "UPDATE RMA SET RMA_STATUS='20' WHERE RMA_NO = :RMA_NO";
            //    string strSQL_RMADETAIL = "UPDATE RMADETAIL SET RMAD_STATUS='20' WHERE RMAD_ID = :RMAD_ID";
            //    string strSQL_RMAREPAIR = "UPDATE RMAREPAIR SET RMAR_RMADID = RMAR_RMADID || 'X' WHERE RMAR_RMADID = :RMAR_RMADID";
            //    string strSQL_RMAREPAIR_DETAIL = "UPDATE RMAREPAIR_DETAIL SET RMARED_RMADID = RMARED_RMADID || 'X' WHERE RMARED_RMADID = :RMARED_RMADID";
            //    string strSQL_RMAREPAIR_QUOTED = "UPDATE RMAREPAIR_QUOTED SET RMARQ_RMADID = RMARQ_RMADID || 'X' WHERE RMARQ_RMADID = :RMARQ_RMADID";
            //    string strSQL_RMAREPAIR_QUOTED_DETAIL = "UPDATE RMAREPAIR_QUOTED_DETAIL SET RMARQD_RMADID = RMARQD_RMADID || 'X' WHERE RMARQD_RMADID = :RMARQD_RMADID";
            //    string strSQL_RMASALE_QUOTED = "UPDATE RMASALE_QUOTED SET RMASQ_RMADID = RMASQ_RMADID || 'X' WHERE RMASQ_RMADID = :RMASQ_RMADID";
            //    OracleCommand cmd = new OracleCommand();

            //    using (OracleConnection Conn = new OracleConnection(RMADBLink))
            //    {
            //        Conn.Open();
            //        OracleTransaction oTran = Conn.BeginTransaction();
            //        try
            //        {
            //            foreach (DataRow dr in dt_dgv.Rows)
            //            {
            //                bool chk;
            //                Boolean.TryParse(dr["CheckBox"].ToString(),out chk);
            //                if (chk)
            //                {
            //                    string RMAD_ID = dr["RMAD_ID"].ToString().Trim();
            //                    string RMAD_RMANO = dr["RMAD_RMANO"].ToString().Trim();
            //                    cmd = new OracleCommand(strSQL_RMA, Conn);
            //                    cmd.Parameters.Add(new OracleParameter("RMA_NO", OracleDbType.NVarchar2)).Value = RMAD_RMANO;
            //                    cmd.ExecuteNonQuery();

            //                    cmd = new OracleCommand(strSQL_RMADETAIL, Conn);
            //                    cmd.Parameters.Add(new OracleParameter("RMAD_ID", OracleDbType.NVarchar2)).Value = RMAD_ID;
            //                    cmd.ExecuteNonQuery();

            //                    cmd = new OracleCommand(strSQL_RMAREPAIR, Conn);
            //                    cmd.Parameters.Add(new OracleParameter("RMAR_RMADID", OracleDbType.NVarchar2)).Value = RMAD_ID;
            //                    cmd.ExecuteNonQuery();

            //                    cmd = new OracleCommand(strSQL_RMAREPAIR_DETAIL, Conn);
            //                    cmd.Parameters.Add(new OracleParameter("RMARED_RMADID", OracleDbType.NVarchar2)).Value = RMAD_ID;
            //                    cmd.ExecuteNonQuery();

            //                    cmd = new OracleCommand(strSQL_RMAREPAIR_QUOTED, Conn);
            //                    cmd.Parameters.Add(new OracleParameter("RMARQ_RMADID", OracleDbType.NVarchar2)).Value = RMAD_ID;
            //                    cmd.ExecuteNonQuery();

            //                    cmd = new OracleCommand(strSQL_RMAREPAIR_QUOTED_DETAIL, Conn);
            //                    cmd.Parameters.Add(new OracleParameter("RMARQD_RMADID", OracleDbType.NVarchar2)).Value = RMAD_ID;
            //                    cmd.ExecuteNonQuery();

            //                    cmd = new OracleCommand(strSQL_RMASALE_QUOTED, Conn);
            //                    cmd.Parameters.Add(new OracleParameter("RMASQ_RMADID", OracleDbType.NVarchar2)).Value = RMAD_ID;
            //                    cmd.ExecuteNonQuery();
            //                }
            //            }
            //            oTran.Commit();
            //            lbl_msg.Text = "回退完成";
            //        }
            //        catch
            //        {
            //            lbl_msg.Text = "回退失敗";
            //            oTran.Rollback();
            //        }
            //    }
            //}
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

        private void dgv_RMA_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (curRow != dgv_RMA.CurrentRow.Index)
            {
                dgv_RMADetail.Columns.Clear();
                DataTable dt = new DataTable();

                dt = Myfunc.GetRMADetailData(tb_RMANO.Text.Trim());

                //string strSQL = "Select RMAD_ID,RMAD_SERIALNO,RMAD_SEQ,RMAD_RMANO,RMAD_WARRANTY,RMAD_STATUS,RMAD_MARK,RMAD_ISWARRANTY from RMADETAIL WHERE RMAD_RMANO = :RMAD_RMANO AND RMAD_STATUS > 20 ";
                //using (OracleConnection Conn = new OracleConnection(RMADBLink))
                //{
                //    Conn.Open();
                //    OracleCommand cmd = new OracleCommand(strSQL, Conn);
                //    cmd.Parameters.Add(new OracleParameter("RMAD_RMANO", OracleDbType.NVarchar2)).Value = dgv_RMA.CurrentRow.Cells[dgv_RMA.Columns["RMA_NO"].Index].Value.ToString().Trim();
                //    OracleDataAdapter da = new OracleDataAdapter(cmd);
                //    da.Fill(dt);
                //}

                dgv_RMADetail.DataSource = dt;

                DataGridViewCheckBoxColumn cbCol = new DataGridViewCheckBoxColumn();
                cbCol.Width = 80;
                cbCol.HeaderText = "CheckBox";
                cbCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv_RMADetail.Columns.Insert(0, cbCol);
                foreach (DataGridViewColumn column in dgv_RMADetail.Columns)
                {
                    if (column.Index != 0)
                        column.ReadOnly = true;

                }
                curRow = dgv_RMA.CurrentRow.Index;
            }
        }

        private void btn_return30_Click(object sender, EventArgs e)
        {
            lbl_msg.Text = "";

            if (dgv_RMADetail.Rows.Count == 0)
            {
                MessageBox.Show("無資料");
                return;
            }

            string sMsg = string.Empty;
            DataTable dt_dgv = DataGridView2DataTable(dgv_RMADetail);


            if (MessageBox.Show("確定將此單號回退保留報價資訊(30)?", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                lbl_msg.Text = Myfunc.UpdateReturn30(dt_dgv);
                MessageBox.Show(lbl_msg.Text, "Message", MessageBoxButtons.OK);
            }

        }
    }
}
