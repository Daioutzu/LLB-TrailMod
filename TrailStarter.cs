using BepInEx;
using HarmonyLib; //https://harmony.pardeike.net/articles/patching-postfix.html
using StageBackground;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace TrailMod {

    public class TrailStarter : MonoBehaviour {

        public Transform holderPlayers;

        void Start() {
            // Avoid the MainMenu
            if (SceneManager.GetActiveScene().name == "title")
                return;

            holderPlayers = GameObject.Find("holderPlayers").transform;

            // For each Players in HolderPlayers
            for (int i = 0; i < holderPlayers.childCount; i++) {
                Transform player = holderPlayers.GetChild(i);

                // For each Elements in Player/main/...
                foreach (Transform element in player.GetChild(0)) {
                    if (element.name.Contains("_MainRenderer")) {
                        element.gameObject.AddComponent<Trail>();
                    }
                }

                // Candyman Legs Exception
                Transform candylegs = player.Find("runLegsVisual");
                if (candylegs != null) {
                    foreach (Transform element in candylegs) {
                        if (element.name.Contains("_MainRenderer")) {
                            element.gameObject.AddComponent<Trail>();
                        }
                    }
                }

                // Doombox Crouch Exception
                Transform boomboxProp = player.GetChild(0).Find("boomboxProp");
                if (boomboxProp != null) {
                    boomboxProp.gameObject.AddComponent<Trail>();
                }

                // Dust&Ashes JumpProp Exception
                Transform jumpProp2 = player.GetChild(0).Find("jumpProp2");
                if (jumpProp2 != null) {
                    jumpProp2.gameObject.AddComponent<Trail>();
                }

                // Nitro cuffVisual
                Transform cuffVisual = GameObject.Find("holderBalls/ball/cuffVisual/cuffMesh_MainRenderer").transform;
                if (cuffVisual != null) {
                    cuffVisual.gameObject.AddComponent<Trail>();
                }
            }
        }
    }
}
