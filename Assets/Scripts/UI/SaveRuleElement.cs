using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveRuleElement : MonoBehaviour
{
    public Canvas myRuleElementCanvas;
    public GameObject myDynamicContainerCanvas;
    public Button buttonSaveRuleElement;
    public AnchorCreator anchorCreator; 
    public TempRule tempRuleScript;
    // Start is called before the first frame update
    void Start()
    {
        buttonSaveRuleElement = GameObject.Find("SaveRuleElement").GetComponent<Button>();
        buttonSaveRuleElement.onClick.AddListener(delegate () { saveRuleElementManager(); });
        anchorCreator = FindObjectOfType<AnchorCreator>(); 
        tempRuleScript= FindObjectOfType<TempRule>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    /**
     *  
     */
    void saveRuleElementManager()
    {
        ScreenLog.Log("SAVE RULE ELEMENT MANAGER");
        // get the value from the dropdown canvas
        string myCapability = tempRuleScript.getTempCapability();
        //ObjectOrServiceCapability myCapabilities = anchorCreator.getSingleObjectOrServiceCapability(myCapability); // WTF
        // Get the data from the canvas
        Canvas[] myCanvasArray = myDynamicContainerCanvas.GetComponentsInChildren<Canvas>(); //
        foreach (Canvas canvasElement in myCanvasArray)
        {
            canvasElement.enabled = false;
        }
        // disable the canvas
        myRuleElementCanvas.enabled = false;
        // send the temp rule element to temprule to be added to the rule in construction
        // :) 
        // Open again the exclamation mark
        anchorCreator.reactivateExclamationMarks();
    }

}
