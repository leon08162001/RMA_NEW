Imports System.Runtime.Serialization
Imports Microsoft.VisualBasic

<DataContract>
Public Class PhoneWithType
    <DataMember(Name:="phone_number", EmitDefaultValue:=False)>
    Public Property PhoneNumber As Phone

    <DataMember(Name:="phone_type", EmitDefaultValue:=False)>
    Public Property PhoneType As String
End Class
