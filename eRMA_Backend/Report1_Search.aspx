<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Report1_Search.aspx.vb" Inherits="Report1_Search" Title="Untitled Page" %>

<%@ Register Src="ascx/ucSubstitute.ascx" TagName="ucSubstitute" TagPrefix="uc1" %>
<%@ Register Src="ascx/ucPartPic.ascx" TagName="ucPartPic" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
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
                                    <asp:Label ID="UI_lblTittle" runat="server" Text="001_Part's Query" CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
                                </td>
                                <td width="3%">&nbsp;</td>
                            </tr>

                            <!--[End]Tittle-->
                            <!--[Begin]資料查詢條件區-->
                            <tr>
                                <td>&nbsp;</td>
                                <td align="left">

                                    <asp:Panel runat="server" ID="UI_panSearch" DefaultButton="UI_cmdSearch">
                                        <table width="800px" border="0" cellspacing="1" cellpadding="0" class="default">
                                            <tr>
                                                <td width="15%">
                                                    <asp:Label ID="UI_lblPrimalModelNo" runat="server" Text="149_Model No原型機種"></asp:Label>
                                                </td>
                                                <td width="35%">:
			                            <asp:RadioButton runat="server" ID="UI_radSelect1" GroupName="radSelect" />
                                                    <asp:DropDownList runat="server" ID="UI_cboPrimalModelNo"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <%--
			                    <tr>
			                        <td>
			                            <asp:Label ID="UI_lblSerialNo" runat="server" Text="150_Serial No"></asp:Label>
			                        </td>
			                        <td align="left" colspan ="3">:
			                            <asp:RadioButton runat="server" ID="UI_radSelect2" GroupName="radSelect" />
			                            <asp:TextBox ID="UI_txtSerialNo" runat="server" Width="120"></asp:TextBox>
	                                </td>
			                    </tr>
                                            --%>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="UI_lblProduct_SerialNo" runat="server" Text="151_Product’s Serial No"></asp:Label>
                                                </td>
                                                <td align="left" colspan="3">:
			                            <asp:RadioButton runat="server" ID="UI_radSelect3" GroupName="radSelect" />
                                                    <asp:TextBox ID="UI_txtProduct_SerialNo" runat="server" Width="120"></asp:TextBox>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;
			                            <asp:Label runat="server" ID="UI_lblUpperPartsNo3" Text="165_upper Part's No"></asp:Label>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td>
                                                    <asp:Label ID="UI_lblPartsNo" runat="server" Text="152_Part’s No"></asp:Label>
                                                </td>
                                                <td align="left" colspan="3">:
			                            <asp:RadioButton runat="server" ID="UI_radSelect4" Checked="true" GroupName="radSelect" />
                                                    <asp:TextBox ID="UI_txtPartsNo" runat="server" Width="120"></asp:TextBox>
                                                    <asp:Button ID="UI_cmdSearch" runat="server" Text="004_Search" CssClass="Search" />
                                                </td>
                                            </tr>

                                        </table>
                                    </asp:Panel>

                                </td>
                                <td>&nbsp;</td>
                            </tr>

                            <tr>
                                <td colspan="3">&nbsp;</td>
                            </tr>
                            <!--[End]資料查詢條件區-->
                        </table>
                    </td>
                </tr>

                <tr height="28">
                    <td width="24" background="Images/pic_14.gif">&nbsp;</td>
                    <td background="Images/pic_15.gif">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="1%">&nbsp;</td>
                                <td width="99%" align="left" class="default">
                                    <asp:Label ID="UI_lblReportTittle" runat="server" Text="030_Report Information" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>

                <tr height="80%">
                    <td background="Images/pic_20.gif">&nbsp;</td>
                    <td valign="top" bgcolor="#E3D8BE" align="left">
                        <table runat="server" id="tbReport" border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="1%">&nbsp;</td>
                                <td width="99%" align="left" class="default">
                                    <!--[Begin]資料列表表單-->
                                    <div align="center">
                                        <asp:GridView ID="UI_dvReport" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderStyle Width="35px" Height="15px" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="UI_SeqID" runat="server"></asp:Label>
                                                        <asp:Label ID="UI_bmb03" runat="server" Text='<%# Eval("bmb03") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_Substitute1" runat="server" Text='<%# Eval("Substitute1") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_Substitute2" runat="server" Text='<%# Eval("Substitute2") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_imgFile" runat="server" Text='<%# Eval("imgfile") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="EXPORT_MODELNO" HeaderText="101_Model No" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="150px"></asp:BoundField>
                                                <asp:BoundField DataField="bmb03" HeaderText="152_Part’s No" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="150px"></asp:BoundField>
                                                <asp:BoundField DataField="RPBOM_DESC" HeaderText="161_Description" ItemStyle-HorizontalAlign="left"></asp:BoundField>
                                                <asp:BoundField DataField="RPBOM_MATERIALCOST" HeaderText="191_ListPrice" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px"></asp:BoundField>
                                                <asp:TemplateField HeaderText="162_取代">
                                                    <HeaderStyle Width="80px" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Button ID="UI_cmdSubstitute1" runat="server" Text="073_Edit" CssClass="Problem_Edit" CommandName="cmdSubstitute1" CommandArgument='<%#Me.UI_dvReport.Rows.Count%>' Visible="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="163_替代">
                                                    <HeaderStyle Width="80px" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Button ID="UI_cmdSubstitute2" runat="server" Text="074_Edit" CssClass="Problem_Edit" CommandName="cmdSubstitute2" CommandArgument='<%#Me.UI_dvReport.Rows.Count%>' Visible="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="164_Pic">
                                                    <HeaderStyle Width="80px" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Button ID="UI_cmdPic" runat="server" Text="075_Edit" CssClass="Problem_Edit" CommandName="cmdPic" CommandArgument='<%#Me.UI_dvReport.Rows.Count%>' Visible="false" />
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
                            <tr>
                                <td align="left" colspan="3">
                                    <p style="margin-top: 10px; padding-left: 50px">
                                        <asp:Button ID="UI_cmdExport" runat="server" Text="076_Export" CssClass="Confirm"
                                            OnClick="UI_cmdExport_Click"></asp:Button>
                                        <asp:Button ID="UI_ExportWithPic" runat="server" Text="Export" CssClass="Confirm" />
                                    </p>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="3">
                                    <p style="margin-top: 10px; padding-left: 50px">
                                        <asp:Button ID="UI_ExportAll" runat="server" Text="Exp All Model" CssClass="Confirm" />
                                    </p>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <uc1:ucSubstitute ID="ucSubstitute" runat="server" />
            <uc2:ucPartPic ID="ucPartPic" runat="server" />
        </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="UI_cmdSearch" />
            <asp:PostBackTrigger ControlID="UI_cmdExport" />
            <asp:PostBackTrigger ControlID="UI_ExportWithPic" />
            <asp:PostBackTrigger ControlID="UI_ExportAll" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>
