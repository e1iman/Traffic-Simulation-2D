using UnityEngine;
using System.Collections;

public class Bilder : MonoBehaviour {
    public static Bilder bilder;
    public GameObject[] roads;
    public Material mat;
    public bool editMode = false;
	void Awake () {
        if (bilder != null)
            GameObject.Destroy(bilder);
        else
            bilder = this;
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            EditModeToggle();
        }
        if (editMode && Input.GetButtonDown("AddFlowInfo")) // Input.GetKeyDown(KeyCode.Z)
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
             GameObject tmp = (GameObject)Instantiate(roads[0], new Vector3(hit.point.x, hit.point.y, 0), Quaternion.identity);
             tmp.transform.parent = GameObject.Find("AnaliticObjects/TrafficFlow").transform;
            }

        }
    }
    public void EditModeToggle()
    {
        editMode = (editMode) ? false : true;
        mat.color = (editMode) ? new Color32(255, 0, 0, 255) : new Color32(221, 221, 221, 255);
    }
}
