using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class DebugStateScript : MonoBehaviour {
    public Unit unit;

    private Text text;

    // Use this for initialization
    void Start () {
        text = GetComponent<Text>();
    }
    
    // Update is called once per frame
    void Update () {
        //if (Input.anyKey)
        //{
        text.text = unit.UnitState.StateSummary();
        //}
    }
}
