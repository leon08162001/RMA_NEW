Imports System.Runtime.Serialization
Imports Microsoft.VisualBasic

<DataContract>
Public Class OrderRequest


    <DataMember(Name:="application_context", EmitDefaultValue:=False)>
    Public Property ApplicationContext As ApplicationContext

    <DataMember(Name:="intent", EmitDefaultValue:=False)>
    Public Property CheckoutPaymentIntent As String

    <DataMember(Name:="payer", EmitDefaultValue:=False)>
    Public Property Payer As Payer

    <DataMember(Name:="processing_instruction", EmitDefaultValue:=False)>
    Public Property ProcessingInstruction As String

    <DataMember(Name:="purchase_units", EmitDefaultValue:=False)>
    Public Property PurchaseUnits As List(Of PurchaseUnitRequest)

End Class
