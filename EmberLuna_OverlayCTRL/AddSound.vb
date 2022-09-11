Imports System.IO.Ports

Public Class AddSound


    Public SoundIndex As Integer = -1, CatIndex As Integer = -1, ButtonIndex As Integer = -1, ButtData As SoundButt
    Private WithEvents MySound As SoundController


    Private Sub OBSSoundEditor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MySound = New SoundController(SoundSource.SFX, "Sound Editor")
        SetDrawing(Me.Handle, WM_SETREDRAW, False, 0)
        For i As Integer = 0 To AudioControl.SoundList.Count - 1
            SoundPicker.Items.Add(AudioControl.SoundList(i))
        Next
        If SoundIndex = -1 Then
            ButtData.InitializeSoundFile()
        Else
            ButtData = AudioControl.SoundFiles(SoundIndex)
        End If
        ReadButtData()
        SetDrawing(Me.Handle, WM_SETREDRAW, True, 0)
        Me.Refresh()
        'End If
    End Sub

    Private Sub OBSSoundEditor_Closing(sender As Object, e As FormClosingEventArgs) Handles MyBase.Closing
    End Sub

    Private Sub SoundPlayerActive() Handles MySound.SoundStarted
        Button4.BackgroundImage = My.Resources._STOP
        Button4.BackColor = ActiveBUTT
    End Sub
    Private Sub SoundPlayerInActive() Handles MySound.SoundStopped
        Button4.BackgroundImage = My.Resources.play
        Button4.BackColor = StandardBUTT
    End Sub

    Private Sub ReadButtData()
        SoundName.Text = ButtData.Name
        SoundTitle.Text = ButtData.Title
        SoundPublic.Checked = ButtData.PublicBool
        SoundMute.Checked = ButtData.MuteMusic
        SoundRando.Checked = ButtData.Randomize
        If ButtData.Sounds.Count <> 0 Then
            SoundListBox.Text = String.Join(vbCrLf, ButtData.Sounds)
        End If
        SoundButtPreview.BackColor = ButtData.SoundColor
    End Sub

    Private Sub GetSoundList()
        Dim OutputList As New List(Of String)
        If SoundListBox.Text <> "" Then
            If InStr(SoundListBox.Text, vbCrLf) <> 0 Then
                Dim InputSTring() As String = Split(SoundListBox.Text, vbCrLf)
                For i As Integer = 0 To InputSTring.Length - 1
                    OutputList.Add(InputSTring(i))
                Next
            Else
                OutputList.Add(SoundListBox.Text)
            End If
        End If
        ButtData.Sounds = OutputList
    End Sub

    Private Sub SoundListBox_Click(sender As Object, e As EventArgs) Handles SoundListBox.Click
        TextboxLineSelector(sender)
    End Sub

    Private Sub SoundListBox_DClick(sender As Object, e As EventArgs) Handles SoundListBox.DoubleClick
        Dim Soundstring As String = TextboxLineSelector(sender)
        If Soundstring <> "" Then Dim STASK As Task = MySound.PlaySound(Soundstring)
    End Sub

    Private Sub CHPcolor_Click(sender As Object, e As EventArgs) Handles ColorPicker.Click
        Dim Input As ColorDialog = New ColorDialog
        Input.Color = SoundButtPreview.BackColor
        If Input.ShowDialog() = DialogResult.OK Then
            SoundButtPreview.BackColor = Input.Color
            ButtData.SoundColor = Input.Color
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        TextBoxLineRemover(SoundListBox)
    End Sub

    Private Sub SoundName_TextChanged(sender As Object, e As EventArgs) Handles SoundName.TextChanged
        If SoundName.Text <> "" Then
            GoButt.BackColor = ActiveBUTT
        Else
            GoButt.BackColor = StandardBUTT
        End If
        ButtData.Name = SoundName.Text
    End Sub

    Private Sub GoButt_Click(sender As Object, e As EventArgs) Handles GoButt.Click
        If SoundName.Text <> "" Then
            GetSoundList()
            If SoundIndex = -1 Then
                If AudioControl.CheckSoundFileNames(SoundName.Text) Then
                    AudioControl.AddSoundFile(ButtData)
                    If CatIndex > -1 And ButtonIndex > -1 Then
                        AudioControl.UpdateSoundBoardButton(CatIndex, ButtonIndex, ButtData.Name)
                    End If
                    Close()
                Else
                    SendMessage("Sound Name Must be Unique", "I CAN'T LET YOU DO THAT")
                End If
            Else
                If AudioControl.SoundFiles(SoundIndex).Name <> ButtData.Name Then
                    If AudioControl.CheckSoundFileNames(SoundName.Text) Then
                        AudioControl.UpdateSoundFile(SoundIndex, ButtData)
                        Close()
                    Else
                        SendMessage("Sound Name Must be Unique", "I CAN'T LET YOU DO THAT")
                    End If
                Else
                    AudioControl.UpdateSoundFile(SoundIndex, ButtData)
                    Close()
                End If
            End If
        Else
            SendMessage("Sound Name Can't be Blank", "I CAN'T LET YOU DO THAT")
        End If
    End Sub

    Private Sub SoundTitle_TextChanged(sender As Object, e As EventArgs) Handles SoundTitle.TextChanged
        SoundButtPreview.Text = SoundTitle.Text
        ButtData.Title = SoundTitle.Text
    End Sub

    Private Sub SoundPublic_CheckedChanged(sender As Object, e As EventArgs) Handles SoundPublic.CheckedChanged
        ButtData.PublicBool = SoundPublic.Checked
    End Sub

    Private Sub SoundMute_CheckedChanged(sender As Object, e As EventArgs) Handles SoundMute.CheckedChanged
        ButtData.MuteMusic = SoundMute.Checked
    End Sub

    Private Sub SoundRando_CheckedChanged(sender As Object, e As EventArgs) Handles SoundRando.CheckedChanged
        ButtData.Randomize = SoundRando.Checked
    End Sub

    Private Sub SoundButtPreview_Click(sender As Object, e As EventArgs) Handles SoundButtPreview.Click
        GetSoundList()
        Dim SoundFile As String = ButtData.GetSoundFile
        If SoundFile <> "" Then Dim STASK As Task = MySound.PlaySound(SoundFile)
    End Sub

    Private Sub SoundPicker_SelectedIndexChanged(sender As Object, e As EventArgs) Handles SoundPicker.SelectedIndexChanged

    End Sub

    Private Sub SoundListBox_TextChanged(sender As Object, e As EventArgs) Handles SoundListBox.TextChanged
        TextBoxScrollToEnd(sender)
    End Sub

    Private Sub FilterSoundsButt_Click(sender As Object, e As EventArgs) Handles FilterSoundsButt.Click
        If FilterSoundsButt.BackColor = StandardBUTT Then
            FilterSoundsButt.BackColor = ActiveBUTT
            SoundPicker.Items.Clear()
            Dim InputList As List(Of String) = AudioControl.FilterSoundFilesByUnused()
            For i As Integer = 0 To InputList.Count - 1
                SoundPicker.Items.Add(InputList(i))
            Next
        Else
            FilterSoundsButt.BackColor = StandardBUTT
            SoundPicker.Items.Clear()
            For i As Integer = 0 To AudioControl.SoundList.Count - 1
                SoundPicker.Items.Add(AudioControl.SoundList(i))
            Next
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If MySound.IsActive Then
            Dim Stask As Task = MySound.StopSound()
        Else
            If SoundPicker.Text <> "" Then Dim SoundTask As Task = MySound.PlaySound(SoundPicker.Text)
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If SoundPicker.Text <> "" Then
            If SoundListBox.Text = "" Then
                SoundListBox.Text = SoundPicker.Text
            Else
                SoundListBox.Text = SoundListBox.Text & vbCrLf & SoundPicker.Text
            End If
            If SoundName.Text = "" Then
                Dim outputtext As String = Replace(SoundPicker.Text, ".wav", "")
                outputtext = Replace(outputtext, ".mp3", "")
                SoundName.Text = outputtext
            End If
        End If
    End Sub

End Class