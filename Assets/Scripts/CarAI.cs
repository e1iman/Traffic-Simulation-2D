using UnityEngine;
using System.Collections;

public class CarAI : MonoBehaviour {

	public Transform target;
	public float maxMoveSpeed = 10;//10 meters per second = 36 kilometers per hour
    public float brakeSpeed = 2;
	public float turnSpeed = 4.5f;
    public float raycastPoint = 1;
    public float raycastDistance = 4;

    public Road road;
	//private Vector3 moveDirection;
    public float step = 2.0f;
    float moveSpeed;
    float rotateSpeed;
    Vector3 from;
    Vector3 to;
    SpriteRenderer sprite;
    Color colorStart;
    RaycastHit2D outRay;

	void Start () {
        moveSpeed = maxMoveSpeed;
        colorStart = new Color(Random.Range(0.1f, 1), Random.Range(0.1f, 1), Random.Range(0.1f, 1));
        GetComponent<SpriteRenderer>().color = colorStart;
        GameMaster.GM.objList.Add(gameObject);
		if(!target)
		{
			target = GameMaster.GM.GetRandomNode();
			Debug.LogWarning("Recieved random target");
		}
        road = target.GetComponent<Node>().nextNode[0];
        from = transform.position;

        UpdateDirectionVector();
	}

    public void SetNextTarget(Road rd, Vector3 _from)
    {
        from = _from;

        road = rd;
        target = rd.destination;

        UpdateDirectionVector();
    }

	void Update () {

        //get next Road 
        if(Vector3.Distance(transform.position, target.position)<step)
        {
            //Vector3 ppl = road.destination.transform.position;
            Vector3 ppl = target.position;
            SetNextTarget(target.GetComponent<Node>().NextTarget(), ppl);
        }

        if (GameMaster.GM.debug)
        {
            Debug.DrawLine(from, from + to * step, Color.black);
            Debug.DrawLine(transform.position, from, Color.blue);
        }

        if(Vector3.Distance(transform.position, from) < step)
        {
            from += to*step;
        }
        
        //check for obstacle infron
        outRay = Physics2D.Raycast(transform.position + transform.right * raycastPoint, transform.right, raycastDistance);
        if (outRay.collider != null)
        {
            switch (outRay.collider.tag)
            {
                case "Car":
                    GetComponent<SpriteRenderer>().color = Color.red;
                    moveSpeed = 0;
                    break;

                case "TrafficLight":
                    GetComponent<SpriteRenderer>().color = Color.yellow;
                    moveSpeed = 0;
                    break;

                default:
                    GetComponent<SpriteRenderer>().color = colorStart;
                    moveSpeed = maxMoveSpeed;
                    break;
            }
        }
        else
        {
            GetComponent<SpriteRenderer>().color = colorStart;
            moveSpeed += 15 * Time.deltaTime;
        }
        moveSpeed = Mathf.Clamp(moveSpeed, 0, maxMoveSpeed);

        //move
		transform.position += (transform.right * moveSpeed * Time.deltaTime);

		//rotate
		//Vector3 dir = target.position - transform.position;
        Vector3 dir = from - transform.position;
		float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
		//transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        rotateSpeed = turnSpeed * (moveSpeed / maxMoveSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotateSpeed);//!!<<<<<<<<<<<  WAS TURN SPEED
    }

    void UpdateDirectionVector()
    {
        to = target.position - from;
        to.z = 0;
        to.Normalize();
    }

    void OnDrawGizmosSelected()
    {
        if (from != null && to != null)
        {
            Gizmos.DrawRay(transform.position + transform.right * raycastPoint, transform.right);
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(from, 0.5f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(from + to*2, 0.5f);
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(outRay.point, 0.6f);
        }
    }

}
