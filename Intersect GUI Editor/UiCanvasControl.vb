Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms
Imports IntersectGuiDesigner.Core

Public Class UiCanvasControl
    Inherits Control

    Private Class RenderedNode
        Public Sub New(node As UiNode, bounds As Rectangle)
            Me.Node = node
            Me.Bounds = bounds
        End Sub

        Public ReadOnly Property Node As UiNode
        Public ReadOnly Property Bounds As Rectangle
    End Class

    Public Class UiNodeSelectedEventArgs
        Inherits EventArgs

        Public Sub New(node As UiNode)
            Me.Node = node
        End Sub

        Public ReadOnly Property Node As UiNode
    End Class

    Private _rootNode As UiNode
    Private _selectedNode As UiNode
    Private _renderedNodes As List(Of RenderedNode)
    Private _canvasSize As Size

    Public Event NodeSelected As EventHandler(Of UiNodeSelectedEventArgs)

    Public Sub New()
        DoubleBuffered = True
        SetStyle(ControlStyles.ResizeRedraw, True)
        BackColor = Color.FromArgb(20, 24, 30)
        ForeColor = Color.Gainsboro
        _renderedNodes = New List(Of RenderedNode)()
        _canvasSize = New Size(1280, 720)
        Size = _canvasSize
    End Sub

    Public Property RootNode As UiNode
        Get
            Return _rootNode
        End Get
        Set(value As UiNode)
            _rootNode = value
            Invalidate()
        End Set
    End Property

    Public Property SelectedNode As UiNode
        Get
            Return _selectedNode
        End Get
        Set(value As UiNode)
            _selectedNode = value
            Invalidate()
        End Set
    End Property

    Public Property CanvasSize As Size
        Get
            Return _canvasSize
        End Get
        Set(value As Size)
            _canvasSize = value
            Size = value
            Invalidate()
        End Set
    End Property

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        e.Graphics.Clear(BackColor)
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias
        _renderedNodes.Clear()

        If _rootNode Is Nothing Then
            DrawEmptyState(e.Graphics)
            Return
        End If

        DrawNode(e.Graphics, _rootNode, New Point(0, 0))
    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)

        Dim hitNode As UiNode = Nothing
        For index = _renderedNodes.Count - 1 To 0 Step -1
            Dim rendered = _renderedNodes(index)
            If rendered.Bounds.Contains(e.Location) Then
                hitNode = rendered.Node
                Exit For
            End If
        Next

        If hitNode IsNot _selectedNode Then
            _selectedNode = hitNode
            Invalidate()
            RaiseEvent NodeSelected(Me, New UiNodeSelectedEventArgs(hitNode))
        End If
    End Sub

    Private Sub DrawEmptyState(graphics As Graphics)
        Using brush = New SolidBrush(Color.FromArgb(120, ForeColor))
            Dim text = "Load a UI layout to preview bounds."
            Dim textSize = TextRenderer.MeasureText(text, Font)
            Dim x = Math.Max(0, (ClientSize.Width - textSize.Width) \ 2)
            Dim y = Math.Max(0, (ClientSize.Height - textSize.Height) \ 2)
            TextRenderer.DrawText(graphics, text, Font, New Point(x, y), brush.Color)
        End Using
    End Sub

    Private Sub DrawNode(graphics As Graphics, node As UiNode, offset As Point)
        If node Is Nothing Then
            Return
        End If

        Dim bounds = node.GetBounds()
        Dim nextOffset = offset

        If bounds.HasValue Then
            Dim absoluteBounds = New Rectangle(offset.X + bounds.Value.X, offset.Y + bounds.Value.Y, bounds.Value.Width, bounds.Value.Height)
            DrawNodeBounds(graphics, node, absoluteBounds)
            _renderedNodes.Add(New RenderedNode(node, absoluteBounds))
            nextOffset = absoluteBounds.Location
        End If

        For Each child In node.Children
            DrawNode(graphics, child, nextOffset)
        Next
    End Sub

    Private Sub DrawNodeBounds(graphics As Graphics, node As UiNode, bounds As Rectangle)
        Dim isSelected = node Is _selectedNode
        Dim strokeColor = If(isSelected, Color.DeepSkyBlue, Color.FromArgb(160, 200, 200, 200))
        Dim strokeWidth As Single = If(isSelected, 2.0F, 1.0F)

        Using pen = New Pen(strokeColor, strokeWidth)
            graphics.DrawRectangle(pen, bounds)
        End Using

        Dim hidden = node.GetHidden().GetValueOrDefault(False)
        If hidden Then
            Using overlayBrush = New SolidBrush(Color.FromArgb(80, Color.OrangeRed))
                graphics.FillRectangle(overlayBrush, bounds)
            End Using

            Dim hiddenLabelBounds = New Rectangle(bounds.X + 4, bounds.Bottom - Font.Height - 2, Math.Max(0, bounds.Width - 8), Font.Height)
            TextRenderer.DrawText(graphics, "Hidden", Font, hiddenLabelBounds, Color.OrangeRed, TextFormatFlags.EndEllipsis)
        End If

        Dim labelBounds = New Rectangle(bounds.X + 4, bounds.Y + 4, Math.Max(0, bounds.Width - 8), Font.Height)
        TextRenderer.DrawText(graphics, node.Name, Font, labelBounds, ForeColor, TextFormatFlags.EndEllipsis)
    End Sub
End Class
