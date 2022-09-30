
Imports System.Threading
Imports System.Math
Imports System.IO
Imports TwitchLib.PubSub

Public Class MainWindow

    Private CurrentSceneName As String = ""
    Private StrimStarter As StreamStarter
    Private StrimStarterState As Boolean = False
    Private PubSubStarterState As Boolean = False
    Private ChannelPointStarterState As Boolean

    Private HatsTest As VideoPlayer

    'Public Event OBSSceneChanged(SceneName As String)

    Private Sub MainWindow_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        SetDrawing(Me.Handle, WM_SETREDRAW, False, 0)
        'InitializeCountersAndTimers()
        ReadProgramSettings(ProgramSettingsFile)

        InitializeOBS_WS()
        SourceWindow = Me
        DemoPlayer = New DemoModule
        ChannelPointsDisplay = New ChannelPointsForm
        MyResourceManager = New ResourceManager(ResourceIDs.LastID)
        ChannelPoints = New ChannelPointData
        SceneChanger = New SceneSelector
        GamesList = New TwitchGames
        ChatManager = New Chat
        OBSTimers = New TimerControls
        TimerCollection = New OBSTimerData
        SpriteControls = New OBScharacters
        'MessageBuffer = New ConcurrentQueue(Of String)
        AudioControl = New OBSaudioPlayer
        MusicPlayer = New OBSMusicPlayer
        SoundBoard = New OBSSoundBoard
        Counters = New OBScounters
        CounterData = New OBScounterData
        ChatUserInfo = New UserData
        MySceneCollection = New SceneCollection
        MyOBSevents = New OBSevents

        'HatsTest = New VideoPlayer("EmbersHats", "Events and Alerts", "Hats.wav")

        IRC = New IrcClient("irc.twitch.tv", 6667, BotName, twitchOAuth, Broadcastername)
        Ember = New CharacterControls("Ember")
        Luna = New CharacterControls("Luna")

        myAPI = New APIfunctions
        PubSub = New TwitchPubSub

        AddHandler ChannelPoints.AllRewardsUpdated, AddressOf ChannelPointsStarter
        AddHandler OBS.StreamStateChanged, AddressOf StreamStartHandler
        'AddHandler OBS.Connected, AddressOf OBSisConnected

        AddHandler IRC.IRCconnected, AddressOf IRCisConnected
        AddHandler IRC.IRCdisconnected, AddressOf IRCdisConnected
        AddHandler IRC.IRCeventRecieved, AddressOf IRCevent
        AddHandler IRC.IRCmessageRecieved, AddressOf GenericNotification

        AddHandler MyResourceManager.TaskEvent, AddressOf RMeventHandler

        AddHandler myAPI.StreamInfoAvailable, AddressOf LaunchStreamStarter
        AddHandler myAPI.AuthorizationInitialized, AddressOf WaitforAuthorization
        AddHandler myAPI.TokenAcquired, AddressOf GenericNotification

        'AddHandler Ember.Said, AddressOf GenericNotification
        'AddHandler Luna.Said, AddressOf GenericNotification

        AddHandler ChatUserInfo.ChatUserDetected, AddressOf NewUserHandler
        AddHandler ChatUserInfo.MessageRecieved, AddressOf UpdateChatBox
        'AddHandler CounterData.CounterStarted, AddressOf CounterUpdated

        AddHandler PubSub.OnViewCount, AddressOf ViewCountChanged
        AddHandler PubSub.OnChannelPointsRewardRedeemed, AddressOf ChannelPointsNotification
        AddHandler PubSub.OnListenResponse, AddressOf ListenResponse
        AddHandler PubSub.OnBitsReceivedV2, AddressOf iGOTStheBITS
        AddHandler PubSub.OnChannelSubscription, AddressOf SubscriberAcquired
        AddHandler PubSub.OnPubSubServiceConnected, AddressOf PubSubConnected
        AddHandler PubSub.OnPubSubServiceClosed, AddressOf PubSubDisconnect

        AddHandler AudioControl.MusicPlayer.Started, AddressOf MediaStarted
        AddHandler AudioControl.MusicPlayer.Paused, AddressOf MediaPaused
        AddHandler AudioControl.MusicPlayer.Stopped, AddressOf MediaEnded
        AddHandler AudioControl.SoundPlayer.SoundPlayed, AddressOf GenericNotification
        'AddHandler AudioControl.SoundPlayer.Paused, AddressOf SoundPaused
        'AddHandler AudioControl.SoundPlayer.Stopped, AddressOf SoundEnded

        SetDrawing(Me.Handle, WM_SETREDRAW, True, 0)
        Me.Refresh()
    End Sub

    Private Sub MainWindow_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Dim STARTYOURENGINES As Task = Startup()
    End Sub

    Private Async Function CloseMe() As Task
        ChannelPoints.UpdateProgramRewards(False)
        CounterData.SaveCounterData()
        OBStimerObject(TimerIDs.Ember).State = False
        OBStimerObject(TimerIDs.Luna).State = False
        OBStimerObject(TimerIDs.GlobalCC).State = False
        If AudioControl.MusicPlayer.Active = True Then AudioControl.StopMusic()

        Await Task.Delay(500)

        If PSubConnected Then PubSub.Disconnect()
        If IRCconnected Then IRC.DisconnectChat()
        If OBSconnected Then DisconnectOBS()

        Do Until AmIdisconnected()
            Await Task.Delay(200)
            If OBSconnected Then
                If DisconnectOBS() = False Then
                    OBSstateChanged(False)
                End If
            End If
        Loop
        Await Task.Delay(500)
        Close()
    End Function

    Private Sub MainWindow_Closing(sender As Object, e As FormClosingEventArgs) Handles MyBase.Closing
        If Enabled = True Then
            Enabled = False

            e.Cancel = True
            Dim CloseTask = CloseMe()
        End If



        'RemoveHandler ChannelPoints.AllRewardsUpdated, AddressOf ChannelPointsStarter
        'RemoveHandler OBS.StreamStateChanged, AddressOf StreamStartHandler
        ''RemoveHandler OBS.Connected, AddressOf OBSisConnected

        'RemoveHandler myAPI.StreamInfoAvailable, AddressOf LaunchStreamStarter
        'RemoveHandler myAPI.AuthorizationInitialized, AddressOf WaitforAuthorization
        'RemoveHandler myAPI.TokenAcquired, AddressOf GenericNotification

        'RemoveHandler IRC.IRCconnected, AddressOf IRCisConnected
        'RemoveHandler IRC.IRCdisconnected, AddressOf IRCdisConnected

        'AddHandler MyResourceManager.TaskEvent, AddressOf RMeventHandler

        ''RemoveHandler Ember.Said, AddressOf GenericNotification
        ''RemoveHandler Luna.Said, AddressOf GenericNotification

        'RemoveHandler ChatUserInfo.ChatUserDetected, AddressOf NewUserHandler
        'RemoveHandler ChatUserInfo.MessageRecieved, AddressOf UpdateChatBox
        ''RemoveHandler CounterData.CounterStarted, AddressOf CounterUpdated

        'RemoveHandler PubSub.OnViewCount, AddressOf ViewCountChanged
        'RemoveHandler PubSub.OnChannelPointsRewardRedeemed, AddressOf ChannelPointsNotification
        'RemoveHandler PubSub.OnListenResponse, AddressOf ListenResponse
        'RemoveHandler PubSub.OnBitsReceivedV2, AddressOf iGOTStheBITS
        'RemoveHandler PubSub.OnChannelSubscription, AddressOf SubscriberAcquired
        'RemoveHandler PubSub.OnPubSubServiceConnected, AddressOf PubSubConnected
        'RemoveHandler PubSub.OnPubSubServiceClosed, AddressOf PubSubDisconnect

        'RemoveHandler AudioControl.MusicPlayer.Started, AddressOf MediaStarted
        'RemoveHandler AudioControl.MusicPlayer.Paused, AddressOf MediaPaused
        'RemoveHandler AudioControl.MusicPlayer.Stopped, AddressOf MediaEnded
        'RemoveHandler AudioControl.SoundPlayer.SoundPlayed, AddressOf GenericNotification
        'RemoveHandler AudioControl.SoundPlayer.Paused, AddressOf SoundPaused
        'RemoveHandler AudioControl.SoundPlayer.Stopped, AddressOf SoundEnded

        'Call DisconnectOBS()
        'Application.DoEvents()
        'Thread.Sleep(500)

    End Sub

    Private PSubConnected As Boolean = False
    Private IRCconnected As Boolean = False
    Public OBSconnected As Boolean = False
    Private TwitchAuthorized As Boolean = False
    Private APIconnected As Boolean = False

    Private Async Function Startup() As Task
        Me.Enabled = False
        OBSstateChanged(Await CheckOBSconnect())
        IRC.ConnectChat()
        myAPI.APIauthorization(True)
        myAPI.GetStreamInfo()

        Dim StartupCounter As Integer = 0
        Do Until AmIconnected()
            Await Task.Delay(100)
            'startupCounter = StartupCounter + 1
            'If StartupCounter > 100 Then
            '    SendMessage("Failed to Start...", "UH OH")
            '    Me.Close()
            'End If
        Loop

        ChannelPointStarterState = True
        myAPI.SyncChannelRedemptions()
        SyncSceneDisplay()
        AudioControl.MyMixer.SyncAll()
        Await MyResourceManager.WaitAllRequests
        'AudioControl.MusicPlayer.SyncState()

        Me.Enabled = True
    End Function

    Private Function AmIconnected() As Boolean
        If PSubConnected And IRCconnected And OBSconnected And APIconnected And TwitchAuthorized Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function AmIdisconnected() As Boolean
        If PSubConnected = False And IRCconnected = False And OBSconnected = False Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub IRCstateChanged(IRCState As Boolean)
        If IRCState <> IRCconnected Then
            IRCconnected = IRCState
            If IRCconnected = True Then
                IRCdisplay.BackColor = ActiveBUTT
                LogEventString("IRC Connected")
            Else
                IRCdisplay.BackColor = StandardBUTT
                LogEventString("IRC Disconnected")
            End If
        End If
    End Sub

    Private Sub IRCisConnected()
        IRCstateChanged(True)
    End Sub
    Private Sub IRCdisConnected()
        IRCstateChanged(False)
    End Sub

    Private Sub RMeventHandler(TaskString As String, TaskState As Integer)
        Select Case TaskState
            Case = RMtaskStates.Started
                LogEventString(TaskString & " Started")
            Case = RMtaskStates.Queued
                LogEventString("Added to Task Scheuler: " & TaskString)
            Case = RMtaskStates.Ended
                LogEventString(TaskString & " Ended")
            Case = RMtaskStates.Abandoned
                LogEventString(TaskString & " Abandoned")
        End Select
    End Sub

    Public Sub OBSstateChanged(OBSState As Boolean)
        If OBSState <> OBSconnected Then
            OBSconnected = OBSState
            If OBSconnected = True Then
                OBSdisplay.BackColor = ActiveBUTT
                LogEventString("OBS Connected")
            Else
                OBSdisplay.BackColor = StandardBUTT
                LogEventString("OBS Disconnected")
            End If
        End If
    End Sub



    Private Sub PubSubStateChanged(PubSubState As Boolean)
        If PubSubState <> PSubConnected Then
            PSubConnected = PubSubState
            If PSubConnected = True Then
                PUBSUBdisplay.BackColor = ActiveBUTT
                LogEventString("PubSub Connected")
            Else
                PUBSUBdisplay.BackColor = StandardBUTT
                LogEventString("PubSub Disconnected")
            End If
        End If
    End Sub
    Private Sub PubSubConnected(sender As Object, e As EventArgs)
        PubSubStateChanged(True)
    End Sub

    Public Sub PubSubDisconnect(sender As Object, e As EventArgs)
        PubSubStateChanged(False)
    End Sub


    Public Sub LaunchStreamStarter(CurrentStreamInfo As String)
        APIconnected = True
        APIdisplay.BackColor = ActiveBUTT
        LogEventString(CurrentStreamInfo)
    End Sub

    Private Sub SceneChanged(SceneName As String)
        If SceneName <> CurrentSceneName Then
            CurrentSceneName = SceneName
            LogEventString("OBS Scene Change: " & SceneName)
        End If
    End Sub

    Private Sub StreamStartHandler()
        Dim StateThread As New Thread(
        Sub()
            'SendMessage("triggered")
            If OBSstreamState() = True Then
                BeginInvoke(
                Sub()
                    LogEventString("Streaming Started")
                    Button13.Text = "END STREAM"
                    Button13.BackColor = ActiveBUTT
                    Button13.Enabled = True
                    IRC.ChatReady()
                End Sub)
            Else
                BeginInvoke(
                Sub()
                    LogEventString("Streaming Stopped")
                    Button13.Text = "START STREAM"
                    Button13.BackColor = StandardBUTT
                    Button13.Enabled = True
                    IRC.ChatOff()
                End Sub)
            End If
        End Sub)
        StateThread.Start()
    End Sub

    Private Sub NewUserHandler(UserName As String, FreshUser As Boolean)
        WelcomeNewUser(UserName, FreshUser)
    End Sub

    Private Sub LogEventString(Message As String)
        BeginInvoke(Sub() DatastreamBox.Text = "- " & Message & vbCrLf & vbCrLf & DatastreamBox.Text)
    End Sub


    Private Sub IRCevent(IRCstring As String)
        UpdateEventBox(IRCstring)
    End Sub

    Private Sub GenericNotification(InputString As String)
        LogEventString(InputString)
    End Sub

    Private Sub CounterUpdated(CounterIndex As Integer)
        LogEventString(CounterData.Counters(CounterIndex).Name & " Counter Updated")
    End Sub

    Private Sub MediaStarted()
        LogEventString("OBS Music player started playback: " & AudioControl.MusicPlayer.Current)
        File.WriteAllText("\\StreamPC-V2\OBS Assets\Text\NowPlaying.txt", "Now Playing:  " & MPstringFormat(AudioControl.MusicPlayer.Current))
    End Sub

    Private Sub MediaPaused()
        LogEventString("OBS Music player paused")
        File.WriteAllText("\\StreamPC-V2\OBS Assets\Text\NowPlaying.txt", "")
    End Sub

    Private Sub MediaEnded()
        LogEventString("OBS Music player stopped")
        File.WriteAllText("\\StreamPC-V2\OBS Assets\Text\NowPlaying.txt", "")
    End Sub


    Private Sub TextBox3_keypress(sender As Object, e As KeyPressEventArgs) Handles TextBox3.KeyPress
        If e.KeyChar = Chr(13) Then
            'If ChatActive = True Then
            Dim InputText As String
            InputText = TextBox3.Text
            IRC.SendChat(InputText)
            TextBox3.Text = ""
            'End If
            e.Handled = True
        End If
    End Sub

    Private Sub CountersButt_Click(sender As Object, e As EventArgs) Handles CountersButt.Click
        If Counters.Visible = False Then
            If OBSconnected Then
                Counters = New OBScounters
                Counters.Show()
            End If
        Else
            Counters.Select()
        End If
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        If DemoPlayer.Visible Then
            DemoPlayer.Select()
        Else
            If OBSconnected Then
                DemoPlayer = New DemoModule
                DemoPlayer.Show()
            End If
        End If
    End Sub

    Private Sub SOUND_BOARD_Click(sender As Object, e As EventArgs) Handles SOUND_BOARD.Click
        If SoundBoard.Visible = False Then
            If OBSconnected Then
                SoundBoard = New OBSSoundBoard
                SoundBoard.Show()
            End If
        Else
            SoundBoard.Select()
        End If
    End Sub

    Private Sub CHANNEL_POINTS_Click(sender As Object, e As EventArgs) Handles CHANNEL_POINTS.Click
        If ChannelPointsDisplay.Visible = True Then
            ChannelPointsDisplay.Select()
        Else
            ChannelPointsDisplay = New ChannelPointsForm
            ChannelPointsDisplay.Show()
        End If
    End Sub

    Private Sub SCENE_SELECTOR_Click(sender As Object, e As EventArgs) Handles SCENE_SELECTOR.Click
        If SceneChanger.Visible = False Then
            If OBSconnected Then
                SceneChanger = New SceneSelector
                SceneChanger.Show()
            End If
        Else
            SceneChanger.Select()
        End If
    End Sub
    Private Sub TimersButt_Click(sender As Object, e As EventArgs) Handles TimersButt.Click
        If OBSTimers.Visible = False Then
            If OBSconnected Then
                OBSTimers = New TimerControls
                OBSTimers.Show()
            End If
        Else
            OBSTimers.Select()
        End If
    End Sub

    Private Sub CHARACTERS_Click(sender As Object, e As EventArgs) Handles CHARACTERS.Click
        If SpriteControls.Visible = False Then
            If OBSconnected Then
                SpriteControls = New OBScharacters
                SpriteControls.Show()
            End If
        Else
            SpriteControls.Select()
        End If
    End Sub


    Private Sub MUSIC_PLAYER_Click(sender As Object, e As EventArgs) Handles MUSIC_PLAYER.Click
        If MusicPlayer.Visible = False Then
            If OBSconnected Then
                MusicPlayer = New OBSMusicPlayer
                MusicPlayer.Show()
            End If
        Else
            MusicPlayer.Select()
        End If
    End Sub

    Private Sub ChatBUTT_Click(sender As Object, e As EventArgs) Handles ChatBUTT.Click
        If ChatManager.Visible = False Then
            'If ChatActive = True Then
            ChatManager = New Chat
            ChatManager.Show()
            ' End If
        Else
            ChatManager.Select()
        End If
    End Sub

    Private Sub NextMessage_Click(sender As Object, e As EventArgs) Handles NextMessage.Click
        ChatHighlighter.Image = Nothing
        NextMessage.Visible = False
    End Sub

    Private Sub NextEvent_Click(sender As Object, e As EventArgs) Handles NextEvent.Click
        EventHighlighter.Image = Nothing
        NextEvent.Visible = False
        'Call UpdateEventBox()
    End Sub

    Private Sub SoundBoardClosed()
        SOUND_BOARD.BackColor = StandardBUTT
    End Sub

    Private Sub SoundBoardOpened()
        SOUND_BOARD.BackColor = ActiveBUTT
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        myAPI.SyncChannelRedemptions()
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        'If ChatActive = True Then
        Dim FD As OpenFileDialog = New OpenFileDialog
            FD.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
            FD.FilterIndex = 2

            If FD.ShowDialog() = DialogResult.OK Then
                Dim filepath = FD.FileName
                Dim BanThread As New Thread(
                Sub()
                    If File.Exists(filepath) = True Then
                        Dim Reader As StreamReader = File.OpenText(filepath)
                        Dim Outputtext As String
                        Do Until Reader.EndOfStream = True
                            Outputtext = "/ban " & Reader.ReadLine
                            IRC.SendChat(Outputtext)
                            Thread.Sleep(1000)
                        Loop
                        Reader.Close()
                        Reader.Dispose()
                    End If
                End Sub)
                BanThread.Start()
            End If
        'End If
    End Sub


    Public Sub ConnectPubSub()
        PubSub.ListenToChannelPoints(JHID)
        PubSub.ListenToVideoPlayback(JHID)
        PubSub.ListenToBitsEventsV2(JHID)
        PubSub.ListenToFollows(JHID)
        PubSub.ListenToRaid(JHID)
        PubSub.ListenToSubscriptions(JHID)
        PubSub.Connect()
        PubSub.SendTopics(myAPI.AccessToken)
    End Sub


    Private Sub WaitforAuthorization()
        TwitchAuthorized = True
        AUTHdisplay.BackColor = ActiveBUTT
        LogEventString("Authorization Recieved")
        If PSubConnected = False Then
            ConnectPubSub()
        End If


    End Sub

    Private Sub ChannelPointsStarter()
        'LogEventString("Channel-Point Reward Data Updated")
        If ChannelPointStarterState = True Then
            ChannelPoints.UpdateProgramRewards(True)
            ChannelPointStarterState = False
            'LaunchChannelPointsControls()
        End If
    End Sub

    'Private Sub LaunchChannelPointsControls()

    'End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        If OBSconnected Then
            If OBSstreamState() = False Then
                If Button13.Text = "START STREAM" Then
                    Button13.Enabled = False
                    ShowStrimStarter()
                Else
                    Button13.Text = "START STREAM"
                    Button13.BackColor = StandardBUTT
                End If
            Else
                If Button13.Text = "END STREAM" Then
                    Button13.Enabled = False
                    OBS.StopStream()
                Else
                    Button13.Text = "END STREAM"
                    Button13.BackColor = ActiveBUTT
                End If
            End If
        End If
    End Sub



    Private Sub ShowStrimStarter()
        StrimStarter = New StreamStarter
        StrimStarter.Show()
    End Sub

    Private Sub ListenResponse(sender As Object, e As Events.OnListenResponseArgs)
        If e.Successful = False Then
            LogEventString("Failed to Listen: " & e.Response.Error)
        Else
            LogEventString(e.Topic & " Message Recieved")
        End If
    End Sub

    Private Sub ChannelPointsNotification(sender As Object, e As Events.OnChannelPointsRewardRedeemedArgs)
        Dim Rewardstring As String = ""

        Select Case e.RewardRedeemed.Redemption.Reward.Title
            Case = "Ember's Hats"
                MyOBSevents.PlayHats()
            Case = "Play Random Sound-Alert"
                MyOBSevents.PlaySoundAlert(e.RewardRedeemed.Redemption.User.DisplayName)
            Case = "Play Sound-Alert"
                MyOBSevents.PlaySoundAlert(e.RewardRedeemed.Redemption.User.DisplayName, Replace(e.RewardRedeemed.Redemption.UserInput, "_", " "))
            Case = "Rock and Stone"
                Dim Speak As Task = Ember.Says("ROCK AND STONE" & vbCrLf & e.RewardRedeemed.Redemption.User.DisplayName,
                           Ember.Mood.RockandStone, "Rock And Stone",, 2800)
            Case = "Poo-Brain"
                Luna.ChangeMood(Luna.Mood.PooBrain, 4050,, "Luna Poo Brain")
            Case Else
                Rewardstring = "#CHPTS " & e.RewardRedeemed.Redemption.User.DisplayName & " Redeemed: " &
                e.RewardRedeemed.Redemption.Reward.Title & " for " &
                e.RewardRedeemed.Redemption.Reward.Cost & " points"
                Invoke(Sub() UpdateEventBox(Rewardstring))
        End Select

    End Sub

    Public Sub iGOTStheBITS(sender As Object, e As Events.OnBitsReceivedV2Args)
        Dim MYSTRING As String = e.TotalBitsUsed

    End Sub

    Public Sub SubscriberAcquired(sender As Object, e As Events.OnChannelSubscriptionArgs)
        Dim MYSTRING As String = e.Subscription.DisplayName

    End Sub

    Private Sub ViewCountChanged(sender As Object, e As Events.OnViewCountArgs)
        Dim OutputString As String = "View CouNt = " & e.Viewers
        LogEventString(OutputString)
    End Sub


    Private Sub UpdateChatBox(UserName As String, MessageData As String, TimeString As String, ColorIndex As Integer)
        Dim SplitString() As String = TimeString.Split(" ")
        Dim OutputText As String = UserName & "(" & SplitString(1) & "): " & MessageData
        My.Computer.Audio.Play(My.Resources.LOZ_Alert_2, AudioPlayMode.Background)
        ChatBox.ForeColor = ChatUserInfo.ChatColor(ColorIndex)
        ChatBox.Text = OutputText
        ChatHighlighter.Image = My.Resources.gradient
        NextMessage.Visible = True
    End Sub

    Private Sub UpdateEventBox(EventString As String)
        Dim OutputText As String = ""
        EventHighlighter.Image = My.Resources.gradient
        NextEvent.Visible = True
        My.Computer.Audio.Play(My.Resources.LOZ_secret, AudioPlayMode.Background)
        EventBox.Text = EventString
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'MyOBSevents.ViewerEvent(RandomInt(0, 1), ChatUserInfo.AllChatUsers(RandomInt(0, ChatUserInfo.AllChatUsers.Count - 1)).UserName)
        MyOBSevents.Roll6(RandomInt(1, 3), ChatUserInfo.AllChatUsers(RandomInt(0, ChatUserInfo.AllChatUsers.Count - 1)).UserName)
    End Sub


End Class

