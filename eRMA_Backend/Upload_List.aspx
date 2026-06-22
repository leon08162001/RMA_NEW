<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Upload_List.aspx.vb" Inherits="Upload_List" %>

<%@ Register Src="ascx/ucImportErrorMsg.ascx" TagName="ucImportErrorMsg" TagPrefix="uc1" %>
<%@ Register Src="ascx/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">

    <script type="text/javascript" language="javascript">
        function FrmDelete() {
            if (confirm(delMsg)) {
                onProgress('Delete')
                return true;
            }

            return false;
        }
        function SubmitControl_Group(objName) {
            var oGridView = document.getElementById('<%=UI_dvRequest.ClientID %>');
            var oGridViewID = oGridView.id;
            var iRows = oGridView.rows.length;

            for (var i = 1; i < iRows; i++) {
                cell = oGridView.rows[i].cells[1];
                for (j = 0; j < cell.childNodes.length; j++) {
                    if (cell.childNodes[j].type == "checkbox") {
                        cell.childNodes[j].checked = objName.checked;
                    }
                }
            }
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table height="410" cellspacing="0" cellpadding="0" width="100%" border="0">
                <tbody>
                    <tr height="10%">
                        <td width="24" background="Images/pic_12.gif">&nbsp;</td>
                        <td valign="top" align="left">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <!--[Begin]Tittle-->
                                <tbody>
                                    <tr>
                                        <td width="3%">&nbsp;</td>
                                        <td align="left" width="79%">
                                            <asp:Label ID="UI_lblTittle" runat="server" Text="001_Tempoary Data" ForeColor="#326B9B" CssClass="text_tittle"></asp:Label>
                                        </td>
                                        <td width="15%"></td>
                                        <td width="3%">&nbsp;</td>
                                    </tr>
                                    <!--[End]Tittle-->
                                    <!--[Begin]資料查詢條件區-->
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td align="left">
                                            <asp:Panel ID="UI_panSearch" runat="server" DefaultButton="UI_cmdSearch">
                                                <table class="default" cellspacing="1" cellpadding="0" width="95%" border="0">
                                                    <tbody>
                                                        <tr>
                                                            <td colspan="4">
                                                                <asp:Label ID="UI_lblRequestInformation" runat="server" Text="002_Search for Import Items Information" Font-Bold="true"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="15%">
                                                                <asp:Label ID="UI_lblCust" runat="server" Text="003_Customer."></asp:Label>
                                                            </td>
                                                            <td width="35%">:
                                                                <asp:TextBox ID="UI_txtCust" runat="server" Width="120"></asp:TextBox>
                                                            </td>
                                                            <td width="15%">&nbsp;</td>
                                                            <td width="35%">&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td width="15%">
                                                                <asp:Label ID="UI_lblSerialNumber" runat="server" Text="004_Serial Number"></asp:Label>
                                                            </td>
                                                            <td width="35%">:
                                                                <asp:TextBox ID="UI_txtSerialNumber" runat="server" Width="120"></asp:TextBox>
                                                            </td>
                                                            <td width="15%">&nbsp;</td>
                                                            <td align="right" width="35%">
                                                                <asp:Button ID="UI_butImport" runat="server" Text="007_Import" CssClass="butOperation" OnClick="UI_butImport_Click"></asp:Button>&nbsp;
                                                                <asp:Button ID="UI_butReject" runat="server" Text="008_Reject" CssClass="butOperation" OnClientClick="return FrmDelete()" OnClick="UI_butReject_Click"></asp:Button>&nbsp; </td>
                                                        </tr>
                                                        <tr>
                                                            <td>*
                                                                <asp:Label ID="UI_lblRequestDate" runat="server" Text="005_Date"></asp:Label>
                                                            </td>
                                                            <td align="left" colspan="3">:
                                                                <asp:DropDownList ID="UI_cboBYear" runat="server">
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="UI_cboBMonth" runat="server">
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="UI_cboBDay" runat="server">
                                                                </asp:DropDownList>&nbsp;~&nbsp;
                                                                <asp:DropDownList ID="UI_cboEYear" runat="server">
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="UI_cboEMonth" runat="server">
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="UI_cboEDay" runat="server">
                                                                </asp:DropDownList>
                                                                <asp:Button ID="UI_cmdSearch" runat="server" Text="_Search" CssClass="Search"></asp:Button>&nbsp;&nbsp; </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <!--[End]資料查詢條件區-->
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr height="20">
                        <td background="Images/pic_12.gif">&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr height="10">
                        <td width="24" background="Images/pic_14.gif">&nbsp;</td>
                        <td background="Images/pic_15.gif">
                            <asp:Panel ID="Panel1" runat="server">
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tbody>
                                        <tr>
                                            <td width="1%">&nbsp;</td>
                                            <td class="default" align="left" width="49%">
                                                <asp:Label ID="UI_lblRequestedTittle" runat="server" Text="005_Import Information" Font-Bold="true"></asp:Label>
                                            </td>
                                            <td class="default" align="right" width="50%">&nbsp;</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr valign="top" height="250">
                        <td background="Images/pic_20.gif">&nbsp;</td>
                        <td valign="top" align="center" bgcolor="#e3d8be">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tbody>
                                    <tr>
                                        <td width="1%">&nbsp;</td>
                                        <td class="default" align="left" width="99%">
                                            <!--[Begin]資料列表表單-->

                                            <asp:GridView ID="UI_dvRequest" runat="server" CssClass="default" AllowSorting="true" PagerSettings-Mode="Numeric" AllowPaging="true" AutoGenerateColumns="False" border="0" CellSpacing="1" CellPadding="0" Width="100%">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="UI_SeqID" runat="server"></asp:Label>
                                                            <asp:Label ID="UI_RMAID" runat="server" Text='<%# Eval("IRMA_ID") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="UI_RMANo" runat="server" Text='<%# Eval("IRMA_No") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="UI_CUNO" runat="server" Text='<%# Eval("IRMA_CUNO") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="UI_ERROR" runat="server" Text='<%# Eval("IRMA_ERROR") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="UI_FILE" runat="server" Text='<%# Eval("IRMA_FILE") %>' Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="029_RMA No">
                                                        <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="UI_CheckGroup" runat="server" onclick="SubmitControl_Group(this);" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="UI_Check" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="009_Temp No" SortExpression="IRMA_No">
                                                        <HeaderStyle Width="12%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="UI_lblRMA" runat="server" Text='<%# Eval("IRMA_No") %>' CommandName="cmdEdit" CommandArgument='<%# me.UI_dvRequest.Rows.Count%>' PostBackUrl="~/Upload_Edit.aspx"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="IRMA_CSTMP" SortExpression="IRMA_CSTMP" HeaderText="010_Import Date" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HtmlEncode="False" DataFormatString="{0:yyyy/MM/dd}"></asp:BoundField>
                                                    <asp:BoundField DataField="cu_name" SortExpression="cu_name" HeaderText="011_Customer" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                    <asp:BoundField DataField="IRMA_STATUS" SortExpression="IRMA_STATUS" HeaderText="012_Status" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>

                                                    <asp:TemplateField HeaderText="013_Error Messag" SortExpression="IRMA_ERROR">
                                                        <HeaderStyle Width="15%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="UI_IRMA_ERROR" runat="server" Text='<%# Eval("IRMA_ERROR_ST") %>' CommandName="cmdErrorMsg" CommandArgument='<%# me.UI_dvRequest.Rows.Count%>'></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="014_Link" SortExpression="IRMA_FILE">
                                                        <HeaderStyle Width="15%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                        <ItemTemplate>
                                                            <a target="_self" href="<%=_TempExcelFolder %><%# Eval("IRMA_FILE") %>"><%# Eval("IRMA_FILE_ST") %></a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="015_Cancel">
                                                        <HeaderStyle Width="5%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
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
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
            <uc1:ucImportErrorMsg ID="ucImportErrorMsg1" runat="server"></uc1:ucImportErrorMsg>
            <uc2:ucMessage ID="ucMessage" runat="server" />

        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Label ID="UI_lblPreviousPage_RMAID" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="UI_lblPreviousPage_RMANO" runat="server" Visible="false"></asp:Label>
</asp:Content>
