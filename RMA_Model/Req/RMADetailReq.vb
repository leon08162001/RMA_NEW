
Public Class RMADetailReq
    Public Property RMAD_ID As String
    Public Property RMAD_SEQ As String
    Public Property RMAD_RMANO As String
    Public Property RMAD_MODELNO As String
    Public Property RMAD_SERIALNO As String
    Public Property RMAD_CUSNAME As String
    Public Property RMAD_WARRANTY As String
    Public Property RMAD_FARFARCNO As String
    Public Property RMAD_FARNO As String
    Public Property RMAD_UPLOADFILE As String
    Public Property RMAD_PROBLEMDESC As String
    Public Property RMAD_STATUS As String
    Public Property RMAD_ISFILL As String
    Public Property RMAD_RECVAD As String
    Public Property RMAD_RECVADNAME As String
    Public Property RMAD_RECVDATE As String
    Public Property RMAD_RECEVSTATUS As String
    Public Property RMAD_AD As String
    Public Property RMAD_ADNAME As String
    Public Property RMAD_CSTMP As DateTime
    Public Property RMAD_LUAD As String
    Public Property RMAD_LUADNAME As String
    Public Property RMAD_LUSTMP As DateTime
    Public Property RMAD_MARK As String
    Public Property RMAD_PRODUCTDESC As String
    Public Property RMAD_ISWARRANTY As String
    Public Property RMAD_ISCW As String
    Public Property RMAD_ISSW As String
    Public Property RMAD_PARTSN As String
    Public Property CUSTOMER_PRODUCT_NUMBER As String
    Public Property QUERY_ALERT_READ As String
    Public Property RMAD_APPLY_BI As String
    Public Property RMAD_REJAD As String
    Public Property RMAD_REJADNAME As String
    Public Property RMAD_REJSTMP As DateTime

    Public Property RepairQuoted As List(Of RMARepairQuotedReq)
    Public Property SaleQuoted As List(Of RMASaleQuotedReq)
End Class