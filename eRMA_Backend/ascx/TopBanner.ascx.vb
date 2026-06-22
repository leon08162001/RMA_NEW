
Imports System.IO

Partial Class ascx_TopBanner
    Inherits System.Web.UI.UserControl




    Protected Sub UI_hrefLogout_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_hrefLogout.Click
        Response.Redirect("/logout.aspx")
    End Sub

    Protected Sub UI_linkUserGuide_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UI_linkUserGuide.Click
        Dim buffer As Byte() = File.ReadAllBytes(Server.MapPath("~/FILE/manual/user_guide_En.pdf"))
        Dim ms As New MemoryStream(buffer)
        ms.Position = 0
        ms.Read(buffer, 0, Integer.Parse(ms.Length.ToString()))
        Response.Clear()
        Response.AddHeader("Content-Disposition", ("attachment;filename=RMA_UserGuide_2024.pdf"))
        Response.AddHeader("Content-Length", ms.Length.ToString())
        Response.ContentType = "application/pdf"
        ms.Close()
        Response.BinaryWrite(buffer)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
    End Sub


End Class
