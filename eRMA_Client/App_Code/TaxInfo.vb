Imports System.Runtime.Serialization

<DataContract>
Public Class TaxInfo
    <DataMember(Name:="tax_id", EmitDefaultValue:=False)>
    Public Property TaxId As String

    <DataMember(Name:="tax_id_type", EmitDefaultValue:=False)>
    Public Property TaxIdType As String
End Class
