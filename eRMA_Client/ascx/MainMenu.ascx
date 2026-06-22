<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MainMenu.ascx.vb" Inherits="ascx_MainMenu" %>

<!--<script src="https://cdn.staticfile.org/jquery-cookie/1.4.1/jquery.cookie.min.js"></script> -->

<style>
@media screen and (max-width: 1366px) {
	*{
		Font-Size:12px;
	}
}

</style>

<script> 

    $(document).ready(function () {

    var index_check = "0";
	var genericTime = 300;

    $("#Show_Close_Btn").click(function () {

    $("#Show_Close_Window_Left").animate({ width: 'toggle', opacity: 'toggle' },genericTime);

    if (index_check == "0") {
    index_check = "1";

    $("#Show_Close_Window_Right").delay(100).animate({ position: 'absolute', left: '5px', width: '100%' },genericTime);

    }
    else {
        index_check = "0";

        $("#Show_Close_Window_Right").animate({  width: '75%'  },genericTime);
         
    }
    });
	
	$("#Show_Close_Btn_Left").click(function () {
     index_check = "1";
    $("#Show_Close_Window_Left").animate({ width: 'toggle', opacity: 'toggle' },genericTime);

    $("#Show_Close_Window_Right").delay(100).animate({ position: 'absolute', left: '5px', width: '100%' },genericTime);

    });
	
			
	if ($(window).width() > 1500) {
	
	}
	else
	{
document.getElementById("Show_Close_Btn").click();
	}
	
    });


    function goUrl() {
        window.location = "Client_FlowCase01_Worklist.aspx";

    }






</script> 

<style>
    .HyperLink_a {
    display:block;
    }

</style>

<ul>
    <li class="erma-tab-expand" id="Show_Close_Btn">
         <img style="width:15px;height:auto;" src="../images/kisspng-hamburger-button-computer-icons-menu-ios-7-5aebaafc34d670.2160278215253941722164.png"  />
    </li>

    <li class="erma-tab-wait active"  ID="Unprocessed_li" runat="server"    onclick="goUrl();"   >
		<asp:HyperLink runat="server" id="UI_linButton00"  NavigateUrl="~/Client_FlowCase01_Worklist.aspx"  CssClass="HyperLink_a" > </asp:HyperLink>    
    </li>
   
    <li class="erma-tab-query active" >
		<asp:Label ID="Query_Area_Lab" runat="server" Text="Query Area" class="Main_a"></asp:Label>    
		<img src="../images/ExpandArrow.png" alt="">
		<div class="erma-tab-query">                  
			
			<ul>		
				<li class="erma-query-li" runat="server" ID="UI_linButton01_li"   >
					<asp:HyperLink runat="server" id="UI_linButton01"  CssClass="HyperLink_a"  >
					<asp:Image runat="server" ID="button_01" name="button_01" ImageUrl="~/images/pic_04.gif"  onMouseOver="img_act('button_01');return" onMouseOut="img_inact('button_01')"></asp:Image>
					</asp:HyperLink>    
				</li>
				
				<li  class="erma-query-li" runat="server" ID="UI_linButton02_li"    >
					<asp:HyperLink runat="server" id="UI_linButton02"   CssClass="HyperLink_a"  >
					<asp:Image runat="server" ID="button_02" name="button_02" ImageUrl="~/images/pic_05.gif"  onMouseOver="img_act('button_02');return" onMouseOut="img_inact('button_02')"></asp:Image>
					</asp:HyperLink>    
				</li>
				
				<li  class="erma-query-li"  runat="server" ID="UI_linButton03_li"    >
					<asp:HyperLink runat="server" id="UI_linButton03" CssClass="HyperLink_a"  >
					<asp:Image runat="server" ID="Image1" name="button_03" ImageUrl="~/images/pic_06.gif"  onMouseOver="img_act('button_03');return" onMouseOut="img_inact('button_03')"></asp:Image>
					</asp:HyperLink>    
				</li>

				<li  class="erma-query-li"  runat="server" ID="UI_linButton07_li" >
					<asp:HyperLink runat="server" id="UI_linButton07"  CssClass="HyperLink_a"  >
					<asp:Image runat="server" ID="button_07" name="button_07" ImageUrl="~/images/pic_menu_06.gif"  onMouseOver="img_act('button_07');return" onMouseOut="img_inact('button_07')"></asp:Image>
					</asp:HyperLink>    
				</li>

				<li  class="erma-query-li" runat="server" ID="UI_linButton04_li">
					<asp:HyperLink runat="server" id="UI_linButton04"   CssClass="HyperLink_a"  >
					<asp:Image runat="server" ID="button_04" name="button_04" ImageUrl="~/images/pic_07.gif"  onMouseOver="img_act('button_04');return" onMouseOut="img_inact('button_04')"></asp:Image>
					</asp:HyperLink>    
				</li>

				<li  class="erma-query-li"  runat="server" ID="UI_linButton05_li">
					<asp:HyperLink runat="server" id="UI_linButton05"   CssClass="HyperLink_a" >
					<asp:Image runat="server" ID="button_05" name="button_05" ImageUrl="~/images/pic_08.gif"  onMouseOver="img_act('button_05');return" onMouseOut="img_inact('button_05')"></asp:Image>
					</asp:HyperLink>    
				</li>
				
				
				<li  class="erma-query-li"  runat="server" ID="UI_linButton06_li">
					<asp:HyperLink runat="server" id="UI_linButton06"   CssClass="HyperLink_a" >
					<asp:Image runat="server" ID="button_06" name="button_06" ImageUrl="~/images/pic_24.gif"  onMouseOver="img_act('button_06');return" onMouseOut="img_inact('button_06')"></asp:Image>
					</asp:HyperLink>  
				</li>
				
				<li  class="erma-query-li" runat="server" ID="UI_linButton08_li">
					<!--JW/shaili-->
					<asp:hyperlink runat="server" id="UI_linButton08" CssClass="HyperLink_a"   >
					<asp:Image runat="server" ID="button_08" name="button_08" ImageUrl="~/images/pic_menu_07.gif"  onMouseOver="img_act('button_08');return" onMouseOut="img_inact('button_08')"></asp:Image>
					</asp:hyperlink> 
				</li>	

				<li  class="erma-query-li" runat="server" ID="UI_linButton09_li">
					<!--JW/shaili-->
					<asp:hyperlink runat="server" id="UI_linButton09"  CssClass="HyperLink_a"  >
					<asp:Image runat="server" ID="button_09" name="button_09" ImageUrl="~/images/pic_menu_07.gif"  onMouseOver="img_act('button_09');return" onMouseOut="img_inact('button_09')"></asp:Image>
					</asp:hyperlink> 
				</li>

				<li  class="erma-query-li" runat="server" ID="UI_linButton10_li" >
					<!--JW/shaili-->
					<asp:hyperlink runat="server" id="UI_linButton10" CssClass="HyperLink_a"  >
					<asp:Image runat="server" ID="button_10" name="button_10" ImageUrl="~/images/pic_menu_07.gif"  onMouseOver="img_act('button_10');return" onMouseOut="img_inact('button_10')"></asp:Image>
					</asp:hyperlink> 
				</li>
			</ul>	
		</div>
    </li>

    <li class="erma-tab-charge"  id="UI_li_linChargeQUOTED" runat="server">
		<asp:HyperLink ID="UI_linChargeQUOTED" runat="server" Text="Charge Quoted" CssClass="menu-1" NavigateUrl="~/ChargeQuoted_List.aspx"></asp:HyperLink>
    </li>

    <li class="erma-tab-Warranty_Selling"  id="UI_li_tbWarrantyOrder"  runat="server"  >
		<asp:Label ID="Label1" runat ="server" Font-Bold="true" Text ="Warranty Selling"></asp:Label>
		<img src="../images/ExpandArrow.png" alt="">
		<div class="erma-tab-query">                  
			<ul>
			
				<li  class="erma-query-li" >
					<asp:HyperLink ID="UI_linWarrantyExtension" runat="server" Text="Ext & Com W.Setting" CssClass="menu-1" NavigateUrl="~/Warranty_TypeSetting.aspx"></asp:HyperLink>
				</li>   
				
				<li  class="erma-query-li" >     
					<asp:HyperLink ID="UI_linWarrantySpecialSetting" runat="server" Text="Special W.Setting" CssClass="menu-1" NavigateUrl="~/Warranty_SpecialSetting.aspx"></asp:HyperLink>
				</li>   
				
				<li  class="erma-query-li" >          
					<asp:HyperLink ID="UI_linWarrantyGroupSetting" runat="server" Text="Warranty Group Setting" CssClass="menu-1" NavigateUrl="~/Warranty_GroupSetting.aspx"></asp:HyperLink>
				</li>   

			</ul>
		</div>		
	</li>

	<li class="erma-tab-batch"   id="UI_li_linWarrantyBatch"  runat="server"    >
		<asp:HyperLink ID="UI_linWarrantyBatch" runat="server" Text="Batch Serial Search" CssClass="menu-1" NavigateUrl="~/Warranty_SerialSearch.aspx"></asp:HyperLink>
	</li>

	<li class="erma-tab-Warranty_Sales_Order"  id="UI_li_linSellingafterOrder"  runat="server"  >
		<asp:HyperLink ID="UI_linSellingafterOrder" runat="server" Text="Warranty Sales Order" CssClass="menu-1" NavigateUrl="~/Warranty_SellingafterOrder.aspx"></asp:HyperLink>
	</li>
 </ul>


