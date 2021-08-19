using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldManController : MonoBehaviour
{
    Animator animController;
    CharacterController charController;
   
    bool isWalking = false;
    float speed;
    float walkingSpeed=0.5f;
    float runningSpeed=1;
    float verticalSpeed;
    float acceleration = 2;
    float braking = 6;
    public float turnSpeed = 50;
    public float baseSpeed=5;
    public float zoomSpeed = 400;
    public float jumpSpeed = 10;
    public float gravity = 10;

    public Vector3 direction = Vector3.zero;
    Vector3 currentDirection = Vector3.zero;

    Vector3 camPosition;
    float camDistance = -5;
    Vector2 mouse = new Vector2(0, 0);
    public Transform cameraPivot;




    // Start is called before the first frame update
    void Start()
    {
        animController = GetComponent<Animator>();
        charController = GetComponent<CharacterController>();
        camPosition = new Vector3(0, 0, Camera.main.transform.localPosition.magnitude);
        isGrounded = charController.isGrounded;
    }
    bool isJumping = false;
    bool isLanding = false;
    bool isGrounded = true;
    private void FixedUpdate()
    {

       
        verticalSpeed -= gravity * Time.deltaTime;

        if (charController.isGrounded && isGrounded != charController.isGrounded && !isLanding) {
            isLanding = true;
            StartCoroutine(Land());
        }

        if (Input.GetButton("Jump") && charController.isGrounded && !isJumping && !isLanding)
        {
            isJumping = true;
            StartCoroutine(Jump());

        }
        isGrounded = charController.isGrounded;
        charController.Move(verticalSpeed * Time.deltaTime * Vector3.up);
        animController.SetBool("isGrounded", charController.isGrounded);


    }
    IEnumerator Land()
    {
        yield return new WaitForSeconds(0.2f);
        isLanding = false;
    }

    IEnumerator Jump() {
        animController.SetTrigger("jump");
        yield return new WaitForSeconds(0.1f);
        verticalSpeed = jumpSpeed;
        isJumping = false;
        charController.Move(verticalSpeed * Time.deltaTime * Vector3.up);
        animController.SetBool("isGrounded", charController.isGrounded);
    }

    // Update is called once per frame
    void Update()
    {


        direction = new Vector3(0, 0, 0);
        /*
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
            direction.z++;       
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
            direction.z--;         
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
            direction.x--;           
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)){
            direction.x++;
        }

        */
        direction = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));


        

        








        isWalking = Input.GetKey(KeyCode.LeftShift);
        if (direction.sqrMagnitude > 0)
        {
            


            if (isWalking)
            {
                speed += acceleration * Time.deltaTime;
                speed = Mathf.Min(walkingSpeed,speed);
               

            }
            else
            {
                speed += acceleration * Time.deltaTime;
                speed = Mathf.Min(runningSpeed, speed);
               

            }
            direction.Normalize();
            currentDirection = Vector3.RotateTowards(currentDirection, direction, 5 * Time.deltaTime, 0.1f);
            currentDirection.Normalize();
            direction = transform.TransformDirection(direction);

            charController.Move(Time.deltaTime * direction * speed * baseSpeed);
            //transform.position += ;

           
        }
        else if(speed>0){
            
            speed -= braking * Time.deltaTime;
            speed = Mathf.Max(0,speed);
            currentDirection = Vector3.RotateTowards(currentDirection, Vector3.forward, 5 * Time.deltaTime, 0.1f);
        }
        
        animController.SetFloat("speedX", currentDirection.x * speed+2);
        animController.SetFloat("speedZ", currentDirection.z * speed+2);

        //Control camera

        mouse.x += Input.GetAxis("Mouse X")*5;
        mouse.y -= Input.GetAxis("Mouse Y")*5;
        mouse.y = Mathf.Clamp(mouse.y,-5,50);
        Quaternion camRotation = Quaternion.Euler(mouse.y, transform.eulerAngles.y, 0);
        if (Input.mouseScrollDelta.y != 0)
        {

            camDistance += Input.mouseScrollDelta.y * Time.deltaTime * zoomSpeed;
            camDistance = Mathf.Clamp(camDistance, -10, -5);


        }
        camPosition = new Vector3(0, 0, camDistance);



        
        Camera.main.transform.position = cameraPivot.position + camRotation * camPosition;
        Camera.main.transform.LookAt(cameraPivot);
        float rotate = 0;


        if (Input.mousePosition.x >= Camera.main.pixelWidth*0.55) {
            rotate = turnSpeed*Time.deltaTime;
        }
        if (Input.mousePosition.x <= Camera.main.pixelWidth*0.45)
        {
            rotate = -turnSpeed*Time.deltaTime;
        }
        //print(rotate);
        transform.RotateAround(transform.position,Vector3.up,rotate);
        if (speed == 0)
        {
            animController.SetFloat("turning", rotate);
        }
        else {
            animController.SetFloat("turning", 0);
        }


      


    }


}
