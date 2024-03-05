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
    Public Property id_proveedor As Nullable(Of Integer)
    Public Property codigoSat As String
    Public Property codigo_barras As Byte()
    Public Property no_factura As String
    Public Property importe_factura_proveedor As Nullable(Of Decimal)
    Public Property gratificacion As String
    Public Property partner_envio_id As String
    Public Property costo_envio_proveedor As String

    Public Overridable Property C_CLIENTES As Cliente
    Public Overridable Property C_DESTINATARIOS As Destinatario
    Public Overridable Property D_ENVIOS_PREASIGNADOS As EnviosPreAsignados

End Class
