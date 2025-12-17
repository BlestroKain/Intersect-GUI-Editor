Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
Imports System.Linq
Imports IntersectGuiDesigner.Core

Public Class DesignerForm
    Private ReadOnly _document As New GuiJsonDocument()
    Private _selectedNode As UiNode
    Private _currentFilePath As String

    Private Sub DesignerForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        UpdateInspector(Nothing)
        canvasPanel.Invalidate()
    End Sub

    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        Using dialog As New OpenFileDialog()
            dialog.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*"
            dialog.Title = "Open GUI JSON"

            If dialog.ShowDialog(Me) = DialogResult.OK Then
                LoadDocument(dialog.FileName)
            End If
        End Using
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If _document.Document Is Nothing Then
            MessageBox.Show(Me, "Load a GUI JSON file before saving.", "Nothing to save", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Using dialog As New SaveFileDialog()
            dialog.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*"
            dialog.Title = "Save GUI JSON"
            dialog.FileName = If(String.IsNullOrWhiteSpace(_currentFilePath), "gui.json", Path.GetFileName(_currentFilePath))

            If dialog.ShowDialog(Me) = DialogResult.OK Then
                _document.SyncFromRoot()
                _document.Save(dialog.FileName)
                _currentFilePath = dialog.FileName
                lblCurrentFile.Text = Path.GetFileName(dialog.FileName)
            End If
        End Using
    End Sub

    Private Sub LoadDocument(path As String)
        _document.Load(path)
        _currentFilePath = path
        lblCurrentFile.Text = Path.GetFileName(path)
        PopulateTree()
        canvasPanel.Invalidate()
    End Sub

    Private Sub PopulateTree()
        treeViewNodes.BeginUpdate()
        treeViewNodes.Nodes.Clear()

        Dim root = _document.Root
        If root IsNot Nothing Then
            treeViewNodes.Nodes.Add(BuildTreeNode(root))
            treeViewNodes.ExpandAll()
            treeViewNodes.SelectedNode = treeViewNodes.Nodes(0)
        End If

        treeViewNodes.EndUpdate()
    End Sub

    Private Function BuildTreeNode(node As UiNode) As TreeNode
        Dim treeNode = New TreeNode(node.Name) With {.Tag = node}

        For Each child In node.Children
            treeNode.Nodes.Add(BuildTreeNode(child))
        Next

        Return treeNode
    End Function

    Private Sub treeViewNodes_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles treeViewNodes.AfterSelect
        _selectedNode = TryCast(e.Node.Tag, UiNode)
        UpdateInspector(_selectedNode)
        canvasPanel.Invalidate()
    End Sub

    Private Sub UpdateInspector(node As UiNode)
        If node Is Nothing Then
            boundsXText.Text = String.Empty
            boundsYText.Text = String.Empty
            boundsWidthText.Text = String.Empty
            boundsHeightText.Text = String.Empty
            dockText.Text = String.Empty
            paddingText.Text = String.Empty
            marginText.Text = String.Empty
            chkHidden.Checked = False
            chkDisabled.Checked = False
            fontText.Text = String.Empty
            textText.Text = String.Empty
            inspectorGroup.Enabled = False
            Return
        End If

        inspectorGroup.Enabled = True

        Dim bounds = node.GetBounds()
        If bounds.HasValue Then
            boundsXText.Text = bounds.Value.X.ToString()
            boundsYText.Text = bounds.Value.Y.ToString()
            boundsWidthText.Text = bounds.Value.Width.ToString()
            boundsHeightText.Text = bounds.Value.Height.ToString()
        Else
            boundsXText.Text = String.Empty
            boundsYText.Text = String.Empty
            boundsWidthText.Text = String.Empty
            boundsHeightText.Text = String.Empty
        End If

        dockText.Text = node.GetDock()
        paddingText.Text = node.GetString("Padding")
        marginText.Text = node.GetString("Margin")
        chkHidden.Checked = node.GetBoolean("Hidden") = True
        chkDisabled.Checked = node.GetBoolean("Disabled") = True
        fontText.Text = node.GetString("Font")
        textText.Text = node.GetString("Text")
    End Sub

    Private Sub bounds_TextChanged(sender As Object, e As EventArgs) Handles boundsXText.Validated, boundsYText.Validated, boundsWidthText.Validated, boundsHeightText.Validated
        If _selectedNode Is Nothing Then
            Return
        End If

        Dim x, y, width, height As Integer
        If Integer.TryParse(boundsXText.Text, x) AndAlso Integer.TryParse(boundsYText.Text, y) AndAlso Integer.TryParse(boundsWidthText.Text, width) AndAlso Integer.TryParse(boundsHeightText.Text, height) Then
            _selectedNode.SetBounds(New GuiBounds(x, y, width, height))
            canvasPanel.Invalidate()
        End If
    End Sub

    Private Sub dockText_Validated(sender As Object, e As EventArgs) Handles dockText.Validated
        If _selectedNode Is Nothing Then
            Return
        End If

        Dim value = If(String.IsNullOrWhiteSpace(dockText.Text), Nothing, dockText.Text)
        _selectedNode.SetDock(value)
    End Sub

    Private Sub paddingText_Validated(sender As Object, e As EventArgs) Handles paddingText.Validated
        If _selectedNode Is Nothing Then
            Return
        End If

        SetOptionalString(_selectedNode, "Padding", paddingText.Text)
    End Sub

    Private Sub marginText_Validated(sender As Object, e As EventArgs) Handles marginText.Validated
        If _selectedNode Is Nothing Then
            Return
        End If

        SetOptionalString(_selectedNode, "Margin", marginText.Text)
    End Sub

    Private Sub fontText_Validated(sender As Object, e As EventArgs) Handles fontText.Validated
        If _selectedNode Is Nothing Then
            Return
        End If

        SetOptionalString(_selectedNode, "Font", fontText.Text)
    End Sub

    Private Sub textText_Validated(sender As Object, e As EventArgs) Handles textText.Validated
        If _selectedNode Is Nothing Then
            Return
        End If

        SetOptionalString(_selectedNode, "Text", textText.Text)
    End Sub

    Private Sub chkHidden_CheckedChanged(sender As Object, e As EventArgs) Handles chkHidden.CheckedChanged
        If _selectedNode Is Nothing Then
            Return
        End If

        _selectedNode.SetBoolean("Hidden", chkHidden.Checked)
    End Sub

    Private Sub chkDisabled_CheckedChanged(sender As Object, e As EventArgs) Handles chkDisabled.CheckedChanged
        If _selectedNode Is Nothing Then
            Return
        End If

        _selectedNode.SetBoolean("Disabled", chkDisabled.Checked)
    End Sub

    Private Sub SetOptionalString(node As UiNode, propertyName As String, value As String)
        If String.IsNullOrWhiteSpace(value) Then
            node.RemoveProperty(propertyName)
        Else
            node.SetString(propertyName, value)
        End If
    End Sub

    Private Sub canvasPanel_Paint(sender As Object, e As PaintEventArgs) Handles canvasPanel.Paint
        e.Graphics.Clear(Color.FromArgb(30, 30, 30))

        Dim root = _document.Root
        If root Is Nothing Then
            Return
        End If

        Dim nodesWithBounds = Flatten(root).
            Select(Function(n) New With {
                Key .Node = n,
                Key .Bounds = n.GetBounds()
            }).
            Where(Function(entry) entry.Bounds.HasValue).
            Select(Function(entry) New With {
                entry.Node,
                entry.Bounds.Value
            }).
            ToList()

        If nodesWithBounds.Count = 0 Then
            Return
        End If

        Dim maxX = nodesWithBounds.Max(Function(entry) entry.Value.X + entry.Value.Width)
        Dim maxY = nodesWithBounds.Max(Function(entry) entry.Value.Y + entry.Value.Height)

        If maxX = 0 OrElse maxY = 0 Then
            Return
        End If

        Dim padding As Integer = 10
        Dim scaleX = (canvasPanel.Width - padding * 2) / Math.Max(1.0F, maxX)
        Dim scaleY = (canvasPanel.Height - padding * 2) / Math.Max(1.0F, maxY)
        Dim scale = Math.Min(scaleX, scaleY)

        Using pen As New Pen(Color.DeepSkyBlue, 1.5F), selectedPen As New Pen(Color.Yellow, 2.5F)
            For Each entry In nodesWithBounds
                Dim rectangle = New Rectangle(
                    padding + CInt(entry.Value.X * scale),
                    padding + CInt(entry.Value.Y * scale),
                    Math.Max(1, CInt(entry.Value.Width * scale)),
                    Math.Max(1, CInt(entry.Value.Height * scale))
                )

                If ReferenceEquals(entry.Node, _selectedNode) Then
                    e.Graphics.DrawRectangle(selectedPen, rectangle)
                Else
                    e.Graphics.DrawRectangle(pen, rectangle)
                End If
            Next
        End Using
    End Sub

    Private Iterator Function Flatten(node As UiNode) As IEnumerable(Of UiNode)
        Yield node
        For Each child In node.Children
            For Each descendant In Flatten(child)
                Yield descendant
            Next
        Next
    End Function
End Class
