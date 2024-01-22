using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NL : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public string generateEntryDescription(SingleEntry entry)
    {
        ScreenLog.Log("RECOMMENDATION ENTRY!!");
        ScreenLog.Log("completeName: " + entry.completeName);
        ScreenLog.Log("name: " +entry.name);
        ScreenLog.Log("realName: " + entry.realName);
        ScreenLog.Log("value: " +entry.value);
        ScreenLog.Log("operator: " +entry.myOperator);
        string description;
        string value = entry.value;
        string initial = "";
        if (entry.ECA == "a" || entry.ECA == "action")
        {
            if(entry.parent == "alarms" || entry.parent == "reminders")
            {
                initial = "send";
            }
            else 
            {
                initial = "set";
            }
        }
        else if (entry.ECA =="e" || entry.ECA == "event")
        {
            initial = "when";
        }
        else if (entry.ECA =="c" || entry.ECA == "condition")
        {
            initial = "if";
        }
        string parent = entry.parent;
        if (parent == "alarms" || parent == "reminders")
        {
            parent = "";
        }

        if(value == "VALUE")
        {
            value = "TRUE"; //ci sarebbe da considerare il not ma per ora lasciamo stare
        }
        else if (value == "invokeFunctions:changeApplianceState")
        {
            value = "turn ON / OFF the light";
        }
        else if (value == "update:lightColor")
        {
            value = "change the color of the lamp";
        }
        else if (value == "invokeFunctions:changeDoorState")
        {
            value = "Open / Close the door or the window";
        }
        if(entry.myOperator == "OPERATOR")
        {
            description = initial +" " + parent + " "+ entry.realName + " " + value;
        }
        else
        {
            description = initial + " " + parent + " "+ entry.realName + " " + entry.myOperator + " " + value;

        }
        return description; //
    }
    public string generatePartialTriggerNL(string tempEca, string tempCapability)
    {
        string eventCondition = "";
        if(tempEca == "event")
        {
            eventCondition = "when ";
        }
        else
        {

            eventCondition = "if ";
        }
        eventCondition += tempCapability; //needed also the NL one //
        if(tempEca == "event")
        {
            eventCondition += " becomes ";
        }
        else
        {

            eventCondition += " is ";
        }
        return eventCondition;
    }
    public string generatePartialTriggerNLWithValue(string tempEca, string descriptiveName, string myOperator, string value)
    {
        string eventCondition = generatePartialTriggerNL(tempEca, descriptiveName);
        if (myOperator == "OPERATOR")
        {
            myOperator = "";
        }
        return eventCondition + " " + myOperator + " " + value;
    }
    public string generatePartialTriggerNLWithValue(string tempEca, string descriptiveName, string myOperator, int value)
    {
        string eventCondition = generatePartialTriggerNL(tempEca, descriptiveName);
        if (myOperator == "OPERATOR")
        {
            myOperator = "";
        }
        return eventCondition + " " + myOperator + " " + value;
    }

    public string generatePartialActionNL(string descriptiveName)
    {
        string actionStr = "then "  + descriptiveName;
        return actionStr;
    }

    public string generatePartialActionNLWithValue(string tempCapability, string value)
    {
        string actionStr = "then "  + tempCapability + " " + value;
        return actionStr;
    }

    public string generatePartialActionNLWithValue(string tempCapability, int value)
    {
        string actionStr = "then "  + tempCapability + " " + value;
        return actionStr;
    }
    public string generatePartialActionNLWithValueMap(string tempCapability, string value)
    {
        string actionStr = "- "  + tempCapability + " " + value;
        return actionStr;
    }

    public string generatePartialActionNLWithValueMap(string tempCapability, int value)
    {
        string actionStr = "- "  + tempCapability + " " + value;
        return actionStr;
    }


    /**
     * Only used to generate the save/load summary
     * TODO use descriptive name
     */
    public string generateFullNL(List<RuleElement> events, List<RuleElement> conditions, List<RuleElement> actions)
    {
        // when ... .... , and 
        string myNl = "";
        int eventsLen = events.Count;
        int conditionsLen = conditions.Count;
        int actionsLen = actions.Count;
        for (int count = 0; count < eventsLen; count++)
        {
            myNl += "when ";
            myNl += events[count].fullName + " becomes " + events[count].currentOperator + " " + events[count].value;
            myNl += ", ";
            if(events[count].nextOperator != "none")
            {
                myNl += events[count].nextOperator;
            }
        }
        for (int count = 0; count < conditionsLen; count++)
        {
            myNl += " if ";
            myNl += conditions[count].fullName + " is " + conditions[count].currentOperator + " " + conditions[count].value;
            myNl += ", ";
            if(conditions[count].nextOperator != "none")
            {
                myNl += conditions[count].nextOperator;
            }
        }
        for (int count = 0; count < actionsLen; count++)
        {
            myNl += " then ";
            myNl += actions[count].fullName + " " + actions[count].currentOperator + " " + actions[count].value;
            if (count +1 != actionsLen)
            {
                myNl += ", ";
                if (actions[count].nextOperator != "none")
                {
                    myNl += actions[count].nextOperator;
                }
            }
        }
        return myNl;
    }

    /*
    public string generateTriggerNL()
    {
        string eventCondition;
        eventCondition = generatePartialTriggerNL();
        eventCondition += " " + tempValue;
        return eventCondition;
    }
    */
}
