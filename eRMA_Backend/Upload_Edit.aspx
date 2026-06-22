<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Upload_Edit.aspx.vb" Inherits="Upload_Edit" Title="Untitled Page" %>

<%@ Register TagPrefix="Upload" Namespace="Brettle.Web.NeatUpload" Assembly="Brettle.Web.NeatUpload" %>
<%@ Register Src="ascx/ucProgressStatus.ascx" TagName="ucProgressStatus" TagPrefix="uc1" %>
<%@ Register Src="ascx/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>
<%@ Register Src="ascx/ucImportRepairDetail.ascx" TagName="ucRepairDetail" TagPrefix="uc3" %>



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

    <uc1:ucProgressStatus ID="ucProgressStatus" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>

            <table border="0" cellspacing="0" cellpadding="0" width="100%" height="410px">
                <tr height="90px">
                    <td width="24" background="Images/pic_12.gif">&nbsp;</td>
                    <td valign="top" align="left">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <!--[Begin]Tittle-->
                            <tr>
                                <td width="3%">&nbsp;</td>
                                <td width="94%" align="left">
                                    <asp:Label ID="UI_lblTittle" runat="server" Text="114_Request Detail" CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
                                </td>
                                <td width="3%">&nbsp;</td>
                            </tr>
                            <!--[End]Tittle-->
                            <!--[Begin]資料查詢條件區-->
                            <tr>
                                <td>&nbsp;</td>
                                <td align="left">
                                    <table width="95%" border="0" cellspacing="1" cellpadding="0" class="default">
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="UI_lblClientInformation" runat="server" Text="002_Client Information" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <asp:Label ID="UI_lblRMANo" runat="server" Text="029_RMA No."></asp:Label>
                                            </td>
                                            <td colspan="3">:
			                            <asp:Label ID="UI_lblRMANoText" runat="server"></asp:Label>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td width="15%">
                                                <asp:Label ID="UI_lblAccountID" runat="server" Text="003_Account ID"></asp:Label>
                                            </td>
                                            <td width="35%">:
			                            <asp:Label ID="UI_lblAccountIDText" runat="server"></asp:Label>
                                            </td>
                                            <td width="15%" align="left">
                                                <asp:Label ID="UI_lblAccountName" runat="server" Text="004_Account Name"></asp:Label>
                                            </td>
                                            <td width="35%" align="left">:
			                            <asp:Label ID="UI_lblAccountNameText" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="UI_lblApplicantID" runat="server" Text="045_Applicant ID"></asp:Label>
                                            </td>
                                            <td>:
			                            <asp:Label ID="UI_lblApplicantIDText" runat="server"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="UI_lblApplicant" runat="server" Text="006_Applicant"></asp:Label>
                                            </td>
                                            <td align="left">:
			                            <asp:Label ID="UI_lblApplicantText" runat="server"></asp:Label>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td valign="top">
                                                <asp:Label ID="UI_lblTel" runat="server" Text="007_Tel No"></asp:Label>
                                            </td>
                                            <td valign="top">:
			                            <asp:Label ID="UI_lblTelText" runat="server"></asp:Label>
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:Label ID="UI_lblAddress" runat="server" Text="008_Address"></asp:Label>
                                            </td>
                                            <td align="left">:
			                            <asp:Label ID="UI_lblAddressText" runat="server"></asp:Label>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <asp:Label ID="UI_lblRepairCenter" runat="server" Text="009_Repair Center"></asp:Label>
                                            </td>
                                            <td colspan="3">:
			                            <asp:Label ID="UI_lblRepairCenterText" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <!--[End]資料查詢條件區-->
                        </table>
                    </td>
                </tr>

                <tr height="20px">
                    <td width="24" background="Images/pic_14.gif">&nbsp;</td>
                    <td background="Images/pic_15.gif">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="1%">&nbsp;</td>
                                <td width="99%" align="left" class="default">
                                    <asp:Label ID="UI_lblProductTittle" runat="server" Text="" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>

                <tr height="250px">
                    <td background="Images/pic_20.gif">&nbsp;</td>
                    <td valign="top" bgcolor="#E3D8BE" align="center">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="1%">&nbsp;</td>
                                <td width="99%" align="left" class="default">
                                    <!--[Begin]資料列表表單-->
                                    <div align="center">
                                        <asp:GridView ID="UI_dvRetailDetail" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="UI_SeqID" runat="server" Text='<%# me.UI_dvRetailDetail.Rows.Count +1 %>'></asp:Label>
                                                        <asp:Label ID="UI_RMADID" runat="server" Text='<%# Eval("IRMAD_ID") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMADCSTMP" runat="server" Text='<%# Eval("IRMAD_CSTMP") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="004_Serial Number" SortExpression="IRMAD_SERIALNO">
                                                    <HeaderStyle Width="12%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="UI_lblIRMAD_SERIALNO" runat="server" Text='<%# Eval("IRMAD_SERIALNO") %>' CommandName="cmdEditSn" CommandArgument='<%# me.UI_dvRetailDetail.Rows.Count%>' PostBackUrl="~/Upload_Detail.aspx"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="IRMAD_MODELNO" HeaderText="016_Model No" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="IRMAD_WARRANTY" HeaderText="017_Warranty" DataFormatString="{0:yyyy-MM-dd}" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>

                                                <asp:TemplateField HeaderText="018_far_reason" SortExpression="farc_name">
                                                    <HeaderStyle Width="20%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="UI_farc_name" runat="server" Text='<%# Eval("farc_name") %>'></asp:Label>&nbsp;&nbsp;
                                            <asp:Label ID="UI_far_reason" runat="server" Text='<%# Eval("far_reason") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="IRMAD_PROBLEMDESC" HeaderText="019_Description" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:TemplateField HeaderText="020_Edit">
                                                    <HeaderStyle Width="5%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Button ID="UI_cmdEdit" runat="server" Text="_Edit" CssClass="Problem_Edit" CommandName="cmdEdit" CommandArgument='<%# me.UI_dvRetailDetail.Rows.Count%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="021_Delete">
                                                    <HeaderStyle Width="5%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="UI_cmdDelete" runat="server" ImageUrl="~/images/xx.gif" CommandName="cmdDelete" OnClientClick="return FrmDelete()" CommandArgument='<%# me.UI_dvRetailDetail.Rows.Count%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="irmad_status_name" HeaderText="012_Status" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="IRMAD_LUADNAME" HeaderText="022_Modified" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="IRMAD_LUSTMP" HeaderText="023_Date" DataFormatString="{0:yyyy-MM-dd}" HtmlEncode="false" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
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
                    <td background="Images/pic_20.gif">&nbsp;</td>
                    <td valign="top" bgcolor="#E3D8BE" align="center">
                        <!--[Begin]頁數-->
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="1%">&nbsp;</td>
                                <td width="99%" align="center" class="default">
                                    <input id="UI_cmdBack" runat="server" type="button" value="006_Back" class="Problem_Edit" onclick="javascript:history.back();" />&nbsp;
	                        <asp:Button ID="UI_cmdCancel" runat="server" Text="038_RMA Cancel" CssClass="Confirm" Visible="false" />
                                </td>
                            </tr>
                        </table>
                        <!--[End]頁數-->
                    </td>
                </tr>
            </table>

            <asp:Label ID="UI_lblPreviousPage_RMAID" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_lblPreviousPage_RMADID" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_lblPreviousPage_RMANO" runat="server" Visible="false"></asp:Label>
            <uc2:ucMessage ID="ucMessage" runat="server" />

            <uc3:ucRepairDetail ID="ucRepairDetail" runat="server" />

        </ContentTemplate>

        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="UI_cmdCancel" EventName="Click" />
            <asp:PostBackTrigger ControlID="ucMessage" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:ValidationSummary ID="vsRepairUpload" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="vsRepairUploadGroup" />

</asp:Content>

