using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public struct Phase
{
    public List<TrafficLight> lightPhaseList;
    public float greenInterval;
    public float redInteval;
}

public class LightMaster : MonoBehaviour {
    [SerializeField] GameObject UIelement;

    public int haha = 0;
    public List<TrafficLight> lightList;
    public List<Phase> phaseList;
    [Range(0,5)]
    public int state = 0;

    void Start()
    {
        StartCoroutine(Go(0));
    }
    void Update()
    {
        //state = Mathf.Clamp(state, 0, phaseList.Count-1);


        for (int i = 0; i < lightList.Count; i++)
        {
            lightList[i].green = phaseList[state].lightPhaseList.Contains(lightList[i]);
        }
        if (GameMaster.GM.selected == transform)
        {
            GameMaster.GM.infoPanel.text = "<b> Traffic Light </b> \n <color=yellow>Phase:</color> " + (state + 1) + "\n TotalPhase:" + phaseList.Count;
        }
    }

    IEnumerator Go(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(phaseList[state].greenInterval);
            state += 1;
            if (state >= phaseList.Count) state = 0;
        }
    }

    //void OnGUI()
    //{
    //    for (int i = 0; i < phaseList.Count; i++)
    //    {
    //        if (GUI.Button(new Rect(10, 70 + i * 30, 50, 30), phaseList[i].greenInterval.ToString()))
    //        {
    //            Phase temp = phaseList[i];
    //            temp.greenInterval += 1;
    //            phaseList[i] = temp;
    //        }
    //    }
    //}
        //foreach (LevelSerializer.SaveEntry sg in LevelSerializer.SavedGames[LevelSerializer.PlayerName]) {
        //    if (GUILayout.Button(sg.Caption)) {
        //        DateTime t = DateTime.Now;
        //        LevelSerializer.LoadNow(sg.Data);
        //        if (logProgress) {
        //            Debug.Log(string.Format("Loaded in: {0:0.000} seconds", (DateTime.Now - t).TotalSeconds));
        //        }
        //        Time.timeScale = 1.0f;
        //        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        //    }
        //}
    GameObject clone = null;
    void OnMouseUp()
    {
        //GameMaster.GM.inputPanel.Enable();
        //GameMaster.GM.lightMaster = this;

        GameMaster.GM.selected = transform;
        GameMaster.GM.infoPanel.Enable();
        GameMaster.GM.infoPanel.text = "<b> Traffic Light </b> \n <color=yellow>Phase:</color> " + (state + 1) + "\n TotalPhase:" + phaseList.Count;
        
        if (clone == null)
        {
            clone = (GameObject)Instantiate(UIelement);
        }
        
        clone.transform.SetParent(transform);
    }
    
}
