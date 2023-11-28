Imports System.IO

Partial Class Reports_EstafetaLabel
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim id_envio As Integer

        Try
            If Integer.TryParse(Request.QueryString(0), id_envio) Then
                Dim estafetaLabel = DaspackDALC.EstafetaLabel(id_envio)
                If estafetaLabel IsNot Nothing Then
                    Dim base64String As String = Convert.ToBase64String(estafetaLabel.labelPDF, 0, estafetaLabel.labelPDF.Length)
                    'Dim chtLoginsByMonthStream As MemoryStream = New MemoryStream()
                    'Dim Width As Double = Double.Parse(5.3) * 72
                    'Dim Height As Double = Double.Parse(8.2) * 72

                    ''Read the entire stream
                    'chtLoginsByMonthStream.Read(estafetaLabel.labelPDF, 0, estafetaLabel.labelPDF.Length)

                    'Dim image As New Image
                    'image.Width = CType(Width, Unit)
                    'image.Height = CType(Height, Unit)

                    'image.ImageUrl = "data:image/png;base64," & base64String

                    'image.Visible = True


                    'FedexLabelPanel.Controls.Add(image)


                    If base64String = "" Then
                        Response.Write("Etiqueta no disponible.")
                    Else
                        Response.Buffer = True
                        Response.Clear()
                        Response.ClearContent()
                        Response.ClearHeaders()
                        Response.ContentType = "application/pdf"
                        Response.BinaryWrite(estafetaLabel.labelPDF)
                        Response.End()

                        'Response.AddHeader("Content-Type", "application/pdf")
                        'Response.AddHeader("Content-Length", base64String.Length.ToString())
                        'Response.AddHeader("Content-Disposition", "inline;")
                        'Response.AddHeader("Cache-Control", "private, max-age=0, must-revalidate")
                        'Response.AddHeader("Pragma", "public")
                        'Response.BinaryWrite(Convert.FromBase64String(base64String))
                    End If
                End If
            End If

        Catch ex As Exception
            Response.Write("Ocurrio un error, favor de contactar al administrador." + ex.Message + " - " + ex.Source)
        End Try
    End Sub
End Class
