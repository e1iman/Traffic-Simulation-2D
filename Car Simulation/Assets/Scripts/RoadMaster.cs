using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RoadMaster : MonoBehaviour {
    public List<RoadWay> RoadList;
    public RoadWay RoadWay
    {
        get
        {
            return RoadList[Random.Range(0,(int)RoadList.Capacity)];
        }
    }
    public int Get()
    {
      return Random.Range(0, (int)RoadList.Capacity);
    }

    #region Editor
    public void Fix()
    {
        var tempList = transform.Cast<Transform>().ToList();
        //tempList = tempList.Where(x => x != null).ToList();
        for (var i = RoadList.Count - 1; i > -1; i--)
        {
            if (RoadList[i] == null)
                RoadList.RemoveAt(i);
        }

        foreach (var child in tempList)
        {
            RoadList.Add(child.GetComponent<RoadWay>());
            child.localPosition = Vector3.zero;
        }
        RoadList = RoadList.Distinct().ToList();
    }
    #endregion
}
