using UnityEngine;
using UnityEngine.SceneManagement;

//This script was created by maxhusak.wordpress.com. If you have any feedback intend on using it, please contact me first.

// This script handles win and lose conditions for a game
// The player loses if they fall off the platform (y < -20)
// the win condition is met when the player reaches the finish line, marked by an object with the "Finish" tag
public class GameController : MonoBehaviour
{
    // reference to the player GameObject for position checking
    // this can be automatically set if the script is attached to the player object, or manually set in the inspector
    public GameObject player;
    // reference to the respawn point GameObject where the player will be respawned if they fall off the platform
    public Transform respawnPoint;

    // update is called once per frame to continuously check the player's position
    void Update()
    {
        // check if the player has fallen below a specified y-coordinate, indicating they have fallen off the platform
        if (player.transform.position.y < -20)
        {
            // if the player has fallen off, handle the lose condition by respawning the player
            handleLoseCondition();
        }
    }

    // called when the GameObject this script is attached to collides with another GameObject marked as a trigger
    private void OnTriggerEnter(Collider other)
    {
        // check if the object collided with has the tag "Finish"
        if (other.CompareTag("Finish"))
        {
            // if the collision is with the "Finish" object, handle the win condition
            handleWinCondition();
        }
    }

    // method to handle the lose condition by respawning the player at a predefined point
    void handleLoseCondition()
    {
        // reset the player's position to that of the respawn point
        player.transform.position = respawnPoint.position;
        // additional logic can be added here, such as resetting player state or game variables
    }

    // method to handle the win condition, which can be customized based on game requirements
    void handleWinCondition()
    {
        // log a message or implement win logic, such as loading a new level or showing a win screen
        Debug.Log("win condition met: player has reached the finish line");
        // example of loading a new scene upon winning - replace "NextLevelSceneName" with your actual scene name
        // SceneManager.LoadScene("NextLevelSceneName");
    }
}
