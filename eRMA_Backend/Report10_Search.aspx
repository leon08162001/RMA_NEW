<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Report10_Search.aspx.vb" Inherits="Report10_Search" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <script type="text/javascript" language="javascript">
        function JS_CheckAll(objName) {
            var oGridView = document.getElementById('<%=UI_dvReport.ClientID %>');
            var oGridViewID = oGridView.id;
            var iRows = oGridView.rows.length;

            for (var i = 1; i < iRows; i++) {
                var oRMAcheck = oGridView.rows[i].cells[1].childNodes[0];
                if (oRMAcheck != null && oRMAcheck.type == "checkbox") {
                    oRMAcheck.checked = objName.checked;
                }
            }
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
                            <asp:Label ID="UI_lblTittle" runat="server" Text="010_RMA Detail Report" CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
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
                                        <td width="15%">
                                            <asp:Label ID="UI_lblRMANo" runat="server" Text="029_RMA No."></asp:Label>
                                        </td>
                                        <td width="35%">:
			                            <asp:TextBox ID="UI_txtRMANo" runat="server" Width="120"></asp:TextBox>
                                        </td>

                                        <td width="12%" align="right">
                                            <asp:Label ID="UI_lblCompanyName" runat="server" Text="100_Company Name"></asp:Label>
                                        </td>
                                        <td width="30%" align="left">
                                            <asp:Label runat="server" ID="UI_lblCompanyName_Tag">:</asp:Label>
                                            <asp:TextBox ID="UI_txtCompanyName" runat="server" Width="120"></asp:TextBox>&nbsp;
                                        </td>

                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="UI_lblModelNo" runat="server" Text="012_Model No."></asp:Label>
                                        </td>
                                        <td>:
			                            <asp:TextBox ID="UI_txtModelNo" runat="server" Width="120"></asp:TextBox>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="UI_lblRepairCenter" runat="server" Text="102_Repair Center"></asp:Label>
                                        </td>
                                        <td align="left">:
			                            <asp:DropDownList ID="UI_cboRepairCenter" runat="server"></asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <asp:Label ID="UI_lblShippingDate" runat="server" Text="033_Shipping Date"></asp:Label>
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

                <table border="0" width="100%" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="1%">&nbsp;</td>
                        <td width="49%" align="left" class="default">
                            <asp:Label ID="UI_lblReportTittle" runat="server" Text="030_Report Information" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                </table>
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

                            <asp:GridView ID="UI_dvReport" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric" AllowSorting="true">
                                <Columns>

                                    <asp:TemplateField>
                                        <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="UI_SeqID" runat="server"></asp:Label>
                                            <asp:Label ID="UI_RMAID" runat="server" Text='<%# Eval("RMA_ID") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="UI_lblRMA" runat="server" Text='<%# Eval("RMA_No") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>

                                        <HeaderTemplate>
                                            <asp:CheckBox ID="UI_CheckAll" runat="server" onclick="JS_CheckAll(this);" />
                                        </HeaderTemplate>

                                        <ItemTemplate>
                                            <asp:CheckBox ID="UI_Check" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="106_RMA No" SortExpression="RMA_No">
                                        <HeaderStyle Width="15%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="UI_RMADNO" runat="server" Text='<%# Eval("RMA_No") %>' CommandName="cmdDetail" CommandArgument='<%# me.UI_dvReport.Rows.Count%>' PostBackUrl="RMARepair_UpLoad.aspx"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="RequestedData" SortExpression="RequestedData" HeaderText="107_Request Date" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center" HtmlEncode="False" DataFormatString="{0:yyyy/MM/dd}"></asp:BoundField>
                                    <asp:BoundField DataField="ShippedDate" HeaderText="129_Shipping Date" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center" HtmlEncode="False" DataFormatString="{0:yyyy/MM/dd}"></asp:BoundField>
                                    <asp:BoundField DataField="CU_NAME" SortExpression="CU_NAME" HeaderText="186_Client" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                    <asp:BoundField DataField="COMP_NAME" SortExpression="COMP_NAME" HeaderText="187_Repairing Center" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                    <asp:BoundField DataField="TotalCount" SortExpression="ReceivedCount" HeaderText="188_Shipping / Received Number" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                    <asp:BoundField DataField="BillDate" SortExpression="bill_date" HeaderText="189_Bill Date" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>

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

                    <tr>
                        <td colspan="2" align="right">
                            <p style="margin-top: 10px; padding-right: 10px">
                                <asp:Button ID="UI_cmdExport" runat="server" Text="044_Confirm and Print" CssClass="Confirm_l" />
                                &nbsp;&nbsp;&nbsp;&nbsp;
	                         <asp:Button ID="UI_cmdDel" runat="server" Text="044_Delete" CssClass="Confirm" />
                            </p>
                        </td>
                    </tr>

                </table>
            </td>
        </tr>

        <asp:Label ID="UI_lblPreviousPage_RMAID" runat="server" Visible="false"></asp:Label>
        <asp:Label ID="UI_lblPreviousPage_RMANO" runat="server" Visible="false"></asp:Label>
    </table>

    <iframe runat="server" id="ui_frame" src="#" width="1px" style="visibility: hidden"></iframe>

    <asp:Label runat="server" ID="ui_jascript"></asp:Label>
</asp:Content>

