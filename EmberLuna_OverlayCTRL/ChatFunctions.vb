Imports System.Threading
Imports System.IO
Imports System.Collections.Concurrent
Public Module ChatFunctions

    Public ChatLogDirectory As String = "\\StreamPC-V2\OBS Assets\Chat Log"

    Public Class UserData

        Public ChatColor() As Color
        Public AllChatUsers As List(Of ChatUser)
        Public CurrentChatUsers As List(Of String)
        Public CurrentAccess As Mutex
        Public LogAccess As Mutex
        Public Event ChatUserDetected(UserName As String, NewUser As Boolean)
        Public Event MessageRecieved(UserName As String, MessageData As String, TimeString As String, ColorIndex As Integer)
        Public Colorizer As Integer

        Public Function GetColorIndex() As Integer
            If Colorizer > 19 Then
                Colorizer = 0
            Else
                Colorizer = Colorizer + 1
            End If
            Return Colorizer
            File.WriteAllText(ChatLogDirectory & "\" & "Color Index.txt", Colorizer)
        End Function

        Public Sub New()
            Dim NewChatColors(0 To 19) As Color
            InitializeColorizer()
            CurrentAccess = New Mutex
            LogAccess = New Mutex
            AllChatUsers = New List(Of ChatUser)
            CurrentChatUsers = New List(Of String)
            ImportAllUsers()
            ImportCurrentUsers()
            NewChatColors(0) = Color.FromArgb(255, 245, 252, 200)
            NewChatColors(1) = Color.FromArgb(255, 65, 172, 0)
            NewChatColors(2) = Color.FromArgb(255, 248, 135, 11)
            NewChatColors(3) = Color.FromArgb(255, 168, 164, 85)
            NewChatColors(4) = Color.FromArgb(255, 248, 94, 11)
            NewChatColors(5) = Color.FromArgb(255, 255, 176, 0)
            NewChatColors(6) = Color.FromArgb(255, 255, 25, 0)
            NewChatColors(7) = Color.FromArgb(255, 180, 220, 17)
            NewChatColors(8) = Color.FromArgb(255, 66, 113, 38)
            NewChatColors(9) = Color.FromArgb(255, 126, 182, 92)
            NewChatColors(10) = Color.FromArgb(255, 176, 111, 39)
            NewChatColors(11) = Color.FromArgb(255, 255, 0, 62)
            NewChatColors(12) = Color.FromArgb(255, 65, 175, 79)
            NewChatColors(13) = Color.FromArgb(255, 150, 74, 48)
            NewChatColors(14) = Color.FromArgb(255, 255, 229, 0)
            NewChatColors(15) = Color.FromArgb(255, 231, 119, 146)
            NewChatColors(16) = Color.FromArgb(255, 223, 237, 126)
            NewChatColors(17) = Color.FromArgb(255, 46, 101, 175)
            NewChatColors(18) = Color.FromArgb(255, 142, 20, 0)
            NewChatColors(19) = Color.FromArgb(255, 178, 59, 190)
            ChatColor = NewChatColors
        End Sub

        Private Sub InitializeColorizer()
            If File.Exists(ChatLogDirectory & "\" & "Color Index.txt") Then
                Dim InputString As String = File.ReadAllText(ChatLogDirectory & "\" & "Color Index.txt")
                If IsNumeric(InputString) Then
                    Colorizer = InputString
                Else
                    Colorizer = 0
                End If
            Else
                Colorizer = 0
            End If
        End Sub

        Private Function CheckAllChatUsers(InputUserName As String) As Integer
            For i As Integer = 0 To AllChatUsers.Count - 1
                If AllChatUsers(i).UserName = InputUserName Then
                    Return i
                    Exit Function
                End If
            Next
            Return -1
        End Function

        Private Sub LogChatMessage(Username As String, ChatString As String, TimeString As String)
            LogAccess.WaitOne()
            Dim FileString As String = "\" & DateFileString() & ".txt"
            If File.Exists(ChatLogDirectory & FileString) = False Then File.Create(ChatLogDirectory & FileString).Dispose()
            Dim Writer As StreamWriter = File.AppendText(ChatLogDirectory & FileString)
            Writer.WriteLine(TimeString & ": " & Username & ": " & ChatString)
            Writer.Close()
            Writer.Dispose()
            LogAccess.ReleaseMutex()
        End Sub

        Public Sub RecieveChatMessage(UserName As String, ChatString As String, TimeString As String)
            Dim ChatUserIndex As Integer = CheckAllChatUsers(UserName)
            LogChatMessage(UserName, ChatString, TimeString)
            If ChatUserIndex < 0 Then
                Dim ChatUserData As New ChatUser
                ChatUserData.CreateUserData(UserName, GetColorIndex)
                ChatUserData.AppendChatLog(ChatString, TimeString)
                AllChatUsers.Add(ChatUserData)
                ChatUserIndex = AllChatUsers.Count - 1
                AppendCurrentUsers(UserName)
                RaiseEvent ChatUserDetected(UserName, True)
            Else
                AllChatUsers(ChatUserIndex).AppendChatLog(ChatString, TimeString)
                If CurrentChatUsers.Contains(UserName) = False Then
                    AppendCurrentUsers(UserName)
                    RaiseEvent ChatUserDetected(UserName, False)
                End If
            End If
            RaiseEvent MessageRecieved(UserName, ChatString, TimeString, AllChatUsers(ChatUserIndex).ColorIndex)
        End Sub

        Public Sub ResetCurrentUsers()
            CurrentAccess.WaitOne()
            CurrentChatUsers = New List(Of String)
            File.Create(ChatLogDirectory & "\Current Chat Users.txt").Dispose()
            CurrentAccess.ReleaseMutex()
        End Sub

        Public Sub AppendCurrentUsers(UserName As String)
            CurrentAccess.WaitOne()
            If File.Exists(ChatLogDirectory & "\Current Chat Users.txt") = False Then
                File.Create(ChatLogDirectory & "\Current Chat Users.txt").Dispose()
            End If
            Dim Writer As StreamWriter = File.AppendText(ChatLogDirectory & "\Current Chat Users.txt")
            Writer.WriteLine(UserName)
            Writer.Close()
            Writer.Dispose()
            CurrentChatUsers.Add(UserName)
            CurrentAccess.ReleaseMutex()
        End Sub

        Public Sub ImportCurrentUsers()
            CurrentAccess.WaitOne()
            If File.Exists(ChatLogDirectory & "\Current Chat Users.txt") Then
                Dim Reader As StreamReader = File.OpenText(ChatLogDirectory & "\Current Chat Users.txt")
                Do Until Reader.EndOfStream = True
                    CurrentChatUsers.Add(Reader.ReadLine)
                Loop
            End If
            CurrentAccess.ReleaseMutex()
        End Sub

        Public Sub ImportAllUsers()
            Dim ReplaceSearch As String = ChatLogDirectory & "\"
            Dim NameString As String = ""
            Dim ChatUserData As New ChatUser
            For Each Dir As String In Directory.GetDirectories(ChatLogDirectory)
                NameString = Replace(Dir, ReplaceSearch, "")
                ChatUserData.ImportUserData(NameString)
                AllChatUsers.Add(ChatUserData)
            Next
        End Sub

    End Class

    Public Structure ChatUser

        Public ColorIndex As Integer
        Public UserName As String
        'Public UserID As String
        Public Access As Mutex


        Public Sub AddJHpoints(PointValue As String)
            Dim PointsValue As Double = GetJHpoints()
            PointsValue = PointsValue + PointValue
            File.WriteAllText(ChatLogDirectory & "\" & UserName & "\JHpoints.txt", PointsValue.ToString)
        End Sub

        Public Function GetJHpoints() As Double
            If Access Is Nothing Then Access = New Mutex
            Access.WaitOne()
            Dim PointsValue As Double, InputString As String
            InputString = File.ReadAllText(ChatLogDirectory & "\" & UserName & "\JHpoints.txt")
            If IsNumeric(InputString) Then
                PointsValue = InputString
            Else
                PointsValue = 0
            End If
            Access.ReleaseMutex()
            Return PointsValue
        End Function

        Public Sub CreateUserData(InputUserName As String, InputColorIndex As Integer)
            If Access Is Nothing Then Access = New Mutex
            Access.WaitOne()
            UserName = InputUserName
            My.Computer.FileSystem.CreateDirectory(ChatLogDirectory & "\" & UserName)
            ColorIndex = InputColorIndex
            File.WriteAllText(ChatLogDirectory & "\" & UserName & "\ColorIndex.txt", ColorIndex)
            File.WriteAllText(ChatLogDirectory & "\" & UserName & "\JHpoints.txt", 0)
            File.Create(ChatLogDirectory & "\" & UserName & "\ChatLog.txt").Dispose()
            Access.ReleaseMutex()
        End Sub


        Public Sub AppendChatLog(ChatString As String, TimeString As String)
            If Access Is Nothing Then Access = New Mutex
            Access.WaitOne()
            Dim ChatWriter As StreamWriter = File.AppendText(ChatLogDirectory & "\" & UserName & "\ChatLog.txt")
            ChatWriter.WriteLine(TimeString & ": " & ChatString)
            ChatWriter.Close()
            ChatWriter.Dispose()
            Access.ReleaseMutex()
        End Sub

        Public Sub ImportUserData(Optional InputUserName As String = "")
            If Access Is Nothing Then Access = New Mutex
            Access.WaitOne()
            If InputUserName <> "" Then UserName = InputUserName
            Dim ColorString = File.ReadAllText(ChatLogDirectory & "\" & UserName & "\ColorIndex.txt")
            If IsNumeric(ColorString) Then ColorIndex = ColorString
            Access.ReleaseMutex()
        End Sub

    End Structure


End Module
