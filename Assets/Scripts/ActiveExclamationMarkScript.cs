using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ActiveExclamationMarkScript : MonoBehaviour
{
    public Vector3 myPosition;
    public AnchorCreator anchorCreator;
    public NL nl;
    public TempRule tempRuleScript;
    public ParticleSystem myParticleSystem;
    public int myObjectReferenceId;
    public List<int> myRuleElementsIds;
    public string myName;
    public string myLocation;
    public string myRealName;
    public string myElementTypeStr;
    public string myReferenceObjectStr;
    public string myCapabilityStr;
    public string mySettingStr;
    public string myNextOperatorStr;
    // Start is called before the first frame update
    void Start()
    {
        myParticleSystem.Stop();
        //selectedForUseInRuleScript = FindObjectOfType<SelectedForUseInRuleScript>();
        anchorCreator = FindObjectOfType<AnchorCreator>(); // 
        tempRuleScript = FindObjectOfType<TempRule>(); 
        nl = FindObjectOfType<NL>();
        
    }
    public void addToRuleElementId(int id)
    {
        if (!myRuleElementsIds.Contains(id)) //Horrible hack to prevent duplicates...
        {
            myRuleElementsIds.Add(id);
        }
    }
    public void removeFromRuleElementId(int id)
    {
        myRuleElementsIds.Remove(id);
    }

    public List<int> getRuleElementsIds() {
        return myRuleElementsIds;
    }
    public void setPosition(Vector3 position)
    {
        myPosition = position;
    }
    public Vector3 getPosition()
    {
        return myPosition;
    }
    public string getName()
    {
        return myName;
    }
    public string getRealName()
    {
        return myRealName;
    }
    public int getReferenceObjectId()
    {
        return myObjectReferenceId;
    }
    public void setInfo(int objectReferenceId, string name, string realName, string location)
    {
        myName = name;
        myRealName = realName;
        myLocation = location;
        myObjectReferenceId = objectReferenceId;
    }

    public void startParticle()
    {
        myParticleSystem.Play();
    }

    public void stopParticle()
    {
        myParticleSystem.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
