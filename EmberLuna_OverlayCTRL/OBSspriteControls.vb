Imports System.Threading
Imports System.IO
Imports System.Collections.Concurrent

Public Module OBSspriteControls

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
        Public LeftH As String
        Public RightH As String
        Public Hands As String

        Public Sub InitializeMoods(CharacterSelect As Boolean)
            If CharacterSelect = SpriteID.Ember Then
                Neutral = "\ember_blink.mp4"
                Happy = "\ember_happy.mp4"
                Sadge = "\ember_sad.mp4"
                Angy = ""
                Wumpy = "\ember_wumpy.mp4"
                Cringe = ""
                Wow = ""
                Sparkle = ""
                WTF = "\ember_WTF.mp4"
                OMG = ""
                LeftH = "\ember_l.mp4"
                RightH = "\ember_r.mp4"
                Hands = "\ember_both.mp4"
            Else
                Neutral = "\luna_blink.mp4"
                Happy = "\luna_smile.mp4"
                Sadge = "\luna_sad.mp4"
                Angy = "\luna_angery.mp4"
                Wumpy = "\luna_pissed.mp4"
                Cringe = "\luna_cringe.mp4"
                Wow = "\luna_wow.mp4"
                Sparkle = "\luna_sparkles.mp4"
                WTF = ""
                OMG = "\luna_omg.mp4"
                LeftH = ""
                RightH = ""
                Hands = ""
            End If
        End Sub
    End Structure

    Public Class CharacterControls
        Private Name As String
        Public Directory As String
        Public CurrentMood As String
        Private SourceName As String
        Private ActiveEvent As Boolean
        Private Bubble As SpeechBubble
        Private EventQueue As ConcurrentQueue(Of String())
        Public Mood As CharacterMoods
        Public Event Said(MessageData As String)
        Public Event MoodChange(MoodString As String)

        Public Sub New()
            EventQueue = New ConcurrentQueue(Of String())
            ActiveEvent = False
            AddHandler Bubble.MessageSent, AddressOf MessageSent
        End Sub

        Public Sub initLUNA()
            Name = "Luna"
            Mood.InitializeMoods(SpriteID.Luna)
            Directory = "\\StreamPC-V2\OBS Assets\Characters\" & Name
            CurrentMood = Directory & Mood.Neutral
            SourceName = Name & " Sprite"
            Bubble.InitializeBubble(Name & "'s Speech Bubble", Name & " Messenger", Name & " Mtext")
        End Sub

        Public Sub initEMBER()
            Name = "Ember"
            Mood.InitializeMoods(SpriteID.Ember)
            Directory = "\\StreamPC-V2\OBS Assets\Characters\" & Name
            CurrentMood = Directory & Mood.Neutral
            SourceName = Name & " Sprite"
            Bubble.InitializeBubble(Name & "'s Speech Bubble", Name & " Messenger", Name & " Mtext")
        End Sub

        Private Sub CheckNext()
            If EventQueue.Count <> 0 Then
                Dim InputString() As String
TryAgain:
                If EventQueue.TryDequeue(InputString) = True Then
                    Select Case InputString.Length
                        Case = 2
                            ChangeMood(InputString(0), InputString(1))
                        Case = 3
                            Says(InputString(0), InputString(1), InputString(2))
                    End Select
                Else
                    GoTo TryAgain
                End If
            End If
        End Sub

        Public Sub Says(InputMessage As String,
                        Optional MessageMood As String = "",
                        Optional MessageSound As String = "")
            If ActiveEvent = False Then
                ActiveEvent = True
                Dim MessageThread As New Thread(
                    Sub()
                        If MessageSound <> "" Then
                            AudioControl.PlaySoundAlert(AudioControl.GetSoundFileDataByName(MessageSound))
                        End If
                        Dim TempMood As String = CurrentMood
                        If MessageMood <> "" Then
                            CurrentMood = MediaSourceChange(SourceName, 1, Directory & MessageMood)
                            RaiseEvent MoodChange(CurrentMood)
                        End If
                        Bubble.SendMessage(InputMessage)
                        If MessageMood <> "" Then
                            CurrentMood = MediaSourceChange(SourceName, 1, TempMood)
                            RaiseEvent MoodChange(CurrentMood)
                        End If
                        ActiveEvent = False
                        CheckNext()
                    End Sub)
                MessageThread.Start()
            Else
                Dim OutputString(0 To 2) As String
                OutputString(0) = InputMessage
                OutputString(1) = MessageMood
                OutputString(2) = MessageSound
                EventQueue.Enqueue(OutputString)
            End If

        End Sub

        Public Sub MessageSent(Messenger As String, Message As String)
            RaiseEvent Said(Messenger & " sent text: (" & Message & ")")
        End Sub

        Public Sub ChangeMood(MoodFileName As String, Optional duration As Integer = 0)
            If MoodFileName <> "" Then
                If ActiveEvent = False Then
                    If duration > 0 Then
                        ActiveEvent = True
                        Dim MoodThread As New Thread(
                        Sub()
                            Dim TempMood As String = CurrentMood
                            CurrentMood = MediaSourceChange(SourceName, 1, Directory & MoodFileName)
                            RaiseEvent MoodChange(CurrentMood)
                            Thread.Sleep(duration)
                            CurrentMood = MediaSourceChange(SourceName, 1, TempMood)
                            RaiseEvent MoodChange(CurrentMood)
                            ActiveEvent = False
                            CheckNext()
                        End Sub)
                        MoodThread.Start()
                    Else
                        If CurrentMood <> Directory & MoodFileName Then
                            CurrentMood = MediaSourceChange(SourceName, 1, Directory & MoodFileName)
                            RaiseEvent MoodChange(CurrentMood)
                        End If
                    End If
                Else
                    Dim OutputString(0 To 1) As String
                    OutputString(0) = MoodFileName
                    OutputString(1) = duration
                    EventQueue.Enqueue(OutputString)
                End If
            End If
        End Sub
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

        Public Sub SendMessage(MessageText As String)
            Mtext = MessageText
            RaiseEvent MessageSent(Name, Mtext)
            Call MediaSourceChange(Bubble, 4)
            Thread.Sleep(220)
            Call SetOBSsourceText(Msource, Mtext)
            Mtext = ""
            Thread.Sleep(3900)
            Call SetOBSsourceText(Msource, Mtext)
            Thread.Sleep(100)
        End Sub
    End Structure
End Module
