<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Request_Policy.aspx.vb" Inherits="Request_Policy" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">

    <head>
        <meta charset="utf-8">
        <meta name="viewport" content="width=device-width, initial-scale=1">
        <title>jQuery UI Dialog - Default functionality</title>
        <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
        <link rel="stylesheet" href="/resources/demos/style.css">
        <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
        <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
        <script>
            /*出現視窗後 底下的Button可以動*/
            //$( function() {
            //    /*$("#dialog").dialog();*/
            //    $("#dialog").dialog({
            //        width: 500,
            //        //width: 350,
            //    });
            // } );

            $(function () {
                var sRepair = '<%=Session("_RepairID")%>';
                if (sRepair.indexOf('CEAT') > -1) {
                    $("#dialog").dialog({
                        modal: true,
                        width: 800,
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                            }
                        }
                    });
                }
                else {
                    $("#dialog").hide();
                }
            });
        </script>
    </head>

    <div id="dialog" title="Announcement Dialog">
        <p>
            Notice：
            <p style="color: #ff0000; font-size: 15px;">2023. Sep 7th update</p>
            <br />
            We are pleased to announce an enhancement to our maintenance services.<br />
            Starting September 11, 2023, all repair-related operations will be transferred to our newly established European Service Center located in France.<br />

            Location:<p style="color: #ff0000; font-size: 15px;">4 Rue de Cadorago, 21310 Belleneuve, France.</p>
            <br />
            Contact email：<p style="color: #ff0000; font-size: 15px;">cipherlabrma@adac-electronique.fr</p>
            <br />
            After submitting your new request, please send your product to our new repair center.
            <br />
        </p>
    </div>

    <table border="0" width="100%" id="table4" cellspacing="0" cellpadding="0" height="100%">
        <tr>
            <td width="24" background="Images/pic_12.gif">&nbsp;</td>
            <td valign="top" align="left">
                <!--Content of Tittle-->
                <table border="0" width="95%" id="table6" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="text_tittle">
                            <asp:Label ID="UI_lblTittle" runat="server" Text="001_Policy Agreement" CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <iframe name="I1" src="pop_Policy.aspx" class="menuframe" frameborder="0" marginwidth="1" marginheight="1" target="_parent" scrolling="Yes" width="100%" height="300px"></iframe>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>

        <tr>
            <td width="24" background="Images/pic_12.gif">&nbsp;</td>
            <td valign="top" align="left">
                <table border="0" width="100%" id="table7" cellspacing="0" cellpadding="0" height="60">
                    <tr>
                        <td align="center">
                            <asp:Button ID="UI_cmdDisagree" runat="server" Text="_Disagree" CssClass="Confirm" PostBackUrl="Client_Worklist.aspx" />
                            <asp:Button ID="UI_cmdAgree" runat="server" Text="_Agree" CssClass="Confirm" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

</asp:Content>

