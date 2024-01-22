using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/*
 * Manages the instantiated Apollo QR code and call methods in AnchorCreator when 
 * an "apollo" image is tracked. 
 * Now we are using a static placing of AR viz, so this class is not used. 
 */

[RequireComponent(typeof(ARTrackedImageManager))]

public class ARPlaceTrackedImages : MonoBehaviour
{
    // Cache AR tracked images manager from ARCoreSession
    private ARTrackedImageManager _trackedImagesManager;

    // List of prefabs - these have to have the same names as the 2D images in the reference image library
    public GameObject[] ArPrefabs;

    // Internal storage of created prefabs for easier updating
    private readonly Dictionary<string, GameObject> _instantiatedPrefabs = new Dictionary<string, GameObject>();





    void Awake()
    {
        _trackedImagesManager = GetComponent<ARTrackedImageManager>();
        //_trackedImagesManager.referenceLibrary = myLibrary;
        _trackedImagesManager.enabled = true;
        anchorCreator = FindObjectOfType<AnchorCreator>();
    }

    void OnEnable()
    {
        _trackedImagesManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        _trackedImagesManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }
    void ImageManagerOnTrackedImagesChanged(ARTrackedImagesChangedEventArgs obj)
    {

    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // Good reference: https://forum.unity.com/threads/arfoundation-2-image-tracking-with-many-ref-images-and-many-objects.680518/#post-4668326
        // https://github.com/Unity-Technologies/arfoundation-samples/issues/261#issuecomment-555618182
        // how to save the position of an object when the state become "success"?
        // https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/ImageTracking/Scripts/ImageTrackingObjectManager.cs

        if (tracking)
        {
            foreach (ARTrackedImage image in eventArgs.updated)
            {
                // image is tracking or tracking with limited state, show visuals and update it's position and rotation
                if (image.trackingState == TrackingState.Tracking)
                {
                    trackedImageTransform = image.transform;
                    //ScreenLog.Log(trackedImageTransform.position[0].ToString()+trackedImageTransform.position[1].ToString()+ trackedImageTransform.position[2].ToString());
                    var imageName = image.referenceImage.name;
                    //ScreenLog.Log("GET IMAGE: " + imageName);
                    if (imageName == "apollo")
                    {
                        // There is anything else than localPostion/position? because these does not get the object position, but the camera position
                        anchorCreator.resetObjectsPositionWrtApollo(trackedImageTransform);
                        tracking = false;
                        StartCoroutine(WaitToRestartTracking());
                    }
                }
            }
        }
    }

    // From the unity docs! https://docs.unity3d.com/ScriptReference/WaitForSeconds.html
    IEnumerator WaitToRestartTracking()
    {
        //Print the time of when the function is first called.
        ScreenLog.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(2.5f);

        //After we have waited 5 seconds print the time again.
        ScreenLog.Log("Finished Coroutine at timestamp : " + Time.time);
        tracking = true;
    }


    public void setInstantiatedApollo(bool myBool)
    {
        instantiatedApollo = myBool;
    }

    public Boolean getInstantiatedApollo()
    {
        ScreenLog.Log("Getting instantiated Apollo: " + instantiatedApollo);
        return instantiatedApollo;
    }

    public Transform getApolloTransform()
    {
        return apolloTransform;
    }

    public void setApolloTransform(Transform transform)
    {
        apolloTransform = transform;
    }

    public AnchorCreator anchorCreator;
    public Transform apolloTransform;
    public GameObject _anchorPrefab;
    public Transform trackedImageTransform;
    public bool instantiatedApollo = false;
    private bool tracking = true;
}