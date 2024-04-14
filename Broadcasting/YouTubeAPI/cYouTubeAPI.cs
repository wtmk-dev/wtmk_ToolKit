using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor.PackageManager;
using System.Net.Http;
using System.Linq;

namespace WTMK.Broadcasting.YouTubeAPI
{
    public class YouTubeServiceFactory
    {
        public YouTubeService Service
        {
            get
            {
                return _Service;
            }
        }

        public async Task<string> GetAccessToken(string clientID, string clientSecret)
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

        public async Task<string> GetAccessTokenWithRefreshToken(string clientID, string clientSecret)
        {
            var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            new ClientSecrets
            {
                ClientId = clientID,
                ClientSecret = clientSecret
            },
            new[] { "https://www.googleapis.com/auth/youtube", "offline_access" }, // Add the "offline_access" scope to get refresh
            "user",
            CancellationToken.None);

            return credential.Token.AccessToken;
        }

        public async Task<string> GetAccessTokenFromRefreshToken(string clientID, string clientSecret, string refreshToken)
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

            // Attempt to refresh the token
            if (await credential.RefreshTokenAsync(CancellationToken.None))
            {
                // Access token refreshed successfully, return the new access token
                return credential.Token.AccessToken;
            }
            else
            {
                // Token refresh failed, handle the error
                return "Failed to refresh access token.";
            }
        }

        public YouTubeServiceFactory(string clientID, string clientSecret)
        {
            var token = GetAccessTokenWithRefreshToken(clientID, clientSecret);
            _Service = new YouTubeService(new BaseClientService.Initializer
            {
                HttpClientInitializer = new UserCredential(new GoogleAuthorizationCodeFlow(
                new GoogleAuthorizationCodeFlow.Initializer
                {
                    
                }),
                "user",
                new TokenResponse { AccessToken = token }
                ApplicationName = "YourApplicationName"
            });
        }

        private readonly YouTubeService _Service;
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

        public async Task<List<ChatMessage>> GetNewChatMessages(string liveChatId, string accessToken, string nextPageToken)
        {
            var request = _Service.LiveChatMessages.List(liveChatId, "snippet");
            request.AccessToken = accessToken;

            // Set the page token to retrieve the next page of results
            request.PageToken = nextPageToken;

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

            OnMessagesReceived?.Invoke(newMessages);

            return newMessages;
        }

        public LiveChatMonitor(YouTubeService service)
        {
            _Service = service;
        }

        private DateTime _LastMessagePublishedAt = DateTime.MinValue;
        private readonly YouTubeService _Service;
    }
}
