<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SubMenu.ascx.vb" Inherits="ascx_SubMenu" %>


<td background="images/pic_11.gif" width="186" valign="top" class="default" height="350px">
    <p style="margin-left: 10px; margin-right: 10px; margin-top: 10px">
        <b><img border="0" src="Images/icon-arrow.gif" width="5" height="9">
        <asp:Label runat="server" ID="UI_lblRGAccount" Text="035_Account" CssClass="default"></asp:Label>:
        </b>
        <br>&nbsp;<asp:Label runat="server" ID="UI_lblAccountText" Text="035_Account" CssClass="default"></asp:Label>
        <br>&nbsp;<asp:Label runat="server" ID="UI_lblRoleNameText" Text="067_RoleName" CssClass="default"></asp:Label>
        
    <p style="margin-left: 10px; margin-right: 10px; margin-top: 10px">
        <b><img border="0" src="Images/icon-arrow.gif" width="5" height="9">
        <asp:Label runat="server" ID="UI_lblRepair" Text="036_Repair Center" CssClass="default"></asp:Label>:
        </b>
        <br>&nbsp;<asp:Label runat="server" ID="UI_lblCUNAME" CssClass="default"></asp:Label>
        <br>&nbsp;<asp:Label runat="server" ID="UI_lblCUTEL" CssClass="default"></asp:Label>
    </p>
        <hr>
        
<asp:Panel runat="server" ID="UI_panSearch" DefaultButton="UI_imgSearch">
    <p style="margin-left: 10px; margin-right: 10px; margin-top: 10px"><b>
        <img border="0" src="images/icon-arrow.gif" width="5" height="9"><asp:Label ID="bl_RmaQuery" runat="server" Text="RMA Status Query"></asp:Label></b>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox runat="server" ID="UI_globalRMAID" CssClass="default" style="WIDTH: 120px"></asp:TextBox>
        <asp:ImageButton runat="server" ID="UI_imgSearch" ImageUrl="../images/go.gif" />
    </p>
</asp:Panel>
        <hr>

        
    <p style="margin-left: 10px; margin-right: 10px; margin-top: 10px">
        <b>
        
        <table runat="server" id="UI_tbUser" border="0" width="95%" cellspacing="0" cellpadding="0" align="center" visible="false">
            <tr><td align="left" colspan="2" class=default>&nbsp;</td></tr>
            
            <tr>
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9" >&nbsp;</td>
                <td><asp:HyperLink ID="UI_linFAQ" runat="server" Text="FAQ" CssClass="menu-1" NavigateUrl="~/FAQ_List.aspx" Visible="True"></asp:HyperLink></td>
            </tr>
             <tr>
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9" >&nbsp;</td>
                <td><asp:HyperLink ID="UI_linDOA" runat="server" Text="DOA" CssClass="menu-1" Target="_blank"  NavigateUrl="~/GoToDOA.aspx" Visible="True"></asp:HyperLink></td>
            </tr>
            <tr>
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="UI_linCustomer" runat="server" Text="Account Setting" CssClass="menu-1" NavigateUrl="~/Customer.aspx"></asp:HyperLink></td>
            </tr>
        </table>


        <table runat="server" id="UI_tbProcess" border="0" width="95%" cellspacing="0" cellpadding="0" align="center" visible="false">
            <tr>
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="UI_linFAQ_Process" runat="server" Text="FAQ" CssClass="menu-1" NavigateUrl="~/FAQ_List.aspx"></asp:HyperLink></td>
            </tr>
            	<tr>
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="UI_linDOA_Process" runat="server" Text="DOA" CssClass="menu-1" Target="_blank"  NavigateUrl="~/GoToDOA.aspx"></asp:HyperLink></td>
            </tr>
            <tr runat="server" id="UI_trAccount_Process" visible="false">
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="UI_linAccount_Process" runat="server" Text="Account Setting" CssClass="menu-1" NavigateUrl="~/MaintainAccount_Edit.aspx"></asp:HyperLink></td>
            </tr>

            <tr runat="server" id="UI_trShipment" visible="false">
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="UI_linShipment" runat="server" Text="Shipment Notice" CssClass="menu-1" NavigateUrl="~/Shipment_List.aspx"></asp:HyperLink></td>
            </tr>
            
            <tr runat="server" id="UI_trShipping" visible="false">
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="UI_linShipping" runat="server" Text="Shipping" CssClass="menu-1" NavigateUrl="~/Shipping_List.aspx"></asp:HyperLink></td>
            </tr>

            <tr runat="server" id="UI_trExtend_1" visible="true">
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="UI_linExtend_1" runat="server" Text="Extend_1" CssClass="menu-1" Target="_blank"></asp:HyperLink></td>
            </tr>

            <tr runat="server" id="UI_trExtend_2" visible="true">
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="UI_linExtend_2" runat="server" Text="Export" CssClass="menu-1" Target="_blank" NavigateUrl="~/extend/ext_main2.aspx"></asp:HyperLink></td>
            </tr>

            <tr runat="server" id="UI_HQQuote" visible="false">
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="UI_linHQQuote" runat="server" Text="HQ Quote" CssClass="menu-1" NavigateUrl="~/HQQuote_List.aspx"></asp:HyperLink></td>
            </tr>

            <tr runat="server" id="UI_ChargeQUOTED" visible="false">
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="UI_linChargeQUOTED" runat="server" Text="Charge Quoted" CssClass="menu-1" NavigateUrl="~/ChargeQuoted_List.aspx"></asp:HyperLink></td>
            </tr>

            <tr runat="server" id="UI_HQRepairList" visible="false">
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="UI_linRepairList" runat="server" Text="HQ Quote" CssClass="menu-1" NavigateUrl="~/Repair_List.aspx"></asp:HyperLink></td>
            </tr>
            <tr>
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="UI_linDecrypt_Process" runat="server" Text="Decrypt Slip Number" CssClass="menu-1" NavigateUrl="~/SlipNo_Search.aspx"></asp:HyperLink></td>
            </tr>
            <tr>
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="UI_Maintenance_Statement" runat="server" Text="Maintenance Report" CssClass="menu-1" NavigateUrl="~/Maintenance_Statement.aspx" Visible="false"></asp:HyperLink></td>
            </tr>
        </table>


        <table runat="server" border="0" width="90%" id="UI_tbAdmin" cellspacing="0" cellpadding="0" align="center" visible="false">
            <tr>
                <td align="left" colspan="2" class=default><font color="#FFFFFF">&nbsp;<asp:Label ID="UI_lblManagement" runat ="server" Font-Bold="true" Text ="Management-Setting"></asp:Label></font></td>
            </tr>
            <tr>
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="UI_linRepairCenter" runat="server" Text="Repair Center" CssClass="menu-1" NavigateUrl="~/RepairCenter_List.aspx"></asp:HyperLink></td>
            </tr>
            
            <tr>
                <td align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="UI_linCustomer_Search" runat="server" Text="Customer Setting" CssClass="menu-1" NavigateUrl="~/Customer_Search.aspx"></asp:HyperLink></td>
            </tr>
            <tr>
                <td align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="UI_linExchange" runat="server" Text="Exchange Rate" CssClass="menu-1" NavigateUrl="~/ExchangeRate.aspx"></asp:HyperLink></td>
            </tr>
            
        
           <%-- <tr>
                <td align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="UI_linWarranty" runat="server" Text="Warranty" CssClass="menu-1" NavigateUrl="~/Warranty.aspx"></asp:HyperLink></td>
            </tr>--%>
        

            <tr>
                <td align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="UI_linCountry" runat="server" Text="Country" CssClass="menu-1" NavigateUrl="~/Country.aspx"></asp:HyperLink></td>
                
            </tr>
            <tr>
                <td align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="UI_linFailure" runat="server" Text="Failure Reason" CssClass="menu-1" NavigateUrl="~/Failure_List.aspx"></asp:HyperLink></td>
            </tr>

            <tr>
                <td align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="UI_linDefective" runat="server" Text="Defective" CssClass="menu-1" NavigateUrl="~/Defective_List.aspx"></asp:HyperLink></td>
            </tr>

            <tr>
                <td align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="UI_linContentPolicy" runat="server" Text="Content-Policy" CssClass="menu-1" NavigateUrl="~/Policy.aspx"></asp:HyperLink></td>
            </tr>
            <tr>
                <td align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="UI_linContentFAQ" runat="server" Text="Content-FAQ" CssClass="menu-1" NavigateUrl="~/FAQ_Search.aspx"></asp:HyperLink></td>
            </tr>
            <tr>
                <td align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="UI_linMaintainAccount" runat="server" Text="Maintain Account" CssClass="menu-1" NavigateUrl="~/MaintainAccount_Search.aspx"></asp:HyperLink></td>
            </tr>
            <tr>
                <td align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="UI_linMantainMail" runat="server" Text="Mantain Mail" CssClass="menu-1" NavigateUrl="~/MantainMail.aspx"></asp:HyperLink></td>
            </tr>
            <tr>
                <td align="left" colspan="2" class=default><font color="#FFFFFF">&nbsp;<asp:Label ID="UI_lblAdminReport" runat ="server" Font-Bold="true" Text ="Admin Report"></asp:Label></font></td>
            </tr>
            <tr>
                <td align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="UI_linAccountReport" runat="server" Text="Check Account Report" CssClass="menu-1" NavigateUrl="~/Report10_Search.aspx"></asp:HyperLink></td>
            </tr>
        </table>
        
       <table runat="server" border="0" width="90%" id="UI_tbWarrantySetting" cellspacing="0" cellpadding="0" align="center" visible="false">
            <tr>
                <td align="left" colspan="2" class=default><font color="#FFFFFF">&nbsp;<asp:Label ID="Label1" runat ="server" Font-Bold="true" Text ="Warranty Selling"></asp:Label></font></td>
            </tr>
            <tr>
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="UI_linWarrantyExtension" runat="server" Text="Ext & Com W.Setting" CssClass="menu-1" NavigateUrl="~/Warranty_TypeSetting.aspx"></asp:HyperLink></td>
            </tr>
            <tr>
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="UI_linWarrantySpecialSetting" runat="server" Text="Special W.Setting" CssClass="menu-1" NavigateUrl="~/Warranty_SpecialSetting.aspx"></asp:HyperLink></td>
            </tr>
            <tr>
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="UI_linWarrantyGroupSetting" runat="server" Text="Warranty Group Setting" CssClass="menu-1" NavigateUrl="~/Warranty_GroupSetting.aspx"></asp:HyperLink></td>
            </tr>
        </table>  
       <table runat="server" border="0" width="90%" id="UI_tbWarrantyOrder" cellspacing="0" cellpadding="0" align="center" visible="false">
           <tr>
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="UI_linWarrantyBatch" runat="server" Text="Batch Serial Search" CssClass="menu-1" NavigateUrl="~/Warranty_SerialSearch.aspx"></asp:HyperLink></td>
            </tr>
            <tr>
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="UI_linSellingafterOrder" runat="server" Text="Warranty Sales Order" CssClass="menu-1" NavigateUrl="~/Warranty_SellingafterOrder.aspx"></asp:HyperLink></td>
            </tr>
        </table>  
       
        <table runat="server" id="UI_tbReports" border="0" width="95%" cellspacing="0" cellpadding="0" align="center" visible="false">
            <tr id="trRPT1" runat="server" visible="false">
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="ui_HrefReport1" runat="server" Text="001_Part's Query" CssClass="menu-1"></asp:HyperLink></td>
            </tr>

            <tr id="trRPT2" runat="server" visible="false">
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="ui_HrefReport2" runat="server" Text="002_Repair Frequency Query" CssClass="menu-1"></asp:HyperLink></td>
            </tr>

            <tr id="trRPT3" runat="server" visible="true">
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="ui_HrefReport3" runat="server" Text="003_Warranty Query" CssClass="menu-1"></asp:HyperLink></td>
            </tr>

            <tr id="trRPT4" runat="server" visible="true">
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="ui_HrefReport4" runat="server" Text="004_RMA Detail Report" CssClass="menu-1"></asp:HyperLink></td>
            </tr>

            <tr id="trRPT5" runat="server" visible="false">
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td>
                    <asp:HyperLink ID="ui_HrefReport5" runat="server" Text="005_Repair Center Monthly Report" CssClass="menu-1"></asp:HyperLink>
                </td>
            </tr>

            <tr id="trRPT6" runat="server" visible="false">
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td>
                    <asp:HyperLink ID="ui_HrefReport6" runat="server" Text="006_Over Due Report" CssClass="menu-1" Visible="false"></asp:HyperLink>
                </td>
            </tr>


            <tr id="trRPT7" runat="server" visible="false">
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="ui_HrefReport7" runat="server" Text="007_Customers Request Report" CssClass="menu-1"></asp:HyperLink></td>
            </tr>

            <tr id="trRPT8" runat="server" visible="false">
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="ui_HrefReport8" runat="server" Text="008_Failure Reason Report" CssClass="menu-1"></asp:HyperLink></td>
            </tr>

            <tr id="trRPT9" runat="server" visible="false">
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="ui_HrefReport9" runat="server" Text="009_Failure Reason Report - by Model" CssClass="menu-1"></asp:HyperLink></td>
            </tr>

            <tr id="trRPT10" runat="server" visible="false">
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="ui_HrefReport10" runat="server" Text="010_Shipping Report" CssClass="menu-1"></asp:HyperLink></td>
            </tr>

            <tr id="trRPT11" runat="server" visible="true">
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="ui_HrefReport11" runat="server" Text="011_Order Delivery Schedule Query" CssClass="menu-1"></asp:HyperLink></td>
            </tr>
            
            <tr id="trRPT12" runat="server" visible="true">
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="ui_HrefReport12" runat="server" Text="012_Parts Warranty Query" CssClass="menu-1"></asp:HyperLink></td>
            </tr>
            
        </table>               
        
        <table runat="server" border="0" width="90%" id="UI_tbCost" cellspacing="0" cellpadding="0" align="center" visible="false">
            <tr>
                <td align="left" colspan="2" class=default><font color="#FFFFFF">&nbsp;<asp:Label ID="Label2" runat ="server" Font-Bold="true" Text ="RMA Cost"></asp:Label></font></td>
            </tr>
            <tr>
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="UI_linkCost" runat="server" Text="RMA Cost" CssClass="menu-1" NavigateUrl="~/RMA_CostData.aspx"></asp:HyperLink></td>
            </tr>
             <tr>
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="UI_LinkRMA_Return_Status" runat="server" Text="Return RMA Status" CssClass="menu-1" NavigateUrl="~/RMA_Return_Status.aspx"></asp:HyperLink></td>
            </tr>
             <tr>
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="UI_LinkRMA_Repair_Company" runat="server" Text="RMA Repair Company" CssClass="menu-1" NavigateUrl="~/RMA_Repair_Company.aspx"></asp:HyperLink></td>
            </tr>
             <tr>
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="HyperRMA_Data" runat="server" Text="Search RMA Data" CssClass="menu-1" NavigateUrl="~/RMA_Data_Search.aspx"></asp:HyperLink></td>
            </tr>
        </table>

        <table runat="server" border="0" width="90%" id="UI_tbAssistant" cellspacing="0" cellpadding="0" align="center" visible="false">
            <tr>
                <td align="left" colspan="2" class=default><font color="#FFFFFF">&nbsp;<asp:Label ID="Label3" runat ="server" Font-Bold="true" Text ="Assistant"></asp:Label></font></td>
            </tr>
             <tr>
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="UI_linkWarrantyData" runat="server" Text="Search WarrantyData" CssClass="menu-1" NavigateUrl="~/WarrantyData.aspx"></asp:HyperLink></td>
            </tr>
            <tr>
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="UI_linkSALES_RELATE" runat="server" Text="Search SALES RELATE" CssClass="menu-1" NavigateUrl="~/SALES_RELATE.aspx"></asp:HyperLink></td>
            </tr>
             <tr>
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="UI_linkCustomer" runat="server" Text="Search Customer" CssClass="menu-1" NavigateUrl="~/RMA_Customer_Search.aspx"></asp:HyperLink></td>
            </tr>
             <tr>
                <td width="10%" align="right"><img border="0" src="Images/icon-arrow.gif" width="5" height="9">&nbsp;</td>
                <td><asp:HyperLink ID="UI_linkSearchDuplicateSN" runat="server" Text="Search Duplicate SN" CssClass="menu-1" NavigateUrl="~/SearchDuplicateSN.aspx"></asp:HyperLink></td>
            </tr>
			
		
			
        </table>

        </b>
</td>
