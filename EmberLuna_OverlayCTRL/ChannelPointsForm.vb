Public Class ChannelPointsForm

    Private DisplayTotal As Integer
    Private TotalRewards As Integer = 0
    Private SourceIndex As Integer = 0
    Private SizeIncrement As Integer = 45
    Private ButtonsWide As Integer = 1
    Private ButtonsHigh As Integer = 12
    Private BaseHeight As Integer = 130
    Private RewardEditor As ChannelPointsEditor
    Private Loaded As Boolean = False
    Private Sub ChannelPointsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RewardEditor = New ChannelPointsEditor

        DisplayTotal = ButtonsWide * ButtonsHigh
        SourceWindow.CHANNEL_POINTS.BackColor = ActiveBUTT
        DisplayRewards()
        AddHandler ChannelPoints.AllRewardsUpdated, AddressOf RemoteDisplayAll
        AddHandler ChannelPoints.SingleRewardUpdated, AddressOf RemoteDisplaySingle
        Loaded = True
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
            Dim TitleLab As Label = CType(RewardDisplay.Controls("Title" & DisplayIndex), Label)
            Dim CostLab As Label = CType(RewardDisplay.Controls("Cost" & DisplayIndex), Label)
            Dim BGcolorLab As Label = CType(RewardDisplay.Controls("BGcolor" & DisplayIndex), Label)
            Dim PGMrequired As CheckBox = CType(RewardDisplay.Controls("PD" & DisplayIndex), CheckBox)
            Dim AutoEnable As CheckBox = CType(RewardDisplay.Controls("AE" & DisplayIndex), CheckBox)

            If ChannelPoints.Rewards(ActualIndex).TwitchData.IsEnabled = True Then
                EnableButt.BackColor = ActiveBUTT
                EnableButt.Text = "ON"
            Else
                EnableButt.BackColor = StandardBUTT
                EnableButt.Text = "OFF"
            End If

            'CategoryLab.Text = TypeName(ChannelPoints.Rewards(ActualIndex).Type)
            TitleLab.Text = ChannelPoints.Rewards(ActualIndex).TwitchData.Title
            PGMrequired.Checked = ChannelPoints.Rewards(ActualIndex).IsProgramDependant
            AutoEnable.Checked = ChannelPoints.Rewards(ActualIndex).IsAutoEnabled

            If PGMrequired.Checked Then
                AutoEnable.Visible = True
            Else
                AutoEnable.Visible = False
            End If
            BGcolorLab.BackColor = ColorTranslator.FromHtml(ChannelPoints.Rewards(ActualIndex).TwitchData.BackgroundColor)
            PGMrequired.BackColor = ColorTranslator.FromHtml(ChannelPoints.Rewards(ActualIndex).TwitchData.BackgroundColor)
            AutoEnable.BackColor = ColorTranslator.FromHtml(ChannelPoints.Rewards(ActualIndex).TwitchData.BackgroundColor)
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
        If SourceIndex > 0 Then
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
    Private Sub PDClick(MyBox As CheckBox)
        If Loaded Then
            Dim DisplayIndex As Integer = Replace(MyBox.Name, "PD", "")
            Dim ActualIndex As Integer = DisplayIndex + (SourceIndex * DisplayTotal)
            ChannelPoints.Rewards(ActualIndex).IsProgramDependant = MyBox.Checked
            ChannelPoints.Rewards(ActualIndex).SyncLocalData()
            Dim RewardBox As GroupBox = CType(Controls("Redemption" & DisplayIndex), GroupBox)
            Dim AEbox As CheckBox = CType(RewardBox.Controls("AE" & DisplayIndex), CheckBox)
            If MyBox.Checked Then
                AEbox.Visible = True
            Else
                AEbox.Visible = False
            End If
        End If
    End Sub
    Private Sub PD0_CheckedChanged(sender As Object, e As EventArgs) Handles PD0.CheckedChanged
        PDClick(sender)
    End Sub
    Private Sub PD1_CheckedChanged(sender As Object, e As EventArgs) Handles PD1.CheckedChanged
        PDClick(sender)
    End Sub
    Private Sub PD2_CheckedChanged(sender As Object, e As EventArgs) Handles PD2.CheckedChanged
        PDClick(sender)
    End Sub
    Private Sub PD3_CheckedChanged(sender As Object, e As EventArgs) Handles PD3.CheckedChanged
        PDClick(sender)
    End Sub
    Private Sub PD4_CheckedChanged(sender As Object, e As EventArgs) Handles PD4.CheckedChanged
        PDClick(sender)
    End Sub
    Private Sub PD5_CheckedChanged(sender As Object, e As EventArgs) Handles PD5.CheckedChanged
        PDClick(sender)
    End Sub
    Private Sub PD6_CheckedChanged(sender As Object, e As EventArgs) Handles PD6.CheckedChanged
        PDClick(sender)
    End Sub
    Private Sub PD7_CheckedChanged(sender As Object, e As EventArgs) Handles PD7.CheckedChanged
        PDClick(sender)
    End Sub
    Private Sub PD8_CheckedChanged(sender As Object, e As EventArgs) Handles PD8.CheckedChanged
        PDClick(sender)
    End Sub
    Private Sub PD9_CheckedChanged(sender As Object, e As EventArgs) Handles PD9.CheckedChanged
        PDClick(sender)
    End Sub
    Private Sub PD10_CheckedChanged(sender As Object, e As EventArgs) Handles PD10.CheckedChanged
        PDClick(sender)
    End Sub
    Private Sub PD11_CheckedChanged(sender As Object, e As EventArgs) Handles PD11.CheckedChanged
        PDClick(sender)
    End Sub


    Private Sub AEClick(MyBox As CheckBox)
        If Loaded Then
            Dim DisplayIndex As Integer = Replace(MyBox.Name, "AE", "")
            Dim ActualIndex As Integer = DisplayIndex + (SourceIndex * DisplayTotal)
            ChannelPoints.Rewards(ActualIndex).IsAutoEnabled = MyBox.Checked
            ChannelPoints.Rewards(ActualIndex).SyncLocalData()
        End If

    End Sub
    Private Sub AE0_CheckedChanged(sender As Object, e As EventArgs) Handles AE0.CheckedChanged
        AEClick(sender)
    End Sub
    Private Sub AE1_CheckedChanged(sender As Object, e As EventArgs) Handles AE1.CheckedChanged
        AEClick(sender)
    End Sub
    Private Sub AE2_CheckedChanged(sender As Object, e As EventArgs) Handles AE2.CheckedChanged
        AEClick(sender)
    End Sub
    Private Sub AE3_CheckedChanged(sender As Object, e As EventArgs) Handles AE3.CheckedChanged
        AEClick(sender)
    End Sub
    Private Sub AE4_CheckedChanged(sender As Object, e As EventArgs) Handles AE4.CheckedChanged
        AEClick(sender)
    End Sub
    Private Sub AE5_CheckedChanged(sender As Object, e As EventArgs) Handles AE5.CheckedChanged
        AEClick(sender)
    End Sub
    Private Sub AE6_CheckedChanged(sender As Object, e As EventArgs) Handles AE6.CheckedChanged
        AEClick(sender)
    End Sub
    Private Sub AE7_CheckedChanged(sender As Object, e As EventArgs) Handles AE7.CheckedChanged
        AEClick(sender)
    End Sub
    Private Sub AE8_CheckedChanged(sender As Object, e As EventArgs) Handles AE8.CheckedChanged
        AEClick(sender)
    End Sub
    Private Sub AE9_CheckedChanged(sender As Object, e As EventArgs) Handles AE9.CheckedChanged
        AEClick(sender)
    End Sub
    Private Sub AE10_CheckedChanged(sender As Object, e As EventArgs) Handles AE10.CheckedChanged
        AEClick(sender)
    End Sub
    Private Sub AE11_CheckedChanged(sender As Object, e As EventArgs) Handles AE11.CheckedChanged
        AEClick(sender)
    End Sub

End Class