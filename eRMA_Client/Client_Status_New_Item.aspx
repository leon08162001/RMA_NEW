<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Client_Status_New_Item.aspx.vb" Inherits="Client_Status_New_Item" %>

<%@ Register Src="ascx/ucClientDetail.ascx" TagName="ucClientDetail" TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<link rel="stylesheet" href="css/system.css">
<link rel="stylesheet" href="css/commonStyle.css">

<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style>
        * {
            font-size: 14px;
        }

        .RMAD_RMANO_001 {
            min-width: 170px;
        }

        .UI_cmdDetail_Btn {
            background: transparent;
            color: #496FF2;
            border: none;
            border-bottom: 1px solid #496FF2;
            cursor: pointer;
        }

        .erma-input-inputStyle-001 {
            padding: 6px 10px;
            width: 100%;
            box-sizing: border-box;
            border-radius: 5px;
            border: 1px solid #D8D8D8;
            background: #FFF;
            text-align: center;
        }

        .select {
            background: transparent;
            border: none;
            padding-left: 10px;
            width: 120px;
            height: 100%;
        }

        th {
            border-bottom: 1px solid #000000;
            color: #000000;
            margin: 1px;
            padding: 1px;
        }

        a {
            border: none;
            color: #000000;
            text-decoration: none;
        }

        .erma-components-table {
            border: 1px solid #D8D8D8;
            height: 485px;
            width: 1260px;
            overflow: auto;
            background: #fff;
            box-sizing: border-box;
            display: inline-block;
        }

        .btn_a_txt {
            border: none;
            background-color: transparent;
            color: #0c66f5;
        }

        .Modal {
            background-color: black;
            opacity: 0.5;
        }

        .Deatail_Cancel_Btn {
            position: absolute;
            right: 15px;
            top: 15px;
            z-index: 1000;
            background: #FFF;
            border: 0px solid #FFF;
        }

        .erma-button-next-div {
            border: 1px solid black;
            background: #fff;
            color: black;
            border-radius: 5px;
            margin-left: 5px;
            cursor: pointer;
            width: 90px;
            height: 40px;
        }


            .erma-button-next-div:hover {
                border: 1px solid #496FF2;
                background: #496FF2;
                color: #ffffff;
                border-radius: 5px;
                margin-left: 5px;
                cursor: pointer;
                width: 90px;
                height: 40px;
            }

        .erma-client-status-new-content .erma-table-components th {
            width: 9%;
        }

        .erma-client-status-new-content .erma-table-components .erma-status-no {
            width: 5%;
        }

        .erma-table-components th, .erma-table-components td {
            min-width: auto;
            width: auto;
            text-align: left;
        }

        .erma-client-status-new-item-remark {
            width: 90%;
        }
    </style>
</head>
<body style="background-color: #FFF;">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" Class="erma-client-status-new-item" runat="server" RenderMode="Inline" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="erma-client-status-new-item-background">
                    <div class="erma-box-div">
                        <asp:Label ID="UI_lblTittle" runat="server" Text="027_Status Query" CssClass="text_tittle" ForeColor="#326B9B" Visible="false"></asp:Label>
                        <asp:Label ID="UI_lblRequestInformation" runat="server" Text="028_Search for Request Items Information" Font-Bold="false"></asp:Label><br />
                    </div>
                    <div class="erma-client-status-new-item-topInfo">
                        <table>
                            <tr>
                                <td>
                                    <div style="float: right;">
                                        <asp:Label ID="UI_lblRMANo" runat="server" Text="029_RMA No."></asp:Label>&nbsp;:&nbsp;
                                    </div>
                                </td>
                                <td>
                                    <asp:Label ID="UI_RMANo" runat="server"></asp:Label>
                                </td>

                            </tr>
                            <tr>
                                <td>
                                    <div style="float: right;">
                                        <asp:Label ID="UI_lblRequestDate" runat="server" Text="033_Request Date"></asp:Label>&nbsp;:&nbsp;
                                    </div>
                                </td>
                                <td>
                                    <asp:Label ID="UI_RequestDate" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div style="float: right;">
                                        <asp:Label ID="UI_lblRepairCenter" runat="server" Text="009_Repair Center"></asp:Label>&nbsp;:&nbsp;
                                    </div>
                                </td>
                                <td>
                                    <asp:Label ID="UI_RepairCenter" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div style="float: right;">
                                        <asp:Label ID="UI_lblApplicant" runat="server" Text="006_Applicant"></asp:Label>&nbsp;:&nbsp;
                                    </div>
                                </td>
                                <td>
                                    <asp:Label ID="UI_Applicant" runat="server"></asp:Label>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <div style="float: right;">
                                        <asp:Label ID="UI_lblRemark" runat="server" Text="134_Remark"></asp:Label>&nbsp;:&nbsp;
                                    </div>
                                </td>
                                <td class="erma-client-status-new-item-remark">
                                    <asp:Label ID="UI_Remark" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="erma-client-status-new-content">
                        <asp:Label ID="UI_lblRequestedTittle" runat="server" Text="034_Requested Information" Visible="false"></asp:Label>
                        <fieldset class="form_div" valign="top" style="border: none;">
                            <!--[Begin]資料列表表單-->
                            <asp:GridView ID="UI_dvRequest" runat="server" CellPadding="0" CellSpacing="0" border="0" ShowFooter="true" GridLines="None" CssClass="erma-table-components" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderStyle HorizontalAlign="Center" CssClass="erma-status-no"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center" CssClass="text9pt erma-status-no"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="UI_SeqID" runat="server" Text='<%# Eval("SeqID") %>'></asp:Label>
                                            <asp:Label ID="UI_RMADWARRANTY" runat="server" Text='<%# Eval("RMAD_WARRANTY") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="UI_RMADCSTMP" runat="server" Text='<%# Eval("RMAD_CSTMP") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="UI_RMANO" runat="server" Text='<%# Eval("RMAD_RMANO") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="UI_RMADID" runat="server" Text='<%# Eval("RMAD_ID") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="UI_SERIALNO" runat="server" Text='<%# Eval("RMAD_SERIALNO") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="RMAD_SERIALNO" HeaderText="013_Serial Number" ItemStyle-HorizontalAlign="Center"></asp:BoundField>

                                    <asp:TemplateField HeaderText="Clinet product No">
                                        <HeaderStyle CssClass="erma-clinetProduct-no" />
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="UI_QueryCUSTOMER_PRODUCT_NUMBER" Text='<%# QueryCUSTOMER_PRODUCT_NUMBER(Eval("RMAD_ID")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="RMAD_MODELNO" HeaderText="035_Model" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                    <asp:BoundField DataField="WARRANTY" HeaderText="015_Warranty" ItemStyle-HorizontalAlign="Center" HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd}"></asp:BoundField>
                                    <asp:BoundField DataField="FailureReason" HeaderText="023_Failure Reason" ItemStyle-HorizontalAlign="Center" HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd}"></asp:BoundField>

                                    <asp:BoundField DataField="LaborCost" HeaderText="125_Labor Cost" ItemStyle-HorizontalAlign="Center" Visible="false"></asp:BoundField>
                                    <asp:BoundField DataField="MaterialCost" HeaderText="126_Material Cost" ItemStyle-HorizontalAlign="Center" Visible="false"></asp:BoundField>

                                    <asp:BoundField DataField="TotalAmount" HeaderText="127_Total Amount" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                    <asp:BoundField DataField="Status" HeaderText="032_Status" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                    <asp:BoundField DataField="ShippedDate" HeaderText="141_Shipped Date" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                    <asp:BoundField DataField="TrackingNo" HeaderText="140_Tracking No" ItemStyle-HorizontalAlign="Center"></asp:BoundField>

                                    <asp:TemplateField HeaderText="038_Detail">
                                        <HeaderStyle Width="8%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                        <ItemTemplate>

                                            <asp:Button ID="UI_cmdDetail" runat="server" Text="View" CssClass="UI_cmdDetail_Btn" CommandName="cmdDetail" CommandArgument='<%#Me.UI_dvRequest.Rows.Count%>' />
                                            <asp:Button ID="UI_Test_Report" runat="server" Text="Test Report" CssClass="UI_cmdDetail_Btn" CommandName="Test_Report" CommandArgument='<%#Me.UI_dvRequest.Rows.Count%>' />

                                        </ItemTemplate>
                                    </asp:TemplateField>





                                </Columns>
                                <HeaderStyle CssClass="Text_Head" />
                                <RowStyle CssClass="TR_1" />
                                <AlternatingRowStyle CssClass="ListRowEven" />
                                <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Center" />
                            </asp:GridView>
                            <!--[End]資料列表表單-->
                        </fieldset>
                        <div class="erma-warrantyRepair-calc-content">
                            <div class="erma-calc-number">
                                <p>&nbsp;</p>
                            </div>
                        </div>
                        <div class="erma-buttons-div" style="float: right;">
                            <input id="UI_cmdBack" runat="server" type="button" value="006_back" class="erma-button-next-div" onclick="javascript:history.back();" style="display: none;" />
                            <input id="UI_SavedBtn" runat="server" type="button" value="Done" class="erma-button-next-div" onclick="javascript:history.back();" style="display: none;" />
                            <asp:Button ID="UI_SavedBtn_" runat="server" Text="Done" CssClass="erma-button-next-div" OnClientClick="window.parent.Close_Client_Status_List();" />
                        </div>

                    </div>
                </div>

                <uc1:ucClientDetail ID="ucClientDetail" runat="server" />
                <asp:Label ID="UI_lblPreviousPage_RMAID" runat="server" Visible="false"></asp:Label>
                <asp:Label ID="UI_lblPreviousPage_RMANO" runat="server" Visible="false"></asp:Label>

            </ContentTemplate>
        </asp:UpdatePanel>

    </form>
</body>
</html>

