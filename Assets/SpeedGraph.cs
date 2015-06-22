using UnityEngine;
using System.Collections;

public class SpeedGraph : MonoBehaviour {

    public static SpeedGraph SG;

    [Range(10, 200)]
    public int resolution = 25;

    private int currentResolution;
    private ParticleSystem.Particle[] points;

	void Start () {
        if (SG == null)
        {
            SG = this;
        }
        CreatePoints();
	}

    private void CreatePoints()
    {
        currentResolution = resolution;
        points = new ParticleSystem.Particle[resolution];
        float increment = 1f / (resolution - 1);
        for (int i = 0; i < resolution; i++)
        {
            float x = i * increment;
            points[i].position = new Vector3(x, 0f, 0f);
            points[i].color = ColorGradient.FullColorBWR(x, 0, 1);
            points[i].size = 0.1f;
        }
    }
    void Update()
    {
        if (currentResolution != resolution)
        {
            CreatePoints();
        }
        for (int i = 0; i < resolution; i++)
        {
            Vector3 p = points[i].position;
//            p.y = p.x;
            points[i].position = p;
        }
        GetComponent<ParticleSystem>().SetParticles(points, points.Length);
    }
    int k = 0;
    public void SetPoint(float value)
    {
        k = (k < resolution) ? k : 0;
        Vector3 tmp = points[k].position;
        tmp.y = value / 20;
        tmp.y = Mathf.Clamp(tmp.y, 0, 20);
        points[k].position = tmp;
        Color col = ColorGradient.FullColorBWR(1 - tmp.y, 0, 1);
        col.a = 1;
        points[k].color = col;
        k++;
    }
}
