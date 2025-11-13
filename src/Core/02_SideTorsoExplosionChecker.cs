using BattleTech;

namespace SurvivableArms.Core
{
    internal class SideTorsoExplosionChecker
    {
        /// <summary>
        /// Tracks damage to determine if arms survive side torso destruction.
        /// </summary>
        [HarmonyPatch(typeof(Mech), "DamageLocation")]
        public static class Mech_DamageLocation
        {
            [HarmonyPrefix]
            public static void Prefix(Mech __instance, ArmorLocation aLoc, float totalArmorDamage, float directStructureDamage)
            {
                if (aLoc is ArmorLocation.None or ArmorLocation.Invalid)
                    return;

                var cLoc = MechStructureRules.GetChassisLocationFromArmorLocation(aLoc);
                float currentStructure = __instance.GetCurrentStructure(cLoc);

                // If location is already destroyed, nothing to do.
                if (currentStructure <= 0f)
                    return;

                float currentArmor = __instance.GetCurrentArmor(aLoc);
                float damageSpillover = System.Math.Max(0f, totalArmorDamage - currentArmor);
                float effectiveDamage = damageSpillover + directStructureDamage;

                // If the damage is enough to destroy the location, we need to check if the arms survived.
                if (effectiveDamage > 0f && currentStructure <= effectiveDamage)
                {
                    var state = Holder.GetOrCreateState(__instance.GUID);

                    // Invalidate if the actual arm was destroyed.
                    if (cLoc == ChassisLocations.LeftArm)
                        state.LeftArmSurvived = false;
                    else if (cLoc == ChassisLocations.RightArm)
                        state.RightArmSurvived = false;

                    var dependentLocation = MechStructureRules.GetDependentLocation(cLoc);
                    if (dependentLocation != ChassisLocations.None && !__instance.IsLocationDestroyed(dependentLocation))
                    {
                        // Side torso was destroyed, no reason the arm should be totally trashed.
                        if (dependentLocation == ChassisLocations.LeftArm)
                            state.LeftArmSurvived = true;
                        else if (dependentLocation == ChassisLocations.RightArm)
                            state.RightArmSurvived = true;
                    }
                }
            }
        }
    }
}