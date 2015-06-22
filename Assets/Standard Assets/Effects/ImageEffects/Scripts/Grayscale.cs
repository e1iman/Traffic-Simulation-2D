using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
    [ExecuteInEditMode]
    [AddComponentMenu("Image Effects/Color Adjustments/Grayscale")]
    public class Grayscale : ImageEffectBase {
        public Texture  textureRamp;
        public float    rampOffset;
        [Range(0,1)]
        public float effectAmount = 1;
        public Camera HeatMapCamera;
        // Called by camera to apply image effect
        void OnRenderImage (RenderTexture source, RenderTexture destination) {
            material.SetTexture("_RampTex", textureRamp);
            material.SetFloat("_RampOffset", rampOffset);
            material.SetFloat("_EffectAmount", effectAmount);
            Graphics.Blit (source, destination, material);
        }
        public void ToggleHeatMap()
        {
                effectAmount = (effectAmount == 0) ? 1 : 0;
                HeatMapCamera.cullingMask = (Convert.ToBoolean(effectAmount)) ? 2 : 0;
        }
        public void ToggleEditMap()
        {
                effectAmount = (effectAmount == 0) ? 1 : 0;
                HeatMapCamera.cullingMask = (Convert.ToBoolean(effectAmount)) ? 1 << 8 : 0;
        }
    }
}
