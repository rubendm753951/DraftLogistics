'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated from a template.
'
'     Manual changes to this file may cause unexpected behavior in your application.
'     Manual changes to this file will be overwritten if the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Imports System
Imports System.Collections.Generic

Partial Public Class EnvioDatos
    Public Property id_envio As Integer
    Public Property id_cliente As Integer
    Public Property id_destinatario As Integer
    Public Property id_contenido As Nullable(Of Integer)
    Public Property observaciones As String
    Public Property exportado As Nullable(Of Boolean)
    Public Property entregado As Nullable(Of Boolean)
    Public Property fecha_entrega As Nullable(Of Date)

    Public Overridable Property C_CLIENTES As Cliente
    Public Overridable Property C_DESTINATARIOS As Destinatario

End Class
