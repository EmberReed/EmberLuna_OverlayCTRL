<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainWindow
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
        Me.IPaddress1 = New System.Windows.Forms.TextBox()
        Me.IPaddress2 = New System.Windows.Forms.TextBox()
        Me.ChatBox = New System.Windows.Forms.TextBox()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.DatastreamBox = New System.Windows.Forms.TextBox()
        Me.SOUND_BOARD = New System.Windows.Forms.Button()
        Me.SCENE_SELECTOR = New System.Windows.Forms.Button()
        Me.CHARACTERS = New System.Windows.Forms.Button()
        Me.TimersButt = New System.Windows.Forms.Button()
        Me.ChatBUTT = New System.Windows.Forms.Button()
        Me.CountersButt = New System.Windows.Forms.Button()
        Me.CHANNEL_POINTS = New System.Windows.Forms.Button()
        Me.MUSIC_PLAYER = New System.Windows.Forms.Button()
        Me.EventBox = New System.Windows.Forms.TextBox()
        Me.NextMessage = New System.Windows.Forms.Button()
        Me.NextEvent = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.Button8 = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.Button9 = New System.Windows.Forms.Button()
        Me.Button10 = New System.Windows.Forms.Button()
        Me.Button11 = New System.Windows.Forms.Button()
        Me.Button12 = New System.Windows.Forms.Button()
        Me.Button13 = New System.Windows.Forms.Button()
        Me.IRCdisplay = New System.Windows.Forms.Label()
        Me.OBSdisplay = New System.Windows.Forms.Label()
        Me.PUBSUBdisplay = New System.Windows.Forms.Label()
        Me.APIdisplay = New System.Windows.Forms.Label()
        Me.CTRL2display = New System.Windows.Forms.Label()
        Me.CTRL1display = New System.Windows.Forms.Label()
        Me.AUTHdisplay = New System.Windows.Forms.Label()
        Me.EventHighlighter = New System.Windows.Forms.PictureBox()
        Me.ChatHighlighter = New System.Windows.Forms.PictureBox()
        CType(Me.EventHighlighter, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ChatHighlighter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'IPaddress1
        '
        Me.IPaddress1.BackColor = System.Drawing.Color.Black
        Me.IPaddress1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.IPaddress1.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.IPaddress1.ForeColor = System.Drawing.Color.White
        Me.IPaddress1.Location = New System.Drawing.Point(457, 4)
        Me.IPaddress1.Name = "IPaddress1"
        Me.IPaddress1.Size = New System.Drawing.Size(208, 22)
        Me.IPaddress1.TabIndex = 1
        Me.IPaddress1.Text = "192.168.1.191"
        Me.IPaddress1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'IPaddress2
        '
        Me.IPaddress2.BackColor = System.Drawing.Color.Black
        Me.IPaddress2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.IPaddress2.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.IPaddress2.ForeColor = System.Drawing.Color.White
        Me.IPaddress2.Location = New System.Drawing.Point(457, 30)
        Me.IPaddress2.Name = "IPaddress2"
        Me.IPaddress2.Size = New System.Drawing.Size(208, 22)
        Me.IPaddress2.TabIndex = 3
        Me.IPaddress2.Text = "192.168.1.193"
        Me.IPaddress2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'ChatBox
        '
        Me.ChatBox.BackColor = System.Drawing.Color.Black
        Me.ChatBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.ChatBox.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChatBox.ForeColor = System.Drawing.Color.White
        Me.ChatBox.Location = New System.Drawing.Point(17, 14)
        Me.ChatBox.Multiline = True
        Me.ChatBox.Name = "ChatBox"
        Me.ChatBox.ReadOnly = True
        Me.ChatBox.Size = New System.Drawing.Size(375, 79)
        Me.ChatBox.TabIndex = 6
        Me.ChatBox.Text = "Chat text will appear here"
        '
        'TextBox3
        '
        Me.TextBox3.BackColor = System.Drawing.Color.Black
        Me.TextBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox3.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox3.ForeColor = System.Drawing.Color.White
        Me.TextBox3.Location = New System.Drawing.Point(457, 475)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(208, 22)
        Me.TextBox3.TabIndex = 8
        Me.TextBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'DatastreamBox
        '
        Me.DatastreamBox.BackColor = System.Drawing.Color.Black
        Me.DatastreamBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.DatastreamBox.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DatastreamBox.ForeColor = System.Drawing.Color.White
        Me.DatastreamBox.Location = New System.Drawing.Point(457, 56)
        Me.DatastreamBox.Multiline = True
        Me.DatastreamBox.Name = "DatastreamBox"
        Me.DatastreamBox.ReadOnly = True
        Me.DatastreamBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.DatastreamBox.Size = New System.Drawing.Size(208, 415)
        Me.DatastreamBox.TabIndex = 9
        Me.DatastreamBox.Text = "Chat text will appear here"
        '
        'SOUND_BOARD
        '
        Me.SOUND_BOARD.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.SOUND_BOARD.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.SOUND_BOARD.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SOUND_BOARD.ForeColor = System.Drawing.Color.White
        Me.SOUND_BOARD.Location = New System.Drawing.Point(5, 216)
        Me.SOUND_BOARD.Name = "SOUND_BOARD"
        Me.SOUND_BOARD.Size = New System.Drawing.Size(107, 71)
        Me.SOUND_BOARD.TabIndex = 13
        Me.SOUND_BOARD.Text = "SOUND BOARD"
        Me.SOUND_BOARD.UseVisualStyleBackColor = False
        '
        'SCENE_SELECTOR
        '
        Me.SCENE_SELECTOR.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.SCENE_SELECTOR.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.SCENE_SELECTOR.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SCENE_SELECTOR.ForeColor = System.Drawing.Color.White
        Me.SCENE_SELECTOR.Location = New System.Drawing.Point(118, 216)
        Me.SCENE_SELECTOR.Name = "SCENE_SELECTOR"
        Me.SCENE_SELECTOR.Size = New System.Drawing.Size(107, 71)
        Me.SCENE_SELECTOR.TabIndex = 15
        Me.SCENE_SELECTOR.Text = "SCENE SELECTOR"
        Me.SCENE_SELECTOR.UseVisualStyleBackColor = False
        '
        'CHARACTERS
        '
        Me.CHARACTERS.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.CHARACTERS.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CHARACTERS.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CHARACTERS.ForeColor = System.Drawing.Color.White
        Me.CHARACTERS.Location = New System.Drawing.Point(344, 216)
        Me.CHARACTERS.Name = "CHARACTERS"
        Me.CHARACTERS.Size = New System.Drawing.Size(107, 71)
        Me.CHARACTERS.TabIndex = 17
        Me.CHARACTERS.Text = "CHARACTER CONTROLS"
        Me.CHARACTERS.UseVisualStyleBackColor = False
        '
        'TimersButt
        '
        Me.TimersButt.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.TimersButt.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.TimersButt.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TimersButt.ForeColor = System.Drawing.Color.White
        Me.TimersButt.Location = New System.Drawing.Point(231, 216)
        Me.TimersButt.Name = "TimersButt"
        Me.TimersButt.Size = New System.Drawing.Size(107, 71)
        Me.TimersButt.TabIndex = 16
        Me.TimersButt.Text = "TIMERS"
        Me.TimersButt.UseVisualStyleBackColor = False
        '
        'ChatBUTT
        '
        Me.ChatBUTT.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.ChatBUTT.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ChatBUTT.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChatBUTT.ForeColor = System.Drawing.Color.White
        Me.ChatBUTT.Location = New System.Drawing.Point(344, 293)
        Me.ChatBUTT.Name = "ChatBUTT"
        Me.ChatBUTT.Size = New System.Drawing.Size(107, 71)
        Me.ChatBUTT.TabIndex = 21
        Me.ChatBUTT.Text = "CHAT MANAGER"
        Me.ChatBUTT.UseVisualStyleBackColor = False
        '
        'CountersButt
        '
        Me.CountersButt.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.CountersButt.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CountersButt.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CountersButt.ForeColor = System.Drawing.Color.White
        Me.CountersButt.Location = New System.Drawing.Point(231, 293)
        Me.CountersButt.Name = "CountersButt"
        Me.CountersButt.Size = New System.Drawing.Size(107, 71)
        Me.CountersButt.TabIndex = 20
        Me.CountersButt.Text = "COUNTERS"
        Me.CountersButt.UseVisualStyleBackColor = False
        '
        'CHANNEL_POINTS
        '
        Me.CHANNEL_POINTS.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.CHANNEL_POINTS.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CHANNEL_POINTS.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CHANNEL_POINTS.ForeColor = System.Drawing.Color.White
        Me.CHANNEL_POINTS.Location = New System.Drawing.Point(118, 293)
        Me.CHANNEL_POINTS.Name = "CHANNEL_POINTS"
        Me.CHANNEL_POINTS.Size = New System.Drawing.Size(107, 71)
        Me.CHANNEL_POINTS.TabIndex = 19
        Me.CHANNEL_POINTS.Text = "CHANNEL POINTS"
        Me.CHANNEL_POINTS.UseVisualStyleBackColor = False
        '
        'MUSIC_PLAYER
        '
        Me.MUSIC_PLAYER.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.MUSIC_PLAYER.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.MUSIC_PLAYER.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MUSIC_PLAYER.ForeColor = System.Drawing.Color.White
        Me.MUSIC_PLAYER.Location = New System.Drawing.Point(5, 293)
        Me.MUSIC_PLAYER.Name = "MUSIC_PLAYER"
        Me.MUSIC_PLAYER.Size = New System.Drawing.Size(107, 71)
        Me.MUSIC_PLAYER.TabIndex = 18
        Me.MUSIC_PLAYER.Text = "MUSIC PLAYER"
        Me.MUSIC_PLAYER.UseVisualStyleBackColor = False
        '
        'EventBox
        '
        Me.EventBox.BackColor = System.Drawing.Color.Black
        Me.EventBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.EventBox.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.EventBox.ForeColor = System.Drawing.Color.White
        Me.EventBox.Location = New System.Drawing.Point(17, 121)
        Me.EventBox.Multiline = True
        Me.EventBox.Name = "EventBox"
        Me.EventBox.ReadOnly = True
        Me.EventBox.Size = New System.Drawing.Size(375, 79)
        Me.EventBox.TabIndex = 23
        Me.EventBox.Text = "Event text will appear here"
        '
        'NextMessage
        '
        Me.NextMessage.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.NextMessage.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.NextMessage.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NextMessage.ForeColor = System.Drawing.Color.White
        Me.NextMessage.Location = New System.Drawing.Point(341, 57)
        Me.NextMessage.Name = "NextMessage"
        Me.NextMessage.Size = New System.Drawing.Size(49, 33)
        Me.NextMessage.TabIndex = 25
        Me.NextMessage.Text = "OK"
        Me.NextMessage.UseVisualStyleBackColor = False
        Me.NextMessage.Visible = False
        '
        'NextEvent
        '
        Me.NextEvent.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.NextEvent.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.NextEvent.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NextEvent.ForeColor = System.Drawing.Color.White
        Me.NextEvent.Location = New System.Drawing.Point(341, 164)
        Me.NextEvent.Name = "NextEvent"
        Me.NextEvent.Size = New System.Drawing.Size(49, 33)
        Me.NextEvent.TabIndex = 26
        Me.NextEvent.Text = "OK"
        Me.NextEvent.UseVisualStyleBackColor = False
        Me.NextEvent.Visible = False
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button2.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.ForeColor = System.Drawing.Color.White
        Me.Button2.Location = New System.Drawing.Point(5, 447)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(70, 50)
        Me.Button2.TabIndex = 28
        Me.Button2.Text = "EVENT TESTER"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'Button4
        '
        Me.Button4.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button4.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button4.ForeColor = System.Drawing.Color.White
        Me.Button4.Location = New System.Drawing.Point(80, 447)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(70, 50)
        Me.Button4.TabIndex = 29
        Me.Button4.Text = "PROGRAM SETTINGS"
        Me.Button4.UseVisualStyleBackColor = False
        '
        'Button6
        '
        Me.Button6.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button6.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button6.ForeColor = System.Drawing.Color.White
        Me.Button6.Location = New System.Drawing.Point(155, 447)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(70, 50)
        Me.Button6.TabIndex = 30
        Me.Button6.Text = "TEST API"
        Me.Button6.UseVisualStyleBackColor = False
        '
        'Button8
        '
        Me.Button8.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Button8.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button8.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button8.ForeColor = System.Drawing.Color.White
        Me.Button8.Location = New System.Drawing.Point(231, 447)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(70, 50)
        Me.Button8.TabIndex = 31
        Me.Button8.Text = "BAN BOTS"
        Me.Button8.UseVisualStyleBackColor = False
        '
        'Button5
        '
        Me.Button5.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button5.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button5.ForeColor = System.Drawing.Color.White
        Me.Button5.Location = New System.Drawing.Point(344, 370)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(107, 71)
        Me.Button5.TabIndex = 35
        Me.Button5.UseVisualStyleBackColor = False
        '
        'Button9
        '
        Me.Button9.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Button9.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button9.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button9.ForeColor = System.Drawing.Color.White
        Me.Button9.Location = New System.Drawing.Point(231, 370)
        Me.Button9.Name = "Button9"
        Me.Button9.Size = New System.Drawing.Size(107, 71)
        Me.Button9.TabIndex = 34
        Me.Button9.UseVisualStyleBackColor = False
        '
        'Button10
        '
        Me.Button10.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Button10.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button10.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button10.ForeColor = System.Drawing.Color.White
        Me.Button10.Location = New System.Drawing.Point(118, 370)
        Me.Button10.Name = "Button10"
        Me.Button10.Size = New System.Drawing.Size(107, 71)
        Me.Button10.TabIndex = 33
        Me.Button10.UseVisualStyleBackColor = False
        '
        'Button11
        '
        Me.Button11.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Button11.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button11.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button11.ForeColor = System.Drawing.Color.White
        Me.Button11.Location = New System.Drawing.Point(5, 370)
        Me.Button11.Name = "Button11"
        Me.Button11.Size = New System.Drawing.Size(107, 71)
        Me.Button11.TabIndex = 32
        Me.Button11.Text = "VOLUME CONTROL"
        Me.Button11.UseVisualStyleBackColor = False
        '
        'Button12
        '
        Me.Button12.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Button12.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button12.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button12.ForeColor = System.Drawing.Color.White
        Me.Button12.Location = New System.Drawing.Point(306, 447)
        Me.Button12.Name = "Button12"
        Me.Button12.Size = New System.Drawing.Size(70, 50)
        Me.Button12.TabIndex = 36
        Me.Button12.Text = "REFRESH TOKEN"
        Me.Button12.UseVisualStyleBackColor = False
        '
        'Button13
        '
        Me.Button13.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Button13.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button13.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button13.ForeColor = System.Drawing.Color.White
        Me.Button13.Location = New System.Drawing.Point(381, 447)
        Me.Button13.Name = "Button13"
        Me.Button13.Size = New System.Drawing.Size(70, 50)
        Me.Button13.TabIndex = 37
        Me.Button13.Text = "START STREAM"
        Me.Button13.UseVisualStyleBackColor = False
        '
        'IRCdisplay
        '
        Me.IRCdisplay.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.IRCdisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.IRCdisplay.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.IRCdisplay.ForeColor = System.Drawing.Color.White
        Me.IRCdisplay.Location = New System.Drawing.Point(411, 108)
        Me.IRCdisplay.Name = "IRCdisplay"
        Me.IRCdisplay.Size = New System.Drawing.Size(40, 22)
        Me.IRCdisplay.TabIndex = 38
        Me.IRCdisplay.Text = "IRC"
        Me.IRCdisplay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'OBSdisplay
        '
        Me.OBSdisplay.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.OBSdisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.OBSdisplay.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OBSdisplay.ForeColor = System.Drawing.Color.White
        Me.OBSdisplay.Location = New System.Drawing.Point(411, 56)
        Me.OBSdisplay.Name = "OBSdisplay"
        Me.OBSdisplay.Size = New System.Drawing.Size(40, 22)
        Me.OBSdisplay.TabIndex = 39
        Me.OBSdisplay.Text = "OBS"
        Me.OBSdisplay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PUBSUBdisplay
        '
        Me.PUBSUBdisplay.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.PUBSUBdisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PUBSUBdisplay.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PUBSUBdisplay.ForeColor = System.Drawing.Color.White
        Me.PUBSUBdisplay.Location = New System.Drawing.Point(411, 134)
        Me.PUBSUBdisplay.Name = "PUBSUBdisplay"
        Me.PUBSUBdisplay.Size = New System.Drawing.Size(40, 22)
        Me.PUBSUBdisplay.TabIndex = 40
        Me.PUBSUBdisplay.Text = "API"
        Me.PUBSUBdisplay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'APIdisplay
        '
        Me.APIdisplay.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.APIdisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.APIdisplay.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.APIdisplay.ForeColor = System.Drawing.Color.White
        Me.APIdisplay.Location = New System.Drawing.Point(411, 161)
        Me.APIdisplay.Name = "APIdisplay"
        Me.APIdisplay.Size = New System.Drawing.Size(40, 22)
        Me.APIdisplay.TabIndex = 41
        Me.APIdisplay.Text = "PUB"
        Me.APIdisplay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'CTRL2display
        '
        Me.CTRL2display.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.CTRL2display.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.CTRL2display.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CTRL2display.ForeColor = System.Drawing.Color.White
        Me.CTRL2display.Location = New System.Drawing.Point(411, 30)
        Me.CTRL2display.Name = "CTRL2display"
        Me.CTRL2display.Size = New System.Drawing.Size(40, 22)
        Me.CTRL2display.TabIndex = 44
        Me.CTRL2display.Text = "CTRL2"
        Me.CTRL2display.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'CTRL1display
        '
        Me.CTRL1display.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.CTRL1display.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.CTRL1display.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CTRL1display.ForeColor = System.Drawing.Color.White
        Me.CTRL1display.Location = New System.Drawing.Point(411, 4)
        Me.CTRL1display.Name = "CTRL1display"
        Me.CTRL1display.Size = New System.Drawing.Size(40, 22)
        Me.CTRL1display.TabIndex = 43
        Me.CTRL1display.Text = "CTRL1"
        Me.CTRL1display.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'AUTHdisplay
        '
        Me.AUTHdisplay.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.AUTHdisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.AUTHdisplay.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AUTHdisplay.ForeColor = System.Drawing.Color.White
        Me.AUTHdisplay.Location = New System.Drawing.Point(411, 82)
        Me.AUTHdisplay.Name = "AUTHdisplay"
        Me.AUTHdisplay.Size = New System.Drawing.Size(40, 22)
        Me.AUTHdisplay.TabIndex = 42
        Me.AUTHdisplay.Text = "AUTH"
        Me.AUTHdisplay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'EventHighlighter
        '
        Me.EventHighlighter.BackColor = System.Drawing.Color.Black
        Me.EventHighlighter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.EventHighlighter.Location = New System.Drawing.Point(4, 110)
        Me.EventHighlighter.Name = "EventHighlighter"
        Me.EventHighlighter.Size = New System.Drawing.Size(401, 101)
        Me.EventHighlighter.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.EventHighlighter.TabIndex = 24
        Me.EventHighlighter.TabStop = False
        '
        'ChatHighlighter
        '
        Me.ChatHighlighter.BackColor = System.Drawing.Color.Black
        Me.ChatHighlighter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ChatHighlighter.InitialImage = Nothing
        Me.ChatHighlighter.Location = New System.Drawing.Point(4, 4)
        Me.ChatHighlighter.Name = "ChatHighlighter"
        Me.ChatHighlighter.Size = New System.Drawing.Size(401, 101)
        Me.ChatHighlighter.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.ChatHighlighter.TabIndex = 22
        Me.ChatHighlighter.TabStop = False
        '
        'MainWindow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(31, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(31, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(671, 502)
        Me.Controls.Add(Me.CTRL2display)
        Me.Controls.Add(Me.CTRL1display)
        Me.Controls.Add(Me.AUTHdisplay)
        Me.Controls.Add(Me.APIdisplay)
        Me.Controls.Add(Me.PUBSUBdisplay)
        Me.Controls.Add(Me.OBSdisplay)
        Me.Controls.Add(Me.IRCdisplay)
        Me.Controls.Add(Me.Button13)
        Me.Controls.Add(Me.Button12)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.Button9)
        Me.Controls.Add(Me.Button10)
        Me.Controls.Add(Me.Button11)
        Me.Controls.Add(Me.Button8)
        Me.Controls.Add(Me.Button6)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.NextEvent)
        Me.Controls.Add(Me.NextMessage)
        Me.Controls.Add(Me.EventBox)
        Me.Controls.Add(Me.EventHighlighter)
        Me.Controls.Add(Me.ChatBUTT)
        Me.Controls.Add(Me.CountersButt)
        Me.Controls.Add(Me.CHANNEL_POINTS)
        Me.Controls.Add(Me.MUSIC_PLAYER)
        Me.Controls.Add(Me.CHARACTERS)
        Me.Controls.Add(Me.TimersButt)
        Me.Controls.Add(Me.SCENE_SELECTOR)
        Me.Controls.Add(Me.SOUND_BOARD)
        Me.Controls.Add(Me.DatastreamBox)
        Me.Controls.Add(Me.TextBox3)
        Me.Controls.Add(Me.ChatBox)
        Me.Controls.Add(Me.IPaddress2)
        Me.Controls.Add(Me.IPaddress1)
        Me.Controls.Add(Me.ChatHighlighter)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "MainWindow"
        Me.Text = "STREAM CONTROL CENTER"
        Me.TransparencyKey = System.Drawing.Color.Yellow
        CType(Me.EventHighlighter, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ChatHighlighter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents IPaddress1 As TextBox
    Friend WithEvents IPaddress2 As TextBox
    Friend WithEvents ChatBox As TextBox
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents DatastreamBox As TextBox
    Friend WithEvents SOUND_BOARD As Button
    Friend WithEvents SCENE_SELECTOR As Button
    Friend WithEvents CHARACTERS As Button
    Friend WithEvents TimersButt As Button
    Friend WithEvents ChatBUTT As Button
    Friend WithEvents CountersButt As Button
    Friend WithEvents CHANNEL_POINTS As Button
    Friend WithEvents MUSIC_PLAYER As Button
    Friend WithEvents ChatHighlighter As PictureBox
    Friend WithEvents EventBox As TextBox
    Friend WithEvents EventHighlighter As PictureBox
    Friend WithEvents NextMessage As Button
    Friend WithEvents NextEvent As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents Button4 As Button
    Friend WithEvents Button6 As Button
    Friend WithEvents Button8 As Button
    Friend WithEvents Button5 As Button
    Friend WithEvents Button9 As Button
    Friend WithEvents Button10 As Button
    Friend WithEvents Button11 As Button
    Friend WithEvents Button12 As Button
    Friend WithEvents Button13 As Button
    Friend WithEvents IRCdisplay As Label
    Friend WithEvents OBSdisplay As Label
    Friend WithEvents PUBSUBdisplay As Label
    Friend WithEvents APIdisplay As Label
    Friend WithEvents CTRL2display As Label
    Friend WithEvents CTRL1display As Label
    Friend WithEvents AUTHdisplay As Label
End Class
