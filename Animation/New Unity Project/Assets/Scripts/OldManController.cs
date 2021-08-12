using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldManController : MonoBehaviour
{
    Animator animController;
    bool isWalking = false;
    float speed;
    float walkingSpeed=0.5f;
    float runningSpeed=1;
    float acceleration = 2;
    float braking = 6;
    public float baseSpeed=5;
    public Vector3 direction = new Vector3(0,0 ,0);
    Vector3 currentDirection =  new Vector3(0, 0, 0);


    // Start is called before the first frame update
    void Start()
    {
        animController = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {


        direction = new Vector3(0, 0, 0);
        isWalking = Input.GetKey(KeyCode.LeftShift);
        animController.SetBool("isWalking", isWalking);


        

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
            direction.z++;
           
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
            direction.z--;
            
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
            direction.x--;
            
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
            direction.x++;
        }

        
     

        if (Input.GetKeyDown(KeyCode.Space)) {
            animController.SetTrigger("Jump");
            
        }

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
            direction = transform.TransformDirection(direction);

            currentDirection = Vector3.RotateTowards(currentDirection, direction, 5*Time.deltaTime, 0.1f);
            transform.position += Time.deltaTime * direction * speed* baseSpeed;
           
        }
        else if(speed>0){
            speed -= braking * Time.deltaTime;
            speed = Mathf.Max(0,speed); 
     
        }
        animController.SetFloat("speed", speed);
        animController.SetFloat("direction", Vector3.SignedAngle(currentDirection,transform.forward, Vector3.up));

        print(Input.mousePosition);
        float rotate = 0;
        if (Input.mousePosition.x >= 3*Camera.main.pixelWidth/4) {
            rotate = 200*Time.deltaTime;
        }
        if (Input.mousePosition.x <= Camera.main.pixelWidth/4)
        {
            rotate = -200*Time.deltaTime;
        }
        print(rotate);
        transform.RotateAround(transform.position,Vector3.up,rotate);
        
    }
}
