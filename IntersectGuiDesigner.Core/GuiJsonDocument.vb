Imports System.IO
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Namespace IntersectGuiDesigner.Core
    Public Class GuiJsonDocument
        Public Sub New(document As JObject)
            Me.Document = document
        End Sub

        Public Property Document As JObject

        Public Shared Function Load(path As String) As JObject
            Dim json = File.ReadAllText(path)
            Return JObject.Parse(json)
        End Function

        Public Sub Save(path As String)
            If Document Is Nothing Then
                Throw New InvalidOperationException("Document cannot be null when saving.")
            End If

            Dim json = Document.ToString(Formatting.Indented)
            File.WriteAllText(path, json)
        End Sub
    End Class
End Namespace
