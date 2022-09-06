Imports System.Math
Public Class SceneSelector
    Private CurrentSceneName As String = ""
    Private OutputSelection As List(Of String)
    Private SceneFile As String = ""
    Private Loaded As Boolean = False
    Private Sub SceneSelector_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ToggleVolumeControls(0)
        SourceWindow.SCENE_SELECTOR.BackColor = ActiveBUTT
        AddHandler OBS.SceneChanged, AddressOf RemoteDisplayChange
        AddHandler MySceneCollection.ScenesUpdated, AddressOf UpdateSceneListBox
        AddHandler AudioControl.MyMixer.MixerChannelChanged, AddressOf ImportSingleR
        UpdateSceneListBox(MySceneCollection.GetSceneList)
        BuildOutputSelection()
        DisplayChange(CurrentScene.Name)
        UpdateScene()
        ImportAll()
        Loaded = True
    End Sub

    Private Sub UpdateScene(Optional SceneName As String = "")
        Dim SceneTask As Task = UpdateSceneDisplay(SceneName)
    End Sub


    Public Sub UpdateSceneListBox(SceneArray() As String)
        If SceneArray IsNot Nothing Then
            SceneListBox.Text = Join(SceneArray, vbCrLf)
        Else
            SceneListBox.Text = ""
        End If
    End Sub

    Private Sub BuildOutputSelection()
        OutputSelection = New List(Of String)
        OutputSelection.Add("Ember's PC")
        OutputSelection.Add("Luna's PC")
        OutputSelection.Add("Ember's Cam")
        OutputSelection.Add("Luna's Cam")
        OutputSelection.Add("Aux In")
        OutputSelection.Add("Gaming Console")
        OutputSelection.Add("Aux Cam 1")
        OutputSelection.Add("Aux Cam 2")
        EscreenSelect.Items.AddRange(OutputSelection.ToArray)
        LscreenSelect.Items.AddRange(OutputSelection.ToArray)
        EcamSelect.Items.AddRange(OutputSelection.ToArray)
        LcamSelect.Items.AddRange(OutputSelection.ToArray)
    End Sub

    Private Sub SceneSelector_Closing(sender As Object, e As EventArgs) Handles MyBase.Closing
        RemoveHandler AudioControl.MyMixer.MixerChannelChanged, AddressOf ImportSingleR
        RemoveHandler OBS.SceneChanged, AddressOf RemoteDisplayChange
        SourceWindow.SCENE_SELECTOR.BackColor = StandardBUTT
    End Sub

    Private Sub RemoteDisplayChange(Sender As Object, SceneName As String)
TryAgain:
        Try
            BeginInvoke(Sub() DisplayChange(SceneName))
        Catch
            GoTo TryAgain
        End Try
    End Sub

    Private Sub DisplayChange(SceneName As String)
        SetDrawing(Me.Handle, WM_SETREDRAW, False, 0)
        If SceneName <> CurrentSceneName Then
            CurrentSceneName = SceneName
            Select Case SceneName
                Case = "Dual Screen Mode"
                    DualScreenBUTT.BackColor = ActiveBUTT
                    CenterScreenBUTT.BackColor = StandardBUTT
                    SingleScreenBUTT.BackColor = StandardBUTT
                    SplitScreenBUTT.BackColor = StandardBUTT
                Case = "Center Screen Mode"
                    DualScreenBUTT.BackColor = StandardBUTT
                    CenterScreenBUTT.BackColor = ActiveBUTT
                    SingleScreenBUTT.BackColor = StandardBUTT
                    SplitScreenBUTT.BackColor = StandardBUTT
                Case = "Single Screen Mode"
                    DualScreenBUTT.BackColor = StandardBUTT
                    CenterScreenBUTT.BackColor = StandardBUTT
                    SingleScreenBUTT.BackColor = ActiveBUTT
                    SplitScreenBUTT.BackColor = StandardBUTT
                Case = "Split Screen Mode"
                    DualScreenBUTT.BackColor = StandardBUTT
                    CenterScreenBUTT.BackColor = StandardBUTT
                    SingleScreenBUTT.BackColor = StandardBUTT
                    SplitScreenBUTT.BackColor = ActiveBUTT
                Case Else
                    DualScreenBUTT.BackColor = StandardBUTT
                    CenterScreenBUTT.BackColor = StandardBUTT
                    SingleScreenBUTT.BackColor = StandardBUTT
                    SplitScreenBUTT.BackColor = StandardBUTT
            End Select
        End If

        If Screen1AwayMode = True And Screen2AwayMode = True Then
            ScreensAway.BackColor = ActiveBUTT
        Else
            ScreensAway.BackColor = StandardBUTT
        End If

        If EmberAwayB = True And LunaAwayB = True Then
            CamsAway.BackColor = ActiveBUTT
        Else
            CamsAway.BackColor = StandardBUTT
        End If

        If EmberCamera = True And LunaCamera = True And EmberSpriteB = True And LunaSpriteB = True Then
            EnableAllBUTT.BackColor = ActiveBUTT
        Else
            EnableAllBUTT.BackColor = StandardBUTT
        End If

        If EmberCamera = False And LunaCamera = False And EmberSpriteB = False And LunaSpriteB = False Then
            DisableAllBUTT.BackColor = ActiveBUTT
        Else
            DisableAllBUTT.BackColor = StandardBUTT
        End If

        If EmberSpriteB = True And LunaSpriteB = True Then
            SpritesBUTT.BackColor = ActiveBUTT
        Else
            SpritesBUTT.BackColor = StandardBUTT
        End If

        If EmberCamera = True And LunaCamera = True Then
            CamsBUTT.BackColor = ActiveBUTT
        Else
            CamsBUTT.BackColor = StandardBUTT
        End If


        If Screen2Enabled = True Then
            'SendMessage("SCREEN2")
            Lscreen.BackColor = ActiveBUTT
            Escreen.BackColor = StandardBUTT
            LunaPIP.BackColor = StandardBUTT
            If PIPenabled Then
                EmberPIP.BackColor = ActiveBUTT
            Else
                EmberPIP.BackColor = StandardBUTT
            End If
        Else
            'SendMessage("SCREEN1")
            Lscreen.BackColor = StandardBUTT
            Escreen.BackColor = ActiveBUTT
            EmberPIP.BackColor = StandardBUTT
            If PIPenabled Then
                LunaPIP.BackColor = ActiveBUTT
            Else
                LunaPIP.BackColor = StandardBUTT
            End If
        End If

        If LunaAwayB = True And Screen2AwayMode = True Then
            LunaAway.BackColor = ActiveBUTT
        Else
            LunaAway.BackColor = StandardBUTT
        End If

        If EmberAwayB = True And Screen1AwayMode = True Then
            EmberAway.BackColor = ActiveBUTT
        Else
            EmberAway.BackColor = StandardBUTT
        End If

        If EmberCamera = True Then
            Ecam.BackColor = ActiveBUTT
        Else
            Ecam.BackColor = StandardBUTT
        End If

        If LunaCamera = True Then
            Lcam.BackColor = ActiveBUTT
        Else
            Lcam.BackColor = StandardBUTT
        End If

        If EmberSpriteB = True Then
            EmberSprite.BackColor = ActiveBUTT
        Else
            EmberSprite.BackColor = StandardBUTT
        End If

        If LunaSpriteB = True Then
            LunaSprite.BackColor = ActiveBUTT
        Else
            LunaSprite.BackColor = StandardBUTT
        End If

        If CurrentSceneName = "Single Screen Mode" And LunaCamera = False And LunaSpriteB = False And Screen2Enabled = False Then
            EmberSolo.BackColor = ActiveBUTT
        Else
            EmberSolo.BackColor = StandardBUTT
        End If

        If EmberCamera = False And EmberSpriteB = False Then
            EmberDisable.BackColor = ActiveBUTT
        Else
            EmberDisable.BackColor = StandardBUTT
        End If

        If EmberCamera = True And EmberSpriteB = True Then
            EmberEnable.BackColor = ActiveBUTT
        Else
            EmberEnable.BackColor = StandardBUTT
        End If

        If CurrentSceneName = "Single Screen Mode" And EmberCamera = False And EmberSpriteB = False And Screen2Enabled = True Then
            LunaSolo.BackColor = ActiveBUTT
        Else
            LunaSolo.BackColor = StandardBUTT
        End If

        If LunaCamera = False And LunaSpriteB = False Then
            LunaDisable.BackColor = ActiveBUTT
        Else
            LunaDisable.BackColor = StandardBUTT
        End If

        If LunaCamera = True And LunaSpriteB = True Then
            LunaEnable.BackColor = ActiveBUTT
        Else
            LunaEnable.BackColor = StandardBUTT
        End If

        EscreenSelect.Text = OutputSelection(Screen1Setting)
        LscreenSelect.Text = OutputSelection(Screen2Setting)
        EcamSelect.Text = OutputSelection(Cam1Setting)
        LcamSelect.Text = OutputSelection(Cam2Setting)

        If Screen1AwayMode = True Then
            EscreenAway.BackColor = ActiveBUTT
        Else
            EscreenAway.BackColor = StandardBUTT
        End If

        If Screen2AwayMode = True Then
            LscreenAway.BackColor = ActiveBUTT
        Else
            LscreenAway.BackColor = StandardBUTT
        End If

        If EmberAwayB = True Then
            EcamAway.BackColor = ActiveBUTT
        Else
            EcamAway.BackColor = StandardBUTT
        End If

        If LunaAwayB = True Then
            LcamAway.BackColor = ActiveBUTT
        Else
            LcamAway.BackColor = StandardBUTT
        End If


        SetDrawing(Me.Handle, WM_SETREDRAW, True, 0)
        Me.Refresh()

        'Me.Enabled = True
    End Sub

    Private Sub DualScreenBUTT_Click(sender As Object, e As EventArgs) Handles DualScreenBUTT.Click
        'Me.re
        If SceneChangeInProgress = False And Loaded = True Then
            Call DisplayChange("Dual Screen Mode")
            'Me.Refresh()
            UpdateScene("Dual Screen Mode")
        End If

        'Me.Enabled = True
    End Sub

    Private Sub CenterScreenBUTT_Click(sender As Object, e As EventArgs) Handles CenterScreenBUTT.Click
        'Me.Enabled = False
        If SceneChangeInProgress = False And Loaded = True Then
            Call DisplayChange("Center Screen Mode")
            'Me.Refresh()
            UpdateScene("Center Screen Mode")
        End If

        'Me.Enabled = True
    End Sub

    Private Sub SingleScreenBUTT_Click(sender As Object, e As EventArgs) Handles SingleScreenBUTT.Click
        'Me.Enabled = False
        If SceneChangeInProgress = False And Loaded = True Then
            Call DisplayChange("Single Screen Mode")
            'Me.Refresh()
            UpdateScene("Single Screen Mode")
        End If

        'Me.Enabled = True
    End Sub

    Private Sub SplitScreenBUTT_Click(sender As Object, e As EventArgs) Handles SplitScreenBUTT.Click
        'Me.Enabled = False
        If SceneChangeInProgress = False And Loaded = True Then
            Call DisplayChange("Split Screen Mode")
            'Me.Refresh()
            UpdateScene("Split Screen Mode")
        End If

        'Me.Enabled = True
    End Sub

    Private Sub EmberAway_Click(sender As Object, e As EventArgs) Handles EmberAway.Click
        If SceneChangeInProgress = False And Loaded = True Then
            If EmberAway.BackColor = ActiveBUTT Then
                EmberAwayB = False
                Screen1AwayMode = False
            Else
                EmberAwayB = True
                Screen1AwayMode = True
            End If
            Call DisplayChange(CurrentSceneName)
            UpdateScene()
        End If

    End Sub

    Private Sub LunaAway_Click(sender As Object, e As EventArgs) Handles LunaAway.Click
        If SceneChangeInProgress = False And Loaded = True Then
            If LunaAway.BackColor = ActiveBUTT Then
                LunaAwayB = False
                Screen2AwayMode = False
            Else
                LunaAwayB = True
                Screen2AwayMode = True
            End If
            Call DisplayChange(CurrentSceneName)
            UpdateScene()
        End If
    End Sub

    Private Sub EmberEnable_Click(sender As Object, e As EventArgs) Handles EmberEnable.Click
        If SceneChangeInProgress = False And Loaded = True Then
            If EmberEnable.BackColor = ActiveBUTT Then
                If EmberCamera <> False Or EmberSpriteB <> False Then
                    EmberCamera = False
                    EmberSpriteB = False
                    Call DisplayChange(CurrentSceneName)
                    UpdateScene()
                End If
            Else
                If EmberCamera <> True Or EmberSpriteB <> True Then
                    EmberCamera = True
                    EmberSpriteB = True
                    Call DisplayChange(CurrentSceneName)
                    UpdateScene()
                End If
            End If

        End If

    End Sub
    Private Sub EmberDisable_Click(sender As Object, e As EventArgs) Handles EmberDisable.Click
        If SceneChangeInProgress = False And Loaded = True Then
            If EmberDisable.BackColor = ActiveBUTT Then
                If EmberCamera <> True Or EmberSpriteB <> True Then
                    EmberCamera = True
                    EmberSpriteB = True
                    Call DisplayChange(CurrentSceneName)
                    UpdateScene()
                End If
            Else
                If EmberCamera <> False Or EmberSpriteB <> False Then
                    EmberCamera = False
                    EmberSpriteB = False
                    Call DisplayChange(CurrentSceneName)
                    UpdateScene()
                End If
            End If
        End If
    End Sub

    Private Sub EmberCam_Click(sender As Object, e As EventArgs) Handles Ecam.Click
        If SceneChangeInProgress = False And Loaded = True Then
            If EmberCamera = True Then
                EmberCamera = False
            Else
                EmberCamera = True
            End If
            Call DisplayChange(CurrentSceneName)
            UpdateScene()
        End If
    End Sub

    Private Sub EmberSprite_Click(sender As Object, e As EventArgs) Handles EmberSprite.Click
        If SceneChangeInProgress = False And Loaded = True Then
            If EmberSpriteB = True Then
                EmberSpriteB = False
            Else
                EmberSpriteB = True
            End If
            Call DisplayChange(CurrentSceneName)
            UpdateScene()
        End If

    End Sub

    Private Sub EmberSolo_Click(sender As Object, e As EventArgs) Handles EmberSolo.Click
        If SceneChangeInProgress = False And Loaded = True Then
            If LunaSpriteB <> False Or LunaCamera <> False Or Screen2Enabled <> False Or CurrentSceneName <> "Single Screen Mode" Then
                EmberSpriteB = True
                EmberCamera = True
                LunaSpriteB = False
                LunaCamera = False
                Screen2Enabled = False
                Call DisplayChange("Single Screen Mode")
                UpdateScene("Single Screen Mode")
            End If
        End If

    End Sub

    Private Sub lunaSolo_Click(sender As Object, e As EventArgs) Handles LunaSolo.Click
        If SceneChangeInProgress = False And Loaded = True Then
            If EmberSpriteB <> False Or EmberCamera <> False Or Screen2Enabled <> True Or CurrentSceneName <> "Single Screen Mode" Then
                LunaSpriteB = True
                LunaCamera = True
                EmberSpriteB = False
                EmberCamera = False
                Screen2Enabled = True
                Call DisplayChange("Single Screen Mode")
                UpdateScene("Single Screen Mode")
            End If
        End If
    End Sub
    Private Sub lunaEnable_Click(sender As Object, e As EventArgs) Handles LunaEnable.Click
        If SceneChangeInProgress = False And Loaded = True Then
            If LunaEnable.BackColor = ActiveBUTT Then
                If LunaCamera <> False Or LunaSpriteB <> False Then
                    LunaCamera = False
                    LunaSpriteB = False
                    Call DisplayChange(CurrentSceneName)
                    UpdateScene()
                End If
            Else
                If LunaCamera <> True Or LunaSpriteB <> True Then
                    LunaCamera = True
                    LunaSpriteB = True
                    Call DisplayChange(CurrentSceneName)
                    UpdateScene()
                End If
            End If
        End If
    End Sub
    Private Sub lunaDisable_Click(sender As Object, e As EventArgs) Handles LunaDisable.Click
        If SceneChangeInProgress = False And Loaded = True Then
            If LunaDisable.BackColor = ActiveBUTT Then
                If LunaCamera <> True Or LunaSpriteB <> True Then
                    LunaCamera = True
                    LunaSpriteB = True
                    Call DisplayChange(CurrentSceneName)
                    UpdateScene()
                End If
            Else
                If LunaCamera <> False Or LunaSpriteB <> False Then
                    LunaCamera = False
                    LunaSpriteB = False
                    Call DisplayChange(CurrentSceneName)
                    UpdateScene()
                End If
            End If
        End If
    End Sub

    Private Sub lunaCam_Click(sender As Object, e As EventArgs) Handles Lcam.Click
        If SceneChangeInProgress = False And Loaded = True Then
            If LunaCamera = True Then
                LunaCamera = False
            Else
                LunaCamera = True
            End If
            Call DisplayChange(CurrentSceneName)
            UpdateScene()
        End If
    End Sub

    Private Sub lunaSprite_Click(sender As Object, e As EventArgs) Handles LunaSprite.Click
        If SceneChangeInProgress = False And Loaded = True Then
            If LunaSpriteB = True Then
                LunaSpriteB = False
            Else
                LunaSpriteB = True
            End If
            Call DisplayChange(CurrentSceneName)
            UpdateScene()
        End If
    End Sub
    Private Sub EmberPIP_Click(sender As Object, e As EventArgs) Handles EmberPIP.Click
        If SceneChangeInProgress = False And Loaded = True Then
            If PIPenabled = True Then
                PIPenabled = False
            Else
                PIPenabled = True
            End If
            Call DisplayChange(CurrentSceneName)
            UpdateScene()
        End If
    End Sub
    Private Sub LunaPIP_Click(sender As Object, e As EventArgs) Handles LunaPIP.Click
        If SceneChangeInProgress = False And Loaded = True Then
            If PIPenabled = True Then
                PIPenabled = False
            Else
                PIPenabled = True
            End If
            Call DisplayChange(CurrentSceneName)
            UpdateScene()
        End If
    End Sub
    Private Sub Screen1Away_Click(sender As Object, e As EventArgs) Handles EscreenAway.Click
        If SceneChangeInProgress = False And Loaded = True Then
            If Screen1AwayMode = True Then
                Screen1AwayMode = False
            Else
                Screen1AwayMode = True
            End If
            Call DisplayChange(CurrentSceneName)
            UpdateScene()
        End If
    End Sub
    Private Sub cam1Away_Click(sender As Object, e As EventArgs) Handles EcamAway.Click
        If SceneChangeInProgress = False And Loaded = True Then
            If EmberAwayB = True Then
                EmberAwayB = False
            Else
                EmberAwayB = True
            End If
            Call DisplayChange(CurrentSceneName)
            UpdateScene()
        End If
    End Sub
    Private Sub screen2Away_Click(sender As Object, e As EventArgs) Handles LscreenAway.Click
        If SceneChangeInProgress = False And Loaded = True Then
            If Screen2AwayMode = True Then
                Screen2AwayMode = False
            Else
                Screen2AwayMode = True
            End If
            Call DisplayChange(CurrentSceneName)
            UpdateScene()
        End If
    End Sub
    Private Sub cam2Away_Click(sender As Object, e As EventArgs) Handles LcamAway.Click
        If SceneChangeInProgress = False And Loaded = True Then
            If LunaAwayB = True Then
                LunaAwayB = False
            Else
                LunaAwayB = True
            End If
            Call DisplayChange(CurrentSceneName)
            UpdateScene()
        End If
    End Sub

    Private Sub Label4_DoubleClick(sender As Object, e As EventArgs) Handles Lscreen.Click
        If SceneChangeInProgress = False And Loaded = True Then
            If Screen2Enabled = False Then
                Screen2Enabled = True
                Call DisplayChange(CurrentSceneName)
                UpdateScene()
            End If
        End If

    End Sub

    Private Sub Label3_DoubleClick(sender As Object, e As EventArgs) Handles Escreen.Click
        If SceneChangeInProgress = False And Loaded = True Then
            If Screen2Enabled = True Then
                Screen2Enabled = False
                Call DisplayChange(CurrentSceneName)
                UpdateScene()
            End If
        End If
    End Sub

    Private Sub EscreenSelect_SelectedIndexChanged(sender As Object, e As EventArgs) Handles EscreenSelect.SelectedIndexChanged
        If SceneChangeInProgress = False And Loaded = True Then
            If EscreenSelect.SelectedIndex > -1 Then
                If Screen1Setting <> EscreenSelect.SelectedIndex Then
                    Screen1Setting = EscreenSelect.SelectedIndex
                    Call DisplayChange(CurrentSceneName)
                    UpdateScene()
                End If
            End If
        End If
    End Sub

    Private Sub lscreenSelect_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LscreenSelect.SelectedIndexChanged
        If SceneChangeInProgress = False And Loaded = True Then
            If LscreenSelect.SelectedIndex > -1 Then
                If Screen2Setting <> LscreenSelect.SelectedIndex Then
                    Screen2Setting = LscreenSelect.SelectedIndex
                    Call DisplayChange(CurrentSceneName)
                    UpdateScene()
                End If
            End If
        End If
    End Sub
    Private Sub ecamSelect_SelectedIndexChanged(sender As Object, e As EventArgs) Handles EcamSelect.SelectedIndexChanged
        If SceneChangeInProgress = False And Loaded = True Then
            If EcamSelect.SelectedIndex > -1 Then
                If Cam1Setting <> EcamSelect.SelectedIndex Then
                    Cam1Setting = EcamSelect.SelectedIndex
                    Call DisplayChange(CurrentSceneName)
                    UpdateScene()
                End If
            End If
        End If
    End Sub

    Private Sub lcamSelect_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LcamSelect.SelectedIndexChanged
        If SceneChangeInProgress = False And Loaded = True Then
            If LcamSelect.SelectedIndex > -1 Then
                If Cam2Setting <> LcamSelect.SelectedIndex Then
                    Cam2Setting = LcamSelect.SelectedIndex
                    Call DisplayChange(CurrentSceneName)
                    UpdateScene()
                End If
            End If
        End If
    End Sub

    Private Sub ScreenSwitch_Click(sender As Object, e As EventArgs) Handles ScreenSwitch.Click
        If SceneChangeInProgress = False And Loaded = True Then
            If Screen1Setting <> Screen2Setting Then
                Dim InputInt As Integer = Screen1Setting
                Screen1Setting = Screen2Setting
                Screen2Setting = InputInt
                Call DisplayChange(CurrentSceneName)
                UpdateScene()
            End If
        End If
    End Sub

    Private Sub CamSwitch_Click(sender As Object, e As EventArgs) Handles CamSwitch.Click
        If SceneChangeInProgress = False And Loaded = True Then
            If Cam1Setting <> Cam2Setting Then
                Dim InputInt As Integer = Cam1Setting
                Cam1Setting = Cam2Setting
                Cam2Setting = InputInt
                Call DisplayChange(CurrentSceneName)
                UpdateScene()
            End If
        End If
    End Sub

    Private Sub ResetButt_Click(sender As Object, e As EventArgs) Handles ResetButt.Click
        If SceneListBox.SelectedText <> "" Then
            SceneFile = SceneListBox.SelectedText
            Dim SceneTask As Task = ApplySceneFile()
        ElseIf SceneFile <> "" Then
            Dim SceneTask As Task = ApplySceneFile()
        End If
    End Sub

    Private Async Function ApplySceneFile() As Task
        'Enabled = False
        CurrentSceneLabel.Text = SceneFile
        Await MySceneCollection.SceneCollection(MySceneCollection.IndexByName(SceneFile)).ApplySceneSettings()
        DisplayChange(CurrentScene.Name)
        'Enabled = True
    End Function

    Private Sub CamsBUTT_Click(sender As Object, e As EventArgs) Handles CamsBUTT.Click
        If SceneChangeInProgress = False And Loaded = True Then
            If CamsBUTT.BackColor = ActiveBUTT Then
                EmberCamera = False
                LunaCamera = False
                Call DisplayChange(CurrentSceneName)
                UpdateScene()
            Else
                EmberCamera = True
                LunaCamera = True
                Call DisplayChange(CurrentSceneName)
                UpdateScene()
            End If
        End If
    End Sub

    Private Sub SpritesBUTT_Click(sender As Object, e As EventArgs) Handles SpritesBUTT.Click
        If SceneChangeInProgress = False And Loaded = True Then
            If SpritesBUTT.BackColor = ActiveBUTT Then
                EmberSpriteB = False
                LunaSpriteB = False
                Call DisplayChange(CurrentSceneName)
                UpdateScene()
            Else
                EmberSpriteB = True
                LunaSpriteB = True
                Call DisplayChange(CurrentSceneName)
                UpdateScene()
            End If
        End If
    End Sub

    Private Sub EnableAllBUTT_Click(sender As Object, e As EventArgs) Handles EnableAllBUTT.Click
        If SceneChangeInProgress = False And Loaded = True Then
            If EnableAllBUTT.BackColor = ActiveBUTT Then
                EmberSpriteB = False
                LunaSpriteB = False
                LunaCamera = False
                EmberCamera = False
                Call DisplayChange(CurrentSceneName)
                UpdateScene()
            Else
                EmberSpriteB = True
                LunaSpriteB = True
                LunaCamera = True
                EmberCamera = True
                Call DisplayChange(CurrentSceneName)
                UpdateScene()
            End If
        End If
    End Sub

    Private Sub DisableAllBUTT_Click(sender As Object, e As EventArgs) Handles DisableAllBUTT.Click
        If SceneChangeInProgress = False And Loaded = True Then
            If DisableAllBUTT.BackColor = ActiveBUTT Then
                EmberSpriteB = True
                LunaSpriteB = True
                LunaCamera = True
                EmberCamera = True
                Call DisplayChange(CurrentSceneName)
                UpdateScene()
            Else
                EmberSpriteB = False
                LunaSpriteB = False
                LunaCamera = False
                EmberCamera = False
                Call DisplayChange(CurrentSceneName)
                UpdateScene()
            End If
        End If
    End Sub

    Private Sub ScreensAway_Click(sender As Object, e As EventArgs) Handles ScreensAway.Click
        If SceneChangeInProgress = False And Loaded = True Then
            If ScreensAway.BackColor = ActiveBUTT Then
                Screen1AwayMode = False
                Screen2AwayMode = False
                Call DisplayChange(CurrentSceneName)
                UpdateScene()
            Else
                Screen1AwayMode = True
                Screen2AwayMode = True
                Call DisplayChange(CurrentSceneName)
                UpdateScene()
            End If
        End If
    End Sub

    Private Sub CamsAway_Click(sender As Object, e As EventArgs) Handles CamsAway.Click
        If SceneChangeInProgress = False And Loaded = True Then
            If CamsAway.BackColor = ActiveBUTT Then
                EmberAwayB = False
                LunaAwayB = False
                Call DisplayChange(CurrentSceneName)
                UpdateScene()
            Else
                EmberAwayB = True
                LunaAwayB = True
                Call DisplayChange(CurrentSceneName)
                UpdateScene()
            End If
        End If
    End Sub

    Private Sub NewButt_Click(sender As Object, e As EventArgs) Handles NewButt.Click
        Dim SceneName As String = InputBox("What are we calling this", "GO ON...")
        If SceneName <> "" Then
            MySceneCollection.AddScene(SceneName)
            SceneFile = SceneName
        End If
    End Sub

    Private Sub Button5_Click_1(sender As Object, e As EventArgs) Handles SaveButt.Click
        If SceneFile <> "" Then
            Dim Result As MsgBoxResult = MsgBox("Are you syre you want to overwrite """ & SceneFile & """", MsgBoxStyle.YesNo, "ARE U SURE ABOUT THAT!?")
            If Result = MsgBoxResult.Yes Then MySceneCollection.UpdateScene(MySceneCollection.IndexByName(SceneFile))
        End If
    End Sub

    Private Sub DeleteButt_Click(sender As Object, e As EventArgs) Handles DeleteButt.Click
        Dim MySceneName As String
        If SceneListBox.SelectedText <> "" Then
            MySceneName = SceneListBox.SelectedText
        Else
            MySceneName = SceneFile
        End If
        If MySceneName <> "" Then
            Dim Result As MsgBoxResult = MsgBox("Are you syre you want to delete """ & MySceneName & """", MsgBoxStyle.YesNo, "ARE U SURE ABOUT THAT!?")
            If Result = MsgBoxResult.Yes Then
                MySceneCollection.DeleteScene(MySceneCollection.IndexByName(MySceneName))
                If MySceneName = SceneFile Then SceneFile = ""
            End If
        End If
    End Sub

    Private Sub SceneListBox_Click(sender As Object, e As EventArgs) Handles SceneListBox.Click
        TextboxLineSelector(sender)
    End Sub

    Private Sub SceneListBox_DoubleClick(sender As Object, e As EventArgs) Handles SceneListBox.DoubleClick
        Dim SelectedFile As String = TextboxLineSelector(sender)
        If SelectedFile <> "" Then
            SceneFile = SelectedFile
            Dim SceneTask As Task = ApplySceneFile()
        End If

    End Sub

    Private Sub SceneListBox_TextChanged(sender As Object, e As EventArgs) Handles SceneListBox.TextChanged

    End Sub


    'Private Sub EmberPC_Click(sender As Object, e As EventArgs) Handles EmberScreen.Click
    'If LunaScreenB = True Then
    'LunaScreenB = False
    'End If
    'UpdateScene()
    'Call DisplayChange(CurrentSceneName)
    'End Sub

    'Private Sub LunaPC_Click(sender As Object, e As EventArgs)
    'If LunaScreenB = False Then
    'LunaScreenB = True
    'End If
    'UpdateScene()
    'Call DisplayChange(CurrentSceneName)
    'End Sub



    Public Sub ImportSingleR(ChannelIndex As Integer)
        ImportSingle(ChannelIndex, True)
    End Sub

    Public Sub ImportSingle(ChannelIndex As Integer, Optional UpdateMatrix As Boolean = False)
        Dim MyChannel As GroupBox = Controls("GroupBox" & ChannelIndex + 1)
        Dim MydB As NumericUpDown = MyChannel.Controls("NumericUpDown" & ChannelIndex + 1)
        Dim MyMute As Windows.Forms.Button = MyChannel.Controls("Button" & ChannelIndex + 1)
        Dim MyMon As Windows.Forms.Button = MyChannel.Controls("Mon" & ChannelIndex + 1)
        MydB.Value = Round(100 + AudioControl.MyMixer.Channels(ChannelIndex).Level, 1)
        If AudioControl.MyMixer.Channels(ChannelIndex).Muted Then
            'MyMute.Text = "UNMUTE"
            MyMute.BackColor = ActiveBUTT
        Else
            'MyMute.Text = "MUTE"
            MyMute.BackColor = StandardBUTT
        End If
        If AudioControl.MyMixer.Channels(ChannelIndex).Monitored Then
            MyMon.BackColor = ActiveBUTT
        Else
            MyMon.BackColor = StandardBUTT
        End If

        If UpdateMatrix Then UpdateMuteMatrix()
    End Sub

    Public Sub ImportAll()
        For I As Integer = 0 To AudioChannelIDs.LastChanneL
            ImportSingle(I)
        Next
        UpdateMuteMatrix()
    End Sub

    Public Sub UpdateMuteMatrix()
        If AudioControl.MyMixer.Channels(AudioChannelIDs.EmberMic).Muted = True And
            AudioControl.MyMixer.Channels(AudioChannelIDs.EmberPC).Muted = True Then
            EMBERmute.BackColor = ActiveBUTT
        Else
            EMBERmute.BackColor = StandardBUTT
        End If
        If AudioControl.MyMixer.Channels(AudioChannelIDs.LunaMic).Muted = True And
            AudioControl.MyMixer.Channels(AudioChannelIDs.LunaPC).Muted = True Then
            LUNAmute.BackColor = ActiveBUTT
        Else
            LUNAmute.BackColor = StandardBUTT
        End If
        If AudioControl.MyMixer.Channels(AudioChannelIDs.EmberPC).Muted = True And
            AudioControl.MyMixer.Channels(AudioChannelIDs.LunaPC).Muted = True And
            AudioControl.MyMixer.Channels(AudioChannelIDs.Console).Muted = True Then
            PCmute.BackColor = ActiveBUTT
        Else
            PCmute.BackColor = StandardBUTT
        End If
        If AudioControl.MyMixer.Channels(AudioChannelIDs.LunaMic).Muted = True And
            AudioControl.MyMixer.Channels(AudioChannelIDs.EmberMic).Muted = True And
            AudioControl.MyMixer.Channels(AudioChannelIDs.Discord).Muted = True Then
            MICmute.BackColor = ActiveBUTT
        Else
            MICmute.BackColor = StandardBUTT
        End If
    End Sub




    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        Number_Output_toSlider(sender)
    End Sub
    Private Sub ProgressBar1_Click(sender As Object, e As MouseEventArgs) Handles ProgressBar1.MouseClick
        Control_Click_todB(e, sender)
    End Sub
    Private Sub ProgressBar1_MouseHover(sender As Object, e As MouseEventArgs) Handles ProgressBar1.MouseMove
        ToolTip1.SetToolTip(sender, Control_dB(e, sender, True) & "dB")
    End Sub

    Private Sub NumericUpDown2_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown2.ValueChanged
        Number_Output_toSlider(sender)
    End Sub
    Private Sub ProgressBar2_Click(sender As Object, e As MouseEventArgs) Handles ProgressBar2.MouseClick
        Control_Click_todB(e, sender)
    End Sub
    Private Sub ProgressBar2_MouseHover(sender As Object, e As MouseEventArgs) Handles ProgressBar2.MouseMove
        ToolTip1.SetToolTip(sender, Control_dB(e, sender, True) & "dB")
    End Sub

    Private Sub NumericUpDown3_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown3.ValueChanged
        Number_Output_toSlider(sender)
    End Sub
    Private Sub ProgressBar3_Click(sender As Object, e As MouseEventArgs) Handles ProgressBar3.MouseClick
        Control_Click_todB(e, sender)
    End Sub
    Private Sub ProgressBar3_MouseHover(sender As Object, e As MouseEventArgs) Handles ProgressBar3.MouseMove
        ToolTip1.SetToolTip(sender, Control_dB(e, sender, True) & "dB")
    End Sub

    Private Sub NumericUpDown4_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown4.ValueChanged
        Number_Output_toSlider(sender)
    End Sub
    Private Sub ProgressBar4_Click(sender As Object, e As MouseEventArgs) Handles ProgressBar4.MouseClick
        Control_Click_todB(e, sender)
    End Sub
    Private Sub ProgressBar4_MouseHover(sender As Object, e As MouseEventArgs) Handles ProgressBar4.MouseMove
        ToolTip1.SetToolTip(sender, Control_dB(e, sender, True) & "dB")
    End Sub

    Private Sub NumericUpDown5_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown5.ValueChanged
        Number_Output_toSlider(sender)
    End Sub
    Private Sub ProgressBar5_Click(sender As Object, e As MouseEventArgs) Handles ProgressBar5.MouseClick
        Control_Click_todB(e, sender)
    End Sub
    Private Sub ProgressBar5_MouseHover(sender As Object, e As MouseEventArgs) Handles ProgressBar5.MouseMove
        ToolTip1.SetToolTip(sender, Control_dB(e, sender, True) & "dB")
    End Sub

    Private Sub NumericUpDown6_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown6.ValueChanged
        Number_Output_toSlider(sender)
    End Sub
    Private Sub ProgressBar6_Click(sender As Object, e As MouseEventArgs) Handles ProgressBar6.MouseClick
        Control_Click_todB(e, sender)
    End Sub
    Private Sub ProgressBar6_MouseHover(sender As Object, e As MouseEventArgs) Handles ProgressBar6.MouseMove
        ToolTip1.SetToolTip(sender, Control_dB(e, sender, True) & "dB")
    End Sub

    Private Sub NumericUpDown7_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown7.ValueChanged
        Number_Output_toSlider(sender)
    End Sub
    Private Sub ProgressBar7_Click(sender As Object, e As MouseEventArgs) Handles ProgressBar7.MouseClick
        Control_Click_todB(e, sender)
    End Sub
    Private Sub ProgressBar7_MouseHover(sender As Object, e As MouseEventArgs) Handles ProgressBar7.MouseMove
        ToolTip1.SetToolTip(sender, Control_dB(e, sender, True) & "dB")
    End Sub

    Private Sub NumericUpDown8_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown8.ValueChanged
        Number_Output_toSlider(sender)
    End Sub
    Private Sub ProgressBar8_Click(sender As Object, e As MouseEventArgs) Handles ProgressBar8.MouseClick
        Control_Click_todB(e, sender)
    End Sub
    Private Sub ProgressBar8_MouseHover(sender As Object, e As MouseEventArgs) Handles ProgressBar8.MouseMove
        ToolTip1.SetToolTip(sender, Control_dB(e, sender, True) & "dB")
    End Sub

    Private Function Control_dB(MousePOS As MouseEventArgs, Control As Windows.Forms.ProgressBar, Optional dBout As Boolean = False)
        Dim Xpercent As Double = MousePOS.X / Control.Size.Width
        Xpercent = 100 + (50 * Log10(Xpercent))
        If Xpercent < 0 Then Xpercent = 0
        If dBout Then
            Return Round(Xpercent - 100, 1)
        Else
            Return Round(Xpercent, 1)
        End If
    End Function

    Private Sub Number_Output_toSlider(Control As NumericUpDown)
        Dim ControlInt As Integer = Replace(Control.Name, "NumericUpDown", "")
        Dim MyChannel As GroupBox = Controls("GroupBox" & ControlInt)
        Dim NumOut As Windows.Forms.ProgressBar = MyChannel.Controls("ProgressBar" & ControlInt)
        Dim ValueOut As Label = MyChannel.Controls("Label" & ControlInt)
        ValueOut.Text = AudioControl.MyMixer.Channels(ControlInt - 1).Name & " (" & Control.Value - 100 & "dB)"
        NumOut.Value = 100 * (10 ^ ((Control.Value - 100) / 50))
        If Loaded Then
            AudioControl.MyMixer.Channels(ControlInt - 1).ApplySettings(Control.Value - 100)
        End If
    End Sub

    Private Sub Control_Click_todB(MousePOS As MouseEventArgs, Control As Windows.Forms.ProgressBar)
        Dim ControlInt As Integer = Replace(Control.Name, "ProgressBar", "")
        Dim MyChannel As GroupBox = Controls("GroupBox" & ControlInt)
        Dim NumOut As NumericUpDown = MyChannel.Controls("NumericUpDown" & ControlInt)
        NumOut.Value = Control_dB(MousePOS, Control)
    End Sub


    Private Sub MuteClick(Muter As Windows.Forms.Button)
        Dim ControlInt As Integer = Replace(Muter.Name, "Button", "")
        AudioControl.MyMixer.ToggleMute(ControlInt - 1)
    End Sub
    Private Sub MonClick(Muter As Windows.Forms.Button)
        Dim ControlInt As Integer = Replace(Muter.Name, "Mon", "")
        AudioControl.MyMixer.ToggleMon(ControlInt - 1)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        MuteClick(sender)
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        MuteClick(sender)
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        MuteClick(sender)
    End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        MuteClick(sender)
    End Sub
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        MuteClick(sender)
    End Sub
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        MuteClick(sender)
    End Sub
    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        MuteClick(sender)
    End Sub
    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        MuteClick(sender)
    End Sub

    Private Sub Mon1_Click(sender As Object, e As EventArgs) Handles Mon1.Click
        MonClick(sender)
    End Sub
    Private Sub Mon2_Click(sender As Object, e As EventArgs) Handles Mon2.Click
        MonClick(sender)
    End Sub
    Private Sub Mon3_Click(sender As Object, e As EventArgs) Handles Mon3.Click
        MonClick(sender)
    End Sub
    Private Sub Mon4_Click(sender As Object, e As EventArgs) Handles Mon4.Click
        MonClick(sender)
    End Sub
    Private Sub Mon5_Click(sender As Object, e As EventArgs) Handles Mon5.Click
        MonClick(sender)
    End Sub
    Private Sub Mon6_Click(sender As Object, e As EventArgs) Handles Mon6.Click
        MonClick(sender)
    End Sub
    Private Sub Mon7_Click(sender As Object, e As EventArgs) Handles Mon7.Click
        MonClick(sender)
    End Sub
    Private Sub Mon8_Click(sender As Object, e As EventArgs) Handles Mon8.Click
        MonClick(sender)
    End Sub

    Private Sub EMBERmute_Click(sender As Object, e As EventArgs) Handles EMBERmute.Click
        If EMBERmute.BackColor = ActiveBUTT Then
            AudioControl.MyMixer.Channels(AudioChannelIDs.EmberPC).Muted = False
            AudioControl.MyMixer.Channels(AudioChannelIDs.EmberMic).Muted = False
            AudioControl.MyMixer.Channels(AudioChannelIDs.EmberPC).ApplySettings()
            AudioControl.MyMixer.Channels(AudioChannelIDs.EmberMic).ApplySettings()
        Else
            AudioControl.MyMixer.Channels(AudioChannelIDs.EmberPC).Muted = True
            AudioControl.MyMixer.Channels(AudioChannelIDs.EmberMic).Muted = True
            AudioControl.MyMixer.Channels(AudioChannelIDs.EmberPC).ApplySettings()
            AudioControl.MyMixer.Channels(AudioChannelIDs.EmberMic).ApplySettings()
        End If
        ImportSingle(AudioChannelIDs.EmberPC)
        ImportSingle(AudioChannelIDs.EmberMic, True)
    End Sub

    Private Sub LUNAmute_Click(sender As Object, e As EventArgs) Handles LUNAmute.Click
        If LUNAmute.BackColor = ActiveBUTT Then
            AudioControl.MyMixer.Channels(AudioChannelIDs.LunaPC).Muted = False
            AudioControl.MyMixer.Channels(AudioChannelIDs.LunaMic).Muted = False
            AudioControl.MyMixer.Channels(AudioChannelIDs.LunaPC).ApplySettings()
            AudioControl.MyMixer.Channels(AudioChannelIDs.LunaMic).ApplySettings()
        Else
            AudioControl.MyMixer.Channels(AudioChannelIDs.LunaPC).Muted = True
            AudioControl.MyMixer.Channels(AudioChannelIDs.LunaMic).Muted = True
            AudioControl.MyMixer.Channels(AudioChannelIDs.LunaPC).ApplySettings()
            AudioControl.MyMixer.Channels(AudioChannelIDs.LunaMic).ApplySettings()
        End If
        ImportSingle(AudioChannelIDs.LunaPC)
        ImportSingle(AudioChannelIDs.LunaMic, True)
    End Sub

    Private Sub PCmute_Click(sender As Object, e As EventArgs) Handles PCmute.Click
        If PCmute.BackColor = ActiveBUTT Then
            AudioControl.MyMixer.Channels(AudioChannelIDs.LunaPC).Muted = False
            AudioControl.MyMixer.Channels(AudioChannelIDs.EmberPC).Muted = False
            AudioControl.MyMixer.Channels(AudioChannelIDs.Console).Muted = False
            AudioControl.MyMixer.Channels(AudioChannelIDs.LunaPC).ApplySettings()
            AudioControl.MyMixer.Channels(AudioChannelIDs.EmberPC).ApplySettings()
            AudioControl.MyMixer.Channels(AudioChannelIDs.Console).ApplySettings()
        Else
            AudioControl.MyMixer.Channels(AudioChannelIDs.LunaPC).Muted = True
            AudioControl.MyMixer.Channels(AudioChannelIDs.EmberPC).Muted = True
            AudioControl.MyMixer.Channels(AudioChannelIDs.Console).Muted = True
            AudioControl.MyMixer.Channels(AudioChannelIDs.LunaPC).ApplySettings()
            AudioControl.MyMixer.Channels(AudioChannelIDs.EmberPC).ApplySettings()
            AudioControl.MyMixer.Channels(AudioChannelIDs.Console).ApplySettings()
        End If
        ImportSingle(AudioChannelIDs.LunaPC)
        ImportSingle(AudioChannelIDs.EmberPC)
        ImportSingle(AudioChannelIDs.Console, True)
    End Sub

    Private Sub MICmute_Click(sender As Object, e As EventArgs) Handles MICmute.Click
        If MICmute.BackColor = ActiveBUTT Then
            AudioControl.MyMixer.Channels(AudioChannelIDs.EmberMic).Muted = False
            AudioControl.MyMixer.Channels(AudioChannelIDs.LunaMic).Muted = False
            AudioControl.MyMixer.Channels(AudioChannelIDs.Discord).Muted = False
            AudioControl.MyMixer.Channels(AudioChannelIDs.EmberMic).ApplySettings()
            AudioControl.MyMixer.Channels(AudioChannelIDs.LunaMic).ApplySettings()
            AudioControl.MyMixer.Channels(AudioChannelIDs.Discord).ApplySettings()
        Else
            AudioControl.MyMixer.Channels(AudioChannelIDs.EmberMic).Muted = True
            AudioControl.MyMixer.Channels(AudioChannelIDs.LunaMic).Muted = True
            AudioControl.MyMixer.Channels(AudioChannelIDs.Discord).Muted = True
            AudioControl.MyMixer.Channels(AudioChannelIDs.EmberMic).ApplySettings()
            AudioControl.MyMixer.Channels(AudioChannelIDs.LunaMic).ApplySettings()
            AudioControl.MyMixer.Channels(AudioChannelIDs.Discord).ApplySettings()
        End If
        ImportSingle(AudioChannelIDs.EmberMic)
        ImportSingle(AudioChannelIDs.LunaMic)
        ImportSingle(AudioChannelIDs.Discord, True)
    End Sub

    Private MixerBool As Boolean = False
    Private Sub ToggleVolumeControls(Optional EnforceValue As Integer = -1)
        If EnforceValue > -1 Then
            If EnforceValue = 0 Then
                MixerBool = False
            Else
                MixerBool = True
            End If
        Else
            If MixerBool Then
                MixerBool = False
            Else
                MixerBool = True
            End If
        End If

        If MixerBool = True Then
            MaximumSize = New Size(2000, 900)
            MinimumSize = New Size(794, 900)
            ShowMixer.Text = "HIDE VOLUME CONTROLS"
            ShowMixer.BackColor = ActiveBUTT
        Else
            MaximumSize = New Size(2000, 498)
            MinimumSize = New Size(794, 498)
            ShowMixer.Text = "SHOW VOLUME CONTROLS"
            ShowMixer.BackColor = StandardBUTT
        End If
    End Sub



    Private Sub ShowMixer_Click(sender As Object, e As EventArgs) Handles ShowMixer.Click
        ToggleVolumeControls()
    End Sub
End Class