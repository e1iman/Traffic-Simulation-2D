using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour {
    public List<Road> nextNode;
    public Color color ;//= new Color(Random.value, Random.value, Random.value);
	BoxCollider2D boxCol;
    void Awake()
    {
        color = new Color(Random.value, Random.value, Random.value);
		boxCol = GetComponent<BoxCollider2D>(); 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Col name: " + other.name);
        int i = Random.Range(0, nextNode.Count);
        //if (nextNode.Count > 1) Debug.Log(i);
        //other.GetComponent<CarAI>().SetNextTarget(nextNode[i], transform.position);
    }

    void OnDDrawGizmos()
    {
        Gizmos.color = color;
        if(GameMaster.GM.debug)
            nextNode.ForEach(Dr);
    }
    void Dr(Road rd)
    {
        Gizmos.DrawLine(transform.position, rd.destination.transform.position);
    }
    public Road NextTarget()
    {
        int i = Random.Range(0, nextNode.Count);
        //if (nextNode.Count > 1)
        //    Debug.Log(i);
        return nextNode[i];
    }
}