<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AddTimer
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
        Me.AddBUTT = New System.Windows.Forms.Button()
        Me.TimerID = New System.Windows.Forms.NumericUpDown()
        Me.TimerIDlabel = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.AddSoundButt = New System.Windows.Forms.Button()
        Me.SoundPicker = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.TimerSoundlist = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.TimerLabel = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.TimerName = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.SoundTimeDisplay = New System.Windows.Forms.TextBox()
        Me.SoundTime = New System.Windows.Forms.NumericUpDown()
        Me.RemoveSoundButt = New System.Windows.Forms.Button()
        Me.PlaySoundButt = New System.Windows.Forms.Button()
        Me.TimerTimeDisplay = New System.Windows.Forms.TextBox()
        Me.TimerTime = New System.Windows.Forms.NumericUpDown()
        Me.PubPriBUTT = New System.Windows.Forms.Label()
        Me.StopTimeDisplay = New System.Windows.Forms.TextBox()
        Me.StopTime = New System.Windows.Forms.NumericUpDown()
        Me.Label1 = New System.Windows.Forms.Label()
        CType(Me.TimerID, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SoundTime, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TimerTime, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.StopTime, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'AddBUTT
        '
        Me.AddBUTT.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.AddBUTT.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.AddBUTT.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AddBUTT.ForeColor = System.Drawing.Color.White
        Me.AddBUTT.Location = New System.Drawing.Point(228, 314)
        Me.AddBUTT.Name = "AddBUTT"
        Me.AddBUTT.Size = New System.Drawing.Size(107, 37)
        Me.AddBUTT.TabIndex = 88
        Me.AddBUTT.Text = "OK"
        Me.AddBUTT.UseVisualStyleBackColor = False
        '
        'TimerID
        '
        Me.TimerID.BackColor = System.Drawing.Color.Black
        Me.TimerID.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TimerID.ForeColor = System.Drawing.Color.White
        Me.TimerID.Location = New System.Drawing.Point(241, 239)
        Me.TimerID.Maximum = New Decimal(New Integer() {2, 0, 0, 0})
        Me.TimerID.Name = "TimerID"
        Me.TimerID.ReadOnly = True
        Me.TimerID.Size = New System.Drawing.Size(79, 31)
        Me.TimerID.TabIndex = 85
        Me.TimerID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TimerIDlabel
        '
        Me.TimerIDlabel.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.TimerIDlabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TimerIDlabel.ForeColor = System.Drawing.Color.White
        Me.TimerIDlabel.Location = New System.Drawing.Point(228, 223)
        Me.TimerIDlabel.Name = "TimerIDlabel"
        Me.TimerIDlabel.Size = New System.Drawing.Size(107, 54)
        Me.TimerIDlabel.TabIndex = 84
        Me.TimerIDlabel.Text = "GLOBAL"
        Me.TimerIDlabel.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(228, 119)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(107, 48)
        Me.Label3.TabIndex = 82
        Me.Label3.Text = "START TIME"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'AddSoundButt
        '
        Me.AddSoundButt.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.AddSoundButt.BackgroundImage = Global.EmberLuna_OverlayCTRL.My.Resources.Resources.plus
        Me.AddSoundButt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.AddSoundButt.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.AddSoundButt.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AddSoundButt.ForeColor = System.Drawing.Color.White
        Me.AddSoundButt.Location = New System.Drawing.Point(104, 283)
        Me.AddSoundButt.Name = "AddSoundButt"
        Me.AddSoundButt.Size = New System.Drawing.Size(32, 30)
        Me.AddSoundButt.TabIndex = 110
        Me.AddSoundButt.UseVisualStyleBackColor = False
        '
        'SoundPicker
        '
        Me.SoundPicker.BackColor = System.Drawing.Color.Black
        Me.SoundPicker.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.SoundPicker.Font = New System.Drawing.Font("Calibri", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SoundPicker.ForeColor = System.Drawing.Color.White
        Me.SoundPicker.FormattingEnabled = True
        Me.SoundPicker.Location = New System.Drawing.Point(17, 319)
        Me.SoundPicker.Name = "SoundPicker"
        Me.SoundPicker.Size = New System.Drawing.Size(194, 21)
        Me.SoundPicker.TabIndex = 109
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label8.ForeColor = System.Drawing.Color.White
        Me.Label8.Location = New System.Drawing.Point(4, 274)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(219, 77)
        Me.Label8.TabIndex = 108
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'TimerSoundlist
        '
        Me.TimerSoundlist.BackColor = System.Drawing.Color.Black
        Me.TimerSoundlist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TimerSoundlist.Font = New System.Drawing.Font("Calibri", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TimerSoundlist.ForeColor = System.Drawing.Color.White
        Me.TimerSoundlist.Location = New System.Drawing.Point(17, 137)
        Me.TimerSoundlist.Multiline = True
        Me.TimerSoundlist.Name = "TimerSoundlist"
        Me.TimerSoundlist.ReadOnly = True
        Me.TimerSoundlist.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TimerSoundlist.Size = New System.Drawing.Size(194, 122)
        Me.TimerSoundlist.TabIndex = 106
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label9.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.White
        Me.Label9.Location = New System.Drawing.Point(4, 119)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(219, 151)
        Me.Label9.TabIndex = 107
        Me.Label9.Text = "SOUND LIST"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'TimerLabel
        '
        Me.TimerLabel.BackColor = System.Drawing.Color.Black
        Me.TimerLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TimerLabel.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TimerLabel.ForeColor = System.Drawing.Color.White
        Me.TimerLabel.Location = New System.Drawing.Point(17, 78)
        Me.TimerLabel.Name = "TimerLabel"
        Me.TimerLabel.Size = New System.Drawing.Size(306, 27)
        Me.TimerLabel.TabIndex = 104
        Me.TimerLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.TimerLabel.WordWrap = False
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label10.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.White
        Me.Label10.Location = New System.Drawing.Point(4, 61)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(331, 54)
        Me.Label10.TabIndex = 105
        Me.Label10.Text = "LABEL"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'TimerName
        '
        Me.TimerName.BackColor = System.Drawing.Color.Black
        Me.TimerName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TimerName.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TimerName.ForeColor = System.Drawing.Color.White
        Me.TimerName.Location = New System.Drawing.Point(17, 20)
        Me.TimerName.Name = "TimerName"
        Me.TimerName.Size = New System.Drawing.Size(306, 27)
        Me.TimerName.TabIndex = 102
        Me.TimerName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.TimerName.WordWrap = False
        '
        'Label11
        '
        Me.Label11.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label11.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.White
        Me.Label11.Location = New System.Drawing.Point(4, 3)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(331, 54)
        Me.Label11.TabIndex = 103
        Me.Label11.Text = "NAME"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'SoundTimeDisplay
        '
        Me.SoundTimeDisplay.BackColor = System.Drawing.Color.Black
        Me.SoundTimeDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SoundTimeDisplay.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SoundTimeDisplay.ForeColor = System.Drawing.Color.White
        Me.SoundTimeDisplay.Location = New System.Drawing.Point(17, 285)
        Me.SoundTimeDisplay.Name = "SoundTimeDisplay"
        Me.SoundTimeDisplay.ReadOnly = True
        Me.SoundTimeDisplay.Size = New System.Drawing.Size(64, 26)
        Me.SoundTimeDisplay.TabIndex = 115
        Me.SoundTimeDisplay.Text = "00:00"
        Me.SoundTimeDisplay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'SoundTime
        '
        Me.SoundTime.BackColor = System.Drawing.Color.Black
        Me.SoundTime.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SoundTime.Location = New System.Drawing.Point(81, 285)
        Me.SoundTime.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.SoundTime.Name = "SoundTime"
        Me.SoundTime.Size = New System.Drawing.Size(18, 26)
        Me.SoundTime.TabIndex = 114
        '
        'RemoveSoundButt
        '
        Me.RemoveSoundButt.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.RemoveSoundButt.BackgroundImage = Global.EmberLuna_OverlayCTRL.My.Resources.Resources.minus
        Me.RemoveSoundButt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.RemoveSoundButt.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.RemoveSoundButt.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RemoveSoundButt.ForeColor = System.Drawing.Color.White
        Me.RemoveSoundButt.Location = New System.Drawing.Point(141, 283)
        Me.RemoveSoundButt.Name = "RemoveSoundButt"
        Me.RemoveSoundButt.Size = New System.Drawing.Size(35, 30)
        Me.RemoveSoundButt.TabIndex = 112
        Me.RemoveSoundButt.UseVisualStyleBackColor = False
        '
        'PlaySoundButt
        '
        Me.PlaySoundButt.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.PlaySoundButt.BackgroundImage = Global.EmberLuna_OverlayCTRL.My.Resources.Resources.play
        Me.PlaySoundButt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PlaySoundButt.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.PlaySoundButt.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PlaySoundButt.ForeColor = System.Drawing.Color.White
        Me.PlaySoundButt.Location = New System.Drawing.Point(181, 283)
        Me.PlaySoundButt.Name = "PlaySoundButt"
        Me.PlaySoundButt.Size = New System.Drawing.Size(30, 30)
        Me.PlaySoundButt.TabIndex = 111
        Me.PlaySoundButt.UseVisualStyleBackColor = False
        '
        'TimerTimeDisplay
        '
        Me.TimerTimeDisplay.BackColor = System.Drawing.Color.Black
        Me.TimerTimeDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TimerTimeDisplay.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TimerTimeDisplay.ForeColor = System.Drawing.Color.White
        Me.TimerTimeDisplay.Location = New System.Drawing.Point(240, 135)
        Me.TimerTimeDisplay.Name = "TimerTimeDisplay"
        Me.TimerTimeDisplay.ReadOnly = True
        Me.TimerTimeDisplay.Size = New System.Drawing.Size(64, 26)
        Me.TimerTimeDisplay.TabIndex = 117
        Me.TimerTimeDisplay.Text = "00:00"
        Me.TimerTimeDisplay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TimerTime
        '
        Me.TimerTime.BackColor = System.Drawing.Color.Black
        Me.TimerTime.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TimerTime.Location = New System.Drawing.Point(304, 135)
        Me.TimerTime.Maximum = New Decimal(New Integer() {3599, 0, 0, 0})
        Me.TimerTime.Name = "TimerTime"
        Me.TimerTime.Size = New System.Drawing.Size(18, 26)
        Me.TimerTime.TabIndex = 116
        Me.TimerTime.Value = New Decimal(New Integer() {60, 0, 0, 0})
        '
        'PubPriBUTT
        '
        Me.PubPriBUTT.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.PubPriBUTT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PubPriBUTT.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PubPriBUTT.ForeColor = System.Drawing.Color.White
        Me.PubPriBUTT.Location = New System.Drawing.Point(228, 281)
        Me.PubPriBUTT.Name = "PubPriBUTT"
        Me.PubPriBUTT.Size = New System.Drawing.Size(107, 28)
        Me.PubPriBUTT.TabIndex = 118
        Me.PubPriBUTT.Text = "PRIVATE"
        Me.PubPriBUTT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'StopTimeDisplay
        '
        Me.StopTimeDisplay.BackColor = System.Drawing.Color.Black
        Me.StopTimeDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.StopTimeDisplay.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StopTimeDisplay.ForeColor = System.Drawing.Color.White
        Me.StopTimeDisplay.Location = New System.Drawing.Point(240, 187)
        Me.StopTimeDisplay.Name = "StopTimeDisplay"
        Me.StopTimeDisplay.ReadOnly = True
        Me.StopTimeDisplay.Size = New System.Drawing.Size(64, 26)
        Me.StopTimeDisplay.TabIndex = 121
        Me.StopTimeDisplay.Text = "00:00"
        Me.StopTimeDisplay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'StopTime
        '
        Me.StopTime.BackColor = System.Drawing.Color.Black
        Me.StopTime.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StopTime.Location = New System.Drawing.Point(304, 187)
        Me.StopTime.Maximum = New Decimal(New Integer() {3599, 0, 0, 0})
        Me.StopTime.Name = "StopTime"
        Me.StopTime.Size = New System.Drawing.Size(18, 26)
        Me.StopTime.TabIndex = 120
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(228, 171)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(107, 48)
        Me.Label1.TabIndex = 119
        Me.Label1.Text = "STOP AT"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'AddTimer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(31, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(31, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(340, 356)
        Me.Controls.Add(Me.StopTimeDisplay)
        Me.Controls.Add(Me.StopTime)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.PubPriBUTT)
        Me.Controls.Add(Me.TimerTimeDisplay)
        Me.Controls.Add(Me.TimerTime)
        Me.Controls.Add(Me.SoundTimeDisplay)
        Me.Controls.Add(Me.SoundTime)
        Me.Controls.Add(Me.RemoveSoundButt)
        Me.Controls.Add(Me.PlaySoundButt)
        Me.Controls.Add(Me.AddSoundButt)
        Me.Controls.Add(Me.SoundPicker)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.TimerSoundlist)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.TimerLabel)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.TimerName)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.AddBUTT)
        Me.Controls.Add(Me.TimerID)
        Me.Controls.Add(Me.TimerIDlabel)
        Me.Controls.Add(Me.Label3)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "AddTimer"
        Me.Text = "TIMER EDITOR"
        CType(Me.TimerID, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SoundTime, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TimerTime, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.StopTime, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents AddBUTT As Button
    Friend WithEvents TimerID As NumericUpDown
    Friend WithEvents TimerIDlabel As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents RemoveSoundButt As Button
    Friend WithEvents PlaySoundButt As Button
    Friend WithEvents AddSoundButt As Button
    Friend WithEvents SoundPicker As ComboBox
    Friend WithEvents Label8 As Label
    Friend WithEvents TimerSoundlist As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents TimerLabel As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents TimerName As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents SoundTimeDisplay As TextBox
    Friend WithEvents SoundTime As NumericUpDown
    Friend WithEvents TimerTimeDisplay As TextBox
    Friend WithEvents TimerTime As NumericUpDown
    Friend WithEvents PubPriBUTT As Label
    Friend WithEvents StopTimeDisplay As TextBox
    Friend WithEvents StopTime As NumericUpDown
    Friend WithEvents Label1 As Label
End Class
