<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OBSMusicPlayer
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
        Me.CurrentSong = New System.Windows.Forms.TextBox()
        Me.RPBUTT = New System.Windows.Forms.PictureBox()
        Me.SKIPFBUTT = New System.Windows.Forms.PictureBox()
        Me.SKIPBBUTT = New System.Windows.Forms.PictureBox()
        Me.PAUSEBUTT = New System.Windows.Forms.PictureBox()
        Me.PLAYBUTT = New System.Windows.Forms.PictureBox()
        CType(Me.RPBUTT, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SKIPFBUTT, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SKIPBBUTT, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PAUSEBUTT, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PLAYBUTT, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'CurrentSong
        '
        Me.CurrentSong.BackColor = System.Drawing.Color.Black
        Me.CurrentSong.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.CurrentSong.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CurrentSong.ForeColor = System.Drawing.Color.White
        Me.CurrentSong.Location = New System.Drawing.Point(12, 68)
        Me.CurrentSong.Name = "CurrentSong"
        Me.CurrentSong.ReadOnly = True
        Me.CurrentSong.Size = New System.Drawing.Size(300, 23)
        Me.CurrentSong.TabIndex = 8
        Me.CurrentSong.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'RPBUTT
        '
        Me.RPBUTT.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.RPBUTT.BackgroundImage = Global.EmberLuna_OverlayCTRL.My.Resources.Resources.replay
        Me.RPBUTT.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.RPBUTT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.RPBUTT.Location = New System.Drawing.Point(256, 12)
        Me.RPBUTT.Name = "RPBUTT"
        Me.RPBUTT.Size = New System.Drawing.Size(55, 50)
        Me.RPBUTT.TabIndex = 9
        Me.RPBUTT.TabStop = False
        '
        'SKIPFBUTT
        '
        Me.SKIPFBUTT.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.SKIPFBUTT.BackgroundImage = Global.EmberLuna_OverlayCTRL.My.Resources.Resources.skipf
        Me.SKIPFBUTT.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.SKIPFBUTT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SKIPFBUTT.Location = New System.Drawing.Point(195, 12)
        Me.SKIPFBUTT.Name = "SKIPFBUTT"
        Me.SKIPFBUTT.Size = New System.Drawing.Size(55, 50)
        Me.SKIPFBUTT.TabIndex = 4
        Me.SKIPFBUTT.TabStop = False
        '
        'SKIPBBUTT
        '
        Me.SKIPBBUTT.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.SKIPBBUTT.BackgroundImage = Global.EmberLuna_OverlayCTRL.My.Resources.Resources.skipb
        Me.SKIPBBUTT.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.SKIPBBUTT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SKIPBBUTT.Location = New System.Drawing.Point(134, 12)
        Me.SKIPBBUTT.Name = "SKIPBBUTT"
        Me.SKIPBBUTT.Size = New System.Drawing.Size(55, 50)
        Me.SKIPBBUTT.TabIndex = 3
        Me.SKIPBBUTT.TabStop = False
        '
        'PAUSEBUTT
        '
        Me.PAUSEBUTT.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.PAUSEBUTT.BackgroundImage = Global.EmberLuna_OverlayCTRL.My.Resources.Resources.pause
        Me.PAUSEBUTT.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PAUSEBUTT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PAUSEBUTT.Image = Global.EmberLuna_OverlayCTRL.My.Resources.Resources.pause
        Me.PAUSEBUTT.Location = New System.Drawing.Point(73, 12)
        Me.PAUSEBUTT.Name = "PAUSEBUTT"
        Me.PAUSEBUTT.Size = New System.Drawing.Size(55, 50)
        Me.PAUSEBUTT.TabIndex = 1
        Me.PAUSEBUTT.TabStop = False
        '
        'PLAYBUTT
        '
        Me.PLAYBUTT.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.PLAYBUTT.BackgroundImage = Global.EmberLuna_OverlayCTRL.My.Resources.Resources.play
        Me.PLAYBUTT.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PLAYBUTT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PLAYBUTT.Image = Global.EmberLuna_OverlayCTRL.My.Resources.Resources.play
        Me.PLAYBUTT.Location = New System.Drawing.Point(12, 12)
        Me.PLAYBUTT.Name = "PLAYBUTT"
        Me.PLAYBUTT.Size = New System.Drawing.Size(55, 50)
        Me.PLAYBUTT.TabIndex = 0
        Me.PLAYBUTT.TabStop = False
        '
        'OBSMusicPlayer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(31, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(31, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(324, 101)
        Me.Controls.Add(Me.RPBUTT)
        Me.Controls.Add(Me.CurrentSong)
        Me.Controls.Add(Me.SKIPFBUTT)
        Me.Controls.Add(Me.SKIPBBUTT)
        Me.Controls.Add(Me.PAUSEBUTT)
        Me.Controls.Add(Me.PLAYBUTT)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "OBSMusicPlayer"
        Me.Text = "OBSMusicPlayer"
        CType(Me.RPBUTT, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SKIPFBUTT, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SKIPBBUTT, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PAUSEBUTT, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PLAYBUTT, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents PLAYBUTT As PictureBox
    Friend WithEvents PAUSEBUTT As PictureBox
    Friend WithEvents SKIPBBUTT As PictureBox
    Friend WithEvents SKIPFBUTT As PictureBox
    Friend WithEvents CurrentSong As TextBox
    Friend WithEvents RPBUTT As PictureBox
End Class
