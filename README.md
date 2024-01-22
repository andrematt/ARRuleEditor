# ARRuleEditor

**A solution that use Augmented Reality to create and modify automation rules in a smart environment**

*Incorporate object tracking and conventional 2D object detection.*

The project is based on [Unity Detection 2AR](https://github.com/derenlei/Unity_Detection2AR)

## Pre: Java and Android SDK

Unity Android configuration (edit => preferences => external tools):

Java JDK 1.8.0_311: https://www.oracle.com/it/java/technologies/javase/javase8u211-later-archive-downloads.html#license-lightbox and then "Java SE Development Kit 8u311"

Android SDK 30.0.3: from Android Studio, download it from tools => SDK manager => SDK tools => Android SDK build-tools32 => show package details. Make sure that there are no more recent versions also installed (e.g. if the 31.0.0 is also installed it gives errors).

Android NDK r19: https://dl.google.com/android/repository/android-ndk-r19-windows-x86_64.zip

Also check in Android Studio if Android 10 is installed: tools => SDK Manager => Android 10.0(Q)

*note:* If you don't use Android Studio, the Sdk, build tools and USB utilities can be downloaded from the Android 
[Command Line Tool](https://developer.android.com/studio#cmdline-tools)


## How to Build

**Note:** to collaborate on the repo, do not directly download or clone the project, 
but follow this steps instead
[collaboration workflow](https://medium.com/@williamschinonso11/understanding-git-2-collaboration-workflow-best-practices-dos-and-don-ts-1424c3f02947)

Download the project and place it in a directory of choice or clone it with 
`git clone https://gitea-s2i2s.isti.cnr.it/Human_in_Information_Systems_HIIS/ARRuleEditor.git`

Open Unity Hub, click "add", search the project root directory and select it, click on the project to resolve packages and open it in Unity. 

**Warning**: Unity will ask to enter in safe mode because there are compilation errors. Ignore it for now: it is a conflict between Barracuda and ArFoundation extension on the Protobuf file. Just click on "ignore". 

When the project is loaded the first thing to do is to setup the build for Android. Go to file, build settings, android, switch platform. After the reloading close the "build settings" panel (do not click build or build and run). 

You should see on the left of the screen a "hierarchy" panel. Delete the Main Camera and the Directional light. Then, click on the "Scenes" directory under the "Assets" root on the lower part of the screen and double click on "SampleScene.unity". This will load our custom scene. 

Now select the display size: click the "Game" label (centre of the screen, next to "scene") and select "2960x1440 portrait"

Then, go back to the operating system, open the "Library/PackageCache/com.google.ar.core.arfoundation.extensions@0b8e217fb0/Editor/Scripts/Internal/Analytics" path in the project root and delete "Google.Protobuf.dll". Go back to unity: the project will recompile without errors. 

If you try to build now, it will give the error "Unable to sign the application; please provide passwords!". You need to disable the signing for the app: go to file, build settings, player settings, publishing settings and deselect the "project keystore" check, close the  "player settings" panel and click "build and run" on the "build settings" panel.

If the application starts but you can't see the buttons, close unity and open it again (I think that is the old scene still displayed).


## APP Info

*Please keep in mind that the code is not clean and there are a lot of TODOs around.*

The application is based on 2 main functionalities:
- Save Object Location: used to detect objects in the real world and save their position. The position can be used by the rule editor.
- AR rule editor: allow for the selection of objects with augmented capabilities into the environment, and for enabling the different editor functionalities. The rule editor does not have to detect objects, because their location and type is already provided.

For the moment, the Save Object Location is not used, because *first we want to focus on defining the AR rule editor capabilities in a "static", beforehand defined environment*. 
Most of the functionalities of Save Object Location are in the Scripts/PhoneARCamera.cs and Scripts/ArPlaceTrackedImages.cs classes. 
Other related scripts are in the Scripts/Detector/ directory. 
Scripts/SaveSerial.cs and Scripts/SerializeStruct.cs are used to save to file the position and info of the detected object (are just stubs for the moment). 
Also, Scripts/NewElementScript.cs and Scripts/EditElementScript.cs (and the related NewElementCanvas and EditElementCanvas GameObjects) will be used to save information about the position of an object, but for the moment they are stubs.

The functionalities of AR Rule Editor are rooted in the "CubePanel" prefab and related script (Assets/Prefabs/CubePanel) and on the Scripts/AnchorCreator.cs class.  

The anchorCreator class have many functionalities:
- Manages the user input (raytracing) and their effect on the environment (calling methods of the "tapped" CubePanels)
- Define the static AR Objects that will be used (in the Awake method) 
- Instantiate these CubePanel and anchor them (associate their game object to a position in the real world) 

The AnchorCreator class is associated with the "AR Session Origin" Game object (see the Andreas Jakl tutorial in the links below). 

The association between the CubePanel anchor prefab and the AnchorCreator script is made using the Unity UI. 
You can see this association in the Unity Inspector on the right of the screen: click on the AR Session Origin on the Hierarchy, then search for "Anchor Creator (Script)". 
Using the Inspector you can e.g. change the associated prefab, or assign a script to a game object (click Add Component - New Script).

That "CubePanel" is the `public GameObject _anchorPrefab` that you can see in the AnchorCreator script: unity makes the connection with the CubePanel autonomously. 

This class have also methods used by the Save Object Location functionality (e.g. CreateAnchor).

## Debugging 

If you can't copy the APK on the device, check [these answers](https://android.stackexchange.com/questions/101933/samsung-galaxy-s4-does-not-show-authorize-usb-debugging-dialog-box)

To enable debugging from Visual Studio, see [Unity manual](https://docs.unity3d.com/Manual/ManagedCodeDebugging.html).
If you can't see the Android device on the visual studio debugger, check [these troubleshooting steps](https://docs.unity3d.com/2017.3/Documentation/Manual/TroubleShootingAndroid.html)
Also make sure that the "Preferred Android Sdk Root" that is setted in Tools -> options -> tools for Unity -> general links
to the same Sdk dir used by Unity, see [this post on Unity forum](https://forum.unity.com/threads/cannot-debug-on-android-device-from-visual-studio.1015027/).


## Useful resources on Unity and augmented reality

[https://docs.unity3d.com/Manual/VisualStudioIntegration.html](https://docs.unity3d.com/Manual/VisualStudioIntegration.html)
Visual studio - Unity integration

[http://dotween.demigiant.com/](http://dotween.demigiant.com/)
Library to animate game objects

[https://catlikecoding.com/unity/tutorials/basics/game-objects-and-scripts/](https://catlikecoding.com/unity/tutorials/basics/game-objects-and-scripts/)
Unity tutorial

[https://www.andreasjakl.com/ar-foundation-fundamentals-with-unity-part-1/](https://www.andreasjakl.com/ar-foundation-fundamentals-with-unity-part-1/)
how to setup an augmented reality application in unity using Arfoundation. The application uses raycast to place 3d objects into the environment.

[https://github.com/derenlei/Unity_Detection2AR](https://github.com/derenlei/Unity_Detection2AR)
object detection + augmented reality. Is the code that we used as a base for our application.

[https://developers.google.com/ar/devices](https://developers.google.com/ar/devices)
list of devices supported by ARCore (unity ARFoundation is also based on ARCore, so the device have to be compatible)

[https://answers.unity.com/questions/46124/what-is-the-difference-between-a-prefab-and-a-game.html](https://answers.unity.com/questions/46124/what-is-the-difference-between-a-prefab-and-a-game.html)
Difference between a game object (an object in the "hierarchy" in Unity) and a prefab (a "component" found in the Assets/Prefabs directory)

[https://json2csharp.com/code-converters/csharp-object-initializer](https://json2csharp.com/code-converters/csharp-object-initializer)
JSON object initializer: from a JSON structure, generates the classes needed to read it with JsonUtility

[https://www.red-gate.com/simple-talk/development/dotnet-development/calling-restful-apis-unity3d/](https://www.red-gate.com/simple-talk/development/dotnet-development/calling-restful-apis-unity3d/)
Networking in Unity
