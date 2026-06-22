Imports System.Runtime.Serialization
Imports Microsoft.VisualBasic


<DataContract>
Public Class ApplicationContext

    <DataMember(Name:="brand_name", EmitDefaultValue:=False)>
    Public Property BrandName As String

    <DataMember(Name:="cancel_url", EmitDefaultValue:=False)>
    Public Property CancelUrl As String

    <DataMember(Name:="landing_page", EmitDefaultValue:=False)>
    Public Property LandingPage As String

    <DataMember(Name:="locale", EmitDefaultValue:=False)>
    Public Property Locale As String


    'Public Property PaymentMethod As PaymentMethod
    'Public Property PaymentToken As String

    <DataMember(Name:="return_url", EmitDefaultValue:=False)>
    Public Property ReturnUrl As String

    <DataMember(Name:="shipping_preference", EmitDefaultValue:=False)>
    Public Property ShippingPreference As String

    <DataMember(Name:="user_action", EmitDefaultValue:=False)>
    Public Property UserAction As String

End Class
