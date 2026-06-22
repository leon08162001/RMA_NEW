Imports System.Runtime.Serialization

<DataContract>
Public Class ShippingDetail

    <DataMember(Name:="address", EmitDefaultValue:=False)>
    Public Property AddressPortable As AddressPortable

    <DataMember(Name:="name", EmitDefaultValue:=False)>
    Public Property Name As Name

    '[DataMember(Name="options", EmitDefaultValue = false)]
    '    Public List<ShippingOption> Options;
End Class
