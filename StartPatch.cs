using HarmonyLib; //https://harmony.pardeike.net/articles/patching-postfix.html
using StageBackground;
using UnityEngine;

namespace TrailMod {

    [HarmonyPatch(typeof(BG), nameof(BG.StartUp))]
    public class StartPatch {
        public static void Postfix() { // A postfix is a method that is executed after the original method

            if (!BG.instance.gameObject.GetComponent<TrailStarter>()) {
                BG.instance.gameObject.AddComponent<TrailStarter>();
            }
        }
    }
}
