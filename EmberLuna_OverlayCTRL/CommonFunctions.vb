Imports System.Net.Sockets
Imports System.Threading
Imports System.IO
Imports TwitchLib.PubSub
Imports System.Net
Imports System.Text
Imports System.Collections.Concurrent

Public Module CommonFunctions
    Public Declare Function SetDrawing Lib "user32" Alias "SendMessageA" (ByVal hwndLock As IntPtr, ByVal Msg As Integer, ByVal wParam As Boolean, ByVal lParam As IntPtr) As Integer
    Public Const WM_SETREDRAW As Integer = 11

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
    Public WithEvents DemoPlayer As DemoModule

    Public WithEvents ChannelPoints As ChannelPointData
    Public WithEvents myAPI As APIfunctions
    Public WithEvents Ember As CharacterControls
    Public WithEvents Luna As CharacterControls
    Public WithEvents AudioControl As OBSaudioPlayer
    Public WithEvents CounterData As OBScounterData
    Public WithEvents ChatUserInfo As UserData
    Public WithEvents IRC As IrcClient
    Public WithEvents MyResourceManager As ResourceManager
    Public WithEvents MySceneCollection As SceneCollection
    Public WithEvents MyOBSevents As OBSevents

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

    Public Function RandomUsername() As String
        Return ChatUserInfo.AllChatUsers(RandomInt(0, ChatUserInfo.AllChatUsers.Count - 1)).UserName
    End Function

    Public Function ConvertStringToInteger(input As String) As Integer
        Dim output As Integer
        Integer.TryParse(input, output)
        Return output
    End Function

    Public Function ConvertStringToBool(input As String) As Boolean
        Dim output As Boolean
        Boolean.TryParse(input, output)
        Return output
    End Function

    Public Sub TextBoxScrollToEnd(Source As TextBox)
        Source.SelectionStart = Source.TextLength
        Source.ScrollToCaret()
    End Sub

    Public Function TextBoxLineRemover(Source As TextBox) As String
        Dim OutputString As String
        If Source.Text <> "" Then
            If Source.SelectedText <> "" Then
                OutputString = Source.SelectedText
                Source.SelectionLength = Source.SelectionLength + 2
                Source.SelectedText = ""
                Return OutputString
            Else
                If InStr(Source.Text, vbCrLf) <> 0 Then
                    Dim InputSTring() As String = Split(Source.Text, vbCrLf)
                    OutputString = InputSTring.Last
                    Array.Resize(InputSTring, InputSTring.Length - 1)
                    Source.Text = String.Join(vbCrLf, InputSTring)
                    Return OutputString
                Else
                    OutputString = Source.Text
                    Source.Text = ""
                    Return OutputString
                End If
            End If
        Else
            Return ""
        End If
    End Function

    Public Function TextboxLineSelector(Source As TextBox) As String
        If Source.SelectionStart <> 0 Then
            Dim StartPoint As Integer = InStrRev(Source.Text, vbCrLf, Source.SelectionStart) + 1
            If StartPoint = 1 Then StartPoint = 0
            Dim EndPoint As Integer = InStr(Source.SelectionStart, Source.Text, vbCrLf) - 1
            If EndPoint = -1 Then EndPoint = Source.Text.Length
            Source.SelectionStart = StartPoint
            Source.SelectionLength = EndPoint - StartPoint
            Return Source.SelectedText
        Else
            Return ""
        End If
    End Function

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

    Public Function RandomMessage(UserName As String, MessageType As String, Optional Amount As String = "") As String
        Dim OutputString As String = RandomString(MessageType)

        OutputString = Replace(OutputString, "[user]", UserName)

        If Amount <> "" Then OutputString = Replace(OutputString, "[amount]", Amount)

        If InStr(OutputString, "[food]") <> 0 Then
            OutputString = Replace(OutputString, "[food]", RandomString("Food"))
        End If

        Return OutputString
    End Function

    Public Function RandomInt(Lbound As Integer, Ubound As Integer) As Integer
        Static Thagomizer As Random = New Random
        Dim Output As Integer = Thagomizer.Next(Lbound, Ubound + 1)
        Return Output
    End Function

    Public Function DateFileString() As String
        Dim InputString As String = DateAndTime.Now.ToString
        Dim SplitString() As String = InputString.Split(" ")
        InputString = Replace(SplitString(0), "/", "-")
        Return InputString
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


    Public Class ExternalConnections
        Private WithEvents EmberRemote As EmberLunaControlServer
        Private WithEvents LunaRemote As EmberLunaControlServer
        Private WithEvents EmberSprite As EmberLunaControlServer
        Private WithEvents LunaSprite As EmberLunaControlServer

        Private EmberSwaiting As Boolean
        Private EmberSresponseRecieved As TaskCompletionSource(Of Boolean)

        Public Sub New()
            EmberRemote = New EmberLunaControlServer(8001)
            LunaRemote = New EmberLunaControlServer(8002)
            EmberSprite = New EmberLunaControlServer(8003)
            LunaSprite = New EmberLunaControlServer(8004)
        End Sub

        Public Sub EmberSdataRecieved(DataString As String) Handles EmberSprite.DataReceived
            If EmberSwaiting Then EmberSresponseRecieved.SetResult(False)
        End Sub

        Public Async Function EmberSpriteMood(MoodString As String) As Task
            EmberSwaiting = True
            EmberSresponseRecieved = New TaskCompletionSource(Of Boolean)
            EmberSprite.SendString(MoodString)
            EmberSwaiting = Await EmberSresponseRecieved.Task
        End Function


    End Class

    Public Class EmberLunaControlServer

        Public Event ServerEvent(EventString As String)
        Public Event ConnectionEstablished(IPaddress As String, PortNumber As Integer)
        Public Event DataReceived(DataString As String)
        Private RunServer As Boolean = False
        Private ConnectionAlive As Boolean = False
        Private StringQueue As ConcurrentQueue(Of String)
        Private RunTask As Task

        Public Sub New(PortNumber As Integer)
            StringQueue = New ConcurrentQueue(Of String)
            RunServer = True
            ConnectionAlive = False
            RunTask = RuntcpServer(PortNumber)
        End Sub

        Public Function IsAlive() As Boolean
            Return ConnectionAlive
        End Function

        Public Async Function CloseServer() As Task
            RunServer = False
            Await RunTask
        End Function

        Public Sub SendString(StringToSend As String)
            StringQueue.Enqueue(StringToSend)
        End Sub


        Private Async Function RuntcpServer(PortNumber As Integer) As Task
            Dim serverSocket As New TcpListener(Net.IPAddress.Any, PortNumber)
            Dim hostname As String = Dns.GetHostName()
            Dim iphe As System.Net.IPHostEntry = System.Net.Dns.GetHostEntry(hostname)
            Dim STRipaddress As String = "", SendString As String


            For Each ipheal As System.Net.IPAddress In iphe.AddressList
                If ipheal.AddressFamily = System.Net.Sockets.AddressFamily.InterNetwork Then
                    STRipaddress = ipheal.ToString()
                End If
            Next

            Dim clientSocket As TcpClient
            Dim MynetworkStream As NetworkStream
            Dim bytesFrom() As Byte
            Dim dataFromClient As String
            Dim SendBytes() As Byte

            serverSocket.Start()
            RaiseEvent ConnectionEstablished(STRipaddress, PortNumber)
            RaiseEvent ServerEvent("-Server Started: " & STRipaddress & ":" & PortNumber & vbCrLf)
ReconnectClient:
            Do Until serverSocket.Pending = True
                If RunServer = False Then GoTo StopServer
                Await Task.Delay(20)
            Loop
            clientSocket = Await serverSocket.AcceptTcpClientAsync()
            RaiseEvent ServerEvent("-Connected to: " & clientSocket.Client.RemoteEndPoint.ToString & vbCrLf)
            Try
                MynetworkStream = clientSocket.GetStream()
                ConnectionAlive = True
            Catch ex As Exception
                MsgBox("-Connection fail: " & ex.ToString)
                GoTo didntwork
            End Try

            While RunServer And MynetworkStream.CanRead And serverSocket.Pending = False 'And serverSocket.Pending = False And MynetworkStream.CanRead
                Try
                    If MynetworkStream.DataAvailable = True Then
                        dataFromClient = ""
                        Await MynetworkStream.ReadAsync(bytesFrom, 0, clientSocket.Available)
                        For Each DataByte In bytesFrom
                            dataFromClient &= Chr(DataByte)
                        Next
                        RaiseEvent DataReceived(dataFromClient)
                        RaiseEvent ServerEvent("-Recieved: " & dataFromClient & vbCrLf)
                    Else
                        If StringQueue.Count > 0 Then
                            Do Until StringQueue.Count = 0
                                If StringQueue.TryDequeue(SendString) Then
                                    If SendString <> "" Then
                                        SendBytes = Encoding.ASCII.GetBytes(SendString & vbCrLf)
                                        MynetworkStream.Write(SendBytes, 0, SendBytes.Length)
                                        RaiseEvent ServerEvent("-Sent: " & SendString & vbCrLf)
                                        SendString = ""
                                    End If
                                End If
                            Loop
                        Else
                            Await Task.Delay(20)
                        End If
                    End If
                Catch ex As Exception
                    MsgBox("socket fail: " & ex.ToString)
                    GoTo didntwork
                End Try
            End While

            If RunServer = True Then
                ConnectionAlive = False
                clientSocket.Close()
                RaiseEvent ServerEvent("-Disconnected" & vbCrLf)
                GoTo ReconnectClient
            End If
didntwork:
            ConnectionAlive = False
            clientSocket.Close()
            RaiseEvent ServerEvent("-Disconnected" & vbCrLf)
StopServer:
            serverSocket.Stop()
            RaiseEvent ServerEvent("-Server Stopped")
            Await Task.Delay(100)
        End Function
    End Class

End Module
