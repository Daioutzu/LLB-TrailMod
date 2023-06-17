using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TrailMod {

    [BepInPlugin(TrailModInfos.PLUGIN_ID, TrailModInfos.PLUGIN_NAME, TrailModInfos.PLUGIN_VERSION)]
    [BepInDependency(LLBML.PluginInfos.PLUGIN_ID, BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("no.mrgentle.plugins.llb.modmenu", BepInDependency.DependencyFlags.SoftDependency)]

    public class TrailModPlugin : BaseUnityPlugin {
        public static TrailModPlugin instance;
        public static DirectoryInfo PluginDir => LLBML.Utils.ModdingFolder.GetModSubFolder(instance.Info);
        internal static ManualLogSource Log { get; private set; }

        internal static ConfigEntry<int> speedThreshold;

        internal static ConfigEntry<int> numberOfClones;
        internal static ConfigEntry<int> lifeTime;

        internal static ConfigEntry<int> firstColorR;
        internal static ConfigEntry<int> firstColorG;
        internal static ConfigEntry<int> firstColorB;
        internal static ConfigEntry<int> firstColorA;

        internal static ConfigEntry<int> secondColorR;
        internal static ConfigEntry<int> secondColorG;
        internal static ConfigEntry<int> secondColorB;
        internal static ConfigEntry<int> secondColorA;

        internal static ConfigEntry<int> matFactor1;
        internal static ConfigEntry<int> matFactor2;

        internal static AssetBundle bundle;

        void Awake() {
            // Parameters
            Config.Bind("1. Activation", "Activation_header", "1. Activation", new ConfigDescription("", null, "modmenu_header"));

            speedThreshold = Config.Bind("1. Activation", "AtBallSpeed", 250, "At what speed the effect is triggered");

            Config.Bind("", "gap1", 20, new ConfigDescription("", null, "modmenu_gap"));
            Config.Bind("2. TrailConfig", "Trail_header", "2. TrailConfig", new ConfigDescription("", null, "modmenu_header"));

            numberOfClones = Config.Bind("2. TrailConfig", "NumberOfClones", 6, "The number of clones inside the Trail");
            lifeTime = Config.Bind("2. TrailConfig", "LifeTimeInMS", 400, "The life time of those clones, basically how long is your Trail");//ms

            Config.Bind("", "gap2", 20, new ConfigDescription("", null, "modmenu_gap"));
            Config.Bind("3. Colors", "Color_header", "3. Colors", new ConfigDescription("", null, "modmenu_header"));

            firstColorR = Config.Bind("3. Colors", "FirstColorR", 204, new ConfigDescription("", new AcceptableValueRange<int>(0, 255)));
            firstColorG = Config.Bind("3. Colors", "FirstColorG", 51, new ConfigDescription("", new AcceptableValueRange<int>(0, 255)));
            firstColorB = Config.Bind("3. Colors", "FirstColorB", 76, new ConfigDescription("", new AcceptableValueRange<int>(0, 255)));
            firstColorA = Config.Bind("3. Colors", "FirstColorA", 255, new ConfigDescription("", new AcceptableValueRange<int>(0, 255)));

            Config.Bind("", "gap3", 20, new ConfigDescription("", null, "modmenu_gap"));

            secondColorR = Config.Bind("3. Colors", "SecondColorR", 128, new ConfigDescription("", new AcceptableValueRange<int>(0, 255)));
            secondColorG = Config.Bind("3. Colors", "SecondColorG", 0, new ConfigDescription("", new AcceptableValueRange<int>(0, 255)));
            secondColorB = Config.Bind("3. Colors", "SecondColorB", 76, new ConfigDescription("", new AcceptableValueRange<int>(0, 255)));
            secondColorA = Config.Bind("3. Colors", "SecondColorA", 0, new ConfigDescription("", new AcceptableValueRange<int>(0, 255)));

            Config.Bind("", "gap4", 20, new ConfigDescription("", null, "modmenu_gap"));
            Config.Bind("4. Texture intensity", "Intnsity_header", "4. Texture intensity", new ConfigDescription("", null, "modmenu_header"));

            matFactor1 = Config.Bind("4. Texture intensity", "Start", 0, new ConfigDescription("0 = visible texture --> 100 = uniformed color/no texture", new AcceptableValueRange<int>(0, 100)));
            matFactor2 = Config.Bind("4. Texture intensity", "End", 0, new ConfigDescription("0 = visible texture --> 100 = uniformed color/no texture", new AcceptableValueRange<int>(0, 100)));

            // Init
            instance = this;
            Log = this.Logger;

            // Load Bundle
            bundle = AssetBundle.LoadFromFile(Paths.BepInExRootPath + "/plugins/Andyzubukoup-TrailMod/LLB-TrailMod/trailbundle");

            // Path from Harmony
            var harmony = new Harmony(TrailModInfos.PLUGIN_NAME);
            harmony.PatchAll();

            // Log
            Log.LogInfo(PluginDir);
            log("TrailMod is loaded");
        }

        void Start() {
            LLBML.Utils.ModDependenciesUtils.RegisterToModMenu(this.Info, new List<string> { "Adds a trail following your character.", "", "4. Texture intensity:", "0 = visible texture -- > 100 = uniformed color / no texture" });
        }

        static public void log(string message) {
            Debug.Log($"<color=cyan>[TrailMod] {message}</color>");
        }
    }
}
