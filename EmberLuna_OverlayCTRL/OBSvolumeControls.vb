Imports System.Math
Public Class OBSvolumeControls
    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        ProgressBar1.Value = 100 * (10 ^ ((NumericUpDown1.Value - 100) / 50))
        Label1.Text = "EMBER MIC (" & Round(NumericUpDown1.Value - 100, 1) & "dB)"
        If SourceWindow.Button11.BackColor = ActiveBUTT Then
            SetVolume({"Sounds0"}, Round(NumericUpDown1.Value - 100, 1), False)
        End If
    End Sub

    Private Sub ProgressBar1_Click(sender As Object, e As MouseEventArgs) Handles ProgressBar1.MouseClick, ProgressBar1.Click
        Dim Xpercent As Double = e.X / ProgressBar1.Size.Width
        Xpercent = 100 + (50 * Log10(Xpercent))
        If Xpercent < 0 Then Xpercent = 0
        NumericUpDown1.Value = Round(Xpercent, 1)
    End Sub

    Private Sub OBSvolumeControls_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub OBSvolumeControls_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        SourceWindow.Button11.BackColor = ActiveBUTT
    End Sub

    Private Sub OBSvolumeControls_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        SourceWindow.Button11.BackColor = StandardBUTT
    End Sub

    Private Sub ProgressBar1_Click(sender As Object, e As EventArgs)

    End Sub
End Class