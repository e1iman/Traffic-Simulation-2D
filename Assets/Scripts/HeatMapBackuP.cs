using UnityEngine;
using System.Collections;

public class HeatMapBackuP: MonoBehaviour {
    public GameObject cube;

    public Transform bottomLeft;
    public Transform upperRight;
    public bool debug = true;
    public Color col;
    int tmp = 0;//DELETE THIS!!
    [Range(1,50)]
    public int gridStep = 1;
    
    Vector3 center;
    Vector3 size;
    LineRenderer lineR;
    float[,] map;

    void Start()
    {
        cube.transform.localScale = Vector3.one * gridStep;
        SwapPoints();
        #region SetSize
        center = new Vector3((bottomLeft.transform.position.x + upperRight.transform.position.x) / 2, (bottomLeft.transform.position.y + upperRight.transform.position.y) / 2, (bottomLeft.transform.position.z + upperRight.transform.position.z) / 2);
        size = new Vector3((bottomLeft.transform.position.x - upperRight.transform.position.x), (bottomLeft.transform.position.y - upperRight.transform.position.y), (bottomLeft.transform.position.z - upperRight.transform.position.z));
        #endregion
        //DrawFrame();
        SetMapSize();
        DrawMap();
    }

    void OnDrawGizmos()
    {
        SwapPoints();
        if (debug)
        {
            center = new Vector3((bottomLeft.transform.position.x + upperRight.transform.position.x) / 2, (bottomLeft.transform.position.y + upperRight.transform.position.y) / 2, (bottomLeft.transform.position.z + upperRight.transform.position.z) / 2);
            size = new Vector3((bottomLeft.transform.position.x - upperRight.transform.position.x), (bottomLeft.transform.position.y - upperRight.transform.position.y), (bottomLeft.transform.position.z - upperRight.transform.position.z));
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(center, 4);
            Gizmos.DrawWireCube(center, size);

            /*for (int i = 0; i < (int)Mathf.Abs(size.x) / gridStep; i++)
            {
                for (int j = 0; j < (int)Mathf.Abs(size.y) / gridStep; j++)
                {
                    Gizmos.color = new Color(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255));
                    Vector3 pos = new Vector3(bottomLeft.position.x + i * gridStep + gridStep / 2, bottomLeft.position.y + j * gridStep + gridStep / 2);
                    Gizmos.DrawCube(pos, Vector3.one * gridStep);
                }
            }
             */
        }
    }

    void SetMapSize()
    {
        Debug.Log("x Size = " + Mathf.Abs(size.x));
        Debug.Log("y Size = " + Mathf.Abs(size.y));
        map = new float[(int)Mathf.Abs(size.x) / gridStep, (int)Mathf.Abs(size.y) / gridStep];

        
        //random fill 
        for (int i = 0; i < (int)Mathf.Abs(size.x) / gridStep; i++)
        {
            for (int j = 0; j < (int)Mathf.Abs(size.y) / gridStep; j++)
            {
                map[i, j] = tmp;
                tmp++;
            }
        }
    }

    void DrawFrame()
    {
        center = new Vector3((bottomLeft.transform.position.x + upperRight.transform.position.x) / 2, (bottomLeft.transform.position.y + upperRight.transform.position.y) / 2, (bottomLeft.transform.position.z + upperRight.transform.position.z) / 2);
        lineR = GetComponent<LineRenderer>();
        if (lineR == null)
        {
            Debug.LogWarning("Attached LineRendere to:" + gameObject.name);
            lineR = gameObject.AddComponent<LineRenderer>();
        }
        lineR.SetVertexCount(9);
        lineR.SetPosition(0, bottomLeft.position);
        lineR.SetPosition(1, new Vector3(bottomLeft.position.x, upperRight.position.y, center.z));
        lineR.SetPosition(2, upperRight.position);
        lineR.SetPosition(3, new Vector3(upperRight.position.x, bottomLeft.position.y, center.z));
        lineR.SetPosition(4, bottomLeft.position);
        lineR.SetPosition(5, new Vector3(upperRight.position.x, bottomLeft.position.y, center.z));
        lineR.SetPosition(6, upperRight.position);
        lineR.SetPosition(7, new Vector3(bottomLeft.position.x, upperRight.position.y, center.z));
        lineR.SetPosition(8, bottomLeft.position);
    }

    void SwapPoints()
    {
        if(bottomLeft.position.x > upperRight.position.x)
        {
            Transform tmp = bottomLeft;
            bottomLeft = upperRight;
            upperRight = tmp;
        }
    }
    void DrawMap()
    {
        size = new Vector3((bottomLeft.transform.position.x - upperRight.transform.position.x), (bottomLeft.transform.position.y - upperRight.transform.position.y), (bottomLeft.transform.position.z - upperRight.transform.position.z));

        for (int i = 0; i < (int)Mathf.Abs(size.x) / gridStep; i++)
        {
            for (int j = 0; j < (int)Mathf.Abs(size.y) / gridStep; j++)
            {
                //Color color = new Color(map[i, j], map[i, j], map[i, j]);
                //Color color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
                Color color = ColorGradient.FullColorGradient(map[i,j],0,tmp);

                Vector3 pos = new Vector3(bottomLeft.position.x + i * gridStep + gridStep / 2, bottomLeft.position.y + j * gridStep + gridStep / 2, 5);

                GameObject other = (GameObject)Instantiate(cube, pos, Quaternion.identity);
                other.name = map[i, j].ToString();
                other.GetComponent<Renderer>().material.color = color;
            }
        }
        
    }
    void NormalizeMap()
    {

    }
}
