using BattleTech;

namespace SurvivableArms.Patches
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

                // If an arm is being nuked, ensure it survived side torso destruction.
                if ((Holder.LeftArmSurvived && mountedLocation == ChassisLocations.LeftArm) ||
                    (Holder.RightArmSurvived && mountedLocation == ChassisLocations.RightArm))
                {
                    damageLevel = ComponentDamageLevel.NonFunctional;
                }
            }
        }
    }
}