Imports OBSWebsocketDotNet
Imports OBSWebsocketDotNet.Types
Imports System.Net.Sockets
Imports System.Threading
Imports System.IO
Imports TwitchLib.PubSub
Public Module CommonFunctions
    Public Declare Function SetDrawing Lib "user32" Alias "SendMessageA" (ByVal hwndLock As IntPtr, ByVal Msg As Integer, ByVal wParam As Boolean, ByVal lParam As IntPtr) As Integer
    Public Const WM_SETREDRAW As Integer = 11

    Public PubSub As TwitchPubSub
    Public OBS As OBSWebsocket
    Public SceneChanger As SceneSelector
    Public ChatManager As Chat
    Public SourceWindow As MainWindow
    Public OBSTimers As TimerControls
    Public TimerCollection As OBSTimerData
    Public MusicPlayer As OBSMusicPlayer
    Public SoundBoard As OBSSoundBoard
    Public Counters As OBScounters
    Public SpriteControls As OBScharacters
    Public GamesList As TwitchGames
    Public ChannelPointsDisplay As ChannelPointsForm
    Public WithEvents ChannelPoints As ChannelPointData
    Public WithEvents myAPI As APIfunctions
    Public WithEvents Ember As CharacterControls
    Public WithEvents Luna As CharacterControls
    Public WithEvents AudioControl As OBSaudioPlayer
    Public WithEvents CounterData As OBScounterData
    Public WithEvents ChatUserInfo As UserData

    'COLORS
    Public ActiveBUTT As Color = Color.FromArgb(255, 255, 82, 0)
    Public StandardBUTT As Color = Color.FromArgb(255, 52, 52, 52)
    Public BaseColor As Color = Color.FromArgb(255, 31, 31, 31)
    Public SoundAlertDisplay As Boolean = False

    Public Structure TypeIDs
        Public Const Glob As Integer = 0
        Public Const Ember As Integer = 1
        Public Const Luna As Integer = 2
        Public Const EandL As Integer = 2
        Public Const Doggo As Integer = 3
        Public Const Exercise As Integer = 4
        Public Const Gaming As Integer = 5
        Public Const Art As Integer = 6
    End Structure

    Public Function TypeName(TypeInt As Integer) As String
        Select Case TypeInt
            Case = TypeIDs.Glob
                Return "GLOBAL"
            Case = TypeIDs.Ember
                Return "EMBER"
            Case = TypeIDs.Luna
                Return "LUNA"
            Case = TypeIDs.EandL
                Return "EMBER LUNA"
            Case = TypeIDs.Doggo
                Return "DOGGO"
            Case = TypeIDs.Exercise
                Return "EXERCISE"
            Case = TypeIDs.Gaming
                Return "GAMING"
            Case = TypeIDs.Art
                Return "ART"
            Case Else
                Return ""
        End Select
    End Function

    Public OBSmutex As Mutex


    Public Function GetFormHeight(ButtonCount As Integer, ButtonsWide As Integer, ButtonsHigh As Integer,
                                  BaseHeight As Integer, HeightIncrement As Integer) As Integer
        Dim OutputHeight As Integer = BaseHeight - HeightIncrement
        For I As Integer = 0 To ButtonsHigh - 1
            If ButtonCount > ButtonsWide * I Then OutputHeight = OutputHeight + HeightIncrement
        Next
        Return OutputHeight
    End Function

    Public Function RandomString(Category As String, Optional Directory As String = "\\StreamPC-V2\OBS Assets\Messages") As String
        Dim Reader As StreamReader = File.OpenText(Directory & "\" & Category & ".txt")
        Dim InputList As New List(Of String)
        Do Until Reader.EndOfStream = True
            InputList.Add(Reader.ReadLine())
        Loop
        Return InputList(RandomInt(0, InputList.Count - 1))
    End Function

    Public Function RandomMessage(UserName As String, MessageType As String) As String
        Dim OutputString As String = RandomString(MessageType)
        OutputString = Replace(OutputString, "[user]", UserName)
        If InStr(OutputString, "[food]") <> 0 Then
            OutputString = Replace(OutputString, "[food]", RandomString("Food"))
        End If
        Return OutputString
    End Function

    Public Function RandomInt(Lbound As Integer, Ubound As Integer) As Integer
        Dim Thagomizer As Random = New Random
        Dim Output As Integer = Thagomizer.Next(Lbound, Ubound + 1)
        Return Output
    End Function

    Public Function DateFileString() As String
        Dim InputString As String = DateAndTime.Now.ToString
        Dim SplitString() As String = InputString.Split(" ")
        InputString = Replace(SplitString(0), "/", "-")
        Return InputString
    End Function

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

    Function Shuffle(Of T)(collection As IEnumerable(Of T)) As List(Of T)
        Dim r As Random = New Random()
        Shuffle = collection.OrderBy(Function(a) r.Next()).ToList()
    End Function

    Public Sub sendlist(InputList As List(Of String))
        Dim inputstring As String = ""
        For i As Integer = 0 To InputList.Count - 1
            inputstring = inputstring & vbCrLf & InputList(i)
        Next
        SendMessage(inputstring)
    End Sub

    Public Function TimeString(TimeInSeconds As Integer, Optional FormatString As String = "mm:ss") As String
        Dim Time As DateTime
        Time = New DateTime(TimeSpan.FromSeconds(TimeInSeconds).Ticks)
        Return Time.ToString(FormatString)
    End Function

    Public Structure MediaFcode
        Public Const Play As Integer = 1
        Public Const Pause As Integer = 2
        Public Const Stopp As Integer = 3
        Public Const Restart As Integer = 4
    End Structure


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

    Public Function OBSstreamState() As Boolean
        OBSmutex.WaitOne()
        If OBS.GetStreamingStatus.IsStreaming = True Then
            OBSmutex.ReleaseMutex()
            Return True
        Else
            OBSmutex.ReleaseMutex()
            Return False
        End If
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
                SendMessage(OBSsocketString & " " & OBSsocketPassword)
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

    Public Function DataExtract(InputText As String, PreTEXT As String, PostTEXT As String, Optional InstanceCT As Integer = 1) As String
        Dim LOC1 As Integer, LOC2 As Integer, PTLEN As Integer

        PTLEN = Len(PreTEXT)
        LOC1 = InStr(InputText, PreTEXT)
        If LOC1 = 0 Then
            DataExtract = ""
        Else
            If InstanceCT > 1 Then
                Do Until InstanceCT = 1
                    If LOC1 <> 0 Then LOC1 = InStr(LOC1 + PTLEN, InputText, PreTEXT)
                    InstanceCT = InstanceCT - 1
                Loop
            End If
            If LOC1 <> 0 Then
                LOC2 = InStr(LOC1 + PTLEN, InputText, PostTEXT)
                If LOC2 = 0 Then
                    DataExtract = ""
                Else
                    DataExtract = Mid(InputText, LOC1 + PTLEN, LOC2 - LOC1 - PTLEN)
                End If
            Else
                DataExtract = ""
            End If
        End If
    End Function

    Public Sub SendMessage(MessageText As String, Optional TitleString As String = "Something Happened!!")
        Dim MessageSender As Thread
        MessageSender = New Thread(AddressOf Messenger)
        MessageSender.Start(MessageText & ":<>:" & TitleString)
    End Sub

    Private Sub Messenger(MessageText As String)
        Dim SplitString() As String
        SplitString = Split(MessageText, ":<>:")
        MsgBox(SplitString(0), , SplitString(1))
    End Sub

    Public Function MPstringFormat(Inputstring As String) As String
        Dim OutputString As String = Replace(Inputstring, ".wav", "")
        OutputString = Replace(OutputString, ".mp3", "")
        OutputString = Replace(OutputString, "ES_", "")
        Return OutputString
    End Function

    Public Class ToTcpClient
        Inherits TcpClient
        Public tmr As Timer
        Public id As String = ""
        Public Event TimeoutReached(ByVal sender As ToTcpClient)
        Public Sub BeginConnectWithTimeout(ByVal host As String,
                                           ByVal port As Integer,
                                           ByVal ConnectCallback As AsyncCallback,
                                           Optional ByVal timeout As Integer = 500)
            BeginConnect(host, port, ConnectCallback, Me)
            tmr = New Timer(AddressOf ToTcpClient_TimeoutReached,
                Nothing, timeout, Threading.Timeout.Infinite)
        End Sub

        Private Sub ToTcpClient_TimeoutReached(ByVal e As Object)
            RaiseEvent TimeoutReached(Me)
        End Sub
    End Class


    Public Class IrcClient


        Public UserName As String
        Private Channel As String

        Private VarTcpClient As TcpClient
        Private Inputstream As StreamReader
        Private Outputstream As StreamWriter

        Public Sub New(IPaddress As String, IntPort As Integer, ThisUserName As String, Password As String, ThisChannel As String)

            Try
                UserName = ThisUserName
                Channel = ThisChannel

                VarTcpClient = New TcpClient(IPaddress, IntPort)
                Inputstream = New StreamReader(VarTcpClient.GetStream())
                Outputstream = New StreamWriter(VarTcpClient.GetStream())

                Outputstream.WriteLine("PASS " & Password)
                Outputstream.WriteLine("NICK " & UserName)
                Outputstream.WriteLine("USER " & UserName & " 8 * :" & UserName)
                Outputstream.WriteLine("JOIN #" & Channel)
                Outputstream.Flush()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try

        End Sub




        Public Sub SendIrcMessage(InputString As String)
            Try
                Outputstream.WriteLine(InputString)
                Outputstream.Flush()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End Sub

        Public Sub SendPublicChatMessage(InputString As String)
            Try
                SendIrcMessage(":" & UserName & "!" & UserName & "@" + UserName +
                    ".tmi.twitch.tv PRIVMSG #" & Channel & " :" + InputString)
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End Sub

        Public Function ReadMessage() As String
            Dim Outputstring As String = "" ', IntWat As Integer
            Try
                Outputstring = Inputstream.ReadLine
                Return Outputstring
            Catch ex As Exception
                Return "Error recieving message: " & ex.Message
            End Try
        End Function

        Public Sub Close()

            Outputstream.Close()
            Outputstream.Dispose()

            Inputstream.Close()
            Inputstream.Dispose()

            VarTcpClient.Close()
            VarTcpClient.Dispose()

        End Sub

    End Class

    Public Class PingSender

        Private ThisIRC As IrcClient
        Private PingSenderThread As Thread

        Public Sub New(IRC As IrcClient)
            ThisIRC = IRC
            PingSenderThread = New Thread(New ThreadStart(AddressOf Run))
        End Sub

        Public Sub Start()
            PingSenderThread.IsBackground = True
            PingSenderThread.Start()
        End Sub

        Public Sub Run()
            Do While True
                ThisIRC.SendIrcMessage("PING irc.twitch.tv")
                Thread.Sleep(300000)
            Loop
        End Sub

        Public Sub StopPing()
            'PingSenderThread.Suspend()
            PingSenderThread.Abort()
        End Sub
    End Class

End Module
