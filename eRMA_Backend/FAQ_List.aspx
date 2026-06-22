<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="FAQ_List.aspx.vb" Inherits="FAQ_List" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>

            <table border="0" width="100%" id="table4" cellspacing="0" cellpadding="0" height="410px">
                <tr>
                    <td width="24" background="Images/pic_12.gif">&nbsp;</td>
                    <td valign="top" align="left">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0" height="100%">
                            <!--[Begin]Tittle-->
                            <tr>
                                <td width="3%">&nbsp;</td>
                                <td width="94%" align="left">&nbsp;
		                    <asp:Label ID="UI_lblTittle" runat="server" Text="032_Return Merchanise Authorization (RMA) FAQ" CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
                                </td>
                                <td width="3%">&nbsp;</td>
                            </tr>
                            <!--[End]Tittle-->
                            <!--[Begin]新增資料-->
                            <tr>
                                <td>&nbsp;</td>
                                <td align="left">
                                    <table width="100%" align="center" border="0" cellspacing="1" cellpadding="0" class="default">
                                        <tr>
                                            <td align="left">
                                                <asp:DropDownList ID="UI_cboCategory1" runat="server" AutoPostBack="true"></asp:DropDownList>
                                                <asp:DropDownList ID="UI_cboCategory2" runat="server"></asp:DropDownList>
                                                <asp:TextBox ID="UI_txtQuestion" runat="server" Width="200"></asp:TextBox>
                                                <asp:Button ID="UI_cmdSearch" runat="server" Text="_Search" CssClass="Search" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <!--[End]新增資料-->
                        </table>
                    </td>
                </tr>

                <tr height="27px">
                    <td width="24" height="27px" background="Images/pic_14.gif">&nbsp;</td>
                    <td height="27px" background="Images/pic_15.gif">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="3%">&nbsp;</td>
                                <td width="94%">&nbsp;</td>
                                <td width="3%">&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>

                <tr height="100%">
                    <td background="Images/pic_20.gif">&nbsp;</td>
                    <td valign="top" bgcolor="#E3D8BE" align="left">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="3%">&nbsp;</td>
                                <td width="97%" align="left">
                                    <!--[Begin]資料列表表單-->
                                    <asp:GridView ID="UI_dvFAQ" runat="server" Width="98%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric" ShowHeader="false">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemStyle HorizontalAlign="Left" CssClass="text9pt" Height="25px"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="UI_QUESTION" runat="server" Text='<%# Eval("FAQ_QUESTION") %>' Font-Bold="true"></asp:Label><br />
                                                    <asp:Label ID="UI_ANSWER" runat="server" Text='<%# Eval("FAQ_ANSWER") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>

                                        <%--<RowStyle CssClass="TR_1" />--%>
                                        <AlternatingRowStyle CssClass="ListRowEven" />
                                        <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Center" />
                                    </asp:GridView>
                                    <!--[End]資料列表表單-->
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="UI_cboCategory1" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>
