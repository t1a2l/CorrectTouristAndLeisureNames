using ColossalFramework.Math;
using HarmonyLib;
using System;


namespace CorrectTouristAndLeisureNames
{
    [HarmonyPatch(typeof(CommercialBuildingAI), "GenerateName")]
    public static class GenerateNamesPatch
    {
        public static string[] hotel_names = [
            "Hotel Hiya", "Grand Hotel", "Night Inn", "Fiesta Hotel", "Your Choice Hotel", 
            "Hotel Intercontinental", "Crest Resorts", "Best Eastern", "Stratus Hotel", 
            "Almost Five Star Hotel", "Octahotel", "Hotel Beacon", 
        ];

        public static bool Prefix(CommercialBuildingAI __instance, ushort buildingID, InstanceID caller, ref string __result)
        {
            if (__instance.m_info.m_prefabDataIndex != -1)
			{
				if (__instance.m_info.m_class.m_subService == ItemClass.SubService.CommercialTourist)
                {
                    Random random = new();
					Randomizer randomizer = new Randomizer(buildingID);
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
                    if(Settings.UseDefaultRandomHotelNames.value == true)
                    {
                        int index = random.Next(hotel_names.Length);
                        __result = hotel_names[index];
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
