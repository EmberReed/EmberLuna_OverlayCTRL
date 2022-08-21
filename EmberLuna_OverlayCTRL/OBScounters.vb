Public Class OBScounters

    Private SourceIndex As Integer = 0, ValuesInitialised As Boolean = False
    Private DisplayTotal As Integer = 0
    Private TotalCounters As Integer = 0
    Private SizeIncrement As Integer = 71
    Private ButtonsWide As Integer = 5
    Private ButtonsHigh As Integer = 6
    Private BaseHeight As Integer = 175

    Private Sub OBScounters_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DisplayTotal = ButtonsWide * ButtonsHigh
        SourceWindow.CountersButt.BackColor = ActiveBUTT
        Call DisplayCounters()
        AddHandler MyResourceManager.TaskEvent, AddressOf CounterEvent
        'AddHandler CounterData.CounterStarted, AddressOf CounterStarted
        'AddHandler CounterData.CounterStopped, AddressOf CounterStopped
    End Sub

    Private Sub OBScounters_Closing(sender As Object, e As FormClosingEventArgs) Handles MyBase.Closing
        'RemoveHandler CounterData.CounterStarted, AddressOf CounterStarted
        'RemoveHandler CounterData.CounterStopped, AddressOf CounterStopped
        RemoveHandler MyResourceManager.TaskEvent, AddressOf CounterEvent
        SourceWindow.CountersButt.BackColor = StandardBUTT
    End Sub

    Private Sub SAVE_Click(sender As Object, e As EventArgs) Handles SAVE.Click
        CounterData.SaveCounterData()
    End Sub

    Public Sub CounterEvent(TaskName As String, TaskEnum As Integer)
        If InStr(TaskName, "CountIndex#(") > 0 Then
            Select Case TaskEnum
                Case = RMtaskStates.Started
                    CounterStarted(DataExtract(TaskName, "CountIndex#(", ")"))
                Case = RMtaskStates.Ended
                    CounterStopped(DataExtract(TaskName, "CountIndex#(", ")"))
            End Select
        End If
    End Sub

    Public Sub DisplayCounters()
        SetDrawing(Me.Handle, WM_SETREDRAW, False, 0)
        ValuesInitialised = False
        TotalCounters = CounterData.Total
        Dim OutputString As String
        Dim StartIndex As Integer = SourceIndex * DisplayTotal
        'Dim StopIndex As Integer = ((SourceIndex + 1) * 36) - 1
        Dim CounterLabel As Label, CounterValue As NumericUpDown
        Dim ActualIndex As Integer
        For i As Integer = 0 To DisplayTotal - 1
            ActualIndex = i + StartIndex
            If ActualIndex < CounterData.Total Then
                OutputString = CounterData.Counters(ActualIndex).Name
            Else
                OutputString = ""
            End If
            CounterLabel = Controls("CounterLabel" & i)
            CounterValue = Controls("CounterValue" & i)
            CounterLabel.Text = OutputString
            If OutputString <> "" Then
                CounterValue.Visible = True
                CounterValue.Value = CounterData.Counters(ActualIndex).Count
                CounterValue.Increment = CounterData.Counters(ActualIndex).Increment
            Else
                CounterValue.Visible = False
                CounterValue.Increment = 0
            End If
        Next
        Dim InputSize As Size = Me.Size
        InputSize.Height = GetFormHeight(TotalCounters - StartIndex, ButtonsWide, ButtonsHigh, BaseHeight, SizeIncrement)
        Me.Size = InputSize
        ValuesInitialised = True
        SetDrawing(Me.Handle, WM_SETREDRAW, True, 0)
        Me.Refresh()
    End Sub

    Private Sub CounterValueChanged(CounterIndex As Integer)
        Dim CounterValue As NumericUpDown = Controls("CounterValue" & CounterIndex)
        CounterValue.Enabled = False
        CounterIndex = CounterIndex + (SourceIndex * DisplayTotal)
        If ValuesInitialised = True Then
            If CounterIndex < CounterData.Total Then
                If CounterValue.Value > CounterData.Counters(CounterIndex).Count Then
                    If DisableCountEvent.Checked = False Then
                        CounterData.CounterAdd(CounterIndex)
                    Else
                        CounterData.Counters(CounterIndex).AddCount()
                        CounterValue.Enabled = True
                    End If
                ElseIf CounterValue.Value < CounterData.Counters(CounterIndex).Count Then
                    CounterData.CounterSub(CounterIndex)
                    CounterValue.Enabled = True
                ElseIf CounterValue.Value = CounterData.Counters(CounterIndex).Count Then
                    CounterValue.Enabled = True
                End If
            Else
                CounterValue.Enabled = True
            End If
        Else
            CounterValue.Value = CounterData.Counters(CounterIndex).Count
            CounterValue.Enabled = True
        End If
    End Sub

    Private Sub CounterStarted(CounterIndex As Integer)
        BeginInvoke(
        Sub()
            Dim CounterLabel As Label ', CounterValue As NumericUpDown
            If CounterIndex <> -1 Then
                CounterIndex = CounterIndex - (SourceIndex * DisplayTotal)
                CounterLabel = Controls("CounterLabel" & CounterIndex)
                'CounterValue = Controls("CounterValue" & CounterIndex)
                If CounterLabel.Text = CounterData.Counters(CounterIndex).Name Then
                    CounterLabel.BackColor = ActiveBUTT
                    'CounterValue.Value = CounterData.Counters(CounterIndex).Count
                End If
            End If
        End Sub)
    End Sub

    Private Sub CounterStopped(CounterIndex As Integer)
        BeginInvoke(
        Sub()
            Dim CounterLabel As Label, CounterValue As NumericUpDown
            If CounterIndex <> -1 Then
                CounterIndex = CounterIndex - (SourceIndex * DisplayTotal)
                CounterLabel = Controls("CounterLabel" & CounterIndex)
                CounterValue = Controls("CounterValue" & CounterIndex)
                If CounterLabel.Text = CounterData.Counters(CounterIndex).Name Then
                    CounterLabel.BackColor = StandardBUTT
                    CounterValue.Enabled = True
                End If
            End If
        End Sub)
    End Sub

    Private Sub CounterMouseDoubleClick(CounterIndex As Integer)
        'Dim CounterValue As NumericUpDown = Controls("CounterValue" & CounterIndex)
        'CounterValue.Enabled = False
        CounterIndex = CounterIndex + (SourceIndex * DisplayTotal)
        'If ValuesInitialised = True Then
        If CounterIndex < CounterData.Total Then
            Dim NewCounter As New AddCounter
            NewCounter.CounterIndex = CounterIndex
            NewCounter.Show()
        End If

    End Sub




    Private Sub CounterValue0_ValueChanged(sender As Object, e As EventArgs) Handles CounterValue0.ValueChanged
        CounterValueChanged(0)
    End Sub
    Private Sub CounterValue1_ValueChanged(sender As Object, e As EventArgs) Handles CounterValue1.ValueChanged
        CounterValueChanged(1)
    End Sub
    Private Sub CounterValue2_ValueChanged(sender As Object, e As EventArgs) Handles CounterValue2.ValueChanged
        CounterValueChanged(2)
    End Sub
    Private Sub CounterValue3_ValueChanged(sender As Object, e As EventArgs) Handles CounterValue3.ValueChanged
        CounterValueChanged(3)
    End Sub
    Private Sub CounterValue4_ValueChanged(sender As Object, e As EventArgs) Handles CounterValue4.ValueChanged
        CounterValueChanged(4)
    End Sub
    Private Sub CounterValue5_ValueChanged(sender As Object, e As EventArgs) Handles CounterValue5.ValueChanged
        CounterValueChanged(5)
    End Sub
    Private Sub CounterValue6_ValueChanged(sender As Object, e As EventArgs) Handles CounterValue6.ValueChanged
        CounterValueChanged(6)
    End Sub
    Private Sub CounterValue7_ValueChanged(sender As Object, e As EventArgs) Handles CounterValue7.ValueChanged
        CounterValueChanged(7)
    End Sub
    Private Sub CounterValue8_ValueChanged(sender As Object, e As EventArgs) Handles CounterValue8.ValueChanged
        CounterValueChanged(8)
    End Sub
    Private Sub CounterValue9_ValueChanged(sender As Object, e As EventArgs) Handles CounterValue9.ValueChanged
        CounterValueChanged(9)
    End Sub
    Private Sub CounterValue10_ValueChanged(sender As Object, e As EventArgs) Handles CounterValue10.ValueChanged
        CounterValueChanged(10)
    End Sub
    Private Sub CounterValue11ValueChanged(sender As Object, e As EventArgs) Handles CounterValue11.ValueChanged
        CounterValueChanged(11)
    End Sub
    Private Sub CounterValue12_ValueChanged(sender As Object, e As EventArgs) Handles CounterValue12.ValueChanged
        CounterValueChanged(12)
    End Sub
    Private Sub CounterValue13_ValueChanged(sender As Object, e As EventArgs) Handles CounterValue13.ValueChanged
        CounterValueChanged(13)
    End Sub
    Private Sub CounterValue14_ValueChanged(sender As Object, e As EventArgs) Handles CounterValue14.ValueChanged
        CounterValueChanged(14)
    End Sub
    Private Sub CounterValue15_ValueChanged(sender As Object, e As EventArgs) Handles CounterValue15.ValueChanged
        CounterValueChanged(15)
    End Sub
    Private Sub CounterValue16_ValueChanged(sender As Object, e As EventArgs) Handles CounterValue16.ValueChanged
        CounterValueChanged(16)
    End Sub
    Private Sub CounterValue17_ValueChanged(sender As Object, e As EventArgs) Handles CounterValue17.ValueChanged
        CounterValueChanged(17)
    End Sub
    Private Sub CounterValue18_ValueChanged(sender As Object, e As EventArgs) Handles CounterValue18.ValueChanged
        CounterValueChanged(18)
    End Sub
    Private Sub CounterValue19_ValueChanged(sender As Object, e As EventArgs) Handles CounterValue19.ValueChanged
        CounterValueChanged(19)
    End Sub
    Private Sub CounterValue20_ValueChanged(sender As Object, e As EventArgs) Handles CounterValue20.ValueChanged
        CounterValueChanged(20)
    End Sub
    Private Sub CounterValue21_ValueChanged(sender As Object, e As EventArgs) Handles CounterValue21.ValueChanged
        CounterValueChanged(21)
    End Sub
    Private Sub CounterValue22_ValueChanged(sender As Object, e As EventArgs) Handles CounterValue22.ValueChanged
        CounterValueChanged(22)
    End Sub
    Private Sub CounterValue23_ValueChanged(sender As Object, e As EventArgs) Handles CounterValue23.ValueChanged
        CounterValueChanged(23)
    End Sub
    Private Sub CounterValue24_ValueChanged(sender As Object, e As EventArgs) Handles CounterValue24.ValueChanged
        CounterValueChanged(24)
    End Sub
    Private Sub CounterValue25_ValueChanged(sender As Object, e As EventArgs) Handles CounterValue25.ValueChanged
        CounterValueChanged(25)
    End Sub
    Private Sub CounterValue26_ValueChanged(sender As Object, e As EventArgs) Handles CounterValue26.ValueChanged
        CounterValueChanged(26)
    End Sub
    Private Sub CounterValue27_ValueChanged(sender As Object, e As EventArgs) Handles CounterValue27.ValueChanged
        CounterValueChanged(27)
    End Sub

    Private Sub CounterValue28_ValueChanged(sender As Object, e As EventArgs) Handles CounterValue28.ValueChanged
        CounterValueChanged(28)
    End Sub
    Private Sub CounterValue29_ValueChanged(sender As Object, e As EventArgs) Handles CounterValue29.ValueChanged
        CounterValueChanged(29)
    End Sub



    Private Sub CounterLabel0_MouseDoubleClick(sender As Object, e As EventArgs) Handles CounterLabel0.MouseDoubleClick
        CounterMouseDoubleClick(0)
    End Sub
    Private Sub CounterLabel1_MouseDoubleClick(sender As Object, e As EventArgs) Handles CounterLabel1.MouseDoubleClick
        CounterMouseDoubleClick(1)
    End Sub
    Private Sub CounterLabel2_MouseDoubleClick(sender As Object, e As EventArgs) Handles CounterLabel2.MouseDoubleClick
        CounterMouseDoubleClick(2)
    End Sub
    Private Sub CounterLabel3_MouseDoubleClick(sender As Object, e As EventArgs) Handles CounterLabel3.MouseDoubleClick
        CounterMouseDoubleClick(3)
    End Sub
    Private Sub CounterLabel4_MouseDoubleClick(sender As Object, e As EventArgs) Handles CounterLabel4.MouseDoubleClick
        CounterMouseDoubleClick(4)
    End Sub
    Private Sub CounterLabel5_MouseDoubleClick(sender As Object, e As EventArgs) Handles CounterLabel5.MouseDoubleClick
        CounterMouseDoubleClick(5)
    End Sub
    Private Sub CounterLabel6_MouseDoubleClick(sender As Object, e As EventArgs) Handles CounterLabel6.MouseDoubleClick
        CounterMouseDoubleClick(6)
    End Sub
    Private Sub CounterLabel7_MouseDoubleClick(sender As Object, e As EventArgs) Handles CounterLabel7.MouseDoubleClick
        CounterMouseDoubleClick(7)
    End Sub
    Private Sub CounterLabel8_MouseDoubleClick(sender As Object, e As EventArgs) Handles CounterLabel8.MouseDoubleClick
        CounterMouseDoubleClick(8)
    End Sub
    Private Sub CounterLabel9_MouseDoubleClick(sender As Object, e As EventArgs) Handles CounterLabel9.MouseDoubleClick
        CounterMouseDoubleClick(9)
    End Sub
    Private Sub CounterLabel10_MouseDoubleClick(sender As Object, e As EventArgs) Handles CounterLabel10.MouseDoubleClick
        CounterMouseDoubleClick(10)
    End Sub
    Private Sub CounterLabel11MouseDoubleClick(sender As Object, e As EventArgs) Handles CounterLabel11.MouseDoubleClick
        CounterMouseDoubleClick(11)
    End Sub
    Private Sub CounterLabel12_MouseDoubleClick(sender As Object, e As EventArgs) Handles CounterLabel12.MouseDoubleClick
        CounterMouseDoubleClick(12)
    End Sub
    Private Sub CounterLabel13_MouseDoubleClick(sender As Object, e As EventArgs) Handles CounterLabel13.MouseDoubleClick
        CounterMouseDoubleClick(13)
    End Sub
    Private Sub CounterLabel14_MouseDoubleClick(sender As Object, e As EventArgs) Handles CounterLabel14.MouseDoubleClick
        CounterMouseDoubleClick(14)
    End Sub
    Private Sub CounterLabel15_MouseDoubleClick(sender As Object, e As EventArgs) Handles CounterLabel15.MouseDoubleClick
        CounterMouseDoubleClick(15)
    End Sub
    Private Sub CounterLabel16_MouseDoubleClick(sender As Object, e As EventArgs) Handles CounterLabel16.MouseDoubleClick
        CounterMouseDoubleClick(16)
    End Sub
    Private Sub CounterLabel17_MouseDoubleClick(sender As Object, e As EventArgs) Handles CounterLabel17.MouseDoubleClick
        CounterMouseDoubleClick(17)
    End Sub
    Private Sub CounterLabel18_MouseDoubleClick(sender As Object, e As EventArgs) Handles CounterLabel18.MouseDoubleClick
        CounterMouseDoubleClick(18)
    End Sub
    Private Sub CounterLabel19_MouseDoubleClick(sender As Object, e As EventArgs) Handles CounterLabel19.MouseDoubleClick
        CounterMouseDoubleClick(19)
    End Sub
    Private Sub CounterLabel20_MouseDoubleClick(sender As Object, e As EventArgs) Handles CounterLabel20.MouseDoubleClick
        CounterMouseDoubleClick(20)
    End Sub
    Private Sub CounterLabel21_MouseDoubleClick(sender As Object, e As EventArgs) Handles CounterLabel21.MouseDoubleClick
        CounterMouseDoubleClick(21)
    End Sub
    Private Sub CounterLabel22_MouseDoubleClick(sender As Object, e As EventArgs) Handles CounterLabel22.MouseDoubleClick
        CounterMouseDoubleClick(22)
    End Sub
    Private Sub CounterLabel23_MouseDoubleClick(sender As Object, e As EventArgs) Handles CounterLabel23.MouseDoubleClick
        CounterMouseDoubleClick(23)
    End Sub
    Private Sub CounterLabel24_MouseDoubleClick(sender As Object, e As EventArgs) Handles CounterLabel24.MouseDoubleClick
        CounterMouseDoubleClick(24)
    End Sub
    Private Sub CounterLabel25_MouseDoubleClick(sender As Object, e As EventArgs) Handles CounterLabel25.MouseDoubleClick
        CounterMouseDoubleClick(25)
    End Sub
    Private Sub CounterLabel26_MouseDoubleClick(sender As Object, e As EventArgs) Handles CounterLabel26.MouseDoubleClick
        CounterMouseDoubleClick(26)
    End Sub
    Private Sub CounterLabel27_MouseDoubleClick(sender As Object, e As EventArgs) Handles CounterLabel27.MouseDoubleClick
        CounterMouseDoubleClick(27)
    End Sub

    Private Sub CounterLabel28_MouseDoubleClick(sender As Object, e As EventArgs) Handles CounterLabel28.MouseDoubleClick
        CounterMouseDoubleClick(28)
    End Sub
    Private Sub CounterLabel29_MouseDoubleClick(sender As Object, e As EventArgs) Handles CounterLabel29.MouseDoubleClick
        CounterMouseDoubleClick(29)
    End Sub

    Private Sub ADD_Click(sender As Object, e As EventArgs) Handles ADD.Click
        Dim NewCounter As New AddCounter
        NewCounter.Show()
    End Sub
End Class