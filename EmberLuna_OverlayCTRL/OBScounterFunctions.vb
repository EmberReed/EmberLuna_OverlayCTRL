Imports System.Threading
Imports System.Collections.Concurrent
Imports System.IO
Public Module OBScounterFunctions

    Public OBScounterObject() As TimerCounterData
    Public Event CountersUpdated()

    Public Structure CounterIDs
        Public Const Glob As Integer = 0
        Public Const Ember As Integer = 1
        Public Const Luna As Integer = 2
    End Structure

    Public Sub RunOBScounter(CounterSelection As Integer, PrevValue As Integer, NewValue As Integer,
                               Optional CounterTitle As String = "",
                               Optional CounterSound As String = "")
        Dim TimerThread As New Thread(
            Sub()
                OBScounterObject(CounterSelection).State = True

                OBScounterObject(CounterSelection).Title = Replace(CounterTitle, " ", "  ")
                SetOBSsourceText(OBScounterObject(CounterSelection).TitleSource, OBScounterObject(CounterSelection).Title)

                If PrevValue < 10 Then
                    OBScounterObject(CounterSelection).Value = "0" & PrevValue
                Else
                    OBScounterObject(CounterSelection).Value = PrevValue
                End If
                SetOBSsourceText(OBScounterObject(CounterSelection).ValueSource, OBScounterObject(CounterSelection).Value)

                Call UpdateSceneDisplay()
                RaiseEvent CountersUpdated()

                Thread.Sleep(2000)
                If NewValue < 10 Then
                    OBScounterObject(CounterSelection).Value = "0" & NewValue
                Else
                    OBScounterObject(CounterSelection).Value = NewValue
                End If
                SetOBSsourceText(OBScounterObject(CounterSelection).ValueSource, OBScounterObject(CounterSelection).Value)
                If CounterSound <> "" Then AudioControl.SoundPlayer.Play(AudioControl.GetSoundFileDataByName(CounterSound), True)

                Thread.Sleep(3000)
                OBScounterObject(CounterSelection).Title = ""
                OBScounterObject(CounterSelection).State = False
                Call UpdateSceneDisplay()
                RaiseEvent CountersUpdated()

            End Sub)
        TimerThread.Start()
    End Sub

    Public Sub InitializeCountersObjects()
        ReDim OBScounterObject(0 To 2)

        OBScounterObject(CounterIDs.Glob).State = False
        OBScounterObject(CounterIDs.Glob).Title = ""
        OBScounterObject(CounterIDs.Glob).Value = "00"
        OBScounterObject(CounterIDs.Glob).TitleSource = "GlobalCounterTitle"
        OBScounterObject(CounterIDs.Glob).ValueSource = "GlobalCounterValue"

        OBScounterObject(CounterIDs.Ember).State = False
        OBScounterObject(CounterIDs.Ember).Title = ""
        OBScounterObject(CounterIDs.Ember).Value = "00"
        OBScounterObject(CounterIDs.Ember).TitleSource = "EmberCounterTitle"
        OBScounterObject(CounterIDs.Ember).ValueSource = "EmberCounterValue"

        OBScounterObject(CounterIDs.Luna).State = False
        OBScounterObject(CounterIDs.Luna).Title = ""
        OBScounterObject(CounterIDs.Luna).Value = "00"
        OBScounterObject(CounterIDs.Luna).TitleSource = "LunaCounterTitle"
        OBScounterObject(CounterIDs.Luna).ValueSource = "LunaCounterValue"
    End Sub

    Public Structure TimerCounterData
        Public State As Boolean
        Public Title As String
        Public TitleSource As String
        Public Value As String
        Public ValueSource As String
    End Structure

    Public Class OBScounterData
        Public Counters() As CounterProperties
        Public CountQueue As ConcurrentQueue(Of Integer)
        Public Total As Integer
        Public Active As Boolean
        Public Current As Integer
        Public Event CounterStarted(CurrentCounter As Integer)
        Public Event CounterStopped(CurrentCounter As Integer)

        Public Sub New()
            InitializeCountersObjects()
            AddHandler CountersUpdated, AddressOf CounterEventHandler
            Active = False
            Current = -1
            CountQueue = New ConcurrentQueue(Of Integer)
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

        Public Sub CounterEventHandler()
            If OBScounterObject(CounterIDs.Ember).State = True Or
                OBScounterObject(CounterIDs.Luna).State = True Or
                OBScounterObject(CounterIDs.Glob).State = True Then

                If Active = False Then
                    Active = True
                    RaiseEvent CounterStarted(Current)
                End If
            Else
                If Active = True Then
                    Dim InputValue As Integer = -1
TryAgain:
                    If CountQueue.Count <> 0 Then
                        CountQueue.TryDequeue(InputValue)
                        If InputValue = -1 Then
                            GoTo TryAgain
                        End If
                    End If
                    Active = False
                    RaiseEvent CounterStopped(Current)
                    Current = -1
                    If InputValue > -1 Then RunCounter(InputValue)
                End If
            End If
        End Sub

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
                If OBScounterObject(Counters(CounterIndex).Type).State = False And Active = False Then
                    RunCounter(CounterIndex)
                    ReturnValue = True
                Else
                    If Not (UserGenerated = True And CounterIndex = Current) Then
                        CountQueue.Enqueue(CounterIndex)
                        ReturnValue = True
                    End If
                End If
            End If
            Return ReturnValue
        End Function

        Public Sub RunCounter(CounterIndex As Integer)
            If CounterIndex < Total Then
                Current = CounterIndex
                RunOBScounter(Counters(CounterIndex).Type, Counters(CounterIndex).PrevValue, Counters(CounterIndex).Count,
                                Counters(CounterIndex).Label, Counters(CounterIndex).Sound)
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
            'File.Create(FileString).Dispose()
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

        Public Function PrevValue() As Integer
            Dim InputValue = Count
            InputValue = InputValue - Increment
            'SendMessage(InputValue)
            Return InputValue
        End Function

        Public Sub SubCount()
            If Count > 0 Then
                Count = Count - Increment
            End If
            'SendMessage(Count)
        End Sub

        Public Sub AddCount()
            If Count < 100 Then
                Count = Count + Increment
            End If
            If Count > 100 Then
                Count = 100
            End If
            'SendMessage(Count)
        End Sub

        Public Sub ReadCounterData(DataLine As String)
            Dim splitstring() As String = Split(DataLine, ",")
            Name = splitstring(0)
            Label = splitstring(1)
            Count = splitstring(2)
            Increment = splitstring(3)
            Type = splitstring(4)
            Sound = splitstring(5)
            If splitstring.Length > 6 Then
                OBSevent = splitstring(6)
                NotificationType = splitstring(7)
                PublicCount = splitstring(8)
            End If
        End Sub

        Public Function WriteCounterData() As String
            Dim Outputstring As String =
            Name & "," &
            Label & "," &
            Count & "," &
            Increment & "," &
            Type & "," &
            Sound & "," &
            OBSevent & "," &
            NotificationType & "," &
            PublicCount

            Return Outputstring
        End Function
    End Structure

End Module
