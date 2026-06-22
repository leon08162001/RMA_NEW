<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="ShippingNotice.aspx.vb" Inherits="ShippingNotice" Title="Untitled Page" %>

<%@ Register Src="ascx/ucProgressStatus.ascx" TagName="ucProgressStatus" TagPrefix="uc1" %>
<%@ Register Src="ascx/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <script type="text/javascript" language="javascript">
       $(function () {
            $('#btnSendAR_Invoice').click(function () {
                var strjson = { 'RMA_NO': $('#RMA_NO').val(), 'RepairCenter': $('#<%=UI_RepairCenter.ClientID %>').text()};
                $.ajax({
                    type: "POST",
                    url: "ShippingNotice.aspx/CreateAR_INVOICE2",//'<%= ResolveUrl("~/ShippingNotice.aspx/CreateAR_INVOICE2") %>', 
                    data: JSON.stringify(strjson),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response != null && response.d != null) {
                            var data = $.parseJSON(response.d);
                            if (data.Status == true) {
                                alert("更新成功");
                            }
                            else {
                                alert("更新失敗：" + data.Msg);
                            }
                        }
                        else {
                            alert("回傳資料為空");
                        }
                    },

                    error: function (xhr, status, error) {
                        alert("AJAX錯誤：" + error);
                    }
                });

            });

        });

        function FrmDelete() {
            if (confirm(delMsg)) {
                onProgress('Delete')
                return true;
            }
            return false;
        }

        function chkTrackingNo() {
            var UI_txtTracking = document.getElementById('<%=UI_txtTracking.ClientID %>');
            var lblMsgTracking = document.getElementById('<%=lblMsgTracking.ClientID %>').value;

            if (Trim(UI_txtTracking.value) == "") {
                alert(lblMsgTracking);
                UI_txtTracking.focus();
                return false;
            }

            onProgress('Save')
            return true;
        }

        function JScheckItem() {
            var blnFlag = false;

            var UI_cmdSave = document.getElementById('<%=UI_cmdSave.ClientID %>');
            var UI_cmdSubmit = document.getElementById('<%=UI_cmdSubmit.ClientID %>');

            UI_cmdSave.disabled = true;
            UI_cmdSubmit.disabled = true;

            var oGridView = document.getElementById('<%=UI_dvSerial.ClientID %>');
            var oGridViewID = oGridView.id;
            var iRows = oGridView.rows.length;

            for (var i = 1; i < iRows; i++) {
                var oRMAcheck = oGridView.rows[i].cells[0].childNodes[0];
                if (oRMAcheck != null && oRMAcheck.type == "checkbox") {
                    if (oRMAcheck.checked == true) {
                        blnFlag = true;
                        break;
                    }
                }
            }


            if (blnFlag == true) {
                UI_cmdSave.disabled = false;
                UI_cmdSubmit.disabled = false;
            }

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
                                <td width="94%" align="left">&nbsp;&nbsp;
                                    <asp:Label ID="UI_lblTittle" runat="server" Text="154_Shipping" CssClass="text_tittle"
                                        ForeColor="#326B9B"></asp:Label>
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
                                            <td colspan="4">&nbsp;&nbsp;
                                                <asp:Label ID="UI_lblShipmentInformation" runat="server" Text="155_Search for Shipping Information"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="20%">&nbsp;&nbsp;
                                                <asp:Label ID="UI_lblShipping" runat="server" Text="156_Shipping Number"></asp:Label>
                                            </td>
                                            <td width="40%">:
                                                <asp:Label ID="UI_lblShippingText" runat="server"></asp:Label>
                                            </td>
                                            <td width="15%" align="right">
                                                <asp:Label ID="UI_lblDate" runat="server" Text="097_Date"></asp:Label>
                                            </td>
                                            <td width="25%" align="left">:
                                                <asp:Label ID="UI_lblDateText" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>*
                                                <asp:Label ID="UI_lblPacking" runat="server" Text="157_Packing List of"></asp:Label>
                                            </td>
                                            <td colspan="3">:
                                                <asp:DropDownList ID="cboCustomer" runat="server" Width="300px" AutoPostBack="true"
                                                    OnSelectedIndexChanged="cboCustomer_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>*
                                                <asp:Label ID="UI_lblPackingEU" runat="server" Text="158_Packing List of"></asp:Label>
                                            </td>
                                            <td colspan="3">:
                                                <asp:DropDownList ID="cboCustomerEU" runat="server" Width="300px" AutoPostBack="true"
                                                    OnSelectedIndexChanged="cboCustomerEU_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>*
                                                <asp:Label ID="UI_lblFrom" runat="server" Text="159_From"></asp:Label>
                                            </td>
                                            <td colspan="3">:
                                                <asp:DropDownList ID="UI_cboFrom" runat="server" AutoPostBack="true">
                                                </asp:DropDownList>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>*
                                                <asp:Label ID="UI_lblTo" runat="server" Text="160_To"></asp:Label>
                                            </td>
                                            <td colspan="3">:
                                                <asp:TextBox ID="UI_txtCustomer" runat="server" Width="300" Enabled="false"></asp:TextBox>&nbsp;
                                                <asp:Label ID="UI_lblCustomerID" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="UI_lblCurrencyCode" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="UI_lblCurrencyRate" runat="server" Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;&nbsp;
                                                <asp:Label ID="UI_lblAddress" runat="server" Text="008_Address"></asp:Label>
                                            </td>
                                            <td colspan="3">:
                                                <asp:DropDownList ID="UI_cboAddress" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;&nbsp;
                                                <asp:Label ID="UI_lblExpress" runat="server" Text="161_Express Co."></asp:Label>
                                            </td>
                                            <td colspan="3">:
                                                <asp:DropDownList ID="UI_cboExpress" runat="server">
                                                </asp:DropDownList>
                                                <asp:HyperLink ID="UI_urlExpress" runat="server"></asp:HyperLink>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>*
                                                <asp:Label ID="UI_lblTracking" runat="server" Text="140_Tracking No."></asp:Label>
                                            </td>
                                            <td colspan="3">:
                                                <asp:TextBox ID="UI_txtTracking" runat="server" Width="300" MaxLength="40"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;&nbsp;
                                                <asp:Label ID="UI_lblMemo" runat="server" Text="151_Memo"></asp:Label>
                                            </td>
                                            <td align="left" colspan="3">:
                                                <asp:TextBox ID="UI_txtMemo" runat="server" TextMode="MultiLine" Rows="3" Columns="60"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr runat="server" id="uiTR_ShippingOrders">
                                            <td>
                                                <asp:Label ID="UI_lblShippingOrders" runat="server" Text="149_Ship with Orders"></asp:Label>
                                            </td>
                                            <td align="left" colspan="3">:
                                                <asp:RadioButtonList ID="UI_opgShippingOrders" runat="server" RepeatDirection="Horizontal"
                                                    RepeatLayout="Flow">
                                                    <asp:ListItem Text="066_No" Value="0" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="065_Yes" Value="1"></asp:ListItem>
                                                </asp:RadioButtonList>,
                                                <asp:Label ID="UI_lblShippingNumber" runat="server" Text="150_Number"></asp:Label>:
                                                <asp:TextBox ID="UI_txtShippingNumber" runat="server" Width="100"></asp:TextBox>
                                            </td>
                                        </tr>

                                    </table>
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
                <tr height="28px">
                    <td width="24" background="Images/pic_14.gif">&nbsp;</td>
                    <td background="Images/pic_15.gif">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="1%">&nbsp;</td>
                                <td width="49%" align="left" class="default">
                                    <asp:Label ID="UI_lblShippingTittle" runat="server" Text="162_Add Shipping Pack"
                                        Font-Bold="true"></asp:Label>
                                </td>
                                <td width="50%" align="right">
                                    <asp:Button ID="UI_cmdAdd" runat="server" CssClass="Pick" Text="003_Add" Visible="false" ValidationGroup="ShippingAddGroup" />
                                    <asp:Button ID="UI_cmdPrint" runat="server" CssClass="Confirm" Text="044_Print" Enabled="false" Visible="false"
                                        ValidationGroup="ShippingGroup" OnClientClick="onProgress('Save')" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
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
                                    <div class="form_div" align="center">
                                        <fieldset>
                                            <asp:Label ID="UI_lblAddShippingTittle" runat="server" Text="162_Add Shipping Pack"
                                                Font-Bold="true" Font-Size="Larger"></asp:Label>
                                            <asp:GridView ID="UI_dvSerial" runat="server" Width="100%" CellPadding="0" CellSpacing="1"
                                                border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true"
                                                PagerSettings-Mode="Numeric">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderStyle Width="5%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="UI_CheckGroup" runat="server" AutoPostBack="true" OnCheckedChanged="UI_CheckGroup_CheckedChanged" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="UI_Check" runat="server" AutoPostBack="true" OnCheckedChanged="UI_check_CheckedChanged" />
                                                            <asp:Label ID="UI_RMADID" runat="server" Text='<%# Eval("RMASMD_RMADID") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="UI_Mark" runat="server" Text='<%# Eval("RMASMD_oldMark") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="UI_RMASMD_RMANO" runat="server" Text='<%# Eval("RMASMD_RMANO") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="UI_RMASMD_SERIALNO" runat="server" Text='<%# Eval("RMASMD_SERIALNO") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="UI_RMASMD_MODELNO" runat="server" Text='<%# Eval("RMASMD_MODELNO") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="UI_RMADPARTSN" runat="server" Text='<%# Eval("RMAD_PARTSN") %>' Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="046_RMA Number">
                                                        <HeaderStyle Width="30%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:LinkButton runat="server" ID="UI_linkRMANO" Text='<%# Eval("RMASMD_RMANO") %>' OnClick="UI_linkRMANO_Click"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>



                                                    <asp:TemplateField HeaderText="013_Serial Number">
                                                        <ItemTemplate>
                                                            <asp:Label ID="UI_RMAD_SERIALNO" runat="server" Text='<%# Bind("RMASMD_SERIALNO") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="32%" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="RMASMD_MODELNO" HeaderText="021_Model Name" HeaderStyle-Width="33%"
                                                        ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                </Columns>
                                                <HeaderStyle CssClass="Text_Head" />
                                                <RowStyle CssClass="TR_1" />
                                                <AlternatingRowStyle CssClass="ListRowEven" />
                                                <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Center" />
                                            </asp:GridView>


                                            <input id="UI_cmdBack" runat="server" type="button" value="006_Back" class="Problem_Edit" onclick="javascript: history.back();" />&nbsp;
                                            <asp:Button ID="UI_cmdSave" runat="server" CssClass="Confirm_l" Text="002_Save" ValidationGroup="ShippingGroup" />
                                            <asp:Button ID="UI_cmdSubmit" runat="server" CssClass="Confirm_l" Text="001_Submit" ValidationGroup="ShippingGroup" />


                                            <!--[End]ShippingList列表表單-->
                                            <asp:Button ID="Button1" runat="server" Text="產生AR (admin)" Style="height: 21px" />
                                            <asp:Button ID="Button2" runat="server" Text="重送 AR/Invoice Mail (admin)" Style="height: 21px" />
                                        </fieldset>
                                    </div>
                                    <!--[End]資料列表表單-->
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            
            <div class="container-fluid">

                <div class="row align-items-center">
                    <div class="col-2 my-0">
                    </div>
                    <div class="col-2 my-0 d-flex justify-content-end">
                        <label for="InputRMA_NO" class="form-label">RMA NO：</label>
                    </div>
                    <div class="col-2 my-0">
                        <input class="form-control" id="RMA_NO" />
                    </div>
                    <div class="col-2 my-0">
                        <button id="btnSendAR_Invoice" type="button" class="btn btn-primary" >
                            一鍵重送AR/Invoice<br />
                            (含產AR單號)</button>
                    </div>
                    <div class="col-2 my-0">
                    </div>
                    <div class="col-2 my-0">
                    </div>
                </div>

            </div>

            <asp:Label ID="UI_lblPreviousPage_RMASHID" runat="server" Visible="true"></asp:Label>
            <asp:Label ID="UI_lblPreviousPage_SHIPPINGNO" runat="server" Visible="true"></asp:Label>
            <asp:Label ID="UI_lblPreviousPage_RMANO" runat="server" Visible="true"></asp:Label>
            <asp:HiddenField ID="UI_RepairCenter" runat="server" />

            <uc2:ucMessage ID="ucMessage" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="UI_cmdPrint" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="UI_cmdSave" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="UI_cmdSubmit" EventName="Click" />
            <asp:PostBackTrigger ControlID="ucMessage" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:TextBox runat="server" ID="lblMsgTracking" Style="visibility: hidden" Width="1px"></asp:TextBox>

    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />

    <asp:RequiredFieldValidator ID="rfv_txtTracking" runat="server" ErrorMessage="201_必須填寫 Tracking No"
        Display="None" ControlToValidate="UI_txtTracking" ValidationGroup="ShippingGroup"
        SetFocusOnError="true" Visible="false"></asp:RequiredFieldValidator>

    <asp:RequiredFieldValidator ID="rfv_txtCustomer" runat="server" ErrorMessage="181_請選取客戶"
        Display="None" ControlToValidate="UI_txtCustomer" ValidationGroup="ShippingGroup"
        SetFocusOnError="true"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="rfv_txtCustomerAdd" runat="server" ErrorMessage="181_請選取客戶"
        Display="None" ControlToValidate="UI_cboAddress" ValidationGroup="ShippingGroup"
        SetFocusOnError="true"></asp:RequiredFieldValidator>
    <asp:ValidationSummary ID="vsShipping" runat="server" ShowMessageBox="true" ShowSummary="false"
        ValidationGroup="ShippingGroup" />

    <asp:ValidationSummary ID="vsShippingAdd" runat="server" ShowMessageBox="true" ShowSummary="false"
        ValidationGroup="ShippingAddGroup" />

    <asp:Label ID="UI_flowCase" runat="server" Visible="false"></asp:Label>

</asp:Content>

