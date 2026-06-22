<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Warranty.aspx.vb" Inherits="Warranty" Title="Untitled Page" %>

<%@ Register Src="ascx/ucProgressStatus.ascx" TagName="ucProgressStatus" TagPrefix="uc1" %>
<%@ Register Src="ascx/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <uc1:ucProgressStatus ID="ucProgressStatus" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table border="0" cellspacing="0" cellpadding="0" width="100%" height="100%">
                <tr height="10%">
                    <td width="24" background="Images/pic_12.gif">&nbsp;</td>
                    <td valign="top" align="left">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <!--[Begin]Tittle-->
                            <tr>
                                <td width="3%">&nbsp;</td>
                                <td width="94%" align="left">
                                    <asp:Label ID="UI_lblTittle" runat="server" Text="001_Setting - Warranty" CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
                                </td>
                                <td width="3%">&nbsp;</td>
                            </tr>
                            <!--[End]Tittle-->
                        </table>
                    </td>
                </tr>
                <%--		    <tr height ="30%">
		        <td background="Images/pic_12.gif">&nbsp;</td>
		        <td>&nbsp;</td>
		    </tr>--%>
                <tr height="30px">
                    <td width="24" background="Images/pic_14.gif">&nbsp;</td>
                    <td background="Images/pic_15.gif">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="98%" align="left" class="default">
                                    <asp:Label ID="UI_lblWarrantyTittle" runat="server" Text="002_Warranty Information" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td background="Images/pic_20.gif">&nbsp;</td>
                    <td valign="top" bgcolor="#E3D8BE" align="center">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="100%" align="left" class="default">
                                    <!--[Begin]資料列表表單-->
                                    <div align="center">
                                        <asp:GridView ID="UI_Warranty" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="UI_SeqID" runat="server" Text='<%# me.UI_Warranty.Rows.Count +1 %>'></asp:Label>
                                                        <asp:Label ID="UI_WarrID" runat="server" Text='<%# Eval("WARR_ID") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_Unit" runat="server" Text='<%# Eval("WARR_UNIT") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_Visible" runat="server" Text='<%# Eval("WARR_VISIBLE") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="003_Value">
                                                    <HeaderStyle Width="20%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="UI_WarrNumber" runat="server" Text='<%# Eval("WARR_NUMBER") %>' Width="50"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfv_WarrNumber" runat="server" ErrorMessage="013_請輸入數值" Display="None" SetFocusOnError="true" ValidationGroup="WarrantyGroup"></asp:RequiredFieldValidator>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="004_Unit">
                                                    <HeaderStyle Width="20%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="UI_WarrUnit" runat="server">
                                                            <asp:ListItem Value="1" Text="008_Year"></asp:ListItem>
                                                            <asp:ListItem Value="2" Text="009_Month"></asp:ListItem>
                                                            <asp:ListItem Value="3" Text="010_Day"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="WARR_LUSTMP" HeaderText="005_Edit Date" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Center" HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd}"></asp:BoundField>
                                                <asp:BoundField DataField="WARR_LUAD" HeaderText="006_Last Editor" HeaderStyle-Width="17%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>

                                                <asp:TemplateField HeaderText="007_Status">
                                                    <HeaderStyle Width="20%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="UI_WarrVisible" runat="server">
                                                            <asp:ListItem Value="1" Text="011_Open"></asp:ListItem>
                                                            <asp:ListItem Value="0" Text="012_Close"></asp:ListItem>
                                                        </asp:DropDownList>
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
                <tr height="10%">
                    <td background="Images/pic_20.gif" height="370">&nbsp;</td>
                    <td valign="top" bgcolor="#E3D8BE" align="center">
                        <!--[Begin]頁數-->
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="3%">&nbsp;</td>
                                <td width="94%" align="center" class="default">
                                    <br>
                                    <asp:Button ID="UI_cmdSave" runat="server" Text="_Save" CssClass="Confirm_l" ValidationGroup="WarrantyGroup" /><br>
                                </td>
                                <td width="3%">&nbsp;</td>
                            </tr>
                        </table>
                        <!--[End]頁數-->
                    </td>
                </tr>
            </table>

            <uc2:ucMessage ID="ucMessage" runat="server" />

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="UI_cmdSave" EventName="Click" />
            <asp:PostBackTrigger ControlID="ucMessage" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:ValidationSummary ID="vsWarranty" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="WarrantyGroup" />

</asp:Content>

