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
                if (damageLevel != ComponentDamageLevel.Destroyed) return;
                var mountedLocation = __instance.mechComponentRef?.MountedLocation;

                if ((Holder.LeftArmSurvived && mountedLocation == ChassisLocations.LeftArm) ||
                    (Holder.RightArmSurvived && mountedLocation == ChassisLocations.RightArm))
                {
                    damageLevel = ComponentDamageLevel.NonFunctional;
                }
            }
        }
    }
}