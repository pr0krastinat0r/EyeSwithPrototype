using UnityEngine;
using UnityEngine.UI;

//This script was created by maxhusak.wordpress.com. If you have any feedback or intend on using it, please contact me first.
//This is responsible for the visual representation of the player's sprint status in the UI
public class SprintUIController : MonoBehaviour
{
    // reference to the player controller to access sprint values
    public PlayerController playerController;
    // references to the UI Images that represent the sprint meter on screen
    public Image[] sprintBars; // ensure these are assigned in the inspector

    void Update()
    {
        // update the UI based on the current sprint status
        updateSprintUI();
    }

    // updates the sprint UI to reflect the current amount of sprint available
    void updateSprintUI()
    {
        // calculate the fraction of sprint remaining
        float sprintFraction = playerController.sprintRemaining / playerController.sprintDuration;

        // iterate over each sprint bar to update its visual state
        foreach (Image sprintBar in sprintBars)
        {
            // for a horizontal fill, we directly use the sprint fraction
            // for a vertical or radial fill, additional calculations may be necessary based on the fill method
            sprintBar.fillAmount = sprintFraction;
        }

        // this logic assumes a direct relationship between sprint amount and fill amount
        // if the UI involves movement or other effects, adjust the properties of the sprintBars accordingly
    }
}
