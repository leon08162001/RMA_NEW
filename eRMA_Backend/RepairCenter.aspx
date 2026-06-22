<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="RepairCenter.aspx.vb" Inherits="RepairCenter" Title="Untitled Page" %>

<%@ Register Src="ascx/ucProgressStatus.ascx" TagName="ucProgressStatus" TagPrefix="uc1" %>
<%@ Register Src="ascx/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <script type="text/javascript" language="javascript">
        function Validate_Country(oSrc, args) {
            var sValue1 = document.getElementById('<%=UI_cboCountry.ClientID %>').value;

            if (sValue1 == -1) {
                args.IsValid = false;
            } else {
                args.IsValid = true;
            }
        }

        function Validate_Currency(oSrc, args) {
            var sValue1 = document.getElementById('<%=UI_cboCurrency.ClientID %>').value;

            if (sValue1 == -1) {
                args.IsValid = false;
            } else {
                args.IsValid = true;
            }
        }

        function FrmDelete() {
            if (confirm(delMsg)) {
                onProgress('Delete')
                return true;
            }
            return false;
        }
    </script>

    <uc1:ucProgressStatus ID="ucProgressStatus" runat="server" />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>

            <table border="0" width="100%" id="table4" cellspacing="0" cellpadding="0" height="100%">

                <tr>
                    <td width="24" background="Images/pic_12.gif">&nbsp;</td>
                    <td valign="top" align="left">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0" height="100%">
                            <!--[Begin]Tittle-->
                            <tr>
                                <td width="3%">&nbsp;</td>
                                <td width="94%" align="left">
                                    <asp:Label ID="UI_lblTittle" runat="server" Text="001_Setting - Repair Center" CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
                                </td>
                                <td width="3%">&nbsp;</td>
                            </tr>
                            <!--[End]Tittle-->
                            <!--[Begin]新增資料-->
                            <tr>
                                <td>&nbsp;</td>
                                <td align="left">
                                    <table width="100%" align="center" border="0" cellspacing="1" class="default">
                                        <tr>
                                            <td width="25%">&nbsp;
				                        <asp:Label ID="UI_lblCompName" runat="server" Text="009_Repair Center."></asp:Label>
                                                <td width="35%">:
				                        <asp:TextBox ID="UI_txtCompName" runat="server" Width="200"></asp:TextBox>

                                                </td>
                                                <td width="12%" align="right">
                                                    <asp:Label ID="UI_lblCompNo" runat="server" Text="003_Center Code"></asp:Label>
                                                </td>
                                                <td width="20%" align="left">:
				                        <asp:Label ID="UI_lblCompNo1" runat="server"></asp:Label>
                                                    <asp:TextBox ID="UI_txtCompNo" runat="server" Width="100" MaxLength="20"></asp:TextBox>
                                                </td>
                                                <td width="10%" align="left">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;
				                        <asp:Label ID="UI_lblCountry" runat="server" Text="010_Country"></asp:Label>
                                            </td>
                                            <td colspan="4">:
				                        <asp:DropDownList ID="UI_cboCountry" runat="server"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;
				                        <asp:Label ID="UI_lblCurrency" runat="server" Text="004_Currency"></asp:Label>
                                            </td>
                                            <td colspan="4">:
				                        <asp:DropDownList ID="UI_cboCurrency" runat="server"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;
				                        <asp:Label ID="UI_lblLaborCost" runat="server" Text="011_Labor Cost"></asp:Label>
                                            </td>
                                            <td colspan="4">:
				                        <asp:TextBox ID="UI_txtLaborCost" runat="server" Width="100" Text="0"></asp:TextBox>/
				                        <asp:Label ID="UI_lblManHour" runat="server" Text="012_man hour"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;
				                        <asp:Label ID="UI_lblApprovalAmount" runat="server" Text="013_Repair Approval amount"></asp:Label>
                                            </td>
                                            <td colspan="4">:
				                        <asp:TextBox ID="UI_txtApprovalAmount" runat="server" Width="100" Text="0"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;
				                        <asp:Label ID="UI_lblLowestDiscount" runat="server" Text="014_Lowest Discount"></asp:Label>
                                            </td>
                                            <td colspan="4">:
				                        <asp:TextBox ID="UI_txtLowestDiscount" runat="server" Width="100" Text="0"></asp:TextBox>%
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;
				                        <asp:Label ID="UI_lblTEL" runat="server" Text="005_TEL"></asp:Label>
                                            </td>
                                            <td colspan="4">:
				                        <asp:TextBox ID="UI_txtTEL" runat="server" Width="200"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;
				                        <asp:Label ID="UI_lblAddress" runat="server" Text="016_Address (1)"></asp:Label>
                                            </td>
                                            <td colspan="4">: 
						                <asp:TextBox ID="UI_txtAddress" runat="server" Width="400"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;
				                        <asp:Label ID="UI_lblExpress" runat="server" Text="017_Express Co."></asp:Label>
                                            </td>
                                            <td colspan="4">: 
						                <asp:TextBox ID="UI_txtExpress" runat="server" Width="400"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;
				                        <asp:Label ID="UI_lblExpressURL" runat="server" Text="018_Express URL"></asp:Label>
                                            </td>
                                            <td colspan="4">: 
						                <asp:TextBox ID="UI_txtExpressURL" runat="server" Width="400" Text="http://"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;
				                        <asp:Label ID="UI_lblVisible" runat="server" Text="007_Status"></asp:Label>
                                            </td>
                                            <td colspan="4">: 
						                <asp:RadioButtonList ID="UI_opgVisible" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="1" Text="019_Open" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="0" Text="020_Close"></asp:ListItem>
                                        </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;
				                        <asp:Label ID="UI_lblStockManager" runat="server" Text="017_StockManager."></asp:Label>
                                            </td>
                                            <td colspan="4">: 
						                <asp:TextBox ID="UI_txtStockManager" runat="server" Width="400"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;
				                        <asp:Label ID="UI_lblRemark" runat="server" Text="021_Remark"></asp:Label>
                                            </td>
                                            <td colspan="4">:<asp:TextBox ID="UI_txtRemark" runat="server" TextMode="MultiLine" Rows="3" Columns="70"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="5">
                                                <asp:Button ID="UI_cmdSubmit" runat="server" Text="_Submit" CssClass="Confirm_l" ValidationGroup="RepairCenterGroup" OnClientClick="onProgress('Save')" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5">&nbsp;</td>
                                        </tr>
                                    </table>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <!--[End]新增資料-->
                        </table>
                    </td>
                </tr>

                <!-- Part Search START  -->
                <tr>
                    <td width="24" height="27" background="Images/pic_14.gif">&nbsp;</td>
                    <td height="27" background="Images/pic_15.gif">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td>&nbsp;</td>
                                <td width="100%" class="default">
                                    <table border="0" width="100%" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td width="60%" align="left" class="default">
                                                <asp:Label ID="UI_lblInformationTittle" runat="server" Text="022_Part's Information" Font-Bold="true"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
		                                <asp:Button ID="UI_cmdAdd" runat="server" Text="_Add" CssClass="Confirm_l" Width="100" OnClick="UI_cmdAdd_Click" Visible="false" />
                                            </td>
                                            <td align="center" class="default">
                                                <asp:Label ID="UI_lblPart" runat="server" Text="023_Part's No."></asp:Label>
                                                <asp:TextBox ID="UI_txtPart" runat="server" Width="100"></asp:TextBox>&nbsp;
		                                <asp:Button ID="UI_cmdSearch" runat="server" Text="_Search" CssClass="Search" OnClick="UI_cmdSearch_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <!-- Part Search END  -->

                <!-- Part List START  -->
                <tr id="UI_trPart" runat="server">
                    <td background="Images/pic_20.gif">&nbsp;</td>
                    <td valign="top" bgcolor="#E3D8BE" align="center">
                        <table border="0" cellspacing="0" cellpadding="0" height="100%" width="100%">
                            <tr>
                                <td>&nbsp;</td>
                                <td width="100%" align="left" class="default">
                                    <!--[Begin]新增資料列表-->
                                    <div align="center">
                                        <asp:GridView ID="UI_RepairBOM" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="UI_SeqID" runat="server" Text='<%# Eval("SeqID") %>'></asp:Label>
                                                        <asp:Label ID="UI_Rpbomad" runat="server" Text='<%# Eval("RPBOM_AD") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_oldRpbomPartNo" runat="server" Text='<%# Eval("RPBOM_oldPARTNO") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_Rpbom_CompNo" runat="server" Text='<%# Eval("RPBOM_COMPNO") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="023_Part's No.">
                                                    <HeaderStyle Width="15%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="UI_lblRpbomPartNo" Text='<%# Eval("RPBOM_PARTNO") %>'></asp:Label>
                                                        <asp:TextBox ID="UI_RpbomPartNo" runat="server" Width="100" Text='<%# Eval("RPBOM_PARTNO") %>' Visible="false"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfv_RpbomPartNo" runat="server" ErrorMessage="035_請輸入Part’s No" Display="None" SetFocusOnError="true" ValidationGroup="RepairBOMGroup"></asp:RequiredFieldValidator>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="025_Location">
                                                    <HeaderStyle Width="15%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="UI_lblRpbomLocation" Text='<%# Eval("RPBOM_LOCATION") %>'></asp:Label>
                                                        <asp:TextBox ID="UI_RpbomLocation" runat="server" Width="100" Text='<%# Eval("RPBOM_LOCATION") %>' Visible="false"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="026_Material Cost">
                                                    <HeaderStyle Width="15%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="UI_RpbomMateriaLcost" runat="server" Width="100" Text='<%# Eval("RPBOM_MATERIALCOST") %>'></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfv_RpbomMateriaLcost" runat="server" ErrorMessage="036_請輸入零件費用" Display="None" SetFocusOnError="true" ValidationGroup="RepairBOMGroup"></asp:RequiredFieldValidator>
                                                        <asp:RangeValidator ID="rv_RpbomMateriaLcost" runat="server" ErrorMessage="037_輸入零件費用格式有誤" Display="None" MinimumValue="0" MaximumValue="10000" Type="Double" ValidationGroup="RepairBOMGroup"></asp:RangeValidator>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="027_Description">
                                                    <HeaderStyle Width="45%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="UI_lblRpbomDesc" Text='<%# Eval("RPBOM_DESC") %>'></asp:Label>
                                                        <asp:TextBox ID="UI_RpbomDesc" runat="server" Width="95%" Text='<%# Eval("RPBOM_DESC") %>' Visible="false"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="028_Delete">
                                                    <HeaderStyle Width="7%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="UI_cmdEdit" runat="server" ImageAlign="AbsMiddle" ImageUrl="images/xx.gif" CommandName="cmdDel" CommandArgument='<%# me.UI_RepairBOM.Rows.Count%>' OnClientClick="return FrmDelete()" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="Text_Head" />
                                            <RowStyle CssClass="TR_1" />
                                            <AlternatingRowStyle CssClass="ListRowEven" />
                                            <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Center" />
                                        </asp:GridView>
                                    </div>
                                    <!--[End]新增資料列表-->
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <!-- Part List START  -->

                <tr>
                    <td background="Images/pic_20.gif" height="130">&nbsp;</td>
                    <td valign="top" bgcolor="#E3D8BE" align="center">
                        <!--[Begin]Submit-->
                        <table border="0" cellspacing="0" cellpadding="0" width="100%" height="50px">
                            <tr>
                                <td width="3%">&nbsp;</td>
                                <td width="94%" align="center" class="default">
                                    <asp:Button ID="UI_cmdCancel" runat="server" Text="_Cancel" CssClass="Problem_Edit" CausesValidation="false" OnClientClick="onProgress('Cancel')" OnClick="UI_cmdCancel_Click" />&nbsp;
		                    <asp:Button ID="UI_cmdPartSubmit" runat="server" Text="_Submit" CssClass="Confirm_l" ValidationGroup="RepairBOMGroup" OnClientClick="onProgress('Save')" OnClick="UI_cmdPartSubmit_Click" />
                                </td>
                                <td width="3%">&nbsp;</td>
                            </tr>
                        </table>
                        <!--[End]Submit-->
                    </td>
                </tr>
            </table>

            <uc2:ucMessage ID="ucMessage" runat="server" />

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="UI_cmdSubmit" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="UI_cmdAdd" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="UI_cmdSearch" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="UI_cmdPartSubmit" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="UI_cmdCancel" EventName="Click" />
            <asp:PostBackTrigger ControlID="ucMessage" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:RequiredFieldValidator ID="rfvCompName" runat="server" ControlToValidate="UI_txtCompName"
        ErrorMessage="029_請輸入公司名稱" Display="None" TabIndex="0" SetFocusOnError="true" ValidationGroup="RepairCenterGroup"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="rfvCompNo" runat="server" ControlToValidate="UI_txtCompNo"
        ErrorMessage="030_請輸入公司代碼" Display="None" TabIndex="0" SetFocusOnError="true" ValidationGroup="RepairCenterGroup"></asp:RequiredFieldValidator>

    <asp:CustomValidator ID="cvCountry" runat="server" ClientValidationFunction="Validate_Country" ErrorMessage="031_請選取國家名稱" Display="None" Operator="DataTypeCheck" SetFocusOnError="true" ValidationGroup="RepairCenterGroup"></asp:CustomValidator>
    <asp:CustomValidator ID="cvCurrency" runat="server" ClientValidationFunction="Validate_Currency" ErrorMessage="032_請選取幣別" Display="None" Operator="DataTypeCheck" SetFocusOnError="true" ValidationGroup="RepairCenterGroup"></asp:CustomValidator>

    <asp:RequiredFieldValidator ID="rfvLaborCost" runat="server" ControlToValidate="UI_txtLaborCost" ErrorMessage="033_請輸入工時單價" Display="None" TabIndex="0" SetFocusOnError="true" ValidationGroup="RepairCenterGroup"></asp:RequiredFieldValidator>
    <asp:RangeValidator ID="rvLaborCost" runat="server" ControlToValidate="UI_txtLaborCost" ErrorMessage="034_輸入工時格式有誤" Display="None" MinimumValue="0" MaximumValue="10000" Type="Double" SetFocusOnError="true" ValidationGroup="RepairCenterGroup"></asp:RangeValidator>

    <asp:RequiredFieldValidator ID="rfvApprovalAmount" runat="server" ControlToValidate="UI_txtApprovalAmount" ErrorMessage="038_請輸入同意維修金額" Display="None" TabIndex="0" SetFocusOnError="true" ValidationGroup="RepairCenterGroup"></asp:RequiredFieldValidator>
    <asp:RangeValidator ID="rvApprovalAmount" runat="server" ControlToValidate="UI_txtApprovalAmount" ErrorMessage="039_輸入同意維修金額格式有誤" Display="None" MinimumValue="0" MaximumValue="10000" Type="Integer" SetFocusOnError="true" ValidationGroup="RepairCenterGroup"></asp:RangeValidator>

    <asp:RequiredFieldValidator ID="rfvLowestDiscount" runat="server" ControlToValidate="UI_txtLowestDiscount" ErrorMessage="040_請輸入最低折扣" Display="None" TabIndex="0" SetFocusOnError="true" ValidationGroup="RepairCenterGroup"></asp:RequiredFieldValidator>
    <asp:RangeValidator ID="rvLowestDiscount" runat="server" ControlToValidate="UI_txtLowestDiscount" ErrorMessage="041_輸入最低折扣格式有誤" Display="None" MinimumValue="0" MaximumValue="10000" Type="Double" SetFocusOnError="true" ValidationGroup="RepairCenterGroup"></asp:RangeValidator>

    <asp:ValidationSummary ID="vsRepairCenter" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="RepairCenterGroup" />
    <asp:ValidationSummary ID="VaRepairBom" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="RepairBOMGroup" />


    <asp:Label runat="server" ID="UI_lblPreviousPage_CompNo" Visible="false"></asp:Label>

</asp:Content>
