using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapIconScript : MonoBehaviour
{
    public Canvas ruleMapCanvas;
    public Button mapButton;
    public TempRule tempRuleScript;
    // Start is called before the first frame update
    void Start()
    {
        tempRuleScript = FindObjectOfType<TempRule>(); 
        mapButton.onClick.AddListener(delegate
        {
            manageMapClick();
        });
        
    }

    public void manageMapClick()
    {
        if (ruleMapCanvas.enabled == false)
        {
            ruleMapCanvas.enabled = true;
            //MapCanvasScript mapCanvasScript = ruleMapCanvas;
            MapCanvasScript mapCanvasScript = GameObject.Find("RuleMapCanvas").GetComponent<MapCanvasScript>();
            mapCanvasScript.addECA();
        }
        else
        {
            ruleMapCanvas.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
