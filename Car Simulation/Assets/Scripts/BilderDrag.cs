using UnityEngine;
using System.Collections;

public class BilderDrag : MonoBehaviour {
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
        deltaTime = Time.realtimeSinceStartup - prevTime;
        prevTime = Time.realtimeSinceStartup;

        if (Bilder.bilder.editMode)
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                gameObject.transform.position = new Vector3(hit.point.x - posX, hit.point.y - posY, transform.position.z);
            }

            if(Input.GetButton("RotateLeft")){ //Input.GetKey(KeyCode.Q)
                transform.RotateAroundLocal(transform.forward, deltaTime);
            }
            if (Input.GetButton("RotateRight"))//Input.GetKey(KeyCode.E)
            {
                transform.RotateAroundLocal(transform.forward, -deltaTime);
            }
            if (Input.GetButton("ScaleBig1"))//Input.GetKey(KeyCode.X)
            {
                int tmp = 1;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    tmp = 5;
                }
                transform.localScale += Vector3.right * deltaTime * tmp;
            }
            if (Input.GetButton("ScaleBig2"))//Input.GetKey(KeyCode.C)
            {
                int tmp = 1;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    tmp = 5;
                }
                transform.localScale += Vector3.up * deltaTime * tmp;
            }
            if(Input.GetButton("ScaleSmall1"))//Input.GetKey(KeyCode.V)
            {
                int tmp = 1;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    tmp = 5;
                }
                transform.localScale -= Vector3.right * deltaTime * tmp;
            }
            if (Input.GetButton("ScaleSmall2"))//Input.GetKey(KeyCode.B)
            {
                int tmp = 1;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    tmp = 5;
                }
                transform.localScale -= Vector3.up * deltaTime * tmp;
            }
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Destroy(gameObject);
            }
        }
    }

}
