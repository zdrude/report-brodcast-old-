using System;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Server;
using ServerHandlers = Exiled.Events.Handlers.Server;

namespace AdminReportBroadcast
{
    public class Plugin : Plugin<Config>
    {
        public override string Name => "AdminReportBroadcast";
        public override string Author => "Rude";
        public override Version Version => new Version(1, 0, 3);
        public override Version RequiredExiledVersion => new Version(8, 0, 0);

        public override void OnEnabled()
        {
            ServerHandlers.LocalReporting += OnLocalReporting;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            ServerHandlers.LocalReporting -= OnLocalReporting;
            base.OnDisabled();
        }

        private void OnLocalReporting(LocalReportingEventArgs ev)
        {
            if (ev.Player == null || ev.Target == null)
                return;

            string reporterName = ev.Player.Nickname;
            string reporterId = ev.Player.RawUserId;

            string targetName = ev.Target.Nickname;
            string targetId = ev.Target.RawUserId;

            string reason = ev.Reason;

            string broadcastMessage = $"<color=#FF4444>{reporterName}</color> ({reporterId})\n" +
                                      $"подал жалобу на игрока <color=#FF4444>{targetName}</color>\n" +
                                      $"({targetId}) <color=#FFFF00>По причине:</color> {reason}";

            foreach (Player player in Player.List)
            {
                if (player.RemoteAdminAccess)
                {
                    player.Broadcast(8, broadcastMessage);
                }
            }
        }
    }
}