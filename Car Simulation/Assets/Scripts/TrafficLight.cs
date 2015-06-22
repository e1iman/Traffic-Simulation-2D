using UnityEngine;
using System.Collections;

public class TrafficLight : MonoBehaviour {

    public bool green = false;

    SpriteRenderer rendMat;
    BoxCollider2D boxCol;
    MeshRenderer mr;
    public Color _red = new Color(0.7f, 0.08f, 0.2f, 0.2f);
    public Color _green = new Color(0.08f, 0.7f, 0.2f, 0.2f);
    void Start()
    {
        rendMat = transform.GetComponent<SpriteRenderer>();
        boxCol = transform.GetComponent<BoxCollider2D>();
        mr = transform.GetComponentInChildren<MeshRenderer>();
    }

    void Update()
    {
        if (green)
        {
            rendMat.color = _green;
            boxCol.enabled = false;
            if (mr)
            {
                mr.material.color = _green;
            }
        }
        else
        {
            rendMat.color = _red;
            boxCol.enabled = true;
            if (mr)
            {
                mr.material.color = _red;
            }
        }
    }
    public void SetGreen(bool inp)
    {
        green = inp;
    }


    //public float offset = 0;
    //public float greenTime = 20;
    //public float yellowTime = 2;
    //public float redTime = 10;
    //public Transform sprite;
    //SpriteRenderer rendMat;
    //BoxCollider2D boxCol;
    //void Start () {
    //    rendMat = sprite.GetComponent<SpriteRenderer>();
    //    boxCol = sprite.GetComponent<BoxCollider2D>();
    
    //    print("Starting " + Time.time);
    //    StartCoroutine(Test(2.0F));
    //    print("Before WaitAndPrint Finishes " + Time.time);
    //}
    //IEnumerator Test(float waitTime) {
    //    yield return new WaitForSeconds(offset);
    //    while (true)
    //    {
    //        rendMat.color = Color.green;
    //        boxCol.enabled = false;
    //        yield return new WaitForSeconds(greenTime);
    //        rendMat.color = Color.yellow;
    //        boxCol.enabled = true;
    //        yield return new WaitForSeconds(yellowTime);
    //        rendMat.color = Color.red;
    //        boxCol.enabled = true;
    //        yield return new WaitForSeconds(redTime);
    //    }
    //}
}
