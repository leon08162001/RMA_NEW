<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeFile="MantainMail.aspx.vb" Inherits="MantainMail" %>

<%@ Register Src="~/ascx/ucProgressStatus.ascx" TagPrefix="uc1" TagName="ucProgressStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <uc1:ucProgressStatus runat="server" ID="ucProgressStatus" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table border="0" width="100%" id="table1" cellspacing="0" cellpadding="0" height="100%">
                <tr>
                    <td background="Images/pic_20.gif">&nbsp;</td>
                    <td valign="top" bgcolor="#E3D8BE" align="center">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="1%">&nbsp;</td>
                                <td width="99%" align="left" class="default">
                                    <div align="center">
                                        <asp:Label ID="UI_lblPartMail" runat="server" Text="for Parts Request Mail"></asp:Label>
                                        <asp:TextBox ID="UI_tbPartMail" runat="server" Width="500"></asp:TextBox>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div align="center">
                                        <asp:Button ID="UI_btnSave" Text="_Temporary Save" runat="server" CssClass="Confirm" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
