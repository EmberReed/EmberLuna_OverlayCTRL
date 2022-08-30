Imports System.Math
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class OBSvolumeControls

    Public Sub ImportSingleR(ChannelIndex As Integer)
        ImportSingle(ChannelIndex, True)
    End Sub

    Public Sub ImportSingle(ChannelIndex As Integer, Optional UpdateMatrix As Boolean = False)
        Dim MyChannel As GroupBox = Controls("GroupBox" & ChannelIndex + 1)
        Dim MydB As NumericUpDown = MyChannel.Controls("NumericUpDown" & ChannelIndex + 1)
        Dim MyMute As Windows.Forms.Button = MyChannel.Controls("Button" & ChannelIndex + 1)
        MydB.Value = Round(100 + AudioControl.MyMixer.Channels(ChannelIndex).Level, 1)
        If AudioControl.MyMixer.Channels(ChannelIndex).Muted Then
            MyMute.Text = "UNMUTE"
            MyMute.BackColor = ActiveBUTT
        Else
            MyMute.Text = "MUTE"
            MyMute.BackColor = StandardBUTT
        End If
        If UpdateMatrix Then UpdateMuteMatrix()
    End Sub

    Public Sub ImportAll()
        For I As Integer = 0 To AudioChannelIDs.LastChanneL
            ImportSingle(I)
        Next
        UpdateMuteMatrix()
    End Sub

    Public Sub UpdateMuteMatrix()
        If AudioControl.MyMixer.Channels(AudioChannelIDs.EmberMic).Muted = True And
            AudioControl.MyMixer.Channels(AudioChannelIDs.EmberPC).Muted = True Then
            EMBERmute.BackColor = ActiveBUTT
        Else
            EMBERmute.BackColor = StandardBUTT
        End If
        If AudioControl.MyMixer.Channels(AudioChannelIDs.LunaMic).Muted = True And
            AudioControl.MyMixer.Channels(AudioChannelIDs.LunaPC).Muted = True Then
            LUNAmute.BackColor = ActiveBUTT
        Else
            LUNAmute.BackColor = StandardBUTT
        End If
        If AudioControl.MyMixer.Channels(AudioChannelIDs.EmberPC).Muted = True And
            AudioControl.MyMixer.Channels(AudioChannelIDs.LunaPC).Muted = True Then
            PCmute.BackColor = ActiveBUTT
        Else
            PCmute.BackColor = StandardBUTT
        End If
        If AudioControl.MyMixer.Channels(AudioChannelIDs.LunaMic).Muted = True And
            AudioControl.MyMixer.Channels(AudioChannelIDs.EmberMic).Muted = True Then
            MICmute.BackColor = ActiveBUTT
        Else
            MICmute.BackColor = StandardBUTT
        End If
    End Sub

    Private Sub OBSvolumeControls_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ImportAll()
        AddHandler AudioControl.MyMixer.MixerChannelChanged, AddressOf ImportSingleR
    End Sub

    Private Sub OBSvolumeControls_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        RemoveHandler AudioControl.MyMixer.MixerChannelChanged, AddressOf ImportSingleR
    End Sub

    Private Sub OBSvolumeControls_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        SourceWindow.Button11.BackColor = ActiveBUTT
    End Sub

    Private Sub OBSvolumeControls_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        SourceWindow.Button11.BackColor = StandardBUTT
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        Number_Output_toSlider(sender)
    End Sub
    Private Sub ProgressBar1_Click(sender As Object, e As MouseEventArgs) Handles ProgressBar1.MouseClick
        Control_Click_todB(e, sender)
    End Sub
    Private Sub ProgressBar1_MouseHover(sender As Object, e As MouseEventArgs) Handles ProgressBar1.MouseMove
        ToolTip1.SetToolTip(sender, Control_dB(e, sender, True) & "dB")
    End Sub

    Private Sub NumericUpDown2_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown2.ValueChanged
        Number_Output_toSlider(sender)
    End Sub
    Private Sub ProgressBar2_Click(sender As Object, e As MouseEventArgs) Handles ProgressBar2.MouseClick
        Control_Click_todB(e, sender)
    End Sub
    Private Sub ProgressBar2_MouseHover(sender As Object, e As MouseEventArgs) Handles ProgressBar2.MouseMove
        ToolTip1.SetToolTip(sender, Control_dB(e, sender, True) & "dB")
    End Sub

    Private Sub NumericUpDown3_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown3.ValueChanged
        Number_Output_toSlider(sender)
    End Sub
    Private Sub ProgressBar3_Click(sender As Object, e As MouseEventArgs) Handles ProgressBar3.MouseClick
        Control_Click_todB(e, sender)
    End Sub
    Private Sub ProgressBar3_MouseHover(sender As Object, e As MouseEventArgs) Handles ProgressBar3.MouseMove
        ToolTip1.SetToolTip(sender, Control_dB(e, sender, True) & "dB")
    End Sub

    Private Sub NumericUpDown4_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown4.ValueChanged
        Number_Output_toSlider(sender)
    End Sub
    Private Sub ProgressBar4_Click(sender As Object, e As MouseEventArgs) Handles ProgressBar4.MouseClick
        Control_Click_todB(e, sender)
    End Sub
    Private Sub ProgressBar4_MouseHover(sender As Object, e As MouseEventArgs) Handles ProgressBar4.MouseMove
        ToolTip1.SetToolTip(sender, Control_dB(e, sender, True) & "dB")
    End Sub

    Private Sub NumericUpDown5_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown5.ValueChanged
        Number_Output_toSlider(sender)
    End Sub
    Private Sub ProgressBar5_Click(sender As Object, e As MouseEventArgs) Handles ProgressBar5.MouseClick
        Control_Click_todB(e, sender)
    End Sub
    Private Sub ProgressBar5_MouseHover(sender As Object, e As MouseEventArgs) Handles ProgressBar5.MouseMove
        ToolTip1.SetToolTip(sender, Control_dB(e, sender, True) & "dB")
    End Sub

    Private Sub NumericUpDown6_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown6.ValueChanged
        Number_Output_toSlider(sender)
    End Sub
    Private Sub ProgressBar6_Click(sender As Object, e As MouseEventArgs) Handles ProgressBar6.MouseClick
        Control_Click_todB(e, sender)
    End Sub
    Private Sub ProgressBar6_MouseHover(sender As Object, e As MouseEventArgs) Handles ProgressBar6.MouseMove
        ToolTip1.SetToolTip(sender, Control_dB(e, sender, True) & "dB")
    End Sub

    Private Sub NumericUpDown7_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown7.ValueChanged
        Number_Output_toSlider(sender)
    End Sub
    Private Sub ProgressBar7_Click(sender As Object, e As MouseEventArgs) Handles ProgressBar7.MouseClick
        Control_Click_todB(e, sender)
    End Sub
    Private Sub ProgressBar7_MouseHover(sender As Object, e As MouseEventArgs) Handles ProgressBar7.MouseMove
        ToolTip1.SetToolTip(sender, Control_dB(e, sender, True) & "dB")
    End Sub

    Private Function Control_dB(MousePOS As MouseEventArgs, Control As Windows.Forms.ProgressBar, Optional dBout As Boolean = False)
        Dim Xpercent As Double = MousePOS.X / Control.Size.Width
        Xpercent = 100 + (50 * Log10(Xpercent))
        If Xpercent < 0 Then Xpercent = 0
        If dBout Then
            Return Round(Xpercent - 100, 1)
        Else
            Return Round(Xpercent, 1)
        End If
    End Function

    Private Sub Number_Output_toSlider(Control As NumericUpDown)
        Dim ControlInt As Integer = Replace(Control.Name, "NumericUpDown", "")
        Dim MyChannel As GroupBox = Controls("GroupBox" & ControlInt)
        Dim NumOut As Windows.Forms.ProgressBar = MyChannel.Controls("ProgressBar" & ControlInt)
        Dim ValueOut As Label = MyChannel.Controls("Label" & ControlInt)
        ValueOut.Text = AudioControl.MyMixer.Channels(ControlInt - 1).Name & " (" & Control.Value - 100 & "dB)"
        NumOut.Value = 100 * (10 ^ ((Control.Value - 100) / 50))
        If SourceWindow.Button11.BackColor = ActiveBUTT Then
            AudioControl.MyMixer.Channels(ControlInt - 1).ApplySettings(Control.Value - 100)
        End If
    End Sub

    Private Sub Control_Click_todB(MousePOS As MouseEventArgs, Control As Windows.Forms.ProgressBar)
        Dim ControlInt As Integer = Replace(Control.Name, "ProgressBar", "")
        Dim MyChannel As GroupBox = Controls("GroupBox" & ControlInt)
        Dim NumOut As NumericUpDown = MyChannel.Controls("NumericUpDown" & ControlInt)
        NumOut.Value = Control_dB(MousePOS, Control)
    End Sub


    Private Sub MuteClick(Muter As Windows.Forms.Button)
        Dim ControlInt As Integer = Replace(Muter.Name, "Button", "")
        AudioControl.MyMixer.ToggleMute(ControlInt - 1)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        MuteClick(sender)
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        MuteClick(sender)
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        MuteClick(sender)
    End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        MuteClick(sender)
    End Sub
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        MuteClick(sender)
    End Sub
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        MuteClick(sender)
    End Sub
    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        MuteClick(sender)
    End Sub

    Private Sub EMBERmute_Click(sender As Object, e As EventArgs) Handles EMBERmute.Click
        If EMBERmute.BackColor = ActiveBUTT Then
            AudioControl.MyMixer.Channels(AudioChannelIDs.EmberPC).Muted = False
            AudioControl.MyMixer.Channels(AudioChannelIDs.EmberMic).Muted = False
            AudioControl.MyMixer.Channels(AudioChannelIDs.EmberPC).ApplySettings()
            AudioControl.MyMixer.Channels(AudioChannelIDs.EmberMic).ApplySettings()
        Else
            AudioControl.MyMixer.Channels(AudioChannelIDs.EmberPC).Muted = True
            AudioControl.MyMixer.Channels(AudioChannelIDs.EmberMic).Muted = True
            AudioControl.MyMixer.Channels(AudioChannelIDs.EmberPC).ApplySettings()
            AudioControl.MyMixer.Channels(AudioChannelIDs.EmberMic).ApplySettings()
        End If
        ImportSingle(AudioChannelIDs.EmberPC)
        ImportSingle(AudioChannelIDs.EmberMic, True)
    End Sub

    Private Sub LUNAmute_Click(sender As Object, e As EventArgs) Handles LUNAmute.Click
        If LUNAmute.BackColor = ActiveBUTT Then
            AudioControl.MyMixer.Channels(AudioChannelIDs.LunaPC).Muted = False
            AudioControl.MyMixer.Channels(AudioChannelIDs.LunaMic).Muted = False
            AudioControl.MyMixer.Channels(AudioChannelIDs.LunaPC).ApplySettings()
            AudioControl.MyMixer.Channels(AudioChannelIDs.LunaMic).ApplySettings()
        Else
            AudioControl.MyMixer.Channels(AudioChannelIDs.LunaPC).Muted = True
            AudioControl.MyMixer.Channels(AudioChannelIDs.LunaMic).Muted = True
            AudioControl.MyMixer.Channels(AudioChannelIDs.LunaPC).ApplySettings()
            AudioControl.MyMixer.Channels(AudioChannelIDs.LunaMic).ApplySettings()
        End If
        ImportSingle(AudioChannelIDs.LunaPC)
        ImportSingle(AudioChannelIDs.LunaMic, True)
    End Sub

    Private Sub PCmute_Click(sender As Object, e As EventArgs) Handles PCmute.Click
        If PCmute.BackColor = ActiveBUTT Then
            AudioControl.MyMixer.Channels(AudioChannelIDs.LunaPC).Muted = False
            AudioControl.MyMixer.Channels(AudioChannelIDs.EmberPC).Muted = False
            AudioControl.MyMixer.Channels(AudioChannelIDs.LunaPC).ApplySettings()
            AudioControl.MyMixer.Channels(AudioChannelIDs.EmberPC).ApplySettings()
        Else
            AudioControl.MyMixer.Channels(AudioChannelIDs.LunaPC).Muted = True
            AudioControl.MyMixer.Channels(AudioChannelIDs.EmberPC).Muted = True
            AudioControl.MyMixer.Channels(AudioChannelIDs.LunaPC).ApplySettings()
            AudioControl.MyMixer.Channels(AudioChannelIDs.EmberPC).ApplySettings()
        End If
        ImportSingle(AudioChannelIDs.LunaPC)
        ImportSingle(AudioChannelIDs.EmberPC, True)
    End Sub

    Private Sub MICmute_Click(sender As Object, e As EventArgs) Handles MICmute.Click
        If MICmute.BackColor = ActiveBUTT Then
            AudioControl.MyMixer.Channels(AudioChannelIDs.EmberMic).Muted = False
            AudioControl.MyMixer.Channels(AudioChannelIDs.LunaMic).Muted = False
            AudioControl.MyMixer.Channels(AudioChannelIDs.EmberMic).ApplySettings()
            AudioControl.MyMixer.Channels(AudioChannelIDs.LunaMic).ApplySettings()
        Else
            AudioControl.MyMixer.Channels(AudioChannelIDs.EmberMic).Muted = True
            AudioControl.MyMixer.Channels(AudioChannelIDs.LunaMic).Muted = True
            AudioControl.MyMixer.Channels(AudioChannelIDs.EmberMic).ApplySettings()
            AudioControl.MyMixer.Channels(AudioChannelIDs.LunaMic).ApplySettings()
        End If
        ImportSingle(AudioChannelIDs.EmberMic)
        ImportSingle(AudioChannelIDs.LunaMic, True)
    End Sub

End Class