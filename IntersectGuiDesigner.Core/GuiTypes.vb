Imports System.Drawing
Imports System.Globalization
Imports Newtonsoft.Json.Linq

Namespace IntersectGuiDesigner.Core
    Public Structure GuiBounds
        Public Sub New(x As Integer, y As Integer, width As Integer, height As Integer)
            Me.X = x
            Me.Y = y
            Me.Width = width
            Me.Height = height
        End Sub

        Public Property X As Integer
        Public Property Y As Integer
        Public Property Width As Integer
        Public Property Height As Integer

        Public Shared Function Parse(value As String) As GuiBounds
            Dim parts = ParseIntegerList(value, 4, NameOf(GuiBounds))
            Return New GuiBounds(parts(0), parts(1), parts(2), parts(3))
        End Function

        Public Shared Function FromJObject(source As JObject, propertyName As String) As GuiBounds
            Return Parse(GetRequiredString(source, propertyName))
        End Function

        Public Function ToRectangle() As Rectangle
            Return New Rectangle(X, Y, Width, Height)
        End Function
    End Structure

    Public Structure GuiSize
        Public Sub New(width As Integer, height As Integer)
            Me.Width = width
            Me.Height = height
        End Sub

        Public Property Width As Integer
        Public Property Height As Integer

        Public Shared Function Parse(value As String) As GuiSize
            Dim parts = ParseIntegerList(value, 2, NameOf(GuiSize))
            Return New GuiSize(parts(0), parts(1))
        End Function

        Public Shared Function FromJObject(source As JObject, propertyName As String) As GuiSize
            Return Parse(GetRequiredString(source, propertyName))
        End Function

        Public Function ToDrawingSize() As Size
            Return New Size(Width, Height)
        End Function
    End Structure

    Public Structure GuiColor
        Public Sub New(red As Integer, green As Integer, blue As Integer, alpha As Integer)
            Me.Red = red
            Me.Green = green
            Me.Blue = blue
            Me.Alpha = alpha
        End Sub

        Public Property Red As Integer
        Public Property Green As Integer
        Public Property Blue As Integer
        Public Property Alpha As Integer

        Public Shared Function Parse(value As String) As GuiColor
            Dim parts = ParseIntegerList(value, 4, NameOf(GuiColor))
            Return New GuiColor(parts(0), parts(1), parts(2), parts(3))
        End Function

        Public Shared Function FromJObject(source As JObject, propertyName As String) As GuiColor
            Return Parse(GetRequiredString(source, propertyName))
        End Function

        Public Function ToDrawingColor() As Color
            Return Color.FromArgb(Alpha, Red, Green, Blue)
        End Function
    End Structure

    Friend Module GuiTypeParserHelpers
        Friend Function ParseIntegerList(value As String, expectedLength As Integer, targetName As String) As Integer()
            If String.IsNullOrWhiteSpace(value) Then
                Throw New ArgumentException($"{targetName} value must not be empty.", NameOf(value))
            End If

            Dim split = value.Split(","c).
                Select(Function(part) part.Trim()).
                Where(Function(part) part.Length > 0).
                Select(Function(part) Integer.Parse(part, NumberStyles.Integer, CultureInfo.InvariantCulture)).
                ToArray()

            If split.Length <> expectedLength Then
                Throw New FormatException($"{targetName} expects {expectedLength} numeric components separated by commas.")
            End If

            Return split
        End Function

        Friend Function GetRequiredString(source As JObject, propertyName As String) As String
            If source Is Nothing Then
                Throw New ArgumentNullException(NameOf(source))
            End If

            Dim token = source(propertyName)
            If token Is Nothing Then
                Throw New ArgumentException($"Property '{propertyName}' was not found on the provided JSON object.", NameOf(propertyName))
            End If

            Return token.ToString()
        End Function
    End Module
End Namespace
