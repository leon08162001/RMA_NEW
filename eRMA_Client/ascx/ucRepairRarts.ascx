<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ucRepairRarts.ascx.vb" Inherits="ascx_ucRepairRarts" %>


<asp:Panel ID="UI_panel" runat="server" style="display:none;position:absolute;width:70%;border:3px solid #808080;">

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
                if(objRadio!=nowRadio && objRadio.name.indexOf("UI_dvRepairRart")>-1 && objRadio.name.indexOf("UI_Radio")>-1)
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
                <asp:Label ID="UI_lblTitle" runat="server" Text ="221_Please click radio box for Part's selection and get window closed from below lists."></asp:Label>
            </td>
        </tr>
        
        
        <tr class="default">
            <td align="left">&nbsp;&nbsp;
                <asp:Label ID="UI_lblKeyword" runat="server" Text="174_Keyword Search" Font-Bold="true"></asp:Label>.:
                <asp:TextBox ID = "UI_txtKeyword" runat ="server" Width="150px"></asp:TextBox>
                <asp:Button ID="UI_cmdSearch" runat ="server" Text = "016_Search" CssClass ="Search" />
            </td>
        </tr>

        
        
        <tr class="default" valign="top">
            <td align="center">
                <asp:GridView ID="UI_dvRepairRart" runat ="server" Width = "100%" CellPadding ="0" CellSpacing ="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric">
                    <Columns>
                      <asp:TemplateField>
                         <HeaderStyle Width="10%" Height ="20px" HorizontalAlign="Center"></HeaderStyle>
                         <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                         <ItemTemplate>
                                <asp:RadioButton ID="UI_Radio" runat="server" />
                                <asp:Label ID ="UI_PartNo" runat ="server" Text='<%# Eval("rpbom_partno") %>' Visible="false"></asp:Label> 
                                <asp:Label ID ="UI_Desc" runat ="server" Text='<%# Eval("rpbom_desc") %>' Visible="false"></asp:Label> 
                         </ItemTemplate>
                      </asp:TemplateField>

                      <asp:BoundField DataField="rpbom_partno" HeaderText="152_Part’s No" HeaderStyle-Width="40%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                      <asp:BoundField DataField="rpbom_desc" HeaderText="161_Description" HeaderStyle-Width="50%" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                     
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
                <asp:Button ID="UI_cmdSubmit" runat ="server" Text = "001_Submit" CssClass ="Confirm_l" />
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


