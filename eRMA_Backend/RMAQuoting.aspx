<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="RMAQuoting.aspx.vb" Inherits="RMAQuoting" Title="Untitled Page" ValidateRequest="false" %>

<%@ PreviousPageType VirtualPath="Repair_WorkList.aspx" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register Src="ascx/ucProgressStatus.ascx" TagName="ucProgressStatus" TagPrefix="uc1" %>
<%@ Register Src="ascx/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>
<%@ Register Src="ascx/ucClientDetailPur.ascx" TagName="ucClientDetailPur" TagPrefix="uc4" %>
<%@ Register Src="ascx/ucSpecialSetting.ascx" TagName="ucSpecialSetting" TagPrefix="uc8" %>
<%@ Register Src="ascx/ucRepairRarts.ascx" TagName="ucRepairRarts" TagPrefix="uc5" %>
<%--<%@ Register Src="ascx/UcSDCView.ascx" TagName="UcSDCView" TagPrefix="uc9" %>--%>
<%@ Register Src="ascx/UcSDCViewG.ascx" TagName="UcSDCViewG" TagPrefix="uc10" %>
<%@ Register Src="ascx/UcWarrantyView.ascx" TagName="UcWarrantyView" TagPrefix="uc11" %>
<%@ Register Src="ascx/ucClientDetail.ascx" TagName="ucClientDetail" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <script type="text/javascript" language="javascript">
        function FrmDelete() {
            if (confirm(delMsg)) {
                onProgress('Delete')
                return true;
            }
            return false;
        }

        function SerialSearch() {
            if (event.keyCode == 13) {
                var vs = document.getElementById('ctl00_ContentPlaceHolder_btnQuickSearch');
                vs.focus();
                vs.click();
            }
        }

        function cal_subTotalAMT() {
            var blnFlag_Cal = false;
            var RMARQD_PRICE = 0;
            var iTotalParts = 0;

            var UI_ImproPerusage = window.document.getElementById("ctl00_ContentPlaceHolder_UI_opgImproPerusage_1").checked;
            var UI_RMAD_ISCW = window.document.getElementById('<%=uiTxt_RMAD_ISCW.ClientID %>').value;
            var UI_chkWarranty_1 = window.document.getElementById('<%=UI_chkWarranty_1.ClientID %>').checked;
            var UI_chkWarranty_C1 = window.document.getElementById('<%=UI_chkWarranty_C1.ClientID %>').checked;

            var oGridView = document.getElementById('<%=UI_dvRepairDetail.ClientID %>');
            var oGridViewID = oGridView.id;
            var iRows = oGridView.rows.length;
            //alert(iRows);
            for (var i = 1; i < iRows; i++) {

                if (i <= 9) {
                    var chkParts = document.getElementById(oGridViewID + '_ctl0' + i + '_chkParts');
                    var chhWaive = document.getElementById(oGridViewID + '_ctl0' + i + '_chhWaive');
                    var oQty = document.getElementById(oGridViewID + '_ctl0' + i + '_txtQty');
                    var oMaterialCost = document.getElementById(oGridViewID + '_ctl0' + i + '_UI_txtMaterialCost');
                    var UI_txtRMARQD_PRICE = document.getElementById(oGridViewID + '_ctl0' + i + '_UI_txtRMARQD_PRICE');
                    var oPrice = document.getElementById(oGridViewID + '_ctl0' + i + '_txtPrice');
                } else {
                    var chkParts = document.getElementById(oGridViewID + '_ctl' + i + '_chkParts');
                    var chhWaive = document.getElementById(oGridViewID + '_ctl' + i + '_chhWaive');
                    var oQty = document.getElementById(oGridViewID + '_ctl' + i + '_txtQty');
                    var oMaterialCost = document.getElementById(oGridViewID + '_ctl' + i + '_UI_txtMaterialCost');
                    var UI_txtRMARQD_PRICE = document.getElementById(oGridViewID + '_ctl' + i + '_UI_txtRMARQD_PRICE');
                    var oPrice = document.getElementById(oGridViewID + '_ctl' + i + '_txtPrice');
                }
                
                //保固内且不是使用不当，允许收零件费用
                if ((UI_chkWarranty_1 == true || UI_chkWarranty_C1 == true) && UI_ImproPerusage == true) {
                    chkParts.disabled = false;
                } else {
                    chkParts.disabled = true;
                }


                if (oQty != null && oMaterialCost != null) {
                    if (isNaN(oQty.value) != true && isNaN(oMaterialCost.value) != true) {
                        var RMARQD_QTY = parseFloat(oQty.value);
                        var RMARQD_MATERIALCOST = parseFloat(oMaterialCost.value);

                        RMARQD_PRICE = PartsRule_Exception(RMARQD_QTY, RMARQD_MATERIALCOST, chhWaive, chkParts);

                        UI_txtRMARQD_PRICE.value = RMARQD_PRICE;
                        if (oPrice != null) {
                            oPrice.innerHTML = RMARQD_PRICE;
                        }

                        iTotalParts = iTotalParts + RMARQD_PRICE;

                        blnFlag_Cal = true;
                    }
                }

            }

            //alert(RMARQD_PRICE);

            //Service Charge 金額 
            var UI_lblServiceCharge = window.document.getElementById('<%=UI_lblServiceCharge.ClientID %>');         //人工維修費用(Service Charge 金額)
            var uiTxt_Repair_ManHour = window.document.getElementById('<%=uiTxt_Repair_ManHour.ClientID %>');       //人工維修費用(Service Charge 金額)
            var uiLbl_Repair_LaborCost = window.document.getElementById('<%=uiLbl_Repair_LaborCost.ClientID %>');   //人工維修費用(Service Charge 金額 * 人工每小時單價)

            var iServiceCharge = ServiceChargeRule_Exception();
            //UI_lblServiceCharge.innerHTML = Round(iServiceCharge,2);
            uiTxt_Repair_ManHour.value = Round(iServiceCharge, 2);
            uiLbl_Repair_LaborCost.innerHTML = Round(iServiceCharge, 2);


            //報價零件加總金額
            var uiLbl_Repair_PartsTotal = window.document.getElementById('<%=uiLbl_Repair_PartsTotal.ClientID %>');
            var uiTxt_Repair_PartsTotal = window.document.getElementById('<%=uiTxt_Repair_PartsTotal.ClientID %>');
            uiLbl_Repair_PartsTotal.innerHTML = Round(iTotalParts, 2);
            uiTxt_Repair_PartsTotal.value = Round(iTotalParts, 2);


            //總金額 (Service Charge + 報價零件加總金額)
            var uiLbl_Repair_Total = window.document.getElementById('<%=uiLbl_Repair_Total.ClientID %>');
            uiLbl_Repair_Total.innerHTML = Round(iServiceCharge + iTotalParts, 2);

        }


        //領件費用, 例外規格計算
        function PartsRule_Exception(RMARQD_QTY, RMARQD_MATERIALCOST, chhWaive, chkParts) {
            var RMARQD_PRICE = 0;
            //若rmad_iscw=’1’ , 並且Improper Usage = N  , 則Parts合計金額=0 ,各項次list price不變.  Insert 業務報價時parts合計=0
            //if (UI_RMAD_ISCW=="1" && UI_ImproPerusage==true){
            //     iTotalParts=0;
            //}


            //計算折扣後的零件金額
            var CU_DISCOUNT_OFF = 1;
            CU_DISCOUNT_OFF = Trim(window.document.getElementById('<%=UI_CU_DISCOUNT_OFF.ClientID %>').value);
            if (CU_DISCOUNT_OFF != "") {
                CU_DISCOUNT_OFF = parseFloat(CU_DISCOUNT_OFF);
            }

            if (RMARQD_QTY > 0 && RMARQD_MATERIALCOST > 0) {
                RMARQD_PRICE = Round((RMARQD_QTY * RMARQD_MATERIALCOST) * CU_DISCOUNT_OFF, 2);
            }


            //'1. IF (ISWARRANTY=Y OR ISCW=Y) THEN RMARQD_PRICE = 0
            var UI_ImproPerusage = window.document.getElementById("ctl00_ContentPlaceHolder_UI_opgImproPerusage_1").checked;
            var UI_chkWarranty_1 = window.document.getElementById('<%=UI_chkWarranty_1.ClientID %>').checked;
            var UI_chkWarranty_C1 = window.document.getElementById('<%=UI_chkWarranty_C1.ClientID %>').checked;

            //保固日期之内，非人为，零件不需要额外收费才给0金额。
            if ((UI_chkWarranty_1 == true || UI_chkWarranty_C1 == true) && UI_ImproPerusage == true) {
                if (chkParts != null && chkParts.checked == false) {
                    RMARQD_PRICE = 0;
                }
            }

            //'2. 客戶編號為'Ni.'開頭的, RMARQD_PRICE = 0
            Customer_ExceptionCharge = Trim(window.document.getElementById('<%=UI_Customer_ExceptionCharge.ClientID %>').value);
            UI_CUNO = Trim(window.document.getElementById('<%=UI_CUNO.ClientID %>').value);
            var arrItem = Customer_ExceptionCharge.split(',');
            for (var i = 0; i < arrItem.length; i++) {
                if (UI_CUNO.indexOf(arrItem[i]) != -1) {
                    RMARQD_PRICE = 0
                    break;
                }
            }

            //waive：表示此零件是我方吸收必修，維修收費價格會是0；
            if (chhWaive != null) {
                if (chhWaive.checked == true) {
                    RMARQD_PRICE = 0;
                }
            }


            return RMARQD_PRICE;
        }


        function ServiceChargeRule_Exception() {
            var iServiceCharge = 0;
            var sFlowCase = "";
            sFlowCase = "F" + Trim(window.document.getElementById('<%=UI_flowCase.ClientID %>').value);

            var uiTxt_Repair_ManHour = window.document.getElementById('<%=uiTxt_Repair_ManHour.ClientID %>');
            uiTxt_Repair_ManHour.disabled = false;

            var UI_CU_SERVICE_CHG = Trim(window.document.getElementById('<%=UI_CU_SERVICE_CHG.ClientID %>').value);

            switch (sFlowCase) {
                case "F01":
                    if (UI_CU_SERVICE_CHG != "") {
                        iServiceCharge = parseFloat(UI_CU_SERVICE_CHG);
                    }
                    break;

                case "F02":
                    if (parseFloat(uiTxt_Repair_ManHour.value) == 0) {
                        if (UI_CU_SERVICE_CHG != "") {
                            iServiceCharge = parseFloat(UI_CU_SERVICE_CHG);
                        }
                    } else {
                        iServiceCharge = parseFloat(uiTxt_Repair_ManHour.value);
                    }


//            var UI_lblServiceCharge = window.document.getElementById('<%=UI_lblServiceCharge.ClientID %>');   
                    //            if (parseFloat(UI_lblServiceCharge.innerHTML)==0){
                    //                if (UI_CU_SERVICE_CHG!=""){
                    //                    iServiceCharge = parseFloat(UI_CU_SERVICE_CHG);
                    //                }
                    //            }else{
                    //                //iServiceCharge = parseFloat(UI_lblServiceCharge.innerHTML);
                    //                iServiceCharge = parseFloat(uiTxt_Repair_ManHour.value);
                    //            }
                    break;

                default:
                    if (UI_CU_SERVICE_CHG != "") {
                        iServiceCharge = parseFloat(UI_CU_SERVICE_CHG);
                    }
                    break;
            }


            //保固中and 非人為 service Charge=0
            //若rmad_iscw=’1’ , 並且Improper Usage = N  , 則Parts合計金額=0 ,各項次list price不變.  Insert 業務報價時parts合計=0
            //'1. IF ISWARRANTY=Y OR ISCW=Y THEN SERVICE CHARGE = 0  
            var UI_ImproPerusage = window.document.getElementById("ctl00_ContentPlaceHolder_UI_opgImproPerusage_1").checked;
            var UI_chkWarranty_1 = window.document.getElementById('<%=UI_chkWarranty_1.ClientID %>').checked;
            var UI_chkWarranty_C1 = window.document.getElementById('<%=UI_chkWarranty_C1.ClientID %>').checked;
            if ((UI_chkWarranty_1 == true || UI_chkWarranty_C1 == true) && UI_ImproPerusage == true) {
                iServiceCharge = 0;
                uiTxt_Repair_ManHour.disabled = true;
            }


            //'2. 客戶編號為'Ni.'開頭的, SERVICE CHARGE = 0  
            Customer_ExceptionCharge = Trim(window.document.getElementById('<%=UI_Customer_ExceptionCharge.ClientID %>').value);
            UI_CUNO = Trim(window.document.getElementById('<%=UI_CUNO.ClientID %>').value);
            var arrItem = Customer_ExceptionCharge.split(',');
            for (var i = 0; i < arrItem.length; i++) {
                if (UI_CUNO.indexOf(arrItem[i]) != -1) {
                    iServiceCharge = 0
                    break;
                }
            }

            return iServiceCharge;
        }



        function cal_TotalAMT() {
            var blnFlag_Cal = false;
            var iServiceCharge = 0;

            //Service Charge 金額 
            var UI_lblServiceCharge = window.document.getElementById('<%=UI_lblServiceCharge.ClientID %>');         //人工維修費用(Service Charge 金額)
            var uiTxt_Repair_ManHour = window.document.getElementById('<%=uiTxt_Repair_ManHour.ClientID %>');       //人工維修費用(Service Charge 金額)
            var uiLbl_Repair_LaborCost = window.document.getElementById('<%=uiLbl_Repair_LaborCost.ClientID %>');   //人工維修費用(Service Charge 金額 * 人工每小時單價)

            iServiceCharge = parseFloat(uiTxt_Repair_ManHour.value, 2);
            //UI_lblServiceCharge.innerHTML = Round(iServiceCharge,2);
            uiLbl_Repair_LaborCost.innerHTML = Round(iServiceCharge, 2);

            var iTotalParts = 0;
            var UI_txtPartsTotal = window.document.getElementById('<%=uiTxt_Repair_PartsTotal.ClientID %>').value;
            if (UI_txtPartsTotal != "") {
                if (isNaN(UI_txtPartsTotal) != true) {
                    iTotalParts = parseFloat(UI_txtPartsTotal);
                    iTotalParts = Round(iTotalParts, 2);
                    blnFlag_Cal = true;
                }
            }

            var UI_lblTotal = window.document.getElementById('<%=uiLbl_Repair_Total.ClientID %>');
            if (blnFlag_Cal == true) {
                UI_lblTotal.innerHTML = Round(iServiceCharge + iTotalParts, 2);
            }

        }

        function Round(value, dights) {
            var nNewValue = Math.round(value * Math.pow(10, dights)) / Math.pow(10, dights);
            return nNewValue;
        }

        function Validate_FailureClass(oSrc, args) {
            var sValue1 = document.getElementById('<%=UI_cboFailureClass.ClientID %>').value;

            if (sValue1 == -1) {
                args.IsValid = false;
            } else {
                args.IsValid = true;
            }
        }

        function Validate_Failure(oSrc, args) {
            var sValue1 = document.getElementById('<%=UI_cboFailure.ClientID %>').value;

            if (sValue1 == -1) {
                args.IsValid = false;
            } else {
                args.IsValid = true;
            }
        }

        function Validate_Repair(oSrc, args) {
            var sValue1 = document.getElementById('<%=UI_cboAssignRepair.ClientID %>').value;

            if (sValue1 == -1) {
                args.IsValid = false;
            } else {
                args.IsValid = true;
            }
        }


        function chkWarranty(objName, isCalTotalAMT) {
            debugger;
            objName.checked = false;

            if (isCalTotalAMT == true) {
                cal_subTotalAMT();
            }
        }

        function opgImproPerusage() {
            cal_subTotalAMT();
        }

    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="UI_panel" runat="server">
                <fieldset class="form_div" valign="top" style="width: 100%">
                    <table border="0" bgcolor="#E3D8BE" width="100%" id="table2" cellspacing="1" cellpadding="0"
                        class="TableListdownright" align="center">
                        <tr>
                            <td align="left" colspan="4" background="Images/pic_15.gif" class="default">&nbsp;
                                <asp:Label ID="UI_QuoteInformation" runat="server" Text="063_Quote Information" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="default">&nbsp;&nbsp;
                                <asp:Label ID="UI_lblModelNo" runat="server" Text="012_ModelNo" Font-Bold="true"></asp:Label>
                            </td>
                            <td align="left" class="default" colspan="3">:
                                <asp:Label ID="UI_lblModelNoText" runat="server" Font-Bold="true" ForeColor="red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="default" width="20%">&nbsp;&nbsp;
                                <asp:Label ID="UI_lblSerial" runat="server" Text="013_Serial Number" Font-Bold="true"></asp:Label>
                            </td>
                            <td align="left" class="default" width="40%">:
                                <asp:Label ID="UI_lblShowSerial" runat="server" Font-Bold="true"></asp:Label>
                                <asp:TextBox ID="UI_lblSerialText" runat="server" AutoPostBack="false" Visible="false"></asp:TextBox>
                            </td>
                            <td align="left" class="default" width="12%">
                                <asp:Label ID="UI_lblRepair" runat="server" Text="009_Repair Center" Font-Bold="true"></asp:Label>
                            </td>
                            <!-- 原本維修中心 -->
                            <td align="left" class="default" width="30%">:
                                <asp:Label ID="UI_lblRepairText" runat="server" Font-Bold="true"></asp:Label>
                                <asp:Label ID="UI_lblRepairNo" runat="server" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="default">
                                <font color="#ff0000">*</font>
                                <asp:Label ID="UI_lblFailureClass" runat="server" Text="023_Failure Reason" Font-Bold="true"></asp:Label>
                            </td>
                            <td align="left" class="default">:
                                <asp:DropDownList ID="UI_cboFailureClass" runat="server" AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:DropDownList ID="UI_cboFailure" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td align="left" class="default">
                                <font color="#ff0000">*</font>
                                <asp:Label ID="UI_lblAssign" runat="server" Text="052_Assign to" Font-Bold="true"></asp:Label>
                            </td>
                            <!-- 指派的維修中心-->
                            <td align="left" class="default">:
                                <asp:DropDownList ID="UI_cboAssignRepair" runat="server" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="default">&nbsp;
                                <asp:Label ID="UI_lblWarranty" runat="server" Text="015_Warranty" Font-Bold="true"></asp:Label>
                            </td>
                            <td align="left" class="default">:
                                <asp:CheckBox runat="server" ID="UI_chkWarranty_1" Text="065_Yes" />
                                <asp:CheckBox runat="server" ID="UI_chkWarranty_0" Text="066_No" />
                            </td>

                            <td align="left" colspan="2" rowspan="9" class="default">
                                <table width="95%" border="1" cellspacing="0" cellpadding="0" class=" box_table">
                                    <tr class="Text_Head">
                                        <td width="5%">
                                            <asp:Label ID="lblPartSeq" runat="server" Text="#"></asp:Label>
                                        </td>
                                        <td width="15%">
                                            <asp:Label ID="lblPartPO" runat="server" Text="PO Date"></asp:Label>
                                        </td>
                                        <td width="20%">
                                            <asp:Label ID="lblPartContent" runat="server" Text="Content"></asp:Label>
                                        </td>
                                        <td width="15%">
                                            <asp:Label ID="lblPartMonth" runat="server" Text="Month"></asp:Label>
                                        </td>
                                        <td width="15%">
                                            <asp:Label ID="lblPartExtra" runat="server" Text="Extra"></asp:Label>
                                        </td>
                                        <td width="15%">
                                            <asp:Label ID="lblPartMemo" runat="server" Text="Memo"></asp:Label>
                                        </td>
                                        <td width="15%">
                                            <asp:Label ID="lblPartEndDate" runat="server" Text="End Date"></asp:Label>
                                        </td>
                                    </tr>
                                    <asp:DataList ID="lstParts" runat="server" Width="100%" HorizontalAlign="left"
                                        CellPadding="0" CellSpacing="0" RepeatLayout="Flow" RepeatColumns="1000000">
                                        <ItemTemplate>
                                            <tr>
                                                <td style="text-align: center">
                                                    <%# (Container.ItemIndex + 1).ToString("00") + "." %>
                                                </td>
                                                <td align="center">
                                                    <%# DataBinder.Eval(Container.DataItem, "PODate")%>
                                                </td>
                                                <td>
                                                    <%# DataBinder.Eval(Container.DataItem, "TYPE_NAME")%>
                                                </td>
                                                <td align="center">
                                                    <%# DataBinder.Eval(Container.DataItem, "WAP_MON")%>
                                                </td>
                                                <td align="center">
                                                    <%# DataBinder.Eval(Container.DataItem, "WAP_EMON")%>
                                                </td>
                                                <td align="center">
                                                    <%# DataBinder.Eval(Container.DataItem, "WAP_DESC")%>
                                                </td>
                                                <td align="center">
                                                    <font color="red"><%# DataBinder.Eval(Container.DataItem, "WarrEndDate")%></font>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:DataList>

                                    <tr align="left">
                                        <td colspan="7">
                                            <uc10:UcSDCViewG ID="UcSDCViewG" runat="server" />

                                        </td>
                                    </tr>

                                    <%--<tr align ="left" >
                                    <td colspan="7" >
                                    
                                        <uc11:UcWarrantyView ID="UcWarrantyView" runat="server"  />
                                          
                                     </td>
                                </tr>--%>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="default">&nbsp;
                                <asp:Label ID="UI_lblCWarranty" runat="server" Text="Comprehensive" Font-Bold="true"></asp:Label>
                            </td>
                            <td align="left" class="default">:
                                <asp:CheckBox runat="server" ID="UI_chkWarranty_C1" Text="Yes" />
                                <asp:CheckBox runat="server" ID="UI_chkWarranty_C0" Text="No" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="default">&nbsp;
                                <asp:Label ID="UI_lblSWarranty" runat="server" Text="Special" Font-Bold="true"></asp:Label>
                            </td>
                            <td align="left" class="default">:
                                <asp:CheckBox runat="server" ID="UI_chkWarranty_S1" Text="Yes" />
                                <asp:CheckBox runat="server" ID="UI_chkWarranty_S0" Text="No" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="default">
                                <font color="#ff0000">*</font>
                                <asp:Label ID="UI_lblImproperUsage" runat="server" Text="064_Improper Usage" Font-Bold="true"></asp:Label>
                            </td>
                            <td align="left" class="default">:
                                <asp:RadioButtonList ID="UI_opgImproPerusage" runat="server" RepeatDirection="Horizontal"
                                    RepeatLayout="Flow">
                                    <asp:ListItem Value="1" Text="065_Yes"></asp:ListItem>
                                    <asp:ListItem Value="0" Text="066_No" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:TextBox ID="uiTxt_RMAD_ISCW" runat="server" Width="10px" Style="display: none; width: 1px"></asp:TextBox>
                            </td>                        
                        </tr>
                        <%-- 需求新增:BI保固 By buck Add 20250902 begin --%>
                        <tr id="UI_BI_Row" runat ="server">
                            <td align="left" class="default">&nbsp;
                                <asp:Label ID="UI_lblApplyBatteryInsurance" runat="server" Text="222_Apply Battery Insurance" Font-Bold="true"></asp:Label>
                            </td>
                            <td align="left" class="default">:
                                <asp:RadioButtonList ID="UI_opgApplyBatteryInsurance" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                    RepeatLayout="Flow">
                                    <asp:ListItem Value="1" Text="065_Yes"></asp:ListItem>
                                    <asp:ListItem Value="0" Text="066_No" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:Label ID="uiTxt_ApplyBatteryInsurance" runat="server" Text="223_Apply Battery Insurance" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <%--需求新增:BI保固 By buck Add 20250902 end --%>
                        <tr style="display: none">
                            <td align="left" class="default">&nbsp;
                                <asp:Label ID="UI_lblProductDesc" runat="server" Text="196_Product Description" Font-Bold="true"></asp:Label>
                            </td>
                            <td align="left" class="default">:
                                <asp:Label ID="UI_txtProductDesc" runat="server" Width="300px" ReadOnly="true"></asp:Label>
                            </td>
                        </tr>

                        <tr>

                            <td align="left" class="default">&nbsp;&nbsp;
                                <asp:Label ID="Insurance_Label" runat="server" Text="Apply Total Loss Insurance" Font-Bold="true"></asp:Label>

                            </td>
                            <td align="left" class="default">
                                <asp:Label ID="Apply_Label" runat="server" Text=":"></asp:Label>
                                <asp:RadioButtonList ID="UI_Apply_Total_Loss_Insurance" runat="server" RepeatDirection="Horizontal"
                                    RepeatLayout="Flow">
                                    <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                    <asp:ListItem Value="0" Text="No" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>

                        <%--電池保固 開始--%>
                        <tr>
                            <td align="left" class="default">&nbsp;&nbsp;
                            <asp:Label ID="RMAD_RMANO" runat="server" Text="Standard Battery" Font-Bold="true"></asp:Label>
                            </td>
                            <td align="left" class="default">

                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButtonList ID="rblBI" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                                RepeatLayout="Flow">
                                                <asp:ListItem Value="1" Text="Yes" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td>
                                            <asp:Panel ID="plBI" runat="server">
                                                <asp:Label ID="LabRMAD_RMANO_UI" runat="server" Text=":"></asp:Label>
                                                <asp:TextBox ID="txtRMAD_RMANO_QTY" runat="server" Width="45" placeholder="Qty"></asp:TextBox>
                                                <asp:Label ID="LabRMAD_RMANO_QTY" runat="server" Text=""></asp:Label>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <%--電池保固 結束--%>

                        <tr>
                            <td align="left" class="default">
                                <font color="#ff0000">*</font>
                                <asp:Label ID="UI_lblProblemDesc" runat="server" Text="025_Problem Description" Font-Bold="true"></asp:Label>
                            </td>
                            <td align="left" class="default">:
                                <asp:TextBox ID="UI_txtProblemDesc" runat="server" TextMode="MultiLine" Rows="3"
                                    Columns="70"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="default">
                                <font color="#ff0000">*</font>
                                <asp:Label ID="UI_lblRepairDesc" runat="server" Text="053_Repair Description" Font-Bold="true"></asp:Label>
                            </td>
                            <td align="left" class="default">:
                                <asp:TextBox ID="UI_txtRepairDesc" runat="server" TextMode="MultiLine" Rows="3" Columns="70"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="default">&nbsp;&nbsp;
                                <asp:Label ID="UI_lblRepairMemo" runat="server" Text="054_Repair Memo" Font-Bold="true"></asp:Label>
                            </td>
                            <td align="left" class="default">:
                                <asp:TextBox ID="UI_txtRepairMemo" runat="server" TextMode="MultiLine" Rows="3" Columns="70"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="default">&nbsp;&nbsp;
                                <asp:Label ID="UI_lblEditor" runat="server" Text="060_Editor" Font-Bold="true"></asp:Label>
                            </td>
                            <td align="left" class="default">:
                                <%--<asp:Label ID="UI_lblEditorText" runat ="server" Text="061_TW Repair Center-Taipei"></asp:Label>--%>
                                &nbsp;&nbsp;&nbsp;
                                <asp:Label ID="UI_lblEditName" runat="server" Text=""></asp:Label>&nbsp;&nbsp;
                                <asp:Label ID="UI_lblLuStmp" runat="server"></asp:Label>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="4">
                                <table border="0" width="100%" cellspacing="0" cellpadding="0">
                                    <tr height="20px">
                                        <td width="24" background="Images/pic_14.gif">&nbsp;</td>
                                        <td background="Images/pic_15.gif">
                                            <table border="0" width="100%" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td width="1%">&nbsp;</td>
                                                    <td align="left" class="default">
                                                        <asp:Label ID="UI_lblInformationTittle" runat="server" Text="082_Replace Component" Font-Bold="true"></asp:Label>
                                                    </td>
                                                    <td align="right" class="default">&nbsp;</td>
                                                    <td width="2%">&nbsp;</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>

                                </table>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="4">
                                <table id="table6" class="default" cellspacing="0" cellpadding="3" width="100%" border="0">
                                    <tr>
                                        <td bgcolor="#ffeeb2" align="left">
                                            <asp:Panel runat="server" ID="UI_Panel_AddPart" DefaultButton="UI_cmdSearch">
                                                <asp:Label ID="uiLbl_Repair_Model" runat="server" Text="012_Model No" Visible="false" class="default"></asp:Label><%--.:--%>
                                                <asp:TextBox ID="uiTxt_Repair_Model" runat="server" Width="100" Visible="false" class="default"></asp:TextBox>&nbsp;&nbsp;&nbsp;
            
            <%--<font color="#ff0000">*</font>--%>
                                                <asp:Label ID="uiLbl_Repair_PartsNo" runat="server" Text="083_Part's No" class="default"></asp:Label>.:
                                                <asp:TextBox ID="uiTxt_Repair_PartsNo" runat="server" Width="100" MaxLength="13" class="default"></asp:TextBox>&nbsp;&nbsp;&nbsp;
            
                                                <asp:Label ID="uiLbl_Repair_Location" runat="server" Text="084_Location" Visible="false" class="default"></asp:Label><%--:--%>
                                                <asp:TextBox ID="uiTxt_Repair_Location" runat="server" Width="80" Visible="false" class="default"></asp:TextBox>&nbsp;&nbsp;&nbsp;

                                                <asp:Button ID="UI_cmdSearch" runat="server" CssClass="Confirm_l" Text="Search" ValidationGroup="Repair_AddPart" />&nbsp;&nbsp;&nbsp;
                                                <asp:Button ID="UI_cmdParts_Search" runat="server" Text="078_Search" CssClass="Confirm" CausesValidation="false" OnClick="UI_cmdParts_Search_Click" />

                                                <asp:RequiredFieldValidator ID="rfvPartNo" runat="server" ErrorMessage="107_請輸入New Part No" Display="None" TabIndex="0" ControlToValidate="uiTxt_Repair_PartsNo"
                                                    ValidationGroup="Repair_AddPart" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            </asp:Panel>
                                        </td>
                                        <td bgcolor="#ffeeb2">
                                            <asp:Label ID="uiTag_Repair_Currency" runat="server" Text="085_Currency" class="default"></asp:Label>:&nbsp;
                                            <asp:Label ID="uiLbl_Repair_CurrencyCode" runat="server" class="default"></asp:Label>
                                        </td>
                                    </tr>
                                </table>

                            </td>
                        </tr>

                        <tr>
                            <td colspan="4" class="default">

                                <asp:DataList ID="UI_dvRepairDetail" runat="server" ExtractTemplateRows="true" BorderColor="#c0c0c0"
                                    CellSpacing="0" CellPadding="3" Width="100%" border="1" bordercolorlight="#c0c0c0">
                                    <HeaderTemplate>
                                        <asp:Table ID="oTableHeader" runat="server">
                                            <asp:TableHeaderRow bgcolor="#fff4d0">
                                                <asp:TableHeaderCell Width="2%">&nbsp;</asp:TableHeaderCell>
                                                <asp:TableHeaderCell Width="3%"> <asp:Label ID="lblParts" runat="server" Text="Parts"></asp:Label></asp:TableHeaderCell>
                                                <asp:TableHeaderCell Width="3%"> <asp:Label ID="lblWaive" runat="server" Text="405_Waive"></asp:Label></asp:TableHeaderCell>
                                                <asp:TableHeaderCell Width="3%"> <asp:Label ID="lblOption" runat="server" Text="406_Option"></asp:Label></asp:TableHeaderCell>
                                                <asp:TableHeaderCell Width="6%"> <asp:Label ID="lblHPart" runat="server" Text="083_Part'sNo"></asp:Label></asp:TableHeaderCell>
                                                <asp:TableHeaderCell Width="10%" Visible="false"> <asp:Label ID="lblHSerial" runat="server" Text="098_Serial No"></asp:Label></asp:TableHeaderCell>
                                                <asp:TableHeaderCell Width="28%"> <asp:Label ID="lblHDescription" runat="server" Text="099_Description"></asp:Label></asp:TableHeaderCell>
                                                <asp:TableHeaderCell Width="10%"> <asp:Label ID="lblHLocation" runat="server" Text="100_SMT Location"></asp:Label></asp:TableHeaderCell>
                                                <asp:TableHeaderCell Width="8%"> <asp:Label ID="lblHImproper" runat="server" Text="101_Improper Usage"></asp:Label></asp:TableHeaderCell>
                                                <asp:TableHeaderCell Width="19%"> <asp:Label ID="lblHReason" runat="server" Text="102_Defective Reason"></asp:Label></asp:TableHeaderCell>
                                                <asp:TableHeaderCell Width="5%"> <asp:Label ID="lblHQty" runat="server" Text="103_Qty"></asp:Label></asp:TableHeaderCell>
                                                <asp:TableHeaderCell Width="6%"> <asp:Label ID="lblHPrice" runat="server" Text="104_Price"></asp:Label></asp:TableHeaderCell>
                                                <asp:TableHeaderCell Width="3%"> <asp:Label ID="lblHDel" runat="server" Text="017_Delete"></asp:Label></asp:TableHeaderCell>
                                            </asp:TableHeaderRow>
                                        </asp:Table>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <asp:Table ID="oTableRow" runat="server">
                                            <asp:TableRow>
                                                <asp:TableCell HorizontalAlign="Center"> <asp:Label ID="lblSeq" runat="server" class="default" Text='<%# Container.ItemIndex+1 %>'></asp:Label><asp:Label ID="lblNew" runat="server" Text="_New" class="default" Visible="false"></asp:Label><asp:Label ID="lblRMARQDID" runat="server" Text='<%# Eval("RMARQD_ID") %>' class="default" Visible="false"></asp:Label><asp:Label ID="lblIMPROPERUSAGE" runat="server" Text='<%# Eval("RMARQD_IMPROPERUSAGE") %>' class="default" Visible="false"></asp:Label><asp:Label ID="lblDEFECTIVE" runat="server" Text='<%# Eval("RMARQD_DEFECTIVE") %>' class="default" Visible="false"></asp:Label></asp:TableCell>
                                                <asp:TableCell HorizontalAlign="Center"> <asp:CheckBox runat="server" ID="chkParts" Checked='<%# Eval("RMARQD_ACC") %>' /></asp:TableCell>
                                                <asp:TableCell HorizontalAlign="Center"> <asp:CheckBox runat="server" ID="chhWaive" Checked='<%# Eval("RMARQD_WAIVE") %>' /></asp:TableCell>
                                                <asp:TableCell HorizontalAlign="Center"> <asp:CheckBox runat="server" ID="chkOption" Checked='<%# Eval("RMARQD_OPTION") %>' /></asp:TableCell>

                                                <asp:TableCell> <asp:TextBox ID="txtNewPart" runat="server" Width="100" MaxLength="13" Text='<%# Eval("RMARQD_NPARTNO") %>' class="default"></asp:TextBox><asp:RequiredFieldValidator ID="rfvNewPart" runat="server" ErrorMessage="107_請輸入New Part No" Display="None" TabIndex="0" ValidationGroup="RepairGroup" SetFocusOnError="true"></asp:RequiredFieldValidator><asp:TextBox ID="txtOldPart" runat="server" Width="100" MaxLength="13" Text='<%# Eval("RMARQD_OPARTNO") %>' class="default" Visible="false"></asp:TextBox><asp:TextBox ID="txtOldSerial" runat="server" Width="80px" Text='<%# Eval("RMARQD_OSERIALNO") %>' class="default" Visible="false"></asp:TextBox></asp:TableCell>

                                                <asp:TableCell Visible="false"> <asp:TextBox ID="txtNewSerial" runat="server" Width="80px" Text='<%# Eval("RMARQD_NSERIALNO") %>' class="default"></asp:TextBox></asp:TableCell>

                                                <asp:TableCell> <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Columns="20" Rows="3" Text='<%# Eval("RMARQD_DESC") %>' class="default"></asp:TextBox></asp:TableCell>

                                                <asp:TableCell HorizontalAlign="Center"> <asp:TextBox ID="txtLocation" runat="server" Width="80px" Text='<%# Eval("RMARQD_LOCATION") %>' class="default"></asp:TextBox></asp:TableCell>

                                                <asp:TableCell HorizontalAlign="Center"> <asp:DropDownList ID="cboImproper" runat="server" class="default"><asp:ListItem Value="1" Text="065_Yes"></asp:ListItem><asp:ListItem Value="0" Text="066_No" Selected="True"></asp:ListItem></asp:DropDownList></asp:TableCell>

                                                <asp:TableCell HorizontalAlign="Center"> <asp:DropDownList ID="UI_cboDefective" runat="server" class="default"></asp:DropDownList></asp:TableCell>

                                                <asp:TableCell HorizontalAlign="Center"> <asp:TextBox ID="txtQty" runat="server" Width="20px" Text='<%# Eval("RMARQD_QTY") %>' class="default"></asp:TextBox><asp:TextBox ID="UI_txtMaterialCost" runat="server" Text='<%# Eval("RMARQD_MATERIALCOST") %>' class="default" Style="display: none; width: 1px"></asp:TextBox><asp:TextBox ID="UI_txtRMARQD_PRICE" runat="server" Text='<%# Eval("RMARQD_PRICE") %>' class="default" Style="display: none; width: 1px"></asp:TextBox><asp:RangeValidator ID="rvQty" runat="server" ErrorMessage="109_輸入Qty值型態有誤" Display="None" MinimumValue="0" MaximumValue="10000" Type="Integer" ValidationGroup="RepairGroup"></asp:RangeValidator></asp:TableCell>

                                                <asp:TableCell> <asp:Label ID="txtPrice" runat="server" Text='<%# Eval("RMARQD_PRICE") %>' class="default"></asp:Label></asp:TableCell>

                                                <asp:TableCell> <asp:ImageButton ID="imgDel" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/xx.gif" CommandName="cmdDel" OnClientClick="return FrmDelete()" /></asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </ItemTemplate>
                                </asp:DataList>

                            </td>
                        </tr>

                        <tr>
                            <td colspan="4">

                                <table class="default" bordercolor="#c0c0c0" cellspacing="0" cellpadding="3" width="100%" border="0" id="table8">
                                    <tr>
                                        <td bgcolor="#ffeeb2" align="left" width="30%">
                                            <asp:Label ID="uiLbl_Repair_Manpower" runat="server" Text="086_Man power" class="default"></asp:Label>
                                            <asp:Label ID="uiLbl_Repair_Manpower_Delimited" runat="server" Text="&nbsp;:&nbsp;" class="default"></asp:Label>

                                            <!-- Service Charge 金額 -->
                                            <asp:Label ID="UI_lblServiceCharge" runat="server" Text="0" class="default"></asp:Label>
                                            <asp:TextBox ID="uiTxt_Repair_ManHour" runat="server" Text="0" Width="60px" class="default"></asp:TextBox>

                                            <asp:Label ID="uiLbl_Repair_Hour" runat="server" Text="057_hour" class="default"></asp:Label>
                                            <asp:Label ID="uiLbl_Repair_Hour_Delimited" runat="server" Text="&nbsp;=&nbsp;" class="default"></asp:Label>

                                            <!-- 人工每小時單價, 已用不到了, 預設是 1 -->
                                            <asp:TextBox ID="uiTxt_Repair_LABORPrice" runat="server" class="default"></asp:TextBox>
                                            <!-- 人工維修費用(Service Charge 金額 * 人工每小時單價) -->
                                            <asp:Label ID="uiLbl_Repair_LaborCost" runat="server" Text="0" class="default"></asp:Label>
                                            <!-- Style="display: none; width: 1px"-->
                                        </td>

                                        <td bgcolor="#ffeeb2" align="left" width="30%">
                                            <asp:Label ID="uiTag_Repair_Parts" runat="server" Text="087_Parts" class="default"></asp:Label>
                                            <asp:Label ID="uiTag_Repair_Parts_Delimited" runat="server" Text=" :" class="default"></asp:Label>

                                            <!-- 報價零件加總金額 -->
                                            <asp:Label ID="uiLbl_Repair_PartsTotal" runat="server" Text="0" class="default"></asp:Label>
                                            <asp:TextBox ID="uiTxt_Repair_PartsTotal" runat="server" Text="" class="default"></asp:TextBox>
                                            <!-- Style="display: none; width: 1px"-->
                                        </td>

                                        <td bgcolor="#ffeeb2" align="right">
                                            <asp:Label ID="uiTag_Repair_TotalText" runat="server" Text="088_Total Amount" class="default"></asp:Label>
                                            <asp:Label ID="uiTag_Repair_TotalText_Delimited" runat="server" Text=" :" class="default"></asp:Label>
                                        </td>
                                        <td bgcolor="#ffeeb2" align="left" width="20%">
                                            <!-- 總金額 (Service Charge + 報價零件加總金額) -->
                                            <asp:Label ID="uiLbl_Repair_Total" runat="server" Text="0" Font-Bold="true" class="default"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                        <tr height="40" valign="middle">
                            <td colspan="4" align="center">
                                <asp:Button ID="UI_cmdCancel" runat="server" Text="_Cancel" CssClass="Problem_Edit" />
                                <asp:Button ID="UI_cmdSubmit" runat="server" Text="_Confirm" CssClass="Confirm_l" ValidationGroup="RepairGroup" />
                                <asp:Button ID="UI_cmdSendMail" runat="server" Text="_SendMail" CssClass="Confirm_l" />
                            </td>
                        </tr>
                    </table>

                    <table border="0" width="100%" cellspacing="0" cellpadding="0">
                        <tr height="20px">
                            <td width="24" background="Images/pic_14.gif">&nbsp;</td>
                            <td background="Images/pic_15.gif">
                                <table border="0" width="100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="1%">&nbsp;</td>
                                        <td align="left" class="default">
                                            <asp:Label ID="UI_lblProductTittle" runat="server" Text="010_Product Information"
                                                Font-Bold="true"></asp:Label>
                                        </td>
                                        <td align="right" class="default">
                                            <asp:Label ID="UI_lblQuickSearch" runat="server" Text="069_Quick search"></asp:Label>:
                                            <asp:Label ID="UI_lblQcSn" runat="server" Text="013_Serial Number search"></asp:Label>:
                                            <asp:TextBox ID="UI_txtSN" runat="server" Width="140px" onkeydown="if(event.keyCode==13){SerialSearch();} "></asp:TextBox>
                                            <asp:Button ID="btnQuickSearch" runat="server" Text="_Search" CssClass="Confirm_l" OnClick="btnQuickSearch_Click" />
                                        </td>
                                        <td width="2%">&nbsp;</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>

                    <asp:GridView ID="UI_dvRequest" runat="server" Width="100%" CellPadding="0" CellSpacing="1"
                        border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true"
                        PagerSettings-Mode="Numeric" AllowSorting="true">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label runat="server" ID="UI_lblSeq" Text="Item"></asp:Label>
                                    <asp:CheckBox ID="UI_CheckGroup" runat="server" AutoPostBack="true" OnCheckedChanged="UI_checkGroup_CheckedChanged" Visible="false" />
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <asp:Label ID="UI_Icount" runat="server" Text='<%# me.UI_dvRequest.Rows.Count +1 %>'></asp:Label>
                                    <asp:CheckBox ID="UI_Check" runat="server" AutoPostBack="true" OnCheckedChanged="UI_Check_CheckedChanged" Visible="false" />

                                    <asp:Label ID="UI_SeqID" runat="server"></asp:Label>
                                    <asp:Label ID="UI_RMAID" runat="server" Text='<%# Eval("RMA_ID") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="UI_RMADID" runat="server" Text='<%# Eval("RMAD_ID") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="UI_RMANO" runat="server" Text='<%# Eval("RMAD_RMANO") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="UI_RMADSTATUS" runat="server" Text='<%# Eval("RMAD_STATUS") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="UI_SERIALNO" runat="server" Text='<%# Eval("RMAD_SERIALNO") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="UI_RMAR_COMPNO" runat="server" Text='<%# Eval("RMAR_COMPNO") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="UI_RMADCSTMP" runat="server" Text='<%# Eval("RMAD_CSTMP") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="UI_RMADPARTSN" runat="server" Text='<%# Eval("RMAD_PARTSN") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="013_Serial Numbe">
                                <HeaderStyle Width="14%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton ID="UI_RMAD_SERIALNO" runat="server" Text='<%# Eval("RMAD_SERIALNO") %>'
                                        CommandName="cmdChangeSn" CommandArgument='<%# me.UI_dvRequest.Rows.Count%>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="RMAD_MODELNO" HeaderText="035_Model" HeaderStyle-Width="7%"
                                ItemStyle-HorizontalAlign="Center"></asp:BoundField>

                            <asp:BoundField DataField="Warranty" HeaderText="EW Warranty" HeaderStyle-Width="7%"
                                ItemStyle-HorizontalAlign="Center" HtmlEncode="False" DataFormatString="{0:yyyy/MM/dd}"></asp:BoundField>
                            <asp:BoundField DataField="CWEndWarr" HeaderText="CW Warranty" HeaderStyle-Width="7%"
                                ItemStyle-HorizontalAlign="Center" HtmlEncode="False"></asp:BoundField>
                            <asp:TemplateField HeaderText="SW Warranty">
                                <HeaderStyle Width="8%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton ID="cmdSWDetail" runat="server" Text='<%# Eval("SWEndWarr") %>' CommandName="cmdSWDetail"
                                        CommandArgument='<%# me.UI_dvRequest.Rows.Count%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Detail">
                                <HeaderStyle Width="5%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton ID="UI_cmdDetail" runat="server" ImageUrl="images/icon-detail.gif" CommandName="cmdDetail" Visible="false" CommandArgument='<%#Me.UI_dvRequest.Rows.Count%>' />
                                    <asp:ImageButton ID="UI_cmdDetail_img" runat="server" ImageUrl="images/icon-detail.gif" CommandName="cmdDetail_img" CommandArgument='<%# me.UI_dvRequest.Rows.Count%>' />
                                </ItemTemplate>
                            </asp:TemplateField>



                            <%-- <asp:TemplateField HeaderText="SDC">
                                                         <HeaderStyle Width="5%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                         <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                         <ItemTemplate>
                                                                <asp:ImageButton ID="UI_cmdSDC" runat="server" ImageUrl="images/icon-detail.gif" CommandName="cmdSDC" CommandArgument='<%# me.UI_dvRequest.Rows.Count%>'/>
                                                         </ItemTemplate>
                                                      </asp:TemplateField>--%>
                            <asp:BoundField DataField="Quoted" HeaderText="070_Quote" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>

                            <asp:BoundField DataField="Status" HeaderText="032_Status" HeaderStyle-Width="10%"
                                ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                            <asp:BoundField DataField="Assign" HeaderText="071_Re-Assign" HeaderStyle-Width="10%"
                                ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                        </Columns>
                        <EmptyDataTemplate>
                            <asp:Label runat="server" ID="UI_NotFound" Text='<%# Session("_isShowNotFound").tostring()%>'></asp:Label>
                        </EmptyDataTemplate>
                        <EmptyDataRowStyle HorizontalAlign="Center" />
                        <HeaderStyle CssClass="Text_Head" />
                        <RowStyle CssClass="TR_1" />
                        <AlternatingRowStyle CssClass="ListRowEven" />
                        <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Center" />
                    </asp:GridView>
                    <asp:Button ID="UI_cmdApply" runat="server" Style="display: none1" Text="_Apply" CssClass="Confirm" ValidationGroup="RepairGroup" OnClientClick="onProgress('Save')" /><!-- UI_cmdApply_Click-->                    
                </fieldset>
                 <%-- 需求新增:BI保固 By buck Add 20250902 begin --%>
                 <fieldset id="fdt_Warranty" runat ="server" class="form_div" valign="top" style="width: 100%">
                    <table border="0" width="100%" cellspacing="0" cellpadding="0">
                        <tr height="20px">
                            <td width="24" background="Images/pic_14.gif">&nbsp;</td>
                            <td background="Images/pic_15.gif">
                                <table border="0" width="100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="1%">&nbsp;</td>
                                        <td align="left" class="default">
                                            <asp:Label ID="UI_lblWarrantyBI" runat="server" Text="047_Warranty BI"
                                                Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                     
                    <asp:GridView ID="UI_dvBATRECORD" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="false" AllowPaging="true" PagerSettings-Mode="Numeric" AllowSorting="true">
                    <%--<asp:GridView ID="UI_dvBATRECORD" runat="server" AutoGenerateColumns="true" BorderStyle="Solid" BorderWidth="1px" Width="100%" CellPadding="6" CellSpacing="0">--%>
                        <Columns>
                            <asp:BoundField DataField="BE_ORDERNO" HeaderText="訂單號碼"  HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="BE_TYPE" HeaderText="耗用類型"  HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="BE_REFNO" HeaderText="RMA單號"  HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="BE_PRODSERIAL" HeaderText="主機序號"  HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="BE_BATSERIAL_OLD" HeaderText="舊電池序號"  HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="BE_BATSERIAL_NEW" HeaderText="新電池序號"  HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="BE_USEQTY" HeaderText="耗用數"  HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center" />
                        </Columns>
                        <EmptyDataRowStyle HorizontalAlign="Center" />
                        <HeaderStyle CssClass="Text_Head" />
                        <RowStyle CssClass="TR_1" />
                        <AlternatingRowStyle CssClass="ListRowEven" />
                        <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Center" />
                    </asp:GridView>                      
                </fieldset>
                <%-- 需求新增:BI保固 By buck Add 20250902 end --%>
                <uc11:UcWarrantyView ID="UcWarrantyView" runat="server" />

                <asp:Label ID="UI_RMANo" runat="server" Visible="false"></asp:Label>
                <asp:Label ID="UI_RMA_APPLICANT" runat="server" Visible="false"></asp:Label>
                <asp:Label ID="UI_RMA_MAIL" runat="server" Visible="false"></asp:Label>
                <asp:TextBox ID="UI_CUNO" runat="server" Style="display: none; width: 1px"></asp:TextBox>
                <asp:Label ID="UI_ACCOUNTID" runat="server" Visible="false"></asp:Label>
                <asp:Label ID="UI_CUName" runat="server" Visible="false"></asp:Label>

                <%--                <asp:TextBox ID="UI_txtLABORPRICE" Style="display: none; width: 1px" runat="server" ></asp:TextBox>--%>
                <%--                <asp:TextBox ID="UI_txtTotalManAmt" runat="server" Style="display: none; width: 1px"></asp:TextBox>--%>
                <%--                <asp:TextBox ID="UI_txtTotalQuote" runat="server" Style="display: none; width: 1px"></asp:TextBox>--%>

                <asp:TextBox ID="UI_txtAssigeCurrencyRate" runat="server" Visible="false"></asp:TextBox>
                <asp:TextBox ID="UI_txtAssigeCurrencyCode" runat="server" Visible="false"></asp:TextBox>
                <asp:TextBox ID="UI_txtCurrencyRate" runat="server" Visible="false"></asp:TextBox>
                <asp:TextBox ID="UI_txtCurrencyCode" runat="server" Visible="false"></asp:TextBox>

                <asp:TextBox ID="UI_CU_DISCOUNT_OFF" runat="server" Style="display: none; width: 1px"></asp:TextBox>
                <asp:TextBox ID="UI_CU_SERVICE_CHG" runat="server" Style="display: none; width: 1px"></asp:TextBox>
                <asp:TextBox ID="UI_isRepairQuoted" runat="server" Text="0" Visible="false"></asp:TextBox>
                <asp:TextBox ID="UI_Customer_ExceptionCharge" runat="server" Style="display: none; width: 1px"></asp:TextBox>
                <asp:TextBox ID="UI_flowCase" runat="server" Style="display: none; width: 1px"></asp:TextBox>

                <asp:RequiredFieldValidator ID="rfvSerialNo" runat="server" ErrorMessage="108_請輸入Serial No"
                    ControlToValidate="UI_lblSerialText" Display="None" TabIndex="0" ValidationGroup="RepairGroup"
                    SetFocusOnError="true"></asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="rfvProblemDesc" runat="server" ErrorMessage="072_請輸入問題描述"
                    ControlToValidate="UI_txtProblemDesc" Display="None" TabIndex="0" ValidationGroup="RepairGroup"
                    SetFocusOnError="true"></asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="rfvRepairDesc" runat="server" ErrorMessage="073_請輸入維修描述"
                    ControlToValidate="UI_txtRepairDesc" Display="None" TabIndex="0" ValidationGroup="RepairGroup"
                    SetFocusOnError="true"></asp:RequiredFieldValidator>

                <asp:CustomValidator ID="cvFailureClass" runat="server" ClientValidationFunction="Validate_FailureClass"
                    ErrorMessage="078_請選取不良原因類別名稱" Display="None" Operator="DataTypeCheck" ValidationGroup="RepairGroup"></asp:CustomValidator>
                <asp:CustomValidator ID="cvFailure" runat="server" ClientValidationFunction="Validate_Failure"
                    ErrorMessage="079_請選取不良原因名稱" Display="None" Operator="DataTypeCheck" ValidationGroup="RepairGroup"></asp:CustomValidator>
                <asp:CustomValidator ID="cvRepair" runat="server" ClientValidationFunction="Validate_Repair"
                    ErrorMessage="050_請選取公司名稱" Display="None" Operator="DataTypeCheck" ValidationGroup="RepairGroup"></asp:CustomValidator>

                <asp:ValidationSummary ID="vsRepairr" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="RepairGroup" />
                <asp:ValidationSummary ID="vsAddPart" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="Repair_AddPart" />

                <asp:Button ID="UI_butTarget" Width="0px" Height="0px" runat="server" Style="display: none"></asp:Button>
                <asp:Button ID="UI_butOK" Width="0px" Height="0px" runat="server" Style="display: none"></asp:Button>
            </asp:Panel>
            <uc2:ucMessage ID="ucMessage" runat="server" />

            <uc4:ucClientDetailPur ID="ucClientDetailPur" runat="server" />
            <uc5:ucRepairRarts ID="ucRepairRarts" runat="server" />
            <uc8:ucSpecialSetting ID="ucSpecialSetting" runat="server" />
            <%--<uc9:UcSDCView ID="UcSDCView" runat="server" /> --%>

            <script type="text/javascript">
                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(cal_subTotalAMT);
            </script>

            <uc1:ucProgressStatus ID="ucProgressStatus" runat="server" />

            <uc1:ucClientDetail ID="ucClientDetail" runat="server" />

            <div align="center">
                <asp:Panel ID="total_loss_Panel_Out" runat="server" Visible="false">
                    <asp:Panel ID="total_loss_Panel" runat="server">
                        <table width="95%" border="1" cellspacing="0" cellpadding="0" class=" box_table">
                            <tr>
                                <td>
                                    <asp:Label ID="Shipped_Quantity_Label" runat="server" Text="Shipped Quantity"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Accumulated_Repair_Quantity_Label" runat="server" Text="Accumulated Repair Quantity"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Repair_Rate_Label" runat="server" Text="Repair Rate"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Shipping_Lab" runat="server" Text=""></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Repair_Lab" runat="server" Text=""></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Exceed_Lab" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />

</asp:Content>
