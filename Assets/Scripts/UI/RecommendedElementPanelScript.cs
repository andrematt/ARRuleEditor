using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecommendedElementPanelScript : MonoBehaviour
{
    public Vector3 myPosition;
    public NL nl;
    public TempRule tempRuleScript;
    public AnchorCreator anchorCreator;
    //public Text myNl;
    public TMP_Text myNlPro;
    private int objectReferenceId; 
    private float myYValue; 

    public int getObjectReferenceId()
    {
        return objectReferenceId;
    }
    public float getMyY()
    {
        return myYValue;
    }

    // Start is called before the first frame update
    void Start()
    {
        anchorCreator = FindObjectOfType<AnchorCreator>();  //
        tempRuleScript = FindObjectOfType<TempRule>(); 
        nl = FindObjectOfType<NL>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setInfo(SingleEntry entry, int myObjReferenceId, float myY)
    {
        string entryDesc = nl.generateEntryDescription(entry);  //
        //entryDesc = "Turn the Entrance Light ON"; //TEST Screenshot
        myNlPro.text = entryDesc;
        objectReferenceId = myObjReferenceId;
        myYValue = myY;
    }

}
