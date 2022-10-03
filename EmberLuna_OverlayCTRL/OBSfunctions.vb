Imports OBSWebsocketDotNet
Imports OBSWebsocketDotNet.Types
Imports System.Threading
Imports System.IO

Public Module OBSfunctions

    Public WithEvents OBS As OBSWebsocket
    Private OBSmutex As Mutex
    Private WaitForConnect As Boolean
    Private OBSstarted As TaskCompletionSource(Of Boolean)
    Private WaitForDisconnect As Boolean
    Private OBSended As TaskCompletionSource(Of Boolean)
    Private MyVolume As VolumeInfo


    Public Sub InitializeOBS_WS()
        OBS = New OBSWebsocket
        OBSmutex = New Mutex
    End Sub

    'Public UpTime As Integer
    'Public StopCount As Boolean
    'Public Async Function CountUpTime() As Task
    '    UpTime = 0
    '    StopCount = False
    '    Await Task.Delay(1000)
    '    Do Until StopCount = True
    '        UpTime = UpTime + 1
    '        Await Task.Delay(1000)
    '    Loop
    '    StopCount = False
    '    SendMessage(UpTime)
    'End Function

    Public Function OBSstreamState() As Boolean
        OBSmutex.WaitOne()
        Dim MyStatus As OutputStatus = OBS.GetStreamStatus
        OBSmutex.ReleaseMutex()
        Return MyStatus.IsActive
    End Function

    Public Sub SetOBSsourceText(SourceName As String, TextData As String, Optional BypassMutex As Boolean = False)
        If BypassMutex = False Then OBSmutex.WaitOne()
        Dim InputSettings As InputSettings = OBS.GetInputSettings(SourceName)
        'SourceSettings.
        'Dim SourceSettings As Newtonsoft.Json.Linq.JObject = InputSettings.Settings
        InputSettings.Settings.Property("text").Value = TextData
        'InputSettings.Settings = SourceSettings
        OBS.SetInputSettings(InputSettings)
        If BypassMutex = False Then OBSmutex.ReleaseMutex()
    End Sub

    Public Sub SetOBSscene(SceneString As String)
        OBSmutex.WaitOne()
        If OBS.IsConnected = True Then
            OBS.SetCurrentProgramScene(SceneString)
        End If
        OBSmutex.ReleaseMutex()
    End Sub

    Private Sub OBSdisconnected(sender As Object, e As Communication.ObsDisconnectionInfo) Handles OBS.Disconnected
        'StopCount = True
        'SendMessage(e.DisconnectReason)
        'If WaitForDisconnect Then OBSended.SetResult(False)
        OBSmutex.WaitOne()
        Dim StateBool As Boolean = OBS.IsConnected
        OBSmutex.ReleaseMutex()
        If StateBool = False Then
            SourceWindow.OBSstateChanged(False)
        End If
    End Sub

    Public Function DisconnectOBS() As Boolean
        OBSmutex.WaitOne()
        Dim ReturnBool = OBS.IsConnected
        If ReturnBool Then
            'OBSended = New TaskCompletionSource(Of Boolean)
            'WaitForDisconnect = True
            OBS.Disconnect()
            'SendMessage("Disconnecting OBS")
            'WaitForDisconnect = Await OBSended.Task
        End If
        OBSmutex.ReleaseMutex()
        Return ReturnBool
    End Function
    Private Sub OBSconnected(sender As Object, e As EventArgs) Handles OBS.Connected
        If WaitForConnect Then OBSstarted.SetResult(False)
        'Dim CountTask As Task = CountUpTime()
    End Sub

    Public Async Function CheckOBSconnect() As Task(Of Boolean)
        OBSmutex.WaitOne()
        If OBS.IsConnected = False Then
            Try
                OBSstarted = New TaskCompletionSource(Of Boolean)
                WaitForConnect = True
                OBS.ConnectAsync(OBSsocketString, OBSsocketPassword)
                WaitForConnect = Await OBSstarted.Task
                CurrentScene = OBS.GetCurrentProgramScene
                OBSmutex.ReleaseMutex()
                Return True
            Catch ex As Exception
                OBSmutex.ReleaseMutex()
                Return False
            End Try
        Else
            OBSmutex.ReleaseMutex()
            Return True
        End If
    End Function

    Public Structure MediaFcode
        Public Const Play As Integer = 1
        Public Const Pause As Integer = 2
        Public Const Stopp As Integer = 3
        Public Const Restart As Integer = 4
    End Structure
    Public Structure MediaActions
        Public Const Restart As String = "OBS_WEBSOCKET_MEDIA_INPUT_ACTION_RESTART"
        Public Const Play As String = "OBS_WEBSOCKET_MEDIA_INPUT_ACTION_PLAY"
        Public Const Pause As String = "OBS_WEBSOCKET_MEDIA_INPUT_ACTION_PAUSE"
        Public Const Stopp As String = "OBS_WEBSOCKET_MEDIA_INPUT_ACTION_STOP"
        Public Const Playing As String = "OBS_MEDIA_STATE_PLAYING"
        Public Const Paused As String = "OBS_MEDIA_STATE_PAUSED"
        Public Const Stopped As String = "OBS_MEDIA_STATE_STOPPED"
        Public Const Ended As String = "OBS_MEDIA_STATE_ENDED"
        Public Const Openning As String = "OBS_MEDIA_STATE_OPENNING"
        Public Const Bufferring As String = "OBS_MEDIA_STATE_BUFFERRING"
        Public Const Errorr As String = "OBS_MEDIA_STATE_ERROR"
        Public Const None As String = "OBS_MEDIA_STATE_NONE"
    End Structure

    Public Structure MonitorType
        Public Const None As String = "OBS_MONITORING_TYPE_NONE"
        Public Const Enabled As String = "OBS_MONITORING_TYPE_MONITOR_AND_OUTPUT"
        Public Const Only As String = "OBS_MONITORING_TYPE_MONITOR_ONLY"
    End Structure

    Public Sub ResetMediaSource(SourceName As String)
        Dim ResetThread As Thread = New Thread(
            Sub()
                OBSmutex.WaitOne()
                Dim MySettings As InputSettings = OBS.GetInputSettings(SourceName)
                MySettings.Settings.Property("local_file").Value = ""
                OBS.SetInputSettings(MySettings)
                OBSmutex.ReleaseMutex()
            End Sub)
        ResetThread.Start()
    End Sub



    Public Function MediaSourceChange(SourceName As String, Optional ActionType As Integer = 0, Optional FileName As String = "") As String
        OBSmutex.WaitOne()
        Dim MySettings As InputSettings = OBS.GetInputSettings(SourceName)
        Dim OutputFile As String = MySettings.Settings.Property("local_file").Value

        If FileName <> "" Then
            MySettings.Settings.Property("local_file").Value = FileName
            OBS.SetInputSettings(MySettings)
            OBS.TriggerMediaInputAction(SourceName, MediaActions.Restart)
            OutputFile = FileName
        Else
            'If InStr(SourceName, "Roll") > 0 Then SendMessage(SourceName & " " & ActionType)
            Select Case ActionType
                Case = MediaFcode.Play
                    If OBS.GetMediaInputStatus(SourceName).State = MediaActions.Paused Then
                        OBS.TriggerMediaInputAction(SourceName, MediaActions.Play)
                    Else
                        OBS.TriggerMediaInputAction(SourceName, MediaActions.Restart)
                    End If
                Case = MediaFcode.Pause
                    OBS.TriggerMediaInputAction(SourceName, MediaActions.Pause)
                Case = MediaFcode.Stopp
                    OBS.TriggerMediaInputAction(SourceName, MediaActions.Stopp)
                Case = MediaFcode.Restart
                    OBS.TriggerMediaInputAction(SourceName, MediaActions.Restart)
            End Select
        End If

        'Dim Settings As MediaSourceSettings = OBS.GetMediaSourceSettings(SourceName)
        'Dim OutputFile As String = Settings.Media.LocalFile
        'If FileName <> "" Then
        '    Settings.Media.LocalFile = FileName
        '    OBS.SetMediaSourceSettings(Settings)
        '    OutputFile = Settings.Media.LocalFile
        'Else
        '    Select Case ActionType
        '        Case = MediaFcode.Play
        '            OBS.PlayPauseMedia(SourceName, False)
        '        Case = MediaFcode.Pause
        '            OBS.PlayPauseMedia(SourceName, True)
        '        Case = MediaFcode.Stopp
        '            OBS.StopMedia(SourceName)
        '        Case = MediaFcode.Restart
        '            OBS.RestartMedia(SourceName)
        '    End Select
        'End If
        OBSmutex.ReleaseMutex()
        Return OutputFile
    End Function

    '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    '///////////////////////////////////////////////////SCENE FUNCTIONS///////////////////////////////////////////////////////
    '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    Public CurrentScene As String

    'AWAY MODE
    Public EmberAwayB As Boolean = False
    Public LunaAwayB As Boolean = False
    Public Screen1AwayMode As Boolean = False
    Public Screen2AwayMode As Boolean = False

    'SCREEN SETTINGS
    Public Screen2Enabled As Boolean = False
    Public PIPenabled As Boolean = False
    Public Screen1Setting As Integer = ScreenSetting.EmberPC
    Public Screen2Setting As Integer = ScreenSetting.LunaPC
    Public Cam1Setting As Integer = ScreenSetting.EmberCam
    Public Cam2Setting As Integer = ScreenSetting.LunaCam

    'PERIPHERAL SETTINGS
    Public EmberCamera As Boolean = True
    Public LunaCamera As Boolean = True
    Public EmberSpriteB As Boolean = True
    Public LunaSpriteB As Boolean = True


    'HANDLERS
    Public SceneChangeInProgress As Boolean = False
    Public Event SceneChangeInitiated()
    Public Event SceneChangeCompleted()
    Public SceneChangeNeeded As Boolean
    Public SceneHasChanged As TaskCompletionSource(Of Boolean)

    Public Structure SceneEnums
        Public Const E_cam_Away As Integer = 0
        Public Const E_scr_Away As Integer = 1
        Public Const L_cam_Away As Integer = 2
        Public Const L_scr_Away As Integer = 3

        Public Const E_Sprite As Integer = 0
        Public Const E_Camera As Integer = 1
        Public Const L_Sprite As Integer = 2
        Public Const L_Camera As Integer = 3

        Public Const Screen1 As Integer = 0
        Public Const Screen2 As Integer = 1
        Public Const Camera1 As Integer = 2
        Public Const Camera2 As Integer = 3
    End Structure

    Public Class SceneCollection
        Public Const SceneDirectory As String = "\\StreamPC-V2\OBS Assets\Scenes"
        Public SceneCollection() As SceneFile
        Public Event ScenesUpdated(NameList() As String)

        Public Sub New()
            RefreshSceneCollection()
        End Sub

        Public Sub RefreshSceneCollection()
            Dim SceneList() As String = Nothing
            If Directory.Exists(SceneDirectory) Then
                Dim di As New IO.DirectoryInfo(SceneDirectory)
                Dim aryFi As IO.FileInfo() = di.GetFiles("*.txt")
                If aryFi.Length <> 0 Then
                    ReDim SceneCollection(0 To aryFi.Length - 1)
                    ReDim SceneList(0 To aryFi.Length - 1)
                    For i As Integer = 0 To aryFi.Length - 1
                        SceneCollection(i) = New SceneFile(aryFi(i).FullName)
                        SceneList(i) = SceneCollection(i).Fname
                    Next
                Else
                    SceneCollection = Nothing
                End If
            End If
            RaiseEvent ScenesUpdated(SceneList)
        End Sub

        Public Function GetSceneList() As String()
            If SceneCollection IsNot Nothing Then
                Dim SceneNames(0 To SceneCollection.Length - 1) As String
                For I As Integer = 0 To SceneCollection.Length - 1
                    SceneNames(I) = SceneCollection(I).Fname
                Next
                Return SceneNames
            Else
                Return {}
            End If
        End Function

        Public Function CheckName(InputName) As Boolean
            If SceneCollection IsNot Nothing Then
                For i As Integer = 0 To SceneCollection.Length - 1
                    If SceneCollection(i).Fname = InputName Then
                        Return False
                        Exit Function
                    End If
                Next
            End If
            Return True
        End Function

        Public Function IndexByName(SceneName As String)
            If SceneCollection IsNot Nothing Then
                For i As Integer = 0 To SceneCollection.Length - 1
                    If SceneCollection(i).Fname = SceneName Then
                        Return i
                        Exit Function
                    End If
                Next
            End If
            Return -1
        End Function

        Public Sub DeleteScene(SceneIndex As Integer)
            SceneCollection(SceneIndex).DeleteSceneFile(SceneDirectory)
            RefreshSceneCollection()
        End Sub

        Public Sub UpdateScene(SceneIndex As Integer)
            Dim NewScene As New SceneFile(, SceneCollection(SceneIndex).Fname,
                                            {EmberAwayB, LunaAwayB, Screen1AwayMode, Screen2AwayMode},
                                            {EmberSpriteB, EmberCamera, LunaSpriteB, LunaCamera},
                                            {Screen1Setting, Screen2Setting, Cam1Setting, Cam2Setting},
                                            Screen2Enabled, CurrentScene, PIPenabled)

            NewScene.WriteSceneFile(SceneDirectory)
            SceneCollection(SceneIndex) = NewScene
        End Sub

        Public Sub RenameScene(SceneIndex As Integer, NewName As String)
            If CheckName(NewName) = True Then
                SceneCollection(SceneIndex).Fname = NewName
                SceneCollection(SceneIndex).WriteSceneFile(SceneDirectory)
                RefreshSceneCollection()
            Else
                SendMessage("Cannot create duplicate scene name", "NOPE")
            End If
        End Sub

        Public Sub AddScene(SceneName As String)
            If CheckName(SceneName) = True Then
                Dim NewScene As New SceneFile(, SceneName,
                                            {EmberAwayB, LunaAwayB, Screen1AwayMode, Screen2AwayMode},
                                            {EmberSpriteB, EmberCamera, LunaSpriteB, LunaCamera},
                                            {Screen1Setting, Screen2Setting, Cam1Setting, Cam2Setting},
                                            Screen2Enabled, CurrentScene, PIPenabled)
                NewScene.WriteSceneFile(SceneDirectory)
                RefreshSceneCollection()
            Else
                SendMessage("Cannot create duplicate scene name", "NOPE")
            End If
        End Sub

    End Class
    Public Class SceneFile

        Public Fname As String
        Public Sname As String
        Public FullName As String
        Public Away() As Boolean
        Public Features() As Boolean
        Public Outputs() As Integer
        Public Screen2en As Boolean
        Public PIPen As Boolean
        Public Sub New(Optional SourceFile As String = "",
                       Optional SceneName As String = "",
                       Optional Aways() As Boolean = Nothing,
                       Optional Feats() As Boolean = Nothing,
                       Optional Settings() As Integer = Nothing,
                       Optional Screen2 As Boolean = False,
                       Optional ScName As String = "",
                       Optional PIP As Boolean = False)
            If SourceFile <> "" Then
                ReadSceneFile(SourceFile)
            Else
                Fname = SceneName
                If Aways IsNot Nothing Then
                    Away = Aways
                Else
                    Away = {False, False, False, False}
                End If
                If Feats IsNot Nothing Then
                    Features = Feats
                Else
                    Features = {True, True, True, True}
                End If
                If Settings IsNot Nothing Then
                    Outputs = Settings
                Else
                    Outputs = {ScreenSetting.EmberPC, ScreenSetting.LunaPC, ScreenSetting.EmberCam, ScreenSetting.LunaCam}
                End If
                Screen2en = Screen2
                PIPen = PIP
                Sname = ScName
            End If

        End Sub

        Public Async Function ApplySceneSettings() As Task
            EmberAwayB = Away(SceneEnums.E_cam_Away)
            LunaAwayB = Away(SceneEnums.L_cam_Away)
            Screen1AwayMode = Away(SceneEnums.E_scr_Away)
            Screen2AwayMode = Away(SceneEnums.L_scr_Away)
            EmberCamera = Features(SceneEnums.E_Camera)
            LunaCamera = Features(SceneEnums.L_Camera)
            EmberSpriteB = Features(SceneEnums.E_Sprite)
            LunaSpriteB = Features(SceneEnums.L_Sprite)
            Screen1Setting = Outputs(SceneEnums.Screen1)
            Screen2Setting = Outputs(SceneEnums.Screen2)
            Cam1Setting = Outputs(SceneEnums.Camera1)
            Cam2Setting = Outputs(SceneEnums.Camera2)
            Screen2Enabled = Screen2en
            PIPenabled = PIPen
            Await UpdateSceneDisplay()
            If CurrentScene <> Sname Then
                Await Task.Delay(350)
                Await UpdateSceneDisplay(Sname)
                'Await Task.Delay(1000)
            End If
        End Function

        Public Sub ReadSceneFile(Optional SceneFileName As String = "")
            If SceneFileName = "" Then
                If FullName <> "" Then
                    SceneFileName = FullName
                End If
            End If
            If SceneFileName <> "" Then
                If File.Exists(SceneFileName) = True Then
                    Fname = Replace(Split(SceneFileName, "\").Last, ".txt", "")
                    Dim Reader As StreamReader = File.OpenText(SceneFileName)
                    Do Until Reader.EndOfStream = True
                        ReadSceneFileLine(Reader.ReadLine())
                    Loop
                    Reader.Close()
                    Reader.Dispose()
                End If
            End If
        End Sub

        Public Sub ReadSceneFileLine(InputLine As String)
            Dim SplitString As String() = Split(InputLine, "<>")
            Select Case SplitString(0)
                Case = "Away"
                    Away = Array.ConvertAll(Split(SplitString(1), ","), AddressOf ConvertStringToBool)
                Case = "Features"
                    Features = Array.ConvertAll(Split(SplitString(1), ","), AddressOf ConvertStringToBool)
                Case = "Outputs"
                    Outputs = Array.ConvertAll(Split(SplitString(1), ","), AddressOf ConvertStringToInteger)
                Case = "Screen2en"
                    Screen2en = SplitString(1)
                Case = "PIPen"
                    PIPen = SplitString(1)
                Case = "Sname"
                    Sname = SplitString(1)
            End Select
        End Sub

        Public Sub WriteSceneFile(SceneFileDirectory As String)
            FullName = SceneFileDirectory & "\" & Fname & ".txt"
            File.Create(FullName).Dispose()
            If File.Exists(FullName) = True Then
                Dim Writer As StreamWriter = File.AppendText(FullName)

                Writer.WriteLine("Away<>" & String.Join(",", Away))
                Writer.WriteLine("Features<>" & String.Join(",", Features))
                Writer.WriteLine("Outputs<>" & String.Join(",", Outputs))
                Writer.WriteLine("Screen2en<>" & Screen2en)
                Writer.WriteLine("PIPen<>" & PIPen)
                Writer.WriteLine("Sname<>" & Sname)

                Writer.Close()
                Writer.Dispose()
            End If
        End Sub

        Public Sub DeleteSceneFile(SceneFileDirectory As String)
            FullName = SceneFileDirectory & "\" & Fname & ".txt"
            File.Delete(FullName)
        End Sub
    End Class

    Public Structure ScreenSetting
        Public Const EmberPC As Integer = 0
        Public Const LunaPC As Integer = 1
        Public Const EmberCam As Integer = 2
        Public Const LunaCam As Integer = 3
        Public Const Aux1 As Integer = 4
        Public Const Aux2 As Integer = 5
        Public Const Aux3 As Integer = 6
        Public Const Aux4 As Integer = 7
        Public Const StreamPC1 As Integer = 8
        Public Const StreamPC2 As Integer = 9
    End Structure

    Public Structure OBSinput
        Public ObjectName As String
        Public DisplayName As String
    End Structure

    Public Sub SceneChangeDetected() Handles OBS.SceneTransitionEnded
        If SceneChangeInProgress = True Then
            SceneChangeInProgress = False
            If SceneChangeNeeded = True Then SceneHasChanged.SetResult(False)
        End If
    End Sub

    Public Sub SetSourceVisibility(SourceID As Integer, SceneName As String, Visibool As Boolean,
                                   Optional UseMutex As Boolean = False, Optional SourceName As String = "")
        If UseMutex = True Then OBSmutex.WaitOne()
        If SourceName <> "" Then SourceID = OBS.GetSceneItemId(SceneName, SourceName, SourceID)
        If OBS.GetSceneItemEnabled(SceneName, SourceID) <> Visibool Then
            OBS.SetSceneItemEnabled(SceneName, SourceID, Visibool)
        End If
        If UseMutex = True Then OBSmutex.ReleaseMutex()
    End Sub

    Public Function SyncScreenSettings(ActiveScene As SceneBasicInfo, AwayName As String, Optional Screen2 As Boolean = False) As Integer
        Dim Output As Integer = 0
        For Each SceneItem In OBS.GetSceneItemList(ActiveScene.Name)
            Select Case SceneItem.SourceName
                Case = "Ember's PC"
                    If OBS.GetSceneItemEnabled(ActiveScene.Name, SceneItem.ItemId) Then Output = ScreenSetting.EmberPC
                Case = "Luna's PC"
                    If OBS.GetSceneItemEnabled(ActiveScene.Name, SceneItem.ItemId) Then Output = ScreenSetting.LunaPC
                Case = "J-Cam 1"
                    If OBS.GetSceneItemEnabled(ActiveScene.Name, SceneItem.ItemId) Then Output = ScreenSetting.EmberCam
                Case = "E-Cam 1"
                    If OBS.GetSceneItemEnabled(ActiveScene.Name, SceneItem.ItemId) Then Output = ScreenSetting.LunaCam
                Case = "Aux In 1"
                    If OBS.GetSceneItemEnabled(ActiveScene.Name, SceneItem.ItemId) Then Output = ScreenSetting.Aux1
                Case = "Aux In 2"
                    If OBS.GetSceneItemEnabled(ActiveScene.Name, SceneItem.ItemId) Then Output = ScreenSetting.Aux2
                Case = "Aux-Cam 1"
                    If OBS.GetSceneItemEnabled(ActiveScene.Name, SceneItem.ItemId) Then Output = ScreenSetting.Aux3
                Case = "Aux-Cam 2"
                    If OBS.GetSceneItemEnabled(ActiveScene.Name, SceneItem.ItemId) Then Output = ScreenSetting.Aux4
                Case = "StreamPC Screen1"
                    If OBS.GetSceneItemEnabled(ActiveScene.Name, SceneItem.ItemId) Then Output = ScreenSetting.StreamPC1
                Case = "StreamPC Screen2"
                    If OBS.GetSceneItemEnabled(ActiveScene.Name, SceneItem.ItemId) Then Output = ScreenSetting.StreamPC2
                Case = AwayName
                    Select Case AwayName
                        Case "Ember Is Away"
                            EmberAwayB = OBS.GetSceneItemEnabled(ActiveScene.Name, SceneItem.ItemId)
                        Case "Luna Is Away"
                            LunaAwayB = OBS.GetSceneItemEnabled(ActiveScene.Name, SceneItem.ItemId)
                        Case "Jerin Montage"
                            Screen1AwayMode = OBS.GetSceneItemEnabled(ActiveScene.Name, SceneItem.ItemId)
                        Case "Jerin Montage S2"
                            Screen2AwayMode = OBS.GetSceneItemEnabled(ActiveScene.Name, SceneItem.ItemId)
                    End Select
            End Select
        Next
        Return Output
    End Function
    Public Sub SetScreenSettings(ActiveScene As SceneBasicInfo, Setting As Integer, Awaymode As Boolean, AwayName As String)
        For Each SceneItem In OBS.GetSceneItemList(ActiveScene.Name)
            Select Case SceneItem.SourceName
                Case = "Ember's PC"
                    SetSourceVisibility(SceneItem.ItemId, ActiveScene.Name, Setting = ScreenSetting.EmberPC)
                Case = "Luna's PC"
                    SetSourceVisibility(SceneItem.ItemId, ActiveScene.Name, Setting = ScreenSetting.LunaPC)
                Case = "J-Cam 1"
                    SetSourceVisibility(SceneItem.ItemId, ActiveScene.Name, Setting = ScreenSetting.EmberCam)
                Case = "E-Cam 1"
                    SetSourceVisibility(SceneItem.ItemId, ActiveScene.Name, Setting = ScreenSetting.LunaCam)
                Case = "Aux In 1"
                    SetSourceVisibility(SceneItem.ItemId, ActiveScene.Name, Setting = ScreenSetting.Aux1)
                Case = "Aux In 2"
                    SetSourceVisibility(SceneItem.ItemId, ActiveScene.Name, Setting = ScreenSetting.Aux2)
                Case = "Aux-Cam 1"
                    SetSourceVisibility(SceneItem.ItemId, ActiveScene.Name, Setting = ScreenSetting.Aux3)
                Case = "Aux-Cam 2"
                    SetSourceVisibility(SceneItem.ItemId, ActiveScene.Name, Setting = ScreenSetting.Aux4)
                Case = "StreamPC Screen1"
                    SetSourceVisibility(SceneItem.ItemId, ActiveScene.Name, Setting = ScreenSetting.StreamPC1)
                Case = "StreamPC Screen2"
                    SetSourceVisibility(SceneItem.ItemId, ActiveScene.Name, Setting = ScreenSetting.StreamPC2)
                Case = AwayName
                    SetSourceVisibility(SceneItem.ItemId, ActiveScene.Name, Awaymode)
            End Select
        Next
    End Sub

    Public Sub SyncSceneDisplay()

        SceneChangeInProgress = True

        Dim ItemsList As List(Of SceneItemDetails)
        Dim CurrentSceneList As GetSceneListInfo

        OBSmutex.WaitOne()
        CurrentScene = OBS.GetCurrentProgramScene
        Dim SceneName As String = CurrentScene

        PIPenabled = False

        CurrentSceneList = OBS.GetSceneList
        For Each ActiveScene In CurrentSceneList.Scenes
            Select Case ActiveScene.Name
                Case "Cam 1"
                    Cam1Setting = SyncScreenSettings(ActiveScene, "Ember Is Away")
                Case "Screen 1"
                    Screen1Setting = SyncScreenSettings(ActiveScene, "Jerin Montage")
                Case "Cam 2"
                    Cam2Setting = SyncScreenSettings(ActiveScene, "Luna Is Away")
                Case "Screen 2"
                    Screen2Setting = SyncScreenSettings(ActiveScene, "Jerin Montage", True)
                Case "EmberPIP"

                    ItemsList = OBS.GetSceneItemList(ActiveScene.Name)
                    For Each SceneItem In ItemsList
                        If SceneItem.SourceName = "Ember" Then
                            EmberSpriteB = OBS.GetSceneItemEnabled(ActiveScene.Name, SceneItem.ItemId)
                        End If
                    Next
                Case "LunaPIP"
                    ItemsList = OBS.GetSceneItemList(ActiveScene.Name)
                    For Each SceneItem In ItemsList
                        If SceneItem.SourceName = "Luna" Then
                            LunaSpriteB = OBS.GetSceneItemEnabled(ActiveScene.Name, SceneItem.ItemId)
                        End If
                    Next
                Case "Single Screen Mode"
                    ItemsList = OBS.GetSceneItemList(ActiveScene.Name)
                    For Each SceneItem In ItemsList
                        Select Case SceneItem.SourceName
                            Case = "Screen 2"
                                Screen2Enabled = OBS.GetSceneItemEnabled(ActiveScene.Name, SceneItem.ItemId)
                            Case = "Ember Cam"
                                EmberCamera = OBS.GetSceneItemEnabled(ActiveScene.Name, SceneItem.ItemId)
                            Case = "Luna Cam"
                                LunaCamera = OBS.GetSceneItemEnabled(ActiveScene.Name, SceneItem.ItemId)
                            Case "EmberPIP"
                                If OBS.GetSceneItemEnabled(ActiveScene.Name, SceneItem.ItemId) Then PIPenabled = True
                            Case "LunaPIP"
                                If OBS.GetSceneItemEnabled(ActiveScene.Name, SceneItem.ItemId) Then PIPenabled = True
                        End Select
                    Next
            End Select
        Next
        OBSmutex.ReleaseMutex()

    End Sub

    Public Async Function UpdateSceneDisplay(Optional ChangeSceneString As String = "") As Task

        SceneChangeInProgress = True

        Dim ItemsList As List(Of SceneItemDetails)
        Dim CurrentSceneList As OBSWebsocketDotNet.Types.GetSceneListInfo

        OBSmutex.WaitOne()
        CurrentScene = OBS.GetCurrentProgramScene
        Dim SceneName As String = CurrentScene

        CurrentSceneList = OBS.GetSceneList()
        For Each ActiveScene In CurrentSceneList.Scenes
            Select Case ActiveScene.Name
                Case "Cam 1"
                    SetScreenSettings(ActiveScene, Cam1Setting, EmberAwayB, "Ember Is Away")
                Case "Screen 1"
                    SetScreenSettings(ActiveScene, Screen1Setting, Screen1AwayMode, "Jerin Montage")
                Case "Cam 2"
                    SetScreenSettings(ActiveScene, Cam2Setting, LunaAwayB, "Luna Is Away")
                Case "Screen 2"
                    SetScreenSettings(ActiveScene, Screen2Setting, Screen2AwayMode, "Jerin Montage")
                Case "Events and Alerts"
                    ItemsList = OBS.GetSceneItemList(ActiveScene.Name)
                    For Each SceneItem In ItemsList
                        Select Case SceneItem.SourceName
                            Case = "SoundAlert"
                                SetSourceVisibility(SceneItem.ItemId, ActiveScene.Name, SoundAlertDisplay)
                        End Select
                    Next
                Case "EmberPIP"
                    ItemsList = OBS.GetSceneItemList(ActiveScene.Name)
                    For Each SceneItem In ItemsList
                        If SceneItem.SourceName = "Ember" Then
                            SetSourceVisibility(SceneItem.ItemId, ActiveScene.Name, EmberSpriteB)
                        End If
                    Next
                Case "LunaPIP"
                    ItemsList = OBS.GetSceneItemList(ActiveScene.Name)
                    For Each SceneItem In ItemsList
                        If SceneItem.SourceName = "Luna" Then
                            SetSourceVisibility(SceneItem.ItemId, ActiveScene.Name, LunaSpriteB)
                        End If
                    Next
                Case "Counters&Timers"
                    ItemsList = OBS.GetSceneItemList(ActiveScene.Name)
                    For Each SceneItem In ItemsList
                        Select Case SceneItem.SourceName
                            Case = "EmberTimer"
                                SetSourceVisibility(SceneItem.ItemId, ActiveScene.Name,
                                                    OBStimerObject(TimerIDs.Ember).State)
                            Case = "EmberCounter"
                                SetSourceVisibility(SceneItem.ItemId, ActiveScene.Name,
                                                    OBScounterObject(CounterIDs.Ember).State)
                            Case = "LunaTimer"
                                SetSourceVisibility(SceneItem.ItemId, ActiveScene.Name,
                                                    OBStimerObject(TimerIDs.Luna).State)
                            Case = "LunaCounter"
                                SetSourceVisibility(SceneItem.ItemId, ActiveScene.Name,
                                                    OBScounterObject(CounterIDs.Luna).State)
                            Case = "GlobalCounter"
                                SetSourceVisibility(SceneItem.ItemId, ActiveScene.Name,
                                                    OBScounterObject(CounterIDs.Glob).State)
                        End Select
                    Next
                Case "Dual Screen Mode", "Center Screen Mode", "Single Screen Mode", "Split Screen Mode"
                    ItemsList = OBS.GetSceneItemList(ActiveScene.Name)
                    For Each SceneItem In ItemsList
                        Select Case SceneItem.SourceName
                            Case = "GlobalTimer"
                                SetSourceVisibility(SceneItem.ItemId, ActiveScene.Name,
                                                    OBStimerObject(TimerIDs.GlobalCC).State)
                            Case = "Screen 1"
                                If ActiveScene.Name = "Center Screen Mode" Or ActiveScene.Name = "Single Screen Mode" Then
                                    SetSourceVisibility(SceneItem.ItemId, ActiveScene.Name, Screen2Enabled = False)
                                End If
                            Case = "Screen 2"
                                If ActiveScene.Name = "Center Screen Mode" Or ActiveScene.Name = "Single Screen Mode" Then
                                    SetSourceVisibility(SceneItem.ItemId, ActiveScene.Name,
                                                    Screen2Enabled)
                                End If
                            Case = "Ember Cam"
                                If ActiveScene.Name = "Split Screen Mode" Or ActiveScene.Name = "Single Screen Mode" Then
                                    SetSourceVisibility(SceneItem.ItemId, ActiveScene.Name, EmberCamera)
                                Else
                                    SetSourceVisibility(SceneItem.ItemId, ActiveScene.Name, True)
                                End If
                            Case = "Luna Cam"
                                If ActiveScene.Name = "Split Screen Mode" Or ActiveScene.Name = "Single Screen Mode" Then
                                    SetSourceVisibility(SceneItem.ItemId, ActiveScene.Name, LunaCamera)
                                Else
                                    SetSourceVisibility(SceneItem.ItemId, ActiveScene.Name, True)
                                End If
                            Case = "Ember"
                                Select Case ActiveScene.Name
                                    Case "Single Screen Mode"
                                        SetSourceVisibility(SceneItem.ItemId, ActiveScene.Name,
                                                            (EmberSpriteB = False Or (PIPenabled And Screen2Enabled)) = False)
                                    Case "Split Screen Mode"
                                        SetSourceVisibility(SceneItem.ItemId, ActiveScene.Name, EmberSpriteB)
                                    Case Else
                                        SetSourceVisibility(SceneItem.ItemId, ActiveScene.Name, True)
                                End Select
                            Case = "Luna"
                                Select Case ActiveScene.Name
                                    Case "Single Screen Mode"
                                        SetSourceVisibility(SceneItem.ItemId, ActiveScene.Name,
                                                            (LunaSpriteB = False Or (PIPenabled And Screen2Enabled = False)) = False)
                                    Case "Split Screen Mode"
                                        SetSourceVisibility(SceneItem.ItemId, ActiveScene.Name, LunaSpriteB)
                                    Case Else
                                        SetSourceVisibility(SceneItem.ItemId, ActiveScene.Name, True)
                                End Select
                            Case "EmberPIP"
                                SetSourceVisibility(SceneItem.ItemId, ActiveScene.Name,
                                                    PIPenabled And Screen2Enabled)
                            Case "LunaPIP"
                                SetSourceVisibility(SceneItem.ItemId, ActiveScene.Name,
                                                    PIPenabled And Screen2Enabled = False)
                        End Select
                    Next
            End Select
        Next

        If ChangeSceneString <> "" And CurrentScene <> ChangeSceneString Then
            RaiseEvent SceneChangeInitiated()
            SceneHasChanged = New TaskCompletionSource(Of Boolean)
            SceneChangeNeeded = True
            OBS.SetCurrentProgramScene(ChangeSceneString)
            SceneChangeNeeded = Await SceneHasChanged.Task
            CurrentScene = OBS.GetCurrentProgramScene
            RaiseEvent SceneChangeCompleted()
        Else
            SceneChangeInProgress = False
        End If

        OBSmutex.ReleaseMutex()

    End Function


    '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    '///////////////////////////////////////////////////SPRITE CONTROLS///////////////////////////////////////////////////////
    '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    Public Sub WelcomeNewUser(UserName As String, NewUser As Boolean)
        If UserName <> Broadcastername Then
            'If CheckOBSconnect() = True Then
            Dim MessageType As String
            If NewUser = True Then
                MessageType = "Greetings"
            Else
                MessageType = "Returns"
            End If

            Select Case EmberOrLuna()
                Case WhichSprite.Ember
                    Dim Speak As Task = Ember.Says(RandomMessage(UserName, MessageType), Ember.Mood.Happy, "Hello")
                Case WhichSprite.Luna
                    Dim Speak As Task = Luna.Says(RandomMessage(UserName, MessageType), Luna.Mood.Cringe, "Hello")
                Case WhichSprite.Neither
                    IRC.SendChat(RandomMessage(UserName, MessageType))
                    AudioControl.SoundPlayer.PlaySound(AudioControl.GetSoundFileDataByName("Hello"), SoundSource.SFX)
            End Select
        End If
    End Sub

    Public Structure WhichSprite
        Public Const Neither = 0
        Public Const Ember = 1
        Public Const Luna = 2
    End Structure

    Public Function EmberOrLuna(Optional PreferAvailable As Boolean = True) As Integer
        Dim EmberAvailable As Boolean = MyResourceManager.OKtoGO({ResourceIDs.EmberSprite})
        Dim LunaAvailable As Boolean = MyResourceManager.OKtoGO({ResourceIDs.EmberSprite})

        If CurrentScene = "Center Screen Mode" Or
            CurrentScene = "Dual Screen Mode" Or
            (EmberSpriteB = True And LunaSpriteB = True) Then
            If PreferAvailable = False Or
                (EmberAvailable And LunaAvailable) Or
                (EmberAvailable = False And LunaAvailable = False) Then
                Return RandomInt(WhichSprite.Ember, WhichSprite.Luna)
            Else
                If EmberAvailable Then Return WhichSprite.Ember
                If LunaAvailable Then Return WhichSprite.Luna
            End If
        Else
            If EmberSpriteB = True Then Return WhichSprite.Ember
            If LunaSpriteB = True Then Return WhichSprite.Luna
        End If
        Return WhichSprite.Neither
    End Function


    'Ember animations should be *0.6 for timing purposes
    'Luna anumations should be *0.95 for timing puroses

    Public Structure SpriteID
        Public Const Ember As Boolean = True
        Public Const Luna As Boolean = False
    End Structure

    Public Structure CharacterMoods
        Public Neutral As String
        Public Happy As String
        Public Sadge As String
        Public Angy As String
        Public Wumpy As String
        Public Cringe As String
        Public Wow As String
        Public Sparkle As String
        Public WTF As String
        Public OMG As String
        Public RockandStone As String
        Public Wibble As String
        Public DeepBreath As String
        Public WooHoo As String
        Public PooBrain As String

        Public Sub InitializeMoods(CharacterSelect As Boolean, FileHeader As String)
            If CharacterSelect = SpriteID.Ember Then
                Neutral = FileHeader & "blink.gif"
                Happy = FileHeader & "happy.gif"
                Sadge = FileHeader & "sadge.gif"
                Angy = FileHeader & "angery.gif"
                Wumpy = FileHeader & "wumpy.gif"
                Cringe = ""
                Wow = ""
                Sparkle = ""
                WTF = FileHeader & "wtf.gif"
                OMG = ""
                RockandStone = FileHeader & "rocknstone.gif"
                Wibble = ""
                DeepBreath = ""
                WooHoo = FileHeader & "woo.gif"
                PooBrain = ""
            Else
                Neutral = FileHeader & "blink.gif"
                Happy = FileHeader & "manic.gif"
                Sadge = FileHeader & "sad.gif"
                Angy = FileHeader & "angery.gif"
                Wumpy = FileHeader & "pissed.gif"
                Cringe = FileHeader & "cringe.gif"
                Wow = FileHeader & "nowai.gif"
                Sparkle = FileHeader & "sparkles.gif"
                WTF = ""
                OMG = FileHeader & "omg.gif"
                RockandStone = ""
                Wibble = FileHeader & "wibble.gif"
                DeepBreath = FileHeader & "deepbreath.gif"
                WooHoo = FileHeader & "woo.gif"
                PooBrain = FileHeader & "poobrain.gif"
            End If
        End Sub

    End Structure

    Public Class CharacterControls
        Private Name As String
        Public Directory As String
        Private CurrentMood As String
        Private SourceName As String
        Private Bubble As SpeechBubble
        Public SoundPlayer As SoundController
        Public Mood As CharacterMoods
        Private ResourceID As Integer
        'Public Event Said(MessageData As String)
        Public Event MoodChange(MoodString As String)
        Public LeftHand As Boolean
        Public RightHand As Boolean

        Public Sub New(CharName As String)
            Select Case CharName
                Case "Ember"
                    initEMBER()
                Case "Luna"
                    initLUNA()
            End Select
        End Sub

        Public Sub initLUNA()
            Name = "Luna"
            Mood.InitializeMoods(SpriteID.Luna, "luna_")
            Directory = "\\StreamPC-V2\OBS Assets\Characters\" & Name & "Source.html"
            CurrentMood = ApplyMoodChange()
            SourceName = Name & "SpriteHTML"
            ResourceID = ResourceIDs.LunaSprite
            LeftHand = False
            RightHand = False
            Bubble = New SpeechBubble(Name)
            SoundPlayer = New SoundController(SoundSource.SFX, Name)
        End Sub

        Public Sub initEMBER()
            Name = "Ember"
            Mood.InitializeMoods(SpriteID.Ember, "ember_")
            Directory = "\\StreamPC-V2\OBS Assets\Characters\" & Name & "Source.html"
            CurrentMood = ApplyMoodChange()
            SourceName = Name & "SpriteHTML"
            ResourceID = ResourceIDs.EmberSprite
            LeftHand = False
            RightHand = False
            Bubble = New SpeechBubble(Name)
            SoundPlayer = New SoundController(SoundSource.SFX, Name)
        End Sub

        Public Function ApplyMoodChange(Optional NewMood As String = "") As String
            Dim HTMLinput As String = File.ReadAllText(Directory)
            Dim ActualMood As String = DataExtract(HTMLinput, """GIF/", """>" & vbCrLf)
            If NewMood <> "" Then
                HTMLinput = Replace(HTMLinput, ActualMood, NewMood)
                File.WriteAllText(Directory, HTMLinput)
                OBSmutex.WaitOne()


                'Dim OutputString As String = ""
                'For I As Integer = 0 To Input.Count - 1
                ' OutputString = OutputString & Input(I).ToString & vbCrLf
                'Next
                'SendMessage(OutputString)
                OBS.PressInputPropertiesButton(SourceName, "refreshnocache")
                OBSmutex.ReleaseMutex()
                CurrentMood = NewMood
                Return NewMood
            Else
                Return ActualMood
            End If
        End Function


        Public Async Function Says(InputMessage As String,
                        Optional MessageMood As String = "",
                        Optional MessageSound As String = "",
                        Optional StaticMoodChange As Boolean = False,
                        Optional MoodTimer As Integer = 0,
                        Optional BypassManager As Boolean = False,
                        Optional Lefth As Integer = -1,
                        Optional Righth As Integer = -1,
                        Optional InterimMood As String = "") As Task

            Dim SpeechTask As Task(Of Task) = New Task(Of Task) _
            (Async Function() As Task
                 Dim TempMood As String
                 'SendMessage(CurrentMood)
                 If StaticMoodChange = False And MessageMood <> "" Then
                     TempMood = CurrentMood
                     MessageMood = AppendMood(MessageMood, Lefth, Righth)
                     'CurrentMood = MediaSourceChange(SourceName, 1, Directory & FileHeader & MessageMood)
                     ApplyMoodChange(MessageMood)
                     RaiseEvent MoodChange(CurrentMood)
                 Else
                     TempMood = ""
                 End If

                 Dim MoodTask As Task = Nothing

                 If MoodTimer > 0 And TempMood <> "" Then
                     MoodTask = DelayedSetMood(TempMood, MoodTimer, InterimMood)
                 End If

                 Dim BubbleTask As Task = Bubble.SendMessage(InputMessage)
                 Dim SoundTask As Task = Nothing

                 If MessageSound <> "" Then
                     Await Task.Delay(250)
                     SoundTask = SoundPlayer.PlaySound(AudioControl.GetSoundFileDataByName(MessageSound))
                 End If

                 Await BubbleTask

                 If SoundTask IsNot Nothing Then Await SoundTask

                 If MoodTask IsNot Nothing Then
                     Await MoodTask
                     If InterimMood <> "" And TempMood <> "" Then
                         'CurrentMood = MediaSourceChange(SourceName, 1, TempMood)
                         ApplyMoodChange(TempMood)
                         RaiseEvent MoodChange(CurrentMood)
                     End If
                 Else
                     If MessageMood <> "" And TempMood <> "" Then
                         'CurrentMood = MediaSourceChange(SourceName, 1, TempMood)
                         ApplyMoodChange(TempMood)
                         RaiseEvent MoodChange(CurrentMood)
                     End If
                 End If

                 Await Task.Delay(250)

             End Function)

            If BypassManager = False Then
                Dim MyRequest As New ResourceRequest({ResourceID}, SpeechTask, Name & " Sprite Says: " & InputMessage)
                Dim Speak As Task = MyResourceManager.RequestResource(MyRequest)
            Else
                SpeechTask.Start()
                Await SpeechTask.Result
            End If

        End Function

        Private Async Function DelayedSetMood(MoodString As String, Delay As Integer, InterimMood As String) As Task
            Await Task.Delay(Delay)
            If InterimMood = "" Then
                'CurrentMood = MediaSourceChange(SourceName, 1, MoodString)
                ApplyMoodChange(MoodString)
            Else
                'CurrentMood = MediaSourceChange(SourceName, 1, Directory & FileHeader & InterimMood)
                ApplyMoodChange(InterimMood)
            End If
            RaiseEvent MoodChange(CurrentMood)

        End Function

        Public Function MyMood(Optional Input As String = "", Optional LeftH As Integer = -1, Optional RightH As Integer = -1) As String
            Dim OutputString As String = CurrentMood
            Dim HandsInt As Integer = 0
            If LeftH > -1 Then
                HandsInt = HandsInt + (LeftH * 1)
            Else
                If LeftHand = True Then HandsInt = HandsInt + 1
            End If
            If RightH > -1 Then
                HandsInt = HandsInt + (RightH * 2)
            Else
                If RightHand = True Then HandsInt = HandsInt + 2
            End If

            Select Case HandsInt
                Case 0
                    Return OutputString
                Case = 1
                    Return Replace(OutputString, "_left", "")
                Case = 2
                    Return Replace(OutputString, "_right", "")
                Case = 3
                    Return Replace(OutputString, "_both", "")
            End Select
        End Function

        'Private Async Function MessageAsync(InputMessage As String,
        '                Optional MessageMood As String = "",
        '                Optional MessageSound As String = "",
        '                Optional StaticMoodChange As Boolean = False) As Task
        '    Dim TempMood As String
        '    'SendMessage(CurrentMood)
        '    If StaticMoodChange = False Then
        '        TempMood = CurrentMood
        '    Else
        '        TempMood = ""
        '    End If
        '    If MessageMood <> "" Then
        '        MessageMood = AppendMood(MessageMood)
        '        CurrentMood = MediaSourceChange(SourceName, 1, Directory & FileHeader & MessageMood)
        '        RaiseEvent MoodChange(CurrentMood)
        '    End If
        '    Dim BubbleTask As Task = Bubble.SendMessage(InputMessage)
        '    Dim SoundTask As Task = Nothing
        '    If MessageSound <> "" Then
        '        Await Task.Delay(250)
        '        SoundTask = SoundPlayer.PlaySound(AudioControl.GetSoundFileDataByName(MessageSound))
        '    End If
        '    Await BubbleTask
        '    If SoundTask IsNot Nothing Then Await SoundTask
        '    If MessageMood <> "" And TempMood <> "" Then
        '        CurrentMood = MediaSourceChange(SourceName, 1, TempMood)
        '        RaiseEvent MoodChange(CurrentMood)
        '    End If
        '    Await Task.Delay(250)
        'End Function

        Private Function AppendMood(InputString As String, Optional LeftH As Integer = -1, Optional RightH As Integer = -1) As String
            Dim HandsInt As Integer = 0
            If LeftH > -1 Then
                HandsInt = HandsInt + (LeftH * 1)
            Else
                If LeftHand = True Then HandsInt = HandsInt + 1
            End If
            If RightH > -1 Then
                HandsInt = HandsInt + (RightH * 2)
            Else
                If RightHand = True Then HandsInt = HandsInt + 2
            End If

            Select Case HandsInt
                Case = 1
                    Return Replace(InputString, ".", "_left.")
                Case = 2
                    Return Replace(InputString, ".", "_right.")
                Case = 3
                    Return Replace(InputString, ".", "_both.")
                Case Else
                    Return InputString
            End Select
        End Function

        Public Sub ChangeMood(MoodFileName As String,
                              Optional duration As Integer = 0,
                              Optional BypassManager As Boolean = False,
                              Optional PlaySound As String = "")
            Dim MoodTask As Task(Of Task) = New Task(Of Task) _
            (Async Function() As Task
                 Await ChangeMoodAsync(MoodFileName, duration, PlaySound)
             End Function)
            If BypassManager Then
                MoodTask.Start()
            Else
                Dim MyRequest As New ResourceRequest({ResourceID}, MoodTask, Name & " Sprite Mood Change")
                Dim RunMood As Task = MyResourceManager.RequestResource(MyRequest)
            End If
        End Sub

        Private Async Function ChangeMoodAsync(MoodFileName As String,
                                               Optional duration As Integer = 0,
                                               Optional PlaySound As String = "") As Task
            If MoodFileName <> "" Then
                If PlaySound <> "" Then Dim SoundTask As Task = SoundPlayer.PlaySound(AudioControl.GetSoundFileDataByName(PlaySound))
                MoodFileName = AppendMood(MoodFileName)
                    If duration > 0 Then
                        Dim TempMood As String = CurrentMood
                        'CurrentMood = MediaSourceChange(SourceName, 1, Directory & FileHeader & MoodFileName)
                        ApplyMoodChange(MoodFileName)
                        RaiseEvent MoodChange(CurrentMood)
                        Await Task.Delay(duration)
                        'CurrentMood = MediaSourceChange(SourceName, 1, TempMood)
                        ApplyMoodChange(TempMood)
                        RaiseEvent MoodChange(CurrentMood)
                    Else
                        If CurrentMood <> MoodFileName Then
                            'CurrentMood = MediaSourceChange(SourceName, 1, Directory & FileHeader & MoodFileName)
                            ApplyMoodChange(MoodFileName)
                            RaiseEvent MoodChange(CurrentMood)
                        End If
                    End If
                End If
        End Function
    End Class

    Public Class SpeechBubble
        Private Name As String
        Private Bubble As String
        Private Msource As String
        Private Character As String

        Public Event MessageSent(Messenger As String, Message As String)

        Public Sub New(CharName As String)
            Character = CharName
            Name = CharName & "'s Speech Bubble"
            Bubble = CharName & "MessengerGIF"
            Msource = CharName & " Mtext"
        End Sub

        Public Async Function SendMessage(MessageText As String) As Task
            RaiseEvent MessageSent(Name, MessageText)
            SetSourceVisibility(0, Character, True, True, Bubble)
            AudioControl.SoundPlayer.PlaySound("sfx_drip.wav", SoundSource.BeepBoop)
            Await Task.Delay(220)
            Call SetOBSsourceText(Msource, MessageText)
            Await Task.Delay(3700)
            Call SetOBSsourceText(Msource, "")
            Await Task.Delay(500)
            SetSourceVisibility(0, Character, False, True, Bubble)
            'Bubble.Hide()
        End Function
    End Class


    '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    '///////////////////////////////////////////////COUNTER FUNCTIONS/////////////////////////////////////////////////////////
    '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    Public OBScounterObject() As TimerCounterData
    Public CounterTicker As SoundController
    Public Event CountersUpdated()

    Public Structure CounterIDs
        Public Const Glob As Integer = 0
        Public Const Ember As Integer = 1
        Public Const Luna As Integer = 2
    End Structure

    Public Async Function RunOBScounter(CounterSelection As Integer, PrevValue As Integer, NewValue As Integer,
                                        Optional CounterTitle As String = "",
                                        Optional CounterSound As String = "",
                                        Optional CounterTick As String = "") As Task
        OBScounterObject(CounterSelection).State = True

        OBScounterObject(CounterSelection).Title = Replace(CounterTitle, " ", "  ")
        SetOBSsourceText(OBScounterObject(CounterSelection).TitleSource, OBScounterObject(CounterSelection).Title)

        If PrevValue < 10 Then
            OBScounterObject(CounterSelection).Value = "0" & PrevValue
        Else
            OBScounterObject(CounterSelection).Value = PrevValue
        End If
        SetOBSsourceText(OBScounterObject(CounterSelection).ValueSource, OBScounterObject(CounterSelection).Value)


        Await UpdateSceneDisplay()
        RaiseEvent CountersUpdated()
        Dim SoundTask As Task
        If CounterSound <> "" Then SoundTask =
            OBScounterObject(CounterSelection).SoundPlayer.PlaySound(AudioControl.GetSoundFileDataByName(CounterSound))

        Await Task.Delay(1800)
        If NewValue < 10 Then
            OBScounterObject(CounterSelection).Value = "0" & NewValue
        Else
            OBScounterObject(CounterSelection).Value = NewValue
        End If
        Dim Splode As Task = OBScounterObject(CounterSelection).CounterBoom.Show()
        If CounterTick <> "" Then Dim TickTask As Task = CounterTicker.PlaySound(CounterTick)
        Await Task.Delay(10)
        SetOBSsourceText(OBScounterObject(CounterSelection).ValueSource, OBScounterObject(CounterSelection).Value)

        Await Task.Delay(2200)
        OBScounterObject(CounterSelection).Title = ""
        OBScounterObject(CounterSelection).State = False
        Await UpdateSceneDisplay()
        RaiseEvent CountersUpdated()
        Await Task.Delay(500)

        If CounterSound <> "" Then Await SoundTask

    End Function

    Public Sub InitializeCountersObjects()
        ReDim OBScounterObject(0 To 2)
        OBScounterObject(CounterIDs.Glob) = New TimerCounterData("Global Counter", ResourceIDs.GlobalCounter)
        OBScounterObject(CounterIDs.Ember) = New TimerCounterData("Ember Counter", ResourceIDs.EmberCounter)
        OBScounterObject(CounterIDs.Luna) = New TimerCounterData("Luna Counter", ResourceIDs.LunaCounter)
        CounterTicker = New SoundController(SoundSource.BeepBoop, "Counter Tick")
    End Sub

    Public Class TimerCounterData
        Public State As Boolean
        Public Title As String
        Public TitleSource As String
        Public Value As String
        Public ValueSource As String
        Public Name As String
        Public ResourceID As Integer
        Public SoundPlayer As SoundController
        Public CounterBoom As VideoPlayer
        Public Sub New(MyName As String, RID As Integer)
            State = False
            Title = ""
            Value = "00"
            Name = MyName
            ResourceID = RID
            TitleSource = Replace(Name, " ", "") & "Title"
            ValueSource = Replace(Name, " ", "") & "Value"
            SoundPlayer = New SoundController(SoundSource.SFX, Name)
            CounterBoom = New VideoPlayer(Replace(Name, " ", "") & "Splode", "Counters&Timers")
        End Sub
    End Class

    Public Class OBScounterData
        Public Counters() As CounterProperties
        Public Total As Integer

        Public Sub New()
            InitializeCountersObjects()
            Call LoadCounterData()
        End Sub

        Public Function GetCounterIndex(CounterName As String) As Integer
            For i = 0 To Total - 1
                If Counters(i).Name = CounterName Then
                    Return i
                    Exit Function
                End If
            Next
            Return -1
        End Function

        Public Sub CounterSub(CounterIndex As Integer)
            If CounterIndex < Total Then
                Counters(CounterIndex).SubCount()
            End If
        End Sub

        Public Function UserCount(CounterName As String) As Boolean
            For i As Integer = 0 To Total - 1
                If Counters(i).Name.Replace(" ", "_") = CounterName Then
                    Return CounterAdd(i, True)
                    Exit Function
                End If
            Next
            Return False
        End Function

        Public Function CounterAdd(CounterIndex As Integer, Optional UserGenerated As Boolean = False) As Boolean
            Dim ReturnValue As Boolean = False
            If CounterIndex < Total Then
                Counters(CounterIndex).AddCount()
                RunCounter(CounterIndex)
                ReturnValue = True
            End If
            Return ReturnValue
        End Function

        Public Sub RunCounter(CounterIndex As Integer)
            If CounterIndex < Total Then
                Dim CountMe As New Task(Of Task) _
                (Async Function() As Task
                     Await RunOBScounter(Counters(CounterIndex).Type,
                                         Counters(CounterIndex).PrevValue,
                                         Counters(CounterIndex).Count,
                                         Counters(CounterIndex).Label,
                                         Counters(CounterIndex).Sound,
                                         Counters(CounterIndex).Tick)
                 End Function)

                Dim MyRequest As New ResourceRequest({OBScounterObject(Counters(CounterIndex).Type).ResourceID},
                                                      CountMe, "CountIndex#(" & CounterIndex & ") " & Counters(CounterIndex).Name)
                Dim TheCount As Task = MyResourceManager.RequestResource(MyRequest)
            End If
        End Sub

        Public Function ReadPublicCounters() As String
            Dim OutputString As String = ""
            For i As Integer = 0 To Total - 1
                If Counters(i).PublicCount = True Then
                    If OutputString <> "" Then OutputString = OutputString & ", "
                    OutputString = OutputString & Counters(i).Name.Replace(" ", "_") & "(" & Counters(i).Count & ")"
                End If
            Next
            Return OutputString
        End Function

        Public Sub LoadCounterData(Optional FileString As String = "\\StreamPC-V2\OBS Assets\Counters\CounterData.csv")
            Dim CountersList As List(Of CounterProperties) = New List(Of CounterProperties)
            If File.Exists(FileString) = True Then
                Dim Reader As StreamReader = File.OpenText(FileString)
                Dim InputCounter As New CounterProperties
                Do Until Reader.EndOfStream = True
                    InputCounter.ReadCounterData(Reader.ReadLine())
                    CountersList.Add(InputCounter)
                Loop
                Reader.Close()
                Reader.Dispose()
                Total = CountersList.Count
                Dim InputCounters(0 To Total - 1) As CounterProperties
                For i As Integer = 0 To Total - 1
                    InputCounters(i) = CountersList(i)
                Next
                Counters = InputCounters
            End If
        End Sub

        Public Sub SaveCounterData(Optional FileString As String = "\\StreamPC-V2\OBS Assets\Counters\CounterData.csv")
            File.Create(FileString).Dispose()
            If File.Exists(FileString) = True Then
                Dim Writer As StreamWriter = File.AppendText(FileString)
                For i As Integer = 0 To Total - 1
                    Writer.WriteLine(Counters(i).WriteCounterData)
                Next
                Writer.Close()
                Writer.Dispose()
            End If
        End Sub

        Public Sub AppendCounterData(InputCounter As CounterProperties, Optional FileString As String = "\\StreamPC-V2\OBS Assets\Counters\CounterData.csv")
            If File.Exists(FileString) = True Then
                Dim Writer As StreamWriter = File.AppendText(FileString)
                Writer.WriteLine(InputCounter.WriteCounterData)
                Writer.Close()
                Writer.Dispose()
            End If
        End Sub

    End Class


    Public Structure CounterProperties
        Public Name As String
        Public Label As String
        Public Count As Integer
        Public Increment As Integer
        Public Type As Integer '0=Global, 1=Ember, 2=Luna
        Public Sound As String
        Public OBSevent As String
        Public NotificationType As Boolean
        Public PublicCount As Boolean
        Public Tick As String

        Public Function PrevValue() As Integer
            Dim InputValue = Count
            InputValue = InputValue - Increment
            Return InputValue
        End Function

        Public Sub SubCount()
            If Count > 0 Then
                Count = Count - Increment
            End If
        End Sub

        Public Sub AddCount()
            If Count < 100 Then
                Count = Count + Increment
            End If
            If Count > 100 Then
                Count = 100
            End If
        End Sub

        Public Sub ReadCounterData(DataLine As String)
            Dim splitstring() As String = Split(DataLine, ",")
            Dim SplitString2() As String
            For I As Integer = 0 To splitstring.Length - 1
                SplitString2 = Split(splitstring(I), "<>")
                Select Case SplitString2(0)
                    Case = "Name"
                        Name = SplitString2(1)
                    Case = "Label"
                        Label = SplitString2(1)
                    Case = "Count"
                        Count = SplitString2(1)
                    Case = "Increment"
                        Increment = SplitString2(1)
                    Case = "Type"
                        Type = SplitString2(1)
                    Case = "Sound"
                        Sound = SplitString2(1)
                    Case = "OBSevent"
                        OBSevent = SplitString2(1)
                    Case = "NotificationType"
                        NotificationType = SplitString2(1)
                    Case = "PublicCount"
                        PublicCount = SplitString2(1)
                    Case = "Tick"
                        Tick = SplitString2(1)
                End Select
            Next
        End Sub

        Public Function WriteCounterData() As String
            Dim Outputstring As String =
            "Name<>" & Name & "," &
            "Label<>" & Label & "," &
            "Count<>" & Count & "," &
            "Increment<>" & Increment & "," &
            "Type<>" & Type & "," &
            "Sound<>" & Sound & "," &
            "OBSevent<>" & OBSevent & "," &
            "NotificationType<>" & NotificationType & "," &
            "PublicCount<>" & PublicCount & "," &
            "Tick<>" & Tick

            Return Outputstring
        End Function
    End Structure



    '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    '/////////////////////////////////////////////////////TIMER FUNCTIONS/////////////////////////////////////////////////////
    '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    Public OBStimerObject() As OBSTimer
    Public Event TimersUpdated(TimerType As Integer)

    Public Class OBSTimer
        Public State As Boolean
        Public Pause As Boolean
        Public Unpause As TaskCompletionSource(Of Boolean)
        Public Name As String
        Public Title As String
        Public TitleSource As String
        Public Clock As String
        Public ClockSource As String
        Public Sounds As List(Of TimerSound)
        'Public Alarm As String
        'Public SoundSource As String
        Public ResourceID As Integer
        Public SoundPlayer As SoundController
        Public Event TimerStarted()
        Public Event TimerStopped()
        Public Sub StartTimer()
            RaiseEvent TimerStarted()
        End Sub
        Public Sub StopTimer()
            RaiseEvent TimerStopped()
        End Sub
        Public Sub New(MyName As String, RID As Integer, Tsource As String, Csource As String)
            State = False
            Pause = False
            Title = ""
            Clock = "5:00"
            Sounds = New List(Of TimerSound)
            Name = MyName
            ResourceID = RID
            TitleSource = Tsource
            ClockSource = Csource
            SoundPlayer = New SoundController(SoundSource.SFX, Name)
        End Sub

    End Class

    Public Structure TimerSound
        Public SoundTime As Integer
        Public SoundObject As String
    End Structure

    Public Structure TimerIDs
        Public Const GlobalCC As Integer = 0
        Public Const Ember As Integer = 1
        Public Const Luna As Integer = 2
        Public Const GlobalUL As Integer = 3
        Public Const GlobalLR As Integer = 4
    End Structure

    Public Sub InitializeOBStimers()
        ReDim OBStimerObject(0 To 2)

        OBStimerObject(TimerIDs.GlobalCC) = New OBSTimer("Global Timer",
                                                        ResourceIDs.GlobalTimer,
                                                        "GlobalTimerTitle",
                                                        "GlobalTimerClock")

        OBStimerObject(TimerIDs.Ember) = New OBSTimer("Ember Timer",
                                                        ResourceIDs.EmberTimer,
                                                        "EmberTimerTitle",
                                                        "EmberTimerClock")

        OBStimerObject(TimerIDs.Luna) = New OBSTimer("Luna Timer",
                                                        ResourceIDs.LunaTimer,
                                                        "LunaTimerTitle",
                                                        "LunaTimerClock")
    End Sub

    Public Async Function RunTimer(TimeInSeconds As Integer, TimerSelection As Integer, Optional StopTime As Integer = 0,
                                   Optional Label As String = "", Optional tSounds As List(Of TimerSound) = Nothing) As Task
        OBStimerObject(TimerSelection).State = True

        If Label <> "" Then
            OBStimerObject(TimerSelection).Title = Replace(Label, " ", "  ")
        End If

        SetOBSsourceText(OBStimerObject(TimerSelection).TitleSource, OBStimerObject(TimerSelection).Title)

        OBStimerObject(TimerSelection).Clock = TimeString(TimeInSeconds)
        SetOBSsourceText(OBStimerObject(TimerSelection).ClockSource, OBStimerObject(TimerSelection).Clock)

        Dim Increment As Integer = -1
        If StopTime >= TimeInSeconds Then Increment = 1

        Await UpdateSceneDisplay()
        RaiseEvent TimersUpdated(TimerSelection)

        Await Task.Delay(1000)
        Do Until OBStimerObject(TimerSelection).State = False
            If tSounds IsNot Nothing Then
                If tSounds.Count > 0 Then
                    For I As Integer = 0 To tSounds.Count - 1
                        If tSounds(I).SoundTime = TimeInSeconds Then
                            If tSounds(I).SoundObject <> "" Then
                                Dim Stask As Task = OBStimerObject(TimerSelection).SoundPlayer.PlaySound(AudioControl.GetSoundFileDataByName(tSounds(I).SoundObject))
                                GoTo SoundPlayed1
                            End If
                        End If
                    Next
                End If
            End If
SoundPlayed1:
            Await Task.Delay(1000)
            TimeInSeconds = TimeInSeconds + Increment
            OBStimerObject(TimerSelection).Clock = TimeString(TimeInSeconds)
            SetOBSsourceText(OBStimerObject(TimerSelection).ClockSource, OBStimerObject(TimerSelection).Clock)
            RaiseEvent TimersUpdated(TimerSelection)

            If OBStimerObject(TimerSelection).Pause = True Then
                'Do Until OBStimerObject(TimerSelection).Pause = False
                OBStimerObject(TimerSelection).Pause = Await OBStimerObject(TimerSelection).Unpause.Task
                'Loop
            End If

            If TimeInSeconds = StopTime Then
                If tSounds IsNot Nothing Then
                    If tSounds.Count > 0 Then
                        For I As Integer = 0 To tSounds.Count - 1
                            If tSounds(I).SoundTime = TimeInSeconds Then
                                If tSounds(I).SoundObject <> "" Then
                                    Dim Stask As Task = OBStimerObject(TimerSelection).SoundPlayer.PlaySound(AudioControl.GetSoundFileDataByName(tSounds(I).SoundObject))
                                    GoTo SoundPlayed2
                                End If
                            End If
                        Next
                    End If
                End If
SoundPlayed2:
                Await Task.Delay(1000)
                OBStimerObject(TimerSelection).State = False
            End If
        Loop
        OBStimerObject(TimerSelection).Title = ""
        OBStimerObject(TimerSelection).Sounds = Nothing

        Await UpdateSceneDisplay()
        OBStimerObject(TimerSelection).Pause = False
        RaiseEvent TimersUpdated(TimerSelection)
        Await Task.Delay(500)
    End Function

    Public Structure TimerButton
        Public Name As String
        Public Label As String
        Public Type As Integer
        Public Time As Integer
        'Public Active As Boolean
        Public PublicBool As Boolean
        Public Sounds As List(Of TimerSound)
        'Public Alarm As String
        Public StopTime As Integer
        Public Access As Mutex

        Public Sub InitializeTimer()
            If Access Is Nothing Then Access = New Mutex
            Access.WaitOne()
            Name = ""
            Label = ""
            Time = 60
            'Active = False
            Sounds = New List(Of TimerSound)
            'Alarm = ""
            StopTime = 0
            Access.ReleaseMutex()
        End Sub

        Public Function TimertoString() As String
            Dim OutputString As String = ""
            OutputString = OutputString & "Label: " & Label & vbCrLf
            OutputString = OutputString & "Type: " & Type & vbCrLf
            OutputString = OutputString & "Time: " & Time & vbCrLf
            OutputString = OutputString & "StopTime: " & StopTime & vbCrLf
            'OutputString = OutputString & "Alarm: " & Alarm & vbCrLf
            OutputString = OutputString & "PublicBool: " & PublicBool & vbCrLf
            If Sounds.Count <> 0 Then
                OutputString = OutputString & "Sounds: "
                For I As Integer = 0 To Sounds.Count - 1
                    OutputString = OutputString & Sounds(I).SoundTime & "#" & Sounds(I).SoundObject
                    If I < Sounds.Count - 1 Then OutputString = OutputString & ","
                Next
            End If
            Return OutputString
        End Function

        Public Sub DeleteTimerButt(TimerDirectory As String)
            If Name <> "" Then
                If Access Is Nothing Then Access = New Mutex
                Access.WaitOne()
                If File.Exists(TimerDirectory & "\" & Name & ".txt") Then
                    File.Delete(TimerDirectory & "\" & Name & ".txt")
                End If
                Access.ReleaseMutex()
                InitializeTimer()
            End If
        End Sub

        Public Sub ChangeName(TimerDirectory As String, NewName As String)
            If Access Is Nothing Then Access = New Mutex
            Access.WaitOne()
            If Name <> "" Then
                If File.Exists(TimerDirectory & "\" & Name & ".txt") Then
                    Rename(TimerDirectory & "\" & Name & ".txt", TimerDirectory & "\" & NewName & ".txt")
                End If
            End If
            Name = NewName
            Access.ReleaseMutex()
        End Sub

        Public Sub ReadTimerButt(TimerDirectory As String, NameString As String)
            If Access Is Nothing Then Access = New Mutex
            Access.WaitOne()
            If NameString <> "" Then
                Name = NameString
            End If
            If Name <> "" Then
                If File.Exists(TimerDirectory & "\" & Name & ".txt") Then
                    'SendMessage("File exists")
                    Dim InputStream As StreamReader = File.OpenText(TimerDirectory & "\" & Name & ".txt")
                    Do Until InputStream.EndOfStream = True
                        ReadTimerElement(InputStream.ReadLine())
                    Loop
                    InputStream.Close()
                    InputStream.Dispose()
                End If
            End If
            Access.ReleaseMutex()
        End Sub

        Public Sub WriteTimerButt(TimerDirectory As String)
            If Name <> "" Then
                If Access Is Nothing Then Access = New Mutex
                Access.WaitOne()
                File.WriteAllText(TimerDirectory & "\" & Name & ".txt", TimertoString)
                Access.ReleaseMutex()
            End If
        End Sub

        Private Sub ReadTimerElement(InputString As String)
            Dim SplitString() As String
            SplitString = Split(InputString, ": ")
            If SplitString.Length > 1 Then
                Select Case SplitString(0)
                    Case = "Label"
                        Label = SplitString(1)
                    Case = "Time"
                        Time = SplitString(1)
                    Case = "StopTime"
                        StopTime = SplitString(1)
                    Case = "Type"
                        Type = SplitString(1)
                    Case = "PublicBool"
                        PublicBool = SplitString(1)
                    Case = "Sounds"
                        Dim SoundsString() As String = SplitString(1).Split(",")
                        If SoundsString.Length <> 0 Then
                            Sounds = New List(Of TimerSound)
                            Dim InputSound As TimerSound, SplitSound() As String
                            For i As Integer = 0 To SoundsString.Length - 1
                                SplitSound = SoundsString(i).Split("#")
                                InputSound.SoundTime = SplitSound(0)
                                InputSound.SoundObject = SplitSound(1)
                                Sounds.Add(InputSound)
                            Next
                        End If
                End Select
            End If
        End Sub

    End Structure

    Public Class OBSTimerData

        Public TimerDirectory As String
        Public Event TimerDataUpdated()
        Public Timers() As TimerButton

        Public Sub New()
            TimerDirectory = "\\StreamPC-V2\OBS Assets\Timers"
            InitializeOBStimers()
            ReadTimerData()
        End Sub

        Public Sub RunTimerbyData(TimeInSeconds As Integer, TimerID As Integer)
            Dim GoTimer As New Task(Of Task)(Async Function() As Task
                                                 Await RunTimer(TimeInSeconds, TimerID, , OBStimerObject(TimerID).Title)
                                             End Function)
            Dim MyRequest As New ResourceRequest({OBStimerObject(TimerID).ResourceID}, GoTimer, OBStimerObject(TimerID).Name)
            Dim RunThyTimer As Task = MyResourceManager.RequestResource(MyRequest)
        End Sub



        Public Async Function RunTimerbyIndex(TimerIndex As Integer, Optional DontQueue As Boolean = True) As Task
            Dim GoTimer As New Task(Of Task) _
            (Async Function() As Task
                 Await RunTimer(Timers(TimerIndex).Time, Timers(TimerIndex).Type, Timers(TimerIndex).StopTime,
                                Timers(TimerIndex).Label, Timers(TimerIndex).Sounds)
             End Function)

            Dim MyRequest As New ResourceRequest({OBStimerObject(Timers(TimerIndex).Type).ResourceID},
                                                GoTimer, OBStimerObject(Timers(TimerIndex).Type).Name)
            Dim RunThyTimer As Boolean = Await MyResourceManager.RequestResource(MyRequest, DontQueue)
            If RunThyTimer = False Then SendMessage("Timer is in Use", "UH OH!")
        End Function

        Public Sub RunTimerbyName(TimerName As String, Optional DontQueue As Boolean = True)
            Dim TimerIndex As Integer = GetTimerIndexByName(TimerName)
            Dim TimerTask As Task = RunTimerbyIndex(TimerIndex, DontQueue)
        End Sub

        Public Sub PauseTimerByID(TimerID As Integer)
            If OBStimerObject(TimerID).Pause = True Then
                OBStimerObject(TimerID).Unpause.SetResult(False)
            Else
                OBStimerObject(TimerID).Unpause = New TaskCompletionSource(Of Boolean)
                OBStimerObject(TimerID).Pause = True
            End If
        End Sub

        Public Sub PauseTimerByIndex(TimerIndex As Integer)
            PauseTimerByID(Timers(TimerIndex).Type)
        End Sub

        Public Sub PauseTimerByName(TimerName As String)
            Dim TimerIndex As Integer = GetTimerIndexByName(TimerName)
            PauseTimerByID(Timers(TimerIndex).Type)
        End Sub

        Public Sub StopTimerByID(TimerID As Integer)
            OBStimerObject(TimerID).Pause = False
            OBStimerObject(TimerID).State = False
        End Sub

        Public Sub StopTimerByIndex(TimerIndex As Integer)
            StopTimerByID(Timers(TimerIndex).Type)
        End Sub

        Public Sub StopTimerByName(TimerName As String)
            Dim TimerIndex As Integer = GetTimerIndexByName(TimerName)
            StopTimerByID(Timers(TimerIndex).Type)
        End Sub

        Public Sub DeleteTimer(ButtIndex As Integer)
            Timers(ButtIndex).DeleteTimerButt(TimerDirectory)
            ReadTimerData()
        End Sub

        Public Sub AddTimer(ButtData As TimerButton)
            ButtData.WriteTimerButt(TimerDirectory)
            ReadTimerData()
        End Sub

        Public Sub UpdateTimer(ButtIndex As Integer, ButtData As TimerButton)
            If Timers(ButtIndex).Name <> ButtData.Name Then
                Timers(ButtIndex).ChangeName(TimerDirectory, ButtData.Name)
            End If
            Timers(ButtIndex) = ButtData
            Timers(ButtIndex).WriteTimerButt(TimerDirectory)
            RaiseEvent TimerDataUpdated()
        End Sub

        Public Sub RenameTimer(TimerIndex As Integer, NewName As String)
            Timers(TimerIndex).ChangeName(TimerDirectory, NewName)
            RaiseEvent TimerDataUpdated()
        End Sub

        Public Sub ReadTimerData()
            If Directory.Exists(TimerDirectory) Then
                Dim di As New IO.DirectoryInfo(TimerDirectory)
                Dim aryFi As IO.FileInfo() = di.GetFiles("*.txt")
                If aryFi.Length <> 0 Then
                    ReDim Timers(0 To aryFi.Length - 1)
                    For i As Integer = 0 To aryFi.Length - 1
                        Timers(i).InitializeTimer()
                        Timers(i).ReadTimerButt(TimerDirectory, Replace(aryFi(i).Name, ".txt", ""))
                    Next
                Else
                    Timers = Nothing
                End If
            End If
            RaiseEvent TimerDataUpdated()
        End Sub

        Public Sub WriteTimerData()
            If Timers IsNot Nothing Then
                If Timers.Length <> 0 Then
                    For i As Integer = 0 To Timers.Length - 1
                        Timers(i).WriteTimerButt(TimerDirectory)
                    Next
                End If
            End If
        End Sub

        Public Function GetFullTimerList() As List(Of String)
            Dim OutputList As New List(Of String)
            If Timers IsNot Nothing Then
                If Timers.Length <> 0 Then
                    For I As Integer = 0 To Timers.Length - 1
                        If Timers(I).Name <> "" Then
                            OutputList.Add(Timers(I).Name)
                        End If
                    Next
                End If
            End If
            OutputList.Sort()
            Return OutputList
        End Function

        Public Function GetPublicTimersList() As List(Of String)
            Dim OutputList As New List(Of String)
            If Timers IsNot Nothing Then
                If Timers.Length <> 0 Then
                    For I As Integer = 0 To Timers.Length - 1
                        If Timers(I).PublicBool = True Then
                            If Timers(I).Name <> "" Then
                                OutputList.Add(Timers(I).Name)
                            End If
                        End If
                    Next
                End If
            End If
            OutputList.Sort()
            Return OutputList
        End Function

        Public Function CheckTimerNames(NameSuggestion As String) As Boolean
            If Timers IsNot Nothing Then
                If Timers.Length <> 0 Then
                    For I As Integer = 0 To Timers.Length - 1
                        If Timers(I).Name = NameSuggestion Then
                            Return False
                            Exit Function
                        End If
                    Next
                End If
            End If
            Return True
        End Function

        Public Function GetTimerIndexByName(TimerName As String) As Integer
            Dim TimerIndex As String = -1
            If Timers IsNot Nothing Then
                If Timers.Length <> 0 Then
                    For I As Integer = 0 To Timers.Length - 1
                        If Timers(I).Name = TimerName Then
                            TimerIndex = I
                            GoTo FoundIt
                        End If
                    Next
                End If
            End If
FoundIt:
            Return TimerIndex
        End Function

    End Class



    '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    '/////////////////////////////////////////////////////AUDIO FUNCTIONS/////////////////////////////////////////////////////
    '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    Public Function BuildMusicList(Optional DirectoryString As String = "\\StreamPC-V2\OBS Assets\Music",
                                   Optional ShuffleList As Boolean = True,
                                   Optional Randoms As Boolean = False) As List(Of String)
        Dim di As New IO.DirectoryInfo(DirectoryString)
        Dim aryFi As IO.FileInfo() = di.GetFiles("*.wav")
        Dim fi As IO.FileInfo
        Dim OutputList As New List(Of String)
        For Each fi In aryFi
            OutputList.Add(fi.Name)
        Next
        aryFi = di.GetFiles("*.mp3")
        For Each fi In aryFi
            If fi.Name <> "" Then OutputList.Add(fi.Name)
        Next
        If Randoms = True Then
            For Each Dir As String In Directory.GetDirectories(DirectoryString)
                OutputList.Add(Replace(Dir, DirectoryString & "\", ""))
            Next
        End If
        If ShuffleList = True Then OutputList = Shuffle(OutputList)
        Return OutputList
    End Function

    Public Sub SetVolume(Sources() As String, Level As Single, Muted As Boolean, Monitor As Boolean, Optional SyncChannel As AudioChannel = Nothing)
        OBSmutex.WaitOne()
        Dim MyInfo As VolumeInfo, MonBool As Boolean, MuteBool As Boolean
        For I As Integer = 0 To Sources.Length - 1
            MyInfo = OBS.GetInputVolume(Sources(I))
            MuteBool = OBS.GetInputMute(Sources(I))
            MonBool = (OBS.GetInputAudioMonitorType(Sources(I)) = MonitorType.None) = False
            If I = 0 And SyncChannel IsNot Nothing Then
                SyncChannel.Level = MyInfo.VolumeDb
                Level = SyncChannel.Level
                SyncChannel.Muted = MuteBool
                Muted = SyncChannel.Muted
                SyncChannel.Monitored = MonBool
                Monitor = MonBool
            Else
                If MyInfo.VolumeDb <> Level Then OBS.SetInputVolume(Sources(I), Level, True)
                If MuteBool <> Muted Then OBS.SetInputMute(Sources(I), Muted)
                If Monitor <> MonBool Then
                    If Monitor Then
                        OBS.SetInputAudioMonitorType(Sources(I), MonitorType.Enabled)
                    Else
                        OBS.SetInputAudioMonitorType(Sources(I), MonitorType.None)
                    End If
                End If
            End If
        Next
        OBSmutex.ReleaseMutex()
    End Sub

    Public Class AudioChannels
        Public Channels() As AudioChannel
        Public Const AudioData As String = "\\StreamPC-V2\OBS Assets\Sounds\MixerSettings"
        Public Event MixerChannelChanged(ChannelID As Integer)

        Public Sub New()
            ReDim Channels(0 To AudioChannelIDs.LastChanneL)
            For I As Integer = 0 To AudioChannelIDs.LastChanneL
                Channels(I) = New AudioChannel(I)
            Next
            'AddHandler OBS.SourceVolumeChanged, AddressOf VolumeChanged
            'AddHandler OBS.SourceMuteStateChanged, AddressOf MuteChanged
        End Sub

        Public Sub SyncAll()
            For I As Integer = 0 To AudioChannelIDs.LastChanneL
                Channels(I).SyncSettings()
            Next
        End Sub

        Public Sub ApplyAll()
            For I As Integer = 0 To AudioChannelIDs.LastChanneL
                Channels(I).ApplySettings()
            Next
        End Sub

        Public Sub MuteChanged(sender As OBSWebsocketDotNet.OBSWebsocket, SourceName As String, Muted As Boolean)
            For I As Integer = 0 To AudioChannelIDs.LastChanneL
                If Channels(I).SourceCollection.Contains(SourceName) Then
                    Channels(I).Muted = Muted
                    Channels(I).ApplySettings()
                    RaiseEvent MixerChannelChanged(I)
                    Exit Sub
                End If
            Next
        End Sub

        Public Sub VolumeChanged(sender As OBSWebsocketDotNet.OBSWebsocket, SourceName As String, Volume As Single)
            For I As Integer = 0 To AudioChannelIDs.LastChanneL
                If Channels(I).SourceCollection.Contains(SourceName) Then
                    Channels(I).ApplySettings(Volume)
                    RaiseEvent MixerChannelChanged(I)
                    Exit Sub
                End If
            Next
        End Sub

        Public Sub ToggleMute(ChannelIndex As Integer)
            Channels(ChannelIndex).ApplySettings(, True)
            RaiseEvent MixerChannelChanged(ChannelIndex)
        End Sub


        Public Sub ToggleMon(ChannelIndex As Integer)
            Channels(ChannelIndex).ApplySettings(, , True)
            RaiseEvent MixerChannelChanged(ChannelIndex)
        End Sub

        Public Sub ChangeVolume(ChannelIndex As Integer, Level As Single)
            Channels(ChannelIndex).ApplySettings(Level)
            RaiseEvent MixerChannelChanged(ChannelIndex)
        End Sub

        Public Sub ReadChannelSettings()
            If File.Exists(AudioData) Then
                Dim InputStream As StreamReader = File.OpenText(AudioData)
                Dim Startindex As Integer = 0
                Do Until InputStream.EndOfStream = True
                    If Startindex > AudioChannelIDs.LastChanneL Then GoTo ExitLoop
                    Channels(Startindex).ReadLine(InputStream.ReadLine())
                    Startindex = Startindex + 1
                Loop
ExitLoop:
                InputStream.Close()
                InputStream.Dispose()
            End If
        End Sub

        Public Sub WriteChannelSetting()
            File.Create(AudioData).Dispose()
            If File.Exists(AudioData) = True Then
                Dim Writer As StreamWriter = File.AppendText(AudioData)
                For i As Integer = 0 To AudioChannelIDs.LastChanneL
                    Writer.WriteLine(Channels(i).WriteLine())
                Next
                Writer.Close()
                Writer.Dispose()
            End If
        End Sub
    End Class

    Public Structure AudioChannelIDs
        Public Const EmberMic As Integer = 0
        Public Const EmberPC As Integer = 2
        Public Const LunaMic As Integer = 1
        Public Const LunaPC As Integer = 3
        Public Const Console As Integer = 4
        Public Const Discord As Integer = 5
        Public Const MusicPlayer As Integer = 6
        Public Const SoundsAndEvents As Integer = 7

        Public Const LastChanneL As Integer = 7
    End Structure

    Public Class AudioChannel
        Public Name As String
        Public Muted As Boolean
        Public Monitored As Boolean
        Public Level As Single
        Public SourceCollection() As String

        Public Sub New(ChannelID As Integer)
            Muted = False
            Level = -10
            Monitored = False
            Select Case ChannelID
                Case AudioChannelIDs.EmberMic
                    Name = "EMBER MIC"
                    SourceCollection = {"MOTU Ember"}
                Case AudioChannelIDs.EmberPC
                    Name = "EMBER PC"
                    SourceCollection = {"Ember's PC Audio"}
                Case AudioChannelIDs.LunaMic
                    Name = "LUNA MIC"
                    SourceCollection = {"MOTU Luna"}
                Case AudioChannelIDs.LunaPC
                    Name = "LUNA PC"
                    SourceCollection = {"Luna's PC Audio"}
                Case AudioChannelIDs.Console
                    Name = "CONSOLE"
                    SourceCollection = {"Console Audio"}
                Case AudioChannelIDs.Discord
                    Name = "DISCORD"
                    SourceCollection = {"Discord"}
                Case AudioChannelIDs.MusicPlayer
                    Name = "MUSIC PLAYER"
                    SourceCollection = {"Music Player"}
                Case AudioChannelIDs.SoundsAndEvents
                    Name = "SOUNDS + EVENTS"
                    Dim SourceList As New List(Of String)
                    For I As Integer = 0 To 5
                        SourceList.Add("Sounds" & I)
                    Next
                    SourceCollection = SourceList.ToArray
            End Select
        End Sub

        Public Sub SyncSettings()
            SetVolume(SourceCollection, Level, Muted, Monitored, Me)
        End Sub



        Public Sub ApplySettings(Optional NewLevel As Single = -101, Optional ToggleMute As Boolean = False, Optional ToggleMon As Boolean = False)
            If NewLevel >= -100 Then Level = NewLevel
            If ToggleMute = True Then
                If Muted = False Then
                    Muted = True
                Else
                    Muted = False
                End If
            End If
            If ToggleMon = True Then
                If Monitored = False Then
                    Monitored = True
                Else
                    Monitored = False
                End If
            End If
            SetVolume(SourceCollection, Level, Muted, Monitored)
        End Sub

        Public Sub ReadLine(LineData As String)
            Dim Split1() As String = Split(LineData, "++")
            Dim Split2() As String
            For Each DataPoint As String In Split1
                Split2 = Split(DataPoint, "<>")
                Select Case Split2(0)
                    Case = "Name"
                        Name = Split2(1)
                    Case = "Muted"
                        Muted = Split2(1)
                    Case = "Monitored"
                        Monitored = Split2(1)
                    Case = "Level"
                        Level = Split2(1)
                    Case = "SourceCollection"
                        Array.ConstrainedCopy(Split2, 1, SourceCollection, 0, Split2.Length - 1)
                End Select
            Next
        End Sub

        Public Function WriteLine() As String
            Dim LineData As String = "Name" & "<>" & Name & "++Muted" & "<>" & Muted & "++Monitored" & "<>" & Monitored & "++Level" & "<>" & Level & "++SourceCollection"
            For i As Integer = 0 To SourceCollection.Length - 1
                LineData = LineData & "<>" & SourceCollection(i)
            Next
            Return LineData
        End Function

    End Class

    Public Class OBSaudioPlayer

        Public WithEvents MusicPlayer As AudioPlayer
        Public WithEvents SoundPlayer As SoundBank


        Public SongList As List(Of String)
        Public SoundList As List(Of String)
        Public TickList As List(Of String)

        Public SongIndex As Integer
        Public MusicRunning As Boolean = False

        Public Const SoundBoardDirectory As String = "\\StreamPC-V2\OBS Assets\Sounds\Sound Board"
        Public SoundBoards() As SoundButts
        Public SoundFiles() As SoundButt

        Public MyMixer As AudioChannels

        Public Event SoundBoardsUpdated()
        Public Event SoundBoardUpdated(CatIndex As Integer)
        Public Event SoundBoardButtonUpdated(CatIndex As Integer, ButtNumb As Integer)
        Public Event SoundFilesUpdated()

        Public PublicSoundListLink As String

        Private Const PublicSoundsHTML As String = "\\StreamPC-V2\OBS Assets\Web Files\jerinsfx\index.html"
        Private Const PublicSoundsWeb As String = "ftp://ftp.drawingwithjerin.com//public_html/jerinsfx/index.html"

        Public Sub New()
            ReadSoundFiles()
            ReadSoundBoards()
            MusicPlayer = New AudioPlayer("Music Player", "\\StreamPC-V2\OBS Assets\Music\")
            SoundPlayer = New SoundBank

            MyMixer = New AudioChannels
            MyMixer.ReadChannelSettings()

            SongList = BuildMusicList()
            SoundList = BuildMusicList("\\StreamPC-V2\OBS Assets\Sounds\SFX", False, True)
            TickList = BuildMusicList("\\StreamPC-V2\OBS Assets\Sounds\beepboop", False, False)

            SongIndex = 0
            PublicSoundListLink = "https://drawingwithjerin.com/jerinsfx/"
            AddHandler MusicPlayer.Stopped, AddressOf ContinueMusic
        End Sub



        'Public Async Function PlaySoundAlert(SoundFile As String) As Task

        '    Dim AlertPlayer As New Task(Of Task) _
        '   (Async Function() As Task
        '        Await PlaySoundAlertTask(SoundFile)
        '    End Function)
        '    Await MyResourceManager.RequestResource({ResourceIDs.AlertPlayer, ResourceIDs.CenterScreen}, AlertPlayer, "Sound Alert")

        'End Function

        'Private Async Function PlaySoundAlertTask(SoundFile As String) As Task
        '    SoundAlertDisplay = True
        '    Await UpdateSceneDisplay()
        '    Dim MySound As Integer = SoundPlayer.PlaySound(SoundFile, SoundSource.SFX)
        '    Await Task.Delay(3000)
        '    'Await 
        '    SoundAlertDisplay = False
        '    Await UpdateSceneDisplay()
        '    Await Task.Delay(1000)
        'End Function


        '#############################SOUND BOARD CONTROLS#############################

        Public Sub SwapSoundBoardButton(CatIndex As Integer, FromButtIndex As Integer, ToButtIndex As Integer)
            SoundBoards(CatIndex).SwapSoundBoardButton(FromButtIndex, ToButtIndex, SoundBoardDirectory)
            RaiseEvent SoundBoardButtonUpdated(CatIndex, FromButtIndex)
            RaiseEvent SoundBoardButtonUpdated(CatIndex, ToButtIndex)
        End Sub


        Public Sub RemoveSoundBoardButtonByName(SoundButtName As String)
            If SoundBoards IsNot Nothing Then
                For i As Integer = 0 To SoundBoards.Count - 1
                    For J As Integer = 0 To SoundBoards(i).Buttons.Count - 1
                        If SoundBoards(i).Buttons(J) = SoundButtName Then
                            SoundBoards(i).UpdateSoundBoardButton("", J, SoundBoardDirectory)
                            'SendMessage("Board " & i & ", Button " & J & ": Updated")
                            RaiseEvent SoundBoardButtonUpdated(i, J)
                        End If
                    Next
                Next
            End If
        End Sub

        Public Sub ChangeSoundBoardButtonName(oldButtName As String, newButtName As String)
            If SoundBoards IsNot Nothing Then
                For i As Integer = 0 To SoundBoards.Count - 1
                    For J As Integer = 0 To SoundBoards(i).Buttons.Count - 1
                        If SoundBoards(i).Buttons(J) = oldButtName Then
                            SoundBoards(i).UpdateSoundBoardButton(newButtName, J, SoundBoardDirectory)
                        End If
                    Next
                Next
            End If
        End Sub

        Public Sub RaiseSoundBoardButtonChangedEventByName(SoundButtName As String)
            If SoundBoards IsNot Nothing Then
                For i As Integer = 0 To SoundBoards.Count - 1
                    For J As Integer = 0 To SoundBoards(i).Buttons.Count - 1
                        If SoundBoards(i).Buttons(J) = SoundButtName Then
                            RaiseEvent SoundBoardButtonUpdated(i, J)
                        End If
                    Next
                Next
            End If
        End Sub

        Public Sub UpdateSoundBoardButton(CatIndex As Integer, ButtIndex As Integer, Optional ButtData As String = "")
            SoundBoards(CatIndex).UpdateSoundBoardButton(ButtData, ButtIndex, SoundBoardDirectory)
            RaiseEvent SoundBoardButtonUpdated(CatIndex, ButtIndex)
        End Sub

        Public Sub RenameSoundBoard(CategoryIndex As Integer, NewName As String)
            SoundBoards(CategoryIndex).ChangeSoundBoardName(SoundBoardDirectory, NewName)
            RaiseEvent SoundBoardUpdated(CategoryIndex)
        End Sub

        Public Sub DeleteSoundBoard(CategoryIndex As Integer)
            SoundBoards(CategoryIndex).DeleteSoundBoard(SoundBoardDirectory)
            ReadSoundBoards()
        End Sub

        Public Sub CreateNewSoundBoard()
            Dim InputString As String = InputBox("Name new Sound Board:", "CREATE NEW SOUND BOARD!")
TryAgain:
            If InputString <> "" Then
                If SoundBoards IsNot Nothing Then
                    If SoundBoards.Count <> 0 Then
                        For i As Integer = 0 To SoundBoards.Count - 1
                            If SoundBoards(i).Name = InputString Then
                                InputString = InputBox("New name must be unique!" & vbCrLf & "Name new Board:", "CREATE NEW SOUND BOARD!")
                                GoTo TryAgain
                            End If
                        Next

                    End If
                End If
                Dim SoundBoard As New SoundButts
                SoundBoard.Name = InputString
                SoundBoard.WriteSoundBoard(SoundBoardDirectory)
                ReadSoundBoards()
            End If
        End Sub

        Public Sub WriteSoundBoardData()
            If SoundBoards IsNot Nothing Then
                If SoundBoards.Length <> 0 Then
                    For i As Integer = 0 To SoundBoards.Length - 1
                        SoundBoards(i).WriteSoundBoard(SoundBoardDirectory)
                    Next
                End If
            End If
        End Sub

        Public Sub ReadSoundBoards()
            Dim di As New IO.DirectoryInfo(SoundBoardDirectory)
            Dim aryFi As IO.FileInfo() = di.GetFiles("*.csv")
            If aryFi.Count <> 0 Then
                ReDim SoundBoards(0 To aryFi.Count - 1)
                For i As Integer = 0 To aryFi.Count - 1
                    SoundBoards(i) = New SoundButts
                    SoundBoards(i).ReadSoundBoard(SoundBoardDirectory, Replace(aryFi(i).Name, ".csv", ""))
                Next
            Else
                SoundBoards = Nothing
            End If
            RaiseEvent SoundBoardsUpdated()
        End Sub

        '#############################SOUND FILE CONTROLS#############################

        Public Sub RenameSoundFile(SoundIndex As Integer, NewName As String)
            SoundFiles(SoundIndex).ChangeSoundFileName(SoundBoardDirectory, NewName)
            ChangeSoundBoardButtonName(SoundFiles(SoundIndex).Name, NewName)
            RaiseSoundBoardButtonChangedEventByName(SoundFiles(SoundIndex).Name)
            RaiseEvent SoundFilesUpdated()
        End Sub

        Public Sub DeleteSoundFile(ButtIndex As Integer)
            RemoveSoundBoardButtonByName(SoundFiles(ButtIndex).Name)
            SoundFiles(ButtIndex).DeleteSoundFile(SoundBoardDirectory)
            ReadSoundFiles()
        End Sub

        Public Sub AddSoundFile(ButtData As SoundButt)
            ButtData.WriteSoundFile(SoundBoardDirectory)
            ReadSoundFiles()
        End Sub

        Public Sub UpdateSoundFile(ButtIndex As Integer, ButtData As SoundButt)
            If SoundFiles(ButtIndex).Name <> ButtData.Name Then
                ChangeSoundBoardButtonName(SoundFiles(ButtIndex).Name, ButtData.Name)
                SoundFiles(ButtIndex).ChangeSoundFileName(SoundBoardDirectory, ButtData.Name)
            End If
            SoundFiles(ButtIndex) = ButtData
            SoundFiles(ButtIndex).WriteSoundFile(SoundBoardDirectory)
            RaiseSoundBoardButtonChangedEventByName(SoundFiles(ButtIndex).Name)
            RaiseEvent SoundFilesUpdated()
        End Sub

        Public Sub ReadSoundFiles()
            Dim di As New IO.DirectoryInfo(SoundBoardDirectory)
            Dim aryFi As IO.FileInfo() = di.GetFiles("*.txt")
            If aryFi.Count <> 0 Then
                ReDim SoundFiles(0 To aryFi.Count - 1)
                For i As Integer = 0 To aryFi.Count - 1
                    SoundFiles(i).InitializeSoundFile()
                    SoundFiles(i).ReadSoundFile(SoundBoardDirectory, Replace(aryFi(i).Name, ".txt", ""))
                Next
            Else
                SoundFiles = Nothing
            End If
            RaiseEvent SoundFilesUpdated()
        End Sub
        Public Sub WriteSoundFiles()
            If SoundFiles IsNot Nothing Then
                If SoundFiles.Length <> 0 Then
                    For i As Integer = 0 To SoundFiles.Length - 1
                        SoundFiles(i).WriteSoundFile(SoundBoardDirectory)
                    Next
                End If
            End If
        End Sub


        Public Function CheckSoundFileNames(NameSuggestion As String) As Boolean
            If SoundFiles IsNot Nothing Then
                If SoundFiles.Length <> 0 Then
                    For I As Integer = 0 To SoundFiles.Length - 1
                        'For J As Integer = 0 To SoundBoards(I).Buttons.Length - 1
                        If SoundFiles(I).Name = NameSuggestion Then
                            Return False
                            Exit Function
                        End If
                        'Next
                    Next
                End If
            End If
            Return True
        End Function

        Public Function GetSoundFileDataByName(SoundButtonName As String, Optional FullName As Boolean = False) As String
            Dim SoundFile As String = ""
            If SoundFiles IsNot Nothing Then
                If SoundFiles.Length <> 0 Then
                    For I As Integer = 0 To SoundFiles.Length - 1
                        'For J As Integer = 0 To SoundBoards(I).Buttons.Length - 1
                        If SoundFiles(I).Name = SoundButtonName Then
                            SoundFile = SoundFiles(I).GetSoundFile
                            GoTo FoundIt
                        End If
                        'Next
                    Next
                End If
            End If
FoundIt:
            If FullName = True Then
                SoundFile = "\\StreamPC-V2\OBS Assets\Sounds\SFX\" & SoundFile
            End If
            Return SoundFile
        End Function

        Public Function GetSoundFileIndexByName(SoundButtonName As String) As Integer
            Dim Output As Integer = -1
            If SoundFiles IsNot Nothing Then
                If SoundFiles.Length <> 0 Then
                    For I As Integer = 0 To SoundFiles.Length - 1
                        'For J As Integer = 0 To SoundBoards(I).Buttons.Length - 1
                        If SoundFiles(I).Name = SoundButtonName Then
                            Output = I
                            GoTo FoundIt
                        End If
                        'Next
                    Next
                End If
            End If
FoundIt:
            Return Output
        End Function

        Public Function GetRandomPublicSoundFile() As String
            Dim SoundList As List(Of String) = GetPublicSoundFilesList()
            Return GetSoundFileDataByName(SoundList(RandomInt(0, SoundList.Count - 1)))
        End Function

        Public Function FilterSoundFilesByUnused() As List(Of String)
            Dim Outputlist As New List(Of String)
            Outputlist.AddRange(SoundList)
            If SoundFiles IsNot Nothing Then
                If SoundFiles.Length <> 0 Then
                    For I As Integer = 0 To SoundFiles.Length - 1
                        'For J As Integer = 0 To SoundBoards(I).Buttons.Length - 1
                        If SoundFiles(I).Name <> "" Then
                            If SoundFiles(I).Sounds IsNot Nothing Then
                                If SoundFiles(I).Sounds.Count <> 0 Then
                                    For K As Integer = 0 To SoundFiles(I).Sounds.Count - 1
                                        If Outputlist.Contains(SoundFiles(I).Sounds(K)) Then
                                            Outputlist.Remove(SoundFiles(I).Sounds(K))
                                        End If
                                    Next
                                End If
                            End If
                        End If
                        'Next
                    Next
                End If
            End If
            Return Outputlist
        End Function

        Public Function GetFullSoundFilesList() As List(Of String)
            Dim OutputList As New List(Of String)
            If SoundFiles IsNot Nothing Then
                If SoundFiles.Length <> 0 Then
                    For I As Integer = 0 To SoundFiles.Length - 1
                        'For J As Integer = 0 To SoundBoards(I).Buttons.Length - 1
                        If SoundFiles(I).Name <> "" Then
                            If SoundFiles(I).Sounds IsNot Nothing Then
                                If SoundFiles(I).Sounds.Count <> 0 Then
                                    OutputList.Add(SoundFiles(I).Name)
                                End If
                            End If
                        End If
                        'Next
                    Next
                End If
            End If
            OutputList.Sort()
            Return OutputList
        End Function

        Public Function GetPublicSoundFilesList() As List(Of String)
            Dim OutputList As New List(Of String)
            If SoundFiles IsNot Nothing Then
                If SoundFiles.Length <> 0 Then
                    For I As Integer = 0 To SoundFiles.Length - 1
                        If SoundFiles(I).PublicBool = True Then
                            If SoundFiles(I).Name <> "" Then
                                If SoundFiles(I).Sounds IsNot Nothing Then
                                    If SoundFiles(I).Sounds.Count <> 0 Then
                                        OutputList.Add(SoundFiles(I).Name)
                                    End If
                                End If
                            End If
                        End If
                    Next
                End If
            End If
            OutputList.Sort()
            Return OutputList
        End Function

        Public Function BuildPublicSoundFilesList() As String
            Dim Outputstring As String = ""
            If SoundFiles IsNot Nothing Then
                If SoundFiles.Length <> 0 Then
                    For I As Integer = 0 To SoundFiles.Length - 1
                        'For J As Integer = 0 To SoundBoards(I).Buttons.Length - 1
                        If SoundFiles(I).PublicBool = True Then
                            If SoundFiles(I).Name <> "" Then
                                If SoundFiles(I).Sounds IsNot Nothing Then
                                    If SoundFiles(I).Sounds.Count <> 0 Then
                                        Outputstring = Outputstring &
                                                "<span class=""sfxpreview""><a target=""_blank"" title=""Preview Sound"" href=""" &
                                                SoundFiles(I).Sounds(0) &
                                                """><i class=""fas fa-volume-up""></i></a></span>&nbsp;" &
                                                Replace(SoundFiles(I).Name, " ", "_") & "<br>" & vbCrLf
                                    End If
                                End If
                            End If
                        End If
                        'Next
                    Next
                End If
            End If
            Return Outputstring
        End Function

        Public Sub UpdatePublicSoundFileIndex()

            Dim inputstring As String = File.ReadAllText(PublicSoundsHTML)

            Dim OutputString As String = ""

            OutputString = OutputString & "<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01//EN"" ""http://www.w3.org/TR/html4/strict.dtd"">" & vbCrLf
            OutputString = OutputString & "<html lang=""en"">" & vbCrLf
            OutputString = OutputString & "<head>" & vbCrLf
            OutputString = OutputString & "<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"">" & vbCrLf
            OutputString = OutputString & "<title>Jerinhaus - Twitch SFX</title>" & vbCrLf
            OutputString = OutputString & "<link rel=""stylesheet"" type=""text/css"" href=""style.css"" media=""screen"">" & vbCrLf
            OutputString = OutputString & "<link rel=""stylesheet"" href=""https://use.fontawesome.com/releases/v5.6.3/css/all.css"" integrity=""sha384-UHRtZLI+pbxtHCWp1t77Bi1L4ZtiqrqD80Kn4Z8NTSRyMA2Fd33n5dQ8lWUE00s/"" crossorigin=""anonymous"">" & vbCrLf
            OutputString = OutputString & "<link rel=""icon"" href=""favicon.ico"" type=""image/x-icon"">" & vbCrLf
            OutputString = OutputString & "</head>" & vbCrLf
            OutputString = OutputString & "<body>" & vbCrLf
            OutputString = OutputString & "<div class=""contentbox"">" & vbCrLf
            OutputString = OutputString & "<h1><a href=""https://drawingwithjerin.com/"">jerinhaus</a></h1>" & vbCrLf
            OutputString = OutputString & "<h2><span class=""green"">twitch sfx directory</span></h2>" & vbCrLf
            OutputString = OutputString & "<p>To use the sound alert redeem, copy and paste the sound name from the list below into the twitch chat window!</p>" & vbCrLf
            OutputString = OutputString & "<p>" & vbCrLf
            OutputString = OutputString & BuildPublicSoundFilesList()
            OutputString = OutputString & "</p>" & vbCrLf
            OutputString = OutputString & "</div>" & vbCrLf
            OutputString = OutputString & "</body>" & vbCrLf
            OutputString = OutputString & "</html>" & vbCrLf

            If OutputString <> inputstring Then
                File.WriteAllText(PublicSoundsHTML, OutputString)

                Dim clsRequest As System.Net.FtpWebRequest =
                DirectCast(System.Net.WebRequest.Create(PublicSoundsWeb), System.Net.FtpWebRequest)

                clsRequest.Credentials = New System.Net.NetworkCredential(WebUsername, WebPword)
                clsRequest.Method = System.Net.WebRequestMethods.Ftp.UploadFile
                Dim bFile() As Byte = System.IO.File.ReadAllBytes(PublicSoundsHTML)
                Dim clsStream As System.IO.Stream = clsRequest.GetRequestStream()
                clsStream.Write(bFile, 0, bFile.Length)
                clsStream.Close()
                clsStream.Dispose()
            End If
        End Sub

        Public Sub PlayMusic(Optional Replay As Boolean = False)
            If MusicPlayer.isActive = False Or Replay = True Then
                If SongList(SongIndex) <> "" Then
                    MusicPlayer.Play(SongList(SongIndex))
                    MusicRunning = True
                End If
            End If
        End Sub

        Public Sub StopMusic()
            If MusicPlayer.isActive = True Then
                MusicRunning = False
                MusicPlayer.Stopp()
            End If
        End Sub

        Public Sub PauseMusic()
            If MusicPlayer.isActive = True Then
                MusicPlayer.Pausing()
            End If
        End Sub

        Public Sub SkipMusicF()
            If SongIndex > SongList.Count - 2 Then
                SongIndex = 0
            Else
                SongIndex = SongIndex + 1
            End If

            If SongList(SongIndex) <> "" Then
                'SendMessage(SongList(SongIndex))
                MusicPlayer.Play(SongList(SongIndex))
                MusicRunning = True
            End If
        End Sub

        Public Sub SkipMusicB()
            If SongIndex > 0 Then
                SongIndex = SongIndex - 1
            Else
                SongIndex = SongList.Count - 1
            End If
            If SongList(SongIndex) <> "" Then
                'SendMessage(SongList(SongIndex))
                MusicPlayer.Play(SongList(SongIndex))
                MusicRunning = True
            End If
        End Sub

        Private Sub ContinueMusic()
            Dim PlayTask As Task = ContinueMusicAsync()
        End Sub

        Private Async Function ContinueMusicAsync() As Task
            Await Task.Delay(100)
            If MusicRunning = True Then
                If SongIndex > SongList.Count - 2 Then
                    SongIndex = 0
                Else
                    SongIndex = SongIndex + 1
                End If
                PlayMusic()
            End If
        End Function

    End Class


    Public Structure SoundSource
        Public Const SFX As String = "\\StreamPC-V2\OBS Assets\Sounds\SFX\"
        Public Const BeepBoop As String = "\\StreamPC-V2\OBS Assets\Sounds\beepboop\"
    End Structure


    Public Class SoundBank
        Private SoundPlayers() As AudioPlayer
        Private Access As Mutex
        Public Event SoundPlayed(EventString As String)
        Public SoundTasks() As Task
        Private SoundBools() As Boolean
        Public Sub New()
            Access = New Mutex
            Access.WaitOne()
            ReDim SoundPlayers(0 To 5)
            ReDim SoundTasks(0 To 5)
            ReDim SoundBools(0 To 5)
            For I As Integer = 0 To 5
                SoundPlayers(I) = New AudioPlayer("Sounds" & I, SoundSource.SFX)
            Next
            Access.ReleaseMutex()
        End Sub

        Public Function PlaySound(SoundFile As String, Path As String, Optional Source As String = "") As Integer
            Access.WaitOne()
            Dim SoundIndex As Integer = -1
            For I As Integer = 0 To 5
                If SoundBools(I) = False Then
                    SoundIndex = I
                    GoTo FoundPlayer
                End If
            Next
FoundPlayer:
            If SoundIndex > -1 Then
                SoundPlayers(SoundIndex).ChangePath(Path)
                SoundTasks(SoundIndex) = Play(SoundIndex, SoundFile)
                If Source <> "" Then
                    RaiseEvent SoundPlayed(Source & " Played " & SoundFile & " (SB_INDEX# " & SoundIndex & ")")
                Else
                    RaiseEvent SoundPlayed("Played " & SoundFile & " (SB_INDEX# " & SoundIndex & ")")
                End If
            Else
                If Source <> "" Then
                    RaiseEvent SoundPlayed(Source & " Skipped " & SoundFile & " (No Outputs Available!?!)")
                Else
                    RaiseEvent SoundPlayed("Skipped " & SoundFile & " (No Outputs Available!?!)")
                End If
            End If
            Access.ReleaseMutex()
            Return SoundIndex
        End Function

        Private Async Function Play(SoundIndex As Integer, SoundFile As String) As Task
            SoundBools(SoundIndex) = True
            Await SoundPlayers(SoundIndex).PlayAsync(SoundFile)
            'SendMessage(SoundFile & " " & SoundIndex)
            SoundBools(SoundIndex) = False
        End Function

        Public Sub StopSounds(Optional SoundIndex() As Integer = Nothing)
            If SoundIndex Is Nothing Then
                For I As Integer = 0 To SoundBools.Length - 1
                    If SoundBools(I) Then
                        SoundPlayers(SoundIndex(I)).Stopp()
                    End If
                Next
            Else
                For I As Integer = 0 To SoundIndex.Length - 1
                    If SoundBools(SoundIndex(I)) Then
                        SoundPlayers(SoundIndex(I)).Stopp()
                    End If
                Next
            End If
        End Sub

    End Class

    Public Class SoundController

        Private MySound As Integer
        Private Path As String
        Private Controller As String
        Private MyMutex As Mutex
        Private WaitForStop As Boolean
        Private Stopped As TaskCompletionSource(Of Boolean)
        Public Event SoundStarted()
        Public Event SoundStopped()
        'Private StoppingSound As Boolean
        'Private SoundStopped As TaskCompletionSource(Of Boolean)

        Public Sub New(Source As String, Optional FormName As String = "")
            MySound = -1
            Path = Source
            Controller = FormName
            MyMutex = New Mutex
        End Sub

        Public Async Function StopSound() As Task
            If MySound > -1 Then
                MyMutex.WaitOne()
                AudioControl.SoundPlayer.StopSounds({MySound})
                WaitForStop = True
                Stopped = New TaskCompletionSource(Of Boolean)
                MyMutex.ReleaseMutex()
                WaitForStop = Await Stopped.Task
            End If
        End Function

        Public Async Function PlaySound(SoundFile As String) As Task
            If MySound > -1 Then
                Await StopSound()
                Await Task.Delay(10)
            End If
            Await PlaySoundFile(SoundFile)
        End Function

        Public Function IsActive() As Boolean
            Return MySound > -1
        End Function

        Private Async Function PlaySoundFile(SoundFile As String) As Task
            MySound = AudioControl.SoundPlayer.PlaySound(SoundFile, Path, Controller)
            If MySound > -1 Then
                RaiseEvent SoundStarted()
                Await AudioControl.SoundPlayer.SoundTasks(MySound)
                RaiseEvent SoundStopped()
                MyMutex.WaitOne()
                MySound = -1
                If WaitForStop Then Stopped.SetResult(False)
                MyMutex.ReleaseMutex()
            End If
        End Function

    End Class

    Public Class SoundButts

        Public Name As String
        Public Buttons() As String
        Public Const ButtonCount As Integer = 36
        Public Access As Mutex

        Public Sub New()
            Init()
        End Sub

        Public Sub Init()
            If Access Is Nothing Then Access = New Mutex
            Access.WaitOne()
            Name = ""
            ReDim Buttons(0 To ButtonCount - 1)
            For I As Integer = 0 To ButtonCount - 1
                Buttons(I) = ""
            Next
            Access.ReleaseMutex()
        End Sub

        Public Sub UpdateSoundBoardButton(ButtName As String, ButtIndex As Integer, SoundDirectory As String)
            'Dim InputString As String = "(" & ButtIndex & ")Before: " & Buttons(ButtIndex) & ", & (" & ButtIndex & ")After: "
            Buttons(ButtIndex) = ButtName
            'InputString = InputString & Buttons(ButtIndex)
            'SendMessage(InputString)
            WriteSoundBoard(SoundDirectory)
        End Sub

        Public Sub SwapSoundBoardButton(FromButt As Integer, ToButt As Integer, SoundDirectory As String)
            Dim InputButt As String = Buttons(FromButt)
            Buttons(FromButt) = Buttons(ToButt)
            Buttons(ToButt) = InputButt
            WriteSoundBoard(SoundDirectory)
        End Sub

        Public Sub ChangeSoundBoardName(SoundDirectory As String, NewName As String)
            If Name <> "" Then
                If File.Exists(SoundDirectory & "\" & Name & ".csv") Then
                    Rename(SoundDirectory & "\" & Name, SoundDirectory & "\" & NewName & ".csv")
                    Name = NewName
                End If
            End If
        End Sub

        Public Sub ReadSoundBoard(SoundDirectory As String, Optional InputCategoryName As String = "")
            For I As Integer = 0 To ButtonCount - 1
                Buttons(I) = ""
            Next
            If InputCategoryName <> "" Then
                Name = InputCategoryName
            End If
            If Name <> "" Then
                If Access Is Nothing Then Access = New Mutex
                Access.WaitOne()
                If Directory.Exists(SoundDirectory) Then
                    Dim SplitString() As String =
                    Split(File.ReadAllText(SoundDirectory & "\" & Name & ".csv"), ",")
                    For i As Integer = 0 To SplitString.Count - 1
                        Buttons(i) = SplitString(i)
                    Next
                End If
                Access.ReleaseMutex()
            End If
        End Sub

        Public Sub WriteSoundBoard(SoundDirectory As String)
            If Name <> "" Then
                If Access Is Nothing Then Access = New Mutex
                Access.WaitOne()
                File.WriteAllText(SoundDirectory & "\" & Name & ".csv", Join(Buttons, ","))
                'SendMessage(Join(Buttons, ","))
                Access.ReleaseMutex()
            End If
        End Sub

        Public Sub DeleteSoundBoard(SoundDirectory As String)
            If Name <> "" Then
                If Access Is Nothing Then Access = New Mutex
                Access.WaitOne()
                File.Delete(SoundDirectory & "\" & Name & ".csv")
                Access.ReleaseMutex()
                Init()
            End If
        End Sub
    End Class


    Public Structure SoundButt
        Public Name As String
        Public Title As String
        Public SoundColor As Color
        Public Image As String
        Public PublicBool As Boolean
        Public MuteMusic As Boolean
        Public Randomize As Boolean
        Public Sounds As List(Of String)
        Private Index As Integer
        Public Active As Boolean
        Public Access As Mutex

        Public Function GetSoundFile() As String
            If Access Is Nothing Then Access = New Mutex
            Access.WaitOne()
            Dim ReturnString As String = ""
            If Sounds.Count <> 0 Then
                If Sounds.Count > 1 Then
                    If Randomize = True Then
                        Index = RandomInt(0, Sounds.Count - 1)
                    Else
                        If Index >= (Sounds.Count - 1) Then
                            Index = 0
                        Else
                            Index = Index + 1
                        End If
                    End If
                    ReturnString = Sounds(Index)
                Else
                    ReturnString = Sounds(0)
                End If
            End If
            Access.ReleaseMutex()
            Return ReturnString
        End Function

        Public Sub InitializeSoundFile()
            If Access Is Nothing Then Access = New Mutex
            Access.WaitOne()
            Name = ""
            Title = ""
            SoundColor = StandardBUTT
            Image = ""
            PublicBool = False
            MuteMusic = False
            Randomize = False
            Index = 0
            Sounds = New List(Of String)
            Active = False
            Access.ReleaseMutex()
        End Sub

        Public Sub DeleteSoundFile(SoundDirectory As String)
            If Name <> "" Then
                If Access Is Nothing Then Access = New Mutex
                Access.WaitOne()
                If File.Exists(SoundDirectory & "\" & Name & ".txt") Then
                    File.Delete(SoundDirectory & "\" & Name & ".txt")
                End If
                Access.ReleaseMutex()
                InitializeSoundFile()
            End If
        End Sub

        Public Sub ChangeSoundFileName(SoundDirectory As String, NewName As String)
            If Access Is Nothing Then Access = New Mutex
            Access.WaitOne()
            If Name <> "" Then
                If File.Exists(SoundDirectory & "\" & Name & ".txt") Then
                    Rename(SoundDirectory & "\" & Name & ".txt", SoundDirectory & NewName & ".txt")
                End If
            End If
            Name = NewName
            Access.ReleaseMutex()
        End Sub

        Public Sub WriteSoundFile(SoundDirectory As String)
            If Name <> "" Then
                If Access Is Nothing Then Access = New Mutex
                Access.WaitOne()
                Dim OutputString As String = ""
                OutputString = OutputString & "Title: " & Title & vbCrLf
                OutputString = OutputString & "Color: " & ColorTranslator.ToHtml(SoundColor) & vbCrLf
                OutputString = OutputString & "Image: " & Image & vbCrLf
                OutputString = OutputString & "PublicBool: " & PublicBool & vbCrLf
                OutputString = OutputString & "MuteMusic: " & MuteMusic & vbCrLf
                OutputString = OutputString & "Randomize: " & Randomize & vbCrLf
                OutputString = OutputString & "Index: " & Index & vbCrLf
                If Sounds.Count <> 0 Then
                    OutputString = OutputString & "Sounds: " & String.Join(",", Sounds)
                End If
                File.WriteAllText(SoundDirectory & "\" & Name & ".txt", OutputString)
                Access.ReleaseMutex()
            End If
        End Sub

        Public Sub ReadSoundFile(SoundDirectory As String, Optional NameString As String = "")
            If Access Is Nothing Then Access = New Mutex
            Access.WaitOne()
            If NameString <> "" Then
                Name = NameString
            End If
            If Name <> "" Then
                If File.Exists(SoundDirectory & "\" & Name & ".txt") Then
                    Dim InputStream As StreamReader = File.OpenText(SoundDirectory & "\" & Name & ".txt")
                    Do Until InputStream.EndOfStream = True
                        ReadSoundElement(InputStream.ReadLine())
                    Loop
                    InputStream.Close()
                    InputStream.Dispose()
                End If
            End If
            Access.ReleaseMutex()
        End Sub

        Private Sub ReadSoundElement(InputString As String)
            Dim SplitString() As String
            SplitString = Split(InputString, ": ")
            If SplitString.Length > 1 Then
                Select Case SplitString(0)
                    Case = "Title"
                        Title = SplitString(1)
                    Case = "Color"
                        SoundColor = ColorTranslator.FromHtml(SplitString(1))
                    Case = "Image"
                        Image = SplitString(1)
                    Case = "PublicBool"
                        PublicBool = SplitString(1)
                    Case = "MuteMusic"
                        MuteMusic = SplitString(1)
                    Case = "Randomize"
                        Randomize = SplitString(1)
                    Case = "Index"
                        Index = SplitString(1)
                    Case = "Sounds"
                        Dim SoundsString() As String = SplitString(1).Split(",")
                        If SoundsString.Length <> 0 Then
                            Sounds = New List(Of String)
                            For i As Integer = 0 To SoundsString.Length - 1
                                Sounds.Add(SoundsString(i))
                            Next
                        End If
                End Select
            End If
        End Sub

    End Structure

    Public Class AudioPlayer

        Public Name As String
        Private Path As String
        Private Active As Boolean
        Private IsStarted As Boolean
        Private Pause As Boolean
        Private Access As Mutex
        Public Current As String
        Private AsyncTrigg As Boolean
        Private KeepFile As Boolean = False
        Private AsyncSigg As TaskCompletionSource(Of Boolean)
        Public Event Started()
        Public Event Stopped()
        Public Event Paused()

        Public Sub New(SourceName As String, Optional SourcePath As String = "", Optional IKeepFile As Boolean = False)
            Access = New Mutex
            Access.WaitOne()
            Name = SourceName
            Path = SourcePath
            Active = False

            Pause = False
            Access.ReleaseMutex()
            AsyncTrigg = False
            KeepFile = IKeepFile
            AddHandler OBS.MediaInputPlaybackStarted, AddressOf MediaStartTriggered
            AddHandler OBS.MediaInputPlaybackEnded, AddressOf mediastopped
            AddHandler OBS.MediaInputActionTriggered, AddressOf MediaTriggered
            'SyncState()
        End Sub

        Public Function isActive() As Boolean
            Access.WaitOne()
            Dim Output As Boolean = Active
            Access.ReleaseMutex()
            Return Output
        End Function
        Public Function isPaused() As Boolean
            Access.WaitOne()
            Dim Output As Boolean = Pause
            Access.ReleaseMutex()
            Return Output
        End Function

        Public Sub ChangePath(NewPath As String)
            Access.WaitOne()
            Path = NewPath
            Access.ReleaseMutex()
        End Sub

        Public Sub SyncState()

            OBSmutex.WaitOne()
            Dim InputState As String = OBS.GetMediaInputStatus(Name).State
            If InputState <> "" Then
                SendMessage(InputState)
                Select Case InputState
                    Case MediaActions.Playing, MediaActions.Openning, MediaActions.Bufferring
                        Access.WaitOne()
                        Active = True
                        Pause = False
                        Access.ReleaseMutex()
                    Case MediaActions.Paused
                        'OBSmutex.ReleaseMutex()
                        Access.WaitOne()
                        Active = True
                        Pause = True
                        Access.ReleaseMutex()
                    Case Else
                        'OBSmutex.ReleaseMutex()
                        Access.WaitOne()
                        Active = False
                        Pause = False
                        Access.ReleaseMutex()
                End Select
            Else
                Active = False
                Pause = False
            End If
            OBSmutex.ReleaseMutex()
        End Sub

        Private Sub MediaTriggered(Sender As Object, E As Events.MediaInputActionTriggeredEventArgs)
            If E.InputName = Name Then
                Access.WaitOne()
                Select Case E.MediaAction
                    Case MediaActions.Restart
                        If IsStarted = False Then
                            mediastarted()
                        End If
                        Access.ReleaseMutex()
                        RaiseEvent Started()
                        Exit Sub
                    Case MediaActions.Play
                        mediastarted()
                        Access.ReleaseMutex()
                        RaiseEvent Started()
                        Exit Sub
                    Case MediaActions.Pause
                        Pause = True
                        Access.ReleaseMutex()
                        RaiseEvent Paused()
                        Exit Sub
                End Select
                Access.ReleaseMutex()
            End If
        End Sub
        Private Sub mediastopped(Sender As Object, E As Events.MediaInputPlaybackEndedEventArgs)
            If E.InputName = Name Then
                Dim StopTask As Task = StopMe()
            End If
        End Sub
        Private Sub MediaStartTriggered(Sender As Object, E As Events.MediaInputPlaybackStartedEventArgs)
            If E.InputName = Name Then
                Access.WaitOne()
                If IsStarted = False Then mediastarted()
                Access.ReleaseMutex()
            End If
        End Sub

        Private Sub mediastarted()
            IsStarted = True
            Pause = False
            Active = True
        End Sub

        Public Sub Stopp()
            Access.WaitOne()
            If Active = True Then
                If Path <> "" Then
                    Current = Replace(MediaSourceChange(Name, 3), Path, "")
                Else
                    Current = MediaSourceChange(Name, 3)
                End If
            End If
            Access.ReleaseMutex()
        End Sub

        Public Sub Pausing()
            Access.WaitOne()
            If Pause = True Then
                If Path <> "" Then
                    Current = Replace(MediaSourceChange(Name, 1), Path, "")
                Else
                    Current = MediaSourceChange(Name, 1)
                End If
            Else
                If Path <> "" Then
                    Current = Replace(MediaSourceChange(Name, 2), Path, "")
                Else
                    Current = MediaSourceChange(Name, 2)
                End If
            End If
            Access.ReleaseMutex()
        End Sub

        Private Async Function WaitForStart() As Task(Of Boolean)
            Dim WaitCount As Integer
            Do Until IsStarted = True
                Await Task.Delay(2)
                WaitCount = WaitCount + 2
                If WaitCount > 100 Then
                    AsyncTrigg = False
                    Await StopMe()
                    Return False
                End If
            Loop
            Return True
        End Function

        Public Async Function PlayAsync(FileName As String) As Task
            Access.WaitOne()
            Active = True
            AsyncTrigg = True
            AsyncSigg = New TaskCompletionSource(Of Boolean)
            If Path <> "" Then
                Current = Replace(MediaSourceChange(Name, , Path & FileName), Path, "")
            Else
                Current = MediaSourceChange(Name, , FileName)
            End If
            Access.ReleaseMutex()
            If Await WaitForStart() = True Then
                AsyncTrigg = Await AsyncSigg.Task
                Await Task.Delay(5)
                Dim StopTask As Task = StopMe()
            End If
        End Function

        Private Async Function StopMe() As Task
            Dim ImDone As New TaskCompletionSource(Of Boolean)
            Dim StopThread As New Thread(
            Sub()
                Access.WaitOne()
                If AsyncTrigg = True Then
                    AsyncSigg.SetResult(False)
                    Thread.Sleep(5)
                Else
                    If KeepFile = False Then ResetMediaSource(Name)
                    Thread.Sleep(25)
                    Pause = False
                    Active = False
                    IsStarted = False
                End If
                Access.ReleaseMutex()
                RaiseEvent Stopped()
            End Sub)
            StopThread.Start()
            Await ImDone.Task
        End Function

        Public Sub Play(FileName As String)
            Access.WaitOne()
            If Active = False Then
                If Path <> "" Then
                    Current = Replace(MediaSourceChange(Name, , Path & FileName), Path, "")
                Else
                    Current = MediaSourceChange(Name, , FileName)
                End If
            End If
            Access.ReleaseMutex()
        End Sub

    End Class

    '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    '/////////////////////////////////////////////////////MEDIA PLAYERS///////////////////////////////////////////////////////
    '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    Public Class VideoPlayer
        Private WithEvents Player As AudioPlayer
        Private Audio As SoundController
        Private SceneName As String
        Private FileList() As String
        Private LoopDuration As Integer
        Private HoldDuration As Integer
        Private EndMedia As TaskCompletionSource(Of Boolean)
        Private MediaActive As Boolean = False
        Private SoundFile As String
        Private GroupName As String
        Private StayVisible As Boolean

        Public Sub New(SourceName As String, Scene As String,
                       Optional Sound As String = "",
                       Optional InputFileList() As String = Nothing,
                       Optional LoopTime As Integer = 0,
                       Optional HoldTime As Integer = False,
                       Optional InfiniteLoop As Boolean = False,
                       Optional Group As String = "",
                       Optional SFXsource As String = "")

            Player = New AudioPlayer(SourceName, , True)
            GroupName = Group

            If SFXsource = "" Then
                Audio = New SoundController(SoundSource.SFX, SourceName)
            Else
                Audio = New SoundController(SFXsource, SourceName)
            End If

            'AddHandler Player.Stopped, AddressOf Hide
            If InputFileList IsNot Nothing Then
                FileList = InputFileList
            Else
                FileList = {}
            End If
            SceneName = Scene
            SoundFile = Sound


            'If InfiniteLoop Then
            '    StayVisible = True
            '    LoopDuration = 0
            '    HoldDuration = 0
            'Else
            StayVisible = InfiniteLoop
            LoopDuration = LoopTime
            HoldDuration = HoldTime
            'End If

            MediaActive = False
            'SetSourceVisibility(Player.Name, SceneName, False, True)
        End Sub

        Public Sub Hide() Handles Player.Stopped
            Dim StopThread As New Thread(
                Sub()
                    If GroupName = "" Then
                        SetSourceVisibility(0, SceneName, False, True, Player.Name)
                    Else
                        SetSourceVisibility(0, SceneName, False, True, GroupName)
                    End If
                    If Audio.IsActive Then Dim StopTask As Task = Audio.StopSound()
                    If MediaActive = True Then EndMedia.SetResult(False)
                End Sub)
            StopThread.Start()
        End Sub

        Private Sub VideoStarted() Handles Player.Started
            AmStarted = True
        End Sub

        Private AmStarted As Boolean = False

        Private Async Function waitforstart() As Task
            AmStarted = False
            Await Task.Delay(250)
            If AmStarted = False Then
                Hide()
            End If
        End Function

        Public Function IsActive() As Boolean
            Return MediaActive
        End Function
        Public Async Function Show(Optional FileIndex As Integer = -1,
                                   Optional CustomSound As String = "",
                                   Optional WaitForSound As Boolean = False,
                                   Optional CustomLoop As Integer = -1,
                                   Optional CustomHold As Integer = -1) As Task

            If MediaActive = False Then
                Dim EndMe As Boolean = False
                MediaActive = True
                EndMedia = New TaskCompletionSource(Of Boolean)
                Dim SoundTask As Task
                If FileIndex > -1 And FileIndex < FileList.Length Then
                    MediaSourceChange(Player.Name, , FileList(FileIndex))
                End If
                Dim StartWait As Task = waitforstart()
                If GroupName = "" Then
                    SetSourceVisibility(0, SceneName, True, True, Player.Name)
                Else
                    SetSourceVisibility(0, SceneName, True, True, GroupName)
                End If
                If CustomSound <> "" Or SoundFile <> "" Then
                    If CustomSound = "" Then
                        SoundTask = Audio.PlaySound(SoundFile)
                    Else
                        SoundTask = Audio.PlaySound(CustomSound)
                    End If
                End If
                'If StayVisible = False Then
                If LoopDuration > 0 Or CustomLoop > -1 Then
                    EndMe = True
                    If CustomLoop > -1 Then
                        Await Task.Delay(CustomLoop)
                    Else
                        Await Task.Delay(LoopDuration)
                    End If
                End If
                If HoldDuration > 0 Or CustomHold > -1 Then
                    EndMe = True
                    Player.Pausing()
                    'SendMessage(Player.Name & " Paused")
                    If CustomHold > -1 Then
                        Await Task.Delay(CustomHold)
                    Else
                        Await Task.Delay(HoldDuration)
                    End If
                    If StayVisible = False Then
                        Player.Pausing()
                        EndMe = False
                    End If
                    'SendMessage(Player.Name & " Resumed")
                End If
                If WaitForSound = True Then EndMe = True
                'End If
                If EndMe Then
                    If WaitForSound = True And SoundTask IsNot Nothing Then
                        'If SoundTask.Status = TaskStatus.Running Then
                        Await SoundTask
                        'End If
                    End If
                    MediaActive = False
                    If GroupName = "" Then
                        SetSourceVisibility(0, SceneName, False, True, Player.Name)
                    Else
                        SetSourceVisibility(0, SceneName, False, True, GroupName)
                    End If
                Else
                    If StayVisible = False Then
                        MediaActive = Await EndMedia.Task
                    End If
                End If
            End If
        End Function
    End Class

    Public Structure UserEvents
        Public Const NewFollower As Integer = 0
        Public Const NewSubscriber As Integer = 1
        Public Const NewDonation As Integer = 2
        Public Const BitsDetected As Integer = 3
        Public Const RaidDetected As Integer = 4
        Public Const MaxEvents As Integer = 4
    End Structure

    Public Class OBSevents
        Private Hats As VideoPlayer
        Private SoundAlert As VideoPlayer
        Private UserAlert As VideoPlayer
        Private Confetti As VideoPlayer
        Private Rollers() As VideoPlayer
        Private doublekill As VideoPlayer
        Private OHBABYTRIPPLE As VideoPlayer

        Public Sub New()
            Dim UserEventVidSources(0 To UserEvents.MaxEvents) As String
            UserEventVidSources(UserEvents.NewFollower) = "\\StreamPC-V2\OBS Assets\Video\New Follower.mp4"
            UserEventVidSources(UserEvents.NewSubscriber) = "\\StreamPC-V2\OBS Assets\Video\New Subscriber.mp4"
            UserEventVidSources(UserEvents.NewDonation) = "\\StreamPC-V2\OBS Assets\Video\New Follower.mp4"
            UserEventVidSources(UserEvents.BitsDetected) = "\\StreamPC-V2\OBS Assets\Video\New Follower.mp4"
            UserEventVidSources(UserEvents.RaidDetected) = "\\StreamPC-V2\OBS Assets\Video\New Follower.mp4"

            Dim Roll6vidSources(0 To 5) As String
            Roll6vidSources(0) = "\\StreamPC-V2\OBS Assets\Video\Roll6\rolldiamond.mp4"
            Roll6vidSources(1) = "\\StreamPC-V2\OBS Assets\Video\Roll6\rollclub.mp4"
            Roll6vidSources(2) = "\\StreamPC-V2\OBS Assets\Video\Roll6\rollheart.mp4"
            Roll6vidSources(3) = "\\StreamPC-V2\OBS Assets\Video\Roll6\rollspade.mp4"
            Roll6vidSources(4) = "\\StreamPC-V2\OBS Assets\Video\Roll6\rollflame.mp4"
            Roll6vidSources(5) = "\\StreamPC-V2\OBS Assets\Video\Roll6\rollmoon.mp4"

            Dim ConfettiSources(0 To 4) As String
            ConfettiSources(0) = "\\StreamPC-V2\OBS Assets\Media\confetti1.mp4"
            ConfettiSources(1) = "\\StreamPC-V2\OBS Assets\Media\confetti2.mp4"
            ConfettiSources(2) = "\\StreamPC-V2\OBS Assets\Media\confetti3.mp4"
            ConfettiSources(3) = "\\StreamPC-V2\OBS Assets\Media\confetti5.mp4"
            ConfettiSources(4) = "\\StreamPC-V2\OBS Assets\Media\confetti7.mp4"

            ReDim Rollers(0 To 4)
            For I As Integer = 0 To 4
                Rollers(I) = New VideoPlayer("Roll" & I + 1, "Roll For Prizes", "rollsound1.wav", Roll6vidSources, 6000, 3000)
            Next

            Hats = New VideoPlayer("EmbersHats", "Events and Alerts", "Hats.wav")
            SoundAlert = New VideoPlayer("Sound Alert Diplay", "Events and Alerts",,, 4000,,, "SoundAlert")
            UserAlert = New VideoPlayer("Viewer Generated Event", "Events and Alerts",, UserEventVidSources)
            Confetti = New VideoPlayer("ConfettiBlast", "Events and Alerts", "Yay.wav", ConfettiSources)
            doublekill = New VideoPlayer("double", "Events and Alerts", "DoubleKilll.wav")
            OHBABYTRIPPLE = New VideoPlayer("triple", "Events and Alerts", "OH BABY A TRIPLE.wav")
        End Sub

        Public Sub PlayHats()
            Dim HatsTask As Task(Of Task) = New Task(Of Task) _
           (Async Function() As Task
                Await Hats.Show()
                Await Task.Delay(500)
            End Function)
            Dim MyRequest As New ResourceRequest({ResourceIDs.CenterScreen}, HatsTask, "Hats Hats Hats")
            Dim Alert As Task = MyResourceManager.RequestResource(MyRequest)
        End Sub

        Public Sub PlaySoundAlert(Username As String, Optional SoundFile As String = "")
            Dim AlertTask As Task(Of Task) = New Task(Of Task) _
           (Async Function() As Task
                If SoundFile = "" Then
                    SoundFile = AudioControl.GetRandomPublicSoundFile
                Else
                    SoundFile = AudioControl.GetSoundFileDataByName(SoundFile)
                End If
                SetOBSsourceText("Sound Alert Text", Username & " PLAYED:" & vbCrLf & Replace(SoundFile, ".wav", ""))
                Await SoundAlert.Show(, SoundFile, True)
                Await Task.Delay(500)
            End Function)
            Dim MyRequest As New ResourceRequest({ResourceIDs.CenterScreen}, AlertTask, "Sound Alert")
            Dim Alert As Task = MyResourceManager.RequestResource(MyRequest)
        End Sub

        Public Function Roll6(RollTier As Integer, Username As String, Optional ForceWin As Boolean = False) As Integer()
            Dim Output As New List(Of Integer), GottaDouble As Boolean, GottaTripple As Boolean, PauseTime As Integer
            Select Case RollTier
                Case 1
                    Output.Add(RandomInt(0, 5))
                    'SendMessage(Output(0))
                    GottaDouble = False
                    GottaTripple = False
                    PauseTime = 300
                Case 2
                    If ForceWin Then
                        Dim InputInt As Integer = RandomInt(0, 5)
                        Output.Add(InputInt)
                        Output.Add(InputInt)
                    Else
                        Output.Add(RandomInt(0, 5))
                        Output.Add(RandomInt(0, 5))
                        'SendMessage(Output(0) & ", " & Output(1))
                    End If
                    GottaDouble = (Output(0) = Output(1))
                    If GottaDouble Then
                        PauseTime = 3000
                    Else
                        PauseTime = 300
                    End If
                    GottaTripple = False
                Case 3
                    If ForceWin Then
                        Dim InputInt As Integer = RandomInt(0, 5)
                        Output.Add(InputInt)
                        Output.Add(InputInt)
                        Output.Add(InputInt)
                    Else
                        Output.Add(RandomInt(0, 5))
                        Output.Add(RandomInt(0, 5))
                        Output.Add(RandomInt(0, 5))
                        'SendMessage(Output(0) & ", " & Output(1) & ", " & Output(2))
                    End If

                    Select Case Output.Distinct.Count
                        Case = 3
                            GottaDouble = False
                            GottaTripple = False
                            PauseTime = 100
                        Case = 2
                            GottaDouble = True
                            GottaTripple = False
                            PauseTime = 3000
                        Case = 1
                            GottaDouble = False
                            GottaTripple = True
                            PauseTime = 5000
                    End Select
            End Select
            Dim RollTask As Task(Of Task) = New Task(Of Task) _
           (Async Function() As Task
                Dim TaskList As New List(Of Task)
                Select Case RollTier
                    Case 1
                        SetOBSsourceText("Roll Text", Username & "  Redeemed  roll6  tier1")
                        SetSourceVisibility(0, "Events and Alerts", True,, "Roll6")
                        Await Task.Delay(250)
                        TaskList.Add(Rollers(1).Show(Output(0),,,, PauseTime))
                        Await Task.WhenAll(TaskList.ToArray)
                        SetSourceVisibility(0, "Events and Alerts", False,, "Roll6")
                        Await Task.Delay(1500)
                    Case 2
                        SetOBSsourceText("Roll Text", Username & "  Redeemed  roll6  tier2")
                        SetSourceVisibility(0, "Events and Alerts", True,, "Roll6")
                        Await Task.Delay(250)
                        TaskList.Add(Rollers(3).Show(Output(0),,,, PauseTime))
                        Await Task.Delay(150)
                        TaskList.Add(Rollers(4).Show(Output(1), "rollsound2.wav",,, PauseTime))
                        If GottaDouble Then
                            Await Task.Delay(6000)
                            Dim Celebrate As Task = Confetti.Show(RandomInt(0, 3))
                            Await doublekill.Show()
                            Await Task.WhenAll(TaskList.ToArray)
                            Confetti.Hide()
                        Else
                            Await Task.WhenAll(TaskList.ToArray)
                        End If
                        SetSourceVisibility(0, "Events and Alerts", False,, "Roll6")
                        Await Task.Delay(1500)
                    Case 3
                        SetOBSsourceText("Roll Text", Username & "  Redeemed  roll6  tier3")
                        SetSourceVisibility(0, "Events and Alerts", True,, "Roll6")
                        Await Task.Delay(250)
                        TaskList.Add(Rollers(0).Show(Output(0),,,, PauseTime))
                        Await Task.Delay(150)
                        TaskList.Add(Rollers(1).Show(Output(1), "rollsound2.wav",,, PauseTime))
                        Await Task.Delay(150)
                        TaskList.Add(Rollers(2).Show(Output(2), "rollsound2.wav",,, PauseTime))
                        If GottaTripple Then AudioControl.SoundPlayer.PlaySound("officeno_long.wav", SoundSource.SFX)
                        If GottaDouble Or GottaTripple Then
                            Await Task.Delay(6000)
                            Dim Celebrate As Task = Confetti.Show(RandomInt(0, 3))
                            If GottaDouble Then Await doublekill.Show()
                            If GottaTripple Then
                                AudioControl.SoundPlayer.PlaySound("Jackpot.wav", SoundSource.SFX)
                                Await OHBABYTRIPPLE.Show()
                            End If
                            Await Task.WhenAll(TaskList.ToArray)
                            Confetti.Hide()
                        Else
                            Await Task.WhenAll(TaskList.ToArray)
                        End If
                        SetSourceVisibility(0, "Events and Alerts", False,, "Roll6")
                        Await Task.Delay(1500)
                End Select
            End Function)
            Dim MyRequest As New ResourceRequest({ResourceIDs.CenterScreen}, RollTask, "Rolling Tier " & RollTier)
            Dim RunRollTask As Task = MyResourceManager.RequestResource(MyRequest)
            Return Output.ToArray
        End Function

        Public Sub ViewerEvent(EventType As Integer, Optional Username As String = "", Optional Amount As Double = 0)
            Dim AlertTask As Task(Of Task) = New Task(Of Task) _
           (Async Function() As Task
                Dim Speaker1 As Task, Speaker2 As Task, Show As Task
                Select Case EventType
                    Case UserEvents.NewFollower
                        Show = UserAlert.Show(EventType, "PKM Achievement.wav")
                        Await Task.Delay(250)
                        Dim Messenger1 As Integer = EmberOrLuna()
                        Select Case Messenger1
                            Case WhichSprite.Ember
                                Speaker1 = Ember.Says("THANK YOU " & Username & "!!!", Ember.Mood.Happy,,,, True, 1, 0)
                            Case WhichSprite.Luna
                                Speaker1 = Luna.Says("THANK YOU " & Username & "!!!", Luna.Mood.Happy,,,, True)
                        End Select
                        Await Task.Delay(250)
                        Select Case Messenger1
                            Case WhichSprite.Ember
                                If LunaSpriteB Then
                                    Speaker2 = Luna.Says(RandomMessage(Username, "NewFollower"), Luna.Mood.Happy,,,, True)
                                Else
                                    Await Speaker1
                                    Speaker1 = Nothing
                                    Speaker2 = Ember.Says(RandomMessage(Username, "NewFollower"), Ember.Mood.Happy,,,, True, 1, 0)
                                End If
                            Case WhichSprite.Luna
                                If EmberSpriteB Then
                                    Speaker2 = Ember.Says(RandomMessage(Username, "NewFollower"), Ember.Mood.Happy,,,, True, 1, 0)
                                Else
                                    Await Speaker1
                                    Speaker1 = Nothing
                                    Speaker2 = Luna.Says(RandomMessage(Username, "NewFollower"), Luna.Mood.Happy,,,, True)
                                End If
                        End Select
                        Await Show
                        If Speaker1 IsNot Nothing Then Await Speaker1
                        If Speaker2 IsNot Nothing Then Await Speaker2
                    Case UserEvents.NewSubscriber
                        Show = UserAlert.Show(EventType, "PKM Achievement 3.wav")
                        Dim Celebrate As Task = Confetti.Show(RandomInt(0, 3))
                        Await Task.Delay(250)
                        Dim Messenger1 As Integer = EmberOrLuna()
                        Select Case Messenger1
                            Case WhichSprite.Ember
                                Speaker1 = Ember.Says("THANK YOU SO MUCH " & Username & "!!!", Ember.Mood.WooHoo,,, 2800, True, ,, Ember.Mood.Happy)
                            Case WhichSprite.Luna
                                Speaker1 = Luna.Says("THANK YOU SO MUCH " & Username & "!!!", Luna.Mood.WooHoo,,, 2250, True,,, Luna.Mood.Happy)
                        End Select
                        Await Task.Delay(250)
                        Select Case Messenger1
                            Case WhichSprite.Ember
                                If LunaSpriteB Then
                                    Speaker2 = Luna.Says(RandomMessage(Username, "NewSubscriber"), Luna.Mood.WooHoo,,, 2250, True,,, Luna.Mood.Happy)
                                Else
                                    Await Speaker1
                                    Speaker1 = Nothing
                                    Speaker2 = Ember.Says(RandomMessage(Username, "NewSubscriber"), Ember.Mood.Happy,,,, True)
                                End If
                            Case WhichSprite.Luna
                                If EmberSpriteB Then
                                    Speaker2 = Ember.Says(RandomMessage(Username, "NewSubscriber"), Ember.Mood.WooHoo,,, 2800, True, ,, Ember.Mood.Happy)
                                Else
                                    Await Speaker1
                                    Speaker1 = Nothing
                                    Speaker2 = Luna.Says(RandomMessage(Username, "NewSubscriber"), Luna.Mood.Happy,,,, True)
                                End If
                        End Select
                        Await Show
                        If Speaker1 IsNot Nothing Then Await Speaker1
                        If Speaker2 IsNot Nothing Then Await Speaker2
                        Confetti.Hide()
                        Await Task.Delay(1000)
                End Select

            End Function)
            Dim MyRequest As New ResourceRequest({ResourceIDs.CenterScreen, ResourceIDs.EmberSprite, ResourceIDs.LunaSprite}, AlertTask, "Viewer Event")
            Dim Alert As Task = MyResourceManager.RequestResource(MyRequest)
        End Sub
    End Class


    '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    '/////////////////////////////////////////////////////RESOURCE MANAGER////////////////////////////////////////////////////
    '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    Public Structure ResourceIDs
        Public Const SoundPlayer As Integer = 0
        Public Const AlertPlayer As Integer = 1
        Public Const MusicPlayer As Integer = 2
        Public Const EmberSprite As Integer = 3
        Public Const LunaSprite As Integer = 4
        Public Const EmberTimer As Integer = 5
        Public Const LunaTimer As Integer = 6
        Public Const GlobalTimer As Integer = 7
        Public Const EmberCounter As Integer = 8
        Public Const LunaCounter As Integer = 9
        Public Const GlobalCounter As Integer = 10
        Public Const CenterScreen As Integer = 11
        Public Const TwitchAPI As Integer = 12
        Public Const LastID As Integer = 12
    End Structure

    Public Structure RMtaskStates
        Public Const Started As Integer = 0
        Public Const Ended As Integer = 1
        Public Const Queued As Integer = 2
        Public Const Abandoned As Integer = 3
    End Structure

    Public Class ResourceManager
        Private Rqueue As List(Of ResourceRequest)
        Private Reservations() As Boolean
        Private QueueTex As Mutex
        Private WaitingForCompletion As Boolean
        Private TasksCompleted As TaskCompletionSource(Of Boolean)
        Private RunningTasks As Boolean
        Public Event TaskEvent(TaskString As String, StateID As Integer)

        Public Sub New(ResourceCount As Integer)
            QueueTex = New Mutex
            Rqueue = New List(Of ResourceRequest)
            WaitingForCompletion = False
            RunningTasks = False
            ReDim Reservations(0 To ResourceCount)
            For i As Integer = 0 To ResourceCount
                Reservations(i) = False
            Next
        End Sub

        Public Function OKtoGO(RIDs() As Integer) As Boolean
            For i As Integer = 0 To RIDs.Length - 1
                If Reservations(RIDs(i)) = True Then
                    Return False
                    Exit Function
                End If
            Next
            Return True
        End Function

        Private Sub SetRIDs(RIDs() As Integer, Value As Boolean)
            For i As Integer = 0 To RIDs.Length - 1
                Reservations(RIDs(i)) = Value
            Next
        End Sub

        Public Sub CancelAllRequests(Optional SpecifyRIDs() As Integer = Nothing)
            QueueTex.WaitOne()
            If Rqueue.Count > 0 Then

            Else

            End If
            QueueTex.ReleaseMutex()
        End Sub

        Public Async Function WaitAllRequests() As Task
            If RunningTasks And WaitingForCompletion = False Then
                WaitingForCompletion = True
                TasksCompleted = New TaskCompletionSource(Of Boolean)
            End If
            If WaitingForCompletion Then WaitingForCompletion = Await TasksCompleted.Task
        End Function

        Public Async Function RequestResource(InputRequest As ResourceRequest,
                                         Optional RejectIfUnavailable As Boolean = False) As Task(Of Boolean)
            If OKtoGO(InputRequest.RIDs) Then
                RunningTasks = True
                SetRIDs(InputRequest.RIDs, True)
                RaiseEvent TaskEvent(InputRequest.TaskString, RMtaskStates.Started)
                If InputRequest.MyTask IsNot Nothing Then
                    InputRequest.MyTask.Start()
                    Await InputRequest.MyTask.Result
                    InputRequest.RequestCompleted.SetResult(True)
                End If
                RaiseEvent TaskEvent(InputRequest.TaskString, RMtaskStates.Ended)
                SetRIDs(InputRequest.RIDs, False)
            Else
                If RejectIfUnavailable Then
                    RaiseEvent TaskEvent(InputRequest.TaskString, RMtaskStates.Abandoned)
                    Return False
                Else
                    QueueTex.WaitOne()
                    InputRequest.QID = Rqueue.Count + 1
                    Rqueue.Add(InputRequest)
                    Dim QueueCount As Integer = Rqueue.Count
                    QueueTex.ReleaseMutex()
                    RaiseEvent TaskEvent(InputRequest.TaskString & " (Queue# " & QueueCount & ")", RMtaskStates.Queued)
                    Return True
                End If
                Exit Function
            End If
            Dim MyRequest As ResourceRequest
            Dim ReleaseMe As Boolean
FromTheTop:
            QueueTex.WaitOne()
            ReleaseMe = True
            If Rqueue.Count > 0 Then
                For I As Integer = 0 To Rqueue.Count - 1
                    If OKtoGO(Rqueue(I).RIDs) Then
                        MyRequest = Rqueue(I)
                        Rqueue.RemoveAt(I)
                        QueueTex.ReleaseMutex()
                        ReleaseMe = False
                        SetRIDs(MyRequest.RIDs, True)
                        RaiseEvent TaskEvent(MyRequest.TaskString & " (Q#I" & MyRequest.QID & "/O" & I & "/R" & Rqueue.Count & ")", RMtaskStates.Started)
                        If MyRequest.MyTask IsNot Nothing Then
                            MyRequest.MyTask.Start()
                            Await MyRequest.MyTask.Result
                            MyRequest.RequestCompleted.SetResult(True)
                        End If
                        RaiseEvent TaskEvent(MyRequest.TaskString & " (Q#I" & MyRequest.QID & "/O" & I & "/R" & Rqueue.Count & ")", RMtaskStates.Ended)
                        SetRIDs(MyRequest.RIDs, False)
                        GoTo FromTheTop
                    End If
                Next
            Else
                RunningTasks = False
                If WaitingForCompletion Then TasksCompleted.SetResult(False)
            End If
            If ReleaseMe Then QueueTex.ReleaseMutex()
            Return True
        End Function

    End Class

    Public Class ResourceRequest

        Public RIDs() As Integer
        Public MyTask As Task(Of Task)
        Public TaskString As String
        Public QID As Integer
        Public RequestCompleted As TaskCompletionSource(Of Boolean)

        Public Sub New(ResourceIDs() As Integer, TaskToRun As Task(Of Task), Optional TaskTitle As String = "RMtask")
            RIDs = ResourceIDs
            MyTask = TaskToRun
            TaskString = TaskTitle
            QID = -1
            RequestCompleted = New TaskCompletionSource(Of Boolean)
        End Sub

    End Class

End Module
