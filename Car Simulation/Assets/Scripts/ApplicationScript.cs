using UnityEngine;
using System.Collections;
using System.Threading;
using System;

public class ApplicationScript : MonoBehaviour {
    //// This method will be called when the thread is started.
    //public void DoWork()
    //{
    //    while (!_shouldStop)
    //    {
    //                }
    //    Debug.Log("worker thread: terminating gracefully.");
    //}
    //IEnumerator IEDoWork()
    //{
    //    while (!_shouldStop)
    //    {
    //        Debug.Log("worker thread: working...");
    //        yield return new WaitForEndOfFrame();
    //    }
    //}
    //public void RequestStop()
    //{
    //    _shouldStop = true;
    //}
    //bool _shouldStop = false;

    //void Start()
    //{
    //    //Application.targetFrameRate = 60;
    //    Debug.Log("ThreadStart: " + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
    //        StartCoroutine("IEDoWork");
    //    Debug.Log("ThreadEnd: " + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);

    //    Debug.Log("Courotine Start: " + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
    //    Thread tmp = new Thread(DoWork);
    //        tmp.Start();
    //    Debug.Log("Courotine End: " + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
    //    RequestStop();
    //    tmp.Abort();
        
    //}
    public void ReloadLevel()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
	public void ExitApplication () {
        Application.Quit();
	}
}
