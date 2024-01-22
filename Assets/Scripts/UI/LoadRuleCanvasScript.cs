using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadRuleCanvasScript : MonoBehaviour
{
    public AnchorCreator anchorCreator;
    public RuleElementScript ruleElementScript;
    private Canvas loadRuleCanvas;
    private Canvas mainMenuCanvas;
    public RuleSaveAndLoad ruleSaveAndLoad;
    public TMP_Dropdown myRuleListDropdown;
    public Text myNlText;
    public List<Rule> myRules;
    public NL nl;
    public TempRule tempRule;
    public Button backButton;
    public Button loadButton;
    public Rule selectedRule;

    // Start is called before the first frame update
    void Start()
    {
        anchorCreator = FindObjectOfType<AnchorCreator>();  //
        mainMenuCanvas = GameObject.Find("MainMenuCanvas").GetComponent<Canvas>();
        ruleSaveAndLoad = FindObjectOfType<RuleSaveAndLoad>();  // 
        ruleElementScript = FindObjectOfType<RuleElementScript>();
        nl = FindObjectOfType<NL>();  
        tempRule = FindObjectOfType<TempRule>();  
        loadRuleCanvas = GameObject.Find("LoadRuleCanvas").GetComponent<Canvas>();
        loadRuleCanvas.enabled = false;
        selectedRule = new Rule();
        selectedRule.name = "default";
        
        myRuleListDropdown.onValueChanged.AddListener(delegate
        {
            loadSelectedRuleNL(myRuleListDropdown);
        });
        
        backButton.onClick.AddListener(delegate
        {
            manageBackClick();
        });
        
        loadButton.onClick.AddListener(delegate
        {
            manageLoadClick();
        });
        
    }

    public void manageBackClick()
    {
        anchorCreator.editMode = false;
        tempRule.clearECALists();
        tempRule.resetLoadedRule();
        loadRuleCanvas.enabled = false;
        mainMenuCanvas.enabled = true; //

    }
    public void manageLoadClick()
    {
        ScreenLog.Log("MANAGE LOAD START");
        ruleElementScript.editSavedRuleMode = true;
        anchorCreator.editMode = true;
        loadRuleCanvas.enabled = false;
        tempRule.setLoadedRule(selectedRule);
        ScreenLog.Log("MANAGE LOAD 1");
        anchorCreator.placeAnchorsFromLoadedRule();
        ScreenLog.Log("MANAGE LOAD END");
    }


    public void loadSelectedRuleNL(TMP_Dropdown myRuleList)
    {
        ScreenLog.Log(myRuleList.options[myRuleList.value].text);
        foreach(Rule savedRule in myRules)
        {
            if(savedRule.name == myRuleList.options[myRuleList.value].text)
            {
                selectedRule = savedRule;
            }
        }
        if(selectedRule.name == "default")
        {
            ScreenLog.Log("ERROR IN RETREIVING RULE");
            return;
        }
        tempRule.generateECAListsFromRule(selectedRule);
        string myNl = nl.generateFullNL(tempRule.events, tempRule.conditions, tempRule.actions);
        myNlText.text = myNl;

        //ScreenLog.Log(myRuleList.options[myRuleList.value]);
        //int myId = myRuleList.options[myRuleList.value];
        //ScreenLog.Log(selectedRule.name);
        //cicla e prendi da rulelist
        ScreenLog.Log("OK");
        return;
    }

    public void show()
    {
        myRules = ruleSaveAndLoad.getRules();
        ScreenLog.Log("Loaded " + myRules.Count + " Rules");
        myRuleListDropdown.options.Clear();
        foreach (Rule rule in myRules)
        {
            ScreenLog.Log(rule.id.ToString());
            myRuleListDropdown.options.Add(new TMP_Dropdown.OptionData(rule.name));
        }
        myRuleListDropdown.value = -1; //Ook?


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
