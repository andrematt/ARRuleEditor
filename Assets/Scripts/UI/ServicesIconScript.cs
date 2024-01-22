using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class ServicesIconScript : MonoBehaviour
{
    public Canvas servicesIconCanvas;
    public Canvas servicesCanvas;
    public Canvas ruleMapCanvas;
    public Button getServicesButton;
    public TempRule tempRule;
    // Start is called before the first frame update
    void Start()
    {
        //recommendationsIconCanvas.enabled = false;
        tempRule = FindObjectOfType<TempRule>();  
        
        getServicesButton.onClick.AddListener(delegate
        {
            manageGetServicesClick(); //
        });
        
    }

    public void enableServiceIcon()
    {
        servicesIconCanvas.enabled = true;
    }
    public void disableServiceIcon()
    {
        servicesIconCanvas.enabled = false;
    }

    public void manageGetServicesClick()
    {
        ruleMapCanvas.enabled = false;
        disableServiceIcon();
        servicesCanvas.enabled = true;
        ServiceScript serviceScript = (ServiceScript) servicesCanvas.GetComponent("ServiceScript"); //
        serviceScript.drawServices();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
