Imports System.Web.Script.Serialization
Imports System.Web.Services
Imports SiExProData

Partial Class ops_pages_actualizacion_facturas
    Inherits BasePage
    Public Shared Paso As String = ""
    Private Shared _idModulo As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim usuarioId As Integer = Integer.Parse(CType(HttpContext.Current.Session("id_usuario"), String))
        Dim modulo As Modulo = DaspackDALC.GetModuloPorDescripcion(Me.AppRelativeVirtualPath.ToString)

        If modulo IsNot Nothing Then
            _idModulo = modulo.IdModulo
        End If

        If Not IsPostBack Then

        End If
    End Sub

    <WebMethod()>
    Public Shared Function ReadFile(fileFullName As String) As genericResponse
        Dim response As New genericResponse
        Dim listField As New ArrayList
        Try
            Dim usuarioId As Integer = Integer.Parse(CType(HttpContext.Current.Session("id_usuario"), String))
            Dim facturas As New List(Of ActualizacionFactura)

            Using MyReader As New Microsoft.VisualBasic.FileIO.TextFieldParser(ConfigurationManager.AppSettings("fullPath") + fileFullName, Encoding.Default)
                MyReader.TextFieldType = FileIO.FieldType.Delimited
                MyReader.SetDelimiters(",")

                Dim currentRow As String()
                Dim isFirstRow As Boolean = True
                While Not MyReader.EndOfData
                    currentRow = MyReader.ReadFields()
                    If Not isFirstRow Then
                        If currentRow(0).Trim <> "" AndAlso currentRow(1).Trim <> "" Then
                            facturas.Add(ActualizacionFacturasRow(currentRow, fileFullName))
                        End If
                    Else
                        isFirstRow = False
                    End If
                End While
                response.responseArray = New ArrayList(facturas)
            End Using
            response.responseMessage = ""
            response.responseSuccess = True

            Return response
        Catch ex As Exception
            response.responseSuccess = False
            response.responseMessage = "Ocurrió un error al leer archivo -->" + ex.Message.ToString
            Return response
        Finally
            response = Nothing
        End Try
    End Function

    <WebMethod(EnableSession:=True)>
    Public Shared Function ActualizarFacturas(facturas As List(Of Object)) As genericResponse
        Dim response As New genericResponse
        Try

            Dim usuarioId As Integer = Integer.Parse(CType(HttpContext.Current.Session("id_usuario"), String))
            Dim serializer As New JavaScriptSerializer()
            Dim facturasList = serializer.ConvertToType(Of List(Of ActualizacionFactura))(facturas)
            Dim allSuccess = True
            Dim envios As String = ""
            For Each row As ActualizacionFactura In facturasList
                Dim respuesta = DaspackDALC.ModificacionFactura(row.IdEnvio, row.Comentario, row.NumeroFactura, usuarioId)
                If respuesta = False Then
                    allSuccess = False
                    envios = envios + "," + row.IdEnvio
                End If
            Next

            response.responseSuccess = allSuccess
            response.responseMessage = IIf(allSuccess, "Envios actualizados", "Algunos envios no se pudieron actualizar: " + envios)

            Return response
        Catch ex As Exception
            response.responseSuccess = False
            response.responseMessage = "Ocurrió un error al actualizar tarifas -->" + ex.Message.ToString
            Return response
        Finally
            response = Nothing
        End Try
    End Function

    Private Shared Function ActualizacionFacturasRow(row As String(), fileName As String) As ActualizacionFactura
        Dim factura As New ActualizacionFactura

        With factura
            .IdEnvio = row(0)
            .NumeroFactura = row(1)
            .Comentario = row(2)
        End With

        Return factura
    End Function
End Class
