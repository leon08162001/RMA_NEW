<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Warranty_GroupSetting.aspx.vb" Inherits="Warranty_GroupSetting" %>

<%@ Register Src="ascx/ucProgressStatus.ascx" TagName="ucProgressStatus" TagPrefix="uc1" %>
<%@ Register Src="ascx/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>
<%@ Register Src="ascx/ucProductGroup.ascx" TagName="ucProductGroup" TagPrefix="uc6" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <uc1:ucProgressStatus ID="ucProgressStatus" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                <tbody>
                    <tr height="50">
                        <td width="24" background="Images/pic_12.gif">&nbsp;
                        </td>
                        <td valign="top" align="left">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tbody>
                                    <tr>
                                        <td width="3%">&nbsp;
                                        </td>
                                        <td align="left" width="94%">
                                            <asp:Label ID="UI_lblTittle" runat="server" Text="021_Warranty Group Setting"
                                                ForeColor="#326B9B" CssClass="text_tittle"></asp:Label>
                                        </td>
                                        <td width="3%">&nbsp;
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr height="30">
                        <td width="24" background="Images/pic_14.gif">&nbsp;
                        </td>
                        <td background="Images/pic_15.gif">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tbody>
                                    <tr>
                                        <td width="1%">&nbsp;
                                        </td>
                                        <td class="default" align="left" width="49%">
                                            <asp:Button ID="UI_cmdAdd" runat="server" Text="_Add" CssClass="Confirm_l" PostBackUrl="~/Warranty_GroupSetting_add.aspx"
                                                Width="100" />
                                        </td>
                                        <td class="default" align="right" width="50%">&nbsp;
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr valign="top" height="250">
                        <td background="Images/pic_20.gif">&nbsp;
                        </td>
                        <td valign="top" align="center" bgcolor="#e3d8be">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tbody>
                                    <tr>
                                        <td width="1%">&nbsp;
                                        </td>
                                        <td class="default" align="left" width="99%">
                                            <div align="left">
                                                <asp:GridView ID="UI_dvSales" runat="server" CssClass="default" Width="100%" PagerSettings-Mode="Numeric" AllowPaging="true" AutoGenerateColumns="False" border="0"
                                                    CellSpacing="1" CellPadding="0" AllowSorting="true">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="UI_SEQID" runat="server"></asp:Label>
                                                                <asp:Label ID="lblGroupNo" runat="server" Text='<%# Eval("GROUP_NO") %>' Visible="false"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Group No" SortExpression="GROUP_NO">
                                                            <HeaderStyle Width="15%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="UI_GROUP_NO" runat="server" Text='<%# Eval("GROUP_NO")%>' CommandName="cmdDetail" CommandArgument='<%# me.UI_dvSales.Rows.Count%>' PostBackUrl="Warranty_GroupSetting_add.aspx"></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="GROUP_NAME" SortExpression="GROUP_NAME" HeaderText="Group Name"
                                                            HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="GROUP_AD" SortExpression="GROUP_AD" HeaderText="Create User"
                                                            HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center" HtmlEncode="False"></asp:BoundField>
                                                        <asp:BoundField DataField="GROUP_ADNAME" SortExpression="GROUP_ADNAME" HeaderText="Create Name"
                                                            HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="GROUP_LUAD" SortExpression="GROUP_LUAD" HeaderText="Update User"
                                                            HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center" HtmlEncode="False"></asp:BoundField>
                                                        <asp:BoundField DataField="GROUP_LUADNAME" SortExpression="GROUP_LUADNAME" HeaderText="Update Name"
                                                            HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:TemplateField HeaderText="">
                                                            <HeaderStyle Width="9%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:Button ID="UI_cmdEdit" runat="server" Text="_Edit" CssClass="Pick" Width="50"
                                                                    CommandName="cmdEdit" CommandArgument='<%#me.UI_dvSales.Rows.Count%>' PostBackUrl="Warranty_GroupSetting_add.aspx" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <asp:Label runat="server" ID="UI_NotFound" Text='<%# Session("_isShowNotFound").tostring()%>'></asp:Label>
                                                    </EmptyDataTemplate>
                                                    <EmptyDataRowStyle HorizontalAlign="Center" />
                                                    <HeaderStyle CssClass="Text_Head" />
                                                    <RowStyle CssClass="TR_1" />
                                                    <AlternatingRowStyle CssClass="ListRowEven" />
                                                    <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Center" />
                                                </asp:GridView>
                                            </div>
                                            <!--[End]資料列表表單-->
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
            <uc2:ucMessage ID="ucMessage" runat="server"></uc2:ucMessage>
            <uc6:ucProductGroup ID="ucProductGroup" runat="server"></uc6:ucProductGroup>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ucMessage" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="vsSaleSave" runat="server" ShowMessageBox="true" ShowSummary="false"
        ValidationGroup="SaleSaveGroup" />
    <asp:Label runat="server" ID="lblPreviousPage_GroupNo" Visible="false"></asp:Label>
</asp:Content>
