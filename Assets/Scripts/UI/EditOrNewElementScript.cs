using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EditOrNewElementScript : MonoBehaviour
{
    public Button buttonCancel;
    public Button buttonEditCapability;
    public Button buttonNewCapability;
    public TMP_Dropdown ruleElementsDropdown;
    public int relatedObjectId; 
    public string relatedObjectName; 
    private Canvas editNewCanvasObject; // the buttons calls methods on these game objs
    private Canvas editCanvasObject; // the buttons calls methods on these game objs
    private Canvas newCanvasObject;
    public TempRule tempRuleScript;
    public AnchorCreator anchorCreator;
    public string selectedRuleElementName;
    private List<RuleElement> ruleElementsOnThisObject = new List<RuleElement>();
    // Start is called before the first frame update
    void Start()
    {
        anchorCreator = FindObjectOfType<AnchorCreator>();  //
        tempRuleScript = FindObjectOfType<TempRule>(); 
        editNewCanvasObject = GameObject.Find("EditOrNewElementCanvas").GetComponent<Canvas>();
        editNewCanvasObject.enabled = false; 
        editCanvasObject = GameObject.Find("EditElementCanvas").GetComponent<Canvas>();
        editCanvasObject.enabled = false; 
        newCanvasObject = GameObject.Find("NewElementCanvas").GetComponent<Canvas>();
        newCanvasObject.enabled = false; 
        buttonCancel.onClick.AddListener(delegate () { onClickCancel(); });
        buttonEditCapability.onClick.AddListener(delegate () { onClickEditCapability(); });
        buttonNewCapability.onClick.AddListener(delegate () { onClickNewCapability(); });
        ruleElementsDropdown.onValueChanged.AddListener(delegate
        {
            storeSelectedElement(ruleElementsDropdown);
        });
    }

    public void storeSelectedElement(TMP_Dropdown myDropdown)
    {
        ScreenLog.Log("DROPDOWN VALUE CHANGED");
        ScreenLog.Log(myDropdown.options[myDropdown.value].text);
        selectedRuleElementName = myDropdown.options[myDropdown.value].text;
        ScreenLog.Log("DROPDOWN VALUE CHANGED OK");
    }


    public void enable(List<int> myRuleElementsIds, string myRelatedObjectName, int myRelatedObjectId)
    {
        relatedObjectId = myRelatedObjectId;
        relatedObjectName = myRelatedObjectName;
        ruleElementsOnThisObject = new List<RuleElement>();
        editNewCanvasObject = GameObject.Find("EditOrNewElementCanvas").GetComponent<Canvas>();
        editNewCanvasObject.enabled = true;
        ScreenLog.Log("STARTING THE FOREACH IN ENABLE");
        foreach (int id in myRuleElementsIds)
        {
            ScreenLog.Log("MY RULE ELEMENT ID: " + id); // When executing from the "icon" gameObject this id is empty
            RuleElement currentRuleElement = tempRuleScript.getRuleElementFromId(id);
            ruleElementsOnThisObject.Add(currentRuleElement);
        }
        // Draw a dropdown with these rule elements
        ruleElementsDropdown.options.Clear();
        foreach (RuleElement ruleElement in ruleElementsOnThisObject)
        {
            ruleElementsDropdown.options.Add(new TMP_Dropdown.OptionData(ruleElement.fullName));
        }
        //selectedRuleElementName = ruleElementsOnThisObject[0].fullName; //
        ScreenLog.Log("SELECTED RULE ELEMENT: " + selectedRuleElementName);
        ruleElementsDropdown.value = -1; //Ook?
        anchorCreator.UIOpen = true;
    }

    public void onClickCancel()
    {
        selectedRuleElementName = "";
        ruleElementsOnThisObject = new List<RuleElement>();
        editNewCanvasObject.enabled = false;
        anchorCreator.UIOpen = false;
    }
    public void onClickEditCapability()
    {
        // get the selected capability
        RuleElement selectedRuleElement = new RuleElement();
        foreach (RuleElement ruleElement in ruleElementsOnThisObject)
        {
            if (ruleElement.fullName == selectedRuleElementName)
            {
                ScreenLog.Log("FOUND! ");
                ScreenLog.Log(ruleElement.fullName);
                selectedRuleElement = ruleElement;
                // pass it to EditElementScript
                RuleElementScript ruleElementScript = GameObject.Find("RuleElementCanvas").GetComponent<RuleElementScript>();
                ruleElementScript.editFromAlreadyOpened = true; //This is the only place where this flag is setted true.
                ruleElementScript.showEdit(selectedRuleElement); //

                ruleElementsOnThisObject = new List<RuleElement>();
                editNewCanvasObject.enabled = false;
                selectedRuleElementName = "";
                return;
            }
        }
    }
    public void onClickNewCapability()
    {
        selectedRuleElementName = "";
        ruleElementsOnThisObject = new List<RuleElement>();
        editNewCanvasObject.enabled = false;
        anchorCreator.loadRuleElementPanel(relatedObjectId, relatedObjectName); //
    }
    public static bool getIsOpen()
    {
        return isOpen;
    }

    public static void setIsOpen(bool value)
    {
        isOpen = value;
    }

    public static bool isOpen;

    // Update is called once per frame
    void Update()
    {
        
    }
}
