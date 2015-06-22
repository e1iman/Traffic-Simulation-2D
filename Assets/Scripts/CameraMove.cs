using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour
{

    // Use this for initialization
    public float speed = 10;
    public float minAngle = 310;
    public float maxAngle = 355;

    public float minHeigh = 20;
    public float maxHeigh = 120;

    float deltaTime = 0;
    float prevTime = 0;
    Vector3 position;
    Vector3 rotation;
    float t;
    void Update()
    {
        deltaTime = Time.realtimeSinceStartup - prevTime;
        prevTime = Time.realtimeSinceStartup;

        //315 - 355
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(speed * deltaTime, 0, 0), Space.World);
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(-speed * deltaTime, 0, 0), Space.World);
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            transform.Translate(new Vector3(0, -speed * deltaTime, 0), Space.World);
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector3(0, speed * deltaTime, 0), Space.World);
        }
        if (!GameMaster.GM.inputFocus && Input.GetAxis("Mouse ScrollWheel") < 0) // OUT
        {
            position = transform.position;
            position.z -= 5;
            position.z = Mathf.Clamp(position.z, -maxHeigh, -minHeigh);
            transform.position = position;

            t = (position.z - (-minHeigh)) / (-maxHeigh - (-minHeigh));
            t = Mathf.Clamp01(t);
            //rotation.x = Mathfx.Sinerp(310, 350, t);
            rotation.x = Mathfx.Clerp(minAngle, maxAngle, t);
            transform.eulerAngles = rotation;
        }
        if (!GameMaster.GM.inputFocus && Input.GetAxis("Mouse ScrollWheel") > 0) // IN
        {
            position = transform.position;
            position.z += 5;
            position.z = Mathf.Clamp(position.z, -maxHeigh, -minHeigh);
            transform.position = position;

            t = (position.z - (-minHeigh)) / (-maxHeigh - (-minHeigh));
            t = Mathf.Clamp01(t);
            //rotation.x = Mathfx.Sinerp(310, 350f, t);
            rotation.x = Mathfx.Clerp(minAngle, maxAngle, t);
            transform.eulerAngles = rotation;
        }
        //if(Input.GetMouseButton(1)){
        //    transform.Translate(new Vector3(Input.GetAxis ("Mouse X") * 2, Input.GetAxis ("Mouse Y") * 2, 0), Space.World);
        //}

        /*
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit))
        {
            gameObject.transform.position = hit.point;
        }
         */
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
     {
     
         lastPosition = Input.mousePosition;
     }
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
     {
         var delta = Input.mousePosition - lastPosition;
         transform.Translate(-delta.x * 0.4f, -delta.y * 0.4f, 0, Space.World);
         lastPosition = Input.mousePosition;
     }
    }
    Vector3 lastPosition = Vector3.zero;
    Vector3 posOrigin = Vector3.zero;
}

