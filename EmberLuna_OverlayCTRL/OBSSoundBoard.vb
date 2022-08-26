Imports System.Threading
Public Class OBSSoundBoard

    Private CurrentActiveSound As String = "", PreviousActiveSound As String = "",
        SourceIndex As Integer = 0, TotalSounds As Integer = 0
    Private DisplayTotal As Integer = 0
    Private SizeIncrement As Integer = 73
    Private ButtonsWide As Integer = 6
    Private ButtonsHigh As Integer = 6
    Private BaseHeight As Integer = 159
    Private SoundEditor As AddSound

    Private TextVariable As String
    Private ButtonHold() As Boolean
    Private SourceButtonNumber As Integer = -1
    Private SourceButtonText As String = ""
    Private MouseIsDown As Boolean = False

    Private Sub SoundBoard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SoundEditor = New AddSound
        DisplayTotal = ButtonsWide * ButtonsHigh
        ReDim ButtonHold(0 To DisplayTotal - 1)
        SourceWindow.SOUND_BOARD.BackColor = ActiveBUTT
        'SourceList = BuildMusicList("\\StreamPC-V2\OBS Assets\Sounds\SFX", False)
        DisplaySoundList()
        DisplaySounds()
        'AddHandler AudioControl.SoundPlayer.Started, AddressOf ActiveSoundDisplay
        'AddHandler AudioControl.SoundPlayer.Stopped, AddressOf InactiveSoundDisplay
        AddHandler AudioControl.SoundBoardsUpdated, AddressOf RemoteDisplaySounds
        AddHandler AudioControl.SoundBoardUpdated, AddressOf RemoteDisplayCategory
        AddHandler AudioControl.SoundBoardButtonUpdated, AddressOf RemoteDisplaySingle
        AddHandler AudioControl.SoundFilesUpdated, AddressOf RemoteDisplaySoundList
    End Sub
    Private Sub SoundBoard_CLOSING(sender As Object, e As EventArgs) Handles MyBase.Closing
        'RemoveHandler AudioControl.SoundPlayer.Started, AddressOf ActiveSoundDisplay
        'RemoveHandler AudioControl.SoundPlayer.Stopped, AddressOf InactiveSoundDisplay
        RemoveHandler AudioControl.SoundBoardsUpdated, AddressOf RemoteDisplaySounds
        RemoveHandler AudioControl.SoundBoardUpdated, AddressOf RemoteDisplayCategory
        RemoveHandler AudioControl.SoundBoardButtonUpdated, AddressOf RemoteDisplaySingle
        RemoveHandler AudioControl.SoundFilesUpdated, AddressOf RemoteDisplaySoundList
        SourceWindow.SOUND_BOARD.BackColor = StandardBUTT
    End Sub



    Private Sub IntializeCategorySelector()
        SoundCategorySelector.Items.Clear()
        If AudioControl.SoundBoards IsNot Nothing Then
            If AudioControl.SoundBoards.Length <> 0 Then
                For I As Integer = 0 To AudioControl.SoundBoards.Length - 1
                    SoundCategorySelector.Items.Add(New ToolStripMenuItem(I + 1 & " " & AudioControl.SoundBoards(I).Name))
                Next
            End If
        End If
    End Sub

    Private Sub CategorySelector_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles SoundCategorySelector.ItemClicked
        Dim ValuesString() As String
        ValuesString = Split(e.ClickedItem.ToString)
        Dim CatIndex As Integer = ValuesString(0)
        SourceIndex = CatIndex - 1
        DisplaySounds()
    End Sub

    Private Sub ADDSB_Click(sender As Object, e As EventArgs) Handles ADD.Click
        AudioControl.CreateNewSoundBoard()
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Dim Output As MsgBoxResult = MsgBox("Delete This Sound Board?", MsgBoxStyle.OkCancel, "ARE YOU SURE ABOUT THAT")
        If Output = MsgBoxResult.Ok Then
            AudioControl.DeleteSoundBoard(SourceIndex)
        End If
    End Sub

    'Private Sub MoveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MoveToolStripMenuItem.Click
    'Dim InputSTring As String = Replace(ActiveSoundButtMenu.SourceControl.Name, "SoundButt", "")
    'If IsNumeric(InputSTring) Then
    'Dim SoundIndex As Integer = InputSTring
    'Call DisplaySounds(True)
    'InputString = InputBox("Select New Sound Button Position", "WHERE TO?")
    'TryAgain:
    'If IsNumeric(InputSTring) Then
    'Dim NewSoundIndex As Integer = InputSTring
    'If AudioControl.SoundBoards(SourceIndex).Buttons(NewSoundIndex - 1).Name = "" Then
    'AudioControl.MoveSoundButt(SourceIndex, SoundIndex, NewSoundIndex - 1)
    'Call DisplaySounds()
    'Else
    'InputSTring = InputBox("Not Available! Try Again:", "WHERE TO?")
    'GoTo TryAgain
    'End If
    'Else
    'InputString = InputBox("Needs to be a Number Stupid!", "WHERE TO?")
    'GoTo TryAgain
    'End If
    'End If
    'End Sub
    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox3.Click
        Dim SoundString As String = TextboxLineSelector(SoundListBox)
        If SoundString <> "" Then
            AudioControl.SoundPlayer.Play(AudioControl.GetSoundFileDataByName(SoundString))
            'Dim SoundTask As Task = PlaySoundSyncTest(SoundString)
        End If
    End Sub

    Private Async Function PlaySoundSyncTest(SoundString As String) As Task
        Await AudioControl.SoundPlayer.PlayAsync(AudioControl.GetSoundFileDataByName(SoundString))
        SendMessage("Success")
    End Function

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        Dim ButtIndex As Integer = -1
        Dim SoundString As String = TextboxLineSelector(SoundListBox)
        If SoundString <> "" Then
            ButtIndex = AudioControl.GetSoundFileIndexByName(SoundString)
        End If
        If ButtIndex > -1 Then
            Dim Output As MsgBoxResult = MsgBox("Delete This Sound Button?", MsgBoxStyle.OkCancel, "ARE YOU SURE ABOUT THAT")
            If Output = MsgBoxResult.Ok Then
                AudioControl.DeleteSoundFile(ButtIndex)
            End If
        End If
    End Sub

    Private Sub DeleteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteToolStripMenuItem.Click
        Dim ButtIndex As Integer = -1
        If ActiveSoundButtMenu.SourceControl.Name = "SoundListBox" Then
            Dim SoundString As String = TextboxLineSelector(SoundListBox)
            If SoundString <> "" Then
                ButtIndex = AudioControl.GetSoundFileIndexByName(SoundString)
            End If
        Else
            Dim InputSTring As String = Replace(ActiveSoundButtMenu.SourceControl.Name, "SoundButt", "")
            If IsNumeric(InputSTring) Then
                Dim SoundIndex As Integer = InputSTring
                ButtIndex = AudioControl.GetSoundFileIndexByName(AudioControl.SoundBoards(SourceIndex).Buttons(SoundIndex))
            End If
        End If
        If ButtIndex > -1 Then
            Dim Output As MsgBoxResult = MsgBox("Delete This Sound Button?", MsgBoxStyle.OkCancel, "ARE YOU SURE ABOUT THAT")
            If Output = MsgBoxResult.Ok Then
                AudioControl.DeleteSoundFile(ButtIndex)
            End If
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If SoundEditor.Visible = False Then
            SoundEditor = New AddSound
            'SoundEditor.SoundIndex = AudioControl.GetSoundFileIndexByName(SoundString)
            SoundEditor.Show()
        End If
    End Sub

    Private Sub AddToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddToolStripMenuItem.Click
        If SoundEditor.Visible = False Then
            Dim InputSTring As String = Replace(InactiveSoundButtMenu.SourceControl.Name, "SoundButt", "")
            If IsNumeric(InputSTring) Then
                Dim SoundIndex As Integer = InputSTring
                SoundEditor = New AddSound
                SoundEditor.CatIndex = SourceIndex
                SoundEditor.ButtonIndex = SoundIndex
                'SoundEditor.SoundIndex = AudioControl.GetSoundFileIndexByName(AudioControl.SoundBoards(SourceIndex).Buttons(SoundIndex))
                SoundEditor.Show()
            End If
        End If
    End Sub

    Private Sub EditToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditToolStripMenuItem.Click
        If SoundEditor.Visible = False Then
            If ActiveSoundButtMenu.SourceControl.Name = "SoundListBox" Then
                Dim SoundString As String = TextboxLineSelector(SoundListBox)
                If SoundString <> "" Then
                    SoundEditor = New AddSound
                    SoundEditor.SoundIndex = AudioControl.GetSoundFileIndexByName(SoundString)
                    SoundEditor.Show()
                End If
            Else
                Dim InputSTring As String = Replace(ActiveSoundButtMenu.SourceControl.Name, "SoundButt", "")
                If IsNumeric(InputSTring) Then
                    Dim SoundIndex As Integer = InputSTring
                    SoundEditor = New AddSound
                    SoundEditor.CatIndex = SourceIndex
                    SoundEditor.ButtonIndex = SoundIndex
                    SoundEditor.SoundIndex = AudioControl.GetSoundFileIndexByName(AudioControl.SoundBoards(SourceIndex).Buttons(SoundIndex))
                    SoundEditor.Show()
                End If
            End If
        End If
    End Sub

    Private Sub RemoteDisplaySounds()
        BeginInvoke(Sub() DisplaySounds())
    End Sub
    Private Sub RemoteDisplayCategory(CatIndex As Integer)
        If CatIndex = SourceIndex Then
            BeginInvoke(Sub() DisplaySounds())
        End If
    End Sub

    Private Sub RemoteDisplaySingle(CatIndex As Integer, SoundIndex As Integer)
        If CatIndex = SourceIndex Then
            BeginInvoke(Sub() DisplaySoundSingle(SoundIndex))
        End If
    End Sub

    Private Sub DisplaySounds()
        SetDrawing(Me.Handle, WM_SETREDRAW, False, 0)
        IntializeCategorySelector()
        If AudioControl.SoundBoards IsNot Nothing Then
            If AudioControl.SoundBoards.Length <> 0 Then
                TotalSounds = DisplayTotal
                If SourceIndex > AudioControl.SoundBoards.Length - 1 Then
                    SourceIndex = AudioControl.SoundBoards.Length - 1
                End If
                CategoryLabel.Text = AudioControl.SoundBoards(SourceIndex).Name
            Else
                CategoryLabel.Text = "N/A"
                TotalSounds = 0
                SourceIndex = 0
            End If
        Else
            CategoryLabel.Text = "N/A"
            TotalSounds = 0
            SourceIndex = 0
        End If
        For i As Integer = 0 To DisplayTotal - 1
            DisplaySoundSingle(i)
        Next
        'Dim InputSize As Size = Me.Size
        'InputSize.Height = GetFormHeight(TotalSounds, ButtonsWide, ButtonsHigh, BaseHeight, SizeIncrement)
        'Me.Size = InputSize
        'DisplaySoundList()

        SetDrawing(Me.Handle, WM_SETREDRAW, True, 0)
        Me.Refresh()
    End Sub

    Private Sub RemoteDisplaySoundList()
        BeginInvoke(Sub() DisplaySoundList())
    End Sub

    Private Sub DisplaySoundList()
        SoundListBox.Text = Join(AudioControl.GetFullSoundFilesList.ToArray, vbCrLf)
    End Sub

    Private Function SBstringFormat(InputString As String) As String
        Dim Outputstring As String
        Outputstring = Replace(InputString, ".wav", "")
        Outputstring = Replace(Outputstring, ".mp3", "")
        Outputstring = Replace(Outputstring, "(SB) ", "")
        Return Outputstring
    End Function

    Private Sub DisplaySoundSingle(DisplayIndex As Integer)
        If AudioControl.SoundBoards IsNot Nothing Then
            Dim SoundButt As Button = CType(Controls("SoundButt" & DisplayIndex), Button)
            If AudioControl.SoundBoards(SourceIndex).Buttons(DisplayIndex) <> "" Then
                SoundButt.ContextMenuStrip = ActiveSoundButtMenu
                Dim ButtIndex As Integer = AudioControl.GetSoundFileIndexByName(AudioControl.SoundBoards(SourceIndex).Buttons(DisplayIndex))
                SoundButt.Text = AudioControl.SoundFiles(ButtIndex).Title
                SoundButt.BackColor = AudioControl.SoundFiles(ButtIndex).SoundColor
            Else
                SoundButt.ContextMenuStrip = InactiveSoundButtMenu
                SoundButt.Text = ""
                SoundButt.BackColor = StandardBUTT
            End If
        End If
    End Sub

    Private Sub SKIPFBUTT_Click(sender As Object, e As EventArgs) Handles SKIPFBUTT.Click
        Call IncreaseSoundIndex()
    End Sub

    Private Sub SKIPBBUTT_Click(sender As Object, e As EventArgs) Handles SKIPBBUTT.Click
        Call DecreaseSoundIndex()
    End Sub

    Private Sub IncreaseSoundIndex()
        If AudioControl.SoundBoards.Length - 1 > SourceIndex Then
            SourceIndex = SourceIndex + 1
            Call DisplaySounds()
        End If
    End Sub
    Private Sub DecreaseSoundIndex()
        If SourceIndex > 0 Then
            SourceIndex = SourceIndex - 1
            Call DisplaySounds()
        End If
    End Sub

    Private Sub STOPBUTT_Click(sender As Object, e As EventArgs) Handles STOPBUTT.Click
        If AudioControl.SoundPlayer.Active = True Then
            AudioControl.SoundPlayer.Stopp()
        End If
    End Sub


    Private Sub SoundButt0_Click(sender As Object, e As EventArgs) Handles SoundButt0.Click
        PlaySound(0)
    End Sub
    Private Sub SoundButt1_Click(sender As Object, e As EventArgs) Handles SoundButt1.Click
        PlaySound(1)
    End Sub
    Private Sub SoundButt2_Click(sender As Object, e As EventArgs) Handles SoundButt2.Click
        PlaySound(2)
    End Sub
    Private Sub SoundButt3_Click(sender As Object, e As EventArgs) Handles SoundButt3.Click
        PlaySound(3)
    End Sub
    Private Sub SoundButt4_Click(sender As Object, e As EventArgs) Handles SoundButt4.Click
        PlaySound(4)
    End Sub
    Private Sub SoundButt5_Click(sender As Object, e As EventArgs) Handles SoundButt5.Click
        PlaySound(5)
    End Sub
    Private Sub SoundButt6_Click(sender As Object, e As EventArgs) Handles SoundButt6.Click
        PlaySound(6)
    End Sub
    Private Sub SoundButt7_Click(sender As Object, e As EventArgs) Handles SoundButt7.Click
        PlaySound(7)
    End Sub
    Private Sub SoundButt8_Click(sender As Object, e As EventArgs) Handles SoundButt8.Click
        PlaySound(8)
    End Sub
    Private Sub SoundButt9_Click(sender As Object, e As EventArgs) Handles SoundButt9.Click
        PlaySound(9)
    End Sub
    Private Sub SoundButt10_Click(sender As Object, e As EventArgs) Handles SoundButt10.Click
        PlaySound(10)
    End Sub
    Private Sub SoundButt11_Click(sender As Object, e As EventArgs) Handles SoundButt11.Click
        PlaySound(11)
    End Sub
    Private Sub SoundButt12_Click(sender As Object, e As EventArgs) Handles SoundButt12.Click
        PlaySound(12)
    End Sub
    Private Sub SoundButt13_Click(sender As Object, e As EventArgs) Handles SoundButt13.Click
        PlaySound(13)
    End Sub
    Private Sub SoundButt14_Click(sender As Object, e As EventArgs) Handles SoundButt14.Click
        PlaySound(14)
    End Sub
    Private Sub SoundButt15_Click(sender As Object, e As EventArgs) Handles SoundButt15.Click
        PlaySound(15)
    End Sub
    Private Sub SoundButt16_Click(sender As Object, e As EventArgs) Handles SoundButt16.Click
        PlaySound(16)
    End Sub
    Private Sub SoundButt17_Click(sender As Object, e As EventArgs) Handles SoundButt17.Click
        PlaySound(17)
    End Sub
    Private Sub SoundButt18_Click(sender As Object, e As EventArgs) Handles SoundButt18.Click
        PlaySound(18)
    End Sub
    Private Sub SoundButt19_Click(sender As Object, e As EventArgs) Handles SoundButt19.Click
        PlaySound(19)
    End Sub
    Private Sub SoundButt20_Click(sender As Object, e As EventArgs) Handles SoundButt20.Click
        PlaySound(20)
    End Sub
    Private Sub SoundButt21_Click(sender As Object, e As EventArgs) Handles SoundButt21.Click
        PlaySound(21)
    End Sub
    Private Sub SoundButt22_Click(sender As Object, e As EventArgs) Handles SoundButt22.Click
        PlaySound(22)
    End Sub
    Private Sub SoundButt23_Click(sender As Object, e As EventArgs) Handles SoundButt23.Click
        PlaySound(23)
    End Sub
    Private Sub SoundButt24_Click(sender As Object, e As EventArgs) Handles SoundButt24.Click
        PlaySound(24)
    End Sub
    Private Sub SoundButt25_Click(sender As Object, e As EventArgs) Handles SoundButt25.Click
        PlaySound(25)
    End Sub
    Private Sub SoundButt26_Click(sender As Object, e As EventArgs) Handles SoundButt26.Click
        PlaySound(26)
    End Sub
    Private Sub SoundButt27_Click(sender As Object, e As EventArgs) Handles SoundButt27.Click
        PlaySound(27)
    End Sub
    Private Sub SoundButt28_Click(sender As Object, e As EventArgs) Handles SoundButt28.Click
        PlaySound(28)
    End Sub
    Private Sub SoundButt29_Click(sender As Object, e As EventArgs) Handles SoundButt29.Click
        PlaySound(29)
    End Sub
    Private Sub SoundButt30_Click(sender As Object, e As EventArgs) Handles SoundButt30.Click
        PlaySound(30)
    End Sub
    Private Sub SoundButt31_Click(sender As Object, e As EventArgs) Handles SoundButt31.Click
        PlaySound(31)
    End Sub

    Private Sub SoundButt32_Click(sender As Object, e As EventArgs) Handles SoundButt32.Click
        PlaySound(32)
    End Sub


    Private Sub SoundButt33_Click(sender As Object, e As EventArgs) Handles SoundButt33.Click
        PlaySound(33)
    End Sub

    Private Sub SoundButt34_Click(sender As Object, e As EventArgs) Handles SoundButt34.Click
        PlaySound(34)
    End Sub

    Private Sub SoundButt35_Click(sender As Object, e As EventArgs) Handles SoundButt35.Click
        PlaySound(35)
    End Sub



    'Public Function GetSelectedSoundFromSoundList() As String
    '    If SoundListBox.SelectionStart <> 0 Then
    '        Dim StartPoint As Integer = InStrRev(SoundListBox.Text, vbCrLf, SoundListBox.SelectionStart) + 1
    '        If StartPoint = 1 Then StartPoint = 0
    '        Dim EndPoint As Integer = InStr(SoundListBox.SelectionStart, SoundListBox.Text, vbCrLf) - 1
    '        If EndPoint = -1 Then EndPoint = SoundListBox.Text.Length
    '        SoundListBox.SelectionStart = StartPoint
    '        SoundListBox.SelectionLength = EndPoint - StartPoint


    '        'Dim current_line As Integer = SoundListBox.GetLineFromCharIndex(SoundListBox.SelectionStart)
    '        'Dim line_length As Integer = SoundListBox.Lines(current_line).Length
    '        'SoundListBox.SelectionStart = SoundListBox.GetFirstCharIndexOfCurrentLine
    '        'SoundListBox.SelectionLength = line_length
    '        Return SoundListBox.SelectedText
    '    Else
    '        Return ""
    '    End If

    'End Function

    Private Sub SoundListBox_Click(sender As TextBox, e As EventArgs) Handles SoundListBox.Click
        MouseIsDown = False
        TextboxLineSelector(sender)
        'TextVariable = SoundListBox.SelectedText
    End Sub

    Private Sub SoundListBox_DClick(sender As TextBox, e As EventArgs) Handles SoundListBox.DoubleClick
        MouseIsDown = False
        AudioControl.SoundPlayer.Play(AudioControl.GetSoundFileDataByName(TextboxLineSelector(sender)))
    End Sub

    Private Sub SoundListBox_MouseDown(ByVal sender As TextBox, ByVal e As MouseEventArgs) Handles SoundListBox.MouseDown
        MouseIsDown = True
    End Sub

    Private Sub SoundListBox_MouseMove(ByVal sender As TextBox, ByVal e As MouseEventArgs) Handles SoundListBox.MouseMove
        If MouseIsDown Then
            Dim SoundString As String = TextboxLineSelector(sender)
            If SoundString <> "" Then
                SoundListBox.DoDragDrop(SoundString, DragDropEffects.Copy)
            End If
        End If
        MouseIsDown = False
    End Sub

    Private Sub SoundListBox_DragEnter(sender As TextBox, e As DragEventArgs) Handles SoundListBox.DragEnter
        If (e.Data.GetDataPresent(DataFormats.Text)) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub SoundListBox_DragDrop(sender As TextBox, e As DragEventArgs) Handles SoundListBox.DragDrop
        If SourceButtonNumber > -1 Then
            AudioControl.UpdateSoundBoardButton(SourceIndex, SourceButtonNumber, "")
            SourceButtonNumber = -1
        End If
    End Sub

    'Sound Button 0
    Private Sub SoundButt0_MouseDown(sender As Object, e As EventArgs) Handles SoundButt0.MouseDown
        ButtMouseDown(0)
    End Sub
    Private Sub SoundButt0_MouseLeave(sender As Object, e As EventArgs) Handles SoundButt0.MouseLeave
        ButtMouseLeave(0)
    End Sub
    Private Sub SoundButt0_MouseDragEnter(sender As Object, e As DragEventArgs) Handles SoundButt0.DragEnter
        ButtDragEnter(0, e)
    End Sub
    Private Sub SoundButt0_MouseDragDrop(sender As Object, e As DragEventArgs) Handles SoundButt0.DragDrop
        ButtDragDrop(0, e)
    End Sub

    'Sound Button 1
    Private Sub SoundButt1_MouseDown(sender As Object, e As EventArgs) Handles SoundButt1.MouseDown
        ButtMouseDown(1)
    End Sub
    Private Sub SoundButt1_MouseLeave(sender As Object, e As EventArgs) Handles SoundButt1.MouseLeave
        ButtMouseLeave(1)
    End Sub
    Private Sub SoundButt1_MouseDragEnter(sender As Object, e As DragEventArgs) Handles SoundButt1.DragEnter
        ButtDragEnter(1, e)
    End Sub
    Private Sub SoundButt1_MouseDragDrop(sender As Object, e As DragEventArgs) Handles SoundButt1.DragDrop
        ButtDragDrop(1, e)
    End Sub

    'Sound Button 2
    Private Sub SoundButt2_MouseDown(sender As Object, e As EventArgs) Handles SoundButt2.MouseDown
        ButtMouseDown(2)
    End Sub
    Private Sub SoundButt2_MouseLeave(sender As Object, e As EventArgs) Handles SoundButt2.MouseLeave
        ButtMouseLeave(2)
    End Sub
    Private Sub SoundButt2_MouseDragEnter(sender As Object, e As DragEventArgs) Handles SoundButt2.DragEnter
        ButtDragEnter(2, e)
    End Sub
    Private Sub SoundButt2_MouseDragDrop(sender As Object, e As DragEventArgs) Handles SoundButt2.DragDrop
        ButtDragDrop(2, e)
    End Sub

    'Sound Button 3
    Private Sub SoundButt3_MouseDown(sender As Object, e As EventArgs) Handles SoundButt3.MouseDown
        ButtMouseDown(3)
    End Sub
    Private Sub SoundButt3_MouseLeave(sender As Object, e As EventArgs) Handles SoundButt3.MouseLeave
        ButtMouseLeave(3)
    End Sub
    Private Sub SoundButt3_MouseDragEnter(sender As Object, e As DragEventArgs) Handles SoundButt3.DragEnter
        ButtDragEnter(3, e)
    End Sub
    Private Sub SoundButt3_MouseDragDrop(sender As Object, e As DragEventArgs) Handles SoundButt3.DragDrop
        ButtDragDrop(3, e)
    End Sub

    'Sound Button 4
    Private Sub SoundButt4_MouseDown(sender As Object, e As EventArgs) Handles SoundButt4.MouseDown
        ButtMouseDown(4)
    End Sub
    Private Sub SoundButt4_MouseLeave(sender As Object, e As EventArgs) Handles SoundButt4.MouseLeave
        ButtMouseLeave(4)
    End Sub
    Private Sub SoundButt4_MouseDragEnter(sender As Object, e As DragEventArgs) Handles SoundButt4.DragEnter
        ButtDragEnter(4, e)
    End Sub
    Private Sub SoundButt4_MouseDragDrop(sender As Object, e As DragEventArgs) Handles SoundButt4.DragDrop
        ButtDragDrop(4, e)
    End Sub

    'Sound Button 5
    Private Sub SoundButt5_MouseDown(sender As Object, e As EventArgs) Handles SoundButt5.MouseDown
        ButtMouseDown(5)
    End Sub
    Private Sub SoundButt5_MouseLeave(sender As Object, e As EventArgs) Handles SoundButt5.MouseLeave
        ButtMouseLeave(5)
    End Sub
    Private Sub SoundButt5_MouseDragEnter(sender As Object, e As DragEventArgs) Handles SoundButt5.DragEnter
        ButtDragEnter(5, e)
    End Sub
    Private Sub SoundButt5_MouseDragDrop(sender As Object, e As DragEventArgs) Handles SoundButt5.DragDrop
        ButtDragDrop(5, e)
    End Sub

    'Sound Button 6
    Private Sub SoundButt6_MouseDown(sender As Object, e As EventArgs) Handles SoundButt6.MouseDown
        ButtMouseDown(6)
    End Sub
    Private Sub SoundButt6_MouseLeave(sender As Object, e As EventArgs) Handles SoundButt6.MouseLeave
        ButtMouseLeave(6)
    End Sub
    Private Sub SoundButt6_MouseDragEnter(sender As Object, e As DragEventArgs) Handles SoundButt6.DragEnter
        ButtDragEnter(6, e)
    End Sub
    Private Sub SoundButt6_MouseDragDrop(sender As Object, e As DragEventArgs) Handles SoundButt6.DragDrop
        ButtDragDrop(6, e)
    End Sub

    'Sound Button 7
    Private Sub SoundButt7_MouseDown(sender As Object, e As EventArgs) Handles SoundButt7.MouseDown
        ButtMouseDown(7)
    End Sub
    Private Sub SoundButt7_MouseLeave(sender As Object, e As EventArgs) Handles SoundButt7.MouseLeave
        ButtMouseLeave(7)
    End Sub
    Private Sub SoundButt7_MouseDragEnter(sender As Object, e As DragEventArgs) Handles SoundButt7.DragEnter
        ButtDragEnter(7, e)
    End Sub
    Private Sub SoundButt7_MouseDragDrop(sender As Object, e As DragEventArgs) Handles SoundButt7.DragDrop
        ButtDragDrop(7, e)
    End Sub

    'Sound Button 8
    Private Sub SoundButt8_MouseDown(sender As Object, e As EventArgs) Handles SoundButt8.MouseDown
        ButtMouseDown(8)
    End Sub
    Private Sub SoundButt8_MouseLeave(sender As Object, e As EventArgs) Handles SoundButt8.MouseLeave
        ButtMouseLeave(8)
    End Sub
    Private Sub SoundButt8_MouseDragEnter(sender As Object, e As DragEventArgs) Handles SoundButt8.DragEnter
        ButtDragEnter(8, e)
    End Sub
    Private Sub SoundButt8_MouseDragDrop(sender As Object, e As DragEventArgs) Handles SoundButt8.DragDrop
        ButtDragDrop(8, e)
    End Sub

    'Sound Button 9
    Private Sub SoundButt9_MouseDown(sender As Object, e As EventArgs) Handles SoundButt9.MouseDown
        ButtMouseDown(9)
    End Sub
    Private Sub SoundButt9_MouseLeave(sender As Object, e As EventArgs) Handles SoundButt9.MouseLeave
        ButtMouseLeave(9)
    End Sub
    Private Sub SoundButt9_MouseDragEnter(sender As Object, e As DragEventArgs) Handles SoundButt9.DragEnter
        ButtDragEnter(9, e)
    End Sub
    Private Sub SoundButt9_MouseDragDrop(sender As Object, e As DragEventArgs) Handles SoundButt9.DragDrop
        ButtDragDrop(9, e)
    End Sub

    'Sound Button 10
    Private Sub SoundButt10_MouseDown(sender As Object, e As EventArgs) Handles SoundButt10.MouseDown
        ButtMouseDown(10)
    End Sub
    Private Sub SoundButt10_MouseLeave(sender As Object, e As EventArgs) Handles SoundButt10.MouseLeave
        ButtMouseLeave(10)
    End Sub
    Private Sub SoundButt10_MouseDragEnter(sender As Object, e As DragEventArgs) Handles SoundButt10.DragEnter
        ButtDragEnter(10, e)
    End Sub
    Private Sub SoundButt10_MouseDragDrop(sender As Object, e As DragEventArgs) Handles SoundButt10.DragDrop
        ButtDragDrop(10, e)
    End Sub

    'Sound Button 11
    Private Sub SoundButt11_MouseDown(sender As Object, e As EventArgs) Handles SoundButt11.MouseDown
        ButtMouseDown(11)
    End Sub
    Private Sub SoundButt11_MouseLeave(sender As Object, e As EventArgs) Handles SoundButt11.MouseLeave
        ButtMouseLeave(11)
    End Sub
    Private Sub SoundButt11_MouseDragEnter(sender As Object, e As DragEventArgs) Handles SoundButt11.DragEnter
        ButtDragEnter(11, e)
    End Sub
    Private Sub SoundButt11_MouseDragDrop(sender As Object, e As DragEventArgs) Handles SoundButt11.DragDrop
        ButtDragDrop(11, e)
    End Sub

    'Sound Button 12
    Private Sub SoundButt12_MouseDown(sender As Object, e As EventArgs) Handles SoundButt12.MouseDown
        ButtMouseDown(12)
    End Sub
    Private Sub SoundButt12_MouseLeave(sender As Object, e As EventArgs) Handles SoundButt12.MouseLeave
        ButtMouseLeave(12)
    End Sub
    Private Sub SoundButt12_MouseDragEnter(sender As Object, e As DragEventArgs) Handles SoundButt12.DragEnter
        ButtDragEnter(12, e)
    End Sub
    Private Sub SoundButt12_MouseDragDrop(sender As Object, e As DragEventArgs) Handles SoundButt12.DragDrop
        ButtDragDrop(12, e)
    End Sub

    'Sound Button 13
    Private Sub SoundButt13_MouseDown(sender As Object, e As EventArgs) Handles SoundButt13.MouseDown
        ButtMouseDown(13)
    End Sub
    Private Sub SoundButt13_MouseLeave(sender As Object, e As EventArgs) Handles SoundButt13.MouseLeave
        ButtMouseLeave(13)
    End Sub
    Private Sub SoundButt13_MouseDragEnter(sender As Object, e As DragEventArgs) Handles SoundButt13.DragEnter
        ButtDragEnter(13, e)
    End Sub
    Private Sub SoundButt13_MouseDragDrop(sender As Object, e As DragEventArgs) Handles SoundButt13.DragDrop
        ButtDragDrop(13, e)
    End Sub

    'Sound Button 14
    Private Sub SoundButt14_MouseDown(sender As Object, e As EventArgs) Handles SoundButt14.MouseDown
        ButtMouseDown(14)
    End Sub
    Private Sub SoundButt14_MouseLeave(sender As Object, e As EventArgs) Handles SoundButt14.MouseLeave
        ButtMouseLeave(14)
    End Sub
    Private Sub SoundButt14_MouseDragEnter(sender As Object, e As DragEventArgs) Handles SoundButt14.DragEnter
        ButtDragEnter(14, e)
    End Sub
    Private Sub SoundButt14_MouseDragDrop(sender As Object, e As DragEventArgs) Handles SoundButt14.DragDrop
        ButtDragDrop(14, e)
    End Sub

    'Sound Button 15
    Private Sub SoundButt15_MouseDown(sender As Object, e As EventArgs) Handles SoundButt15.MouseDown
        ButtMouseDown(15)
    End Sub
    Private Sub SoundButt15_MouseLeave(sender As Object, e As EventArgs) Handles SoundButt15.MouseLeave
        ButtMouseLeave(15)
    End Sub
    Private Sub SoundButt15_MouseDragEnter(sender As Object, e As DragEventArgs) Handles SoundButt15.DragEnter
        ButtDragEnter(15, e)
    End Sub
    Private Sub SoundButt15_MouseDragDrop(sender As Object, e As DragEventArgs) Handles SoundButt15.DragDrop
        ButtDragDrop(15, e)
    End Sub

    'Sound Button 16
    Private Sub SoundButt16_MouseDown(sender As Object, e As EventArgs) Handles SoundButt16.MouseDown
        ButtMouseDown(16)
    End Sub
    Private Sub SoundButt16_MouseLeave(sender As Object, e As EventArgs) Handles SoundButt16.MouseLeave
        ButtMouseLeave(16)
    End Sub
    Private Sub SoundButt16_MouseDragEnter(sender As Object, e As DragEventArgs) Handles SoundButt16.DragEnter
        ButtDragEnter(16, e)
    End Sub
    Private Sub SoundButt16_MouseDragDrop(sender As Object, e As DragEventArgs) Handles SoundButt16.DragDrop
        ButtDragDrop(16, e)
    End Sub

    'Sound Button 17
    Private Sub SoundButt17_MouseDown(sender As Object, e As EventArgs) Handles SoundButt17.MouseDown
        ButtMouseDown(17)
    End Sub
    Private Sub SoundButt17_MouseLeave(sender As Object, e As EventArgs) Handles SoundButt17.MouseLeave
        ButtMouseLeave(17)
    End Sub
    Private Sub SoundButt17_MouseDragEnter(sender As Object, e As DragEventArgs) Handles SoundButt17.DragEnter
        ButtDragEnter(17, e)
    End Sub
    Private Sub SoundButt17_MouseDragDrop(sender As Object, e As DragEventArgs) Handles SoundButt17.DragDrop
        ButtDragDrop(17, e)
    End Sub

    'Sound Button 18
    Private Sub SoundButt18_MouseDown(sender As Object, e As EventArgs) Handles SoundButt18.MouseDown
        ButtMouseDown(18)
    End Sub
    Private Sub SoundButt18_MouseLeave(sender As Object, e As EventArgs) Handles SoundButt18.MouseLeave
        ButtMouseLeave(18)
    End Sub
    Private Sub SoundButt18_MouseDragEnter(sender As Object, e As DragEventArgs) Handles SoundButt18.DragEnter
        ButtDragEnter(18, e)
    End Sub
    Private Sub SoundButt18_MouseDragDrop(sender As Object, e As DragEventArgs) Handles SoundButt18.DragDrop
        ButtDragDrop(18, e)
    End Sub

    'Sound Button 19
    Private Sub SoundButt19_MouseDown(sender As Object, e As EventArgs) Handles SoundButt19.MouseDown
        ButtMouseDown(19)
    End Sub
    Private Sub SoundButt19_MouseLeave(sender As Object, e As EventArgs) Handles SoundButt19.MouseLeave
        ButtMouseLeave(19)
    End Sub
    Private Sub SoundButt19_MouseDragEnter(sender As Object, e As DragEventArgs) Handles SoundButt19.DragEnter
        ButtDragEnter(19, e)
    End Sub
    Private Sub SoundButt19_MouseDragDrop(sender As Object, e As DragEventArgs) Handles SoundButt19.DragDrop
        ButtDragDrop(19, e)
    End Sub

    'Sound Button 20
    Private Sub SoundButt20_MouseDown(sender As Object, e As EventArgs) Handles SoundButt20.MouseDown
        ButtMouseDown(20)
    End Sub
    Private Sub SoundButt20_MouseLeave(sender As Object, e As EventArgs) Handles SoundButt20.MouseLeave
        ButtMouseLeave(20)
    End Sub
    Private Sub SoundButt20_MouseDragEnter(sender As Object, e As DragEventArgs) Handles SoundButt20.DragEnter
        ButtDragEnter(20, e)
    End Sub
    Private Sub SoundButt20_MouseDragDrop(sender As Object, e As DragEventArgs) Handles SoundButt20.DragDrop
        ButtDragDrop(20, e)
    End Sub

    'Sound Button 21
    Private Sub SoundButt21_MouseDown(sender As Object, e As EventArgs) Handles SoundButt21.MouseDown
        ButtMouseDown(21)
    End Sub
    Private Sub SoundButt21_MouseLeave(sender As Object, e As EventArgs) Handles SoundButt21.MouseLeave
        ButtMouseLeave(21)
    End Sub
    Private Sub SoundButt21_MouseDragEnter(sender As Object, e As DragEventArgs) Handles SoundButt21.DragEnter
        ButtDragEnter(21, e)
    End Sub
    Private Sub SoundButt21_MouseDragDrop(sender As Object, e As DragEventArgs) Handles SoundButt21.DragDrop
        ButtDragDrop(21, e)
    End Sub

    'Sound Button 22
    Private Sub SoundButt22_MouseDown(sender As Object, e As EventArgs) Handles SoundButt22.MouseDown
        ButtMouseDown(22)
    End Sub
    Private Sub SoundButt22_MouseLeave(sender As Object, e As EventArgs) Handles SoundButt22.MouseLeave
        ButtMouseLeave(22)
    End Sub
    Private Sub SoundButt22_MouseDragEnter(sender As Object, e As DragEventArgs) Handles SoundButt22.DragEnter
        ButtDragEnter(22, e)
    End Sub
    Private Sub SoundButt22_MouseDragDrop(sender As Object, e As DragEventArgs) Handles SoundButt22.DragDrop
        ButtDragDrop(22, e)
    End Sub

    'Sound Button 23
    Private Sub SoundButt23_MouseDown(sender As Object, e As EventArgs) Handles SoundButt23.MouseDown
        ButtMouseDown(23)
    End Sub
    Private Sub SoundButt23_MouseLeave(sender As Object, e As EventArgs) Handles SoundButt23.MouseLeave
        ButtMouseLeave(23)
    End Sub
    Private Sub SoundButt23_MouseDragEnter(sender As Object, e As DragEventArgs) Handles SoundButt23.DragEnter
        ButtDragEnter(23, e)
    End Sub
    Private Sub SoundButt23_MouseDragDrop(sender As Object, e As DragEventArgs) Handles SoundButt23.DragDrop
        ButtDragDrop(23, e)
    End Sub

    'Sound Button 24
    Private Sub SoundButt24_MouseDown(sender As Object, e As EventArgs) Handles SoundButt24.MouseDown
        ButtMouseDown(24)
    End Sub
    Private Sub SoundButt24_MouseLeave(sender As Object, e As EventArgs) Handles SoundButt24.MouseLeave
        ButtMouseLeave(24)
    End Sub
    Private Sub SoundButt24_MouseDragEnter(sender As Object, e As DragEventArgs) Handles SoundButt24.DragEnter
        ButtDragEnter(24, e)
    End Sub
    Private Sub SoundButt24_MouseDragDrop(sender As Object, e As DragEventArgs) Handles SoundButt24.DragDrop
        ButtDragDrop(24, e)
    End Sub

    'Sound Button 25
    Private Sub SoundButt25_MouseDown(sender As Object, e As EventArgs) Handles SoundButt25.MouseDown
        ButtMouseDown(25)
    End Sub
    Private Sub SoundButt25_MouseLeave(sender As Object, e As EventArgs) Handles SoundButt25.MouseLeave
        ButtMouseLeave(25)
    End Sub
    Private Sub SoundButt25_MouseDragEnter(sender As Object, e As DragEventArgs) Handles SoundButt25.DragEnter
        ButtDragEnter(25, e)
    End Sub
    Private Sub SoundButt25_MouseDragDrop(sender As Object, e As DragEventArgs) Handles SoundButt25.DragDrop
        ButtDragDrop(25, e)
    End Sub

    'Sound Button 26
    Private Sub SoundButt26_MouseDown(sender As Object, e As EventArgs) Handles SoundButt26.MouseDown
        ButtMouseDown(26)
    End Sub
    Private Sub SoundButt26_MouseLeave(sender As Object, e As EventArgs) Handles SoundButt26.MouseLeave
        ButtMouseLeave(26)
    End Sub
    Private Sub SoundButt26_MouseDragEnter(sender As Object, e As DragEventArgs) Handles SoundButt26.DragEnter
        ButtDragEnter(26, e)
    End Sub
    Private Sub SoundButt26_MouseDragDrop(sender As Object, e As DragEventArgs) Handles SoundButt26.DragDrop
        ButtDragDrop(26, e)
    End Sub

    'Sound Button 27
    Private Sub SoundButt27_MouseDown(sender As Object, e As EventArgs) Handles SoundButt27.MouseDown
        ButtMouseDown(27)
    End Sub
    Private Sub SoundButt27_MouseLeave(sender As Object, e As EventArgs) Handles SoundButt27.MouseLeave
        ButtMouseLeave(27)
    End Sub
    Private Sub SoundButt27_MouseDragEnter(sender As Object, e As DragEventArgs) Handles SoundButt27.DragEnter
        ButtDragEnter(27, e)
    End Sub
    Private Sub SoundButt27_MouseDragDrop(sender As Object, e As DragEventArgs) Handles SoundButt27.DragDrop
        ButtDragDrop(27, e)
    End Sub

    'Sound Button 28
    Private Sub SoundButt28_MouseDown(sender As Object, e As EventArgs) Handles SoundButt28.MouseDown
        ButtMouseDown(28)
    End Sub
    Private Sub SoundButt28_MouseLeave(sender As Object, e As EventArgs) Handles SoundButt28.MouseLeave
        ButtMouseLeave(28)
    End Sub
    Private Sub SoundButt28_MouseDragEnter(sender As Object, e As DragEventArgs) Handles SoundButt28.DragEnter
        ButtDragEnter(28, e)
    End Sub
    Private Sub SoundButt28_MouseDragDrop(sender As Object, e As DragEventArgs) Handles SoundButt28.DragDrop
        ButtDragDrop(28, e)
    End Sub

    'Sound Button 29
    Private Sub SoundButt29_MouseDown(sender As Object, e As EventArgs) Handles SoundButt29.MouseDown
        ButtMouseDown(29)
    End Sub
    Private Sub SoundButt29_MouseLeave(sender As Object, e As EventArgs) Handles SoundButt29.MouseLeave
        ButtMouseLeave(29)
    End Sub
    Private Sub SoundButt29_MouseDragEnter(sender As Object, e As DragEventArgs) Handles SoundButt29.DragEnter
        ButtDragEnter(29, e)
    End Sub
    Private Sub SoundButt29_MouseDragDrop(sender As Object, e As DragEventArgs) Handles SoundButt29.DragDrop
        ButtDragDrop(29, e)
    End Sub

    'Sound Button 30
    Private Sub SoundButt30_MouseDown(sender As Object, e As EventArgs) Handles SoundButt30.MouseDown
        ButtMouseDown(30)
    End Sub
    Private Sub SoundButt30_MouseLeave(sender As Object, e As EventArgs) Handles SoundButt30.MouseLeave
        ButtMouseLeave(30)
    End Sub
    Private Sub SoundButt30_MouseDragEnter(sender As Object, e As DragEventArgs) Handles SoundButt30.DragEnter
        ButtDragEnter(30, e)
    End Sub
    Private Sub SoundButt30_MouseDragDrop(sender As Object, e As DragEventArgs) Handles SoundButt30.DragDrop
        ButtDragDrop(30, e)
    End Sub

    'Sound Button 31
    Private Sub SoundButt31_MouseDown(sender As Object, e As EventArgs) Handles SoundButt31.MouseDown
        ButtMouseDown(31)
    End Sub
    Private Sub SoundButt31_MouseLeave(sender As Object, e As EventArgs) Handles SoundButt31.MouseLeave
        ButtMouseLeave(31)
    End Sub
    Private Sub SoundButt31_MouseDragEnter(sender As Object, e As DragEventArgs) Handles SoundButt31.DragEnter
        ButtDragEnter(31, e)
    End Sub
    Private Sub SoundButt31_MouseDragDrop(sender As Object, e As DragEventArgs) Handles SoundButt31.DragDrop
        ButtDragDrop(31, e)
    End Sub

    'Sound Button 32
    Private Sub SoundButt32_MouseDown(sender As Object, e As EventArgs) Handles SoundButt32.MouseDown
        ButtMouseDown(32)
    End Sub
    Private Sub SoundButt32_MouseLeave(sender As Object, e As EventArgs) Handles SoundButt32.MouseLeave
        ButtMouseLeave(32)
    End Sub
    Private Sub SoundButt32_MouseDragEnter(sender As Object, e As DragEventArgs) Handles SoundButt32.DragEnter
        ButtDragEnter(32, e)
    End Sub
    Private Sub SoundButt32_MouseDragDrop(sender As Object, e As DragEventArgs) Handles SoundButt32.DragDrop
        ButtDragDrop(32, e)
    End Sub

    'Sound Button 33
    Private Sub SoundButt33_MouseDown(sender As Object, e As EventArgs) Handles SoundButt33.MouseDown
        ButtMouseDown(33)
    End Sub
    Private Sub SoundButt33_MouseLeave(sender As Object, e As EventArgs) Handles SoundButt33.MouseLeave
        ButtMouseLeave(33)
    End Sub
    Private Sub SoundButt33_MouseDragEnter(sender As Object, e As DragEventArgs) Handles SoundButt33.DragEnter
        ButtDragEnter(33, e)
    End Sub
    Private Sub SoundButt33_MouseDragDrop(sender As Object, e As DragEventArgs) Handles SoundButt33.DragDrop
        ButtDragDrop(33, e)
    End Sub

    'Sound Button 34
    Private Sub SoundButt34_MouseDown(sender As Object, e As EventArgs) Handles SoundButt34.MouseDown
        ButtMouseDown(34)
    End Sub
    Private Sub SoundButt34_MouseLeave(sender As Object, e As EventArgs) Handles SoundButt34.MouseLeave
        ButtMouseLeave(34)
    End Sub
    Private Sub SoundButt34_MouseDragEnter(sender As Object, e As DragEventArgs) Handles SoundButt34.DragEnter
        ButtDragEnter(34, e)
    End Sub
    Private Sub SoundButt34_MouseDragDrop(sender As Object, e As DragEventArgs) Handles SoundButt34.DragDrop
        ButtDragDrop(34, e)
    End Sub

    'Sound Button 35
    Private Sub SoundButt35_MouseDown(sender As Object, e As EventArgs) Handles SoundButt35.MouseDown
        ButtMouseDown(35)
    End Sub
    Private Sub SoundButt35_MouseLeave(sender As Object, e As EventArgs) Handles SoundButt35.MouseLeave
        ButtMouseLeave(35)
    End Sub
    Private Sub SoundButt35_MouseDragEnter(sender As Object, e As DragEventArgs) Handles SoundButt35.DragEnter
        ButtDragEnter(35, e)
    End Sub
    Private Sub SoundButt35_MouseDragDrop(sender As Object, e As DragEventArgs) Handles SoundButt35.DragDrop
        ButtDragDrop(35, e)
    End Sub



    Private Sub ButtMouseDown(ButtNum As Integer)
        ButtonHold(ButtNum) = True
    End Sub

    Private Sub ButtMouseLeave(ButtNumb As Integer)
        If ButtonHold(ButtNumb) Then
            If AudioControl.SoundBoards(SourceIndex).Buttons(ButtNumb) <> "" Then
                SourceButtonNumber = ButtNumb
                Dim MyButton As Button = Me.Controls("SoundButt" & ButtNumb)
                MyButton.DoDragDrop(AudioControl.SoundBoards(SourceIndex).Buttons(ButtNumb), DragDropEffects.Copy)
            End If
        End If
        ButtonHold(ButtNumb) = False
    End Sub

    Private Sub SoundListBox_TextChanged(sender As Object, e As EventArgs) Handles SoundListBox.TextChanged

    End Sub

    Private Sub ButtDragEnter(ButtNum As Integer, e As DragEventArgs)
        If (e.Data.GetDataPresent(DataFormats.Text)) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub ButtDragDrop(ButtNumb As Integer, e As DragEventArgs)
        If SourceButtonNumber > -1 Then
            AudioControl.SwapSoundBoardButton(SourceIndex, SourceButtonNumber, ButtNumb)
            SourceButtonNumber = -1
        Else
            AudioControl.UpdateSoundBoardButton(SourceIndex, ButtNumb, e.Data.GetData(DataFormats.Text))
        End If
    End Sub

    Private Sub PlaySound(SoundIndex As Integer)
        If AudioControl.SoundBoards IsNot Nothing Then
            Dim SoundFile As String = AudioControl.GetSoundFileDataByName(AudioControl.SoundBoards(SourceIndex).Buttons(SoundIndex))
            If SoundFile <> "" Then
                AudioControl.SoundPlayer.Play(SoundFile)
            End If
        End If
        ButtonHold(SoundIndex) = False
    End Sub


End Class