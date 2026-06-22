<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="FAQ_Search.aspx.vb" Inherits="FAQ_Search" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">

    <table border="0" cellspacing="0" cellpadding="0" width="100%" height="100%">
        <tr>
            <td width="24" background="Images/pic_12.gif">&nbsp;</td>
            <td valign="top" align="left" height="100">

                <table border="0" width="100%" cellspacing="0" cellpadding="0">
                    <!--[Begin]Tittle-->
                    <tr>
                        <td width="3%">&nbsp;</td>
                        <td width="94%" align="left">
                            <asp:Label ID="UI_lblTittle" runat="server" Text="001_Setting - FAQ" CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
                        </td>
                        <td width="3%">&nbsp;</td>
                    </tr>

                    <!--[End]Tittle-->
                    <!--[Begin]資料查詢條件區-->

                    <tr>
                        <td>&nbsp;</td>
                        <td align="left" colspan="2">
                            <table width="95%" border="0" cellspacing="1" cellpadding="0" class="default">
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="UI_lblFAQSearching" runat="server" Text="002_FAQ Searching" Font-Bold="true"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="15%">
                                        <asp:Label ID="UI_lblCategory" runat="server" Text="003_Category"></asp:Label>
                                    </td>
                                    <td width="85%">:
				                        <asp:DropDownList ID="UI_cboCategory1" runat="server" AutoPostBack="true"></asp:DropDownList>
                                        <asp:DropDownList ID="UI_cboCategory2" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="UI_lblQuestion" runat="server" Text="004_Question"></asp:Label>
                                    </td>
                                    <td colspan="3">:
				                        <asp:TextBox ID="UI_txtQuestion" runat="server" Width="120"></asp:TextBox>&nbsp;
				                        <asp:Button ID="UI_cmdSearch" runat="server" Text="_Search" CssClass="Search" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <!--[End]資料查詢條件區-->
                </table>
            </td>
        </tr>

        <tr height="10">
            <td background="Images/pic_12.gif">&nbsp;</td>
            <td>&nbsp;</td>
        </tr>

        <tr>
            <td width="24" background="Images/pic_14.gif" height="25px">&nbsp;</td>
            <td background="Images/pic_15.gif">
                <table border="0" width="100%" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="1%">&nbsp;</td>
                        <td width="20%" align="left">
                            <asp:Label ID="UI_lblFAQTittle" runat="server" Text="006_FAQ Information" Font-Size="Smaller" Font-Bold="true"></asp:Label>
                        </td>
                        <td width="72%" align="left" class="default">
                            <asp:Button ID="UI_cmdAdd" runat="server" Text="_Add" CssClass="Confirm" PostBackUrl="FAQ.aspx" />
                        </td>
                        <td width="3%">&nbsp;</td>
                    </tr>
                </table>
            </td>
        </tr>

        <tr>
            <td background="Images/pic_20.gif">&nbsp;</td>
            <td valign="top" bgcolor="#E3D8BE" align="center">
                <table border="0" width="100%" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="99%" align="left" class="default">
                            <!--[Begin]資料列表表單-->
                            <div align="center">

                                <asp:GridView ID="UI_dvFAQ" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric" AllowSorting="true">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="UI_SeqID" runat="server"></asp:Label>
                                                <asp:Label ID="UI_FAQID" runat="server" Text='<%# Eval("FAQ_ID") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="UI_FAQCID" runat="server" Text='<%# Eval("FAQ_FAQCID") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="UI_FAQSCID" runat="server" Text='<%# Eval("FAQ_FAQSCID") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="QUESTION" SortExpression="QUESTION" HeaderText="004_Question" HeaderStyle-Width="37%" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                        <asp:BoundField DataField="ANSWER" SortExpression="ANSWER" HeaderText="007_Answer" HeaderStyle-Width="37%" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                        <asp:BoundField DataField="FAQ_ISSUEDATE" SortExpression="FAQ_ISSUEDATE" HeaderText="008_Date" HeaderStyle-Width="15%" HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd}"></asp:BoundField>

                                        <asp:TemplateField HeaderText="009_Edit">
                                            <HeaderStyle Width="8%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Button ID="imgEdit" runat="server" Text="_Edit" CssClass="Problem_Edit" CommandName="cmdEdit" CommandArgument='<%#Me.UI_dvFAQ.Rows.Count%>' PostBackUrl="~/FAQ.aspx" />
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
    </table>

    <asp:Label runat="server" ID="UI_lblPreviousPage_FAQID" Visible="false"></asp:Label>

</asp:Content>
