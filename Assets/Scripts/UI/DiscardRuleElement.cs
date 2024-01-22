using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscardRuleElement : MonoBehaviour
{
    public Canvas myRuleElementCanvas;
    public GameObject myDynamicContainerCanvas;
    public Button buttonDiscardRuleElement;
    public AnchorCreator anchorCreator; 
    public TempRule tempRuleScript;
    
    // Start is called before the first frame update
    void Start()
    {
        //mainMenuCanvas = GameObject.Find("MainMenuCanvas").GetComponent<Canvas>();
        buttonDiscardRuleElement = GameObject.Find("DiscardRuleElement").GetComponent<Button>();
        buttonDiscardRuleElement.onClick.AddListener(delegate () { discardRuleElementManager(); });
        anchorCreator = FindObjectOfType<AnchorCreator>(); 
        tempRuleScript= FindObjectOfType<TempRule>(); 
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /**
     * Clean the temp rule element and the canvases 
     */
    void discardRuleElementManager()
    {
        // disable the dropdown canvas
        Canvas[] myCanvasArray = myDynamicContainerCanvas.GetComponentsInChildren<Canvas>();
        foreach (Canvas canvasElement in myCanvasArray) //It is better to take the type of element and only disable that canvas, but for now ok
        {
            canvasElement.enabled = false;
        }
        // disable the canvas
        myRuleElementCanvas.enabled = false;
        // Delete the temp rule element
        tempRuleScript.resetTempCapability();
        tempRuleScript.resetTempECA();
        tempRuleScript.resetObjectReferenceId();
        // Open again the exclamation mark
        anchorCreator.reactivateExclamationMarks();
    }
}
