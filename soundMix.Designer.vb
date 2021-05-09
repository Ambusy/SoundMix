<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SoundMix
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SoundMix))
        Me.c0PictureBox = New System.Windows.Forms.PictureBox()
        Me.c1PictureBox = New System.Windows.Forms.PictureBox()
        Me.c2PictureBox = New System.Windows.Forms.PictureBox()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveAndExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FinalizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UndoAddToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SummaryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PlayToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StartToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StopToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MixPlasLastToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SecsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SecsToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.SecsToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.c0Sel = New System.Windows.Forms.CheckBox()
        Me.c1Sel = New System.Windows.Forms.CheckBox()
        Me.c2Sel = New System.Windows.Forms.CheckBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.c0head = New System.Windows.Forms.Label()
        Me.c1head = New System.Windows.Forms.Label()
        Me.c2head = New System.Windows.Forms.Label()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        CType(Me.c0PictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.c1PictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.c2PictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'c0PictureBox
        '
        Me.c0PictureBox.BackColor = System.Drawing.Color.Black
        Me.c0PictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.c0PictureBox.Location = New System.Drawing.Point(33, 81)
        Me.c0PictureBox.Name = "c0PictureBox"
        Me.c0PictureBox.Size = New System.Drawing.Size(379, 55)
        Me.c0PictureBox.TabIndex = 0
        Me.c0PictureBox.TabStop = False
        Me.c0PictureBox.Visible = False
        '
        'c1PictureBox
        '
        Me.c1PictureBox.BackColor = System.Drawing.Color.Black
        Me.c1PictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.c1PictureBox.Location = New System.Drawing.Point(33, 179)
        Me.c1PictureBox.Name = "c1PictureBox"
        Me.c1PictureBox.Size = New System.Drawing.Size(379, 60)
        Me.c1PictureBox.TabIndex = 2
        Me.c1PictureBox.TabStop = False
        Me.c1PictureBox.Visible = False
        '
        'c2PictureBox
        '
        Me.c2PictureBox.BackColor = System.Drawing.Color.Black
        Me.c2PictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.c2PictureBox.Location = New System.Drawing.Point(33, 313)
        Me.c2PictureBox.Name = "c2PictureBox"
        Me.c2PictureBox.Size = New System.Drawing.Size(379, 60)
        Me.c2PictureBox.TabIndex = 4
        Me.c2PictureBox.TabStop = False
        Me.c2PictureBox.Visible = False
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.PlayToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(800, 24)
        Me.MenuStrip1.TabIndex = 6
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveAndExitToolStripMenuItem, Me.FinalizeToolStripMenuItem, Me.UndoAddToolStripMenuItem, Me.SummaryToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'SaveAndExitToolStripMenuItem
        '
        Me.SaveAndExitToolStripMenuItem.Name = "SaveAndExitToolStripMenuItem"
        Me.SaveAndExitToolStripMenuItem.Size = New System.Drawing.Size(142, 22)
        Me.SaveAndExitToolStripMenuItem.Text = "Save and exit"
        '
        'FinalizeToolStripMenuItem
        '
        Me.FinalizeToolStripMenuItem.Name = "FinalizeToolStripMenuItem"
        Me.FinalizeToolStripMenuItem.Size = New System.Drawing.Size(142, 22)
        Me.FinalizeToolStripMenuItem.Text = "Finalize"
        '
        'UndoAddToolStripMenuItem
        '
        Me.UndoAddToolStripMenuItem.Name = "UndoAddToolStripMenuItem"
        Me.UndoAddToolStripMenuItem.Size = New System.Drawing.Size(142, 22)
        Me.UndoAddToolStripMenuItem.Text = "Undo Add"
        '
        'SummaryToolStripMenuItem
        '
        Me.SummaryToolStripMenuItem.Name = "SummaryToolStripMenuItem"
        Me.SummaryToolStripMenuItem.Size = New System.Drawing.Size(142, 22)
        Me.SummaryToolStripMenuItem.Text = "Summary"
        '
        'PlayToolStripMenuItem
        '
        Me.PlayToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StartToolStripMenuItem, Me.StopToolStripMenuItem, Me.MixPlasLastToolStripMenuItem})
        Me.PlayToolStripMenuItem.Name = "PlayToolStripMenuItem"
        Me.PlayToolStripMenuItem.Size = New System.Drawing.Size(41, 20)
        Me.PlayToolStripMenuItem.Text = "Play"
        '
        'StartToolStripMenuItem
        '
        Me.StartToolStripMenuItem.Name = "StartToolStripMenuItem"
        Me.StartToolStripMenuItem.Size = New System.Drawing.Size(142, 22)
        Me.StartToolStripMenuItem.Text = "Start"
        '
        'StopToolStripMenuItem
        '
        Me.StopToolStripMenuItem.Name = "StopToolStripMenuItem"
        Me.StopToolStripMenuItem.Size = New System.Drawing.Size(142, 22)
        Me.StopToolStripMenuItem.Text = "Stop"
        '
        'MixPlasLastToolStripMenuItem
        '
        Me.MixPlasLastToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SecsToolStripMenuItem, Me.SecsToolStripMenuItem1, Me.SecsToolStripMenuItem2})
        Me.MixPlasLastToolStripMenuItem.Name = "MixPlasLastToolStripMenuItem"
        Me.MixPlasLastToolStripMenuItem.Size = New System.Drawing.Size(142, 22)
        Me.MixPlasLastToolStripMenuItem.Text = "Mix: play last"
        '
        'SecsToolStripMenuItem
        '
        Me.SecsToolStripMenuItem.Name = "SecsToolStripMenuItem"
        Me.SecsToolStripMenuItem.Size = New System.Drawing.Size(111, 22)
        Me.SecsToolStripMenuItem.Text = "5 secs"
        '
        'SecsToolStripMenuItem1
        '
        Me.SecsToolStripMenuItem1.Name = "SecsToolStripMenuItem1"
        Me.SecsToolStripMenuItem1.Size = New System.Drawing.Size(111, 22)
        Me.SecsToolStripMenuItem1.Text = "10 secs"
        '
        'SecsToolStripMenuItem2
        '
        Me.SecsToolStripMenuItem2.Name = "SecsToolStripMenuItem2"
        Me.SecsToolStripMenuItem2.Size = New System.Drawing.Size(111, 22)
        Me.SecsToolStripMenuItem2.Text = "? secs"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AboutToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(107, 22)
        Me.AboutToolStripMenuItem.Text = "About"
        '
        'c0Sel
        '
        Me.c0Sel.AutoSize = True
        Me.c0Sel.BackColor = System.Drawing.Color.White
        Me.c0Sel.Location = New System.Drawing.Point(12, 98)
        Me.c0Sel.Name = "c0Sel"
        Me.c0Sel.Size = New System.Drawing.Size(15, 14)
        Me.c0Sel.TabIndex = 7
        Me.c0Sel.TabStop = False
        Me.c0Sel.UseVisualStyleBackColor = False
        Me.c0Sel.Visible = False
        '
        'c1Sel
        '
        Me.c1Sel.AutoSize = True
        Me.c1Sel.Location = New System.Drawing.Point(12, 196)
        Me.c1Sel.Name = "c1Sel"
        Me.c1Sel.Size = New System.Drawing.Size(15, 14)
        Me.c1Sel.TabIndex = 8
        Me.c1Sel.TabStop = False
        Me.c1Sel.UseVisualStyleBackColor = True
        Me.c1Sel.Visible = False
        '
        'c2Sel
        '
        Me.c2Sel.AutoSize = True
        Me.c2Sel.Location = New System.Drawing.Point(12, 344)
        Me.c2Sel.Name = "c2Sel"
        Me.c2Sel.Size = New System.Drawing.Size(15, 14)
        Me.c2Sel.TabIndex = 9
        Me.c2Sel.TabStop = False
        Me.c2Sel.UseVisualStyleBackColor = True
        Me.c2Sel.Visible = False
        '
        'Timer1
        '
        Me.Timer1.Interval = 5
        '
        'c0head
        '
        Me.c0head.AutoSize = True
        Me.c0head.Location = New System.Drawing.Point(33, 65)
        Me.c0head.Name = "c0head"
        Me.c0head.Size = New System.Drawing.Size(92, 13)
        Me.c0head.TabIndex = 10
        Me.c0head.Text = " No file loaded yet"
        Me.c0head.Visible = False
        '
        'c1head
        '
        Me.c1head.AutoSize = True
        Me.c1head.Location = New System.Drawing.Point(33, 163)
        Me.c1head.Name = "c1head"
        Me.c1head.Size = New System.Drawing.Size(92, 13)
        Me.c1head.TabIndex = 11
        Me.c1head.Text = " No file loaded yet"
        Me.c1head.Visible = False
        '
        'c2head
        '
        Me.c2head.AutoSize = True
        Me.c2head.Location = New System.Drawing.Point(30, 297)
        Me.c2head.Name = "c2head"
        Me.c2head.Size = New System.Drawing.Size(92, 13)
        Me.c2head.TabIndex = 12
        Me.c2head.Text = " No file loaded yet"
        Me.c2head.Visible = False
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProgressBar1.Location = New System.Drawing.Point(12, 65)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(776, 10)
        Me.ProgressBar1.TabIndex = 13
        Me.ProgressBar1.Visible = False
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(61, 4)
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'ListBox1
        '
        Me.ListBox1.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.ItemHeight = 14
        Me.ListBox1.Location = New System.Drawing.Point(630, 33)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(111, 4)
        Me.ListBox1.TabIndex = 14
        Me.ListBox1.Visible = False
        '
        'SoundMix
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.ListBox1)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.c2head)
        Me.Controls.Add(Me.c1head)
        Me.Controls.Add(Me.c0head)
        Me.Controls.Add(Me.c2Sel)
        Me.Controls.Add(Me.c1Sel)
        Me.Controls.Add(Me.c0Sel)
        Me.Controls.Add(Me.c2PictureBox)
        Me.Controls.Add(Me.c1PictureBox)
        Me.Controls.Add(Me.c0PictureBox)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "SoundMix"
        Me.Text = "Form1"
        CType(Me.c0PictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.c1PictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.c2PictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents c0PictureBox As PictureBox
    Friend WithEvents c1PictureBox As PictureBox
    Friend WithEvents c2PictureBox As PictureBox
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents c0Sel As CheckBox
    Friend WithEvents c1Sel As CheckBox
    Friend WithEvents c2Sel As CheckBox
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PlayToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents StartToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents StopToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Timer1 As Timer
    Friend WithEvents c0head As Label
    Friend WithEvents c1head As Label
    Friend WithEvents c2head As Label
    Friend WithEvents ProgressBar1 As ProgressBar
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents MixPlasLastToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SecsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SecsToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents SaveAndExitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents FinalizeToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents SecsToolStripMenuItem2 As ToolStripMenuItem
    Friend WithEvents UndoAddToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SaveFileDialog1 As SaveFileDialog
    Friend WithEvents SummaryToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ListBox1 As ListBox
    Friend WithEvents HelpToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As ToolStripMenuItem
End Class
