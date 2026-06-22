<%@ page language="VB" autoeventwireup="false" inherits="menu, App_Web_nhysuesw" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<link rel="stylesheet" type="text/css" href="../script/main.css" />
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        body 
        {
	        background-image: url(images6/bg.gif);
	        
	        	        
        }
    </style>
    <title >e-RMA Query</title>

</head>
<body>
     <form id="form1" runat="server">
    <table class="style1" style="width: 100%">
    <tr>
        <td style="height: 50px; width: 30%;"> <asp:Image ID="Image1" runat="server" ImageUrl="~/images/logo.gif" /> </td>
        <td style="height: 50px; font-weight: bold; font-size: large; width: 40%; color: #000099; font-family: 新細明體; text-align: center;" rowspan="">
            &nbsp;</td>
        <td style="height: 50px; width: 30%;">  </td>
    </tr>
    </table>
<ul id="navbar">
<li>
 <a href="ExpRMAData.aspx" target="bodyFrame">Export RMA Data</a>   
</li>
<li>
 <a href="ExpRMAData2.aspx" target="bodyFrame">Replace Parts Report</a>   
</li>
<li>
 <a href="SNQueryList.aspx" target="bodyFrame">Delivery Query (Packing, MES, ERP)</a>   
</li>
<li>
 <a href="ExpRMAData4.aspx" target="bodyFrame">Repair Quote Report</a>   
</li>

</ul>
    </form>
</body>
</html>
