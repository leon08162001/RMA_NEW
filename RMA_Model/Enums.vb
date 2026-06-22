Imports System.ComponentModel

Public Class Enums

    Public Class ProgramName
        Public Const ExpiredQuery As String = "ExpiredQuery"
        Public Const MainMenu As String = "MainMenu"
        Public Const Report3_Search As String = "Report3_Search"
    End Class
    ''' <summary>
    ''' 定義所有結果代碼
    ''' </summary>
    Public Enum ResultCodeEnum


        ''' <summary>
        ''' 執行成功
        ''' </summary>
        OK = 0
        ''' <summary>
        ''' 執行失敗
        ''' </summary>
        FAIL = -1
        ''' <summary>
        ''' 拒絕存取
        ''' </summary>
        AuthError = 100001

        ''' <summary>
        ''' 資料驗證錯誤
        ''' </summary>
        ValidationError = 200001

        ''' <summary>
        ''' 資料不存在
        ''' </summary>
        DataNotFoundDBError = 200002

        ''' <summary>
        ''' 資料不允許修改
        ''' </summary>
        ModifyNotAllowedDBError = 200003

        ''' <summary>
        ''' 資料不允許新增
        ''' </summary>
        InsertNotAllowedDBError = 200004

        ''' <summary>
        ''' 資料已存在
        ''' </summary>
        DataExistedDBError = 200005

        ''' <summary>
        ''' 無角色權限，前端會強制登出，轉址至ADFS登入頁面
        ''' </summary>
        EIAMAuthError = 200006

        ''' <summary>
        ''' 資料重複
        ''' </summary>
        DataDuplicationDBError = 200007

        ''' <summary>
        ''' DB存取錯誤
        ''' </summary>
        DBError = 300001

        ''' <summary>
        ''' 呼叫API錯誤
        ''' </summary>
        ServiceError = 400001

        ''' <summary>
        ''' 系統錯誤
        ''' </summary>
        APError = 500001

    End Enum

    Public Enum RMAstatus
        RequestedTmp = 0
        Requested = 10
        inProgress = 20
        Closed = 90
        Canceled = 91
    End Enum
    Public Enum RMADstatus
        RequestedTmp = 0
        Requested = 10
        Received = 20
        RepairQuoted = 30
        SalesQuoted = 35
        SalesConfirmed = 40
        CustomerConfirmed = 50
        Repaired = 60
        Shipped = 70
        Closed = 90
        Canceled = 91
    End Enum

    Enum Type As Integer
        Tag = 1
        Validator = 2
    End Enum

    Enum Message As Integer
        AddOK = 1
        EditOK = 2
        DelOK = 3
        MailOK = 4
        ConfirmOK = 5
        ChangePassWordOK = 6
        ProcessOK = 7
    End Enum
    Enum Role As Integer
        View = 0
        Receiver = 1
        Repair = 2
        Sales = 3
        Shipping = 4
        Admin = 9
    End Enum

    Public Enum WarrantyType
        M0
        EO
        P0
        EB
        PB
        EW
        CW
    End Enum

    ''' <summary>
    ''' 列舉-維修中心(Comp_No)
    ''' </summary>
    Enum RepairCenter
        <Description("AU")>
        AU
        <Description("AUS")>
        AUS
        <Description("AU_LAPTOP_KINGS")>
        CL_AU_Service_Center
        <Description("AC_GMBH")>
        CL_GMBH_Service_Center
        <Description("CLHQ")>
        CL_TW_Service_Center
        <Description("CEAT")>
        CL_EU_Service_Center
        <Description("CL_CHINA")>
        CL_CN_Service_Center
        <Description("CL_USA")>
        CL_US_Service_center
        <Description("UK_FALA")>
        CL_UK_Service_Centre
        <Description("JP_BYTE")>
        CL_JP_Service_Center
        <Description("JP_BYTE_MPLUS")>
        MPlus_JP_Service_Center
        <Description("NZ_PB_TECH")>
        CL_NZ_Service_Center
        <Description("US_CL_MPLUS")>
        MPlus_US_Service_Center
        <Description("US_CL_CLUSA")>
        CU_US_Service_center
        <Description("JRC")>
        CL_JRC_Service_Center
    End Enum

End Class

