<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ucSubstitute.ascx.vb" Inherits="ascx_ucSubstitute" %>

    <style>

        .table-Sub {
        min-width:660px;
        min-height:180px;
        overflow: auto;
        background: #fff;
        box-sizing: border-box;
        display: inline-block;
        overflow: auto;
        padding:15px;
        }

        .table-Sub	th 
		{
			border-bottom:1px solid #000000;
			color:#000000;
		}

        .table-Sub	td 
        {
        padding-left:10px;
        padding-top:10px;
        min-width:300px;
        }

        .table-Sub td span {
        color: var(--black, #000);
        font-family: Source Sans Pro;
        font-size: 16px;
        font-style: normal;
        line-height: normal;
        }

        .ucSubstitutebtn {
            display: flex;
            width:86px;
            height:36px;
            align-items: flex-start;
            border-radius: 5px;
            border: 1px solid #1C218C;
            background: #1C218C;
            color:#ffffff;
            text-align:center;
            padding-left:25px;
            padding-right:25px;
        }
    </style>

<asp:Panel ID="UI_panel" runat="server"  CssClass="ucSubstitute_div" style="display:none;width:700px;height:316px;background:#ffffff;border:1px solid #000000;border-radius: 5px;">

<fieldset class="form_div" valign="top" style="border:none;">

        <table   style="margin:25px 20px 25px 20px;">
        <tr>
        <td>
            <center>
            <asp:Label ID="UI_lblReportTittle" runat="server" Text="030_Report Information" Font-Bold ="true" Font-Size="Larger"></asp:Label>
                </center>
        </td>
        </tr>

        <tr>
        <td>
            <center>       
            <asp:GridView ID="UI_dvSubstitute" runat ="server"  CssClass="table-Sub" CellPadding ="0" CellSpacing ="0" border="0" GridLines="None"  PagerSettings-Visible="false"  Width="650" AutoGenerateColumns="False"  AllowPaging="true" >
            <Columns>
            <asp:TemplateField Visible="false">
            <HeaderStyle  HorizontalAlign="Center"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center" ></ItemStyle>
            <ItemTemplate>
            <asp:Label ID ="UI_SeqID" runat = "server" ></asp:Label>
            </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="bmd04"  HeaderText="165_166_取替代料件"  ItemStyle-HorizontalAlign="Left"></asp:BoundField>
            <asp:BoundField DataField="RPBOM_DESC" HeaderText="161_Description" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
            </Columns> 
            <HeaderStyle />
            <RowStyle  />
            <AlternatingRowStyle  />
            <PagerStyle  />
            </asp:GridView>	    
            <asp:Button ID="UI_cmdCancel" runat ="server" Text = "008_Cancel" CssClass ="ucSubstitutebtn" /> 
            </center>
 
        </td>
        </tr>
        </table>
</fieldset>

<asp:button id="UI_butTarget" Width="0px" Height="0px" runat="server" style="display:none"></asp:button>
</asp:Panel>




<ajaxToolkit:ModalPopupExtender ID="ajModalProgress" TargetControlID="UI_butTarget"  
PopupControlID="UI_panel"  
CancelControlID="UI_cmdCancel"
BackgroundCssClass = "Modal"
runat="server">
</ajaxToolkit:ModalPopupExtender>

