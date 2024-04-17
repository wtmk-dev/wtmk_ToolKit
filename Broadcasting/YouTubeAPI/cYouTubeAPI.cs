using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System;
using System.Collections.Generic;
using System.Threading;
using s = System.Threading.Tasks;
using UnityEditor.PackageManager;
using System.Net.Http;
using System.Linq;
using Unity.VisualScripting;


namespace WTMK.Broadcasting.YouTubeAPI
{
    public class TokenSupplier
    {
        public string AccessToken
        {
            get; private set;
        }

        public string ClientId
        {
            get; private set;
        }

        public string ClientSecret
        {
            get; private set;
        }

        public string NextPageToken
        {
            get; set;
        }

        public void Set(string clientID, string clientSecret, string accessToken)
        {
            ClientId = clientID;
            ClientSecret = clientSecret;
            AccessToken = accessToken;
        }

        public async s.Task<string> GetAccessToken(string clientID, string clientSecret)
        {
            var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = clientID,
                    ClientSecret = clientSecret
                },
                new[] { "https://www.googleapis.com/auth/youtube" },
                "user",
                CancellationToken.None);

            return credential.Token.AccessToken;
        }

        public async s.Task<string> GetAccessTokenWithRefreshToken(string clientID, string clientSecret)
        {
            var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = clientID,
                    ClientSecret = clientSecret
                },
                new[] { "https://www.googleapis.com/auth/youtube" },
                "user",
                CancellationToken.None);

            return credential.Token.AccessToken;
        }

        public async s.Task<string> GetAccessTokenFromRefreshToken(string clientID, string clientSecret, string refreshToken)
        {
            var clientSecrets = new ClientSecrets
            {
                ClientId = clientID,
                ClientSecret = clientSecret
            };

            var flow = new GoogleAuthorizationCodeFlow(
                new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = clientSecrets
                });

            var token = new TokenResponse { RefreshToken = refreshToken };

            var credential = new UserCredential(flow, "user", token);

            if (await credential.RefreshTokenAsync(CancellationToken.None))
            {
                return credential.Token.AccessToken;
            }
            else
            {
                return "Failed to refresh access token.";
            }
        }
    }

    public class YouTubeServiceFactory
    {
        public YouTubeService Service
        {
            get
            {
                return _Service;
            }
        }

        public void UpdateAccessToken(string newAccessToken)
        {
            // Create a new YouTubeService instance with the updated access token
            var newService = new YouTubeService(new BaseClientService.Initializer
            {
                HttpClientInitializer = new UserCredential(
                    new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer { }),
                    "user",
                    new TokenResponse { AccessToken = newAccessToken }
                ),
                ApplicationName = _Service.ApplicationName // Keep the same application name
            });

            // Replace the existing YouTubeService instance with the new one
            _Service = newService;
        }

        public static async s.Task<YouTubeServiceFactory> CreateAsync(TokenSupplier tokenSupplier, string clientID, string clientSecret)
        {
            var accessToken = await tokenSupplier.GetAccessTokenWithRefreshToken(clientID, clientSecret);

            var initializer = new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = clientID,
                    ClientSecret = clientSecret
                }
            };

            var flow = new GoogleAuthorizationCodeFlow(initializer);

            var service = new YouTubeService(new BaseClientService.Initializer
            {
                HttpClientInitializer = new UserCredential(flow, "user", new TokenResponse { AccessToken = accessToken }),
                ApplicationName = "After Earth Arcade"
            });

            tokenSupplier.Set(clientID, clientSecret, accessToken);
            return new YouTubeServiceFactory(service);
        }

        public YouTubeServiceFactory(YouTubeService service)
        {
            _Service = service;
        }

        private async void UpdateExpiredService()
        {
            //TO::DO Update serice
        }
        
        private YouTubeService _Service;
    }

    public class ChatMessage
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public DateTime PublishedAt { get; set; }
    }

    public class LiveChatMonitor
    {
        public event Action<List<ChatMessage>> OnMessagesReceived;

        public async s.Task<string> GetLiveChatIdsForChannel()
        {
            var liveBroadcastRequest = _Factory.Service.LiveBroadcasts.List("id,snippt");
            liveBroadcastRequest.BroadcastStatus = LiveBroadcastsResource.ListRequest.BroadcastStatusEnum.Active;
            liveBroadcastRequest.BroadcastType = LiveBroadcastsResource.ListRequest.BroadcastTypeEnum.All;
            liveBroadcastRequest.Mine = true;

            var liveBroadcastResponse = await liveBroadcastRequest.ExecuteAsync();
            var liveChatIds = new List<string>();

            foreach (var liveBroadcast in liveBroadcastResponse.Items)
            {
                liveChatIds.Add(liveBroadcast.Snippet.LiveChatId);
            }

            return liveChatIds[0];
        }

        public async s.Task<List<ChatMessage>> GetNewChatMessages()
        {
            if(_LiveChatId == "")
            {
                _LiveChatId = await GetLiveChatIdsForChannel();
            }

            var request = _Factory.Service.LiveChatMessages.List(_LiveChatId, "snippet");
            request.AccessToken = _TokenSupplier.AccessToken;

            // Set the page token to retrieve the next page of results
            request.PageToken = _TokenSupplier.NextPageToken;

            var response = await request.ExecuteAsync();
            var newMessages = response.Items
                .Select(item => new ChatMessage
                {
                    Id = item.Id,
                    Message = item.Snippet.DisplayMessage,
                    PublishedAt = item.Snippet.PublishedAt ?? DateTime.MinValue
                })
                .Where(message => message.PublishedAt > _LastMessagePublishedAt)
                .ToList();

            // Update the most recent message timestamp for the next request
            if (newMessages.Any())
            {
                _LastMessagePublishedAt = newMessages.Max(message => message.PublishedAt);
            }

            _TokenSupplier.NextPageToken = response.NextPageToken;

            OnMessagesReceived?.Invoke(newMessages);

            return newMessages;
        }

        public LiveChatMonitor(YouTubeServiceFactory factory, TokenSupplier supplier)
        {
            _Factory = factory;
            _TokenSupplier = supplier;
        }

        private DateTime _LastMessagePublishedAt = DateTime.MinValue;
        private readonly YouTubeServiceFactory _Factory;
        private readonly TokenSupplier _TokenSupplier;
        private string _LiveChatId = "";
    }

}
