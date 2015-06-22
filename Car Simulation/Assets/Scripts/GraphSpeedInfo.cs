using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GraphSpeedInfo : MonoBehaviour
{
    public float recordRate;
    public Transform displayGraphPoint;
    public bool drawLine = false;
    public int size = 50;

    public float[] arr;
    private float nextTime;
    private Vector3 prevPos;
    private int cursor;

    LineRenderer lr;

    void Start()
    {
        cursor = 0;
        arr = new float[size];
        prevPos = transform.position;
        nextTime = Time.time + recordRate;
        lr = displayGraphPoint.GetComponent<LineRenderer>();
        lr.SetVertexCount(size);
    }

    void Update()
    {
       if(Time.time > nextTime)
       {
           nextTime = Time.time + recordRate;
           float dist = Vector3.Distance(transform.position, prevPos);
           arr[cursor] = (dist/recordRate);
           prevPos = transform.position;
           cursor++;
           if (cursor >= size) cursor = 0;
       }

       if (drawLine) showGraph();
    }

    void showGraph()
    {
        for (int i=0; i < size; i++)
        {
            Vector3 pos = new Vector3(i,arr[i], 0);
            lr.SetPosition(i, pos);
        }
    }
}
