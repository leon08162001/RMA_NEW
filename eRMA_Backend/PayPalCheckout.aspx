<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PayPalCheckout.aspx.vb" Inherits="PayPalCheckout" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <div id="paypal-button-container"></div>

    <script src="https://www.paypal.com/sdk/js?client-id=AdphsgsB0AE9ykwwxzv3X8OZp3L3c-ArKvIU5df5Jw8nKCk1ZRaMnyjz9WB6J18HJIHOyK3IJg2tcQ_X"></script>
    <script>
        paypal.Buttons({
            createOrder: function (data, actions) {

                var sAmt = location.search.replace('?sAmt=', '');

                return actions.order.create({

                    purchase_units: [{
                        amount: {
                            value: sAmt
                        }

                    }]
                })
            }

        }).render('#paypal-button-container')
    </script>

</body>
</html>

