using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(RoadMaster))]
public class RoadMasterEditor : Editor {
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RoadMaster myScript = (RoadMaster)target;
        if (GUILayout.Button("Fix"))
        {
            myScript.Fix();
        }
    }

}
