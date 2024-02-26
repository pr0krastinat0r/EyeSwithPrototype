using UnityEngine;

//This script was created by maxhusak.wordpress.com. If you have any feedback or intend on using it, please contact me first.
//It manages player movement, including walking, sprinting, and jumping, enhances gameplay with camera fov adjustments during sprinting for an immersive experience
public class PlayerController : MonoBehaviour
{
    // public variables for movement and camera control
    public float speed = 5.0f; // walking speed
    public float sprintSpeed = 10.0f; // speed while sprinting
    public float jumpForce = 7.0f; // force applied to create a jump
    public float mouseSensitivity = 100.0f; // sensitivity of mouse movement

    public float walkBobbingSpeed = 14f; // speed of the headbob
    public float walkBobbingAmount = 0.05f; // amount of headbob
    private float defaultPosY = 0; // default Y position of the camera
    private float timer = 0; // timer for the headbob effect

    private int jumpCount = 0; // tracks the number of jumps performed
    public int maxJump = 2; // allows for one extra jump (double jump)

    // sprint mechanics
    public float sprintDuration = 5.0f; // how long the player can sprint
    public float sprintRemaining; // tracks remaining sprint time
    public float sprintRecoveryRate = 1.0f; // how quickly sprint recovers

    // camera fov for sprinting
    public Camera playerCamera; // reference to the camera attached to the player
    public float normalFOV = 60.0f; // normal field of view
    public float sprintFOV = 75.0f; // field of view when sprinting
    public float fovChangeSpeed = 5.0f; // how quickly the fov changes to sprintFOV

    // xRotation stores the current vertical rotation of the camera to prevent it from flipping over
    private float xRotation = 0f;
    // cameraTransform should be assigned to the transform of the Camera component attached to the player
    public Transform cameraTransform;

    // private components
    private Rigidbody rb; // rigidbody component for physics interactions
    private bool isGrounded; // checks if the player is grounded to allow jumping

    void Start()
    {
        // initializing components and variables
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked; // hides and locks the cursor to the center of the screen
        sprintRemaining = sprintDuration; // sets sprint time to maximum at start
        playerCamera.fieldOfView = normalFOV; // sets the initial fov to normal
        // sets the initial orientation of the camera to ensure it's looking forward when the game starts
       // this orientation aligns the camera's forward direction with the player's forward movement direction
        cameraTransform.localRotation = Quaternion.Euler(0, 0, 0);

        defaultPosY = playerCamera.transform.localPosition.y; // Set the default Y position
    }

    void Update()
    {
        // input handling for player movement and actions
        movePlayer();
        mouseLook();

        applyHeadbobEffect();

        // check for jump input and if jump conditions are met
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJump)
        {
            jump();
        }

        // manages sprint usage and recovery
        manageSprint();
        // adjusts the camera's field of view based on sprinting
        changeFOV();
    }

    // handles horizontal and vertical player movement, including sprinting
    void movePlayer()
    {
        float currentSpeed = speed; // sets the current speed to the walking speed by default
        // if sprint key is held, player has remaining sprint time, and player is moving forward
        if (Input.GetKey(KeyCode.LeftShift) && sprintRemaining > 0 && Input.GetAxis("Vertical") > 0)
        {
            currentSpeed = sprintSpeed; // changes speed to sprint speed
            sprintRemaining -= Time.deltaTime; // depletes the remaining sprint time
        }
        else if (!Input.GetKey(KeyCode.LeftShift) && sprintRemaining < sprintDuration)
        {
            // recovers sprint time when the sprint key is not held
            sprintRemaining += Time.deltaTime * sprintRecoveryRate;
            sprintRemaining = Mathf.Clamp(sprintRemaining, 0, sprintDuration); // ensures sprint time does not exceed its max value
        }

        // calculates movement based on player input
        Vector3 movement = (transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical")) * currentSpeed;
        rb.MovePosition(transform.position + movement * Time.deltaTime); // applies movement to the player's position
    }

    // applyHeadbobEffect simulates a bobbing motion of the camera to mimic head movements while walking
    // this effect is only active when the player is moving without sprinting to enhance realism
    void applyHeadbobEffect()
    {
        // check for player movement without sprinting
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f)
        {
            if (!Input.GetKey(KeyCode.LeftShift)) // ensure the player is not sprinting
            {
                // calculate and apply the headbob position based on a sine wave
                timer += Time.deltaTime * walkBobbingSpeed;
                playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x,
                    defaultPosY + Mathf.Sin(timer) * walkBobbingAmount, playerCamera.transform.localPosition.z);
            }
        }
        else
        {
            // gradually reset the camera position to its default state when the player stops moving
            timer = 0;
            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x,
                Mathf.Lerp(playerCamera.transform.localPosition.y, defaultPosY, Time.deltaTime * walkBobbingSpeed),
                playerCamera.transform.localPosition.z);
        }
    }

    // adjusts the camera's field of view to simulate speed when sprinting
    void changeFOV()
    {
        float targetFOV = Input.GetKey(KeyCode.LeftShift) && sprintRemaining > 0 ? sprintFOV : normalFOV; // determines the target fov based on sprinting state
        // smoothly transitions between the current fov and the target fov
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, fovChangeSpeed * Time.deltaTime);
    }

    // applies an upward force to simulate jumping and handles double jump logic
    void jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // reset vertical velocity for consistent jump height
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        jumpCount++; // increment the jump counter
    }

    // manages the sprint duration and refill
    void manageSprint()
    {
        // use sprint if shift key is pressed and there's remaining sprint time
        if (Input.GetKey(KeyCode.LeftShift) && sprintRemaining > 0)
        {
            sprintRemaining -= Time.deltaTime; // decrement remaining sprint time
        }
        // refill sprint when not sprinting
        else if (!Input.GetKey(KeyCode.LeftShift) && sprintRemaining < sprintDuration)
        {
            sprintRemaining += Time.deltaTime * sprintRecoveryRate; // refill sprint
            sprintRemaining = Mathf.Clamp(sprintRemaining, 0, sprintDuration); // ensure sprint time does not exceed max duration
        }
    }

    void mouseLook()
    {
        // captures mouse movement along the x (horizontal) and y (vertical) axes and applies sensitivity scaling
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // adjusts the vertical angle based on mouse input, inverting the y input for natural up/down look
        // prevents the camera from rotating beyond vertical limits to avoid disorientation
        xRotation -= mouseY;
        // clamps the vertical rotation to just shy of directly upwards or downwards to maintain orientation
        xRotation = Mathf.Clamp(xRotation, -89f, 89f);

        // applies the vertical rotation to the camera alone, maintaining horizontal orientation with the player body
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        // applies horizontal rotation to the entire player, affecting both the camera and body
        transform.Rotate(Vector3.up * mouseX);
    }


    // checks for collisions to determine if the player is grounded
    void OnCollisionEnter(Collision collision)
    {
        // if the player collides with an object tagged as 'Ground'
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // sets the player as grounded, allowing them to jump again
            jumpCount = 0; //reset the double jump
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; // player is no longer grounded
        }
    }
}
