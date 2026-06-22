Imports System.Runtime.Serialization
Imports Microsoft.VisualBasic

<DataContract>
Public Class Name
    <DataMember(Name:="alternate_full_name", EmitDefaultValue:=False)>
    Public Property AlternateFullName As String

    <DataMember(Name:="full_name", EmitDefaultValue:=False)>
    Public Property FullName As String

    <DataMember(Name:="given_name", EmitDefaultValue:=False)>
    Public Property GivenName As String

    <DataMember(Name:="middle_name", EmitDefaultValue:=False)>
    Public Property MiddleName As String

    <DataMember(Name:="prefix", EmitDefaultValue:=False)>
    Public Property Prefix As String

    <DataMember(Name:="suffix", EmitDefaultValue:=False)>
    Public Property Suffix As String

    <DataMember(Name:="surname", EmitDefaultValue:=False)>
    Public Property Surname As String
End Class
