Imports System.Runtime.Serialization
Imports Microsoft.VisualBasic


<DataContract>
Public Class Phone
    <DataMember(Name:="country_code", EmitDefaultValue:=False)>
    Public Property CountryCallingCode As String

    <DataMember(Name:="extension_number", EmitDefaultValue:=False)>
    Public Property ExtensionNumber As String

    <DataMember(Name:="national_number", EmitDefaultValue:=False)>
    Public Property NationalNumber As String

End Class
