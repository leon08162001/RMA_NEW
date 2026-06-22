Imports System.Runtime.Serialization
Imports Microsoft.VisualBasic

<DataContract>
Public Class AddressPortable
    '<DataMember(Name:="address_details", EmitDefaultValue:=False)>
    'Public Property AddressDetails As AddressDetails

    <DataMember(Name:="address_line_1", EmitDefaultValue:=False)>
    Public Property AddressLine1 As String

    <DataMember(Name:="address_line_2", EmitDefaultValue:=False)>
    Public Property AddressLine2 As String

    <DataMember(Name:="address_line_3", EmitDefaultValue:=False)>
    Public Property AddressLine3 As String

    <DataMember(Name:="admin_area_1", EmitDefaultValue:=False)>
    Public Property AdminArea1 As String

    <DataMember(Name:="admin_area_2", EmitDefaultValue:=False)>
    Public Property AdminArea2 As String

    <DataMember(Name:="admin_area_3", EmitDefaultValue:=False)>
    Public Property AdminArea3 As String

    <DataMember(Name:="admin_area_4", EmitDefaultValue:=False)>
    Public Property AdminArea4 As String

    <DataMember(Name:="country_code", EmitDefaultValue:=False)>
    Public Property CountryCode As String

    <DataMember(Name:="postal_code", EmitDefaultValue:=False)>
    Public Property PostalCode As String
End Class
