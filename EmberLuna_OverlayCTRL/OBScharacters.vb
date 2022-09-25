Imports systen.threading
Public Class OBScharacters

    'Public CharacterBool As Boolean

    'Private Sub EmberAway_Click(sender As Object, e As EventArgs) Handles CharacterSwitch.Click
    '    If CharacterBool = SpriteID.Ember Then
    '        CharacterBool = SpriteID.Luna
    '    Else
    '        CharacterBool = SpriteID.Ember
    '    End If
    '    Call UpdateDisplay()
    'End Sub

    Private Sub OBScharacters_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SourceWindow.CHARACTERS.BackColor = ActiveBUTT
        'CharacterBool = SpriteID.Ember
        AddHandler Ember.MoodChange, AddressOf MoodChangeDetected
        AddHandler Luna.MoodChange, AddressOf MoodChangeDetected
        Call UpdateDisplay()
    End Sub

    Private Sub OBScharacters_closing(sender As Object, e As EventArgs) Handles MyBase.Closing
        SourceWindow.CHARACTERS.BackColor = StandardBUTT
    End Sub

    Public Sub MoodChangeDetected(InputString As String)
TryAgain:
        UpdateDisplay()
    End Sub

    Public Sub UpdateDisplay()

        Select Case Ember.MyMood
            Case = Ember.Mood.Neutral
                E_NeutralBUTT.BackColor = ActiveBUTT
                E_HappyBUTT.BackColor = StandardBUTT
                E_SadgeBUTT.BackColor = StandardBUTT
                E_AngyBUTT.BackColor = StandardBUTT
                E_WumpyBUTT.BackColor = StandardBUTT
                E_WtfBUTT.BackColor = StandardBUTT
                E_RocKnStonE.BackColor = StandardBUTT
                E_Woohoo.BackColor = StandardBUTT
            Case = Ember.Mood.Happy
                E_NeutralBUTT.BackColor = StandardBUTT
                E_HappyBUTT.BackColor = ActiveBUTT
                E_SadgeBUTT.BackColor = StandardBUTT
                E_AngyBUTT.BackColor = StandardBUTT
                E_WumpyBUTT.BackColor = StandardBUTT
                E_WtfBUTT.BackColor = StandardBUTT
                E_RocKnStonE.BackColor = StandardBUTT
                E_Woohoo.BackColor = StandardBUTT
            Case = Ember.Mood.Sadge
                E_NeutralBUTT.BackColor = StandardBUTT
                E_HappyBUTT.BackColor = StandardBUTT
                E_SadgeBUTT.BackColor = ActiveBUTT
                E_AngyBUTT.BackColor = StandardBUTT
                E_WumpyBUTT.BackColor = StandardBUTT
                E_WtfBUTT.BackColor = StandardBUTT
                E_RocKnStonE.BackColor = StandardBUTT
                E_Woohoo.BackColor = StandardBUTT
            Case = Ember.Mood.Angy
                E_NeutralBUTT.BackColor = StandardBUTT
                E_HappyBUTT.BackColor = StandardBUTT
                E_SadgeBUTT.BackColor = StandardBUTT
                E_AngyBUTT.BackColor = ActiveBUTT
                E_WumpyBUTT.BackColor = StandardBUTT
                E_WtfBUTT.BackColor = StandardBUTT
                E_RocKnStonE.BackColor = StandardBUTT
                E_Woohoo.BackColor = StandardBUTT
            Case = Ember.Mood.Wumpy
                E_NeutralBUTT.BackColor = StandardBUTT
                E_HappyBUTT.BackColor = StandardBUTT
                E_SadgeBUTT.BackColor = StandardBUTT
                E_AngyBUTT.BackColor = StandardBUTT
                E_WumpyBUTT.BackColor = ActiveBUTT
                E_WtfBUTT.BackColor = StandardBUTT
                E_RocKnStonE.BackColor = StandardBUTT
                E_Woohoo.BackColor = StandardBUTT
            Case = Ember.Mood.WTF
                E_NeutralBUTT.BackColor = StandardBUTT
                E_HappyBUTT.BackColor = StandardBUTT
                E_SadgeBUTT.BackColor = StandardBUTT
                E_AngyBUTT.BackColor = StandardBUTT
                E_WumpyBUTT.BackColor = StandardBUTT
                E_WtfBUTT.BackColor = ActiveBUTT
                E_RocKnStonE.BackColor = StandardBUTT
                E_Woohoo.BackColor = StandardBUTT
            Case = Ember.Mood.RockandStone
                E_NeutralBUTT.BackColor = StandardBUTT
                E_HappyBUTT.BackColor = StandardBUTT
                E_SadgeBUTT.BackColor = StandardBUTT
                E_AngyBUTT.BackColor = StandardBUTT
                E_WumpyBUTT.BackColor = StandardBUTT
                E_WtfBUTT.BackColor = StandardBUTT
                E_RocKnStonE.BackColor = ActiveBUTT
                E_Woohoo.BackColor = StandardBUTT
            Case = Ember.Mood.WooHoo
                E_NeutralBUTT.BackColor = StandardBUTT
                E_HappyBUTT.BackColor = StandardBUTT
                E_SadgeBUTT.BackColor = StandardBUTT
                E_AngyBUTT.BackColor = StandardBUTT
                E_WumpyBUTT.BackColor = StandardBUTT
                E_WtfBUTT.BackColor = StandardBUTT
                E_RocKnStonE.BackColor = StandardBUTT
                E_Woohoo.BackColor = ActiveBUTT
        End Select

        If Ember.LeftHand Then
            E_LefthBUTT.BackColor = ActiveBUTT
        Else
            E_LefthBUTT.BackColor = StandardBUTT
        End If

        If Ember.RightHand Then
            E_RighthBUTT.BackColor = ActiveBUTT
        Else
            E_RighthBUTT.BackColor = StandardBUTT
        End If

        Select Case Luna.MyMood
            Case = Luna.Mood.Neutral
                L_NeutralBUTT.BackColor = ActiveBUTT
                L_HappyBUTT.BackColor = StandardBUTT
                L_SadgeBUTT.BackColor = StandardBUTT
                L_AngyBUTT.BackColor = StandardBUTT
                L_WumpyBUTT.BackColor = StandardBUTT
                L_CringeBUTT.BackColor = StandardBUTT
                L_WowBUTT.BackColor = StandardBUTT
                L_OmgBUTT.BackColor = StandardBUTT
                L_SparkleBUTT.BackColor = StandardBUTT
                L_DeepBreath.BackColor = StandardBUTT
                L_WibbleBUTT.BackColor = StandardBUTT
                L_PooBrain.BackColor = StandardBUTT
                L_Woohoo.BackColor = StandardBUTT
            Case = Luna.Mood.Happy
                L_NeutralBUTT.BackColor = StandardBUTT
                L_HappyBUTT.BackColor = ActiveBUTT
                L_SadgeBUTT.BackColor = StandardBUTT
                L_AngyBUTT.BackColor = StandardBUTT
                L_WumpyBUTT.BackColor = StandardBUTT
                L_CringeBUTT.BackColor = StandardBUTT
                L_WowBUTT.BackColor = StandardBUTT
                L_OmgBUTT.BackColor = StandardBUTT
                L_SparkleBUTT.BackColor = StandardBUTT
                L_DeepBreath.BackColor = StandardBUTT
                L_WibbleBUTT.BackColor = StandardBUTT
                L_PooBrain.BackColor = StandardBUTT
                L_Woohoo.BackColor = StandardBUTT
            Case = Luna.Mood.Sadge
                L_NeutralBUTT.BackColor = StandardBUTT
                L_HappyBUTT.BackColor = StandardBUTT
                L_SadgeBUTT.BackColor = ActiveBUTT
                L_AngyBUTT.BackColor = StandardBUTT
                L_WumpyBUTT.BackColor = StandardBUTT
                L_CringeBUTT.BackColor = StandardBUTT
                L_WowBUTT.BackColor = StandardBUTT
                L_OmgBUTT.BackColor = StandardBUTT
                L_SparkleBUTT.BackColor = StandardBUTT
                L_DeepBreath.BackColor = StandardBUTT
                L_WibbleBUTT.BackColor = StandardBUTT
                L_PooBrain.BackColor = StandardBUTT
                L_Woohoo.BackColor = StandardBUTT
            Case = Luna.Mood.Angy
                L_NeutralBUTT.BackColor = StandardBUTT
                L_HappyBUTT.BackColor = StandardBUTT
                L_SadgeBUTT.BackColor = StandardBUTT
                L_AngyBUTT.BackColor = ActiveBUTT
                L_WumpyBUTT.BackColor = StandardBUTT
                L_CringeBUTT.BackColor = StandardBUTT
                L_WowBUTT.BackColor = StandardBUTT
                L_OmgBUTT.BackColor = StandardBUTT
                L_SparkleBUTT.BackColor = StandardBUTT
                L_DeepBreath.BackColor = StandardBUTT
                L_WibbleBUTT.BackColor = StandardBUTT
                L_PooBrain.BackColor = StandardBUTT
                L_Woohoo.BackColor = StandardBUTT
            Case = Luna.Mood.Wumpy
                L_NeutralBUTT.BackColor = StandardBUTT
                L_HappyBUTT.BackColor = StandardBUTT
                L_SadgeBUTT.BackColor = StandardBUTT
                L_AngyBUTT.BackColor = StandardBUTT
                L_WumpyBUTT.BackColor = ActiveBUTT
                L_CringeBUTT.BackColor = StandardBUTT
                L_WowBUTT.BackColor = StandardBUTT
                L_OmgBUTT.BackColor = StandardBUTT
                L_SparkleBUTT.BackColor = StandardBUTT
                L_DeepBreath.BackColor = StandardBUTT
                L_WibbleBUTT.BackColor = StandardBUTT
                L_PooBrain.BackColor = StandardBUTT
                L_Woohoo.BackColor = StandardBUTT
            Case = Luna.Mood.Cringe
                L_NeutralBUTT.BackColor = StandardBUTT
                L_HappyBUTT.BackColor = StandardBUTT
                L_SadgeBUTT.BackColor = StandardBUTT
                L_AngyBUTT.BackColor = StandardBUTT
                L_WumpyBUTT.BackColor = StandardBUTT
                L_CringeBUTT.BackColor = ActiveBUTT
                L_WowBUTT.BackColor = StandardBUTT
                L_OmgBUTT.BackColor = StandardBUTT
                L_SparkleBUTT.BackColor = StandardBUTT
                L_DeepBreath.BackColor = StandardBUTT
                L_WibbleBUTT.BackColor = StandardBUTT
                L_PooBrain.BackColor = StandardBUTT
                L_Woohoo.BackColor = StandardBUTT
            Case = Luna.Mood.Wow
                L_NeutralBUTT.BackColor = StandardBUTT
                L_HappyBUTT.BackColor = StandardBUTT
                L_SadgeBUTT.BackColor = StandardBUTT
                L_AngyBUTT.BackColor = StandardBUTT
                L_WumpyBUTT.BackColor = StandardBUTT
                L_CringeBUTT.BackColor = StandardBUTT
                L_WowBUTT.BackColor = ActiveBUTT
                L_OmgBUTT.BackColor = StandardBUTT
                L_SparkleBUTT.BackColor = StandardBUTT
                L_DeepBreath.BackColor = StandardBUTT
                L_WibbleBUTT.BackColor = StandardBUTT
                L_PooBrain.BackColor = StandardBUTT
                L_Woohoo.BackColor = StandardBUTT
            Case = Luna.Mood.OMG
                L_NeutralBUTT.BackColor = StandardBUTT
                L_HappyBUTT.BackColor = StandardBUTT
                L_SadgeBUTT.BackColor = StandardBUTT
                L_AngyBUTT.BackColor = StandardBUTT
                L_WumpyBUTT.BackColor = StandardBUTT
                L_CringeBUTT.BackColor = StandardBUTT
                L_WowBUTT.BackColor = StandardBUTT
                L_OmgBUTT.BackColor = ActiveBUTT
                L_SparkleBUTT.BackColor = StandardBUTT
                L_DeepBreath.BackColor = StandardBUTT
                L_WibbleBUTT.BackColor = StandardBUTT
                L_PooBrain.BackColor = StandardBUTT
                L_Woohoo.BackColor = StandardBUTT
            Case = Luna.Mood.Sparkle
                L_NeutralBUTT.BackColor = StandardBUTT
                L_HappyBUTT.BackColor = StandardBUTT
                L_SadgeBUTT.BackColor = StandardBUTT
                L_AngyBUTT.BackColor = StandardBUTT
                L_WumpyBUTT.BackColor = StandardBUTT
                L_CringeBUTT.BackColor = StandardBUTT
                L_WowBUTT.BackColor = StandardBUTT
                L_OmgBUTT.BackColor = StandardBUTT
                L_SparkleBUTT.BackColor = ActiveBUTT
                L_DeepBreath.BackColor = StandardBUTT
                L_WibbleBUTT.BackColor = StandardBUTT
                L_PooBrain.BackColor = StandardBUTT
                L_Woohoo.BackColor = StandardBUTT
            Case = Luna.Mood.DeepBreath
                L_NeutralBUTT.BackColor = StandardBUTT
                L_HappyBUTT.BackColor = StandardBUTT
                L_SadgeBUTT.BackColor = StandardBUTT
                L_AngyBUTT.BackColor = StandardBUTT
                L_WumpyBUTT.BackColor = StandardBUTT
                L_CringeBUTT.BackColor = StandardBUTT
                L_WowBUTT.BackColor = StandardBUTT
                L_OmgBUTT.BackColor = StandardBUTT
                L_SparkleBUTT.BackColor = StandardBUTT
                L_DeepBreath.BackColor = ActiveBUTT
                L_WibbleBUTT.BackColor = StandardBUTT
                L_PooBrain.BackColor = StandardBUTT
                L_Woohoo.BackColor = StandardBUTT
            Case = Luna.Mood.Wibble
                L_NeutralBUTT.BackColor = StandardBUTT
                L_HappyBUTT.BackColor = StandardBUTT
                L_SadgeBUTT.BackColor = StandardBUTT
                L_AngyBUTT.BackColor = StandardBUTT
                L_WumpyBUTT.BackColor = StandardBUTT
                L_CringeBUTT.BackColor = StandardBUTT
                L_WowBUTT.BackColor = StandardBUTT
                L_OmgBUTT.BackColor = StandardBUTT
                L_SparkleBUTT.BackColor = StandardBUTT
                L_DeepBreath.BackColor = StandardBUTT
                L_WibbleBUTT.BackColor = ActiveBUTT
                L_PooBrain.BackColor = StandardBUTT
                L_Woohoo.BackColor = StandardBUTT
            Case = Luna.Mood.PooBrain
                L_NeutralBUTT.BackColor = StandardBUTT
                L_HappyBUTT.BackColor = StandardBUTT
                L_SadgeBUTT.BackColor = StandardBUTT
                L_AngyBUTT.BackColor = StandardBUTT
                L_WumpyBUTT.BackColor = StandardBUTT
                L_CringeBUTT.BackColor = StandardBUTT
                L_WowBUTT.BackColor = StandardBUTT
                L_OmgBUTT.BackColor = StandardBUTT
                L_SparkleBUTT.BackColor = StandardBUTT
                L_DeepBreath.BackColor = StandardBUTT
                L_WibbleBUTT.BackColor = StandardBUTT
                L_PooBrain.BackColor = ActiveBUTT
                L_Woohoo.BackColor = StandardBUTT
            Case = Luna.Mood.WooHoo
                L_NeutralBUTT.BackColor = StandardBUTT
                L_HappyBUTT.BackColor = StandardBUTT
                L_SadgeBUTT.BackColor = StandardBUTT
                L_AngyBUTT.BackColor = StandardBUTT
                L_WumpyBUTT.BackColor = StandardBUTT
                L_CringeBUTT.BackColor = StandardBUTT
                L_WowBUTT.BackColor = StandardBUTT
                L_OmgBUTT.BackColor = StandardBUTT
                L_SparkleBUTT.BackColor = StandardBUTT
                L_DeepBreath.BackColor = StandardBUTT
                L_WibbleBUTT.BackColor = StandardBUTT
                L_PooBrain.BackColor = StandardBUTT
                L_Woohoo.BackColor = ActiveBUTT
        End Select
        'End If
    End Sub

    Private Sub TextBox1_keypress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        If e.KeyChar = Chr(13) Then
            If TextBox1.Text <> "" Then
                Dim Speak As Task = Luna.Says(TextBox1.Text)
                TextBox1.Text = ""
            End If
            e.Handled = True
        End If
    End Sub

    Private Sub TextBox3_keypress(sender As Object, e As KeyPressEventArgs) Handles TextBox3.KeyPress
        If e.KeyChar = Chr(13) Then
            If TextBox3.Text <> "" Then
                Dim Speak As Task = Ember.Says(TextBox3.Text)
                TextBox3.Text = ""
            End If
            e.Handled = True
        End If
    End Sub

    Private Sub ENeutralBUTT_Click(sender As Object, e As EventArgs) Handles E_NeutralBUTT.Click
        Ember.ChangeMood(Ember.Mood.Neutral)
    End Sub
    Private Sub LNeutralBUTT_Click(sender As Object, e As EventArgs) Handles L_NeutralBUTT.Click
        Luna.ChangeMood(Luna.Mood.Neutral)
    End Sub

    Private Sub EhappyBUTT_Click(sender As Object, e As EventArgs) Handles E_HappyBUTT.Click
        If E_HappyBUTT.BackColor = ActiveBUTT Then
            Ember.ChangeMood(Ember.Mood.Neutral)
        Else
            Ember.ChangeMood(Ember.Mood.Happy)
        End If
    End Sub
    Private Sub LhappyBUTT_Click(sender As Object, e As EventArgs) Handles L_HappyBUTT.Click
        If L_HappyBUTT.BackColor = ActiveBUTT Then
            Luna.ChangeMood(Luna.Mood.Neutral)
        Else
            Luna.ChangeMood(Luna.Mood.Happy)
        End If
    End Sub

    Private Sub EsadgeBUTT_Click(sender As Object, e As EventArgs) Handles E_SadgeBUTT.Click
        If E_SadgeBUTT.BackColor = ActiveBUTT Then
            Ember.ChangeMood(Ember.Mood.Neutral)
        Else
            Ember.ChangeMood(Ember.Mood.Sadge)
        End If
    End Sub
    Private Sub LsadgeBUTT_Click(sender As Object, e As EventArgs) Handles L_SadgeBUTT.Click
        If L_SadgeBUTT.BackColor = ActiveBUTT Then
            Luna.ChangeMood(Luna.Mood.Neutral)
        Else
            Luna.ChangeMood(Luna.Mood.Sadge)
        End If
    End Sub

    Private Sub EangyBUTT_Click(sender As Object, e As EventArgs) Handles E_AngyBUTT.Click
        If E_AngyBUTT.BackColor = ActiveBUTT Then
            Ember.ChangeMood(Ember.Mood.Neutral)
        Else
            Ember.ChangeMood(Ember.Mood.Angy)
        End If
    End Sub
    Private Sub LangyBUTT_Click(sender As Object, e As EventArgs) Handles L_AngyBUTT.Click
        If L_AngyBUTT.BackColor = ActiveBUTT Then
            Luna.ChangeMood(Luna.Mood.Neutral)
        Else
            Luna.ChangeMood(Luna.Mood.Angy)
        End If
    End Sub

    Private Sub EwumpyBUTT_Click(sender As Object, e As EventArgs) Handles E_WumpyBUTT.Click
        If E_WumpyBUTT.BackColor = ActiveBUTT Then
            Ember.ChangeMood(Ember.Mood.Neutral)
        Else
            Ember.ChangeMood(Ember.Mood.Wumpy)
        End If
    End Sub
    Private Sub LwumpyBUTT_Click(sender As Object, e As EventArgs) Handles L_WumpyBUTT.Click
        If L_WumpyBUTT.BackColor = ActiveBUTT Then
            Luna.ChangeMood(Luna.Mood.Neutral)
        Else
            Luna.ChangeMood(Luna.Mood.Wumpy)
        End If
    End Sub

    Private Sub cringeBUTT_Click(sender As Object, e As EventArgs) Handles L_CringeBUTT.Click
        If L_CringeBUTT.BackColor = ActiveBUTT Then
            Luna.ChangeMood(Luna.Mood.Neutral)
        Else
            Luna.ChangeMood(Luna.Mood.Cringe)
        End If
    End Sub

    Private Sub L_wowBUTT_Click(sender As Object, e As EventArgs) Handles L_WowBUTT.Click
        If L_WowBUTT.BackColor = ActiveBUTT Then
            Luna.ChangeMood(Luna.Mood.Neutral)
        Else
            Luna.ChangeMood(Luna.Mood.Wow)
        End If

    End Sub

    Private Sub wtfBUTT_Click(sender As Object, e As EventArgs) Handles E_WtfBUTT.Click
        If E_WtfBUTT.BackColor = ActiveBUTT Then
            Ember.ChangeMood(Ember.Mood.Neutral)
        Else
            Ember.ChangeMood(Ember.Mood.WTF)
        End If
    End Sub

    Private Sub L_omgBUTT_Click(sender As Object, e As EventArgs) Handles L_OmgBUTT.Click
        If L_OmgBUTT.BackColor = ActiveBUTT Then
            Luna.ChangeMood(Luna.Mood.Neutral)
        Else
            Luna.ChangeMood(Luna.Mood.OMG)
        End If
    End Sub

    Private Sub L_sparkleBUTT_Click(sender As Object, e As EventArgs) Handles L_SparkleBUTT.Click
        If L_SparkleBUTT.BackColor = ActiveBUTT Then
            Luna.ChangeMood(Luna.Mood.Neutral)
        Else
            Luna.ChangeMood(Luna.Mood.Sparkle)
        End If
    End Sub

    Private Sub L_DEEPBREATH_Click(sender As Object, e As EventArgs) Handles L_DeepBreath.Click
        Luna.ChangeMood(Luna.Mood.DeepBreath, 4200)
    End Sub

    Private Sub L_WIBBLEBUTT_Click(sender As Object, e As EventArgs) Handles L_WibbleBUTT.Click
        If L_WibbleBUTT.BackColor = ActiveBUTT Then
            Luna.ChangeMood(Luna.Mood.Neutral)
        Else
            Luna.ChangeMood(Luna.Mood.Wibble)
        End If
    End Sub

    Private Sub E_lefthBUTT_Click(sender As Object, e As EventArgs) Handles E_LefthBUTT.Click
        Dim MoodString As String = Ember.MyMood
        If E_LefthBUTT.BackColor = ActiveBUTT Then
            Ember.LeftHand = False
            Ember.ChangeMood(MoodString)
        Else
            Ember.LeftHand = True
            Ember.ChangeMood(MoodString)
        End If
    End Sub

    Private Sub righthBUTT_Click(sender As Object, e As EventArgs) Handles E_RighthBUTT.Click
        Dim MoodString As String = Ember.MyMood
        If E_RighthBUTT.BackColor = ActiveBUTT Then
            Ember.RightHand = False
            Ember.ChangeMood(MoodString)
        Else
            Ember.RightHand = True
            Ember.ChangeMood(MoodString)
        End If
    End Sub

    Private Sub E_RocKnStonE_Click(sender As Object, e As EventArgs) Handles E_RocKnStonE.Click
        'Ember.ChangeMood(Ember.Mood.RockandStone, 2800)
        'SendMessage(AudioControl.GetSoundFileDataByName("Rock And Stone"))
        Dim Speak As Task = Ember.Says("ROCK AND STONE", Ember.Mood.RockandStone, "Rock And Stone",, 2800)
    End Sub

    Private Sub OBScharacters_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        'SendMessage(Ember.MyMood)
        If Ember.MyMood = "" Then
            Ember.ChangeMood(Ember.Mood.Neutral)
        End If
        'SendMessage(Luna.MyMood)
        If Luna.MyMood = "" Then
            Luna.ChangeMood(Luna.Mood.Neutral)
        End If
    End Sub

    Private Sub L_PooBrain_Click(sender As Object, e As EventArgs) Handles L_PooBrain.Click
        Luna.ChangeMood(Luna.Mood.PooBrain, 4050,, "Luna Poo Brain")
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox3.TextChanged

    End Sub

    Private Sub E_Woohoo_Click(sender As Object, e As EventArgs) Handles E_Woohoo.Click
        Ember.ChangeMood(Ember.Mood.WooHoo, 2800)
    End Sub

    Private Sub L_Woohoo_Click(sender As Object, e As EventArgs) Handles L_Woohoo.Click
        Luna.ChangeMood(Luna.Mood.WooHoo, 2250)
    End Sub
End Class