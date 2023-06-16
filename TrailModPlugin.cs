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

        internal static ConfigEntry<float> rate;
        internal static ConfigEntry<float> lifeTime;

        internal static ConfigEntry<float> firstColorR;
        internal static ConfigEntry<float> firstColorG;
        internal static ConfigEntry<float> firstColorB;
        internal static ConfigEntry<float> firstColorA;

        internal static ConfigEntry<float> secondColorR;
        internal static ConfigEntry<float> secondColorG;
        internal static ConfigEntry<float> secondColorB;
        internal static ConfigEntry<float> secondColorA;

        internal static ConfigEntry<float> matFactor1;
        internal static ConfigEntry<float> matFactor2;

        internal static AssetBundle bundle;

        void Awake() {
            // Parameters
            speedThreshold = Config.Bind("1. Activation", "AtBallSpeed", 250, new ConfigDescription("At what speed the effect is triggered", new AcceptableValueRange<int>(4, 10000000)));

            rate = Config.Bind("2. TrailConfig", "Rate", 0.05f, "The rate of 'clones' inside the Trail");
            lifeTime = Config.Bind("2. TrailConfig", "LifeTime", 0.4f, "The life time of those 'clones', basically how long is your Trail");

            firstColorR = Config.Bind("3. Colors (preferably between 0 & 1)", "FirstColorR", 0.8f, "");
            firstColorG = Config.Bind("3. Colors (preferably between 0 & 1)", "FirstColorG", 0.2f, "");
            firstColorB = Config.Bind("3. Colors (preferably between 0 & 1)", "FirstColorB", 0.3f, "");
            firstColorA = Config.Bind("3. Colors (preferably between 0 & 1)", "FirstColorA", 1f, "");

            secondColorR = Config.Bind("3. Colors (preferably between 0 & 1)", "SecondColorR", 0.5f, "");
            secondColorG = Config.Bind("3. Colors (preferably between 0 & 1)", "SecondColorG", 0f, "");
            secondColorB = Config.Bind("3. Colors (preferably between 0 & 1)", "SecondColorB", 0.3f, "");
            secondColorA = Config.Bind("3. Colors (preferably between 0 & 1)", "SecondColorA", 0f, "");

            matFactor1 = Config.Bind("4. Texture intensity", "Start", 0f, "0 = visible texture --> 1 = uniformed color/no texture");
            matFactor2 = Config.Bind("4. Texture intensity", "End", 0f, "0 = visible texture --> 1 = uniformed color/no texture");

            // Init
            instance = this;
            Log = this.Logger;

            // Load Bundle
            bundle = AssetBundle.LoadFromFile(Paths.BepInExRootPath + "/plugins/LLB-TrailMod/trailbundle");

            // Path from Harmony
            var harmony = new Harmony(TrailModInfos.PLUGIN_NAME);
            harmony.PatchAll();

            // Log
            Log.LogInfo(PluginDir);
            log("TrailMod is loaded");
        }

        void Start() {
            LLBML.Utils.ModDependenciesUtils.RegisterToModMenu(this.Info, new List<string> { "", "" });
        }

        static public void log(string message) {
            Debug.Log($"<color=cyan>[TrailMod] {message}</color>");
        }
    }
}
