<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Receive_WorkList.aspx.vb" Inherits="Receive_WorkList" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <script type="text/javascript" language="javascript">
        function FrmDelete() {
            if (confirm(delMsg)) {
                onProgress('Delete')
                return true;
            }
            return false;
        }
    </script>

    <table border="0" cellspacing="0" cellpadding="0" width="100%" height="410px">
        <tr height="10%">
            <td width="24" background="Images/pic_12.gif">&nbsp;</td>
            <td valign="top" align="left">
                <table border="0" width="100%" cellspacing="0" cellpadding="0">
                    <!--[Begin]Tittle-->
                    <tr>
                        <td width="3%">&nbsp;</td>
                        <td width="94%" align="left">
                            <asp:Label ID="UI_lblTittle" runat="server" Text="067_Wait for Processing" CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
                        </td>
                        <td width="3%">&nbsp;</td>
                    </tr>
                    <!--[End]Tittle-->
                    <!--[Begin]資料查詢條件區-->

                    <tr>
                        <td>&nbsp;</td>
                        <td align="left">

                            <asp:Panel runat="server" ID="UI_panSearch" DefaultButton="UI_cmdSearch">
                                <table width="95%" border="0" cellspacing="1" cellpadding="0" class="default">
                                    <tr>
                                        <td colspan="4">
                                            <asp:Label ID="UI_lblRequestInformation" runat="server" Text="028_Search for Request Items Information" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td width="15%">
                                            <asp:Label ID="UI_lblRMANo" runat="server" Text="029_RMA No."></asp:Label>
                                        </td>
                                        <td width="35%">:
			                            <asp:TextBox ID="UI_txtRMANo" runat="server" Width="150"></asp:TextBox>
                                        </td>

                                        <td width="12%" align="right">
                                            <asp:Label ID="UI_lblCustomer" runat="server" Text="030_Customer"></asp:Label>
                                        </td>
                                        <td width="30%" align="left">:
			                            <asp:TextBox ID="UI_txtCustomer" runat="server" Width="120"></asp:TextBox>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="UI_lblModelNo" runat="server" Text="012_Model No."></asp:Label>
                                        </td>
                                        <td>:
			                            <asp:TextBox ID="UI_txtModelNo" runat="server" Width="150"></asp:TextBox>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="UI_lblSerialNumber" runat="server" Text="013_Serial Number"></asp:Label>
                                        </td>
                                        <td align="left">:
			                            <asp:TextBox ID="UI_txtSerialNumber" runat="server" Width="120"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <!--
			                    <tr>
			                        <td width="15%">&nbsp;</td>
			                        <td width="35%">&nbsp;</td>
			                        <td width="12%" align="right">
			                            <asp:Label ID="UI_lblStatus" runat="server" Text="032_Status"></asp:Label>
			                        </td>
			                        <td width="30%" align="left">:
			                            <asp:DropDownList ID="UI_cboStatus" runat="server"></asp:DropDownList>
					                </td>
			                    </tr>
-->
                                    <tr>
                                        <td>*
			                            <asp:Label ID="UI_lblRequestDate" runat="server" Text="033_Request Date"></asp:Label>
                                        </td>
                                        <td align="left" colspan="3">:
			                            <asp:DropDownList ID="UI_cboBYear" runat="server"></asp:DropDownList>
                                            <asp:DropDownList ID="UI_cboBMonth" runat="server"></asp:DropDownList>
                                            <asp:DropDownList ID="UI_cboBDay" runat="server"></asp:DropDownList>&nbsp;~&nbsp;
                                        <asp:DropDownList ID="UI_cboEYear" runat="server"></asp:DropDownList>
                                            <asp:DropDownList ID="UI_cboEMonth" runat="server"></asp:DropDownList>
                                            <asp:DropDownList ID="UI_cboEDay" runat="server"></asp:DropDownList>
                                            <asp:Button ID="UI_cmdSearch" runat="server" Text="_Search" CssClass="Search" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>

                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <!--[End]資料查詢條件區-->
                </table>
            </td>
        </tr>

        <tr height="20px">
            <td background="Images/pic_12.gif">&nbsp;</td>
            <td>&nbsp;</td>
        </tr>

        <tr height="10px">
            <td width="24" background="Images/pic_14.gif">&nbsp;</td>
            <td background="Images/pic_15.gif">

                <asp:Panel runat="server" ID="Panel1" DefaultButton="UI_cmdQuery">
                    <table border="0" width="100%" cellspacing="0" cellpadding="0">
                        <tr>
                            <td width="1%">&nbsp;</td>
                            <td width="49%" align="left" class="default">
                                <asp:Label ID="UI_lblRequestedTittle" runat="server" Text="034_Requested Information" Font-Bold="true"></asp:Label>
                            </td>
                            <td width="50%" align="right" class="default">
                                <asp:Label ID="UI_lblQuickRMA" runat="server" Text="039_Quick Receiving:  RMA No."></asp:Label>
                                <asp:TextBox ID="UI_txtQuickRMA" runat="server" Width="120px"></asp:TextBox>
                                <asp:Button ID="UI_cmdQuery" runat="server" Text="_Query" CssClass="Confirm" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>

            </td>
        </tr>

        <tr height="250px" valign="top">
            <td background="Images/pic_20.gif">&nbsp;</td>
            <td valign="top" bgcolor="#E3D8BE" align="center">
                <table border="0" width="100%" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="1%">&nbsp;</td>
                        <td width="99%" align="left" class="default">
                            <!--[Begin]資料列表表單-->

                            <asp:GridView ID="UI_dvRequest" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric" AllowSorting="true">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="UI_SeqID" runat="server"></asp:Label>
                                            <asp:Label ID="UI_RMAID" runat="server" Text='<%# Eval("RMA_ID") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="UI_RMANo" runat="server" Text='<%# Eval("RMA_No") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="UI_CUNO" runat="server" Text='<%# Eval("CU_NO") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="029_RMA No" SortExpression="RMA_No">
                                        <HeaderStyle Width="19%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                        <ItemTemplate>
                                            <%--                                            <asp:LinkButton ID="UI_RMA" runat="server" Text='<%# Eval("RMA_No") %>' CommandName="cmdDetail" CommandArgument='<%# me.UI_dvRequest.Rows.Count%>'></asp:LinkButton>--%>
                                            <asp:Label ID="UI_lblRMA" runat="server" Text='<%# Eval("RMA_No") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="RMA_CSTMP" SortExpression="RMA_CSTMP" HeaderText="033_Request Date" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HtmlEncode="False" DataFormatString="{0:yyyy/MM/dd}"></asp:BoundField>
                                    <asp:BoundField DataField="CU_NAME" SortExpression="CU_NAME" HeaderText="036_Client" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                    <asp:BoundField DataField="ReceivedTotal" SortExpression="ReceivedTotal" HeaderText="040_Received / Total Number" HeaderStyle-Width="30%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>

                                    <asp:TemplateField HeaderText="032_Status" SortExpression="Status">
                                        <HeaderStyle Width="10%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="UI_Status" runat="server" Text='<%# Eval("Status") %>' CommandName="cmdEdit" CommandArgument='<%# me.UI_dvRequest.Rows.Count%>' PostBackUrl="~/Receive_Edit.aspx"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="041_Cancel">
                                        <HeaderStyle Width="8%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgDel" runat="server" ImageAlign="AbsBottom" ImageUrl="~/images/xx.gif" CommandName="cmdDel" OnClientClick="return FrmDelete()" CommandArgument='<%# me.UI_dvRequest.Rows.Count%>' />
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

                            <!--[End]資料列表表單-->
                        </td>
                    </tr>
                </table>
            </td>
        </tr>

    </table>

    <asp:Label ID="UI_lblPreviousPage_RMAID" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="UI_lblPreviousPage_RMANO" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="UI_lblPreviousPage_CUNO" runat="server" Visible="false"></asp:Label>

</asp:Content>
