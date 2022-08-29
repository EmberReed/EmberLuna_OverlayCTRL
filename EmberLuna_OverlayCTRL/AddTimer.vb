Imports System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar

Public Class AddTimer
    Public TimerIndex As Integer = -1, TimerData As TimerButton

    Public WithEvents MySounds As SoundController

    Private Sub AddTimer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MySounds = New SoundController(SoundSource.SFX, "Timer Editor")
        If TimerIndex > -1 Then
            TimerData = TimerCollection.Timers(TimerIndex)
        Else
            TimerData.InitializeTimer()
        End If
        Dim SoundList As List(Of String) = AudioControl.GetFullSoundFilesList
        For i As Integer = 0 To SoundList.Count - 1
            SoundPicker.Items.Add(SoundList(i))
        Next
        UpdateDisplay()
    End Sub
    Private Sub SoundPlayerActive() Handles MySounds.SoundStarted
        PlaySoundButt.BackgroundImage = My.Resources._STOP
        PlaySoundButt.BackColor = ActiveBUTT
    End Sub

    Private Sub SoundPlayerInActive() Handles MySounds.SoundStopped
        PlaySoundButt.BackgroundImage = My.Resources.play
        PlaySoundButt.BackColor = StandardBUTT
    End Sub

    Private Sub UpdateDisplay()
        TimerName.Text = TimerData.Name
        TimerLabel.Text = TimerData.Label
        StopTime.Value = TimerData.StopTime
        TimerTime.Value = TimerData.Time
        TimerID.Value = TimerData.Type
        If TimerData.PublicBool Then
            PubPriBUTT.Text = "PUBLIC"
        Else
            PubPriBUTT.Text = "PRIVATE"
        End If
        UpdateSoundlist()
    End Sub

    Private Sub UpdateSoundlist()
        TimerSoundlist.Text = ""
        If TimerData.Sounds IsNot Nothing Then
            If TimerData.Sounds.Count <> 0 Then
                For i As Integer = 0 To TimerData.Sounds.Count - 1
                    TimerSoundlist.Text = TimerSoundlist.Text & "(" & TimerData.Sounds(i).SoundTime & ")" & TimerData.Sounds(i).SoundObject
                    If i < TimerData.Sounds.Count - 1 Then TimerSoundlist.Text = TimerSoundlist.Text & vbCrLf
                Next
            End If
        End If
    End Sub

    Private Sub CheckSoundsData(Value As Integer, Optional Min As Boolean = False, Optional Max As Boolean = False)
        If TimerData.Sounds IsNot Nothing Then
            If TimerData.Sounds.Count <> 0 Then
                For i As Integer = 0 To TimerData.Sounds.Count - 1
                    If Max = True Then
                        If TimerData.Sounds(i).SoundTime > Value Then
                            Dim NewTimerSound As TimerSound = TimerData.Sounds(i)
                            NewTimerSound.SoundTime = Value
                            TimerData.Sounds(i) = NewTimerSound
                        End If
                    End If
                    If Min = True Then
                        If TimerData.Sounds(i).SoundTime < Value Then
                            Dim NewTimerSound As TimerSound = TimerData.Sounds(i)
                            NewTimerSound.SoundTime = Value
                            TimerData.Sounds(i) = NewTimerSound
                        End If
                    End If
                Next

            End If
        End If

    End Sub

    Private Sub CheckTimerRange()
        If TimerTime.Value >= StopTime.Value Then
            SoundTime.Maximum = TimerTime.Value
            SoundTime.Minimum = StopTime.Value
            CheckSoundsData(TimerTime.Value,, True)
            CheckSoundsData(StopTime.Value, True)
        Else
            SoundTime.Minimum = TimerTime.Value
            SoundTime.Maximum = StopTime.Value
            CheckSoundsData(TimerTime.Value, True)
            CheckSoundsData(StopTime.Value,, True)
        End If
        UpdateSoundlist()
    End Sub

    Private Sub TimerTime_ValueChanged(sender As Object, e As EventArgs) Handles TimerTime.ValueChanged
        Dim Time As DateTime = New DateTime(TimeSpan.FromSeconds(TimerTime.Value).Ticks)
        TimerTimeDisplay.Text = Time.ToString("mm:ss")
        CheckTimerRange()
        TimerData.Time = TimerTime.Value
    End Sub

    Private Sub StopTime_ValueChanged(sender As Object, e As EventArgs) Handles StopTime.ValueChanged
        Dim Time As DateTime = New DateTime(TimeSpan.FromSeconds(StopTime.Value).Ticks)
        StopTimeDisplay.Text = Time.ToString("mm:ss")
        CheckTimerRange()
        TimerData.StopTime = StopTime.Value
    End Sub

    Private Sub SoundTime_ValueChanged(sender As Object, e As EventArgs) Handles SoundTime.ValueChanged
        Dim Time As DateTime = New DateTime(TimeSpan.FromSeconds(SoundTime.Value).Ticks)
        SoundTimeDisplay.Text = Time.ToString("mm:ss")
    End Sub

    Private Sub AddSoundButt_Click(sender As Object, e As EventArgs) Handles AddSoundButt.Click
        If SoundPicker.Text <> "" Then
            Dim NewTimerSound As TimerSound
            NewTimerSound.SoundTime = SoundTime.Value
            NewTimerSound.SoundObject = SoundPicker.Text
            TimerData.Sounds.Add(NewTimerSound)
            UpdateSoundlist()
        End If
    End Sub

    Private Sub RemoveSoundButt_Click(sender As Object, e As EventArgs) Handles RemoveSoundButt.Click
        If TimerData.Sounds IsNot Nothing Then
            If TimerData.Sounds.Count > 0 Then
                TimerData.Sounds.RemoveAt(TimerData.Sounds.Count - 1)
                UpdateSoundlist()
            End If
        End If
    End Sub

    Private Sub PlaySoundButt_Click(sender As Object, e As EventArgs) Handles PlaySoundButt.Click
        If MySounds.IsActive Then
            Dim STASK As Task = MySounds.StopSound
        Else
            If SoundPicker.Text <> "" Then Dim Stask As Task = MySounds.PlaySound(AudioControl.GetSoundFileDataByName(SoundPicker.Text))
        End If
    End Sub

    Private Sub TimerID_ValueChanged(sender As Object, e As EventArgs) Handles TimerID.ValueChanged
        TimerData.Type = TimerID.Value
        Select Case TimerID.Value
            Case = TimerIDs.GlobalCC
                TimerIDlabel.Text = "GLOBAL"
            Case = TimerIDs.Ember
                TimerIDlabel.Text = "EMBER"
            Case = TimerIDs.Luna
                TimerIDlabel.Text = "LUNA"
        End Select
    End Sub

    Private Sub TimerName_TextChanged(sender As Object, e As EventArgs) Handles TimerName.TextChanged
        If TimerName.Text <> "" Then
            AddBUTT.BackColor = ActiveBUTT
        Else
            AddBUTT.BackColor = StandardBUTT
        End If
        TimerData.Name = TimerName.Text
    End Sub

    Private Sub TimerLabel_TextChanged(sender As Object, e As EventArgs) Handles TimerLabel.TextChanged
        TimerData.Label = TimerLabel.Text
    End Sub

    Private Sub AddBUTT_Click(sender As Object, e As EventArgs) Handles AddBUTT.Click
        If TimerName.Text <> "" Then
            If TimerIndex > -1 Then
                If TimerData.Name <> TimerCollection.Timers(TimerIndex).Name Then
                    If TimerCollection.CheckTimerNames(TimerData.Name) Then
                        TimerCollection.UpdateTimer(TimerIndex, TimerData)
                        Me.Close()
                    Else
                        SendMessage("That name is Taken. Try something else", "BE MOAR ORIGINAL, DOOFUS")
                    End If
                Else
                    TimerCollection.UpdateTimer(TimerIndex, TimerData)
                    Me.Close()
                End If
            Else
                If TimerCollection.CheckTimerNames(TimerData.Name) Then
                    TimerCollection.AddTimer(TimerData)
                    Me.Close()
                Else
                    SendMessage("That name is Taken. Try something else", "BE MOAR ORIGINAL, DOOFUS")
                End If
            End If
        Else
            SendMessage("You must Name the timer... DUMBASS", "WHAT'S IN A NAME?")
        End If

    End Sub

    Private Sub PubPriBUTT_Click(sender As Object, e As EventArgs) Handles PubPriBUTT.Click
        If PubPriBUTT.Text = "PRIVATE" Then
            PubPriBUTT.Text = "PUBLIC"
            TimerData.PublicBool = True
        Else
            PubPriBUTT.Text = "PRIVATE"
            TimerData.PublicBool = False
        End If
    End Sub


End Class