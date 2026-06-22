Imports System.Data
Imports DataService
Imports DefLanguage
Partial Class ascx_ucWarrantyPartsView
    Inherits System.Web.UI.UserControl
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage

    'Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize_Requested")
    Dim _PageSize As String = "15"




    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Call setDefault()
        End If
    End Sub


    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setDefault()
        '取得Tag Text
        'Me.UI_lblAddressTittle.Text = _oLanguage.getText("Warranty", "055", ctlLanguage.eumType.Tag)

        Me.UI_cmdClose.Text = _oLanguage.getText("Common", "008", ctlLanguage.eumType.Command)

        Me.lblPartPO.Text = _oLanguage.getText("Transfer", "004", ctlLanguage.eumType.Word)
        Me.lblPartContent.Text = _oLanguage.getText("Transfer", "005", ctlLanguage.eumType.Word)
        Me.lblPartMonth.Text = _oLanguage.getText("Transfer", "006", ctlLanguage.eumType.Word)
        Me.lblPartExtra.Text = _oLanguage.getText("Transfer", "007", ctlLanguage.eumType.Word)
        Me.lblPartMemo.Text = _oLanguage.getText("Transfer", "008", ctlLanguage.eumType.Word)
        Me.lblPartEndDate.Text = _oLanguage.getText("Transfer", "009", ctlLanguage.eumType.Word)

        Me.lblTitle.Text = _oLanguage.getText("Transfer", "036", ctlLanguage.eumType.Word)

    End Sub
    Public Sub show(ByVal isShow As Boolean, ByVal RMAD_SERIALNO As String, ByVal RMAD_MODELNO As String, ByVal OperationCenter As String)
        Me.ViewState("_show") = isShow

        Dim dtWarrParts As DataTable = GetData(RMAD_SERIALNO, RMAD_MODELNO, OperationCenter)
        Me.ViewState("dtWarrParts") = dtWarrParts
        lstParts.DataSource = dtWarrParts
        lstParts.DataBind()


        If Convert.ToBoolean(Me.ViewState("_show")) = True Then
            Me.ajModalProgress.Show()
        Else
            Me.ajModalProgress.Hide()
        End If

    End Sub

    Public Function GetData(ByVal RMAD_SERIALNO As String, ByVal RMAD_MODELNO As String, ByVal OperationCenter As String) As DataTable
        If OperationCenter <> "CL_CHINA" Then
            OperationCenter = "CLHQ"
        End If

        'Response.Write("RMAD_SERIALNO " + RMAD_SERIALNO + "-RMAD_MODELNO " + RMAD_MODELNO)

        Dim oExport As New ctlRMA.Export
        Dim sEWEnd As String = oExport.getMaxWarranty(RMAD_SERIALNO, Session("_CustomerID").ToString(), Session("_RepairID").ToString())
        Dim sCWEnd As String = oExport.getWarrantyCW(RMAD_SERIALNO, "")
        Dim sSWEnd As String = oExport.getWarrantySW(RMAD_SERIALNO, "")
        Dim sWarDate As String = oExport.getWarrantyStart(RMAD_SERIALNO)
        Dim sWarVersion As String = String.Empty

        Dim sWar_id As String = ""
        Dim oWarranty As New ctlWarranty
        Dim dtPur As DataTable = oWarranty.QueryWarrantyPO(RMAD_SERIALNO)
        If dtPur.Rows.Count > 0 Then
            sWar_id = dtPur.Rows(0)("wati_ver").ToString()
            'sWarDate = DateTime.Parse(dtPur.Rows(0)("waty_date").ToString()).ToString("yyyy/MM/dd")
        End If

        If sWar_id = "" Then
            Dim dtEwVer As New WarrantyDTO.WARRSETDataTable
            If sCWEnd.Trim() <> "" Then
                dtEwVer = oWarranty.QueryWarrSet("", OperationCenter, RMAD_MODELNO, "CW", "", "Y", "WAR_VERSION")
            Else
                dtEwVer = oWarranty.QueryWarrSet("", OperationCenter, RMAD_MODELNO, "EW", "", "Y", "WAR_VERSION")
            End If

            If dtEwVer.Rows.Count > 0 Then
                If dtEwVer.Rows(0)("WAR_VERSION").ToString().Trim().Equals("0") Then
                    sWar_id = dtEwVer.Rows(0)("WAR_ID").ToString()
                End If

                '20200217 wisely modify 抓訂單細目的版本
                sWarVersion = oWarranty.QueryWARVERSION(RMAD_SERIALNO)
                If sWarVersion <> String.Empty Then
                    Dim find_rows As DataRow() = dtEwVer.Select("WAR_VERSION='" + sWarVersion + "'")
                    If find_rows.Length > 0 Then
                        sWar_id = find_rows(0)("WAR_ID").ToString()
                    End If
                End If

            End If
        End If

        If sWarDate <> "" Then
            sWarDate = DateTime.Parse(sWarDate).ToString("yyyy/MM/dd")
        End If

        If sWar_id = "" Then
            sWar_id = "123$321"
        End If

		lblTitle2.Text = sWar_id
		'Response.Write(sWar_id)
		'Response.End

        Dim dtWarrParts As New WarrantyDTO.WarrPartsDataTable
        'sWar_id = "9700CW00125"
        dtWarrParts = oWarranty.QueryWarrParts(Session("_LanguageID").ToString(), sWar_id, RMAD_SERIALNO, "")
        dtWarrParts.Columns.Add("PODate")
        dtWarrParts.Columns.Add("WarrEndDate")

        Dim i As Integer
        For i = 0 To dtWarrParts.Rows.Count - 1
            dtWarrParts.Rows(i)("PODate") = sWarDate
            If sWarDate <> "" Then
                dtWarrParts.Rows(i)("WarrEndDate") = DateTime.Parse(sWarDate).AddMonths(Double.Parse(dtWarrParts.Rows(i)("WAP_MON").ToString()) + Double.Parse(dtWarrParts.Rows(i)("WAP_EMON").ToString())).AddDays(-1).ToString("yyyy/MM/dd")
            End If
        Next

        Return dtWarrParts
    End Function

End Class
