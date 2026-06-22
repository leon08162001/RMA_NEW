<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Client_FlowCase01_Worklist_Item_New.aspx.vb" Inherits="Client_FlowCase01_Worklist_Item_New" %>

<%@ Register Src="ascx/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>
<%@ Register Src="ascx/ucClientDetail.ascx" TagName="ucClientDetail" TagPrefix="uc1" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="css/system.css" rel="stylesheet" />
    <link href="css/commonStyle.css" rel="stylesheet" />
    <%--    <script>    		
        function Open_Client_FlowCase01_Worklist_Item_New() {
            alert("第一次能成功 只是運氣好");
            location.reload();
        }
    </script>--%>

    <style>
        .erma-components-table {
            border: 1px solid #D8D8D8;
            height: 60vh;
            width: 100%;
            overflow: auto;
            background: #fff;
            box-sizing: border-box;
            display: inline-block;
            overflow: auto;
            /* padding:15px; */
        }

            .erma-components-table th {
                border-bottom: 1px solid #000000;
                color: #000000;
                /*margin:1px;
			padding:1px;*/
            }

        .erma-button-next-div {
            border: 1px solid black;
            background: #fff;
            color: black;
            border-radius: 5px;
            margin-left: 5px;
            cursor: pointer;
        }

            .erma-button-next-div:hover {
                border: 1px solid #496FF2;
                background: #496FF2;
                color: #ffffff;
                border-radius: 5px;
                margin-left: 5px;
                cursor: pointer;
            }

        #UpdatePanel1 .erma-window-quotation-background .erma-warrantyRepair-content {
            width: auto;
        }

        #UpdatePanel1 .erma-quotation-box {
        }

        #UpdatePanel1 .erma-buttons-div-leftRight {
            display: flex;
            justify-content: space-between;
            vertical-align: middle;
            align-items: center;
        }

        #UpdatePanel1 .erma-buttons-div-Detail {
            margin-top: 20px;
            margin-bottom: 5px;
        }

        .UI_totalLabel {
            margin-right: 5px;
        }

        @media screen and (max-width: 1080px) {
            * {
                font-size: 12px;
            }

            .erma-components-table {
                height: 45vh;
            }
        }

        @media screen and (max-width: 920px) {
            #UpdatePanel1 .erma-components-table {
                height: 40vh;
            }
        }

        @media screen and (max-width: 900px) {
            #UpdatePanel1 .erma-components-table {
                height: 37vh;
            }
        }
    </style>

    <script type="text/javascript">
        //.parent.UI_Add_RMA_panel_iframe_RMA_Small_Function();
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="erma-window-quotation-background">
                    <div class="erma-quotation-box">
                        <div class="erma-box-div">
                            <asp:Label ID="UI_lblTittle" runat="server" Text="067_Wait for Processing"></asp:Label>
                            <div class="erma-image-cancel" onclick="window.parent.Open_Client_FlowCase01_Worklist();">
                                <asp:ImageButton ID="Deatail_Cancel_Btn" runat="server" ImageUrl="../images/cancel.svg" />
                            </div>
                        </div>
                        <div class="erma-quotation-topInfo">
                            <table border="0" class="table" cellspacing="0" cellpadding="0">
                                <!--[End]Tittle-->
                                <!--[Begin]資料查詢條件區-->
                                <tr>
                                    <td align="left">
                                        <table class="" border="0" cellspacing="1" cellpadding="0">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="UI_lblRMANo" runat="server" Text="029_RMA No." CssClass="erma-txt"></asp:Label>&nbsp;:&nbsp;</td>
                                                <td>
                                                    <asp:Label ID="UI_RMANo" runat="server"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="UI_lblRequestDate" runat="server" Text="033_Request Date" CssClass="erma-txt"></asp:Label>&nbsp;:&nbsp;</td>
                                                <td>
                                                    <asp:Label ID="UI_RequestDate" runat="server"></asp:Label></td>
                                            </tr>

                                            <tr>
                                                <td>
                                                    <asp:Label ID="UI_lblRepairCenter" runat="server" Text="009_Repair Center" CssClass="erma-txt"></asp:Label>&nbsp;:&nbsp;</td>
                                                <td>
                                                    <asp:Label ID="UI_RepairCenter" runat="server"></asp:Label></td>

                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="UI_lblApplicant" runat="server" Text="006_Applicant" CssClass="erma-txt"></asp:Label>&nbsp;:&nbsp;</td>
                                                <td>
                                                    <asp:Label ID="UI_Applicant" runat="server"></asp:Label></td>

                                            </tr>
                                        </table>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <!--[End]資料查詢條件區-->
                            </table>
                        </div>
                        <div class="erma-warrantyRepair-content">
                            <asp:Label ID="UI_lblRequestedTittle" runat="server" Text="034_Requested Information"></asp:Label>
                            <div>
                                <%--零件 開始--%>
                                <asp:DataList ID="UI_dvRepairDetail" runat="server" ExtractTemplateRows="true" CellSpacing="0" CellPadding="3" border="0" CssClass="erma-components-table">
                                    <HeaderTemplate>
                                        <asp:Table ID="oTableHeader" runat="server">
                                            <asp:TableHeaderRow>
                                                <asp:TableHeaderCell Width="2%">&nbsp;</asp:TableHeaderCell>
                                                <asp:TableHeaderCell Width="3%" Visible="false">
                                                    <asp:Label ID="lblWaive" runat="server" Text="405_Waive"></asp:Label>
                                                </asp:TableHeaderCell>
                                                <asp:TableHeaderCell Width="5%">
                                                    <asp:Label ID="lblOption" runat="server" Text="406_Option"></asp:Label>
                                                </asp:TableHeaderCell>
                                                <asp:TableHeaderCell Width="10%">
                                                    <asp:Label ID="lblHPart" runat="server" Text="083_Part'sNo"></asp:Label>
                                                </asp:TableHeaderCell>


                                                <asp:TableHeaderCell Width="28%">
                                                    <asp:Label ID="lblHDescription" runat="server" Text="099_Description"></asp:Label>
                                                </asp:TableHeaderCell>
                                                <asp:TableHeaderCell Width="10%">
                                                    <asp:Label ID="lblHLocation" runat="server" Text="100_SMT Location"></asp:Label>
                                                </asp:TableHeaderCell>
                                                <asp:TableHeaderCell Width="11%">
                                                    <asp:Label ID="lblHImproper" runat="server" Text="101_Improper Usage"></asp:Label>
                                                </asp:TableHeaderCell>
                                                <asp:TableHeaderCell Width="10%">
                                                    <asp:Label ID="lblHQty" runat="server" Text="103_Qty"></asp:Label>
                                                </asp:TableHeaderCell>
                                                <asp:TableHeaderCell Width="10%">
                                                    <asp:Label ID="lblHPrice" runat="server" Text="104_Price"></asp:Label>
                                                </asp:TableHeaderCell>
                                            </asp:TableHeaderRow>
                                        </asp:Table>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <asp:Table ID="oTableRow" runat="server">
                                            <asp:TableRow>
                                                <asp:TableCell HorizontalAlign="Center">
                                                    <asp:Label ID="lblSeq" runat="server" Text='<%# Container.ItemIndex + 1 %>'></asp:Label>

                                                    <asp:Label ID="lblNew" runat="server" Text="_New" class="default" Visible="false"></asp:Label>
                                                    <asp:Label ID="lblRMARQDID" runat="server" Text='<%# Eval("RMARQD_ID") %>' class="default" Visible="false"></asp:Label>
                                                    <asp:Label ID="lblRMARQD_RMADID" runat="server" Text='<%# Eval("RMARQD_RMADID") %>' class="default" Visible="false"></asp:Label>

                                                    <asp:Label ID="lblIMPROPERUSAGE" runat="server" Text='<%# Eval("RMARQD_IMPROPERUSAGE") %>' class="default" Visible="false"></asp:Label>
                                                    <asp:Label ID="lblDEFECTIVE" runat="server" Text='<%# Eval("RMARQD_DEFECTIVE") %>' class="default" Visible="false"></asp:Label>
                                                    <asp:TextBox ID="UI_txtMaterialCost" runat="server" Text='<%# Eval("RMARQD_MATERIALCOST") %>' Style="display: none; width: 1px" class="default"></asp:TextBox>
                                                    <asp:Label ID="lblRMARQD_CURRENCYCODE" runat="server" Text='<%# Eval("RMARQD_CURRENCYCODE") %>' class="default" Visible="false"></asp:Label>
                                                    <asp:Label ID="lblRMARQD_CURRENCYRATE" runat="server" Text='<%# Eval("RMARQD_CURRENCYRATE") %>' class="default" Visible="false"></asp:Label>

                                                    <asp:Label ID="lblRMARQD_ASSIGECURRENCYCODE" runat="server" Text='<%# Eval("RMARQD_ASSIGECURRENCYCODE") %>' class="default" Visible="false"></asp:Label>
                                                    <asp:Label ID="lblRMARQD_ASSIGECURRENCYRATE" runat="server" Text='<%# Eval("RMARQD_ASSIGECURRENCYRATE") %>' class="default" Visible="false"></asp:Label>

                                                    <asp:Label ID="UI_RMARQD_WAIVE" runat="server" Text='<%# Eval("RMARQD_WAIVE") %>' class="default" Visible="false"></asp:Label>
                                                    <asp:Label ID="UI_RMARQD_ACC" runat="server" Text='<%# Eval("RMARQD_ACC") %>' class="default" Visible="false"></asp:Label>
                                                    <asp:Label ID="UI_RMARQD_OPTION" runat="server" Text='<%# Eval("RMARQD_OPTION") %>' class="default" Visible="false"></asp:Label>
                                                    <asp:Label ID="UI_RMARQD_OPTIONCLIENT" runat="server" Text='<%# Eval("RMARQD_OPTIONCLIENT") %>' class="default" Visible="false"></asp:Label>

                                                    <asp:Label ID="lblRMARQD_AD" runat="server" Text='<%# Eval("RMARQD_AD") %>' class="default" Visible="false"></asp:Label>
                                                    <asp:Label ID="lblRMARQD_ADNAME" runat="server" Text='<%# Eval("RMARQD_ADNAME") %>' class="default" Visible="false"></asp:Label>
                                                    <asp:Label ID="lblRMARQD_CSTMP" runat="server" Text='<%# Eval("RMARQD_CSTMP") %>' class="default" Visible="false"></asp:Label>

                                                </asp:TableCell>

                                                <asp:TableCell HorizontalAlign="Center" Visible="false">
                                                    <asp:CheckBox runat="server" ID="chhWaive" Checked='<%# Eval("RMARQD_WAIVE") %>' />
                                                </asp:TableCell>
                                                <asp:TableCell HorizontalAlign="Center">
                                                    <asp:CheckBox runat="server" ID="UI_chkOptionClent" OnCheckedChanged="UI_chkOptionClent_CheckedChanged" AutoPostBack="true" Text='<%# Container.ItemIndex + 1  %>' ForeColor="Transparent" Font-Size="Small" />
                                                </asp:TableCell>

                                                <asp:TableCell HorizontalAlign="Center">
                                                    <asp:Label ID="lblRMARQD_NPARTNO" runat="server" Width="100" MaxLength="13" Text='<%# Eval("RMARQD_NPARTNO") %>' class="default"></asp:Label>

                                                    <asp:Label ID="txtOldPart" runat="server" Width="100" MaxLength="13" Text='<%# Eval("RMARQD_OPARTNO") %>' class="default" Visible="false"></asp:Label>
                                                    <asp:Label ID="txtOldSerial" runat="server" Width="80px" Text='<%# Eval("RMARQD_OSERIALNO")  %>' class="default" Visible="false"></asp:Label>
                                                </asp:TableCell>

                                                <asp:TableCell HorizontalAlign="Center">
                                                    <asp:Label ID="txtDescription" runat="server" Text='<%# Eval("RMARQD_DESC") %>' class="default" Style="width: 98%"></asp:Label>
                                                </asp:TableCell>

                                                <asp:TableCell HorizontalAlign="Center">
                                                    <asp:Label ID="txtLocation" runat="server" Text='<%# Eval("RMARQD_LOCATION") %>' class="default"></asp:Label>
                                                </asp:TableCell>

                                                <asp:TableCell HorizontalAlign="Center">
                                                    <asp:Label ID="UI_Improper" runat="server" class="default"></asp:Label>
                                                </asp:TableCell>

                                                <asp:TableCell HorizontalAlign="Center">
                                                    <asp:Label ID="lblRMARQD_QTY" runat="server" Text='<%# Eval("RMARQD_QTY") %>' class="default"></asp:Label>
                                                </asp:TableCell>

                                                <asp:TableCell>
                                                    <div>

                                                        <asp:Label runat="server" ID="lblRMARQD_PRICE" Text='<%#  GetPrice(Eval("RMARQD_PRICE").ToString().Trim(), Eval("RMARQD_OPTIONCLIENT").ToString().Trim(), Eval("RMARQD_OPTION").ToString().Trim(), Container.ItemIndex.ToString().Trim()) %>' class="default" />
                                                        <asp:Label runat="server" ID="UI_lblRMARQD_PRICE" Text='<%# Eval("RMARQD_PRICE") %>' Font-Strikeout="true" ForeColor="Red" Visible="false" Style="display: inline-block; margin-right: 10px;" />
                                                        <asp:Label runat="server" ID="UI_lblRMARQD_PRICE_Cancel" Text='0' ForeColor="Black" Visible="false" Style="display: inline-block;" />

                                                    </div>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </ItemTemplate>
                                </asp:DataList>
                                <%--零件 結束--%>
                            </div>
                            <div class="erma-warrantyRepair-calc-content">
                                <div class="erma-calc-number">
                                    <img src="images/info-black.svg" />

                                    <asp:Label ID="lbl_Manpower" runat="server" Text="086_Man power" class="default"></asp:Label>:
                                    <asp:Label ID="UI_RMARQ_LABORHOUR" runat="server" Text="0" class="default UI_totalLabel" Font-Bold="true"></asp:Label><asp:Label ID="uiLbl_Parts" runat="server" Text="087_Parts" class="default"></asp:Label><asp:Label ID="uiLbl_Parts_Delimited" runat="server" Text=" :" class="default"></asp:Label><asp:Label ID="UI_RMARQ_MATERIALCOST" runat="server" Text="0" class="default UI_totalLabel" Font-Bold="true"></asp:Label><asp:Label ID="uiLbl_TotalAmountText" runat="server" Text="088_Total Amount" class="default"></asp:Label><asp:Label ID="uiLbl_TotalAmountText_Delimited" runat="server" Text=" :" class="default"></asp:Label><asp:Label ID="UI_RMARQ_QUOTE" runat="server" Text="0" Font-Bold="true" class="default  UI_totalLabel"></asp:Label><asp:Label ID="UI_RMARQ_ID" runat="server" Visible="false"></asp:Label><asp:Label ID="UI_RMARQ_LABORPRICE" runat="server" class="default" Visible="false"></asp:Label><asp:Label ID="UI_RMARQ_CURRENCYRATE" runat="server" class="default" Visible="false"></asp:Label><asp:Label ID="UI_RMARQ_ASSIGECURRENCYRATE" runat="server" class="default" Visible="false"></asp:Label>
                                </div>
                            </div>
                            <div class="erma-buttons-div">
                                <div class="erma-buttons-div-leftRight">
                                    <div class="erma-buttons-div-Detail" style="display: inline-block; float: left;">
                                        <asp:Button ID="UI_btnDetail" CssClass="erma-button-next-div" Text="Repair Detail" runat="server" Width="125" Height="40" />
                                    </div>
                                    <div class="erma-buttons-div" style="display: inline-block; float: right;">
                                        <input id="UI_cmdBack" runat="server" type="button" value="006_back" class="erma-button-back" onclick="window.parent.Open_Client_FlowCase01_Worklist();" style="width: 90px; height: 40px;" />

                                        <asp:Button ID="UI_cmdConfirm" runat="server" Text="_Confirm" CssClass="erma-button-next" OnClientClick="onProgress('Save')" Width="90" Height="40" />
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>


                    <uc2:ucMessage ID="ucMessage" runat="server" />
                    <uc1:ucClientDetail ID="ucClientDetail" runat="server" />
                    <asp:Label ID="UI_lblPreviousPage_RMADID" runat="server" Visible="false"></asp:Label><asp:Label ID="UI_lblPreviousPage_RMAID" runat="server" Visible="false"></asp:Label><asp:Label ID="UI_lblPreviousPage_RMANO" runat="server" Visible="false"></asp:Label><asp:Label ID="UI_lblRMAD_STATUS" runat="server" Visible="false"></asp:Label><asp:Label ID="UI_CU_DISCOUNT_OFF" runat="server" Visible="false"></asp:Label><asp:Label ID="UI_RMAD_ISWARRANTY" runat="server" Visible="false"></asp:Label><asp:Label ID="UI_RMARQ_IMPROPERUSAGE" runat="server" Visible="false"></asp:Label><asp:Label ID="UI_RMAD_ISCW" runat="server" Visible="false"></asp:Label><asp:Label ID="UI_RMA_CUNO" runat="server" Visible="false"></asp:Label>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="ucMessage" />
                <asp:PostBackTrigger ControlID="UI_cmdConfirm" />
            </Triggers>
        </asp:UpdatePanel>

    </form>
</body>
</html>




