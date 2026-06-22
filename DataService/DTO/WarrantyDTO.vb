

Partial Public Class WarrantyDTO
    Partial Class WARRANTYORDDataTable

        Private Sub WARRANTYORDDataTable_ColumnChanging(ByVal sender As System.Object, ByVal e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.WATY_SALESIDColumn.ColumnName) Then
                'Add user code here
            End If

        End Sub

    End Class

End Class
