using UnityEngine;
using System.Collections;

public class InputImage : MonoBehaviour {

    public Texture2D texture;
    public Vector3[] vertices;
    Mesh mesh;
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;

        texture = new Texture2D(12, 12);
        GetComponent<Renderer>().material.mainTexture = texture;
        int y = 0;
        int tmp = 0;
        while (y < texture.width)
        {
            int x = 0;
            while (x < texture.height)
            {
                Color color = ColorGradient.FullColorGradient(tmp, 0, 12*12);
                texture.SetPixel(x, y, color);
                ++tmp;
                ++x;
            }
            ++y;
        }
        texture.Apply();
    }

    void Update()
    {
        mesh.vertices = vertices;
    }
}
