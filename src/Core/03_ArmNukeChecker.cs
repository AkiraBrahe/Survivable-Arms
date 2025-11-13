using BattleTech;

namespace SurvivableArms.Core
{
    internal class ArmNukeChecker
    {
        /// <summary>
        /// Prevents arms from being destroyed if they are marked as surviving side torso destruction.
        /// </summary>
        [HarmonyPatch(typeof(MechComponent), "DamageComponent")]
        public static class MechComponent_DamageComponent
        {
            [HarmonyPrefix]
            public static void Prefix(MechComponent __instance, ref ComponentDamageLevel damageLevel)
            {
                if (damageLevel != ComponentDamageLevel.Destroyed) return;
                if (__instance.parent is not Mech mech) return;
                var state = Holder.GetOrCreateState(mech.GUID);
                var mountedLocation = __instance.mechComponentRef?.MountedLocation;

                // If an arm is being nuked, ensure it survives side torso destruction.
                if ((state.LeftArmSurvived && mountedLocation == ChassisLocations.LeftArm) ||
                    (state.RightArmSurvived && mountedLocation == ChassisLocations.RightArm))
                {
                    damageLevel = ComponentDamageLevel.NonFunctional;
                }
            }
        }
    }
}