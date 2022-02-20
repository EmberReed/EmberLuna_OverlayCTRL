<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class TimerControls
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
        Me.EmberLabel = New System.Windows.Forms.Label()
        Me.EmberClock = New System.Windows.Forms.NumericUpDown()
        Me.EmberTime = New System.Windows.Forms.TextBox()
        Me.EmberTitle = New System.Windows.Forms.TextBox()
        Me.LunaTitle = New System.Windows.Forms.TextBox()
        Me.LunaTime = New System.Windows.Forms.TextBox()
        Me.LunaClock = New System.Windows.Forms.NumericUpDown()
        Me.LunaLabel = New System.Windows.Forms.Label()
        Me.GlobalTitle = New System.Windows.Forms.TextBox()
        Me.GlobalTime = New System.Windows.Forms.TextBox()
        Me.GlobalClock = New System.Windows.Forms.NumericUpDown()
        Me.GlobalLabel = New System.Windows.Forms.Label()
        Me.EmberEnable = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.Button7 = New System.Windows.Forms.Button()
        Me.Button8 = New System.Windows.Forms.Button()
        Me.Button9 = New System.Windows.Forms.Button()
        Me.Button10 = New System.Windows.Forms.Button()
        Me.Button11 = New System.Windows.Forms.Button()
        Me.Button12 = New System.Windows.Forms.Button()
        Me.Button13 = New System.Windows.Forms.Button()
        Me.Button14 = New System.Windows.Forms.Button()
        Me.Button15 = New System.Windows.Forms.Button()
        Me.Button16 = New System.Windows.Forms.Button()
        Me.TimerDataDisplay = New System.Windows.Forms.TextBox()
        Me.TimerObjectMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StartToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GlobalPause = New System.Windows.Forms.PictureBox()
        Me.GlobalTrigger = New System.Windows.Forms.PictureBox()
        Me.LunaPause = New System.Windows.Forms.PictureBox()
        Me.LunaTrigger = New System.Windows.Forms.PictureBox()
        Me.EmberPause = New System.Windows.Forms.PictureBox()
        Me.EmberTrigger = New System.Windows.Forms.PictureBox()
        CType(Me.EmberClock, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LunaClock, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GlobalClock, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TimerObjectMenu.SuspendLayout()
        CType(Me.GlobalPause, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GlobalTrigger, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LunaPause, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LunaTrigger, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EmberPause, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EmberTrigger, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'EmberLabel
        '
        Me.EmberLabel.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.EmberLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.EmberLabel.Font = New System.Drawing.Font("Calibri", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.EmberLabel.ForeColor = System.Drawing.Color.White
        Me.EmberLabel.Location = New System.Drawing.Point(5, 5)
        Me.EmberLabel.Name = "EmberLabel"
        Me.EmberLabel.Size = New System.Drawing.Size(305, 44)
        Me.EmberLabel.TabIndex = 3
        Me.EmberLabel.Text = "EMBER TIMER"
        '
        'EmberClock
        '
        Me.EmberClock.BackColor = System.Drawing.Color.Black
        Me.EmberClock.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.EmberClock.Location = New System.Drawing.Point(74, 20)
        Me.EmberClock.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.EmberClock.Name = "EmberClock"
        Me.EmberClock.Size = New System.Drawing.Size(18, 26)
        Me.EmberClock.TabIndex = 4
        Me.EmberClock.Value = New Decimal(New Integer() {60, 0, 0, 0})
        '
        'EmberTime
        '
        Me.EmberTime.BackColor = System.Drawing.Color.Black
        Me.EmberTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.EmberTime.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.EmberTime.ForeColor = System.Drawing.Color.White
        Me.EmberTime.Location = New System.Drawing.Point(10, 20)
        Me.EmberTime.Name = "EmberTime"
        Me.EmberTime.ReadOnly = True
        Me.EmberTime.Size = New System.Drawing.Size(64, 26)
        Me.EmberTime.TabIndex = 6
        Me.EmberTime.Text = "1:00"
        Me.EmberTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'EmberTitle
        '
        Me.EmberTitle.BackColor = System.Drawing.Color.Black
        Me.EmberTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.EmberTitle.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.EmberTitle.ForeColor = System.Drawing.Color.White
        Me.EmberTitle.Location = New System.Drawing.Point(95, 20)
        Me.EmberTitle.Name = "EmberTitle"
        Me.EmberTitle.ReadOnly = True
        Me.EmberTitle.Size = New System.Drawing.Size(133, 26)
        Me.EmberTitle.TabIndex = 7
        Me.EmberTitle.Text = "TIMER TITLE"
        Me.EmberTitle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'LunaTitle
        '
        Me.LunaTitle.BackColor = System.Drawing.Color.Black
        Me.LunaTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LunaTitle.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LunaTitle.ForeColor = System.Drawing.Color.White
        Me.LunaTitle.Location = New System.Drawing.Point(95, 66)
        Me.LunaTitle.Name = "LunaTitle"
        Me.LunaTitle.ReadOnly = True
        Me.LunaTitle.Size = New System.Drawing.Size(133, 26)
        Me.LunaTitle.TabIndex = 12
        Me.LunaTitle.Text = "TIMER TITLE"
        Me.LunaTitle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'LunaTime
        '
        Me.LunaTime.BackColor = System.Drawing.Color.Black
        Me.LunaTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LunaTime.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LunaTime.ForeColor = System.Drawing.Color.White
        Me.LunaTime.Location = New System.Drawing.Point(10, 66)
        Me.LunaTime.Name = "LunaTime"
        Me.LunaTime.ReadOnly = True
        Me.LunaTime.Size = New System.Drawing.Size(64, 26)
        Me.LunaTime.TabIndex = 11
        Me.LunaTime.Text = "1:00"
        Me.LunaTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'LunaClock
        '
        Me.LunaClock.BackColor = System.Drawing.Color.Black
        Me.LunaClock.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LunaClock.Location = New System.Drawing.Point(74, 66)
        Me.LunaClock.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.LunaClock.Name = "LunaClock"
        Me.LunaClock.Size = New System.Drawing.Size(18, 26)
        Me.LunaClock.TabIndex = 10
        Me.LunaClock.Value = New Decimal(New Integer() {60, 0, 0, 0})
        '
        'LunaLabel
        '
        Me.LunaLabel.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.LunaLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LunaLabel.Font = New System.Drawing.Font("Calibri", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LunaLabel.ForeColor = System.Drawing.Color.White
        Me.LunaLabel.Location = New System.Drawing.Point(5, 52)
        Me.LunaLabel.Name = "LunaLabel"
        Me.LunaLabel.Size = New System.Drawing.Size(305, 44)
        Me.LunaLabel.TabIndex = 9
        Me.LunaLabel.Text = "LUNA TIMER"
        '
        'GlobalTitle
        '
        Me.GlobalTitle.BackColor = System.Drawing.Color.Black
        Me.GlobalTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.GlobalTitle.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GlobalTitle.ForeColor = System.Drawing.Color.White
        Me.GlobalTitle.Location = New System.Drawing.Point(95, 113)
        Me.GlobalTitle.Name = "GlobalTitle"
        Me.GlobalTitle.ReadOnly = True
        Me.GlobalTitle.Size = New System.Drawing.Size(133, 26)
        Me.GlobalTitle.TabIndex = 19
        Me.GlobalTitle.Text = "TIMER TITLE"
        Me.GlobalTitle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'GlobalTime
        '
        Me.GlobalTime.BackColor = System.Drawing.Color.Black
        Me.GlobalTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.GlobalTime.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GlobalTime.ForeColor = System.Drawing.Color.White
        Me.GlobalTime.Location = New System.Drawing.Point(10, 113)
        Me.GlobalTime.Name = "GlobalTime"
        Me.GlobalTime.ReadOnly = True
        Me.GlobalTime.Size = New System.Drawing.Size(64, 26)
        Me.GlobalTime.TabIndex = 18
        Me.GlobalTime.Text = "1:00"
        Me.GlobalTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'GlobalClock
        '
        Me.GlobalClock.BackColor = System.Drawing.Color.Black
        Me.GlobalClock.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GlobalClock.Location = New System.Drawing.Point(74, 113)
        Me.GlobalClock.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.GlobalClock.Name = "GlobalClock"
        Me.GlobalClock.Size = New System.Drawing.Size(18, 26)
        Me.GlobalClock.TabIndex = 17
        Me.GlobalClock.Value = New Decimal(New Integer() {60, 0, 0, 0})
        '
        'GlobalLabel
        '
        Me.GlobalLabel.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.GlobalLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.GlobalLabel.Font = New System.Drawing.Font("Calibri", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GlobalLabel.ForeColor = System.Drawing.Color.White
        Me.GlobalLabel.Location = New System.Drawing.Point(5, 99)
        Me.GlobalLabel.Name = "GlobalLabel"
        Me.GlobalLabel.Size = New System.Drawing.Size(305, 44)
        Me.GlobalLabel.TabIndex = 16
        Me.GlobalLabel.Text = "GLOBAL TIMER"
        '
        'EmberEnable
        '
        Me.EmberEnable.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.EmberEnable.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.EmberEnable.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.EmberEnable.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.EmberEnable.Location = New System.Drawing.Point(338, 12)
        Me.EmberEnable.Name = "EmberEnable"
        Me.EmberEnable.Size = New System.Drawing.Size(305, 29)
        Me.EmberEnable.TabIndex = 31
        Me.EmberEnable.UseVisualStyleBackColor = False
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button1.Location = New System.Drawing.Point(338, 44)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(305, 29)
        Me.Button1.TabIndex = 32
        Me.Button1.UseVisualStyleBackColor = False
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button2.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button2.Location = New System.Drawing.Point(338, 108)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(305, 29)
        Me.Button2.TabIndex = 34
        Me.Button2.UseVisualStyleBackColor = False
        '
        'Button3
        '
        Me.Button3.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button3.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button3.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button3.Location = New System.Drawing.Point(338, 76)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(305, 29)
        Me.Button3.TabIndex = 33
        Me.Button3.UseVisualStyleBackColor = False
        '
        'Button4
        '
        Me.Button4.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button4.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button4.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button4.Location = New System.Drawing.Point(338, 172)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(305, 29)
        Me.Button4.TabIndex = 36
        Me.Button4.UseVisualStyleBackColor = False
        '
        'Button5
        '
        Me.Button5.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button5.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button5.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button5.Location = New System.Drawing.Point(338, 140)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(305, 29)
        Me.Button5.TabIndex = 35
        Me.Button5.UseVisualStyleBackColor = False
        '
        'Button6
        '
        Me.Button6.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button6.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button6.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button6.Location = New System.Drawing.Point(338, 236)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(305, 29)
        Me.Button6.TabIndex = 38
        Me.Button6.UseVisualStyleBackColor = False
        '
        'Button7
        '
        Me.Button7.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button7.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button7.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button7.Location = New System.Drawing.Point(338, 204)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(305, 29)
        Me.Button7.TabIndex = 37
        Me.Button7.UseVisualStyleBackColor = False
        '
        'Button8
        '
        Me.Button8.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Button8.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button8.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button8.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button8.Location = New System.Drawing.Point(338, 300)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(305, 29)
        Me.Button8.TabIndex = 40
        Me.Button8.UseVisualStyleBackColor = False
        '
        'Button9
        '
        Me.Button9.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Button9.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button9.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button9.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button9.Location = New System.Drawing.Point(338, 268)
        Me.Button9.Name = "Button9"
        Me.Button9.Size = New System.Drawing.Size(305, 29)
        Me.Button9.TabIndex = 39
        Me.Button9.UseVisualStyleBackColor = False
        '
        'Button10
        '
        Me.Button10.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Button10.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button10.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button10.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button10.Location = New System.Drawing.Point(5, 145)
        Me.Button10.Name = "Button10"
        Me.Button10.Size = New System.Drawing.Size(87, 29)
        Me.Button10.TabIndex = 41
        Me.Button10.Text = "NEW"
        Me.Button10.UseVisualStyleBackColor = False
        '
        'Button11
        '
        Me.Button11.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Button11.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button11.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button11.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button11.Location = New System.Drawing.Point(96, 145)
        Me.Button11.Name = "Button11"
        Me.Button11.Size = New System.Drawing.Size(105, 29)
        Me.Button11.TabIndex = 68
        Me.Button11.Text = "STOP ALL"
        Me.Button11.UseVisualStyleBackColor = False
        '
        'Button12
        '
        Me.Button12.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Button12.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button12.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button12.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button12.Location = New System.Drawing.Point(205, 145)
        Me.Button12.Name = "Button12"
        Me.Button12.Size = New System.Drawing.Size(105, 29)
        Me.Button12.TabIndex = 69
        Me.Button12.Text = "PAUSE ALL"
        Me.Button12.UseVisualStyleBackColor = False
        '
        'Button13
        '
        Me.Button13.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Button13.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button13.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button13.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button13.Location = New System.Drawing.Point(338, 428)
        Me.Button13.Name = "Button13"
        Me.Button13.Size = New System.Drawing.Size(305, 29)
        Me.Button13.TabIndex = 73
        Me.Button13.UseVisualStyleBackColor = False
        '
        'Button14
        '
        Me.Button14.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Button14.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button14.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button14.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button14.Location = New System.Drawing.Point(338, 396)
        Me.Button14.Name = "Button14"
        Me.Button14.Size = New System.Drawing.Size(305, 29)
        Me.Button14.TabIndex = 72
        Me.Button14.UseVisualStyleBackColor = False
        '
        'Button15
        '
        Me.Button15.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Button15.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button15.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button15.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button15.Location = New System.Drawing.Point(338, 364)
        Me.Button15.Name = "Button15"
        Me.Button15.Size = New System.Drawing.Size(305, 29)
        Me.Button15.TabIndex = 71
        Me.Button15.UseVisualStyleBackColor = False
        '
        'Button16
        '
        Me.Button16.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.Button16.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button16.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button16.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button16.Location = New System.Drawing.Point(338, 332)
        Me.Button16.Name = "Button16"
        Me.Button16.Size = New System.Drawing.Size(305, 29)
        Me.Button16.TabIndex = 70
        Me.Button16.UseVisualStyleBackColor = False
        '
        'TimerDataDisplay
        '
        Me.TimerDataDisplay.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TimerDataDisplay.BackColor = System.Drawing.Color.Black
        Me.TimerDataDisplay.ContextMenuStrip = Me.TimerObjectMenu
        Me.TimerDataDisplay.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TimerDataDisplay.ForeColor = System.Drawing.Color.White
        Me.TimerDataDisplay.Location = New System.Drawing.Point(5, 176)
        Me.TimerDataDisplay.Multiline = True
        Me.TimerDataDisplay.Name = "TimerDataDisplay"
        Me.TimerDataDisplay.ReadOnly = True
        Me.TimerDataDisplay.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TimerDataDisplay.Size = New System.Drawing.Size(305, 251)
        Me.TimerDataDisplay.TabIndex = 75
        Me.TimerDataDisplay.Text = "Test 1" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Test 2" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Test 3"
        '
        'TimerObjectMenu
        '
        Me.TimerObjectMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EditToolStripMenuItem, Me.DeleteToolStripMenuItem, Me.StartToolStripMenuItem})
        Me.TimerObjectMenu.Name = "TimerObjectMenu"
        Me.TimerObjectMenu.Size = New System.Drawing.Size(181, 92)
        '
        'EditToolStripMenuItem
        '
        Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
        Me.EditToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.EditToolStripMenuItem.Text = "Edit"
        '
        'DeleteToolStripMenuItem
        '
        Me.DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem"
        Me.DeleteToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.DeleteToolStripMenuItem.Text = "Delete"
        '
        'StartToolStripMenuItem
        '
        Me.StartToolStripMenuItem.Name = "StartToolStripMenuItem"
        Me.StartToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.StartToolStripMenuItem.Text = "Start"
        '
        'GlobalPause
        '
        Me.GlobalPause.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.GlobalPause.BackgroundImage = Global.EmberLuna_OverlayCTRL.My.Resources.Resources.pause
        Me.GlobalPause.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.GlobalPause.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.GlobalPause.Image = Global.EmberLuna_OverlayCTRL.My.Resources.Resources.pause
        Me.GlobalPause.Location = New System.Drawing.Point(270, 104)
        Me.GlobalPause.Name = "GlobalPause"
        Me.GlobalPause.Size = New System.Drawing.Size(35, 35)
        Me.GlobalPause.TabIndex = 30
        Me.GlobalPause.TabStop = False
        '
        'GlobalTrigger
        '
        Me.GlobalTrigger.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.GlobalTrigger.BackgroundImage = Global.EmberLuna_OverlayCTRL.My.Resources.Resources.play
        Me.GlobalTrigger.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.GlobalTrigger.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.GlobalTrigger.Image = Global.EmberLuna_OverlayCTRL.My.Resources.Resources.play
        Me.GlobalTrigger.Location = New System.Drawing.Point(231, 104)
        Me.GlobalTrigger.Name = "GlobalTrigger"
        Me.GlobalTrigger.Size = New System.Drawing.Size(35, 35)
        Me.GlobalTrigger.TabIndex = 29
        Me.GlobalTrigger.TabStop = False
        '
        'LunaPause
        '
        Me.LunaPause.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.LunaPause.BackgroundImage = Global.EmberLuna_OverlayCTRL.My.Resources.Resources.pause
        Me.LunaPause.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.LunaPause.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LunaPause.Image = Global.EmberLuna_OverlayCTRL.My.Resources.Resources.pause
        Me.LunaPause.Location = New System.Drawing.Point(270, 57)
        Me.LunaPause.Name = "LunaPause"
        Me.LunaPause.Size = New System.Drawing.Size(35, 35)
        Me.LunaPause.TabIndex = 28
        Me.LunaPause.TabStop = False
        '
        'LunaTrigger
        '
        Me.LunaTrigger.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.LunaTrigger.BackgroundImage = Global.EmberLuna_OverlayCTRL.My.Resources.Resources.play
        Me.LunaTrigger.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.LunaTrigger.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LunaTrigger.Image = Global.EmberLuna_OverlayCTRL.My.Resources.Resources.play
        Me.LunaTrigger.Location = New System.Drawing.Point(231, 57)
        Me.LunaTrigger.Name = "LunaTrigger"
        Me.LunaTrigger.Size = New System.Drawing.Size(35, 35)
        Me.LunaTrigger.TabIndex = 27
        Me.LunaTrigger.TabStop = False
        '
        'EmberPause
        '
        Me.EmberPause.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.EmberPause.BackgroundImage = Global.EmberLuna_OverlayCTRL.My.Resources.Resources.pause
        Me.EmberPause.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.EmberPause.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.EmberPause.Image = Global.EmberLuna_OverlayCTRL.My.Resources.Resources.pause
        Me.EmberPause.Location = New System.Drawing.Point(270, 10)
        Me.EmberPause.Name = "EmberPause"
        Me.EmberPause.Size = New System.Drawing.Size(35, 35)
        Me.EmberPause.TabIndex = 26
        Me.EmberPause.TabStop = False
        '
        'EmberTrigger
        '
        Me.EmberTrigger.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.EmberTrigger.BackgroundImage = Global.EmberLuna_OverlayCTRL.My.Resources.Resources.play
        Me.EmberTrigger.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.EmberTrigger.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.EmberTrigger.Image = Global.EmberLuna_OverlayCTRL.My.Resources.Resources.play
        Me.EmberTrigger.Location = New System.Drawing.Point(231, 10)
        Me.EmberTrigger.Name = "EmberTrigger"
        Me.EmberTrigger.Size = New System.Drawing.Size(35, 35)
        Me.EmberTrigger.TabIndex = 25
        Me.EmberTrigger.TabStop = False
        '
        'TimerControls
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(31, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(31, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(314, 431)
        Me.Controls.Add(Me.TimerDataDisplay)
        Me.Controls.Add(Me.Button13)
        Me.Controls.Add(Me.Button14)
        Me.Controls.Add(Me.Button15)
        Me.Controls.Add(Me.Button16)
        Me.Controls.Add(Me.Button12)
        Me.Controls.Add(Me.Button11)
        Me.Controls.Add(Me.Button10)
        Me.Controls.Add(Me.Button8)
        Me.Controls.Add(Me.Button9)
        Me.Controls.Add(Me.Button6)
        Me.Controls.Add(Me.Button7)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.EmberEnable)
        Me.Controls.Add(Me.GlobalPause)
        Me.Controls.Add(Me.GlobalTrigger)
        Me.Controls.Add(Me.LunaPause)
        Me.Controls.Add(Me.LunaTrigger)
        Me.Controls.Add(Me.EmberPause)
        Me.Controls.Add(Me.EmberTrigger)
        Me.Controls.Add(Me.GlobalTitle)
        Me.Controls.Add(Me.GlobalTime)
        Me.Controls.Add(Me.GlobalClock)
        Me.Controls.Add(Me.LunaTitle)
        Me.Controls.Add(Me.LunaTime)
        Me.Controls.Add(Me.LunaClock)
        Me.Controls.Add(Me.LunaLabel)
        Me.Controls.Add(Me.EmberTitle)
        Me.Controls.Add(Me.EmberTime)
        Me.Controls.Add(Me.EmberClock)
        Me.Controls.Add(Me.EmberLabel)
        Me.Controls.Add(Me.GlobalLabel)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(330, 3000)
        Me.MinimumSize = New System.Drawing.Size(330, 330)
        Me.Name = "TimerControls"
        Me.Text = "TimerControls"
        CType(Me.EmberClock, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LunaClock, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GlobalClock, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TimerObjectMenu.ResumeLayout(False)
        CType(Me.GlobalPause, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GlobalTrigger, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LunaPause, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LunaTrigger, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EmberPause, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EmberTrigger, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents EmberLabel As Label
    Friend WithEvents EmberClock As NumericUpDown
    Friend WithEvents EmberTime As TextBox
    Friend WithEvents EmberTitle As TextBox
    Friend WithEvents LunaTitle As TextBox
    Friend WithEvents LunaTime As TextBox
    Friend WithEvents LunaClock As NumericUpDown
    Friend WithEvents LunaLabel As Label
    Friend WithEvents GlobalTitle As TextBox
    Friend WithEvents GlobalTime As TextBox
    Friend WithEvents GlobalClock As NumericUpDown
    Friend WithEvents GlobalLabel As Label
    Friend WithEvents EmberPause As PictureBox
    Friend WithEvents EmberTrigger As PictureBox
    Friend WithEvents LunaPause As PictureBox
    Friend WithEvents LunaTrigger As PictureBox
    Friend WithEvents GlobalPause As PictureBox
    Friend WithEvents GlobalTrigger As PictureBox
    Friend WithEvents EmberEnable As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents Button4 As Button
    Friend WithEvents Button5 As Button
    Friend WithEvents Button6 As Button
    Friend WithEvents Button7 As Button
    Friend WithEvents Button8 As Button
    Friend WithEvents Button9 As Button
    Friend WithEvents Button10 As Button
    Friend WithEvents Button11 As Button
    Friend WithEvents Button12 As Button
    Friend WithEvents Button13 As Button
    Friend WithEvents Button14 As Button
    Friend WithEvents Button15 As Button
    Friend WithEvents Button16 As Button
    Friend WithEvents TimerDataDisplay As TextBox
    Friend WithEvents TimerObjectMenu As ContextMenuStrip
    Friend WithEvents EditToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DeleteToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents StartToolStripMenuItem As ToolStripMenuItem
End Class
