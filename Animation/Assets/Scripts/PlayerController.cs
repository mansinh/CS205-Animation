using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animController;
    CharacterController charController;
   
    bool isWalking = true;
    public float verticalSpeed;
    public float speed;
    float targetSpeed;
    float walkingSpeed = 0.5f;
    float runningSpeed = 1;
    
    [SerializeField] float acceleration = 2;
    [SerializeField] float braking = 3;

    [SerializeField] float turnSpeed = 500;
    [SerializeField] float baseSpeed=5;
    [SerializeField] float zoomSpeed = 400;
    [SerializeField] float jumpSpeed = 10;
    [SerializeField] float gravity = 10;

    [SerializeField] Vector3 direction = Vector3.zero;
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

    public bool isJumping = false;
    public bool isLanding = false;
    bool isGrounded = true;

    private void FixedUpdate()
    {
        // Apply gravity to player
        verticalSpeed -= gravity * Time.fixedDeltaTime;
        charController.Move(verticalSpeed * Time.fixedDeltaTime * Vector3.up);


        if (charController.isGrounded && !isGrounded) {
            StartCoroutine(Land());
        }
        // Check if player is grounded
        isGrounded = charController.isGrounded;
        

    }

    void Update()
    {
        if (Time.timeScale > 0)
        {
            MovementControl();
            CameraControl();
        }
    }

    //*****************************************************************************************************
    // Move character by player input (jump, walk and run)
    //*****************************************************************************************************
    void MovementControl() {
        // Jump if the jump button pressed and character is on the ground
        if (Input.GetButton("Jump") && charController.isGrounded && !isJumping && !isLanding)
        {
            isJumping = true;
            StartCoroutine(Jump());
        }

        // Read direction from player input (WASD/directional keys/left joystick)
        direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // Walk if left shift is held
        isWalking = !Input.GetKey(KeyCode.LeftShift);

        if (direction.sqrMagnitude > 0)
        {
            // If there is directional input, accelerate character to appropriate speed
            if (isWalking)
            {
                 targetSpeed = walkingSpeed;
            }
            else
            {   
                targetSpeed = runningSpeed;
            }

            speed += acceleration*(targetSpeed-speed) * Time.deltaTime;

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
        animController.SetFloat("speed", speed);
    }

    // Apply vertical jump speed after some delay (build up jump animation)
    IEnumerator Jump()
    {
        animController.SetTrigger("jump");
        verticalSpeed = 0;
        yield return new WaitForSeconds(0.3f);
        verticalSpeed = jumpSpeed;
        
        charController.Move(verticalSpeed * Time.deltaTime * Vector3.up);
       
    }

    // Triggers when landing
    IEnumerator Land()
    {
        animController.SetBool("isLanding", true);
        isLanding = true;
        isJumping = false;
        yield return new WaitForSeconds(0.3f);    
        isLanding = false;
        animController.SetBool("isLanding", false);
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
        float fromCenter = Input.mousePosition.x/ Camera.main.pixelWidth - 0.5f;
        // Rotate character and camera if mouse is left or right of character
        if (fromCenter*fromCenter > 0.05*0.05)
        {
            rotate = turnSpeed*fromCenter * Time.deltaTime;
        }   
        transform.RotateAround(transform.position, Vector3.up, rotate);
        Vector3 e = transform.eulerAngles;
        transform.eulerAngles = new Vector3(e.x,e.y,-fromCenter*speed*speed*20);

        // Shuffle feet animation if rotating on the spot
        
            animController.SetFloat("turning", Input.mousePosition.x / Camera.main.pixelWidth);
        
        
    }
}
