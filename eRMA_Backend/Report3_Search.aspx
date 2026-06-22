<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Report3_Search.aspx.vb" Inherits="Report3_Search" Title="Untitled Page" %>

<%@ Register Src="ascx/UcSDCViewG.ascx" TagName="UcSDCViewG" TagPrefix="uc10" %>
<%@ Register Src="ascx/UcWarrantyView.ascx" TagName="UcWarrantyView" TagPrefix="uc11" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <table border="0" cellspacing="0" cellpadding="0" width="100%" height="100%">
        <tr height="10%">
            <td width="24" background="Images/pic_12.gif">&nbsp;</td>
            <td valign="top" align="left">
                <table border="0" width="100%" cellspacing="0" cellpadding="0">
                    <!--[Begin]Tittle-->
                    <tr>
                        <td width="3%">&nbsp;</td>
                        <td width="94%" align="left">
                            <asp:Label ID="UI_lblTittle" runat="server" Text="003_Warranty Query" CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
                        </td>
                        <td width="3%">&nbsp;</td>
                    </tr>

                    <!--[End]Tittle-->
                    <!--[Begin]資料查詢條件區-->
                    <tr>
                        <td>&nbsp;</td>
                        <td align="left">

                            <asp:Panel runat="server" ID="UI_panSearch" DefaultButton="UI_cmdSearch">
                                <table width="40%" border="0" cellspacing="1" cellpadding="0" class="default">
                                    <tr>
                                        <td width="15%">
                                            <asp:Label ID="UI_lblProduct_SerialNo" runat="server" Text="153_Product Serial No"></asp:Label>
                                        </td>
                                        <td align="left" width="25%">:
			                            <asp:TextBox ID="UI_txtProduct_SerialNo" runat="server" Width="120"></asp:TextBox>
                                            <asp:Button ID="UI_cmdSearch" runat="server" Text="004_Search" CssClass="Search" />
                                        </td>
                                    </tr>

                                </table>
                            </asp:Panel>

                        </td>
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
                                <asp:GridView ID="UI_gvReport" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:TemplateField Visible="false">
                                            <HeaderStyle Width="35px" Height="15px" HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="UI_SeqID" runat="server"></asp:Label>
                                                <asp:Label ID="UI_isDetail" runat="server" Text='<%# Eval("isDetail") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--多語系調整 by buck modify 20251201 begin--%>
                                        <asp:BoundField DataField="EXPORT_PARTNO" HeaderText="PartsNo" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="EXPORT_SERIALNO" HeaderText="SerialNo" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="EXPORT_CUSTOMERNAME" HeaderText="CustomerName" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="EXPORT_SHIPPING_TIME" HeaderText="Shippeddate" ItemStyle-HorizontalAlign="Center" HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd}"></asp:BoundField>
                                        <asp:BoundField DataField="EXPORT_WARRANTY_DATE" HeaderText="Warrantydate" ItemStyle-HorizontalAlign="Center" HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd}"></asp:BoundField>
                                        <asp:BoundField DataField="CW_EDATE" HeaderText="CWEWWarrantydate" ItemStyle-HorizontalAlign="Center" HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd}"></asp:BoundField>
                                        <%--多語系調整 by buck modify 20251201 end--%>
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
                            </div>
                            <!--[End]資料列表表單-->
                        </td>
                    </tr>

                    <!-- 需求新增:BI保固 By buck Add 20250902 begin -->
                    <tr>
                        <td width="1%">&nbsp;</td>
                        <td width="99%" align="left" class="default">
                            <!--[Begin]資料列表表單-->
                            <div align="center">
                                <br>
                                <asp:GridView ID="UI_gvWARRANTY_BI" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="False">
                                    <Columns>
                                        <%--多語系調整 by buck modify 20251201 begin--%>
                                        <asp:TemplateField HeaderText="Order_No" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <%--多語系調整 by buck modify 20251201 end--%>
                                            <ItemTemplate>
                                                <asp:HyperLink ID="lnkOrder" runat="server" Text='<%# Eval("Order_No") %>'></asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:BoundField DataField="Order_No" HeaderText="Order No." ItemStyle-HorizontalAlign="Center"></asp:BoundField>--%>
                                        <asp:BoundField DataField="Order_Qty" HeaderText="Order Q'ty" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="BI_Year" HeaderText="Battery Insurance Year" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="Replaceable_Qty" HeaderText="Replaceable Q’ty" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="Qty_replaced" HeaderText="Q’ty replaced" ItemStyle-HorizontalAlign="Center"></asp:BoundField>

                                    </Columns>
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
                    <!-- 需求新增:BI保固 By buck Add 20250902 end-->
                    <tr>
                        <td width="1%">&nbsp;</td>
                        <td width="99%">
                            <!--[Begin]資料列表表單-->
                            <div align="center">
                                <br>

                                <asp:GridView ID="GridView_EXPORT_axmt410_axmt400" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:BoundField DataField="Project_No" HeaderText="專案編號" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="Project_Qty" HeaderText="專案數量" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="Order_No" HeaderText="訂單編號" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="Order_Qty" HeaderText="訂單數量" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="Total_Loss_Insurance" HeaderText="全損保險" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="Replaceable_quantity" HeaderText="可更換數量" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="Quantity_replaced" HeaderText="已更換數量" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                    </Columns>
                                    <EmptyDataRowStyle HorizontalAlign="Center" />

                                    <HeaderStyle CssClass="Text_Head" />
                                    <RowStyle CssClass="TR_1" />
                                    <AlternatingRowStyle CssClass="ListRowEven" />
                                    <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Center" />
                                </asp:GridView>
                                <br />

                                <asp:Panel ID="total_loss_Panel_Out" runat="server" Visible="false">
                                    <asp:Panel ID="total_loss_Panel" runat="server">
                                        <table width="95%" border="1" cellspacing="0" cellpadding="0" class=" box_table" style="visibility: hidden;">
                                            <tr>
                                                <td>出貨數量 </td>
                                                <td>目前累計維修數量 </td>
                                                <td>維修比率</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Shipping_Lab" runat="server" Text=""></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Repair_Lab" runat="server" Text=""></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Exceed_Lab" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>

                                    <br>
                                    <br>
                                    <table width="95%" border="1" cellspacing="0" cellpadding="0" class=" box_table">
                                        <tr class="Text_Head">
                                            <td width="30px">
                                                <asp:Label ID="Warranty_TypeSetting_add_Label3" runat="server" Text="Seq"></asp:Label>
                                            </td>
                                            <td width="30%">
                                                <asp:Label ID="Warranty_TypeSetting_add_Label4" runat="server" Text="Warranty Content"></asp:Label>
                                            </td>
                                            <td width="20%">
                                                <asp:Label ID="Warranty_TypeSetting_add_Label5" runat="server" Text="Warranty Rule"></asp:Label>
                                            </td>
                                        </tr>
                                        <asp:DataList ID="Warranty_TypeSetting_add_lstSpecs" runat="server" Width="100%" HorizontalAlign="left"
                                            CellPadding="0" CellSpacing="0" RepeatLayout="Flow" RepeatColumns="1000000">
                                            <ItemTemplate>
                                                <tr>
                                                    <td style="text-align: center">
                                                        <%# (Container.ItemIndex + 1).ToString("00") + "." %>
                                                        <asp:Label ID="lstSpeclblSeq" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "WAP_SEQ")%>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lstSpeclblPart" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "WAP_NAME")%>'></asp:Label>
                                                        <%# DataBinder.Eval(Container.DataItem, "WAP_NAME")%>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="lstSpectxtRule" runat="server" Width="98%" Text='<%# DataBinder.Eval(Container.DataItem, "WAP_RULE")%>'></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </table>
                                    <br>
                                    <br>
                                </asp:Panel>

                                <!-- [Begin] RMAQuoting搬動到產品序號保固頁面 by Buck 20260310-->
                                <tr>
                                    <td width="1%">&nbsp;</td>
                                    <td width="99%" align="left" class="default">
                                        <uc11:ucwarrantyview ID="UcWarrantyView" runat="server" />
                                    </td>
                                </tr>
                                <!-- [End] RMAQuoting搬動到產品序號保固頁面 by Buck 20260310-->

                                <tr>
                                    <td width="1%">&nbsp;</td>
                                    <td width="99%" align="left" class="default">
                                        <!--[Begin]資料列表表單-->
                                        <div align="center">
                                            <table width="100%" border="1" cellspacing="0" cellpadding="0" class=" box_table">
                                                <tr class="Text_Head">
                                                    <td width="5%">
                                                        <asp:Label ID="lblPartSeq" runat="server" Text="#"></asp:Label>
                                                    </td>
                                                    <td width="15%">
                                                        <asp:Label ID="lblPartPO" runat="server" Text="PO Date"></asp:Label>
                                                    </td>
                                                    <td width="20%">
                                                        <asp:Label ID="lblPartContent" runat="server" Text="Content"></asp:Label>
                                                    </td>
                                                    <td width="15%">
                                                        <asp:Label ID="lblPartMonth" runat="server" Text="Month"></asp:Label>
                                                    </td>
                                                    <td width="15%">
                                                        <asp:Label ID="lblPartExtra" runat="server" Text="Extra"></asp:Label>
                                                    </td>
                                                    <td width="15%">
                                                        <asp:Label ID="lblPartMemo" runat="server" Text="Memo"></asp:Label>
                                                    </td>
                                                    <td width="15%">
                                                        <asp:Label ID="lblPartEndDate" runat="server" Text="End Date"></asp:Label>
                                                    </td>
                                                </tr>
                                                <asp:DataList ID="lstParts" runat="server" Width="100%" HorizontalAlign="left"
                                                    CellPadding="0" CellSpacing="0" RepeatLayout="Flow" RepeatColumns="1000000">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td style="text-align: center">
                                                                <%# (Container.ItemIndex + 1).ToString("00") + "." %>
                                                            </td>
                                                            <td align="center">
                                                                <%# DataBinder.Eval(Container.DataItem, "PODate")%>
                                                            </td>
                                                            <td>
                                                                <%# DataBinder.Eval(Container.DataItem, "TYPE_NAME")%>
                                                            </td>
                                                            <td align="center">
                                                                <%# DataBinder.Eval(Container.DataItem, "WAP_MON")%>
                                                            </td>
                                                            <td align="center">
                                                                <%# DataBinder.Eval(Container.DataItem, "WAP_EMON")%>
                                                            </td>
                                                            <td align="center">
                                                                <%# DataBinder.Eval(Container.DataItem, "WAP_DESC")%>
                                                            </td>
                                                            <td align="center">
                                                                <font color="red"><%# DataBinder.Eval(Container.DataItem, "WarrEndDate")%></font>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:DataList>
                                            </table>
                                        </div>
                                        <!--[End]資料列表表單-->
                                    </td>
                                </tr>

                                <tr>
                                    <td width="1%">&nbsp;</td>
                                    <td width="99%" align="left" class="default">
                                        <font size="1">
                                            <uc10:UcSDCViewG ID="UcSDCViewG" runat="server" />
                                        </font>

                                    </td>
                                </tr>
                </table>
                <!-- 版本 START -->
                <table id="table3" class="default" bordercolor="#c0c0c0" cellspacing="0" cellpadding="3"
                    width="100%" border="1" bordercolorlight="#FFFFFF">
                    <tr>
                        <td align="left">
                            <asp:CheckBox ID="UI_CheckVer" runat="server" AutoPostBack="true" Visible="true" OnCheckedChanged="UI_CheckVer_CheckedChanged" /><asp:Label runat="server" ID="lblVerChange"></asp:Label>
                        </td>
                    </tr>
                </table>

                <asp:Panel ID="pnlVersion" runat="server">
                    <table id="table2" class="default" bordercolor="#c0c0c0" cellspacing="0" cellpadding="3"
                        width="100%" border="1" bordercolorlight="#FFFFFF">
                        <tr>
                            <td style="width: 10%;" class="default">
                                <asp:Label runat="server" ID="lblColumn"></asp:Label>
                            </td>
                            <td style="width: 15%;" class="default">
                                <asp:Label runat="server" ID="lblVerName"></asp:Label>
                            </td>
                            <td style="width: 37%;" class="default">
                                <asp:Label runat="server" ID="lblVerBefore"></asp:Label>
                            </td>
                            <td class="default">
                                <asp:Label runat="server" ID="lblVerAfter"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="default" align="left">
                                <asp:Label runat="server" ID="lblC01"></asp:Label></td>
                            <td class="default" align="left">
                                <asp:Label ID="lblA01" runat="Server"></asp:Label></td>
                            <td align="left">
                                <asp:TextBox ID="txtA01" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                &nbsp;<asp:Label ID="lblOA01" runat="Server"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtQA01" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                &nbsp;<asp:Label ID="lblNA01" runat="Server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="default" align="left">
                                <asp:Label runat="server" ID="lblC02"></asp:Label></td>
                            <td class="default" align="left">
                                <asp:Label ID="lblA02" runat="Server"></asp:Label></td>
                            <td align="left">
                                <asp:TextBox ID="txtA02" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                &nbsp;<asp:Label ID="lblOA02" runat="Server"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtQA02" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                &nbsp;<asp:Label ID="lblNA02" runat="Server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="default" align="left">
                                <asp:Label runat="server" ID="lblC03"></asp:Label></td>
                            <td class="default" align="left">
                                <asp:Label ID="lblA03" runat="Server"></asp:Label></td>
                            <td align="left">
                                <asp:TextBox ID="txtA03" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                &nbsp;<asp:Label ID="lblOA03" runat="Server"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtQA03" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                &nbsp;<asp:Label ID="lblNA03" runat="Server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="default" align="left">
                                <asp:Label runat="server" ID="lblC04"></asp:Label></td>
                            <td class="default" align="left">
                                <asp:Label ID="lblA04" runat="Server"></asp:Label></td>
                            <td align="left">
                                <asp:TextBox ID="txtA04" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                &nbsp;<asp:Label ID="lblOA04" runat="Server"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtQA04" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                &nbsp;<asp:Label ID="lblNA04" runat="Server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="default" align="left">
                                <asp:Label runat="server" ID="lblC05"></asp:Label></td>
                            <td class="default" align="left">
                                <asp:Label ID="lblA05" runat="Server"></asp:Label></td>
                            <td align="left">
                                <asp:TextBox ID="txtA05" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                &nbsp;<asp:Label ID="lblOA05" runat="Server"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtQA05" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                &nbsp;<asp:Label ID="lblNA05" runat="Server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="default" align="left">
                                <asp:Label runat="server" ID="lblC06"></asp:Label></td>
                            <td class="default" align="left">
                                <asp:Label ID="lblA06" runat="Server"></asp:Label></td>
                            <td align="left">
                                <asp:TextBox ID="txtA06" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                &nbsp;<asp:Label ID="lblOA06" runat="Server"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtQA06" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                &nbsp;<asp:Label ID="lblNA06" runat="Server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="default" align="left">
                                <asp:Label runat="server" ID="lblC07"></asp:Label></td>
                            <td class="default" align="left">
                                <asp:Label ID="lblA07" runat="Server"></asp:Label></td>
                            <td align="left">
                                <asp:TextBox ID="txtA07" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                &nbsp;<asp:Label ID="lblOA07" runat="Server"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtQA07" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                &nbsp;<asp:Label ID="lblNA07" runat="Server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="default" align="left">
                                <asp:Label runat="server" ID="lblC08"></asp:Label></td>
                            <td class="default" align="left">
                                <asp:Label ID="lblA08" runat="Server"></asp:Label></td>
                            <td align="left">
                                <asp:TextBox ID="txtA08" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                &nbsp;<asp:Label ID="lblOA08" runat="Server"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtQA08" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                &nbsp;<asp:Label ID="lblNA08" runat="Server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="default" align="left">
                                <asp:Label runat="server" ID="lblC09"></asp:Label></td>
                            <td class="default" align="left">
                                <asp:Label ID="lblA09" runat="Server"></asp:Label></td>
                            <td align="left">
                                <asp:TextBox ID="txtA09" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                &nbsp;<asp:Label ID="lblOA09" runat="Server"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtQA09" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                &nbsp;<asp:Label ID="lblNA09" runat="Server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="default" align="left">
                                <asp:Label runat="server" ID="lblC10"></asp:Label></td>
                            <td class="default" align="left">
                                <asp:Label ID="lblA10" runat="Server"></asp:Label></td>
                            <td align="left">
                                <asp:TextBox ID="txtA10" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                &nbsp;<asp:Label ID="lblOA10" runat="Server"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtQA10" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                &nbsp;<asp:Label ID="lblNA10" runat="Server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="default" align="left">
                                <asp:Label runat="server" ID="lblC11"></asp:Label></td>
                            <td class="default" align="left">
                                <asp:Label ID="lblA11" runat="Server"></asp:Label></td>
                            <td align="left">
                                <asp:TextBox ID="txtA11" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                &nbsp;<asp:Label ID="lblOA11" runat="Server"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtQA11" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                &nbsp;<asp:Label ID="lblNA11" runat="Server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="default" align="left">
                                <asp:Label runat="server" ID="lblC12"></asp:Label></td>
                            <td class="default" align="left">
                                <asp:Label ID="lblA12" runat="Server"></asp:Label></td>
                            <td align="left">
                                <asp:TextBox ID="txtA12" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                &nbsp;<asp:Label ID="lblOA12" runat="Server"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtQA12" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                &nbsp;<asp:Label ID="lblNA12" runat="Server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="default02" align="left">
                                <asp:Label runat="server" ID="lblC13"></asp:Label></td>
                            <td class="default02" align="left">
                                <asp:Label ID="lblB01" runat="Server"></asp:Label></td>
                            <td align="left">
                                <asp:TextBox ID="txtB01" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                &nbsp;<asp:Label ID="lblOB01" runat="Server"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtQB01" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                &nbsp;<asp:Label ID="lblNB01" runat="Server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="default02" align="left">
                                <asp:Label runat="server" ID="lblC14"></asp:Label></td>
                            <td class="default02" align="left">
                                <asp:Label ID="lblB02" runat="Server"></asp:Label></td>
                            <td align="left">
                                <asp:TextBox ID="txtB02" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                &nbsp;<asp:Label ID="lblOB02" runat="Server"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtQB02" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                &nbsp;<asp:Label ID="lblNB02" runat="Server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="default02" align="left">
                                <asp:Label runat="server" ID="lblC15"></asp:Label></td>
                            <td class="default02" align="left">
                                <asp:Label ID="lblB03" runat="Server"></asp:Label></td>
                            <td align="left">
                                <asp:TextBox ID="txtB03" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                &nbsp;<asp:Label ID="lblOB03" runat="Server"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtQB03" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                &nbsp;<asp:Label ID="lblNB03" runat="Server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="default02" align="left">
                                <asp:Label runat="server" ID="lblC16"></asp:Label></td>
                            <td class="default02" align="left">
                                <asp:Label ID="lblB04" runat="Server"></asp:Label></td>
                            <td align="left">
                                <asp:TextBox ID="txtB04" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                &nbsp;<asp:Label ID="lblOB04" runat="Server"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtQB04" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                &nbsp;<asp:Label ID="lblNB04" runat="Server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="default02" align="left">
                                <asp:Label runat="server" ID="lblC17"></asp:Label></td>
                            <td class="default02" align="left">
                                <asp:Label ID="lblB05" runat="Server"></asp:Label></td>
                            <td align="left">
                                <asp:TextBox ID="txtB05" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                &nbsp;<asp:Label ID="lblOB05" runat="Server"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtQB05" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                &nbsp;<asp:Label ID="lblNB05" runat="Server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="default02" align="left">
                                <asp:Label runat="server" ID="lblC18"></asp:Label></td>
                            <td class="default02" align="left">
                                <asp:Label ID="lblB06" runat="Server"></asp:Label></td>
                            <td align="left">
                                <asp:TextBox ID="txtB06" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                &nbsp;<asp:Label ID="lblOB06" runat="Server"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtQB06" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                &nbsp;<asp:Label ID="lblNB06" runat="Server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <!-- 版本 END -->
            </td>
        </tr>
    </table>

</asp:Content>



