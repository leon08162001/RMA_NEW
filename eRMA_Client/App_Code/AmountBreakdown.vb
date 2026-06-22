Imports System.Runtime.Serialization
Imports Microsoft.VisualBasic

<DataContract>
Public Class AmountBreakdown
    <DataMember(Name:="discount", EmitDefaultValue:=False)>
    Public Property Discount As Money

    <DataMember(Name:="handling", EmitDefaultValue:=False)>
    Public Property Handling As Money

    <DataMember(Name:="insurance", EmitDefaultValue:=False)>
    Public Property Insurance As Money

    <DataMember(Name:="item_total", EmitDefaultValue:=False)>
    Public Property ItemTotal As Money

    <DataMember(Name:="shipping", EmitDefaultValue:=False)>
    Public Property Shipping As Money

    <DataMember(Name:="shipping_discount", EmitDefaultValue:=False)>
    Public Property ShippingDiscount As Money

    <DataMember(Name:="tax_total", EmitDefaultValue:=False)>
    Public Property TaxTotal As Money

End Class
