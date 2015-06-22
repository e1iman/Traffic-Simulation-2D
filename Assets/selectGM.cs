using UnityEngine;
using System.Collections;

public class selectGM : MonoBehaviour {
    void OnMouseDown()
    {
        GameMaster.GM.selected = transform;
        GameMaster.GM.infoPanel.Enable();
    }
}
