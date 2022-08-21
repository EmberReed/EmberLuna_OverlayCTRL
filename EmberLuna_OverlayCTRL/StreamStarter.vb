Public Class StreamStarter

    Private Starting As Boolean = False
    Private Sub StreamStarter_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        StrimTitle.Text = GamesList.StreamTitle
        StrimCat.Text = GamesList.GetGameName
        For i As Integer = 0 To GamesList.Games.Length - 1
            StrimCat.Items.Add(GamesList.Games(i).Name)
        Next
    End Sub

    Private Async Function StartMyStream() As Task
        If StrimTitle.Text <> "" And StrimCat.Text <> "" Then
            StartBUTT.Enabled = False
            GamesList.StreamTitle = StrimTitle.Text
            GamesList.SetGameIndexByName(StrimCat.Text)
            If UsrReset.Checked = True Then
                ChatUserInfo.ResetCurrentUsers()
            End If
            Await myAPI.SetStreamInfo()
            OBS.StartStreaming()
            Starting = True
            Close()
        Else
            SendMessage("Title and Category can't be blank")
        End If
    End Function

    Private Sub StreamStarter_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Starting = False Then SourceWindow.Button13.Enabled = True
    End Sub

    Private Sub StartBUTT_Click(sender As Object, e As EventArgs) Handles StartBUTT.Click
        Dim StartTask As Task = StartMyStream()
    End Sub
End Class