Public Class ChannelPointsForm

    Private DisplayTotal As Integer
    Private TotalRewards As Integer = 0
    Private SourceIndex As Integer = 0
    Private SizeIncrement As Integer = 45
    Private ButtonsWide As Integer = 1
    Private ButtonsHigh As Integer = 12
    Private BaseHeight As Integer = 130
    Private RewardEditor As ChannelPointsEditor

    Private Sub ChannelPointsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RewardEditor = New ChannelPointsEditor
        DisplayTotal = ButtonsWide * ButtonsHigh
        SourceWindow.CHANNEL_POINTS.BackColor = ActiveBUTT
        DisplayRewards()
        AddHandler ChannelPoints.AllRewardsUpdated, AddressOf RemoteDisplayAll
        AddHandler ChannelPoints.SingleRewardUpdated, AddressOf RemoteDisplaySingle
    End Sub

    Private Sub ChannelPointsForm_Closing(sender As Object, e As EventArgs) Handles MyBase.Closing
        RemoveHandler ChannelPoints.AllRewardsUpdated, AddressOf RemoteDisplayAll
        RemoveHandler ChannelPoints.SingleRewardUpdated, AddressOf RemoteDisplaySingle
        SourceWindow.CHANNEL_POINTS.BackColor = StandardBUTT
    End Sub

    Private Sub RemoteDisplayAll()
        SourceIndex = 0
        BeginInvoke(Sub() DisplayRewards())
    End Sub

    Private Sub DisplayRewards()
        SetDrawing(Me.Handle, WM_SETREDRAW, False, 0)

        TotalRewards = ChannelPoints.Rewards.Length
        Dim StartIndex As Integer = SourceIndex * DisplayTotal
        For i As Integer = 0 To DisplayTotal - 1
            DisplaySingle(i)
        Next
        Dim InputSize As Size = Me.Size
        InputSize.Height = GetFormHeight(TotalRewards - StartIndex, ButtonsWide, ButtonsHigh, BaseHeight, SizeIncrement)
        Me.Size = InputSize
        SetDrawing(Me.Handle, WM_SETREDRAW, True, 0)
        Me.Refresh()
    End Sub

    Private Sub RemoteDisplaySingle(ActualIndex As Integer)
        Dim DisplayIndex As Integer = ActualIndex - (SourceIndex * DisplayTotal)
        If DisplayIndex > -1 And DisplayIndex < DisplayTotal Then
            BeginInvoke(Sub() DisplaySingle(DisplayIndex))
        End If
    End Sub

    Private Sub DisplaySingle(DisplayIndex As Integer)
        Dim ActualIndex As Integer = DisplayIndex + (SourceIndex * DisplayTotal)
        Dim RewardDisplay As GroupBox = CType(Controls("Redemption" & DisplayIndex), GroupBox)
        If ActualIndex < TotalRewards Then
            RewardDisplay.Visible = True
            Dim EnableButt As Button = CType(RewardDisplay.Controls("Enabled" & DisplayIndex), Button)
            Dim CategoryLab As Label = CType(RewardDisplay.Controls("Category" & DisplayIndex), Label)
            Dim TitleLab As Label = CType(RewardDisplay.Controls("Title" & DisplayIndex), Label)
            Dim CostLab As Label = CType(RewardDisplay.Controls("Cost" & DisplayIndex), Label)
            Dim BGcolorLab As Label = CType(RewardDisplay.Controls("BGcolor" & DisplayIndex), Label)
            If ChannelPoints.Rewards(ActualIndex).TwitchData.IsEnabled = True Then
                EnableButt.BackColor = ActiveBUTT
                EnableButt.Text = "ON"
            Else
                EnableButt.BackColor = StandardBUTT
                EnableButt.Text = "OFF"
            End If
            CategoryLab.Text = TypeName(ChannelPoints.Rewards(ActualIndex).Type)
            TitleLab.Text = ChannelPoints.Rewards(ActualIndex).TwitchData.Title
            BGcolorLab.BackColor = ColorTranslator.FromHtml(ChannelPoints.Rewards(ActualIndex).TwitchData.BackgroundColor)
            CostLab.Text = ChannelPoints.Rewards(ActualIndex).TwitchData.Cost
        Else
            RewardDisplay.Visible = False
        End If
    End Sub

    Private Sub SKIPFBUTT_Click(sender As Object, e As EventArgs) Handles SKIPFBUTT.Click
        IncreaseRewardIndex()
    End Sub
    Private Sub SKIPBBUTT_Click(sender As Object, e As EventArgs) Handles SKIPBBUTT.Click
        DecreaseRewardIndex()
    End Sub

    Private Sub IncreaseRewardIndex()
        If TotalRewards > ((SourceIndex + 1) * DisplayTotal) Then
            SourceIndex = SourceIndex + 1
            DisplayRewards()
        End If
    End Sub

    Private Sub DecreaseRewardIndex()
        If SourceIndex > 0 And AudioControl.SoundPlayer.Active = False Then
            SourceIndex = SourceIndex - 1
            DisplayRewards()
        End If
    End Sub

    Private Sub EnableClick(DisplayIndex As Integer)
        Dim ActualIndex As Integer = DisplayIndex + (SourceIndex * DisplayTotal)
        ChannelPoints.ToggleRewardEnable(ActualIndex)
    End Sub
    Private Sub Enabled0_Click(sender As Object, e As EventArgs) Handles Enabled0.Click
        EnableClick(0)
    End Sub

    Private Sub Enabled1_Click(sender As Object, e As EventArgs) Handles Enabled1.Click
        EnableClick(1)
    End Sub

    Private Sub Enabled2Click(sender As Object, e As EventArgs) Handles Enabled2.Click
        EnableClick(2)
    End Sub
    Private Sub Enabled3_Click(sender As Object, e As EventArgs) Handles Enabled3.Click
        EnableClick(3)
    End Sub
    Private Sub Enabled4_Click(sender As Object, e As EventArgs) Handles Enabled4.Click
        EnableClick(4)
    End Sub
    Private Sub Enabled5_Click(sender As Object, e As EventArgs) Handles Enabled5.Click
        EnableClick(5)
    End Sub
    Private Sub Enabled6_Click(sender As Object, e As EventArgs) Handles Enabled6.Click
        EnableClick(6)
    End Sub
    Private Sub Enabled7_Click(sender As Object, e As EventArgs) Handles Enabled7.Click
        EnableClick(7)
    End Sub
    Private Sub Enabled8_Click(sender As Object, e As EventArgs) Handles Enabled8.Click
        EnableClick(8)
    End Sub
    Private Sub Enabled9_Click(sender As Object, e As EventArgs) Handles Enabled9.Click
        EnableClick(9)
    End Sub
    Private Sub Enabled10_Click(sender As Object, e As EventArgs) Handles Enabled10.Click
        EnableClick(10)
    End Sub
    Private Sub Enabled11_Click(sender As Object, e As EventArgs) Handles Enabled11.Click
        EnableClick(11)
    End Sub

    Private Sub DclickCHPR(DisplayIndex As Integer)
        If RewardEditor.Visible = False Then
            Dim ActualIndex As Integer = DisplayIndex + (SourceIndex * DisplayTotal)
            RewardEditor = New ChannelPointsEditor
            RewardEditor.RewardIndex = ActualIndex
            RewardEditor.Show()
        End If
    End Sub

    Private Sub Title0_DoubleClick(sender As Object, e As EventArgs) Handles Title0.DoubleClick
        DclickCHPR(0)
    End Sub
    Private Sub Title1_DoubleClick(sender As Object, e As EventArgs) Handles Title1.DoubleClick
        DclickCHPR(1)
    End Sub
    Private Sub Title2_DoubleClick(sender As Object, e As EventArgs) Handles Title2.DoubleClick
        DclickCHPR(2)
    End Sub
    Private Sub Title3_DoubleClick(sender As Object, e As EventArgs) Handles Title3.DoubleClick
        DclickCHPR(3)
    End Sub
    Private Sub Title4_DoubleClick(sender As Object, e As EventArgs) Handles Title4.DoubleClick
        DclickCHPR(4)
    End Sub
    Private Sub Title5_DoubleClick(sender As Object, e As EventArgs) Handles Title5.DoubleClick
        DclickCHPR(5)
    End Sub
    Private Sub Title6_DoubleClick(sender As Object, e As EventArgs) Handles Title6.DoubleClick
        DclickCHPR(6)
    End Sub
    Private Sub Title7_DoubleClick(sender As Object, e As EventArgs) Handles Title7.DoubleClick
        DclickCHPR(7)
    End Sub
    Private Sub Title8_DoubleClick(sender As Object, e As EventArgs) Handles Title8.DoubleClick
        DclickCHPR(8)
    End Sub
    Private Sub Title9_DoubleClick(sender As Object, e As EventArgs) Handles Title9.DoubleClick
        DclickCHPR(9)
    End Sub
    Private Sub Title10_DoubleClick(sender As Object, e As EventArgs) Handles Title10.DoubleClick
        DclickCHPR(10)
    End Sub
    Private Sub Title11_DoubleClick(sender As Object, e As EventArgs) Handles Title11.DoubleClick
        DclickCHPR(11)
    End Sub

    Private Sub ADD_Click2(sender As Object, e As EventArgs) Handles ADD.Click
        If RewardEditor.Visible = False Then
            RewardEditor = New ChannelPointsEditor
            RewardEditor.Show()
        End If
    End Sub

    Private Sub DeleteButt(DisplayIndex As Integer)
        Dim Response As MsgBoxResult = MsgBox("Are you sure about that?", MsgBoxStyle.OkCancel, "hmmm..")
        If Response = MsgBoxResult.Ok Then
            Dim ActualIndex As Integer = DisplayIndex + (SourceIndex * DisplayTotal)
            ChannelPoints.DeleteRward(ActualIndex)
        End If
    End Sub

    Private Sub Delete0_Click(sender As Object, e As EventArgs) Handles Delete0.Click
        DeleteButt(0)
    End Sub
    Private Sub Delete1_Click(sender As Object, e As EventArgs) Handles Delete1.Click
        DeleteButt(1)
    End Sub
    Private Sub Delete2_Click(sender As Object, e As EventArgs) Handles Delete2.Click
        DeleteButt(2)
    End Sub
    Private Sub Delete3_Click(sender As Object, e As EventArgs) Handles Delete3.Click
        DeleteButt(3)
    End Sub
    Private Sub Delete4_Click(sender As Object, e As EventArgs) Handles Delete4.Click
        DeleteButt(4)
    End Sub
    Private Sub Delete5_Click(sender As Object, e As EventArgs) Handles Delete5.Click
        DeleteButt(5)
    End Sub
    Private Sub Delete6_Click(sender As Object, e As EventArgs) Handles Delete6.Click
        DeleteButt(6)
    End Sub
    Private Sub Delete7_Click(sender As Object, e As EventArgs) Handles Delete7.Click
        DeleteButt(7)
    End Sub
    Private Sub Delete8_Click(sender As Object, e As EventArgs) Handles Delete8.Click
        DeleteButt(8)
    End Sub
    Private Sub Delete9_Click(sender As Object, e As EventArgs) Handles Delete9.Click
        DeleteButt(9)
    End Sub
    Private Sub Delete10_Click(sender As Object, e As EventArgs) Handles Delete10.Click
        DeleteButt(10)
    End Sub
    Private Sub Delete11_Click(sender As Object, e As EventArgs) Handles Delete11.Click
        DeleteButt(11)
    End Sub

End Class