Public Class StreamStarter
    Private Sub StreamStarter_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AddHandler myAPI.StreamInfoUpdated, AddressOf LetsaGo
        StrimTitle.Text = GamesList.StreamTitle
        StrimCat.Text = GamesList.GetGameName
        For i As Integer = 0 To GamesList.Games.Length - 1
            StrimCat.Items.Add(GamesList.Games(i).Name)
        Next
    End Sub

    Public Sub LetsaGo()
        SourceWindow.BeginInvoke(Sub() SourceWindow.ChatReady())
        OBS.StartStreaming()
TryAgain:
        Try
            BeginInvoke(Sub() Close())
        Catch
            GoTo TryAgain
        End Try
    End Sub

    Private Sub StartBUTT_Click(sender As Object, e As EventArgs) Handles StartBUTT.Click
        If StrimTitle.Text <> "" And StrimCat.Text <> "" Then
            StartBUTT.Enabled = False
            GamesList.StreamTitle = StrimTitle.Text
            GamesList.SetGameIndexByName(StrimCat.Text)
            If UsrReset.Checked = True Then
                ChatUserInfo.ResetCurrentUsers()
            End If
            myAPI.SetStreamInfoSub()
        End If
    End Sub
End Class