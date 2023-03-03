﻿
Partial Class ops_pages_guia_individual_dl
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '        Try

        Dim empresa As Empresa
        Dim logoImageBase As Byte()
        If Session("empresa") IsNot Nothing Then
            Try
                empresa = CType(Session("empresa"), Empresa)
                If empresa.logo IsNot Nothing Then
                    logoImageBase = empresa.logo
                End If
            Catch ex As Exception

            End Try
        End If

        Dim proveedor = Request.QueryString("id_proveedor")

        Dim MyConnection As ConnectionStringSettings
        MyConnection = ConfigurationManager.ConnectionStrings("paqueteriaDB_ConnectionString")
        Dim connection As Data.Common.DbConnection = New Data.SqlClient.SqlConnection()
        connection.ConnectionString = MyConnection.ConnectionString

        Dim cmd As Data.IDbCommand = connection.CreateCommand()
        cmd.CommandType = Data.CommandType.Text
        connection.Open()
        If logoImageBase Is Nothing Then
            cmd.CommandText = "select top 1 logo from C_Empresas Where Nombre = 'Gombar' "
            Dim readerCompany As Data.SqlClient.SqlDataReader = cmd.ExecuteReader()
            If readerCompany.HasRows Then
                readerCompany.Read()
                logoImageBase = readerCompany.GetValue(0)
            End If
        End If

        connection.Close()

        Dim provider = String.Empty
        Select Case proveedor
            Case "10"
                provider = "F"
            Case "20"
                provider = ""
            Case "30"
                provider = "E"
            Case "40"
                provider = "P"
            Case "50"
                provider = "D"
        End Select


        Guia.DataBind()
        'Crear Guia Tabsa
        Dim warnings As Microsoft.Reporting.WebForms.Warning() = Nothing
        Dim streamids As String() = Nothing
        Dim mimeType As String = Nothing
        Dim encoding As String = Nothing
        Dim extension As String = Nothing
        Dim deviceInfo As String
        Dim bytes As Byte()
        Dim lr As New Microsoft.Reporting.WebForms.LocalReport
        lr.EnableExternalImages = True

        lr.EnableExternalImages = True
        lr.ReportPath = Server.MapPath("~/Reports/label_masupack_mult_DL.rdlc")
        lr.DataSources.Add(New Microsoft.Reporting.WebForms.ReportDataSource("DataSet2_sp_Select_Datos_Envio_Mult_DL", Guia()))

        Dim parameters = New List(Of Microsoft.Reporting.WebForms.ReportParameter)
        Dim parameterI As New Microsoft.Reporting.WebForms.ReportParameter("ImagePath", Convert.ToBase64String(logoImageBase))
        Dim parameterP As New Microsoft.Reporting.WebForms.ReportParameter("Provider", provider)
        parameters.Add(parameterI)
        parameters.Add(parameterP)

        lr.SetParameters(parameters)
        deviceInfo = "<DeviceInfo><SimplePageHeaders>True</SimplePageHeaders></DeviceInfo>"
        bytes = lr.Render("PDF", deviceInfo, mimeType, encoding, extension, streamids, warnings)

        Response.ContentType = "Application/pdf"
        Response.BinaryWrite(bytes)
        Response.End()
        'Catch ex As Exception
        '    TxtMsg.Text = "El número de guías supera el máximo de 100 " & vbCrLf & _
        '    "o no existen gías para el rango y agente indicados" & vbCrLf & _
        '    ex.Message.ToString
        'End Try

    End Sub

End Class