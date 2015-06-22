using UnityEngine;
using System.Collections;

public class ColorLerp : MonoBehaviour {
    public Color tmp1;
    public Color tmp2;
    [Range(0, 1)]
    public float inp;
	void Update () {
        tmp1 = Color.Lerp(Color.blue, Color.white, inp);
        tmp2 = ColorGradient.MonoChrome(inp);
	}
}
