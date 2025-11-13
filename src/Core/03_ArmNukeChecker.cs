using BattleTech;

namespace SurvivableArms.Core
{
    internal class ArmNukeChecker
    {
        /// <summary>
        /// Prevents arms from being nuked from side torso destruction.
        /// </summary>
        [HarmonyPatch(typeof(MechComponent), "DamageComponent")]
        public static class MechComponent_DamageComponent
        {
            [HarmonyPrefix]
            public static void Prefix(MechComponent __instance, ref ComponentDamageLevel damageLevel)
            {
                if (damageLevel != ComponentDamageLevel.Destroyed) return;
                var mountedLocation = __instance.mechComponentRef?.MountedLocation;

                // If an arm is being nuked, ensure it survives side torso destruction.
                if ((Holder.LeftArmSurvived && mountedLocation == ChassisLocations.LeftArm) ||
                    (Holder.RightArmSurvived && mountedLocation == ChassisLocations.RightArm))
                {
                    damageLevel = ComponentDamageLevel.NonFunctional;
                }
            }
        }
    }
}