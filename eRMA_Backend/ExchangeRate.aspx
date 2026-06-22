<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="ExchangeRate.aspx.vb" Inherits="ExchangeRate" Title="Untitled Page" %>

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
                                    <asp:Label ID="UI_lblTittle" runat="server" Text="001_Setting - Exchange Rate" CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
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
                                <td width="98%" align="left" class="default">
                                    <asp:Label ID="UI_lblExchangeTittle" runat="server" Text="002_Currency &amp; Exchange Rate Information" Font-Bold="true"></asp:Label>
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
                                        <asp:GridView ID="UI_ExchangeRate" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric" AllowSorting="true">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="UI_SeqID" runat="server"></asp:Label>
                                                        <asp:Label ID="UI_Visible" runat="server" Text='<%# Eval("CURRENCY_VISIBLE") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="003_USD">
                                                    <HeaderStyle Width="10%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="UI_Uint" runat="server" Text='1'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="004_Currency" SortExpression="CURRENCY_CODE">
                                                    <HeaderStyle Width="20%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="UI_CurrencyCode" runat="server" Text='<%# Eval("CURRENCY_CODE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="005_Rate" SortExpression="CURRENCY_RATE">
                                                    <HeaderStyle Width="20%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="UI_CurrencyRate" runat="server" Text='<%# Eval("CURRENCY_RATE") %>' Width="50"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfv_CurrencyRate" runat="server" ErrorMessage="011_請輸入兌美金匯率" Display="None" SetFocusOnError="true" ValidationGroup="ExchangeGroup"></asp:RequiredFieldValidator>
                                                        <asp:CompareValidator ID="cv_CurrencyRate" runat="server" ErrorMessage="011_請輸入兌美金匯率" Display="None" ControlToValidate="UI_CurrencyRate" Type="Double" SetFocusOnError="true" ValidationGroup="ExchangeGroup"></asp:CompareValidator>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="CURRENCY_LUSTMP" SortExpression="CURRENCY_LUSTMP" HeaderText="006_Edit Date" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Center" HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd}"></asp:BoundField>
                                                <asp:BoundField DataField="CURRENCY_LUAD" SortExpression="CURRENCY_LUAD" HeaderText="007_Last Editor" HeaderStyle-Width="17%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>

                                                <asp:TemplateField HeaderText="008_Status" SortExpression="CURRENCY_VISIBLE">
                                                    <HeaderStyle Width="10%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="UI_CurrencyVisible" runat="server">
                                                            <asp:ListItem Value="1" Text="009_Open"></asp:ListItem>
                                                            <asp:ListItem Value="0" Text="010_Close"></asp:ListItem>
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
                                    <asp:Button ID="UI_cmdSave" runat="server" Text="_Save" CssClass="Confirm_l" ValidationGroup="ExchangeGroup" /><br>
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

    <asp:ValidationSummary ID="vsExchange" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="ExchangeGroup" />

</asp:Content>

