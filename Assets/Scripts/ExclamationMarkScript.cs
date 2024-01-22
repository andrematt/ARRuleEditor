using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ExclamationMarkScript : MonoBehaviour
{
    private Transform myTransform;
    //public GameObject myGameObject;
    public Vector3 myPosition;
    public string myName;
    public string myRealName;
    public string myLocation;
    public string myRelatedObjectName;
    public string myRelatedObjectRealName;
    public int myRelatedObjectId;
    //private List<int> myRuleElementsIds; // An inactive exclamation mark should not have associated rule elements!!! check if correct
    
    // Start is called before the first frame update
    void Start()
    {
        ScreenLog.Log("START INACTIVE EXCL MARK!!!!");
    }

    public void stop()
    {
        //myGameObject.transform.DOKill();
    }
   
    /*
    public void move()
    {
        
        //Vector3 myStartPosition = myGameObject.transform.position;
        //Vector3 myEndPosition = myGameObject.transform.position;
        //myEndPosition[1] += 0.1f;  // move the Exclamation Mark a little up
        //myGameObject.transform.DOMoveY(0.3f, 0.8f).SetEase(Ease.OutQuint); 
        
        //myGameObject.transform.DOMove(myStartPosition, 0); 
        //myGameObject.transform.DOMove(myEndPosition, 3).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo); 
    }
    */

    // Update is called once per frame
    void Update()
    {
    }
    public void setInfo(string name, string relatedObjectName, string relatedObjectRealName, int relatedObjectId, string realName, string location) //
    {
        myName = name;
        myRelatedObjectName = relatedObjectName;
        myRelatedObjectRealName = relatedObjectRealName;
        myRelatedObjectId = relatedObjectId;
        myRealName = realName;
        myLocation = location;
    }

    /*
    public void addToRuleElementId(int id)
    {
        myRuleElementsIds.Add(id);
    }
    */
    //public void setRuleElementsIds(int id)

    /*
    public List<int> getRuleElementsIds() {
        return myRuleElementsIds;
    }
    */

    public string getName()
    {
        return myName;
    }
    public int getReferenceObjectId()
    {
        return myRelatedObjectId;
    }
    public string getRealName()
    {
        return myRealName;
    }
    public string getRelatedObjectRealName()
    {
        return myRelatedObjectRealName;
    }

    public string getRelatedObjectName()
    {
        return myRelatedObjectName;
    }
}
