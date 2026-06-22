<%@ Page Language="VB" AutoEventWireup="false" CodeFile="BuidLanguage.aspx.vb" Inherits="BuidLanguage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>未命名頁面</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="Button1" runat="server" Text="產生語系XML" />
            <br />
            <br />
            <br />
            <asp:TextBox runat="server" ID="UI_XML" Text="D:\Project\欣技\eRMA\Web\Bin\English.xml" Width="300px"></asp:TextBox>
            <asp:Button ID="Button2" runat="server" Text="語系XML匯入DATA " />
        </div>
    </form>
</body>
</html>
