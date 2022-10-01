Imports TwitchLib.Api
Imports System.IO
Imports System.Threading
Imports System.Collections.Concurrent
Imports System.Net
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net.Sockets
Imports TwitchLib.PubSub

Public Module TwitchFunctions

    '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    '////////////////////////////////////////////////////API FUNCTIONS////////////////////////////////////////////////////////
    '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    Public Class APIfunctions
        Private Authorized As Boolean
        Public AccessToken As String
        Private RefreshToken As String
        Private AuthDirectory As String = "\\StreamPC-V2\OBS Assets\Auth"
        Private CodeString As String
        Private ScopesList As List(Of Core.Enums.AuthScopes)
        Private WithEvents MyAPI As TwitchAPI
        Private WithEvents PubSub As TwitchPubSub
        'Private PSUBstate As Boolean
        Public Event TokenAcquired(TokenMessage As String)
        'Public Event AuthorizationInitialized()
        Public Event StreamInfoAvailable(StreamInfoString)
        Public Event StreamInfoUpdated()

        Private AuthorizationRecieved As TaskCompletionSource(Of Boolean)
        Private WaitingForAuthorization As Boolean = False

        Private AutoRefreshTask As Task

        Public Sub New()
            AddHandler TokenAcquired, AddressOf AuthorizationAcquired
            Authorized = False
            APIstate = False
            MyAPI = New TwitchAPI()
            'PubSub = New TwitchPubSub
            MyAPI.Settings.ClientId = JHclientID
            MyAPI.Settings.Secret = JHsecret
            ScopesList = New List(Of Core.Enums.AuthScopes)
            ScopesList.Add(Core.Enums.AuthScopes.Helix_Channel_Read_Redemptions)
            ScopesList.Add(Core.Enums.AuthScopes.Helix_Channel_Manage_Redemptions)
            ScopesList.Add(Core.Enums.AuthScopes.Helix_Bits_Read)
            ScopesList.Add(Core.Enums.AuthScopes.Helix_Channel_Manage_Broadcast)
            ScopesList.Add(Core.Enums.AuthScopes.Helix_Channel_Read_Subscriptions)
            MyAPI.Settings.Scopes = ScopesList
        End Sub



        '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        '/////////////////////////////////////////////////PUBSUB FUNCTIONS////////////////////////////////////////////////////////
        '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        Private AwaitingPSUBconnect As Boolean = False
        Private PSUBconnected As TaskCompletionSource(Of Boolean)

        Public Async Function ConnectPubSub() As Task
            If PubSub Is Nothing Then
                PubSub = New TwitchPubSub
                AwaitingPSUBconnect = True
                PSUBconnected = New TaskCompletionSource(Of Boolean)
                PubSub.ListenToChannelPoints(JHID)
                PubSub.ListenToVideoPlayback(JHID)
                PubSub.ListenToBitsEventsV2(JHID)
                PubSub.ListenToFollows(JHID)
                PubSub.ListenToRaid(JHID)
                PubSub.ListenToSubscriptions(JHID)
                PubSub.Connect()
                AwaitingPSUBconnect = Await PSUBconnected.Task
                PubSub.SendTopics(AccessToken)
            End If
            If WaitingForAuthorization Then AuthorizationRecieved.SetResult(False)
            Await GetChannelPointData()
            Await ChannelPoints.UpdateProgramRewards(True)
            SourceWindow.CHPstateChanged(True)
            SourceWindow.IMREADY()
        End Function

        Private Sub PubSubConnected(sender As Object, e As EventArgs) Handles PubSub.OnPubSubServiceConnected
            If AwaitingPSUBconnect Then PSUBconnected.SetResult(False)
            SourceWindow.PubSubStateChanged(True)
            'Else
            'SendMessage("WHAT THE FUCK?!?")
            'PubSub.Disconnect()
            'End If
        End Sub

        Public Sub PubSubDisconnect(sender As Object, e As EventArgs) Handles PubSub.OnPubSubServiceClosed
            PSUBdisconnect()
        End Sub

        Private Sub PSUBdisconnect()
            'PSUBstate = False
            PubSub = Nothing
            If AwaitingPSUBDisconnect Then PSUBdisconnected.SetResult(False)
            SourceWindow.PubSubStateChanged(False)
            If SourceWindow.ProgramState And Authorized Then
                'SendMessage("RECONNECTING")
                APIstate = False
                Authorized = False
                SourceWindow.APIstateChanged(False)
                SourceWindow.CHPstateChanged(False)
                SourceWindow.TwitchAuthStateChanged(False)
                Dim ConnectTask As Task = APIauthorization(True)
            End If
        End Sub

        Private Sub ViewCountChanged(sender As Object, e As Events.OnViewCountArgs) Handles PubSub.OnViewCount
            Dim OutputString As String = "View CouNt = " & e.Viewers
            SourceWindow.LogEventString(OutputString)
        End Sub

        Private AwaitingPSUBDisconnect As Boolean = False
        Private PSUBdisconnected As TaskCompletionSource(Of Boolean)
        Private Async Function DisconnectPubSub() As Task
            If PubSub IsNot Nothing Then
                PSUBdisconnected = New TaskCompletionSource(Of Boolean)
                AwaitingPSUBDisconnect = True
                PubSub.Disconnect()
                AwaitingPSUBDisconnect = Await PSUBdisconnected.Task
            End If
        End Function

        Private Sub ChannelPointsNotification(sender As Object, e As Events.OnChannelPointsRewardRedeemedArgs) Handles PubSub.OnChannelPointsRewardRedeemed
            Dim Rewardstring As String = ""

            Select Case e.RewardRedeemed.Redemption.Reward.Title
                Case = "Ember's Hats"
                    MyOBSevents.PlayHats()
                Case = "Play Random Sound-Alert"
                    MyOBSevents.PlaySoundAlert(e.RewardRedeemed.Redemption.User.DisplayName)
                Case = "Play Sound-Alert"
                    MyOBSevents.PlaySoundAlert(e.RewardRedeemed.Redemption.User.DisplayName, Replace(e.RewardRedeemed.Redemption.UserInput, "_", " "))
                Case = "Rock and Stone"
                    Dim Speak As Task = Ember.Says("ROCK AND STONE" & vbCrLf & e.RewardRedeemed.Redemption.User.DisplayName,
                           Ember.Mood.RockandStone, "Rock And Stone",, 2800)
                Case = "Poo-Brain"
                    Luna.ChangeMood(Luna.Mood.PooBrain, 4050,, "Luna Poo Brain")
                Case Else
                    Rewardstring = "#CHPTS " & e.RewardRedeemed.Redemption.User.DisplayName & " Redeemed: " &
                e.RewardRedeemed.Redemption.Reward.Title & " for " &
                e.RewardRedeemed.Redemption.Reward.Cost & " points"
                    SourceWindow.RemoteUpdateEventBox(Rewardstring)
            End Select
        End Sub

        Private Sub ListenResponse(sender As Object, e As Events.OnListenResponseArgs) Handles PubSub.OnListenResponse
            If e.Successful = False Then
                SourceWindow.LogEventString("Failed to Listen: " & e.Response.Error)
            Else
                SourceWindow.LogEventString(e.Topic & " Message Recieved")
            End If
        End Sub

        Public Sub iGOTStheBITS(sender As Object, e As Events.OnBitsReceivedV2Args) Handles PubSub.OnBitsReceivedV2
            Dim MYSTRING As String = e.TotalBitsUsed

        End Sub

        Public Sub SubscriberAcquired(sender As Object, e As Events.OnChannelSubscriptionArgs) Handles PubSub.OnChannelSubscription
            MyOBSevents.ViewerEvent(UserEvents.NewSubscriber, e.Subscription.DisplayName)
        End Sub

        '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        '///////////////////////////////////////////////AUTHORIZATION FUNCTIONS///////////////////////////////////////////////////
        '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        Public Async Function DisconnectAPI(Optional RefreshMe As Boolean = False) As Task
            SourceWindow.IMNOTREADY()
            Await ChannelPoints.UpdateProgramRewards(False)
            Authorized = False
            APIstate = False
            SourceWindow.CHPstateChanged(False)
            SourceWindow.APIstateChanged(False)
            SourceWindow.TwitchAuthStateChanged(False)
            Await DisconnectPubSub()
            If RefreshMe Then
                Await APIauthorization(True)
            Else
                SourceWindow.IMREADY()
            End If
        End Function

        Private Sub AuthorizationAcquired(TokenMessage As String)
            Authorized = True
            SourceWindow.TwitchAuthStateChanged(True)
            GetStreamInfo()
            Dim ConnectTask As Task = ConnectPubSub()
        End Sub

        Public Sub ReadAccessToken()
            If File.Exists(AuthDirectory & "\" & "Access Token.txt") Then
                Dim Inputstring As String = File.ReadAllText(AuthDirectory & "\" & "Access Token.txt")
                AccessToken = Inputstring
            Else
                AccessToken = ""
            End If
        End Sub

        Public Sub WriteAccessToken(TokenString As String)
            If TokenString = "" Then
                TokenString = AccessToken
            Else
                AccessToken = TokenString
            End If
            File.WriteAllText(AuthDirectory & "\" & "Access Token.txt", TokenString)
        End Sub

        Public Sub ReadRefreshToken()
            If File.Exists(AuthDirectory & "\" & "Refresh Token.txt") Then
                Dim Inputstring As String = File.ReadAllText(AuthDirectory & "\" & "Refresh Token.txt")
                RefreshToken = Inputstring
            Else
                RefreshToken = ""
            End If
        End Sub

        Public Sub WriteRefreshToken(Optional TokenString As String = "")
            If TokenString = "" Then
                TokenString = RefreshToken
            Else
                RefreshToken = TokenString
            End If
            File.WriteAllText(AuthDirectory & "\" & "Refresh Token.txt", TokenString)
        End Sub

        Public Async Function APIauthorization(Optional ForceRefresh As Boolean = False, Optional ForceReset As Boolean = False) As Task
            SourceWindow.IMNOTREADY()
            AuthorizationRecieved = New TaskCompletionSource(Of Boolean)
            WaitingForAuthorization = True
            Dim APItask As Task(Of Task) = New Task(Of Task) _
                (Async Function() As Task
                     WaitingForAuthorization = Await AuthorizationRecieved.Task
                 End Function)
            Dim MyRequest As New ResourceRequest({ResourceIDs.TwitchAPI}, APItask, "TwitchAPI Getting Authorization")
            Dim RunAPItask As Task = MyResourceManager.RequestResource(MyRequest)
            If ForceReset = False Then
                If ForceRefresh = False Then
                    If AccessToken = "" Then
                        ReadAccessToken()
                    End If
                    If AccessToken = "" Then
                        GetAMotherFukkenToken()
                    Else
                        Dim CheckToken As Task = CheckTheFukkenToken()
                    End If
                Else
                    Dim FreshToken As New Task(AddressOf RefreshTheFukkenToken)
                    FreshToken.Start()
                End If
            Else
                GetAMotherFukkenToken()
            End If
            Await MyRequest.RequestCompleted.Task
        End Function

        Public Async Sub RefreshTheFukkenToken()
            Dim OutputString As String = ""
            ReadRefreshToken()
            If RefreshToken <> "" Then
                Dim ffff As Auth.RefreshResponse = Await MyAPI.Auth.RefreshAuthTokenAsync(RefreshToken, JHsecret)
                If ffff IsNot Nothing Then
                    OutputString = "Refreshed" & vbCrLf
                    OutputString = OutputString & "ExpiresIn: " & ffff.ExpiresIn & vbCrLf
                    OutputString = OutputString & "Scopes: " & vbCrLf
                    For i As Integer = 0 To ffff.Scopes.Count - 1
                        OutputString = OutputString & "-" & ffff.Scopes(i) & vbCrLf
                    Next
                    WriteAccessToken(ffff.AccessToken)
                    WriteRefreshToken(ffff.RefreshToken)
                    MyAPI.Settings.AccessToken = AccessToken
                    RaiseEvent TokenAcquired(OutputString)
                Else
                    GetAMotherFukkenToken()
                End If
            Else
                GetAMotherFukkenToken()
            End If
        End Sub

        Public Async Function CheckTheFukkenToken() As Task
            Dim fff As Auth.ValidateAccessTokenResponse = Await MyAPI.Auth.ValidateAccessTokenAsync(AccessToken)
            Dim OutputString As String = ""
            If fff IsNot Nothing Then
                OutputString = OutputString & "Login: " & fff.Login & vbCrLf
                OutputString = OutputString & "ClientID: " & fff.ClientId & vbCrLf
                OutputString = OutputString & "UserID: " & fff.UserId & vbCrLf
                OutputString = OutputString & "ExpiresIn: " & fff.ExpiresIn & vbCrLf
                OutputString = OutputString & "Scopes: " & vbCrLf
                For i As Integer = 0 To fff.Scopes.Count - 1
                    OutputString = OutputString & "-" & fff.Scopes(i) & vbCrLf
                Next
                If fff.ExpiresIn > 0 Then
                    MyAPI.Settings.AccessToken = AccessToken
                    RaiseEvent TokenAcquired(OutputString)
                Else
                    Dim FreshToken As New Task(AddressOf RefreshTheFukkenToken)
                    FreshToken.Start()
                End If
            Else
                Dim FreshToken As New Task(AddressOf RefreshTheFukkenToken)
                FreshToken.Start()
            End If
        End Function

        Public Async Sub GetAFuckingToken()
            Dim TokenResponse As Auth.AuthCodeResponse = Await MyAPI.Auth.GetAccessTokenFromCodeAsync(CodeString, JHsecret, "http://localhost:8080")
            Dim OutputString As String = ""
            OutputString = OutputString & "ExpiresIn: " & TokenResponse.ExpiresIn & vbCrLf
            OutputString = OutputString & "Scopes: " & vbCrLf
            For i As Integer = 0 To TokenResponse.Scopes.Count - 1
                OutputString = OutputString & "-" & TokenResponse.Scopes(i) & vbCrLf
            Next
            WriteAccessToken(TokenResponse.AccessToken)
            WriteRefreshToken(TokenResponse.RefreshToken)
            MyAPI.Settings.AccessToken = AccessToken
            RaiseEvent TokenAcquired(OutputString)
        End Sub

        Public Sub GetAMotherFukkenToken()
            Dim urlstring As String = MyAPI.Auth.GetAuthorizationCodeUrl("http://localhost:8080", ScopesList)
            Process.Start(urlstring)

            Dim Server As HttpListener = New HttpListener
            Server.Prefixes.Add("http://localhost:8080/")
            Server.Start()
            Dim context As HttpListenerContext = Server.GetContext
            'context.Response = New HttpListenerResponse

            CodeString = DataExtract(context.Request.Url.OriginalString, "?code=", "&scope=")

            Server.Stop()
            Server.Close()

            Dim GetToken As New Task(AddressOf GetAFuckingToken)
            GetToken.Start()
        End Sub

        Private Async Function TryReconnectAsync() As Task
            If Authorized = True Then
                Await DisconnectPubSub()
            Else
                Await APIauthorization(True)
            End If
        End Function


        Private Sub TryReconnect()
            Dim ReconnectThread As New Thread(
                Sub()
                    Dim ReconnectTask As Task = TryReconnectAsync()
                End Sub)
            ReconnectThread.Start()
        End Sub

        Private MyChannelData As Helix.Models.Channels.GetChannelInformation.ChannelInformation
        Private APIstate As Boolean
        Private MyCheckValue As Helix.Models.Chat.GetUserChatColor.GetUserChatColorResponse
        Private Async Function CheckAPIstate() As Task(Of Boolean)
            If Authorized Then
                Try
                    Await Task.Delay(1)
                    Dim MyUserList As New List(Of String)
                    MyUserList.Add(JHID)
                    MyCheckValue = Await MyAPI.Helix.Chat.GetUserChatColorAsync(MyUserList)
                    APIstate = True
                    SourceWindow.APIstateChanged(True)
                    Return True
                Catch
                    APIstate = False
                    SourceWindow.APIstateChanged(False)
                    TryReconnect()
                    Return False
                End Try
            Else
                APIstate = False
                SourceWindow.APIstateChanged(False)
                TryReconnect()
                Return False
            End If
        End Function

        '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        '///////////////////////////////////////////////STREAM INFO FUNCTIONS/////////////////////////////////////////////////////
        '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        Public Sub SetStreamInfo()
            Dim APItask As Task(Of Task) = New Task(Of Task) _
           (Async Function() As Task
                If Await CheckAPIstate() Then
                    Dim InfoRequest As New Helix.Models.Channels.ModifyChannelInformation.ModifyChannelInformationRequest
                    InfoRequest.BroadcasterLanguage = GamesList.StreamLanguage
                    InfoRequest.GameId = GamesList.StreamGameID
                    InfoRequest.Title = GamesList.StreamTitle
                    Await MyAPI.Helix.Channels.ModifyChannelInformationAsync(JHID, InfoRequest)
                    If GamesList.StreamGameIndex > -1 Then
                        Dim TagList As List(Of String) = GamesList.GetTagsList
                        If TagList.Count < 6 Then
                            Await MyAPI.Helix.Streams.ReplaceStreamTagsAsync(JHID, TagList, AccessToken)
                        End If
                    End If
                    RaiseEvent StreamInfoUpdated()
                End If
            End Function)
            Dim MyRequest As New ResourceRequest({ResourceIDs.TwitchAPI}, APItask, "TwitchAPI Set Stream Info")
            Dim RunAPItask As Task = MyResourceManager.RequestResource(MyRequest)
        End Sub

        Public Sub GetStreamInfo()
            Dim APItask As Task(Of Task) = New Task(Of Task) _
           (Async Function() As Task
                If Await CheckAPIstate() Then
                    MyChannelData = (Await MyAPI.Helix.Channels.GetChannelInformationAsync(JHID)).Data(0)
                    GamesList.StreamLanguage = MyChannelData.BroadcasterLanguage
                    GamesList.SetGameID(MyChannelData.GameId)
                    Dim GameName As String = MyChannelData.GameName
                    Dim Delay As Integer = MyChannelData.Delay
                    GamesList.StreamTitle = MyChannelData.Title
                    Dim OUTPUTSTRING As String = "STREAM INFO" & vbCrLf
                    OUTPUTSTRING = OUTPUTSTRING & "Title: " & GamesList.StreamTitle & vbCrLf
                    OUTPUTSTRING = OUTPUTSTRING & "Language: " & GamesList.StreamLanguage & vbCrLf
                    OUTPUTSTRING = OUTPUTSTRING & "GameID: " & GamesList.StreamGameID & vbCrLf
                    OUTPUTSTRING = OUTPUTSTRING & "GameName: " & GameName & vbCrLf
                    'OUTPUTSTRING = OUTPUTSTRING & "Delay: " & Delay & vbCrLf
                    'OUTPUTSTRING = OUTPUTSTRING & TagDataString
                    'SendMessage(OUTPUTSTRING)
                    RaiseEvent StreamInfoAvailable(OUTPUTSTRING)
                End If
            End Function)
            Dim MyRequest As New ResourceRequest({ResourceIDs.TwitchAPI}, APItask, "TwitchAPI Get Stream Info")
            Dim RunAPItask As Task = MyResourceManager.RequestResource(MyRequest)
        End Sub




        '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        '////////////////////////////////////////////////CHANNEL POINT REWARDS////////////////////////////////////////////////////
        '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        Public Sub DeleteChannelPointsReward(RewardIndex As Integer)
            Dim APItask As Task(Of Task) = New Task(Of Task) _
           (Async Function() As Task
                If Await CheckAPIstate() Then
                    Await MyAPI.Helix.ChannelPoints.DeleteCustomRewardAsync(JHID, ChannelPoints.Rewards(RewardIndex).TwitchData.Id)
                    Await GetChannelPointData()
                End If
            End Function)
            Dim MyRequest As New ResourceRequest({ResourceIDs.TwitchAPI}, APItask, "TwitchAPI Get Stream Info")
            Dim RunAPItask As Task = MyResourceManager.RequestResource(MyRequest)
        End Sub

        Public Function UpdateChannelPointDataExplicit(RewardIndex As Integer,
                                                  Request As Helix.Models.ChannelPoints.UpdateCustomReward.UpdateCustomRewardRequest) As Task
            Dim APItask As Task(Of Task) = New Task(Of Task) _
           (Async Function() As Task
                If Await CheckAPIstate() Then
                    Dim Response As Helix.Models.ChannelPoints.UpdateCustomReward.UpdateCustomRewardResponse =
                            Await MyAPI.Helix.ChannelPoints.UpdateCustomRewardAsync(JHID, ChannelPoints.Rewards(RewardIndex).TwitchData.Id, Request)
                    ChannelPoints.Rewards(RewardIndex).ReadResponse(Response)
                    ChannelPoints.RewardUpdated(RewardIndex)
                End If
            End Function)
            Dim MyRequest As New ResourceRequest({ResourceIDs.TwitchAPI}, APItask, "Updating Channel Point Reward")
            Dim RunAPItask As Task = MyResourceManager.RequestResource(MyRequest)
            Return MyRequest.RequestCompleted.Task
        End Function

        Public Sub CreateChannelPointReward(Request As Helix.Models.ChannelPoints.CreateCustomReward.CreateCustomRewardsRequest)
            Dim APItask As Task(Of Task) = New Task(Of Task) _
           (Async Function() As Task
                If Await CheckAPIstate() Then
                    Dim Response As Helix.Models.ChannelPoints.CreateCustomReward.CreateCustomRewardsResponse =
                    Await MyAPI.Helix.ChannelPoints.CreateCustomRewardsAsync(JHID, Request)
                    Await GetChannelPointData()
                End If
            End Function)
            Dim MyRequest As New ResourceRequest({ResourceIDs.TwitchAPI}, APItask, "Creating Channel Point Reward")
            Dim RunAPItask As Task = MyResourceManager.RequestResource(MyRequest)
        End Sub

        Public Sub UpdateChannelPointReward(RewardIndex As Integer)
            Dim APItask As Task(Of Task) = New Task(Of Task) _
           (Async Function() As Task
                If Await CheckAPIstate() Then
                    Dim Request As Helix.Models.ChannelPoints.UpdateCustomReward.UpdateCustomRewardRequest =
                        ChannelPoints.Rewards(RewardIndex).GetUpdateData
                    Dim Response As Helix.Models.ChannelPoints.UpdateCustomReward.UpdateCustomRewardResponse =
                 Await MyAPI.Helix.ChannelPoints.UpdateCustomRewardAsync(JHID, ChannelPoints.Rewards(RewardIndex).TwitchData.Id, Request)
                    ChannelPoints.Rewards(RewardIndex).ReadResponse(Response)
                    ChannelPoints.RewardUpdated(RewardIndex)
                End If
            End Function)
            Dim MyRequest As New ResourceRequest({ResourceIDs.TwitchAPI}, APItask, "Updating Channel Point Reward")
            Dim RunAPItask As Task = MyResourceManager.RequestResource(MyRequest)
        End Sub

        Public Sub SyncChannelRedemptions()
            Dim APItask As Task(Of Task) = New Task(Of Task) _
           (Async Function() As Task
                If Await CheckAPIstate() Then
                    Await GetChannelPointData()
                End If

            End Function)
            Dim MyRequest As New ResourceRequest({ResourceIDs.TwitchAPI}, APItask, "Syncing All Channel Point Data")
            Dim RunAPItask As Task = MyResourceManager.RequestResource(MyRequest)
        End Sub

        Private Async Function GetChannelPointData() As Task
            Dim ChannelPointData As Helix.Models.ChannelPoints.GetCustomReward.GetCustomRewardsResponse =
                Await MyAPI.Helix.ChannelPoints.GetCustomRewardAsync(JHID)

            ChannelPoints.InitRewardData(ChannelPointData.Data.Length)
            Dim RewardData As Helix.Models.ChannelPoints.CustomReward
            For i As Integer = 0 To ChannelPointData.Data.Length - 1
                RewardData = ChannelPointData.Data.GetValue(i)
                ChannelPoints.Rewards(i).ReadRedemptionDatafromTwitch(RewardData)
            Next
            ChannelPoints.RewardsUpdated()

        End Function

    End Class

    Public Class ChannelPointData

        Public Rewards() As RedemptionData
        Public Event AllRewardsUpdated()
        Public Event SingleRewardUpdated(RewardNum As Integer)
        Public Event SingleRewardRedeemed(RewardNum As Integer)
        Public Event NewRewardCreated()

        Public Async Function UpdateProgramRewards(EnableProgramRewards As Boolean) As Task
            For I As Integer = 0 To Rewards.Length - 1
                If Rewards(I).TwitchData.IsEnabled <> EnableProgramRewards Then
                    If EnableProgramRewards Then
                        If Rewards(I).IsProgramDependant And Rewards(I).IsAutoEnabled Then
                            Await ToggleRewardEnable(I)
                        End If
                    Else
                        If Rewards(I).IsProgramDependant Then
                            Await ToggleRewardEnable(I)
                        End If
                    End If
                End If
            Next
        End Function

        Public Sub New()
            Rewards = Nothing
        End Sub

        Public Sub UpdateReward(RewardIndex As Integer, Request As Helix.Models.ChannelPoints.UpdateCustomReward.UpdateCustomRewardRequest)
            myAPI.UpdateChannelPointDataExplicit(RewardIndex, Rewards(RewardIndex).GetUpdateData(, Request))
        End Sub

        Public Async Function ToggleRewardEnable(RewardIndex As Integer) As Task
            Dim Request As Helix.Models.ChannelPoints.UpdateCustomReward.UpdateCustomRewardRequest = New Helix.Models.ChannelPoints.UpdateCustomReward.UpdateCustomRewardRequest
            If Rewards(RewardIndex).TwitchData.IsEnabled Then
                Request.IsEnabled = False
            Else
                Request.IsEnabled = True
            End If
            Await myAPI.UpdateChannelPointDataExplicit(RewardIndex, Rewards(RewardIndex).GetUpdateData(, Request))
        End Function

        Public Sub RewardsUpdated()
            RaiseEvent AllRewardsUpdated()
        End Sub

        Public Sub RewardUpdated(RewardIndex As Integer)
            RaiseEvent SingleRewardUpdated(RewardIndex)
        End Sub

        Public Sub InitRewardData(RewardCount As Integer)
            ReDim Rewards(0 To RewardCount - 1)
            For i As Integer = 0 To RewardCount - 1
                Rewards(i) = New RedemptionData
            Next
        End Sub

        Public Sub DeleteRward(RewardIndex As Integer)
            Rewards(RewardIndex).DeleteLocalData()
            myAPI.DeleteChannelPointsReward(RewardIndex)
        End Sub

        Public Sub SyncRewardData(RewardsList As List(Of RedemptionData))
            Dim RewardCount As Integer = RewardsList.Count - 1
            ReDim Rewards(0 To RewardCount)
            For I = 0 To RewardCount
                Rewards(I) = RewardsList(I)
            Next
        End Sub

    End Class

    Public RewardDataDirectory As String = "\\StreamPC-V2\OBS Assets\Rewards"

    Public Class RedemptionData
        Public TwitchData As Helix.Models.ChannelPoints.CustomReward
        Public IsProgramDependant As Boolean
        Public IsAutoEnabled As Boolean
        Public Access As Mutex

        Public Sub New()
            If Access Is Nothing Then Access = New Mutex
            Access.WaitOne()
            TwitchData = Nothing
            IsProgramDependant = False
            IsAutoEnabled = False
            Access.ReleaseMutex()
        End Sub

        Public Function GetCopyRedemptionData(Optional BypassMutex As Boolean = False) As Helix.Models.ChannelPoints.CreateCustomReward.CreateCustomRewardsRequest
            If Access Is Nothing Then Access = New Mutex
            If BypassMutex = False Then Access.WaitOne()
            Dim Output As New Helix.Models.ChannelPoints.CreateCustomReward.CreateCustomRewardsRequest
            Output.BackgroundColor = TwitchData.BackgroundColor
            Output.GlobalCooldownSeconds = TwitchData.GlobalCooldownSetting.GlobalCooldownSeconds
            Output.IsGlobalCooldownEnabled = TwitchData.GlobalCooldownSetting.IsEnabled
            Output.IsMaxPerStreamEnabled = TwitchData.MaxPerStreamSetting.IsEnabled
            Output.IsUserInputRequired = TwitchData.IsUserInputRequired
            Output.Cost = TwitchData.Cost
            Output.IsEnabled = TwitchData.IsEnabled
            Output.Title = TwitchData.Title & " 2"
            Output.Prompt = TwitchData.Prompt & " 2"
            If BypassMutex = False Then Access.ReleaseMutex()
            Return Output
        End Function

        Public Function GetUpdateData(Optional BypassMutex As Boolean = False,
                                      Optional Request As Helix.Models.ChannelPoints.UpdateCustomReward.UpdateCustomRewardRequest = Nothing) As _
                                      Helix.Models.ChannelPoints.UpdateCustomReward.UpdateCustomRewardRequest

            If Access Is Nothing Then Access = New Mutex
            If BypassMutex = False Then Access.WaitOne()
            If Request Is Nothing Then Request = New Helix.Models.ChannelPoints.UpdateCustomReward.UpdateCustomRewardRequest

            If Request.BackgroundColor Is Nothing Then _
                Request.BackgroundColor = TwitchData.BackgroundColor
            If Request.BroadcasterId Is Nothing Then _
                Request.BroadcasterId = TwitchData.BroadcasterId

            If Request.IsGlobalCooldownEnabled Is Nothing Then
                If TwitchData.GlobalCooldownSetting.GlobalCooldownSeconds <> 0 Then
                    Request.IsGlobalCooldownEnabled = True
                    Request.GlobalCooldownSeconds = TwitchData.GlobalCooldownSetting.GlobalCooldownSeconds
                Else
                    Request.IsGlobalCooldownEnabled = False
                    Request.GlobalCooldownSeconds = 0
                End If
            End If

            If Request.IsMaxPerStreamEnabled Is Nothing Then
                If TwitchData.MaxPerStreamSetting.MaxPerStream <> 0 Then
                    Request.IsMaxPerStreamEnabled = True
                    Request.MaxPerStream = TwitchData.MaxPerStreamSetting.MaxPerStream
                Else
                    Request.IsMaxPerStreamEnabled = False
                    Request.MaxPerStream = 0
                End If
            End If

            If Request.IsMaxPerUserPerStreamEnabled Is Nothing Then
                If TwitchData.MaxPerUserPerStreamSetting.MaxPerUserPerStream <> 0 Then
                    Request.IsMaxPerUserPerStreamEnabled = True
                    Request.MaxPerUserPerStream = TwitchData.MaxPerUserPerStreamSetting.MaxPerUserPerStream
                Else
                    Request.IsMaxPerUserPerStreamEnabled = False
                    Request.MaxPerUserPerStream = 0
                End If
            End If

            'SendMessage("MPSE: " & Request.IsMaxPerStreamEnabled & " MPSV: " & Request.MaxPerStream & " MPUPS: " & Request.IsMaxPerUserPerStreamEnabled & " MPUPSV: " & Request.MaxPerUserPerStream)

            If Request.ShouldRedemptionsSkipRequestQueue Is Nothing Then _
                Request.ShouldRedemptionsSkipRequestQueue = TwitchData.ShouldRedemptionsSkipQueue
            If Request.IsUserInputRequired Is Nothing Then _
                Request.IsUserInputRequired = TwitchData.IsUserInputRequired

            If Request.Cost Is Nothing Then Request.Cost = TwitchData.Cost
            If Request.IsEnabled Is Nothing Then Request.IsEnabled = TwitchData.IsEnabled
            If Request.Title Is Nothing Then Request.Title = TwitchData.Title
            If Request.Prompt Is Nothing Then Request.Prompt = TwitchData.Prompt

            If BypassMutex = False Then Access.ReleaseMutex()
            Return Request
        End Function

        Public Sub ReadResponse(Response As Helix.Models.ChannelPoints.UpdateCustomReward.UpdateCustomRewardResponse)
            If Response.Data.Length > 0 Then
                TwitchData = Response.Data(0)
            End If
        End Sub

        Public Sub SyncLocalData(Optional BypassMutex As Boolean = False)
            If Access Is Nothing Then Access = New Mutex
            If BypassMutex = False Then Access.WaitOne()
            Dim Path As String = RewardDataDirectory & "\" & TwitchData.Id
            If Directory.Exists(Path) Then
                Dim OutputData As New List(Of String)
                OutputData.Add("IsProgramDependant<>" & IsProgramDependant)
                OutputData.Add("IsAutoEnabled<>" & IsAutoEnabled)
                File.WriteAllText(Path & "\Reward Properties.txt", Join(OutputData.ToArray, vbCrLf))
            End If
            If BypassMutex = False Then Access.ReleaseMutex()
        End Sub

        Public Sub DeleteLocalData(Optional BypassMutex As Boolean = False)
            If Access Is Nothing Then Access = New Mutex
            If BypassMutex = False Then Access.WaitOne()
            Dim Path As String = RewardDataDirectory & "\" & TwitchData.Id
            If Directory.Exists(Path) Then
                Directory.Delete(Path, True)
            End If
            If BypassMutex = False Then Access.ReleaseMutex()
        End Sub

        Public Sub CheckLocalData(Optional BypassMutex As Boolean = False)
            If Access Is Nothing Then Access = New Mutex
            If BypassMutex = False Then Access.WaitOne()
            Dim Path As String = RewardDataDirectory & "\" & TwitchData.Id
            If Directory.Exists(Path) Then
                If File.Exists(Path & "\Reward Properties.txt") Then
                    Dim InputData() As String = Split(File.ReadAllText(Path & "\Reward Properties.txt"), vbCrLf)
                    For i As Integer = 0 To InputData.Length - 1
                        Dim InputDataPoint() As String = Split(InputData(i), "<>")
                        Select Case InputDataPoint(0)
                            Case "IsProgramDependant"
                                IsProgramDependant = InputDataPoint(1)
                            Case "IsAutoEnabled"
                                IsAutoEnabled = InputDataPoint(1)
                        End Select
                    Next
                End If
            End If
            If BypassMutex = False Then Access.ReleaseMutex()
        End Sub

        Public Function TwitchDataToString(Optional BypassMutex As Boolean = False) As String
            If Access Is Nothing Then Access = New Mutex
            If BypassMutex = False Then Access.WaitOne()
            Dim outputstring As String = ""
            If TwitchData IsNot Nothing Then
                outputstring = "Custom Reward Data:" & vbCrLf
                outputstring = outputstring & "ID: " & TwitchData.Id & vbCrLf
                outputstring = outputstring & "Title: " & TwitchData.Title & vbCrLf
                outputstring = outputstring & "Prompt: " & TwitchData.Prompt & vbCrLf
                outputstring = outputstring & "Enabled: " & TwitchData.IsEnabled & vbCrLf
                outputstring = outputstring & "Cost: " & TwitchData.Cost & vbCrLf
                outputstring = outputstring & "Redeem Count: " & TwitchData.RedemptionsRedeemedCurrentStream & vbCrLf
                outputstring = outputstring & "IsInStock: " & TwitchData.IsInStock & vbCrLf
                outputstring = outputstring & "IsPaused: " & TwitchData.IsPaused & vbCrLf
                outputstring = outputstring & "CD Expires: " & TwitchData.CooldownExpiresAt & vbCrLf
                outputstring = outputstring & "SkipQueue: " & TwitchData.ShouldRedemptionsSkipQueue & vbCrLf
                outputstring = outputstring & "UI Req: " & TwitchData.IsUserInputRequired & vbCrLf
                outputstring = outputstring & "Global CD(s): " & TwitchData.GlobalCooldownSetting.GlobalCooldownSeconds & vbCrLf
                outputstring = outputstring & "Global CD(en): " & TwitchData.GlobalCooldownSetting.IsEnabled & vbCrLf
                outputstring = outputstring & "Max RpStream(en): " & TwitchData.MaxPerStreamSetting.IsEnabled & vbCrLf
                outputstring = outputstring & "Max RpStream(ct): " & TwitchData.MaxPerStreamSetting.MaxPerStream & vbCrLf
                outputstring = outputstring & "Max RpStream/Usr(en): " & TwitchData.MaxPerUserPerStreamSetting.IsEnabled & vbCrLf
                outputstring = outputstring & "Max RpStream/Usr(ct): " & TwitchData.MaxPerUserPerStreamSetting.MaxPerUserPerStream & vbCrLf
                outputstring = outputstring & "BG Color: " & TwitchData.BackgroundColor & vbCrLf & vbCrLf
            End If
            If BypassMutex = False Then Access.ReleaseMutex()
            Return outputstring
        End Function

        Public Sub ReadRedemptionDatafromTwitch(InputData As Helix.Models.ChannelPoints.CustomReward)
            If Access Is Nothing Then Access = New Mutex
            Access.WaitOne()
            TwitchData = InputData
            CheckLocalData(True)
            Access.ReleaseMutex()
        End Sub

    End Class




    '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    '///////////////////////////////////////////////////STREAM SETTINGS///////////////////////////////////////////////////////
    '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    Public Class TwitchGames
        Public Games() As GameSet
        Public StreamTitle As String
        Public StreamGameIndex As Integer
        Public StreamGameID As Integer
        Public StreamLanguage As String
        Public Const MaxGames As Integer = 8

        Public Sub New()
            ReDim Games(0 To MaxGames - 1)
            Games(0).initART()
            Games(1).initBL3()
            Games(2).initStarBound()
            Games(3).InitMHR()
            Games(4).InitMHW()
            Games(5).InitSoftwareDev()
            Games(6).InitAnimalCrossing()
            Games(7).InitDRG()
        End Sub

        Public Sub SetGameIndexByName(GameName As String)
            For i As Integer = 0 To MaxGames - 1
                If Games(i).Name = GameName Then
                    StreamGameIndex = i
                    StreamGameID = Games(i).Code
                    Exit Sub
                End If
            Next
            StreamGameIndex = -1
        End Sub

        Public Function GetGameName() As String
            If StreamGameIndex > -1 Then
                Return Games(StreamGameIndex).Name
            Else
                Return ""
            End If
        End Function

        Public Function GetTagsList() As List(Of String)
            If StreamGameIndex > -1 Then
                Return Games(StreamGameIndex).Tags
            Else
                Return New List(Of String)
            End If
        End Function

        Public Sub SetGameID(GameId As Integer)
            StreamGameID = GameId
            For i As Integer = 0 To MaxGames - 1
                If Games(i).Code = StreamGameID Then
                    StreamGameIndex = i
                    Exit Sub
                End If
            Next
            StreamGameIndex = -1
        End Sub

    End Class

    Public Structure GameSet
        Public Name As String
        Public Code As Integer
        Public Tags As List(Of String)


        Public Sub initBL3()
            Name = "Borderlands 3"
            Code = StreamGames.Borderlands3
            Tags = New List(Of String)
            Tags.Add(StreamTags.Multiplayer)
            Tags.Add(StreamTags.CasualPlaythrough)
            'Tags.Add(StreamTags.Cooperative)
            'Tags.Add(StreamTags.FirstPlaythrough)
        End Sub

        Public Sub initART()
            Name = "Art"
            Code = StreamGames.Art
            Tags = New List(Of String)
            Tags.Add(StreamTags.Coloring)
            Tags.Add(StreamTags.Cooperative)
            Tags.Add(StreamTags.OriginalWork)
            Tags.Add(StreamTags.DigitalArt)
        End Sub
        Public Sub initStarBound()
            Name = "Starbound"
            Code = StreamGames.Starbound
            Tags = New List(Of String)
            Tags.Add(StreamTags.Multiplayer)
            Tags.Add(StreamTags.CasualPlaythrough)
            Tags.Add(StreamTags.Cooperative)
            Tags.Add(StreamTags.FirstPlaythrough)
        End Sub

        Public Sub InitMHW()
            Name = "Monster Hunter World"
            Code = StreamGames.MonsterHunterWorld
            Tags = New List(Of String)
            Tags.Add(StreamTags.Multiplayer)
            Tags.Add(StreamTags.CasualPlaythrough)
            Tags.Add(StreamTags.Cooperative)
            Tags.Add(StreamTags.Hiking)
        End Sub

        Public Sub InitMHR()
            Name = "Monster Hunter Rise"
            Code = StreamGames.MonsterHunterRise
            Tags = New List(Of String)
            Tags.Add(StreamTags.Multiplayer)
            Tags.Add(StreamTags.CasualPlaythrough)
            Tags.Add(StreamTags.Cooperative)
            Tags.Add(StreamTags.Hiking)
        End Sub

        Public Sub InitSoftwareDev()
            Name = "Software Development"
            Code = StreamGames.SoftwareAndGameDev
            Tags = New List(Of String)
            Tags.Add(StreamTags.Design)
            Tags.Add(StreamTags.Programming)
            Tags.Add(StreamTags.SoftwareDev)
            Tags.Add(StreamTags.Engineering)
        End Sub

        Public Sub InitAnimalCrossing()
            Name = "Animal Crossing"
            Code = StreamGames.AnimalCrossingNH
            Tags = New List(Of String)
            Tags.Add(StreamTags.CasualPlaythrough)
            Tags.Add(StreamTags.MentalHealth)
            Tags.Add(StreamTags.FirstPlaythrough)

        End Sub

        Public Sub InitDRG()
            Name = "Deep Rock Galactic"
            Code = StreamGames.DRG
            Tags = New List(Of String)
            Tags.Add(StreamTags.Multiplayer)
            Tags.Add(StreamTags.Cooperative)
            Tags.Add(StreamTags.Programming)

        End Sub

        Public Sub InitFitness()
            Name = "Fitness Training"
            Code = StreamGames.FitnessAndHealth
            Tags = New List(Of String)
            Tags.Add(StreamTags.EnduranceTraining)
            Tags.Add(StreamTags.StrengthTraining)
            Tags.Add(StreamTags.MentalHealth)
            Tags.Add(StreamTags.Programming)

        End Sub

    End Structure

    Public Structure StreamTags
        Public Const Coloring As String = "319e29b7-9b61-4f0b-ad11-43e4c01ca697"
        Public Const DesktopDev As String = "a106f013-6e26-4f27-9a4b-01e9d76084e2"
        Public Const t3dModeling As String = "b97ee881-e15a-455d-9876-657fcba7cfd8"
        Public Const t3dPrinting As String = "6803fe7e-4e3d-4369-a8ae-1dc2afb758f9"
        Public Const ArtCommissions As String = "df448da8-7082-45b2-92ad-c624dba6551f"
        Public Const CasualPlaythrough As String = "cc8d5abb-39c9-4942-a1ee-e1558512119e"
        Public Const Charity As String = "bb5e7234-380e-48ad-a59c-03e51274e478"
        Public Const Costream As String = "adb55316-f1ab-4f81-9ff5-17da7719d0ee"
        Public Const Cooperative As String = "63e83904-a70b-4709-b963-a37a105d9932"
        Public Const Cosplay As String = "2ffd5c3e-b927-4749-ba53-79d3b626b2da"
        Public Const Crafting As String = "541dad34-35c6-4431-a1f5-9d7e8a5b5b3a"
        Public Const CrossStitch As String = "572bbbbc-ab94-4cbd-8c1f-649d856267b5"
        Public Const Depression As String = "c1ea4496-4418-4a4f-bc30-083cfc6a616b"
        Public Const Design As String = "7b49f69a-5d95-4c94-b7e3-66e2c0c6f6c6"
        Public Const DigitalArt As String = "02ba4017-ed3b-4b82-ab20-011860784f77"
        Public Const Drawing As String = "f2d10665-9980-452a-b091-f0ca30777d4b"
        Public Const EmoteDesign As String = "9ba743f4-85e4-40e2-a83c-c5b24aab797b"
        Public Const EnduranceTraining As String = "3379ff9c-3c15-4bef-bfb8-2085cd196062"
        Public Const Engineering As String = "dff0aca6-52fe-4cc4-a93a-194852b522f0"
        Public Const ElectronicMusic As String = "338d7a92-8bcc-429e-a30c-9f1c41a2d79a"
        Public Const FanArt As String = "86bd38a4-8ef9-4e4a-892f-f4d0aa336d67"
        Public Const FirstPlaythrough As String = "d0976a7e-26a7-4a48-9225-c522808540f2"
        Public Const Gardening As String = "69b5f1ed-6224-49c2-8c87-708fe3850f0a"
        Public Const GraphicDesign As String = "0930677c-dd75-424d-9190-b779f3d1c136"
        Public Const Hiking As String = "2a824c85-8c64-4a62-9532-84a50633c6fc"
        Public Const MentalHealth As String = "589c4c39-a3cc-41fa-b70c-bf0b735fa21f"
        Public Const Metalwork As String = "cee0e0bb-60b1-46a8-be53-ba46c05727b3"
        Public Const Mindfulness As String = "9801169b-afa4-4785-890e-53911955e4d7"
        Public Const Multiplayer As String = "ff56eeeb-99ed-4a60-93fc-0b3f05d8661e"
        Public Const MusicEvents As String = "c5247b10-deec-4d7a-84a5-db6a75cb5908"
        Public Const MusicProduction As String = "ddb625af-5920-49cc-9f13-3716f87941dc"
        Public Const OriginalWork As String = "ba8a733b-9721-4980-812b-e40da19e860a"
        Public Const Outdoors As String = "89e105c9-2c45-42a9-a5f0-fc1ea6e7ba8b"
        Public Const Painting As String = "1d3d4f9c-be88-44f3-8e80-3b0779308c64"
        Public Const PartyGame As String = "13c95a9e-a17d-4225-b199-d08cb8e66f40"
        Public Const Programming As String = "a59f1e4e-257b-4bd0-90c7-189c3efbf917"
        Public Const Sculpture As String = "e844c795-5128-4ca0-a950-f64dabf5a324"
        Public Const Sewing As String = "0b122d41-1e9b-4086-9a42-1e7db94f1bd1"
        Public Const Singing As String = "8c1ada77-52f0-4935-a1b0-a02dc65f28b4"
        Public Const SoftwareDev As String = "6f86127d-6051-4a38-94bb-f7b475dde109"
        Public Const Spirituality As String = "63f8ee17-1678-41ee-906a-8c55c3ce81df"
        Public Const StrengthTraining As String = "07a1b526-3725-4ba5-8c53-efaad4941fb9"
        Public Const TraditionalArt As String = "5ec52c4f-a055-404c-82fe-ea98c74c7fe6"
        Public Const Tutorial As String = "dc709206-c072-4340-a706-694578574c7e"
        Public Const VectorArt As String = "f0ab2b07-14ed-4429-8ea3-3d7d400a50cd"
        Public Const Woodwork As String = "c0bfabb1-0a43-41a8-8f6c-c7c434ca18f9"
        Public Const Writing As String = "81975ba3-5c53-41f8-b614-ce5b3193955a"
    End Structure

    Public Structure StreamGames
        Public Const Borderlands3 As Integer = 491318
        Public Const Starbound As Integer = 33945
        Public Const Art As Integer = 509660
        Public Const Terraria As Integer = 31376
        Public Const SoftwareAndGameDev As Integer = 1469308723
        Public Const FitnessAndHealth As Integer = 509671
        Public Const MakersAndCrafting As Integer = 509673
        Public Const MonsterHunterWorld As Integer = 497467
        Public Const MonsterHunterRise As Integer = 1275666892
        Public Const SkyrimSE As Integer = 1050003477
        Public Const DRG As Integer = 494839
        Public Const PokemonFR_LG As Integer = 13332
        Public Const AnimalCrossingNH As Integer = 509538
    End Structure


    Public Class WebServer

        Private Server As HttpListener

        Public Sub New()
            Server = New HttpListener
            Server.Prefixes.Add(IPAddress.Loopback.ToString & ":80")
            Server.Start()
            Dim context As HttpListenerContext = Server.GetContext
            Dim sr As StreamReader = New StreamReader(context.Request.InputStream)
            Dim outputstring As String = sr.ReadToEnd
            sr.Close()
            Server.Stop()
            Server.Close()
            SendMessage(outputstring)
        End Sub

    End Class

    '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    '/////////////////////////////////////////////CHAT & USER FUNCTIONS///////////////////////////////////////////////////////
    '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    Public Class IrcClient

        Public SendBuffer As ConcurrentQueue(Of String)

        Public UserName As String
        Public Event IRCconnected()
        Public Event IRCdisconnected()
        Public Event IRCeventRecieved(IRCmessage As String)
        Public Event IRCmessageRecieved(IRCmessage As String)
        Private Channel As String
        Private ChatActive As Boolean
        Private ChannelPart As Boolean
        Private VarTcpClient As TcpClient
        Private Inputstream As StreamReader
        Private Outputstream As StreamWriter
        Private MyIPaddress As String
        Private MyPort As Integer
        Private MyPassword As String

        Public Sub New(IPaddress As String, IntPort As Integer, ThisUserName As String, Password As String, ThisChannel As String)
            UserName = ThisUserName
            Channel = ThisChannel
            MyPort = IntPort
            MyIPaddress = IPaddress
            MyPassword = Password
            ChannelPart = False
            ChatActive = False
        End Sub

        Public Sub ChatReady()
            If ChatActive = True Then
                SendBuffer.Enqueue("/emoteonlyoff")
                SendBuffer.Enqueue("/subscribersoff")
            End If
        End Sub

        Public Sub ChatOff()
            If ChatActive = True Then
                SendBuffer.Enqueue("/emoteonly")
                SendBuffer.Enqueue("/subscribers")
            End If
        End Sub

        Private Async Function ChatSpeaker() As Task
            SendBuffer = New ConcurrentQueue(Of String)
            Dim OutputMessage As String = ""
            Do Until ChannelPart = True Or ChatActive = False
                If SendBuffer.Count <> 0 Then
                    If SendBuffer.TryDequeue(OutputMessage) = True Then
                        If OutputMessage <> "" Then SendPublicChatMessage(OutputMessage)
                        Await Task.Delay(333)
                    End If
                Else
                    Await Task.Delay(20)
                End If
            Loop
            If ChannelPart = True Then
                SendIrcMessage("PART #" & Broadcastername)
                ChannelPart = False
            End If
        End Function

        Private Async Function ChatReader() As Task
            Dim InputMessage As String = ""
            Do Until ChatActive = False
                InputMessage = Await ReadMessage()
                If InStr(InputMessage, "Error recieving message: ") > 0 Then
                    RaiseEvent IRCmessageRecieved(InputMessage)
                    ChatActive = False
                    Close()
                    RaiseEvent IRCdisconnected()
                    Await Task.Delay(1000)
                    ConnectChat()
                    Exit Function
                End If
                If InputMessage = "PING :tmi.twitch.tv" Then SendIrcMessage("PONG :tmi.twitch.tv")
                If InputMessage <> "" Then ReadChat(InputMessage)
                If InputMessage = ":" & BotName & "!" & BotName & "@" & BotName & ".tmi.twitch.tv PART #" & Broadcastername Then _
            ChatActive = False
            Loop
            Close()
            RaiseEvent IRCdisconnected()
        End Function

        Public Sub DisconnectChat()
            If ChatActive = True Then
                ChannelPart = True
            End If
        End Sub

        Public Sub SendChat(ChatMessage As String)
            SendBuffer.Enqueue(ChatMessage)
        End Sub

        Public Sub ConnectChat()
            Try
                VarTcpClient = New TcpClient(MyIPaddress, MyPort)
                Inputstream = New StreamReader(VarTcpClient.GetStream())
                Outputstream = New StreamWriter(VarTcpClient.GetStream())

                Outputstream.WriteLine("PASS " & MyPassword)
                Outputstream.WriteLine("NICK " & UserName)
                Outputstream.WriteLine("USER " & UserName & " 8 * :" & UserName)
                Outputstream.WriteLine("JOIN #" & Channel)
                Outputstream.Flush()
            Catch ex As Exception
                MsgBox(ex.Message)
                Exit Sub
            End Try
            ChannelPart = False
            ChatActive = True
            RaiseEvent IRCconnected()
            Dim Reader As Task = ChatReader()
            Dim Speaker As Task = ChatSpeaker()
        End Sub

        Private Sub ReadChat(InputString As String)

            Dim UserName As String, ChatText As String = "", Index As Integer, Findstring As String = "PRIVMSG #jerinhaus :"
            UserName = DataExtract(InputString, ":", "!")

            Index = InStr(InputString, Findstring)

            If Index <> 0 Then
                Index = Index + Len(Findstring)
                ChatText = Mid(InputString, Index, Len(InputString) - Index + 1)
            End If
            If UserName <> "" And ChatText <> "" Then
                If UserName = "nightbot" Then
                    Select Case ChatText
                        Case = "We have llamas: https://www.deviantart.com/drawingwithjerin"
                        Case = "Beware of doggos: https://discord.gg/nG7PTTY"
                        Case = "ok boomer: https://www.facebook.com/DrawingWithJerin"
                        Case = "Come see our pretty pictures ^_^: https://www.instagram.com/drawingwithjerin/"
                        Case = "All the things!!! Website(www.drawingwithjerin.com),  Store(https://drawingwithjerin.com/jerinhaus/shop/), Youtube(https://www.youtube.com/channel/UCrDFqdGFty2YkM57fSE0VOg), Insta(https://www.instagram.com/jerinhaus/), Twitter(https://twitter.com/Jerinhaus), DA(https://www.deviantart.com/drawingwithjerin),  FB(https://www.facebook.com/Jerinhaus/)"
                        Case = "Have fun storming the castle: https://twitter.com/JerinDrawing"
                        Case = "And this is a website! It's dot com: https://drawingwithjerin.com/"
                        Case = "*In Bro Speak: ""Please subscribe To my youtube channel"" https://www.youtube.com/channel/UCrDFqdGFty2YkM57fSE0VOg"
                        Case Else
                            'EventOutputBuffer.Enqueue(ChatText)
                            'Call UpdateEventBox(ChatText)
                            RaiseEvent IRCeventRecieved(ChatText)
                    End Select
                Else
                    If ChatText.Substring(0, 1) <> "!" Then
                        ChatUserInfo.RecieveChatMessage(UserName, ChatText, DateAndTime.Now.ToString)
                        'ChatOutputBuffer.Enqueue(UserName & ": " & ChatText)
                        'Call UpdateChatBox()
                    Else

                        Dim Splitstring() As String = ChatText.Split(" ")
                        Splitstring(0) = Splitstring(0).Replace("!", "")
                        Select Case Splitstring(0)

                            '[][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][]
                            '[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[CHAT COMMANDS]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]
                            '[][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][]

                            Case = "counts"
                                SendBuffer.Enqueue("@" & UserName & " " & CounterData.ReadPublicCounters)

                            Case = "count"
                                If Splitstring.Length > 1 Then
                                    If MainWindow.OBSconnected Then
                                        If CounterData.UserCount(Splitstring(1)) = True Then
                                            SendBuffer.Enqueue("@" & UserName & " Counted " & Splitstring(1))
                                        End If
                                    End If
                                End If

                            Case = "soundlist"
                                AudioControl.UpdatePublicSoundFileIndex()
                                SendBuffer.Enqueue("@" & UserName & " " & AudioControl.PublicSoundListLink)

                        End Select
                    End If
                End If
                RaiseEvent IRCmessageRecieved(UserName & ": " & ChatText)
                'BeginInvoke(Sub() DatastreamBox.Text = UserName & ": " & ChatText & vbCrLf & DatastreamBox.Text)
            Else
                RaiseEvent IRCmessageRecieved(InputString)
                'BeginInvoke(Sub() DatastreamBox.Text = InputString & vbCrLf & DatastreamBox.Text)
            End If

        End Sub

        Private Sub SendIrcMessage(InputString As String)
            Try
                Outputstream.WriteLine(InputString)
                Outputstream.Flush()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End Sub

        Private Sub SendPublicChatMessage(InputString As String)
            Try
                SendIrcMessage(":" & UserName & "!" & UserName & "@" + UserName +
                    ".tmi.twitch.tv PRIVMSG #" & Channel & " :" + InputString)
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End Sub

        Private Async Function ReadMessage() As Task(Of String)
            Dim Outputstring As String = "" ', IntWat As Integer
            Try
                Outputstring = Await Inputstream.ReadLineAsync
                Return Outputstring
            Catch ex As Exception
                Return "Error recieving message: " & ex.Message
            End Try
        End Function

        Private Sub Close()
            Outputstream.Close()
            Outputstream.Dispose()

            Inputstream.Close()
            Inputstream.Dispose()

            VarTcpClient.Close()
            VarTcpClient.Dispose()
        End Sub
    End Class

    Public ChatLogDirectory As String = "\\StreamPC-V2\OBS Assets\Chat Log"

    Public Class UserData

        Public ChatColor() As Color
        Public AllChatUsers As List(Of ChatUser)
        Public CurrentChatUsers As List(Of String)
        Public CurrentAccess As Mutex
        Public LogAccess As Mutex
        Public Event ChatUserDetected(UserName As String, NewUser As Boolean)
        Public Event MessageRecieved(UserName As String, MessageData As String, TimeString As String, ColorIndex As Integer)
        Public Colorizer As Integer

        Public Function GetColorIndex() As Integer
            If Colorizer > 19 Then
                Colorizer = 0
            Else
                Colorizer = Colorizer + 1
            End If
            Return Colorizer
            File.WriteAllText(ChatLogDirectory & "\" & "Color Index.txt", Colorizer)
        End Function

        Public Sub New()
            Dim NewChatColors(0 To 19) As Color
            InitializeColorizer()
            CurrentAccess = New Mutex
            LogAccess = New Mutex
            AllChatUsers = New List(Of ChatUser)
            CurrentChatUsers = New List(Of String)
            ImportAllUsers()
            ImportCurrentUsers()
            NewChatColors(0) = Color.FromArgb(255, 245, 252, 200)
            NewChatColors(1) = Color.FromArgb(255, 65, 172, 0)
            NewChatColors(2) = Color.FromArgb(255, 248, 135, 11)
            NewChatColors(3) = Color.FromArgb(255, 168, 164, 85)
            NewChatColors(4) = Color.FromArgb(255, 248, 94, 11)
            NewChatColors(5) = Color.FromArgb(255, 255, 176, 0)
            NewChatColors(6) = Color.FromArgb(255, 255, 25, 0)
            NewChatColors(7) = Color.FromArgb(255, 180, 220, 17)
            NewChatColors(8) = Color.FromArgb(255, 66, 113, 38)
            NewChatColors(9) = Color.FromArgb(255, 126, 182, 92)
            NewChatColors(10) = Color.FromArgb(255, 176, 111, 39)
            NewChatColors(11) = Color.FromArgb(255, 255, 0, 62)
            NewChatColors(12) = Color.FromArgb(255, 65, 175, 79)
            NewChatColors(13) = Color.FromArgb(255, 150, 74, 48)
            NewChatColors(14) = Color.FromArgb(255, 255, 229, 0)
            NewChatColors(15) = Color.FromArgb(255, 231, 119, 146)
            NewChatColors(16) = Color.FromArgb(255, 223, 237, 126)
            NewChatColors(17) = Color.FromArgb(255, 46, 101, 175)
            NewChatColors(18) = Color.FromArgb(255, 142, 20, 0)
            NewChatColors(19) = Color.FromArgb(255, 178, 59, 190)
            ChatColor = NewChatColors
        End Sub

        Private Sub InitializeColorizer()
            If File.Exists(ChatLogDirectory & "\" & "Color Index.txt") Then
                Dim InputString As String = File.ReadAllText(ChatLogDirectory & "\" & "Color Index.txt")
                If IsNumeric(InputString) Then
                    Colorizer = InputString
                Else
                    Colorizer = 0
                End If
            Else
                Colorizer = 0
            End If
        End Sub

        Private Function CheckAllChatUsers(InputUserName As String) As Integer
            For i As Integer = 0 To AllChatUsers.Count - 1
                If AllChatUsers(i).UserName = InputUserName Then
                    Return i
                    Exit Function
                End If
            Next
            Return -1
        End Function

        Private Sub LogChatMessage(Username As String, ChatString As String, TimeString As String)
            LogAccess.WaitOne()
            Dim FileString As String = "\" & DateFileString() & ".txt"
            If File.Exists(ChatLogDirectory & FileString) = False Then File.Create(ChatLogDirectory & FileString).Dispose()
            Dim Writer As StreamWriter = File.AppendText(ChatLogDirectory & FileString)
            Writer.WriteLine(TimeString & ": " & Username & ": " & ChatString)
            Writer.Close()
            Writer.Dispose()
            LogAccess.ReleaseMutex()
        End Sub

        Public Sub RecieveChatMessage(UserName As String, ChatString As String, TimeString As String)
            Dim ChatUserIndex As Integer = CheckAllChatUsers(UserName)
            LogChatMessage(UserName, ChatString, TimeString)
            If ChatUserIndex < 0 Then
                Dim ChatUserData As New ChatUser
                ChatUserData.CreateUserData(UserName, GetColorIndex)
                ChatUserData.AppendChatLog(ChatString, TimeString)
                AllChatUsers.Add(ChatUserData)
                ChatUserIndex = AllChatUsers.Count - 1
                AppendCurrentUsers(UserName)
                RaiseEvent ChatUserDetected(UserName, True)
            Else
                AllChatUsers(ChatUserIndex).AppendChatLog(ChatString, TimeString)
                If CurrentChatUsers.Contains(UserName) = False Then
                    AppendCurrentUsers(UserName)
                    RaiseEvent ChatUserDetected(UserName, False)
                End If
            End If
            RaiseEvent MessageRecieved(UserName, ChatString, TimeString, AllChatUsers(ChatUserIndex).ColorIndex)
        End Sub

        Public Sub ResetCurrentUsers()
            CurrentAccess.WaitOne()
            CurrentChatUsers = New List(Of String)
            File.Create(ChatLogDirectory & "\Current Chat Users.txt").Dispose()
            CurrentAccess.ReleaseMutex()
        End Sub

        Public Sub AppendCurrentUsers(UserName As String)
            CurrentAccess.WaitOne()
            If File.Exists(ChatLogDirectory & "\Current Chat Users.txt") = False Then
                File.Create(ChatLogDirectory & "\Current Chat Users.txt").Dispose()
            End If
            Dim Writer As StreamWriter = File.AppendText(ChatLogDirectory & "\Current Chat Users.txt")
            Writer.WriteLine(UserName)
            Writer.Close()
            Writer.Dispose()
            CurrentChatUsers.Add(UserName)
            CurrentAccess.ReleaseMutex()
        End Sub

        Public Sub ImportCurrentUsers()
            CurrentAccess.WaitOne()
            If File.Exists(ChatLogDirectory & "\Current Chat Users.txt") Then
                Dim Reader As StreamReader = File.OpenText(ChatLogDirectory & "\Current Chat Users.txt")
                Do Until Reader.EndOfStream = True
                    CurrentChatUsers.Add(Reader.ReadLine)
                Loop
            End If
            CurrentAccess.ReleaseMutex()
        End Sub

        Public Sub ImportAllUsers()
            Dim ReplaceSearch As String = ChatLogDirectory & "\"
            Dim NameString As String = ""
            Dim ChatUserData As New ChatUser
            For Each Dir As String In Directory.GetDirectories(ChatLogDirectory)
                NameString = Replace(Dir, ReplaceSearch, "")
                ChatUserData.ImportUserData(NameString)
                AllChatUsers.Add(ChatUserData)
            Next
        End Sub

    End Class

    Public Structure ChatUser

        Public ColorIndex As Integer
        Public UserName As String
        'Public UserID As String
        Public Access As Mutex


        Public Sub AddJHpoints(PointValue As String)
            Dim PointsValue As Double = GetJHpoints()
            PointsValue = PointsValue + PointValue
            File.WriteAllText(ChatLogDirectory & "\" & UserName & "\JHpoints.txt", PointsValue.ToString)
        End Sub

        Public Function GetJHpoints() As Double
            If Access Is Nothing Then Access = New Mutex
            Access.WaitOne()
            Dim PointsValue As Double, InputString As String
            InputString = File.ReadAllText(ChatLogDirectory & "\" & UserName & "\JHpoints.txt")
            If IsNumeric(InputString) Then
                PointsValue = InputString
            Else
                PointsValue = 0
            End If
            Access.ReleaseMutex()
            Return PointsValue
        End Function

        Public Sub CreateUserData(InputUserName As String, InputColorIndex As Integer)
            If Access Is Nothing Then Access = New Mutex
            Access.WaitOne()
            UserName = InputUserName
            My.Computer.FileSystem.CreateDirectory(ChatLogDirectory & "\" & UserName)
            ColorIndex = InputColorIndex
            File.WriteAllText(ChatLogDirectory & "\" & UserName & "\ColorIndex.txt", ColorIndex)
            File.WriteAllText(ChatLogDirectory & "\" & UserName & "\JHpoints.txt", 0)
            File.Create(ChatLogDirectory & "\" & UserName & "\ChatLog.txt").Dispose()
            Access.ReleaseMutex()
        End Sub


        Public Sub AppendChatLog(ChatString As String, TimeString As String)
            If Access Is Nothing Then Access = New Mutex
            Access.WaitOne()
            Dim ChatWriter As StreamWriter = File.AppendText(ChatLogDirectory & "\" & UserName & "\ChatLog.txt")
            ChatWriter.WriteLine(TimeString & ": " & ChatString)
            ChatWriter.Close()
            ChatWriter.Dispose()
            Access.ReleaseMutex()
        End Sub

        Public Sub ImportUserData(Optional InputUserName As String = "")
            If Access Is Nothing Then Access = New Mutex
            Access.WaitOne()
            If InputUserName <> "" Then UserName = InputUserName
            Dim ColorString = File.ReadAllText(ChatLogDirectory & "\" & UserName & "\ColorIndex.txt")
            If IsNumeric(ColorString) Then ColorIndex = ColorString
            Access.ReleaseMutex()
        End Sub

    End Structure


End Module
