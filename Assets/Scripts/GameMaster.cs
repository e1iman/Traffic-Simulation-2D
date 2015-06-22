using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public struct Road
{
    public int speedLimit;
    public int pathSize;
    public Transform destination;
    public Road(int spdLim, int pthSize, Transform dest)
    {
        speedLimit = spdLim;
        pathSize = pthSize;
        destination = dest;
    }
}

public class GameMaster : MonoBehaviour {
    public int carsAlive = 0;
    public Text carsAliveText;

    public static GameMaster GM;
	public GameObject Nodes;
    public List<Transform> nodes;
    public bool debug = true;
    public bool inputFocus = false;

    public List<GameObject> objList;
    public Object obj;
    public Image imgPauseFrame;
    public Image imgTimeSpeed;
    public Sprite[] imgTimeArr;
    public InfoPanel infoPanel;
    public Transform selected;
    public InputPanel inputPanel;
    public LightMaster lightMaster;
    public bool infoMode = true;
    public bool carSpeedMode = true;
    float timeScale = 1;
    AudioSource audio;
	void Awake()
	{
		if(GM != null)
			GameObject.Destroy(GM);
		else
			GM = this;
		
		//DontDestroyOnLoad(this);

		foreach (Transform child in Nodes.transform)
		{
			if (child.tag == "Node")
			{
				nodes.Add(child.transform);
			}
		}
        Time.timeScale = 1;
        audio = GM.GetComponent<AudioSource>();
        audio.clip = (AudioClip)Resources.Load("click", typeof(AudioClip));
    }
    void Update()
    {
        carsAliveText.text = carsAlive.ToString();
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F))
        {
                Debug.Log("SPAWNING");
                Instantiate(obj);
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.D) && objList.Count > 0)
        {
            Debug.Log("Deleting");
            Destroy(objList[0]);
            objList.RemoveAt(0);
        }
        if (Input.GetButtonDown("TimeScale0"))
        {
            if (Time.timeScale != 0)
            {
                timeScale = Time.timeScale;
                Time.timeScale = 0;
                imgTimeSpeed.color = Color.gray;
            }
            else
            {
                Time.timeScale = timeScale;
                imgTimeSpeed.color = Color.white;
            }
        }
        if (Time.deltaTime == 0)
        {
            imgPauseFrame.color = Color.red;
        }
        else
        {
            imgPauseFrame.color = Color.clear;
        }


        //inputFocus = ((EventSystem.current.currentSelectedGameObject != null)) ? true : inputFocus;
        //inputFocus = (EventSystem.current.IsPointerOverGameObject()) ? true : false;

        if (Input.GetButtonDown("TimeScale1") && EventSystem.current.currentSelectedGameObject == null) //Input.GetKeyDown(KeyCode.Alpha1)
        {
            greenTime();
        }
        if (Input.GetButtonDown("TimeScale2") && EventSystem.current.currentSelectedGameObject == null) //Input.GetKeyDown(KeyCode.Alpha2)
        {
            yellowTime();
        }
        if (Input.GetButtonDown("TimeScale3") && EventSystem.current.currentSelectedGameObject == null) //Input.GetKeyDown(KeyCode.Alpha3) 
        {
            redTime();
        }
    }
	public Transform GetRandomNode()
	{
		return nodes[0];
	}

    public void greenTime()
    {
        Time.timeScale = 1;
        imgTimeSpeed.sprite = imgTimeArr[0];
        imgTimeSpeed.color = Color.white;
    }
    public void yellowTime()
    {
        Time.timeScale = 2;
        imgTimeSpeed.sprite = imgTimeArr[1];
        imgTimeSpeed.color = Color.white;
    }
    public void redTime()
    {
        Time.timeScale = 3;
        imgTimeSpeed.sprite = imgTimeArr[2];
        imgTimeSpeed.color = Color.white;
    }
    public void click()
    {
        if (audio != null)
            audio.Play();
    }
    public void toggleInfoMode()
    {
        infoMode = (infoMode) ? false : true;
    }
    public void toggleCarSpeedMode()
    {
        carSpeedMode = (carSpeedMode) ? false : true;
    }
}
