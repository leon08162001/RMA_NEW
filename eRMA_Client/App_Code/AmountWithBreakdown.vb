Imports System.Runtime.Serialization
Imports Microsoft.VisualBasic

<DataContract>
Public Class AmountWithBreakdown
    <DataMember(Name:="breakdown", EmitDefaultValue:=False)>
    Public Property AmountBreakdown As AmountBreakdown

    <DataMember(Name:="currency_code", EmitDefaultValue:=False)>
    Public Property CurrencyCode As String

    <DataMember(Name:="value", EmitDefaultValue:=False)>
    Public Property Value As String
End Class
