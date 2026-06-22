<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Warranty_TypeSetting_add.aspx.vb" Inherits="Warranty_TypeSetting_add" Title="Untitled Page" %>

<%@ Register Src="ascx/ucProgressStatus.ascx" TagName="ucProgressStatus" TagPrefix="uc1" %>
<%@ Register Src="ascx/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>
<%@ Register Src="ascx/ucProductGroup.ascx" TagName="ucProductGroup" TagPrefix="uc6" %>
<%@ Register Src="ascx/ucValidCustomer.ascx" TagName="ucValidCustomer" TagPrefix="uc8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <script type="text/javascript" src="script/jquery-3.6.0.min.js"></script>

    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            <%-- $("#<%=UI_txtProductGroup.ClientID%>").on('change', function () {
                 alert("aaa");
             });

             $("#<%=UI_txtProductGroup.ClientID%>").change(function () {
                 alert('Your Name field just lost focus');
             });
             --%>
        });

        function dropchange() {
            //var id = ddl.id;
            //var selectedVal = ddl.value;
            var selectedVal = document.getElementById("<%=UI_txtProductGroup.ClientID%>").value +
                document.getElementById("<%=UI_cboWarrantyType.ClientID%>").value +
                document.getElementById("<%=UI_cboProgramType.ClientID%>").value +
                document.getElementById("<%=UI_cboItemType.ClientID%>").value +
                document.getElementById("<%=UI_cboPriceVer.ClientID%>").value + '00';

            //inputF.setAttribute('value', 'defaultValue');   //<input />
            /*alert(selectedVal);*/
            document.getElementById("<%=lbl_part_no.ClientID%>").innerHTML = selectedVal;

            <%-- var selectedText = $("#<%=UI_txtProductGroup.ClientID%>").val() +
                 $("#<%=UI_cboWarrantyType.ClientID%> option:selected").text() +
                 $("#<%=UI_cboProgramType.ClientID%> option:selected").text() +
                 $("#<%=UI_cboItemType.ClientID%> option:selected").text() +
                 $("#<%=UI_cboPriceVer.ClientID%> option:selected").text();--%>

            var userData = '{War_Group:"' + $("#<%=UI_txtProductGroup.ClientID%>").val() + '",WARRSET_TYPE:"' + $("#<%=UI_cboWarrantyType.ClientID%>").val() + '", \
             Program_Type: "' + $("#<%=UI_cboProgramType.ClientID%>").val() + '", Item_Type: "' + $("#<%=UI_cboItemType.ClientID%>").val() + '", \
             Price_Ver: "' + $("#<%=UI_cboPriceVer.ClientID%>").val() + '"} ';
            /*data: '{ pID: "1833", qty: "' + $(this).val() + '", lblType: "1" }',*/

            $.ajax({
                type: 'post',
                url: 'Warranty_TypeSetting_add.aspx/GetWarrsetPartNM',
                dataType: 'json',
                contentType: 'application/json',
                data: userData,
                success: function (result) {
                    $("#<%=UI_txtWarrantyName.ClientID%>").val(result.d);
                 },
                 error: function () {
                     alert('error');
                 }
             });
        }
    </script>

    <uc1:ucProgressStatus ID="ucProgressStatus" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table border="0" width="100%" id="table1" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="24" background="Images/pic_12.gif">&nbsp;
                    </td>
                    <td valign="top" align="left">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0" height="100%">
                            <!--[Begin]Tittle-->
                            <tr>
                                <td width="3%">&nbsp;
                                </td>
                                <td width="94%" align="left">
                                    <asp:Label ID="UI_lblTittle" runat="server" Text="035_Warrenty Setting- Special"
                                        CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
                                </td>
                                <td width="3%">&nbsp;
                                </td>
                            </tr>
                            <!--[End]Tittle-->
                            <!--[Begin]新增資料-->
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td align="left">
                                    <table width="100%" align="center" border="0" cellspacing="1" class="default">
                                        <tr>
                                            <td colspan="7">&nbsp;<asp:Label ID="UI_lblSubTittle" runat="server" Text="038_Please fill in the header below and click button Go, system will list the price table."></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="100px" align="left">
                                                <asp:Label ID="UI_lblOperationCenter" runat="server" Text="026_Operation Center"></asp:Label>
                                            </td>
                                            <td width="1" align="left">:
                                            </td>
                                            <td width="20%" align="left">
                                                <asp:DropDownList ID="UI_cboOperationCenter" runat="server" Width="150px"></asp:DropDownList>
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="UI_lblWarrantyName" runat="server" Text="029_Warranty Name"></asp:Label>
                                            </td>
                                            <td align="left">:
                                                <asp:TextBox ID="UI_txtWarrantyName" TextMode="MultiLine" runat="server" Width="524px"></asp:TextBox>
                                            </td>
                                            <td width="10%" align="right">
                                                <asp:Label ID="UI_lblExtraMonths" runat="server" Text="031_Extra Months"></asp:Label>
                                            </td>
                                            <td width="20%" align="left">:
                                                <asp:TextBox ID="UI_ExtraMonths" runat="server" Width="150"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="UI_lblProductGroup" runat="server" Text="027_Product Group"></asp:Label>
                                            </td>
                                            <td width="1" align="left">:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="UI_txtProductGroup" ReadOnly="true" CssClass="readyonly" runat="server" Width="90px"></asp:TextBox>
                                                <asp:Label ID="lblProductGroup" runat="server"></asp:Label>
                                                <asp:Button ID="UI_cmdProductGroupPick" runat="server" Text="_Pick" CssClass="Pick"></asp:Button>
                                            </td>
                                            <td width="5%" align="right">
                                                <asp:Label ID="UI_lblWarrantyType" runat="server" Text="024_Warranty Type"></asp:Label>
                                            </td>
                                            <td align="left">:
                                                <asp:DropDownList ID="UI_cboWarrantyType" runat="server" Width="250" AutoPostBack="True"></asp:DropDownList>
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="UI_lblYears" runat="server" Text="032_Years"></asp:Label>
                                            </td>
                                            <td align="left">:
                                                <asp:TextBox ID="UI_Years" runat="server" Width="150" onblur="LongestYearsEnter();"></asp:TextBox>
                                                <asp:TextBox ID="UI_YearsOld" runat="server" Width="1" Style="display: none;"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="UI_lblVersion" runat="server" Text="028_Version"></asp:Label>
                                            </td>
                                            <td width="1" align="left">:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="UI_txtVersion" ReadOnly="true" CssClass="readyonly" runat="server" Width="150px" MaxLength="8"></asp:TextBox>
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="UI_lblDiscount" runat="server" Text="049_Discount"></asp:Label>
                                            </td>
                                            <td align="left">:
                                                <asp:TextBox ID="UI_txtDiscount" runat="server" Width="240"></asp:TextBox>%
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="UI_lblWar_longyy" runat="server" Text="037_Unit Price"></asp:Label>
                                            </td>
                                            <td align="left">:
                                                <asp:TextBox ID="UI_txtWar_longyy" runat="server" Width="150" onblur="LongestYearsEnter();"></asp:TextBox>
                                                <asp:TextBox ID="UI_txtWar_longyyOld" runat="server" Width="1" Style="display: none;"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="UI_lblProgramType" runat="server" Text="010_Project Type"></asp:Label>

                                            </td>
                                            <td width="1" align="left">:
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="UI_cboProgramType" runat="server" Visible="true" onchange="dropchange()"></asp:DropDownList>
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="UI_lblItemtype" runat="server" Text="009_Item Type"></asp:Label>
                                            </td>
                                            <td align="left">:
			                                    <asp:DropDownList ID="UI_cboItemType" runat="server" Visible="true" onchange="dropchange()"></asp:DropDownList>
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="UI_lblPriceVer" runat="server" Text="009_Price Version"></asp:Label>
                                            </td>
                                            <td align="left">:
                                                <asp:DropDownList ID="UI_cboPriceVer" runat="server" Visible="true" onchange="dropchange()"></asp:DropDownList>
                                            </td>

                                        </tr>

                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="lblExpDate" runat="server" Text="Expired Date"></asp:Label>
                                            </td>
                                            <td width="1" align="left">:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtExpDate" runat="server" Width="100"></asp:TextBox>
                                                <img style="vertical-align: middle" alt="" src="Images/icon_cal.gif" onclick="window.showModalDialog('Calendar.aspx',document.elementFromPoint(event.x-60,event.y),'dialogHeight:195px;dialogWidth:224px;dialogTop:'+(parseInt(window.event.screenY)+10)+';dialogLeft:'+window.event.screenX+';help:0;resizable:0;status:0;')" />
                                            </td>
                                            <td align="right">&nbsp;</td>
                                            <td align="left">
                                                <asp:Label ID="lbl_part_no" runat="server" Text="009_Part No"></asp:Label>
                                            </td>
                                            <td align="right"></td>
                                            <td align="left"></td>
                                        </tr>

                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="UI_lblDescription" runat="server" Text="036_Description"></asp:Label>
                                            </td>
                                            <td width="1" align="left">:
                                            </td>
                                            <td align="left" colspan="5">
                                                <asp:TextBox ID="UI_txtDescription" TextMode="MultiLine" Height="50" runat="server" Width="95%"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <asp:Panel ID="pnParts" runat="server" Width="100%">
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblPats037" runat="server" Text="Parts Type"></asp:Label>
                                                </td>
                                                <td width="1" align="left">:
                                                </td>
                                                <td align="left" colspan="5">
                                                    <asp:DropDownList ID="cboPartsType" runat="server"></asp:DropDownList>
                                                    <asp:Button ID="btnPartsAdd" runat="server" Text="Add" CssClass="Problem_Edit" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblPats038" runat="server" Text="Parts"></asp:Label>
                                                </td>
                                                <td width="1" align="left">:
                                                </td>
                                                <td align="left" colspan="5">
                                                    <table width="95%" border="1" cellspacing="0" cellpadding="0" class=" box_table">
                                                        <tr class="Text_Head">
                                                            <td width="30px">
                                                                <asp:Label ID="lblPartSeq" runat="server" Text="Seq"></asp:Label>
                                                            </td>
                                                            <td width="30%">
                                                                <asp:Label ID="lblPartContent" runat="server" Text="Warranty Content"></asp:Label>
                                                            </td>
                                                            <td width="20%">
                                                                <asp:Label ID="lblPartMonth" runat="server" Text="Warranty Month"></asp:Label>
                                                            </td>
                                                            <td width="20%">
                                                                <asp:Label ID="lblPartExtra" runat="server" Text="Extra Month"></asp:Label>
                                                            </td>
                                                            <td width="20%">
                                                                <asp:Label ID="lblPartMemo" runat="server" Text="Memo"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <asp:DataList ID="lstParts" runat="server" Width="100%" HorizontalAlign="left"
                                                            CellPadding="0" CellSpacing="0" RepeatLayout="Flow" RepeatColumns="1000000">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td style="text-align: center">
                                                                        <%# (Container.ItemIndex + 1).ToString("00") + "." %>
                                                                        <asp:Label ID="lstPartlblSeq" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "WAP_SEQ")%>'></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lstPartlblPart" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "WAP_NAME")%>'></asp:Label>
                                                                        <%# DataBinder.Eval(Container.DataItem, "WAP_NAME")%>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="lstParttxtMon" runat="server" Width="98%" Text='<%# DataBinder.Eval(Container.DataItem, "WAP_MON")%>'></asp:TextBox>
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:TextBox ID="lstParttxtExtMon" runat="server" Width="98%" Text='<%# DataBinder.Eval(Container.DataItem, "WAP_EMON")%>'></asp:TextBox>
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:TextBox ID="lstParttxtMemo" runat="server" Width="98%" Text='<%# DataBinder.Eval(Container.DataItem, "WAP_DESC")%>'></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:DataList>
                                                    </table>
                                                </td>
                                            </tr>
                                        </asp:Panel>


                                        <%--=============================================================================================================--%>

                                        <asp:Panel ID="pnSpec" runat="server" Width="100%">
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="Label1" runat="server" Text="Specs Type"></asp:Label>
                                                </td>
                                                <td width="1" align="left">:
                                                </td>
                                                <td align="left" colspan="5">
                                                    <asp:DropDownList ID="cboSpecsType" runat="server"></asp:DropDownList>
                                                    <asp:Button ID="btnSpecsAdd" runat="server" Text="Add" CssClass="Problem_Edit" />
                                                </td>
                                            </tr>



                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="Label2" runat="server" Text="Specs"></asp:Label>
                                                </td>
                                                <td width="1" align="left">:
                                                </td>
                                                <td align="left" colspan="5">
                                                    <table width="95%" border="1" cellspacing="0" cellpadding="0" class=" box_table">
                                                        <tr class="Text_Head">
                                                            <td width="30px">
                                                                <asp:Label ID="Label3" runat="server" Text="Seq"></asp:Label>
                                                            </td>
                                                            <td width="30%">
                                                                <asp:Label ID="Label4" runat="server" Text="Warranty Content"></asp:Label>
                                                            </td>
                                                            <td width="20%">
                                                                <asp:Label ID="Label5" runat="server" Text="Warranty Rule"></asp:Label>
                                                            </td>
                                                            <%--<td width="20%">
                                                                 <asp:Label ID="Label6" runat="server" Text="Extra Month"></asp:Label>
                                                            </td>
                                                            <td width="20%">
                                                                 <asp:Label ID="Label7" runat="server" Text="Memo"></asp:Label>
                                                            </td>--%>
                                                        </tr>
                                                        <asp:DataList ID="lstSpecs" runat="server" Width="100%" HorizontalAlign="left"
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
                                                                    <%--<td align="center">
                                                                        <asp:TextBox ID="lstSpectxtExtMon" runat="server" Width="98%" Text='<%# DataBinder.Eval(Container.DataItem, "WAP_EMON")%>'></asp:TextBox>
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:TextBox ID="lstSpectxtMemo" runat="server" Width="98%" Text='<%# DataBinder.Eval(Container.DataItem, "WAP_DESC")%>'></asp:TextBox>
                                                                    </td>--%>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:DataList>
                                                    </table>
                                                </td>
                                            </tr>
                                        </asp:Panel>
                                        <%--========================================================================--%>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="UI_lblWarrSpecDesc" runat="server" Text="037_Warranty_Spec_Desc"></asp:Label>
                                            </td>
                                            <td width="1" align="left">:
                                            </td>
                                            <td align="left" colspan="5">
                                                <asp:TextBox ID="UI_txtWarrSpecDesc" TextMode="MultiLine" Height="50" runat="server" Width="95%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <%--' 加入Warranty Card Content欄位 by buck add 20260128 begin--%>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="UI_lblWarCardContent" runat="server" Text="Warranty Card Content"></asp:Label>
                                            </td>
                                            <td width="1" align="left">:
                                            </td>
                                            <td align="left" colspan="5">
                                                <asp:TextBox ID="UI_txtWarCardContent" TextMode="MultiLine" Height="50" runat="server" Width="95%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <%--' 加入Warranty Card Content欄位 by buck add 20260128 end--%>
                                        <tr>
                                            <td colspan="7" align="center">
                                                <asp:Label ID="lblSwID" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lblSwIDStatus" runat="server" Visible="false"></asp:Label>
                                                <asp:Button ID="btnBackTop" runat="server" Text="Back" CssClass="Problem_Edit" />
                                                <asp:Button ID="UI_cmdSave" ValidationGroup="AddGroup" runat="server" Text="_Save" CssClass="Search" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <!--[End]新增資料-->
                        </table>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlFile" runat="server" Width="100%">
                <table width="100%" align="center" border="0" cellspacing="1" id="Step2" style="display: display">
                    <tr align="left">
                        <td height="27" background="images/pic_15.gif" class="default" align="left">
                            <b>Price Lists
                                                        <asp:CheckBox ID="chkOKAll" runat="server" OnCheckedChanged="chkOKAll_CheckedChanged" AutoPostBack="true" />All
                            </b>
                            <asp:Button ID="btnValidClient" runat="server" Text="Effect Customer" CssClass="Confirm" />
                            <asp:Button ID="btnInvalidAll" runat="server" OnClientClick="return confirm('Are your sure to clear the price?')" Text="Clear all price" CssClass="Confirm" />
                            <b>
                                <asp:CheckBox ID="chkCopyChina" runat="server" Text="Copy to CL_CHINA" />
                            </b>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" bgcolor="#E3D8BE" align="center">
                            <table id="table7" class="default" width="100%" cellspacing="1">
                                <tr class="Text_Head">
                                    <td class="Text_Head">Addition Year
                                    </td>
                                    <td width="9%">1st
                                    </td>
                                    <td width="9%">2nd
                                    </td>
                                    <td width="9%">3th
                                    </td>
                                    <td width="9%">4th
                                    </td>
                                    <td width="9%">5th
                                    </td>
                                    <td width="9%">6th
                                    </td>
                                    <td width="9%">7th
                                    </td>
                                    <td width="9%">8th
                                    </td>
                                    <td width="9%">9th
                                    </td>
                                    <td width="9%">10th
                                    </td>
                                </tr>
                                <tr class="Text_Head">
                                    <td class="Text_Head">Beginning Year
                                    </td>
                                    <td>Unit Price
                                    </td>
                                    <td>Unit Price
                                    </td>
                                    <td>Unit Price
                                    </td>
                                    <td>Unit Price
                                    </td>
                                    <td>Unit Price
                                    </td>
                                    <td>Unit Price
                                    </td>
                                    <td>Unit Price
                                    </td>
                                    <td>Unit Price
                                    </td>
                                    <td>Unit Price
                                    </td>
                                    <td>Unit Price
                                    </td>
                                </tr>
                                <tr align="center" class="TR_1">
                                    <td>1
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice10" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice11" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice12" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice13" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice14" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice15" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice16" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice17" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice18" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice19" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td>2
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice20" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice21" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice22" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice23" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice24" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice25" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice26" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice27" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice28" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice29" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="center" class="TR_1">
                                    <td>3
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice30" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice31" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice32" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice33" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice34" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice35" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice36" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice37" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice38" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice39" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td>4
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice40" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice41" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice42" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice43" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice44" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice45" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice46" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice47" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice48" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice49" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="center" class="TR_1">
                                    <td>5
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice50" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice51" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice52" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice53" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice54" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice55" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice56" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice57" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice58" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice59" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td>6
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice60" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice61" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice62" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice63" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice64" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice65" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice66" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice67" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice68" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice69" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="center" class="TR_1">
                                    <td>7
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice70" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice71" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice72" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice73" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice74" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice75" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice76" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice77" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice78" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice79" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td>8
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice80" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice81" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice82" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice83" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice84" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice85" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice86" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice87" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice88" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice89" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="center" class="TR_1">
                                    <td>9
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice90" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice91" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice92" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice93" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice94" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice95" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice96" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice97" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice98" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice99" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="center" class="TR_1">
                                    <td>10
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice100" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice101" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice102" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice103" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice104" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice105" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice106" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice107" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice108" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrice109" runat="server" Width="80px" Style="text-align: right;"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <p style="padding-top: 0px; padding-bottom: 20px;">
                                <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="Problem_Edit" />
                                <asp:Button ID="btnValid" runat="server" OnClientClick="return confirm('Are your sure?')" Text="Cancel" CssClass="Confirm" />
                                <asp:Button ID="btnSaveAll" ValidationGroup="AddGroup" runat="server" Text="_Save" CssClass="Confirm" />
                                <asp:Button ID="btnSubmitAll" ValidationGroup="AddGroup" runat="server" Text="_Save" CssClass="Confirm" />
                                <asp:Button ID="btnInValid" runat="server" OnClientClick="return confirm('Are your sure?')" Text="InValid" CssClass="Confirm" />
                            </p>
                        </td>
                    </tr>
                </table>

            </asp:Panel>
            <uc2:ucMessage ID="ucMessage" runat="server" />
            <uc6:ucProductGroup ID="ucProductGroup" runat="server"></uc6:ucProductGroup>
            <uc8:ucValidCustomer ID="ucValidCustomer" runat="server"></uc8:ucValidCustomer>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="UI_cmdSave" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="UI_cmdProductGroupPick" EventName="Click" />
            <asp:PostBackTrigger ControlID="ucMessage" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:RequiredFieldValidator ID="rfvMonth" runat="server" ErrorMessage="001_Extra Months not allow null" ControlToValidate="UI_ExtraMonths" Display="None" TabIndex="0" ValidationGroup="AddGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="rfvProductGroup" runat="server" ErrorMessage="002_Product Group allow null" ControlToValidate="UI_txtProductGroup" Display="None" TabIndex="0" ValidationGroup="AddGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="rfvWarrName" runat="server" ErrorMessage="003_Warranty Name not allow null" ControlToValidate="UI_txtWarrantyName" Display="None" TabIndex="0" ValidationGroup="AddGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="rfvDiscount" runat="server" ErrorMessage="004_Years not allow null" ControlToValidate="UI_txtDiscount" Display="None" TabIndex="0" ValidationGroup="AddGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="rfvLongestYears" runat="server" ErrorMessage="005_Unit Price not allow null" ControlToValidate="UI_txtWar_longyy" Display="None" TabIndex="0" ValidationGroup="AddGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="rfvStandardYears" runat="server" ErrorMessage="005_Unit Price not allow null" ControlToValidate="UI_Years" Display="None" TabIndex="0" ValidationGroup="AddGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="rfvITEM_TYPE" runat="server" ErrorMessage="006_Item Type not allow null" ControlToValidate="UI_cboItemType" InitialValue="-1" Display="None" TabIndex="0" ValidationGroup="AddGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="rfvPROGRAM_TYPE" runat="server" ErrorMessage="007_Program Type not allow null" ControlToValidate="UI_cboProgramType" InitialValue="-1" Display="None" TabIndex="0" ValidationGroup="AddGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="rfvPRICE_VER" runat="server" ErrorMessage="008_Price Ver not allow null" ControlToValidate="UI_cboPriceVer" InitialValue="-1" Display="None" TabIndex="0" ValidationGroup="AddGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
    <asp:ValidationSummary ID="vsadd" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="AddGroup" />
</asp:Content>
