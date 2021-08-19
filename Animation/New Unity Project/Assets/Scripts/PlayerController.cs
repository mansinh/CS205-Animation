using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animController;
    CharacterController charController;
   
    bool isWalking = false;
    public float verticalSpeed;
    public float speed;
    float walkingSpeed = 0.5f;
    float runningSpeed = 1;
    
    float acceleration = 2;
    float braking = 6;

    public float turnSpeed = 50;
    public float baseSpeed=5;
    public float zoomSpeed = 400;
    public float jumpSpeed = 10;
    public float gravity = 10;

    public Vector3 direction = Vector3.zero;
    Vector3 facingDirection = Vector3.zero;

    Vector3 camPosition;
    float camDistance = -5;
    Vector2 mouse = new Vector2(0, 0);
    public Transform cameraPivot;

    void Start()
    {
        animController = GetComponent<Animator>();
        charController = GetComponent<CharacterController>();
        camPosition = new Vector3(0, 0, Camera.main.transform.localPosition.magnitude);
        isGrounded = charController.isGrounded;
    }

    bool isJumping = false;
    bool isGrounded = true;

    private void FixedUpdate()
    {

        // Apply gravity to player
        verticalSpeed -= gravity * Time.fixedDeltaTime;
        charController.Move(verticalSpeed * Time.fixedDeltaTime * Vector3.up);
        // Check if player is grounded
        isGrounded = charController.isGrounded;
        animController.SetBool("isGrounded", charController.isGrounded);


    }

    void Update()
    {
        MovementControl();
        CameraControl();
    }

    //*****************************************************************************************************
    // Move character by player input (jump, walk and run)
    //*****************************************************************************************************
    void MovementControl() {
        // Jump if the jump button pressed and character is on the ground
        if (Input.GetButton("Jump") && charController.isGrounded && !isJumping)
        {
            isJumping = true;
            StartCoroutine(Jump());
        }

        // Read direction from player input (WASD/directional keys/left joystick)
        direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // Walk if left shift is held
        isWalking = Input.GetKey(KeyCode.LeftShift);

        if (direction.sqrMagnitude > 0)
        {
            // If there is directional input, accelerate character to appropriate speed
            if (isWalking)
            {
                speed += acceleration * Time.deltaTime;
                speed = Mathf.Min(walkingSpeed, speed);
            }
            else
            {
                speed += acceleration * Time.deltaTime;
                speed = Mathf.Min(runningSpeed, speed);
            }

            // Rotate character facing direction towards input direction
            direction.Normalize();
            facingDirection = Vector3.RotateTowards(facingDirection, direction, 5 * Time.deltaTime, 0.1f);
            facingDirection.Normalize();

            // Transform direction to world space from local space
            direction = transform.TransformDirection(direction);

            // Apply movement to character
            charController.Move(Time.deltaTime * direction * speed * baseSpeed);


        }
        else if (speed > 0)
        {
            // If there is no directional input, slow down character to a stop while maintaining current facing direction
            speed -= braking * Time.deltaTime;
            speed = Mathf.Max(0, speed);
            facingDirection = Vector3.RotateTowards(facingDirection, Vector3.forward, 5 * Time.deltaTime, 0.1f);
        }

        // Apply movement direction to animations 
        // (added 2 to both axis as 3D freeform directional blend tree does not register negative values)
        animController.SetFloat("speedX", facingDirection.x * speed + 2);
        animController.SetFloat("speedZ", facingDirection.z * speed + 2);
    }

    // Apply vertical jump speed after some delay (build up jump animation)
    IEnumerator Jump()
    {
        animController.SetTrigger("jump");
        yield return new WaitForSeconds(0.2f);
        verticalSpeed = jumpSpeed;
        isJumping = false;
        charController.Move(verticalSpeed * Time.deltaTime * Vector3.up);
        animController.SetBool("isGrounded", charController.isGrounded);
    }


    //*****************************************************************************************************
    // Move camera by player input (pitch, zoom and rotate)
    //*****************************************************************************************************

    void CameraControl() {
        // Pitch control
        mouse.x += Input.GetAxis("Mouse X") * 5;
        mouse.y -= Input.GetAxis("Mouse Y") * 5;
        mouse.y = Mathf.Clamp(mouse.y, -5, 50);
        Quaternion camRotation = Quaternion.Euler(mouse.y, transform.eulerAngles.y, 0);

        // Zoom control
        if (Input.mouseScrollDelta.y != 0)
        {
            camDistance += Input.mouseScrollDelta.y * Time.deltaTime * zoomSpeed;
            camDistance = Mathf.Clamp(camDistance, -10, -5);
        }
        camPosition = new Vector3(0, 0, camDistance);

        // Apply controls to camera
        Camera.main.transform.position = cameraPivot.position + camRotation * camPosition;
        Camera.main.transform.LookAt(cameraPivot);


        float rotate = 0;
        // Rotate player to the right if mouse is right of player
        if (Input.mousePosition.x >= Camera.main.pixelWidth * 0.55)
        {
            rotate = turnSpeed * Time.deltaTime;
        }

        // Rotate player to the left if mouse is left of player
        if (Input.mousePosition.x <= Camera.main.pixelWidth * 0.45)
        {
            rotate = -turnSpeed * Time.deltaTime;
        }
        transform.RotateAround(transform.position, Vector3.up, rotate);

        // Shuffle feet animation if rotating on the spot
        if (speed == 0)
        {
            animController.SetFloat("turning", rotate);
        }
        else
        {
            animController.SetFloat("turning", 0);
        }
    }
}
