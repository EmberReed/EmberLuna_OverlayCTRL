Imports System.IO

Module Constants

    'DO NOT SHOW

    Public BotName As String
    Public Broadcastername As String
    Public twitchOAuth As String
    Public JHclientID As String
    Public JHOauth As String
    Public JHID As String
    Public JH_AToken As String
    Public JHsecret As String
    Public JH_FullAToken As String
    Public JH_FullARefresh As String
    Public OBSsocketString As String
    Public OBSsocketPassword As String

    Public ProgramSettingsFile As String = "\\StreamPC-V2\OBS Assets\Auth\Program Settings.txt"
    Public Sub ReadProgramSettings(FileString)

        If File.Exists(FileString) = True Then
            Dim Reader As StreamReader = File.OpenText(FileString)
            Dim InputString As String()
            Do Until Reader.EndOfStream = True
                InputString = Split(Reader.ReadLine(), "<>")
                Select Case InputString(0)
                    Case = "BotName"
                        BotName = InputString(1)
                    Case = "Broadcastername"
                        Broadcastername = InputString(1)
                    Case = "twitchOAuth"
                        twitchOAuth = InputString(1)
                    Case = "JHclientID"
                        JHclientID = InputString(1)
                    Case = "JHOauth"
                        JHOauth = InputString(1)
                    Case = "JHID"
                        JHID = InputString(1)
                    Case = "JH_AToken"
                        JH_AToken = InputString(1)
                    Case = "JHsecret"
                        JHsecret = InputString(1)
                    Case = "JH_FullAToken"
                        JH_FullAToken = InputString(1)
                    Case = "JH_FullARefresh"
                        JH_FullARefresh = InputString(1)
                    Case = "OBSsocketString"
                        OBSsocketString = InputString(1)
                    Case = "OBSsocketPassword"
                        OBSsocketPassword = InputString(1)
                End Select
            Loop
            Reader.Close()
            Reader.Dispose()
        End If

    End Sub

End Module
