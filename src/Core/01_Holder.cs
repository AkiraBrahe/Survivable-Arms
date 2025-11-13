using BattleTech;

namespace SurvivableArms.Core
{
    internal class Holder
    {
        public static bool LeftArmSurvived = false;
        public static bool RightArmSurvived = false;

        /// <summary>
        /// Resets arm survival status on contract launch.
        /// </summary>
        [HarmonyPatch(typeof(GameInstance), "LaunchContract", [typeof(Contract), typeof(string)])]
        public static class GameInstance_HolderLaunchContract
        {
            [HarmonyPostfix]
            public static void Postfix() => Reset();

        }

        public static void Reset()
        {
            LeftArmSurvived = false;
            RightArmSurvived = false;
        }
    }
}