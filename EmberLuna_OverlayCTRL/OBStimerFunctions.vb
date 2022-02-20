
Imports OBSWebsocketDotNet
Imports OBSWebsocketDotNet.Types
Imports System.Net.Sockets
Imports System.Threading
Imports System.IO
Imports TwitchLib.PubSub

Public Module OBStimerFunctions
    Public OBStimerObject() As OBSTimer
    Public Event TimersUpdated(TimerType As Integer)

    Public Structure OBSTimer
        Public State As Boolean
        Public Pause As Boolean
        Public Title As String
        Public TitleSource As String
        Public Clock As String
        Public ClockSource As String
        Public Sounds As List(Of TimerSound)
        'Public Alarm As String
        Public SoundSource As String
        Public Event TimerStarted()
        Public Event TimerStopped()
        Public Sub StartTimer()
            RaiseEvent TimerStarted()
        End Sub
        Public Sub StopTimer()
            RaiseEvent TimerStopped()
        End Sub
    End Structure

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

        OBStimerObject(TimerIDs.GlobalCC).State = False
        OBStimerObject(TimerIDs.GlobalCC).Pause = False
        OBStimerObject(TimerIDs.GlobalCC).Title = ""
        OBStimerObject(TimerIDs.GlobalCC).TitleSource = "GlobalTimerTitle"
        OBStimerObject(TimerIDs.GlobalCC).Clock = "5:00"
        OBStimerObject(TimerIDs.GlobalCC).ClockSource = "GlobalTimerClock"
        'OBStimerObject(TimerIDs.GlobalCC).Alarm = ""
        OBStimerObject(TimerIDs.GlobalCC).Sounds = New List(Of TimerSound)
        OBStimerObject(TimerIDs.GlobalCC).SoundSource = "TimerAudio"

        OBStimerObject(TimerIDs.Ember).State = False
        OBStimerObject(TimerIDs.Ember).Pause = False
        OBStimerObject(TimerIDs.Ember).Title = ""
        OBStimerObject(TimerIDs.Ember).TitleSource = "EmberTimerTitle"
        OBStimerObject(TimerIDs.Ember).Clock = "5:00"
        OBStimerObject(TimerIDs.Ember).ClockSource = "EmberTimerClock"
        'OBStimerObject(TimerIDs.Ember).Alarm = ""
        OBStimerObject(TimerIDs.Ember).Sounds = New List(Of TimerSound)
        OBStimerObject(TimerIDs.Ember).SoundSource = "TimerAudio"

        OBStimerObject(TimerIDs.Luna).State = False
        OBStimerObject(TimerIDs.Luna).Pause = False
        OBStimerObject(TimerIDs.Luna).Title = ""
        OBStimerObject(TimerIDs.Luna).TitleSource = "LunaTimerTitle"
        OBStimerObject(TimerIDs.Luna).Clock = "5:00"
        OBStimerObject(TimerIDs.Luna).ClockSource = "LunaTimerClock"
        'OBStimerObject(TimerIDs.Luna).Alarm = ""
        OBStimerObject(TimerIDs.Luna).Sounds = New List(Of TimerSound)
        OBStimerObject(TimerIDs.Luna).SoundSource = "TimerAudio"

        'OBStimerObject(TimerIDs.GlobalUL).State = False
        'OBStimerObject(TimerIDs.GlobalUL).Pause = False
        'OBStimerObject(TimerIDs.GlobalUL).Title = ""
        'OBStimerObject(TimerIDs.GlobalUL).TitleSource = "Aux1TimerTitle"
        'OBStimerObject(TimerIDs.GlobalUL).Clock = "5:00"
        'OBStimerObject(TimerIDs.GlobalUL).ClockSource = "Aux1TimerClock"
        'OBStimerObject(TimerIDs.GlobalUL).Sounds = New List(Of TimerSound)
        'OBStimerObject(TimerIDs.GlobalUL).SoundSource = "Aux1TimerAlert"

        'OBStimerObject(TimerIDs.GlobalLR).State = False
        'OBStimerObject(TimerIDs.GlobalLR).Pause = False
        'OBStimerObject(TimerIDs.GlobalLR).Title = ""
        'OBStimerObject(TimerIDs.GlobalLR).TitleSource = "Aux2TimerTitle"
        'OBStimerObject(TimerIDs.GlobalLR).Clock = "5:00"
        'OBStimerObject(TimerIDs.GlobalLR).ClockSource = "Aux2TimerClock"
        'OBStimerObject(TimerIDs.GlobalLR).Sounds = New List(Of TimerSound)
        'OBStimerObject(TimerIDs.GlobalLR).SoundSource = "Aux2TimerAlert"
    End Sub

    Public Sub RunTimer(TimeInSeconds As Integer, TimerSelection As Integer, Optional StopTime As Integer = 0)
        Dim TimerThread As New Thread(
            Sub()

                If OBStimerObject(TimerSelection).Title <> "" Then
                    OBStimerObject(TimerSelection).Title =
                    Replace(OBStimerObject(TimerSelection).Title, " ", "  ")
                End If

                SetOBSsourceText(OBStimerObject(TimerSelection).TitleSource, OBStimerObject(TimerSelection).Title)

                OBStimerObject(TimerSelection).Clock = TimeString(TimeInSeconds)
                SetOBSsourceText(OBStimerObject(TimerSelection).ClockSource, OBStimerObject(TimerSelection).Clock)

                'OBStimerObject(TimerSelection).State = True

                Dim Increment As Integer = -1
                'If StopTime = 0 Then
                'If TimeInSeconds = 0 Then Increment = 1
                'Else
                If StopTime >= TimeInSeconds Then Increment = 1
                'End If


                Call UpdateSceneDisplay()
                RaiseEvent TimersUpdated(TimerSelection)

                Thread.Sleep(1000)
                Do Until OBStimerObject(TimerSelection).State = False
                    If OBStimerObject(TimerSelection).Sounds IsNot Nothing Then
                        If OBStimerObject(TimerSelection).Sounds.Count > 0 Then
                            For I As Integer = 0 To OBStimerObject(TimerSelection).Sounds.Count - 1
                                If OBStimerObject(TimerSelection).Sounds(I).SoundTime = TimeInSeconds Then
                                    If OBStimerObject(TimerSelection).Sounds(I).SoundObject <> "" Then
                                        MediaSourceChange(OBStimerObject(TimerSelection).SoundSource, MediaFcode.Play,
                                        AudioControl.GetSoundFileDataByName(OBStimerObject(TimerSelection).Sounds(I).SoundObject, True))
                                        GoTo SoundPlayed1
                                    End If
                                End If
                            Next
                        End If
                    End If
SoundPlayed1:
                    Thread.Sleep(1000)
                    TimeInSeconds = TimeInSeconds + Increment
                    OBStimerObject(TimerSelection).Clock = TimeString(TimeInSeconds)
                    SetOBSsourceText(OBStimerObject(TimerSelection).ClockSource, OBStimerObject(TimerSelection).Clock)
                    RaiseEvent TimersUpdated(TimerSelection)

                    If OBStimerObject(TimerSelection).Pause = True Then
                        Do Until OBStimerObject(TimerSelection).Pause = False
                            Thread.Sleep(1000)
                        Loop
                    End If

                    If TimeInSeconds = StopTime Then
                        If OBStimerObject(TimerSelection).Sounds IsNot Nothing Then
                            If OBStimerObject(TimerSelection).Sounds.Count > 0 Then
                                For I As Integer = 0 To OBStimerObject(TimerSelection).Sounds.Count - 1
                                    If OBStimerObject(TimerSelection).Sounds(I).SoundTime = TimeInSeconds Then
                                        If OBStimerObject(TimerSelection).Sounds(I).SoundObject <> "" Then
                                            MediaSourceChange(OBStimerObject(TimerSelection).SoundSource, MediaFcode.Play,
                                            AudioControl.GetSoundFileDataByName(OBStimerObject(TimerSelection).Sounds(I).SoundObject, True))
                                            GoTo SoundPlayed2
                                        End If
                                    End If
                                Next
                            End If
                        End If
SoundPlayed2:
                        Thread.Sleep(1000)
                        OBStimerObject(TimerSelection).State = False
                    End If
                Loop
                OBStimerObject(TimerSelection).Title = ""
                'OBStimerObject(TimerSelection).Alarm = ""
                OBStimerObject(TimerSelection).Sounds = Nothing

                Call UpdateSceneDisplay()
                RaiseEvent TimersUpdated(TimerSelection)

            End Sub)
        TimerThread.Start()
    End Sub

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
            If OBStimerObject(TimerID).State = False Then
                OBStimerObject(TimerID).State = True
                'SendMessage(OBStimerObject(TimerID).State & ", " & TimerID)
                RunTimer(TimeInSeconds, TimerID)
            Else
                SendMessage("Requested Timer is In Use", "I'M SORRY DAVE...")
            End If
        End Sub



        Public Sub RunTimerbyIndex(TimerIndex As Integer)
            'SendMessage(Timers(TimerIndex).TimertoString)
            If OBStimerObject(Timers(TimerIndex).Type).State = False Then
                OBStimerObject(Timers(TimerIndex).Type).State = True
                OBStimerObject(Timers(TimerIndex).Type).Title = Timers(TimerIndex).Label
                OBStimerObject(Timers(TimerIndex).Type).Sounds = Timers(TimerIndex).Sounds
                'OBStimerObject(Timers(TimerIndex).Type).Alarm = Timers(TimerIndex).Alarm
                RunTimer(Timers(TimerIndex).Time, Timers(TimerIndex).Type, Timers(TimerIndex).StopTime)
            Else
                SendMessage("Requested Timer is In Use", "I'M SORRY DAVE...")
            End If
        End Sub

        Public Sub RunTimerbyName(TimerName As String)
            Dim TimerIndex As Integer = GetTimerIndexByName(TimerName)
            RunTimerbyIndex(timerindex)
        End Sub

        Public Sub PauseTimerByID(TimerID As Integer)
            If OBStimerObject(TimerID).Pause = True Then
                OBStimerObject(TimerID).Pause = False
            Else
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

End Module
