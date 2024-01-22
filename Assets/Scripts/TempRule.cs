using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Manages a "temp rule" as a collection of 3 lists (events, condition, actions) of 
 * "rule element" structs
 */
public class TempRule : MonoBehaviour
{
    public List<RuleElement> events = new List<RuleElement>();
    public List<RuleElement> conditions =  new List<RuleElement>();
    public List<RuleElement> actions = new List<RuleElement>();
    private List<SingleEntry> recommendations;
    public Dictionary<int, RuleElement> allRuleElementsDict = new Dictionary<int, RuleElement>();
    public Rule loadedRule;
    private string tempCapability;
    private int tempObjectReferenceId;
    private int tempObjectId;
    private string tempEca;
    private string tempValue;
    private string tempSecondValue;
    private List<string> tempValues; //TODO: check if it is better to use a list or 1-2 value strings
    private string tempOperator;
    private string tempNextOperator;
    private string tempNl;
    private string ruleName;
    private int ruleId;
    public int progressive = 0;
    private Vector3 tempObjectReferencePosition;
    public Utils utils;
    public NL nl;
    public ContextData contextDataScript;
    public RuleElement selectedRecommendation;
    // Start is called before the first frame update
    void Start()
    {
        utils = FindObjectOfType<Utils>();  //
        nl = FindObjectOfType<NL>();  //
        contextDataScript = FindObjectOfType<ContextData>();  //
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public List<RuleElement> getAllEvents()
    {
        return events;
    }
    public List<RuleElement> getAllConditions()
    {
        return conditions;
    }
    public List<RuleElement> getAllActions()
    {
        return actions;
    }

    

    public void resetRule()
    {
        events = new List<RuleElement>();
        conditions = new List<RuleElement>();
        actions = new List<RuleElement>();
        allRuleElementsDict = new Dictionary<int, RuleElement>();
    }

    public List<SingleEntry> getRecommendations()
    {
        return recommendations;
    }

    /*
     * 
     */
    public void setRecommendations(string myRecs)
    {
        List<SingleEntry> receivedRecs = new List<SingleEntry>();
        Response response = JsonUtility.FromJson<Response>(myRecs);
        //ScreenLog.Log(response.data.Count.ToString());
        foreach(SingleEntry entry in response.data)
        {
            receivedRecs.Add(entry);
        }
        recommendations = receivedRecs;
    }

    public Dictionary<int, RuleElement> getAllRuleElementsDict() 
    {
        return allRuleElementsDict;
    }

    /**
     * 
     */
    public int getLastInsertedRuleElement()
    {
        foreach (var myElement in allRuleElementsDict)
        {
            //ScreenLog.Log("element in dict: " + myElement.Value.refId.ToString());
            if (myElement.Value.progressive == progressive - 1)
            {
                return myElement.Key;
            }
        }
        return 0; // It should never return 0
    }
    
    /**
     * Generate an ID
     * if yes and it is opened, what to do? 
     * if no, what to do?
     * TODO: secondValue???
     */
    public void generateRuleElementFromRecommendation(SingleEntry recommendation)
    {
        ScreenLog.Log("GENERATE RULE ELEMENT FROM RECOMMENDATION"); //
        ScreenLog.Log(recommendation.ECA); //
        //int id = utils.generatePseudoRandom();
        setTempCapability(recommendation.completeName);
        setTempECA(recommendation.ECA);
        setTempNextOperator(recommendation.nextOperator);
        setTempOperator(recommendation.myOperator);
        string objectOrService = contextDataScript.isObjectOrServiceFromCapabilityFullName(recommendation.completeName);
        ScreenLog.Log("RECOMMENDATION COMPLETENAME!! " + recommendation.completeName);
        int id = contextDataScript.getObjectOrServiceIdFromFullName(recommendation.completeName);
        ScreenLog.Log("RECOMMENDATION ID!! " + id);
        string descriptiveName = contextDataScript.getDescriptiveNameFromFullName(recommendation.completeName);
        setTempObjectReferenceId(id);
        setTempObjectReferencePosition(contextDataScript.getObjectPositionFromFullName(recommendation.completeName));
         
        if(recommendation.ECA == "a" || recommendation.ECA == "action")
        {
            //setTempNl(nl.generatePartialActionNLWithValue(recommendation.completeName, recommendation.value));
            setTempNl(nl.generatePartialActionNLWithValue(descriptiveName, recommendation.value));
        }
        else
        {
            //setTempNl(nl.generatePartialTriggerNLWithValue(recommendation.ECA, recommendation.completeName, recommendation.myOperator, recommendation.value));
            setTempNl(nl.generatePartialTriggerNLWithValue(recommendation.ECA, descriptiveName, recommendation.myOperator, recommendation.value));
        }
        ScreenLog.Log("ADD TO TEMP RECOMMENDATION!!!");
        if(tempEca == "")
        {
            ScreenLog.Log("ERROR: THE RULE ELEMENT IS NOT COMPLETE");
        }
        tempObjectId = utils.generatePseudoRandom();
        RuleElement ruleElement = new RuleElement(tempObjectId, tempObjectReferenceId, progressive, tempCapability, tempEca, tempOperator, tempValue, tempNextOperator, tempNl, tempSecondValue);
        progressive += 1;
        ScreenLog.Log("Temp Recommendation " + tempEca + " Added!"); //OK
        setSelectedRecommendation(ruleElement);
        //addToTempRule();
    }

    public RuleElement getSelectedRecommendation()
    {
        return selectedRecommendation;
    }

    public void setSelectedRecommendation(RuleElement ruleElement)
    {
        selectedRecommendation = ruleElement;
    }


    public bool thisObjectAlreadyUsedInRules()
    {
        //ScreenLog.Log("Object reference ID:"+ tempObjectReferenceId);
        //ScreenLog.Log("Object ID:"+ tempObjectId);
        foreach (var myElement in allRuleElementsDict)
        {
            //ScreenLog.Log("element in dict: " + myElement.Value.refId.ToString());
            if (myElement.Value.refId == tempObjectReferenceId)
            {
                return true;
            }
        }
        return false;
    }

    public void printTempRule()
    {
        ScreenLog.Log("--------------------- TEMP RULE ----------------------");
        ScreenLog.Log(tempNl);
        ScreenLog.Log(tempCapability);
        ScreenLog.Log(tempEca);
        ScreenLog.Log(tempValue);
        ScreenLog.Log(tempOperator);
        ScreenLog.Log(tempNextOperator);
        ScreenLog.Log(tempObjectReferenceId.ToString());
        ScreenLog.Log(tempObjectId.ToString());
        ScreenLog.Log(tempSecondValue);
    }


    public void resetTempRuleElement()
    {
        ScreenLog.Log("RESET TEMP RULE ELEMENT");
        resetTempNl();
        resetTempCapability();
        resetTempECA();
        resetTempValue();
        resetTempOperator();
        resetTempNextOperator();
        resetObjectReferenceId();
        resetTempSecondValue();
    }

    public void setLoadedRule(Rule rule)
    {
        loadedRule = rule;
    }
    public Rule getLoadedRule()
    {
        return loadedRule;
    }

    public void resetLoadedRule()
    {
        loadedRule = new Rule();
    }

    /**
     * TODO continue this method if we want to edit rules in the user test
     * Here we got the Rule loadedRule!
     */
    public RuleElement getRuleElementFromLoad()
    {
        return new RuleElement();
    }

    public bool editTempRule(int id, bool fromRec = false, bool fromLoad = false)
    {
        ScreenLog.Log("I WILL EDIT A TEMP RULE! " + id);
        RuleElement edited = new RuleElement();
        if (fromRec)
        {
            edited = getSelectedRecommendation(); //oppure ruleElementScript.myRuleElementInEdit: dovrebbero contenere lo  stesso obj
        }
        else if (fromLoad)
        {
            edited = getRuleElementFromLoad();
            return false; //TODO
        }
        else
        {
            edited = getRuleElementFromId(id);
        }

        ScreenLog.Log("MY ID: " + edited.id);
        //get the tempRule elements 
        edited.fullName = tempCapability;
        edited.eca = tempEca;
        edited.currentOperator = tempOperator;
        edited.nextOperator = tempNextOperator;
        edited.value = tempValue;
        edited.secondValue = tempSecondValue;
        /*
        ScreenLog.Log("MY FULLNAME: " + edited.fullName);
        ScreenLog.Log("MY VALUE: " + edited.value);
        ScreenLog.Log("MY OP: " + edited.currentOperator);
        ScreenLog.Log("MY next OP: " + edited.nextOperator);
        ScreenLog.Log("MY ECA: " + edited.eca);
        */
        // update the allRuleElementsDict and the ECA lists
        /*
        ScreenLog.Log("Events length: " + events.Count);
        ScreenLog.Log("Conditions length: " + conditions.Count);
        ScreenLog.Log("Actions length: " + actions.Count);
        */
        events.RemoveAll(r => r.id == id);
        conditions.RemoveAll(r => r.id == id);
        actions.RemoveAll(r => r.id == id);
        /*
        ScreenLog.Log("Events length: " + events.Count);
        ScreenLog.Log("Conditions length: " + conditions.Count);
        ScreenLog.Log("Actions length: " + actions.Count);
        */ 
        addToECALists(tempEca, edited);
        allRuleElementsDict[id] = edited;

        return true;
    }

    public void addToECALists(string tempEca, RuleElement ruleElement)
    {
        if (tempEca == "event")
        {
            events.Add(ruleElement);
        }
        else if(tempEca == "condition")
        {
            conditions.Add(ruleElement);
        }
        else if(tempEca == "action")
        {
            actions.Add(ruleElement);
        }
    }

    public bool addToTempRule()
    {
        ScreenLog.Log("ADD TO TEMP RULE!!!");
        if(tempEca == "")
        {
            ScreenLog.Log("ERROR: THE RULE ELEMENT IS NOT COMPLETE");
            return false;
        }
        tempObjectId = utils.generatePseudoRandom();
        RuleElement ruleElement = new RuleElement(tempObjectId, tempObjectReferenceId, progressive, tempCapability, tempEca, tempOperator, tempValue, tempNextOperator, tempNl, tempSecondValue);
        progressive += 1;
        addToECALists(tempEca, ruleElement);
        allRuleElementsDict.Add(tempObjectId, ruleElement);
        ScreenLog.Log("Temp " + tempEca + " Added!"); //OK
        return true;
    }

    public void removeRuleElementFromId(int id)
    {
        allRuleElementsDict.Remove(id);
        events.RemoveAll(r => r.id == id);
        conditions.RemoveAll(r => r.id == id);
        actions.RemoveAll(r => r.id == id);
    }

    public RuleElement getRuleElementFromId(int id)
    {
        return allRuleElementsDict[id];
    }
    public void setTempNextOperator(string value)
    {
        tempNextOperator = value;
    }

    public string getTempNextOperator()
    {
        return tempNextOperator;
    }

    public void resetTempNextOperator()
    {
        tempNextOperator = "";
    }
    public void setTempOperator(string value)
    {
        tempOperator = value;
    }

    public string getTempOperator()
    {
        return tempOperator;
    }

    public void resetTempNl()
    {
        tempNl = "";
    }

    public string getTempNl()
    {
        return tempNl;
    }

    public void setTempNl(string nl)
    {
        ScreenLog.Log("IM SETTING NL: " + nl);
        tempNl = nl;
    }
    public int getTempObjectId()
    {
        return tempObjectId;
    }
    public void setTempObjectId(int id)
    {
        tempObjectId = id;
    }

    public void resetTempOperator()
    {
        tempOperator = "";
    }

    public void setTempValue(string value)
    {
        tempValue = value;
    }
    public void setTempSecondValue(string value)
    {
        tempSecondValue = value;
    }

    public string getTempValue()
    {
        return tempValue;
    }
    public string getTempSecondValue()
    {
        return tempSecondValue;
    }

    public void resetTempValue()
    {
        tempValue = "";
    }
    public void resetTempSecondValue()
    {
        tempSecondValue = "";
    }

    public void setTempECA(string myEca)
    {
        tempEca = myEca;
    }
    public void resetTempECA()
    {
        tempEca = "";
    }

    public string getTempECA()
    {
        return tempEca;
    }

    public void clearECALists()
    {
        events.Clear();
        conditions.Clear();
        actions.Clear();
    }

    public void  addRuleElementToAllRulesElementDict(RuleElement myRuleElement)
    {
        ScreenLog.Log("ADDING " + myRuleElement.fullName);
        allRuleElementsDict.Add(myRuleElement.refId, myRuleElement);
    }
    public void generateECAListsFromRule(Rule myRule)
    {
        ScreenLog.Log("Generating the event condition action lists from the rule " + myRule.name);
        clearECALists();
        Dictionary<int, RuleElement> ruleElements = myRule.ruleElements;
        foreach(KeyValuePair<int, RuleElement> ruleElement in ruleElements)
        {
           if(ruleElement.Value.eca == "event")
            {
                events.Add(ruleElement.Value);
            }
           else if(ruleElement.Value.eca == "condition")
            {
                conditions.Add(ruleElement.Value);
            }
           else if(ruleElement.Value.eca == "action")
            {
                actions.Add(ruleElement.Value);
            }
        }
        ScreenLog.Log("ECA generated");
    }

    public void setTempCapability(string capability)
    {
       tempCapability = capability;
       ScreenLog.Log("temp obj capability xpath Set! " + capability);
    }

    public string getTempCapability()
    {
        return tempCapability;
    }
    public void resetTempCapability()
    {
       tempCapability = "";
    }
    public void setTempObjectReferencePosition(Vector3 position)
    {
       tempObjectReferencePosition = position;
    }
    public Vector3 getTempObjectReferencePosition()
    {
       return tempObjectReferencePosition;
    }
    public void setTempObjectReferenceId(int objectReferenceId)
    {
       tempObjectReferenceId = objectReferenceId; // QUA ARRIVA! AAAAAAAAAAAAAAAAAAAAAA
       ScreenLog.Log("temp obj reference Id Set! " + objectReferenceId.ToString());
    }
    public int getTempObjectReferenceId()
    {
       return tempObjectReferenceId;
    }


    public void resetObjectReferenceId()
    {
        tempObjectReferenceId = -1;
    }
}

[System.Serializable]
public struct RuleElement
{
    public int id;
    public int refId;
    public int progressive;
    public string fullName;
    public string eca;
    public string nextOperator;
    public string value;
    public string currentOperator;
    public string nl;
    public string secondValue;

    public RuleElement(int myid, int myRefId, int myProgressive, string myFullName, string myEca = "", string myOperator ="", string myValue="", string myNextOperator="", string myNl = "", string mySecondValue = "")
    {
        id = myid;
        refId = myRefId;
        progressive = myProgressive;
        fullName = myFullName;
        eca = myEca;
        nextOperator = myNextOperator;
        value = myValue;
        currentOperator = myOperator;
        nl = myNl;
        secondValue = mySecondValue;
    }
}

