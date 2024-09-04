using Exiled.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Server;
using Exiled.Loader;
using MEC;
using PlayerRoles;
using Respawning;
using System;
using System.Collections.Generic;

namespace RRR
{
     internal sealed class EventHandlers
     {
          private int Respawns = 0;
          private int RRRRespawns = 0;
          private CoroutineHandle calcuationCoroutine;

          public void OnRoundStarted()
          {
               RRR.Instance.IsSpawnable = false;
               RRR.Instance.NextIsForced = false;
               Respawns = 0;
               RRRRespawns = 0;

               if (calcuationCoroutine.IsRunning)
                    Timing.KillCoroutines(calcuationCoroutine);

               calcuationCoroutine = Timing.RunCoroutine(spawnCalculation());
          }

          private IEnumerator<float> spawnCalculation()
          {
               while (true)
               {
                    yield return Timing.WaitForSeconds(1f);

                    if (Round.IsEnded)
                         break;

                    if (Math.Round(Respawn.TimeUntilSpawnWave.TotalSeconds, 0) != RRR.Instance.Config.SpawnWaveCalculation)
                         continue;

                    if (Respawn.NextKnownTeam == SpawnableTeamType.NineTailedFox)
                         RRR.Instance.IsSpawnable = (Loader.Random.Next(100) <= RRR.Instance.Config.SpawnManager.Probability &&
                             Respawns >= RRR.Instance.Config.SpawnManager.Respawns &&
                             RRRRespawns < RRR.Instance.Config.SpawnManager.MaxSpawns) || RRR.Instance.NextIsForced;
               }
          }

          public void OnRespawningTeam(RespawningTeamEventArgs ev)
          {
               if (RRR.Instance.IsSpawnable || RRR.Instance.NextIsForced)
               {
                    List<Player> players = new List<Player>();
                    if (ev.Players.Count > RRR.Instance.Config.SpawnManager.MaxSquad)
                         players = ev.Players.GetRange(0, RRR.Instance.Config.SpawnManager.MaxSquad);
                    else
                         players = ev.Players.GetRange(0, ev.Players.Count);

                    Queue<RoleTypeId> queue = ev.SpawnQueue;
                    foreach (RoleTypeId role in queue)
                    {
                         if (players.Count <= 0)
                              break;
                         Player player = players.RandomItem();
                         players.Remove(player);
                         switch (role)
                         {
                              case RoleTypeId.NtfCaptain:
                                   RRR.Instance.Config.RRRLeader.AddRole(player);
                                   break;
                              case RoleTypeId.NtfSergeant:
                                   RRR.Instance.Config.RRRSergeant.AddRole(player);
                                   break;
                              case RoleTypeId.NtfPrivate:
                                   RRR.Instance.Config.RRRPrivate.AddRole(player);
                                   break;
                            case RoleTypeId.NtfSpecialist:
                                   RRR.Instance.Config.Alpha_1AssistantCaptian.AddRole(player);
                                   break;
                         }
                    }
                    RRRRespawns++;
                    
                    ev.NextKnownTeam = SpawnableTeamType.None;
               }
               Respawns++;
          }

          public void OnAnnouncingNtfEntrance(AnnouncingNtfEntranceEventArgs ev)
          {
               string cassieMessage = string.Empty;
               string cassieText = string.Empty;
               if (RRR.Instance.IsSpawnable || RRR.Instance.NextIsForced)
               {
                    if (ev.ScpsLeft == 0 && !string.IsNullOrEmpty(RRR.Instance.Config.SpawnManager.RRRAnnouncmentCassieNoScp))
                    {
                         ev.IsAllowed = false;
                         cassieMessage = RRR.Instance.Config.SpawnManager.RRRAnnouncmentCassieNoScp;
                         cassieText = RRR.Instance.Config.SpawnManager.CassieTextUiuNoSCPs;
                    }
                    else if (ev.ScpsLeft >= 1 && !string.IsNullOrEmpty(RRR.Instance.Config.SpawnManager.RRRAnnouncementCassie))
                    {
                         ev.IsAllowed = false;
                         cassieMessage = RRR.Instance.Config.SpawnManager.RRRAnnouncementCassie;
                         cassieText = RRR.Instance.Config.SpawnManager.CassieTextUiuSCPs;
                    }
                    RRR.Instance.NextIsForced = false;
                    RRR.Instance.IsSpawnable = false;
               }
               else
               {
                    if (ev.ScpsLeft == 0 && !string.IsNullOrEmpty(RRR.Instance.Config.SpawnManager.NtfAnnouncmentCassieNoScp))
                    {
                         ev.IsAllowed = false;
                         cassieMessage = RRR.Instance.Config.SpawnManager.NtfAnnouncmentCassieNoScp;
                         cassieText = RRR.Instance.Config.SpawnManager.CassieTextMtfNoSCPs;
                    }
                    else if (ev.ScpsLeft >= 1 && !string.IsNullOrEmpty(RRR.Instance.Config.SpawnManager.NtfAnnouncementCassie))
                    {
                         ev.IsAllowed = false;
                         cassieMessage = RRR.Instance.Config.SpawnManager.NtfAnnouncementCassie;
                         cassieText = RRR.Instance.Config.SpawnManager.CassieTextMtfSCPs;
                    }
               }

               cassieMessage = cassieMessage.Replace("{scpnum}", $"{ev.ScpsLeft} scpsubject");
               cassieText = cassieText.Replace("{scpnum}", $"{ev.ScpsLeft} SCP subject");

            if (ev.ScpsLeft > 1)
            {
                cassieMessage = cassieMessage.Replace("scpsubject", "scpsubjects");
                cassieText = cassieText.Replace("SCP subject", "SCP subjects");
            }
               cassieMessage = cassieMessage.Replace("{designation}", $"nato_{ev.UnitName[0]} {ev.UnitNumber}");
               cassieText = cassieText.Replace("{designation}", GetNatoName(ev.UnitName) + " " + ev.UnitNumber);

               if (!string.IsNullOrEmpty(cassieMessage))
                    Cassie.MessageTranslated(cassieMessage, cassieText, isSubtitles: RRR.Instance.Config.SpawnManager.Subtitles);
          }
        public string GetNatoName(string unitName)
        {
            Dictionary<string, string> natoAlphabet = new Dictionary<string, string>()
            {
                {"a", "ALPHA"},
                {"b", "BRAVO"},
                {"c", "CHARLIE"},
                {"d", "DELTA"},
                {"e", "ECHO"},
                {"f", "FOXTROT"},
                {"g", "GOLF"},
                {"h", "HOTEL"},
                {"i", "INDIA"},
                {"j", "JULIET"},
                {"k", "KILO"},
                {"l", "LIMA"},
                {"m", "MIKE"},
                {"n", "NOVEMBER"},
                {"o", "OSCAR"},
                {"p", "PAPA"},
                {"q", "QUEBEC"},
                {"r", "ROMEO"},
                {"s", "SIERRA"},
                {"t", "TANGO"},
                {"u", "UNIFORM"},
                {"v", "VICTOR"},
                {"w", "WHISKEY"},
                {"x", "XRAY"},
                {"y", "YANKEE"},
                {"z", "ZULU" },
            };

            string firstLetter = unitName[0].ToString().ToLower();

            if (natoAlphabet.ContainsKey(firstLetter))
            {
                return natoAlphabet[firstLetter];
            }
            else
            {
                return $"nato_{firstLetter}";
            }
        }
    }
}
