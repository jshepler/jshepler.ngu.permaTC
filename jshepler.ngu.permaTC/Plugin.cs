using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace jshepler.ngu.permaTC
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInIncompatibility("jshepler.ngu.mods")]
    [BepInIncompatibility("jshepler.ngu.hardcore")]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony(PluginInfo.PLUGIN_GUID);
        private static ManualLogSource Log;
        internal static void LogInfo(string text) => Log.LogInfo(text);

        private void Awake()
        {
            // prevents the bepinex manager object (i.e. this plugin instance) from being destroyed after Awake()
            // https://github.com/aedenthorn/PlanetCrafterMods/issues/7
            // not needed for all games, but I'm not currently aware of anything that it would hurt
            this.gameObject.hideFlags = HideFlags.HideAndDontSave;

            Log = base.Logger;

            harmony.PatchAll();
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }
    }
}
