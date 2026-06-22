using Oracle.DataAccess.Client;
using System;
using System.Data;
using System.Windows.Forms;

namespace RMA_Troubleshooting
{
    public partial class Insert_Repair_Price : Form
    {
        //private string DBLink = "Data Source=192.168.7.20/TOPPROD;Persist Security Info=True;User ID=rma;Password=4321rma";
        private string DBLink = "";
        bool AutoRun = false;
        public Insert_Repair_Price(string[] args)
        {
            InitializeComponent();
            if (args.Length > 0)
            {
                if (args[0].ToUpper().Trim() == "AUTO")
                    AutoRun = true;
            }

            if (AutoRun)
            {
                Auto();
            }
        }
        private void Auto()
        {
            Insert_RepairBom(ChkNoData());
            this.Close();
            Environment.Exit(Environment.ExitCode);
        }

        private DataTable ChkNoData()
        {
            DataTable dt = new DataTable();
            string strSQL = @"SELECT b.ima01, b.ima021, ta_ima020
                                FROM (SELECT ima01
                                        FROM cipherlab.ima_file
                                       WHERE imaacti = 'Y' AND ima140 <> 'Y'
                                      MINUS
                                      SELECT DISTINCT RPBOM_PARTNO FROM repairBom) a
                                     LEFT JOIN cipherlab.ima_file b ON a.ima01 = b.ima01
                               WHERE ta_ima020 > 0 
                            ORDER BY b.ima01"; //IMA有價格資料而RMA無資料

            using (OracleConnection Conn = new OracleConnection(DBLink))
            {
                OracleCommand cmd = new OracleCommand(strSQL, Conn);
                OracleDataAdapter od = new OracleDataAdapter(cmd);
                od.Fill(dt);
            }
            dgv_list.DataSource = dt;
            return dt;
        }

        private void Insert_RepairBom(DataTable dt)
        {
            string strSQL = @"INSERT INTO RMA.RepairBOM
                                   SELECT {0},
                                          ima01,
                                          '',
                                          ima021,
                                          ta_ima020,
                                          'admin',
                                          'admin',
                                          SYSDATE,
                                          'admin',
                                          'admin',
                                          SYSDATE
                                     FROM cipherlab.ima_file
                                    WHERE ima01 = :ima01 ";

            string strSQL_CN = @"INSERT INTO RepairBOM
                               SELECT {0},
                                      ima01,
                                      '',
                                      ima021,
                                        ta_ima020
                                      / (SELECT tc_oaa030
                                           FROM cipherlab.tc_oaa_file
                                          WHERE tc_oaa010 = 'RMB'
                                            AND TO_CHAR (tc_oaa020, 'YYYY/MM/DD') = TO_CHAR (SYSDATE, 'YYYY') || '/01/01' )
                                         ta_ima020,
                                      'admin',
                                      'admin',
                                      SYSDATE,
                                      'admin',
                                      'admin',
                                      SYSDATE
                                 FROM cipherlab.ima_file
                                WHERE ima01 = :ima01 ";
            using (OracleConnection Conn = new OracleConnection(DBLink))
            {
                Conn.Open();
                string[] center = { "AU", "AUS", "CEAT", "CLHQ", "CL_CHINA", "CL_USA", "JRC" };
                OracleCommand cmd = new OracleCommand();

                foreach (DataRow dr in dt.Rows)
                {
                    OracleTransaction tran = Conn.BeginTransaction();
                    try
                    {
                        foreach (string s in center)
                        {

                            if (s == "CL_CHINA")
                            {
                                //strSQL_CN = string.Format(strSQL_CN,"'" + s + "'");
                                cmd = new OracleCommand(string.Format(strSQL_CN, "'" + s + "'"), Conn);
                            }
                            else
                            {
                                //strSQL = string.Format(strSQL, "'" + s + "'");
                                cmd = new OracleCommand(string.Format(strSQL, "'" + s + "'"), Conn);
                            }
                            cmd.Transaction = tran;
                            cmd.Parameters.Add(new OracleParameter("ima01", dr["ima01"].ToString()));
                            //textBox1.Text = strSQL.Replace(":ima01", "'" + dr["ima01"].ToString() + "'").Replace(":center", "'" + s + "'");
                            cmd.ExecuteNonQuery();
                        }
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        label1.Text = ex.ToString();
                        tran.Rollback();
                    }
                }
            }
        }

        private void btn_Show_Click(object sender, EventArgs e)
        {
            Insert_RepairBom(ChkNoData());
        }
    }
}
