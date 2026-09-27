using System.Reflection;
using HarmonyLib;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Visual.CharacterSystem;

namespace WOTRMultiplayer.HarmonyPatches.NewGameSequence
{
    // TeleportParty.LoadProcess() runs
    // Game.Instance.Player.PartyCharacters.Select(x => x.Value.View.CharacterAvatar).ToArray()
    // for every party member with no null guard. Our New Campaign companion's view is
    // recreated at runtime (see GameInteractionService.AttachNewGameCompanionToParty) rather
    // than going through vanilla's own companion-creation flow, and can end up with a null
    // View or CharacterAvatar - which throws an NRE here and surfaces as a vanilla error
    // popup on every subsequent TeleportParty. LoadProcess's lambda has no captured state, so
    // the compiler caches it as its own method on TeleportParty's nested <>c closure class -
    // that generated method is what actually appears in the crash stack trace
    // (TeleportParty+<>c.<LoadProcess>b__9_1), and is patchable directly since LoadProcess
    // itself only holds a local delegate reference that Harmony cannot target.
    [HarmonyPatch]
    public static class TeleportPartyLoadProcessPatches
    {
        public static MethodBase TargetMethod()
        {
            var closureType = AccessTools.Inner(typeof(TeleportParty), "<>c");
            return AccessTools.Method(closureType, "<LoadProcess>b__9_1");
        }

        public static bool Prefix(UnitReference x, ref Character __result)
        {
            __result = x.Value?.View?.CharacterAvatar;
            return false;
        }
    }
}
