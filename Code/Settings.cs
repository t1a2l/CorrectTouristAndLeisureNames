using ColossalFramework;
using System;
using UnityEngine;

namespace CorrectTouristAndLeisureNames
{
    public class Settings
    {
        public const string settingsFileName = "CorrectTouristAndLeisureNames_Settings";

        public static SavedBool UseAfterTheDarkDLCHoteNames = new("UseAfterTheDarkDLCHoteNames", settingsFileName, false, true);

        public static void Init()
        {
            try
            {
                // Creating setting file
                if (GameSettings.FindSettingsFileByName(settingsFileName) == null)
                {
                    GameSettings.AddSettingsFile([new SettingsFile() { fileName = settingsFileName }]);
                }
            }
            catch (Exception e)
            {
                Debug.Log("Could not load/create the setting file.");
                Debug.LogException(e);
            }
        }
    }
}
