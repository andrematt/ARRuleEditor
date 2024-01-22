using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveRuleIconScript : MonoBehaviour
{
    public Canvas saveRuleIconCanvas;
    public Canvas ruleMapCanvas;
    public Canvas saveRuleCanvas;
    public Button saveRuleButton;
    // Start is called before the first frame update
    void Start()
    {
        saveRuleIconCanvas = GameObject.Find("SaveRuleIconCanvas").GetComponent<Canvas>();
        saveRuleCanvas = GameObject.Find("SaveRuleCanvas").GetComponent<Canvas>();
        saveRuleIconCanvas.enabled = false;
        
        saveRuleButton.onClick.AddListener(delegate
        {
            manageSaveClick();
        });
        
    }

    public void manageSaveClick()
    {
        //ScreenLog.Log("HELLO SAVEBUTTON :)");
        saveRuleCanvas.enabled = true;
        SaveRuleCanvasScript saveRuleCanvasScript = FindObjectOfType<SaveRuleCanvasScript>();  //
        ruleMapCanvas.enabled = false;
        saveRuleCanvasScript.updateMyNl();
    }

    public void enableSaveButton()
    {
        saveRuleIconCanvas.enabled = true;
    }
    public void disableSaveButton()
    {
        saveRuleIconCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
