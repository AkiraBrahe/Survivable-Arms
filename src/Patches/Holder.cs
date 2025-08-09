using BattleTech;

namespace SurvivableArms
{
    internal class Holder
    {
        public static bool LeftArmSurvived = false;
        public static bool RightArmSurvived = false;

        [HarmonyPatch(typeof(GameInstance), "LaunchContract", [typeof(Contract), typeof(string)])]
        public static class GameInstance_HolderLaunchContract_Patch
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                LeftArmSurvived = false;
                RightArmSurvived = false;
            }
        }
    }
}
