Public Class OBSMusicPlayer
    Private Sub OBSMusicPlayer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Updatedisplay()
        AddHandler AudioControl.MusicPlayer.Started, AddressOf UpdateRemote
        AddHandler AudioControl.MusicPlayer.Stopped, AddressOf UpdateRemote
        AddHandler AudioControl.MusicPlayer.Paused, AddressOf UpdateRemote
        SourceWindow.BeginInvoke(Sub() SourceWindow.MUSIC_PLAYER.BackColor = ActiveBUTT)
    End Sub

    Private Sub OBSMusicPlayer_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        RemoveHandler AudioControl.MusicPlayer.Started, AddressOf UpdateRemote
        RemoveHandler AudioControl.MusicPlayer.Stopped, AddressOf UpdateRemote
        RemoveHandler AudioControl.MusicPlayer.Paused, AddressOf UpdateRemote
        SourceWindow.BeginInvoke(Sub() SourceWindow.MUSIC_PLAYER.BackColor = StandardBUTT)
    End Sub


    Public Sub UpdateRemote()
        BeginInvoke(Sub() Updatedisplay())
    End Sub

    Public Sub Updatedisplay()
        SKIPBBUTT.BackColor = StandardBUTT
        SKIPFBUTT.BackColor = StandardBUTT
        PLAYBUTT.BackColor = StandardBUTT
        PAUSEBUTT.BackColor = StandardBUTT
        RPBUTT.BackColor = StandardBUTT
        If AudioControl.MusicPlayer.Active = True Or AudioControl.MusicRunning = True Then
            PLAYBUTT.BackgroundImage = My.Resources._STOP
        Else
            PLAYBUTT.BackgroundImage = My.Resources.play
        End If
        If AudioControl.MusicPlayer.Pause = True Then
            PAUSEBUTT.BackgroundImage = My.Resources.play
        Else
            PAUSEBUTT.BackgroundImage = My.Resources.pause
        End If
        CurrentSong.Text = MPstringFormat(AudioControl.MusicPlayer.Current)
    End Sub

    'Private Function MPstringFormat(Inputstring As String) As String
    'Dim OutputString As String = Replace(Inputstring, ".wav", "")
    '   OutputString = Replace(outputstring, ".mp3", "")
    '  outputstring = Replace(outputstring, "ES_", "")
    'Return outputstring
    'End Function


    Private Sub PLAYBUTT_Click(sender As Object, e As EventArgs) Handles PLAYBUTT.Click
        PLAYBUTT.BackColor = ActiveBUTT
        If AudioControl.MusicPlayer.Active = True Then
            AudioControl.StopMusic()
        Else
            AudioControl.PlayMusic()
        End If
    End Sub
    Private Sub RPBUTT_Click(sender As Object, e As EventArgs) Handles RPBUTT.Click
        RPBUTT.BackColor = ActiveBUTT
        AudioControl.PlayMusic(True)
    End Sub
    Private Sub PAUSEBUTT_Click(sender As Object, e As EventArgs) Handles PAUSEBUTT.Click
        If AudioControl.MusicPlayer.Active = True Then
            PAUSEBUTT.BackColor = ActiveBUTT
            AudioControl.PauseMusic()
        End If
    End Sub

    Private Sub SKIPBBUTT_Click(sender As Object, e As EventArgs) Handles SKIPBBUTT.Click
        SKIPBBUTT.BackColor = ActiveBUTT
        AudioControl.SkipMusicB()
    End Sub

    Private Sub SKIPFBUTT_Click(sender As Object, e As EventArgs) Handles SKIPFBUTT.Click
        SKIPFBUTT.BackColor = ActiveBUTT
        AudioControl.SkipMusicF()
    End Sub

End Class