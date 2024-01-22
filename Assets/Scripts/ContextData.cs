using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


/**
 * Just a fake context server to test the UI part of the application. 
 * This class will retreive data from the context manager.
 * NOTA: Vengono usati i "fullName" per identificare in modo univoco le 
 * funzionalità, ma in realtà non serve a niente, perché nella definizione 
 * dei servizi () c'è già il campo "realName": è uno step in più, che 
 * rende poi necessario linkare il fullName al dispositivo
 */
public class ContextData : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        initializeActivableObjects();
        initializeActivableServices();
        initializeUsedInRules();
        initializeEnumType();
        initiateObjectCapabilities();
        initializeNonObjectRelatedCapabilities();
        initializeFullNameToObjectIdDict();
        initializeFullNameToServiceIdDict();
        initializeFullNameToDescDict();
        initializeFullNameToDescriptiveNameDict();
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public Dictionary<int, ActivableObject> activableObjects = new Dictionary<int, ActivableObject>();
    public Dictionary<int, List<ObjectOrServiceCapability>> objectsCapabilities = new Dictionary<int, List<ObjectOrServiceCapability>>();
    public Dictionary<string, ObjectOrServiceCapability> singleObjectOrServiceCapability = new Dictionary<string, ObjectOrServiceCapability>();
    public Dictionary<string, string> capabilitiesDescriptions = new Dictionary<string, string>(); //
    public Dictionary<string, string> fullNameToDescriptiveName = new Dictionary<string, string>();
    public Dictionary<int, ActivableService> activableServices = new Dictionary<int, ActivableService>();
    public Dictionary<int, List<ObjectOrServiceCapability>> serviceCapabilities = new Dictionary<int, List<ObjectOrServiceCapability>>();
    //public Dictionary<string, ObjectOrServiceCapability> singleServiceCapability = new Dictionary<string, ObjectOrServiceCapability>();
    public Dictionary<string, string> ruleElementType = new Dictionary<string, string>();
    public Dictionary<string, List<string>> ruleEnumType = new Dictionary<string, List<string>>();
    public Dictionary<string, List<string>> usedInRules = new Dictionary<string, List<string>>();
    public Dictionary<string, int> fullNameToObjectID = new Dictionary<string, int>();
    public Dictionary<string, int> fullNameToServiceID = new Dictionary<string, int>();
    //public static Dictionary<string, Vector> triggerType = new Dictionary<string, string>();
  
    public bool isAction(string myFullName)
    {
        if (singleObjectOrServiceCapability[myFullName].capabilityType == "a" || singleObjectOrServiceCapability[myFullName].capabilityType == "action") //
        {
            return true;
        }
        return false;
    }

   public string dataTypeFromFullName(string myFullName)
    {
        return singleObjectOrServiceCapability[myFullName].capabilityDataType;
    }

    public string capabilityFromFullName(string myFullName)
    {
        return singleObjectOrServiceCapability[myFullName].capabilityDesc;
    }

    /*
     * Check if the passed fullName correspond to an enum 
     * if no returns 0
     * if yes returns the number of choices for that enum
     */ 
    public int getEnumElementsNumber(string fullName)
    {
        List<string> myEnumTypes = ruleEnumType[fullName];
        return myEnumTypes.Count;
    }
    
    // Si può direttamente specificare il nome completo, e non usare questa
    // Non serve più, e inoltre è sbagliata perché non considera i services
    public string getTempObjectComposedName(int myObjId)
    {
        //string myParent = activableObjects[myObjId].parent;
        string myName = activableObjects[myObjId].name;
        //return myParent + " " + myName;
        return myName;
    }


    /**
     *  Returns the capabilties list assocaited to an object/service 
     */
    public List<ObjectOrServiceCapability> getObjectOrServicesCapabilitiesFromId(int id)
    {
        if (objectsCapabilities.ContainsKey(id))
        {
            return objectsCapabilities[id];
        }
        else if (serviceCapabilities.ContainsKey(id))
        {
            return serviceCapabilities[id];
        }
        throw new Exception("NO CAPABILITIES ASSOCIATED TO THIS OBJECT/SERVICE ID!");
    }

    public int getObjectOrServiceIdFromFullName(string fullName)
    {
        if (fullNameToObjectID.ContainsKey(fullName))
        {
            return fullNameToObjectID[fullName];
        }
        else if (fullNameToServiceID.ContainsKey(fullName))
        {
            return fullNameToServiceID[fullName];
        }
        return -1;

    }
    public int getServiceIdFromServiceFullName(string serviceName)
    {
        foreach (KeyValuePair<int, ActivableService> entry in activableServices)
        {
            if(entry.Value.parent + "-" + entry.Value.realName == serviceName)
            {
                return entry.Value.referenceId;
            }
        }
        return -1; // This should never happen
    }

    public int getServiceIdFromFullName(string fullName)
    {
        if (fullNameToServiceID.ContainsKey(fullName))
        {
            return fullNameToServiceID[fullName];
        }
        return -1;

    }
    public string getObjectRealNameFromId(int id)
    {
        return activableObjects[id].realName;
    }

    public int getObjectIdFromFullName(string fullName)
    {
        if (fullNameToObjectID.ContainsKey(fullName))
        {
            return fullNameToObjectID[fullName];
        }
        return -1;
    }

    public Vector3 getObjectPositionFromObjectId(int objectId)
    {
        if (activableObjects.ContainsKey(objectId))
        {
            return activableObjects[objectId].position;
        }
        return new Vector3(-1,-1,-1);
    }
    public Vector3 getObjectPositionWrtApolloFromObjectId(int objectId)
    {
        if (activableObjects.ContainsKey(objectId))
        {
            return activableObjects[objectId].positionWrtApollo;
        }
        return new Vector3(-1,-1,-1);
    }

    public Vector3 getObjectPositionFromFullName(string fullName)
    {
        int objectId = getObjectIdFromFullName(fullName);
        Vector3 objectPosition = getObjectPositionFromObjectId(objectId);
        return objectPosition;
    }
    public Vector3 getObjectPositionWrtApolloFromFullName(string fullName)
    {
        int objectId = getObjectIdFromFullName(fullName);
        Vector3 objectPosition = getObjectPositionFromObjectId(objectId);
        return objectPosition;
    }

    public string isObjectOrServiceFromCapabilityFullName(string fullName)
    {
        if (fullNameToObjectID.ContainsKey(fullName))
        {
            return "o";
        }
        else if (fullNameToServiceID.ContainsKey(fullName))
        {
            return "s";
        }
        else
        {
            return "error"; // TODO manage the error //
        }

    }
    public string isObjectOrServiceFromId(int referenceId)
    {
        //ScreenLog.Log("IS OBJECT OR SERVICE FROM ID: " + referenceId);
        if (activableObjects.ContainsKey(referenceId))
        {
            return "o";
        }
        else if (activableServices.ContainsKey(referenceId))
        {
            return "s";
        }
        else
        {
            return "error"; // TODO manage the error //
        }
    }
    public void initializeFullNameToDescriptiveNameDict()
    {
        foreach (KeyValuePair<string, ObjectOrServiceCapability> entry in singleObjectOrServiceCapability)
        {
            string myName = entry.Value.capabilityName;
            ScreenLog.Log("FULL NAME TO DESC NAME" + myName);
            ScreenLog.Log("FULL NAME TO DESC NAME" + entry.Value.capabilityFullName);
            fullNameToDescriptiveName.Add(entry.Value.capabilityFullName, myName);
        }
    }

    public void initializeFullNameToDescDict()
    {
        foreach (KeyValuePair<string, ObjectOrServiceCapability> entry in singleObjectOrServiceCapability)
        {
            capabilitiesDescriptions.Add(entry.Value.capabilityFullName, entry.Value.capabilityDesc);
        }
    }
    
    /**
     *  Todo add checks if not present (or return back fullName)
     */
    public string getDescriptiveNameFromFullName(string fullName)
    {
        return fullNameToDescriptiveName[fullName];
    }
   
    /**
     *  Todo add checks if not present (or return back fullName)
     */
    public string getDescriptionFromFullName(string fullName)
    {
        return capabilitiesDescriptions[fullName];
    }
    /**
     * Make the connection Capability - ObjectReferenceId (for services is a little different: is the "channel" full name, not the capability
     */
    public void initializeFullNameToServiceIdDict()
    {
        fullNameToServiceID.Add("reminders-reminders", 6);
        fullNameToServiceID.Add("alarms-alarms", 7);
        fullNameToServiceID.Add("currentweather-rain", 8);
        fullNameToServiceID.Add("currentweather-snow", 8);
        fullNameToServiceID.Add("currentweather-outdoorcondition", 8);
        fullNameToServiceID.Add("currentweather-outdoortemperature", 8);
        fullNameToServiceID.Add("twentyfourhoursweatherforecast-rain", 9);
        fullNameToServiceID.Add("twentyfourhoursweatherforecast-snow", 9);
        fullNameToServiceID.Add("twentyfourhoursweatherforecast-outdoorcondition", 9);
        fullNameToServiceID.Add("twentyfourhoursweatherforecast-outdoortemperature", 9);
        fullNameToServiceID.Add("datetime-localtime", 17);
        fullNameToServiceID.Add("weekday-localtime", 17);
        fullNameToServiceID.Add("daytype-localtime", 17);
        fullNameToServiceID.Add("relativeposition-typeofproximity", 18);
        fullNameToServiceID.Add("physiological-steps", 23);
        fullNameToServiceID.Add("cognitive-trainingtime", 23);
        //fullNameToServiceID.Add("datetime-datetime", 8);
        //fullNameToServiceID.Add("weather-weather", 9);
        return;
    }

    /**
     * Make the connection Capability - ObjectReferenceId
     */
    public void initializeFullNameToObjectIdDict()
    {
        fullNameToObjectID.Add("fridge-door", 1);
        fullNameToObjectID.Add("microwave-door", 2);
        
        fullNameToObjectID.Add("kitchen-gassensor", 4);
        fullNameToObjectID.Add("kitchen-smokesensor", 4);
        
        fullNameToObjectID.Add("kitchen-hue color light kitchen", 3);
        fullNameToObjectID.Add("entrance-hue color light entrance", 12);
        fullNameToObjectID.Add("livingroom-hue color light living room", 5);
        fullNameToObjectID.Add("bedroom-hue color light bedroom", 21);
        
        fullNameToObjectID.Add("kitchenlight-state", 3);
        fullNameToObjectID.Add("entrancelight-state", 12);
        fullNameToObjectID.Add("livingroomlight-state", 5);
        fullNameToObjectID.Add("bedroomlight-state", 21);

        fullNameToObjectID.Add("kitchen-alllight", 3);
        fullNameToObjectID.Add("entrance-alllight", 12);
        fullNameToObjectID.Add("livingroom-alllight", 5);
        fullNameToObjectID.Add("bedroom-alllight", 21);
        
        fullNameToObjectID.Add("kitchen-lightlevel", 10);
        fullNameToObjectID.Add("entrance-lightlevel", 13);
        fullNameToObjectID.Add("livingroom-lightlevel", 11);
        fullNameToObjectID.Add("bedroom-lightlevel", 20);
        
        fullNameToObjectID.Add("kitchen-temperaturelevel", 10);
        fullNameToObjectID.Add("entrance-temperaturelevel", 13);
        fullNameToObjectID.Add("livingroom-temperaturelevel", 11);
        fullNameToObjectID.Add("bedroom-temperaturelevel", 20);
        
        fullNameToObjectID.Add("kitchen-humiditylevel", 10);
        fullNameToObjectID.Add("entrance-humiditylevel", 13);
        fullNameToObjectID.Add("livingroom-humiditylevel", 11);
        fullNameToObjectID.Add("bedroom-humiditylevel", 20);
        
        
        fullNameToObjectID.Add("kitchen-motion", 10);
        fullNameToObjectID.Add("entrance-motion", 13);
        fullNameToObjectID.Add("livingroom-motion", 11);
        fullNameToObjectID.Add("bedroom-motion", 20);
        
        fullNameToObjectID.Add("entrance-doorsensor", 14); 
        fullNameToObjectID.Add("kitchen-windowsensor", 15);
        fullNameToObjectID.Add("livingroom-windowsensor", 16);
        fullNameToObjectID.Add("bedroom-windowsensor", 19);
        
        fullNameToObjectID.Add("entrance-door", 14); 
        fullNameToObjectID.Add("kitchen-window", 15);
        fullNameToObjectID.Add("livingroom-window", 16);
        fullNameToObjectID.Add("bedroom-window", 19);
        
        fullNameToObjectID.Add("sleep-sleepduration", 22);
        fullNameToObjectID.Add("sleep-bedoccupancy", 22);
        
        return;
    }

    /**
     * Similar to the object definition, but simpler (no position, no room)
     */
    public void initializeActivableServices()
    {
        string firstElementName = "Reminders";
        string firstElementRealName = "remindersservice";
        string firstElementParent = "reminders";
        int firstElementReferenceId = 6;
        ActivableService firstElement = new ActivableService(firstElementName, firstElementRealName, firstElementParent, firstElementReferenceId);
        activableServices.Add(firstElement.referenceId, firstElement);
        
        string secondElementName = "Alarms";
        string secondElementRealName = "alarmsservice";
        string secondElementParent = "alarms";
        int secondElementReferenceId = 7;
        ActivableService secondElement = new ActivableService(secondElementName, secondElementRealName, secondElementParent, secondElementReferenceId);
        activableServices.Add(secondElement.referenceId, secondElement);
       
        string fourthElementName = "Current weather";
        string fourthElementRealName = "currentweatherservice";
        string fourthElementParent = "currentweather";
        int fourthElementReferenceId = 8;
        ActivableService fourthElement = new ActivableService(fourthElementName, fourthElementRealName, fourthElementParent, fourthElementReferenceId);
        activableServices.Add(fourthElement.referenceId, fourthElement);
        
        string fifthElementName = "24 hours forecast";
        string fifthElementRealName = "twentyfourhoursweatherforecastservice";
        string fifthElementParent =   "twentyfourhoursweatherforecast";
        int fifthElementReferenceId = 9;
        ActivableService fifthElement = new ActivableService(fifthElementName, fifthElementRealName, fifthElementParent, fifthElementReferenceId);
        activableServices.Add(fifthElement.referenceId, fifthElement);
        
        string sixthElementName = "Date - Time";
        string sixthElementRealName = "datetimeservice";
        string sixthElementParent =   "datetime";
        int sixthElementReferenceId = 17;
        ActivableService sixthElement = new ActivableService(sixthElementName, sixthElementRealName, sixthElementParent, sixthElementReferenceId);
        activableServices.Add(sixthElement.referenceId, sixthElement);
        
        string seventhElementName = "relative position";
        string seventhElementRealName = "typeofproximityservice";
        string seventhElementParent =   "relativeposition";
        int seventhElementReferenceId = 18;
        ActivableService seventhElement = new ActivableService(seventhElementName, seventhElementRealName, seventhElementParent, seventhElementReferenceId);
        activableServices.Add(seventhElement.referenceId, seventhElement);
        
        string eightElementName = "Training";
        string eightElementRealName = "trainingservice";
        string eightElementParent =   "training";
        int eightElementReferenceId = 23;
        ActivableService eightElement = new ActivableService(eightElementName, eightElementRealName, eightElementParent, eightElementReferenceId);
        activableServices.Add(eightElement.referenceId, eightElement);
        

    }

    /**
     * Create a dict of objects.
     * They are references to the physical "real" objects, not related 
     * to xPaths.
     * They are realted to the Anchors and the Capabilities using the 
     * objectReferenceId.
     */
    public void initializeActivableObjects()
    {
        string fifthElementName = "Living Room Hue Lamp";
        string fifthElementRealName = "livingroomhuelampphysical";
        string fifthElementReferenceRoom = "living room";
        int fifthElementReferenceId = 5; //
        Vector3 fifthElementPosition = new Vector3(0f,0f,0f); 
        Vector3 fifthElementPositionWrtApollo = new Vector3(1.55f,0.6f,-4f);    //
        ActivableObject fifthElement = new ActivableObject(fifthElementName, fifthElementRealName, fifthElementReferenceRoom, fifthElementReferenceId, fifthElementPosition, fifthElementPositionWrtApollo);
        activableObjects.Add(fifthElement.referenceId, fifthElement); //
        
        string tenthElementName = "Entrance Door Sensor"; // Should be renamed entrance door
        string tenthElementRealName = "entrancedoorsensorphysical";
        string tenthElementReferenceRoom = "entrance";
        int tenthElementReferenceId = 14;
        Vector3 tenthElementPosition = new Vector3(0f,0f,0f);
        Vector3 tenthElementPositionWrtApollo = new Vector3(-1.4f,0.2f,-3.1f); //
        ActivableObject tenthElement = new ActivableObject(tenthElementName, tenthElementRealName, tenthElementReferenceRoom, tenthElementReferenceId, tenthElementPosition, tenthElementPositionWrtApollo);
        activableObjects.Add(tenthElement.referenceId, tenthElement);
        /*
        string firstElementName = "Fridge";
        string firstElementRealName = "fridgephysical";
        string firstElementReferenceRoom = "kitchen";
        int firstElementReferenceId = 1;
        Vector3 firstElementPosition = new Vector3(-2,0,3);
        Vector3 firstElementPositionWrtApollo = new Vector3(-2,0,3);
        ActivableObject firstElement = new ActivableObject(firstElementName, firstElementRealName, firstElementReferenceRoom, firstElementReferenceId, firstElementPosition, firstElementPositionWrtApollo);
        //activableObjects.Add(firstElement.referenceId, firstElement); //Kitchen is already crowded enougth
        
        string  secondElementName = "Microwave";
        string secondElementRealName = "microwavephysical";
        string secondElementReferenceRoom = "kitchen";
        int secondElementReferenceId = 2;
        Vector3 secondElementPosition = new Vector3(-4.2f,0,5.1f);
        Vector3 secondElementPositionWrtApollo = new Vector3(-1.3f,-0.2f,-6f);
        ActivableObject secondElement = new ActivableObject(secondElementName, secondElementRealName, secondElementReferenceRoom, secondElementReferenceId, secondElementPosition, secondElementPositionWrtApollo);
        activableObjects.Add(secondElement.referenceId, secondElement);
        
        string thirdElementName = "Kichen Hue Lamp";
        string thirdElementRealName = "kitchenhuelampphysical";
        string thirdElementReferenceRoom = "kitchen";
        int thirdElementReferenceId = 3;
        Vector3 thirdElementPosition = new Vector3(-2.6f,1.5f,3.6f);
        Vector3 thirdElementPositionWrtApollo = new Vector3(0.0f,1.0f,-4.2f);
        ActivableObject thirdElement = new ActivableObject(thirdElementName, thirdElementRealName, thirdElementReferenceRoom, thirdElementReferenceId, thirdElementPosition, thirdElementPositionWrtApollo);
        activableObjects.Add(thirdElement.referenceId, thirdElement);
        
        string fourthElementName = "Kitchen Gas and Smoke Sensor";
        string fourthElementRealName = "kitchengassmokesensorphysical";
        string fourthElementReferenceRoom = "kitchen";
        int fourthElementReferenceId = 4;
        Vector3 fourthElementPosition = new Vector3(-2f,0,3.9f);
        Vector3 fourthElementPositionWrtApollo = new Vector3(-0.5f,-0.4f,-4.7f);
        ActivableObject fourthElement = new ActivableObject(fourthElementName, fourthElementRealName, fourthElementReferenceRoom, fourthElementReferenceId, fourthElementPosition, fourthElementPositionWrtApollo);
        activableObjects.Add(fourthElement.referenceId, fourthElement);
        
        string fifthElementName = "Living Room Hue Lamp";
        string fifthElementRealName = "livingroomhuelampphysical";
        string fifthElementReferenceRoom = "living room";
        int fifthElementReferenceId = 5;
        Vector3 fifthElementPosition = new Vector3(0.3f,1.1f,7.2f); 
        Vector3 fifthElementPositionWrtApollo = new Vector3(-3.5f,0.6f,-1.5f);  // GOOD!!!!
        ActivableObject fifthElement = new ActivableObject(fifthElementName, fifthElementRealName, fifthElementReferenceRoom, fifthElementReferenceId, fifthElementPosition, fifthElementPositionWrtApollo);
        activableObjects.Add(fifthElement.referenceId, fifthElement);
        
        string sixthElementName = "Kitchen Hue Sensor";
        string sixthElementRealName = "kitchenhuesensorphysical";
        string sixthElementReferenceRoom = "kitchen";
        int sixthElementReferenceId = 10;
        Vector3 sixthElementPosition = new Vector3(-3.1f,0,3.8f); 
        Vector3 sixthElementPositionWrtApollo = new Vector3(-0.5f,-0.4f,-3.7f); 
        ActivableObject sixthElement = new ActivableObject(sixthElementName, sixthElementRealName, sixthElementReferenceRoom, sixthElementReferenceId, sixthElementPosition, sixthElementPositionWrtApollo);
        activableObjects.Add(sixthElement.referenceId, sixthElement);
        
        string seventhElementName = "Living Room Hue Sensor";
        string seventhElementRealName = "livingroomhuesensorphysical";
        string seventhElementReferenceRoom = "livingroom";
        int seventhElementReferenceId = 11;
        Vector3 seventhElementPosition = new Vector3(3.0f,-3.1f,14.4f);  
        Vector3 seventhElementPositionWrtApollo = new Vector3(-3.5f,-0.5f,-1.5f); 
        ActivableObject seventhElement = new ActivableObject(seventhElementName, seventhElementRealName, seventhElementReferenceRoom, seventhElementReferenceId, seventhElementPosition, seventhElementPositionWrtApollo);
        activableObjects.Add(seventhElement.referenceId, seventhElement);
        
        string eightElementName = "Entrance Hue Lamp";
        string eightElementRealName = "entrancehuelampphysical";
        string eightElementReferenceRoom = "entrance";
        int eightElementReferenceId = 12;
        Vector3 eightElementPosition = new Vector3(3.75f,0.55f,5.3f);
        Vector3 eightElementPositionWrtApollo = new Vector3(-1.8f,0.0f,2.3f);
        ActivableObject eightElement = new ActivableObject(eightElementName, eightElementRealName, eightElementReferenceRoom, eightElementReferenceId, eightElementPosition, eightElementPositionWrtApollo);
        activableObjects.Add(eightElement.referenceId, eightElement);
        
        string ninthElementName = "Entrance Hue Sensor";
        string ninthElementRealName = "entrancehuesensorphysical";
        string ninthElementReferenceRoom = "entrance";
        int ninthElementReferenceId = 13;
        Vector3 ninthElementPosition = new Vector3(3.75f,0.7f,6.4f); //
        Vector3 ninthElementPositionWrtApollo = new Vector3(-3.1f,0.2f,2.3f); //
        ActivableObject ninthElement = new ActivableObject(ninthElementName, ninthElementRealName, ninthElementReferenceRoom, ninthElementReferenceId, ninthElementPosition, ninthElementPositionWrtApollo);
        activableObjects.Add(ninthElement.referenceId, ninthElement);
        
        string tenthElementName = "Entrance Door Sensor"; // Should be renamed entrance door
        string tenthElementRealName = "entrancedoorsensorphysical";
        string tenthElementReferenceRoom = "entrance";
        int tenthElementReferenceId = 14;
        Vector3 tenthElementPosition = new Vector3(3.72f,0.15f,4.6f);
        Vector3 tenthElementPositionWrtApollo = new Vector3(-1.2f,-0.2f,2.3f);
        ActivableObject tenthElement = new ActivableObject(tenthElementName, tenthElementRealName, tenthElementReferenceRoom, tenthElementReferenceId, tenthElementPosition, tenthElementPositionWrtApollo);
        activableObjects.Add(tenthElement.referenceId, tenthElement);
        
        string eleventhElementName = "Kitchen Window Sensor"; // should be renamed kitchen window
        string eleventhElementRealName = "kitchenwindowsensorphysical";
        string eleventhElementReferenceRoom = "kitchen";
        int eleventhElementReferenceId = 15;
        Vector3 eleventhElementPosition = new Vector3(-2.2f,0,5.3f);
        Vector3 eleventhElementPositionWrtApollo = new Vector3(-1.8f,0,-3.7f);
        ActivableObject eleventhElement = new ActivableObject(eleventhElementName, eleventhElementRealName, eleventhElementReferenceRoom, eleventhElementReferenceId, eleventhElementPosition, eleventhElementPositionWrtApollo);
        activableObjects.Add(eleventhElement.referenceId, eleventhElement);
        
        string twelvthElementName = "Living Room Window Sensor"; // etc. 
        string twelvthElementRealName = "livingroomwindowsensorphysical";
        string twelvthElementReferenceRoom = "livingroom";
        int twelvthElementReferenceId = 16;
        Vector3 twelvthElementPosition = new Vector3(2.1f,0.5f,8.6f); //
        Vector3 twelvthElementPositionWrtApollo = new Vector3(-5.4f,0.5f,0.6f); 
        ActivableObject twelvthElement = new ActivableObject(twelvthElementName, twelvthElementRealName, twelvthElementReferenceRoom, twelvthElementReferenceId, twelvthElementPosition, twelvthElementPositionWrtApollo);
        activableObjects.Add(twelvthElement.referenceId, twelvthElement);
       
        string thirthinthElementName = "Bedroom Window Sensor"; //etc. 
        string thirthinthElementRealName = "bedroomwindowsensorphysical";
        string thirthinthElementReferenceRoom = "bedroom";
        int thirthinthElementReferenceId = 19;
        Vector3 thirthinthElementPosition = new Vector3(2.7f,0.5f,-2.4f); //
        Vector3 thirthinthElementPositionWrtApollo = new Vector3(4.2f,0.0f,1.2f); //
        ActivableObject thirthinthElement = new ActivableObject(thirthinthElementName, thirthinthElementRealName, thirthinthElementReferenceRoom, thirthinthElementReferenceId, thirthinthElementPosition, thirthinthElementPositionWrtApollo);
        activableObjects.Add(thirthinthElement.referenceId, thirthinthElement);
        
        string fourtenthElementName = "Bedroom Hue Sensor";
        string fourtenthElementRealName = "bedroomhuesensorphysical";
        string fourtenthElementReferenceRoom = "bedroom";
        int fourtenthElementReferenceId = 20;
        Vector3 fourtenthElementPosition = new Vector3(2.3f,-0.1f,1.7f);  //
        Vector3 fourtenthElementPositionWrtApollo = new Vector3(2.0f,-0.5f,0.8f);  //
        ActivableObject fourtenthElement = new ActivableObject(fourtenthElementName, fourtenthElementRealName, fourtenthElementReferenceRoom, fourtenthElementReferenceId, fourtenthElementPosition, fourtenthElementPositionWrtApollo);
        activableObjects.Add(fourtenthElement.referenceId, fourtenthElement);
        
        string fifteenthElementName = "Bedroom Hue Lamp";
        string fifteenthElementRealName = "bedroomhuelampphysical"; //
        string fifteenthElementReferenceRoom = "bedroom";
        int fifteenthElementReferenceId = 21;
        Vector3 fifteenthElementPosition = new Vector3(3.6f,1.2f,.8f); //
        Vector3 fifteenthElementPositionWrtApollo = new Vector3(2.6f,0.7f,2.3f); //
        ActivableObject fifteenthElement = new ActivableObject(fifteenthElementName, fifteenthElementRealName, fifteenthElementReferenceRoom, fifteenthElementReferenceId, fifteenthElementPosition, fifteenthElementPositionWrtApollo);
        activableObjects.Add(fifteenthElement.referenceId, fifteenthElement);
        
        string sixteenthElementName = "Bed";
        string sixteenthElementRealName = "bedphysical";
        string sixteenthElementReferenceRoom = "bedroom";
        int sixteenthElementReferenceId = 22;
        Vector3 sixteenthElementPosition = new Vector3(3.4f,-0.4f,.4f); //
        Vector3 sixteenthElementPositionWrtApollo = new Vector3(2.9f,-0.6f,1.5f); //
        ActivableObject sixteenthElement = new ActivableObject(sixteenthElementName, sixteenthElementRealName, sixteenthElementReferenceRoom, sixteenthElementReferenceId, sixteenthElementPosition, sixteenthElementPositionWrtApollo);
        activableObjects.Add(sixteenthElement.referenceId, sixteenthElement);
        */
    }

    // TODO use the objectReferenceId instead!
    public void initializeUsedInRules()
    {
        usedInRules.Add("fridgephysical", new List<String>());
        usedInRules.Add("microwavephysical", new List<String>());
        usedInRules.Add("kitchenhuelampphysical", new List<String>());
        usedInRules.Add("kitchengassmokesensorphysical", new List<String>());
        usedInRules.Add("livingroomhuelampphysical", new List<String>());
        usedInRules.Add("kitchenhuesensorphysical", new List<String>());
        usedInRules.Add("livingroomhuesensorphysical",new List<String>());
        usedInRules.Add("reminderservice", new List<String>());
        usedInRules.Add("alertsservice", new List<String>());
        usedInRules.Add("currentweatherphysical", new List<String>());
        usedInRules.Add("twentyfourhoursweatherforecastphysical", new List<String>());
        usedInRules.Add("entrancehuesensorphysical", new List<String>());
        usedInRules.Add("entrancehuelampphysical", new List<String>());
        usedInRules.Add("entrancedoorsensorphysical", new List<String>());
        usedInRules.Add("livingroomwindowsensorphysical", new List<String>());
        usedInRules.Add("kitchenwindowsensorphysical", new List<String>());
        usedInRules.Add("datetimeservice", new List<String>());
        usedInRules.Add("typeofproximityservice", new List<String>());
        usedInRules.Add("bedroomwindowsensorphysical", new List<String>());
        usedInRules.Add("bedroomhuesensorphysical", new List<String>());
        usedInRules.Add("bedroomhuelampphysical", new List<String>());
        usedInRules.Add("bedphysical", new List<String>());
        usedInRules.Add("trainingservice", new List<String>());
    }

    public Dictionary<string, List<String>> getUsedInRulesDict()
    {
        return usedInRules;
    }

    public void updateUsedInRulesDict(Rule savedRule)
    {
        ScreenLog.Log("I WILL UPDATE THE USED IN RULE PANELS FOR" + savedRule.name);
        String ruleName = savedRule.name;
        String ruleNl = savedRule.nl;
        ScreenLog.Log("RETREIVED RULE: " + ruleName);
        ScreenLog.Log(ruleNl);
        foreach (KeyValuePair<int, RuleElement> element in savedRule.ruleElements)
        {
            string myCapability = element.Value.fullName;
            if (isObjectOrServiceFromCapabilityFullName(myCapability) == "o")
            {
                int myReferenceObjectId = element.Value.refId;
                //ScreenLog.Log("GET AN OBJ WITH REFERENCE ID " + myReferenceObjectId);
                // Get the realName associated to that object
                string myRealName = getObjectRealNameFromId(myReferenceObjectId);
                //ScreenLog.Log("GET A REAL NAME " + myRealName); //
                usedInRules[myRealName].Add(ruleName + ": " + ruleNl);
            }
        }
        //ScreenLog.Log("UPDATED ALL OBJECTS USED IN RULES!!!!!!!!!!!!!!!!!!!!!!");
    }


    /**
     * Explicitate the various enums of a Capability (We are not referring 
     * to the physical object here)
     */
    public void initializeEnumType()
    {
        List<string> fridgeDoorEnum = new List<string>();
        fridgeDoorEnum.Add("OPEN");
        fridgeDoorEnum.Add("CLOSE");
        ruleEnumType.Add("fridge-door", fridgeDoorEnum);
        ruleEnumType.Add("microwave-door", fridgeDoorEnum);
        ruleEnumType.Add("entrance-doorsensor", fridgeDoorEnum); 
        ruleEnumType.Add("kitchen-windowsensor", fridgeDoorEnum); 
        ruleEnumType.Add("livingroom-windowsensor", fridgeDoorEnum); 
        ruleEnumType.Add("bedroom-windowsensor", fridgeDoorEnum); 
        ruleEnumType.Add("entrance-door", fridgeDoorEnum); 
        ruleEnumType.Add("kitchen-window", fridgeDoorEnum); 
        ruleEnumType.Add("livingroom-window", fridgeDoorEnum); 
        ruleEnumType.Add("bedroom-window", fridgeDoorEnum); 
        
        
        List<string> kitchenLampEnum = new List<string>();
        kitchenLampEnum.Add("WHITE");
        kitchenLampEnum.Add("BLUE");
        kitchenLampEnum.Add("BEIGE");
        kitchenLampEnum.Add("PURPLE");
        kitchenLampEnum.Add("GREEN");
        kitchenLampEnum.Add("RED");
        kitchenLampEnum.Add("AZURE");
        kitchenLampEnum.Add("PINK");
        kitchenLampEnum.Add("CREAM");
        kitchenLampEnum.Add("VIOLET");
        kitchenLampEnum.Add("YELLOW");
        kitchenLampEnum.Add("BROWN");
        kitchenLampEnum.Add("ORANGE");
       
        ruleEnumType.Add("kitchen-hue color light kitchen", kitchenLampEnum);
        ruleEnumType.Add("livingroom-hue color light living room", kitchenLampEnum);
        ruleEnumType.Add("entrance-hue color light entrance", kitchenLampEnum);
        ruleEnumType.Add("bedroom-hue color light bedroom", kitchenLampEnum);
        
        List<string> kitchenLampStateEnum = new List<string>();
        kitchenLampStateEnum.Add("OFF");
        kitchenLampStateEnum.Add("ON");
        
        ruleEnumType.Add("kitchenlight-state", kitchenLampStateEnum);
        ruleEnumType.Add("livingroomlight-state", kitchenLampStateEnum);
        ruleEnumType.Add("entrancelight-state", kitchenLampStateEnum);
        ruleEnumType.Add("bedroomlight-state", kitchenLampStateEnum);
        ruleEnumType.Add("kitchen-alllight", kitchenLampStateEnum);
        ruleEnumType.Add("livingroom-alllight", kitchenLampStateEnum);
        ruleEnumType.Add("entrance-alllight", kitchenLampStateEnum);
        ruleEnumType.Add("bedroom-alllight", kitchenLampStateEnum);
        
        List<string> kitchenLightSensorEnum = new List<string>(); // //
        kitchenLightSensorEnum.Add("DAY_LIGHT");
        kitchenLightSensorEnum.Add("LOW_LIGHT");
        kitchenLightSensorEnum.Add("VERY_LOW_LIGHT");
        kitchenLightSensorEnum.Add("DARK");
        kitchenLightSensorEnum.Add("NORMAL_LIGHT");
        kitchenLightSensorEnum.Add("NO_LIGHT");
        
        ruleEnumType.Add("kitchen-lightlevel", kitchenLightSensorEnum);
        ruleEnumType.Add("livingroom-lightlevel", kitchenLightSensorEnum);
        ruleEnumType.Add("entrance-lightlevel", kitchenLightSensorEnum);
        ruleEnumType.Add("bedroom-lightlevel", kitchenLightSensorEnum);
        
        List<string> outdoorConditionEnum = new List<string>();
        outdoorConditionEnum.Add("FEW_CLOUDS");
        outdoorConditionEnum.Add("RAIN");
        outdoorConditionEnum.Add("CLEAR_SKY");
        outdoorConditionEnum.Add("BROKEN_CLOUDS");
        outdoorConditionEnum.Add("SCATTERED_CLOUDS");
        outdoorConditionEnum.Add("SNOW");
        outdoorConditionEnum.Add("SHOVER_RAIN");
        outdoorConditionEnum.Add("THUNDERSTORM");
        outdoorConditionEnum.Add("MIST");
        outdoorConditionEnum.Add("FOG");
        ruleEnumType.Add("currentweather-outdoorcondition", outdoorConditionEnum);
        ruleEnumType.Add("twentyfourhoursweatherforecast-outdoorcondition", outdoorConditionEnum);
        
        List<string> positionConditionEnum = new List<string>();
        positionConditionEnum.Add("ENTRANCE");
        positionConditionEnum.Add("KITCHEN");
        positionConditionEnum.Add("LIVING_ROOM");
        positionConditionEnum.Add("BEDROOM");
        positionConditionEnum.Add("BATHROOM");
        positionConditionEnum.Add("HOUSE");
        positionConditionEnum.Add("GARDEN");
        ruleEnumType.Add("relativeposition-typeofproximity", positionConditionEnum); //


        List<string> weekDayEnum = new List<string>();
        weekDayEnum.Add("Monday");
        weekDayEnum.Add("Tuesday");
        weekDayEnum.Add("Wednesday");
        weekDayEnum.Add("Thursday");
        weekDayEnum.Add("Friday");
        weekDayEnum.Add("Saturnday");
        weekDayEnum.Add("Sunday");
        ruleEnumType.Add("datetime-weekday", weekDayEnum); //
        
        List<string> dayTypeEnum = new List<string>();
        dayTypeEnum.Add("WEEK_DAYS");
        dayTypeEnum.Add("HOLYDAY");
        ruleEnumType.Add("datetime-daytype", dayTypeEnum); //
        
        List<string> bedOccupancyEnum = new List<string>();
        bedOccupancyEnum.Add("IN");
        bedOccupancyEnum.Add("OUT");
        ruleEnumType.Add("sleep-bedoccupancy", bedOccupancyEnum); //
    }

    /**
     * For the moment these capabilities are placed in a separated capability list
     */
    public void initializeNonObjectRelatedCapabilities()
    {
        List<ObjectOrServiceCapability> reminderCapabilitiesList = new List<ObjectOrServiceCapability>();
        int reminderCapabilityReferenceObjectID = 6;
        string reminderCapabilityName = "Reminders";
        string reminderCapabilityType = "a";
        string reminderCapabilityDataType = "STRING";
        string reminderCapabilityRealName = "reminders";
        string reminderCapabilityParent = "reminders";
        string reminderCapabilityFullName = "reminders-reminders";
        string reminderCapabilityDesc = "Send a reminder (Action)";
        ObjectOrServiceCapability reminderCapability = new ObjectOrServiceCapability(reminderCapabilityReferenceObjectID, reminderCapabilityName, reminderCapabilityType, reminderCapabilityDataType, reminderCapabilityRealName, reminderCapabilityParent, reminderCapabilityFullName, reminderCapabilityDesc);
        reminderCapabilitiesList.Add(reminderCapability);
        singleObjectOrServiceCapability.Add(reminderCapabilityFullName, reminderCapability);
        
        serviceCapabilities.Add(reminderCapability.objectReferenceId, reminderCapabilitiesList);
        
        List<ObjectOrServiceCapability> alertCapabilitiesList = new List<ObjectOrServiceCapability>();
        int alertCapabilityReferenceObjectID = 7;
        string alertCapabilityName = "Alarms";
        string alertCapabilityType = "a";
        string alertCapabilityDataType = "STRING";
        string alertCapabilityRealName = "alarms";
        string alertCapabilityParent = "alarms";
        string alertCapabilityFullName = "alarms-alarms";
        string alertCapabilityDesc = "Send an alarm (Action)";
        ObjectOrServiceCapability alertCapability = new ObjectOrServiceCapability(alertCapabilityReferenceObjectID, alertCapabilityName, alertCapabilityType, alertCapabilityDataType, alertCapabilityRealName, alertCapabilityParent, alertCapabilityFullName, alertCapabilityDesc);
        alertCapabilitiesList.Add(alertCapability);
        singleObjectOrServiceCapability.Add(alertCapabilityFullName, alertCapability);
        
        serviceCapabilities.Add(alertCapability.objectReferenceId, alertCapabilitiesList);
        
        List<ObjectOrServiceCapability> currentWeatherCapabilitiesList = new List<ObjectOrServiceCapability>();

        int rainCapabilityReferenceObjectID = 8;
        string rainCapabilityName = "Rain";
        string rainCapabilityType = "t";
        string rainCapabilityDataType = "BOOLEAN";
        string rainCapabilityRealName = "rain";
        string rainCapabilityParent = "currentweather";
        string rainCapabilityFullName = "currentweather-rain";
        string rainCapabilityDesc = "Check if it is raining (Trigger)";
        ObjectOrServiceCapability rainCapability = new ObjectOrServiceCapability(rainCapabilityReferenceObjectID, rainCapabilityName, rainCapabilityType, rainCapabilityDataType, rainCapabilityRealName, rainCapabilityParent, rainCapabilityFullName, rainCapabilityDesc);
        currentWeatherCapabilitiesList.Add(rainCapability);
        singleObjectOrServiceCapability.Add(rainCapabilityFullName, rainCapability);
        
        int snowCapabilityReferenceObjectID = 8;
        string snowCapabilityName = "Snow";
        string snowCapabilityType = "t";
        string snowCapabilityDataType = "BOOLEAN";
        string snowCapabilityRealName = "snow";
        string snowCapabilityParent = "currentweather";
        string snowCapabilityFullName = "currentweather-snow";
        string snowCapabilityDesc = "Check if it is snowing (Trigger)";
        ObjectOrServiceCapability snowCapability = new ObjectOrServiceCapability(snowCapabilityReferenceObjectID, snowCapabilityName, snowCapabilityType, snowCapabilityDataType, snowCapabilityRealName, snowCapabilityParent, snowCapabilityFullName, snowCapabilityDesc);
        currentWeatherCapabilitiesList.Add(snowCapability);
        singleObjectOrServiceCapability.Add(snowCapabilityFullName, snowCapability);
        
        int outdoorConditionCapabilityReferenceObjectID = 8;
        string outdoorConditionCapabilityName = "Outdoor Condition";
        string outdoorConditionCapabilityType = "t";
        string outdoorConditionCapabilityDataType = "ENUM";
        string outdoorConditionCapabilityRealName = "outdoorcondition";
        string outdoorConditionCapabilityParent = "currentweather";
        string outdoorConditionCapabilityFullName = "currentweather-outdoorcondition";
        string outdoorConditionCapabilityDesc = "Check the outdoor condition (Trigger)";
        ObjectOrServiceCapability outdoorConditionCapability = new ObjectOrServiceCapability(outdoorConditionCapabilityReferenceObjectID, outdoorConditionCapabilityName, outdoorConditionCapabilityType, outdoorConditionCapabilityDataType, outdoorConditionCapabilityRealName, outdoorConditionCapabilityParent, outdoorConditionCapabilityFullName, outdoorConditionCapabilityDesc);
        currentWeatherCapabilitiesList.Add(outdoorConditionCapability);
        singleObjectOrServiceCapability.Add(outdoorConditionCapabilityFullName, outdoorConditionCapability);
        
        int outdoorTemperatureCapabilityReferenceObjectID = 8;
        string outdoorTemperatureCapabilityName = "Outdoor Temperature";
        string outdoorTemperatureCapabilityType = "t";
        string outdoorTemperatureCapabilityDataType = "INTEGER";
        string outdoorTemperatureCapabilityRealName = "outdoortemperature";
        string outdoorTemperatureCapabilityParent = "currentweather";
        string outdoorTemperatureCapabilityFullName = "currentweather-outdoortemperature";
        string outdoorTemperatureCapabilityDesc = "Check the outdoor temperature (Trigger)";
        ObjectOrServiceCapability outdoorTemperatureCapability = new ObjectOrServiceCapability(outdoorTemperatureCapabilityReferenceObjectID, outdoorTemperatureCapabilityName, outdoorTemperatureCapabilityType, outdoorTemperatureCapabilityDataType, outdoorTemperatureCapabilityRealName, outdoorTemperatureCapabilityParent, outdoorTemperatureCapabilityFullName, outdoorTemperatureCapabilityDesc);
        currentWeatherCapabilitiesList.Add(outdoorTemperatureCapability);
        singleObjectOrServiceCapability.Add(outdoorTemperatureCapabilityFullName, outdoorTemperatureCapability);
        
        serviceCapabilities.Add(rainCapability.objectReferenceId, currentWeatherCapabilitiesList);
       
        /////////// 24 h forecast
        
        List<ObjectOrServiceCapability> weatherForecastCapabilitiesList = new List<ObjectOrServiceCapability>();

        int rainForecastCapabilityReferenceObjectID = 9;
        string rainForecastCapabilityName = "Rain Forecast in 24 hours";
        string rainForecastCapabilityType = "t";
        string rainForecastCapabilityDataType = "BOOLEAN";
        string rainForecastCapabilityRealName = "rain";
        string rainForecastCapabilityParent = "twentyfourhoursweatherforecast";
        string rainForecastCapabilityFullName = "twentyfourhoursweatherforecast-rain";
        string rainForecastCapabilityDesc = "Check if rain is forecasted in 24 hours(Trigger)";
        ObjectOrServiceCapability rainForecastCapability = new ObjectOrServiceCapability(rainForecastCapabilityReferenceObjectID, rainForecastCapabilityName, rainForecastCapabilityType, rainForecastCapabilityDataType, rainForecastCapabilityRealName, rainForecastCapabilityParent, rainForecastCapabilityFullName, rainForecastCapabilityDesc);
        weatherForecastCapabilitiesList.Add(rainForecastCapability);
        singleObjectOrServiceCapability.Add(rainForecastCapabilityFullName, rainForecastCapability);
       
        int snowForecastCapabilityReferenceObjectID = 9;
        string snowForecastCapabilityName = "Snow Forecast in 24 hours";
        string snowForecastCapabilityType = "t";
        string snowForecastCapabilityDataType = "BOOLEAN";
        string snowForecastCapabilityRealName = "snow";
        string snowForecastCapabilityParent = "twentyfourhoursweatherforecast";
        string snowForecastCapabilityFullName = "twentyfourhoursweatherforecast-snow";
        string snowForecastCapabilityDesc = "Check if snow is forecasted in 24 hours (Trigger)";
        ObjectOrServiceCapability snowForecastCapability = new ObjectOrServiceCapability(snowForecastCapabilityReferenceObjectID, snowForecastCapabilityName, snowForecastCapabilityType, snowForecastCapabilityDataType, snowForecastCapabilityRealName, snowForecastCapabilityParent, snowForecastCapabilityFullName, snowForecastCapabilityDesc);
        weatherForecastCapabilitiesList.Add(snowForecastCapability);
        singleObjectOrServiceCapability.Add(snowForecastCapabilityFullName, snowForecastCapability);
        
        int outdoorConditionForecastCapabilityReferenceObjectID = 9;
        string outdoorConditionForecastCapabilityName = "Outdoor Condition forecast in 24 hours";
        string outdoorConditionForecastCapabilityType = "t";
        string outdoorConditionForecastCapabilityDataType = "ENUM";
        string outdoorConditionForecastCapabilityRealName = "outdoorcondition";
        string outdoorConditionForecastCapabilityParent = "twentyfourhoursweatherforecast";
        string outdoorConditionForecastCapabilityFullName = "twentyfourhoursweatherforecast-outdoorcondition";
        string outdoorConditionForecastCapabilityDesc = "Check the forecasted outdoor condition in 24 hours (Trigger)";
        ObjectOrServiceCapability outdoorConditionForecastCapability = new ObjectOrServiceCapability(outdoorConditionForecastCapabilityReferenceObjectID, outdoorConditionForecastCapabilityName, outdoorConditionForecastCapabilityType, outdoorConditionForecastCapabilityDataType, outdoorConditionForecastCapabilityRealName, outdoorConditionForecastCapabilityParent, outdoorConditionForecastCapabilityFullName, outdoorConditionForecastCapabilityDesc);
        weatherForecastCapabilitiesList.Add(outdoorConditionForecastCapability);
        singleObjectOrServiceCapability.Add(outdoorConditionForecastCapabilityFullName, outdoorConditionForecastCapability);
        
        int outdoorTemperatureForecastCapabilityReferenceObjectID = 9;
        string outdoorTemperatureForecastCapabilityName = "Outdoor Temperature forecast in 24 hours";
        string outdoorTemperatureForecastCapabilityType = "t";
        string outdoorTemperatureForecastCapabilityDataType = "INTEGER";
        string outdoorTemperatureForecastCapabilityRealName = "outdoortemperature";
        string outdoorTemperatureForecastCapabilityParent = "twentyfourhoursweatherforecast";
        string outdoorTemperatureForecastCapabilityFullName = "twentyfourhoursweatherforecast-outdoortemperature";
        string outdoorTemperatureForecastCapabilityDesc = "Check the forecaster outdoor temperature in 24 hours (Trigger)";
        ObjectOrServiceCapability outdoorTemperatureForecastCapability = new ObjectOrServiceCapability(outdoorTemperatureForecastCapabilityReferenceObjectID, outdoorTemperatureForecastCapabilityName, outdoorTemperatureForecastCapabilityType, outdoorTemperatureForecastCapabilityDataType, outdoorTemperatureForecastCapabilityRealName, outdoorTemperatureForecastCapabilityParent, outdoorTemperatureForecastCapabilityFullName, outdoorTemperatureForecastCapabilityDesc);
        weatherForecastCapabilitiesList.Add(outdoorTemperatureForecastCapability);
        singleObjectOrServiceCapability.Add(outdoorTemperatureForecastCapabilityFullName, outdoorTemperatureForecastCapability);
        
        serviceCapabilities.Add(rainForecastCapability.objectReferenceId, weatherForecastCapabilitiesList);

        // Date time

        List<ObjectOrServiceCapability> dateTimeCapabilitiesList = new List<ObjectOrServiceCapability>();

        int localTimeCapabilityReferenceObjectID = 17;
        string localTimeCapabilityName = "Local Time";
        string localTimeCapabilityType = "t";
        string localTimeCapabilityDataType = "DATE";
        string localTimeCapabilityRealName = "localtime";
        string localTimeCapabilityParent = "datetime";
        string localTimeCapabilityFullName = "datetime-localtime";
        string localTimeCapabilityDesc = "Check if it's this time";
        ObjectOrServiceCapability localTimeCapability = new ObjectOrServiceCapability(localTimeCapabilityReferenceObjectID, localTimeCapabilityName, localTimeCapabilityType, localTimeCapabilityDataType, localTimeCapabilityRealName, localTimeCapabilityParent, localTimeCapabilityFullName, localTimeCapabilityDesc);
        dateTimeCapabilitiesList.Add(localTimeCapability);
        singleObjectOrServiceCapability.Add(localTimeCapabilityFullName, localTimeCapability);
       
        // Eventually add other datetime capabilities
        
        int weekDayCapabilityReferenceObjectID = 17;
        string weekDayCapabilityName = "Week Day";
        string weekDayCapabilityType = "t";
        string weekDayCapabilityDataType = "ENUM";
        string weekDayCapabilityRealName = "weekday";
        string weekDayCapabilityParent = "datetime";
        string weekDayCapabilityFullName = "datetime-weekday";
        string weekDayCapabilityDesc = "Check what day of the week is";
        ObjectOrServiceCapability weekDayCapability = new ObjectOrServiceCapability(weekDayCapabilityReferenceObjectID, weekDayCapabilityName, weekDayCapabilityType, weekDayCapabilityDataType, weekDayCapabilityRealName, weekDayCapabilityParent, weekDayCapabilityFullName, weekDayCapabilityDesc);
        dateTimeCapabilitiesList.Add(weekDayCapability);
        singleObjectOrServiceCapability.Add(weekDayCapabilityFullName, weekDayCapability);

        int dayTypeCapabilityReferenceObjectID = 17;
        string dayTypeCapabilityName = "Day Type";
        string dayTypeCapabilityType = "t";
        string dayTypeCapabilityDataType = "ENUM";
        string dayTypeCapabilityRealName = "daytype";
        string dayTypeCapabilityParent = "datetime";
        string dayTypeCapabilityFullName = "datetime-daytype";
        string dayTypeCapabilityDesc = "Check the type of the day";
        ObjectOrServiceCapability dayTypeCapability = new ObjectOrServiceCapability(dayTypeCapabilityReferenceObjectID, dayTypeCapabilityName, dayTypeCapabilityType, dayTypeCapabilityDataType, dayTypeCapabilityRealName, dayTypeCapabilityParent, dayTypeCapabilityFullName, dayTypeCapabilityDesc);
        dateTimeCapabilitiesList.Add(dayTypeCapability);
        singleObjectOrServiceCapability.Add(dayTypeCapabilityFullName, dayTypeCapability);
        
        serviceCapabilities.Add(localTimeCapability.objectReferenceId, dateTimeCapabilitiesList);
        
       // Position 
        
        List<ObjectOrServiceCapability> positionCapabilitiesList = new List<ObjectOrServiceCapability>();

        ScreenLog.Log("BEGIN POSITION SERIVECE");
        int positionCapabilityReferenceObjectID = 18;
        string positionCapabilityName = "relative position";
        string positionCapabilityType = "t";
        string positionCapabilityDataType = "ENUM";
        string positionCapabilityRealName = "typeofproximity";
        string positionCapabilityParent = "relativeposition";
        string positionCapabilityFullName = "relativeposition-typeofproximity";
        string positionCapabilityDesc = "Check the position of the user";
        ObjectOrServiceCapability positionCapability = new ObjectOrServiceCapability(positionCapabilityReferenceObjectID, positionCapabilityName, positionCapabilityType, positionCapabilityDataType, positionCapabilityRealName, positionCapabilityParent, positionCapabilityFullName, positionCapabilityDesc);
        positionCapabilitiesList.Add(positionCapability);
        singleObjectOrServiceCapability.Add(positionCapabilityFullName, positionCapability);
       
        serviceCapabilities.Add(positionCapability.objectReferenceId, positionCapabilitiesList);
        ScreenLog.Log("POSITION SERIVE ADDED");
        
        List<ObjectOrServiceCapability> trainingCapabilitiesList = new List<ObjectOrServiceCapability>();

        ScreenLog.Log("BEGIN TRAINING SERIVECE");
        int stepsCapabilityReferenceObjectID = 23;
        string stepsCapabilityName = "steps";
        string stepsCapabilityType = "t";
        string stepsCapabilityDataType = "INTEGER";
        string stepsCapabilityRealName = "steps";
        string stepsCapabilityParent = "physiological";
        string stepsCapabilityFullName = "physiological-steps";
        string stepsCapabilityDesc = "Check the daily steps";
        ObjectOrServiceCapability stepsCapability = new ObjectOrServiceCapability(stepsCapabilityReferenceObjectID, stepsCapabilityName, stepsCapabilityType, stepsCapabilityDataType, stepsCapabilityRealName, stepsCapabilityParent, stepsCapabilityFullName, stepsCapabilityDesc);
        trainingCapabilitiesList.Add(stepsCapability);
        singleObjectOrServiceCapability.Add(stepsCapabilityFullName, stepsCapability);
        
        int cognitiveCapabilityReferenceObjectID = 23;
        string cognitiveCapabilityName = "training time";
        string cognitiveCapabilityType = "t";
        string cognitiveCapabilityDataType = "INTEGER";
        string cognitiveCapabilityRealName = "trainingtime";
        string cognitiveCapabilityParent = "cognitive";
        string cognitiveCapabilityFullName = "cognitive-trainingtime";
        string cognitiveCapabilityDesc = "Check the training time";
        ObjectOrServiceCapability cognitiveCapability = new ObjectOrServiceCapability(cognitiveCapabilityReferenceObjectID, cognitiveCapabilityName, cognitiveCapabilityType, cognitiveCapabilityDataType, cognitiveCapabilityRealName, cognitiveCapabilityParent, cognitiveCapabilityFullName, cognitiveCapabilityDesc);
        trainingCapabilitiesList.Add(cognitiveCapability);
        singleObjectOrServiceCapability.Add(cognitiveCapabilityFullName, cognitiveCapability);
       
        serviceCapabilities.Add(stepsCapability.objectReferenceId, trainingCapabilitiesList);
        ScreenLog.Log("TRAINING SERIVE ADDED");
    }

    /**
     * Here we define the various Capabilities of the objects
     * CAREFUL: before the reference objects were identified with the real name, now we are using the ID!!
     */
    public void initiateObjectCapabilities()
    {
        List<ObjectOrServiceCapability> livingroomHueLampCapabilitiesList = new List<ObjectOrServiceCapability>();
        int livingroomHueLampFirstCapabilityReferenceObjectID = 5;
        string livingroomHueLampFirstCapabilityName = "Living Room Light State";
        string livingroomHueLampFirstCapabilityType = "t";
        string livingroomHueLampFirstCapabilityDataType = "ENUM";
        string livingroomHueLampFirstCapabilityRealName = "state";
        string livingroomHueLampFirstCapabilityParent = "livingroom";
        string livingroomHueLampFirstCapabilityFullName = "livingroomlight-state";
        string livingroomHueLampFirstCapabilityDesc = "Check the Living Room Hue Lamp state (Trigger)";
        ObjectOrServiceCapability livingroomHueLampFirstCapability = new ObjectOrServiceCapability(livingroomHueLampFirstCapabilityReferenceObjectID, livingroomHueLampFirstCapabilityName, livingroomHueLampFirstCapabilityType, livingroomHueLampFirstCapabilityDataType, livingroomHueLampFirstCapabilityRealName, livingroomHueLampFirstCapabilityParent, livingroomHueLampFirstCapabilityFullName, livingroomHueLampFirstCapabilityDesc);
        livingroomHueLampCapabilitiesList.Add(livingroomHueLampFirstCapability);
        singleObjectOrServiceCapability.Add(livingroomHueLampFirstCapabilityFullName, livingroomHueLampFirstCapability);
        
        int livingroomHueLampSecondCapabilityReferenceObjectID = 5;
        string livingroomHueLampSecondCapabilityName = "Hue Color Light Living Room";
        string livingroomHueLampSecondCapabilityType = "a";
        string livingroomHueLampSecondCapabilityDataType = "ENUM";
        string livingroomHueLampSecondCapabilityRealName = "hue color light living room";
        string livingroomHueLampSecondCapabilityParent = "livingroom";
        string livingroomHueLampSecondCapabilityFullName = "livingroom-hue color light living room";
        string livingroomHueLampSecondCapabilityDesc = "Change the Living Room Hue Lamp Color (Action)";
        ObjectOrServiceCapability livingroomHueLampSecondCapability = new ObjectOrServiceCapability(livingroomHueLampSecondCapabilityReferenceObjectID, livingroomHueLampSecondCapabilityName, livingroomHueLampSecondCapabilityType, livingroomHueLampSecondCapabilityDataType, livingroomHueLampSecondCapabilityRealName, livingroomHueLampSecondCapabilityParent, livingroomHueLampSecondCapabilityFullName, livingroomHueLampSecondCapabilityDesc);
        livingroomHueLampCapabilitiesList.Add(livingroomHueLampSecondCapability);
        singleObjectOrServiceCapability.Add(livingroomHueLampSecondCapabilityFullName, livingroomHueLampSecondCapability);
        
        int livingroomHueLampThirdCapabilityReferenceObjectID = 5;
        string livingroomHueLampThirdCapabilityName = "Turn the living room light";
        string livingroomHueLampThirdCapabilityType = "a";
        string livingroomHueLampThirdCapabilityDataType = "ENUM";
        string livingroomHueLampThirdCapabilityRealName = "alllight";
        string livingroomHueLampThirdCapabilityParent = "livingroom";
        string livingroomHueLampThirdCapabilityFullName = "livingroom-alllight";
        string livingroomHueLampThirdCapabilityDesc = "Turn ON / OFF the Living room Hue Lamp (Action)";
        ObjectOrServiceCapability livingroomHueLampThirdCapability = new ObjectOrServiceCapability(livingroomHueLampThirdCapabilityReferenceObjectID, livingroomHueLampThirdCapabilityName, livingroomHueLampThirdCapabilityType, livingroomHueLampThirdCapabilityDataType, livingroomHueLampThirdCapabilityRealName, livingroomHueLampThirdCapabilityParent, livingroomHueLampThirdCapabilityFullName, livingroomHueLampThirdCapabilityDesc);
        livingroomHueLampCapabilitiesList.Add(livingroomHueLampThirdCapability);
        singleObjectOrServiceCapability.Add(livingroomHueLampThirdCapabilityFullName, livingroomHueLampThirdCapability);
        
        objectsCapabilities.Add(livingroomHueLampFirstCapability.objectReferenceId, livingroomHueLampCapabilitiesList);
        
        
        
        List<ObjectOrServiceCapability> entranceDoorWindowSensorCapabilitiesList = new List<ObjectOrServiceCapability>();

        int entranceDoorWindowSensorFirstCapabilityReferenceObjectID = 14; // 
        string entranceDoorWindowSensorFirstCapabilityName = "Entrance Door";
        string entranceDoorWindowSensorFirstCapabilityType = "t";
        string entranceDoorWindowSensorFirstCapabilityDataType = "ENUM";
        string entranceDoorWindowSensorFirstCapabilityRealName = "doorsensor";
        string entranceDoorWindowSensorFirstCapabilityParent = "entrance";
        string entranceDoorWindowSensorFirstCapabilityFullName = "entrance-doorsensor";
        string entranceDoorWindowSensorFirstCapabilityDesc = "Check the state of the entrance door (Trigger)";
        ObjectOrServiceCapability entranceDoorWindowSensorFirstCapability = new ObjectOrServiceCapability(entranceDoorWindowSensorFirstCapabilityReferenceObjectID, entranceDoorWindowSensorFirstCapabilityName, entranceDoorWindowSensorFirstCapabilityType, entranceDoorWindowSensorFirstCapabilityDataType, entranceDoorWindowSensorFirstCapabilityRealName, entranceDoorWindowSensorFirstCapabilityParent, entranceDoorWindowSensorFirstCapabilityFullName, entranceDoorWindowSensorFirstCapabilityDesc);
        entranceDoorWindowSensorCapabilitiesList.Add(entranceDoorWindowSensorFirstCapability);
        singleObjectOrServiceCapability.Add(entranceDoorWindowSensorFirstCapabilityFullName, entranceDoorWindowSensorFirstCapability);
        
        int entranceDoorWindowSensorSecondCapabilityReferenceObjectID = 14; // 
        string entranceDoorWindowSensorSecondCapabilityName = "Entrance Door";
        string entranceDoorWindowSensorSecondCapabilityType = "a";
        string entranceDoorWindowSensorSecondCapabilityDataType = "ENUM";
        string entranceDoorWindowSensorSecondCapabilityRealName = "door";
        string entranceDoorWindowSensorSecondCapabilityParent = "entrance";
        string entranceDoorWindowSensorSecondCapabilityFullName = "entrance-door";
        string entranceDoorWindowSensorSecondCapabilityDesc = "Open / close the entrance door (Action)";
        ObjectOrServiceCapability entranceDoorWindowSensorSecondCapability = new ObjectOrServiceCapability(entranceDoorWindowSensorSecondCapabilityReferenceObjectID, entranceDoorWindowSensorSecondCapabilityName, entranceDoorWindowSensorSecondCapabilityType, entranceDoorWindowSensorSecondCapabilityDataType, entranceDoorWindowSensorSecondCapabilityRealName, entranceDoorWindowSensorSecondCapabilityParent, entranceDoorWindowSensorSecondCapabilityFullName, entranceDoorWindowSensorSecondCapabilityDesc);
        entranceDoorWindowSensorCapabilitiesList.Add(entranceDoorWindowSensorSecondCapability);
        singleObjectOrServiceCapability.Add(entranceDoorWindowSensorSecondCapabilityFullName, entranceDoorWindowSensorSecondCapability);
        
        objectsCapabilities.Add(entranceDoorWindowSensorSecondCapability.objectReferenceId, entranceDoorWindowSensorCapabilitiesList);

        /*
        List<ObjectOrServiceCapability> fridgeCapabilitiesList = new List<ObjectOrServiceCapability>();
        int fridgeFirstCapabilityReferenceObjectID = 1;
        string fridgeFirstCapabilityName = "Fridge Door";
        string fridgeFirstCapabilityType = "t";
        string fridgeFirstCapabilityDataType = "ENUM";
        string fridgeFirstCapabilityRealName = "door";
        string fridgeFirstCapabilityParent = "fridge";
        string fridgeFirstCapabilityFullName = "fridge-door";
        string fridgeFirstCapabilityDesc = "Check the fridge door (Trigger)";
        ObjectOrServiceCapability fridgeFirstCapability = new ObjectOrServiceCapability(fridgeFirstCapabilityReferenceObjectID, fridgeFirstCapabilityName, fridgeFirstCapabilityType, fridgeFirstCapabilityDataType, fridgeFirstCapabilityRealName, fridgeFirstCapabilityParent, fridgeFirstCapabilityFullName, fridgeFirstCapabilityDesc);
        fridgeCapabilitiesList.Add(fridgeFirstCapability);
        singleObjectOrServiceCapability.Add(fridgeFirstCapabilityFullName, fridgeFirstCapability);
        
        // objectsCapabilities.Add(fridgeFirstCapability.objectReferenceId, fridgeCapabilitiesList); Lets try without fridge
        
        List<ObjectOrServiceCapability> microwaveCapabilitiesList = new List<ObjectOrServiceCapability>();
        int microwaveFirstCapabilityReferenceObjectID = 2;
        string microwaveFirstCapabilityName = "Microwave Door";
        string microwaveFirstCapabilityType = "t";
        string microwaveFirstCapabilityDataType = "ENUM";
        string microwaveFirstCapabilityRealName = "door";
        string microwaveFirstCapabilityParent = "microwave";
        string microwaveFirstCapabilityFullName = "microwave-door";
        string microwaveFirstCapabilityDesc = "Check the microwave door (Trigger)";
        ObjectOrServiceCapability microwaveFirstCapability = new ObjectOrServiceCapability(microwaveFirstCapabilityReferenceObjectID, microwaveFirstCapabilityName, microwaveFirstCapabilityType, microwaveFirstCapabilityDataType, microwaveFirstCapabilityRealName, microwaveFirstCapabilityParent, microwaveFirstCapabilityFullName, microwaveFirstCapabilityDesc);
        microwaveCapabilitiesList.Add(microwaveFirstCapability);
        singleObjectOrServiceCapability.Add(microwaveFirstCapabilityFullName, microwaveFirstCapability);
        
        objectsCapabilities.Add(microwaveFirstCapability.objectReferenceId, microwaveCapabilitiesList);
        
        List<ObjectOrServiceCapability> kitchenHueLampCapabilitiesList = new List<ObjectOrServiceCapability>();
        int kitchenHueLampFirstCapabilityReferenceObjectID = 3;
        string kitchenHueLampFirstCapabilityName = "Hue Color Light Kitchen";
        string kitchenHueLampFirstCapabilityType = "a";
        string kitchenHueLampFirstCapabilityDataType = "ENUM";
        string kitchenHueLampFirstCapabilityRealName = "hue color light kitchen";
        string kitchenHueLampFirstCapabilityParent = "kitchen";
        string kitchenHueLampFirstCapabilityFullName = "kitchen-hue color light kitchen";
        string kitchenHueLampFirstCapabilityDesc = "Change the Kitchen Hue Lamp Color (Action)";
        ObjectOrServiceCapability kitchenHueLampFirstCapability = new ObjectOrServiceCapability(kitchenHueLampFirstCapabilityReferenceObjectID, kitchenHueLampFirstCapabilityName, kitchenHueLampFirstCapabilityType, kitchenHueLampFirstCapabilityDataType, kitchenHueLampFirstCapabilityRealName, kitchenHueLampFirstCapabilityParent, kitchenHueLampFirstCapabilityFullName, kitchenHueLampFirstCapabilityDesc);
        kitchenHueLampCapabilitiesList.Add(kitchenHueLampFirstCapability);
        singleObjectOrServiceCapability.Add(kitchenHueLampFirstCapabilityFullName, kitchenHueLampFirstCapability);
       
        int kitchenHueLampSecondCapabilityReferenceObjectID = 3;
        string kitchenHueLampSecondCapabilityName = "Kitchen Light State";
        string kitchenHueLampSecondCapabilityType = "t";
        string kitchenHueLampSecondCapabilityDataType = "ENUM";
        string kitchenHueLampSecondCapabilityRealName = "state";
        string kitchenHueLampSecondCapabilityParent = "kitchenlight";
        string kitchenHueLampSecondCapabilityFullName = "kitchenlight-state";
        string kitchenHueLampSecondCapabilityDesc = "Check the state of the Kitchen Hue Lamp Color (Trigger)";
        ObjectOrServiceCapability kitchenHueLampSecondCapability = new ObjectOrServiceCapability(kitchenHueLampSecondCapabilityReferenceObjectID, kitchenHueLampSecondCapabilityName, kitchenHueLampSecondCapabilityType, kitchenHueLampSecondCapabilityDataType, kitchenHueLampSecondCapabilityRealName, kitchenHueLampSecondCapabilityParent, kitchenHueLampSecondCapabilityFullName, kitchenHueLampSecondCapabilityDesc);
        kitchenHueLampCapabilitiesList.Add(kitchenHueLampSecondCapability);
        singleObjectOrServiceCapability.Add(kitchenHueLampSecondCapabilityFullName, kitchenHueLampSecondCapability);
        
        int kitchenHueLampThirdCapabilityReferenceObjectID = 3;
        string kitchenHueLampThirdCapabilityName = "Turn the kitchen light";
        string kitchenHueLampThirdCapabilityType = "a";
        string kitchenHueLampThirdCapabilityDataType = "ENUM";
        string kitchenHueLampThirdCapabilityRealName = "alllight";
        string kitchenHueLampThirdCapabilityParent = "kitchen";
        string kitchenHueLampThirdCapabilityFullName = "kitchen-alllight";
        string kitchenHueLampThirdCapabilityDesc = "Turn ON / OFF the Kitchen Hue Lamp (Action)";
        ObjectOrServiceCapability kitchenHueLampThirdCapability = new ObjectOrServiceCapability(kitchenHueLampThirdCapabilityReferenceObjectID, kitchenHueLampThirdCapabilityName, kitchenHueLampThirdCapabilityType, kitchenHueLampThirdCapabilityDataType, kitchenHueLampThirdCapabilityRealName, kitchenHueLampThirdCapabilityParent, kitchenHueLampThirdCapabilityFullName, kitchenHueLampThirdCapabilityDesc);
        kitchenHueLampCapabilitiesList.Add(kitchenHueLampThirdCapability);
        singleObjectOrServiceCapability.Add(kitchenHueLampThirdCapabilityFullName, kitchenHueLampThirdCapability);
        
        objectsCapabilities.Add(kitchenHueLampFirstCapability.objectReferenceId, kitchenHueLampCapabilitiesList);
        
        List<ObjectOrServiceCapability> kitchenGasSmokeSensorCapabilitiesList = new List<ObjectOrServiceCapability>();
        int kitchenGasSmokeSensorFirstCapabilityReferenceObjectID = 4;
        string kitchenGasSmokeSensorFirstCapabilityName = "Gas Sensor";
        string kitchenGasSmokeSensorFirstCapabilityType = "t";
        string kitchenGasSmokeSensorFirstCapabilityDataType = "BOOLEAN";
        string kitchenGasSmokeSensorFirstCapabilityRealName = "gassensor";
        string kitchenGasSmokeSensorFirstCapabilityParent = "kitchen";
        string kitchenGasSmokeSensorFirstCapabilityFullName = "kitchen-gassensor";
        string kitchenGasSmokeSensorFirstCapabilityDesc = "Check if the Kitchen Gas Sensor is active (Trigger)";
        ObjectOrServiceCapability kitchenGasSmokeSensorFirstCapability = new ObjectOrServiceCapability(kitchenGasSmokeSensorFirstCapabilityReferenceObjectID, kitchenGasSmokeSensorFirstCapabilityName, kitchenGasSmokeSensorFirstCapabilityType, kitchenGasSmokeSensorFirstCapabilityDataType, kitchenGasSmokeSensorFirstCapabilityRealName, kitchenGasSmokeSensorFirstCapabilityParent, kitchenGasSmokeSensorFirstCapabilityFullName, kitchenGasSmokeSensorFirstCapabilityDesc);
        kitchenGasSmokeSensorCapabilitiesList.Add(kitchenGasSmokeSensorFirstCapability);
        singleObjectOrServiceCapability.Add(kitchenGasSmokeSensorFirstCapabilityFullName, kitchenGasSmokeSensorFirstCapability);
        
        int kitchenGasSmokeSensorSecondCapabilityReferenceObjectID = 4;
        string kitchenGasSmokeSensorSecondCapabilityName = "Smoke Sensor";
        string kitchenGasSmokeSensorSecondCapabilityType = "t";
        string kitchenGasSmokeSensorSecondCapabilityDataType = "BOOLEAN";
        string kitchenGasSmokeSensorSecondCapabilityRealName = "smokesensor";
        string kitchenGasSmokeSensorSecondCapabilityParent = "kitchen";
        string kitchenGasSmokeSensorSecondCapabilityFullName = "kitchen-smokesensor";
        string kitchenGasSmokeSensorSecondCapabilityDesc = "Check if the Kitchen Smoke Sensor is active (Trigger)";
        ObjectOrServiceCapability kitchenGasSmokeSensorSecondCapability = new ObjectOrServiceCapability(kitchenGasSmokeSensorSecondCapabilityReferenceObjectID, kitchenGasSmokeSensorSecondCapabilityName, kitchenGasSmokeSensorSecondCapabilityType, kitchenGasSmokeSensorSecondCapabilityDataType, kitchenGasSmokeSensorSecondCapabilityRealName, kitchenGasSmokeSensorSecondCapabilityParent, kitchenGasSmokeSensorSecondCapabilityFullName, kitchenGasSmokeSensorSecondCapabilityDesc);
        kitchenGasSmokeSensorCapabilitiesList.Add(kitchenGasSmokeSensorSecondCapability);
        singleObjectOrServiceCapability.Add(kitchenGasSmokeSensorSecondCapabilityFullName, kitchenGasSmokeSensorSecondCapability);
        
        objectsCapabilities.Add(kitchenGasSmokeSensorFirstCapability.objectReferenceId, kitchenGasSmokeSensorCapabilitiesList);
        
        List<ObjectOrServiceCapability> livingroomHueLampCapabilitiesList = new List<ObjectOrServiceCapability>();
        int livingroomHueLampFirstCapabilityReferenceObjectID = 5;
        string livingroomHueLampFirstCapabilityName = "Living Room Light State";
        string livingroomHueLampFirstCapabilityType = "t";
        string livingroomHueLampFirstCapabilityDataType = "ENUM";
        string livingroomHueLampFirstCapabilityRealName = "state";
        string livingroomHueLampFirstCapabilityParent = "livingroom";
        string livingroomHueLampFirstCapabilityFullName = "livingroomlight-state";
        string livingroomHueLampFirstCapabilityDesc = "Check the Living Room Hue Lamp state (Trigger)";
        ObjectOrServiceCapability livingroomHueLampFirstCapability = new ObjectOrServiceCapability(livingroomHueLampFirstCapabilityReferenceObjectID, livingroomHueLampFirstCapabilityName, livingroomHueLampFirstCapabilityType, livingroomHueLampFirstCapabilityDataType, livingroomHueLampFirstCapabilityRealName, livingroomHueLampFirstCapabilityParent, livingroomHueLampFirstCapabilityFullName, livingroomHueLampFirstCapabilityDesc);
        livingroomHueLampCapabilitiesList.Add(livingroomHueLampFirstCapability);
        singleObjectOrServiceCapability.Add(livingroomHueLampFirstCapabilityFullName, livingroomHueLampFirstCapability);
        
        int livingroomHueLampSecondCapabilityReferenceObjectID = 5;
        string livingroomHueLampSecondCapabilityName = "Hue Color Light Living Room";
        string livingroomHueLampSecondCapabilityType = "a";
        string livingroomHueLampSecondCapabilityDataType = "ENUM";
        string livingroomHueLampSecondCapabilityRealName = "hue color light living room";
        string livingroomHueLampSecondCapabilityParent = "livingroom";
        string livingroomHueLampSecondCapabilityFullName = "livingroom-hue color light living room";
        string livingroomHueLampSecondCapabilityDesc = "Change the Living Room Hue Lamp Color (Action)";
        ObjectOrServiceCapability livingroomHueLampSecondCapability = new ObjectOrServiceCapability(livingroomHueLampSecondCapabilityReferenceObjectID, livingroomHueLampSecondCapabilityName, livingroomHueLampSecondCapabilityType, livingroomHueLampSecondCapabilityDataType, livingroomHueLampSecondCapabilityRealName, livingroomHueLampSecondCapabilityParent, livingroomHueLampSecondCapabilityFullName, livingroomHueLampSecondCapabilityDesc);
        livingroomHueLampCapabilitiesList.Add(livingroomHueLampSecondCapability);
        singleObjectOrServiceCapability.Add(livingroomHueLampSecondCapabilityFullName, livingroomHueLampSecondCapability);
        
        int livingroomHueLampThirdCapabilityReferenceObjectID = 5;
        string livingroomHueLampThirdCapabilityName = "Turn the living room light";
        string livingroomHueLampThirdCapabilityType = "a";
        string livingroomHueLampThirdCapabilityDataType = "ENUM";
        string livingroomHueLampThirdCapabilityRealName = "alllight";
        string livingroomHueLampThirdCapabilityParent = "livingroom";
        string livingroomHueLampThirdCapabilityFullName = "livingroom-alllight";
        string livingroomHueLampThirdCapabilityDesc = "Turn ON / OFF the Living room Hue Lamp (Action)";
        ObjectOrServiceCapability livingroomHueLampThirdCapability = new ObjectOrServiceCapability(livingroomHueLampThirdCapabilityReferenceObjectID, livingroomHueLampThirdCapabilityName, livingroomHueLampThirdCapabilityType, livingroomHueLampThirdCapabilityDataType, livingroomHueLampThirdCapabilityRealName, livingroomHueLampThirdCapabilityParent, livingroomHueLampThirdCapabilityFullName, livingroomHueLampThirdCapabilityDesc);
        livingroomHueLampCapabilitiesList.Add(livingroomHueLampThirdCapability);
        singleObjectOrServiceCapability.Add(livingroomHueLampThirdCapabilityFullName, livingroomHueLampThirdCapability);
        
        objectsCapabilities.Add(livingroomHueLampFirstCapability.objectReferenceId, livingroomHueLampCapabilitiesList);

        List<ObjectOrServiceCapability> kitchenHueSensorCapabilitiesList = new List<ObjectOrServiceCapability>();

        int kitchenHueSensorFirstCapabilityReferenceObjectID = 10;
        string kitchenHueSensorFirstCapabilityName = "Temperature Level Kitchen";
        string kitchenHueSensorFirstCapabilityType = "t";
        string kitchenHueSensorFirstCapabilityDataType = "INTEGER";
        string kitchenHueSensorFirstCapabilityRealName = "temperaturelevel";
        string kitchenHueSensorFirstCapabilityParent = "kitchen";
        string kitchenHueSensorFirstCapabilityFullName = "kitchen-temperaturelevel";
        string kitchenHueSensorFirstCapabilityDesc = "Check the temperature of the kitchen (Trigger)";
        ObjectOrServiceCapability kitchenHueSensorFirstCapability = new ObjectOrServiceCapability(kitchenHueSensorFirstCapabilityReferenceObjectID, kitchenHueSensorFirstCapabilityName, kitchenHueSensorFirstCapabilityType, kitchenHueSensorFirstCapabilityDataType, kitchenHueSensorFirstCapabilityRealName, kitchenHueSensorFirstCapabilityParent, kitchenHueSensorFirstCapabilityFullName, kitchenHueSensorFirstCapabilityDesc);
        kitchenHueSensorCapabilitiesList.Add(kitchenHueSensorFirstCapability);
        singleObjectOrServiceCapability.Add(kitchenHueSensorFirstCapabilityFullName, kitchenHueSensorFirstCapability);
        
        int kitchenHueSensorSecondCapabilityReferenceObjectID = 10;
        string kitchenHueSensorSecondCapabilityName = "Humidity Level Kitchen";
        string kitchenHueSensorSecondCapabilityType = "t";
        string kitchenHueSensorSecondCapabilityDataType = "INTEGER";
        string kitchenHueSensorSecondCapabilityRealName = "humiditylevel";
        string kitchenHueSensorSecondCapabilityParent = "kitchen";
        string kitchenHueSensorSecondCapabilityFullName = "kitchen-humiditylevel";
        string kitchenHueSensorSecondCapabilityDesc = "Check the humidity of the kitchen (Trigger)";
        ObjectOrServiceCapability kitchenHueSensorSecondCapability = new ObjectOrServiceCapability(kitchenHueSensorSecondCapabilityReferenceObjectID, kitchenHueSensorSecondCapabilityName, kitchenHueSensorSecondCapabilityType, kitchenHueSensorSecondCapabilityDataType, kitchenHueSensorSecondCapabilityRealName, kitchenHueSensorSecondCapabilityParent, kitchenHueSensorSecondCapabilityFullName, kitchenHueSensorSecondCapabilityDesc);
        kitchenHueSensorCapabilitiesList.Add(kitchenHueSensorSecondCapability);
        singleObjectOrServiceCapability.Add(kitchenHueSensorSecondCapabilityFullName, kitchenHueSensorSecondCapability);
        
        int kitchenHueSensorThirdCapabilityReferenceObjectID = 10;
        string kitchenHueSensorThirdCapabilityName = "Light Level Kitchen";
        string kitchenHueSensorThirdCapabilityType = "t";
        string kitchenHueSensorThirdCapabilityDataType = "ENUM";
        string kitchenHueSensorThirdCapabilityRealName = "lightlevel";
        string kitchenHueSensorThirdCapabilityParent = "kitchen";
        string kitchenHueSensorThirdCapabilityFullName = "kitchen-lightlevel";
        string kitchenHueSensorThirdCapabilityDesc = "Check the light level of the kitchen (Trigger)";
        ObjectOrServiceCapability kitchenHueSensorThirdCapability = new ObjectOrServiceCapability(kitchenHueSensorThirdCapabilityReferenceObjectID, kitchenHueSensorThirdCapabilityName, kitchenHueSensorThirdCapabilityType, kitchenHueSensorThirdCapabilityDataType, kitchenHueSensorThirdCapabilityRealName, kitchenHueSensorThirdCapabilityParent, kitchenHueSensorThirdCapabilityFullName, kitchenHueSensorThirdCapabilityDesc);
        kitchenHueSensorCapabilitiesList.Add(kitchenHueSensorThirdCapability);
        singleObjectOrServiceCapability.Add(kitchenHueSensorThirdCapabilityFullName, kitchenHueSensorThirdCapability);
        
        int kitchenHueSensorFourthCapabilityReferenceObjectID = 10;
        string kitchenHueSensorFourthCapabilityName = "Kitchen motion";
        string kitchenHueSensorFourthCapabilityType = "t";
        string kitchenHueSensorFourthCapabilityDataType = "BOOLEAN";
        string kitchenHueSensorFourthCapabilityRealName = "motion";
        string kitchenHueSensorFourthCapabilityParent = "kitchen";
        string kitchenHueSensorFourthCapabilityFullName = "kitchen-motion";
        string kitchenHueSensorFourthCapabilityDesc = "Check if there is motion in the kitchen (Trigger)";
        ObjectOrServiceCapability kitchenHueSensorFourthCapability = new ObjectOrServiceCapability(kitchenHueSensorFourthCapabilityReferenceObjectID, kitchenHueSensorFourthCapabilityName, kitchenHueSensorFourthCapabilityType, kitchenHueSensorFourthCapabilityDataType, kitchenHueSensorFourthCapabilityRealName, kitchenHueSensorFourthCapabilityParent, kitchenHueSensorFourthCapabilityFullName, kitchenHueSensorFourthCapabilityDesc);
        kitchenHueSensorCapabilitiesList.Add(kitchenHueSensorFourthCapability);
        singleObjectOrServiceCapability.Add(kitchenHueSensorFourthCapabilityFullName, kitchenHueSensorFourthCapability);
        
        objectsCapabilities.Add(kitchenHueSensorFirstCapability.objectReferenceId, kitchenHueSensorCapabilitiesList);
        
        List<ObjectOrServiceCapability> livingroomHueSensorCapabilitiesList = new List<ObjectOrServiceCapability>();

        int livingroomHueSensorFirstCapabilityReferenceObjectID = 11;
        string livingroomHueSensorFirstCapabilityName = "Temperature Level Living Room";
        string livingroomHueSensorFirstCapabilityType = "t";
        string livingroomHueSensorFirstCapabilityDataType = "INTEGER";
        string livingroomHueSensorFirstCapabilityRealName = "temperaturelevel";
        string livingroomHueSensorFirstCapabilityParent = "livingroom";
        string livingroomHueSensorFirstCapabilityFullName = "livingroom-temperaturelevel";
        string livingroomHueSensorFirstCapabilityDesc = "Check the temperature of the living room (Trigger)";
        ObjectOrServiceCapability livingroomHueSensorFirstCapability = new ObjectOrServiceCapability(livingroomHueSensorFirstCapabilityReferenceObjectID, livingroomHueSensorFirstCapabilityName, livingroomHueSensorFirstCapabilityType, livingroomHueSensorFirstCapabilityDataType, livingroomHueSensorFirstCapabilityRealName, livingroomHueSensorFirstCapabilityParent, livingroomHueSensorFirstCapabilityFullName, livingroomHueSensorFirstCapabilityDesc);
        livingroomHueSensorCapabilitiesList.Add(livingroomHueSensorFirstCapability);
        singleObjectOrServiceCapability.Add(livingroomHueSensorFirstCapabilityFullName, livingroomHueSensorFirstCapability);
        
        int livingroomHueSensorSecondCapabilityReferenceObjectID = 11;
        string livingroomHueSensorSecondCapabilityName = "Humidity Level Living Room";
        string livingroomHueSensorSecondCapabilityType = "t";
        string livingroomHueSensorSecondCapabilityDataType = "INTEGER";
        string livingroomHueSensorSecondCapabilityRealName = "humiditylevel";
        string livingroomHueSensorSecondCapabilityParent = "livingroom";
        string livingroomHueSensorSecondCapabilityFullName = "livingroom-humiditylevel";
        string livingroomHueSensorSecondCapabilityDesc = "Check the humidity of the living room (Trigger)";
        ObjectOrServiceCapability livingroomHueSensorSecondCapability = new ObjectOrServiceCapability(livingroomHueSensorSecondCapabilityReferenceObjectID, livingroomHueSensorSecondCapabilityName, livingroomHueSensorSecondCapabilityType, livingroomHueSensorSecondCapabilityDataType, livingroomHueSensorSecondCapabilityRealName, livingroomHueSensorSecondCapabilityParent, livingroomHueSensorSecondCapabilityFullName, livingroomHueSensorSecondCapabilityDesc);
        livingroomHueSensorCapabilitiesList.Add(livingroomHueSensorSecondCapability);
        singleObjectOrServiceCapability.Add(livingroomHueSensorSecondCapabilityFullName, livingroomHueSensorSecondCapability);
        
        int livingroomHueSensorThirdCapabilityReferenceObjectID = 11; // 
        string livingroomHueSensorThirdCapabilityName = "Light Level Living Room";
        string livingroomHueSensorThirdCapabilityType = "t";
        string livingroomHueSensorThirdCapabilityDataType = "ENUM";
        string livingroomHueSensorThirdCapabilityRealName = "lightlevel";
        string livingroomHueSensorThirdCapabilityParent = "livingroom";
        string livingroomHueSensorThirdCapabilityFullName = "livingroom-lightlevel";
        string livingroomHueSensorThirdCapabilityDesc = "Check the light level of the living room (Trigger)";
        ObjectOrServiceCapability livingroomHueSensorThirdCapability = new ObjectOrServiceCapability(livingroomHueSensorThirdCapabilityReferenceObjectID, livingroomHueSensorThirdCapabilityName, livingroomHueSensorThirdCapabilityType, livingroomHueSensorThirdCapabilityDataType, livingroomHueSensorThirdCapabilityRealName, livingroomHueSensorThirdCapabilityParent, livingroomHueSensorThirdCapabilityFullName, livingroomHueSensorThirdCapabilityDesc);
        livingroomHueSensorCapabilitiesList.Add(livingroomHueSensorThirdCapability);
        singleObjectOrServiceCapability.Add(livingroomHueSensorThirdCapabilityFullName, livingroomHueSensorThirdCapability);
        
        int livingroomHueSensorFourthCapabilityReferenceObjectID = 11; // 
        string livingroomHueSensorFourthCapabilityName = "Living Room motion";
        string livingroomHueSensorFourthCapabilityType = "t";
        string livingroomHueSensorFourthCapabilityDataType = "BOOLEAN";
        string livingroomHueSensorFourthCapabilityRealName = "motion";
        string livingroomHueSensorFourthCapabilityParent = "livingroom";
        string livingroomHueSensorFourthCapabilityFullName = "livingroom-motion";
        string livingroomHueSensorFourthCapabilityDesc = "Check if there is motion in the living room (Trigger)";
        ObjectOrServiceCapability livingroomHueSensorFourthCapability = new ObjectOrServiceCapability(livingroomHueSensorFourthCapabilityReferenceObjectID, livingroomHueSensorFourthCapabilityName, livingroomHueSensorFourthCapabilityType, livingroomHueSensorFourthCapabilityDataType, livingroomHueSensorFourthCapabilityRealName, livingroomHueSensorFourthCapabilityParent, livingroomHueSensorFourthCapabilityFullName, livingroomHueSensorFourthCapabilityDesc);
        livingroomHueSensorCapabilitiesList.Add(livingroomHueSensorFourthCapability);
        singleObjectOrServiceCapability.Add(livingroomHueSensorFourthCapabilityFullName, livingroomHueSensorFourthCapability);
        
        objectsCapabilities.Add(livingroomHueSensorFirstCapability.objectReferenceId, livingroomHueSensorCapabilitiesList);
        
        
        
        List<ObjectOrServiceCapability> entranceHueLampCapabilitiesList = new List<ObjectOrServiceCapability>();
        int entranceHueLampFirstCapabilityReferenceObjectID = 12;
        string entranceHueLampFirstCapabilityName = "Entrance Light State";
        string entranceHueLampFirstCapabilityType = "t";
        string entranceHueLampFirstCapabilityDataType = "ENUM";
        string entranceHueLampFirstCapabilityRealName = "state";
        string entranceHueLampFirstCapabilityParent = "entrance";
        string entranceHueLampFirstCapabilityFullName = "entrancelight-state";
        string entranceHueLampFirstCapabilityDesc = "Check the Entrance Hue Lamp state (Trigger)";
        ObjectOrServiceCapability entranceHueLampFirstCapability = new ObjectOrServiceCapability(entranceHueLampFirstCapabilityReferenceObjectID, entranceHueLampFirstCapabilityName, entranceHueLampFirstCapabilityType, entranceHueLampFirstCapabilityDataType, entranceHueLampFirstCapabilityRealName, entranceHueLampFirstCapabilityParent, entranceHueLampFirstCapabilityFullName, entranceHueLampFirstCapabilityDesc);
        entranceHueLampCapabilitiesList.Add(entranceHueLampFirstCapability);
        singleObjectOrServiceCapability.Add(entranceHueLampFirstCapabilityFullName, entranceHueLampFirstCapability);
        
        int entranceHueLampSecondCapabilityReferenceObjectID = 12;
        string entranceHueLampSecondCapabilityName = "Hue Color Light Entrance";
        string entranceHueLampSecondCapabilityType = "a";
        string entranceHueLampSecondCapabilityDataType = "ENUM";
        string entranceHueLampSecondCapabilityRealName = "hue color light entrance";
        string entranceHueLampSecondCapabilityParent = "entrance";
        string entranceHueLampSecondCapabilityFullName = "entrance-hue color light entrance";
        string entranceHueLampSecondCapabilityDesc = "Change the Entrance Hue Lamp Color (Action)";
        ObjectOrServiceCapability entranceHueLampSecondCapability = new ObjectOrServiceCapability(entranceHueLampSecondCapabilityReferenceObjectID, entranceHueLampSecondCapabilityName, entranceHueLampSecondCapabilityType, entranceHueLampSecondCapabilityDataType, entranceHueLampSecondCapabilityRealName, entranceHueLampSecondCapabilityParent, entranceHueLampSecondCapabilityFullName, entranceHueLampSecondCapabilityDesc);
        entranceHueLampCapabilitiesList.Add(entranceHueLampSecondCapability);
        singleObjectOrServiceCapability.Add(entranceHueLampSecondCapabilityFullName, entranceHueLampSecondCapability);
        
        int entranceHueLampThirdCapabilityReferenceObjectID = 12;
        string entranceHueLampThirdCapabilityName = "Turn the entrance light";
        string entranceHueLampThirdCapabilityType = "a";
        string entranceHueLampThirdCapabilityDataType = "ENUM";
        string entranceHueLampThirdCapabilityRealName = "alllight";
        string entranceHueLampThirdCapabilityParent = "entrance";
        string entranceHueLampThirdCapabilityFullName = "entrance-alllight";
        string entranceHueLampThirdCapabilityDesc = "Turn ON / OFF the Entrance Hue Lamp (Action)";
        ObjectOrServiceCapability entranceHueLampThirdCapability = new ObjectOrServiceCapability(entranceHueLampThirdCapabilityReferenceObjectID, entranceHueLampThirdCapabilityName, entranceHueLampThirdCapabilityType, entranceHueLampThirdCapabilityDataType, entranceHueLampThirdCapabilityRealName, entranceHueLampThirdCapabilityParent, entranceHueLampThirdCapabilityFullName, entranceHueLampThirdCapabilityDesc);
        entranceHueLampCapabilitiesList.Add(entranceHueLampThirdCapability);
        singleObjectOrServiceCapability.Add(entranceHueLampThirdCapabilityFullName, entranceHueLampThirdCapability);
        
        objectsCapabilities.Add(entranceHueLampFirstCapability.objectReferenceId, entranceHueLampCapabilitiesList);




        
        List<ObjectOrServiceCapability> entranceHueSensorCapabilitiesList = new List<ObjectOrServiceCapability>();

        int entranceHueSensorFirstCapabilityReferenceObjectID = 13;
        string entranceHueSensorFirstCapabilityName = "Temperature Level Entrance";
        string entranceHueSensorFirstCapabilityType = "t";
        string entranceHueSensorFirstCapabilityDataType = "INTEGER";
        string entranceHueSensorFirstCapabilityRealName = "temperaturelevel";
        string entranceHueSensorFirstCapabilityParent = "entrance";
        string entranceHueSensorFirstCapabilityFullName = "entrance-temperaturelevel";
        string entranceHueSensorFirstCapabilityDesc = "Check the temperature of the entrance (Trigger)";
        ObjectOrServiceCapability entranceHueSensorFirstCapability = new ObjectOrServiceCapability(entranceHueSensorFirstCapabilityReferenceObjectID, entranceHueSensorFirstCapabilityName, entranceHueSensorFirstCapabilityType, entranceHueSensorFirstCapabilityDataType, entranceHueSensorFirstCapabilityRealName, entranceHueSensorFirstCapabilityParent, entranceHueSensorFirstCapabilityFullName, entranceHueSensorFirstCapabilityDesc);
        entranceHueSensorCapabilitiesList.Add(entranceHueSensorFirstCapability);
        singleObjectOrServiceCapability.Add(entranceHueSensorFirstCapabilityFullName, entranceHueSensorFirstCapability);
        
        int entranceHueSensorSecondCapabilityReferenceObjectID = 13;
        string entranceHueSensorSecondCapabilityName = "Humidity Level Entrance";
        string entranceHueSensorSecondCapabilityType = "t";
        string entranceHueSensorSecondCapabilityDataType = "INTEGER";
        string entranceHueSensorSecondCapabilityRealName = "humiditylevel";
        string entranceHueSensorSecondCapabilityParent = "entrance";
        string entranceHueSensorSecondCapabilityFullName = "entrance-humiditylevel";
        string entranceHueSensorSecondCapabilityDesc = "Check the humidity of the entrance (Trigger)";
        ObjectOrServiceCapability entranceHueSensorSecondCapability = new ObjectOrServiceCapability(entranceHueSensorSecondCapabilityReferenceObjectID, entranceHueSensorSecondCapabilityName, entranceHueSensorSecondCapabilityType, entranceHueSensorSecondCapabilityDataType, entranceHueSensorSecondCapabilityRealName, entranceHueSensorSecondCapabilityParent, entranceHueSensorSecondCapabilityFullName, entranceHueSensorSecondCapabilityDesc);
        entranceHueSensorCapabilitiesList.Add(entranceHueSensorSecondCapability);
        singleObjectOrServiceCapability.Add(entranceHueSensorSecondCapabilityFullName, entranceHueSensorSecondCapability);
        
        int entranceHueSensorThirdCapabilityReferenceObjectID = 13; // 
        string entranceHueSensorThirdCapabilityName = "Light Level Entrance";
        string entranceHueSensorThirdCapabilityType = "t";
        string entranceHueSensorThirdCapabilityDataType = "ENUM";
        string entranceHueSensorThirdCapabilityRealName = "lightlevel";
        string entranceHueSensorThirdCapabilityParent = "entrance";
        string entranceHueSensorThirdCapabilityFullName = "entrance-lightlevel";
        string entranceHueSensorThirdCapabilityDesc = "Check the light level of the entrance (Trigger)";
        ObjectOrServiceCapability entranceHueSensorThirdCapability = new ObjectOrServiceCapability(entranceHueSensorThirdCapabilityReferenceObjectID, entranceHueSensorThirdCapabilityName, entranceHueSensorThirdCapabilityType, entranceHueSensorThirdCapabilityDataType, entranceHueSensorThirdCapabilityRealName, entranceHueSensorThirdCapabilityParent, entranceHueSensorThirdCapabilityFullName, entranceHueSensorThirdCapabilityDesc);
        entranceHueSensorCapabilitiesList.Add(entranceHueSensorThirdCapability);
        singleObjectOrServiceCapability.Add(entranceHueSensorThirdCapabilityFullName, entranceHueSensorThirdCapability);
        
        int entranceHueSensorFourthCapabilityReferenceObjectID = 13; // 
        string entranceHueSensorFourthCapabilityName = "Entrance Motion";
        string entranceHueSensorFourthCapabilityType = "t";
        string entranceHueSensorFourthCapabilityDataType = "BOOLEAN";
        string entranceHueSensorFourthCapabilityRealName = "motion";
        string entranceHueSensorFourthCapabilityParent = "entrance";
        string entranceHueSensorFourthCapabilityFullName = "entrance-motion";
        string entranceHueSensorFourthCapabilityDesc = "Check if there is motion on the entrance (Trigger)";
        ObjectOrServiceCapability entranceHueSensorFourthCapability = new ObjectOrServiceCapability(entranceHueSensorFourthCapabilityReferenceObjectID, entranceHueSensorFourthCapabilityName, entranceHueSensorFourthCapabilityType, entranceHueSensorFourthCapabilityDataType, entranceHueSensorFourthCapabilityRealName, entranceHueSensorFourthCapabilityParent, entranceHueSensorFourthCapabilityFullName, entranceHueSensorFourthCapabilityDesc);
        entranceHueSensorCapabilitiesList.Add(entranceHueSensorFourthCapability);
        singleObjectOrServiceCapability.Add(entranceHueSensorFourthCapabilityFullName, entranceHueSensorFourthCapability);
        
        objectsCapabilities.Add(entranceHueSensorFirstCapability.objectReferenceId, entranceHueSensorCapabilitiesList);



        
       // doorwindowsensors 
        List<ObjectOrServiceCapability> entranceDoorWindowSensorCapabilitiesList = new List<ObjectOrServiceCapability>();

        int entranceDoorWindowSensorFirstCapabilityReferenceObjectID = 14; // 
        string entranceDoorWindowSensorFirstCapabilityName = "Entrance Door";
        string entranceDoorWindowSensorFirstCapabilityType = "t";
        string entranceDoorWindowSensorFirstCapabilityDataType = "ENUM";
        string entranceDoorWindowSensorFirstCapabilityRealName = "doorsensor";
        string entranceDoorWindowSensorFirstCapabilityParent = "entrance";
        string entranceDoorWindowSensorFirstCapabilityFullName = "entrance-doorsensor";
        string entranceDoorWindowSensorFirstCapabilityDesc = "Check the state of the entrance door (Trigger)";
        ObjectOrServiceCapability entranceDoorWindowSensorFirstCapability = new ObjectOrServiceCapability(entranceDoorWindowSensorFirstCapabilityReferenceObjectID, entranceDoorWindowSensorFirstCapabilityName, entranceDoorWindowSensorFirstCapabilityType, entranceDoorWindowSensorFirstCapabilityDataType, entranceDoorWindowSensorFirstCapabilityRealName, entranceDoorWindowSensorFirstCapabilityParent, entranceDoorWindowSensorFirstCapabilityFullName, entranceDoorWindowSensorFirstCapabilityDesc);
        entranceDoorWindowSensorCapabilitiesList.Add(entranceDoorWindowSensorFirstCapability);
        singleObjectOrServiceCapability.Add(entranceDoorWindowSensorFirstCapabilityFullName, entranceDoorWindowSensorFirstCapability);
        
        int entranceDoorWindowSensorSecondCapabilityReferenceObjectID = 14; // 
        string entranceDoorWindowSensorSecondCapabilityName = "Entrance Door";
        string entranceDoorWindowSensorSecondCapabilityType = "a";
        string entranceDoorWindowSensorSecondCapabilityDataType = "ENUM";
        string entranceDoorWindowSensorSecondCapabilityRealName = "door";
        string entranceDoorWindowSensorSecondCapabilityParent = "entrance";
        string entranceDoorWindowSensorSecondCapabilityFullName = "entrance-door";
        string entranceDoorWindowSensorSecondCapabilityDesc = "Open / close the entrance door (Action)";
        ObjectOrServiceCapability entranceDoorWindowSensorSecondCapability = new ObjectOrServiceCapability(entranceDoorWindowSensorSecondCapabilityReferenceObjectID, entranceDoorWindowSensorSecondCapabilityName, entranceDoorWindowSensorSecondCapabilityType, entranceDoorWindowSensorSecondCapabilityDataType, entranceDoorWindowSensorSecondCapabilityRealName, entranceDoorWindowSensorSecondCapabilityParent, entranceDoorWindowSensorSecondCapabilityFullName, entranceDoorWindowSensorSecondCapabilityDesc);
        entranceDoorWindowSensorCapabilitiesList.Add(entranceDoorWindowSensorSecondCapability);
        singleObjectOrServiceCapability.Add(entranceDoorWindowSensorSecondCapabilityFullName, entranceDoorWindowSensorSecondCapability);
        
        objectsCapabilities.Add(entranceDoorWindowSensorSecondCapability.objectReferenceId, entranceDoorWindowSensorCapabilitiesList);



        
        List<ObjectOrServiceCapability> kitchenDoorWindowSensorCapabilitiesList = new List<ObjectOrServiceCapability>();

        int kitchenDoorWindowSensorFirstCapabilityReferenceObjectID = 15; // 
        string kitchenDoorWindowSensorFirstCapabilityName = "Kitchen Window";
        string kitchenDoorWindowSensorFirstCapabilityType = "t";
        string kitchenDoorWindowSensorFirstCapabilityDataType = "ENUM";
        string kitchenDoorWindowSensorFirstCapabilityRealName = "kitchenwindowsensor";
        string kitchenDoorWindowSensorFirstCapabilityParent = "kitchen";
        string kitchenDoorWindowSensorFirstCapabilityFullName = "kitchen-windowsensor";
        string kitchenDoorWindowSensorFirstCapabilityDesc = "Check the state of the kitchen window (Trigger)";
        ObjectOrServiceCapability kitchenDoorWindowSensorFirstCapability = new ObjectOrServiceCapability(kitchenDoorWindowSensorFirstCapabilityReferenceObjectID, kitchenDoorWindowSensorFirstCapabilityName, kitchenDoorWindowSensorFirstCapabilityType, kitchenDoorWindowSensorFirstCapabilityDataType, kitchenDoorWindowSensorFirstCapabilityRealName, kitchenDoorWindowSensorFirstCapabilityParent, kitchenDoorWindowSensorFirstCapabilityFullName, kitchenDoorWindowSensorFirstCapabilityDesc);
        kitchenDoorWindowSensorCapabilitiesList.Add(kitchenDoorWindowSensorFirstCapability);
        singleObjectOrServiceCapability.Add(kitchenDoorWindowSensorFirstCapabilityFullName, kitchenDoorWindowSensorFirstCapability);
        
        int kitchenDoorWindowSensorSecondCapabilityReferenceObjectID = 15; // 
        string kitchenDoorWindowSensorSecondCapabilityName = "Kitchen Window";
        string kitchenDoorWindowSensorSecondCapabilityType = "a";
        string kitchenDoorWindowSensorSecondCapabilityDataType = "ENUM";
        string kitchenDoorWindowSensorSecondCapabilityRealName = "kitchenwindow";
        string kitchenDoorWindowSensorSecondCapabilityParent = "kitchen";
        string kitchenDoorWindowSensorSecondCapabilityFullName = "kitchen-window";
        string kitchenDoorWindowSensorSecondCapabilityDesc = "Open / close the kitchen window (Action)";
        ObjectOrServiceCapability kitchenDoorWindowSensorSecondCapability = new ObjectOrServiceCapability(kitchenDoorWindowSensorSecondCapabilityReferenceObjectID, kitchenDoorWindowSensorSecondCapabilityName, kitchenDoorWindowSensorSecondCapabilityType, kitchenDoorWindowSensorSecondCapabilityDataType, kitchenDoorWindowSensorSecondCapabilityRealName, kitchenDoorWindowSensorSecondCapabilityParent, kitchenDoorWindowSensorSecondCapabilityFullName, kitchenDoorWindowSensorSecondCapabilityDesc);
        kitchenDoorWindowSensorCapabilitiesList.Add(kitchenDoorWindowSensorSecondCapability);
        singleObjectOrServiceCapability.Add(kitchenDoorWindowSensorSecondCapabilityFullName, kitchenDoorWindowSensorSecondCapability);
        
        objectsCapabilities.Add(kitchenDoorWindowSensorFirstCapability.objectReferenceId, kitchenDoorWindowSensorCapabilitiesList);




        
        List<ObjectOrServiceCapability> livingroomDoorWindowSensorCapabilitiesList = new List<ObjectOrServiceCapability>();

        int livingroomDoorWindowSensorFirstCapabilityReferenceObjectID = 16; // 
        string livingroomDoorWindowSensorFirstCapabilityName = "Living Room Window";
        string livingroomDoorWindowSensorFirstCapabilityType = "t";
        string livingroomDoorWindowSensorFirstCapabilityDataType = "ENUM";
        string livingroomDoorWindowSensorFirstCapabilityRealName = "livingroomwindowsensor";
        string livingroomDoorWindowSensorFirstCapabilityParent = "livingroom";
        string livingroomDoorWindowSensorFirstCapabilityFullName = "livingroom-windowsensor";
        string livingroomDoorWindowSensorFirstCapabilityDesc = "Check the state of the living room window (Trigger)";
        ObjectOrServiceCapability livingroomDoorWindowSensorFirstCapability = new ObjectOrServiceCapability(livingroomDoorWindowSensorFirstCapabilityReferenceObjectID, livingroomDoorWindowSensorFirstCapabilityName, livingroomDoorWindowSensorFirstCapabilityType, livingroomDoorWindowSensorFirstCapabilityDataType, livingroomDoorWindowSensorFirstCapabilityRealName, livingroomDoorWindowSensorFirstCapabilityParent, livingroomDoorWindowSensorFirstCapabilityFullName, livingroomDoorWindowSensorFirstCapabilityDesc);
        livingroomDoorWindowSensorCapabilitiesList.Add(livingroomDoorWindowSensorFirstCapability);
        singleObjectOrServiceCapability.Add(livingroomDoorWindowSensorFirstCapabilityFullName, livingroomDoorWindowSensorFirstCapability);
        
        int livingroomDoorWindowSensorSecondCapabilityReferenceObjectID = 16; // 
        string livingroomDoorWindowSensorSecondCapabilityName = "Living Room Window";
        string livingroomDoorWindowSensorSecondCapabilityType = "a";
        string livingroomDoorWindowSensorSecondCapabilityDataType = "ENUM";
        string livingroomDoorWindowSensorSecondCapabilityRealName = "livingroomwindow";
        string livingroomDoorWindowSensorSecondCapabilityParent = "livingroom";
        string livingroomDoorWindowSensorSecondCapabilityFullName = "livingroom-window";
        string livingroomDoorWindowSensorSecondCapabilityDesc = "Open / close the living room window (Action)";
        ObjectOrServiceCapability livingroomDoorWindowSensorSecondCapability = new ObjectOrServiceCapability(livingroomDoorWindowSensorSecondCapabilityReferenceObjectID, livingroomDoorWindowSensorSecondCapabilityName, livingroomDoorWindowSensorSecondCapabilityType, livingroomDoorWindowSensorSecondCapabilityDataType, livingroomDoorWindowSensorSecondCapabilityRealName, livingroomDoorWindowSensorSecondCapabilityParent, livingroomDoorWindowSensorSecondCapabilityFullName, livingroomDoorWindowSensorSecondCapabilityDesc);
        livingroomDoorWindowSensorCapabilitiesList.Add(livingroomDoorWindowSensorSecondCapability);
        singleObjectOrServiceCapability.Add(livingroomDoorWindowSensorSecondCapabilityFullName, livingroomDoorWindowSensorSecondCapability);
        
        objectsCapabilities.Add(livingroomDoorWindowSensorFirstCapability.objectReferenceId, livingroomDoorWindowSensorCapabilitiesList);
       
        


        List<ObjectOrServiceCapability> bedroomDoorWindowSensorCapabilitiesList = new List<ObjectOrServiceCapability>();

        int bedroomDoorWindowSensorFirstCapabilityReferenceObjectID = 19; // 
        string bedroomDoorWindowSensorFirstCapabilityName = "Bedroom Window ";
        string bedroomDoorWindowSensorFirstCapabilityType = "t";
        string bedroomDoorWindowSensorFirstCapabilityDataType = "ENUM";
        string bedroomDoorWindowSensorFirstCapabilityRealName = "bedroomwindowsensor";
        string bedroomDoorWindowSensorFirstCapabilityParent = "bedroom";
        string bedroomDoorWindowSensorFirstCapabilityFullName = "bedroom-windowsensor";
        string bedroomDoorWindowSensorFirstCapabilityDesc = "Check the state of the bedroom window (Trigger)";
        ObjectOrServiceCapability bedroomDoorWindowSensorFirstCapability = new ObjectOrServiceCapability(bedroomDoorWindowSensorFirstCapabilityReferenceObjectID, bedroomDoorWindowSensorFirstCapabilityName, bedroomDoorWindowSensorFirstCapabilityType, bedroomDoorWindowSensorFirstCapabilityDataType, bedroomDoorWindowSensorFirstCapabilityRealName, bedroomDoorWindowSensorFirstCapabilityParent, bedroomDoorWindowSensorFirstCapabilityFullName, bedroomDoorWindowSensorFirstCapabilityDesc);
        bedroomDoorWindowSensorCapabilitiesList.Add(bedroomDoorWindowSensorFirstCapability);
        singleObjectOrServiceCapability.Add(bedroomDoorWindowSensorFirstCapabilityFullName, bedroomDoorWindowSensorFirstCapability);
        
        int bedroomDoorWindowSensorSecondCapabilityReferenceObjectID = 19; // 
        string bedroomDoorWindowSensorSecondCapabilityName = "Bedroom Window ";
        string bedroomDoorWindowSensorSecondCapabilityType = "a";
        string bedroomDoorWindowSensorSecondCapabilityDataType = "ENUM";
        string bedroomDoorWindowSensorSecondCapabilityRealName = "bedroomwindow";
        string bedroomDoorWindowSensorSecondCapabilityParent = "bedroom";
        string bedroomDoorWindowSensorSecondCapabilityFullName = "bedroom-window";
        string bedroomDoorWindowSensorSecondCapabilityDesc = "Open / close the bedroom window (Action)";
        ObjectOrServiceCapability bedroomDoorWindowSensorSecondCapability = new ObjectOrServiceCapability(bedroomDoorWindowSensorSecondCapabilityReferenceObjectID, bedroomDoorWindowSensorSecondCapabilityName, bedroomDoorWindowSensorSecondCapabilityType, bedroomDoorWindowSensorSecondCapabilityDataType, bedroomDoorWindowSensorSecondCapabilityRealName, bedroomDoorWindowSensorSecondCapabilityParent, bedroomDoorWindowSensorSecondCapabilityFullName, bedroomDoorWindowSensorSecondCapabilityDesc);
        bedroomDoorWindowSensorCapabilitiesList.Add(bedroomDoorWindowSensorSecondCapability);
        singleObjectOrServiceCapability.Add(bedroomDoorWindowSensorSecondCapabilityFullName, bedroomDoorWindowSensorSecondCapability);
        
        objectsCapabilities.Add(bedroomDoorWindowSensorFirstCapability.objectReferenceId, bedroomDoorWindowSensorCapabilitiesList);
        

        // Bedroom hue sensor
        List<ObjectOrServiceCapability> bedroomHueSensorCapabilitiesList = new List<ObjectOrServiceCapability>();

        int bedroomHueSensorFirstCapabilityReferenceObjectID = 20;
        string bedroomHueSensorFirstCapabilityName = "Temperature Level Bedroom";
        string bedroomHueSensorFirstCapabilityType = "t";
        string bedroomHueSensorFirstCapabilityDataType = "INTEGER";
        string bedroomHueSensorFirstCapabilityRealName = "temperaturelevel";
        string bedroomHueSensorFirstCapabilityParent = "bedroom";
        string bedroomHueSensorFirstCapabilityFullName = "bedroom-temperaturelevel";
        string bedroomHueSensorFirstCapabilityDesc = "Check the temperature of the bedroom (Trigger)";
        ObjectOrServiceCapability bedroomHueSensorFirstCapability = new ObjectOrServiceCapability(bedroomHueSensorFirstCapabilityReferenceObjectID, bedroomHueSensorFirstCapabilityName, bedroomHueSensorFirstCapabilityType, bedroomHueSensorFirstCapabilityDataType, bedroomHueSensorFirstCapabilityRealName, bedroomHueSensorFirstCapabilityParent, bedroomHueSensorFirstCapabilityFullName, bedroomHueSensorFirstCapabilityDesc);
        bedroomHueSensorCapabilitiesList.Add(bedroomHueSensorFirstCapability);
        singleObjectOrServiceCapability.Add(bedroomHueSensorFirstCapabilityFullName, bedroomHueSensorFirstCapability);
        
        int bedroomHueSensorSecondCapabilityReferenceObjectID = 20;
        string bedroomHueSensorSecondCapabilityName = "Humidity Level Bedroom";
        string bedroomHueSensorSecondCapabilityType = "t";
        string bedroomHueSensorSecondCapabilityDataType = "INTEGER";
        string bedroomHueSensorSecondCapabilityRealName = "humiditylevel";
        string bedroomHueSensorSecondCapabilityParent = "bedroom";
        string bedroomHueSensorSecondCapabilityFullName = "bedroom-humiditylevel";
        string bedroomHueSensorSecondCapabilityDesc = "Check the humidity of the bedroom (Trigger)";
        ObjectOrServiceCapability bedroomHueSensorSecondCapability = new ObjectOrServiceCapability(bedroomHueSensorSecondCapabilityReferenceObjectID, bedroomHueSensorSecondCapabilityName, bedroomHueSensorSecondCapabilityType, bedroomHueSensorSecondCapabilityDataType, bedroomHueSensorSecondCapabilityRealName, bedroomHueSensorSecondCapabilityParent, bedroomHueSensorSecondCapabilityFullName, bedroomHueSensorSecondCapabilityDesc);
        bedroomHueSensorCapabilitiesList.Add(bedroomHueSensorSecondCapability);
        singleObjectOrServiceCapability.Add(bedroomHueSensorSecondCapabilityFullName, bedroomHueSensorSecondCapability);
        
        int bedroomHueSensorThirdCapabilityReferenceObjectID = 20; // 
        string bedroomHueSensorThirdCapabilityName = "Light Level Bedroom";
        string bedroomHueSensorThirdCapabilityType = "t";
        string bedroomHueSensorThirdCapabilityDataType = "ENUM";
        string bedroomHueSensorThirdCapabilityRealName = "lightlevel";
        string bedroomHueSensorThirdCapabilityParent = "bedroom";
        string bedroomHueSensorThirdCapabilityFullName = "bedroom-lightlevel";
        string bedroomHueSensorThirdCapabilityDesc = "Check the light level of the bedroom (Trigger)";
        ObjectOrServiceCapability bedroomHueSensorThirdCapability = new ObjectOrServiceCapability(bedroomHueSensorThirdCapabilityReferenceObjectID, bedroomHueSensorThirdCapabilityName, bedroomHueSensorThirdCapabilityType, bedroomHueSensorThirdCapabilityDataType, bedroomHueSensorThirdCapabilityRealName, bedroomHueSensorThirdCapabilityParent, bedroomHueSensorThirdCapabilityFullName, bedroomHueSensorThirdCapabilityDesc);
        bedroomHueSensorCapabilitiesList.Add(bedroomHueSensorThirdCapability);
        singleObjectOrServiceCapability.Add(bedroomHueSensorThirdCapabilityFullName, bedroomHueSensorThirdCapability);
        
        int bedroomHueSensorFourthCapabilityReferenceObjectID = 20; // 
        string bedroomHueSensorFourthCapabilityName = "Bedroom Motion";
        string bedroomHueSensorFourthCapabilityType = "t";
        string bedroomHueSensorFourthCapabilityDataType = "BOOLEAN";
        string bedroomHueSensorFourthCapabilityRealName = "motion";
        string bedroomHueSensorFourthCapabilityParent = "bedroom";
        string bedroomHueSensorFourthCapabilityFullName = "bedroom-motion";
        string bedroomHueSensorFourthCapabilityDesc = "Check if there is motion on the bedroom (Trigger)";
        ObjectOrServiceCapability bedroomHueSensorFourthCapability = new ObjectOrServiceCapability(bedroomHueSensorFourthCapabilityReferenceObjectID, bedroomHueSensorFourthCapabilityName, bedroomHueSensorFourthCapabilityType, bedroomHueSensorFourthCapabilityDataType, bedroomHueSensorFourthCapabilityRealName, bedroomHueSensorFourthCapabilityParent, bedroomHueSensorFourthCapabilityFullName, bedroomHueSensorFourthCapabilityDesc);
        bedroomHueSensorCapabilitiesList.Add(bedroomHueSensorFourthCapability);
        singleObjectOrServiceCapability.Add(bedroomHueSensorFourthCapabilityFullName, bedroomHueSensorFourthCapability);
        
        objectsCapabilities.Add(bedroomHueSensorFirstCapability.objectReferenceId, bedroomHueSensorCapabilitiesList);
       
        
        

        List<ObjectOrServiceCapability> bedroomHueLampCapabilitiesList = new List<ObjectOrServiceCapability>();
        
        int bedroomHueLampFirstCapabilityReferenceObjectID = 21;
        string bedroomHueLampFirstCapabilityName = "Hue Color Light Bedroom";
        string bedroomHueLampFirstCapabilityType = "a";
        string bedroomHueLampFirstCapabilityDataType = "ENUM";
        string bedroomHueLampFirstCapabilityRealName = "hue color light bedroom";
        string bedroomHueLampFirstCapabilityParent = "bedroom";
        string bedroomHueLampFirstCapabilityFullName = "bedroom-hue color light bedroom";
        string bedroomHueLampFirstCapabilityDesc = "Change the Bedroom Hue Lamp Color (Action)";
        ObjectOrServiceCapability bedroomHueLampFirstCapability = new ObjectOrServiceCapability(bedroomHueLampFirstCapabilityReferenceObjectID, bedroomHueLampFirstCapabilityName, bedroomHueLampFirstCapabilityType, bedroomHueLampFirstCapabilityDataType, bedroomHueLampFirstCapabilityRealName, bedroomHueLampFirstCapabilityParent, bedroomHueLampFirstCapabilityFullName, bedroomHueLampFirstCapabilityDesc);
        bedroomHueLampCapabilitiesList.Add(bedroomHueLampFirstCapability);
        singleObjectOrServiceCapability.Add(bedroomHueLampFirstCapabilityFullName, bedroomHueLampFirstCapability);
        
        int bedroomHueLampSecondCapabilityReferenceObjectID = 21;
        string bedroomHueLampSecondCapabilityName = "Bedroom Light State";
        string bedroomHueLampSecondCapabilityType = "t";
        string bedroomHueLampSecondCapabilityDataType = "ENUM";
        string bedroomHueLampSecondCapabilityRealName = "state";
        string bedroomHueLampSecondCapabilityParent = "bedroomlight";
        string bedroomHueLampSecondCapabilityFullName = "bedroomlight-state";
        string bedroomHueLampSecondCapabilityDesc = "Check the state of the Bedroom Hue Lamp Color (Trigger)";
        ObjectOrServiceCapability bedroomHueLampSecondCapability = new ObjectOrServiceCapability(bedroomHueLampSecondCapabilityReferenceObjectID, bedroomHueLampSecondCapabilityName, bedroomHueLampSecondCapabilityType, bedroomHueLampSecondCapabilityDataType, bedroomHueLampSecondCapabilityRealName, bedroomHueLampSecondCapabilityParent, bedroomHueLampSecondCapabilityFullName, bedroomHueLampSecondCapabilityDesc);
        bedroomHueLampCapabilitiesList.Add(bedroomHueLampSecondCapability);
        singleObjectOrServiceCapability.Add(bedroomHueLampSecondCapabilityFullName, bedroomHueLampSecondCapability);
        
        int bedroomHueLampThirdCapabilityReferenceObjectID = 21;
        string bedroomHueLampThirdCapabilityName = "Turn the Bedroom light";
        string bedroomHueLampThirdCapabilityType = "a";
        string bedroomHueLampThirdCapabilityDataType = "ENUM";
        string bedroomHueLampThirdCapabilityRealName = "alllight";
        string bedroomHueLampThirdCapabilityParent = "bedroom";
        string bedroomHueLampThirdCapabilityFullName = "bedroom-alllight";
        string bedroomHueLampThirdCapabilityDesc = "Turn ON / OFF the Bedroom Hue Lamp (Action)";
        ObjectOrServiceCapability bedroomHueLampThirdCapability = new ObjectOrServiceCapability(bedroomHueLampThirdCapabilityReferenceObjectID, bedroomHueLampThirdCapabilityName, bedroomHueLampThirdCapabilityType, bedroomHueLampThirdCapabilityDataType, bedroomHueLampThirdCapabilityRealName, bedroomHueLampThirdCapabilityParent, bedroomHueLampThirdCapabilityFullName, bedroomHueLampThirdCapabilityDesc);
        bedroomHueLampCapabilitiesList.Add(bedroomHueLampThirdCapability);
        singleObjectOrServiceCapability.Add(bedroomHueLampThirdCapabilityFullName, bedroomHueLampThirdCapability);
        
        objectsCapabilities.Add(bedroomHueLampFirstCapability.objectReferenceId, bedroomHueLampCapabilitiesList);
        
        
        List<ObjectOrServiceCapability> bedCapabilitiesList = new List<ObjectOrServiceCapability>();
        
        int bedFirstCapabilityReferenceObjectID = 22;
        string bedFirstCapabilityName = "Sleep Duration";
        string bedFirstCapabilityType = "t";
        string bedFirstCapabilityDataType = "INTEGER";
        string bedFirstCapabilityRealName = "sleepduration";
        string bedFirstCapabilityParent = "sleep";
        string bedFirstCapabilityFullName = "sleep-sleepduration";
        string bedFirstCapabilityDesc = "Check the sleep duration in minutes";
        ObjectOrServiceCapability bedFirstCapability = new ObjectOrServiceCapability(bedFirstCapabilityReferenceObjectID, bedFirstCapabilityName, bedFirstCapabilityType, bedFirstCapabilityDataType, bedFirstCapabilityRealName, bedFirstCapabilityParent, bedFirstCapabilityFullName, bedFirstCapabilityDesc);
        bedCapabilitiesList.Add(bedFirstCapability);
        singleObjectOrServiceCapability.Add(bedFirstCapabilityFullName, bedFirstCapability);
        
        int bedSecondCapabilityReferenceObjectID = 22;
        string bedSecondCapabilityName = "Bed Occupancy";
        string bedSecondCapabilityType = "t";
        string bedSecondCapabilityDataType = "ENUM";
        string bedSecondCapabilityRealName = "bedoccupancy";
        string bedSecondCapabilityParent = "sleep";
        string bedSecondCapabilityFullName = "sleep-bedoccupancy";
        string bedSecondCapabilityDesc = "Check the occupancy of the bed";
        ObjectOrServiceCapability bedSecondCapability = new ObjectOrServiceCapability(bedSecondCapabilityReferenceObjectID, bedSecondCapabilityName, bedSecondCapabilityType, bedSecondCapabilityDataType, bedSecondCapabilityRealName, bedSecondCapabilityParent, bedSecondCapabilityFullName, bedSecondCapabilityDesc);
        bedCapabilitiesList.Add(bedSecondCapability);
        singleObjectOrServiceCapability.Add(bedSecondCapabilityFullName, bedSecondCapability);
        
        objectsCapabilities.Add(bedFirstCapability.objectReferenceId, bedCapabilitiesList);
        */
        
    }
    
}

/**
 * Reference structure for an object
 */
public struct ActivableObject
{
    public string name;
    public string realName;
    public string referenceRoom; // just a the descriptive name used for visualization //
    public int referenceId;
    public Vector3 position;
    public Vector3 positionWrtApollo;

    public ActivableObject(string myName, string myRealName, string myReferenceRoom, int myReferenceId, Vector3 myPosition, Vector3 myPositionWrtApollo)
    {
        name = myName;
        realName = myRealName;
        referenceRoom = myReferenceRoom;
        referenceId = myReferenceId;
        position = myPosition;
        positionWrtApollo = myPositionWrtApollo;
    }
}



/**
 * Reference structure for a service 
 */
public struct ActivableService
{
    public string name;
    public string realName;
    public string parent;
    public int referenceId;

    public ActivableService(string myName, string myRealName, string myParent, int myReferenceId)
    {
        name = myName;
        realName = myRealName;
        parent = myParent;
        referenceId = myReferenceId;
    }
}

/**
 * Reference structure for a context element (object or service)
 */
public struct ObjectOrServiceCapability
{
    public int objectReferenceId;
    public string capabilityName;
    public string capabilityType; //Trigger / Action
    public string capabilityDataType; //BOOLEAN, INTEGER, DOUBLE, ENUM, TIME, DATE //For now the DOUBLE are used as INTEGER
    public string capabilityRealName;
    public string capabilityParent;
    public string capabilityFullName;
    public string capabilityDesc;

    public ObjectOrServiceCapability(int myObjectReferenceId, string myCapabilityName, string myCapabilityType, string myCapabilityDataType, string myCapabilityRealName, string myCapabilityParent, string myCapabilityFullName, string myCapabilityDesc)
    {
        objectReferenceId = myObjectReferenceId; //
        capabilityName = myCapabilityName;
        capabilityType = myCapabilityType;
        capabilityDataType = myCapabilityDataType;
        capabilityRealName = myCapabilityRealName;
        capabilityParent = myCapabilityParent;
        capabilityFullName = myCapabilityFullName;
        capabilityDesc = myCapabilityDesc;
    }
}

