using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LLBML.Math;
using BepInEx;

// C'est nous c'est la France :D (Anksoup & Aru !)

namespace TrailMod {

    public class Trail : MonoBehaviour {
        private float tick = 0;
        private float speed;
        private GameObject trailFolder;
        private Material mat;
        private bool isTrailActive;
        private SkinnedMeshRenderer skinnedMeshRenderer;
        private Texture tex;

        // Paramaters
        private float speedThreshold;
        private float lifeTime;
        private float rate;
        private Color color1;
        private Color color2;
        private float factor1;
        private float factor2;

        private void Start() {
            // Create a "folder" for the trails
            trailFolder = new GameObject();
            trailFolder.name = "TrailMod Folder";

            // Load the Shader and Assign it to Material
            Shader shader = TrailModPlugin.bundle.LoadAsset<Shader>("TrailShader");
            mat = new Material(shader);

            // Get the Texture from SkinnedMeshRenderer (_MainTexture is for old models from Aru)
            if (GetComponent<SkinnedMeshRenderer>().sharedMaterial.GetTexture("_MainTex") != null)
                tex = GetComponent<SkinnedMeshRenderer>().sharedMaterial.GetTexture("_MainTex");
            else
                tex = GetComponent<SkinnedMeshRenderer>().sharedMaterial.GetTexture("_MainTexture");

            // Init Parameters
            speedThreshold = (float)TrailModPlugin.speedThreshold.Value;
            lifeTime = (float)TrailModPlugin.lifeTime.Value / 1000;
            rate = lifeTime / ((float)TrailModPlugin.numberOfClones.Value * 1.15f);
            factor1 = (float)TrailModPlugin.matFactor1.Value / 100;
            factor2 = (float)TrailModPlugin.matFactor2.Value / 100;

            // Assign Colors
            color1 = new Color(
                (float)TrailModPlugin.firstColorR.Value / 255,
                (float)TrailModPlugin.firstColorG.Value / 255,
                (float)TrailModPlugin.firstColorB.Value / 255,
                (float)TrailModPlugin.firstColorA.Value / 255);

            color2 = new Color(
                (float)TrailModPlugin.secondColorR.Value / 255,
                (float)TrailModPlugin.secondColorG.Value / 255,
                (float)TrailModPlugin.secondColorB.Value / 255,
                (float)TrailModPlugin.secondColorA.Value / 255);
            

            // Assign Material
            mat.SetColor("_Color", color1);
            mat.SetTexture("_MainTex", tex);
        }

        void Update() {
            speed = (Floatf)LLHandlers.BallHandler.instance.balls[0].GetPixelFlySpeed(true);

            if (speed >= speedThreshold) {
                if (tick < rate) {
                    tick += Time.smoothDeltaTime;
                } else {
                    tick = 0;
                    ActivateTrail();
                }

                isTrailActive = true;
            } else {
                isTrailActive = false;
            }
        }

        void ActivateTrail() {
            if (isTrailActive) {

                if (skinnedMeshRenderer == null)
                    skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

                GameObject gObj = new GameObject();
                gObj.transform.parent = trailFolder.transform;
                gObj.transform.SetPositionAndRotation(transform.position, transform.rotation);

                MeshFilter mf = gObj.AddComponent<MeshFilter>();
                MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
                Mesh mesh = new Mesh();
                skinnedMeshRenderer.BakeMesh(mesh);

                mf.mesh = mesh;
                mr.material = mat;

                StartCoroutine(AnimateMaterialFloat(mr.material, 1, gObj.transform.GetSiblingIndex()));
                RenderQueueUpdate();

                Destroy(gObj, lifeTime);
                Destroy(mesh, lifeTime);
            }
        }

        void RenderQueueUpdate() {
            for (int i = 0; i < trailFolder.transform.childCount; i++) {
                Transform mesh = trailFolder.transform.GetChild(i);
                mesh.GetComponent<MeshRenderer>().material.renderQueue = (3000 + trailFolder.transform.childCount) - mesh.GetSiblingIndex();
            }
        }

        IEnumerator AnimateMaterialFloat(Material _mat, float timer, int index) {
            while (timer > 0) {
                timer -= rate / lifeTime;

                // Colors
                Color _color = Color.Lerp(color2, color1, timer);
                _mat.SetColor("_Color", _color);

                // Factor
                float _factor = Mathf.Lerp(factor2, factor1, timer);
                _mat.SetFloat("_Factor", _factor);

                yield return new WaitForSeconds(rate);
            }
        }
    }
}
