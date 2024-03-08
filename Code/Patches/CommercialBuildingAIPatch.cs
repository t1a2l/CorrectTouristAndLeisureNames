using ColossalFramework.Math;
using HarmonyLib;
using CorrectTouristAndLeisureNames.Managers;

namespace CorrectTouristAndLeisureNames.Patches
{
    [HarmonyPatch(typeof(CommercialBuildingAI))]
    public static class CommercialBuildingAIPatch
    {
        [HarmonyPatch(typeof(CommercialBuildingAI), "ReleaseBuilding")]
        [HarmonyPostfix]
        public static void ReleaseBuilding(ushort buildingID, ref Building data)
        {
            HotelNamesManager.RemoveHotelName(buildingID);
        }

        [HarmonyPatch(typeof(CommercialBuildingAI), "GenerateName")]
        [HarmonyPrefix]
        public static bool GenerateNames(CommercialBuildingAI __instance, ushort buildingID, InstanceID caller, ref string __result)
        {
            if (__instance.m_info.m_prefabDataIndex != -1)
			{
				if (__instance.m_info.m_class.m_subService == ItemClass.SubService.CommercialTourist)
                {
					Randomizer randomizer = new(buildingID);
					string key = PrefabCollection<BuildingInfo>.PrefabName((uint)__instance.m_info.m_prefabDataIndex);
                    // has a name already in the locale then use it
                    if (key == "3x4_winter_nightclub_02")
                    {
                        key = "3x4_winter_nightclub_01";
                    }
                    uint num = ColossalFramework.Globalization.Locale.CountUnchecked("BUILDING_NAME", key);
					if (num != 0)
					{
						__result = ColossalFramework.Globalization.Locale.Get("BUILDING_NAME", key, randomizer.Int32(num));
						return false;
					}
                    if(Settings.UseAfterTheDarkDLCHoteNames.value == true)
                    {
                        __result = HotelNamesManager.GetHotelName(buildingID);
                        return false;
                    }
                    else
                    {
                        HotelNamesManager.RemoveHotelName(buildingID);
                    }
                    int periodPos = __instance.m_info.name.IndexOf('.');
                    if (periodPos >= 0)
                    {
                        // Found a period - strip off leading package ID and any trailing _Data and returned the cleaned-up name.
                        __result = __instance.m_info.name.Substring(periodPos + 1).Replace("_Data", string.Empty);
                        return false;
                    }
                    // No period - check for traiing _Data for local assets.
                    else if (__instance.m_info.name.EndsWith("_Data"))
                    {
                        // Trailing _Data found - trim and return the trimmed name.
                        __result = __instance.m_info.name.Substring(0, __instance.m_info.name.Length - 5);
                        return false;
                    }
                    __result = __instance.m_info.name;
					return false;
                }
                else if (__instance.m_info.m_class.m_subService == ItemClass.SubService.CommercialLeisure)
                {
                    Randomizer randomizer = new Randomizer(buildingID);
                    string key = PrefabCollection<BuildingInfo>.PrefabName((uint)__instance.m_info.m_prefabDataIndex);
                    // has a name already in the locale then use it
                    uint num = ColossalFramework.Globalization.Locale.CountUnchecked("BUILDING_NAME", key);
                    if (num != 0)
                    {
                        __result = ColossalFramework.Globalization.Locale.Get("BUILDING_NAME", key, randomizer.Int32(num));
                        return false;
                    }
                    int periodPos = __instance.m_info.name.IndexOf('.');
                    if (periodPos >= 0)
                    {
                        // Found a period - strip off leading package ID and any trailing _Data and returned the cleaned-up name.
                        __result = __instance.m_info.name.Substring(periodPos + 1).Replace("_Data", string.Empty);
                        return false;
                    }
                    // No period - check for traiing _Data for local assets.
                    else if (__instance.m_info.name.EndsWith("_Data"))
                    {
                        // Trailing _Data found - trim and return the trimmed name.
                        __result = __instance.m_info.name.Substring(0, __instance.m_info.name.Length - 5);
                        return false;
                    }
                    __result = __instance.m_info.name;
                    return false;
                }
                return true;
			}
			return true;
        }
    }
}
