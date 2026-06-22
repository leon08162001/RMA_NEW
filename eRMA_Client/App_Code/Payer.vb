Imports System.Runtime.Serialization
Imports Microsoft.VisualBasic

<DataContract>
Public Class Payer

    <DataMember(Name:="address", EmitDefaultValue:=False)>
    Public Property AddressPortable As AddressPortable

    <DataMember(Name:="birth_date", EmitDefaultValue:=False)>
    Public Property BirthDate As String

    <DataMember(Name:="email_address", EmitDefaultValue:=False)>
    Public Property Email As String

    <DataMember(Name:="name", EmitDefaultValue:=False)>
    Public Property Name As Name

    <DataMember(Name:="payer_id", EmitDefaultValue:=False)>
    Public Property PayerId As String

    <DataMember(Name:="phone", EmitDefaultValue:=False)>
    Public Property PhoneWithType As PhoneWithType

    <DataMember(Name:="tax_info", EmitDefaultValue:=False)>
    Public Property TaxInfo As TaxInfo
End Class
