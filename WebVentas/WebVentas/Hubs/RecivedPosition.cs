using System;
using Microsoft.AspNet.SignalR;
using WebVentas.Utils;

namespace WebVentas.Server.Hubs
{
    public class recived : Hub
    {
        public void SendPosition(LivePositionRequest livePositionRequest)
        {
            livePositionRequest.fecha = DateTime.Now.ToLocalTime();
            Clients.All.MessageReceived(livePositionRequest);
        }
    }
}
