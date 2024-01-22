using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class RecommendationsIconScript : MonoBehaviour
{
    public Canvas recommendationsIconCanvas;
    public Canvas recommendationsCanvas;
    public Canvas ruleMapCanvas;
    public Button getRecsButton;
    public TempRule tempRule;

    // Start is called before the first frame update
    void Start()
    {
        recommendationsIconCanvas = GameObject.Find("RecommendationsIconCanvas").GetComponent<Canvas>();
        recommendationsCanvas = GameObject.Find("RecommendRuleCanvas").GetComponent<Canvas>(); 
        recommendationsIconCanvas.enabled = false;
        tempRule = FindObjectOfType<TempRule>();  
        
        getRecsButton.onClick.AddListener(delegate
        {
            manageGetRecsClick();
        });
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /**
     * Retreive the recs from tempRule
     */
    void manageGetRecsClick()
    {
        List<SingleEntry> recommendations = tempRule.getRecommendations();

        // Pass the retreived recs to the RecommendRuleCanvas script
        //anchorCreator.UIOpen = true; //
        recommendationsCanvas.enabled = true;
        ruleMapCanvas.enabled = false;
        RecommendRuleCanvasScript recommendRuleCanvasScript = FindObjectOfType<RecommendRuleCanvasScript>();  //
        recommendRuleCanvasScript.getReceivedRecommendations(recommendations);
        //ScreenLog.Log(jsonResponse);
        return;
    }
    public void enableRecButton()
    {
        recommendationsIconCanvas.enabled = true;
    }
    public void disableRecButton()
    {
        recommendationsIconCanvas.enabled = false;
    }
}
