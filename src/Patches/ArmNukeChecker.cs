using BattleTech;

namespace SurvivableArms
{
    internal class ArmNukeChecker
    {
        [HarmonyPatch(typeof(MechComponent), "DamageComponent")]
        public static class MechComponent_DamageComponent_Patch
        {
            [HarmonyPrefix]
            public static void Prefix(MechComponent __instance, ref ComponentDamageLevel damageLevel)
            {
                if (__instance.mechComponentRef.MountedLocation == ChassisLocations.LeftArm && Holder.LeftArmSurvived)
                {
                    if (damageLevel == ComponentDamageLevel.Destroyed)
                    {
                        Main.Log.LogDebug($"Left Arm survived!");
                        damageLevel = ComponentDamageLevel.NonFunctional;
                    }
                }

                if (__instance.mechComponentRef.MountedLocation == ChassisLocations.RightArm && Holder.RightArmSurvived)
                {
                    if (damageLevel == ComponentDamageLevel.Destroyed)
                    {
                        Main.Log.LogDebug($"Right Arm survived!");
                        damageLevel = ComponentDamageLevel.NonFunctional;
                    }
                }
            }
        }
    }
}