using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace AdvanceChat.Client.Services
{
    public interface IMyHubConnectionService
    {
        HubConnection GetHubConnection();
        bool GetConnectionState();
        bool IsConnected { get; set; }
    }
    public class MyHubConnectionService : IMyHubConnectionService
    {
      
        private readonly HubConnection _hubConnection;
        public NavigationManager _navigationManager { get; set; }
        public bool IsConnected { get; set; }
        public MyHubConnectionService(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;

            // Initialize hub connection
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(_navigationManager.ToAbsoluteUri("/chathub"))
                .Build();

            // Start the connection asynchronously without awaiting in constructor
            _ = _hubConnection.StartAsync();

            GetConnectionState();
        }

        public HubConnection GetHubConnection() => _hubConnection;

        public bool GetConnectionState()
        {
            var hubconnection = GetHubConnection();
            IsConnected = hubconnection != null;
            return IsConnected;
        }
    }
}
