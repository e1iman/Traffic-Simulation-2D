using UnityEngine;
using System.Collections;

public class DragAndMove : MonoBehaviour {

    float posX;
    float posY;

    float deltaTime = 0;
    float prevTime = 0;
    void OnMouseDown()
    {
        if (Bilder.bilder.editMode)
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                posX = hit.point.x - transform.position.x;
                posY = hit.point.y - transform.position.y;
            }
        }
    }
    void OnMouseDrag()
    {
        Debug.Log("DRAGGING");
        deltaTime = Time.realtimeSinceStartup - prevTime;
        prevTime = Time.realtimeSinceStartup;

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            gameObject.transform.position = new Vector3(hit.point.x - posX, hit.point.y - posY, transform.position.z);
        }
    }
}

        