<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FileUpload.aspx.vb" Inherits="FileUpload" %>

<%@ Register TagPrefix="Upload" Namespace="Brettle.Web.NeatUpload" Assembly="Brettle.Web.NeatUpload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <style>
        .Upload {
            height: 19px;
        }

        .Button_Upload {
            FONT-SIZE: 11px;
            width: 50px;
            height: 19px;
            border: medium none;
            color: #000000;
            TEXT-DECORATION: Bold;
            cursor: pointer;
            text-valign: top;
            text-align: center;
            font-weight: bold;
            font-family: Arial, Helvetica, sans-serif;
            padding-left: 5px;
            background: url('images/icon_bg.gif');
        }
    </style>
</head>

<body bgcolor="#E3D8BE">
    <form id="form1" runat="server">
        <%--<asp:FileUpload runat="server" id="html_FileUpload" Width="200px" CssClass="Upload"></asp:FileUpload>--%>

        <Upload:InputFile ID="html_FileUpload" runat="server" Width="200px" CssClass="Upload" />
        <asp:Button runat="server" ID="butOK" Text="037_Upload" CssClass="Button_Upload" ValidationGroup="vsRepairUploadGroup" />
        <asp:TextBox runat="server" ID="html_File" Visible="false"></asp:TextBox>
        <asp:TextBox runat="server" ID="html_FullFile" Visible="false"></asp:TextBox>
        <asp:TextBox runat="server" ID="html_FilePath" Visible="false"></asp:TextBox>
        <asp:Label runat="server" ID="lblScript"></asp:Label>



        <Upload:ProgressBar ID="progressBar" runat="server" Triggers="submitButton linkButton commandButton htmlInputButtonButton htmlInputButtonSubmit">
            <asp:Label ID="label" runat="server" Text="Check Progress" />
        </Upload:ProgressBar>

        <asp:RequiredFieldValidator ID="rfvhtmlFullFile" runat="server" ErrorMessage="121_請選取要上傳的檔案" Display="None" ControlToValidate="html_FileUpload" ValidationGroup="vsRepairUploadGroup"></asp:RequiredFieldValidator>
        <asp:ValidationSummary ID="vsRepairUpload" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="vsRepairUploadGroup" />

    </form>
</body>
</html>
