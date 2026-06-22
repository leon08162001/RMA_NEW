<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Upload_Detail.aspx.vb" Inherits="Upload_Detail" Title="Untitled Page" %>

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

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table border="0" width="100%" id="table4" cellspacing="0" cellpadding="0" height="100%">
                <tr>
                    <td width="24" background="Images/pic_12.gif">&nbsp;</td>
                    <td valign="top" align="left">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0" height="100%">
                            <!--[Begin]Tittle-->
                            <tr style="height: 30px;">
                                <td width="3%">&nbsp;</td>
                                <td width="94%" align="left">
                                    <asp:Label ID="UI_lblTittle" runat="server" Text="111_Repair Detail" CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
                                </td>
                                <td width="3%">&nbsp;</td>
                            </tr>
                            <!--[End]Tittle-->
                            <!--[Begin]新增資料-->
                            <tr style="vertical-align: top; height: 100px;">
                                <td>&nbsp;</td>
                                <td align="left">
                                    <table width="100%" align="center" border="0" cellspacing="1" class="default">
                                        <tr>
                                            <td width="20%">&nbsp;
				                        <asp:Label ID="UI_lblSerial" runat="server" Text="013_Serial Number"></asp:Label>
                                            </td>
                                            <td>:
				                        <asp:Label ID="UI_lblSerialText" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;
				                        <asp:Label ID="UI_lblRepair" runat="server" Text="122_Repairer"></asp:Label>
                                            </td>
                                            <td>:
						                <asp:Label ID="UI_txtRepair" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;
				                        <asp:Label ID="UI_lblRepairDate" runat="server" Text="053_Repair Date"></asp:Label>
                                            </td>
                                            <td>:
						                <asp:Label ID="UI_txtRepairDate" runat="server"></asp:Label>
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
                <tr>
                    <td width="24" height="27" background="Images/pic_14.gif">&nbsp;</td>
                    <td height="27" background="Images/pic_15.gif">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="99%" class="default">
                                    <table border="0" width="100%" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td width="100%" align="left">
                                                <asp:Label ID="UI_lblInformationTittle" runat="server" Text="082_Replace Component" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td width="24" background="Images/pic_20.gif">&nbsp;</td>
                    <td valign="top" bgcolor="#E3D8BE" align="center" colspan="2">
                        <!-- List -->
                        <div class="form_div" align="center">
                            <fieldset>
                                <table class="default" style="border-collapse: collapse" bordercolor="#111111" cellspacing="0" cellpadding="5" width="100%" border="0">
                                    <tr>
                                        <td>
                                            <!-- [Begin] Search Export -->
                                            <asp:DataList ID="UI_dvRepairDetail" runat="server" ExtractTemplateRows="true" BorderColor="#c0c0c0" CellSpacing="0" CellPadding="3" Width="100%" border="1" bordercolorlight="#c0c0c0">
                                                <HeaderTemplate>
                                                    <asp:Table ID="oTableHeader" runat="server">
                                                        <asp:TableHeaderRow bgcolor="#fff4d0">
                                                            <asp:TableHeaderCell Width="15%">
                                                                <asp:Label ID="lblHNewPart" runat="server" Text="184_New Part No"></asp:Label>
                                                            </asp:TableHeaderCell>
                                                            <asp:TableHeaderCell Width="15%">
                                                                <asp:Label ID="lblHNewSerial" runat="server" Text="185_New Serial No"></asp:Label>
                                                            </asp:TableHeaderCell>
                                                            <asp:TableHeaderCell Width="30%" RowSpan="2">
                                                                <asp:Label ID="lblHDescription" runat="server" Text="099_Description"></asp:Label>
                                                            </asp:TableHeaderCell>
                                                            <asp:TableHeaderCell Width="10%" RowSpan="2">
                                                                <asp:Label ID="lblHImproperUsage" runat="server" Text="064_Improper Usage"></asp:Label>
                                                            </asp:TableHeaderCell>
                                                            <asp:TableHeaderCell Width="10%" RowSpan="2">
                                                                <asp:Label ID="lblHDefectReason" runat="server" Text="102_Defect Reason"></asp:Label>
                                                            </asp:TableHeaderCell>
                                                            <asp:TableHeaderCell Width="5%" RowSpan="2">
                                                                <asp:Label ID="lblHEdit" runat="server" Text="005_Edit"></asp:Label>
                                                            </asp:TableHeaderCell>
                                                            <asp:TableHeaderCell Width="5%" RowSpan="2">
                                                                <asp:Label ID="lblHDelete" runat="server" Text="007_Delete"></asp:Label>
                                                            </asp:TableHeaderCell>
                                                        </asp:TableHeaderRow>
                                                        <asp:TableHeaderRow bgcolor="#fff4d0">
                                                            <asp:TableHeaderCell>
                                                                <asp:Label ID="lblHPart" runat="server" Text="083_Part's No"></asp:Label>
                                                            </asp:TableHeaderCell>
                                                            <asp:TableHeaderCell>
                                                                <asp:Label ID="lblHSerial" runat="server" Text="098_Serial No"></asp:Label>
                                                            </asp:TableHeaderCell>
                                                        </asp:TableHeaderRow>
                                                    </asp:Table>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Table ID="oTableRow" runat="server">
                                                        <asp:TableRow>
                                                            <asp:TableCell>
                                                                <asp:Label ID="lblNewPart" runat="server" Text='<%# Eval("IRMARED_NPARTNO") %>'></asp:Label>&nbsp;
                                                            </asp:TableCell>
                                                            <asp:TableCell>
                                                                <asp:Label ID="lblNewSerial" runat="server" Text='<%# Eval("IRMARED_NSERIALNO") %>'></asp:Label>&nbsp;
                                                            </asp:TableCell>
                                                            <asp:TableCell RowSpan="2">
                                                                <asp:Label ID="lblDescription" runat="server" Height="50px" Width="300px" Text='<%# Eval("IRMARED_DESC") %>'></asp:Label>&nbsp;
                                                            </asp:TableCell>
                                                            <asp:TableCell RowSpan="2">
                                                                <asp:Label ID="lblImproperUsage" runat="server" Text='<%# Eval("IRMARED_IMPROPERUSAGE_NAME") %>'></asp:Label>&nbsp;
                                                            </asp:TableCell>
                                                            <asp:TableCell RowSpan="2">
                                                                <asp:Label ID="lblDefective" runat="server" Text='<%# Eval("defective_name") %>'></asp:Label>&nbsp;
                                                            </asp:TableCell>
                                                            <asp:TableCell RowSpan="2">
                                                                <asp:Button ID="UI_cmdEdit" runat="server" Text="_Edit" CssClass="Problem_Edit" CommandName="cmdEdit" CommandArgument='<%# Eval("IRMARED_ID") %>' />
                                                            </asp:TableCell>
                                                            <asp:TableCell RowSpan="2">
                                                                <asp:Button ID="UI_cmdDel" runat="server" Text="_Del" CssClass="Problem_Edit" CommandName="cmdDel" OnClientClick="return FrmDelete()" CommandArgument='<%# Eval("IRMARED_ID") %>' />
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell>
                                                                &nbsp;
                                                <asp:Label ID="lblIRMARED_ID" runat="server" Visible="false" Text='<%# Eval("IRMARED_ID") %>'></asp:Label>
                                                                <asp:Label ID="lblPart" Visible="false" runat="server" Text='<%# Eval("IRMARED_OPARTNO") %>'></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell>
                                                                <asp:Label ID="lblSerial" runat="server" Text='<%# Eval("IRMARED_OSERIALNO") %>'></asp:Label>&nbsp;
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                    </asp:Table>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Table ID="oTableRow" runat="server">
                                                        <asp:TableRow>
                                                            <asp:TableCell>
                                                                <asp:TextBox ID="lblNewPart" runat="server" Text='<%# Eval("IRMARED_NPARTNO") %>'></asp:TextBox>
                                                            </asp:TableCell>
                                                            <asp:TableCell>
                                                                <asp:TextBox ID="lblNewSerial" runat="server" Text='<%# Eval("IRMARED_NSERIALNO") %>'></asp:TextBox>
                                                            </asp:TableCell>
                                                            <asp:TableCell RowSpan="2">
                                                                <asp:TextBox ID="lblDescription" runat="server" TextMode="MultiLine" Height="50px" Width="300px" Text='<%# Eval("IRMARED_DESC") %>'></asp:TextBox>
                                                            </asp:TableCell>
                                                            <asp:TableCell RowSpan="2">
                                                                <asp:DropDownList ID="UI_cboImproperUsage" Width="80px" runat="server"></asp:DropDownList>
                                                            </asp:TableCell>
                                                            <asp:TableCell RowSpan="2">
                                                                <asp:DropDownList ID="UI_cboDefectReason" Width="120px" runat="server"></asp:DropDownList>
                                                            </asp:TableCell>
                                                            <asp:TableCell RowSpan="2">
                                                                <asp:Button ID="UI_cmdSave" runat="server" Text="_Save" CssClass="Problem_Edit" CommandName="cmdSave" CommandArgument='<%# Eval("IRMARED_ID") %>' />
                                                            </asp:TableCell>
                                                            <asp:TableCell RowSpan="2">
                                                 &nbsp;
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell>
                                                                &nbsp;
                                                <asp:TextBox ID="lblPart" Visible="false" runat="server" Text='<%# Eval("IRMARED_OPARTNO") %>'></asp:TextBox>
                                                                <asp:Label ID="lblIRMARED_ID" runat="server" Visible="false" Text='<%# Eval("IRMARED_ID") %>'></asp:Label>
                                                                <asp:Label ID="lblIRMARED_IMPROPERUSAGE" runat="server" Visible="false" Text='<%# Eval("IRMARED_IMPROPERUSAGE") %>'></asp:Label>
                                                                <asp:Label ID="lblIRMARED_DEFECTIVE" runat="server" Visible="false" Text='<%# Eval("IRMARED_DEFECTIVE") %>'></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell>
                                                                <asp:TextBox ID="lblSerial" runat="server" Text='<%# Eval("IRMARED_OSERIALNO") %>'></asp:TextBox>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                    </asp:Table>
                                                </EditItemTemplate>
                                            </asp:DataList>
                                            <!-- [End] Search Export -->
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                    </td>
                </tr>

                <tr height="10%">
                    <td background="Images/pic_20.gif">&nbsp;</td>
                    <td valign="top" bgcolor="#E3D8BE" align="center">
                        <!--[Begin]頁數-->
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="1%">&nbsp;</td>
                                <td width="99%" align="center" class="default">
                                    <input id="UI_cmdBack" runat="server" type="button" value="006_Back" class="Problem_Edit" onclick="javascript:history.back();" />&nbsp;
                                </td>
                            </tr>
                        </table>
                        <!--[End]頁數-->
                    </td>
                </tr>

            </table>

        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:Label ID="UI_lblPreviousPage_RMADID" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="UI_lblPreviousPage_RMANO" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="UI_lblMaterial" runat="server" Text="0" Visible="false"></asp:Label>
    <asp:Label ID="UI_Status" runat="server" Visible="false"></asp:Label>

</asp:Content>

