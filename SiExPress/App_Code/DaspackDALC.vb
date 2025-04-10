Imports System.Data
Imports System.Data.Entity
Imports System.Net
Imports SiExProData
Imports iTextSharp.text.pdf
Imports System.Drawing
Imports System.Drawing.Imaging

Public Class DaspackDALC
    Public Shared Property JsonConvert As Object

    Public Shared Function GetModuloPrivilegio(ByVal idModulo As Integer, ByVal idUsuario As Integer, ByVal privilegio As Integer) As Boolean
        Dim db As New DaspackDataContext
        Dim tienePermiso As Boolean = False
        Return True
        Dim userPrivilegesList As IEnumerable(Of PrivilegioModulo) = db.GetPrivilegiosModulo(idModulo, idUsuario)
        Dim usuarioPrivilegios As List(Of PrivilegioModulo) = New List(Of PrivilegioModulo)(userPrivilegesList.ToList())
        Dim usuarioPrivilegio As PrivilegioModulo

        If usuarioPrivilegios IsNot Nothing Then
            usuarioPrivilegio = usuarioPrivilegios.FirstOrDefault()
        End If


        If usuarioPrivilegio IsNot Nothing Then
            Select Case privilegio
                Case TipoPrivilegio.Lee
                    If (usuarioPrivilegio.PuedeLeer = 1) Then
                        tienePermiso = True
                    End If
                Case TipoPrivilegio.Borra
                    If (usuarioPrivilegio.PuedeBorrar = 1) Then
                        tienePermiso = True
                    End If
                Case TipoPrivilegio.Escribe
                    If (usuarioPrivilegio.PuedeEscribir = 1) Then
                        tienePermiso = True
                    End If
            End Select
        End If

        Return tienePermiso
    End Function

    Public Shared Function GetModuloPorDescripcion(ByVal descripcion As String) As Modulo
        Dim db As New DaspackDataContext
        Dim modulo As Modulo = Nothing
        Dim modulosList As IEnumerable(Of Modulo) = db.GetModuloPorDescripcion(descripcion)

        Dim modulos As List(Of Modulo) = New List(Of Modulo)(modulosList.ToList())

        If modulos IsNot Nothing Then
            modulo = modulos.FirstOrDefault()
        End If

        Return modulo

    End Function

    Public Shared Function InsComenatrio(ByVal idEnvio As Integer, ByVal idUsuario As Integer, ByVal comentario As String) As Boolean
        Dim db As New DaspackDataContext
        db.InsComentario(idEnvio, idUsuario, comentario)
        Return True

    End Function

    Public Shared Function GetUsuarioEmpresa(ByVal id_usuario As Integer) As Empresa
        Dim db As New DaspackDataContext
        Dim empresa As Empresa = Nothing
        Dim empresasList As IEnumerable(Of Empresa) = db.GetUsuarioEmpresa(id_usuario)

        Dim empresas As List(Of Empresa) = New List(Of Empresa)(empresasList.ToList())

        If empresas IsNot Nothing Then
            empresa = empresas.FirstOrDefault()
        End If

        Return empresa

    End Function

    Public Shared Function GetAdeudoEmpresaTotal(ByVal id_empresa As Integer) As Decimal
        Dim db As New DaspackDataContext
        Dim adeudo As Decimal = 0
        Dim adeudoEmpresa As IEnumerable(Of AdeudoEmpresa) = db.GetAdeudoEmpresa(id_empresa)

        Dim adeudoEmpresasList As List(Of AdeudoEmpresa) = New List(Of AdeudoEmpresa)(adeudoEmpresa.ToList())

        If adeudoEmpresasList IsNot Nothing Then
            adeudo = adeudoEmpresasList.Sum(Function(x) x.Mensualidad - x.Deposito + (x.NumeroEnvios * x.TarifaPorEnvio))
        End If

        Return adeudo

    End Function

    Public Shared Function GetAdeudos(ByVal id_empresa As Integer) As List(Of AdeudoEmpresa)
        Dim db As New DaspackDataContext
        Dim adeudoEmpresa As IEnumerable(Of AdeudoEmpresa) = db.GetAdeudoEmpresa(id_empresa)

        Dim adeudoEmpresasList As List(Of AdeudoEmpresa) = New List(Of AdeudoEmpresa)(adeudoEmpresa.ToList())

        Return adeudoEmpresasList

    End Function

    Public Shared Function UpdateAdeudoEmpresa(idFactura As Integer, idUsuario As Integer, referencia As String, comentarios As String, facturado As Boolean, deposito As Decimal) As Boolean
        Dim db As New DaspackDataContext
        db.UpdateAdeudoEmpresa(idFactura, idUsuario, referencia, comentarios, facturado, deposito)

        Return True

    End Function

    Public Shared Function GetDatosUsuario(ByVal id_usuario As Integer) As Usuario
        Dim db As New DaspackDataContext

        Dim usuario As Usuario = db.GetDatosUsuario(id_usuario).FirstOrDefault()

        Return usuario

    End Function

    Public Shared Function GetUltimoEnvio(ByVal id_usuario As Integer) As Envio
        Dim dbContext As New SiExProEntities
        Dim envio As Envio = dbContext.D_ENVIOS.OrderByDescending(Function(x) x.id_envio).FirstOrDefault(Function(x) x.id_usuario = id_usuario)

        Return envio
    End Function

    Public Shared Function GetDatosEnvio(ByVal idEnvio As Integer) As Envio
        Dim dbContext As New SiExProEntities
        Dim envio As Envio = dbContext.D_ENVIOS.FirstOrDefault(Function(x) x.id_envio = idEnvio)

        Return envio
    End Function

    Public Shared Function GetDatosEnvioByReferenciaFedex(ByVal trackId As String) As Envio
        Dim dbContext As New SiExProEntities
        Dim envio As Envio = dbContext.D_ENVIOS.FirstOrDefault(Function(x) x.Referencia_FedEx.Trim() = trackId.Trim())

        Return envio
    End Function

    Public Shared Function GetDatosCliente(id_envio As Integer) As Cliente
        Dim dbContext As New SiExProEntities
        Dim cliente As Cliente

        cliente = CType((From ed In dbContext.D_ENVIOS_DATOS.Where(Function(x) x.id_envio = id_envio)
                         Join c In dbContext.C_CLIENTES
                             On c.id_cliente Equals ed.id_cliente
                         Select c).FirstOrDefault(), Cliente)

        Return cliente

    End Function

    Public Shared Function GetDatosDestinatario(id_envio As Integer) As Destinatario
        Dim dbContext As New SiExProEntities
        Dim destinatario As Destinatario

        destinatario = CType((From ed In dbContext.D_ENVIOS_DATOS.Where(Function(x) x.id_envio = id_envio)
                              Join c In dbContext.C_DESTINATARIOS
                                  On c.id_destinatario Equals ed.id_destinatario
                              Select c).FirstOrDefault(), Destinatario)

        Return destinatario
    End Function

    Public Shared Function GetTarifaAgencia(id_tarifa_agencia As Integer) As TarifaAgencia
        Dim dbContext As New SiExProEntities
        Dim tarifaAgencias As TarifaAgencia = dbContext.D_TARIFAS_AGENCIA.FirstOrDefault(Function(x) x.id_tarifa_agencia = id_tarifa_agencia)

        Return tarifaAgencias
    End Function

    Public Shared Function EstafetaLabel(id_envio As Integer) As EstafetaLabel
        Dim dbContext As New SiExProEntities
        Return dbContext.D_ESTAFETA_LABEL.FirstOrDefault(Function(x) x.id_envio = id_envio)
    End Function

    Public Shared Function ProviderLabel(id_envio As Integer, providerId As Integer) As ProviderLabel
        Dim dbContext As New SiExProEntities
        Return dbContext.D_PROVIDER_LABEL.FirstOrDefault(Function(x) x.ShipmentId = id_envio And x.ProviderId = providerId)
    End Function

    Public Shared Function InsEstafetaLabel(id_envio As Integer, labelPDF As Byte(), referencia As String, resultDescription As String, identificador As Guid) As Boolean
        Dim dbContext As New SiExProEntities

        Dim envio As Envio = dbContext.D_ENVIOS.FirstOrDefault(Function(x) x.id_envio = id_envio)

        envio.Referencia_FedEx = referencia
        envio.id_status = 300
        dbContext.SaveChanges()

        Dim envioDato As EnvioDatos = dbContext.D_ENVIOS_DATOS.FirstOrDefault(Function(x) x.id_envio = id_envio)

        envioDato.id_proveedor = 1

        dbContext.SaveChanges()

        Dim estafetaLabel As EstafetaLabel = dbContext.D_ESTAFETA_LABEL.FirstOrDefault(Function(x) x.id_envio = id_envio)
        Dim existeLabel As Boolean = True

        If estafetaLabel Is Nothing Then
            estafetaLabel = New EstafetaLabel()
            existeLabel = False
        End If

        With estafetaLabel
            .id_envio = id_envio
            .fecha = DateTime.Now()
            .labelPDF = labelPDF
            .trackId = referencia
            .relacion = identificador
            .wayBill = resultDescription
        End With

        If Not existeLabel Then
            dbContext.D_ESTAFETA_LABEL.Add(estafetaLabel)
        End If

        Return dbContext.SaveChanges() > 0

    End Function

    Public Shared Function InsFrecuanciaCotizador(id_envio As Integer, respuestaFrecuenciaCotizador As Estafeta.Frecuenciacotizador.Respuesta(), servicioSeleccionado As Estafeta.Frecuenciacotizador.TipoServicio) As Boolean
        Dim dbContext As New SiExProEntities

        For Each respuesta As Estafeta.Frecuenciacotizador.Respuesta In respuestaFrecuenciaCotizador
            Dim frecuenciaCotizador As EstafetaFrecuenciaCotizador = dbContext.D_ESTAFETA_FRECUENCIA_COTIZADOR.FirstOrDefault(Function(x) x.id_envio = id_envio)
            Dim existeFrecuenciaCotizador As Boolean = True
            If frecuenciaCotizador Is Nothing Then
                frecuenciaCotizador = New EstafetaFrecuenciaCotizador()
                existeFrecuenciaCotizador = False
            End If

            With frecuenciaCotizador
                .id_envio = id_envio
                .EsPaquete = respuesta.TipoEnvio.EsPaquete
                .Largo = respuesta.TipoEnvio.Largo
                .Peso = respuesta.TipoEnvio.Peso
                .Alto = respuesta.TipoEnvio.Alto
                .Ancho = respuesta.TipoEnvio.Ancho
                .OcurreForzoso = respuesta.ModalidadEntrega.OcurreForzoso
                .Frecuencia = respuesta.ModalidadEntrega.Frecuencia
                .CostoReexpedicion = respuesta.CostoReexpedicion
                .ExistenteSiglaOri = respuesta.ExistenteSiglaOri
                .ExistenteSiglaDes = respuesta.ExistenteSiglaDes
                .Lunes = respuesta.DiasEntrega.Lunes
                .Martes = respuesta.DiasEntrega.Martes
                .Miercoles = respuesta.DiasEntrega.Miercoles
                .Jueves = respuesta.DiasEntrega.Jueves
                .Viernes = respuesta.DiasEntrega.Viernes
                .Sabado = respuesta.DiasEntrega.Sabado
                .Domingo = respuesta.DiasEntrega.Domingo
                .FechaCotizacion = DateTime.Now()
            End With

            If Not existeFrecuenciaCotizador Then
                dbContext.D_ESTAFETA_FRECUENCIA_COTIZADOR.Add(frecuenciaCotizador)
            End If

            dbContext.SaveChanges()

            Dim tiposServicio = dbContext.D_ESTAFETA_TIPO_SERVICIO.Where(Function(x) x.id_envio = id_envio)

            If tiposServicio IsNot Nothing Then
                dbContext.D_ESTAFETA_TIPO_SERVICIO.RemoveRange(tiposServicio)
            End If

            For Each tipoServicio In respuesta.TipoServicio
                Dim ts As New EstafetaTipoServicio()
                With ts
                    .id_envio = id_envio
                    .DescripcionServicio = tipoServicio.DescripcionServicio
                    .TipoEnvioRes = tipoServicio.TipoEnvioRes
                    .AplicaCotizacion = tipoServicio.AplicaCotizacion
                    .TarifaBase = tipoServicio.TarifaBase
                    .CCTarifaBase = tipoServicio.CCTarifaBase
                    .CargosExtra = tipoServicio.CargosExtra
                    .SobrePeso = tipoServicio.SobrePeso
                    .CCSobrePeso = tipoServicio.CCSobrePeso
                    .CostoTotal = tipoServicio.CostoTotal
                    .Peso = tipoServicio.Peso
                    .AplicaServicio = tipoServicio.AplicaServicio
                    .FechaCotizacion = DateTime.Now()
                End With

                If servicioSeleccionado.DescripcionServicio = ts.DescripcionServicio Then
                    ts.Selecccionado = 1
                End If
                dbContext.D_ESTAFETA_TIPO_SERVICIO.Add(ts)
            Next
            dbContext.SaveChanges()
        Next

        Return True
    End Function

    Public Shared Function LogEstafetaRequestResponse(metodo As String, request As String, response As String, cuenta As Integer, carrier As Integer, Optional imagenBase64 As String = "") As Boolean
        Dim dbContext As New SiExProEntities
        Dim requestResponse As New EstafetaRequestResponse()

        With requestResponse
            .metodo = metodo
            .request = request
            .response = response
            .fecha = DateTime.Now
            .cuenta = cuenta
            .imagenBase64 = imagenBase64
            .carrier = carrier
        End With

        dbContext.D_ESTAFETA_REQUEST_RESPONSE.Add(requestResponse)

        Return dbContext.SaveChanges()
    End Function

    Public Shared Function GetServicioSelecionado(id_envio As Integer) As EstafetaTipoServicio
        Dim dbContext As New SiExProEntities
        Return dbContext.D_ESTAFETA_TIPO_SERVICIO.FirstOrDefault(Function(x) x.id_envio = id_envio And x.Selecccionado = True)
    End Function

    Public Shared Function FindZipCode(zip_code As String) As Cobertura
        Dim dbContext As New SiExProEntities

        Return dbContext.D_COBERTURAS.FirstOrDefault(Function(x) x.codigo_postal = zip_code)

    End Function

    Public Shared Function FindGombarState(zone_code As String) As EstadosGombar
        Dim dbContext As New SiExProEntities

        Return dbContext.C_ESTADOS_GOMBAR.FirstOrDefault(Function(x) x.siglas = zone_code)

    End Function

    Public Shared Function UserZone(zoneId As Integer) As Zonas
        Dim dbContext As New SiExProEntities

        Return dbContext.C_ZONAS.FirstOrDefault(Function(x) x.id_zona = zoneId)

    End Function

    Public Shared Function GetSender(zoneId As Integer) As Cliente
        Dim dbContext As New SiExProEntities

        Return dbContext.C_CLIENTES.OrderByDescending(Function(x) x.id_cliente).FirstOrDefault()

    End Function

    Public Shared Function GetGombarSender(clienteId As Integer) As Cliente
        Dim dbContext As New SiExProEntities

        Return dbContext.C_CLIENTES.FirstOrDefault(Function(x) x.NIT = clienteId)

    End Function

    Public Shared Function GetSenderById(clienteId As Integer) As Cliente
        Dim dbContext As New SiExProEntities

        Return dbContext.C_CLIENTES.FirstOrDefault(Function(x) x.id_cliente = clienteId)

    End Function

    Public Shared Function GetSearchZipCode(cp As String) As List(Of Sepomex)
        Dim dbContext As New SiExProEntities
        Dim result = dbContext.C_SEPOMEX.Where(Function(x) x.d_codigo = cp)

        If result IsNot Nothing Then
            Return result.OrderBy(Function(x) x.d_asenta).ToList()
        Else
            Return Nothing
        End If

    End Function

    Public Shared Function GetMinMaxEnvioMult(idEnvio As Integer) As List(Of EstafetaLabel)
        Dim dbContext As New SiExProEntities
        Dim minmax = New List(Of EstafetaLabel)
        Dim result = dbContext.D_ESTAFETA_LABEL.FirstOrDefault(Function(x) x.id_envio = idEnvio)
        If result IsNot Nothing Then
            If result.relacion IsNot Nothing Then
                Dim resultList = GetRelacionEnvioMult(result.relacion)

                If resultList IsNot Nothing Then
                    minmax.Add(resultList.FirstOrDefault())
                    minmax.Add(resultList(resultList.Count - 1))
                End If
            End If
        End If

        Return minmax
    End Function

    Public Shared Function GetRelacionEnvioMult(guid As System.Guid) As List(Of EstafetaLabel)
        Dim dbContext As New SiExProEntities
        Dim result = dbContext.D_ESTAFETA_LABEL.Where(Function(x) x.relacion = guid).OrderBy(Function(x) x.id_envio)

        If result IsNot Nothing Then
            Return result.OrderBy(Function(x) x.id_envio).ToList()
        Else
            Return Nothing
        End If
    End Function

    Public Shared Function CreatePak2GoReference(rateId As Integer, reference As String) As String
        Dim dbContext As New SiExProEntities

        Dim pak2GoReference As New Pak2GoReference()

        pak2GoReference.RateId = rateId
        pak2GoReference.ReferenceNumber = reference
        pak2GoReference.CreatedOn = DateTime.Now.Date()

        dbContext.D_PAK2GO_REFERENCE.Add(pak2GoReference)

        dbContext.SaveChanges()

    End Function

    Public Shared Function UpdatePak2GoReference(rateId As Integer, shipmentId As Integer, trackNumber As String) As String
        Dim dbContext As New SiExProEntities

        Dim pak2GoReference = dbContext.D_PAK2GO_REFERENCE.FirstOrDefault(Function(x) x.RateId = rateId)

        If pak2GoReference IsNot Nothing Then
            pak2GoReference.ShipmentId = shipmentId
            pak2GoReference.TrackNumber = trackNumber
            pak2GoReference.ModifiedOn = DateTime.Now.Date()

            dbContext.SaveChanges()
        End If

    End Function

    Public Shared Function FedexShipment(ByVal fedexShipRequest As ShipRequestDto) As ResponseData
        Dim webClient As New WebClient()
        Dim resByte As Byte()
        Dim resString As String
        Dim response As New ResponseData()
        Try

            webClient.Headers("Content-type") = "application/json;charset=utf-8"
            webClient.Encoding = Encoding.UTF8

            Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
            Dim jsonRequest = serializer.Serialize(fedexShipRequest)

            Dim reqString = Encoding.UTF8.GetBytes(jsonRequest)
            resByte = webClient.UploadData(ConfigurationManager.AppSettings("EstafetaService.Ship"), "post", reqString)
            resString = Encoding.Default.GetString(resByte)
            response = serializer.Deserialize(Of ResponseData)(resString)
            webClient.Dispose()
        Catch ex As Exception
            response.Success = False
            response.ErrorMessage = ex.Message
        End Try
        Return response
    End Function

    Public Shared Function FedexRate(ByVal fedexShipRequest As ShipRequestDto) As RateResponseData
        Dim webClient As New WebClient()
        Dim resByte As Byte()
        Dim resString As String
        Dim response As New RateResponseData()
        Try

            webClient.Headers("Content-type") = "application/json;charset=utf-8"
            webClient.Encoding = Encoding.UTF8

            Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
            Dim jsonRequest = serializer.Serialize(fedexShipRequest)

            Dim reqString = Encoding.UTF8.GetBytes(jsonRequest)
            resByte = webClient.UploadData(ConfigurationManager.AppSettings("EstafetaService.Rate"), "post", reqString)
            resString = Encoding.Default.GetString(resByte)
            response = serializer.Deserialize(Of RateResponseData)(resString)
            webClient.Dispose()
        Catch ex As Exception
            response.Success = False
            response.ErrorMessage = ex.Message
        End Try
        Return response
    End Function

    Public Shared Function PaqueteExpressQuote(ByVal fedexShipRequest As ShipRequestDto) As PaqueteExpressQuoteServiceResponse
        Dim webClient As New WebClient()
        Dim resByte As Byte()
        Dim resString As String
        Dim response As New PaqueteExpressQuoteServiceResponse()
        Try

            webClient.Headers("Content-type") = "application/json;charset=utf-8"
            webClient.Encoding = Encoding.UTF8

            Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
            Dim jsonRequest = serializer.Serialize(fedexShipRequest)

            Dim reqString = Encoding.UTF8.GetBytes(jsonRequest)
            resByte = webClient.UploadData(ConfigurationManager.AppSettings("PaqueteExpress.Quote"), "post", reqString)
            resString = Encoding.Default.GetString(resByte)
            response = serializer.Deserialize(Of PaqueteExpressQuoteServiceResponse)(resString)
            webClient.Dispose()
        Catch ex As Exception
            response.Success = False
            response.ErrorMessage = ex.Message
        End Try
        Return response
    End Function

    Public Shared Function Pak2GoQuote(ByVal request As ShipRequestDto) As Pak2GoShipServiceResponse
        Dim webClient As New WebClient()
        Dim resByte As Byte()
        Dim resString As String
        Dim response As New Pak2GoShipServiceResponse()
        Try

            webClient.Headers("Content-type") = "application/json;charset=utf-8"
            webClient.Encoding = Encoding.UTF8

            Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
            Dim jsonRequest = serializer.Serialize(request)

            Dim reqString = Encoding.UTF8.GetBytes(jsonRequest)
            resByte = webClient.UploadData(ConfigurationManager.AppSettings("Pak2Go.Quote"), "post", reqString)
            resString = Encoding.Default.GetString(resByte)
            response = serializer.Deserialize(Of Pak2GoShipServiceResponse)(resString)
            webClient.Dispose()
        Catch ex As Exception
            response.Success = False
            response.ErrorMessage = ex.Message
        End Try
        Return response
    End Function

    Public Shared Function Pak2GoLabel(ByVal request As ShipRequestDto) As Pak2GoLabelServiceResponse
        Dim webClient As New WebClient()
        Dim resByte As Byte()
        Dim resString As String
        Dim response As New Pak2GoLabelServiceResponse()
        Try

            webClient.Headers("Content-type") = "application/json;charset=utf-8"
            webClient.Encoding = Encoding.UTF8

            Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
            Dim jsonRequest = serializer.Serialize(request)

            Dim reqString = Encoding.UTF8.GetBytes(jsonRequest)
            resByte = webClient.UploadData(ConfigurationManager.AppSettings("Pak2Go.Ship"), "post", reqString)
            resString = Encoding.Default.GetString(resByte)
            response = serializer.Deserialize(Of Pak2GoLabelServiceResponse)(resString)
            webClient.Dispose()
        Catch ex As Exception
            response.Success = False
            response.ErrorMessage = ex.Message
        End Try
        Return response
    End Function

    Public Shared Function PaqueteExpressBranch(ByVal fedexShipRequest As ShipRequestDto) As PaqueteExpressQuoteServiceResponse
        Dim webClient As New WebClient()
        Dim resByte As Byte()
        Dim resString As String
        Dim response As New PaqueteExpressQuoteServiceResponse()
        Try

            webClient.Headers("Content-type") = "application/json;charset=utf-8"
            webClient.Encoding = Encoding.UTF8

            Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
            Dim jsonRequest = serializer.Serialize(fedexShipRequest)

            Dim reqString = Encoding.UTF8.GetBytes(jsonRequest)
            resByte = webClient.UploadData(ConfigurationManager.AppSettings("PaqueteExpress.Branch"), "post", reqString)
            resString = Encoding.Default.GetString(resByte)
            response = serializer.Deserialize(Of PaqueteExpressQuoteServiceResponse)(resString)
            webClient.Dispose()
        Catch ex As Exception
            response.Success = False
            response.ErrorMessage = ex.Message
        End Try
        Return response
    End Function

    Public Shared Function PaqueteExpressShip(ByVal fedexShipRequest As ShipRequestDto) As PaqueteExpressShipResponse
        Dim webClient As New WebClient()
        Dim resByte As Byte()
        Dim resString As String
        Dim response As New PaqueteExpressShipResponse()
        Try

            webClient.Headers("Content-type") = "application/json;charset=utf-8"
            webClient.Encoding = Encoding.UTF8

            Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
            Dim jsonRequest = serializer.Serialize(fedexShipRequest)

            Dim reqString = Encoding.UTF8.GetBytes(jsonRequest)
            resByte = webClient.UploadData(ConfigurationManager.AppSettings("PaqueteExpress.Ship"), "post", reqString)
            resString = Encoding.Default.GetString(resByte)
            response = serializer.Deserialize(Of PaqueteExpressShipResponse)(resString)
            webClient.Dispose()
        Catch ex As Exception
            response.Success = False
            response.ErrorMessage = ex.Message
        End Try
        Return response
    End Function

    Public Shared Function AddPaqueteExpressTipoPaquetes(dtgridview As DataTable, idEnvio As Integer, idProveedor As Integer, servicio As Integer) As Boolean
        Dim dbContext As New SiExProEntities

        For Each row As DataRow In dtgridview.Rows
            Dim tipoPaquete As New PaqueteExpressTipoPaquete()

            With tipoPaquete
                .id_envio = idEnvio
                .cantidad = row("Cantidad")
                .contenido = row("Contenido")
                .tipoId = row("TipoClave")
                .tipo = row("Tipo")
                .alto = row("Alto")
                .largo = row("Largo")
                .ancho = row("Ancho")
                .peso = row("Peso")
                .fecha = DateTime.Now
                .seguro = row("Seguro")
                .servicioSat = row("ServicioSAT")
                .areaExtendida = row("AreaExtendida")
                .idProveedor = idProveedor
                .servicio = servicio
            End With

            If idProveedor = 3 Then
                If servicio = 1 Then
                    tipoPaquete.Precio = row("PrecioPEEconomic")
                    tipoPaquete.total = row("PrecioPEEconomic") * row("Cantidad")
                Else
                    tipoPaquete.Precio = row("PrecioPENextDay")
                    tipoPaquete.total = row("PrecioPENextDay") * row("Cantidad")
                End If
            End If

            If idProveedor = 5 Then
                Select Case servicio
                    Case 1
                        tipoPaquete.Precio = row("PrecioDLGombar")
                        tipoPaquete.total = row("PrecioDLGombar") * row("Cantidad")
                    Case 2
                        tipoPaquete.Precio = row("PrecioDLGombarExpress")
                        tipoPaquete.total = row("PrecioDLGombarExpress") * row("Cantidad")
                    Case 3
                        tipoPaquete.Precio = row("PrecioDLGombarTarima")
                        tipoPaquete.total = row("PrecioDLGombarTarima") * row("Cantidad")
                    Case 4
                        tipoPaquete.Precio = row("PrecioDLGombarNacional")
                        tipoPaquete.total = row("PrecioDLGombarNacional") * row("Cantidad")
                    Case 5
                        tipoPaquete.Precio = row("PrecioDLRutaLeonPueCdmx")
                        tipoPaquete.total = row("PrecioDLRutaLeonPueCdmx") * row("Cantidad")
                    Case 6
                        tipoPaquete.Precio = row("PrecioDLRutaNorte")
                        tipoPaquete.total = row("PrecioDLRutaNorte") * row("Cantidad")
                    Case 7
                        tipoPaquete.Precio = row("PrecioDLTarimasRutaLeonPueCdmx")
                        tipoPaquete.total = row("PrecioDLTarimasRutaLeonPueCdmx") * row("Cantidad")
                    Case 8
                        tipoPaquete.Precio = row("PrecioDLRutaPacifico")
                        tipoPaquete.total = row("PrecioDLRutaPacifico") * row("Cantidad")
                    Case 9
                        tipoPaquete.Precio = row("PrecioDLTarimasRutaPacifico")
                        tipoPaquete.total = row("PrecioDLTarimasRutaPacifico") * row("Cantidad")
                    Case 10
                        tipoPaquete.Precio = row("PrecioDLTarimasOcurreRutaPacifico")
                        tipoPaquete.total = row("PrecioDLTarimasOcurreRutaPacifico") * row("Cantidad")
                End Select
            End If


            dbContext.D_ENVIOS_PE_TIPO_PAQUETES.Add(tipoPaquete)
        Next row

        Return dbContext.SaveChanges() > 0

    End Function

    Public Shared Function BuscarCodigoSat(codigoSat As String) As CodigosServiciosSat
        Dim dbContext As New SiExProEntities

        Return dbContext.D_CODIGOS_SERVICIOS_SAT.FirstOrDefault(Function(x) x.codigo_servicio_id = codigoSat)
    End Function

    Public Shared Function ActualizaCodigosBarras() As Boolean
        Dim dbContext As New SiExProEntities

        Dim envios = dbContext.D_ENVIOS_DATOS.Where(Function(x) x.codigo_barras Is Nothing).OrderByDescending(Function(x) x.id_envio).Take(100)
        For Each envio As EnvioDatos In envios
            Dim barcodeBm As Bitmap = Nothing
            barcodeBm = DaspackDALC.codigo128("A" + envio.id_envio.ToString + "B", False, 20)
            Dim bitmapBytes As Byte()

            Using stream As New System.IO.MemoryStream
                barcodeBm.Save(stream, ImageFormat.Png)
                bitmapBytes = stream.GetBuffer()
            End Using

            envio.codigo_barras = bitmapBytes
        Next

        dbContext.SaveChanges()

        Return True
    End Function

    Public Shared Function codigo128(ByVal _code As String, Optional ByVal vertexto As Boolean = False, Optional ByVal Height As Single = 0)
        Dim barcode As New BarcodeCodabar
        barcode.StartStopText = True
        If Height <> 0 Then
            barcode.BarHeight = Height
        End If
        barcode.Code = _code
        Try
            Dim bm As New System.Drawing.Bitmap(barcode.CreateDrawingImage(Color.Black, Color.White))
            If vertexto = False Then
                Return bm
            Else
                'generando el texto
                Dim bmT As Image
                bmT = New Bitmap(bm.Width, bm.Height + 14)
                Dim g As Graphics = Graphics.FromImage(bmT)
                g.FillRectangle(New SolidBrush(Color.White), 0, 0, bm.Width, bm.Height + 14)

                Dim pintarTexto As New Font("Arial", 8)
                Dim brocha As New SolidBrush(Color.Black)

                Dim stringSize As New SizeF
                stringSize = g.MeasureString(_code, pintarTexto)
                Dim centrox As Single = (bm.Width - stringSize.Width) / 2
                Dim x As Single = centrox
                Dim y As Single = bm.Height

                Dim drawformat As New StringFormat
                drawformat.FormatFlags = StringFormatFlags.NoWrap
                g.DrawImage(bm, 0, 0)

                Dim ncode As String = _code.Substring(1, _code.Length - 2)
                g.DrawString(ncode, pintarTexto, brocha, x, y, drawformat)
                Return bmT

            End If
        Catch ex As Exception
            Throw New Exception("Error al generar el codigo" & ex.ToString)
        End Try
    End Function

    Public Shared Function GetDeliveryConfirmationImage(shipmentId As Integer) As DeliveryConfirmation
        Dim dbContext As New SiExProEntities

        Return dbContext.D_DELIVERY_CONFIRMATION.OrderByDescending(Function(x) x.DeliveryConfirmationId).FirstOrDefault(Function(x) x.ShipmentId = shipmentId)
    End Function

    Public Shared Function ActualizaTarifas(nuevasTarifas As List(Of TarifaAgenciaFedex), idAgente As Integer) As String
        Dim response As String = ""
        Dim dbContext As New SiExProEntities
        Try
            Dim tarifas = dbContext.D_TARIFAS_AGENCIA_FEDEX.Where(Function(x) x.ID_AGENCIA = idAgente)
            dbContext.D_TARIFAS_AGENCIA_FEDEX.RemoveRange(tarifas)
            dbContext.SaveChanges()

            dbContext.D_TARIFAS_AGENCIA_FEDEX.AddRange(nuevasTarifas)
            dbContext.SaveChanges()

            response = "Tarifas actualizadas"
        Catch ex As Exception
            response = "No se pudieron actualizar tarifas"
        End Try
        Return response
    End Function

    Public Shared Function ActualizaTarifas(nuevasTarifas As List(Of TarifaAgenciaDraft), idAgente As Integer) As String
        Dim response As String = ""
        Dim dbContext As New SiExProEntities
        Try
            Dim tarifas = dbContext.D_TARIFAS_AGENCIA_DRAFT.Where(Function(x) x.ID_AGENCIA = idAgente)
            dbContext.D_TARIFAS_AGENCIA_DRAFT.RemoveRange(tarifas)
            dbContext.SaveChanges()

            dbContext.D_TARIFAS_AGENCIA_DRAFT.AddRange(nuevasTarifas)
            dbContext.SaveChanges()

            response = "Tarifas actualizadas"
        Catch ex As Exception
            response = "No se pudieron actualizar tarifas"
        End Try
        Return response
    End Function

    Public Shared Function ActualizaTarifas(nuevasTarifas As List(Of TarifaAgenciaPaqueteExpress), idAgente As Integer) As String
        Dim response As String = ""
        Dim dbContext As New SiExProEntities
        Try
            Dim tarifas = dbContext.D_TARIFAS_AGENCIA_PAQUETE_EXPRESS.Where(Function(x) x.ID_AGENCIA = idAgente)
            dbContext.D_TARIFAS_AGENCIA_PAQUETE_EXPRESS.RemoveRange(tarifas)
            dbContext.SaveChanges()

            dbContext.D_TARIFAS_AGENCIA_PAQUETE_EXPRESS.AddRange(nuevasTarifas)
            dbContext.SaveChanges()

            response = "Tarifas actualizadas"
        Catch ex As Exception
            response = "No se pudieron actualizar tarifas"
        End Try
        Return response
    End Function

    Public Shared Function ActualizaTarifas(nuevasTarifas As List(Of TarifaAgenciaEstafeta), idAgente As Integer) As String
        Dim response As String = ""
        Dim dbContext As New SiExProEntities
        Try
            Dim tarifas = dbContext.D_TARIFAS_AGENCIA_ESTAFETA.Where(Function(x) x.ID_AGENCIA = idAgente)
            dbContext.D_TARIFAS_AGENCIA_ESTAFETA.RemoveRange(tarifas)
            dbContext.SaveChanges()

            dbContext.D_TARIFAS_AGENCIA_ESTAFETA.AddRange(nuevasTarifas)
            dbContext.SaveChanges()

            response = "Tarifas actualizadas"
        Catch ex As Exception
            response = "No se pudieron actualizar tarifas"
        End Try
        Return response
    End Function

    Public Shared Function ActualizaTarifas(nuevasTarifas As List(Of TarifaAgenciaDliverExpress), idAgente As Integer) As String
        Dim response As String = ""
        Dim dbContext As New SiExProEntities
        Try
            Dim tarifas = dbContext.D_TARIFAS_AGENCIA_DLIVER_EXPRESS.Where(Function(x) x.ID_AGENCIA = idAgente)
            dbContext.D_TARIFAS_AGENCIA_DLIVER_EXPRESS.RemoveRange(tarifas)
            dbContext.SaveChanges()

            dbContext.D_TARIFAS_AGENCIA_DLIVER_EXPRESS.AddRange(nuevasTarifas)
            dbContext.SaveChanges()

            response = "Tarifas actualizadas"
        Catch ex As Exception
            response = "No se pudieron actualizar tarifas"
        End Try
        Return response
    End Function

    Public Shared Function ActualizaTarifas(nuevasTarifas As List(Of TarifasAgenciaNorte), idAgente As Integer) As String
        Dim response As String = ""
        Dim dbContext As New SiExProEntities
        Try
            Dim tarifas = dbContext.D_TARIFAS_AGENCIA_NORTE.Where(Function(x) x.ID_AGENCIA = idAgente)
            dbContext.D_TARIFAS_AGENCIA_NORTE.RemoveRange(tarifas)
            dbContext.SaveChanges()

            dbContext.D_TARIFAS_AGENCIA_NORTE.AddRange(nuevasTarifas)
            dbContext.SaveChanges()

            response = "Tarifas actualizadas"
        Catch ex As Exception
            response = "No se pudieron actualizar tarifas"
        End Try
        Return response
    End Function

    Public Shared Function ActualizaTarifas(nuevasTarifas As List(Of TarifasAgenciaTufesa), idAgente As Integer) As String
        Dim response As String = ""
        Dim dbContext As New SiExProEntities
        Try
            Dim tarifas = dbContext.D_TARIFAS_AGENCIA_TUFESA.Where(Function(x) x.ID_AGENCIA = idAgente)
            dbContext.D_TARIFAS_AGENCIA_TUFESA.RemoveRange(tarifas)
            dbContext.SaveChanges()

            dbContext.D_TARIFAS_AGENCIA_TUFESA.AddRange(nuevasTarifas)
            dbContext.SaveChanges()

            response = "Tarifas actualizadas"
        Catch ex As Exception
            response = "No se pudieron actualizar tarifas"
        End Try
        Return response
    End Function

    Public Shared Function ActualizaTarifas(nuevasTarifas As List(Of TarifasAgenciaRedpack), idAgente As Integer) As String
        Dim response As String = ""
        Dim dbContext As New SiExProEntities
        Try
            Dim tarifas = dbContext.D_TARIFAS_AGENCIA_REDPACK.Where(Function(x) x.ID_AGENCIA = idAgente)
            dbContext.D_TARIFAS_AGENCIA_REDPACK.RemoveRange(tarifas)
            dbContext.SaveChanges()

            dbContext.D_TARIFAS_AGENCIA_REDPACK.AddRange(nuevasTarifas)
            dbContext.SaveChanges()

            response = "Tarifas actualizadas"
        Catch ex As Exception
            response = "No se pudieron actualizar tarifas"
        End Try
        Return response
    End Function

    Public Shared Function AgentesPorUsuario(ByVal idUsuario As String) As List(Of Agente)

        Try
            Dim MyConnection As ConnectionStringSettings
            MyConnection = ConfigurationManager.ConnectionStrings("paqueteriaDB_ConnectionString")
            Dim connection As Data.Common.DbConnection = New Data.SqlClient.SqlConnection()
            connection.ConnectionString = MyConnection.ConnectionString

            Dim cmd As Data.IDbCommand = connection.CreateCommand()
            cmd.CommandType = Data.CommandType.StoredProcedure
            cmd.CommandText = "dbo.sp_Select_Agentes_por_usuarios"

            Dim parm1 As Data.Common.DbParameter = cmd.CreateParameter()
            parm1.ParameterName = "@id_usuario"
            parm1.Value = idUsuario
            cmd.Parameters.Add(parm1)

            connection.Open()

            Dim reader As Data.SqlClient.SqlDataReader = cmd.ExecuteReader()

            Dim agentes As New List(Of Agente)
            If reader.HasRows Then
                While reader.Read()
                    Dim agente As New Agente()

                    With agente
                        .IdUsuario = reader.GetValue(0)
                        .IdAgente = reader.GetValue(1)
                        .Nombre = reader.GetValue(2)
                        .Orden = reader.GetValue(3)
                    End With

                    agentes.Add(agente)
                End While
            End If
            connection.Close()

            Return agentes.OrderBy(Function(x) x.Orden).ToList()
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Shared Function VarificaEnvioPreAsignado(idEnvio As Integer, idAgencia As Integer) As String
        Dim response As String = ""
        Dim dbContext As New SiExProEntities
        Try
            response = ""
            Dim envio = dbContext.D_ENVIOS_PREASIGNADOS.FirstOrDefault(Function(x) x.id_envio = idEnvio)
            If envio Is Nothing Then
                response = "Envio no esta asignado a agencia"
            Else
                If envio.id_agencia <> idAgencia Then
                    response = "Envio no esta asignado a agencia"
                End If
            End If

        Catch ex As Exception
            response = "No se pudo verificar envio"
        End Try
        Return response
    End Function

    Public Shared Function ModificacionFactura(id_envio As Integer, comentarios As String, noFactura As String, idUsuario As Integer) As Boolean
        Dim dbContext As New SiExProEntities

        InsComenatrio(id_envio, idUsuario, "Importacion Factura Envio Modificado: " + IIf(String.IsNullOrWhiteSpace(comentarios), "", comentarios))

        Dim auditLogEnvio = New AuditLog
        Dim envioDato As EnvioDatos = dbContext.D_ENVIOS_DATOS.FirstOrDefault(Function(x) x.id_envio = id_envio)

        Dim auditLogEnvioDatosNoFactura = New AuditLog
        auditLogEnvioDatosNoFactura.Columna = "no_factura"
        auditLogEnvioDatosNoFactura.Fecha = DateTime.Now
        auditLogEnvioDatosNoFactura.Tabla = "D_ENVIOS_DATOS"
        auditLogEnvioDatosNoFactura.Usuario = idUsuario
        auditLogEnvioDatosNoFactura.ValorAnterior = IIf(envioDato.no_factura Is Nothing, "", envioDato.no_factura)
        auditLogEnvioDatosNoFactura.ValorActual = noFactura
        auditLogEnvioDatosNoFactura.IdEnvio = id_envio

        envioDato.no_factura = noFactura
        dbContext.D_AUDIT_LOG.Add(auditLogEnvioDatosNoFactura)

        dbContext.SaveChanges()

        Return True

    End Function

    Public Shared Function ModificacionEnvioProveedor(id_envio As Integer, comentarios As String, idProveedor As Integer, idUsuario As Integer, noFactura As String, importeFacturaProveedor As Decimal, gratificacion As String) As Boolean
        Dim dbContext As New SiExProEntities
        Dim auditLogEnvio = New AuditLog

        Dim envioDato As EnvioDatos = dbContext.D_ENVIOS_DATOS.FirstOrDefault(Function(x) x.id_envio = id_envio)

        Dim currentIdProveedor As Integer = IIf(envioDato.id_proveedor Is Nothing, 0, envioDato.id_proveedor)

        If currentIdProveedor <> idProveedor Then
            InsComenatrio(id_envio, idUsuario, "Envio Modificado Proveedor: " + comentarios + " Valor Anterior:" + IIf(envioDato.id_proveedor Is Nothing, "", envioDato.id_proveedor.ToString) + " Valor Actual: " + idProveedor.ToString())

            Dim auditLogEnvioDatos = New AuditLog
            auditLogEnvioDatos.Columna = "id_proveedor"
            auditLogEnvioDatos.Fecha = DateTime.Now
            auditLogEnvioDatos.Tabla = "D_ENVIOS_DATOS"
            auditLogEnvioDatos.Usuario = idUsuario
            auditLogEnvioDatos.ValorAnterior = envioDato.id_proveedor.ToString
            auditLogEnvioDatos.ValorActual = idProveedor.ToString
            auditLogEnvioDatos.IdEnvio = id_envio

            envioDato.id_proveedor = idProveedor
            dbContext.D_AUDIT_LOG.Add(auditLogEnvioDatos)
        End If

        If envioDato.no_factura <> noFactura Then
            InsComenatrio(id_envio, idUsuario, "Envio Modificado No Factura: " + comentarios + " Valor Anterior:" + IIf(envioDato.no_factura Is Nothing, "", envioDato.no_factura) + " Valor Actual: " + noFactura)

            Dim auditLogEnvioDatosNoFactura = New AuditLog
            auditLogEnvioDatosNoFactura.Columna = "no_factura"
            auditLogEnvioDatosNoFactura.Fecha = DateTime.Now
            auditLogEnvioDatosNoFactura.Tabla = "D_ENVIOS_DATOS"
            auditLogEnvioDatosNoFactura.Usuario = idUsuario
            auditLogEnvioDatosNoFactura.ValorAnterior = IIf(envioDato.no_factura Is Nothing, "", envioDato.no_factura)
            auditLogEnvioDatosNoFactura.ValorActual = noFactura
            auditLogEnvioDatosNoFactura.IdEnvio = id_envio

            envioDato.no_factura = noFactura
            dbContext.D_AUDIT_LOG.Add(auditLogEnvioDatosNoFactura)
        End If

        Dim currentImporteFactura As Decimal = IIf(envioDato.importe_factura_proveedor Is Nothing, 0, envioDato.importe_factura_proveedor)

        If currentImporteFactura <> importeFacturaProveedor Then
            InsComenatrio(id_envio, idUsuario, "Envio Modificado Importe Factura Proveedor: " + comentarios + " Valor Anterior:" + IIf(envioDato.importe_factura_proveedor Is Nothing, "", envioDato.importe_factura_proveedor.ToString) + " Valor Actual: " + importeFacturaProveedor.ToString)

            Dim auditLogEnvioDatosImporteFacturaProveedor = New AuditLog
            auditLogEnvioDatosImporteFacturaProveedor.Columna = "importe_factura_proveedor"
            auditLogEnvioDatosImporteFacturaProveedor.Fecha = DateTime.Now
            auditLogEnvioDatosImporteFacturaProveedor.Tabla = "D_ENVIOS_DATOS"
            auditLogEnvioDatosImporteFacturaProveedor.Usuario = idUsuario
            auditLogEnvioDatosImporteFacturaProveedor.ValorAnterior = IIf(envioDato.importe_factura_proveedor Is Nothing, "", envioDato.importe_factura_proveedor.ToString)
            auditLogEnvioDatosImporteFacturaProveedor.ValorActual = importeFacturaProveedor
            auditLogEnvioDatosImporteFacturaProveedor.IdEnvio = id_envio

            envioDato.importe_factura_proveedor = importeFacturaProveedor
            dbContext.D_AUDIT_LOG.Add(auditLogEnvioDatosImporteFacturaProveedor)
        End If


        If envioDato.gratificacion <> gratificacion Then
            InsComenatrio(id_envio, idUsuario, "Envio Modificado Gratificacion: " + comentarios + " Valor Anterior:" + IIf(envioDato.gratificacion Is Nothing, "", envioDato.gratificacion) + " Valor Actual: " + gratificacion.ToString)

            Dim auditLogEnvioDatosGratificacion = New AuditLog
            auditLogEnvioDatosGratificacion.Columna = "gratificacion"
            auditLogEnvioDatosGratificacion.Fecha = DateTime.Now
            auditLogEnvioDatosGratificacion.Tabla = "D_ENVIOS_DATOS"
            auditLogEnvioDatosGratificacion.Usuario = idUsuario
            auditLogEnvioDatosGratificacion.ValorAnterior = IIf(envioDato.gratificacion Is Nothing, "", envioDato.gratificacion)
            auditLogEnvioDatosGratificacion.ValorActual = gratificacion
            auditLogEnvioDatosGratificacion.IdEnvio = id_envio

            envioDato.gratificacion = gratificacion
            dbContext.D_AUDIT_LOG.Add(auditLogEnvioDatosGratificacion)
        End If

        dbContext.SaveChanges()

        Return True

    End Function

    Public Shared Function ModificacionTotalEnvio(id_envio As Integer, totalEnvio As Double, comentarios As String, idUsuario As Integer) As Boolean
        Dim dbContext As New SiExProEntities

        Dim auditLogEnvio = New AuditLog

        Dim envio As Envio = dbContext.D_ENVIOS.FirstOrDefault(Function(x) x.id_envio = id_envio)

        InsComenatrio(id_envio, idUsuario, "Total Envio Modificado: " + comentarios + " Valor Anterior:" + envio.total_envio.ToString + " Valor Actual: " + totalEnvio.ToString)

        auditLogEnvio.Columna = "total_envio"
        auditLogEnvio.Fecha = DateTime.Now
        auditLogEnvio.Tabla = "D_ENVIOS"
        auditLogEnvio.Usuario = idUsuario
        auditLogEnvio.ValorAnterior = envio.total_envio.ToString
        auditLogEnvio.ValorActual = totalEnvio.ToString
        auditLogEnvio.IdEnvio = id_envio

        envio.observaciones = envio.observaciones + " -- " + comentarios
        envio.total_envio = totalEnvio

        dbContext.SaveChanges()

        dbContext.D_AUDIT_LOG.Add(auditLogEnvio)
        dbContext.SaveChanges()

        Return True

    End Function

    Public Shared Function ModificacionReferenciaFedex(id_envio As Integer, referenciaFedex As String, comentarios As String, idUsuario As Integer) As Boolean

        Dim dbContext As New SiExProEntities
        Dim auditLogEnvio = New AuditLog
        Dim envio As Envio = dbContext.D_ENVIOS.FirstOrDefault(Function(x) x.id_envio = id_envio)

        InsComenatrio(id_envio, idUsuario, "Referencia Fedex Modificado: " + comentarios + " Valor Anterior:" + envio.Referencia_FedEx.ToString + " Valor Actual: " + referenciaFedex)

        Dim control As New seguimiento_envios
        'validar si el código proporcionado es refeencia o guía interna
        control.actualiza_FedEx(id_envio, referenciaFedex)

        auditLogEnvio.Columna = "ReferenciaFedex"
        auditLogEnvio.Fecha = DateTime.Now
        auditLogEnvio.Tabla = "D_ENVIOS"
        auditLogEnvio.Usuario = idUsuario
        auditLogEnvio.ValorAnterior = envio.Referencia_FedEx
        auditLogEnvio.ValorActual = referenciaFedex
        auditLogEnvio.IdEnvio = id_envio

        envio.observaciones = envio.observaciones + " -- " + comentarios
        dbContext.SaveChanges()

        dbContext.D_AUDIT_LOG.Add(auditLogEnvio)
        dbContext.SaveChanges()

        Return True

    End Function

    Public Shared Function ModificacionCasetas(id_envio As Integer, comentarios As String, casetas As Double, idUsuario As Integer) As Boolean
        Dim dbContext As New SiExProEntities

        Dim auditLogEnvio = New AuditLog
        Dim envioDato As EnvioDatos = dbContext.D_ENVIOS_DATOS.FirstOrDefault(Function(x) x.id_envio = id_envio)

        InsComenatrio(id_envio, idUsuario, "Casetas Modificado: " + IIf(String.IsNullOrWhiteSpace(comentarios), "", comentarios) + " Valor Anterior:" + IIf(envioDato.casetas Is Nothing, "", envioDato.casetas.ToString()) + " Valor Actual: " + casetas.ToString())

        Dim auditLogEnvioDatosNoFactura = New AuditLog
        auditLogEnvioDatosNoFactura.Columna = "casetas"
        auditLogEnvioDatosNoFactura.Fecha = DateTime.Now
        auditLogEnvioDatosNoFactura.Tabla = "D_ENVIOS_DATOS"
        auditLogEnvioDatosNoFactura.Usuario = idUsuario
        auditLogEnvioDatosNoFactura.ValorAnterior = IIf(envioDato.casetas Is Nothing, "", envioDato.casetas.ToString())
        auditLogEnvioDatosNoFactura.ValorActual = casetas.ToString()
        auditLogEnvioDatosNoFactura.IdEnvio = id_envio

        envioDato.casetas = casetas
        dbContext.D_AUDIT_LOG.Add(auditLogEnvioDatosNoFactura)

        dbContext.SaveChanges()
        Return True

    End Function

    Public Shared Function ModificacionGastos(id_envio As Integer, comentarios As String, gastos As Double, idUsuario As Integer) As Boolean
        Dim dbContext As New SiExProEntities

        Dim auditLogEnvio = New AuditLog
        Dim envioDato As EnvioDatos = dbContext.D_ENVIOS_DATOS.FirstOrDefault(Function(x) x.id_envio = id_envio)

        InsComenatrio(id_envio, idUsuario, "Gastos Modificado: " + IIf(String.IsNullOrWhiteSpace(comentarios), "", comentarios) + " Valor Anterior:" + IIf(envioDato.gastos Is Nothing, "", envioDato.gastos.ToString()) + " Valor Actual: " + gastos.ToString())

        Dim auditLogEnvioDatosNoFactura = New AuditLog
        auditLogEnvioDatosNoFactura.Columna = "gastos"
        auditLogEnvioDatosNoFactura.Fecha = DateTime.Now
        auditLogEnvioDatosNoFactura.Tabla = "D_ENVIOS_DATOS"
        auditLogEnvioDatosNoFactura.Usuario = idUsuario
        auditLogEnvioDatosNoFactura.ValorAnterior = IIf(envioDato.gastos Is Nothing, "", envioDato.gastos.ToString())
        auditLogEnvioDatosNoFactura.ValorActual = gastos.ToString()
        auditLogEnvioDatosNoFactura.IdEnvio = id_envio

        envioDato.gastos = gastos
        dbContext.D_AUDIT_LOG.Add(auditLogEnvioDatosNoFactura)

        dbContext.SaveChanges()
        Return True
    End Function

    Public Shared Function ModificacionViaticos(id_envio As Integer, comentarios As String, viaticos As Double, idUsuario As Integer) As Boolean
        Dim dbContext As New SiExProEntities

        Dim auditLogEnvio = New AuditLog
        Dim envioDato As EnvioDatos = dbContext.D_ENVIOS_DATOS.FirstOrDefault(Function(x) x.id_envio = id_envio)

        InsComenatrio(id_envio, idUsuario, "Viaticos Modificado: " + IIf(String.IsNullOrWhiteSpace(comentarios), "", comentarios) + " Valor Anterior:" + IIf(envioDato.viaticos Is Nothing, "", envioDato.viaticos.ToString()) + " Valor Actual: " + viaticos.ToString())

        Dim auditLogEnvioDatosNoFactura = New AuditLog
        auditLogEnvioDatosNoFactura.Columna = "viaticos"
        auditLogEnvioDatosNoFactura.Fecha = DateTime.Now
        auditLogEnvioDatosNoFactura.Tabla = "D_ENVIOS_DATOS"
        auditLogEnvioDatosNoFactura.Usuario = idUsuario
        auditLogEnvioDatosNoFactura.ValorAnterior = IIf(envioDato.viaticos Is Nothing, "", envioDato.viaticos.ToString())
        auditLogEnvioDatosNoFactura.ValorActual = viaticos.ToString()
        auditLogEnvioDatosNoFactura.IdEnvio = id_envio

        envioDato.viaticos = viaticos
        dbContext.D_AUDIT_LOG.Add(auditLogEnvioDatosNoFactura)

        dbContext.SaveChanges()
        Return True
    End Function

    Public Shared Function ModificacionPension(id_envio As Integer, comentarios As String, pension As Double, idUsuario As Integer) As Boolean
        Dim dbContext As New SiExProEntities

        Dim auditLogEnvio = New AuditLog
        Dim envioDato As EnvioDatos = dbContext.D_ENVIOS_DATOS.FirstOrDefault(Function(x) x.id_envio = id_envio)

        InsComenatrio(id_envio, idUsuario, "Pension Modificado: " + IIf(String.IsNullOrWhiteSpace(comentarios), "", comentarios) + " Valor Anterior:" + IIf(envioDato.pension Is Nothing, "", envioDato.pension.ToString()) + " Valor Actual: " + pension.ToString())

        Dim auditLogEnvioDatosNoFactura = New AuditLog
        auditLogEnvioDatosNoFactura.Columna = "pension"
        auditLogEnvioDatosNoFactura.Fecha = DateTime.Now
        auditLogEnvioDatosNoFactura.Tabla = "D_ENVIOS_DATOS"
        auditLogEnvioDatosNoFactura.Usuario = idUsuario
        auditLogEnvioDatosNoFactura.ValorAnterior = IIf(envioDato.pension Is Nothing, "", envioDato.pension.ToString())
        auditLogEnvioDatosNoFactura.ValorActual = pension.ToString()
        auditLogEnvioDatosNoFactura.IdEnvio = id_envio

        envioDato.pension = pension
        dbContext.D_AUDIT_LOG.Add(auditLogEnvioDatosNoFactura)

        dbContext.SaveChanges()
        Return True
    End Function

    Public Shared Function ModificacionManiobrasCliente(id_envio As Integer, comentarios As String, maniobras_cliente As Double, idUsuario As Integer) As Boolean
        Dim dbContext As New SiExProEntities

        Dim auditLogEnvio = New AuditLog
        Dim envioDato As EnvioDatos = dbContext.D_ENVIOS_DATOS.FirstOrDefault(Function(x) x.id_envio = id_envio)

        InsComenatrio(id_envio, idUsuario, "Maniobras Cliente Modificado: " + IIf(String.IsNullOrWhiteSpace(comentarios), "", comentarios) + " Valor Anterior:" + IIf(envioDato.maniobras_cliente Is Nothing, "", envioDato.maniobras_cliente.ToString()) + " Valor Actual: " + maniobras_cliente.ToString())

        Dim auditLogEnvioDatosNoFactura = New AuditLog
        auditLogEnvioDatosNoFactura.Columna = "maniobras_cliente"
        auditLogEnvioDatosNoFactura.Fecha = DateTime.Now
        auditLogEnvioDatosNoFactura.Tabla = "D_ENVIOS_DATOS"
        auditLogEnvioDatosNoFactura.Usuario = idUsuario
        auditLogEnvioDatosNoFactura.ValorAnterior = IIf(envioDato.maniobras_cliente Is Nothing, "", envioDato.maniobras_cliente.ToString())
        auditLogEnvioDatosNoFactura.ValorActual = maniobras_cliente.ToString()
        auditLogEnvioDatosNoFactura.IdEnvio = id_envio

        envioDato.maniobras_cliente = maniobras_cliente
        dbContext.D_AUDIT_LOG.Add(auditLogEnvioDatosNoFactura)

        dbContext.SaveChanges()
        Return True
    End Function

    Public Shared Function ModificacionManiobrasPropias(id_envio As Integer, comentarios As String, maniobras_propias As Double, idUsuario As Integer) As Boolean
        Dim dbContext As New SiExProEntities

        Dim auditLogEnvio = New AuditLog
        Dim envioDato As EnvioDatos = dbContext.D_ENVIOS_DATOS.FirstOrDefault(Function(x) x.id_envio = id_envio)

        InsComenatrio(id_envio, idUsuario, "Maniobras Propias Modificado: " + IIf(String.IsNullOrWhiteSpace(comentarios), "", comentarios) + " Valor Anterior:" + IIf(envioDato.maniobras_propias Is Nothing, "", envioDato.maniobras_propias.ToString()) + " Valor Actual: " + maniobras_propias.ToString())

        Dim auditLogEnvioDatosNoFactura = New AuditLog
        auditLogEnvioDatosNoFactura.Columna = "maniobras_propias"
        auditLogEnvioDatosNoFactura.Fecha = DateTime.Now
        auditLogEnvioDatosNoFactura.Tabla = "D_ENVIOS_DATOS"
        auditLogEnvioDatosNoFactura.Usuario = idUsuario
        auditLogEnvioDatosNoFactura.ValorAnterior = IIf(envioDato.maniobras_propias Is Nothing, "", envioDato.maniobras_propias.ToString())
        auditLogEnvioDatosNoFactura.ValorActual = maniobras_propias.ToString()
        auditLogEnvioDatosNoFactura.IdEnvio = id_envio

        envioDato.maniobras_propias = maniobras_propias
        dbContext.D_AUDIT_LOG.Add(auditLogEnvioDatosNoFactura)

        dbContext.SaveChanges()
        Return True
    End Function

    Public Shared Function ModificacionEstadias(id_envio As Integer, comentarios As String, estadias As Double, idUsuario As Integer) As Boolean
        Dim dbContext As New SiExProEntities

        Dim auditLogEnvio = New AuditLog
        Dim envioDato As EnvioDatos = dbContext.D_ENVIOS_DATOS.FirstOrDefault(Function(x) x.id_envio = id_envio)

        InsComenatrio(id_envio, idUsuario, "Estadias Modificado: " + IIf(String.IsNullOrWhiteSpace(comentarios), "", comentarios) + " Valor Anterior:" + IIf(envioDato.estadias Is Nothing, "", envioDato.estadias.ToString()) + " Valor Actual: " + estadias.ToString())

        Dim auditLogEnvioDatosNoFactura = New AuditLog
        auditLogEnvioDatosNoFactura.Columna = "estadias"
        auditLogEnvioDatosNoFactura.Fecha = DateTime.Now
        auditLogEnvioDatosNoFactura.Tabla = "D_ENVIOS_DATOS"
        auditLogEnvioDatosNoFactura.Usuario = idUsuario
        auditLogEnvioDatosNoFactura.ValorAnterior = IIf(envioDato.estadias Is Nothing, "", envioDato.estadias.ToString())
        auditLogEnvioDatosNoFactura.ValorActual = estadias.ToString()
        auditLogEnvioDatosNoFactura.IdEnvio = id_envio

        envioDato.estadias = estadias
        dbContext.D_AUDIT_LOG.Add(auditLogEnvioDatosNoFactura)

        dbContext.SaveChanges()
        Return True
    End Function

    Public Shared Function ModificacionDemoras(id_envio As Integer, comentarios As String, demoras As Double, idUsuario As Integer) As Boolean
        Dim dbContext As New SiExProEntities

        Dim auditLogEnvio = New AuditLog
        Dim envioDato As EnvioDatos = dbContext.D_ENVIOS_DATOS.FirstOrDefault(Function(x) x.id_envio = id_envio)

        InsComenatrio(id_envio, idUsuario, "Demoras Modificado: " + IIf(String.IsNullOrWhiteSpace(comentarios), "", comentarios) + " Valor Anterior:" + IIf(envioDato.demoras Is Nothing, "", envioDato.demoras.ToString()) + " Valor Actual: " + demoras.ToString())

        Dim auditLogEnvioDatosNoFactura = New AuditLog
        auditLogEnvioDatosNoFactura.Columna = "demoras"
        auditLogEnvioDatosNoFactura.Fecha = DateTime.Now
        auditLogEnvioDatosNoFactura.Tabla = "D_ENVIOS_DATOS"
        auditLogEnvioDatosNoFactura.Usuario = idUsuario
        auditLogEnvioDatosNoFactura.ValorAnterior = IIf(envioDato.demoras Is Nothing, "", envioDato.demoras.ToString())
        auditLogEnvioDatosNoFactura.ValorActual = demoras.ToString()
        auditLogEnvioDatosNoFactura.IdEnvio = id_envio

        envioDato.demoras = demoras
        dbContext.D_AUDIT_LOG.Add(auditLogEnvioDatosNoFactura)

        dbContext.SaveChanges()
        Return True
    End Function

    Public Shared Function ModificacionNombreProveedor(id_envio As Integer, comentarios As String, nombre_proveedor As String, idUsuario As Integer) As Boolean
        Dim dbContext As New SiExProEntities

        Dim auditLogEnvio = New AuditLog
        Dim envioDato As EnvioDatos = dbContext.D_ENVIOS_DATOS.FirstOrDefault(Function(x) x.id_envio = id_envio)

        InsComenatrio(id_envio, idUsuario, "Nombre Proveedor Modificado: " + IIf(String.IsNullOrWhiteSpace(comentarios), "", comentarios) + " Valor Anterior:" + IIf(String.IsNullOrWhiteSpace(envioDato.nombre_proveedor), "", envioDato.nombre_proveedor) + " Valor Actual: " + nombre_proveedor.ToString())

        Dim auditLogEnvioDatosNoFactura = New AuditLog
        auditLogEnvioDatosNoFactura.Columna = "nombre_proveedor"
        auditLogEnvioDatosNoFactura.Fecha = DateTime.Now
        auditLogEnvioDatosNoFactura.Tabla = "D_ENVIOS_DATOS"
        auditLogEnvioDatosNoFactura.Usuario = idUsuario
        auditLogEnvioDatosNoFactura.ValorAnterior = IIf(String.IsNullOrWhiteSpace(envioDato.nombre_proveedor), "", envioDato.nombre_proveedor)
        auditLogEnvioDatosNoFactura.ValorActual = nombre_proveedor
        auditLogEnvioDatosNoFactura.IdEnvio = id_envio

        envioDato.nombre_proveedor = nombre_proveedor
        dbContext.D_AUDIT_LOG.Add(auditLogEnvioDatosNoFactura)

        dbContext.SaveChanges()
        Return True
    End Function

    Public Shared Function RedPackRate(ByVal redPackShipRequest As ShipRequestDto) As RedPackQuoteServiceResponse
        Dim webClient As New WebClient()
        Dim resByte As Byte()
        Dim resString As String
        Dim response As New RedPackQuoteServiceResponse()
        Try

            webClient.Headers("Content-type") = "application/json;charset=utf-8"
            webClient.Encoding = Encoding.UTF8

            Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
            Dim jsonRequest = serializer.Serialize(redPackShipRequest)

            Dim reqString = Encoding.UTF8.GetBytes(jsonRequest)
            resByte = webClient.UploadData(ConfigurationManager.AppSettings("RedPAck.Rate"), "post", reqString)
            resString = Encoding.Default.GetString(resByte)
            response = serializer.Deserialize(Of RedPackQuoteServiceResponse)(resString)
            webClient.Dispose()
        Catch ex As Exception
            response.Success = False
            response.ErrorMessage = ex.Message
        End Try
        Return response
    End Function

    Public Shared Function RedPackShip(ByVal redPackShipRequest As ShipRequestDto) As RedPackShipServiceResponse
        Dim webClient As New WebClient()
        Dim resByte As Byte()
        Dim resString As String
        Dim response As New RedPackShipServiceResponse()
        Try

            webClient.Headers("Content-type") = "application/json;charset=utf-8"
            webClient.Encoding = Encoding.UTF8

            Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
            Dim jsonRequest = serializer.Serialize(redPackShipRequest)

            Dim reqString = Encoding.UTF8.GetBytes(jsonRequest)
            resByte = webClient.UploadData(ConfigurationManager.AppSettings("RedPAck.ShipDocumentacion"), "post", reqString)
            resString = Encoding.Default.GetString(resByte)
            response = serializer.Deserialize(Of RedPackShipServiceResponse)(resString)
            webClient.Dispose()
        Catch ex As Exception
            response.Success = False
            response.ErrorMessage = ex.Message
        End Try
        Return response
    End Function

    Public Shared Function SuperEnviosQuote(ByVal fedexShipRequest As ShipRequestDto) As SuperEnviosQuoteServiceResponse
        Dim webClient As New WebClient()
        Dim resByte As Byte()
        Dim resString As String
        Dim response As New SuperEnviosQuoteServiceResponse()
        Try

            webClient.Headers("Content-type") = "application/json;charset=utf-8"
            webClient.Encoding = Encoding.UTF8

            Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
            Dim jsonRequest = serializer.Serialize(fedexShipRequest)

            Dim reqString = Encoding.UTF8.GetBytes(jsonRequest)
            resByte = webClient.UploadData(ConfigurationManager.AppSettings("SuperEnvios.Quote"), "post", reqString)
            resString = Encoding.Default.GetString(resByte)
            response = serializer.Deserialize(Of SuperEnviosQuoteServiceResponse)(resString)
            webClient.Dispose()
        Catch ex As Exception
            response.Success = False
            response.ErrorMessage = ex.Message
        End Try
        Return response
    End Function
    Public Shared Function SuperEnviosShip(ByVal fedexShipRequest As ShipRequestDto) As SuperEnviosShipServiceResponse
        Dim webClient As New WebClient()
        Dim resByte As Byte()
        Dim resString As String
        Dim response As New SuperEnviosShipServiceResponse()
        Try

            webClient.Headers("Content-type") = "application/json;charset=utf-8"
            webClient.Encoding = Encoding.UTF8

            Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
            Dim jsonRequest = serializer.Serialize(fedexShipRequest)

            Dim reqString = Encoding.UTF8.GetBytes(jsonRequest)
            resByte = webClient.UploadData(ConfigurationManager.AppSettings("SuperEnvios.Ship"), "post", reqString)
            resString = Encoding.Default.GetString(resByte)
            response = serializer.Deserialize(Of SuperEnviosShipServiceResponse)(resString)
            webClient.Dispose()
        Catch ex As Exception
            response.Success = False
            response.ErrorMessage = ex.Message
        End Try
        Return response
    End Function

    Public Shared Function ConciliacionProveedores(conciliacion As List(Of ProveedorReconciliaciones)) As String
        Dim response As String = ""
        Dim dbContext As New SiExProEntities
        Try

            dbContext.D_PROVEEDOR_RECONC.AddRange(conciliacion)
            dbContext.SaveChanges()

            response = "Importacion Actualizada"
        Catch ex As Exception
            response = "No se pudieron insertar registros"
        End Try
        Return response
    End Function
End Class
