using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetIconScript : MonoBehaviour
{
    public TempRule tempRuleScript;
    public AnchorCreator anchorCreator;
    public Button resetButton;
    public RuleChecks ruleChecks;

    // Start is called before the first frame update
    void Start()
    {
        ruleChecks = FindObjectOfType<RuleChecks>();  //
        tempRuleScript = FindObjectOfType<TempRule>(); 
        anchorCreator = FindObjectOfType<AnchorCreator>(); 
        resetButton.onClick.AddListener(delegate
        {
            manageResetClick();
        });
        
    }

    public void manageResetClick()
    {
        tempRuleScript.resetRule();
        anchorCreator.resetAllOjects();
        ruleChecks.checkSaveRule(); // To hide the save icon
        //ScreenLog.Log("RESET!!!!!!!!!!!!!!!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
