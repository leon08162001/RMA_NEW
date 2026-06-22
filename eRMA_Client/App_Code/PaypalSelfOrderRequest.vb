Imports System.Runtime.Serialization
Imports Microsoft.VisualBasic

'<DataContract>
Public Class PaypalSelfOrderRequest
    Public Property PayType As String
    '<DataMember(Name:="brandname", EmitDefaultValue:=False)>
    Public Property BrandName As String
    '<DataMember(Name:="landingpage", EmitDefaultValue:=False)>
    Public Property LandingPage As String
    '<DataMember(Name:="paymentintent", EmitDefaultValue:=False)>
    Public Property PaymentIntent As String
    '<DataMember(Name:="useraction", EmitDefaultValue:=False)>
    Public Property UserAction As String
    '<DataMember(Name:="cancelurl", EmitDefaultValue:=False)>
    Public Property CancelUrl As String
    '<DataMember(Name:="returnurl", EmitDefaultValue:=False)>
    Public Property ReturnUrl As String
    '<DataMember(Name:="referenceid", EmitDefaultValue:=False)>
    Public Property ReferenceId As String
    '秀在刷信用卡下拉頁,備份
    '<DataMember(Name:="description", EmitDefaultValue:=False)>
    Public Property Description As String
    '<DataMember(Name:="shippingpreference", EmitDefaultValue:=False)>
    Public Property ShippingPreference As String
    '自定義
    '<DataMember(Name:="customid", EmitDefaultValue:=False)>
    Public Property CustomId As String
    '<DataMember(Name:="softdescriptor", EmitDefaultValue:=False)>
    Public Property SoftDescriptor As String
    '<DataMember(Name:="currencycode", EmitDefaultValue:=False)>
    Public Property CurrencyCode As String
    '<DataMember(Name:="value", EmitDefaultValue:=False)>
    Public Property Value As String
    '<DataMember(Name:="totalvalue", EmitDefaultValue:=False)>
    Public Property TotalValue As String
    '<DataMember(Name:="shippingvalue", EmitDefaultValue:=False)>
    Public Property ShippingValue As String
    '<DataMember(Name:="handlingvalue", EmitDefaultValue:=False)>
    Public Property HandlingValue As String
    '<DataMember(Name:="taxvalue", EmitDefaultValue:=False)>
    Public Property TaxValue As String
    '<DataMember(Name:="shippingdiscountvalue", EmitDefaultValue:=False)>
    Public Property ShippingDiscountValue As String
    Public Property OrderDetails As List(Of OrderDetail)
    Public Property ShippingAddr As ShippingAddr
End Class

'<DataContract>
Public Class OrderDetail
    Public Property Name As String
    Public Property Description As String
    Public Property Category As String
    Public Property Sku As String
    Public Property Quantity As String
    Public Property AmountValue As String
    Public Property TaxValue As String
End Class

'<DataContract>
Public Class ShippingAddr
    Public Property Name As String
    Public Property AddressLine1 As String
    Public Property AddressLine2 As String
    Public Property AdminArea1 As String
    Public Property AdminArea2 As String
    Public Property PostalCode As String
    Public Property CountryCode As String
End Class


<DataContract>
Public Class LoginData
    '<JsonPropertyName("username")>
    <DataMember(Name:="username", EmitDefaultValue:=False)>
    Public Property UserName As String
    '<JsonPropertyName("password")>
    <DataMember(Name:="password", EmitDefaultValue:=False)>
    Public Property Password As String
End Class

Public Class TokenData
    Public Property token As String

End Class


