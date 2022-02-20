Imports TwitchLib.Api
Imports System.IO
Imports System.Threading
Imports System.Collections.Concurrent
Imports System.Net
Imports System.Collections.Generic
Imports System.Linq


Public Module TwitchAPIfunctions

    Public Class APIfunctions
        Public Authorized As Boolean
        Public AccessToken As String
        Public RefreshToken As String
        Public AuthDirectory As String = "\\StreamPC-V2\OBS Assets\Auth"
        Private CodeString As String
        Private ScopesList As List(Of Core.Enums.AuthScopes)
        Public api As TwitchAPI
        Public APIqueue As List(Of String)
        Public Event TokenAcquired(TokenMessage As String)
        Public Event AuthorizationInitialized()
        Public Event StreamInfoAvailable(StreamInfoString)
        Public Event StreamInfoUpdated()

        Public Sub New()
            AddHandler TokenAcquired, AddressOf AuthorizationAcquired
            Authorized = False
            api = New TwitchAPI()
            api.Settings.ClientId = JHclientID
            api.Settings.Secret = JHsecret
            ScopesList = New List(Of Core.Enums.AuthScopes)
            ScopesList.Add(Core.Enums.AuthScopes.Helix_Channel_Read_Redemptions)
            ScopesList.Add(Core.Enums.AuthScopes.Helix_Channel_Manage_Redemptions)
            ScopesList.Add(Core.Enums.AuthScopes.Helix_Bits_Read)
            ScopesList.Add(Core.Enums.AuthScopes.Helix_Channel_Manage_Broadcast)
            ScopesList.Add(Core.Enums.AuthScopes.Helix_Channel_Read_Subscriptions)
            api.Settings.Scopes = ScopesList
        End Sub

        Private Sub AuthorizationAcquired(TokenMessage As String)
            If Authorized = False Then
                Authorized = True
                'SendMessage(TokenMessage, "Authorization Successful")
                RaiseEvent AuthorizationInitialized()
            End If
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

        Public Sub APIauthorization(Optional ForceRefresh As Boolean = False, Optional ForceReset As Boolean = False)
            If ForceReset = False Then
                If ForceRefresh = False Then
                    If AccessToken = "" Then
                        ReadAccessToken()
                    End If
                    If AccessToken = "" Then
                        GetAMotherFukkenToken()
                    Else
                        Dim CheckToken As Task = New Task(AddressOf CheckTheFukkenToken)
                        CheckToken.Start()
                    End If
                Else
                    Dim FreshToken As Task = New Task(AddressOf RefreshTheFukkenToken)
                    FreshToken.Start()
                End If
            Else
                GetAMotherFukkenToken()
            End If
        End Sub

        'Public Function CheckScopes(InputScopesList As List(Of String)) As Boolean
        'For i As Integer = 0 To InputScopesList.Count
        ''Core.Enums.AuthScopes.


        'Next
        'End Function

        Public Sub GetStreamInfoSub()
            If Authorized = True Then
                Dim TaskMaster As New Task(AddressOf GetStreamInfo)
                TaskMaster.Start()
            End If
        End Sub
        Public Sub SetStreamInfoSub()
            If Authorized = True Then
                Dim TaskMaster As New Task(AddressOf SetStreamInfo)
                TaskMaster.Start()
            End If
        End Sub

        Private Async Sub SetStreamInfo()
            Dim InfoRequest As New Helix.Models.Channels.ModifyChannelInformation.ModifyChannelInformationRequest
            InfoRequest.BroadcasterLanguage = GamesList.StreamLanguage
            InfoRequest.GameId = GamesList.StreamGameID
            InfoRequest.Title = GamesList.StreamTitle
            Await api.Helix.Channels.ModifyChannelInformationAsync(JHID, InfoRequest)
            If GamesList.StreamGameIndex > -1 Then
                Dim TagList As List(Of String) = GamesList.GetTagsList
                'Dim TagData As Helix.Models.Streams.GetStreamTags.GetStreamTagsResponse = Await api.Helix.Streams.GetStreamTagsAsync(JHID)
                'For I As Integer = 0 To TagData.Data.Length - 1
                'If TagData.Data(I).IsAuto Then
                'TagList.Add(TagData.Data(I).TagId)
                'End If
                'Next
                If TagList.Count < 6 Then
                    Await api.Helix.Streams.ReplaceStreamTagsAsync(JHID, TagList, AccessToken)
                End If
            End If
            RaiseEvent StreamInfoUpdated()
        End Sub

        Private Async Sub GetStreamInfo()
            Dim InputData As Helix.Models.Channels.GetChannelInformation.GetChannelInformationResponse = Await api.Helix.Channels.GetChannelInformationAsync(JHID)
            Dim ChannelData As Helix.Models.Channels.GetChannelInformation.ChannelInformation = InputData.Data(0)
            GamesList.StreamLanguage = ChannelData.BroadcasterLanguage
            GamesList.SetGameID(ChannelData.GameId)
            Dim GameName As String = ChannelData.GameName
            Dim Delay As Integer = ChannelData.Delay
            GamesList.StreamTitle = ChannelData.Title
            Dim OUTPUTSTRING As String = "STREAM INFO" & vbCrLf
            OUTPUTSTRING = OUTPUTSTRING & "Title: " & GamesList.StreamTitle & vbCrLf
            OUTPUTSTRING = OUTPUTSTRING & "Language: " & GamesList.StreamLanguage & vbCrLf
            OUTPUTSTRING = OUTPUTSTRING & "GameID: " & GamesList.StreamGameID & vbCrLf
            OUTPUTSTRING = OUTPUTSTRING & "GameName: " & GameName & vbCrLf
            'OUTPUTSTRING = OUTPUTSTRING & "Delay: " & Delay & vbCrLf
            'OUTPUTSTRING = OUTPUTSTRING & TagDataString
            'SendMessage(OUTPUTSTRING)
            RaiseEvent StreamInfoAvailable(OUTPUTSTRING)
        End Sub

        Public Async Sub RefreshTheFukkenToken()
            Dim OutputString As String = ""
            ReadRefreshToken()
            If RefreshToken <> "" Then
                Dim ffff As Auth.RefreshResponse = Await api.Auth.RefreshAuthTokenAsync(RefreshToken, JHsecret)
                If ffff IsNot Nothing Then
                    OutputString = "Refreshed" & vbCrLf
                    OutputString = OutputString & "ExpiresIn: " & ffff.ExpiresIn & vbCrLf
                    OutputString = OutputString & "Scopes: " & vbCrLf
                    For i As Integer = 0 To ffff.Scopes.Count - 1
                        OutputString = OutputString & "-" & ffff.Scopes(i) & vbCrLf
                    Next
                    WriteAccessToken(ffff.AccessToken)
                    WriteRefreshToken(ffff.RefreshToken)
                    api.Settings.AccessToken = AccessToken
                    RaiseEvent TokenAcquired(OutputString)
                Else
                    GetAMotherFukkenToken()
                End If
            Else
                GetAMotherFukkenToken()
            End If
        End Sub

        Public Async Sub CheckTheFukkenToken()
            Dim fff As Auth.ValidateAccessTokenResponse = Await api.Auth.ValidateAccessTokenAsync(AccessToken)
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
                    api.Settings.AccessToken = AccessToken
                    RaiseEvent TokenAcquired(OutputString)
                Else
                    Dim FreshToken As Task = New Task(AddressOf RefreshTheFukkenToken)
                    FreshToken.Start()
                End If
            Else
                Dim FreshToken As Task = New Task(AddressOf RefreshTheFukkenToken)
                FreshToken.Start()
            End If
        End Sub

        Public Async Sub GetAFuckingToken()
            Dim TokenResponse As Auth.AuthCodeResponse = Await api.Auth.GetAccessTokenFromCodeAsync(CodeString, JHsecret, "http://localhost:8080")
            Dim OutputString As String = ""
            OutputString = OutputString & "ExpiresIn: " & TokenResponse.ExpiresIn & vbCrLf
            OutputString = OutputString & "Scopes: " & vbCrLf
            For i As Integer = 0 To TokenResponse.Scopes.Count - 1
                OutputString = OutputString & "-" & TokenResponse.Scopes(i) & vbCrLf
            Next
            WriteAccessToken(TokenResponse.AccessToken)
            WriteRefreshToken(TokenResponse.RefreshToken)
            api.Settings.AccessToken = AccessToken
            RaiseEvent TokenAcquired(OutputString)
        End Sub

        Public Sub GetAMotherFukkenToken()
            Dim urlstring As String = api.Auth.GetAuthorizationCodeUrl("http://localhost:8080", ScopesList)
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

        'Private Async Function getusersubs() As Task
        'Checks subscription for a specific user and the channel specified.
        'Dim subscription As V5.Models.Subscriptions.Subscription = Await api.V5.Channels.CheckChannelSubscriptionByUserAsync("channel_id", "user_id")
        '   Console.WriteLine("User subed: " + subscription.User.Name.ToString)
        'End Function

        'Private Async Sub Streaming()
        'shows if the channel is streaming or not (true/false)

        'Dim isStreaming As Boolean = Await api.V5.Streams.BroadcasterOnlineAsync(Broadcastername)
        'Dim isStreaming As Boolean = Await api.Helix.ChannelPoints.GetCustomRewardRedemption()
        'If isStreaming = True Then
        '       APIqueue.Add("Streaming")
        'Else
        '       APIqueue.Add("Not Streaming")
        'End If
        'End Sub

        'Private Async Function chanupdate() As Task
        'Update Channel Title/Game
        'not used this yet
        'Await api.V5.Channels.UpdateChannelAsync(JHID, "New stream title", "Stronghold Crusader")
        'End Function


        Private Async Sub CreateCustomReward()
            Dim CustomReward As New Helix.Models.ChannelPoints.CreateCustomReward.CreateCustomRewardsRequest
            CustomReward.Title = "Custom Reward Test"
            CustomReward.Title = "Custom Reward Test"

            Dim response2 As Helix.Models.ChannelPoints.CreateCustomReward.CreateCustomRewardsResponse =
                Await api.Helix.ChannelPoints.CreateCustomRewards(JHID, CustomReward)
        End Sub


        Public Sub DeleteChannelPointsReward(RewardIndex As Integer)
            If Authorized = True Then
                Dim TaskMaster As Task =
                    New Task(
                    Async Sub()
                        Await api.Helix.ChannelPoints.DeleteCustomReward(JHID, ChannelPoints.Rewards(RewardIndex).TwitchData.Id)
                        GetChannelPointData()
                    End Sub)
                TaskMaster.Start()
            Else
                SendMessage("Not Authorized")
            End If
        End Sub

        Public Sub UpdateChannelPointDataExplicit(RewardIndex As Integer, Request As Helix.Models.ChannelPoints.UpdateCustomReward.UpdateCustomRewardRequest)
            If Authorized = True Then
                Dim TaskMaster As Task =
                    New Task(
                    Async Sub()
                        Dim Response As Helix.Models.ChannelPoints.UpdateCustomReward.UpdateCustomRewardResponse =
                        Await api.Helix.ChannelPoints.UpdateCustomReward(JHID, ChannelPoints.Rewards(RewardIndex).TwitchData.Id, Request)
                        ChannelPoints.Rewards(RewardIndex).ReadResponse(Response)
                        ChannelPoints.RewardUpdated(RewardIndex)
                    End Sub)
                TaskMaster.Start()
            Else
                SendMessage("Not Authorized")
            End If
        End Sub

        Public Sub CreateChannelPointReward(Request As Helix.Models.ChannelPoints.CreateCustomReward.CreateCustomRewardsRequest)
            If Authorized = True Then
                Dim TaskMaster As Task =
                    New Task(
                    Async Sub()
                        Dim Response As Helix.Models.ChannelPoints.CreateCustomReward.CreateCustomRewardsResponse =
                        Await api.Helix.ChannelPoints.CreateCustomRewards(JHID, Request)
                        ChannelPoints.RewardCreated()
                        GetChannelPointData()
                    End Sub)
                TaskMaster.Start()
            Else
                SendMessage("Not Authorized")
            End If
        End Sub

        Public Sub UpdateChannelPointData(RewardIndex As Integer)
            If Authorized = True Then
                Dim TaskMaster As Task =
                    New Task(
                    Sub()
                        UpdateChannelPointReward(RewardIndex)
                    End Sub)
                TaskMaster.Start()
            Else
                SendMessage("Not Authorized")
            End If
        End Sub

        Private Async Sub UpdateChannelPointReward(RewardIndex As Integer)
            Dim Request As Helix.Models.ChannelPoints.UpdateCustomReward.UpdateCustomRewardRequest =
                ChannelPoints.Rewards(RewardIndex).GetUpdateData
            Dim Response As Helix.Models.ChannelPoints.UpdateCustomReward.UpdateCustomRewardResponse =
                 Await api.Helix.ChannelPoints.UpdateCustomReward(JHID, ChannelPoints.Rewards(RewardIndex).TwitchData.Id, Request)
            ChannelPoints.Rewards(RewardIndex).ReadResponse(Response)
            ChannelPoints.RewardUpdated(RewardIndex)
        End Sub

        Public Sub SyncChannelRedemptions()
            If Authorized = True Then
                Dim TaskMaster As New Task(AddressOf GetChannelPointData)
                TaskMaster.Start()
                TaskMaster.Wait()
            Else
                SendMessage("Not Authorized")
            End If
        End Sub

        Private Async Sub GetChannelPointData()
            Dim ChannelPointData As Helix.Models.ChannelPoints.GetCustomReward.GetCustomRewardsResponse = Await api.Helix.ChannelPoints.GetCustomReward(JHID)

            ChannelPoints.InitRewardData(ChannelPointData.Data.Length)
            Dim RewardData As Helix.Models.ChannelPoints.CustomReward
            'Dim Request As Helix.Models.ChannelPoints.UpdateCustomReward.UpdateCustomRewardRequest
            'Dim Response As Helix.Models.ChannelPoints.UpdateCustomReward.UpdateCustomRewardResponse
            'Dim Copy As Helix.Models.ChannelPoints.CreateCustomReward.CreateCustomRewardsRequest
            'Dim response2 As Helix.Models.ChannelPoints.CreateCustomReward.CreateCustomRewardsResponse
            For i As Integer = 0 To ChannelPointData.Data.Length - 1
                RewardData = ChannelPointData.Data.GetValue(i)
                ChannelPoints.Rewards(i).ReadRedemptionDatafromTwitch(RewardData)
                'Request = ChannelPoints.Rewards(i).GetUpdateData
                'Response = Await api.Helix.ChannelPoints.UpdateCustomReward(JHID, ChannelPoints.Rewards(i).TwitchData.Id, Request)
                'ChannelPoints.Rewards(i).ReadResponse(Response)
                'End If
                'Copy = ChannelPoints.Rewards(i).GetCopyRedemptionData
                'response2 = Await api.Helix.ChannelPoints.CreateCustomRewards(JHID, Copy, AccessToken)
            Next

            ChannelPoints.RewardsUpdated()
            'SendMessage("Reward Data Synced")
        End Sub

        Private Async Sub Getchanfollows()
            'Get Specified Channel Follows
            'Dim channelFollowers = Await api.V5.Channels.GetChannelFollowersAsync(Broadcastername)
            Dim channelFollowers = Await api.Helix.Channels.GetChannelInformationAsync(JHID)
            SendMessage(channelFollowers.Data.Length)
            SendMessage(channelFollowers.Data.Rank)

            'SendMessage(channelFollowers.Data.ToString)
            APIqueue.Add(channelFollowers.Data.ToString)
        End Sub

        Private Async Sub getchanuserfollow()
            'Get channels a specified user follows.
            Dim userFollows As V5.Models.Users.UserFollows = Await api.V5.Users.GetUserFollowsAsync(Broadcastername)
            APIqueue.Add(userFollows.Total.ToString)
        End Sub

        Private Async Sub Getnumberofsubs()
            'Get the number of subs to your channel
            Dim numberofsubs = Await api.V5.Channels.GetChannelSubscribersAsync(Broadcastername)
            APIqueue.Add(numberofsubs.Total.ToString)
        End Sub

        Private Async Sub getsubstochannel()
            'Gets a list of all the subscritions of the specified channel.
            Dim allSubscriptions As List(Of V5.Models.Subscriptions.Subscription) = Await api.V5.Channels.GetAllSubscribersAsync(Broadcastername)
            Dim num As Integer
            For num = 0 To allSubscriptions.Count - 1
                APIqueue.Add(allSubscriptions.Item(num).User.Name.ToString)
            Next num
        End Sub
    End Class

    Public RewardDataDirectory As String = "\\StreamPC-V2\OBS Assets\Rewards"

    Public Structure RedemptionData
        Public TwitchData As Helix.Models.ChannelPoints.CustomReward
        Public Type As Integer '0=Global, 1=Ember, 2=Luna
        Public Sound As String
        Public Access As Mutex

        Public Sub InitRewardData()
            If Access Is Nothing Then Access = New Mutex
            Access.WaitOne()
            TwitchData = Nothing
            Type = 0
            Sound = ""
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

            'If Request.IsMaxPerStreamEnabled Is Nothing Then
            'If TwitchData.MaxPerStreamSetting.MaxPerStream <> 0 Then
            'Request.IsMaxPerStreamEnabled = True
            'Request.MaxPerStream = TwitchData.MaxPerStreamSetting.MaxPerStream
            'End If
            'End If

            'If Request.IsMaxPerUserPerStreamEnabled Is Nothing Then
            'If TwitchData.MaxPerUserPerStreamSetting.MaxPerStream <> 0 Then
            'Request.IsMaxPerUserPerStreamEnabled = True
            'Request.MaxPerUserPerStream = TwitchData.MaxPerUserPerStreamSetting.MaxPerStream
            'End If
            'End If

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
            Dim ReturnBool As Boolean = True
            Dim Path As String = RewardDataDirectory & "\" & TwitchData.Id
            Dim InputString As String = ""
            If Directory.Exists(Path) Then
                File.WriteAllText(Path & "\Sound.txt", Sound)
                File.WriteAllText(Path & "\Type.txt", Type)
            End If
            If BypassMutex = False Then Access.ReleaseMutex()
        End Sub

        Public Sub DeleteLocalData(Optional BypassMutex As Boolean = False)
            If Access Is Nothing Then Access = New Mutex
            If BypassMutex = False Then Access.WaitOne()
            Dim Path As String = RewardDataDirectory & "\" & TwitchData.Id
            Dim InputString As String = ""
            If Directory.Exists(Path) Then
                Directory.Delete(Path, True)
            End If
            If BypassMutex = False Then Access.ReleaseMutex()
        End Sub

        Public Sub CheckLocalData(Optional BypassMutex As Boolean = False)
            If Access Is Nothing Then Access = New Mutex
            If BypassMutex = False Then Access.WaitOne()
            Dim Path As String = RewardDataDirectory & "\" & TwitchData.Id
            Dim InputString As String = ""
            If Directory.Exists(Path) Then
                If File.Exists(Path & "\Type.txt") Then
                    InputString = File.ReadAllText(Path & "\Type.txt")
                    If IsNumeric(InputString) Then
                        Type = InputString
                    Else
                        Type = 0
                    End If
                Else
                    Type = 0
                    File.WriteAllText(Path & "\Type.txt", Type)
                End If
                If File.Exists(Path & "\Sound.txt") Then
                    Sound = File.ReadAllText(Path & "\Type.txt")
                Else
                    Sound = ""
                    File.WriteAllText(Path & "\Sound.txt", Sound)
                End If
            Else
                Directory.CreateDirectory(Path)
                Sound = ""
                File.WriteAllText(Path & "\Sound.txt", Sound)
                Type = 0
                File.WriteAllText(Path & "\Type.txt", Type)
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
                outputstring = outputstring & "Max RpStream/Usr(ct): " & TwitchData.MaxPerUserPerStreamSetting.MaxPerStream & vbCrLf
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

    End Structure

    Public Class ChannelPointData

        Public Rewards() As RedemptionData
        Public Event AllRewardsUpdated()
        Public Event SingleRewardUpdated(RewardNum As Integer)
        Public Event SingleRewardRedeemed(RewardNum As Integer)
        Public Event NewRewardCreated()

        Public Sub New()
            Rewards = Nothing
        End Sub

        Public Sub UpdateReward(RewardIndex As Integer, Request As Helix.Models.ChannelPoints.UpdateCustomReward.UpdateCustomRewardRequest)
            myAPI.UpdateChannelPointDataExplicit(RewardIndex, Rewards(RewardIndex).GetUpdateData(, Request))
        End Sub

        Public Sub ToggleRewardEnable(RewardIndex As Integer)
            Dim Request As Helix.Models.ChannelPoints.UpdateCustomReward.UpdateCustomRewardRequest = New Helix.Models.ChannelPoints.UpdateCustomReward.UpdateCustomRewardRequest
            If Rewards(RewardIndex).TwitchData.IsEnabled Then
                Request.IsEnabled = False
            Else
                Request.IsEnabled = True
            End If
            myAPI.UpdateChannelPointDataExplicit(RewardIndex, Rewards(RewardIndex).GetUpdateData(, Request))
        End Sub

        Public Sub RewardCreated()
            myAPI.SyncChannelRedemptions()
        End Sub

        Public Sub RewardsUpdated()
            RaiseEvent AllRewardsUpdated()
        End Sub


        Public Sub RewardUpdated(RewardIndex As Integer)
            RaiseEvent SingleRewardUpdated(RewardIndex)
        End Sub

        Public Sub InitRewardData(RewardCount As Integer)
            ReDim Rewards(0 To RewardCount - 1)
            For i As Integer = 0 To RewardCount - 1
                Rewards(i).InitRewardData()
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

    Public Class TwitchGames
        Public Games() As GameSet
        Public StreamTitle As String
        Public StreamGameIndex As Integer
        Public StreamGameID As Integer
        Public StreamLanguage As String
        Public Const MaxGames As Integer = 6

        Public Sub New()
            ReDim Games(0 To MaxGames - 1)
            Games(0).initART()
            Games(1).initBL3()
            Games(2).initStarBound()
            Games(3).InitMHR()
            Games(4).InitMHW()
            Games(5).InitSoftwareDev()
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


End Module
