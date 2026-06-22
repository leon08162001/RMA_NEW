Imports System.Data
Imports DataService
Imports DefLanguage


Partial Class ascx_ucModel
    Inherits System.Web.UI.UserControl
    Dim oCommon As New Common
    Dim _oLanguage As New ctlLanguage

    'Dim _PageSize As String = ConfigurationSettings.AppSettings("PageSize_Requested")
    Dim _PageSize As String = "5"


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Call setDefault()
            Call QueryData(0)
        End If
    End Sub

    ''' <summary>
    ''' 設定控制項
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setDefault()
        '取得Tag Text
        Me.UI_lblModelTittle.Text = _oLanguage.getText("RMA", "018", ctlLanguage.eumType.Tag)
        Me.UI_lblModel.Text = _oLanguage.getText("RMA", "019", ctlLanguage.eumType.Tag)

        Me.UI_cmdSearch.Text = _oLanguage.getText("Common", "004", ctlLanguage.eumType.Command)
        Me.UI_cmdClose.Text = _oLanguage.getText("Common", "008", ctlLanguage.eumType.Command)
        Me.UI_cmdSubmit.Text = _oLanguage.getText("Common", "001", ctlLanguage.eumType.Command)
    End Sub



    ''' <summary>
    ''' 查詢
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSearch.Click
        Call QueryData(0)
        Me.ajModalProgress.Show()
    End Sub

    Private Sub QueryData(ByVal iPageIndex As Integer)
        Dim oModel As New ctlRMA.Model
        Dim dtModel As New RmaDTO.ModelDataTable

        Dim sModel As String = Me.UI_txtModel.Text.ToString().Trim()

        dtModel = oModel.Query(sModel)

        Me.UI_dvModel.PageSize = _PageSize
        Me.UI_dvModel.PageIndex = iPageIndex
        Me.UI_dvModel.DataSource = dtModel.DefaultView
        Me.UI_dvModel.DataBind()
    End Sub

    Protected Sub UI_dvModel_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvModel.RowDataBound
        Dim i As Integer = 0
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Text = _oLanguage.getText("RMA", "020", ctlLanguage.eumType.Tag)
            e.Row.Cells(2).Text = _oLanguage.getText("RMA", "021", ctlLanguage.eumType.Tag)
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim raoModelNo As RadioButton = e.Row.FindControl("raoModelNo")
            '先清除
            raoModelNo.Checked = False
        End If

        If e.Row.RowType = DataControlRowType.Pager Then
            Dim iLoop As Integer = e.Row.Cells(0).Controls(0).Controls(0).Controls.Count
            For i = 0 To iLoop - 1
                If e.Row.Cells(0).Controls(0).Controls(0).Controls(i).Controls(0).GetType.Name.ToLower() = "Label".ToLower() Then
                    Dim oLabel As Label = e.Row.Cells(0).Controls(0).Controls(0).Controls(i).Controls(0)
                    oLabel.ForeColor = Drawing.Color.Red
                    oLabel.Text = "&nbsp;(" & oLabel.Text & ")&nbsp;"
                End If
            Next
        End If
    End Sub

    Protected Sub UI_dvModel_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvModel.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        Call QueryData(iPageIndex)
        Me.ajModalProgress.Show()
    End Sub





    ''' <summary>
    ''' 新增Model
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub UI_cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_cmdSubmit.Click
        Dim i As Integer = 0
        Dim dtRMADetail As New RmaDTO.RMADetailDataTable

        For i = 0 To Me.UI_dvModel.Rows.Count - 1
            Dim sModelNo As String = ""
            Dim raoModelNo As RadioButton = Me.UI_dvModel.Rows(i).FindControl("raoModelNo")
            Dim sMoelNo As Label = Me.UI_dvModel.Rows(i).FindControl("UI_MoelNo")

            If raoModelNo.Checked = True Then
                Dim UI_txtModel As TextBox = Me.Parent.FindControl("UI_txtModel")
                UI_txtModel.Text = sMoelNo.Text.Trim()

                'If IsNothing(Session("_dtRMADetail")) = True Then
                '    dtRMADetail = AddRMADetailModel(sMoelNo.Text.Trim(), dtRMADetail)
                'Else
                '    dtRMADetail = Session("_dtRMADetail")
                '    dtRMADetail = AddRMADetailModel(sMoelNo.Text.Trim(), dtRMADetail)
                'End If
            End If
        Next

        'Session("_dtRMADetail") = dtRMADetail
        'Dim UI_dvRMADetail As GridView = Me.Parent.FindControl("UI_dvRMADetail")
        'dtRMADetail = Session("_dtRMADetail")
        'Call ArrangementData(dtRMADetail)
        'UI_dvRMADetail.DataSource = dtRMADetail
        'UI_dvRMADetail.DataBind()
    End Sub

    Private Sub ArrangementData(ByVal dtRMADetail As RmaDTO.RMADetailDataTable)
        Dim i As Integer = 0

        If dtRMADetail.Columns("SeqID") Is Nothing Then
            dtRMADetail.Columns.Add("SeqID")
        End If

        For i = 0 To dtRMADetail.Rows.Count - 1
            dtRMADetail.Rows(i)("SeqID") = i + 1

            dtRMADetail.Rows(i)("RMAD_sWARRANTY") = _oLanguage.getText("RMA", "080", ctlLanguage.eumType.Tag)
            If IsDate(dtRMADetail.Rows(i)("RMAD_WARRANTY")) = True Then
                dtRMADetail.Rows(i)("RMAD_sWARRANTY") = Convert.ToDateTime(dtRMADetail.Rows(i)("RMAD_WARRANTY")).ToShortDateString
            End If

        Next
    End Sub

    ''' <summary>
    ''' 新增 報修品項
    ''' </summary>
    ''' <param name="dtRMADetail"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddRMADetailModel(ByVal sMoelNo As String, ByVal dtRMADetail As RmaDTO.RMADetailDataTable) As RmaDTO.RMADetailDataTable
        Dim drRMADetail As RmaDTO.RMADetailRow = dtRMADetail.NewRMADetailRow

        Try
            Dim oGuid As Guid = Guid.NewGuid
            Dim sGUID As String = oGuid.ToString

            drRMADetail.RMAD_ID = sGUID.ToString().Trim()
            drRMADetail.RMAD_SEQ = 0

            drRMADetail.RMAD_RMANO = ""
            drRMADetail.RMAD_MODELNO = sMoelNo.Trim()
            drRMADetail.RMAD_SERIALNO = ""
            drRMADetail.RMAD_CUSNAME = ""                   'Customer Product Name
            drRMADetail.RMAD_sWARRANTY = ""                 'Warranty 字串格式

            drRMADetail.RMAD_FARFARCNO = "-1"
            drRMADetail.RMAD_FARNO = "-1"
            drRMADetail.RMAD_UPLOADFILE = ""
            drRMADetail.RMAD_PROBLEMDESC = ""
            drRMADetail.RMAD_STATUS = 0

            drRMADetail.RMAD_AD = Session("_UserID")
            drRMADetail.RMAD_ADNAME = Session("_UserName")
            drRMADetail.RMAD_CSTMP = Date.Now
            drRMADetail.RMAD_LUAD = Session("_UserID")
            drRMADetail.RMAD_LUADNAME = Session("_UserName")
            drRMADetail.RMAD_LUSTMP = Date.Now
            drRMADetail.RMAD_MARK = 0

            drRMADetail.RMAD_ISFILL = 0             '是否已填寫問題:0.否, 1.是
            drRMADetail.RMAD_RECEVSTATUS = 0        '是否收貨:0.尚未收貨, 1.已收貨, 2.刪除

            dtRMADetail.AddRMADetailRow(drRMADetail)


        Catch ex As Exception
            Throw ex

        End Try
        Return dtRMADetail
    End Function




    ''' <summary>
    ''' show
    ''' </summary>
    ''' <param name="sModel"></param>
    ''' <param name="isShow"></param>
    ''' <remarks></remarks>
    Public Sub show(ByVal sModel As String, ByVal isShow As Boolean)
        Me.ViewState("_show") = isShow

        If Convert.ToBoolean(Me.ViewState("_show")) = True Then
            'Me.UI_txtModel.Text = sModel.Trim()
            Call QueryData(0)

            Me.ajModalProgress.Show()
        Else
            Me.ajModalProgress.Hide()
        End If

    End Sub

End Class
