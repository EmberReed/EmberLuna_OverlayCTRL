Imports System.Threading
Public Class StreamStarter

    Public Sub New(Optional EditMode As Boolean = False)

        InitializeComponent()
        If EditMode = True Then StartBUTT.Text = "UPDATE"

    End Sub

    Private Starting As Boolean = False
    Private Sub StreamStarter_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        StrimTitle.Text = GamesList.StreamTitle
        StrimCat.Text = GamesList.GetGameName
        For i As Integer = 0 To GamesList.Games.Length - 1
            StrimCat.Items.Add(GamesList.Games(i).Name)
        Next
    End Sub

    Private Sub StreamStarter_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Starting = False Then SourceWindow.Button13.Enabled = True
    End Sub

    Private Sub StartBUTT_Click(sender As Object, e As EventArgs) Handles StartBUTT.Click
        If StrimTitle.Text <> "" And StrimCat.Text <> "" Then
            StartBUTT.Enabled = False
            GamesList.StreamTitle = StrimTitle.Text
            GamesList.SetGameIndexByName(StrimCat.Text)
            If UsrReset.Checked = True Then
                ChatUserInfo.ResetCurrentUsers()
            End If
            myAPI.SetStreamInfo()
            If StartBUTT.Text <> "UPDATE" Then
                Thread.Sleep(100)
                OBS.StartStream()
                Starting = True
            End If

            Close()
        Else
            SendMessage("Title and Category can't be blank")
        End If
    End Sub
End Class