
Imports System.Net.Sockets
Imports System.Threading
Imports System.Media
Imports System.Math
Imports System.IO
Imports System.Collections.Concurrent
Imports OBSWebsocketDotNet
Imports OBSWebsocketDotNet.Types
Imports TwitchLib.PubSub

Public Class MainWindow

    Private CurrentSceneName As String = ""
    Private MainProgramRunning As Boolean = False, Estream1 As NetworkStream, Estream2 As NetworkStream, OpenPort1 As ToTcpClient,
        OpenPort2 As ToTcpClient, MessageBuffer As ConcurrentQueue(Of String), 'ChatBuffer As ConcurrentQueue(Of String),
        Port1Active As Boolean = False, Port2Active As Boolean = False, ChatActive As Boolean = False, ChannelPart As Boolean = False,
        SendBuffer As ConcurrentQueue(Of String), PONG As Boolean = False, 'ChatOutputBuffer As ConcurrentQueue(Of String), 
        PubSubState As Boolean = False, StrimStarter As StreamStarter
    Private StrimStarterState As Boolean = False
    Private PubSubStarterState As Boolean = False
    Private ChannelPointStarterState As Boolean

    'Public Event OBSSceneChanged(SceneName As String)

    Private Sub MainWindow_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        SetDrawing(Me.Handle, WM_SETREDRAW, False, 0)
        'InitializeCountersAndTimers()
        ReadProgramSettings(ProgramSettingsFile)

        OBS = New OBSWebsocket
        OBSmutex = New Mutex
        SourceWindow = Me
        ChannelPointsDisplay = New ChannelPointsForm
        ChannelPoints = New ChannelPointData
        SceneChanger = New SceneSelector
        GamesList = New TwitchGames
        ChatManager = New Chat
        OBSTimers = New TimerControls
        TimerCollection = New OBSTimerData
        SpriteControls = New OBScharacters
        MessageBuffer = New ConcurrentQueue(Of String)
        AudioControl = New OBSaudioPlayer
        MusicPlayer = New OBSMusicPlayer
        SoundBoard = New OBSSoundBoard
        Counters = New OBScounters
        CounterData = New OBScounterData
        ChatUserInfo = New UserData
        Ember = New CharacterControls
        Ember.initEMBER()
        Luna = New CharacterControls
        Luna.initLUNA()

        myAPI = New APIfunctions
        PubSub = New TwitchPubSub

        AddHandler ChannelPoints.AllRewardsUpdated, AddressOf ChannelPointsStarter
        AddHandler OBS.SceneChanged, AddressOf SceneChangeDetected
        AddHandler OBS.StreamingStateChanged, AddressOf StreamStartHandler
        AddHandler myAPI.StreamInfoAvailable, AddressOf LaunchStreamStarter
        AddHandler myAPI.AuthorizationInitialized, AddressOf WaitforAuthorization
        AddHandler myAPI.TokenAcquired, AddressOf Authorized
        AddHandler Ember.Said, AddressOf GenericNotification
        AddHandler Luna.Said, AddressOf GenericNotification
        AddHandler ChatUserInfo.ChatUserDetected, AddressOf NewUserHandler
        AddHandler ChatUserInfo.MessageRecieved, AddressOf UpdateChatBox
        AddHandler CounterData.CounterStarted, AddressOf CounterUpdated

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
        AddHandler AudioControl.SoundPlayer.Started, AddressOf SoundStarted
        AddHandler AudioControl.SoundPlayer.Paused, AddressOf SoundPaused
        AddHandler AudioControl.SoundPlayer.Stopped, AddressOf SoundEnded

        SetDrawing(Me.Handle, WM_SETREDRAW, True, 0)
        Me.Refresh()
    End Sub


    Private Sub MainWindow_Closing(sender As Object, e As EventArgs) Handles MyBase.Closing
        CounterData.SaveCounterData()
        OBStimerObject(TimerIDs.Ember).State = False
        OBStimerObject(TimerIDs.Luna).State = False
        OBStimerObject(TimerIDs.GlobalCC).State = False
        If AudioControl.MusicPlayer.Active = True Then AudioControl.StopMusic()

        MainProgramRunning = False
        Port1Active = False
        Port2Active = False
        If ChatActive = True Then
            ChannelPart = True
            Do Until ChatActive = False
                Application.DoEvents()
            Loop
        End If
        If PubSubState = True Then PubSub.Disconnect()

        RemoveHandler ChannelPoints.AllRewardsUpdated, AddressOf ChannelPointsStarter
        RemoveHandler OBS.StreamingStateChanged, AddressOf StreamStartHandler
        RemoveHandler myAPI.StreamInfoAvailable, AddressOf LaunchStreamStarter
        RemoveHandler myAPI.AuthorizationInitialized, AddressOf WaitforAuthorization
        RemoveHandler myAPI.TokenAcquired, AddressOf Authorized
        RemoveHandler Ember.Said, AddressOf GenericNotification
        RemoveHandler Luna.Said, AddressOf GenericNotification
        RemoveHandler ChatUserInfo.ChatUserDetected, AddressOf NewUserHandler
        RemoveHandler ChatUserInfo.MessageRecieved, AddressOf UpdateChatBox
        RemoveHandler CounterData.CounterStarted, AddressOf CounterUpdated

        RemoveHandler PubSub.OnViewCount, AddressOf ViewCountChanged
        RemoveHandler PubSub.OnChannelPointsRewardRedeemed, AddressOf ChannelPointsNotification
        RemoveHandler PubSub.OnListenResponse, AddressOf ListenResponse
        RemoveHandler PubSub.OnBitsReceivedV2, AddressOf iGOTStheBITS
        RemoveHandler PubSub.OnChannelSubscription, AddressOf SubscriberAcquired
        RemoveHandler PubSub.OnPubSubServiceConnected, AddressOf PubSubConnected
        RemoveHandler PubSub.OnPubSubServiceClosed, AddressOf PubSubDisconnect

        RemoveHandler AudioControl.MusicPlayer.Started, AddressOf MediaStarted
        RemoveHandler AudioControl.MusicPlayer.Paused, AddressOf MediaPaused
        RemoveHandler AudioControl.MusicPlayer.Stopped, AddressOf MediaEnded
        RemoveHandler AudioControl.SoundPlayer.Started, AddressOf SoundStarted
        RemoveHandler AudioControl.SoundPlayer.Paused, AddressOf SoundPaused
        RemoveHandler AudioControl.SoundPlayer.Stopped, AddressOf SoundEnded

        Call DisconnectOBS()
        Application.DoEvents()
        Thread.Sleep(500)

    End Sub
    Private Sub Authorized(TokenMessage As String)
        BeginInvoke(Sub() LogEventString(TokenMessage))
    End Sub

    Private Sub StreamStartHandler()
        Dim StateThread As New Thread(
            Sub()
                'SendMessage("triggered")
                If OBSstreamState() = True Then
                    BeginInvoke(
                        Sub()
                            Button13.Text = "END STREAM"
                            Button13.BackColor = ActiveBUTT
                        End Sub)
                Else
                    BeginInvoke(
                        Sub()
                            Button13.Text = "START STREAM"
                            Button13.BackColor = StandardBUTT
                        End Sub)
                End If
            End Sub)
        StateThread.Start()
    End Sub

    Private Sub NewUserHandler(UserName As String, FreshUser As Boolean)
        If UserName <> Broadcastername Then
            If CheckOBSconnect() = True Then
                Dim MessageType As String
                If FreshUser = True Then
                    MessageType = "Greetings"
                Else
                    MessageType = "Returns"
                End If
                If CurrentScene.Name = "Center Screen Mode" Or CurrentScene.Name = "Dual Screen Mode" Then
                    Dim CoinFlip As Integer = RandomInt(0, 1)
                    If CoinFlip = 0 Then
                        Luna.Says(RandomMessage(UserName, MessageType), Luna.Mood.Cringe, "Hello")
                    Else
                        Ember.Says(RandomMessage(UserName, MessageType), Ember.Mood.Happy, "Hello")
                    End If
                Else
                    If EmberSpriteB = True And LunaSpriteB = True Then
                        Dim CoinFlip As Integer = RandomInt(0, 1)
                        If CoinFlip = 0 Then
                            Luna.Says(RandomMessage(UserName, MessageType), Luna.Mood.Cringe, "Hello")
                        Else
                            Ember.Says(RandomMessage(UserName, MessageType), Ember.Mood.Happy, "Hello")
                        End If
                    Else
                        If EmberSpriteB = True And LunaSpriteB = False Then Ember.Says(RandomMessage(UserName, MessageType), Ember.Mood.Happy, "Hello")
                        If EmberSpriteB = False And LunaSpriteB = True Then Luna.Says(RandomMessage(UserName, MessageType), Luna.Mood.Cringe, "Hello")
                        If EmberSpriteB = False And LunaSpriteB = False Then
                            SendBuffer.Enqueue(RandomMessage(UserName, MessageType))
                            AudioControl.PlaySoundAlert(AudioControl.GetSoundFileDataByName("Hello"))
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub LogEventString(Message As String)
        DatastreamBox.Text = Message & vbCrLf & DatastreamBox.Text
    End Sub

    Private Sub GenericNotification(InputString As String)
        BeginInvoke(Sub() LogEventString(InputString))
    End Sub

    Private Sub CounterUpdated(CounterIndex As Integer)
        BeginInvoke(Sub() LogEventString(CounterData.Counters(CounterIndex).Name & " Counter Updated"))
    End Sub

    Private Sub MediaStarted()
        BeginInvoke(Sub() LogEventString("OBS Music player started playback: " & AudioControl.MusicPlayer.Current))
        File.WriteAllText("\\StreamPC-V2\OBS Assets\Text\NowPlaying.txt", "Now Playing:  " & MPstringFormat(AudioControl.MusicPlayer.Current))
    End Sub

    Private Sub MediaPaused()
        BeginInvoke(Sub() LogEventString("OBS Music player paused"))
        File.WriteAllText("\\StreamPC-V2\OBS Assets\Text\NowPlaying.txt", "")
    End Sub

    Private Sub MediaEnded()
        BeginInvoke(Sub() LogEventString("OBS Music player stopped"))
        File.WriteAllText("\\StreamPC-V2\OBS Assets\Text\NowPlaying.txt", "")
    End Sub

    Private Sub SoundStarted()
        BeginInvoke(Sub() LogEventString("OBS Sound-Board Played: " & AudioControl.SoundPlayer.Current))
    End Sub

    Private Sub SoundPaused()
        BeginInvoke(Sub() LogEventString("OBS Sound-Board paused"))
    End Sub

    Private Sub SoundEnded()
        BeginInvoke(Sub() LogEventString("OBS Sound-Board stopped"))
    End Sub

    Private Sub TextBox3_keypress(sender As Object, e As KeyPressEventArgs) Handles TextBox3.KeyPress
        If e.KeyChar = Chr(13) Then
            If ChatActive = True Then
                Dim InputText As String
                InputText = TextBox3.Text
                SendBuffer.Enqueue(InputText)
                TextBox3.Text = ""
            End If
            e.Handled = True
        End If
    End Sub

    Private Sub PortConnect2_Click(sender As Object, e As EventArgs) Handles PortConnect2.Click

    End Sub

    Private Sub ChatSpeaker(ThisIRC As IrcClient)
        SendBuffer = New ConcurrentQueue(Of String)
        Do Until ChannelPart = True
            Dim OutputMessage As String = ""
            If SendBuffer.Count <> 0 Then
                If SendBuffer.TryDequeue(OutputMessage) = True Then
                    If OutputMessage <> "" Then ThisIRC.SendPublicChatMessage(OutputMessage)
                End If
            End If
        Loop
        ThisIRC.SendIrcMessage("PART #" & Broadcastername)
        ChannelPart = False
    End Sub

    Private Sub ConnectChat()
        Dim InputMessage As String = ""
        Dim IRC As New IrcClient("irc.twitch.tv", 6667, BotName, twitchOAuth, Broadcastername)

        Dim SenderThread As Thread
        SenderThread = New Thread(AddressOf ChatSpeaker)
        SenderThread.Start(IRC)

        Do Until ChatActive = False
            InputMessage = IRC.ReadMessage
            If InputMessage = "PING :tmi.twitch.tv" Then IRC.SendIrcMessage("PONG :tmi.twitch.tv")
            If InputMessage <> "" Then ReadChat(InputMessage)
            If InputMessage = ":" & BotName & "!" & BotName & "@" & BotName & ".tmi.twitch.tv PART #" & Broadcastername Then _
            ChatActive = False
        Loop

        IRC.Close()
    End Sub

    Private Sub CountersButt_Click(sender As Object, e As EventArgs) Handles CountersButt.Click
        If Counters.Visible = False Then
            If CheckOBSconnect() = True Then
                Counters = New OBScounters
                Counters.Show()
            End If
        Else
            Counters.BeginInvoke(Sub() Counters.Select())
        End If
    End Sub

    Private Sub SOUND_BOARD_Click(sender As Object, e As EventArgs) Handles SOUND_BOARD.Click
        If SoundBoard.Visible = False Then
            If CheckOBSconnect() = True Then
                SoundBoard = New OBSSoundBoard
                SoundBoard.Show()
            End If
        Else
            SoundBoard.BeginInvoke(Sub() SoundBoard.Select())
        End If
    End Sub

    Private Sub CHANNEL_POINTS_Click(sender As Object, e As EventArgs) Handles CHANNEL_POINTS.Click
        If ChannelPointsDisplay.Visible = True Then
            ChannelPointsDisplay.BeginInvoke(Sub() ChannelPointsDisplay.Select())
        Else
            If myAPI.Authorized = True Then
                If ChannelPoints.Rewards IsNot Nothing Then
                    LaunchChannelPointsControls()
                Else
                    ChannelPointStarterState = True
                    myAPI.SyncChannelRedemptions()
                End If
            Else
                ChannelPointStarterState = True
                myAPI.APIauthorization(True)
            End If
        End If
    End Sub

    Private Sub SCENE_SELECTOR_Click(sender As Object, e As EventArgs) Handles SCENE_SELECTOR.Click
        If SceneChanger.Visible = False Then
            If CheckOBSconnect() = True Then
                SceneChanger = New SceneSelector
                SceneChanger.Show()
            End If
        Else
            SceneChanger.BeginInvoke(Sub() SceneChanger.Select())
        End If
    End Sub
    Private Sub TimersButt_Click(sender As Object, e As EventArgs) Handles TimersButt.Click
        If OBSTimers.Visible = False Then
            If CheckOBSconnect() = True Then
                OBSTimers = New TimerControls
                OBSTimers.Show()
            End If
        Else
            OBSTimers.BeginInvoke(Sub() OBSTimers.Select())
        End If
    End Sub

    Private Sub CHARACTERS_Click(sender As Object, e As EventArgs) Handles CHARACTERS.Click
        If SpriteControls.Visible = False Then
            If CheckOBSconnect() = True Then
                SpriteControls = New OBScharacters
                SpriteControls.Show()
            End If
        Else
            SpriteControls.BeginInvoke(Sub() SpriteControls.Select())
        End If
    End Sub


    Private Sub MUSIC_PLAYER_Click(sender As Object, e As EventArgs) Handles MUSIC_PLAYER.Click
        If MusicPlayer.Visible = False Then
            If CheckOBSconnect() = True Then
                MusicPlayer = New OBSMusicPlayer
                MusicPlayer.Show()
            End If
        Else
            MusicPlayer.BeginInvoke(Sub() MusicPlayer.Select())
        End If
    End Sub

    Private Sub ChatBUTT_Click(sender As Object, e As EventArgs) Handles ChatBUTT.Click
        If ChatManager.Visible = False Then
            If ChatActive = True Then
                ChatManager = New Chat
                ChatManager.Show()
            End If
        Else
            ChatManager.BeginInvoke(Sub() ChatManager.Select())
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

    Private Sub IRCclientConnect()
        ChatConnect.Enabled = False
        ChatConnect.Text = "Connecting to Chat"
        Application.DoEvents()
        ChatActive = True
        Dim ChatThread As Thread
        ChatThread = New Thread(AddressOf ConnectChat)
        ChatThread.Start()
        ChatConnect.Text = "Disconnect Chat"
        ChatConnect.BackColor = ActiveBUTT
        ChatConnect.Enabled = True
        If MainProgramRunning = False Then
            MainProgramRunning = True
            Call RunMainProgram()
        End If
    End Sub

    Private Sub IRCclientDisconnect()
        ChatConnect.Enabled = False
        ChannelPart = True
        Do Until ChatActive = False
            Application.DoEvents()
        Loop
        If Port2Active = False And Port1Active = False Then
            MainProgramRunning = False
        End If
        Thread.Sleep(500)
        ChatConnect.Text = "Connect Chat"
        ChatConnect.BackColor = StandardBUTT
        ChatConnect.Enabled = True
    End Sub

    Private Sub ChatConnect_Click(sender As Object, e As EventArgs) Handles ChatConnect.Click
        If ChatConnect.Text = "Connect Chat" Then
            IRCclientConnect()
        ElseIf ChatConnect.Text = "Disconnect Chat" Then
            IRCclientDisconnect()
        End If
    End Sub

    Private Sub PortConnect1_Click(sender As Object, e As EventArgs) Handles PortConnect1.Click
        If PortConnect1.Text = "Connect Port 1" Then
            PortConnect1.Enabled = False
            PortConnect1.Text = "Connecting to Port 1"
            Application.DoEvents()
            Port1Active = IsPort1Open(IPaddress1.Text, 23)
            If Port1Active = True Then
                Dim Port1Thread As Thread
                Port1Thread = New Thread(AddressOf ReadStream1)
                Port1Thread.Start()
                PortConnect1.Text = "Disconnect Port 1"
                PortConnect1.BackColor = ActiveBUTT
                PortConnect1.Enabled = True
                If MainProgramRunning = False Then
                    Dim RunThread As New Thread(
                        Sub()
                            MainProgramRunning = True
                            Call RunMainProgram()
                        End Sub)
                    RunThread.Start()
                End If
            Else
                SendMessage("Connection Failed", "FML")
                PortConnect1.Text = "Connect Port 1"
                PortConnect1.Enabled = True
            End If
        ElseIf PortConnect1.Text = "Disconnect Port 1" Then
            PortConnect1.Enabled = False
            PortConnect1.Text = "Disconnecting Port 1"
            Application.DoEvents()
            Port1Active = False
            If Port2Active = False And ChatActive = False Then
                MainProgramRunning = False
            End If
            Thread.Sleep(500)
            PortConnect1.Text = "Connect Port 1"
            PortConnect1.BackColor = StandardBUTT
            PortConnect1.Enabled = True
        End If
    End Sub

    Private Sub SoundBoardClosed()
        SOUND_BOARD.BackColor = StandardBUTT
    End Sub

    Private Sub SoundBoardOpened()
        SOUND_BOARD.BackColor = ActiveBUTT
    End Sub

    Private Sub PubSubConnected(sender As Object, e As EventArgs)
        BeginInvoke(
            Sub()
                Button1.BackColor = ActiveBUTT
                Button1.Text = "Disconnect PubSub"
                PubSubState = True
                DatastreamBox.Text = "PubSub Connected" & vbCrLf & DatastreamBox.Text
            End Sub)
    End Sub

    Public Sub ChatReady()
        If ChatActive = True Then
            SendBuffer.Enqueue("/emoteonlyoff")
            SendBuffer.Enqueue("/subscribersoff")
        End If
    End Sub

    Public Sub ChatOff()
        If ChatActive = True Then
            SendBuffer.Enqueue("/emoteonly")
            SendBuffer.Enqueue("/subscribers")
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        myAPI.SyncChannelRedemptions()
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        If ChatActive = True Then
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
                            SendBuffer.Enqueue(Outputtext)
                            Thread.Sleep(1000)
                        Loop
                        Reader.Close()
                        Reader.Dispose()
                    End If
                End Sub)
                BanThread.Start()
            End If
        End If
    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox3.TextChanged

    End Sub

    Public Sub PubSubDisconnect(sender As Object, e As EventArgs)
        BeginInvoke(
            Sub()
                Button1.BackColor = StandardBUTT
                Button1.Text = "Connect PubSub"
                PubSubState = False
                DatastreamBox.Text = "PubSub Disconnected" & vbCrLf & DatastreamBox.Text
            End Sub)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If myAPI.Authorized = True Then
            Button1.Enabled = False
            If Button1.Text = "Connect PubSub" Then
                ConnectPubSub()
            Else
                PubSub.Disconnect()
            End If
            Button1.Enabled = True
        Else
            Button1.Enabled = False
            PubSubStarterState = True
            myAPI.APIauthorization(True)
            'SendMessage("Need Authorization")
        End If
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

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        myAPI.APIauthorization(True)
    End Sub

    Private Sub WaitforAuthorization()
        If StrimStarterState = True Then
            Dim GoThread As New Thread(
            Sub()
                BeginInvoke(Sub() LogEventString("Authorization Recieved"))
                If PubSubState = False Then
                    ConnectPubSub()
                End If
                myAPI.GetStreamInfoSub()
            End Sub)
            GoThread.Start()
        End If
        If PubSubStarterState = True Then
            Dim GoThread As New Thread(
                Sub()
                    BeginInvoke(Sub() LogEventString("Authorization Recieved"))
                    If PubSubState = False Then
                        ConnectPubSub()
                    End If
                    BeginInvoke(Sub() Button1.Enabled = True)
                    PubSubStarterState = False
                End Sub)
            GoThread.Start()
        End If
        If ChannelPointStarterState = True Then
            Dim GoThread As New Thread(
                Sub()
                    BeginInvoke(Sub() LogEventString("Authorization Recieved"))
                    If ChannelPoints.Rewards IsNot Nothing Then
                        LaunchChannelPointsControls()
                        ChannelPointStarterState = False
                    Else
                        myAPI.SyncChannelRedemptions()
                    End If
                End Sub)
            GoThread.Start()
        End If
    End Sub

    Private Sub ChannelPointsStarter()
        BeginInvoke(Sub() LogEventString("Channel-Point Reward Data Updated"))
        If ChannelPointStarterState = True Then
            ChannelPointStarterState = False
            BeginInvoke(Sub() LaunchChannelPointsControls())
        End If
    End Sub

    Private Sub LaunchChannelPointsControls()
        ChannelPointsDisplay = New ChannelPointsForm
        ChannelPointsDisplay.Show()
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        If CheckOBSconnect() = True Then
            If OBSstreamState() = False Then
                If Button13.Text = "START STREAM" Then
                    StrimStarterState = True
                    Dim StartStream As Thread = New Thread(
                        Sub()
                            If ChatActive = False Then
                                BeginInvoke(Sub() IRCclientConnect())
                            End If
                            If myAPI.Authorized = False Then
                                BeginInvoke(Sub() LogEventString("Getting Authorization"))
                                myAPI.APIauthorization(True)
                            Else
                                If PubSubState = False Then
                                    ConnectPubSub()
                                End If
                                myAPI.GetStreamInfoSub()
                            End If
                        End Sub)
                    StartStream.Start()
                Else
                    Button13.Text = "START STREAM"
                    Button13.BackColor = StandardBUTT
                End If
            Else
                If Button13.Text = "END STREAM" Then
                    If ChatActive = True Then
                        ChatOff()
                        Thread.Sleep(300)
                        BeginInvoke(Sub() IRCclientDisconnect())
                    End If
                    If PubSubState = True Then
                        PubSub.Disconnect()
                    End If
                    OBS.StopStreaming()
                Else
                    Button13.Text = "END STREAM"
                    Button13.BackColor = ActiveBUTT
                End If
            End If
        End If
    End Sub

    Public Sub LaunchStreamStarter(CurrentStreamInfo As String)
        If StrimStarterState = True Then
            Dim GoThread As New Thread(
            Sub()
                BeginInvoke(Sub() LogEventString(CurrentStreamInfo))
                BeginInvoke(Sub() ShowStrimStarter())
                StrimStarterState = False
            End Sub)
            GoThread.Start()
        End If
    End Sub

    Private Sub ShowStrimStarter()
        StrimStarter = New StreamStarter
        StrimStarter.Show()
    End Sub

    Private Sub ListenResponse(sender As Object, e As Events.OnListenResponseArgs)
        If e.Successful = False Then
            BeginInvoke(
                Sub()
                    DatastreamBox.Text = "Failed to Listen: " & e.Response.Error & vbCrLf & DatastreamBox.Text
                End Sub)
        Else
            BeginInvoke(
                Sub()
                    DatastreamBox.Text = e.Topic & " Message Recieved" & vbCrLf & DatastreamBox.Text
                End Sub)
        End If
    End Sub

    Private Sub ChannelPointsNotification(sender As Object, e As Events.OnChannelPointsRewardRedeemedArgs)
        Dim Rewardstring As String = ""

        Select Case e.RewardRedeemed.Redemption.Reward.Title
            Case = "Ember's Hats"
                Call MediaSourceChange("Embers Hats", MediaFcode.Restart)
            Case = "Play Random Sound-Alert"
                AudioControl.PlaySoundAlert(AudioControl.GetRandomPublicSoundFile)
            Case = "Play Sound-Alert"
                Dim SoundFileName As String = Replace(e.RewardRedeemed.Redemption.UserInput, "_", " ")
                AudioControl.PlaySoundAlert(AudioControl.GetSoundFileDataByName(SoundFileName))
            Case Else
                Rewardstring = "#CHPTS " & e.RewardRedeemed.Redemption.User.DisplayName & " Redeemed: " &
                e.RewardRedeemed.Redemption.Reward.Title & " for " &
                e.RewardRedeemed.Redemption.Reward.Cost & " points"
                Call UpdateEventBox(Rewardstring)
        End Select

    End Sub
    'Private Sub ChannelPointsNotificationV2(sender As Object, e As Events.OnRewardRedeemedArgs)
    'Dim Rewardstring As String = ""
    '   Rewardstring = "#CHPTS " & e.DisplayName & " Redeemed: " &
    '      e.RewardTitle & " for " &
    '     e.RewardCost & " points"
    'Call UpdateEventBox(Rewardstring)
    'End Sub

    Public Sub iGOTStheBITS(sender As Object, e As Events.OnBitsReceivedV2Args)
        Dim MYSTRING As String = e.TotalBitsUsed
    End Sub

    Public Sub SubscriberAcquired(sender As Object, e As Events.OnChannelSubscriptionArgs)
        Dim MYSTRING As String = e.Subscription.DisplayName

    End Sub

    Private Sub ViewCountChanged(sender As Object, e As Events.OnViewCountArgs)
        Dim OutputString As String = "View Count = " & e.Viewers
        BeginInvoke(Sub() LogEventString(OutputString))
    End Sub



    Private Sub RunMainProgram()
        Dim MessageString As String = ""
        Do Until MainProgramRunning = False
            If MessageBuffer.Count <> 0 Then
                If MessageBuffer.TryDequeue(MessageString) = True Then
                    Call ReadMessage(MessageString)
                End If
            End If
            Application.DoEvents()
        Loop
    End Sub

    Private Sub ReadChat(InputString As String)

        Dim UserName As String, ChatText As String = "", Index As Integer, Findstring As String = "PRIVMSG #jerinhaus :"
        UserName = DataExtract(InputString, ":", "!")

        Index = InStr(InputString, Findstring)

        If Index <> 0 Then
            Index = Index + Len(Findstring)
            ChatText = Mid(InputString, Index, Len(InputString) - Index + 1)
        End If
        If UserName <> "" And ChatText <> "" Then
            If UserName = "nightbot" Then
                Select Case ChatText
                    Case = "We have llamas: https://www.deviantart.com/drawingwithjerin"
                    Case = "Beware of doggos: https://discord.gg/nG7PTTY"
                    Case = "ok boomer: https://www.facebook.com/DrawingWithJerin"
                    Case = "Come see our pretty pictures ^_^: https://www.instagram.com/drawingwithjerin/"
                    Case = "All the things!!! Website(www.drawingwithjerin.com),  Store(https://drawingwithjerin.com/jerinhaus/shop/), Youtube(https://www.youtube.com/channel/UCrDFqdGFty2YkM57fSE0VOg), Insta(https://www.instagram.com/jerinhaus/), Twitter(https://twitter.com/Jerinhaus), DA(https://www.deviantart.com/drawingwithjerin),  FB(https://www.facebook.com/Jerinhaus/)"
                    Case = "Have fun storming the castle: https://twitter.com/JerinDrawing"
                    Case = "And this is a website! It's dot com: https://drawingwithjerin.com/"
                    Case = "*In Bro Speak: ""Please subscribe To my youtube channel"" https://www.youtube.com/channel/UCrDFqdGFty2YkM57fSE0VOg"
                    Case Else
                        'EventOutputBuffer.Enqueue(ChatText)
                        Call UpdateEventBox(ChatText)
                End Select
            Else
                If ChatText.Substring(0, 1) <> "!" Then
                    ChatUserInfo.RecieveChatMessage(UserName, ChatText, DateAndTime.Now.ToString)
                    'ChatOutputBuffer.Enqueue(UserName & ": " & ChatText)
                    'Call UpdateChatBox()
                Else
                    Dim Splitstring() As String = ChatText.Split(" ")
                    Splitstring(0) = Splitstring(0).Replace("!", "")
                    Select Case Splitstring(0)
                        Case = "counts"
                            SendBuffer.Enqueue("@" & UserName & " " & CounterData.ReadPublicCounters)
                        Case = "count"
                            If Splitstring.Length > 1 Then
                                If CheckOBSconnect() = True Then
                                    If CounterData.UserCount(Splitstring(1)) = True Then
                                        SendBuffer.Enqueue("@" & UserName & " Counted " & Splitstring(1))
                                    End If
                                End If
                            End If
                        Case = "soundlist"
                            AudioControl.UpdatePublicSoundFileIndex()
                            SendBuffer.Enqueue("@" & UserName & " " & AudioControl.PublicSoundListLink)
                    End Select
                End If
            End If
            BeginInvoke(Sub() DatastreamBox.Text = UserName & ": " & ChatText & vbCrLf & DatastreamBox.Text)
        Else
            BeginInvoke(Sub() DatastreamBox.Text = InputString & vbCrLf & DatastreamBox.Text)
        End If

    End Sub

    Private Sub UpdateChatBox(UserName As String, MessageData As String, TimeString As String, ColorIndex As Integer)
        BeginInvoke(
            Sub()
                Dim SplitString() As String = TimeString.Split(" ")
                Dim OutputText As String = UserName & "(" & SplitString(1) & "): " & MessageData
                'Audio Notifications:
                My.Computer.Audio.Play(My.Resources.LOZ_Alert_2, AudioPlayMode.Background)
                ChatBox.ForeColor = ChatUserInfo.ChatColor(ColorIndex)
                ChatBox.Text = OutputText
                ChatHighlighter.Image = My.Resources.gradient
                NextMessage.Visible = True
            End Sub)
    End Sub

    Private Sub UpdateEventBox(EventString As String)
        BeginInvoke(
            Sub()
                Dim OutputText As String = ""
                'Tryagain:
                'If EventOutputBuffer.Count <> 0 Then
                'If EventHighlighter.Image Is Nothing Then
                'If ChatHighlighter.Image Is Nothing Then _
                EventHighlighter.Image = My.Resources.gradient
                NextEvent.Visible = True
                'End If
                'If EventOutputBuffer.TryDequeue(OutputText) = True Then
                My.Computer.Audio.Play(My.Resources.LOZ_secret, AudioPlayMode.Background)
                EventBox.Text = EventString
                'Else
                'GoTo Tryagain
                'End If
                'Else
                'EventHighlighter.Image = Nothing
                'NextEvent.Visible = False
                'End If
            End Sub)
    End Sub

    Private Sub ReadMessage(InputString As String)

        DatastreamBox.Text = InputString & vbCrLf & DatastreamBox.Text
        Select Case InputString
            Case = "Ember, Standard"
                'EmberBox.Image = My.Resources.ResourceManager.GetObject("ember_blink")
            Case = "Ember, Happy"
                'EmberBox.Image = My.Resources.ResourceManager.GetObject("ember_happy")
            Case = "Ember, Wumpy"
                'EmberBox.Image = My.Resources.ResourceManager.GetObject("ember_annoyed")
        End Select

    End Sub

    Private Sub ReadStream1()

        Dim InputBytes(1) As Byte, OutputString As String = ""

        Do Until Port1Active = False
            If OpenPort1.Connected = True Then
                If Estream1.DataAvailable = True Then
                    Do Until Estream1.DataAvailable = False
                        Estream1.Read(InputBytes, 0, 1)
                        OutputString = OutputString & Chr(InputBytes(0))
                    Loop
                    OutputString = MessageQueue(OutputString)
                End If
            Else
                GoTo EndPort1
            End If
        Loop

        OpenPort1.Close()
EndPort1:
        OpenPort1.Dispose()
        OpenPort1 = Nothing
    End Sub

    Private Function MessageQueue(InputString As String) As String
        Dim SplitString() As String
        SplitString = Split(InputString, vbCrLf)
        If SplitString.Length > 1 Then
            For I As Integer = 0 To SplitString.Length - 2
                'SendMessage(SplitString(I))
                If SplitString(I) <> "" Then MessageBuffer.Enqueue(SplitString(I))
            Next
            Return SplitString.Last
        Else
            Return InputString
        End If
    End Function

    'Public Sub OnSceneChanged(Sender As OBSWebsocket, newSceneName As String)

    'Call SceneChanged(newSceneName)
    'RaiseEvent OBSSceneChanged(newSceneName)

    'End Sub

    Private Function IsPort1Open(ByVal Host As String, ByVal PortNumber As Integer) As Boolean

        OpenPort1 = New ToTcpClient

        OpenPort1.BeginConnectWithTimeout(Host, PortNumber, Nothing, 500)
        Thread.Sleep(500)
        If OpenPort1.Connected = True Then
            Try
                Estream1 = OpenPort1.GetStream
                Return True
            Catch
                Return False
            End Try
        Else
            Return False
            OpenPort1.Close()
            OpenPort1.Dispose()
            OpenPort1 = Nothing
        End If

    End Function

    Private Sub SceneChanged(SceneName As String)
        If SceneName <> CurrentSceneName Then
            CurrentSceneName = SceneName
            BeginInvoke(
            Sub()
                DatastreamBox.Text = "OBS Scene Change: " & SceneName & vbCrLf & DatastreamBox.Text
            End Sub)
        End If
    End Sub




End Class

