<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ucCustomerUser.ascx.vb" Inherits="ascx_ucCustomerUser" %>
<%@ Register Src="ucProgressStatus.ascx" TagName="ucProgressStatus" TagPrefix="uc1" %>
<%@ Register Src="ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>

<style>

   .ucCustomerUser_Btn {
        background: none;
        padding: 9px 19px;
        border-radius: 5px;
        width: 70px;
        margin: 0 10px;
        border: 1px solid #000;
        cursor: pointer;
    }


</style>

<uc1:ucProgressStatus ID="ucProgressStatus" runat="server" />

<link href="../css/personalInformation.css" rel="stylesheet" />

<div class="erma-window-background erma-window-image-background erma-window-changePassword-background"   id="ucCustomerUser_001"  style="display:none;">
    <div class="erma-window-box"   style="width:350PX;">
        <div class="erma-password-title">
            <h5>
                     <asp:Label ID="change_Password_001_Lab" runat="server" Text=""></asp:Label>
            </h5>
        </div>
        <div class="erma-password-input">
		    <asp:Label ID="ucCustomerUser_Password_Lab" runat="server" Text=""></asp:Label>:
            <asp:TextBox ID="ucCustomerUser_Txt" CssClass="erma-input-input" TextMode="Password" runat="server"></asp:TextBox>
        </div>
        <div class="erma-buttons">         
             <asp:Button ID="CancelBtn" runat="server" Text="Cancel" CssClass="ucCustomerUser_Btn" OnClientClick="displayFun('.erma-window-changePassword-background')" Width="150"  />
            <asp:Button ID="ucCustomerUser_Btn" runat="server" Text="Save" CssClass="ucCustomerUser_Btn" OnClientClick="displayFun('.erma-window-changePassword-background')"  Width="150" />
			
        </div>
    </div>
</div>

<asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
    <ContentTemplate> 

      <div class="erma-personalInformation-block" onclick="()">
    <div class="erma-top-bar">
       
        
        <a href="Client_FlowCase01_Worklist.aspx" style="display:inline-block;">
            <div class="erma-button-back" >
                <img src="../images_new/previous.svg" />
                <span>Back</span>
            </div>
        </a>

    
        <div  style="display:inline-block;float:right;">
     
        </div>

        <div class="erma-personalInformation-title" style="text-align:center;">
            <asp:Label ID="personal_information_Lab" runat="server" Text=""  Font-Size="Larger" ></asp:Label>
        </div>

    </div>
    <div class="erma-personalInformation-content">
        <div class="erma-user-icon">
            <img src="../images_new/big-user.svg" />
               <asp:Button ID="AddressPanelBtnShow" runat="server" Text="Address"  CssClass="erma-input-input"  OnClick="AddressPanelBtnShow_Click" ValidationGroup="PanelShow" Visible="false" />
        </div>

        <asp:Panel ID="DetailsPanel" runat="server">
        <table>
            <tr>
                <td><asp:Label ID="UI_lblCustomerID" runat="server" Text="003_Customer ID"></asp:Label> :</td>
                <td><asp:Label ID="UI_lblCustomerIDText" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td> <asp:Label ID="UI_lblContactPerson" runat="server" Text="016_Contact Person"></asp:Label>:</td>
                <td><asp:Label ID="UI_lblContactPersonText" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td> <asp:Label ID="UI_lblTEL" runat="server" Text="011_TEL"></asp:Label>:</td>
                <td> <asp:Label ID="UI_lblTELText" runat="server"></asp:Label></td>    
            </tr>
            <tr>
                <td><asp:Label ID="UI_lblEMail" runat="server" Text="021_eMail"></asp:Label> :</td>
                <td><asp:Label ID="UI_lblEMailText" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td><asp:Label ID="UI_lblCountry" runat="server" Text="013_Country"></asp:Label>:</td>
                <td><asp:Label ID="UI_lblCountryText" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td><asp:Label ID="UI_lblCustomerName" runat="server" Text="004_Customer Name."></asp:Label> :</td>
                <td><asp:Label ID="UI_lblCustomerNameText" runat="server"></asp:Label></td>
            </tr>
            <tr>  
                <td><asp:Label ID="UI_lblRepairCenter" runat="server" Text="009_Repair Center"></asp:Label>:</td>
                <td><asp:Label ID="UI_lblRepairCenterText" runat="server"></asp:Label></td>
            </tr>

            <tr>
            <td>
           <asp:Label ID="AddressLabel" runat="server" Text=""></asp:Label>:
            </td> 
            <td>      
                            <asp:Label ID="UI_lblAddress1Text" runat="server"></asp:Label>
            <asp:DropDownList ID="Drop_txtAddress" runat="server"  CssClass="erma-combobox-choose" Visible="false"></asp:DropDownList>
            </td>
            </tr>

            <tr style="display:none;" ><td>
            <asp:Label ID="UI_lblAddress1" runat="server" Text="017_Address (1)"></asp:Label> :</td> <td>        

            </td></tr>
            <tr style="display:none;"><td>
            <asp:Label ID="UI_lblAddress2" runat="server" Text="018_Address (2)"></asp:Label> :</td> <td>      
            <asp:Label ID="UI_lblAddress2Text" runat="server"></asp:Label>
            </td></tr>
            <tr style="display:none;"><td>
            <asp:Label ID="UI_lblAddress3" runat="server" Text="019_Address (3)"></asp:Label>   :</td> <td>       
            <asp:Label ID="UI_lblAddress3Text" runat="server"></asp:Label>
            </td></tr>
            <tr style="display:none;"><td>
            <asp:Label ID="UI_lblAddress4" runat="server" Text="020_Address (4)"></asp:Label>    :</td> <td> 
            <asp:Label ID="UI_lblAddress4Text" runat="server"></asp:Label>
            </td></tr>
 
            <tr>
            <td> 
            

            </td>
            </tr>

            <tr>
                <td>
                    <asp:Label ID="PasswordLab" runat="server" Text=""></asp:Label>:</td>
                <td>**** &nbsp;&nbsp; <span  class="erma-infomation-addButton" style="text-align:center;display:inline;background:#ffffff;border-bottom:1px solid #1C218C;color:#1C218C;font-size:15px;" onclick=" document.getElementById('ucCustomerUser_001').style.display = 'block'; "> <asp:Label ID="change_Password_002_Lab" runat="server" Text=""></asp:Label></span></td>
            </tr>

        </table>
        </asp:Panel>

        <asp:Panel ID="AddressPanel" runat="server">
        <table>
                        <tr>
                        <td>Address</td>
                        <td>
                        <asp:TextBox ID="ADDRESS_Txt" runat="server" CssClass="erma-input-input" Width="250" ></asp:TextBox>
                        </td>
                        <td>
                        <asp:Button ID="ADDRESS_Btn" runat="server" Text="新增"   CssClass="erma-input-input"  />
                        </td>
                        <td>
                        <asp:Button ID="ADDRESS_Save_Btn" runat="server" Text="儲存"    CssClass="erma-input-input"   />
                        </td>
                        </tr>
                       
                        <tr>
                        <td  colspan="4">
                        <asp:GridView ID="ADDRESS_GridView" runat="server" Width="450" Cssclass="table"  AutoGenerateColumns="False" DataKeyNames="編號" >
                        <Columns>
                        <asp:BoundField DataField="編號" HeaderText="編號" SortExpression="編號"   >
                        <ItemStyle Width="60px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="地址" HeaderText="地址" SortExpression="地址" />
                        </Columns>
                        </asp:GridView>
                        </td>   
                        </tr>
                </table>
        </asp:Panel>

    </div>
    </div>

                    <div  style="display:none;" >
             
                    <asp:Label ID="UI_lblTittle" runat="server" Text="001_Setting - Customer" CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
                    <asp:Label ID="UI_lblStatus" runat="server" Text="022_Account status"></asp:Label>
                    <asp:Label ID="UI_lblStatusText" runat="server"></asp:Label>
			         
	     
	                 <!--[End]新增資料-->
	    
	                 <asp:Label ID="UI_lblInformationTittle" runat="server" Text="023_User Information" Font-Bold ="true"></asp:Label>
	                           
		             <!--[Begin]新增資料列表-->
       
                         <asp:GridView ID="UI_CustomerUser" runat ="server"  CellPadding ="0" CellSpacing ="1" border="0" Cssclass="table" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric"  >
                <Columns>
                  <asp:TemplateField>
                     <HeaderStyle Width="3%" Height ="20px" HorizontalAlign="Center"></HeaderStyle>
                     <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                     <ItemTemplate>
                            <asp:Label ID ="UI_SeqID" runat = "server" text='<%# me.UI_CustomerUser.Rows.Count +1 %>'></asp:Label>
                            <asp:Label ID ="UI_Cuusad" runat = "server" text='<%# Eval("CUUS_AD") %>' Visible ="false" ></asp:Label>
                            <asp:Label ID ="UI_oldAccountID" runat = "server" text='<%# Eval("CUUS_oldAccountID") %>' Visible ="false" ></asp:Label>
                            <asp:Label ID ="UI_ISMANAGER" runat = "server" text='<%# Eval("CUUS_ISMANAGER") %>' Visible ="false"></asp:Label>
                            <asp:Label ID ="UI_ISStatus" runat = "server" text='<%# Eval("CUUS_STATUS") %>' Visible ="false"></asp:Label>
                     </ItemTemplate>
                  </asp:TemplateField>
                  
                  <asp:TemplateField HeaderText="024_User ID">
                     <HeaderStyle Width="15%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                     <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                     <ItemTemplate>
                            <asp:TextBox ID="UI_AccountID" runat ="server" Width ="100px" Text='<%# Eval("CUUS_ACCOUNTID") %>'  CssClass="erma-input-input" ></asp:TextBox>
                            <asp:Label ID="UI_lblAccountID" runat ="server" Text='<%# Eval("CUUS_ACCOUNTID") %>' Visible="false"></asp:Label>
                            <asp:RequiredFieldValidator ID="rfv_AccountID" runat="server" ErrorMessage="036_請輸入帳號" Display="None" ValidationGroup="CustomerGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
                     </ItemTemplate>
                  </asp:TemplateField>
                  
                  <asp:TemplateField HeaderText="025_Password">
                     <HeaderStyle Width="15%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                     <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                     <ItemTemplate>
                            <asp:TextBox ID="UI_Pwd" runat ="server" Width ="80px" Text='<%# Eval("CUUS_PWD") %>'  CssClass="erma-input-input" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfv_Password" runat="server" ErrorMessage="037_請輸入密碼" Display="None" ValidationGroup="CustomerGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
                     </ItemTemplate>
                  </asp:TemplateField>
                  
                  <asp:TemplateField HeaderText="011_TEL">
                     <HeaderStyle Width="15%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                     <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                     <ItemTemplate>
                            <asp:TextBox ID="UI_Tel" runat ="server" Width ="120px" Text='<%# Eval("CUUS_TEL") %>'  CssClass="erma-input-input" ></asp:TextBox>
                     </ItemTemplate>
                  </asp:TemplateField>
                  
                  <asp:TemplateField HeaderText="026_Address">
                     <HeaderStyle Width="30%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                     <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                     <ItemTemplate>
                            <asp:TextBox ID="UI_EMail" runat ="server" Width ="300px" Text='<%# Eval("CUUS_EMAIL") %>' CssClass="erma-input-input" ></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revUIEMail_1" runat="server" ErrorMessage="035_EMail輸入格式有誤"  Display="None" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="UserGroup" SetFocusOnError="true"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="revUIEMail_2" runat="server" ErrorMessage="035_EMail輸入格式有誤" Display="None" ValidationGroup="UserGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
                     </ItemTemplate>
                  </asp:TemplateField>           
               
                  <asp:TemplateField HeaderText="005_Status">
                     <HeaderStyle Width="5%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                     <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                     <ItemTemplate>
                            <asp:Label ID ="UI_lblStatus" runat = "server" Visible ="false"></asp:Label>
                            <asp:DropDownList ID="UI_Status" runat="server" Visible="false">
		                        <asp:ListItem Value ="1" Text ="007_Open" Selected ="True"></asp:ListItem>
		                        <asp:ListItem Value ="0" Text ="008_Close"></asp:ListItem>
		                    </asp:DropDownList>
                     </ItemTemplate>
                  </asp:TemplateField>
                  
                  <asp:TemplateField HeaderText="012_Manager">
                     <HeaderStyle Width="8%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                     <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                     <ItemTemplate>
                            <asp:Label ID ="UI_Manager" runat = "server" text=""></asp:Label>
                     </ItemTemplate>
                  </asp:TemplateField>
                </Columns> 
                <HeaderStyle CssClass="Text_Head"/>
                <RowStyle CssClass="TR_1" />
                <AlternatingRowStyle CssClass="ListRowEven" />
                <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Center" />
            </asp:GridView>
        
                     <!--[End]新增資料列表-->

                    <asp:Button ID="UI_cmdCancel" runat="server" Text="_Cancel" CssClass="btn btn-primary" ValidationGroup="UserGroup" OnClick="UI_cmdCancel_Click" OnClientClick="onProgress('Process')" CausesValidation="false" />&nbsp;
                    <asp:Button ID="UI_cmdSubmit" runat="server" Text="_Submit" CssClass="btn btn-primary" ValidationGroup="UserGroup" OnClick="UI_cmdSubmit_Click" OnClientClick="onProgress('Save')" />

                    <uc2:ucMessage ID="ucMessage" runat="server" />

                    <asp:Label ID="UI_lblPreviousPage_CuNo" runat="server" Visible="true"></asp:Label>
                    <asp:Label ID="UI_lblPreviousPage_CuusID" runat="server" Visible="false"></asp:Label>
                    </div>

</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="UI_cmdCancel" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="UI_cmdSubmit" EventName="Click" />
<asp:PostBackTrigger ControlID="ucMessage" />
</Triggers>
</asp:UpdatePanel>

                

                    <asp:ValidationSummary ID="vsCustomer" runat ="server" ShowMessageBox ="true" ShowSummary ="false" ValidationGroup="CustomerGroup"/>


 