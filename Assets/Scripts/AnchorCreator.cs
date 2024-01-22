using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/*
 * This is one of the most used classes in both the "AR rule editor" and "save object location" modalities.
 * Manages the anchors (points in the real world uses as reference 
 * for the visualizations) and the raycast (activated by the user tap)  
 * 
 * TODO: This class also manages the data exchange with the "ContextData" class. 
 * This is ***WRONG***: all these functionalities (e.g. getSingleobjectOrServiceCapability) have to be moved to a specific 
 * class, that also instantiates and contains the current context.
 */
public class AnchorCreator : MonoBehaviour
{
    /* 
     * Used to remove all achors and trackables in the "save object position" mode. 
     * In the "rule editor" mode it has no use. 
     * Called when pressing the green button with an arrow in the UI. 
     */

    // TODO test
    public void disableSessionOrigin()
    {
        SessionOrigin.gameObject.SetActive(false);
    }

    public void enableSessionOrigin()
    {
        SessionOrigin.gameObject.SetActive(true);
    }

    public void RemoveAllAnchors()
    {
        
        foreach (var anchor in anchorDic)
        {
            Destroy(anchor.Key.gameObject);
        }
        s_Hits.Clear();
        anchorDic.Clear();
        trackableList.Clear();
        trackableDic.Clear();
        raycastHitDic.Clear();
        labelsOfAnchors.Clear();
        retreived = false;
    }


    public void resetAllOjects()
    {
        //ScreenLog.Log("REMOVE ALL ANCHORS");
        ///deactivateCubePanels();
        deactivateActiveExclamationMarks();
        deactivateRecommendations();
        deactivateUsedInRule();
        reactivateExclamationMarks();
        // Reset the lists of instantiated objects 

        /*
        ScreenLog.Log("DESTROYED INSTANTIATED RECS");
        foreach (KeyValuePair <int, GameObject> element in instantiatedCubePanelsId){
            Destroy(element.Value);
        }
        ScreenLog.Log("DESTROYED INSTANTIATED CUBE PANELS");
        foreach (KeyValuePair <int, GameObject> element in instantiatedExclamationMarksId){
            Destroy(element.Value);
        }
        */
        //ScreenLog.Log("DESTROYED INSTANTIATED EXCL MARKS");
        
    }

    public void deactivateUsedInRule()
    {
        foreach (GameObject element in instantiatedUsedInRuleList)
        {
            Destroy(element);
        }
        instantiatedUsedInRuleList = new List<GameObject>();
    }
    public void deactivateRecommendations()
    {
        return; //TODO
        foreach (GameObject element in instantiatedRecsList){
            Destroy(element); //
        }
        instantiatedRecsList = new List<GameObject>();
    }

    public void placeAnchorsFromLoadedRule() //
    {
        m_MainMenuCanvas = GameObject.Find("MainMenuCanvas").GetComponent<MainMenuScript>();
        m_MainMenuCanvas.setViewAR(true);
        placeSavedAnchors();
        activateRuleElementsOfALoadedRule();
    }

    public void activateRuleElementsOfALoadedRule()
    {
        Rule myRule = tempRuleScript.getLoadedRule();
        //ScreenLog.Log("I will place anchors for the rule " + myRule.name);
        //Activate info panel for the related refId
        foreach (RuleElement myEvent in tempRuleScript.events)
        {
            // CAMBIATO SOLO PER EVENTS
            //ScreenLog.Log("EVENT! " + myEvent.refId.ToString()); //
            tempRuleScript.addRuleElementToAllRulesElementDict(myEvent);
           
            GameObject myReferenceElementPanel = instantiatedActiveExclamationMarksId[myEvent.refId];
            myReferenceElementPanel.transform.localScale = activeExclamationMarkOriginalSize;
            ActiveExclamationMarkScript elementPanelScript = (ActiveExclamationMarkScript)myReferenceElementPanel.GetComponent("ActiveExclamationMarkScript");
            elementPanelScript.startParticle();
            elementPanelScript.addToRuleElementId(myEvent.refId); //Same id setted in the exclamation mark and in the CubePanel
            GameObject myReferenceExclamationMark = instantiatedExclamationMarksId[myEvent.refId];
            myReferenceExclamationMark.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);  
            //ExclamationMarkScript exclamationMarkScript = (ExclamationMarkScript)myReferenceExclamationMark.GetComponent("ExclamationMarkScript");
            //exclamationMarkScript.setRuleElementId(myEvent.refId); //Same id setted in the exclamation mark and in the CubePanel WHY????????????????????
        }
        foreach (RuleElement myCondition in tempRuleScript.conditions)
        {
            //ScreenLog.Log("CONDITION! " + myCondition.refId.ToString());
            tempRuleScript.addRuleElementToAllRulesElementDict(myCondition);
            GameObject myReferenceElementPanel = instantiatedActiveExclamationMarksId[myCondition.refId];
            myReferenceElementPanel.transform.localScale = activeExclamationMarkOriginalSize;
            ActiveExclamationMarkScript elementPanelScript = (ActiveExclamationMarkScript)myReferenceElementPanel.GetComponent("ActiveExclamationMarkScript");
            elementPanelScript.addToRuleElementId(myCondition.refId); //Same id setted in the exclamation mark and in the CubePanel
            GameObject myReferenceExclamationMark = instantiatedExclamationMarksId[myCondition.refId];
            myReferenceExclamationMark.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f); 
            //ExclamationMarkScript exclamationMarkScript = (ExclamationMarkScript)myReferenceExclamationMark.GetComponent("ExclamationMarkScript");
            //exclamationMarkScript.setRuleElementId(myCondition.refId); //Same id setted in the exclamation mark and in the CubePanel
        }
        foreach (RuleElement myAction in tempRuleScript.actions) 
        {
            //ScreenLog.Log("ACTION! " + myAction.refId.ToString());
            tempRuleScript.addRuleElementToAllRulesElementDict(myAction);
            GameObject myReferenceElementPanel = instantiatedActiveExclamationMarksId[myAction.refId];
            myReferenceElementPanel.transform.localScale = activeExclamationMarkOriginalSize;
            ActiveExclamationMarkScript elementPanelScript = (ActiveExclamationMarkScript)myReferenceElementPanel.GetComponent("ActiveExclamationMarkScript");
            elementPanelScript.addToRuleElementId(myAction.refId); //Same id setted in the exclamation mark and in the CubePanel
            GameObject myReferenceExclamationMark = instantiatedExclamationMarksId[myAction.refId];
            myReferenceExclamationMark.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f); 
            //ExclamationMarkScript exclamationMarkScript = (ExclamationMarkScript)myReferenceExclamationMark.GetComponent("ExclamationMarkScript");
            //exclamationMarkScript.setRuleElementId(myAction.refId); //Same id setted in the exclamation mark and in the CubePanel
        }
        //ScreenLog.Log("ALL DONE");
    }

    /**
     * Initialize a dictionary (key:ObjectName, value:1)
     */
    public Dictionary<string, int> instantiateObjectsCountDictionary()
    {
        Dictionary<string, int> objectsCountDict = new Dictionary<string, int>();
        Dictionary<string, List<string>> objectsAndRules = contextDataScript.getUsedInRulesDict();
        foreach(KeyValuePair<string, List<string>> element in objectsAndRules)
        {
            objectsCountDict.Add(element.Key, 1);
        }
        //ScreenLog.Log("RETURN FROM INSTANTIATE...........!!!");
        //ScreenLog.Log(Newtonsoft.Json.JsonConvert.SerializeObject(objectsCountDict)); //Super useful console log "Trick"
        return objectsCountDict;
    }

    /**
     * If other anchors are never been placed, initialize them and resize to 0. 
     * Else, just place the "Used In Rule" Wiz (approach similar to activateRecInfoPanel)
     */
    public void placeSavedAnchorsExplore(List<Rule> myRules)
    {
        ScreenLog.Log("PLACE ANCHORS FOR EXPLORE RULE!!!!!!!!!!!!!!!!!!!!!!!!");
        m_MainMenuCanvas = GameObject.Find("MainMenuCanvas").GetComponent<MainMenuScript>();
        if (m_MainMenuCanvas.getViewAR() == false)
        {
            return;
        }
        if (instantiatedExclamationMarksId.Count == 0)
        {
            placeSavedAnchors();
        }
        
        Dictionary<string, int> rulePanelsAddedOverAnObject = instantiateObjectsCountDictionary();
        float distanceToAdd = 0.40f; //45 ok
        float initialPadding = 0.2f;
        foreach (Rule singleRule in myRules)
        {
             ScreenLog.Log("INSIDE RULE LOOP");
             string ruleNL = singleRule.nl;
             string ruleName = singleRule.name;
             foreach (KeyValuePair<int, RuleElement> ruleElement in singleRule.ruleElements)
            {
                ScreenLog.Log("INSIDE RULE ELEMENT LOOP");
                if (contextDataScript.isObjectOrServiceFromCapabilityFullName(ruleElement.Value.fullName) == "o")
                {
                    ScreenLog.Log("INSIDE IS OBJECT: " + ruleElement.Value.fullName);
                    int objRefId = ruleElement.Value.refId;
                    string objectName = contextDataScript.getObjectRealNameFromId(objRefId);
                    //ScreenLog.Log("GET ID: " + objRefId);
                    //GameObject myExclMark = instantiatedExclamationMarksId[objRefId]; 
                    //GameObject myPanel = instantiatedCubePanelsId[objRefId];  // Cube Panels does not exists anymore
                    GameObject myPanel = instantiatedExclamationMarksId[objRefId];  // Con kitchen-temperaturelevel arriva a before me e poi crahsa?
                    //ScreenLog.Log("GET PANEL, x = " +myPanel.transform.position[0]);
                    //ScreenLog.Log(myPanel.transform.position[0] +"");
                    float myY = myPanel.transform.position[1] + initialPadding + (rulePanelsAddedOverAnObject[objectName] * distanceToAdd);
                    //ScreenLog.Log("GET Y");
                    Vector3 myStartPosition = myPanel.transform.position;
                    myStartPosition[1] = myY;
                    //ScreenLog.Log("TRANSFORM Y");
                    //Initialize a viz over the obj 
                    GameObject rulePanel = Instantiate(_usedInRulePanel, myStartPosition, Quaternion.identity); // We do not care about the rotation of the object: it is managed by the RotateARCamera script
                    rulePanel.AddComponent<ARAnchor>(); // just adding the component already creates the anchor!
                    // Add the nl text to the UsedInRulePanel
                    UsedInRulePanelScript panelScript = (UsedInRulePanelScript)rulePanel.GetComponent("UsedInRulePanelScript");
                    ScreenLog.Log("I WILL UPDATE THE RULE PANEL WITH THESE DATA: ");
                    ScreenLog.Log(ruleName);
                    ScreenLog.Log(ruleNL);
                    panelScript.setInfo(ruleName, ruleNL, objRefId, myY);
                    instantiatedUsedInRuleList.Add(rulePanel); //TODO instantiatedUsedInRuleList!!!!!!!
                    //ScreenLog.Log("INSTANTIATED");
                    rulePanelsAddedOverAnObject[objectName]++;
                    //ScreenLog.Log(Newtonsoft.Json.JsonConvert.SerializeObject(rulePanelsAddedOverAnObject)); //Super useful console log "Trick"
                }
            }
        }
        //ScreenLog.Log("NOT CRASHED!!!");
        /*
        float myY = myPanel.transform.position[1] + initialPadding + (recsBeforeMeOnSameObject * distanceToAdd);
        Vector3 myStartPosition = myPanel.transform.position;
        myStartPosition[1] = myY;
        /*
        foreach(SingleEntry entry in response.data) // Loop the rules instead
        {
            string objectOrService = contextDataScript.isObjectOrServiceFromCapabilityFullName(entry.completeName);
            //ScreenLog.Log(objectOrService);
            if(objectOrService == "o")
            {
                int id = contextDataScript.getObjectOrServiceIdFromFullName(entry.completeName);
                anchorCreator.activateRecInfoPanel(id, entry, index);
            }
            index++;
            */

    }

    /*
     * Now, it loads the objects from a list instantiaded when the application start. 
     * In the future, it should load positions from a saved file (the commented part). 
     */
    public void placeSavedAnchors()
    {
        m_MainMenuCanvas = GameObject.Find("MainMenuCanvas").GetComponent<MainMenuScript>();
        if (m_MainMenuCanvas.getViewAR() == false)
        {
            return;
        }
        if (allAnchorsPlaced)
        {
            return;
        }
        foreach (var myObj in augmentedActiveExclamationMarkList)
        {
            GameObject instantiatedObject = Instantiate(_activeExclamationMark, myObj.distanceFromTrackable, Quaternion.identity);
            //ScreenLog.Log("instantiated an active excl mark ");
            instantiatedObject.AddComponent<ARAnchor>(); // just adding the component already creates the anchor!
            allAnchors.Add(instantiatedObject.GetComponent<ARAnchor>());
            instantiatedActiveExclamationMarksId.Add(myObj.id, instantiatedObject); //OK: ID done!!!
            //ScreenLog.Log("ADDED TO ANCHORS");
            isAnchoredActiveExclamationMarkDict[myObj.name] = true;
            //ScreenLog.Log("SETTING INFO:");
            //ScreenLog.Log(myObj.id + myObj.name + myObj.realName + myObj.referenceRoom);
            instantiatedObject.GetComponent<ActiveExclamationMarkScript>().setInfo(myObj.id, myObj.name, myObj.realName, myObj.referenceRoom);
            instantiatedObject.GetComponent<ActiveExclamationMarkScript>().setPosition(myObj.distanceFromTrackable);
            instantiatedObject.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f); 
            //instantiatedObject.SetActive(false); //Hide the objects at creation //
        }
        
        foreach (var myObj in augmentedExclamationMarkList)
        {
            ScreenLog.Log("INSTANTIATING A PLACEHOLDER");
            Vector3 coord = myObj.referenceObject.distanceFromTrackable;
            //coord += new Vector3(0, -2, 0);
            GameObject instantiatedObject  = Instantiate(_inactiveExclamationMark, coord, Quaternion.identity);
            //GameObject instantiatedObject  = Instantiate(_switch, coord, Quaternion.identity); //ARGH
            instantiatedObject.AddComponent<ARAnchor>(); // just adding the component already creates the anchor!  
            ScreenLog.Log("EXCLAMATION MARK NAME " + myObj.name);
            //instantiatedExclamationMarks.Add(myObj.name, instantiatedObject);
            instantiatedExclamationMarksId.Add(myObj.referenceObject.id, instantiatedObject);
            //instantiatedObject.GetComponent<ExclamationMarkScript>().move();
            instantiatedObject.GetComponent<ExclamationMarkScript>().setInfo(myObj.name, myObj.referenceObject.name, myObj.referenceObject.realName, myObj.referenceObject.id, myObj.realName, myObj.referenceRoom);
            instantiatedGameObjectSaver newExclamationMark = new instantiatedGameObjectSaver();
            newExclamationMark.name = myObj.name;
            newExclamationMark.gameObject = instantiatedObject;
            ScreenLog.Log("ADDING TO EXCLAMATIONMARKLIST" + myObj.name); //
            ScreenLog.Log("OBJECT PLACEHOLDER INSTANTIATED & ANCHORED"); 
        }
        allAnchorsPlaced = true;
        /*
        ScreenLog.Log(phoneARCamera.shiftX.ToString());
        ScreenLog.Log(phoneARCamera.transform.position[0].ToString());
        Vector3 translatedPosition = new Vector3((float)0.5, (float)0.01, (float)5.99);
            GameObject test = Instantiate(_cubePanel, translatedPosition, Quaternion.identity);
      
        Vector3 translatedPosition2 = new Vector3((float)2.15, (float)0.77, (float)0.11);
            GameObject test2 = Instantiate(_cubePanel, translatedPosition2, Quaternion.identity);
        */
        /* OK this laod and place
        saveSerial.LoadGame();
        if (saveSerial.retreivedPoseCheck)
        {
            ScreenLog.Log("PLACING A RETREIVED ANCHOR!");

            SerializablePose retreived = saveSerial.getPose();
            ScreenLog.Log("X of retreived pose (In AnchorCreator): " + retreived.position.x.ToString());
            Vector3 translatedPosition = new Vector3(retreived.position.x, retreived.position.y, retreived.position.z);
            GameObject test = Instantiate(_cubePanel, translatedPosition, Quaternion.identity);

        }
        else { 
            ScreenLog.Log("NO RETREIVED ANCHORS!");
        }
        */
    }

    /**
     * Read from context data and create the Exclamation Mark and Active Exclamation Mark Placeholders accordingly
     */
    public void anchorObjectsFromContextData()
    {

        foreach (KeyValuePair<int, ActivableObject> element in contextDataScript.activableObjects)
        {
            ScreenLog.Log(element.Value.realName);
            // Let's try
            ActiveExclamationMarkInfo activeExclamationMark = new ActiveExclamationMarkInfo();
            activeExclamationMark.id = element.Value.referenceId;
            activeExclamationMark.name = element.Value.name;
            activeExclamationMark.realName = element.Value.realName;
            activeExclamationMark.referenceRoom = element.Value.referenceRoom;
            activeExclamationMark.trackableName = "APOLLO"; // "APOLLO" is the reference name of the tracker (QR code). To be used in a real settings, application needs a QR code to be used as reference point to save the position of objects and place augmentation. At the moment is not needed.
            //activeExclamationMark.distanceFromTrackable = contextDataScript.getObjectPositionFromObjectId(cubePanelInfoForObject.id);
            activeExclamationMark.distanceFromTrackable = contextDataScript.getObjectPositionFromObjectId(activeExclamationMark.id);
            
            augmentedActiveExclamationMarkList.Add(activeExclamationMark);
            isAnchoredActiveExclamationMarkDict.Add(activeExclamationMark.name, false);
            
            AugmentedExclamationMarkInfo placeholderForObject = new AugmentedExclamationMarkInfo();
            placeholderForObject.name = element.Value.name + " placeholder";
            placeholderForObject.realName = element.Value.realName + "-placeholder";
            placeholderForObject.trackableName = "APOLLO";
            placeholderForObject.referenceRoom = element.Value.referenceRoom; 
            //placeholderForObject.referenceObject = cubePanelInfoForObject;
            placeholderForObject.referenceObject = activeExclamationMark;
            
            augmentedExclamationMarkList.Add(placeholderForObject);
            isAnchoredExclamationMarkDict.Add(placeholderForObject.name, false);
            ScreenLog.Log("ADDED " + placeholderForObject.realName + " EXCLAMATION MARK");
            
        }
        
        ScreenLog.Log("Everything added");
    }

    /*
     * initialize the AR objects. 
     * For the moment, the placement uses static coordinates 
     * defined with Vector3(x, y, z). 
     * In the future, the APP will read the saved coordinates and 
     * place objects accordingly. 
     */
    void Start()
    {
        cubePanelInfoOriginalSize = new Vector3(0.5f, 0.5f, 0.1f);
        exclamationMarkOriginalSize = new Vector3(1, 1, 1);
        activeExclamationMarkOriginalSize = new Vector3(1, 1, 1);


        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        currentCamera = Camera.current;
        currentCamera2 = Camera.main;
        
        saveSerial = new SaveSerial();
        contextDataScript = GetComponent<ContextData>();
        tempRuleScript = GetComponent<TempRule>();
        m_RaycastManager = GetComponent<ARRaycastManager>();
        m_AnchorManager = GetComponent<ARAnchorManager>();
        GameObject cameraImage = GameObject.Find("Camera Image");
        phoneARCamera = cameraImage.GetComponent<PhoneARCamera>();
        
        //_anchorObject = null;
        m_TrackedImageManager = GetComponent<ARTrackedImageManager>(); 
        aRPlaceTrackedImages = FindObjectOfType<ARPlaceTrackedImages>();
        
        anchorObjectsFromContextData();
        
        
        // Chiama la funzione TrovaOggettoVicino ogni 2 secondi // 
        //InvokeRepeating("findNearestObject", 0f, 3f);
        InvokeRepeating("findNearestPlane", 0f, 3f); //


        /*
        if (saveSerial.checkSavedListLenght()) //
        {
            poseList = saveSerial.getPoseList();
        }
        */
    }


    /*
     * Used in the "Save object location" mode to attach an AR viz over a selected object
     * here we'll place the plane anchoring code
     * Try first a plane (easier to retreive with a tap)
     */
    ARAnchor CreateAnchor(in ARRaycastHit hit)
    {
        
        // If we hit a plane, try to "attach" the anchor to the plane
        if (hit.trackable is ARPlane plane)
        {
           lastPlane = plane;
           //ScreenLog.Log($"DEBUG: Creating plane anchor. distance: {hit.distance}. session distance: {hit.sessionRelativeDistance} type: {hit.hitType}.");
           return m_AnchorManager.AttachAnchor(plane, hit.pose);
        }
        else
        {
            // create a regular anchor at the hit pose
            //ScreenLog.Log($"DEBUG: Creating regular anchor. distance: {hit.distance}. session distance: {hit.sessionRelativeDistance} type: {hit.hitType}.");
            return m_AnchorManager.AddAnchor(hit.pose);
        }
    }


    
    /*
     * Info log
     */
    public void hitsToAnchorLogger(ARRaycastHit hit, BoundingBox outline)
    {
        Debug.Log("Managing an hit");
        Debug.Log("Hit:" + hit.hitType);
        Debug.Log("outline label:" + outline.Label);
    }
    
    /*
     * 
     */
    public bool alreadyInAnchorList(BoundingBox outline)
    {
        return labelsOfAnchors.Contains(outline.Label);
    }
    
    /*
     * trabableDic is a list of <ARTrackable, anchor> tuples
     * check if the passed ARTrackable is already on this dict
     */
    public bool alreadyInTrackableDic(ARRaycastHit hit)
    {
        ARTrackable trackable = hit.trackable;
        return raycastHitDic.ContainsKey(hit);
    }
    
    // Search the trackable list 
    public bool trackableAlreadyUsed(ARTrackable trackable)
    {
        return trackableList.Contains(trackable);
    }

    // 
    public BoundingBox returnOutlineFromAnchor(ARAnchor anchor)
    {
        return anchorDic[anchor];
    }

    public ARAnchor returnAnchorFromTrackable(ARTrackable trackable)
    {
        return trackableDic[trackable];
    }
    

    /*
     * Used in the "Save object location" mode to attach an AR viz over a selected object
     * Creates a new anchor on the passed position
     * opens the "edit rule element" panel if the outline label is used and on that trackable there is an anchor
     * otherwise, opens the "new rule element" panel 
     * for now, it works with only 1 object per type
     */
    private bool Pos2AnchorNew(float x, float y, BoundingBox outline, ARRaycastHit hit)
    {
        // GameObject anchorObj = m_RaycastManager.raycastPrefab;
        // TextMesh anchorObj_mesh = anchorObj.GetComponent<TextMesh>();
        anchorObj_mesh.text = $"{outline.Label}: {(int)(outline.Confidence * 100)}%";

        //TextMesh anchorObj = GameObject.Find("New Text").GetComponent<TextMesh>();
        if (alreadyInAnchorList(outline) && trackableAlreadyUsed(hit.trackable)) 
        {
            //Load the edit rule element from the RuleElementCanvas game object 
            Debug.Log("anchor already exists: load the edit rule element canvas");
            if (!NewElementScript.getIsOpen() && !EditElementScript.getIsOpen())
            {
                EditElementScript.editElement(outline);
                EditElementScript.setIsOpen(true);
            }
            //phoneARCamera.localization = false;
            return false;
        }
        else
        {
            //if (!alreadyInAnchorList(outline))
            //{ //There can be duplicate objects: just not on the same trackable!!!
            // Create a new anchor
            var anchor = CreateAnchor(hit);
            if (anchor)
            {
                poseList.Add(hit.pose);
                //ScreenLog.Log("X of saved hit pose: " + hit.pose.position.x.ToString());
                ScreenLog.Log("LOCAL POSITION OF THE ANCHOR:");
                ScreenLog.Log(anchor.transform.localPosition[0].ToString() +", " +anchor.transform.localPosition[1].ToString() +", " + anchor.transform.localPosition[2].ToString());
                ScreenLog.Log("POSITION OF THE ANCHOR:");
                ScreenLog.Log(anchor.transform.position[0].ToString() +", " + anchor.transform.position[1].ToString() +", " + anchor.transform.position[2].ToString());
                ScreenLog.Log("LOCAL POSITION OF THE PLANE:");
                ScreenLog.Log(lastPlane.transform.localPosition[0].ToString() +", " + lastPlane.transform.localPosition[1].ToString() +", " + lastPlane.transform.localPosition[2].ToString());
                ScreenLog.Log("POSITION OF THE PLANE:");
                ScreenLog.Log(lastPlane.transform.position[0].ToString() +", " + lastPlane.transform.position[1].ToString() +", " + lastPlane.transform.position[2].ToString());
                saveSerial.addToSaveList(hit.pose);
                // Devo prendere il game object inserito nell'ambiente, da questo il transform, e da questo localPosition: è questo che devo salvare, non la pose
                //saveSerial.addToSaveList(1);

                labelsOfAnchors.Add(outline.Label);
                Debug.Log("anchor created: localization false");
                phoneARCamera.localization = false;
                Debug.Log($"DEBUG: creating anchor. {outline}");
                if (!NewElementScript.getIsOpen() && !EditElementScript.getIsOpen())
                {
                    NewElementScript.newElement(outline);
                    NewElementScript.setIsOpen(true);
                }
                // Remember the anchor so we can remove it later.
                ARTrackable hitted = hit.trackable;
                anchorDic.Add(anchor, outline);
                // Also remember the raycast hit with the associated anchor
                //raycastHitDic.Add(hit, anchor);
                // and the hitted trackable.
                trackableList.Add(hitted);
                trackableDic.Add(hitted, anchor);

                Debug.Log($"DEBUG: Current number of anchors {anchorDic.Count}.");
                return true;
            }
        }
        ScreenLog.Log("Anchor NOT created");
        return false;
        //}
    }
    
    /*
     * Collection of checks to decide if a touch is not useful
     */ 
    public bool checkReturnConditions()
    {
        //Return if there is no touch input
        if (Input.touchCount == 0)
        { 
            return true;
        }
        
        //Return if the touch is over an UI element 
        foreach (Touch touchLoop in Input.touches)
        {
            int id = touchLoop.fingerId;
            if (EventSystem.current.IsPointerOverGameObject(id))
            {
                return true;
            }
        }
        
        //Return if a game object (e.g. UI element) is currenctly selected 
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            return true;
        }
        return false;
    } 



    //public void resetObjectsPositionWrtApollo(Vector3 apolloPosition, Quaternion apolloRotation)
    public void resetObjectsPositionWrtApollo(Transform trackedImageTransform)
    {
      // Foreach exclamation mark 
        // Get the associated activableObject
        // set my position
        Vector3 apolloPosition = trackedImageTransform.localPosition;
        Quaternion apolloRotation = trackedImageTransform.rotation;
        
        apolloPositionForPhone = apolloPosition;

        currentCamera2 = Camera.current;
        Vector3 cameraRotation = currentCamera2.transform.rotation.eulerAngles;
        float toRadCameraRot =  cameraRotation[1] * Mathf.PI / 180;
        float toRadApolloRot =  apolloRotation[1] * Mathf.PI / 180;
        float distance = toRadApolloRot - toRadCameraRot;
        // NOO!! We need instantiatedActive ... and the other DICT
        foreach (KeyValuePair<int, GameObject> myAugmentedGameObject in instantiatedExclamationMarksId)
        {
                //ScreenLog.Log("PLACER " + myAugmentedGameObject.Key);
                //ARAnchor myAnchor = myAugmentedGameObject.Value.GetComponent<ARAnchor>();
                 Destroy(myAugmentedGameObject.Value.GetComponent<ARAnchor>()); 
                
                //Destroy(myAnchor); // Docs says that it is not needed to destroy/recreate the anchor: see here https://learn.unity.com/tutorial/placing-and-manipulating-objects-in-ar#605103a5edbc2a6c32bf5663
                // In reality you need to, otherwise the anchor will come back to the old position after some time!

                
                //Vector3 myPositionWrtApollo = contextDataScript.getObjectPositionWrtApolloFromObjectId(myObj.referenceObject.id);
                Vector3 myPositionWrtApollo = contextDataScript.getObjectPositionWrtApolloFromObjectId(myAugmentedGameObject.Key);
                Vector3 translatedPosition = new Vector3();
                translatedPosition[0] = apolloPosition[0] + (myPositionWrtApollo[0] * Mathf.Cos(distance) - myPositionWrtApollo[2] * Mathf.Sin(distance));
                translatedPosition[1] = apolloPosition[1] + myPositionWrtApollo[1];
                translatedPosition[2] = apolloPosition[2] + (myPositionWrtApollo[2] * Mathf.Cos(distance) + myPositionWrtApollo[0] * Mathf.Sin(distance));
                
                // TEST!!!
                //trackedImageTransform.position = translatedPosition; // Pass all the transform to the obj??
                myAugmentedGameObject.Value.transform.position = translatedPosition; 
                myAugmentedGameObject.Value.transform.localPosition = translatedPosition; 
                myAugmentedGameObject.Value.AddComponent<ARAnchor>(); // just adding the component already creates the anchor!  
                
            //instantiatedExclamationMarksId[myAugmentedGameObject.Key] = myAugmentedGameObject.Value;
                //ScreenLog.Log("PLACER OK ");

        }
        /*
        foreach (KeyValuePair<int, GameObject> myAugmentedGameObject in instantiatedActiveIconId)
        {
                //ScreenLog.Log("ACTIVE STARTING ICON ID: " + myAugmentedGameObject.Key);
                Destroy(myAugmentedGameObject.Value.GetComponent<ARAnchor>()); 
                Vector3 myPositionWrtApollo = contextDataScript.getObjectPositionWrtApolloFromObjectId(myAugmentedGameObject.Key);
                //ScreenLog.Log("ACTIVE MY COORD: " + myPositionWrtApollo); //
                Vector3 translatedPosition = new Vector3();
                translatedPosition[0] = apolloPosition[0] + (myPositionWrtApollo[0] * Mathf.Cos(distance) - myPositionWrtApollo[2] * Mathf.Sin(distance));
                translatedPosition[1] = apolloPosition[1] + myPositionWrtApollo[1];
                translatedPosition[2] = apolloPosition[2] + (myPositionWrtApollo[2] * Mathf.Cos(distance) + myPositionWrtApollo[0] * Mathf.Sin(distance));
                myAugmentedGameObject.Value.transform.position = translatedPosition; 
                myAugmentedGameObject.Value.transform.localPosition = translatedPosition; 
                myAugmentedGameObject.Value.AddComponent<ARAnchor>();
        }
        */

        /*
        foreach (KeyValuePair<int, GameObject> myAugmentedGameObject in instantiatedIconId)
        {
                //ScreenLog.Log("STARTING ICON ID: " + myAugmentedGameObject.Key);
                Destroy(myAugmentedGameObject.Value.GetComponent<ARAnchor>()); 
                Vector3 myPositionWrtApollo = contextDataScript.getObjectPositionWrtApolloFromObjectId(myAugmentedGameObject.Key);
                //ScreenLog.Log("MY COORD: " + myPositionWrtApollo); //
                Vector3 translatedPosition = new Vector3();
                translatedPosition[0] = apolloPosition[0] + (myPositionWrtApollo[0] * Mathf.Cos(distance) - myPositionWrtApollo[2] * Mathf.Sin(distance));
                translatedPosition[1] = apolloPosition[1] + myPositionWrtApollo[1];
                translatedPosition[2] = apolloPosition[2] + (myPositionWrtApollo[2] * Mathf.Cos(distance) + myPositionWrtApollo[0] * Mathf.Sin(distance));
                myAugmentedGameObject.Value.transform.position = translatedPosition; 
                myAugmentedGameObject.Value.transform.localPosition = translatedPosition; 
                myAugmentedGameObject.Value.AddComponent<ARAnchor>();
                //ScreenLog.Log("ICON ID OK ");
        }
        */
        foreach (KeyValuePair<int, GameObject> myAugmentedGameObject in instantiatedActiveExclamationMarksId)
        {
                //ScreenLog.Log("PLACER ACTIVE" + myAugmentedGameObject.Key);
                //ARAnchor myAnchor = myAugmentedGameObject.Value.GetComponent<ARAnchor>();
                 Destroy(myAugmentedGameObject.Value.GetComponent<ARAnchor>()); 
                //Destroy(myAnchor); // Docs says that yi is not needed to destroy/recreate the anchor: see here https://learn.unity.com/tutorial/placing-and-manipulating-objects-in-ar#605103a5edbc2a6c32bf5663
                // In reality you need to, otherwise the anchor will come back to the old position after some time!

                
                //Vector3 myPositionWrtApollo = contextDataScript.getObjectPositionWrtApolloFromObjectId(myObj.referenceObject.id);
                Vector3 myPositionWrtApollo = contextDataScript.getObjectPositionWrtApolloFromObjectId(myAugmentedGameObject.Key);
                Vector3 translatedPosition = new Vector3();
                translatedPosition[0] = apolloPosition[0] + (myPositionWrtApollo[0] * Mathf.Cos(distance) - myPositionWrtApollo[2] * Mathf.Sin(distance));
                translatedPosition[1] = apolloPosition[1] + myPositionWrtApollo[1];
                translatedPosition[2] = apolloPosition[2] + (myPositionWrtApollo[2] * Mathf.Cos(distance) + myPositionWrtApollo[0] * Mathf.Sin(distance));
                
                // TEST!!!
                //trackedImageTransform.position = translatedPosition; // Pass all the transform to the obj??
                myAugmentedGameObject.Value.transform.position = translatedPosition; 
                myAugmentedGameObject.Value.transform.localPosition = translatedPosition; 
                myAugmentedGameObject.Value.AddComponent<ARAnchor>(); // just adding the component already creates the anchor!  
                
            //instantiatedExclamationMarksId[myAugmentedGameObject.Key] = myAugmentedGameObject.Value;
                //ScreenLog.Log("PLACER ACTIVE OK ");

        }
        // Foreach instantiatedUsedInRuleList
        foreach (GameObject myObj in instantiatedUsedInRuleList)
        { // AAAAAAAAA
            //ScreenLog.Log("PLACER USED IN RULE ");
            UsedInRulePanelScript panelScript = (UsedInRulePanelScript)myObj.GetComponent("UsedInRulePanelScript");
            //ScreenLog.Log("GET THE PANEL");
            int myObjReferenceId = panelScript.getObjReferenceId();
            float myOldYValue = panelScript.getMyY();
            //ScreenLog.Log("GET Y: " + myOldYValue);
            Vector3 myPositionWrtApollo = contextDataScript.getObjectPositionWrtApolloFromObjectId(myObjReferenceId); //
            //ScreenLog.Log("GET EVERYTHING: DESTROYNG ANCHOR");
            Destroy(myObj.GetComponent<ARAnchor>());  // Now it seems to work, but I can't understand why it crashed when I tried to get the Y from the anchor transform in explore mode (first time it works, then crashes).
            //ScreenLog.Log("DSTROYED OLD ANCHOR");   // However, now it just store the Y value (calculated when explore mode is called first time) in the PanelScipt and always reuse it (we don't care about Y axis).
            Vector3 translatedPosition = new Vector3();
            translatedPosition[0] = apolloPosition[0] + (myPositionWrtApollo[0] * Mathf.Cos(distance) - myPositionWrtApollo[2] * Mathf.Sin(distance));
            translatedPosition[1] = myOldYValue;
            translatedPosition[2] = apolloPosition[2] + (myPositionWrtApollo[2] * Mathf.Cos(distance) + myPositionWrtApollo[0] * Mathf.Sin(distance));
            //ScreenLog.Log("CREATED TRANSLATED POSITION VECTOR");
            myObj.transform.position = translatedPosition; 
            myObj.transform.localPosition = translatedPosition; 
            myObj.AddComponent<ARAnchor>(); // just adding the component already creates the anchor!  
            //ScreenLog.Log("ADDED NEW ANCHOR");
        }
       
        // Same as used In Rule panels
        foreach (GameObject myObj in instantiatedRecsList)
        { // AAAAAAAAA
            //ScreenLog.Log("PLACER USED IN RULE ");
            RecommendedElementPanelScript panelScript = (RecommendedElementPanelScript)myObj.GetComponent("RecommendedElementPanelScript");
            //ScreenLog.Log("GET THE PANEL");
            int myObjReferenceId = panelScript.getObjectReferenceId();
            float myOldYValue = panelScript.getMyY();
            //ScreenLog.Log("GET Y: " + myOldYValue);
            Vector3 myPositionWrtApollo = contextDataScript.getObjectPositionWrtApolloFromObjectId(myObjReferenceId); //
            //ScreenLog.Log("GET EVERYTHING: DESTROYNG ANCHOR");
            Destroy(myObj.GetComponent<ARAnchor>());  // Now it seems to work, but I can't understand why it crashed when I tried to get the Y from the anchor transform in explore mode (first time it works, then crashes).
            //ScreenLog.Log("DSTROYED OLD ANCHOR");   // However, now it just store the Y value (calculated when explore mode is called first time) in the PanelScipt and always reuse it (we don't care about Y axis).
            Vector3 translatedPosition = new Vector3();
            translatedPosition[0] = apolloPosition[0] + (myPositionWrtApollo[0] * Mathf.Cos(distance) - myPositionWrtApollo[2] * Mathf.Sin(distance));
            translatedPosition[1] = myOldYValue;
            translatedPosition[2] = apolloPosition[2] + (myPositionWrtApollo[2] * Mathf.Cos(distance) + myPositionWrtApollo[0] * Mathf.Sin(distance));
            //ScreenLog.Log("CREATED TRANSLATED POSITION VECTOR");
            myObj.transform.position = translatedPosition; 
            myObj.transform.localPosition = translatedPosition; 
            myObj.AddComponent<ARAnchor>(); // just adding the component already creates the anchor!  
            //ScreenLog.Log("ADDED NEW ANCHOR");
        }
    }


    public Vector3 getCameraPosition()
    {
        return Camera.current.transform.position;
    }

    public void callRecommender(int objId)
    {
        if (objId != -1)
        {
            //ScreenLog.Log("CALLING RECOMMENDER SYSTEM!!!"); //
            //ExclamationMarkScript myScript = (ExclamationMarkScript)instantiatedExclamationMarksId[objId].GetComponent("ExclamationMarkScript");
            //ScreenLog.Log(myScript.myRelatedObjectName);
        }
    }

    public void findNearestPlane()
    {
        // Esegui un raycast per individuare un piano
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        if (m_RaycastManager.Raycast(new Vector3(Screen.width / 2, Screen.height / 2, 0), hits, trackableTypes))
        {
            foreach (ARRaycastHit hit in hits)
            {
                if (hit.trackable is ARPlane hitPlane) //
                {
                    foreach (KeyValuePair<int, GameObject> myAugmentedGameObject in instantiatedExclamationMarksId)
                    {
                        float distanzaTraAncoraEPiano = Vector3.Distance(myAugmentedGameObject.Value.transform.position, hit.pose.position);
                        //ScreenLog.Log(distanzaTraAncoraEPiano.ToString());
                        if (distanzaTraAncoraEPiano < 0.6f)
                        {
                            Destroy(myAugmentedGameObject.Value.GetComponent<ARAnchor>()); //
                            // "Attacca" l'ancora al piano
                            //public ARAnchorManager m_AnchorManager;
                            ARAnchor nuovaAncora = m_AnchorManager.AttachAnchor(hitPlane, hit.pose);
                            /*
                            myAugmentedGameObject.Value.transform.position = hit.pose.position; //
                            myAugmentedGameObject.Value.transform.localPosition = hit.pose.position; 
                            myAugmentedGameObject.Value.AddComponent<ARAnchor>(); // just adding the component already creates the anchor!  
                            */
                            ScreenLog.Log("Ancora collegata al piano!");
                        }
                    }
                }
            }
        }
    }

    public void findNearestObject() 
    {
        currentCamera2 = Camera.current;
        Vector3 myPosition = currentCamera2.transform.position; 
        int nearestGameObjectId = 9999;

        // Inizializza la distanza minima come un valore elevato
        float distanzaMinima = 300f;
        // Ottieni la posizione dello smartphone rispetto ad Apollo
        Vector3 posizioneRelativa = myPosition - apolloPositionForPhone;

        foreach (KeyValuePair<int, GameObject> myAugmentedGameObject in instantiatedExclamationMarksId)
        {
            Vector3 myPositionWrtApollo = contextDataScript.getObjectPositionWrtApolloFromObjectId(myAugmentedGameObject.Key); //
            // Calcola la distanza tra la posizione dello smartphone rispetto ad Apollo e la visualizzazione corrente
            float distanza = Vector3.Distance(posizioneRelativa, myPositionWrtApollo); //

            // Se la distanza è minore della distanza minima registrata, aggiorna l'oggetto più vicino
            if (distanza < distanzaMinima)
            {
                distanzaMinima = distanza;
                nearestGameObjectId = myAugmentedGameObject.Key; //
            }
        }
        ExclamationMarkScript myScript = (ExclamationMarkScript)instantiatedExclamationMarksId[nearestGameObjectId].GetComponent("ExclamationMarkScript");
        //ScreenLog.Log("OGGETTO PIU VICINO:"); //
        //ScreenLog.Log(myScript.myRelatedObjectName + " " +distanzaMinima.ToString());
       
        if (nearestObject != nearestGameObjectId)
        {
            nearestObject = nearestGameObjectId;
            callRecommender(nearestGameObjectId);
        }
    }

    /*
     * Manages the touch inputs. 
     */
    void Update()
    {

        if (Input.touchCount < 1 || (Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            return; 
        }

        if (UIOpen)
        {
            return;
        }

        //Ray ray = currentCamera.ScreenPointToRay(Input.GetTouch(0).position);
        Ray ray = Camera.current.ScreenPointToRay(Input.GetTouch(0).position);
        RaycastHit myHit;
        //ScreenLog.Log("Raycasting");
        if (Physics.Raycast(ray, out myHit))
        {
            if(myHit.collider.gameObject.tag=="ExitIconCanvas"            ||
               myHit.collider.gameObject.tag=="RuleMapCanvas"             ||
               myHit.collider.gameObject.tag=="MapIconCanvas"             ||
               myHit.collider.gameObject.tag=="SaveRuleIconCanvas"        ||
               myHit.collider.gameObject.tag=="RecommendationsIconCanvas" ||
               myHit.collider.gameObject.tag=="ResetIconCanvas")
            {
                //ScreenLog.Log("CLICK ON A ICON: EXITING TOUCH RAYTRACER!!");
                return; //TODO TEST!!!!
            }


            else if(myHit.collider.gameObject.tag=="ActiveExclamationMarkSphere")
            {
                ScreenLog.Log("CLICK ON A EXCLAMATION MARK SPHERE!!!!");

                // Get the rule element id
                GameObject hittedCubePanelButton = myHit.collider.gameObject;
                GameObject parentCubePanelObj = hittedCubePanelButton.transform.parent.gameObject;
                ActiveExclamationMarkScript myScript = (ActiveExclamationMarkScript)parentCubePanelObj.GetComponent("ActiveExclamationMarkScript");
                List<int> myRuleElementsIds = myScript.getRuleElementsIds(); // # fin qua ID preso è giusto
                string myName = myScript.getRealName();
                int myId = myScript.getReferenceObjectId(); //
                //ScreenLog.Log(myName+ myRelatedObjectName);getObjectOrServicesCapabilitiesFromId(
                int myCapabilitiesCount = contextDataScript.getObjectOrServicesCapabilitiesFromId(myId).Count;
                ScreenLog.Log("CAPABILITY COUNT: " + myCapabilitiesCount);
                if(myCapabilitiesCount > 1)
                {
                    EditOrNewElementScript editOrNewElementScript = GameObject.Find("EditOrNewElementCanvas").GetComponent<EditOrNewElementScript>();
                    editOrNewElementScript.enable(myRuleElementsIds, myName, myId);
                    ScreenLog.Log("EDIT OR NEW ENABLED!!!");
                }
                else // No need to load the decision panel, just load the edit
                {
                    int myRuleElementId = myRuleElementsIds[0]; // We have only one element
                    // Retreive the rule element values from TempRule
                    RuleElement myData = tempRuleScript.getRuleElementFromId(myRuleElementId); // # Ok
                    // Load the Rule Element Panel with the current values
                    RuleElementScript ruleElementScript = GameObject.Find("RuleElementCanvas").GetComponent<RuleElementScript>();
                    ruleElementScript.editFromAlreadyOpened = true; //
                    ruleElementScript.showEdit(myData); //
                    ScreenLog.Log("SHOWEDIT ENABLED!!!");
                }
            }
            

            
            // the exclamation mark referes to an object //
            else if(myHit.collider.gameObject.tag=="ExclamationMarkSphere")
            {
                GameObject hittedExclamationMarkPart = myHit.collider.gameObject;
                GameObject parentExclamationMarkObj = hittedExclamationMarkPart.transform.parent.gameObject;
                ExclamationMarkScript myScript = (ExclamationMarkScript)parentExclamationMarkObj.GetComponent("ExclamationMarkScript");
                myScript.stop(); // We have to stop the animation before do something to the transform!!
                int myId = myScript.getReferenceObjectId();
                ScreenLog.Log(myId.ToString());
                //ScreenLog.Log(myRelatedObjectName+ " " + myName); 
                //instantiatedExclamationMarks[myName].transform.localScale = new Vector3(0.001f, 0.001f, 0.001f); 
                instantiatedExclamationMarksId[myId].transform.localScale = new Vector3(0.001f, 0.001f, 0.001f); 
                //setLastActiveExclamationMark(parentExclamationMarkObj);
                configureRuleElementExclamationMark(parentExclamationMarkObj);
            }
            else if(myHit.collider.gameObject.tag=="ExclamationMarkCapsule")
            {
                GameObject hittedExclamationMarkPart = myHit.collider.gameObject;
                GameObject parentExclamationMarkObj = hittedExclamationMarkPart.transform.parent.gameObject;
                ExclamationMarkScript myScript = (ExclamationMarkScript)parentExclamationMarkObj.GetComponent("ExclamationMarkScript");
                myScript.stop(); // We have to stop the animation before do something to the transform!!
                int myId = myScript.getReferenceObjectId();
                ScreenLog.Log(myId.ToString());
                //ScreenLog.Log(myRelatedObjectName+ " " + myName); 
                //instantiatedExclamationMarks[myName].transform.localScale = new Vector3(0.001f, 0.001f, 0.001f); 
                instantiatedExclamationMarksId[myId].transform.localScale = new Vector3(0.001f, 0.001f, 0.001f); 
                //setLastActiveExclamationMark(parentExclamationMarkObj);
                configureRuleElementExclamationMark(parentExclamationMarkObj);
                return;
            }
        }
        return;
    }


    
    /*
     * Get the last active exclamation mark, retreive related objet, 
     * load the related info panel
     * The last created rule elements can be retreived from allRuleElementsList
     *  TODO: Just the ID is needed! remove all the other arguments
     */
    public void activateActiveExclamationMark(string ECA, string referenceObject, string capability, string value, string nextOperator, int id)
    {
        RuleElement lastModifiedTempRule = tempRuleScript.getRuleElementFromId(id); // TODO CHECK: The problem is that this id is not always correct
        int lastModifiedReferenceToObject = lastModifiedTempRule.refId; // this seems ok!
        ExclamationMarkScript myScript = (ExclamationMarkScript)instantiatedExclamationMarksId[lastModifiedReferenceToObject].GetComponent("ExclamationMarkScript");
        int myId = myScript.getReferenceObjectId();
        instantiatedActiveExclamationMarksId[myId].transform.localScale = activeExclamationMarkOriginalSize; //
        ActiveExclamationMarkScript activeExclamationMarkScript = (ActiveExclamationMarkScript)instantiatedActiveExclamationMarksId[myId].GetComponent("ActiveExclamationMarkScript"); //
        activeExclamationMarkScript.startParticle();
        activeExclamationMarkScript.addToRuleElementId(id);
        //ScreenLog.Log(myId.ToString());
        instantiatedActiveExclamationMarksId[myId].transform.localScale = activeExclamationMarkOriginalSize; 
        ScreenLog.Log("10 END");
    }

    /**
     * restore all exclamation marks original size
     */
    public void reactivateExclamationMarks() {
        ScreenLog.Log("REACTIVATING EXCL MARK");
        foreach (KeyValuePair<int, GameObject> myObj in instantiatedExclamationMarksId)
        {
            myObj.Value.transform.localScale = exclamationMarkOriginalSize; 
        }
        ScreenLog.Log("EXCL MARK REACTIVATED");
    } 
    
    /**
     * restore all Active Exclamation Mark original size
     */
    public void deactivateActiveExclamationMarks() { //
        foreach (KeyValuePair<int, GameObject> myObj in instantiatedActiveExclamationMarksId)
        {
            myObj.Value.transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f); 
            ActiveExclamationMarkScript activeExclamationMarkScript = (ActiveExclamationMarkScript)myObj.Value.GetComponent("ActiveExclamationMarkScript");
            activeExclamationMarkScript.stopParticle();
        }
        /*
        foreach (KeyValuePair<int, GameObject> myObj in instantiatedActiveIconId)
        {
            myObj.Value.transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f); 
            //ActiveExclamationMarkScript activeExclamationMarkScript = (ActiveExclamationMarkScript)myObj.Value.GetComponent("ActiveExclamationMarkScript");
            //activeExclamationMarkScript.stopParticle();
        }
        */
        ScreenLog.Log("deactivateActiveExclamationMarks Ok!");
    } 
    
    /**
     * restore all Exclamation Mark original size
     */
    public void deactivateExclamationMarks() { //
        foreach (KeyValuePair<int, GameObject> myObj in instantiatedExclamationMarksId)
        {
            myObj.Value.transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f); 
        }
        /*
        foreach (KeyValuePair<int, GameObject> myObj in instantiatedIconId)
        {
            myObj.Value.transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f); 
            //ActiveExclamationMarkScript activeExclamationMarkScript = (ActiveExclamationMarkScript)myObj.Value.GetComponent("ActiveExclamationMarkScript");
            //activeExclamationMarkScript.stopParticle();
        }
        */
        ScreenLog.Log("deactivateExclamationMarks Ok!");
    }
    
    

    /*
     */
    public void deactivateExclamationMark(int objId)
    {
        ScreenLog.Log("DEACTIVATING EXCL MARK!!!");
        //ScreenLog.Log("DEACTIVATING THIS EXCLAMATION MARK: " + objId);
        foreach (KeyValuePair<int, GameObject> myObj in instantiatedExclamationMarksId)
        {
            //ScreenLog.Log("KEY: " + myObj.Key); //
            if(objId == myObj.Key)
            {
                myObj.Value.transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f); 
            }
        }
    }
    public void deactivateActiveExclamationMark(int objId)
    {
        //ScreenLog.Log("DEACTIVATING THIS ACTIVE EXCLAMATION MARK: " + objId);
        ActiveExclamationMarkScript activeExclamationMarkScript = instantiatedActiveExclamationMarksId[objId].GetComponent<ActiveExclamationMarkScript>();
        activeExclamationMarkScript.stopParticle();
        foreach (KeyValuePair<int, GameObject> myObj in instantiatedActiveExclamationMarksId)
        {
            //ScreenLog.Log("KEY: " + myObj.Key);
            if(objId == myObj.Key)
            {
                myObj.Value.transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f); 
            }
        }
    }

    /**
     * ID is the identificator of the anchor object, not the ID of the rule element
     */
    public void reactivateSingleExclamationMark(int id)
    {
        ScreenLog.Log("REACTIVATING SINGLE EXCL MARK!" + id);  //
        instantiatedExclamationMarksId[id].transform.localScale = exclamationMarkOriginalSize; 
        instantiatedCubePanelsId[id].transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
        ActiveExclamationMarkScript activeExclamationMarkScript = instantiatedActiveExclamationMarksId[id].GetComponent<ActiveExclamationMarkScript>();
        activeExclamationMarkScript.stopParticle();
        instantiatedActiveExclamationMarksId[id].transform.localScale = new Vector3(0.001f, 0.001f, 0.001f); 
        return;
    }

    /**
     * Destroy the passed list of gameobjects
     */ 
    public void cleanGameObjectList(List<GameObject> gameObjects)
    {
        foreach (var entry in gameObjects)
        {
            Destroy(entry);
        }
    }
    
    /**
     *  Deactivate the AR view and loads the Rule Element Panel
     */
    public void loadRuleElementPanel(int referenceObjId, string myName)
    {
        //ScreenLog.Log("OBJ REFERENCE ID: " + referenceObjId);
        RuleElementScript ruleElementScript = GameObject.Find("RuleElementCanvas").GetComponent<RuleElementScript>();
        ruleElementScript.showNew(referenceObjId, myName);
    }
    
    
    /*
     * We don't know which type of Game object has been clicked
     * This function is only called when inactive game objects are tapped
     * Ok, this works!
     */
    public void configureRuleElement(GameObject myGameObject)
    {
        myGameObject.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
        string myTag = myGameObject.tag;
        InactiveIconScript myScript = (InactiveIconScript)myGameObject.GetComponent("InactiveIconScript");
        string myName = myScript.getRealName();
        string myRelatedObjectName = myScript.getRelatedObjectRealName(); //
        //ScreenLog.Log(myName+ myRelatedObjectName);
        int myId = myScript.getReferenceObjectId();
        loadRuleElementPanel(myId, myRelatedObjectName); //
        ScreenLog.Log("End configureRuleElement");
    }


    /*
     * Game object is an exclamation mark
     */
    public void configureRuleElementExclamationMark(GameObject myGameObject)
    {
        myGameObject.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f); 
        ExclamationMarkScript myScript = (ExclamationMarkScript)myGameObject.GetComponent("ExclamationMarkScript");
        string myName = myScript.getRealName();
        string myRelatedObjectName = myScript.getRelatedObjectRealName(); //
        //ScreenLog.Log(myName+ myRelatedObjectName);
        int myId = myScript.getReferenceObjectId();
        loadRuleElementPanel(myId, myRelatedObjectName); //
    }

    /**
      * get id of object from fullname 
      * If they are the same, check if index < forLoopPosition
      * /If yes, increase beforeMe count
      */
    public int getRecsBeforeMeOnSameObject(int objRefId, SingleEntry entry, int forLoopPosition)
    {
        List<SingleEntry> allRecs = tempRuleScript.getRecommendations();
        int beforeMe = 1; 
        int index = 0;
        foreach (SingleEntry myElement in allRecs)
        {
            int objOrServiceId = contextDataScript.getObjectOrServiceIdFromFullName(myElement.completeName);
            if(objOrServiceId == objRefId)
            {
                //ScreenLog.Log("FOUND ANOTHER RECOMMENDATION RELATED TO MY OBJECT: " + objOrServiceId);
                if(index < forLoopPosition)
                {
                    beforeMe++;  //
                }
            }
            index++;
        }
        //ScreenLog.Log("END GET BEFORE ME");
        return beforeMe;
    }

    /**
     * Places the recommendations over the related object:
     * Get the number of previous recs already placed on my object
     * Obtain how much to translate on the Y axis 
     * Place rec
     */ 
    public void activateRecInfoPanel(int objRefId, SingleEntry entry, int forLoopPosition)
    {
        int recsBeforeMeOnSameObject = getRecsBeforeMeOnSameObject(objRefId, entry, forLoopPosition);
        //ScreenLog.Log("BEFORE ME!!! " + recsBeforeMeOnSameObject);
        // Y pos = recsBefore... * n; 
        GameObject myExclMark = instantiatedExclamationMarksId[objRefId]; 
        //GameObject myPanel = instantiatedCubePanelsId[objRefId];  // Cube Panels does not exists anymore
        GameObject myPanel = instantiatedActiveExclamationMarksId[objRefId];  // Con kitchen-temperaturelevel arriva a before me e poi crahsa?
        //ScreenLog.Log(myPanel.transform.position[0] +"");
        float distanceToAdd = 0.35f;
        float initialPadding = 0.2f;
        float myY = myPanel.transform.position[1] + initialPadding + (recsBeforeMeOnSameObject * distanceToAdd);
        Vector3 myStartPosition = myPanel.transform.position;
        myStartPosition[1] = myY;
        //ScreenLog.Log("x: "+ myStartPosition[0]);
        //ScreenLog.Log("y: "+ myStartPosition[1]);
        //ScreenLog.Log("z: "+ myStartPosition[2]);
        GameObject instantiatedRec = Instantiate(_recommendationPanel, myStartPosition, Quaternion.identity); // We do not care about the rotation of the object: it is managed by the RotateARCamera script
        instantiatedRec.AddComponent<ARAnchor>(); // just adding the component already creates the anchor!
        RecommendedElementPanelScript recScript = (RecommendedElementPanelScript)instantiatedRec.GetComponent("RecommendedElementPanelScript");
        recScript.setInfo(entry, objRefId, myY); // TODO TEST!
        instantiatedRecsList.Add(instantiatedRec);
        //ScreenLog.Log("END ACTIVATE REC INFO PANEL!!!!!!!!!!!!!");  //
    }


    public void setLastImageTargetName(string name)
    {
        lastImageTargetName=name;
    }
    public string getLastImageTargetName()
    {
        return lastImageTargetName;
    }
    public void setLastImageTargetTransform(Transform transform)
    {
        lastImageTargetTransform=transform;
    }
    public Transform getLastImageTargetTransform()
    {
        return lastImageTargetTransform;
    }

    public TempRule passTempRuleObject()
    {
        return tempRuleScript;
    }

    public ARAnchorManager passARAnchorManager()
    {
        return m_AnchorManager;
    }

    public bool editMode = false; //TODO Remove????
    //private GameObject lastActiveCubePanel;
    //private GameObject lastActiveExclamationMark;
    public bool UIOpen = false;

    public bool allAnchorsPlaced = false; // Flag that indicates that all flags are correctly placed: used when the editor is exited and reentered.

    public ContextData contextDataScript;
    public TempRule tempRuleScript;
    
    public MainMenuScript m_MainMenuCanvas;
    //public ExclamationMarkScript exclamationMarkScript
    //public Dictionary<string, GameObject> instantiatedCubePanels = new Dictionary<string, GameObject>();
    //public Dictionary<string, GameObject> instantiatedExclamationMarks = new Dictionary<string, GameObject>();
    public List<ARAnchor> allAnchors = new List<ARAnchor>();

    public Dictionary<int, GameObject> instantiatedCubePanelsId = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> instantiatedExclamationMarksId = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> instantiatedActiveExclamationMarksId = new Dictionary<int, GameObject>();
    //public Dictionary<int, GameObject> instantiatedIconId = new Dictionary<int, GameObject>();  CHECK if ok to remove
    //public Dictionary<int, GameObject> instantiatedActiveIconId = new Dictionary<int, GameObject>(); CHECK if ok to remove

    public List<GameObject> instantiatedUsedInRuleList = new List<GameObject>();
    public List<GameObject> instantiatedRecsList = new List<GameObject>();
    //public List<AugmentedActiveIcon> augmentedActiveIconsList = new List<AugmentedActiveIcon>();
    public List<AugmentedExclamationMarkInfo> augmentedExclamationMarkList = new List<AugmentedExclamationMarkInfo>();
    public List<ActiveExclamationMarkInfo> augmentedActiveExclamationMarkList = new List<ActiveExclamationMarkInfo>();
    
    //public List<GameObject> instantiatedCapability = new List<GameObject>();
    //public List<GameObject> instantiatedEventCondition = new List<GameObject>();
    public Dictionary<string, bool> isAnchoredCubePanelDict = new Dictionary<string, bool>();
    public Dictionary<string, bool> isAnchoredExclamationMarkDict = new Dictionary<string, bool>();
    public Dictionary<string, bool> isAnchoredActiveExclamationMarkDict = new Dictionary<string, bool>();
    public Dictionary<string, bool> isAnchoredActiveIconsDict = new Dictionary<string, bool>();
    public Dictionary<string, bool> isAnchoredIconsDict = new Dictionary<string, bool>();

    public string lastImageTargetName = "";
    public Transform lastImageTargetTransform;
    public Camera currentCamera;
    public Camera currentCamera2;
    
    //GameObject instantiatedObject; // Non era questo il problema

    ARTrackedImageManager m_TrackedImageManager;
    public ARPlaceTrackedImages aRPlaceTrackedImages;

    // Gets a value indicating whether the Origin of the new World Coordinate System,
    // i.e. the Cloud Anchor was placed.
    public bool IsOriginPlaced
    {
        get;
        private set;
    }
    public SaveSerial saveSerial;

    public bool isSet = false;

    // The world origin transform for this session.
    // https://docs.unity3d.com/Manual/class-Transform.html
    //private Transform m_WorldOrigin = null;

    // The active AR Session Origin used in the example.
    public ARSessionOrigin SessionOrigin;

    // The various AR prefabs/game objs need to be PUBLIC to be seen from unity
    public GameObject _usedInRulePanel;

    // The recommendation panel game obj 
    public GameObject _recommendationPanel;
    
    // the Exclamation Mark Game obj
    public GameObject _inactiveExclamationMark;
    
    // the Exclamation Mark Game obj
    public GameObject _activeExclamationMark;
    
    private bool retreived = false; 
    public ARPlane lastPlane;
    public GameObject attachAnchor;
    public TextMesh new_anchorObj_mesh;
    
    private List<Pose> poseList = new List<Pose>();
    

    // Reference to logging UI element in the canvas
    public UnityEngine.UI.Text Log;
    
    // List for labels of objects with an anchor placed
    public List<string> labelsOfAnchors = new List<string>();
    
    
    // List for trackabels already used (anchor is placed on them)
    public List<ARTrackable> trackableList = new List<ARTrackable>();
    
    // List for raycast hits is re-used by raycast manager
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    
    // stores the anchor inserted into the environment with the associated boundingbox 
    IDictionary<ARAnchor, BoundingBox> anchorDic = new Dictionary<ARAnchor, BoundingBox>();

    // stores the anchor inserted into the environment withh the associated trackable
    // (feature point, planeWithinPolygon, ... )
    IDictionary<ARTrackable, ARAnchor> trackableDic = new Dictionary<ARTrackable, ARAnchor>();

    // Needed to estabilish the position of the phne WRT apollo. We need to know this because we want to 
    // generate recommendations related to the obj nearer to the user, but the obj coordinates are wrt apollo, 
    // while the phone coords are wrt where the application starts. Hence, when an apollo is tracked, we 
    // save the distance from phone (origin?) and its position, to calculate subsequently obtain the position of
    // the phone wrt apollo. 
    Vector3 apolloPositionForPhone = new Vector3();
    int nearestObject = -1;

    // stores the raycasthit with the associated anchor
    // There is no need to also store the trackable (see https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@4.1/api/UnityEngine.XR.ARFoundation.ARTrackable.html))
    // because it is stored in the .hit property of the raycastHit
    IDictionary<ARRaycastHit, ARAnchor> raycastHitDic = new Dictionary<ARRaycastHit, ARAnchor>();
    public Vector3 cubePanelInfoOriginalSize; 
    public Vector3 exclamationMarkOriginalSize; 
    public Vector3 activeExclamationMarkOriginalSize; 

    // from PhoneARCamera
    private List<BoundingBox> boxSavedOutlines;
    private float shiftX;
    private float shiftY;
    private float scaleFactor;

    public PhoneARCamera phoneARCamera;
   
    // Cache ARRaycastManager GameObject from ARCoreSession
    public ARRaycastManager m_RaycastManager;
  
    public TextMesh anchorObj_mesh;
    public ARAnchorManager m_AnchorManager; //

    // Raycast against planes and feature points
    // It will be used by the Select Object Location functionality, to detect the position in the real space where to place a AR viz. 
    // Generic planes also includes estimatedPlanes that are not optimal, but is better then rely on feature points that are difficult to tap
    // Using only real planes gives too few anchoring points
    const TrackableType trackableTypes = TrackableType.FeaturePoint | TrackableType.PlaneWithinPolygon | TrackableType.Planes | TrackableType.PlaneWithinBounds;
    //const TrackableType trackableTypes = TrackableType.Planes; //

}



/** //
 * Reference structure for the Exclamation Mark gameobject
 */
public struct AugmentedExclamationMarkInfo
{
    public string name;
    public string realName;
    public string trackableName; // INUTILE, c'è un unico trakable in tutto l'ambiente
    public string referenceRoom;
    public ActiveExclamationMarkInfo referenceObject;
    public AugmentedExclamationMarkInfo(string myName, string myRealName, string myTrackableName, string myReferenceRoom, ActiveExclamationMarkInfo myReferenceObject)
    {
        name = myName;
        realName = myRealName;
        trackableName = myTrackableName;
        referenceRoom = myReferenceRoom;
        referenceObject = myReferenceObject;
    }

}

/**
 * Reference structure for the Active Exclamation Mark gameobject //Potrebbe essere usata la stessa struct per excl e panels
 */
public struct ActiveExclamationMarkInfo
{
    public int id;
    public string name;
    public string realName;
    public string trackableName;
    public string referenceRoom;
    public Vector3 distanceFromTrackable;

    public ActiveExclamationMarkInfo(int myId, string myName, string myRealName, string myTrackableName, string myReferenceRoom, Vector3 myDistanceFromTrackable)
    {
        id = myId;
        name = myName;
        realName = myRealName;
        trackableName = myTrackableName;
        referenceRoom = myReferenceRoom;
        distanceFromTrackable = myDistanceFromTrackable;
    }
}


/*
 * needed because dictionaries are not keeped by unity in the Inspector,
 * so when a game object is deactivated it is a problem to gain it back 
 * and reactivate it. 
 */
[System.Serializable]
public struct instantiatedGameObjectSaver
{
    public string name;
    public GameObject gameObject;
}

