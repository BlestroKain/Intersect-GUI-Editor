Imports System.Collections.Generic
Imports Newtonsoft.Json.Linq

Namespace IntersectGuiDesigner.Core
    Public Class UiNode
        Public Sub New(name As String, raw As JObject, Optional parent As UiNode = Nothing)
            If String.IsNullOrWhiteSpace(name) Then
                Throw New ArgumentException("A node name is required.", NameOf(name))
            End If

            If raw Is Nothing Then
                Throw New ArgumentNullException(NameOf(raw))
            End If

            Me.Name = name
            Me.Raw = raw
            Me.Parent = parent
            Me.Children = New List(Of UiNode)()
        End Sub

        Public Property Name As String

        Public Property Raw As JObject

        Public Property Parent As UiNode

        Public ReadOnly Property Children As IList(Of UiNode)

        Public Function GetBoolean(propertyName As String) As Boolean?
            If String.IsNullOrWhiteSpace(propertyName) Then
                Throw New ArgumentException("Property name is required.", NameOf(propertyName))
            End If

            Dim token = Raw(propertyName)
            If token Is Nothing Then
                Return Nothing
            End If

            Dim result As Boolean
            If Boolean.TryParse(token.ToString(), result) Then
                Return result
            End If

            Return Nothing
        End Function

        Public Function GetBounds() As GuiBounds?
            Dim bounds = Raw("Bounds")
            If bounds Is Nothing Then
                Return Nothing
            End If

            Return GuiBounds.Parse(bounds.ToString())
        End Function

        Public Sub SetBounds(bounds As GuiBounds)
            Raw("Bounds") = $"{bounds.X}, {bounds.Y}, {bounds.Width}, {bounds.Height}"
        End Sub

        Public Function GetDock() As String
            Dim dock = Raw("Dock")
            If dock Is Nothing Then
                Return Nothing
            End If

            Return dock.ToString()
        End Function

        Public Sub SetDock(value As String)
            If value Is Nothing Then
                Raw.Remove("Dock")
            Else
                Raw("Dock") = value
            End If
        End Sub

        Public Function GetString(propertyName As String) As String
            If String.IsNullOrWhiteSpace(propertyName) Then
                Throw New ArgumentException("Property name is required.", NameOf(propertyName))
            End If

            Dim token = Raw(propertyName)
            Return If(token Is Nothing, Nothing, token.ToString())
        End Function

        Public Sub SetString(propertyName As String, value As String)
            If String.IsNullOrWhiteSpace(propertyName) Then
                Throw New ArgumentException("Property name is required.", NameOf(propertyName))
            End If

            Raw(propertyName) = value
        End Sub

        Public Sub SetBoolean(propertyName As String, value As Boolean?)
            If String.IsNullOrWhiteSpace(propertyName) Then
                Throw New ArgumentException("Property name is required.", NameOf(propertyName))
            End If

            If value.HasValue Then
                Raw(propertyName) = value.Value
            Else
                Raw.Remove(propertyName)
            End If
        End Sub

        Public Sub RemoveProperty(propertyName As String)
            If String.IsNullOrWhiteSpace(propertyName) Then
                Throw New ArgumentException("Property name is required.", NameOf(propertyName))
            End If

            Raw.Remove(propertyName)
        End Sub

        Public Function ToJObject() As JObject
            Return CType(Raw.DeepClone(), JObject)
        End Function

        Public Shared Function BuildTree(rootObject As JObject, Optional rootName As String = "Root", Optional parent As UiNode = Nothing) As UiNode
            If rootObject Is Nothing Then
                Throw New ArgumentNullException(NameOf(rootObject))
            End If

            Dim rootNode = New UiNode(rootName, rootObject, parent)
            BuildChildren(rootNode, rootObject)
            Return rootNode
        End Function

        Private Shared Sub BuildChildren(parent As UiNode, current As JObject)
            For Each propertyEntry As JProperty In current.Properties()
                Dim childObject = TryCast(propertyEntry.Value, JObject)
                If childObject Is Nothing Then
                    Continue For
                End If

                If childObject.Property("Bounds") IsNot Nothing Then
                    Dim childNode = New UiNode(propertyEntry.Name, childObject, parent)
                    parent.Children.Add(childNode)
                    BuildChildren(childNode, childObject)
                Else
                    BuildChildren(parent, childObject)
                End If
            Next
        End Sub
    End Class
End Namespace
