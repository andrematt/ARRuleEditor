using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Fai il listener sulla dropdown per il next operator e metti default none
// Fai i controlli quando viene fatto edit di un rule element
public class RuleElementScript : MonoBehaviour
{
    private RuleElement myRuleElementInEdit;
    public ServicesIconScript servicesIconScript;
    public RecommendationsIconScript recommendationsIconScript;
    public Button buttonEvent;
    public Button buttonCondition;
    public Button saveRuleElement;
    public Button discardRuleElement;
    public Button removeRuleElement;
    public Text myNlText;
    public InputField myFirstDateHour;
    public InputField myFirstDateMin; //
    
    public InputField mySecondDateHour; //These 3 objects are dynamic: appears only when the "between" operator is selected in the "Date" canvas
    public InputField mySecondDateMin;  //
    public Text spacerSecondLine;       //
    public TMP_Dropdown dropdownDate;   //This is the dropdown where it is possible to select "between"
    public GameObject secondLineContainer; //Used to show/hide the 3 dynamic objects related to the second time that should appear when the "between" op is selected
    public TMP_Dropdown boolDropdown; 
    public TMP_Dropdown myCapabilityDropdown;
    public TMP_Dropdown myEnumDropdown;
    public TMP_Dropdown myNextOperatorDropdown;
    public TMP_Dropdown myOperatorDropdown;
    public InputField myIntInputField;
    private Canvas ruleElementCanvas;
    private Canvas enumCanvas;
    private Canvas stringCanvas;
    private Canvas intCanvas;
    private Canvas doubleCanvas;
    private Canvas boolCanvas;
    private Canvas timeCanvas;
    private Canvas dateCanvas;
    private Canvas eventConditionCanvas;
    private Canvas nextOperatorCanvas;
    public TempRule tempRuleScript;
    public AnchorCreator anchorCreator;
    public GetRecommendations getRecommendationsScript;
    public ContextData contextDataScript;
    public RuleChecks ruleChecks;
    public NL nl;
    public UnityEngine.UI.Text ECAText;
    public UnityEngine.UI.Text capabilityText;

    private List<ObjectOrServiceCapability> myCapabilities;
    ObjectOrServiceCapability myCapability;

    public bool recommendMode = false; 
    public bool editSavedRuleMode = false; //from an OOP perspective, it does not make sense to keep this here. But we got only a single RuleElement panel that is modified (show/hide, ...) each time, so this can work
    private bool editMode = false;
    public bool editFromAlreadyOpened = false; 
    private bool editModeProperty
    {
        get { return editMode; }
        set
        {
            if (value == editMode)
            {
                return;
            }
            editMode = value;
            if (editMode)
            {
                removeRuleElement.gameObject.SetActive(true);
            }
            else
            {
                removeRuleElement.gameObject.SetActive(false);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //editModeProperty= false;
        recommendationsIconScript = FindObjectOfType<RecommendationsIconScript>(); //
        servicesIconScript = FindObjectOfType<ServicesIconScript>(); //
        nl = FindObjectOfType<NL>();
        tempRuleScript = FindObjectOfType<TempRule>(); 
        contextDataScript = FindObjectOfType<ContextData>(); // 
        getRecommendationsScript = FindObjectOfType<GetRecommendations>();
        anchorCreator = FindObjectOfType<AnchorCreator>();  //
        ruleChecks = FindObjectOfType<RuleChecks>();  //
        boolDropdown.onValueChanged.AddListener(delegate
        {
            updateRuleOperatorAndValue();
            quickUpdateNl();
        });
        dropdownDate.onValueChanged.AddListener(delegate
        {
            checkBetweenDateListener(dropdownDate);
            updateRuleOperatorAndValue();
            quickUpdateNl();
        });
        myCapabilityDropdown.onValueChanged.AddListener(delegate
        {
            checkCapabilityDropdownChange(myCapabilityDropdown);
            updateRuleOperatorAndValue();
            quickUpdateNl();
        });
        myEnumDropdown.onValueChanged.AddListener(delegate
        {
            checkEnumDropdownChange(myEnumDropdown);
            updateRuleOperatorAndValue();
            quickUpdateNl();
        });
        myNextOperatorDropdown.onValueChanged.AddListener(delegate
        {
            checkNextOperatorDropdownChange(myNextOperatorDropdown);
            updateRuleOperatorAndValue();
            quickUpdateNl();
        });
        myOperatorDropdown.onValueChanged.AddListener(delegate
        {
            updateRuleOperatorAndValue();
            quickUpdateNl();
        });
        myIntInputField.onValueChanged.AddListener(delegate
        {
            updateRuleOperatorAndValue();
            quickUpdateNl();
        });
        buttonEvent.onClick.AddListener(delegate
        {
            manageEventClick();
            updateRuleOperatorAndValue();
            quickUpdateNl();
        });
        buttonCondition.onClick.AddListener(delegate
        {
            manageConditionClick();
            updateRuleOperatorAndValue();
            quickUpdateNl();
        });
        saveRuleElement.onClick.AddListener(delegate
        {
            manageSaveClick();
        });
        discardRuleElement.onClick.AddListener(delegate
        {
            manageDiscardClick();
        });
        removeRuleElement.onClick.AddListener(delegate
        {
            manageRemoveClick();
        });
        removeRuleElement.gameObject.SetActive(false);
        ruleElementCanvas = GameObject.Find("RuleElementCanvas").GetComponent<Canvas>();
        ruleElementCanvas.enabled = false;
        eventConditionCanvas = GameObject.Find("EventConditionCanvas").GetComponent<Canvas>();
        enumCanvas = GameObject.Find("EnumCanvas").GetComponent<Canvas>();
        stringCanvas = GameObject.Find("StringCanvas").GetComponent<Canvas>();
        intCanvas = GameObject.Find("IntCanvas").GetComponent<Canvas>();
        doubleCanvas = GameObject.Find("DoubleCanvas").GetComponent<Canvas>();
        boolCanvas = GameObject.Find("BoolCanvas").GetComponent<Canvas>();
        timeCanvas = GameObject.Find("TimeCanvas").GetComponent<Canvas>();
        dateCanvas = GameObject.Find("DateCanvas").GetComponent<Canvas>();
        nextOperatorCanvas = GameObject.Find("SelectNextOperatorCanvas").GetComponent<Canvas>();
        myFirstDateHour.text = "00";
        myFirstDateHour.onValidateInput = OnInputChangedHour;
        myFirstDateMin.text = "00";
        myFirstDateMin.onValidateInput = OnInputChangedMin;
        mySecondDateHour.text = "00";
        mySecondDateHour.onValidateInput = OnInputChangedHour;
        mySecondDateMin.text = "00";
        mySecondDateMin.onValidateInput = OnInputChangedMin;
        secondLineContainer.SetActive(false);

        disableAllSecondaryCanvas();

    }
    private char OnInputChangedHour(string text, int index, char addedChar)
    {
        int x = 0;
        int y = 0;
        // you know that the parsing attempt
        // was successful
        if (Int32.TryParse(addedChar+"", out y))
        {
            y = Int32.Parse(addedChar+"");
            //ScreenLog.Log("Y!!" + y);
        }
        if (Int32.TryParse(text, out x))
        {
            x = Int32.Parse(text);
            //ScreenLog.Log("X!!" + x);
            if (x*10 + y > 23)
            {
                return '\0';
            }
        }
        return addedChar;
    }
    private char OnInputChangedMin(string text, int index, char addedChar)
    {
        int x = 0;
        int y = 0;
        // you know that the parsing attempt
        // was successful
        if (Int32.TryParse(addedChar+"", out y))
        {
            y = Int32.Parse(addedChar+"");
            //ScreenLog.Log("Y!!" + y);
        }
        if (Int32.TryParse(text, out x))
        {
            x = Int32.Parse(text);
            //ScreenLog.Log("X!!" + x);
            if (x*10 + y > 59)
            {
                return '\0';
            }
        }
        return addedChar;
    }
    public void setRuleElementInEdit(RuleElement ruleElement)
    {
        myRuleElementInEdit = ruleElement;
    }

    public RuleElement getRuleElementInEdit()
    {
        return myRuleElementInEdit;
    }

    public void manageEventClick()
    {
        tempRuleScript.setTempECA("event");
        //updateNlPartialTrigger();
    }
    public void manageConditionClick()
    {
        tempRuleScript.setTempECA("condition");
        //updateNlPartialTriggerWithValue();
    }

    public void printDebugInfo()
    {
        int myId = tempRuleScript.getTempObjectId(); 
        string myCapabilityXpath = tempRuleScript.getTempCapability();
        string myFullValue = tempRuleScript.getTempOperator() + " " + tempRuleScript.getTempValue();
        string myCapability = contextDataScript.capabilityFromFullName(myCapabilityXpath);
        string myNextOperator = tempRuleScript.getTempNextOperator(); //
        string myNL = tempRuleScript.getTempNl(); //
        ScreenLog.Log("My ID: " + myId.ToString());
        ScreenLog.Log("My next operator: " + myNextOperator);  
        ScreenLog.Log("My temp ECA: " + tempRuleScript.getTempECA());
        ScreenLog.Log("My Capability: " + myCapability);
        ScreenLog.Log("My FullValue: " + myFullValue);
        ScreenLog.Log("My NL: " + myNL);
        ScreenLog.Log("My NextOperator: " + myNextOperator);
        ScreenLog.Log("My reference object ID: " +  tempRuleScript.getTempObjectReferenceId()); 
        ScreenLog.Log("printDebugInfo OK!");

    }

    public void deactivateAssociatedExclamationMark()
    {
        ScreenLog.Log("deactivateAssociatedExclamationMark Start");
        int myReferenceObjId = tempRuleScript.getTempObjectReferenceId();
        anchorCreator.deactivateExclamationMark(myReferenceObjId);
        ScreenLog.Log("deactivateAssociatedExclamationMark OK!");
    }
    
    // TODO: sostutuire update...with temp con questa
    public void updatePanelWithLastRuleElement()
    {
        ScreenLog.Log("updatePanelWithLastRuleElement Start");
        int myId = tempRuleScript.getTempObjectId(); // 
        //ScreenLog.Log("OBTAINED ID");
        //ScreenLog.Log(myId.ToString());
        RuleElement lastInsertedRuleElement = tempRuleScript.getRuleElementFromId(myId);
        //ScreenLog.Log("OBTAINED RULE ELEMENT FROM ID");
        //ScreenLog.Log(lastInsertedRuleElement.fullName);
        string myCapabilityFullName = lastInsertedRuleElement.fullName; 
        string myFullValue = lastInsertedRuleElement.currentOperator + " " + lastInsertedRuleElement.value;
        string myCapability = contextDataScript.capabilityFromFullName(myCapabilityFullName);
        //ScreenLog.Log("OBTAINED CAPABILITY FROM FULL NAME");
        //ScreenLog.Log(myCapability);
        string myNextOperator = lastInsertedRuleElement.nextOperator;
        //ScreenLog.Log("OBTAINED NEXT OPERATOR");
        //printDebugInfo();
        if (contextDataScript.isObjectOrServiceFromCapabilityFullName(myCapabilityFullName) == "o")
        {
            /*
            ScreenLog.Log("7"); //
            ScreenLog.Log("tempECA"); //
            ScreenLog.Log(tempRuleScript.getTempECA()); //
            ScreenLog.Log("CAPABILITY"); //
            ScreenLog.Log(myCapability);
            ScreenLog.Log("FULL VALUE"); //
            ScreenLog.Log(myFullValue);
            ScreenLog.Log("NEXTOPERATOR"); //
            ScreenLog.Log(myNextOperator);
            ScreenLog.Log("ID"); //
            ScreenLog.Log(myId.ToString());
            ScreenLog.Log("TEMPOPERATORREFID"); //
            ScreenLog.Log(tempRuleScript.getTempObjectReferenceId().ToString());
            //ScreenLog.Log(contextDataScript.getTempObjectComposedName())
            */
            //anchorCreator.activateInfoPanel(tempRuleScript.getTempECA(), contextDataScript.getTempObjectComposedName(tempRuleScript.getTempObjectReferenceId()), myCapability, myFullValue, myNextOperator, myId);
            ScreenLog.Log("ACTIVATING EXCL MARK"); //
            anchorCreator.activateActiveExclamationMark(tempRuleScript.getTempECA(), contextDataScript.getTempObjectComposedName(tempRuleScript.getTempObjectReferenceId()), myCapability, myFullValue, myNextOperator, myId);
            ScreenLog.Log("8");
        }
        ScreenLog.Log("updatePanelWithLastRuleElement OK!");
    }

    /*
     * TODO:
     * all the listeners could be moved here, to make a less sparse class
     * Now works, but it is horrible
     */
    public void updateRuleOperatorAndValue() 
    {
        if(intCanvas.isActiveAndEnabled == true)
        { //perché questo è fatto qua e non in un listener? perché altrimenti devi mettere 1000 listeners nello start
            Text intInputField = GameObject.Find("IntText").GetComponent<Text>();
            TMP_Dropdown intDropdown = GameObject.Find("DropdownInt").GetComponent<TMP_Dropdown>();
            // Get the operator!!
            string myOperator = intDropdown.options[intDropdown.value].text;
            string intValue = intInputField.text;
            tempRuleScript.setTempValue(intValue);
            tempRuleScript.setTempOperator(myOperator);  
        }
        if(boolCanvas.isActiveAndEnabled == true)
        { //Perché questo è fatto qua e non in un listener? 
            TMP_Dropdown boolDropdown = GameObject.Find("DropdownBool").GetComponent<TMP_Dropdown>();
            // Get the operator!!
            string myOperator = boolDropdown.options[boolDropdown.value].text;
            tempRuleScript.setTempValue("true");
            tempRuleScript.setTempOperator(myOperator);  
        }
        if(stringCanvas.isActiveAndEnabled == true)
        { 
            TMP_Text strInputField = GameObject.Find("StringText").GetComponent<TMP_Text>(); //
            string strValue = strInputField.text;
            tempRuleScript.resetTempOperator(); // It should not be required, just for good measure
            ScreenLog.Log("SETTING STRING VALUE: " + strValue);
            tempRuleScript.setTempValue(strValue);
        }
        if(enumCanvas.isActiveAndEnabled == true)
        { 
            tempRuleScript.resetTempOperator(); // It should not be required, just for good measure
            // The value is already managed in checkEnumDropdownChange
        }

    }
    

    // directly modify the already saved rule element
    // TODO: all this managements have to be separated in a different script foreach type canvas
    public void editRuleElement()
    {
        if(intCanvas.isActiveAndEnabled == true)
        { //perché questo è fatto qua e non in un listener? perché altrimenti devi mettere 1000 listeners nello start
            Text intInputField = GameObject.Find("IntText").GetComponent<Text>();
            TMP_Dropdown intDropdown = GameObject.Find("DropdownInt").GetComponent<TMP_Dropdown>();
            // Get the operator!!
            string myOperator = intDropdown.options[intDropdown.value].text;
            string intValue = intInputField.text;
            tempRuleScript.setTempValue(intValue);
            tempRuleScript.setTempOperator(myOperator);  
        }
        if(boolCanvas.isActiveAndEnabled == true)
        { //Perché questo è fatto qua e non in un listener? 
            TMP_Dropdown boolDropdown = GameObject.Find("DropdownBool").GetComponent<TMP_Dropdown>();
            // Get the operator!!
            string myOperator = boolDropdown.options[boolDropdown.value].text;
            tempRuleScript.setTempValue("true");
            tempRuleScript.setTempOperator(myOperator);  
        }
        if(stringCanvas.isActiveAndEnabled == true)
        { 
            TMP_Text strInputField = GameObject.Find("StringText").GetComponent<TMP_Text>(); //
            string strValue = strInputField.text;
            tempRuleScript.resetTempOperator(); // It should not be required, just for good measure
            ScreenLog.Log("SETTING STRING VALUE: " + strValue);
            tempRuleScript.setTempValue(strValue);
        }
        
        bool check = false;
        if (recommendMode)
        {
            check = tempRuleScript.editTempRule(myRuleElementInEdit.id, true); // Flag true for recommendMode
            recommendMode = false;
        }
        else if (editSavedRuleMode)
        {
            check = tempRuleScript.editTempRule(myRuleElementInEdit.id, false, true); // Flag true for editSavedRuleMode
        }
        else
        {
            check = tempRuleScript.editTempRule(myRuleElementInEdit.id);
        }
        
        if (check)
        {
            //updatePanelWithTempRuleElement();
            updatePanelWithLastRuleElement();
            ScreenLog.Log("updatePanelWithLastRuleElement OK!");
            ruleChecks.checkSaveRule();
            ScreenLog.Log("chekSaveRule OK!");
            ruleChecks.checkRecommendRule();
            ScreenLog.Log("checkRecommendRule OK!");
            ruleChecks.checkOperatorNeeded();
            ScreenLog.Log("checkOperatorNeeded OK!");

        }
        //ScreenLog.Log("editRuleElement correctly ended");
    }

    public void quickUpdateNl()
    {
        if (tempRuleScript.getTempECA() == "action" || tempRuleScript.getTempECA() == "a")
        {
            updateNlPartialActionWithValue();
        }
        else
        {
            updateNlPartialTriggerWithValue();
        }
    }

    public void addNlToTempRule()
    {
        //string myNl = nl.generatePartialTriggerNL(tempRuleScript.getTempECA(), tempRuleScript.getTempCapability()); //
        string myEca = tempRuleScript.getTempECA();
        ScreenLog.Log("MY ECA; " + myEca);
        string myNl = tempRuleScript.getTempNl();
        ScreenLog.Log("MY NL; " + myNl);
    }
    public void manageSaveClick()
    {
        ScreenLog.Log("ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ"); //
        ScreenLog.Log("manageSaveClick");
        anchorCreator.UIOpen = false;
        int ruleElementId = myRuleElementInEdit.id; // # This Id is correct
        tempRuleScript.setTempObjectId(ruleElementId); //each time we are editing, we want to store the id in the tempRUleElement
        //addNlToTempRule();
        //ScreenLog.Log("SAVE CLICK");
        if (editModeProperty) //Objects from recs are always considered as edits
        {
            quickUpdateNl();
            // save the changes
            editRuleElement();
            // disable the canvas
            disableAllSecondaryCanvas();
            //Remove the exclamation mark
            ScreenLog.Log("MY REFERENCE OBJECT ID: " + tempRuleScript.getTempObjectReferenceId());
            string objectOrService = contextDataScript.isObjectOrServiceFromId(tempRuleScript.getTempObjectReferenceId());
            ScreenLog.Log("TEST OBJECT OR SERVICE EEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE: " +objectOrService);
            tempRuleScript.printTempRule();
            //myCapability = contextDataScript.singleObjectOrServiceCapability[myRuleElement.fullName]; // Mi serve per passarla a DrawRelatedInterface
            //myCapability = contextDataScript.singleObjectOrServiceCapability[tempRuleScript.getTemp.fullName]; // Mi serve per passarla a DrawRelatedInterface
            if (objectOrService == "o")
            {
                deactivateAssociatedExclamationMark();
            }
            //ScreenLog.Log("HEre 1");
            ruleElementCanvas.enabled = false;
            editModeProperty= false;
            //ScreenLog.Log("HEre 2");
            // unset the rule element in edit
            myRuleElementInEdit.id = 0;
            //ScreenLog.Log("HEre 3");
            servicesIconScript.enableServiceIcon();
            //ScreenLog.Log("HEre 4");
            return;
        }
        editModeProperty= false;
        // It could be done also in a listener
        // These are done separately because I want that the value is taken at the end, not at each modification. 
        if(intCanvas.isActiveAndEnabled == true)
        { 
            Text intInputField = GameObject.Find("IntText").GetComponent<Text>();
            TMP_Dropdown intDropdown = GameObject.Find("DropdownInt").GetComponent<TMP_Dropdown>();
            string myOperator = intDropdown.options[intDropdown.value].text;
            string intValue = intInputField.text;
            tempRuleScript.setTempValue(intValue);
            tempRuleScript.setTempOperator(myOperator);
        }
        if(boolCanvas.isActiveAndEnabled == true)
        { //Perché questo è fatto qua e non in un listener? 
            TMP_Dropdown boolDropdown = GameObject.Find("DropdownBool").GetComponent<TMP_Dropdown>();
            // Get the operator!!
            string myOperator = boolDropdown.options[boolDropdown.value].text;
            tempRuleScript.setTempValue("true");
            tempRuleScript.setTempOperator(myOperator);  
        }
        
        if(stringCanvas.isActiveAndEnabled == true)
        { 
            TMP_Text strInputField = GameObject.Find("StringText").GetComponent<TMP_Text>(); //
            string strValue = strInputField.text;
            tempRuleScript.resetTempOperator(); // String does not have operators. It should not be required, just for good measure
            ScreenLog.Log("SETTING STRING VALUE: " + strValue);
            tempRuleScript.setTempValue(strValue);
        }
        
        if(dateCanvas.isActiveAndEnabled == true)
        { 
            tempRuleScript.setTempValue(myFirstDateHour.text + ":" + myFirstDateMin.text);
            TMP_Dropdown dateDropdown = GameObject.Find("DropdownDate").GetComponent<TMP_Dropdown>();
            // Get the operator!!
            string myOperator = dateDropdown.options[dateDropdown.value].text;
            tempRuleScript.setTempOperator(myOperator);
            if (myOperator == "Between")
            {
                ScreenLog.Log("BETWEEN OP!");
                //tempRuleScript.setTempSecondValue(mySecondDateHour.text + ":" + mySecondDateMin.text);
                string firstValue = myFirstDateHour.text + ":" + myFirstDateMin.text;
                string secondValue = mySecondDateHour.text + ":" + mySecondDateMin.text;
                tempRuleScript.setTempValue(firstValue + " and " +secondValue);
            }
        }

        bool check = tempRuleScript.addToTempRule();
        if (check)
        {
            ScreenLog.Log("CHECK OK!!");
            // update NL (it is needed for rec system!) 
            // NL is not perfect, but works enought for the rs
            //ScreenLog.Log("IS ACTION????'");
            //ScreenLog.Log("MY VALUE: " + tempRuleScript.getTempValue());
            quickUpdateNl();
            // disable the canvas
            disableAllSecondaryCanvas();
            ruleElementCanvas.enabled = false;
            //anchorCreator.reactivateExclamationMarks();
            //anchorCreator.placeEcaSymbol(tempRuleScript.getTempECA());
            //updatePanelWithTempRuleElement();
            // CHECK, if it is a service don't call this!!
            string objectOrService = contextDataScript.isObjectOrServiceFromId(tempRuleScript.getTempObjectReferenceId());
            //myCapability = contextDataScript.singleObjectOrServiceCapability[myRuleElement.fullName]; // Mi serve per passarla a DrawRelatedInterface
            //myCapability = contextDataScript.singleObjectOrServiceCapability[tempRuleScript.getTemp.fullName]; // Mi serve per passarla a DrawRelatedInterface
            ScreenLog.Log("UPDATING PANEL WITH LAST RULE ELEMENT"); //QUA NON ARRIVA //
            if (objectOrService == "o")
            {
                updatePanelWithLastRuleElement();
                deactivateAssociatedExclamationMark(); //Ho aggiunto questa chiamata: mi sa che prima gli "inactive exclamation mark" non venivano disattivati, ma semplicemente "nascosti alla vista" 
                                                       //dall'excl mark attivo che si posizionava sopra di loro!!!
            }
            ScreenLog.Log("CALLING RECOMMENDER"); //QUA NON ARRIVA //
            // Destroy the old recs anchors (no need to destroy the objects, they are overwritten)
            
            // IF the toggle switch is deselected don't call RS
            anchorCreator.deactivateRecommendations();
            
            //getRecommendationsScript.fireRecommendation();
            
            // Delete the temp rule element
            tempRuleScript.resetTempRuleElement(); 
            
            ruleChecks.checkSaveRule();
            ruleChecks.checkRecommendRule();
            ruleChecks.checkOperatorNeeded();
            // unset the rule element in edit
            myRuleElementInEdit.id = 0;
        
            // Deactivate the flag "already used in rule" 
            editFromAlreadyOpened = false;

            // Re enable the services Icon
            servicesIconScript.enableServiceIcon();
        }
        else
        {
            ScreenLog.Log("RULE ELEMENT NOT SAVED");
            // Delete the temp rule element
            tempRuleScript.resetTempRuleElement(); 
        }
    }

    public void manageRemoveClick()
    {
        anchorCreator.UIOpen = false;
        //ScreenLog.Log(tempRuleScript.allRuleElementsDict.Count.ToString()); //
        //Get the rule element and the anchor object ids
        int ruleElementId = myRuleElementInEdit.id; // # This Id is correct
        int anchorObjectId = tempRuleScript.allRuleElementsDict[ruleElementId].refId;
        
        // Remove the element from TempRule 
        tempRuleScript.removeRuleElementFromId(ruleElementId);
        //ScreenLog.Log(tempRuleScript.allRuleElementsDict.Count.ToString());
        // disable the canvas
        disableAllSecondaryCanvas(); 
        ruleElementCanvas.enabled = false;
        
        // Delete the temp rule element
        tempRuleScript.resetTempRuleElement(); 
        editModeProperty= false;
        
        // unset the rule element in edit
        myRuleElementInEdit.id = 0;

        // UI Buttons / operators check
        ruleChecks.checkSaveRule();
        ruleChecks.checkRecommendRule();
        ruleChecks.checkOperatorNeeded();
        
        //Deactivate the active excl mark associated with the rule element
        anchorCreator.deactivateActiveExclamationMark(anchorObjectId);

        //Reactivate the excl mark associated with the rule element
        anchorCreator.reactivateSingleExclamationMark(anchorObjectId);
        
        // Deactivate the flag "already used in rule" 
        editFromAlreadyOpened = false;
        
        //Re enable the services Icon
        servicesIconScript.enableServiceIcon();

        return;
    }

    // Se proviene da un rule element appena inserito o da una rec deve riattivare il punto excl blu
    // Se proviene da un rule element modificato con editNewRule ... allora no
    public void manageDiscardClick()
    {
        ScreenLog.Log("DISCARD CLICK");
        anchorCreator.UIOpen = false;
        
        // disable the canvas
        disableAllSecondaryCanvas();
        ruleElementCanvas.enabled = false;
        // unset the rule element in edit
        
        // Remove the element from the ruleElements (if it is a rec, it is automatically added in the dict to generate the rule element)
        //ScreenLog.Log("AFTER EDIT MODE PROPERTY");
        // Delete the temp rule element
        tempRuleScript.resetTempRuleElement(); 
        editModeProperty= false; // WTF
        
        //Re enable the services Icon
        servicesIconScript.enableServiceIcon();

        if (editFromAlreadyOpened) // Horrible spaghetti
        {
            //ScreenLog.Log("COME FROME ALREADY USED RULE ELEMENT: EXIT WITHOUT RESTORING GREEN EXCLAMATION MARK");
            editFromAlreadyOpened = false;
            return;
        }

        if (!editModeProperty)
        {
            editFromAlreadyOpened = false;
            //ScreenLog.Log("NOT EDIT MODE PROPERTY");
            // Open again the exclamation mark
            if (!tempRuleScript.thisObjectAlreadyUsedInRules())
            {
                int ruleElementId = myRuleElementInEdit.id; // reactivate the single exlclamation mark basing on rule element ID
                if (ruleElementId == 0)  // We do not always have this id
                {
                    ObjectOrServiceCapability firstElement = myCapabilities[0]; //All the xpaths for a device referes to the same object id
                    string myObjectFullName = firstElement.capabilityFullName;
                    //    ScreenLog.Log("MY OBJ FULL NAME" + myObjectFullName);
                    if(contextDataScript.isObjectOrServiceFromCapabilityFullName(myObjectFullName) == "o") // We just want to reactivate the objects
                    {
                        int objectId = contextDataScript.fullNameToObjectID[myObjectFullName];
                        //ScreenLog.Log("MY OBJ ID" + objectId);
                        anchorCreator.reactivateSingleExclamationMark(objectId); 
                    }
                }
                else
                {
                    int anchorObjectId = tempRuleScript.allRuleElementsDict[ruleElementId].refId;
                    anchorCreator.reactivateSingleExclamationMark(anchorObjectId);
                }
            }
        }
        ScreenLog.Log("END OF MANAGEDISCARD");
        return;
    }

    public void disableAllSecondaryCanvas()
    {
        //ScreenLog.Log("I WILL DISABLE THE CANVAS");
        nextOperatorCanvas.enabled = false;
        eventConditionCanvas.enabled = false;
        enumCanvas.enabled = false;
        stringCanvas.enabled = false;
        intCanvas.enabled = false;
        doubleCanvas.enabled = false;
        boolCanvas.enabled = false;
        timeCanvas.enabled = false;
        dateCanvas.enabled = false;
        //ScreenLog.Log("DISABLED ALL");
    }

    public void hide()
    {
        anchorCreator.UIOpen = false;
        ruleElementCanvas = GameObject.Find("RuleElementCanvas").GetComponent<Canvas>();
        ruleElementCanvas.enabled = false;
    }


    // If dropdown value == "between" set visible, else hide
    public void checkBetweenDateListener(TMP_Dropdown dropdown)
    {
        ScreenLog.Log("HELLO BETWEEEN!!!!!!!!");
        if (dropdown.options[dropdown.value].text == "Between")
        {
            secondLineContainer.SetActive(true);
        }
        else
        {
            secondLineContainer.SetActive(false);
        }
    }

    public void checkCapabilityDropdownChange(TMP_Dropdown dropdown)
    {
        ScreenLog.Log(dropdown.options[dropdown.value].text);
        ObjectOrServiceCapability selectedCapability = myCapabilities[dropdown.value];
        tempRuleScript.setTempCapability(selectedCapability.capabilityFullName);
        ScreenLog.Log("CHECK DROPDOWN CHANGE !!!!!!!!!!!!!!!!!!!!");
        if (contextDataScript.isAction(selectedCapability.capabilityFullName))
        {
            tempRuleScript.setTempECA("action");
        }
        else
        {
            tempRuleScript.setTempECA("condition"); //default
        }
        drawRelatedInterface(selectedCapability);
    }
    public void checkNextOperatorDropdownChange(TMP_Dropdown dropdown)
    {
        ScreenLog.Log(dropdown.options[dropdown.value].text);
        //ObjectOrServiceCapability selectedCapability = myCapabilities[dropdown.value];
        tempRuleScript.setTempNextOperator(dropdown.options[dropdown.value].text);
    }
    public void checkEnumDropdownChange(TMP_Dropdown dropdown)
    {
        ScreenLog.Log(dropdown.options[dropdown.value].text);
        //ObjectOrServiceCapability selectedCapability = myCapabilities[dropdown.value];
        tempRuleScript.setTempValue(dropdown.options[dropdown.value].text);
    }
    public void updateNlPartialTriggerWithValue()
    {
        string myDescriptiveName = contextDataScript.getDescriptiveNameFromFullName(tempRuleScript.getTempCapability());
        //string myNl = nl.generatePartialTriggerNLWithValue(tempRuleScript.getTempECA(), tempRuleScript.getTempCapability(), tempRuleScript.getTempOperator(), tempRuleScript.getTempValue()); //
        string myNl = nl.generatePartialTriggerNLWithValue(tempRuleScript.getTempECA(), myDescriptiveName, tempRuleScript.getTempOperator(), tempRuleScript.getTempValue()); //
        myNlText.text = myNl;
        tempRuleScript.setTempNl(myNl);
    }
    public void updateNlPartialActionWithValue()
    {
        string myDescriptiveName = contextDataScript.getDescriptiveNameFromFullName(tempRuleScript.getTempCapability());
        //string myNl = nl.generatePartialActionNLWithValueMap(tempRuleScript.getTempCapability(), tempRuleScript.getTempValue()); //
        string myNl = nl.generatePartialActionNLWithValue(myDescriptiveName, tempRuleScript.getTempValue()); //
        myNlText.text = myNl;
        ScreenLog.Log("IM SETTING NL: " + myNl);
        tempRuleScript.setTempNl(myNl);
    }


    public void updateNlPartialTrigger()
    {
        string myCapability = tempRuleScript.getTempCapability();
        //ScreenLog.Log("AAAAAAAAAAAAAAAA My capability:" + myCapability);
        string myDescriptiveName = contextDataScript.getDescriptiveNameFromFullName(myCapability);
        //ScreenLog.Log("BBBBBBBBBBBB My descriptive name:" + myDescriptiveName);
        //string myNl = nl.generatePartialTriggerNL(tempRuleScript.getTempECA(), tempRuleScript.getTempCapability()); //
        string myNl = nl.generatePartialTriggerNL(tempRuleScript.getTempECA(), myDescriptiveName); //
        //ScreenLog.Log("CCCCCCCCCC My NL" + myNl);
        myNlText.text = myNl;
        tempRuleScript.setTempNl(myNl);
        ScreenLog.Log("OK!");
    }

    public void updateNlPartialAction()
    {
        string myDescriptiveName = contextDataScript.getDescriptiveNameFromFullName(tempRuleScript.getTempCapability());
        //string myNl = nl.generatePartialActionNL(tempRuleScript.getTempCapability()) ;
        string myNl = nl.generatePartialActionNL(myDescriptiveName) ;
        myNlText.text = myNl;
        tempRuleScript.setTempNl(myNl);
    }

    public void showEventConditionSelection()
    {
        eventConditionCanvas.enabled = true;
    }
    public void hideEventConditionSelection()
    {
        eventConditionCanvas.enabled = false;
    }

    public void drawRelatedInterface(ObjectOrServiceCapability ObjectOrServiceCapability)
    {
        ScreenLog.Log("DRAW INTERFACE!"); // ARRIVA QUA 
        string eca = ObjectOrServiceCapability.capabilityType; 
        string desc = ObjectOrServiceCapability.capabilityDesc;
        string myDataType = ObjectOrServiceCapability.capabilityDataType;
        string capability = ObjectOrServiceCapability.capabilityFullName;
        ScreenLog.Log("DRAW INTERFACE1!"); // ARRIVA QUA 
        ScreenLog.Log(eca + " " + myDataType + " " + capability);
        if (eca == "t" || eca == "trigger")
        {
            showEventConditionSelection();
            updateNlPartialTrigger();
            //showNextOperatorCanvas();
        }
        else if(eca == "a" || eca == "action")
        {
            hideEventConditionSelection();
            updateNlPartialAction();
            //hideNextOperatorCanvas();
        }
        
        ScreenLog.Log("DRAW INTERFACE2!"); // ARRIVA QUA 
        if (myDataType == "ENUM") // //
        {
            ScreenLog.Log("DRAWING ENUM INTERFACE");
            enumCanvas.enabled = true;
            intCanvas.enabled = false;
            stringCanvas.enabled = false;
            boolCanvas.enabled = false;
            timeCanvas.enabled = false;
            dateCanvas.enabled = false;
            ScreenLog.Log(contextDataScript.ToString());
            List<string> dropOptions = contextDataScript.ruleEnumType[capability]; 
            ScreenLog.Log("GOT THE DROP OPTIONS");
            TMPro.TMP_Dropdown[] myDropDownList = enumCanvas.GetComponentsInChildren<TMPro.TMP_Dropdown>();
            TMPro.TMP_Dropdown myDropdown = myDropDownList[0]; 
            myDropdown.options.Clear();
            foreach (string option in dropOptions)
            {
                ScreenLog.Log(option);
                myDropdown.options.Add(new TMPro.TMP_Dropdown.OptionData(option));
            }
            //myDropdown.AddOptions(m_DropOptions);
            ScreenLog.Log("ADDED THE DROP OPTIONS");
            myDropdown.RefreshShownValue(); // 
            myDropdown.value = -1; //Ook?
            ScreenLog.Log("SETTED DEFAULT, END");

        }
        else if(myDataType == "STRING") //
        {
            stringCanvas.enabled = true;
            intCanvas.enabled = false;
            enumCanvas.enabled = false;
            boolCanvas.enabled = false;
            timeCanvas.enabled = false;
            dateCanvas.enabled = false;

        }
        else if(myDataType == "INTEGER")
        {
            intCanvas.enabled = true;
            stringCanvas.enabled = false;
            enumCanvas.enabled = false;
            boolCanvas.enabled = false;
            timeCanvas.enabled = false;
            dateCanvas.enabled = false;

        }
        else if(myDataType == "DOUBLE")
        {
            intCanvas.enabled = true; // For the moment int and double canvas are the same

        }
        else if(myDataType == "BOOLEAN")
        {
            boolCanvas.enabled = true;
            stringCanvas.enabled = false; //
            intCanvas.enabled = false;
            enumCanvas.enabled = false;
            timeCanvas.enabled = false;
            dateCanvas.enabled = false;

        }
        else if(myDataType == "DATE")
        {
            dateCanvas.enabled = true;
            boolCanvas.enabled = false;
            stringCanvas.enabled = false; //
            intCanvas.enabled = false;
            enumCanvas.enabled = false;
            timeCanvas.enabled = false;

        }
        else if(myDataType == "TIME")
        {
            timeCanvas.enabled = true; //
            boolCanvas.enabled = false;
            stringCanvas.enabled = false; //
            intCanvas.enabled = false;
            enumCanvas.enabled = false;
            dateCanvas.enabled = false;

        }
        ScreenLog.Log("DRAW INTERFACE END");
    }

    public void showNextOperatorCanvas() //
    {
        nextOperatorCanvas.enabled = true;
    }
    public void hideNextOperatorCanvas() //
    {
        nextOperatorCanvas.enabled = false;
    }
    public void showHideNextOperatorCanvasAtStart(List<ObjectOrServiceCapability> myCapabilities)
    {
        ObjectOrServiceCapability firstElement = myCapabilities[0];
        string myType = firstElement.capabilityType;
        if(myType == "t")
        {
            nextOperatorCanvas.enabled = true;
        }
        return;
        // TODO: in the future, make it better, for example:
        /*
        if (editMode)
        {
            // get the selected capability, show or hide basing on that
        }
        else
        {
            // return basing on the first element of the capability list
        }
        */
    }

    /**
     * 
     */
    public void showNew(int referenceObjId, string myName) // Inizia a debuggare da qua 
    {
        anchorCreator.UIOpen = true;
        //ScreenLog.Log("start showNew"); // QUA ARRIVA
        editModeProperty = false;
        // Set tempObjId!!!
        tempRuleScript.setTempObjectReferenceId(referenceObjId);
        ruleElementCanvas.enabled = true;
        //ScreenLog.Log(myName);
        //ScreenLog.Log(referenceObjId.ToString()); //
        //myCapabilities = contextDataScript.objectsCapabilities[myName]; // Probabilmente crasha questo //
        myCapabilities = contextDataScript.getObjectOrServicesCapabilitiesFromId(referenceObjId);
        myCapabilityDropdown.options.Clear();
        //ScreenLog.Log("before foreach "); // QUA NON ARRIVA!!!!!!!!!!!!
        foreach (ObjectOrServiceCapability capability in myCapabilities)
        {
            myCapabilityDropdown.options.Add(new TMP_Dropdown.OptionData(capability.capabilityDesc));
        }
        //ScreenLog.Log("after foreach ");
        myCapabilityDropdown.value = -1; //Ook?
        showHideNextOperatorCanvasAtStart(myCapabilities);
        tempRuleScript.setTempNextOperator("none"); //none is set by default
        //ScreenLog.Log("end showNew");
        return;
    }
    
   
    // TODO How to pre-select elements basing on the clicked recommendation??
    public void showFromRecommendation(RuleElement myRuleElement)
    {
        anchorCreator.UIOpen = true;
        ScreenLog.Log("start showedit : " + myRuleElement.fullName); // OK
        string myType = contextDataScript.isObjectOrServiceFromId(myRuleElement.refId);
        myRuleElementInEdit = myRuleElement;
        editModeProperty= true;
        int myReferenceObjId = myRuleElement.refId; // Qua arriva, se l'oggetto non è presente setta -1
        // Set tempObjId!!!
        tempRuleScript.setTempObjectReferenceId(myReferenceObjId); // Qua arriva
        string myName = myRuleElement.fullName;
        //ScreenLog.Log("MY NAME: " + myName);
        //ScreenLog.Log("MY REFERENCE OBJ ID " + myReferenceObjId);
        //string myObjReferenceName = contextDataScript.activableObjects[myReferenceObjId].realName;
        //ScreenLog.Log("myObjReferenceName " +myObjReferenceName);
        //myCapabilities = contextDataScript.objectsCapabilities[myObjReferenceName]; // Non usiamo più il nome come referenceObj ma l'ID!!
        myCapability = contextDataScript.singleObjectOrServiceCapability[myRuleElement.fullName]; // Mi serve per passarla a DrawRelatedInterface
        //ScreenLog.Log("AFTER MYCAPABILITY");
        if(myType == "o")
        {
            //ScreenLog.Log("BEFORE OBJECT CAPABILITY"+myCapabilities.Count.ToString());
            myCapabilities = contextDataScript.objectsCapabilities[myReferenceObjId]; // TODO CHECK!!!! //
            //ScreenLog.Log("AFTER OBJECT CAPABILITY"+myCapabilities.Count.ToString());
        }
        else if(myType == "s")
        {
            //ScreenLog.Log("BEFORE SERVICE CAPABILITY"+myCapabilities.Count.ToString());
            myCapabilities = contextDataScript.serviceCapabilities[myReferenceObjId]; // TODO CHECK!!!! //
            //ScreenLog.Log("AFTER SERVICE CAPABILITY"+myCapabilities.Count.ToString());

        }
        else
        {
            ScreenLog.Log("ERROR! My type is not object nor service!!"); //
        }
        ruleElementCanvas.enabled = true;
        myCapabilityDropdown.options.Clear();
        foreach (ObjectOrServiceCapability capability in myCapabilities)
        {
            //TMP_Dropdown.OptionData myOption = new TMP_Dropdown.OptionData(capability.capabilityFullName);
            TMP_Dropdown.OptionData myOption = new TMP_Dropdown.OptionData(capability.capabilityDesc);
            //myOption.text = capability.capabilityDesc; //test
            myCapabilityDropdown.options.Add(myOption);
            //myCapabilityDropdown.options.Add(new TMP_Dropdown.OptionData(capability.capabilityDesc));
        }
        // Take the capability in the suggestion and select it in the rule element panel
        int suggestionIndex = getCapabilityIndexInDropdown(myCapabilityDropdown, contextDataScript.getDescriptionFromFullName(myRuleElement.fullName));
        myCapabilityDropdown.value = suggestionIndex;
        myCapabilityDropdown.RefreshShownValue(); // test
        drawRelatedInterface(myCapability); 
        showHideNextOperatorCanvasAtStart(myCapabilities);
        tempRuleScript.setTempNextOperator("none"); //none is set by default

        ScreenLog.Log("end showedit");
        return;

    }

  public int getCapabilityIndexInDropdown(TMP_Dropdown myDropdown, string desc)
    {
        //ScreenLog.Log("DESC TO MATCH");
        //ScreenLog.Log(desc);
        int index = 0;
        foreach (TMP_Dropdown.OptionData option in myDropdown.options)
        {
            //ScreenLog.Log(option.text);
            if (option.text == desc)
            {
                //ScreenLog.Log("MATCH!!!");
                return index;
            }
            index++;
        }
        return -1;
    }

    public void showEdit(RuleElement myRuleElement)
    {
        anchorCreator.UIOpen = true;
        myRuleElementInEdit = myRuleElement;
        ScreenLog.Log("SHOWEDIT!!!!!!!!!!!!!!!!!!!!");
        editModeProperty= true;
        int myReferenceObjId = myRuleElement.refId;
        // Set tempObjId!!!
        tempRuleScript.setTempObjectReferenceId(myReferenceObjId);
        string myName = myRuleElement.fullName;
        string myObjReferenceName = contextDataScript.activableObjects[myReferenceObjId].realName;
        // test!!
        ruleElementCanvas.enabled = true;
        ScreenLog.Log("OK?"+myName);
        //myCapabilities = contextDataScript.objectsCapabilities[myObjReferenceName]; // Non usiamo più il nome come referenceObj ma l'ID!!
        myCapabilities = contextDataScript.objectsCapabilities[myReferenceObjId]; // TODO CHECK!!!! //
        ScreenLog.Log("OK?"+myCapabilities.Count.ToString());
        myCapabilityDropdown.options.Clear();
        foreach (ObjectOrServiceCapability capability in myCapabilities)
        {
            myCapabilityDropdown.options.Add(new TMP_Dropdown.OptionData(capability.capabilityDesc));
        }
        myCapabilityDropdown.value = -1; //Ook?
        showHideNextOperatorCanvasAtStart(myCapabilities);
        tempRuleScript.setTempNextOperator("none"); //none is set by default
        ScreenLog.Log("end showedit");
        return;
    }


    void getData()
    {
        ScreenLog.Log(tempRuleScript.getTempECA());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
