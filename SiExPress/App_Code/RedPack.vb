Imports Microsoft.VisualBasic

Partial Public Class RedPackQuoteServiceResponse
    Public Property Data() As QuoteServiceResponse
    Public Property ErrorMessage() As String

    Public Property Success() As Boolean
End Class

Partial Public Class QuoteServiceResponse
    Public Property DeliveryDate As DateTime
    Public Property DeliveryTime As Double
    Public Property Rate As Double
    Public Property QuoteDetail As List(Of QuoteDetail)
    Public Property ServiceType As RedPackServiceType
End Class

Partial Public Class QuoteDetail
    Public Property Price As Double
    Public Property Description As String
End Class

Partial Public Class RedPackServiceType
    Public Property ServiceId As Integer
    Public Property ServiceType As String
    Public Property Equivalence As String
    Public Property Auxiliary As List(Of AuxiliaryParcel)
End Class

Partial Public Class AuxiliaryParcel
    Public Property Description As String
    Public Property Equivalence As String
    Public Property Id As Integer
    Public Property Note As String
End Class

Partial Public Class RedPackShipServiceResponse
    Public Property Data() As RedPackShipResponse
    Public Property ErrorMessage() As String

    Public Property Success() As Boolean
End Class

Partial Public Class RedPackShipResponse
    Public Property TrackingNumber() As String
    Public Property Label() As String
End Class