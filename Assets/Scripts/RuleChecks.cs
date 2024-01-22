using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleChecks : MonoBehaviour
{
    public TempRule tempRuleScript;
    public AnchorCreator anchorCreator;
    public ContextData contextDataScript;

    // Start is called before the first frame update
    void Start()
    {
        tempRuleScript = FindObjectOfType<TempRule>(); 
        contextDataScript = FindObjectOfType<ContextData>();  //
        anchorCreator = FindObjectOfType<AnchorCreator>();  //
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /**
     * Returns true if recommendations can be 
     * retreived, i.e., if at least 1 element is present 
     * in the temp rule
     */
    public bool checkRecommendRule()
    {
        RecommendationsIconScript recScript = (RecommendationsIconScript)GameObject.Find("RecommendationsIconCanvas").GetComponent("RecommendationsIconScript");
        if (tempRuleScript.events.Count > 0 || tempRuleScript.conditions.Count > 0 || tempRuleScript.actions.Count > 0)
        //if (tempRuleScript.allRuleElementsDict.Count > 0) // TODO: check if elements are removed from dictionart when "remove element" is pressed
        {
            recScript.enableRecButton();
            return true;
        }
        recScript.disableRecButton();
        return false;
    }

    /**
     * Returns true if a rule is ready to be saved:
     * there are at least 1 trigger and 1 action, 
     * or there are at least 1 action, more then 1 
     * trigger and there is no need to add an operator
     */
    public bool checkSaveRule()
    {
        //ScreenLog.Log("CHEK SAVE RULE");
        //ScreenLog.Log("Checking if save is possible"); //
        SaveRuleIconScript saveScript = (SaveRuleIconScript)GameObject.Find("SaveRuleIconCanvas").GetComponent("SaveRuleIconScript");
        if(tempRuleScript.events.Count > 0 || tempRuleScript.conditions.Count > 0)
        {
            if (checkOperatorNeeded() == -1)
            {
                if (tempRuleScript.actions.Count > 0)
                {
                    saveScript.enableSaveButton();
                    return true;
                }
                else
                {
                    ScreenLog.Log("ACTION NEEDED !!!");
                }
            }
            else
            {
                ScreenLog.Log("OPERATOR NEEDED!!!");
            }
        }
        saveScript.disableSaveButton();
        return false;
    }

    /**
     * Returns true if there are at least 2 conditions and no operator between them
     */
    public int checkOperatorNeeded()
    {
        //ScreenLog.Log("Checking if operator is needed");
        int events = tempRuleScript.events.Count;
        int conditions = tempRuleScript.conditions.Count;
        if(tempRuleScript.events.Count > 1)
        {
            //ScreenLog.Log("CHECKING EVENTS");
            for (int count = 0; count < events; count++)
            {
                if((count + 1) == events)
                {
                    break;
                }
                else
                {
                    if(tempRuleScript.events[count].nextOperator == "")
                    {
                        return tempRuleScript.events[count].id;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        if (tempRuleScript.conditions.Count > 1)
        {
            //ScreenLog.Log("CHECKING CONDITIONS");
            for (int count = 0; count < conditions; count++)
            {
                if((count + 1) == conditions)
                {
                    break;
                }
                else
                {
                    if(tempRuleScript.conditions[count].nextOperator == "")
                    {
                        return tempRuleScript.conditions[count].id;
                    }
                    else
                    {
                        break;
                    }
                }
            }

        }
        if (tempRuleScript.events.Count >0 && tempRuleScript.conditions.Count > 0)
        {
            //ScreenLog.Log("CHECKING EVENTS AND CONDITIONS");
            //Todo
        }
        return -1;
    }
}
