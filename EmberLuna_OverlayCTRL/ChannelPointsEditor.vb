Imports TwitchLib.Api
Public Class ChannelPointsEditor
    Public RewardIndex As Integer = -1, Loaded As Boolean = False

    Private Sub ChannelPointsEditor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If RewardIndex > -1 Then
            CHPcost.Value = ChannelPoints.Rewards(RewardIndex).TwitchData.Cost
            CHPtitle.Text = ChannelPoints.Rewards(RewardIndex).TwitchData.Title
            CHPprompt.Text = ChannelPoints.Rewards(RewardIndex).TwitchData.Prompt
            If ChannelPoints.Rewards(RewardIndex).TwitchData.GlobalCooldownSetting.IsEnabled Then
                CHPcooldown.Value = ChannelPoints.Rewards(RewardIndex).TwitchData.GlobalCooldownSetting.GlobalCooldownSeconds
            Else
                CHPcooldown.Value = 0
            End If
            If ChannelPoints.Rewards(RewardIndex).TwitchData.MaxPerStreamSetting.IsEnabled Then
                CHPmps.Value = ChannelPoints.Rewards(RewardIndex).TwitchData.MaxPerStreamSetting.MaxPerStream
            Else
                CHPmps.Value = 0
            End If
            If ChannelPoints.Rewards(RewardIndex).TwitchData.MaxPerUserPerStreamSetting.IsEnabled Then
                CHPmpups.Value = ChannelPoints.Rewards(RewardIndex).TwitchData.MaxPerUserPerStreamSetting.MaxPerUserPerStream
            Else
                CHPmpups.Value = 0
            End If

            If ChannelPoints.Rewards(RewardIndex).TwitchData.IsEnabled Then
                CHPenable.Text = "ENABLED"
                CHPenable.BackColor = ActiveBUTT
            Else
                CHPenable.Text = "DISABLED"
                CHPenable.BackColor = StandardBUTT
            End If

            CHPuserinput.Checked = ChannelPoints.Rewards(RewardIndex).TwitchData.IsUserInputRequired
            CHPskipqueue.Checked = ChannelPoints.Rewards(RewardIndex).TwitchData.ShouldRedemptionsSkipQueue

            ProgramDependant.Checked = ChannelPoints.Rewards(RewardIndex).IsProgramDependant
            AutoEnable.Checked = ChannelPoints.Rewards(RewardIndex).IsAutoEnabled

            CHPskipqueue.Checked = ChannelPoints.Rewards(RewardIndex).TwitchData.ShouldRedemptionsSkipQueue
            CHPcolor.BackColor = ColorTranslator.FromHtml(ChannelPoints.Rewards(RewardIndex).TwitchData.BackgroundColor)
        Else
            ProgramDependant.Visible = False
            AutoEnable.Visible = False
        End If
        Loaded = True
    End Sub

    Private Sub CHPcolor_Click(sender As Object, e As EventArgs) Handles CHPcolor.Click
        Dim Input As ColorDialog = New ColorDialog
        Input.Color = CHPcolor.BackColor
        If Input.ShowDialog() = DialogResult.OK Then
            CHPcolor.BackColor = Input.Color
        End If
    End Sub

    Private Sub CHPenable_Click(sender As Object, e As EventArgs) Handles CHPenable.Click
        If CHPenable.Text = "DISABLED" Then
            CHPenable.Text = "ENABLED"
            CHPenable.BackColor = ActiveBUTT
        Else
            CHPenable.Text = "DISABLED"
            CHPenable.BackColor = StandardBUTT
        End If
    End Sub

    Private Sub APPLYBUTT_Click(sender As Object, e As EventArgs) Handles APPLYBUTT.Click
        If APPLYBUTT.BackColor = ActiveBUTT Then
            If RewardIndex > -1 Then
                UpdateReward()
            Else
                CreateReward()
            End If
            Me.Close()
        Else
            SendMessage("TITLE and PROMPT can't be blank, dummy", "YOU DONE MESSED UP")
        End If
    End Sub

    Private Sub UpdateReward()
        Dim Request As New Helix.Models.ChannelPoints.UpdateCustomReward.UpdateCustomRewardRequest
        If CHPenable.Text = "DISABLED" Then
            Request.IsEnabled = False
        Else
            Request.IsEnabled = True
        End If

        'Request.BackgroundColor = ColorTranslator.ToHtml(CHPcolor.BackColor)

        If CHPcooldown.Value <> 0 Then
            Request.IsGlobalCooldownEnabled = True
            Request.GlobalCooldownSeconds = CHPcooldown.Value
        Else
            Request.IsGlobalCooldownEnabled = False
            Request.GlobalCooldownSeconds = 0
        End If

        If CHPmps.Value > 0 Then
            Request.IsMaxPerStreamEnabled = True
            Request.MaxPerStream = CHPmps.Value
        Else
            Request.IsMaxPerStreamEnabled = False
            Request.MaxPerStream = 0
        End If
        If CHPmpups.Value > 0 Then
            Request.IsMaxPerUserPerStreamEnabled = True
            Request.MaxPerUserPerStream = CHPmpups.Value
        Else
            Request.IsMaxPerUserPerStreamEnabled = False
            Request.MaxPerUserPerStream = 0
        End If

        Request.Cost = CHPcost.Value
        Request.Prompt = CHPprompt.Text
        Request.Title = CHPtitle.Text
        Request.IsUserInputRequired = CHPuserinput.Checked
        Request.ShouldRedemptionsSkipRequestQueue = CHPskipqueue.Checked
        ChannelPoints.UpdateReward(RewardIndex, Request)
    End Sub

    Private Sub CreateReward()

        Dim Request As New Helix.Models.ChannelPoints.CreateCustomReward.CreateCustomRewardsRequest

        If CHPenable.Text = "DISABLED" Then
            Request.IsEnabled = False
        Else
            Request.IsEnabled = True
        End If


        'This doesn't work, but i don't know why?!
        'Request.BackgroundColor = ColorTranslator.ToHtml(CHPcolor.BackColor)

        If CHPcooldown.Value <> 0 Then
            Request.IsGlobalCooldownEnabled = True
            Request.GlobalCooldownSeconds = CHPcooldown.Value
        Else
            Request.IsGlobalCooldownEnabled = False
            Request.GlobalCooldownSeconds = 0
        End If

        'These values don't work either... Fuck'em  (Trying again>>>)

        If CHPmps.Value <> 0 Then
            Request.IsMaxPerStreamEnabled = True
            Request.MaxPerStream = CHPmps.Value
        Else
            Request.IsMaxPerStreamEnabled = False
            Request.MaxPerStream = 0
        End If

        If CHPmpups.Value <> 0 Then
            Request.IsMaxPerUserPerStreamEnabled = True
            Request.MaxPerUserPerStream = CHPmpups.Value
        Else
            Request.IsMaxPerStreamEnabled = False
            Request.MaxPerStream = 0
        End If

        Request.Cost = CHPcost.Value
        Request.Prompt = CHPprompt.Text
        Request.Title = CHPtitle.Text
        Request.IsUserInputRequired = CHPuserinput.Checked
        Request.ShouldRedemptionsSkipRequestQueue = CHPskipqueue.Checked
        myAPI.CreateChannelPointReward(Request)
    End Sub

    Private Sub CANCELBUTT_Click(sender As Object, e As EventArgs) Handles CANCELBUTT.Click
        Me.Close()
    End Sub

    Private Sub CHPtitle_TextChanged(sender As Object, e As EventArgs) Handles CHPtitle.TextChanged
        If CHPtitle.Text <> "" And CHPprompt.Text <> "" Then APPLYBUTT.BackColor = ActiveBUTT
    End Sub

    Private Sub CHPprompt_TextChanged(sender As Object, e As EventArgs) Handles CHPprompt.TextChanged
        If CHPtitle.Text <> "" And CHPprompt.Text <> "" Then APPLYBUTT.BackColor = ActiveBUTT
    End Sub

    Private Sub ProgramDependant_CheckedChanged(sender As Object, e As EventArgs) Handles ProgramDependant.CheckedChanged
        If Loaded And RewardIndex > -1 Then
            ChannelPoints.Rewards(RewardIndex).IsProgramDependant = ProgramDependant.Checked
            ChannelPoints.Rewards(RewardIndex).SyncLocalData()
        End If
    End Sub
    Private Sub AutoEnable_CheckedChanged(sender As Object, e As EventArgs) Handles AutoEnable.CheckedChanged
        If Loaded And RewardIndex > -1 Then
            ChannelPoints.Rewards(RewardIndex).IsAutoEnabled = AutoEnable.Checked
            ChannelPoints.Rewards(RewardIndex).SyncLocalData()
        End If
    End Sub
End Class