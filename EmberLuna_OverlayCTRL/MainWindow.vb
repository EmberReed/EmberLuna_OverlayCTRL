
Imports System.Threading
Imports System.Math
Imports System.IO
Imports TwitchLib.PubSub

Public Class MainWindow

    Private CurrentSceneName As String = ""
    Private StrimStarter As StreamStarter
    'Private ChannelPointStarterState As Boolean

    Public ProgramState As Boolean = False
    'Public Event OBSSceneChanged(SceneName As String)

    Private Sub MainWindow_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'ProgramState = True
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

        'AddHandler ChannelPoints.AllRewardsUpdated, AddressOf ChannelPointsStarter
        AddHandler OBS.StreamStateChanged, AddressOf StreamStartHandler
        'AddHandler OBS.Connected, AddressOf OBSisConnected

        AddHandler IRC.IRCconnected, AddressOf IRCisConnected
        AddHandler IRC.IRCdisconnected, AddressOf IRCdisConnected
        AddHandler IRC.IRCeventRecieved, AddressOf IRCevent
        AddHandler IRC.IRCmessageRecieved, AddressOf GenericNotification

        AddHandler MyResourceManager.TaskEvent, AddressOf RMeventHandler

        'AddHandler myAPI.StreamInfoAvailable, AddressOf LaunchStreamStarter
        'AddHandler myAPI.AuthorizationInitialized, AddressOf WaitforAuthorization
        AddHandler myAPI.TokenAcquired, AddressOf GenericNotification

        'AddHandler Ember.Said, AddressOf GenericNotification
        'AddHandler Luna.Said, AddressOf GenericNotification

        AddHandler ChatUserInfo.ChatUserDetected, AddressOf NewUserHandler
        AddHandler ChatUserInfo.MessageRecieved, AddressOf UpdateChatBox
        'AddHandler CounterData.CounterStarted, AddressOf CounterUpdated


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
        ProgramState = False
        CounterData.SaveCounterData()
        OBStimerObject(TimerIDs.Ember).State = False
        OBStimerObject(TimerIDs.Luna).State = False
        OBStimerObject(TimerIDs.GlobalCC).State = False
        If AudioControl.MusicPlayer.Active = True Then AudioControl.StopMusic()

        Await Task.Delay(500)

        If PSubConnected Then Await myAPI.DisconnectAPI()
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
    End Sub

    Private PSubConnected As Boolean = False
    Private IRCconnected As Boolean = False
    Public OBSconnected As Boolean = False
    Private TwitchAuthorized As Boolean = False
    Private APIconnected As Boolean = False
    Private CHpointsReady As Boolean = False

    Public Sub IMREADY()
        If ProgramState Then Invoke(Sub() Enabled = True)
    End Sub

    Public Sub IMNOTREADY()
        If ProgramState Then Invoke(Sub() Enabled = False)
    End Sub

    Private Async Function Startup() As Task
        Me.Enabled = False
        OBSstateChanged(Await CheckOBSconnect())
        IRC.ConnectChat()
        Await myAPI.APIauthorization(True)
        Do Until AmIconnected()
            Await Task.Delay(100)
        Loop
        SyncSceneDisplay()
        AudioControl.MyMixer.SyncAll()
        ProgramState = True
        Enabled = True
    End Function

    Private Function AmIconnected() As Boolean
        If PSubConnected And
            IRCconnected And
            OBSconnected And
            APIconnected And
            TwitchAuthorized And
            CHpointsReady Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function AmIdisconnected() As Boolean
        If PSubConnected = False And
            IRCconnected = False And
            OBSconnected = False And
            TwitchAuthorized = False And
            CHpointsReady = False Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub CHPstateChanged(CHPState As Boolean)
        If CHPState <> CHpointsReady Then
            CHpointsReady = CHPState
            If CHpointsReady = True Then
                CHPdisplay.BackColor = ActiveBUTT
                LogEventString("Channel Points Synced")
            Else
                CHPdisplay.BackColor = StandardBUTT
                LogEventString("Channel Points Disabled")
            End If
        End If
    End Sub

    Public Sub APIstateChanged(APIState As Boolean)
        If APIState <> APIconnected Then
            APIconnected = APIState
            If APIconnected = True Then
                APIdisplay.BackColor = ActiveBUTT
                LogEventString("API Connected")
            Else
                APIdisplay.BackColor = StandardBUTT
                LogEventString("API Disconnected")
            End If
        End If
    End Sub

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



    Public Sub PubSubStateChanged(PubSubState As Boolean)
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

    Private Sub SceneChanged(SceneName As String)
        If SceneName <> CurrentSceneName Then
            CurrentSceneName = SceneName
            LogEventString("OBS Scene Change: " & SceneName)
        End If
    End Sub

    Private Sub StreamStartHandler()
        Dim StateThread As New Thread(
        Sub()
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

    Public Sub LogEventString(Message As String)
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
            Dim InputText As String
            InputText = TextBox3.Text
            IRC.SendChat(InputText)
            TextBox3.Text = ""
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


    Public Sub TwitchAuthStateChanged(TwitchState As Boolean)
        If TwitchAuthorized <> TwitchState Then
            TwitchAuthorized = TwitchState
            If TwitchAuthorized = True Then
                AUTHdisplay.BackColor = ActiveBUTT
                LogEventString("Twitch API Authorized")
            Else
                AUTHdisplay.BackColor = StandardBUTT
                LogEventString("Twitch API Disconnected")
            End If
        End If
    End Sub


    'Private Sub WaitforAuthorization()
    '    TwitchAuthorized = True
    '    AUTHdisplay.BackColor = ActiveBUTT
    '    LogEventString("Authorization Recieved")
    'End Sub



    'Private Sub ChannelPointsStarter()
    '    If ChannelPointStarterState = True Then
    '        ChannelPoints.UpdateProgramRewards(True)
    '        ChannelPointStarterState = False
    '    End If
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

    Private Sub UpdateChatBox(UserName As String, MessageData As String, TimeString As String, ColorIndex As Integer)
        Dim SplitString() As String = TimeString.Split(" ")
        Dim OutputText As String = UserName & "(" & SplitString(1) & "): " & MessageData
        My.Computer.Audio.Play(My.Resources.LOZ_Alert_2, AudioPlayMode.Background)
        ChatBox.ForeColor = ChatUserInfo.ChatColor(ColorIndex)
        ChatBox.Text = OutputText
        ChatHighlighter.Image = My.Resources.gradient
        NextMessage.Visible = True
    End Sub

    Public Sub RemoteUpdateEventBox(EventString As String)
        Invoke(Sub() UpdateEventBox(EventString))
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



    Private Sub AUTHdisplay_Click(sender As Object, e As EventArgs) Handles AUTHdisplay.Click
        If TwitchAuthorized Then
            Dim DisConnectTask As Task = myAPI.DisconnectAPI(True)
        Else
            Dim ConnectTask As Task = myAPI.APIauthorization(True)
        End If
    End Sub
End Class

