using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class spawner : MonoBehaviour {

    public GameObject gameobject;
    [Range(0.5f, 20f)]
    public float spawnrate = 10;

    float nextSpawnTime;

    [SerializeField]
    GameObject UI;

	// Use this for initialization
	void Start () {
        nextSpawnTime = spawnrate;
        if (gameobject == null)
        {
            gameobject = (GameObject)Resources.Load("CAR");
        }
        DrawMenu();
	}
	
	// Update is called once per frame
	void Update () {
        if (nextSpawnTime < Time.time)
        {
                nextSpawnTime = Time.time + spawnrate;
                if (Physics2D.OverlapCircle(transform.position, 3) == null)
                {

                    var tmp = (GameObject)Instantiate(gameobject, transform.position, transform.rotation);

                    float random = Random.Range(0, 100);
                    //Debug.Log("random: " + random);
                    //Debug.Log("(verySlow * 100 / total) = " + (verySlow * 100 / total));
                    //Debug.Log("(slow * 100 / total) = " + (slow * 100 / total));
                    //Debug.Log("(medium * 100 / total) = " + (medium * 100 / total));
                    //Debug.Log("(fast * 100 / total) = " + (fast * 100 / total));
                    //Debug.Log("(veryFast * 100 / total) = " + (veryFast * 100 / total));

                    if (random <= (verySlow * 100 / total))
                    {
                        tmp.GetComponent<TestCar>().SetBackupSpeed(Random.Range(6f,10f));
                    }
                    else
                    {
                        if (random <= (slow * 100 / total) + (verySlow * 100 / total))
                        {
                            tmp.GetComponent<TestCar>().SetBackupSpeed(Random.Range(10f, 15f));
                        }
                        else
                        {
                            if (random <= (medium * 100 / total) + (slow * 100 / total) + (verySlow * 100 / total))
                            {
                                tmp.GetComponent<TestCar>().SetBackupSpeed(Random.Range(15f, 20f));
                            }
                            else
                            {
                                if (random <= (fast * 100 / total) + (medium * 100 / total) + (slow * 100 / total) + (verySlow * 100 / total))
                                {
                                    tmp.GetComponent<TestCar>().SetBackupSpeed(Random.Range(20f, 25f));
                                }
                                else
                                {
                                    tmp.GetComponent<TestCar>().SetBackupSpeed(Random.Range(25f, 30f));
                                }
                            }
                        }
                    }

                }
        }
	}
    float verySlow = 3;
    float slow = 20;
    float medium = 40;
    float fast = 20;
    float veryFast = 8;
    float total = 0;

    void DrawMenu()
    {
        if (UI != null)
        {
            UI = (GameObject)Instantiate(UI);
            UI.transform.SetParent(transform);
            UI.transform.localPosition = -Vector3.forward * 10;
            UI.transform.rotation = Quaternion.EulerAngles(new Vector3(-45, 0, 0));
            
            UI.GetComponentInChildren<Text>().text = "Intensity: <color=" + ColorRate((60 / spawnrate) * 15) + ">" + ((60 / spawnrate) * 15).ToString("F2") + "</color>\nTU / 15 min";
            UI.GetComponentInChildren<Slider>().onValueChanged.AddListener((value) => { spawnrate = value; UI.GetComponentInChildren<Text>().text = "Intensity: <color=" + ColorRate((60 / spawnrate) * 15) + ">" + ((60 / spawnrate) * 15).ToString("F2") + "</color>\nTU / 15 min"; });
            UI.GetComponentInChildren<Button>().onClick.AddListener(() => { nextSpawnTime = Time.time;});


            UI.transform.FindChild("SliderVerySlow").GetComponent<Slider>().value = (verySlow);
            UI.transform.FindChild("SliderSlow").GetComponent<Slider>().value = slow;
            UI.transform.FindChild("SliderMedium").GetComponent<Slider>().value = medium;
            UI.transform.FindChild("SliderFast").GetComponent<Slider>().value = fast;
            UI.transform.FindChild("SliderVeryFast").GetComponent<Slider>().value = veryFast;
            ReCalcTotal();

            UI.transform.FindChild("SliderVerySlow").GetComponent<Slider>().onValueChanged.AddListener((value) => { verySlow = value; ReCalcTotal(); });
            UI.transform.FindChild("SliderSlow").GetComponent<Slider>().onValueChanged.AddListener((value) => { slow = value; ReCalcTotal(); });
            UI.transform.FindChild("SliderMedium").GetComponent<Slider>().onValueChanged.AddListener((value) => { medium = value; ReCalcTotal(); });
            UI.transform.FindChild("SliderFast").GetComponent<Slider>().onValueChanged.AddListener((value) => { fast = value; ReCalcTotal(); });
            UI.transform.FindChild("SliderVeryFast").GetComponent<Slider>().onValueChanged.AddListener((value) => { veryFast = value; ReCalcTotal(); });
        }
    }
    void ReCalcTotal()
    {
        total = verySlow + slow + medium + fast + veryFast;        
        if (total == 0)
        {
            verySlow = 50;
            slow = 50;
            medium = 50;
            fast = 50;
            veryFast = 50;

            total = verySlow + slow + medium + fast + veryFast;

            UI.transform.FindChild("SliderVerySlow").GetComponent<Slider>().value = verySlow;
            UI.transform.FindChild("SliderSlow").GetComponent<Slider>().value = slow;
            UI.transform.FindChild("SliderMedium").GetComponent<Slider>().value = medium;
            UI.transform.FindChild("SliderFast").GetComponent<Slider>().value = fast;
            UI.transform.FindChild("SliderVeryFast").GetComponent<Slider>().value = veryFast;
        }

        UI.transform.FindChild("TextVerySlow").GetComponent<Text>().text = "very slow: " + (verySlow * 100 / total).ToString("F1");
        UI.transform.FindChild("TextSlow").GetComponent<Text>().text = "slow: " + (slow * 100 / total).ToString("F1");
        UI.transform.FindChild("TextMedium").GetComponent<Text>().text = "medium: " + (medium * 100 / total).ToString("F1");
        UI.transform.FindChild("TextFast").GetComponent<Text>().text = "fast: " + (fast * 100 / total).ToString("F1");
        UI.transform.FindChild("TextVeryFast").GetComponent<Text>().text = "very fast: " + (veryFast * 100 / total).ToString("F1");
        
    }
    string ColorRate(float val)
    {
        if (val > 452) return "#A00A28";
        if (val > 385) return "#FF4000";
        if (val > 318) return "#FFC000";
        if (val > 251) return "#FFFF00";
        if (val > 184) return "#80FF00";
        if (val > 117) return "#40C040";
        if (val > 50) return "#3C964B";

        return "#3C80DD";
    }
}
