Public Class TimerControls

    Private Initialized As Boolean = False
    Private ActiveTimer As String
    Private TimerEditor As AddTimer

    Private Sub TimerControls_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Initialized = False
        RemoveHandler TimersUpdated, AddressOf UpdateDisplayEXT
        RemoveHandler TimerCollection.TimerDataUpdated, AddressOf RemoteUpdateTimerList
        SourceWindow.BeginInvoke(
            Sub()
                SourceWindow.TimersButt.BackColor = StandardBUTT
            End Sub)
    End Sub

    Private Sub TimerControls_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TimerEditor = New AddTimer
        UpdateTimerList()
        UpdateDisplay()
        AddHandler TimersUpdated, AddressOf UpdateDisplayEXT
        AddHandler TimerCollection.TimerDataUpdated, AddressOf RemoteUpdateTimerList
        UpdateSceneDisplay()
        Initialized = True
        SourceWindow.BeginInvoke(
            Sub()
                SourceWindow.TimersButt.BackColor = ActiveBUTT
            End Sub)
    End Sub

    Private Sub EmberClock_ValueChanged(sender As Object, e As EventArgs) Handles EmberClock.ValueChanged
        Dim Time As DateTime = New DateTime(TimeSpan.FromSeconds(EmberClock.Value).Ticks)
        EmberTime.Text = Time.ToString("mm:ss")
    End Sub

    Private Sub EmberTitle_TextChanged(sender As Object, e As EventArgs) Handles EmberTitle.TextChanged
        If InStr(EmberTitle.Text, vbCrLf) <> 0 Then
            EmberTitle.Text = Replace(EmberTitle.Text, vbCrLf, "")
        End If
        If Initialized = True Then
            OBStimerObject(TimerIDs.Ember).Title = EmberTitle.Text
        End If
    End Sub

    Private Sub EmberTrigger_Click(sender As Object, e As EventArgs) Handles EmberTrigger.Click
        EmberTrigger.Enabled = False
        EmberTrigger.BackColor = ActiveBUTT
        If OBStimerObject(TimerIDs.Ember).State = False Then
            TimerCollection.RunTimerbyData(EmberClock.Value, TimerIDs.Ember)
            'OBStimerObject(TimerIDs.Ember).State = True
            'Call RunTimer(EmberClock.Value, TimerIDs.Ember)
        Else
            OBStimerObject(TimerIDs.Ember).State = False
            If OBStimerObject(TimerIDs.Ember).Pause = True Then TimerCollection.PauseTimerByID(TimerIDs.Ember)
        End If
    End Sub

    Private Sub EmberPause_Click(sender As Object, e As EventArgs) Handles EmberPause.Click
        If OBStimerObject(TimerIDs.Ember).State = True Then
            EmberPause.Enabled = False
            EmberPause.BackColor = ActiveBUTT
            TimerCollection.PauseTimerByID(TimerIDs.Ember)
        End If
    End Sub

    Private Sub LunaClock_ValueChanged(sender As Object, e As EventArgs) Handles LunaClock.ValueChanged
        Dim Time As DateTime = New DateTime(TimeSpan.FromSeconds(LunaClock.Value).Ticks)
        LunaTime.Text = Time.ToString("mm:ss")
    End Sub

    Private Sub LunaTitle_TextChanged(sender As Object, e As EventArgs) Handles LunaTitle.TextChanged
        If InStr(LunaTitle.Text, vbCrLf) <> 0 Then
            LunaTitle.Text = Replace(LunaTitle.Text, vbCrLf, "")
        End If
        If Initialized = True Then
            OBStimerObject(TimerIDs.Luna).Title = LunaTitle.Text
        End If
    End Sub

    Private Sub LunaTrigger_Click(sender As Object, e As EventArgs) Handles LunaTrigger.Click
        LunaTrigger.Enabled = False
        LunaTrigger.BackColor = ActiveBUTT
        If OBStimerObject(TimerIDs.Luna).State = False Then
            TimerCollection.RunTimerbyData(LunaClock.Value, TimerIDs.Luna)
        Else
            OBStimerObject(TimerIDs.Luna).State = False
            'OBStimerObject(TimerIDs.Luna).Pause = False
            If OBStimerObject(TimerIDs.Luna).Pause = True Then TimerCollection.PauseTimerByID(TimerIDs.Luna)
        End If
    End Sub
    Private Sub LunaPause_Click(sender As Object, e As EventArgs) Handles LunaPause.Click
        If OBStimerObject(TimerIDs.Luna).State = True Then
            LunaPause.Enabled = False
            LunaPause.BackColor = ActiveBUTT
            TimerCollection.PauseTimerByID(TimerIDs.Luna)
        End If
    End Sub

    Private Sub GlobalClock_ValueChanged(sender As Object, e As EventArgs) Handles GlobalClock.ValueChanged
        Dim Time As DateTime = New DateTime(TimeSpan.FromSeconds(GlobalClock.Value).Ticks)
        GlobalTime.Text = Time.ToString("mm:ss")
    End Sub

    Private Sub GlobalTitle_TextChanged(sender As Object, e As EventArgs) Handles GlobalTitle.TextChanged
        If InStr(GlobalTitle.Text, vbCrLf) <> 0 Then
            GlobalTitle.Text = Replace(GlobalTitle.Text, vbCrLf, "")
        End If
        If Initialized = True Then
            OBStimerObject(TimerIDs.GlobalCC).Title = GlobalTitle.Text
        End If
    End Sub

    Private Sub globalTrigger_Click(sender As Object, e As EventArgs) Handles GlobalTrigger.Click
        GlobalTrigger.Enabled = False
        GlobalTrigger.BackColor = ActiveBUTT
        If OBStimerObject(TimerIDs.GlobalCC).State = False Then
            TimerCollection.RunTimerbyData(GlobalClock.Value, TimerIDs.GlobalCC)
        Else
            OBStimerObject(TimerIDs.GlobalCC).State = False
            'OBStimerObject(TimerIDs.GlobalCC).Pause = False
            If OBStimerObject(TimerIDs.GlobalCC).Pause = True Then TimerCollection.PauseTimerByID(TimerIDs.GlobalCC)
        End If
    End Sub

    Private Sub globalPause_Click(sender As Object, e As EventArgs) Handles GlobalPause.Click
        If OBStimerObject(TimerIDs.GlobalCC).State = True Then
            GlobalPause.Enabled = False
            GlobalPause.BackColor = ActiveBUTT
            TimerCollection.PauseTimerByID(TimerIDs.GlobalCC)
        End If
    End Sub

    Private Sub UpdateDisplayEXT(TimerID As Integer)
TryAgain:
        Try
            BeginInvoke(Sub() UpdateDisplay(TimerID))
        Catch
            GoTo TryAgain
        End Try
    End Sub

    Private Sub UpdateDisplay(Optional TimerID As Integer = -1)
        If TimerID = TimerIDs.Ember Or TimerID = -1 Then
            If OBStimerObject(TimerIDs.Ember).State = True Then
                EmberLabel.BackColor = ActiveBUTT
                EmberClock.Increment = 0
                EmberTitle.ReadOnly = True
                EmberTrigger.BackgroundImage = My.Resources._STOP
                EmberTime.Text = OBStimerObject(TimerIDs.Ember).Clock
                If OBStimerObject(TimerIDs.Ember).Pause = True Then
                    EmberPause.BackgroundImage = My.Resources.play
                Else
                    EmberPause.BackgroundImage = My.Resources.pause
                End If
            Else
                EmberLabel.BackColor = StandardBUTT
                'OBStimerObject(TimerIDs.Ember).Pause = False
                If OBStimerObject(TimerIDs.Ember).Pause = True Then TimerCollection.PauseTimerByID(TimerIDs.Ember)
                EmberPause.BackgroundImage = My.Resources.pause
                EmberClock.Increment = 60
                EmberTitle.ReadOnly = False
                EmberTrigger.BackgroundImage = My.Resources.play
                EmberTime.Text = TimeString(EmberClock.Value, "mm:ss")
            End If
            EmberTrigger.BackColor = StandardBUTT
            EmberPause.BackColor = StandardBUTT
            EmberTitle.Text = OBStimerObject(TimerIDs.Ember).Title
            EmberTrigger.Enabled = True
            EmberPause.Enabled = True
        End If

        If TimerID = TimerIDs.Luna Or TimerID = -1 Then
            If OBStimerObject(TimerIDs.Luna).State = True Then
                LunaLabel.BackColor = ActiveBUTT
                LunaClock.Increment = 0
                LunaTitle.ReadOnly = True
                LunaTrigger.BackgroundImage = My.Resources._STOP
                LunaTime.Text = OBStimerObject(TimerIDs.Luna).Clock
                If OBStimerObject(TimerIDs.Luna).Pause = True Then
                    LunaPause.BackgroundImage = My.Resources.play
                Else
                    LunaPause.BackgroundImage = My.Resources.pause
                End If
            Else
                LunaLabel.BackColor = StandardBUTT
                'OBStimerObject(TimerIDs.Luna).Pause = False
                If OBStimerObject(TimerIDs.Luna).Pause = True Then TimerCollection.PauseTimerByID(TimerIDs.Luna)
                LunaPause.BackgroundImage = My.Resources.pause
                LunaClock.Increment = 60
                LunaTitle.ReadOnly = False
                LunaTrigger.BackgroundImage = My.Resources.play
                LunaTime.Text = TimeString(LunaClock.Value, "mm:ss")
            End If
            LunaTrigger.BackColor = StandardBUTT
            LunaPause.BackColor = StandardBUTT
            LunaTitle.Text = OBStimerObject(TimerIDs.Luna).Title
            LunaTrigger.Enabled = True
            LunaPause.Enabled = True
        End If

        If TimerID = TimerIDs.GlobalCC Or TimerID = -1 Then
            If OBStimerObject(TimerIDs.GlobalCC).State = True Then
                GlobalLabel.BackColor = ActiveBUTT
                GlobalClock.Increment = 0
                GlobalTitle.ReadOnly = True
                GlobalTrigger.BackgroundImage = My.Resources._STOP
                GlobalTime.Text = OBStimerObject(TimerIDs.GlobalCC).Clock
                If OBStimerObject(TimerIDs.GlobalCC).Pause = True Then
                    GlobalPause.BackgroundImage = My.Resources.play
                Else
                    GlobalPause.BackgroundImage = My.Resources.pause
                End If
            Else
                GlobalLabel.BackColor = StandardBUTT
                If OBStimerObject(TimerIDs.GlobalCC).Pause = True Then TimerCollection.PauseTimerByID(TimerIDs.GlobalCC)
                'OBStimerObject(TimerIDs.GlobalCC).Pause = False
                GlobalPause.BackgroundImage = My.Resources.pause
                GlobalClock.Increment = 60
                GlobalTitle.ReadOnly = False
                GlobalTrigger.BackgroundImage = My.Resources.play
                GlobalTime.Text = TimeString(GlobalClock.Value, "mm:ss")
            End If
            GlobalTrigger.BackColor = StandardBUTT
            GlobalPause.BackColor = StandardBUTT
            GlobalTitle.Text = OBStimerObject(TimerIDs.GlobalCC).Title
            GlobalTrigger.Enabled = True
            GlobalPause.Enabled = True
        End If
    End Sub

    Private Sub RemoteUpdateTimerList()
TryAgain:
        Try
            BeginInvoke(Sub() UpdateTimerList())
        Catch
            GoTo TryAgain
        End Try
    End Sub

    Private Sub UpdateTimerList()
        SetDrawing(TimerDataDisplay.Handle, WM_SETREDRAW, False, 0)
        TimerDataDisplay.Text = ""
        Dim TimerIDname As String
        If TimerCollection.Timers IsNot Nothing Then
            If TimerCollection.Timers.Count <> 0 Then
                For i As Integer = 0 To TimerCollection.Timers.Count - 1
                    Select Case TimerCollection.Timers(i).Type
                        Case = TimerIDs.GlobalCC
                            TimerIDname = "(GLOBAL) "
                        Case = TimerIDs.Ember
                            TimerIDname = "(EMBER) "
                        Case = TimerIDs.Luna
                            TimerIDname = "(LUNA) "
                        Case Else
                            TimerIDname = ""
                    End Select
                    TimerDataDisplay.Text = TimerDataDisplay.Text & TimerIDname & TimerCollection.Timers(i).Name
                    If i < TimerCollection.Timers.Count - 1 Then TimerDataDisplay.Text = TimerDataDisplay.Text & vbCrLf
                Next
            End If
        End If
        SetDrawing(TimerDataDisplay.Handle, WM_SETREDRAW, True, 0)
        Me.Refresh()
    End Sub

    Private Sub TimerDataDisplay_Click(sender As Object, e As EventArgs) Handles TimerDataDisplay.Click
        If TimerDataDisplay.Text <> "" Then
            Dim current_line As Integer = TimerDataDisplay.GetLineFromCharIndex(TimerDataDisplay.SelectionStart)
            Dim line_length As Integer = TimerDataDisplay.Lines(current_line).Length
            TimerDataDisplay.SelectionStart = TimerDataDisplay.GetFirstCharIndexOfCurrentLine
            TimerDataDisplay.SelectionLength = line_length
            Dim splitstring() As String = Split(TimerDataDisplay.SelectedText, ") ", 0)
            ActiveTimer = splitstring(1)

        End If
    End Sub

    Private Sub TimerDataDisplay_DClick(sender As Object, e As EventArgs) Handles TimerDataDisplay.MouseDoubleClick
        If TimerDataDisplay.Text <> "" Then
            Dim current_line As Integer = TimerDataDisplay.GetLineFromCharIndex(TimerDataDisplay.SelectionStart)
            Dim line_length As Integer = TimerDataDisplay.Lines(current_line).Length
            TimerDataDisplay.SelectionStart = TimerDataDisplay.GetFirstCharIndexOfCurrentLine
            TimerDataDisplay.SelectionLength = line_length
            Dim splitstring() As String = Split(TimerDataDisplay.SelectedText, ") ", 0)
            ActiveTimer = splitstring(1)
            If ActiveTimer <> "" Then
                TimerCollection.RunTimerbyName(ActiveTimer)
            End If
        End If
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        If TimerEditor.Visible = False Then
            TimerEditor = New AddTimer
            TimerEditor.Show()
        End If
    End Sub

    Private Sub EditToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditToolStripMenuItem.Click
        If ActiveTimer <> "" Then
            If TimerEditor.Visible = False Then
                TimerEditor = New AddTimer
                TimerEditor.TimerIndex = TimerCollection.GetTimerIndexByName(ActiveTimer)
                TimerEditor.Show()
            End If
        End If
    End Sub

    Private Sub DeleteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteToolStripMenuItem.Click
        If ActiveTimer <> "" Then
            Dim CheckValue As MsgBoxResult = MsgBox("Are you sure about that?", MsgBoxStyle.OkCancel, "CAN(NOT) UNDO!")
            If CheckValue = MsgBoxResult.Ok Then TimerCollection.DeleteTimer(TimerCollection.GetTimerIndexByName(ActiveTimer))
        End If
    End Sub

    Private Sub StartToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StartToolStripMenuItem.Click
        If ActiveTimer <> "" Then
            TimerCollection.RunTimerbyName(ActiveTimer)
        End If
    End Sub
End Class