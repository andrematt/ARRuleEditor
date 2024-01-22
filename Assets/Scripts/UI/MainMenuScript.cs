using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    bool detectObjects = false;
    bool viewAR = false;
    public ServicesIconScript serviceIconScript; 
    public RuleSaveAndLoad ruleSaveAndLoad; 
    public ContextData contextData; 
    public Button buttonSaveObjectsLocation;
    public Button buttonStartRuleEditor;
    public Button buttonLoadSavedRules;
    public Button buttonExploreRules;
    private Canvas mainMenuCanvas;
    private Canvas loadRuleCanvas;
    // Start is called before the first frame update //
    void Start()
    {
        mainMenuCanvas = GameObject.Find("MainMenuCanvas").GetComponent<Canvas>();
        loadRuleCanvas = GameObject.Find("LoadRuleCanvas").GetComponent<Canvas>();
        
        buttonStartRuleEditor = GameObject.Find("StartRuleEditor").GetComponent<Button>();
        buttonStartRuleEditor.onClick.AddListener(delegate () { onClickViewAR(); });

        buttonLoadSavedRules = GameObject.Find("LoadSavedRules").GetComponent<Button>();
        buttonLoadSavedRules.onClick.AddListener(delegate () { onClickLoadSavedRules(); });
        
        buttonSaveObjectsLocation = GameObject.Find("SaveObjectsLocation").GetComponent<Button>();
        buttonSaveObjectsLocation.onClick.AddListener(delegate () { onClickSaveObjectsLocation(); });
        
        buttonExploreRules = GameObject.Find("ExploreEnvironment").GetComponent<Button>();
        buttonExploreRules.onClick.AddListener(delegate () { onClickExploreRules(); }); //

        ruleSaveAndLoad = FindObjectOfType<RuleSaveAndLoad>();
        serviceIconScript = FindObjectOfType<ServicesIconScript>();
        contextData = FindObjectOfType<ContextData>();
        serviceIconScript.disableServiceIcon();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showMainMenu()
    {
        setViewAR(false);
        mainMenuCanvas.enabled = true;
    }

    public void setDetectObjects(bool setValue)
    {
        detectObjects = setValue;
    }

    public void setViewAR(bool setValue)
    {
        viewAR = setValue; //I am not sure that viewAR should be a property of mainMenu (better in anchorCreator?)
        //AnchorCreator anchorCreator = FindObjectOfType<AnchorCreator>(); 
        //anchorCreator.placeSavedAnchors();
    }


    public void updateAllObjectUsedInRulesDict(List<Rule> myRules)
    {
        //ScreenLog.Log("BEGIN UPDATE ALL OBJECT IN RULES");
        foreach (Rule rule in myRules)
        {
            //ScreenLog.Log("ITERATION START");
            contextData.updateUsedInRulesDict(rule);
            //ScreenLog.Log("ITERATION END");
        }
        //ScreenLog.Log("END UPDATE ALL OBJECT IN RULES");

    }

    public void onClickExploreRules()
    {
        setViewAR(true);
        AnchorCreator anchorCreator = FindObjectOfType<AnchorCreator>(); 
        mainMenuCanvas.enabled = false;
        ScreenLog.Log("load rules");
        List<Rule> myRules= new List<Rule>();
        if (ruleSaveAndLoad.checkSavedRulesListExistence())
        {
            myRules = ruleSaveAndLoad.getRules();
        }
        updateAllObjectUsedInRulesDict(myRules);
        anchorCreator.placeSavedAnchorsExplore(myRules);
        setDetectObjects(false);
        // TODO: place a viz to show if an object is used in rules
    }

    public void onClickSaveObjectsLocation() //
    {
        setViewAR(false);
        AnchorCreator anchorCreator = FindObjectOfType<AnchorCreator>(); 
        mainMenuCanvas.enabled = false;
        anchorCreator.placeSavedAnchors();
        setDetectObjects(true);
    }
    public void onClickViewAR()
    {
        setViewAR(true); //...
        serviceIconScript.enableServiceIcon(); //Also add the Services icon
        AnchorCreator anchorCreator = FindObjectOfType<AnchorCreator>(); 
        mainMenuCanvas.enabled = false;
        anchorCreator.placeSavedAnchors();
        setDetectObjects(false);
    }
    public void onClickSaveObjectsLocationOld()
    {
        mainMenuCanvas.enabled = false;
        setViewAR(false);
        setDetectObjects(true);
    }
    public void onClickViewAROld()
    {
        mainMenuCanvas.enabled = false;
        setViewAR(true);
        setDetectObjects(false);
    }

    public void onClickLoadSavedRules()  //
    {
        ScreenLog.Log("load saved rules");
        mainMenuCanvas.enabled = false;
        loadRuleCanvas.enabled = true;
        LoadRuleCanvasScript loadRuleScript = (LoadRuleCanvasScript)GameObject.Find("LoadRuleCanvas").GetComponent("LoadRuleCanvasScript");
        loadRuleScript.show();
        return;
    }

    public bool getDetectObjects()
    {
        return detectObjects;
    }
    public bool getViewAR()
    {
        return viewAR;
    }

}
