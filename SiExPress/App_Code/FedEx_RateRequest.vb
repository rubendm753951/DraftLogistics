Option Strict On
Imports System.Net
Imports RateRequest

Public Class FedEx_RateRequest
    Inherits System.Web.UI.Page

    Function Main(ByRef envio As ObjEnvio, ByRef cliente As ObjCliente, ByRef destinatario As ObjDestinatario) As String

        Dim request As RateRequest.RateRequest = CreateRateRequest(envio, cliente, destinatario)
        Dim result As String = ""
        Dim service As RateService = New RateService() ' Initialize the service

        ServicePointManager.Expect100Continue = True
        ServicePointManager.DefaultConnectionLimit = 9999
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        Dim reply As RateReply = service.getRates(request)
        Dim log As String = ConfigurationManager.AppSettings("Estafeta.LogRequestResponse")
        If log = "true" Then
            Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
            Dim jsonRequest = serializer.Serialize(request)
            Dim jsonResonse = serializer.Serialize(reply)

            DaspackDALC.LogEstafetaRequestResponse("FedexRate", jsonRequest, jsonResonse, 0, 2)
        End If
        If ((Not reply.HighestSeverity = NotificationSeverityType.ERROR) And (Not reply.HighestSeverity = NotificationSeverityType.FAILURE)) Then ' check if the call was successful
            result = ShowRateReply(reply)
        Else
            result = ""
            For Each notification As Notification In reply.Notifications
                result = result & notification.Message & vbCrLf
            Next
        End If

        Dim avisos As String
        avisos = ""
        For Each notification As Notification In reply.Notifications
            avisos = avisos & notification.Message & vbCrLf
        Next
        result = result & avisos
        Return result
    End Function

    Function CreateRateRequest(ByRef envio As ObjEnvio, ByRef cliente As ObjCliente, ByRef destinatario As ObjDestinatario) As RateRequest.RateRequest
        ' Build a RateRequest
        Dim request As RateRequest.RateRequest = New RateRequest.RateRequest()
        '
        request.WebAuthenticationDetail = New WebAuthenticationDetail()
        request.WebAuthenticationDetail.UserCredential = New WebAuthenticationCredential()
        request.WebAuthenticationDetail.UserCredential.Key = ConfigurationManager.AppSettings("FedEx.Key")
        request.WebAuthenticationDetail.UserCredential.Password = ConfigurationManager.AppSettings("FedEx.Password")
        request.ClientDetail = New ClientDetail()
        request.ClientDetail.AccountNumber = ConfigurationManager.AppSettings("FedEx.AccountNumber")
        request.ClientDetail.MeterNumber = ConfigurationManager.AppSettings("FedEx.MeterNumber")

        request.TransactionDetail = New TransactionDetail()
        request.TransactionDetail.CustomerTransactionId = "*** Rate v9 Request using VB.NET ***" ' This is a reference field for the customer.  Any value can be used and will be provided in the response.
        '
        request.Version = New VersionId() ' WSDL version information, value is automatically set from wsdl
        '
        request.ReturnTransitAndCommit = True
        request.ReturnTransitAndCommitSpecified = True
        '
        SetShipmentDetails(request, envio)
        '
        SetOrigin(request, cliente)
        '
        SetDestination(request, destinatario)
        '
        'SetPayment(request)
        ''
        'Dim isCodShipment As Boolean = False ' set to true to request COD rate
        'If (isCodShipment) Then
        '    SetCOD(request, envio)
        'End If
        '
        SetPackageLineItems(request, envio)
        '
        Return request
    End Function

    Sub SetShipmentDetails(ByRef request As RateRequest.RateRequest, ByRef envio As ObjEnvio)

        Dim today As DayOfWeek = DateTime.Now.DayOfWeek
        Dim PickupDate As Date
        If today = DayOfWeek.Sunday Then
            PickupDate = DateTime.Now.AddDays(1)
        ElseIf today = DayOfWeek.Saturday Then
            PickupDate = DateTime.Now.AddDays(2)
        Else
            PickupDate = DateTime.Now
        End If
        envio.fecha_rec = PickupDate

        request.RequestedShipment = New RequestedShipment()
        request.RequestedShipment.ShipTimestamp = DateTime.Now ' Ship date and time
        request.RequestedShipment.ShipTimestampSpecified = True
        request.RequestedShipment.RateRequestTypes = New RateRequestType(0) {RateRequestType.PREFERRED} ' Rate types requested LIST, MULTIWEIGHT, ...
        request.RequestedShipment.RateRequestTypes(0) = RateRequestType.PREFERRED
        request.RequestedShipment.DropoffType = DropoffType.REGULAR_PICKUP 'Drop off types are BUSINESS_SERVICE_CENTER, DROP_BOX, REGULAR_PICKUP, REQUEST_COURIER, STATION
        request.RequestedShipment.ServiceType = 0  ' Service types are STANDARD_OVERNIGHT, PRIORITY_OVERNIGHT, FEDEX_GROUND ...
        request.RequestedShipment.ServiceTypeSpecified = False
        request.RequestedShipment.PackagingType = 0 ' Packaging type FEDEX_BOK, FEDEX_PAK, FEDEX_TUBE, YOUR_PACKAGING, ...
        request.RequestedShipment.PackagingTypeSpecified = False

        request.RequestedShipment.TotalInsuredValue = New Money()
        request.RequestedShipment.TotalInsuredValue.Amount = 100
        request.RequestedShipment.TotalInsuredValue.Currency = "USD"
        '
        request.RequestedShipment.PackageCount = "1"

    End Sub

    Sub SetOrigin(ByRef request As RateRequest.RateRequest, ByRef cliente As ObjCliente)
        request.RequestedShipment.Shipper = New Party()
        request.RequestedShipment.Shipper.Address = New Address()
        request.RequestedShipment.Shipper.Address.StreetLines = New String(0) {cliente.direccion.ToString} '{"Sender Address Line 1"}
        request.RequestedShipment.Shipper.Address.City = cliente.ciudad.ToString  '"Memphis"
        request.RequestedShipment.Shipper.Address.StateOrProvinceCode = cliente.estadoprovincia.ToString  '"TN"
        request.RequestedShipment.Shipper.Address.PostalCode = cliente.codigo_postal.ToString  '"38115"
        request.RequestedShipment.Shipper.Address.CountryCode = cliente.codigo_pais.ToString  '"US"

    End Sub

    Sub SetDestination(ByRef request As RateRequest.RateRequest, ByRef destinatario As ObjDestinatario)
        request.RequestedShipment.Recipient = New Party()
        request.RequestedShipment.Recipient.Address = New Address()
        request.RequestedShipment.Recipient.Address.StreetLines = New String(0) {destinatario.calle.ToString} '{"Recipient Address Line 1"}
        request.RequestedShipment.Recipient.Address.City = destinatario.ciudad.ToString  '"Montreal"
        request.RequestedShipment.Recipient.Address.StateOrProvinceCode = destinatario.estadoprovincia.ToString  '"PQ"
        request.RequestedShipment.Recipient.Address.PostalCode = destinatario.codigo_postal.ToString  '"H1E1A1"
        request.RequestedShipment.Recipient.Address.CountryCode = "MX"  '"CA"
    End Sub

    Sub SetPayment(ByRef request As RateRequest.RateRequest)
        request.RequestedShipment.ShippingChargesPayment = New Payment()
        request.RequestedShipment.ShippingChargesPayment.PaymentType = PaymentType.SENDER
        request.RequestedShipment.ShippingChargesPayment.PaymentTypeSpecified = True
        request.RequestedShipment.ShippingChargesPayment.Payor = New Payor()
        request.RequestedShipment.ShippingChargesPayment.Payor.ResponsibleParty = New Party()
        request.RequestedShipment.ShippingChargesPayment.Payor.ResponsibleParty.AccountNumber = ConfigurationManager.AppSettings("FedEx.AccountNumber")
    End Sub


    'Sub SetCOD(ByRef request As RateRequest.RateRequest, ByRef envio As ObjEnvio)
    '    request.RequestedShipment.SpecialServicesRequested = New ShipmentSpecialServicesRequested() ' Special service requested
    '    request.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = New ShipmentSpecialServiceType(0) {ShipmentSpecialServiceType.COD} ' Special Services types COD, HOLD_AT_LOCATION, RESIDENTIAL DELIVERY, ...
    '    request.RequestedShipment.SpecialServicesRequested.CodDetail = New CodDetail()
    '    request.RequestedShipment.SpecialServicesRequested.CodDetail.CollectionType = CodCollectionType.ANY ' ANY, CASH, GUARANTEED_FUNDS
    '    request.RequestedShipment.SpecialServicesRequested.CodDetail.CodCollectionAmount = New Money()
    '    request.RequestedShipment.SpecialServicesRequested.CodDetail.CodCollectionAmount.Amount = CInt(envio.valor_seguro) '150
    '    request.RequestedShipment.SpecialServicesRequested.CodDetail.CodCollectionAmount.Currency = "MXN"
    'End Sub

    Sub SetPackageLineItems(ByRef request As RateRequest.RateRequest, ByRef envio As ObjEnvio)
        request.RequestedShipment.RequestedPackageLineItems = New RequestedPackageLineItem(0) {New RequestedPackageLineItem()}
        request.RequestedShipment.RequestedPackageLineItems(0).GroupPackageCount = "1"
        request.RequestedShipment.RequestedPackageLineItems(0).SequenceNumber = "1" ' package sequence number
        request.RequestedShipment.RequestedPackageLineItems(0).Weight = New Weight() ' Package weight information
        request.RequestedShipment.RequestedPackageLineItems(0).Weight.Value = CDec(envio.peso) '15D
        request.RequestedShipment.RequestedPackageLineItems(0).Weight.ValueSpecified = True
        request.RequestedShipment.RequestedPackageLineItems(0).Weight.Units = WeightUnits.KG '.LB
        request.RequestedShipment.RequestedPackageLineItems(0).Weight.UnitsSpecified = True

        request.RequestedShipment.RequestedPackageLineItems(0).Dimensions = New Dimensions() ' package dimensions
        request.RequestedShipment.RequestedPackageLineItems(0).Dimensions.Length = CInt(envio.largo).ToString  '"10"
        request.RequestedShipment.RequestedPackageLineItems(0).Dimensions.Width = CInt(envio.ancho).ToString  '"13"
        request.RequestedShipment.RequestedPackageLineItems(0).Dimensions.Height = CInt(envio.alto).ToString  '"4"
        request.RequestedShipment.RequestedPackageLineItems(0).Dimensions.Units = LinearUnits.CM '.IN
        request.RequestedShipment.RequestedPackageLineItems(0).Dimensions.UnitsSpecified = True

        ' insured value
        'request.RequestedShipment.RequestedPackageLineItems(0).InsuredValue = New Money()
        'request.RequestedShipment.RequestedPackageLineItems(0).InsuredValue.Amount = envio.valor_seguro
        'request.RequestedShipment.RequestedPackageLineItems(0).InsuredValue.Currency = "MXN"

    End Sub

    Function ShowRateReply(ByRef reply As RateReply) As String
        Dim result As String
        'Console.WriteLine("RateReply details:")
        result = "Detalles de la tarifa:" & vbCrLf
        If (reply.RateReplyDetails IsNot Nothing) Then
            For i As Integer = 0 To reply.RateReplyDetails.Length - 1
                Dim rateReplyDetail As RateReplyDetail = reply.RateReplyDetails(i)
                result = result & vbCrLf
                result = result & "Tipo de Servicio: " + rateReplyDetail.ServiceType.ToString & vbCrLf
                result = result & "Tipo de Empaque: " + rateReplyDetail.PackagingType.ToString & vbCrLf
                If (rateReplyDetail.RatedShipmentDetails IsNot Nothing) Then
                    For j As Integer = 0 To rateReplyDetail.RatedShipmentDetails.Length - 1
                        Dim ratedShipmentDetail As RatedShipmentDetail = rateReplyDetail.RatedShipmentDetails(j)
                        result = result & vbCrLf
                        result = result & "-- Tarifa consultada : " & (j + 1).ToString & vbCrLf 'Shipment and package rates for each Rate Type 
                        'result = result & vbCrLf
                        result = result & "Tipo de tarifa : " + ratedShipmentDetail.ShipmentRateDetail.RateType.ToString & vbCrLf
                        result = result & vbCrLf
                        If (ratedShipmentDetail.ShipmentRateDetail IsNot Nothing) Then
                            result = result & "--- Detalle de tarifa de envio ---" & vbCrLf
                            result = result & vbCrLf
                            result = result & ShowShipmentRateDetails(ratedShipmentDetail.ShipmentRateDetail).ToString & vbCrLf
                        End If
                        ' This is weight, and charge per package
                        If (ratedShipmentDetail.RatedPackages IsNot Nothing) Then
                            result = result & "--- Detalle de tarifa de empaque ---" & vbCrLf
                            result = result & vbCrLf
                            result = result & ShowPackageRateDetails(ratedShipmentDetail.RatedPackages) & vbCrLf
                        End If
                    Next j
                End If
                result = result & ShowDeliveryDetails(rateReplyDetail, result) & vbCrLf
            Next i
        End If

        Return result
    End Function

    Function ShowShipmentRateDetails(ByRef shipmentRateDetail As ShipmentRateDetail) As String
        Dim result As String
        result = ""
        If (shipmentRateDetail.TotalBillingWeight IsNot Nothing) Then ' Total weight of all packages
            'Console.WriteLine("Total billing weight {0} {1}", shipmentRateDetail.TotalBillingWeight.Value, shipmentRateDetail.TotalBillingWeight.Units)
            result = result & "Total de peso cobrado : " & shipmentRateDetail.TotalBillingWeight.Value.ToString & " " & shipmentRateDetail.TotalBillingWeight.Units.ToString & vbCrLf
        End If
        If (shipmentRateDetail.TotalSurcharges IsNot Nothing) Then ' Total Surcharges for all packages
            'Console.WriteLine("    Total surcharges {0} {1}", shipmentRateDetail.TotalSurcharges.Amount, shipmentRateDetail.TotalSurcharges.Currency)
            result = result & "    Cargos extras : " & shipmentRateDetail.TotalSurcharges.Amount.ToString & " " & shipmentRateDetail.TotalSurcharges.Currency.ToString & vbCrLf
        End If
        If (shipmentRateDetail.TotalNetCharge IsNot Nothing) Then ' TotalNetCharge is the Rate plus all surcharges for all packages
            'Console.WriteLine("    Total net charge {0} {1}", shipmentRateDetail.TotalNetCharge.Amount, shipmentRateDetail.TotalNetCharge.Currency)
            result = result & "    Total de Neto : " & shipmentRateDetail.TotalNetCharge.Amount.ToString & " " & shipmentRateDetail.TotalNetCharge.Currency.ToString & vbCrLf
        End If
        Return result
    End Function

    Function ShowPackageRateDetails(ByRef ratedPackages As RatedPackageDetail()) As String
        Dim result As String
        result = ""
        For k As Integer = 0 To ratedPackages.Length - 1
            Dim ratedPackage As RatedPackageDetail = ratedPackages(k)
            'Console.WriteLine("Package {0}", k + 1)
            result = result & "Empaque : " & " " & (k + 1).ToString
            If (ratedPackage.PackageRateDetail IsNot Nothing) Then
                'Console.WriteLine("Billing weight {0} {1}", ratedPackage.PackageRateDetail.BillingWeight.Value, ratedPackage.PackageRateDetail.BillingWeight.Units)
                result = result & "Peso facturado : " & ratedPackage.PackageRateDetail.BillingWeight.Value.ToString & " " & ratedPackage.PackageRateDetail.BillingWeight.Units.ToString & vbCrLf
                'Console.WriteLine("   Base charge {0} {1}", ratedPackage.PackageRateDetail.BaseCharge.Amount, ratedPackage.PackageRateDetail.BaseCharge.Currency)
                result = result & "   Cargo base : " & ratedPackage.PackageRateDetail.BaseCharge.Amount.ToString & " " & ratedPackage.PackageRateDetail.BaseCharge.Currency.ToString & vbCrLf
                If (ratedPackage.PackageRateDetail.Surcharges IsNot Nothing) Then
                    For Each surcharge As Surcharge In ratedPackage.PackageRateDetail.Surcharges
                        'Console.WriteLine("{0} surcharge {1} {2}", surcharge.SurchargeType.ToString(), surcharge.Amount.Amount, surcharge.Amount.Currency)
                        result = result + "cargos extra : " & surcharge.SurchargeType.ToString() & " " & surcharge.Amount.Amount.ToString & " " & surcharge.Amount.Currency.ToString & vbCrLf
                    Next
                End If
                'Console.WriteLine("    Net charge {0} {1}", ratedPackage.PackageRateDetail.NetCharge.Amount, ratedPackage.PackageRateDetail.NetCharge.Currency)
                result = result & " cargos netos : " & ratedPackage.PackageRateDetail.NetCharge.Amount.ToString & " " & ratedPackage.PackageRateDetail.NetCharge.Currency.ToString & vbCrLf
            End If
        Next k
        Return result
    End Function

    Function ShowDeliveryDetails(ByRef rateReplyDetail As RateReplyDetail, ByRef result As String) As String
        Dim mi_result As String
        mi_result = ""
        If (rateReplyDetail.DeliveryTimestampSpecified) Then
            'Console.WriteLine("Delivery timestamp: " + rateReplyDetail.DeliveryTimestamp.ToString)
            mi_result = mi_result & "Fecha de entrega: " + rateReplyDetail.DeliveryTimestamp.ToString & vbCrLf
        End If
        If (rateReplyDetail.TransitTimeSpecified) Then
            'Console.WriteLine("Transit time: " + rateReplyDetail.TransitTime.ToString)
            mi_result = mi_result & "Tiempo de tr?nsito: " + rateReplyDetail.TransitTime.ToString & vbCrLf
        End If
        Return mi_result
    End Function
    'End Module
End Class
