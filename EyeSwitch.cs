using System.Collections.Generic;
using UnityEngine;

//This script was created by maxhusak.wordpress.com. If you have any feedback intend on using it, please contact me first.

// Manages the visibility of objects based on the 'eye switch' mechanic
// Allows for toggling between two sets of objects to simulate changing perspectives
public class EyeSwitch : MonoBehaviour
{
    // holds objects visible with the first 'eye'
    public List<GameObject> firstEyeObjects = new List<GameObject>();
    // holds objects visible with the second 'eye'
    public List<GameObject> secondEyeObjects = new List<GameObject>();

    // references to the materials for each of the skyboxes corresponding to the eyes
    public Material skyboxForFirstEye;
    public Material skyboxForSecondEye;

    // called once per frame, checks for input to switch 'eyes'
    void Update()
    {
        // check for left mouse button press to activate the first set of objects
        if (Input.GetMouseButtonDown(0)) // 0 is the left mouse button
        {
            // make first set of objects visible and second set invisible
            setObjectsVisibility(firstEyeObjects, true);
            setObjectsVisibility(secondEyeObjects, false);

            // calls the method to switch to the first eye's view and skybox
            SwitchToFirstEye();
        }
        // check for right mouse button press to activate the second set of objects
        else if (Input.GetMouseButtonDown(1)) // 1 is the right mouse button
        {
            // make second set of objects visible and first set invisible
            setObjectsVisibility(firstEyeObjects, false);
            setObjectsVisibility(secondEyeObjects, true);

            // calls the method to switch to the second eye's view and skybox
            SwitchToSecondEye();
        }
    }

    // toggles the visibility of objects in a list based on the isVisible flag
    void setObjectsVisibility(List<GameObject> objects, bool isVisible)
    {
        // iterate through each object in the provided list
        foreach (GameObject obj in objects)
        {
            // check if the object has a MeshRenderer component
            if (obj.GetComponent<MeshRenderer>() != null)
            {
                // set the object's visibility based on the provided flag
                obj.GetComponent<MeshRenderer>().enabled = isVisible;
            }
        }
    }

    // switches to the first eye's view and updates the skybox to the first eye's skybox
    void SwitchToFirstEye()
    {
        // logic for activating first eye's view can be added here
        // updates the skybox to the first eye's skybox
        RenderSettings.skybox = skyboxForFirstEye;
        // ensures the changes to the skybox are immediately visible
        DynamicGI.UpdateEnvironment();
    }

    // switches to the second eye's view and updates the skybox to the second eye's skybox
    void SwitchToSecondEye()
    {
        // logic for activating second eye's view can be added here
        // updates the skybox to the second eye's skybox
        RenderSettings.skybox = skyboxForSecondEye;
        // ensures the changes to the skybox are immediately visible
        DynamicGI.UpdateEnvironment();
    }
}
