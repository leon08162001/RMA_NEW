<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Shipping_Print.aspx.vb" Inherits="Shipping_Print" Title="Untitled Page" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <table width="100%" id="table2" cellspacing="0" cellpadding="15" class="default" bordercolor="#808080">
        <tr>
            <td width="100%" align="center" height="100%">
                <p>
                    <asp:Label ID="UI_Tittle" runat="server" Text="170_After save this form , Are you going to print out this form?." Font-Bold="true"></asp:Label>
                </p>
                <asp:Button ID="UI_cmdClose" runat="server" Text="010_Cancel" CssClass="Problem_Edit" />&nbsp;
            <asp:Button ID="UI_cmdPrint" runat="server" Text="045_Print Now" CssClass="Confirm" />
                <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
            </td>
        </tr>
    </table>
    <asp:Label ID="UI_lblPreviousPage_RMASHID" runat="server" Visible="false"></asp:Label>
</asp:Content>

