Imports systen.threading
Public Class OBScharacters

    Public CharacterBool As Boolean

    Private Sub EmberAway_Click(sender As Object, e As EventArgs) Handles CharacterSwitch.Click
        If CharacterBool = SpriteID.Ember Then
            CharacterBool = SpriteID.Luna
        Else
            CharacterBool = SpriteID.Ember
        End If
        Call UpdateDisplay()
    End Sub

    Private Sub OBScharacters_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SourceWindow.CHARACTERS.BackColor = ActiveBUTT
        CharacterBool = SpriteID.Ember
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
        If CharacterBool = SpriteID.Ember Then
            CharacterSwitch.Text = "EMBER"
            CharacterSprite.BackgroundImage = My.Resources.ember

            Select Case Ember.CurrentMood
                Case = Ember.Directory & Ember.Mood.Neutral
                    NeutralBUTT.BackColor = ActiveBUTT
                    HappyBUTT.BackColor = StandardBUTT
                    SadgeBUTT.BackColor = StandardBUTT
                    AngyBUTT.BackColor = StandardBUTT
                    WumpyBUTT.BackColor = StandardBUTT
                    CringeBUTT.BackColor = StandardBUTT
                    WowBUTT.BackColor = StandardBUTT
                    WtfBUTT.BackColor = StandardBUTT
                    OmgBUTT.BackColor = StandardBUTT
                    SparkleBUTT.BackColor = StandardBUTT
                    HandsBUTT.BackColor = StandardBUTT
                    LefthBUTT.BackColor = StandardBUTT
                    RighthBUTT.BackColor = StandardBUTT
                Case = Ember.Directory & Ember.Mood.Happy
                    NeutralBUTT.BackColor = StandardBUTT
                    HappyBUTT.BackColor = ActiveBUTT
                    SadgeBUTT.BackColor = StandardBUTT
                    AngyBUTT.BackColor = StandardBUTT
                    WumpyBUTT.BackColor = StandardBUTT
                    CringeBUTT.BackColor = StandardBUTT
                    WowBUTT.BackColor = StandardBUTT
                    WtfBUTT.BackColor = StandardBUTT
                    OmgBUTT.BackColor = StandardBUTT
                    SparkleBUTT.BackColor = StandardBUTT
                    HandsBUTT.BackColor = StandardBUTT
                    LefthBUTT.BackColor = StandardBUTT
                    RighthBUTT.BackColor = StandardBUTT
                Case = Ember.Directory & Ember.Mood.Sadge
                    NeutralBUTT.BackColor = StandardBUTT
                    HappyBUTT.BackColor = StandardBUTT
                    SadgeBUTT.BackColor = ActiveBUTT
                    AngyBUTT.BackColor = StandardBUTT
                    WumpyBUTT.BackColor = StandardBUTT
                    CringeBUTT.BackColor = StandardBUTT
                    WowBUTT.BackColor = StandardBUTT
                    WtfBUTT.BackColor = StandardBUTT
                    OmgBUTT.BackColor = StandardBUTT
                    SparkleBUTT.BackColor = StandardBUTT
                    HandsBUTT.BackColor = StandardBUTT
                    LefthBUTT.BackColor = StandardBUTT
                    RighthBUTT.BackColor = StandardBUTT
                Case = Ember.Directory & Ember.Mood.Angy
                    NeutralBUTT.BackColor = StandardBUTT
                    HappyBUTT.BackColor = StandardBUTT
                    SadgeBUTT.BackColor = StandardBUTT
                    AngyBUTT.BackColor = ActiveBUTT
                    WumpyBUTT.BackColor = StandardBUTT
                    CringeBUTT.BackColor = StandardBUTT
                    WowBUTT.BackColor = StandardBUTT
                    WtfBUTT.BackColor = StandardBUTT
                    OmgBUTT.BackColor = StandardBUTT
                    SparkleBUTT.BackColor = StandardBUTT
                    HandsBUTT.BackColor = StandardBUTT
                    LefthBUTT.BackColor = StandardBUTT
                    RighthBUTT.BackColor = StandardBUTT
                Case = Ember.Directory & Ember.Mood.Wumpy
                    NeutralBUTT.BackColor = StandardBUTT
                    HappyBUTT.BackColor = StandardBUTT
                    SadgeBUTT.BackColor = StandardBUTT
                    AngyBUTT.BackColor = StandardBUTT
                    WumpyBUTT.BackColor = ActiveBUTT
                    CringeBUTT.BackColor = StandardBUTT
                    WowBUTT.BackColor = StandardBUTT
                    WtfBUTT.BackColor = StandardBUTT
                    OmgBUTT.BackColor = StandardBUTT
                    SparkleBUTT.BackColor = StandardBUTT
                    HandsBUTT.BackColor = StandardBUTT
                    LefthBUTT.BackColor = StandardBUTT
                    RighthBUTT.BackColor = StandardBUTT
                Case = Ember.Directory & Ember.Mood.Cringe
                    NeutralBUTT.BackColor = StandardBUTT
                    HappyBUTT.BackColor = StandardBUTT
                    SadgeBUTT.BackColor = StandardBUTT
                    AngyBUTT.BackColor = StandardBUTT
                    WumpyBUTT.BackColor = StandardBUTT
                    CringeBUTT.BackColor = ActiveBUTT
                    WowBUTT.BackColor = StandardBUTT
                    WtfBUTT.BackColor = StandardBUTT
                    OmgBUTT.BackColor = StandardBUTT
                    SparkleBUTT.BackColor = StandardBUTT
                    HandsBUTT.BackColor = StandardBUTT
                    LefthBUTT.BackColor = StandardBUTT
                    RighthBUTT.BackColor = StandardBUTT
                Case = Ember.Directory & Ember.Mood.Wow
                    NeutralBUTT.BackColor = StandardBUTT
                    HappyBUTT.BackColor = StandardBUTT
                    SadgeBUTT.BackColor = StandardBUTT
                    AngyBUTT.BackColor = StandardBUTT
                    WumpyBUTT.BackColor = StandardBUTT
                    CringeBUTT.BackColor = StandardBUTT
                    WowBUTT.BackColor = ActiveBUTT
                    WtfBUTT.BackColor = StandardBUTT
                    OmgBUTT.BackColor = StandardBUTT
                    SparkleBUTT.BackColor = StandardBUTT
                    HandsBUTT.BackColor = StandardBUTT
                    LefthBUTT.BackColor = StandardBUTT
                    RighthBUTT.BackColor = StandardBUTT
                Case = Ember.Directory & Ember.Mood.WTF
                    NeutralBUTT.BackColor = StandardBUTT
                    HappyBUTT.BackColor = StandardBUTT
                    SadgeBUTT.BackColor = StandardBUTT
                    AngyBUTT.BackColor = StandardBUTT
                    WumpyBUTT.BackColor = StandardBUTT
                    CringeBUTT.BackColor = StandardBUTT
                    WowBUTT.BackColor = StandardBUTT
                    WtfBUTT.BackColor = ActiveBUTT
                    OmgBUTT.BackColor = StandardBUTT
                    SparkleBUTT.BackColor = StandardBUTT
                    HandsBUTT.BackColor = StandardBUTT
                    LefthBUTT.BackColor = StandardBUTT
                    RighthBUTT.BackColor = StandardBUTT
                Case = Ember.Directory & Ember.Mood.OMG
                    NeutralBUTT.BackColor = StandardBUTT
                    HappyBUTT.BackColor = StandardBUTT
                    SadgeBUTT.BackColor = StandardBUTT
                    AngyBUTT.BackColor = StandardBUTT
                    WumpyBUTT.BackColor = StandardBUTT
                    CringeBUTT.BackColor = StandardBUTT
                    WowBUTT.BackColor = StandardBUTT
                    WtfBUTT.BackColor = StandardBUTT
                    OmgBUTT.BackColor = ActiveBUTT
                    SparkleBUTT.BackColor = StandardBUTT
                    HandsBUTT.BackColor = StandardBUTT
                    LefthBUTT.BackColor = StandardBUTT
                    RighthBUTT.BackColor = StandardBUTT
                Case = Ember.Directory & Ember.Mood.Sparkle
                    NeutralBUTT.BackColor = StandardBUTT
                    HappyBUTT.BackColor = StandardBUTT
                    SadgeBUTT.BackColor = StandardBUTT
                    AngyBUTT.BackColor = StandardBUTT
                    WumpyBUTT.BackColor = StandardBUTT
                    CringeBUTT.BackColor = StandardBUTT
                    WowBUTT.BackColor = StandardBUTT
                    WtfBUTT.BackColor = StandardBUTT
                    OmgBUTT.BackColor = StandardBUTT
                    SparkleBUTT.BackColor = ActiveBUTT
                    HandsBUTT.BackColor = StandardBUTT
                    LefthBUTT.BackColor = StandardBUTT
                    RighthBUTT.BackColor = StandardBUTT
                Case = Ember.Directory & Ember.Mood.Hands
                    NeutralBUTT.BackColor = StandardBUTT
                    HappyBUTT.BackColor = StandardBUTT
                    SadgeBUTT.BackColor = StandardBUTT
                    AngyBUTT.BackColor = StandardBUTT
                    WumpyBUTT.BackColor = StandardBUTT
                    CringeBUTT.BackColor = StandardBUTT
                    WowBUTT.BackColor = StandardBUTT
                    WtfBUTT.BackColor = StandardBUTT
                    OmgBUTT.BackColor = StandardBUTT
                    SparkleBUTT.BackColor = StandardBUTT
                    HandsBUTT.BackColor = ActiveBUTT
                    LefthBUTT.BackColor = StandardBUTT
                    RighthBUTT.BackColor = StandardBUTT
                Case = Ember.Directory & Ember.Mood.LeftH
                    NeutralBUTT.BackColor = StandardBUTT
                    HappyBUTT.BackColor = StandardBUTT
                    SadgeBUTT.BackColor = StandardBUTT
                    AngyBUTT.BackColor = StandardBUTT
                    WumpyBUTT.BackColor = StandardBUTT
                    CringeBUTT.BackColor = StandardBUTT
                    WowBUTT.BackColor = StandardBUTT
                    WtfBUTT.BackColor = StandardBUTT
                    OmgBUTT.BackColor = StandardBUTT
                    SparkleBUTT.BackColor = StandardBUTT
                    HandsBUTT.BackColor = StandardBUTT
                    LefthBUTT.BackColor = ActiveBUTT
                    RighthBUTT.BackColor = StandardBUTT
                Case = Ember.Directory & Ember.Mood.RightH
                    NeutralBUTT.BackColor = StandardBUTT
                    HappyBUTT.BackColor = StandardBUTT
                    SadgeBUTT.BackColor = StandardBUTT
                    AngyBUTT.BackColor = StandardBUTT
                    WumpyBUTT.BackColor = StandardBUTT
                    CringeBUTT.BackColor = StandardBUTT
                    WowBUTT.BackColor = StandardBUTT
                    WtfBUTT.BackColor = StandardBUTT
                    OmgBUTT.BackColor = StandardBUTT
                    SparkleBUTT.BackColor = StandardBUTT
                    HandsBUTT.BackColor = StandardBUTT
                    LefthBUTT.BackColor = StandardBUTT
                    RighthBUTT.BackColor = ActiveBUTT
            End Select
        Else
            CharacterSwitch.Text = "LUNA"
            CharacterSprite.BackgroundImage = My.Resources.luna1

            Select Case Luna.CurrentMood
                Case = Luna.Directory & Luna.Mood.Neutral
                    NeutralBUTT.BackColor = ActiveBUTT
                    HappyBUTT.BackColor = StandardBUTT
                    SadgeBUTT.BackColor = StandardBUTT
                    AngyBUTT.BackColor = StandardBUTT
                    WumpyBUTT.BackColor = StandardBUTT
                    CringeBUTT.BackColor = StandardBUTT
                    WowBUTT.BackColor = StandardBUTT
                    WtfBUTT.BackColor = StandardBUTT
                    OmgBUTT.BackColor = StandardBUTT
                    SparkleBUTT.BackColor = StandardBUTT
                    HandsBUTT.BackColor = StandardBUTT
                    LefthBUTT.BackColor = StandardBUTT
                    RighthBUTT.BackColor = StandardBUTT
                Case = Luna.Directory & Luna.Mood.Happy
                    NeutralBUTT.BackColor = StandardBUTT
                    HappyBUTT.BackColor = ActiveBUTT
                    SadgeBUTT.BackColor = StandardBUTT
                    AngyBUTT.BackColor = StandardBUTT
                    WumpyBUTT.BackColor = StandardBUTT
                    CringeBUTT.BackColor = StandardBUTT
                    WowBUTT.BackColor = StandardBUTT
                    WtfBUTT.BackColor = StandardBUTT
                    OmgBUTT.BackColor = StandardBUTT
                    SparkleBUTT.BackColor = StandardBUTT
                    HandsBUTT.BackColor = StandardBUTT
                    LefthBUTT.BackColor = StandardBUTT
                    RighthBUTT.BackColor = StandardBUTT
                Case = Luna.Directory & Luna.Mood.Sadge
                    NeutralBUTT.BackColor = StandardBUTT
                    HappyBUTT.BackColor = StandardBUTT
                    SadgeBUTT.BackColor = ActiveBUTT
                    AngyBUTT.BackColor = StandardBUTT
                    WumpyBUTT.BackColor = StandardBUTT
                    CringeBUTT.BackColor = StandardBUTT
                    WowBUTT.BackColor = StandardBUTT
                    WtfBUTT.BackColor = StandardBUTT
                    OmgBUTT.BackColor = StandardBUTT
                    SparkleBUTT.BackColor = StandardBUTT
                    HandsBUTT.BackColor = StandardBUTT
                    LefthBUTT.BackColor = StandardBUTT
                    RighthBUTT.BackColor = StandardBUTT
                Case = Luna.Directory & Luna.Mood.Angy
                    NeutralBUTT.BackColor = StandardBUTT
                    HappyBUTT.BackColor = StandardBUTT
                    SadgeBUTT.BackColor = StandardBUTT
                    AngyBUTT.BackColor = ActiveBUTT
                    WumpyBUTT.BackColor = StandardBUTT
                    CringeBUTT.BackColor = StandardBUTT
                    WowBUTT.BackColor = StandardBUTT
                    WtfBUTT.BackColor = StandardBUTT
                    OmgBUTT.BackColor = StandardBUTT
                    SparkleBUTT.BackColor = StandardBUTT
                    HandsBUTT.BackColor = StandardBUTT
                    LefthBUTT.BackColor = StandardBUTT
                    RighthBUTT.BackColor = StandardBUTT
                Case = Luna.Directory & Luna.Mood.Wumpy
                    NeutralBUTT.BackColor = StandardBUTT
                    HappyBUTT.BackColor = StandardBUTT
                    SadgeBUTT.BackColor = StandardBUTT
                    AngyBUTT.BackColor = StandardBUTT
                    WumpyBUTT.BackColor = ActiveBUTT
                    CringeBUTT.BackColor = StandardBUTT
                    WowBUTT.BackColor = StandardBUTT
                    WtfBUTT.BackColor = StandardBUTT
                    OmgBUTT.BackColor = StandardBUTT
                    SparkleBUTT.BackColor = StandardBUTT
                    HandsBUTT.BackColor = StandardBUTT
                    LefthBUTT.BackColor = StandardBUTT
                    RighthBUTT.BackColor = StandardBUTT
                Case = Luna.Directory & Luna.Mood.Cringe
                    NeutralBUTT.BackColor = StandardBUTT
                    HappyBUTT.BackColor = StandardBUTT
                    SadgeBUTT.BackColor = StandardBUTT
                    AngyBUTT.BackColor = StandardBUTT
                    WumpyBUTT.BackColor = StandardBUTT
                    CringeBUTT.BackColor = ActiveBUTT
                    WowBUTT.BackColor = StandardBUTT
                    WtfBUTT.BackColor = StandardBUTT
                    OmgBUTT.BackColor = StandardBUTT
                    SparkleBUTT.BackColor = StandardBUTT
                    HandsBUTT.BackColor = StandardBUTT
                    LefthBUTT.BackColor = StandardBUTT
                    RighthBUTT.BackColor = StandardBUTT
                Case = Luna.Directory & Luna.Mood.Wow
                    NeutralBUTT.BackColor = StandardBUTT
                    HappyBUTT.BackColor = StandardBUTT
                    SadgeBUTT.BackColor = StandardBUTT
                    AngyBUTT.BackColor = StandardBUTT
                    WumpyBUTT.BackColor = StandardBUTT
                    CringeBUTT.BackColor = StandardBUTT
                    WowBUTT.BackColor = ActiveBUTT
                    WtfBUTT.BackColor = StandardBUTT
                    OmgBUTT.BackColor = StandardBUTT
                    SparkleBUTT.BackColor = StandardBUTT
                    HandsBUTT.BackColor = StandardBUTT
                    LefthBUTT.BackColor = StandardBUTT
                    RighthBUTT.BackColor = StandardBUTT
                Case = Luna.Directory & Luna.Mood.WTF
                    NeutralBUTT.BackColor = StandardBUTT
                    HappyBUTT.BackColor = StandardBUTT
                    SadgeBUTT.BackColor = StandardBUTT
                    AngyBUTT.BackColor = StandardBUTT
                    WumpyBUTT.BackColor = StandardBUTT
                    CringeBUTT.BackColor = StandardBUTT
                    WowBUTT.BackColor = StandardBUTT
                    WtfBUTT.BackColor = ActiveBUTT
                    OmgBUTT.BackColor = StandardBUTT
                    SparkleBUTT.BackColor = StandardBUTT
                    HandsBUTT.BackColor = StandardBUTT
                    LefthBUTT.BackColor = StandardBUTT
                    RighthBUTT.BackColor = StandardBUTT
                Case = Luna.Directory & Luna.Mood.OMG
                    NeutralBUTT.BackColor = StandardBUTT
                    HappyBUTT.BackColor = StandardBUTT
                    SadgeBUTT.BackColor = StandardBUTT
                    AngyBUTT.BackColor = StandardBUTT
                    WumpyBUTT.BackColor = StandardBUTT
                    CringeBUTT.BackColor = StandardBUTT
                    WowBUTT.BackColor = StandardBUTT
                    WtfBUTT.BackColor = StandardBUTT
                    OmgBUTT.BackColor = ActiveBUTT
                    SparkleBUTT.BackColor = StandardBUTT
                    HandsBUTT.BackColor = StandardBUTT
                    LefthBUTT.BackColor = StandardBUTT
                    RighthBUTT.BackColor = StandardBUTT
                Case = Luna.Directory & Luna.Mood.Sparkle
                    NeutralBUTT.BackColor = StandardBUTT
                    HappyBUTT.BackColor = StandardBUTT
                    SadgeBUTT.BackColor = StandardBUTT
                    AngyBUTT.BackColor = StandardBUTT
                    WumpyBUTT.BackColor = StandardBUTT
                    CringeBUTT.BackColor = StandardBUTT
                    WowBUTT.BackColor = StandardBUTT
                    WtfBUTT.BackColor = StandardBUTT
                    OmgBUTT.BackColor = StandardBUTT
                    SparkleBUTT.BackColor = ActiveBUTT
                    HandsBUTT.BackColor = StandardBUTT
                    LefthBUTT.BackColor = StandardBUTT
                    RighthBUTT.BackColor = StandardBUTT
                Case = Luna.Directory & Luna.Mood.Hands
                    NeutralBUTT.BackColor = StandardBUTT
                    HappyBUTT.BackColor = StandardBUTT
                    SadgeBUTT.BackColor = StandardBUTT
                    AngyBUTT.BackColor = StandardBUTT
                    WumpyBUTT.BackColor = StandardBUTT
                    CringeBUTT.BackColor = StandardBUTT
                    WowBUTT.BackColor = StandardBUTT
                    WtfBUTT.BackColor = StandardBUTT
                    OmgBUTT.BackColor = StandardBUTT
                    SparkleBUTT.BackColor = StandardBUTT
                    HandsBUTT.BackColor = ActiveBUTT
                    LefthBUTT.BackColor = StandardBUTT
                    RighthBUTT.BackColor = StandardBUTT
                Case = Luna.Directory & Luna.Mood.LeftH
                    NeutralBUTT.BackColor = StandardBUTT
                    HappyBUTT.BackColor = StandardBUTT
                    SadgeBUTT.BackColor = StandardBUTT
                    AngyBUTT.BackColor = StandardBUTT
                    WumpyBUTT.BackColor = StandardBUTT
                    CringeBUTT.BackColor = StandardBUTT
                    WowBUTT.BackColor = StandardBUTT
                    WtfBUTT.BackColor = StandardBUTT
                    OmgBUTT.BackColor = StandardBUTT
                    SparkleBUTT.BackColor = StandardBUTT
                    HandsBUTT.BackColor = StandardBUTT
                    LefthBUTT.BackColor = ActiveBUTT
                    RighthBUTT.BackColor = StandardBUTT
                Case = Luna.Directory & Luna.Mood.RightH
                    NeutralBUTT.BackColor = StandardBUTT
                    HappyBUTT.BackColor = StandardBUTT
                    SadgeBUTT.BackColor = StandardBUTT
                    AngyBUTT.BackColor = StandardBUTT
                    WumpyBUTT.BackColor = StandardBUTT
                    CringeBUTT.BackColor = StandardBUTT
                    WowBUTT.BackColor = StandardBUTT
                    WtfBUTT.BackColor = StandardBUTT
                    OmgBUTT.BackColor = StandardBUTT
                    SparkleBUTT.BackColor = StandardBUTT
                    HandsBUTT.BackColor = StandardBUTT
                    LefthBUTT.BackColor = StandardBUTT
                    RighthBUTT.BackColor = ActiveBUTT
            End Select
        End If
    End Sub

    Private Sub TextBox3_keypress(sender As Object, e As KeyPressEventArgs) Handles TextBox3.KeyPress
        If e.KeyChar = Chr(13) Then
            If TextBox3.Text <> "" Then
                If CharacterBool = SpriteID.Ember Then
                    Ember.Says(TextBox3.Text)
                Else
                    Luna.Says(TextBox3.Text)
                End If
                TextBox3.Text = ""
            End If
            e.Handled = True
        End If
    End Sub

    Private Sub NeutralBUTT_Click(sender As Object, e As EventArgs) Handles NeutralBUTT.Click
        If CharacterBool = SpriteID.Ember Then
            Ember.ChangeMood(Ember.Mood.Neutral)
        Else
            Luna.ChangeMood(Luna.Mood.Neutral)
        End If
    End Sub

    Private Sub happyBUTT_Click(sender As Object, e As EventArgs) Handles HappyBUTT.Click
        If HappyBUTT.BackColor = ActiveBUTT Then
            If CharacterBool = SpriteID.Ember Then
                Ember.ChangeMood(Ember.Mood.Neutral)
            Else
                Luna.ChangeMood(Luna.Mood.Neutral)
            End If
        Else
            If CharacterBool = SpriteID.Ember Then
                Ember.ChangeMood(Ember.Mood.Happy)
            Else
                Luna.ChangeMood(Luna.Mood.Happy)
            End If
        End If
    End Sub

    Private Sub sadgeBUTT_Click(sender As Object, e As EventArgs) Handles SadgeBUTT.Click
        If SadgeBUTT.BackColor = ActiveBUTT Then
            If CharacterBool = SpriteID.Ember Then
                Ember.ChangeMood(Ember.Mood.Neutral)
            Else
                Luna.ChangeMood(Luna.Mood.Neutral)
            End If
        Else
            If CharacterBool = SpriteID.Ember Then
                Ember.ChangeMood(Ember.Mood.Sadge)
            Else
                Luna.ChangeMood(Luna.Mood.Sadge)
            End If
        End If

    End Sub

    Private Sub angyBUTT_Click(sender As Object, e As EventArgs) Handles AngyBUTT.Click
        If AngyBUTT.BackColor = ActiveBUTT Then
            If CharacterBool = SpriteID.Ember Then
                Ember.ChangeMood(Ember.Mood.Neutral)
            Else
                Luna.ChangeMood(Luna.Mood.Neutral)
            End If
        Else
            If CharacterBool = SpriteID.Ember Then
                Ember.ChangeMood(Ember.Mood.Angy)
            Else
                Luna.ChangeMood(Luna.Mood.Angy)
            End If
        End If

    End Sub

    Private Sub wumpyBUTT_Click(sender As Object, e As EventArgs) Handles WumpyBUTT.Click
        If WumpyBUTT.BackColor = ActiveBUTT Then
            If CharacterBool = SpriteID.Ember Then
                Ember.ChangeMood(Ember.Mood.Neutral)
            Else
                Luna.ChangeMood(Luna.Mood.Neutral)
            End If
        Else
            If CharacterBool = SpriteID.Ember Then
                Ember.ChangeMood(Ember.Mood.Wumpy)
            Else
                Luna.ChangeMood(Luna.Mood.Wumpy)
            End If
        End If

    End Sub

    Private Sub cringeBUTT_Click(sender As Object, e As EventArgs) Handles CringeBUTT.Click
        If CringeBUTT.BackColor = ActiveBUTT Then
            If CharacterBool = SpriteID.Ember Then
                Ember.ChangeMood(Ember.Mood.Neutral)
            Else
                Luna.ChangeMood(Luna.Mood.Neutral)
            End If
        Else
            If CharacterBool = SpriteID.Ember Then
                Ember.ChangeMood(Ember.Mood.Cringe)
            Else
                Luna.ChangeMood(Luna.Mood.Cringe)
            End If
        End If

    End Sub

    Private Sub wowBUTT_Click(sender As Object, e As EventArgs) Handles WowBUTT.Click
        If WowBUTT.BackColor = ActiveBUTT Then
            If CharacterBool = SpriteID.Ember Then
                Ember.ChangeMood(Ember.Mood.Neutral)
            Else
                Luna.ChangeMood(Luna.Mood.Neutral)
            End If
        Else
            If CharacterBool = SpriteID.Ember Then
                Ember.ChangeMood(Ember.Mood.Wow)
            Else
                Luna.ChangeMood(Luna.Mood.Wow)
            End If
        End If

    End Sub

    Private Sub wtfBUTT_Click(sender As Object, e As EventArgs) Handles WtfBUTT.Click
        If WtfBUTT.BackColor = ActiveBUTT Then
            If CharacterBool = SpriteID.Ember Then
                Ember.ChangeMood(Ember.Mood.Neutral)
            Else
                Luna.ChangeMood(Luna.Mood.Neutral)
            End If
        Else
            If CharacterBool = SpriteID.Ember Then
                Ember.ChangeMood(Ember.Mood.WTF)
            Else
                Luna.ChangeMood(Luna.Mood.WTF)
            End If
        End If

    End Sub

    Private Sub omgBUTT_Click(sender As Object, e As EventArgs) Handles OmgBUTT.Click
        If OmgBUTT.BackColor = ActiveBUTT Then
            If CharacterBool = SpriteID.Ember Then
                Ember.ChangeMood(Ember.Mood.Neutral)
            Else
                Luna.ChangeMood(Luna.Mood.Neutral)
            End If
        Else
            If CharacterBool = SpriteID.Ember Then
                Ember.ChangeMood(Ember.Mood.OMG)
            Else
                Luna.ChangeMood(Luna.Mood.OMG)
            End If
        End If

    End Sub

    Private Sub sparkleBUTT_Click(sender As Object, e As EventArgs) Handles SparkleBUTT.Click
        If SparkleBUTT.BackColor = ActiveBUTT Then
            If CharacterBool = SpriteID.Ember Then
                Ember.ChangeMood(Ember.Mood.Neutral)
            Else
                Luna.ChangeMood(Luna.Mood.Neutral)
            End If
        Else
            If CharacterBool = SpriteID.Ember Then
                Ember.ChangeMood(Ember.Mood.Sparkle)
            Else
                Luna.ChangeMood(Luna.Mood.Sparkle)
            End If
        End If
    End Sub

    Private Sub handsBUTT_Click(sender As Object, e As EventArgs) Handles HandsBUTT.Click
        If HandsBUTT.BackColor = ActiveBUTT Then
            If CharacterBool = SpriteID.Ember Then
                Ember.ChangeMood(Ember.Mood.Neutral)
            Else
                Luna.ChangeMood(Luna.Mood.Neutral)
            End If
        Else
            If CharacterBool = SpriteID.Ember Then
                Ember.ChangeMood(Ember.Mood.Hands)
            Else
                Luna.ChangeMood(Luna.Mood.Hands)
            End If
        End If

    End Sub

    Private Sub lefthBUTT_Click(sender As Object, e As EventArgs) Handles LefthBUTT.Click
        If LefthBUTT.BackColor = ActiveBUTT Then
            If CharacterBool = SpriteID.Ember Then
                Ember.ChangeMood(Ember.Mood.Neutral)
            Else
                Luna.ChangeMood(Luna.Mood.Neutral)
            End If
        Else
            If CharacterBool = SpriteID.Ember Then
                Ember.ChangeMood(Ember.Mood.LeftH)
            Else
                Luna.ChangeMood(Luna.Mood.LeftH)
            End If
        End If
    End Sub

    Private Sub righthBUTT_Click(sender As Object, e As EventArgs) Handles RighthBUTT.Click
        If RighthBUTT.BackColor = ActiveBUTT Then
            If CharacterBool = SpriteID.Ember Then
                Ember.ChangeMood(Ember.Mood.Neutral)
            Else
                Luna.ChangeMood(Luna.Mood.Neutral)
            End If
        Else
            If CharacterBool = SpriteID.Ember Then
                Ember.ChangeMood(Ember.Mood.RightH)
            Else
                Luna.ChangeMood(Luna.Mood.RightH)
            End If
        End If
    End Sub
End Class