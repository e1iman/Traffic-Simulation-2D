using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class InputPanel : MonoBehaviour {

    public InputField input;
    public void OnSubmit()
    {
        if (input.text != "")
        {
            if (GameMaster.GM.lightMaster != null)
            {
                try
                {
                    GameMaster.GM.lightMaster.haha = Convert.ToInt32(input.text);
                }
                catch (OverflowException)
                {
                    Debug.Log("OverflowExceptrion");
                    GameMaster.GM.lightMaster.haha = 999;
                }
            }
        }
    }
    public void Disable()
    {
        gameObject.SetActive(false);
    }
    public void Enable()
    {
        gameObject.SetActive(true);
    }
}
