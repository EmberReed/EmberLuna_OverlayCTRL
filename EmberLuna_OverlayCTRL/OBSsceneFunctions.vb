Imports OBSWebsocketDotNet
Imports OBSWebsocketDotNet.Types
Imports System.Net.Sockets
Imports System.Threading
Imports System.IO
Imports TwitchLib.PubSub
Public Module OBSsceneFunctions

    Public CurrentScene As OBSScene
    Public EmberAwayB As Boolean = False
    Public LunaAwayB As Boolean = False
    Public Screen2Enabled As Boolean = False
    Public EmberCamera As Boolean = True
    Public LunaCamera As Boolean = True
    Public EmberSpriteB As Boolean = True
    Public LunaSpriteB As Boolean = True
    Public Screen1Setting As Integer = ScreenSetting.EmberPC
    Public Screen2Setting As Integer = ScreenSetting.LunaPC
    Public Cam1Setting As Integer = ScreenSetting.EmberCam
    Public Cam2Setting As Integer = ScreenSetting.LunaCam

    Public OBSinputs() As OBSinput
    Public InputsInitialized As Boolean = False

    Public Screen1AwayMode As Boolean = False
    Public Screen2AwayMode As Boolean = False
    Public SceneAccess As Mutex

    Public SceneChangeInProgress As Boolean = False

    Public Event SceneChangeInitiated()
    Public Event SceneChangeCompleted()

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

    Public Sub InitializeInputs()
        If InputsInitialized = False Then
            ReDim OBSinputs(0 To 7)
            OBSinputs(ScreenSetting.EmberPC).ObjectName = "Ember's PC"
            OBSinputs(ScreenSetting.EmberPC).DisplayName = "Ember's PC"

        End If
    End Sub


    Public Sub SceneChangeDetected()
        If SceneChangeInProgress = True Then
            SceneChangeInProgress = False
            'SceneAccess.ReleaseMutex()
            RaiseEvent SceneChangeCompleted()
        End If
    End Sub

    Public Sub UpdateSceneDisplay(Optional ChangeSceneString As String = "")

        'If SceneAccess Is Nothing Then SceneAccess = New Mutex
        'SceneAccess.WaitOne()
        SceneChangeInProgress = True
        RaiseEvent SceneChangeInitiated()

        Dim ItemsList As List(Of OBSWebsocketDotNet.Types.SceneItem)
        Dim Container As OBSWebsocketDotNet.Types.SceneItemProperties
        Dim CurrentSceneList As OBSWebsocketDotNet.Types.GetSceneListInfo

        OBSmutex.WaitOne()
        CurrentScene = OBS.GetCurrentScene
        Dim SceneName As String = CurrentScene.Name

        CurrentSceneList = OBS.GetSceneList()
        For Each ActiveScene In CurrentSceneList.Scenes
            Select Case ActiveScene.Name
                Case = "Cam 1"
                    For Each SceneItem In ActiveScene.Items
                        Select Case SceneItem.SourceName
                            Case = "Ember's PC"
                                Container = OBS.GetSceneItemProperties(SceneItem.SourceName, ActiveScene.Name)
                                If Cam1Setting = ScreenSetting.EmberPC Then
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
                                If Cam1Setting = ScreenSetting.LunaPC Then
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
                                If Cam1Setting = ScreenSetting.EmberCam Then
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
                                If Cam1Setting = ScreenSetting.LunaCam Then
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
                                If Cam1Setting = ScreenSetting.Aux1 Then
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
                                If Cam1Setting = ScreenSetting.Aux2 Then
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
                                If Cam1Setting = ScreenSetting.Aux3 Then
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
                                If Cam1Setting = ScreenSetting.Aux4 Then
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
                            Case = "Ember Is Away"
                                Container = OBS.GetSceneItemProperties(SceneItem.SourceName, ActiveScene.Name)
                                If EmberAwayB = True Then
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

                Case = "Screen 1"
                    For Each SceneItem In ActiveScene.Items
                        Select Case SceneItem.SourceName
                            Case = "Ember's PC"
                                Container = OBS.GetSceneItemProperties(SceneItem.SourceName, ActiveScene.Name)
                                If Screen1Setting = ScreenSetting.EmberPC Then
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
                                If Screen1Setting = ScreenSetting.LunaPC Then
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
                                If Screen1Setting = ScreenSetting.EmberCam Then
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
                                If Screen1Setting = ScreenSetting.LunaCam Then
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
                                If Screen1Setting = ScreenSetting.Aux1 Then
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
                                If Screen1Setting = ScreenSetting.Aux2 Then
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
                                If Screen1Setting = ScreenSetting.Aux3 Then
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
                                If Screen1Setting = ScreenSetting.Aux4 Then
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
                            Case = "Jerin Montage"
                                Container = OBS.GetSceneItemProperties(SceneItem.SourceName, ActiveScene.Name)
                                If Screen1AwayMode <> Container.Visible Then
                                    If Screen1AwayMode = True Then
                                        Container.Visible = True
                                        OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                                    Else
                                        Container.Visible = False
                                        OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                                    End If
                                End If
                        End Select
                    Next
                Case = "Cam 2"
                    For Each SceneItem In ActiveScene.Items
                        Select Case SceneItem.SourceName
                            Case = "Ember's PC"
                                Container = OBS.GetSceneItemProperties(SceneItem.SourceName, ActiveScene.Name)
                                If Cam2Setting = ScreenSetting.EmberPC Then
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
                                If Cam2Setting = ScreenSetting.LunaPC Then
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
                                If Cam2Setting = ScreenSetting.EmberCam Then
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
                                If Cam2Setting = ScreenSetting.LunaCam Then
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
                                If Cam2Setting = ScreenSetting.Aux1 Then
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
                                If Cam2Setting = ScreenSetting.Aux2 Then
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
                                If Cam2Setting = ScreenSetting.Aux3 Then
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
                                If Cam2Setting = ScreenSetting.Aux4 Then
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
                            Case = "Luna Is Away"
                                Container = OBS.GetSceneItemProperties(SceneItem.SourceName, ActiveScene.Name)
                                If LunaAwayB = True Then
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
                Case = "Screen 2"
                    For Each SceneItem In ActiveScene.Items
                        Select Case SceneItem.SourceName
                            Case = "Ember's PC"
                                Container = OBS.GetSceneItemProperties(SceneItem.SourceName, ActiveScene.Name)
                                If Screen2Setting = ScreenSetting.EmberPC Then
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
                                If Screen2Setting = ScreenSetting.LunaPC Then
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
                                If Screen2Setting = ScreenSetting.EmberCam Then
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
                                If Screen2Setting = ScreenSetting.LunaCam Then
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
                                If Screen2Setting = ScreenSetting.Aux1 Then
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
                                If Screen2Setting = ScreenSetting.Aux2 Then
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
                                If Screen2Setting = ScreenSetting.Aux3 Then
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
                                If Screen2Setting = ScreenSetting.Aux4 Then
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
                            Case = "Jerin Montage"
                                Container = OBS.GetSceneItemProperties(SceneItem.SourceName, ActiveScene.Name)
                                If Screen2AwayMode <> Container.Visible Then
                                    If Screen2AwayMode = True Then
                                        Container.Visible = True
                                        OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                                    Else
                                        Container.Visible = False
                                        OBS.SetSceneItemProperties(Container, ActiveScene.Name)
                                    End If
                                End If
                        End Select
                    Next
                Case Else
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
            OBS.SetCurrentScene(ChangeSceneString)
        Else
            SceneChangeInProgress = False
        End If

        OBSmutex.ReleaseMutex()

    End Sub

End Module
