using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ServiceScript : MonoBehaviour
{
    public Canvas serviceCanvas;
    public Button backButton;
    public Button loadButton;
    public ServicesIconScript serviceIconScript; 
    public TMP_Dropdown myServicesListDropdown;
    public AnchorCreator anchorCreator;
    public ContextData contextDataScript;
    public RuleElementScript ruleElementScript;
    public string selectedService = "";
    // Start is called before the first frame update
    void Start()
    {
        serviceIconScript = FindObjectOfType<ServicesIconScript>();
        ruleElementScript = FindObjectOfType<RuleElementScript>();  //
        contextDataScript = FindObjectOfType<ContextData>();  //
        anchorCreator = FindObjectOfType<AnchorCreator>();  //
        serviceCanvas.enabled = false; //
        
        myServicesListDropdown.onValueChanged.AddListener(delegate
        {
            manageSelectedService(myServicesListDropdown);
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


    public void drawServices()
    {
        anchorCreator.UIOpen = true;
        myServicesListDropdown.options.Clear();
        foreach (var element in contextDataScript.activableServices)
        {
            string entryFullName = element.Value.parent + "-" +element.Value.realName;  //
            myServicesListDropdown.options.Add(new TMP_Dropdown.OptionData(entryFullName)); //
            //myServicesListDropdown.options.Add(new TMP_Dropdown.OptionData(element.Value.realName)); // here the fullName is not used, because we are talking about services ("channels"), not capabilities! So there is no "fullName"
        }
        myServicesListDropdown.value = -1;
    }

    public void manageSelectedService(TMP_Dropdown myRuleList)
    {
        ScreenLog.Log("CHANGED VALUE");
        ScreenLog.Log(myRuleList.options[myRuleList.value].text);
        selectedService = myRuleList.options[myRuleList.value].text;
        return;

    }
    public void manageBackClick()
    {
        anchorCreator.UIOpen = false;
        serviceIconScript.enableServiceIcon();
        serviceCanvas.enabled = false;
    }

    /**
     * Just send the service ID to "showNew" (as the same way as when an object is selected)
     */
    public void manageLoadClick()
    {
        ScreenLog.Log("CLICK");
        ScreenLog.Log("Service: " + selectedService);
        if (selectedService != "")
        {
            //int myServiceId = contextDataScript.getServiceIdFromFullName(selectedService);
            int myServiceId = contextDataScript.getServiceIdFromServiceFullName(selectedService);
            if (myServiceId != -1)
            {
                ruleElementScript.showNew(myServiceId, selectedService);  // Id is used to actually draw interface, the service name is just for logging
            }
            else
            {
                ScreenLog.Log("NOT FOUND");
                throw new Exception("NO ID ASSOCIATED TO THIS SERVICE FULLNAME"); //
            }
        }
        //ScreenLog.Log("END MANAGELOADCLICK");
        anchorCreator.UIOpen = false;
        serviceCanvas.enabled = false;
    }
}
