using BattleTech;

namespace SurvivableArms
{
    class SideTorsoExplosionChecker
    {
        [HarmonyPatch(typeof(Mech), "DamageLocation")]
        public static class Mech_DamageLocation_Patch
        {
            [HarmonyPrefix]
            public static void Prefix(Mech __instance, ArmorLocation aLoc, float totalArmorDamage, float directStructureDamage)
            {
                if (aLoc is ArmorLocation.None or ArmorLocation.Invalid)
                    return;

                ChassisLocations cLoc = MechStructureRules.GetChassisLocationFromArmorLocation(aLoc);
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
                    // Invalidate if the actual arm was destroyed.
                    if (cLoc == ChassisLocations.LeftArm)
                        Holder.LeftArmSurvived = false;
                    else if (cLoc == ChassisLocations.RightArm)
                        Holder.RightArmSurvived = false;

                    ChassisLocations dependentLocation = MechStructureRules.GetDependentLocation(cLoc);
                    if (dependentLocation != ChassisLocations.None && !__instance.IsLocationDestroyed(dependentLocation))
                    {
                        // Side torso was destroyed, no reason the arm should be totally trashed.
                        if (dependentLocation == ChassisLocations.LeftArm)
                            Holder.LeftArmSurvived = true;
                        else if (dependentLocation == ChassisLocations.RightArm)
                            Holder.RightArmSurvived = true;
                    }
                }
            }
        }
    }
}
