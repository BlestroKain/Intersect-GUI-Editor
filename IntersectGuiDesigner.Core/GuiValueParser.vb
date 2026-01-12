Imports System.Drawing

Namespace IntersectGuiDesigner.Core
    Public NotInheritable Class GuiValueParser
        Private Sub New()
        End Sub

        Public Shared Function ParseBounds(value As String) As Rectangle
            Dim parts = ParseIntegerParts(value, 4, "bounds")
            Return New Rectangle(parts(0), parts(1), parts(2), parts(3))
        End Function

        Public Shared Function ParseSize(value As String) As Size
            Dim parts = ParseIntegerParts(value, 2, "size")
            Return New Size(parts(0), parts(1))
        End Function

        Public Shared Function ParseColor(value As String) As Color
            Dim parts = ParseIntegerParts(value, 4, "color")
            Return Color.FromArgb(parts(3), parts(0), parts(1), parts(2))
        End Function

        Public Shared Function ParsePadding(value As String, Optional label As String = "padding") As Padding
            Dim parts = ParseIntegerParts(value, 4, label)
            Return New Padding(parts(0), parts(1), parts(2), parts(3))
        End Function

        Public Shared Function SerializeBounds(bounds As Rectangle) As String
            Return String.Format("{0},{1},{2},{3}", bounds.X, bounds.Y, bounds.Width, bounds.Height)
        End Function

        Public Shared Function SerializeSize(size As Size) As String
            Return String.Format("{0},{1}", size.Width, size.Height)
        End Function

        Public Shared Function SerializeColor(color As Color) As String
            Return String.Format("{0},{1},{2},{3}", color.R, color.G, color.B, color.A)
        End Function

        Public Shared Function SerializePadding(padding As Padding) As String
            Return String.Format("{0},{1},{2},{3}", padding.Left, padding.Top, padding.Right, padding.Bottom)
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
