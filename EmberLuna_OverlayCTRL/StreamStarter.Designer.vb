<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class StreamStarter
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
        Me.StrimCat = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.StrimTitle = New System.Windows.Forms.TextBox()
        Me.CounterLabel0 = New System.Windows.Forms.Label()
        Me.UsrReset = New System.Windows.Forms.CheckBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.StartBUTT = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'StrimCat
        '
        Me.StrimCat.BackColor = System.Drawing.Color.Black
        Me.StrimCat.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.StrimCat.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StrimCat.ForeColor = System.Drawing.Color.White
        Me.StrimCat.FormattingEnabled = True
        Me.StrimCat.Location = New System.Drawing.Point(25, 90)
        Me.StrimCat.Name = "StrimCat"
        Me.StrimCat.Size = New System.Drawing.Size(292, 27)
        Me.StrimCat.TabIndex = 24
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(12, 72)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(320, 54)
        Me.Label5.TabIndex = 23
        Me.Label5.Text = "CATEGORY"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'StrimTitle
        '
        Me.StrimTitle.BackColor = System.Drawing.Color.Black
        Me.StrimTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.StrimTitle.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StrimTitle.ForeColor = System.Drawing.Color.White
        Me.StrimTitle.Location = New System.Drawing.Point(25, 26)
        Me.StrimTitle.Name = "StrimTitle"
        Me.StrimTitle.Size = New System.Drawing.Size(539, 27)
        Me.StrimTitle.TabIndex = 21
        Me.StrimTitle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.StrimTitle.WordWrap = False
        '
        'CounterLabel0
        '
        Me.CounterLabel0.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.CounterLabel0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.CounterLabel0.ForeColor = System.Drawing.Color.White
        Me.CounterLabel0.Location = New System.Drawing.Point(12, 9)
        Me.CounterLabel0.Name = "CounterLabel0"
        Me.CounterLabel0.Size = New System.Drawing.Size(564, 54)
        Me.CounterLabel0.TabIndex = 22
        Me.CounterLabel0.Text = "STREAM TITLE"
        Me.CounterLabel0.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'UsrReset
        '
        Me.UsrReset.AutoSize = True
        Me.UsrReset.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.UsrReset.Checked = True
        Me.UsrReset.CheckState = System.Windows.Forms.CheckState.Checked
        Me.UsrReset.ForeColor = System.Drawing.Color.White
        Me.UsrReset.Location = New System.Drawing.Point(350, 91)
        Me.UsrReset.Name = "UsrReset"
        Me.UsrReset.Size = New System.Drawing.Size(95, 17)
        Me.UsrReset.TabIndex = 77
        Me.UsrReset.Text = "USER RESET"
        Me.UsrReset.UseVisualStyleBackColor = False
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label7.ForeColor = System.Drawing.Color.White
        Me.Label7.Location = New System.Drawing.Point(339, 72)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(110, 54)
        Me.Label7.TabIndex = 76
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'StartBUTT
        '
        Me.StartBUTT.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(82, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.StartBUTT.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.StartBUTT.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StartBUTT.ForeColor = System.Drawing.Color.White
        Me.StartBUTT.Location = New System.Drawing.Point(456, 72)
        Me.StartBUTT.Name = "StartBUTT"
        Me.StartBUTT.Size = New System.Drawing.Size(120, 54)
        Me.StartBUTT.TabIndex = 78
        Me.StartBUTT.Text = "START STREAM"
        Me.StartBUTT.UseVisualStyleBackColor = False
        '
        'StreamStarter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(31, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(31, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(586, 137)
        Me.Controls.Add(Me.StartBUTT)
        Me.Controls.Add(Me.UsrReset)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.StrimCat)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.StrimTitle)
        Me.Controls.Add(Me.CounterLabel0)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "StreamStarter"
        Me.Text = "Stream Starter"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents StrimCat As ComboBox
    Friend WithEvents Label5 As Label
    Friend WithEvents StrimTitle As TextBox
    Friend WithEvents CounterLabel0 As Label
    Friend WithEvents UsrReset As CheckBox
    Friend WithEvents Label7 As Label
    Friend WithEvents StartBUTT As Button
End Class
