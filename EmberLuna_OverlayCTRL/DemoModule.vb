Public Class DemoModule
    Private Sub buttclick1(sender As Object, e As EventArgs) Handles ADD.Click
        MyOBSevents.ViewerEvent(UserEvents.NewFollower, RandomUsername)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        MyOBSevents.ViewerEvent(UserEvents.NewSubscriber, RandomUsername)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        MyOBSevents.ViewerEvent(UserEvents.RaidDetected, RandomUsername, RandomInt(5, 1000))
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        MyOBSevents.Roll6(1, RandomUsername)
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        MyOBSevents.Roll6(2, RandomUsername)
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        MyOBSevents.Roll6(3, RandomUsername)
    End Sub

    Private Sub Button23_Click(sender As Object, e As EventArgs) Handles Button23.Click
        MyOBSevents.PlayHats()
    End Sub

    Private Sub Button20_Click(sender As Object, e As EventArgs) Handles Button20.Click
        MyOBSevents.PlaySoundAlert(RandomUsername)
    End Sub

    Private Sub Button21_Click(sender As Object, e As EventArgs) Handles Button21.Click
        Luna.ChangeMood(Luna.Mood.PooBrain, 4050,, "Luna Poo Brain")
    End Sub

    Private Sub Button22_Click(sender As Object, e As EventArgs) Handles Button22.Click
        Dim Speak As Task = Ember.Says("ROCK AND STONE", Ember.Mood.RockandStone, "Rock And Stone",, 2800)
    End Sub
End Class