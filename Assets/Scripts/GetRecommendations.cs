using System.Collections;
using System.Collections.Generic;
using System.Net;
using System;
using System.IO;
using UnityEngine;


public class GetRecommendations : MonoBehaviour
{
    public TempRule tempRule;
    public AnchorCreator anchorCreator;
    public ContextData contextDataScript;
    public Utils utils;
    // Start is called before the first frame update
    void Start()
    {
        anchorCreator = FindObjectOfType<AnchorCreator>();  //
        contextDataScript = FindObjectOfType<ContextData>();  //
        utils = FindObjectOfType<Utils>();  //
        tempRule = FindObjectOfType<TempRule>();  
    }

    public void fireRecommendation()
    {
        ScreenLog.Log("fireRecommendation Start");
        int lastInsertedRuleElement = tempRule.getLastInsertedRuleElement();
        RuleElement lastInserted = tempRule.getRuleElementFromId(lastInsertedRuleElement);
        Vector3 userPosition = anchorCreator.getCameraPosition();
        string context = "y";
        //ScreenLog.Log("DATA FOR GETTING A REC:");
        //ScreenLog.Log(lastInserted.fullName);
        //ScreenLog.Log(lastInserted.nl);
        string nl = tempRule.getTempNl().ToLower(); // Horrible temporary solution / hack, the problem is that lastInsertedNl is not updated
        //ScreenLog.Log("BEFORE FILTER: " +nl);
        List<char> charsToRemove = new List<char>() { '@', '_', ',', '.' };
        nl = utils.stringFilter(nl, charsToRemove);
        //ScreenLog.Log("AFTER FILTER: " +nl);

        HttpWebRequest request =
         (HttpWebRequest)WebRequest.Create(String.Format("https://ar-recommender.onrender.com/query-example?nl={0}&capability={1}&x={2}&y={3}&z={4}&context={5}",
          nl, lastInserted.fullName, userPosition[0], userPosition[1], userPosition[2], context)); //
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        //ScreenLog.Log(jsonResponse);
        ScreenLog.Log("I GET A RESPONSE!!!");
        ScreenLog.Log(jsonResponse);
        tempRule.setRecommendations(jsonResponse);
        checkReceivedRecommendations(jsonResponse);
        ScreenLog.Log("fireRecommendation OK!");
    }

    /**
      * Check in the context server if the rec can be associated to a physical object
      * call to the method that place the recommendations related to a physical obj
      */
    public void checkReceivedRecommendations(string recs)
    {
        Response response = JsonUtility.FromJson<Response>(recs);
        //ScreenLog.Log(response.data.Count.ToString());
        int index = 0;
        foreach(SingleEntry entry in response.data)
        {
            /*
            ScreenLog.Log("A REC:");
            ScreenLog.Log(entry.completeName);
            ScreenLog.Log(entry.ECA);
            ScreenLog.Log(entry.myOperator);
            ScreenLog.Log(entry.value);
            ScreenLog.Log(entry.nextOperator);
            ScreenLog.Log("--------------");
            */
            string objectOrService = contextDataScript.isObjectOrServiceFromCapabilityFullName(entry.completeName);
            //ScreenLog.Log(objectOrService);
            if(objectOrService == "o")
            {
                int id = contextDataScript.getObjectOrServiceIdFromFullName(entry.completeName);
                /*
                ScreenLog.Log("IS OBJECT");
                ScreenLog.Log("RECOMMENDATION OBJECT ID: " + id);
                ScreenLog.Log("POSITION IN THE FOR LOOP: " + index);
                */
                anchorCreator.activateRecInfoPanel(id, entry, index);
            }
            index++;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
