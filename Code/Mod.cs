using ICities;
using CitiesHarmony.API;

namespace CorrectTouristAndLeisureNames
{
    public class Mod : IUserMod
    {
        public string Name => "Correct Tourist And Leisure Names";

        public string Description => "Give correct names to the hotles and clubs in the city";

        public void OnEnabled()
        {
            HarmonyHelper.DoOnHarmonyReady(() => Patcher.PatchAll());
        }

        public void OnDisabled()
        {
            if (HarmonyHelper.IsHarmonyInstalled)
            {
                Patcher.UnpatchAll();
            }
        }
    }
}
