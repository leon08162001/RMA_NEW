Imports System.Data
Imports DataService
Imports DefLanguage
Partial Class ascx_ucWarrantyOrderSNView
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
    End Sub


    Private Sub QueryData(ByVal dtWarrantySerial As DataTable, ByVal iPageIndex As Integer)
        Me.UI_dvAddress.PageSize = _PageSize
        Me.UI_dvAddress.PageIndex = iPageIndex
        Me.UI_dvAddress.DataSource = dtWarrantySerial.DefaultView
        Me.UI_dvAddress.DataBind()
    End Sub
    Protected Sub UI_dvAddress_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UI_dvAddress.RowDataBound
        Dim i As Integer = 0
        If e.Row.RowType = DataControlRowType.Header Then
            'e.Row.Cells(1).Text = _oLanguage.getText("Warranty", "056", ctlLanguage.eumType.Tag)
            'e.Row.Cells(2).Text = _oLanguage.getText("Warranty", "057", ctlLanguage.eumType.Tag)
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



    Protected Sub UI_dvAddress_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles UI_dvAddress.PageIndexChanging
        Dim iPageIndex As Integer = e.NewPageIndex.ToString()

        Dim dtWarrantySerial As DataTable = Me.ViewState("_dtWarrantySerial")
        Call QueryData(dtWarrantySerial, iPageIndex)
        Me.ajModalProgress.Show()
    End Sub

    Public Sub show(ByVal isShow As Boolean, ByVal wats_watyno As String, ByVal wats_watyseq As Integer)
        Me.ViewState("_show") = isShow

        Dim oWarranty As New ctlWarranty
        Dim dtWarrantySerial As New WarrantyDTO.WARRANTYSERIALDataTable
        dtWarrantySerial = oWarranty.QueryWarrantySerial(wats_watyno, wats_watyseq, "", "wats_sn")

        Me.ViewState("_dtWarrantySerial") = dtWarrantySerial

        If Convert.ToBoolean(Me.ViewState("_show")) = True Then
            Call QueryData(dtWarrantySerial, 0)

            Me.ajModalProgress.Show()
        Else
            Me.ajModalProgress.Hide()
        End If

    End Sub


End Class
