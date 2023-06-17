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

        public Transform players;
        public Transform player;

        void Start() {
            // Avoid the MainMenu
            if (SceneManager.GetActiveScene().name == "title")
                return;

            players = GameObject.Find("holderPlayers").transform;

            // For each Players in HolderPlayers
            for (int i = 0; i < players.childCount; i++) {
                player = players.GetChild(i);

                // For each Elements in Player/main/...
                foreach (Transform element in player.GetChild(0)) {
                    if (element.name.Contains("_MainRenderer")) {
                        element.gameObject.AddComponent<Trail>();
                    }
                }

                /// EXCEPTIONS
                if (player.name == "candyPlayer")
                    CandymanException();

                if (player.name == "bossPlayer")
                    DoomboxException();

                if (player.name == "bagPlayer") {
                    DustAndAshesException();
                    //DustAndAshesShadowException(); // WIP
                }

                if (player.name == "copPlayer")
                    NitroException();
            }
        }

        void CandymanException() {
            Transform candylegs = player.Find("runLegsVisual");

            if (candylegs != null) {
                foreach (Transform mesh in candylegs) {
                    if (mesh.name.Contains("_MainRenderer")) {
                        mesh.gameObject.AddComponent<Trail>();
                        TrailModPlugin.log("[Trail Exception] Player" + (player.GetSiblingIndex() + 1) + "/" + mesh.name);
                        return;
                    }
                }
            }
        }

        void DoomboxException() {
            Transform DoomboxProps = player.GetChild(0);

            foreach (Transform mesh in DoomboxProps) {
                if (mesh.name.Contains("Prop")) {
                    mesh.gameObject.AddComponent<Trail>();
                    TrailModPlugin.log("[Trail Exception] Player" + (player.GetSiblingIndex() + 1) + "/" + mesh.name);
                    return;
                }
            }
        }

        void DustAndAshesException() {
            // Kite
            Transform DustAndAshesProps = player.GetChild(0);
            foreach (Transform mesh in DustAndAshesProps) {
                if (mesh.name.Contains("jumpProp")) {
                    mesh.gameObject.AddComponent<Trail>();
                    TrailModPlugin.log("[Trail Exception] Player" + (player.GetSiblingIndex() + 1) + "/" + mesh.name);
                    return;
                }
            }
        }

        // WIP
        void DustAndAshesShadowException() {
            GameObject[] plop = SceneManager.GetActiveScene().GetRootGameObjects();

            for (int i = 0; i < plop.Length; i++) {
                if (plop[i].name == "shadowVisual") {
                    plop[i].transform.Find("mesh2_MainRenderer").gameObject.AddComponent<Trail>();
                    TrailModPlugin.log("[Trail Exception] Player" + (player.GetSiblingIndex() + 1) + "/" + plop[i].name);
                }
            }
        }

        void NitroException() {
            Transform NitroCuffs = GameObject.Find("holderBalls/ball").transform;

            foreach (Transform cuff in NitroCuffs) {
                if (cuff.name.Contains("cuff")) {
                    foreach (Transform mesh in cuff) {
                        if (mesh.name.Contains("_MainRenderer")) {
                            mesh.gameObject.AddComponent<Trail>();
                            TrailModPlugin.log("[Trail Exception] Player" + (player.GetSiblingIndex() + 1) + "/" + mesh.name);
                            return;
                        }
                    }
                }
            }
        }
    }
}
