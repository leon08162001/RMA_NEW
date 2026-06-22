<%@ Control Language="VB" AutoEventWireup="false" CodeFile="uc_Wait.ascx.vb" Inherits="ascx_uc_Wait" %>



<script>
			// 顯示讀取遮罩
			function ShowProgressBar() {
			displayProgress();
			displayMaskFrame();
			}

			// 隱藏讀取遮罩
			function HideProgressBar() {
			var progress = $('#divProgress');
			var maskFrame = $("#divMaskFrame");
			progress.hide();
			maskFrame.hide();
			}
			// 顯示讀取畫面
			function displayProgress() {
			var w = $(document).width();
			var h = $(window).height();
			var progress = $('#divProgress');
			progress.css({ "z-index": 999999, "top": (h / 2) - (progress.height() / 2), "left": (w / 2) - (progress.width() / 2) });
			progress.show();
			}
			// 顯示遮罩畫面
			function displayMaskFrame() {
			var w = $(window).width();
			var h = $(document).height();
			var maskFrame = $("#divMaskFrame");
			maskFrame.css({ "z-index": 999998, "opacity": 0.7, "width": w, "height": h });
			maskFrame.show();
			}


			</script>
			<div id="divProgress" style="text-align:center; display: none; position: fixed; top: 50%;  left: 50%;" >
			
			
			<style>


        .loading-container {
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
            height: 100vh;
            background-color: rgba(255, 255, 255, 0.3);
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
        }

        .loading-box {
            position: relative;
        }

        .loading-spinners {
            border: 10px solid #3498db;
            border-width: 10px;
            border-style: solid;
            border-radius: 50%;
            width: 60px;
            height: 60px;
        }

        .loading-spinner {
            animation: spin 3s linear infinite;
            border-top: 10px solid transparent;
            border-right: 10px solid transparent;
        }

        .loading-spinner2 {
            position: absolute;
            top: 0;
            border: 10px solid #3498db00;
            border-top: 10px solid transparent;
            border-right: 10px solid transparent;
            animation: spin2 2s 0s linear infinite;
            animation-delay: -1s;
        }

        .loading-spinner3 {
            position: absolute;
            top: 0;
            border: 10px solid #3498db00;
            border-top: 10px solid transparent;
            border-right: 10px solid transparent;
            animation: spin2 2s 1s linear infinite;
            animation-delay: -1.5s;
        }

        .loading-spinner4 {
            position: absolute;
            top: 0;
            border: 10px solid #3498db00;
            border-top: 10px solid transparent;
            border-right: 10px solid transparent;
            animation: spin2 2s 1s linear infinite;
            animation-delay: -1.9s;
        }

        .loading-ball-holder {
            position: absolute;
            width: 59px;
            height: 100%;
            left: 0px;
            right: 0;
            top: 0px;
            bottom: 0;
            margin: auto;
            transform: rotate(0deg);
        }

        .loading-balls {
            position: absolute;
            top: 0;
            left: 0;
            width: 10px;
            height: 10px;
            border-radius: 100%;
            background: #3498db;
            animation: backgroundColor 3s linear infinite;
        }

        .loading-ball2 {
            bottom: 0;
            right: 0;
            top: unset;
            left: unset;
        }

        @keyframes spin {
            0% {
                transform: rotate(0deg);
                border: 10px solid #3498db;
                border-top: 10px solid transparent;
                border-right: 10px solid transparent;
            }

            25% {
                border: 10px solid #0bdec2;
                border-top: 10px solid transparent;
                border-right: 10px solid transparent;

            }

            50% {
                border: 10px solid #f7d423;
                border-top: 10px solid transparent;
                border-right: 10px solid transparent;
            }

            75% {
                border: 10px solid #d91487;
                border-top: 10px solid transparent;
                border-right: 10px solid transparent;
            }


            100% {
                transform: rotate(360deg);
                border: 10px solid #3498db;
                border-top: 10px solid transparent;
                border-right: 10px solid transparent;

            }
        }


        @keyframes spin2 {
            0% {
                transform: rotate(0deg);
            }

            100% {
                transform: rotate(360deg);
            }
        }


        @keyframes backgroundColor {
            0% {
                background: #3498db;
            }

            25% {
                background: #0bdec2;
            }

            50% {
                background: #f7d423;
            }

            75% {
                background: #d91487;

            }

            100% {
                background: #3498db;
            }
        }

        .erma-wait-text{
            margin-top:20px;
        }
    </style>
	
	<div class="loading-container">
        <div class="loading-box">
            <div class="loading-spinners loading-spinner">
                <div class="loading-ball-holder">
                    <div class="loading-balls loading-ball1"></div>
                    <div class="loading-balls loading-ball2"></div>
                </div>
            </div>
            <div class="loading-spinners loading-spinner2">
                <div class="loading-ball-holder">
                    <div class="loading-balls"></div>
                    <!-- <div class="loading-balls loading-ball-m"></div> -->
                </div>
            </div>
            <div class="loading-spinners loading-spinner3">
                <div class="loading-ball-holder">
                    <div class="loading-balls"></div>
                    <!-- <div class="loading-balls loading-ball-m"></div> -->
                </div>
            </div>
            <div class="loading-spinners loading-spinner4">
                <div class="loading-ball-holder">
                    <div class="loading-balls"></div>
                    <!-- <div class="loading-balls loading-ball-m"></div> -->
                </div>
            </div>
        </div>
            <asp:Label ID="loadingLabel" CssClass="erma-wait-text" runat="server" Text=""></asp:Label>
    </div>
			


			</div>
			<div id="divMaskFrame" style="background-color: #F2F4F7; display: none; left: 0px;
			position: absolute; top: 0px;">
			</div>
			
			
			
			
			