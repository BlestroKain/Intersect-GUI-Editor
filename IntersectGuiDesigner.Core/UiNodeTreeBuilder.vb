Imports Newtonsoft.Json.Linq

Namespace IntersectGuiDesigner.Core
    Public Class UiNodeTreeBuilder
        Public Shared Function Build(rootName As String, raw As JObject) As UiNode
            Return BuildNode(rootName, raw, Nothing)
        End Function

        Private Shared Function BuildNode(name As String, raw As JObject, parent As UiNode) As UiNode
            Dim node = New UiNode(name, raw, parent)

            For Each propertyToken In raw.Properties()
                If String.Equals(propertyToken.Name, "Children", StringComparison.Ordinal) Then
                    Dim childrenContainer = TryCast(propertyToken.Value, JObject)
                    If childrenContainer Is Nothing Then
                        Continue For
                    End If

                    For Each childProperty In childrenContainer.Properties()
                        Dim childRaw = TryCast(childProperty.Value, JObject)
                        If childRaw Is Nothing OrElse Not IsUiNodeCandidate(childRaw) Then
                            Continue For
                        End If

                        node.Children.Add(BuildNode(childProperty.Name, childRaw, node))
                    Next

                    Continue For
                End If

                Dim nestedRaw = TryCast(propertyToken.Value, JObject)
                If nestedRaw Is Nothing OrElse Not IsUiNodeCandidate(nestedRaw) Then
                    Continue For
                End If

                node.Children.Add(BuildNode(propertyToken.Name, nestedRaw, node))
            Next

            Return node
        End Function

        Private Shared Function IsUiNodeCandidate(raw As JObject) As Boolean
            Dim boundsProperty = raw.Property("Bounds")
            If boundsProperty Is Nothing Then
                Return False
            End If

            If boundsProperty.Value Is Nothing OrElse boundsProperty.Value.Type = JTokenType.Null Then
                Return False
            End If

            Return True
        End Function
    End Class
End Namespace
