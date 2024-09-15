using System;
using Exiled.API.Features;
using Exiled.CustomRoles.API;
using Exiled.CustomRoles.API.Features;

using MapEvent = Exiled.Events.Handlers.Map;
using ServerEvent = Exiled.Events.Handlers.Server;

namespace RRR
{
     public class RRR : Plugin<Configs.Config>
     {

          public override string Name { get; } = "Alpha-1 \"Red Right Hand\"";
          public override string Author { get; } = "Jospeh_fallen";
          public override string Prefix { get; } = "Alpha-1 \"Red Right Hand\"";
          public override Version Version { get; } = new Version(5, 3, 2);
          public override Version RequiredExiledVersion => new Version(8, 11, 0);
          
          public static RRR Instance;

          public bool IsSpawnable = false;
          public bool NextIsForced = false;

          private EventHandlers eventHandlers;

          public override void OnEnabled()
          {
               Instance = this;
               Config.RRRPrivate.Register();
               Config.RRRSergeant.Register();
               Config.RRRLeader.Register();
               Config.RRRAssistantCaptain.Register();

               eventHandlers = new();

               ServerEvent.RoundStarted += eventHandlers.OnRoundStarted;
               ServerEvent.RespawningTeam += eventHandlers.OnRespawningTeam;
               MapEvent.AnnouncingNtfEntrance += eventHandlers.OnAnnouncingNtfEntrance;

               base.OnEnabled();
          }

          public override void OnDisabled()
          {
            Config.RRRPrivate.Unregister();
            Config.RRRSergeant.Unregister();
            Config.RRRLeader.Unregister();
            Config.RRRAssistantCaptain.Unregister();

            ServerEvent.RoundStarted -= eventHandlers.OnRoundStarted;
               ServerEvent.RespawningTeam -= eventHandlers.OnRespawningTeam;
               MapEvent.AnnouncingNtfEntrance -= eventHandlers.OnAnnouncingNtfEntrance;

               eventHandlers = null;
               Instance = null!;
               base.OnDisabled();
          }
     }
}
