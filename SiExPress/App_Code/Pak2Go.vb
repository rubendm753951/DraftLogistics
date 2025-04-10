Imports Microsoft.VisualBasic

Partial Public Class Pak2GoShipServiceResponse
    Public Property Data() As Pak2GoShipIncluded
    Public Property ErrorMessage() As String

    Public Property Success() As Boolean
End Class

Partial Public Class Pak2GoLabelServiceResponse
    Public Property Data() As Pak2GoLabelResponseDto
    Public Property ErrorMessage() As String

    Public Property Success() As Boolean
End Class

Public Class Pak2GoLabelResponseDto
    Public Property data As Pak2GoData
    Public Property message As String
    Public Property code As String
End Class

Public Class Pak2GoLabelAttributes
    Public Property created_at As DateTime
    Public Property updated_at As DateTime
    Public Property status As String
    Public Property tracking_number As String
    Public Property label_url As String
    Public Property tracking_url_provider As String
    Public Property rate_id As Integer
    Public Property label_urls_png As List(Of String)
    Public Property label_urls_zpl As List(Of String)
    Public Property error_message As Pak2GoErrorMessage
End Class

Public Class Pak2GoData
    Public Property id As String
    Public Property type As String
    Public Property attributes As Pak2GoLabelAttributes
End Class

Public Class Pak2GoErrorMessage
    Public Property coupon As List(Of String)
    Public Property message As String
    Public Property code As String
End Class


Public Class Pak2GoShipIncluded
    Public Property id As String
    Public Property type As String
    Public Property attributes As Pak2GoShipAttributes
End Class

Public Class Pak2GoShipAttributes
    Public Property status As String
    Public Property created_at As DateTime
    Public Property updated_at As DateTime
    Public Property overweight_status As String
    Public Property overweight_created_at As Object
    Public Property overweight As Integer
    Public Property overweight_amount As Double
    Public Property kg_sent As Object
    Public Property insurance_amount As String
    Public Property insurance_currency As Object
    Public Property declared_cost As String
    Public Property ecommerce_source As String
    Public Property ecommerce_order_id As String
    Public Property length As String
    Public Property height As String
    Public Property width As String
    Public Property weight As String
    Public Property mass_unit As String
    Public Property distance_unit As String
    Public Property amount_local As String
    Public Property currency_local As String
    Public Property provider As String
    Public Property service_level_name As String
    Public Property service_level_code As String
    Public Property additional_services As List(Of Pak2GoAdditionalServices)
    Public Property service_level_terms As Object
    Public Property days As Integer?
    Public Property duration_terms As Object
    Public Property zone As Object
    Public Property arrives_by As Object
    Public Property out_of_area As Boolean?
    Public Property out_of_area_pricing As String
    Public Property total_pricing As String
    Public Property name As String
    Public Property company As String
    Public Property rfc As String
    Public Property address1 As String
    Public Property address2 As String
    Public Property city As String
    Public Property province As String
    Public Property zip As String
    Public Property country As String
    Public Property phone As String
    Public Property email As String
    Public Property reference As String
    Public Property province_code As String
    Public Property contents As Object
End Class

Public Class Pak2GoAdditionalServices
    Public Property name As String
    Public Property cost As Double
End Class
