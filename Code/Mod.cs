using ICities;
using CitiesHarmony.API;
using CorrectTouristAndLeisureNames.Managers;

namespace CorrectTouristAndLeisureNames
{
    public class Mod : IUserMod
    {
        public string Name => "Correct Tourist And Leisure Names";

        public string Description => "Give correct names to custom hotels in the city";

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

            OriginalDLCHotels.AddCheckbox("Use after the dark dlc hotel names", Settings.UseAfterTheDarkDLCHoteNames.value, (b) =>
            {
                Settings.UseAfterTheDarkDLCHoteNames.value = b;
            });

        }
    }
}
