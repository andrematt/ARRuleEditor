using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UsedInRulePanelScript : MonoBehaviour
{
    public NL nl;
    public TempRule tempRuleScript;
    public AnchorCreator anchorCreator;
    public TMP_Text myNlProName;
    public TMP_Text myNlProDesc;
    private int myObjReferenceId;
    private float myY;
    // Start is called before the first frame update
    void Start()
    {
        anchorCreator = FindObjectOfType<AnchorCreator>();  //
        tempRuleScript = FindObjectOfType<TempRule>(); 
        nl = FindObjectOfType<NL>();
        
    }

    public int getObjReferenceId()
    {
        return myObjReferenceId;
    }
    public float getMyY()
    {
        return myY;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setInfo(string name, string nl, int objRefId, float yValue)
    {
        myY = yValue;
        myObjReferenceId = objRefId;
        myNlProName.text = "<b>" +name + "</b>";
        //nl = "WHEN the Entrance Door becomes Open IF time is between 20:00 and 23:00 THEN Turn the Entrance Light ON "; //TEST Screenshot
        myNlProDesc.text = nl;
        //myNlPro.text = "<b>Rule Name:</b>\n<b>" +name + "</b>\n<b>Description:</b>\n" + nl;
    }
}
