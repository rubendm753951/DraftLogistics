Imports Microsoft.VisualBasic
Partial Public Class EstafetaLabelServiceResponse
    Public Property Data() As EstafetaLabelIndividualResult
    Public Property ErrorMessage() As String

    Public Property Success() As Boolean
End Class


Partial Public Class EstafetaLabelIndividualResult
    Public Property ElementsCount As Integer
    Public Property Elements As List(Of EstafetaServiceResponse)
    Public Property DestinationAddress As String
    Public Property ReferenceNumber As String
    Public Property Result As EstafetaResultDto
End Class

Partial Public Class EstafetaResultDto
    Public Property Code As Integer
    Public Property Description As String
End Class

Partial Public Class EstafetaServiceResponse
    Public Property Data As String
    Public Property WayBill As String
    Public Property TrackingCode As String
End Class