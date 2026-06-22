<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="MaintainAccount_Search.aspx.vb" Inherits="MaintainAccount_Search" Title="Untitled Page" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">

    <table border="0" cellspacing="0" cellpadding="0" width="100%" height="100%">
        <tr height="20%">
            <td width="24" background="Images/pic_12.gif">&nbsp;</td>
            <td valign="top" align="left">
                <table border="0" width="100%" cellspacing="0" cellpadding="0">
                    <!--[Begin]Tittle-->
                    <tr>
                        <td width="3%">&nbsp;</td>
                        <td width="94%" align="left">
                            <asp:Label ID="UI_lblTittle" runat="server" Text="001_Setting - Maintain Account" CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
                        </td>
                        <td width="3%">&nbsp;</td>
                    </tr>
                    <!--[End]Tittle-->
                    <!--[Begin]資料查詢條件區-->
                    <tr>
                        <td>&nbsp;</td>
                        <td align="left">
                            <table width="95%" border="0" cellspacing="1" cellpadding="0" class="default">
                                <tr>
                                    <td colspan="4">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td width="15%">
                                        <asp:Label ID="UI_lblRepairCenter" runat="server" Text="002_Repair Center"></asp:Label>
                                    </td>
                                    <td width="85%">:
			                            <asp:DropDownList ID="UI_cboRepairCenter" runat="server"></asp:DropDownList>
                                        <asp:Button ID="UI_cmdSearch" runat="server" Text="_Search" CssClass="Search" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">&nbsp;</td>
                                </tr>
                            </table>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <!--[End]資料查詢條件區-->
                </table>
            </td>
        </tr>
        <%--	    <tr height ="30%">
	        <td background="Images/pic_12.gif">&nbsp;</td>
	        <td>&nbsp;</td>
	    </tr>--%>

        <tr height="5%">
            <td width="24" background="Images/pic_14.gif">&nbsp;</td>
            <td background="Images/pic_15.gif">
                <table border="0" width="100%" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="1%">&nbsp;</td>
                        <td width="20%" align="left" class="default">
                            <asp:Label ID="UI_lblUserTittle" runat="server" Text="003_User Information" Font-Bold="true"></asp:Label>
                        </td>
                        <td width="79%" align="left">
                            <asp:Button ID="UI_cmdAdd" runat="server" Text="_Add User" CssClass="Confirm_l" PostBackUrl="~/MaintainAccount.aspx" Width="120" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>

        <tr height="55%" valign="top">
            <td background="Images/pic_20.gif">&nbsp;</td>
            <td valign="top" bgcolor="#E3D8BE" align="center">
                <table border="0" width="100%" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="1%">&nbsp;</td>
                        <td width="99%" align="left" class="default">
                            <!--[Begin]資料列表表單-->
                            <div align="center">
                                <asp:GridView ID="UI_MaintainAccount" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric" AllowSorting="true">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="UI_SeqID" runat="server"></asp:Label>
                                                <asp:Label ID="UI_adID" runat="server" Text='<%# Eval("AD_ID") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="AD_ID" SortExpression="AD_ID" HeaderText="005_ID" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="AD_NAME" SortExpression="AD_NAME" HeaderText="006_Name" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="Level" SortExpression="Level" HeaderText="007_Level" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="Roles" SortExpression="Roles" HeaderText="008_Roles" HeaderStyle-Width="30%" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                        <asp:BoundField DataField="RepairCenter" SortExpression="RepairCenter" HeaderText="009_Repair Center" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                        <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="010_Status" HeaderStyle-Width="9%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>

                                        <asp:TemplateField>
                                            <HeaderStyle Width="8%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Button ID="UI_cmdEdit" runat="server" Text="_Edit" CssClass="Problem_Edit" CommandName="cmdEdit" CommandArgument='<%# me.UI_MaintainAccount.Rows.Count%>' PostBackUrl="MaintainAccount.aspx" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>

                                    <HeaderStyle CssClass="Text_Head" />
                                    <RowStyle CssClass="TR_1" />
                                    <AlternatingRowStyle CssClass="ListRowEven" />
                                    <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Center" />
                                </asp:GridView>
                            </div>
                            <!--[End]資料列表表單-->
                        </td>
                    </tr>
                </table>
            </td>
        </tr>

        <tr height="20%" valign="top">
            <td width="24" background="Images/pic_20.gif">&nbsp;</td>
            <td bgcolor="#E3D8BE">&nbsp;
                <table border="0" width="100%" cellspacing="0" cellpadding="0">
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center">
                            <font size="2" color="#800080">
                                <asp:Label ID="UI_lblTittle0" runat="server" Text="011_說明:  個人登入預設僅顯示上表詳細內容及修改密碼功能," Visible="false"></asp:Label>
                            </font>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <font size="2" color="#800080">
                                <asp:Label ID="UI_lblTittle1" runat="server" Text="012_列表僅有具Admin 角色者可以檢視及修改全部資料;  列表依更新時間倒排. 最後修改的放最前面. 可以按標題字重新排序" Visible="false"></asp:Label>
                            </font>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
    <asp:Label runat="server" ID="UI_lblPreviousPage_adID" Visible="false"></asp:Label>

</asp:Content>

