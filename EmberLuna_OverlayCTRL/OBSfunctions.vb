Imports OBSWebsocketDotNet
Imports OBSWebsocketDotNet.Types
Imports System.Net.Sockets
Imports System.Threading
Imports System.IO
Imports System.Math
Imports TwitchLib.PubSub
Imports System.Collections.Concurrent
Imports EmberLuna_OverlayCTRL.OBSfunctions
Imports System.Media
Imports System.IO.Ports
Imports System.Windows.Forms.AxHost
Imports System.Reflection.Emit

Public Module OBSfunctions

    Public OBS As OBSWebsocket
    Public OBSmutex As Mutex

    Public Function OBSstreamState() As Boolean
        OBSmutex.WaitOne()
        Dim MyStatus As OutputStatus = OBS.GetStreamingStatus
        OBSmutex.ReleaseMutex()
        Return MyStatus.IsStreaming
    End Function

    Public Sub SetOBSsourceText(SourceName As String, TextData As String, Optional BypassMutex As Boolean = False)
        If BypassMutex = False Then OBSmutex.WaitOne()
        Dim SourceSettings As Newtonsoft.Json.Linq.JObject = OBS.GetSourceSettings(SourceName).Settings
        SourceSettings.Property("text").Value = TextData
        OBS.SetSourceSettings(SourceName, SourceSettings)
        If BypassMutex = False Then OBSmutex.ReleaseMutex()
    End Sub

    Public Sub SetOBSscene(SceneString As String)
        OBSmutex.WaitOne()
        If OBS.IsConnected = True Then
            OBS.SetCurrentScene(SceneString)
        End If
        OBSmutex.ReleaseMutex()
    End Sub



    Public Sub DisconnectOBS()
        OBSmutex.WaitOne()
        If OBS.IsConnected = True Then
            OBS.Disconnect()
        End If
        OBSmutex.ReleaseMutex()
    End Sub

    Public Function CheckOBSconnect() As Boolean
        OBSmutex.WaitOne()
        If OBS.IsConnected = False Then
            Try
                'SendMessage(OBSsocketString & " " & OBSsocketPassword)
                OBS.Connect(OBSsocketString, OBSsocketPassword)
                CurrentScene = OBS.GetCurrentScene
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

    Public Sub ResetMediaSource(SourceName As String)
        Dim ResetThread As Thread = New Thread(
            Sub()
                OBSmutex.WaitOne()
                Dim Settings As MediaSourceSettings = OBS.GetMediaSourceSettings(SourceName)
                Settings.Media.LocalFile = ""
                OBS.SetMediaSourceSettings(Settings)
                OBSmutex.ReleaseMutex()
            End Sub)
        ResetThread.Start()
    End Sub



    Public Function MediaSourceChange(SourceName As String, Optional ActionType As Integer = 0, Optional FileName As String = "") As String
        OBSmutex.WaitOne()
        Dim Settings As MediaSourceSettings = OBS.GetMediaSourceSettings(SourceName)
        Dim OutputFile As String = Settings.Media.LocalFile
        If FileName <> "" Then
            Settings.Media.LocalFile = FileName
            OBS.SetMediaSourceSettings(Settings)
            OutputFile = Settings.Media.LocalFile
        Else
            Select Case ActionType
                Case = MediaFcode.Play
                    OBS.PlayPauseMedia(SourceName, False)
                Case = MediaFcode.Pause
                    OBS.PlayPauseMedia(SourceName, True)
                Case = MediaFcode.Stopp
                    OBS.StopMedia(SourceName)
                Case = MediaFcode.Restart
                    OBS.RestartMedia(SourceName)
            End Select
        End If
        OBSmutex.ReleaseMutex()
        Return OutputFile
    End Function

    '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    '///////////////////////////////////////////////////SCENE FUNCTIONS///////////////////////////////////////////////////////
    '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    Public CurrentScene As OBSScene

    'AWAY MODE
    Public EmberAwayB As Boolean = False
    Public LunaAwayB As Boolean = False
    Public Screen1AwayMode As Boolean = False
    Public Screen2AwayMode As Boolean = False

    'SCREEN SETTINGS
    Public Screen2Enabled As Boolean = False
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
                                            Screen2Enabled, CurrentScene.Name)

            NewScene.WriteSceneFile(SceneDirectory)
            SceneCollection(SceneIndex) = NewScene
            'RefreshSceneCollection()
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
                                            Screen2Enabled, CurrentScene.Name)
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
        Public Screen2en As Integer
        Public Sub New(Optional SourceFile As String = "",
                       Optional SceneName As String = "",
                       Optional Aways() As Boolean = Nothing,
                       Optional Feats() As Boolean = Nothing,
                       Optional Settings() As Integer = Nothing,
                       Optional Screen2 As Boolean = False,
                       Optional ScName As String = "")
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
            Await UpdateSceneDisplay()
            If CurrentScene.Name <> Sname Then
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
    End Structure

    Public Structure OBSinput
        Public ObjectName As String
        Public DisplayName As String
    End Structure

    Public Sub SceneChangeDetected()
        If SceneChangeInProgress = True Then
            SceneChangeInProgress = False
            If SceneChangeNeeded = True Then SceneHasChanged.SetResult(False)
        End If
    End Sub


    Public Async Function UpdateSceneDisplay(Optional ChangeSceneString As String = "") As Task

        SceneChangeInProgress = True

        Dim ItemsList As List(Of OBSWebsocketDotNet.Types.SceneItem)
        Dim Container As OBSWebsocketDotNet.Types.SceneItemProperties
        Dim CurrentSceneList As OBSWebsocketDotNet.Types.GetSceneListInfo

        OBSmutex.WaitOne()
        CurrentScene = OBS.GetCurrentScene
        Dim SceneName As String = CurrentScene.Name

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
                    ItemsList = ActiveScene.Items
                    For Each SceneItem In ItemsList
                        Select Case SceneItem.SourceName
                            Case = "SoundAlert"
                                Container = OBS.GetSceneItemProperties(SceneItem.SourceName, ActiveScene.Name)
                                If SoundAlertDisplay <> Container.Visible Then
                                    If SoundAlertDisplay = True Then
                                        Container.Visible = True
                                        OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                                    Else
                                        Container.Visible = False
                                        OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                                    End If
                                End If
                        End Select
                    Next
                Case "Dual Screen Mode", "Center Screen Mode", "Single Screen Mode", "Split Screen Mode"
                    ItemsList = ActiveScene.Items
                    For Each SceneItem In ItemsList
                        Select Case SceneItem.SourceName
                            Case = "EmberTimer"
                                Container = OBS.GetSceneItemProperties(SceneItem.SourceName, ActiveScene.Name)
                                If OBStimerObject(TimerIDs.Ember).State <> Container.Visible Then
                                    If OBStimerObject(TimerIDs.Ember).State = True Then
                                        Container.Visible = True
                                        OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                                    Else
                                        Container.Visible = False
                                        OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                                    End If
                                End If
                            Case = "EmberCounter"
                                Container = OBS.GetSceneItemProperties(SceneItem.SourceName, ActiveScene.Name)
                                If OBScounterObject(CounterIDs.Ember).State <> Container.Visible Then
                                    If OBScounterObject(CounterIDs.Ember).State = True Then
                                        Container.Visible = True
                                        OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                                    Else
                                        Container.Visible = False
                                        OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                                    End If
                                End If
                            Case = "LunaTimer"
                                Container = OBS.GetSceneItemProperties(SceneItem.SourceName, ActiveScene.Name)
                                If OBStimerObject(TimerIDs.Luna).State <> Container.Visible Then
                                    If OBStimerObject(TimerIDs.Luna).State = True Then
                                        Container.Visible = True
                                        OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                                    Else
                                        Container.Visible = False
                                        OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                                    End If
                                End If
                            Case = "LunaCounter"
                                Container = OBS.GetSceneItemProperties(SceneItem.SourceName, ActiveScene.Name)
                                If OBScounterObject(CounterIDs.Luna).State <> Container.Visible Then
                                    If OBScounterObject(CounterIDs.Luna).State = True Then
                                        Container.Visible = True
                                        OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                                    Else
                                        Container.Visible = False
                                        OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                                    End If
                                End If
                            Case = "GlobalTimer"
                                Container = OBS.GetSceneItemProperties(SceneItem.SourceName, ActiveScene.Name)
                                If OBStimerObject(TimerIDs.GlobalCC).State <> Container.Visible Then
                                    If OBStimerObject(TimerIDs.GlobalCC).State = True Then
                                        Container.Visible = True
                                        OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                                    Else
                                        Container.Visible = False
                                        OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                                    End If
                                End If
                            Case = "GlobalCounter"
                                Container = OBS.GetSceneItemProperties(SceneItem.SourceName, ActiveScene.Name)
                                If OBScounterObject(CounterIDs.Glob).State <> Container.Visible Then
                                    If OBScounterObject(CounterIDs.Glob).State = True Then
                                        Container.Visible = True
                                        OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                                    Else
                                        Container.Visible = False
                                        OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                                    End If
                                End If

                            Case = "Screen 1"
                                If ActiveScene.Name = "Center Screen Mode" Or ActiveScene.Name = "Single Screen Mode" Then
                                    Container = OBS.GetSceneItemProperties(SceneItem.SourceName, ActiveScene.Name)
                                    If Screen2Enabled = True Then
                                        If Container.Visible = True Then
                                            Container.Visible = False
                                            OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                                        End If
                                    Else
                                        If Container.Visible = False Then
                                            Container.Visible = True
                                            OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                                        End If
                                    End If
                                End If
                            Case = "Screen 2"
                                If ActiveScene.Name = "Center Screen Mode" Or ActiveScene.Name = "Single Screen Mode" Then
                                    Container = OBS.GetSceneItemProperties(SceneItem.SourceName, ActiveScene.Name)
                                    If Screen2Enabled = False Then
                                        If Container.Visible = True Then
                                            Container.Visible = False
                                            OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                                        End If
                                    Else
                                        If Container.Visible = False Then
                                            Container.Visible = True
                                            OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                                        End If
                                    End If
                                End If
                            Case = "Ember Cam"
                                Container = OBS.GetSceneItemProperties(SceneItem.SourceName, ActiveScene.Name)
                                If ActiveScene.Name = "Split Screen Mode" Or ActiveScene.Name = "Single Screen Mode" Then
                                    If EmberCamera = False Then
                                        If Container.Visible = True Then
                                            Container.Visible = False
                                            OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                                        End If
                                    Else
                                        If Container.Visible = False Then
                                            Container.Visible = True
                                            OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                                        End If
                                    End If
                                Else
                                    If Container.Visible = False Then
                                        Container.Visible = True
                                        OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                                    End If
                                End If
                            Case = "Luna Cam"
                                Container = OBS.GetSceneItemProperties(SceneItem.SourceName, ActiveScene.Name)
                                If ActiveScene.Name = "Split Screen Mode" Or ActiveScene.Name = "Single Screen Mode" Then
                                    If LunaCamera = False Then
                                        If Container.Visible = True Then
                                            Container.Visible = False
                                            OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                                        End If
                                    Else
                                        If Container.Visible = False Then
                                            Container.Visible = True
                                            OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                                        End If
                                    End If
                                Else
                                    If Container.Visible = False Then
                                        Container.Visible = True
                                        OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                                    End If
                                End If
                            Case = "Ember"
                                Container = OBS.GetSceneItemProperties(SceneItem.SourceName, ActiveScene.Name)
                                If ActiveScene.Name = "Split Screen Mode" Or ActiveScene.Name = "Single Screen Mode" Then
                                    If EmberSpriteB = False Then
                                        If Container.Visible = True Then
                                            Container.Visible = False
                                            OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                                        End If
                                    Else
                                        If Container.Visible = False Then
                                            Container.Visible = True
                                            OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                                        End If
                                    End If
                                Else
                                    If Container.Visible = False Then
                                        Container.Visible = True
                                        OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                                    End If
                                End If
                            Case = "Luna"
                                Container = OBS.GetSceneItemProperties(SceneItem.SourceName, ActiveScene.Name)
                                If ActiveScene.Name = "Split Screen Mode" Or ActiveScene.Name = "Single Screen Mode" Then
                                    If LunaSpriteB = False Then
                                        If Container.Visible = True Then
                                            Container.Visible = False
                                            OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                                        End If
                                    Else
                                        If Container.Visible = False Then
                                            Container.Visible = True
                                            OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                                        End If
                                    End If
                                Else
                                    If Container.Visible = False Then
                                        Container.Visible = True
                                        OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                                    End If
                                End If
                        End Select
                    Next
            End Select
        Next

        If ChangeSceneString <> "" And CurrentScene.Name <> ChangeSceneString Then
            RaiseEvent SceneChangeInitiated()
            SceneHasChanged = New TaskCompletionSource(Of Boolean)
            SceneChangeNeeded = True
            OBS.SetCurrentScene(ChangeSceneString)
            SceneChangeNeeded = Await SceneHasChanged.Task
            CurrentScene = OBS.GetCurrentScene
            RaiseEvent SceneChangeCompleted()
        Else
            SceneChangeInProgress = False
        End If

        OBSmutex.ReleaseMutex()

    End Function
    Public Sub SetScreenSettings(ActiveScene As OBSScene, Setting As Integer, Awaymode As Boolean, AwayName As String)
        Dim Container As OBSWebsocketDotNet.Types.SceneItemProperties
        For Each SceneItem In ActiveScene.Items
            Select Case SceneItem.SourceName
                Case = "Ember's PC"
                    Container = OBS.GetSceneItemProperties(SceneItem.SourceName, ActiveScene.Name)
                    If Setting = ScreenSetting.EmberPC Then
                        If Container.Visible = False Then
                            Container.Visible = True
                            OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                        End If
                    Else
                        If Container.Visible = True Then
                            Container.Visible = False
                            OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                        End If
                    End If
                Case = "Luna's PC"
                    Container = OBS.GetSceneItemProperties(SceneItem.SourceName, ActiveScene.Name)
                    If Setting = ScreenSetting.LunaPC Then
                        If Container.Visible = False Then
                            Container.Visible = True
                            OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                        End If
                    Else
                        If Container.Visible = True Then
                            Container.Visible = False
                            OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                        End If
                    End If
                Case = "J-Cam 1"
                    Container = OBS.GetSceneItemProperties(SceneItem.SourceName, ActiveScene.Name)
                    If Setting = ScreenSetting.EmberCam Then
                        If Container.Visible = False Then
                            Container.Visible = True
                            OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                        End If
                    Else
                        If Container.Visible = True Then
                            Container.Visible = False
                            OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                        End If
                    End If
                Case = "E-Cam 1"
                    Container = OBS.GetSceneItemProperties(SceneItem.SourceName, ActiveScene.Name)
                    If Setting = ScreenSetting.LunaCam Then
                        If Container.Visible = False Then
                            Container.Visible = True
                            OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                        End If
                    Else
                        If Container.Visible = True Then
                            Container.Visible = False
                            OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                        End If
                    End If
                Case = "Aux In 1"
                    Container = OBS.GetSceneItemProperties(SceneItem.SourceName, ActiveScene.Name)
                    If Setting = ScreenSetting.Aux1 Then
                        If Container.Visible = False Then
                            Container.Visible = True
                            OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                        End If
                    Else
                        If Container.Visible = True Then
                            Container.Visible = False
                            OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                        End If
                    End If
                Case = "Aux In 2"
                    Container = OBS.GetSceneItemProperties(SceneItem.SourceName, ActiveScene.Name)
                    If Setting = ScreenSetting.Aux2 Then
                        If Container.Visible = False Then
                            Container.Visible = True
                            OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                        End If
                    Else
                        If Container.Visible = True Then
                            Container.Visible = False
                            OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                        End If
                    End If
                Case = "Aux-Cam 1"
                    Container = OBS.GetSceneItemProperties(SceneItem.SourceName, ActiveScene.Name)
                    If Setting = ScreenSetting.Aux3 Then
                        If Container.Visible = False Then
                            Container.Visible = True
                            OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                        End If
                    Else
                        If Container.Visible = True Then
                            Container.Visible = False
                            OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                        End If
                    End If
                Case = "Aux-Cam 2"
                    Container = OBS.GetSceneItemProperties(SceneItem.SourceName, ActiveScene.Name)
                    If Setting = ScreenSetting.Aux4 Then
                        If Container.Visible = False Then
                            Container.Visible = True
                            OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                        End If
                    Else
                        If Container.Visible = True Then
                            Container.Visible = False
                            OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                        End If
                    End If
                Case = AwayName
                    Container = OBS.GetSceneItemProperties(SceneItem.SourceName, ActiveScene.Name)
                    If Awaymode = True Then
                        If Container.Visible = False Then
                            Container.Visible = True
                            OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                        End If
                    Else
                        If Container.Visible = True Then
                            Container.Visible = False
                            OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                        End If
                    End If
            End Select
        Next
    End Sub

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
                        IRC.SendChat(RandomMessage(UserName, MessageType))
                        AudioControl.SoundPlayer.PlaySound(AudioControl.GetSoundFileDataByName("Hello"), SoundSource.SFX)
                    End If
                End If
            End If
            'End If
        End If
    End Sub

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
        Public Manic As String
        Public RockandStone As String
        Public Wibble As String
        Public DeepBreath As String

        Public Sub InitializeMoods(CharacterSelect As Boolean)
            If CharacterSelect = SpriteID.Ember Then
                Neutral = "blink.avi"
                Happy = "happy.avi"
                Sadge = "sadge.avi"
                Angy = "angery.avi"
                Wumpy = "wumpy.avi"
                Cringe = ""
                Wow = ""
                Sparkle = ""
                WTF = "wtf.avi"
                OMG = ""
                RockandStone = "rocknstone.avi"
                Manic = ""
                Wibble = ""
                DeepBreath = ""
            Else
                Neutral = "blink.mp4"
                Happy = "smile.mp4"
                Sadge = "sad.mp4"
                Angy = "angery.mp4"
                Wumpy = "pissed.mp4"
                Cringe = "cringe.mp4"
                Wow = "wow.mp4"
                Sparkle = "sparkles.mp4"
                WTF = ""
                OMG = "omg.mp4"
                RockandStone = ""
                Manic = "manic.mp4"
                Wibble = "wibble.mp4"
                DeepBreath = "deepbreath.mp4"
            End If
        End Sub

    End Structure

    Public Class CharacterControls
        Private Name As String
        Public Directory As String
        Private CurrentMood As String
        Private SourceName As String
        Private Bubble As SpeechBubble
        Public Mood As CharacterMoods
        Private ResourceID As Integer
        Private FileHeader As String
        Public Event Said(MessageData As String)
        Public Event MoodChange(MoodString As String)
        Public LeftHand As Boolean
        Public RightHand As Boolean

        Public Sub New()
            AddHandler Bubble.MessageSent, AddressOf MessageSent
        End Sub

        Public Sub initLUNA()
            Name = "Luna"
            FileHeader = "\luna_"
            Mood.InitializeMoods(SpriteID.Luna)
            Directory = "\\StreamPC-V2\OBS Assets\Characters\" & Name
            CurrentMood = ""
            SourceName = Name & " Sprite"
            ResourceID = ResourceIDs.LunaSprite
            LeftHand = False
            RightHand = False
            Bubble.InitializeBubble(Name & "'s Speech Bubble", Name & " Messenger", Name & " Mtext")
        End Sub

        Public Sub initEMBER()
            Name = "Ember"
            FileHeader = "\ember_"
            Mood.InitializeMoods(SpriteID.Ember)
            Directory = "\\StreamPC-V2\OBS Assets\Characters\" & Name
            CurrentMood = ""
            SourceName = Name & " Sprite"
            ResourceID = ResourceIDs.EmberSprite
            LeftHand = False
            RightHand = False
            Bubble.InitializeBubble(Name & "'s Speech Bubble", Name & " Messenger", Name & " Mtext")
        End Sub

        Public Sub Says(InputMessage As String,
                        Optional MessageMood As String = "",
                        Optional MessageSound As String = "")
            Dim SpeechTask As Task(Of Task) = New Task(Of Task) _
            (Async Function() As Task
                 Await MessageAsync(InputMessage, MessageMood, MessageSound)
             End Function)
            Dim Speak As Task = MyResourceManager.RequestResource({ResourceID}, SpeechTask,
                                                                  Name & " Sprite Says: " & InputMessage)
        End Sub

        Public Function MyMood() As String
            Dim OutputString As String = Replace(CurrentMood, Directory & FileHeader, "")
            Dim HandsInt As Integer = 0
            If LeftHand = True Then HandsInt = HandsInt + 1
            If RightHand = True Then HandsInt = HandsInt + 2
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
        Private Async Function MessageAsync(InputMessage As String,
                        Optional MessageMood As String = "",
                        Optional MessageSound As String = "",
                        Optional StaticMoodChange As Boolean = False) As Task
            If MessageSound <> "" Then
                AudioControl.SoundPlayer.PlaySound(AudioControl.GetSoundFileDataByName(MessageSound), SoundSource.SFX)
            End If
            Dim TempMood As String
            If StaticMoodChange = False Then
                TempMood = CurrentMood
            Else
                TempMood = ""
            End If
            If MessageMood <> "" Then
                MessageMood = AppendMood(MessageMood)
                CurrentMood = MediaSourceChange(SourceName, 1, Directory & FileHeader & MessageMood)
                RaiseEvent MoodChange(CurrentMood)
            End If
            Await Bubble.SendMessage(InputMessage)
            If MessageMood <> "" And TempMood <> "" Then
                CurrentMood = MediaSourceChange(SourceName, 1, TempMood)
                RaiseEvent MoodChange(CurrentMood)
            End If
        End Function

        Private Function AppendMood(InputString As String) As String
            Dim HandsInt As Integer = 0
            If LeftHand = True Then HandsInt = HandsInt + 1
            If RightHand = True Then HandsInt = HandsInt + 2
            Select Case HandsInt
                Case 0
                    Return InputString
                Case = 1
                    Return Replace(InputString, ".", "_left.")
                Case = 2
                    Return Replace(InputString, ".", "_right.")
                Case = 3
                    Return Replace(InputString, ".", "_both.")
            End Select
        End Function



        Public Sub MessageSent(Messenger As String, Message As String)
            RaiseEvent Said(Messenger & " sent text: (" & Message & ")")
        End Sub

        Public Sub ChangeMood(MoodFileName As String, Optional duration As Integer = 0)
            Dim MoodTask As Task(Of Task) = New Task(Of Task) _
            (Async Function() As Task
                 Await ChangeMoodAsync(MoodFileName, duration)
             End Function)
            Dim RunMood As Task = MyResourceManager.RequestResource({ResourceID}, MoodTask,
                                                                    Name & " Sprite Mood Change")
        End Sub

        Private Async Function ChangeMoodAsync(MoodFileName As String, Optional duration As Integer = 0) As Task
            If MoodFileName <> "" Then
                MoodFileName = AppendMood(MoodFileName)
                If duration > 0 Then
                    Dim TempMood As String = CurrentMood
                    CurrentMood = MediaSourceChange(SourceName, 1, Directory & FileHeader & MoodFileName)
                    RaiseEvent MoodChange(CurrentMood)
                    Await Task.Delay(duration)
                    CurrentMood = MediaSourceChange(SourceName, 1, TempMood)
                    RaiseEvent MoodChange(CurrentMood)
                Else
                    If CurrentMood <> Directory & FileHeader & MoodFileName Then
                        CurrentMood = MediaSourceChange(SourceName, 1, Directory & FileHeader & MoodFileName)
                        RaiseEvent MoodChange(CurrentMood)
                    End If
                End If
            End If
        End Function
    End Class

    Public Structure SpeechBubble
        Public Name As String
        Public Bubble As String
        Public Msource As String
        Public Mtext As String
        Public Event MessageSent(Messenger As String, Message As String)

        Public Sub InitializeBubble(InputName As String, InputBubble As String, InputSource As String)
            Name = InputName
            Bubble = InputBubble
            Msource = InputSource
            Mtext = ""
        End Sub

        Public Async Function SendMessage(MessageText As String) As Task
            Mtext = MessageText
            RaiseEvent MessageSent(Name, Mtext)
            Call MediaSourceChange(Bubble, 4)
            Await Task.Delay(220)
            Call SetOBSsourceText(Msource, Mtext)
            Mtext = ""
            Await Task.Delay(3900)
            Call SetOBSsourceText(Msource, Mtext)
            Await Task.Delay(100)
        End Function
    End Structure


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
        SetOBSsourceText(OBScounterObject(CounterSelection).ValueSource, OBScounterObject(CounterSelection).Value)
        If CounterTick <> "" Then Dim TickTask As Task = CounterTicker.PlaySound(CounterTick)

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
        OBScounterObject(CounterIDs.Glob) = New TimerCounterData("Global Counter",
                                                                ResourceIDs.GlobalCounter,
                                                                "GlobalCounterTitle",
                                                                "GlobalCounterValue")

        OBScounterObject(CounterIDs.Ember) = New TimerCounterData("Ember Counter",
                                                                ResourceIDs.EmberCounter,
                                                                "EmberCounterTitle",
                                                                "EmberCounterValue")

        OBScounterObject(CounterIDs.Luna) = New TimerCounterData("Luna Counter",
                                                                ResourceIDs.LunaCounter,
                                                                "LunaCounterTitle",
                                                                "LunaCounterValue")

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
        Public Sub New(MyName As String, RID As Integer, TSource As String, VSource As String)
            State = False
            Title = ""
            Value = "00"
            Name = MyName
            ResourceID = RID
            TitleSource = TSource
            ValueSource = VSource
            SoundPlayer = New SoundController(SoundSource.SFX, Name)
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

                Dim TheCount As Task =
                    MyResourceManager.RequestResource({OBScounterObject(Counters(CounterIndex).Type).ResourceID},
                                                      CountMe, "CountIndex#(" & CounterIndex & ") " & Counters(CounterIndex).Name)
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
                        'Case = "Alarm"
                        'Alarm = SplitString(1)
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
        'Public Event TimerUpdated(TimerIndex As Integer)
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
            Dim RunThyTimer As Task = MyResourceManager.RequestResource({OBStimerObject(TimerID).ResourceID},
                                                                    GoTimer, OBStimerObject(TimerID).Name)
        End Sub



        Public Async Function RunTimerbyIndex(TimerIndex As Integer, Optional DontQueue As Boolean = True) As Task

            Dim GoTimer As New Task(Of Task) _
            (Async Function() As Task
                 Await RunTimer(Timers(TimerIndex).Time, Timers(TimerIndex).Type, Timers(TimerIndex).StopTime,
                                Timers(TimerIndex).Label, Timers(TimerIndex).Sounds)
             End Function)


            Dim RunThyTimer As Boolean = Await MyResourceManager.RequestResource({OBStimerObject(Timers(TimerIndex).Type).ResourceID},
                                                                       GoTimer, OBStimerObject(Timers(TimerIndex).Type).Name, DontQueue)
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

    Public Sub SetVolume(Sources() As String, Level As Single, Muted As Boolean, Optional SyncChannel As AudioChannel = Nothing)
        OBSmutex.WaitOne()
        Dim MyInfo As VolumeInfo
        For I As Integer = 0 To Sources.Length - 1
            MyInfo = OBS.GetVolume(Sources(I), True)
            'SendMessage(Sources(I))
            If I = 0 And SyncChannel IsNot Nothing Then
                SyncChannel.Level = MyInfo.Volume
                Level = SyncChannel.Level
                SyncChannel.Muted = MyInfo.Muted
                Muted = SyncChannel.Muted
            Else
                If MyInfo.Volume <> Level Then OBS.SetVolume(Sources(I), Level, True)
                If MyInfo.Muted <> Muted Then OBS.SetMute(Sources(I), Muted)
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
        Public Const Discord As Integer = 4
        Public Const MusicPlayer As Integer = 5
        Public Const SoundsAndEvents As Integer = 6

        Public Const LastChanneL As Integer = 6
    End Structure

    Public Class AudioChannel
        Public Name As String
        Public Muted As Boolean
        Public Level As Single
        Public SourceCollection() As String

        Public Sub New(ChannelID As Integer)
            Muted = False
            Level = -10
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
                Case AudioChannelIDs.Discord
                    Name = "DISCORD"
                    SourceCollection = {"Discord"}
                Case AudioChannelIDs.MusicPlayer
                    Name = "MUSIC PLAYER"
                    SourceCollection = {"Music Player"}
                Case AudioChannelIDs.SoundsAndEvents
                    Name = "SOUNDS + EVENTS"
                    Dim SourceList As New List(Of String)
                    For I As Integer = 0 To 9
                        SourceList.Add("Sounds" & I)
                    Next
                    SourceCollection = SourceList.ToArray
            End Select
        End Sub

        Public Sub SyncSettings()
            SetVolume(SourceCollection, Level, Muted, Me)
        End Sub



        Public Sub ApplySettings(Optional NewLevel As Single = -101, Optional ToggleMute As Boolean = False)
            If NewLevel >= -100 Then Level = NewLevel
            If ToggleMute = True Then
                If Muted = False Then
                    Muted = True
                Else
                    Muted = False
                End If
            End If
            SetVolume(SourceCollection, Level, Muted)
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
                    Case = "Level"
                        Level = Split2(1)
                    Case = "SourceCollection"
                        Array.ConstrainedCopy(Split2, 1, SourceCollection, 0, Split2.Length - 1)
                End Select
            Next
        End Sub

        Public Function WriteLine() As String
            Dim LineData As String = "Name" & "<>" & Name & "++Muted" & "<>" & Muted & "++Level" & "<>" & Level & "++SourceCollection"
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

        Public Async Function PlaySoundAlert(SoundFile As String) As Task

            Dim AlertPlayer As New Task(Of Task) _
           (Async Function() As Task
                Await PlaySoundAlertTask(SoundFile)
            End Function)
            Await MyResourceManager.RequestResource({ResourceIDs.AlertPlayer, ResourceIDs.CenterScreen}, AlertPlayer, "Sound Alert")

        End Function

        Private Async Function PlaySoundAlertTask(SoundFile As String) As Task
            SoundAlertDisplay = True
            Await UpdateSceneDisplay()
            Dim MySound As SoundTask = SoundPlayer.PlaySound(SoundFile, SoundSource.SFX)
            Await Task.Delay(3000)
            Await MySound.Player
            SoundAlertDisplay = False
            Await UpdateSceneDisplay()
            Await Task.Delay(1000)
        End Function


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
            If MusicPlayer.Active = False Or Replay = True Then
                If SongList(SongIndex) <> "" Then
                    MusicPlayer.Play(SongList(SongIndex))
                    MusicRunning = True
                End If
            End If
        End Sub

        Public Sub StopMusic()
            If MusicPlayer.Active = True Then
                MusicRunning = False
                MusicPlayer.Stopp()
            End If
        End Sub

        Public Sub PauseMusic()
            If MusicPlayer.Active = True Then
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

    Public Structure SoundTask
        Public Index As Integer
        Public Player As Task
    End Structure

    Public Class SoundBank
        Private SoundPlayers() As AudioPlayer
        Private Access As Mutex
        Public Event SoundPlayed(EventString As String)
        Public Sub New()
            Access = New Mutex
            Access.WaitOne()
            ReDim SoundPlayers(0 To 9)
            For I As Integer = 0 To 9
                SoundPlayers(I) = New AudioPlayer("Sounds" & I, SoundSource.SFX)
            Next
            Access.ReleaseMutex()
        End Sub

        Public Function PlaySound(SoundFile As String, Path As String, Optional Source As String = "") As SoundTask
            Access.WaitOne()
            Dim SoundIndex As Integer = -1
            For I As Integer = 0 To 9
                If SoundPlayers(I).Active = False Then
                    SoundIndex = I
                    GoTo FoundPlayer
                End If
            Next
FoundPlayer:
            Dim Output As New SoundTask
            Output.Index = SoundIndex
            If SoundIndex > -1 Then
                SoundPlayers(SoundIndex).Path = Path
                Output.Player = SoundPlayers(SoundIndex).PlayAsync(SoundFile)
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
            Return Output
        End Function

        Public Sub StopSounds(Optional SoundIndex() As Integer = Nothing)
            If SoundIndex Is Nothing Then
                For I As Integer = 0 To SoundIndex.Length - 1
                    If SoundPlayers(SoundIndex(I)).Active = True Then
                        SoundPlayers(SoundIndex(I)).Stopp()
                    End If
                Next
            Else
                For I As Integer = 0 To SoundIndex.Length - 1
                    If SoundPlayers(SoundIndex(I)).Active = True Then
                        SoundPlayers(SoundIndex(I)).Stopp()
                    End If
                Next
            End If
        End Sub

    End Class

    Public Class SoundController

        Private MySound As SoundTask
        Private Stask As Task
        Private Path As String
        Private Controller As String
        Public Event SoundStarted()
        Public Event SoundStopped()
        'Private StoppingSound As Boolean
        'Private SoundStopped As TaskCompletionSource(Of Boolean)

        Public Sub New(Source As String, Optional FormName As String = "")
            MySound.Index = -1
            Path = Source
            Controller = FormName
        End Sub

        Public Async Function StopSound() As Task
            If IsActive() Then
                AudioControl.SoundPlayer.StopSounds({MySound.Index})
                Await Stask
            End If
        End Function

        Public Async Function PlaySound(SoundFile As String) As Task
            If IsActive() Then
                Await StopSound()
                Await Task.Delay(10)
            End If
            Stask = PlaySoundFile(SoundFile)
            Await Stask
        End Function

        Public Function IsActive() As Boolean
            If MySound.Index > -1 Then
                Return True
            Else
                Return False
            End If
        End Function

        Private Async Function PlaySoundFile(SoundFile As String) As Task
            MySound = AudioControl.SoundPlayer.PlaySound(SoundFile, Path, Controller)
            RaiseEvent SoundStarted()
            Await MySound.Player
            MySound.Index = -1
            RaiseEvent SoundStopped()
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
        Public Path As String
        Public Active As Boolean
        Public Pause As Boolean
        Public Access As Mutex
        Public Current As String
        Private AsyncTrigg As Boolean
        Private AsyncSigg As TaskCompletionSource(Of Boolean)
        Public Event Started()
        Public Event Stopped()
        Public Event Paused()



        Public Sub New(SourceName As String, SourcePath As String)
            Access = New Mutex
            Access.WaitOne()
            Name = SourceName
            Path = SourcePath
            Active = False
            Pause = False
            Access.ReleaseMutex()
            AsyncTrigg = False
            AddHandler OBS.MediaEnded, AddressOf mediastopped
            AddHandler OBS.MediaStarted, AddressOf mediastarted
            AddHandler OBS.MediaPaused, AddressOf mediapaused

        End Sub

        Private Sub mediastopped(sender As OBSWebsocket, medianame As String, mediatype As String)
            If medianame = Name Then
                Active = False
                Pause = False
                ResetMediaSource(Name)
                RaiseEvent Stopped()
                If AsyncTrigg = True Then AsyncSigg.SetResult(False)
            End If
        End Sub

        Private Sub mediastarted(sender As OBSWebsocket, medianame As String, mediatype As String)
            If medianame = Name Then
                Pause = False
                Active = True
                RaiseEvent Started()
            End If
        End Sub

        Private Sub mediapaused(sender As OBSWebsocket, medianame As String, mediatype As String)
            If medianame = Name Then
                Pause = True
                RaiseEvent Paused()
            End If
        End Sub

        Public Sub Stopp()
            Access.WaitOne()
            If Active = True Then
                Current = Replace(MediaSourceChange(Name, 3), Path, "")
                If InStr(Current, "(RND) ") Then
                    Dim SplitString() As String = Current.Split("\")
                    Current = SplitString(0)
                End If
            End If
            Access.ReleaseMutex()
        End Sub

        Public Sub Pausing()
            Access.WaitOne()
            If Pause = True Then
                Current = Replace(MediaSourceChange(Name, 1), Path, "")
                If InStr(Current, "(RND) ") Then
                    Dim SplitString() As String = Current.Split("\")
                    Current = SplitString(0)
                End If
            Else
                Current = Replace(MediaSourceChange(Name, 2), Path, "")
                If InStr(Current, "(RND) ") Then
                    Dim SplitString() As String = Current.Split("\")
                    Current = SplitString(0)
                End If
            End If
            Access.ReleaseMutex()
        End Sub

        Public Async Function PlayAsync(FileName As String) As Task
            Access.WaitOne()
            AsyncTrigg = True
            AsyncSigg = New TaskCompletionSource(Of Boolean)
            Current = Replace(MediaSourceChange(Name, , Path & FileName), Path, "")
            If InStr(Current, "(RND) ") Then
                Dim SplitString() As String = Current.Split("\")
                Current = SplitString(0)
            End If
            Access.ReleaseMutex()
            AsyncTrigg = Await AsyncSigg.Task
        End Function


        Public Sub Play(FileName As String)
            If AsyncTrigg = True Then AsyncSigg.SetResult(False)
            Access.WaitOne()
            Current = Replace(MediaSourceChange(Name, , Path & FileName), Path, "")
            If InStr(Current, "(RND) ") Then
                Dim SplitString() As String = Current.Split("\")
                Current = SplitString(0)
            End If
            Access.ReleaseMutex()
        End Sub

    End Class





    '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    '/////////////////////////////////////////////////////RESOURCE MANAGER////////////////////////////////////////////////////
    '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    'Public MyReourceManager As ResourceManager

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

        Public Const LastID As Integer = 11
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
        Public Event TaskEvent(TaskString As String, StateID As Integer)

        Public Sub New()
            QueueTex = New Mutex
            Rqueue = New List(Of ResourceRequest)
            ReDim Reservations(0 To ResourceIDs.LastID)
            For i As Integer = 0 To ResourceIDs.LastID
                Reservations(i) = False
            Next
        End Sub

        Private Function OKtoGO(RIDs() As Integer) As Boolean
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

        Public Async Function RequestResource(RIDs() As Integer, TaskToRun As Task(Of Task), TaskInfo As String,
                                         Optional RejectIfUnavailable As Boolean = False) As Task(Of Boolean)
            If OKtoGO(RIDs) Then
                SetRIDs(RIDs, True)
                RaiseEvent TaskEvent(TaskInfo, RMtaskStates.Started)
                If TaskToRun IsNot Nothing Then
                    TaskToRun.Start()
                    Await TaskToRun.Result
                End If
                RaiseEvent TaskEvent(TaskInfo, RMtaskStates.Ended)
                SetRIDs(RIDs, False)
            Else
                If RejectIfUnavailable Then
                    RaiseEvent TaskEvent(TaskInfo, RMtaskStates.Abandoned)
                    Return False
                Else
                    QueueTex.WaitOne()
                    Rqueue.Add(New ResourceRequest(RIDs, TaskToRun, TaskInfo, Rqueue.Count + 1))
                    Dim QueueCount As Integer = Rqueue.Count
                    QueueTex.ReleaseMutex()
                    RaiseEvent TaskEvent(TaskInfo & " (Queue# " & QueueCount & ")", RMtaskStates.Queued)
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
                        RaiseEvent TaskEvent(MyRequest.TaskString & " (Q#" & MyRequest.QID & ")", RMtaskStates.Started)
                        If MyRequest.MyTask IsNot Nothing Then
                            MyRequest.MyTask.Start()
                            Await MyRequest.MyTask.Result
                        End If
                        RaiseEvent TaskEvent(MyRequest.TaskString & " (Q#" & MyRequest.QID & ")", RMtaskStates.Ended)
                        SetRIDs(MyRequest.RIDs, False)
                        GoTo FromTheTop
                    End If
                Next
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
        Public Sub New(ResourceIDs() As Integer, TaskToRun As Task(Of Task), TaskTitle As String, QueueNum As Integer)
            RIDs = ResourceIDs
            MyTask = TaskToRun
            TaskString = TaskTitle
            QID = QueueNum
        End Sub
    End Class

End Module
