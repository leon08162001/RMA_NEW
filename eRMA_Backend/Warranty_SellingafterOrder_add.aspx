<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Warranty_SellingafterOrder_add.aspx.vb" Inherits="Warranty_SellingafterOrder_add" Title="Untitled Page" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register Src="ascx/ucProgressStatus.ascx" TagName="ucProgressStatus" TagPrefix="uc1" %>
<%@ Register Src="ascx/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>
<%@ Register Src="ascx/ucWarrantyOrderCust.ascx" TagName="ucWarrantyOrderCust" TagPrefix="uc6" %>
<%@ Register Src="ascx/ucParts.ascx" TagName="ucParts" TagPrefix="uc7" %>
<%@ Register Src="ascx/ucWarrantyOrderOrder.ascx" TagName="ucWarrantyOrderOrder" TagPrefix="uc8" %>
<%@ Register Src="ascx/ucWarrantyOrderSKU.ascx" TagName="ucWarrantyOrderSKU" TagPrefix="uc9" %>
<%@ Register Src="ascx/ucWarrantyOrderSNAdd.ascx" TagName="ucWarrantyOrderSNAdd" TagPrefix="uc10" %>
<%@ Register Src="ascx/ucWarrantyOrderSNView.ascx" TagName="ucWarrantyOrderSNView" TagPrefix="uc11" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <script type="text/javascript" language="javascript">
        function CheckNoData() {
            //var id = ddl.id;
            //var selectedVal = ddl.value;
            var selectedVal = document.getElementById("<%=rdoType.ClientID%>").value;
            if (selectedVal == "") {

                //如果value是空白的，就返回。

                alert("Warranty Type Not choose");
                return false;
            }

        }
    </script>

    <uc1:ucProgressStatus ID="ucProgressStatus" runat="server" />

    <table border="0" width="100%" id="table1" cellspacing="0" cellpadding="0">
        <tr>
            <td width="24" background="Images/pic_12.gif">&nbsp;
            </td>
            <td valign="top" align="left">
                <table border="0" width="100%" cellspacing="0" cellpadding="0" height="100%">
                    <!--[Begin]Tittle-->
                    <tr>
                        <td width="3%">&nbsp;</td>
                        <td width="94%" align="left">
                            <asp:Label ID="UI_lblTittle" runat="server" Text="Selling after Order"
                                CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
                        </td>
                        <td width="3%">&nbsp;
                        </td>
                    </tr>
                    <!--[End]Tittle-->
                    <!--[Begin]新增資料-->
                    <tr>
                        <td>&nbsp;
                        </td>
                        <td align="left">
                            <table width="100%" align="center" border="0" cellspacing="1" class="default">
                                <tr>
                                    <td colspan="6">&nbsp;<asp:Label ID="UI_lblSubTittle" runat="server" Text="Please fill in the header below and click button Next, system will list the Order table."></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnPI" runat="server" Text="PI" CssClass="Confirm" />
                                        <asp:Button ID="btnService" runat="server" Text="Service" CssClass="Confirm" Visible="False" />
                                        <asp:Button ID="btnNewService" runat="server" Text="Service" CssClass="Confirm" />
                                        <asp:Button ID="btnReMoCloud" runat="server" Text="ReMoCloud" CssClass="Confirm" Visible="False" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="100px" align="left">
                                        <asp:Label ID="lblWarrantyNo" runat="server" Text="Warranty No"></asp:Label>
                                    </td>
                                    <td width="1" align="left">:
                                    </td>
                                    <td width="20%" align="left">
                                        <asp:TextBox ID="txtWarrantyNo" runat="server" Width="150"></asp:TextBox>
                                    </td>
                                    <td width="10%" align="right">
                                        <asp:Label ID="lblOperationCenter" runat="server" Text="Order Type"></asp:Label>
                                    </td>
                                    <td align="left">:
                                                <asp:DropDownList ID="cboOperationCenter" runat="server" Width="150px">
                                                </asp:DropDownList>
                                    </td>
                                    <td width="10%" align="right">
                                        <asp:Label ID="lblSales" runat="server" Text="Sales"></asp:Label>
                                    </td>
                                    <td width="20%" align="left">:
                                                <asp:TextBox ID="txtSales" runat="server" Width="70px"></asp:TextBox>
                                        <asp:TextBox ID="txtSalesName" runat="server" Width="80px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblCustomer" runat="server" Text="Customer"></asp:Label>
                                    </td>
                                    <td width="1" align="left">:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtCustomer" runat="server" Width="40px"></asp:TextBox>
                                        <asp:TextBox ID="txtCustomerName" runat="server" Width="50px"></asp:TextBox>
                                        <asp:Button ID="cmdPickCustomer" runat="server" Text="_Pick" CssClass="Pick"></asp:Button>
                                    </td>
                                    <td width="10%" align="right">
                                        <asp:Label ID="lblOrderType" runat="server" Text="Order Type"></asp:Label>
                                    </td>
                                    <td align="left">:
                                                <asp:DropDownList ID="cboWarrantyType" runat="server" Width="120px">
                                                    <asp:ListItem Value="" Text="-Select-" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="1.Local"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="2.Export"></asp:ListItem>
                                                </asp:DropDownList>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblERPNo" runat="server" Text="Invoice No"></asp:Label>
                                    </td>
                                    <td align="left">:
                                                <asp:TextBox ID="txtErpNo" runat="server" Width="150"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblPurDate" runat="server" Text="Purchase Date"></asp:Label>
                                    </td>
                                    <td width="1" align="left">:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtPurDate" runat="server" Width="150px"></asp:TextBox>
                                        <img style="vertical-align: middle" alt="" src="Images/icon_cal.gif" onclick="window.showModalDialog('Calendar.aspx',document.elementFromPoint(event.x-60,event.y),'dialogHeight:195px;dialogWidth:224px;dialogTop:'+(parseInt(window.event.screenY)+10)+';dialogLeft:'+window.event.screenX+';help:0;resizable:0;status:0;')" />
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblCurrency" runat="server" Text="Currency"></asp:Label>
                                    </td>
                                    <td align="left">:
                                                <asp:TextBox ID="txtCurrency" runat="server" Width="120"></asp:TextBox>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblFlow" runat="server" Text="Status"></asp:Label>
                                    </td>
                                    <td align="left">:
                                                <asp:TextBox ID="txtFlow" runat="server" Width="150"></asp:TextBox>
                                        <asp:TextBox ID="txtConfirm" runat="server" Width="1" Visible="false"></asp:TextBox>
                                        <asp:TextBox ID="txtSubmit" runat="server" Style="display: none"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblcustpo" runat="server" Text="Cust PO"></asp:Label>
                                    </td>
                                    <td width="1" align="left">:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtCustPo" runat="server" Width="150px"></asp:TextBox>
                                    </td>
                                    <%--<td align="right">
                                                <asp:Label ID="Label2" runat="server" Text="Currency"></asp:Label>
                                            </td>
                                            <td align="left">
                                                :
                                                <asp:TextBox ID="TextBox2" runat="server" Width="120"></asp:TextBox>
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="Label3" runat="server" Text="Status"></asp:Label>
                                            </td>
                                            <td align="left">
                                                :
                                                <asp:TextBox ID="TextBox3" runat="server" Width="150"></asp:TextBox>
                                                <asp:TextBox ID="TextBox4" runat="server" Width="1" Visible="false"></asp:TextBox>
                                                <asp:TextBox ID="TextBox5" runat="server" style="display:none"></asp:TextBox>
                                            </td>--%>
                                </tr>
                            </table>
                        </td>
                        <td>&nbsp;
                                   <asp:Button ID="UI_cmdSave" ValidationGroup="AddGroup" runat="server" Text="_Save" CssClass="Search" Style="height: 18px" />

                        </td>
                    </tr>
                    <!--[End]新增資料-->
                </table>
                <asp:Panel ID="pnAdd" runat="server" Width="100%">
                    <table border="0" width="100%" id="Step2" cellspacing="0" cellpadding="0" style="vertical-align: top;" class="default">
                        <tr align="Left">
                            <td width="3%" background="images/pic_15.gif" class="default">&nbsp;</td>
                            <td width="30%" height="27" background="images/pic_15.gif" class="default">
                                <asp:Label ID="lblOrderNo" runat="server" Text="Order No"></asp:Label>
                                <asp:TextBox ID="txtOrderNo" ReadOnly="true" runat="server" Width="110"></asp:TextBox>
                                <asp:TextBox ID="txtOrderSeq" ReadOnly="true" runat="server" Width="30"></asp:TextBox>
                                <asp:Button ID="btnOrderSel" runat="server" Text="Select" CssClass="Pick"></asp:Button>
                            </td>
                            <td width="20%" height="27" background="images/pic_15.gif" class="default">
                                <asp:Label ID="lblSKU" runat="server" Text="SKU No"></asp:Label>
                                <asp:TextBox ID="txtSKU" ReadOnly="true" runat="server" Width="120"></asp:TextBox>
                                <asp:Button ID="btnSKUSel" runat="server" Text="Select" CssClass="Pick"></asp:Button>
                            </td>
                            <td width="20%" height="27" background="images/pic_15.gif" class="default">
                                <asp:Label ID="lblModel" runat="server" Text="Model"></asp:Label>
                                <asp:Label ID="txtModel" runat="server" Text=""></asp:Label>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="lblDescription" runat="server" Text="Description"></asp:Label>
                                <asp:Label ID="txtDescription" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr align="left">
                            <td width="3%" background="images/pic_15.gif" class="default">&nbsp;</td>
                            <td height="27" background="images/pic_15.gif" class="default">
                                <asp:Label ID="lblWarrantyType" runat="server" Text="Warranty Type"></asp:Label>
                                <%--<asp:RadioButtonList ID="rdoType" runat="server" AutoPostBack="true" RepeatColumns="3" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                <asp:ListItem Text="CW" Value="CW" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="EW " Value="EW"></asp:ListItem>
                                                <asp:ListItem Text="SW" Value="SW"></asp:ListItem>
                                            </asp:RadioButtonList>--%>
                                <asp:DropDownList ID="rdoType" runat="server" Width="150" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td height="27" background="images/pic_15.gif" class="default">
                                <asp:Label ID="lblVersion" runat="server" Text="Version"></asp:Label>
                                <asp:DropDownList ID="cboVersion" runat="server" AutoPostBack="true" Width="200"></asp:DropDownList>
                            </td>
                            <td height="27" background="images/pic_15.gif" class="default">
                                <asp:Label ID="lblPurchaseYear" runat="server" Text="Purchase Year"></asp:Label>
                                <asp:TextBox ID="txtPurchaseYear" runat="server" Width="40" onblur="OrderYearsEnter(this);"></asp:TextBox>
                                <asp:Button ID="btnSaveOrder" runat="server" Text="Add" CssClass="Search" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlFile" runat="server" Width="100%">
                    <table border="0" width="100%" id="Table2" cellspacing="0" cellpadding="0" style="vertical-align: top;" class="default">
                        <tr align="Left">
                            <td width="3%" background="images/pic_15.gif" class="default">&nbsp;</td>
                            <td>
                                <asp:GridView ID="dvWarrantyItem" runat="server" CssClass="default" Width="100%" AutoGenerateColumns="False" border="0"
                                    CellSpacing="1" CellPadding="0">
                                    <Columns>
                                        <asp:BoundField DataField="wati_seq" HeaderText="Seq" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="wati_order" HeaderText="Order No" HeaderStyle-Width="9%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="wati_ordseq" HeaderText="Item" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="wati_skuno" HeaderText="SKU No" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="wati_skudesc" HeaderText="Description" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="wati_type" HeaderText="Type" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="WAR_VERSION" HeaderText="Version" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="wati_name" HeaderText="Warranty Name" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="wati_year" HeaderText="Years" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="wati_qty" HeaderText="Ｑ’ty" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:TemplateField HeaderText="Unit Price">
                                            <HeaderStyle Width="6%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPrice" runat="server" Width="40px" Text='<%# Eval("wati_price") %>' onblur="ItemPriceEnter(this);"></asp:TextBox>
                                                <asp:TextBox ID="txtBase" runat="server" Width="40px" Text='<%# Eval("wati_base") %>' Style="display: none"></asp:TextBox>
                                                <asp:TextBox ID="txtPriceOld" runat="server" Width="40px" Text='<%# Eval("wati_price") %>' Style="display: none"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="wati_base" HeaderText="Base Price" HeaderStyle-Width="6%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:TemplateField HeaderText="">
                                            <HeaderStyle Width="12%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblwati_base" runat="server" Text='<%# Eval("wati_base") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblQty" runat="server" Text='<%# Eval("wati_qty") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblOrder" runat="server" Text='<%# Eval("wati_order") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblwati_seq" runat="server" Text='<%# Eval("wati_seq") %>' Visible="false"></asp:Label>
                                                <asp:Button ID="btnSerial" runat="server" Text="Serial" CssClass="Pick" Width="50" CommandArgument='<%# me.dvWarrantyItem.Rows.Count%>' CommandName="cmdAdd" />
                                                <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="Pick" Width="50" OnClientClick="return confirm('Are your sure?')" CommandArgument='<%# me.dvWarrantyItem.Rows.Count%>' CommandName="cmdDel" />
                                                <asp:Button ID="btnView" runat="server" Text="Detail" CssClass="Pick" Width="50" CommandArgument='<%# me.dvWarrantyItem.Rows.Count%>' CommandName="cmdDetail" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <asp:Label runat="server" ID="UI_NotFound" Text='<%# Session("_isShowNotFound").ToString()%>'></asp:Label>
                                    </EmptyDataTemplate>
                                    <EmptyDataRowStyle HorizontalAlign="Center" />
                                    <HeaderStyle CssClass="Text_Head" />
                                    <RowStyle CssClass="TR_1" />
                                    <AlternatingRowStyle CssClass="ListRowEven" />
                                    <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Center" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                    <table border="0" width="100%" id="Table3" cellspacing="0" cellpadding="0" style="vertical-align: top;" class="default">
                        <tr align="center">
                            <td width="3%" background="images/pic_15.gif" class="default">&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr align="center">
                            <td width="3%" background="images/pic_15.gif" class="default">&nbsp;</td>
                            <td>
                                <asp:TextBox ID="txtDiffCnt" runat="server" Style="display: none"></asp:TextBox>
                                <asp:Button ID="btnCancel" runat="server" Text="Back" CssClass="Problem_Edit" />
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Confirm" />
                                <asp:Button ID="btnSubmit" runat="server" Text="Confirm" CssClass="Confirm" />
                                <asp:Button ID="btnDelete" runat="server" Text="Cancel" CssClass="Confirm" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <uc2:ucMessage ID="ucMessage" runat="server" />
    <uc6:ucWarrantyOrderCust ID="ucWarrantyOrderCust" runat="server"></uc6:ucWarrantyOrderCust>
    <uc7:ucParts ID="ucParts" runat="server"></uc7:ucParts>
    <uc8:ucWarrantyOrderOrder ID="ucWarrantyOrderOrder" runat="server"></uc8:ucWarrantyOrderOrder>
    <uc9:ucWarrantyOrderSKU ID="ucWarrantyOrderSKU" runat="server"></uc9:ucWarrantyOrderSKU>
    <uc10:ucWarrantyOrderSNAdd ID="ucWarrantyOrderSNAdd" runat="server"></uc10:ucWarrantyOrderSNAdd>
    <uc11:ucWarrantyOrderSNView ID="ucWarrantyOrderSNView" runat="server"></uc11:ucWarrantyOrderSNView>

    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
</asp:Content>
