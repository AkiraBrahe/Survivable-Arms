using BattleTech;
using System.Collections.Generic;

namespace SurvivableArms.Core
{
    internal class ArmSurvivalState
    {
        public bool LeftArmSurvived;
        public bool RightArmSurvived;
    }

    internal class Holder
    {
        /// <summary>
        /// Tracks the arm survival states for each mech by their unique ID.
        /// </summary>
        public static Dictionary<string, ArmSurvivalState> MechArmStates = [];

        /// <summary>
        /// Resets all tracked mech arm survival states.
        /// </summary>
        public static void Reset() => MechArmStates.Clear();

        /// <summary>
        /// Gets or creates the arm survival state for a given mech ID.
        /// </summary>
        public static ArmSurvivalState GetOrCreateState(string mechId)
        {
            if (!MechArmStates.TryGetValue(mechId, out var state))
            {
                state = new ArmSurvivalState();
                MechArmStates[mechId] = state;
            }
            return state;
        }

        /// <summary>
        /// Resets the arm survival states when launching a new contract.
        /// </summary>
        [HarmonyPatch(typeof(GameInstance), "LaunchContract", [typeof(Contract), typeof(string)])]
        public static class GameInstance_HolderLaunchContract
        {
            [HarmonyPostfix]
            public static void Postfix() => Reset();
        }
    }
}