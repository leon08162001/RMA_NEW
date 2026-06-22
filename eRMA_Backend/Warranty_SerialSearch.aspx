<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Warranty_SerialSearch.aspx.vb" Inherits="Warranty_SerialSearch" %>

<%@ Register Src="ascx/ucProgressStatus.ascx" TagName="ucProgressStatus" TagPrefix="uc1" %>
<%@ Register Src="ascx/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>
<%@ Register Src="ascx/ucSKU.ascx" TagName="ucSKU" TagPrefix="uc6" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <script type="text/javascript" language="javascript">
        function startDownload() {
            var uc = document.getElementById('<%= ucProgressStatus.ClientID %>');
            uc.style.display = 'block';

            var iframe = document.getElementById('downloadFrame');
            iframe.onload = function () {
                uc.style.display = 'none';
            };
            iframe.src = '<%= ResolveUrl("/ashx/ExportWarrantyExcel.ashx") %>';
        }
    </script>
    <uc1:ucProgressStatus ID="ucProgressStatus" runat="server" Visible="false" />
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
                                            <asp:Label ID="lblTittle" runat="server" Text="021_Speciall Warranty Setting-Searching"
                                                ForeColor="#326B9B" CssClass="text_tittle"></asp:Label>
                                        </td>
                                        <td width="3%">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td align="left">
                                            <table class="default" cellspacing="1" cellpadding="0" width="95%" border="0">
                                                <tbody>
                                                    <tr>
                                                        <td width="8%">
                                                            <asp:Label ID="lblImport" runat="server" Text="022_Import patch of serial No. and output warranty information to below"></asp:Label>
                                                            :
                                                           <br />
                                                            <input name="FileUp" type="file" id="FileUp" runat="server" class="input_upload" size="50" />
                                                            <asp:Button ID="btnImport" runat="server" Text="_Import" CssClass="Confirm_l" />
                                                            <asp:Label ID="lblSKU" runat="server" Text="SN"></asp:Label>
                                                            :
                                                            <asp:TextBox ID="txtSKU" runat="server" Width="120"></asp:TextBox>
                                                            <asp:Button ID="UI_cmdSearch" runat="server" Text="_Search" CssClass="Search"></asp:Button>

                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                           Example Download: <a href="sn.xls">SN.xls</a>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                        <td>&nbsp;
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
                                            <asp:Label ID="UI_lblPriceList" runat="server" Text="025_Price List" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td class="default" align="right" width="50%">
                                            <asp:Button ID="btnExportWarranty" runat="server" Text="Export" CssClass="Confirm" OnClick="btnExportWarranty_Click" Visible="false" ></asp:Button>
                                            <asp:Button ID="btnExportWarranty1" runat="server" Text="Export" CssClass="Confirm" OnClientClick="startDownload(); return false;"></asp:Button>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr valign="top">
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
                                                <asp:GridView ID="dgvImport" runat="server" CssClass="default" Width="100%" PagerSettings-Mode="Numeric" AllowPaging="true" AutoGenerateColumns="False" border="0"
                                                    CellSpacing="1" CellPadding="0" AllowSorting="true">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="UI_SEQID" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="SerialNo" SortExpression="SerialNo" HeaderText="SerialNo"
                                                            HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="CWStart" SortExpression="CWStart" HeaderText="CW Start"
                                                            HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="CWEnd" SortExpression="CWEnd" HeaderText="CW End"
                                                            HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="EWStart" SortExpression="EWStart" HeaderText="EW Start"
                                                            HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="EWEnd" SortExpression="EWEnd" HeaderText="EW End"
                                                            HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="WAR_TYPE" SortExpression="WAR_TYPE" HeaderText="Warranty TYPE"
                                                            HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="WAR_PROGRAM_TYPE" SortExpression="WAR_PROGRAM_TYPE" HeaderText="PROGRAM TYPE"
                                                            HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="WAR_ITEM_TYPE" SortExpression="WAR_ITEM_TYPE" HeaderText="ITEM TYPE"
                                                            HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="WAR_PRICE_VER" SortExpression="WAR_PRICE_VER" HeaderText="PRICE VER"
                                                            HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="Model" SortExpression="Model" HeaderText="Model"
                                                            HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="SKU" SortExpression="SKU" HeaderText="SKU"
                                                            HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="Customer" SortExpression="Customer" HeaderText="Customer"
                                                            HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="ShipNo" SortExpression="ShipNo" HeaderText="Order No"
                                                            HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="DeliverDate" SortExpression="DeliverDate" HeaderText="DeliverDate"
                                                            HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
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
                                            <asp:Label ID="lblPurchasing" runat="server" Text="025_Purchasing Records" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td class="default" align="right" width="50%">
                                            <asp:Button ID="btnPurWarranty" runat="server" Text="Export" CssClass="Confirm" OnClick="btnPurWarranty_Click"></asp:Button>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr valign="top">
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
                                                <asp:GridView ID="dgvPurchasing" runat="server" CssClass="default" Width="100%" PagerSettings-Mode="Numeric" AllowPaging="true" AutoGenerateColumns="False" border="0"
                                                    CellSpacing="1" CellPadding="0" AllowSorting="true">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="UI_SEQID" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="SerialNo" SortExpression="SerialNo" HeaderText="SerialNo"
                                                            HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="PurchaseDate" SortExpression="PurchaseDate" HeaderText="PurchaseDate"
                                                            HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="WarrantyCode" SortExpression="WarrantyCode" HeaderText="Type"
                                                            HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="StartDate" SortExpression="StartDate" HeaderText="StartDate"
                                                            HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="EndDate" SortExpression="EndDate" HeaderText="EndDate"
                                                            HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="Model" SortExpression="Model" HeaderText="Model"
                                                            HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="SKU" SortExpression="SKU" HeaderText="SKU"
                                                            HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description"
                                                            HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>


                                                        <asp:TemplateField HeaderText="WAR_SPEC_DESC">
                                                            <HeaderStyle Width="15%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="WAR_SPEC_DESC" runat="server"></asp:Label>
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
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
            <uc2:ucMessage ID="ucMessage" runat="server"></uc2:ucMessage>
            <uc6:ucSKU ID="ucSKU" runat="server"></uc6:ucSKU>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="UI_cmdSearch" />
            <asp:PostBackTrigger ControlID="ucMessage" />
            <asp:PostBackTrigger ControlID="btnImport" />
            <asp:PostBackTrigger ControlID="btnExportWarranty" />
            <asp:PostBackTrigger ControlID="btnPurWarranty" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="vsSaleSave" runat="server" ShowMessageBox="true" ShowSummary="false"
        ValidationGroup="SaleSaveGroup" />
    <asp:Label runat="server" ID="lblPreviousPage_SwID" Visible="false"></asp:Label>
</asp:Content>

