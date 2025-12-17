Imports System.IO
Imports IntersectGuiDesigner.Core
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Module BagItemGUI
    Public Class BagItem
        Public Property Bounds As String
        Public Property Padding As String
        Public Property AlignmentEdgeDistances As String
        Public Property AlignmentTransform As String
        Public Property Margin As String
        Public Property RenderColor As String
        Public Property Alignments As String
        Public Property DrawBackground As Boolean
        Public Property MinimumSize As String
        Public Property MaximumSize As String
        Public Property Disabled As Boolean
        Public Property Hidden As Boolean
        Public Property RestrictToParent As Boolean
        Public Property MouseInputEnabled As Boolean
        Public Property HideToolTip As Boolean
        Public Property ToolTipBackground As String
        Public Property ToolTipFont As String
        Public Property ToolTipTextColor As String
        Public Property Texture As String
        Public Property HoverSound As String
        Public Property LeftMouseClickSound As String
        Public Property RightMouseClickSound As String
        Public Property Children As Children
    End Class

    Public Class Children
        Public Property BagItemIcon As IntersectIcon
        Public Property BagItemValue As IntersectLabel
    End Class

    Public Sub LoadBagItemGUI(ByVal jsonfile As String)
        Form1.StatusText("[MAIN]     Opening " & jsonfile)
        Form1.jsonValue.Text = ""
        Form1.jsonType.Text = ""
        Form1.jsonTypeCombo.Text = ""
        Form1.MainBagItemPanel.Visible = True
        Form1.RefreshBtn.Visible = True
        Form1.gridToggle.Visible = True
        Form1.SaveToolStripMenuItem.Visible = True
        Form1.toolSplitContainer.Panel2.BackColor = Color.Gray
        Form1.toolSplitContainer.Panel2.BackgroundImage = Nothing

        Dim document = New GuiJsonDocument()
        document.Load(jsonfile)
        Form1.fullJson.Text = document.Document.ToString(Formatting.Indented)
        Form1.JTokenTreeUserControl1.SetJsonSource(Form1.fullJson.Text)
        Dim imgResources As String = Application.StartupPath & "\gui\"
        Dim infoPull As BagItem = document.Document.ToObject(Of BagItem)()
        Form1.StatusText("[MAIN]     BagItem.json Deserialized")

        Dim root As JObject = document.Document
        Dim children = CType(root("Children"), JObject)
        Dim mainwindowbounds = GuiBounds.FromJObject(root, NameOf(BagItem.Bounds))
        Dim BagItemIconBounds = GuiBounds.FromJObject(CType(children("BagItemIcon"), JObject), "Bounds")
        Dim BagItemValueBounds = GuiBounds.FromJObject(CType(children("BagItemValue"), JObject), "Bounds")

        Form1.MainBagItemPanel.Location = New Point(mainwindowbounds.X, mainwindowbounds.Y)
        Form1.MainBagItemPanel.Width = mainwindowbounds.Width
        Form1.MainBagItemPanel.Height = mainwindowbounds.Height
        Form1.MainBagItemPanel.BackgroundImage = Image.FromFile(imgResources & infoPull.Texture)
        Form1.BagItemIcon.Location = New Point(BagItemIconBounds.X, BagItemIconBounds.Y)
        Form1.BagItemIcon.Width = BagItemIconBounds.Width
        Form1.BagItemIcon.Height = BagItemIconBounds.Height
        Form1.BagItemIcon.BackgroundImage = Image.FromFile(Application.StartupPath & "\resources\itemIcon.png")
        Form1.BagItemValue.Location = New Point(BagItemValueBounds.X, BagItemValueBounds.Y)
    End Sub

    Public Sub UpdateBagItemGUI(ByVal jsonfile As String)
        Dim document = New GuiJsonDocument()
        document.Load(jsonfile)
        Form1.fullJson.Text = document.Document.ToString(Formatting.Indented)
        Dim imgResources As String = Application.StartupPath & "\gui\"
        Dim infoPull As BagItem = document.Document.ToObject(Of BagItem)()
        Form1.StatusText("[MAIN]     BagItem.json Deserialized")

        Dim root As JObject = document.Document
        Dim children = CType(root("Children"), JObject)
        Dim mainwindowbounds = GuiBounds.FromJObject(root, NameOf(BagItem.Bounds))
        Dim BagItemIconBounds = GuiBounds.FromJObject(CType(children("BagItemIcon"), JObject), "Bounds")
        Dim BagItemValueBounds = GuiBounds.FromJObject(CType(children("BagItemValue"), JObject), "Bounds")

        Form1.MainBagItemPanel.Location = New Point(mainwindowbounds.X, mainwindowbounds.Y)
        Form1.MainBagItemPanel.Width = mainwindowbounds.Width
        Form1.MainBagItemPanel.Height = mainwindowbounds.Height
        Form1.MainBagItemPanel.BackgroundImage = Image.FromFile(imgResources & infoPull.Texture)
        Form1.BagItemIcon.Location = New Point(BagItemIconBounds.X, BagItemIconBounds.Y)
        Form1.BagItemIcon.Width = BagItemIconBounds.Width
        Form1.BagItemIcon.Height = BagItemIconBounds.Height
        Form1.BagItemIcon.BackgroundImage = Image.FromFile(Application.StartupPath & "\resources\itemIcon.png")
        Form1.BagItemValue.Location = New Point(BagItemValueBounds.X, BagItemValueBounds.Y)
    End Sub
End Module
