<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ucClientDetail.ascx.vb" Inherits="ascx_ucClientDetail" %>

<style>
    .Repair_Detail_001 {
        color: #000;
        text-align: center;
        font-family: Source Sans Pro;
        font-size: 24px;
        font-style: normal;
        line-height: normal;
        top: 5px;
    }

    .table_span {
        text-align: right;
        font-family: Source Sans Pro;
        font-style: normal;
        font-weight: 400;
        line-height: normal;
    }

    .table_span_btn {
        border-radius: 5px;
        border: 1px solid #1C218C;
        background: #1C218C;
        display: inline-flex;
        padding: 9px 25px;
        align-items: flex-start;
        color: white;
    }

    .Modal_ucClient {
        /*background-color:transparent;*/
        background-color: black;
        opacity: 0.001;
    }

    .Modal {
        background-color: black;
        opacity: 0.5;
    }

    .erma-panel-repairWarrantyDetail #table1 td {
        padding: 3px;
        word-break: break-all;
    }

    .erma-center-dgvPurchasing {
        margin-top: 10px;
    }

    .span_ui_dol {
        color: blue;
        text-decoration: underline;
    }

    #table1 tr td.row-name1 {
        width: 23%;
        vertical-align: top;
    }

    #table2 tr td.row-name1 {
        width: 23%;
        vertical-align: top;
    }

    .bottombtn {
        margin-bottom: 20px;
    }


    .SourceSans {
        font-family: 'Source Sans Pro';
        font-size: 14px;
    }
</style>

<asp:Panel ID="UI_panel" CssClass="erma-panel-repairWarrantyDetail" runat="server" Width="700px" Style="display: none; position: absolute; background: #ffffff; border-radius: 10px; height: 90%; overflow-y: scroll;">
    <div style="margin-top: 20px">
        <center>
            <asp:Label ID="UI_lblRepairDetail" runat="server" Text="111_Repair Detail" CssClass="Repair_Detail_001"></asp:Label>
        </center>
    </div>

    <table id="table1" style="margin-top: 20px; display: block; overflow: auto;">
        <tbody style="display: inline-table; width: 100%;">
            <tr>
                <td class="row-name1" style="text-align: right; width: 30%;">
                    <asp:Label ID="UI_lblSerial" runat="server" Text="013_Serial Number" CssClass="table_span"></asp:Label>:
                </td>
                <td>
                    <asp:Label ID="UI_lblSerialText" runat="server"></asp:Label>
                </td>
            </tr>

            <tr>
                <td class="row-name1" style="text-align: right;">
                    <asp:Label ID="UI_lblModel" runat="server" Text="035_Model" CssClass="table_span"></asp:Label>:
                </td>
                <td>
                    <asp:Label ID="UI_lblModelText" runat="server"></asp:Label>
                </td>
            </tr>

            <tr>
                <td class="row-name1" style="text-align: right;">
                    <asp:Label ID="UI_lblFailure" runat="server" Text="023_Failure Reason" CssClass="table_span"></asp:Label>:
                </td>
                <td>
                    <asp:Label ID="UI_lblFailureText" runat="server"></asp:Label>
                </td>
            </tr>

            <tr>
                <td class="row-name1" style="text-align: right;">
                    <asp:Label ID="UI_lblProductDesc" runat="server" Text="196_Product Desc" CssClass="table_span"></asp:Label>:
                </td>
                <td>
                    <asp:Label ID="UI_lblProductDescText" runat="server"></asp:Label>
                </td>
            </tr>

            <tr>
                <td class="row-name1" style="text-align: right;">
                    <asp:Label ID="UI_lblProblemDesc" runat="server" Text="122_Problem Desc" CssClass="table_span"></asp:Label>:
                </td>
                <td>
                    <asp:Label ID="UI_lblProblemDescText" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="row-name1" style="text-align: right;">
                    <asp:Label ID="UI_lblDescription" runat="server" Text="099_Description" CssClass="table_span"></asp:Label>:
                </td>
                <td>
                    <asp:Label ID="UI_lblDescriptionText" runat="server"></asp:Label>
                </td>
            </tr>
            <%-- 需求新增:BI保固 By buck Add 20250902 begin --%>
            <tr ID="UI_trApply_BI" runat="server" >
                <td class="row-name1" style="text-align: right;">
                    <asp:Label ID="UI_lblApply_BI" runat="server" Text="099_Description" CssClass="table_span"></asp:Label>:
                </td>
                <td>
                    <asp:Label ID="UI_lblApply_BIText" runat="server"></asp:Label>
                </td>
            </tr>
            <%-- 需求新增:BI保固 By buck Add 20250902 end --%>
            <tr>
                <td class="row-name1" style="text-align: right;">
                    <asp:Label ID="RMAD_RMANO" runat="server" Text="Standard Battery" CssClass="SourceSans"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="labAmount" runat="server" CssClass="SourceSans"></asp:Label>
                    <asp:Panel ID="Panel1" runat="server" Visible="false">
                        <asp:Label ID="txtRMAD_RMANO_QTY" runat="server" CssClass="SourceSans"></asp:Label>
                        <asp:Label ID="LabRMAD_RMANO_QTY" runat="server" Text="" CssClass="SourceSans"></asp:Label>
                    </asp:Panel>
                </td>
            </tr>

            <tr>
                <td class="row-name1" style="text-align: right;">&nbsp;&nbsp;
                    <asp:Label ID="Insurance_Label" runat="server" Text="Apply Total "></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Apply_Label" runat="server" Text="Loss Insurance:"></asp:Label>
                    <asp:Label ID="UI_Apply_Total_Loss_Insurance" runat="server" Text="No"></asp:Label>
                </td>
            </tr>

            <tr>
                <td class="row-name1" style="text-align: right;">
                    <asp:Label ID="UI_lblCustomerFile" runat="server" Text="123_Customer File" CssClass="table_span"></asp:Label>:
                </td>
                <td>
                    <asp:Label ID="UI_Downloadlbl" runat="server" Text=""></asp:Label>
                    <asp:HyperLink ID="UI_DownloadFile" runat="server" Target="_blank" CssClass="span_ui_dol" Visible="false"></asp:HyperLink>
                </td>
            </tr>

            <tr>
                <td colspan="2">
                    <asp:Button ID="UI_butTarget" Width="0px" Height="0px" runat="server" Style="display: none"></asp:Button>
                    <asp:Button ID="UI_butOK" Width="0px" Height="0px" runat="server" Style="display: none"></asp:Button>
                </td>
            </tr>
        </tbody>
    </table>

    <asp:Panel runat="server" ID="UI_panRepairedQUOTE" Visible="false">

        <asp:Label ID="UI_lblCharge" runat="server" Text="124_Charge"></asp:Label>

        <asp:Label ID="UI_lblLaborCost" runat="server" Text="125_Labor Cost" CssClass="table_span"></asp:Label>:
         
        <asp:Label ID="UI_lblLaborCostText" runat="server" Font-Underline="true"></asp:Label>

        <asp:Label ID="UI_lblMaterialCost" runat="server" Text="126_Material Cost" CssClass="table_span"></asp:Label>

        <asp:Label ID="UI_lblMaterialCostText" runat="server" Font-Underline="true"></asp:Label>

        <asp:Label ID="UI_lblTotalAmount" runat="server" Text="127_Total Amount" CssClass="table_span"></asp:Label>

        <asp:Label ID="UI_lblTotalAmountText" runat="server" Font-Underline="true"></asp:Label>

    </asp:Panel>

    <asp:Panel runat="server" ID="UI_panRepaired" Visible="false">

        <asp:Label ID="UI_lblRepairedTitle" runat="server" Text="202_RMA Repaired" CssClass="table_span"></asp:Label>

        <asp:GridView ID="UI_dvRepairDetail" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric">
            <Columns>
                <asp:TemplateField>
                    <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label ID="SeqID" runat="server" Text='<%# Eval("SeqID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="RMARED_NPARTNO" HeaderText="083_Part's No" HeaderStyle-Width="25%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                <asp:BoundField DataField="RMARED_DESC" HeaderText="099_Description" HeaderStyle-Width="40%" ItemStyle-HorizontalAlign="left"></asp:BoundField>
                <asp:BoundField DataField="RMARED_QTY" HeaderText="103_Qty" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
            </Columns>

            <HeaderStyle CssClass="Text_Head" />
            <RowStyle CssClass="TR_1" />
            <AlternatingRowStyle CssClass="ListRowEven" />
        </asp:GridView>
    </asp:Panel>

    <asp:Panel runat="server" ID="UI_panReport" Visible="false">
        <table id="table2" style="display: block; overflow: auto;">
            <tbody style="display: inline-table; width: 100%;">
                <tr>
                    <td class="row-name1" style="text-align: right; padding: 3px;">
                        <asp:Label ID="UI_lblTittel" runat="server" Text="128_RMA Report Attachment"></asp:Label>:
                    </td>
                    <td>
                        <asp:DataList ID="UI_dvRepairUpload" runat="server">
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td class="row-name1">
                                            <asp:Label ID="UI_SeqID1" runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="UI_lblRepair1" runat="server" Text="129_維修報告-" Visible="false"></asp:Label>
                                            <asp:HyperLink ID="UI_DownloadRepair1" runat="server" Target="_blank" Visible="false" CssClass="span_ui_dol"></asp:HyperLink>
                                            <asp:Label ID="UI_UPLOADFILE1" runat="server" Text='<%# Eval("RMARU_UPLOADFILE_1") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="UI_lblSeqID1" runat="server" Text='<%# Eval("SeqID_1") %>' Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="row-name1">
                                            <asp:Label ID="UI_SeqID2" runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="UI_lblRepair2" runat="server" Text="129_維修報告-" Visible="false"></asp:Label>
                                            <asp:HyperLink ID="UI_DownloadRepair2" runat="server" Target="_blank" Visible="false" CssClass="span_ui_dol"></asp:HyperLink>
                                            <asp:Label ID="UI_UPLOADFILE2" runat="server" Text='<%# Eval("RMARU_UPLOADFILE_2") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="UI_lblSeqID2" runat="server" Text='<%# Eval("SeqID_2") %>' Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="row-name1">
                                            <asp:Label ID="UI_SeqID3" runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="UI_lblRepair3" runat="server" Text="129_維修報告-" Visible="false"></asp:Label>
                                            <asp:HyperLink ID="UI_DownloadRepair3" runat="server" Target="_blank" Visible="false" CssClass="span_ui_dol"></asp:HyperLink>
                                            <asp:Label ID="UI_UPLOADFILE3" runat="server" Text='<%# Eval("RMARU_UPLOADFILE_3") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="UI_lblSeqID3" runat="server" Text='<%# Eval("SeqID_3") %>' Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:DataList>
                    </td>
                </tr>
            </tbody>
        </table>
    </asp:Panel>

    <div style="visibility: hidden;">
        <asp:Label ID="PurchasingRecordsLab" runat="server" Text="Purchasing Records : "></asp:Label>

        <center class="erma-center-dgvPurchasing">
            <asp:GridView ID="dgvPurchasing" runat="server" Cssclass="table" PagerSettings-Mode="Numeric" AllowPaging="true" AutoGenerateColumns="False" border="0"
                CellSpacing="1" CellPadding="0" AllowSorting="true">
                <columns>
                    <asp:TemplateField>
                        <headerstyle width="3%" height="20px" horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Center" cssclass="text9pt" height="25px"></itemstyle>
                        <itemtemplate>
                            <asp:Label ID="UI_SEQID" runat="server"></asp:Label>
                        </itemtemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="SerialNo" HeaderText="SerialNo"
                        HeaderStyle-Width="12%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                    <asp:BoundField DataField="PurchaseDate" HeaderText="PurchaseDate"
                        HeaderStyle-Width="12%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                    <asp:BoundField DataField="WarrantyCode" HeaderText="WarrantyCode"
                        HeaderStyle-Width="12%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                    <asp:BoundField DataField="StartDate" HeaderText="StartDate"
                        HeaderStyle-Width="12%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                    <asp:BoundField DataField="EndDate" HeaderText="EndDate"
                        HeaderStyle-Width="12%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                </columns>
                <emptydatatemplate>
                    <asp:Label runat="server" ID="UI_NotFound" Text='<%# Session("_isShowNotFound").ToString()%>'></asp:Label>
                </emptydatatemplate>
                <emptydatarowstyle horizontalalign="Center" />
                <headerstyle cssclass="Text_Head" />
                <rowstyle cssclass="TR_1" />
                <alternatingrowstyle cssclass="ListRowEven" />
                <pagerstyle backcolor="#C6C3C6" forecolor="Black" horizontalalign="Center" />
            </asp:GridView>
        </center>
    </div>

    <div class="bottombtn">
        <center>
            <asp:Button ID="UI_cmdClose" runat="server" Text="039_Close Window" CssClass="table_span_btn" />
        </center>
    </div>
</asp:Panel>


<ajaxToolkit:ModalPopupExtender ID="ajModalProgress" TargetControlID="UI_butTarget"
    PopupControlID="UI_panel"
    OkControlID="UI_butOK"
    BackgroundCssClass="Modal"
    CancelControlID="UI_cmdClose" runat="server">
</ajaxToolkit:ModalPopupExtender>
