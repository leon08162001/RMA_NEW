<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Report4_Search.aspx.vb" Inherits="Report4_Search" Title="Untitled Page" %>

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
                            <asp:Label ID="UI_lblTittle" runat="server" Text="004_RMA Detail Report" CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
                        </td>
                        <td width="3%">&nbsp;</td>
                    </tr>
                    <!--[End]Tittle-->
                    <!--[Begin]資料查詢條件區-->
                    <tr>
                        <td>&nbsp;</td>
                        <td align="left">

                            <asp:Panel runat="server" ID="UI_panSearch" DefaultButton="UI_cmdSearch">
                                <table border="0" cellspacing="1" cellpadding="0" class="default" style="width: 972px">
                                    <tr>
                                        <td width="15%">
                                            <asp:Label ID="UI_lblModelNo" runat="server" Text="101_Model No"></asp:Label>
                                        </td>
                                        <td width="35%">:
			                            <asp:TextBox ID="UI_txtModelNo" runat="server" Width="120"></asp:TextBox>
                                        </td>
                                        <td width="12%" align="right">
                                            <asp:Label ID="UI_lblCompanyName" runat="server" Text="100_Company Name"></asp:Label>
                                        </td>
                                        <td width="30%" align="left">
                                            <asp:Label runat="server" ID="UI_lblCompanyName_Tag">:</asp:Label>
                                            <asp:TextBox ID="UI_txtCompanyName" runat="server" Width="120"></asp:TextBox>
                                        </td>
                                        <td width="12%" align="right">
                                            <asp:Label ID="UI_lblRmaNo" runat="server" Text="100_RMA_NO"></asp:Label>
                                            <asp:Label runat="server" ID="UI_lblRmaNo_Tag">:</asp:Label>
                                        </td>
                                        <td width="30%" align="left">
                                            <asp:TextBox ID="UI_txtRmaNo" runat="server" Width="150"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="UI_lblRepairCenter" runat="server" Text="102_Repair Center"></asp:Label>
                                        </td>
                                        <td>:
			                            <asp:DropDownList ID="UI_cboRepairCenter" runat="server"></asp:DropDownList>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="UI_lblWarranty" runat="server" Text="103_Warranty"></asp:Label>
                                        </td>
                                        <td align="left">:
			                            <asp:DropDownList ID="UI_cboWarranty" runat="server"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="UI_lblStatus" runat="server" Text="104_Status"></asp:Label>
                                        </td>
                                        <td align="left" colspan="3">:
			                            <asp:DropDownList ID="UI_cboStatus" runat="server"></asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <asp:Label ID="UI_lblDurationDate" runat="server" Text="105_Duration"></asp:Label>
                                        </td>
                                        <td align="left" colspan="3">:
			                            <asp:DropDownList ID="UI_cboBYear" runat="server"></asp:DropDownList>
                                            <asp:DropDownList ID="UI_cboBMonth" runat="server"></asp:DropDownList>
                                            <asp:DropDownList ID="UI_cboBDay" runat="server"></asp:DropDownList>&nbsp;~&nbsp;
                                        <asp:DropDownList ID="UI_cboEYear" runat="server"></asp:DropDownList>
                                            <asp:DropDownList ID="UI_cboEMonth" runat="server"></asp:DropDownList>
                                            <asp:DropDownList ID="UI_cboEDay" runat="server"></asp:DropDownList>
                                            <%--<asp:Button ID="UI_cmdSearch" runat="server" Text="004_Search" CssClass="Search" style="height: 18px" />--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="UI_lblShippingDate" runat="server" Text="106_Shipping Date"></asp:Label>
                                        </td>
                                        <td align="left" colspan="3">:
			                            <asp:DropDownList ID="UI_cboSDBYear" runat="server"></asp:DropDownList>
                                            <asp:DropDownList ID="UI_cboSDBMonth" runat="server"></asp:DropDownList>
                                            <asp:DropDownList ID="UI_cboSDBDay" runat="server"></asp:DropDownList>&nbsp;~&nbsp;
                                        <asp:DropDownList ID="UI_cboSDEYear" runat="server"></asp:DropDownList>
                                            <asp:DropDownList ID="UI_cboSDEMonth" runat="server"></asp:DropDownList>
                                            <asp:DropDownList ID="UI_cboSDEDay" runat="server"></asp:DropDownList>
                                            <asp:Button ID="UI_cmdSearch" runat="server" Text="004_Search" CssClass="Search" Style="height: 18px" />
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
                <table runat="server" id="tbReport" border="0" width="2900px" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="1%">&nbsp;</td>
                        <td width="99%" align="left" class="default">
                            <!--[Begin]資料列表表單-->
                            <div align="center">
                                <asp:GridView ID="UI_dvReport" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="True" PagerSettings-Mode="Numeric">
                                    <Columns>
                                        <%--<asp:BoundField DataField="RequestMonth" HeaderText="197_Request Month" />--%>
                                        <asp:TemplateField>
                                            <HeaderStyle Width="35px" Height="15px" HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="UI_SeqID" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="RMA_COMPNO" HeaderText="200_Repair Centre" />
                                        <asp:BoundField DataField="COUNTRY_NAME" HeaderText="201_Country Name" />

                                        <asp:BoundField DataField="RMA_NO" HeaderText="106_RMA No." HeaderStyle-Width="155px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="155px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RequestDate" HeaderText="107_Request Date" HeaderStyle-Width="76px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="76px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RequestYear" HeaderText="196_Request Year" HeaderStyle-Width="76px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="76px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RequestMonth" HeaderText="197_Request Month" HeaderStyle-Width="76px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="76px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CU_NAME" HeaderText="108_Company	Applicant" HeaderStyle-Width="76px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="76px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>

                                        <asp:BoundField DataField="RMAD_SERIALNO" HeaderText="159_Serial No" HeaderStyle-Width="76px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="76px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RMAD_MODELNO" HeaderText="109_Model" HeaderStyle-Width="76px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="76px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RMAD_PRODUCTDESC" HeaderText="110_Product Desc." HeaderStyle-Width="162px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="162px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RMAR_PROBLEMDESC" HeaderText="178_Problem Desc." HeaderStyle-Width="162px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="162px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RMAR_REPAIRDESC" HeaderText="182_Repair Desc." HeaderStyle-Width="162px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="162px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>


                                        <asp:BoundField DataField="FAR_REASON" HeaderText="111_Fail Reason" HeaderStyle-Width="76px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="76px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="EXPORT_SHIPPING_DATE" HeaderText="130_EXPORT_SHIPPING_DATE" HeaderStyle-Width="77px" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:yyyy/MM/dd}" HtmlEncode="false">
                                            <HeaderStyle Width="77px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Warranty" HeaderText="112_Warranty" HeaderStyle-Width="76px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="76px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="sIsWarranty" HeaderText="183_isWarranty" HeaderStyle-Width="76px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="76px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="sRMAD_ISCW" HeaderText="EW/CW" HeaderStyle-Width="76px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="76px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IMPROPERUSAGE_Text" HeaderText="184_IMPROPER USAGE" HeaderStyle-Width="76px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="76px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>

                                        <asp:BoundField DataField="sRMARQ_ASSIGLABORCOST" HeaderText="113_Labor Amount" HeaderStyle-Width="76px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="76px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="sRMARQ_ASSIGMATERIALCOST" HeaderText="114_Material Amount" HeaderStyle-Width="76px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="76px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>

                                        <asp:BoundField DataField="sRMACQSN_DISCOUNTAMOUNT" HeaderText="194_Sales Quoted Amount" HeaderStyle-Width="76px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="76px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>

                                        <asp:BoundField DataField="sRMASQ_LABORCOST" HeaderText="115_Labor Amount-Sales" HeaderStyle-Width="76px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="76px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="sRMASQ_MATERIALCOST" HeaderText="116_Material Amount-Sales" HeaderStyle-Width="76px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="76px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="sRMASQ_QUOTE" HeaderText="117_Total Amount-Sales" HeaderStyle-Width="76px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="76px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>

                                        <asp:BoundField DataField="sRMACQ_DISCOUNT" HeaderText="195_Extra Discount for RMA" HeaderStyle-Width="76px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="76px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>


                                        <asp:BoundField DataField="sRMAR_LABORHOUR" HeaderText="130_Man hour" HeaderStyle-Width="77px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="77px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="sRMAR_LABORPRICE" HeaderText="131_Manpower Price" HeaderStyle-Width="77px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="77px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="sRMAR_MATERIALCOST" HeaderText="132_Part’s Price" HeaderStyle-Width="77px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="77px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="sRMAR_QUOTE" HeaderText="133_Total Amount" HeaderStyle-Width="77px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="77px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>

                                        <asp:BoundField DataField="sRMARSD_LABORCOST" HeaderText="134_Estimated Manpower" HeaderStyle-Width="77px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="77px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="sRMARSD_MATERIALCOST" HeaderText="185_Estimated Part's" HeaderStyle-Width="77px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="77px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="sRMARSD_QUOTE" HeaderText="135_Estimated Amount" HeaderStyle-Width="77px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="77px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>


                                        <asp:BoundField DataField="COMP_NAME" HeaderText="118_Repair Center" HeaderStyle-Width="77px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="77px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Status" HeaderText="119_Status" HeaderStyle-Width="77px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="77px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Received_Name" HeaderText="120_Receiver" HeaderStyle-Width="77px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="77px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Received_Date" HeaderText="121_Received Date" HeaderStyle-Width="77px" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:yyyy/MM/dd HH:mm:ss}" HtmlEncode="false">
                                            <HeaderStyle Width="77px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>

                                        <asp:BoundField DataField="Repaired_Name" HeaderText="122_Repairer" HeaderStyle-Width="77px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="77px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RepairQuoted_Date" HeaderText="123_Quoted Date" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:yyyy/MM/dd HH:mm:ss}" HtmlEncode="false">
                                            <HeaderStyle Width="80px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Repaired_Date" HeaderText="124_Repaired Date" HeaderStyle-Width="77px" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:yyyy/MM/dd HH:mm:ss}" HtmlEncode="false">
                                            <HeaderStyle Width="77px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>

                                        <asp:BoundField DataField="Sales_Name" HeaderText="125_Sales" HeaderStyle-Width="77px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="77px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Sales_Date" HeaderText="126_Confirmed Date" HeaderStyle-Width="77px" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:yyyy/MM/dd HH:mm:ss}" HtmlEncode="false">
                                            <HeaderStyle Width="77px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Client_Date" HeaderText="127_Customer Confirm Date" HeaderStyle-Width="77px" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:yyyy/MM/dd HH:mm:ss}" HtmlEncode="false">
                                            <HeaderStyle Width="77px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>

                                        <asp:BoundField DataField="NoticedDate" HeaderText="128_Noticed Date" HeaderStyle-Width="77px" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:yyyy/MM/dd HH:mm:ss}" HtmlEncode="false">
                                            <HeaderStyle Width="77px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ShippingDate" HeaderText="129_Shipping Date" HeaderStyle-Width="77px" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:yyyy/MM/dd HH:mm:ss}" HtmlEncode="false">
                                            <HeaderStyle Width="77px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ShippingYear" HeaderText="130_Shipping Year" HeaderStyle-Width="77px" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:yyyy/MM/dd HH:mm:ss}" HtmlEncode="false">
                                            <HeaderStyle Width="77px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ShippingMonth" HeaderText="131_Shipping Month" HeaderStyle-Width="77px" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:yyyy/MM/dd HH:mm:ss}" HtmlEncode="false">
                                            <HeaderStyle Width="77px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="EXPORT_PARTNO" HeaderText="132_EXPORT_PARTNO" HeaderStyle-Width="77px" ItemStyle-HorizontalAlign="Center" HtmlEncode="false">
                                            <HeaderStyle Width="77px"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>

                                    </Columns>

                                    <EmptyDataTemplate>
                                        <asp:Label runat="server" ID="UI_NotFound" Text='<%# Session("_isShowNotFound").tostring()%>'></asp:Label>
                                    </EmptyDataTemplate>
                                    <EmptyDataRowStyle HorizontalAlign="Left" />

                                    <HeaderStyle CssClass="Text_Head" />
                                    <RowStyle CssClass="TR_1" />
                                    <AlternatingRowStyle CssClass="ListRowEven" />
                                    <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Left" />
                                </asp:GridView>
                            </div>
                            <!--[End]資料列表表單-->
                        </td>
                    </tr>

                    <tr>
                        <td colspan="2" align="left">
                            <p style="margin-top: 10px; padding-left: 50px">
                                <asp:Button ID="UI_cmdExport" runat="server" Text="076_Export" CssClass="Confirm" />
                            </p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="left" background="Images/pic_23.gif">&nbsp;</td>
        </tr>
    </table>

</asp:Content>

