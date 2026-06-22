using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace RMA_Troubleshooting
{
    public partial class Pack_ExcelInsertSn : Form
    {
        int maxBox = 20;//幾個一箱
        int sncol = 0;//正試序號位置
        //int No = 201;//箱號開始值
        //string BoxWord = "BOXA-1811000";//箱號字軌
        string ogb01 = "AXGA-181100027";//出通單號
        private string CipherlabDB = "Data Source=192.168.7.20/TOPPROD;Persist Security Info=True;User ID=cipherlab;Password=cip2us91ab";
        IWorkbook _workBook;
        DataTable dt_Excel = new DataTable();

        public class PACKBOX
        {
            /// <summary>箱號</summary>
            public string BOX_NO { get; set; }

            /// <summary>箱號項次</summary>
            public string BOX_SEQ { get; set; }

            /// <summary>數量</summary>
            public string BOX_QTY { get; set; }

            /// <summary>出通單</summary>
            public string ogb01 { get; set; }

            /// <summary>出通單項次</summary>
            public string ogb03 { get; set; }
        }

        public Pack_ExcelInsertSn()
        {
            InitializeComponent();
        }

        private void btn_Insert_Click(object sender, EventArgs e)
        {
            if (dt_Excel.Rows.Count > 0)
                run();
        }

        private void run()
        {
            DataTable dt = new DataTable();
            List<PACKBOX> boxlist = new List<PACKBOX>();

            string strSQL = @"select BOX_NO,BOX_SEQ,BOX_QTY from pms_packbox
                             WHERE pack_no = 'ASXA-181100064' AND BOX_NO LIKE 'BOXA%' and CREATE_USER = 'WIN' and CON_STATUS = 'N' and box_seq between 108 and 177";

            using (OracleConnection Conn = new OracleConnection(CipherlabDB))
            {
                Conn.Open();

                OracleCommand cmd = new OracleCommand(strSQL, Conn);
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                da.Fill(dt);
                int a = 1;
                string ogb03 = "1";
                foreach (DataRow dr in dt.Rows)
                {
                    if (a <= 55)
                        ogb03 = "1";
                    else
                        ogb03 = "3";

                    boxlist.Add(new PACKBOX
                    {
                        BOX_NO = dr[0].ToString(),
                        BOX_SEQ = dr[1].ToString(),
                        BOX_QTY = dr[2].ToString(),
                        ogb01 = ogb01,
                        ogb03 = ogb03
                    });
                    a++;
                }

                int startItem = 0;

                int Box = 0;
                string[,] arr = new string[boxlist.Count, maxBox];
                foreach (DataRow dr in dt_Excel.Rows)
                {
                    arr[Box, startItem] = dr[sncol].ToString();
                    startItem += 1;
                    if (startItem == maxBox)
                    {
                        Box += 1;
                        startItem = 0;
                    }
                }

                for (int x = 0; x < boxlist.Count; x++)
                {
                    //BOXA-18090000328
                    string BoxNo = boxlist[x].BOX_NO;
                    for (int y = 0; y < maxBox; y++)
                    {
                        cmd = new OracleCommand("sp_pxmr420_TxPackOgbSn", Conn);
                        cmd.Parameters.Add(new OracleParameter("vBoxNo", OracleDbType.NVarchar2)).Value = boxlist[x].BOX_NO;
                        cmd.Parameters.Add(new OracleParameter("vOgb01", OracleDbType.NVarchar2)).Value = boxlist[x].ogb01;
                        cmd.Parameters.Add(new OracleParameter("vOgb03", OracleDbType.NVarchar2)).Value = boxlist[x].ogb03;
                        cmd.Parameters.Add(new OracleParameter("vSN", OracleDbType.NVarchar2)).Value = arr[x, y];
                        cmd.Parameters.Add(new OracleParameter("vUser", OracleDbType.NVarchar2)).Value = "jay.gong";
                        cmd.CommandType = CommandType.StoredProcedure;

                        OracleParameter Param = cmd.Parameters.Add("vResult", OracleDbType.NVarchar2, 250);
                        Param.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                    //No++;
                }

                MessageBox.Show(arr.Length.ToString());
            }
        }

        private DataTable ReadData(Stream stream)
        {
            _workBook = new XSSFWorkbook(stream); ;
            DataTable dt = new DataTable();
            ISheet sheet = _workBook.GetSheetAt(0);
            IRow row = sheet.GetRow(0);
            int LastCell = row.LastCellNum;

            for (int i = row.FirstCellNum; i < LastCell; i++)
            {
                if (row.GetCell(i) != null)
                    dt.Columns.Add(new DataColumn("col" + i));
            }

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                row = sheet.GetRow(i);
                if (row != null && row.GetCell(1) != null && !string.IsNullOrEmpty(row.GetCell(1).ToString())) //第一格不能是NULL
                {
                    DataRow dr = dt.NewRow();
                    for (int j = row.FirstCellNum; j < LastCell; j++)
                    {
                        ICell cell = row.GetCell(j);
                        if (cell != null)
                        {
                            switch (cell.CellType)
                            {
                                case CellType.String:
                                    dr[j] = cell.StringCellValue;
                                    break;
                                case CellType.Numeric:
                                    if (DateUtil.IsCellDateFormatted(cell))
                                    {
                                        DateTime date = cell.DateCellValue;
                                        dr[j] = date.ToString("yyyy/MM/dd HH:mm:ss");
                                    }
                                    else
                                    {
                                        dr[j] = cell.NumericCellValue.ToString();
                                    }
                                    break;
                                default:
                                    dr[j] = cell.ToString().Trim();
                                    break;
                            }
                        }
                    }
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }

        private void btn_Del_Click(object sender, EventArgs e)
        {
            DataTable dt_Box = new DataTable();
            string strSQL = @"SELECT DISTINCT BOX_NO, OGB01, OGB03
  FROM PACK_PACKSN
 WHERE ogb01 = 'AXGA-181000001' AND sn_num LIKE 'FD1%'";
            using (OracleConnection Conn = new OracleConnection(CipherlabDB))
            {
                Conn.Open();
                OracleCommand cmd = new OracleCommand(strSQL, Conn);
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                da.Fill(dt_Box);
            }
            //MessageBox.Show(dt_Box.Rows.Count.ToString());
            using (OracleConnection Conn = new OracleConnection(CipherlabDB))
            {
                Conn.Open();
                foreach (DataRow dr in dt_Box.Rows)
                {
                    OracleCommand cmd = new OracleCommand("sp_pxmr420_TxPackDelBoxSn", Conn);
                    cmd.Parameters.Add(new OracleParameter("vBoxNo", OracleDbType.NVarchar2)).Value = dr["BOX_NO"].ToString();
                    cmd.Parameters.Add(new OracleParameter("vOgb01", OracleDbType.NVarchar2)).Value = dr["OGB01"].ToString();
                    cmd.Parameters.Add(new OracleParameter("vOgb03", OracleDbType.NVarchar2)).Value = dr["OGB03"].ToString();
                    cmd.Parameters.Add(new OracleParameter("vUser", OracleDbType.NVarchar2)).Value = "jay.gong";
                    cmd.CommandType = CommandType.StoredProcedure;

                    OracleParameter Param = cmd.Parameters.Add("vResult", OracleDbType.NVarchar2, 250);
                    Param.Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void btn_read_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Excel 活頁簿|*.xlsx|Excel 97-2003 活頁簿|*.xls|Excel 97-2003 範本|*.xlt";
            openFileDialog1.Title = "Select a Excel File";

            // Show the Dialog.  
            // If the user clicked OK in the dialog and  
            // a .CUR file was selected, open it.  
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Assign the cursor in the Stream to the Form's Cursor property.  
                using (Stream stream = openFileDialog1.OpenFile())
                {
                    dt_Excel = ReadData(stream);
                    dataGridView1.DataSource = dt_Excel;
                }
            }
        }

    }
}
