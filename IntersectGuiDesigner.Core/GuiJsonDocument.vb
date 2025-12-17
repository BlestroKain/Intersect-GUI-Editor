Imports System.IO
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Namespace IntersectGuiDesigner.Core
    Public Class GuiJsonDocument
        Public Property Document As JObject

        Public Function Load(path As String) As JObject
            If String.IsNullOrWhiteSpace(path) Then
                Throw New ArgumentException("A valid file path is required.", NameOf(path))
            End If

            Dim jsonText As String

            Using reader = New StreamReader(path)
                jsonText = reader.ReadToEnd()
            End Using

            Document = JObject.Parse(jsonText)
            Return Document
        End Function

        Public Sub Save(path As String)
            If Document Is Nothing Then
                Throw New InvalidOperationException("No JSON document has been loaded to save.")
            End If

            If String.IsNullOrWhiteSpace(path) Then
                Throw New ArgumentException("A valid file path is required.", NameOf(path))
            End If

            Using writer = New StreamWriter(path)
                writer.Write(Document.ToString(Formatting.Indented))
            End Using
        End Sub
    End Class
End Namespace
