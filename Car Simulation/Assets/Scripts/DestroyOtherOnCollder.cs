using UnityEngine;
using System.Collections;

public class DestroyOtherOnCollder : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D obj)
    {

        Destroy(obj.gameObject);
    }
}
