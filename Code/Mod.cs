using ICities;
using CitiesHarmony.API;
using CorrectTouristAndLeisureNames.Managers;

namespace CorrectTouristAndLeisureNames
{
    public class Mod : IUserMod
    {
        public string Name => "Correct Tourist And Leisure Names";

        public string Description => "Give correct names to the hotles and clubs in the city";

        public void OnEnabled()
        {
            Settings.Init();
            HotelNamesManager.Init();
            HarmonyHelper.DoOnHarmonyReady(() => Patcher.PatchAll());
        }

        public void OnDisabled()
        {
            if (HarmonyHelper.IsHarmonyInstalled)
            {
                Patcher.UnpatchAll();
            }
        }

        public void OnSettingsUI(UIHelperBase helper)
        {
            UIHelper OriginalDLCHotels = helper.AddGroup("Options") as UIHelper;

            OriginalDLCHotels.AddCheckbox("Use default random hotel names", Settings.UseDefaultRandomHotelNames.value, (b) =>
            {
                Settings.UseDefaultRandomHotelNames.value = b;
            });

        }
    }
}
