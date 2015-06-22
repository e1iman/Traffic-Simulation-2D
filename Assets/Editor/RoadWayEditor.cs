using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoadWay))]
public class RoadWayEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RoadWay myScript = (RoadWay)target;
        if (GUILayout.Button("Build Object"))
        {
            myScript.CreateRoad();
        }
        if (GUILayout.Button("AddOne"))
        {
            myScript.AddNode();
        }
    }
}