using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitIconScript : MonoBehaviour
{
    public MainMenuScript mainMenuScript;
    public TempRule tempRuleScript;
    public AnchorCreator anchorCreator;
    public Button exitButton;
    public RuleChecks ruleChecks;
    public Canvas mainMenuCanvas;
    public Canvas ruleMapCanvas; 

    // Start is called before the first frame update
    void Start()
    {
        ruleChecks = FindObjectOfType<RuleChecks>();  //
        tempRuleScript = FindObjectOfType<TempRule>(); 
        anchorCreator = FindObjectOfType<AnchorCreator>();
        mainMenuScript = FindObjectOfType<MainMenuScript>();
        exitButton.onClick.AddListener(delegate
        {
            manageExitClick();
        });
        
    }
    public void manageExitClick()
    {
        tempRuleScript.resetRule();
        anchorCreator.resetAllOjects();
        ruleChecks.checkSaveRule(); // To hide the save icon
        // m_MainMenuCanvas = GameObject.Find("MainMenuCanvas").GetComponent<MainMenuScript>(); //altro modo di prendere mainmenuscript
        //MainMenuScript mainMenuScript = mainMenuCanvas.GetComponent <MainMenuScript>();
        mainMenuScript.setViewAR(false);
        mainMenuScript.showMainMenu();  
        ruleMapCanvas.enabled = false;
        ScreenLog.Log("Back to main menu complete");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
