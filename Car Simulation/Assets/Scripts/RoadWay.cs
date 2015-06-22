using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class RoadWay : MonoBehaviour {
	public List <Transform> roadWaypath;
    
    [Range(1,999)]
    public int speedLimit = 999;
    [Range(1,5)]
    public int pathSize = 1;

    #region EditorScripts
    public void CreateRoad()
    {
        var tempList = transform.Cast<Transform>().ToList();
        foreach (var child in tempList)
        {
            DestroyImmediate(child.gameObject);
        }

        if (roadWaypath.Capacity != 0)
        {
            for (int i = 0; i < roadWaypath.Capacity; i++)
            {
                GameObject tmp = new GameObject((transform.name + " " + i).ToString());
                tmp.transform.parent = transform;
                if (i != 0) {
                    tmp.AddComponent<GizmosIcon>();
                }
                tmp.transform.localPosition = Vector2.right * i * 3.5f * 2;

                roadWaypath[i] = tmp.transform;
            }
        }
    }
    public void AddNode()
    {
        GameObject tmp = new GameObject((transform.name + " " + roadWaypath.Capacity + 1).ToString());
        tmp.transform.parent = transform;
        tmp.AddComponent<GizmosIcon>();
        tmp.transform.localPosition = roadWaypath[roadWaypath.Capacity-1].localPosition + Vector3.right;
        roadWaypath.Capacity++;
        roadWaypath.Add(tmp.transform);
    }
    void OnDrawGizmos()
    {
        if (roadWaypath.Count == roadWaypath.Capacity)
        {
            for (int i = 0; i < roadWaypath.Capacity - 1; i++)
            {
                Gizmos.color = ColorGradient.TwoColorGradentRB(i, 0, roadWaypath.Capacity);
                Gizmos.DrawLine(roadWaypath[i].position, roadWaypath[i + 1].position);
                Vector3 tmpDir = (roadWaypath[i + 1].position - roadWaypath[i].position).normalized;
                Gizmos.DrawSphere(roadWaypath[i].position + new Vector3(tmpDir.y, -tmpDir.x) * (pathSize - 1) * 3.5f, 1.75f);

                Gizmos.DrawLine(roadWaypath[0].position, roadWaypath[1].position);
                //Gizmos.DrawSphere(roadWaypath[i].position + new Vector3(-tmpDir.y, tmpDir.x) * 3.5f, 1.75f);
                int kkk = roadWaypath.Capacity - 1;
                if (i == kkk-1)
                {
                    Gizmos.DrawSphere(roadWaypath[kkk].position + new Vector3(tmpDir.y, -tmpDir.x) * (pathSize - 1) * 3.5f, 1.75f);
                }
            }
            
        }
    }
    #endregion


    Vector3 LineIntersectionPoint(Vector3 ps1, Vector3 pe1, Vector3 ps2, Vector3 pe2)
    {
        // Get A,B,C of first line - points : ps1 to pe1
        float A1 = pe1.y - ps1.y;
        float B1 = ps1.x - pe1.x;
        float C1 = A1 * ps1.x + B1 * ps1.y;

        // Get A,B,C of second line - points : ps2 to pe2
        float A2 = pe2.y - ps2.y;
        float B2 = ps2.x - pe2.x;
        float C2 = A2 * ps2.x + B2 * ps2.y;

        // Get delta and check if the lines are parallel
        float delta = A1 * B2 - A2 * B1;
        if (delta == 0)
            throw new System.Exception("Lines are parallel");

        // now return the Vector2 intersection point
        return new Vector3(
            (B2 * C1 - B1 * C2) / delta,
            (A1 * C2 - A2 * C1) / delta
        );
    }
}

