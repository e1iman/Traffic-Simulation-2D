using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AgentCount : MonoBehaviour {
    public List<Transform> agentList = new List<Transform>();
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Car"))
        {
            agentList.Add(other.transform);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Car"))
        {
            agentList.Remove(other.transform);
        }
    }


}
