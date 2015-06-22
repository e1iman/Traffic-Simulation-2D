using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QueueInfo : MonoBehaviour {
    [SerializeField]
    List<float> dc;
    public int queue = 0;
    public int volume = 0;
    int maxQueue = 0;
    int maxQueue5m = 0;
    float maxQueue5mLastTime = 0;
	void Start () {
        dc = new List<float>();
        this.GetComponent<BoxCollider2D>().offset = Vector2.one;
        this.GetComponent<BoxCollider2D>().offset = Vector2.zero;
	}
	
	void Update () {
        if (GameMaster.GM.selected == transform.parent)
        {
            GameMaster.GM.infoPanel.text = "<b> Detector </b> \nqueue: " + queue + " veh\nvolume: " +
                volume + " veh/hr" + "\n" + (volume / 4) + " veh/15m" + "\nMax Queue: " + maxQueue + "\nMax Queue in last 5 min: " + maxQueue5m;
        }

            if (dc.Count > 0 && dc[0] < Time.time - 300)
            {
                dc.RemoveAt(0);
            }

        volume = dc.Count * 12;
	}

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Car")
        {
            queue++;
            if (maxQueue < queue)
            {
                maxQueue = queue;
            }

            if ((maxQueue5mLastTime + 300) < Time.time)
            {
                maxQueue5mLastTime = Time.time;
                maxQueue5m = 0;
            }
            if (maxQueue5m < queue)
            {
                maxQueue5m = queue;
            }
            dc.Add(Time.time);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Car")
        {
            queue--;
        }
    }

}
