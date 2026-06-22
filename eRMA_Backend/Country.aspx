<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Country.aspx.vb" Inherits="Country" Title="Untitled Page" %>

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
                                    <asp:Label ID="UI_lblTittle" runat="server" Text="001_Setting - Country" CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
                                </td>
                                <td width="3%">&nbsp;</td>
                            </tr>
                            <!--[End]Tittle-->
                        </table>
                    </td>
                </tr>
                <%--		<tr height ="30%">
		    <td background="Images/pic_12.gif">&nbsp;</td>
		    <td>&nbsp;</td>
		</tr>--%>
                <tr height="30px">
                    <td width="24" background="Images/pic_14.gif">&nbsp;</td>
                    <td background="Images/pic_15.gif">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="1%">&nbsp;</td>
                                <td width="98%" align="left" class="default">
                                    <asp:Label ID="UI_lblCountryTittle" runat="server" Text="002_Country Information" Font-Bold="true"></asp:Label>
                                </td>
                                <td width="1%">&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td background="Images/pic_20.gif">&nbsp;</td>
                    <td valign="top" bgcolor="#E3D8BE" align="center">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="1%">&nbsp;</td>
                                <td width="98%" align="left" class="default">
                                    <!--[Begin]資料列表表單-->
                                    <div align="center">
                                        <asp:GridView ID="UI_Country" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric" AllowSorting="true">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="UI_SeqID" runat="server"></asp:Label>
                                                        <asp:Label ID="UI_CountryID" runat="server" Text='<%# Eval("COUNTRY_ID") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_Visible" runat="server" Text='<%# Eval("COUNTRY_VISIBLE") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="003_Country" SortExpression="COUNTRY_NAME">
                                                    <HeaderStyle Width="30%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="UI_CountryName" runat="server" Text='<%# Eval("COUNTRY_NAME") %>' Width="150"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfv_CountryName" runat="server" ErrorMessage="009_請輸入國家名稱" Display="None" SetFocusOnError="true" ValidationGroup="CountryGroup"></asp:RequiredFieldValidator>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="COUNTRY_LUSTMP" SortExpression="COUNTRY_LUSTMP" HeaderText="004_Edit Date" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Center" HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd}"></asp:BoundField>
                                                <asp:BoundField DataField="COUNTRY_LUAD" SortExpression="COUNTRY_LUAD" HeaderText="005_Last Editor" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>

                                                <asp:TemplateField HeaderText="006_Status" SortExpression="COUNTRY_VISIBLE">
                                                    <HeaderStyle Width="20%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="UI_CountryVisible" runat="server">
                                                            <asp:ListItem Value="1" Text="007_Open"></asp:ListItem>
                                                            <asp:ListItem Value="0" Text="008_Close"></asp:ListItem>
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
                                <td width="1%">&nbsp;</td>
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
                                    <asp:Button ID="UI_cmdSave" runat="server" Text="_Save" CssClass="Confirm_l" ValidationGroup="CountryGroup" /><br>
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

    <asp:ValidationSummary ID="vsCountry" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="CountryGroup" />

</asp:Content>

