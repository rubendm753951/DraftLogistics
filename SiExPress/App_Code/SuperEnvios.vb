Imports Microsoft.VisualBasic


Partial Public Class SuperEnviosQuoteServiceResponse
    Public Property Data() As SuperEnviosQuoteResponseDto
    Public Property ErrorMessage() As String

    Public Property Success() As Boolean
End Class

Partial Public Class SuperEnviosQuoteResponseDto
    Public Property paqueterias As Paqueteria()
End Class

Partial Public Class Paqueteria
    Public Property idServicio As String
    Public Property logo As String
    Public Property proveedor As String
    Public Property nombre_servicio As String
    Public Property tiempo_de_entrega As String
    Public Property precio_regular As String
    Public Property zona_extendida As String
    Public Property precio_zona_extendida As Integer
    Public Property precio As String
    Public Property TipoCotización As String
    Public Property Zona As String
End Class

Partial Public Class SuperEnviosShipServiceResponse
    Public Property Data() As SuperEnviosShipResponseDto
    Public Property ErrorMessage() As String

    Public Property Success() As Boolean
End Class

Public Class SuperEnviosShipResponseDto
    Public Property respuesta As SuperEnviosResponseDto
End Class

Public Class SuperEnviosResponseDto
    Public Property etiqueta As String
End Class