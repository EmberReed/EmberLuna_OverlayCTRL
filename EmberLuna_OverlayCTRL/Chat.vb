Public Class Chat
    Private Sub Chat_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SourceWindow.BeginInvoke(Sub() SourceWindow.ChatBUTT.BackColor = ActiveBUTT)

    End Sub

    Private Sub Chat_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        SourceWindow.BeginInvoke(Sub() SourceWindow.ChatBUTT.BackColor = StandardBUTT)
    End Sub

End Class