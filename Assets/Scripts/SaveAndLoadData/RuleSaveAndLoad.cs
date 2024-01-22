using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class RuleSaveAndLoad : MonoBehaviour
{
    public ContextData contextDataScript;
    public AnchorCreator anchorCreator;
    public Utils utils;
    public TempRule tempRule;
    public NL nl;
    // Start is called before the first frame update
    void Start()
    {
        anchorCreator = FindObjectOfType<AnchorCreator>();  //
        nl = FindObjectOfType<NL>();  //
        utils = FindObjectOfType<Utils>();  //
        tempRule = FindObjectOfType<TempRule>();  //
        contextDataScript = FindObjectOfType<ContextData>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool checkSavedRulesListExistence()
    {
        if (File.Exists(Application.persistentDataPath + "/MyRules.dat"))
        {
            ScreenLog.Log("saved data exists!");
            return true;
        }
        ScreenLog.Log("NO saved data"); 
        return false;
    }

    public List<Rule> getRules()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/MyRules.dat", FileMode.Open);
        List<Rule> data = (List<Rule>)bf.Deserialize(file);
        file.Close();
        ScreenLog.Log("Game data loaded!");
        return data;
    }


    public void saveNewRule(string ruleName)
    {
        Dictionary<int, RuleElement> ruleElements = tempRule.allRuleElementsDict; //a cosa serviva l'ID nei ruleElements?
        List<RuleElement> events = tempRule.getAllEvents();
        List<RuleElement> conditions = tempRule.getAllConditions();
        List<RuleElement> actions = tempRule.getAllActions();
        int myId = utils.generatePseudoRandom();
        string myNl = nl.generateFullNL(events, conditions, actions);
        Rule myRuleToSave = new Rule(myId, ruleName, myNl, ruleElements);
        ScreenLog.Log("save game");
        List<Rule> myRules= new List<Rule>();
        if (checkSavedRulesListExistence())
        {
            myRules = getRules();
        }
        myRules.Add(myRuleToSave);
        BinaryFormatter bf = new BinaryFormatter();
        ScreenLog.Log(Application.persistentDataPath);
        FileStream file = File.Create(Application.persistentDataPath + "/MyRules.dat");
        bf.Serialize(file, myRules);
        file.Close();
    }

    // TODO do the actual saving 
    public void saveAlreadyPresentRule(string ruleName)
    {
        //ScreenLog.Log("SAVING AN ALREADY PRESENT RULE!!");
    }

    // a check is needed to see if the editor is in "edit rule"
    // In this case, we need to delete the old rule before saving the new one
    public bool saveRule(string ruleName) {
        if (anchorCreator.editMode)
        {
            saveAlreadyPresentRule(ruleName);
        }
        else
        {
            saveNewRule(ruleName);
        }
        return true;
    }
}

[System.Serializable]
public struct Rule
{
    public int id;
    public string name;
    public string nl;
    public Dictionary<int, RuleElement> ruleElements;

    public Rule(int myId, string myName, string myNl, Dictionary<int, RuleElement> myRuleElements)
    {
        id = myId;
        name = myName;
        nl = myNl;
        ruleElements = myRuleElements;
    }
}
