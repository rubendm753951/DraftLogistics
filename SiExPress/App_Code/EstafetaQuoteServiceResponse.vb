Imports Microsoft.VisualBasic

Partial Public Class EstafetaQuoteServiceResponse
    Public Property Data() As IEnumerable(Of EstafetaQuoteService)
    Public Property ErrorMessage() As String

    Public Property Success() As Boolean
End Class

Public Class EstafetaQuoteService
    Public Property ServiceCode As String
    Public Property ServiceName As String
    Public Property Modality As String
    Public Property ListPrice As Double
    Public Property DiscountPrice As Double
    Public Property VATApplied As Long
    Public Property FuelChargeListPrice As Double
    Public Property FuelChargeDiscount As Double
    Public Property OverweightListPrice As Double
    Public Property OverweightPriceDiscount As Double
    Public Property FuelChargeOverweightListPrice As Double
    Public Property FuelChargeOverweightPriceDiscount As Double
    Public Property ForwardingLevelCostListPrice As Double
    Public Property ForwardingLevelCostDiscount As Double
    Public Property SpecialHandlingCostListPrice As Double
    Public Property SpecialHandlingCostDiscount As Double
    Public Property InsuredCost As Double
    Public Property TotalAmount As Double
    Public Property CoversWarranty As String
    Public Property MaxWarranty As String
    Public Property Overweight As Double
End Class


