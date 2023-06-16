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
        private Vector3 meshOffset = new Vector3(0, 0, 0.2f);
        private GameObject trailFolder;
        private Material mat;
        private bool isTrailActive;
        private SkinnedMeshRenderer skinnedMeshRenderer;
        private Texture tex;

        // Paramaters
        private float speedThreshold;
        private float rate;
        private float lifeTime;
        private float factor1;
        private float factor2;
        private Color color1;
        private Color color2;

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
            speedThreshold = TrailModPlugin.speedThreshold.Value;
            rate = TrailModPlugin.rate.Value;
            lifeTime = TrailModPlugin.lifeTime.Value;
            factor1 = TrailModPlugin.matFactor1.Value;
            factor2 = TrailModPlugin.matFactor2.Value;

            // Assign Colors
            color1 = new Color(
                TrailModPlugin.firstColorR.Value,
                TrailModPlugin.firstColorG.Value,
                TrailModPlugin.firstColorB.Value,
                TrailModPlugin.firstColorA.Value);

            color2 = new Color(
                TrailModPlugin.secondColorR.Value,
                TrailModPlugin.secondColorG.Value,
                TrailModPlugin.secondColorB.Value,
                TrailModPlugin.secondColorA.Value);

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
                gObj.transform.SetPositionAndRotation(transform.position + meshOffset, transform.rotation);

                MeshFilter mf = gObj.AddComponent<MeshFilter>();
                MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
                Mesh mesh = new Mesh();
                skinnedMeshRenderer.BakeMesh(mesh);

                mf.mesh = mesh;
                mr.material = mat;

                StartCoroutine(AnimateMaterialFloat(mr.material, 1));

                Destroy(gObj, lifeTime);
                Destroy(mesh, lifeTime);
            }
        }

        IEnumerator AnimateMaterialFloat(Material _mat, float timer) {
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
