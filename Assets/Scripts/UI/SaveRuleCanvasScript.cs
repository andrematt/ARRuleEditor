using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveRuleCanvasScript : MonoBehaviour
{
    public RuleChecks ruleChecks;
    public AnchorCreator anchorCreator;
    private Canvas saveRuleCanvas;
    public TempRule tempRuleScript;
    public string myNl;
    public Text myNlText;
    public TMP_InputField ruleNameInput;
    public Button saveRuleButton;
    public Button discardRuleButton;
    public RuleSaveAndLoad ruleSaveAndLoad;
    public NL nl;

    // Start is called before the first frame update
    void Start()
    {
        anchorCreator = FindObjectOfType<AnchorCreator>();  //
        ruleChecks = FindObjectOfType<RuleChecks>();  //
        ruleSaveAndLoad = FindObjectOfType<RuleSaveAndLoad>();  //
        nl = FindObjectOfType<NL>();  //
        saveRuleCanvas = GameObject.Find("SaveRuleCanvas").GetComponent<Canvas>();
        saveRuleCanvas.enabled = false;
        tempRuleScript = FindObjectOfType<TempRule>();
        
        saveRuleButton.onClick.AddListener(delegate
        {
            manageSaveClick();
        });
        discardRuleButton.onClick.AddListener(delegate
        {
            manageDiscardClick();
        });
    }

    public void manageSaveClick()
    {
        string ruleName = ruleNameInput.text;
        bool saveRuleCheck = ruleSaveAndLoad.saveRule(ruleName);
        if (saveRuleCheck)
        {
            //Deactivate the ActiveExclamationMarks
            anchorCreator.deactivateActiveExclamationMarks();
            // Reactiveate the exclamation marks
            anchorCreator.reactivateExclamationMarks(); // TODO TEST!!!
            //Destroy tempRule
            tempRuleScript.resetRule();
            // Check for the save rule icon
            ruleChecks.checkSaveRule();
            // Notify user
            ScreenLog.Log("SAVE OK");
        }
        else
        {
            // Notify user
            ScreenLog.Log("ERROR IN SAVING");
        }
        saveRuleCanvas.enabled = false;

    }
    public void manageDiscardClick()
    {
        saveRuleCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateMyNl()
    {
        List<RuleElement> myEvents = tempRuleScript.events;
        List<RuleElement> myConditions = tempRuleScript.conditions;
        List<RuleElement> myActions = tempRuleScript.actions;
        myNl = nl.generateFullNL(myEvents, myConditions, myActions); 
        myNlText.text = myNl;
    }
}
