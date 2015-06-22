using UnityEngine;
public class GizmosIcon : MonoBehaviour {
    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "test.png", true);
    }
}
