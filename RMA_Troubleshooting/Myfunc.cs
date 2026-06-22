using Oracle.DataAccess.Client;
using RMA_Troubleshooting.OrderWebService;
using System;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace RMA_Troubleshooting
{
    class Myfunc
    {
        static private string RMADBLink = "Data Source=192.168.7.20/TOPPROD;Persist Security Info=True;User ID=rma;Password=4321rma";
        static private string CipherlabDBLink = "Data Source=192.168.7.20/TOPTEST;Persist Security Info=True;User ID=cipherlab;Password=cip2us91ab";
        //static OracleTransaction FTrans;

        static private OracleConnection FConn = new OracleConnection();

        /// <summary> 取得 Connection </summary>
        /// <returns> </returns>	
        static public OracleConnection OracleConn
        {
            get
            {
                return FConn;
            }

        }

        static public bool RMAConnected()
        {
            FConn.Close();
            FConn.ConnectionString = RMADBLink;
            try
            {

                FConn.Open();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK);
                return false;
            }

            return true;

        }

        static public bool CipherlabConnected()
        {
            FConn.Close();
            FConn.ConnectionString = CipherlabDBLink;
            try
            {

                FConn.Open();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK);
                return false;
            }

            return true;

        }

        static public void OracleClosed()
        {
            FConn.Close();
        }

        #region 回退RMA

        static public DataTable GetRMAData(string sRMA_NO)
        {
            string strSQL = @"SELECT RMA_NO,RMA_CUNO,RMA_COMPNO,RMA_STATUS,RMA_LUSTMP
                              FROM RMA WHERE RMA_STATUS >= 20 AND RMA_STATUS <= 60 AND RMA_MARK = 0  AND RMA_NO = :RMA_NO ";

            DataTable dt = new DataTable();

            Myfunc.RMAConnected();

            OracleCommand cmd = new OracleCommand(strSQL, Myfunc.FConn);
            cmd.Parameters.Add(new OracleParameter("RMA_NO", OracleDbType.NVarchar2)).Value = sRMA_NO.Trim();
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            da.Fill(dt);

            Myfunc.OracleClosed();

            return dt;
        }

        static public DataTable GetRMADetailData(string sRMA_NO)
        {
            string strSQL = @"Select RMAD_ID,RMAD_SERIALNO,RMAD_SEQ,RMAD_RMANO,RMAD_WARRANTY,RMAD_STATUS,RMAD_MARK,RMAD_ISWARRANTY from RMADETAIL WHERE RMAD_RMANO = :RMAD_RMANO AND RMAD_STATUS > 20 ";

            DataTable dt = new DataTable();

            Myfunc.RMAConnected();

            OracleCommand cmd = new OracleCommand(strSQL, FConn);
            cmd.Parameters.Add(new OracleParameter("RMAD_RMANO", OracleDbType.NVarchar2)).Value = sRMA_NO.Trim();
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            da.Fill(dt);

            Myfunc.OracleClosed();

            return dt;
        }

        static public string UpdateReturn20(DataTable dt)
        {
            string strSQL_RMA = "UPDATE RMA SET RMA_STATUS='20' WHERE RMA_NO = :RMA_NO";
            string strSQL_RMADETAIL = "UPDATE RMADETAIL SET RMAD_STATUS='20' WHERE RMAD_ID = :RMAD_ID";
            string strSQL_RMAREPAIR = "UPDATE RMAREPAIR SET RMAR_RMADID = RMAR_RMADID || 'X' WHERE RMAR_RMADID = :RMAR_RMADID";
            string strSQL_RMAREPAIR_DETAIL = "UPDATE RMAREPAIR_DETAIL SET RMARED_RMADID = RMARED_RMADID || 'X' WHERE RMARED_RMADID = :RMARED_RMADID";
            string strSQL_RMAREPAIR_QUOTED = "UPDATE RMAREPAIR_QUOTED SET RMARQ_RMADID = RMARQ_RMADID || 'X' WHERE RMARQ_RMADID = :RMARQ_RMADID";
            string strSQL_RMAREPAIR_QUOTED_DETAIL = "UPDATE RMAREPAIR_QUOTED_DETAIL SET RMARQD_RMADID = RMARQD_RMADID || 'X' WHERE RMARQD_RMADID = :RMARQD_RMADID";
            string strSQL_RMASALE_QUOTED = "UPDATE RMASALE_QUOTED SET RMASQ_RMADID = RMASQ_RMADID || 'X' WHERE RMASQ_RMADID = :RMASQ_RMADID";
            OracleCommand cmd = new OracleCommand();

            Myfunc.RMAConnected();

            OracleTransaction oTran = FConn.BeginTransaction();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    bool chk;
                    Boolean.TryParse(dr["CheckBox"].ToString(), out chk);
                    if (chk)
                    {
                        string RMAD_ID = dr["RMAD_ID"].ToString().Trim();
                        string RMAD_RMANO = dr["RMAD_RMANO"].ToString().Trim();
                        cmd = new OracleCommand(strSQL_RMA, FConn);
                        cmd.Parameters.Add(new OracleParameter("RMA_NO", OracleDbType.NVarchar2)).Value = RMAD_RMANO;
                        cmd.ExecuteNonQuery();

                        cmd = new OracleCommand(strSQL_RMADETAIL, FConn);
                        cmd.Parameters.Add(new OracleParameter("RMAD_ID", OracleDbType.NVarchar2)).Value = RMAD_ID;
                        cmd.ExecuteNonQuery();

                        cmd = new OracleCommand(strSQL_RMAREPAIR, FConn);
                        cmd.Parameters.Add(new OracleParameter("RMAR_RMADID", OracleDbType.NVarchar2)).Value = RMAD_ID;
                        cmd.ExecuteNonQuery();

                        cmd = new OracleCommand(strSQL_RMAREPAIR_DETAIL, FConn);
                        cmd.Parameters.Add(new OracleParameter("RMARED_RMADID", OracleDbType.NVarchar2)).Value = RMAD_ID;
                        cmd.ExecuteNonQuery();

                        cmd = new OracleCommand(strSQL_RMAREPAIR_QUOTED, FConn);
                        cmd.Parameters.Add(new OracleParameter("RMARQ_RMADID", OracleDbType.NVarchar2)).Value = RMAD_ID;
                        cmd.ExecuteNonQuery();

                        cmd = new OracleCommand(strSQL_RMAREPAIR_QUOTED_DETAIL, FConn);
                        cmd.Parameters.Add(new OracleParameter("RMARQD_RMADID", OracleDbType.NVarchar2)).Value = RMAD_ID;
                        cmd.ExecuteNonQuery();

                        cmd = new OracleCommand(strSQL_RMASALE_QUOTED, FConn);
                        cmd.Parameters.Add(new OracleParameter("RMASQ_RMADID", OracleDbType.NVarchar2)).Value = RMAD_ID;
                        cmd.ExecuteNonQuery();
                    }
                }

                oTran.Commit();

                Myfunc.OracleClosed();

                return "回退完成";
            }
            catch (Exception ex)
            {
                oTran.Rollback();
                //MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK);

                Myfunc.OracleClosed();

                return "回退失敗 -- " + ex.Message.ToString();
            }



        }


        static public string UpdateReturn30(DataTable dt)
        {
            string strSQL_RMA = "UPDATE RMA SET RMA_STATUS='20' WHERE RMA_NO = :RMA_NO";
            string strSQL_RMADETAIL = "UPDATE RMADETAIL SET RMAD_STATUS='30' WHERE RMAD_ID = :RMAD_ID";
            //string strSQL_RMAREPAIR = "UPDATE RMAREPAIR SET RMAR_RMADID = RMAR_RMADID || 'X' WHERE RMAR_RMADID = :RMAR_RMADID";
            //string strSQL_RMAREPAIR_DETAIL = "UPDATE RMAREPAIR_DETAIL SET RMARED_RMADID = RMARED_RMADID || 'X' WHERE RMARED_RMADID = :RMARED_RMADID";
            //string strSQL_RMAREPAIR_QUOTED = "UPDATE RMAREPAIR_QUOTED SET RMARQ_RMADID = RMARQ_RMADID || 'X' WHERE RMARQ_RMADID = :RMARQ_RMADID";
            //string strSQL_RMAREPAIR_QUOTED_DETAIL = "UPDATE RMAREPAIR_QUOTED_DETAIL SET RMARQD_RMADID = RMARQD_RMADID || 'X' WHERE RMARQD_RMADID = :RMARQD_RMADID";
            string strSQL_RMASALE_QUOTED = "UPDATE RMASALE_QUOTED SET RMASQ_RMADID = RMASQ_RMADID || 'X' WHERE RMASQ_RMADID = :RMASQ_RMADID";
            OracleCommand cmd = new OracleCommand();

            Myfunc.RMAConnected();

            OracleTransaction oTran = FConn.BeginTransaction();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    bool chk;
                    Boolean.TryParse(dr["CheckBox"].ToString(), out chk);
                    if (chk)
                    {
                        string RMAD_ID = dr["RMAD_ID"].ToString().Trim();
                        string RMAD_RMANO = dr["RMAD_RMANO"].ToString().Trim();
                        cmd = new OracleCommand(strSQL_RMA, FConn);
                        cmd.Parameters.Add(new OracleParameter("RMA_NO", OracleDbType.NVarchar2)).Value = RMAD_RMANO;
                        cmd.ExecuteNonQuery();

                        cmd = new OracleCommand(strSQL_RMADETAIL, FConn);
                        cmd.Parameters.Add(new OracleParameter("RMAD_ID", OracleDbType.NVarchar2)).Value = RMAD_ID;
                        cmd.ExecuteNonQuery();

                        //cmd = new OracleCommand(strSQL_RMAREPAIR, FConn);
                        //cmd.Parameters.Add(new OracleParameter("RMAR_RMADID", OracleDbType.NVarchar2)).Value = RMAD_ID;
                        //cmd.ExecuteNonQuery();

                        //cmd = new OracleCommand(strSQL_RMAREPAIR_DETAIL, FConn);
                        //cmd.Parameters.Add(new OracleParameter("RMARED_RMADID", OracleDbType.NVarchar2)).Value = RMAD_ID;
                        //cmd.ExecuteNonQuery();

                        //cmd = new OracleCommand(strSQL_RMAREPAIR_QUOTED, FConn);
                        //cmd.Parameters.Add(new OracleParameter("RMARQ_RMADID", OracleDbType.NVarchar2)).Value = RMAD_ID;
                        //cmd.ExecuteNonQuery();

                        //cmd = new OracleCommand(strSQL_RMAREPAIR_QUOTED_DETAIL, FConn);
                        //cmd.Parameters.Add(new OracleParameter("RMARQD_RMADID", OracleDbType.NVarchar2)).Value = RMAD_ID;
                        //cmd.ExecuteNonQuery();

                        cmd = new OracleCommand(strSQL_RMASALE_QUOTED, FConn);
                        cmd.Parameters.Add(new OracleParameter("RMASQ_RMADID", OracleDbType.NVarchar2)).Value = RMAD_ID;
                        cmd.ExecuteNonQuery();
                    }
                }

                oTran.Commit();

                Myfunc.OracleClosed();

                return "回退完成";
            }
            catch (Exception ex)
            {
                oTran.Rollback();

                Myfunc.OracleClosed();
                //MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK);
                return "回退失敗 -- " + ex.Message.ToString();
            }

        }

        #endregion


        #region  產生invoice & AR

        static public DataTable GetNo_Invoice(string sRMA_NO)
        {
            string strSQL = @"SELECT a.*
                              FROM (  SELECT RMA_NO, NVL (SUM (RMASQ_QUOTE), 0) - NVL (RMACQ_DISCOUNT, 0) price,rma_invno,RMA_ARNO
                                        FROM RMADETAIL,
                                             RMASALE_QUOTED,
                                             RMA
                                             LEFT OUTER JOIN RMACHARGE_QUOTED
                                                ON RMA.RMA_NO = RMACHARGE_QUOTED.RMACQ_RMANO
                                       WHERE     RMA_NO = RMAD_RMANO
                                             AND RMASQ_RMADID = RMAD_ID
                                             AND RMADETAIL.RMAD_STATUS LIKE '9%'
                                             AND RMA_STATUS = '90'
                                             AND RMA_NO = :RMA_NO
                                             AND (NVL(rma_invno,' ') = ' ' OR NVL(RMA_ARNO,' ') = ' ')
                                    GROUP BY RMA_NO, RMACQ_DISCOUNT,rma_invno,RMA_ARNO
                                    HAVING NVL (SUM (RMASQ_QUOTE), 0) - NVL (RMACQ_DISCOUNT, 0) > 0) a
                                   --LEFT JOIN cipherlab.ofb_file ON RMA_no = ofb06
                             --WHERE  rma_invno is null
                                ORDER BY RMA_NO";

            DataTable dt = new DataTable();

            Myfunc.RMAConnected();

            OracleCommand cmd = new OracleCommand(strSQL, FConn);
            cmd.Parameters.Add(new OracleParameter("RMA_NO", OracleDbType.NVarchar2)).Value = sRMA_NO.Trim();
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            da.Fill(dt);

            Myfunc.OracleClosed();

            return dt;
        }

        static public DataTable Get_Invoice(string sRMA_NO)
        {
            string strSQL = @"SELECT a.*
                              FROM (  SELECT RMA_NO, NVL (SUM (RMASQ_QUOTE), 0) - NVL (RMACQ_DISCOUNT, 0) price,rma_invno,RMA_ARNO
                                        FROM RMADETAIL,
                                             RMASALE_QUOTED,
                                             RMA
                                             LEFT OUTER JOIN RMACHARGE_QUOTED
                                                ON RMA.RMA_NO = RMACHARGE_QUOTED.RMACQ_RMANO
                                       WHERE     RMA_NO = RMAD_RMANO
                                             AND RMASQ_RMADID = RMAD_ID
                                             AND RMADETAIL.RMAD_STATUS LIKE '9%'
                                             AND RMA_STATUS = '90'
                                             AND RMA_NO = :RMA_NO
                                    GROUP BY RMA_NO, RMACQ_DISCOUNT,rma_invno,RMA_ARNO) a
                                   --LEFT JOIN cipherlab.ofb_file ON RMA_no = ofb06
                             --WHERE  rma_invno is null
                                ORDER BY RMA_NO";

            DataTable dt = new DataTable();

            Myfunc.RMAConnected();

            OracleCommand cmd = new OracleCommand(strSQL, FConn);
            cmd.Parameters.Add(new OracleParameter("RMA_NO", OracleDbType.NVarchar2)).Value = sRMA_NO.Trim();
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            da.Fill(dt);

            Myfunc.OracleClosed();

            return dt;
        }

        static public DataTable GetShipping_NO(string sRMA_NO)
        {
            string strSQL = @"SELECT DISTINCT RMASH_SHIPPINGNO,
                                                    RMA_NO,
                                                    RMASH_PACKINGLIST,
                                                    RMA_COMPNO,
                                                    RMA_CUNO,
                                                    COMP_CURRENCYCODE,
                                                    RMA_ADNAME,
                                                    RMA_CSTMP,
                                                    RMA_LUADNAME,
                                                    RMA_LUSTMP
                                      FROM RMA_SHIPPING
                                           INNER JOIN RMA_SHIPPINGDETAIL ON RMASH_ID = RMASHD_RMASHID
                                           INNER JOIN RMA ON RMASHD_RMANO = RMA_NO
                                           INNER JOIN COMPANY ON RMA_COMPNO = COMP_NO
                                     WHERE RMA_NO = :RMA_NO";

            DataTable dt = new DataTable();

            Myfunc.RMAConnected();

            OracleCommand cmd = new OracleCommand(strSQL, FConn);
            cmd.Parameters.Add(new OracleParameter("RMA_NO", OracleDbType.NVarchar2)).Value = sRMA_NO.Trim();
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            da.Fill(dt);

            Myfunc.OracleClosed();

            return dt;
        }

        static public string RUN_SP_SHP_INS_AR(DataTable dt_dgv)
        {
            string ResultStr = string.Empty;

            Myfunc.RMAConnected();

            foreach (DataRow dr in dt_dgv.Rows)
            {
                bool chk;
                Boolean.TryParse(dr["CheckBox"].ToString(), out chk);
                if (chk)
                {
                    string ShippingNo = dr["RMASH_SHIPPINGNO"].ToString().Trim();
                    string CustomerNo = dr["RMA_CUNO"].ToString().Trim();
                    string Currency = dr["COMP_CURRENCYCODE"].ToString().Trim();
                    string UserNo = "Admin";
                    string CompNo = dr["RMA_COMPNO"].ToString().Trim();

                    OracleCommand cmd = new OracleCommand("SP_SHP_INS_AR", FConn);
                    cmd.Parameters.Add(new OracleParameter("vCUSTNO", OracleDbType.NVarchar2)).Value = CustomerNo;
                    cmd.Parameters.Add(new OracleParameter("vSHPNO", OracleDbType.NVarchar2)).Value = ShippingNo;
                    cmd.Parameters.Add(new OracleParameter("vCurr", OracleDbType.NVarchar2)).Value = Currency;
                    cmd.Parameters.Add(new OracleParameter("vUserNo", OracleDbType.NVarchar2)).Value = UserNo;
                    cmd.Parameters.Add(new OracleParameter("vCompno", OracleDbType.NVarchar2)).Value = CompNo;
                    cmd.CommandType = CommandType.StoredProcedure;

                    OracleParameter Param = cmd.Parameters.Add("vResult", OracleDbType.NVarchar2, 250);
                    Param.Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    ResultStr = Param.Value.ToString();
                }
            }

            Myfunc.OracleClosed();

            return ResultStr;
        }

        #endregion


        #region  刪除Sales Quoted


        #endregion

        #region 產生報價單

        static public DataTable Get_SalesQuoted_Head(string sRMA_NO)
        {
            string strSQL = @"SELECT RMA_NO, RMA_APPLICANT, RMA_TEL, RMA_ADDRESS, CU_NAME, RMA_COMPNO, TO_CHAR(RMAD_RECVDATE, 'YYYY/MM/DD') as RMAD_RECVDATE,  TO_CHAR(sysdate, 'YYYY/MM/DD HH24:MI') as PrintDate,
                              vwSalesQuoted.RMASQ_QUOTE, TO_CHAR(vwSalesQuoted.SALES_DATE, 'YYYY/MM/DD') as SALES_DATE, 
                     nvl(AcceptCount,0) as AcceptCount, nvl(RejectCount,0) as RejectCount, 
                 RMACHARGE_QUOTED.RMACQ_DISCOUNT, RMACHARGE_QUOTED.RMACQ_CHARGEQUOTE, RMACHARGE_QUOTED.RMACQ_QUOTE_ORIGINAL,vwSalesQuoted.RMASQ_CURRENCYCODE AS Currency_NO 
                 FROM RMA
                 INNER JOIN CUSTOMER ON RMA_CUNO = CU_NO
                 INNER JOIN 
                 (
                     SELECT RMAD_RMANO, min(RMAD_RECVDATE) as RMAD_RECVDATE
                     FROM RMADETAIL WHERE RMAD_MARK=0 AND RMAD_RECEVSTATUS=1
                     GROUP BY RMAD_RMANO
                 ) vwRecDate
                 ON RMAD_RMANO = RMA_NO
                --是否要維修: 1.Accept, 2.Reject
                INNER JOIN 
                (
                     SELECT RMAD_RMANO,RMASQ_CURRENCYCODE, sum(RMASQ_QUOTE) as RMASQ_QUOTE, min(RMASQ_SALEDATE) as SALES_DATE
                     FROM RMADETAIL, RMASALE_QUOTED 
                     WHERE RMAD_ID = RMASQ_RMADID 
                     GROUP BY RMAD_RMANO,RMASQ_CURRENCYCODE 
                 ) vwSalesQuoted ON RMA_NO = vwSalesQuoted.RMAD_RMANO

                LEFT OUTER JOIN 
                (
                     SELECT RMAD_RMANO, COUNT(*) as AcceptCount
                     FROM RMADETAIL, RMAREPAIR_QUOTED 
                     WHERE RMAD_ID = RMARQ_RMADID AND RMARQ_ACCEPT=1
                     GROUP BY RMAD_RMANO
                ) vwAccept ON RMA_NO = vwAccept.RMAD_RMANO
                LEFT OUTER JOIN 
                (
                     SELECT RMAD_RMANO, COUNT(*) as RejectCount
                     FROM RMADETAIL, RMAREPAIR_QUOTED 
                     WHERE RMAD_ID = RMARQ_RMADID AND RMARQ_ACCEPT=2
                     GROUP BY RMAD_RMANO
                 ) vwReject ON RMA_NO = vwReject.RMAD_RMANO
                 LEFT OUTER JOIN RMACHARGE_QUOTED ON RMACHARGE_QUOTED.RMACQ_RMANO = RMA.RMA_NO 
                 WHERE RMA_NO =:RMA_NO ";

            DataTable dt = new DataTable();

            Myfunc.RMAConnected();

            OracleCommand cmd = new OracleCommand(strSQL, Myfunc.FConn);
            cmd.Parameters.Add(new OracleParameter("RMA_NO", OracleDbType.NVarchar2)).Value = sRMA_NO.Trim();
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            da.Fill(dt);

            Myfunc.OracleClosed();

            return dt;
        }

        static public DataTable Get_SalesQuoted_SN(string sRMA_NO)
        {
            string strSQL = @"SELECT RMA_NO, RMAD_ID , RMAD_SERIALNO, RMAD_MODELNO, RMAD_STATUS, 
                     RMAR_PROBLEMDESC, RMAR_REPAIRDESC,
                     RMARQ_IMPROPERUSAGE, null as RMARQ_IMPROPERUSAGE_Text, 
                     RMAD_ISWARRANTY, null as RMAD_ISWARRANTY_Text,
                     RMARQ_ACCEPT, null as RMARQ_ACCEPT_Text,
                     CASE WHEN RMACQSN_LABORCOST is not null THEN RMACQSN_LABORCOST ELSE nvl(RMASQ_LABORCOST,0) END as RMASQ_LABORCOST,
                     CASE WHEN RMARQ_ACCEPT=2 THEN 0 ELSE 
                     CASE WHEN RMACQSN_MATERIALCOST is not null THEN RMACQSN_MATERIALCOST  ELSE nvl(RMASQ_MATERIALCOST,0) END END as RMASQ_MATERIALCOST,
                     CASE WHEN RMACQSN_QUOTE is not null THEN RMACQSN_QUOTE ELSE nvl(RMASQ_QUOTE,0) END as RMASQ_QUOTE,
                     nvl(RMASQ_CURRENCYCODE,' ') as RMASQ_CURRENCYCODE, 
                     nvl(RMASQ_CURRENCYRATE , 0) as RMASQ_CURRENCYRATE, 
                     nvl(RMACHARGE_QUOTED_SN.RMACQSN_QUOTE , 0) - nvl(RMACHARGE_QUOTED_SN.RMACQSN_DISCOUNTAMOUNT , 0) as chargeQuoted , 
                     RMACHARGE_QUOTED_SN.RMACQSN_DISCOUNTAMOUNT 
                 FROM RMA
                 INNER JOIN RMADETAIL ON RMA_NO = RMAD_RMANO AND RMAD_MARK=0
                 INNER JOIN RMAREPAIR ON RMAD_ID=RMAR_RMADID
                 INNER JOIN RMAREPAIR_QUOTED ON RMAD_ID = RMARQ_RMADID
                 LEFT OUTER JOIN RMASALE_QUOTED ON RMAD_ID = RMASQ_RMADID
                 LEFT OUTER JOIN RMACHARGE_QUOTED_SN ON RMAD_ID = RMACHARGE_QUOTED_SN.RMACQSN_RMADID
                 WHERE RMA_NO =:RMA_NO ";
            strSQL += " ORDER BY RMARQ_CSTMP asc";

            DataTable dt = new DataTable();

            Myfunc.RMAConnected();

            OracleCommand cmd = new OracleCommand(strSQL, Myfunc.FConn);
            cmd.Parameters.Add(new OracleParameter("RMA_NO", OracleDbType.NVarchar2)).Value = sRMA_NO.Trim();
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            da.Fill(dt);

            Myfunc.OracleClosed();

            return dt;
        }


        static public DataTable Get_SalesQuoted_Part(string sRMA_NO)
        {
            string strSQL = @"SELECT RMA_NO, RMAD_ID, RMARQD_NPARTNO, RMARQD_DESC, RMARQD_QTY, 
                 CASE WHEN RMACQPT_PRICE is not null THEN RMACQPT_PRICE ELSE RMARQD_PRICE END as RMARQD_PRICE, 
                 RMARQD_CURRENCYCODE, RMARQD_CURRENCYRATE, RMARQD_ASSIGEPRICE, RMARQD_ASSIGECURRENCYCODE, RMARQD_ASSIGECURRENCYRATE,
                 RMARQD_WAIVE, RMARQD_OPTION, RMARQD_OPTIONCLIENT, 
                 vwRMAREPAIR_QUOTED_DETAIL.RMACQPT_RECHARGE_PRICE
                FROM RMA
                INNER JOIN RMADETAIL ON RMA_NO = RMAD_RMANO AND RMAD_MARK=0
                INNER JOIN RMAREPAIR_QUOTED ON RMAD_ID = RMARQ_RMADID AND RMARQ_ACCEPT<>2
                INNER JOIN
                (
                     SELECT * FROM RMAREPAIR_QUOTED_DETAIL 
                     LEFT OUTER JOIN RMACHARGE_QUOTED_PART ON RMARQD_ID = RMACQPT_RMARQD_ID and RMARQD_RMADID = RMACQPT_RMADID
                     WHERE RMARQD_MARK=0
                 ) vwRMAREPAIR_QUOTED_DETAIL ON RMAD_ID = RMARQD_RMADID 
                 WHERE RMA_NO =:RMA_NO ";
            strSQL += " ORDER BY rmarqd_cstmp asc, RMARQD_ID asc";

            Myfunc.RMAConnected();

            DataTable dt = new DataTable();

            OracleCommand cmd = new OracleCommand(strSQL, Myfunc.FConn);
            cmd.Parameters.Add(new OracleParameter("RMA_NO", OracleDbType.NVarchar2)).Value = sRMA_NO.Trim();
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            da.Fill(dt);

            Myfunc.OracleClosed();

            return dt;
        }

        #endregion

        #region Import ReMoCloud

        static public DataTable GetReMoCloudData(string sSN)
        {
            StringBuilder sSQL = new StringBuilder();

            sSQL.AppendLine("SELECT a.PKEY,a.AKEY_OGB01,a.AD_ORDERNO,a.SERIAL,a.AD_EMAIL,a.AD_COMPANYNAME,a.AD_TECHNICALCONTACT,a.AD_PHONE,a.AD_STREETADDRESS,");
            sSQL.AppendLine("TO_CHAR(b.EXPORT_WARRANTY_DATE,'yyyy/mm/dd') EXPORT_WARRANTY_DATE,c.OGB04");
            sSQL.AppendLine("FROM AKEY_DETAIL_FILE a");
            sSQL.AppendLine("JOIN RMA.EXPORT b ON b.EXPORT_SERIALNO = a.SERIAL");
            sSQL.AppendLine("JOIN ogb_file c ON c.ogb01 = a.AKEY_OGB01 AND ogb03 = a.AKEY_OGB03");
            //sSQL.AppendLine("WHERE SERIAL = 'FJ1205A000798'");
            sSQL.AppendLine("WHERE SERIAL = :SERIAL AND a.AD_AKEY_TYPE = '19' ");

            DataTable dt = new DataTable();

            Myfunc.CipherlabConnected();

            OracleCommand cmd = new OracleCommand(sSQL.ToString(), FConn);
            cmd.Parameters.Add(new OracleParameter("SERIAL", OracleDbType.NVarchar2)).Value = sSN.Trim();
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            da.Fill(dt);

            Myfunc.OracleClosed();

            return dt;
        }

        static public string callWebServiceBody(string sKey, string sOrderNo, string sCompanyName, string sEmail, string sContact, string sPhone, string sShippingAddress, string sSerialNumber, string sWarrantyEndDate, string sProductNo)
        {
            string tResult = "";

            //string spKey = "1001";
            //string spOrderNo = "AXBA-200500061-1";
            //string spCompanyName = "王zhangsan ООО";
            //string spContact = "Michael~Lassen";
            //string spEmail = "vegansunsun@gmail.com";
            //string spPhone = "+88626548521";
            //string spShippingAddress = "ñńŁł Citroën";
            //string spSerialNumber = "FJ1205A000798";
            //string spWarrantyEndDate = "2023/01/01";


            //string stProductNo = "";
            OrderWebServiceClient tws = new OrderWebServiceClient();
            //OrderWebService.OrderWebServiceService tws = new OrderWebService.OrderWebServiceService();
            //tResult = tws.createOrderKeyDetailString(pKey, pOrderNo, tProductNo, pCompanyName, pEmail, pContact, pPhone, pShippingAddress, pSerialNumber, pWarrantyEndDate);
            tResult = tws.createOrderKeyDetailString(sKey, sOrderNo, sProductNo, sCompanyName, sEmail, sContact, sPhone, sShippingAddress, sSerialNumber, sWarrantyEndDate);
            return tResult;
        }

        #endregion 
    }
}
