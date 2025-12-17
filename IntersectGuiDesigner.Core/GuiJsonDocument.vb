Imports System.IO
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Namespace IntersectGuiDesigner.Core
    Public Class GuiJsonDocument
        Private _document As JObject
        Private _rootName As String = "Root"

        Public Property Document As JObject
            Get
                Return _document
            End Get
            Set(value As JObject)
                _document = value

                If value Is Nothing Then
                    Root = Nothing
                    Return
                End If

                Root = UiNode.BuildTree(value, _rootName)
            End Set
        End Property

        Public Property Root As UiNode

        Public Sub SyncFromRoot()
            If Root Is Nothing Then
                Return
            End If

            _document = Root.ToJObject()
        End Sub

        Public Function Load(path As String) As JObject
            If String.IsNullOrWhiteSpace(path) Then
                Throw New ArgumentException("A valid file path is required.", NameOf(path))
            End If

            Dim jsonText As String

            Using reader = New StreamReader(path)
                jsonText = reader.ReadToEnd()
            End Using

            _rootName = Path.GetFileNameWithoutExtension(path)
            Document = JObject.Parse(jsonText)
            Return Document
        End Function

        Public Sub Save(path As String)
            If Document Is Nothing Then
                Throw New InvalidOperationException("No JSON document has been loaded to save.")
            End If

            If Root IsNot Nothing Then
                SyncFromRoot()
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
