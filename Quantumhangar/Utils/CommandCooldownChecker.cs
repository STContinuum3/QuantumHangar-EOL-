using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Game.World;
using Torch.Commands;

namespace QuantumHangar.Utils
{
    public static class CommandCooldownChecker
    {
        public static bool FailsAlliancePreChecks(CommandContext Context, out Guid allianceId)
        {
            allianceId = Guid.Empty;

            if (Context.Player == null)
            {
                Context.Respond("This is a player only command!");
                return true;
            }

            // Check if either Alliances or Groups plugin is installed
            if (Hangar.Alliances == null && Hangar.Groups == null)
            {
                Context?.Respond("Neither Alliances nor Groups plugin is installed!");
                return true;
            }

            var faction = MySession.Static.Factions.GetPlayerFaction(Context.Player.IdentityId);
            if (faction == null)
            {
                Context.Respond("Players without a faction cannot use alliance hanger!");
                return true;
            }

            var methodInput = new object[] { faction.Tag };

            // Try Alliances plugin first
            if (Hangar.Alliances != null)
            {
                allianceId = (Guid)(Hangar.GetAllianceId?.Invoke(null, methodInput));
                if (allianceId == null || allianceId == Guid.Empty)
                {
                    Context?.Respond("Players without an alliance cannot use alliance hanger!");
                    return true;
                }
            }
            // If no Alliances plugin, try Groups plugin
            else if (Hangar.Groups != null)
            {
                allianceId = (Guid)(Hangar.GetGroupId?.Invoke(null, methodInput));
                if (allianceId == null || allianceId == Guid.Empty)
                {
                    Context?.Respond("Players without a group cannot use alliance hanger!");
                    return true;
                }
            }

            if (Hangar.AllianceAttempts.TryGetValue(allianceId, out var timer))
            {
                if (DateTime.Now < timer)
                {
                    Context.Respond("Cannot use this for 5 seconds.");
                    return true;
                }

                Hangar.AllianceAttempts[allianceId] = DateTime.Now.AddSeconds(5);
            }
            else
            {
                Hangar.AllianceAttempts[allianceId] = DateTime.Now.AddSeconds(5);
            }

            return false;
        }

        public static bool FailsFactionPreChecks(CommandContext Context)
        {
            if (Context.Player == null)
            {
                Context.Respond("This is a player only command!");
                return true;
            }

            var playersFaction = MySession.Static.Factions.TryGetPlayerFaction(Context.Player.IdentityId);
            if (playersFaction == null)
            {
                Context.Respond("Need a faction to use faction hangar.");
                return true;
            }

            if (Hangar.FactionAttempts.TryGetValue(playersFaction.FactionId, out var timer))
            {
                if (DateTime.Now < timer)
                {
                    Context.Respond("Cannot use this for 5 seconds.");
                    return true;
                }

                Hangar.FactionAttempts[playersFaction.FactionId] = DateTime.Now.AddSeconds(5);
            }
            else
            {
                Hangar.FactionAttempts[playersFaction.FactionId] = DateTime.Now.AddSeconds(5);
            }

            return false;
        }
    }
}
