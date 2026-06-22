<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Customer.aspx.vb" Inherits="Customer" Title="Untitled Page" %>

<%@ Register Src="ascx/ucCustomerAdmin.ascx" TagName="ucCustomerAdmin" TagPrefix="uc1" %>
<%@ Register Src="ascx/ucCustomerUser.ascx" TagName="ucCustomerUser" TagPrefix="uc2" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">

    <uc1:ucCustomerAdmin ID="ucCustomerAdmin" runat="server" Visible="false" />
    <uc2:ucCustomerUser ID="ucCustomerUser" runat="server" Visible="false" />

    <asp:Label ID="UI_lblPreviousPage_CuNo" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="UI_lblPreviousPage_CuusID" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="UI_lblPreviousPage_Role" runat="server" Visible="false"></asp:Label>

</asp:Content>


