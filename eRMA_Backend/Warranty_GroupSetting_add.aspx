<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Warranty_GroupSetting_add.aspx.vb" Inherits="Warranty_GroupSetting_add" %>

<%@ Register Src="ascx/ucProgressStatus.ascx" TagName="ucProgressStatus" TagPrefix="uc1" %>
<%@ Register Src="ascx/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <script type="text/javascript" language="javascript">
        function FrmDelete() {
            if (confirm(delMsg)) {
                onProgress('Delete')
                return true;
            }
            return false;
        }
    </script>

    <uc1:ucProgressStatus ID="ucProgressStatus" runat="server" />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>

            <table border="0" width="100%" id="table4" cellspacing="0" cellpadding="0" height="100%">
                <tr>
                    <td width="24" background="Images/pic_12.gif">&nbsp;</td>
                    <td valign="top" align="left">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0" height="100%">
                            <!--[Begin]Tittle-->
                            <tr>
                                <td width="3%">&nbsp;</td>
                                <td width="94%" align="left">
                                    <asp:Label ID="UI_lblTittle" runat="server" Text="	Setting - Warranty Group" CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
                                </td>
                                <td width="3%">&nbsp;</td>
                            </tr>
                            <!--[End]Tittle-->
                            <!--[Begin]新增資料-->
                            <tr>
                                <td>&nbsp;</td>
                                <td align="left">
                                    <table width="100%" align="center" border="0" cellspacing="1" class="default">
                                        <tr>
                                            <td width="15%">&nbsp;
				                        <asp:Label ID="UI_lblGroupNo" runat="server" Text="Group No"></asp:Label>
                                                <td width="75%">:
				                        <asp:TextBox ID="UI_txtGroupNo" runat="server" Width="200"></asp:TextBox>
                                                </td>
                                                <td width="10%" align="left">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td width="15%">&nbsp;
				                        <asp:Label ID="UI_lblGroupName" runat="server" Text="Group Name"></asp:Label>
                                                <td width="75%">:
				                        <asp:TextBox ID="UI_txtGroupName" runat="server" Width="200"></asp:TextBox>
                                                </td>
                                                <td width="10%" align="left">&nbsp;</td>
                                        </tr>
                                    </table>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <!--[End]新增資料-->
                        </table>
                    </td>
                </tr>
                <tr id="UI_trPart" runat="server">
                    <td background="Images/pic_20.gif">&nbsp;</td>
                    <td valign="top" bgcolor="#E3D8BE" align="center">
                        <table border="0" cellspacing="0" cellpadding="0" height="100%" width="100%">
                            <tr>
                                <td>&nbsp;</td>
                                <td width="100%" align="left" class="default">
                                    <!--[Begin]新增資料列表-->
                                    <div align="center">
                                        <asp:GridView ID="UI_GroupParts" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="false">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="UI_SeqID" runat="server" Text='<%# Eval("SeqID") %>'></asp:Label>
                                                        <asp:Label ID="UI_TypeNo" runat="server" Text='<%# Eval("TYPE_NO") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_Check" runat="server" Text='<%# Eval("Checked") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Type No.">
                                                    <HeaderStyle Width="25%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="UI_lblTypeNo" Text='<%# Eval("TYPE_NO")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Type Name.">
                                                    <HeaderStyle Width="25%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="UI_lblTypeName" Text='<%# Eval("TYPE_NAME")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Check">
                                                    <HeaderStyle Width="10%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkType" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="Text_Head" />
                                            <RowStyle CssClass="TR_1" />
                                            <AlternatingRowStyle CssClass="ListRowEven" />
                                        </asp:GridView>
                                    </div>
                                    <!--[End]新增資料列表-->
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td background="Images/pic_20.gif" height="130">&nbsp;</td>
                    <td valign="top" bgcolor="#E3D8BE" align="center">
                        <!--[Begin]Submit-->
                        <table border="0" cellspacing="0" cellpadding="0" width="100%" height="50px">
                            <tr>
                                <td width="3%">&nbsp;</td>
                                <td width="94%" align="center" class="default">
                                    <asp:Button ID="UI_cmdSubmit" runat="server" Text="Save" CssClass="Confirm_l" OnClientClick="onProgress('Save')" />
                                </td>
                                <td width="3%">&nbsp;</td>
                            </tr>
                        </table>
                        <!--[End]Submit-->
                    </td>
                </tr>
            </table>

            <uc2:ucMessage ID="ucMessage" runat="server" />

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="UI_cmdSubmit" EventName="Click" />
            <asp:PostBackTrigger ControlID="ucMessage" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:Label runat="server" ID="UI_lblPreviousPage_GroupNo" Visible="false"></asp:Label>

</asp:Content>
