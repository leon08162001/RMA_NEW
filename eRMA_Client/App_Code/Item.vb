Imports System.Runtime.Serialization
Imports Microsoft.VisualBasic

<DataContract>
Public Class Item
    <DataMember(Name:="category", EmitDefaultValue:=False)>
    Public Property Category As String

    <DataMember(Name:="description", EmitDefaultValue:=False)>
    Public Property Description As String

    <DataMember(Name:="name", EmitDefaultValue:=False)>
    Public Property Name As String

    <DataMember(Name:="quantity", EmitDefaultValue:=False)>
    Public Property Quantity As String

    <DataMember(Name:="sku", EmitDefaultValue:=False)>
    Public Property Sku As String

    <DataMember(Name:="tax", EmitDefaultValue:=False)>
    Public Property Tax As Money

    <DataMember(Name:="unit_amount", EmitDefaultValue:=False)>
    Public Property UnitAmount As Money
End Class
