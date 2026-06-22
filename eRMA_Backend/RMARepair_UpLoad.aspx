<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="RMARepair_UpLoad.aspx.vb" Inherits="RMARepair_UpLoad" Title="Untitled Page" %>

<%@ Register TagPrefix="Upload" Namespace="Brettle.Web.NeatUpload" Assembly="Brettle.Web.NeatUpload" %>
<%@ Register Src="ascx/ucProgressStatus.ascx" TagName="ucProgressStatus" TagPrefix="uc1" %>
<%@ Register Src="ascx/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>
<%@ Register Src="ascx/ucRepairDetail.ascx" TagName="ucRepairDetail" TagPrefix="uc3" %>
<%@ Register Src="ascx/ucClientDetail.ascx" TagName="ucClientDetail" TagPrefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <script type="text/javascript" language="javascript">
        function FrmDelete() {
            //    if (confirm(delMsg))   {
            //        onProgress('Delete')
            //        return true;
            //    }
            //    
            //    return false;
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
                                <td width="94%" align="left">
                                    <asp:Label ID="UI_lblTittle" runat="server" Text="114_Request Detail" CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
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
                                            <td>
                                                <asp:Label ID="UI_lblRMANo" runat="server" Text="029_RMA No."></asp:Label>
                                            </td>
                                            <td colspan="3">:
			                            <asp:Label ID="UI_lblRMANoText" runat="server"></asp:Label>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td width="15%">
                                                <asp:Label ID="UI_lblAccountID" runat="server" Text="003_Account ID"></asp:Label>
                                            </td>
                                            <td width="35%">:
			                            <asp:Label ID="UI_lblAccountIDText" runat="server"></asp:Label>
                                            </td>
                                            <td width="15%" align="left">
                                                <asp:Label ID="UI_lblAccountName" runat="server" Text="004_Account Name"></asp:Label>
                                            </td>
                                            <td width="35%" align="left">:
			                            <asp:Label ID="UI_lblAccountNameText" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="UI_lblApplicantID" runat="server" Text="045_Applicant ID"></asp:Label>
                                            </td>
                                            <td>:
			                            <asp:Label ID="UI_lblApplicantIDText" runat="server"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="UI_lblApplicant" runat="server" Text="006_Applicant"></asp:Label>
                                            </td>
                                            <td align="left">:
			                            <asp:Label ID="UI_lblApplicantText" runat="server"></asp:Label>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td valign="top">
                                                <asp:Label ID="UI_lblTel" runat="server" Text="007_Tel No"></asp:Label>
                                            </td>
                                            <td valign="top">:
			                            <asp:Label ID="UI_lblTelText" runat="server"></asp:Label>
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:Label ID="UI_lblAddress" runat="server" Text="008_Address"></asp:Label>
                                            </td>
                                            <td align="left">:
			                            <asp:Label ID="UI_lblAddressText" runat="server"></asp:Label>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <asp:Label ID="UI_lblRepairCenter" runat="server" Text="009_Repair Center"></asp:Label>
                                            </td>
                                            <td colspan="3">:
			                            <asp:Label ID="UI_lblRepairCenterText" runat="server"> </asp:Label>
                                                <asp:Label ID="UI_lblRepairCenterNO" runat="server" Visible="false"></asp:Label>
                                            </td>
                                        </tr>

                                        <tr runat="server" id="UI_TRFileUpload" visible="false">
                                            <td align="left" class="default">
                                                <asp:Label ID="UI_lblReportUpload" runat="server" Text="115_Report Upload"></asp:Label>
                                            </td>
                                            <td align="left" colspan="3" class="default">:
                                        <%--<asp:FileUpload runat="server" id="html_FileUpload" ></asp:FileUpload>--%>
                                                <Upload:InputFile ID="html_FileUpload" runat="server" />
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="UI_lblDesc" runat="server" Text="120_Desc"></asp:Label>.
                                        <asp:TextBox ID="UI_lblDescription" runat="server" Width="200px"></asp:TextBox>
                                                <asp:Button ID="UI_cmdUpload" runat="server" Text="037_Upload" Height="23px" Style="font-size: 11px;" ValidationGroup="vsRepairUploadGroup" />
                                            </td>
                                        </tr>

                                        <tr runat="server" id="UI_TRUploadFileDesc" visible="false">
                                            <td>&nbsp;</td>
                                            <td colspan="3">&nbsp;&nbsp;
                                        <asp:Label runat="server" ID="UI_UploadFileDesc" ForeColor="red" Text="186_上傳容量(最大5MB)"></asp:Label>
                                                <Upload:ProgressBar ID="progressBar" runat="server" Triggers="submitButton linkButton commandButton htmlInputButtonButton htmlInputButtonSubmit">
                                                    <asp:Label ID="label" runat="server" Text="Check Progress" />
                                                </Upload:ProgressBar>

                                                <%--
                                        <br>&nbsp;
                                        <Upload:ProgressBar id="inlineProgressBar" runat="server" inline="true" Height="50px" />

			<Upload:ProgressBar id="progressBar" runat="server" Triggers="submitButton linkButton commandButton htmlInputButtonButton htmlInputButtonSubmit">
				<asp:Label id="label" runat="server" Text="Check Progress"/>
			</Upload:ProgressBar>
                                                --%>


                                            </td>
                                        </tr>

                                        <tr runat="server" id="UI_TRReportAttachment" visible="false">
                                            <td>
                                                <asp:Label ID="UI_lblReportAttachment" runat="server" Text="116_Report  Attachment"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:GridView ID="UI_dvRetailUpload" runat="server" border="1" Width="95%" CellSpacing="0" CellPadding="0" CssClass="default" BorderColor="#C0C0C0" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="117_File Name">
                                                            <HeaderStyle Width="25%" Height="15px" HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="left" Height="15px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:HyperLink runat="server" ID="UI_RepairUpload" Target="_blank"></asp:HyperLink>
                                                                <asp:Label ID="UI_lblRepairUpload" runat="server" Text='<%# Eval("RMARU_UPLOADFILE") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="UI_RMARU_ID" runat="server" Text='<%# Eval("RMARU_ID") %>' Visible="false"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:BoundField DataField="RMARU_DESC" HeaderText="118_File Description" HeaderStyle-Width="32%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="RMARU_LUSTMP" HeaderText="097_Date" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd hh:ss}"></asp:BoundField>

                                                        <asp:TemplateField HeaderText="041_Cancel">
                                                            <HeaderStyle Width="8%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgDel" runat="server" ImageAlign="AbsBottom" ImageUrl="~/images/xx.gif" CommandName="cmdDel" OnClientClick="return FrmDelete()" CommandArgument='<%# me.UI_dvRetailUpload.Rows.Count%>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                    </Columns>
                                                    <HeaderStyle CssClass="Text_Head" />
                                                    <RowStyle CssClass="TR_1" />
                                                    <AlternatingRowStyle CssClass="ListRowEven" />
                                                    <%--<PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Center" />--%>
                                                </asp:GridView>
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

                <tr height="20px">
                    <td width="24" background="Images/pic_14.gif">&nbsp;</td>
                    <td background="Images/pic_15.gif">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="1%">&nbsp;</td>
                                <td width="99%" align="left" class="default">
                                    <asp:Label ID="UI_lblProductTittle" runat="server" Text="010_Product Information" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>


                <tr height="250px">
                    <td background="Images/pic_20.gif">&nbsp;</td>
                    <td valign="top" bgcolor="#E3D8BE" align="center">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="1%">&nbsp;</td>
                                <td width="99%" align="left" class="default">
                                    <!--[Begin]資料列表表單-->
                                    <div align="center">
                                        <asp:GridView ID="UI_dvRetailDetail" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="UI_SeqID" runat="server" Text='<%# me.UI_dvRetailDetail.Rows.Count +1 %>'></asp:Label>
                                                        <asp:Label ID="UI_RMADID" runat="server" Text='<%# Eval("RMAD_ID") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMADCSTMP" runat="server" Text='<%# Eval("RMAD_CSTMP") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="RMAD_SERIALNO" HeaderText="013_Serial Number" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="RMAD_MODELNO" HeaderText="012_Model No" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="RMAD_PRODUCTDESC" HeaderText="196_PRODUCT DESC" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="Warranty" HeaderText="EW Warranty" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="CWEndWarr" HeaderText="CW Warranty" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="SWEndWarr" HeaderText="SW Warranty" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="FarcName" HeaderText="119_Problem Category" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>

                                                <asp:TemplateField HeaderText="038_Detail">
                                                    <HeaderStyle Width="12%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="UI_cmdEdit" runat="server" ImageUrl="images/icon-detail.gif" CommandName="cmdDetail" CommandArgument='<%# me.UI_dvRetailDetail.Rows.Count%>' />
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
                        </table>
                    </td>
                </tr>


                <tr height="10%">
                    <td background="Images/pic_20.gif">&nbsp;</td>
                    <td valign="top" bgcolor="#E3D8BE" align="center">
                        <!--[Begin]頁數-->
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="1%">&nbsp;</td>
                                <td width="99%" align="center" class="default">
                                    <input id="UI_cmdBack" runat="server" type="button" value="006_Back" class="Problem_Edit" onclick="javascript:history.back();" />&nbsp;
	                        <asp:Button ID="UI_cmdCancel" runat="server" Text="038_RMA Cancel" CssClass="Confirm" Visible="false" />
                                </td>
                            </tr>
                        </table>
                        <!--[End]頁數-->
                    </td>
                </tr>
            </table>

            <asp:Label ID="UI_lblPreviousPage_RMAID" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_lblPreviousPage_RMANO" runat="server" Visible="false"></asp:Label>
            <uc2:ucMessage ID="ucMessage" runat="server" />

            <uc3:ucRepairDetail ID="ucRepairDetail" runat="server" />
            <uc4:ucClientDetail ID="ucClientDetail" runat="server" />

        </ContentTemplate>

        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="UI_cmdCancel" EventName="Click" />
            <asp:PostBackTrigger ControlID="UI_cmdUpload" />
            <asp:PostBackTrigger ControlID="ucMessage" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:RequiredFieldValidator ID="rfvhtmlFullFile" runat="server" ErrorMessage="121_請選取要上傳的檔案" Display="None" ControlToValidate="html_FileUpload" ValidationGroup="vsRepairUploadGroup"></asp:RequiredFieldValidator>
    <asp:ValidationSummary ID="vsRepairUpload" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="vsRepairUploadGroup" />

</asp:Content>

