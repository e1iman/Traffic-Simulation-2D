using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class TestCar : MonoBehaviour
{
    List<Transform> road;

    [Range(0, 20)]
    public float speed = 10;
    public int MaxSpeed = 17;
    public float turnSpeed = 10;
    public Projector projector;

    public float step = 2;
    public float AngleTest;// <<<<<<<<<<<<<<<<<<<<<<<<<<<  DELETE THIS!! ITS TEST FUNCTION!!!!
    [Range(0, 4)]
    public int currentRoadWay = 0;
    [Range(1, 5)]
    public int roadSize = 1;
    Vector3 to;
    Vector3 from;
    int i = 0;
    Vector3 changeNode;

    RaycastHit2D hitInfoL;
    RaycastHit2D hitInfoR;
    BoxCollider2D boxCol;
    Renderer carMat;
    Color colorStart;
    bool agresive = false;
    public Mesh[] models;
    void OnDestroy()
    {
        GameMaster.GM.carsAlive -= 1;
    }
    void Start()
    {
        if (Random.Range(0, 200) == 15) { agresive = true; }
        GameMaster.GM.carsAlive += 1;

        transform.parent = GameObject.Find("DynamicObjects").transform;
        to = transform.right.normalized;
        from = transform.position + to * step*2;
        speed = Random.Range(15, 20);
        currentRoadWay = Random.Range(0, 1);
        if (speedBackup <= 0)
        {
            speedBackup = Random.Range(15f, 30f);
            if (Random.Range(0, 100) < 3) speedBackup = Random.Range(10f, 15f);
            if (Random.Range(0, 100) == 3) speedBackup = Random.Range(6f, 9f);
        }
        

        boxCol = gameObject.GetComponent<BoxCollider2D>();
        carMat = transform.FindChild("lowpoly-car").FindChild("Mesh5_Model").GetComponent<Renderer>();
        colorStart = new Color(Random.Range(0.1f, 1), Random.Range(0.1f, 1), Random.Range(0.1f, 1));
    }

    public void SetBackupSpeed(float val)
    {
        speedBackup = val;
        speedBackup = Mathf.Clamp(val, 6, 30);
        if (models.Length == 5)
        {
            if (speedBackup < 10)
            {
                //very slow
                transform.GetChild(0).transform.localPosition = new Vector3(1.74f, 0.6f, -0.26f);
                transform.GetChild(0).transform.localEulerAngles = new Vector3(270, 0, 0);
                transform.FindChild("lowpoly-car").FindChild("Mesh5_Model").GetComponent<MeshFilter>().mesh = models[0];
            }
            if (speedBackup >= 10 && speedBackup < 15)
            {
                //slow
                transform.GetChild(0).transform.localPosition = new Vector3(-0.45f, 0f, -0.26f);
                transform.GetChild(0).transform.localScale = new Vector3(1, 0.8f, 1);
                transform.GetChild(0).transform.localEulerAngles = new Vector3(0, -180, -90);
                transform.FindChild("lowpoly-car").FindChild("Mesh5_Model").GetComponent<MeshFilter>().mesh = models[1];
            }
            if (speedBackup >= 15 && speedBackup < 20)
            {
                //medium
                transform.GetChild(0).transform.localPosition = new Vector3(-0.85f, 0f, -0.26f);
                transform.GetChild(0).transform.localEulerAngles = new Vector3(0, -180, -90);
                transform.FindChild("lowpoly-car").FindChild("Mesh5_Model").GetComponent<MeshFilter>().mesh = models[2];
            }
            if (speedBackup >= 20 && speedBackup < 25)
            {
                //fast
                transform.GetChild(0).transform.localPosition = new Vector3(-0.9f, 0f, -0.26f);
                transform.GetChild(0).transform.localEulerAngles = new Vector3(0, -180, -90);
                transform.FindChild("lowpoly-car").FindChild("Mesh5_Model").GetComponent<MeshFilter>().mesh = models[3];
            }
            if (speedBackup >= 25)
            {
                //very fast
                transform.GetChild(0).transform.localPosition = new Vector3(-0.7f, 0f, -0.26f);
                transform.GetChild(0).transform.localEulerAngles = new Vector3(0, -180, -90);
                transform.GetChild(0).transform.localScale = new Vector3(1, 1.1f, 1);
                transform.FindChild("lowpoly-car").FindChild("Mesh5_Model").GetComponent<MeshFilter>().mesh = models[4];
            }
        }
    }
    #region UItemporary
    void OnMouseUp()
    {
        GameMaster.GM.selected = transform;
        GameMaster.GM.infoPanel.Enable();
    }
    void OnMouseDrag()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit))
        {
            gameObject.transform.position = new Vector3(hit.point.x, hit.point.y, transform.position.z);
        }
    }
    #endregion
    [SerializeField]
    float speedBackup;
    TestCar otherCar;
    Collider2D prevCol;
    float prevTimeTryChangeLane = 0;
    Vector3 leftStart;
    Vector3 rightStart;
    void Update()
    {
        if (GameMaster.GM.selected == transform)
        {
            if (agresive)
            {
                GameMaster.GM.infoPanel.text = "<color=red><b> car </b> \nSpeed: " + speed.ToString("F1") + "\nMax Speed: " + speedBackup.ToString("F1") + "\nAllowed Speed: " + MaxSpeed.ToString("F1") + "\nLane:</color> " + currentRoadWay;
            }
            else
            {
                GameMaster.GM.infoPanel.text = "<b> car </b> \nSpeed: " + speed.ToString("F1") + "\nMax Speed: " + speedBackup.ToString("F1") + "\nAllowed Speed: " + MaxSpeed.ToString("F1") + "\n <color=yellow>Lane:</color> " + currentRoadWay;
            }
            carMat.material.color = Color.black;
        }
        else
        {
            if (GameMaster.GM.carSpeedMode)
            {
                carMat.material.color = SpeedColor(speed);
            }
            else
            {
                if (agresive)
                {
                    carMat.material.color = Color.white;
                }
                else
                {
                    carMat.material.color = colorStart;
                }
            }
        }
        //AngleTest = Vector2.Angle(transform.right, (from - transform.position)) / 180;// <<<<<<<<<<<<<<<<<<<<<<<<<<<  DELETE THIS!! ITS TEST FUNCTION!!!!
        #region RayCast2D

        leftStart = transform.position + transform.right * boxCol.size.x / 2 * 1.01f + transform.up * boxCol.size.y / 2;
        rightStart = transform.position + transform.right * boxCol.size.x / 2 * 1.01f - transform.up * boxCol.size.y / 2;
        hitInfoL = Physics2D.Raycast(leftStart, transform.right, 5f);
        hitInfoR = Physics2D.Raycast(rightStart, transform.right, 5f);
        Debug.DrawRay(leftStart, transform.right);
        Debug.DrawRay(rightStart, transform.right);

        if (hitInfoL.collider == boxCol) { Debug.Log("I hit myslef LEFT"); }
        if (hitInfoR.collider == boxCol) { Debug.Log("I hit myslef RIGHT"); }
        
        if (hitInfoL.point == Vector2.zero && hitInfoR.point == Vector2.zero)
        {
            //nothing is hit
            if (speed < 1)
            {
                speed += Time.deltaTime * 2;
            }
            else
            {
                speed += Random.Range(5, 10) * Time.deltaTime;
            }
            prevCol = null;
            otherCar = null;
        }
        else
        {
            // Something is hit
            if ((hitInfoL.point - (Vector2)transform.position).magnitude < (hitInfoR.point - (Vector2)transform.position).magnitude)
            {
                if (prevCol != hitInfoL.collider && hitInfoL.collider.tag == "Car")
                {
                    prevCol = hitInfoL.collider;
                    try
                    {
                        otherCar = hitInfoL.collider.GetComponent<TestCar>();
                        TryChangeLane();
                        //if (otherCar.speedBackup + 5 < speedBackup) { if (currentRoadWay == roadSize - 1) { currentRoadWay = otherCar.currentRoadWay - 1; } else { currentRoadWay = otherCar.currentRoadWay + 1; } }
                    }
                    catch { }
                }
                
                //speed = 0;//speed = 15 * hitInfoL.fraction;
                if( Vector3.Distance(leftStart, hitInfoL.point) <= 2f)
                {
                    try
                    {
                        float tmpspeed = otherCar.speed;//hitInfoL.collider.GetComponent<TestCar>().speed;
                        speed = Mathf.Clamp(speed, 0, tmpspeed);
                    }
                    catch { }
                }
                else
                {
                    try
                    {
                        if (otherCar.speed == 0)
                        {
                            speed -= 25 * Time.deltaTime;
                        }
                        else
                        {
                            speed -= Random.Range(0.8f, 1.2f) * Time.deltaTime;
                            if(speed < otherCar.speed) speed += Random.Range(0.8f, 1.2f) * Time.deltaTime;
                            //speed = Mathf.Clamp(speed, otherCar.speed - 0.5f, MaxSpeed);
                        }
                    }
                    catch { }
                }
                if (Vector3.Distance(leftStart, hitInfoL.point) < 1f)
                {
                    speed = 0;//speed = Mathf.Lerp(speed, 0, Time.deltaTime / 4); // FAST BREAK speed = 0
                }
                
            }
            else
            {
                if (prevCol != hitInfoR.collider && hitInfoR.collider.tag == "Car")
                {
                    prevCol = hitInfoR.collider;
                    try
                    {
                        otherCar = hitInfoR.collider.GetComponent<TestCar>();
                        TryChangeLane();
                        //if (otherCar.speedBackup + 5 < speedBackup) { if (currentRoadWay == roadSize - 1) { currentRoadWay = otherCar.currentRoadWay - 1; } else { currentRoadWay = otherCar.currentRoadWay + 1; } }
                    }
                    catch { }
                }
                if (Vector3.Distance(rightStart, hitInfoR.point) <= 2f)
                {
                    try
                    {
                        float tmpspeed = otherCar.speed;//= hitInfoR.collider.GetComponent<TestCar>().speed;
                        speed = Mathf.Clamp(speed, 0, tmpspeed);
                    }
                    catch { }
                }
                else
                {
                    try
                    {
                        if (otherCar.speed == 0)
                        {
                            speed -= 25 * Time.deltaTime;
                        }
                        else
                        {
                            speed -= Random.Range(0.8f, 1.2f) * Time.deltaTime;
                            if (speed < otherCar.speed) speed += Random.Range(0.8f, 1.2f) * Time.deltaTime;
                            //speed = Mathf.Clamp(speed, otherCar.speed - 0.5f, MaxSpeed);
                        }
                    }
                    catch { }
                }
                if (Vector3.Distance(rightStart, hitInfoR.point) < 1f)
                {
                    speed = 0;//speed = Mathf.Lerp(speed, 0, Time.deltaTime / 4); // FAST BREAK speed = 0
                }
            }
            //Debug.Log((hitInfo.point - (Vector2)transform.position).magnitude);
            if (hitInfoL.collider != null && hitInfoL.collider.tag == "TrafficLight" || hitInfoR.collider != null && hitInfoR.collider.tag == "TrafficLight")
            {
                speed = 0;
                //speed -= 20 * Time.deltaTime;
            }
        }
        if (agresive)
        {
            speed = Mathf.Clamp(speed, 0, speedBackup);
            speed = Mathf.Clamp(speed, 0, MaxSpeed + 5);
        }
        else
        {
            speed = Mathf.Clamp(speed, 0, speedBackup);
            speed = Mathf.Clamp(speed, 0, MaxSpeed);
        }

        #endregion
        #region MoveForward
        //move
        transform.position += transform.right * speed * Time.deltaTime;
        #endregion
        #region Rotate

        if (road != null)
        {   //when moving towards last node we whant to change our end point to this:
            if (i == road.Capacity - 1)
            {
                changeNode = road[i].position + new Vector3(to.y, -to.x) * currentRoadWay * 3.5f;
            }
            else
            {
                if (i < road.Capacity)
                {
                    if (i == 0)
                    {
                        Vector3 to2 = road[i + 1].position - road[i].position;
                        to2.Normalize();
                        Vector3 to2Inv = new Vector3(to2.y, -to2.x) * currentRoadWay * 3.5f;

                        changeNode = road[i].position + to2Inv;
                        to = (changeNode - transform.position).normalized;
                    }
                    else
                    {
                        Vector3 to1 = road[i].position - road[i - 1].position;
                        Vector3 to2 = road[i + 1].position - road[i].position;
                        to1.Normalize();
                        to2.Normalize();

                        Vector3 to1Inv = new Vector3(to.y, -to.x) * currentRoadWay * 3.5f;
                        Vector3 to2Inv = new Vector3(to2.y, -to2.x) * currentRoadWay * 3.5f;
                        try
                        {
                            changeNode = LineIntersectionPoint(road[i].position + to1Inv, road[i].position + to1Inv + to1, road[i + 1].position + to2Inv, road[i + 1].position + to2Inv + to2);
                        }
                        catch{
                            changeNode = road[i].position + to1Inv;
                            Debug.Log("Lines Are Paralel");
                        }
                    }
                }
            }
        }

        if (Vector3.Distance(transform.position, from + new Vector3(to.y, -to.x) * currentRoadWay * 3.5f) < step)
        {
            from += to * 2;
        }
        Vector3 dir = (from + new Vector3(to.y, -to.x) * currentRoadWay * 3.5f) - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

        //float rotateSpeed = turnSpeed * (speed / MaxSpeed) * (1-AngleTest);
        float rotateSpeed = turnSpeed * (speed / MaxSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotateSpeed);

        //Debug.DrawRay(from + new Vector3(to.y, -to.x) * currentRoadWay * 3.5f, to);
        //Debug.DrawRay(changeNode, to);
        #endregion
        #region waypointNodeChange
        if (road != null && i < road.Capacity)
        {
            //Vector3 target = road[i].position;

            if (Vector3.Distance(changeNode, transform.position) < step)
            {
                if (i < road.Capacity - 1)
                {
                    //from = road[i].position;
                    to = (road[i + 1].position - road[i].position).normalized;
                    to.z = 0;
                    from = changeNode + new Vector3(-to.y, to.x).normalized * currentRoadWay * 3.5f;
                    i++; //i = Mathf.Clamp(i, 0, road.Capacity);
                }
                else //on last node  take direction normal from last - prelast nodes
                {
                    from = road[i].position;
                    to = (road[i].position - road[i - 1].position).normalized;
                    to.z = 0;
                    i++;
                }
            }
        }
        #endregion
        if (prevTimeTryChangeLane < Time.time )
        {
            if (speed < 5)
            {
                if (agresive)
                {
                    prevTimeTryChangeLane = Time.time + Random.Range(0, 2);
                }
                else
                {
                    prevTimeTryChangeLane = Time.time + Random.Range(0, 5);
                }
            }
            else
            {
                if (agresive)
                {
                    prevTimeTryChangeLane = Time.time + Random.Range(3, 5);
                }
                else
                {
                    prevTimeTryChangeLane = Time.time + Random.Range(10, 20);
                }
                TryChangeLane2();
            }
        }
        currentRoadWay = Mathf.Clamp(currentRoadWay, 0, (int)(roadSize - 1));
        //Debug.DrawLine(rightStart + ((-transform.up) * 3.0f), rightStart + ((-transform.up) * 3.0f) + (transform.right * (-9f)));
    }
    void TryChangeLane()
    {//if (otherCar.speedBackup + 5 < speedBackup) { if (currentRoadWay == roadSize - 1) { currentRoadWay = otherCar.currentRoadWay - 1; } else { currentRoadWay = otherCar.currentRoadWay + 1; } }
        if (otherCar != null && speed > 2 && otherCar.currentRoadWay == currentRoadWay)
        {
            if (otherCar.speedBackup + 4.2f < speedBackup)
            {
                if (currentRoadWay >= 1 && !Physics2D.Raycast(leftStart + transform.right * 2, transform.up, 3.0f) && !Physics2D.Raycast(leftStart + (transform.up * 3.0f), -to, 9f))
                {
                    currentRoadWay = otherCar.currentRoadWay - 1;
                }
                else
                {
                    if (currentRoadWay + 1 <= roadSize - 1 && !Physics2D.Raycast(rightStart + transform.right * 2, -transform.up, 3.0f) && !Physics2D.Raycast(rightStart + ((-transform.up) * 3.0f), -to, 9f))
                    {
                        currentRoadWay = otherCar.currentRoadWay + 1;
                    }
                }
            }
        }
    }
    void TryChangeLane2()
    {//if (otherCar.speedBackup + 5 < speedBackup) { if (currentRoadWay == roadSize - 1) { currentRoadWay = otherCar.currentRoadWay - 1; } else { currentRoadWay = otherCar.currentRoadWay + 1; } }
        if (otherCar != null)
        {
            if (otherCar.speedBackup + 0.5f < speedBackup)
            {
                if (currentRoadWay == roadSize - 1 && !Physics2D.Raycast(leftStart, transform.up, 3.5f) && !Physics2D.Raycast(leftStart + (transform.up * 3.5f), -to, 14f))
                {
                    currentRoadWay = otherCar.currentRoadWay - 1;
                }
                else
                {
                    if (!Physics2D.Raycast(rightStart, -transform.up, 3.5f) && !Physics2D.Raycast(rightStart + ((-transform.up) * 3.5f), -to, 14f))
                    {
                        currentRoadWay = otherCar.currentRoadWay + 1;
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (GameMaster.GM.selected == transform)
        {
            SpeedGraph.SG.SetPoint(speed + Random.Range(-0.2f, 0.1f));
        }
    }
    Color32 SpeedColor(float val)
    {
        if (val == 0) return new Color32(160, 10, 40, 255);
        if (val < 4) return new Color32(255, 64, 0, 255);
        if (val < 8) return new Color32(255, 192, 0, 255);
        if (val < 12) return new Color32(255, 255, 0, 255);
        if (val < 16) return new Color32(128, 255, 0, 255);
        if (val < 20) return new Color32(64, 192, 64, 255);
        return new Color32(60, 150, 75, 255);
    }
    #region OnTriggerEnter
    Collider2D previousCollider2D;
    float previousTriggerTime = 0;
    void OnTriggerEnter2D(Collider2D other)
    {
        //prevent entering same trigger in next 5 seconds
        if (previousTriggerTime + 5 < Time.time)
        {
            previousTriggerTime = Time.time;
            previousCollider2D = null;
        }
        if (other.tag == "Force")
        {
            currentRoadWay = other.GetComponent<LaneChangeForce>().ForceLane;
        }
        if (other.tag == "NM" && previousCollider2D != other)
        {
            previousCollider2D = other;
            int index = other.GetComponent<RoadMaster>().Get();
            road = other.GetComponent<RoadMaster>().RoadList[index].roadWaypath; //RoadList[0].roadWaypath;
            MaxSpeed = other.GetComponent<RoadMaster>().RoadList[index].speedLimit;
            roadSize = other.GetComponent<RoadMaster>().RoadList[index].pathSize;
            //currentRoadWay = Random.Range(0, roadSize);
            //currentRoadWay = Mathf.Clamp(currentRoadWay, 0, (int)(roadSize - 1));
            i = 0;

            if (road != null)
            {
                if (road.Capacity != 0)
                {
                    from = transform.position + new Vector3(-to.y, to.x) * currentRoadWay * 3.5f;

                    Vector3 to2 = road[i + 1].position - road[i].position;
                    to2.Normalize();
                    Vector3 to2Inv = new Vector3(to2.y, -to2.x) * currentRoadWay * 3.5f;

                    changeNode = road[i].position + to2Inv;
                    to = (changeNode - transform.position).normalized;
                }
            }
        }
    }
    #endregion
    #region Debug
    void OnDrawGizmos()
    {
        if (hitInfoL.point != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hitInfoL.point, 0.75f);
        }
        if (hitInfoR.point != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(hitInfoR.point, 0.75f);
        }
    }
    //void OnDrawGizmos()
    //{
    //    if (from != null && to != null)
    //    {
    //        Gizmos.color = Color.yellow;
    //        Gizmos.DrawSphere(from, 0.5f);
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawSphere(from + to * 2, 0.5f);
    //        Gizmos.color = Color.magenta;
    //        if (road != null)
    //        {
    //            if (i == road.Capacity - 1)
    //            {
    //                Gizmos.DrawSphere(road[i].position + new Vector3(to.y, -to.x) * roadSize * 3.5f, 3.5f);
    //            }
    //            else
    //            {
    //                if (i < road.Capacity)
    //                {
    //                    Vector3 to1 = to;
    //                    Vector3 to1Inv = new Vector3(to.y, -to.x) * roadSize * 3.5f;
    //                    Vector3 to2 = road[i + 1].position - road[i].position;
    //                    Vector3 to2Inv = new Vector3(to2.y, -to2.x) * roadSize * 3.5f;
    //                    to1.Normalize();
    //                    to2.Normalize();
    //                    to1Inv.Normalize();
    //                    to2Inv.Normalize();
    //                    to1Inv *= roadSize * 3.5f;
    //                    to2Inv *= roadSize * 3.5f;
    //                    Vector3 test = LineIntersectionPoint(road[i].position + to1Inv, road[i].position + to1Inv + to1, road[i + 1].position + to2Inv, road[i + 1].position + to2Inv + to2);
    //                    if (i == 0)
    //                    {
    //                        test = road[i].position + to2Inv;
    //                    }
    //                    Gizmos.DrawSphere(test, 3.5f);
    //                }
    //            }
    //        }
    //    }
    //}
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
