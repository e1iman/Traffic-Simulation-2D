using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class TrafficLightMaster : MonoBehaviour {
    [SerializeField] GameObject UIelementToInstatiate;
    [SerializeField] List<Stage> phaseList;
    [SerializeField] int ChildrenLenth = 0;
    [SerializeField] bool reset = true;
    public int phaseSize = 5;
    [Range(0,5)]
    public int phaseCurrent = 0;

    [SerializeField] bool[] fuzzyList;

    void Start()
    {
        #region Initializate
        if (reset)
        {
            ChildrenLenth = transform.childCount;
            for (int i = 0; i < phaseSize; i++)
            {
                phaseList.Add(new Stage(ChildrenLenth, 0));
            }

            fuzzyList = new bool[ChildrenLenth];
            for (int i = 0; i < fuzzyList.Length; i++)
            {
                fuzzyList[i] = false;
            }
        }
        UIstage = phaseCurrent;
        //instatiate UI pannels with nomber shown above each trafficLight
        #endregion
        DrawIcons();
        DrawMenu();
        StartCoroutine(TrafficMasterControler());
    }
    void Update()
    {
        phaseCurrent = Mathf.Clamp(phaseCurrent, 0, phaseList.Count-1);
        
        if (GameMaster.GM.selected == transform)
        {
            GameMaster.GM.infoPanel.text = InfoString();
        }
    }

    #region SendInfoPanelText
    void OnMouseUp()
    {
        GameMaster.GM.selected = transform;
        GameMaster.GM.infoPanel.Enable();
        //GameMaster.GM.infoPanel.text = InfoString();
    } 
    #endregion
    #region DrawMenu
    [SerializeField]
    GameObject UIInputBox;
    [SerializeField]
    GameObject toggleButton;
    [SerializeField]
    GameObject togglePanel;
    [SerializeField]
    GameObject slider;
    [SerializeField]
    GameObject inputGreenTime;
    [SerializeField]
    GameObject inputPanel;
    [SerializeField]
    GameObject textPanel;
    [SerializeField]
    GameObject textScore;
    [SerializeField]
    GameObject flowInfo;
    Toggle[] togArr;
    InputField greenInput;
    int UIstage = 0;

    void DrawMenu()
    {
        GameObject UIbox = (GameObject)Instantiate(UIInputBox);
        UIbox.transform.SetParent(transform);
        UIbox.transform.localPosition = -Vector3.forward * 10;
        togglePanel = UIbox.transform.FindChild("TogglePanel").gameObject;
        inputPanel = UIbox.transform.FindChild("InputPanel").gameObject;
        textPanel = UIbox.transform.FindChild("TextPanel").gameObject;

        togArr = new Toggle[ChildrenLenth];

        GameObject text = (GameObject)Instantiate(textScore);
        text.transform.SetParent(textPanel.transform,false);
        text.GetComponent<Text>().text = "Stage: " + phaseCurrent + " <color=black>(0 s)</color>";

        GameObject slidField = (GameObject)Instantiate(slider);
        slidField.transform.SetParent(inputPanel.transform, false);
        slidField.GetComponent<Slider>().minValue = 0;
        slidField.GetComponent<Slider>().maxValue = phaseSize-1;
        slidField.GetComponent<Slider>().value = phaseCurrent;
        slidField.GetComponent<Slider>().onValueChanged.AddListener(
            (call) =>
            {   UIstage = (int)call;
                float totalGreenTime = 0;
                foreach (var el in phaseList) { totalGreenTime += el.greentime; }
                text.GetComponent<Text>().text = "Stage: " + UIstage.ToString() + " <color=black>(" + totalGreenTime + " s)</color>";
                ReDrawUI();
            }
            );

        GameObject inpField = (GameObject)Instantiate(inputGreenTime);
        inpField.transform.SetParent(inputPanel.transform, false);
        inpField.GetComponent<InputField>().text = phaseList[phaseCurrent].greentime.ToString();
        inpField.GetComponent<InputField>().onEndEdit.AddListener(
                (value) => { ChangeGreenTime(value); }
            );
        greenInput = inpField.GetComponent<InputField>();

        for (int i = 0; i < ChildrenLenth; i++)
        {
            GameObject toggle = (GameObject)Instantiate(toggleButton);
            toggle.transform.SetParent(togglePanel.transform, false);
            toggle.GetComponentInChildren<Text>().text = i.ToString();
            toggle.GetComponent<Toggle>().isOn = phaseList[phaseCurrent].lightGreen[i];
            int index = i;
            toggle.GetComponent<Toggle>().onValueChanged.AddListener(
                (value) => { ChangeBool(index, value); }
                );
            togArr[i] = toggle.GetComponent<Toggle>();

            GameObject fuzzyToggle = toggle.transform.FindChild("Label").gameObject;
            //fuzzyToggle.GetComponent<Text>().color = Color.red;
            fuzzyToggle.GetComponent<Toggle>().onValueChanged.AddListener((value) =>
            {
                if (value == true)
                {
                    fuzzyToggle.GetComponent<Text>().color = Color.magenta;
                    fuzzyList[index] = true;
                }
                else
                {
                    fuzzyToggle.GetComponent<Text>().color = Color.black;
                    fuzzyList[index] = false;
                }
            });
        }
    }
    #endregion
    #region DrawIcons
    void DrawIcons()
    { //instatiate UI pannels with nomber shown above each trafficLight
        int index = 0;
        foreach (Transform child in transform)
        {
            GameObject uiClone = (GameObject)Instantiate(UIelementToInstatiate, new Vector3(child.position.x, child.position.y, child.position.z + (-10)), Quaternion.EulerAngles(new Vector3(-45, 0, 0)));
            
            //GameObject uiCloneFlowInfo = (GameObject)Instantiate(flowInfo, new Vector3(child.position.x, child.position.y, child.position.z), Quaternion.identity);
            //GameObject uiCloneFI2 = (GameObject)Instantiate(UIelementToInstatiate, new Vector3(uiCloneFlowInfo.transform.position.x, uiCloneFlowInfo.transform.position.y, uiCloneFlowInfo.transform.position.z + (-10)), Quaternion.EulerAngles(new Vector3(-45, 0, 0)));
            //uiCloneFI2.GetComponentInChildren<Text>().text = index.ToString();
            //uiCloneFI2.transform.SetParent(uiCloneFlowInfo.transform);

            uiClone.GetComponentInChildren<Text>().text = index.ToString();
            uiClone.transform.SetParent(child);
            index++;
        }
    }
    #endregion
    void ChangeBool(int i, bool value)
    {
        Debug.Log("i: " + i + " : " + value + " UIstage: " + UIstage);
        phaseList[UIstage].lightGreen[i] = value;
    }
    void ChangeGreenTime(string text)
    {
        Debug.Log("new GreenTime: " + text + " Stage: " + UIstage.ToString());
        try
        {
            phaseList[UIstage].greentime = Convert.ToInt16(text);
        }
        catch (OverflowException)
        {
            Debug.LogWarning("OVERFLOW EXCEPTION OCCURED");
            phaseList[UIstage].greentime = 999;
        }
    }
    void ReDrawUI()
    {
        for (int i = 0; i < ChildrenLenth; i++)
        {
            togArr[i].isOn = phaseList[UIstage].lightGreen[i];
        }
        greenInput.text = phaseList[UIstage].greentime.ToString();
        greenInput.transform.GetComponent<Image>().color = (phaseList[UIstage].greentime == 0 ) ? new Color(0.8f, 0.8f, 0.8f) : Color.white;
    }
    float StringTime = 0;
    String InfoString()
    {
        return "<b>Traffic Light Master</b>\n<color=yellow>Phase:</color> " + (phaseCurrent) + "\nTotalPhase: " + phaseList.Count + "\nTraffc Lights count: " + ChildrenLenth + "\nphase time: " + StringTime + " s";
    }

    IEnumerator TrafficMasterControler()
    {
        int maxQ;
        int volume;
        float time;
        float tmpFuzzyTime;
        while (true)
        {
            do
            {
                phaseCurrent++;
            }
            while (phaseCurrent != phaseSize && phaseList[phaseCurrent].greentime == 0);

            phaseCurrent = (phaseCurrent == phaseSize) ? 0 : phaseCurrent;

            for (int i = 0; i < ChildrenLenth; i++)
            {
                transform.GetChild(i).GetComponent<TrafficLight>().SetGreen(phaseList[phaseCurrent].lightGreen[i]);
            }

            maxQ = 0;
            volume = 0;
            tmpFuzzyTime = 0;

            time = phaseList[phaseCurrent].greentime;
            

            for (int i = 0; i < ChildrenLenth; i++)
            {
                if (phaseList[phaseCurrent].lightGreen[i] == true && fuzzyList[i] == true)
                {
                    try
                    {
                        int tmpQ = transform.GetChild(i).GetComponentInChildren<QueueInfo>().queue;
                        int tmpV = transform.GetChild(i).GetComponentInChildren<QueueInfo>().volume;
                        if (maxQ < tmpQ)
                        {
                            maxQ = tmpQ;
                            volume = tmpV;
                        }
                    }
                    catch
                    {
                        Debug.LogWarning("CantFind QueueInfo in " + transform.GetChild(i).ToString());
                    }
                }
            }
            if (maxQ > 0)
            {
                tmpFuzzyTime = FuzzySem.fs.Calc(volume, maxQ);
            }

            if (time < tmpFuzzyTime)
            {
                time = tmpFuzzyTime;
            }
            StringTime = time;
            yield return new WaitForSeconds(time);
        }
    }
}

[System.Serializable]
public class Stage{
    public Stage(int size, int _greentime)
    {
        lightGreen = new bool[size];
        greentime = _greentime;
    }
    public bool[] lightGreen;
    public int greentime;
}