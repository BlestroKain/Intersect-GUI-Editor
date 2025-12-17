<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DesignerForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.mainLayout = New System.Windows.Forms.TableLayoutPanel()
        Me.leftPanel = New System.Windows.Forms.Panel()
        Me.treeViewNodes = New System.Windows.Forms.TreeView()
        Me.leftHeader = New System.Windows.Forms.Panel()
        Me.lblCurrentFile = New System.Windows.Forms.Label()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnLoad = New System.Windows.Forms.Button()
        Me.rightLayout = New System.Windows.Forms.TableLayoutPanel()
        Me.inspectorGroup = New System.Windows.Forms.GroupBox()
        Me.lblText = New System.Windows.Forms.Label()
        Me.textText = New System.Windows.Forms.TextBox()
        Me.fontText = New System.Windows.Forms.TextBox()
        Me.lblFont = New System.Windows.Forms.Label()
        Me.chkDisabled = New System.Windows.Forms.CheckBox()
        Me.chkHidden = New System.Windows.Forms.CheckBox()
        Me.marginText = New System.Windows.Forms.TextBox()
        Me.lblMargin = New System.Windows.Forms.Label()
        Me.paddingText = New System.Windows.Forms.TextBox()
        Me.lblPadding = New System.Windows.Forms.Label()
        Me.dockText = New System.Windows.Forms.TextBox()
        Me.lblDock = New System.Windows.Forms.Label()
        Me.boundsHeightText = New System.Windows.Forms.TextBox()
        Me.boundsWidthText = New System.Windows.Forms.TextBox()
        Me.boundsYText = New System.Windows.Forms.TextBox()
        Me.boundsXText = New System.Windows.Forms.TextBox()
        Me.lblBounds = New System.Windows.Forms.Label()
        Me.canvasPanel = New DoubleBufferedPanel()
        Me.mainLayout.SuspendLayout()
        Me.leftPanel.SuspendLayout()
        Me.leftHeader.SuspendLayout()
        Me.rightLayout.SuspendLayout()
        Me.inspectorGroup.SuspendLayout()
        Me.SuspendLayout()
        '
        'mainLayout
        '
        Me.mainLayout.ColumnCount = 2
        Me.mainLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 260.0!))
        Me.mainLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.mainLayout.Controls.Add(Me.leftPanel, 0, 0)
        Me.mainLayout.Controls.Add(Me.rightLayout, 1, 0)
        Me.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.mainLayout.Location = New System.Drawing.Point(0, 0)
        Me.mainLayout.Name = "mainLayout"
        Me.mainLayout.RowCount = 1
        Me.mainLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.mainLayout.Size = New System.Drawing.Size(1184, 681)
        Me.mainLayout.TabIndex = 0
        '
        'leftPanel
        '
        Me.leftPanel.Controls.Add(Me.treeViewNodes)
        Me.leftPanel.Controls.Add(Me.leftHeader)
        Me.leftPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.leftPanel.Location = New System.Drawing.Point(3, 3)
        Me.leftPanel.Name = "leftPanel"
        Me.leftPanel.Size = New System.Drawing.Size(254, 675)
        Me.leftPanel.TabIndex = 0
        '
        'treeViewNodes
        '
        Me.treeViewNodes.BackColor = System.Drawing.Color.FromArgb(CType(CType(26, Byte), Integer), CType(CType(26, Byte), Integer), CType(CType(26, Byte), Integer))
        Me.treeViewNodes.Dock = System.Windows.Forms.DockStyle.Fill
        Me.treeViewNodes.ForeColor = System.Drawing.Color.Gainsboro
        Me.treeViewNodes.Location = New System.Drawing.Point(0, 60)
        Me.treeViewNodes.Name = "treeViewNodes"
        Me.treeViewNodes.Size = New System.Drawing.Size(254, 615)
        Me.treeViewNodes.TabIndex = 1
        '
        'leftHeader
        '
        Me.leftHeader.Controls.Add(Me.lblCurrentFile)
        Me.leftHeader.Controls.Add(Me.btnSave)
        Me.leftHeader.Controls.Add(Me.btnLoad)
        Me.leftHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.leftHeader.Location = New System.Drawing.Point(0, 0)
        Me.leftHeader.Name = "leftHeader"
        Me.leftHeader.Padding = New System.Windows.Forms.Padding(6)
        Me.leftHeader.Size = New System.Drawing.Size(254, 60)
        Me.leftHeader.TabIndex = 0
        '
        'lblCurrentFile
        '
        Me.lblCurrentFile.AutoEllipsis = True
        Me.lblCurrentFile.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblCurrentFile.ForeColor = System.Drawing.Color.Gainsboro
        Me.lblCurrentFile.Location = New System.Drawing.Point(86, 6)
        Me.lblCurrentFile.Name = "lblCurrentFile"
        Me.lblCurrentFile.Size = New System.Drawing.Size(162, 48)
        Me.lblCurrentFile.TabIndex = 2
        Me.lblCurrentFile.Text = "No file loaded"
        Me.lblCurrentFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnSave
        '
        Me.btnSave.BackColor = System.Drawing.Color.FromArgb(CType(CType(45, Byte), Integer), CType(CType(45, Byte), Integer), CType(CType(45, Byte), Integer))
        Me.btnSave.Dock = System.Windows.Forms.DockStyle.Left
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.ForeColor = System.Drawing.Color.White
        Me.btnSave.Location = New System.Drawing.Point(46, 6)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(40, 48)
        Me.btnSave.TabIndex = 1
        Me.btnSave.Text = "💾"
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnLoad
        '
        Me.btnLoad.BackColor = System.Drawing.Color.FromArgb(CType(CType(45, Byte), Integer), CType(CType(45, Byte), Integer), CType(CType(45, Byte), Integer))
        Me.btnLoad.Dock = System.Windows.Forms.DockStyle.Left
        Me.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLoad.ForeColor = System.Drawing.Color.White
        Me.btnLoad.Location = New System.Drawing.Point(6, 6)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(40, 48)
        Me.btnLoad.TabIndex = 0
        Me.btnLoad.Text = "📂"
        Me.btnLoad.UseVisualStyleBackColor = False
        '
        'rightLayout
        '
        Me.rightLayout.ColumnCount = 1
        Me.rightLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.rightLayout.Controls.Add(Me.inspectorGroup, 0, 0)
        Me.rightLayout.Controls.Add(Me.canvasPanel, 0, 1)
        Me.rightLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rightLayout.Location = New System.Drawing.Point(263, 3)
        Me.rightLayout.Name = "rightLayout"
        Me.rightLayout.RowCount = 2
        Me.rightLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 240.0!))
        Me.rightLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.rightLayout.Size = New System.Drawing.Size(918, 675)
        Me.rightLayout.TabIndex = 1
        '
        'inspectorGroup
        '
        Me.inspectorGroup.Controls.Add(Me.lblText)
        Me.inspectorGroup.Controls.Add(Me.textText)
        Me.inspectorGroup.Controls.Add(Me.fontText)
        Me.inspectorGroup.Controls.Add(Me.lblFont)
        Me.inspectorGroup.Controls.Add(Me.chkDisabled)
        Me.inspectorGroup.Controls.Add(Me.chkHidden)
        Me.inspectorGroup.Controls.Add(Me.marginText)
        Me.inspectorGroup.Controls.Add(Me.lblMargin)
        Me.inspectorGroup.Controls.Add(Me.paddingText)
        Me.inspectorGroup.Controls.Add(Me.lblPadding)
        Me.inspectorGroup.Controls.Add(Me.dockText)
        Me.inspectorGroup.Controls.Add(Me.lblDock)
        Me.inspectorGroup.Controls.Add(Me.boundsHeightText)
        Me.inspectorGroup.Controls.Add(Me.boundsWidthText)
        Me.inspectorGroup.Controls.Add(Me.boundsYText)
        Me.inspectorGroup.Controls.Add(Me.boundsXText)
        Me.inspectorGroup.Controls.Add(Me.lblBounds)
        Me.inspectorGroup.Dock = System.Windows.Forms.DockStyle.Fill
        Me.inspectorGroup.ForeColor = System.Drawing.Color.Gainsboro
        Me.inspectorGroup.Location = New System.Drawing.Point(3, 3)
        Me.inspectorGroup.Name = "inspectorGroup"
        Me.inspectorGroup.Padding = New System.Windows.Forms.Padding(10)
        Me.inspectorGroup.Size = New System.Drawing.Size(912, 234)
        Me.inspectorGroup.TabIndex = 0
        Me.inspectorGroup.TabStop = False
        Me.inspectorGroup.Text = "Inspector"
        '
        'lblText
        '
        Me.lblText.AutoSize = True
        Me.lblText.Location = New System.Drawing.Point(13, 181)
        Me.lblText.Name = "lblText"
        Me.lblText.Size = New System.Drawing.Size(30, 16)
        Me.lblText.TabIndex = 16
        Me.lblText.Text = "Text"
        '
        'textText
        '
        Me.textText.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.textText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.textText.ForeColor = System.Drawing.Color.White
        Me.textText.Location = New System.Drawing.Point(80, 178)
        Me.textText.Name = "textText"
        Me.textText.Size = New System.Drawing.Size(280, 22)
        Me.textText.TabIndex = 8
        '
        'fontText
        '
        Me.fontText.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.fontText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.fontText.ForeColor = System.Drawing.Color.White
        Me.fontText.Location = New System.Drawing.Point(80, 150)
        Me.fontText.Name = "fontText"
        Me.fontText.Size = New System.Drawing.Size(280, 22)
        Me.fontText.TabIndex = 7
        '
        'lblFont
        '
        Me.lblFont.AutoSize = True
        Me.lblFont.Location = New System.Drawing.Point(13, 153)
        Me.lblFont.Name = "lblFont"
        Me.lblFont.Size = New System.Drawing.Size(34, 16)
        Me.lblFont.TabIndex = 13
        Me.lblFont.Text = "Font"
        '
        'chkDisabled
        '
        Me.chkDisabled.AutoSize = True
        Me.chkDisabled.Location = New System.Drawing.Point(216, 122)
        Me.chkDisabled.Name = "chkDisabled"
        Me.chkDisabled.Size = New System.Drawing.Size(84, 20)
        Me.chkDisabled.TabIndex = 6
        Me.chkDisabled.Text = "Disabled"
        Me.chkDisabled.UseVisualStyleBackColor = True
        '
        'chkHidden
        '
        Me.chkHidden.AutoSize = True
        Me.chkHidden.Location = New System.Drawing.Point(80, 122)
        Me.chkHidden.Name = "chkHidden"
        Me.chkHidden.Size = New System.Drawing.Size(69, 20)
        Me.chkHidden.TabIndex = 5
        Me.chkHidden.Text = "Hidden"
        Me.chkHidden.UseVisualStyleBackColor = True
        '
        'marginText
        '
        Me.marginText.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.marginText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.marginText.ForeColor = System.Drawing.Color.White
        Me.marginText.Location = New System.Drawing.Point(352, 94)
        Me.marginText.Name = "marginText"
        Me.marginText.Size = New System.Drawing.Size(180, 22)
        Me.marginText.TabIndex = 4
        '
        'lblMargin
        '
        Me.lblMargin.AutoSize = True
        Me.lblMargin.Location = New System.Drawing.Point(292, 97)
        Me.lblMargin.Name = "lblMargin"
        Me.lblMargin.Size = New System.Drawing.Size(50, 16)
        Me.lblMargin.TabIndex = 9
        Me.lblMargin.Text = "Margin"
        '
        'paddingText
        '
        Me.paddingText.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.paddingText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.paddingText.ForeColor = System.Drawing.Color.White
        Me.paddingText.Location = New System.Drawing.Point(80, 94)
        Me.paddingText.Name = "paddingText"
        Me.paddingText.Size = New System.Drawing.Size(180, 22)
        Me.paddingText.TabIndex = 3
        '
        'lblPadding
        '
        Me.lblPadding.AutoSize = True
        Me.lblPadding.Location = New System.Drawing.Point(13, 97)
        Me.lblPadding.Name = "lblPadding"
        Me.lblPadding.Size = New System.Drawing.Size(56, 16)
        Me.lblPadding.TabIndex = 7
        Me.lblPadding.Text = "Padding"
        '
        'dockText
        '
        Me.dockText.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.dockText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.dockText.ForeColor = System.Drawing.Color.White
        Me.dockText.Location = New System.Drawing.Point(80, 66)
        Me.dockText.Name = "dockText"
        Me.dockText.Size = New System.Drawing.Size(180, 22)
        Me.dockText.TabIndex = 2
        '
        'lblDock
        '
        Me.lblDock.AutoSize = True
        Me.lblDock.Location = New System.Drawing.Point(13, 69)
        Me.lblDock.Name = "lblDock"
        Me.lblDock.Size = New System.Drawing.Size(35, 16)
        Me.lblDock.TabIndex = 5
        Me.lblDock.Text = "Dock"
        '
        'boundsHeightText
        '
        Me.boundsHeightText.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.boundsHeightText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.boundsHeightText.ForeColor = System.Drawing.Color.White
        Me.boundsHeightText.Location = New System.Drawing.Point(488, 37)
        Me.boundsHeightText.Name = "boundsHeightText"
        Me.boundsHeightText.Size = New System.Drawing.Size(80, 22)
        Me.boundsHeightText.TabIndex = 1
        Me.boundsHeightText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'boundsWidthText
        '
        Me.boundsWidthText.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.boundsWidthText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.boundsWidthText.ForeColor = System.Drawing.Color.White
        Me.boundsWidthText.Location = New System.Drawing.Point(402, 37)
        Me.boundsWidthText.Name = "boundsWidthText"
        Me.boundsWidthText.Size = New System.Drawing.Size(80, 22)
        Me.boundsWidthText.TabIndex = 0
        Me.boundsWidthText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'boundsYText
        '
        Me.boundsYText.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.boundsYText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.boundsYText.ForeColor = System.Drawing.Color.White
        Me.boundsYText.Location = New System.Drawing.Point(196, 37)
        Me.boundsYText.Name = "boundsYText"
        Me.boundsYText.Size = New System.Drawing.Size(80, 22)
        Me.boundsYText.TabIndex = 0
        Me.boundsYText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'boundsXText
        '
        Me.boundsXText.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.boundsXText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.boundsXText.ForeColor = System.Drawing.Color.White
        Me.boundsXText.Location = New System.Drawing.Point(110, 37)
        Me.boundsXText.Name = "boundsXText"
        Me.boundsXText.Size = New System.Drawing.Size(80, 22)
        Me.boundsXText.TabIndex = 0
        Me.boundsXText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblBounds
        '
        Me.lblBounds.AutoSize = True
        Me.lblBounds.Location = New System.Drawing.Point(13, 40)
        Me.lblBounds.Name = "lblBounds"
        Me.lblBounds.Size = New System.Drawing.Size(47, 16)
        Me.lblBounds.TabIndex = 0
        Me.lblBounds.Text = "Bounds"
        '
        'canvasPanel
        '
        Me.canvasPanel.BackColor = System.Drawing.Color.Black
        Me.canvasPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.canvasPanel.Location = New System.Drawing.Point(3, 243)
        Me.canvasPanel.Name = "canvasPanel"
        Me.canvasPanel.Size = New System.Drawing.Size(912, 429)
        Me.canvasPanel.TabIndex = 1
        '
        'DesignerForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(22, Byte), Integer), CType(CType(22, Byte), Integer), CType(CType(22, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1184, 681)
        Me.Controls.Add(Me.mainLayout)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.Gainsboro
        Me.Name = "DesignerForm"
        Me.Text = "Intersect GUI Designer"
        Me.mainLayout.ResumeLayout(False)
        Me.leftPanel.ResumeLayout(False)
        Me.leftHeader.ResumeLayout(False)
        Me.rightLayout.ResumeLayout(False)
        Me.inspectorGroup.ResumeLayout(False)
        Me.inspectorGroup.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents mainLayout As TableLayoutPanel
    Friend WithEvents leftPanel As Panel
    Friend WithEvents treeViewNodes As TreeView
    Friend WithEvents leftHeader As Panel
    Friend WithEvents rightLayout As TableLayoutPanel
    Friend WithEvents inspectorGroup As GroupBox
    Friend WithEvents canvasPanel As DoubleBufferedPanel
    Friend WithEvents lblBounds As Label
    Friend WithEvents boundsHeightText As TextBox
    Friend WithEvents boundsWidthText As TextBox
    Friend WithEvents boundsYText As TextBox
    Friend WithEvents boundsXText As TextBox
    Friend WithEvents dockText As TextBox
    Friend WithEvents lblDock As Label
    Friend WithEvents paddingText As TextBox
    Friend WithEvents lblPadding As Label
    Friend WithEvents marginText As TextBox
    Friend WithEvents lblMargin As Label
    Friend WithEvents chkDisabled As CheckBox
    Friend WithEvents chkHidden As CheckBox
    Friend WithEvents fontText As TextBox
    Friend WithEvents lblFont As Label
    Friend WithEvents textText As TextBox
    Friend WithEvents lblText As Label
    Friend WithEvents btnSave As Button
    Friend WithEvents btnLoad As Button
    Friend WithEvents lblCurrentFile As Label
End Class

Public Class DoubleBufferedPanel
    Inherits Panel

    Public Sub New()
        Me.DoubleBuffered = True
        Me.ResizeRedraw = True
    End Sub
End Class
