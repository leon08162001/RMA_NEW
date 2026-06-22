<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ucCustomer_pick.ascx.vb" Inherits="ascx_ucCustomer_pick" %>


<asp:Panel ID="UI_panel" runat="server" Width="500px" style="display:none;position:absolute">

<script type="text/javascript">
    function setRadio(nowRadio)
    {
        var myForm,objRadio;
        myForm=document.forms[0];
        for(var i=0;i<myForm.length;i++)
        {
            if(myForm.elements[i].type=="radio")
            {
                objRadio=myForm.elements[i];
                if(objRadio!=nowRadio && objRadio.name.indexOf("UI_dvCustomer")>-1 && objRadio.name.indexOf("UI_Radio")>-1)
                {
                    if(objRadio.checked)
                    {
                        objRadio.checked=false;
                    }
                }
            }
        }
    }
</script>

    <div align="center">
    <fieldset class = "form_div" align="top" style="width: 100%">
    <table id="table2" width="100%" align="center" border="0" cellspacing="1" >
        <tr class="default">
            <td align="left" colspan="4">
                <asp:Label ID="UI_lblShippingCustomer" runat="server" Text ="173_Please click radio box for customer selection and get window closed from below lists."></asp:Label>
            </td>
        </tr>
        <tr class="default">
            <td align="left">&nbsp;&nbsp;
                <asp:Label ID="UI_lblKeyword" runat="server" Text="174_Keyword Search" Font-Bold="true"></asp:Label>.:
                <asp:TextBox ID = "UI_txtKeyword" runat ="server" Width="150px"></asp:TextBox>
                <asp:Button ID="UI_cmdSearch" runat ="server" Text = "004_Search" CssClass ="Search" />
            </td>
        </tr>
        <tr class="default" valign="top">
            <td align="center">
                <asp:GridView ID="UI_dvCustomer" runat ="server" Width = "100%" CellPadding ="0" CellSpacing ="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric">
                    <Columns>
                      <asp:TemplateField>
                         <HeaderStyle Width="10%" Height ="20px" HorizontalAlign="Center"></HeaderStyle>
                         <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                         <ItemTemplate>
                                <asp:RadioButton ID="UI_Radio" runat="server" />
                                <asp:Label ID ="UI_CustomerID" runat ="server" Text='<%# Eval("CU_NO") %>' Visible="false"></asp:Label> 
                                <asp:Label ID ="UI_CustomerName" runat ="server" Text='<%# Eval("CU_NAME") %>' Visible="false"></asp:Label> 
                                <asp:Label ID ="UI_CurrencyCode" runat ="server" Text='<%# Eval("CURRENCY_CODE") %>' Visible="false"></asp:Label> 
                                <asp:Label ID ="UI_CurrencyRate" runat ="server" Text='<%# Eval("CURRENCY_RATE") %>' Visible="false"></asp:Label> 
                                <asp:Label ID ="UI_CuAddress1" runat ="server" Text='<%# Eval("CU_ADDRESS1") %>' Visible="false"></asp:Label> 
                                <asp:Label ID ="UI_Visible" runat ="server" Text='<%# Eval("Visible") %>' Visible="false"></asp:Label> 
                         </ItemTemplate>
                      </asp:TemplateField>

                      <asp:BoundField DataField="CU_NO" HeaderText="175_Customer ID" HeaderStyle-Width="40%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                      <asp:BoundField DataField="CU_NAME" HeaderText="176_Name" HeaderStyle-Width="50%" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                     
                    </Columns> 
                    <HeaderStyle CssClass="Text_Head"/>
                    <RowStyle CssClass="TR_1" />
                    <AlternatingRowStyle CssClass="ListRowEven" />
                    <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Center" />
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td align="center">&nbsp;&nbsp;
                <asp:Button ID="UI_cmdCancel" runat ="server" Text = "010_Cancel" CssClass ="Problem_Edit" />
                <asp:Button ID="UI_cmdSubmitShipment" runat ="server" Text = "001_Submit" CssClass ="Confirm_l" />
                <asp:Button ID="UI_cmdSubmitShipping" runat ="server" Text = "001_Submit" CssClass ="Confirm_l" />
                <asp:Button ID="UI_cmdNewRequest" runat ="server" Text = "001_Submit" CssClass ="Confirm_l" />
            </td>
        </tr>
    </table>
    </fieldset>	
    </div>

    <asp:Label ID ="UI_lblCompNo" runat ="server" Visible="false"></asp:Label> 
    <asp:button id="UI_butTarget" Width="0px" Height="0px" runat="server" style="display:none"></asp:button>
    <asp:button id="UI_butOK" Width="0px" Height="0px" runat="server" style="display:none"></asp:button></asp:Panel>


<ajaxToolkit:ModalPopupExtender ID="ajModalProgress" TargetControlID="UI_butTarget"  
PopupControlID="UI_panel"  
OkControlID="UI_butOK" 
CancelControlID="UI_cmdCancel"
BackgroundCssClass = "modalBackground"
runat="server">
</ajaxToolkit:ModalPopupExtender>


