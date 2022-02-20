<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AddSound
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
        Me.SoundName = New System.Windows.Forms.TextBox()
        Me.CounterLabel0 = New System.Windows.Forms.Label()
        Me.SoundTitle = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ColorPicker = New System.Windows.Forms.Button()
        Me.SoundMute = New System.Windows.Forms.CheckBox()
        Me.SoundPublic = New System.Windows.Forms.CheckBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.SoundButtPreview = New System.Windows.Forms.Button()
        Me.SoundRando = New System.Windows.Forms.CheckBox()
        Me.ImagePicker = New System.Windows.Forms.Button()
        Me.GoButt = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.SoundListBox = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.SoundPicker = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.FilterSoundsButt = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'SoundName
        '
        Me.SoundName.BackColor = System.Drawing.Color.Black
        Me.SoundName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SoundName.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SoundName.ForeColor = System.Drawing.Color.White
        Me.SoundName.Location = New System.Drawing.Point(17, 21)
        Me.SoundName.Name = "SoundName"
        Me.SoundName.Size = New System.Drawing.Size(194, 27)
        Me.SoundName.TabIndex = 75
        Me.SoundName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.SoundName.WordWrap = False
        '
        'CounterLabel0
        '
        Me.CounterLabel0.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.CounterLabel0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.CounterLabel0.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CounterLabel0.ForeColor = System.Drawing.Color.White
        Me.CounterLabel0.Location = New System.Drawing.Point(4, 4)
        Me.CounterLabel0.Name = "CounterLabel0"
        Me.CounterLabel0.Size = New System.Drawing.Size(219, 54)
        Me.CounterLabel0.TabIndex = 76
        Me.CounterLabel0.Text = "NAME"
        Me.CounterLabel0.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'SoundTitle
        '
        Me.SoundTitle.BackColor = System.Drawing.Color.Black
        Me.SoundTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SoundTitle.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SoundTitle.ForeColor = System.Drawing.Color.White
        Me.SoundTitle.Location = New System.Drawing.Point(17, 79)
        Me.SoundTitle.Name = "SoundTitle"
        Me.SoundTitle.Size = New System.Drawing.Size(194, 27)
        Me.SoundTitle.TabIndex = 77
        Me.SoundTitle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.SoundTitle.WordWrap = False
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label1.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(4, 62)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(219, 54)
        Me.Label1.TabIndex = 78
        Me.Label1.Text = "TITLE"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'ColorPicker
        '
        Me.ColorPicker.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.ColorPicker.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ColorPicker.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ColorPicker.ForeColor = System.Drawing.Color.White
        Me.ColorPicker.Location = New System.Drawing.Point(229, 120)
        Me.ColorPicker.Name = "ColorPicker"
        Me.ColorPicker.Size = New System.Drawing.Size(119, 30)
        Me.ColorPicker.TabIndex = 79
        Me.ColorPicker.Text = "COLOR"
        Me.ColorPicker.UseVisualStyleBackColor = False
        '
        'SoundMute
        '
        Me.SoundMute.AutoSize = True
        Me.SoundMute.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.SoundMute.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SoundMute.Location = New System.Drawing.Point(242, 50)
        Me.SoundMute.Name = "SoundMute"
        Me.SoundMute.Size = New System.Drawing.Size(101, 22)
        Me.SoundMute.TabIndex = 88
        Me.SoundMute.Text = "Mute Music"
        Me.SoundMute.UseVisualStyleBackColor = False
        '
        'SoundPublic
        '
        Me.SoundPublic.AutoSize = True
        Me.SoundPublic.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.SoundPublic.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SoundPublic.Location = New System.Drawing.Point(242, 19)
        Me.SoundPublic.Name = "SoundPublic"
        Me.SoundPublic.Size = New System.Drawing.Size(65, 22)
        Me.SoundPublic.TabIndex = 87
        Me.SoundPublic.Text = "Public"
        Me.SoundPublic.UseVisualStyleBackColor = False
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label7.ForeColor = System.Drawing.Color.White
        Me.Label7.Location = New System.Drawing.Point(229, 4)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(119, 113)
        Me.Label7.TabIndex = 86
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'SoundButtPreview
        '
        Me.SoundButtPreview.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.SoundButtPreview.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.SoundButtPreview.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SoundButtPreview.ForeColor = System.Drawing.Color.White
        Me.SoundButtPreview.Location = New System.Drawing.Point(243, 218)
        Me.SoundButtPreview.Name = "SoundButtPreview"
        Me.SoundButtPreview.Size = New System.Drawing.Size(91, 67)
        Me.SoundButtPreview.TabIndex = 89
        Me.SoundButtPreview.UseVisualStyleBackColor = False
        '
        'SoundRando
        '
        Me.SoundRando.AutoSize = True
        Me.SoundRando.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.SoundRando.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SoundRando.Location = New System.Drawing.Point(242, 81)
        Me.SoundRando.Name = "SoundRando"
        Me.SoundRando.Size = New System.Drawing.Size(96, 22)
        Me.SoundRando.TabIndex = 90
        Me.SoundRando.Text = "Randomize"
        Me.SoundRando.UseVisualStyleBackColor = False
        '
        'ImagePicker
        '
        Me.ImagePicker.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.ImagePicker.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ImagePicker.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ImagePicker.ForeColor = System.Drawing.Color.White
        Me.ImagePicker.Location = New System.Drawing.Point(229, 156)
        Me.ImagePicker.Name = "ImagePicker"
        Me.ImagePicker.Size = New System.Drawing.Size(119, 30)
        Me.ImagePicker.TabIndex = 91
        Me.ImagePicker.Text = "IMAGE"
        Me.ImagePicker.UseVisualStyleBackColor = False
        '
        'GoButt
        '
        Me.GoButt.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.GoButt.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.GoButt.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GoButt.ForeColor = System.Drawing.Color.White
        Me.GoButt.Location = New System.Drawing.Point(229, 295)
        Me.GoButt.Name = "GoButt"
        Me.GoButt.Size = New System.Drawing.Size(119, 67)
        Me.GoButt.TabIndex = 92
        Me.GoButt.Text = "OK"
        Me.GoButt.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(229, 191)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(119, 100)
        Me.Label2.TabIndex = 93
        Me.Label2.Text = "PREVIEW"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'SoundListBox
        '
        Me.SoundListBox.BackColor = System.Drawing.Color.Black
        Me.SoundListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SoundListBox.Font = New System.Drawing.Font("Calibri", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SoundListBox.ForeColor = System.Drawing.Color.White
        Me.SoundListBox.Location = New System.Drawing.Point(17, 139)
        Me.SoundListBox.Multiline = True
        Me.SoundListBox.Name = "SoundListBox"
        Me.SoundListBox.ReadOnly = True
        Me.SoundListBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.SoundListBox.Size = New System.Drawing.Size(194, 122)
        Me.SoundListBox.TabIndex = 94
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label3.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(4, 121)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(219, 151)
        Me.Label3.TabIndex = 95
        Me.Label3.Text = "SOUND LIST"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'SoundPicker
        '
        Me.SoundPicker.BackColor = System.Drawing.Color.Black
        Me.SoundPicker.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.SoundPicker.Font = New System.Drawing.Font("Calibri", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SoundPicker.ForeColor = System.Drawing.Color.White
        Me.SoundPicker.FormattingEnabled = True
        Me.SoundPicker.Location = New System.Drawing.Point(17, 290)
        Me.SoundPicker.Name = "SoundPicker"
        Me.SoundPicker.Size = New System.Drawing.Size(191, 21)
        Me.SoundPicker.TabIndex = 97
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(4, 276)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(219, 86)
        Me.Label5.TabIndex = 96
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Button3
        '
        Me.Button3.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Button3.BackgroundImage = Global.EmberLuna_OverlayCTRL.My.Resources.Resources.plus
        Me.Button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button3.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button3.ForeColor = System.Drawing.Color.White
        Me.Button3.Location = New System.Drawing.Point(17, 321)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(32, 30)
        Me.Button3.TabIndex = 98
        Me.Button3.UseVisualStyleBackColor = False
        '
        'Button5
        '
        Me.Button5.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Button5.BackgroundImage = Global.EmberLuna_OverlayCTRL.My.Resources.Resources.minus
        Me.Button5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button5.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button5.ForeColor = System.Drawing.Color.White
        Me.Button5.Location = New System.Drawing.Point(55, 321)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(35, 30)
        Me.Button5.TabIndex = 100
        Me.Button5.UseVisualStyleBackColor = False
        '
        'FilterSoundsButt
        '
        Me.FilterSoundsButt.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.FilterSoundsButt.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.FilterSoundsButt.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FilterSoundsButt.ForeColor = System.Drawing.Color.White
        Me.FilterSoundsButt.Location = New System.Drawing.Point(132, 321)
        Me.FilterSoundsButt.Name = "FilterSoundsButt"
        Me.FilterSoundsButt.Size = New System.Drawing.Size(76, 30)
        Me.FilterSoundsButt.TabIndex = 101
        Me.FilterSoundsButt.Text = "FILTER"
        Me.FilterSoundsButt.UseVisualStyleBackColor = False
        '
        'Button4
        '
        Me.Button4.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Button4.BackgroundImage = Global.EmberLuna_OverlayCTRL.My.Resources.Resources.play
        Me.Button4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button4.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button4.ForeColor = System.Drawing.Color.White
        Me.Button4.Location = New System.Drawing.Point(96, 321)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(30, 30)
        Me.Button4.TabIndex = 99
        Me.Button4.UseVisualStyleBackColor = False
        '
        'AddSound
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(31, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(31, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(353, 368)
        Me.Controls.Add(Me.FilterSoundsButt)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.SoundPicker)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.SoundListBox)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.GoButt)
        Me.Controls.Add(Me.ImagePicker)
        Me.Controls.Add(Me.SoundRando)
        Me.Controls.Add(Me.SoundButtPreview)
        Me.Controls.Add(Me.SoundMute)
        Me.Controls.Add(Me.SoundPublic)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.ColorPicker)
        Me.Controls.Add(Me.SoundTitle)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.SoundName)
        Me.Controls.Add(Me.CounterLabel0)
        Me.Controls.Add(Me.Label2)
        Me.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.White
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AddSound"
        Me.Text = "EDIT SOUND BOARD"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents SoundName As TextBox
    Friend WithEvents CounterLabel0 As Label
    Friend WithEvents SoundTitle As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents ColorPicker As Button
    Friend WithEvents SoundMute As CheckBox
    Friend WithEvents SoundPublic As CheckBox
    Friend WithEvents Label7 As Label
    Friend WithEvents SoundButtPreview As Button
    Friend WithEvents SoundRando As CheckBox
    Friend WithEvents ImagePicker As Button
    Friend WithEvents GoButt As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents SoundListBox As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents SoundPicker As ComboBox
    Friend WithEvents Label5 As Label
    Friend WithEvents Button3 As Button
    Friend WithEvents Button4 As Button
    Friend WithEvents Button5 As Button
    Friend WithEvents FilterSoundsButt As Button
End Class
