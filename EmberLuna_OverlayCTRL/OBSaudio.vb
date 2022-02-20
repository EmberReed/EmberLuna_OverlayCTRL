Imports System.Threading
Imports System.Collections.Concurrent
Imports OBSWebsocketDotNet
Imports System.IO
Public Module OBSaudio
    Public Class OBSaudioPlayer

        Public WithEvents MusicPlayer As AudioPlayer
        Public WithEvents SoundPlayer As AudioPlayer
        Public WithEvents AlertPlayer As AudioPlayer
        Public AlertQueue As ConcurrentQueue(Of String)
        Public AlertActive As Boolean
        Public SongList As List(Of String)
        Public SoundList As List(Of String)
        Public SongIndex As Integer
        Public MusicRunning As Boolean = False
        Public Const SoundBoardDirectory As String = "\\StreamPC-V2\OBS Assets\Sounds\Sound Board"
        Public SoundBoards() As SoundButts
        Public SoundFiles() As SoundButt
        Public Event SoundBoardsUpdated()
        Public Event SoundBoardUpdated(CatIndex As Integer)
        Public Event SoundBoardButtonUpdated(CatIndex As Integer, ButtNumb As Integer)
        Public Event SoundFilesUpdated()
        'Public Event SoundFileUpdated(ButtIndex As Integer)
        Public PublicSoundListLink As String
        Private Const PublicSoundsHTML As String = "\\StreamPC-V2\OBS Assets\Web Files\jerinsfx\index.html"
        Private Const PublicSoundsWeb As String = "ftp://ftp.drawingwithjerin.com//public_html/jerinsfx/index.html"
        Private Const WebUsername = "drawyemn"
        Private Const WebPword = "DragonWolf123!"

        Public Sub New()
            ReadSoundFiles()
            ReadSoundBoards()
            MusicPlayer = New AudioPlayer
            MusicPlayer.Name = "Music Player"
            MusicPlayer.Path = "\\StreamPC-V2\OBS Assets\Music\"

            SoundPlayer = New AudioPlayer
            SoundPlayer.Name = "Sound Player"
            SoundPlayer.Path = "\\StreamPC-V2\OBS Assets\Sounds\SFX\"

            AlertPlayer = New AudioPlayer
            AlertPlayer.Name = "Alert Player"
            AlertPlayer.Path = "\\StreamPC-V2\OBS Assets\Sounds\SFX\"
            AlertActive = False
            AlertQueue = New ConcurrentQueue(Of String)

            SongList = BuildMusicList()
            SoundList = BuildMusicList("\\StreamPC-V2\OBS Assets\Sounds\SFX", False, True)
            SongIndex = 0
            PublicSoundListLink = "https://drawingwithjerin.com/jerinsfx/"
            AddHandler OBS.MediaEnded, AddressOf MediaStopped
            AddHandler OBS.MediaStarted, AddressOf MediaStarted
            AddHandler OBS.MediaPaused, AddressOf MediaPaused
            AddHandler MusicPlayer.Stopped, AddressOf ContinueMusic
        End Sub

        Public Sub PlaySoundAlert(SoundFile As String)
            If AlertActive = False Then
                AlertActive = True
PlayNextSound:
                SoundAlertDisplay = True
                Call UpdateSceneDisplay()
                AlertPlayer.Play(SoundFile, True)
                Thread.Sleep(4000)
                SoundAlertDisplay = False
                Call UpdateSceneDisplay()
                Thread.Sleep(1000)
                If AlertQueue.Count <> 0 Then
TryAgain:
                    If AlertQueue.TryDequeue(SoundFile) = True Then
                        GoTo PlayNextSound
                    Else
                        GoTo TryAgain
                    End If
                Else
                    AlertActive = False
                End If
            Else
                AlertQueue.Enqueue(SoundFile)
            End If
        End Sub


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
                            If SoundBoards(i).name = InputString Then
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
        End Sub






        Public Sub PlayMusic(Optional Replay As Boolean = False)
            If MusicPlayer.Active = False Or Replay = True Then
                If SongList(SongIndex) <> "" Then
                    MusicPlayer.Play(SongList(SongIndex), Replay)
                    MusicRunning = True
                End If
            End If
        End Sub

        Public Sub StopMusic()
            If MusicPlayer.Active = True Then
                MusicRunning = False
                MusicPlayer.Stopp(True, True)
            End If

        End Sub

        Public Sub PauseMusic()
            If MusicPlayer.Active = True Then
                MusicPlayer.Pausing(True)
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
                MusicPlayer.Play(SongList(SongIndex), True)
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
                MusicPlayer.Play(SongList(SongIndex), True)
                MusicRunning = True
            End If
        End Sub

        Private Sub ContinueMusic()
            Dim PlayThread As New Thread(
                Sub()
                    Thread.Sleep(100)
                    If MusicRunning = True Then
                        If SongIndex > SongList.Count - 2 Then
                            SongIndex = 0
                        Else
                            SongIndex = SongIndex + 1
                        End If
                        PlayMusic()
                    End If
                End Sub)
            PlayThread.Start()
        End Sub

        Private Sub MediaStopped(Sender As OBSWebsocket, MediaName As String, MediaType As String)
            Select Case MediaName
                Case = MusicPlayer.Name
                    MusicPlayer.Stopp()
                Case = SoundPlayer.Name
                    SoundPlayer.Stopp()
            End Select
        End Sub

        Private Sub MediaStarted(Sender As OBSWebsocket, MediaName As String, MediaType As String)
            Select Case MediaName
                Case = MusicPlayer.Name
                    MusicPlayer.Play()
                Case = SoundPlayer.Name
                    SoundPlayer.Play()
            End Select
        End Sub

        Private Sub MediaPaused(Sender As OBSWebsocket, MediaName As String, MediaType As String)
            Select Case MediaName
                Case = MusicPlayer.Name
                    MusicPlayer.Pausing()
                Case = SoundPlayer.Name
                    SoundPlayer.Pausing()
            End Select
        End Sub


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
        'Public RNDpath As String
        Public FileQueue As ConcurrentQueue(Of String)
        Public Active As Boolean
        Public Pause As Boolean
        Public Access As Mutex
        Public Current As String
        Public Event Started()
        Public Event Stopped()
        Public Event Paused()

        Public Sub New()
            Access = New Mutex
            Access.WaitOne()
            Name = ""
            FileQueue = New ConcurrentQueue(Of String)
            Active = False
            Pause = False
            Access.ReleaseMutex()
        End Sub

        Public Sub Stopp(Optional Immediate As Boolean = False, Optional FullStop As Boolean = False)
            Dim filename As String = ""
            If Immediate = True Then
                Access.WaitOne()
                If Active = True Then
                    Current = Replace(MediaSourceChange(Name, 3), Path, "")
                    If InStr(Current, "(RND) ") Then
                        Dim SplitString() As String = Current.Split("\")
                        Current = SplitString(0)
                    End If
                    If FullStop = True Then
                        FileQueue = New ConcurrentQueue(Of String)
                    Else
TryAgain:
                        If FileQueue.Count <> 0 Then
                            FileQueue.TryDequeue(filename)
                            If filename <> "" Then GoTo TryAgain
                            Play(filename)
                        End If
                    End If
                End If
                Access.ReleaseMutex()
            Else
                Active = False
                Pause = False
                RaiseEvent Stopped()
            End If
        End Sub

        Public Sub Pausing(Optional Immediate As Boolean = False)

            If Immediate = True Then
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
            Else
                Pause = True
                RaiseEvent Paused()
            End If
        End Sub

        Public Sub Play(Optional FileName As String = "", Optional Immediate As Boolean = False)
            If FileName <> "" Then
                Access.WaitOne()
                If Active = False Then
                    Current = Replace(MediaSourceChange(Name, , Path & FileName), Path, "")
                    If InStr(Current, "(RND) ") Then
                        Dim SplitString() As String = Current.Split("\")
                        Current = SplitString(0)
                    End If
                Else
                    If Immediate = True Then
                        Current = Replace(MediaSourceChange(Name, , Path & FileName), Path, "")
                        If InStr(Current, "(RND) ") Then
                            Dim SplitString() As String = Current.Split("\")
                            Current = SplitString(0)
                        End If
                    Else
                        FileQueue.Enqueue(FileName)
                    End If
                End If
                Access.ReleaseMutex()
            Else
                Pause = False
                Active = True
                RaiseEvent Started()
            End If
        End Sub

    End Class

End Module
