<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Sales_WorkList_Item.aspx.vb" Inherits="Sales_WorkList_Item" Title="Untitled Page" %>

<%@ Register Src="ascx/ucProgressStatus.ascx" TagName="ucProgressStatus" TagPrefix="uc1" %>
<%@ Register Src="ascx/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>
<%@ Register Src="ascx/ucSalesDetail.ascx" TagName="ucSalesDetail" TagPrefix="uc3" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
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
                                                <asp:Label ID="UI_lblRMANo" runat="server" Text="029_RMA No"></asp:Label>
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

                <tr height="28px">
                    <td width="24" background="Images/pic_14.gif">&nbsp;</td>
                    <td background="Images/pic_15.gif">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="1%">&nbsp;</td>
                                <td width="49%" align="left" class="default">
                                    <asp:Label ID="UI_lblRequestedTittle" runat="server" Text="034_Requested Information" Font-Bold="true"></asp:Label>
                                </td>
                                <td width="50%" align="right" class="default">&nbsp;</td>
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
                                    <div align="center">

                                        <asp:GridView ID="UI_dvSales" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="UI_CheckGroup" runat="server" AutoPostBack="true" OnCheckedChanged="UI_checkGroup_CheckedChanged" Visible="false" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="UI_Check" runat="server" AutoPostBack="true" OnCheckedChanged="UI_Check_CheckedChanged" Visible="false" />
                                                        <asp:Label ID="UI_SeqID" runat="server" Text='<%# Eval("SeqID") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMADID" runat="server" Text='<%# Eval("RMAD_ID") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMAID" runat="server" Text='<%# Eval("RMA_ID") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMANO" runat="server" Text='<%# Eval("RMA_NO") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMASQID" runat="server" Text='<%# Eval("RMASQ_ID") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_Status" runat="server" Text='<%# Eval("RMAD_STATUS") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMA_COMPNO" runat="server" Text='<%# Eval("RMA_COMPNO") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMAR_COMPNO" runat="server" Text='<%# Eval("RMAR_COMPNO") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="RMAD_SERIALNO" HeaderText="013_Serial Number" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="RMAD_MODELNO" HeaderText="035_Model" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="RMAD_WARRANTY" HeaderText="015_Warranty" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center" HtmlEncode="False" DataFormatString="{0:yyyy/MM/dd}"></asp:BoundField>
                                                <asp:BoundField DataField="RMA_CSTMP" HeaderText="033_Request Date" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center" HtmlEncode="False" DataFormatString="{0:yyyy/MM/dd}"></asp:BoundField>


                                                <asp:TemplateField HeaderText="070_Quote">
                                                    <HeaderStyle Width="10%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="UI_RepairCode" runat="server" Text='<%# Eval("RepairCode") %>'></asp:Label>
                                                        <asp:Label ID="UI_RepairQuoted" runat="server" Text='<%# Eval("RepairQuoted") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="037_Amount">
                                                    <HeaderStyle Width="10%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="UI_SaleCode" runat="server" Text='<%# Eval("SaleCode") %>'></asp:Label>
                                                        <asp:Label ID="UI_SaleQuoted" runat="server" Width="50px" Text='<%# Eval("SaleQuoted") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="Status" HeaderText="032_Status" HeaderStyle-Width="9%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>

                                                <asp:TemplateField HeaderText="038_Detail ">
                                                    <HeaderStyle Width="9%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Button ID="UI_cmdEdit" runat="server" Text="_Quoting" CssClass="Pick" Width="50" CommandName="cmdEdit" CommandArgument='<%# me.UI_dvSales.Rows.Count%>' />
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
                            <tr>
                                <td colspan="2" align="center">
                                    <br />
                                    <input id="UI_cmdBack" runat="server" type="button" value="006_back" class="Problem_Edit" onclick="javascript:history.back();" />&nbsp;&nbsp;
	                        <asp:Button ID="UI_cmdSubmit" runat="server" Text="040_Submit to Customer" CssClass="Confirm_l" Enabled="false" OnClientClick="onProgress('Process')" />&nbsp;&nbsp;
	                        <asp:Button ID="UI_cmdConfirm" runat="server" Text="041_Confirm by Sales" CssClass="Confirm_l" Enabled="false" OnClientClick="onProgress('Process')" />
                                    <asp:Button ID="UI_QuoteDownload" OnClick="Button1_Click" runat="server" Text="H_014_Quote Download" CssClass="Confirm_l" OnClientClick="onProgress('Process')"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <asp:Label ID="UI_lblPreviousPage_RMAID" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_lblPreviousPage_RMANO" runat="server" Visible="false"></asp:Label>

            <uc2:ucMessage ID="ucMessage" runat="server" />
            <uc3:ucSalesDetail ID="UcSalesDetail1" runat="server" />

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="UI_cmdSubmit" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="UI_cmdConfirm" EventName="Click" />
            <asp:PostBackTrigger ControlID="ucMessage" />
        </Triggers>
    </asp:UpdatePanel>
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true"></CR:CrystalReportViewer>

    <asp:ValidationSummary ID="vsSaleSave" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="SaleSaveGroup" />

</asp:Content>

