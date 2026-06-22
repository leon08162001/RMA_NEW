using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Data;
using System.Windows.Forms;

namespace RMA_Troubleshooting
{
    public partial class Generate_Quoted : Form
    {
        private string sCu_Name = string.Empty;

        public Generate_Quoted()
        {
            InitializeComponent();
        }

        private void btn_quoted_Click(object sender, EventArgs e)
        {
            if (tb_RMANO.Text.Trim() == string.Empty)
            {
                MessageBox.Show("請先輸入RMA單號");
                return;
            }

            string sRMANO = tb_RMANO.Text.Trim();
            DataTable VWRPTCLIENT_SALESQUOTED_HEAD = Myfunc.Get_SalesQuoted_Head(sRMANO);
            DataTable VWRPTCLIENT_SALESQUOTED_SN = Myfunc.Get_SalesQuoted_SN(sRMANO);
            DataTable VWRPTCLIENT_SALESQUOTED_PART = Myfunc.Get_SalesQuoted_Part(sRMANO);

            VWRPTCLIENT_SALESQUOTED_HEAD.TableName = "VWRPTCLIENT_SALESQUOTED_HEAD";
            VWRPTCLIENT_SALESQUOTED_SN.TableName = "VWRPTCLIENT_SALESQUOTED_SN";
            VWRPTCLIENT_SALESQUOTED_PART.TableName = "VWRPTCLIENT_SALESQUOTED_PART";


            string sRMA_COMPNO = string.Empty;
            if (VWRPTCLIENT_SALESQUOTED_HEAD.Rows.Count > 0)
            {
                sRMA_COMPNO = VWRPTCLIENT_SALESQUOTED_HEAD.Rows[0]["RMA_COMPNO"].ToString().Trim();
                sCu_Name = VWRPTCLIENT_SALESQUOTED_HEAD.Rows[0]["CU_NAME"].ToString().Trim();
            }

            VWRPTCLIENT_SALESQUOTED_SN.Columns.Add("SEQSN", typeof(int)); //資料型別為 數字
            VWRPTCLIENT_SALESQUOTED_PART.Columns.Add("SEQPART", typeof(int)); //資料型別為 數字
            DataView dvPart = VWRPTCLIENT_SALESQUOTED_PART.DefaultView;

            for (int i = 0; i <= VWRPTCLIENT_SALESQUOTED_SN.Rows.Count - 1; i++)
            {
                DataRow SnRow = VWRPTCLIENT_SALESQUOTED_SN.Rows[i];
                SnRow["SEQSN"] = (i + 1).ToString();

                //非正常使用: 0.No, 1.Yes
                string RMARQ_IMPROPERUSAGE_Text = "N";
                if (SnRow["RMARQ_IMPROPERUSAGE"].ToString() == "1")
                {
                    RMARQ_IMPROPERUSAGE_Text = "Y";
                }
                SnRow["RMARQ_IMPROPERUSAGE_Text"] = RMARQ_IMPROPERUSAGE_Text;

                //是否保固日期內:null.未定(Unidentified), 0.否, 1.是
                string RMAD_ISWARRANTY_Text = "N";
                if (SnRow["RMAD_ISWARRANTY"] != null)
                {
                    if (SnRow["RMAD_ISWARRANTY"].ToString() == "1")
                    {
                        RMARQ_IMPROPERUSAGE_Text = "Y";
                    }
                }
                SnRow["RMAD_ISWARRANTY_Text"] = RMAD_ISWARRANTY_Text;

                //是否要維修: 1.Accept, 2.Reject
                string RMARQ_Reject = "";
                if (SnRow["RMARQ_ACCEPT"] != null)
                {
                    if (SnRow["RMARQ_ACCEPT"].ToString() == "2")
                    {
                        RMARQ_Reject = "Y";
                    }
                }
                SnRow["RMARQ_ACCEPT_Text"] = RMARQ_Reject;

                dvPart.RowFilter = "RMAD_ID='" + SnRow["RMAD_ID"].ToString().Trim() + "'";
                for (int j = 0; j < dvPart.Count; j++)
                {
                    dvPart[j]["SEQPART"] = (j + 1).ToString();
                }
            }
            dvPart.RowFilter = "";

            DataSet oDsReport = new DataSet();
            oDsReport.Tables.Add(VWRPTCLIENT_SALESQUOTED_HEAD);
            oDsReport.Tables.Add(VWRPTCLIENT_SALESQUOTED_SN);
            oDsReport.Tables.Add(VWRPTCLIENT_SALESQUOTED_PART);

            Print(oDsReport, sRMA_COMPNO);

            MessageBox.Show("OK");
        }

        private void Print(DataSet oDsReport, string sRMA_COMPNO)
        {
            //===================================================================================================================
            //產生Report檔
            //====================================================================================================================

            string sPath = System.Environment.CurrentDirectory;

            ReportDocument ReportDoc = new ReportDocument();
            if (sRMA_COMPNO.ToUpper() == "CL_CHINA")
            {
                ReportDoc.Load(sPath + @"\Report\rptClient_SalesQuoted_CHINA.rpt");
            }
            else
            {
                ReportDoc.Load(sPath + @"\Report\rptClient_SalesQuoted.rpt");
            }

            ReportDoc.SetDataSource(oDsReport);

            CrystalReportViewer1.ReportSource = ReportDoc;
            CrystalReportViewer1.Refresh();
            //CrystalReportViewer1.DataBind();

            ExportOptions myExportOptions = new ExportOptions();
            DiskFileDestinationOptions myDiskFileDestinationOptions = new DiskFileDestinationOptions();
            myExportOptions = ReportDoc.ExportOptions;

            ReportDoc.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
            myExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;

            //===================================================================================================================
            //傳PDF檔
            //===================================================================================================================
            string _ReportToPDF = "ClientQuotation_" + DateTime.Now.ToString("yyyyMMdd") + "_" + sCu_Name + ".pdf";
            myExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
            myDiskFileDestinationOptions.DiskFileName = _ReportToPDF;
            myExportOptions.DestinationOptions = myDiskFileDestinationOptions;
            ReportDoc.Export();

            CrystalReportViewer1.Visible = false;
            ReportDoc.Close();

        }
    }
}
