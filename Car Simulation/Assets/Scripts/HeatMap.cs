using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.Rendering;
using System.Collections;

public class HeatMap : MonoBehaviour {

    GameObject projectionPlane;//was static
    static bool shitbool = true;
    [SerializeField]
    GameObject UI;
    public void CreateRenderPlane(Texture2D map, Vector2 size, Vector3 position)//was static
    {
        if (projectionPlane == null)
        {
            // Create Plane to project Heatmap
            Mesh m = new Mesh();
            Vector3[] vertices = new Vector3[4];
            int[] triangles = new int[6] {
			2, 1, 0,
			2, 3, 1
		};

            //vertices[0] = cam.ScreenToWorldPoint(new Vector2(0f, 0f));
            //vertices[1] = cam.ScreenToWorldPoint(new Vector2(cam.pixelWidth, 0f));
            //vertices[2] = cam.ScreenToWorldPoint(new Vector2(0f, cam.pixelHeight));
            //vertices[3] = cam.ScreenToWorldPoint(new Vector2(cam.pixelWidth, cam.pixelHeight));

            vertices[0] = new Vector2(0f, 0f);
            vertices[1] = new Vector2(size.x, 0f);
            vertices[2] = new Vector2(0f, size.y);
            vertices[3] = new Vector2(size.x, size.y);

            Vector2[] uvs = new Vector2[4];

            uvs[0] = new Vector2(0f, 0f);
            uvs[1] = new Vector2(1f, 0f);
            uvs[2] = new Vector2(0f, 1f);
            uvs[3] = new Vector2(1f, 1f);

            m.vertices = vertices;
            m.triangles = triangles;
            m.uv = uvs;
            m.RecalculateNormals();
            m.Optimize();

            // Hook it all up
            if (projectionPlane == null)
            {
                projectionPlane = new GameObject("Plane" + UnityEngine.Random.Range(1, 10).ToString());
                projectionPlane.layer = 1;
                projectionPlane.AddComponent<MeshFilter>();
                projectionPlane.AddComponent<MeshRenderer>();
                //projectionPlane.AddComponent<BoxCollider2D>();
                //projectionPlane.AddComponent<AgentCount>();
            }
            else
            {
                DestroyImmediate(projectionPlane.GetComponent<MeshFilter>().sharedMesh);
                DestroyImmediate(projectionPlane.GetComponent<MeshRenderer>().sharedMaterial.mainTexture);
            }

            projectionPlane.GetComponent<MeshFilter>().sharedMesh = m;
            MeshRenderer mr = projectionPlane.GetComponent<MeshRenderer>();
            Material mat = new Material((Material)Resources.Load("UnlitMaterial"));
            //BoxCollider2D col = projectionPlane.GetComponent<BoxCollider2D>();
            //col.isTrigger = true;
            //col.size = size;
            //col.offset = new Vector3(size.x / 2, size.y / 2);
            mat.mainTexture = map;
            mr.sharedMaterial = mat;
            mr.receiveShadows = false;
            mr.shadowCastingMode = ShadowCastingMode.Off;

            //projectionPlane.name = "Heatmap Render Plane";
            // Move the heatmap gameobject in front of the camera
            projectionPlane.transform.position = position; //new Vector3(0f, 0f, 0f);
            projectionPlane.transform.parent = gameObject.transform;
            //projectionPlane.transform.Translate(Vector3.forward, Camera.main.transform);
        }
    }

    Texture2D texture;
    AgentCount agCount;
    BoxCollider2D boxCol2D;

    public Transform bottomLeft;
    public Transform upperRight;
    public bool debug = true;

    [Range(1,50)]
    public int gridStep = 1;
    [Range(0,5)]
    public int colorMode = 0;
    
    Vector3 center;
    public Vector3 size;

    float[,] map;//data matrix
    float mapMin = 0;
    [Range(1,500)]
    public float mapMax = 1;
    void Start()
    {
        gameObject.AddComponent<BoxCollider2D>();
        gameObject.AddComponent<AgentCount>();
        boxCol2D = GetComponent<BoxCollider2D>();
        agCount = GetComponent<AgentCount>();

        SwapPoints();
        CreateMap();
        CreateTrigger2DBox();
        //RandomMap();
        //NormalizeMap();
        CreateTexture();
        CreateRenderPlane(texture, size, bottomLeft.position);

        InvokeRepeating("CreateTexture", 2, 10F);
        //InvokeRepeating("ResetMap", 2, 30F);
        InvokeRepeating("GetData", 1, 0.2f);
        DrawMenu();
    }
    void FixedUpdate()
    {
        //GetData();
        //NormalizeMap();
        //CreateTexture();
    }

    void OnDrawGizmos()
    {
        SwapPoints();
        if (debug)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(center, 4);
            Gizmos.DrawWireCube(center, size);
        }
    }
    int mapX = 0;
    int mapY = 0;
    void CreateMap()
    {
        mapX = (int)Mathf.Abs(size.x) / gridStep;
        mapY = (int)Mathf.Abs(size.y) / gridStep;
        map = new float[mapX, mapY];
        for (int i = 0; i < (int)Mathf.Abs(size.x) / gridStep; i++)
        {
            for (int j = 0; j < (int)Mathf.Abs(size.y) / gridStep; j++)
            {
                map[i, j] = 0;
            }
        }
    }
    void CreateTrigger2DBox()
    {
        boxCol2D.size = size;
        boxCol2D.offset = ((upperRight.localPosition - bottomLeft.localPosition) / 2) + bottomLeft.localPosition;
        boxCol2D.isTrigger = true;
    }
    void RandomMap()
    {
        int tmp = 0;
        for (int i = 0; i < (int)Mathf.Abs(size.x) / gridStep; i++)
        {
            for (int j = 0; j < (int)Mathf.Abs(size.y) / gridStep; j++)
            {
                map[i, j] = tmp;
                if (mapMax < map[i, j]) mapMax = map[i, j];
                if (mapMin > map[i, j]) mapMin = map[i, j];
                tmp++;
            }
        }
    }
    
      void CreateTexture()
    {
        if (texture == null)
        {
            texture = new Texture2D((int)size.x / gridStep, (int)size.y / gridStep);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Point;

        }

        if (!shitbool) texture.filterMode = FilterMode.Point;
        else texture.filterMode = FilterMode.Bilinear;
        //texture = new Texture2D(-5, -1);

        int y = 0;
        //Debug.Log("S: " + DateTime.Now.Second +":"+ DateTime.Now.Millisecond);
        while (y < texture.height)
        {
            int x = 0;
            while (x < texture.width)
            {
                Color color = Color.clear;
                switch (colorMode)
                {
                    case 0:
                        color = ColorGradient.MonoChrome(map[x, y], mapMin, mapMax);
                        break;
                    case 1:
                        color = ColorGradient.TwoColorGradentRG(map[x, y], mapMin, mapMax);
                        break;
                    case 2:
                        color = ColorGradient.FullColorGradient(map[x, y], mapMin, mapMax);
                        break;
                    case 3:
                        color = ColorGradient.FullColorBWR(map[x, y], mapMin, mapMax);
                        break;
                    case 4:
                        color = ColorGradient.FullColorWBR(map[x, y], mapMin, mapMax);
                        break;
                    case 5:
                        color = ColorGradient.FullColorCGY(map[x, y], mapMin, mapMax);
                        break;
                    default:
                        color = ColorGradient.FullColorGradient(map[x, y], mapMin, mapMax);
                        break;
                }
                color.a = Mathf.Clamp(color.a, 0f, 0.6f);
                texture.SetPixel(x, y, color);
                ++x;
            }
            ++y;
        }
        texture.Apply();
        //Debug.Log("F: " + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);

    }

    void GetData()
    {
        for (int i = 0; i < agCount.agentList.Count; i++)
        {
            if (agCount.agentList[i] != null)
            {
                //if (agCount.agentList[i].GetComponent<TestCar>().speed == 0)
                SetMapData(agCount.agentList[i]);
            }
            else
            {
                agCount.agentList.RemoveAt(i);
            }
        }
    }
    public void SetMapData(Transform input)
    {
        //  map = new float[(int)Mathf.Abs(size.x) / gridStep, (int)Mathf.Abs(size.y) / gridStep];

        //((upperRight.localPosition - bottomLeft.localPosition) / 2) + bottomLeft.localPosition;
        
        //size = new Vector3((upperRight.transform.position.x - bottomLeft.transform.position.x), 
        //(upperRight.transform.position.y - bottomLeft.transform.position.y), 
        //(upperRight.transform.position.z - bottomLeft.transform.position.z));

        int x = (int)((transform.InverseTransformPoint(input.position) / gridStep).x);
        int y = (int)((transform.InverseTransformPoint(input.position) / gridStep).y);

        x = Mathf.Clamp(x, 0, mapX-1);
        y = Mathf.Clamp(y, 0, mapY-1);

        //Vector3 lel = new Vector3((int)(input.position).x, (int)(input.position).y, 0);

        //Instantiate(objTest, lel, Quaternion.identity);
        //int x = (int)((((upperRight.position - input.position) / 2) + bottomLeft.position).x);
        //int y = (int)((((upperRight.position - input.position) / 2) + bottomLeft.position).y);
        map[x, y] += 1;
        try
        {
            
        }
        catch
        {

        }
        //if (mapMax < map[x, y]) mapMax = map[x, y];
        if (debug)
        {

           // Debug.Log(((input.position)/gridStep).ToString());
        }
    }
    void SwapPoints()
    {
        if(bottomLeft.position.x > upperRight.position.x)
        {
            Transform tmp = bottomLeft;
            bottomLeft = upperRight;
            upperRight = tmp;
        }
        #region SetSize
        center = new Vector3((bottomLeft.transform.position.x + upperRight.transform.position.x) / 2, (bottomLeft.transform.position.y + upperRight.transform.position.y) / 2, (bottomLeft.transform.position.z + upperRight.transform.position.z) / 2);
        size = new Vector3((upperRight.transform.position.x - bottomLeft.transform.position.x), (upperRight.transform.position.y - bottomLeft.transform.position.y), (upperRight.transform.position.z - bottomLeft.transform.position.z));
        #endregion
    }
    void ResetMap()
    {
        for (int i = 0; i < (int)Mathf.Abs(size.x) / gridStep; i++)
        {
            for (int j = 0; j < (int)Mathf.Abs(size.y) / gridStep; j++)
            {
                map[i, j] = 0;
            }
        }
    }
    public void InterpolateMap()
    {
        for (int i = 1; i < mapX-1; i++)
        {
            for (int j = 1; j < mapY-1; j++)
            {
                if (map[i, j] > mapMax / 2)
                {
                    map[i - 1, j] += Mathf.Sqrt(map[i,j]);
                    map[i + 1, j] += Mathf.Sqrt(map[i, j]);
                    map[i, j - 1] += Mathf.Sqrt(map[i, j]);
                    map[i, j + 1] += Mathf.Sqrt(map[i, j]);
                }
            }
        }
    }

    public void SetMax(float inp)
    {
        mapMax = inp;
        CreateTexture();
    }
    public void test()
    {
        if (shitbool == true)
            shitbool = false;
        else
            shitbool = true;
    }

    void DrawMenu()
    {
        if (UI != null)
        {
            UI.GetComponentInChildren<Button>().onClick.AddListener(
                () => { colorMode++;
                colorMode = (colorMode > 5) ? 0 : colorMode;
                CreateTexture();
                }
            );
            UI.GetComponentInChildren<Slider>().onValueChanged.AddListener(
                (value) => { mapMax = value; CreateTexture(); }    
            );
            UI.GetComponentInChildren<Toggle>().onValueChanged.AddListener(
            (value) => { UploadPNG(); }
            );
            UI.transform.FindChild("Reset").GetComponent<Toggle>().onValueChanged.AddListener(
                (value) => { ResetMap(); CreateTexture(); }
            );
        }
    }
    int jpgCounter = 0;
    void UploadPNG()
    {
        var tex = new Texture2D(texture.width, texture.height, TextureFormat.RGB24, false);
        tex.SetPixels(texture.GetPixels());
        try
        {
            TextureScale.Bilinear(tex, 1280, 1280 * texture.height / texture.width);
        }
        catch (DivideByZeroException e)
        {
            Debug.LogException(e);
        }
        tex.Apply();

        var bytes = tex.EncodeToPNG();
        Destroy(tex);
        try
        {
            File.WriteAllBytes(Application.dataPath + "/../HeatMap" + jpgCounter + ".png", bytes);
        }
        catch (ArgumentNullException e)
        {
            Debug.LogException(e);
        }
        jpgCounter++;
    }
}

