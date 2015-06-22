using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SpriteGrayscale : MonoBehaviour {
        [Range(0,1)]
        public float effectAmount = 1;
        public Renderer shader;
        

        // Called by camera to apply image effect
        void Update()
        {
            //mater.SetFloat("_EffectAmount", effectAmount);
            shader.sharedMaterial.SetFloat("_EffectAmount", effectAmount);
            //gameObject.GetComponent<SpriteRenderer>().sharedMaterial.SetFloat("_EffectAmount", effectAmount);

             //GAMEOBJECT.renderer.material.SetFloat('_EffectAmount', effectAmount);
        }
}