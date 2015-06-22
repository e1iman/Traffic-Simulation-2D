using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AvSpeed : MonoBehaviour {

    public List<Transform> carList = new List<Transform>();
    public List<TestCar> testCarList = new List<TestCar>();
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Car"))
        {
            carList.Add(other.transform);
            carCounter++;
            carPassed++;
            try
            {
                testCarList.Add(other.GetComponent<TestCar>());
            }
            catch { }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Car"))
        {
            carList.Remove(other.transform);
            try
            {
                testCarList.Remove(other.GetComponent<TestCar>());
            }
            catch { }
        }
    }
    public int timeClock = 5;
    float startTime;
    void Start()
    {
        StartCoroutine(CFlowInfo());
        StartCoroutine(CountWaitTime());

        startTime = Time.time;
    }
    public float sumSpeed = 0;
    public float avrSpeed = 0;
    int carCounter = 0;
    public int intensity = 0;
    public float avIntensity = 0;
    int k = 0;
    string outputStr;
    float totalWaitTime = 0;
    int carPassed = 0;
    float totalSumSpeed = 0;
    int totalSumSpeedCounter = 0;
    IEnumerator CFlowInfo()
    {
        while (true)
        {
            sumSpeed = 0;
            avrSpeed = 0;
            intensity = carCounter;
            avIntensity = (avIntensity * k + intensity) / (k + 1);
            carCounter = 0;
            foreach (Transform tr in carList)
            {
                sumSpeed += tr.GetComponent<TestCar>().speed;
            }
            avrSpeed = sumSpeed / carList.Count;
            
            Debug.Log(totalSumSpeed + "   " + avrSpeed);

            outputStr = "<b> FlowInfo </b>\nSpeed: " + avrSpeed.ToString("F1") + "\nintesity: " + 
                intensity + "/" + timeClock + "s" + "\nAv Intesity: " + (avIntensity * (float)(60 / timeClock) * 15).ToString("F1") 
                + "TU / 15m\nin " + ((Time.time - startTime) / 60).ToString("F1") + " m" + "\ntotal wait time: " + (totalWaitTime / 60f).ToString("F1") 
                + "m" + "\naverage wait time: " + (totalWaitTime / carPassed).ToString("F1") + "s";

            k++;
            if (GameMaster.GM.selected == transform.parent)
            {
                SpeedGraph.SG.SetPoint(avrSpeed);
            }
            
            yield return new WaitForSeconds(timeClock);
        }
    }

    IEnumerator CountWaitTime()
    {
        while (true)
        {
            foreach (TestCar tc in testCarList)
            {
                if (tc.speed == 0)
                {
                    totalWaitTime += Time.deltaTime;
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

    void Update()
    {
        if (GameMaster.GM.selected == transform.parent)
        {
            GameMaster.GM.infoPanel.text = outputStr;
        }
    }
}