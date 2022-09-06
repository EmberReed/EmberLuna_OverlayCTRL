Public Class AddCounter
    Private NewCounter As CounterProperties
    Public CounterIndex As Integer = -1
    Private Sub AddCounter_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If CounterIndex = -1 Then
            NewCounter = New CounterProperties
            NewCounter.Name = ""
            NewCounter.Label = ""
            NewCounter.Type = 0
            NewCounter.Count = 0
            NewCounter.Increment = 1
            NewCounter.Sound = ""
            NewCounter.OBSevent = ""
            NewCounter.Tick = ""
            NewCounter.PublicCount = False
            NewCounter.NotificationType = False
        Else
            NewCounter = CounterData.Counters(CounterIndex)
            EmberTitle.Text = NewCounter.Name
            TextBox1.Text = NewCounter.Label
            NumericUpDown2.Value = NewCounter.Type
            CounterValue0.Value = NewCounter.Count
            NumericUpDown1.Value = NewCounter.Increment
            ComboBox1.Text = NewCounter.Sound
            ComboBox2.Text = NewCounter.OBSevent
            ComboBox3.Text = NewCounter.Tick
            If NewCounter.PublicCount Then
                PubPriBUTT.Text = "PUBLIC"
            Else
                PubPriBUTT.Text = "PRIVATE"
            End If
        End If

        Dim SoundList As List(Of String) = AudioControl.GetFullSoundFilesList
        For i As Integer = 0 To SoundList.Count - 1
            ComboBox1.Items.Add(SoundList(i))
        Next
        For i As Integer = 0 To AudioControl.TickList.Count - 1
            ComboBox3.Items.Add(AudioControl.TickList(i))
        Next
    End Sub

    Private Sub EmberTitle_TextChanged(sender As Object, e As EventArgs) Handles EmberTitle.TextChanged
        NewCounter.Name = EmberTitle.Text
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        NewCounter.Label = TextBox1.Text
    End Sub

    Private Sub NumericUpDown2_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown2.ValueChanged
        NewCounter.Type = NumericUpDown2.Value
        Select Case NumericUpDown2.Value
            Case = TypeIDs.Glob
                Label4.Text = "GLOBAL"
            Case = TypeIDs.Ember
                Label4.Text = "EMBER"
            Case = TypeIDs.Luna
                Label4.Text = "LUNA"
        End Select
    End Sub

    Private Sub CounterValue0_ValueChanged(sender As Object, e As EventArgs) Handles CounterValue0.ValueChanged
        NewCounter.Count = CounterValue0.Value
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        NewCounter.Increment = NumericUpDown1.Value
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.TextChanged
        NewCounter.Sound = ComboBox1.Text
    End Sub
    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.TextChanged
        NewCounter.OBSevent = ComboBox2.Text
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        Close()
    End Sub

    Private Sub ADDCOUNTER_Click(sender As Object, e As EventArgs) Handles ADD.Click
        If CounterIndex = -1 Then
            CounterData.SaveCounterData()
            CounterData.AppendCounterData(NewCounter)
            CounterData.LoadCounterData()
        Else
            CounterData.Counters(CounterIndex) = NewCounter
        End If
        Counters.BeginInvoke(Sub() Counters.DisplayCounters())
        Close()
    End Sub

    Private Sub PublicPrivate_CheckedChanged(sender As Object, e As EventArgs) Handles PubPriBUTT.Click
        If PubPriBUTT.Text = "PRIVATE" Then
            PubPriBUTT.Text = "PUBLIC"
            NewCounter.PublicCount = True
        Else
            PubPriBUTT.Text = "PRIVATE"
            NewCounter.PublicCount = False
        End If
        'NewCounter.PublicCount = PublicPrivate.Checked
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged_1(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged

    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged
        If ComboBox3.Text <> "" Then AudioControl.SoundPlayer.PlaySound(ComboBox3.Text, SoundSource.BeepBoop)
        NewCounter.Tick = ComboBox3.Text
    End Sub
End Class