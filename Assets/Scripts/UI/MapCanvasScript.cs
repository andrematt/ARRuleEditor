using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapCanvasScript : MonoBehaviour
{
    public Canvas ruleMapCanvas;
    public Text myEvents;
    public Text myConditions;
    public Text myActions;
    public TempRule tempRuleScript;
    public ContextData contextDataScript;
    public NL nl;
    // Start is called before the first frame update
    void Start()
    {
        tempRuleScript = FindObjectOfType<TempRule>(); 
        contextDataScript = FindObjectOfType<ContextData>(); 
        nl = FindObjectOfType<NL>(); 
        ruleMapCanvas.enabled = false; //
        
    }

    /**
     * Generate description of each added to ECALists elements
     */
    public void addECA()
    {
        //ScreenLog.Log("INSIDE ADDECA");
        string myEventsText = "";
        List<RuleElement> allEvents = tempRuleScript.getAllEvents();
        if (allEvents.Count > 0)
        {
            myEventsText = "";
            foreach (RuleElement element in allEvents)
            {
                string descriptiveName = contextDataScript.getDescriptiveNameFromFullName(element.fullName);
                //public string generatePartialTriggerNLWithValue(string tempEca, string tempCapability, string myOperator, int value)
                //string myText = nl.generatePartialTriggerNLWithValue(element.eca, element.fullName, element.currentOperator, element.value);
                string myText = nl.generatePartialTriggerNLWithValue(element.eca, descriptiveName, element.currentOperator, element.value);
                myEventsText += myText + "\n";
            }
        }
        myEvents.text = myEventsText;
        
        string myConditionsText = "";
        List<RuleElement> allConditions = tempRuleScript.getAllConditions();
        if (allConditions.Count > 0)
        {
            myConditionsText = "";
            foreach (RuleElement element in allConditions)
            {
                string descriptiveName = contextDataScript.getDescriptiveNameFromFullName(element.fullName);
                //string myText = nl.generatePartialTriggerNLWithValue(element.eca, element.fullName, element.currentOperator, element.value);
                string myText = nl.generatePartialTriggerNLWithValue(element.eca, descriptiveName, element.currentOperator, element.value);
                myConditionsText += myText + "\n";
            }
        }
        myConditions.text = myConditionsText;

        string myActionsText = "";
        List<RuleElement> allActions = tempRuleScript.getAllActions();
        if (allActions.Count > 0)
        {
            myActionsText = "";
            foreach (RuleElement element in allActions)
            {
                //ScreenLog.Log("VALUE: " + element.value);
                string descriptiveName = contextDataScript.getDescriptiveNameFromFullName(element.fullName);
                //string myText = nl.generatePartialActionNLWithValueMap(element.fullName, element.value);
                string myText = nl.generatePartialActionNLWithValueMap(descriptiveName, element.value);
                myActionsText += myText + "\n";
            }
        }
        myActions.text = myActionsText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
