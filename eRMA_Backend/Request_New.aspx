<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Request_New.aspx.vb" Inherits="Request_New" Title="Untitled Page" ValidateRequest="false" %>

<%@ Register Src="ascx/ucProgressStatus.ascx" TagName="ucProgressStatus" TagPrefix="uc1" %>
<%@ Register Src="ascx/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc3" %>
<%@ Register Src="ascx/ucPopProblem.ascx" TagName="ucPopProblem" TagPrefix="uc4" %>
<%@ Register Src="ascx/ucModel.ascx" TagName="ucModel" TagPrefix="uc5" %>
<%@ Register Src="ascx/ucCustAddress.ascx" TagName="ucCustAddress" TagPrefix="uc6" %>
<%@ Register Src="ascx/ucCustomer_pick.ascx" TagName="ucCustomer_pick" TagPrefix="uc7" %>
<%@ Register Src="ascx/ucWarrantyPartsView.ascx" TagName="ucWarrantyPartsView" TagPrefix="uc8" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>

<%--<%@ Register TagPrefix="Upload" Namespace="Brettle.Web.NeatUpload" Assembly="Brettle.Web.NeatUpload" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <script type="text/javascript" language="javascript">
        function FrmCancel() {
            if (confirm(CancelItemMsg)) {
                onProgress('Cancel')
                return true;
            }
            return false;
        }

        function FrmDelete() {
            if (confirm(delMsg)) {
                onProgress('Delete')
                return true;
            }
            return false;
        }

        function FrmSubmit(Progress, action) {
            if (action == "Save") {
                alert("Save the request temporarily. The request will not become effective unless the 'Submit' button is clicked.");
            }

            var i;
            for (i = 0; i < Page_Validators.length; i++) {
                ValidatorValidate(Page_Validators[i]);
            }

            for (i = 0; i < Page_Validators.length; i++) {
                if (!Page_Validators[i].isvalid && Page_Validators[i].validationGroup == "vsRMAGroup") {
                    alert(Page_Validators[i].errormessage);
                    Page_IsValid = false;
                    return false;
                }
            }

            if (action != "Submit") {
                var blnFlag = false;
                var sMsg = document.getElementById('<%=chkWarrantyMsg.ClientID %>').outerText;
                Date.prototype.dateAdd = dateAddExtention;

                var toDay = new Date();
                var fdate = new Date(toDay.toDateString());
                var edate = new Date(toDay.dateAdd("m", 1).toDateString());

                var oGridView = document.getElementById('<%=UI_dvRMADetail.ClientID %>');
                var oGridViewID = oGridView.id;
                var iRows = oGridView.rows.length;

                for (var i = 1; i < iRows; i++) {
                    var j = i + 1;
                    j = "0000000" + j
                    if (iRows.toString().length == 1) {
                        j = Right(j, iRows.toString().length + 1);
                    } else {
                        j = Right(j, iRows.toString().length);
                    }

                    //var UI_WARRANTY = document.getElementById(oGridViewID + '_ctl'+ j + '_UI_WARRANTY').outerText;

                    //if (isDate(UI_WARRANTY)==true)
                    //{
                    //    var WARRANTY = new Date(UI_WARRANTY);
                    //    if ((fdate<=WARRANTY && edate>=WARRANTY) && (fdate<=WARRANTY))
                    //    {
                    //        blnFlag = true;
                    //    }
                    //}
                }
                if (blnFlag == true) {
                    alert(sMsg);
                }
            }

            onProgress(Progress);

            //    if (action=="Save"){
            //        onProgress('Save')
            //    }
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

        function Validate_FileUpLoad(oSrc, args) {
            var html_FileUpload = document.getElementById('<%=html_FileUpload.ClientID %>').value.toLowerCase(); //檔案名稱

            if (html_FileUpload != "") {
                if (html_FileUpload.indexOf("csv") > 0) {
                    args.IsValid = true;
                } else {
                    args.IsValid = false;
                }

            } else {
                args.IsValid = false;
            }
        }

    </script>


    <uc1:ucProgressStatus ID="ucProgressStatus" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table border="0" cellspacing="0" cellpadding="0" width="100%" height="410px">
                <tr height="10%">
                    <td width="24" background="Images/pic_12.gif">&nbsp;</td>
                    <td valign="top" align="left">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <!--[Begin]Tittle-->
                            <tr>
                                <td width="3%">&nbsp;</td>
                                <td width="94%" align="left">
                                    <asp:Label ID="UI_lblTittle" runat="server" Text="001_New Request" CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
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
                                            <td colspan="4">
                                                <asp:Label ID="UI_lblClientInformation" runat="server" Text="002_Client Information" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="15%">
                                                <asp:Label ID="UI_lblAccountID" runat="server" Text="003_* Account ID"></asp:Label>
                                            </td>
                                            <td width="25%">:
			                                    <asp:Label ID="UI_lblAccountIDText" runat="server"></asp:Label>
                                                <asp:TextBox runat="server" ID="UI_txtAccountIDText" Width="50px" Visible="false"></asp:TextBox>
                                                <asp:Button ID="UI_cmdCust_Search" runat="server" Text="016_Search" CssClass="Pick" Visible="false" OnClick="UI_cmdCust_Search_Click" />
                                            </td>
                                            <td width="12%" align="left">
                                                <asp:Label ID="UI_lblAccountName" runat="server" Text="004_* Account Name"></asp:Label>
                                            </td>
                                            <td width="40%" align="left">:
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
                                            <td align="left"><font class="default_Red">*</font>
                                                <asp:Label ID="UI_lblApplicant" runat="server" Text="006_* Applicant"></asp:Label>
                                            </td>
                                            <td align="left">:
			                            <asp:TextBox ID="UI_txtApplicant" runat="server" Width="150"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td><font class="default_Red">*</font>
                                                <asp:Label ID="UI_lblTel" runat="server" Text="007_* Tel No."></asp:Label>
                                            </td>
                                            <td colspan="2">:
			                                    <asp:TextBox ID="UI_txtTel" runat="server" Width="150"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td align="left"><font class="default_Red">*</font>
                                                <asp:Label ID="UI_lblAddress" runat="server" Text="008_* Address"></asp:Label>
                                            </td>
                                            <td align="left" colspan="3">:
			                            <asp:TextBox ID="UI_txtAddress" runat="server" Width="450px" ReadOnly="true"></asp:TextBox>
                                                <asp:Button ID="UI_cmdAdressPick" runat="server" Text="016_Pick" CssClass="Pick" />
                                                <%--			                            <asp:DropDownList ID="UI_cboAddress" runat="server"></asp:DropDownList>--%>
                                            </td>
                                        </tr>


                                        <tr>
                                            <td><font class="default_Red">*</font>
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
			                            <asp:Label ID="UI_lblRepairCenterText" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="UI_lblRepairCenterValue" runat="server" Visible="false"></asp:Label>
                                                <asp:DropDownList ID="UI_cboRepairCenter" runat="server" Visible="false"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="trCompany">
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
                                        <tr runat="server" id="trEUName">
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
                                        <tr runat="server" id="trEUAddress">
                                            <td>
                                                <asp:Label ID="UI_lblEUAddress" runat="server" Text="End User Address"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="UI_txtEUAddress" runat="server" Width="450px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top">
                                                <asp:Label ID="UI_lblRemark" runat="server" Text="134_Remark"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox runat="server" ID="UI_txtRemark" TextMode="MultiLine" Rows="4" Columns="50" MaxLength="1500"></asp:TextBox>
                                                <br />
                                                <div style="width: 50%;">
                                                    <asp:Label runat="server" ID="uiLblDesc01" ForeColor="red"></asp:Label>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:CheckBox ID="UI_PartsRequest" runat="server" /><asp:Label ID="lb_PartsRequest" runat="server" Text="RMA Number for Parts Request Only"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <!--[End]資料查詢條件區-->
                        </table>
                    </td>
                </tr>

                <tr height="30px">
                    <td background="Images/pic_12.gif">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>

                <tr height="10px">
                    <td width="24" background="Images/pic_14.gif">&nbsp;</td>
                    <td background="Images/pic_15.gif">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td>&nbsp;</td>
                                <td align="left">
                                    <asp:Label ID="UI_lblProductInformation" runat="server" class="default" Text="010_Product Information" Font-Bold="true"></asp:Label>
                                </td>
                                <td colspan="3" align="left">&nbsp;&nbsp;&nbsp;&nbsp;
	                        <asp:Label ID="UI_lblFile" runat="server" Text="024_Upload File" class="default"></asp:Label>&nbsp;:&nbsp;
                                    <%--                            <Upload:InputFile id="html_FileUpload" runat="server" />
                                    --%>

                                    <asp:FileUpload ID="html_FileUpload" runat="server" Width="200px" />

                                    <asp:Button ID="UI_cmdFileAdd" runat="server" Text="_FileAdd" CssClass="Search" ValidationGroup="vsFileUpLoad" />
                                    <asp:Label runat="server" ID="UI_UploadFileDesc" CssClass="default" ForeColor="red" Text="186_上傳容量(最大5MB)"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;
                            <asp:HyperLink runat="server" ID="UI_cmdExcelFile" Text="212_Download File" CssClass="default" ForeColor="blue" Target="_blank"></asp:HyperLink>

                                    <%--			<Upload:ProgressBar id="progressBar" runat="server" Triggers="submitButton linkButton commandButton htmlInputButtonButton htmlInputButtonSubmit">
				<asp:Label id="label" runat="server" Text="Check Progress"/>
			</Upload:ProgressBar>
                                    --%>	                    </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td colspan="4" align="left">
                                    <asp:Label ID="UI_lblPleaseTittle" runat="server" class="default" Text="011_Please pick a model you are going to request repair or key in barcode number correctly."></asp:Label>
                                </td>
                            </tr>

                            <tr>
                                <td width="2%">&nbsp;</td>

                                <td align="left" width="12%"><font class="default_Red">*</font>
                                    <asp:Label ID="UI_lblSerial" runat="server" class="default" Text="013_Serial Number"></asp:Label>
                                </td>
                                <td align="left" width="34%">:
	                                <asp:TextBox ID="UI_txtSerial" runat="server" Width="136" Style="text-transform: uppercase;"></asp:TextBox>
                                    <asp:Label ID="UI_txtSerialParts" Visible="false" runat="server" class="default" Text=""></asp:Label>
                                    <asp:Button ID="UI_cmdConfirm" runat="server" Text="_Pick" CssClass="Pick" Visible="false" />
                                </td>

                                <td align="right" width="15%"><font class="default_Red"></font>
                                    <asp:Label ID="UI_lblModel" runat="server" class="default" Text="012_ Model No"></asp:Label>
                                </td>
                                <td align="left" width="40%">
                                    <%--	                        <asp:TextBox ID="UI_txtModel" runat="server" Width="136" ReadOnly="true"></asp:TextBox>--%>
                                    <asp:DropDownList runat="server" ID="UI_cboModel" Width="250px"></asp:DropDownList>
                                    <asp:Button ID="UI_cmdPick" runat="server" Text="_Pick" CssClass="Pick" Visible="false" />
                                    <asp:Button ID="UI_cmdAdd" runat="server" Text="_Add" CssClass="Search" OnClientClick="onProgress('Process')" ValidationGroup="vsSerial" />
                                    <asp:Button ID="UI_cmdMail" runat="server" CssClass="Pick" Text="_重發mail" Visible="false" />
                                </td>

                            </tr>
                        </table>
                    </td>
                </tr>

                <tr valign="top">
                    <td background="Images/pic_20.gif">&nbsp;</td>
                    <td valign="top" bgcolor="#E3D8BE" align="center">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="1%">&nbsp;</td>
                                <td colspan="3" width="99%" align="left" class="default">
                                    <!--[Begin]資料列表表單-->
                                    <div align="center">
                                        <asp:GridView ID="UI_dvRMADetail" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="UI_SeqID" runat="server" Text='<%# Eval("SeqID") %>'></asp:Label>
                                                        <asp:Label ID="UI_RMADID" runat="server" Text='<%# Eval("RMAD_ID") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_ISFILL" runat="server" Text='<%# Eval("RMAD_ISFILL") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMADWARRANTY" runat="server" Text='<%# Eval("RMAD_WARRANTY") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMADCSTMP" runat="server" Text='<%# Eval("RMAD_CSTMP") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_CWEndWarr" runat="server" Text='<%# Eval("CWEndWarr")%>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMAD_MODELNO" runat="server" Text='<%# Eval("RMAD_MODELNO")%>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMAD_SERIALNO" runat="server" Text='<%# Eval("RMAD_SERIALNO")%>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Part Number" SortExpression="RMAD_PARTSN">
                                                    <HeaderStyle Width="15%" CssClass="text9pt" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="UI_PARTNUMBER" runat="server" Text='<%# Eval("RMAD_PARTSN") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="013_Serial" SortExpression="RMAD_SERIALNO">
                                                    <HeaderStyle Width="15%" CssClass="text9pt" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="UI_SERIALNO" ReadOnly="true" runat="server" Text='<%# Eval("RMAD_SERIALNO") %>' Width="130"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="RMAD_MODELNO" HeaderText="012_Model No" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="RMAD_PRODUCTDESC" HeaderText="196_PRODUCT DESC" Visible="false" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>

                                                <asp:TemplateField HeaderText="EW Warranty">
                                                    <HeaderStyle Width="10%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="UI_WARRANTY" Text='<%# Eval("RMAD_sWARRANTY") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="CWEndWarr" HeaderText="CW Warranty">
                                                    <HeaderStyle Width="10%" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Warranty Detail">
                                                    <HeaderStyle Width="10%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="UI_cmdWarrDetail" runat="server" ImageUrl="images/icon-detail.gif" CommandName="cmdWarrDetail" CommandArgument='<%# me.UI_dvRMADetail.Rows.Count%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="016_Problem">
                                                    <HeaderStyle Width="12%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Button ID="UI_cmdEdit" runat="server" Text="_Edit" CssClass="Problem_Edit" CommandName="cmdEdit" CommandArgument='<%# me.UI_dvRMADetail.Rows.Count%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="017_Delete">
                                                    <HeaderStyle Width="12%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgDel" runat="server" ImageAlign="AbsBottom" ImageUrl="images/xx.gif" CommandName="cmdDel" OnClientClick="return FrmDelete()" CommandArgument='<%# me.UI_dvRMADetail.Rows.Count%>' />
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
                            <tr>
                                <td colspan="2">&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td background="Images/pic_20.gif">&nbsp;</td>
                    <td valign="top" bgcolor="#E3D8BE" align="center">
                        <!--[Begin]Submit-->
                        <table border="0" cellspacing="0" cellpadding="0" width="100%" height="100%">
                            <tr>
                                <td width="3%">&nbsp;</td>
                                <td width="94%" align="center" class="default">
                                    <asp:Button ID="UI_cmdTmpSave" runat="server" Text="_Temporary Save" CssClass="Confirm" ValidationGroup="vsRMAGroup" OnClientClick="return FrmSubmit('Save','Save')" />
                                    <asp:Button ID="UI_cmdModify" runat="server" Text="_Modify Confirm" CssClass="Confirm" Visible="false" ValidationGroup="vsRMAGroup" OnClientClick="return FrmSubmit('Save','Modify')" />
                                    <asp:Button ID="UI_cmdSubmit" runat="server" Text="_Submit" CssClass="Confirm" ValidationGroup="vsRMAGroup" OnClientClick="return FrmSubmit('Save','Submit')" />
                                    <asp:Button ID="UI_cmdCancel" runat="server" Text="_Cancel" CssClass="Confirm_l" CausesValidation="false" OnClientClick="return FrmCancel()" Visible="false" />
                                    <input id="UI_cmdBack" runat="server" type="button" value="006_back" class="Problem_Edit" onclick="javascript: history.back();" visible="false" />
                                </td>
                                <td width="3%">&nbsp;</td>
                            </tr>
                        </table>
                        <!--[End]Submit-->
                    </td>
                </tr>
            </table>

            <asp:Label ID="UI_lblPreviousPage_RMAID" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_lblPreviousPage_RMANO" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_RMA_STATUS" runat="server" Visible="false"></asp:Label>

            <uc4:ucPopProblem ID="UcPopProblem" runat="server" />
            <uc3:ucMessage ID="ucMessage" runat="server" />
            <uc5:ucModel ID="ucModel" runat="server" />
            <uc6:ucCustAddress ID="ucCustAddress" runat="server" />
            <uc7:ucCustomer_pick ID="ucCustomer_pick" runat="server" />
            <uc8:ucWarrantyPartsView ID="UcWarrantyPartsView" runat="server" />

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="UI_cmdConfirm" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="UI_cmdAdressPick" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="UI_cmdPick" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="UI_cmdAdd" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="UI_cmdTmpSave" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="UI_cmdSubmit" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="UI_cmdModify" EventName="Click" />

            <asp:PostBackTrigger ControlID="UI_cmdFileAdd" />
            <asp:PostBackTrigger ControlID="ucMessage" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:Label ID="chkWarrantyMsg" runat="server" Style="visibility: hidden"></asp:Label>
    <asp:RequiredFieldValidator ID="rfv_txtAccountID" runat="server" ErrorMessage="220_必須輸入客戶編號" Display="None" ControlToValidate="UI_txtAccountIDText" ValidationGroup="vsRMAGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="rfv_txtTEL" runat="server" ErrorMessage="207_必須填寫電話" Display="None" ControlToValidate="UI_txtTel" ValidationGroup="vsRMAGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="rfv_txtAdress" runat="server" ErrorMessage="047_必須填寫地址" Display="None" ControlToValidate="UI_txtAddress" ValidationGroup="vsRMAGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="rfv_txtApplicant" runat="server" ErrorMessage="048_必須填寫連絡人員" Display="None" ControlToValidate="UI_txtApplicant" ValidationGroup="vsRMAGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
    <%--<asp:RegularExpressionValidator ID="revEMail" runat="server" ErrorMessage="049_EMail輸入格式有誤" ControlToValidate="UI_txtMail" Display="None" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="vsRMAGroup" SetFocusOnError="true"></asp:RegularExpressionValidator>--%>
    <asp:RequiredFieldValidator ID="revEMail_Empty" runat="server" ErrorMessage="049_EMail輸入格式有誤" Display="None" ControlToValidate="UI_txtMail" ValidationGroup="vsRMAGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
    <asp:ValidationSummary ID="vsRMA" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="vsRMAGroup" />
    <%--<asp:RequiredFieldValidator ID="rfv_txtModelNo" runat="server" ErrorMessage = "191_Model No不可空白" Display="None" ControlToValidate="UI_cboModel" ValidationGroup ="vsSerial" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
    <asp:CustomValidator ID="cvModelNo_Serial" runat="server" ClientValidationFunction="Validate_ModelNo_Serial" ErrorMessage="210_Model No不可空白" Display="None" ValidationGroup="vsSerial"></asp:CustomValidator>
    <asp:CustomValidator ID="cvFileUpLoad" runat="server" ClientValidationFunction="Validate_FileUpLoad" ErrorMessage="219_上傳檔案必須為CSV檔" Display="None" ValidationGroup="vsFileUpLoad"></asp:CustomValidator>
    <asp:ValidationSummary ID="vsSerial" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="vsSerial" />
    <asp:ValidationSummary ID="vsFileUpLoad" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="vsFileUpLoad" />
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />

</asp:Content>

