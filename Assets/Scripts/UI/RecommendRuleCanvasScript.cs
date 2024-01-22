using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecommendRuleCanvasScript : MonoBehaviour
{
    private Canvas recommendRuleCanvas;
    public Button backButton;
    public Button loadButton;
    public TMP_Dropdown myRuleElementListDropdown;
    public SingleEntry selectedRuleElement;
    public TempRule tempRule;
    public RuleElementScript ruleElementScript;
    public AnchorCreator anchorCreator;
    public NL nl;
    public ContextData contextDataScript;

    // Start is called before the first frame update
    void Start()
    {
        ruleElementScript = FindObjectOfType<RuleElementScript>();  //
        recommendRuleCanvas = GameObject.Find("RecommendRuleCanvas").GetComponent<Canvas>();
        recommendRuleCanvas.enabled = false; //
        contextDataScript = FindObjectOfType<ContextData>();  //
        anchorCreator = FindObjectOfType<AnchorCreator>();  //
        nl = FindObjectOfType<NL>();  //
        tempRule = FindObjectOfType<TempRule>();

        myRuleElementListDropdown.onValueChanged.AddListener(delegate
        {
            selectRuleElement(myRuleElementListDropdown);
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selectRuleElement(TMP_Dropdown myRuleList)
    {
        //ScreenLog.Log(myRuleList.options[myRuleList.value].text);
        //ScreenLog.Log("I WILL ENTER THE FOR LOOP");
        List<SingleEntry> myRuleElements = tempRule.getRecommendations();
        foreach(SingleEntry entry in myRuleElements)
        {
            string entryDesc = nl.generateEntryDescription(entry);
            if (entryDesc == myRuleList.options[myRuleList.value].text)
            {
                //ScreenLog.Log("FOUND!!! " + entry.realName);
                //ScreenLog.Log("Selected " + entryDesc);
                selectedRuleElement = entry;
            }
        }
        if(selectedRuleElement.realName == "default") //
        {
            //ScreenLog.Log("ERROR IN RETREIVING RULE");
            //ScreenLog.Log("-------------------------------------------------------------");
            throw new Exception("RULE NOT RETREIVED");
            return;
        }

        return;
    }

    //Microwavedoor-close
    //calling generaterulefrom
    // 0

    public void getReceivedRecommendations(List<SingleEntry> recs)
    {
        anchorCreator.UIOpen = true;
        //ScreenLog.Log("N OF RECS: " + recs.Count);
        myRuleElementListDropdown.options.Clear();
        foreach(SingleEntry entry in recs)
        {
            //ScreenLog.Log(entry.xPath);
            string entryDesc = nl.generateEntryDescription(entry);  //
            myRuleElementListDropdown.options.Add(new TMP_Dropdown.OptionData(entryDesc)); //
        }
        myRuleElementListDropdown.value = -1; //Ook?
    }

    
    public void manageBackClick()
    {
        anchorCreator.UIOpen = false;
        recommendRuleCanvas.enabled = false;
    }
    public void manageLoadClick()
    {
        anchorCreator.UIOpen = false;
        recommendRuleCanvas.enabled = false;
        /*
        ScreenLog.Log("CALLING GENERATERULEFROMREC");
        ScreenLog.Log(selectedRuleElement.id.ToString());
        ScreenLog.Log(selectedRuleElement.originalRuleId.ToString());
        ScreenLog.Log(selectedRuleElement.xPath);
        ScreenLog.Log(selectedRuleElement.parent);
        ScreenLog.Log(selectedRuleElement.realName);
        ScreenLog.Log(selectedRuleElement.ECA);
        ScreenLog.Log(selectedRuleElement.myOperator);
        ScreenLog.Log(selectedRuleElement.nextOperator);
        ScreenLog.Log(selectedRuleElement.value);
        ScreenLog.Log(selectedRuleElement.completeName);
        ScreenLog.Log("OK??");
        */
        tempRule.generateRuleElementFromRecommendation(selectedRuleElement);
        RuleElement insertedRecommendation = tempRule.getSelectedRecommendation(); //
        ruleElementScript.setRuleElementInEdit(insertedRecommendation); // maybe in tempRule is useless
        ruleElementScript.recommendMode = true;
        //int lastRuleElementId = insertedRecommendation.id;
        //getLastInsertedRuleElement(); //Not too sure if it is to add now or after
        //kRuleElement ruleElementFromRecommendation = tempRule.allRuleElementsDict[lastRuleElementId];
        ruleElementScript.showFromRecommendation(insertedRecommendation); //
    }
}
    

[System.Serializable]
public class SingleEntry
{
    public string ECA;
    public string completeName;
    public int id;
    public string myOperator;
    public string nextOperator;
    public string originalRuleId;
    public string parent;
    public string realName;
    public string name;
    public string value;
    public string xPath;
}

[System.Serializable]
public class Response
{
    public List<SingleEntry> data;
}
