Imports System.Runtime.Serialization

<DataContract>
Public Class PurchaseUnitRequest

    <DataMember(Name:="amount", EmitDefaultValue:=False)>
    Public Property AmountWithBreakdown As AmountWithBreakdown

    <DataMember(Name:="custom_id", EmitDefaultValue:=False)>
    Public Property CustomId As String

    <DataMember(Name:="description", EmitDefaultValue:=False)>
    Public Property Description As String

    <DataMember(Name:="invoice_id", EmitDefaultValue:=False)>
    Public Property InvoiceId As String
    <DataMember(Name:="items", EmitDefaultValue:=False)>
    Public Property Items As List(Of Item)

    '<DataMember(Name:="payee", EmitDefaultValue:=False)>
    'Public Property Payee As Payee

    '<DataMember(Name:="payment_instruction", EmitDefaultValue:=False)>
    'Public Property PaymentInstruction As PaymentInstruction

    <DataMember(Name:="reference_id", EmitDefaultValue:=False)>
    Public Property ReferenceId As String

    <DataMember(Name:="shipping", EmitDefaultValue:=False)>
    Public Property ShippingDetail As ShippingDetail

    <DataMember(Name:="soft_descriptor", EmitDefaultValue:=False)>
    Public Property SoftDescriptor As String

End Class
