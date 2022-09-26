<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ChannelPointsEditor
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
        Me.CANCELBUTT = New System.Windows.Forms.Button()
        Me.APPLYBUTT = New System.Windows.Forms.Button()
        Me.CHPprompt = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.CHPtitle = New System.Windows.Forms.TextBox()
        Me.CounterLabel0 = New System.Windows.Forms.Label()
        Me.ColorDialog1 = New System.Windows.Forms.ColorDialog()
        Me.CHPenable = New System.Windows.Forms.Button()
        Me.CHPcolor = New System.Windows.Forms.Button()
        Me.TypeLabel = New System.Windows.Forms.Label()
        Me.CHPcost = New System.Windows.Forms.NumericUpDown()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.CHPskipqueue = New System.Windows.Forms.CheckBox()
        Me.CHPuserinput = New System.Windows.Forms.CheckBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.CHPmps = New System.Windows.Forms.NumericUpDown()
        Me.CHPcooldown = New System.Windows.Forms.NumericUpDown()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.CHPmpups = New System.Windows.Forms.NumericUpDown()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.AutoEnable = New System.Windows.Forms.CheckBox()
        Me.ProgramDependant = New System.Windows.Forms.CheckBox()
        CType(Me.CHPcost, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CHPmps, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CHPcooldown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CHPmpups, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'CANCELBUTT
        '
        Me.CANCELBUTT.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.CANCELBUTT.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CANCELBUTT.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CANCELBUTT.ForeColor = System.Drawing.Color.White
        Me.CANCELBUTT.Location = New System.Drawing.Point(230, 215)
        Me.CANCELBUTT.Name = "CANCELBUTT"
        Me.CANCELBUTT.Size = New System.Drawing.Size(243, 31)
        Me.CANCELBUTT.TabIndex = 72
        Me.CANCELBUTT.Text = "CANCEL"
        Me.CANCELBUTT.UseVisualStyleBackColor = False
        '
        'APPLYBUTT
        '
        Me.APPLYBUTT.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.APPLYBUTT.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.APPLYBUTT.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.APPLYBUTT.ForeColor = System.Drawing.Color.White
        Me.APPLYBUTT.Location = New System.Drawing.Point(5, 215)
        Me.APPLYBUTT.Name = "APPLYBUTT"
        Me.APPLYBUTT.Size = New System.Drawing.Size(219, 31)
        Me.APPLYBUTT.TabIndex = 71
        Me.APPLYBUTT.Text = "OK"
        Me.APPLYBUTT.UseVisualStyleBackColor = False
        '
        'CHPprompt
        '
        Me.CHPprompt.BackColor = System.Drawing.Color.Black
        Me.CHPprompt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.CHPprompt.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CHPprompt.ForeColor = System.Drawing.Color.White
        Me.CHPprompt.Location = New System.Drawing.Point(18, 81)
        Me.CHPprompt.Multiline = True
        Me.CHPprompt.Name = "CHPprompt"
        Me.CHPprompt.Size = New System.Drawing.Size(194, 116)
        Me.CHPprompt.TabIndex = 75
        Me.CHPprompt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(5, 64)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(219, 146)
        Me.Label1.TabIndex = 76
        Me.Label1.Text = "PROMPT"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'CHPtitle
        '
        Me.CHPtitle.BackColor = System.Drawing.Color.Black
        Me.CHPtitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.CHPtitle.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CHPtitle.ForeColor = System.Drawing.Color.White
        Me.CHPtitle.Location = New System.Drawing.Point(18, 22)
        Me.CHPtitle.Name = "CHPtitle"
        Me.CHPtitle.Size = New System.Drawing.Size(194, 27)
        Me.CHPtitle.TabIndex = 73
        Me.CHPtitle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.CHPtitle.WordWrap = False
        '
        'CounterLabel0
        '
        Me.CounterLabel0.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.CounterLabel0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.CounterLabel0.ForeColor = System.Drawing.Color.White
        Me.CounterLabel0.Location = New System.Drawing.Point(5, 5)
        Me.CounterLabel0.Name = "CounterLabel0"
        Me.CounterLabel0.Size = New System.Drawing.Size(219, 54)
        Me.CounterLabel0.TabIndex = 74
        Me.CounterLabel0.Text = "TITLE"
        Me.CounterLabel0.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'CHPenable
        '
        Me.CHPenable.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.CHPenable.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CHPenable.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CHPenable.ForeColor = System.Drawing.Color.White
        Me.CHPenable.Location = New System.Drawing.Point(354, 166)
        Me.CHPenable.Name = "CHPenable"
        Me.CHPenable.Size = New System.Drawing.Size(119, 44)
        Me.CHPenable.TabIndex = 77
        Me.CHPenable.Text = "DISABLED"
        Me.CHPenable.UseVisualStyleBackColor = False
        '
        'CHPcolor
        '
        Me.CHPcolor.BackColor = System.Drawing.Color.Gray
        Me.CHPcolor.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CHPcolor.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CHPcolor.ForeColor = System.Drawing.Color.White
        Me.CHPcolor.Location = New System.Drawing.Point(354, 124)
        Me.CHPcolor.Name = "CHPcolor"
        Me.CHPcolor.Size = New System.Drawing.Size(119, 38)
        Me.CHPcolor.TabIndex = 78
        Me.CHPcolor.Text = "BG"
        Me.CHPcolor.UseVisualStyleBackColor = False
        '
        'TypeLabel
        '
        Me.TypeLabel.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.TypeLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TypeLabel.ForeColor = System.Drawing.Color.White
        Me.TypeLabel.Location = New System.Drawing.Point(230, 65)
        Me.TypeLabel.Name = "TypeLabel"
        Me.TypeLabel.Size = New System.Drawing.Size(119, 54)
        Me.TypeLabel.TabIndex = 81
        Me.TypeLabel.Text = "MAX PER STREAM"
        Me.TypeLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'CHPcost
        '
        Me.CHPcost.BackColor = System.Drawing.Color.Black
        Me.CHPcost.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CHPcost.ForeColor = System.Drawing.Color.White
        Me.CHPcost.Location = New System.Drawing.Point(240, 21)
        Me.CHPcost.Maximum = New Decimal(New Integer() {999999, 0, 0, 0})
        Me.CHPcost.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.CHPcost.Name = "CHPcost"
        Me.CHPcost.ReadOnly = True
        Me.CHPcost.Size = New System.Drawing.Size(98, 31)
        Me.CHPcost.TabIndex = 80
        Me.CHPcost.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.CHPcost.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(230, 5)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(119, 54)
        Me.Label3.TabIndex = 79
        Me.Label3.Text = "COST"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'CHPskipqueue
        '
        Me.CHPskipqueue.AutoSize = True
        Me.CHPskipqueue.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.CHPskipqueue.Location = New System.Drawing.Point(237, 150)
        Me.CHPskipqueue.Name = "CHPskipqueue"
        Me.CHPskipqueue.Size = New System.Drawing.Size(82, 17)
        Me.CHPskipqueue.TabIndex = 85
        Me.CHPskipqueue.Text = "Skip Queue"
        Me.CHPskipqueue.UseVisualStyleBackColor = False
        '
        'CHPuserinput
        '
        Me.CHPuserinput.AutoSize = True
        Me.CHPuserinput.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.CHPuserinput.Location = New System.Drawing.Point(237, 130)
        Me.CHPuserinput.Name = "CHPuserinput"
        Me.CHPuserinput.Size = New System.Drawing.Size(83, 17)
        Me.CHPuserinput.TabIndex = 84
        Me.CHPuserinput.Text = "UI Required"
        Me.CHPuserinput.UseVisualStyleBackColor = False
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label7.ForeColor = System.Drawing.Color.White
        Me.Label7.Location = New System.Drawing.Point(230, 124)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(119, 86)
        Me.Label7.TabIndex = 83
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'CHPmps
        '
        Me.CHPmps.BackColor = System.Drawing.Color.Black
        Me.CHPmps.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CHPmps.ForeColor = System.Drawing.Color.White
        Me.CHPmps.Location = New System.Drawing.Point(240, 82)
        Me.CHPmps.Maximum = New Decimal(New Integer() {50, 0, 0, 0})
        Me.CHPmps.Name = "CHPmps"
        Me.CHPmps.ReadOnly = True
        Me.CHPmps.Size = New System.Drawing.Size(98, 31)
        Me.CHPmps.TabIndex = 89
        Me.CHPmps.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'CHPcooldown
        '
        Me.CHPcooldown.BackColor = System.Drawing.Color.Black
        Me.CHPcooldown.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CHPcooldown.ForeColor = System.Drawing.Color.White
        Me.CHPcooldown.Location = New System.Drawing.Point(364, 21)
        Me.CHPcooldown.Maximum = New Decimal(New Integer() {99999, 0, 0, 0})
        Me.CHPcooldown.Name = "CHPcooldown"
        Me.CHPcooldown.ReadOnly = True
        Me.CHPcooldown.Size = New System.Drawing.Size(98, 31)
        Me.CHPcooldown.TabIndex = 87
        Me.CHPcooldown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.CHPcooldown.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(354, 5)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(119, 54)
        Me.Label5.TabIndex = 86
        Me.Label5.Text = "COOLDOWN"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'CHPmpups
        '
        Me.CHPmpups.BackColor = System.Drawing.Color.Black
        Me.CHPmpups.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CHPmpups.ForeColor = System.Drawing.Color.White
        Me.CHPmpups.Location = New System.Drawing.Point(364, 82)
        Me.CHPmpups.Maximum = New Decimal(New Integer() {50, 0, 0, 0})
        Me.CHPmpups.Name = "CHPmpups"
        Me.CHPmpups.ReadOnly = True
        Me.CHPmpups.Size = New System.Drawing.Size(98, 31)
        Me.CHPmpups.TabIndex = 91
        Me.CHPmpups.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(354, 65)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(119, 54)
        Me.Label4.TabIndex = 92
        Me.Label4.Text = "MAX PUP STREAM"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'AutoEnable
        '
        Me.AutoEnable.AutoSize = True
        Me.AutoEnable.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.AutoEnable.Location = New System.Drawing.Point(237, 190)
        Me.AutoEnable.Name = "AutoEnable"
        Me.AutoEnable.Size = New System.Drawing.Size(111, 17)
        Me.AutoEnable.TabIndex = 95
        Me.AutoEnable.Text = "Enable on Startup"
        Me.AutoEnable.UseVisualStyleBackColor = False
        '
        'ProgramDependant
        '
        Me.ProgramDependant.AutoSize = True
        Me.ProgramDependant.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.ProgramDependant.Location = New System.Drawing.Point(237, 170)
        Me.ProgramDependant.Name = "ProgramDependant"
        Me.ProgramDependant.Size = New System.Drawing.Size(91, 17)
        Me.ProgramDependant.TabIndex = 94
        Me.ProgramDependant.Text = "Program Req."
        Me.ProgramDependant.UseVisualStyleBackColor = False
        '
        'ChannelPointsEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(31, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(31, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(478, 252)
        Me.Controls.Add(Me.AutoEnable)
        Me.Controls.Add(Me.ProgramDependant)
        Me.Controls.Add(Me.CANCELBUTT)
        Me.Controls.Add(Me.APPLYBUTT)
        Me.Controls.Add(Me.CHPmpups)
        Me.Controls.Add(Me.CHPmps)
        Me.Controls.Add(Me.CHPcooldown)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.CHPskipqueue)
        Me.Controls.Add(Me.CHPuserinput)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.TypeLabel)
        Me.Controls.Add(Me.CHPcost)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.CHPcolor)
        Me.Controls.Add(Me.CHPenable)
        Me.Controls.Add(Me.CHPprompt)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.CHPtitle)
        Me.Controls.Add(Me.CounterLabel0)
        Me.Controls.Add(Me.Label4)
        Me.ForeColor = System.Drawing.Color.White
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ChannelPointsEditor"
        Me.Text = "ChannelPointsEditor"
        CType(Me.CHPcost, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CHPmps, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CHPcooldown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CHPmpups, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents CANCELBUTT As Button
    Friend WithEvents APPLYBUTT As Button
    Friend WithEvents CHPprompt As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents CHPtitle As TextBox
    Friend WithEvents CounterLabel0 As Label
    Friend WithEvents ColorDialog1 As ColorDialog
    Friend WithEvents CHPenable As Button
    Friend WithEvents CHPcolor As Button
    Friend WithEvents TypeLabel As Label
    Friend WithEvents CHPcost As NumericUpDown
    Friend WithEvents Label3 As Label
    Friend WithEvents CHPskipqueue As CheckBox
    Friend WithEvents CHPuserinput As CheckBox
    Friend WithEvents Label7 As Label
    Friend WithEvents CHPmps As NumericUpDown
    Friend WithEvents CHPcooldown As NumericUpDown
    Friend WithEvents Label5 As Label
    Friend WithEvents CHPmpups As NumericUpDown
    Friend WithEvents Label4 As Label
    Friend WithEvents AutoEnable As CheckBox
    Friend WithEvents ProgramDependant As CheckBox
End Class
