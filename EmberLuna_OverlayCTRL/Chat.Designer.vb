<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Chat
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
        Me.CHAT_AREA = New System.Windows.Forms.RichTextBox()
        Me.SuspendLayout()
        '
        'CHAT_AREA
        '
        Me.CHAT_AREA.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CHAT_AREA.BackColor = System.Drawing.Color.Black
        Me.CHAT_AREA.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CHAT_AREA.ForeColor = System.Drawing.Color.White
        Me.CHAT_AREA.Location = New System.Drawing.Point(3, 2)
        Me.CHAT_AREA.Name = "CHAT_AREA"
        Me.CHAT_AREA.ReadOnly = True
        Me.CHAT_AREA.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical
        Me.CHAT_AREA.Size = New System.Drawing.Size(397, 596)
        Me.CHAT_AREA.TabIndex = 21
        Me.CHAT_AREA.Text = ""
        '
        'Chat
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(31, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(31, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(403, 601)
        Me.Controls.Add(Me.CHAT_AREA)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Chat"
        Me.Text = "CHAT MANAGER"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents CHAT_AREA As RichTextBox
End Class
