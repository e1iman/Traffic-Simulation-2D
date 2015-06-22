using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class infoPanelsUi : MonoBehaviour {
    public Button button;
    public void Click()
    {
     if(GameMaster.GM.infoMode)
     {
         gameObject.active = (gameObject.active) ? false : true;
         button.image.color = (gameObject.active) ? Color.white : new Color(0.3f, 0.3f, 0.3f);
     }
    }
}
