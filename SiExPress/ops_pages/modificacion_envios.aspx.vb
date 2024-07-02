
Partial Class ops_pages_modificacion_envios
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Dim usuarioId As Integer = Integer.Parse(CType(HttpContext.Current.Session("id_usuario"), String))
        Dim mensaje As String = String.Empty
        If usuarioId > 0 Then
            If Not String.IsNullOrEmpty(txtEnvio.Text) Then
                Dim numeroEnvio = 0
                Integer.TryParse(txtEnvio.Text, numeroEnvio)

                If numeroEnvio <= 0 Then
                    Label2.Text = "Ocurrió un error, por favor revise los datos ---> Envio invalido"
                    ModalPopupExtender3.Show()
                    Exit Sub
                End If

                If Not String.IsNullOrEmpty(txtTotalEnvio.Text) Then
                    Dim totalEnvio As Double = 0
                    Double.TryParse(txtTotalEnvio.Text, totalEnvio)

                    If totalEnvio <= 0 Then
                        Label2.Text = "Ocurrió un error, por favor revise los datos ---> Total del envio debe ser mayor a cero"
                        ModalPopupExtender3.Show()
                        Exit Sub
                    End If

                    Dim respuestaTotalEnvio = DaspackDALC.ModificacionTotalEnvio(numeroEnvio, totalEnvio, txtComentarios.Text, usuarioId)
                    If respuestaTotalEnvio = True Then
                        mensaje = "Total Envio."
                    Else
                        mensaje = "Total envio no pudo ser actualizado."
                    End If
                End If

                If Not String.IsNullOrEmpty(txtReferencia.Text) Then
                    Dim respuestaReferenciaFedex = DaspackDALC.ModificacionReferenciaFedex(numeroEnvio, txtReferencia.Text, txtComentarios.Text, usuarioId)
                    If respuestaReferenciaFedex = True Then
                        mensaje = mensaje + " Referencia Fedex."
                    Else
                        mensaje = mensaje + " Referencia no pudo ser actualizada."
                    End If
                End If

                If String.IsNullOrWhiteSpace(txtImporteFacturaProveedor.Text) Then
                    txtImporteFacturaProveedor.Text = "0"
                End If

                If Not String.IsNullOrEmpty(txtCasetas.Text) Then
                    Dim decimalValue As Double = 0
                    Double.TryParse(txtCasetas.Text, decimalValue)

                    If decimalValue <= 0 Then
                        Label2.Text = "Ocurrió un error, por favor revise los datos ---> Casetas debe ser mayor a cero"
                        ModalPopupExtender3.Show()
                        Exit Sub
                    End If

                    Dim respuestaTotalEnvio = DaspackDALC.ModificacionCasetas(numeroEnvio, txtComentarios.Text, decimalValue, usuarioId)
                    If respuestaTotalEnvio = True Then
                        mensaje = mensaje + " Casetas."
                    Else
                        mensaje = mensaje + " Casetas no pudo ser actualizado."
                    End If
                End If

                If Not String.IsNullOrEmpty(txtGastos.Text) Then
                    Dim decimalValue As Double = 0
                    Double.TryParse(txtGastos.Text, decimalValue)

                    If decimalValue <= 0 Then
                        Label2.Text = "Ocurrió un error, por favor revise los datos ---> Gastos debe ser mayor a cero"
                        ModalPopupExtender3.Show()
                        Exit Sub
                    End If

                    Dim respuestaTotalEnvio = DaspackDALC.ModificacionGastos(numeroEnvio, txtComentarios.Text, decimalValue, usuarioId)
                    If respuestaTotalEnvio = True Then
                        mensaje = mensaje + " Gastos."
                    Else
                        mensaje = mensaje + " Gastos no pudo ser actualizado."
                    End If
                End If

                If Not String.IsNullOrEmpty(txtViaticos.Text) Then
                    Dim decimalValue As Double = 0
                    Double.TryParse(txtViaticos.Text, decimalValue)

                    If decimalValue <= 0 Then
                        Label2.Text = "Ocurrió un error, por favor revise los datos ---> Viaticos debe ser mayor a cero"
                        ModalPopupExtender3.Show()
                        Exit Sub
                    End If

                    Dim respuestaTotalEnvio = DaspackDALC.ModificacionViaticos(numeroEnvio, txtComentarios.Text, decimalValue, usuarioId)
                    If respuestaTotalEnvio = True Then
                        mensaje = mensaje + " Viaticos."
                    Else
                        mensaje = mensaje + " Viaticos no pudo ser actualizado."
                    End If
                End If

                If Not String.IsNullOrEmpty(txtPension.Text) Then
                    Dim decimalValue As Double = 0
                    Double.TryParse(txtPension.Text, decimalValue)

                    If decimalValue <= 0 Then
                        Label2.Text = "Ocurrió un error, por favor revise los datos ---> Pension debe ser mayor a cero"
                        ModalPopupExtender3.Show()
                        Exit Sub
                    End If

                    Dim respuestaTotalEnvio = DaspackDALC.ModificacionPension(numeroEnvio, txtComentarios.Text, decimalValue, usuarioId)
                    If respuestaTotalEnvio = True Then
                        mensaje = mensaje + " Pension."
                    Else
                        mensaje = mensaje + " Pension no pudo ser actualizado."
                    End If
                End If

                If Not String.IsNullOrEmpty(txtManiobrasCliente.Text) Then
                    Dim decimalValue As Double = 0
                    Double.TryParse(txtManiobrasCliente.Text, decimalValue)

                    If decimalValue <= 0 Then
                        Label2.Text = "Ocurrió un error, por favor revise los datos ---> Maniobras Cliente debe ser mayor a cero"
                        ModalPopupExtender3.Show()
                        Exit Sub
                    End If

                    Dim respuestaTotalEnvio = DaspackDALC.ModificacionManiobrasCliente(numeroEnvio, txtComentarios.Text, decimalValue, usuarioId)
                    If respuestaTotalEnvio = True Then
                        mensaje = mensaje + " Maniobras Cliente."
                    Else
                        mensaje = mensaje + " Maniobras Cliente no pudo ser actualizado."
                    End If
                End If

                If Not String.IsNullOrEmpty(txtManiobrasPropias.Text) Then
                    Dim decimalValue As Double = 0
                    Double.TryParse(txtManiobrasPropias.Text, decimalValue)

                    If decimalValue <= 0 Then
                        Label2.Text = "Ocurrió un error, por favor revise los datos ---> Maniobras Propias debe ser mayor a cero"
                        ModalPopupExtender3.Show()
                        Exit Sub
                    End If

                    Dim respuestaTotalEnvio = DaspackDALC.ModificacionManiobrasPropias(numeroEnvio, txtComentarios.Text, decimalValue, usuarioId)
                    If respuestaTotalEnvio = True Then
                        mensaje = mensaje + " Maniobras Propias."
                    Else
                        mensaje = mensaje + " Maniobras Propias no pudo ser actualizado."
                    End If
                End If

                If Not String.IsNullOrEmpty(txtEstadias.Text) Then
                    Dim decimalValue As Double = 0
                    Double.TryParse(txtEstadias.Text, decimalValue)

                    If decimalValue <= 0 Then
                        Label2.Text = "Ocurrió un error, por favor revise los datos ---> Estadias debe ser mayor a cero"
                        ModalPopupExtender3.Show()
                        Exit Sub
                    End If

                    Dim respuestaTotalEnvio = DaspackDALC.ModificacionEstadias(numeroEnvio, txtComentarios.Text, decimalValue, usuarioId)
                    If respuestaTotalEnvio = True Then
                        mensaje = mensaje + " Estadias."
                    Else
                        mensaje = mensaje + " Estadias no pudo ser actualizado."
                    End If
                End If

                If Not String.IsNullOrEmpty(txtDemoras.Text) Then
                    Dim decimalValue As Double = 0
                    Double.TryParse(txtDemoras.Text, decimalValue)

                    If decimalValue <= 0 Then
                        Label2.Text = "Ocurrió un error, por favor revise los datos ---> Demoras debe ser mayor a cero"
                        ModalPopupExtender3.Show()
                        Exit Sub
                    End If

                    Dim respuestaTotalEnvio = DaspackDALC.ModificacionDemoras(numeroEnvio, txtComentarios.Text, decimalValue, usuarioId)
                    If respuestaTotalEnvio = True Then
                        mensaje = mensaje + " Demoras."
                    Else
                        mensaje = mensaje + " Demoras no pudo ser actualizado."
                    End If
                End If

                If Not String.IsNullOrEmpty(txtNombreProveedor.Text) Then

                    Dim respuestaTotalEnvio = DaspackDALC.ModificacionNombreProveedor(numeroEnvio, txtComentarios.Text, txtNombreProveedor.Text, usuarioId)
                    If respuestaTotalEnvio = True Then
                        mensaje = mensaje + " Nombre Proveedor."
                    Else
                        mensaje = mensaje + " Nombre Proveedor no pudo ser actualizado."
                    End If
                End If

                Dim respuesta = DaspackDALC.ModificacionEnvioProveedor(numeroEnvio, txtComentarios.Text, DropDownProveedores.SelectedValue, usuarioId, txtNoFactura.Text, txtImporteFacturaProveedor.Text, txtGratificacion.Text)
                If respuesta = True Then
                    mensaje = mensaje + " Proveedor Envio actualizado y datos adicionales."
                    txtComentarios.Text = ""
                    txtEnvio.Text = ""
                    txtTotalEnvio.Text = ""
                    txtReferencia.Text = ""
                    txtNoFactura.Text = ""
                    txtImporteFacturaProveedor.Text = ""
                    txtGratificacion.Text = ""
                Else
                    mensaje = mensaje + " Proveedor envio no pudo ser actualizado."
                End If
            End If
        Else
            mensaje = "Usuario Invalido"
        End If
        Label2.Text = mensaje
        ModalPopupExtender3.Show()
        Exit Sub
    End Sub
End Class
