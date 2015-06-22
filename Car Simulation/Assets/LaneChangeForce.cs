using UnityEngine;
using System.Collections;

public class LaneChangeForce : MonoBehaviour {
    [Range(0, 5), SerializeField]
    int forceLane = 0;
    [SerializeField]
    bool random = false;
    public int ForceLane
    {
        get
        {
            if (random)
            {
                int tmp = Random.Range(0, forceLane + 1);
                return tmp;
            }
            return forceLane;
        }
    }
    void Awake()
    {
        Destroy(GetComponent<SpriteRenderer>());
    }
}
