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

Partial Public Class BloodBankTable
    Public Property BloodbankID As Integer
    Public Property BloodBankName As String
    Public Property Address As String
    Public Property PhoneNo As String
    Public Property Location As String
    Public Property WebSite As String
    Public Property Email As String
    Public Property CityID As Integer
    Public Property UserID As Integer

    Public Overridable Property BloodBankStockTables As ICollection(Of BloodBankStockTable) = New HashSet(Of BloodBankStockTable)
    Public Overridable Property UserTable As UserTable
    Public Overridable Property CityTable As CityTable
    Public Overridable Property UserTable1 As UserTable

End Class
