<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Receive_Edit.aspx.vb" Inherits="Receive_Edit" Title="Untitled Page" ValidateRequest="False" %>

<%@ Register Src="ascx/ucProgressStatus.ascx" TagName="ucProgressStatus" TagPrefix="uc1" %>
<%@ Register Src="ascx/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc3" %>
<%@ Register Src="ascx/ucPopProblem.ascx" TagName="ucPopProblem" TagPrefix="uc4" %>
<%@ Register Src="ascx/ucModel.ascx" TagName="ucModel" TagPrefix="uc5" %>
<%@ Register Src="ascx/ucCustAddress.ascx" TagName="ucCustAddress" TagPrefix="uc6" %>
<%@ Register Src="ascx/ucClientDetailPur.ascx" TagName="ucClientDetailPur" TagPrefix="uc7" %>
<%@ Register Src="ascx/ucSpecialSetting.ascx" TagName="ucSpecialSetting" TagPrefix="uc8" %>
<%@ Register Src="ascx/ucWarrantyPartsView.ascx" TagName="ucWarrantyPartsView" TagPrefix="uc9" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <script type="text/javascript" language="javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SubmitControl);

        function FrmDelete() {
            if (confirm(delMsg)) {
                onProgress('Delete')
                return true;
            }
            return false;
        }

        function SubmitControl_Group(objName) {
            //alert("OK");
            var UI_cmdSubmit = document.getElementById('<%=UI_cmdSubmit.ClientID %>');
            var UI_cmdDelete = document.getElementById('<%=UI_cmdDelete.ClientID %>');
            UI_cmdSubmit.disabled = true;
            UI_cmdDelete.disabled = true;

            var oGridView = document.getElementById('<%=UI_dvRMADetail.ClientID %>');
            var oGridViewID = oGridView.id;
            var iRows = oGridView.rows.length;


            for (var i = 1; i < iRows; i++) {
                cell = oGridView.rows[i].cells[0];
                for (j = 0; j < cell.childNodes.length; j++) {
                    if (cell.childNodes[j].type == "checkbox") {
                        cell.childNodes[j].checked = objName.checked;
                    }
                }
            }

            for (var i = 1; i < iRows; i++) {
                cell = oGridView.rows[i].cells[7];
                for (j = 0; j < cell.childNodes.length; j++) {
                    if (cell.childNodes[j].type == "checkbox") {
                        cell.childNodes[j].disabled = objName.checked;
                    }
                }
            }

            if (objName.checked == true) {
                UI_cmdSubmit.disabled = false;
                UI_cmdDelete.disabled = true;
            }
        }

        function SubmitControl() {
            var blnFlag = false;
            var UI_cmdSubmit = document.getElementById('<%=UI_cmdSubmit.ClientID %>');
            var UI_cmdDelete = document.getElementById('<%=UI_cmdDelete.ClientID %>');
            UI_cmdSubmit.disabled = true;
            UI_cmdDelete.disabled = true;

            var oGridView = document.getElementById('<%=UI_dvRMADetail.ClientID %>');
            var oGridViewID = oGridView.id;
            var iRows = oGridView.rows.length;

            for (var i = 1; i < iRows; i++) {
                cell = oGridView.rows[i].cells[0];
                for (j = 0; j < cell.childNodes.length; j++) {
                    if (cell.childNodes[j].type == "checkbox") {
                        if (cell.childNodes[j].checked == true) {
                            blnFlag = true;
                            break;
                        }
                    }
                }
            }

            for (var i = 1; i < iRows; i++) {
                cell = oGridView.rows[i].cells[7];
                for (j = 0; j < cell.childNodes.length; j++) {
                    if (cell.childNodes[j].type == "checkbox") {
                        cell.childNodes[j].disabled = blnFlag;
                    }
                }
            }

            if (blnFlag == true) {
                UI_cmdSubmit.disabled = false;
                UI_cmdDelete.disabled = true;
            }
        }




        function DelControl() {
            var blnFlag = false;
            var UI_cmdSubmit = document.getElementById('<%=UI_cmdSubmit.ClientID %>');
            var UI_cmdDelete = document.getElementById('<%=UI_cmdDelete.ClientID %>');
            UI_cmdSubmit.disabled = true;
            UI_cmdDelete.disabled = true;

            var oGridView = document.getElementById('<%=UI_dvRMADetail.ClientID %>');
            var oGridViewID = oGridView.id;
            var iRows = oGridView.rows.length;

            for (var i = 1; i < iRows; i++) {
                cell = oGridView.rows[i].cells[7];
                for (j = 0; j < cell.childNodes.length; j++) {
                    if (cell.childNodes[j].type == "checkbox") {
                        if (cell.childNodes[j].checked == true) {
                            blnFlag = true;
                            break;
                        }
                    }
                }
            }

            for (var i = 1; i < iRows; i++) {
                cell = oGridView.rows[i].cells[0];
                for (j = 0; j < cell.childNodes.length; j++) {
                    if (cell.childNodes[j].type == "checkbox") {
                        cell.childNodes[j].disabled = blnFlag;
                    }
                }
            }

            if (iRows > 0) {
                var UI_CheckGroup = document.getElementById(oGridViewID + '_ctl01' + '_UI_CheckGroup');
                UI_CheckGroup.disabled = false;
                if (blnFlag == true) {
                    UI_CheckGroup.disabled = true;
                }
            }

            if (blnFlag == true) {
                UI_cmdSubmit.disabled = true;
                UI_cmdDelete.disabled = false;
            }
        }


        function SerialCheck() {
            var blnFlag = false;
            var oGridView = document.getElementById('<%=UI_dvRMADetail.ClientID %>');
            var oGridViewID = oGridView.id;
            var iRows = oGridView.rows.length;
            var oSerialText = document.getElementById('<%=UI_txtSerialNo.ClientID %>');

            if (oSerialText.value != "") {
                for (var i = 1; i < iRows; i++) {
                    var oRMAcheck = oGridView.rows[i].cells[0].childNodes[0];
                    var SERIALNO = oGridView.rows[i].cells[0].childNodes[2];

                    if (oRMAcheck != null && oRMAcheck.type == "checkbox" && SERIALNO != null) {
                        if (oSerialText.value.toLowerCase() == SERIALNO.value.toLowerCase()) {
                            oRMAcheck.checked = true;
                            blnFlag = true;
                            break;
                        }
                    } else {

                        var SERIALNO = oGridView.rows[i].cells[0].childNodes[0];
                        if (SERIALNO != null) {
                            if (oSerialText.value.toLowerCase() == SERIALNO.value.toLowerCase()) {
                                blnFlag = true;
                                break;
                            }
                        }

                    }

                }
            }

            SubmitControl();


            if (blnFlag == false) {
                var UI_txtSerial = document.getElementById('<%=UI_txtSerial.ClientID %>');
        var UI_cmdAdd = document.getElementById('<%=UI_cmdAdd.ClientID %>');
                UI_txtSerial.value = oSerialText.value;
                oSerialText.value = "";
                UI_cmdAdd.click();
            } else {
                oSerialText.value = "";
            }
        }

        function Validate_ModelNo_Serial(oSrc, args) {
            var UI_txtSerial = document.getElementById('<%=UI_txtSerial.ClientID %>').value;
            var UI_cboModel = document.getElementById('<%=UI_cboModel.ClientID %>').value;

            if (Trim(UI_txtSerial) == "" && Trim(UI_cboModel) == "") {
                args.IsValid = false;
            } else {
                args.IsValid = true;
            }
        }
    </script>

    <uc1:ucProgressStatus ID="ucProgressStatus" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table height="410" cellspacing="0" cellpadding="0" width="100%" border="0">
                <tbody>
                    <tr height="10">
                        <td width="24" background="Images/pic_12.gif">&nbsp;</td>
                        <td valign="top" align="left">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <!--[Begin]Tittle-->
                                <tbody>
                                    <tr>
                                        <td width="3%">&nbsp;</td>
                                        <td align="left" width="94%">
                                            <asp:Label ID="UI_lblTittle" runat="server" Text="001_New Request" ForeColor="#326B9B"
                                                CssClass="text_tittle"></asp:Label>
                                        </td>
                                        <td width="3%">&nbsp;</td>
                                    </tr>
                                    <!--[End]Tittle-->
                                    <!--[Begin]資料查詢條件區-->
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td align="left">
                                            <table class="default" cellspacing="1" cellpadding="0" width="95%" border="0">
                                                <tbody>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label ID="UI_lblClientInformation" runat="server" Text="002_Client Information"
                                                                Font-Bold="true"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="UI_lblRMANumber" runat="server" Text="046_RMA Number" Font-Bold="true"></asp:Label>
                                                        </td>
                                                        <td>:
                                                            <asp:Label ID="UI_lblRMANumberText" runat="server" ForeColor="red"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="15%">
                                                            <asp:Label ID="UI_lblAccountID" runat="server" Text="003_* Account ID"></asp:Label>
                                                        </td>
                                                        <td width="25%">:
                                                            <asp:Label ID="UI_lblAccountIDText" runat="server"></asp:Label>
                                                        </td>
                                                        <td align="left" width="15%">
                                                            <asp:Label ID="UI_lblAccountName" runat="server" Text="004_* Account Name"></asp:Label>
                                                        </td>
                                                        <td align="left" width="37%">:
                                                            <asp:Label ID="UI_lblAccountNameText" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="UI_lblUserID" runat="server" Text="005_* User ID"></asp:Label>
                                                        </td>
                                                        <td>:
                                                            <asp:Label ID="UI_lblUserIDText" runat="server"></asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <font class="default_Red">*</font>
                                                            <asp:Label ID="UI_lblApplicant" runat="server" Text="006_* Applicant"></asp:Label>
                                                        </td>
                                                        <td align="left">:
                                                            <asp:TextBox ID="UI_txtApplicant" runat="server" Width="150"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <font class="default_Red">*</font>
                                                            <asp:Label ID="UI_lblTel" runat="server" Text="007_* Tel No."></asp:Label>
                                                        </td>
                                                        <td colspan="3">:
                                                            <asp:TextBox ID="UI_txtTel" runat="server" Width="150"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <font class="default_Red">*</font>
                                                            <asp:Label ID="UI_lblAddress" runat="server" Text="008_* Address"></asp:Label>
                                                        </td>
                                                        <td align="left" colspan="3">:
                                                            <asp:TextBox ID="UI_txtAddress" runat="server" Width="450px" ReadOnly="true"></asp:TextBox>
                                                            <asp:Button ID="UI_cmdAdressPick" runat="server" Text="_Pick" CssClass="Pick"></asp:Button>
                                                            <%--			                            <asp:DropDownList ID="UI_cboAddress" runat="server"></asp:DropDownList>--%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <font class="default_Red">*</font>
                                                            <asp:Label ID="UI_lblMail" runat="server" Text="044_Mail"></asp:Label>
                                                        </td>
                                                        <td align="left" colspan="3">:
                                                            <asp:TextBox ID="UI_txtMail" runat="server" Width="300px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="UI_lblRepairCenter" runat="server" Text="009_* Repair Center"></asp:Label>
                                                        </td>
                                                        <td colspan="3">:
                                                            <asp:Label ID="UI_lblRepairCenterText" runat="server"></asp:Label>
                                                            <asp:Label ID="UI_lblRepairCenterValue" runat="server" Visible="false"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="UI_lblEUCompany" runat="server" Text="End User Company Name"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="UI_txtEUCompany" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="UI_lblEUTel" runat="server" Text="End User Tel No."></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="UI_txtEUTel" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="UI_lblEUName" runat="server" Text="End User Contact Person"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="UI_txtEUName" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="UI_lblEUMail" runat="server" Text="End User Email"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="UI_txtEUMail" runat="server" Width="300px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="UI_lblEUAddress" runat="server" Text="End User Address"></asp:Label>
                                                        </td>
                                                        <td colspan="3">
                                                            <asp:TextBox ID="UI_txtEUAddress" runat="server" Width="450px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <!--[End]資料查詢條件區-->
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr height="30">
                        <td background="Images/pic_12.gif">&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr height="10">
                        <td width="24" background="Images/pic_14.gif">&nbsp;</td>
                        <td background="Images/pic_15.gif">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tbody>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td align="left" colspan="2">
                                            <asp:Label ID="UI_lblProductInformation" class="default" runat="server" Text="010_Product Information"
                                                Font-Bold="true"></asp:Label>
                                        </td>
                                        <td align="left" colspan="2">
                                            <asp:Label ID="UI_lblSerialNo" runat="server" Text="043_Quick Receiving:  Serial No."
                                                CssClass="default"></asp:Label>:
                                            <asp:TextBox ID="UI_txtSerialNo" runat="server" Width="120"></asp:TextBox>
                                            <input id="UI_cmdQuery" class="Confirm_l" onclick="SerialCheck()" type="button" value="021_Query"
                                                runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="2%">&nbsp;</td>
                                        <td align="left" width="45%" colspan="2">
                                            <font class="default_Red">*</font>
                                            <asp:Label ID="UI_lblSerial" class="default" runat="server" Text="013_Serial Number"></asp:Label>
                                            <asp:TextBox ID="UI_txtSerial" runat="server" Width="136"></asp:TextBox>
                                            <asp:Button ID="UI_cmdConfirm" runat="server" Visible="false" Text="_Pick" CssClass="Pick"></asp:Button>
                                            <asp:Label ID="UI_txtSerialParts" Visible="false" runat="server" class="default" Text=""></asp:Label>
                                        </td>
                                        <td align="left" width="14%">
                                            <font class="default_Red">*</font>
                                            <asp:Label ID="UI_lblModel" class="default" runat="server" Text="012_ Model No"></asp:Label>
                                        </td>
                                        <td align="left" width="40%">:
                                            <asp:DropDownList ID="UI_cboModel" runat="server" Width="100px">
                                            </asp:DropDownList>
                                            <%--	                        <asp:TextBox ID="UI_txtModel" runat="server" Width="136" ReadOnly="true"></asp:TextBox>--%>
                                            <asp:Button ID="UI_cmdPick" runat="server" Visible="false" Text="_Pick" CssClass="Pick"></asp:Button>
                                            <asp:Button ID="UI_cmdAdd" runat="server" Text="_Add" ValidationGroup="vsSerial"
                                                CssClass="Search" OnClientClick="onProgress('Process')"></asp:Button>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td background="Images/pic_20.gif">&nbsp;</td>
                        <td valign="top" align="center" bgcolor="#e3d8be">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tbody>
                                    <tr>
                                        <td width="1%">&nbsp;</td>
                                        <td class="default" align="left" width="99%" colspan="3">
                                            <!--[Begin]資料列表表單-->
                                            <div align="center">
                                                <asp:GridView ID="UI_dvRMADetail" runat="server" CssClass="default" Width="100%"
                                                    AutoGenerateColumns="False" border="0" CellSpacing="1" CellPadding="0" EnableModelValidation="True">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="UI_CheckGroup" runat="server" onclick="SubmitControl_Group(this);" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="UI_RMAcheck" runat="server" Checked='<%# Eval("isCheck") %>' onclick="SubmitControl();" />
                                                                <asp:Label ID="UI_SeqID" runat="server" Text='<%# Eval("SeqID") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="UI_RMADID" runat="server" Text='<%# Eval("RMAD_ID") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="UI_RMADNO" runat="server" Text='<%# Eval("RMAD_RMANO") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="UI_ISFILL" runat="server" Text='<%# Eval("RMAD_ISFILL") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="UI_RMAD_SERIALNO" runat="server" Text='<%# Eval("RMAD_SERIALNO")%>' Visible="false"></asp:Label>
                                                                <asp:Label ID="UI_RMAD_MODELNO" runat="server" Text='<%# Eval("RMAD_MODELNO")%>' Visible="false"></asp:Label>
                                                                <asp:Label ID="UI_RMADWARRANTY" runat="server" Text='<%# Eval("RMAD_WARRANTY") %>'
                                                                    Visible="false"></asp:Label>
                                                                <asp:Label ID="UI_RECEVSTATUS" runat="server" Text='<%# Eval("RMAD_RECEVSTATUS") %>'
                                                                    Visible="false"></asp:Label>
                                                                <asp:TextBox runat="server" ID="UI_SERIALNO" Text='<%# Eval("RMAD_SERIALNO") %>'
                                                                    Width="1px" Style="display: none"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="013_Serial Number">
                                                            <HeaderStyle Width="10%" HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="UI_SHOWSERIAL" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Part Number" SortExpression="RMAD_PARTSN" Visible="false">
                                                            <HeaderStyle Width="10%" CssClass="text9pt" HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="UI_PARTNUMBER" runat="server" Text='<%# Eval("RMAD_PARTSN") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="013_Serial Numbe" SortExpression="RMAD_SERIALNO" Visible="false">
                                                            <HeaderStyle Width="10%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="RMAD_SERIALNO" runat="server" Text='<%# Eval("RMAD_SERIALNO") %>'
                                                                    Width="130"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="RMAD_MODELNO" HeaderText="012_Model No">
                                                            <HeaderStyle Width="12%" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="RMAD_PRODUCTDESC" HeaderText="196_PRODUCT DESC" Visible="false">
                                                            <HeaderStyle Width="15%" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="RMAD_sWARRANTY" HeaderText="EW Warranty">
                                                            <HeaderStyle Width="8%" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CWEndWarr" HeaderText="CW Warranty">
                                                            <HeaderStyle Width="8%" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="SW Warranty">
                                                            <HeaderStyle Width="8%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="cmdSWDetail" runat="server" Text='<%# Eval("SWEndWarr") %>' CommandName="cmdSWDetail"
                                                                    CommandArgument='<%# me.UI_dvRMADetail.Rows.Count%>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="RepairCnt" HeaderText="Repair CNT">
                                                            <HeaderStyle Width="8%" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Warranty Detail">
                                                            <HeaderStyle Width="5%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="UI_cmdWarrDetail" runat="server" ImageUrl="images/icon-detail.gif" CommandName="cmdWarrDetail" CommandArgument='<%# me.UI_dvRMADetail.Rows.Count%>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Detail">
                                                            <HeaderStyle Width="5%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="UI_cmdDetail" runat="server" ImageUrl="images/icon-detail.gif" CommandName="cmdDetail" CommandArgument='<%# me.UI_dvRMADetail.Rows.Count%>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="016_Problem">
                                                            <HeaderStyle Width="8%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:Button ID="UI_cmdEdit" runat="server" Text="_Edit" CssClass="Problem_Edit" CommandName="cmdEdit"
                                                                    CommandArgument='<%# me.UI_dvRMADetail.Rows.Count%>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="RecvDATE" HeaderText="042_Received Date">
                                                            <HeaderStyle Width="10%" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="017_Delete">
                                                            <HeaderStyle Width="12%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="UI_Delcheck" runat="server" onclick="DelControl();" />
                                                                <asp:ImageButton ID="imgDel" runat="server" ImageAlign="AbsBottom" ImageUrl="images/xx.gif"
                                                                    CommandName="cmdDel" OnClientClick="return FrmDelete()" CommandArgument='<%# me.UI_dvRMADetail.Rows.Count%>' />
                                                                <asp:Label ID="UI_RecvDelDATE" runat="server" Text='<%# Eval("RecvDelDATE") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle CssClass="Text_Head" />
                                                    <RowStyle CssClass="TR_1" />
                                                    <AlternatingRowStyle CssClass="ListRowEven" />
                                                </asp:GridView>
                                            </div>
                                            <!--[End]資料列表表單-->
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">&nbsp;</td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td background="Images/pic_20.gif">&nbsp;</td>
                        <td valign="top" align="center" bgcolor="#e3d8be">
                            <!--[Begin]Submit-->
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tbody>
                                    <tr>
                                        <td width="3%">&nbsp;</td>
                                        <td class="default" align="center" width="94%">
                                            <asp:Button ID="UI_cmdTmpSave" runat="server" Text="_Temporary Save" ValidationGroup="vsRMAGroup"
                                                CssClass="Confirm" OnClick="UI_cmdTmpSave_Click"></asp:Button>
                                            <asp:Button ID="UI_cmdSubmit" OnClick="UI_cmdSubmit_Click" runat="server" Text="_Submit"
                                                ValidationGroup="vsRMAGroup" CssClass="Confirm" OnClientClick="onProgress('Save')"></asp:Button>
                                            <asp:Button ID="UI_cmdDelete" OnClick="UI_cmdDelete_Click" runat="server" Text="_Delete"
                                                CssClass="Confirm" OnClientClick="return FrmDelete()"></asp:Button>
                                        </td>
                                        <td width="3%">&nbsp;</td>
                                    </tr>
                                </tbody>
                            </table>
                            <!--[End]Submit-->
                        </td>
                    </tr>
                </tbody>
            </table>
            <asp:Label ID="UI_lblPreviousPage_RMAID" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_lblPreviousPage_RMANO" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_lblPreviousPage_CUNO" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_lblPreviousPage_RMASTATUS" runat="server" Visible="false"></asp:Label>
            <uc4:ucPopProblem ID="UcPopProblem" runat="server"></uc4:ucPopProblem>
            <uc3:ucMessage ID="ucMessage" runat="server"></uc3:ucMessage>
            <uc5:ucModel ID="ucModel" runat="server"></uc5:ucModel>
            <uc6:ucCustAddress ID="ucCustAddress" runat="server"></uc6:ucCustAddress>
            <uc7:ucClientDetailPur ID="ucClientDetailPur" runat="server" />
            <uc8:ucSpecialSetting ID="ucSpecialSetting" runat="server" />
            <uc9:ucWarrantyPartsView ID="UcWarrantyPartsView" runat="server" />

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="UI_cmdConfirm" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="UI_cmdAdressPick" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="UI_cmdPick" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="UI_cmdAdd" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="UI_cmdSubmit" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="UI_cmdDelete" EventName="Click" />
            <asp:PostBackTrigger ControlID="ucMessage" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:RequiredFieldValidator ID="rfv_txtTEL" runat="server" ErrorMessage="207_必須填寫電話"
        Display="None" ControlToValidate="UI_txtTel" ValidationGroup="vsRMAGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="rfv_txtAdress" runat="server" ErrorMessage="047_必須填寫地址"
        Display="None" ControlToValidate="UI_txtAddress" ValidationGroup="vsRMAGroup"
        SetFocusOnError="true"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="rfv_txtApplicant" runat="server" ErrorMessage="048_必須填寫連絡人員"
        Display="None" ControlToValidate="UI_txtApplicant" ValidationGroup="vsRMAGroup"
        SetFocusOnError="true"></asp:RequiredFieldValidator>
    <%--<asp:RegularExpressionValidator ID="revEMail" runat="server" ErrorMessage="049_EMail輸入格式有誤"
        ControlToValidate="UI_txtMail" Display="None" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
        ValidationGroup="vsRMAGroup" SetFocusOnError="true"></asp:RegularExpressionValidator>--%>
    <asp:RequiredFieldValidator ID="revEMail_Empty" runat="server" ErrorMessage="049_EMail輸入格式有誤"
        Display="None" ControlToValidate="UI_txtMail" ValidationGroup="vsRMAGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
    <asp:ValidationSummary ID="vsRMA" runat="server" ShowMessageBox="true" ShowSummary="false"
        ValidationGroup="vsRMAGroup" />
    <%--<asp:RequiredFieldValidator ID="rfv_txtModelNo" runat="server" ErrorMessage = "081_Model No不可空白" Display="None" ControlToValidate="UI_cboModel" ValidationGroup ="vsSerial" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
    <asp:CustomValidator ID="cvModelNo_Serial" runat="server" ClientValidationFunction="Validate_ModelNo_Serial"
        ErrorMessage="210_Model No不可空白" Display="None" ValidationGroup="vsSerial"></asp:CustomValidator>
    <asp:ValidationSummary ID="vsSerial" runat="server" ShowMessageBox="true" ShowSummary="false"
        ValidationGroup="vsSerial" />


</asp:Content>
