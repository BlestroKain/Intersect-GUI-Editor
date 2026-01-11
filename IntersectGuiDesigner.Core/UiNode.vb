Imports System.Drawing
Imports System.Windows.Forms
Imports System.Globalization
Imports Newtonsoft.Json.Linq

Namespace IntersectGuiDesigner.Core
    Public Class UiNode
        Public Sub New(name As String, raw As JObject, Optional parent As UiNode = Nothing)
            If String.IsNullOrWhiteSpace(name) Then
                Throw New ArgumentException("Name cannot be null or whitespace.", NameOf(name))
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
        Public Property Children As List(Of UiNode)

        Public Function GetBounds() As Rectangle?
            Dim value = GetStringProperty("Bounds")
            If String.IsNullOrWhiteSpace(value) Then
                Return Nothing
            End If

            Return GuiValueParser.ParseBounds(value)
        End Function

        Public Sub SetBounds(bounds As Rectangle)
            SetStringProperty("Bounds", GuiValueParser.SerializeBounds(bounds))
        End Sub

        Public Function GetSize() As Size?
            Dim value = GetStringProperty("Size")
            If String.IsNullOrWhiteSpace(value) Then
                Return Nothing
            End If

            Return GuiValueParser.ParseSize(value)
        End Function

        Public Sub SetSize(size As Size)
            SetStringProperty("Size", GuiValueParser.SerializeSize(size))
        End Sub

        Public Function GetColor() As Color?
            Dim value = GetStringProperty("Color")
            If String.IsNullOrWhiteSpace(value) Then
                Return Nothing
            End If

            Return GuiValueParser.ParseColor(value)
        End Function

        Public Sub SetColor(color As Color)
            SetStringProperty("Color", GuiValueParser.SerializeColor(color))
        End Sub

        Public Function GetDock() As DockStyle?
            Return ParseEnumProperty(Of DockStyle)("Dock")
        End Function

        Public Sub SetDock(dockStyle As DockStyle)
            SetStringProperty("Dock", dockStyle.ToString())
        End Sub

        Public Function GetPadding() As Padding?
            Return ParsePaddingProperty("Padding")
        End Function

        Public Sub SetPadding(padding As Padding)
            SetStringProperty("Padding", SerializePadding(padding))
        End Sub

        Public Function GetMargin() As Padding?
            Return ParsePaddingProperty("Margin")
        End Function

        Public Sub SetMargin(margin As Padding)
            SetStringProperty("Margin", SerializePadding(margin))
        End Sub

        Public Function GetHidden() As Boolean?
            Return GetBooleanProperty("Hidden")
        End Function

        Public Sub SetHidden(isHidden As Boolean)
            SetBooleanProperty("Hidden", isHidden)
        End Sub

        Public Function GetDisabled() As Boolean?
            Return GetBooleanProperty("Disabled")
        End Function

        Public Sub SetDisabled(isDisabled As Boolean)
            SetBooleanProperty("Disabled", isDisabled)
        End Sub

        Public Function GetTextAlign() As String
            Return GetStringProperty("TextAlign")
        End Function

        Public Sub SetTextAlign(value As String)
            SetStringProperty("TextAlign", value)
        End Sub

        Public Function GetTextColor() As String
            Return GetStringProperty("TextColor")
        End Function

        Public Sub SetTextColor(value As String)
            SetStringProperty("TextColor", value)
        End Sub

        Public Function GetFontName() As String
            Dim value = GetStringProperty("Font")
            If String.IsNullOrWhiteSpace(value) Then
                Return Nothing
            End If

            Dim parts = value.Split(","c)
            Return parts(0).Trim()
        End Function

        Public Function GetFontSize() As Double?
            Dim value = GetStringProperty("Font")
            If String.IsNullOrWhiteSpace(value) Then
                Return Nothing
            End If

            Dim parts = value.Split(","c)
            If parts.Length < 2 Then
                Return Nothing
            End If

            Dim parsedSize As Double
            If Double.TryParse(parts(1).Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, parsedSize) Then
                Return parsedSize
            End If

            Return Nothing
        End Function

        Public Sub SetFont(fontName As String, fontSize As Double?)
            Dim nameValue = If(fontName, String.Empty).Trim()
            Dim sizeValue As String = Nothing

            If fontSize.HasValue Then
                sizeValue = fontSize.Value.ToString(CultureInfo.InvariantCulture)
            End If

            If String.IsNullOrWhiteSpace(nameValue) AndAlso String.IsNullOrWhiteSpace(sizeValue) Then
                Raw("Font") = Nothing
            ElseIf String.IsNullOrWhiteSpace(sizeValue) Then
                Raw("Font") = nameValue
            Else
                Raw("Font") = String.Format(CultureInfo.InvariantCulture, "{0},{1}", nameValue, sizeValue)
            End If
        End Sub

        Public Function GetTextPadding() As Padding?
            Return ParsePaddingProperty("TextPadding")
        End Function

        Public Sub SetTextPadding(textPadding As Padding)
            SetStringProperty("TextPadding", SerializePadding(textPadding))
        End Sub

        Private Function GetStringProperty(propertyName As String) As String
            If Raw Is Nothing Then
                Throw New InvalidOperationException("Raw cannot be null when accessing properties.")
            End If

            Dim token = Raw(propertyName)
            If token Is Nothing OrElse token.Type = JTokenType.Null Then
                Return Nothing
            End If

            Return token.Value(Of String)()
        End Function

        Private Sub SetStringProperty(propertyName As String, value As String)
            If Raw Is Nothing Then
                Throw New InvalidOperationException("Raw cannot be null when setting properties.")
            End If

            Raw(propertyName) = value
        End Sub

        Private Function GetBooleanProperty(propertyName As String) As Boolean?
            If Raw Is Nothing Then
                Throw New InvalidOperationException("Raw cannot be null when accessing properties.")
            End If

            Dim token = Raw(propertyName)
            If token Is Nothing OrElse token.Type = JTokenType.Null Then
                Return Nothing
            End If

            If token.Type = JTokenType.Boolean Then
                Return token.Value(Of Boolean)()
            End If

            Dim parsed As Boolean
            If Boolean.TryParse(token.ToString(), parsed) Then
                Return parsed
            End If

            Return Nothing
        End Function

        Private Sub SetBooleanProperty(propertyName As String, value As Boolean)
            If Raw Is Nothing Then
                Throw New InvalidOperationException("Raw cannot be null when setting properties.")
            End If

            Raw(propertyName) = value
        End Sub

        Private Function ParsePaddingProperty(propertyName As String) As Padding?
            Dim value = GetStringProperty(propertyName)
            If String.IsNullOrWhiteSpace(value) Then
                Return Nothing
            End If

            Dim parts = ParseIntegerParts(value, 4, propertyName)
            Return New Padding(parts(0), parts(1), parts(2), parts(3))
        End Function

        Private Shared Function SerializePadding(padding As Padding) As String
            Return String.Format("{0},{1},{2},{3}", padding.Left, padding.Top, padding.Right, padding.Bottom)
        End Function

        Private Function ParseEnumProperty(Of TEnum As Structure)(propertyName As String) As TEnum?
            Dim value = GetStringProperty(propertyName)
            If String.IsNullOrWhiteSpace(value) Then
                Return Nothing
            End If

            Dim parsed As TEnum
            If [Enum].TryParse(value, True, parsed) Then
                Return parsed
            End If

            Return Nothing
        End Function

        Private Shared Function ParseIntegerParts(value As String, expectedParts As Integer, label As String) As Integer()
            If String.IsNullOrWhiteSpace(value) Then
                Throw New FormatException(String.Format("Invalid {0} value: input cannot be empty.", label))
            End If

            Dim rawParts = value.Split(","c)
            If rawParts.Length <> expectedParts Then
                Throw New FormatException(String.Format("Invalid {0} value: expected {1} parts but got {2}.", label, expectedParts, rawParts.Length))
            End If

            Dim parsed(expectedParts - 1) As Integer
            For index = 0 To rawParts.Length - 1
                Dim part = rawParts(index).Trim()
                Dim parsedValue As Integer
                If Not Integer.TryParse(part, parsedValue) Then
                    Throw New FormatException(String.Format("Invalid {0} value: '{1}' is not an integer.", label, part))
                End If
                parsed(index) = parsedValue
            Next

            Return parsed
        End Function
    End Class
End Namespace
