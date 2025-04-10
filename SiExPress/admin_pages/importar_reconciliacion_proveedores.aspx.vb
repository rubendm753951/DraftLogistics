
Imports SiExProData
Imports System.Web.Script.Serialization
Imports System.Web.Services

Partial Class admin_pages_importar_reconciliacion_proveedores
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
            Dim conciliacion As New List(Of ProveedorReconciliaciones)

            Using MyReader As New Microsoft.VisualBasic.FileIO.TextFieldParser(ConfigurationManager.AppSettings("fullPath") + fileFullName, Encoding.Default)
                MyReader.TextFieldType = FileIO.FieldType.Delimited
                MyReader.SetDelimiters(",")

                Dim currentRow As String()
                Dim isFirstRow As Boolean = True
                While Not MyReader.EndOfData
                    currentRow = MyReader.ReadFields()
                    If Not isFirstRow Then
                        If currentRow(0).Trim <> "" AndAlso currentRow(1).Trim <> "" Then
                            conciliacion.Add(ProveedorReconciliacionRow(currentRow, fileFullName))
                        End If
                    Else
                        isFirstRow = False
                    End If
                End While
                response.responseArray = New ArrayList(conciliacion)
            End Using
            response.responseMessage = ""
            response.responseSuccess = 1

            Return response
        Catch ex As Exception
            response.responseSuccess = False
            response.responseMessage = "Ocurrió un error al leer archivo -->" + ex.Message.ToString
            Return response
        End Try
    End Function

    <WebMethod(EnableSession:=True)>
    Public Shared Function InsertarReconciliacion(conciliacion As List(Of Object)) As genericResponse
        Dim response As New genericResponse
        Try

            Dim usuarioId As Integer = Integer.Parse(CType(HttpContext.Current.Session("id_usuario"), String))
            Dim serializer As New JavaScriptSerializer()

            Dim conciliaciones = serializer.ConvertToType(Of List(Of ProveedorReconciliaciones))(conciliacion)
            response.responseMessage = DaspackDALC.ConciliacionProveedores(conciliaciones)

            response.responseSuccess = True

            Return response
        Catch ex As Exception
            response.responseSuccess = False
            response.responseMessage = "Ocurrió un error al actualizar tarifas -->" + ex.Message.ToString
            Return response
        Finally
            response = Nothing
        End Try
    End Function



    Private Shared Function ProveedorReconciliacionRow(row As String(), fileName As String) As ProveedorReconciliaciones
        Dim tarifa As New ProveedorReconciliaciones

        Dim guiaDraft As Integer? = 0

        Integer.TryParse(row(2), guiaDraft)

        With tarifa
            .Proveedor = row(0)
            .guia_proveedor = row(1)
            .guia_draft = guiaDraft
            .costo = row(3)
            .peso = row(4)
            .no_cajas = row(5)
        End With

        Return tarifa
    End Function

End Class
