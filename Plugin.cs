using BepInEx;
using HarmonyLib;
using Peglin.Achievements;

namespace PeglinCore
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInProcess("Peglin.exe")]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony(PluginInfo.PLUGIN_GUID);

        private void Awake()
        {
            harmony.PatchAll();
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }

        [HarmonyPatch(typeof(PeglinUI.LoadoutManager.LoadoutManager), "SetupDataForNewGame")]
        [HarmonyPostfix]
        static private void PatchSetupDataForNewGame() {
            AchievementManager.AchievementsOn = false;
        }
    }
}
